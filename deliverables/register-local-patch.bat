@echo off
setlocal
echo.
echo After copying main.unity3d into your cache folder, run this once before Connect.
echo.
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0register-local-patch.ps1" %*
if %ERRORLEVEL% neq 0 (
    echo.
    echo Failed. See message above.
    pause
    exit /b %ERRORLEVEL%
)
echo.
pause
exit /b 0
