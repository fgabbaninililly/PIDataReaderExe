@echo off

rem set PSEXEC="D:\PortableApps\PSTools\PsExec.exe"

rem example: C:\Program Files\Eli Lilly\PIDataReaderApps\PIDataReaderExe.exe
set PIREADER=[FULL PATH TO PI READER EXE]

rem example: D:\Projects\DOTNET\PIDataReaderApps\SampleConfigFiles\config.batches.xml
set CONFIG=[FULL PATH TO CONFIGURATION FILE]

rem example none
set CREATEFLAG=[none|create|append]

echo.
echo Using PIDataReader at %PIREADER%
echo.
echo Using config file at %CONFIG%
echo.

rem echo Using psexec at %PSEXEC%
rem %PSEXEC% -u ema\XFDIAOSIPI -p DiaOSIPI01 %PIREADER% -c %CONFIG% -f %CREATEFLAG%

echo Using runas to run exe as a different user
echo.
runas /user:ema\XFDIAOSIPI "%PIREADER% -c %CONFIG% -f %CREATEFLAG%"