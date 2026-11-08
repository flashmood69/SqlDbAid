namespace SqlDbAid
{
    partial class OptionForm
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
            this.tabOption = new System.Windows.Forms.TabControl();
            this.tpgScripting = new System.Windows.Forms.TabPage();
            this.chkIncludeIndexes = new System.Windows.Forms.CheckBox();
            this.chkHideSystemDbs = new System.Windows.Forms.CheckBox();
            this.chkSingleFileExport = new System.Windows.Forms.CheckBox();
            this.chkAddDropCommand = new System.Windows.Forms.CheckBox();
            this.tpgQueries = new System.Windows.Forms.TabPage();
            this.lblRows = new System.Windows.Forms.Label();
            this.chkOpenWithNoLock = new System.Windows.Forms.CheckBox();
            this.numOpenTopRows = new System.Windows.Forms.NumericUpDown();
            this.lblOpenTable = new System.Windows.Forms.Label();
            this.numConnectionTimeout = new System.Windows.Forms.NumericUpDown();
            this.lblConnectionTimeout = new System.Windows.Forms.Label();
            this.numExecutionTimeout = new System.Windows.Forms.NumericUpDown();
            this.lblExecutionTimeout = new System.Windows.Forms.Label();
            this.tpgTools = new System.Windows.Forms.TabPage();
            this.grbServerActivity = new System.Windows.Forms.GroupBox();
            this.chkGetLockedObjectName = new System.Windows.Forms.CheckBox();
            this.lblTopQryHours = new System.Windows.Forms.Label();
            this.numTopQueriesWithin = new System.Windows.Forms.NumericUpDown();
            this.lblTopQry = new System.Windows.Forms.Label();
            this.chkHideAppQueries = new System.Windows.Forms.CheckBox();
            this.tpgServers = new System.Windows.Forms.TabPage();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.lstServer = new System.Windows.Forms.ListBox();
            this.tpgSqlFormatting = new System.Windows.Forms.TabPage();
            this.lblSqlFormatKeywordCase = new System.Windows.Forms.Label();
            this.cmbSqlFormatKeywordCase = new System.Windows.Forms.ComboBox();
            this.lblSqlFormatIdentifierQuoting = new System.Windows.Forms.Label();
            this.cmbSqlFormatIdentifierQuoting = new System.Windows.Forms.ComboBox();
            this.lblSqlFormatIndentSize = new System.Windows.Forms.Label();
            this.numSqlFormatIndentSize = new System.Windows.Forms.NumericUpDown();
            this.chkSqlFormatUseTabs = new System.Windows.Forms.CheckBox();
            this.chkSqlFormatLogicalOperatorNewlineBefore = new System.Windows.Forms.CheckBox();
            this.chkSqlFormatIndentLists = new System.Windows.Forms.CheckBox();
            this.lblSqlFormatCommaPosition = new System.Windows.Forms.Label();
            this.cmbSqlFormatCommaPosition = new System.Windows.Forms.ComboBox();
            this.lblSqlFormatOnPosition = new System.Windows.Forms.Label();
            this.cmbSqlFormatOnPosition = new System.Windows.Forms.ComboBox();
            this.lblSqlFormatTablePosition = new System.Windows.Forms.Label();
            this.cmbSqlFormatTablePosition = new System.Windows.Forms.ComboBox();
            this.lblSqlFormatJoinIndentation = new System.Windows.Forms.Label();
            this.numSqlFormatJoinIndentation = new System.Windows.Forms.NumericUpDown();
            this.chkSqlFormatIndentJoinConditions = new System.Windows.Forms.CheckBox();
            this.lblSqlFormatLinesBetweenQueries = new System.Windows.Forms.Label();
            this.numSqlFormatLinesBetweenQueries = new System.Windows.Forms.NumericUpDown();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.exportFileBrowser = new System.Windows.Forms.SaveFileDialog();
            this.importFile = new System.Windows.Forms.OpenFileDialog();
            this.tabOption.SuspendLayout();
            this.tpgScripting.SuspendLayout();
            this.tpgQueries.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOpenTopRows)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numConnectionTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numExecutionTimeout)).BeginInit();
            this.tpgTools.SuspendLayout();
            this.grbServerActivity.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTopQueriesWithin)).BeginInit();
            this.tpgServers.SuspendLayout();
            this.tpgSqlFormatting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSqlFormatIndentSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSqlFormatJoinIndentation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSqlFormatLinesBetweenQueries)).BeginInit();
            this.SuspendLayout();
            // 
            // tabOption
            // 
            this.tabOption.Controls.Add(this.tpgScripting);
            this.tabOption.Controls.Add(this.tpgQueries);
            this.tabOption.Controls.Add(this.tpgTools);
            this.tabOption.Controls.Add(this.tpgServers);
            this.tabOption.Controls.Add(this.tpgSqlFormatting);
            this.tabOption.Location = new System.Drawing.Point(12, 12);
            this.tabOption.Name = "tabOption";
            this.tabOption.SelectedIndex = 0;
            this.tabOption.Size = new System.Drawing.Size(536, 306);
            this.tabOption.TabIndex = 0;
            // 
            // tpgScripting
            // 
            this.tpgScripting.Controls.Add(this.chkIncludeIndexes);
            this.tpgScripting.Controls.Add(this.chkHideSystemDbs);
            this.tpgScripting.Controls.Add(this.chkSingleFileExport);
            this.tpgScripting.Controls.Add(this.chkAddDropCommand);
            this.tpgScripting.Location = new System.Drawing.Point(4, 22);
            this.tpgScripting.Name = "tpgScripting";
            this.tpgScripting.Padding = new System.Windows.Forms.Padding(3);
            this.tpgScripting.Size = new System.Drawing.Size(528, 280);
            this.tpgScripting.TabIndex = 0;
            this.tpgScripting.Text = "Scripting";
            this.tpgScripting.UseVisualStyleBackColor = true;
            // 
            // chkIncludeIndexes
            // 
            this.chkIncludeIndexes.AutoSize = true;
            this.chkIncludeIndexes.Checked = true;
            this.chkIncludeIndexes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIncludeIndexes.Location = new System.Drawing.Point(6, 53);
            this.chkIncludeIndexes.Name = "chkIncludeIndexes";
            this.chkIncludeIndexes.Size = new System.Drawing.Size(100, 17);
            this.chkIncludeIndexes.TabIndex = 4;
            this.chkIncludeIndexes.Text = "Include indexes";
            this.chkIncludeIndexes.UseVisualStyleBackColor = true;
            // 
            // chkHideSystemDbs
            // 
            this.chkHideSystemDbs.AutoSize = true;
            this.chkHideSystemDbs.Checked = true;
            this.chkHideSystemDbs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHideSystemDbs.Location = new System.Drawing.Point(6, 76);
            this.chkHideSystemDbs.Name = "chkHideSystemDbs";
            this.chkHideSystemDbs.Size = new System.Drawing.Size(135, 17);
            this.chkHideSystemDbs.TabIndex = 3;
            this.chkHideSystemDbs.Text = "Hide system databases";
            this.chkHideSystemDbs.UseVisualStyleBackColor = true;
            // 
            // chkSingleFileExport
            // 
            this.chkSingleFileExport.AutoSize = true;
            this.chkSingleFileExport.Checked = true;
            this.chkSingleFileExport.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSingleFileExport.Location = new System.Drawing.Point(6, 30);
            this.chkSingleFileExport.Name = "chkSingleFileExport";
            this.chkSingleFileExport.Size = new System.Drawing.Size(103, 17);
            this.chkSingleFileExport.TabIndex = 2;
            this.chkSingleFileExport.Text = "Single file export";
            this.chkSingleFileExport.UseVisualStyleBackColor = true;
            // 
            // chkAddDropCommand
            // 
            this.chkAddDropCommand.AutoSize = true;
            this.chkAddDropCommand.Location = new System.Drawing.Point(6, 6);
            this.chkAddDropCommand.Name = "chkAddDropCommand";
            this.chkAddDropCommand.Size = new System.Drawing.Size(118, 17);
            this.chkAddDropCommand.TabIndex = 1;
            this.chkAddDropCommand.Text = "Add drop command";
            this.chkAddDropCommand.UseVisualStyleBackColor = true;
            // 
            // tpgQueries
            // 
            this.tpgQueries.Controls.Add(this.lblRows);
            this.tpgQueries.Controls.Add(this.chkOpenWithNoLock);
            this.tpgQueries.Controls.Add(this.numOpenTopRows);
            this.tpgQueries.Controls.Add(this.lblOpenTable);
            this.tpgQueries.Controls.Add(this.numConnectionTimeout);
            this.tpgQueries.Controls.Add(this.lblConnectionTimeout);
            this.tpgQueries.Controls.Add(this.numExecutionTimeout);
            this.tpgQueries.Controls.Add(this.lblExecutionTimeout);
            this.tpgQueries.Location = new System.Drawing.Point(4, 22);
            this.tpgQueries.Name = "tpgQueries";
            this.tpgQueries.Padding = new System.Windows.Forms.Padding(3);
            this.tpgQueries.Size = new System.Drawing.Size(528, 280);
            this.tpgQueries.TabIndex = 1;
            this.tpgQueries.Text = "Queries";
            this.tpgQueries.UseVisualStyleBackColor = true;
            // 
            // lblRows
            // 
            this.lblRows.AutoSize = true;
            this.lblRows.Location = new System.Drawing.Point(182, 64);
            this.lblRows.Name = "lblRows";
            this.lblRows.Size = new System.Drawing.Size(66, 13);
            this.lblRows.TabIndex = 0;
            this.lblRows.Text = "rows (0 = all)";
            // 
            // chkOpenWithNoLock
            // 
            this.chkOpenWithNoLock.AutoSize = true;
            this.chkOpenWithNoLock.Checked = true;
            this.chkOpenWithNoLock.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOpenWithNoLock.Location = new System.Drawing.Point(254, 63);
            this.chkOpenWithNoLock.Name = "chkOpenWithNoLock";
            this.chkOpenWithNoLock.Size = new System.Drawing.Size(83, 17);
            this.chkOpenWithNoLock.TabIndex = 3;
            this.chkOpenWithNoLock.Text = "with no lock";
            this.chkOpenWithNoLock.UseVisualStyleBackColor = true;
            // 
            // numOpenTopRows
            // 
            this.numOpenTopRows.Increment = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numOpenTopRows.Location = new System.Drawing.Point(110, 62);
            this.numOpenTopRows.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numOpenTopRows.Name = "numOpenTopRows";
            this.numOpenTopRows.Size = new System.Drawing.Size(69, 20);
            this.numOpenTopRows.TabIndex = 2;
            this.numOpenTopRows.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // lblOpenTable
            // 
            this.lblOpenTable.AutoSize = true;
            this.lblOpenTable.Location = new System.Drawing.Point(6, 64);
            this.lblOpenTable.Name = "lblOpenTable";
            this.lblOpenTable.Size = new System.Drawing.Size(104, 13);
            this.lblOpenTable.TabIndex = 0;
            this.lblOpenTable.Text = "Open table/view top";
            // 
            // numConnectionTimeout
            // 
            this.numConnectionTimeout.Location = new System.Drawing.Point(110, 11);
            this.numConnectionTimeout.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.numConnectionTimeout.Name = "numConnectionTimeout";
            this.numConnectionTimeout.Size = new System.Drawing.Size(69, 20);
            this.numConnectionTimeout.TabIndex = 0;
            // 
            // lblConnectionTimeout
            // 
            this.lblConnectionTimeout.AutoSize = true;
            this.lblConnectionTimeout.Location = new System.Drawing.Point(6, 13);
            this.lblConnectionTimeout.Name = "lblConnectionTimeout";
            this.lblConnectionTimeout.Size = new System.Drawing.Size(98, 13);
            this.lblConnectionTimeout.TabIndex = 0;
            this.lblConnectionTimeout.Text = "Connection timeout";
            // 
            // numExecutionTimeout
            // 
            this.numExecutionTimeout.Location = new System.Drawing.Point(110, 36);
            this.numExecutionTimeout.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.numExecutionTimeout.Name = "numExecutionTimeout";
            this.numExecutionTimeout.Size = new System.Drawing.Size(69, 20);
            this.numExecutionTimeout.TabIndex = 1;
            // 
            // lblExecutionTimeout
            // 
            this.lblExecutionTimeout.AutoSize = true;
            this.lblExecutionTimeout.Location = new System.Drawing.Point(6, 38);
            this.lblExecutionTimeout.Name = "lblExecutionTimeout";
            this.lblExecutionTimeout.Size = new System.Drawing.Size(91, 13);
            this.lblExecutionTimeout.TabIndex = 0;
            this.lblExecutionTimeout.Text = "Execution timeout";
            // 
            // tpgTools
            // 
            this.tpgTools.Controls.Add(this.grbServerActivity);
            this.tpgTools.Location = new System.Drawing.Point(4, 22);
            this.tpgTools.Name = "tpgTools";
            this.tpgTools.Padding = new System.Windows.Forms.Padding(3);
            this.tpgTools.Size = new System.Drawing.Size(528, 280);
            this.tpgTools.TabIndex = 2;
            this.tpgTools.Text = "Tools";
            this.tpgTools.UseVisualStyleBackColor = true;
            // 
            // grbServerActivity
            // 
            this.grbServerActivity.Controls.Add(this.chkGetLockedObjectName);
            this.grbServerActivity.Controls.Add(this.lblTopQryHours);
            this.grbServerActivity.Controls.Add(this.numTopQueriesWithin);
            this.grbServerActivity.Controls.Add(this.lblTopQry);
            this.grbServerActivity.Controls.Add(this.chkHideAppQueries);
            this.grbServerActivity.Location = new System.Drawing.Point(7, 7);
            this.grbServerActivity.Name = "grbServerActivity";
            this.grbServerActivity.Size = new System.Drawing.Size(327, 104);
            this.grbServerActivity.TabIndex = 0;
            this.grbServerActivity.TabStop = false;
            this.grbServerActivity.Text = "Server Activity";
            // 
            // chkGetLockedObjectName
            // 
            this.chkGetLockedObjectName.AutoSize = true;
            this.chkGetLockedObjectName.Checked = true;
            this.chkGetLockedObjectName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGetLockedObjectName.Location = new System.Drawing.Point(7, 43);
            this.chkGetLockedObjectName.Name = "chkGetLockedObjectName";
            this.chkGetLockedObjectName.Size = new System.Drawing.Size(260, 17);
            this.chkGetLockedObjectName.TabIndex = 2;
            this.chkGetLockedObjectName.Text = "Try to get locked object name (could be blocking)";
            this.chkGetLockedObjectName.UseVisualStyleBackColor = true;
            // 
            // lblTopQryHours
            // 
            this.lblTopQryHours.AutoSize = true;
            this.lblTopQryHours.Location = new System.Drawing.Point(180, 72);
            this.lblTopQryHours.Name = "lblTopQryHours";
            this.lblTopQryHours.Size = new System.Drawing.Size(104, 13);
            this.lblTopQryHours.TabIndex = 0;
            this.lblTopQryHours.Text = "minutes (0 = no filter)";
            // 
            // numTopQueriesWithin
            // 
            this.numTopQueriesWithin.Location = new System.Drawing.Point(105, 70);
            this.numTopQueriesWithin.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numTopQueriesWithin.Name = "numTopQueriesWithin";
            this.numTopQueriesWithin.Size = new System.Drawing.Size(69, 20);
            this.numTopQueriesWithin.TabIndex = 1;
            // 
            // lblTopQry
            // 
            this.lblTopQry.AutoSize = true;
            this.lblTopQry.Location = new System.Drawing.Point(6, 72);
            this.lblTopQry.Name = "lblTopQry";
            this.lblTopQry.Size = new System.Drawing.Size(93, 13);
            this.lblTopQry.TabIndex = 0;
            this.lblTopQry.Text = "Top queries within";
            // 
            // chkHideAppQueries
            // 
            this.chkHideAppQueries.AutoSize = true;
            this.chkHideAppQueries.Checked = true;
            this.chkHideAppQueries.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHideAppQueries.Location = new System.Drawing.Point(7, 20);
            this.chkHideAppQueries.Name = "chkHideAppQueries";
            this.chkHideAppQueries.Size = new System.Drawing.Size(230, 17);
            this.chkHideAppQueries.TabIndex = 0;
            this.chkHideAppQueries.Text = "Hide activities generated by this application";
            this.chkHideAppQueries.UseVisualStyleBackColor = true;
            // 
            // tpgServers
            // 
            this.tpgServers.Controls.Add(this.btnExport);
            this.tpgServers.Controls.Add(this.btnImport);
            this.tpgServers.Controls.Add(this.btnSearch);
            this.tpgServers.Controls.Add(this.btnClear);
            this.tpgServers.Controls.Add(this.btnRemove);
            this.tpgServers.Controls.Add(this.btnAdd);
            this.tpgServers.Controls.Add(this.txtServer);
            this.tpgServers.Controls.Add(this.lstServer);
            this.tpgServers.Location = new System.Drawing.Point(4, 22);
            this.tpgServers.Name = "tpgServers";
            this.tpgServers.Size = new System.Drawing.Size(528, 280);
            this.tpgServers.TabIndex = 3;
            this.tpgServers.Text = "Servers";
            this.tpgServers.UseVisualStyleBackColor = true;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(85, 170);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(67, 23);
            this.btnExport.TabIndex = 7;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(4, 170);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(67, 23);
            this.btnImport.TabIndex = 6;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(311, 144);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(26, 21);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "...";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(266, 144);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(39, 21);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(205, 144);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(55, 21);
            this.btnRemove.TabIndex = 3;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(158, 144);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(41, 21);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(4, 144);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(148, 20);
            this.txtServer.TabIndex = 1;
            this.txtServer.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtServer_KeyPress);
            // 
            // lstServer
            // 
            this.lstServer.FormattingEnabled = true;
            this.lstServer.Location = new System.Drawing.Point(3, 3);
            this.lstServer.Name = "lstServer";
            this.lstServer.Size = new System.Drawing.Size(334, 134);
            this.lstServer.Sorted = true;
            this.lstServer.TabIndex = 0;
            // 
            // tpgSqlFormatting
            // 
            this.tpgSqlFormatting.Controls.Add(this.lblSqlFormatKeywordCase);
            this.tpgSqlFormatting.Controls.Add(this.cmbSqlFormatKeywordCase);
            this.tpgSqlFormatting.Controls.Add(this.lblSqlFormatIdentifierQuoting);
            this.tpgSqlFormatting.Controls.Add(this.cmbSqlFormatIdentifierQuoting);
            this.tpgSqlFormatting.Controls.Add(this.lblSqlFormatIndentSize);
            this.tpgSqlFormatting.Controls.Add(this.numSqlFormatIndentSize);
            this.tpgSqlFormatting.Controls.Add(this.chkSqlFormatUseTabs);
            this.tpgSqlFormatting.Controls.Add(this.chkSqlFormatLogicalOperatorNewlineBefore);
            this.tpgSqlFormatting.Controls.Add(this.chkSqlFormatIndentLists);
            this.tpgSqlFormatting.Controls.Add(this.lblSqlFormatCommaPosition);
            this.tpgSqlFormatting.Controls.Add(this.cmbSqlFormatCommaPosition);
            this.tpgSqlFormatting.Controls.Add(this.lblSqlFormatOnPosition);
            this.tpgSqlFormatting.Controls.Add(this.cmbSqlFormatOnPosition);
            this.tpgSqlFormatting.Controls.Add(this.lblSqlFormatTablePosition);
            this.tpgSqlFormatting.Controls.Add(this.cmbSqlFormatTablePosition);
            this.tpgSqlFormatting.Controls.Add(this.lblSqlFormatJoinIndentation);
            this.tpgSqlFormatting.Controls.Add(this.numSqlFormatJoinIndentation);
            this.tpgSqlFormatting.Controls.Add(this.chkSqlFormatIndentJoinConditions);
            this.tpgSqlFormatting.Controls.Add(this.lblSqlFormatLinesBetweenQueries);
            this.tpgSqlFormatting.Controls.Add(this.numSqlFormatLinesBetweenQueries);
            this.tpgSqlFormatting.Location = new System.Drawing.Point(4, 22);
            this.tpgSqlFormatting.Name = "tpgSqlFormatting";
            this.tpgSqlFormatting.Padding = new System.Windows.Forms.Padding(3);
            this.tpgSqlFormatting.Size = new System.Drawing.Size(528, 280);
            this.tpgSqlFormatting.TabIndex = 4;
            this.tpgSqlFormatting.Text = "SQL Formatting";
            this.tpgSqlFormatting.UseVisualStyleBackColor = true;
            // 
            // lblSqlFormatKeywordCase
            // 
            this.lblSqlFormatKeywordCase.AutoSize = true;
            this.lblSqlFormatKeywordCase.Location = new System.Drawing.Point(6, 13);
            this.lblSqlFormatKeywordCase.Name = "lblSqlFormatKeywordCase";
            this.lblSqlFormatKeywordCase.Size = new System.Drawing.Size(74, 13);
            this.lblSqlFormatKeywordCase.TabIndex = 0;
            this.lblSqlFormatKeywordCase.Text = "Keyword case";
            // 
            // cmbSqlFormatKeywordCase
            // 
            this.cmbSqlFormatKeywordCase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSqlFormatKeywordCase.FormattingEnabled = true;
            this.cmbSqlFormatKeywordCase.Items.AddRange(new object[] {
            "Uppercase",
            "Lowercase",
            "Preserve"});
            this.cmbSqlFormatKeywordCase.Location = new System.Drawing.Point(119, 10);
            this.cmbSqlFormatKeywordCase.Name = "cmbSqlFormatKeywordCase";
            this.cmbSqlFormatKeywordCase.Size = new System.Drawing.Size(132, 21);
            this.cmbSqlFormatKeywordCase.TabIndex = 1;
            this.cmbSqlFormatKeywordCase.SelectedIndex = 0;
            // 
            // lblSqlFormatIdentifierQuoting
            // 
            this.lblSqlFormatIdentifierQuoting.AutoSize = true;
            this.lblSqlFormatIdentifierQuoting.Location = new System.Drawing.Point(258, 13);
            this.lblSqlFormatIdentifierQuoting.Name = "lblSqlFormatIdentifierQuoting";
            this.lblSqlFormatIdentifierQuoting.Size = new System.Drawing.Size(85, 13);
            this.lblSqlFormatIdentifierQuoting.TabIndex = 0;
            this.lblSqlFormatIdentifierQuoting.Text = "Identifier quoting";
            // 
            // cmbSqlFormatIdentifierQuoting
            // 
            this.cmbSqlFormatIdentifierQuoting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSqlFormatIdentifierQuoting.FormattingEnabled = true;
            this.cmbSqlFormatIdentifierQuoting.Items.AddRange(new object[] {
            "None",
            "Double quotes",
            "Square brackets"});
            this.cmbSqlFormatIdentifierQuoting.Location = new System.Drawing.Point(370, 10);
            this.cmbSqlFormatIdentifierQuoting.Name = "cmbSqlFormatIdentifierQuoting";
            this.cmbSqlFormatIdentifierQuoting.Size = new System.Drawing.Size(132, 21);
            this.cmbSqlFormatIdentifierQuoting.TabIndex = 2;
            this.cmbSqlFormatIdentifierQuoting.SelectedIndex = 0;
            // 
            // lblSqlFormatIndentSize
            // 
            this.lblSqlFormatIndentSize.AutoSize = true;
            this.lblSqlFormatIndentSize.Location = new System.Drawing.Point(258, 156);
            this.lblSqlFormatIndentSize.Name = "lblSqlFormatIndentSize";
            this.lblSqlFormatIndentSize.Size = new System.Drawing.Size(58, 13);
            this.lblSqlFormatIndentSize.TabIndex = 0;
            this.lblSqlFormatIndentSize.Text = "Indent size";
            // 
            // numSqlFormatIndentSize
            // 
            this.numSqlFormatIndentSize.Location = new System.Drawing.Point(370, 154);
            this.numSqlFormatIndentSize.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numSqlFormatIndentSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSqlFormatIndentSize.Name = "numSqlFormatIndentSize";
            this.numSqlFormatIndentSize.Size = new System.Drawing.Size(70, 20);
            this.numSqlFormatIndentSize.TabIndex = 3;
            this.numSqlFormatIndentSize.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // chkSqlFormatUseTabs
            // 
            this.chkSqlFormatUseTabs.AutoSize = true;
            this.chkSqlFormatUseTabs.Checked = true;
            this.chkSqlFormatUseTabs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSqlFormatUseTabs.Location = new System.Drawing.Point(9, 155);
            this.chkSqlFormatUseTabs.Name = "chkSqlFormatUseTabs";
            this.chkSqlFormatUseTabs.Size = new System.Drawing.Size(138, 17);
            this.chkSqlFormatUseTabs.TabIndex = 4;
            this.chkSqlFormatUseTabs.Text = "Use tabs for indentation";
            this.chkSqlFormatUseTabs.UseVisualStyleBackColor = true;
            // 
            // chkSqlFormatLogicalOperatorNewlineBefore
            // 
            this.chkSqlFormatLogicalOperatorNewlineBefore.AutoSize = true;
            this.chkSqlFormatLogicalOperatorNewlineBefore.Checked = true;
            this.chkSqlFormatLogicalOperatorNewlineBefore.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSqlFormatLogicalOperatorNewlineBefore.Location = new System.Drawing.Point(9, 187);
            this.chkSqlFormatLogicalOperatorNewlineBefore.Name = "chkSqlFormatLogicalOperatorNewlineBefore";
            this.chkSqlFormatLogicalOperatorNewlineBefore.Size = new System.Drawing.Size(223, 17);
            this.chkSqlFormatLogicalOperatorNewlineBefore.TabIndex = 5;
            this.chkSqlFormatLogicalOperatorNewlineBefore.Text = "Logical operators at the start of a new line";
            this.chkSqlFormatLogicalOperatorNewlineBefore.UseVisualStyleBackColor = true;
            // 
            // chkSqlFormatIndentLists
            // 
            this.chkSqlFormatIndentLists.AutoSize = true;
            this.chkSqlFormatIndentLists.Checked = true;
            this.chkSqlFormatIndentLists.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSqlFormatIndentLists.Location = new System.Drawing.Point(9, 219);
            this.chkSqlFormatIndentLists.Name = "chkSqlFormatIndentLists";
            this.chkSqlFormatIndentLists.Size = new System.Drawing.Size(200, 17);
            this.chkSqlFormatIndentLists.TabIndex = 11;
            this.chkSqlFormatIndentLists.Text = "Indent IN/VALUES lists and CASE";
            this.chkSqlFormatIndentLists.UseVisualStyleBackColor = true;
            // 
            // lblSqlFormatCommaPosition
            // 
            this.lblSqlFormatCommaPosition.AutoSize = true;
            this.lblSqlFormatCommaPosition.Location = new System.Drawing.Point(6, 51);
            this.lblSqlFormatCommaPosition.Name = "lblSqlFormatCommaPosition";
            this.lblSqlFormatCommaPosition.Size = new System.Drawing.Size(81, 13);
            this.lblSqlFormatCommaPosition.TabIndex = 0;
            this.lblSqlFormatCommaPosition.Text = "Comma position";
            // 
            // cmbSqlFormatCommaPosition
            // 
            this.cmbSqlFormatCommaPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSqlFormatCommaPosition.FormattingEnabled = true;
            this.cmbSqlFormatCommaPosition.Items.AddRange(new object[] {
            "After",
            "Before (space)",
            "Before (no space)"});
            this.cmbSqlFormatCommaPosition.Location = new System.Drawing.Point(119, 48);
            this.cmbSqlFormatCommaPosition.Name = "cmbSqlFormatCommaPosition";
            this.cmbSqlFormatCommaPosition.Size = new System.Drawing.Size(132, 21);
            this.cmbSqlFormatCommaPosition.TabIndex = 4;
            this.cmbSqlFormatCommaPosition.SelectedIndex = 2;
            // 
            // lblSqlFormatOnPosition
            // 
            this.lblSqlFormatOnPosition.AutoSize = true;
            this.lblSqlFormatOnPosition.Location = new System.Drawing.Point(6, 89);
            this.lblSqlFormatOnPosition.Name = "lblSqlFormatOnPosition";
            this.lblSqlFormatOnPosition.Size = new System.Drawing.Size(62, 13);
            this.lblSqlFormatOnPosition.TabIndex = 0;
            this.lblSqlFormatOnPosition.Text = "ON position";
            // 
            // cmbSqlFormatOnPosition
            // 
            this.cmbSqlFormatOnPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSqlFormatOnPosition.FormattingEnabled = true;
            this.cmbSqlFormatOnPosition.Items.AddRange(new object[] {
            "Same line",
            "Next line"});
            this.cmbSqlFormatOnPosition.Location = new System.Drawing.Point(119, 86);
            this.cmbSqlFormatOnPosition.Name = "cmbSqlFormatOnPosition";
            this.cmbSqlFormatOnPosition.Size = new System.Drawing.Size(132, 21);
            this.cmbSqlFormatOnPosition.TabIndex = 5;
            this.cmbSqlFormatOnPosition.SelectedIndex = 0;
            // 
            // lblSqlFormatTablePosition
            // 
            this.lblSqlFormatTablePosition.AutoSize = true;
            this.lblSqlFormatTablePosition.Location = new System.Drawing.Point(258, 51);
            this.lblSqlFormatTablePosition.Name = "lblSqlFormatTablePosition";
            this.lblSqlFormatTablePosition.Size = new System.Drawing.Size(73, 13);
            this.lblSqlFormatTablePosition.TabIndex = 0;
            this.lblSqlFormatTablePosition.Text = "Table position";
            // 
            // cmbSqlFormatTablePosition
            // 
            this.cmbSqlFormatTablePosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSqlFormatTablePosition.FormattingEnabled = true;
            this.cmbSqlFormatTablePosition.Items.AddRange(new object[] {
            "Same line",
            "Next line"});
            this.cmbSqlFormatTablePosition.Location = new System.Drawing.Point(370, 43);
            this.cmbSqlFormatTablePosition.Name = "cmbSqlFormatTablePosition";
            this.cmbSqlFormatTablePosition.Size = new System.Drawing.Size(132, 21);
            this.cmbSqlFormatTablePosition.TabIndex = 6;
            this.cmbSqlFormatTablePosition.SelectedIndex = 1;
            // 
            // lblSqlFormatJoinIndentation
            // 
            this.lblSqlFormatJoinIndentation.AutoSize = true;
            this.lblSqlFormatJoinIndentation.Location = new System.Drawing.Point(258, 89);
            this.lblSqlFormatJoinIndentation.Name = "lblSqlFormatJoinIndentation";
            this.lblSqlFormatJoinIndentation.Size = new System.Drawing.Size(81, 13);
            this.lblSqlFormatJoinIndentation.TabIndex = 0;
            this.lblSqlFormatJoinIndentation.Text = "Join indentation";
            // 
            // numSqlFormatJoinIndentation
            // 
            this.numSqlFormatJoinIndentation.Location = new System.Drawing.Point(370, 87);
            this.numSqlFormatJoinIndentation.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numSqlFormatJoinIndentation.Name = "numSqlFormatJoinIndentation";
            this.numSqlFormatJoinIndentation.Size = new System.Drawing.Size(70, 20);
            this.numSqlFormatJoinIndentation.TabIndex = 7;
            // 
            // chkSqlFormatIndentJoinConditions
            // 
            this.chkSqlFormatIndentJoinConditions.AutoSize = true;
            this.chkSqlFormatIndentJoinConditions.Checked = true;
            this.chkSqlFormatIndentJoinConditions.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSqlFormatIndentJoinConditions.Location = new System.Drawing.Point(9, 123);
            this.chkSqlFormatIndentJoinConditions.Name = "chkSqlFormatIndentJoinConditions";
            this.chkSqlFormatIndentJoinConditions.Size = new System.Drawing.Size(126, 17);
            this.chkSqlFormatIndentJoinConditions.TabIndex = 10;
            this.chkSqlFormatIndentJoinConditions.Text = "Indent join conditions";
            this.chkSqlFormatIndentJoinConditions.UseVisualStyleBackColor = true;
            // 
            // lblSqlFormatLinesBetweenQueries
            // 
            this.lblSqlFormatLinesBetweenQueries.AutoSize = true;
            this.lblSqlFormatLinesBetweenQueries.Location = new System.Drawing.Point(258, 188);
            this.lblSqlFormatLinesBetweenQueries.Name = "lblSqlFormatLinesBetweenQueries";
            this.lblSqlFormatLinesBetweenQueries.Size = new System.Drawing.Size(93, 13);
            this.lblSqlFormatLinesBetweenQueries.TabIndex = 0;
            this.lblSqlFormatLinesBetweenQueries.Text = "Lines after queries";
            // 
            // numSqlFormatLinesBetweenQueries
            // 
            this.numSqlFormatLinesBetweenQueries.Location = new System.Drawing.Point(370, 186);
            this.numSqlFormatLinesBetweenQueries.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numSqlFormatLinesBetweenQueries.Name = "numSqlFormatLinesBetweenQueries";
            this.numSqlFormatLinesBetweenQueries.Size = new System.Drawing.Size(70, 20);
            this.numSqlFormatLinesBetweenQueries.TabIndex = 8;
            this.numSqlFormatLinesBetweenQueries.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(376, 326);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(468, 326);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // importFile
            // 
            this.importFile.FileName = "ServerList";
            // 
            // OptionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(560, 360);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.tabOption);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.OptionForm_Load);
            this.tabOption.ResumeLayout(false);
            this.tpgScripting.ResumeLayout(false);
            this.tpgScripting.PerformLayout();
            this.tpgQueries.ResumeLayout(false);
            this.tpgQueries.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOpenTopRows)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numConnectionTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numExecutionTimeout)).EndInit();
            this.tpgTools.ResumeLayout(false);
            this.grbServerActivity.ResumeLayout(false);
            this.grbServerActivity.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTopQueriesWithin)).EndInit();
            this.tpgServers.ResumeLayout(false);
            this.tpgServers.PerformLayout();
            this.tpgSqlFormatting.ResumeLayout(false);
            this.tpgSqlFormatting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSqlFormatIndentSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSqlFormatJoinIndentation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSqlFormatLinesBetweenQueries)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabOption;
        private System.Windows.Forms.TabPage tpgScripting;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkAddDropCommand;
        private System.Windows.Forms.CheckBox chkSingleFileExport;
        private System.Windows.Forms.TabPage tpgQueries;
        private System.Windows.Forms.NumericUpDown numConnectionTimeout;
        private System.Windows.Forms.Label lblConnectionTimeout;
        private System.Windows.Forms.NumericUpDown numExecutionTimeout;
        private System.Windows.Forms.Label lblExecutionTimeout;
        private System.Windows.Forms.TabPage tpgTools;
        private System.Windows.Forms.GroupBox grbServerActivity;
        private System.Windows.Forms.CheckBox chkHideAppQueries;
        private System.Windows.Forms.NumericUpDown numTopQueriesWithin;
        private System.Windows.Forms.Label lblTopQry;
        private System.Windows.Forms.Label lblTopQryHours;
        private System.Windows.Forms.NumericUpDown numOpenTopRows;
        private System.Windows.Forms.Label lblOpenTable;
        private System.Windows.Forms.CheckBox chkOpenWithNoLock;
        private System.Windows.Forms.Label lblRows;
        private System.Windows.Forms.CheckBox chkHideSystemDbs;
        private System.Windows.Forms.CheckBox chkGetLockedObjectName;
        private System.Windows.Forms.CheckBox chkIncludeIndexes;
        private System.Windows.Forms.TabPage tpgServers;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.ListBox lstServer;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.TabPage tpgSqlFormatting;
        private System.Windows.Forms.Label lblSqlFormatKeywordCase;
        private System.Windows.Forms.ComboBox cmbSqlFormatKeywordCase;
        private System.Windows.Forms.Label lblSqlFormatIdentifierQuoting;
        private System.Windows.Forms.ComboBox cmbSqlFormatIdentifierQuoting;
        private System.Windows.Forms.Label lblSqlFormatIndentSize;
        private System.Windows.Forms.NumericUpDown numSqlFormatIndentSize;
        private System.Windows.Forms.CheckBox chkSqlFormatUseTabs;
        private System.Windows.Forms.CheckBox chkSqlFormatLogicalOperatorNewlineBefore;
        private System.Windows.Forms.CheckBox chkSqlFormatIndentLists;
        private System.Windows.Forms.Label lblSqlFormatCommaPosition;
        private System.Windows.Forms.ComboBox cmbSqlFormatCommaPosition;
        private System.Windows.Forms.Label lblSqlFormatOnPosition;
        private System.Windows.Forms.ComboBox cmbSqlFormatOnPosition;
        private System.Windows.Forms.Label lblSqlFormatTablePosition;
        private System.Windows.Forms.ComboBox cmbSqlFormatTablePosition;
        private System.Windows.Forms.Label lblSqlFormatJoinIndentation;
        private System.Windows.Forms.NumericUpDown numSqlFormatJoinIndentation;
        private System.Windows.Forms.CheckBox chkSqlFormatIndentJoinConditions;
        private System.Windows.Forms.Label lblSqlFormatLinesBetweenQueries;
        private System.Windows.Forms.NumericUpDown numSqlFormatLinesBetweenQueries;
        private System.Windows.Forms.SaveFileDialog exportFileBrowser;
        private System.Windows.Forms.OpenFileDialog importFile;
    }
}
