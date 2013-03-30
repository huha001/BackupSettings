#region Copyright (C) 2006-2009 Team MediaPortal

/* 
 *	Copyright (C) 2006-2009 Team MediaPortal
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

#endregion

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
using MediaPortal.Plugins;
using BackupSettingsPlugin;


namespace BackupSettingsMP.exe
{
    public partial class Form1 : Form
    {
        #region Globals

        public WhoAmI IDENT = WhoAmI.MP1_Client; //plugin identification to keep code common for all plugins will be defined by calling arguments

        //Constant file names - do not change
        
        //string BACKUPSETTING_TV_DIR = "BackupSettings";
        //string BACKUPSETTING_MP_DIR = "BackupSettings";
        string STATUS_FILE = "BackupSettingsStatus.txt";
        //public string INSTALLPATHS_FILE = "MP_TV_Installpaths.txt";
        //public string POSTPROCESSING_FILE = "ImportPostprocessing.txt";
        //public string MEDIAPORTALDIRS_FILE = "MediaPortalDirs.xml";
        //public string TV_CONFIG_WINDOW_NAME = "MediaPortal - TV Server Configuration";
        //public string TV_CONFIG_WINDOW_CLASS = "WindowsForms10.Window.8.app.0.33c0d9d";

        string CONFIG_FILE = "NOT_DEFINED";
        string RESTART_APPLICATION = "NOT_DEFINED";

        string BackupSettings_DIR = "NOT_DEFINED";

        bool DEBUG = false;
        bool BUSY = false;

        string MP_PROGRAM_FOLDER = "NOT_DEFINED";
        string MP_USER_FOLDER = "NOT_DEFINED";
        string TV_PROGRAM_FOLDER = "NOT_DEFINED";
        string TV_USER_FOLDER = "NOT_DEFINED";
        //string CENTRAL_DATABASE = "NOT_DEFINED";

        string MP2_PROGRAM_FOLDER = "NOT_DEFINED";
        string MP2_USER_FOLDER = "NOT_DEFINED";
        string SV2_PROGRAM_FOLDER = "NOT_DEFINED";
        string SV2_USER_FOLDER = "NOT_DEFINED";

        int SELECTED_COL = 1;
        int SELECTED_ROW = 1;
        char BACKUPSETTINGS_COLUMN_SEPARATOR = ';';
        int LISTVIEWCOLUMNS = 6;


        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private Container components = null;
        StreamWriter Status;
        InstallPaths instpaths = new InstallPaths();  //define new instance for folder detection
        BackupSettingsExportImport newexportimport = new BackupSettingsExportImport();


        System.Threading.Thread PB_th = null;
        System.Threading.Thread th = null;

        #endregion Globals

        public Form1(string id)
        {
            InitializeComponent();

            instpaths.DEBUG = DEBUG; //not loaded yet from settings

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


            //determine ID
            
            if (id.ToLower() == WhoAmI.MP1_Client.ToString().ToLower())
            {
                IDENT = WhoAmI.MP1_Client;
            }
            else if (id.ToLower() == WhoAmI.MP2_Client.ToString().ToLower())
            {
                IDENT = WhoAmI.MP2_Client;                
            }
            else //default MP2-Server
            {
                IDENT = WhoAmI.MP2_Server;                
            }


            //if not found by autodetection user must be asked!!!

            
            // if not available ask for installation folders 
            
            switch (IDENT)
            {
                case WhoAmI.MP1_Client:
                    {
                        #region MP1 Client
                        
                        if (File.Exists(MP_PROGRAM_FOLDER + @"\MediaPortal.exe") == false)
                        {
                            MP_PROGRAM_FOLDER = instpaths.ask_MP_PROGRAM();
                        }


                        if (File.Exists(MP_PROGRAM_FOLDER + @"\MediaPortal.exe") == false)
                        {
                            MessageBox.Show(MP_PROGRAM_FOLDER + @"\MediaPortal.exe does not exist ", "Error");
                            return;
                        }

                        if (Directory.Exists(MP_USER_FOLDER) == false)
                        {
                            MP_USER_FOLDER = instpaths.ask_MP_USER();
                        }

                        if (Directory.Exists(MP_USER_FOLDER) == false)
                        {
                            MessageBox.Show("MediaPortal user folder "+MP_USER_FOLDER+" does not exist ", "Error");
                            return;
                        }

                        BackupSettings_DIR = MP_USER_FOLDER + @"\BackupSettings";
                        CONFIG_FILE = MP_USER_FOLDER + @"\BackupSettingsMP.xml";
                        RESTART_APPLICATION = MP_PROGRAM_FOLDER + @"\Configuration.exe";

                        #endregion MP1 Client
                        break;
                    }
                case WhoAmI.MP2_Client:
                    {
                        #region MP2 Client
                        

                        if (File.Exists(MP2_PROGRAM_FOLDER + @"\MP2-Client.exe") == false)
                        {
                            MP2_PROGRAM_FOLDER = instpaths.ask_MP2_PROGRAM();
                        }


                        if (File.Exists(MP2_PROGRAM_FOLDER + @"\MP2-Client.exe") == false)
                        {
                            MessageBox.Show(MP2_PROGRAM_FOLDER + @"\MP2-Client.exe does not exist ", "Error");
                            return;
                        }

                        if (Directory.Exists(MP2_USER_FOLDER) == false)
                        {
                            MP2_USER_FOLDER = instpaths.ask_MP2_USER();
                        }

                        if (Directory.Exists(MP2_USER_FOLDER) == false)
                        {
                            MessageBox.Show("MediaPortal2 client user folder "+MP2_USER_FOLDER+" does not exist ", "Error");
                            return;
                        }

                        BackupSettings_DIR = instpaths.DIR_MP2_Config + @"\" + System.Environment.UserName + @"\BackupSettings";
                        CONFIG_FILE = instpaths.DIR_MP2_Config + @"\" + System.Environment.UserName + @"\BackupSettings\BackupSettingsMP.xml";
                        RESTART_APPLICATION = MP2_PROGRAM_FOLDER + @"\MP2-Client.exe";
                        
                        #endregion MP2 Client
                        break;
                    }
                case WhoAmI.MP2_Server:
                    {
                        #region MP2 Server
                        
                        if (File.Exists(SV2_PROGRAM_FOLDER + @"\MP2-Server.exe") == false)
                        {
                            SV2_PROGRAM_FOLDER = instpaths.ask_SV2_PROGRAM();
                        }

                        if (File.Exists(SV2_PROGRAM_FOLDER + @"\MP2-Server.exe") == false)
                        {
                            MessageBox.Show(SV2_PROGRAM_FOLDER + @"\MP2-Server.exe does not exist ", "Error");
                            return;
                        }

                        if (Directory.Exists(SV2_USER_FOLDER) == false)
                        {
                            SV2_USER_FOLDER = instpaths.ask_SV2_USER();
                        }

                        if (Directory.Exists(SV2_USER_FOLDER) == false)
                        {
                            MessageBox.Show("MediaPortal2 server user folder "+SV2_USER_FOLDER+" does not exist ", "Error");
                            return;
                        }

                        BackupSettings_DIR = instpaths.DIR_SV2_Config + @"\" + System.Environment.UserName + @"\BackupSettings";
                        CONFIG_FILE = BackupSettings_DIR + @"\BackupSettingsMP.xml";
                        //RESTART_APPLICATION = SV2_PROGRAM_FOLDER + @"\MP2-Server.exe";
                        RESTART_APPLICATION = ""; //no restart
  
                        #endregion SV2 Server
                        break;
                    }

            }

            if (Directory.Exists(BackupSettings_DIR) == false)
            {
                Directory.CreateDirectory(BackupSettings_DIR);
            }


            // read old status from file
            if (File.Exists(BackupSettings_DIR + @"\" + STATUS_FILE) == true)
            {
                try
                {
                    //read old status
                    StreamReader sfile = File.OpenText(BackupSettings_DIR + @"\" + STATUS_FILE);
                    ReadOldStatusFile(sfile);
                }
                catch (Exception exc)
                {
                    textoutput("BackupSettings: Fatal Error: Could not open file in append mode for " + BackupSettings_DIR + @"\" + STATUS_FILE + " - Exception:" + exc.Message);
                    return;
                }
            }

            //open file in append mode for new status
            try
            {
                Status = File.AppendText(BackupSettings_DIR + @"\" + STATUS_FILE);
                if (DEBUG)
                    textoutput("BackupSettingsSetup: Status File opened in append mode");
            }
            catch (Exception exc)
            {
                textoutput("BackupSettingsSetup: Fatal Error: Could not open file in append mode for " + BackupSettings_DIR + @"\" + STATUS_FILE + " - Exception:" + exc.Message);
            }


            defaults();
            LoadSettings();  //update GUI based on settings
            SaveSettings();  //ensures that paths are written to setting file and setting file does exist for importexport


            


            //enable text events
            if (instpaths != null)
            {
                instpaths.newmessage += new textmessage(textoutput);
                instpaths.DEBUG = DEBUG;
            }

            if (newexportimport != null)
            {
                newexportimport.newmessage += new textexportmessage(textoutput);
                newexportimport.ExportImport(ref progressBar, CONFIG_FILE);
                newexportimport.MyLoadSettings();
                newexportimport.getallversionnumbers("", false); //get only program version numbers after loadsettings                
            }
           

            textoutput("Plugin: " + IDENT.ToString() + "\n");

            //enable autodate if checked
            if (checkBoxUseAutoDate.Checked == true)
            {
                //create automated foldername
                SaveSettings();
                newexportimport.MyLoadSettings();
                filenametextBox.Text = newexportimport.CreateAutomatedFolderName(filenametextBox.Text, WhoAmI.Tv_Server);
            }

            UpdateGUI();

            //set current working directory
                        
            switch (IDENT)
            {
                case WhoAmI.MP1_Client:
                    {
                        System.Environment.CurrentDirectory = MP_PROGRAM_FOLDER;
                        break;
                    }
                case WhoAmI.MP2_Client:
                    {                      
                        System.Environment.CurrentDirectory = MP2_PROGRAM_FOLDER;
                        break;
                    }
                case WhoAmI.MP2_Server:
                    {
                        System.Environment.CurrentDirectory = SV2_PROGRAM_FOLDER;
                        break;
                    }
            }
           
        }

        protected void ReadOldStatusFile(StreamReader sfile)
        {
            // read old status from file
            
            
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
                    }
                }
            }
            sfile.Close();  //old status is now displayed in listbox
            
        }

        /// <summary>
        /// Die verwendeten Ressourcen bereinigen.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }

            //disable text events
            if (instpaths != null)
            {
                instpaths.newmessage -= new textmessage(textoutput);
            }

            if (newexportimport != null)
            {
                newexportimport.newmessage -= new textexportmessage(textoutput);
            }


            textoutput("Backup_Settings: Configuration closing");
            SaveSettings();


            if (RESTART_APPLICATION != "NOT_DEFINED") 
            {
                //restart SetupTv.exe
                Process app = new Process();
                ProcessStartInfo appstartinfo = new ProcessStartInfo();
                appstartinfo.FileName = RESTART_APPLICATION;
                app.StartInfo = appstartinfo;
                try
                {
                    app.Start();
                }
                catch (Exception ee)
                {
                    textoutput("Error in starting " + appstartinfo.FileName + "\nException message is \n" + ee.Message);
                }
            }

            if (BUSY == true)
            {
                textoutput("<RED>\nTV Server Configuration has been terminated");
                textoutput("<RED>because you exited BackupSettings");
                textoutput("<RED>before the last operation completed");
                textoutput("<RED>You must repeat your last operation\n");
            }
            if (Status != null)
            {
                Status.WriteLine("ProgressBar.Maximum  " + progressBar.Maximum.ToString());
                Status.WriteLine("ProgressBar.Value  " + progressBar.Value.ToString());
                Status.Close();  //  close status file
                Status=null;
            }

            base.Dispose(disposing);




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

        public void LoadSettings()
        {
            if (File.Exists(CONFIG_FILE) == false)
            {
                textoutput("Could not read settings file " + CONFIG_FILE + " - using default settings");
                //must have defaults
                filenametextBox.Text = @"C:\MediaPortal Backups";
                return;
            }

            try
            {
                if (DEBUG == true)
                    textoutput("Loading Settings");


                
                //Load PB defaults
                

                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.Load(CONFIG_FILE);
                }
                catch (Exception exc)
                {
                    textoutput("Using default values instead of BackupSettingsMP.xml file");

                    if (DEBUG == true)
                        textoutput("Exception message is:" + exc.Message);

                    defaults();
                }
 
                try
                {
                    XmlNodeList nodes = doc.SelectNodes("/BackupSettingsMP/node/setting");
                    string setting = "NOT_DEFINED";

                    foreach (XmlNode node in nodes)
                    {
                        CheckBoxDebugBackupSettings.Checked = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsSetup_debug", "false"));
                        DEBUG = CheckBoxDebugBackupSettings.Checked;


                        setting = getxmldata(doc, node, "Backup_SettingsSetup_TV_PROGRAM_FOLDER", "NOT_DEFINED");
                        if ((setting != "NOT_DEFINED") && (setting != "") && (File.Exists(setting + @"\TvService.exe")))
                            textBoxTV1P.Text = setting;


                        setting = getxmldata(doc, node, "Backup_SettingsSetup_TV_USER_FOLDER", "NOT_DEFINED");
                        if ((setting != "NOT_DEFINED") && (setting != "") && (Directory.Exists(setting)))
                            textBoxTV1U.Text = setting;


                        setting = getxmldata(doc, node, "Backup_SettingsSetup_MP_PROGRAM_FOLDER", "NOT_DEFINED");
                        if ((setting != "NOT_DEFINED") && (setting != "") && (File.Exists(setting + @"\MediaPortal.exe")))
                            textBoxMP1P.Text = setting;



                        setting = getxmldata(doc, node, "Backup_SettingsSetup_MP_USER_FOLDER", "NOT_DEFINED");
                        if ((setting != "NOT_DEFINED") && (setting != "") && (Directory.Exists(setting)))
                            textBoxMP1U.Text = setting;


                        setting = getxmldata(doc, node, "Backup_SettingsSetup_MP2_PROGRAM_FOLDER", "NOT_DEFINED");
                        if ((setting != "NOT_DEFINED") && (setting != "") && (File.Exists(setting + @"\MP2-Client.exe")))
                            textBoxMP2P.Text = setting;


                        setting = getxmldata(doc, node, "Backup_SettingsSetup_MP2_USER_FOLDER", "NOT_DEFINED");
                        if ((setting != "NOT_DEFINED") && (setting != "") && (Directory.Exists(setting)))
                            textBoxMP2U.Text = setting;


                        setting = getxmldata(doc, node, "Backup_SettingsSetup_SV2_PROGRAM_FOLDER", "NOT_DEFINED");
                        if ((setting != "NOT_DEFINED") && (setting != "") && (File.Exists(setting + @"\MP2-Server.exe")))
                            textBoxSV2P.Text = setting;


                        setting = getxmldata(doc, node, "Backup_SettingsSetup_SV2_USER_FOLDER", "NOT_DEFINED");
                        if ((setting != "NOT_DEFINED") && (setting != "") && (Directory.Exists(setting)))
                            textBoxSV2U.Text = setting;


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

                        filenametextBox.Text = getxmldata(doc, node, "Backup_SettingsSetup_filename", @"C:\MediaPortal Backups");

                        checkBoxUseAutoDate.Checked = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsSetup_UseAutoDate", "true"));
                        checkBoxMP.Checked = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsSetup_MP", "true"));
                        checkBoxMP2C.Checked = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsSetup_MP2C", "true"));
                        checkBoxMP2S.Checked = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsSetup_MP2S", "true"));

                        UpdateGUI();

                        checkBoxMPDatabase.Checked = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsMPDatabase", "true"));
                        checkBoxMPInputDevice.Checked = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsMPInputDevice", "true"));
                        checkBoxMPPlugins.Checked = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsMPPlugins", "true"));
                        checkBoxMPProgramXml.Checked = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsMPProgramXml", "true"));
                        checkBoxMPSkins.Checked = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsMPSkins", "true"));
                        checkBoxMPThumbs.Checked = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsMPThumbs", "true"));
                        checkBoxMPUserXML.Checked = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsMPUserXML", "true"));
                        checkBoxMPxmltv.Checked = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsMPxmltv", "true"));
                        checkBoxMPMusicPlayer.Checked = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsMPMusicPlayer", "true"));
                        checkBoxMPDeleteCache.Checked = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsMPDeleteCache", "true"));
                        checkBoxMPAllFolders.Checked = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsMPAllFolders", "true"));
                        checkBoxMPAllProgram.Checked = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsMPAllProgram", "true"));

                        checkBoxMP2Defaults.Checked = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsMP2Defaults", "true"));
                        checkBoxMP2Config.Checked = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsMP2Config", "true"));
                        checkBoxMP2Plugins.Checked = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsMP2Plugins", "true"));
                        checkBoxMP2AllClientFolders.Checked = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsMP2AllClientFolders", "true"));
                        checkBoxMP2AllClientProgramFolders.Checked = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsMP2AllClientProgramFolders", "true"));
                        checkBoxMP2AllClientFiles.Checked = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsMP2AllClientFiles", "true"));

                        checkBoxSV2Config.Checked = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsSV2Configuration", "true"));
                        checkBoxSV2Database.Checked = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsSV2Database", "true"));
                        checkBoxSV2Plugins.Checked = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsSV2Plugins", "true"));
                        checkBoxSV2Defaults.Checked = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsSV2Defaults", "true"));
                        checkBoxSV2AllServerFolders.Checked = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsSV2AllServerFolders", "true"));
                        checkBoxSV2AllServerProgramFolders.Checked = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsSV2AllServerProgramFolders", "true"));
                        checkBoxSV2AllServerFiles.Checked = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsSV2AllServerFiles", "true"));

                        radioButtonEasy.Checked = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsEasy", "true"));
                        radioButtonExpert.Checked = Convert.ToBoolean(getxmldata(doc, node, "Backup_SettingsExpert", "false"));

                        //listviewdata
                        BACKUPSETTINGS_COLUMN_SEPARATOR = Convert.ToChar(getxmldata(doc, node, "Backup_SettingsColumnSeparator", ";"));

                        //remove all rows besides last row 
                        int rowcount = dataGridView1.Rows.Count;
                        for (int i = 0; i < rowcount - 1; i++)
                        {
                            dataGridView1.Rows.Remove(dataGridView1.Rows[0]);
                        }

                        string listviewdata = getxmldata(doc, node, "BackupSettings_ListView", "");


#if (PB)
                    textoutput("1) liestview=" + listviewdata);
                
#endif
                        if (listviewdata == "")
                        {
                            listviewdata = autoinit();
                        }



                        string[] rowdata = listviewdata.Split('\n');


#if (PB)
                    textoutput("2) liestview=" + listviewdata);
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
                    }

                }
                catch (Exception ex)
                {
                    filenametextBox.Text = "";
                    defaults();
                    if (DEBUG == true)
                        textoutput("<RED>Backup_SettingsMP: Setup - LoadSettings(): " + ex.Message);
                }               
            }
            catch (Exception exc)
            {
                textoutput("BackupSettings LoadSettings: Fatal Error Exception:" + exc.Message);
                MessageBox.Show("BackupSettings LoadSettings: Fatal Error Exception:" + exc.Message);
            }
        }

        public void SaveSettings()
        {
            try
            {
                if (DEBUG == true)
                    textoutput("Saving Settings");

                XmlDocument xmlDoc = new XmlDocument();
                XmlNode rootElement = xmlDoc.CreateElement("BackupSettingsMP");
                XmlNode nodes = xmlDoc.CreateElement("node");
                XmlNode node = xmlDoc.CreateElement("setting");

                AddAttribute(node, "Backup_SettingsSetup_filename", filenametextBox.Text);              
                AddAttribute(node, "Backup_SettingsSetup_debug", DEBUG);
                AddAttribute(node, "Backup_SettingsSetup_UseAutoDate", checkBoxUseAutoDate.Checked.ToString());

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

                AddAttribute(node, "Backup_SettingsSetup_MP_PROGRAM_FOLDER", MP_PROGRAM_FOLDER);
                AddAttribute(node, "Backup_SettingsSetup_MP_USER_FOLDER", MP_USER_FOLDER);
                AddAttribute(node, "Backup_SettingsSetup_TV_PROGRAM_FOLDER", TV_PROGRAM_FOLDER);
                AddAttribute(node, "Backup_SettingsSetup_TV_USER_FOLDER", TV_USER_FOLDER);
                AddAttribute(node, "Backup_SettingsSetup_MP2_PROGRAM_FOLDER", MP2_PROGRAM_FOLDER);
                AddAttribute(node, "Backup_SettingsSetup_MP2_USER_FOLDER", MP2_USER_FOLDER);
                AddAttribute(node, "Backup_SettingsSetup_SV2_PROGRAM_FOLDER", SV2_PROGRAM_FOLDER);
                AddAttribute(node, "Backup_SettingsSetup_SV2_USER_FOLDER", SV2_USER_FOLDER);

                AddAttribute(node, "Backup_SettingsSetup_MP", checkBoxMP.Checked.ToString());
                AddAttribute(node, "Backup_SettingsSetup_MP2C", checkBoxMP2C.Checked.ToString());
                AddAttribute(node, "Backup_SettingsSetup_MP2S", checkBoxMP2S.Checked.ToString());

                AddAttribute(node, "Backup_SettingsMPDatabase", checkBoxMPDatabase.Checked.ToString());
                AddAttribute(node, "Backup_SettingsMPInputDevice", checkBoxMPInputDevice.Checked.ToString());
                AddAttribute(node, "Backup_SettingsMPPlugins", checkBoxMPPlugins.Checked.ToString());
                AddAttribute(node, "Backup_SettingsMPProgramXml", checkBoxMPProgramXml.Checked.ToString());
                AddAttribute(node, "Backup_SettingsMPSkins", checkBoxMPSkins.Checked.ToString());
                AddAttribute(node, "Backup_SettingsMPThumbs", checkBoxMPThumbs.Checked.ToString());
                AddAttribute(node, "Backup_SettingsMPUserXML", checkBoxMPUserXML.Checked.ToString());
                AddAttribute(node, "Backup_SettingsMPxmltv", checkBoxMPxmltv.Checked.ToString());
                AddAttribute(node, "Backup_SettingsMPMusicPlayer", checkBoxMPMusicPlayer.Checked.ToString());
                AddAttribute(node, "Backup_SettingsMPDeleteCache", checkBoxMPDeleteCache.Checked.ToString());
                AddAttribute(node, "Backup_SettingsMPAllFolders", checkBoxMPAllFolders.Checked.ToString());
                AddAttribute(node, "Backup_SettingsMPAllProgram", checkBoxMPAllProgram.Checked.ToString());

                AddAttribute(node, "Backup_SettingsMP2Defaults", checkBoxMP2Defaults.Checked.ToString());
                AddAttribute(node, "Backup_SettingsMP2Config", checkBoxMP2Config.Checked.ToString());
                AddAttribute(node, "Backup_SettingsMP2Plugins", checkBoxMP2Plugins.Checked.ToString());
                AddAttribute(node, "Backup_SettingsMP2AllClientFolders", checkBoxMP2AllClientFolders.Checked.ToString());
                AddAttribute(node, "Backup_SettingsMP2AllClientProgramFolders", checkBoxMP2AllClientProgramFolders.Checked.ToString());
                AddAttribute(node, "Backup_SettingsMP2AllClientFiles", checkBoxMP2AllClientFiles.Checked.ToString());

                AddAttribute(node, "Backup_SettingsSV2Defaults", checkBoxSV2Defaults.Checked.ToString());
                AddAttribute(node, "Backup_SettingsSV2Config", checkBoxSV2Config.Checked.ToString());
                AddAttribute(node, "Backup_SettingsSV2Database", checkBoxSV2Database.Checked.ToString());
                AddAttribute(node, "Backup_SettingsSV2Plugins", checkBoxSV2Plugins.Checked.ToString());
                AddAttribute(node, "Backup_SettingsSV2AllServerFolders", checkBoxSV2AllServerFolders.Checked.ToString());
                AddAttribute(node, "Backup_SettingsSV2AllServerProgramFolders", checkBoxSV2AllServerProgramFolders.Checked.ToString());
                AddAttribute(node, "Backup_SettingsSV2AllServerFiles", checkBoxSV2AllServerFiles.Checked.ToString());

                AddAttribute(node, "Backup_SettingsEasy", radioButtonEasy.Checked.ToString());
                AddAttribute(node, "Backup_SettingsExpert", radioButtonExpert.Checked.ToString());


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


#if (PB)
            textoutput("listviewstring=" + listviewstring);
#endif

                AddAttribute(node, "BackupSettings_ListView", listviewstring);

                nodes.AppendChild(node);
                rootElement.AppendChild(nodes);
                xmlDoc.AppendChild(rootElement);
                xmlDoc.Save(CONFIG_FILE);

                if (DEBUG)
                    textoutput("Setting file "+CONFIG_FILE+" saved");
            }
            catch (Exception exc)
            {
                textoutput("BackupSettings SaveSettings: Fatal Error Exception:" + exc.Message);
                MessageBox.Show("BackupSettings SaveSettings: Fatal Error Exception:" + exc.Message);
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


        private void textoutput(string textlines)
        {

            string text = "";

            if (listView1 != null)
            {
                Color mycolor;
                string colorstring = "";
                if (textlines.StartsWith("<RED>") == true)
                {//color red
                    textlines = textlines.Substring(5, textlines.Length - 5);
                    mycolor = Color.Red;
                    colorstring = "<RED>";
                }
                else if (textlines.StartsWith("<YELLOW>") == true)
                {//color yellow
                    textlines = textlines.Substring(8, textlines.Length - 8);
                    mycolor = Color.Orange;
                    colorstring = "<YELLOW>";
                }
                else if (textlines.StartsWith("<GREEN>") == true)
                {//color green
                    textlines = textlines.Substring(8, textlines.Length - 8);
                    mycolor = Color.Green;
                    colorstring = "<GREEN>";
                }
                else
                {//color black
                    mycolor = Color.Black;
                }

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

                    try
                    {
                        Status.WriteLine(colorstring + line);
                    }
                    catch //do nothing if stream writer is not open
                    {
                    }

                }

            }
        }


        private void UpdateGUI()
        {
            
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
                if (tabControl1.TabPages.Contains(tabPage3))
                    tabControl1.TabPages.Remove(tabPage3);
                if (tabControl1.TabPages.Contains(tabPage4))
                    tabControl1.TabPages.Remove(tabPage4);
                if (tabControl1.TabPages.Contains(tabPage2))
                    tabControl1.TabPages.Remove(tabPage2);
                if (tabControl1.TabPages.Contains(tabPage5))
                    tabControl1.TabPages.Remove(tabPage5);
                

                checkBoxMP.Hide();
                checkBoxMP2C.Hide();
                checkBoxMP2S.Hide();
            }
            else
            {
                if (tabControl1.TabPages.Contains(tabPage3))
                    tabControl1.TabPages.Remove(tabPage3);
                if (tabControl1.TabPages.Contains(tabPage4))
                    tabControl1.TabPages.Remove(tabPage4);
                if (tabControl1.TabPages.Contains(tabPage2))
                    tabControl1.TabPages.Remove(tabPage2);
                if (tabControl1.TabPages.Contains(tabPage5))
                    tabControl1.TabPages.Remove(tabPage5);
                


                

                if (!tabControl1.TabPages.Contains(tabPage3) && ((textBoxMP1P.Text != "NOT_DEFINED") && (textBoxMP1U.Text != "NOT_DEFINED")))//MP1
                    tabControl1.TabPages.Add(tabPage3);


                if (!tabControl1.TabPages.Contains(tabPage4) && (((textBoxMP2P.Text != "NOT_DEFINED") && (textBoxMP2U.Text != "NOT_DEFINED")) || ((textBoxSV2P.Text != "NOT_DEFINED") && (textBoxSV2U.Text != "NOT_DEFINED"))))//MP2
                    tabControl1.TabPages.Add(tabPage4);


                if (!tabControl1.TabPages.Contains(tabPage2))
                    tabControl1.TabPages.Add(tabPage2);
                if (!tabControl1.TabPages.Contains(tabPage5))
                    tabControl1.TabPages.Add(tabPage5);
                

                if ((textBoxMP1P.Text != "NOT_DEFINED") && (textBoxMP1U.Text != "NOT_DEFINED"))
                    checkBoxMP.Show();
                else
                    checkBoxMP.Hide();

                
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


        private void buttonClose_Click(object sender, EventArgs e)
        {
            SaveSettings();
            this.Close();
        }


        private void importbutton_Click(object sender, EventArgs e)
        {
            if (BUSY == true)
            {
                MessageBox.Show("Processing ongoing - please wait for completion", "Warning");
                return;
            }
            BUSY = true;

            SaveSettings(); //does save actual paths and settings
            LoadSettings(); //does update paths

            th = new System.Threading.Thread(importthread);
            th.Start();
        }


        private void importthread()
        {
            BackupSettingsExportImport newimport = new BackupSettingsExportImport();

            if (newimport != null)
            {
                newimport.newmessage += new textexportmessage(textoutput);
                newimport.ExportImport(ref progressBar, CONFIG_FILE);

                newimport.MyLoadSettings();
                newimport.getallversionnumbers("", false); //get only program version numbers after loadsettings
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



            /*
            bool ok = newexportimport.restorebackup(filenametextBox.Text);
            if (ok)
                MessageBox.Show("Import completed successfully", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            else
            {
                if (PB_th != null)
                {
                    newexportimport.progressbar((int)PB_action.CANCEL, ref PB_Import, PB_action_number);
                }
                MessageBox.Show(this, "Import not successful - Check the status and the log file");
            }

            //MyLoadSettings();  // reload plugin settings after import
            textoutput("\n");  //create new paragraph after completed import
            BUSY = false; //reset processing flag   */        
        }



        private void exportbutton_Click(object sender, EventArgs e)
        {
            if (BUSY == true)
            {
                MessageBox.Show("Processing ongoing - please wait for completion", "Warning");
                return;
            }
            BUSY = true;

            SaveSettings(); //does save actual paths and settings
            LoadSettings(); //does update paths

            th = new System.Threading.Thread(exportthread);
            th.Start();

            
        }

        
        private void exportthread()
        {

            BackupSettingsExportImport newexport = new BackupSettingsExportImport();

            if (newexport != null)
            {
                //newexport.ExportImportInit(ref progressBar, false);
                newexport.newmessage += new textexportmessage(textoutput);
                newexport.ExportImport(ref progressBar,CONFIG_FILE);

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


        private void buttonDefault_Click(object sender, EventArgs e)
        {// default button restore all default settings
            defaults();
        }

        public void defaults()
        {
            radioButtonEasy.Checked = true;
            //radioButtonExpert.Checked = false;

            //TV and MP
            
            if ((textBoxMP1P.Text != "NOT_DEFINED") && (textBoxMP1U.Text != "NOT_DEFINED"))
                checkBoxMP.Checked = true;

            if ((textBoxMP2P.Text != "NOT_DEFINED") && (textBoxMP2U.Text != "NOT_DEFINED"))
                checkBoxMP2C.Checked = true;

            if ((textBoxSV2P.Text != "NOT_DEFINED") && (textBoxSV2U.Text != "NOT_DEFINED"))
                checkBoxMP2S.Checked = true;

            UpdateGUI();

            
            checkBoxUseAutoDate.Checked = true;

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
        }
        
        private void clearbutton_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            Status.Close();  //  close status file
            File.Delete(BackupSettings_DIR + @"\" + STATUS_FILE);// delete status file
            //open file in append mode for new status
            try
            {
                Status = File.AppendText(BackupSettings_DIR + @"\" + STATUS_FILE);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Fatal Error: Could not open file in append mode for " + BackupSettings_DIR + @"\" + STATUS_FILE + " - Exception:" + exc.Message);
            }
            //initialize progressbar
            newexportimport.ProgressbarInit();
        }

        private void CheckBoxDebugBackupSettings_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxDebugBackupSettings.Checked == true)
                DEBUG = true;
            else
                DEBUG = false;
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


        private void buttonhelp_Click(object sender, EventArgs e)
        {
            Process proc = new Process();
            ProcessStartInfo procstartinfo = new ProcessStartInfo();
            procstartinfo.FileName = MP_PROGRAM_FOLDER + @"\Docs\BackupSettings.pdf";
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
                textoutput("Internet link failed");
                textoutput("Internet link failed with exception: " + exc.Message);
            }

        }

        private void selectfilenamebutton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog selectfolder = new FolderBrowserDialog();
            selectfolder.Description = "Select Import/Export Folder";
            selectfolder.SelectedPath = filenametextBox.Text;
            if (selectfolder.ShowDialog(this) == DialogResult.OK)
                filenametextBox.Text = selectfolder.SelectedPath;
        }

        private void buttonCreateAutoFolder_Click(object sender, EventArgs e)
        {
            //create automated foldername
            SaveSettings();
            newexportimport.MyLoadSettings();
            filenametextBox.Text = newexportimport.CreateAutomatedFolderName(filenametextBox.Text,IDENT);
            
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
            SaveSettings();
            LoadSettings();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            LoadSettings();
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

        

        

        

    

    }   
}

