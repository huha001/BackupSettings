/* 
 *	Copyright (C) 2005-2009 Team MediaPortal
 *	http://www.team-mediaportal.com
 *
 *  This Program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 3, or (at your option)
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

// DEBUG Progressbar 
// #define PB

using System;
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
using MediaPortal.Plugins;
using BackupSettingsPlugin;



namespace SetupTv.Sections
{
    [CLSCompliant(false)]
    public partial class BackupSettingsSetup : SetupTv.SectionSettings
    {
           
        //define window functions
        [DllImport("User32.dll")]
        public static extern Int32 SetForegroundWindow(int hWnd);

        [DllImport("user32.dll")]
        public static extern int FindWindow(string lpClassName, string lpWindowName);	


        #region Properties

        //Constant file names - do not change
        public string RESTART_SETUP_TV_EXE = "RestartSetupTV.exe";
        public string BACKUPSETTING_TV_DIR = "BackupSettings";
        public string BACKUPSETTING_MP_DIR = "BackupSettings";
        public string STATUS_FILE = "Status.txt";
        public string INSTALLPATHS_FILE = "MP_TV_Installpaths.txt";
        public string POSTPROCESSING_FILE = "ImportPostprocessing.txt";
        public string MEDIAPORTALDIRS_FILE = "MediaPortalDirs.xml";
        public string TV_CONFIG_WINDOW_NAME = "MediaPortal - TV Server Configuration";
        public string TV_CONFIG_WINDOW_CLASS = "WindowsForms10.Window.8.app.0.33c0d9d";
        public string SCHEDULED_EXPORT_FILE = "Scheduler.txt";
        
        //main installation folders
        public string MP_PROGRAM_FOLDER = "NOT_DEFINED";
        public string MP_USER_FOLDER = "NOT_DEFINED";
        public string TV_PROGRAM_FOLDER = "NOT_DEFINED";
        public string TV_USER_FOLDER = "NOT_DEFINED";
        public string CENTRAL_DATABASE = "NOT_DEFINED";

        public string MP2_PROGRAM_FOLDER = "NOT_DEFINED";
        public string MP2_USER_FOLDER = "NOT_DEFINED";
        public string SV2_PROGRAM_FOLDER = "NOT_DEFINED";
        public string SV2_USER_FOLDER = "NOT_DEFINED";
        
        
        public string POSTIMPORT = "";
        public bool BUSY = false;
        public bool DEBUG = false;
        //public bool firstuse = false;
        public bool CLOSE_TV_CONFIG = false;
        public bool LIST_RED = false;

        int SELECTED_COL = 1;
        int SELECTED_ROW = 1;
        char BACKUPSETTINGS_COLUMN_SEPARATOR = ';';
        int LISTVIEWCOLUMNS = 6;

        DateTime _LastBackup = DateTime.Now;
        DateTime _NextBackup = DateTime.Now;
        string _OldNextBackup = "";
        string _WeekDays = "";
       
        /*
        int[] PB_Export = new int[49];
        int[] PB_Import = new int[49];
        int PB_PARTNUMBERS = 48;
        int PB_Duplicate = 1;
        
        
        int PB_action_number = 0;*/


        enum PB_action
        {
            INIT = 0,
            START,
            STOP,
            CANCEL,
            COMPLETE,
        }

        enum PB_part
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

        
       

        StreamWriter Status;

        InstallPaths instpaths = new InstallPaths();  //define new instance for folder detection
        BackupSettingsExportImport newexportimport  = new BackupSettingsExportImport();

        System.Threading.Thread th = null;
        System.Threading.Thread PB_th = null;

       
        
        
        

        #endregion Properties       

        #region Constructor

        public BackupSettingsSetup()
        {
            InitializeComponent(); //do not log before status file has been opened

            newexportimport.Status = Status; //hand over status file for closing

            InitializePaths();

            
        }
        #endregion Constructor

        #region SetupTv.SectionSettings

        public override void OnSectionActivated()
        {
            Log.Debug("Backup_SettingsSetup: Configuration activated");

            Log.Debug("TV_PROGRAM_FOLDER=" + TV_PROGRAM_FOLDER);
            Log.Debug("TV_USER_FOLDER=" + TV_USER_FOLDER);
            Log.Debug("MP_PROGRAM_FOLDER=" + MP_PROGRAM_FOLDER);
            Log.Debug("MP_USER_FOLDER=" + MP_USER_FOLDER);

            MyLoadSettings(); //default paths have been searched already in constructor
            Log.Debug("After my loadsettings:");
            Log.Debug("TV_PROGRAM_FOLDER=" + TV_PROGRAM_FOLDER);
            Log.Debug("TV_USER_FOLDER=" + TV_USER_FOLDER);
            Log.Debug("MP_PROGRAM_FOLDER=" + MP_PROGRAM_FOLDER);
            Log.Debug("MP_USER_FOLDER=" + MP_USER_FOLDER);

            //open output file for status reports

            Log.Debug("before check tvprogram=" + TV_PROGRAM_FOLDER + @"\TvService.exe");

            // check installation folder and autodetect
            if (File.Exists(TV_PROGRAM_FOLDER + @"\TvService.exe") == false)
            {
                TV_PROGRAM_FOLDER = instpaths.ask_TV_PROGRAM();
                Log.Debug(instpaths.LOG);
            }

            Log.Debug("After my instpaths.ask_TV_PROGRAM():");
            Log.Debug("TV_PROGRAM_FOLDER=" + TV_PROGRAM_FOLDER);
            Log.Debug("TV_USER_FOLDER=" + TV_USER_FOLDER);
            Log.Debug("MP_PROGRAM_FOLDER=" + MP_PROGRAM_FOLDER);
            Log.Debug("MP_USER_FOLDER=" + MP_USER_FOLDER);

            Log.Debug("after ask tvprogram=" + TV_PROGRAM_FOLDER + @"\TvService.exe");

            if (File.Exists(TV_PROGRAM_FOLDER + @"\TvService.exe") == false)
            {
                MessageBox.Show("TV server program folder does not exist ", "Error");
                return;

            }
            

            if (Directory.Exists(TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR) == false)
                Directory.CreateDirectory(TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR);

            //Log.Debug("Statusfile="+TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + STATUS_FILE);

            // read old status from file
            try
            {
                if (File.Exists(TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + STATUS_FILE) == true)
                {
                    StreamReader sfile = File.OpenText(TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + STATUS_FILE);
                    String textlines = null;
                    if (listView1 != null)
                        listView1.Items.Clear();
                    while ((textlines = sfile.ReadLine()) != null)
                    {

                        Color mycolor;
                        if (textlines.StartsWith("<RED>") == true)
                        {//color red
                            textlines = textlines.Substring(5, textlines.Length - 5);
                            mycolor = Color.Red;
                        }
                        else if (textlines.StartsWith("<YELLOW>") == true)
                        {//color yellow
                            textlines = textlines.Substring(8, textlines.Length - 8);
                            mycolor = Color.Orange;
                        }
                        else if (textlines.StartsWith("<GREEN>") == true)
                        {//color green
                            textlines = textlines.Substring(8, textlines.Length - 8);
                            mycolor = Color.Green;
                        }
                        else
                        {//color black
                            mycolor = Color.Black;
                        }

                        if (textlines.StartsWith("ProgressBar.Maximum") == true)
                        {
                            string maxstring = textlines.Substring(19, textlines.Length - 19);

                            try
                            {
                                progressBar.Maximum = Convert.ToInt32(maxstring);
                            }
                            catch
                            {
                                textoutput("<RED>Error in converting maxstring=" + maxstring);
                            }
                        }
                        else if (textlines.StartsWith("ProgressBar.Value") == true)
                        {
                            string valuestring = textlines.Substring(17, textlines.Length - 17);

                            try
                            {
                                progressBar.Value = Convert.ToInt32(valuestring);
                                progressBar.Update();
                                textlines = "";
                            }
                            catch
                            {
                                textoutput("<RED>Error in converting valuestring=" + valuestring);
                            }
                        }
                        else
                        {
                            string text = "";
                            if (listView1 != null)
                            {
                                char[] splitterchars = { '\n' };  //split lines with \n
                                string[] lines = textlines.Split(splitterchars);
                                foreach (string line in lines)
                                {
                                    text = line;
                                    while (text.Length > 70)   //split long lines
                                    {
                                        int linelength = 69;
                                        for (int i = 69; i >= 4; i--)
                                        {
                                            if (text[i] == ' ')
                                            {
                                                linelength = i;
                                                break;
                                            }
                                        }
                                        string pretext = text.Substring(0, linelength);

                                        listView1.Items.Add(pretext);
                                        listView1.Items[listView1.Items.Count - 1].ForeColor = mycolor;
                                        listView1.Items[listView1.Items.Count - 1].EnsureVisible();

                                        text = "+  " + text.Substring(linelength, text.Length - linelength);

                                    }

                                    listView1.Items.Add(text);
                                    listView1.Items[listView1.Items.Count - 1].ForeColor = mycolor;
                                    listView1.Items[listView1.Items.Count - 1].EnsureVisible();
                                }
                            }//end if
                        }
                    }//end while
                    sfile.Close();  //old status is now displayed in listbox
                }
            }
            catch (Exception exc)
            {
                textoutput("BackupSettingsSetup: Fatal Error: Could not read status file " + TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + STATUS_FILE + " - Exception:" + exc.Message);
            }

            if (BackupSettings.POSTPROCESSING == true)
            {
                MessageBox.Show("Postprocessing of previous import data has not been finished - please wait", "Error");
                return;
            }
            
                

            //open file in append mode for new status
            try
            {
                Status = File.AppendText(TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + STATUS_FILE);
                Log.Debug("BackupSettingsSetup: Status File opened in append mode");
            }
            catch (Exception exc)
            {
                Log.Debug("BackupSettingsSetup: Fatal Error: Could not open file in append mode for " + TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + STATUS_FILE + " - Exception:" + exc.Message);
            }

            SaveSettings();  //ensures that paths are written to setting file and setting file does exist for importexport
                

            




                //enable text events
            if (instpaths != null)
            {
                instpaths.newmessage += new textmessage(textoutput);
                instpaths.DEBUG = DEBUG;
            }

            if (newexportimport != null)
            {
                newexportimport.ExportImportInit(ref progressBar, false);
                newexportimport.newmessage += new textexportmessage(textoutput);
                newexportimport.MyLoadSettings();
                newexportimport.getallversionnumbers("", false); //get only program version numbers after loadsettings
            }
            else
            {
                Log.Debug("BackupSettingsSetup: Fatal Error: Could not initialize newexportimport");
            }

            //enable autodate if checked
            if (checkBoxUseAutoDate.Checked == true)
            {
                //create automated foldername
                MySaveSettings();
                newexportimport.MyLoadSettings();
                filenametextBox.Text = newexportimport.CreateAutomatedFolderName(filenametextBox.Text, WhoAmI.Tv_Server);
            }
                
            UpdateGUI();
            

            base.OnSectionActivated();
        }


        public override void OnSectionDeActivated()
        {


            //disable text events
            if (instpaths != null)
            {
                instpaths.newmessage -= new textmessage(textoutput);
            }

            if (newexportimport != null)
            {
                newexportimport.newmessage -= new textexportmessage(textoutput);
            }
            

            Log.Info("Backup_Settings: Configuration deactivated");
            MySaveSettings();
            if (BUSY == true)
            {
                textoutput("<RED>\nTV Server Configuration has been terminated");
                textoutput("<RED>because you exited BackupSettings");
                textoutput("<RED>before the last operation completed");
                textoutput("<RED>You must repeat your last operation\n");
            }
            if (Status != null)
            {
                Status.WriteLine("ProgressBar.Maximum  "+progressBar.Maximum.ToString());
                Status.WriteLine("ProgressBar.Value  " + progressBar.Value.ToString());
                Status.Close();  //  close status file
            }


            if (BUSY == true)
            {
                Log.Debug("BackupSettingsSetup: Kill SetupTv process");
                Process[] processes = Process.GetProcessesByName("SetupTv");
               
                //starting restartsetuptv.exe with message code
                Process app = new Process();
                ProcessStartInfo appstartinfo = new ProcessStartInfo();
                appstartinfo.FileName = TV_PROGRAM_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + RESTART_SETUP_TV_EXE;
                appstartinfo.Arguments = "\"" + TV_PROGRAM_FOLDER + "\" THREADKILL";
                appstartinfo.WorkingDirectory = TV_PROGRAM_FOLDER + @"\" + BACKUPSETTING_TV_DIR;
                app.StartInfo = appstartinfo;
                try
                {
                    app.Start();
                }
                catch (Exception exc)
                {
                    Log.Debug("<RED>Error in starting the process " + appstartinfo.FileName);
                    Log.Debug("<RED>Exception message is " + exc.Message);
                }

                //killing setuptv because threads are still running 
                foreach (Process process in processes)
                {
                    process.Kill();
                }
            }

            base.OnSectionDeActivated();
        }


        #endregion SetupTv.SectionSettings

        public void InitializePaths()
        {
            //do not use any debug output in this routine as status file is not defined yet!!!
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
            instpaths.DEBUG = DEBUG;

            instpaths.GetInstallPaths(); //do not log before status file has been opened

            textBoxMP1P.Text = instpaths.MP_PROGRAM_FOLDER;
            textBoxMP1U.Text = instpaths.MP_USER_FOLDER;
            textBoxTV1P.Text = instpaths.TV_PROGRAM_FOLDER;
            textBoxTV1U.Text = instpaths.TV_USER_FOLDER;

            instpaths.GetInstallPathsMP2(); //do not log before status file has been opened

            textBoxMP2P.Text = instpaths.MP2_PROGRAM_FOLDER;
            textBoxMP2U.Text = instpaths.MP2_USER_FOLDER;
            textBoxSV2P.Text = instpaths.SV2_PROGRAM_FOLDER;
            textBoxSV2U.Text = instpaths.SV2_USER_FOLDER;

            instpaths.GetMediaPortalDirs(); //do not log before status file has been opened

            instpaths.GetMediaPortalDirsMP2(); //do not log before status file has been opened

            //use loadsettings or installpaths from starting tvservice before loadsettings
            MP_PROGRAM_FOLDER = textBoxMP1P.Text;
            MP_USER_FOLDER = textBoxMP1U.Text;
            TV_PROGRAM_FOLDER = textBoxTV1P.Text;
            TV_USER_FOLDER = textBoxTV1U.Text;

            MP2_PROGRAM_FOLDER = textBoxMP2P.Text;
            MP2_USER_FOLDER = textBoxMP2U.Text; ;
            SV2_PROGRAM_FOLDER = textBoxSV2P.Text;
            SV2_USER_FOLDER = textBoxSV2U.Text;

            setting = layer.GetSetting("Backup_SettingsSetup_TV_PROGRAM_FOLDER", "NOT_DEFINED");
            if ((setting.Value != "NOT_DEFINED") && (setting.Value != "") && (File.Exists(setting.Value+@"\TvService.exe")))
                textBoxTV1P.Text = setting.Value;

            setting = layer.GetSetting("Backup_SettingsSetup_TV_USER_FOLDER", "NOT_DEFINED");
            if ((setting.Value != "NOT_DEFINED") && (setting.Value != "") && (Directory.Exists(setting.Value)))
                textBoxTV1U.Text = setting.Value;

            setting = layer.GetSetting("Backup_SettingsSetup_MP_PROGRAM_FOLDER", "NOT_DEFINED");
            if ((setting.Value != "NOT_DEFINED") && (setting.Value != "") && (File.Exists(setting.Value+@"\MediaPortal.exe")))
                textBoxMP1P.Text = setting.Value;

            setting = layer.GetSetting("Backup_SettingsSetup_MP_USER_FOLDER", "NOT_DEFINED");
            if ((setting.Value != "NOT_DEFINED") && (setting.Value != "") && (Directory.Exists(setting.Value)))
                textBoxMP1U.Text = setting.Value;

            setting = layer.GetSetting("Backup_SettingsSetup_MP2_PROGRAM_FOLDER", "NOT_DEFINED");
            if ((setting.Value != "NOT_DEFINED") && (setting.Value != "") && (File.Exists(setting.Value+@"\MP2-Client.exe")))
                textBoxMP2P.Text = setting.Value;

            setting = layer.GetSetting("Backup_SettingsSetup_MP2_USER_FOLDER", "NOT_DEFINED");
            if ((setting.Value != "NOT_DEFINED") && (setting.Value != "") && (Directory.Exists(setting.Value)))
                textBoxMP2U.Text = setting.Value;

            setting = layer.GetSetting("Backup_SettingsSetup_SV2_PROGRAM_FOLDER", "NOT_DEFINED");
            if ((setting.Value != "NOT_DEFINED") && (setting.Value != "") && (File.Exists(setting.Value + @"\MP2-Server.exe")))
                textBoxSV2P.Text = setting.Value;

            setting = layer.GetSetting("Backup_SettingsSetup_SV2_USER_FOLDER", "NOT_DEFINED");
            if ((setting.Value != "NOT_DEFINED") && (setting.Value != "") && (Directory.Exists(setting.Value)))
                textBoxSV2U.Text = setting.Value;

            //do not use any debug output in this routine as status file is not defined yet!!!
        }

        public void MyLoadSettings()
        {
            try
            {
                TvBusinessLayer layer = new TvBusinessLayer();
                Setting setting;

                setting = layer.GetSetting("Backup_SettingsSetup_debug", "false");
                if (setting.Value.ToLower() == "true")
                {
                    CheckBoxDebugBackupSettings.Checked = true;
                }
                else
                {
                    CheckBoxDebugBackupSettings.Checked = false;
                }

                


                setting = layer.GetSetting("Backup_SettingsSetup_TV_PROGRAM_FOLDER", "NOT_DEFINED");
                if ((setting.Value != "NOT_DEFINED") && (setting.Value != "") && (File.Exists(setting.Value + @"\TvService.exe")))
                    textBoxTV1P.Text = setting.Value;

                setting = layer.GetSetting("Backup_SettingsSetup_TV_USER_FOLDER", "NOT_DEFINED");
                if ((setting.Value != "NOT_DEFINED") && (setting.Value != "") && (Directory.Exists(setting.Value)))
                    textBoxTV1U.Text = setting.Value;

                setting = layer.GetSetting("Backup_SettingsSetup_MP_PROGRAM_FOLDER", "NOT_DEFINED");
                if ((setting.Value != "NOT_DEFINED") && (setting.Value != "") && (File.Exists(setting.Value + @"\MediaPortal.exe")))
                    textBoxMP1P.Text = setting.Value;

                setting = layer.GetSetting("Backup_SettingsSetup_MP_USER_FOLDER", "NOT_DEFINED");
                if ((setting.Value != "NOT_DEFINED") && (setting.Value != "") && (Directory.Exists(setting.Value)))
                    textBoxMP1U.Text = setting.Value;

                setting = layer.GetSetting("Backup_SettingsSetup_MP2_PROGRAM_FOLDER", "NOT_DEFINED");
                if ((setting.Value != "NOT_DEFINED") && (setting.Value != "") && (File.Exists(setting.Value + @"\MP2-Client.exe")))
                    textBoxMP2P.Text = setting.Value;

                setting = layer.GetSetting("Backup_SettingsSetup_MP2_USER_FOLDER", "NOT_DEFINED");
                if ((setting.Value != "NOT_DEFINED") && (setting.Value != "") && (Directory.Exists(setting.Value)))
                    textBoxMP2U.Text = setting.Value;

                setting = layer.GetSetting("Backup_SettingsSetup_SV2_PROGRAM_FOLDER", "NOT_DEFINED");
                if ((setting.Value != "NOT_DEFINED") && (setting.Value != "") && (File.Exists(setting.Value + @"\MP2-Server.exe")))
                    textBoxSV2P.Text = setting.Value;

                setting = layer.GetSetting("Backup_SettingsSetup_SV2_USER_FOLDER", "NOT_DEFINED");
                if ((setting.Value != "NOT_DEFINED") && (setting.Value != "") && (Directory.Exists(setting.Value)))
                    textBoxSV2U.Text = setting.Value;


                //use loadsettings or installpaths from starting tvservice
                MP_PROGRAM_FOLDER = textBoxMP1P.Text;
                MP_USER_FOLDER = textBoxMP1U.Text;
                TV_PROGRAM_FOLDER = textBoxTV1P.Text;
                TV_USER_FOLDER = textBoxTV1U.Text;

                MP2_PROGRAM_FOLDER = textBoxMP2P.Text;
                MP2_USER_FOLDER = textBoxMP2U.Text; ;
                SV2_PROGRAM_FOLDER = textBoxSV2P.Text;
                SV2_USER_FOLDER = textBoxSV2U.Text;

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


                setting = layer.GetSetting("Backup_SettingsSetup_filename", @"C:\MediaPortal Backups");
                filenametextBox.Text = setting.Value;

                setting = layer.GetSetting("Backup_SettingsSetup_UseAutoDate", "true");
                checkBoxUseAutoDate.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSetup_TV", "true");
                checkBoxTV.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSetup_MP", "true");
                checkBoxMP.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSetup_MP2C", "true");
                checkBoxMP2C.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSetup_MP2S", "true");
                checkBoxMP2S.Checked = Convert.ToBoolean(setting.Value);


                UpdateGUI();

                setting = layer.GetSetting("Backup_SettingsSetup_filter_server", "true");
                checkBoxServer.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSetup_filter_channels", "true");
                checkBoxChannels.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSetup_filter_channelcardmappings", "true");
                checkBoxChannelCardMapping.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSetup_filter_tvgroups", "true");
                checkBoxChannelGroups.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSetup_filter_radiogroups", "true");
                checkBoxRadioGroups.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSetup_filter_schedules", "true");
                checkBoxSchedules.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSetup_filter_epg", "false");
                checkBoxEPG.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSetup_filter_recordings", "true");
                checkBoxRecordings.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSetup_filter_general_settings", "true");
                checkBoxAllSettings.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSetup_filter_plugins", "true"); //Backup_SettingsSetup_filter_plugins
                checkBoxClickfinder.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSetup_delete_channels", "true");
                checkBoxDeleteChannels.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSetup_delete_tvgroups", "true");
                checkBoxDeleteTVGroups.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSetup_delete_radiogroups", "true");
                checkBoxDeleteRadioGroups.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSetup_delete_schedules", "true");
                checkBoxDeleteSchedules.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSetup_delete_recordings", "true");
                checkBoxDeleteRecordings.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsTVServerSettings", "true");
                checkBoxTVServer.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsTVallPrograms", "true");
                checkBoxTVallPrograms.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsTVAllFolders", "true");
                checkBoxTVallFolders.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsTVRestart", "true");
                checkBoxtvrestart.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsTVAutoCorrect", "true");
                checkBoxAutoCorrectDataBase.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsMPDatabase", "true");
                checkBoxMPDatabase.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsMPInputDevice", "true");
                checkBoxMPInputDevice.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsMPPlugins", "true");
                checkBoxMPPlugins.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsMPProgramXml", "true");
                checkBoxMPProgramXml.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsMPSkins", "true");
                checkBoxMPSkins.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsMPThumbs", "true");
                checkBoxMPThumbs.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsMPUserXML", "true");
                checkBoxMPUserXML.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsMPxmltv", "true");
                checkBoxMPxmltv.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsMPMusicPlayer", "true");
                checkBoxMPMusicPlayer.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsMPDeleteCache", "true");
                checkBoxMPDeleteCache.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsMPAllFolders", "true");
                checkBoxMPAllFolders.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsMPAllProgram", "true");
                checkBoxMPAllProgram.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsMP2Defaults", "true");
                checkBoxMP2Defaults.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsMP2Config", "true");
                checkBoxMP2Config.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsMP2Plugins", "true");
                checkBoxMP2Plugins.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsMP2AllClientFolders", "true");
                checkBoxMP2AllClientFolders.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsMP2AllClientProgramFolders", "true");
                checkBoxMP2AllClientProgramFolders.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsMP2AllClientFiles", "true");
                checkBoxMP2AllClientFiles.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSV2Configuration", "true");
                checkBoxSV2Config.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSV2Database", "true");
                checkBoxSV2Database.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSV2Plugins", "true");
                checkBoxSV2Plugins.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSV2Defaults", "true");
                checkBoxSV2Defaults.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSV2AllServerFolders", "true");
                checkBoxSV2AllServerFolders.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSV2AllServerProgramFolders", "true");
                checkBoxSV2AllServerProgramFolders.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSV2AllServerFiles", "true");
                checkBoxSV2AllServerFiles.Checked = Convert.ToBoolean(setting.Value);

                /*setting = layer.GetSetting("Backup_Settingsfirstuse", "true");
                firstuse = Convert.ToBoolean(setting.Value);*/

                setting = layer.GetSetting("Backup_SettingsDuplicateInteractive", "false");
                checkBoxduplicateinteractive.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsDuplicateAutoprocess", "false");
                checkBoxduplicateautoprocess.Checked = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_Settings_Easy", "true");
                if (Convert.ToBoolean(setting.Value) == true)
                {
                    radioButtonEasy.Checked = true;
                }

                setting = layer.GetSetting("Backup_Settings_Expert", "false");
                if (Convert.ToBoolean(setting.Value) == true)
                {
                    radioButtonExpert.Checked = true;
                }

                //listviewdata
                setting = layer.GetSetting("Backup_SettingsColumnSeparator", ";");
                BACKUPSETTINGS_COLUMN_SEPARATOR = setting.Value[0];



                //remove all rows besides last row 
                int rowcount = dataGridView1.Rows.Count;
                for (int i = 0; i < rowcount - 1; i++)
                {
                    dataGridView1.Rows.Remove(dataGridView1.Rows[0]);
                }

                if (DEBUG == true)
                    textoutput("Longsetting BackupSettings_ListView");

                //load longsettings 
                string listviewdata = load_longsettings("BackupSettings_ListView");



#if (PB)
                textoutput("1) liestview=" + setting.Value);
                
#endif
                if (listviewdata == "")
                {
                    listviewdata = autoinit(); //autoinitialization
                }



                string[] rowdata = listviewdata.Split('\n');


#if (PB)
                textoutput("2) liestview=" + setting.Value);
                textoutput("Row Count=" + dataGridView1.Rows.Count.ToString());
#endif


                int ctr = 0;
                foreach (string row in rowdata)
                {

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
                            textoutput("row count=" + tempcolumndata.Length.ToString());
                    }


                    int newrow = dataGridView1.Rows.Count - 1;
                    try //add new default row 
                    {

                        //textoutput("Adding row=" + newrow.ToString());
                        dataGridView1.Rows.Insert(newrow, false, "", "", true, "", "");

                    }
                    catch (Exception exc)
                    {
                        textoutput("Adding row failed with message \n" + exc.Message);
                    }


                    //Filling actual values and check for valid entries
                    //active
                    if (tempcolumndata[0] == "True")
                    {
                        dataGridView1[0, ctr].Value = true;
                    }
                    else
                    {
                        dataGridView1[0, ctr].Value = false;
                    }
                    //name
                    dataGridView1[1, ctr].Value = tempcolumndata[1];


                    //folder
                    dataGridView1[2, ctr].Value = tempcolumndata[2];


                    //overwrite
                    if (tempcolumndata[3] == "True")
                    {
                        dataGridView1[3, ctr].Value = true;
                    }
                    else
                    {
                        dataGridView1[3, ctr].Value = false;
                    }

                    //kill process
                    dataGridView1[4, ctr].Value = tempcolumndata[4];

                    //restart
                    dataGridView1[5, ctr].Value = tempcolumndata[5];

                    ctr++;
                }


                //Scheduler
                
                setting = layer.GetSetting("Backup_SettingsAutomatedExportFolderName",string.Empty);                
                textBoxAutomatedExportFolderName.Text = setting.Value;
                if (textBoxAutomatedExportFolderName.Text == string.Empty)
                    textBoxAutomatedExportFolderName.Text = filenametextBox.Text;

                _LastBackup = DateTime.Now;
                setting = layer.GetSetting("Backup_SettingsLastBackup", DateTime.Now.ToString("yyyy-MM-dd_HH:mm", System.Globalization.CultureInfo.InvariantCulture));
                try
                {
                    _LastBackup = DateTime.ParseExact(setting.Value, "yyyy-MM-dd_HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                }
                catch (Exception exc)
                {
                    textoutput("LastBackup failed with exception: " + exc.Message);
                }

                labelLastCheckDate.Text = _LastBackup.ToString();


                _NextBackup = DateTime.Now.AddDays(30);//add one month default
                setting = layer.GetSetting("Backup_SettingsNextBackup", DateTime.Now.ToString("yyyy-MM-dd_HH:mm", System.Globalization.CultureInfo.InvariantCulture));
                _OldNextBackup = setting.Value;
                try
                {
                    _NextBackup = DateTime.ParseExact(setting.Value, "yyyy-MM-dd_HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                }
                catch (Exception exc)
                {
                    textoutput("NextBackup failed with exception: " + exc.Message);
                }



                setting = layer.GetSetting("Backup_SettingsEnableScheduler", "false");
                checkBoxEnableScheduler.Checked = Convert.ToBoolean(setting.Value);

                if (!checkBoxEnableScheduler.Checked)
                {
                    labelCheckingdate.Text = "Not Scheduled";
                    _NextBackup = DateTime.ParseExact("2100-01-01_00:00", "yyyy-MM-dd_HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                }

                setting = layer.GetSetting("Backup_SettingsSchedulerDays", "30");
                comboBoxdays.Text = setting.Value;
                checkcombotextbox(ref comboBoxdays, "D2", 1, 1000000, "Days");

                setting = layer.GetSetting("Backup_SettingsSchedulerHours", "06");
                comboBoxhours.Text = setting.Value;
                checkcombotextbox(ref comboBoxhours, "D2", 0, 23, "Hours");

                setting = layer.GetSetting("Backup_SettingsSchedulerMinutes", "00");
                comboBoxminutes.Text = setting.Value;
                checkcombotextbox(ref comboBoxminutes, "D2", 0, 59, "Minutes");

                setting = layer.GetSetting("Backup_SettingsSchedulerKeepNumber", "3");
                comboBoxKeepNumber.Text = setting.Value;
                if (comboBoxKeepNumber.Text != "All")
                    checkcombotextbox(ref comboBoxKeepNumber, "D2", 1, 1000, "Number");

                setting = layer.GetSetting("Backup_SettingsSchedulerWeekDays", "Any");
                _WeekDays = setting.Value;

                if (_WeekDays.Contains("Monday"))
                    checkBoxMon.Checked = true;
                else
                    checkBoxMon.Checked = false;

                if (_WeekDays.Contains("Tuesday"))
                    checkBoxTue.Checked = true;
                else
                    checkBoxTue.Checked = false;

                if (_WeekDays.Contains("Wednesday"))
                    checkBoxWed.Checked = true;
                else
                    checkBoxWed.Checked = false;

                if (_WeekDays.Contains("Thursday"))
                    checkBoxThur.Checked = true;
                else
                    checkBoxThur.Checked = false;

                if (_WeekDays.Contains("Friday"))
                    checkBoxFri.Checked = true;
                else
                    checkBoxFri.Checked = false;

                if (_WeekDays.Contains("Saturday"))
                    checkBoxSat.Checked = true;
                else
                    checkBoxSat.Checked = false;

                if (_WeekDays.Contains("Sunday"))
                    checkBoxSun.Checked = true;
                else
                    checkBoxSun.Checked = false;

                if (_WeekDays.Contains("Any"))
                    checkBoxAny.Checked = true;
                else
                    checkBoxAny.Checked = false;

            }
            catch (Exception ex)
            {
                filenametextBox.Text = "";
                textoutput("<RED>Backup_SettingsSetup - MyLoadSettings(): " + ex.Message);
            }
        }
       
        public string autoinit()
        {
            string listview = "";
            if (Directory.Exists(MP_USER_FOLDER + @"\..\..\IR Server Suite\IR Commands") == true)
            {
                listview += @"false;IR Server Suite\IR Commands;" + MP_USER_FOLDER + @"\..\..\IR Server Suite\IR Commands;True;;" + "\n";
            }
            if (Directory.Exists(MP_USER_FOLDER + @"\..\..\IR Server Suite\IR Server") == true)
            {
                listview += @"false;IR Server Suite\IR Server;" + MP_USER_FOLDER + @"\..\..\IR Server Suite\IR Server;True;;" + "\n";
            }
            if (Directory.Exists(MP_USER_FOLDER + @"\..\..\IR Server Suite\MP Control Plugin") == true)
            {
                listview += @"false;IR Server Suite\MP Control Plugin;" + MP_USER_FOLDER + @"\..\..\IR Server Suite\MP Control Plugin;True;;" + "\n";
            }
            if (Directory.Exists(MP_USER_FOLDER + @"\..\..\IR Server Suite\Set Top Boxes") == true)
            {
                listview += @"false;IR Server Suite\Set Top Boxes;" + MP_USER_FOLDER + @"\..\..\IR Server Suite\Set Top Boxes;True;;" + "\n";
            }
            if (Directory.Exists(MP_USER_FOLDER + @"\..\..\IR Server Suite\Translator") == true)
            {
                listview += @"false;IR Server Suite\Translator;" + MP_USER_FOLDER + @"\..\..\IR Server Suite\Translator;True;;" + "\n";
            }
            if (Directory.Exists(MP_USER_FOLDER + @"\..\..\IR Server Suite\TV3 Blaster Plugin") == true)
            {
                listview += @"false;IR Server Suite\TV3 Blaster Plugin;" + MP_USER_FOLDER + @"\..\..\IR Server Suite\TV3 Blaster Plugin;True;;" + "\n";
            }

            return listview;
        }

        public void checkcombotextbox(ref ComboBox mycombobox, string format, int min, int max, string fieldname)
        {
            //Log.Debug("Field " + fieldname + " before Checking: " + mycombobox.Text, (int)LogSetting.DEBUG);
            int number = min;
            try
            {
                number = Convert.ToInt32(mycombobox.Text);
            }
            catch
            {
                number = min;
            }

            //min checking
            if (number < min)
            {
                MessageBox.Show("Minimum " + fieldname + " is " + min.ToString(), "Warning");
                number = min;
            }


            //max checking
            if (number > max)
            {
                MessageBox.Show("Maximum " + fieldname + " is " + max.ToString(), "Warning");
                number = max;
            }

            //read numbers from combobox items
            try
            {
                int item_number = 0;
                int old_item_number = 0;

                int ctr = 0;



                foreach (string item in mycombobox.Items)
                {


                    try
                    {
                        item_number = Convert.ToInt32(item);
                        if (item_number == number)
                        {
                            //Log.Debug("Exact Combobox item found -done", (int)LogSetting.DEBUG);
                            mycombobox.Text = number.ToString(format);
                            return;
                        }

                        if (ctr > 0) //ignore first eleement to get interval
                        {
                            if ((number > old_item_number) && (number < item_number))
                            {
                                break;
                            }
                        }
                        else //ctr==0 check for first item insert
                        {
                            if (number < item_number)
                            {
                                break;
                            }
                        }
                        old_item_number = item_number;
                        ctr++;

                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show("Exception: " + exc.Message);
                    }



                }
                //insert new eleemnt to combobox
                //Log.Debug("Inserting new combobox element " + number.ToString() + " at position " + ctr.ToString(), (int)LogSetting.DEBUG);
                mycombobox.Items.Insert(ctr, number.ToString(format));

            }
            catch (Exception exc)
            {
                MessageBox.Show("Exception: " + exc.Message);
            }


            mycombobox.Text = number.ToString(format);
            //Log.Debug("Combobox After Checking: " + mycombobox.Text, (int)LogSetting.DEBUG);
        }

        public void MySaveSettings()
        {
            try
            {
                TvBusinessLayer layer = new TvBusinessLayer();
                Setting setting;
                setting = layer.GetSetting("Backup_SettingsSetup_filename", "");
                setting.Value = filenametextBox.Text;
                setting.Persist();

                if (textBoxTV1P.Text == string.Empty)
                    textBoxTV1P.Text = "NOT_DEFINED";

                if (textBoxTV1U.Text == string.Empty)
                    textBoxTV1U.Text = "NOT_DEFINED";

                if (textBoxMP1P.Text == string.Empty)
                    textBoxMP1P.Text = "NOT_DEFINED";

                if (textBoxMP1U.Text == string.Empty)
                    textBoxMP1U.Text = "NOT_DEFINED";

                if (textBoxSV2P.Text == string.Empty)
                    textBoxSV2P.Text = "NOT_DEFINED";

                if (textBoxSV2U.Text == string.Empty)
                    textBoxSV2U.Text = "NOT_DEFINED";

                if (textBoxMP2P.Text == string.Empty)
                    textBoxMP2P.Text = "NOT_DEFINED";

                if (textBoxMP2U.Text == string.Empty)
                    textBoxMP2U.Text = "NOT_DEFINED";


                setting = layer.GetSetting("Backup_SettingsSetup_TV_PROGRAM_FOLDER", "NOT_DEFINED");
                setting.Value = textBoxTV1P.Text;
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsSetup_TV_USER_FOLDER", "NOT_DEFINED");
                setting.Value = textBoxTV1U.Text;
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsSetup_MP_PROGRAM_FOLDER", "NOT_DEFINED");
                setting.Value = textBoxMP1P.Text;
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsSetup_MP_USER_FOLDER", "NOT_DEFINED");
                setting.Value = textBoxMP1U.Text;
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsSetup_MP2_PROGRAM_FOLDER", "NOT_DEFINED");
                setting.Value = textBoxMP2P.Text;
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsSetup_MP2_USER_FOLDER", "NOT_DEFINED");
                setting.Value = textBoxMP2U.Text;
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsSetup_SV2_PROGRAM_FOLDER", "NOT_DEFINED");
                setting.Value = textBoxSV2P.Text;
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsSetup_SV2_USER_FOLDER", "NOT_DEFINED");
                setting.Value = textBoxSV2U.Text;
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsSetup_debug", "false");
                setting.Value = Convert.ToString(DEBUG);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsSetup_UseAutoDate", "true");
                setting.Value = Convert.ToString(checkBoxUseAutoDate.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsSetup_TV", "true");
                setting.Value = Convert.ToString(checkBoxTV.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsSetup_MP", "true");
                setting.Value = Convert.ToString(checkBoxMP.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsSetup_MP2C", "true");
                setting.Value = Convert.ToString(checkBoxMP2C.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsSetup_MP2S", "true");
                setting.Value = Convert.ToString(checkBoxMP2S.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsSetup_filter_server", "true");
                setting.Value = Convert.ToString(checkBoxServer.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsSetup_filter_channels", "true");
                setting.Value = Convert.ToString(checkBoxChannels.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsSetup_filter_channelcardmappings", "true");
                setting.Value = Convert.ToString(checkBoxChannelCardMapping.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsSetup_filter_schedules", "true");
                setting.Value = Convert.ToString(checkBoxSchedules.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsSetup_filter_epg", "false");
                setting.Value = Convert.ToString(checkBoxEPG.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsSetup_filter_recordings", "true");
                setting.Value = Convert.ToString(checkBoxRecordings.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsSetup_filter_tvgroups", "true");
                setting.Value = Convert.ToString(checkBoxChannelGroups.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsSetup_filter_radiogroups", "true");
                setting.Value = Convert.ToString(checkBoxRadioGroups.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsSetup_filter_general_settings", "true");
                setting.Value = Convert.ToString(checkBoxAllSettings.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsSetup_filter_plugins", "true");  //changed name
                setting.Value = Convert.ToString(checkBoxClickfinder.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsSetup_delete_channels", "true");
                setting.Value = Convert.ToString(checkBoxDeleteChannels.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsSetup_delete_tvgroups", "true");
                setting.Value = Convert.ToString(checkBoxDeleteTVGroups.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsSetup_delete_radiogroups", "true");
                setting.Value = Convert.ToString(checkBoxDeleteRadioGroups.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsSetup_delete_schedules", "true");
                setting.Value = Convert.ToString(checkBoxDeleteSchedules.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsSetup_delete_recordings", "true");
                setting.Value = Convert.ToString(checkBoxDeleteRecordings.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsTVServerSettings", "true");
                setting.Value = Convert.ToString(checkBoxTVServer.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsTVPLugins", "true");
                setting.Value = Convert.ToString(checkBoxTVallPrograms.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsTVallPrograms", "true");
                setting.Value = Convert.ToString(checkBoxTVallPrograms.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsTVAllFolders", "true");
                setting.Value = Convert.ToString(checkBoxTVallFolders.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsTVRestart", "true");
                setting.Value = Convert.ToString(checkBoxtvrestart.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsTVAutoCorrect", "true");
                setting.Value = Convert.ToString(checkBoxAutoCorrectDataBase.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsMPDatabase", "true");
                setting.Value = Convert.ToString(checkBoxMPDatabase.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsMPInputDevice", "true");
                setting.Value = Convert.ToString(checkBoxMPInputDevice.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsMPPlugins", "true");
                setting.Value = Convert.ToString(checkBoxMPPlugins.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsMPProgramXml", "true");
                setting.Value = Convert.ToString(checkBoxMPProgramXml.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsMPSkins", "true");
                setting.Value = Convert.ToString(checkBoxMPSkins.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsMPThumbs", "true");
                setting.Value = Convert.ToString(checkBoxMPThumbs.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsMPUserXML", "true");
                setting.Value = Convert.ToString(checkBoxMPUserXML.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsMPxmltv", "true");
                setting.Value = Convert.ToString(checkBoxMPxmltv.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsMPMusicPlayer", "true");
                setting.Value = Convert.ToString(checkBoxMPMusicPlayer.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsMPDeleteCache", "true");
                setting.Value = Convert.ToString(checkBoxMPDeleteCache.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsMPAllFolders", "true");
                setting.Value = Convert.ToString(checkBoxMPAllFolders.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsMPAllProgram", "true");
                setting.Value = Convert.ToString(checkBoxMPAllProgram.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsMP2Config", "true");
                setting.Value = Convert.ToString(checkBoxMP2Config.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsMP2Defaults", "true");
                setting.Value = Convert.ToString(checkBoxMP2Defaults.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsMP2Plugins", "true");
                setting.Value = Convert.ToString(checkBoxMP2Plugins.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsMP2AllClientFolders", "true");
                setting.Value = Convert.ToString(checkBoxMP2AllClientFolders.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsMP2AllClientProgramFolders", "true");
                setting.Value = Convert.ToString(checkBoxMP2AllClientProgramFolders.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsMP2AllClientFiles", "true");
                setting.Value = Convert.ToString(checkBoxMP2AllClientFiles.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsSV2Configuration", "true");
                setting.Value = Convert.ToString(checkBoxSV2Config.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsSV2Database", "true");
                setting.Value = Convert.ToString(checkBoxSV2Database.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsSV2Plugins", "true");
                setting.Value = Convert.ToString(checkBoxSV2Plugins.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsSV2Defaults", "true");
                setting.Value = Convert.ToString(checkBoxSV2Defaults.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsSV2AllServerFolders", "true");
                setting.Value = Convert.ToString(checkBoxSV2AllServerFolders.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsSV2AllServerProgramFolders", "true");
                setting.Value = Convert.ToString(checkBoxSV2AllServerProgramFolders.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsSV2AllServerFiles", "true");
                setting.Value = Convert.ToString(checkBoxSV2AllServerFiles.Checked);
                setting.Persist();

                /*setting = layer.GetSetting("Backup_Settingsfirstuse", "true");
                 setting.Value = Convert.ToString(firstuse);
                 setting.Persist();*/

                setting = layer.GetSetting("Backup_SettingsDuplicateInteractive", "false");
                setting.Value = Convert.ToString(checkBoxduplicateinteractive.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsDuplicateAutoprocess", "true");
                setting.Value = Convert.ToString(checkBoxduplicateautoprocess.Checked);
                setting.Persist();

                setting = layer.GetSetting("Backup_Settings_Easy", "true");
                if (radioButtonEasy.Checked == true)
                    setting.Value = "true";
                else
                    setting.Value = "false";
                setting.Persist();

                setting = layer.GetSetting("Backup_Settings_Expert", "false");
                if (radioButtonExpert.Checked == true)
                    setting.Value = "true";
                else
                    setting.Value = "false";
                setting.Persist();


                //listview data
                string listviewstring = "";




                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {

                    try //active
                    {
                        listviewstring += dataGridView1[0, i].Value.ToString() + BACKUPSETTINGS_COLUMN_SEPARATOR;
                    }
                    catch
                    {
                        listviewstring += "false" + BACKUPSETTINGS_COLUMN_SEPARATOR;
                    }

                    try //name
                    {
                        listviewstring += dataGridView1[1, i].Value.ToString() + BACKUPSETTINGS_COLUMN_SEPARATOR;
                    }
                    catch
                    {
                        listviewstring += BACKUPSETTINGS_COLUMN_SEPARATOR;
                    }

                    try //folder
                    {
                        listviewstring += dataGridView1[2, i].Value.ToString() + BACKUPSETTINGS_COLUMN_SEPARATOR;
                    }
                    catch
                    {
                        listviewstring += BACKUPSETTINGS_COLUMN_SEPARATOR;
                    }

                    try //overwrite
                    {
                        listviewstring += dataGridView1[3, i].Value.ToString() + BACKUPSETTINGS_COLUMN_SEPARATOR;
                    }
                    catch
                    {
                        listviewstring += "true" + BACKUPSETTINGS_COLUMN_SEPARATOR;
                    }

                    try //kill process
                    {
                        listviewstring += dataGridView1[4, i].Value.ToString() + BACKUPSETTINGS_COLUMN_SEPARATOR;
                    }
                    catch
                    {
                        listviewstring += BACKUPSETTINGS_COLUMN_SEPARATOR;
                    }

                    try //restart process
                    {
                        listviewstring += dataGridView1[5, i].Value.ToString() + "\n";
                    }
                    catch
                    {
                        listviewstring += "\n";
                    }
                }


                if (DEBUG)
                    textoutput("listviewstring=" + listviewstring);

                save_longsetting(listviewstring, "BackupSettings_ListView");

                //Scheduler
                
                /*
                //for testing only remove later!!!!
                if (_NextBackup > DateTime.Now.AddDays(1000.0))
                {
                    _NextBackup = DateTime.Now;
                    MessageBox.Show("_NextBackup=" + _NextBackup.ToString());
                }*/

                setting = layer.GetSetting("Backup_SettingsAutomatedExportFolderName", string.Empty);
                setting.Value = textBoxAutomatedExportFolderName.Text;
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsEnableScheduler", "false");
                setting.Value = Convert.ToString(checkBoxEnableScheduler.Checked);
                setting.Persist();

                checkcombotextbox(ref comboBoxdays, "D2", 1, 1000000, "Days");
                setting = layer.GetSetting("Backup_SettingsSchedulerDays", "30");
                setting.Value = comboBoxdays.Text;
                setting.Persist();

                checkcombotextbox(ref comboBoxhours, "D2", 0, 23, "Hours");
                setting = layer.GetSetting("Backup_SettingsSchedulerHours", "06");
                setting.Value = comboBoxhours.Text;
                setting.Persist();

                checkcombotextbox(ref comboBoxminutes, "D2", 0, 59, "Minutes");
                setting = layer.GetSetting("Backup_SettingsSchedulerMinutes", "00");
                setting.Value = comboBoxminutes.Text;
                setting.Persist();

                if (comboBoxKeepNumber.Text != "All")
                    checkcombotextbox(ref comboBoxKeepNumber, "D2", 1, 1000, "Number");
                setting = layer.GetSetting("Backup_SettingsSchedulerKeepNumber", "3");                
                setting.Value = comboBoxKeepNumber.Text;
                setting.Persist();

                setting = layer.GetSetting("Backup_SettingsSchedulerWeekDays", "Any");
                setting.Value = _WeekDays;
                setting.Persist();

                //must be last entry!!!
                setting = layer.GetSetting("Backup_SettingsNextBackup", DateTime.Now.ToString("yyyy-MM-dd_HH:mm", System.Globalization.CultureInfo.InvariantCulture));
                setting.Value = _NextBackup.ToString("yyyy-MM-dd_HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                if (setting.Value != _OldNextBackup)
                {// values did change inform the tv server and write new date into file
                    textoutput("Writing File" + TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + SCHEDULED_EXPORT_FILE);
                    File.WriteAllText(TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + SCHEDULED_EXPORT_FILE, setting.Value);
                }
                setting.Persist();
                //textoutput("Debug savesettings _NextBackup = " + setting.Value+"\n");
            }
            catch (Exception ex)
            {
                textoutput("<RED>Backup_SettingsSetup - MySaveSettings(): " + ex.Message);
            }
        }

        public bool save_longsetting(string mystring, string mysetting)
        {
            TvBusinessLayer layer = new TvBusinessLayer();
            Setting setting;
            int STRINGLIMIT = 4000; //is 4096  string limit for settings - split if larger

            //cleanup work
            for (int i = 1; i < 1000; i++)
            {
                setting = layer.GetSetting(mysetting + "_" + i.ToString("D3"), "_DOES_NOT_EXIST_");
                if (setting.Value == "_DOES_NOT_EXIST_")
                {
                    setting.Remove();
                    break;
                }
                else
                {
                    setting.Remove();
                }
            }


            // split string if too large  !!! Limit of 4096 characters in tv server

            try
            {
                if (mystring.Length > STRINGLIMIT)
                {

                    string partial_string = mystring.Substring(0, STRINGLIMIT);
                    setting = layer.GetSetting(mysetting, "");
                    setting.Value = partial_string;
                    //Log.Debug("partial string  " + mysetting + "  =" + partial_string);
                    setting.Persist();
                    int ctr = 1;
                    while (ctr * STRINGLIMIT <= mystring.Length)
                    {
                        if ((ctr + 1) * STRINGLIMIT < mystring.Length)
                        {
                            partial_string = mystring.Substring(ctr * STRINGLIMIT, STRINGLIMIT);
                            setting = layer.GetSetting(mysetting + "_" + ctr.ToString("D3"), "");
                            setting.Value = partial_string;
                            //Log.Debug("partial string  " + mysetting + "_" + ctr.ToString("D3") + "  =" + partial_string);
                            setting.Persist();

                        }
                        else
                        {
                            partial_string = mystring.Substring(ctr * STRINGLIMIT, mystring.Length - ctr * STRINGLIMIT);
                            setting = layer.GetSetting(mysetting + "_" + ctr.ToString("D3"), "");
                            setting.Value = partial_string;
                            //Log.Debug("partial listviewstring  " + mysetting + "_" + ctr.ToString("D3") + "  =" + partial_string);
                            setting.Persist();
                            ctr++;
                            setting = layer.GetSetting(mysetting + "_" + ctr.ToString("D3"), "");
                            setting.Value = "";
                            setting.Persist();

                        }
                        ctr++;

                        if (ctr > 999)
                        {
                            Log.Error("!!!!!!!!!!!!!!!!!!!! Fatal Error: Too many data entries - skipping data");
                            break;
                        }
                    }

                }
                else //do not split string - small enough
                {
                    setting = layer.GetSetting(mysetting, "");
                    setting.Value = mystring;
                    setting.Persist();
                    int ctr = 1;
                    //Log.Debug("string  " + mysetting + "=" + mystring);
                    setting = layer.GetSetting(mysetting + "_" + ctr.ToString("D3"), ""); //needed for detecting in loadlongsettings
                    setting.Value = "";
                    setting.Persist();

                }



                return true;
            }
            catch (Exception exc)
            {
                Log.Error("Adding long setting failed with message \n" + exc.Message);
                return false;
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
                //Log.Debug("partial " + name + "_" + count.ToString("D3") + "=" + partial);
            }

            return stringdata;
        }


        private void UpdateGUI()
        {
            if ((checkBoxTV.Checked == true) && ((TV_PROGRAM_FOLDER == "NOT_DEFINED") || (TV_USER_FOLDER == "NOT_DEFINED")))
            {
                MessageBox.Show("Unchecking TV Server because paths are not correctly defined\nCheck the path configuration first", "Error");
                checkBoxTV.Checked = false;
            }

            if ((checkBoxMP.Checked == true) && ((MP_PROGRAM_FOLDER == "NOT_DEFINED") || (MP_USER_FOLDER == "NOT_DEFINED")))
            {
                MessageBox.Show("Unchecking MediaPortal1 because paths are not correctly defined\nCheck the path configuration first", "Error");
                checkBoxMP.Checked = false;
            }

            if ((checkBoxMP2C.Checked == true) && ((MP2_PROGRAM_FOLDER == "NOT_DEFINED") || (MP2_USER_FOLDER == "NOT_DEFINED")))
            {
                MessageBox.Show("Unchecking MediaPortal2 Client because paths are not correctly defined\nCheck the path configuration first", "Error");
                checkBoxMP2C.Checked = false;
            }

            if ((checkBoxMP2S.Checked == true) && ((SV2_PROGRAM_FOLDER == "NOT_DEFINED") || (SV2_USER_FOLDER == "NOT_DEFINED")))
            {
                MessageBox.Show("Unchecking MediaPortal2 Server because paths are not correctly defined\nCheck the path configuration first", "Error");
                checkBoxMP2S.Checked = false;
            }

            if (radioButtonEasy.Checked == true)
            {
                if (tabControl1.TabPages.Contains(tabPage2))
                    tabControl1.TabPages.Remove(tabPage2);
                if (tabControl1.TabPages.Contains(tabPage3))
                    tabControl1.TabPages.Remove(tabPage3);
                if (tabControl1.TabPages.Contains(tabPage7))
                    tabControl1.TabPages.Remove(tabPage7);
                if (tabControl1.TabPages.Contains(tabPage4))
                    tabControl1.TabPages.Remove(tabPage4);
                if (tabControl1.TabPages.Contains(tabPage5))
                    tabControl1.TabPages.Remove(tabPage5);
                if (tabControl1.TabPages.Contains(tabPage6))
                    tabControl1.TabPages.Remove(tabPage6);
                if (tabControl1.TabPages.Contains(tabPage10))
                    tabControl1.TabPages.Remove(tabPage10);


                if (!tabControl1.TabPages.Contains(tabPage10))
                    tabControl1.TabPages.Add(tabPage10);

                checkBoxMP.Hide();
                checkBoxTV.Hide();
                checkBoxMP2C.Hide();
                checkBoxMP2S.Hide();
            }
            else
            {
                if (tabControl1.TabPages.Contains(tabPage2))
                    tabControl1.TabPages.Remove(tabPage2);
                if (tabControl1.TabPages.Contains(tabPage3))
                    tabControl1.TabPages.Remove(tabPage3);
                if (tabControl1.TabPages.Contains(tabPage7))
                    tabControl1.TabPages.Remove(tabPage7);
                if (tabControl1.TabPages.Contains(tabPage4))
                    tabControl1.TabPages.Remove(tabPage4);
                if (tabControl1.TabPages.Contains(tabPage5))
                    tabControl1.TabPages.Remove(tabPage5);
                if (tabControl1.TabPages.Contains(tabPage6))
                    tabControl1.TabPages.Remove(tabPage6);
                if (tabControl1.TabPages.Contains(tabPage10))
                    tabControl1.TabPages.Remove(tabPage10);


                if (!tabControl1.TabPages.Contains(tabPage2) && ((textBoxTV1P.Text != "NOT_DEFINED") && (textBoxTV1U.Text != "NOT_DEFINED")))  //TV
                    tabControl1.TabPages.Add(tabPage2);


                if (!tabControl1.TabPages.Contains(tabPage3) && ((textBoxMP1P.Text != "NOT_DEFINED") && (textBoxMP1U.Text != "NOT_DEFINED")))//MP1
                    tabControl1.TabPages.Add(tabPage3);


                if (!tabControl1.TabPages.Contains(tabPage7) && (((textBoxMP2P.Text != "NOT_DEFINED") && (textBoxMP2U.Text != "NOT_DEFINED")) || ((textBoxSV2P.Text != "NOT_DEFINED") && (textBoxSV2U.Text != "NOT_DEFINED"))))//MP2
                    tabControl1.TabPages.Add(tabPage7);


                if (!tabControl1.TabPages.Contains(tabPage4))
                    tabControl1.TabPages.Add(tabPage4);
                if (!tabControl1.TabPages.Contains(tabPage5))
                    tabControl1.TabPages.Add(tabPage5);
                if (!tabControl1.TabPages.Contains(tabPage6))
                    tabControl1.TabPages.Add(tabPage6);
                if (!tabControl1.TabPages.Contains(tabPage10))
                    tabControl1.TabPages.Add(tabPage10);


                if ((textBoxMP1P.Text != "NOT_DEFINED") && (textBoxMP1U.Text != "NOT_DEFINED"))
                    checkBoxMP.Show();
                else
                    checkBoxMP.Hide();

                if ((textBoxTV1P.Text != "NOT_DEFINED") && (textBoxTV1U.Text != "NOT_DEFINED"))
                    checkBoxTV.Show();
                else
                    checkBoxTV.Hide();

                if ((textBoxMP2P.Text != "NOT_DEFINED") && (textBoxMP2U.Text != "NOT_DEFINED"))
                    checkBoxMP2C.Show();
                else
                    checkBoxMP2C.Hide();

                if ((textBoxSV2P.Text != "NOT_DEFINED") && (textBoxSV2U.Text != "NOT_DEFINED"))
                    checkBoxMP2S.Show();
                else
                    checkBoxMP2S.Hide();
            }

        }



        # region GUI processing
        private void selectfilenamebutton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog selectfolder = new FolderBrowserDialog();
            selectfolder.Description = "Select Import/Export Folder";
            selectfolder.SelectedPath = filenametextBox.Text;
            if (selectfolder.ShowDialog(this) == DialogResult.OK)
                filenametextBox.Text = selectfolder.SelectedPath;
        }


        private void importbutton_Click(object sender, EventArgs e)
        {
            if (BUSY == true)
            {
                MessageBox.Show("Processing ongoing - please wait for completion", "Warning");
                return;
            }
            BUSY = true;

            MySaveSettings(); //does save actual paths and settings
            MyLoadSettings(); //does update paths

            th = new System.Threading.Thread(importthread);
            th.Start();
        }
        

        private void importthread()
        {

            BackupSettingsExportImport newimport = new BackupSettingsExportImport();

            if (newimport != null)
            {
                newimport.ExportImportInit(ref progressBar, false);
                newimport.newmessage += new textexportmessage(textoutput);

                newimport.MyLoadSettings();
                newimport.getallversionnumbers("", false); //get only program version numbers after loadsettings
                newimport.Status = Status;
                bool ok = newimport.restorebackup(filenametextBox.Text);
                if (ok)
                    MessageBox.Show(this, "Import completed successfully");
                else
                {
                    if (PB_th != null)
                    {
                        newimport.ProgressbarCancel();
                    }
                    MessageBox.Show(this, "Import not successful - Check the status and the log file");
                }
                newimport.MySaveSettings();
                newimport.newmessage -= new textexportmessage(textoutput);
            }

            textoutput("\n");  //create new paragraph after completed export
            BUSY = false; //job completed
        }


        private void exportbutton_Click(object sender, EventArgs e)
        {
            if (BUSY == true)
            {
                MessageBox.Show("Processing ongoing - please wait for completion","Warning");
                return;
            }
            BUSY = true;
            MySaveSettings(); //does save actual paths and settings
            MyLoadSettings(); //does update paths
            th = new System.Threading.Thread(exportthread);
            th.Start();
        }

        private void exportthread()
        {
            
            BackupSettingsExportImport newexport = new BackupSettingsExportImport();

            if (newexport != null)
            {
                newexport.ExportImportInit(ref progressBar, false);
                newexport.newmessage += new textexportmessage(textoutput);
                
                newexport.MyLoadSettings();
                newexport.getallversionnumbers("", false); //get only program version numbers after loadsettings
                bool ok = newexport.createbackup(filenametextBox.Text);
                if (ok)
                    MessageBox.Show(this, "Export completed successfully");
                else
                {
                    if (PB_th != null)
                    {
                        newexport.ProgressbarCancel();
                    }
                    MessageBox.Show(this, "Export not successful - Check the status and the log file");
                }
                newexport.MySaveSettings();
                newexport.newmessage -= new textexportmessage(textoutput);
            }     

            textoutput("\n");  //create new paragraph after completed export
            BUSY = false; //job completed
        }

        private void clearbutton_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            Status.Close();  //  close status file
            File.Delete(TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + STATUS_FILE);// delete status file
            //open file in append mode for new status
            try
            {
                Status = File.AppendText(TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + STATUS_FILE);
            }
            catch (Exception exc)
            {
                Log.Debug("Fatal Error: Could not open file in append mode for " + TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + STATUS_FILE + " - Exception:" + exc.Message);
            }
            newexportimport.ProgressbarInit();
            
        }

        private void Debug(string attribute, string value)
        {
            if (DEBUG)
            {
                Log.Debug("BackupSettings: " + attribute );
                //Log.Debug("BackupSettings: "+ attribute + " = " + value);
            }
        }


        private void textoutput(string textlines)
        {
            Log.Debug("BackupSettingsSetup: " + textlines);




            string text = "";

            if (listView1 != null)
            {
                Color mycolor;
                string color_string = "";
                if (textlines.StartsWith("<RED>") == true)
                {//color red
                    textlines = textlines.Substring(5, textlines.Length - 5);
                    mycolor = Color.Red;
                    color_string = "<RED>";
                }
                else if (textlines.StartsWith("<YELLOW>") == true)
                {//color yellow
                    textlines = textlines.Substring(8, textlines.Length - 8);
                    mycolor = Color.Orange;
                    color_string = "<YELLOW>";
                }
                else if (textlines.StartsWith("<GREEN>") == true)
                {//color green
                    textlines = textlines.Substring(8, textlines.Length - 8);
                    mycolor = Color.Green;
                    color_string = "<GREEN>";
                }
                else
                {//color black
                    mycolor = Color.Black;
                    color_string = "";
                }

                char[] splitterchars = { '\n' };  //split lines with \n
                string[] lines = textlines.Split(splitterchars);
                foreach (string line in lines)
                {
                    try
                    {
                        Status.WriteLine(color_string + line);
                    }
                    catch //do nothing if stream writer is not open
                    {
                    }


                    text = line;
                    while (text.Length > 70)   //split long lines
                    {
                        int linelength = 69;
                        for (int i = 69; i >= 4; i--)
                        {
                            if (text[i] == ' ')
                            {
                                linelength = i;
                                break;
                            }
                        }
                        string pretext = text.Substring(0, linelength);

                        listView1.Items.Add(pretext);
                        listView1.Items[listView1.Items.Count - 1].ForeColor = mycolor;
                        listView1.Items[listView1.Items.Count - 1].EnsureVisible();



                        text = "+  " + text.Substring(linelength, text.Length - linelength);

                    }

                    listView1.Items.Add(text);
                    listView1.Items[listView1.Items.Count - 1].ForeColor = mycolor;
                    listView1.Items[listView1.Items.Count - 1].EnsureVisible();

                }

            }
        }



        private void DebugCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxDebugBackupSettings.Checked == true)
            {
                DEBUG = true;
            }
            else
            {
                DEBUG = false;
            }

        }

        private void buttonMpAll_Click(object sender, EventArgs e)
        {
            //MP all
            checkBoxMPDatabase.Checked = true;
            checkBoxMPInputDevice.Checked = true;
            checkBoxMPPlugins.Checked = true;
            checkBoxMPProgramXml.Checked = true;
            checkBoxMPSkins.Checked = true;
            checkBoxMPThumbs.Checked = true;
            checkBoxMPUserXML.Checked = true;
            checkBoxMPxmltv.Checked = true;
            checkBoxMPMusicPlayer.Checked = true;
            checkBoxMPDeleteCache.Checked = true;
            checkBoxMPAllFolders.Checked = true;
            checkBoxMPAllProgram.Checked = true;
        }

        private void buttonMpNone_Click(object sender, EventArgs e)
        {
            //MP None
            checkBoxMPDatabase.Checked = false;
            checkBoxMPInputDevice.Checked = false;
            checkBoxMPPlugins.Checked = false;
            checkBoxMPProgramXml.Checked = false;
            checkBoxMPSkins.Checked = false;
            checkBoxMPThumbs.Checked = false;
            checkBoxMPUserXML.Checked = false;
            checkBoxMPxmltv.Checked = false;
            checkBoxMPMusicPlayer.Checked = false;
            checkBoxMPDeleteCache.Checked = false;
            checkBoxMPAllFolders.Checked = false;
            checkBoxMPAllProgram.Checked = false;
       }

        private void buttonTvNone_Click(object sender, EventArgs e)
        {
            //TV None
            checkBoxServer.Checked = false;  //do not change order
            checkBoxDeleteChannels.Checked = false;  //do not change order
            checkBoxDeleteRadioGroups.Checked = false;
            checkBoxDeleteSchedules.Checked = false;
            checkBoxDeleteRecordings.Checked = false;
            checkBoxDeleteTVGroups.Checked = false;                        
            checkBoxChannels.Checked = false;
            checkBoxChannelCardMapping.Checked = false;
            checkBoxChannelGroups.Checked = false;
            checkBoxRadioGroups.Checked = false;
            checkBoxSchedules.Checked = false;
            checkBoxEPG.Checked = false;
            checkBoxRecordings.Checked = false;
            checkBoxAllSettings.Checked = false;
            checkBoxClickfinder.Checked = false;
            checkBoxTVServer.Checked = false;
            checkBoxTVallPrograms.Checked = false;
            checkBoxTVallFolders.Checked = false;
            checkBoxtvrestart.Checked = false;
            checkBoxAutoCorrectDataBase.Checked = false;
        }

        private void buttonTvAll_Click(object sender, EventArgs e)
        { //TV All
            checkBoxDeleteChannels.Checked = true;
            checkBoxDeleteRadioGroups.Checked = true;
            checkBoxDeleteSchedules.Checked = true;
            checkBoxDeleteRecordings.Checked = true;
            checkBoxDeleteTVGroups.Checked = true;
            checkBoxServer.Checked = true;
            checkBoxChannels.Checked = true;
            checkBoxChannelCardMapping.Checked = true;
            checkBoxChannelGroups.Checked = true;
            checkBoxRadioGroups.Checked = true;
            checkBoxSchedules.Checked = true;
            checkBoxEPG.Checked = true;
            checkBoxRecordings.Checked = true;
            checkBoxAllSettings.Checked = true;
            checkBoxClickfinder.Checked = true;
            checkBoxTVServer.Checked = true;
            checkBoxTVallPrograms.Checked = true;
            checkBoxTVallFolders.Checked = true;
            checkBoxtvrestart.Checked = true;
            checkBoxAutoCorrectDataBase.Checked = true;
        }

        private void buttonDefault_Click(object sender, EventArgs e)
        {// default button restore all default settings
            //TV and MP
            if ((textBoxTV1P.Text != "NOT_DEFINED") &&(textBoxTV1U.Text != "NOT_DEFINED"))
                checkBoxTV.Checked = true;

            if ((textBoxMP1P.Text != "NOT_DEFINED") && (textBoxMP1U.Text != "NOT_DEFINED"))
                checkBoxMP.Checked = true;

            if ((textBoxMP2P.Text != "NOT_DEFINED") && (textBoxMP2U.Text != "NOT_DEFINED"))
                checkBoxMP2C.Checked = true;

            if ((textBoxSV2P.Text != "NOT_DEFINED") && (textBoxSV2U.Text != "NOT_DEFINED"))
                checkBoxMP2S.Checked = true;

            UpdateGUI();

            checkBoxUseAutoDate.Checked = true;

            //TV default
            
            checkBoxDeleteRadioGroups.Checked = true;
            checkBoxDeleteSchedules.Checked = true;
            checkBoxDeleteRecordings.Checked = true;
            checkBoxDeleteTVGroups.Checked = true;
            checkBoxDeleteChannels.Checked = true;  
            checkBoxServer.Checked = true;  
            checkBoxChannels.Checked = true;
            checkBoxChannelCardMapping.Checked = true;
            checkBoxChannelGroups.Checked = true;
            checkBoxRadioGroups.Checked = true;
            checkBoxSchedules.Checked = true;
            checkBoxEPG.Checked = false;
            checkBoxRecordings.Checked = true;
            checkBoxAllSettings.Checked = true;
            checkBoxClickfinder.Checked = true;
            checkBoxTVServer.Checked = true;
            checkBoxTVallPrograms.Checked = true;
            checkBoxTVallFolders.Checked = true;
            checkBoxtvrestart.Checked = true;
            checkBoxAutoCorrectDataBase.Checked = true;

            //MP default
            checkBoxMPDatabase.Checked = true;
            checkBoxMPInputDevice.Checked = true;
            checkBoxMPPlugins.Checked = true;
            checkBoxMPProgramXml.Checked = true;
            checkBoxMPSkins.Checked = true;
            checkBoxMPThumbs.Checked = true;
            checkBoxMPUserXML.Checked = true;
            checkBoxMPxmltv.Checked = true;
            checkBoxMPMusicPlayer.Checked = true;
            checkBoxMPDeleteCache.Checked = true;
            checkBoxMPAllFolders.Checked = true;
            checkBoxMPAllProgram.Checked = true;

            //MP2 default
            checkBoxMP2Config.Checked = true;
            checkBoxMP2Plugins.Checked = true;
            checkBoxMP2Defaults.Checked = true;
            checkBoxMP2AllClientProgramFolders.Checked = true;
            checkBoxMP2AllClientFolders.Checked = true;
            checkBoxMP2AllClientFiles.Checked = true;

            checkBoxSV2Config.Checked = true;
            checkBoxSV2Database.Checked = true;
            checkBoxSV2Plugins.Checked = true;
            checkBoxSV2Defaults.Checked = true;
            checkBoxSV2AllServerProgramFolders.Checked = true;
            checkBoxSV2AllServerFolders.Checked = true;
            checkBoxSV2AllServerFiles.Checked = true;
           
            //duplicate channels
            checkBoxduplicateautoprocess.Checked = false;
            checkBoxduplicateinteractive.Checked = false;

            //scheduler
            checkBoxEnableScheduler.Checked = false;
            _WeekDays = "";
            checkBoxAny.Checked = true;
            checkBoxMon.Checked = false;
            checkBoxTue.Checked = false;
            checkBoxWed.Checked = false;
            checkBoxThur.Checked = false;
            checkBoxFri.Checked = false;
            checkBoxSat.Checked = false;
            checkBoxSun.Checked = false;
            comboBoxdays.Text = "30";
            comboBoxhours.Text = "06";
            comboBoxminutes.Text = "00";
            comboBoxKeepNumber.Text = "3";
            
        }


       

        private void buttonCheck_Click(object sender, EventArgs e)
        {
            if (BUSY == true)
            {
                MessageBox.Show("Processing ongoing - please wait for completion", "Warning");
                return;
            }
            BUSY = true;

            MySaveSettings(); //does save actual paths and settings
            MyLoadSettings(); //does update paths
            th = new System.Threading.Thread(duplicatecheckthread);
            th.Start();
        }

        private void duplicatecheckthread()
        {
            newexportimport.MyLoadSettings();
            newexportimport.checkduplicatechannels(true, true);       //verbose=true messagebox true
            newexportimport.CheckDuplicateGroupmembers(false, true, true);  //process=false  verbose=true messagebox true
            textoutput("\n");  //create new paragraph after completed import
            newexportimport.MySaveSettings();
            BUSY = false;
        }

        private void buttonprocess_Click(object sender, EventArgs e)
        {
            if (BUSY == true)
            {
                MessageBox.Show("Processing ongoing - please wait for completion", "Warning");
                return;
            }           
            BUSY = true;

            MySaveSettings(); //does save actual paths and settings
            MyLoadSettings(); //does update paths
            th = new System.Threading.Thread(duplicateprocessthread);
            th.Start();
        }

        private void duplicateprocessthread()
        {
            newexportimport.MyLoadSettings();
            newexportimport.processduplicates(true); //messagebox true
            newexportimport.CheckDuplicateGroupmembers(true, true, true); //process true verbose true messagebox false
            textoutput("\n");  //create new paragraph after completed import
            newexportimport.MySaveSettings();
            BUSY = false;
        }

        private void buttonundo_Click(object sender, EventArgs e)
        {
            if (BUSY == true)
            {
                MessageBox.Show("Processing ongoing - please wait for completion", "Warning");
                return;
            }
            BUSY = true;

            MySaveSettings(); //does save actual paths and settings
            MyLoadSettings(); //does update paths
            th = new System.Threading.Thread(undorenamethread);
            th.Start();
        }

        private void undorenamethread()
        {
            newexportimport.MyLoadSettings();
            newexportimport.undorename();
            newexportimport.MySaveSettings();
            textoutput("\n");  //create new paragraph after completed import
            BUSY = false;
        }



        private void buttonDelete_Click(object sender, EventArgs e)
        {
            //Delete
            try
            {
                dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
            }
            catch
            {
                //textoutput("Fatal Error: Faileed to delete dataview current row ");
                //textoutput("SELECTED_COL=" + SELECTED_COL.ToString() + "    " + "SELECTED_ROW=" + SELECTED_ROW.ToString());
            }
        }

        private void buttonSelectFolder_Click(object sender, EventArgs e)
        {

            SELECTED_COL = dataGridView1.CurrentCell.ColumnIndex;
            SELECTED_ROW = dataGridView1.CurrentCell.RowIndex;
            //textoutput("SELECTED_COL =" + SELECTED_COL.ToString());
            //textoutput("SELECTED_ROW =" + SELECTED_ROW.ToString());
            //textoutput("rowcount=" + dataGridView1.RowCount.ToString());

            if ((SELECTED_COL == 2) && (SELECTED_ROW < dataGridView1.RowCount - 1) && (SELECTED_ROW >= 0))
            {
                FolderBrowserDialog selectfolder = new FolderBrowserDialog();
                selectfolder.Description = "Select Extra Folder for Backup / Restore";
                try
                {
                    selectfolder.SelectedPath = dataGridView1[SELECTED_COL, SELECTED_ROW].Value.ToString();
                }
                catch
                {
                    selectfolder.SelectedPath = "";
                }
                if (selectfolder.ShowDialog(this) == DialogResult.OK)
                    dataGridView1[SELECTED_COL, SELECTED_ROW].Value = selectfolder.SelectedPath;
            }
            else if ((SELECTED_COL == 5) && (SELECTED_ROW < dataGridView1.RowCount - 1) && (SELECTED_ROW >= 0))
            {
                OpenFileDialog selectfolder = new OpenFileDialog();
                selectfolder.Title = "Select application to restart";
                try
                {
                    selectfolder.FileName = dataGridView1[SELECTED_COL, SELECTED_ROW].Value.ToString();
                }
                catch
                {
                    selectfolder.FileName = "";
                }
                if (selectfolder.ShowDialog(this) == DialogResult.OK)
                    dataGridView1[SELECTED_COL, SELECTED_ROW].Value = selectfolder.FileName;
            }
            else
            {
                MessageBox.Show("You need to select a cell in the Folder or Restart column", "Invalid Column Selected");
            }

        }

        private void buttonActiveAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                dataGridView1[0, i].Value = true;
            }
        }

        private void buttonActiveNone_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                dataGridView1[0, i].Value = false;
            }
        }

        private void checkBoxServer_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxDeleteChannels.Checked = true;
            checkBoxDeleteTVGroups.Checked = true;
            checkBoxDeleteRadioGroups.Checked = true;
            checkBoxDeleteRecordings.Checked = true;
            checkBoxDeleteSchedules.Checked = true;
        }

        private void checkBoxDeleteChannels_CheckedChanged(object sender, EventArgs e)
        {
            if ((checkBoxDeleteChannels.Checked == false) && (checkBoxServer.Checked == true))
            {
                checkBoxDeleteChannels.Checked = true;
                MessageBox.Show("If you import new server/card settings you must also delete all channels, Tv groups, radio groups, recordings and schedules ", "Info");
            }
            checkBoxDeleteTVGroups.Checked = true;
            checkBoxDeleteRadioGroups.Checked = true;
            checkBoxDeleteRecordings.Checked = true;
            checkBoxDeleteSchedules.Checked = true;                   
        }
       
        private void checkBoxDeleteTVGroups_CheckedChanged(object sender, EventArgs e)
        {
            if ((checkBoxDeleteTVGroups.Checked == false)&&((checkBoxDeleteChannels.Checked == true)||(checkBoxServer.Checked == true)))
            {
                checkBoxDeleteTVGroups.Checked=true;
                MessageBox.Show("If you delete all channels or you import new server/card settings you must also delete all TV groups ", "Info");              
            }
        }

        private void checkBoxDeleteRadioGroups_CheckedChanged(object sender, EventArgs e)
        {
            if ((checkBoxDeleteRadioGroups.Checked == false) && ((checkBoxDeleteChannels.Checked == true) || (checkBoxServer.Checked == true)))
            {
                checkBoxDeleteRadioGroups.Checked = true;
                MessageBox.Show("If you delete all channels or you import new server/card settings you must also delete all radio groups ", "Info");
            }
        }

        private void checkBoxDeleteSchedules_CheckedChanged(object sender, EventArgs e)
        {
            if ((checkBoxDeleteSchedules.Checked == false) && ((checkBoxDeleteChannels.Checked == true) || (checkBoxServer.Checked == true)))
            {
                checkBoxDeleteSchedules.Checked = true;
                MessageBox.Show("If you delete all channels or you import new card settings you must also delete all schedules ", "Info");
            }
        }

        private void checkBoxDeleteRecordings_CheckedChanged(object sender, EventArgs e)
        {
            if ((checkBoxDeleteRecordings.Checked == false) && ((checkBoxDeleteChannels.Checked == true) || (checkBoxServer.Checked == true)))
            {
                checkBoxDeleteRecordings.Checked = true;
                MessageBox.Show("If you delete all channels or you import new server/card settings you must also delete all recordings ", "Info");
            }
        }


        private void buttonhelp_Click(object sender, EventArgs e)
        {
            Process proc = new Process();
            ProcessStartInfo procstartinfo = new ProcessStartInfo();
            procstartinfo.FileName = TV_PROGRAM_FOLDER+@"\"+BACKUPSETTING_TV_DIR+@"\BackupSettings.pdf";
            proc.StartInfo = procstartinfo;
            try
            {
                proc.Start();
            }
            catch
            {
                MessageBox.Show("Could not open " + procstartinfo.WorkingDirectory + "\\" + procstartinfo.FileName, "Error");
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start("http://forum.team-mediaportal.com/mediaportal-plugins-47/backupsettings-tv-server-media-portal-45715/");
            }
            catch (Exception exc)
            {
                textoutput("Internet link failed" );
                Log.Error("Internet link failed with exception: " + exc.Message);
            }

        }






        private void buttonCreateAutoFolder_Click(object sender, EventArgs e)
        {
            //create automated foldername
            MySaveSettings();
            newexportimport.MyLoadSettings();
            filenametextBox.Text = newexportimport.CreateAutomatedFolderName(filenametextBox.Text, WhoAmI.Tv_Server);
        }        

        private void radioButtonEasy_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonEasy.Checked == true)
            {
                //EasyMode();
                UpdateGUI();
            }
            
        }

        private void radioButtonExpert_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonExpert.Checked == true)
            {
                //AdvancedMode();
                UpdateGUI();
            }
        }


        private void buttonMP1P_Click(object sender, EventArgs e)
        {
            instpaths.LOG = string.Empty;
            textBoxMP1P.Text = instpaths.ask_MP_PROGRAM_ALWAYS();
            if (instpaths.LOG != string.Empty)
            {
                MessageBox.Show(instpaths.LOG);
                instpaths.LOG = string.Empty;
            }
        }

        private void buttonMP1U_Click(object sender, EventArgs e)
        {
            instpaths.LOG = string.Empty;
            textBoxMP1U.Text = instpaths.ask_MP_USER_ALWAYS();
            if (instpaths.LOG != string.Empty)
            {
                MessageBox.Show(instpaths.LOG);
                instpaths.LOG = string.Empty;
            }
        }

        private void buttonTV1P_Click(object sender, EventArgs e)
        {
            instpaths.LOG = string.Empty;
            textBoxTV1P.Text = instpaths.ask_TV_PROGRAM_ALWAYS();
            if (instpaths.LOG != string.Empty)
            {
                MessageBox.Show(instpaths.LOG);
                instpaths.LOG = string.Empty;
            }
        }

        private void buttonTV1U_Click(object sender, EventArgs e)
        {
            instpaths.LOG = string.Empty;
            textBoxTV1U.Text = instpaths.ask_TV_USER_ALWAYS();
            if (instpaths.LOG != string.Empty)
            {
                MessageBox.Show(instpaths.LOG);
                instpaths.LOG = string.Empty;
            }  
        }

        private void buttonMP2P_Click(object sender, EventArgs e)
        {
            instpaths.LOG = string.Empty;
            textBoxMP2P.Text = instpaths.ask_MP2_PROGRAM_ALWAYS();
            if (instpaths.LOG != string.Empty)
            {
                MessageBox.Show(instpaths.LOG);
                instpaths.LOG = string.Empty;
            }
        }

        private void buttonMP2U_Click(object sender, EventArgs e)
        {
            instpaths.LOG = string.Empty;
            textBoxMP2U.Text = instpaths.ask_MP2_USER_ALWAYS();
            if (instpaths.LOG != string.Empty)
            {
                MessageBox.Show(instpaths.LOG);
                instpaths.LOG = string.Empty;
            }
        }

        private void buttonSV2P_Click(object sender, EventArgs e)
        {
            instpaths.LOG = string.Empty;
            textBoxSV2P.Text = instpaths.ask_SV2_PROGRAM_ALWAYS();
            if (instpaths.LOG != string.Empty)
            {
                MessageBox.Show(instpaths.LOG);
                instpaths.LOG = string.Empty;
            }
        }

        private void buttonSV2U_Click(object sender, EventArgs e)
        {
            instpaths.LOG = string.Empty;
            textBoxSV2U.Text = instpaths.ask_SV2_USER_ALWAYS();
            if (instpaths.LOG != string.Empty)
            {
                MessageBox.Show(instpaths.LOG);
                instpaths.LOG = string.Empty;
            }  
        }

        private void buttonDetect_Click(object sender, EventArgs e)
        {
            instpaths.LOG = "";
            instpaths.GetInstallPaths();
            if (DEBUG)
                textoutput(instpaths.LOG);

            textBoxMP1P.Text = instpaths.MP_PROGRAM_FOLDER;
            textBoxMP1U.Text = instpaths.MP_USER_FOLDER;
            textBoxTV1P.Text = instpaths.TV_PROGRAM_FOLDER;
            textBoxTV1U.Text = instpaths.TV_USER_FOLDER;

            instpaths.LOG = "";
            instpaths.GetInstallPathsMP2();
            if (DEBUG)
                textoutput(instpaths.LOG);

            textBoxMP2P.Text = instpaths.MP2_PROGRAM_FOLDER;
            textBoxMP2U.Text = instpaths.MP2_USER_FOLDER;
            textBoxSV2P.Text = instpaths.SV2_PROGRAM_FOLDER;
            textBoxSV2U.Text = instpaths.SV2_USER_FOLDER;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            MySaveSettings();
            MyLoadSettings();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            MyLoadSettings();
        }

        private void buttonMP2All_Click(object sender, EventArgs e)
        {
            checkBoxMP2AllClientFiles.Checked = true;
            checkBoxMP2AllClientFolders.Checked = true;
            checkBoxMP2Config.Checked = true;
            checkBoxMP2Defaults.Checked = true;
            checkBoxMP2Plugins.Checked = true;
            checkBoxMP2AllClientProgramFolders.Checked = true;

            checkBoxSV2AllServerFiles.Checked = true;
            checkBoxSV2AllServerFolders.Checked = true;
            checkBoxSV2AllServerProgramFolders.Checked = true;
            checkBoxSV2Config.Checked = true;
            checkBoxSV2Database.Checked = true;
            checkBoxSV2Defaults.Checked = true;
            checkBoxSV2Plugins.Checked = true;            
        }

        private void buttonMP2None_Click(object sender, EventArgs e)
        {
            checkBoxMP2AllClientFiles.Checked = false;
            checkBoxMP2AllClientFolders.Checked = false;
            checkBoxMP2Config.Checked = false;
            checkBoxMP2Defaults.Checked = false;
            checkBoxMP2Plugins.Checked = false;
            checkBoxMP2AllClientProgramFolders.Checked = false;

            checkBoxSV2AllServerFiles.Checked = false;
            checkBoxSV2AllServerFolders.Checked = false;
            checkBoxSV2AllServerProgramFolders.Checked = false;
            checkBoxSV2Config.Checked = false;
            checkBoxSV2Database.Checked = false;
            checkBoxSV2Defaults.Checked = false;
            checkBoxSV2Plugins.Checked = false;
        }

        #endregion

        #region Scheduler  
        private void CalculateNextScheduleTime()
        {
            if (!checkBoxEnableScheduler.Checked)
                return;

            double add_days = 30.0;
            try
            {
                add_days = Convert.ToDouble(comboBoxdays.Text);
            }
            catch
            {
                textoutput("Error: Could not convert days into double value - using default of 30 days");
            }
            _NextBackup = _LastBackup.AddDays(add_days);


            double hours = 6.0;
            try
            {
                hours = Convert.ToDouble(comboBoxhours.Text);
            }
            catch
            {
                textoutput("Error: Could not convert hours into double value - using default of 06");
            }
            _NextBackup = _NextBackup.AddHours(hours - _NextBackup.Hour);

            double minutes = 0.0;
            try
            {
                minutes = Convert.ToDouble(comboBoxminutes.Text);
            }
            catch
            {
                textoutput("Error: Could not convert minutess into double value - using default of 00");
            }
            _NextBackup = _NextBackup.AddMinutes(minutes - _NextBackup.Minute);

            if (_WeekDays.Contains("Any") == false)
            { //process days aqnd search for next available day
                bool found = false;
                for (int i = 1; i <= 7; i++)
                {
                    int weekday = (int)_NextBackup.DayOfWeek;

                    /*
                    textoutput("weekday="+_NextBackup.DayOfWeek.ToString());
                    textoutput("int weekday=" + ((int)_NextBackup.DayOfWeek).ToString());
                    textoutput("i=" +i.ToString());
                    textoutput("int Sunday=" + ((int)DayOfWeek.Sunday).ToString());
                    textoutput("int Monday=" + ((int)DayOfWeek.Monday).ToString());
                    */

                    if (weekday == (int)DayOfWeek.Sunday)
                    {
                        if (checkBoxSun.Checked)
                        {
                            found=true;
                            break;
                        }
                        else
                        {
                            _NextBackup = _NextBackup.AddDays(1.0); //try next day
                            continue;
                        }
                    }
                    else if (weekday == (int)DayOfWeek.Monday)
                    {
                        if (checkBoxMon.Checked)
                        {
                            found=true;
                            break;
                        }
                        else
                        {
                            _NextBackup = _NextBackup.AddDays(1.0); //try next day
                            continue;
                        }
                    }
                    else if (weekday == (int)DayOfWeek.Tuesday)
                    {
                        if (checkBoxTue.Checked)
                        {
                            found=true;
                            break;
                        }
                        else
                        {
                            _NextBackup = _NextBackup.AddDays(1.0); //try next day
                            continue;
                        }
                    }
                    else if (weekday == (int)DayOfWeek.Wednesday)
                    {
                        if (checkBoxWed.Checked)
                        {
                            found=true;
                            break;
                        }
                        else
                        {
                            _NextBackup = _NextBackup.AddDays(1.0); //try next day
                            continue;
                        }
                    }
                    else if (weekday == (int)DayOfWeek.Thursday)
                    {
                        if (checkBoxThur.Checked)
                        {
                            found=true;
                            break;
                        }
                        else
                        {
                            _NextBackup = _NextBackup.AddDays(1.0); //try next day
                            continue;
                        }
                    }
                    else if (weekday == (int)DayOfWeek.Friday)
                    {
                        if (checkBoxFri.Checked)
                        {
                            found=true;
                            break;
                        }
                        else
                        {
                            _NextBackup = _NextBackup.AddDays(1.0); //try next day
                            continue;
                        }
                    }
                    else if (weekday == (int)DayOfWeek.Saturday)
                    {
                        if (checkBoxSat.Checked)
                        {
                            found=true;
                            break;
                        }
                        else
                        {
                            _NextBackup = _NextBackup.AddDays(1.0); //try next day
                            continue;
                        }
                    }
                        
                }//end for

                if (!found)
                {
                    //textoutput("No valid day found - resetting to any");
                    checkBoxAny.Checked = true;                   
                }
            }

            //display new values
            
            labelCheckingdate.Text = _NextBackup.ToString();
            
        }

        private void checkBoxMon_CheckedChanged(object sender, EventArgs e)
        {
            ProcessWeekDay(checkBoxMon, "Monday");
            CalculateNextScheduleTime();
        }

        private void checkBoxTue_CheckedChanged(object sender, EventArgs e)
        {
            ProcessWeekDay(checkBoxTue, "Tuesday");
            CalculateNextScheduleTime();
        }

        private void checkBoxWed_CheckedChanged(object sender, EventArgs e)
        {
            ProcessWeekDay(checkBoxWed, "Wednesday");
            CalculateNextScheduleTime();
        }

        private void checkBoxThur_CheckedChanged(object sender, EventArgs e)
        {
            ProcessWeekDay(checkBoxThur, "Thursday");
            CalculateNextScheduleTime();
        }

        private void checkBoxFri_CheckedChanged(object sender, EventArgs e)
        {
            ProcessWeekDay(checkBoxFri, "Friday");
            CalculateNextScheduleTime();
        }

        private void checkBoxSat_CheckedChanged(object sender, EventArgs e)
        {
            ProcessWeekDay(checkBoxSat, "Saturday");
            CalculateNextScheduleTime();
        }

        private void checkBoxSun_CheckedChanged(object sender, EventArgs e)
        {
            ProcessWeekDay(checkBoxSun, "Sunday");
            CalculateNextScheduleTime();
        } 

        private void checkBoxAny_CheckedChanged(object sender, EventArgs e)
        {
            ProcessWeekDay(checkBoxAny, "Any");
            CalculateNextScheduleTime();
        }

        private void comboBoxhours_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalculateNextScheduleTime();
        }

        private void comboBoxminutes_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalculateNextScheduleTime();
        }

        private void comboBoxdays_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalculateNextScheduleTime();
        }

        private void checkBoxEnableScheduler_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxEnableScheduler.Checked)
            {
                CalculateNextScheduleTime();
            }
            else
            {
                labelCheckingdate.Text = "Not Scheduled";
                _NextBackup = DateTime.ParseExact("2200-01-01_00:00", "yyyy-MM-dd_HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            }
            
        }

        private void ProcessWeekDay(CheckBox mycheckbox, string weekday)
        {
            if (mycheckbox.Checked)
            {
                if (_WeekDays.Contains(weekday) == false)
                    _WeekDays += weekday;
            }
            else
            {
                _WeekDays = _WeekDays.Replace(weekday, string.Empty);
            }
        }


        #endregion scheduler

        

    }

       
      
     
}


