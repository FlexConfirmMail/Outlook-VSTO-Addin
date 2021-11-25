[Setup]
AppName=CheckMyMail
AppVerName=CheckMyMail
VersionInfoVersion=1.0.0.0
AppVersion=1.0
DefaultDirName={commonpf}\CheckMyMail
Compression=lzma2
SolidCompression=yes
OutputDir=dest
OutputBaseFilename=CheckMyMailSetup
AppPublisher=DeneBrowser
VersionInfoDescription=CheckMyMailSetup
ArchitecturesAllowed=x64
ArchitecturesInstallIn64BitMode=x64

[Registry]
Root: HKLM; Subkey: "Software\Microsoft\Office\Outlook\Addins\DeneBrowser.CheckMyMail"; Flags: uninsdeletekey
Root: HKLM; Subkey: "Software\Microsoft\Office\Outlook\Addins\DeneBrowser.CheckMyMail"; ValueType: string; ValueName: "Description"; ValueData: "Addon for Checking Recipients and Attachments"
Root: HKLM; Subkey: "Software\Microsoft\Office\Outlook\Addins\DeneBrowser.CheckMyMail"; ValueType: string; ValueName: "FriendlyName"; ValueData: "CheckMyMail"
Root: HKLM; Subkey: "Software\Microsoft\Office\Outlook\Addins\DeneBrowser.CheckMyMail"; ValueType: string; ValueName: "Manifest"; ValueData: "file:///{app}\CheckMyMail.vsto|vstolocal"
Root: HKLM; Subkey: "Software\Microsoft\Office\Outlook\Addins\DeneBrowser.CheckMyMail"; ValueType: dword; ValueName: "LoadBehavior"; ValueData: 3

; Install for 32bit Office as well
Root: HKLM32; Subkey: "Software\Microsoft\Office\Outlook\Addins\DeneBrowser.CheckMyMail"; Flags: uninsdeletekey
Root: HKLM32; Subkey: "Software\Microsoft\Office\Outlook\Addins\DeneBrowser.CheckMyMail"; ValueType: string; ValueName: "Description"; ValueData: "Addon for Checking Recipients and Attachments"
Root: HKLM32; Subkey: "Software\Microsoft\Office\Outlook\Addins\DeneBrowser.CheckMyMail"; ValueType: string; ValueName: "FriendlyName"; ValueData: "CheckMyMail"
Root: HKLM32; Subkey: "Software\Microsoft\Office\Outlook\Addins\DeneBrowser.CheckMyMail"; ValueType: string; ValueName: "Manifest"; ValueData: "file:///{app}\CheckMyMail.vsto|vstolocal"
Root: HKLM32; Subkey: "Software\Microsoft\Office\Outlook\Addins\DeneBrowser.CheckMyMail"; ValueType: dword; ValueName: "LoadBehavior"; ValueData: 3

[Languages]
Name: jp; MessagesFile: "compiler:Languages\Japanese.isl"

[Files]
Source: "bin\Release\CheckMyMail.dll"; DestDir: "{app}"
Source: "bin\Release\CheckMyMail.dll.manifest"; DestDir: "{app}"
Source: "bin\Release\CheckMyMail.vsto"; DestDir: "{app}"
Source: "bin\Release\Microsoft.Office.Tools.Common.v4.0.Utilities.dll"; DestDir: "{app}"
Source: "bin\Release\Microsoft.Office.Tools.Outlook.v4.0.Utilities.dll"; DestDir: "{app}"
