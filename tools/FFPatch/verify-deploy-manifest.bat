@echo off
setlocal

set "ROOT=D:\work\roberto"
set "UUID=6877b37c-e9cd-4826-b82c-5e8d3d5db744"
set "BUNDLE=%ROOT%\%UUID%\main.unity3d"
set "MANIFEST=%APPDATA%\OpenFusionLauncher\versions\%UUID%.json"

if not exist "%BUNDLE%" (
    echo FAIL: bundle missing
    exit /b 1
)
if not exist "%MANIFEST%" (
    echo FAIL: manifest missing
    exit /b 1
)

powershell -NoProfile -Command ^
  "$b='%BUNDLE%'; $m='%MANIFEST%';" ^
  "$bh=(Get-FileHash $b -Algorithm SHA256).Hash.ToLower(); $bs=(Get-Item $b).Length;" ^
  "$j=Get-Content $m -Raw | ConvertFrom-Json;" ^
  "$ok=$true;" ^
  "if ($j.main_file_url -notlike 'file:///D*') { Write-Host 'FAIL: manifest main_file_url not local file://'; $ok=$false };" ^
  "if ($j.main_file_info.hash -ne $bh) { Write-Host ('FAIL: hash mismatch manifest='+$j.main_file_info.hash+' bundle='+$bh); $ok=$false };" ^
  "if ($j.main_file_info.size -ne $bs) { Write-Host ('FAIL: size mismatch manifest='+$j.main_file_info.size+' bundle='+$bs); $ok=$false };" ^
  "if ($ok) { Write-Host 'verify-deploy-manifest: OK' } else { exit 1 }"

exit /b %ERRORLEVEL%
