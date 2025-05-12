using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SqlDbAid
{
    public partial class CompareForm : Form
    {
        int mCurrentRow = -1;

        string mLeftFileName = "";
        string mRightFileName = "";

        const string mInitText = "Select File";

        const string mMsgLeft = "Missing Object (Left)";
        const string mMsgRight = "Missing Object (Right)";
        const string mMsgMismatch = "Mismatch";
        const string mMsgMatch = "Match";

        Color mColorLeft = Color.LightSteelBlue;
        Color mColorRight = Color.Plum;
        Color mColorMismatch = Color.Gold;
        Color mColorMatch = Color.LightGreen;

        bool mLeftReady = false;
        bool mRightReady = false;

        DataTable dtLeft;
        DataTable dtRight;
        Hashtable htLeft;
        Hashtable htRight;

        public CompareForm()
        {
            InitializeComponent();
        }

        private void CopyToClipboard(bool IncludeHeaderText)
        {
            int cellCount = dgvResult.GetCellCount(DataGridViewElementStates.Selected);
            if (cellCount > 0)
            {
                if (IncludeHeaderText)
                {
                    dgvResult.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
                    Clipboard.SetDataObject(ClipboardHelper.RemoveFirstTab(dgvResult.GetClipboardContent().GetText()));
                }
                else
                {
                    dgvResult.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
                    Clipboard.SetDataObject(dgvResult.GetClipboardContent().GetText());
                }
            }
        }

        private void RefreshCompareStatus()
        {
            if (!mLeftReady)
            {
                txtLeftFile.Text = mInitText;
                tltCompare.SetToolTip(txtLeftFile, "");
            }
            else
            {
                txtLeftFile.Text = Path.GetFileNameWithoutExtension(mLeftFileName);
                tltCompare.SetToolTip(txtLeftFile, mLeftFileName);
            }

            if (!mRightReady)
            {
                txtRightFile.Text = mInitText;
                tltCompare.SetToolTip(txtRightFile, "");
            }
            else
            {
                txtRightFile.Text = Path.GetFileNameWithoutExtension(mRightFileName);
                tltCompare.SetToolTip(txtRightFile, mRightFileName);
            }

            if (mLeftReady && mRightReady)
            {
                btnCompare.Enabled = true;
            }
            else
            {
                btnCompare.Enabled = false;
            }

            ClearCompare();
        }

        private void ClearCompare()
        {
            cmpSql.Clear();
            lblCount.Text = "0";
            dgvResult.DataSource = new DataTable();
        }

        private void Compare()
        {
            DataTable objCompare = new DataTable();

            ClearCompare();

            htLeft = new Hashtable();
            htRight = new Hashtable();

            for (int i = 0; i < dtLeft.Rows.Count; i++)
            {
                string src = string.Format("{0}{1}{2}", dtLeft.Rows[i]["obj_type"].ToString(), dtLeft.Rows[i]["obj_schema"].ToString(), dtLeft.Rows[i]["obj_name"].ToString());
                if (chkIgnoreCase.Checked)
                {
                    src = src.ToLower();
                }

                htLeft.Add(src, i);
            }

            for (int i = 0; i < dtRight.Rows.Count; i++)
            {
                string src = string.Format("{0}{1}{2}", dtRight.Rows[i]["obj_type"].ToString(), dtRight.Rows[i]["obj_schema"].ToString(), dtRight.Rows[i]["obj_name"].ToString());
                if (chkIgnoreCase.Checked)
                {
                    src = src.ToLower();
                }

                htRight.Add(src, i);
            }

            objCompare.Columns.Add("Status", typeof(String));
            objCompare.Columns.Add("Type", typeof(String));
            objCompare.Columns.Add("Schema", typeof(String));
            objCompare.Columns.Add("Name", typeof(String));
            objCompare.Columns.Add("ModifiedOn(Left)", typeof(String));
            objCompare.Columns.Add("ModifiedOn(Right)", typeof(String));

            for (int i = 0; i < dtLeft.Rows.Count; i++)
            {
                string src = string.Format("{0}{1}{2}", dtLeft.Rows[i]["obj_type"].ToString(), dtLeft.Rows[i]["obj_schema"].ToString(), dtLeft.Rows[i]["obj_name"].ToString());
                if (chkIgnoreCase.Checked)
                {
                    src = src.ToLower();
                }

                if (!htRight.ContainsKey(src))
                {
                    string objStatus = mMsgRight;
                    string objType = dtLeft.Rows[i]["obj_type"].ToString();
                    string objSchema = dtLeft.Rows[i]["obj_schema"].ToString();
                    string objName = dtLeft.Rows[i]["obj_name"].ToString();
                    string objLeftModifiedOn = dtLeft.Rows[i]["modify_date"].ToString();
                    string objRightModifiedOn = "";

                    object[] row = { objStatus, objType, objSchema, objName, objLeftModifiedOn, objRightModifiedOn };

                    objCompare.Rows.Add(row);
                }
            }

            for (int i = 0; i < dtRight.Rows.Count; i++)
            {
                string src = string.Format("{0}{1}{2}", dtRight.Rows[i]["obj_type"].ToString(), dtRight.Rows[i]["obj_schema"].ToString(), dtRight.Rows[i]["obj_name"].ToString());
                if (chkIgnoreCase.Checked)
                {
                    src = src.ToLower();
                }

                if (!htLeft.ContainsKey(src))
                {
                    string objStatus = mMsgLeft;
                    string objType = dtRight.Rows[i]["obj_type"].ToString();
                    string objSchema = dtRight.Rows[i]["obj_schema"].ToString();
                    string objName = dtRight.Rows[i]["obj_name"].ToString();
                    string objLeftModifiedOn = "";
                    string objRightModifiedOn = dtRight.Rows[i]["modify_date"].ToString();

                    object[] row = { objStatus, objType, objSchema, objName, objLeftModifiedOn, objRightModifiedOn };

                    objCompare.Rows.Add(row);
                }
                else
                {
                    int j = (int)htLeft[src];

                    string left = dtLeft.Rows[j]["code"].ToString();
                    string right = dtRight.Rows[i]["code"].ToString();

                    if (chkIgnoreCase.Checked && chkIgnoreEmptyLines.Checked)
                    {
                        left = left.ToLower();
                        right = right.ToLower();
                    }

                    if (chkIgnoreEmptyLines.Checked)
                    {
                        left = Regex.Replace(left.Replace("\r", "") + "\n", @"(^\s*[\n|\r])|(^\s*)", "", RegexOptions.Multiline);
                        right = Regex.Replace(right.Replace("\r", "") + "\n", @"(^\s*[\n|\r])|(^\s*)", "", RegexOptions.Multiline);
                    }

                    if (left != right)
                    {
                        string objStatus = mMsgMismatch;
                        string objType = dtRight.Rows[i]["obj_type"].ToString();
                        string objSchema = dtRight.Rows[i]["obj_schema"].ToString();
                        string objName = dtRight.Rows[i]["obj_name"].ToString();
                        string objLeftModifiedOn = dtLeft.Rows[j]["modify_date"].ToString();
                        string objRightModifiedOn = dtRight.Rows[i]["modify_date"].ToString();

                        object[] row = { objStatus, objType, objSchema, objName, objLeftModifiedOn, objRightModifiedOn };

                        objCompare.Rows.Add(row);
                    }
                    else if (!chkHideMatches.Checked)
                    {
                        string objStatus = mMsgMatch;
                        string objType = dtRight.Rows[i]["obj_type"].ToString();
                        string objSchema = dtRight.Rows[i]["obj_schema"].ToString();
                        string objName = dtRight.Rows[i]["obj_name"].ToString();
                        string objLeftModifiedOn = dtLeft.Rows[j]["modify_date"].ToString();
                        string objRightModifiedOn = dtRight.Rows[i]["modify_date"].ToString();

                        object[] row = { objStatus, objType, objSchema, objName, objLeftModifiedOn, objRightModifiedOn };

                        objCompare.Rows.Add(row);
                    }

                }
            }

            objCompare.DefaultView.Sort = "Type,Schema,Name";
            dgvResult.DataSource = objCompare;

            lblCount.Text = dgvResult.RowCount.ToString();

            if (dgvResult.RowCount > 0)
            {
                mCurrentRow = 0;
                ShowComparison();
            }
        }

        private void ShowComparison()
        {
            if (mCurrentRow >= 0)
            {
                cmpSql.LeftText = "";
                cmpSql.RightText = "";

                string src = string.Format("{0}{1}{2}", dgvResult.Rows[mCurrentRow].Cells["Type"].Value.ToString(), dgvResult.Rows[mCurrentRow].Cells["Schema"].Value.ToString(), dgvResult.Rows[mCurrentRow].Cells["Name"].Value.ToString());
                if (chkIgnoreCase.Checked)
                {
                    src = src.ToLower();
                }

                if (htLeft.ContainsKey(src))
                {
                    int i = (int)htLeft[src];
                    cmpSql.LeftText = dtLeft.Rows[i]["code"].ToString();
                }

                if (htRight.ContainsKey(src))
                {
                    int i = (int)htRight[src];
                    cmpSql.RightText = dtRight.Rows[i]["code"].ToString();
                }

                cmpSql.CaseSensitive = !chkIgnoreCase.Checked;
                cmpSql.CompareText();
            }
        }

        private void dgvResult_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                dgvResult.ContextMenuStrip = datagridMenu;
            }
        }

        private void dgvResult_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DataGridView.HitTestInfo hti = dgvResult.HitTest(e.X, e.Y);

                mCurrentRow = hti.RowIndex;

                ShowComparison();
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

        private void btnLeftFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                mLeftFileName = openFileDialog.FileName;

                try
                {
                    dtLeft = new DataTable();

                    dtLeft.ReadXml(mLeftFileName);

                    mLeftReady = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    mLeftReady = false;
                }

                RefreshCompareStatus();
            }
        }

        private void btnRightFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                mRightFileName = openFileDialog.FileName;

                try
                {
                    dtRight = new DataTable();

                    dtRight.ReadXml(mRightFileName);

                    mRightReady = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    mRightReady = false;
                }

                RefreshCompareStatus();
            }
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            try
            {
                Compare();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvResult_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            string ht = dgvResult.Columns[e.ColumnIndex].HeaderText;

            if (ht == "Status")
            {
                string status = dgvResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                if (!string.IsNullOrEmpty(status))
                {
                    switch (status)
                    {
                        case mMsgLeft:
                            e.CellStyle.BackColor = mColorLeft;
                            break;
                        case mMsgRight:
                            e.CellStyle.BackColor = mColorRight;
                            break;
                        case mMsgMismatch:
                            e.CellStyle.BackColor = mColorMismatch;
                            break;
                        case mMsgMatch:
                            e.CellStyle.BackColor = mColorMatch;
                            break;
                    }
                }
            }

            if (ht == "ModifiedOn(Left)" || ht == "ModifiedOn(Right)")
            {
                int sc = string.Compare(dgvResult.Rows[e.RowIndex].Cells["ModifiedOn(Left)"].Value.ToString(), dgvResult.Rows[e.RowIndex].Cells["ModifiedOn(Right)"].Value.ToString());
                if (sc > 0 && dgvResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "" && ht == "ModifiedOn(Left)")
                {
                    e.CellStyle.BackColor = mColorLeft;
                }
                else if (sc < 0 && dgvResult.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "" && ht == "ModifiedOn(Right)")
                {
                    e.CellStyle.BackColor = mColorRight;
                }
            }
        }

        private void CompareForm_Load(object sender, EventArgs e)
        {
            txtLeftFile.Text = mInitText;
            txtLeftFile.BackColor = mColorLeft;
            txtRightFile.Text = mInitText;
            txtRightFile.BackColor = mColorRight;
        }
    }
}
