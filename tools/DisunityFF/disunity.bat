@echo off
setlocal

set "JAVA_EXE="
where java >nul 2>&1
if %ERRORLEVEL% equ 0 (
    set "JAVA_EXE=java"
) else (
    for %%D in (
        "%ProgramFiles%\Eclipse Adoptium"
        "%ProgramFiles%\Java"
        "%LOCALAPPDATA%\Programs\Eclipse Adoptium"
    ) do (
        if not defined JAVA_EXE if exist "%%~D" (
            for /f "delims=" %%J in ('dir /b /s "%%~D\java.exe" 2^>nul') do (
                if not defined JAVA_EXE set "JAVA_EXE=%%J"
            )
        )
    )
)

if not defined JAVA_EXE (
    echo Java is required but was not found on PATH.
    echo Install Eclipse Temurin JRE 17, then reopen this terminal.
    exit /b 1
)

set "BASEDIR=%~dp0"
set "CLASSPATH=%BASEDIR%disunity.jar;%BASEDIR%lib\*"

rem DisUnityFF = OpenFusion fork (see VERSION.txt). Verify: disunity.bat -h shows bundle-create
"%JAVA_EXE%" -cp "%CLASSPATH%" info.ata4.unity.cli.DisUnityCli %*
