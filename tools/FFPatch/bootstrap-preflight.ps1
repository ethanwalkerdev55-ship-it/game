param(
    [Parameter(Mandatory = $true)][string]$Bundle,
    [Parameter(Mandatory = $true)][string]$Manifest,
    [Parameter(Mandatory = $true)][string]$OfflineCacheDir,
    [string]$FfcacheDir = ""
)

$fail = $false

function Fail([string]$msg) {
    Write-Host "FAIL: $msg"
    $script:fail = $true
}

if (-not (Test-Path $Bundle)) {
    Fail "bundle missing: $Bundle"
}

if (-not (Test-Path $Manifest)) {
    Fail "manifest missing: $Manifest"
}

$offlineCacheDir = [System.IO.Path]::GetFullPath($OfflineCacheDir)
$expectedBase = "file:///" + ($offlineCacheDir -replace '\\', '/') + "/"
$uuid = Split-Path $offlineCacheDir -Leaf
if (-not $FfcacheDir) {
    $FfcacheDir = Join-Path $env:LOCALAPPDATA "OpenFusionLauncher\ffcache\$uuid"
}
$ffcacheDir = [System.IO.Path]::GetFullPath($FfcacheDir)

function Test-BootstrapDir([string]$label, [string]$dir, [bool]$requireBundle = $false) {
    $assetInfoPath = Join-Path $dir "assetInfo.php"
    $rankUrlPath = Join-Path $dir "rankUrl.txt"
    $bundlePath = Join-Path $dir "main.unity3d"

    if (-not (Test-Path $dir)) {
        Fail "$label directory missing: $dir"
        return
    }
    if (-not (Test-Path $assetInfoPath)) {
        Fail "$label assetInfo.php missing in $dir"
    } else {
        $assetInfo = Get-Content $assetInfoPath -Raw
        if ($assetInfo -ne $expectedBase) {
            Fail "$label assetInfo.php content wrong (expected $expectedBase got $assetInfo)"
        }
    }
    if (-not (Test-Path $rankUrlPath)) {
        Fail "$label rankUrl.txt missing in $dir"
    }
    if ($requireBundle -and -not (Test-Path $bundlePath)) {
        Fail "$label main.unity3d missing in $dir"
    }
}

Test-BootstrapDir "offline" $offlineCacheDir
Test-BootstrapDir "ffcache" $ffcacheDir $true

if (Test-Path $Bundle) {
    $bundleHash = (Get-FileHash $Bundle -Algorithm SHA256).Hash.ToLower()
    $bundleSize = (Get-Item $Bundle).Length
}

if (Test-Path $Manifest) {
    $j = Get-Content $Manifest -Raw | ConvertFrom-Json
    if ($j.main_file_url -notlike 'file:///D:/work/roberto/*') {
        Fail "manifest main_file_url not local file:///D:/work/roberto/... (got $($j.main_file_url))"
    }
    if ($j.main_file_url -notlike 'file:///D:/work/roberto/*') { }
    if (Test-Path $Bundle) {
        if ($j.main_file_info.hash -ne $bundleHash) {
            Fail "manifest hash mismatch (manifest=$($j.main_file_info.hash) bundle=$bundleHash)"
        }
        if ([int]$j.main_file_info.size -ne $bundleSize) {
            Fail "manifest size mismatch (manifest=$($j.main_file_info.size) bundle=$bundleSize)"
        }
    }
}

if ($fail) {
    Write-Host ""
    Write-Host "bootstrap-preflight: BLOCKED"
    Write-Host "  Run: tools\FFPatch\launch-patched-client.bat"
    Write-Host "  If still failing: tools\FFPatch\restore-client.bat"
    exit 1
}

$ffBundle = Join-Path $ffcacheDir "main.unity3d"
$ffHash = (Get-FileHash $ffBundle -Algorithm SHA256).Hash.ToLower()
$ffSize = (Get-Item $ffBundle).Length
if ($ffHash -ne $bundleHash) {
    Fail "ffcache bundle hash mismatch (offline=$bundleHash ffcache=$ffHash)"
}
if ($ffSize -ne $bundleSize) {
    Fail "ffcache bundle size mismatch (offline=$bundleSize ffcache=$ffSize)"
}

Write-Host "bootstrap-preflight: OK"
Write-Host "  bundle size=$bundleSize hash=$bundleHash"
Write-Host "  manifest url=$($j.main_file_url)"
Write-Host "  assetInfo.php -> $expectedBase"
Write-Host "  ffcache synced: $ffcacheDir"
exit 0
