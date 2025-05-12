namespace SqlDbAid
{
    partial class ExportForm
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
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.lblFieldDelimiter = new System.Windows.Forms.Label();
            this.txtFieldDelimiter = new System.Windows.Forms.TextBox();
            this.txtTextQualifier = new System.Windows.Forms.TextBox();
            this.lblTextQualifier = new System.Windows.Forms.Label();
            this.txtRowDelimiter = new System.Windows.Forms.TextBox();
            this.lblRowDelimiter = new System.Windows.Forms.Label();
            this.chkIncludeColumnNames = new System.Windows.Forms.CheckBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.exportFileBrowser = new System.Windows.Forms.SaveFileDialog();
            this.chkScriptInsert = new System.Windows.Forms.CheckBox();
            this.lblSource = new System.Windows.Forms.Label();
            this.lblDestination = new System.Windows.Forms.Label();
            this.lstSource = new System.Windows.Forms.ListView();
            this.ColName = new System.Windows.Forms.ColumnHeader();
            this.ColType = new System.Windows.Forms.ColumnHeader();
            this.ColSize = new System.Windows.Forms.ColumnHeader();
            this.ColInfo = new System.Windows.Forms.ColumnHeader();
            this.ColCollation = new System.Windows.Forms.ColumnHeader();
            this.lstDestination = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.lblEscape = new System.Windows.Forms.Label();
            this.txtEscape = new System.Windows.Forms.TextBox();
            this.btnAbort = new System.Windows.Forms.Button();
            this.exportStatusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblRowCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.chkUseLocalSettings = new System.Windows.Forms.CheckBox();
            this.chkUnicode = new System.Windows.Forms.CheckBox();
            this.exportStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(218, 96);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(48, 23);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = ">";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(218, 125);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(48, 23);
            this.btnRemove.TabIndex = 4;
            this.btnRemove.Text = "<";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnUp
            // 
            this.btnUp.Location = new System.Drawing.Point(218, 26);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(48, 23);
            this.btnUp.TabIndex = 2;
            this.btnUp.Text = "Up";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.Location = new System.Drawing.Point(218, 202);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(48, 23);
            this.btnDown.TabIndex = 5;
            this.btnDown.Text = "Down";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // lblFieldDelimiter
            // 
            this.lblFieldDelimiter.AutoSize = true;
            this.lblFieldDelimiter.Location = new System.Drawing.Point(10, 285);
            this.lblFieldDelimiter.Name = "lblFieldDelimiter";
            this.lblFieldDelimiter.Size = new System.Drawing.Size(72, 13);
            this.lblFieldDelimiter.TabIndex = 7;
            this.lblFieldDelimiter.Text = "Field Delimiter";
            // 
            // txtFieldDelimiter
            // 
            this.txtFieldDelimiter.Location = new System.Drawing.Point(100, 282);
            this.txtFieldDelimiter.Name = "txtFieldDelimiter";
            this.txtFieldDelimiter.Size = new System.Drawing.Size(112, 20);
            this.txtFieldDelimiter.TabIndex = 8;
            this.txtFieldDelimiter.Text = "\\t";
            // 
            // txtTextQualifier
            // 
            this.txtTextQualifier.Location = new System.Drawing.Point(100, 231);
            this.txtTextQualifier.Name = "txtTextQualifier";
            this.txtTextQualifier.Size = new System.Drawing.Size(112, 20);
            this.txtTextQualifier.TabIndex = 6;
            // 
            // lblTextQualifier
            // 
            this.lblTextQualifier.AutoSize = true;
            this.lblTextQualifier.Location = new System.Drawing.Point(10, 234);
            this.lblTextQualifier.Name = "lblTextQualifier";
            this.lblTextQualifier.Size = new System.Drawing.Size(69, 13);
            this.lblTextQualifier.TabIndex = 9;
            this.lblTextQualifier.Text = "Text Qualifier";
            // 
            // txtRowDelimiter
            // 
            this.txtRowDelimiter.Location = new System.Drawing.Point(100, 308);
            this.txtRowDelimiter.Name = "txtRowDelimiter";
            this.txtRowDelimiter.Size = new System.Drawing.Size(112, 20);
            this.txtRowDelimiter.TabIndex = 9;
            this.txtRowDelimiter.Text = "\\r\\n";
            // 
            // lblRowDelimiter
            // 
            this.lblRowDelimiter.AutoSize = true;
            this.lblRowDelimiter.Location = new System.Drawing.Point(10, 311);
            this.lblRowDelimiter.Name = "lblRowDelimiter";
            this.lblRowDelimiter.Size = new System.Drawing.Size(72, 13);
            this.lblRowDelimiter.TabIndex = 11;
            this.lblRowDelimiter.Text = "Row Delimiter";
            // 
            // chkIncludeColumnNames
            // 
            this.chkIncludeColumnNames.AutoSize = true;
            this.chkIncludeColumnNames.Checked = true;
            this.chkIncludeColumnNames.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIncludeColumnNames.Location = new System.Drawing.Point(272, 231);
            this.chkIncludeColumnNames.Name = "chkIncludeColumnNames";
            this.chkIncludeColumnNames.Size = new System.Drawing.Size(135, 17);
            this.chkIncludeColumnNames.TabIndex = 10;
            this.chkIncludeColumnNames.Text = "Include Column Names";
            this.chkIncludeColumnNames.UseVisualStyleBackColor = true;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(306, 304);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 12;
            this.btnExport.Text = "&Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // exportFileBrowser
            // 
            this.exportFileBrowser.DefaultExt = "txt";
            this.exportFileBrowser.Filter = "Txt files|*.txt|All files|*.*";
            // 
            // chkScriptInsert
            // 
            this.chkScriptInsert.AutoSize = true;
            this.chkScriptInsert.Location = new System.Drawing.Point(273, 256);
            this.chkScriptInsert.Name = "chkScriptInsert";
            this.chkScriptInsert.Size = new System.Drawing.Size(82, 17);
            this.chkScriptInsert.TabIndex = 11;
            this.chkScriptInsert.Text = "Script Insert";
            this.chkScriptInsert.UseVisualStyleBackColor = true;
            this.chkScriptInsert.CheckedChanged += new System.EventHandler(this.chkScriptInsert_CheckedChanged);
            // 
            // lblSource
            // 
            this.lblSource.AutoSize = true;
            this.lblSource.Location = new System.Drawing.Point(10, 10);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(93, 13);
            this.lblSource.TabIndex = 13;
            this.lblSource.Text = "Available Columns";
            // 
            // lblDestination
            // 
            this.lblDestination.AutoSize = true;
            this.lblDestination.Location = new System.Drawing.Point(270, 10);
            this.lblDestination.Name = "lblDestination";
            this.lblDestination.Size = new System.Drawing.Size(92, 13);
            this.lblDestination.TabIndex = 14;
            this.lblDestination.Text = "Selected Columns";
            // 
            // lstSource
            // 
            this.lstSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstSource.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColName,
            this.ColType,
            this.ColSize,
            this.ColInfo,
            this.ColCollation});
            this.lstSource.Location = new System.Drawing.Point(13, 26);
            this.lstSource.Name = "lstSource";
            this.lstSource.Size = new System.Drawing.Size(199, 199);
            this.lstSource.TabIndex = 0;
            this.lstSource.UseCompatibleStateImageBehavior = false;
            this.lstSource.View = System.Windows.Forms.View.Details;
            // 
            // ColName
            // 
            this.ColName.Text = "Name";
            this.ColName.Width = 120;
            // 
            // ColType
            // 
            this.ColType.Text = "Type";
            this.ColType.Width = 80;
            // 
            // ColSize
            // 
            this.ColSize.Text = "Size";
            this.ColSize.Width = 40;
            // 
            // ColInfo
            // 
            this.ColInfo.Text = "Info";
            // 
            // ColCollation
            // 
            this.ColCollation.Text = "Collation";
            this.ColCollation.Width = 140;
            // 
            // lstDestination
            // 
            this.lstDestination.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.lstDestination.Location = new System.Drawing.Point(272, 26);
            this.lstDestination.Name = "lstDestination";
            this.lstDestination.Size = new System.Drawing.Size(199, 199);
            this.lstDestination.TabIndex = 1;
            this.lstDestination.UseCompatibleStateImageBehavior = false;
            this.lstDestination.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 120;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Type";
            this.columnHeader2.Width = 80;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Size";
            this.columnHeader3.Width = 40;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Info";
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Collation";
            this.columnHeader5.Width = 140;
            // 
            // lblEscape
            // 
            this.lblEscape.AutoSize = true;
            this.lblEscape.Location = new System.Drawing.Point(10, 260);
            this.lblEscape.Name = "lblEscape";
            this.lblEscape.Size = new System.Drawing.Size(84, 13);
            this.lblEscape.TabIndex = 15;
            this.lblEscape.Text = "Qualifier Escape";
            // 
            // txtEscape
            // 
            this.txtEscape.Location = new System.Drawing.Point(100, 257);
            this.txtEscape.Name = "txtEscape";
            this.txtEscape.Size = new System.Drawing.Size(112, 20);
            this.txtEscape.TabIndex = 7;
            // 
            // btnAbort
            // 
            this.btnAbort.Enabled = false;
            this.btnAbort.Location = new System.Drawing.Point(397, 304);
            this.btnAbort.Name = "btnAbort";
            this.btnAbort.Size = new System.Drawing.Size(75, 23);
            this.btnAbort.TabIndex = 16;
            this.btnAbort.Text = "&Abort";
            this.btnAbort.UseVisualStyleBackColor = true;
            this.btnAbort.Click += new System.EventHandler(this.btnAbort_Click);
            // 
            // exportStatusStrip
            // 
            this.exportStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.lblRowCount});
            this.exportStatusStrip.Location = new System.Drawing.Point(0, 336);
            this.exportStatusStrip.Name = "exportStatusStrip";
            this.exportStatusStrip.Size = new System.Drawing.Size(484, 22);
            this.exportStatusStrip.TabIndex = 17;
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(38, 17);
            this.lblStatus.Text = "Ready";
            // 
            // lblRowCount
            // 
            this.lblRowCount.Name = "lblRowCount";
            this.lblRowCount.Size = new System.Drawing.Size(0, 17);
            // 
            // chkUseLocalSettings
            // 
            this.chkUseLocalSettings.AutoSize = true;
            this.chkUseLocalSettings.Checked = true;
            this.chkUseLocalSettings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUseLocalSettings.Location = new System.Drawing.Point(273, 281);
            this.chkUseLocalSettings.Name = "chkUseLocalSettings";
            this.chkUseLocalSettings.Size = new System.Drawing.Size(93, 17);
            this.chkUseLocalSettings.TabIndex = 18;
            this.chkUseLocalSettings.Text = "Local Settings";
            this.chkUseLocalSettings.UseVisualStyleBackColor = true;
            // 
            // chkUnicode
            // 
            this.chkUnicode.AutoSize = true;
            this.chkUnicode.Location = new System.Drawing.Point(397, 281);
            this.chkUnicode.Name = "chkUnicode";
            this.chkUnicode.Size = new System.Drawing.Size(66, 17);
            this.chkUnicode.TabIndex = 19;
            this.chkUnicode.Text = "Unicode";
            this.chkUnicode.UseVisualStyleBackColor = true;
            // 
            // ExportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 358);
            this.Controls.Add(this.chkUnicode);
            this.Controls.Add(this.chkUseLocalSettings);
            this.Controls.Add(this.exportStatusStrip);
            this.Controls.Add(this.btnAbort);
            this.Controls.Add(this.txtEscape);
            this.Controls.Add(this.lblEscape);
            this.Controls.Add(this.lstDestination);
            this.Controls.Add(this.lstSource);
            this.Controls.Add(this.lblDestination);
            this.Controls.Add(this.lblSource);
            this.Controls.Add(this.chkScriptInsert);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.chkIncludeColumnNames);
            this.Controls.Add(this.txtRowDelimiter);
            this.Controls.Add(this.lblRowDelimiter);
            this.Controls.Add(this.txtTextQualifier);
            this.Controls.Add(this.lblTextQualifier);
            this.Controls.Add(this.txtFieldDelimiter);
            this.Controls.Add(this.lblFieldDelimiter);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export Data";
            this.Load += new System.EventHandler(this.ExportForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExportForm_FormClosing);
            this.exportStatusStrip.ResumeLayout(false);
            this.exportStatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Label lblFieldDelimiter;
        private System.Windows.Forms.TextBox txtFieldDelimiter;
        private System.Windows.Forms.TextBox txtTextQualifier;
        private System.Windows.Forms.Label lblTextQualifier;
        private System.Windows.Forms.TextBox txtRowDelimiter;
        private System.Windows.Forms.Label lblRowDelimiter;
        private System.Windows.Forms.CheckBox chkIncludeColumnNames;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.SaveFileDialog exportFileBrowser;
        private System.Windows.Forms.CheckBox chkScriptInsert;
        private System.Windows.Forms.Label lblSource;
        private System.Windows.Forms.Label lblDestination;
        private System.Windows.Forms.ListView lstSource;
        private System.Windows.Forms.ColumnHeader ColName;
        private System.Windows.Forms.ColumnHeader ColType;
        private System.Windows.Forms.ColumnHeader ColSize;
        private System.Windows.Forms.ColumnHeader ColInfo;
        private System.Windows.Forms.ColumnHeader ColCollation;
        private System.Windows.Forms.ListView lstDestination;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Label lblEscape;
        private System.Windows.Forms.TextBox txtEscape;
        private System.Windows.Forms.Button btnAbort;
        private System.Windows.Forms.StatusStrip exportStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblRowCount;
        private System.Windows.Forms.CheckBox chkUseLocalSettings;
        private System.Windows.Forms.CheckBox chkUnicode;
    }
}