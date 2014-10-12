
//enhancement: check for MP program running during install and uninstall (from tvwishlist)

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


namespace BackupSettingsInstall
{
    public partial class InstallSetup : Form
    {

        bool DEBUG=false;
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

        //other
        public bool UNINSTALL = false;
        public bool INSTALL = false;
        System.Timers.Timer m_timer = null;
        public string RESTART_SETUP_TV_EXE = "RestartSetupTV.exe";


        InstallPaths instpaths = new InstallPaths();  //define new instance for folder detection
        string installdirectory = System.Environment.CurrentDirectory;
                                

        enum CompareFileVersion 
        { 
            Newer = 1, 
            Older = -1, 
            Equal = 0, 
            Error = 89,
            Version1Error = 90, 
            Version1String = 91,
            Version2Error = 92,
            Version2String = 93
        };


        public InstallSetup(bool installflag, bool uninstallflag)
        {
            InitializeComponent();

            AutoDetect();

            UpdatePathVariables();


            //define default setting
            if ((TV_PROGRAM_FOLDER != "NOT_DEFINED") && (TV_USER_FOLDER != "NOT_DEFINED"))
            {
                checkBoxTV.Checked = true;
            }
            if ((MP_PROGRAM_FOLDER != "NOT_DEFINED") && (MP_USER_FOLDER != "NOT_DEFINED"))
            {
                checkBoxMP.Checked = true;
            }
            if ((MP2_PROGRAM_FOLDER != "NOT_DEFINED") && (MP2_USER_FOLDER != "NOT_DEFINED"))
            {
                checkBoxMP2C.Checked = true;
            }
            if ((SV2_PROGRAM_FOLDER != "NOT_DEFINED") && (SV2_USER_FOLDER != "NOT_DEFINED"))
            {
                checkBoxMP2S.Checked = true;
            }

            if (checkBoxTV.Checked) //preferred is only Tv server installation
            {
                checkBoxMP.Checked = false;
                checkBoxMP2C.Checked = false;
                checkBoxMP2S.Checked = false;
            }

            if (checkBoxMP2S.Checked) //preferred MP2 server installation
            {
                checkBoxMP2C.Checked = false;               
            }

            UpdateGUI();

            
            //legacy support delete old directory in PROGRAM_FOLDER
            if (Directory.Exists(MP_PROGRAM_FOLDER + @"\Installer\BackupSettings") == true)
            {
                Directory.Delete(MP_PROGRAM_FOLDER + @"\Installer\BackupSettings",true);
                textoutputdebug("Old BackupSettings install directory " + MP_PROGRAM_FOLDER + @"\Installer\BackupSettings has been deleted"+"\n");
            }


            if (File.Exists(installdirectory + @"\Install.exe") == false)  //1st current directory
            {//select install directory manually if current directory is not set to the extracted BackupSetting folder
                if (File.Exists(MP_USER_FOLDER + @"\Installer\BackupSettings\Install.exe") == true) //2nd try mpi installer directory
                {
                    installdirectory = MP_USER_FOLDER + @"\Installer\BackupSettings";
                }
                else
                {
                    if (File.Exists(System.Environment.CurrentDirectory + @"\%Installer%\BackupSettings\Install.exe") == true) // 3rd try %installer% directory                                    
                    {
                        installdirectory = System.Environment.CurrentDirectory + @"\%Installer%\BackupSettings";
                    }

                    else
                    {
                        FolderBrowserDialog folderDialog = new FolderBrowserDialog();
                        folderDialog.Description = "Select Extracted BackupSettings Release Folder (Contains Install.exe)";
                        folderDialog.ShowNewFolderButton = false;
                        if (folderDialog.ShowDialog() == DialogResult.OK)
                        {
                            if (File.Exists(folderDialog.SelectedPath + @"\Install.exe") == true)
                            {
                                installdirectory = folderDialog.SelectedPath;

                            }
                            else
                            {
                                textoutputdebug("Install.exe does not exist in the selected folder \naborting installation\n" + folderDialog.SelectedPath + "\n");
                                return;
                            }
                        }
                        else
                        {
                            textoutputdebug("User selected invalid BackupSettings folder or canceled \n aborting installation" + "\n");
                            return;
                        }
                    }
                }

            }

            textoutputdebug("Current folder is " + installdirectory + "\n");

            if (installflag == true)
            {
                INSTALL = true;
            }
            else if (uninstallflag == true)
            {
                UNINSTALL = true;
            }
            m_timer = new System.Timers.Timer(10); //close after 0.1s
            m_timer.Enabled = true;
            m_timer.Elapsed += new System.Timers.ElapsedEventHandler(autoinstallation);

        }


        public void AutoDetect()
        {
            //autodetect paths
            instpaths.DEBUG = DEBUG;

            instpaths.LOG = "";
            instpaths.GetInstallPaths();
            if (DEBUG)
                textoutputdebug(instpaths.LOG);

            textBoxMP1P.Text = instpaths.MP_PROGRAM_FOLDER;
            textBoxMP1U.Text = instpaths.MP_USER_FOLDER;
            textBoxTV1P.Text = instpaths.TV_PROGRAM_FOLDER;
            textBoxTV1U.Text = instpaths.TV_USER_FOLDER;

            instpaths.LOG = "";
            instpaths.GetInstallPathsMP2();
            if (DEBUG)
                textoutputdebug(instpaths.LOG);

            textBoxMP2P.Text = instpaths.MP2_PROGRAM_FOLDER;
            textBoxMP2U.Text = instpaths.MP2_USER_FOLDER;
            textBoxSV2P.Text = instpaths.SV2_PROGRAM_FOLDER;
            textBoxSV2U.Text = instpaths.SV2_USER_FOLDER;
            
        }

        public void UpdatePathVariables()
        {
            //kill emty strings for paths
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

            //update pasth variables
            MP_PROGRAM_FOLDER = textBoxMP1P.Text;
            MP_USER_FOLDER = textBoxMP1U.Text;
            TV_PROGRAM_FOLDER = textBoxTV1P.Text;
            TV_USER_FOLDER = textBoxTV1U.Text;

            MP2_PROGRAM_FOLDER = textBoxMP2P.Text;
            MP2_USER_FOLDER = textBoxMP2U.Text; ;
            SV2_PROGRAM_FOLDER = textBoxSV2P.Text;
            SV2_USER_FOLDER = textBoxSV2U.Text;


            //update instpaths variables a swell if not called for autodetect
            instpaths.MP_PROGRAM_FOLDER = textBoxMP1P.Text;
            instpaths.MP_USER_FOLDER = textBoxMP1U.Text;
            instpaths.TV_PROGRAM_FOLDER = textBoxTV1P.Text;
            instpaths.TV_USER_FOLDER = textBoxTV1U.Text;

            instpaths.MP2_PROGRAM_FOLDER = textBoxMP2P.Text;
            instpaths.MP2_USER_FOLDER = textBoxMP2U.Text; ;
            instpaths.SV2_PROGRAM_FOLDER = textBoxSV2P.Text;
            instpaths.SV2_USER_FOLDER = textBoxSV2U.Text;


            //get all install directories
            instpaths.LOG = "";
            instpaths.GetMediaPortalDirs();
            if (DEBUG)
                textoutputdebug(instpaths.LOG);

            instpaths.LOG = "";
            instpaths.GetMediaPortalDirsMP2();

            if (DEBUG)
                textoutputdebug(instpaths.LOG);

            
        }

