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
using Microsoft.Win32;

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

using TvEngine.PowerScheduler.Interfaces;
using TvEngine.Events;

using SetupTv.Sections;
using SetupTv;
using BackupSettingsPlugin;
using MediaPortal.Plugins;



namespace TvEngine
{
    
    [CLSCompliant(false)]
    public class BackupSettings : ITvServerPlugin
    {
        #region Members
        public string RESTART_SETUP_TV_EXE = "RestartSetupTV.exe";
        public string BACKUPSETTING_TV_DIR = "BackupSettings";
        public string STATUS_FILE = "Status.txt";
        public string INSTALLPATHS_FILE = "MP_TV_Installpaths.txt";
        public string POSTPROCESSING_FILE = "ImportPostprocessing.txt";

        public string AUTO_EXPORT_FILE = "AutoExportFile.txt";
        public string SCHEDULED_EXPORT_FILE = "Scheduler.txt";

        ITvServerEvent events = null;
        FileSystemWatcher ImportWatcher = new FileSystemWatcher();
        FileSystemWatcher AutomatedExport = new FileSystemWatcher();
        FileSystemWatcher ScheduledExport = new FileSystemWatcher();

        DateTime _NextBackup = DateTime.Now;



        int _recording = 0;
        bool _timeshifting = false;
        bool _useRecordingFlag = true;
        DateTime _RecordingFlagTime = DateTime.Now;

        //string TV_PROGRAM_FOLDER=".";
        string TV_USER_FOLDER = "NOT_DEFINED";
        string STATUS = "";
        bool DEBUG = false;
        //static bool BUSY = false;
        public static bool POSTPROCESSING = false;  //Global flag for postprocessing active
        public static bool WATCHERACTIVE = false; //Global static flag for FileSystemWatcher activated

        bool runpolling = true;
        
        #endregion


        #region ITvServerPlugin Members



        public string Name
        {
            get { return "BackupSettings"; }
        }

        public string Version
        {
            get { return "1.2.2.13"; }
        }

        public string Author
        {
            get { return "huha"; }
        }

        public bool MasterOnly
        {
            get { return false; }
        }

        

