#region Copyright (C) 2006-2008 Team MediaPortal

/* 
 *	Copyright (C) 2006-2008 Team MediaPortal
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



/* Version History BackupSettingsMP.dll
 
 *  0.0.1.9 version number updated to project number
 *  0.0.0.2
 *  - plugin calls BackupSettingsMP.exe and closes Configuration. This avoids data conflicts with open files during import
 *  0.0.0.1 (release)
 * - initial release
 * 
 * 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using MediaPortal.GUI.Library;
using MediaPortal.BackupSettingMP;
using MediaPortal.Plugins;

namespace MediaPortal.BackupSettingMP
{
    public class BackupSettingMP : GUIWindow, ISetupForm, IShowPlugin
    {

        public BackupSettingMP()
        {
        }
        ~BackupSettingMP()
        {
        }

        #region ISetupForm
        public bool HasSetup()
        {
          return true;
        }
        public string PluginName()
        {
          return "BackupSettingMP";
        }
        public string Description() // Return the description which should b shown in the plugin menu
        {
          return "Backup and Restore MediaPortal User Settings";
        }
        public string Author() // Return the author which should b shown in the plugin menu
        {
          return "huha";
        }
        public void ShowPlugin() // show the setup dialog
        {
            
            
            
          
            //open .exe file to avoid data conflicts during import
            Process nproc = new Process();
            ProcessStartInfo procstartinfo = new ProcessStartInfo();
            if (File.Exists(@"plugins\process\BackupSettingsMP.exe") == true)
            {
                procstartinfo.FileName = @"plugins\process\BackupSettingsMP.exe";
                procstartinfo.Arguments = "MP1_Client";
            }
            else
            {
                // get actual plugin directory
                InstallPaths instpaths = new InstallPaths();  //define new instance for folder detection
                //autodetect paths
                instpaths.GetInstallPaths();
                string DIR_Plugins = instpaths.DIR_Plugins;
                procstartinfo.FileName = DIR_Plugins + @"\process\BackupSettingsMP.exe";
                procstartinfo.Arguments = "MP1_Client";
            }
            
            nproc.StartInfo = procstartinfo;
            try
            {
                nproc.Start();
            }
            catch
            {
                MessageBox.Show("Could not open " +  procstartinfo.FileName, "Error");
            }

        }
        public bool CanEnable() // Indicates whether plugin can be enabled/disabled
        {
          return true;
        }
        
        public int GetWindowId() // get ID of plugin window
        {
          return 0;
        }
        public bool DefaultEnabled() // Indicates if plugin is enabled by default;
        {
          return true;
        }
        
        
        public bool GetHome(out string strButtonText, out string strButtonImage, out string
          strButtonImageFocus, out string strPictureImage)
        {
          strButtonText = null;
          strButtonImage = null;
          strButtonImageFocus = null;
          strPictureImage = null;
          return false;
        }
         

        // With GetID it will be an window-plugin / otherwise a process-plugin
        // Enter the id number here again
        public override int GetID
        {           
            set
            {
            }
        }

        public bool ShowDefaultHome()
        {
            return false;
        }
         

    #endregion
    }
}
