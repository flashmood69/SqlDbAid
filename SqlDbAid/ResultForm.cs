using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SqlDbAid
{
    public partial class ResultForm : Form
    {
        int mCurrentRow = -1;

        bool mExporting = false;
        bool mClosing = false;

        bool mModified = false;

        bool mAutoExec = true;

        string mAppendTitle = "";

        string mFileName = "";
        string mFileFilter = "Sql files|*.sql|All files|*.*";
        string mExportFileHeader = "";
        string mExportColumn = "";
        string mConnectionString = "";
        string mCommandText = "";
        int mExecutionTimeout = 30;
        bool mAllowExport = true;
        bool mAutosizeOutput = false;
        bool mShowQuery = false;
        string mNullString = "NULL";

        string[] mColumnTypes;

        DataExporter dataExporter;

        public string AppendTitle
        {
            get { return mAppendTitle; }
            set { mAppendTitle = value; }
        }

        public bool AutoExec
        {
            get { return mAutoExec; }
            set { mAutoExec = value; }
        }

        public bool AllowExport
        {
            get { return mAllowExport; }
            set { mAllowExport = value; }
        }

        public bool ShowQuery
        {
            get { return mShowQuery; }
            set { mShowQuery = value; }
        }

        public string ExportFileHeader
        {
            get { return mExportFileHeader; }
            set { mExportFileHeader = value; }
        }

        public string FileName
        {
            get { return mFileName; }
            set { mFileName = value; }
        }

        public string FileFilter
        {
            get { return mFileFilter; }
            set { mFileFilter = value; }
        }

        public string ExportColumn
        {
            get { return mExportColumn; }
            set { mExportColumn = value; }
        }

        public string ConnectionString
        {
            get { return mConnectionString; }
            set { mConnectionString = value; }
        }

        public string CommandText
        {
            get { return mCommandText; }
            set { mCommandText = value; }
        }

        public int ExecutionTimeout
        {
            get { return mExecutionTimeout; }
            set { mExecutionTimeout = value; }
        }

        public bool AutosizeOutput
        {
            get { return mAutosizeOutput; }
            set { mAutosizeOutput = value; }
        }

        public string NullString
        {
            get { return mNullString; }
            set { mNullString = value; }
        }

        private void SetTitle()
        {
            this.Text = string.Format("{0}{1}{2}", (mShowQuery ? mFileName : ""), (mModified && mShowQuery ? "*" : ""), mAppendTitle);
        }

        private void CopyToClipboard(bool IncludeHeaderText)
        {
            int cellCount = dgvResult.GetCellCount(DataGridViewElementStates.Selected);
            if (cellCount > 0)
            {
                if (cellCount > 100000)
                {
                    DialogResult dres = MessageBox.Show(string.Format("{0} cells selected. Do you want to continue?", cellCount), "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (dres == DialogResult.No)
                    {
                        return;
                    }
                }

                if (IncludeHeaderText)
                {
                    dgvResult.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
                    Clipboard.SetDataObject(ClipboardHelper.RemoveFirstTab(dgvResult.GetClipboardContent().GetText()));
                }
                else
                {
                    dgvResult.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
                    Clipboard.SetDataObject(dgvResult.GetClipboardContent());
                }
            }
        }

        private void ViewCode()
        {
            string formText = dgvResult.Rows[mCurrentRow].Cells[mExportColumn].Value.ToString();

            if (!string.IsNullOrEmpty(formText))
            {
                CodeForm code = new CodeForm(formText, "", false);
                code.Icon = this.Icon;
                code.Text = mExportColumn;
                code.Show();
            }
        }

        private void RefreshGrid()
        {
            if (txtQuery.SelectedText.Length > 0)
            {
                mCommandText = txtQuery.SelectedText;
            }
            else
            {
                mCommandText = txtQuery.Text;
            }

            if (string.IsNullOrEmpty(mCommandText.Trim(new char[] { ' ', '\r', '\n', '\t' })))
            {
                return;
            }

            dataExporter.ConnectionString = mConnectionString;
            dataExporter.CommandText = mCommandText;
            dataExporter.ExecutionTimeout = mExecutionTimeout;

            mExporting = true;
            btnExport.Enabled = false;
            btnRun.Enabled = false;
            btnAbort.Enabled = true;
            lblRow.Text = "Running...";
            lblCount.Text = "";

            DataTable dt = new DataTable();

            dgvResult.DataSource = dt;

            dataExporter.RunWorkerAsync();
        }

        private void ExportFile()
        {
            exportFileBrowser.OverwritePrompt = true;
            exportFileBrowser.FileName = mFileName;

            if (exportFileBrowser.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(exportFileBrowser.FileName))
                    {
                        if (mExportFileHeader != "")
                        {
                            sw.WriteLine(mExportFileHeader);
                        }

                        foreach (DataGridViewRow row in dgvResult.Rows)
                        {
                            if (!string.IsNullOrEmpty(row.Cells[mExportColumn].Value.ToString()))
                            {
                                sw.WriteLine(row.Cells[mExportColumn].Value);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void OpenFile()
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                mFileName = Path.GetFileNameWithoutExtension(openFileDialog.FileName);

                try
                {
                    using (StreamReader sr = new StreamReader(openFileDialog.FileName))
                    {
                        txtQuery.Text = sr.ReadToEnd();
                    }

                    txtMessages.Clear();
                    tabQuery.SelectedTab = tpgQuery;

                    DataTable dt = new DataTable();
                    dgvResult.DataSource = dt;

                    exportFileBrowser.FileName = openFileDialog.FileName;

                    mModified = false;
                    SetTitle();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SaveFile()
        {
            if (string.IsNullOrEmpty(exportFileBrowser.FileName))
            {
                SaveFileAs();
            }
            else
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(exportFileBrowser.FileName))
                    {
                        sw.Write(txtQuery.Text);
                    }

                    mModified = false;
                    SetTitle();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SaveFileAs()
        {
            if (!string.IsNullOrEmpty(mFileName) && string.IsNullOrEmpty(exportFileBrowser.FileName))
            {
                exportFileBrowser.FileName = mFileName;
            }

            exportFileBrowser.OverwritePrompt = true;

            if (exportFileBrowser.ShowDialog() == DialogResult.OK)
            {
                mFileName = Path.GetFileNameWithoutExtension(exportFileBrowser.FileName);

                try
                {
                    using (StreamWriter sw = new StreamWriter(exportFileBrowser.FileName))
                    {
                        sw.Write(txtQuery.Text);
                    }

                    openFileDialog.FileName = exportFileBrowser.FileName;

                    mModified = false;
                    SetTitle();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public ResultForm()
        {
            InitializeComponent();
        }

        private void ResultForm_Load(object sender, EventArgs e)
        {
            dgvResult.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;

            dataExporter = new DataExporter();
            dataExporter.WorkerReportsProgress = true;
            dataExporter.WorkerSupportsCancellation = true;
            dataExporter.ExportType = DataExporter.OutputType.Table;
            dataExporter.CollectSqlMessages = mShowQuery;
            dataExporter.ProgressChanged += new ProgressChangedEventHandler(dataExporter_ProgressChanged);
            dataExporter.RunWorkerCompleted += new RunWorkerCompletedEventHandler(dataExporter_RunWorkerCompleted);

            txtQuery.Text = mCommandText;
            mModified = false;
            SetTitle();

            if (!mShowQuery)
            {
                mainMenu.Visible = false;

                splitMain.Location = new System.Drawing.Point(splitMain.Location.X, splitMain.Location.Y - 15);
                splitMain.Height += 15;

                splitMain.Panel1Collapsed = true;

                lblPosition.Visible = false;
            }

            if (mAutosizeOutput)
            {
                dgvResult.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }

            if (mAllowExport && mExportColumn != "")
            {
                btnExport.Visible = true;
                viewCodeToolStripMenuItem.Visible = true;
            }

            if (mExportColumn != "")
            {
                viewCodeToolStripMenuItem.Visible = true;
            }

            exportFileBrowser.Filter = mFileFilter;
            openFileDialog.Filter = mFileFilter;

            if (mAutoExec)
            {
                RefreshGrid();
            }
        }

        private void ResultForm_Shown(object sender, EventArgs e)
        {
            if (mCommandText == "" && mShowQuery)
            {
                txtQuery.Focus();
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExportFile();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshGrid();
            txtQuery.Focus();
        }

        private void txtQuery_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                RefreshGrid();
                txtQuery.Focus();
            }
        }

        private void dataExporter_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            mExporting = false;

            txtMessages.Text = dataExporter.Message;

            if (dataExporter.Message.Contains("Message"))
            {
                tabQuery.SelectedTab = tpgMessages;
            }

            if (e.Error != null)
            {
                MessageBox.Show(((Exception)e.Error).Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (e.Cancelled && mClosing)
            {
                dataExporter.ClearResult();
                this.Close();
            }
            else if (dataExporter.ResultTable != null)
            {
                mColumnTypes = dataExporter.ColumnTypes;

                dgvResult.DataSource = dataExporter.ResultTable;
            }

            btnRun.Enabled = true;
            btnAbort.Enabled = false;

            if (dgvResult.Rows.Count > 0 && mAllowExport && mExportColumn != "")
            {
                btnExport.Enabled = true;
            }
            else
            {
                btnExport.Enabled = false;
            }

            lblRow.Text = "Row Count";
            lblCount.Text = dgvResult.Rows.Count.ToString();
        }

        private void dataExporter_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblCount.Text = e.ProgressPercentage.ToString();
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {
            if (!dataExporter.CancellationPending)
            {
                lblRow.Text = "Cancelling...";
                dataExporter.CancelAsync();
            }
        }

        private void ResultForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mModified)
            {
                if (MessageBox.Show("Close without saving changes?", "Confirm close", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            if (mExporting)
            {
                e.Cancel = true;

                if (!dataExporter.CancellationPending)
                {
                    lblRow.Text = "Cancelling...";
                    mClosing = true;
                    dataExporter.CancelAsync();
                }
            }
            else
            {
                dataExporter.ClearResult();
            }
        }

        private void dgvResult_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            string strRowNumber = (e.RowIndex + 1).ToString();
            SizeF size = e.Graphics.MeasureString(strRowNumber, this.Font);

            if (dgvResult.RowHeadersWidth < (int)(size.Width + 20))
            {
                dgvResult.RowHeadersWidth = (int)(size.Width + 20);
            }

            Brush b = SystemBrushes.ControlText;
            e.Graphics.DrawString(strRowNumber, this.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2));
        }

        private void dgvResult_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value == DBNull.Value)
            {
                e.CellStyle.BackColor = Color.LightYellow;
            }
            else if (mColumnTypes[e.ColumnIndex] == "Byte[]")
            {
                e.Value = "<Binary data>";
                e.CellStyle.BackColor = Color.LightGreen;
            }
        }

        private void dgvResult_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            //e.Column.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            e.Column.DefaultCellStyle.NullValue = mNullString;
            int i = e.Column.Index;
            if ("Int16#Int32#Int64#Byte#Single#Double#Decimal".Contains(mColumnTypes[i]))
            {
                e.Column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyToClipboard(false);
        }

        private void copyWithHeadersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyToClipboard(true);
        }

        private void dgvResult_MouseDown(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hti = dgvResult.HitTest(e.X, e.Y);

            mCurrentRow = hti.RowIndex;

            dgvResult.ContextMenuStrip = datagridMenu;
        }

        private void dgvResult_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            mCurrentRow = e.RowIndex;

            if (mExportColumn != "" && mCurrentRow >= 0)
            {
                ViewCode();
            }
        }

        private void viewCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mExportColumn != "" && mCurrentRow >= 0)
            {
                ViewCode();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileAs();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtQuery_TextChanged(object sender, EventArgs e)
        {
            if (!mModified)
            {
                mModified = true;
                SetTitle();
            }
        }

        private void txtQuery_SelectionChanged(object sender, EventArgs e)
        {
            lblPosition.Text = string.Format("Line {0} Column {1}", txtQuery.GetLineFromCharIndex(txtQuery.SelectionStart) + 1, txtQuery.SelectionStart - txtQuery.GetFirstCharIndexOfCurrentLine() + 1);
        }
    }
}
