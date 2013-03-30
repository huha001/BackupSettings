#region Copyright (C) 2007-2011 Team MediaPortal

/*
    Copyright (C) 2007-2011 Team MediaPortal
    http://www.team-mediaportal.com

    This file is part of MediaPortal 2

    MediaPortal 2 is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    MediaPortal 2 is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with MediaPortal 2. If not, see <http://www.gnu.org/licenses/>.
*/

#endregion

using MediaPortal.Common;
using MediaPortal.Common.Logging;
using MediaPortal.Common.PathManager;
using MediaPortal.Common.Configuration.ConfigurationClasses;
using MediaPortal.Plugins.BackupSettingsMP2.Settings;

using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace MediaPortal.Plugins.BackupSettingsMP2.Settings.Configuration
{
    public class StartClass : YesNo
    {
        public override void Load()
        {
            _yes = true;
        }
        public override void Save()
        {
            base.Save();
            BackupSettingsMP2Settings settings = SettingsManager.Load<BackupSettingsMP2Settings>();
            settings.MySetting_Bool = _yes;

            ServiceRegistration.Get<ILogger>().Debug("BackupSettingsMP2.Settings.Configuration: Start class executed: current directory="+System.Environment.CurrentDirectory.ToString());
            
            if (Yes)
            {
                //restart SetupTv.exe
                Process app = new Process();
                ProcessStartInfo appstartinfo = new ProcessStartInfo();
                appstartinfo.Verb = "runas";
                appstartinfo.FileName = System.Environment.CurrentDirectory.ToString()+@"\Plugins\BackupSettings\BackupSettingsMP.exe";
                appstartinfo.Arguments = "MP2_Client";
                appstartinfo.WorkingDirectory = @"Plugins\BackupSettings";
                app.StartInfo = appstartinfo;
                try
                {
                    app.Start();
                    ServiceRegistration.Get<ILogger>().Debug("BackupSettingsMP2.Settings.Configuration: Closing Client");
                    Application.Exit();
                }
                catch (Exception ee)
                {
                    ServiceRegistration.Get<ILogger>().Error("Error in starting " + appstartinfo.FileName + "\nException message is \n" + ee.Message, "Restart TV Server Configuration Error");
                }
            }
            
        }

        

        
    }

   
 
}