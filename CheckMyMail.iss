[Setup]
AppName=FlexConfirmMail
AppVerName=FlexConfirmMail
VersionInfoVersion=1.1.0.0
AppPublisher=DeneBrowser
AppVersion=1.1.0
UninstallDisplayIcon={app}\cmm.ico
DefaultDirName={commonpf}\FlexConfirmMail
Compression=lzma2
SolidCompression=yes
OutputDir=dest
OutputBaseFilename=FlexConfirmMailSetup
VersionInfoDescription=FlexConfirmMailSetup
ArchitecturesAllowed=x64
ArchitecturesInstallIn64BitMode=x64

[Registry]
Root: HKLM; Subkey: "Software\Microsoft\Office\Outlook\Addins\DeneBrowser.FlexConfirmMail"; Flags: uninsdeletekey
Root: HKLM; Subkey: "Software\Microsoft\Office\Outlook\Addins\DeneBrowser.FlexConfirmMail"; ValueType: string; ValueName: "Description"; ValueData: "Addon for Checking Recipients and Attachments"
Root: HKLM; Subkey: "Software\Microsoft\Office\Outlook\Addins\DeneBrowser.FlexConfirmMail"; ValueType: string; ValueName: "FriendlyName"; ValueData: "FlexConfirmMail"
Root: HKLM; Subkey: "Software\Microsoft\Office\Outlook\Addins\DeneBrowser.FlexConfirmMail"; ValueType: string; ValueName: "Manifest"; ValueData: "file:///{app}\FlexConfirmMail.vsto|vstolocal"
Root: HKLM; Subkey: "Software\Microsoft\Office\Outlook\Addins\DeneBrowser.FlexConfirmMail"; ValueType: dword; ValueName: "LoadBehavior"; ValueData: 3

; Install for 32bit Office as well
Root: HKLM32; Subkey: "Software\Microsoft\Office\Outlook\Addins\DeneBrowser.FlexConfirmMail"; Flags: uninsdeletekey
Root: HKLM32; Subkey: "Software\Microsoft\Office\Outlook\Addins\DeneBrowser.FlexConfirmMail"; ValueType: string; ValueName: "Description"; ValueData: "Addon for Checking Recipients and Attachments"
Root: HKLM32; Subkey: "Software\Microsoft\Office\Outlook\Addins\DeneBrowser.FlexConfirmMail"; ValueType: string; ValueName: "FriendlyName"; ValueData: "FlexConfirmMail"
Root: HKLM32; Subkey: "Software\Microsoft\Office\Outlook\Addins\DeneBrowser.FlexConfirmMail"; ValueType: string; ValueName: "Manifest"; ValueData: "file:///{app}\FlexConfirmMail.vsto|vstolocal"
Root: HKLM32; Subkey: "Software\Microsoft\Office\Outlook\Addins\DeneBrowser.FlexConfirmMail"; ValueType: dword; ValueName: "LoadBehavior"; ValueData: 3

; Prevent Outlook from disabling .NET addon
Root: HKCU; Subkey: "Software\Microsoft\Office\16.0\Outlook\Resiliency\DoNotDisableAddinList"; ValueType: dword; ValueName: "DeneBrowser.FlexConfirmMail"; ValueData: 1
Root: HKCU; Subkey: "Software\Microsoft\Office\13.0\Outlook\Resiliency\DoNotDisableAddinList"; ValueType: dword; ValueName: "DeneBrowser.FlexConfirmMail"; ValueData: 1

[Languages]
Name: jp; MessagesFile: "compiler:Languages\Japanese.isl"

[Files]
Source: "bin\Release\FlexConfirmMail.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\FlexConfirmMail.dll.manifest"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\FlexConfirmMail.vsto"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\Microsoft.Office.Tools.Common.v4.0.Utilities.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\Microsoft.Office.Tools.Outlook.v4.0.Utilities.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "doc\cmm.ico"; DestDir: "{app}"; Flags: ignoreversion
Source: "doc\trusted.txt"; DestDir: "{commonappdata}\FlexConfirmMail"; Flags: ignoreversion onlyifdoesntexist uninsneveruninstall
Source: "doc\unsafe.txt"; DestDir: "{commonappdata}\FlexConfirmMail"; Flags: ignoreversion onlyifdoesntexist uninsneveruninstall
