param(
    [string]$CacheDir,
    [string]$Uuid = "6877b37c-e9cd-4826-b82c-5e8d3d5db744",
    [switch]$NoPin
)

$ErrorActionPreference = "Stop"

function To-FileUrl([string]$Path) {
    $full = [System.IO.Path]::GetFullPath($Path) -replace '\\', '/'
    "file:///$full"
}

function From-FileUrl([string]$Url) {
    if ($Url -notlike "file:///*") { return $null }
    ($Url -replace "^file:///", "") -replace "/", "\"
}

function Get-LauncherConfig {
    $path = Join-Path $env:APPDATA "OpenFusionLauncher\config.json"
    if (-not (Test-Path $path)) { return $null }
    Get-Content $path -Raw | ConvertFrom-Json
}

function Get-LauncherInstallDir {
    try {
        $key = "HKCU:\Software\OpenFusionLauncher\OpenFusionLauncher"
        if (Test-Path $key) {
            $value = (Get-ItemProperty $key -ErrorAction Stop).'(default)'
            if ($value) { return [System.IO.Path]::GetFullPath($value) }
        }
    } catch { }
    Join-Path $env:LOCALAPPDATA "OpenFusionLauncher"
}

function Get-CacheRoots([object]$Config, [string]$InstallDir) {
    $gameRoot = Join-Path $env:LOCALAPPDATA "OpenFusionLauncher\ffcache"
    $offlineRoot = Join-Path $InstallDir "offline_cache"

    if ($Config -and $Config.launcher) {
        if ($Config.launcher.game_cache_path) {
            $gameRoot = $Config.launcher.game_cache_path
        }
        if ($Config.launcher.offline_cache_path) {
            $offlineRoot = $Config.launcher.offline_cache_path
        }
    }

    @{
        GameCacheDir = [System.IO.Path]::GetFullPath((Join-Path $gameRoot $Uuid))
        OfflineCacheDir = [System.IO.Path]::GetFullPath((Join-Path $offlineRoot $Uuid))
        GameCacheRoot = [System.IO.Path]::GetFullPath($gameRoot)
        OfflineCacheRoot = [System.IO.Path]::GetFullPath($offlineRoot)
    }
}

function Resolve-OfflineCacheDir {
    param(
        [string]$ExplicitDir,
        [hashtable]$Roots,
        [string]$ManifestPath
    )

    if ($ExplicitDir) {
        return [System.IO.Path]::GetFullPath($ExplicitDir)
    }

    $candidates = New-Object System.Collections.Generic.List[string]

    if (Test-Path $Roots.OfflineCacheDir) { $candidates.Add($Roots.OfflineCacheDir) }
    if (Test-Path $Roots.GameCacheDir) { $candidates.Add($Roots.GameCacheDir) }

    if (Test-Path $ManifestPath) {
        $manifest = Get-Content $ManifestPath -Raw | ConvertFrom-Json
        if ($manifest.main_file_url -like "file:///*") {
            $fromUrl = From-FileUrl $manifest.main_file_url
            if ($fromUrl) {
                if ($fromUrl -like "*\main.unity3d") {
                    $candidates.Add((Split-Path $fromUrl -Parent))
                } else {
                    $candidates.Add($fromUrl)
                }
            }
        }
    }

    foreach ($dir in ($candidates | Select-Object -Unique)) {
        if (Test-Path (Join-Path $dir "main.unity3d")) {
            return $dir
        }
    }

    if (Test-Path $Roots.OfflineCacheDir) { return $Roots.OfflineCacheDir }
    $Roots.GameCacheDir
}

function Ensure-Bootstrap([string]$Dir) {
    $baseUri = (To-FileUrl $Dir).TrimEnd('/') + "/"
    $assetInfo = Join-Path $Dir "assetInfo.php"
    $rankUrl = Join-Path $Dir "rankUrl.txt"
    Set-Content -Path $assetInfo -Value $baseUri -Encoding ASCII -NoNewline
    if (-not (Test-Path $rankUrl)) {
        Set-Content -Path $rankUrl -Value "http://api.retrobution.xyz/getranks" -Encoding ASCII -NoNewline
    }
}

$config = Get-LauncherConfig
$installDir = Get-LauncherInstallDir
$roots = Get-CacheRoots $config $installDir
$manifest = Join-Path $env:APPDATA "OpenFusionLauncher\versions\$Uuid.json"

if (-not (Test-Path $manifest)) {
    Write-Error "Launcher manifest not found: $manifest`nLaunch the game once so the launcher creates it."
}

$cacheDir = Resolve-OfflineCacheDir -ExplicitDir $CacheDir -Roots $roots -ManifestPath $manifest
$bundle = Join-Path $cacheDir "main.unity3d"
$ffcacheDir = $roots.GameCacheDir

if (-not (Test-Path $bundle)) {
    Write-Host "Offline cache root: $($roots.OfflineCacheRoot)"
    Write-Host "Game cache root:    $($roots.GameCacheRoot)"
    Write-Error "main.unity3d not found.`nCopy the patched bundle here first:`n  $($roots.OfflineCacheDir)\main.unity3d`nThen run this script again."
}

Write-Host "Using cache folder: $cacheDir"
if ($config -and $config.launcher.offline_cache_path) {
    Write-Host "From launcher config offline_cache_path: $($config.launcher.offline_cache_path)"
}

$hash = (Get-FileHash $bundle -Algorithm SHA256).Hash.ToLower()
$size = (Get-Item $bundle).Length
$fileUrl = To-FileUrl $bundle

attrib -R $manifest 2>$null

$j = Get-Content $manifest -Raw | ConvertFrom-Json
Write-Host "Before: url=$($j.main_file_url)"
Write-Host "Before: hash=$($j.main_file_info.hash) size=$($j.main_file_info.size)"

$j.main_file_url = $fileUrl
$j.main_file_info.hash = $hash
$j.main_file_info.size = $size
$j | ConvertTo-Json -Depth 100 | Set-Content $manifest -Encoding UTF8

Write-Host "After:  url=$fileUrl"
Write-Host "After:  hash=$hash size=$size"

Ensure-Bootstrap $cacheDir

New-Item -ItemType Directory -Force -Path $ffcacheDir | Out-Null
foreach ($name in @("main.unity3d", "assetInfo.php", "rankUrl.txt")) {
    Copy-Item (Join-Path $cacheDir $name) (Join-Path $ffcacheDir $name) -Force
    Write-Host "Synced $name -> $ffcacheDir"
}

if (-not $NoPin) {
    attrib +R $manifest
    Write-Host "Manifest pinned (read-only)."
}

Write-Host ""
Write-Host "Done. Quit the launcher if it is open, reopen, and Connect."
