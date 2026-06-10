@echo off
setlocal EnableDelayedExpansion

set "JAVA_EXE="
where java >nul 2>&1
if %ERRORLEVEL% equ 0 (
    set "JAVA_EXE=java"
) else if defined JAVA_HOME (
    set "JAVA_EXE=%JAVA_HOME%\bin\java.exe"
)

if not defined JAVA_EXE (
    echo Java is required for bundle-inject-raw.
    exit /b 1
)

set "ROOT=D:\work\roberto"
set "CP=%ROOT%\tools\DisunityFF\disunity.jar;%ROOT%\tools\DisunityFF\lib\*;%ROOT%\tools\FFPatch\_disunity_rebuild"
set "MAIN=BundleInjectRaw"

if not exist "%ROOT%\tools\DisunityFF\disunity.jar" (
    echo Missing disunity.jar
    exit /b 1
)

if "%~1"=="" (
    echo Usage: bundle-inject-raw.bat ^<path\to\main.unity3d^>
    echo Requires sibling extract folder ^(bundle-extract first^).
    exit /b 1
)

"%JAVA_EXE%" -cp "%CP%" %MAIN% "%~1"
exit /b %ERRORLEVEL%
