namespace SqlDbAid
{
    partial class CompareForm
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
            this.dgvResult = new System.Windows.Forms.DataGridView();
            this.datagridMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyWithHeadersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resultStatusStrip = new System.Windows.Forms.StatusStrip();
            this.lblObjectCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.cmpSql = new SqlDbAid.CompareBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.txtLeftFile = new System.Windows.Forms.TextBox();
            this.btnLeftFile = new System.Windows.Forms.Button();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.btnRightFile = new System.Windows.Forms.Button();
            this.txtRightFile = new System.Windows.Forms.TextBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tltCompare = new System.Windows.Forms.ToolTip(this.components);
            this.btnCompare = new System.Windows.Forms.Button();
            this.chkIgnoreCase = new System.Windows.Forms.CheckBox();
            this.chkIgnoreEmptyLines = new System.Windows.Forms.CheckBox();
            this.chkHideMatches = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).BeginInit();
            this.datagridMenu.SuspendLayout();
            this.resultStatusStrip.SuspendLayout();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvResult
            // 
            this.dgvResult.AllowUserToAddRows = false;
            this.dgvResult.AllowUserToDeleteRows = false;
            this.dgvResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvResult.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvResult.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvResult.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvResult.Location = new System.Drawing.Point(7, 3);
            this.dgvResult.Name = "dgvResult";
            this.dgvResult.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvResult.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvResult.Size = new System.Drawing.Size(579, 100);
            this.dgvResult.TabIndex = 3;
            this.dgvResult.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvResult_CellFormatting);
            this.dgvResult.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvResult_MouseDown);
            this.dgvResult.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvResult_MouseUp);
            // 
            // datagridMenu
            // 
            this.datagridMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.copyWithHeadersToolStripMenuItem});
            this.datagridMenu.Name = "datagridMenu";
            this.datagridMenu.Size = new System.Drawing.Size(175, 48);
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
            // resultStatusStrip
            // 
            this.resultStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblObjectCount,
            this.lblCount});
            this.resultStatusStrip.Location = new System.Drawing.Point(0, 351);
            this.resultStatusStrip.Name = "resultStatusStrip";
            this.resultStatusStrip.Size = new System.Drawing.Size(592, 22);
            this.resultStatusStrip.TabIndex = 5;
            this.resultStatusStrip.Text = "statusStrip1";
            // 
            // lblObjectCount
            // 
            this.lblObjectCount.Name = "lblObjectCount";
            this.lblObjectCount.Size = new System.Drawing.Size(78, 17);
            this.lblObjectCount.Text = "Object Count";
            // 
            // lblCount
            // 
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(13, 17);
            this.lblCount.Text = "0";
            // 
            // splitMain
            // 
            this.splitMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitMain.Location = new System.Drawing.Point(0, 3);
            this.splitMain.Name = "splitMain";
            this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.tableLayoutPanel1);
            this.splitMain.Panel1MinSize = 62;
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.dgvResult);
            this.splitMain.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitMain.Panel2MinSize = 62;
            this.splitMain.Size = new System.Drawing.Size(592, 315);
            this.splitMain.SplitterDistance = 194;
            this.splitMain.TabIndex = 6;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.cmpSql, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(592, 194);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // cmpSql
            // 
            this.cmpSql.CaseSensitive = false;
            this.cmpSql.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmpSql.LeftText = "";
            this.cmpSql.Location = new System.Drawing.Point(3, 36);
            this.cmpSql.Name = "cmpSql";
            this.cmpSql.RightText = "";
            this.cmpSql.Size = new System.Drawing.Size(586, 155);
            this.cmpSql.TabIndex = 7;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel4, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(586, 27);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel3.Controls.Add(this.txtLeftFile, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnLeftFile, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(287, 21);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // txtLeftFile
            // 
            this.txtLeftFile.BackColor = System.Drawing.SystemColors.Window;
            this.txtLeftFile.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtLeftFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLeftFile.Location = new System.Drawing.Point(0, 0);
            this.txtLeftFile.Margin = new System.Windows.Forms.Padding(0);
            this.txtLeftFile.Name = "txtLeftFile";
            this.txtLeftFile.ReadOnly = true;
            this.txtLeftFile.Size = new System.Drawing.Size(247, 20);
            this.txtLeftFile.TabIndex = 8;
            // 
            // btnLeftFile
            // 
            this.btnLeftFile.Location = new System.Drawing.Point(247, 0);
            this.btnLeftFile.Margin = new System.Windows.Forms.Padding(0);
            this.btnLeftFile.Name = "btnLeftFile";
            this.btnLeftFile.Size = new System.Drawing.Size(40, 20);
            this.btnLeftFile.TabIndex = 0;
            this.btnLeftFile.Text = "...";
            this.btnLeftFile.UseVisualStyleBackColor = true;
            this.btnLeftFile.Click += new System.EventHandler(this.btnLeftFile_Click);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel4.Controls.Add(this.btnRightFile, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.txtRightFile, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(296, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(287, 21);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // btnRightFile
            // 
            this.btnRightFile.Location = new System.Drawing.Point(247, 0);
            this.btnRightFile.Margin = new System.Windows.Forms.Padding(0);
            this.btnRightFile.Name = "btnRightFile";
            this.btnRightFile.Size = new System.Drawing.Size(40, 20);
            this.btnRightFile.TabIndex = 1;
            this.btnRightFile.Text = "...";
            this.btnRightFile.UseVisualStyleBackColor = true;
            this.btnRightFile.Click += new System.EventHandler(this.btnRightFile_Click);
            // 
            // txtRightFile
            // 
            this.txtRightFile.BackColor = System.Drawing.SystemColors.Window;
            this.txtRightFile.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtRightFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRightFile.Location = new System.Drawing.Point(0, 0);
            this.txtRightFile.Margin = new System.Windows.Forms.Padding(0);
            this.txtRightFile.Name = "txtRightFile";
            this.txtRightFile.ReadOnly = true;
            this.txtRightFile.Size = new System.Drawing.Size(247, 20);
            this.txtRightFile.TabIndex = 9;
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Compare files|*.cmp|All files|*.*";
            // 
            // btnCompare
            // 
            this.btnCompare.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCompare.Enabled = false;
            this.btnCompare.Location = new System.Drawing.Point(511, 321);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(75, 23);
            this.btnCompare.TabIndex = 2;
            this.btnCompare.Text = "&Compare";
            this.btnCompare.UseVisualStyleBackColor = true;
            this.btnCompare.Click += new System.EventHandler(this.btnCompare_Click);
            // 
            // chkIgnoreCase
            // 
            this.chkIgnoreCase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkIgnoreCase.AutoSize = true;
            this.chkIgnoreCase.Checked = true;
            this.chkIgnoreCase.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIgnoreCase.Location = new System.Drawing.Point(7, 325);
            this.chkIgnoreCase.Name = "chkIgnoreCase";
            this.chkIgnoreCase.Size = new System.Drawing.Size(83, 17);
            this.chkIgnoreCase.TabIndex = 4;
            this.chkIgnoreCase.Text = "Ignore Case";
            this.chkIgnoreCase.UseVisualStyleBackColor = true;
            // 
            // chkIgnoreEmptyLines
            // 
            this.chkIgnoreEmptyLines.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkIgnoreEmptyLines.AutoSize = true;
            this.chkIgnoreEmptyLines.Checked = true;
            this.chkIgnoreEmptyLines.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIgnoreEmptyLines.Location = new System.Drawing.Point(96, 325);
            this.chkIgnoreEmptyLines.Name = "chkIgnoreEmptyLines";
            this.chkIgnoreEmptyLines.Size = new System.Drawing.Size(142, 17);
            this.chkIgnoreEmptyLines.TabIndex = 5;
            this.chkIgnoreEmptyLines.Text = "Ignore Empty Lines / CR";
            this.chkIgnoreEmptyLines.UseVisualStyleBackColor = true;
            // 
            // chkHideMatches
            // 
            this.chkHideMatches.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkHideMatches.AutoSize = true;
            this.chkHideMatches.Checked = true;
            this.chkHideMatches.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHideMatches.Location = new System.Drawing.Point(244, 325);
            this.chkHideMatches.Name = "chkHideMatches";
            this.chkHideMatches.Size = new System.Drawing.Size(92, 17);
            this.chkHideMatches.TabIndex = 6;
            this.chkHideMatches.Text = "Hide Matches";
            this.chkHideMatches.UseVisualStyleBackColor = true;
            // 
            // CompareForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 373);
            this.Controls.Add(this.chkIgnoreCase);
            this.Controls.Add(this.chkIgnoreEmptyLines);
            this.Controls.Add(this.resultStatusStrip);
            this.Controls.Add(this.chkHideMatches);
            this.Controls.Add(this.btnCompare);
            this.Controls.Add(this.splitMain);
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "CompareForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Compare";
            this.Load += new System.EventHandler(this.CompareForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResult)).EndInit();
            this.datagridMenu.ResumeLayout(false);
            this.resultStatusStrip.ResumeLayout(false);
            this.resultStatusStrip.PerformLayout();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            this.splitMain.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvResult;
        private System.Windows.Forms.ContextMenuStrip datagridMenu;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.StatusStrip resultStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblObjectCount;
        private System.Windows.Forms.ToolStripStatusLabel lblCount;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.ToolStripMenuItem copyWithHeadersToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private CompareBox cmpSql;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TextBox txtLeftFile;
        private System.Windows.Forms.Button btnLeftFile;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button btnRightFile;
        private System.Windows.Forms.TextBox txtRightFile;
        private System.Windows.Forms.ToolTip tltCompare;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.CheckBox chkIgnoreCase;
        private System.Windows.Forms.CheckBox chkIgnoreEmptyLines;
        private System.Windows.Forms.CheckBox chkHideMatches;
    }
}