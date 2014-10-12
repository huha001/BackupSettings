/* 
 *	Copyright (C) 2005-2012 Team MediaPortal
 *	http://www.team-mediaportal.com
 *
 *  This Program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2, or (at your option)
 *  any later version.
 *   
 *  This Program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 *  GNU General Public License for more details.
 *   
 *  You should have received a copy of the GNU General Public License
 *  along with GNU Make; see the file COPYING.  If not, write to
 *  the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA. 
 *  http://www.gnu.org/copyleft/gpl.html
 *
 */

// #define PB

//*****************
//Version 1.0.0.12
//*****************

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System;

using MediaPortal.Plugins;

#if (TV)
using TvLibrary.Log;
using TvEngine;
using TvControl;
using TvDatabase;

using Gentle.Common;
using Gentle.Framework;
using TvLibrary;
using TvLibrary.Interfaces;
using TvLibrary.Implementations;
using TvLibrary.Channels;
using similaritymetrics;
using MediaPortal.UserInterface.Controls;
using DirectShowLib;
using DirectShowLib.BDA;

using SetupTv.Sections;
using SetupTv;
#else

#endif



namespace BackupSettingsPlugin
{
    public delegate void textexportmessage(string text);


    public enum PB_action
    {
        INIT = 0,
        START,
        STOP,
        CANCEL,
        COMPLETE,
    }

    public enum PB_part
    {
        TV_TvServerProgramFolders = 0,
        TV_TvServerUserFolders,
        TV_TvServerxml,
        TV_DeleteAllChannels,
        TV_DeleteAllTvGroups,
        TV_DeleteAllRadioGroups,
        TV_deleteAllSchedules,
        TV_DeleteAllRecordings,
        TV_Servers,
        TV_Channels,
        TV_CardMappings,
        TV_TvGroups,
        TV_Radiogroups,
        TV_Schedules,
        TV_EPG,
        TV_Recordings,
        TV_GeneralSettings,
        TV_PluginSettings,
        TV_ConfigRestart,

        MP_ProgramXML,
        MP_Plugins,
        MP_Skins,
        MP_Language,
        MP_UserXML,
        MP_Database,
        MP_InputDeviceMappings,
        MP_Thumbs,
        MP_MusicPlayer,
        MP_Xmltv,
        MP_DeleteCache,
        MP_AllMediaPortalUserFolders,
        MP_AllMediaPortalProgramFolders,

        MP2_Defaults,
        MP2_Config,
        MP2_Plugins,
        MP2_AllClientProgramFolders,
        MP2_AllClientFolders,
        MP2_AllClientFiles,

        SV2_Defaults,
        SV2_Config,
        SV2_Database,                      //40
        SV2_Plugins,
        SV2_AllServerProgramFolders,
        SV2_AllServerFolders,
        SV2_AllServerFiles,                //44

        Start_TVServer,
        StopTvServer,
        DeleteOldFiles,

        ExtraFolders

    }

    public enum COMPAREVERSION
    {
        EQUAL = 0,
        NEWER,
        OLDER,
        ERRORVERSION1,
        ERRORVERSION2,
    }

    public enum WhoAmI
    {
        Tv_Server = 0,
        MP1_Client,
        MP2_Client,
        MP2_Server,
    }

    public enum Scripts
    {
        TV_Export = 1,
        TV_Import,
        MP_Export,
        MP_Import,
        AutoCorrect,
    }

    class BackupSettingsExportImport
    {

#region globaldefinitions
        
        bool TV = true;
        bool MP = true;
        bool MP2C = true;
        bool MP2S = true;

        string MP_PROGRAM_FOLDER = "";
        string MP_USER_FOLDER = "";
        string TV_PROGRAM_FOLDER = "";
        string TV_USER_FOLDER = "";

        string MP2_PROGRAM_FOLDER = "";
        string MP2_USER_FOLDER = "";
        string SV2_PROGRAM_FOLDER = "";
        string SV2_USER_FOLDER = "";

        //public string CONFIG_FILE = "NOT_DEFINED";

        //Constant file names - do not change
        public string RESTART_SETUP_TV_EXE = "RestartSetupTV.exe";
        public string BACKUPSETTING_TV_DIR = "BackupSettings";
        public string BACKUPSETTING_MP_DIR = "BackupSettings";
        //public string STATUS_FILE = "Status.txt";
        public string INSTALLPATHS_FILE = "MP_TV_Installpaths.txt";
        public string POSTPROCESSING_FILE = "ImportPostprocessing.txt";
        public string MEDIAPORTALDIRS_FILE = "MediaPortalDirs.xml";
        public string TV_CONFIG_WINDOW_NAME = "MediaPortal - TV Server Configuration";
        public string TV_CONFIG_WINDOW_CLASS = "WindowsForms10.Window.8.app.0.33c0d9d";


        //All paths from MediaPortalDirs.xml
        public string DIR_Config = "NOT_DEFINED";
        public string DIR_Plugins = "NOT_DEFINED";
        public string DIR_Log = "NOT_DEFINED";
        public string DIR_CustomInputDevice = "NOT_DEFINED";
        public string DIR_CustomInputDefault = "NOT_DEFINED";
        public string DIR_Skin = "NOT_DEFINED";
        public string DIR_Language = "NOT_DEFINED";
        public string DIR_Database = "NOT_DEFINED";
        public string DIR_Thumbs = "NOT_DEFINED";
        public string DIR_Weather = "NOT_DEFINED";
        public string DIR_Cache = "NOT_DEFINED";
        public string DIR_BurnerSupport = "NOT_DEFINED";

        public string POSTIMPORT = "";
        public bool BUSY = false;
        public bool DEBUG = false;
        //public bool firstuse = false;
        public bool CLOSE_TV_CONFIG = false;
        public bool LIST_RED = false;

        
        char BACKUPSETTINGS_COLUMN_SEPARATOR = ';';
        int LISTVIEWCOLUMNS = 6;
        

        string[] ExtraFolderData;

        DateTime time1;
        DateTime time2;

        bool SILENT = false;


        public string pathname = "NOT_DEFINED";

#if (!TV)
        public string xmlfilename = "NOT_DEFINED";
#endif

        public int[] PB_Export = new int[49];
        public int[] PB_Import = new int[49];
        public int PB_PARTNUMBERS = 48;
#if (TV)
        int PB_Duplicate = 1;
#endif

        int PB_part_start = 0;
        int PB_part_end = 0;
        int PB_UPDATER = 0;
        int PB_action_number = 0;

        bool PB_PAUSE = false;
        bool PB_CANCEL = false;

        string pluginversion = "1.2.2.13";

        string ActualBackupSettingsVersion = "NOT DEFINED";
        string ActualMediaPortalVersion = "NOT DEFINED";
        string ActualTvServerVersion = "NOT DEFINED";
        string ActualMP2ServerVersion = "NOT DEFINED";
        string ActualMP2ClientVersion = "NOT DEFINED";

        string BackupBackupSettingsVersion = "NOT DEFINED";
        string BackupMediaPortalVersion = "NOT DEFINED";
        string BackupTvServerVersion = "NOT DEFINED";
        string BackupMP2ServerVersion = "NOT DEFINED";
        string BackupMP2ClientVersion = "NOT DEFINED";

        //Tvserver filters
        bool server = false;
        bool channels = false;
        bool channelcardmappings = false;
        bool tvgroups = false;
        bool radiogroups = false;
        bool schedules = false;
        bool epg = false;
        bool recordings = false;
        bool general_settings = false;
        bool clickfinder = false;
        bool delete_channels = false;
        bool delete_tvgroups = false;
        bool delete_radiogroups = false;
        bool delete_schedules = false;
        bool delete_recordings = false;
        bool TVServerSettings = false;
        bool TVallPrograms = false;
        bool TVAllFolders = false;
        public bool TVRestart = false;
        bool TVAutoCorrect = false;

        //MP1 Client Filters
        bool MPDatabase = false;
        bool MPInputDevice = false;
        bool MPPlugins = false;
        bool MPProgramXml = false;
        bool MPSkins = false;
        bool MPThumbs = false;
        bool MPUserXML = false;
        bool MPxmltv = false;
        bool MPMusicPlayer = false;
        bool MPDeleteCache = false;
        bool MPAllFolder = false;
        bool MPAllProgram = false;

        //MP2 Client Filters
        bool MP2Defaults = false;
        bool MP2Config = false;
        bool MP2Plugins = false;
        bool MP2AllClientFolders = false;
        bool MP2AllClientProgramFolders = false;
        bool MP2AllClientFiles = false;

        //MP2 Server Filters
        bool SV2Configuration = false;
        bool SV2Database = false;
        bool SV2Plugins = false;
        bool SV2Defaults = false;
        bool SV2AllServerFolders = false;
        bool SV2AllServerProgramFolders = false;
        bool SV2AllServerFiles = false;

        //duplicate
#if (TV)
        bool duplicateinteractive = false;
        bool duplicateautoprocess = false;

        public StreamWriter Status;
#endif
        

        public event textexportmessage newmessage;

        public ProgressBar exportprogressBar = null;

        InstallPaths instpaths = new InstallPaths();  //define new instance for folder detection

        System.Threading.Thread PB_th = null;


#endregion globaldefinitions




#if (TV)
        #region constructor
        public void ExportImportInit(ref ProgressBar myprogressBar, bool silent )
        {
            exportprogressBar = myprogressBar;
            SILENT = silent;
        }
        #endregion constructor

        public void MyLoadSettings()
        {
            try
            {
                TvBusinessLayer layer = new TvBusinessLayer();
                Setting setting;

                setting = layer.GetSetting("Backup_SettingsSetup_debug", "false");
                if (setting.Value.ToLower() == "true")
                {
                    DEBUG = true;
                }
                else
                {
                    DEBUG = false;
                }

                //pathnames must have been defined before calling importexport!
                TV_PROGRAM_FOLDER = layer.GetSetting("Backup_SettingsSetup_TV_PROGRAM_FOLDER", "NOT_DEFINED").Value;
                TV_USER_FOLDER = layer.GetSetting("Backup_SettingsSetup_TV_USER_FOLDER", "NOT_DEFINED").Value;                
                MP_PROGRAM_FOLDER = layer.GetSetting("Backup_SettingsSetup_MP_PROGRAM_FOLDER", "NOT_DEFINED").Value;
                MP_USER_FOLDER = layer.GetSetting("Backup_SettingsSetup_MP_USER_FOLDER", "NOT_DEFINED").Value;
                MP2_PROGRAM_FOLDER = layer.GetSetting("Backup_SettingsSetup_MP2_PROGRAM_FOLDER", "NOT_DEFINED").Value;
                MP2_USER_FOLDER = layer.GetSetting("Backup_SettingsSetup_MP2_USER_FOLDER", "NOT_DEFINED").Value;
                SV2_PROGRAM_FOLDER = layer.GetSetting("Backup_SettingsSetup_SV2_PROGRAM_FOLDER", "NOT_DEFINED").Value;
                SV2_USER_FOLDER = layer.GetSetting("Backup_SettingsSetup_SV2_USER_FOLDER", "NOT_DEFINED").Value;

                instpaths.DEBUG = DEBUG;
                instpaths.TV_PROGRAM_FOLDER = TV_PROGRAM_FOLDER;
                instpaths.TV_USER_FOLDER = TV_USER_FOLDER;
                instpaths.MP_PROGRAM_FOLDER = MP_PROGRAM_FOLDER;
                instpaths.MP_USER_FOLDER = MP_USER_FOLDER;
                instpaths.MP2_PROGRAM_FOLDER = MP2_PROGRAM_FOLDER;
                instpaths.MP2_USER_FOLDER = MP2_USER_FOLDER;
                instpaths.SV2_PROGRAM_FOLDER = SV2_PROGRAM_FOLDER;
                instpaths.SV2_USER_FOLDER = SV2_USER_FOLDER;

                instpaths.GetMediaPortalDirs();
                instpaths.GetMediaPortalDirsMP2();

                //in case there are invalid paths on tvserver when changing between 32 and 64bit operating system try once again default paths
                if (File.Exists(TV_PROGRAM_FOLDER + @"\TvService.exe") == false)
                {
                    instpaths.GetInstallPaths();
                    TV_PROGRAM_FOLDER = instpaths.TV_PROGRAM_FOLDER;
                }

                if (File.Exists(MP_PROGRAM_FOLDER + @"\MediaPortal.exe") == false)
                {
                    instpaths.GetInstallPaths();
                    MP_PROGRAM_FOLDER = instpaths.MP_PROGRAM_FOLDER;
                }

                if (File.Exists(MP2_PROGRAM_FOLDER + @"\MP2-Client.exe") == false)
                {
                    instpaths.GetInstallPathsMP2();
                    MP2_PROGRAM_FOLDER = instpaths.MP2_PROGRAM_FOLDER;
                }

                if (File.Exists(SV2_PROGRAM_FOLDER + @"\MP2-Server.exe") == false)
                {
                    instpaths.GetInstallPathsMP2();
                    SV2_PROGRAM_FOLDER = instpaths.SV2_PROGRAM_FOLDER;
                }


                
                setting = layer.GetSetting("Backup_SettingsSetup_filename", @"C:\MediaPortal Backups");
                pathname = setting.Value;

                setting = layer.GetSetting("Backup_SettingsSetup_TV", "true");
                TV = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSetup_MP", "true");
                MP = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSetup_MP2C", "true");
                MP2C = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSetup_MP2S", "true");
                MP2S = Convert.ToBoolean(setting.Value);                

                setting = layer.GetSetting("Backup_SettingsSetup_filter_server", "true");
                server = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSetup_filter_channels", "true");
                channels = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSetup_filter_channelcardmappings", "true");
                channelcardmappings = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSetup_filter_tvgroups", "true");
                tvgroups = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSetup_filter_radiogroups", "true");
                radiogroups = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSetup_filter_schedules", "true");
                schedules = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSetup_filter_epg", "false");
                epg = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSetup_filter_recordings", "true");
                recordings = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSetup_filter_general_settings", "true");  
                general_settings = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSetup_filter_plugins", "true");  //check for bug  checkBoxClickfinder setting Backup_SettingsSetup_filter_plugins
                clickfinder = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSetup_delete_channels", "true");
                delete_channels = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSetup_delete_tvgroups", "true");
                delete_tvgroups = Convert.ToBoolean(setting.Value);
                
                setting = layer.GetSetting("Backup_SettingsSetup_delete_radiogroups", "true");
                delete_radiogroups = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSetup_delete_schedules", "true");
                delete_schedules = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSetup_delete_recordings", "true");
                delete_recordings = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsTVServerSettings", "true");
                TVServerSettings = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsTVallPrograms", "true");
                TVallPrograms = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsTVAllFolders", "true");
                TVAllFolders = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsTVRestart", "true");
                TVRestart = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsTVAutoCorrect", "true");
                TVAutoCorrect = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsMPDatabase", "true");
                MPDatabase = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsMPInputDevice", "true");
                MPInputDevice = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsMPPlugins", "true");
                MPPlugins = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsMPProgramXml", "true");
                MPProgramXml = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsMPSkins", "true");
                MPSkins = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsMPThumbs", "true");
                MPThumbs = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsMPUserXML", "true");
                MPUserXML = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsMPxmltv", "true");
                MPxmltv = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsMPMusicPlayer", "true");
                MPMusicPlayer = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsMPDeleteCache", "true");
                MPDeleteCache = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsMPAllFolders", "true");
                MPAllFolder = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsMPAllProgram", "true");
                MPAllProgram = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsMP2Defaults", "true");
                MP2Defaults = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsMP2Config", "true");
                MP2Config = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsMP2Plugins", "true");
                MP2Plugins = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsMP2AllClientFolders", "true");
                MP2AllClientFolders = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsMP2AllClientProgramFolders", "true");
                MP2AllClientProgramFolders = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsMP2AllClientFiles", "true");
                MP2AllClientFiles = Convert.ToBoolean(setting.Value);
                
                setting = layer.GetSetting("Backup_SettingsSV2Configuration", "true");
                SV2Configuration = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSV2Database", "true");
                SV2Database = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSV2Plugins", "true");
                SV2Plugins = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSV2Defaults", "true");
                SV2Defaults = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSV2AllServerFolders", "true");
                SV2AllServerFolders = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSV2AllServerProgramFolders", "true");
                SV2AllServerProgramFolders = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSV2AllServerFiles", "true");
                SV2AllServerFiles = Convert.ToBoolean(setting.Value);

                //Duplicates
                setting = layer.GetSetting("Backup_SettingsDuplicateInteractive", "false");
                duplicateinteractive = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsDuplicateAutoprocess", "false");
                duplicateautoprocess = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsColumnSeparator", ";");
                BACKUPSETTINGS_COLUMN_SEPARATOR = setting.Value[0];

                //load longsettings 
                string listviewdata = load_longsettings("BackupSettings_ListView");
                ExtraFolderData = listviewdata.Split('\n');
                                
                //Load PB defaults
                PB_Export[(int)PB_part.MP_AllMediaPortalProgramFolders] = 1;
                PB_Export[(int)PB_part.MP_AllMediaPortalUserFolders] = 1;
                PB_Export[(int)PB_part.MP_Database] = 1;
                PB_Export[(int)PB_part.MP_InputDeviceMappings] = 1;
                PB_Export[(int)PB_part.MP_Language] = 1;
                PB_Export[(int)PB_part.MP_Plugins] = 1;
                PB_Export[(int)PB_part.MP_ProgramXML] = 1;
                PB_Export[(int)PB_part.MP_Skins] = 1;
                PB_Export[(int)PB_part.MP_Thumbs] = 1;
                PB_Export[(int)PB_part.MP_UserXML] = 1;

                PB_Export[(int)PB_part.MP2_AllClientFiles] = 1;
                PB_Export[(int)PB_part.MP2_AllClientFolders] = 1;
                PB_Export[(int)PB_part.MP2_AllClientProgramFolders] = 1;
                PB_Export[(int)PB_part.MP2_Config] = 1;
                PB_Export[(int)PB_part.MP2_Plugins] = 1;
                PB_Export[(int)PB_part.MP2_Defaults] = 1;

                PB_Export[(int)PB_part.SV2_AllServerFiles] = 1;
                PB_Export[(int)PB_part.SV2_AllServerFolders] = 1;
                PB_Export[(int)PB_part.SV2_AllServerProgramFolders] = 1;
                PB_Export[(int)PB_part.SV2_Config] = 1;
                PB_Export[(int)PB_part.SV2_Database] = 1;
                PB_Export[(int)PB_part.SV2_Plugins] = 1;
                PB_Export[(int)PB_part.SV2_Defaults] = 1;

                PB_Export[(int)PB_part.TV_TvServerProgramFolders] = 1;
                PB_Export[(int)PB_part.TV_TvServerUserFolders] = 1;
                PB_Export[(int)PB_part.TV_TvServerxml] = 1;
                PB_Export[(int)PB_part.DeleteOldFiles] = 1;
                PB_Export[(int)PB_part.ExtraFolders] = 1;

                setting = layer.GetSetting("Backup_SettingsPB_Export", "");
                //MessageBox.Show(setting.Value,"Backup_SettingsPB_Export");
                if (setting.Value != "")
                {
                    string[] array = setting.Value.Split('/');
                    //textoutput("LoadSetting:  Backup_SettingsPB_Export=" + setting.Value);
                    for (int i = 0; i <= PB_PARTNUMBERS; i++)
                    {
                        try
                        {
                            PB_Export[i] = Convert.ToInt32(array[i]);
                        }
                        catch
                        {
                            //textoutput("<RED>Error: PB_Export Conversion " + i.ToString() + " failed");  //unmark in later version
                        }
                    }
                }
                //not used for export
                PB_Export[(int)PB_part.MP_DeleteCache] = 0;
                PB_Export[(int)PB_part.MP_MusicPlayer] = 0;
                PB_Export[(int)PB_part.MP_Xmltv] = 0;
                PB_Export[(int)PB_part.TV_CardMappings] = 0;
                PB_Export[(int)PB_part.TV_DeleteAllChannels] = 0;
                PB_Export[(int)PB_part.TV_DeleteAllRadioGroups] = 0;
                PB_Export[(int)PB_part.TV_DeleteAllRecordings] = 0;
                PB_Export[(int)PB_part.TV_deleteAllSchedules] = 0;
                PB_Export[(int)PB_part.TV_DeleteAllTvGroups] = 0;
                PB_Export[(int)PB_part.TV_Channels] = 0;
                PB_Export[(int)PB_part.TV_EPG] = 0;
                PB_Export[(int)PB_part.TV_GeneralSettings] = 0;
                PB_Export[(int)PB_part.TV_PluginSettings] = 0;
                PB_Export[(int)PB_part.TV_Radiogroups] = 0;
                PB_Export[(int)PB_part.TV_Recordings] = 0;
                PB_Export[(int)PB_part.TV_Schedules] = 0;
                PB_Export[(int)PB_part.TV_Servers] = 0;
                PB_Export[(int)PB_part.TV_TvGroups] = 0;
                PB_Export[(int)PB_part.Start_TVServer] = 0;
                PB_Export[(int)PB_part.StopTvServer] = 0;
                PB_Export[(int)PB_part.TV_ConfigRestart] = 0;

                PB_Import[(int)PB_part.MP_AllMediaPortalProgramFolders] = 1;
                PB_Import[(int)PB_part.MP_AllMediaPortalUserFolders] = 1;
                PB_Import[(int)PB_part.MP_Database] = 1;
                PB_Import[(int)PB_part.MP_DeleteCache] = 1;
                PB_Import[(int)PB_part.MP_InputDeviceMappings] = 1;
                PB_Import[(int)PB_part.MP_Language] = 1;
                PB_Import[(int)PB_part.MP_Plugins] = 1;
                PB_Import[(int)PB_part.MP_ProgramXML] = 1;
                PB_Import[(int)PB_part.MP_Skins] = 1;
                PB_Import[(int)PB_part.MP_Thumbs] = 1;
                PB_Import[(int)PB_part.MP_Xmltv] = 1;
                PB_Import[(int)PB_part.MP_MusicPlayer] = 1;
                PB_Import[(int)PB_part.MP_UserXML] = 1;

                PB_Import[(int)PB_part.MP2_AllClientFiles] = 1;
                PB_Import[(int)PB_part.MP2_AllClientFolders] = 1;
                PB_Import[(int)PB_part.MP2_AllClientProgramFolders] = 1;
                PB_Import[(int)PB_part.MP2_Config] = 1;
                PB_Import[(int)PB_part.MP2_Plugins] = 1;
                PB_Import[(int)PB_part.MP2_Defaults] = 1;
                PB_Import[(int)PB_part.SV2_AllServerFiles] = 1;
                PB_Import[(int)PB_part.SV2_AllServerFolders] = 1;
                PB_Import[(int)PB_part.SV2_AllServerProgramFolders] = 1;
                PB_Import[(int)PB_part.SV2_Config] = 1;
                PB_Import[(int)PB_part.SV2_Database] = 1;
                PB_Import[(int)PB_part.SV2_Plugins] = 1;
                PB_Import[(int)PB_part.SV2_Defaults] = 1;

                PB_Import[(int)PB_part.TV_Channels] = 1;
                PB_Import[(int)PB_part.TV_DeleteAllChannels] = 1;
                PB_Import[(int)PB_part.TV_DeleteAllRadioGroups] = 1;
                PB_Import[(int)PB_part.TV_DeleteAllRecordings] = 1;
                PB_Import[(int)PB_part.TV_deleteAllSchedules] = 1;
                PB_Import[(int)PB_part.TV_DeleteAllTvGroups] = 1;
                PB_Import[(int)PB_part.TV_EPG] = 1;
                PB_Import[(int)PB_part.TV_GeneralSettings] = 1;
                PB_Import[(int)PB_part.TV_PluginSettings] = 1;
                PB_Import[(int)PB_part.TV_Radiogroups] = 1;
                PB_Import[(int)PB_part.TV_Recordings] = 1;
                PB_Import[(int)PB_part.TV_Schedules] = 1;
                PB_Import[(int)PB_part.TV_Servers] = 1;
                PB_Import[(int)PB_part.TV_TvGroups] = 1;
                PB_Import[(int)PB_part.TV_TvServerProgramFolders] = 1;
                PB_Import[(int)PB_part.TV_TvServerUserFolders] = 1;
                PB_Import[(int)PB_part.Start_TVServer] = 1;
                PB_Import[(int)PB_part.StopTvServer] = 1;
                PB_Import[(int)PB_part.ExtraFolders] = 1;



                setting = layer.GetSetting("Backup_SettingsPB_Import", "");
                //MessageBox.Show(setting.Value, "Backup_SettingsPB_Import");
                if (setting.Value != "")
                {
                    string[] array = setting.Value.Split('/');
                    //textoutput("LoadSetting: Backup_SettingsPB_Import="+setting.Value);
                    for (int i = 0; i <= PB_PARTNUMBERS; i++)
                    { 
                        try
                        {
                            PB_Import[i] = Convert.ToInt32(array[i]);
                        }
                        catch
                        {
                            //textoutput("<RED>Error: PB_Import Conversion " + i.ToString() + " failed");  //unmark in later version
                        }
                    }
                }

                //not used for import
                PB_Import[(int)PB_part.TV_TvServerxml] = 0;
                PB_Import[(int)PB_part.TV_CardMappings] = 0;
                PB_Import[(int)PB_part.DeleteOldFiles] = 0;
                PB_Import[(int)PB_part.TV_ConfigRestart] = 4;//4s fixed value for config restart


                setting = layer.GetSetting("Backup_SettingsPB_Duplicate", "");
                //MessageBox.Show(setting.Value, "Backup_SettingsPB_Duplicate");
                if (setting.Value != "")
                {
                    PB_Duplicate = Convert.ToInt32(setting.Value);
                }
            }
            catch (Exception ex)
            {                
                textoutput("<RED>Backup_SettingsSetup - MyLoadSettings(): " + ex.Message);
            }
        }

        public void MySaveSettings()
        {
            try
            {
                TvBusinessLayer layer = new TvBusinessLayer();
                Setting setting;

                setting = layer.GetSetting("Backup_SettingsPB_Export", "");
                setting.Value = PB_Export[0].ToString();
#if (PB)               
                textoutput("PB_Export[0]=" + PB_Export[0].ToString());
#endif
                for (int i = 1; i <= PB_PARTNUMBERS; i++)
                {

                    setting.Value += "/" + PB_Export[i].ToString();
#if (PB)
                    textoutput("PB_Export["+i+"]=" + PB_Export[i].ToString());
#endif
                }
                //textoutput("Savesettings: Backup_SettingsPB_Export=" + setting.Value);
                setting.Persist();
                

                setting = layer.GetSetting("Backup_SettingsPB_Import", "");
                setting.Value = PB_Import[0].ToString();
#if (PB)
                textoutput("PB_Import[0]=" + PB_Import[0].ToString());
#endif
                for (int i = 1; i <= PB_PARTNUMBERS; i++)
                {

                    setting.Value += "/" + PB_Import[i].ToString();
#if (PB)
                    textoutput("PB_Import[" + i + "]=" + PB_Import[i].ToString());
#endif
                }
                //textoutput("Savesettings: Backup_SettingsPB_Import=" + setting.Value);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsPB_Duplicate", "");
                setting.Value = PB_Duplicate.ToString();
#if (PB)
                textoutput("PB_Duplicate=" + PB_Duplicate.ToString());
#endif
                setting.Persist();
            }
            catch (Exception ex)
            {
                textoutput("<RED>Backup_SettingsImprtExport - MySaveSettings(): " + ex.Message);
            }
        }

        public string load_longsettings(string name)
        {

            //splits long setting strings in multiple parts
            TvBusinessLayer layer = new TvBusinessLayer();

            string stringdata = layer.GetSetting(name, "").Value;
            int count = 1;
            string partial = layer.GetSetting(name + "_" + count.ToString("D3"), "").Value;
            while (partial != "")
            {
                stringdata += partial;
                count++;
                partial = layer.GetSetting(name + "_" + count.ToString("D3"), "").Value;
                //textoutput("partial " + name + "_" + count.ToString("D3") + "=" + partial);
            }
            return stringdata;
        }


#else
        #region constructor
        public void ExportImport(ref ProgressBar myprogressBar, string configfile)
        {
            exportprogressBar = myprogressBar;
            xmlfilename = configfile;
        }

        #endregion constructor

        public void MyLoadSettings()
        {
            try
            {
                if (File.Exists(xmlfilename) == false)
                {
                    textoutput("Could not read settings in file "+xmlfilename+" - using default settings");
                    return;
                }

                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.Load(xmlfilename);
                }
                catch (Exception exc)
                {
                    textoutput("Error in loading BackupSettingsMP.xml file");

                    if (DEBUG == true)
                        textoutput("Exception message is:" + exc.Message);


                }
                XmlNodeList nodes = doc.SelectNodes("/BackupSettingsMP/node/setting");
                string PB_Export_string = "";
                string PB_Import_string = "";

                foreach (XmlNode node in nodes)
                {

                    DEBUG = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsSetup_debug", "false"));

                    //pathnames must have been defined before calling importexport!
                    TV_PROGRAM_FOLDER = getxmldata(doc, node, "Backup_SettingsSetup_TV_PROGRAM_FOLDER", "NOT_DEFINED");
                    TV_USER_FOLDER = getxmldata(doc, node, "Backup_SettingsSetup_TV_USER_FOLDER", "NOT_DEFINED");
                    MP_PROGRAM_FOLDER = getxmldata(doc, node, "Backup_SettingsSetup_MP_PROGRAM_FOLDER", "NOT_DEFINED");
                    MP_USER_FOLDER = getxmldata(doc, node, "Backup_SettingsSetup_MP_USER_FOLDER", "NOT_DEFINED");
                    MP2_PROGRAM_FOLDER = getxmldata(doc, node, "Backup_SettingsSetup_MP2_PROGRAM_FOLDER", "NOT_DEFINED");
                    MP2_USER_FOLDER = getxmldata(doc, node, "Backup_SettingsSetup_MP2_USER_FOLDER", "NOT_DEFINED");
                    SV2_PROGRAM_FOLDER = getxmldata(doc, node, "Backup_SettingsSetup_SV2_PROGRAM_FOLDER", "NOT_DEFINED");
                    SV2_USER_FOLDER = getxmldata(doc, node, "Backup_SettingsSetup_SV2_USER_FOLDER", "NOT_DEFINED");

                    instpaths.DEBUG = DEBUG;
                    instpaths.TV_PROGRAM_FOLDER = TV_PROGRAM_FOLDER;
                    instpaths.TV_USER_FOLDER = TV_USER_FOLDER;
                    instpaths.MP_PROGRAM_FOLDER = MP_PROGRAM_FOLDER;
                    instpaths.MP_USER_FOLDER = MP_USER_FOLDER;
                    instpaths.MP2_PROGRAM_FOLDER = MP2_PROGRAM_FOLDER;
                    instpaths.MP2_USER_FOLDER = MP2_USER_FOLDER;
                    instpaths.SV2_PROGRAM_FOLDER = SV2_PROGRAM_FOLDER;
                    instpaths.SV2_USER_FOLDER = SV2_USER_FOLDER;

                    instpaths.GetMediaPortalDirs();
                    instpaths.GetMediaPortalDirsMP2();


                   
                    pathname = getxmldata(doc, node, "Backup_SettingsSetup_filename", @"C:\MediaPortal Backups");

                    TV = false; //never TV server in client 
                    MP = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsSetup_MP", "true"));
                    MP2C = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsSetup_MP2C", "true"));
                    MP2S = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsSetup_MP2S", "true"));

                    MPDatabase = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsMPDatabase", "true"));
                    MPInputDevice = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsMPInputDevice", "true"));
                    MPPlugins = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsMPPlugins", "true"));
                    MPProgramXml = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsMPProgramXml", "true"));
                    MPSkins = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsMPSkins", "true"));
                    MPThumbs = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsMPThumbs", "true"));
                    MPUserXML = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsMPUserXML", "true"));
                    MPxmltv = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsMPxmltv", "true"));
                    MPMusicPlayer = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsMPMusicPlayer", "true"));
                    MPDeleteCache = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsMPDeleteCache", "true"));
                    MPAllFolder = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsMPAllFolders", "true"));
                    MPAllProgram = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsMPAllProgram", "true"));

                    MP2Defaults = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsMP2Defaults", "true"));
                    MP2Config = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsMP2Config", "true"));
                    MP2Plugins = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsMP2Plugins", "true"));
                    MP2AllClientFolders = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsMP2AllClientFolders", "true"));
                    MP2AllClientProgramFolders = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsMP2AllClientProgramFolders", "true"));
                    MP2AllClientFiles = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsMP2AllClientFiles", "true"));

                    SV2Configuration = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsSV2Configuration", "true"));
                    SV2Database = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsSV2Database", "true"));
                    SV2Plugins = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsSV2Plugins", "true"));
                    SV2Defaults = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsSV2Defaults", "true"));
                    SV2AllServerFolders = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsSV2AllServerFolders", "true"));
                    SV2AllServerProgramFolders = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsSV2AllServerProgramFolders", "true"));
                    SV2AllServerFiles = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsSV2AllServerFiles", "true"));

                    //listviewdata
                    BACKUPSETTINGS_COLUMN_SEPARATOR = Convert.ToChar(getxmldata(doc, node, "Backup_SettingsColumnSeparator", ";"));


                    string listviewdata = getxmldata(doc, node, "BackupSettings_ListView", "");
                    ExtraFolderData = listviewdata.Split('\n');

                    PB_Export_string = getxmldata(doc, node, "Backup_SettingsPB_Export", "");
                    PB_Import_string = getxmldata(doc, node, "Backup_SettingsPB_Import", "");


                }
                //Load PB defaults
                PB_Export[(int)PB_part.MP_AllMediaPortalProgramFolders] = 1;
                PB_Export[(int)PB_part.MP_AllMediaPortalUserFolders] = 1;
                PB_Export[(int)PB_part.MP_Database] = 1;
                PB_Export[(int)PB_part.MP_InputDeviceMappings] = 1;
                PB_Export[(int)PB_part.MP_Language] = 1;
                PB_Export[(int)PB_part.MP_Plugins] = 1;
                PB_Export[(int)PB_part.MP_ProgramXML] = 1;
                PB_Export[(int)PB_part.MP_Skins] = 1;
                PB_Export[(int)PB_part.MP_Thumbs] = 1;
                PB_Export[(int)PB_part.MP_UserXML] = 1;

                PB_Export[(int)PB_part.MP2_AllClientFiles] = 1;
                PB_Export[(int)PB_part.MP2_AllClientFolders] = 1;
                PB_Export[(int)PB_part.MP2_AllClientProgramFolders] = 1;
                PB_Export[(int)PB_part.MP2_Config] = 1;
                PB_Export[(int)PB_part.MP2_Plugins] = 1;
                PB_Export[(int)PB_part.MP2_Defaults] = 1;

                PB_Export[(int)PB_part.SV2_AllServerFiles] = 1;
                PB_Export[(int)PB_part.SV2_AllServerFolders] = 1;
                PB_Export[(int)PB_part.SV2_AllServerProgramFolders] = 1;
                PB_Export[(int)PB_part.SV2_Config] = 1;
                PB_Export[(int)PB_part.SV2_Database] = 1;
                PB_Export[(int)PB_part.SV2_Plugins] = 1;
                PB_Export[(int)PB_part.SV2_Defaults] = 1;

                PB_Export[(int)PB_part.TV_TvServerProgramFolders] = 1;
                PB_Export[(int)PB_part.TV_TvServerUserFolders] = 1;
                PB_Export[(int)PB_part.TV_TvServerxml] = 1;
                PB_Export[(int)PB_part.DeleteOldFiles] = 1;
                PB_Export[(int)PB_part.ExtraFolders] = 1;
                
                if (PB_Export_string != "")
                {
                    string[] array = PB_Export_string.Split('/');
                    for (int i = 0; i <= PB_PARTNUMBERS; i++)
                    {
                        try
                        {
                            PB_Export[i] = Convert.ToInt32(array[i]);
                        }
                        catch
                        {
                            //textoutput("<RED>Error: PB_Export Conversion " + i.ToString() + " failed");  unmark later
                        }
                    }
                }
                //not used for export
                PB_Export[(int)PB_part.MP_DeleteCache] = 0;
                PB_Export[(int)PB_part.MP_MusicPlayer] = 0;
                PB_Export[(int)PB_part.MP_Xmltv] = 0;
                PB_Export[(int)PB_part.TV_CardMappings] = 0;
                PB_Export[(int)PB_part.TV_DeleteAllChannels] = 0;
                PB_Export[(int)PB_part.TV_DeleteAllRadioGroups] = 0;
                PB_Export[(int)PB_part.TV_DeleteAllRecordings] = 0;
                PB_Export[(int)PB_part.TV_deleteAllSchedules] = 0;
                PB_Export[(int)PB_part.TV_DeleteAllTvGroups] = 0;
                PB_Export[(int)PB_part.TV_Channels] = 0;
                PB_Export[(int)PB_part.TV_EPG] = 0;
                PB_Export[(int)PB_part.TV_GeneralSettings] = 0;
                PB_Export[(int)PB_part.TV_PluginSettings] = 0;
                PB_Export[(int)PB_part.TV_Radiogroups] = 0;
                PB_Export[(int)PB_part.TV_Recordings] = 0;
                PB_Export[(int)PB_part.TV_Schedules] = 0;
                PB_Export[(int)PB_part.TV_Servers] = 0;
                PB_Export[(int)PB_part.TV_TvGroups] = 0;
                PB_Export[(int)PB_part.Start_TVServer] = 0;
                PB_Export[(int)PB_part.StopTvServer] = 0;
                PB_Export[(int)PB_part.TV_ConfigRestart] = 0;

                PB_Import[(int)PB_part.MP_AllMediaPortalProgramFolders] = 1;
                PB_Import[(int)PB_part.MP_AllMediaPortalUserFolders] = 1;
                PB_Import[(int)PB_part.MP_Database] = 1;
                PB_Import[(int)PB_part.MP_DeleteCache] = 1;
                PB_Import[(int)PB_part.MP_InputDeviceMappings] = 1;
                PB_Import[(int)PB_part.MP_Language] = 1;
                PB_Import[(int)PB_part.MP_Plugins] = 1;
                PB_Import[(int)PB_part.MP_ProgramXML] = 1;
                PB_Import[(int)PB_part.MP_Skins] = 1;
                PB_Import[(int)PB_part.MP_Thumbs] = 1;
                PB_Import[(int)PB_part.MP_Xmltv] = 1;
                PB_Import[(int)PB_part.MP_MusicPlayer] = 1;
                PB_Import[(int)PB_part.MP_UserXML] = 1;

                PB_Import[(int)PB_part.MP2_AllClientFiles] = 1;
                PB_Import[(int)PB_part.MP2_AllClientFolders] = 1;
                PB_Import[(int)PB_part.MP2_AllClientProgramFolders] = 1;
                PB_Import[(int)PB_part.MP2_Config] = 1;
                PB_Import[(int)PB_part.MP2_Plugins] = 1;
                PB_Import[(int)PB_part.MP2_Defaults] = 1;
                PB_Import[(int)PB_part.SV2_AllServerFiles] = 1;
                PB_Import[(int)PB_part.SV2_AllServerFolders] = 1;
                PB_Import[(int)PB_part.SV2_AllServerProgramFolders] = 1;
                PB_Import[(int)PB_part.SV2_Config] = 1;
                PB_Import[(int)PB_part.SV2_Database] = 1;
                PB_Import[(int)PB_part.SV2_Plugins] = 1;
                PB_Import[(int)PB_part.SV2_Defaults] = 1;

                PB_Import[(int)PB_part.TV_Channels] = 1;
                PB_Import[(int)PB_part.TV_DeleteAllChannels] = 1;
                PB_Import[(int)PB_part.TV_DeleteAllRadioGroups] = 1;
                PB_Import[(int)PB_part.TV_DeleteAllRecordings] = 1;
                PB_Import[(int)PB_part.TV_deleteAllSchedules] = 1;
                PB_Import[(int)PB_part.TV_DeleteAllTvGroups] = 1;
                PB_Import[(int)PB_part.TV_EPG] = 1;
                PB_Import[(int)PB_part.TV_GeneralSettings] = 1;
                PB_Import[(int)PB_part.TV_PluginSettings] = 1;
                PB_Import[(int)PB_part.TV_Radiogroups] = 1;
                PB_Import[(int)PB_part.TV_Recordings] = 1;
                PB_Import[(int)PB_part.TV_Schedules] = 1;
                PB_Import[(int)PB_part.TV_Servers] = 1;
                PB_Import[(int)PB_part.TV_TvGroups] = 1;
                PB_Import[(int)PB_part.TV_TvServerProgramFolders] = 1;
                PB_Import[(int)PB_part.TV_TvServerUserFolders] = 1;
                PB_Import[(int)PB_part.Start_TVServer] = 1;
                PB_Import[(int)PB_part.StopTvServer] = 1;
                PB_Import[(int)PB_part.ExtraFolders] = 1;
                if (PB_Import_string != "")
                {
                    string[] array = PB_Import_string.Split('/');
                    for (int i = 0; i <= PB_PARTNUMBERS; i++)
                    {
                        try
                        {
                            PB_Import[i] = Convert.ToInt32(array[i]);
                        }
                        catch
                        {
                            //textoutput("<RED>Error: PB_Import Conversion " + i.ToString() + " failed"); unmark later
                        }
                    }
                }

                //not used for import
                PB_Import[(int)PB_part.TV_TvServerxml] = 0;
                PB_Import[(int)PB_part.TV_CardMappings] = 0;
                PB_Import[(int)PB_part.DeleteOldFiles] = 0;
                PB_Import[(int)PB_part.TV_ConfigRestart] = 4;//4s fixed value for config restart
                
            }
            catch (Exception ex)
            {
                textoutput("<RED>BackupSettings ImportExport: - MyLoadSettings(): " + ex.Message);
            }
        }

        public void MySaveSettings()
        {
            try
            {
                if (DEBUG == true)
                    textoutput("ExpoprtImport: Saving Settings");

                XmlDocument xmlDoc = new XmlDocument();
                XmlNode rootElement = xmlDoc.CreateElement("BackupSettingsMP");
                XmlNode nodes = xmlDoc.CreateElement("node");
                XmlNode node = xmlDoc.CreateElement("setting");

                string PB_Export_string = PB_Export[0].ToString();
                if (DEBUG == true)
                {
                    textoutput("PB_Export[0]=" + PB_Export[0].ToString());
                }
                for (int i = 1; i <= PB_PARTNUMBERS; i++)
                {

                    PB_Export_string += "/" + PB_Export[i].ToString();
                    if (DEBUG == true)
                    {
                        textoutput("PB_Export[" + i + "]=" + PB_Export[i].ToString());
                    }
                }
                AddAttribute(node, "Backup_SettingsPB_Export", PB_Export_string);



                string PB_Import_string = PB_Import[0].ToString();
                if (DEBUG == true)
                {
                    textoutput("PB_Import[0]=" + PB_Import[0].ToString());
                }
                for (int i = 1; i <= PB_PARTNUMBERS; i++)
                {

                    PB_Import_string += "/" + PB_Import[i].ToString();
                    if (DEBUG == true)
                    {
                        textoutput("PB_Import[" + i + "]=" + PB_Import[i].ToString());
                    }
                }
                AddAttribute(node, "Backup_SettingsPB_Import", PB_Export_string);



                nodes.AppendChild(node);
                rootElement.AppendChild(nodes);
                xmlDoc.AppendChild(rootElement);
                xmlDoc.Save(xmlfilename);

                if (DEBUG)
                    textoutput("Setting file " + xmlfilename + " saved");
            }
            catch (Exception exc)
            {
                textoutput("BackupSettingsImortExport MySaveSettings: Fatal Error Exception:" + exc.Message);
                MessageBox.Show("BackupSettingsImortExport MySaveSettings: Fatal Error Exception:" + exc.Message);
            }
        }

        public String getxmldata(XmlDocument doc, XmlNode node, string attribute, string defaultvalue)
        {

            try
            {
                return node.Attributes[attribute].Value;
            }
            catch
            {
                return defaultvalue;
            }


        }

#endif
        
        public bool createbackup(string folderpath)
        {
            //loadsettings
            MyLoadSettings();

            //initialize progressbar
            progressbar((int)PB_action.INIT, ref PB_Export, 0);

            //warn user if default settings are not used
            bool defaultcheck = true;

            if (TV) //20 criteria
            {
                defaultcheck = defaultcheck && server && channels && channelcardmappings && tvgroups;
                defaultcheck = defaultcheck && radiogroups && schedules && !epg && recordings;
                defaultcheck = defaultcheck && general_settings && clickfinder && delete_channels && delete_tvgroups;
                defaultcheck = defaultcheck && delete_radiogroups && delete_schedules && delete_recordings && TVServerSettings;
                defaultcheck = defaultcheck && TVallPrograms && TVAllFolders && TVRestart && TVAutoCorrect;
            }

            if (MP) //12 criteria
            {
                defaultcheck = defaultcheck && MPDatabase && MPInputDevice && MPPlugins && MPProgramXml;
                defaultcheck = defaultcheck && MPSkins && MPThumbs && MPUserXML && MPxmltv;
                defaultcheck = defaultcheck && MPMusicPlayer && MPDeleteCache && MPAllFolder && MPAllProgram;
            }

            if (MP2C) //6 criteria
            {
                defaultcheck = defaultcheck && MP2Config && MP2Plugins && MP2AllClientFolders && MP2AllClientFiles;
                defaultcheck = defaultcheck && MP2Defaults && MP2AllClientProgramFolders;
            }

            if (MP2S) //7 criteria
            {
                defaultcheck = defaultcheck && SV2Configuration && SV2Database && SV2Plugins && SV2AllServerFolders;
                defaultcheck = defaultcheck && SV2AllServerFiles && SV2Defaults && SV2AllServerProgramFolders;
            }


            if (!defaultcheck && !SILENT)
            {
                switch (myMessageBox("You are not using the default filter settings for the backup - Only partial data will be exported.\n\n Do you want to continue?", "Info: Default filter settings not used", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                {
                    case DialogResult.Yes:
                        {
                            // "Yes" processing
                            textoutput("Default filter settings are not used for export");
                            break;
                        }
                    case DialogResult.No:
                        {
                            // "No" processing
                            textoutput("<RED>Export aborted by user because default filter settings are not used - click the Default button and repeat export");
                            return false;
                        }
                }
            }



            // delete old backup folders
            #region DeleteOldFolders
            if (!SILENT)
            {
                if ((Directory.Exists(folderpath + @"\TV_User")) || (Directory.Exists(folderpath + @"\TV_Program")) || (Directory.Exists(folderpath + @"\MP_User")) || (Directory.Exists(folderpath + @"\MP_Program")) || (Directory.Exists(folderpath + @"\SV2_User")) || (Directory.Exists(folderpath + @"\SV2_Program")) || (Directory.Exists(folderpath + @"\MP2_User")) || (Directory.Exists(folderpath + @"\MP2_Program")))
                {
                    switch (myMessageBox("Old backup data do exist - Do you want to delete all old backup data? \nThis will delete all data in\n" + folderpath + "\n\n Yes is recommended", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    {
                        case DialogResult.Yes:
                            {
                                // "Yes" processing
                                textoutput("Deleting old backup files");
                                exportprogressBar.Maximum += PB_Export[(int)PB_part.DeleteOldFiles];
#if PB
                                textoutput("progressBar.Maximum changed to " + exportprogressBar.Maximum.ToString());
#endif
                                progressbar((int)PB_action.START, ref PB_Export, (int)PB_part.DeleteOldFiles);


                                if (DeleteOldBackups(folderpath) == false)
                                    return false;


                                progressbar((int)PB_action.STOP, ref PB_Export, (int)PB_part.DeleteOldFiles);
                                break;
                            }
                        case DialogResult.No:
                            {
                                // "No" processing
                                textoutput("Old backup files not deleted - backup continued");
                                break;
                            }
                        case DialogResult.Cancel:
                            {
                                // "cancel" processing
                                textoutput("Old backup files not deleted - cancel backup");
                                return false;
                            }
                    }
                }

            }//end silent

            // will not delete old backup data if silent operation user must take care of that
            /*else
            {
                if (DeleteOldBackups(folderpath) == false)
                    return false;

            }*/
                
            

               

            // create new backup folder if it does not exist
            if (!Directory.Exists(folderpath))
            {
                try
                {
                    Directory.CreateDirectory(folderpath);
                }
                catch (Exception exc)
                {
                    textoutput("<RED>Could not create folder " + folderpath + " - Exception: " + exc.Message);
                    return false;
                }

            }
            #endregion DeleteOldFolders

            time1 = DateTime.Now;


            //handle extra folders
            progressbar((int)PB_action.START, ref PB_Export, (int)PB_part.ExtraFolders);
            BackupRestoreExtraFolders(true, folderpath);  //export=true means exporting data
            progressbar((int)PB_action.STOP, ref PB_Export, (int)PB_part.ExtraFolders);


            //**********************************
            // TVserver Backup
            //**********************************
            #region TVserver
#if (TV)
            
            if (TV == false)
            {
                textoutput("TV server settings not restored");
            }
            else
            {
                //check for installation folders

                if (Directory.Exists(TV_USER_FOLDER) == false)
                {
                    textoutput("<RED> TV server data folder does not exist - export aborted");
                    return false;
                }

                //program data after user data to avoid reinitialization issue 
                if (File.Exists(TV_PROGRAM_FOLDER + "\\TvService.exe") == false)
                {
                    textoutput("<RED> TV server program folder does not exist - export aborted");
                    return false;
                }

                // save settings before export after auto folder detection
                //MySaveSettings(); no more needed with paths

                // Ensure TV Program folder is current working directory
                System.Environment.CurrentDirectory = TV_PROGRAM_FOLDER;

                //read all version numbers for user warnings
                getallversionnumbers("",false);

                //repair Tv server database
                if (TVAutoCorrect)
                {
                    textoutput("Trying to repair TV server database before export");
                    backupscripts(folderpath, Scripts.AutoCorrect);                    
                }

                if (TVServerSettings == true)
                {
                    //document TV Server Version Number
                    string versionfile = folderpath + @"\Version.xml";
                    try
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        if (File.Exists(versionfile) == true)
                        {
                            xmlDoc.Load(versionfile);
                        }

                        XmlNode rootElement = xmlDoc.SelectSingleNode("/Version");
                        if (rootElement == null)
                        {
                            rootElement = xmlDoc.CreateElement("Version");
                        }

                        string plugversion = detectplugin("BackupSettings");
                        AddAttribute(rootElement, "BackupSettingsVersion", plugversion);

                        FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(TV_PROGRAM_FOLDER + @"\TvService.exe");
                        AddAttribute(rootElement, "TvServiceVersion", myFileVersionInfo.FileVersion.ToString());

                        xmlDoc.AppendChild(rootElement);
                        xmlDoc.Save(versionfile);
                        textoutput("TV server version numbers exported");
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not write TV Server version number to file " + versionfile);
                        if (DEBUG == true)
                        {
                            textoutput("<RED>Exception message is " + exc.Message);
                        }

                    }


                    textoutput("TV server setting export:");

                    try
                    {
                        if (!Directory.Exists(folderpath + @"\TV_User"))
                            Directory.CreateDirectory(folderpath + @"\TV_User");
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not create folder " + folderpath + @"\TV_User - Exception: " + exc.Message);
                        return false;
                    }

                    try
                    {
                        bool ok = Exportxml(folderpath + @"\TV_User\TVsettings.xml");
                        if (!ok)
                            return false;
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not export TV server settings to " + folderpath + @"\TV_User\TVsettings.xml - Exception: " + exc.Message);
                        return false;
                    }


                }

                //TV all program folders
                if (TVallPrograms == true)
                {
                    progressbar((int)PB_action.START, ref PB_Export, (int)PB_part.TV_TvServerProgramFolders);
                    try
                    {
                        if (!Directory.Exists(folderpath + @"\TV_Program"))
                            Directory.CreateDirectory(folderpath + @"\TV_Program");
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not create folder " + folderpath + @"\TV_Program - Exception: " + exc.Message);
                        return false;
                    }

                    DirectoryInfo root = new DirectoryInfo(TV_PROGRAM_FOLDER);

                    DirectoryInfo[] dirs = root.GetDirectories("*", SearchOption.TopDirectoryOnly);

                    foreach (DirectoryInfo subDir in dirs)
                    {
                        FileAttributes attributes = subDir.Attributes;

                        try
                        {
                            if ((subDir.Name != "BackupSettings") && (subDir.Name != "recordings") && (subDir.Name != "timeshiftbuffer") && (subDir.Name != "log") && ((attributes & FileAttributes.Hidden) != FileAttributes.Hidden) && ((attributes & FileAttributes.System) != FileAttributes.System))
                            { // do not copy hidden or system directories
                                textoutput("TV server " + subDir.Name + " backup:");
                                DirectoryCopy(TV_PROGRAM_FOLDER + @"\" + subDir.Name, folderpath + @"\TV_Program\" + subDir.Name, "*", true, DEBUG, true);
                            }
                        }
                        catch (Exception exc)
                        {
                            textoutput("<RED>Could not copy TV server setting folder " + TV_PROGRAM_FOLDER + @"\" + subDir.Name + " - Exception: " + exc.Message);
                            return false;
                        }
                    }


                    // TV Program .dll backup

                    textoutput("TV Server program .dll backup:");

                    try
                    {
                        if (!Directory.Exists(folderpath + @"\TV_Program"))
                            Directory.CreateDirectory(folderpath + @"\TV_Program");
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not create folder " + folderpath + @"\TV_Program - Exception: " + exc.Message);
                        return false;
                    }

                    try
                    {
                        DirectoryCopy(TV_PROGRAM_FOLDER, folderpath + @"\TV_Program", "*.dll", true, DEBUG, false);
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not copy files in program folder " + TV_PROGRAM_FOLDER + @" - Exception: " + exc.Message);
                        return false;
                    }
                    
                }

                progressbar((int)PB_action.STOP, ref PB_Export, (int)PB_part.TV_TvServerProgramFolders);
                

                //TV all data folders
                if (TVAllFolders == true)
                {
                    progressbar((int)PB_action.START, ref PB_Export, (int)PB_part.TV_TvServerUserFolders);
                    try
                    {
                        if (!Directory.Exists(folderpath + @"\TV_User"))
                            Directory.CreateDirectory(folderpath + @"\TV_User");
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not create folder " + folderpath + @"\TV_User - Exception: " + exc.Message);
                        return false;
                    }

                    DirectoryInfo root = new DirectoryInfo(TV_USER_FOLDER);

                    DirectoryInfo[] dirs = root.GetDirectories("*", SearchOption.TopDirectoryOnly);

                    foreach (DirectoryInfo subDir in dirs)
                    {
                        FileAttributes attributes = subDir.Attributes;

                        try
                        {
                            if ((subDir.Name != "recordings") && (subDir.Name != "timeshiftbuffer") && (subDir.Name != "log") && ((attributes & FileAttributes.Hidden) != FileAttributes.Hidden) && ((attributes & FileAttributes.System) != FileAttributes.System))
                            { // do not copy hidden or system directories
                                textoutput("TV server " + subDir.Name + " backup:");
                                DirectoryCopy(TV_USER_FOLDER + @"\" + subDir.Name, folderpath + @"\TV_User\" + subDir.Name, "*", true, DEBUG, true);
                            }
                        }
                        catch (Exception exc)
                        {
                            textoutput("<RED>Could not copy TV server setting folder " + TV_USER_FOLDER + @"\" + subDir.Name + " - Exception: " + exc.Message);
                            return false;
                        }
                    }
                    progressbar((int)PB_action.STOP, ref PB_Export, (int)PB_part.TV_TvServerUserFolders);
                }

                //user batch script TV export
                backupscripts(folderpath, Scripts.TV_Export);
                textoutput("TV server export completed");
            }
#endif
            #endregion TVserver


            //***************************************
            // MP  Backup
            //***************************************
            #region MediaPortal1
            if (MP == false)
            {
                textoutput("Media Portal settings not restored");
            }
            else
            {

                //check for installation folders

                if (File.Exists(MP_PROGRAM_FOLDER + "\\MediaPortal.exe") == false)
                {
                    textoutput("<RED> Media Portal program folder does not exist - export aborted");
                    return false;
                }

                if (Directory.Exists(MP_USER_FOLDER) == false)
                {
                    textoutput("<RED> Media Portal data folder does not exist - export aborted");
                    return false;
                }

                // save settings before export after auto folder detection
                //MySaveSettings();

                //read all version numbers for user warnings
                getallversionnumbers("", false);

                //read MediaPortalDirs.xml and extract installation paths 
                DIR_BurnerSupport = instpaths.DIR_BurnerSupport;
                DIR_Cache = instpaths.DIR_Cache;
                DIR_Config = instpaths.DIR_Config;
                DIR_CustomInputDefault = instpaths.DIR_CustomInputDefault;
                DIR_CustomInputDevice = instpaths.DIR_CustomInputDevice;
                DIR_Database = instpaths.DIR_Database;
                DIR_Language = instpaths.DIR_Language;
                DIR_Log = instpaths.DIR_Log;
                DIR_Plugins = instpaths.DIR_Plugins;
                DIR_Skin = instpaths.DIR_Skin;
                DIR_Thumbs = instpaths.DIR_Thumbs;
                DIR_Weather = instpaths.DIR_Weather;

                //document MediaPortal Version Number
                string versionfile = folderpath + @"\Version.xml";
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    if (File.Exists(versionfile) == true)
                    {
                        xmlDoc.Load(versionfile);
                    }

                    XmlNode rootElement = xmlDoc.SelectSingleNode("/Version");
                    if (rootElement == null)
                    {
                        rootElement = xmlDoc.CreateElement("Version");
                    }


                    AddAttribute(rootElement, "BackupSettingsVersion", pluginversion);

                    FileVersionInfo mydllFileVersionInfo = FileVersionInfo.GetVersionInfo(MP_PROGRAM_FOLDER + @"\MediaPortal.exe");
                    AddAttribute(rootElement, "MediaPortalVersion", mydllFileVersionInfo.FileVersion.ToString());

                    xmlDoc.AppendChild(rootElement);
                    xmlDoc.Save(versionfile);
                }
                catch (Exception exc)
                {
                    textoutput("<RED>Could not write Media Portal version number to file " + versionfile);
                    if (DEBUG == true)
                    {
                        textoutput("<RED>Exception message is " + exc.Message);
                    }

                }


                // MP Program .xml backup
                if (MPProgramXml == true)
                {
                    progressbar((int)PB_action.START, ref PB_Export, (int)PB_part.MP_ProgramXML);
                    textoutput("Media Portal program .xml backup:");

                    try
                    {
                        if (!Directory.Exists(folderpath + @"\MP_Program"))
                            Directory.CreateDirectory(folderpath + @"\MP_Program");
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not create folder " + folderpath + @"\MP_Program - Exception: " + exc.Message);
                        return false;
                    }

                    try
                    {
                        DirectoryCopy(MP_PROGRAM_FOLDER, folderpath + @"\MP_Program", "*.xml", true, DEBUG, false);
                        DirectoryCopy(MP_PROGRAM_FOLDER, folderpath + @"\MP_Program", "*.config", true, DEBUG, false);
                        DirectoryCopy(MP_PROGRAM_FOLDER, folderpath + @"\MP_Program", "*.ini", true, DEBUG, false);
                        DirectoryCopy(MP_PROGRAM_FOLDER, folderpath + @"\MP_Program", "*.sav", true, DEBUG, false);
                        DirectoryCopy(MP_PROGRAM_FOLDER, folderpath + @"\MP_Program", "*.dll", true, DEBUG, false);
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not copy files in program folder " + MP_PROGRAM_FOLDER + @" - Exception: " + exc.Message);
                        return false;
                    }
                    progressbar((int)PB_action.STOP, ref PB_Export, (int)PB_part.MP_ProgramXML);
                }



                //MP all program folders
                if (MPAllProgram == true)
                {
                    progressbar((int)PB_action.START, ref PB_Export, (int)PB_part.MP_AllMediaPortalProgramFolders);
                    try
                    {
                        if (!Directory.Exists(folderpath + @"\MP_Program"))
                            Directory.CreateDirectory(folderpath + @"\MP_Program");
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not create folder " + folderpath + @"\MP_Program - Exception: " + exc.Message);
                        return false;
                    }

                    DirectoryInfo root = new DirectoryInfo(MP_PROGRAM_FOLDER);

                    DirectoryInfo[] dirs = root.GetDirectories("*", SearchOption.TopDirectoryOnly);

                    foreach (DirectoryInfo subDir in dirs)
                    {
                        FileAttributes attributes = subDir.Attributes;


                        try
                        {
                            if ((subDir.Name.ToLower() != "plugins") && (subDir.Name.ToLower() != "musicplayer") && (subDir.Name.ToLower() != "skin") && (subDir.Name.ToLower() != "language") && (subDir.Name.ToLower() != "weather") && (subDir.Name.ToLower() != "burner") && (subDir.Name.ToLower() != "inputdevicemappings") && ((attributes & FileAttributes.Hidden) != FileAttributes.Hidden) && ((attributes & FileAttributes.System) != FileAttributes.System))
                            { // do not copy hidden or system directories
                                textoutput("Media Portal " + subDir.Name + " backup:");
                                DirectoryCopy(MP_PROGRAM_FOLDER + @"\" + subDir.Name, folderpath + @"\MP_Program\" + subDir.Name, "*", false, DEBUG, true); //overwrite verbose recursive
                            }
                        }
                        catch (Exception exc)
                        {
                            textoutput("<RED>Could not copy Media Portal setting folder " + MP_PROGRAM_FOLDER + @"\" + subDir.Name + " - Exception: " + exc.Message);
                            return false;
                        }

                    }

                    // backup MediaPortalDirs.xml folder


                    //DIR_CustomInputDefault
                    try
                    {
                        if (Directory.Exists(DIR_CustomInputDefault) == true)
                        {
                            textoutput(@"Media Portal InputDeviceMappings\defaults backup:");
                            DirectoryCopy(DIR_CustomInputDefault, folderpath + @"\MP_Program\InputDeviceMappings\defaults", "*", false, DEBUG, true); //overwrite verbose recursive
                        }
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not copy Media Portal setting folder " + DIR_CustomInputDefault + " - Exception: " + exc.Message);
                        return false;
                    }

                    //DIR_Weather
                    try
                    {
                        if (Directory.Exists(DIR_Weather) == true)
                        {
                            textoutput("Media Portal weather backup:");
                            DirectoryCopy(DIR_Weather, folderpath + @"\MP_Program\weather", "*", false, DEBUG, true); //overwrite verbose recursive
                        }
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not copy Media Portal setting folder " + DIR_Weather + " - Exception: " + exc.Message);
                        return false;
                    }
                    //DIR_BurnerSupport
                    try
                    {
                        if (Directory.Exists(DIR_BurnerSupport) == true)
                        {
                            textoutput("Media Portal burner backup:");
                            DirectoryCopy(DIR_BurnerSupport, folderpath + @"\MP_Program\Burner", "*", false, DEBUG, true); //overwrite verbose recursive
                        }
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not copy Media Portal setting folder " + DIR_BurnerSupport + " - Exception: " + exc.Message);
                        return false;
                    }

                    progressbar((int)PB_action.STOP, ref PB_Export, (int)PB_part.MP_AllMediaPortalProgramFolders);
                }





                // MP Plugin backup
                if (MPPlugins == true)
                {
                    progressbar((int)PB_action.START, ref PB_Export, (int)PB_part.MP_Plugins);
                    textoutput("Media Portal plugin backup:");
                    try
                    {
                        if (!Directory.Exists(folderpath + @"\MP_Program\plugins"))
                            Directory.CreateDirectory(folderpath + @"\MP_Program\plugins");
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not create folder " + folderpath + @"\MP_Program\plugins - Exception: " + exc.Message);
                        return false;
                    }

                    try
                    {
                        if (Directory.Exists(DIR_Plugins) == true)
                        {
                            DirectoryCopy(DIR_Plugins, folderpath + @"\MP_Program\plugins", "*", false, DEBUG, true); //no overwrite verbose recursive

                        }
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not copy plugins folder " + DIR_Plugins + " - Exception: " + exc.Message);
                        return false;
                    }
                    progressbar((int)PB_action.STOP, ref PB_Export, (int)PB_part.MP_Plugins);

                }

                // MP MusicPlayer backup
                if (MPMusicPlayer == true)
                {
                    textoutput("Media Portal MusicPlayer backup:");
                    try
                    {
                        if (!Directory.Exists(folderpath + @"\MP_Program\MusicPlayer"))
                        {
                            Directory.CreateDirectory(folderpath + @"\MP_Program\MusicPlayer");
                        }
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not create folder " + folderpath + @"\MP_Program\MusicPlayer - Exception: " + exc.Message);
                        return false;
                    }

                    try
                    {
                        if (Directory.Exists(MP_PROGRAM_FOLDER + @"\MusicPlayer") == true)
                            DirectoryCopy(MP_PROGRAM_FOLDER + @"\MusicPlayer", folderpath + @"\MP_Program\MusicPlayer", "*", true, DEBUG, true); //overwrite verbose recursive
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not copy MusicPlayer folder " + MP_PROGRAM_FOLDER + @"\MusicPlayer - Exception: " + exc.Message);
                        return false;
                    }

                }

                // MP Skins backup

                if (MPSkins == true)
                {
                    progressbar((int)PB_action.START, ref PB_Export, (int)PB_part.MP_Skins);
                    textoutput("Media Portal skin backup:");

                    string DestinationDir = null;
                    if (CompareVersions(ActualMediaPortalVersion, "1.0.3.0") == (int)COMPAREVERSION.NEWER)
                    {
                        DestinationDir = @"\MP_User";
                    }
                    else
                    {
                        DestinationDir = @"\MP_Program";
                    }


                    try
                    {
                        if (!Directory.Exists(folderpath + DestinationDir + @"\skin"))
                            Directory.CreateDirectory(folderpath + DestinationDir + @"\skin");
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not create folder " + folderpath + DestinationDir + @"\skin - Exception: " + exc.Message);
                        return false;
                    }
                    try
                    {
                        if (Directory.Exists(DIR_Skin) == true)
                            DirectoryCopy(DIR_Skin, folderpath + DestinationDir + @"\skin", "*", true, DEBUG, true);  //overwrite verbose recursive
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not copy skin folder " + DIR_Skin + " - Exception: " + exc.Message);
                        return false;
                    }
                    progressbar((int)PB_action.STOP, ref PB_Export, (int)PB_part.MP_Skins);

                    progressbar((int)PB_action.START, ref PB_Export, (int)PB_part.MP_Language);
                    try
                    {
                        if (!Directory.Exists(folderpath + DestinationDir + @"\language"))
                            Directory.CreateDirectory(folderpath + DestinationDir + @"\language");
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not create folder " + folderpath + DestinationDir + @"\language - Exception: " + exc.Message);
                        return false;
                    }
                    try
                    {
                        if (Directory.Exists(DIR_Language) == true)
                            DirectoryCopy(DIR_Language, folderpath + DestinationDir + @"\language", "*", true, DEBUG, true); //overwrite verbose recursive
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not copy language folder " + DIR_Language + " - Exception: " + exc.Message);
                        return false;
                    }
                    progressbar((int)PB_action.STOP, ref PB_Export, (int)PB_part.MP_Language);

                }



                // MP Data .xml backup
                if (MPUserXML == true)
                {
                    progressbar((int)PB_action.START, ref PB_Export, (int)PB_part.MP_UserXML);
                    textoutput("Media Portal data .xml file backup:");
                    try
                    {
                        if (!Directory.Exists(folderpath + @"\MP_User"))
                            Directory.CreateDirectory(folderpath + @"\MP_User");
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not create folder " + folderpath + @"\MP_User - Exception: " + exc.Message);
                        return false;
                    }


                    try
                    {
                        DirectoryCopy(DIR_Config, folderpath + @"\MP_User", "*.xml", true, DEBUG, false); //overwrite verbose recursive
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not copy user .xml data from " + DIR_Config + " - Exception: " + exc.Message);
                        return false;
                    }
                    progressbar((int)PB_action.STOP, ref PB_Export, (int)PB_part.MP_UserXML);
                }

                //MP all data folders
                if (MPAllFolder == true)
                {
                    progressbar((int)PB_action.START, ref PB_Export, (int)PB_part.MP_AllMediaPortalUserFolders);
                    try
                    {
                        if (!Directory.Exists(folderpath + @"\MP_User"))
                            Directory.CreateDirectory(folderpath + @"\MP_User");
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not create folder " + folderpath + @"\MP_User - Exception: " + exc.Message);
                        return false;
                    }

                    DirectoryInfo root = new DirectoryInfo(MP_USER_FOLDER);

                    DirectoryInfo[] dirs = root.GetDirectories("*", SearchOption.TopDirectoryOnly);

                    foreach (DirectoryInfo subDir in dirs)
                    {
                        FileAttributes attributes = subDir.Attributes;


                        try
                        {
                            if ((subDir.Name.ToLower() != "skin") && (subDir.Name.ToLower() != "language") && (subDir.Name != "xmltv") && (subDir.Name != "Cache") && (subDir.Name != "log") && (subDir.Name.ToLower() != "inputdevicemappings") && (subDir.Name.ToLower() != "database") && (subDir.Name.ToLower() != "thumbs") && ((attributes & FileAttributes.Hidden) != FileAttributes.Hidden) && ((attributes & FileAttributes.System) != FileAttributes.System))
                            { // do not copy hidden or system directories                               
                                textoutput("Media Portal " + subDir.Name + " backup:");
                                DirectoryCopy(MP_USER_FOLDER + @"\" + subDir.Name, folderpath + @"\MP_User\" + subDir.Name, "*", true, DEBUG, true); //overwrite verbose recursive
                            }
                        }
                        catch (Exception exc)
                        {
                            textoutput("<RED>Could not copy Media Portal setting folder " + MP_USER_FOLDER + @"\" + subDir.Name + " - Exception: " + exc.Message);
                            return false;
                        }

                    }
                    progressbar((int)PB_action.STOP, ref PB_Export, (int)PB_part.MP_AllMediaPortalUserFolders);

                }

                //MP database
                if (MPDatabase == true)
                {
                    progressbar((int)PB_action.START, ref PB_Export, (int)PB_part.MP_Database);
                    textoutput("Media Portal database backup:");

                    try
                    {
                        if (!Directory.Exists(folderpath + @"\MP_User\database"))
                            Directory.CreateDirectory(folderpath + @"\MP_User\database");
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not create folder " + folderpath + @"\MP_User\database - Exception: " + exc.Message);
                        return false;
                    }

                    try
                    {
                        if (Directory.Exists(DIR_Database) == true)
                        {
                            DirectoryCopy(DIR_Database, folderpath + @"\MP_User\database", "*", true, DEBUG, true); //overwrite verbose recursive
                        }
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not copy database folder " + DIR_Database + " - Exception: " + exc.Message);
                        return false;
                    }
                    progressbar((int)PB_action.STOP, ref PB_Export, (int)PB_part.MP_Database);

                }

                // MP Input Device Mapping backup
                if (MPInputDevice == true)
                {
                    progressbar((int)PB_action.START, ref PB_Export, (int)PB_part.MP_InputDeviceMappings);
                    if (Directory.Exists(DIR_CustomInputDevice))
                    {
                        textoutput("Media Portal input device mapping backup:");

                        try
                        {
                            if (!Directory.Exists(folderpath + @"\MP_User\InputDeviceMappings"))
                                Directory.CreateDirectory(folderpath + @"\MP_User\InputDeviceMappings");
                        }
                        catch (Exception exc)
                        {
                            textoutput("<RED>Could not create folder " + folderpath + @"\MP_User\InputDeviceMappings - Exception: " + exc.Message);
                            return false;
                        }

                        try
                        {
                            if (Directory.Exists(DIR_CustomInputDevice) == true)
                                DirectoryCopy(DIR_CustomInputDevice, folderpath + @"\MP_User\InputDeviceMappings", "*", true, DEBUG, true); //overwrite verbose recursive
                        }
                        catch (Exception exc)
                        {
                            textoutput("<RED>Could not copy Input Device Mapping folder " + DIR_CustomInputDevice + " - Exception: " + exc.Message);
                            return false;
                        }

                    }
                    else
                    {
                        textoutput("Media Portal input device mapping folder not found");
                    }
                    progressbar((int)PB_action.STOP, ref PB_Export, (int)PB_part.MP_InputDeviceMappings);
                }

                // MP Thumbs backup
                if (MPThumbs == true)
                {
                    progressbar((int)PB_action.START, ref PB_Export, (int)PB_part.MP_Thumbs);
                    textoutput("Media Portal thumbs folder backup:");

                    try
                    {
                        if (!Directory.Exists(folderpath + @"\MP_User\thumbs"))
                            Directory.CreateDirectory(folderpath + @"\MP_User\thumbs");
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not create folder " + folderpath + @"\MP_User\thumbs - Exception: " + exc.Message);
                        return false;
                    }

                    try
                    {
                        if (Directory.Exists(DIR_Thumbs) == true)
                            DirectoryCopy(DIR_Thumbs, folderpath + @"\MP_User\thumbs", "*", true, DEBUG, true); //overwrite verbose recursive
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>BackupSettings Error: Could not copy thumps folder " + DIR_Thumbs + " - Exception: " + exc.Message);
                        return false;
                    }
                    progressbar((int)PB_action.STOP, ref PB_Export, (int)PB_part.MP_Thumbs);

                }


                // MP xmltv backup
                if (MPxmltv == true)
                {
                    textoutput("Media Portal XmlTV backup:");

                    try
                    {
                        if (!Directory.Exists(folderpath + @"\MP_User\xmltv"))
                            Directory.CreateDirectory(folderpath + @"\MP_User\xmltv");
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not create folder " + folderpath + @"\MP_User\xmltv - Exception: " + exc.Message);
                        return false;
                    }

                    try
                    {
                        if (Directory.Exists(MP_USER_FOLDER + @"\xmltv") == true)
                            DirectoryCopy(MP_USER_FOLDER + @"\xmltv", folderpath + @"\MP_User\xmltv", "*", true, DEBUG, true); //overwrite verbose recursive
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>BackupSettings Error: Could not copy xmltv folder " + MP_USER_FOLDER + @"\xmltv - Exception: " + exc.Message);
                        return false;
                    }

                }


                //user batch script MP export
                backupscripts(folderpath, Scripts.MP_Export);
                textoutput("Media Portal export completed");
            }//end MP export

            #endregion MediaPortal1


            //*******************************
            //MP2 Server Backup
            //*******************************
            #region MP2Server
            if (MP2S == false)
            {
                textoutput("Media Portal2 Server settings not restored");
            }
            else
            {

                //check for installation folders

                if (File.Exists(SV2_PROGRAM_FOLDER + "\\MP2-Server.exe") == false)
                {
                    textoutput("<RED> Media Portal2 Server program folder does not exist - export aborted");
                    return false;
                }

                if (Directory.Exists(SV2_USER_FOLDER) == false)
                {
                    textoutput("<RED> Media Portal2 Server data folder does not exist - export aborted");
                    return false;
                }

                // save settings before export after auto folder detection
                //MySaveSettings();

                

                //read all version numbers for user warnings
                getallversionnumbers("", false);

                //document MediaPortal Version Number
                string versionfile = folderpath + @"\Version.xml";
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    if (File.Exists(versionfile) == true)
                    {
                        xmlDoc.Load(versionfile);
                    }

                    XmlNode rootElement = xmlDoc.SelectSingleNode("/Version");
                    if (rootElement == null)
                    {
                        rootElement = xmlDoc.CreateElement("Version");
                    }


                    AddAttribute(rootElement, "BackupSettingsVersion", pluginversion);

                    FileVersionInfo mydllFileVersionInfo = FileVersionInfo.GetVersionInfo(SV2_PROGRAM_FOLDER + @"\MP2-Server.exe");
                    AddAttribute(rootElement, "BackupMP2ServerVersion", mydllFileVersionInfo.FileVersion.ToString());

                    xmlDoc.AppendChild(rootElement);
                    xmlDoc.Save(versionfile);
                }
                catch (Exception exc)
                {
                    textoutput("<RED>Could not write Media Portal2 Server version number to file " + versionfile);
                    if (DEBUG == true)
                    {
                        textoutput("<RED>Exception message is " + exc.Message);
                    }

                }


                // Media Portal2 Server: All User Data Files Backup
                if (SV2AllServerFiles == true)
                {
                    progressbar((int)PB_action.START, ref PB_Export, (int)PB_part.SV2_AllServerFiles);
                    textoutput("Media Portal2 Server: Backing Up All User Data Files");

                    try
                    {
                        if (!Directory.Exists(folderpath + @"\SV2_USER"))
                            Directory.CreateDirectory(folderpath + @"\SV2_USER");
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not create folder " + folderpath + @"\SV2_USER - Exception: " + exc.Message);
                        return false;
                    }

                    try
                    {
                        DirectoryCopy(SV2_USER_FOLDER, folderpath + @"\SV2_USER", "*.*", true, DEBUG, false);
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not copy files in program folder " + SV2_USER_FOLDER + @" - Exception: " + exc.Message);
                        return false;
                    }
                    progressbar((int)PB_action.STOP, ref PB_Export, (int)PB_part.SV2_AllServerFiles);
                }



                //MP2 Server all user folders
                if (SV2AllServerFolders == true)
                {
                    progressbar((int)PB_action.START, ref PB_Export, (int)PB_part.SV2_AllServerFolders);
                    textoutput("Media Portal2 Server: Backing Up All User Data Folders");

                    try
                    {
                        if (!Directory.Exists(folderpath + @"\SV2_USER"))
                            Directory.CreateDirectory(folderpath + @"\SV2_USER");
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not create folder " + folderpath + @"\SV2_USER - Exception: " + exc.Message);
                        return false;
                    }

                    DirectoryInfo root = new DirectoryInfo(SV2_USER_FOLDER);

                    DirectoryInfo[] dirs = root.GetDirectories("*", SearchOption.TopDirectoryOnly);

                    foreach (DirectoryInfo subDir in dirs)
                    {
                        FileAttributes attributes = subDir.Attributes;


                        try
                        {
                            if ((subDir.Name.ToLower() != "plugins") && (subDir.Name.ToLower() != "config") && (subDir.Name.ToLower() != "log") && ((attributes & FileAttributes.Hidden) != FileAttributes.Hidden) && ((attributes & FileAttributes.System) != FileAttributes.System))
                            { // do not copy hidden or system directories
                                textoutput("Media Portal2 Server (User Data): " + subDir.Name + " Backup:");
                                DirectoryCopy(SV2_USER_FOLDER + @"\" + subDir.Name, folderpath + @"\SV2_USER\" + subDir.Name, "*", false, DEBUG, true); //overwrite verbose recursive
                            }
                        }
                        catch (Exception exc)
                        {
                            textoutput("<RED>Could not copy Media Portal2 Server folder " + SV2_USER_FOLDER + @"\" + subDir.Name + " - Exception: " + exc.Message);
                            return false;
                        }

                    }
                    progressbar((int)PB_action.STOP, ref PB_Export, (int)PB_part.SV2_AllServerFolders);

                }

                //MP2 Server all program folders
                if (SV2AllServerProgramFolders == true)
                {
                    progressbar((int)PB_action.START, ref PB_Export, (int)PB_part.SV2_AllServerProgramFolders);
                    textoutput("Media Portal2 Server: Backing Up All Program Folders");

                    try
                    {
                        if (!Directory.Exists(folderpath + @"\SV2_PROGRAM"))
                            Directory.CreateDirectory(folderpath + @"\SV2_PROGRAM");
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not create folder " + folderpath + @"\SV2_PROGRAM - Exception: " + exc.Message);
                        return false;
                    }

                    DirectoryInfo root = new DirectoryInfo(SV2_PROGRAM_FOLDER);

                    DirectoryInfo[] dirs = root.GetDirectories("*", SearchOption.TopDirectoryOnly);

                    foreach (DirectoryInfo subDir in dirs)
                    {
                        FileAttributes attributes = subDir.Attributes;

                        try
                        {
                            if ((subDir.Name.ToLower() != "plugins") && (subDir.Name.ToLower() != "config") && (subDir.Name.ToLower() != "defaults") && (subDir.Name.ToLower() != "log") && ((attributes & FileAttributes.Hidden) != FileAttributes.Hidden) && ((attributes & FileAttributes.System) != FileAttributes.System))
                            { // do not copy hidden or system directories
                                textoutput("Media Portal2 Server (Program Data): " + subDir.Name + " Backup:");
                                DirectoryCopy(SV2_PROGRAM_FOLDER + @"\" + subDir.Name, folderpath + @"\SV2_PROGRAM\" + subDir.Name, "*", false, DEBUG, true); //overwrite verbose recursive
                            }
                        }
                        catch (Exception exc)
                        {
                            textoutput("<RED>Could not copy Media Portal2 Server program folder " + SV2_PROGRAM_FOLDER + @"\" + subDir.Name + " - Exception: " + exc.Message);
                            return false;
                        }

                    }
                    progressbar((int)PB_action.STOP, ref PB_Export, (int)PB_part.SV2_AllServerProgramFolders);
                }

                //SV2 database
                if (SV2Database == true)
                {
                    progressbar((int)PB_action.START, ref PB_Export, (int)PB_part.SV2_Database);
                    textoutput("Media Portal2 Server Database Backup:");

                    try
                    {
                        if (!Directory.Exists(folderpath + @"\SV2_USER\database"))
                            Directory.CreateDirectory(folderpath + @"\SV2_USER\database");
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not create folder " + folderpath + @"\SV2_USER\database - Exception: " + exc.Message);
                        return false;
                    }

                    try
                    {
                        if (File.Exists(instpaths.DIR_SV2_Database + @"\Datastore.sdf") == true)
                        {
                            File.Copy(instpaths.DIR_SV2_Database + @"\Datastore.sdf", folderpath + @"\SV2_USER\database\Datastore.sdf", true);
                        }
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not copy database" + instpaths.DIR_SV2_Database + @"\Datastore.sdf" + " - Exception: " + exc.Message);
                        return false;
                    }
                    progressbar((int)PB_action.STOP, ref PB_Export, (int)PB_part.SV2_Database);

                }


                //SV2 plugins
                if (SV2Plugins == true)
                {
                    progressbar((int)PB_action.START, ref PB_Export, (int)PB_part.SV2_Plugins);
                    textoutput("Media Portal2 Server Plugins Backup:");

                    try
                    {
                        if (!Directory.Exists(folderpath + @"\SV2_PROGRAM\Plugins"))
                            Directory.CreateDirectory(folderpath + @"\SV2_PROGRAM\Plugins");
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not create folder " + folderpath + @"\SV2_PROGRAM\Plugins - Exception: " + exc.Message);
                        return false;
                    }

                    try
                    {
                        if (Directory.Exists(instpaths.DIR_SV2_Plugins) == true)
                        {
                            DirectoryCopy(instpaths.DIR_SV2_Plugins, folderpath + @"\SV2_PROGRAM\Plugins", "*", true, DEBUG, true); //overwrite verbose recursive
                        }
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not copy Plugins folder" + instpaths.DIR_SV2_Plugins + " - Exception: " + exc.Message);
                        return false;
                    }
                    progressbar((int)PB_action.STOP, ref PB_Export, (int)PB_part.SV2_Plugins);

                }

                //SV2 defaults
                if (MP2Defaults == true)
                {
                    progressbar((int)PB_action.START, ref PB_Export, (int)PB_part.SV2_Defaults);
                    textoutput("Media Portal2 Server Defaults Backup:");

                    try
                    {
                        if (!Directory.Exists(folderpath + @"\SV2_PROGRAM\Defaults"))
                            Directory.CreateDirectory(folderpath + @"\SV2_PROGRAM\Defaults");
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not create folder " + folderpath + @"\SV2_PROGRAM\Defaults - Exception: " + exc.Message);
                        return false;
                    }

                    try
                    {
                        if (Directory.Exists(instpaths.SV2_PROGRAM_FOLDER + @"\Defaults") == true)
                        {
                            DirectoryCopy(instpaths.SV2_PROGRAM_FOLDER + @"\Defaults", folderpath + @"\SV2_PROGRAM\Defaults", "*", true, DEBUG, true); //overwrite verbose recursive
                        }
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not copy Defaults folder" + instpaths.SV2_PROGRAM_FOLDER + @"\Defaults" + " - Exception: " + exc.Message);
                        return false;
                    }
                    progressbar((int)PB_action.STOP, ref PB_Export, (int)PB_part.SV2_Defaults);

                }

                //SV2 config
                if (SV2Configuration == true)
                {
                    progressbar((int)PB_action.START, ref PB_Export, (int)PB_part.SV2_Config);
                    textoutput("Media Portal2 Server Configuration Backup:");

                    try
                    {
                        if (!Directory.Exists(folderpath + @"\SV2_USER\Config"))
                            Directory.CreateDirectory(folderpath + @"\SV2_USER\Config");
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not create folder " + folderpath + @"\SV2_USER\Config - Exception: " + exc.Message);
                        return false;
                    }

                    try
                    {
                        if (Directory.Exists(instpaths.DIR_SV2_Config) == true)
                        {
                            DirectoryCopy(instpaths.DIR_SV2_Config, folderpath + @"\SV2_USER\Config", "*", true, DEBUG, true); //overwrite verbose recursive
                        }
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not copy Configuration folder" + instpaths.DIR_SV2_Config + " - Exception: " + exc.Message);
                        return false;
                    }
                    progressbar((int)PB_action.STOP, ref PB_Export, (int)PB_part.SV2_Config);
                }

            }//end MP2 Server Backup
            #endregion MP2Server

            //*******************************
            //MP2 Client Backup
            //*******************************
            #region MP2Client
            if (MP2C == false)
            {
                textoutput("Media Portal2 Client settings not restored");
            }
            else
            {

                //check for installation folders

                if (File.Exists(MP2_PROGRAM_FOLDER + "\\MP2-Client.exe") == false)
                {
                    textoutput("<RED> Media Portal2 Client program folder does not exist - export aborted");
                    return false;
                }

                if (Directory.Exists(MP2_USER_FOLDER) == false)
                {
                    textoutput("<RED> Media Portal2 Client data folder does not exist - export aborted");
                    return false;
                }

               
                //read all version numbers for user warnings
                getallversionnumbers("", false);

                

                //document MediaPortal Version Number
                string versionfile = folderpath + @"\Version.xml";
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    if (File.Exists(versionfile) == true)
                    {
                        xmlDoc.Load(versionfile);
                    }

                    XmlNode rootElement = xmlDoc.SelectSingleNode("/Version");
                    if (rootElement == null)
                    {
                        rootElement = xmlDoc.CreateElement("Version");
                    }


                    AddAttribute(rootElement, "BackupSettingsVersion", pluginversion);

                    FileVersionInfo mydllFileVersionInfo = FileVersionInfo.GetVersionInfo(MP2_PROGRAM_FOLDER + @"\MP2-Client.exe");
                    AddAttribute(rootElement, "BackupMP2ClientVersion", mydllFileVersionInfo.FileVersion.ToString());

                    xmlDoc.AppendChild(rootElement);
                    xmlDoc.Save(versionfile);
                }
                catch (Exception exc)
                {
                    textoutput("<RED>Could not write Media Portal2 Client version number to file " + versionfile);
                    if (DEBUG == true)
                    {
                        textoutput("<RED>Exception message is " + exc.Message);
                    }

                }


                // Media Portal2 Client: All User Data Files Backup
                if (MP2AllClientFiles == true)
                {
                    progressbar((int)PB_action.START, ref PB_Export, (int)PB_part.MP2_AllClientFiles);
                    textoutput("Media Portal2 Client: Backing Up All User Data Files");

                    try
                    {
                        if (!Directory.Exists(folderpath + @"\MP2_USER"))
                            Directory.CreateDirectory(folderpath + @"\MP2_USER");
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not create folder " + folderpath + @"\MP2_USER - Exception: " + exc.Message);
                        return false;
                    }

                    try
                    {
                        DirectoryCopy(MP2_USER_FOLDER, folderpath + @"\MP2_USER", "*.*", true, DEBUG, false);
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not copy files in program folder " + MP2_USER_FOLDER + @" - Exception: " + exc.Message);
                        return false;
                    }
                    progressbar((int)PB_action.STOP, ref PB_Export, (int)PB_part.MP2_AllClientFiles);
                }



                //MP2 Client all user folders
                if (MP2AllClientFolders == true)
                {
                    progressbar((int)PB_action.START, ref PB_Export, (int)PB_part.MP2_AllClientFolders);
                    textoutput("Media Portal2 Client: Backing Up All User Data Folders");

                    try
                    {
                        if (!Directory.Exists(folderpath + @"\MP2_USER"))
                            Directory.CreateDirectory(folderpath + @"\MP2_USER");
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not create folder " + folderpath + @"\MP2_USER - Exception: " + exc.Message);
                        return false;
                    }

                    DirectoryInfo root = new DirectoryInfo(MP2_USER_FOLDER);

                    DirectoryInfo[] dirs = root.GetDirectories("*", SearchOption.TopDirectoryOnly);

                    foreach (DirectoryInfo subDir in dirs)
                    {
                        FileAttributes attributes = subDir.Attributes;


                        try
                        {
                            if ((subDir.Name.ToLower() != "plugins") && (subDir.Name.ToLower() != "config") && (subDir.Name.ToLower() != "log") && ((attributes & FileAttributes.Hidden) != FileAttributes.Hidden) && ((attributes & FileAttributes.System) != FileAttributes.System))
                            { // do not copy hidden or system directories
                                textoutput("Media Portal2 Client (User Data): " + subDir.Name + " Backup:");
                                DirectoryCopy(MP2_USER_FOLDER + @"\" + subDir.Name, folderpath + @"\MP2_USER\" + subDir.Name, "*", false, DEBUG, true); //overwrite verbose recursive
                            }
                        }
                        catch (Exception exc)
                        {
                            textoutput("<RED>Could not copy Media Portal2 Client folder " + MP2_USER_FOLDER + @"\" + subDir.Name + " - Exception: " + exc.Message);
                            return false;
                        }

                    }
                    progressbar((int)PB_action.STOP, ref PB_Export, (int)PB_part.MP2_AllClientFolders);

                }

                //MP2 Client all program folders
                if (MP2AllClientProgramFolders == true)
                {
                    progressbar((int)PB_action.START, ref PB_Export, (int)PB_part.MP2_AllClientProgramFolders);
                    textoutput("Media Portal2 Client: Backing Up All Program Folders");

                    try
                    {
                        if (!Directory.Exists(folderpath + @"\MP2_PROGRAM"))
                            Directory.CreateDirectory(folderpath + @"\MP2_PROGRAM");
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not create folder " + folderpath + @"\MP2_PROGRAM - Exception: " + exc.Message);
                        return false;
                    }

                    DirectoryInfo root = new DirectoryInfo(MP2_PROGRAM_FOLDER);

                    DirectoryInfo[] dirs = root.GetDirectories("*", SearchOption.TopDirectoryOnly);

                    foreach (DirectoryInfo subDir in dirs)
                    {
                        FileAttributes attributes = subDir.Attributes;

                        try
                        {
                            if ((subDir.Name.ToLower() != "plugins") && (subDir.Name.ToLower() != "config") && (subDir.Name.ToLower() != "defaults") && (subDir.Name.ToLower() != "log") && ((attributes & FileAttributes.Hidden) != FileAttributes.Hidden) && ((attributes & FileAttributes.System) != FileAttributes.System))
                            { // do not copy hidden or system directories
                                textoutput("Media Portal2 Client (Program Data): " + subDir.Name + " Backup:");
                                DirectoryCopy(MP2_PROGRAM_FOLDER + @"\" + subDir.Name, folderpath + @"\MP2_PROGRAM\" + subDir.Name, "*", false, DEBUG, true); //overwrite verbose recursive
                            }
                        }
                        catch (Exception exc)
                        {
                            textoutput("<RED>Could not copy Media Portal2 Client Program folder " + MP2_PROGRAM_FOLDER + @"\" + subDir.Name + " - Exception: " + exc.Message);
                            return false;
                        }

                    }
                    progressbar((int)PB_action.STOP, ref PB_Export, (int)PB_part.SV2_AllServerProgramFolders);
                }




                //MP2 plugins
                if (MP2Plugins == true)
                {
                    progressbar((int)PB_action.START, ref PB_Export, (int)PB_part.MP2_Plugins);
                    textoutput("Media Portal2 Client Plugins Backup:");

                    try
                    {
                        if (!Directory.Exists(folderpath + @"\MP2_PROGRAM\Plugins"))
                            Directory.CreateDirectory(folderpath + @"\MP2_PROGRAM\Plugins");
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not create folder " + folderpath + @"\MP2_PROGRAM\Plugins - Exception: " + exc.Message);
                        return false;
                    }

                    try
                    {
                        if (Directory.Exists(instpaths.DIR_MP2_Plugins) == true)
                        {
                            DirectoryCopy(instpaths.DIR_MP2_Plugins, folderpath + @"\MP2_PROGRAM\Plugins", "*", true, DEBUG, true); //overwrite verbose recursive
                        }
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not copy Plugins folder" + instpaths.DIR_MP2_Plugins + " - Exception: " + exc.Message);
                        return false;
                    }
                    progressbar((int)PB_action.STOP, ref PB_Export, (int)PB_part.MP2_Plugins);

                }

                //MP2 defaults
                if (MP2Defaults == true)
                {
                    progressbar((int)PB_action.START, ref PB_Export, (int)PB_part.SV2_Defaults);
                    textoutput("Media Portal2 Client Defaults Backup:");

                    try
                    {
                        if (!Directory.Exists(folderpath + @"\MP2_PROGRAM\Defaults"))
                            Directory.CreateDirectory(folderpath + @"\MP2_PROGRAM\Defaults");
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not create folder " + folderpath + @"\MP2_PROGRAM\Defaults - Exception: " + exc.Message);
                        return false;
                    }

                    try
                    {
                        if (Directory.Exists(instpaths.MP2_PROGRAM_FOLDER + @"\Defaults") == true)
                        {
                            DirectoryCopy(instpaths.MP2_PROGRAM_FOLDER + @"\Defaults", folderpath + @"\MP2_PROGRAM\Defaults", "*", true, DEBUG, true); //overwrite verbose recursive
                        }
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not copy Defaults folder" + instpaths.MP2_PROGRAM_FOLDER + @"\Defaults" + " - Exception: " + exc.Message);
                        return false;
                    }
                    progressbar((int)PB_action.STOP, ref PB_Export, (int)PB_part.MP2_Defaults);

                }

                //MP2 config
                if (MP2Config == true)
                {
                    progressbar((int)PB_action.START, ref PB_Export, (int)PB_part.MP2_Config);
                    textoutput("Media Portal2 Client Configuration Backup:");

                    try
                    {
                        if (!Directory.Exists(folderpath + @"\MP2_USER\Config"))
                            Directory.CreateDirectory(folderpath + @"\MP2_USER\Config");
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not create folder " + folderpath + @"\MP2_USER\Config - Exception: " + exc.Message);
                        return false;
                    }

                    try
                    {
                        if (Directory.Exists(instpaths.DIR_MP2_Config) == true)
                        {
                            DirectoryCopy(instpaths.DIR_MP2_Config, folderpath + @"\MP2_USER\Config", "*", true, DEBUG, true); //overwrite verbose recursive
                        }
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Could not copy Configuration folder" + instpaths.DIR_MP2_Config + " - Exception: " + exc.Message);
                        return false;
                    }
                    progressbar((int)PB_action.STOP, ref PB_Export, (int)PB_part.MP2_Config);
                }

            }//end MP2 Client Backup
            #endregion MP2Client


            MySaveSettings(); //update progressbar for operation
            progressbar((int)PB_action.COMPLETE, ref PB_Export, 0);
            time2 = DateTime.Now;
            string text = "Export operation completed after " + time2.Subtract(time1).ToString();
            text = text.Substring(0, text.Length - 8);
            textoutput(text);


            return true;

        }

        public bool DeleteOldBackups(string folderpath)
        {
            try
            {
                if (Directory.Exists(folderpath))
                {
                    if (Directory.Exists(folderpath + @"\MP_Program"))
                    {
                        removeReadOnlyRecursive(folderpath + @"\MP_Program");
                        Directory.Delete(folderpath + @"\MP_Program", true);
                    }

                    if (Directory.Exists(folderpath + @"\MP_User"))
                    {
                        removeReadOnlyRecursive(folderpath + @"\MP_User");
                        Directory.Delete(folderpath + @"\MP_User", true);
                    }

                    if (Directory.Exists(folderpath + @"\TV_Program"))
                    {
                        removeReadOnlyRecursive(folderpath + @"\TV_Program");
                        Directory.Delete(folderpath + @"\TV_Program", true);
                    }

                    if (Directory.Exists(folderpath + @"\TV_User"))
                    {
                        removeReadOnlyRecursive(folderpath + @"\TV_User");
                        Directory.Delete(folderpath + @"\TV_User", true);
                    }

                    if (Directory.Exists(folderpath + @"\MP2_PROGRAM"))
                    {
                        removeReadOnlyRecursive(folderpath + @"\MP2_PROGRAM");
                        Directory.Delete(folderpath + @"\MP2_PROGRAM", true);
                    }

                    if (Directory.Exists(folderpath + @"\MP2_USER"))
                    {
                        removeReadOnlyRecursive(folderpath + @"\MP2_USER");
                        Directory.Delete(folderpath + @"\MP2_USER", true);
                    }

                    if (Directory.Exists(folderpath + @"\SV2_PROGRAM"))
                    {
                        removeReadOnlyRecursive(folderpath + @"\SV2_PROGRAM");
                        Directory.Delete(folderpath + @"\SV2_PROGRAM", true);
                    }

                    if (Directory.Exists(folderpath + @"\SV2_USER"))
                    {
                        removeReadOnlyRecursive(folderpath + @"\SV2_USER");
                        Directory.Delete(folderpath + @"\SV2_USER", true);
                    }

                    if (Directory.Exists(folderpath + @"\EXTRA_FOLDERS"))
                    {
                        removeReadOnlyRecursive(folderpath + @"\EXTRA_FOLDERS");
                        Directory.Delete(folderpath + @"\EXTRA_FOLDERS", true);
                    }

                    if (File.Exists(folderpath + @"\Version.xml"))
                    {
                        File.Delete(folderpath + @"\Version.xml");
                    }


                }

            }
            catch (Exception exc)
            {
                textoutput("<RED>Could not delete folder " + folderpath + " - Exception: " + exc.Message);
                return false;
            }
            return true;
        }



#if (TV)
        private bool Exportxml(string fileName)
        {
            // check if plugins are available
            bool ok = plugincheck();

            if (ok == false)
                return false;

            bool trouble = false;

            TvBusinessLayer layer = new TvBusinessLayer();
            Setting setting;
            int motorCount = 0;
            int serverCount = 0;
            int cardCount = 0;
            int channelCount = 0;
            int programCount = 0;
            int scheduleCount = 0;
            int recordingCount = 0;
            int channelGroupCount = 0;
            int radiochannelGroupCount = 0;
            int tvmovieCount = 0;
            //int cardgroupCount = 0;
            //int cardgroupmapCount = 0;

            progressbar((int)PB_action.START, ref PB_Export, (int)PB_part.TV_TvServerxml);
            textoutput("Exporting settings to " + fileName);

            //check for duplicate channel names
            if (duplicateautoprocess == false)
            {
                checkduplicatechannels(false, false); //verbose false messagebox false
                CheckDuplicateGroupmembers(false, false, false);//process false verbose false messagebox false
            }
            else
            {
                processduplicates(false);  //messagebox false
                if (duplicateinteractive == true)
                {
                    CheckDuplicateGroupmembers(true, true, false); //process true verbose true messagebox false
                }
                else
                {
                    CheckDuplicateGroupmembers(false, true, false); //process true verbose true messagebox false
                }

            }

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlNode rootElement = xmlDoc.CreateElement("tvserver");
                AddAttribute(rootElement, "Version", "1.0");

                XmlNode nodes = xmlDoc.CreateElement("versions");
                XmlNode node = xmlDoc.CreateElement("pluginversion");
                string plugversion = detectplugin("BackupSettings");
                AddAttribute(node, "backupSettingVersion", plugversion);
                FileVersionInfo mydllFileVersionInfo = FileVersionInfo.GetVersionInfo(TV_PROGRAM_FOLDER + @"\TvService.exe");
                AddAttribute(node, "TVServerVersion", mydllFileVersionInfo.FileVersion.ToString());
                nodes.AppendChild(node);
                rootElement.AppendChild(nodes);



                XmlNode nodeMotors = xmlDoc.CreateElement("DiSEqCmotors");
#if(MP100)
                IList motors = DiSEqCMotor.ListAll();
#elif(MP101)
                IList<DiSEqCMotor> motors = DiSEqCMotor.ListAll();
#else //MP11BETA or SVN
                IList<DiSEqCMotor> motors = DiSEqCMotor.ListAll();
#endif


                //Export DiSEqC motor settings
                textoutput("Exporting DiSEqC motor settings");
                foreach (DiSEqCMotor motor in motors)
                {
                    try
                    {
                        motorCount++;
                        XmlNode nodeMotor = xmlDoc.CreateElement("motor");
                        AddAttribute(nodeMotor, "IdDiSEqCMotor", motor.IdDiSEqCMotor.ToString());
                        AddAttribute(nodeMotor, "IdCard", motor.IdCard.ToString());
                        AddAttribute(nodeMotor, "IdSatellite", motor.IdSatellite.ToString());
                        AddAttribute(nodeMotor, "Position", motor.Position.ToString());
#if(MP100)
                        IList satellites = motor.ReferringSatellite(); 
#elif(MP101)
                        IList<Satellite> satellites = motor.ReferringSatellite();
#else //MP11BETA or SVN
                        IList<Satellite> satellites = motor.ReferringSatellite();
#endif

                        int count = 0;
                        foreach (Satellite satellite in satellites)
                        {
                            count++;
                            AddAttribute(nodeMotor, "SatelliteName", satellite.SatelliteName);
                            AddAttribute(nodeMotor, "TransponderFileName", satellite.TransponderFileName);
                            if (count > 1)
                            {
                                textoutput("<RED>More than one satellite found for satellite ID " + motor.IdSatellite);
                            }
                        }
                        if (count == 0)
                        {
                            textoutput("<RED>No satellite found for satellite ID " + motor.IdSatellite);
                        }



                        nodeMotors.AppendChild(nodeMotor);
                    }
                    catch (Exception exc)
                    {
                        motorCount--;
                        textoutput("<RED>DiSEqC motor setting" + motor.IdDiSEqCMotor + " could not be exported");
                        if (DEBUG == true)
                        {
                            textoutput("<RED>Exception message is " + exc.Message);
                        }
                        trouble = true;
                    }

                }
                textoutput(motorCount + " DiSEqC motor settings exported");
                rootElement.AppendChild(nodeMotors);

                XmlNode nodeServers = xmlDoc.CreateElement("servers");
#if(MP100)
                IList servers = Server.ListAll();
#elif(MP101)
                IList<Server> servers = Server.ListAll();
#else //MP11BETA or SVN
                IList<Server> servers = Server.ListAll();
#endif

                foreach (Server server in servers)
                {
                    textoutput("Exporting server settings");



                    serverCount++;
                    XmlNode nodeServer = xmlDoc.CreateElement("server");
                    AddAttribute(nodeServer, "HostName", server.HostName);
                    AddAttribute(nodeServer, "IdServer", (int)server.IdServer);
                    AddAttribute(nodeServer, "IsMaster", server.IsMaster);
#if(MP13)
                    try
                    {
                        AddAttribute(nodeServer, "RtspPort", (int)server.RtspPort);
                    }
                    catch //do nothing
                    {
                    }

#endif
                    
                    //card default settings
                    setting = layer.GetSetting("lnbDefault", "BACKUPSETTINGS_NOTFOUND");
                    if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                    {
                        AddAttribute(nodeServer, "lnbDefault", setting.Value);
                    }
                    else
                    {
                        if (DEBUG)
                            textoutput("Removing Setting " + setting.Tag + " Value=" + setting.Value);
                        setting.Remove();
                    }

                    setting = layer.GetSetting("LnbLowFrequency", "BACKUPSETTINGS_NOTFOUND");
                    if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                    {
                        AddAttribute(nodeServer, "LnbLowFrequency", setting.Value);
                    }
                    else
                    {
                        if (DEBUG)
                            textoutput("Removing Setting " + setting.Tag + " Value=" + setting.Value); 
                        setting.Remove();
                    }

                    setting = layer.GetSetting("LnbHighFrequency", "BACKUPSETTINGS_NOTFOUND");
                    if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                    {
                        AddAttribute(nodeServer, "LnbHighFrequency", setting.Value);
                    }
                    else
                    {
                        if (DEBUG)
                            textoutput("Removing Setting " + setting.Tag + " Value=" + setting.Value);
                        setting.Remove();
                    }

                    setting = layer.GetSetting("LnbSwitchFrequency", "BACKUPSETTINGS_NOTFOUND");
                    if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                    {
                        AddAttribute(nodeServer, "LnbSwitchFrequency", setting.Value);
                    }
                    else
                    {
                        if (DEBUG)
                            textoutput("Removing Setting " + setting.Tag + " Value=" + setting.Value);
                        setting.Remove();
                    }


                    textoutput("Exporting card settings");


                    XmlNode nodeCards = xmlDoc.CreateElement("cards");
#if(MP100)
                    IList cards = Card.ListAll();
#elif(MP101)
                    IList<Card> cards = Card.ListAll();
#else //MP11BETA or SVN
                    IList<Card> cards = Card.ListAll();
#endif

                    foreach (Card card in cards)
                    {
                        
                        cardCount++;
                        XmlNode nodeCard = xmlDoc.CreateElement("card");
                        AddAttribute(nodeCard, "IdCard", (int)card.IdCard);
                        AddAttribute(nodeCard, "DevicePath", card.DevicePath);
                        AddAttribute(nodeCard, "Enabled", card.Enabled);
#if(MP100)
                        //do nothing            
#elif(MP101) 
                        //do nothing            
#else //MP11BETA or SVN
                        AddAttribute(nodeCard, "CAM", card.CAM.ToString());
                        AddAttribute(nodeCard, "PreloadCard", card.PreloadCard);
                        AddAttribute(nodeCard, "netProvider", (int)card.netProvider); //bugfix: added16.08.2010
#endif

                        AddAttribute(nodeCard, "CamType", (int)card.CamType);
                        AddAttribute(nodeCard, "DecryptLimit", (int)card.DecryptLimit);
                        AddAttribute(nodeCard, "GrabEPG", card.GrabEPG);
                        AddAttribute(nodeCard, "LastEpgGrab", card.LastEpgGrab.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
                        AddAttribute(nodeCard, "Name", card.Name);
                        AddAttribute(nodeCard, "Priority", (int)card.Priority);
                        AddAttribute(nodeCard, "RecordingFolder", card.RecordingFolder);
                        AddAttribute(nodeCard, "RecordingFormat", (int)card.RecordingFormat);
                        AddAttribute(nodeCard, "TimeShiftFolder", card.TimeShiftFolder);

#if(MP13)
                        try
                        {
                            AddAttribute(nodeServer, "StopGraph", (bool)card.StopGraph);
                        }
                        catch //do nothing
                        {
                        }

#endif


                        //Export Tvlayer card settings for scanning

                        //Analog
                        setting = layer.GetSetting("analog" + card.IdCard.ToString() + "Country", "BACKUPSETTINGS_NOTFOUND");
                        if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                            AddAttribute(nodeCard, "analog" + card.IdCard.ToString() + "Country", setting.Value);

                        setting = layer.GetSetting("analog" + card.IdCard.ToString() + "Source", "BACKUPSETTINGS_NOTFOUND");
                        if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                            AddAttribute(nodeCard, "analog" + card.IdCard.ToString() + "Source", setting.Value);

                        //ATSC
                        setting = layer.GetSetting("atsc" + card.IdCard.ToString() + "supportsqam", "BACKUPSETTINGS_NOTFOUND");
                        if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                            AddAttribute(nodeCard, "atsc" + card.IdCard.ToString() + "supportsqam", setting.Value);

                        //DVBC
                        setting = layer.GetSetting("dvbc" + card.IdCard.ToString() + "Country", "BACKUPSETTINGS_NOTFOUND");
                        if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                            AddAttribute(nodeCard, "dvbc" + card.IdCard.ToString() + "Country", setting.Value);

                        setting = layer.GetSetting("dvbc" + card.IdCard.ToString() + "creategroups", "BACKUPSETTINGS_NOTFOUND");
                        if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                            AddAttribute(nodeCard, "dvbc" + card.IdCard.ToString() + "creategroups", setting.Value);

                        //DVBS
                        setting = layer.GetSetting("dvbs" + card.IdCard.ToString() + "SatteliteContext1", "BACKUPSETTINGS_NOTFOUND");
                        if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                            AddAttribute(nodeCard, "dvbs" + card.IdCard.ToString() + "SatteliteContext1", setting.Value);

                        setting = layer.GetSetting("dvbs" + card.IdCard.ToString() + "SatteliteContext2", "BACKUPSETTINGS_NOTFOUND");
                        if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                            AddAttribute(nodeCard, "dvbs" + card.IdCard.ToString() + "SatteliteContext2", setting.Value);

                        setting = layer.GetSetting("dvbs" + card.IdCard.ToString() + "SatteliteContext3", "BACKUPSETTINGS_NOTFOUND");
                        if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                            AddAttribute(nodeCard, "dvbs" + card.IdCard.ToString() + "SatteliteContext3", setting.Value);

                        setting = layer.GetSetting("dvbs" + card.IdCard.ToString() + "SatteliteContext4", "BACKUPSETTINGS_NOTFOUND");
                        if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                            AddAttribute(nodeCard, "dvbs" + card.IdCard.ToString() + "SatteliteContext4", setting.Value);

                        setting = layer.GetSetting("dvbs" + card.IdCard.ToString() + "DisEqc1", "BACKUPSETTINGS_NOTFOUND");
                        if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                            AddAttribute(nodeCard, "dvbs" + card.IdCard.ToString() + "DisEqc1", setting.Value);

                        setting = layer.GetSetting("dvbs" + card.IdCard.ToString() + "DisEqc2", "BACKUPSETTINGS_NOTFOUND");
                        if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                            AddAttribute(nodeCard, "dvbs" + card.IdCard.ToString() + "DisEqc2", setting.Value);

                        setting = layer.GetSetting("dvbs" + card.IdCard.ToString() + "DisEqc3", "BACKUPSETTINGS_NOTFOUND");
                        if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                            AddAttribute(nodeCard, "dvbs" + card.IdCard.ToString() + "DisEqc3", setting.Value);

                        setting = layer.GetSetting("dvbs" + card.IdCard.ToString() + "DisEqc4", "BACKUPSETTINGS_NOTFOUND");
                        if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                            AddAttribute(nodeCard, "dvbs" + card.IdCard.ToString() + "DisEqc4", setting.Value);

                        setting = layer.GetSetting("dvbs" + card.IdCard.ToString() + "band1", "BACKUPSETTINGS_NOTFOUND");
                        if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                            AddAttribute(nodeCard, "dvbs" + card.IdCard.ToString() + "band1", setting.Value);

                        setting = layer.GetSetting("dvbs" + card.IdCard.ToString() + "band2", "BACKUPSETTINGS_NOTFOUND");
                        if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                            AddAttribute(nodeCard, "dvbs" + card.IdCard.ToString() + "band2", setting.Value);

                        setting = layer.GetSetting("dvbs" + card.IdCard.ToString() + "band3", "BACKUPSETTINGS_NOTFOUND");
                        if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                            AddAttribute(nodeCard, "dvbs" + card.IdCard.ToString() + "band3", setting.Value);

                        setting = layer.GetSetting("dvbs" + card.IdCard.ToString() + "band4", "BACKUPSETTINGS_NOTFOUND");
                        if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                            AddAttribute(nodeCard, "dvbs" + card.IdCard.ToString() + "band4", setting.Value);

                        setting = layer.GetSetting("dvbs" + card.IdCard.ToString() + "LNB1", "BACKUPSETTINGS_NOTFOUND");
                        if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                            AddAttribute(nodeCard, "dvbs" + card.IdCard.ToString() + "LNB1", setting.Value);

                        setting = layer.GetSetting("dvbs" + card.IdCard.ToString() + "LNB2", "BACKUPSETTINGS_NOTFOUND");
                        if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                            AddAttribute(nodeCard, "dvbs" + card.IdCard.ToString() + "LNB2", setting.Value);

                        setting = layer.GetSetting("dvbs" + card.IdCard.ToString() + "LNB3", "BACKUPSETTINGS_NOTFOUND");
                        if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                            AddAttribute(nodeCard, "dvbs" + card.IdCard.ToString() + "LNB3", setting.Value);

                        setting = layer.GetSetting("dvbs" + card.IdCard.ToString() + "LNB4", "BACKUPSETTINGS_NOTFOUND");
                        if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                            AddAttribute(nodeCard, "dvbs" + card.IdCard.ToString() + "LNB4", setting.Value);

                        setting = layer.GetSetting("dvbs" + card.IdCard.ToString() + "LNB1", "BACKUPSETTINGS_NOTFOUND");
                        if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                            AddAttribute(nodeCard, "dvbs" + card.IdCard.ToString() + "LNB1", setting.Value);

                        setting = layer.GetSetting("dvbs" + card.IdCard.ToString() + "LNB2", "BACKUPSETTINGS_NOTFOUND");
                        if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                            AddAttribute(nodeCard, "dvbs" + card.IdCard.ToString() + "LNB2", setting.Value);

                        setting = layer.GetSetting("dvbs" + card.IdCard.ToString() + "LNB3", "BACKUPSETTINGS_NOTFOUND");
                        if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                            AddAttribute(nodeCard, "dvbs" + card.IdCard.ToString() + "LNB3", setting.Value);

                        setting = layer.GetSetting("dvbs" + card.IdCard.ToString() + "LNB4", "BACKUPSETTINGS_NOTFOUND");
                        if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                            AddAttribute(nodeCard, "dvbs" + card.IdCard.ToString() + "LNB4", setting.Value);

                        setting = layer.GetSetting("dvbs" + card.IdCard.ToString() + "creategroups", "BACKUPSETTINGS_NOTFOUND");
                        if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                            AddAttribute(nodeCard, "dvbs" + card.IdCard.ToString() + "creategroups", setting.Value);

                        setting = layer.GetSetting("dvbs" + card.IdCard.ToString() + "creategroupssat", "BACKUPSETTINGS_NOTFOUND");
                        if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                            AddAttribute(nodeCard, "dvbs" + card.IdCard.ToString() + "creategroupssat", setting.Value);

                        setting = layer.GetSetting("dvbs" + card.IdCard.ToString() + "createsignalgroup", "BACKUPSETTINGS_NOTFOUND");
                        if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                            AddAttribute(nodeCard, "dvbs" + card.IdCard.ToString() + "createsignalgroup", setting.Value);

                        setting = layer.GetSetting("dvbs" + card.IdCard.ToString() + "enabledvbs2", "BACKUPSETTINGS_NOTFOUND");
                        if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                            AddAttribute(nodeCard, "dvbs" + card.IdCard.ToString() + "enabledvbs2", setting.Value);

                        setting = layer.GetSetting("dvbs" + card.IdCard.ToString() + "limitsEnabled", "BACKUPSETTINGS_NOTFOUND");
                        if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                            AddAttribute(nodeCard, "dvbs" + card.IdCard.ToString() + "limitsEnabled", setting.Value);

                        setting = layer.GetSetting("dvbs" + card.IdCard.ToString() + "motorEnabled", "BACKUPSETTINGS_NOTFOUND");
                        if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                            AddAttribute(nodeCard, "dvbs" + card.IdCard.ToString() + "motorEnabled", setting.Value);

                        setting = layer.GetSetting("dvbs" + card.IdCard.ToString() + "motorStepSize", "BACKUPSETTINGS_NOTFOUND");
                        if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                            AddAttribute(nodeCard, "dvbs" + card.IdCard.ToString() + "motorStepSize", setting.Value);

                        setting = layer.GetSetting("dvbs" + card.IdCard.ToString() + "selectedMotorSat", "BACKUPSETTINGS_NOTFOUND");
                        if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                            AddAttribute(nodeCard, "dvbs" + card.IdCard.ToString() + "selectedMotorSat", setting.Value);


                        //DVBT
                        setting = layer.GetSetting("dvbt" + card.IdCard.ToString() + "Country", "BACKUPSETTINGS_NOTFOUND");
                        if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                            AddAttribute(nodeCard, "dvbt" + card.IdCard.ToString() + "Country", setting.Value);

                        setting = layer.GetSetting("dvbt" + card.IdCard.ToString() + "creategroups", "BACKUPSETTINGS_NOTFOUND");
                        if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                            AddAttribute(nodeCard, "dvbt" + card.IdCard.ToString() + "creategroups", setting.Value);


                        //DVBIP
                        setting = layer.GetSetting("dvbip" + card.IdCard.ToString() + "Service", "BACKUPSETTINGS_NOTFOUND");
                        if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                            AddAttribute(nodeCard, "dvbip" + card.IdCard.ToString() + "Service", setting.Value);

                        setting = layer.GetSetting("dvbip" + card.IdCard.ToString() + "creategroups", "BACKUPSETTINGS_NOTFOUND");
                        if ((setting.Value != "BACKUPSETTINGS_NOTFOUND") && (setting.Value != "NOTFOUND"))
                            AddAttribute(nodeCard, "dvbip" + card.IdCard.ToString() + "creategroups", setting.Value);

                        nodeCards.AppendChild(nodeCard);
                    }
                    nodeServer.AppendChild(nodeCards);
                    nodeServers.AppendChild(nodeServer);

                }



                textoutput(serverCount + " servers with " + cardCount + " card settings exported");


                rootElement.AppendChild(nodeServers);




                //Exporting Hybrid card settings
                textoutput("Exporting hybrid settings");
                int cardgroupCount = 0;


                XmlNode hybridnodeCardGroups = xmlDoc.CreateElement("HybridCardGroups");
#if(MP100)
                IList hybridcardgroups = CardGroup.ListAll();
#elif(MP101)
                IList<CardGroup> hybridcardgroups = CardGroup.ListAll();
#else //MP11BETA or SVN
                IList<CardGroup> hybridcardgroups = CardGroup.ListAll();
#endif

                foreach (CardGroup hybridcardgroup in hybridcardgroups)
                {
                    cardgroupCount++;
                    XmlNode hybridnodeCardGroup = xmlDoc.CreateElement("HybridCardGroup");
                    AddAttribute(hybridnodeCardGroup, "CardGroupName", hybridcardgroup.Name);
                    AddAttribute(hybridnodeCardGroup, "IdCardGroup", (int)hybridcardgroup.IdCardGroup);
                    hybridnodeCardGroups.AppendChild(hybridnodeCardGroup);
                }
                rootElement.AppendChild(hybridnodeCardGroups);

                int cardgroupmapCount = 0;
                XmlNode hybridnodeCardGroupMaps = xmlDoc.CreateElement("HybridCardGroupMaps");
#if(MP100)
                IList hybridcardgroupmaps = CardGroupMap.ListAll();
#elif(MP101)
                IList<CardGroupMap> hybridcardgroupmaps = CardGroupMap.ListAll();
#else //MP11BETA or SVN
                IList<CardGroupMap> hybridcardgroupmaps = CardGroupMap.ListAll();
#endif

                foreach (CardGroupMap hybridcardgroupmap in hybridcardgroupmaps)
                {


                    try
                    {
                        cardgroupmapCount++;
                        XmlNode hybridnodeCardGroupMap = xmlDoc.CreateElement("HybridCardGroupMap");
                        CardGroup tempgroup = CardGroup.Retrieve(hybridcardgroupmap.IdCardGroup);
                        AddAttribute(hybridnodeCardGroupMap, "HybridGroupName", tempgroup.Name);
                        AddAttribute(hybridnodeCardGroupMap, "IdCardGroup", (int)hybridcardgroupmap.IdCardGroup);
                        Card tempcard = Card.Retrieve(hybridcardgroupmap.IdCard);
                        AddAttribute(hybridnodeCardGroupMap, "CardName", tempcard.Name);
                        AddAttribute(hybridnodeCardGroupMap, "IdCard", (int)hybridcardgroupmap.IdCard);
                        AddAttribute(hybridnodeCardGroupMap, "IdMapping", (int)hybridcardgroupmap.IdMapping);
                        hybridnodeCardGroupMaps.AppendChild(hybridnodeCardGroupMap);
                    }
                    catch (Exception exc)
                    {
                        cardgroupmapCount--;
                        textoutput("<YELLOW>Warning: hybrid card group map " + hybridcardgroupmap.IdMapping + " not retrieved");
                        if (DEBUG == true)
                        {
                            textoutput("<YELLOW>     hybridcardgroupmap " + hybridcardgroupmap.IdCardGroup + " for cardid " + hybridcardgroupmap.IdCard + " mappingid " + hybridcardgroupmap.IdMapping + " not retrieved");
                            textoutput("<YELLOW>Exception message is " + exc.Message);
                        }
                        //trouble = true; removed not to confuse average user
                        //return false;
                    }
                }
                rootElement.AppendChild(hybridnodeCardGroupMaps);
                textoutput(cardgroupCount + " hybrid groups with " + cardgroupmapCount + " card mappings exported");
                /* end export hybrid*/




                // export channels
                XmlNode nodechannels = xmlDoc.CreateElement("channels");
#if(MP100)
                IList channels = Channel.ListAll();
#elif(MP101)
                IList<Channel> channels = Channel.ListAll();
#else //MP11BETA or SVN
                IList<Channel> channels = Channel.ListAll();
#endif

                textoutput("Exporting channel settings");


                foreach (Channel channel in channels)
                {
                    XmlNode nodechannel = xmlDoc.CreateElement("channel");
                    channelCount++;
#if(MP12)
#else
                    AddAttribute(nodechannel, "Name", channel.Name);
                    try
                    {
                        AddAttribute(nodechannel, "FreeToAir", channel.FreeToAir);
                    }
                    catch //do nothing
                    {
                    }
#endif


#if(MP13)
                    
                    try
                    {
                        AddAttribute(nodechannel, "ChannelNumber", channel.ChannelNumber);
                        AddAttribute(nodechannel, "ExternalId", channel.ExternalId);
                        

                        //Ilist string Groupnames
                        //Group CurrentGroup
                        //program CurrentProgram
                        //program NextProgram
                    }
                    catch //do nothing
                    {
                    }
#endif
                    AddAttribute(nodechannel, "GrabEpg", channel.GrabEpg);
                    AddAttribute(nodechannel, "IdChannel", (int)channel.IdChannel);
                    AddAttribute(nodechannel, "IsRadio", channel.IsRadio);
                    AddAttribute(nodechannel, "IsTv", channel.IsTv);
                    AddAttribute(nodechannel, "LastGrabTime", channel.LastGrabTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
                    AddAttribute(nodechannel, "SortOrder", (int)channel.SortOrder);
                    AddAttribute(nodechannel, "TimesWatched", (int)channel.TimesWatched);
                    AddAttribute(nodechannel, "TotalTimeWatched", channel.TotalTimeWatched.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
                    AddAttribute(nodechannel, "VisibleInGuide", channel.VisibleInGuide);

                    AddAttribute(nodechannel, "DisplayName", channel.DisplayName);
                    AddAttribute(nodechannel, "ExternalId", channel.ExternalId);
                    AddAttribute(nodechannel, "EpgHasGaps", channel.EpgHasGaps);

                    XmlNode nodeMaps = xmlDoc.CreateElement("mappings");
                    if (channelcardmappings == true) //filter settings
                    {
                        foreach (ChannelMap map in channel.ReferringChannelMap())
                        {
                            XmlNode nodeMap = xmlDoc.CreateElement("map");
                            AddAttribute(nodeMap, "IdCard", map.IdCard);
                            AddAttribute(nodeMap, "IdChannel", map.IdChannel);
                            AddAttribute(nodeMap, "IdChannelMap", map.IdChannelMap);
                            nodeMaps.AppendChild(nodeMap);
                        }
                        nodechannel.AppendChild(nodeMaps);
                    }
                    XmlNode nodeTuningDetails = xmlDoc.CreateElement("TuningDetails");
                    foreach (TuningDetail detail in channel.ReferringTuningDetail())
                    {
                        XmlNode nodeTune = xmlDoc.CreateElement("tune");
                        AddAttribute(nodeTune, "IdChannel", detail.IdChannel);
                        AddAttribute(nodeTune, "IdTuning", detail.IdTuning);
#if (MP12)
#else
                        AddAttribute(nodeTune, "AudioPid", detail.AudioPid);
                        AddAttribute(nodeTune, "PcrPid", detail.PcrPid);
                        AddAttribute(nodeTune, "VideoPid", (int)detail.VideoPid);                        
#endif
                        AddAttribute(nodeTune, "Bandwidth", detail.Bandwidth.ToString());
                        AddAttribute(nodeTune, "ChannelNumber", detail.ChannelNumber.ToString());
                        Log.Debug(detail.Name+" detail.ChannelNumber=" + detail.ChannelNumber.ToString());
                        AddAttribute(nodeTune, "ChannelType", detail.ChannelType.ToString());
                        AddAttribute(nodeTune, "CountryId", detail.CountryId.ToString());
                        AddAttribute(nodeTune, "Diseqc", detail.Diseqc.ToString());
                        AddAttribute(nodeTune, "FreeToAir", detail.FreeToAir.ToString());
                        AddAttribute(nodeTune, "Frequency", detail.Frequency.ToString());
                        AddAttribute(nodeTune, "MajorChannel", detail.MajorChannel.ToString());
                        AddAttribute(nodeTune, "MinorChannel", detail.MinorChannel.ToString());
                        AddAttribute(nodeTune, "Modulation", detail.Modulation.ToString());
                        AddAttribute(nodeTune, "Name", detail.Name);
                        AddAttribute(nodeTune, "NetworkId", detail.NetworkId.ToString());
                        AddAttribute(nodeTune, "PmtPid", detail.PmtPid.ToString());
                        AddAttribute(nodeTune, "Polarisation", detail.Polarisation.ToString());
                        AddAttribute(nodeTune, "Provider", detail.Provider.ToString());
                        AddAttribute(nodeTune, "ServiceId", detail.ServiceId.ToString());
                        AddAttribute(nodeTune, "SwitchingFrequency", detail.SwitchingFrequency.ToString());
                        AddAttribute(nodeTune, "Symbolrate", detail.Symbolrate.ToString());
                        AddAttribute(nodeTune, "TransportId", detail.TransportId.ToString());
                        AddAttribute(nodeTune, "TuningSource", detail.TuningSource.ToString());
                        AddAttribute(nodeTune, "VideoSource", detail.VideoSource.ToString());
#if(MP100)
                                        //do nothing
#elif(MP101) 
                                        //do nothing
#else //MP11BETA or SVN
                        AddAttribute(nodeTune, "AudioSource", (int)detail.AudioSource);
                        AddAttribute(nodeTune, "IsVCRSignal", (bool)detail.IsVCRSignal);
#endif

                        AddAttribute(nodeTune, "SatIndex", (int)detail.SatIndex);
                        AddAttribute(nodeTune, "InnerFecRate", (int)detail.InnerFecRate);
                        AddAttribute(nodeTune, "Band", (int)detail.Band);
                        AddAttribute(nodeTune, "Pilot", (int)detail.Pilot);
                        AddAttribute(nodeTune, "RollOff", (int)detail.RollOff);
                        AddAttribute(nodeTune, "Url", detail.Url);
                        AddAttribute(nodeTune, "Bitrate", detail.Bitrate);
                        nodeTuningDetails.AppendChild(nodeTune);
                    }
                    nodechannel.AppendChild(nodeTuningDetails);

                    nodechannels.AppendChild(nodechannel);
                }
                textoutput(channelCount + " channel settings exported");
                rootElement.AppendChild(nodechannels);





                // exporting the schedules
                XmlNode nodeSchedules = xmlDoc.CreateElement("schedules");
#if(MP100)
                IList schedules = Schedule.ListAll();
#elif(MP101)
                IList<Schedule> schedules = Schedule.ListAll();
#else //MP11BETA or SVN
                IList<Schedule> schedules = Schedule.ListAll();
#endif

                textoutput("Exporting schedule settings");


                foreach (Schedule schedule in schedules)
                {
                    try
                    {
                        XmlNode nodeSchedule = xmlDoc.CreateElement("schedule");
                        scheduleCount++;
                        Channel tmpchannel = schedule.ReferencedChannel();
                        if (tmpchannel == null)
                        {
                            textoutput("<RED>Channel could not be assigned for schedule id " + schedule.IdSchedule + " title " + schedule.ProgramName);
                        }
#if (MP12)
#else
                        AddAttribute(nodeSchedule, "ChannelName", tmpchannel.Name);
#endif
                        AddAttribute(nodeSchedule, "DisplayName", tmpchannel.DisplayName);
                        AddAttribute(nodeSchedule, "IdChannel", schedule.IdChannel);
                        AddAttribute(nodeSchedule, "IdSchedule", schedule.IdSchedule);
                        AddAttribute(nodeSchedule, "ProgramName", schedule.ProgramName);
                        AddAttribute(nodeSchedule, "StartTime", schedule.StartTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
                        AddAttribute(nodeSchedule, "EndTime", schedule.EndTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
                        AddAttribute(nodeSchedule, "KeepDate", schedule.KeepDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
                        AddAttribute(nodeSchedule, "PreRecordInterval", schedule.PreRecordInterval);
                        AddAttribute(nodeSchedule, "PostRecordInterval", schedule.PostRecordInterval);
                        AddAttribute(nodeSchedule, "Priority", schedule.Priority);
                        AddAttribute(nodeSchedule, "Quality", schedule.Quality);
                        AddAttribute(nodeSchedule, "Directory", schedule.Directory);
                        AddAttribute(nodeSchedule, "KeepMethod", schedule.KeepMethod);
                        AddAttribute(nodeSchedule, "MaxAirings", schedule.MaxAirings);
                        AddAttribute(nodeSchedule, "RecommendedCard", schedule.RecommendedCard);
                        AddAttribute(nodeSchedule, "ScheduleType", schedule.ScheduleType);
                        AddAttribute(nodeSchedule, "Series", schedule.Series);
                        AddAttribute(nodeSchedule, "DoesUseEpisodeManagement", schedule.DoesUseEpisodeManagement);
                        AddAttribute(nodeSchedule, "BitRateMode", (int)schedule.BitRateMode);
                        AddAttribute(nodeSchedule, "QualityType", (int)schedule.QualityType);
                        nodeSchedules.AppendChild(nodeSchedule);
                    }
                    catch (Exception exc)
                    {
                        scheduleCount--;
                        textoutput("<RED>Schedule for program " + schedule.ProgramName + " at " + schedule.StartTime + " till " + schedule.EndTime + " could not be exported");
                        if (DEBUG == true)
                        {
                            textoutput("<RED>Exception message is " + exc.Message);
                        }
                        //trouble = true;
                    }
                }
                textoutput(scheduleCount + " schedule settings exported");
                rootElement.AppendChild(nodeSchedules);

                /*if (epg == true)
                {*/
                // exporting the programs
                XmlNode nodePrograms = xmlDoc.CreateElement("programs");
#if(MP100)
                    IList programs = Program.ListAll();
#elif(MP101)
                IList<Program> programs = Program.ListAll();
#else //MP11BETA or SVN
                IList<Program> programs = Program.ListAll();
#endif

                textoutput("Exporting program settings");


                foreach (Program program in programs)
                {
                    try
                    {
                        XmlNode nodeProgram = xmlDoc.CreateElement("program");
                        programCount++;

                        AddAttribute(nodeProgram, "IdProgram", (int)program.IdProgram);
                        Channel tmpchannel = program.ReferencedChannel();
                        if (tmpchannel == null)
                        {
                            textoutput("<RED>Channel could not be assigned for program id " + (int)program.IdProgram + " title " + program.Title);
                        }
#if (MP12)
#else
                        AddAttribute(nodeProgram, "ChannelName", tmpchannel.Name);
#endif
                        AddAttribute(nodeProgram, "DisplayName", tmpchannel.DisplayName);
                        AddAttribute(nodeProgram, "IdChannel", program.IdChannel);

                        AddAttribute(nodeProgram, "Title", program.Title);
                        AddAttribute(nodeProgram, "Description", program.Description);
                        AddAttribute(nodeProgram, "Classification", program.Classification);

                        AddAttribute(nodeProgram, "EpisodeNum", program.EpisodeNum);
                        
#if(MP100)
                        //do nothing
#elif(MP101) 
                        //do nothing
#else //MP11BETA or SVN
                        AddAttribute(nodeProgram, "EpisodeName", program.EpisodeName);
                        AddAttribute(nodeProgram, "EpisodePart", program.EpisodePart);
                        AddAttribute(nodeProgram, "EpisodeNumber", program.EpisodeNumber);





#endif



                        AddAttribute(nodeProgram, "Genre", program.Genre);
                        AddAttribute(nodeProgram, "SeriesNum", program.SeriesNum);

                        AddAttribute(nodeProgram, "StartTime", program.StartTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
                        AddAttribute(nodeProgram, "EndTime", program.EndTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
                        AddAttribute(nodeProgram, "OriginalAirDate", program.OriginalAirDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));

                        AddAttribute(nodeProgram, "Notify", program.Notify.ToString());
                        AddAttribute(nodeProgram, "ParentalRating", (int)program.ParentalRating);
                        AddAttribute(nodeProgram, "StarRating", (int)program.StarRating);

                        nodePrograms.AppendChild(nodeProgram);
                    }
                    catch (Exception exc)
                    {
                        programCount--;
                        textoutput("<RED>Program title" + program.Title + " at " + program.StartTime + " till " + program.EndTime + " could not be exported");
                        if (DEBUG == true)
                        {
                            textoutput("<RED>Exception message is " + exc.Message);
                        }
                        //trouble = true;
                    }
                }
                textoutput(programCount + " program settings exported");
                rootElement.AppendChild(nodePrograms);

                //}


                // exporting the recordings
                XmlNode nodeRecordings = xmlDoc.CreateElement("recordings");
#if(MP100)
                IList recordings = Recording.ListAll();
#elif(MP101)
                IList<Recording> recordings = Recording.ListAll();
#else //MP11BETA or SVN
                IList<Recording> recordings = Recording.ListAll();
#endif

                textoutput("Exporting recording settings");


                foreach (Recording recording in recordings)
                {
                    try
                    {
                        XmlNode nodeRecording = xmlDoc.CreateElement("recording");
                        recordingCount++;

                        AddAttribute(nodeRecording, "IdRecording", (int)recording.IdRecording);
                        AddAttribute(nodeRecording, "FileName", recording.FileName);
                        AddAttribute(nodeRecording, "Title", recording.Title);
                        Channel tmpchannel = recording.ReferencedChannel();
                        if (tmpchannel == null)
                        {
                            textoutput("<RED>Channel could not be assigned for recording id " + (int)recording.IdRecording + " title " + recording.Title);


                        }
                        else
                        {
                            AddAttribute(nodeRecording, "DisplayName", tmpchannel.DisplayName);
                        }

                        
                        AddAttribute(nodeRecording, "IdChannel", recording.IdChannel);

                        Server tmpserver = recording.ReferencedServer();
                        if (tmpserver == null)
                        {
                            textoutput("<RED>Server could not be assigned for recording id " + (int)recording.IdRecording + " title " + recording.Title);
                        }

                        AddAttribute(nodeRecording, "ServerName", tmpserver.HostName);
                        AddAttribute(nodeRecording, "IdServer", recording.IdServer);

                        AddAttribute(nodeRecording, "StartTime", recording.StartTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
                        AddAttribute(nodeRecording, "EndTime", recording.EndTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
                        AddAttribute(nodeRecording, "StopTime", (int)recording.StopTime);
                        AddAttribute(nodeRecording, "KeepUntilDate", recording.KeepUntilDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
                        AddAttribute(nodeRecording, "KeepUntil", (int)recording.KeepUntil);
                        AddAttribute(nodeRecording, "TimesWatched", (int)recording.TimesWatched);

                        AddAttribute(nodeRecording, "Description", recording.Description);
                        AddAttribute(nodeRecording, "Genre", recording.Genre);

#if(SVN)
                        AddAttribute(nodeRecording, "EpisodeNum", recording.EpisodeNum);
                        AddAttribute(nodeRecording, "EpisodeNumber", recording.EpisodeNumber);
                        AddAttribute(nodeRecording, "EpisodePart", recording.EpisodePart);
                        AddAttribute(nodeRecording, "EpisodeName", recording.EpisodeName);
                        AddAttribute(nodeRecording, "SeriesNum", recording.SeriesNum);


                        AddAttribute(nodeRecording, "IdSchedule", recording.Idschedule.ToString());
                        AddAttribute(nodeRecording, "IsRecording", recording.IsRecording.ToString());




#endif

                        nodeRecordings.AppendChild(nodeRecording);

                    }
                    catch (Exception exc)
                    {
                        recordingCount--;
                        textoutput("<RED>Recording for File " + recording.FileName + " could not be exported");
                        if (DEBUG == true)
                        {
                            textoutput("<RED>Exception message is " + exc.Message);
                        }
                        //trouble = true;
                    }
                }
                textoutput(recordingCount + " recording settings exported");


                rootElement.AppendChild(nodeRecordings);


                //exporting channel groups
                XmlNode nodeChannelGroups = xmlDoc.CreateElement("channelgroups");
#if(MP100)
                IList channelgroups = ChannelGroup.ListAll();
#elif(MP101)
                IList<ChannelGroup> channelgroups = ChannelGroup.ListAll();
#else //MP11BETA or SVN
                IList<ChannelGroup> channelgroups = ChannelGroup.ListAll();
#endif

                textoutput("Exporting channel group settings");


                foreach (ChannelGroup group in channelgroups)
                {
                    XmlNode nodeChannelGroup = xmlDoc.CreateElement("channelgroup");
                    channelGroupCount++;
                    AddAttribute(nodeChannelGroup, "IdGroup", group.IdGroup);
                    AddAttribute(nodeChannelGroup, "GroupName", group.GroupName);
                    AddAttribute(nodeChannelGroup, "SortOrder", group.SortOrder.ToString());
                    XmlNode nodeGroupMap = xmlDoc.CreateElement("mappings");
#if(MP100)
                    IList maps = group.ReferringGroupMap();
#elif(MP101)
                    IList<GroupMap> maps = group.ReferringGroupMap();
#else //MP11BETA or SVN
                    IList<GroupMap> maps = group.ReferringGroupMap();
#endif

                    int mapcount = 0;
                    foreach (GroupMap map in maps)
                    {
                        try
                        {
                            mapcount++;
                            XmlNode nodeMap = xmlDoc.CreateElement("map");
                            Channel channel = map.ReferencedChannel();
                            if (channel == null)
                            {
                                textoutput("<RED>Channel could not be assigned for tvmapping id " + (int)map.IdMap);
                            }
                            AddAttribute(nodeMap, "IdMap", map.IdMap);
#if (MP12)
#else 
                            AddAttribute(nodeMap, "ChannelName", channel.Name);
#endif
                            AddAttribute(nodeMap, "DisplayName", channel.DisplayName);
                            AddAttribute(nodeMap, "SortOrder", map.SortOrder.ToString());
                            nodeGroupMap.AppendChild(nodeMap);
                        }
                        catch (Exception exc)
                        {
                            mapcount--;
                            textoutput("<YELLOW>TV group map " + map.IdMap + " not retrieved");

                            if (DEBUG == true)
                            {
                                textoutput("<YELLOW>TV group map " + map.IdMap + " for channel ID " + map.IdChannel + " in TVGroup ID " + map.IdGroup + " could not be exported");
                                textoutput("<YELLOW>Exception message is " + exc.Message);
                            }
                            //trouble = true; removed not to confuse average user
                        }

                    }
                    textoutput(mapcount + " channels exported to group " + group.GroupName);
                    nodeChannelGroup.AppendChild(nodeGroupMap);
                    nodeChannelGroups.AppendChild(nodeChannelGroup);
                }
                textoutput(channelGroupCount + " channel group settings exported");


                rootElement.AppendChild(nodeChannelGroups);


                //exporting radio channel groups
                XmlNode RadionodeChannelGroups = xmlDoc.CreateElement("radiochannelgroups");
#if(MP100)
                IList radiochannelgroups = RadioChannelGroup.ListAll();
#elif(MP101)
                IList<RadioChannelGroup> radiochannelgroups = RadioChannelGroup.ListAll();
#else //MP11BETA or SVN
                IList<RadioChannelGroup> radiochannelgroups = RadioChannelGroup.ListAll();
#endif

                textoutput("Exporting radio channel group settings");


                foreach (RadioChannelGroup radiogroup in radiochannelgroups)
                {
                    radiochannelGroupCount++;
                    XmlNode nodeRadioChannelGroup = xmlDoc.CreateElement("radiochannelgroup");
                    AddAttribute(nodeRadioChannelGroup, "IdGroup", radiogroup.IdGroup);
                    AddAttribute(nodeRadioChannelGroup, "GroupName", radiogroup.GroupName);
                    AddAttribute(nodeRadioChannelGroup, "SortOrder", radiogroup.SortOrder.ToString());
                    XmlNode nodeRadioGroupMap = xmlDoc.CreateElement("mappings");
#if(MP100)
                    IList radiomaps = radiogroup.ReferringRadioGroupMap();
#elif(MP101)
                    IList<RadioGroupMap> radiomaps = radiogroup.ReferringRadioGroupMap();
#else //MP11BETA or SVN
                    IList<RadioGroupMap> radiomaps = radiogroup.ReferringRadioGroupMap();
#endif

                    int mapcount = 0;
                    foreach (RadioGroupMap radiomap in radiomaps)
                    {
                        try
                        {
                            mapcount++;
                            XmlNode radionodeMap = xmlDoc.CreateElement("radiomap");
                            Channel channel = radiomap.ReferencedChannel();
                            if (channel == null)
                            {
                                textoutput("<RED>Channel could not be assigned for radiomapping id " + (int)radiomap.IdMap);
                            }
                            AddAttribute(radionodeMap, "IdMap", radiomap.IdMap);
#if (MP12)
#else 
                            AddAttribute(radionodeMap, "ChannelName", channel.Name);
#endif
                            AddAttribute(radionodeMap, "DisplayName", channel.DisplayName);
                            AddAttribute(radionodeMap, "SortOrder", radiomap.SortOrder.ToString());
                            nodeRadioGroupMap.AppendChild(radionodeMap);
                        }
                        catch (Exception exc)
                        {
                            mapcount--;
                            textoutput("<YELLOW>Radio group map " + radiomap.IdMap + " not retrieved");

                            if (DEBUG == true)
                            {
                                textoutput("<YELLOW>Radio group map " + radiomap.IdMap + " for channel ID " + radiomap.IdChannel + " in RadioGroup ID " + radiomap.IdGroup + " could not be exported");
                                textoutput("<YELLOW>Exception message is " + exc.Message);
                            }
                            //trouble = true; removed not to confuse average user
                        }
                    }
                    textoutput(mapcount + " channels exported to group " + radiogroup.GroupName);
                    nodeRadioChannelGroup.AppendChild(nodeRadioGroupMap);
                    RadionodeChannelGroups.AppendChild(nodeRadioChannelGroup);
                }

                textoutput(radiochannelGroupCount + " radio channel group settings exported");
                rootElement.AppendChild(RadionodeChannelGroups);







                //exporting TVMovie mappings
                XmlNode TVMovieMappings = xmlDoc.CreateElement("TVMovieMappings");
                textoutput("Exporting TV movie mappings");

                try
                {
#if(MP100)
                        IList mappingDb = TvMovieMapping.ListAll();
#elif(MP101)
                        IList<TvMovieMapping> mappingDb = TvMovieMapping.ListAll();
#else //MP11BETA or SVN
                    IList<TvMovieMapping> mappingDb = TvMovieMapping.ListAll();

#endif

                    if (mappingDb != null)
                    {
                        if (mappingDb.Count > 0)
                        {
                            foreach (TvMovieMapping mapping in mappingDb)
                            {
                                
                                try
                                {
                                    tvmovieCount++;
                                    XmlNode TVMovieMapping = xmlDoc.CreateElement("TVMovieMapping");
                                    AddAttribute(TVMovieMapping, "IdChannel", mapping.IdChannel.ToString());
                                    AddAttribute(TVMovieMapping, "IdMapping", mapping.IdMapping.ToString());
                                    AddAttribute(TVMovieMapping, "StationName", mapping.StationName);
                                    AddAttribute(TVMovieMapping, "TimeSharingEnd", mapping.TimeSharingEnd);
                                    AddAttribute(TVMovieMapping, "TimeSharingStart", mapping.TimeSharingStart);

                                    Channel tmpChannel = Channel.Retrieve(mapping.IdChannel);
                                    if (tmpChannel == null)
                                    {
                                        textoutput("<RED>Channel could not be assigned for tv movie mapping id " + mapping.IdMapping.ToString());
                                    }
#if(MP12)
#else
                                        AddAttribute(TVMovieMapping, "MPChannelName", tmpChannel.Name);
#endif
                                    AddAttribute(TVMovieMapping, "MPDisplayName", tmpChannel.DisplayName);
                                    TVMovieMappings.AppendChild(TVMovieMapping);
                                }
                                catch (Exception exc)
                                {
                                    tvmovieCount--;
                                    textoutput("<YELLOW>TV movie map " + mapping.IdMapping.ToString() + " not retrieved");

                                    if (DEBUG == true)
                                    {
                                        textoutput("<YELLOW>TV movie map " + mapping.IdMapping.ToString() + " for channel ID " + mapping.IdChannel.ToString() + " of station " + mapping.StationName + " could not be exported");
                                        textoutput("<YELLOW>Exception message is " + exc.Message);
                                    }
                                    trouble = true;
                                }
                            }
                        }
                    }
                }
                catch
                {
                    tvmovieCount = 0;
                }
                rootElement.AppendChild(TVMovieMappings);
                textoutput(tvmovieCount + " TV movie mappings exported");

                // export All  Settings
                textoutput("Exporting all settings");

                XmlNode AllSettings = xmlDoc.CreateElement("AllSettings");

                //SQL Query
                string Expression = "";
                IList<Setting> mysettinglist = null;

                textoutput("Starting SQL query for all settings ");
                try
                {
                    SqlBuilder sb = new SqlBuilder(Gentle.Framework.StatementType.Select, typeof(Setting));
                    sb.AddConstraint(Operator.Like, "tag", "%%");  //try like operator and ""

                    SqlStatement stmt = sb.GetStatement(true);
                    mysettinglist = ObjectFactory.GetCollection<Setting>(stmt.Execute());

                }
                catch (Exception exc)
                {
                    textoutput("Error in SQL query for searching " + Expression);
                    textoutput("Exception message was " + exc.Message);
                }
                // End of SQL Query

                int ctr = 0;

                textoutput("Removing temporary settings:");
                foreach (Setting mysetting in mysettinglist)
                {
                    try
                    {

                        //legacy Delete for old leftovers (not found settings from Version 1.2.0.5 and older could have left "NOTFOUND" settings for cards)                   
                        if ((mysetting.Tag.StartsWith("analog")) && ((mysetting.Value == "NOTFOUND") || (mysetting.Value == "BACKUPSETTINGS_NOTFOUND")))
                        {
                            if (DEBUG)
                                textoutput("Removing Setting " + mysetting.Tag + " Value="+mysetting.Value);
                            mysetting.Remove();
                        }
                        else if ((mysetting.Tag.StartsWith("atsc")) && ((mysetting.Value == "NOTFOUND") || (mysetting.Value == "BACKUPSETTINGS_NOTFOUND")))
                        {
                            if (DEBUG)
                                textoutput("Removing Setting " + mysetting.Tag + " Value=" + mysetting.Value);
                            mysetting.Remove();
                        }
                        else if ((mysetting.Tag.StartsWith("dvbc")) && ((mysetting.Value == "NOTFOUND") || (mysetting.Value == "BACKUPSETTINGS_NOTFOUND")))
                        {
                            if (DEBUG)
                                textoutput("Removing Setting " + mysetting.Tag + " Value=" + mysetting.Value);
                            mysetting.Remove();
                        }
                        else if ((mysetting.Tag.StartsWith("dvbs")) && ((mysetting.Value == "NOTFOUND") || (mysetting.Value == "BACKUPSETTINGS_NOTFOUND")))
                        {
                            if (DEBUG)
                                textoutput("Removing Setting " + mysetting.Tag + " Value=" + mysetting.Value);
                            mysetting.Remove();
                        }
                        else if ((mysetting.Tag.StartsWith("dvbt")) && ((mysetting.Value == "NOTFOUND") || (mysetting.Value == "BACKUPSETTINGS_NOTFOUND")))
                        {
                            if (DEBUG)
                                textoutput("Removing Setting " + mysetting.Tag + " Value=" + mysetting.Value);
                            mysetting.Remove();
                        }
                        else if ((mysetting.Tag.StartsWith("dvbip")) && ((mysetting.Value == "NOTFOUND") || (mysetting.Value == "BACKUPSETTINGS_NOTFOUND")))
                        {
                            if (DEBUG)
                                textoutput("Removing Setting " + mysetting.Tag + " Value=" + mysetting.Value);
                            mysetting.Remove();
                        }//end remove temporary settings
                        else
                        {

                            //jump over card settings
                            if (mysetting.Tag.StartsWith("dvbs") == true)
                                continue;
                            else if (mysetting.Tag.StartsWith("dvbt") == true)
                                continue;
                            else if (mysetting.Tag.StartsWith("dvbc") == true)
                                continue;
                            else if (mysetting.Tag.StartsWith("dvbip") == true)
                                continue;
                            else if (mysetting.Tag.StartsWith("atsc") == true)
                                continue;
                            else if (mysetting.Tag.StartsWith("analog") == true)
                                continue;
                            else if (mysetting.Tag.StartsWith("Backup_SettingsSetup_debug") == true)
                                continue;

                            XmlNode nodesetting = xmlDoc.CreateElement("Setting");
                            string tag = mysetting.Tag.Replace(" ", "__SPACE__");  //do not forget to reconvert during import - do not change
                            tag = tag.Replace("+", "__PLUS__");  //do not forget to reconvert during import - do not change
                            string value = mysetting.Value.Replace("\n", "__RETURN__");  //do not forget to reconvert during import - do not change

                            AddAttribute(nodesetting, tag, value);
                            AllSettings.AppendChild(nodesetting);
                            if (DEBUG)
                                textoutput(tag + " :  " + value);

                            ctr++;
                        }
                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>Error for tag=" + mysetting.Tag);
                        textoutput("<RED>Exception=" + exc.Message);
                        textoutput("");
                        Log.Error("Error for tag=" + mysetting.Tag);
                        Log.Error("value=" + mysetting.Value.ToString());
                    }
                }

                rootElement.AppendChild(AllSettings);

                xmlDoc.AppendChild(rootElement);
                xmlDoc.Save(fileName);
                textoutput(ctr.ToString() + " settings have been exported");

                if (trouble == true)
                {
                    myMessageBox("Some settings could not be exported - check the status window or the log file for more information", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                }
                progressbar((int)PB_action.STOP, ref PB_Export, (int)PB_part.TV_TvServerxml);

            }
            catch (Exception exc)
            {
                // check for disk space not done yet!!!!!

                textoutput("<RED>Failed to export TV server settings to file - exception message is " + exc.Message);
                return false;
            }
            return true;


        }

        
        private bool plugincheck()
        {
            string plugintest = detectplugin("BackupSettings");
            if (plugintest == "NOT_FOUND")
            {
                textoutput("<RED>Fatal Error: Plugin could not be recognized");
                textoutput("<RED>Please reboot and repeat import operation");
                textoutput("<RED>If rebooting does not help try to uninstall and reinstall the backupsetting plugin");
                textoutput("<RED>If that does not help you need to uninstall and reinstall tvserver as your installation seems to be corrupted");
                return false;
            }
            else
            {
                textoutput("Plugin check successful");
            }
            return true;
        }

        
        private string detectplugin(string pluginname)
        {
            PluginLoader mypluginloader = new PluginLoader();
            mypluginloader.Load();
            foreach (ITvServerPlugin plug in mypluginloader.Plugins)
            {
                if (plug.Name == pluginname)
                {
                    return plug.Version;
                }
            }
            return "NOT_FOUND";
        }



#endif



        private void backupscripts(string folderpath, Scripts number)
        {
            string scriptfile = "";
            string scriptname = "";
            switch (number)
            {
                case Scripts.TV_Export:
                    {
                        scriptfile = TV_USER_FOLDER + @"\BackupSettingScripts\TV_export.bat";
                        scriptname = "TV_export.bat";
                        break;
                    }
                case Scripts.MP_Export:
                    {
                        scriptfile = TV_USER_FOLDER + @"\BackupSettingScripts\MP_export.bat";
                        scriptname = "MP_export.bat";
                        break;
                    }
                case Scripts.TV_Import:
                    {
                        scriptfile = TV_USER_FOLDER + @"\BackupSettingScripts\TV_import.bat";
                        scriptname = "TV_import.bat";
                        break;
                    }
                case Scripts.MP_Import:
                    {
                        scriptfile = TV_USER_FOLDER + @"\BackupSettingScripts\MP_import.bat";
                        scriptname = "MP_import.bat";
                        break;
                    }
                case Scripts.AutoCorrect:
                    {
                        scriptfile = TV_USER_FOLDER + @"\BackupSettingScripts\AutoRepair.bat";
                        scriptname = "AutoCorrect.bat";
                        break;
                    }
            }
            if (File.Exists(scriptfile) == true)
            {
                textoutput("User batch process " + scriptname + " started");
                ProcessStartInfo BackupSettingBatchStart = new ProcessStartInfo(scriptfile);
                BackupSettingBatchStart.Arguments = "\"" + folderpath + "\"   \"" + TV_PROGRAM_FOLDER + "\"   \"" + TV_USER_FOLDER + "\"   \"" + MP_PROGRAM_FOLDER + "\"   \"" + MP_USER_FOLDER + "\"";
                BackupSettingBatchStart.WorkingDirectory = TV_USER_FOLDER + @"\BackupSettingScripts";
                BackupSettingBatchStart.UseShellExecute = true;

                Process BackupSettingBatchExecute = new Process();
                BackupSettingBatchExecute.StartInfo = BackupSettingBatchStart;
                try
                {
                    BackupSettingBatchExecute.Start();
                }
                catch (Exception exc)
                {
                    textoutput("<RED>Could not start " + scriptfile + "  " + BackupSettingBatchStart.Arguments);
                    if (DEBUG == true)
                        textoutput("<RED>Exception message is " + exc.Message);
                    return;
                }
                BackupSettingBatchExecute.WaitForExit(1000 * 60 * 3); //wait 3 minutes maximum
                if (BackupSettingBatchExecute.HasExited == true)
                {
                    if (BackupSettingBatchExecute.ExitCode != 0)
                    {
                        textoutput("<RED>Batch process completed with Errorcode " + BackupSettingBatchExecute.ExitCode.ToString());
                        return;
                    }
                }
                else
                {
                    textoutput("<RED>Batch process did not complete within 3 minutes");
                    return;
                }
                textoutput("User batch process " + scriptname + " completed ");
            }
        }

        private void BackupRestoreExtraFolders(bool export, string backupfolder)
        {
            backupfolder = backupfolder + @"\EXTRA_FOLDERS";
            for (int i = 0; i < ExtraFolderData.Length; i++)
            {
                string row = ExtraFolderData[i];
                string[] tempcolumndata = row.Split(BACKUPSETTINGS_COLUMN_SEPARATOR);
                //check for valid search entries
                if (tempcolumndata.Length < LISTVIEWCOLUMNS)
                {
                    if (DEBUG)
                        textoutput("Invalid row=" + row + "- will be skipped");
                    continue;
                }
                else
                {
                    if (DEBUG)
                        textoutput("row="+row+"row count=" + tempcolumndata.Length.ToString());
                }


                string active = "False";
                try //active
                {
                    active = tempcolumndata[0];
                }
                catch
                {
                    //do nothing
                }

                string name = "";
                try //name
                {
                    name = tempcolumndata[1];
                }
                catch
                {
                    //do nothing
                }

                string folder = "";
                try //folder
                {
                    folder = tempcolumndata[2];
                }
                catch
                {
                    //do nothing
                }


                string overwrite_str = "False";
                try //active
                {
                    overwrite_str = tempcolumndata[3];
                }
                catch
                {
                    //do nothing
                }

                bool overwrite = false;
                if (overwrite_str == "True")
                    overwrite = true;


                string killprocessname = "";
                try //kill process
                {
                    killprocessname = tempcolumndata[4];
                }
                catch
                {
                    //do nothing
                }

                string restartprocess = "";
                try //restart process
                {
                    restartprocess = tempcolumndata[5];
                }
                catch
                {
                    //do nothing
                }




                if ((active == "True") && (name != ""))
                {
                    //stop process
                    if (killprocessname != "")
                    {
                        Process[] processes = Process.GetProcessesByName(killprocessname);
                        foreach (Process process in processes)
                        {
                            process.Kill();
                        }
                        textoutput("Process " + killprocessname + " has been terminated");
                    }

                    //copy data with overwrite
                    if (export == true) //export
                    {
                        textoutput("Exporting " + folder + "\n to " + backupfolder + "\\" + name + "\n");

                        try
                        {
                            //     verbose , recursive
                            DirectoryCopy(folder, backupfolder + "\\" + name, "*", overwrite, DEBUG, true);
                        }
                        catch (Exception exc)
                        {
                            textoutput("<RED>Error: Could not backup folder - Exception message was:");
                            textoutput(exc.Message);

                        }
                    }
                    else //import
                    {
                        textoutput("Importing " + backupfolder + "\\" + name + "\n to " + folder + "\n");

                        try
                        {
                            if (Directory.Exists(backupfolder + "\\" + name) == false)
                            {
                                textoutput("<RED>Error: Backup folder does not exist");
                            }
                            else
                            {
                                if (File.Exists(folder) == false)
                                {
                                    Directory.CreateDirectory(folder);
                                }
                                //     verbose , recursive
                                DirectoryCopy(backupfolder + "\\" + name, folder, "*", overwrite, DEBUG, true);
                            }
                        }
                        catch (Exception exc)
                        {
                            textoutput("<RED>Error: Could not restore folder - Exception message was:");
                            textoutput(exc.Message);

                        }

                    }

                    //restart process

                    if (restartprocess != "")
                    {
                        if (File.Exists(restartprocess) == true)
                        {
                            Process app = new Process();
                            ProcessStartInfo appstartinfo = new ProcessStartInfo();
                            appstartinfo.FileName = restartprocess;
                            string restartdirectory = "";
                            string[] tokens = restartprocess.Split('\\');
                            //textoutput("tokens length="+tokens.Length.ToString());
                            for (int j = 0; j < tokens.Length - 1; j++)
                            {
                                restartdirectory += tokens[j] + "\\";
                            }
                            appstartinfo.WorkingDirectory = restartdirectory;
                            //textoutput("WorkingDirectory =" + restartdirectory);
                            app.StartInfo = appstartinfo;
                            try
                            {
                                app.Start();

                                textoutput("Process " + appstartinfo.FileName + " started");

                            }
                            catch (Exception exc)
                            {
                                textoutput("<RED>Error in starting the process " + appstartinfo.FileName);
                                if (DEBUG == true)
                                    textoutput("<RED>Exception message is " + exc.Message);
                            }
                        }
                        else
                        {
                            textoutput("<RED> Error: Process could not be restarted");
                            textoutput("<RED>" + restartprocess + " does not exist");

                        }
                    }

                }
            }
        }

        private void AddAttribute(XmlNode node, string tagName, string tagValue)
        {
            XmlAttribute attr = node.OwnerDocument.CreateAttribute(tagName);
            attr.InnerText = tagValue;
            node.Attributes.Append(attr);
        }

        private void AddAttribute(XmlNode node, string tagName, int tagValue)
        {
            AddAttribute(node, tagName, tagValue.ToString());
        }

        private void AddAttribute(XmlNode node, string tagName, DateTime tagValue)
        {// store DateTime Values as strings. Improves readability
            AddAttribute(node, tagName, String.Format("{0}-{1}-{2} {3}:{4}:{5}", tagValue.Year, tagValue.Month, tagValue.Day, tagValue.Hour, tagValue.Minute, tagValue.Second));
        }

        private void AddAttribute(XmlNode node, string tagName, bool tagValue)
        {
            AddAttribute(node, tagName, tagValue.ToString());
        }

        /// <summary>
        /// huha�s DirectoryCopy
        /// Copies a directory source including its subdirectories to a directory destination 
        /// If destination does not exists it will be created
        /// </summary>
        /// <param name="string source">string to the path of the source directory</param>
        /// <param name="string destination">string to the path of the destination directory</param>
        /// <param name="string filepattern">Filter for files (use "*" for all files)</param>
        /// <param name="bool overwrite">use "true" for overwriting existing files in the destination directory or otherwise "false" </param>
        /// <param name="bool verbose">use "true" for verbose output and logging</param>
        /// <param name="bool recursive">use "true" for including recursive directories</param>
        public void DirectoryCopy(string source, string destination, string filepattern, bool overwrite, bool verbose, bool recursive)
        {
            verbose = false;  // disable verbose due to very long and slow processing
            if (verbose)
            {
                textoutput("BackupSettings source=" + source);
                textoutput("BackupSettings destination=" + destination);
            }
            if (!File.Exists(destination))
            {
                try
                {
                    Directory.CreateDirectory(destination);
                }
                catch (Exception exc)
                {
                    textoutput("DirectoryCopy Error: Could not create " + destination + " - Exception: " + exc.Message);
                }
            }

            // Copy files.
            DirectoryInfo sourceDir = new DirectoryInfo(source);
            FileInfo[] files = sourceDir.GetFiles(filepattern);
            foreach (FileInfo file in files)
            {
                if (file.Name != "BackupSettings.dll")  //legacy suuport - do not restore old version which would result in 2 dll files
                {
                    try
                    {
                        if (!File.Exists(destination + "\\" + file.Name))
                        { // file does not exist

                            File.Copy(file.FullName, destination + "\\" + file.Name, false);
                            if (verbose)
                            {
                                textoutput("Copied: " + file.FullName);
                            }

                        }
                        else if (overwrite == false) // and file does exist => do not copy
                        {
                            if (verbose)
                            {
                                textoutput("Exists:" + file.FullName);
                            }
                        }
                        else // file does exist
                        { // do overwrite files
                            // check for read only protection
                            FileAttributes attribute = File.GetAttributes(destination + "\\" + file.Name);

                            if ((attribute & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                            {
                                if (verbose)
                                {
                                    textoutput("ReadOnly: " + file.FullName);
                                }
                            }
                            else if ((attribute & FileAttributes.Hidden) == FileAttributes.Hidden)
                            {
                                if (verbose)
                                {
                                    textoutput("Hidden: " + file.FullName);
                                }
                            }
                            else if ((attribute & FileAttributes.System) == FileAttributes.System)
                            {
                                if (verbose)
                                {
                                    textoutput("System: " + file.FullName);
                                }
                            }
                            else
                            {
                                File.Copy(file.FullName, destination + "\\" + file.Name, true);
                                if (verbose)
                                {
                                    textoutput("Copied: " + file.FullName);
                                }
                            }
                        }

                    }
                    catch (Exception exc)
                    {
                        textoutput("<RED>DirectoryCopy Error: Could not copy file " + file.FullName + " to " + destination + "\\" + file.Name + " - Exception:" + exc.Message);
                    }
                }
            }

            // subdirectories are being called recursively
            if (recursive)
            {
                DirectoryInfo sourceinfo = new DirectoryInfo(source);
                DirectoryInfo[] dirs = sourceinfo.GetDirectories();

                foreach (DirectoryInfo dir in dirs)
                {
                    string dirstring = dir.FullName;
                    FileAttributes attributes = dir.Attributes;

                    if (((attributes & FileAttributes.Hidden) != FileAttributes.Hidden) && ((attributes & FileAttributes.System) != FileAttributes.System))
                        DirectoryCopy(dirstring, destination + "\\" + dir, filepattern, overwrite, verbose, recursive);
                }
            }
        }

        public void removeReadOnlyRecursive(string folderpath)
        {
            try
            {
                DirectoryInfo sourceDir = new DirectoryInfo(folderpath);

                File.SetAttributes(folderpath, File.GetAttributes(folderpath) & ~FileAttributes.ReadOnly);

                FileInfo[] files = sourceDir.GetFiles("*");
                foreach (FileInfo file in files)
                {
                    File.SetAttributes(file.FullName, File.GetAttributes(file.FullName) & ~(FileAttributes.ReadOnly));
                }

                string[] subdir = Directory.GetDirectories(folderpath);
                foreach (string subdirname in subdir)
                {
                    removeReadOnlyRecursive(subdirname);
                }
            }
            catch (Exception exc)
            {
                textoutput("<RED>Fatal Error: Could not remove read only attribute in " + folderpath + " - Exception:" + exc.Message);
            }


        }

        private void checkfilepath(string recdirectory, string name)
        {
            // check directory existence
            if (Directory.Exists(recdirectory) == false)
            {
                textoutput("<RED>Directory " + recdirectory + " does not exist for " + name + " You need to change the file path manually after the import");      // error abort or create directory  
                myMessageBox("Directory " + recdirectory + " does not exist for " + name + "\n\n You need to change the file path manually after the import", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }



            // create temp file for checking file access

            try
            {
                DateTime nowdate = DateTime.Now;
                string tmpfile = @"\temp" + nowdate.ToString("yyyy_MM_dd_HH_mm_ss") + ".tmp";
                tmpfile = recdirectory + tmpfile;
                Stream streamWrite = File.Create(tmpfile);
                streamWrite.Close();
                File.Delete(tmpfile);
            }
            catch (Exception exc)
            {
                myMessageBox("No write access for " + name + " " + recdirectory + " Exception: " + exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1);
                textoutput("<RED>No write access for " + name + " " + recdirectory + " Exception: " + exc.Message);
            }

        }

        private string GetNodeAttribute(XmlNode node, string attribute, string defaultValue)
        {
            if (node.Attributes[attribute] == null)
                return defaultValue;
            else
                return node.Attributes[attribute].Value;
        }

        private int CompareVersions(string versionstring1, string versionstring2)
        {
            //returns   0 if versionstring1 is equal to   versionstring2
            //returns   1 if versionstring1 is newer than versionstring2
            //returns  -1 if versionstring1 is older than versionstring2
            //returns  99 if versionstring1 is invalid
            //returns -99 if versionstring2 is invalid

            int Version1Major = 0;
            int Version1Minor = 0;
            int Version1Build = 0;
            int Version1Private = 0;

            int Version2Major = 0;
            int Version2Minor = 0;
            int Version2Build = 0;
            int Version2Private = 0;

            string[] versionarray1 = versionstring1.Split('.');
            if (versionarray1.Length == 3)
            {
                versionstring1 += ".0";
                versionarray1 = versionstring1.Split('.');
            }
            else if (versionarray1.Length == 2)
            {
                versionstring1 += ".0.0";
                versionarray1 = versionstring1.Split('.');
            }
            else if (versionarray1.Length == 1)
            {
                versionstring1 += ".0.0.0";
                versionarray1 = versionstring1.Split('.');
            }

            try
            {
                Version1Major = Convert.ToInt32(versionarray1[0]);
                Version1Minor = Convert.ToInt32(versionarray1[1]);
                Version1Build = Convert.ToInt32(versionarray1[2]);
                Version1Private = Convert.ToInt32(versionarray1[3]);
            }
            catch
            {
                textoutput("<RED>Error in CompareVersions: Versionstring1 " + versionstring1 + " is invalid");
                return (int)COMPAREVERSION.ERRORVERSION1;
            }

            string[] versionarray2 = versionstring2.Split('.');
            if (versionarray2.Length == 3)
            {
                versionstring2 += ".0";
                versionarray2 = versionstring2.Split('.');
            }
            else if (versionarray2.Length == 2)
            {
                versionstring2 += ".0.0";
                versionarray2 = versionstring2.Split('.');
            }
            else if (versionarray2.Length == 1)
            {
                versionstring2 += ".0.0.0";
                versionarray2 = versionstring2.Split('.');
            }

            try
            {
                Version2Major = Convert.ToInt32(versionarray2[0]);
                Version2Minor = Convert.ToInt32(versionarray2[1]);
                Version2Build = Convert.ToInt32(versionarray2[2]);
                Version2Private = Convert.ToInt32(versionarray2[3]);
            }
            catch
            {
                textoutput("<RED>Error in CompareVersions: Versionstring2 " + versionstring2 + " is invalid");
                return (int)COMPAREVERSION.ERRORVERSION2;
            }

            //Compare major
            if (Version1Major > Version2Major)
            {
                return (int)COMPAREVERSION.NEWER;
            }
            else if (Version1Major < Version2Major)
            {
                return (int)COMPAREVERSION.OLDER;
            }
            else
            {
                //Compare Minor
                if (Version1Minor > Version2Minor)
                {
                    return (int)COMPAREVERSION.NEWER;
                }
                else if (Version1Minor < Version2Minor)
                {
                    return (int)COMPAREVERSION.OLDER;
                }
                else
                {
                    //Compare Build
                    if (Version1Build > Version2Build)
                    {
                        return (int)COMPAREVERSION.NEWER;
                    }
                    else if (Version1Build < Version2Build)
                    {
                        return (int)COMPAREVERSION.OLDER;
                    }
                    else
                    {
                        //Compare Private
                        if (Version1Private > Version2Private)
                        {
                            return (int)COMPAREVERSION.NEWER;
                        }
                        else if (Version1Private < Version2Private)
                        {
                            return (int)COMPAREVERSION.OLDER;
                        }
                        else
                        {
                            //versions are equal
                            return (int)COMPAREVERSION.EQUAL;
                        }
                    }
                }
            }
        }

        public void getallversionnumbers(string pathfolder, bool both)
        {
            
            /*  current working directory must have been defined before calling routine
            // Ensure TV Program folder is current working directory
            System.Environment.CurrentDirectory = TV_PROGRAM_FOLDER;*/

            // read all version numbers

            //ActualBackupSettingsVersion
            ActualBackupSettingsVersion = pluginversion;

            if (DEBUG)
            {
                textoutput("MP_PROGRAM_FOLDER=" + MP_PROGRAM_FOLDER);
                textoutput("TV_PROGRAM_FOLDER=" + TV_PROGRAM_FOLDER);
                textoutput("MP2_PROGRAM_FOLDER=" + MP2_PROGRAM_FOLDER);
                textoutput("SV2_PROGRAM_FOLDER=" + SV2_PROGRAM_FOLDER);
            }

            //ActualMediaPortalVersion
            try
            {
                FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(MP_PROGRAM_FOLDER + @"\MediaPortal.exe");
                ActualMediaPortalVersion = myFileVersionInfo.FileVersion;
            }
            catch
            {
                ActualMediaPortalVersion = "BACKUPSETTINGS_NOTFOUND";
            }

            //ActualTvServerVersion
            try
            {
                FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(TV_PROGRAM_FOLDER + @"\TvService.exe");
                ActualTvServerVersion = myFileVersionInfo.FileVersion;
            }
            catch
            {
                ActualTvServerVersion = "BACKUPSETTINGS_NOTFOUND";
            }

            //ActualMP2ServerVersion
            try
            {
                FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(SV2_PROGRAM_FOLDER + @"\MP2-Server.exe");
                ActualMP2ServerVersion = myFileVersionInfo.FileVersion;
            }
            catch
            {
                ActualMP2ServerVersion = "BACKUPSETTINGS_NOTFOUND";
            }

            //ActualMP2ClientVersion
            try
            {
                FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(MP2_PROGRAM_FOLDER + @"\MP2-Client.exe");
                ActualMP2ClientVersion = myFileVersionInfo.FileVersion;
            }
            catch
            {
                ActualMP2ClientVersion = "BACKUPSETTINGS_NOTFOUND";
            }

            if (both)
            {
                //open stored Version.xml file and read Backup Version Numbers
                string versionfile = pathfolder + @"\Version.xml";
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    if (File.Exists(versionfile) == true)
                    {
                        xmlDoc.Load(versionfile);
                    }

                    XmlNode rootElement = xmlDoc.SelectSingleNode("/Version");
                    if (rootElement != null)
                    {

                        //BackupTvServerVersion
                        BackupTvServerVersion = GetNodeAttribute(rootElement, "TvServiceVersion", "0.0.0.0");


                        //BackupMediaPortalVersion
                        BackupMediaPortalVersion = GetNodeAttribute(rootElement, "MediaPortalVersion", "0.0.0.0");


                        //BackupBackupSettingsVersion
                        BackupBackupSettingsVersion = GetNodeAttribute(rootElement, "BackupSettingsVersion", "0.0.0.0");

                        //BackupMP2ServerVersion
                        BackupMP2ServerVersion = GetNodeAttribute(rootElement, "BackupMP2ServerVersion", "0.0.0.0");

                        //BackupMP2ClientVersion
                        BackupMP2ClientVersion = GetNodeAttribute(rootElement, "BackupMP2ClientVersion", "0.0.0.0");
                    }


                }
                catch (Exception exc)
                {
                    textoutput("<RED>Could not read Version number from file " + versionfile);
                    if (DEBUG == true)
                    {
                        textoutput("<RED>Exception message is " + exc.Message);
                    }

                }
            }
            if (DEBUG == true)
            {
                textoutput("Debug ActualBackupSettingsVersion=" + ActualBackupSettingsVersion.ToString());
                textoutput("Debug ActualMediaPortalVersion=" + ActualMediaPortalVersion.ToString());
                textoutput("Debug ActualTvServerVersion=" + ActualTvServerVersion.ToString());
                textoutput("Debug ActualMP2ServerVersion=" + ActualMP2ServerVersion.ToString());
                textoutput("Debug ActualMP2ClientVersion=" + ActualMP2ClientVersion.ToString());
                textoutput("Debug BackupBackupSettingsVersion=" + BackupBackupSettingsVersion.ToString());
                textoutput("Debug BackupMediaPortalVersion=" + BackupMediaPortalVersion.ToString());
                textoutput("Debug BackupTvServerVersion=" + BackupTvServerVersion.ToString());
                textoutput("Debug BackupMP2ServerVersion=" + BackupMP2ServerVersion.ToString());
                textoutput("Debug BackupMP2ClientVersion=" + BackupMP2ClientVersion.ToString());
            }
            // end all version numbers
}

        public bool restorebackup(string pathfolder)
        {
            POSTIMPORT = "";

            //loadsettings
            MyLoadSettings();

            //initialize progressbar
            progressbar((int)PB_action.INIT, ref PB_Import, 0);




            bool defaultcheck = true;

            if (TV) //20 criteria
            {
                defaultcheck = defaultcheck && server && channels && channelcardmappings && tvgroups;
                defaultcheck = defaultcheck && radiogroups && schedules && !epg && recordings;
                defaultcheck = defaultcheck && general_settings && clickfinder && delete_channels && delete_tvgroups;
                defaultcheck = defaultcheck && delete_radiogroups && delete_schedules && delete_recordings && TVServerSettings;
                defaultcheck = defaultcheck && TVallPrograms && TVAllFolders && TVRestart && TVAutoCorrect;
            }

            if (MP) //12 criteria
            {
                defaultcheck = defaultcheck && MPDatabase && MPInputDevice && MPPlugins && MPProgramXml;
                defaultcheck = defaultcheck && MPSkins && MPThumbs && MPUserXML && MPxmltv;
                defaultcheck = defaultcheck && MPMusicPlayer && MPDeleteCache && MPAllFolder && MPAllProgram;
            }

            if (MP2C) //6 criteria
            {
                defaultcheck = defaultcheck && MP2Config && MP2Plugins && MP2AllClientFolders && MP2AllClientFiles;
                defaultcheck = defaultcheck && MP2Defaults && MP2AllClientProgramFolders;
            }

            if (MP2S) //7 criteria
            {
                defaultcheck = defaultcheck && SV2Configuration && SV2Database && SV2Plugins && SV2AllServerFolders;
                defaultcheck = defaultcheck && SV2AllServerFiles && SV2Defaults && SV2AllServerProgramFolders;
            }

            if (!defaultcheck)
            {
                switch (myMessageBox("You are not using the default filter settings for the restore - Only partial data will be imported.\n\n Do you want to continue?", "Info: Default filter settings not used", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                {
                    case DialogResult.Yes:
                        {
                            // "Yes" processing
                            textoutput("Default filter settings are not used for import");
                            break;
                            
                        }
                    case DialogResult.No:
                        {
                            // "No" processing
                            textoutput("<RED>Import aborted by user because default settings are not used - click the Default button and repeat import");
                            return false;

                        }
                }
            }

            if (MP == true)  // check if MP is running
            {
                Process[] procs = Process.GetProcessesByName("MediaPortal");
                foreach (Process proc in procs) // loop is only executed if Media portal is running
                {
                    textoutput("<RED>You need to close Media Portal before you import Media Portal settings");
                    myMessageBox("You need to close Media Portal before you import Media Portal settings", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return false;
                }


            }

            if (MP2C == true)  // check if MP2 Client is running
            {
                Process[] procs = Process.GetProcessesByName("MP2-Client");
                foreach (Process proc in procs) // loop is only executed if Media portal is running
                {
                    textoutput("<RED>You need to close Media Portal2 Clients before you import Media Portal settings");
                    myMessageBox("You need to close Media Portal2 Clients before you import Media Portal settings", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return false;
                }


            }

            if (MP2S == true)  // check if MP2 Server is running
            {
                Process[] procs = Process.GetProcessesByName("MP2-Server");
                foreach (Process proc in procs) // loop is only executed if Media portal is running
                {
                    textoutput("<RED>You need to close the Media Portal2 Server before you import Media Portal settings");
                    myMessageBox("You need to close the Media Portal2 Server before you import Media Portal settings", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    return false;
                }


            }

            if (!Directory.Exists(pathfolder))
            {
                textoutput("Import folder " + pathfolder + " does not exist");
                return false;
            }



            // check for backup export of actual settings before importing file

            //last chance for user to abort import

            switch (myMessageBox("Importing will overwrite all your current settings - Do you want to continue?", "Info for Import Settings", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
            {
                case DialogResult.No: // "No" import file
                    {
                        textoutput("<RED>Import aborted by user");
                        return false;

                    }

                case DialogResult.Yes: // "Yes" backup actual settings first                    
                    {
                        textoutput("Starting import:");
                        break;
                    }

            }

            time1 = DateTime.Now;

            //handle extra folders
            progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.ExtraFolders);
            BackupRestoreExtraFolders(false, pathfolder);  //export=false means importing data
            progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.ExtraFolders);

            //*************************    
            //TV server
            //*************************
#if (TV)
            #region TVserver
            if (TV == false)
            {
                textoutput("TV server not checked - will not restore settings");
            }
            else
            {
                // check installation folder and autodetect
                
                if (File.Exists(TV_PROGRAM_FOLDER + "\\TvService.exe") == false)
                {
                    textoutput("TV server program folder does not exist - aborting import");
                    return false;
                }

                if (Directory.Exists(TV_USER_FOLDER) == false)
                {
                    textoutput("<RED>TV server data folder does not exist - aborting import");
                    return false;
                }

                //read all version numbers for user warnings
                getallversionnumbers(pathfolder,true);


                //repair Tv server database
                if (TVAutoCorrect)
                {
                    textoutput("Trying to repair TV server database before import");
                    backupscripts(pathfolder, Scripts.AutoCorrect);                   
                }

                // copy all TV program folders back, do not overwrite data
                if (TVallPrograms == true)
                {
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.TV_TvServerProgramFolders);

                    //legacy support renaming Backupsettings installation folder to BackupSettings
                    if (Directory.Exists(pathfolder + @"\MP_User\Installer\Backupsettings") == true)
                    {
                        try
                        {
                            Directory.Move(pathfolder + @"\MP_User\Installer\Backupsettings", pathfolder + @"\MP_User\Installer\BackupSettings");
                        }
                        catch
                        {

                        }
                    }

                    if (Directory.Exists(pathfolder + @"\TV_Program") == true)
                    {

                        DirectoryInfo root = new DirectoryInfo(pathfolder + @"\TV_Program");

                        DirectoryInfo[] dirs = root.GetDirectories("*", SearchOption.TopDirectoryOnly);

                        foreach (DirectoryInfo subDir in dirs)
                        {
                            FileAttributes attributes = subDir.Attributes;
                            try
                            {

                                if ((subDir.Name == "TuningParameters") && (CompareVersions(ActualTvServerVersion, "1.0.3.0") == (int)COMPAREVERSION.NEWER)) //change folder for version 1.1
                                {
                                    DirectoryCopy(pathfolder + @"\TV_Program\" + subDir.Name, TV_USER_FOLDER + @"\" + subDir.Name, "*", false, DEBUG, true);  //overwrite,verbose,recursive
                                    textoutput("TV server " + subDir.Name + " restored");
                                }
                                else if (subDir.Name.ToLower() == "plugins")
                                {
                                    if ((CompareVersions(ActualTvServerVersion, "1.1.6.0") == (int)COMPAREVERSION.NEWER) && (CompareVersions(BackupTvServerVersion, "1.1.6.0") == (int)COMPAREVERSION.OLDER))
                                    {
                                        textoutput("<YELLOW>Warning TV Server Plugins:\nThis program version contains significant program changes and your plugins will not be copied over.\nPlease reinstall the latest version from the internet.\nIf you did not download any plugins from the internet you can ignore this message.\n");
                                    }
                                    else if ((CompareVersions(ActualTvServerVersion, "1.1.6.0") == (int)COMPAREVERSION.OLDER) && (CompareVersions(BackupTvServerVersion, "1.1.6.0") == (int)COMPAREVERSION.NEWER))
                                    {
                                        textoutput("<YELLOW>Warning TV Server Plugins:\nThis program version contains significant program changes and your plugins will not be copied over.\nPlease reinstall the latest version from the internet.\nIf you did not download any plugins from the internet you can ignore this message.\n");
                                    }
                                    else if ((CompareVersions(ActualTvServerVersion, "1.0.6.0") == (int)COMPAREVERSION.NEWER) && (CompareVersions(BackupTvServerVersion, "1.0.6.0") == (int)COMPAREVERSION.OLDER))
                                    {
                                        textoutput("<YELLOW>Warning TV Server Plugins:\nThis program version contains significant program changes and your plugins will not be copied over.\nPlease reinstall the latest version from the internet.\nIf you did not download any plugins from the internet you can ignore this message.\n");
                                    }
                                    else if ((CompareVersions(ActualTvServerVersion, "1.0.6.0") == (int)COMPAREVERSION.OLDER) && (CompareVersions(BackupTvServerVersion, "1.0.6.0") == (int)COMPAREVERSION.NEWER))
                                    {
                                        textoutput("<YELLOW>Warning TV Server Plugins:\nThis program version contains significant program changes and your plugins will not be copied over.\nPlease reinstall the latest version from the internet.\nIf you did not download any plugins from the internet you can ignore this message.\n");
                                    }
                                    else if ((CompareVersions(ActualTvServerVersion, "1.0.5.0") == (int)COMPAREVERSION.NEWER) && (CompareVersions(BackupTvServerVersion, "1.0.5.0") == (int)COMPAREVERSION.OLDER))
                                    {
                                        textoutput("<YELLOW>Warning TV Server Plugins:\nThis program version contains significant program changes and your plugins will not be copied over.\nPlease reinstall the latest version from the internet.\nIf you did not download any plugins from the internet you can ignore this message.\n");
                                    }
                                    else if ((CompareVersions(ActualTvServerVersion, "1.0.5.0") == (int)COMPAREVERSION.OLDER) && (CompareVersions(BackupTvServerVersion, "1.0.5.0") == (int)COMPAREVERSION.NEWER))
                                    {
                                        textoutput("<YELLOW>Warning TV Server Plugins:\nThis program version contains significant program changes and your plugins will not be copied over.\nPlease reinstall the latest version from the internet.\nIf you did not download any plugins from the internet you can ignore this message.\n");
                                    }
                                    else if ((CompareVersions(ActualTvServerVersion, "1.0.3.0") == (int)COMPAREVERSION.NEWER) && (CompareVersions(BackupTvServerVersion, "1.0.3.0") == (int)COMPAREVERSION.OLDER))
                                    {
                                        textoutput("<YELLOW>Warning TV Server Plugins:\nThis program version contains significant program changes and your plugins will not be copied over.\nPlease reinstall the latest version from the internet.\nIf you did not download any plugins from the internet you can ignore this message.\n");
                                    }
                                    else if ((CompareVersions(ActualTvServerVersion, "1.0.3.0") == (int)COMPAREVERSION.OLDER) && (CompareVersions(BackupTvServerVersion, "1.0.3.0") == (int)COMPAREVERSION.NEWER))
                                    {
                                        textoutput("<YELLOW>Warning TV Server Plugins:\nThis program version contains significant program changes and your plugins will not be copied over.\nPlease reinstall the latest version from the internet.\nIf you did not download any plugins from the internet you can ignore this message.\n");
                                    }
                                    else if ((CompareVersions(ActualTvServerVersion, "1.0.1.0") == (int)COMPAREVERSION.NEWER) && (CompareVersions(BackupTvServerVersion, "1.0.1.0") == (int)COMPAREVERSION.OLDER))
                                    {
                                        textoutput("<YELLOW>Warning TV Server Plugins:\nThis program version contains significant program changes and your plugins will not be copied over.\nPlease reinstall the latest version from the internet.\nIf you did not download any plugins from the internet you can ignore this message.\n");
                                    }
                                    else if ((CompareVersions(ActualTvServerVersion, "1.0.1.0") == (int)COMPAREVERSION.OLDER) && (CompareVersions(BackupTvServerVersion, "1.0.1.0") == (int)COMPAREVERSION.NEWER))
                                    {
                                        textoutput("<YELLOW>Warning TV Server Plugins:\nThis program version contains significant program changes and your plugins will not be copied over.\nPlease reinstall the latest version from the internet.\nIf you did not download any plugins from the internet you can ignore this message.\n");
                                    }
                                    else if (((attributes & FileAttributes.Hidden) != FileAttributes.Hidden) && ((attributes & FileAttributes.System) != FileAttributes.System))
                                    { // do not copy hidden or system directories
                                        DirectoryCopy(pathfolder + @"\TV_Program\" + subDir.Name, TV_PROGRAM_FOLDER + @"\" + subDir.Name, "*", false, DEBUG, true);  //overwrite,verbose,recursive
                                        textoutput("TV server " + subDir.Name + " restored");
                                    }

                                }
                                else if ((subDir.Name != "BackupSettings") && ((attributes & FileAttributes.Hidden) != FileAttributes.Hidden) && ((attributes & FileAttributes.System) != FileAttributes.System))
                                { // do not copy hidden or system directories
                                    DirectoryCopy(pathfolder + @"\TV_Program\" + subDir.Name, TV_PROGRAM_FOLDER + @"\" + subDir.Name, "*", false, DEBUG, true);  //overwrite,verbose,recursive
                                    textoutput("TV server " + subDir.Name + " restored");
                                }

                            }
                            catch (Exception exc)
                            {
                                textoutput("<RED>Could not copy TV server setting folder " + pathfolder + @"\TV_Program\" + subDir.Name + " - Exception: " + exc.Message);
                                return false;
                            }
                        }


                        //restore .dll files from Tvserver
                        try
                        {
                            DirectoryCopy(pathfolder + @"\TV_Program", TV_PROGRAM_FOLDER, "*", false, DEBUG, false);  //overwrite,verbose,recursive
                            textoutput("TV server .dll files restored");
                        }
                        catch (Exception exc)
                        {
                            textoutput("<RED>Could not copy TV server .dll files - Exception: " + exc.Message);
                            return false;
                        }
                    }
                    else
                    {
                        textoutput("<YELLOW>Folder " + pathfolder + @"\TV_Program does not exist");
                    }
                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.TV_TvServerProgramFolders);

                }
                // TV server setting import

                if (TVServerSettings == true)
                {
                    if (File.Exists(pathfolder + @"\TV_User\TVsettings.xml"))
                    {

                        string filename = pathfolder + @"\TV_User\TVsettings.xml";

                        //stop tvservice during import
                        progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.StopTvServer);
                        bool ok_stop = stop_tvservice();
                        progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.StopTvServer);
                        if (ok_stop == false)
                            return false;

                        // import tvserver settings
                        bool Ok = Importxmlfile(filename);
                        // restart tv service after import
                        progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.Start_TVServer);
                        bool ok_start = start_tvservice();
                        progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.Start_TVServer);
                        if (ok_start == false)
                            return false;
                        //abort if import did fail
                        if (Ok == false)
                        {
                            return false;
                        }
                        textoutput("TV server settings imported");
                    }
                    else
                    {
                        textoutput("<RED>Error: TV server setting import failed - File " + pathfolder + @"\TV_User\TVsettings.xml does not exist");
                        textoutput("No TV server settings could be imported");
                        //return false;
                    }


                }

                // copy all TV user folders back
                if (TVAllFolders == true)
                {

                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.TV_TvServerUserFolders);


                    if (Directory.Exists(pathfolder + @"\TV_User") == true)
                    {
                        DirectoryInfo root = new DirectoryInfo(pathfolder + @"\TV_User");

                        DirectoryInfo[] dirs = root.GetDirectories("*", SearchOption.TopDirectoryOnly);

                        foreach (DirectoryInfo subDir in dirs)
                        {
                            FileAttributes attributes = subDir.Attributes;

                            try
                            {
                                if ((subDir.Name == "TuningParameters") && (CompareVersions(ActualTvServerVersion, "1.0.3.0") == (int)COMPAREVERSION.OLDER)) //change folder for version older than version 1.1
                                {
                                    DirectoryCopy(pathfolder + @"\TV_User\" + subDir.Name, TV_PROGRAM_FOLDER + @"\" + subDir.Name, "*", false, DEBUG, true);  //overwrite,verbose,recursive
                                    textoutput("TV server " + subDir.Name + " restored");
                                }
                                else if ((subDir.Name != "BackupSettings") && ((attributes & FileAttributes.Hidden) != FileAttributes.Hidden) && ((attributes & FileAttributes.System) != FileAttributes.System))
                                { // do not copy hidden or system directories
                                    DirectoryCopy(pathfolder + @"\TV_User\" + subDir.Name, TV_USER_FOLDER + @"\" + subDir.Name, "*", true, DEBUG, true);  //change later true verbose to false
                                    textoutput("TV server " + subDir.Name + " restored");
                                }





                            }
                            catch (Exception exc)
                            {
                                textoutput("<RED>Could not copy TV server setting folder " + pathfolder + @"\TV_User\" + subDir.Name + " - Exception: " + exc.Message);
                                return false;
                            }
                        }
                    }
                    else
                    {
                        textoutput("<RED>Media Portal TV_User restore failed - Folder " + pathfolder + @"\TV_User does not exist");
                    }
                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.TV_TvServerUserFolders);

                }

                //user batch script TV import
                backupscripts(pathfolder, Scripts.TV_Import);
                textoutput("TV server import completed");

            }// end TV server restore
            #endregion TVserver
#endif
            //*************************    
            // Media Portal1
            //*************************
            #region MediaPortal1
            if (MP == false)
            {
                textoutput("Media Portal not checked - will not restore settings");
            }
            else
            {


                // check installation folder and autodetect

                if (File.Exists(MP_PROGRAM_FOLDER + "\\MediaPortal.exe") == false)
                {
                    textoutput("<RED>Media Portal program folder does not exist - aborting import");
                    return false;
                }

                if (Directory.Exists(MP_USER_FOLDER) == false)
                {
                    textoutput("<RED>Media Portal data folder does not exist - aborting import");
                    return false;
                }
               

                //read all version numbers for user warnings
                getallversionnumbers(pathfolder,true);

                //read MediaPortalDirs.xml and extract installation paths 

                if ((MPProgramXml == true) && (File.Exists(pathfolder + @"\MP_Program\MediaPortalDirs.xml") == true))
                {
                    // change of skin folders and language folders to MP_User
                    if ((CompareVersions(ActualMediaPortalVersion, "1.0.3.0") == (int)COMPAREVERSION.NEWER) && (CompareVersions(BackupMediaPortalVersion, "1.0.3.0") == (int)COMPAREVERSION.OLDER))
                    {
                        textoutput("<YELLOW>Warning: MediaPortalDirs.xml has not been restored because your backup data and your program version are not compatible and some configuration paths did change. \nIf you have manually modified your MediaPortalDirs.xml file you need to redo it again after the import. \nIf you have never done any manual modification to the file MediaPortalDirs.xml you can ignore this warning.\n");
                    }
                    else if ((CompareVersions(ActualMediaPortalVersion, "1.0.3.0") == (int)COMPAREVERSION.OLDER) && (CompareVersions(BackupMediaPortalVersion, "1.0.3.0") == (int)COMPAREVERSION.NEWER))
                    {
                        textoutput("<YELLOW>Warning: MediaPortalDirs.xml has not been restored because your backup data and your program version are not compatible and some configuration paths did change. \nIf you have manually modified your MediaPortalDirs.xml file you need to redo it again after the import. \nIf you have never done any manual modification to the file MediaPortalDirs.xml you can ignore this warning.\n");
                    }
                    else
                    {
                        File.Copy(pathfolder + @"\MP_Program\MediaPortalDirs.xml", MP_PROGRAM_FOLDER + @"\MediaPortalDirs.xml", true); //restore MediaPortalDirs.xml before reading directory names
                        if (DEBUG == true)
                            textoutput("Restoring MediaPortalDirs.xml before reading installation paths");
                    }
                }
                instpaths.DEBUG = DEBUG;
                instpaths.GetInstallPaths();
                instpaths.GetMediaPortalDirs();
                DIR_BurnerSupport = instpaths.DIR_BurnerSupport;
                DIR_Cache = instpaths.DIR_Cache;
                DIR_Config = instpaths.DIR_Config;
                DIR_CustomInputDefault = instpaths.DIR_CustomInputDefault;
                DIR_CustomInputDevice = instpaths.DIR_CustomInputDevice;
                DIR_Database = instpaths.DIR_Database;
                DIR_Language = instpaths.DIR_Language;
                DIR_Log = instpaths.DIR_Log;
                DIR_Plugins = instpaths.DIR_Plugins;
                DIR_Skin = instpaths.DIR_Skin;
                DIR_Thumbs = instpaths.DIR_Thumbs;
                DIR_Weather = instpaths.DIR_Weather;

                // copy Program setting files and do not overwrite data
                if (MPProgramXml == true)
                {
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.MP_ProgramXML);
                    if (Directory.Exists(pathfolder + @"\MP_Program"))
                    {
                        try
                        {
                            DirectoryCopy(pathfolder + @"\MP_Program", MP_PROGRAM_FOLDER, "*", false, DEBUG, false);//overwrite,verbose,recursive
                        }
                        catch (Exception exc)
                        {
                            textoutput("<RED>Media Portal program .xml import failed - Copying " + pathfolder + @"\MP_Program to " + MP_PROGRAM_FOLDER + " failed with exception message: " + exc.Message);
                            return false;
                        }
                        textoutput("Media Portal program .xml files restored");
                    }
                    else
                    {
                        textoutput("<RED>Error: Media Portal program .xml restore failed - Folder " + pathfolder + @"\MP_Program does not exist");
                        //return false;  no abort if folder does not exist
                    }

                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.MP_ProgramXML);
                }



                // copy all MP program folders back and do not overwrite data besides language
                if (MPAllProgram == true)
                {
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.MP_AllMediaPortalProgramFolders);

                    if (Directory.Exists(pathfolder + @"\MP_Program") == true)
                    {
                        DirectoryInfo root = new DirectoryInfo(pathfolder + @"\MP_Program");

                        DirectoryInfo[] dirs = root.GetDirectories("*", SearchOption.TopDirectoryOnly);

                        foreach (DirectoryInfo subDir in dirs)
                        {

                            FileAttributes attributes = subDir.Attributes;
                            string destination = "";
                            string sub = subDir.Name;
                            if (subDir.Name == "plugins")
                            {//do nothing
                            }
                            else if (subDir.Name == "skin")
                            {//do nothing
                            }
                            else if (subDir.Name == "language")
                            {//do nothing
                            }
                            else if (subDir.Name == "MusicPlayer")
                            {//do nothing
                            }
                            else// copy
                            {
                                try
                                {
                                    if ((subDir.Name != "BackupSettings") && ((attributes & FileAttributes.Hidden) != FileAttributes.Hidden) && ((attributes & FileAttributes.System) != FileAttributes.System))
                                    { // do not copy hidden or system directories

                                        //substitute MediaPortalDirs.xml
                                        bool overwrite = false;





                                        if (subDir.Name == "weather")
                                        {
                                            destination = DIR_Weather;
                                            overwrite = false;
                                        }
                                        else if (subDir.Name == "Burner")
                                        {
                                            destination = DIR_BurnerSupport;
                                            overwrite = false;
                                        }
                                        else if (subDir.Name == "InputDeviceMappings")
                                        {
                                            destination = DIR_CustomInputDefault;
                                            sub = @"InputDeviceMappings\defaults";
                                            overwrite = false;
                                        }
                                        else
                                        {
                                            destination = MP_PROGRAM_FOLDER + @"\" + sub;
                                            overwrite = false;
                                        }


                                        DirectoryCopy(pathfolder + @"\MP_Program\" + sub, destination, "*", overwrite, DEBUG, true);  //overwrite,verbose,recursive
                                        textoutput("Media Portal " + sub + " restored");
                                    }
                                }
                                catch (Exception exc)
                                {
                                    textoutput("<RED>Could not copy Media Portal setting folder " + pathfolder + @"\MP_Program\" + sub + " - Exception: " + exc.Message);
                                    return false;
                                }
                            }
                        }
                    }
                    else
                    {
                        textoutput("<RED>Media Portal MP_Program restore failed - Folder " + pathfolder + @"\MP_Program does not exist");
                    }
                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.MP_AllMediaPortalProgramFolders);
                }



                // copy Plugins and do not overwrite data
                if (MPPlugins == true)
                {
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.MP_Plugins);
                    if (Directory.Exists(pathfolder + @"\MP_Program\plugins"))
                    {

                        try
                        {
                            if ((CompareVersions(ActualMediaPortalVersion, "1.1.6.0") == (int)COMPAREVERSION.NEWER) && (CompareVersions(BackupMediaPortalVersion, "1.1.6.0") == (int)COMPAREVERSION.OLDER))
                            {
                                textoutput("<YELLOW>Warning Media Portal Plugins:\nThis program version contains significant core  changes.\n Your plugins will not be copied\n");
                            }
                            else if ((CompareVersions(ActualMediaPortalVersion, "1.1.6.0") == (int)COMPAREVERSION.OLDER) && (CompareVersions(BackupMediaPortalVersion, "1.1.6.0") == (int)COMPAREVERSION.NEWER))
                            {
                                textoutput("<YELLOW>Warning Media Portal Plugins:\nThis program version contains significant core  changes.\n. Your plugins will not be copied.\n");
                            }
                            else if ((CompareVersions(ActualMediaPortalVersion, "1.0.3.0") == (int)COMPAREVERSION.NEWER) && (CompareVersions(BackupMediaPortalVersion, "1.0.3.0") == (int)COMPAREVERSION.OLDER))
                            {
                                textoutput("<YELLOW>Warning Media Portal Plugins:\nThis program version contains significant core  changes.\n Your plugins will not be copied.\n");
                            }
                            else if ((CompareVersions(ActualMediaPortalVersion, "1.0.3.0") == (int)COMPAREVERSION.OLDER) && (CompareVersions(BackupMediaPortalVersion, "1.0.3.0") == (int)COMPAREVERSION.NEWER))
                            {
                                textoutput("<YELLOW>Warning Media Portal Plugins:\nThis program version contains significant core  changes.\n Your plugins will not be copied.\n");
                            }
                            else if (CompareVersions(ActualMediaPortalVersion, BackupMediaPortalVersion) == (int)COMPAREVERSION.OLDER)
                            {
                                textoutput("<YELLOW>Warning Media Portal Plugins:\n Your Actual Media Portal version is lower than your backup data.\nIf your backup data contain plugins downloaded from the internet they may not work or cause instabilities.\nIn that case please check the plugin home page for updates.\nIf you did not download any plugins from the internet you can ignore this message.\n");
                                DirectoryCopy(pathfolder + @"\MP_Program\plugins", DIR_Plugins, "*", false, DEBUG, true);//overwrite,verbose,recursive
                            }
                            else
                            {
                                DirectoryCopy(pathfolder + @"\MP_Program\plugins", DIR_Plugins, "*", false, DEBUG, true);//overwrite,verbose,recursive
                            }
                        }
                        catch (Exception exc)
                        {
                            textoutput("<RED>Media Portal plugins import failed - Copying " + pathfolder + @"\MP_Program\plugins to " + DIR_Plugins + " failed");
                            if (DEBUG == true)
                            {
                                textoutput("<RED>Exception message is " + exc.Message);
                            }
                            return false;
                        }

                        //check for old and new online video .dll files and delete old one
                        if ((File.Exists(DIR_Plugins + @"\Windows\OnlineVideos.MediaPortal1.dll") == true) && (File.Exists(DIR_Plugins + @"\Windows\OnlineVideos.dll") == true))
                        {
                            try
                            {
                                File.Delete(DIR_Plugins + @"\Windows\OnlineVideos.dll");
                                textoutput("Old OnlineVideo plugin OnlineVideos.dll has been deleted");
                            }
                            catch (Exception exc)
                            {
                                textoutput("<RED>Deleting old Onlinevideo plugin OnlineVideos.dll failed - Online video plugin will not work. Please remove it manually in the MediaPortal plugin folder for Windows plugins. Do not remove it from the OnlineVideo Folder!");
                                if (DEBUG == true)
                                {
                                    textoutput("<RED>Exception message is " + exc.Message);
                                }
                                return false;
                            }

                        }

                        textoutput("Media Portal plugins restored");

                    }
                    else
                    {
                        textoutput("<RED>Error: Media Portal plugins restore failed - Folder " + pathfolder + @"\MP_Program\plugins does not exist");
                        //return false;  no abort if folder does not exist
                    }

                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.MP_Plugins);
                }

                // copy Musicplayer and do not overwrite data
                if (MPMusicPlayer == true)
                {
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.MP_MusicPlayer);
                    if (Directory.Exists(pathfolder + @"\MP_Program\MusicPlayer"))
                    {

                        try
                        {
                            DirectoryCopy(pathfolder + @"\MP_Program\MusicPlayer", MP_PROGRAM_FOLDER + @"\MusicPlayer", "*", false, DEBUG, true);//overwrite,verbose,recursive
                        }
                        catch (Exception exc)
                        {
                            textoutput("<RED>Media Portal MusicPlayer import failed - Copying " + pathfolder + @"\MP_Program\MusicPlayer to " + MP_PROGRAM_FOLDER + @"\MusicPlayer failed");
                            if (DEBUG == true)
                            {
                                textoutput("<RED>Exception message is " + exc.Message);
                            }
                            return false;
                        }
                        textoutput("Media Portal MusicPlayer restored");

                    }
                    else
                    {
                        textoutput("<RED>Error: Media Portal MusicPlayer restore failed - Folder " + pathfolder + @"\MP_Program\MusicPlayer does not exist");
                        //return false;  no abort if folder does not exist
                    }

                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.MP_MusicPlayer);
                }

                // copy Skins and do not overwrite data
                // copy languages and do not overwrite data
                string SourceDir = null;
                if (CompareVersions(BackupMediaPortalVersion, "1.0.3.0") == (int)COMPAREVERSION.NEWER)
                {
                    SourceDir = @"\MP_User";
                }
                else
                {
                    SourceDir = @"\MP_Program";
                }

                if (MPSkins == true)
                {
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.MP_Skins);
                    if (Directory.Exists(pathfolder + SourceDir + @"\skin"))
                    {
                        try
                        {
                            if ((CompareVersions(ActualMediaPortalVersion, "1.1.6.0") == (int)COMPAREVERSION.NEWER) && (CompareVersions(BackupMediaPortalVersion, "1.1.6.0") == (int)COMPAREVERSION.OLDER))
                            {
                                textoutput("<YELLOW>Warning Skins:\nThis program version contains significant skin changes.\nYour skins will not be copied.\n");
                            }
                            else if ((CompareVersions(ActualMediaPortalVersion, "1.1.6.0") == (int)COMPAREVERSION.OLDER) && (CompareVersions(BackupMediaPortalVersion, "1.1.6.0") == (int)COMPAREVERSION.NEWER))
                            {
                                textoutput("<YELLOW>Warning Skins:\nThis program version contains significant skin changes.\nYour skins will not be copied.\n");
                            }
                            else if ((CompareVersions(ActualMediaPortalVersion, "1.0.3.0") == (int)COMPAREVERSION.NEWER) && (CompareVersions(BackupMediaPortalVersion, "1.0.3.0") == (int)COMPAREVERSION.OLDER))
                            {
                                textoutput("<YELLOW>Warning Skins:\nThis program version contains significant skin changes.\nYour skins will not be copied.\n");
                            }
                            else if ((CompareVersions(ActualMediaPortalVersion, "1.0.3.0") == (int)COMPAREVERSION.OLDER) && (CompareVersions(BackupMediaPortalVersion, "1.0.3.0") == (int)COMPAREVERSION.NEWER))
                            {
                                textoutput("<YELLOW>Warning Skins:\nThis program version contains significant skin changes.\nYour skins will not be copied.\n");
                            }
                            else if (CompareVersions(ActualMediaPortalVersion, BackupMediaPortalVersion) == (int)COMPAREVERSION.OLDER)
                            {
                                textoutput("<YELLOW>Warning Skins:\n Your Actual MediaPortal version is lower than your backup data.\nIf your backup data contain skins downloaded from the internet they may not work or cause instabilities.\nIn that case please check the skin home page for updates.\nIf you did not download any skins from the internet you can ignore this message.\n");
                                DirectoryCopy(pathfolder + SourceDir + @"\skin", DIR_Skin, "*", false, DEBUG, true);//overwrite,verbose,recursive
                            }
                            else
                            {
                                DirectoryCopy(pathfolder + SourceDir + @"\skin", DIR_Skin, "*", false, DEBUG, true);//overwrite,verbose,recursive
                            }
                        }
                        catch (Exception exc)
                        {
                            textoutput("<RED>Media Portal skins import failed - Copying " + pathfolder + SourceDir + @"\skin to " + DIR_Skin + " failed with exception message: " + exc.Message);
                            return false;
                        }
                    }
                    else
                    {
                        textoutput("<RED>Media Portal skins restore failed - Folder " + pathfolder + SourceDir + @"\skin does not exist");
                        //return false;  no abort if folder does not exist
                    }
                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.MP_Skins);

                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.MP_Language);
                    if (Directory.Exists(pathfolder + SourceDir + @"\language"))
                    {
                        try
                        {
                            DirectoryCopy(pathfolder + SourceDir + @"\language", DIR_Language, "*", false, DEBUG, true);//overwrite,verbose,recursive   added new: changed from true to false
                        }
                        catch (Exception exc)
                        {
                            textoutput("<RED>Media Portal language import failed - Copying " + pathfolder + SourceDir + @"\language to " + DIR_Language + " failed with exception message: " + exc.Message);
                            return false;
                        }
                        textoutput("Media Portal skins and languages restored");
                    }
                    else
                    {
                        textoutput("<RED>Media Portal language restore failed - Folder " + pathfolder + SourceDir + @"\language does not exist");
                        //return false;  no abort if folder does not exist
                    }

                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.MP_Language);
                }



                // copy User XML and overwrite data
                if (MPUserXML == true)
                {
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.MP_UserXML);
                    if (Directory.Exists(pathfolder + @"\MP_User"))
                    {
                        try
                        {
                            DirectoryCopy(pathfolder + @"\MP_User", DIR_Config, "*.xml", true, DEBUG, false);//overwrite,verbose,recursive
                        }
                        catch (Exception exc)
                        {
                            textoutput("<RED>Media Portal user .xml import failed - Copying " + pathfolder + @"\MP_User to " + DIR_Config + " failed with exception message: " + exc.Message);
                            return false;
                        }
                        textoutput("Media Portal User .xml files restored");
                    }
                    else
                    {
                        textoutput("<RED>Media Portal user .xml restore failed - Folder " + pathfolder + @"\MP_User does not exist");
                        //return false;  no abort if folder does not exist
                    }

                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.MP_UserXML);
                }

                // copy all MP user folders back and overwrite data
                if (MPAllFolder == true)
                {
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.MP_AllMediaPortalUserFolders);

                    if (Directory.Exists(pathfolder + @"\MP_User") == true)
                    {
                        DirectoryInfo root = new DirectoryInfo(pathfolder + @"\MP_User");

                        DirectoryInfo[] dirs = root.GetDirectories("*", SearchOption.TopDirectoryOnly);

                        foreach (DirectoryInfo subDir in dirs)
                        {
                            FileAttributes attributes = subDir.Attributes;
                            string destination = "";
                            bool overwrite = true;


                            if ((subDir.Name == "database"))
                            {//do nothing
                            }
                            else if ((subDir.Name == "InputDeviceMappings"))
                            {//do nothing
                            }
                            else if ((subDir.Name == "thumbs"))
                            {//do nothing
                            }
                            else if ((subDir.Name == "xmltv"))
                            {//do nothing
                            }
                            else if (subDir.Name == "skin")
                            {//do nothing
                                //skins are treated above
                            }
                            else if (subDir.Name == "language")
                            {//do nothing
                                //languages are treated above
                            }
                            else if (subDir.Name == "Installer")
                            { //do not copy installation directory between 1.1 and 1.2 up/down grade
                                if ((CompareVersions(ActualMediaPortalVersion, "1.1.6.0") == (int)COMPAREVERSION.NEWER) && (CompareVersions(BackupMediaPortalVersion, "1.1.6.0") == (int)COMPAREVERSION.OLDER))
                                { }
                                else if ((CompareVersions(ActualMediaPortalVersion, "1.1.6.0") == (int)COMPAREVERSION.OLDER) && (CompareVersions(BackupMediaPortalVersion, "1.1.6.0") == (int)COMPAREVERSION.NEWER))
                                { }
                                else
                                {// copy file
                                    try
                                    {
                                        if (((attributes & FileAttributes.Hidden) != FileAttributes.Hidden) && ((attributes & FileAttributes.System) != FileAttributes.System))
                                        { // do not copy hidden or system directories



                                            destination = MP_USER_FOLDER + @"\" + subDir.Name;
                                            DirectoryCopy(pathfolder + @"\MP_User\" + subDir.Name, destination, "*", overwrite, DEBUG, true);  //overwrite,verbose,recursive
                                            textoutput("Media Portal " + subDir.Name + " restored");
                                        }
                                    }
                                    catch (Exception exc)
                                    {
                                        textoutput("<RED>Could not copy Media Portal setting folder " + pathfolder + @"\MP_User\" + subDir.Name + " - Exception: " + exc.Message);
                                        return false;
                                    }
                                }
                            }
                            else  //copy file
                            {
                                try
                                {
                                    if ((subDir.Name != "BackupSettings") && ((attributes & FileAttributes.Hidden) != FileAttributes.Hidden) && ((attributes & FileAttributes.System) != FileAttributes.System))
                                    { // do not copy hidden or system directories



                                        destination = MP_USER_FOLDER + @"\" + subDir.Name;
                                        DirectoryCopy(pathfolder + @"\MP_User\" + subDir.Name, destination, "*", overwrite, DEBUG, true);  //overwrite,verbose,recursive
                                        textoutput("Media Portal " + subDir.Name + " restored");
                                    }
                                }
                                catch (Exception exc)
                                {
                                    textoutput("<RED>Could not copy Media Portal setting folder " + pathfolder + @"\MP_User\" + subDir.Name + " - Exception: " + exc.Message);
                                    return false;
                                }
                            }
                        }
                    }
                    else
                    {
                        textoutput("<RED>Media Portal MP_User restore failed - Folder " + pathfolder + @"\MP_User does not exist");
                    }
                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.MP_AllMediaPortalUserFolders);
                }


                // copy database and overwrite data
                if (MPDatabase == true)
                {
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.MP_Database);
                    if (Directory.Exists(pathfolder + @"\MP_User\database"))
                    {
                        try
                        {
                            if ((CompareVersions(ActualMediaPortalVersion, "1.0.3.0") == (int)COMPAREVERSION.NEWER) && (CompareVersions(BackupMediaPortalVersion, "1.0.3.0") == (int)COMPAREVERSION.OLDER))
                            {
                                textoutput("<YELLOW>Warning Media Portal Database:\n You are upgrading to a version where the format of the Media Portal database has been changed. You need to regenerate all Media Portal databases after the import.");
                            }
                            else if (CompareVersions(ActualMediaPortalVersion, BackupMediaPortalVersion) == (int)COMPAREVERSION.OLDER)
                            {
                                textoutput("<YELLOW>Warning Media Portal Database:\n Your Actual MediaPortal version is lower than your backup data.\nYou are downgrading to a version where the format of the Media Portal database has been changed. You need to regenerate all Media Portal databases after the import.");
                            }
                            DirectoryCopy(pathfolder + @"\MP_User\database", DIR_Database, "*", true, DEBUG, true); //overwrite,verbose,recursive
                        }
                        catch (Exception exc)
                        {
                            textoutput("<RED>Media Portal database import failed - Copying " + pathfolder + @"\MP_User\database to " + DIR_Database + " failed with exception message: " + exc.Message);
                            return false;
                        }
                        textoutput("Media Portal database restored");
                    }
                    else
                    {
                        textoutput("<RED>Media Portal database restore failed - Folder " + pathfolder + @"\MP_User\database does not exist");
                        //return false;  no abort if folder does not exist
                    }

                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.MP_Database);
                }

                // copy Input Device mappings and overwrite data
                if (MPInputDevice == true)
                {
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.MP_InputDeviceMappings);
                    if (Directory.Exists(pathfolder + @"\MP_User\InputDeviceMappings")) //InputDeviceMapping directory may not exit if no usersettings are stored
                    {
                        try
                        {
                            DirectoryCopy(pathfolder + @"\MP_User\InputDeviceMappings", DIR_CustomInputDevice, "*", true, DEBUG, true); //overwrite,verbose,recursive
                        }
                        catch (Exception exc)
                        {
                            textoutput("<RED>Media Portal database import failed - Copying " + pathfolder + @"\MP_User\InputDeviceMappings to " + DIR_CustomInputDevice + " failed with exception message: " + exc.Message);
                            return false;
                        }
                        textoutput("Media Portal input device mappings restored");
                    }
                    else
                    {
                        textoutput("<RED>Media Portal input device mappings not found");
                    }

                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.MP_InputDeviceMappings);
                }

                // copy Thumbs and overwrite data
                if (MPThumbs == true)
                {
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.MP_Thumbs);
                    if (Directory.Exists(pathfolder + @"\MP_User\thumbs"))
                    {
                        try
                        {
                            DirectoryCopy(pathfolder + @"\MP_User\thumbs", DIR_Thumbs, "*", true, DEBUG, true);//overwrite,verbose,recursive
                        }
                        catch (Exception exc)
                        {
                            textoutput("<RED>Media Portal thumbs import failed - Copying " + pathfolder + @"\MP_User\thumbs to " + DIR_Thumbs + " failed with exception message: " + exc.Message);
                            return false;
                        }
                        textoutput("Media Portal thumbs restored");
                    }
                    else
                    {
                        textoutput("<RED>Media Portal thumbs restore copy failed - Folder " + pathfolder + @"\MP_User\thumbs does not exist");
                        //return false;  no abort if folder does not exist
                    }

                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.MP_Thumbs);
                }

                // copy XmlTV and overwrite data
                if (MPxmltv == true)
                {
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.MP_Xmltv);
                    if (Directory.Exists(pathfolder + @"\MP_User\xmltv"))
                    {
                        try
                        {
                            DirectoryCopy(pathfolder + @"\MP_User\xmltv", MP_USER_FOLDER + @"\xmltv", "*", true, DEBUG, true);//overwrite,verbose,recursive
                        }
                        catch (Exception exc)
                        {
                            textoutput("<RED>Media Portal XmlTV import failed - Copying " + pathfolder + @"\MP_User\xmltv to " + MP_USER_FOLDER + @"\xmltv failed with exception message: " + exc.Message);
                            return false;
                        }
                        textoutput("Media Portal XmlTV restored");
                    }
                    else
                    {
                        textoutput("<RED>Media Portal XmlTV restore failed - Folder " + pathfolder + @"\MP_User\xmltv does not exist");
                        //return false;  no abort if folder does not exist
                    }

                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.MP_Xmltv);
                }



                //delete cache
                if (MPDeleteCache == true)
                {
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.MP_DeleteCache);
                    if (Directory.Exists(MP_USER_FOLDER + @"\Cache"))
                    {

                        try
                        {
                            Directory.Delete(DIR_Cache, true);
                        }
                        catch (Exception exc)
                        {
                            textoutput("<RED>Could not delete Cache folder " + DIR_Cache + " - Exception: " + exc.Message);
                            return false;
                        }
                        textoutput("Media Portal cache folder deleted");
                    }
                    else
                    {
                        textoutput("Media Portal cache folder did not exist");
                    }
                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.MP_DeleteCache);

                }

                //delete bass_wv.dll for new SVN
                FileVersionInfo mydllFileVersionInfo = FileVersionInfo.GetVersionInfo(MP_PROGRAM_FOLDER + @"\bass.dll");

                if ((CompareVersions(mydllFileVersionInfo.FileVersion, "2.3.0.0") == (int)COMPAREVERSION.NEWER) && (File.Exists((MP_PROGRAM_FOLDER + @"\MusicPlayer\plugins\audio decoders\bass_wv.dll")) == true))  //new bass.dll version
                {
                    textoutput("Deleting old bass_wv.dll");
                    File.Delete(MP_PROGRAM_FOLDER + @"\MusicPlayer\plugins\audio decoders\bass_wv.dll");
                }


                //user batch script MP import
                backupscripts(pathfolder, Scripts.MP_Import);
                textoutput("Import completed");
                //---------------------------------
                //end copy for BackupSettingsMP
                //--------------------------------- 

            }//end Mediaportal1
            #endregion MediaPortal1

            //*************************    
            // Media Portal2 Server
            //*************************
            #region MP2Server
            if (MP2S == false)
            {
                textoutput("Media Portal2 Server not checked - will not restore settings");
            }
            else
            {

                // check installation folder and autodetect

                if (File.Exists(SV2_PROGRAM_FOLDER + "\\MP2-Server.exe") == false)
                {
                    textoutput("<RED>Media Portal2 Server program folder \n" + SV2_PROGRAM_FOLDER + "\\MP2-Server.exe \ndoes not exist - aborting import");
                    return false;
                }

                if (Directory.Exists(SV2_USER_FOLDER) == false)
                {
                    textoutput("<RED>Media Portal2 Server data folder does not exist - aborting import");
                    return false;
                }
                // save settings before import after auto folder detection
                //MySaveSettings();

                //read all version numbers for user warnings
                getallversionnumbers(pathfolder,true);


                //copy Defaults first and read paths afterwards
                // copy Defaults and overwrite data
                if (SV2Defaults == true)
                {
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.SV2_Defaults);
                    if (Directory.Exists(pathfolder + @"\SV2_PROGRAM\Defaults"))
                    {
                        try
                        {
                            DirectoryCopy(pathfolder + @"\SV2_PROGRAM\Defaults", SV2_PROGRAM_FOLDER + @"\Defaults", "*", true, DEBUG, true);//overwrite,verbose,recursive
                        }
                        catch (Exception exc)
                        {
                            textoutput("<RED>Media Portal2 Server Defaults import failed - Copying " + pathfolder + @"\SV2_PROGRAM\Defaults to " + SV2_PROGRAM_FOLDER + @"\Defaults failed with exception message: " + exc.Message);
                            return false;
                        }
                        textoutput("Media Portal2 Server Defaults restored");
                    }
                    else
                    {
                        textoutput("<RED>Media Portal2 Server Defaults restore failed - Folder " + pathfolder + @"\SV2_PROGRAM\Defaults does not exist");
                        //return false;  no abort if folder does not exist
                    }

                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.SV2_Defaults);
                }

                //read paths
                instpaths.DEBUG = DEBUG;
                instpaths.GetMediaPortalDirsMP2();

                // copy all MP2 Server user folders back and overwrite data
                if (SV2AllServerFolders == true)
                {
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.SV2_AllServerFolders);

                    if (Directory.Exists(pathfolder + @"\SV2_USER") == true)
                    {
                        DirectoryInfo root = new DirectoryInfo(pathfolder + @"\SV2_USER");

                        DirectoryInfo[] dirs = root.GetDirectories("*", SearchOption.TopDirectoryOnly);

                        foreach (DirectoryInfo subDir in dirs)
                        {
                            FileAttributes attributes = subDir.Attributes;
                            string destination = "";
                            bool overwrite = true; //true for user data as default

                            if ((subDir.Name.ToLower() == "plugins"))
                            {//do nothing
                            }
                            else if ((subDir.Name.ToLower() == "defaults"))
                            {//do nothing
                            }
                            else if ((subDir.Name.ToLower() == "config"))
                            {//do nothing
                            }
                            else if ((subDir.Name.ToLower() == "database"))
                            {//do nothing
                            }

                            else  //copy file
                            {
                                try
                                {
                                    if ((subDir.Name != "BackupSettings") && ((attributes & FileAttributes.Hidden) != FileAttributes.Hidden) && ((attributes & FileAttributes.System) != FileAttributes.System))
                                    { // do not copy hidden or system directories
                                        destination = SV2_USER_FOLDER + @"\" + subDir.Name;
                                        DirectoryCopy(pathfolder + @"\SV2_USER\" + subDir.Name, destination, "*", overwrite, DEBUG, true);  //overwrite,verbose,recursive
                                        textoutput("Media Portal2 Server User Data: " + subDir.Name + " restored");
                                    }
                                }
                                catch (Exception exc)
                                {
                                    textoutput("<RED>Could not copy Media Portal2 Server folder " + pathfolder + @"\SV2_USER\" + subDir.Name + " - Exception: " + exc.Message);
                                    return false;
                                }
                            }
                        }
                    }
                    else
                    {
                        textoutput("<RED>Media Portal2 SV2_USER restore failed - Folder " + pathfolder + @"\SV2_USER does not exist");
                    }
                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.SV2_AllServerFolders);
                }


                if (SV2AllServerProgramFolders == true)
                {
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.SV2_AllServerProgramFolders);

                    if (Directory.Exists(pathfolder + @"\SV2_PROGRAM") == true)
                    {
                        DirectoryInfo root = new DirectoryInfo(pathfolder + @"\SV2_PROGRAM");

                        DirectoryInfo[] dirs = root.GetDirectories("*", SearchOption.TopDirectoryOnly);

                        foreach (DirectoryInfo subDir in dirs)
                        {
                            FileAttributes attributes = subDir.Attributes;
                            string destination = "";
                            bool overwrite = false; //false for program data as default

                            if ((subDir.Name.ToLower() == "plugins"))
                            {//do nothing
                            }
                            else if ((subDir.Name.ToLower() == "defaults"))
                            {//do nothing
                            }
                            else if ((subDir.Name.ToLower() == "config"))
                            {//do nothing
                            }
                            else if ((subDir.Name.ToLower() == "database"))
                            {//do nothing
                            }

                            else  //copy file
                            {
                                try
                                {
                                    if ((subDir.Name != "BackupSettings") && ((attributes & FileAttributes.Hidden) != FileAttributes.Hidden) && ((attributes & FileAttributes.System) != FileAttributes.System))
                                    { // do not copy hidden or system directories
                                        destination = SV2_PROGRAM_FOLDER + @"\" + subDir.Name;
                                        DirectoryCopy(pathfolder + @"\SV2_PROGRAM\" + subDir.Name, destination, "*", overwrite, DEBUG, true);  //overwrite,verbose,recursive
                                        textoutput("Media Portal2 Server Program Data: " + subDir.Name + " restored");
                                    }
                                }
                                catch (Exception exc)
                                {
                                    textoutput("<RED>Could not copy Media Portal2 Server program folder " + pathfolder + @"\SV2_PROGRAM\" + subDir.Name + " - Exception: " + exc.Message);
                                    return false;
                                }
                            }
                        }
                    }
                    else
                    {
                        textoutput("<RED>Media Portal2 SV2_PROGRAM restore failed - Folder " + pathfolder + @"\SV2_PROGRAM does not exist");
                    }
                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.SV2_AllServerProgramFolders);
                }


                // copy Program setting files and do not overwrite data
                if (SV2AllServerFiles == true)
                {
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.SV2_AllServerFiles);
                    if (Directory.Exists(pathfolder + @"\SV2_USER"))
                    {
                        try
                        {
                            DirectoryCopy(pathfolder + @"\SV2_USER", SV2_USER_FOLDER, "*.*", true, DEBUG, false);//overwrite,verbose,recursive
                        }
                        catch (Exception exc)
                        {
                            textoutput("<RED>Media Portal2 Server user file restoring failed - Copying " + pathfolder + @"\SV2_USER\*.* to " + SV2_USER_FOLDER + " failed with exception message: " + exc.Message);
                            return false;
                        }
                        textoutput("Media Portal2 Server user files  restored");
                    }
                    else
                    {
                        textoutput("<RED>Error: Media Portal2 Server user file restore failed - Folder " + pathfolder + @"\SV2_USER does not exist");
                    }

                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.SV2_AllServerFiles);
                }

                // copy Config and overwrite data
                if (SV2Configuration == true)
                {
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.SV2_Config);
                    if (Directory.Exists(pathfolder + @"\SV2_USER\Config"))
                    {
                        try
                        {
                            DirectoryCopy(pathfolder + @"\SV2_USER\Config", instpaths.DIR_SV2_Config, "*", true, DEBUG, true);//overwrite,verbose,recursive
                        }
                        catch (Exception exc)
                        {
                            textoutput("<RED>Media Portal2 Server Config import failed - Copying " + pathfolder + @"\SV2_USER\Config to " + instpaths.DIR_SV2_Config + " failed with exception message: " + exc.Message);
                            return false;
                        }
                        textoutput("Media Portal2 Server Config restored");
                    }
                    else
                    {
                        textoutput("<RED>Media Portal2 Server Config restore failed - Folder " + pathfolder + @"\SV2_USER\Config does not exist");
                        //return false;  no abort if folder does not exist
                    }

                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.SV2_Config);
                }

                // copy Plugins and do not overwrite data
                if (SV2Plugins == true)
                {
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.SV2_Plugins);
                    if (Directory.Exists(pathfolder + @"\SV2_PROGRAM\Plugins"))
                    {
                        try
                        {
                            DirectoryCopy(pathfolder + @"\SV2_PROGRAM\Plugins", instpaths.DIR_SV2_Plugins, "*", false, DEBUG, true);//overwrite,verbose,recursive
                        }
                        catch (Exception exc)
                        {
                            textoutput("<RED>Media Portal2 Server Plugins import failed - Copying " + pathfolder + @"\SV2_PROGRAM\Plugins to " + instpaths.DIR_SV2_Plugins + " failed with exception message: " + exc.Message);
                            return false;
                        }
                        textoutput("Media Portal2 Server Plugins restored");
                    }
                    else
                    {
                        textoutput("<RED>Media Portal2 Server Plugins restore failed - Folder " + pathfolder + @"\SV2_PROGRAM\Plugins does not exist");
                        //return false;  no abort if folder does not exist
                    }

                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.SV2_Plugins);
                }

                // copy database and overwrite data
                if (SV2Database == true)
                {
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.SV2_Database);
                    if (File.Exists(pathfolder + @"\SV2_USER\database\Datastore.sdf"))
                    {
                        try
                        {
                            File.Copy(pathfolder + @"\SV2_USER\database\Datastore.sdf", instpaths.DIR_SV2_Database + @"\Datastore.sdf", true);
                        }
                        catch (Exception exc)
                        {
                            textoutput("<RED>Media Portal2 Server Database import failed - Copying " + pathfolder + @"\SV2_USER\database\Datastore.sdf to " + instpaths.DIR_SV2_Database + @"\Datastore.sdf" + " failed with exception message: " + exc.Message);
                            return false;
                        }
                        textoutput("Media Portal2 Server Database restored");
                    }
                    else
                    {
                        textoutput("<RED>Media Portal2 Server database restore failed - Folder " + pathfolder + @"\SV2_USER\database\Datastore.sdf does not exist");
                        //return false;  no abort if folder does not exist
                    }

                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.SV2_Database);
                }


            }//end MP2 Server
            #endregion MP2Server

            //*************************    
            // Media Portal2 Client
            //*************************
            #region MP2Client
            if (MP2C == false)
            {
                textoutput("Media Portal2 Client not checked - will not restore settings");
            }
            else
            {

                // check installation folder and autodetect

                if (File.Exists(MP2_PROGRAM_FOLDER + "\\MP2-Client.exe") == false)
                {
                    textoutput("<RED>Media Portal2 Client program folder \n" + MP2_PROGRAM_FOLDER + "\\MP2-Client.exe \ndoes not exist - aborting import");
                    return false;
                }

                if (Directory.Exists(MP2_USER_FOLDER) == false)
                {
                    textoutput("<RED>Media Portal2 Client data folder \n" + MP2_USER_FOLDER + "does not exist - aborting import");
                    return false;
                }
                // save settings before import after auto folder detection
                //MySaveSettings();

                //read all version numbers for user warnings
                getallversionnumbers(pathfolder,true);


                //copy Defaults first and read paths afterwards
                // copy Defaults and overwrite data
                if (MP2Defaults == true)
                {
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.MP2_Defaults);
                    if (Directory.Exists(pathfolder + @"\MP2_PROGRAM\Defaults"))
                    {
                        try
                        {
                            DirectoryCopy(pathfolder + @"\MP2_PROGRAM\Defaults", MP2_PROGRAM_FOLDER + @"\Defaults", "*", true, DEBUG, true);//overwrite,verbose,recursive
                        }
                        catch (Exception exc)
                        {
                            textoutput("<RED>Media Portal2 Client Defaults import failed - Copying " + pathfolder + @"\MP2_PROGRAM\Defaults to " + MP2_PROGRAM_FOLDER + @"\Defaults failed with exception message: " + exc.Message);
                            return false;
                        }
                        textoutput("Media Portal2 Client Defaults restored");
                    }
                    else
                    {
                        textoutput("<RED>Media Portal2 Client Defaults restore failed - Folder " + pathfolder + @"\MP2_PROGRAM\Defaults does not exist");
                        //return false;  no abort if folder does not exist
                    }

                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.MP2_Defaults);
                }

                //read paths
                instpaths.DEBUG = DEBUG;
                instpaths.GetMediaPortalDirsMP2();

                // copy all MP2 Server user folders back and overwrite data
                if (MP2AllClientFolders == true)
                {
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.MP2_AllClientFolders);

                    if (Directory.Exists(pathfolder + @"\MP2_USER") == true)
                    {
                        DirectoryInfo root = new DirectoryInfo(pathfolder + @"\MP2_USER");

                        DirectoryInfo[] dirs = root.GetDirectories("*", SearchOption.TopDirectoryOnly);

                        foreach (DirectoryInfo subDir in dirs)
                        {
                            FileAttributes attributes = subDir.Attributes;
                            string destination = "";
                            bool overwrite = true; //true for user data as default

                            if ((subDir.Name.ToLower() == "plugins"))
                            {//do nothing
                            }
                            else if ((subDir.Name.ToLower() == "defaults"))
                            {//do nothing
                            }
                            else if ((subDir.Name.ToLower() == "config"))
                            {//do nothing
                            }
                            else if ((subDir.Name.ToLower() == "database"))
                            {//do nothing
                            }

                            else  //copy file
                            {
                                try
                                {
                                    if ((subDir.Name != "BackupSettings") && ((attributes & FileAttributes.Hidden) != FileAttributes.Hidden) && ((attributes & FileAttributes.System) != FileAttributes.System))
                                    { // do not copy hidden or system directories
                                        destination = MP2_USER_FOLDER + @"\" + subDir.Name;
                                        DirectoryCopy(pathfolder + @"\MP2_USER\" + subDir.Name, destination, "*", overwrite, DEBUG, true);  //overwrite,verbose,recursive
                                        textoutput("Media Portal2 Client User Data: " + subDir.Name + " restored");
                                    }
                                }
                                catch (Exception exc)
                                {
                                    textoutput("<RED>Could not copy Media Portal2 Client folder " + pathfolder + @"\MP2_USER\" + subDir.Name + " - Exception: " + exc.Message);
                                    return false;
                                }
                            }
                        }
                    }
                    else
                    {
                        textoutput("<RED>Media Portal2 MP2_USER restore failed - Folder " + pathfolder + @"\MP2_USER does not exist");
                    }
                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.MP2_AllClientFolders);
                }


                if (MP2AllClientProgramFolders == true)
                {
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.MP2_AllClientProgramFolders);

                    if (Directory.Exists(pathfolder + @"\MP2_PROGRAM") == true)
                    {
                        DirectoryInfo root = new DirectoryInfo(pathfolder + @"\MP2_PROGRAM");

                        DirectoryInfo[] dirs = root.GetDirectories("*", SearchOption.TopDirectoryOnly);

                        foreach (DirectoryInfo subDir in dirs)
                        {
                            FileAttributes attributes = subDir.Attributes;
                            string destination = "";
                            bool overwrite = false; //false for program data as default

                            if ((subDir.Name.ToLower() == "plugins"))
                            {//do nothing
                            }
                            else if ((subDir.Name.ToLower() == "defaults"))
                            {//do nothing
                            }
                            else if ((subDir.Name.ToLower() == "config"))
                            {//do nothing
                            }

                            else  //copy file
                            {
                                try
                                {
                                    if ((subDir.Name != "BackupSettings") && ((attributes & FileAttributes.Hidden) != FileAttributes.Hidden) && ((attributes & FileAttributes.System) != FileAttributes.System))
                                    { // do not copy hidden or system directories
                                        destination = MP2_PROGRAM_FOLDER + @"\" + subDir.Name;
                                        DirectoryCopy(pathfolder + @"\MP2_PROGRAM\" + subDir.Name, destination, "*", overwrite, DEBUG, true);  //overwrite,verbose,recursive
                                        textoutput("Media Portal2 Client Program Data: " + subDir.Name + " restored");
                                    }
                                }
                                catch (Exception exc)
                                {
                                    textoutput("<RED>Could not copy Media Portal2 Client program folder " + pathfolder + @"\MP2_PROGRAM\" + subDir.Name + " - Exception: " + exc.Message);
                                    return false;
                                }
                            }
                        }
                    }
                    else
                    {
                        textoutput("<RED>Media Portal2 Client MP2_PROGRAM restore failed - Folder " + pathfolder + @"\MP2_PROGRAM does not exist");
                    }
                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.MP2_AllClientProgramFolders);
                }


                // copy Program setting files and do not overwrite data
                if (MP2AllClientFiles == true)
                {
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.MP2_AllClientFiles);
                    if (Directory.Exists(pathfolder + @"\MP2_USER"))
                    {
                        try
                        {
                            DirectoryCopy(pathfolder + @"\MP2_USER", MP2_USER_FOLDER, "*.*", true, DEBUG, false);//overwrite,verbose,recursive
                        }
                        catch (Exception exc)
                        {
                            textoutput("<RED>Media Portal2 Client user file restoring failed - Copying " + pathfolder + @"\MP2_USER\*.* to " + MP2_USER_FOLDER + " failed with exception message: " + exc.Message);
                            return false;
                        }
                        textoutput("Media Portal2 Client user files restored");
                    }
                    else
                    {
                        textoutput("<RED>Error: Media Portal2 Client user file restore failed - Folder " + pathfolder + @"\MP2_USER does not exist");
                    }

                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.MP2_AllClientFiles);
                }

                // copy Config and overwrite data
                if (MP2Config == true)
                {
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.MP2_Config);
                    if (Directory.Exists(pathfolder + @"\MP2_USER\Config"))
                    {
                        try
                        {
                            DirectoryCopy(pathfolder + @"\MP2_USER\Config", instpaths.DIR_MP2_Config, "*", true, DEBUG, true);//overwrite,verbose,recursive
                        }
                        catch (Exception exc)
                        {
                            textoutput("<RED>Media Portal2 Client Config import failed - Copying " + pathfolder + @"\MP2_USER\Config to " + instpaths.DIR_MP2_Config + " failed with exception message: " + exc.Message);
                            return false;
                        }
                        textoutput("Media Portal2 Client Config restored");
                    }
                    else
                    {
                        textoutput("<RED>Media Portal2 Client Config restore failed - Folder " + pathfolder + @"\MP2_USER\Config does not exist");
                        //return false;  no abort if folder does not exist
                    }

                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.MP2_Config);
                }

                // copy Plugins and do not overwrite data
                if (MP2Plugins == true)
                {
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.MP2_Plugins);
                    if (Directory.Exists(pathfolder + @"\MP2_PROGRAM\Plugins"))
                    {
                        try
                        {
                            DirectoryCopy(pathfolder + @"\MP2_PROGRAM\Plugins", instpaths.DIR_MP2_Plugins, "*", false, DEBUG, true);//overwrite,verbose,recursive
                        }
                        catch (Exception exc)
                        {
                            textoutput("<RED>Media Portal2 Client Plugins import failed - Copying " + pathfolder + @"\MP2_PROGRAM\Plugins to " + instpaths.DIR_MP2_Plugins + " failed with exception message: " + exc.Message);
                            return false;
                        }
                        textoutput("Media Portal2 Client Plugins restored");
                    }
                    else
                    {
                        textoutput("<RED>Media Portal2 Client Plugins restore failed - Folder " + pathfolder + @"\MP2_PROGRAM\Plugins does not exist");
                        //return false;  no abort if folder does not exist
                    }

                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.MP2_Plugins);
                }
            }
            #endregion MP2Client


            MySaveSettings(); //update progressbar for operation
            time2 = DateTime.Now;
            string text = "Import operation completed after " + time2.Subtract(time1).ToString();
            text = text.Substring(0, text.Length - 8);
            textoutput(text);


#if (TV)
            bool okflag = completeimportxml(); //  end tv server configuration and start post processing
#endif

            progressbar((int)PB_action.COMPLETE, ref PB_Import, 0);
            return true;
        }


#if (TV)
        public void CheckDuplicateGroupmembers(bool processing, bool verbose, bool messagebox)
        {
            textoutput("Checking duplicate group members");
            TvBusinessLayer layer = new TvBusinessLayer();

            bool firstflag = true;
#if(MP100)
            IList channelgroups = ChannelGroup.ListAll();
#elif(MP101) 
            IList<ChannelGroup> channelgroups = ChannelGroup.ListAll();
#else //MP11BETA or SVN
            IList<ChannelGroup> channelgroups = ChannelGroup.ListAll();
#endif

            foreach (ChannelGroup group in channelgroups)
            {
                try
                {
#if(MP100)
                    IList maps = group.ReferringGroupMap();
#elif (MP101)
                    IList<GroupMap> maps = group.ReferringGroupMap();
#else //MP11BETA or SVN
                    IList<GroupMap> maps = group.ReferringGroupMap();
#endif


                    GroupMap[] tvgroupmap = new GroupMap[maps.Count];
                    int i = 0;
                    foreach (GroupMap map in maps)
                    {
                        tvgroupmap[i++] = map;
                    }
                    for (i = 0; i < maps.Count; i++)
                    {
                        for (int j = i + 1; j < maps.Count; j++)
                        {
                            if ((tvgroupmap[i] != null) && (tvgroupmap[j] != null))
                            {
                                if (tvgroupmap[i].IdChannel == tvgroupmap[j].IdChannel)
                                {// found identical channel in group
                                    if (processing == true) //&&(duplicateRemoveGroup))
                                    {
                                        textoutput("Deleting duplicate channel id " + tvgroupmap[j].IdChannel + " from group " + group.GroupName);
                                        tvgroupmap[j].Remove();
                                        tvgroupmap[j] = null;
                                    }
                                    else if (verbose == true)
                                    {
                                        if (firstflag == true)
                                        {
                                            textoutput("Warning: Duplicate group members do exist- \n it is recommended to process \"Duplicate Channels\",\n check the option \"Remove Duplicate Group Members\" and redo the export");
                                            firstflag = false;
                                        }
                                        textoutput("<YELLOW>Warning: Duplicate channel id " + tvgroupmap[j].IdChannel + " in group " + group.GroupName);
                                        tvgroupmap[j] = null;
                                    }
                                    else
                                    {
                                        textoutput("<YELLOW>Warning: Duplicate group members do exist- \n " + tvgroupmap[j].IdChannel.ToString() + "\n- it is recommended to process \"Duplicate Channels\",\n check the option \"Remove Duplicate Group Members\" and redo the export");
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception exc)
                {
                    textoutput("<RED>Error in checking group " + group.GroupName + " for duplicate members");
                    textoutput("<RED>Excepption message is " + exc.Message);
                }
            }

#if(MP100)
            IList radiochannelgroups = RadioChannelGroup.ListAll();
#elif (MP101)
            IList<RadioChannelGroup> radiochannelgroups = RadioChannelGroup.ListAll();
#else //MP11BETA or SVN
            IList<RadioChannelGroup> radiochannelgroups = RadioChannelGroup.ListAll();
#endif

            foreach (RadioChannelGroup radiogroup in radiochannelgroups)
            {

                try
                {
#if(MP100)
                    IList radiomaps = radiogroup.ReferringRadioGroupMap();
#elif (MP101)
                    IList<RadioGroupMap> radiomaps = radiogroup.ReferringRadioGroupMap();
#else //MP11BETA or SVN
                    IList<RadioGroupMap> radiomaps = radiogroup.ReferringRadioGroupMap();
#endif


                    RadioGroupMap[] radiogroupmap = new RadioGroupMap[radiomaps.Count];
                    int i = 0;
                    foreach (RadioGroupMap radiomap in radiomaps)
                    {
                        radiogroupmap[i++] = radiomap;
                    }
                    for (i = 0; i < radiomaps.Count; i++)
                    {
                        for (int j = i + 1; j < radiomaps.Count; j++)
                        {
                            if ((radiogroupmap[i] != null) && (radiogroupmap[j] != null))
                            {
                                if (radiogroupmap[i].IdChannel == radiogroupmap[j].IdChannel)
                                {// found identical radiochannel in radiogroup
                                    if (processing == true)
                                    {
                                        textoutput("Deleting duplicate channel id " + radiogroupmap[j].IdChannel + " from radiogroup " + radiogroup.GroupName);
                                        radiogroupmap[j].Remove();
                                        radiogroupmap[j] = null;
                                    }
                                    else if (verbose == true)
                                    {
                                        textoutput("<YELLOW>Warning: Duplicate channel id " + radiogroupmap[j].IdChannel + " in radiogroup " + radiogroup.GroupName);
                                        radiogroupmap[j] = null;
                                    }
                                    else
                                    {
                                        textoutput("<YELLOW>Warning: Duplicate group members do exist- \n " + radiogroupmap[j].IdChannel.ToString() + "\n- it is recommended to process duplicate channel names and \n redo the export");
                                        return;
                                    }

                                }
                            }
                        }
                    }
                }
                catch (Exception exc)
                {
                    textoutput("<RED>Error in checking radiogroup " + radiogroup.GroupName + " for duplicate members");
                    if (DEBUG == true)
                    {
                        textoutput("<RED>Exception message is " + exc.Message);
                    }
                }

            }
            textoutput("Duplicate group member check completed");
            if (messagebox == true)
                MessageBox.Show("Duplicate group member check completed", "Info");
        }

        public void checkduplicatechannels(bool verbose, bool messagebox)
        {

            textoutput("Checking for duplicate channel names");
            TvBusinessLayer layer = new TvBusinessLayer();
#if(MP100)
            IList channellist = Channel.ListAll();
#elif(MP101)
            IList<Channel> channellist = Channel.ListAll();
#else //MP11BETA or SVN
            IList<Channel> channellist = Channel.ListAll();
#endif

            Channel[] channelarray = new Channel[channellist.Count];
            Channel tmpchannel;
            int i = 0, j = 0;
            foreach (Channel channel in channellist)
            {
                channelarray[i++] = channel;
            }

            for (i = 0; i < channellist.Count - 1; i++)
            {
                bool firstfound = true;
                for (j = i + 1; j < channellist.Count; j++)
                {
#if (MP12)
                    if (channelarray[i].DisplayName == channelarray[j].DisplayName)
#else 
                    if ((channelarray[i].Name == channelarray[j].Name) && (channelarray[i].DisplayName == channelarray[j].DisplayName))
#endif
                    {
                        if (firstfound == true)
                        {
                            if (verbose)
                            {
                                textoutput("\nDuplicate channel names found for channel " + channelarray[i].DisplayName);
                                textoutput("ID = " + channelarray[i].IdChannel);
                                firstfound = false;
                            }
                            else
                            {
                                textoutput("<YELLOW>Warning: Duplicate channel names were found - \n it is recommended to process duplicate channel names and \n redo the export");
                                return;
                            }
                        }
                        if (verbose)
                            textoutput("Duplicate ID = " + channelarray[j].IdChannel);

                        //sort duplicate channels in array
                        tmpchannel = channelarray[j];
                        channelarray[j] = channelarray[i + 1];
                        channelarray[i + 1] = tmpchannel;
                        i++;
                    }
                }
            }
            textoutput("Duplicate channel name check completed");
            if (messagebox)
            {
                myMessageBox("Duplicate channel check completed", "Info:", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }

        public void processduplicates(bool messagebox)
        {
            TvBusinessLayer layer = new TvBusinessLayer();
            textoutput("Processing duplicate channel names");

            //detect all duplicate channel names
#if(MP100)
            IList channellist = Channel.ListAll();
#elif(MP101)
            IList<Channel> channellist = Channel.ListAll();
#else //MP11BETA or SVN
            IList<Channel> channellist = Channel.ListAll();
#endif

            Channel[] channelarray = new Channel[channellist.Count];
            int i = 0, j = 0;
            foreach (Channel channel in channellist)
            {
                channelarray[i++] = channel;
            }

            for (i = 0; i < channellist.Count - 1; i++)
            {
                int count = 2;

                for (j = i + 1; j < channellist.Count; j++)
                {
#if(MP12)
                    if (channelarray[i].DisplayName == channelarray[j].DisplayName)
#else
                    if ((channelarray[i].Name == channelarray[j].Name) && (channelarray[i].DisplayName == channelarray[j].DisplayName))
#endif
                    {//channel name and MP display name do match

                        bool identicalchannel = comparechannels(channelarray[i], channelarray[j]);
                        bool identicalcards = comparecardmappings(channelarray[i], channelarray[j]);
                        bool identicaltuningdetails = comparetuningdetails(channelarray[i], channelarray[j]);

                        if (identicalchannel && identicalcards && identicaltuningdetails)
                        { //delete channel
                            if (duplicateinteractive == true) //&& (duplicatedelete == true))
                            {
                                switch (myMessageBox("Do you want to delete the duplicate channel " + channelarray[j].DisplayName + "?", "Confirm Delete Duplicate Channel", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                                {
                                    case DialogResult.Yes:
                                        {
                                            // "Yes" processing
                                            textoutput("Deleting duplicate channel " + channelarray[j].DisplayName);
                                            channelarray[j].Delete();
                                            break;
                                        }
                                    case DialogResult.No:
                                        {
                                            // "No" processing
                                            textoutput("Channel " + channelarray[j].DisplayName + " not deleted");
                                            break;
                                        }
                                    case DialogResult.Cancel:
                                        {
                                            textoutput("Operation has been canceled by user");
                                            return;
                                        }
                                }

                            }
                            else //if (duplicatedelete == true)
                            {
                                textoutput("Deleting duplicate channel " + channelarray[j].DisplayName);
                                channelarray[j].Delete();
                            }
                        }
                        else // rename
                        {
                            if (duplicateinteractive == true) //&& (duplicaterename == true))
                            {
                                switch (myMessageBox("Do you want to rename the duplicate channel " + channelarray[j].DisplayName + " to " + channelarray[j].DisplayName + "_" + count.ToString() + "?", "Confirm Rename Duplicate Channel", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                                {
                                    case DialogResult.Yes:
                                        {
                                            // "Yes" processing
                                            textoutput("Renaming duplicate channel " + channelarray[j].DisplayName + " to " + channelarray[j].DisplayName + "_" + count.ToString());
                                            channelarray[j].DisplayName = channelarray[j].DisplayName + "_" + count.ToString();
                                            channelarray[j].Persist();
                                            count++;
                                            break;
                                        }
                                    case DialogResult.No:
                                        {
                                            // "No" processing
                                            textoutput("Channel " + channelarray[j].DisplayName + " not renamed");
                                            break;
                                        }
                                    case DialogResult.Cancel:
                                        {
                                            textoutput("Operation has been canceled by user");
                                            return;
                                        }
                                }
                            }
                            else //if (duplicaterename == true)
                            {
                                textoutput("Renaming duplicate channel " + channelarray[j].DisplayName + " to " + channelarray[j].DisplayName + "_" + count.ToString());
                                channelarray[j].DisplayName = channelarray[j].DisplayName + "_" + count.ToString();
                                channelarray[j].Persist();
                                count++;
                            }
                        }
                    }
                }

            }
            textoutput("Duplicate channel processing completed");
            if (messagebox == true)
                myMessageBox("Duplicate channel processing completed", "Info:", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        public void undorename()
        {
            //detect all duplicate channel names
#if(MP100)
            IList channellist = Channel.ListAll();
#elif(MP101) 
            IList<Channel>channellist = Channel.ListAll();
#else //MP11BETA or SVN
            IList<Channel> channellist = Channel.ListAll();
#endif
            exportprogressBar.Minimum = 0;
            exportprogressBar.Maximum = channellist.Count;
            exportprogressBar.Step = 1;
            exportprogressBar.Value = 0;
            exportprogressBar.Update();
#if (MP12)
            foreach (Channel channel in channellist)
            {
                if (channel.DisplayName.Contains("_"))
                {
                    string[] tokens = (channel.DisplayName.Split('_'));
                    
                    try
                    {
                        int test = Convert.ToInt32(tokens[tokens.Length-1]);

                        //start renaming
                        string oldname = channel.DisplayName;
                        string newname = tokens[0];
                        for (int i = 1; i < tokens.Length - 1; i++)
                        {
                            newname += "_" + tokens[i];
                        }

                        //newname must exist already as a channel name if it was renamed before
                        foreach (Channel mychannel in channellist)
                        {
                            if (mychannel.DisplayName == newname)
                            {
                                textoutput("Renaming of channel " + oldname + " to " + newname);
                                channel.DisplayName = newname;
                                channel.Persist();
                                break;
                            }
                        }                       
                    }
                    catch{}                    
                }
                exportprogressBar.PerformStep();
                exportprogressBar.Update();
            }
            MessageBox.Show("Undo renaming completed", "Info:");
#else
            foreach (Channel channel in channellist)
            {
                if (channel.Name != channel.DisplayName)
                {
                    textoutput("Undoing renaming of channel " + channel.DisplayName + " to " + channel.Name);
                    channel.DisplayName = channel.Name;
                    channel.Persist();
                }
                exportprogressBar.PerformStep();
                exportprogressBar.Update();
            }
            MessageBox.Show("Undo renaming completed", "Info:");
#endif
        }

        public bool comparechannels(Channel channel1, Channel channel2)
        {

            //  Name  DisplayName  FreeToAir  IsRadio  IsTv 
#if(MP12)
            if ((channel1.IsRadio == channel2.IsRadio) && (channel1.IsTv == channel2.IsTv))
#else
            if ((channel1.Name == channel2.Name) &&  (channel1.IsRadio == channel2.IsRadio) && (channel1.IsTv == channel2.IsTv))  //removed free toair
#endif
            {


                // textoutput("Channel details equal for channel " + channel1.IdChannel + " and channel " + channel2.IdChannel);

                return true;
            }
            else
                return false;
        }

        public bool comparecardmappings(Channel channel1, Channel channel2)
        {
#if(MP100)
            IList allmaps1 = channel1.ReferringChannelMap();
#elif(MP101)
            IList<ChannelMap> allmaps1 = channel1.ReferringChannelMap();
#else //MP11BETA or SVN
            IList<ChannelMap> allmaps1 = channel1.ReferringChannelMap();
#endif

            ChannelMap[] channelmap1 = new ChannelMap[allmaps1.Count];
            int i = 0;
            foreach (ChannelMap map1 in allmaps1)
            {
                channelmap1[i++] = map1;
            }

#if(MP100)
            IList allmaps2 = channel2.ReferringChannelMap();
#elif(MP101)
            IList<ChannelMap> allmaps2 = channel2.ReferringChannelMap();
#else //MP11BETA or SVN
            IList<ChannelMap> allmaps2 = channel2.ReferringChannelMap();
#endif

            ChannelMap[] channelmap2 = new ChannelMap[allmaps2.Count];
            i = 0;
            foreach (ChannelMap map2 in allmaps2)
            {
                channelmap2[i++] = map2;
            }

            if (allmaps1.Count != allmaps2.Count)  //cardmap count not equal
                return false;

            for (i = 0; i < allmaps1.Count; i++)
            {
                if (channelmap1[i].IdCard != channelmap2[i].IdCard) //different cards
                    return false;
            }



            //    textoutput("Cardmappings equal for channel " + channel1.IdChannel + " and channel " + channel2.IdChannel);

            return true;
        }

        public bool comparetuningdetails(Channel channel1, Channel channel2)
        {

#if(MP100)
            IList alltuningdetails1 = channel1.ReferringTuningDetail();
#elif(MP101)
            IList<TuningDetail> alltuningdetails1 = channel1.ReferringTuningDetail();
#else //MP11BETA or SVN
            IList<TuningDetail> alltuningdetails1 = channel1.ReferringTuningDetail();
#endif

            TuningDetail[] tuningdetailarray1 = new TuningDetail[alltuningdetails1.Count];
            int i = 0;
            foreach (TuningDetail tuningdetail1 in alltuningdetails1)
            {
                tuningdetailarray1[i++] = tuningdetail1;
            }

#if(MP100)
            IList alltuningdetails2 = channel2.ReferringTuningDetail();
#elif(MP101)
            IList<TuningDetail> alltuningdetails2 = channel2.ReferringTuningDetail();
#else //MP11BETA or SVN
            IList<TuningDetail> alltuningdetails2 = channel2.ReferringTuningDetail();
#endif

            TuningDetail[] tuningdetailarray2 = new TuningDetail[alltuningdetails2.Count];
            i = 0;
            foreach (TuningDetail tuningdetail2 in alltuningdetails2)
            {
                tuningdetailarray2[i++] = tuningdetail2;
            }

            if (alltuningdetails1.Count != alltuningdetails2.Count)  //tuningdetail count not equal
                return false;

            for (i = 0; i < alltuningdetails1.Count; i++)
            {
                if (comparetuningdetail(tuningdetailarray1[i], tuningdetailarray2[i]) == false) //different tuning parameters
                    return false;
            }

            //    textoutput("Tunigdetails equal for channel " + channel1.IdChannel + " and channel " + channel2.IdChannel);

            return true;
        }

        public bool comparetuningdetail(TuningDetail tuningdetail1, TuningDetail tuningdetail2)
        {

            if ((tuningdetail1 == null) || (tuningdetail2 == null))
            {
                textoutput("Error: tuningdetail of channel is not defined");
                return false;
            }

#if (MP12)
#else
            if (tuningdetail1.AudioPid != tuningdetail2.AudioPid)
                return false;

            if (tuningdetail1.PcrPid != tuningdetail2.PcrPid)
                return false;

            if (tuningdetail1.VideoPid != tuningdetail2.VideoPid)
                return false;
#endif

            if (tuningdetail1.Band != tuningdetail2.Band)
                return false;

            if (tuningdetail1.Bandwidth != tuningdetail2.Bandwidth)
                return false;

            if (tuningdetail1.Bitrate != tuningdetail2.Bitrate)
                return false;

            if (tuningdetail1.ChannelType != tuningdetail2.ChannelType)
                return false;

            if (tuningdetail1.CountryId != tuningdetail2.CountryId)
                return false;

            if (tuningdetail1.Diseqc != tuningdetail2.Diseqc)
                return false;

            if (tuningdetail1.FreeToAir != tuningdetail2.FreeToAir)
                return false;

            if (tuningdetail1.Frequency != tuningdetail2.Frequency)
                return false;

            if (tuningdetail1.InnerFecRate != tuningdetail2.InnerFecRate)
                return false;

            if (tuningdetail1.IsRadio != tuningdetail2.IsRadio)
                return false;

            if (tuningdetail1.IsTv != tuningdetail2.IsTv)
                return false;

            if (tuningdetail1.MajorChannel != tuningdetail2.MajorChannel)
                return false;

            if (tuningdetail1.MinorChannel != tuningdetail2.MinorChannel)
                return false;

            if (tuningdetail1.Modulation != tuningdetail2.Modulation)
                return false;

            if (tuningdetail1.Name != tuningdetail2.Name)
                return false;

            if (tuningdetail1.NetworkId != tuningdetail2.NetworkId)
                return false;

            if (tuningdetail1.Pilot != tuningdetail2.Pilot)
                return false;

            if (tuningdetail1.PmtPid != tuningdetail2.PmtPid)
                return false;

            if (tuningdetail1.Polarisation != tuningdetail2.Polarisation)
                return false;

            if (tuningdetail1.Provider != tuningdetail2.Provider)
                return false;

            if (tuningdetail1.RollOff != tuningdetail2.RollOff)
                return false;

            if (tuningdetail1.SatIndex != tuningdetail2.SatIndex)
                return false;

            if (tuningdetail1.ServiceId != tuningdetail2.ServiceId)
                return false;

            if (tuningdetail1.SwitchingFrequency != tuningdetail2.SwitchingFrequency)
                return false;

            if (tuningdetail1.Symbolrate != tuningdetail2.Symbolrate)
                return false;

            if (tuningdetail1.TransportId != tuningdetail2.TransportId)
                return false;

            if (tuningdetail1.TuningSource != tuningdetail2.TuningSource)
                return false;

            if (tuningdetail1.Url != tuningdetail2.Url)
                return false;

            if (tuningdetail1.VideoSource != tuningdetail2.VideoSource)
                return false;

            return true;



        }



        public bool Importxmlfile(string filename)
        {


            XmlDocument doc = new XmlDocument();
            POSTIMPORT = "";
            if (DEBUG == true)
                textoutput("BackupSettings: Trying to import channels from " + filename);
            try
            {
                doc.Load(filename);
            }
            catch (Exception exc)
            {
                textoutput("<RED>Could not load xml file " + filename);
                if (DEBUG == true)
                    textoutput("<RED>Exception message is:" + exc.Message);

                return (false);
            }








            //list all cards and initiate translationtable

#if(MP100) 
            IList allTVcards = Card.ListAll();
#elif (MP101)
            IList<Card> allTVcards = Card.ListAll();
#else //MP11BETA or SVN
            IList<Card> allTVcards = Card.ListAll();
#endif

            bool[] TVcardassigned = new bool[allTVcards.Count + 1];
            Card[] TVcards = new Card[allTVcards.Count + 1];




            //count all cards in xml file
            XmlNodeList cardList = doc.SelectNodes("/tvserver/servers/server/cards/card");
            Int32 xmlcardnumbers = cardList.Count;
            Int32[] cardidpos = new Int32[xmlcardnumbers + 1];
            int i = 1;
            foreach (XmlNode nodecard in cardList)
            {
                int cardid = Convert.ToInt32(nodecard.Attributes["IdCard"].Value);
                cardidpos[i] = cardid;
                i++;
            }

            int allXMLcards = i - 1;
            bool[] xmlcardassigned = new bool[allXMLcards + 1];
            int[] cardmaptranslator = new int[allXMLcards + 1];
            //initialize tv card TVcardassigned
            i = 1;

            foreach (Card dbservercard in allTVcards)
            {
                TVcards[i] = dbservercard;
                TVcardassigned[i] = false;

                i++;
            }

            //initialize cardmaptranslator and xmlcardsassigned
            for (i = 0; i <= allXMLcards; i++)
            {
                cardmaptranslator[i] = 0;
                xmlcardassigned[i] = false;
            }


            // check if plugins are available

            bool ok = plugincheck();
            if (ok == false)
                return false;

            try
            {

                CountryCollection collection = new CountryCollection();
                TvBusinessLayer layer = new TvBusinessLayer();

                bool identicalCards = false;
                int motorCount = 0;
                int serverCount = 0;
                int cardCount = 0;
                int channelCount = 0;
                int programCount = 0;
                int scheduleCount = 0;
                int recordingCount = 0;
                int channelGroupCount = 0;
                int radiochannelGroupCount = 0;
                int tvmovieCount = 0;






                // version check


                XmlNodeList versionList = doc.SelectNodes("/tvserver/versions/pluginversion");
                bool versionfound = false;
                foreach (XmlNode nodeversion in versionList)
                {
                    string restoreversion;
                    versionfound = true;
                    try
                    {
                        restoreversion = nodeversion.Attributes["backupSettingVersion"].Value;
                    }
                    catch
                    {
                        restoreversion = "0.0.0.1";
                    }

                    string actualversion = detectplugin("BackupSettings");
                    if (restoreversion != actualversion)
                    {
                        textoutput("<YELLOW>Warning: actual BackupSetting plugin version is " + actualversion);
                        textoutput("<YELLOW>Backup data were created by version " + restoreversion);
                        try
                        {
                            myMessageBox("Backup data were created with an older version of BackupSettings\nIn case of problems install the older plugin version from\n" + filename.Substring(0, filename.Length - 22) + @"TV_Program\Plugins", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                        }
                        catch (Exception exc)
                        {
                            if (DEBUG == true)
                            {
                                textoutput("<RED>MessageBox Exception is " + exc.Message);
                            }
                            return false;
                        }

                    }
                }
                if (versionfound == false)
                {
                    textoutput("<YELLOW>No version number found - backup data were created by version 0.0.0.1 or 0.0.0.2");
                }

                // Delete Channels

                if (delete_channels == true)
                {
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.TV_DeleteAllChannels);
                    textoutput("Deleting all channels - Please be patient");

                    channelCount = 0;

                    try
                    {
                        foreach (Channel tmpChannel in Channel.ListAll())
                        {
                            try
                            {
                                tmpChannel.Delete();
                                channelCount++;
                            }
                            catch
                            {
                                textoutput("<RED>Channel " + tmpChannel.DisplayName + " number " + channelCount.ToString() + " could not be deleted");
                            }
                        }
                    }
                    catch
                    {
                        textoutput("<YELLOW>Channels could not be listed");
                    }

                    textoutput(channelCount + " Channels deleted");
                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.TV_DeleteAllChannels);
                }


                // Delete Schedules and Programs
                if (delete_schedules == true)
                {
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.TV_deleteAllSchedules);
                    textoutput("Deleting all programs from database");

                    programCount = 0;
#if(MP100)
                    IList programs = Program.ListAll();
#elif(MP101)
                    IList<Program> programs = Program.ListAll();
#else //MP11BETA or SVN
                    IList<Program> programs = Program.ListAll();
#endif

                    foreach (Program tmpProgram in programs)
                    {
                        tmpProgram.Delete();
                        programCount++;
                    }

                    textoutput(programCount + " Programs deleted");
                    // end program delete


                    //schedules
                    textoutput("Deleting all schedules from database");

                    scheduleCount = 0;
#if(MP100)
                    IList schedules = Schedule.ListAll();
#elif(MP101)
                    IList<Schedule> allschedules = Schedule.ListAll();
#else //MP11BETA or SVN
                    IList<Schedule> allschedules = Schedule.ListAll();
#endif

                    foreach (Schedule tmpSchedule in allschedules)
                    {
                        tmpSchedule.Delete();
                        scheduleCount++;
                    }

                    textoutput(scheduleCount + " Schedules deleted");
                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.TV_deleteAllSchedules);
                }

                // Delete Recordings
                if (delete_recordings == true)
                {
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.TV_DeleteAllRecordings);
                    textoutput("Deleting all recordings from database");

                    recordingCount = 0;
#if(MP100)
                    IList allrecordings = Recording.ListAll();
#elif(MP101)
                    IList<Recording> allrecordings = Recording.ListAll();
#else //MP11BETA or SVN
                    IList<Recording> allrecordings = Recording.ListAll();
#endif

                    foreach (Recording tmpRecording in allrecordings)
                    {
                        tmpRecording.Delete();
                        recordingCount++;
                    }

                    textoutput(recordingCount + " Recordings deleted from database");
                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.TV_DeleteAllRecordings);

                }

                // Delete Channel groups
                if (delete_tvgroups == true)
                {
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.TV_DeleteAllTvGroups);
                    textoutput("Deleting all TV groups");

                    channelGroupCount = 0;
#if(MP100)
                    IList tvgroups = ChannelGroup.ListAll();
#elif(MP101)
                    IList<ChannelGroup> alltvgroups = ChannelGroup.ListAll();
#else //MP11BETA or SVN
                    IList<ChannelGroup> alltvgroups = ChannelGroup.ListAll();
#endif

                    foreach (ChannelGroup tmpGroup in alltvgroups)
                    {
                        tmpGroup.Delete();
                        channelGroupCount++;
                    }

                    textoutput(channelGroupCount + " TV groups deleted");
                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.TV_DeleteAllTvGroups);

                }

                // Delete Radio Channel Groups
                if (delete_radiogroups == true)
                {
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.TV_DeleteAllRadioGroups);
                    textoutput("Deleting all radio groups");

                    radiochannelGroupCount = 0;
#if(MP100)
                    IList allradiogroups = RadioChannelGroup.ListAll();
#elif(MP101)
                    IList<RadioChannelGroup> allradiogroups = RadioChannelGroup.ListAll();
#else //MP11BETA or SVN
                    IList<RadioChannelGroup> allradiogroups = RadioChannelGroup.ListAll();
#endif

                    foreach (RadioChannelGroup tmpRadioGroup in allradiogroups)
                    {
                        tmpRadioGroup.Delete();
                        radiochannelGroupCount++;
                    }

                    textoutput(radiochannelGroupCount + " radio groups deleted");
                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.TV_DeleteAllRadioGroups);

                }

                


                //import servers and cards
                if (server == true)
                {
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.TV_Servers);
                    textoutput("Importing server settings");

                    Server dbserver = null;
                    Card dbserverCard = null;

                    XmlNodeList serverList = doc.SelectNodes("/tvserver/servers/server");
                    serverCount = 0;
                    foreach (XmlNode nodeserver in serverList)
                    {

                        serverCount++;
                        int serverid = Convert.ToInt32(nodeserver.Attributes["IdServer"].Value);
                        try
                        {
                            dbserver = Server.Retrieve(serverid);
                            // check for identical Hostname
                            if (dbserver.HostName != nodeserver.Attributes["HostName"].Value)
                            {
                                textoutput("<RED>Server hostname " + dbserver.HostName + " does not match import data " + nodeserver.Attributes["HostName"].Value);
                                switch (myMessageBox("Do you want to cancel the import? This is recommended! \n\n ", "Server hostname " + dbserver.HostName + " does not match import data " + nodeserver.Attributes["HostName"].Value, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                                {
                                    case DialogResult.Yes: // Abort
                                        {
                                            textoutput("User is aborting import");
                                            return (false);
                                        }


                                    case DialogResult.No: //  Continue                   
                                        {
                                            textoutput("User continues import although server name does not match host");
                                            break;
                                        }
                                }
                            }
                            //Server settings are in SetupTV\Sections\Servers.cs : private void buttonMaster_Click
                            dbserver.IsMaster = Convert.ToBoolean(nodeserver.Attributes["IsMaster"].Value);
#if(MP13)
                            try
                            {
                                dbserver.RtspPort = Convert.ToInt32(nodeserver.Attributes["RtspPort"].Value);
                            }
                            catch //do nothing
                            {
                            }

#endif
                            dbserver.Persist();

                            // global scanning parameters which are saved as server attributes

                            PostImport(doc, nodeserver, "lnbDefault");
                            PostImport(doc, nodeserver, "LnbLowFrequency");
                            PostImport(doc, nodeserver, "LnbHighFrequency");
                            PostImport(doc, nodeserver, "LnbSwitchFrequency");
                            


                        }
                        catch (Exception exc)
                        {
                            textoutput("<RED>Servers: Failed to add server " + serverid);
                            if (DEBUG == true)
                            {
                                textoutput("<RED>Exception message is " + exc.Message);
                            }
                            return false;
                        }


                    }
                    textoutput(serverCount + " Server settings imported");




                    // Cards
                    textoutput("Importing card settings");


                    //case a: match id and name and devicepath
                    i = 1;
                    foreach (XmlNode nodecard in cardList)
                    {
                        if (i > allTVcards.Count) //stop if there are more xml cards than tv cards, id cannot match anymore
                            break;

                        //int cardid = Convert.ToInt32(nodecard.Attributes["IdCard"].Value);
                        int cardid = i;

                        string name = nodecard.Attributes["Name"].Value;
                        string devicepath = nodecard.Attributes["DevicePath"].Value;



                        if ((cardidpos[i] == TVcards[i].IdCard) && (name == TVcards[i].Name) && (devicepath == TVcards[i].DevicePath) && (TVcardassigned[i] == false) && (xmlcardassigned[i] == false))
                        {
                            if (DEBUG == true)
                                textoutput("Card id " + TVcards[i].IdCard.ToString() + " maped to .xml id " + cardidpos[i].ToString() + " name " + name + " (matched id, name, devicepath)");

                            cardmaptranslator[i] = i;
                            TVcardassigned[i] = true;
                            xmlcardassigned[i] = true;
                        }
                        i++;
                    }

                    //case b: match id and name
                    i = 1;
                    foreach (XmlNode nodecard in cardList)
                    {
                        if (i > allTVcards.Count) //stop if there are more xml cards than tv cards, id cannot match anymore
                            break;

                        //int cardid = Convert.ToInt32(nodecard.Attributes["IdCard"].Value);
                        int cardid = i;

                        string name = nodecard.Attributes["Name"].Value;
                        string devicepath = nodecard.Attributes["DevicePath"].Value;

                        if ((cardidpos[i] == TVcards[i].IdCard) && (name == TVcards[i].Name) && (TVcardassigned[i] == false) && (xmlcardassigned[i] == false))
                        {
                            if (DEBUG == true)
                                textoutput("Card id " + TVcards[i].IdCard.ToString() + " maped to .xml id " + cardidpos[i].ToString() + " name " + name + " (matched id, name)");

                            cardmaptranslator[i] = i;
                            TVcardassigned[i] = true;
                            xmlcardassigned[i] = true;

                        }
                        i++;

                    }
                    //case c: match name and devicepath
                    foreach (XmlNode nodecard in cardList)
                    {

                        int cardid = Convert.ToInt32(nodecard.Attributes["IdCard"].Value);
                        int cardidposition = 0;
                        for (int j = 1; j <= xmlcardnumbers; j++)
                        {
                            if (cardid == cardidpos[j])
                            {
                                cardidposition = j;
                                break;
                            }
                        }
                        if (cardidposition == 0)
                        {
                            textoutput("<RED>Card ID position for card number " + cardid.ToString() + "could not be identified - aborting import");
                            return false;
                        }



                        string name = nodecard.Attributes["Name"].Value;
                        string devicepath = nodecard.Attributes["DevicePath"].Value;
                        for (i = 1; i <= allTVcards.Count; i++)
                        {
                            if ((name == TVcards[i].Name) && (devicepath == TVcards[i].DevicePath) && (TVcardassigned[i] == false) && (xmlcardassigned[cardidposition] == false))
                            {
                                if (DEBUG == true)
                                    textoutput("Card id " + TVcards[i].IdCard.ToString() + " maped to .xml id " + cardid.ToString() + " name " + name + " \n(matched name, devicepath)");

                                cardmaptranslator[cardidposition] = i;
                                TVcardassigned[i] = true;
                                xmlcardassigned[cardidposition] = true;
                            }
                        }


                    }
                    //case d: match name
                    foreach (XmlNode nodecard in cardList)
                    {

                        int cardid = Convert.ToInt32(nodecard.Attributes["IdCard"].Value);
                        int cardidposition = 0;
                        for (int j = 1; j <= xmlcardnumbers; j++)
                        {
                            if (cardid == cardidpos[j])
                            {
                                cardidposition = j;
                                break;
                            }
                        }
                        if (cardidposition == 0)
                        {
                            textoutput("<RED>Card ID position for card number " + cardid.ToString() + "could not be identified - aborting import");
                            return false;
                        }


                        string name = nodecard.Attributes["Name"].Value;
                        string devicepath = nodecard.Attributes["DevicePath"].Value;
                        for (i = 1; i <= allTVcards.Count; i++)
                        {
                            if ((name == TVcards[i].Name) && (TVcardassigned[i] == false) && (xmlcardassigned[cardidposition] == false))
                            {
                                if (DEBUG == true)
                                    textoutput("Card id " + TVcards[i].IdCard.ToString() + " maped to .xml id " + cardid.ToString() + " name " + name + " \n(matched name)");

                                cardmaptranslator[cardidposition] = i;
                                TVcardassigned[i] = true;
                                xmlcardassigned[cardidposition] = true;
                            }
                        }


                    }
                    for (i = 1; i <= allTVcards.Count; i++)
                    {
                        if (TVcardassigned[i] == false)
                        {
                            textoutput("<YELLOW>TV card ID " + TVcards[i].IdCard.ToString() + " with name " + TVcards[i].Name + " has not been assigned to a card from the backup data - no channel will be assigned to this card");
                        }
                    }
                    for (i = 1; i <= allXMLcards; i++)
                    {
                        if (xmlcardassigned[i] == false)
                        {
                            textoutput("<YELLOW>TV card ID " + cardidpos[i].ToString() + " from backup data could not assigned to a TV server card - card data will be skipped");

                        }
                    }


                    // now import card attributes
                    //cardList = doc.SelectNodes("/tvserver/servers/server/cards/card");
                    cardCount = 0;
                    foreach (XmlNode nodecard in cardList)
                    {

                        int cardid = Convert.ToInt32(nodecard.Attributes["IdCard"].Value);
                        int cardidposition = 0;
                        for (int j = 1; j <= xmlcardnumbers; j++)
                        {
                            if (cardid == cardidpos[j])
                            {
                                cardidposition = j;
                                break;
                            }
                        }
                        if (cardidposition == 0)
                        {
                            textoutput("<RED>Card ID position for card number " + cardid.ToString() + "could not be identified - aborting import");
                            return false;
                        }

                        if (xmlcardassigned[cardidposition] == true) //cardmapping does exist
                        {
                            try
                            {
                                dbserverCard = TVcards[cardmaptranslator[cardidposition]];
                                cardCount++;


                                // check for identical card names
                                if (dbserverCard.Name != nodecard.Attributes["Name"].Value)
                                {
                                    textoutput("<RED>Card name of cardid " + cardid + " does not match import data - aborting import");
                                    return false;
                                }
                                dbserverCard.RecordingFolder = nodecard.Attributes["RecordingFolder"].Value;
                                checkfilepath(dbserverCard.RecordingFolder, "recording folder card " + cardidposition.ToString());
                                dbserverCard.TimeShiftFolder = nodecard.Attributes["TimeShiftFolder"].Value;
                                checkfilepath(dbserverCard.TimeShiftFolder, "time shift folder card " + cardidposition.ToString());
                                // check for directory existence and write access


                                dbserverCard.RecordingFormat = Convert.ToInt32(nodecard.Attributes["RecordingFormat"].Value);
                                dbserverCard.Enabled = Convert.ToBoolean(nodecard.Attributes["Enabled"].Value);
                                dbserverCard.GrabEPG = Convert.ToBoolean(nodecard.Attributes["GrabEPG"].Value);
                                dbserverCard.Priority = Convert.ToInt32(nodecard.Attributes["Priority"].Value);

#if(MP100)
                                //do nothing            
#elif(MP101) 
                                //do nothing            
#else //MP11BETA or SVN
                                try
                                {
                                    dbserverCard.CAM = Convert.ToBoolean(nodecard.Attributes["CAM"].Value);
                                    dbserverCard.PreloadCard = Convert.ToBoolean(nodecard.Attributes["PreloadCard"].Value);
                                    dbserverCard.netProvider = Convert.ToInt32(nodecard.Attributes["netProvider"].Value); //bugfix: added 15.08.2010
                                }
                                catch
                                {
                                    //do nothing
                                }
#endif

#if(MP13)
                                try
                                {
                                    dbserverCard.CAM = Convert.ToBoolean(nodecard.Attributes["StopGraph"].Value);
                                }
                                catch //do nothing
                                {
                                }

#endif


                                dbserverCard.CamType = Convert.ToInt32(nodecard.Attributes["CamType"].Value);
                                dbserverCard.DecryptLimit = Convert.ToInt32(nodecard.Attributes["DecryptLimit"].Value);

                                dbserverCard.Persist();



                                //Import card settings for scanning
                                //NOTFOUND values are filtered out during Tvserverimport
                                //Analog
                                PostImport(doc, nodecard, "analog" + dbserverCard.IdCard.ToString() + "Country", "BACKUPSETTINGS_NOTFOUND");
                                PostImport(doc, nodecard, "analog" + dbserverCard.IdCard.ToString() + "Source", "BACKUPSETTINGS_NOTFOUND");
                                //ATSC
                                PostImport(doc, nodecard, "atsc" + dbserverCard.IdCard.ToString() + "supportsqam", "BACKUPSETTINGS_NOTFOUND");
                                //DVBC
                                PostImport(doc, nodecard, "dvbc" + dbserverCard.IdCard.ToString() + "Country", "BACKUPSETTINGS_NOTFOUND");
                                PostImport(doc, nodecard, "dvbc" + dbserverCard.IdCard.ToString() + "creategroups", "BACKUPSETTINGS_NOTFOUND");
                                //DVBS
                                PostImport(doc, nodecard, "dvbs" + dbserverCard.IdCard.ToString() + "SatteliteContext1", "BACKUPSETTINGS_NOTFOUND");
                                PostImport(doc, nodecard, "dvbs" + dbserverCard.IdCard.ToString() + "SatteliteContext2", "BACKUPSETTINGS_NOTFOUND");
                                PostImport(doc, nodecard, "dvbs" + dbserverCard.IdCard.ToString() + "SatteliteContext3", "BACKUPSETTINGS_NOTFOUND");
                                PostImport(doc, nodecard, "dvbs" + dbserverCard.IdCard.ToString() + "SatteliteContext4", "BACKUPSETTINGS_NOTFOUND");

                                PostImport(doc, nodecard, "dvbs" + dbserverCard.IdCard.ToString() + "DisEqc1", "BACKUPSETTINGS_NOTFOUND");
                                PostImport(doc, nodecard, "dvbs" + dbserverCard.IdCard.ToString() + "DisEqc2", "BACKUPSETTINGS_NOTFOUND");
                                PostImport(doc, nodecard, "dvbs" + dbserverCard.IdCard.ToString() + "DisEqc3", "BACKUPSETTINGS_NOTFOUND");
                                PostImport(doc, nodecard, "dvbs" + dbserverCard.IdCard.ToString() + "DisEqc4", "BACKUPSETTINGS_NOTFOUND");

                                PostImport(doc, nodecard, "dvbs" + dbserverCard.IdCard.ToString() + "band1", "BACKUPSETTINGS_NOTFOUND");
                                PostImport(doc, nodecard, "dvbs" + dbserverCard.IdCard.ToString() + "band2", "BACKUPSETTINGS_NOTFOUND");
                                PostImport(doc, nodecard, "dvbs" + dbserverCard.IdCard.ToString() + "band3", "BACKUPSETTINGS_NOTFOUND");
                                PostImport(doc, nodecard, "dvbs" + dbserverCard.IdCard.ToString() + "band4", "BACKUPSETTINGS_NOTFOUND");

                                PostImport(doc, nodecard, "dvbs" + dbserverCard.IdCard.ToString() + "LNB1", "BACKUPSETTINGS_NOTFOUND");
                                PostImport(doc, nodecard, "dvbs" + dbserverCard.IdCard.ToString() + "LNB2", "BACKUPSETTINGS_NOTFOUND");
                                PostImport(doc, nodecard, "dvbs" + dbserverCard.IdCard.ToString() + "LNB3", "BACKUPSETTINGS_NOTFOUND");
                                PostImport(doc, nodecard, "dvbs" + dbserverCard.IdCard.ToString() + "LNB4", "BACKUPSETTINGS_NOTFOUND");

                                PostImport(doc, nodecard, "dvbs" + dbserverCard.IdCard.ToString() + "creategroups", "BACKUPSETTINGS_NOTFOUND");
                                PostImport(doc, nodecard, "dvbs" + dbserverCard.IdCard.ToString() + "creategroupssat", "BACKUPSETTINGS_NOTFOUND");
                                PostImport(doc, nodecard, "dvbs" + dbserverCard.IdCard.ToString() + "createsignalgroup", "BACKUPSETTINGS_NOTFOUND");

                                PostImport(doc, nodecard, "dvbs" + dbserverCard.IdCard.ToString() + "enabledvbs2", "BACKUPSETTINGS_NOTFOUND");
                                PostImport(doc, nodecard, "dvbs" + dbserverCard.IdCard.ToString() + "limitsEnabled", "BACKUPSETTINGS_NOTFOUND");
                                PostImport(doc, nodecard, "dvbs" + dbserverCard.IdCard.ToString() + "motorEnabled", "BACKUPSETTINGS_NOTFOUND");
                                PostImport(doc, nodecard, "dvbs" + dbserverCard.IdCard.ToString() + "motorStepSize", "BACKUPSETTINGS_NOTFOUND");
                                PostImport(doc, nodecard, "dvbs" + dbserverCard.IdCard.ToString() + "selectedMotorSat", "BACKUPSETTINGS_NOTFOUND");
                                //DVBT
                                PostImport(doc, nodecard, "dvbt" + dbserverCard.IdCard.ToString() + "Country", "BACKUPSETTINGS_NOTFOUND");
                                PostImport(doc, nodecard, "dvbt" + dbserverCard.IdCard.ToString() + "creategroups", "BACKUPSETTINGS_NOTFOUND");
                                //DVBIP
                                PostImport(doc, nodecard, "dvbip" + dbserverCard.IdCard.ToString() + "Service", "BACKUPSETTINGS_NOTFOUND");
                                PostImport(doc, nodecard, "dvbip" + dbserverCard.IdCard.ToString() + "creategroups", "BACKUPSETTINGS_NOTFOUND");



                            }
                            catch (Exception exc)
                            {
                                textoutput("<RED>Cards: Failed to add card attributes for card ID " + cardid);
                                if (DEBUG == true)
                                {
                                    textoutput("<RED>Exception message is " + exc.Message);
                                }
                                return false;
                            }
                        }


                    }
                    textoutput(cardCount + " Card settings imported");
                    identicalCards = true; // card and server names do match import file


                    //Import DiSEqC motor settings
                    textoutput("Importing DiSEqC motor settings");
                    motorCount = 0;
                    XmlNodeList motorList = doc.SelectNodes("/tvserver/DiSEqCmotors/motor");
                    foreach (XmlNode nodemotor in motorList)
                    {
                        if ((CompareVersions(ActualTvServerVersion, "1.0.3.0") == (int)COMPAREVERSION.NEWER) && (CompareVersions(BackupTvServerVersion, "1.0.3.0") == (int)COMPAREVERSION.OLDER))
                        {
                            myMessageBox("Satellite file names have been changed in your actual version\nDisEqc settings will not get imported", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                            textoutput("<RED>Satellite file names have been changed in your actual version\nDisEqc settings will not get imported");
                            break;
                        }
                        else if ((CompareVersions(BackupTvServerVersion, "1.0.3.0") == (int)COMPAREVERSION.NEWER) && (CompareVersions(ActualTvServerVersion, "1.0.3.0") == (int)COMPAREVERSION.OLDER))
                        {
                            myMessageBox("Satellite file names have been changed in your actual version\nDisEqc settings will not get imported", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                            textoutput("<RED>Satellite file names have been changed in your actual version\nDisEqc settings will not get imported");
                            break;
                        }
                        else // DisEqc settingsb will get imported
                        {
                            try
                            {
                                motorCount++;

                                int cardidmotor = Convert.ToInt32(nodemotor.Attributes["IdCard"].Value);
                                int cardidposition = 0;
                                for (int j = 1; j <= xmlcardnumbers; j++)
                                {
                                    if (cardidmotor == cardidpos[j])
                                    {
                                        cardidposition = j;
                                        break;
                                    }
                                }
                                if (cardidposition == 0)
                                {
                                    textoutput("<RED>Card ID position for card number " + cardidmotor.ToString() + "could not be identified - aborting import");
                                    return false;
                                }
                                int idcardunmapped = cardidposition;


                                if (xmlcardassigned[idcardunmapped] == true)
                                {

                                    //check if satellite does exist
                                    string satelliteName = nodemotor.Attributes["SatelliteName"].Value;
                                    string transponderFileName = nodemotor.Attributes["TransponderFileName"].Value;

                                    Satellite satellite = null;
#if(MP100)
                                IList allsatellites = Satellite.ListAll();
#elif(MP101)
                                IList<Satellite> allsatellites = Satellite.ListAll();
#else //MP11BETA or SVN
                                    IList<Satellite> allsatellites = Satellite.ListAll();
#endif

                                    foreach (Satellite sat in allsatellites)
                                    {
                                        if ((sat.SatelliteName == satelliteName) && (sat.TransponderFileName == transponderFileName))
                                        {
                                            satellite = sat;
                                            if (DEBUG == true)
                                                textoutput("Existing satellite id " + satellite.IdSatellite + " found for name " + satellite.SatelliteName);

                                            break;
                                        }
                                    }

                                    if (satellite == null)
                                    {
                                        //create new satellite
                                        satellite = new Satellite(satelliteName, transponderFileName);

                                        if (satellite == null)
                                        {
                                            textoutput("<RED>Could not create new satellite for " + satelliteName + " with transponder filename \n" + transponderFileName);
                                            return false;
                                        }
                                        satellite.Persist();
                                        if (DEBUG == true)
                                            textoutput("New satellite id " + satellite.IdSatellite + " created for name " + satellite.SatelliteName);

                                    }

                                    if (File.Exists(transponderFileName) == false)
                                    {

                                        //try to translate path
                                        if (transponderFileName.Contains("Tuningparameters") == true)
                                        {
                                            int pos = transponderFileName.IndexOf("Tuningparameters");
                                            string partialfilename = transponderFileName.Substring(pos, transponderFileName.Length - pos);
#if(MP100)
                                        string newtransponderFileName = TV_PROGRAM_FOLDER + @"\" + partialfilename;
#elif(MP101)
                                        string newtransponderFileName = TV_PROGRAM_FOLDER + @"\" + partialfilename;
#else //MP11BETA or SVN
                                            string newtransponderFileName = TV_USER_FOLDER + @"\" + partialfilename;
#endif

                                            if (File.Exists(newtransponderFileName) == true)
                                            {
                                                if (DEBUG == true)
                                                    textoutput("Renaming transponder filename \n" + transponderFileName + "\nto " + newtransponderFileName);

                                                satellite.TransponderFileName = newtransponderFileName;
                                                satellite.Persist();
                                            }
                                            else
                                            {
                                                textoutput("<RED>Renamed transponder filename " + newtransponderFileName + " does not exist for satellite" + satelliteName);
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            textoutput("<RED>Transponder filename " + transponderFileName + " does not exist for satellite" + satelliteName);
                                            return false;
                                        }
                                    }
                                    //check if motorsetting does exist


                                    int cardid = TVcards[cardmaptranslator[idcardunmapped]].IdCard;
                                    int position = Convert.ToInt32(nodemotor.Attributes["Position"].Value);


                                    DiSEqCMotor motor = null;
#if(MP100)
                                IList allmotors = DiSEqCMotor.ListAll();
#elif(MP101)
                                IList<DiSEqCMotor> allmotors = DiSEqCMotor.ListAll();
#else //MP11BETA or SVN
                                    IList<DiSEqCMotor> allmotors = DiSEqCMotor.ListAll();
#endif

                                    foreach (DiSEqCMotor mot in allmotors)
                                    {
                                        if ((mot.IdCard == cardid) && (mot.IdSatellite == satellite.IdSatellite))
                                        {
                                            motor = mot;
                                            if (DEBUG == true)
                                                textoutput("Existing motor id " + motor.IdDiSEqCMotor + " found for satellite " + satellite.SatelliteName + " and card id " + cardid);

                                            break;
                                        }
                                    }

                                    if (motor == null)
                                    {
                                        //create new motor 
                                        motor = new DiSEqCMotor(cardid, satellite.IdSatellite, position);
                                        if (DEBUG == true)
                                            textoutput("New DiSEqC motor created for satellite " + satellite.SatelliteName + " and card id " + cardid);

                                    }

                                    //update values
                                    motor.IdCard = cardid;
                                    motor.IdSatellite = satellite.IdSatellite;
                                    motor.Position = position;
                                    motor.Persist();
                                    if (DEBUG == true)
                                        textoutput("DiSEqC motor id " + motor.IdDiSEqCMotor + " wirth cardid " + motor.IdCard + " and position " + motor.Position + " has been processed successfully");
                                }
                            }
                            catch (Exception exc)
                            {
                                motorCount--;
                                textoutput("<RED>Failed to add/update DiSEqC Motor ");
                                if (DEBUG == true)
                                {
                                    textoutput("<RED>Exception message is " + exc.Message);
                                }
                                return false;
                            }


                        }
                    }
                    textoutput(motorCount + " DiSEqC motor settings imported");

                    // Hybrid Settings
                    textoutput("Importing hybrid card settings");


                    // Delete old groups
#if(MP100)
                    IList hybridcardgroups = CardGroup.ListAll();
#elif(MP101)
                    IList<CardGroup> hybridcardgroups = CardGroup.ListAll();
#else //MP11BETA or SVN
                    IList<CardGroup> hybridcardgroups = CardGroup.ListAll();
#endif

                    int cardgroupCount = 0;
                    foreach (CardGroup hybridcardgroup in hybridcardgroups)
                    {
                        cardgroupCount++;
                        try
                        {
                            hybridcardgroup.Delete();
                        }
                        catch (Exception exc)
                        {
                            textoutput("<RED>Failed to delete hybrid card group " + hybridcardgroup.IdCardGroup + " named " + hybridcardgroup.Name);
                            if (DEBUG == true)
                            {
                                textoutput("<RED>Exception message is " + exc.Message);
                            }
                            return false;
                        }
                    }
                    textoutput(cardgroupCount + " Old hybrid groups deleted");





                    XmlNodeList cardgroupList = doc.SelectNodes("/tvserver/HybridCardGroups/HybridCardGroup");
                    cardgroupCount = 0;
                    foreach (XmlNode nodecardgroup in cardgroupList)
                    {
                        cardgroupCount++;
                        int newIdCardGroup = Convert.ToInt32(nodecardgroup.Attributes["IdCardGroup"].Value);
                        string newname = nodecardgroup.Attributes["CardGroupName"].Value;
                        try
                        {
                            CardGroup newcardgroup = new CardGroup(newIdCardGroup, newname);
                            newcardgroup.Persist();
                        }
                        catch (Exception exc)
                        {
                            textoutput("<RED>CardGroups: Failed to add card group attributes for card group " + newIdCardGroup + " named " + newname);
                            if (DEBUG == true)
                            {
                                textoutput("<RED>Exception message is " + exc.Message);
                            }
                            return false;
                        }
                    }
                    textoutput(cardgroupCount + " hybrid groups imported");




                    XmlNodeList cardgroupmapList = doc.SelectNodes("/tvserver/HybridCardGroupMaps/HybridCardGroupMap");
                    int cardgroupmapCount = 0;



                    foreach (XmlNode nodecardgroupmap in cardgroupmapList)
                    {
                        cardgroupmapCount++;
                        int newIdCardGroupMap = Convert.ToInt32(nodecardgroupmap.Attributes["IdMapping"].Value);
                        int newIdCardGroup = Convert.ToInt32(nodecardgroupmap.Attributes["IdCardGroup"].Value);



                        //int newIdCardunmapped = Convert.ToInt32(nodecardgroupmap.Attributes["IdCard"].Value);                       

                        int cardid = Convert.ToInt32(nodecardgroupmap.Attributes["IdCard"].Value);
                        int cardidposition = 0;
                        for (int j = 1; j <= xmlcardnumbers; j++)
                        {
                            if (cardid == cardidpos[j])
                            {
                                cardidposition = j;
                                break;
                            }
                        }
                        if (cardidposition == 0)
                        {
                            textoutput("<RED>Card ID position for card number " + cardid.ToString() + "could not be identified - aborting import");
                            return false;
                        }
                        int newIdCardunmapped = cardidposition;


                        string HybridGroupName = nodecardgroupmap.Attributes["HybridGroupName"].Value;
                        if (xmlcardassigned[newIdCardunmapped] == true) //cardmapping does exist
                        {
                            int newIdCard = TVcards[cardmaptranslator[newIdCardunmapped]].IdCard;

                            try
                            {

                                POSTIMPORT += "CARDGROUPMAP\t" + newIdCard + "\t" + newIdCardGroup + "\t" + HybridGroupName + "\n";

                                if (DEBUG == true)
                                {
                                    textoutput("newIdCard =" + newIdCard + " newIdCardGroup=" + newIdCardGroup);
                                }
                            }
                            catch (Exception exc)
                            {
                                textoutput("<RED>CardGroupMappings: Failed to add card group mapping " + newIdCardGroupMap + "Error: " + exc.Message);

                            }
                        }
                    }


                    textoutput(cardgroupCount + " Hybrid Groups with " + cardgroupmapCount + " Card Mappings imported");

                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.TV_Servers);
                }






                if (identicalCards == false) // do not import channelmappings if cards and servers are not identical from importfile
                {
                    channelcardmappings = false;
                    textoutput("<YELLOW>Servers and cards box unchecked - will not import channel mappings");
                }

                // Import Channels
                if (channels == true)
                {
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.TV_Channels);
                    textoutput("Importing channel settings - Please be patient");
                    XmlNodeList channelList = doc.SelectNodes("/tvserver/channels/channel");
                    channelCount = 0;

                    foreach (XmlNode nodeChannel in channelList)
                    {

                        string name = nodeChannel.Attributes["DisplayName"].Value;  //use only displayname
                        int IdChannel = Int32.Parse(GetNodeAttribute(nodeChannel, "IdChannel", "0"));
                        try
                        {
                            channelCount++;
                            Channel dbChannel = null;


                            XmlNodeList tuningList = nodeChannel.SelectNodes("TuningDetails/tune");
                            XmlNodeList mappingList = nodeChannel.SelectNodes("mappings/map");

                            bool grabEpg = (GetNodeAttribute(nodeChannel, "GrabEpg", "True") == "True");
                            bool isRadio = (GetNodeAttribute(nodeChannel, "IsRadio", "False") == "True");
                            bool isTv = (GetNodeAttribute(nodeChannel, "IsTv", "True") == "True");
                            DateTime lastGrabTime = DateTime.ParseExact("2000-01-01 00:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                            DateTime totalTimeWatched = DateTime.ParseExact("2000-01-01 00:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                            try
                            {
                                lastGrabTime = DateTime.ParseExact(GetNodeAttribute(nodeChannel, "LastGrabTime", "1900-01-01 00:00:00"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                                totalTimeWatched = DateTime.ParseExact(GetNodeAttribute(nodeChannel, "TotalTimeWatched", "1900-01-01 00:00:00"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                            }
                            catch
                            {
                                textoutput("<RED>Date and time could not be parsed for LastGrabTime and TotalTimeWatched - skipping values");
                            }
                            int sortOrder = Int32.Parse(GetNodeAttribute(nodeChannel, "SortOrder", "0"));
                            int timesWatched = Int32.Parse(GetNodeAttribute(nodeChannel, "TimesWatched", "0"));

                            bool visibileInGuide = (GetNodeAttribute(nodeChannel, "VisibleInGuide", "True") == "True");
                            bool FreeToAir = (GetNodeAttribute(nodeChannel, "FreeToAir", "True") == "True");
                            string displayName = GetNodeAttribute(nodeChannel, "DisplayName", name);
                            bool epgHasGaps = (GetNodeAttribute(nodeChannel, "EpgHasGaps", "False") == "True");
                            string externalId = GetNodeAttribute(nodeChannel, "ExternalId", "0");


                            

                            // rtv: since analog allows NOT to merge channels we need to take care of this. US users e.g. have multiple stations named "Sport" with different tuningdetails.
                            // using AddChannel would incorrectly "merge" these totally different channels.
                            // see this: http://forum.team-mediaportal.com/1-0-rc1-svn-builds-271/importing-exported-channel-list-groups-channels-39368/

                            /*
                            if (DEBUG == true)
                            {
                                Log.Info("TvChannels: Adding {0}. channel: {1} ({2})", channelCount, name, displayName);
                            }*/
                            dbChannel = layer.AddNewChannel(name);
                            if (dbChannel == null)
                            {
                                textoutput("<RED>Failed to add channel " + name + " - aborting import");
                                return false;
                            }
                            dbChannel.GrabEpg = grabEpg;
                            dbChannel.IsRadio = isRadio;
                            dbChannel.IsTv = isTv;
                            dbChannel.LastGrabTime = lastGrabTime;
                            dbChannel.SortOrder = sortOrder;
                            dbChannel.TimesWatched = timesWatched;
                            dbChannel.TotalTimeWatched = totalTimeWatched;
                            dbChannel.VisibleInGuide = visibileInGuide;

#if (MP12)
#else
                            try
                            {
                                dbChannel.FreeToAir = FreeToAir;
                            }
                            catch //do nothing
                            {

                            }
#endif
                            dbChannel.DisplayName = displayName;  //possible bug
                            dbChannel.EpgHasGaps = epgHasGaps;
                            dbChannel.ExternalId = externalId;
                            
#if(MP13)
                                        try
                                        {
                                            dbChannel.ChannelNumber = Int32.Parse(GetNodeAttribute(nodeChannel, "ChannelNumber", "10000"));

                                            //Ilist string Groupnames
                                            //Group CurrentGroup
                                            //program CurrentProgram
                                            //program NextProgram
                                        }
                                        catch //do nothing
                                        {
                                        }
#endif
                            
                            dbChannel.Persist();
                            if (channelcardmappings == true)
                            {
                                foreach (XmlNode nodeMap in mappingList)
                                {
                                    int idCard = Int32.Parse(nodeMap.Attributes["IdCard"].Value);
                                    
                                    //get xml position
                                    bool mapflag = false;
                                    for (int j = 1; j <= allXMLcards; j++)
                                    {
                                        if (idCard == cardidpos[j])  //position of card can be different to id
                                        {
                                            if (xmlcardassigned[cardmaptranslator[j]] == true)
                                            {
                                                try
                                                {
                                                    layer.MapChannelToCard(TVcards[cardmaptranslator[j]], dbChannel, false);
                                                    mapflag = true;
                                                }
                                                catch (Exception exe)
                                                {
                                                    textoutput("<RED>Failed to map channel " + dbChannel.DisplayName + " to card id " + cardmaptranslator[j].ToString());
                                                    textoutput("<RED>Exception message is " + exe.Message);
                                                }
                                            }

                                        }
                                    }
                                    if (mapflag == false)
                                    {
                                        textoutput("<RED>Failed to map channel " + dbChannel.DisplayName + " to any card");
                                    }

                                }
                            }

                            foreach (XmlNode nodeTune in tuningList)
                            {

#if (MP12)
#else
                                int pcrPid = -1;
                                int audioPid = -1;
                                int videoPid = -1;

                                try
                                {
                                    pcrPid = Int32.Parse(nodeTune.Attributes["PcrPid"].Value);
                                }
                                catch { }

                                try
                                {
                                    audioPid = Int32.Parse(nodeTune.Attributes["AudioPid"].Value);
                                }
                                catch { }

                                try
                                {
                                    videoPid = Int32.Parse(GetNodeAttribute(nodeTune, "VideoPid", "-1"));
                                }
                                catch { }
                                
#endif
                                int bandwidth = Int32.Parse(nodeTune.Attributes["Bandwidth"].Value);
                                int channelNumber = Int32.Parse(nodeTune.Attributes["ChannelNumber"].Value);
                                int channelType = Int32.Parse(nodeTune.Attributes["ChannelType"].Value);
                                int countryId = Int32.Parse(nodeTune.Attributes["CountryId"].Value);
                                int diseqc = Int32.Parse(nodeTune.Attributes["Diseqc"].Value);
                                bool fta = (nodeTune.Attributes["FreeToAir"].Value == "True");
                                int frequency = Int32.Parse(nodeTune.Attributes["Frequency"].Value);
                                int majorChannel = Int32.Parse(nodeTune.Attributes["MajorChannel"].Value);
                                int minorChannel = Int32.Parse(nodeTune.Attributes["MinorChannel"].Value);
                                int modulation = Int32.Parse(nodeTune.Attributes["Modulation"].Value);
                                name = nodeTune.Attributes["Name"].Value;
                                int networkId = Int32.Parse(nodeTune.Attributes["NetworkId"].Value);
                                int pmtPid = Int32.Parse(nodeTune.Attributes["PmtPid"].Value);
                                int polarisation = Int32.Parse(nodeTune.Attributes["Polarisation"].Value);
                                string provider = GetNodeAttribute(nodeTune, "Provider", "");
                                int serviceId = Int32.Parse(nodeTune.Attributes["ServiceId"].Value);
                                int switchingFrequency = Int32.Parse(nodeTune.Attributes["SwitchingFrequency"].Value);
                                int symbolrate = Int32.Parse(nodeTune.Attributes["Symbolrate"].Value);
                                int transportId = Int32.Parse(nodeTune.Attributes["TransportId"].Value);
                                int tuningSource = Int32.Parse(GetNodeAttribute(nodeTune, "TuningSource", "0"));
                                int videoSource = Int32.Parse(GetNodeAttribute(nodeTune, "VideoSource", "0"));
                                int audioSource = Int32.Parse(GetNodeAttribute(nodeTune, "AudioSource", "0"));
                                int SatIndex = Int32.Parse(GetNodeAttribute(nodeTune, "SatIndex", "-1"));
                                int InnerFecRate = Int32.Parse(GetNodeAttribute(nodeTune, "InnerFecRate", "-1"));
                                int band = Int32.Parse(GetNodeAttribute(nodeTune, "Band", "0"));
                                int pilot = Int32.Parse(GetNodeAttribute(nodeTune, "Pilot", "-1"));
                                int rollOff = Int32.Parse(GetNodeAttribute(nodeTune, "RollOff", "-1"));
                                string url = GetNodeAttribute(nodeTune, "Url", "");
                                int bitrate = Int32.Parse(GetNodeAttribute(nodeTune, "Bitrate", "0"));
                                bool isVCRSignal = (GetNodeAttribute(nodeChannel, "IsVCRSignal", "False") == "True");

                                

                                switch (channelType)
                                {
                                    case 0: //AnalogChannel
                                        AnalogChannel analogChannel = new AnalogChannel();
                                        if (analogChannel == null)
                                        {
                                            textoutput("<RED>Could not create analog channel for " + name);
                                            return false;
                                        }
                                        analogChannel.ChannelNumber = channelNumber;
                                        analogChannel.Country = collection.Countries[countryId];
                                        analogChannel.Frequency = frequency;
                                        analogChannel.IsRadio = isRadio;
                                        analogChannel.IsTv = isTv;
                                        analogChannel.Name = name;
                                        analogChannel.TunerSource = (TunerInputType)tuningSource;
                                        analogChannel.VideoSource = (AnalogChannel.VideoInputType)videoSource;
#if(MP100)
                                        //do nothing
#elif(MP101) 
                                        //do nothing
#else //MP11BETA or SVN
                                        analogChannel.AudioSource = (AnalogChannel.AudioInputType)audioSource;
                                        analogChannel.IsVCRSignal = isVCRSignal;
#endif


                                        layer.AddTuningDetails(dbChannel, analogChannel);
                                        if (DEBUG)
                                            Log.Info("TvChannels: Added tuning details for analog channel: {0} number: {1}", name, channelNumber);

                                        break;
                                    case 1: //ATSCChannel
                                        ATSCChannel atscChannel = new ATSCChannel();
                                        if (atscChannel == null)
                                        {
                                            textoutput("<RED>Could not create ATSC channel for " + name);
                                            return false;
                                        }
                                        atscChannel.MajorChannel = majorChannel;
                                        atscChannel.MinorChannel = minorChannel;
                                        atscChannel.PhysicalChannel = channelNumber;
                                        atscChannel.LogicalChannelNumber = channelNumber;
                                        atscChannel.FreeToAir = fta;
                                        atscChannel.Frequency = frequency;
                                        atscChannel.IsRadio = isRadio;
                                        atscChannel.IsTv = isTv;
                                        atscChannel.Name = name;
                                        atscChannel.NetworkId = networkId;
#if (MP12)
#else
                                        if (pcrPid > -1)
                                            atscChannel.PcrPid = pcrPid;

                                        if (audioPid > -1)
                                            atscChannel.AudioPid = audioPid;

                                        if (videoPid > -1)
                                            atscChannel.VideoPid = videoPid;
#endif
                                        atscChannel.PmtPid = pmtPid;
                                        atscChannel.Provider = provider;
                                        atscChannel.ServiceId = serviceId;
                                        //atscChannel.SymbolRate = symbolrate;
                                        atscChannel.TransportId = transportId;
                                        atscChannel.ModulationType = (ModulationType)modulation;
                                        layer.AddTuningDetails(dbChannel, atscChannel);
                                       



                                        if (DEBUG)
                                            Log.Info("TvChannels: Added tuning details for ATSC channel: {0} number: {1} provider: {2}", name, channelNumber, provider);

                                        break;
                                    case 2: //DVBCChannel
                                        DVBCChannel dvbcChannel = new DVBCChannel();
                                        if (dvbcChannel == null)
                                        {
                                            textoutput("<RED>Could not create DVB channel for " + name);
                                            return false;
                                        }
                                        dvbcChannel.ModulationType = (ModulationType)modulation;
                                        dvbcChannel.FreeToAir = fta;
                                        dvbcChannel.Frequency = frequency;
                                        dvbcChannel.IsRadio = isRadio;
                                        dvbcChannel.IsTv = isTv;
                                        dvbcChannel.Name = name;
                                        dvbcChannel.NetworkId = networkId;
#if (MP12)
#else
                                        if (pcrPid > -1)
                                            dvbcChannel.PcrPid = pcrPid;

                                        if (audioPid > -1)
                                            dvbcChannel.AudioPid = audioPid;

                                        if (videoPid > -1)
                                            dvbcChannel.VideoPid = videoPid;
                                        
#endif
                                        dvbcChannel.PmtPid = pmtPid;
                                        dvbcChannel.Provider = provider;
                                        dvbcChannel.ServiceId = serviceId;
                                        dvbcChannel.SymbolRate = symbolrate;
                                        dvbcChannel.TransportId = transportId;
                                        dvbcChannel.LogicalChannelNumber = channelNumber;
                                        layer.AddTuningDetails(dbChannel, dvbcChannel);
                                        if (DEBUG)
                                            Log.Info("TvChannels: Added tuning details for DVB-C channel: {0} provider: {1}", name, provider);

                                        break;
                                    case 3: //DVBSChannel
                                        DVBSChannel dvbsChannel = new DVBSChannel();
                                        if (dvbsChannel == null)
                                        {
                                            textoutput("<RED>Could not create DVBS channel for " + name);
                                            return false;
                                        }
                                        dvbsChannel.DisEqc = (DisEqcType)diseqc;
                                        dvbsChannel.Polarisation = (Polarisation)polarisation;
                                        dvbsChannel.SwitchingFrequency = switchingFrequency;
                                        dvbsChannel.FreeToAir = fta;
                                        dvbsChannel.Frequency = frequency;
                                        dvbsChannel.IsRadio = isRadio;
                                        dvbsChannel.IsTv = isTv;
                                        dvbsChannel.Name = name;
                                        dvbsChannel.NetworkId = networkId;
#if (MP12)
#else
                                        if (pcrPid > -1)
                                            dvbsChannel.PcrPid = pcrPid;

                                        if (audioPid > -1)
                                            dvbsChannel.AudioPid = audioPid;

                                        if (videoPid > -1)
                                            dvbsChannel.VideoPid = videoPid;
#endif
                                        dvbsChannel.PmtPid = pmtPid;
                                        dvbsChannel.Provider = provider;
                                        dvbsChannel.ServiceId = serviceId;
                                        dvbsChannel.SymbolRate = symbolrate;
                                        dvbsChannel.TransportId = transportId;
                                        dvbsChannel.SatelliteIndex = SatIndex;
                                        dvbsChannel.ModulationType = (ModulationType)modulation;
                                        dvbsChannel.InnerFecRate = (BinaryConvolutionCodeRate)InnerFecRate;
                                        dvbsChannel.BandType = (BandType)band;
                                        dvbsChannel.Pilot = (Pilot)pilot;
                                        dvbsChannel.Rolloff = (RollOff)rollOff;
                                        dvbsChannel.LogicalChannelNumber = channelNumber;

                                        layer.AddTuningDetails(dbChannel, dvbsChannel);
                                        if (DEBUG)
                                            Log.Info("TvChannels: Added tuning details for DVB-S channel: {0} provider: {1}", name, provider);

                                        break;
                                    case 4: //DVBTChannel
                                        DVBTChannel dvbtChannel = new DVBTChannel();
                                        if (dvbtChannel == null)
                                        {
                                            textoutput("<RED>Could not create DVBT channel for " + name);
                                            return false;
                                        }
                                        dvbtChannel.BandWidth = bandwidth;
                                        dvbtChannel.FreeToAir = fta;
                                        dvbtChannel.Frequency = frequency;
                                        dvbtChannel.IsRadio = isRadio;
                                        dvbtChannel.IsTv = isTv;
                                        dvbtChannel.Name = name;
                                        dvbtChannel.NetworkId = networkId;
#if (MP12)
#else
                                        if (pcrPid > -1)
                                            dvbtChannel.PcrPid = pcrPid;

                                        if (audioPid > -1)
                                            dvbtChannel.AudioPid = audioPid;

                                        if (videoPid > -1)
                                            dvbtChannel.VideoPid = videoPid;
                                        
#endif
                                        dvbtChannel.PmtPid = pmtPid;
                                        dvbtChannel.Provider = provider;
                                        dvbtChannel.ServiceId = serviceId;
                                        dvbtChannel.TransportId = transportId;
                                        dvbtChannel.LogicalChannelNumber = channelNumber;


                                        layer.AddTuningDetails(dbChannel, dvbtChannel);
                                        if (DEBUG)
                                            Log.Info("TvChannels: Added tuning details for DVB-T channel: {0} provider: {1}", name, provider);

                                        break;
                                    case 5: //Webstream
                                        layer.AddWebStreamTuningDetails(dbChannel, url, bitrate);
                                        if (DEBUG)
                                            Log.Info("TvChannels: Added wWeb stream: {0} ", url);

                                        break;
                                    //used IP channel from mvedrina patch only for MP1.1 and SVN
#if(MP100)
                                        //do nothing
#elif(MP101) 
                                        //do nothing
#else //MP11BETA or SVN


                                    case 7: //DVBIPChannel
                                        DVBIPChannel dvbipChannel = new DVBIPChannel();
#if (MP12)
#else
                                        if (pcrPid > -1)
                                            dvbipChannel.PcrPid = pcrPid;

                                        if (audioPid > -1)
                                            dvbipChannel.AudioPid = audioPid;

                                        if (videoPid > -1)
                                            dvbipChannel.VideoPid = videoPid;                                                                              
#endif
                                        dvbipChannel.FreeToAir = fta;
                                        dvbipChannel.Frequency = frequency;
                                        dvbipChannel.IsRadio = isRadio;
                                        dvbipChannel.IsTv = isTv;
                                        dvbipChannel.LogicalChannelNumber = channelNumber;
                                        dvbipChannel.Name = name;
                                        dvbipChannel.NetworkId = networkId;
                                        dvbipChannel.PmtPid = pmtPid;
                                        dvbipChannel.Provider = provider;
                                        dvbipChannel.ServiceId = serviceId;
                                        dvbipChannel.TransportId = transportId;
                                        dvbipChannel.Url = url;
                                        layer.AddTuningDetails(dbChannel, dvbipChannel);
                                        if (DEBUG)
                                            Log.Info("TvChannels: Added tuning details for DVB-IP channel: {0} provider: {1}", name, provider);
                                        break;
#endif
                                }
                            }

                        }
                        catch (Exception exc)
                        {
                            textoutput("<RED>TvChannels: Failed to add channel " + IdChannel + " name " + name);
                            if (DEBUG == true)
                            {
                                textoutput("<RED>Exception message is " + exc.Message);
                            }
                            return false;
                        }

                    }
                    textoutput(channelCount + " channel settings imported");
                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.TV_Channels);
                }



                if (schedules == true)
                {

                    // begin import schedule settings
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.TV_Schedules);
                    textoutput("Importing schedule settings");

                    XmlNodeList scheduleList = doc.SelectNodes("/tvserver/schedules/schedule");
                    scheduleCount = 0;

                    foreach (XmlNode nodeSchedule in scheduleList)
                    {
                        string programName = nodeSchedule.Attributes["ProgramName"].Value;
                        string displayname = nodeSchedule.Attributes["DisplayName"].Value;
                        int idSchedule = Int32.Parse(nodeSchedule.Attributes["IdSchedule"].Value);
                        try
                        {
                            scheduleCount++;

                            Channel tmpchannel = mygetChannelbyName(displayname);
                            if (tmpchannel == null)
                            {
                                textoutput("<RED>Channel " + displayname + " (Display: " + displayname + ") not found for schedule " + idSchedule);
                            }

                            int idChannel = tmpchannel.IdChannel;
                            DateTime startTime = DateTime.ParseExact(nodeSchedule.Attributes["StartTime"].Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                            DateTime endTime = DateTime.ParseExact(nodeSchedule.Attributes["EndTime"].Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                            int scheduletype = Int32.Parse(nodeSchedule.Attributes["ScheduleType"].Value);
                            Schedule schedule = layer.AddSchedule(idChannel, programName, startTime, endTime, scheduletype);
                            schedule.ScheduleType = scheduletype;  //do not remove! AddSchedule(idChannel, programName, startTime,endTime,scheduletype); does not work!
                            schedule.KeepDate = DateTime.ParseExact(nodeSchedule.Attributes["KeepDate"].Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                            schedule.PreRecordInterval = Int32.Parse(nodeSchedule.Attributes["PreRecordInterval"].Value);
                            schedule.PostRecordInterval = Int32.Parse(nodeSchedule.Attributes["PostRecordInterval"].Value);
                            schedule.Priority = Int32.Parse(nodeSchedule.Attributes["Priority"].Value);
                            schedule.Quality = Int32.Parse(nodeSchedule.Attributes["Quality"].Value);
                            schedule.Directory = nodeSchedule.Attributes["Directory"].Value;
                            schedule.KeepMethod = Int32.Parse(nodeSchedule.Attributes["KeepMethod"].Value);
                            schedule.MaxAirings = Int32.Parse(nodeSchedule.Attributes["MaxAirings"].Value);
                            schedule.RecommendedCard = 0;
                            try
                            {
                                int unmappedcard = Int32.Parse(nodeSchedule.Attributes["RecommendedCard"].Value);
                                if (xmlcardassigned[unmappedcard] == true) //cardmapping does exist
                                {
                                    schedule.RecommendedCard = cardmaptranslator[unmappedcard];
                                }
                            }
                            catch// do nothing in case of error: recommended card = 0
                            {

                            }
                            schedule.Series = (GetNodeAttribute(nodeSchedule, "Series", "False") == "True");
                            schedule.BitRateMode = (VIDEOENCODER_BITRATE_MODE)Int32.Parse(GetNodeAttribute(nodeSchedule, "BitRateMode", "-1"));
                            schedule.QualityType = (QualityType)Int32.Parse(GetNodeAttribute(nodeSchedule, "QualityType", "1"));

                            try
                            {
                                schedule.Canceled = DateTime.ParseExact(nodeSchedule.Attributes["Canceled"].Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                            }
                            catch
                            {
                            }
                            schedule.Persist();


                        }
                        catch (Exception ex)
                        {
                            scheduleCount--;
                            textoutput("<RED>Could not create schedule " + idSchedule + " for " + displayname + " (Display: " + displayname + ") with program " + programName);
                            if (DEBUG)
                            {
                                textoutput("<RED>Exception message is " + ex.Message);
                            }
                            //return false;
                        }

                    }
                    textoutput(scheduleCount + " schedule settings imported");
                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.TV_Schedules);
                }

                // Import programs
                if (epg == true)
                {
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.TV_EPG);
                    textoutput("Importing program settings - please be patient");

                    XmlNodeList programList = doc.SelectNodes("/tvserver/programs/program");
                    programCount = 0;
                    foreach (XmlNode nodeProgram in programList)
                    {


                        int idProgram = Int32.Parse(nodeProgram.Attributes["IdProgram"].Value);
                        //string channelname = nodeProgram.Attributes["ChannelName"].Value;
                        string displayname = nodeProgram.Attributes["DisplayName"].Value;
                        string title = nodeProgram.Attributes["Title"].Value;
                        try
                        {
                            programCount++;
                            string description = nodeProgram.Attributes["Description"].Value;
                            string classification = nodeProgram.Attributes["Classification"].Value;
                            string episodeNum = nodeProgram.Attributes["EpisodeNum"].Value;
#if(MP100)
                            //do nothing    
#elif(MP101) 
                            //do nothing
                                
#else //MP11BETA or SVN
                            string episodeName = "";
                            string episodePart = "";
                            try
                            {
                                episodeName = nodeProgram.Attributes["EpisodeName"].Value;
                                episodePart = nodeProgram.Attributes["EpisodePart"].Value;

                                
                            }
                            catch
                            {
                                textoutput("<YELLOW>Warning: Episode name or part not found - will set to \"notavailable\" ");
                                episodeName = "not available";
                                episodePart = "not available";
                            }
#endif

                            string genre = nodeProgram.Attributes["Genre"].Value;
                            string seriesNum = nodeProgram.Attributes["SeriesNum"].Value;
                            DateTime startTime = DateTime.ParseExact(nodeProgram.Attributes["StartTime"].Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                            if (startTime.CompareTo(DateTime.Now) > 0)
                            {
                                DateTime endTime = DateTime.ParseExact(nodeProgram.Attributes["EndTime"].Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                                DateTime originalAirDate = DateTime.ParseExact(nodeProgram.Attributes["OriginalAirDate"].Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                                bool notify = false;
                                try
                                {
                                    notify = Convert.ToBoolean(nodeProgram.Attributes["Notify"].Value);
                                }
                                catch //do nothing
                                {

                                }




                                int parentalRating = Int32.Parse(nodeProgram.Attributes["ParentalRating"].Value);
                                int starRating = Int32.Parse(nodeProgram.Attributes["StarRating"].Value);

                                Channel tmpchannel = mygetChannelbyName(displayname);
                                if (tmpchannel == null)
                                {
                                    textoutput("<RED>Channel " + displayname + " (Display: " + displayname + ") not found for program " + idProgram);
                                }
                                int idChannel = tmpchannel.IdChannel;

#if(MP100)
                                Program program = new Program(idChannel, startTime, endTime, title, description, genre, notify, originalAirDate, seriesNum, episodeNum, starRating, classification, parentalRating);

#elif (MP101)
                                Program program = new Program(idChannel, startTime, endTime, title, description, genre, notify, originalAirDate, seriesNum, episodeNum, starRating, classification, parentalRating);
#elif (MP11BETA)
                                Program program = new Program(idChannel, startTime, endTime, title, description, genre, notify, originalAirDate, seriesNum, episodeNum, episodeName, episodePart, starRating, classification, parentalRating);                                

#else //SVN 1.0.4.24281
                                Program program = new Program(idChannel, startTime, endTime, title, description, genre, Program.ProgramState.None, originalAirDate, seriesNum, episodeNum, episodeName, episodePart, starRating, classification, parentalRating);

#endif








                                program.Persist();

                                /*
                                if (DEBUG)
                                    Log.Info("Added program title: {0} on channel: {1} display {2}", title, channelname,displayname);
                                */
                            }
                            else
                            {
                                programCount++;
                            }

                        }
                        catch (Exception ex)
                        {
                            programCount--;
                            textoutput("<RED>Could not create program " + idProgram + " for title " + title + " with program " + displayname + " (Display: " + displayname + ")");
                            if (DEBUG)
                            {
                                textoutput("<RED>Exception message is " + ex.Message);
                            }
                            //return false;
                        }

                    }
                    textoutput(programCount + " program settings imported");
                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.TV_EPG);
                }


                // Import recordings
                if (recordings == true)
                {
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.TV_Recordings);
                    textoutput("Importing recording settings");

                    XmlNodeList recordingList = doc.SelectNodes("/tvserver/recordings/recording");
                    recordingCount = 0;
                    foreach (XmlNode nodeRecording in recordingList)
                    {
                        string fileName = nodeRecording.Attributes["FileName"].Value;
                        string title = nodeRecording.Attributes["Title"].Value;
                        int idRecording = Int32.Parse(nodeRecording.Attributes["IdRecording"].Value);
                        try
                        {
                            recordingCount++;


                            string description = nodeRecording.Attributes["Description"].Value;
                            string genre = nodeRecording.Attributes["Genre"].Value;

                            //string channelname = nodeRecording.Attributes["ChannelName"].Value;
                            string displayname = string.Empty;
                            try
                            {
                                displayname = nodeRecording.Attributes["DisplayName"].Value;
                            }
                            catch
                            {

                            }
                            Channel tmpchannel = mygetChannelbyName(displayname);
                            int idChannel = -1;
                            if (tmpchannel == null)
                            {
                                textoutput("<RED>Channel " + displayname + " (Display: " + displayname + ") not found for recording " + idRecording);
                            }
                            else
                            {
                                idChannel = tmpchannel.IdChannel;
                            }
                            string servername = nodeRecording.Attributes["ServerName"].Value;
                            Server tmpserver = mygetServerbyName(servername);
                            if (tmpserver == null)
                            {
                                textoutput("<RED>Server " + servername + " not found for recording " + idRecording);
                            }
                            int idServer = tmpserver.IdServer;
                            DateTime startTime = DateTime.ParseExact(nodeRecording.Attributes["StartTime"].Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                            DateTime endTime = DateTime.ParseExact(nodeRecording.Attributes["EndTime"].Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                            //, , , episodePart
                            string episodeName = "";
                            try
                            {
                                episodeName = nodeRecording.Attributes["EpisodeName"].Value;
                            }
                            catch
                            {
                            }
                            string seriesNum = "";
                            try
                            {
                                seriesNum = nodeRecording.Attributes["SeriesNum"].Value;
                            }
                            catch
                            {
                            }
                            string episodeNum = "";
                            try
                            {
                                episodeNum = nodeRecording.Attributes["EpisodeNum"].Value;
                            }
                            catch
                            {
                            }
                            string episodePart = "";
                            try
                            {
                                episodePart = nodeRecording.Attributes["EpisodePart"].Value;
                            }
                            catch
                            {
                            }



                            DateTime keepUntilDate = DateTime.ParseExact(nodeRecording.Attributes["KeepUntilDate"].Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                            int keepUntil = Int32.Parse(nodeRecording.Attributes["KeepUntil"].Value);
                            int timesWatched = Int32.Parse(nodeRecording.Attributes["TimesWatched"].Value);

                            
#if(SVN)
                                Recording recording = new Recording(idChannel, 0, false, startTime, endTime, title, description, genre, fileName, keepUntil, keepUntilDate, timesWatched, idServer, episodeName, seriesNum, episodeNum, episodePart);
                                //idChannel, idSchedule, isRecording, startTime
#elif(MP11BETA)
                            Recording recording = new Recording(idChannel, startTime, endTime, title, description, genre, fileName, keepUntil, keepUntilDate, timesWatched, idServer, episodeName, seriesNum, episodeNum, episodePart);
                            
#else
                            Recording recording = new Recording(idChannel, startTime, endTime, title, description, genre, fileName, keepUntil, keepUntilDate, timesWatched, idServer);
#endif

                                try
                                {
                                    recording.StopTime = Int32.Parse(nodeRecording.Attributes["StopTime"].Value);
                                }
                                catch
                                {
                                }
                                recording.Persist();

                                if (File.Exists(fileName) == false)
                                {
                                    textoutput("<YELLOW>Filename " + fileName + " does not exist for recording " + title + " - recording is not added to database");
                                    recording.Delete();
                                    recordingCount--;
                                }
                            

                        }
                        catch (Exception ex)
                        {
                            recordingCount--;
                            textoutput("<RED>Could not create recording " + idRecording + " for title " + title + " and file " + fileName +  " - use \"Recording -> database ImportedFromTypeLibAttribute\" for manual import");
                            if (DEBUG)
                            {
                                textoutput("<RED>Exception message is " + ex.Message);
                            }
                            //return false;
                        }

                    }
                    textoutput(recordingCount + " recording settings imported");
                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.TV_Recordings);
                }

                // Import channel groups
                if (tvgroups == true)
                {
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.TV_TvGroups);
                    textoutput("Importing channel group settings");

                    XmlNodeList channelGroupList = doc.SelectNodes("/tvserver/channelgroups/channelgroup");
                    channelGroupCount = 0;
                    foreach (XmlNode nodeChannelGroup in channelGroupList)
                    {
                        int idGroup = Int32.Parse(nodeChannelGroup.Attributes["IdGroup"].Value);
                        string groupName = nodeChannelGroup.Attributes["GroupName"].Value;
                        try
                        {
                            channelGroupCount++;
                            int mapcount = 0;
                            int groupSortOrder = Int32.Parse(nodeChannelGroup.Attributes["SortOrder"].Value);
                            ChannelGroup group = layer.GetGroupByName(groupName, groupSortOrder);
                            if (group == null)
                                group = new ChannelGroup(groupName, groupSortOrder);
                            group.Persist();
                            XmlNodeList mappingList = nodeChannelGroup.SelectNodes("mappings/map");
                            foreach (XmlNode nodeMap in mappingList)
                            {
                                mapcount++;
                                //string channelname = nodeMap.Attributes["ChannelName"].Value;
                                string displayname = nodeMap.Attributes["DisplayName"].Value;
                                Channel channel = mygetChannelbyName(displayname);
                                int idMap = Int32.Parse(GetNodeAttribute(nodeMap, "IdMap", "0"));
                                int sortOrder = Int32.Parse(GetNodeAttribute(nodeMap, "SortOrder", "9999"));

                                /*
                                Log.Debug("**************************");
                                Log.Debug("group.GroupName=" + group.GroupName);
                                Log.Debug("displayname=" + displayname);
                                Log.Debug("sortOrder=" + sortOrder.ToString());
                                Log.Debug("idMap=" + idMap.ToString());*/
                                if (channel != null)
                                {
                                    
                                    //GroupMap map = new GroupMap(group.IdGroup, channel.IdChannel, sortOrder);   //!!!!!!!sortorder is overwritten when setuptv is exited
                                    //POSTIMPORT += "GROUPMAPSORTORDER\t" + group.IdGroup.ToString() + "\t" + channel.IdChannel.ToString() + "\t" + sortOrder.ToString() + "\n";

                                    GroupMap map = new GroupMap(group.IdGroup, channel.IdChannel, sortOrder);
                                    map.SortOrder = sortOrder;
                                    map.Persist();
                                    
                                    /*map.SortOrder = sortOrder;
                                    Log.Debug("map.IsPersisted=" + map.IsPersisted.ToString());
                                    map.Persist();
                                    Log.Debug("map.SortOrder=" + map.SortOrder.ToString());
                                    Log.Debug("map.IdMap=" + map.IdMap.ToString());
                                    Log.Debug("map.IsPersisted=" + map.IsPersisted.ToString());*/


                                }
                                else
                                {
                                    textoutput("<YELLOW>Channel " + displayname + " (Display: " + displayname + ") could not be assigned to group " + groupName + " in map number " + idMap);
                                }

                                
                                Log.Debug("End");
                            }
                            textoutput(mapcount + " channels assigned to group " + groupName);
                        }
                        catch (Exception exc)
                        {
                            textoutput("<RED>TvChannels: Failed to add group " + idGroup + " group name " + groupName);
                            if (DEBUG == true)
                            {
                                textoutput("<RED>Exception message is " + exc.Message);
                            }
                            //return false;
                        }
                    }
                    textoutput(channelGroupCount + " channel group settings imported");
                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.TV_TvGroups);
                }

                

                // Import Radio channel groups
                if (radiogroups == true)
                {
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.TV_Radiogroups);
                    textoutput("Importing radio channel group settings");

                    XmlNodeList radiochannelGroupList = doc.SelectNodes("/tvserver/radiochannelgroups/radiochannelgroup");
                    radiochannelGroupCount = 0;
                    foreach (XmlNode radionodeChannelGroup in radiochannelGroupList)
                    {
                        int idGroup = Int32.Parse(radionodeChannelGroup.Attributes["IdGroup"].Value);
                        string groupName = radionodeChannelGroup.Attributes["GroupName"].Value;
                        try
                        {
                            radiochannelGroupCount++;
                            string radiogroupName = radionodeChannelGroup.Attributes["GroupName"].Value;
                            int radiogroupSortOrder = Int32.Parse(radionodeChannelGroup.Attributes["SortOrder"].Value);
                            RadioChannelGroup radiogroup = layer.GetRadioChannelGroupByName(radiogroupName);
                            if (radiogroup == null)
                                radiogroup = new RadioChannelGroup(radiogroupName, radiogroupSortOrder);
                            radiogroup.Persist();
                            XmlNodeList radiomappingList = radionodeChannelGroup.SelectNodes("mappings/radiomap");
                            int mapcount = 0;
                            foreach (XmlNode radionodeMap in radiomappingList)
                            {
                                mapcount++;
                                //string channelname = radionodeMap.Attributes["ChannelName"].Value;
                                string displayname = radionodeMap.Attributes["DisplayName"].Value;
                                Channel channel = mygetChannelbyName(displayname);

                                int sortOrder = Int32.Parse(GetNodeAttribute(radionodeMap, "SortOrder", "9999"));
                                int idMap = Int32.Parse(GetNodeAttribute(radionodeMap, "IdMap", "0"));
                                if (channel != null)
                                {
                                    RadioGroupMap radiomap = new RadioGroupMap(radiogroup.IdGroup, channel.IdChannel, sortOrder);
                                    radiomap.SortOrder = sortOrder;
                                    radiomap.Persist();
                                }
                                else
                                {
                                    textoutput("<YELLOW>Channel " + displayname + " (Display: " + displayname + ") could not be assigned to group " + radiogroupName + " in map number " + idMap);
                                }
                            }
                            textoutput(mapcount + " channels assigned to group " + groupName);
                        }
                        catch (Exception exc)
                        {
                            textoutput("<RED>RadioChannels: Failed to add radio group " + idGroup + " group name " + groupName);
                            if (DEBUG == true)
                            {
                                textoutput("<RED>Exception message is " + exc.Message);
                            }
                            //return false;
                        }
                    }
                    textoutput(radiochannelGroupCount + " radio channel group settings imported");
                    progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.TV_Radiogroups);
                }

                if (general_settings == true)
                {
                    // import all settings
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.TV_GeneralSettings);
                    textoutput("Importing TV server settings");

                    int ctr = 0;
                    XmlNodeList nodesettings = doc.SelectNodes("/tvserver/AllSettings/Setting");
                    foreach (XmlNode nodesetting in nodesettings)
                    {
                        XmlAttributeCollection allattributes = nodesetting.Attributes;
                        try
                        {
                            string myattribute = allattributes[0].Name.Replace("__SPACE__", " "); //reconverting attributes from export - do not change
                            myattribute = myattribute.Replace("__PLUS__", "+"); //reconverting attributes from export - do not change
                            
                            if (DEBUG)
                                textoutput("Attribute: " + myattribute + "    Value=" + allattributes[0].Value);

                            PostImport(doc, nodesetting, myattribute);
                            ctr++;
                            
                        }
                        catch
                        {
                            textoutput("<RED>Error reading attribute");
                        }
                    }

                    textoutput(ctr.ToString() + " settings imported");

                }



                if (clickfinder == true)
                {
                    progressbar((int)PB_action.START, ref PB_Import, (int)PB_part.TV_PluginSettings);

                    string plugversion = detectplugin("TV Movie EPG import");
                    if (plugversion != "NOT_FOUND")
                    {

                        textoutput("Importing TV movie");


                        //TVMovie mappings
                        textoutput("Delete old TV movie mappings");

                        tvmovieCount = 0;
#if(MP100)
                        IList mappingDb = TvMovieMapping.ListAll();
#elif(MP101)
                        IList<TvMovieMapping> mappingDb = TvMovieMapping.ListAll();
#else //MP11BETA or SVN
                        IList<TvMovieMapping> mappingDb = TvMovieMapping.ListAll();
#endif


                        if (mappingDb != null)
                        {
                            if (mappingDb.Count > 0)
                            {
                                foreach (TvMovieMapping mapping in mappingDb)
                                {
                                    tvmovieCount++;
                                    mapping.Remove();



                                }
                            }
                        }
                        textoutput(tvmovieCount + " TV movie mappings deleted");


                        layer = new TvBusinessLayer();


                        textoutput("Importing TV movie mappings");

                        XmlNodeList tvmoviemapping = doc.SelectNodes("/tvserver/TVMovieMappings/TVMovieMapping");
                        tvmovieCount = 0;


                        foreach (XmlNode nodetvmoviemapping in tvmoviemapping)
                        {

                            string dpname = nodetvmoviemapping.Attributes["MPDisplayName"].Value;
                            try
                            {

                                if (DEBUG)
                                {
                                    textoutput("TvMovie mappings: Display ="+ dpname);
                                }
                                //Channel channel = layer.GetChannelByName(chname);

                                Channel channel = mygetChannelbyName(dpname);


                                if (channel == null)
                                {
                                    textoutput("<YELLOW>TV movie EPG mapping failed  (Display: " + dpname + ") was not found");

                                }
                                else
                                {
                                    int idChannel = channel.IdChannel;
                                    string stationName = nodetvmoviemapping.Attributes["StationName"].Value;
                                    string timeSharingStart = nodetvmoviemapping.Attributes["TimeSharingStart"].Value;
                                    string timeSharingEnd = nodetvmoviemapping.Attributes["TimeSharingEnd"].Value;
                                    //TvMovieMapping tvmapping = new TvMovieMapping(idChannel, stationName, timeSharingStart, timeSharingEnd);
                                    //tvmapping.Persist();
                                    tvmovieCount++;
                                    POSTIMPORT += "TVMOVIE\t" + idChannel + "\t" + stationName + "\t" + timeSharingStart + "\t" + timeSharingEnd + "\n";
                                }
                            }
                            catch (Exception exc)
                            {
                                textoutput("<RED>Tv movie mappings: Failed to add mapping for channel  (Display:" + dpname + ") ");
                                if (DEBUG == true)
                                {
                                    textoutput("<RED>Exception message is " + exc.Message);
                                }
                                return false;
                            }
                        }
                        textoutput(tvmovieCount + " TV movie mappings imported");

                    }


                }
                textoutput("TV server import finished");
                progressbar((int)PB_action.STOP, ref PB_Import, (int)PB_part.TV_PluginSettings);

            }
            catch (Exception ex)
            {
                textoutput("<RED>Error while importing:" + ex.ToString() + " " + ex.StackTrace);
                myMessageBox("Error while importing:\n\n" + ex.ToString() + " " + ex.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);


                return false;
            }



            return true;

        }

        public bool completeimportxml()
        {
            if (POSTIMPORT == "")
            {

                return true;
            }

            if (TVRestart == false)
            {
                textoutput("<RED>\nRestarting TV Server Configuration is not allowed by user");
                textoutput("<RED>Some settings (Servers and Cards, Settings and Tv Clickfinder Mappings) will not get imported");
                POSTIMPORT = "";
                return true;
            }

            // end tvserver configuration and complete import
            textoutput("TV server configuration exit for postprocessing");
            if (File.Exists(TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + POSTPROCESSING_FILE) == true)
            {
                File.Delete(TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + POSTPROCESSING_FILE);
                textoutput("BackupSettings Server: File " + TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + POSTPROCESSING_FILE + " deleted");
            }
            string fName = TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + POSTPROCESSING_FILE;
            try
            {
                StreamWriter fs = File.CreateText(fName);
                fs.Write(POSTIMPORT);
                fs.Close();
                if (DEBUG == true)
                    textoutput("BackupSetting: ImportPostprocessing.txt file written");
            }
            catch (Exception ee)
            {
                textoutput("<RED>Error in writing file  " + fName);
                if (DEBUG == true)
                    textoutput("<RED>Exception message is " + ee.Message);
            }

            /*
            fName = TV_PROGRAM_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + INSTALLPATHS_FILE;  //this file contains only the directory of SetupTv.exe
            try
            {
                StreamWriter fs = File.CreateText(fName);
                fs.Write(TV_PROGRAM_FOLDER + "\n" + TV_USER_FOLDER + "\n" + MP_PROGRAM_FOLDER + "\n" + MP_USER_FOLDER + "\nEND\n");
                fs.Close();
                if (DEBUG == true)
                    textoutput("BackupSetting: TV_PROGRAM_FOLDER.txt file written");
            }
            catch (Exception ee)
            {
                textoutput("<RED>Error in writing file  " + fName);
                if (DEBUG == true)
                    textoutput("<RED>Exception message is " + ee.Message);
            }*/

            Process app = new Process();
            ProcessStartInfo appstartinfo = new ProcessStartInfo();
            appstartinfo.FileName = TV_PROGRAM_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + RESTART_SETUP_TV_EXE;
            appstartinfo.Arguments = "\"" + TV_PROGRAM_FOLDER + "\"  \"" + TV_USER_FOLDER + "\" POSTPROCESSING";
            appstartinfo.WorkingDirectory = TV_PROGRAM_FOLDER + @"\" + BACKUPSETTING_TV_DIR;
            app.StartInfo = appstartinfo;
            try
            {
                app.Start();
                if (DEBUG == true)
                    textoutput("Application " + appstartinfo.FileName + " started");

            }
            catch (Exception exc)
            {
                textoutput("<RED>Error in starting the process " + appstartinfo.FileName);
                if (DEBUG == true)
                    textoutput("<RED>Exception message is " + exc.Message);
            }





            try
            {

                textoutput("BackupSettings: Closing status file STATUS");
                if (exportprogressBar != null)
                {
                    textoutput("ProgressBar.Maximum  " + exportprogressBar.Maximum.ToString());
                    textoutput("ProgressBar.Value  " + exportprogressBar.Maximum.ToString());//complete 100%
                }
                textoutput("BackupSettings: Exiting SetupTv.exe");
                textoutput("BackupSettings: Kill SetupTv process");

                if (Status != null)
                {
                    textoutput("BackupSettings: Closing status file");
                    System.Threading.Thread.Sleep(500);
                    Status.Close();  //  close status file
                    Status = null;
                    System.Threading.Thread.Sleep(2000);
                }

            }
            catch (Exception exc)
            {
                Log.Error("BackupSettingsExportImport: Exception:" + exc.Message);
            }    

            Process[] processes = Process.GetProcessesByName("SetupTv");
            foreach (Process process in processes)
            {
                    //process.Close();  not working
                    process.Kill();  
                    //Application.Exit();
            }
            

            Log.Error("BackupSettingsExportImport: Fatal Error: Running after application exit");
            textoutput("BackupSettingsExportImport: Fatal Error: Running after application exit");
            return false;
        }

        public bool start_tvservice()
        {

            textoutput("Starting TV Service");
            Process proc2 = new Process();
            ProcessStartInfo startinfo2 = new ProcessStartInfo();
            startinfo2.FileName = "cmd.exe";
            startinfo2.WorkingDirectory = "";
            startinfo2.Arguments = @"/c net start tvservice";
            startinfo2.WindowStyle = ProcessWindowStyle.Hidden;
            startinfo2.UseShellExecute = false;
            startinfo2.CreateNoWindow = true;
            startinfo2.RedirectStandardError = true;
            startinfo2.RedirectStandardInput = true;
            startinfo2.RedirectStandardOutput = true;

            proc2.StartInfo = startinfo2;
            proc2.EnableRaisingEvents = false;

            try
            {
                proc2.Start();
            }
            catch (Exception exc)
            {
                textoutput("<RED>Could not execute command \n" + startinfo2.FileName + " " + startinfo2.Arguments);
                textoutput("<RED>Exception message was:\n" + exc.Message + "\n");
                return false;
            }
            proc2.WaitForExit(1000 * 60 * 3); //wait 3 minutes maximum
            if (proc2.HasExited == true)
            {
                if (proc2.ExitCode != 0)
                {
                    textoutput("<RED>Tv Service error: Starting Tv service caused an error code " + proc2.ExitCode);

                    textoutput("<RED>Reboot and check the TV service status from the TV server configuration tool\n");
                    return false;
                }
            }
            else
            {
                textoutput("<RED>Tv Service timeout error: Could not stop Tv service within time limit\n");
                textoutput("<RED>Reboot and check the TV service status from the TV server configuration tool\n");
                return false;
            }

            textoutput("TV Service Started");
            return true;
        }

        public bool stop_tvservice()
        {
            textoutput("Stopping TV Service");
            Process proc = new Process();
            ProcessStartInfo startinfo = new ProcessStartInfo();
            startinfo.FileName = "cmd.exe";
            startinfo.WorkingDirectory = "";
            startinfo.Arguments = @"/c net stop tvservice";
            startinfo.UseShellExecute = false;
            startinfo.CreateNoWindow = true;
            startinfo.RedirectStandardError = true;
            startinfo.RedirectStandardInput = true;
            startinfo.RedirectStandardOutput = true;
            proc.StartInfo = startinfo;
            proc.EnableRaisingEvents = false;

            try
            {
                proc.Start();
            }
            catch (Exception exc)
            {
                textoutput("<RED>Could not execute command \n" + startinfo.FileName + " " + startinfo.Arguments);
                textoutput("<RED>Exception message was:\n" + exc.Message + "\n");
                return false;
            }
            proc.WaitForExit(1000 * 60); //wait 1 minutes maximum
            if (proc.HasExited == true)
            {
                if (proc.ExitCode != 0)
                {
                    textoutput("<RED>Tv Service error: Stopping Tv service caused an error code " + proc.ExitCode);

                    textoutput("<RED>Reboot and repeat installation\n");
                    return false;
                }
            }
            else
            {
                textoutput("<RED>Tv Service timeout error: Could not stop Tv service within time limit");
                textoutput("<RED>Reboot and repeat installation\n");
                return false;
            }
            textoutput("TV Service Stopped");

            return true;
        }

        private void PostImport(XmlDocument doc, XmlNode node, string attribute, string defaultvalue, string xmlattribute)
        {
            string setting = "";
            try
            {
                setting = node.Attributes[xmlattribute].Value;

            }
            catch
            {
                setting = defaultvalue;
                if (DEBUG)
                    textoutput("Defaultvalue added for " + attribute);
            }



            if (DEBUG)
            {
                textoutput("BackupSettings Import: " + attribute);
                //textoutput("BackupSettings Import: " + attribute + " = " + setting);
            }
            POSTIMPORT += "SETTING\t" + attribute + "\t" + setting + "\n";

        }
        private void PostImport(XmlDocument doc, XmlNode node, string attribute, string defaultvalue)
        {

            string setting = "";
            try
            {
                setting = node.Attributes[attribute].Value;
            }
            catch
            {
                setting = defaultvalue;
                if (DEBUG)
                    textoutput("Defaultvalue added for " + attribute);
            }



            if (DEBUG)
            {
                textoutput("BackupSettings Import: " + attribute);
                //textoutput("BackupSettings Import: " + attribute + " = " + setting);
            }
            POSTIMPORT += "SETTING\t" + attribute + "\t" + setting + "\n";

        }
        private void PostImport(XmlDocument doc, XmlNode node, string attribute)
        {
            string setting = "";
            try
            {
                setting = node.Attributes[attribute].Value;
                POSTIMPORT += "SETTING\t" + attribute + "\t" + setting + "\n";
                if (DEBUG)
                {
                    textoutput("BackupSettings Import: " + attribute);
                    //textoutput("BackupSettings Import: " + attribute + " = " + setting);
                }
            }
            catch
            {
                if (DEBUG)
                    textoutput("No xml entry found for " + attribute);
            }




        }

        public Channel mygetChannelbyName(string display)
        {
            // This function was written because TVbusinesslayer GetChannelByName was not case sensitive
            // and returned incorrect channel ARTE for a channel name arte which are two different stations

            foreach (Channel onechannel in Channel.ListAll())
            {
                if (onechannel.DisplayName == display)
                    return onechannel;
            }
            if (DEBUG)
                textoutput("<RED>mygetChannelbyName did not find channel name " + display);

            return null;

        }

        public Server mygetServerbyName(string name)
        {
#if(MP100)
            IList allservers = Server.ListAll();
#elif(MP101)
            IList<Server> allservers = Server.ListAll();
#else //MP11BETA or SVN
            IList<Server> allservers = Server.ListAll();
#endif

            foreach (Server oneserver in allservers)
            {
                if (oneserver.HostName == name)
                    return oneserver;
            }
            if (DEBUG)
                textoutput("<RED>mygetServerbyName did not find server name " + name);

            return null;

        }
#endif

        public string CreateAutomatedFolderName(string folderpath,WhoAmI ident)
        {
            //create automated foldername

            string versionNumber = "BACKUPSETTINGS_NOTFOUND";
            getallversionnumbers("", false);

            if (ident==WhoAmI.Tv_Server)
            {
                versionNumber = ActualTvServerVersion;
            }
            else if (ident == WhoAmI.MP1_Client)
            {
                versionNumber = ActualMediaPortalVersion;
            }
            else if (ident == WhoAmI.MP2_Server)
            {
                versionNumber = ActualMP2ServerVersion;
            }
            else if (ident == WhoAmI.MP2_Client)
            {
                versionNumber = ActualMP2ClientVersion;
            }

            string timestamp = "Version_" + versionNumber + "_Date_" + DateTime.Now.ToString("yyyy_MM_dd", CultureInfo.InvariantCulture) + "_Time_" + DateTime.Now.ToString("HH_mm_ss", CultureInfo.InvariantCulture);
            string[] tokenarray = folderpath.Split('\\');
            if ((tokenarray.Length > 0) && (folderpath.Length > 2))
            {
                string lastFolder = tokenarray[tokenarray.Length - 1];
                if ((lastFolder.StartsWith("Version_") == true) && (lastFolder.Contains("_Date_") == true) && (lastFolder.Contains("_Time_") == true))
                {
                    folderpath = "";
                    for (int i = 0; i <= tokenarray.Length - 2; i++)
                    {
                        folderpath += tokenarray[i] + "\\";
                    }
                    folderpath += timestamp;
                }
                else
                {
                    folderpath = folderpath + "\\" + timestamp;
                }
            }
            else
            {
                folderpath = @"C:\MediaPortal Backups\" + timestamp;
            }
            return folderpath;
        }


        public void progressbar(int action, ref int[] vector, int number)
        {
            try
            {
                if (exportprogressBar == null)
                    return; //prgress bar not defined

                if (action == (int)PB_action.INIT)
                {
                    //textoutput("Initializing progress bar");
                    exportprogressBar.Minimum = 0;
                    exportprogressBar.Maximum = 0;
                    if ((MPAllProgram == true) && (MP == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.MP_AllMediaPortalProgramFolders];
#if PB
                        textoutput("attribute=" + PB_part.MP_AllMediaPortalProgramFolders.ToString() + " number= " + ((int)PB_part.MP_AllMediaPortalProgramFolders).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((MPAllFolder == true) && (MP == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.MP_AllMediaPortalUserFolders];
#if PB
                        textoutput("attribute=" + PB_part.MP_AllMediaPortalUserFolders.ToString() + " number= " + ((int)PB_part.MP_AllMediaPortalUserFolders).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((MPDatabase == true) && (MP == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.MP_Database];
#if PB
                        textoutput("attribute=" + PB_part.MP_Database.ToString() + " number= " + ((int)PB_part.MP_Database).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((MPDeleteCache == true) && (MP == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.MP_DeleteCache];
#if PB
                        textoutput("attribute=" + PB_part.MP_DeleteCache.ToString() + " number= " + ((int)PB_part.MP_DeleteCache).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }

                    if ((MPInputDevice == true) && (MP == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.MP_InputDeviceMappings];
#if PB
                        textoutput("attribute=" + PB_part.MP_InputDeviceMappings.ToString() + " number= " + ((int)PB_part.MP_InputDeviceMappings).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((MPSkins == true) && (MP == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.MP_Language];//separate field for language although triggered by skins
#if PB
                        textoutput("attribute=" + PB_part.MP_Language.ToString() + " number= " + ((int)PB_part.MP_Language).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((MPPlugins == true) && (MP == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.MP_Plugins];
#if PB
                        textoutput("attribute=" + PB_part.MP_Plugins.ToString() + " number= " + ((int)PB_part.MP_Plugins).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((MPProgramXml == true) && (MP == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.MP_ProgramXML];
#if PB
                        textoutput("attribute=" + PB_part.MP_ProgramXML.ToString() + " number= " + ((int)PB_part.MP_ProgramXML).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((MPSkins == true) && (MP == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.MP_Skins];
#if PB
                        textoutput("attribute=" + PB_part.MP_Skins.ToString() + " number= " + ((int)PB_part.MP_Skins).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((MPThumbs == true) && (MP == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.MP_Thumbs];
#if PB
                        textoutput("attribute=" + PB_part.MP_Thumbs.ToString() + " number= " + ((int)PB_part.MP_Thumbs).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((MPMusicPlayer == true) && (MP == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.MP_MusicPlayer];
#if PB
                        textoutput("attribute=" + PB_part.MP_MusicPlayer.ToString() + " number= " + ((int)PB_part.MP_MusicPlayer).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((MPxmltv == true) && (MP == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.MP_Xmltv];
#if PB
                        textoutput("attribute=" + PB_part.MP_Xmltv.ToString() + " number= " + ((int)PB_part.MP_Xmltv).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((MPUserXML == true) && (MP == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.MP_UserXML];
#if PB
                        textoutput("attribute=" + PB_part.MP_UserXML.ToString() + " number= " + ((int)PB_part.MP_UserXML).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }

                    if ((MP2Config == true) && (MP2C == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.MP2_Config];
#if PB
                        textoutput("attribute=" + PB_part.MP2_Config.ToString() + " number= " + ((int)PB_part.MP2_Config).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((MP2Defaults == true) && (MP2C == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.MP2_Defaults];
#if PB
                        textoutput("attribute=" + PB_part.MP2_Defaults.ToString() + " number= " + ((int)PB_part.MP2_Defaults).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((MP2Plugins == true) && (MP2C == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.MP2_Plugins];
#if PB
                        textoutput("attribute=" + PB_part.MP2_Plugins.ToString() + " number= " + ((int)PB_part.MP2_Plugins).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((MP2AllClientFolders == true) && (MP2C == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.MP2_AllClientFolders];
#if PB
                        textoutput("attribute=" + PB_part.MP2_AllClientFolders.ToString() + " number= " + ((int)PB_part.MP2_AllClientFolders).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((MP2AllClientProgramFolders == true) && (MP2C == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.MP2_AllClientProgramFolders];
#if PB
                        textoutput("attribute=" + PB_part.MP2_AllClientProgramFolders.ToString() + " number= " + ((int)PB_part.MP2_AllClientProgramFolders).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((MP2AllClientFiles == true) && (MP2C == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.MP2_AllClientFiles];
#if PB
                        textoutput("attribute=" + PB_part.MP2_AllClientFiles.ToString() + " number= " + ((int)PB_part.MP2_AllClientFiles).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    
                    if ((SV2AllServerFiles == true) && (MP2S == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.SV2_AllServerFiles];
#if PB
                        textoutput("attribute=" + PB_part.SV2_AllServerFiles.ToString() + " number= " + ((int)PB_part.SV2_AllServerFiles).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((SV2AllServerFolders == true) && (MP2S == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.SV2_AllServerFolders];
#if PB
                        textoutput("attribute=" + PB_part.SV2_AllServerFolders.ToString() + " number= " + ((int)PB_part.SV2_AllServerFolders).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((SV2AllServerProgramFolders == true) && (MP2C == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.SV2_AllServerProgramFolders];
#if PB
                        textoutput("attribute=" + PB_part.SV2_AllServerProgramFolders.ToString() + " number= " + ((int)PB_part.SV2_AllServerProgramFolders).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((SV2Configuration == true) && (MP2S == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.SV2_Config];
#if PB
                        textoutput("attribute=" + PB_part.SV2_Config.ToString() + " number= " + ((int)PB_part.SV2_Config).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((SV2Database == true) && (MP2S == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.SV2_Database];
#if PB
                        textoutput("attribute=" + PB_part.SV2_Database.ToString() + " number= " + ((int)PB_part.SV2_Database).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((SV2Plugins == true) && (MP2S == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.SV2_Plugins];
#if PB
                        textoutput("attribute=" + PB_part.SV2_Plugins.ToString() + " number= " + ((int)PB_part.SV2_Plugins).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((SV2Defaults == true) && (MP2S == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.SV2_Defaults];
#if PB
                        textoutput("attribute=" + PB_part.SV2_Defaults.ToString() + " number= " + ((int)PB_part.SV2_Defaults).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }

                    if ((channelcardmappings == true) && (TV == true) && (TVServerSettings == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.TV_CardMappings];
#if PB
                        textoutput("attribute=" + PB_part.TV_CardMappings.ToString() + " number= " + ((int)PB_part.TV_CardMappings).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((channels == true) && (TV == true) && (TVServerSettings == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.TV_Channels];
#if PB
                        textoutput("attribute=" + PB_part.TV_Channels.ToString() + " number= " + ((int)PB_part.TV_Channels).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((delete_channels == true) && (TV == true) && (TVServerSettings == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.TV_DeleteAllChannels];
#if PB
                        textoutput("attribute=" + PB_part.TV_DeleteAllChannels.ToString() + " number= " + ((int)PB_part.TV_DeleteAllChannels).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((delete_radiogroups == true) && (TV == true) && (TVServerSettings == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.TV_DeleteAllRadioGroups];
#if PB
                        textoutput("attribute=" + PB_part.TV_DeleteAllRadioGroups.ToString() + " number= " + ((int)PB_part.TV_DeleteAllRadioGroups).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((delete_recordings == true) && (TV == true) && (TVServerSettings == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.TV_DeleteAllRecordings];
#if PB
                        textoutput("attribute=" + PB_part.TV_DeleteAllRecordings.ToString() + " number= " + ((int)PB_part.TV_DeleteAllRecordings).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((delete_schedules == true) && (TV == true) && (TVServerSettings == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.TV_deleteAllSchedules];
#if PB
                        textoutput("attribute=" + PB_part.TV_deleteAllSchedules.ToString() + " number= " + ((int)PB_part.TV_deleteAllSchedules).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((delete_tvgroups == true) && (TV == true && (TVServerSettings == true)))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.TV_DeleteAllTvGroups];
#if PB
                        textoutput("attribute=" + PB_part.TV_DeleteAllTvGroups.ToString() + " number= " + ((int)PB_part.TV_DeleteAllTvGroups).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((epg == true) && (TV == true) && (TVServerSettings == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.TV_EPG];
#if PB
                        textoutput("attribute=" + PB_part.TV_EPG.ToString() + " number= " + ((int)PB_part.TV_EPG).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((general_settings == true) && (TV == true) && (TVServerSettings == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.TV_GeneralSettings];
#if PB
                        textoutput("attribute=" + PB_part.TV_GeneralSettings.ToString() + " number= " + ((int)PB_part.TV_GeneralSettings).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((clickfinder == true) && (TV == true) && (TVServerSettings == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.TV_PluginSettings];
#if PB
                        textoutput("attribute=" + PB_part.TV_PluginSettings.ToString() + " number= " + ((int)PB_part.TV_PluginSettings).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((radiogroups == true) && (TV == true) && (TVServerSettings == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.TV_Radiogroups];
#if PB
                        textoutput("attribute=" + PB_part.TV_Radiogroups.ToString() + " number= " + ((int)PB_part.TV_Radiogroups).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((recordings == true) && (TV == true) && (TVServerSettings == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.TV_Recordings];
#if PB
                        textoutput("attribute=" + PB_part.TV_Recordings.ToString() + " number= " + ((int)PB_part.TV_Recordings).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((schedules == true) && (TV == true) && (TVServerSettings == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.TV_Schedules];
#if PB
                        textoutput("attribute=" + PB_part.TV_Schedules.ToString() + " number= " + ((int)PB_part.TV_Schedules).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((server == true) && (TV == true) && (TVServerSettings == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.TV_Servers];
#if PB
                        textoutput("attribute=" + PB_part.TV_Servers.ToString() + " number= " + ((int)PB_part.TV_Servers).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((tvgroups == true) && (TV == true) && (TVServerSettings == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.TV_TvGroups];
#if PB
                        textoutput("attribute=" + PB_part.TV_TvGroups.ToString() + " number= " + ((int)PB_part.TV_TvGroups).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((TVallPrograms == true) && (TV == true) && (TVServerSettings == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.TV_TvServerProgramFolders];
#if PB
                        textoutput("attribute=" + PB_part.TV_TvServerProgramFolders.ToString() + " number= " + ((int)PB_part.TV_TvServerProgramFolders).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((TVAllFolders == true) && (TV == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.TV_TvServerUserFolders];
#if PB
                        textoutput("attribute=" + PB_part.TV_TvServerUserFolders.ToString() + " number= " + ((int)PB_part.TV_TvServerUserFolders).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((TVServerSettings == true) && (TV == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.TV_TvServerxml];
#if PB
                        textoutput("attribute=" + PB_part.TV_TvServerxml.ToString() + " number= " + ((int)PB_part.TV_TvServerxml).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if ((TVRestart == true) && (TV == true))
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.TV_ConfigRestart];
#if PB
                        textoutput("attribute=" + PB_part.TV_ConfigRestart.ToString() + " number= " + ((int)PB_part.TV_ConfigRestart).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if (TV == true)
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.Start_TVServer];
#if PB
                        textoutput("attribute=" + PB_part.Start_TVServer.ToString() + " number= " + ((int)PB_part.Start_TVServer).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }
                    if (TV == true)
                    {
                        exportprogressBar.Maximum += vector[(int)PB_part.StopTvServer];
#if PB
                        textoutput("attribute=" + PB_part.StopTvServer.ToString() + " number= " + ((int)PB_part.StopTvServer).ToString() + "   value= " + exportprogressBar.Maximum.ToString());
#endif
                    }

                    //add Extrafolderrs
                    exportprogressBar.Maximum += vector[(int)PB_part.ExtraFolders];

                    exportprogressBar.Maximum++;//add one more step for ending

#if (PB)
                        textoutput("ProgressBarMaximum=" + exportprogressBar.Maximum.ToString());
#endif


                    exportprogressBar.Step = 1;
                    exportprogressBar.Value = 0;
                    exportprogressBar.Update();
                }
                else if (action == (int)PB_action.START)
                {
#if (PB)
                    textoutput(DateTime.Now.ToString() + ": Start progress bar part=" + number.ToString());
#endif
                    PB_action_number = number;
                    PB_UPDATER = 0;
                    PB_CANCEL = false;
                    PB_part_start = exportprogressBar.Value;
#if (PB)
                    textoutput("PB_part_start=" + PB_part_start.ToString());
#endif
                    PB_part_end = exportprogressBar.Value + vector[number];
#if (PB)
                    textoutput("PB_part_end=" + PB_part_end.ToString());
                    textoutput("Dration=" + (PB_part_end - PB_part_start).ToString());
#endif
                    PB_th = new System.Threading.Thread(progressbarthread);
                    PB_th.Start();


                }
                else if (action == (int)PB_action.STOP)
                {
#if (PB)
                    textoutput(DateTime.Now.ToString() + ": Stop progress bar part=" + number.ToString());
#endif
                    if (exportprogressBar.Value < PB_part_end)
                    {
                        //update vector with new timing
#if (PB)
                        textoutput("Reducing time from " + vector[number].ToString() + " to " + (exportprogressBar.Value - PB_part_start).ToString());
#endif
                        vector[number] = exportprogressBar.Value - PB_part_start;  //reduce time

                        exportprogressBar.Value = PB_part_end;
                        exportprogressBar.Update();
                    }
                    else
                    {
#if (PB)
                        textoutput("Increasing time from " + vector[number].ToString() + " to " + (vector[number] + PB_UPDATER).ToString());
#endif
                        vector[number] = vector[number] + PB_UPDATER; //increase  time
                    }
                    PB_th.Abort();

                }
                else if (action == (int)PB_action.COMPLETE)
                {
                    exportprogressBar.Maximum = 1;
                    exportprogressBar.Value = exportprogressBar.Maximum;
                    exportprogressBar.Update();
                }
                else if (action == (int)PB_action.CANCEL)
                {
                    PB_CANCEL = true;
                    PB_th.Abort();
                    //textoutput("Cancel progress bar part=" + number.ToString());
                }
            }
            catch (Exception exc)
            {
                textoutput("<RED>Error in progressbar update ");
                textoutput("<RED>Excepption message is " + exc.Message);
            }

        }

        public void progressbarthread()
        {
            if (exportprogressBar == null)
                return; //prgress bar not defined


            //textoutput("Progress bar thread started:" + PB_action_number.ToString());

            for (int i = exportprogressBar.Value; i < PB_part_end; i++)
            {
                System.Threading.Thread.Sleep(1000); //sleep 1s
                while ((PB_PAUSE == true) && (PB_CANCEL == false))
                {
                    System.Threading.Thread.Sleep(1000); //sleep 1s
                    //textoutput("Pausing"+PB_action_number.ToString());
                }
                exportprogressBar.PerformStep();
                exportprogressBar.Update();
                //textoutput("Counting" + PB_action_number.ToString());

            }


            //endless loop till thread is killed
            while (PB_CANCEL == false)
            {
                System.Threading.Thread.Sleep(1000); //sleep 1s
                while ((PB_PAUSE == true) && (PB_CANCEL == false))
                {
                    System.Threading.Thread.Sleep(1000); //sleep 1s
                    //textoutput("Pausing2" + PB_action_number.ToString());
                }
                PB_UPDATER++;
                //textoutput("Updating" + PB_action_number.ToString());
            }
        }

        public void ProgressbarInit()
        {
            progressbar((int)PB_action.INIT, ref PB_Export, 0);
        }

        public void ProgressbarCancel()
        {
            progressbar((int)PB_action.CANCEL, ref PB_Export, PB_action_number);
        }



        public DialogResult myMessageBox(string text1, string text2, MessageBoxButtons button, MessageBoxIcon icon, MessageBoxDefaultButton buttondefault)
        {
            //returnbutton for silent mode make sure they match
            if (SILENT && (button==MessageBoxButtons.AbortRetryIgnore))
                return DialogResult.Abort;
            else if (SILENT && (button == MessageBoxButtons.OK))
                return DialogResult.OK;
            else if (SILENT && (button == MessageBoxButtons.OKCancel))
                return DialogResult.OK;
            else if (SILENT && (button == MessageBoxButtons.RetryCancel))
                return DialogResult.Cancel;
            else if (SILENT && (button == MessageBoxButtons.YesNo))
                return DialogResult.Yes;
            else if (SILENT && (button == MessageBoxButtons.YesNoCancel))
                return DialogResult.Yes;

            PB_PAUSE = true;
            DialogResult i = MessageBox.Show(text1, text2, button, icon, buttondefault);
            PB_PAUSE = false;
            return i;
        }

        private void textoutput(string textlines)
        {
            if (newmessage != null)
            {
                newmessage(textlines);
            }

        }


    }
}