        [CLSCompliant(false)]
        public void Start(IController controller)
        {
            Log.Debug("BackupSettingsServer: Plugin BackupSettings active");

            
            events = GlobalServiceProvider.Instance.Get<ITvServerEvent>();
            events.OnTvServerEvent += new TvServerEventHandler(events_OnTvServerEvent);

            TvBusinessLayer layer = new TvBusinessLayer();
            Setting setting = null;

            //get debug setting
            setting = layer.GetSetting("Backup_SettingsSetup_debug", "false");
            if (setting.Value.ToLower() == "true")
            {
                DEBUG = true;
            }
            else
            {
                DEBUG = false;
            }

            //get next backup time
            _NextBackup = DateTime.Now.AddDays(30);//add one month default
            _RecordingFlagTime = DateTime.Now.AddHours(1.0); //add 1 hour to give time for setup

            setting = layer.GetSetting("Backup_SettingsNextBackup", DateTime.Now.ToString("yyyy-MM-dd_HH:mm", System.Globalization.CultureInfo.InvariantCulture));
            try
            {
                _NextBackup = DateTime.ParseExact(setting.Value, "yyyy-MM-dd_HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception exc)
            {
                Log.Error("NextBackup failed with exception: " + exc.Message);
            }

            //is host name stored in database for BackupSettingsMP2?
            setting = layer.GetSetting("BackupSettings_MachineName", "NONE");
            if (setting.Value != System.Environment.MachineName.ToString())
            {
                setting.Value = System.Environment.MachineName.ToString();
                setting.Persist();
            }


            //Get TV_USER_FOLDER
            TV_USER_FOLDER = layer.GetSetting("BackupSettings_TV_USER_FOLDER", "NOT_FOUND").Value;
            if ((File.Exists(TV_USER_FOLDER + @"\TvService.exe") == true) || (Directory.Exists(TV_USER_FOLDER) == false))
            {
                //autodetect paths
                InstallPaths instpaths = new InstallPaths();  //define new instance for folder detection
                instpaths.GetInstallPaths();
                TV_USER_FOLDER = instpaths.TV_USER_FOLDER;
                Log.Debug("TV server user folder detected at " + TV_USER_FOLDER);

                if ((File.Exists(TV_USER_FOLDER + @"\TvService.exe") == true) || (Directory.Exists(TV_USER_FOLDER) == false))
                {
                    Log.Error(@" TV server user folder does not exist - using C:\MediaPortal BackupSettings");
                    Log.Debug(@" TV server user folder does not exist - using C:\MediaPortal BackupSettings");
                    TV_USER_FOLDER = @"C:\MediaPortal BackupSettings";
                    if (Directory.Exists(TV_USER_FOLDER) == false)
                        Directory.CreateDirectory(TV_USER_FOLDER + @"\BackupSettings");
                }

                //store new TV_USER_FOLDER
                setting = layer.GetSetting("BackupSettings_TV_USER_FOLDER", "NOT_FOUND");
                setting.Value = TV_USER_FOLDER;
                setting.Persist();               
            }

            //enable textmessages from other classes by starting filewatchers

            if (Directory.Exists(TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR) == true)
            {
                if (File.Exists(TV_USER_FOLDER + @"\"+BACKUPSETTING_TV_DIR + @"\" + POSTPROCESSING_FILE) == true)  //delete old corrupt file
                {
                    File.Delete(TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + POSTPROCESSING_FILE);
                    Log.Debug("BackupSettings Server: File " + TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + POSTPROCESSING_FILE + " deleted");
                }
                ImportWatcher.Path = TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR;
                ImportWatcher.Created += new FileSystemEventHandler(importpostprocessing);
                ImportWatcher.Filter = POSTPROCESSING_FILE;
                ImportWatcher.EnableRaisingEvents = true;
                WATCHERACTIVE = true;
                Log.Debug("BackupSettings Server: Importwatcher activated for " + ImportWatcher.Path + "\\" + ImportWatcher.Filter);



                if (File.Exists(TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + AUTO_EXPORT_FILE) == true)  //delete old corrupt file
                {
                    File.Delete(TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + AUTO_EXPORT_FILE);
                    Log.Debug("BackupSettings Server: File " + TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + AUTO_EXPORT_FILE + " deleted");
                }
                AutomatedExport.Path = TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR;
                AutomatedExport.Created += new FileSystemEventHandler(AutomatedExportProcessing);
                AutomatedExport.Filter = AUTO_EXPORT_FILE;
                AutomatedExport.EnableRaisingEvents = true;
                Log.Debug("BackupSettings Server: AutomatedExport activated for " + AutomatedExport.Path + "\\" + AutomatedExport.Filter);


                if (File.Exists(TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + SCHEDULED_EXPORT_FILE) == true)  //delete old corrupt file
                {
                    File.Delete(TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + SCHEDULED_EXPORT_FILE);
                    Log.Debug("BackupSettings Server: File " + TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + SCHEDULED_EXPORT_FILE + " deleted");
                }
                ScheduledExport.Path = TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR;
                ScheduledExport.Created += new FileSystemEventHandler(ScheduledExportProcessing);
                ScheduledExport.Filter = SCHEDULED_EXPORT_FILE;
                ScheduledExport.EnableRaisingEvents = true;
                Log.Debug("BackupSettings Server: AutomatedExport activated for " + ScheduledExport.Path + "\\" + ScheduledExport.Filter);

            }
            else
            {
                Log.Debug("BackupSettings Server Error: Importwatcher/AutomatedExport could not be started because directory " + @"\" + TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR + " does not exist");
            }


            //start pollingthread
            runpolling = true;
            System.Threading.Thread th = new System.Threading.Thread(startpolling);
            th.IsBackground = true;
            th.Start();
            //Log.Debug("Polling thread starting");

            
        }

        public void Stop()
        {
            Log.Debug("BackupSettings Server: Plugin BackupSettings stopped");

            runpolling = false;  //terminate polling loop
            
            //ITvServerEvent events = GlobalServiceProvider.Instance.Get<ITvServerEvent>();
            events.OnTvServerEvent -= new TvServerEventHandler(events_OnTvServerEvent);


            if (ImportWatcher != null)
            {
                ImportWatcher.EnableRaisingEvents = false;
                Log.Debug("BackupSettings Server: ImportWatcher disabled");
                WATCHERACTIVE = false;
            }

            if (AutomatedExport != null)
            {
                AutomatedExport.EnableRaisingEvents = false;
                Log.Debug("BackupSettings Server: AutomatedExport disabled");
            }

            if (ScheduledExport != null)
            {
                ScheduledExport.EnableRaisingEvents = false;
                Log.Debug("BackupSettings Server: ScheduledExport disabled");
            }
            
        }

        
        [CLSCompliant(false)]
        public SetupTv.SectionSettings Setup
        {
            get 
            {
                return new SetupTv.Sections.BackupSettingsSetup(); 
            }
        }

        

        #endregion

        #region methods

        public void startpolling()
        {
            try
            {
                Log.Debug("Polling thread started");
                while (runpolling == true)
                {
                    System.Threading.Thread.Sleep(60000); //sleep 60s  

                    textoutput("BackupSettings Polling: _NextBackup=" + _NextBackup.ToString() + "     recording=" + _recording.ToString() + "  TimeShiftingActive=" + _timeshifting.ToString());
                   
                    //if ((DateTime.Now > _NextBackup) && (Recordings == 0) && (TimeShiftingActive==false))
                    if (DateTime.Now > _NextBackup)
                    {
                        if (((_recording == 0) && (_timeshifting == false) && (_useRecordingFlag)) || (( (DateTime.Now > _RecordingFlagTime) && (_useRecordingFlag)) || (_useRecordingFlag == false)))
                        {
                            SetStandbyAllowed(false);
                            textoutput("**************************************************************");
                            textoutput("Scheduled Export started at " + DateTime.Now.ToString());
                            textoutput("**************************************************************");

                            //delete older backups not within keepnumber
                            string newBackupFolder = DeleteOlderBackups();

                            //do export
                            CreateScheduledExport(newBackupFolder);

                            //calculate next backuptime
                            CalculateNextBackupTime();

                            //write setting _nextBackup and _lastbackup
                            TvBusinessLayer layer = new TvBusinessLayer();
                            Setting setting = null;

                            setting = layer.GetSetting("Backup_SettingsNextBackup", DateTime.Now.ToString("yyyy-MM-dd_HH:mm", System.Globalization.CultureInfo.InvariantCulture));
                            setting.Value = _NextBackup.ToString("yyyy-MM-dd_HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                            setting.Persist();

                            setting = layer.GetSetting("Backup_SettingsLastBackup", DateTime.Now.ToString("yyyy-MM-dd_HH:mm", System.Globalization.CultureInfo.InvariantCulture));
                            setting.Value = DateTime.Now.ToString("yyyy-MM-dd_HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                            setting.Persist();

                            textoutput("Automated Export finished \n");
                            SetStandbyAllowed(true);

                        }
                    }
                }
                Log.Debug("Polling thread stopped");
            }
            catch (Exception exc)
            {
                Log.Debug("Polling thread failed with exception message:");
                Log.Error("Polling thread failed with exception message:");
                Log.Debug(exc.Message);
                Log.Error(exc.Message);

            }


        }

        public string DeleteOlderBackups()
        {
            TvBusinessLayer layer = new TvBusinessLayer();
            Setting setting = null;

            setting = layer.GetSetting("Backup_SettingsSchedulerKeepNumber", "3");
            string keepNumberString = setting.Value;
            int keepNumber = 10000000; //large enough number not to be repeated
            try
            {
                keepNumber = Convert.ToInt32(keepNumberString);
            }
            catch
            {

            }
            Log.Debug("keepNumberString=" + keepNumberString);

            setting = layer.GetSetting("Backup_SettingsAutomatedExportFolderName", @"C:\MediaPortal Backups");
            string foldername = setting.Value;
            Log.Debug("foldername=" + foldername);

            try
            {
                if (!Directory.Exists(foldername))
                {
                    Directory.CreateDirectory(foldername);
                }

                int ctr = 1;
                string backupfolder=foldername+@"\BackupSettingsBackup_" + ctr.ToString("D3");
                Log.Debug("backupfolder=" + backupfolder);
                while (Directory.Exists(backupfolder))
                {
                    ctr++;
                    backupfolder = foldername + @"\BackupSettingsBackup_" + ctr.ToString("D3");
                    Log.Debug("backupfolder=" + backupfolder);
                }
                ctr--;
                Log.Debug("Found " + ctr.ToString() + " Backup Folders");

                if (ctr >= keepNumber)
                {
                    for (int i = 1; i <= (ctr - keepNumber + 1); i++)
                    {
                        removeReadOnlyRecursive(foldername + @"\BackupSettingsBackup_" + i.ToString("D3"));
                        Directory.Delete(foldername + @"\BackupSettingsBackup_" + i.ToString("D3"), true); 
                    }

                    for (int i = 1; i <= (keepNumber - 1); i++)
                    {
                        removeReadOnlyRecursive((foldername + @"\BackupSettingsBackup_" + (ctr - keepNumber + 1 + i).ToString("D3")));
                        Directory.Move(foldername + @"\BackupSettingsBackup_" + (ctr - keepNumber + 1 + i).ToString("D3"), foldername + @"\BackupSettingsBackup_" + (i).ToString("D3"));
                    }
                    foldername = foldername + @"\BackupSettingsBackup_" + (keepNumber).ToString("D3");
                }
                else
                {
                    foldername = foldername + @"\BackupSettingsBackup_" + (ctr + 1).ToString("D3");
                }
                return foldername;
            }
            catch (Exception exc)
            {
                Log.Debug("DeleteOlderBackups failed with exception message:");
                Log.Error("DeleteOlderBackups failed with exception message:");
                Log.Debug(exc.Message);
                Log.Error(exc.Message);

            }
            return foldername+@"\ErrorBackupFolder";
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


        public void CreateScheduledExport(string folderName)
        {
            try
            {
                Log.Debug("CreateScheduledExport folder=" + folderName);
                BackupSettingsExportImport newexportimport = new BackupSettingsExportImport();
                newexportimport.DEBUG = DEBUG;
                newexportimport.newmessage += new textexportmessage(textoutput); //enable logging from newimportexport class
                newexportimport.MyLoadSettings();
                newexportimport.getallversionnumbers("", false); //get only program version numbers after loadsettings
                STATUS = "";
                ProgressBar progressBar = new System.Windows.Forms.ProgressBar();
                newexportimport.ExportImportInit(ref progressBar, true);
                newexportimport.createbackup(folderName); //perform export
                newexportimport.newmessage -= new textexportmessage(textoutput);

                string fName = TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + STATUS_FILE;
                try
                {
                    StreamWriter fs = File.AppendText(fName);
                    fs.Write(STATUS);
                    fs.Close();
                    if (DEBUG == true)
                        Log.Debug("BackupSetting: " + STATUS_FILE + " file written");
                }
                catch (Exception ee)
                {
                    Log.Debug("BackupSettingsServer: Error in writing file  " + fName);
                    Log.Debug("BackupSettingsServer: Exception message is " + ee.Message);
                }
                Log.Debug("CreateScheduledExport completed");
            }
            catch (Exception exc)
            {
                Log.Error("CreateScheduledExport: Exception=" + exc.Message);
            } 
        }
              
        private void CalculateNextBackupTime()
        {
            
            Log.Debug("CalculateNextBackupTime Old date=" + _NextBackup.ToString());
            _NextBackup = DateTime.Now; //last export was now
            Log.Debug("CalculateNextBackupTime NOW=" + _NextBackup.ToString());

            try
            {
                TvBusinessLayer layer = new TvBusinessLayer();
                Setting setting = null;

                setting = layer.GetSetting("Backup_SettingsEnableScheduler", "false");
                bool EnableScheduler = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSchedulerDays", "30");
                string Days = setting.Value;

                setting = layer.GetSetting("Backup_SettingsSchedulerHours", "06");
                string Hours = setting.Value;

                setting = layer.GetSetting("Backup_SettingsSchedulerMinutes", "00");
                string Minutes = setting.Value;

                setting = layer.GetSetting("Backup_SettingsSchedulerWeekDays", "Any");
                string WeekDays = setting.Value;

                if (!EnableScheduler)
                {
                    _NextBackup = DateTime.ParseExact("2200-01-01_00:00", "yyyy-MM-dd_HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                    Log.Debug("CalculateNextBackupTime New date==" + _NextBackup.ToString());
                    return;
                }


                double add_days = 30.0;
                try
                {
                    add_days = Convert.ToDouble(Days);
                }
                catch
                {
                    textoutput("Error: Could not convert days into double value - using default of 30 days");
                }
                _NextBackup = _NextBackup.AddDays(add_days); //use old _nextbackup for new date


                double hours = 6.0;
                try
                {
                    hours = Convert.ToDouble(Hours);
                }
                catch
                {
                    textoutput("Error: Could not convert hours into double value - using default of 06");
                }
                _NextBackup = _NextBackup.AddHours(hours - _NextBackup.Hour);

                double minutes = 0.0;
                try
                {
                    minutes = Convert.ToDouble(Minutes);
                }
                catch
                {
                    textoutput("Error: Could not convert minutess into double value - using default of 00");
                }
                _NextBackup = _NextBackup.AddMinutes(minutes - _NextBackup.Minute);

                if (WeekDays.Contains("Any") == false)
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
                            if (WeekDays.Contains("Sunday"))
                            {
                                found = true;
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
                            if (WeekDays.Contains("Monday"))
                            {
                                found = true;
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
                            if (WeekDays.Contains("Tuesday"))
                            {
                                found = true;
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
                            if (WeekDays.Contains("Wednesday"))
                            {
                                found = true;
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
                            if (WeekDays.Contains("Thursday"))
                            {
                                found = true;
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
                            if (WeekDays.Contains("Friday"))
                            {
                                found = true;
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
                            if (WeekDays.Contains("Saturday"))
                            {
                                found = true;
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
                        Log.Error("No valid weekday found - adding 7 days to next Backup date");
                    }
                }
            }
            catch (Exception exc)
            {
                Log.Error("CalculateNextBackupTime: Exception=" + exc.Message);
            } 

            Log.Debug("CalculateNextBackupTime New date==" + _NextBackup.ToString());
        }

        


        void events_OnTvServerEvent(object sender, EventArgs eventArgs)
        {
            try
            {
                TvServerEventArgs tvEvent = (TvServerEventArgs)eventArgs;
                switch (tvEvent.EventType)
                {
                    ///StartZapChannel is called just before the server is going to change channels

                    ///StartRecording is called just before the server is going to start recording
                    case TvServerEventType.StartRecording:
                        _recording++;
                        _RecordingFlagTime = DateTime.Now.AddHours(4.0);
                        Log.Debug("Recording flag increased -   Recordings=" + _recording.ToString() + "  TimeShiftingActive=" + _timeshifting.ToString());
                        Log.Debug("RecordingFlagTime increased to " + _RecordingFlagTime.ToString() + " - start recording");
                        break;
                        
                    ///RecordingStarted is called when the recording is started
                    case TvServerEventType.RecordingStarted:                        
                        Log.Debug("recording started");
                        break;
                    ///RecordingEnded is called when the recording has been stopped
                    case TvServerEventType.RecordingEnded:
                        if (_recording > 0)
                        _recording--;

                        Log.Debug("Recording flag decreased - Recordings=" + _recording.ToString() + "  TimeShiftingActive=" + _timeshifting.ToString());

                        break;
                    ///Timeshifting started
                    case TvServerEventType.StartTimeShifting:
                        _timeshifting = true;
                        Log.Debug("Timeshifting started -  Recordings=" + _recording.ToString() + "  TimeShiftingActive=" + _timeshifting.ToString());
                        break;
                    ///Timeshifting ended
                    case TvServerEventType.EndTimeShifting:
                        _timeshifting = false;
                        Log.Debug("Timeshifting stopped -  Recordings=" + _recording.ToString() + "  TimeShiftingActive=" + _timeshifting.ToString());
                        break;
                }
            }
            catch (Exception exc)
            {
                Log.Debug("events_OnTvServerEvent failed with exception message:");
                Log.Error("events_OnTvServerEvent failed with exception message:");
                Log.Debug(exc.Message);
                Log.Error(exc.Message);
            }
        }


        private void SetStandbyAllowed(bool allowed)
        {
            try
            {
                //use IEPG handler to prevent shutdown
                if (GlobalServiceProvider.Instance.IsRegistered<IEpgHandler>())
                {

                    GlobalServiceProvider.Instance.Get<IEpgHandler>().SetStandbyAllowed(this, allowed, 1800);//30 minutes timeout           
                    Log.Debug("Telling PowerScheduler standby is: " + allowed.ToString() + ", timeout is 30 minutes");
                }
            }
            catch (Exception exc)
            {
                Log.Debug("SetStandbyAllowed failed with exception message:");
                Log.Error("SetStandbyAllowed failed with exception message:");
                Log.Debug(exc.Message);
                Log.Error(exc.Message);
            }
        }

        public void ScheduledExportProcessing(object sender, FileSystemEventArgs e)
        {
            try
            {
                ScheduledExport.EnableRaisingEvents = false; //set watcher inactive
                System.Threading.Thread.Sleep(2000);
                string datestring = File.ReadAllText(TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + SCHEDULED_EXPORT_FILE);
                try
                {
                    _NextBackup = DateTime.ParseExact(datestring, "yyyy-MM-dd_HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                }
                catch (Exception exc)
                {
                    Log.Error("reading NextBackup from file "+TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + SCHEDULED_EXPORT_FILE+" failed with exception: " + exc.Message);
                }


                File.Delete(TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + SCHEDULED_EXPORT_FILE); //delete file

                /*
                //get next backup time
                TvBusinessLayer layer = new TvBusinessLayer();
                Setting setting = null;
                _NextBackup = DateTime.Now.AddDays(30);//add one month default
                setting = layer.GetSetting("Backup_SettingsNextBackup", DateTime.Now.ToString("yyyy-MM-dd_HH:mm", System.Globalization.CultureInfo.InvariantCulture));
                try
                {
                    _NextBackup = DateTime.ParseExact(setting.Value, "yyyy-MM-dd_HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                }
                catch (Exception exc)
                {
                    Log.Error("NextBackup failed with exception: " + exc.Message);
                }*/
                Log.Debug("ScheduledExportProcessing: _NextBackup=" + _NextBackup.ToString());

                ScheduledExport.EnableRaisingEvents = true; //set watcher inactive
            }
            catch (Exception exc)
            {
                Log.Error("ScheduledExportProcessing: Exception=" + exc.Message);
            }
        }

        public void AutomatedExportProcessing(object sender, FileSystemEventArgs e)
        {            
            try
            {
                textoutput("**************************************************************");
                textoutput("Automated Export started at "+DateTime.Now.ToString());
                textoutput("**************************************************************");
                AutomatedExport.EnableRaisingEvents = false; //set watcher inactive
                System.IO.StreamReader sr = new System.IO.StreamReader(TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + AUTO_EXPORT_FILE);
                string foldername = sr.ReadToEnd();

                sr.Close();
                File.Delete(TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + AUTO_EXPORT_FILE); //delete file

                BackupSettingsExportImport newexportimport = new BackupSettingsExportImport();
                newexportimport.DEBUG = DEBUG;
                newexportimport.newmessage += new textexportmessage(textoutput); //enable logging from newimportexport class
                newexportimport.MyLoadSettings();
                newexportimport.getallversionnumbers("", false); //get only program version numbers after loadsettings
                STATUS = "";

                string[] folderarray = foldername.Split('\n');

                if (foldername.ToLower().StartsWith("autocreatefilename"))  //use autocreate or use string as new foldername
                {
                    if (folderarray.Length > 1)
                    {
                        foldername = newexportimport.CreateAutomatedFolderName(folderarray[1], WhoAmI.Tv_Server); //second line in file gives foldername for autocreate
                    }
                    else
                    {
                        foldername = newexportimport.CreateAutomatedFolderName(newexportimport.pathname, WhoAmI.Tv_Server); //now folder defined for auto create - use last stored folder
                    }
                }
                newexportimport.pathname = foldername;


                ProgressBar progressBar = new System.Windows.Forms.ProgressBar();
                newexportimport.ExportImportInit(ref progressBar, true);

                newexportimport.createbackup(foldername); //perform export

                //append status of postprocessing to status file
                // do not use textoutput anymore
                newexportimport.newmessage -= new textexportmessage(textoutput);
                string fName = TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + STATUS_FILE;
                try
                {
                    StreamWriter fs = File.AppendText(fName);
                    fs.Write(STATUS);
                    fs.Close();
                    if (DEBUG == true)
                        Log.Debug("BackupSetting: " + STATUS_FILE + " file written");
                }
                catch (Exception ee)
                {
                    Log.Debug("BackupSettingsServer: Error in writing file  " + fName);
                    Log.Debug("BackupSettingsServer: Exception message is " + ee.Message);
                }
                textoutput("Automated Export finished \n");
                POSTPROCESSING = false; // reset POSTPROCESSING flag
                AutomatedExport.EnableRaisingEvents = true;  //set watcher active
            }
            catch (Exception exc)
            {
                Log.Error("ScheduledExportProcessing: Exception=" + exc.Message);
            }           
        }


        public void importpostprocessing(object sender, FileSystemEventArgs e)
        {
            ImportWatcher.EnableRaisingEvents = false;
            STATUS="";
            
            textoutput("TV server import postprocessing started");

            if (POSTPROCESSING == true)
            {
                textoutput("<RED>Error: Import postprocessing is busy from another process - aborting postprocessing");
                return;
            }

            POSTPROCESSING = true;  //set busy postprocessing status

            //wait for SetupTv.exe to close
            Process[] processes = Process.GetProcessesByName("SetupTv");
            foreach (Process process in processes)
            {

                Log.Debug("BackupSettingsServer: Process SetupTv identified");

                process.WaitForExit(1000 * 60 * 1); // wait 1 minutes maximum
                if (process.HasExited == true)
                {
                    System.Threading.Thread.Sleep(2000);
                    PostProcessingFile();
                }
                else
                {
                    textoutput("<RED>Tv Service timeout error: SetupTv did not close within time limit - aborting postprocessing");
                }
            }
            

            exitpostprocessing();

            POSTPROCESSING = false;  //set busy postprocessing status

            ImportWatcher.EnableRaisingEvents = true;

        }

        public void PostProcessingFile()
        {


            TvBusinessLayer layer = new TvBusinessLayer();
            Setting setting;

            try
            {
                POSTPROCESSING = true;  //set busy postprocessing status

                
                setting = layer.GetSetting("Backup_SettingsSetup_debug", "false");
                DEBUG = Convert.ToBoolean(setting.Value);

                setting = layer.GetSetting("Backup_SettingsSetup_TV_USER_FOLDER", ".");
                TV_USER_FOLDER = setting.Value;

                //if ((TV_PROGRAM_FOLDER=="NOT_FOUND")||(TV_PROGRAM_FOLDER=="NOT_DEFINED"))
                 //   TV_PROGRAM_FOLDER = ".";               
            }
            catch (Exception ee)
            {
                textoutput("<RED>BackupSettingsServer: Error when waiting for SetupTv exit - aborting postprocessing");
                if (DEBUG==true)
                    textoutput("<RED>BackupSettingsServer: Exception message is " + ee.Message);
            }

            //read postprocessing file
            string fileName = TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + POSTPROCESSING_FILE;
            string fileText = "";

            try
            {
                if (System.IO.File.Exists(fileName) == true)
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(fileName);
                    fileText = sr.ReadToEnd();
                    sr.Close();
                    string[] lines = fileText.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    //process data
                    
                    foreach (String line in lines)
                    {
                        
                        //postprocessing goes here
                        
                        if (line.StartsWith("SETTING") == true)
                        {
                            //"SETTING\t" + attribute + "\t" + setting + "\n";
                            string[] tokens = line.Split("\t".ToCharArray());
                            if (tokens.Length != 3)
                            {
                                textoutput("<RED>BackupSetting Postprocessing Error: SETTING line " + line + " has " + tokens.Length.ToString() + " tokens - skipping entry");
                            }
                            else
                            {
                                if (tokens[2] != "BACKUPSETTINGS_NOTFOUND")
                                {
                                    setting = layer.GetSetting(tokens[1], "");
                                    setting.Value = tokens[2];
                                    setting.Value = setting.Value.Replace("__RETURN__", "\n");
                                    setting.Persist();
                                    if (DEBUG)
                                    {
                                        textoutput("Processed SETTING with attribute=" + tokens[1] + ", value=" + tokens[2]);
                                    }
                                }
                            }
                            
                        }
                        else if (line.StartsWith("CARDGROUPMAP") == true)
                        {
                            //"CARDGROUPMAP\t" + newIdCard + "\t" + newIdCardGroup + "\t" + HybridGroupName + "\n";
                            
                            string[] tokens = line.Split("\t".ToCharArray());
                            if (tokens.Length != 4)
                            {
                                textoutput("<RED>BackupSetting Postprocessing Error: CARDGROUPMAP line " + line + " has " + tokens.Length.ToString() + " tokens - skipping entry");
                            }
                            else
                            {
                                try
                                {
                                    //get cardgroup id from name
                                    int cardid = Convert.ToInt32(tokens[1]);
                                    int cardgroupid = Convert.ToInt32(tokens[2]);

#if(MP100)
                                    IList hybridcardgroups = CardGroup.ListAll();
#elif(MP101) 
                                    IList<CardGroup> hybridcardgroups = CardGroup.ListAll();
#else //MP11BETA or SVN 
                                    IList<CardGroup> hybridcardgroups = CardGroup.ListAll();
#endif

                                    foreach (CardGroup hybridcardgroup in hybridcardgroups)
                                    {
                                        if (hybridcardgroup.Name == tokens[3])
                                        {
                                            cardgroupid=hybridcardgroup.IdCardGroup;
                                            if (DEBUG == true)
                                                textoutput("Identified card group " + hybridcardgroup.Name + " with id " + cardgroupid);

                                            break;
                                        }

                                    }


                                    CardGroupMap newcardgroupmap = new CardGroupMap(cardid, cardgroupid);
                                    newcardgroupmap.Persist();
                                    if (DEBUG)
                                    {
                                        textoutput("Processed CARDGROUPMAP with card id=" + cardid.ToString() + ", cardgroup id=" + cardgroupid.ToString() + " HybridGroupName=" + tokens[3].ToString());
                                    }
                                }
                                catch
                                {
                                    textoutput("<RED>Error: Could not create new cardgroup map for card id " + tokens[1].ToString() + " and cardgroup id " + tokens[2].ToString() + " - skipping entry");
                                }
                                
                            }
                        }
                        else if (line.StartsWith("TVMOVIE") == true)
                        {
                            //"TVMOVIE\t" + idChannel + "\t" + stationName + "\t" + timeSharingStart + "\t" + timeSharingEnd + "\n";
                            
                            string[] tokens = line.Split("\t".ToCharArray());
                            if (tokens.Length != 5)
                            {
                                textoutput("<RED>BackupSetting Postprocessing Error: TVMOVIE line " + line + " has " + tokens.Length.ToString() + " tokens - skipping entry");
                            }
                            else
                            {
                                try
                                {
                                    TvMovieMapping tvmapping = new TvMovieMapping(Convert.ToInt32(tokens[1]), tokens[2],tokens[3],tokens[4]);
                                    tvmapping.Persist();
                                }
                                catch
                                {
                                    textoutput("<RED>Error: Could not create new Tv Movie map for channel id=" + tokens[1] + ", station name=" + tokens[2] + " time sharing start=" + tokens[3] + " time sharing end=" + tokens[4] + " - skipping entry");
                                }
                                if (DEBUG)
                                {
                                    textoutput("Processed TVMOVIE with channel id=" + tokens[1] + ", station name=" + tokens[2] + " time sharing start=" + tokens[3] + " time sharing end=" + tokens[4]);
                                }
                            }
                            
                            
                        }
                        
                    }

                    CalculateNextBackupTime(); //calculate next automated export time after import

                }
            }
            catch (Exception ee)
            {
                textoutput("<RED>Server error in processing the file " + fileName);
                if (DEBUG == true)
                {
                    textoutput("<RED>File text was:");
                    textoutput(fileText);
                    textoutput("<RED>Exception message is " + ee.Message);
                }
            }

            textoutput("TV server import postprocessing completed\n\n");
        }


        public void exitpostprocessing()
        {
            //append status of postprocessing to status file
            // do not use textoutput anymore
            string fName = TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + STATUS_FILE;
            try
            {
                StreamWriter fs = File.AppendText(fName);
                fs.Write(STATUS);
                fs.Close();
                if (DEBUG == true)
                    Log.Debug("BackupSetting: " + TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + STATUS_FILE + " file written");
            }
            catch (Exception ee)
            {
                Log.Debug("BackupSettingsServer: Error in writing file  " + fName);
                Log.Debug("BackupSettingsServer: Exception message is " + ee.Message);

            }

            //trigger BackupSettingImport to close application and start TVsetup.exe by deleting the postprocessing file

            try
            {
                File.Copy(TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + POSTPROCESSING_FILE, TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + POSTPROCESSING_FILE + ".bak.txt", true);
                File.Delete(TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + POSTPROCESSING_FILE);
            }
            catch (Exception ee)
            {
                Log.Debug("BackupSettingsServer: Error in copying/deleting file  " + TV_USER_FOLDER + @"\" + BACKUPSETTING_TV_DIR + @"\" + POSTPROCESSING_FILE);
                Log.Debug("BackupSettingsServer: Exception message is " + ee.Message);

            }

            Log.Debug("BackupSettingsServer: Server completed postprocessing");
        }


        public void textoutput(string text)
        {
            if (DEBUG)
                Log.Debug("BackupSettingsServer: " + text);

            STATUS += text + "\n";
        }

        #endregion
    }
}
