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
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
using System.Diagnostics;
//using System.Drawing;
//using System.Linq;
//using System.Text;
using System.Windows.Forms;
//using System.Runtime.InteropServices;
using System.IO;
//using System.Security;


namespace RestartSetupTV.ImportPostprocessing
{
    public partial class Importpostprocessing : Form
    {

        System.Timers.Timer m_timer;
                
        
        string BACKUPSETTING_TV_DIR = "BackupSettings";
        string POSTPROCESSING_FILE = "ImportPostprocessing.txt";
        

        public Importpostprocessing()
        {
            InitializeComponent();

            string TV_PROGRAM_FOLDER = RestartSetupTv.TV_PROGRAM_FOLDER;
            string TV_USER_FOLDER = RestartSetupTv.TV_USER_FOLDER;
            string SETUP_TV_EXE = RestartSetupTv.SETUP_TV_EXE;

            // wait until postprocessing is completed by tvserver and the file StartImportPostprocessing.txt has been deleted by the backupsetting tvserver routine
            while (File.Exists(TV_USER_FOLDER + @"\"+BACKUPSETTING_TV_DIR+@"\"+POSTPROCESSING_FILE)==true)
            {
                System.Threading.Thread.Sleep(1000);
                //Log.Debug("BackupSettings Import Controller: Waiting for tvserver postprocessing");
            }


            //restart SetupTv.exe
            Process app = new Process();
            ProcessStartInfo appstartinfo = new ProcessStartInfo();
            appstartinfo.FileName = TV_PROGRAM_FOLDER + @"\"+SETUP_TV_EXE;
            appstartinfo.WorkingDirectory = TV_PROGRAM_FOLDER;
            app.StartInfo = appstartinfo;
            try
            {
                app.Start();
            }
            catch (Exception ee)
            {
                MessageBox.Show("Error in starting " + appstartinfo.FileName + "\nException message is \n" + ee.Message,"Restart TV Server Configuration Error");
            }
            
            m_timer = new System.Timers.Timer(4000 ); //close after 2s
            m_timer.Enabled = true;
            m_timer.Elapsed += new System.Timers.ElapsedEventHandler(closeapplication);
            
            
        }
        public void closeapplication(object sender, System.Timers.ElapsedEventArgs e)
        {
            Application.Exit();
        }
        
    }
}
