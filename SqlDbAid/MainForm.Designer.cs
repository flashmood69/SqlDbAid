namespace SqlDbAid
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.datagridMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.viewCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scriptSelectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scriptInsertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scriptUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.countDistinctToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.missingIndexesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.indexStatisticsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.limitedFragmentAnalysisToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.sampledFragmentAnalysisToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.detailedFragmentAnalysisToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmbDatabase = new System.Windows.Forms.ComboBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.lblServer = new System.Windows.Forms.Label();
            this.lblDatabase = new System.Windows.Forms.Label();
            this.exportFolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.lblObjType = new System.Windows.Forms.Label();
            this.cmbAuthMode = new System.Windows.Forms.ComboBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.mainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.lblRow = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.exportFileBrowser = new System.Windows.Forms.SaveFileDialog();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newQueryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataSearchQueryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serverMonitorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.processesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deadlocksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.locksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.topQueriesTimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.topQueriesReadsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.cpuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jobsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cacheToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.plansCacheToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.databasesCacheToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.objectsCacheToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.compareDatabasesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportSelectedObjectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compareExportedObjectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tableMBytesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableStatisticsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.columnInconsistencyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.indexesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.missingForeignKeyIndexesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.missingIndexesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.existingIndexesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statisticsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.limitedFragmentAnalysisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sampledFragmentAnalysisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.detailedFragmentAnalysisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.serverInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configurationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rolesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.databaseFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.databasePermissionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.databaseRolesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblContains = new System.Windows.Forms.Label();
            this.txtContains = new System.Windows.Forms.TextBox();
            this.cmbServer = new System.Windows.Forms.ComboBox();
            this.tltMain = new System.Windows.Forms.ToolTip(this.components);
            this.chlObjType = new System.Windows.Forms.CheckedListBox();
            this.dgvCode = new System.Windows.Forms.DataGridView();
            this.selected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.obj_type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.obj_schema = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.obj_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modify_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ord_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkRegex = new System.Windows.Forms.CheckBox();
            this.lblAuthentication = new System.Windows.Forms.Label();
            this.datagridMenu.SuspendLayout();
            this.mainStatusStrip.SuspendLayout();
            this.mainMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCode)).BeginInit();
            this.SuspendLayout();
            // 
            // datagridMenu
            // 
            this.datagridMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewCodeToolStripMenuItem,
            this.scriptSelectToolStripMenuItem,
            this.scriptInsertToolStripMenuItem,
            this.scriptUpdateToolStripMenuItem,
            this.viewDataToolStripMenuItem,
            this.countDistinctToolStripMenuItem,
            this.exportDataToolStripMenuItem,
            this.missingIndexesToolStripMenuItem1,
            this.indexStatisticsToolStripMenuItem});
            this.datagridMenu.Name = "cmsDatagrid";
            this.datagridMenu.Size = new System.Drawing.Size(158, 202);
            // 
            // viewCodeToolStripMenuItem
            // 
            this.viewCodeToolStripMenuItem.Name = "viewCodeToolStripMenuItem";
            this.viewCodeToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.viewCodeToolStripMenuItem.Text = "View Code";
            this.viewCodeToolStripMenuItem.Click += new System.EventHandler(this.viewCodeToolStripMenuItem_Click);
            // 
            // scriptSelectToolStripMenuItem
            // 
            this.scriptSelectToolStripMenuItem.Name = "scriptSelectToolStripMenuItem";
            this.scriptSelectToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.scriptSelectToolStripMenuItem.Text = "Script Select";
            this.scriptSelectToolStripMenuItem.Click += new System.EventHandler(this.scriptSelectToolStripMenuItem_Click);
            // 
            // scriptInsertToolStripMenuItem
            // 
            this.scriptInsertToolStripMenuItem.Name = "scriptInsertToolStripMenuItem";
            this.scriptInsertToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.scriptInsertToolStripMenuItem.Text = "Script Insert";
            this.scriptInsertToolStripMenuItem.Click += new System.EventHandler(this.scriptInsertToolStripMenuItem_Click);
            // 
            // scriptUpdateToolStripMenuItem
            // 
            this.scriptUpdateToolStripMenuItem.Name = "scriptUpdateToolStripMenuItem";
            this.scriptUpdateToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.scriptUpdateToolStripMenuItem.Text = "Script Update";
            this.scriptUpdateToolStripMenuItem.Click += new System.EventHandler(this.scriptUpdateToolStripMenuItem_Click);
            // 
            // viewDataToolStripMenuItem
            // 
            this.viewDataToolStripMenuItem.Name = "viewDataToolStripMenuItem";
            this.viewDataToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.viewDataToolStripMenuItem.Text = "View Data";
            this.viewDataToolStripMenuItem.Click += new System.EventHandler(this.viewDataToolStripMenuItem_Click);
            // 
            // countDistinctToolStripMenuItem
            // 
            this.countDistinctToolStripMenuItem.Name = "countDistinctToolStripMenuItem";
            this.countDistinctToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.countDistinctToolStripMenuItem.Text = "Count Distinct";
            this.countDistinctToolStripMenuItem.Click += new System.EventHandler(this.countDistinctToolStripMenuItem_Click);
            // 
            // exportDataToolStripMenuItem
            // 
            this.exportDataToolStripMenuItem.Name = "exportDataToolStripMenuItem";
            this.exportDataToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.exportDataToolStripMenuItem.Text = "Export Data";
            this.exportDataToolStripMenuItem.Click += new System.EventHandler(this.exportDataToolStripMenuItem_Click);
            // 
            // missingIndexesToolStripMenuItem1
            // 
            this.missingIndexesToolStripMenuItem1.Name = "missingIndexesToolStripMenuItem1";
            this.missingIndexesToolStripMenuItem1.Size = new System.Drawing.Size(157, 22);
            this.missingIndexesToolStripMenuItem1.Text = "Missing Indexes";
            this.missingIndexesToolStripMenuItem1.Click += new System.EventHandler(this.missingIndexesToolStripMenuItem1_Click);
            // 
            // indexStatisticsToolStripMenuItem
            // 
            this.indexStatisticsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusToolStripMenuItem1,
            this.limitedFragmentAnalysisToolStripMenuItem1,
            this.sampledFragmentAnalysisToolStripMenuItem1,
            this.detailedFragmentAnalysisToolStripMenuItem1});
            this.indexStatisticsToolStripMenuItem.Name = "indexStatisticsToolStripMenuItem";
            this.indexStatisticsToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.indexStatisticsToolStripMenuItem.Text = "Statistics";
            // 
            // statusToolStripMenuItem1
            // 
            this.statusToolStripMenuItem1.Name = "statusToolStripMenuItem1";
            this.statusToolStripMenuItem1.Size = new System.Drawing.Size(220, 22);
            this.statusToolStripMenuItem1.Text = "Status";
            this.statusToolStripMenuItem1.Click += new System.EventHandler(this.statusToolStripMenuItem1_Click);
            // 
            // limitedFragmentAnalysisToolStripMenuItem1
            // 
            this.limitedFragmentAnalysisToolStripMenuItem1.Name = "limitedFragmentAnalysisToolStripMenuItem1";
            this.limitedFragmentAnalysisToolStripMenuItem1.Size = new System.Drawing.Size(220, 22);
            this.limitedFragmentAnalysisToolStripMenuItem1.Text = "Limited Fragment Analysis";
            this.limitedFragmentAnalysisToolStripMenuItem1.Click += new System.EventHandler(this.limitedFragmentAnalysisToolStripMenuItem1_Click);
            // 
            // sampledFragmentAnalysisToolStripMenuItem1
            // 
            this.sampledFragmentAnalysisToolStripMenuItem1.Name = "sampledFragmentAnalysisToolStripMenuItem1";
            this.sampledFragmentAnalysisToolStripMenuItem1.Size = new System.Drawing.Size(220, 22);
            this.sampledFragmentAnalysisToolStripMenuItem1.Text = "Sampled Fragment Analysis";
            this.sampledFragmentAnalysisToolStripMenuItem1.Click += new System.EventHandler(this.sampledFragmentAnalysisToolStripMenuItem1_Click);
            // 
            // detailedFragmentAnalysisToolStripMenuItem1
            // 
            this.detailedFragmentAnalysisToolStripMenuItem1.Name = "detailedFragmentAnalysisToolStripMenuItem1";
            this.detailedFragmentAnalysisToolStripMenuItem1.Size = new System.Drawing.Size(220, 22);
            this.detailedFragmentAnalysisToolStripMenuItem1.Text = "Detailed Fragment Analysis";
            this.detailedFragmentAnalysisToolStripMenuItem1.Click += new System.EventHandler(this.detailedFragmentAnalysisToolStripMenuItem1_Click);
            // 
            // cmbDatabase
            // 
            this.cmbDatabase.DisplayMember = "database_name";
            this.cmbDatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDatabase.FormattingEnabled = true;
            this.cmbDatabase.Location = new System.Drawing.Point(63, 126);
            this.cmbDatabase.MaxDropDownItems = 20;
            this.cmbDatabase.Name = "cmbDatabase";
            this.cmbDatabase.Size = new System.Drawing.Size(176, 21);
            this.cmbDatabase.TabIndex = 5;
            this.tltMain.SetToolTip(this.cmbDatabase, "Drop down to connect and select a database.");
            this.cmbDatabase.ValueMember = "id";
            this.cmbDatabase.DropDown += new System.EventHandler(this.cmbDatabase_DropDown);
            this.cmbDatabase.SelectedIndexChanged += new System.EventHandler(this.cmbDatabase_SelectedIndexChanged);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Enabled = false;
            this.btnRefresh.Location = new System.Drawing.Point(507, 127);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(60, 21);
            this.btnRefresh.TabIndex = 9;
            this.btnRefresh.Text = "&Refresh";
            this.tltMain.SetToolTip(this.btnRefresh, "Refresh database objects script.");
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnExport
            // 
            this.btnExport.Enabled = false;
            this.btnExport.Location = new System.Drawing.Point(573, 127);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(60, 21);
            this.btnExport.TabIndex = 10;
            this.btnExport.Text = "&Script";
            this.tltMain.SetToolTip(this.btnExport, "Export selected objects script.");
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(5, 106);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(38, 13);
            this.lblServer.TabIndex = 6;
            this.lblServer.Text = "Server";
            // 
            // lblDatabase
            // 
            this.lblDatabase.AutoSize = true;
            this.lblDatabase.Location = new System.Drawing.Point(5, 130);
            this.lblDatabase.Name = "lblDatabase";
            this.lblDatabase.Size = new System.Drawing.Size(53, 13);
            this.lblDatabase.TabIndex = 7;
            this.lblDatabase.Text = "Database";
            // 
            // lblObjType
            // 
            this.lblObjType.AutoSize = true;
            this.lblObjType.Location = new System.Drawing.Point(254, 56);
            this.lblObjType.Name = "lblObjType";
            this.lblObjType.Size = new System.Drawing.Size(65, 13);
            this.lblObjType.TabIndex = 8;
            this.lblObjType.Text = "Object Type";
            // 
            // cmbAuthMode
            // 
            this.cmbAuthMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAuthMode.FormattingEnabled = true;
            this.cmbAuthMode.Items.AddRange(new object[] {
            "Windows Authentication",
            "SQL Server Authentication",
            "Microsoft Entra Password"});
            this.cmbAuthMode.Location = new System.Drawing.Point(63, 76);
            this.cmbAuthMode.Name = "cmbAuthMode";
            this.cmbAuthMode.Size = new System.Drawing.Size(176, 21);
            this.cmbAuthMode.TabIndex = 3;
            this.cmbAuthMode.SelectedIndexChanged += new System.EventHandler(this.cmbAuthMode_SelectedIndexChanged);
            // 
            // txtUsername
            // 
            this.txtUsername.Enabled = false;
            this.txtUsername.Location = new System.Drawing.Point(63, 29);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(176, 20);
            this.txtUsername.TabIndex = 1;
            this.txtUsername.TextChanged += new System.EventHandler(this.txtUsername_TextChanged);
            // 
            // txtPassword
            // 
            this.txtPassword.Enabled = false;
            this.txtPassword.Location = new System.Drawing.Point(63, 53);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(176, 20);
            this.txtPassword.TabIndex = 2;
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(5, 33);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(55, 13);
            this.lblUsername.TabIndex = 13;
            this.lblUsername.Text = "Username";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(5, 56);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 14;
            this.lblPassword.Text = "Password";
            // 
            // mainStatusStrip
            // 
            this.mainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblRow,
            this.lblCount});
            this.mainStatusStrip.Location = new System.Drawing.Point(0, 464);
            this.mainStatusStrip.Name = "mainStatusStrip";
            this.mainStatusStrip.Size = new System.Drawing.Size(684, 22);
            this.mainStatusStrip.TabIndex = 15;
            this.mainStatusStrip.Text = "statusStrip1";
            // 
            // lblRow
            // 
            this.lblRow.Name = "lblRow";
            this.lblRow.Size = new System.Drawing.Size(78, 17);
            this.lblRow.Text = "Object Count";
            // 
            // lblCount
            // 
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(13, 17);
            this.lblCount.Text = "0";
            // 
            // exportFileBrowser
            // 
            this.exportFileBrowser.DefaultExt = "sql";
            this.exportFileBrowser.Filter = "Sql files|*.sql|All files|*.*";
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.mainMenu.Size = new System.Drawing.Size(684, 24);
            this.mainMenu.TabIndex = 0;
            this.mainMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newQueryToolStripMenuItem,
            this.dataSearchQueryToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newQueryToolStripMenuItem
            // 
            this.newQueryToolStripMenuItem.Enabled = false;
            this.newQueryToolStripMenuItem.Name = "newQueryToolStripMenuItem";
            this.newQueryToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.newQueryToolStripMenuItem.Text = "New &Query";
            this.newQueryToolStripMenuItem.Click += new System.EventHandler(this.newQueryToolStripMenuItem_Click);
            // 
            // dataSearchQueryToolStripMenuItem
            // 
            this.dataSearchQueryToolStripMenuItem.Enabled = false;
            this.dataSearchQueryToolStripMenuItem.Name = "dataSearchQueryToolStripMenuItem";
            this.dataSearchQueryToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.dataSearchQueryToolStripMenuItem.Text = "&Data Search Query";
            this.dataSearchQueryToolStripMenuItem.Click += new System.EventHandler(this.dataSearchQueryToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Enabled = false;
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.exportToolStripMenuItem.Text = "&Script";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(168, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.serverMonitorToolStripMenuItem,
            this.cacheToolStripMenuItem,
            this.toolStripSeparator6,
            this.compareDatabasesToolStripMenuItem,
            this.toolStripSeparator5,
            this.tableMBytesToolStripMenuItem,
            this.tableStatisticsToolStripMenuItem,
            this.columnInconsistencyToolStripMenuItem,
            this.indexesToolStripMenuItem,
            this.toolStripSeparator1,
            this.serverInfoToolStripMenuItem,
            this.databaseFilesToolStripMenuItem,
            this.databasePermissionsToolStripMenuItem,
            this.databaseRolesToolStripMenuItem,
            this.toolStripSeparator3,
            this.optionsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // serverMonitorToolStripMenuItem
            // 
            this.serverMonitorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.processesToolStripMenuItem,
            this.deadlocksToolStripMenuItem,
            this.locksToolStripMenuItem,
            this.topQueriesTimeToolStripMenuItem,
            this.topQueriesReadsToolStripMenuItem,
            this.toolStripSeparator4,
            this.cpuToolStripMenuItem,
            this.jobsToolStripMenuItem});
            this.serverMonitorToolStripMenuItem.Enabled = false;
            this.serverMonitorToolStripMenuItem.Name = "serverMonitorToolStripMenuItem";
            this.serverMonitorToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.serverMonitorToolStripMenuItem.Text = "&Server Monitor";
            // 
            // processesToolStripMenuItem
            // 
            this.processesToolStripMenuItem.Enabled = false;
            this.processesToolStripMenuItem.Name = "processesToolStripMenuItem";
            this.processesToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.processesToolStripMenuItem.Text = "&Processes";
            this.processesToolStripMenuItem.Click += new System.EventHandler(this.processesToolStripMenuItem_Click);
            // 
            // deadlocksToolStripMenuItem
            // 
            this.deadlocksToolStripMenuItem.Enabled = false;
            this.deadlocksToolStripMenuItem.Name = "deadlocksToolStripMenuItem";
            this.deadlocksToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.deadlocksToolStripMenuItem.Text = "&Deadlocks";
            this.deadlocksToolStripMenuItem.Click += new System.EventHandler(this.deadlocksToolStripMenuItem_Click);
            // 
            // locksToolStripMenuItem
            // 
            this.locksToolStripMenuItem.Enabled = false;
            this.locksToolStripMenuItem.Name = "locksToolStripMenuItem";
            this.locksToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.locksToolStripMenuItem.Text = "&Locks";
            this.locksToolStripMenuItem.Click += new System.EventHandler(this.locksToolStripMenuItem_Click);
            // 
            // topQueriesTimeToolStripMenuItem
            // 
            this.topQueriesTimeToolStripMenuItem.Enabled = false;
            this.topQueriesTimeToolStripMenuItem.Name = "topQueriesTimeToolStripMenuItem";
            this.topQueriesTimeToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.topQueriesTimeToolStripMenuItem.Text = "&Top Queries (Time)";
            this.topQueriesTimeToolStripMenuItem.Click += new System.EventHandler(this.topQueriesTimeToolStripMenuItem_Click);
            // 
            // topQueriesReadsToolStripMenuItem
            // 
            this.topQueriesReadsToolStripMenuItem.Enabled = false;
            this.topQueriesReadsToolStripMenuItem.Name = "topQueriesReadsToolStripMenuItem";
            this.topQueriesReadsToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.topQueriesReadsToolStripMenuItem.Text = "Top &Queries (Reads)";
            this.topQueriesReadsToolStripMenuItem.Click += new System.EventHandler(this.topQueriesReadsToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(176, 6);
            // 
            // cpuToolStripMenuItem
            // 
            this.cpuToolStripMenuItem.Enabled = false;
            this.cpuToolStripMenuItem.Name = "cpuToolStripMenuItem";
            this.cpuToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.cpuToolStripMenuItem.Text = "Cp&u";
            this.cpuToolStripMenuItem.Click += new System.EventHandler(this.cpuToolStripMenuItem_Click);
            // 
            // jobsToolStripMenuItem
            // 
            this.jobsToolStripMenuItem.Enabled = false;
            this.jobsToolStripMenuItem.Name = "jobsToolStripMenuItem";
            this.jobsToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.jobsToolStripMenuItem.Text = "&Jobs";
            this.jobsToolStripMenuItem.Click += new System.EventHandler(this.jobsToolStripMenuItem_Click);
            // 
            // cacheToolStripMenuItem
            // 
            this.cacheToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.plansCacheToolStripMenuItem,
            this.databasesCacheToolStripMenuItem,
            this.objectsCacheToolStripMenuItem});
            this.cacheToolStripMenuItem.Enabled = false;
            this.cacheToolStripMenuItem.Name = "cacheToolStripMenuItem";
            this.cacheToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.cacheToolStripMenuItem.Text = "Cac&he";
            // 
            // plansCacheToolStripMenuItem
            // 
            this.plansCacheToolStripMenuItem.Enabled = false;
            this.plansCacheToolStripMenuItem.Name = "plansCacheToolStripMenuItem";
            this.plansCacheToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.plansCacheToolStripMenuItem.Text = "&Plans Cache";
            this.plansCacheToolStripMenuItem.Click += new System.EventHandler(this.planCachesToolStripMenuItem_Click);
            // 
            // databasesCacheToolStripMenuItem
            // 
            this.databasesCacheToolStripMenuItem.Enabled = false;
            this.databasesCacheToolStripMenuItem.Name = "databasesCacheToolStripMenuItem";
            this.databasesCacheToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.databasesCacheToolStripMenuItem.Text = "&Databases Cache";
            this.databasesCacheToolStripMenuItem.Click += new System.EventHandler(this.cacheDistrubutionToolStripMenuItem_Click);
            // 
            // objectsCacheToolStripMenuItem
            // 
            this.objectsCacheToolStripMenuItem.Enabled = false;
            this.objectsCacheToolStripMenuItem.Name = "objectsCacheToolStripMenuItem";
            this.objectsCacheToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.objectsCacheToolStripMenuItem.Text = "&Objects Cache";
            this.objectsCacheToolStripMenuItem.Click += new System.EventHandler(this.cachedObjectsToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(194, 6);
            // 
            // compareDatabasesToolStripMenuItem
            // 
            this.compareDatabasesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportSelectedObjectsToolStripMenuItem,
            this.compareExportedObjectsToolStripMenuItem});
            this.compareDatabasesToolStripMenuItem.Name = "compareDatabasesToolStripMenuItem";
            this.compareDatabasesToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.compareDatabasesToolStripMenuItem.Text = "Offline &Compare";
            // 
            // exportSelectedObjectsToolStripMenuItem
            // 
            this.exportSelectedObjectsToolStripMenuItem.Enabled = false;
            this.exportSelectedObjectsToolStripMenuItem.Name = "exportSelectedObjectsToolStripMenuItem";
            this.exportSelectedObjectsToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.exportSelectedObjectsToolStripMenuItem.Text = "&Export Selected Objects";
            this.exportSelectedObjectsToolStripMenuItem.Click += new System.EventHandler(this.exportSelectedObjectsToolStripMenuItem_Click);
            // 
            // compareExportedObjectsToolStripMenuItem
            // 
            this.compareExportedObjectsToolStripMenuItem.Name = "compareExportedObjectsToolStripMenuItem";
            this.compareExportedObjectsToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.compareExportedObjectsToolStripMenuItem.Text = "&Compare Exported Objects";
            this.compareExportedObjectsToolStripMenuItem.Click += new System.EventHandler(this.compareExportedObjectsToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(194, 6);
            // 
            // tableMBytesToolStripMenuItem
            // 
            this.tableMBytesToolStripMenuItem.Enabled = false;
            this.tableMBytesToolStripMenuItem.Name = "tableMBytesToolStripMenuItem";
            this.tableMBytesToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.tableMBytesToolStripMenuItem.Text = "&Tables MBytes";
            this.tableMBytesToolStripMenuItem.Click += new System.EventHandler(this.tableMBytesToolStripMenuItem_Click);
            // 
            // tableStatisticsToolStripMenuItem
            // 
            this.tableStatisticsToolStripMenuItem.Enabled = false;
            this.tableStatisticsToolStripMenuItem.Name = "tableStatisticsToolStripMenuItem";
            this.tableStatisticsToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.tableStatisticsToolStripMenuItem.Text = "T&ables Statistics";
            this.tableStatisticsToolStripMenuItem.Click += new System.EventHandler(this.tableStatisticsToolStripMenuItem_Click);
            // 
            // columnInconsistencyToolStripMenuItem
            // 
            this.columnInconsistencyToolStripMenuItem.Enabled = false;
            this.columnInconsistencyToolStripMenuItem.Name = "columnInconsistencyToolStripMenuItem";
            this.columnInconsistencyToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.columnInconsistencyToolStripMenuItem.Text = "Co&lumns Inconsistency";
            this.columnInconsistencyToolStripMenuItem.Click += new System.EventHandler(this.columnInconsistencyToolStripMenuItem_Click);
            // 
            // indexesToolStripMenuItem
            // 
            this.indexesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.missingForeignKeyIndexesToolStripMenuItem,
            this.missingIndexesToolStripMenuItem,
            this.existingIndexesToolStripMenuItem,
            this.statisticsToolStripMenuItem});
            this.indexesToolStripMenuItem.Enabled = false;
            this.indexesToolStripMenuItem.Name = "indexesToolStripMenuItem";
            this.indexesToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.indexesToolStripMenuItem.Text = "&Indexes";
            // 
            // missingForeignKeyIndexesToolStripMenuItem
            // 
            this.missingForeignKeyIndexesToolStripMenuItem.Enabled = false;
            this.missingForeignKeyIndexesToolStripMenuItem.Name = "missingForeignKeyIndexesToolStripMenuItem";
            this.missingForeignKeyIndexesToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.missingForeignKeyIndexesToolStripMenuItem.Text = "Missing &Foreign Key Indexes";
            this.missingForeignKeyIndexesToolStripMenuItem.Click += new System.EventHandler(this.missingForeignKeyIndexesToolStripMenuItem_Click);
            // 
            // missingIndexesToolStripMenuItem
            // 
            this.missingIndexesToolStripMenuItem.Enabled = false;
            this.missingIndexesToolStripMenuItem.Name = "missingIndexesToolStripMenuItem";
            this.missingIndexesToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.missingIndexesToolStripMenuItem.Text = "&Missing Indexes";
            this.missingIndexesToolStripMenuItem.Click += new System.EventHandler(this.missingIndexesToolStripMenuItem_Click);
            // 
            // existingIndexesToolStripMenuItem
            // 
            this.existingIndexesToolStripMenuItem.Enabled = false;
            this.existingIndexesToolStripMenuItem.Name = "existingIndexesToolStripMenuItem";
            this.existingIndexesToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.existingIndexesToolStripMenuItem.Text = "&Existing Indexes";
            this.existingIndexesToolStripMenuItem.Click += new System.EventHandler(this.existingIndexesToolStripMenuItem_Click);
            // 
            // statisticsToolStripMenuItem
            // 
            this.statisticsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusToolStripMenuItem,
            this.limitedFragmentAnalysisToolStripMenuItem,
            this.sampledFragmentAnalysisToolStripMenuItem,
            this.detailedFragmentAnalysisToolStripMenuItem});
            this.statisticsToolStripMenuItem.Enabled = false;
            this.statisticsToolStripMenuItem.Name = "statisticsToolStripMenuItem";
            this.statisticsToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.statisticsToolStripMenuItem.Text = "&Statistics";
            // 
            // statusToolStripMenuItem
            // 
            this.statusToolStripMenuItem.Name = "statusToolStripMenuItem";
            this.statusToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.statusToolStripMenuItem.Text = "&Status";
            this.statusToolStripMenuItem.Click += new System.EventHandler(this.statusToolStripMenuItem_Click);
            // 
            // limitedFragmentAnalysisToolStripMenuItem
            // 
            this.limitedFragmentAnalysisToolStripMenuItem.Name = "limitedFragmentAnalysisToolStripMenuItem";
            this.limitedFragmentAnalysisToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.limitedFragmentAnalysisToolStripMenuItem.Text = "&Limited Fragment Analysis";
            this.limitedFragmentAnalysisToolStripMenuItem.Click += new System.EventHandler(this.limitedFragmentAnalysisToolStripMenuItem_Click);
            // 
            // sampledFragmentAnalysisToolStripMenuItem
            // 
            this.sampledFragmentAnalysisToolStripMenuItem.Name = "sampledFragmentAnalysisToolStripMenuItem";
            this.sampledFragmentAnalysisToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.sampledFragmentAnalysisToolStripMenuItem.Text = "S&ampled Fragment Analysis";
            this.sampledFragmentAnalysisToolStripMenuItem.Click += new System.EventHandler(this.sampledFragmentAnalysisToolStripMenuItem_Click);
            // 
            // detailedFragmentAnalysisToolStripMenuItem
            // 
            this.detailedFragmentAnalysisToolStripMenuItem.Name = "detailedFragmentAnalysisToolStripMenuItem";
            this.detailedFragmentAnalysisToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.detailedFragmentAnalysisToolStripMenuItem.Text = "&Detailed Fragment Analysis";
            this.detailedFragmentAnalysisToolStripMenuItem.Click += new System.EventHandler(this.detailedFragmentAnalysisToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(194, 6);
            // 
            // serverInfoToolStripMenuItem
            // 
            this.serverInfoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configurationsToolStripMenuItem,
            this.propertiesToolStripMenuItem,
            this.rolesToolStripMenuItem});
            this.serverInfoToolStripMenuItem.Enabled = false;
            this.serverInfoToolStripMenuItem.Name = "serverInfoToolStripMenuItem";
            this.serverInfoToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.serverInfoToolStripMenuItem.Text = "Ser&ver Info";
            // 
            // configurationsToolStripMenuItem
            // 
            this.configurationsToolStripMenuItem.Enabled = false;
            this.configurationsToolStripMenuItem.Name = "configurationsToolStripMenuItem";
            this.configurationsToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.configurationsToolStripMenuItem.Text = "&Configurations";
            this.configurationsToolStripMenuItem.Click += new System.EventHandler(this.configurationsToolStripMenuItem_Click);
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Enabled = false;
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.propertiesToolStripMenuItem.Text = "&Properties";
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // rolesToolStripMenuItem
            // 
            this.rolesToolStripMenuItem.Enabled = false;
            this.rolesToolStripMenuItem.Name = "rolesToolStripMenuItem";
            this.rolesToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.rolesToolStripMenuItem.Text = "&Roles";
            this.rolesToolStripMenuItem.Click += new System.EventHandler(this.rolesToolStripMenuItem_Click);
            // 
            // databaseFilesToolStripMenuItem
            // 
            this.databaseFilesToolStripMenuItem.Enabled = false;
            this.databaseFilesToolStripMenuItem.Name = "databaseFilesToolStripMenuItem";
            this.databaseFilesToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.databaseFilesToolStripMenuItem.Text = "&Database Files";
            this.databaseFilesToolStripMenuItem.Click += new System.EventHandler(this.databaseFilesToolStripMenuItem_Click);
            // 
            // databasePermissionsToolStripMenuItem
            // 
            this.databasePermissionsToolStripMenuItem.Enabled = false;
            this.databasePermissionsToolStripMenuItem.Name = "databasePermissionsToolStripMenuItem";
            this.databasePermissionsToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.databasePermissionsToolStripMenuItem.Text = "Database P&ermissions";
            this.databasePermissionsToolStripMenuItem.Click += new System.EventHandler(this.databasePermissionsToolStripMenuItem_Click);
            // 
            // databaseRolesToolStripMenuItem
            // 
            this.databaseRolesToolStripMenuItem.Enabled = false;
            this.databaseRolesToolStripMenuItem.Name = "databaseRolesToolStripMenuItem";
            this.databaseRolesToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.databaseRolesToolStripMenuItem.Text = "Database &Roles";
            this.databaseRolesToolStripMenuItem.Click += new System.EventHandler(this.databaseRolesToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(194, 6);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.optionsToolStripMenuItem.Text = "&Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updatesToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // updatesToolStripMenuItem
            // 
            this.updatesToolStripMenuItem.Name = "updatesToolStripMenuItem";
            this.updatesToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.updatesToolStripMenuItem.Text = "&Check for Updates";
            this.updatesToolStripMenuItem.Click += new System.EventHandler(this.updatesToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.aboutToolStripMenuItem.Text = "&About SqlDbAid";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // lblContains
            // 
            this.lblContains.AutoSize = true;
            this.lblContains.Location = new System.Drawing.Point(254, 33);
            this.lblContains.Name = "lblContains";
            this.lblContains.Size = new System.Drawing.Size(65, 13);
            this.lblContains.TabIndex = 16;
            this.lblContains.Text = "Search Text";
            // 
            // txtContains
            // 
            this.txtContains.Location = new System.Drawing.Point(325, 29);
            this.txtContains.MaxLength = 200;
            this.txtContains.Name = "txtContains";
            this.txtContains.Size = new System.Drawing.Size(176, 20);
            this.txtContains.TabIndex = 6;
            this.tltMain.SetToolTip(this.txtContains, "Get only objects containig\r\nthis text in their definition\r\n(regular expressions s" +
        "upported).");
            this.txtContains.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtContains_KeyPress);
            // 
            // cmbServer
            // 
            this.cmbServer.FormattingEnabled = true;
            this.cmbServer.Location = new System.Drawing.Point(63, 101);
            this.cmbServer.Name = "cmbServer";
            this.cmbServer.Size = new System.Drawing.Size(176, 21);
            this.cmbServer.Sorted = true;
            this.cmbServer.TabIndex = 4;
            this.cmbServer.Text = "(local)";
            this.tltMain.SetToolTip(this.cmbServer, "SERVER\\INSTANCE[,PORT]");
            this.cmbServer.TextChanged += new System.EventHandler(this.cmbServer_TextChanged);
            // 
            // chlObjType
            // 
            this.chlObjType.CheckOnClick = true;
            this.chlObjType.FormattingEnabled = true;
            this.chlObjType.Location = new System.Drawing.Point(325, 53);
            this.chlObjType.Name = "chlObjType";
            this.chlObjType.Size = new System.Drawing.Size(176, 94);
            this.chlObjType.TabIndex = 8;
            this.tltMain.SetToolTip(this.chlObjType, "INS / DEL select / unselect all");
            this.chlObjType.KeyDown += new System.Windows.Forms.KeyEventHandler(this.chlObjType_KeyDown);
            // 
            // dgvCode
            // 
            this.dgvCode.AllowUserToAddRows = false;
            this.dgvCode.AllowUserToDeleteRows = false;
            this.dgvCode.AllowUserToOrderColumns = true;
            this.dgvCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCode.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvCode.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCode.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.selected,
            this.obj_type,
            this.obj_schema,
            this.obj_name,
            this.modify_date,
            this.code,
            this.ord_id});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCode.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvCode.Location = new System.Drawing.Point(8, 156);
            this.dgvCode.Name = "dgvCode";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCode.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvCode.ShowEditingIcon = false;
            this.dgvCode.Size = new System.Drawing.Size(667, 299);
            this.dgvCode.TabIndex = 11;
            this.dgvCode.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvCode_CellFormatting);
            this.dgvCode.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvCode_CellMouseDoubleClick);
            this.dgvCode.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvCode_ColumnHeaderMouseClick);
            this.dgvCode.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvCode_RowsAdded);
            this.dgvCode.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvCode_MouseDown);
            // 
            // selected
            // 
            this.selected.DataPropertyName = "selected";
            this.selected.HeaderText = "Sel.";
            this.selected.MinimumWidth = 30;
            this.selected.Name = "selected";
            this.selected.Width = 30;
            // 
            // obj_type
            // 
            this.obj_type.DataPropertyName = "obj_type";
            this.obj_type.HeaderText = "Type";
            this.obj_type.Name = "obj_type";
            this.obj_type.ReadOnly = true;
            this.obj_type.Width = 80;
            // 
            // obj_schema
            // 
            this.obj_schema.DataPropertyName = "obj_schema";
            this.obj_schema.HeaderText = "Schema";
            this.obj_schema.Name = "obj_schema";
            this.obj_schema.ReadOnly = true;
            this.obj_schema.Width = 60;
            // 
            // obj_name
            // 
            this.obj_name.DataPropertyName = "obj_name";
            this.obj_name.HeaderText = "Name";
            this.obj_name.Name = "obj_name";
            this.obj_name.ReadOnly = true;
            this.obj_name.Width = 200;
            // 
            // modify_date
            // 
            this.modify_date.DataPropertyName = "modify_date";
            this.modify_date.HeaderText = "Modified On";
            this.modify_date.Name = "modify_date";
            this.modify_date.ReadOnly = true;
            this.modify_date.Width = 120;
            // 
            // code
            // 
            this.code.DataPropertyName = "code";
            this.code.HeaderText = "Code";
            this.code.Name = "code";
            this.code.ReadOnly = true;
            this.code.Width = 1000;
            // 
            // ord_id
            // 
            this.ord_id.DataPropertyName = "ord_id";
            this.ord_id.HeaderText = "ord_id";
            this.ord_id.Name = "ord_id";
            this.ord_id.ReadOnly = true;
            this.ord_id.Visible = false;
            // 
            // chkRegex
            // 
            this.chkRegex.AutoSize = true;
            this.chkRegex.Location = new System.Drawing.Point(507, 32);
            this.chkRegex.Name = "chkRegex";
            this.chkRegex.Size = new System.Drawing.Size(117, 17);
            this.chkRegex.TabIndex = 7;
            this.chkRegex.Text = "Regular Expression";
            this.chkRegex.UseVisualStyleBackColor = true;
            // 
            // lblAuthentication
            // 
            this.lblAuthentication.AutoSize = true;
            this.lblAuthentication.Location = new System.Drawing.Point(5, 79);
            this.lblAuthentication.Name = "lblAuthentication";
            this.lblAuthentication.Size = new System.Drawing.Size(47, 13);
            this.lblAuthentication.TabIndex = 17;
            this.lblAuthentication.Text = "Authent.";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 486);
            this.Controls.Add(this.lblAuthentication);
            this.Controls.Add(this.chlObjType);
            this.Controls.Add(this.chkRegex);
            this.Controls.Add(this.dgvCode);
            this.Controls.Add(this.cmbServer);
            this.Controls.Add(this.txtContains);
            this.Controls.Add(this.lblContains);
            this.Controls.Add(this.mainStatusStrip);
            this.Controls.Add(this.mainMenu);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblServer);
            this.Controls.Add(this.lblObjType);
            this.Controls.Add(this.lblDatabase);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.cmbAuthMode);
            this.Controls.Add(this.cmbDatabase);
            this.Controls.Add(this.btnExport);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenu;
            this.MinimumSize = new System.Drawing.Size(700, 400);
            this.Name = "MainForm";
            this.Text = "SqlDbAid";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.datagridMenu.ResumeLayout(false);
            this.mainStatusStrip.ResumeLayout(false);
            this.mainStatusStrip.PerformLayout();
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCode)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbDatabase;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.Label lblDatabase;
        private System.Windows.Forms.FolderBrowserDialog exportFolderBrowser;
        private System.Windows.Forms.Label lblObjType;
        private System.Windows.Forms.ComboBox cmbAuthMode;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.StatusStrip mainStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblRow;
        private System.Windows.Forms.ToolStripStatusLabel lblCount;
        private System.Windows.Forms.SaveFileDialog exportFileBrowser;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ContextMenuStrip datagridMenu;
        private System.Windows.Forms.ToolStripMenuItem viewCodeToolStripMenuItem;
        private System.Windows.Forms.Label lblContains;
        private System.Windows.Forms.TextBox txtContains;
        private System.Windows.Forms.ToolStripMenuItem scriptSelectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scriptInsertToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scriptUpdateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ComboBox cmbServer;
        private System.Windows.Forms.ToolStripMenuItem exportDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tableMBytesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newQueryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem countDistinctToolStripMenuItem;
        private System.Windows.Forms.ToolTip tltMain;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem serverMonitorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem processesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem locksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem topQueriesReadsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem indexesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem missingForeignKeyIndexesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem missingIndexesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem existingIndexesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compareDatabasesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportSelectedObjectsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compareExportedObjectsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataSearchQueryToolStripMenuItem;
        private System.Windows.Forms.DataGridView dgvCode;
        private System.Windows.Forms.DataGridViewCheckBoxColumn selected;
        private System.Windows.Forms.DataGridViewTextBoxColumn obj_type;
        private System.Windows.Forms.DataGridViewTextBoxColumn obj_schema;
        private System.Windows.Forms.DataGridViewTextBoxColumn obj_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn modify_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn code;
        private System.Windows.Forms.DataGridViewTextBoxColumn ord_id;
        private System.Windows.Forms.ToolStripMenuItem databaseFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem statisticsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem statusToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem limitedFragmentAnalysisToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sampledFragmentAnalysisToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem detailedFragmentAnalysisToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem indexStatisticsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem statusToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem limitedFragmentAnalysisToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem sampledFragmentAnalysisToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem detailedFragmentAnalysisToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem topQueriesTimeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tableStatisticsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem databasePermissionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem serverInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configurationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem cpuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jobsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rolesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem databaseRolesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem columnInconsistencyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cacheToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem databasesCacheToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem objectsCacheToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.CheckBox chkRegex;
        private System.Windows.Forms.ToolStripMenuItem missingIndexesToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem plansCacheToolStripMenuItem;
        private System.Windows.Forms.CheckedListBox chlObjType;
        private System.Windows.Forms.ToolStripMenuItem deadlocksToolStripMenuItem;
        private System.Windows.Forms.Label lblAuthentication;
    }
}

