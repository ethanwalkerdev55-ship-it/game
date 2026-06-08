@echo off
setlocal

echo.
echo === Mission patch launcher prep ===
echo.
echo OpenFusionLauncher rewrites the version manifest when you switch pages
echo (login -^> servers -^> game builds). That reverts main.unity3d to CDN vanilla
echo and interrupts Connect / reloads the client at CreateGameMode:1.
echo.
echo This script pins the manifest to your LOCAL patched bundle.
echo.

call "%~dp0fix-launcher-manifest.bat"
if %ERRORLEVEL% neq 0 exit /b %ERRORLEVEL%

call "%~dp0verify-deploy-manifest.bat"
if %ERRORLEVEL% neq 0 (
    echo.
    echo Manifest still points at CDN. Run: apply-mission-patch-client-deploy.bat
    exit /b 1
)

echo.
echo Manifest OK and pinned. In the launcher:
echo   1. Stay on the server list page (Tester1 selected)
echo   2. Click Connect immediately — do not open Game Builds / other pages first
echo   3. If you already navigated away, run this script again, then Connect
echo.
echo Rollback / unpin manifest: restore-client.bat
exit /b 0
