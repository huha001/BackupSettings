REM *************************************************************************
REM DEFINE VARIABLES
REM *************************************************************************
REM 

SET OLDVERSION=1.2.2.12
SET NEWVERSION=1.2.2.13

SET MSFRAMEPATH="C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe"
SET MSFRAMEPATH_MP2="C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"

REM SET FILE_REPLACE=..\FileReplaceString.exe
SET FILE_REPLACE=FileReplaceString.exe
REM ******************************END VARIABLES******************************



REM *************************************************************************
REM Version Update
REM *************************************************************************


MOVE BackupSetting.V%OLDVERSION%.odt BackupSetting.V%NEWVERSION%.odt

DEL BackupSettings.Source\MPEI2\*.mpe1
DEL BackupSettings.Source\MPEI2\*.xml


%FILE_REPLACE% "BackupSettings.Source\BackupSettingsTV_1.3\BackupSettingsTV\BackupSettingsTV.cs" %OLDVERSION% %NEWVERSION%
%FILE_REPLACE% "BackupSettings.Source\BackupSettingsTV_1.3\BackupSettingsTV\Properties\AssemblyInfo.cs" %OLDVERSION% %NEWVERSION%
%FILE_REPLACE% "BackupSettings.Source\BackupSettingsTV_1.3\BackupSettingsTV\BackupSettingsExportImport.cs" %OLDVERSION% %NEWVERSION%

%FILE_REPLACE% "BackupSettings.Source\BackupSettingsTV_1.2\BackupSettingsTV\BackupSettingsTV.cs" %OLDVERSION% %NEWVERSION%
%FILE_REPLACE% "BackupSettings.Source\BackupSettingsTV_1.2\BackupSettingsTV\Properties\AssemblyInfo.cs" %OLDVERSION% %NEWVERSION%
%FILE_REPLACE% "BackupSettings.Source\BackupSettingsTV_1.2\BackupSettingsTV\BackupSettingsExportImport.cs" %OLDVERSION% %NEWVERSION%

%FILE_REPLACE% "BackupSettings.Source\BackupSettingsTV_1.1RC1\BackupSettingsTV\BackupSettingsTV.cs" %OLDVERSION% %NEWVERSION%
%FILE_REPLACE% "BackupSettings.Source\BackupSettingsTV_1.1RC1\BackupSettingsTV\Properties\AssemblyInfo.cs" %OLDVERSION% %NEWVERSION%

%FILE_REPLACE% "BackupSettings.Source\BackupSettingsTV_1.1RC2\BackupSettingsTV\BackupSettingsTV.cs" %OLDVERSION% %NEWVERSION%
%FILE_REPLACE% "BackupSettings.Source\BackupSettingsTV_1.1RC2\BackupSettingsTV\Properties\AssemblyInfo.cs" %OLDVERSION% %NEWVERSION%

%FILE_REPLACE% "BackupSettings.Source\BackupSettingsTV_1.0.1\BackupSettingsTV\BackupSettingsTV.cs" %OLDVERSION% %NEWVERSION%
%FILE_REPLACE% "BackupSettings.Source\BackupSettingsTV_1.0.1\BackupSettingsTV\Properties\AssemblyInfo.cs" %OLDVERSION% %NEWVERSION%

%FILE_REPLACE% "BackupSettings.Source\BackupSettingsMP\BackupSettingsMP\Properties\AssemblyInfo.cs" %OLDVERSION% %NEWVERSION%
%FILE_REPLACE% "BackupSettings.Source\BackupSettingsMP_1.2\BackupSettingsMP\Properties\AssemblyInfo.cs" %OLDVERSION% %NEWVERSION%

%FILE_REPLACE% "BackupSettings.Source\BackupSettingsMP.exe\BackupSettingsMP.exe\Form1.cs" %OLDVERSION% %NEWVERSION%
%FILE_REPLACE% "BackupSettings.Source\BackupSettingsMP.exe\BackupSettingsMP.exe\Form1.Designer.cs" %OLDVERSION% %NEWVERSION%
%FILE_REPLACE% "BackupSettings.Source\BackupSettingsMP.exe\BackupSettingsMP.exe\Properties\AssemblyInfo.cs" %OLDVERSION% %NEWVERSION%
%FILE_REPLACE% "BackupSettings.Source\BackupSettingsMP.exe\BackupSettingsMP.exe\BackupSettingsExportImport.cs" %OLDVERSION% %NEWVERSION%

%FILE_REPLACE% "BackupSettings.Source\Install\Install\InstallSetup.Designer.cs" %OLDVERSION% %NEWVERSION%
%FILE_REPLACE% "BackupSettings.Source\Install\Install\Properties\AssemblyInfo.cs" %OLDVERSION% %NEWVERSION%

%FILE_REPLACE% "BackupSettings.Source\RestartSetupTV\RestartSetupTV\Properties\AssemblyInfo.cs" %OLDVERSION% %NEWVERSION%



%FILE_REPLACE% BackupSettings.Source\MPEI2\BackupSettings.xmp2 %OLDVERSION% %NEWVERSION%

REM ******************** END VERSION UPDATE ********************



