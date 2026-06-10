@echo off
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0bootstrap-snapshot.ps1" %*
exit /b %ERRORLEVEL%
