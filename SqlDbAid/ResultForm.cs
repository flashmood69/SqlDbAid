using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SqlDbAid
{
    public partial class ResultForm : Form
    {
        private const int MAX_AUTOMATIC_SYNTAX_LENGTH = 100000;

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
        ToolStripMenuItem exportResultsToolStripMenuItem;
        private readonly SqlHighlighter sqlHighlighter = new SqlHighlighter();
        private Timer syntaxHighlightTimer;
        private bool mApplyingSyntaxHighlight;
        private string mQuerySearchText = string.Empty;
        private string mQueryReplacementText = string.Empty;
        private bool mQuerySearchUsesRegex;
        private MatchCollection mQuerySearchMatches;
        private int mCurrentQuerySearchMatch = -1;
        private ContextMenuStrip mQueryEditorMenu;
        private ToolStripMenuItem mQueryUndoMenuItem;
        private ToolStripMenuItem mQueryRedoMenuItem;
        private ToolStripMenuItem mQueryFindNextMenuItem;
        private ToolStripMenuItem mQueryFindPreviousMenuItem;
        private readonly LinkedList<QueryEditorState> mQueryUndoHistory = new LinkedList<QueryEditorState>();
        private readonly LinkedList<QueryEditorState> mQueryRedoHistory = new LinkedList<QueryEditorState>();
        private QueryEditorState mLastQueryEditorState;
        private int mQueryHistoryCharacters;
        private bool mRestoringQueryHistory;

        private const int MAX_QUERY_HISTORY_STEPS = 40;
        private const int MAX_QUERY_HISTORY_CHARACTERS = 500000;

        private sealed class QueryEditorState
        {
            public string Text;
            public int SelectionStart;
            public int SelectionLength;
        }

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
            dataExporter.ClearResult();
            exportResultsToolStripMenuItem.Enabled = false;
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

        private static string FormatExportValue(object value, string delimiter)
        {
            if (value == null || value == DBNull.Value)
            {
                return "";
            }

            string text;
            byte[] binary = value as byte[];
            if (binary != null)
            {
                text = "0x" + BitConverter.ToString(binary).Replace("-", "");
            }
            else
            {
                IFormattable formattable = value as IFormattable;
                text = formattable == null
                    ? value.ToString()
                    : formattable.ToString(null, CultureInfo.InvariantCulture);
            }

            if (text.Contains(delimiter) || text.Contains("\"") || text.Contains("\r") || text.Contains("\n"))
            {
                return "\"" + text.Replace("\"", "\"\"") + "\"";
            }

            return text;
        }

        private void ExportResults()
        {
            DataTable result = dataExporter == null ? null : dataExporter.ResultTable;
            if (result == null || result.Columns.Count == 0)
            {
                return;
            }

            string originalFilter = exportFileBrowser.Filter;
            string originalDefaultExt = exportFileBrowser.DefaultExt;
            exportFileBrowser.Filter = "CSV files (*.csv)|*.csv|Tab-delimited files (*.tsv)|*.tsv|All files (*.*)|*.*";
            exportFileBrowser.FilterIndex = 1;
            exportFileBrowser.DefaultExt = "csv";
            exportFileBrowser.FileName = FileHelper.CleanFileName(string.IsNullOrEmpty(mFileName) ? "Results" : mFileName);

            try
            {
                if (exportFileBrowser.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                string delimiter = exportFileBrowser.FilterIndex == 2 ? "\t" : ",";
                using (StreamWriter writer = new StreamWriter(exportFileBrowser.FileName, false, new UTF8Encoding(true)))
                {
                    for (int columnIndex = 0; columnIndex < result.Columns.Count; columnIndex++)
                    {
                        if (columnIndex > 0)
                        {
                            writer.Write(delimiter);
                        }
                        writer.Write(FormatExportValue(result.Columns[columnIndex].ColumnName, delimiter));
                    }
                    writer.WriteLine();

                    foreach (DataRow row in result.Rows)
                    {
                        for (int columnIndex = 0; columnIndex < result.Columns.Count; columnIndex++)
                        {
                            if (columnIndex > 0)
                            {
                                writer.Write(delimiter);
                            }
                            writer.Write(FormatExportValue(row[columnIndex], delimiter));
                        }
                        writer.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Export Results", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                exportFileBrowser.Filter = originalFilter;
                exportFileBrowser.DefaultExt = originalDefaultExt;
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
                    ResetQueryEditHistory();
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
            txtQuery.HideSelection = false;
            InitializeQuerySearchMenu();
            txtQuery.TextChanged += txtQuery_TextChanged;
            syntaxHighlightTimer = new Timer(components);
            syntaxHighlightTimer.Interval = 300;
            syntaxHighlightTimer.Tick += syntaxHighlightTimer_Tick;
            ResetQueryEditHistory();
        }

        private void ResultForm_Load(object sender, EventArgs e)
        {
            dgvResult.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;

            exportResultsToolStripMenuItem = new ToolStripMenuItem("Export Results...");
            exportResultsToolStripMenuItem.Click += new EventHandler(exportResultsToolStripMenuItem_Click);
            datagridMenu.Items.Insert(2, exportResultsToolStripMenuItem);

            dataExporter = new DataExporter();
            dataExporter.WorkerReportsProgress = true;
            dataExporter.WorkerSupportsCancellation = true;
            dataExporter.ExportType = DataExporter.OutputType.Table;
            dataExporter.CollectSqlMessages = mShowQuery;
            dataExporter.ProgressChanged += new ProgressChangedEventHandler(dataExporter_ProgressChanged);
            dataExporter.RunWorkerCompleted += new RunWorkerCompletedEventHandler(dataExporter_RunWorkerCompleted);

            txtQuery.Text = mCommandText;
            mModified = false;
            ResetQueryEditHistory();
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

        private void InitializeQuerySearchMenu()
        {
            mQueryEditorMenu = new ContextMenuStrip(components);
            mQueryUndoMenuItem = new ToolStripMenuItem("&Undo");
            mQueryUndoMenuItem.ShortcutKeyDisplayString = "Ctrl+Z";
            mQueryUndoMenuItem.Click += (sender, e) => UndoQueryEdit();

            mQueryRedoMenuItem = new ToolStripMenuItem("&Redo");
            mQueryRedoMenuItem.ShortcutKeyDisplayString = "Ctrl+Y";
            mQueryRedoMenuItem.Click += (sender, e) => RedoQueryEdit();

            ToolStripMenuItem findMenuItem = new ToolStripMenuItem("&Find...");
            findMenuItem.ShortcutKeyDisplayString = "Ctrl+F";
            findMenuItem.Click += queryFindToolStripMenuItem_Click;

            ToolStripMenuItem replaceMenuItem = new ToolStripMenuItem("&Replace...");
            replaceMenuItem.ShortcutKeyDisplayString = "Ctrl+H";
            replaceMenuItem.Click += queryReplaceToolStripMenuItem_Click;

            mQueryFindNextMenuItem = new ToolStripMenuItem("Find &Next");
            mQueryFindNextMenuItem.Enabled = false;
            mQueryFindNextMenuItem.Click += queryFindNextToolStripMenuItem_Click;

            mQueryFindPreviousMenuItem = new ToolStripMenuItem("Find &Previous");
            mQueryFindPreviousMenuItem.Enabled = false;
            mQueryFindPreviousMenuItem.Click += queryFindPreviousToolStripMenuItem_Click;

            mQueryEditorMenu.Items.AddRange(new ToolStripItem[]
            {
                mQueryUndoMenuItem,
                mQueryRedoMenuItem,
                new ToolStripSeparator(),
                findMenuItem,
                replaceMenuItem,
                new ToolStripSeparator(),
                mQueryFindNextMenuItem,
                mQueryFindPreviousMenuItem
            });
            mQueryEditorMenu.Opening += (sender, e) => UpdateQueryEditMenuItems();
            txtQuery.ContextMenuStrip = mQueryEditorMenu;
        }

        private QueryEditorState CaptureQueryEditorState()
        {
            return new QueryEditorState
            {
                Text = txtQuery.Text,
                SelectionStart = txtQuery.SelectionStart,
                SelectionLength = txtQuery.SelectionLength
            };
        }

        private void ResetQueryEditHistory()
        {
            mQueryUndoHistory.Clear();
            mQueryRedoHistory.Clear();
            mQueryHistoryCharacters = 0;
            mLastQueryEditorState = txtQuery.TextLength <= MAX_QUERY_HISTORY_CHARACTERS
                ? CaptureQueryEditorState()
                : null;
            UpdateQueryEditMenuItems();
        }

        private void RecordQueryEdit()
        {
            if (mRestoringQueryHistory)
            {
                return;
            }

            if (txtQuery.TextLength > MAX_QUERY_HISTORY_CHARACTERS)
            {
                ResetQueryEditHistory();
                return;
            }

            QueryEditorState currentState = CaptureQueryEditorState();
            if (mLastQueryEditorState != null &&
                !string.Equals(mLastQueryEditorState.Text, currentState.Text, StringComparison.Ordinal))
            {
                mQueryUndoHistory.AddLast(mLastQueryEditorState);
                mQueryHistoryCharacters += mLastQueryEditorState.Text.Length;
                ClearQueryHistory(mQueryRedoHistory);
            }

            mLastQueryEditorState = currentState;
            TrimQueryEditHistory();
            UpdateQueryEditMenuItems();
        }

        private void ClearQueryHistory(LinkedList<QueryEditorState> history)
        {
            foreach (QueryEditorState state in history)
            {
                mQueryHistoryCharacters -= state.Text.Length;
            }
            history.Clear();
        }

        private void TrimQueryEditHistory()
        {
            while (mQueryHistoryCharacters > MAX_QUERY_HISTORY_CHARACTERS ||
                mQueryUndoHistory.Count + mQueryRedoHistory.Count > MAX_QUERY_HISTORY_STEPS)
            {
                LinkedList<QueryEditorState> oldestHistory =
                    mQueryUndoHistory.First == null ? mQueryRedoHistory : mQueryUndoHistory;
                if (oldestHistory.First == null)
                {
                    break;
                }

                mQueryHistoryCharacters -= oldestHistory.First.Value.Text.Length;
                oldestHistory.RemoveFirst();
            }
        }

        private void UndoQueryEdit()
        {
            if (mQueryUndoHistory.Last == null)
            {
                return;
            }

            QueryEditorState state = mQueryUndoHistory.Last.Value;
            mQueryUndoHistory.RemoveLast();
            mQueryHistoryCharacters -= state.Text.Length;

            QueryEditorState currentState = CaptureQueryEditorState();
            mQueryRedoHistory.AddLast(currentState);
            mQueryHistoryCharacters += currentState.Text.Length;
            RestoreQueryEditorState(state);
        }

        private void RedoQueryEdit()
        {
            if (mQueryRedoHistory.Last == null)
            {
                return;
            }

            QueryEditorState state = mQueryRedoHistory.Last.Value;
            mQueryRedoHistory.RemoveLast();
            mQueryHistoryCharacters -= state.Text.Length;

            QueryEditorState currentState = CaptureQueryEditorState();
            mQueryUndoHistory.AddLast(currentState);
            mQueryHistoryCharacters += currentState.Text.Length;
            RestoreQueryEditorState(state);
        }

        private void RestoreQueryEditorState(QueryEditorState state)
        {
            mRestoringQueryHistory = true;
            try
            {
                txtQuery.Text = state.Text;
                txtQuery.Select(
                    Math.Min(state.SelectionStart, txtQuery.TextLength),
                    Math.Min(state.SelectionLength, txtQuery.TextLength - Math.Min(state.SelectionStart, txtQuery.TextLength)));
                mLastQueryEditorState = state;
            }
            finally
            {
                mRestoringQueryHistory = false;
            }

            try
            {
                UpdateQuerySearchMatches();
            }
            catch (ArgumentException)
            {
                mQuerySearchMatches = null;
            }
            catch (RegexMatchTimeoutException)
            {
                mQuerySearchMatches = null;
            }

            if (!mModified)
            {
                mModified = true;
                SetTitle();
            }

            syntaxHighlightTimer.Stop();
            if (mShowQuery && txtQuery.TextLength <= MAX_AUTOMATIC_SYNTAX_LENGTH)
            {
                syntaxHighlightTimer.Start();
            }

            UpdateQueryEditMenuItems();
        }

        private void UpdateQueryEditMenuItems()
        {
            if (mQueryUndoMenuItem == null)
            {
                return;
            }

            mQueryUndoMenuItem.Enabled = mQueryUndoHistory.Count > 0;
            mQueryRedoMenuItem.Enabled = mQueryRedoHistory.Count > 0;
        }

        private DialogResult ShowQueryFindDialog(ref string searchText, ref bool useRegex)
        {
            using (Form dialog = new Form())
            using (Label prompt = new Label())
            using (TextBox searchBox = new TextBox())
            using (CheckBox regexCheckBox = new CheckBox())
            using (Button findButton = new Button())
            using (Button cancelButton = new Button())
            {
                dialog.Text = "Find";
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.MinimizeBox = false;
                dialog.MaximizeBox = false;
                dialog.ShowInTaskbar = false;
                dialog.ClientSize = new Size(396, 107);

                prompt.AutoSize = true;
                prompt.Location = new Point(12, 12);
                prompt.Text = "Find what:";

                searchBox.Location = new Point(12, 32);
                searchBox.Size = new Size(372, 20);
                searchBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                searchBox.Text = searchText;

                regexCheckBox.AutoSize = true;
                regexCheckBox.Location = new Point(12, 70);
                regexCheckBox.Text = "Regular Expression";
                regexCheckBox.Checked = useRegex;

                findButton.Location = new Point(228, 72);
                findButton.Size = new Size(75, 23);
                findButton.Text = "Find Next";
                findButton.DialogResult = DialogResult.OK;
                findButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

                cancelButton.Location = new Point(309, 72);
                cancelButton.Size = new Size(75, 23);
                cancelButton.Text = "Cancel";
                cancelButton.DialogResult = DialogResult.Cancel;
                cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

                dialog.Controls.AddRange(new Control[]
                {
                    prompt, searchBox, regexCheckBox, findButton, cancelButton
                });
                dialog.AcceptButton = findButton;
                dialog.CancelButton = cancelButton;

                DialogResult result = dialog.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    searchText = searchBox.Text;
                    useRegex = regexCheckBox.Checked;
                }

                return result;
            }
        }

        private void FindInQuery()
        {
            string searchText = mQuerySearchText;
            bool useRegex = mQuerySearchUsesRegex;
            if (ShowQueryFindDialog(ref searchText, ref useRegex) != DialogResult.OK)
            {
                return;
            }

            try
            {
                mQuerySearchText = searchText;
                mQuerySearchUsesRegex = useRegex;
                UpdateQuerySearchMatches();
                if (mQuerySearchMatches == null || mQuerySearchMatches.Count == 0)
                {
                    MessageBox.Show(this, "No matches found.", "Find", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SelectQuerySearchMatch(1);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(this, "The search expression is invalid.\r\n" + ex.Message, "Find", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (RegexMatchTimeoutException)
            {
                MessageBox.Show(this, "The search exceeded the 250 ms limit. Please simplify the expression.", "Find", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ReplaceInQuery()
        {
            using (Form dialog = new Form())
            using (Label findLabel = new Label())
            using (TextBox findBox = new TextBox())
            using (Label replaceLabel = new Label())
            using (TextBox replaceBox = new TextBox())
            using (CheckBox regexCheckBox = new CheckBox())
            using (Button findNextButton = new Button())
            using (Button replaceButton = new Button())
            using (Button replaceAllButton = new Button())
            using (Button closeButton = new Button())
            {
                dialog.Text = "Replace";
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.MinimizeBox = false;
                dialog.MaximizeBox = false;
                dialog.ShowInTaskbar = false;
                dialog.ClientSize = new Size(440, 164);

                findLabel.AutoSize = true;
                findLabel.Location = new Point(12, 12);
                findLabel.Text = "Find what:";
                findBox.Location = new Point(92, 9);
                findBox.Size = new Size(336, 20);
                findBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                findBox.Text = mQuerySearchText;

                replaceLabel.AutoSize = true;
                replaceLabel.Location = new Point(12, 42);
                replaceLabel.Text = "Replace with:";
                replaceBox.Location = new Point(92, 39);
                replaceBox.Size = new Size(336, 20);
                replaceBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                replaceBox.Text = mQueryReplacementText;

                regexCheckBox.AutoSize = true;
                regexCheckBox.Location = new Point(12, 72);
                regexCheckBox.Text = "Regular Expression";
                regexCheckBox.Checked = mQuerySearchUsesRegex;

                findNextButton.Location = new Point(66, 126);
                findNextButton.Size = new Size(90, 25);
                findNextButton.Text = "Find Next";
                findNextButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

                replaceButton.Location = new Point(162, 126);
                replaceButton.Size = new Size(78, 25);
                replaceButton.Text = "Replace";
                replaceButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

                replaceAllButton.Location = new Point(246, 126);
                replaceAllButton.Size = new Size(90, 25);
                replaceAllButton.Text = "Replace All";
                replaceAllButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

                closeButton.Location = new Point(342, 126);
                closeButton.Size = new Size(86, 25);
                closeButton.Text = "Close";
                closeButton.DialogResult = DialogResult.Cancel;
                closeButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

                Action updateSearch = () =>
                {
                    mQuerySearchText = findBox.Text;
                    mQueryReplacementText = replaceBox.Text;
                    mQuerySearchUsesRegex = regexCheckBox.Checked;
                    UpdateQuerySearchMatches();
                };

                findNextButton.Click += (sender, e) =>
                {
                    try
                    {
                        updateSearch();
                        if (mQuerySearchMatches == null || mQuerySearchMatches.Count == 0)
                        {
                            MessageBox.Show(dialog, "No matches found.", "Find", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        mCurrentQuerySearchMatch = FindSelectedQueryMatchIndex();
                        SelectQuerySearchMatch(1, false);
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show(dialog, "The search expression is invalid.\r\n" + ex.Message, "Find", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    catch (RegexMatchTimeoutException)
                    {
                        MessageBox.Show(dialog, "The search exceeded the 250 ms limit. Please simplify the expression.", "Find", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };

                replaceButton.Click += (sender, e) =>
                {
                    try
                    {
                        updateSearch();
                        ReplaceCurrentQueryMatch();
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show(dialog, "The search expression is invalid.\r\n" + ex.Message, "Replace", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    catch (RegexMatchTimeoutException)
                    {
                        MessageBox.Show(dialog, "The search exceeded the 250 ms limit. Please simplify the expression.", "Replace", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };

                replaceAllButton.Click += (sender, e) =>
                {
                    try
                    {
                        updateSearch();
                        ReplaceAllQueryMatches(dialog);
                    }
                    catch (ArgumentException ex)
                    {
                        MessageBox.Show(dialog, "The search expression is invalid.\r\n" + ex.Message, "Replace", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    catch (RegexMatchTimeoutException)
                    {
                        MessageBox.Show(dialog, "The search exceeded the 250 ms limit. Please simplify the expression.", "Replace", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };

                dialog.Controls.AddRange(new Control[]
                {
                    findLabel, findBox, replaceLabel, replaceBox,
                    regexCheckBox, findNextButton, replaceButton,
                    replaceAllButton, closeButton
                });
                dialog.AcceptButton = findNextButton;
                dialog.CancelButton = closeButton;
                dialog.ShowDialog(this);
            }
        }

        private void ReplaceCurrentQueryMatch()
        {
            if (mQuerySearchMatches == null || mQuerySearchMatches.Count == 0)
            {
                MessageBox.Show(this, "No matches found.", "Replace", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int selectedMatch = FindSelectedQueryMatchIndex();

            if (selectedMatch < 0)
            {
                selectedMatch = FindQueryMatchAtOrAfter(txtQuery.SelectionStart);
            }

            Match target = mQuerySearchMatches[selectedMatch];
            string replacement = mQuerySearchUsesRegex
                ? target.Result(mQueryReplacementText)
                : mQueryReplacementText;
            txtQuery.Select(target.Index, target.Length);
            txtQuery.SelectedText = replacement;

            SelectQueryMatchAtOrAfter(target.Index + replacement.Length, false);
        }

        private void ReplaceAllQueryMatches(IWin32Window owner)
        {
            if (mQuerySearchMatches == null || mQuerySearchMatches.Count == 0)
            {
                MessageBox.Show(owner, "No matches found.", "Replace", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int count = mQuerySearchMatches.Count;
            int selectionStart = txtQuery.SelectionStart;
            Regex regex = new Regex(
                mQuerySearchUsesRegex ? mQuerySearchText : Regex.Escape(mQuerySearchText),
                RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase,
                TimeSpan.FromMilliseconds(250));
            string updatedText = mQuerySearchUsesRegex
                ? regex.Replace(txtQuery.Text, mQueryReplacementText)
                : regex.Replace(txtQuery.Text, match => mQueryReplacementText);

            txtQuery.Text = updatedText;
            txtQuery.SelectionStart = Math.Min(selectionStart, txtQuery.TextLength);
            txtQuery.SelectionLength = 0;
            MessageBox.Show(
                owner,
                string.Format("Replaced {0} occurrence(s).", count),
                "Replace",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private int FindQueryMatchAtOrAfter(int position)
        {
            for (int i = 0; i < mQuerySearchMatches.Count; i++)
            {
                if (mQuerySearchMatches[i].Index >= position)
                {
                    return i;
                }
            }

            return 0;
        }

        private int FindSelectedQueryMatchIndex()
        {
            if (mQuerySearchMatches == null)
            {
                return -1;
            }

            for (int i = 0; i < mQuerySearchMatches.Count; i++)
            {
                Match match = mQuerySearchMatches[i];
                if (match.Index == txtQuery.SelectionStart &&
                    match.Length == txtQuery.SelectionLength)
                {
                    return i;
                }
            }

            return -1;
        }

        private void SelectQueryMatchAtOrAfter(int position, bool focusEditor = true)
        {
            UpdateQuerySearchMatches();
            if (mQuerySearchMatches == null || mQuerySearchMatches.Count == 0)
            {
                return;
            }

            int index = FindQueryMatchAtOrAfter(position);
            mCurrentQuerySearchMatch = index - 1;
            SelectQuerySearchMatch(1, focusEditor);
        }

        private void UpdateQuerySearchMatches()
        {
            mQuerySearchMatches = null;
            mCurrentQuerySearchMatch = -1;
            mQueryFindNextMenuItem.Enabled = false;
            mQueryFindPreviousMenuItem.Enabled = false;
            if (string.IsNullOrEmpty(mQuerySearchText))
            {
                return;
            }

            string pattern = mQuerySearchUsesRegex
                ? mQuerySearchText
                : Regex.Escape(mQuerySearchText);
            Regex searchRegex = new Regex(
                pattern,
                RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase,
                TimeSpan.FromMilliseconds(250));
            mQuerySearchMatches = searchRegex.Matches(txtQuery.Text);
            bool hasMatches = mQuerySearchMatches.Count > 0;
            mQueryFindNextMenuItem.Enabled = hasMatches;
            mQueryFindPreviousMenuItem.Enabled = hasMatches;
        }

        private void SelectQuerySearchMatch(int direction, bool focusEditor = true)
        {
            if (mQuerySearchMatches == null || mQuerySearchMatches.Count == 0)
            {
                return;
            }

            mCurrentQuerySearchMatch += direction;
            if (mCurrentQuerySearchMatch >= mQuerySearchMatches.Count)
            {
                mCurrentQuerySearchMatch = 0;
            }
            else if (mCurrentQuerySearchMatch < 0)
            {
                mCurrentQuerySearchMatch = mQuerySearchMatches.Count - 1;
            }

            Match match = mQuerySearchMatches[mCurrentQuerySearchMatch];
            txtQuery.Select(match.Index, match.Length);
            if (focusEditor)
            {
                txtQuery.Focus();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (mShowQuery && txtQuery.Visible && keyData == (Keys.Control | Keys.Z))
            {
                UndoQueryEdit();
                return true;
            }

            if (mShowQuery && txtQuery.Visible && keyData == (Keys.Control | Keys.Y))
            {
                RedoQueryEdit();
                return true;
            }

            if (mShowQuery && txtQuery.Visible && keyData == (Keys.Control | Keys.F))
            {
                FindInQuery();
                return true;
            }

            if (mShowQuery && txtQuery.Visible && keyData == (Keys.Control | Keys.H))
            {
                ReplaceInQuery();
                return true;
            }

            if (mShowQuery && txtQuery.Visible &&
                (keyData == Keys.F3 || keyData == (Keys.Shift | Keys.F3)))
            {
                if (mQuerySearchMatches != null && mQuerySearchMatches.Count > 0)
                {
                    SelectQuerySearchMatch(keyData == Keys.F3 ? 1 : -1);
                }

                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void queryFindToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FindInQuery();
        }

        private void queryReplaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReplaceInQuery();
        }

        private void queryFindNextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectQuerySearchMatch(1);
        }

        private void queryFindPreviousToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectQuerySearchMatch(-1);
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

            exportResultsToolStripMenuItem.Enabled =
                dataExporter.ResultTable != null && dataExporter.ResultTable.Columns.Count > 0;
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
            exportResultsToolStripMenuItem.Enabled =
                !mExporting && dataExporter.ResultTable != null && dataExporter.ResultTable.Columns.Count > 0;

            dgvResult.ContextMenuStrip = datagridMenu;
        }

        private void exportResultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportResults();
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
            if (mApplyingSyntaxHighlight)
            {
                return;
            }

            RecordQueryEdit();

            if (!string.IsNullOrEmpty(mQuerySearchText))
            {
                try
                {
                    UpdateQuerySearchMatches();
                }
                catch (ArgumentException)
                {
                    mQuerySearchMatches = null;
                    mQueryFindNextMenuItem.Enabled = false;
                    mQueryFindPreviousMenuItem.Enabled = false;
                }
                catch (RegexMatchTimeoutException)
                {
                    mQuerySearchMatches = null;
                    mQueryFindNextMenuItem.Enabled = false;
                    mQueryFindPreviousMenuItem.Enabled = false;
                }
            }

            if (!mModified)
            {
                mModified = true;
                SetTitle();
            }

            syntaxHighlightTimer.Stop();
            if (mShowQuery && txtQuery.TextLength <= MAX_AUTOMATIC_SYNTAX_LENGTH)
            {
                syntaxHighlightTimer.Start();
            }
        }

        private void syntaxHighlightTimer_Tick(object sender, EventArgs e)
        {
            syntaxHighlightTimer.Stop();
            if (!mShowQuery || IsDisposed || txtQuery.TextLength > MAX_AUTOMATIC_SYNTAX_LENGTH)
            {
                return;
            }

            int selectionStart = txtQuery.SelectionStart;
            int selectionLength = txtQuery.SelectionLength;
            mApplyingSyntaxHighlight = true;
            try
            {
                sqlHighlighter.Highlight(txtQuery);
                txtQuery.Select(
                    Math.Min(selectionStart, txtQuery.TextLength),
                    Math.Min(selectionLength, txtQuery.TextLength - Math.Min(selectionStart, txtQuery.TextLength)));
            }
            finally
            {
                mApplyingSyntaxHighlight = false;
            }
        }

        private void txtQuery_SelectionChanged(object sender, EventArgs e)
        {
            if (!mRestoringQueryHistory && mLastQueryEditorState != null &&
                string.Equals(mLastQueryEditorState.Text, txtQuery.Text, StringComparison.Ordinal))
            {
                mLastQueryEditorState.SelectionStart = txtQuery.SelectionStart;
                mLastQueryEditorState.SelectionLength = txtQuery.SelectionLength;
            }

            lblPosition.Text = string.Format("Line {0} Column {1}", txtQuery.GetLineFromCharIndex(txtQuery.SelectionStart) + 1, txtQuery.SelectionStart - txtQuery.GetFirstCharIndexOfCurrentLine() + 1);
        }
    }
}
