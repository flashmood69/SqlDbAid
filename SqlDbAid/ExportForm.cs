using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SqlDbAid
{
    public partial class ExportForm : Form
    {
        bool mExporting = false;
        bool mClosing = false;

        string mConnectionString;
        int mExecutionTimeout;
        string mObjectType;
        string mObjectSchema;
        string mObjectName;

        bool mScriptInsert = false;

        DataExporter dataExporter;

        public bool ComputedColumnSelected()
        {
            bool selected = false;

            foreach (ListViewItem s in lstDestination.Items)
            {
                if (s.SubItems[3].Text.ToLower() == "computed")
                {
                    selected = true;
                    break;
                }
            }

            return selected;
        }

        public void LoadFieldNames()
        {
            using (SqlConnection cnn = new SqlConnection(mConnectionString))
            {
                SqlCommand cmd = new SqlCommand(string.Format(QueryHelper.Query(QueryHelper.QueryId.ExportData), mObjectType.ToLower()), cnn);
                cmd.Parameters.Add(new SqlParameter("@schema", mObjectSchema));
                cmd.Parameters.Add(new SqlParameter("@name", mObjectName));

                cnn.Open();
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    string[] colData = new string[5];

                    dr.GetValues(colData);

                    lstDestination.Items.Add(new ListViewItem(colData));
                }

                dr.Close();
            }
        }

        public void ExportData()
        {
            if (lstDestination.Items.Count > 0 && chkScriptInsert.Checked)
            {
                if (ComputedColumnSelected())
                {
                    MessageBox.Show("Cannot include computed fields in insert scripts! Check info column in the selected fields list.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    return;
                }
            }
            if (lstDestination.Items.Count > 0)
            {
                DialogResult dres;

                if (chkScriptInsert.Checked)
                {
                    exportFileBrowser.DefaultExt = "sql";
                    exportFileBrowser.Filter = "Sql files|*.sql|All files|*.*";
                }
                else
                {
                    exportFileBrowser.DefaultExt = "txt";
                    exportFileBrowser.Filter = "Txt files|*.txt|All files|*.*";
                }

                exportFileBrowser.OverwritePrompt = true;
                exportFileBrowser.FileName = FileHelper.CleanFileName(string.Format("{0}.{1}_{2}", mObjectSchema, mObjectName, System.DateTime.Now.ToString("yyyyMMddHHmm")));
                dres = exportFileBrowser.ShowDialog();

                if (dres == DialogResult.OK)
                {
                    lblStatus.Text = "Exporting...";
                    mExporting = true;
                    btnExport.Enabled = false;
                    btnAbort.Enabled = true;

                    string columns = "";

                    foreach (ListViewItem s in lstDestination.Items)
                    {
                        string columnName = "[" + s.Text + "]";
                        string columnType = s.SubItems[1].Text.ToLower();
                        string columnSize = s.SubItems[2].Text;
                        string columnCollation = s.SubItems[4].Text;

                        bool escaping = false;
                        string computedColumn = "";

                        if (columns != "")
                        {
                            columns += ",";
                        }

                        if (columnType == "smalldatetime")
                        {
                            escaping = true;
                            computedColumn = string.Format("REPLACE(CONVERT(VARCHAR, {0}, 120), '-', '')", columnName);
                        }
                        else if (columnType == "datetime" || columnType == "datetime2")
                        {
                            escaping = true;
                            computedColumn = string.Format("REPLACE(CONVERT(VARCHAR, {0}, 121), '-', '')", columnName);
                        }
                        else if (columnType == "datetimeoffset")
                        {
                            escaping = true;
                            computedColumn = string.Format("REPLACE(CONVERT(VARCHAR(35), {0}, 121), '-', '')", columnName);
                        }
                        else if (columnType == "date")
                        {
                            escaping = true;
                            computedColumn = string.Format("CONVERT(VARCHAR(8), {0}, 112)", columnName);
                        }
                        else if (columnType == "time")
                        {
                            escaping = true;
                            computedColumn = string.Format("CONVERT(VARCHAR, {0})", columnName);
                        }
                        else if (columnType == "uniqueidentifier")
                        {
                            escaping = true;
                            computedColumn = string.Format("CONVERT(VARCHAR(36), {0})", columnName);
                        }
                        else if (columnType == "bit")
                        {
                            computedColumn = string.Format("CONVERT(TINYINT, {0})", columnName);
                        }
                        else if (columnType == "text" || columnType == "geography" || columnType == "geometry" || columnType == "hierarchyid")
                        {
                            escaping = true;
                            computedColumn = string.Format("CONVERT(VARCHAR(MAX), {0})", columnName);
                        }
                        else if (columnType == "ntext")
                        {
                            escaping = true;
                            computedColumn = string.Format("CONVERT(NVARCHAR(MAX), {0})", columnName);
                        }
                        else
                        {
                            if (columnCollation != "")
                            {
                                escaping = true;
                            }
                            computedColumn = columnName;
                        }

                        if (!chkScriptInsert.Checked && escaping && txtEscape.Text != "")
                        {
                            columns += string.Format("REPLACE({0}, '{1}', '{2}') {3}", computedColumn, txtTextQualifier.Text.Replace("'", "''"), txtEscape.Text.Replace("'", "''"), columnName);
                        }
                        else if (computedColumn != columnName)
                        {
                            columns += string.Format("{0} {1}", computedColumn, columnName);
                        }
                        else
                        {
                            columns += computedColumn;
                        }
                    }

                    string commandText = string.Format("SELECT {0} FROM [{1}].[{2}] WITH(NOLOCK)", columns, mObjectSchema, mObjectName);

                    if (chkScriptInsert.Checked)
                    {
                        dataExporter.ExportType = DataExporter.OutputType.Script;

                        dataExporter.TableSchema = mObjectSchema;
                        dataExporter.TableName = mObjectName;

                        dataExporter.UseLocalSettings = false;
                    }
                    else
                    {
                        dataExporter.ExportType = DataExporter.OutputType.Text;

                        dataExporter.FieldDelimiter = txtFieldDelimiter.Text.Replace(@"\r", "\r").Replace(@"\n", "\n").Replace(@"\t", "\t");
                        dataExporter.RowDelimiter = txtRowDelimiter.Text.Replace(@"\r", "\r").Replace(@"\n", "\n").Replace(@"\t", "\t");
                        dataExporter.TextQualifier = txtTextQualifier.Text.Replace(@"\r", "\r").Replace(@"\n", "\n").Replace(@"\t", "\t");

                        dataExporter.ExportColumnNames = chkIncludeColumnNames.Checked;
                        dataExporter.UseLocalSettings = chkUseLocalSettings.Checked;
                    }

                    dataExporter.Unicode = chkUnicode.Checked;

                    dataExporter.ConnectionString = mConnectionString;
                    dataExporter.ExecutionTimeout = mExecutionTimeout;
                    dataExporter.CommandText = commandText;
                    dataExporter.FileName = exportFileBrowser.FileName;

                    dataExporter.RunWorkerAsync();
                }
            }
        }

        public ExportForm()
        {
            InitializeComponent();

            dataExporter = new DataExporter();
            dataExporter.WorkerSupportsCancellation = true;
            dataExporter.WorkerReportsProgress = true;

            dataExporter.ProgressChanged += new ProgressChangedEventHandler(dataExporter_ProgressChanged);
            dataExporter.RunWorkerCompleted += new RunWorkerCompletedEventHandler(dataExporter_RunWorkerCompleted);
        }

        public ExportForm(string connectionString, int executionTimeout, string objectType, string objectSchema, string objectName)
            : this()
        {
            mConnectionString = connectionString;
            mExecutionTimeout = executionTimeout;
            mObjectType = objectType;
            mObjectSchema = objectSchema;
            mObjectName = objectName;
        }

        private void dataExporter_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblRowCount.Text = e.ProgressPercentage.ToString();
        }

        private void dataExporter_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            mExporting = false;
            btnExport.Enabled = true;
            btnAbort.Enabled = false;
            lblRowCount.Text = "";

            if (e.Cancelled && mClosing)
            {
                this.Close();
            }
            else if (e.Cancelled)
            {
                lblStatus.Text = "Cancelled";
                MessageBox.Show("The export has been cancelled", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (e.Error != null)
            {
                lblStatus.Text = "Error";
                MessageBox.Show(((Exception)e.Error).Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                lblStatus.Text = "Completed";
                MessageBox.Show("Rows Exported: " + ((int)e.Result).ToString(), "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExportData();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (lstDestination.SelectedIndices.Count > 0)
            {
                if (lstDestination.SelectedIndices[0] > 0)
                {
                    foreach (int i in lstDestination.SelectedIndices)
                    {
                        ListViewItem s = lstDestination.Items[i];
                        lstDestination.Items.RemoveAt(i);
                        lstDestination.Items.Insert(i - 1, s);
                    }
                }
            }
            lstDestination.Focus();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (lstDestination.SelectedIndices.Count > 0)
            {
                if (lstDestination.SelectedIndices[lstDestination.SelectedIndices.Count - 1] < lstDestination.Items.Count - 1)
                {
                    for (int p = lstDestination.SelectedIndices.Count - 1; p >= 0; p--)
                    {
                        int i = lstDestination.SelectedIndices[p];
                        ListViewItem s = lstDestination.Items[i];
                        lstDestination.Items.RemoveAt(i);
                        lstDestination.Items.Insert(i + 1, s);
                    }
                }
            }
            lstDestination.Focus();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            while (lstDestination.SelectedIndices.Count > 0)
            {
                int i = lstDestination.SelectedIndices[0];
                ListViewItem s = lstDestination.Items[i];
                lstDestination.Items.RemoveAt(i);
                lstSource.Items.Add(s);
            }

            if (lstDestination.Items.Count > 0)
            {
                lstDestination.SelectedIndices.Add(0);
            }
            else
            {
                btnExport.Enabled = false;
            }

            lstSource.SelectedItems.Clear();

            lstDestination.Focus();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            while (lstSource.SelectedIndices.Count > 0)
            {
                int i = lstSource.SelectedIndices[0];
                ListViewItem s = lstSource.Items[i];
                lstSource.Items.RemoveAt(i);
                lstDestination.Items.Add(s);
            }

            if (lstSource.Items.Count > 0)
            {
                lstSource.SelectedIndices.Add(0);
            }

            if (lstDestination.Items.Count > 0)
            {
                btnExport.Enabled = true;
            }

            lstDestination.SelectedItems.Clear();

            lstSource.Focus();
        }

        private void ExportForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (mObjectType.ToLower() != "table")
                {
                    chkScriptInsert.Enabled = false;
                }

                LoadFieldNames();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chkScriptInsert_CheckedChanged(object sender, EventArgs e)
        {
            mScriptInsert = chkScriptInsert.Checked;

            txtFieldDelimiter.Enabled = !mScriptInsert;
            txtRowDelimiter.Enabled = !mScriptInsert;
            txtTextQualifier.Enabled = !mScriptInsert;
            txtEscape.Enabled = !mScriptInsert;
            chkIncludeColumnNames.Enabled = !mScriptInsert;
            chkUseLocalSettings.Enabled = !mScriptInsert;
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {
            if (!dataExporter.CancellationPending)
            {
                lblStatus.Text = "Cancelling...";
                dataExporter.CancelAsync();
            }
        }

        private void ExportForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mExporting)
            {
                e.Cancel = true;

                if (!dataExporter.CancellationPending)
                {
                    lblStatus.Text = "Cancelling...";
                    mClosing = true;
                    dataExporter.CancelAsync();
                }
            }
        }
    }
}