REM *************************************************************************
REM Compile ALL
REM *************************************************************************

CD BackupSettings.Source

COPY /Y "BackupSettingsTV_1.3\BackupSettingsTV\BackupSettingsSetupTV.cs" "BackupSettingsTV_1.0.1\BackupSettingsTV\BackupSettingsSetupTV.cs"
COPY /Y "BackupSettingsTV_1.3\BackupSettingsTV\InstallPaths.cs" "BackupSettingsTV_1.0.1\BackupSettingsTV\InstallPaths.cs"
COPY /Y "BackupSettingsTV_1.3\BackupSettingsTV\BackupSettingsTV.cs" "BackupSettingsTV_1.0.1\BackupSettingsTV\BackupSettingsTV.cs"
COPY /Y "BackupSettingsTV_1.3\BackupSettingsTV\BackupSettingsSetupTV.Designer.cs" "BackupSettingsTV_1.0.1\BackupSettingsTV\BackupSettingsSetupTV.Designer.cs"
COPY /Y "BackupSettingsTV_1.3\BackupSettingsTV\BackupSettingsSetupTV.resx" "BackupSettingsTV_1.0.1\BackupSettingsTV\BackupSettingsSetupTV.resx"
COPY /Y "BackupSettingsTV_1.3\BackupSettingsTV\BackupSettingsExportImport.cs" "BackupSettingsTV_1.0.1\BackupSettingsTV\BackupSettingsExportImport.cs"

COPY /Y "BackupSettingsTV_1.3\BackupSettingsTV\BackupSettingsSetupTV.cs" "BackupSettingsTV_1.1RC1\BackupSettingsTV\BackupSettingsSetupTV.cs"
COPY /Y "BackupSettingsTV_1.3\BackupSettingsTV\InstallPaths.cs" "BackupSettingsTV_1.1RC1\BackupSettingsTV\InstallPaths.cs"
COPY /Y "BackupSettingsTV_1.3\BackupSettingsTV\BackupSettingsTV.cs" "BackupSettingsTV_1.1RC1\BackupSettingsTV\BackupSettingsTV.cs"
COPY /Y "BackupSettingsTV_1.3\BackupSettingsTV\BackupSettingsSetupTV.Designer.cs" "BackupSettingsTV_1.1RC1\BackupSettingsTV\BackupSettingsSetupTV.Designer.cs"
COPY /Y "BackupSettingsTV_1.3\BackupSettingsTV\BackupSettingsSetupTV.resx" "BackupSettingsTV_1.1RC1\BackupSettingsTV\BackupSettingsSetupTV.resx"
COPY /Y "BackupSettingsTV_1.3\BackupSettingsTV\BackupSettingsExportImport.cs" "BackupSettingsTV_1.1RC1\BackupSettingsTV\BackupSettingsExportImport.cs"

COPY /Y "BackupSettingsTV_1.3\BackupSettingsTV\BackupSettingsSetupTV.cs" "BackupSettingsTV_1.1RC2\BackupSettingsTV\BackupSettingsSetupTV.cs"
COPY /Y "BackupSettingsTV_1.3\BackupSettingsTV\InstallPaths.cs" "BackupSettingsTV_1.1RC2\BackupSettingsTV\InstallPaths.cs"
COPY /Y "BackupSettingsTV_1.3\BackupSettingsTV\BackupSettingsTV.cs" "BackupSettingsTV_1.1RC2\BackupSettingsTV\BackupSettingsTV.cs"
COPY /Y "BackupSettingsTV_1.3\BackupSettingsTV\BackupSettingsSetupTV.Designer.cs" "BackupSettingsTV_1.1RC2\BackupSettingsTV\BackupSettingsSetupTV.Designer.cs"
COPY /Y "BackupSettingsTV_1.3\BackupSettingsTV\BackupSettingsSetupTV.resx" "BackupSettingsTV_1.1RC2\BackupSettingsTV\BackupSettingsSetupTV.resx"
COPY /Y "BackupSettingsTV_1.3\BackupSettingsTV\BackupSettingsExportImport.cs" "BackupSettingsTV_1.1RC2\BackupSettingsTV\BackupSettingsExportImport.cs"

COPY /Y "BackupSettingsTV_1.3\BackupSettingsTV\BackupSettingsSetupTV.cs" "BackupSettingsTV_1.2\BackupSettingsTV\BackupSettingsSetupTV.cs"
COPY /Y "BackupSettingsTV_1.3\BackupSettingsTV\InstallPaths.cs" "BackupSettingsTV_1.2\BackupSettingsTV\InstallPaths.cs"
COPY /Y "BackupSettingsTV_1.3\BackupSettingsTV\BackupSettingsTV.cs" "BackupSettingsTV_1.2\BackupSettingsTV\BackupSettingsTV.cs"
COPY /Y "BackupSettingsTV_1.3\BackupSettingsTV\BackupSettingsSetupTV.Designer.cs" "BackupSettingsTV_1.2\BackupSettingsTV\BackupSettingsSetupTV.Designer.cs"
COPY /Y "BackupSettingsTV_1.3\BackupSettingsTV\BackupSettingsSetupTV.resx" "BackupSettingsTV_1.2\BackupSettingsTV\BackupSettingsSetupTV.resx"
COPY /Y "BackupSettingsTV_1.3\BackupSettingsTV\BackupSettingsExportImport.cs" "BackupSettingsTV_1.2\BackupSettingsTV\BackupSettingsExportImport.cs"

