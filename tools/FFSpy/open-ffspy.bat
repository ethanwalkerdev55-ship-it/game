@echo off
setlocal

set "BASEDIR=%~dp0"
set "DLL=D:\work\roberto\6877b37c-e9cd-4826-b82c-5e8d3d5db744\main\Assembly - CSharp.dll"

echo FFSpy/ILSpy is for browsing only. For dnSpy paste-back export use:
echo   %BASEDIR%export-assembly-csharp.bat
echo.

if exist "%DLL%" (
    start "" "%BASEDIR%ILSpy.exe" "%DLL%"
) else (
    start "" "%BASEDIR%ILSpy.exe"
    echo Opened ILSpy. Use File -^> Open and select your DLL.
)
