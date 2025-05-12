using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32; //registry

namespace SqlDbAid
{
    public partial class OptionForm : Form
    {
        private void AddServer()
        {
            if (txtServer.Text != "" && !lstServer.Items.Contains(txtServer.Text.ToUpper()))
            {
                lstServer.Items.Add(txtServer.Text.ToUpper());
            }
        }

        public OptionForm()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ScrAddDropCommand = chkAddDropCommand.Checked;
            Properties.Settings.Default.ScrSingleFileExport = chkSingleFileExport.Checked;
            Properties.Settings.Default.ScrIncludeIndexes = chkIncludeIndexes.Checked;
            Properties.Settings.Default.ScrHideSystemDbs = chkHideSystemDbs.Checked;

            Properties.Settings.Default.QrsConnectionTimeout = (int)(numConnectionTimeout.Value);
            Properties.Settings.Default.QrsExecutionTimeout = (int)(numExecutionTimeout.Value);
            Properties.Settings.Default.QrsOpenTopRows = (int)(numOpenTopRows.Value);
            Properties.Settings.Default.QrsOpenWithNoLock = chkOpenWithNoLock.Checked;

            Properties.Settings.Default.TlsHideAppQueries = chkHideAppQueries.Checked;
            Properties.Settings.Default.TlsGetLockedObjectName = chkGetLockedObjectName.Checked;
            Properties.Settings.Default.TlsTopQueriesWithin = (int)(numTopQueriesWithin.Value);

            string serverList = "";

            for (int i = 0; i < lstServer.Items.Count; i++)
            {
                if (serverList != "")
                {
                    serverList = serverList + "#";
                }
                serverList = serverList + lstServer.Items[i].ToString();
            }

            Properties.Settings.Default.SrvServers = serverList;

            this.Close();
        }

        private void OptionForm_Load(object sender, EventArgs e)
        {
            chkAddDropCommand.Checked = Properties.Settings.Default.ScrAddDropCommand;
            chkSingleFileExport.Checked = Properties.Settings.Default.ScrSingleFileExport;
            chkIncludeIndexes.Checked = Properties.Settings.Default.ScrIncludeIndexes;
            chkHideSystemDbs.Checked = Properties.Settings.Default.ScrHideSystemDbs;

            numConnectionTimeout.Value = Properties.Settings.Default.QrsConnectionTimeout;
            numExecutionTimeout.Value = Properties.Settings.Default.QrsExecutionTimeout;
            numOpenTopRows.Value = Properties.Settings.Default.QrsOpenTopRows;
            chkOpenWithNoLock.Checked = Properties.Settings.Default.QrsOpenWithNoLock;

            chkHideAppQueries.Checked = Properties.Settings.Default.TlsHideAppQueries;
            chkGetLockedObjectName.Checked = Properties.Settings.Default.TlsGetLockedObjectName;
            numTopQueriesWithin.Value = Properties.Settings.Default.TlsTopQueriesWithin;

            string[] values = Properties.Settings.Default.SrvServers.Split('#');

            foreach (string value in values)
            {
                if (value != "")
                {
                    lstServer.Items.Add(value);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddServer();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lstServer.SelectedIndex >= 0)
            {
                lstServer.Items.RemoveAt(lstServer.SelectedIndex);
            }
        }

        private void txtServer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                AddServer();
                txtServer.Text = "";
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lstServer.Items.Clear();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                using (DataTable dt = System.Data.Sql.SqlDataSourceEnumerator.Instance.GetDataSources())
                {
                    foreach (System.Data.DataRow row in dt.Rows)
                    {
                        string[] versionPart = row["version"].ToString().Split('.');
                        int versionNum = 0;
                        int.TryParse(versionPart[0], out versionNum);

                        if (versionNum > 8)
                        {
                            string serverName = row["servername"].ToString();
                            string instanceName = row["instancename"].ToString();
                            string fullServerName = serverName.ToUpper();

                            if (instanceName != "")
                            {
                                fullServerName = string.Format("{0}\\{1}", serverName, instanceName).ToUpper();
                            }

                            if (!lstServer.Items.Contains(fullServerName))
                            {
                                lstServer.Items.Add(fullServerName);
                            }
                        }
                    }
                }

                RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL");

                if (rk != null)
                {
                    string serverName = System.Environment.MachineName.ToUpper();

                    foreach (string instanceName in rk.GetValueNames())
                    {
                        string fullServerName = serverName.ToUpper();

                        if (instanceName.ToUpper() != "MSSQLSERVER")
                        {
                            fullServerName = string.Format("{0}\\{1}", serverName, instanceName).ToUpper();
                        }

                        if (!lstServer.Items.Contains(fullServerName))
                        {
                            lstServer.Items.Add(fullServerName);
                        }
                    }
                }
            }
            catch
            {
            }

            this.Cursor = Cursors.Default;

        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (lstServer.Items.Count > 0)
            {
                DialogResult dres;

                exportFileBrowser.OverwritePrompt = true;
                exportFileBrowser.Filter = "Txt files|*.txt|All files|*.*";
                exportFileBrowser.FileName = "ServerList";
                dres = exportFileBrowser.ShowDialog();

                if (dres == DialogResult.OK)
                {
                    this.Cursor = Cursors.WaitCursor;

                    try
                    {
                        using (StreamWriter sw = new StreamWriter(exportFileBrowser.FileName))
                        {
                            for (int i = 0; i < lstServer.Items.Count; i++)
                            {
                                sw.WriteLine(lstServer.Items[i].ToString());
                            }
                        }
                        MessageBox.Show("Servers Exported: " + lstServer.Items.Count.ToString(), "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default;
                    }
                }
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            DialogResult dres;

            importFile.Filter = "Txt files|*.txt|All files|*.*";
            importFile.FileName = "ServerList";
            dres = importFile.ShowDialog();

            if (dres == DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;

                try
                {
                    int objCount = 0;

                    using (StreamReader sr = new StreamReader(importFile.FileName))
                    {
                        string line;
                        string fullServerName;
                        while ((line = sr.ReadLine()) != null && objCount < 500)
                        {
                            fullServerName = line.Length > 50 ? line.Substring(0, 50).ToUpper() : line.ToUpper();

                            if (!lstServer.Items.Contains(fullServerName))
                            {
                                lstServer.Items.Add(fullServerName);
                                objCount++;
                            }
                        }
                    }
                    MessageBox.Show("Servers Imported: " + objCount.ToString(), "Import", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }
    }
}
