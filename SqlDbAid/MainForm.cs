using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace SqlDbAid
{
    public partial class MainForm : Form
    {
        #region Fields

        private bool mCheckAll = false;
        private bool mHasXeSessions = false;
        private bool mHasMSDB = false;
        private bool mLoadingDatabases = false;
        private bool mRefreshingCode = false;
        private bool mIsClosing = false;

        private int mCurrentRow = -1;
        private int mQryCount = 1;
        private int mObjectTypeLoadVersion = 0;

        private string mTitleFormat = "{0}.{1} ({2} - {3})";
        private string mDbName = "";

        private BindingSource mDatabaseBinding;
        //private BindingSource mObjTypeBinding;
        private BindingSource mObjCodeBinding;

        #endregion

        #region Types
        public class CheckedComboBoxItem
        {
            private string mVal;
            private string mName;

            public string Value
            {
                get { return mVal; }
                set { mVal = value; }
            }

            public string Name
            {
                get { return mName; }
                set { mName = value; }
            }

            public CheckedComboBoxItem(string val, string name)
            {
                this.mVal = val;
                this.mName = name;
            }

            public override string ToString()
            {
                return mName;
            }
        }

        private sealed class DatabaseLoadResult
        {
            public DataTable Databases { get; set; }
            public bool HasXeSessions { get; set; }
            public bool HasMsdb { get; set; }
        }

        #endregion

        #region Routines

        private void LoadServerList()
        {
            cmbServer.Items.Clear();

            string[] values = Properties.Settings.Default.SrvServers.Split('#');

            foreach (string value in values)
            {
                if (value != "")
                {
                    cmbServer.Items.Add(value);
                }
            }
        }

        private string BuildConnectionString(string initialCatalog)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                ["Data Source"] = Properties.Settings.Default.MwnServer,
                ["Connect Timeout"] = Properties.Settings.Default.QrsConnectionTimeout
            };

            string authMode = Properties.Settings.Default.MwnAuthenticationMode;
            
            switch (authMode)
            {
                case "Windows Authentication":
                    builder.IntegratedSecurity = true;
                    break;
                case "SQL Server Authentication":
                    builder["User ID"] = Properties.Settings.Default.MwnUsername;
                    builder["Password"] = txtPassword.Text;
                    break;
                case "Microsoft Entra Password":
                    builder["Authentication"] = "Active Directory Password";
                    builder["Password"] = txtPassword.Text;
                    if (!string.IsNullOrEmpty(Properties.Settings.Default.MwnUsername))
                    {
                        builder["User ID"] = Properties.Settings.Default.MwnUsername;
                    }
                    break;
                default:
                    // Default to Windows Authentication for backward compatibility
                    builder.IntegratedSecurity = true;
                    break;
            }

            if (!string.IsNullOrEmpty(initialCatalog))
            {
                builder["Initial Catalog"] = initialCatalog;
            }

            return builder.ConnectionString;
        }

        private bool RunnableQuery(int compatibilityLevel, bool checkViewServerStateRights, bool checkViewDatabaseStateRights, string dbName)
        {
            StringBuilder message = new StringBuilder();

            try
            {
                using (SqlConnection cnn = new SqlConnection(BuildConnectionString(mDbName)))
                {
                    SqlCommand cmd = new SqlCommand(string.Format(QueryHelper.Query(QueryHelper.QueryId.RunnableCheck), dbName.Replace("'", "''")), cnn);
                    cnn.Open();

                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    if (dr.Read())
                    {
                        if (dr.GetSqlInt32(0) < compatibilityLevel)
                        {
                            message.AppendFormat("database compatibility level must be {0} or higher\r\n", compatibilityLevel);
                        }

                        if (checkViewServerStateRights && dr.GetSqlInt32(1) == 0)
                        {
                            message.Append("VIEW SERVER STATE permission must be granted\r\n");
                        }

                        if (checkViewDatabaseStateRights && dr.GetSqlInt32(2) == 0)
                        {
                            message.Append("VIEW DATABASE STATE permission must be granted\r\n");
                        }

                        if (message.Length > 0)
                        {
                            message.Insert(0, "To perform this action\r\n");
                        }
                    }

                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                message.Append(ex.Message);
            }

            if (message.Length > 0)
            {
                MessageBox.Show(message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }

            return true;
        }

        private bool FeatureQuery(QueryHelper.QueryId queryId, string objectName, string columnName)
        {
            return FeatureQuery(BuildConnectionString(mDbName), queryId, objectName, columnName);
        }

        private static bool FeatureQuery(string connectionString, QueryHelper.QueryId queryId, string objectName, string columnName)
        {
            bool supported = false;

            try
            {
                using (SqlConnection cnn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(string.Format(QueryHelper.Query(queryId), objectName, columnName), cnn);
                    cnn.Open();

                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    if (dr.Read())
                    {
                        supported = true;
                    }

                    dr.Close();
                }
            }
            catch { }

            return supported;
        }

        private static bool DatabaseQueryAvailable(string connectionString, QueryHelper.QueryId queryId)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(QueryHelper.Query(queryId), cnn))
                {
                    cnn.Open();
                    cmd.ExecuteNonQuery();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static DatabaseLoadResult LoadDatabaseList(string connectionString, string hideSystemDbs, int executionTimeout)
        {
            DataTable databases = new DataTable();

            using (SqlConnection cnn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(string.Format(QueryHelper.Query(QueryHelper.QueryId.GuiDatabase), hideSystemDbs), cnn))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                cmd.CommandTimeout = executionTimeout;
                adapter.Fill(databases);
            }

            return new DatabaseLoadResult
            {
                Databases = databases,
                HasXeSessions = DatabaseQueryAvailable(connectionString, QueryHelper.QueryId.HasXeSessions),
                HasMsdb = DatabaseQueryAvailable(connectionString, QueryHelper.QueryId.HasMSDB)
            };
        }

        private static DataTable LoadObjectTypes(string connectionString, int executionTimeout)
        {
            DataTable table = new DataTable();

            using (SqlConnection cnn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(QueryHelper.Query(QueryHelper.QueryId.GuiObjType), cnn))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                cmd.CommandTimeout = executionTimeout;
                adapter.Fill(table);
            }

            return table;
        }

        private async Task RefreshCode()
        {
            if (chlObjType.Items.Count == 0 || mRefreshingCode)
            {
                return;
            }

            mRefreshingCode = true;
            lblCount.Text = "Loading...";

            string connectionString = BuildConnectionString(mDbName);
            string databaseName = mDbName;
            int executionTimeout = Properties.Settings.Default.QrsExecutionTimeout;
            string indexSetting = Properties.Settings.Default.ScrIncludeIndexes ? "Y" : "N";
            string searchText = txtContains.Text;
            bool useRegex = chkRegex.Checked;
            StringBuilder typeFilter = new StringBuilder("#");
            foreach (CheckedComboBoxItem item in chlObjType.CheckedItems)
            {
                typeFilter.Append(item.Value).Append("#");
            }

            bool serverEnabled = cmbServer.Enabled;
            bool databaseEnabled = cmbDatabase.Enabled;
            bool authEnabled = cmbAuthMode.Enabled;
            bool objectTypesEnabled = chlObjType.Enabled;
            bool searchEnabled = txtContains.Enabled;
            bool regexEnabled = chkRegex.Enabled;
            bool refreshEnabled = btnRefresh.Enabled;

            cmbServer.Enabled = false;
            cmbDatabase.Enabled = false;
            cmbAuthMode.Enabled = false;
            chlObjType.Enabled = false;
            txtContains.Enabled = false;
            chkRegex.Enabled = false;
            btnRefresh.Enabled = false;
            Cursor = Cursors.WaitCursor;

            try
            {
                DataTable resultTable = await Task.Run(() =>
                {
                    string filter = FeatureQuery(connectionString, QueryHelper.QueryId.FeatureTest, "indexes", "filter_definition") ? "" : "--";

                    using (SqlConnection cnn = new SqlConnection(connectionString))
                    {
                        SqlCommand cmd = new SqlCommand(string.Format(QueryHelper.Query(QueryHelper.QueryId.GuiObjCode), filter), cnn);
                        cmd.CommandTimeout = executionTimeout;
                        cmd.Parameters.Add(new SqlParameter("@type", typeFilter.ToString()));
                        cmd.Parameters.Add(new SqlParameter("@index", indexSetting));
                        //cmd.Parameters.Add(new SqlParameter("@contains", txtContains.Text.Replace("[", "[[]").Replace("_", "[_]")));
                        cmd.Parameters.Add(new SqlParameter("@contains", ""));

                        DataTable table = new DataTable();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(table);
                        }

                        if (!string.IsNullOrEmpty(searchText))
                        {
                            string search = useRegex ? searchText : Regex.Escape(searchText);
                            Regex codeRegex = new Regex(
                                search,
                                RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase,
                                TimeSpan.FromMilliseconds(250));
                            DateTime searchDeadline = DateTime.UtcNow.AddSeconds(30);

                            foreach (DataRow row in table.Rows)
                            {
                                if (DateTime.UtcNow > searchDeadline)
                                {
                                    throw new TimeoutException("The search exceeded the 30 second limit. Please narrow the search or simplify the pattern.");
                                }

                                if (!codeRegex.IsMatch(row["code"].ToString()))
                                {
                                    row.Delete();
                                }
                            }
                        }

                        return table;
                    }
                });

                if (!mIsClosing && !IsDisposed && databaseName == mDbName)
                {
                    mObjCodeBinding.DataSource = resultTable;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                if (!mIsClosing && !IsDisposed)
                {
                    MessageBox.Show("A regular expression match exceeded the 250 ms limit. Please simplify the pattern.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (TimeoutException ex)
            {
                if (!mIsClosing && !IsDisposed)
                {
                    MessageBox.Show(ex.Message, "Search", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (ArgumentException ex)
            {
                if (!mIsClosing && !IsDisposed)
                {
                    MessageBox.Show("Invalid search pattern: " + ex.Message, "Search", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                if (!mIsClosing && !IsDisposed)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                mRefreshingCode = false;
                if (!mIsClosing && !IsDisposed)
                {
                    Cursor = Cursors.Default;
                    cmbServer.Enabled = serverEnabled;
                    cmbDatabase.Enabled = databaseEnabled;
                    cmbAuthMode.Enabled = authEnabled;
                    chlObjType.Enabled = objectTypesEnabled;
                    txtContains.Enabled = searchEnabled;
                    chkRegex.Enabled = regexEnabled;
                    btnRefresh.Enabled = refreshEnabled;
                    UpdateControlStatus();
                }
            }
        }

        private int ExportCodeSingle(string fileName)
        {
            int objCount = 0;

            SortedList<int, int> code = new SortedList<int, int>();
            for (int i = 0; i < dgvCode.RowCount; i++)
            {
                if (dgvCode[0, i].EditedFormattedValue.Equals(true))
                {
                    code.Add((int)dgvCode["ord_id", i].Value, i);
                }
            }

            if (code.Count == 0)
            {
                return 0;
            }

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.WriteLine("USE [{0}]\r\n", mDbName);

                if (Properties.Settings.Default.ScrAddDropCommand == true)
                {
                    for (int p = code.Count - 1; p >= 0; p--)
                    {
                        int i = code.Values[p];
                        string objSchema = dgvCode["obj_schema", i].Value.ToString();
                        string objName = dgvCode["obj_name", i].Value.ToString();
                        string objType = dgvCode["obj_type", i].Value.ToString();

                        string header = "";

                        if (objType == "Schema")
                        {
                            header = QueryHelper.Query(QueryHelper.QueryId.DropSchemaTemplate);
                            sw.WriteLine(string.Format(header, objName.Replace("'", "''"), objType.ToUpper(), objName));
                        }
                        else if (objType == "Type")
                        {
                            header = QueryHelper.Query(QueryHelper.QueryId.DropTypeTemplate);
                            sw.WriteLine(string.Format(header, objName.Replace("'", "''"), objSchema.Replace("'", "''"), objType.ToUpper(), objSchema, objName));
                        }
                        else
                        {
                            header = QueryHelper.Query(QueryHelper.QueryId.DropTemplate);
                            sw.WriteLine(string.Format(header, objSchema.Replace("'", "''"), objName.Replace("'", "''"), objType.ToUpper(), objSchema, objName));
                        }

                        sw.WriteLine("GO\r\n");
                    }
                }

                for (int p = 0; p < code.Count; p++)
                {
                    int i = code.Values[p];
                    string objSchema = dgvCode["obj_schema", i].Value.ToString();
                    string objName = dgvCode["obj_name", i].Value.ToString();
                    string objType = dgvCode["obj_type", i].Value.ToString();
                    string objCode = dgvCode["code", i].Value.ToString();

                    sw.WriteLine("\r\nGO");
                    sw.Write(objCode);
                    if (objCode.EndsWith("\r\n") == false)
                    {
                        sw.WriteLine();
                    }
                    sw.WriteLine("GO\r\n");

                    objCount++;
                }
            }

            return objCount;
        }

        private int ExportCodeMultiple(string path)
        {
            int objCount = 0;

            ReplaceForm.ReplaceChoice replaceChoice = ReplaceForm.ReplaceChoice.No;

            for (int i = 0; i < dgvCode.RowCount; i++)
            {
                //if (dgvCode["selected", i].Value == null)
                //{
                //    continue;
                //}

                if (dgvCode[0, i].EditedFormattedValue.Equals(true))
                {
                    string objSchema = dgvCode["obj_schema", i].Value.ToString();
                    string objName = dgvCode["obj_name", i].Value.ToString();
                    string objType = dgvCode["obj_type", i].Value.ToString();
                    string objCode = dgvCode["code", i].Value.ToString();

                    string fileName = string.Format("{0}\\{1}", path, FileHelper.CleanFileName(string.Format("{0}.{1}.sql", objSchema, objName)));

                    bool fileExists = File.Exists(fileName);

                    if (replaceChoice != ReplaceForm.ReplaceChoice.YesAll && replaceChoice != ReplaceForm.ReplaceChoice.NoAll)
                    {
                        if (fileExists == true)
                        {
                            ReplaceForm confirmReplace = new ReplaceForm(fileName);
                            confirmReplace.ShowDialog();

                            replaceChoice = confirmReplace.Choice;

                            if (replaceChoice == ReplaceForm.ReplaceChoice.Cancel)
                            {
                                return objCount;
                            }
                        }
                    }

                    if (fileExists == false || replaceChoice == ReplaceForm.ReplaceChoice.Yes || replaceChoice == ReplaceForm.ReplaceChoice.YesAll)
                    {
                        using (StreamWriter sw = new StreamWriter(fileName))
                        {
                            if (Properties.Settings.Default.ScrAddDropCommand == true)
                            {
                                string header = "";

                                if (objType == "Schema")
                                {
                                    header = QueryHelper.Query(QueryHelper.QueryId.DropSchemaTemplate);
                                    sw.WriteLine(string.Format(header, objName.Replace("'", "''"), objType.ToUpper(), objName));
                                }
                                else
                                {
                                    header = QueryHelper.Query(QueryHelper.QueryId.DropTemplate);
                                    sw.WriteLine(string.Format(header, objSchema.Replace("'", "''"), objName.Replace("'", "''"), objType.ToUpper(), objSchema, objName));
                                }

                                sw.WriteLine("\r\nGO");
                            }

                            sw.Write(objCode);
                        }

                        objCount++;
                    }
                }
            }

            return objCount;
        }

        private void ExportCode()
        {
            if (dgvCode.RowCount > 0)
            {
                DialogResult dres;

                if (Properties.Settings.Default.ScrSingleFileExport == true)
                {
                    exportFileBrowser.OverwritePrompt = true;
                    exportFileBrowser.Filter = "Sql files|*.sql|All files|*.*";
                    exportFileBrowser.FileName = FileHelper.CleanFileName(string.Format("{0}_{1}", mDbName, System.DateTime.Now.ToString("yyyyMMddHHmm")));
                    dres = exportFileBrowser.ShowDialog();
                }
                else
                {
                    dres = exportFolderBrowser.ShowDialog();
                }

                if (dres == DialogResult.OK)
                {
                    try
                    {
                        int objCount = 0;

                        this.Cursor = Cursors.WaitCursor;

                        if (Properties.Settings.Default.ScrSingleFileExport == true)
                        {
                            objCount = ExportCodeSingle(exportFileBrowser.FileName);
                        }
                        else
                        {
                            objCount = ExportCodeMultiple(exportFolderBrowser.SelectedPath);
                        }

                        this.Cursor = Cursors.Default;

                        MessageBox.Show("Objects Exported: " + objCount.ToString(), "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        this.Cursor = Cursors.Default;

                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void WriteCompareScript()
        {
            DialogResult dres;

            exportFileBrowser.OverwritePrompt = true;
            exportFileBrowser.Filter = "Compare files|*.cmp|All files|*.*";
            exportFileBrowser.FileName = FileHelper.CleanFileName(string.Format("{0}_{1}_{2}", cmbServer.Text.ToLower(), mDbName.ToLower(), DateTime.Now.ToString("yyyyMMddHHmm")));
            dres = exportFileBrowser.ShowDialog();

            if (dres == DialogResult.OK)
            {
                try
                {
                    int objCount = 0;

                    DataTable objExport = new DataTable("obj_export");

                    objExport.Columns.Add("obj_schema", typeof(String));
                    objExport.Columns.Add("obj_name", typeof(String));
                    objExport.Columns.Add("obj_type", typeof(String));
                    objExport.Columns.Add("modify_date", typeof(String));
                    objExport.Columns.Add("code", typeof(String));

                    for (int i = 0; i < dgvCode.RowCount; i++)
                    {
                        if (dgvCode[0, i].EditedFormattedValue.Equals(true))
                        {
                            string objSchema = dgvCode["obj_schema", i].Value.ToString();
                            string objName = dgvCode["obj_name", i].Value.ToString();
                            string objType = dgvCode["obj_type", i].Value.ToString();
                            string objModifiedOn = dgvCode["modify_date", i].Value.ToString();
                            string objCode = dgvCode["code", i].Value.ToString();

                            object[] row = { objSchema, objName, objType, objModifiedOn, objCode };

                            objExport.Rows.Add(row);

                            objCount++;
                        }
                    }

                    if (objCount > 0)
                    {
                        //FileStream stream = new FileStream(exportFileBrowser.FileName, FileMode.Create);
                        //XmlTextWriter xmlWriter = new XmlTextWriter(stream, Encoding.Default);
                        //objExport.WriteXml(xmlWriter, XmlWriteMode.WriteSchema);
                        //xmlWriter.Close();

                        //FileStream fs = new FileStream(exportFileBrowser.FileName, FileMode.Create);
                        //StreamWriter writer = new StreamWriter(fs, Encoding.UTF8);
                        //objExport.WriteXml(writer, XmlWriteMode.WriteSchema);
                        //writer.Close();

                        objExport.WriteXml(exportFileBrowser.FileName, XmlWriteMode.WriteSchema);
                    }

                    MessageBox.Show("Objects Exported: " + objCount.ToString(), "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string ScriptCommand(QueryHelper.QueryId queryId, string schema, string name)
        {
            string code = "";
            string qry = QueryHelper.Query(queryId);
            SqlDataReader dr;

            using (SqlConnection cnn = new SqlConnection(BuildConnectionString(mDbName)))
            {
                SqlCommand cmd = new SqlCommand(qry, cnn);
                cmd.CommandTimeout = Properties.Settings.Default.QrsExecutionTimeout;

                cmd.Parameters.Add(new SqlParameter("@schema", schema));
                cmd.Parameters.Add(new SqlParameter("@name", name));

                cnn.Open();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                if (dr.Read())
                {
                    code = dr["code"].ToString();
                }

                dr.Close();
            }

            return code;
        }

        private void CreateScriptCommand(QueryHelper.QueryId queryId)
        {
            string obj_schema = dgvCode.Rows[mCurrentRow].Cells["obj_schema"].Value.ToString();
            string obj_name = dgvCode.Rows[mCurrentRow].Cells["obj_name"].Value.ToString();

            try
            {
                string code = ScriptCommand(queryId, obj_schema, obj_name);

                ViewCode(string.Format(mTitleFormat, obj_schema, obj_name, mDbName, cmbServer.Text), code, "");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateControlStatus()
        {
            bool serverLevel = (cmbDatabase.Items.Count > 0) ? true : false;
            bool databaseLevel = (cmbDatabase.SelectedIndex > 0) ? true : false;

            //#### serverLevel ####
            //ServerMonitor
            serverMonitorToolStripMenuItem.Enabled = serverLevel;
            //submenu
            processesToolStripMenuItem.Enabled = serverLevel;
            deadlocksToolStripMenuItem.Enabled = serverLevel && mHasXeSessions;
            locksToolStripMenuItem.Enabled = serverLevel;
            topQueriesTimeToolStripMenuItem.Enabled = serverLevel;
            topQueriesReadsToolStripMenuItem.Enabled = serverLevel;
            cpuToolStripMenuItem.Enabled = serverLevel;
            jobsToolStripMenuItem.Enabled = serverLevel && mHasMSDB;

            //ServerInfo
            serverInfoToolStripMenuItem.Enabled = serverLevel;
            //submenu
            propertiesToolStripMenuItem.Enabled = serverLevel;
            configurationsToolStripMenuItem.Enabled = serverLevel;
            rolesToolStripMenuItem.Enabled = serverLevel;

            //Cache
            cacheToolStripMenuItem.Enabled = serverLevel;
            //submenu
            databasesCacheToolStripMenuItem.Enabled = serverLevel;
            plansCacheToolStripMenuItem.Enabled = serverLevel;

            databaseFilesToolStripMenuItem.Enabled = serverLevel;

            //#### databaseLevel ####
            newQueryToolStripMenuItem.Enabled = databaseLevel;
            dataSearchQueryToolStripMenuItem.Enabled = databaseLevel;

            //Indexes
            indexesToolStripMenuItem.Enabled = databaseLevel;
            missingForeignKeyIndexesToolStripMenuItem.Enabled = databaseLevel;
            missingIndexesToolStripMenuItem.Enabled = databaseLevel;
            existingIndexesToolStripMenuItem.Enabled = databaseLevel;
            statisticsToolStripMenuItem.Enabled = databaseLevel;

            tableMBytesToolStripMenuItem.Enabled = databaseLevel;
            tableStatisticsToolStripMenuItem.Enabled = databaseLevel;
            databasePermissionsToolStripMenuItem.Enabled = databaseLevel;
            databaseRolesToolStripMenuItem.Enabled = databaseLevel;

            objectsCacheToolStripMenuItem.Enabled = databaseLevel;

            chlObjType.Enabled = databaseLevel;

            columnInconsistencyToolStripMenuItem.Enabled = databaseLevel;

            if (chlObjType.Items.Count > 0)
            {
                btnRefresh.Enabled = true;
            }
            else
            {
                btnRefresh.Enabled = false;
            }

            if (dgvCode.RowCount > 0)
            {
                exportSelectedObjectsToolStripMenuItem.Enabled = true;
                exportToolStripMenuItem.Enabled = true;
                btnExport.Enabled = true;
                mCheckAll = true;
            }
            else
            {
                exportSelectedObjectsToolStripMenuItem.Enabled = false;
                exportToolStripMenuItem.Enabled = false;
                btnExport.Enabled = false;
                mCheckAll = false;
            }

            lblCount.Text = dgvCode.RowCount.ToString();
        }

        private void ViewCode(string formTitle, string formText, string searchText)
        {
            CodeForm code = new CodeForm(formText, searchText, chkRegex.Checked);
            code.Icon = this.Icon;
            code.Text = formTitle;
            code.Show();
        }

        private void ClearCodeView()
        {
            DataTable table = new DataTable();
            table.Columns.Add("selected");
            table.Columns.Add("obj_type");
            table.Columns.Add("obj_schema");
            table.Columns.Add("obj_name");
            table.Columns.Add("modify_date");
            table.Columns.Add("code");
            table.Columns.Add("ord_id");
            mObjCodeBinding.DataSource = table;
        }

        private void TableStatistics(string objectName, string objectSchema)
        {
            string supportFilter = (FeatureQuery(QueryHelper.QueryId.FeatureTest, "stats", "has_filter")) ? "" : "--";

            ResultForm resultForm = new ResultForm();
            resultForm.AppendTitle = string.Format("Tables Statistics ({0} - {1})", mDbName, cmbServer.Text);
            resultForm.ShowInTaskbar = true;
            resultForm.Icon = this.Icon;
            resultForm.ShowIcon = true;

            resultForm.ConnectionString = BuildConnectionString(mDbName);
            resultForm.CommandText = string.Format(QueryHelper.Query(QueryHelper.QueryId.TableStatistics), supportFilter, objectName.Replace("'", "''"), objectSchema.Replace("'", "''"));
            resultForm.ExecutionTimeout = Properties.Settings.Default.QrsExecutionTimeout;

            resultForm.ExportFileHeader = string.Format("USE [{0}]\r\n", mDbName);
            resultForm.FileName = FileHelper.CleanFileName(string.Format("{0}{1}", mDbName.ToLower(), resultForm.AppendTitle.Replace(" ", "")));
            resultForm.ExportColumn = "script";

            resultForm.NullString = "";

            resultForm.AutosizeOutput = true;

            resultForm.Show();

        }

        private void MissingIndexes(string objectName, string objectSchema)
        {
            if (!RunnableQuery(0, true, false, ""))
            {
                return;
            }

            ResultForm resultForm = new ResultForm();
            resultForm.AppendTitle = string.Format("Missing Indexes ({0} - {1})", mDbName, cmbServer.Text);
            resultForm.ShowInTaskbar = true;
            resultForm.Icon = this.Icon;
            resultForm.ShowIcon = true;

            resultForm.ConnectionString = BuildConnectionString(mDbName);
            resultForm.CommandText = string.Format(QueryHelper.Query(QueryHelper.QueryId.MissingIndexes), objectName.Replace("'", "''"), objectSchema.Replace("'", "''"));
            resultForm.ExecutionTimeout = Properties.Settings.Default.QrsExecutionTimeout;

            resultForm.ExportFileHeader = string.Format("USE [{0}]\r\n", mDbName);
            resultForm.FileName = FileHelper.CleanFileName(string.Format("{0}{1}", mDbName.ToLower(), resultForm.AppendTitle.Replace(" ", "")));
            resultForm.ExportColumn = "script";

            resultForm.Show();
        }

        private void IndexesStatus(int level, string objectName, string objectSchema)
        {
            if (!RunnableQuery(90, true, true, ""))
            {
                return;
            }

            string analysisLevel;
            string comment1 = "--";
            string comment2 = "";
            string compression = (FeatureQuery(QueryHelper.QueryId.FeatureTest, "partitions", "data_compression_desc")) ? "" : "--";
            switch (level)
            {
                case 1:
                    analysisLevel = "LIMITED";
                    break;
                case 2:
                    analysisLevel = "SAMPLED";
                    break;
                case 3:
                    analysisLevel = "DETAILED";
                    break;
                default:
                    analysisLevel = "LIMITED";
                    comment1 = "";
                    comment2 = "--";
                    break;
            }

            ResultForm resultForm = new ResultForm();
            resultForm.AppendTitle = string.Format("Indexes Status ({0} - {1})", mDbName, cmbServer.Text);
            resultForm.ShowInTaskbar = true;
            resultForm.Icon = this.Icon;
            resultForm.ShowIcon = true;

            resultForm.ConnectionString = BuildConnectionString(mDbName);
            resultForm.CommandText = string.Format(QueryHelper.Query(QueryHelper.QueryId.IndexesStatus), analysisLevel, objectName.Replace("'", "''"), objectSchema.Replace("'", "''"), comment1, comment2, compression);
            resultForm.ExecutionTimeout = Properties.Settings.Default.QrsExecutionTimeout;

            resultForm.ExportFileHeader = string.Format("USE [{0}]\r\n", mDbName);
            resultForm.FileName = FileHelper.CleanFileName(string.Format("{0}{1}", mDbName.ToLower(), resultForm.AppendTitle.Replace(" ", "")));
            resultForm.ExportColumn = "script";

            resultForm.NullString = "";

            resultForm.Show();
        }

        private void TopQueries(bool time)
        {
            if (!RunnableQuery(0, true, false, ""))
            {
                return;
            }

            string filterAppQry = Properties.Settings.Default.TlsHideAppQueries ? "" : "--";
            string currentDbOnly = (mDbName != "") ? "" : "--";
            string topQryWithin = (Properties.Settings.Default.TlsTopQueriesWithin > 0) ? "" : "--";
            string byTime = (time) ? "" : "--";
            string byReads = (!time) ? "" : "--";

            ResultForm resultForm = new ResultForm();
            resultForm.AppendTitle = string.Format("Top Queries ({0} - {1})", cmbDatabase.Text, cmbServer.Text);
            resultForm.ShowInTaskbar = true;
            resultForm.Icon = this.Icon;
            resultForm.ShowIcon = true;

            resultForm.ConnectionString = BuildConnectionString("master");
            resultForm.CommandText = string.Format(QueryHelper.Query(QueryHelper.QueryId.TopQueries), filterAppQry, currentDbOnly, mDbName.Replace("'", "''"), topQryWithin, Properties.Settings.Default.TlsTopQueriesWithin.ToString(), byTime, byReads);
            resultForm.ExecutionTimeout = Properties.Settings.Default.QrsExecutionTimeout;

            resultForm.ExportColumn = "sql_statement";
            resultForm.AllowExport = false;

            resultForm.Show();
        }

        #endregion

        #region Constructors

        public MainForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Events

        private void frmMain_Load(object sender, EventArgs e)
        {
            cmbServer.Text = Properties.Settings.Default.MwnServer;
            txtUsername.Text = Properties.Settings.Default.MwnUsername;
            cmbAuthMode.SelectedItem = Properties.Settings.Default.MwnAuthenticationMode;
            
            // Set default if no value is saved
            if (cmbAuthMode.SelectedItem == null)
            {
                cmbAuthMode.SelectedItem = "Windows Authentication";
            }

            LoadServerList();

            mObjCodeBinding = new BindingSource();
            dgvCode.DataSource = mObjCodeBinding;
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            mIsClosing = true;
            Properties.Settings.Default.MwnServer = cmbServer.Text;
            Properties.Settings.Default.MwnUsername = txtUsername.Text;
            Properties.Settings.Default.MwnAuthenticationMode = cmbAuthMode.SelectedItem.ToString();

            Properties.Settings.Default.Save();
        }

        private void cmbServer_TextChanged(object sender, EventArgs e)
        {
            if (cmbDatabase.Items.Count > 1)
            {
                cmbDatabase.DataSource = null;
                cmbDatabase.Items.Clear();

                UpdateControlStatus();
            }

            Properties.Settings.Default.MwnServer = cmbServer.Text;
        }

        private async void cmbDatabase_DropDown(object sender, EventArgs e)
        {
            if (mLoadingDatabases)
            {
                return;
            }

            mLoadingDatabases = true;
            string hideSystemDbs = Properties.Settings.Default.ScrHideSystemDbs ? "" : "--";
            int executionTimeout = Properties.Settings.Default.QrsExecutionTimeout;
            bool serverEnabled = cmbServer.Enabled;
            bool authEnabled = cmbAuthMode.Enabled;
            cmbServer.Enabled = false;
            cmbAuthMode.Enabled = false;
            Cursor = Cursors.WaitCursor;
            bool databaseListLoaded = false;

            try
            {
                string connectionString = BuildConnectionString("");
                DatabaseLoadResult result = await Task.Run(() => LoadDatabaseList(connectionString, hideSystemDbs, executionTimeout));
                if (mIsClosing || IsDisposed)
                {
                    return;
                }

                mHasXeSessions = result.HasXeSessions;
                mHasMSDB = result.HasMsdb;
                mDatabaseBinding = new BindingSource();
                mDatabaseBinding.DataSource = result.Databases;
                cmbDatabase.DataSource = mDatabaseBinding;
                cmbDatabase.DisplayMember = "database_name";
                cmbDatabase.ValueMember = "id";
                databaseListLoaded = true;
            }
            catch (Exception ex)
            {
                if (mIsClosing || IsDisposed)
                {
                    return;
                }

                cmbDatabase.DataSource = null;
                chlObjType.Items.Clear();
                mDbName = "";
                ClearCodeView();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                mLoadingDatabases = false;
                if (!mIsClosing && !IsDisposed)
                {
                    Cursor = Cursors.Default;
                    cmbServer.Enabled = serverEnabled;
                    cmbAuthMode.Enabled = authEnabled;
                    UpdateControlStatus();
                }
            }

            if (databaseListLoaded && !mIsClosing && !IsDisposed)
            {
                cmbDatabase_SelectedIndexChanged(cmbDatabase, EventArgs.Empty);
            }
        }

        private async void cmbDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mLoadingDatabases || mIsClosing || IsDisposed)
            {
                return;
            }

            int loadVersion = ++mObjectTypeLoadVersion;
            chlObjType.Items.Clear();
            mDbName = "";

            if (dgvCode.RowCount > 0)
            {
                ClearCodeView();
            }
            UpdateControlStatus();

            if (cmbDatabase.SelectedIndex > 0)
            {
                string databaseName = cmbDatabase.Text;
                int executionTimeout = Properties.Settings.Default.QrsExecutionTimeout;
                mDbName = databaseName;
                Cursor = Cursors.WaitCursor;
                bool loadFailed = false;

                cmbServer.Enabled = false;
                cmbDatabase.Enabled = false;
                cmbAuthMode.Enabled = false;

                try
                {
                    string connectionString = BuildConnectionString(databaseName);
                    DataTable table = await Task.Run(() => LoadObjectTypes(connectionString, executionTimeout));

                    if (!mIsClosing && !IsDisposed && loadVersion == mObjectTypeLoadVersion)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            chlObjType.Items.Add(new CheckedComboBoxItem(row.ItemArray[0].ToString().Trim(), row.ItemArray[1].ToString().Trim()), true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!mIsClosing && !IsDisposed && loadVersion == mObjectTypeLoadVersion)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        mDbName = "";
                        loadFailed = true;
                    }
                }
                finally
                {
                    if (!mIsClosing && !IsDisposed && loadVersion == mObjectTypeLoadVersion)
                    {
                        Cursor = Cursors.Default;
                        cmbServer.Enabled = true;
                        cmbDatabase.Enabled = true;
                        cmbAuthMode.Enabled = true;

                        if (loadFailed)
                        {
                            cmbDatabase.SelectedIndex = 0;
                        }

                        UpdateControlStatus();
                    }
                }
            }
            else
            {
                Cursor = Cursors.Default;
                cmbServer.Enabled = true;
                cmbDatabase.Enabled = true;
                cmbAuthMode.Enabled = true;
            }

            UpdateControlStatus();
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await RefreshCode();
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportCode();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExportCode();
        }

        private void dgvCode_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            dgvCode.Rows[e.RowIndex].HeaderCell.ToolTipText = "Double click to view the code.\r\nRight click for object type related options.";
        }

        private void dgvCode_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvCode.Columns[e.ColumnIndex].HeaderText == "Name" && "#Function#Procedure#View#Trigger#".Contains(dgvCode.Rows[e.RowIndex].Cells["obj_type"].Value.ToString()))
            {
                string code = dgvCode.Rows[e.RowIndex].Cells["code"].Value.ToString();

                if (!string.IsNullOrEmpty(code))
                {
                    string obj_name = dgvCode.Rows[e.RowIndex].Cells["obj_name"].Value.ToString();

                    try
                    {
                        if (!Regex.IsMatch(code, string.Format(@"(\s|\r|\n|\.|""|\[){0}(\s|\r|\n|\(|""|\])", Regex.Escape(obj_name))))
                        {
                            e.CellStyle.BackColor = Color.Yellow;
                            dgvCode.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = "Code definition is using a different object name.\r\nDrop and recreate using this name to align definition.";
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        private void dgvCode_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0 && dgvCode.RowCount > 0)
            {
                mCheckAll = !mCheckAll;

                for (int i = 0; i < dgvCode.RowCount; i++)
                {
                    dgvCode["selected", i].Value = mCheckAll;
                }
                dgvCode.EndEdit();
            }
        }

        private void cmbAuthMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool enableCredentials = cmbAuthMode.SelectedItem.ToString() != "Windows Authentication";
            txtUsername.Enabled = enableCredentials;
            txtPassword.Enabled = enableCredentials;

            Properties.Settings.Default.MwnAuthenticationMode = cmbAuthMode.SelectedItem.ToString();

            if (cmbDatabase.Items.Count > 1)
            {
                cmbDatabase.DataSource = null;
                cmbDatabase.Items.Clear();

                UpdateControlStatus();
            }
        }

        private void missingForeignKeyIndexesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResultForm resultForm = new ResultForm();
            resultForm.AppendTitle = string.Format("Missing Fk Indexes ({0} - {1})", mDbName, cmbServer.Text);
            resultForm.ShowInTaskbar = true;
            resultForm.Icon = this.Icon;
            resultForm.ShowIcon = true;

            resultForm.ConnectionString = BuildConnectionString(mDbName);
            resultForm.CommandText = QueryHelper.Query(QueryHelper.QueryId.MissingFkIndexes);
            resultForm.ExecutionTimeout = Properties.Settings.Default.QrsExecutionTimeout;

            resultForm.ExportFileHeader = string.Format("USE [{0}]\r\n", mDbName);
            resultForm.FileName = FileHelper.CleanFileName(string.Format("{0}{1}", mDbName.ToLower(), resultForm.AppendTitle.Replace(" ", "")));
            resultForm.ExportColumn = "script";

            resultForm.Show();
        }

        private void missingIndexesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MissingIndexes("", "");
        }

        private void statusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IndexesStatus(0, "", "");
        }

        private void limitedFragmentAnalysisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IndexesStatus(1, "", "");
        }

        private void sampledFragmentAnalysisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IndexesStatus(2, "", "");
        }

        private void detailedFragmentAnalysisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IndexesStatus(3, "", "");
        }

        private void existingIndexesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filter = (FeatureQuery(QueryHelper.QueryId.FeatureTest, "indexes", "filter_definition")) ? "" : "--";

            ResultForm resultForm = new ResultForm();
            resultForm.AppendTitle = string.Format("Existing Indexes ({0} - {1})", mDbName, cmbServer.Text);
            resultForm.ShowInTaskbar = true;
            resultForm.Icon = this.Icon;
            resultForm.ShowIcon = true;

            resultForm.ConnectionString = BuildConnectionString(mDbName);
            resultForm.CommandText = string.Format(QueryHelper.Query(QueryHelper.QueryId.ExistingIndexes), filter);
            resultForm.ExecutionTimeout = Properties.Settings.Default.QrsExecutionTimeout;

            resultForm.ExportFileHeader = string.Format("USE [{0}]\r\n", mDbName);
            resultForm.FileName = FileHelper.CleanFileName(string.Format("{0}{1}", mDbName.ToLower(), resultForm.AppendTitle.Replace(" ", "")));
            resultForm.ExportColumn = "script";

            resultForm.Show();
        }

        private void tableStatisticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TableStatistics("", "");
        }


        private void columnInconsistencyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResultForm resultForm = new ResultForm();
            resultForm.AppendTitle = string.Format("Columns Inconsistency ({0} - {1})", mDbName, cmbServer.Text);
            resultForm.ShowInTaskbar = true;
            resultForm.Icon = this.Icon;
            resultForm.ShowIcon = true;

            resultForm.ConnectionString = BuildConnectionString(mDbName);
            resultForm.CommandText = QueryHelper.Query(QueryHelper.QueryId.ColumnInconsistency);
            resultForm.ExecutionTimeout = Properties.Settings.Default.QrsExecutionTimeout;

            resultForm.Show();
        }

        private void tableMBytesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResultForm resultForm = new ResultForm();
            resultForm.AppendTitle = string.Format("Tables MBytes ({0} - {1})", mDbName, cmbServer.Text);
            resultForm.ShowInTaskbar = true;
            resultForm.Icon = this.Icon;
            resultForm.ShowIcon = true;

            resultForm.ConnectionString = BuildConnectionString(mDbName);
            resultForm.CommandText = QueryHelper.Query(QueryHelper.QueryId.TableMBytes);
            resultForm.ExecutionTimeout = Properties.Settings.Default.QrsExecutionTimeout;

            resultForm.ExportColumn = "script";
            resultForm.AllowExport = true;

            resultForm.AutosizeOutput = true;

            resultForm.Show();
        }

        private void databaseFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResultForm resultForm = new ResultForm();
            resultForm.AppendTitle = string.Format("Database Files ({0} - {1})", cmbDatabase.Text, cmbServer.Text);
            resultForm.ShowInTaskbar = true;
            resultForm.Icon = this.Icon;
            resultForm.ShowIcon = true;

            resultForm.ConnectionString = BuildConnectionString(mDbName);
            resultForm.CommandText = string.Format(QueryHelper.Query(QueryHelper.QueryId.DatabaseFiles), mDbName.Replace("'", "''"));
            resultForm.ExecutionTimeout = Properties.Settings.Default.QrsExecutionTimeout;

            resultForm.ExportColumn = "script";
            resultForm.AllowExport = true;

            resultForm.AutosizeOutput = true;

            resultForm.Show();
        }

        private void databasePermissionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResultForm resultForm = new ResultForm();
            resultForm.AppendTitle = string.Format("Databases Permissions ({0} - {1})", cmbDatabase.Text, cmbServer.Text);
            resultForm.ShowInTaskbar = true;
            resultForm.Icon = this.Icon;
            resultForm.ShowIcon = true;

            resultForm.ConnectionString = BuildConnectionString(mDbName);
            resultForm.CommandText = QueryHelper.Query(QueryHelper.QueryId.DatabasePermissions);
            resultForm.ExecutionTimeout = Properties.Settings.Default.QrsExecutionTimeout;

            resultForm.ExportColumn = "script";
            resultForm.AllowExport = true;

            resultForm.AutosizeOutput = true;

            resultForm.Show();
        }

        private void databaseRolesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResultForm resultForm = new ResultForm();
            resultForm.AppendTitle = string.Format("Databases Roles ({0} - {1})", cmbDatabase.Text, cmbServer.Text);
            resultForm.ShowInTaskbar = true;
            resultForm.Icon = this.Icon;
            resultForm.ShowIcon = true;

            resultForm.ConnectionString = BuildConnectionString(mDbName);
            resultForm.CommandText = QueryHelper.Query(QueryHelper.QueryId.DatabaseRoles);
            resultForm.ExecutionTimeout = Properties.Settings.Default.QrsExecutionTimeout;

            resultForm.ExportColumn = "script";
            resultForm.AllowExport = true;

            resultForm.AutosizeOutput = true;

            resultForm.Show();
        }

        private void configurationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResultForm resultForm = new ResultForm();
            resultForm.AppendTitle = string.Format("Server Configurations ({0})", cmbServer.Text);
            resultForm.ShowInTaskbar = true;
            resultForm.Icon = this.Icon;
            resultForm.ShowIcon = true;

            resultForm.ConnectionString = BuildConnectionString("master");
            resultForm.CommandText = QueryHelper.Query(QueryHelper.QueryId.ServerConfigurations);
            resultForm.ExecutionTimeout = Properties.Settings.Default.QrsExecutionTimeout;

            resultForm.AllowExport = false;

            resultForm.Show();
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string memory_B = (FeatureQuery(QueryHelper.QueryId.FeatureTest, "dm_os_sys_info", "physical_memory_in_bytes")) ? "" : "--";
            string memory_KB = (memory_B == "--") ? "" : "--";

            ResultForm resultForm = new ResultForm();
            resultForm.AppendTitle = string.Format("Server Properties ({0})", cmbServer.Text);
            resultForm.ShowInTaskbar = true;
            resultForm.Icon = this.Icon;
            resultForm.ShowIcon = true;
            resultForm.AutosizeOutput = true;

            resultForm.ConnectionString = BuildConnectionString("master");
            resultForm.CommandText = string.Format(QueryHelper.Query(QueryHelper.QueryId.ServerProperties), memory_B, memory_KB);
            resultForm.ExecutionTimeout = Properties.Settings.Default.QrsExecutionTimeout;

            resultForm.AllowExport = false;

            resultForm.Show();
        }

        private void rolesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResultForm resultForm = new ResultForm();
            resultForm.AppendTitle = string.Format("Server Roles ({0})", cmbServer.Text);
            resultForm.ShowInTaskbar = true;
            resultForm.Icon = this.Icon;
            resultForm.ShowIcon = true;
            resultForm.AutosizeOutput = true;

            resultForm.ConnectionString = BuildConnectionString("master");
            resultForm.CommandText = QueryHelper.Query(QueryHelper.QueryId.ServerRoles);
            resultForm.ExecutionTimeout = Properties.Settings.Default.QrsExecutionTimeout;

            resultForm.ExportColumn = "script";
            resultForm.AllowExport = true;

            resultForm.Show();
        }

        private void processesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!RunnableQuery(0, true, false, ""))
            {
                return;
            }

            string hideAppQry = Properties.Settings.Default.TlsHideAppQueries ? "" : "--";
            string currentDbOnly = "--";
            string hasMsdb = mHasMSDB ? "" : "--";
            string hasNotMsdb = mHasMSDB ? "--" : "";
            //string currentDbOnly = (mDbName != "") ? "" : "--";
            //string database_id = (FeatureQuery(QueryHelper.QueryId.FeatureTest, "dm_exec_sessions", "database_id")) ? "" : "--";
            //string dbid = (database_id == "--") ? "" : "--";

            ResultForm resultForm = new ResultForm();
            resultForm.AppendTitle = string.Format("Processes ({0})", cmbServer.Text);
            //resultForm.AppendTitle = string.Format("Processes ({0} - {1})", cmbDatabase.Text, cmbServer.Text);
            resultForm.ShowInTaskbar = true;
            resultForm.Icon = this.Icon;
            resultForm.ShowIcon = true;

            resultForm.ConnectionString = BuildConnectionString("master");
            resultForm.CommandText = string.Format(QueryHelper.Query(QueryHelper.QueryId.Processes), hideAppQry, currentDbOnly, mDbName.Replace("'", "''"), hasMsdb, hasNotMsdb);
            resultForm.ExecutionTimeout = Properties.Settings.Default.QrsExecutionTimeout;

            resultForm.ExportColumn = "sql_statement";
            resultForm.AllowExport = false;

            resultForm.Show();
        }

        private void jobsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResultForm resultForm = new ResultForm();
            resultForm.AppendTitle = string.Format("Jobs ({0})", cmbServer.Text);
            resultForm.ShowInTaskbar = true;
            resultForm.Icon = this.Icon;
            resultForm.ShowIcon = true;

            resultForm.ConnectionString = BuildConnectionString("msdb");
            resultForm.CommandText = QueryHelper.Query(QueryHelper.QueryId.Jobs);
            resultForm.ExecutionTimeout = Properties.Settings.Default.QrsExecutionTimeout;

            resultForm.AllowExport = false;

            resultForm.Show();
        }

        private void cpuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResultForm resultForm = new ResultForm();
            resultForm.AppendTitle = string.Format("Cpu usage % ({0})", cmbServer.Text);
            resultForm.ShowInTaskbar = true;
            resultForm.Icon = this.Icon;
            resultForm.ShowIcon = true;

            resultForm.ConnectionString = BuildConnectionString("master");
            resultForm.CommandText = QueryHelper.Query(QueryHelper.QueryId.Cpu);
            resultForm.ExecutionTimeout = Properties.Settings.Default.QrsExecutionTimeout;

            resultForm.AllowExport = false;

            resultForm.Show();
        }

        private void locksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!RunnableQuery(0, true, false, ""))
            {
                return;
            }

            string getObjectName = Properties.Settings.Default.TlsGetLockedObjectName ? "1" : "0";
            string filterAppQry = Properties.Settings.Default.TlsHideAppQueries ? "" : "--";
            string currentDbOnly = (mDbName != "") ? "" : "--";

            ResultForm resultForm = new ResultForm();
            resultForm.AppendTitle = string.Format("Locks ({0} - {1})", cmbDatabase.Text, cmbServer.Text);
            resultForm.ShowInTaskbar = true;
            resultForm.Icon = this.Icon;
            resultForm.ShowIcon = true;

            resultForm.ConnectionString = BuildConnectionString("master");
            resultForm.CommandText = string.Format(QueryHelper.Query(QueryHelper.QueryId.Locks), filterAppQry, currentDbOnly, mDbName.Replace("'", "''"), getObjectName);
            resultForm.ExecutionTimeout = Properties.Settings.Default.QrsExecutionTimeout;

            resultForm.ExportColumn = "sql_statement";
            resultForm.AllowExport = false;

            resultForm.Show();
        }

        private void deadlocksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!RunnableQuery(110, true, false, "master")) //SQL Server 2012
            {
                return;
            }

            string hasMsdb = mHasMSDB ? "" : "--";
            string hasNotMsdb = mHasMSDB ? "--" : "";

            ResultForm resultForm = new ResultForm();
            resultForm.AppendTitle = string.Format("Deadlocks ({0})", cmbServer.Text);
            resultForm.ShowInTaskbar = true;
            resultForm.Icon = this.Icon;
            resultForm.ShowIcon = true;

            resultForm.ConnectionString = BuildConnectionString("master");
            resultForm.CommandText = String.Format(QueryHelper.Query(QueryHelper.QueryId.Deadlocks), hasMsdb, hasNotMsdb);
            resultForm.ExecutionTimeout = Properties.Settings.Default.QrsExecutionTimeout;

            resultForm.ExportColumn = "process_code";
            resultForm.AllowExport = false;

            resultForm.Show();
        }

        private void topQueriesTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TopQueries(true);
        }

        private void topQueriesReadsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TopQueries(false);
        }

        private void planCachesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!RunnableQuery(0, true, false, ""))
            {
                return;
            }

            string filterAppQry = Properties.Settings.Default.TlsHideAppQueries ? "" : "--";
            string currentDbOnly = (mDbName != "") ? "" : "--";

            ResultForm resultForm = new ResultForm();
            resultForm.AppendTitle = string.Format("Plans Cache ({0} - {1})", cmbDatabase.Text, cmbServer.Text);
            resultForm.ShowInTaskbar = true;
            resultForm.Icon = this.Icon;
            resultForm.ShowIcon = true;

            resultForm.ConnectionString = BuildConnectionString("master");
            resultForm.CommandText = string.Format(QueryHelper.Query(QueryHelper.QueryId.PlansCache), filterAppQry, currentDbOnly, mDbName.Replace("'", "''"));
            resultForm.ExecutionTimeout = Properties.Settings.Default.QrsExecutionTimeout;

            resultForm.Show();
        }

        private void cacheDistrubutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!RunnableQuery(0, true, false, ""))
            {
                return;
            }

            string currentDbOnly = (mDbName != "") ? "" : "--";

            ResultForm resultForm = new ResultForm();
            resultForm.AppendTitle = string.Format("Databases Cache ({0} - {1})", cmbDatabase.Text, cmbServer.Text);
            resultForm.ShowInTaskbar = true;
            resultForm.Icon = this.Icon;
            resultForm.ShowIcon = true;

            resultForm.ConnectionString = BuildConnectionString("master");
            resultForm.CommandText = string.Format(QueryHelper.Query(QueryHelper.QueryId.DatabasesCache), currentDbOnly, mDbName.Replace("'", "''"));
            resultForm.ExecutionTimeout = Properties.Settings.Default.QrsExecutionTimeout;

            resultForm.Show();
        }

        private void cachedObjectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!RunnableQuery(0, true, false, ""))
            {
                return;
            }

            ResultForm resultForm = new ResultForm();
            resultForm.AppendTitle = string.Format("Objects Cache ({0} - {1})", cmbDatabase.Text, cmbServer.Text);
            resultForm.ShowInTaskbar = true;
            resultForm.Icon = this.Icon;
            resultForm.ShowIcon = true;

            resultForm.ConnectionString = BuildConnectionString(cmbDatabase.Text);
            resultForm.CommandText = QueryHelper.Query(QueryHelper.QueryId.ObjectsCache);
            resultForm.ExecutionTimeout = Properties.Settings.Default.QrsExecutionTimeout;

            resultForm.Show();
        }

        private void newQueryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResultForm resultForm = new ResultForm();
            resultForm.AppendTitle = string.Format(" ({0} - {1})", cmbDatabase.Text, cmbServer.Text);
            resultForm.ShowInTaskbar = true;
            resultForm.ShowIcon = true;
            resultForm.Icon = this.Icon;

            resultForm.ConnectionString = BuildConnectionString(mDbName);
            resultForm.CommandText = "";
            resultForm.ExecutionTimeout = Properties.Settings.Default.QrsExecutionTimeout;

            resultForm.ShowQuery = true;
            resultForm.FileName = string.Format("Query{0}", mQryCount++);

            resultForm.Show();
        }

        private void dataSearchQueryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string qry = QueryHelper.Query(QueryHelper.QueryId.DataSearch);
            SqlDataReader dr;
            StringBuilder searchQry = new StringBuilder();

            using (SqlConnection cnn = new SqlConnection(BuildConnectionString(mDbName)))
            {
                SqlCommand cmd = new SqlCommand(qry, cnn);
                cmd.CommandTimeout = Properties.Settings.Default.QrsExecutionTimeout;

                cnn.Open();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    searchQry.AppendFormat("{0}\r\n", dr["cmd"].ToString());
                }

                dr.Close();
            }

            ResultForm resultForm = new ResultForm();
            resultForm.AppendTitle = string.Format(" ({0} - {1})", cmbDatabase.Text, cmbServer.Text);
            resultForm.ShowInTaskbar = true;
            resultForm.Icon = this.Icon;
            resultForm.ShowIcon = true;

            resultForm.ConnectionString = BuildConnectionString(mDbName);
            resultForm.CommandText = searchQry.ToString();
            resultForm.ExecutionTimeout = Properties.Settings.Default.QrsExecutionTimeout;

            resultForm.ShowQuery = true;
            resultForm.FileName = string.Format("Query{0}", mQryCount++);

            resultForm.AutoExec = false;
            resultForm.AutosizeOutput = true;

            resultForm.Show();
        }

        private void exportSelectedObjectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvCode.RowCount > 0)
            {
                WriteCompareScript();
            }
        }

        private void compareExportedObjectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CompareForm compare = new CompareForm();
            compare.Icon = this.Icon;
            compare.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm about = new AboutForm();
            about.ShowDialog();
        }

        private void dgvCode_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            mCurrentRow = e.RowIndex;
            if (mCurrentRow >= 0)
            {
                string obj_schema = dgvCode.Rows[mCurrentRow].Cells["obj_schema"].Value.ToString();
                string obj_name = dgvCode.Rows[mCurrentRow].Cells["obj_name"].Value.ToString();
                string code = dgvCode.Rows[mCurrentRow].Cells["code"].Value.ToString();

                ViewCode(string.Format(mTitleFormat, obj_schema, obj_name, mDbName, cmbServer.Text), code, txtContains.Text);
            }
        }

        private void dgvCode_MouseDown(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hti = dgvCode.HitTest(e.X, e.Y);

            mCurrentRow = hti.RowIndex;

            if (mCurrentRow >= 0)
            {
                string obj_type = dgvCode.Rows[mCurrentRow].Cells["obj_type"].Value.ToString();

                if (obj_type == "Table")
                {
                    scriptSelectToolStripMenuItem.Visible = true;
                    scriptInsertToolStripMenuItem.Visible = true;
                    scriptUpdateToolStripMenuItem.Visible = true;
                    countDistinctToolStripMenuItem.Visible = true;

                    exportDataToolStripMenuItem.Visible = true;
                    viewDataToolStripMenuItem.Visible = true;

                    missingIndexesToolStripMenuItem1.Visible = true;
                    indexStatisticsToolStripMenuItem.Visible = true;
                }
                else if (obj_type == "View")
                {
                    scriptSelectToolStripMenuItem.Visible = true;
                    scriptInsertToolStripMenuItem.Visible = false;
                    scriptUpdateToolStripMenuItem.Visible = false;
                    countDistinctToolStripMenuItem.Visible = false;

                    exportDataToolStripMenuItem.Visible = true;
                    viewDataToolStripMenuItem.Visible = true;

                    missingIndexesToolStripMenuItem1.Visible = true;
                    indexStatisticsToolStripMenuItem.Visible = false;
                }
                else
                {
                    scriptSelectToolStripMenuItem.Visible = false;
                    scriptInsertToolStripMenuItem.Visible = false;
                    scriptUpdateToolStripMenuItem.Visible = false;
                    countDistinctToolStripMenuItem.Visible = false;

                    exportDataToolStripMenuItem.Visible = false;
                    viewDataToolStripMenuItem.Visible = false;

                    missingIndexesToolStripMenuItem1.Visible = false;
                    indexStatisticsToolStripMenuItem.Visible = false;
                }

                dgvCode.ContextMenuStrip = datagridMenu;
            }
        }

        private void viewCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mCurrentRow >= 0)
            {
                string obj_schema = dgvCode.Rows[mCurrentRow].Cells["obj_schema"].Value.ToString();
                string obj_name = dgvCode.Rows[mCurrentRow].Cells["obj_name"].Value.ToString();
                string code = dgvCode.Rows[mCurrentRow].Cells["code"].Value.ToString();

                ViewCode(string.Format(mTitleFormat, obj_schema, obj_name, mDbName, cmbServer.Text), code, txtContains.Text);
            }
        }

        private void scriptSelectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mCurrentRow >= 0)
            {
                if (dgvCode.Rows[mCurrentRow].Cells["obj_type"].Value.ToString() == "Table")
                {
                    CreateScriptCommand(QueryHelper.QueryId.ScriptSelect);
                }
                else
                {
                    CreateScriptCommand(QueryHelper.QueryId.ScriptViewSelect);
                }
            }
        }

        private void scriptInsertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mCurrentRow >= 0)
            {
                CreateScriptCommand(QueryHelper.QueryId.ScriptInsert);
            }
        }

        private void scriptUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mCurrentRow >= 0)
            {
                CreateScriptCommand(QueryHelper.QueryId.ScriptUpdate);
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionForm option = new OptionForm();
            option.ShowDialog();

            LoadServerList();
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.MwnUsername = txtUsername.Text;
        }

        private async void txtContains_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                await RefreshCode();
            }
        }

        private void exportDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mCurrentRow >= 0)
            {
                string obj_type = dgvCode.Rows[mCurrentRow].Cells["obj_type"].Value.ToString();
                string obj_schema = dgvCode.Rows[mCurrentRow].Cells["obj_schema"].Value.ToString();
                string obj_name = dgvCode.Rows[mCurrentRow].Cells["obj_name"].Value.ToString();

                ExportForm exportData = new ExportForm(BuildConnectionString(mDbName), Properties.Settings.Default.QrsExecutionTimeout, obj_type, obj_schema, obj_name);
                exportData.Text = string.Format("{0}.{1}", obj_schema, obj_name);
                exportData.ShowDialog();
            }
        }

        private void viewDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mCurrentRow >= 0)
            {
                string openTopRows = (Properties.Settings.Default.QrsOpenTopRows > 0) ? string.Format(" TOP {0}", Properties.Settings.Default.QrsOpenTopRows) : "";
                string openWithNoLock = Properties.Settings.Default.QrsOpenWithNoLock ? " WITH(NOLOCK)" : "";

                string obj_type = dgvCode.Rows[mCurrentRow].Cells["obj_type"].Value.ToString();
                string obj_schema = dgvCode.Rows[mCurrentRow].Cells["obj_schema"].Value.ToString();
                string obj_name = dgvCode.Rows[mCurrentRow].Cells["obj_name"].Value.ToString();

                ResultForm resultForm = new ResultForm();
                resultForm.AppendTitle = string.Format(" ({0} - {1})", cmbDatabase.Text, cmbServer.Text);
                resultForm.ShowInTaskbar = true;
                resultForm.Icon = this.Icon;
                resultForm.ShowIcon = true;

                resultForm.ConnectionString = BuildConnectionString(mDbName);
                resultForm.CommandText = string.Format(QueryHelper.Query(QueryHelper.QueryId.ViewData), openTopRows, mDbName, obj_schema, obj_name, openWithNoLock);
                resultForm.ExecutionTimeout = Properties.Settings.Default.QrsExecutionTimeout;

                resultForm.ShowQuery = true;
                resultForm.FileName = string.Format("{0}.{1}", obj_schema, obj_name);

                resultForm.Show();
            }
        }

        private void countDistinctToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mCurrentRow >= 0)
            {
                string obj_schema = dgvCode.Rows[mCurrentRow].Cells["obj_schema"].Value.ToString();
                string obj_name = dgvCode.Rows[mCurrentRow].Cells["obj_name"].Value.ToString();

                try
                {
                    string code = ScriptCommand(QueryHelper.QueryId.ScriptCountDistinct, obj_schema, obj_name);

                    ResultForm resultForm = new ResultForm();
                    resultForm.AppendTitle = string.Format(mTitleFormat, obj_schema, obj_name, cmbDatabase.Text, cmbServer.Text);
                    resultForm.ShowInTaskbar = true;
                    resultForm.Icon = this.Icon;
                    resultForm.ShowIcon = true;

                    resultForm.ConnectionString = BuildConnectionString(mDbName);
                    resultForm.CommandText = code;
                    resultForm.ExecutionTimeout = Properties.Settings.Default.QrsExecutionTimeout;

                    resultForm.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void missingIndexesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MissingIndexes(dgvCode.Rows[mCurrentRow].Cells["obj_name"].Value.ToString(), dgvCode.Rows[mCurrentRow].Cells["obj_schema"].Value.ToString());
        }

        private void statusToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            IndexesStatus(0, dgvCode.Rows[mCurrentRow].Cells["obj_name"].Value.ToString(), dgvCode.Rows[mCurrentRow].Cells["obj_schema"].Value.ToString());
        }

        private void limitedFragmentAnalysisToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            IndexesStatus(1, dgvCode.Rows[mCurrentRow].Cells["obj_name"].Value.ToString(), dgvCode.Rows[mCurrentRow].Cells["obj_schema"].Value.ToString());
        }

        private void sampledFragmentAnalysisToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            IndexesStatus(2, dgvCode.Rows[mCurrentRow].Cells["obj_name"].Value.ToString(), dgvCode.Rows[mCurrentRow].Cells["obj_schema"].Value.ToString());
        }

        private void detailedFragmentAnalysisToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            IndexesStatus(3, dgvCode.Rows[mCurrentRow].Cells["obj_name"].Value.ToString(), dgvCode.Rows[mCurrentRow].Cells["obj_schema"].Value.ToString());
        }

        private void updatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Version latestVersion = null;
            string downloadUrl = "";

            try
            {
                this.Cursor = Cursors.WaitCursor;

                using (var client = new WebClient())
                {
                    client.Headers.Add("User-Agent", "AppUpdateChecker");
                    string json = client.DownloadString(HttpUrl.AppVersionUrl);

                    var serializer = new JavaScriptSerializer();
                    var release = serializer.Deserialize<Dictionary<string, object>>(json);

                    // Parse version
                    string tagName = (string)release["tag_name"];
                    latestVersion = new Version(tagName.TrimStart('v', 'V'));

                    // Find the correct asset
                    string expectedAssetName = $"{HttpUrl.RepoName}-v{latestVersion}.zip".ToLower();
                    //downloadUrl = (string)release["html_url"]; // Fallback to release page

                    if (release.ContainsKey("assets") && release["assets"] is ArrayList assets)
                    {
                        foreach (Dictionary<string, object> asset in assets)
                        {
                            string assetName = ((string)asset["name"]).ToLower();
                            if (assetName == expectedAssetName)
                            {
                                downloadUrl = (string)asset["browser_download_url"];
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            if (latestVersion != null && downloadUrl != "")
            {
                Version curVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

                if (curVersion.CompareTo(latestVersion) < 0)
                {
                    if (DialogResult.Yes == MessageBox.Show("Newer version detected. Do you want to download it?", "Check for Updates", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        SaveFileDialog sfd = new SaveFileDialog();
                        sfd.OverwritePrompt = true;
                        sfd.FileName = downloadUrl.Substring(downloadUrl.LastIndexOf("/") + 1);
                        DialogResult dres = sfd.ShowDialog();

                        if (dres != DialogResult.OK)
                        {
                            return;
                        }

                        try
                        {
                            this.Cursor = Cursors.WaitCursor;

                            WebClient client = new WebClient();
                            client.UseDefaultCredentials = true;

                            client.DownloadFile(downloadUrl, sfd.FileName);

                            //System.Diagnostics.Process.Start(downloadUrl);
                        }
                        catch (Exception ex)
                        {
                            this.Cursor = Cursors.Default;
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            this.Cursor = Cursors.Default;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No updates available.", "Check for Updates", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void chlObjType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                for (int i = 0; i < chlObjType.Items.Count; i++)
                {
                    chlObjType.SetItemChecked(i, false);
                }
            }
            else if (e.KeyCode == Keys.Insert)
            {
                for (int i = 0; i < chlObjType.Items.Count; i++)
                {
                    chlObjType.SetItemChecked(i, true);
                }
            }
        }
    }

    #endregion
}
