@echo off
setlocal EnableDelayedExpansion

set "ROOT=D:\work\roberto"
set "JAVA_EXE="
set "JAVAC_EXE="
where java >nul 2>&1
if %ERRORLEVEL% equ 0 (
    set "JAVA_EXE=java"
)
where javac >nul 2>&1
if %ERRORLEVEL% equ 0 (
    set "JAVAC_EXE=javac"
)

if not defined JAVA_EXE (
    echo Java is required for bundle-inject-preserve.
    exit /b 1
)
if not defined JAVAC_EXE (
    echo javac is required to build BundleInjectPreserve.
    exit /b 1
)

if "%~1"=="" (
    echo Usage: bundle-inject-preserve.bat ^<patched-bundle^> ^<client-bundle^>
    exit /b 1
)
if "%~2"=="" (
    echo Usage: bundle-inject-preserve.bat ^<patched-bundle^> ^<client-bundle^>
    exit /b 1
)

set "REBUILD=%ROOT%\tools\FFPatch\_disunity_rebuild"
set "CP=%ROOT%\tools\DisunityFF\disunity.jar;%ROOT%\tools\DisunityFF\lib\*;%REBUILD%"
if not exist "%REBUILD%\BundleInjectPreserve.class" (
    goto :compile_preserve
)
for %%J in ("%REBUILD%\BundleInjectPreserve.java") do set "JAVA_SRC_TIME=%%~tJ"
for %%C in ("%REBUILD%\BundleInjectPreserve.class") do set "JAVA_CLASS_TIME=%%~tC"
if "%JAVA_SRC_TIME%" gtr "%JAVA_CLASS_TIME%" goto :compile_preserve
goto :run_preserve

:compile_preserve
"%JAVAC_EXE%" -cp "%CP%" -source 8 -target 8 -d "%REBUILD%" "%REBUILD%\BundleInjectPreserve.java"
if errorlevel 1 (
    echo FAIL: javac BundleInjectPreserve.java
    exit /b 1
)

:run_preserve
"%JAVA_EXE%" -cp "%CP%" BundleInjectPreserve "%~1" "%~2"
exit /b %ERRORLEVEL%
