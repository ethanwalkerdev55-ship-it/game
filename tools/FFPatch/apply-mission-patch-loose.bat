@echo off
setlocal EnableDelayedExpansion

set "ROOT=D:\work\roberto"
set "UUID=6877b37c-e9cd-4826-b82c-5e8d3d5db744"
set "BUNDLE=%ROOT%\%UUID%\main.unity3d"
set "MAIN=%ROOT%\%UUID%\main"
set "CLIENT_BUNDLE=%ROOT%\_inspect_bundle\client\main.unity3d"
set "CLIENT_FIRSTPASS=%ROOT%\_inspect_bundle\client\Assembly - CSharp - first pass.dll"
set "STAGED=%ROOT%\tools\FFPatch\staging\Assembly-CSharp.patched.dll"
set "DISUNITY=%ROOT%\tools\DisunityFF\disunity.bat"

echo === Mission patch LOOSE deploy (FALLBACK — patch NOT loaded) ===
echo WARNING: Game loads Assembly-CSharp from main.unity3d bundle only.
echo LOOSE keeps vanilla bundle — Connect works but ForceCompleteV2 will NOT appear.
echo Use apply-mission-patch-client-deploy.bat for real patch deploy.
echo Rollback: restore-client.bat

if not exist "%STAGED%" (
    echo Staging DLL missing. Run apply-mission-patch-staging.bat first.
    exit /b 1
)
if not exist "%CLIENT_BUNDLE%" (
    echo Client backup missing.
    exit /b 1
)

echo [1/4] Restore vanilla main.unity3d...
copy /Y "%CLIENT_BUNDLE%" "%BUNDLE%" >nul

echo [2/4] Extract bundle...
if exist "%MAIN%" rmdir /S /Q "%MAIN%"
call "%DISUNITY%" bundle-extract "%BUNDLE%"
if %ERRORLEVEL% neq 0 exit /b %ERRORLEVEL%

echo [3/4] Install patched DLL (no bundle-inject)...
if exist "%CLIENT_FIRSTPASS%" copy /Y "%CLIENT_FIRSTPASS%" "%MAIN%\Assembly - CSharp - first pass.dll" >nul
copy /Y "%STAGED%" "%MAIN%\Assembly - CSharp.dll" >nul

echo [4/4] Update manifest + bootstrap...
call "%~dp0fix-launcher-manifest.bat"
if %ERRORLEVEL% neq 0 exit /b %ERRORLEVEL%
call "%~dp0verify-deploy-manifest.bat"
if %ERRORLEVEL% neq 0 exit /b %ERRORLEVEL%

echo.
echo LOOSE deploy done. Vanilla bundle hash preserved; patched DLL is in %MAIN%
echo Connect from launcher, then test hotkey. Log must show:
echo   ForceCompleteV2: patch build 2026-06-08-doc504
exit /b 0
