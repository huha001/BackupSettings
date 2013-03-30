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



/* Version History RestartSetupTV.exe
 * 1.0.0.3  add dummy for argument TEST
 * 0.0.1.9  version number updated
 * 0.0.0.2  added support for threadkill
 * 0.0.0.1  initial release
*/

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

using RestartSetupTV.ThreadKillMessage;
using RestartSetupTV.ImportPostprocessing;

namespace RestartSetupTV
{
    class RestartSetupTv
    {
        static System.Timers.Timer error_timer;
        public static string TV_PROGRAM_FOLDER = @"..\";
        public static string TV_USER_FOLDER = @"..\";
        public static string SETUP_TV_EXE = "SetupTv.exe";
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //start timeout
            error_timer = new System.Timers.Timer(1000 * 60); //close after 1 minutes
            error_timer.Enabled = true;
            error_timer.Elapsed += new System.Timers.ElapsedEventHandler(errorapplication);

           
            if (args.Length < 1)
            {
                MessageBox.Show("Please restart TV Server Configuration manually","Process Call Error");
            }
            else if (args.Length == 1)
            {
                if (args[0] == "TEST")
                {// do nothing TEST run is done by installer to enforce internet security confirmation during install process
                    return;
                }
                else
                {
                    MessageBox.Show("Please restart TV Server Configuration manually", "Process Call Error");
                    return;
                }
            }
            else if (args.Length ==2)
            {
                TV_PROGRAM_FOLDER = args[0];
                if (args[1] == "THREADKILL")
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Threadkillmessage());
                    return;
                }
                else
                {
                    MessageBox.Show("Please restart TV Server Configuration manually", "Message Error:" + args[1]);
                    return;
                }
            }
            else if (args.Length ==3)
            {
                TV_PROGRAM_FOLDER = args[0];
                TV_USER_FOLDER = args[1];
                if (args[2] == "POSTPROCESSING")
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Importpostprocessing());
                    return;
                }
                else
                {
                    MessageBox.Show("Please restart TV Server Configuration manually", "Message Error:" + args[2]);
                    return;
                }
            }
        

            
        }


        static public void errorapplication(object sender, System.Timers.ElapsedEventArgs e)
        {
            //Log.Debug("BackupSettings Server: Application exit");
            MessageBox.Show("A timeout error occured\n Reboot your computer and repeat the import", "Restart TV Server Configuration Error");
            
            Process[] processes = Process.GetProcessesByName("RestartSetupTv");
            foreach (Process process in processes)
            {
                process.Kill();
            }
        }
    }
}
