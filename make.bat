@REM ==================
@REM Compile C# sources
@REM ==================
msbuild /p:Configuration=Release

@REM ==================
@REM Build an installer
@REM ==================
iscc.exe /Opublic FlexConfirmMail.iss

@REM ==================
@REM Compress templates
@REM ==================
powershell -C "Compress-Archive  -DestinationPath public\FlexConfirmMailADMX.zip policy"
