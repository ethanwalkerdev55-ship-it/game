param(
    [Parameter(Mandatory = $true)][string]$Bundle,
    [Parameter(Mandatory = $true)][string]$Manifest,
    [switch]$Pin
)

$fail = $false
function Fail([string]$msg) {
    Write-Host "FAIL: $msg"
    $script:fail = $true
}

$bundle = [System.IO.Path]::GetFullPath($Bundle)
$offlineDir = Split-Path $bundle -Parent
$uuid = Split-Path $offlineDir -Leaf
$ffcacheDir = Join-Path $env:LOCALAPPDATA "OpenFusionLauncher\ffcache\$uuid"

if (-not (Test-Path $bundle)) {
    Fail "bundle missing: $bundle"
    exit 1
}

$fixScript = Join-Path $PSScriptRoot "fix-launcher-manifest.ps1"
$bootstrapScript = Join-Path $PSScriptRoot "ensure-offline-bootstrap.ps1"
$preflightScript = Join-Path $PSScriptRoot "bootstrap-preflight.ps1"

if ($Pin) {
    & $fixScript -Bundle $bundle -Manifest $Manifest -Pin
} else {
    & $fixScript -Bundle $bundle -Manifest $Manifest
}
if ($LASTEXITCODE -ne 0) { exit 1 }

& $bootstrapScript -OfflineCacheDir $offlineDir
if ($LASTEXITCODE -ne 0) { exit 1 }

New-Item -ItemType Directory -Force -Path $ffcacheDir | Out-Null

$bootstrapFiles = @("assetInfo.php", "rankUrl.txt")
foreach ($name in $bootstrapFiles) {
    $src = Join-Path $offlineDir $name
    if (-not (Test-Path $src)) {
        Fail "$name missing in offline cache"
        continue
    }
    Copy-Item $src (Join-Path $ffcacheDir $name) -Force
    Write-Host "Synced $name -> ffcache"
}

Copy-Item $bundle (Join-Path $ffcacheDir "main.unity3d") -Force
Write-Host "Synced main.unity3d -> ffcache ($((Get-Item $bundle).Length) bytes)"

& $preflightScript -Bundle $bundle -Manifest $Manifest -OfflineCacheDir $offlineDir
if ($LASTEXITCODE -ne 0) { exit 1 }

foreach ($name in $bootstrapFiles) {
    if (-not (Test-Path (Join-Path $ffcacheDir $name))) {
        Fail "$name missing in ffcache after sync"
    }
}
if (-not (Test-Path (Join-Path $ffcacheDir "main.unity3d"))) {
    Fail "main.unity3d missing in ffcache after sync"
}

if ($fail) {
    Write-Host "sync-patch-cache: BLOCKED"
    exit 1
}

Write-Host "sync-patch-cache: OK (offline + ffcache + manifest)"
Write-Host "  offline: $offlineDir"
Write-Host "  ffcache: $ffcacheDir"
exit 0
