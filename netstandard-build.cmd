@echo off
@echo Running full Build Script, capturing output to buildlog.txt file...
#tools\NAnt\bin\nant.exe build -f:NetStandard-Spring.Rest.build > buildlog.txt

tools\NAnt\bin\nant.exe package-nuget -f:NetStandard-Spring.Rest.build > buildlog.txt
@echo Launching text file viewer to display buildlog.txt contents...
start "ignored but required placeholder window title argument" buildlog.txt

