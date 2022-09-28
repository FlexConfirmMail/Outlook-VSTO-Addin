@REM Defines the certificate to use for signatures.
@REM Use Powershell to find the available certs:
@REM
@REM PS> Get-ChildItem -Path Cert:CurrentUser\My
@REM
set cert=73E7B9D1F72EDA033E7A9D6B17BC37A96CE8513A
set timestamp=http://timestamp.sectigo.com

@REM ==================
@REM Compile C# sources
@REM ==================
msbuild /p:Configuration=Release

@REM ==================
@REM Build an installer
@REM ==================
iscc.exe FlexConfirmMail.iss

@REM ==================
@REM Sign the installer
@REM ==================
signtool sign /t %timestamp%  /fd SHA256 /sha1 %cert% dest\FlexConfirmMailSetup*.exe

@REM ==================
@REM Compress templates
@REM ==================
powershell -C "Compress-Archive  -DestinationPath dest\FlexConfirmMailADMX.zip policy"