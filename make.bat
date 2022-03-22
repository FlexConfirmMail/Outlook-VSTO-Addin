@REM Defines the certificate to use for signatures.
@REM Use Powershell to find the available certs:
@REM
@REM PS> Get-ChildItem -Path Cert:CurrentUser\My
@REM
set cert=1F02225E5C681EFACE0520494285DAF0D24987DF
set timestamp=http://timestamp.digicert.com

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