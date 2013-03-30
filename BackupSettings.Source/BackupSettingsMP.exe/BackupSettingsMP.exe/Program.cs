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



/* Version History BackupSettingsMP.exe
 * 1.0.4.2
 * only single program instance can be started
 * code cleanup in restore routine and improvements
 * 0.0.1.9 
 * - version number updated
 * - installpath class added
 * 
 * 0.0.0.3
 * - updated autodetect function for directories and do no more require helpreferences.xml for MP_USER_FOLDER
 * 0.0.0.2  initial release for BackupSettingsMP,dll V0.0.0.2
 * - same functionality as the TV SErver plugin for export/import of MediaPortal data only
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;

namespace BackupSettingsMP.exe
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //MessageBox.Show("args.Length=" + args.Length.ToString());
            

            
            //admin rights
            if (args.Length == 0)
            {
                Process proc = new Process();
                ProcessStartInfo startinfo = new ProcessStartInfo();
                startinfo.FileName = "BackupSettingsMP.exe";
                startinfo.WorkingDirectory = System.Environment.CurrentDirectory;
                startinfo.Arguments = "MP2_Server";
                startinfo.Verb = "runas";
                proc.StartInfo = startinfo;
                proc.Start();
                return;
            }

            
            //only single instance
            Process[] processes = Process.GetProcessesByName("BackupSettingsMP");
            //MessageBox.Show("processes.Length=" + processes.Length.ToString());
            if (processes.Length != 1)
                return;//exit if a process exists already
            

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //MessageBox.Show("args.Length=" + args.Length.ToString(), "Debug");
            if (args.Length >= 1)
                Application.Run(new Form1(args[0]));
            else
                Application.Run(new Form1("MP2_Server"));
        }
    }
}
