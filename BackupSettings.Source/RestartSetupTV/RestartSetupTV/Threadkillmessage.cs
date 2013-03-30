using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Security;

namespace RestartSetupTV.ThreadKillMessage
{
    public partial class Threadkillmessage : Form
    {
        public Threadkillmessage()
        {
            InitializeComponent();

            string TV_PROGRAM_FOLDER = RestartSetupTv.TV_PROGRAM_FOLDER;
            string SETUP_TV_EXE = RestartSetupTv.SETUP_TV_EXE;
            System.Timers.Timer m_timer;

            //restart SetupTv.exe
            Process app = new Process();
            ProcessStartInfo appstartinfo = new ProcessStartInfo();
            appstartinfo.FileName = TV_PROGRAM_FOLDER + @"\" + SETUP_TV_EXE;
            appstartinfo.WorkingDirectory = TV_PROGRAM_FOLDER;
            app.StartInfo = appstartinfo;
            try
            {
                app.Start();
            }
            catch (Exception ee)
            {
                MessageBox.Show("Error in starting " + appstartinfo.FileName + "\nException message is \n" + ee.Message, "Restart TV Server Configuration Error");
            }

            m_timer = new System.Timers.Timer(4000); //close after 2s
            m_timer.Enabled = true;
            m_timer.Elapsed += new System.Timers.ElapsedEventHandler(closeapplication);
        }

        public void closeapplication(object sender, System.Timers.ElapsedEventArgs e)
        {
            Application.Exit();
        }

        
    }
}
