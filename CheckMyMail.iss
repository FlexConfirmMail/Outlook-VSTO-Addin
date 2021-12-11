[Setup]
AppName=CheckMyMail
AppVerName=CheckMyMail
VersionInfoVersion=1.0.0.0
AppPublisher=DeneBrowser
AppVersion=1.0
UninstallDisplayIcon={app}\cmm.ico
DefaultDirName={commonpf}\CheckMyMail
Compression=lzma2
SolidCompression=yes
OutputDir=dest
OutputBaseFilename=CheckMyMailSetup
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

; Prevent Outlook from disabling .NET addon
Root: HKCU; Subkey: "Software\Microsoft\Office\16.0\Outlook\Resiliency\DoNotDisableAddinList"; ValueType: dword; ValueName: "DeneBrowser.CheckMyMail"; ValueData: 1
Root: HKCU; Subkey: "Software\Microsoft\Office\13.0\Outlook\Resiliency\DoNotDisableAddinList"; ValueType: dword; ValueName: "DeneBrowser.CheckMyMail"; ValueData: 1

[Languages]
Name: jp; MessagesFile: "compiler:Languages\Japanese.isl"

[Files]
Source: "bin\Release\CheckMyMail.dll"; DestDir: "{app}"
Source: "bin\Release\CheckMyMail.dll.manifest"; DestDir: "{app}"
Source: "bin\Release\CheckMyMail.vsto"; DestDir: "{app}"
Source: "bin\Release\Microsoft.Office.Tools.Common.v4.0.Utilities.dll"; DestDir: "{app}"
Source: "bin\Release\Microsoft.Office.Tools.Outlook.v4.0.Utilities.dll"; DestDir: "{app}"
Source: "doc\cmm.ico"; DestDir: "{app}"
Source: "doc\EnableCheckMyMail.bat "; DestDir: "{app}"
