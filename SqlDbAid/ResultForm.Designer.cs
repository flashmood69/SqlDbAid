namespace SqlDbAid
{
    partial class ResultForm
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
            this.dgvResult = new System.Windows.Forms.DataGridView();
            this.datagridMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyWithHeadersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resultStatusStrip = new System.Windows.Forms.StatusStrip();
            this.lblPosition = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblRow = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnExport = new System.Windows.Forms.Button();
            this.exportFileBrowser = new System.Windows.Forms.SaveFileDialog();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnAbort = new System.Windows.Forms.Button();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.tabQuery = new System.Windows.Forms.TabControl();
            this.tpgQuery = new System.Windows.Forms.TabPage();
            this.txtQuery = new System.Windows.Forms.RichTextBox();
            this.tpgMessages = new System.Windows.Forms.TabPage();
            this.txtMessages = new System.Windows.Forms.RichTextBox();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).BeginInit();
            this.datagridMenu.SuspendLayout();
            this.resultStatusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.tabQuery.SuspendLayout();
            this.tpgQuery.SuspendLayout();
            this.tpgMessages.SuspendLayout();
            this.mainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvResult
            // 
            this.dgvResult.AllowUserToAddRows = false;
            this.dgvResult.AllowUserToDeleteRows = false;
            this.dgvResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResult.Location = new System.Drawing.Point(3, 3);
            this.dgvResult.Name = "dgvResult";
            this.dgvResult.ReadOnly = true;
            this.dgvResult.RowTemplate.Height = 20;
            this.dgvResult.Size = new System.Drawing.Size(562, 134);
            this.dgvResult.TabIndex = 3;
            this.dgvResult.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvResult_CellFormatting);
            this.dgvResult.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvResult_CellMouseDoubleClick);
            this.dgvResult.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgvResult_ColumnAdded);
            this.dgvResult.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvResult_RowPostPaint);
            this.dgvResult.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvResult_MouseDown);
            // 
            // datagridMenu
            // 
            this.datagridMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.copyWithHeadersToolStripMenuItem,
            this.viewCodeToolStripMenuItem});
            this.datagridMenu.Name = "datagridMenu";
            this.datagridMenu.Size = new System.Drawing.Size(175, 70);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // copyWithHeadersToolStripMenuItem
            // 
            this.copyWithHeadersToolStripMenuItem.Name = "copyWithHeadersToolStripMenuItem";
            this.copyWithHeadersToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.copyWithHeadersToolStripMenuItem.Text = "Copy with Headers";
            this.copyWithHeadersToolStripMenuItem.Click += new System.EventHandler(this.copyWithHeadersToolStripMenuItem_Click);
            // 
            // viewCodeToolStripMenuItem
            // 
            this.viewCodeToolStripMenuItem.Name = "viewCodeToolStripMenuItem";
            this.viewCodeToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.viewCodeToolStripMenuItem.Text = "View Script";
            this.viewCodeToolStripMenuItem.Visible = false;
            this.viewCodeToolStripMenuItem.Click += new System.EventHandler(this.viewCodeToolStripMenuItem_Click);
            // 
            // resultStatusStrip
            // 
            this.resultStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblPosition,
            this.lblRow,
            this.lblCount});
            this.resultStatusStrip.Location = new System.Drawing.Point(0, 349);
            this.resultStatusStrip.Name = "resultStatusStrip";
            this.resultStatusStrip.Size = new System.Drawing.Size(592, 24);
            this.resultStatusStrip.TabIndex = 5;
            this.resultStatusStrip.Text = "statusStrip1";
            // 
            // lblPosition
            // 
            this.lblPosition.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size(97, 19);
            this.lblPosition.Text = "Line 1 Column 1";
            // 
            // lblRow
            // 
            this.lblRow.Name = "lblRow";
            this.lblRow.Size = new System.Drawing.Size(66, 19);
            this.lblRow.Text = "Row Count";
            // 
            // lblCount
            // 
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(13, 19);
            this.lblCount.Text = "0";
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Enabled = false;
            this.btnExport.Location = new System.Drawing.Point(340, 325);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 2;
            this.btnExport.Text = "&Script";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Visible = false;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // exportFileBrowser
            // 
            this.exportFileBrowser.DefaultExt = "sql";
            // 
            // btnRun
            // 
            this.btnRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRun.Location = new System.Drawing.Point(421, 325);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 0;
            this.btnRun.Text = "&Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnAbort
            // 
            this.btnAbort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAbort.Enabled = false;
            this.btnAbort.Location = new System.Drawing.Point(502, 325);
            this.btnAbort.Name = "btnAbort";
            this.btnAbort.Size = new System.Drawing.Size(75, 23);
            this.btnAbort.TabIndex = 1;
            this.btnAbort.Text = "&Cancel";
            this.btnAbort.UseVisualStyleBackColor = true;
            this.btnAbort.Click += new System.EventHandler(this.btnAbort_Click);
            // 
            // splitMain
            // 
            this.splitMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitMain.Location = new System.Drawing.Point(12, 27);
            this.splitMain.Name = "splitMain";
            this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.tabQuery);
            this.splitMain.Panel1MinSize = 62;
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.dgvResult);
            this.splitMain.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitMain.Panel2MinSize = 62;
            this.splitMain.Size = new System.Drawing.Size(568, 292);
            this.splitMain.SplitterDistance = 133;
            this.splitMain.TabIndex = 4;
            // 
            // tabQuery
            // 
            this.tabQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabQuery.Controls.Add(this.tpgQuery);
            this.tabQuery.Controls.Add(this.tpgMessages);
            this.tabQuery.Location = new System.Drawing.Point(3, 3);
            this.tabQuery.Name = "tabQuery";
            this.tabQuery.SelectedIndex = 0;
            this.tabQuery.Size = new System.Drawing.Size(562, 131);
            this.tabQuery.TabIndex = 5;
            // 
            // tpgQuery
            // 
            this.tpgQuery.Controls.Add(this.txtQuery);
            this.tpgQuery.Location = new System.Drawing.Point(4, 22);
            this.tpgQuery.Name = "tpgQuery";
            this.tpgQuery.Padding = new System.Windows.Forms.Padding(3);
            this.tpgQuery.Size = new System.Drawing.Size(554, 105);
            this.tpgQuery.TabIndex = 0;
            this.tpgQuery.Text = "Query";
            this.tpgQuery.UseVisualStyleBackColor = true;
            // 
            // txtQuery
            // 
            this.txtQuery.AcceptsTab = true;
            this.txtQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtQuery.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtQuery.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQuery.Location = new System.Drawing.Point(0, 0);
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.Size = new System.Drawing.Size(554, 105);
            this.txtQuery.TabIndex = 4;
            this.txtQuery.Text = "";
            this.txtQuery.WordWrap = false;
            this.txtQuery.SelectionChanged += new System.EventHandler(this.txtQuery_SelectionChanged);
            this.txtQuery.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtQuery_KeyDown);
            // 
            // tpgMessages
            // 
            this.tpgMessages.Controls.Add(this.txtMessages);
            this.tpgMessages.Location = new System.Drawing.Point(4, 22);
            this.tpgMessages.Name = "tpgMessages";
            this.tpgMessages.Padding = new System.Windows.Forms.Padding(3);
            this.tpgMessages.Size = new System.Drawing.Size(554, 105);
            this.tpgMessages.TabIndex = 1;
            this.tpgMessages.Text = "Messages";
            this.tpgMessages.UseVisualStyleBackColor = true;
            // 
            // txtMessages
            // 
            this.txtMessages.AcceptsTab = true;
            this.txtMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMessages.BackColor = System.Drawing.SystemColors.Window;
            this.txtMessages.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMessages.Location = new System.Drawing.Point(0, 0);
            this.txtMessages.Name = "txtMessages";
            this.txtMessages.ReadOnly = true;
            this.txtMessages.Size = new System.Drawing.Size(554, 105);
            this.txtMessages.TabIndex = 4;
            this.txtMessages.Text = "";
            this.txtMessages.WordWrap = false;
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.mainMenu.Size = new System.Drawing.Size(592, 24);
            this.mainMenu.TabIndex = 6;
            this.mainMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveAsToolStripMenuItem.Text = "Save &As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.closeToolStripMenuItem.Text = "&Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // ResultForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 373);
            this.Controls.Add(this.resultStatusStrip);
            this.Controls.Add(this.mainMenu);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.btnAbort);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnRun);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.mainMenu;
            this.MinimumSize = new System.Drawing.Size(350, 250);
            this.Name = "ResultForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ResultForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ResultForm_FormClosing);
            this.Load += new System.EventHandler(this.ResultForm_Load);
            this.Shown += new System.EventHandler(this.ResultForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).EndInit();
            this.datagridMenu.ResumeLayout(false);
            this.resultStatusStrip.ResumeLayout(false);
            this.resultStatusStrip.PerformLayout();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.tabQuery.ResumeLayout(false);
            this.tpgQuery.ResumeLayout(false);
            this.tpgMessages.ResumeLayout(false);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvResult;
        private System.Windows.Forms.ContextMenuStrip datagridMenu;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.StatusStrip resultStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblRow;
        private System.Windows.Forms.ToolStripStatusLabel lblCount;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.SaveFileDialog exportFileBrowser;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnAbort;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.ToolStripMenuItem copyWithHeadersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewCodeToolStripMenuItem;
        private System.Windows.Forms.TabControl tabQuery;
        private System.Windows.Forms.TabPage tpgQuery;
        private System.Windows.Forms.TabPage tpgMessages;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.RichTextBox txtQuery;
        private System.Windows.Forms.RichTextBox txtMessages;
        private System.Windows.Forms.ToolStripStatusLabel lblPosition;
    }
}