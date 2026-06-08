param(
    [Parameter(Mandatory = $true)][string]$OfflineCacheDir
)

$offlineCacheDir = [System.IO.Path]::GetFullPath($OfflineCacheDir)
if (-not (Test-Path $offlineCacheDir)) {
    New-Item -ItemType Directory -Path $offlineCacheDir -Force | Out-Null
}

$baseUri = "file:///" + ($offlineCacheDir -replace '\\', '/') + "/"
$assetInfoPath = Join-Path $offlineCacheDir "assetInfo.php"
$rankUrlPath = Join-Path $offlineCacheDir "rankUrl.txt"

Set-Content -Path $assetInfoPath -Value $baseUri -Encoding ASCII -NoNewline
Set-Content -Path $rankUrlPath -Value "http://api.retrobution.xyz/getranks" -Encoding ASCII -NoNewline

Write-Host "Bootstrap files OK in $offlineCacheDir"
Write-Host "  assetInfo.php -> $baseUri"
Write-Host "  rankUrl.txt -> http://api.retrobution.xyz/getranks"
