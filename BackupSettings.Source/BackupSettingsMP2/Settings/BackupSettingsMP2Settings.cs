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

using System;
using System.Collections.Generic;
using MediaPortal.Common.Settings;

namespace MediaPortal.Plugins.BackupSettingsMP2.Settings
{

    //Settings will be saved in C:\ProgramData\Team MediaPortal\MP2-Client\Config\<USER>\Plugins.MyTestPlugin.Settings.MyTestPluginSettings.xml
    public class BackupSettingsMP2Settings
  {
      
    /// <summary>
    /// test setting bool 
    /// </summary>
    [Setting(SettingScope.User, true)]
      public bool MySetting_Bool { get; set; } 

  }
}