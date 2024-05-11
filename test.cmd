@echo off
REM tommojphillips, 08.05.2024
REM test script for loadini.exe

REM redirect stdout to the for loop. This will capture the commands and execute them in this environment.
for /f "delims=" %%k in ('%*') do ( %%k )

REM redirect stderr to nul. This will ignore the error output of loadini.exe
REM for /f "delims=" %%k in ('%* 2^>nul') do ( %%k )

REM redirect stderr to a file. This will log the error output of loadini.exe to error_log.txt
REM for /f "delims=" %%k in ('%* 2^>error_log.txt') do ( %%k )
