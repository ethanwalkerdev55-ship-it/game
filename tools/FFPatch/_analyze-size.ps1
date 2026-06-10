$client = [IO.File]::ReadAllBytes('D:\work\roberto\_inspect_bundle\client\Assembly - CSharp.dll')
$patched = 'D:\work\roberto\tools\FFPatch\staging\Assembly-CSharp.patched.dll'
if (-not (Test-Path $patched)) {
    Write-Host 'no patched dll'
    exit 1
}
$p = [IO.File]::ReadAllBytes($patched)
Write-Host "client=$($client.Length) patched=$($p.Length) delta=$($p.Length - $client.Length)"
$tail = 0
for ($i = $client.Length - 1; $i -ge 0; $i--) {
    if ($client[$i] -eq 0) { $tail++ } else { break }
}
Write-Host "client trailing zeros: $tail"
$diff = 0
$max = [Math]::Min($client.Length, $p.Length)
for ($i = 0; $i -lt $max; $i++) {
    if ($client[$i] -ne $p[$i]) { $diff++ }
}
Write-Host "byte diffs in shared prefix: $diff"
if ($p.Length -gt $client.Length) {
    $extra = $p[$client.Length..($p.Length - 1)]
    $nonZero = ($extra | Where-Object { $_ -ne 0 }).Count
    Write-Host "extra bytes beyond client size: $($extra.Count) nonZero=$nonZero"
}