        private void UpdateGUI()
        {
            if ((checkBoxTV.Checked == true) && ((TV_PROGRAM_FOLDER == "NOT_DEFINED") || (TV_USER_FOLDER == "NOT_DEFINED")))
            {
                MessageBox.Show("Unchecking Tv Server because paths are not correctly defined\nCheck the path configuration first", "Error");
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


            if (!checkBoxTV.Checked && !checkBoxMP.Checked && !checkBoxMP2C.Checked && !checkBoxMP2S.Checked)
                textoutputdebug("No Plugin Selection was found - Check the settings and/or installation paths");

        }


        public void autoinstallation(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (m_timer != null)
                m_timer.Elapsed -= new System.Timers.ElapsedEventHandler(autoinstallation);

            if (INSTALL == true)
            {
                install();
                System.Threading.Thread.Sleep(2000);
                Application.Exit();
            }
            else if (UNINSTALL == true)
            {
                uninstall();
                System.Threading.Thread.Sleep(2000);
                Application.Exit();
            }

        }
       
        private void buttoninstall_Click(object sender, EventArgs e)
        {
            install();
        }

        private void install()
        {
            UpdatePathVariables();
            UpdateGUI();

            if (File.Exists(installdirectory + @"\Install.exe") == false)
            {
                textoutputdebug("Cannot find install.exe in install directory " + installdirectory + " - aborting\n");
                return;
            }
            else
            {
                System.Environment.CurrentDirectory = installdirectory;
            }

            if ((checkBoxTV.Checked)&&(Directory.Exists(TV_PROGRAM_FOLDER) == true) && (Directory.Exists(TV_USER_FOLDER) == true)&&(TV_PROGRAM_FOLDER != "")&&(TV_USER_FOLDER != ""))
            { //install TVserver Plugin
                switch (MessageBox.Show("Do you want to install the BackupSettings TV Server Plugin?\nThe Tv server will be stopped, so make sure you are not recording!\nYes is recommended ", "Tv Server BackupSettings Plugin Installation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                {
                    case DialogResult.Yes:
                        {
                            // "Yes" processing
                            textoutputdebug("Installing TV Server Plugin BackupSettingsTV in ");
                            textoutputdebug(TV_PROGRAM_FOLDER + @"\Plugins"+"\n");
                            
                            //check for setupTv.exe
                            Process[] sprocs = Process.GetProcessesByName("SetupTv");
                            foreach (Process sproc in sprocs) // loop is only executed if Media portal is running
                            {
                                textoutputdebug("You need to close Tv Server Configuration before you install the plugin\n");
                                MessageBox.Show("You need to close Tv Server Configuration before you install the plugin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            Process[] tprocs = null;
                            tprocs= Process.GetProcessesByName("TVService");
                            bool TvserviceRunning=false;
                            foreach (Process tproc in tprocs)
                            {
                                TvserviceRunning = true;
                            }

                            //Stopping tv service
                            
                            if (TvserviceRunning == true)
                            {
                                StopTvService();                                
                            }
                            else
                            {
                                textoutputdebug("TV Service is not running" + "\n");
                            }

                            TvServerInstall();
 
                            //Starting tv service
                            if (TvserviceRunning == true)
                            {
                                StartTvService();                                
                            }

                            break;
                        }
                    case DialogResult.No:
                        {
                            // "No" processing
                            textoutputdebug("Installation aborted by user\n");
                            break;

                        }
                }

            }//end tvserver installation


            if ((checkBoxMP.Checked)&&(Directory.Exists(MP_PROGRAM_FOLDER) == true) && (Directory.Exists(MP_USER_FOLDER) == true)&&(MP_PROGRAM_FOLDER != "")&&(MP_USER_FOLDER != ""))
            {//install Media Portal1 Plugin
                switch (MessageBox.Show("Do you want to install the BackupSettingsMP Media Portal1 Plugin?\nYes is recommended ", "MediaPortal1 BackupSettings Plugin Installation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                {
                    case DialogResult.Yes:
                        {
                            //check for MediaPortal.exe
                            Process[] sprocs = Process.GetProcessesByName("MediaPortal");
                            foreach (Process sproc in sprocs) // loop is only executed if Media portal is running
                            {
                                textoutputdebug("You need to close MediaPortal before you install the plugin\n");
                                MessageBox.Show("You need to close MediaPortal before you install the plugin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            //check for Configuration.exe
                            sprocs = Process.GetProcessesByName("Configuration");
                            foreach (Process sproc in sprocs) // loop is only executed if Media portal is running
                            {
                                textoutputdebug("You need to close MediaPortal Configuration before you install the plugin\n");
                                MessageBox.Show("You need to close MediaPortal Configuration before you install the plugin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            // "Yes" processing
                            MP1Install();                            
                            break;
                        }
                    case DialogResult.No:
                        {
                            // "No" processing
                            textoutputdebug("Installation aborted by user\n");
                            break;
                        }
                }
            }//end MP1 installation
            
            if ((checkBoxMP2C.Checked) && (Directory.Exists(MP2_PROGRAM_FOLDER) == true) && (Directory.Exists(MP2_USER_FOLDER) == true) && (MP2_PROGRAM_FOLDER != "") && (MP2_USER_FOLDER != ""))
            {//install Media Portal Plugin
                switch (MessageBox.Show("Do you want to install the BackupSettingsMP2 Media Portal2 Client Plugin?\nYes is recommended ", "MediaPortal2 Client BackupSettings Plugin Installation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                {
                    case DialogResult.Yes:
                        {
                            //check for MP2-Client.exe
                            Process[] sprocs = Process.GetProcessesByName("MP2-Client");
                            foreach (Process sproc in sprocs) // loop is only executed if Media portal is running
                            {
                                textoutputdebug("You need to close the MediaPortal2 Client before you install the plugin\n");
                                MessageBox.Show("You need to close the MediaPortal2 Client you install the plugin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            // "Yes" processing
                            MP2ClientInstall();                          
                            break;
                        }
                    case DialogResult.No:
                        {
                            // "No" processing
                            textoutputdebug("Installation aborted by user\n");
                            break;
                        }
                }
            }//end MP2 client installation

            if ((checkBoxMP2S.Checked) && (Directory.Exists(SV2_PROGRAM_FOLDER) == true) && (Directory.Exists(SV2_USER_FOLDER) == true) && (SV2_PROGRAM_FOLDER != "") && (SV2_USER_FOLDER != ""))
            {//install Media Portal Plugin
                switch (MessageBox.Show("Do you want to install the BackupSettingsMP2 Media Portal2 Server Plugin?\nYes is recommended ", "MediaPortal2 Server BackupSettings Plugin Installation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                {
                    case DialogResult.Yes:
                        {
                            /* not needed for server
                            //check for MP2-Server.exe
                            Process[] sprocs = Process.GetProcessesByName("MP2-Server");
                            foreach (Process sproc in sprocs) // loop is only executed if Media portal is running
                            {
                                textoutputdebug("You need to close the MediaPortal2 Server before you install the plugin\n");
                                MessageBox.Show("You need to close the MediaPortal2 Server you install the plugin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }*/

                            // "Yes" processing

                            MP2ServerInstall();
                           
                            break;
                        }
                    case DialogResult.No:
                        {
                            // "No" processing
                            textoutputdebug("Installation aborted by user\n");
                            break;

                        }
                }
            }//end MP2 server installation

            textoutputdebug("Plugin installation finished\n");
            
        }

        private void TvServerInstall()
        {
            bool success = false;

            CreateTVServerAutoRepairBatch();
            textoutputdebug("");

            try  //copy RestartSetupTV.exe
            {
                if (Directory.Exists(TV_PROGRAM_FOLDER + @"\BackupSettings") == false)
                    Directory.CreateDirectory(TV_PROGRAM_FOLDER + @"\BackupSettings");
               
                if (Directory.Exists(TV_USER_FOLDER + @"\BackupSettings") == false)
                    Directory.CreateDirectory(TV_USER_FOLDER + @"\BackupSettings");

                //legacy processing

                // TV_PROGRAM_FOLDER + @"\BackupSettings\Status.txt"
                if (File.Exists(TV_PROGRAM_FOLDER + @"\BackupSettings\Status.txt"))
                {
                    try
                    {
                        File.Move(TV_PROGRAM_FOLDER + @"\BackupSettings\Status.txt", TV_USER_FOLDER + @"\BackupSettings\Status.txt");
                    }
                    catch { }
                }

                //TV_PROGRAM_FOLDER + @"\BackupSettings\ImportPostprocessing.txt.bak.txt"
                if (File.Exists(TV_PROGRAM_FOLDER + @"\BackupSettings\ImportPostprocessing.txt.bak.txt"))
                {
                    try
                    {
                        File.Move(TV_PROGRAM_FOLDER + @"\BackupSettings\ImportPostprocessing.txt.bak.txt", TV_USER_FOLDER + @"\BackupSettings\ImportPostprocessing.txt.bak.txt");
                    }
                    catch { }
                }

                //TV_PROGRAM_FOLDER + @"\BackupSettings\MP_TV_Version.txt"
                if (File.Exists(TV_PROGRAM_FOLDER + @"\BackupSettings\MP_TV_Version.txt"))
                {
                    try
                    {
                        File.Delete(TV_PROGRAM_FOLDER + @"\BackupSettings\MP_TV_Version.txt");
                    }
                    catch { }
                }
                //end legacy processing


                File.Copy("RestartSetupTV.exe", TV_PROGRAM_FOLDER + @"\BackupSettings\RestartSetupTV.exe", true);
                textoutputdebug("RestartSetupTV.exe  copied to \n" + TV_PROGRAM_FOLDER + @"\BackupSettings\RestartSetupTV.exe" + "\n");

                //test during install for internet security confirmation by starting RestartSetupTV.exe TEST
                textoutputdebug("You may need to confirm the windows internet security message");
                Process proc = new Process();
                ProcessStartInfo startinfo = new ProcessStartInfo();
                startinfo.FileName = TV_PROGRAM_FOLDER + @"\BackupSettings\RestartSetupTV.exe"; ;
                startinfo.WorkingDirectory = "";
                startinfo.Arguments = "TEST";
                proc.StartInfo = startinfo;

                try
                {
                    proc.Start();
                }
                catch (Exception exc)
                {
                    textoutputdebug("Error: Could not execute command \n" + startinfo.FileName + " " + startinfo.Arguments);
                    textoutputdebug("Exception message was:\n" + exc.Message + "\n");
                    return;
                }
                textoutputdebug("Windows access rights security check completed" + "\n");


            }
            catch (Exception exc)
            {
                textoutputdebug("Error: Could not copy RestartSetupTV.exe to\n" + TV_PROGRAM_FOLDER + @"\BackupSettings\RestartSetupTV.exe");
                textoutputdebug("Exception message was:\n" + exc.Message + "\n");
                return;
            }

            try  //copy BackupSettingsTV.dll
            {

                //legacy support delete old version
                if (File.Exists(TV_PROGRAM_FOLDER + @"\Plugins\BackupSettings.dll") == true)
                {
                    File.Delete(TV_PROGRAM_FOLDER + @"\Plugins\BackupSettings.dll");
                    textoutputdebug("Old Version BackupSettings.dll deleted" + "\n");
                }

                // first delete dll
                if (File.Exists(TV_PROGRAM_FOLDER + @"\Plugins\BackupSettingsTV.dll") == true)
                    File.Delete(TV_PROGRAM_FOLDER + @"\Plugins\BackupSettingsTV.dll");


                //fileversion tvservice used for recognizing different versions 
                FileVersionInfo tvserviceFileVersionInfo = FileVersionInfo.GetVersionInfo(TV_PROGRAM_FOLDER + @"\TvService.exe");
                if (FileVersionComparison(tvserviceFileVersionInfo.FileVersion.ToString(), "1.0.1.0") >= (int)CompareFileVersion.Error)
                {
                    textoutputdebug("Your tv server version could not be identified  - aborting installation \n");
                    return;
                }


                if (FileVersionComparison(tvserviceFileVersionInfo.FileVersion.ToString(), "1.0.1.0") == (int)CompareFileVersion.Older)  //MP1.0final version if older than 1.0.1.0
                {
                    textoutputdebug("Your Media Portal version is no more supported by BackupSettings. Please upgrade to at least 1.0.1.0");
                    return;

                }
                else if (FileVersionComparison(tvserviceFileVersionInfo.FileVersion.ToString(), "1.0.3.0") == (int)CompareFileVersion.Older)  //1.0.1 or 1.0.2 version if TVservice is older than 1.0.3.0
                {
                    textoutputdebug("Installing TV1.0.1 Plugin version");
                    File.Copy(@"BackupSettingsTV_1.0.1\BackupSettingsTV.dll", TV_PROGRAM_FOLDER + @"\Plugins\BackupSettingsTV.dll", true);
                    textoutputdebug(@"BackupSettingsTV_1.0.1\BackupSettingsTV.dll  copied to " + "\n" + TV_PROGRAM_FOLDER + @"\Plugins\BackupSettingsTV.dll" + "\n");

                }
                else if (FileVersionComparison(tvserviceFileVersionInfo.FileVersion.ToString(), "1.0.5.0") == (int)CompareFileVersion.Older)  //1.1 alpha or 1.1 beta version if TVservice is older than 1.0.5.0
                {
                    textoutputdebug("Media Portal version 1.1alpha or 1.1 beta are no more supported");
                    textoutputdebug("Please upgrade to 1.1RC1 or newer");
                    return;
                }
                else if (FileVersionComparison(tvserviceFileVersionInfo.FileVersion.ToString(), "1.0.6.0") == (int)CompareFileVersion.Older)//latest 1.1RC1 if older than 1.0.6.0
                {
                    textoutputdebug("Installing TV1.1 RC1 Plugin version");
                    File.Copy(@"BackupSettingsTV_1.1RC1\BackupSettingsTV.dll", TV_PROGRAM_FOLDER + @"\Plugins\BackupSettingsTV.dll", true);
                    textoutputdebug(@"BackupSettingsTV_1.1RC1\BackupSettingsTV.dll  copied to " + "\n" + TV_PROGRAM_FOLDER + @"\Plugins\BackupSettingsTV.dll" + "\n");
                }
                else if (FileVersionComparison(tvserviceFileVersionInfo.FileVersion.ToString(), "1.1.6.0") == (int)CompareFileVersion.Older)//latest 1.1RC2 if older than 1.1.6.0
                {
                    textoutputdebug("Installing TV1.1 RC2 Plugin version");
                    File.Copy(@"BackupSettingsTV_1.1RC2\BackupSettingsTV.dll", TV_PROGRAM_FOLDER + @"\Plugins\BackupSettingsTV.dll", true);
                    textoutputdebug(@"BackupSettingsTV_1.1RC2\BackupSettingsTV.dll  copied to " + "\n" + TV_PROGRAM_FOLDER + @"\Plugins\BackupSettingsTV.dll" + "\n");
                }
                else if (FileVersionComparison(tvserviceFileVersionInfo.FileVersion.ToString(), "1.2.100.0") == (int)CompareFileVersion.Older)//latest 1.2 if older than 1.2.100.0
                {
                    textoutputdebug("Installing TV1.2 Plugin version");
                    File.Copy(@"BackupSettingsTV_1.2\BackupSettingsTV.dll", TV_PROGRAM_FOLDER + @"\Plugins\BackupSettingsTV.dll", true);
                    textoutputdebug(@"BackupSettingsTV_1.2\BackupSettingsTV.dll  copied to " + "\n" + TV_PROGRAM_FOLDER + @"\Plugins\BackupSettingsTV.dll" + "\n");
                }
                else //MP1.3 newer than 1.2.100.0
                {
                    textoutputdebug("Installing TV1.3 Plugin version");
                    File.Copy(@"BackupSettingsTV_1.3\BackupSettingsTV.dll", TV_PROGRAM_FOLDER + @"\Plugins\BackupSettingsTV.dll", true);
                    textoutputdebug(@"BackupSettingsTV_1.3\BackupSettingsTV.dll  copied to " + "\n" + TV_PROGRAM_FOLDER + @"\Plugins\BackupSettingsTV.dll" + "\n");
                }
                success = true;


            }
            catch (Exception exc)
            {
                textoutputdebug("Error: Could not copy BackupSettingsTV.dll to\n" + TV_PROGRAM_FOLDER + @"\Plugins\BackupSettingsTV.dll");
                textoutputdebug("Exception message was:\n" + exc.Message + "\n");
                textoutputdebug("Try to uninstall first and then reinstall");
                textoutputdebug("If it does not help reboot your computer");
                return;
            }

            try  //copy BackupSettings.pdf
            {
                File.Copy("BackupSettings.pdf", TV_PROGRAM_FOLDER + @"\BackupSettings\BackupSettings.pdf", true);
                textoutputdebug("BackupSettings.pdf  copied to \n" + TV_PROGRAM_FOLDER + @"\BackupSettings\BackupSettings.pdf" + "\n");
            }
            catch (Exception exc)
            {
                textoutputdebug("Error: Could not copy BackupSettings.pdf to\n" + TV_PROGRAM_FOLDER + @"\BackupSettings\BackupSettings.pdf");
                textoutputdebug("Exception message was:\n" + exc.Message + "\n");
                return;
            }

            if (success == true)
            {
                textoutputdebug("Plugin installation succeeded");
                textoutputdebug("You can start now the TV Server Configuration and enable the plugin\n");
            }
        }

        private void MP1Install()
        {
            textoutputdebug("Installing MediaPortal1 Plugin BackupSettingsMP in ");
            textoutputdebug(MP_PROGRAM_FOLDER + @"\plugins\process" + "\n");
            try //BackupSettingsMP.dll
            {
                //fileversion tvservice used for recognizing different versions 
                FileVersionInfo MediaPortalFileVersionInfo = FileVersionInfo.GetVersionInfo(MP_PROGRAM_FOLDER + @"\MediaPortal.exe");
                if (FileVersionComparison(MediaPortalFileVersionInfo.FileVersion.ToString(), "1.1.6.0") == (int)CompareFileVersion.Older)//MP1.0 if older than 1.1.6.0
                {
                    textoutputdebug("Installing MP1.0 Plugin version" + "\n");
                    File.Copy(@"BackupSettingsMP\BackupSettingsMP.dll", MP_PROGRAM_FOLDER + @"\plugins\process\BackupSettingsMP.dll", true);
                    textoutputdebug(@"BackupSettingsMP\BackupSettingsMP.dll  copied to" + "\n" + MP_PROGRAM_FOLDER + @"\plugins\process\BackupSettingsMP.dll" + "\n");
                }
                else //MP1.2 if newer than 1.1.6.0
                {
                    textoutputdebug("Installing MP1.2 Plugin version" + "\n");
                    File.Copy(@"BackupSettingsMP_1.2\BackupSettingsMP.dll", MP_PROGRAM_FOLDER + @"\plugins\process\BackupSettingsMP.dll", true);
                    textoutputdebug(@"BackupSettingsMP_1.2\BackupSettingsMP.dll  copied to" + "\n" + MP_PROGRAM_FOLDER + @"\plugins\process\BackupSettingsMP.dll" + "\n");
                }

            }
            catch (Exception exc)
            {
                textoutputdebug("Error: Could not copy BackupSettingsMP.dll to\n" + MP_PROGRAM_FOLDER + @"\plugins\process\BackupSettingsMP.dll");
                textoutputdebug("Exception message was:\n" + exc.Message + "\n");
                return;
            }
            try //BackupSettingsMP.exe
            {
                File.Copy("BackupSettingsMP.exe", MP_PROGRAM_FOLDER + @"\plugins\process\BackupSettingsMP.exe", true);
                textoutputdebug("BackupSettingsMP.exe  copied to \n" + MP_PROGRAM_FOLDER + @"\plugins\process\BackupSettingsMP.exe" + "\n");
            }
            catch (Exception exc)
            {
                textoutputdebug("Error: Could not copy BackupSettingsMP.exe to\n" + MP_PROGRAM_FOLDER + @"\plugins\process\BackupSettingsMP.exe");
                textoutputdebug("Exception message was:\n" + exc.Message + "\n");
                return;
            }
            try  //copy BackupSettings.pdf
            {
                if (Directory.Exists(MP_PROGRAM_FOLDER + @"\Docs") == false)
                    Directory.CreateDirectory(MP_PROGRAM_FOLDER + @"\Docs");
                File.Copy("BackupSettings.pdf", MP_PROGRAM_FOLDER + @"\Docs\BackupSettings.pdf", true);
                textoutputdebug("BackupSettings.pdf  copied to \n" + MP_PROGRAM_FOLDER + @"\Docs\BackupSettings.pdf" + "\n");

            }
            catch (Exception exc)
            {
                textoutputdebug("Error: Could not copy BackupSettings.pdf to\n" + MP_PROGRAM_FOLDER + @"\Docs\BackupSettings.pdf");
                textoutputdebug("Exception message was:\n" + exc.Message + "\n");
                return;
            }

            textoutputdebug("Plugin installation succeeded");
            textoutputdebug("You can start now the Media Portal Configuration and enable the plugin in the section \"Other Plugins\" " + "\n");
                            

        }

        private void MP2ClientInstall()
        {
            textoutputdebug("Installing MediaPortal2 Client Plugin BackupSettingsMP2 in ");
            textoutputdebug(MP2_PROGRAM_FOLDER + @"\Plugins" + "\n");


            //create directories first
            try
            {
                if (Directory.Exists(instpaths.DIR_MP2_Plugins + @"\BackupSettings") == false)
                    Directory.CreateDirectory(instpaths.DIR_MP2_Plugins + @"\BackupSettings");

                if (Directory.Exists(instpaths.DIR_MP2_Plugins + @"\BackupSettings\Language") == false)
                    Directory.CreateDirectory(instpaths.DIR_MP2_Plugins + @"\BackupSettings\Language");

                if (Directory.Exists(MP2_PROGRAM_FOLDER + @"\Docs") == false)
                    Directory.CreateDirectory(MP2_PROGRAM_FOLDER + @"\Docs");
            }
            catch (Exception exc)
            {
                textoutputdebug("Error: Could not create directories");
                textoutputdebug("Exception message was:\n" + exc.Message + "\n");
                return;
            }

            try  //copy MP2 plugin
            {

                File.Copy(@"BackupSettingsMP2\BackupSettingsMP2.dll", instpaths.DIR_MP2_Plugins + @"\BackupSettings\BackupSettingsMP2.dll", true);
                textoutputdebug(@"BackupSettingsMP2\BackupSettingsMP2.dll  copied to \n" + instpaths.DIR_MP2_Plugins + @"\BackupSettings\BackupSettingsMP2.dll" + "\n");

                File.Copy(@"BackupSettingsMP2\plugin.xml", instpaths.DIR_MP2_Plugins + @"\BackupSettings\plugin.xml", true);
                textoutputdebug(@"BackupSettingsMP2\plugin.xml  copied to \n" + instpaths.DIR_MP2_Plugins + @"\BackupSettings\plugin.xml" + "\n");

                DirectoryInfo mydirinfo = new DirectoryInfo(@"BackupSettingsMP2\Language");
                FileInfo[] files = mydirinfo.GetFiles("*.xml");
                foreach (FileInfo file in files)
                {
                    try
                    {
                        File.Copy(file.FullName, instpaths.DIR_MP2_Plugins + @"\BackupSettings\Language\" + file.Name, true);
                        textoutputdebug(file.Name + "copied to " + @"\BackupSettings\Language\" + file.Name);
                    }
                    catch (Exception exc)
                    {
                        textoutputdebug("Error in copying Language\\" + file.Name + " to " + @"\BackupSettings\Language\" + file.Name + " exception is: \n" + exc.Message + "\n");
                    }
                }

            }
            catch (Exception exc)
            {
                textoutputdebug("Error: Could not copy BackupSettingsMP2 plugin to\n" + instpaths.DIR_MP2_Plugins + @"\BackupSettings");
                textoutputdebug("Exception message was:\n" + exc.Message + "\n");
                return;
            }

            try //BackupSettingsMP.exe
            {
                File.Copy("BackupSettingsMP.exe", instpaths.DIR_MP2_Plugins + @"\BackupSettings\BackupSettingsMP.exe", true);
                textoutputdebug("BackupSettingsMP.exe  copied to \n" + instpaths.DIR_MP2_Plugins + @"\BackupSettings\BackupSettingsMP.exe" + "\n");
            }
            catch (Exception exc)
            {
                textoutputdebug("Error: Could not copy BackupSettingsMP.exe to\n" + instpaths.DIR_MP2_Plugins + @"\BackupSettings\BackupSettingsMP.exe");
                textoutputdebug("Exception message was:\n" + exc.Message + "\n");
                return;
            }

            try  //copy BackupSettings.pdf
            {

                File.Copy("BackupSettings.pdf", MP2_PROGRAM_FOLDER + @"\Docs\BackupSettings.pdf", true);
                textoutputdebug("BackupSettings.pdf  copied to \n" + MP2_PROGRAM_FOLDER + @"\Docs\BackupSettings.pdf" + "\n");

            }
            catch (Exception exc)
            {
                textoutputdebug("Error: Could not copy BackupSettings.pdf to\n" + MP2_PROGRAM_FOLDER + @"\Docs\BackupSettings.pdf");
                textoutputdebug("Exception message was:\n" + exc.Message + "\n");
                return;
            }

                            
        }

        private void MP2ServerInstall()
        {
            textoutputdebug("Installing MediaPortal2 Server Plugin BackupSettingsSV2 in ");
            textoutputdebug(SV2_PROGRAM_FOLDER + @"\Plugins" + "\n");
            //create directories first
            try
            {
                if (Directory.Exists(instpaths.DIR_SV2_Plugins + @"\BackupSettings") == false)
                {
                    Directory.CreateDirectory(instpaths.DIR_SV2_Plugins + @"\BackupSettings");
                    textoutputdebug("Created folder " + instpaths.DIR_SV2_Plugins + @"\BackupSettings" + "\n");
                }

                if (Directory.Exists(SV2_PROGRAM_FOLDER + @"\Docs") == false)
                {
                    Directory.CreateDirectory(SV2_PROGRAM_FOLDER + @"\Docs");
                    textoutputdebug("Created folder " + SV2_PROGRAM_FOLDER + @"\Docs" + "\n");
                }
            }
            catch (Exception exc)
            {
                textoutputdebug("Error: Could not create directories");
                textoutputdebug("Exception message was:\n" + exc.Message + "\n");
                return;
            }


            try //BackupSettingsMP.exe
            {
                File.Copy("BackupSettingsMP.exe", instpaths.DIR_SV2_Plugins + @"\BackupSettings\BackupSettingsMP.exe", true);
                textoutputdebug("BackupSettingsMP.exe  copied to \n" + instpaths.DIR_SV2_Plugins + @"\BackupSettings\BackupSettingsMP.exe" + "\n");
            }
            catch (Exception exc)
            {
                textoutputdebug("Error: Could not copy BackupSettingsMP.exe to\n" + instpaths.DIR_SV2_Plugins + @"\BackupSettings\BackupSettingsMP.exe");
                textoutputdebug("Exception message was:\n" + exc.Message + "\n");
                return;
            }

            try  //copy BackupSettings.pdf
            {
                File.Copy("BackupSettings.pdf", SV2_PROGRAM_FOLDER + @"\Docs\BackupSettings.pdf", true);
                textoutputdebug("BackupSettings.pdf  copied to \n" + SV2_PROGRAM_FOLDER + @"\Docs\BackupSettings.pdf" + "\n");
            }
            catch (Exception exc)
            {
                textoutputdebug("Error: Could not copy BackupSettings.pdf to\n" + SV2_PROGRAM_FOLDER + @"\Docs\BackupSettings.pdf");
                textoutputdebug("Exception message was:\n" + exc.Message + "\n");
                return;
            }
        }

        private void buttonuninstall_Click(object sender, EventArgs e)
        {
            uninstall();
        }

        private void uninstall()
        {
            UpdatePathVariables();
            UpdateGUI();


            if ((checkBoxTV.Checked) && (Directory.Exists(TV_PROGRAM_FOLDER) == true) && (Directory.Exists(TV_USER_FOLDER) == true) && (TV_PROGRAM_FOLDER != "") && (TV_USER_FOLDER != ""))
            {
                if (UNINSTALL == false)
                {
                    switch (MessageBox.Show("Do you want to uninstall the BackupSettings TV Server Plugin? ", "Tv Server BackupSettings Plugin Deinstallation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    {
                        case DialogResult.Yes:
                            {
                                // "Yes" processing


                                break;
                            }
                        case DialogResult.No:
                            {
                                // "No" processing
                                textoutputdebug("Uninstall aborted by user\n");
                                return;
                            }
                    }
                }

                //check for setupTv.exe
                Process[] sprocs = Process.GetProcessesByName("SetupTv");
                foreach (Process sproc in sprocs) // loop is only executed if Media portal is running
                {
                    textoutputdebug("You need to close Tv Server Configuration before you uninstall the plugin\n");
                    MessageBox.Show("You need to close Tv Server Configuration before you uninstall the plugin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                Process[] tprocs = null;
                tprocs = Process.GetProcessesByName("TVService");
                bool TvserviceRunning = false;
                foreach (Process tproc in tprocs)
                {
                    TvserviceRunning = true;
                }



                //Stopping tv service
                if (TvserviceRunning == true)
                {
                    StopTvService();                    
                }
                else
                {
                    textoutputdebug("TV Service is not running" + "\n");
                }

                TvServerUninstall();

                //Starting tv service
                if (TvserviceRunning == true)
                {
                    StartTvService();                    
                }
            }//end tvserver deinstallation


            if ((checkBoxMP.Checked) && (Directory.Exists(MP_PROGRAM_FOLDER) == true) && (Directory.Exists(MP_USER_FOLDER) == true) && (MP_PROGRAM_FOLDER != "") && (MP_USER_FOLDER != ""))
            {
                if (UNINSTALL == false)
                {
                    switch (MessageBox.Show("Do you want to uninstall the BackupSettingsMP MediaPortal1 Plugin? ", "MediaPortal1 BackupSettings Plugin Deinstallation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    {
                        case DialogResult.Yes:
                            {
                                // "Yes" processing


                                break;
                            }
                        case DialogResult.No:
                            {
                                // "No" processing
                                textoutputdebug("Uninstall aborted by user\n");
                                return;
                            }
                    }
                }

                //check for MediaPortal.exe
                Process[] sprocs = Process.GetProcessesByName("MediaPortal");
                foreach (Process sproc in sprocs) // loop is only executed if Media portal is running
                {
                    textoutputdebug("You need to close MediaPortal before you uninstall the plugin\n");
                    MessageBox.Show("You need to close MediaPortal before you uninstall the plugin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MP1Uninstall();

            }//end MP1 deinstallation

            if ((checkBoxMP2C.Checked) && (Directory.Exists(MP2_PROGRAM_FOLDER) == true) && (Directory.Exists(MP2_USER_FOLDER) == true) && (MP2_PROGRAM_FOLDER != "") && (MP2_USER_FOLDER != ""))
            {
                if (UNINSTALL == false)
                {
                    switch (MessageBox.Show("Do you want to uninstall the BackupSettingsMP2 MediaPortal2 Client Plugin? ", "MediaPortal2 Client BackupSettings Plugin Deinstallation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    {
                        case DialogResult.Yes:
                            {
                                // "Yes" processing


                                break;
                            }
                        case DialogResult.No:
                            {
                                // "No" processing
                                textoutputdebug("Uninstall aborted by user\n");
                                return;
                            }
                    }
                }

                //check for MP2-Client.exe
                Process[] sprocs = Process.GetProcessesByName("MP2-Client");
                foreach (Process sproc in sprocs) // loop is only executed if Media portal is running
                {
                    textoutputdebug("You need to close the MediaPortal2 Client before you uninstall the plugin\n");
                    MessageBox.Show("You need to close the MediaPortal2 Client before you uninstall the plugin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MP2ClientUninstall();

                
            }//end MP2 Client deinstallation

            if ((checkBoxMP2S.Checked) && (Directory.Exists(SV2_PROGRAM_FOLDER) == true) && (Directory.Exists(SV2_USER_FOLDER) == true) && (SV2_PROGRAM_FOLDER != "") && (SV2_USER_FOLDER != ""))
            {
                if (UNINSTALL == false)
                {
                    switch (MessageBox.Show("Do you want to uninstall the BackupSettingsSV2 MediaPortal2 Server Plugin? ", "MediaPortal2 Server BackupSettings Plugin Deinstallation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    {
                        case DialogResult.Yes:
                            {
                                // "Yes" processing


                                break;
                            }
                        case DialogResult.No:
                            {
                                // "No" processing
                                textoutputdebug("Uninstall aborted by user\n");
                                return;
                            }
                    }
                }

                /* not needed because no specific server plugin
                //check for MP2-Server.exe
                Process[] sprocs = Process.GetProcessesByName("MP2-Server");
                foreach (Process sproc in sprocs) // loop is only executed if Media portal is running
                {
                    textoutputdebug("You need to close the MediaPortal2 Server before you uninstall the plugin\n");
                    MessageBox.Show("You need to close the MediaPortal2 Server before you uninstall the plugin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }*/

                MP2ServerUninstall();
                
            }//end MP2 Server deinstallation

            textoutputdebug("Plugin deinstallation finished\n");
        }

        private void TvServerUninstall()
        {
            try  //uninstall /BackupSettings
            {
                if (File.Exists(TV_PROGRAM_FOLDER + @"\BackupSettings\RestartSetupTV.exe") == true)
                {
                    File.Delete(TV_PROGRAM_FOLDER + @"\BackupSettings\RestartSetupTV.exe");
                    textoutputdebug("Deleting " + TV_PROGRAM_FOLDER + @"\BackupSettings\RestartSetupTV.exe " + " \n");
                }
                if (File.Exists(TV_USER_FOLDER + @"\BackupSettings\Status.txt") == true)
                {
                    File.Delete(TV_USER_FOLDER + @"\BackupSettings\Status.txt");
                    textoutputdebug("Deleting " + TV_USER_FOLDER + @"\BackupSettings\Status.txt " + " \n");
                }

                if (File.Exists(TV_USER_FOLDER + @"\BackupSettings\ImportPostprocessing.txt.bak.txt") == true)
                {
                    File.Delete(TV_USER_FOLDER + @"\BackupSettings\ImportPostprocessing.txt.bak.txt");
                    textoutputdebug("Deleting " + TV_USER_FOLDER + @"\BackupSettings\ImportPostprocessing.txt.bak.txt  " + "\n");
                }
                if (File.Exists(TV_PROGRAM_FOLDER + @"\BackupSettings\BackupSettings.pdf") == true)
                {
                    File.Delete(TV_PROGRAM_FOLDER + @"\BackupSettings\BackupSettings.pdf");
                    textoutputdebug("Deleting " + TV_PROGRAM_FOLDER + @"\BackupSettings\BackupSettings.pdf " + " \n");
                }


                if (File.Exists(TV_PROGRAM_FOLDER + @"\Plugins\BackupSettingsTV.dll") == true)
                {
                    File.Delete(TV_PROGRAM_FOLDER + @"\Plugins\BackupSettingsTV.dll");
                    textoutputdebug("Deleting " + TV_PROGRAM_FOLDER + @"\Plugins\BackupSettingsTV.dll" + "  \n");
                }

                if (File.Exists(TV_USER_FOLDER + @"\BackupSettingScripts\AutoRepair.bat") == true)
                {
                    File.Delete(TV_USER_FOLDER + @"\BackupSettingScripts\AutoRepair.bat");
                    textoutputdebug("Deleting " + TV_USER_FOLDER + @"\BackupSettingScripts\AutoRepair.bat" + "  \n");
                }

                try
                {
                    Directory.Delete(TV_PROGRAM_FOLDER + @"\BackupSettings");
                }
                catch //do nothing
                {
                }
                if (Directory.Exists(TV_PROGRAM_FOLDER + @"\BackupSettings") == false)
                {
                    textoutputdebug("Deleting " + TV_PROGRAM_FOLDER + @"\BackupSettings" + "  \n");
                }
                else
                {
                    textoutputdebug("Directory " + TV_PROGRAM_FOLDER + @"\BackupSettings  is not empty - not deleted" + "\n");
                }

                try
                {
                    Directory.Delete(TV_USER_FOLDER + @"\BackupSettings");
                }
                catch //do nothing
                {
                }
                if (Directory.Exists(TV_USER_FOLDER + @"\BackupSettings") == false)
                {
                    textoutputdebug("Deleting " + TV_USER_FOLDER + @"\BackupSettings" + "  \n");
                }
                else
                {
                    textoutputdebug("Directory " + TV_USER_FOLDER + @"\BackupSettings  is not empty - not deleted" + "\n");
                }


                //Scripts
                try
                {
                    Directory.Delete(TV_USER_FOLDER + @"\BackupSettingScripts");
                }
                catch //do nothing
                {
                }
                if (Directory.Exists(TV_USER_FOLDER + @"\BackupSettingScripts") == false)
                {
                    textoutputdebug("Deleting " + TV_USER_FOLDER + @"\BackupSettingScripts" + "  \n");
                }
                else
                {
                    textoutputdebug("Directory " + TV_USER_FOLDER + @"\BackupSettingScript could not be deleted" + "\n");
                }

                textoutputdebug("Tv Server plugin has been uninstalled");
            }
            catch (Exception exc)
            {
                textoutputdebug("Error: Uninstalling BackupSettingsTV caused exception \n" + exc.Message);
            }
        }

        private void MP1Uninstall()
        {
            try //uninstall BackupSettingsMP.dll
            {
                if (File.Exists(MP_PROGRAM_FOLDER + @"\plugins\process\BackupSettingsMP.dll") == true)
                {
                    File.Delete(MP_PROGRAM_FOLDER + @"\plugins\process\BackupSettingsMP.dll");
                    textoutputdebug("Deleting " + MP_PROGRAM_FOLDER + @"\plugins\process\BackupSettingsMP.dll" + "  \n");
                }
                if (File.Exists(MP_PROGRAM_FOLDER + @"\plugins\process\BackupSettingsMP.exe") == true)
                {
                    File.Delete(MP_PROGRAM_FOLDER + @"\plugins\process\BackupSettingsMP.exe");
                    textoutputdebug("Deleting " + MP_PROGRAM_FOLDER + @"\plugins\process\BackupSettingsMP.exe" + "  \n");
                }
                if (File.Exists(MP_PROGRAM_FOLDER + @"\Docs\BackupSettings.pdf") == true)
                {
                    File.Delete(MP_PROGRAM_FOLDER + @"\Docs\BackupSettings.pdf");
                    textoutputdebug("Deleting " + MP_PROGRAM_FOLDER + @"\Docs\BackupSettings.pdf" + "  \n");
                }

                //textoutputdebug("BackupSettingsMP.xml="+MP_USER_FOLDER + @"\BackupSettingsMP.xml");

                if (File.Exists(MP_USER_FOLDER + @"\BackupSettingsMP.xml") == true)
                {
                    File.Delete(MP_USER_FOLDER + @"\BackupSettingsMP.xml");
                    textoutputdebug("Deleting " + MP_USER_FOLDER + @"\BackupSettingsMP.xml" + "  \n");
                }

                if (File.Exists(MP_USER_FOLDER + @"\BackupSettingsMP.xml.bak") == true)
                {
                    File.Delete(MP_USER_FOLDER + @"\BackupSettingsMP.xml.bak");
                    textoutputdebug("Deleting " + MP_USER_FOLDER + @"\BackupSettingsMP.xml.bak" + "  \n");
                }

                if (File.Exists(MP_USER_FOLDER + @"\BackupSettings\BackupSettingsStatus.txt") == true)
                {
                    File.Delete(MP_USER_FOLDER + @"\BackupSettings\BackupSettingsStatus.txt");
                    textoutputdebug("Deleting " + MP_USER_FOLDER + @"\BackupSettings\BackupSettingsStatus.txt" + "  \n");
                }

                try
                {
                    Directory.Delete(MP_USER_FOLDER + @"\BackupSettings");
                }
                catch //do nothing
                {
                }
                if (Directory.Exists(MP_USER_FOLDER + @"\BackupSettings") == false)
                {
                    textoutputdebug("Deleting " + MP_USER_FOLDER + @"\BackupSettings" + "  \n");
                }
                else
                {
                    textoutputdebug("Directory " + MP_USER_FOLDER + @"\BackupSettings  is not empty - not deleted" + "\n");
                }

                textoutputdebug("MediaPortal plugin has been uninstalled");

            }
            catch (Exception exc)
            {
                textoutputdebug("Error: Uninstalling BackupSettingsMP caused exception \n" + exc.Message);
            }

        }

        private void MP2ClientUninstall()
        {
            try //uninstall 
            {
                if (File.Exists(instpaths.DIR_MP2_Plugins + @"\BackupSettings\BackupSettingsMP2.dll") == true)
                {
                    File.Delete(instpaths.DIR_MP2_Plugins + @"\BackupSettings\BackupSettingsMP2.dll");
                    textoutputdebug("Deleting " + instpaths.DIR_MP2_Plugins + @"\BackupSettings\BackupSettingsMP2.dll" + "  \n");
                }
                if (File.Exists(instpaths.DIR_MP2_Plugins + @"\BackupSettings\plugin.xml") == true)
                {
                    File.Delete(instpaths.DIR_MP2_Plugins + @"\BackupSettings\plugin.xml");
                    textoutputdebug("Deleting " + instpaths.DIR_MP2_Plugins + @"\BackupSettings\plugin.xml" + "  \n");
                }

                string BackupSettings_DIR = instpaths.DIR_MP2_Config + @"\" + System.Environment.UserName + @"\BackupSettings";
                if (File.Exists(BackupSettings_DIR + @"\Status.txt") == true)
                {
                    File.Delete(BackupSettings_DIR + @"\Status.txt");
                    textoutputdebug("Deleting " + BackupSettings_DIR + @"\Status.txt" + "  \n");
                }

                if (File.Exists(BackupSettings_DIR + @"\BackupSettingsMP.xml") == true)
                {
                    File.Delete(BackupSettings_DIR + @"\BackupSettingsMP.xml");
                    textoutputdebug("Deleting " + BackupSettings_DIR + @"\BackupSettingsMP.xml" + "  \n");
                }

                if (File.Exists(BackupSettings_DIR + @"\BackupSettingsMP.xml.bak") == true)
                {
                    File.Delete(BackupSettings_DIR + @"\BackupSettingsMP.xml.bak");
                    textoutputdebug("Deleting " + BackupSettings_DIR + @"\BackupSettingsMP.xml.bak" + "  \n");
                }                

                if (File.Exists(instpaths.DIR_MP2_Plugins + @"\BackupSettings\BackupSettingsMP.exe") == true)
                {
                    File.Delete(instpaths.DIR_MP2_Plugins + @"\BackupSettings\BackupSettingsMP.exe");
                    textoutputdebug("Deleting " + instpaths.DIR_MP2_Plugins + @"\BackupSettings\BackupSettingsMP.exe" + "  \n");
                }

                if (File.Exists(MP2_PROGRAM_FOLDER + @"\Docs\BackupSettings.pdf") == true)
                {
                    File.Delete(MP2_PROGRAM_FOLDER + @"\Docs\BackupSettings.pdf");
                    textoutputdebug("Deleting " + MP2_PROGRAM_FOLDER + @"\Docs\BackupSettings.pdf" + "  \n");
                }

                DirectoryInfo mydirinfo = new DirectoryInfo(instpaths.DIR_MP2_Plugins + @"\BackupSettings\Language");
                FileInfo[] files = mydirinfo.GetFiles("*.xml");
                foreach (FileInfo file in files)
                {
                    try
                    {
                        File.Delete(instpaths.DIR_MP2_Plugins + @"\BackupSettings\Language\" + file.Name);
                        textoutputdebug("Deleting " + instpaths.DIR_MP2_Plugins + @"\BackupSettings\Language\" + file.Name + "  \n");
                    }
                    catch (Exception exc)
                    {
                        textoutputdebug("Error in deleting Language\\" + file.Name + " to " + instpaths.DIR_MP2_Plugins + @"\BackupSettings\Language\" + file.Name + " exception is: \n" + exc.Message + "\n");
                    }
                }


                //remove language folder
                try
                {
                    Directory.Delete(instpaths.DIR_MP2_Plugins + @"\BackupSettings\Language");
                }
                catch //do nothing
                {
                }
                if (Directory.Exists(instpaths.DIR_MP2_Plugins + @"\BackupSettings\Language") == false)
                {
                    textoutputdebug("Deleting " + instpaths.DIR_MP2_Plugins + @"\BackupSettings\Language" + "  \n");
                }
                else
                {
                    textoutputdebug("Directory " + instpaths.DIR_MP2_Plugins + @"\BackupSettings\Language is not empty - not deleted" + "\n");
                }


                //remove plugin folder
                try
                {
                    Directory.Delete(instpaths.DIR_MP2_Plugins + @"\BackupSettings");
                }
                catch //do nothing
                {
                }
                if (Directory.Exists(instpaths.DIR_MP2_Plugins + @"\BackupSettings") == false)
                {
                    textoutputdebug("Deleting " + instpaths.DIR_MP2_Plugins + @"\BackupSettings" + "  \n");
                }
                else
                {
                    textoutputdebug("Directory " + instpaths.DIR_MP2_Plugins + @"\BackupSettings is not empty - not deleted" + "\n");
                }

                textoutputdebug("MediaPortal2 Client plugin has been uninstalled");

            }
            catch (Exception exc)
            {
                textoutputdebug("Error: Uninstalling BackupSettingsMP caused exception \n" + exc.Message);
            }

            
        }

        private void MP2ServerUninstall()
        {
            if (File.Exists(instpaths.DIR_SV2_Plugins + @"\BackupSettings\BackupSettingsMP.exe") == true)
            {
                File.Delete(instpaths.DIR_SV2_Plugins + @"\BackupSettings\BackupSettingsMP.exe");
                textoutputdebug("Deleting " + instpaths.DIR_SV2_Plugins + @"\BackupSettings\BackupSettingsMP.exe" + "  \n");
            }

            if (File.Exists(SV2_PROGRAM_FOLDER + @"\Docs\BackupSettings.pdf") == true)
            {
                File.Delete(SV2_PROGRAM_FOLDER + @"\Docs\BackupSettings.pdf");
                textoutputdebug("Deleting " + SV2_PROGRAM_FOLDER + @"\Docs\BackupSettings.pdf" + "  \n");
            }

            string BackupSettings_DIR = instpaths.DIR_SV2_Config + @"\" + System.Environment.UserName + @"\BackupSettings";
            if (File.Exists(BackupSettings_DIR + @"\Status.txt") == true)
            {
                File.Delete(BackupSettings_DIR + @"\Status.txt");
                textoutputdebug("Deleting " + BackupSettings_DIR + @"\Status.txt" + "  \n");
            }

            if (File.Exists(BackupSettings_DIR + @"\BackupSettingsMP.xml") == true)
            {
                File.Delete(BackupSettings_DIR + @"\BackupSettingsMP.xml");
                textoutputdebug("Deleting " + BackupSettings_DIR + @"\BackupSettingsMP.xml" + "  \n");
            }

            if (File.Exists(BackupSettings_DIR + @"\BackupSettingsMP.xml.bak") == true)
            {
                File.Delete(BackupSettings_DIR + @"\BackupSettingsMP.xml.bak");
                textoutputdebug("Deleting " + BackupSettings_DIR + @"\BackupSettingsMP.xml.bak" + "  \n");
            }


            if (File.Exists(instpaths.DIR_SV2_Plugins + @"\BackupSettings\BackupSettingsMP.exe") == true)
            {
                File.Delete(instpaths.DIR_SV2_Plugins + @"\BackupSettings\BackupSettingsMP.exe");
                textoutputdebug("Deleting " + instpaths.DIR_SV2_Plugins + @"\BackupSettings\BackupSettingsMP.exe" + "  \n");
            }

            if (File.Exists(SV2_PROGRAM_FOLDER + @"\Docs\BackupSettings.pdf") == true)
            {
                File.Delete(SV2_PROGRAM_FOLDER + @"\Docs\BackupSettings.pdf");
                textoutputdebug("Deleting " + SV2_PROGRAM_FOLDER + @"\Docs\BackupSettings.pdf" + "  \n");
            }

            //remove plugin folder
            try
            {
                Directory.Delete(instpaths.DIR_SV2_Plugins + @"\BackupSettings");
            }
            catch //do nothing
            {
            }
            if (Directory.Exists(instpaths.DIR_SV2_Plugins + @"\BackupSettings") == false)
            {
                textoutputdebug("Deleting " + instpaths.DIR_SV2_Plugins + @"\BackupSettings" + "  \n");
            }
            else
            {
                textoutputdebug("Directory " + instpaths.DIR_SV2_Plugins + @"\BackupSettings is not empty - not deleted" + "\n");
            }

            textoutputdebug("MediaPortal2 Server plugin has been uninstalled");
        }

        private void StopTvService()
        {
            textoutputdebug("Stopping TV Service");
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
                textoutputdebug("Error: Could not execute command \n" + startinfo.FileName + " " + startinfo.Arguments);
                textoutputdebug("Exception message was:\n" + exc.Message + "\n");
                return;
            }
            proc.WaitForExit(1000 * 60); //wait 1 minutes maximum
            if (proc.HasExited == true)
            {
                if (proc.ExitCode != 0)
                {
                    textoutputdebug("Tv Service error: Stopping Tv service caused an error code " + proc.ExitCode);

                    //textoutputdebug("Reboot and repeat installation\n");
                    //return;
                }
                else
                {
                    textoutputdebug("TV Service Stopped" + "\n");
                }
            }
            else
            {
                textoutputdebug("Tv Service timeout error: Could not stop Tv service within time limit");
                //textoutputdebug("Reboot and repeat installation\n");
                //return;
            }
        }

        private void StartTvService()
        {
            textoutputdebug("Starting TV Service");
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
                textoutputdebug("Error: Could not execute command \n" + startinfo2.FileName + " " + startinfo2.Arguments);
                textoutputdebug("Exception message was:\n" + exc.Message + "\n");
                return;
            }
            proc2.WaitForExit(1000 * 60 * 3); //wait 3 minutes maximum
            if (proc2.HasExited == true)
            {
                if (proc2.ExitCode != 0)
                {
                    textoutputdebug("Tv Service error: Starting Tv service caused an error code " + proc2.ExitCode);

                    textoutputdebug("Reboot and check the TV service status from the TV server configuration tool\n");
                    return;
                }
            }
            else
            {
                textoutputdebug("Tv Service timeout error: Could not stop Tv service within time limit\n");
                textoutputdebug("Reboot and check the TV service status from the TV server configuration tool\n");
                return;
            }
            textoutputdebug("TV Service Started" + "\n");
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
            if (!File.Exists(destination))
            {
                try
                {
                    Directory.CreateDirectory(destination);
                }
                catch (Exception exc)
                {
                    textoutputdebug("DirectoryCopy Error: Could not create " + destination + " - Exception: " + exc.Message);
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
                                textoutputdebug("Copied: " + file.Name);
                            }

                        }
                        else if (overwrite == false) // and file does exist => do not copy
                        {
                            if (verbose)
                            {
                                textoutputdebug("Exists:" + file.Name);
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
                                    textoutputdebug("ReadOnly: " + file.Name);
                                }
                            }
                            else if ((attribute & FileAttributes.Hidden) == FileAttributes.Hidden)
                            {
                                if (verbose)
                                {
                                    textoutputdebug("Hidden: " + file.Name);
                                }
                            }
                            else if ((attribute & FileAttributes.System) == FileAttributes.System)
                            {
                                if (verbose)
                                {
                                    textoutputdebug("System: " + file.Name);
                                }
                            }
                            else
                            {
                                File.Copy(file.FullName, destination + "\\" + file.Name, true);
                                if (verbose)
                                {
                                    textoutputdebug("Copied: " + file.Name);
                                }
                            }
                        }

                    }
                    catch (Exception exc)
                    {
                        textoutputdebug("<RED>DirectoryCopy Error: Could not copy file " + file.FullName + " to " + destination + "\\" + file.Name + " - Exception:" + exc.Message);
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

        private int FileVersionComparison(string version1, string version2)
        {
            //valid return code: 
            //  1 version1 is newer than version2
            // -1 version1 is older than version2
            //  0 version1 is same as version2
            // 99 error occured

            //errorchecking version1
            string[] tokenarray1 = version1.Split('.');
            if ((tokenarray1.Length != 4) && (tokenarray1.Length != 3))
            {
                return (int)CompareFileVersion.Version1Error;
            }
            for (int i = 0; i < tokenarray1.Length; i++)
            {
                try
                {
                    int j = Convert.ToInt32(tokenarray1[i]);
                }
                catch
                {
                    return (int)CompareFileVersion.Version1String;
                }
            }

            //errorchecking version2
            string[] tokenarray2 = version2.Split('.');
            if ((tokenarray2.Length != 4) && (tokenarray2.Length != 3))
            {
                return (int)CompareFileVersion.Version2Error;
            }
            for (int i = 0; i < tokenarray2.Length; i++)
            {
                try
                {
                    int j = Convert.ToInt32(tokenarray2[i]);
                }
                catch
                {
                    return (int)CompareFileVersion.Version2String;
                }
            }

            if (Convert.ToInt32(tokenarray1[0]) > Convert.ToInt32(tokenarray2[0]))
            {
                return (int)CompareFileVersion.Newer;
            }
            else if (Convert.ToInt32(tokenarray1[0]) < Convert.ToInt32(tokenarray2[0]))
            {
                return (int)CompareFileVersion.Older;
            }
            else //same
            {
                if (Convert.ToInt32(tokenarray1[1]) > Convert.ToInt32(tokenarray2[1]))
                {
                    return (int)CompareFileVersion.Newer;
                }
                else if (Convert.ToInt32(tokenarray1[1]) < Convert.ToInt32(tokenarray2[1]))
                {
                    return (int)CompareFileVersion.Older;
                }
                else //same
                {
                    if (Convert.ToInt32(tokenarray1[2]) > Convert.ToInt32(tokenarray2[2]))
                    {
                        return (int)CompareFileVersion.Newer;
                    }
                    else if (Convert.ToInt32(tokenarray1[2]) < Convert.ToInt32(tokenarray2[2]))
                    {
                        return (int)CompareFileVersion.Older;
                    }
                    else //same
                    {
                        if (tokenarray1.Length == 3)
                        {
                            return (int)CompareFileVersion.Equal;
                        }

                        if (Convert.ToInt32(tokenarray1[3]) > Convert.ToInt32(tokenarray2[3]))
                        {
                            return (int)CompareFileVersion.Newer;
                        }
                        else if (Convert.ToInt32(tokenarray1[3]) < Convert.ToInt32(tokenarray2[3]))
                        {
                            return (int)CompareFileVersion.Older;
                        }
                        else //same
                        {
                            return (int)CompareFileVersion.Equal;
                        }
                    }
                }
            }

        }

        private void CreateTVServerAutoRepairBatch()
        {
            if (File.Exists(TV_USER_FOLDER + @"\BackupSettingScripts\AutoRepair.bat"))
            {
                textoutputdebug("AutoRepair script exists already - not created");
                return;
            }

             textoutputdebug("Trying to create AutoRepair script for Tv server database");

            try
            {               
                if (Directory.Exists(TV_USER_FOLDER + @"\BackupSettingScripts") == false)
                {
                    Directory.CreateDirectory(TV_USER_FOLDER + @"\BackupSettingScripts");
                }
            }
            catch (Exception exc)
            {
                textoutputdebug("<RED>Could not create directory " + TV_USER_FOLDER + @"\BackupSettingScripts");
                textoutputdebug("<RED>Exception message is:" + exc.Message);
                return;
            }

            string sqlexedir = "";
            string PROGRAMFILES = Environment.GetEnvironmentVariable("PROGRAMFILES");
            //textoutputdebug("PROGRAMFILES=" + PROGRAMFILES);
            string PROGRAMFILESx86 = Environment.GetEnvironmentVariable("PROGRAMFILES(X86)");
            //textoutputdebug("PROGRAMFILESx86=" + PROGRAMFILESx86);
            //get mysql location

            


            if (File.Exists(TV_PROGRAM_FOLDER + @"..\..\MySQL\MySQL Server 5.6\bin\mysqlcheck.exe"))
            {
                sqlexedir = TV_PROGRAM_FOLDER + @"..\..\MySQL\MySQL Server 5.6\bin";
                //textoutputdebug("1: sqlexe=" + sqlexedir);
            }
            else if (File.Exists(PROGRAMFILES + @"\MySQL\MySQL Server 5.6\bin\mysqlcheck.exe"))
            {
                sqlexedir = PROGRAMFILES + @"\MySQL\MySQL Server 5.6\bin";
                //textoutputdebug("2: sqlexe=" + sqlexedir);
            }
            else if (File.Exists(PROGRAMFILESx86 + @"\MySQL\MySQL Server 5.6\bin\mysqlcheck.exe"))
            {
                sqlexedir = PROGRAMFILESx86 + @"\MySQL\MySQL Server 5.6\bin";
                //textoutputdebug("3: sqlexe=" + sqlexedir); 
            }
            else if (File.Exists(TV_PROGRAM_FOLDER + @"..\..\MySQL\MySQL Server 5.1\bin\mysqlcheck.exe"))
            {
                sqlexedir = TV_PROGRAM_FOLDER + @"..\..\MySQL\MySQL Server 5.1\bin";
                //textoutputdebug("1: sqlexe=" + sqlexedir);
            }
            else if (File.Exists(PROGRAMFILES + @"\MySQL\MySQL Server 5.1\bin\mysqlcheck.exe"))
            {
                sqlexedir = PROGRAMFILES + @"\MySQL\MySQL Server 5.1\bin";
                //textoutputdebug("2: sqlexe=" + sqlexedir);
            }
            else if (File.Exists(PROGRAMFILESx86 + @"\MySQL\MySQL Server 5.1\bin\mysqlcheck.exe"))
            {
                sqlexedir = PROGRAMFILESx86 + @"\MySQL\MySQL Server 5.1\bin";
                //textoutputdebug("3: sqlexe=" + sqlexedir); 
            }
            else
            {
                textoutputdebug("Could not find mysqlcheck.exe file- cannot create script");
                return;
            }

            //textoutputdebug("sqlexedir=" + sqlexedir);

            //get gentle.config data
            if (File.Exists(TV_USER_FOLDER + @"\Gentle.config") == false)
            {
                textoutputdebug("Could not find Gentle.config file " + TV_USER_FOLDER + @"\Gentle.config - cannot create script");
                return;
            }

            XmlDocument doc = new XmlDocument();
            string name = "";
            string connectionString = "";
            string Server = "";
            string Database = "";
            string User_ID = "";
            string Password = "";

            try
            {
                //read inputfile
                string[] inputlines = File.ReadAllLines(TV_USER_FOLDER + @"\Gentle.config");
                foreach (string line in inputlines)
                {
                    if (line.Contains("<DefaultProvider"))
                    {
                        string[] tokens = line.Split('"');
                        //textoutputdebug("tokens.Length=" + tokens.Length.ToString());
                        for (int i = 0; i < tokens.Length; i++)
                        {
                            //textoutputdebug("tokens["+i.ToString()+"]="+tokens[i]);
                        }

                        if (tokens.Length < 4)
                        {
                            textoutputdebug("<RED>Invalid line for providerstring ("+line+") in file " + TV_USER_FOLDER + @"\Gentle.config");
                            return;
                        }

                        name = tokens[1];

                        if (name == "MySQL")
                        {
                            connectionString = tokens[3];
                            tokens = connectionString.Split(';');
                            for (int i = 0; i < tokens.Length; i++)
                            {
                                if (tokens[i].StartsWith("Server="))
                                {
                                    Server = tokens[i].Replace("Server=", "");
                                    //textoutputdebug("Server=" + Server);
                                }
                                else if (tokens[i].StartsWith("Database="))
                                {
                                    Database = tokens[i].Replace("Database=", "");
                                    //textoutputdebug("Database=" + Database);
                                }
                                else if (tokens[i].StartsWith("User ID="))
                                {
                                    User_ID = tokens[i].Replace("User ID=", "");
                                    //textoutputdebug("User_ID=" + User_ID);
                                }
                                else if (tokens[i].StartsWith("Password="))
                                {
                                    Password = tokens[i].Replace("Password=", "");
                                    //textoutputdebug("Password=" + Password);
                                }
                            }
                            break;//done
                        }
                        else
                        {
                            textoutputdebug("<RED>Not using MySQL in providerstring (" + line + ") in file " + TV_USER_FOLDER + @"\Gentle.config - cannot create batch file");
                            return;
                        }

                    }
                }
            }
            catch (Exception exc)
            {
                textoutputdebug("<RED>Could not read file " + TV_USER_FOLDER + @"\Gentle.config");            
                textoutputdebug("<RED>Exception message is:" + exc.Message);
                return ;
            }

            //write batchfile
            try
            {
                if ((User_ID==string.Empty)||(Password==string.Empty))
                {
                    textoutputdebug("<RED>Could not read username or password in file " + TV_USER_FOLDER + @"\Gentle.config");
                    return;
                }
                textoutputdebug("Writing script file " + TV_USER_FOLDER + @"\BackupSettingScripts\AutoRepair.bat");
                            
                string batchfilestring = "REM AutoRepair.bat: automatic repair of MySQL Tv server data base \n\r ";
                batchfilestring += "cd \"" + sqlexedir + "\" \n\r ";
                batchfilestring += "mysqlcheck.exe -u "+User_ID+" -p"+Password+" --auto-repair --all-databases \n\r";

                File.WriteAllText(TV_USER_FOLDER + @"\BackupSettingScripts\AutoRepair.bat", batchfilestring);
            }
            catch (Exception exc)
            {
                textoutputdebug("<RED>Could not write file " + TV_USER_FOLDER + @"\BackupSettingScripts\AutoRepair.bat");
                textoutputdebug("<RED>Exception message is:" + exc.Message);
                return;
            }
        }


        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonReadme_Click(object sender, EventArgs e)
        {
            Process proc = new Process();
            ProcessStartInfo procstartinfo = new ProcessStartInfo();
            procstartinfo.FileName = "BackupSettings.pdf";
            procstartinfo.WorkingDirectory = System.Environment.CurrentDirectory;
            proc.StartInfo = procstartinfo;
            try
            {               
                proc.Start();
            }
            catch
            {
                MessageBox.Show("Could not open " + procstartinfo.WorkingDirectory+"\\"+procstartinfo.FileName, "Error");
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
            AutoDetect();
            UpdatePathVariables();
            UpdateGUI();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            //updating installation button visibility on main tab
            UpdateGUI();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }


        private void textoutputdebug(string textlines)
        {

            string text = "";

            if (listBox1 != null)
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

                        listBox1.Items.Add(pretext);

                        text = "+  " + text.Substring(linelength, text.Length - linelength);

                    }

                    listBox1.Items.Add(text);
                    if (listBox1.Items.Count > 10)
                        listBox1.TopIndex = listBox1.Items.Count - 9;

                    listBox1.Update();
                }
            }
        }

    }
}
