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


using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;


namespace BackupSettingsMP.exe
{
    partial class Form1
    {
        private CheckBox checkBoxMPAllFolders;
        private CheckBox checkBoxMPDeleteCache;
        private Button importbutton;
        private Button buttonMpNone;
        private CheckBox checkBoxMPMusicPlayer;
        private CheckBox checkBoxMPxmltv;
        private GroupBox groupBox5;
        private CheckBox checkBoxMPThumbs;
        private CheckBox checkBoxMPInputDevice;
        private CheckBox checkBoxMPDatabase;
        private CheckBox checkBoxMPSkins;
        private CheckBox checkBoxMPPlugins;
        private CheckBox checkBoxMPProgramXml;
        private CheckBox checkBoxMPUserXML;
        private TabPage tabPage3;
        private Button buttonMpAll;
        private Label label3;
        private GroupBox groupBox6;
        private Button buttonDefault;
        private CheckBox CheckBoxDebugBackupSettings;
        private TabPage tabPage1;
        private Button clearbutton;
        private TabControl tabControl1;
        private Label label1;
        private Button selectfilenamebutton;
        private TextBox filenametextBox;
        private GroupBox groupBox1;
        private Button exportbutton;
        

        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.checkBoxMPAllFolders = new System.Windows.Forms.CheckBox();
            this.checkBoxMPDeleteCache = new System.Windows.Forms.CheckBox();
            this.importbutton = new System.Windows.Forms.Button();
            this.buttonMpNone = new System.Windows.Forms.Button();
            this.checkBoxMPMusicPlayer = new System.Windows.Forms.CheckBox();
            this.checkBoxMPxmltv = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.checkBoxMPThumbs = new System.Windows.Forms.CheckBox();
            this.checkBoxMPInputDevice = new System.Windows.Forms.CheckBox();
            this.checkBoxMPDatabase = new System.Windows.Forms.CheckBox();
            this.checkBoxMPSkins = new System.Windows.Forms.CheckBox();
            this.checkBoxMPPlugins = new System.Windows.Forms.CheckBox();
            this.checkBoxMPProgramXml = new System.Windows.Forms.CheckBox();
            this.checkBoxMPUserXML = new System.Windows.Forms.CheckBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.buttonMpAll = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.checkBoxMPAllProgram = new System.Windows.Forms.CheckBox();
            this.buttonDefault = new System.Windows.Forms.Button();
            this.CheckBoxDebugBackupSettings = new System.Windows.Forms.CheckBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.checkBoxUseAutoDate = new System.Windows.Forms.CheckBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.checkBoxMP2S = new System.Windows.Forms.CheckBox();
            this.checkBoxMP2C = new System.Windows.Forms.CheckBox();
            this.checkBoxMP = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.radioButtonExpert = new System.Windows.Forms.RadioButton();
            this.radioButtonEasy = new System.Windows.Forms.RadioButton();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonhelp = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.clearbutton = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.buttonMP2None = new System.Windows.Forms.Button();
            this.buttonMP2All = new System.Windows.Forms.Button();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.checkBoxMP2Defaults = new System.Windows.Forms.CheckBox();
            this.checkBoxMP2AllClientProgramFolders = new System.Windows.Forms.CheckBox();
            this.checkBoxMP2AllClientFiles = new System.Windows.Forms.CheckBox();
            this.checkBoxMP2AllClientFolders = new System.Windows.Forms.CheckBox();
            this.checkBoxMP2Config = new System.Windows.Forms.CheckBox();
            this.checkBoxMP2Plugins = new System.Windows.Forms.CheckBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.checkBoxSV2Defaults = new System.Windows.Forms.CheckBox();
            this.checkBoxSV2AllServerProgramFolders = new System.Windows.Forms.CheckBox();
            this.checkBoxSV2AllServerFiles = new System.Windows.Forms.CheckBox();
            this.checkBoxSV2Config = new System.Windows.Forms.CheckBox();
            this.checkBoxSV2AllServerFolders = new System.Windows.Forms.CheckBox();
            this.checkBoxSV2Database = new System.Windows.Forms.CheckBox();
            this.checkBoxSV2Plugins = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonActiveNone = new System.Windows.Forms.Button();
            this.buttonSelectFolder = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.buttonDetect = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.label17 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonMP1P = new System.Windows.Forms.Button();
            this.buttonMP1U = new System.Windows.Forms.Button();
            this.buttonTV1P = new System.Windows.Forms.Button();
            this.buttonTV1U = new System.Windows.Forms.Button();
            this.textBoxTV1U = new System.Windows.Forms.TextBox();
            this.textBoxTV1P = new System.Windows.Forms.TextBox();
            this.textBoxMP1U = new System.Windows.Forms.TextBox();
            this.textBoxMP1P = new System.Windows.Forms.TextBox();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.label12 = new System.Windows.Forms.Label();
            this.buttonSV2U = new System.Windows.Forms.Button();
            this.textBoxSV2U = new System.Windows.Forms.TextBox();
            this.buttonMP2P = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.buttonMP2U = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.buttonSV2P = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.textBoxSV2P = new System.Windows.Forms.TextBox();
            this.textBoxMP2U = new System.Windows.Forms.TextBox();
            this.textBoxMP2P = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.selectfilenamebutton = new System.Windows.Forms.Button();
            this.filenametextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonCreateAutoFolder = new System.Windows.Forms.Button();
            this.exportbutton = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.groupBox5.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabPage5.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage8.SuspendLayout();
            this.tabPage9.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBoxMPAllFolders
            // 
            this.checkBoxMPAllFolders.AutoSize = true;
            this.checkBoxMPAllFolders.Location = new System.Drawing.Point(8, 46);
            this.checkBoxMPAllFolders.Name = "checkBoxMPAllFolders";
            this.checkBoxMPAllFolders.Size = new System.Drawing.Size(158, 17);
            this.checkBoxMPAllFolders.TabIndex = 2;
            this.checkBoxMPAllFolders.Text = "All MediaPortal User Folders";
            this.checkBoxMPAllFolders.UseVisualStyleBackColor = true;
            // 
            // checkBoxMPDeleteCache
            // 
            this.checkBoxMPDeleteCache.AutoSize = true;
            this.checkBoxMPDeleteCache.Location = new System.Drawing.Point(8, 20);
            this.checkBoxMPDeleteCache.Name = "checkBoxMPDeleteCache";
            this.checkBoxMPDeleteCache.Size = new System.Drawing.Size(91, 17);
            this.checkBoxMPDeleteCache.TabIndex = 1;
            this.checkBoxMPDeleteCache.Text = "Delete Cache";
            this.checkBoxMPDeleteCache.UseVisualStyleBackColor = true;
            // 
            // importbutton
            // 
            this.importbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.importbutton.Location = new System.Drawing.Point(389, 42);
            this.importbutton.Name = "importbutton";
            this.importbutton.Size = new System.Drawing.Size(55, 26);
            this.importbutton.TabIndex = 0;
            this.importbutton.Text = "Import";
            this.importbutton.UseVisualStyleBackColor = true;
            this.importbutton.Click += new System.EventHandler(this.importbutton_Click);
            // 
            // buttonMpNone
            // 
            this.buttonMpNone.Location = new System.Drawing.Point(373, 229);
            this.buttonMpNone.Name = "buttonMpNone";
            this.buttonMpNone.Size = new System.Drawing.Size(57, 23);
            this.buttonMpNone.TabIndex = 10;
            this.buttonMpNone.Text = "None";
            this.buttonMpNone.UseVisualStyleBackColor = true;
            this.buttonMpNone.Click += new System.EventHandler(this.buttonMpNone_Click);
            // 
            // checkBoxMPMusicPlayer
            // 
            this.checkBoxMPMusicPlayer.AutoSize = true;
            this.checkBoxMPMusicPlayer.Location = new System.Drawing.Point(28, 207);
            this.checkBoxMPMusicPlayer.Name = "checkBoxMPMusicPlayer";
            this.checkBoxMPMusicPlayer.Size = new System.Drawing.Size(83, 17);
            this.checkBoxMPMusicPlayer.TabIndex = 9;
            this.checkBoxMPMusicPlayer.Text = "MusicPlayer";
            this.checkBoxMPMusicPlayer.UseVisualStyleBackColor = true;
            // 
            // checkBoxMPxmltv
            // 
            this.checkBoxMPxmltv.AutoSize = true;
            this.checkBoxMPxmltv.Location = new System.Drawing.Point(28, 184);
            this.checkBoxMPxmltv.Name = "checkBoxMPxmltv";
            this.checkBoxMPxmltv.Size = new System.Drawing.Size(84, 17);
            this.checkBoxMPxmltv.TabIndex = 8;
            this.checkBoxMPxmltv.Text = "Xmltv Folder";
            this.checkBoxMPxmltv.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.checkBoxMPMusicPlayer);
            this.groupBox5.Controls.Add(this.checkBoxMPxmltv);
            this.groupBox5.Controls.Add(this.checkBoxMPThumbs);
            this.groupBox5.Controls.Add(this.checkBoxMPInputDevice);
            this.groupBox5.Controls.Add(this.checkBoxMPDatabase);
            this.groupBox5.Controls.Add(this.checkBoxMPSkins);
            this.groupBox5.Controls.Add(this.checkBoxMPPlugins);
            this.groupBox5.Controls.Add(this.checkBoxMPProgramXml);
            this.groupBox5.Controls.Add(this.checkBoxMPUserXML);
            this.groupBox5.Location = new System.Drawing.Point(23, 22);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(195, 230);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "File Settimgs for Export and Import";
            // 
            // checkBoxMPThumbs
            // 
            this.checkBoxMPThumbs.AutoSize = true;
            this.checkBoxMPThumbs.Location = new System.Drawing.Point(28, 161);
            this.checkBoxMPThumbs.Name = "checkBoxMPThumbs";
            this.checkBoxMPThumbs.Size = new System.Drawing.Size(64, 17);
            this.checkBoxMPThumbs.TabIndex = 7;
            this.checkBoxMPThumbs.Text = "Thumbs";
            this.checkBoxMPThumbs.UseVisualStyleBackColor = true;
            // 
            // checkBoxMPInputDevice
            // 
            this.checkBoxMPInputDevice.AutoSize = true;
            this.checkBoxMPInputDevice.Location = new System.Drawing.Point(28, 138);
            this.checkBoxMPInputDevice.Name = "checkBoxMPInputDevice";
            this.checkBoxMPInputDevice.Size = new System.Drawing.Size(136, 17);
            this.checkBoxMPInputDevice.TabIndex = 6;
            this.checkBoxMPInputDevice.Text = "Input Device Mappings";
            this.checkBoxMPInputDevice.UseVisualStyleBackColor = true;
            // 
            // checkBoxMPDatabase
            // 
            this.checkBoxMPDatabase.AutoSize = true;
            this.checkBoxMPDatabase.Location = new System.Drawing.Point(28, 115);
            this.checkBoxMPDatabase.Name = "checkBoxMPDatabase";
            this.checkBoxMPDatabase.Size = new System.Drawing.Size(72, 17);
            this.checkBoxMPDatabase.TabIndex = 5;
            this.checkBoxMPDatabase.Text = "Database";
            this.checkBoxMPDatabase.UseVisualStyleBackColor = true;
            // 
            // checkBoxMPSkins
            // 
            this.checkBoxMPSkins.AutoSize = true;
            this.checkBoxMPSkins.Location = new System.Drawing.Point(28, 69);
            this.checkBoxMPSkins.Name = "checkBoxMPSkins";
            this.checkBoxMPSkins.Size = new System.Drawing.Size(129, 17);
            this.checkBoxMPSkins.TabIndex = 4;
            this.checkBoxMPSkins.Text = "Skins and Languages\r\n";
            this.checkBoxMPSkins.UseVisualStyleBackColor = true;
            // 
            // checkBoxMPPlugins
            // 
            this.checkBoxMPPlugins.AutoSize = true;
            this.checkBoxMPPlugins.Location = new System.Drawing.Point(28, 46);
            this.checkBoxMPPlugins.Name = "checkBoxMPPlugins";
            this.checkBoxMPPlugins.Size = new System.Drawing.Size(60, 17);
            this.checkBoxMPPlugins.TabIndex = 3;
            this.checkBoxMPPlugins.Text = "Plugins";
            this.checkBoxMPPlugins.UseVisualStyleBackColor = true;
            // 
            // checkBoxMPProgramXml
            // 
            this.checkBoxMPProgramXml.AutoSize = true;
            this.checkBoxMPProgramXml.Location = new System.Drawing.Point(28, 20);
            this.checkBoxMPProgramXml.Name = "checkBoxMPProgramXml";
            this.checkBoxMPProgramXml.Size = new System.Drawing.Size(110, 17);
            this.checkBoxMPProgramXml.TabIndex = 2;
            this.checkBoxMPProgramXml.Text = "Program .xml Files";
            this.checkBoxMPProgramXml.UseVisualStyleBackColor = true;
            // 
            // checkBoxMPUserXML
            // 
            this.checkBoxMPUserXML.AutoSize = true;
            this.checkBoxMPUserXML.Location = new System.Drawing.Point(28, 92);
            this.checkBoxMPUserXML.Name = "checkBoxMPUserXML";
            this.checkBoxMPUserXML.Size = new System.Drawing.Size(93, 17);
            this.checkBoxMPUserXML.TabIndex = 0;
            this.checkBoxMPUserXML.Text = "User .xml Files";
            this.checkBoxMPUserXML.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.buttonMpNone);
            this.tabPage3.Controls.Add(this.buttonMpAll);
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Controls.Add(this.groupBox6);
            this.tabPage3.Controls.Add(this.groupBox5);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(453, 322);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Media Portal1";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // buttonMpAll
            // 
            this.buttonMpAll.Location = new System.Drawing.Point(240, 229);
            this.buttonMpAll.Name = "buttonMpAll";
            this.buttonMpAll.Size = new System.Drawing.Size(57, 23);
            this.buttonMpAll.TabIndex = 9;
            this.buttonMpAll.Text = "All";
            this.buttonMpAll.UseVisualStyleBackColor = true;
            this.buttonMpAll.Click += new System.EventHandler(this.buttonMpAll_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 266);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(326, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Do not change default settings unless you are a Media Portal expert";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.checkBoxMPAllProgram);
            this.groupBox6.Controls.Add(this.checkBoxMPAllFolders);
            this.groupBox6.Controls.Add(this.checkBoxMPDeleteCache);
            this.groupBox6.Location = new System.Drawing.Point(240, 22);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(190, 99);
            this.groupBox6.TabIndex = 2;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Actions";
            // 
            // checkBoxMPAllProgram
            // 
            this.checkBoxMPAllProgram.AutoSize = true;
            this.checkBoxMPAllProgram.Location = new System.Drawing.Point(6, 69);
            this.checkBoxMPAllProgram.Name = "checkBoxMPAllProgram";
            this.checkBoxMPAllProgram.Size = new System.Drawing.Size(175, 17);
            this.checkBoxMPAllProgram.TabIndex = 4;
            this.checkBoxMPAllProgram.Text = "All MediaPortal Program Folders";
            this.checkBoxMPAllProgram.UseVisualStyleBackColor = true;
            // 
            // buttonDefault
            // 
            this.buttonDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonDefault.Location = new System.Drawing.Point(146, 234);
            this.buttonDefault.Name = "buttonDefault";
            this.buttonDefault.Size = new System.Drawing.Size(55, 26);
            this.buttonDefault.TabIndex = 10;
            this.buttonDefault.Text = "Default";
            this.buttonDefault.UseVisualStyleBackColor = true;
            this.buttonDefault.Click += new System.EventHandler(this.buttonDefault_Click);
            // 
            // CheckBoxDebugBackupSettings
            // 
            this.CheckBoxDebugBackupSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CheckBoxDebugBackupSettings.AutoSize = true;
            this.CheckBoxDebugBackupSettings.Location = new System.Drawing.Point(148, 266);
            this.CheckBoxDebugBackupSettings.Name = "CheckBoxDebugBackupSettings";
            this.CheckBoxDebugBackupSettings.Size = new System.Drawing.Size(100, 17);
            this.CheckBoxDebugBackupSettings.TabIndex = 7;
            this.CheckBoxDebugBackupSettings.Text = "Verbose Debug";
            this.CheckBoxDebugBackupSettings.UseVisualStyleBackColor = true;
            this.CheckBoxDebugBackupSettings.CheckedChanged += new System.EventHandler(this.CheckBoxDebugBackupSettings_CheckedChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.checkBoxUseAutoDate);
            this.tabPage1.Controls.Add(this.linkLabel1);
            this.tabPage1.Controls.Add(this.checkBoxMP2S);
            this.tabPage1.Controls.Add(this.checkBoxMP2C);
            this.tabPage1.Controls.Add(this.checkBoxMP);
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Controls.Add(this.listView1);
            this.tabPage1.Controls.Add(this.buttonhelp);
            this.tabPage1.Controls.Add(this.buttonClose);
            this.tabPage1.Controls.Add(this.buttonDefault);
            this.tabPage1.Controls.Add(this.CheckBoxDebugBackupSettings);
            this.tabPage1.Controls.Add(this.clearbutton);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(453, 322);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Main";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // checkBoxUseAutoDate
            // 
            this.checkBoxUseAutoDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxUseAutoDate.AutoSize = true;
            this.checkBoxUseAutoDate.Location = new System.Drawing.Point(148, 308);
            this.checkBoxUseAutoDate.Name = "checkBoxUseAutoDate";
            this.checkBoxUseAutoDate.Size = new System.Drawing.Size(96, 17);
            this.checkBoxUseAutoDate.TabIndex = 29;
            this.checkBoxUseAutoDate.Text = "Use Auto Date";
            this.checkBoxUseAutoDate.UseVisualStyleBackColor = true;
            // 
            // linkLabel1
            // 
            this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(260, 309);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(141, 13);
            this.linkLabel1.TabIndex = 28;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "BackupSettings Home Page";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // checkBoxMP2S
            // 
            this.checkBoxMP2S.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxMP2S.AutoSize = true;
            this.checkBoxMP2S.Location = new System.Drawing.Point(263, 266);
            this.checkBoxMP2S.Name = "checkBoxMP2S";
            this.checkBoxMP2S.Size = new System.Drawing.Size(125, 17);
            this.checkBoxMP2S.TabIndex = 27;
            this.checkBoxMP2S.Text = "Media Portal2 Server";
            this.checkBoxMP2S.UseVisualStyleBackColor = true;
            // 
            // checkBoxMP2C
            // 
            this.checkBoxMP2C.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxMP2C.AutoSize = true;
            this.checkBoxMP2C.Location = new System.Drawing.Point(263, 288);
            this.checkBoxMP2C.Name = "checkBoxMP2C";
            this.checkBoxMP2C.Size = new System.Drawing.Size(120, 17);
            this.checkBoxMP2C.TabIndex = 26;
            this.checkBoxMP2C.Text = "Media Portal2 Client";
            this.checkBoxMP2C.UseVisualStyleBackColor = true;
            // 
            // checkBoxMP
            // 
            this.checkBoxMP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxMP.AutoSize = true;
            this.checkBoxMP.Location = new System.Drawing.Point(148, 288);
            this.checkBoxMP.Name = "checkBoxMP";
            this.checkBoxMP.Size = new System.Drawing.Size(91, 17);
            this.checkBoxMP.TabIndex = 25;
            this.checkBoxMP.Text = "Media Portal1";
            this.checkBoxMP.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox4.Controls.Add(this.radioButtonExpert);
            this.groupBox4.Controls.Add(this.radioButtonEasy);
            this.groupBox4.Location = new System.Drawing.Point(23, 234);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(91, 68);
            this.groupBox4.TabIndex = 23;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Modus";
            // 
            // radioButtonExpert
            // 
            this.radioButtonExpert.AutoSize = true;
            this.radioButtonExpert.Location = new System.Drawing.Point(18, 42);
            this.radioButtonExpert.Name = "radioButtonExpert";
            this.radioButtonExpert.Size = new System.Drawing.Size(55, 17);
            this.radioButtonExpert.TabIndex = 16;
            this.radioButtonExpert.TabStop = true;
            this.radioButtonExpert.Text = "Expert";
            this.radioButtonExpert.UseVisualStyleBackColor = true;
            this.radioButtonExpert.CheckedChanged += new System.EventHandler(this.radioButtonExpert_CheckedChanged);
            // 
            // radioButtonEasy
            // 
            this.radioButtonEasy.AutoSize = true;
            this.radioButtonEasy.Location = new System.Drawing.Point(18, 19);
            this.radioButtonEasy.Name = "radioButtonEasy";
            this.radioButtonEasy.Size = new System.Drawing.Size(48, 17);
            this.radioButtonEasy.TabIndex = 15;
            this.radioButtonEasy.TabStop = true;
            this.radioButtonEasy.Text = "Easy";
            this.radioButtonEasy.UseVisualStyleBackColor = true;
            this.radioButtonEasy.CheckedChanged += new System.EventHandler(this.radioButtonEasy_CheckedChanged);
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listView1.Location = new System.Drawing.Point(23, 19);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(398, 209);
            this.listView1.TabIndex = 22;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Status";
            this.columnHeader1.Width = 398;
            // 
            // buttonhelp
            // 
            this.buttonhelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonhelp.Location = new System.Drawing.Point(290, 234);
            this.buttonhelp.Name = "buttonhelp";
            this.buttonhelp.Size = new System.Drawing.Size(55, 26);
            this.buttonhelp.TabIndex = 13;
            this.buttonhelp.Text = "Help";
            this.buttonhelp.UseVisualStyleBackColor = true;
            this.buttonhelp.Click += new System.EventHandler(this.buttonhelp_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonClose.Location = new System.Drawing.Point(366, 234);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(55, 26);
            this.buttonClose.TabIndex = 12;
            this.buttonClose.Text = "Exit";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // clearbutton
            // 
            this.clearbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.clearbutton.Location = new System.Drawing.Point(217, 234);
            this.clearbutton.Name = "clearbutton";
            this.clearbutton.Size = new System.Drawing.Size(55, 26);
            this.clearbutton.TabIndex = 6;
            this.clearbutton.Text = "Clear";
            this.clearbutton.UseVisualStyleBackColor = true;
            this.clearbutton.Click += new System.EventHandler(this.clearbutton_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Location = new System.Drawing.Point(9, 108);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(461, 348);
            this.tabControl1.TabIndex = 10;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.buttonMP2None);
            this.tabPage4.Controls.Add(this.buttonMP2All);
            this.tabPage4.Controls.Add(this.groupBox10);
            this.tabPage4.Controls.Add(this.groupBox9);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(453, 322);
            this.tabPage4.TabIndex = 4;
            this.tabPage4.Text = "MediaPortal2";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // buttonMP2None
            // 
            this.buttonMP2None.Location = new System.Drawing.Point(254, 269);
            this.buttonMP2None.Name = "buttonMP2None";
            this.buttonMP2None.Size = new System.Drawing.Size(57, 23);
            this.buttonMP2None.TabIndex = 14;
            this.buttonMP2None.Text = "None";
            this.buttonMP2None.UseVisualStyleBackColor = true;
            this.buttonMP2None.Click += new System.EventHandler(this.buttonMP2None_Click);
            // 
            // buttonMP2All
            // 
            this.buttonMP2All.Location = new System.Drawing.Point(130, 269);
            this.buttonMP2All.Name = "buttonMP2All";
            this.buttonMP2All.Size = new System.Drawing.Size(57, 23);
            this.buttonMP2All.TabIndex = 13;
            this.buttonMP2All.Text = "All";
            this.buttonMP2All.UseVisualStyleBackColor = true;
            this.buttonMP2All.Click += new System.EventHandler(this.buttonMP2All_Click);
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.checkBoxMP2Defaults);
            this.groupBox10.Controls.Add(this.checkBoxMP2AllClientProgramFolders);
            this.groupBox10.Controls.Add(this.checkBoxMP2AllClientFiles);
            this.groupBox10.Controls.Add(this.checkBoxMP2AllClientFolders);
            this.groupBox10.Controls.Add(this.checkBoxMP2Config);
            this.groupBox10.Controls.Add(this.checkBoxMP2Plugins);
            this.groupBox10.Location = new System.Drawing.Point(237, 31);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(190, 215);
            this.groupBox10.TabIndex = 5;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "MP2-Client";
            // 
            // checkBoxMP2Defaults
            // 
            this.checkBoxMP2Defaults.AutoSize = true;
            this.checkBoxMP2Defaults.Location = new System.Drawing.Point(6, 19);
            this.checkBoxMP2Defaults.Name = "checkBoxMP2Defaults";
            this.checkBoxMP2Defaults.Size = new System.Drawing.Size(65, 17);
            this.checkBoxMP2Defaults.TabIndex = 8;
            this.checkBoxMP2Defaults.Text = "Defaults";
            this.checkBoxMP2Defaults.UseVisualStyleBackColor = true;
            // 
            // checkBoxMP2AllClientProgramFolders
            // 
            this.checkBoxMP2AllClientProgramFolders.AutoSize = true;
            this.checkBoxMP2AllClientProgramFolders.Location = new System.Drawing.Point(6, 135);
            this.checkBoxMP2AllClientProgramFolders.Name = "checkBoxMP2AllClientProgramFolders";
            this.checkBoxMP2AllClientProgramFolders.Size = new System.Drawing.Size(170, 17);
            this.checkBoxMP2AllClientProgramFolders.TabIndex = 5;
            this.checkBoxMP2AllClientProgramFolders.Text = "All MP2 Client Program Folders";
            this.checkBoxMP2AllClientProgramFolders.UseVisualStyleBackColor = true;
            // 
            // checkBoxMP2AllClientFiles
            // 
            this.checkBoxMP2AllClientFiles.AutoSize = true;
            this.checkBoxMP2AllClientFiles.Location = new System.Drawing.Point(6, 87);
            this.checkBoxMP2AllClientFiles.Name = "checkBoxMP2AllClientFiles";
            this.checkBoxMP2AllClientFiles.Size = new System.Drawing.Size(140, 17);
            this.checkBoxMP2AllClientFiles.TabIndex = 4;
            this.checkBoxMP2AllClientFiles.Text = "All MP2 Client User Files";
            this.checkBoxMP2AllClientFiles.UseVisualStyleBackColor = true;
            // 
            // checkBoxMP2AllClientFolders
            // 
            this.checkBoxMP2AllClientFolders.AutoSize = true;
            this.checkBoxMP2AllClientFolders.Location = new System.Drawing.Point(6, 112);
            this.checkBoxMP2AllClientFolders.Name = "checkBoxMP2AllClientFolders";
            this.checkBoxMP2AllClientFolders.Size = new System.Drawing.Size(153, 17);
            this.checkBoxMP2AllClientFolders.TabIndex = 3;
            this.checkBoxMP2AllClientFolders.Text = "All MP2 Client User Folders";
            this.checkBoxMP2AllClientFolders.UseVisualStyleBackColor = true;
            // 
            // checkBoxMP2Config
            // 
            this.checkBoxMP2Config.AutoSize = true;
            this.checkBoxMP2Config.Location = new System.Drawing.Point(6, 64);
            this.checkBoxMP2Config.Name = "checkBoxMP2Config";
            this.checkBoxMP2Config.Size = new System.Drawing.Size(88, 17);
            this.checkBoxMP2Config.TabIndex = 2;
            this.checkBoxMP2Config.Text = "Configuration";
            this.checkBoxMP2Config.UseVisualStyleBackColor = true;
            // 
            // checkBoxMP2Plugins
            // 
            this.checkBoxMP2Plugins.AutoSize = true;
            this.checkBoxMP2Plugins.Location = new System.Drawing.Point(6, 41);
            this.checkBoxMP2Plugins.Name = "checkBoxMP2Plugins";
            this.checkBoxMP2Plugins.Size = new System.Drawing.Size(60, 17);
            this.checkBoxMP2Plugins.TabIndex = 1;
            this.checkBoxMP2Plugins.Text = "Plugins";
            this.checkBoxMP2Plugins.UseVisualStyleBackColor = true;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.checkBoxSV2Defaults);
            this.groupBox9.Controls.Add(this.checkBoxSV2AllServerProgramFolders);
            this.groupBox9.Controls.Add(this.checkBoxSV2AllServerFiles);
            this.groupBox9.Controls.Add(this.checkBoxSV2Config);
            this.groupBox9.Controls.Add(this.checkBoxSV2AllServerFolders);
            this.groupBox9.Controls.Add(this.checkBoxSV2Database);
            this.groupBox9.Controls.Add(this.checkBoxSV2Plugins);
            this.groupBox9.Location = new System.Drawing.Point(23, 31);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(190, 215);
            this.groupBox9.TabIndex = 4;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "MP2-Server";
            // 
            // checkBoxSV2Defaults
            // 
            this.checkBoxSV2Defaults.AutoSize = true;
            this.checkBoxSV2Defaults.Location = new System.Drawing.Point(6, 18);
            this.checkBoxSV2Defaults.Name = "checkBoxSV2Defaults";
            this.checkBoxSV2Defaults.Size = new System.Drawing.Size(65, 17);
            this.checkBoxSV2Defaults.TabIndex = 7;
            this.checkBoxSV2Defaults.Text = "Defaults";
            this.checkBoxSV2Defaults.UseVisualStyleBackColor = true;
            // 
            // checkBoxSV2AllServerProgramFolders
            // 
            this.checkBoxSV2AllServerProgramFolders.AutoSize = true;
            this.checkBoxSV2AllServerProgramFolders.Location = new System.Drawing.Point(6, 158);
            this.checkBoxSV2AllServerProgramFolders.Name = "checkBoxSV2AllServerProgramFolders";
            this.checkBoxSV2AllServerProgramFolders.Size = new System.Drawing.Size(175, 17);
            this.checkBoxSV2AllServerProgramFolders.TabIndex = 6;
            this.checkBoxSV2AllServerProgramFolders.Text = "All MP2 Server Program Folders";
            this.checkBoxSV2AllServerProgramFolders.UseVisualStyleBackColor = true;
            // 
            // checkBoxSV2AllServerFiles
            // 
            this.checkBoxSV2AllServerFiles.AutoSize = true;
            this.checkBoxSV2AllServerFiles.Location = new System.Drawing.Point(6, 112);
            this.checkBoxSV2AllServerFiles.Name = "checkBoxSV2AllServerFiles";
            this.checkBoxSV2AllServerFiles.Size = new System.Drawing.Size(145, 17);
            this.checkBoxSV2AllServerFiles.TabIndex = 5;
            this.checkBoxSV2AllServerFiles.Text = "All MP2 Server User Files";
            this.checkBoxSV2AllServerFiles.UseVisualStyleBackColor = true;
            // 
            // checkBoxSV2Config
            // 
            this.checkBoxSV2Config.AutoSize = true;
            this.checkBoxSV2Config.Location = new System.Drawing.Point(6, 87);
            this.checkBoxSV2Config.Name = "checkBoxSV2Config";
            this.checkBoxSV2Config.Size = new System.Drawing.Size(88, 17);
            this.checkBoxSV2Config.TabIndex = 4;
            this.checkBoxSV2Config.Text = "Configuration";
            this.checkBoxSV2Config.UseVisualStyleBackColor = true;
            // 
            // checkBoxSV2AllServerFolders
            // 
            this.checkBoxSV2AllServerFolders.AutoSize = true;
            this.checkBoxSV2AllServerFolders.Location = new System.Drawing.Point(6, 135);
            this.checkBoxSV2AllServerFolders.Name = "checkBoxSV2AllServerFolders";
            this.checkBoxSV2AllServerFolders.Size = new System.Drawing.Size(158, 17);
            this.checkBoxSV2AllServerFolders.TabIndex = 3;
            this.checkBoxSV2AllServerFolders.Text = "All MP2 Server User Folders";
            this.checkBoxSV2AllServerFolders.UseVisualStyleBackColor = true;
            // 
            // checkBoxSV2Database
            // 
            this.checkBoxSV2Database.AutoSize = true;
            this.checkBoxSV2Database.Location = new System.Drawing.Point(6, 64);
            this.checkBoxSV2Database.Name = "checkBoxSV2Database";
            this.checkBoxSV2Database.Size = new System.Drawing.Size(76, 17);
            this.checkBoxSV2Database.TabIndex = 2;
            this.checkBoxSV2Database.Text = "Data Base";
            this.checkBoxSV2Database.UseVisualStyleBackColor = true;
            // 
            // checkBoxSV2Plugins
            // 
            this.checkBoxSV2Plugins.AutoSize = true;
            this.checkBoxSV2Plugins.Location = new System.Drawing.Point(6, 41);
            this.checkBoxSV2Plugins.Name = "checkBoxSV2Plugins";
            this.checkBoxSV2Plugins.Size = new System.Drawing.Size(60, 17);
            this.checkBoxSV2Plugins.TabIndex = 1;
            this.checkBoxSV2Plugins.Text = "Plugins";
            this.checkBoxSV2Plugins.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.button1);
            this.tabPage2.Controls.Add(this.buttonActiveNone);
            this.tabPage2.Controls.Add(this.buttonSelectFolder);
            this.tabPage2.Controls.Add(this.buttonDelete);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.dataGridView1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(453, 322);
            this.tabPage2.TabIndex = 3;
            this.tabPage2.Text = "Extra Folders";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(20, 239);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(82, 26);
            this.button1.TabIndex = 77;
            this.button1.Text = "All Active";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.buttonActiveAll_Click);
            // 
            // buttonActiveNone
            // 
            this.buttonActiveNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonActiveNone.Location = new System.Drawing.Point(134, 239);
            this.buttonActiveNone.Name = "buttonActiveNone";
            this.buttonActiveNone.Size = new System.Drawing.Size(82, 26);
            this.buttonActiveNone.TabIndex = 76;
            this.buttonActiveNone.Text = "None Active";
            this.buttonActiveNone.UseVisualStyleBackColor = true;
            this.buttonActiveNone.Click += new System.EventHandler(this.buttonActiveNone_Click);
            // 
            // buttonSelectFolder
            // 
            this.buttonSelectFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSelectFolder.Location = new System.Drawing.Point(245, 239);
            this.buttonSelectFolder.Name = "buttonSelectFolder";
            this.buttonSelectFolder.Size = new System.Drawing.Size(82, 26);
            this.buttonSelectFolder.TabIndex = 75;
            this.buttonSelectFolder.Text = "Select Dialog";
            this.buttonSelectFolder.UseVisualStyleBackColor = true;
            this.buttonSelectFolder.Click += new System.EventHandler(this.buttonSelectFolder_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonDelete.Location = new System.Drawing.Point(355, 239);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(82, 26);
            this.buttonDelete.TabIndex = 74;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(17, 293);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(293, 13);
            this.label9.TabIndex = 73;
            this.label9.Text = "Restart will start the specified process after a backup/restore";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(17, 280);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(344, 13);
            this.label8.TabIndex = 72;
            this.label8.Text = "Kill process will terminate the specified process before a backup/restore";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(16, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(263, 20);
            this.label7.TabIndex = 71;
            this.label7.Text = "Backup / Restore Additional Folders";
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeight = 25;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column7,
            this.Column3,
            this.Column8,
            this.Column9});
            this.dataGridView1.Location = new System.Drawing.Point(20, 55);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 25;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.Size = new System.Drawing.Size(417, 178);
            this.dataGridView1.TabIndex = 70;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Active";
            this.Column1.Name = "Column1";
            this.Column1.Width = 43;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column2.HeaderText = "Name";
            this.Column2.Name = "Column2";
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column2.Width = 41;
            // 
            // Column7
            // 
            this.Column7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column7.HeaderText = "Folder";
            this.Column7.Name = "Column7";
            this.Column7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column7.Width = 42;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Overwrite";
            this.Column3.Name = "Column3";
            this.Column3.Width = 58;
            // 
            // Column8
            // 
            this.Column8.HeaderText = "Kill Process";
            this.Column8.Name = "Column8";
            this.Column8.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column8.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column8.Width = 67;
            // 
            // Column9
            // 
            this.Column9.HeaderText = "Restart";
            this.Column9.Name = "Column9";
            this.Column9.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column9.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column9.Width = 47;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.buttonDetect);
            this.tabPage5.Controls.Add(this.buttonSave);
            this.tabPage5.Controls.Add(this.buttonCancel);
            this.tabPage5.Controls.Add(this.tabControl2);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(453, 322);
            this.tabPage5.TabIndex = 5;
            this.tabPage5.Text = "Paths";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // buttonDetect
            // 
            this.buttonDetect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDetect.Location = new System.Drawing.Point(119, 277);
            this.buttonDetect.Name = "buttonDetect";
            this.buttonDetect.Size = new System.Drawing.Size(55, 26);
            this.buttonDetect.TabIndex = 24;
            this.buttonDetect.Text = "Detect";
            this.buttonDetect.UseVisualStyleBackColor = true;
            this.buttonDetect.Click += new System.EventHandler(this.buttonDetect_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.Location = new System.Drawing.Point(193, 277);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(55, 26);
            this.buttonSave.TabIndex = 22;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(269, 277);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(55, 26);
            this.buttonCancel.TabIndex = 23;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage8);
            this.tabControl2.Controls.Add(this.tabPage9);
            this.tabControl2.Location = new System.Drawing.Point(23, 24);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(396, 247);
            this.tabControl2.TabIndex = 21;
            // 
            // tabPage8
            // 
            this.tabPage8.Controls.Add(this.label17);
            this.tabPage8.Controls.Add(this.label10);
            this.tabPage8.Controls.Add(this.label11);
            this.tabPage8.Controls.Add(this.label4);
            this.tabPage8.Controls.Add(this.buttonMP1P);
            this.tabPage8.Controls.Add(this.buttonMP1U);
            this.tabPage8.Controls.Add(this.buttonTV1P);
            this.tabPage8.Controls.Add(this.buttonTV1U);
            this.tabPage8.Controls.Add(this.textBoxTV1U);
            this.tabPage8.Controls.Add(this.textBoxTV1P);
            this.tabPage8.Controls.Add(this.textBoxMP1U);
            this.tabPage8.Controls.Add(this.textBoxMP1P);
            this.tabPage8.Location = new System.Drawing.Point(4, 22);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage8.Size = new System.Drawing.Size(388, 221);
            this.tabPage8.TabIndex = 0;
            this.tabPage8.Text = "MediaPortal1";
            this.tabPage8.UseVisualStyleBackColor = true;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(15, 171);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(129, 13);
            this.label17.TabIndex = 34;
            this.label17.Text = "TvServer1 Users (Config):";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(15, 65);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(141, 13);
            this.label10.TabIndex = 33;
            this.label10.Text = "MediaPortal1 Users (Config):";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(15, 116);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(140, 13);
            this.label11.TabIndex = 32;
            this.label11.Text = "TvServer1 Programs (Base):";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(152, 13);
            this.label4.TabIndex = 31;
            this.label4.Text = "MediaPortal1 Programs (Base):";
            // 
            // buttonMP1P
            // 
            this.buttonMP1P.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMP1P.Location = new System.Drawing.Point(356, 30);
            this.buttonMP1P.Name = "buttonMP1P";
            this.buttonMP1P.Size = new System.Drawing.Size(26, 26);
            this.buttonMP1P.TabIndex = 30;
            this.buttonMP1P.Text = "...";
            this.buttonMP1P.UseVisualStyleBackColor = true;
            this.buttonMP1P.Click += new System.EventHandler(this.buttonMP1P_Click);
            // 
            // buttonMP1U
            // 
            this.buttonMP1U.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMP1U.Location = new System.Drawing.Point(356, 75);
            this.buttonMP1U.Name = "buttonMP1U";
            this.buttonMP1U.Size = new System.Drawing.Size(26, 26);
            this.buttonMP1U.TabIndex = 29;
            this.buttonMP1U.Text = "...";
            this.buttonMP1U.UseVisualStyleBackColor = true;
            this.buttonMP1U.Click += new System.EventHandler(this.buttonMP1U_Click);
            // 
            // buttonTV1P
            // 
            this.buttonTV1P.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTV1P.Location = new System.Drawing.Point(356, 126);
            this.buttonTV1P.Name = "buttonTV1P";
            this.buttonTV1P.Size = new System.Drawing.Size(26, 26);
            this.buttonTV1P.TabIndex = 28;
            this.buttonTV1P.Text = "...";
            this.buttonTV1P.UseVisualStyleBackColor = true;
            this.buttonTV1P.Click += new System.EventHandler(this.buttonTV1P_Click);
            // 
            // buttonTV1U
            // 
            this.buttonTV1U.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTV1U.Location = new System.Drawing.Point(356, 178);
            this.buttonTV1U.Name = "buttonTV1U";
            this.buttonTV1U.Size = new System.Drawing.Size(26, 26);
            this.buttonTV1U.TabIndex = 27;
            this.buttonTV1U.Text = "...";
            this.buttonTV1U.UseVisualStyleBackColor = true;
            this.buttonTV1U.Click += new System.EventHandler(this.buttonTV1U_Click);
            // 
            // textBoxTV1U
            // 
            this.textBoxTV1U.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTV1U.Location = new System.Drawing.Point(18, 184);
            this.textBoxTV1U.Name = "textBoxTV1U";
            this.textBoxTV1U.Size = new System.Drawing.Size(335, 20);
            this.textBoxTV1U.TabIndex = 26;
            // 
            // textBoxTV1P
            // 
            this.textBoxTV1P.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTV1P.Location = new System.Drawing.Point(18, 132);
            this.textBoxTV1P.Name = "textBoxTV1P";
            this.textBoxTV1P.Size = new System.Drawing.Size(335, 20);
            this.textBoxTV1P.TabIndex = 25;
            // 
            // textBoxMP1U
            // 
            this.textBoxMP1U.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMP1U.Location = new System.Drawing.Point(18, 81);
            this.textBoxMP1U.Name = "textBoxMP1U";
            this.textBoxMP1U.Size = new System.Drawing.Size(332, 20);
            this.textBoxMP1U.TabIndex = 24;
            // 
            // textBoxMP1P
            // 
            this.textBoxMP1P.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMP1P.Location = new System.Drawing.Point(18, 36);
            this.textBoxMP1P.Name = "textBoxMP1P";
            this.textBoxMP1P.Size = new System.Drawing.Size(335, 20);
            this.textBoxMP1P.TabIndex = 23;
            // 
            // tabPage9
            // 
            this.tabPage9.Controls.Add(this.label12);
            this.tabPage9.Controls.Add(this.buttonSV2U);
            this.tabPage9.Controls.Add(this.textBoxSV2U);
            this.tabPage9.Controls.Add(this.buttonMP2P);
            this.tabPage9.Controls.Add(this.label13);
            this.tabPage9.Controls.Add(this.buttonMP2U);
            this.tabPage9.Controls.Add(this.label14);
            this.tabPage9.Controls.Add(this.buttonSV2P);
            this.tabPage9.Controls.Add(this.label15);
            this.tabPage9.Controls.Add(this.textBoxSV2P);
            this.tabPage9.Controls.Add(this.textBoxMP2U);
            this.tabPage9.Controls.Add(this.textBoxMP2P);
            this.tabPage9.Location = new System.Drawing.Point(4, 22);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage9.Size = new System.Drawing.Size(388, 221);
            this.tabPage9.TabIndex = 1;
            this.tabPage9.Text = "MediaPortal2";
            this.tabPage9.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(17, 169);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(135, 13);
            this.label12.TabIndex = 37;
            this.label12.Text = "MP2 Server Users (Config):";
            // 
            // buttonSV2U
            // 
            this.buttonSV2U.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSV2U.Location = new System.Drawing.Point(356, 179);
            this.buttonSV2U.Name = "buttonSV2U";
            this.buttonSV2U.Size = new System.Drawing.Size(26, 26);
            this.buttonSV2U.TabIndex = 36;
            this.buttonSV2U.Text = "...";
            this.buttonSV2U.UseVisualStyleBackColor = true;
            this.buttonSV2U.Click += new System.EventHandler(this.buttonSV2U_Click);
            // 
            // textBoxSV2U
            // 
            this.textBoxSV2U.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSV2U.Location = new System.Drawing.Point(20, 185);
            this.textBoxSV2U.Name = "textBoxSV2U";
            this.textBoxSV2U.Size = new System.Drawing.Size(330, 20);
            this.textBoxSV2U.TabIndex = 35;
            // 
            // buttonMP2P
            // 
            this.buttonMP2P.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMP2P.Location = new System.Drawing.Point(356, 29);
            this.buttonMP2P.Name = "buttonMP2P";
            this.buttonMP2P.Size = new System.Drawing.Size(26, 26);
            this.buttonMP2P.TabIndex = 31;
            this.buttonMP2P.Text = "...";
            this.buttonMP2P.UseVisualStyleBackColor = true;
            this.buttonMP2P.Click += new System.EventHandler(this.buttonMP2P_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(17, 64);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(130, 13);
            this.label13.TabIndex = 34;
            this.label13.Text = "MP2 Client Users (Config):";
            // 
            // buttonMP2U
            // 
            this.buttonMP2U.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMP2U.Location = new System.Drawing.Point(356, 74);
            this.buttonMP2U.Name = "buttonMP2U";
            this.buttonMP2U.Size = new System.Drawing.Size(26, 26);
            this.buttonMP2U.TabIndex = 30;
            this.buttonMP2U.Text = "...";
            this.buttonMP2U.UseVisualStyleBackColor = true;
            this.buttonMP2U.Click += new System.EventHandler(this.buttonMP2U_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(17, 114);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(146, 13);
            this.label14.TabIndex = 33;
            this.label14.Text = "MP2 Server Programs (Base):";
            // 
            // buttonSV2P
            // 
            this.buttonSV2P.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSV2P.Location = new System.Drawing.Point(356, 124);
            this.buttonSV2P.Name = "buttonSV2P";
            this.buttonSV2P.Size = new System.Drawing.Size(26, 26);
            this.buttonSV2P.TabIndex = 29;
            this.buttonSV2P.Text = "...";
            this.buttonSV2P.UseVisualStyleBackColor = true;
            this.buttonSV2P.Click += new System.EventHandler(this.buttonSV2P_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(17, 19);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(141, 13);
            this.label15.TabIndex = 32;
            this.label15.Text = "MP2 Client Programs (Base):";
            // 
            // textBoxSV2P
            // 
            this.textBoxSV2P.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSV2P.Location = new System.Drawing.Point(20, 130);
            this.textBoxSV2P.Name = "textBoxSV2P";
            this.textBoxSV2P.Size = new System.Drawing.Size(330, 20);
            this.textBoxSV2P.TabIndex = 28;
            // 
            // textBoxMP2U
            // 
            this.textBoxMP2U.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMP2U.Location = new System.Drawing.Point(20, 80);
            this.textBoxMP2U.Name = "textBoxMP2U";
            this.textBoxMP2U.Size = new System.Drawing.Size(330, 20);
            this.textBoxMP2U.TabIndex = 27;
            // 
            // textBoxMP2P
            // 
            this.textBoxMP2P.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMP2P.Location = new System.Drawing.Point(20, 35);
            this.textBoxMP2P.Name = "textBoxMP2P";
            this.textBoxMP2P.Size = new System.Drawing.Size(330, 20);
            this.textBoxMP2P.TabIndex = 26;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Foldername";
            // 
            // selectfilenamebutton
            // 
            this.selectfilenamebutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.selectfilenamebutton.Location = new System.Drawing.Point(323, 37);
            this.selectfilenamebutton.Name = "selectfilenamebutton";
            this.selectfilenamebutton.Size = new System.Drawing.Size(26, 26);
            this.selectfilenamebutton.TabIndex = 5;
            this.selectfilenamebutton.Text = "...";
            this.selectfilenamebutton.UseVisualStyleBackColor = true;
            this.selectfilenamebutton.Click += new System.EventHandler(this.selectfilenamebutton_Click);
            // 
            // filenametextBox
            // 
            this.filenametextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.filenametextBox.Location = new System.Drawing.Point(27, 41);
            this.filenametextBox.Name = "filenametextBox";
            this.filenametextBox.Size = new System.Drawing.Size(290, 20);
            this.filenametextBox.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.buttonCreateAutoFolder);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.selectfilenamebutton);
            this.groupBox1.Controls.Add(this.filenametextBox);
            this.groupBox1.Controls.Add(this.exportbutton);
            this.groupBox1.Controls.Add(this.importbutton);
            this.groupBox1.Location = new System.Drawing.Point(9, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(461, 74);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Import / Export";
            // 
            // buttonCreateAutoFolder
            // 
            this.buttonCreateAutoFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCreateAutoFolder.Location = new System.Drawing.Point(355, 37);
            this.buttonCreateAutoFolder.Name = "buttonCreateAutoFolder";
            this.buttonCreateAutoFolder.Size = new System.Drawing.Size(26, 26);
            this.buttonCreateAutoFolder.TabIndex = 7;
            this.buttonCreateAutoFolder.Text = "C";
            this.buttonCreateAutoFolder.UseVisualStyleBackColor = true;
            this.buttonCreateAutoFolder.Click += new System.EventHandler(this.buttonCreateAutoFolder_Click);
            // 
            // exportbutton
            // 
            this.exportbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exportbutton.Location = new System.Drawing.Point(389, 14);
            this.exportbutton.Name = "exportbutton";
            this.exportbutton.Size = new System.Drawing.Size(55, 26);
            this.exportbutton.TabIndex = 4;
            this.exportbutton.Text = "Export";
            this.exportbutton.UseVisualStyleBackColor = true;
            this.exportbutton.Click += new System.EventHandler(this.exportbutton_Click);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(9, 10);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(461, 12);
            this.progressBar.TabIndex = 11;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(478, 460);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "MediaPortal BackupSettingsMP V1.2.2.10";
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabPage5.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage8.ResumeLayout(false);
            this.tabPage8.PerformLayout();
            this.tabPage9.ResumeLayout(false);
            this.tabPage9.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        private Button buttonClose;
        private CheckBox checkBoxMPAllProgram;
        private Button buttonhelp;

        #endregion
        private ProgressBar progressBar;
        private ListView listView1;
        private ColumnHeader columnHeader1;
        private TabPage tabPage2;
        private Button button1;
        private Button buttonActiveNone;
        private Button buttonSelectFolder;
        private Button buttonDelete;
        private Label label9;
        private Label label8;
        private Label label7;
        private DataGridView dataGridView1;
        private DataGridViewCheckBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column7;
        private DataGridViewCheckBoxColumn Column3;
        private DataGridViewTextBoxColumn Column8;
        private DataGridViewTextBoxColumn Column9;
        private Button buttonCreateAutoFolder;
        private GroupBox groupBox4;
        private RadioButton radioButtonExpert;
        private RadioButton radioButtonEasy;
        private LinkLabel linkLabel1;
        private CheckBox checkBoxMP2S;
        private CheckBox checkBoxMP2C;
        private CheckBox checkBoxMP;
        private TabPage tabPage4;
        private Button buttonMP2None;
        private Button buttonMP2All;
        private GroupBox groupBox10;
        private CheckBox checkBoxMP2Defaults;
        private CheckBox checkBoxMP2AllClientProgramFolders;
        private CheckBox checkBoxMP2AllClientFiles;
        private CheckBox checkBoxMP2AllClientFolders;
        private CheckBox checkBoxMP2Config;
        private CheckBox checkBoxMP2Plugins;
        private GroupBox groupBox9;
        private CheckBox checkBoxSV2Defaults;
        private CheckBox checkBoxSV2AllServerProgramFolders;
        private CheckBox checkBoxSV2AllServerFiles;
        private CheckBox checkBoxSV2Config;
        private CheckBox checkBoxSV2AllServerFolders;
        private CheckBox checkBoxSV2Database;
        private CheckBox checkBoxSV2Plugins;
        private TabPage tabPage5;
        private Button buttonDetect;
        private Button buttonSave;
        private Button buttonCancel;
        private TabControl tabControl2;
        private TabPage tabPage8;
        private Label label17;
        private Label label10;
        private Label label11;
        private Label label4;
        private Button buttonMP1P;
        private Button buttonMP1U;
        private Button buttonTV1P;
        private Button buttonTV1U;
        private TextBox textBoxTV1U;
        private TextBox textBoxTV1P;
        private TextBox textBoxMP1U;
        private TextBox textBoxMP1P;
        private TabPage tabPage9;
        private Label label12;
        private Button buttonSV2U;
        private TextBox textBoxSV2U;
        private Button buttonMP2P;
        private Label label13;
        private Button buttonMP2U;
        private Label label14;
        private Button buttonSV2P;
        private Label label15;
        private TextBox textBoxSV2P;
        private TextBox textBoxMP2U;
        private TextBox textBoxMP2P;
        private CheckBox checkBoxUseAutoDate;
    }
}

