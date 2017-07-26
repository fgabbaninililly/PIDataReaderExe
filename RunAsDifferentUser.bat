echo off

rem set PSEXEC="D:\PortableApps\PSTools\PsExec.exe"
set PIREADER=[FULL PATH TO PI READER EXE] rem example: C:\Program Files\Eli Lilly\PIDataReaderApps\PIDataReaderExe.exe
set CONFIG=[FULL PATH TO CONFIGURATION FILE] rem example: D:\Projects\DOTNET\PIDataReaderApps\SampleConfigFiles\config.batches.xml
set CREATEFLAG=[none|create|append] rem example none

echo Using PIDataReader at %PIREADER%
echo Using config file at %CONFIG%

rem echo Using psexec at %PSEXEC%
rem %PSEXEC% -u ema\XFDIAOSIPI -p DiaOSIPI01 %PIREADER% -c %CONFIG% -f %CREATEFLAG%

echo Using runas to run exe as a different user
runas /user:ema\XFDIAOSIPI "%PIREADER% -c %CONFIG% -f %CREATEFLAG%"