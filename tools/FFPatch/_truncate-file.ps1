$client = 'D:\work\roberto\_inspect_bundle\client\Assembly - CSharp.dll'
$rt = "$client.roundtrip.dll"
$out = 'D:\work\roberto\tools\FFPatch\staging\rt-trunc.dll'
$cb = [IO.File]::ReadAllBytes($client)
$rb = [IO.File]::ReadAllBytes($rt)
Write-Host "truncate file from $($rb.Length) to $($cb.Length)"
$trunc = New-Object byte[] $cb.Length
[Array]::Copy($rb, $trunc, $cb.Length)
[IO.File]::WriteAllBytes($out, $trunc)
$diff = 0
for ($i=0; $i -lt $cb.Length; $i++) { if ($cb[$i] -ne $trunc[$i]) { $diff++ } }
Write-Host "diffs vs client: $diff"
