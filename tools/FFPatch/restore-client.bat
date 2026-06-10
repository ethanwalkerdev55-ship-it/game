@echo off
setlocal EnableDelayedExpansion

set "ROOT=D:\work\roberto"
set "UUID=6877b37c-e9cd-4826-b82c-5e8d3d5db744"
set "CACHE=%ROOT%\%UUID%"
set "BUNDLE=%CACHE%\main.unity3d"
set "MAIN=%CACHE%\main"
set "CLIENT_BUNDLE=%ROOT%\_inspect_bundle\client\main.unity3d"
if not exist "%CLIENT_BUNDLE%" set "CLIENT_BUNDLE=%ROOT%\ClientFile\main 2.unity3d"
set "MANIFEST=%APPDATA%\OpenFusionLauncher\versions\%UUID%.json"

echo === Restore client bundle (UdpLogger + working game state) ===

if not exist "%CLIENT_BUNDLE%" (
    echo Missing client backup. Copy ClientFile\main.unity3d to:
    echo   %CLIENT_BUNDLE%
    exit /b 1
)

echo [1/3] Copy client main.unity3d...
copy /Y "%CLIENT_BUNDLE%" "%BUNDLE%" >nul
if %ERRORLEVEL% neq 0 exit /b %ERRORLEVEL%

if exist "%MAIN%" (
    echo [2/3] Remove stale extracted main folder...
    rmdir /S /Q "%MAIN%"
)

echo [3/3] Sync vanilla bundle to offline + ffcache + manifest...
call "%~dp0fix-launcher-manifest.bat" --unpin
call "%~dp0sync-patch-cache.bat"
if %ERRORLEVEL% neq 0 exit /b %ERRORLEVEL%

echo Done. Client bundle restored (no mission patch). Connect from launcher now.
exit /b 0
