; -- UPEvents360 Installer Script --

[Setup]
AppName=UP Events 360
AppVersion=1.0.0
AppPublisher=UP Events
AppPublisherURL=https://upevents.ro
DefaultDirName={autopf}\UPEvents360
DefaultGroupName=UP Events 360
UninstallDisplayIcon={app}\Gopro360App.exe
OutputBaseFilename=UPEvents360_Installer
Compression=lzma
SolidCompression=yes
WizardStyle=modern
SetupIconFile=packaging\upevents.ico

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "romanian"; MessagesFile: "compiler:Languages\Romanian.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\UP Events 360"; Filename: "{app}\Gopro360App.exe"
Name: "{commondesktop}\UP Events 360"; Filename: "{app}\Gopro360App.exe"; Tasks: desktopicon

[Run]
Filename: "{app}\Gopro360App.exe"; Description: "{cm:LaunchProgram,UP Events 360}"; Flags: nowait postinstall skipifsilent