COPY /Y "BackupSettingsMP_1.2\BackupSettingsMP\BackupSettingsMP.cs" "BackupSettingsMP\BackupSettingsMP\BackupSettingsMP.cs"
COPY /Y "BackupSettingsMP_1.2\BackupSettingsMP\InstallPaths.cs" "BackupSettingsMP\BackupSettingsMP\InstallPaths.cs"

%MSFRAMEPATH% BackupSettingsTV_1.3\BackupSettingsTV.sln /p:Configuration=Release
%MSFRAMEPATH% BackupSettingsTV_1.2\BackupSettingsTV.sln /p:Configuration=Release
%MSFRAMEPATH% BackupSettingsTV_1.1RC2\BackupSettingsTV.sln /p:Configuration=Release
%MSFRAMEPATH% BackupSettingsTV_1.1RC1\BackupSettingsTV.sln /p:Configuration=Release
%MSFRAMEPATH% BackupSettingsTV_1.0.1\BackupSettingsTV.sln /p:Configuration=Release


%MSFRAMEPATH% BackupSettingsMP_1.2\BackupSettingsMP.sln /p:Configuration=Release
%MSFRAMEPATH% BackupSettingsMP\BackupSettingsMP.sln /p:Configuration=Release
%MSFRAMEPATH% BackupSettingsMP.exe\BackupSettingsMP.exe.sln /p:Configuration=Release
%MSFRAMEPATH% Install\Install.sln /p:Configuration=Release

%MSFRAMEPATH_MP2% "BackupSettingsMP2\BackupSettingsMP2.sln" /p:Configuration=Release


REM ******************** END COMPILE  ********************



REM *************************************************************************
REM release All
REM *************************************************************************

CD BackupSettings.Source

COPY /Y "Install\Install\bin\Release\Install.exe" "..\BackupSettings.Release\Install.exe"
COPY /Y "RestartSetupTV\RestartSetupTV\bin\Release\RestartSetupTV.exe" "..\BackupSettings.Release\RestartSetupTV.exe"

COPY /Y "BackupSettingsMP\BackupSettingsMP\bin\Release\BackupSettingsMP.dll" "..\BackupSettings.Release\BackupSettingsMP\BackupSettingsMP.dll"
COPY /Y "BackupSettingsMP_1.2\BackupSettingsMP\bin\Release\BackupSettingsMP.dll" "..\BackupSettings.Release\BackupSettingsMP_1.2\BackupSettingsMP.dll"
COPY /Y "BackupSettingsMP.exe\BackupSettingsMP.exe\bin\Release\BackupSettingsMP.exe" "..\BackupSettings.Release\BackupSettingsMP.exe"

COPY /Y "BackupSettingsMP2\bin\x86\Debug\BackupSettingsMP2.dll" "..\BackupSettings.Release\BackupSettingsMP2\BackupSettingsMP2.dll"
COPY /Y "BackupSettingsMP2\plugin.xml" "..\BackupSettings.Release\BackupSettingsMP2\plugin.xml"
COPY /Y "BackupSettingsMP2\Language\strings_en.xml" "..\BackupSettings.Release\BackupSettingsMP2\Language\strings_en.xml"

COPY /Y "BackupSettingsTV_1.3\BackupSettingsTV\bin\Release\BackupSettingsTV.dll"    "..\BackupSettings.Release\BackupSettingsTV_1.3\BackupSettingsTV.dll"
COPY /Y "BackupSettingsTV_1.2\BackupSettingsTV\bin\Release\BackupSettingsTV.dll"    "..\BackupSettings.Release\BackupSettingsTV_1.2\BackupSettingsTV.dll"
COPY /Y "BackupSettingsTV_1.1RC2\BackupSettingsTV\bin\Release\BackupSettingsTV.dll" "..\BackupSettings.Release\BackupSettingsTV_1.1RC2\BackupSettingsTV.dll"
COPY /Y "BackupSettingsTV_1.1RC1\BackupSettingsTV\bin\Release\BackupSettingsTV.dll" "..\BackupSettings.Release\BackupSettingsTV_1.1RC1\BackupSettingsTV.dll"
COPY /Y "BackupSettingsTV_1.0.1\BackupSettingsTV\bin\Release\BackupSettingsTV.dll" "..\BackupSettings.Release\BackupSettingsTV_1.0.1\BackupSettingsTV.dll"

XCOPY "Bin\BackupSettingsMP2\bin\x86\Release\Plugins\BackupSettingsMP2" "..\BackupSettings.Release\BackupSettingsMP2"  /S /C /I /Y


REM ******************** END RELEASE ********************

CD "..\BackupSettings.Release"
Install.exe install

"C:\Program Files (x86)\Team MediaPortal\MediaPortal TV Server\SetupTv.exe"
