@echo off
setlocal

set "ROOT=D:\work\roberto"
set "UUID=6877b37c-e9cd-4826-b82c-5e8d3d5db744"
set "BUNDLE=%ROOT%\%UUID%\main.unity3d"
set "MANIFEST=%APPDATA%\OpenFusionLauncher\versions\%UUID%.json"
set "CACHE=%ROOT%\%UUID%"

powershell -NoProfile -File "%~dp0bootstrap-preflight.ps1" -Bundle "%BUNDLE%" -Manifest "%MANIFEST%" -OfflineCacheDir "%CACHE%"
exit /b %ERRORLEVEL%
