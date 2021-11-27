@REM =========================================================
@REM A small script to keep Outlook from disabling CheckMyMail
@REM by adding an entry to DoNotDisableAddinList.
@REM
@REM https://docs.microsoft.com/en-us/office/vba/outlook/Concepts/Getting-Started/support-for-keeping-add-ins-enabled
@REM ==========================================================

REG ADD HKCU\Software\Microsoft\Office\15.0\Outlook\Resiliency\DoNotDisableAddinList /v DeneBrowser.CheckMyMail /t REG_DWORD /d 1
REG ADD HKCU\Software\Microsoft\Office\16.0\Outlook\Resiliency\DoNotDisableAddinList /v DeneBrowser.CheckMyMail /t REG_DWORD /d 1

REG ADD HKCU\Software\Microsoft\Office\15.0\Outlook\Resiliency\DoNotDisableAddinList /v DeneBrowser.CheckMyMail /t REG_DWORD /d 1 /reg:32
REG ADD HKCU\Software\Microsoft\Office\16.0\Outlook\Resiliency\DoNotDisableAddinList /v DeneBrowser.CheckMyMail /t REG_DWORD /d 1 /reg:32
