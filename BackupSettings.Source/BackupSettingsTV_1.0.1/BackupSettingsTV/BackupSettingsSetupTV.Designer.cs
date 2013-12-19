/* 
 *	Copyright (C) 2005-2008 Team MediaPortal
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

using System.Windows.Forms;

namespace SetupTv.Sections
{
    partial class BackupSettingsSetup
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (BUSY == true)
            {
                MessageBox.Show("Processing ongoing - please wait","Warning");   
            }

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.importbutton = new System.Windows.Forms.Button();
            this.filenametextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.exportbutton = new System.Windows.Forms.Button();
            this.selectfilenamebutton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonCreateAutoFolder = new System.Windows.Forms.Button();
            this.clearbutton = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.checkBoxUseAutoDate = new System.Windows.Forms.CheckBox();
            this.checkBoxMP2S = new System.Windows.Forms.CheckBox();
            this.checkBoxMP2C = new System.Windows.Forms.CheckBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.radioButtonExpert = new System.Windows.Forms.RadioButton();
            this.radioButtonEasy = new System.Windows.Forms.RadioButton();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.buttonhelp = new System.Windows.Forms.Button();
            this.buttonDefault = new System.Windows.Forms.Button();
            this.checkBoxMP = new System.Windows.Forms.CheckBox();
            this.checkBoxTV = new System.Windows.Forms.CheckBox();
            this.CheckBoxDebugBackupSettings = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.buttonTvNone = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.checkBoxAutoCorrectDataBase = new System.Windows.Forms.CheckBox();
            this.checkBoxTVallFolders = new System.Windows.Forms.CheckBox();
            this.checkBoxTVallPrograms = new System.Windows.Forms.CheckBox();
            this.checkBoxTVServer = new System.Windows.Forms.CheckBox();
            this.buttonTvAll = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBoxtvrestart = new System.Windows.Forms.CheckBox();
            this.checkBoxEPG = new System.Windows.Forms.CheckBox();
            this.checkBoxRecordings = new System.Windows.Forms.CheckBox();
            this.checkBoxRadioGroups = new System.Windows.Forms.CheckBox();
            this.checkBoxChannelGroups = new System.Windows.Forms.CheckBox();
            this.checkBoxClickfinder = new System.Windows.Forms.CheckBox();
            this.checkBoxServer = new System.Windows.Forms.CheckBox();
            this.checkBoxAllSettings = new System.Windows.Forms.CheckBox();
            this.checkBoxChannels = new System.Windows.Forms.CheckBox();
            this.checkBoxSchedules = new System.Windows.Forms.CheckBox();
            this.checkBoxChannelCardMapping = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxDeleteRecordings = new System.Windows.Forms.CheckBox();
            this.checkBoxDeleteSchedules = new System.Windows.Forms.CheckBox();
            this.checkBoxDeleteRadioGroups = new System.Windows.Forms.CheckBox();
            this.checkBoxDeleteTVGroups = new System.Windows.Forms.CheckBox();
            this.checkBoxDeleteChannels = new System.Windows.Forms.CheckBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.buttonMpNone = new System.Windows.Forms.Button();
            this.buttonMpAll = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.checkBoxMPAllProgram = new System.Windows.Forms.CheckBox();
            this.checkBoxMPAllFolders = new System.Windows.Forms.CheckBox();
            this.checkBoxMPDeleteCache = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.checkBoxMPMusicPlayer = new System.Windows.Forms.CheckBox();
            this.checkBoxMPxmltv = new System.Windows.Forms.CheckBox();
            this.checkBoxMPThumbs = new System.Windows.Forms.CheckBox();
            this.checkBoxMPInputDevice = new System.Windows.Forms.CheckBox();
            this.checkBoxMPDatabase = new System.Windows.Forms.CheckBox();
            this.checkBoxMPSkins = new System.Windows.Forms.CheckBox();
            this.checkBoxMPPlugins = new System.Windows.Forms.CheckBox();
            this.checkBoxMPProgramXml = new System.Windows.Forms.CheckBox();
            this.checkBoxMPUserXML = new System.Windows.Forms.CheckBox();
            this.tabPage7 = new System.Windows.Forms.TabPage();
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
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonundo = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.checkBoxduplicateautoprocess = new System.Windows.Forms.CheckBox();
            this.checkBoxduplicateinteractive = new System.Windows.Forms.CheckBox();
            this.buttonCheck = new System.Windows.Forms.Button();
            this.buttonprocess = new System.Windows.Forms.Button();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.buttonActiveAll = new System.Windows.Forms.Button();
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
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.buttonDetect = new System.Windows.Forms.Button();
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
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.tabPage10 = new System.Windows.Forms.TabPage();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.label27 = new System.Windows.Forms.Label();
            this.textBoxAutomatedExportFolderName = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.checkBoxAny = new System.Windows.Forms.CheckBox();
            this.labelLastCheckDate = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.comboBoxKeepNumber = new System.Windows.Forms.ComboBox();
            this.label25 = new System.Windows.Forms.Label();
            this.checkBoxEnableScheduler = new System.Windows.Forms.CheckBox();
            this.label24 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.checkBoxSun = new System.Windows.Forms.CheckBox();
            this.checkBoxSat = new System.Windows.Forms.CheckBox();
            this.checkBoxFri = new System.Windows.Forms.CheckBox();
            this.checkBoxThur = new System.Windows.Forms.CheckBox();
            this.checkBoxWed = new System.Windows.Forms.CheckBox();
            this.checkBoxTue = new System.Windows.Forms.CheckBox();
            this.label19 = new System.Windows.Forms.Label();
            this.checkBoxMon = new System.Windows.Forms.CheckBox();
            this.label20 = new System.Windows.Forms.Label();
            this.comboBoxminutes = new System.Windows.Forms.ComboBox();
            this.comboBoxhours = new System.Windows.Forms.ComboBox();
            this.comboBoxdays = new System.Windows.Forms.ComboBox();
            this.labelCheckingdate = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabPage6.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage8.SuspendLayout();
            this.tabPage9.SuspendLayout();
            this.tabPage10.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.SuspendLayout();
            // 
            // importbutton
            // 
            this.importbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.importbutton.Location = new System.Drawing.Point(378, 42);
            this.importbutton.Name = "importbutton";
            this.importbutton.Size = new System.Drawing.Size(55, 26);
            this.importbutton.TabIndex = 0;
            this.importbutton.Text = "Import";
            this.importbutton.UseVisualStyleBackColor = true;
            this.importbutton.Click += new System.EventHandler(this.importbutton_Click);
            // 
            // filenametextBox
            // 
            this.filenametextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.filenametextBox.Location = new System.Drawing.Point(6, 41);
            this.filenametextBox.Name = "filenametextBox";
            this.filenametextBox.Size = new System.Drawing.Size(296, 20);
            this.filenametextBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Foldername";
            // 
            // exportbutton
            // 
            this.exportbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exportbutton.Location = new System.Drawing.Point(378, 14);
            this.exportbutton.Name = "exportbutton";
            this.exportbutton.Size = new System.Drawing.Size(55, 26);
            this.exportbutton.TabIndex = 4;
            this.exportbutton.Text = "Export";
            this.exportbutton.UseVisualStyleBackColor = true;
            this.exportbutton.Click += new System.EventHandler(this.exportbutton_Click);
            // 
            // selectfilenamebutton
            // 
            this.selectfilenamebutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.selectfilenamebutton.Location = new System.Drawing.Point(308, 37);
            this.selectfilenamebutton.Name = "selectfilenamebutton";
            this.selectfilenamebutton.Size = new System.Drawing.Size(26, 26);
            this.selectfilenamebutton.TabIndex = 5;
            this.selectfilenamebutton.Text = "...";
            this.selectfilenamebutton.UseVisualStyleBackColor = true;
            this.selectfilenamebutton.Click += new System.EventHandler(this.selectfilenamebutton_Click);
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
            this.groupBox1.Location = new System.Drawing.Point(16, 20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(450, 74);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Import / Export";
            // 
            // buttonCreateAutoFolder
            // 
            this.buttonCreateAutoFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCreateAutoFolder.Location = new System.Drawing.Point(340, 37);
            this.buttonCreateAutoFolder.Name = "buttonCreateAutoFolder";
            this.buttonCreateAutoFolder.Size = new System.Drawing.Size(26, 26);
            this.buttonCreateAutoFolder.TabIndex = 6;
            this.buttonCreateAutoFolder.Text = "C";
            this.buttonCreateAutoFolder.UseVisualStyleBackColor = true;
            this.buttonCreateAutoFolder.Click += new System.EventHandler(this.buttonCreateAutoFolder_Click);
            // 
            // clearbutton
            // 
            this.clearbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.clearbutton.Location = new System.Drawing.Point(157, 228);
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
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage7);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Controls.Add(this.tabPage10);
            this.tabControl1.Location = new System.Drawing.Point(16, 100);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(450, 337);
            this.tabControl1.TabIndex = 8;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.checkBoxUseAutoDate);
            this.tabPage1.Controls.Add(this.checkBoxMP2S);
            this.tabPage1.Controls.Add(this.checkBoxMP2C);
            this.tabPage1.Controls.Add(this.groupBox7);
            this.tabPage1.Controls.Add(this.listView1);
            this.tabPage1.Controls.Add(this.linkLabel1);
            this.tabPage1.Controls.Add(this.buttonhelp);
            this.tabPage1.Controls.Add(this.buttonDefault);
            this.tabPage1.Controls.Add(this.checkBoxMP);
            this.tabPage1.Controls.Add(this.checkBoxTV);
            this.tabPage1.Controls.Add(this.CheckBoxDebugBackupSettings);
            this.tabPage1.Controls.Add(this.clearbutton);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(442, 311);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Main";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // checkBoxUseAutoDate
            // 
            this.checkBoxUseAutoDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxUseAutoDate.AutoSize = true;
            this.checkBoxUseAutoDate.Location = new System.Drawing.Point(336, 263);
            this.checkBoxUseAutoDate.Name = "checkBoxUseAutoDate";
            this.checkBoxUseAutoDate.Size = new System.Drawing.Size(96, 17);
            this.checkBoxUseAutoDate.TabIndex = 25;
            this.checkBoxUseAutoDate.Text = "Use Auto Date";
            this.checkBoxUseAutoDate.UseVisualStyleBackColor = true;
            // 
            // checkBoxMP2S
            // 
            this.checkBoxMP2S.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxMP2S.AutoSize = true;
            this.checkBoxMP2S.Location = new System.Drawing.Point(193, 260);
            this.checkBoxMP2S.Name = "checkBoxMP2S";
            this.checkBoxMP2S.Size = new System.Drawing.Size(125, 17);
            this.checkBoxMP2S.TabIndex = 24;
            this.checkBoxMP2S.Text = "Media Portal2 Server";
            this.checkBoxMP2S.UseVisualStyleBackColor = true;
            // 
            // checkBoxMP2C
            // 
            this.checkBoxMP2C.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxMP2C.AutoSize = true;
            this.checkBoxMP2C.Location = new System.Drawing.Point(193, 283);
            this.checkBoxMP2C.Name = "checkBoxMP2C";
            this.checkBoxMP2C.Size = new System.Drawing.Size(120, 17);
            this.checkBoxMP2C.TabIndex = 23;
            this.checkBoxMP2C.Text = "Media Portal2 Client";
            this.checkBoxMP2C.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox7.Controls.Add(this.radioButtonExpert);
            this.groupBox7.Controls.Add(this.radioButtonEasy);
            this.groupBox7.Location = new System.Drawing.Point(23, 226);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(67, 74);
            this.groupBox7.TabIndex = 22;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Modus";
            // 
            // radioButtonExpert
            // 
            this.radioButtonExpert.AutoSize = true;
            this.radioButtonExpert.Location = new System.Drawing.Point(9, 37);
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
            this.radioButtonEasy.Location = new System.Drawing.Point(9, 19);
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
            this.listView1.Location = new System.Drawing.Point(23, 6);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(392, 214);
            this.listView1.TabIndex = 21;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Status";
            this.columnHeader1.Width = 398;
            // 
            // linkLabel1
            // 
            this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(279, 241);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(141, 13);
            this.linkLabel1.TabIndex = 20;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "BackupSettings Home Page";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // buttonhelp
            // 
            this.buttonhelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonhelp.Location = new System.Drawing.Point(218, 228);
            this.buttonhelp.Name = "buttonhelp";
            this.buttonhelp.Size = new System.Drawing.Size(55, 26);
            this.buttonhelp.TabIndex = 12;
            this.buttonhelp.Text = "Help";
            this.buttonhelp.UseVisualStyleBackColor = true;
            this.buttonhelp.Click += new System.EventHandler(this.buttonhelp_Click);
            // 
            // buttonDefault
            // 
            this.buttonDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDefault.Location = new System.Drawing.Point(96, 228);
            this.buttonDefault.Name = "buttonDefault";
            this.buttonDefault.Size = new System.Drawing.Size(55, 26);
            this.buttonDefault.TabIndex = 10;
            this.buttonDefault.Text = "Default";
            this.buttonDefault.UseVisualStyleBackColor = true;
            this.buttonDefault.Click += new System.EventHandler(this.buttonDefault_Click);
            // 
            // checkBoxMP
            // 
            this.checkBoxMP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxMP.AutoSize = true;
            this.checkBoxMP.Location = new System.Drawing.Point(96, 283);
            this.checkBoxMP.Name = "checkBoxMP";
            this.checkBoxMP.Size = new System.Drawing.Size(91, 17);
            this.checkBoxMP.TabIndex = 9;
            this.checkBoxMP.Text = "Media Portal1";
            this.checkBoxMP.UseVisualStyleBackColor = true;
            // 
            // checkBoxTV
            // 
            this.checkBoxTV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxTV.AutoSize = true;
            this.checkBoxTV.Location = new System.Drawing.Point(96, 260);
            this.checkBoxTV.Name = "checkBoxTV";
            this.checkBoxTV.Size = new System.Drawing.Size(74, 17);
            this.checkBoxTV.TabIndex = 8;
            this.checkBoxTV.Text = "TV Server";
            this.checkBoxTV.UseVisualStyleBackColor = true;
            // 
            // CheckBoxDebugBackupSettings
            // 
            this.CheckBoxDebugBackupSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CheckBoxDebugBackupSettings.AutoSize = true;
            this.CheckBoxDebugBackupSettings.Location = new System.Drawing.Point(336, 283);
            this.CheckBoxDebugBackupSettings.Name = "CheckBoxDebugBackupSettings";
            this.CheckBoxDebugBackupSettings.Size = new System.Drawing.Size(100, 17);
            this.CheckBoxDebugBackupSettings.TabIndex = 7;
            this.CheckBoxDebugBackupSettings.Text = "Verbose Debug";
            this.CheckBoxDebugBackupSettings.UseVisualStyleBackColor = true;
            this.CheckBoxDebugBackupSettings.CheckedChanged += new System.EventHandler(this.DebugCheckBox_CheckedChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.buttonTvNone);
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.buttonTvAll);
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(442, 311);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "TV Server";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // buttonTvNone
            // 
            this.buttonTvNone.Location = new System.Drawing.Point(142, 263);
            this.buttonTvNone.Name = "buttonTvNone";
            this.buttonTvNone.Size = new System.Drawing.Size(57, 23);
            this.buttonTvNone.TabIndex = 10;
            this.buttonTvNone.Text = "None";
            this.buttonTvNone.UseVisualStyleBackColor = true;
            this.buttonTvNone.Click += new System.EventHandler(this.buttonTvNone_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.checkBoxAutoCorrectDataBase);
            this.groupBox4.Controls.Add(this.checkBoxTVallFolders);
            this.groupBox4.Controls.Add(this.checkBoxTVallPrograms);
            this.groupBox4.Controls.Add(this.checkBoxTVServer);
            this.groupBox4.Location = new System.Drawing.Point(23, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(191, 107);
            this.groupBox4.TabIndex = 9;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "File Settings for Export and Import";
            // 
            // checkBoxAutoCorrectDataBase
            // 
            this.checkBoxAutoCorrectDataBase.AutoSize = true;
            this.checkBoxAutoCorrectDataBase.Location = new System.Drawing.Point(6, 88);
            this.checkBoxAutoCorrectDataBase.Name = "checkBoxAutoCorrectDataBase";
            this.checkBoxAutoCorrectDataBase.Size = new System.Drawing.Size(131, 17);
            this.checkBoxAutoCorrectDataBase.TabIndex = 17;
            this.checkBoxAutoCorrectDataBase.Text = "Auto Repair Database";
            this.checkBoxAutoCorrectDataBase.UseVisualStyleBackColor = true;
            // 
            // checkBoxTVallFolders
            // 
            this.checkBoxTVallFolders.AutoSize = true;
            this.checkBoxTVallFolders.Location = new System.Drawing.Point(6, 65);
            this.checkBoxTVallFolders.Name = "checkBoxTVallFolders";
            this.checkBoxTVallFolders.Size = new System.Drawing.Size(136, 17);
            this.checkBoxTVallFolders.TabIndex = 16;
            this.checkBoxTVallFolders.Text = "TV Server User Folders";
            this.checkBoxTVallFolders.UseVisualStyleBackColor = true;
            // 
            // checkBoxTVallPrograms
            // 
            this.checkBoxTVallPrograms.AutoSize = true;
            this.checkBoxTVallPrograms.Location = new System.Drawing.Point(6, 42);
            this.checkBoxTVallPrograms.Name = "checkBoxTVallPrograms";
            this.checkBoxTVallPrograms.Size = new System.Drawing.Size(153, 17);
            this.checkBoxTVallPrograms.TabIndex = 15;
            this.checkBoxTVallPrograms.Text = "TV Server Program Folders";
            this.checkBoxTVallPrograms.UseVisualStyleBackColor = true;
            // 
            // checkBoxTVServer
            // 
            this.checkBoxTVServer.AutoSize = true;
            this.checkBoxTVServer.Location = new System.Drawing.Point(6, 19);
            this.checkBoxTVServer.Name = "checkBoxTVServer";
            this.checkBoxTVServer.Size = new System.Drawing.Size(94, 17);
            this.checkBoxTVServer.TabIndex = 13;
            this.checkBoxTVServer.Text = "TVsettings.xml";
            this.checkBoxTVServer.UseVisualStyleBackColor = true;
            // 
            // buttonTvAll
            // 
            this.buttonTvAll.Location = new System.Drawing.Point(40, 263);
            this.buttonTvAll.Name = "buttonTvAll";
            this.buttonTvAll.Size = new System.Drawing.Size(57, 23);
            this.buttonTvAll.TabIndex = 8;
            this.buttonTvAll.Text = "All";
            this.buttonTvAll.UseVisualStyleBackColor = true;
            this.buttonTvAll.Click += new System.EventHandler(this.buttonTvAll_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBoxtvrestart);
            this.groupBox3.Controls.Add(this.checkBoxEPG);
            this.groupBox3.Controls.Add(this.checkBoxRecordings);
            this.groupBox3.Controls.Add(this.checkBoxRadioGroups);
            this.groupBox3.Controls.Add(this.checkBoxChannelGroups);
            this.groupBox3.Controls.Add(this.checkBoxClickfinder);
            this.groupBox3.Controls.Add(this.checkBoxServer);
            this.groupBox3.Controls.Add(this.checkBoxAllSettings);
            this.groupBox3.Controls.Add(this.checkBoxChannels);
            this.groupBox3.Controls.Add(this.checkBoxSchedules);
            this.groupBox3.Controls.Add(this.checkBoxChannelCardMapping);
            this.groupBox3.Location = new System.Drawing.Point(239, 7);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(197, 269);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Filter Settings for Import";
            // 
            // checkBoxtvrestart
            // 
            this.checkBoxtvrestart.AutoSize = true;
            this.checkBoxtvrestart.Location = new System.Drawing.Point(5, 250);
            this.checkBoxtvrestart.Name = "checkBoxtvrestart";
            this.checkBoxtvrestart.Size = new System.Drawing.Size(146, 17);
            this.checkBoxtvrestart.TabIndex = 11;
            this.checkBoxtvrestart.Text = "Enable TV Config Restart";
            this.checkBoxtvrestart.UseVisualStyleBackColor = true;
            // 
            // checkBoxEPG
            // 
            this.checkBoxEPG.AutoSize = true;
            this.checkBoxEPG.Location = new System.Drawing.Point(5, 158);
            this.checkBoxEPG.Name = "checkBoxEPG";
            this.checkBoxEPG.Size = new System.Drawing.Size(48, 17);
            this.checkBoxEPG.TabIndex = 10;
            this.checkBoxEPG.Text = "EPG";
            this.checkBoxEPG.UseVisualStyleBackColor = true;
            // 
            // checkBoxRecordings
            // 
            this.checkBoxRecordings.AutoSize = true;
            this.checkBoxRecordings.Location = new System.Drawing.Point(5, 181);
            this.checkBoxRecordings.Name = "checkBoxRecordings";
            this.checkBoxRecordings.Size = new System.Drawing.Size(80, 17);
            this.checkBoxRecordings.TabIndex = 9;
            this.checkBoxRecordings.Text = "Recordings";
            this.checkBoxRecordings.UseVisualStyleBackColor = true;
            // 
            // checkBoxRadioGroups
            // 
            this.checkBoxRadioGroups.AutoSize = true;
            this.checkBoxRadioGroups.Location = new System.Drawing.Point(5, 112);
            this.checkBoxRadioGroups.Name = "checkBoxRadioGroups";
            this.checkBoxRadioGroups.Size = new System.Drawing.Size(91, 17);
            this.checkBoxRadioGroups.TabIndex = 8;
            this.checkBoxRadioGroups.Text = "Radio Groups";
            this.checkBoxRadioGroups.UseVisualStyleBackColor = true;
            // 
            // checkBoxChannelGroups
            // 
            this.checkBoxChannelGroups.AutoSize = true;
            this.checkBoxChannelGroups.Location = new System.Drawing.Point(6, 89);
            this.checkBoxChannelGroups.Name = "checkBoxChannelGroups";
            this.checkBoxChannelGroups.Size = new System.Drawing.Size(77, 17);
            this.checkBoxChannelGroups.TabIndex = 7;
            this.checkBoxChannelGroups.Text = "TV Groups";
            this.checkBoxChannelGroups.UseVisualStyleBackColor = true;
            // 
            // checkBoxClickfinder
            // 
            this.checkBoxClickfinder.AutoSize = true;
            this.checkBoxClickfinder.Location = new System.Drawing.Point(5, 227);
            this.checkBoxClickfinder.Name = "checkBoxClickfinder";
            this.checkBoxClickfinder.Size = new System.Drawing.Size(140, 17);
            this.checkBoxClickfinder.TabIndex = 6;
            this.checkBoxClickfinder.Text = "Tv Clickfinder Mappings";
            this.checkBoxClickfinder.UseVisualStyleBackColor = true;
            // 
            // checkBoxServer
            // 
            this.checkBoxServer.AutoSize = true;
            this.checkBoxServer.Location = new System.Drawing.Point(6, 19);
            this.checkBoxServer.Name = "checkBoxServer";
            this.checkBoxServer.Size = new System.Drawing.Size(113, 17);
            this.checkBoxServer.TabIndex = 1;
            this.checkBoxServer.Text = "Servers and Cards";
            this.checkBoxServer.UseVisualStyleBackColor = true;
            this.checkBoxServer.CheckedChanged += new System.EventHandler(this.checkBoxServer_CheckedChanged);
            // 
            // checkBoxAllSettings
            // 
            this.checkBoxAllSettings.AutoSize = true;
            this.checkBoxAllSettings.Location = new System.Drawing.Point(5, 204);
            this.checkBoxAllSettings.Name = "checkBoxAllSettings";
            this.checkBoxAllSettings.Size = new System.Drawing.Size(64, 17);
            this.checkBoxAllSettings.TabIndex = 5;
            this.checkBoxAllSettings.Text = "Settings";
            this.checkBoxAllSettings.UseVisualStyleBackColor = true;
            // 
            // checkBoxChannels
            // 
            this.checkBoxChannels.AutoSize = true;
            this.checkBoxChannels.Location = new System.Drawing.Point(6, 42);
            this.checkBoxChannels.Name = "checkBoxChannels";
            this.checkBoxChannels.Size = new System.Drawing.Size(70, 17);
            this.checkBoxChannels.TabIndex = 2;
            this.checkBoxChannels.Text = "Channels";
            this.checkBoxChannels.UseVisualStyleBackColor = true;
            // 
            // checkBoxSchedules
            // 
            this.checkBoxSchedules.AutoSize = true;
            this.checkBoxSchedules.Location = new System.Drawing.Point(5, 135);
            this.checkBoxSchedules.Name = "checkBoxSchedules";
            this.checkBoxSchedules.Size = new System.Drawing.Size(76, 17);
            this.checkBoxSchedules.TabIndex = 4;
            this.checkBoxSchedules.Text = "Schedules";
            this.checkBoxSchedules.UseVisualStyleBackColor = true;
            // 
            // checkBoxChannelCardMapping
            // 
            this.checkBoxChannelCardMapping.AutoSize = true;
            this.checkBoxChannelCardMapping.Location = new System.Drawing.Point(6, 66);
            this.checkBoxChannelCardMapping.Name = "checkBoxChannelCardMapping";
            this.checkBoxChannelCardMapping.Size = new System.Drawing.Size(139, 17);
            this.checkBoxChannelCardMapping.TabIndex = 3;
            this.checkBoxChannelCardMapping.Text = "Channel Card Mappings";
            this.checkBoxChannelCardMapping.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBoxDeleteRecordings);
            this.groupBox2.Controls.Add(this.checkBoxDeleteSchedules);
            this.groupBox2.Controls.Add(this.checkBoxDeleteRadioGroups);
            this.groupBox2.Controls.Add(this.checkBoxDeleteTVGroups);
            this.groupBox2.Controls.Add(this.checkBoxDeleteChannels);
            this.groupBox2.Location = new System.Drawing.Point(23, 119);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(191, 138);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Before Importing TV server Settings";
            // 
            // checkBoxDeleteRecordings
            // 
            this.checkBoxDeleteRecordings.AutoSize = true;
            this.checkBoxDeleteRecordings.Location = new System.Drawing.Point(6, 110);
            this.checkBoxDeleteRecordings.Name = "checkBoxDeleteRecordings";
            this.checkBoxDeleteRecordings.Size = new System.Drawing.Size(128, 17);
            this.checkBoxDeleteRecordings.TabIndex = 13;
            this.checkBoxDeleteRecordings.Text = "Delete All Recordings";
            this.checkBoxDeleteRecordings.UseVisualStyleBackColor = true;
            this.checkBoxDeleteRecordings.CheckedChanged += new System.EventHandler(this.checkBoxDeleteRecordings_CheckedChanged);
            // 
            // checkBoxDeleteSchedules
            // 
            this.checkBoxDeleteSchedules.AutoSize = true;
            this.checkBoxDeleteSchedules.Location = new System.Drawing.Point(6, 89);
            this.checkBoxDeleteSchedules.Name = "checkBoxDeleteSchedules";
            this.checkBoxDeleteSchedules.Size = new System.Drawing.Size(170, 17);
            this.checkBoxDeleteSchedules.TabIndex = 12;
            this.checkBoxDeleteSchedules.Text = "Delete All Schedules and EPG";
            this.checkBoxDeleteSchedules.UseVisualStyleBackColor = true;
            this.checkBoxDeleteSchedules.CheckedChanged += new System.EventHandler(this.checkBoxDeleteSchedules_CheckedChanged);
            // 
            // checkBoxDeleteRadioGroups
            // 
            this.checkBoxDeleteRadioGroups.AutoSize = true;
            this.checkBoxDeleteRadioGroups.Location = new System.Drawing.Point(6, 66);
            this.checkBoxDeleteRadioGroups.Name = "checkBoxDeleteRadioGroups";
            this.checkBoxDeleteRadioGroups.Size = new System.Drawing.Size(139, 17);
            this.checkBoxDeleteRadioGroups.TabIndex = 11;
            this.checkBoxDeleteRadioGroups.Text = "Delete All Radio Groups";
            this.checkBoxDeleteRadioGroups.UseVisualStyleBackColor = true;
            this.checkBoxDeleteRadioGroups.CheckedChanged += new System.EventHandler(this.checkBoxDeleteRadioGroups_CheckedChanged);
            // 
            // checkBoxDeleteTVGroups
            // 
            this.checkBoxDeleteTVGroups.AutoSize = true;
            this.checkBoxDeleteTVGroups.Location = new System.Drawing.Point(6, 43);
            this.checkBoxDeleteTVGroups.Name = "checkBoxDeleteTVGroups";
            this.checkBoxDeleteTVGroups.Size = new System.Drawing.Size(125, 17);
            this.checkBoxDeleteTVGroups.TabIndex = 10;
            this.checkBoxDeleteTVGroups.Text = "Delete All TV Groups";
            this.checkBoxDeleteTVGroups.UseVisualStyleBackColor = true;
            this.checkBoxDeleteTVGroups.CheckedChanged += new System.EventHandler(this.checkBoxDeleteTVGroups_CheckedChanged);
            // 
            // checkBoxDeleteChannels
            // 
            this.checkBoxDeleteChannels.AutoSize = true;
            this.checkBoxDeleteChannels.Location = new System.Drawing.Point(6, 20);
            this.checkBoxDeleteChannels.Name = "checkBoxDeleteChannels";
            this.checkBoxDeleteChannels.Size = new System.Drawing.Size(118, 17);
            this.checkBoxDeleteChannels.TabIndex = 9;
            this.checkBoxDeleteChannels.Text = "Delete All Channels";
            this.checkBoxDeleteChannels.UseVisualStyleBackColor = true;
            this.checkBoxDeleteChannels.CheckedChanged += new System.EventHandler(this.checkBoxDeleteChannels_CheckedChanged);
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
            this.tabPage3.Size = new System.Drawing.Size(442, 311);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Media Portal1";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // buttonMpNone
            // 
            this.buttonMpNone.Location = new System.Drawing.Point(366, 133);
            this.buttonMpNone.Name = "buttonMpNone";
            this.buttonMpNone.Size = new System.Drawing.Size(57, 23);
            this.buttonMpNone.TabIndex = 10;
            this.buttonMpNone.Text = "None";
            this.buttonMpNone.UseVisualStyleBackColor = true;
            this.buttonMpNone.Click += new System.EventHandler(this.buttonMpNone_Click);
            // 
            // buttonMpAll
            // 
            this.buttonMpAll.Location = new System.Drawing.Point(242, 133);
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
            this.label3.Location = new System.Drawing.Point(20, 280);
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
            this.groupBox6.Location = new System.Drawing.Point(242, 22);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(190, 95);
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
            this.checkBoxMPAllProgram.TabIndex = 3;
            this.checkBoxMPAllProgram.Text = "All MediaPortal Program Folders";
            this.checkBoxMPAllProgram.UseVisualStyleBackColor = true;
            // 
            // checkBoxMPAllFolders
            // 
            this.checkBoxMPAllFolders.AutoSize = true;
            this.checkBoxMPAllFolders.Location = new System.Drawing.Point(6, 46);
            this.checkBoxMPAllFolders.Name = "checkBoxMPAllFolders";
            this.checkBoxMPAllFolders.Size = new System.Drawing.Size(158, 17);
            this.checkBoxMPAllFolders.TabIndex = 2;
            this.checkBoxMPAllFolders.Text = "All MediaPortal User Folders";
            this.checkBoxMPAllFolders.UseVisualStyleBackColor = true;
            // 
            // checkBoxMPDeleteCache
            // 
            this.checkBoxMPDeleteCache.AutoSize = true;
            this.checkBoxMPDeleteCache.Location = new System.Drawing.Point(6, 23);
            this.checkBoxMPDeleteCache.Name = "checkBoxMPDeleteCache";
            this.checkBoxMPDeleteCache.Size = new System.Drawing.Size(91, 17);
            this.checkBoxMPDeleteCache.TabIndex = 1;
            this.checkBoxMPDeleteCache.Text = "Delete Cache";
            this.checkBoxMPDeleteCache.UseVisualStyleBackColor = true;
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
            this.groupBox5.Size = new System.Drawing.Size(195, 242);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "File Settings for Export and Import";
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
            this.checkBoxMPSkins.Text = "Skins and Languages";
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
            this.checkBoxMPProgramXml.Location = new System.Drawing.Point(28, 23);
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
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.buttonMP2None);
            this.tabPage7.Controls.Add(this.buttonMP2All);
            this.tabPage7.Controls.Add(this.groupBox10);
            this.tabPage7.Controls.Add(this.groupBox9);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Size = new System.Drawing.Size(442, 311);
            this.tabPage7.TabIndex = 6;
            this.tabPage7.Text = "MediaPortal2";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // buttonMP2None
            // 
            this.buttonMP2None.Location = new System.Drawing.Point(251, 260);
            this.buttonMP2None.Name = "buttonMP2None";
            this.buttonMP2None.Size = new System.Drawing.Size(57, 23);
            this.buttonMP2None.TabIndex = 12;
            this.buttonMP2None.Text = "None";
            this.buttonMP2None.UseVisualStyleBackColor = true;
            this.buttonMP2None.Click += new System.EventHandler(this.buttonMP2None_Click);
            // 
            // buttonMP2All
            // 
            this.buttonMP2All.Location = new System.Drawing.Point(127, 260);
            this.buttonMP2All.Name = "buttonMP2All";
            this.buttonMP2All.Size = new System.Drawing.Size(57, 23);
            this.buttonMP2All.TabIndex = 11;
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
            this.groupBox10.Location = new System.Drawing.Point(229, 25);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(190, 215);
            this.groupBox10.TabIndex = 4;
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
            this.groupBox9.Location = new System.Drawing.Point(20, 25);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(190, 215);
            this.groupBox9.TabIndex = 3;
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
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.label2);
            this.tabPage4.Controls.Add(this.buttonundo);
            this.tabPage4.Controls.Add(this.label6);
            this.tabPage4.Controls.Add(this.label5);
            this.tabPage4.Controls.Add(this.groupBox8);
            this.tabPage4.Controls.Add(this.buttonCheck);
            this.tabPage4.Controls.Add(this.buttonprocess);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(442, 311);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Duplicate Channels";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 254);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(349, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Channels with identical display names can be renamed to a unique name";
            // 
            // buttonundo
            // 
            this.buttonundo.Location = new System.Drawing.Point(304, 207);
            this.buttonundo.Name = "buttonundo";
            this.buttonundo.Size = new System.Drawing.Size(94, 26);
            this.buttonundo.TabIndex = 16;
            this.buttonundo.Text = "Undo Rename";
            this.buttonundo.UseVisualStyleBackColor = true;
            this.buttonundo.Click += new System.EventHandler(this.buttonundo_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 35);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(188, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Processing Duplicate Channel Names:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 267);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(326, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Do not change default settings unless you are a Media Portal expert";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.checkBoxduplicateautoprocess);
            this.groupBox8.Controls.Add(this.checkBoxduplicateinteractive);
            this.groupBox8.Location = new System.Drawing.Point(23, 74);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(229, 73);
            this.groupBox8.TabIndex = 13;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Options";
            // 
            // checkBoxduplicateautoprocess
            // 
            this.checkBoxduplicateautoprocess.AutoSize = true;
            this.checkBoxduplicateautoprocess.Location = new System.Drawing.Point(17, 42);
            this.checkBoxduplicateautoprocess.Name = "checkBoxduplicateautoprocess";
            this.checkBoxduplicateautoprocess.Size = new System.Drawing.Size(199, 17);
            this.checkBoxduplicateautoprocess.TabIndex = 6;
            this.checkBoxduplicateautoprocess.Text = "Automated Processing During Export";
            this.checkBoxduplicateautoprocess.UseVisualStyleBackColor = true;
            // 
            // checkBoxduplicateinteractive
            // 
            this.checkBoxduplicateinteractive.AutoSize = true;
            this.checkBoxduplicateinteractive.Location = new System.Drawing.Point(17, 19);
            this.checkBoxduplicateinteractive.Name = "checkBoxduplicateinteractive";
            this.checkBoxduplicateinteractive.Size = new System.Drawing.Size(162, 17);
            this.checkBoxduplicateinteractive.TabIndex = 5;
            this.checkBoxduplicateinteractive.Text = "Interactive User Confirmation";
            this.checkBoxduplicateinteractive.UseVisualStyleBackColor = true;
            // 
            // buttonCheck
            // 
            this.buttonCheck.Location = new System.Drawing.Point(304, 74);
            this.buttonCheck.Name = "buttonCheck";
            this.buttonCheck.Size = new System.Drawing.Size(94, 26);
            this.buttonCheck.TabIndex = 12;
            this.buttonCheck.Text = "Check";
            this.buttonCheck.UseVisualStyleBackColor = true;
            this.buttonCheck.Click += new System.EventHandler(this.buttonCheck_Click);
            // 
            // buttonprocess
            // 
            this.buttonprocess.Location = new System.Drawing.Point(304, 142);
            this.buttonprocess.Name = "buttonprocess";
            this.buttonprocess.Size = new System.Drawing.Size(94, 26);
            this.buttonprocess.TabIndex = 11;
            this.buttonprocess.Text = "Process";
            this.buttonprocess.UseVisualStyleBackColor = true;
            this.buttonprocess.Click += new System.EventHandler(this.buttonprocess_Click);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.buttonActiveAll);
            this.tabPage5.Controls.Add(this.buttonActiveNone);
            this.tabPage5.Controls.Add(this.buttonSelectFolder);
            this.tabPage5.Controls.Add(this.buttonDelete);
            this.tabPage5.Controls.Add(this.label9);
            this.tabPage5.Controls.Add(this.label8);
            this.tabPage5.Controls.Add(this.label7);
            this.tabPage5.Controls.Add(this.dataGridView1);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(442, 311);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Extra Folders";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // buttonActiveAll
            // 
            this.buttonActiveAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonActiveAll.Location = new System.Drawing.Point(23, 216);
            this.buttonActiveAll.Name = "buttonActiveAll";
            this.buttonActiveAll.Size = new System.Drawing.Size(82, 26);
            this.buttonActiveAll.TabIndex = 69;
            this.buttonActiveAll.Text = "All Active";
            this.buttonActiveAll.UseVisualStyleBackColor = true;
            this.buttonActiveAll.Click += new System.EventHandler(this.buttonActiveAll_Click);
            // 
            // buttonActiveNone
            // 
            this.buttonActiveNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonActiveNone.Location = new System.Drawing.Point(137, 216);
            this.buttonActiveNone.Name = "buttonActiveNone";
            this.buttonActiveNone.Size = new System.Drawing.Size(82, 26);
            this.buttonActiveNone.TabIndex = 68;
            this.buttonActiveNone.Text = "None Active";
            this.buttonActiveNone.UseVisualStyleBackColor = true;
            this.buttonActiveNone.Click += new System.EventHandler(this.buttonActiveNone_Click);
            // 
            // buttonSelectFolder
            // 
            this.buttonSelectFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSelectFolder.Location = new System.Drawing.Point(248, 216);
            this.buttonSelectFolder.Name = "buttonSelectFolder";
            this.buttonSelectFolder.Size = new System.Drawing.Size(82, 26);
            this.buttonSelectFolder.TabIndex = 67;
            this.buttonSelectFolder.Text = "Select Dialog";
            this.buttonSelectFolder.UseVisualStyleBackColor = true;
            this.buttonSelectFolder.Click += new System.EventHandler(this.buttonSelectFolder_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonDelete.Location = new System.Drawing.Point(358, 216);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(82, 26);
            this.buttonDelete.TabIndex = 66;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(20, 270);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(293, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "Restart will start the specified process after a backup/restore";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(20, 257);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(344, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "Kill process will terminate the specified process before a backup/restore";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(19, 19);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(263, 20);
            this.label7.TabIndex = 2;
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
            this.dataGridView1.Location = new System.Drawing.Point(23, 58);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 25;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.Size = new System.Drawing.Size(417, 152);
            this.dataGridView1.TabIndex = 1;
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
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.buttonDetect);
            this.tabPage6.Controls.Add(this.tabControl2);
            this.tabPage6.Controls.Add(this.buttonSave);
            this.tabPage6.Controls.Add(this.buttonCancel);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(442, 311);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "Paths";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // buttonDetect
            // 
            this.buttonDetect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDetect.Location = new System.Drawing.Point(116, 265);
            this.buttonDetect.Name = "buttonDetect";
            this.buttonDetect.Size = new System.Drawing.Size(55, 26);
            this.buttonDetect.TabIndex = 21;
            this.buttonDetect.Text = "Detect";
            this.buttonDetect.UseVisualStyleBackColor = true;
            this.buttonDetect.Click += new System.EventHandler(this.buttonDetect_Click);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage8);
            this.tabControl2.Controls.Add(this.tabPage9);
            this.tabControl2.Location = new System.Drawing.Point(21, 12);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(396, 247);
            this.tabControl2.TabIndex = 20;
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
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.Location = new System.Drawing.Point(190, 265);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(55, 26);
            this.buttonSave.TabIndex = 0;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(266, 265);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(55, 26);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // tabPage10
            // 
            this.tabPage10.Controls.Add(this.groupBox11);
            this.tabPage10.Location = new System.Drawing.Point(4, 22);
            this.tabPage10.Name = "tabPage10";
            this.tabPage10.Size = new System.Drawing.Size(442, 311);
            this.tabPage10.TabIndex = 7;
            this.tabPage10.Text = "Scheduler";
            this.tabPage10.UseVisualStyleBackColor = true;
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.label27);
            this.groupBox11.Controls.Add(this.textBoxAutomatedExportFolderName);
            this.groupBox11.Controls.Add(this.label29);
            this.groupBox11.Controls.Add(this.checkBoxAny);
            this.groupBox11.Controls.Add(this.labelLastCheckDate);
            this.groupBox11.Controls.Add(this.label28);
            this.groupBox11.Controls.Add(this.label26);
            this.groupBox11.Controls.Add(this.comboBoxKeepNumber);
            this.groupBox11.Controls.Add(this.label25);
            this.groupBox11.Controls.Add(this.checkBoxEnableScheduler);
            this.groupBox11.Controls.Add(this.label24);
            this.groupBox11.Controls.Add(this.label23);
            this.groupBox11.Controls.Add(this.label16);
            this.groupBox11.Controls.Add(this.label18);
            this.groupBox11.Controls.Add(this.checkBoxSun);
            this.groupBox11.Controls.Add(this.checkBoxSat);
            this.groupBox11.Controls.Add(this.checkBoxFri);
            this.groupBox11.Controls.Add(this.checkBoxThur);
            this.groupBox11.Controls.Add(this.checkBoxWed);
            this.groupBox11.Controls.Add(this.checkBoxTue);
            this.groupBox11.Controls.Add(this.label19);
            this.groupBox11.Controls.Add(this.checkBoxMon);
            this.groupBox11.Controls.Add(this.label20);
            this.groupBox11.Controls.Add(this.comboBoxminutes);
            this.groupBox11.Controls.Add(this.comboBoxhours);
            this.groupBox11.Controls.Add(this.comboBoxdays);
            this.groupBox11.Controls.Add(this.labelCheckingdate);
            this.groupBox11.Controls.Add(this.label21);
            this.groupBox11.Controls.Add(this.label22);
            this.groupBox11.Location = new System.Drawing.Point(17, 17);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(412, 282);
            this.groupBox11.TabIndex = 61;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Schedule Automated Backups";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(160, 7);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(176, 13);
            this.label27.TabIndex = 181;
            this.label27.Text = "Foldername for Automated Backups";
            // 
            // textBoxAutomatedExportFolderName
            // 
            this.textBoxAutomatedExportFolderName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxAutomatedExportFolderName.Location = new System.Drawing.Point(163, 23);
            this.textBoxAutomatedExportFolderName.Name = "textBoxAutomatedExportFolderName";
            this.textBoxAutomatedExportFolderName.Size = new System.Drawing.Size(243, 20);
            this.textBoxAutomatedExportFolderName.TabIndex = 180;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(240, 196);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(130, 13);
            this.label29.TabIndex = 179;
            this.label29.Text = "Computer must be running";
            // 
            // checkBoxAny
            // 
            this.checkBoxAny.AutoSize = true;
            this.checkBoxAny.Location = new System.Drawing.Point(121, 215);
            this.checkBoxAny.Name = "checkBoxAny";
            this.checkBoxAny.Size = new System.Drawing.Size(44, 17);
            this.checkBoxAny.TabIndex = 178;
            this.checkBoxAny.Text = "Any";
            this.checkBoxAny.UseVisualStyleBackColor = true;
            this.checkBoxAny.CheckedChanged += new System.EventHandler(this.checkBoxAny_CheckedChanged);
            // 
            // labelLastCheckDate
            // 
            this.labelLastCheckDate.AutoSize = true;
            this.labelLastCheckDate.Location = new System.Drawing.Point(252, 239);
            this.labelLastCheckDate.Name = "labelLastCheckDate";
            this.labelLastCheckDate.Size = new System.Drawing.Size(61, 13);
            this.labelLastCheckDate.TabIndex = 177;
            this.labelLastCheckDate.Text = "2009.05.11";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(92, 239);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(147, 13);
            this.label28.TabIndex = 176;
            this.label28.Text = "Last TV Server Backup Date:";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(223, 95);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(160, 13);
            this.label26.TabIndex = 175;
            this.label26.Text = "Backups and Delete Old Folders";
            // 
            // comboBoxKeepNumber
            // 
            this.comboBoxKeepNumber.FormattingEnabled = true;
            this.comboBoxKeepNumber.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "All"});
            this.comboBoxKeepNumber.Location = new System.Drawing.Point(163, 87);
            this.comboBoxKeepNumber.Name = "comboBoxKeepNumber";
            this.comboBoxKeepNumber.Size = new System.Drawing.Size(53, 21);
            this.comboBoxKeepNumber.TabIndex = 174;
            this.comboBoxKeepNumber.Text = "07";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(92, 95);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(56, 13);
            this.label25.TabIndex = 173;
            this.label25.Text = "Keep Only";
            // 
            // checkBoxEnableScheduler
            // 
            this.checkBoxEnableScheduler.AutoSize = true;
            this.checkBoxEnableScheduler.Location = new System.Drawing.Point(20, 19);
            this.checkBoxEnableScheduler.Name = "checkBoxEnableScheduler";
            this.checkBoxEnableScheduler.Size = new System.Drawing.Size(116, 17);
            this.checkBoxEnableScheduler.TabIndex = 172;
            this.checkBoxEnableScheduler.Text = "Activate Scheduler";
            this.checkBoxEnableScheduler.UseVisualStyleBackColor = true;
            this.checkBoxEnableScheduler.CheckedChanged += new System.EventHandler(this.checkBoxEnableScheduler_CheckedChanged);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(17, 118);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(144, 13);
            this.label24.TabIndex = 171;
            this.label24.Text = "Use Specific Days and Time:";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(17, 68);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(131, 13);
            this.label23.TabIndex = 170;
            this.label23.Text = "Schedule An Export Every";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(316, 153);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(44, 13);
            this.label16.TabIndex = 169;
            this.label16.Text = "Minutes";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(272, 153);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(35, 13);
            this.label18.TabIndex = 168;
            this.label18.Text = "Hours";
            // 
            // checkBoxSun
            // 
            this.checkBoxSun.AutoSize = true;
            this.checkBoxSun.Location = new System.Drawing.Point(20, 215);
            this.checkBoxSun.Name = "checkBoxSun";
            this.checkBoxSun.Size = new System.Drawing.Size(62, 17);
            this.checkBoxSun.TabIndex = 166;
            this.checkBoxSun.Text = "Sunday";
            this.checkBoxSun.UseVisualStyleBackColor = true;
            this.checkBoxSun.CheckedChanged += new System.EventHandler(this.checkBoxSun_CheckedChanged);
            // 
            // checkBoxSat
            // 
            this.checkBoxSat.AutoSize = true;
            this.checkBoxSat.Location = new System.Drawing.Point(121, 192);
            this.checkBoxSat.Name = "checkBoxSat";
            this.checkBoxSat.Size = new System.Drawing.Size(68, 17);
            this.checkBoxSat.TabIndex = 165;
            this.checkBoxSat.Text = "Saturday";
            this.checkBoxSat.UseVisualStyleBackColor = true;
            this.checkBoxSat.CheckedChanged += new System.EventHandler(this.checkBoxSat_CheckedChanged);
            // 
            // checkBoxFri
            // 
            this.checkBoxFri.AutoSize = true;
            this.checkBoxFri.Location = new System.Drawing.Point(20, 192);
            this.checkBoxFri.Name = "checkBoxFri";
            this.checkBoxFri.Size = new System.Drawing.Size(54, 17);
            this.checkBoxFri.TabIndex = 164;
            this.checkBoxFri.Text = "Friday";
            this.checkBoxFri.UseVisualStyleBackColor = true;
            this.checkBoxFri.CheckedChanged += new System.EventHandler(this.checkBoxFri_CheckedChanged);
            // 
            // checkBoxThur
            // 
            this.checkBoxThur.AutoSize = true;
            this.checkBoxThur.Location = new System.Drawing.Point(122, 169);
            this.checkBoxThur.Name = "checkBoxThur";
            this.checkBoxThur.Size = new System.Drawing.Size(70, 17);
            this.checkBoxThur.TabIndex = 163;
            this.checkBoxThur.Text = "Thursday";
            this.checkBoxThur.UseVisualStyleBackColor = true;
            this.checkBoxThur.CheckedChanged += new System.EventHandler(this.checkBoxThur_CheckedChanged);
            // 
            // checkBoxWed
            // 
            this.checkBoxWed.AutoSize = true;
            this.checkBoxWed.Location = new System.Drawing.Point(20, 166);
            this.checkBoxWed.Name = "checkBoxWed";
            this.checkBoxWed.Size = new System.Drawing.Size(83, 17);
            this.checkBoxWed.TabIndex = 162;
            this.checkBoxWed.Text = "Wednesday";
            this.checkBoxWed.UseVisualStyleBackColor = true;
            this.checkBoxWed.CheckedChanged += new System.EventHandler(this.checkBoxWed_CheckedChanged);
            // 
            // checkBoxTue
            // 
            this.checkBoxTue.AutoSize = true;
            this.checkBoxTue.Location = new System.Drawing.Point(122, 143);
            this.checkBoxTue.Name = "checkBoxTue";
            this.checkBoxTue.Size = new System.Drawing.Size(67, 17);
            this.checkBoxTue.TabIndex = 161;
            this.checkBoxTue.Text = "Tuesday";
            this.checkBoxTue.UseVisualStyleBackColor = true;
            this.checkBoxTue.CheckedChanged += new System.EventHandler(this.checkBoxTue_CheckedChanged);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(222, 68);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(31, 13);
            this.label19.TabIndex = 160;
            this.label19.Text = "Days";
            // 
            // checkBoxMon
            // 
            this.checkBoxMon.AutoSize = true;
            this.checkBoxMon.Location = new System.Drawing.Point(20, 143);
            this.checkBoxMon.Name = "checkBoxMon";
            this.checkBoxMon.Size = new System.Drawing.Size(64, 17);
            this.checkBoxMon.TabIndex = 159;
            this.checkBoxMon.Text = "Monday";
            this.checkBoxMon.UseVisualStyleBackColor = true;
            this.checkBoxMon.CheckedChanged += new System.EventHandler(this.checkBoxMon_CheckedChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(144, 140);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(10, 13);
            this.label20.TabIndex = 61;
            this.label20.Text = ":";
            // 
            // comboBoxminutes
            // 
            this.comboBoxminutes.FormattingEnabled = true;
            this.comboBoxminutes.Items.AddRange(new object[] {
            "00",
            "05",
            "10",
            "15",
            "20",
            "25",
            "30",
            "35",
            "40",
            "45",
            "50",
            "55"});
            this.comboBoxminutes.Location = new System.Drawing.Point(319, 169);
            this.comboBoxminutes.MaxDropDownItems = 100;
            this.comboBoxminutes.MaxLength = 2;
            this.comboBoxminutes.Name = "comboBoxminutes";
            this.comboBoxminutes.Size = new System.Drawing.Size(37, 21);
            this.comboBoxminutes.TabIndex = 60;
            this.comboBoxminutes.Text = "00";
            this.comboBoxminutes.SelectedIndexChanged += new System.EventHandler(this.comboBoxminutes_SelectedIndexChanged);
            // 
            // comboBoxhours
            // 
            this.comboBoxhours.FormattingEnabled = true;
            this.comboBoxhours.Items.AddRange(new object[] {
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23"});
            this.comboBoxhours.Location = new System.Drawing.Point(274, 169);
            this.comboBoxhours.MaxDropDownItems = 100;
            this.comboBoxhours.MaxLength = 2;
            this.comboBoxhours.Name = "comboBoxhours";
            this.comboBoxhours.Size = new System.Drawing.Size(37, 21);
            this.comboBoxhours.Sorted = true;
            this.comboBoxhours.TabIndex = 59;
            this.comboBoxhours.Text = "00";
            this.comboBoxhours.SelectedIndexChanged += new System.EventHandler(this.comboBoxhours_SelectedIndexChanged);
            // 
            // comboBoxdays
            // 
            this.comboBoxdays.FormattingEnabled = true;
            this.comboBoxdays.Items.AddRange(new object[] {
            "1",
            "7",
            "14",
            "21",
            "28",
            "30",
            "31",
            "60",
            "90",
            "120",
            "150",
            "180"});
            this.comboBoxdays.Location = new System.Drawing.Point(163, 60);
            this.comboBoxdays.Name = "comboBoxdays";
            this.comboBoxdays.Size = new System.Drawing.Size(53, 21);
            this.comboBoxdays.TabIndex = 58;
            this.comboBoxdays.Text = "07";
            this.comboBoxdays.SelectedIndexChanged += new System.EventHandler(this.comboBoxdays_SelectedIndexChanged);
            // 
            // labelCheckingdate
            // 
            this.labelCheckingdate.AutoSize = true;
            this.labelCheckingdate.Location = new System.Drawing.Point(251, 261);
            this.labelCheckingdate.Name = "labelCheckingdate";
            this.labelCheckingdate.Size = new System.Drawing.Size(61, 13);
            this.labelCheckingdate.TabIndex = 53;
            this.labelCheckingdate.Text = "2009.05.11";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(92, 261);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(149, 13);
            this.label21.TabIndex = 52;
            this.label21.Text = "Next TV Server Backup Date:";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(240, 172);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(16, 13);
            this.label22.TabIndex = 51;
            this.label22.Text = "at";
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(16, 5);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(449, 10);
            this.progressBar.TabIndex = 9;
            // 
            // BackupSettingsSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox1);
            this.Name = "BackupSettingsSetup";
            this.Size = new System.Drawing.Size(485, 440);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.tabPage7.ResumeLayout(false);
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabPage6.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage8.ResumeLayout(false);
            this.tabPage8.PerformLayout();
            this.tabPage9.ResumeLayout(false);
            this.tabPage9.PerformLayout();
            this.tabPage10.ResumeLayout(false);
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button importbutton;
        private System.Windows.Forms.TextBox filenametextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button exportbutton;
        private System.Windows.Forms.Button selectfilenamebutton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button clearbutton;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.CheckBox CheckBoxDebugBackupSettings;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBoxAllSettings;
        private System.Windows.Forms.CheckBox checkBoxSchedules;
        private System.Windows.Forms.CheckBox checkBoxChannelCardMapping;
        private System.Windows.Forms.CheckBox checkBoxChannels;
        private System.Windows.Forms.CheckBox checkBoxServer;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBoxClickfinder;
        private System.Windows.Forms.CheckBox checkBoxRadioGroups;
        private System.Windows.Forms.CheckBox checkBoxChannelGroups;
        private System.Windows.Forms.Button buttonTvAll;
        private System.Windows.Forms.CheckBox checkBoxDeleteSchedules;
        private System.Windows.Forms.CheckBox checkBoxDeleteRadioGroups;
        private System.Windows.Forms.CheckBox checkBoxDeleteTVGroups;
        private System.Windows.Forms.CheckBox checkBoxDeleteChannels;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox checkBoxTVServer;
        private System.Windows.Forms.CheckBox checkBoxTVallPrograms;
        private System.Windows.Forms.CheckBox checkBoxMP;
        private System.Windows.Forms.CheckBox checkBoxTV;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox checkBoxMPDeleteCache;
        private System.Windows.Forms.CheckBox checkBoxMPUserXML;
        private System.Windows.Forms.CheckBox checkBoxMPxmltv;
        private System.Windows.Forms.CheckBox checkBoxMPThumbs;
        private System.Windows.Forms.CheckBox checkBoxMPInputDevice;
        private System.Windows.Forms.CheckBox checkBoxMPDatabase;
        private System.Windows.Forms.CheckBox checkBoxMPSkins;
        private System.Windows.Forms.CheckBox checkBoxMPPlugins;
        private System.Windows.Forms.CheckBox checkBoxMPProgramXml;
        private System.Windows.Forms.Button buttonTvNone;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button buttonMpNone;
        private System.Windows.Forms.Button buttonMpAll;
        private System.Windows.Forms.CheckBox checkBoxMPAllFolders;
        private System.Windows.Forms.Button buttonDefault;
        private System.Windows.Forms.CheckBox checkBoxTVallFolders;
        private System.Windows.Forms.CheckBox checkBoxRecordings;
        private System.Windows.Forms.CheckBox checkBoxDeleteRecordings;
        private System.Windows.Forms.CheckBox checkBoxEPG;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Button buttonCheck;
        private System.Windows.Forms.Button buttonprocess;
        private System.Windows.Forms.CheckBox checkBoxduplicateinteractive;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonundo;
        private System.Windows.Forms.CheckBox checkBoxduplicateautoprocess;
        private System.Windows.Forms.CheckBox checkBoxMPMusicPlayer;
        private System.Windows.Forms.Button buttonhelp;
        private System.Windows.Forms.CheckBox checkBoxMPAllProgram;
        private CheckBox checkBoxtvrestart;
        private LinkLabel linkLabel1;
        private ProgressBar progressBar;
        private ListView listView1;
        private ColumnHeader columnHeader1;
        private TabPage tabPage5;
        private Label label7;
        private DataGridView dataGridView1;
        private Label label8;
        private Label label9;
        private Button buttonDelete;
        private Button buttonSelectFolder;
        private Button buttonActiveNone;
        private Button buttonActiveAll;
        private DataGridViewCheckBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column7;
        private DataGridViewCheckBoxColumn Column3;
        private DataGridViewTextBoxColumn Column8;
        private DataGridViewTextBoxColumn Column9;
        private Button buttonCreateAutoFolder;
        private GroupBox groupBox7;
        private RadioButton radioButtonExpert;
        private RadioButton radioButtonEasy;
        private CheckBox checkBoxMP2C;
        private TabPage tabPage7;
        private TabPage tabPage6;
        private GroupBox groupBox10;
        private CheckBox checkBoxMP2AllClientFolders;
        private CheckBox checkBoxMP2Config;
        private CheckBox checkBoxMP2Plugins;
        private GroupBox groupBox9;
        private CheckBox checkBoxSV2AllServerFolders;
        private CheckBox checkBoxSV2Database;
        private CheckBox checkBoxSV2Plugins;
        private Button buttonSave;
        private Button buttonCancel;
        private CheckBox checkBoxMP2AllClientFiles;
        private CheckBox checkBoxSV2AllServerFiles;
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
        private Button buttonDetect;
        private CheckBox checkBoxMP2S;
        private CheckBox checkBoxMP2AllClientProgramFolders;
        private CheckBox checkBoxSV2Defaults;
        private CheckBox checkBoxSV2AllServerProgramFolders;
        private CheckBox checkBoxSV2Config;
        private CheckBox checkBoxMP2Defaults;
        private Button buttonMP2None;
        private Button buttonMP2All;
        private CheckBox checkBoxAutoCorrectDataBase;
        private Label label2;
        private TabPage tabPage10;
        private GroupBox groupBox11;
        private Label label16;
        private Label label18;
        private CheckBox checkBoxSun;
        private CheckBox checkBoxSat;
        private CheckBox checkBoxFri;
        private CheckBox checkBoxThur;
        private CheckBox checkBoxWed;
        private CheckBox checkBoxTue;
        private Label label19;
        private CheckBox checkBoxMon;
        private Label label20;
        private ComboBox comboBoxminutes;
        private ComboBox comboBoxhours;
        private ComboBox comboBoxdays;
        private Label labelCheckingdate;
        private Label label21;
        private Label label22;
        private Label label26;
        private ComboBox comboBoxKeepNumber;
        private Label label25;
        private CheckBox checkBoxEnableScheduler;
        private Label label24;
        private Label label23;
        private Label labelLastCheckDate;
        private Label label28;
        private Label label29;
        private CheckBox checkBoxAny;
        private Label label27;
        private TextBox textBoxAutomatedExportFolderName;
        private CheckBox checkBoxUseAutoDate;
    }
}