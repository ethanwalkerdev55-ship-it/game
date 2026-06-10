@echo off
setlocal

set "ROOT=D:\work\roberto"
set "UUID=6877b37c-e9cd-4826-b82c-5e8d3d5db744"
set "BUNDLE=%ROOT%\%UUID%\main.unity3d"
set "MANIFEST=%APPDATA%\OpenFusionLauncher\versions\%UUID%.json"

if "%~1"=="--unpin" (
    powershell -NoProfile -File "%~dp0fix-launcher-manifest.ps1" -Bundle "%BUNDLE%" -Manifest "%MANIFEST%" -Unpin
    exit /b %ERRORLEVEL%
)

if not exist "%BUNDLE%" (
    echo Missing bundle: %BUNDLE%
    exit /b 1
)
if not exist "%MANIFEST%" (
    echo Missing manifest: %MANIFEST%
    exit /b 1
)

powershell -NoProfile -File "%~dp0fix-launcher-manifest.ps1" -Bundle "%BUNDLE%" -Manifest "%MANIFEST%"
exit /b %ERRORLEVEL%
