param(
    [string]$Root = "D:\work\roberto",
    [string]$Uuid = "6877b37c-e9cd-4826-b82c-5e8d3d5db744",
    [string]$LogPath = "D:\work\roberto\_inspect_udp_listener\fusionfall_log.txt",
    [string]$ContextPath = "D:\work\roberto\_inspect_udp_listener\bootstrap_context.json"
)

$offlineDir = Join-Path $Root $Uuid
$ffcacheDir = Join-Path $env:LOCALAPPDATA "OpenFusionLauncher\ffcache\$Uuid"
$manifest = Join-Path $env:APPDATA "OpenFusionLauncher\versions\$Uuid.json"
$ffrunnerLog = Join-Path $env:LOCALAPPDATA "OpenFusionLauncher\ffrunner.log"

function FileFacts([string]$path) {
    if (-not (Test-Path $path)) {
        return @{ exists = $false; path = $path }
    }
    $item = Get-Item $path
    return @{
        exists = $true
        path = $path
        size = $item.Length
        hash = (Get-FileHash $path -Algorithm SHA256).Hash.ToLower()
        mtime = $item.LastWriteTimeUtc.ToString("o")
    }
}

function BootstrapFacts([string]$dir) {
    $facts = @{ dir = $dir; exists = (Test-Path $dir) }
    foreach ($name in @("assetInfo.php", "rankUrl.txt", "main.unity3d")) {
        $facts[$name] = FileFacts (Join-Path $dir $name)
    }
    if ($facts["assetInfo.php"].exists) {
        $facts["assetInfo_content"] = (Get-Content (Join-Path $dir "assetInfo.php") -Raw).Trim()
    }
    return $facts
}

$manifestFacts = @{ exists = $false }
if (Test-Path $manifest) {
    $j = Get-Content $manifest -Raw | ConvertFrom-Json
    $manifestFacts = @{
        exists = $true
        path = $manifest
        main_file_url = $j.main_file_url
        main_hash = $j.main_file_info.hash
        main_size = [int]$j.main_file_info.size
        read_only = (Get-Item $manifest).IsReadOnly
    }
}

$snapshot = @{
    captured_at = (Get-Date).ToUniversalTime().ToString("o")
    note = "Requesting to assetInfo.php is a REQUEST-START log line, not the failure. Stall means local WWW never completed."
    offline = BootstrapFacts $offlineDir
    ffcache = BootstrapFacts $ffcacheDir
    manifest = $manifestFacts
    ffrunner_log = FileFacts $ffrunnerLog
}

$json = $snapshot | ConvertTo-Json -Depth 8
Set-Content -Path $ContextPath -Value $json -Encoding UTF8

$block = @(
    "",
    "--- bootstrap snapshot $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss') ---",
    "NOTE: 'Requesting to assetInfo.php' = WWW request START (failures are silent in game log).",
    "offline bundle: size=$($snapshot.offline.'main.unity3d'.size) hash=$($snapshot.offline.'main.unity3d'.hash)",
    "ffcache bundle: size=$($snapshot.ffcache.'main.unity3d'.size) hash=$($snapshot.ffcache.'main.unity3d'.hash)",
    "manifest url: $($snapshot.manifest.main_file_url)",
    "manifest hash: $($snapshot.manifest.main_hash)",
    "offline assetInfo: $($snapshot.offline.assetInfo_content)",
    "ffcache assetInfo exists: $($snapshot.ffcache.'assetInfo.php'.exists)",
    "context: $ContextPath",
    "---"
)
Add-Content -Path $LogPath -Value ($block -join "`n") -Encoding UTF8

Write-Host "bootstrap-snapshot: wrote $ContextPath"
Write-Host "  offline bundle hash=$($snapshot.offline.'main.unity3d'.hash)"
Write-Host "  ffcache bundle hash=$($snapshot.ffcache.'main.unity3d'.hash)"
if ($snapshot.offline.'main.unity3d'.hash -ne $snapshot.ffcache.'main.unity3d'.hash) {
    Write-Host "WARN: offline and ffcache bundle hashes differ"
}
if ($snapshot.manifest.main_hash -and $snapshot.offline.'main.unity3d'.hash -and
    $snapshot.manifest.main_hash -ne $snapshot.offline.'main.unity3d'.hash) {
    Write-Host "WARN: manifest hash does not match offline bundle"
}

exit 0
