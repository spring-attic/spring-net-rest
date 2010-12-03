REM other targets are:
REM 'build'

@ECHO OFF
cls

.\tools\nant\bin\nant.exe -f:Spring.Windows.build package-zip -D:project.sign=true -D:project.releasetype=release > buildlog.txt

start "ignored but required placeholder window title argument" buildlog.txt
