using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SqlDbAid
{
    public partial class CodeForm : Form
    {
        private const int CODE_CHUNK_SIZE = 65536;
        private const int MAX_AUTOMATIC_SYNTAX_LENGTH = 100000;

        private string searchText = "";
        private string pendingCode = string.Empty;
        private string codeText = string.Empty;
        private int pendingCodePosition;
        private bool useRegex = false;

        private SqlHighlighter sqlHL = new SqlHighlighter();
        private Timer highlightTimer;

        private Regex searchRegex;
        private MatchCollection matchColl;
        private int currentMatch = -1;

        private bool highlighting = true;
        private bool searchMatchesInitialized;
        private bool syntaxColorsEnabled = true;

        private static DialogResult InputBox(string title, string promptText, ref string value, ref bool regex)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            CheckBox chkRegex = new CheckBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            form.ShowInTaskbar = false;
            label.Text = promptText;
            textBox.Text = value;
            chkRegex.Checked = regex;

            chkRegex.Text = "Regular Expression";

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            chkRegex.SetBounds(12, 72, 120, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            chkRegex.Anchor = chkRegex.Anchor | AnchorStyles.Left;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, chkRegex, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            regex = chkRegex.Checked;
            return dialogResult;
        }

        private void SelectNext()
        {
            if (matchColl != null && matchColl.Count > 0)
            {
                currentMatch++;

                if (currentMatch == matchColl.Count)
                {
                    currentMatch = 0;
                }

                int selectionLength = codeText.Length > MAX_AUTOMATIC_SYNTAX_LENGTH
                    ? matchColl[currentMatch].Length
                    : 0;
                rtbCode.Select(matchColl[currentMatch].Index, selectionLength);
            }
            else
            {
                rtbCode.Select(0, 0);
            }
        }

        private void SelectPrev()
        {
            if (matchColl != null && matchColl.Count > 0)
            {
                currentMatch--;

                if (currentMatch < 0)
                {
                    currentMatch = matchColl.Count - 1;
                }

                int selectionLength = codeText.Length > MAX_AUTOMATIC_SYNTAX_LENGTH
                    ? matchColl[currentMatch].Length
                    : 0;
                rtbCode.Select(matchColl[currentMatch].Index, selectionLength);
            }
            else
            {
                rtbCode.Select(0, 0);
            }
        }

        private void SyntaxHighlight()
        {
            highlighting = true;
            try
            {
                codeText = rtbCode.Text;
                UpdateSearchMatches();
                searchMatchesInitialized = true;

                if (syntaxColorsEnabled)
                {
                    sqlHL.Highlight(rtbCode, searchRegex);
                    codeText = rtbCode.Text;
                    UpdateSearchMatches();
                }

                rtbCode.Invalidate();
            }
            finally
            {
                highlighting = false;
            }
        }

        public CodeForm()
        {
            InitializeComponent();
            highlightTimer = new Timer(components);
            highlightTimer.Interval = 15;
            highlightTimer.Tick += highlightTimer_Tick;
            Shown += CodeForm_Shown;
            syntaxHighlightToolStripMenuItem.Visible = false;
            highlighting = false;
        }

        public CodeForm(string code, string search, bool regex)
            : this()
        {
            pendingCode = code ?? string.Empty;
            searchText = search ?? string.Empty;
            useRegex = regex;
            syntaxHighlightToolStripMenuItem.Visible =
                pendingCode.Length > MAX_AUTOMATIC_SYNTAX_LENGTH;
            highlighting = false;
        }

        private void CodeForm_Load(object sender, EventArgs e)
        {
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;

            int winHeight;
            int winWidth;
            if (pendingCode.Length > 20000)
            {
                winHeight = screenHeight - 50;
                winWidth = screenWidth - 50;
            }
            else
            {
                int maxLineColumns = 0;
                int currentLineColumns = 0;
                int lineCount = 1;
                for (int i = 0; i < pendingCode.Length; i++)
                {
                    char current = pendingCode[i];
                    if (current == '\r' || current == '\n')
                    {
                        maxLineColumns = Math.Max(maxLineColumns, currentLineColumns);
                        currentLineColumns = 0;
                        lineCount++;
                        if (current == '\r' && i + 1 < pendingCode.Length && pendingCode[i + 1] == '\n')
                        {
                            i++;
                        }
                    }
                    else if (current == '\t')
                    {
                        currentLineColumns += 8 - currentLineColumns % 8;
                    }
                    else
                    {
                        currentLineColumns++;
                    }
                }
                maxLineColumns = Math.Max(maxLineColumns, currentLineColumns);

                int charWidth = TextRenderer.MeasureText("M", rtbCode.Font, Size.Empty, TextFormatFlags.NoPadding).Width;
                long estimatedWidth = (long)maxLineColumns * charWidth + 50;
                long estimatedHeight = (long)lineCount * rtbCode.Font.Height + 90;
                winHeight = (int)Math.Min(estimatedHeight, int.MaxValue);
                winWidth = (int)Math.Min(estimatedWidth, int.MaxValue);
            }

            if (winHeight < 300)
            {
                winHeight = 300;
            }
            if (winWidth < 400)
            {
                winWidth = 400;
            }
            if (winHeight > screenHeight - 50)
            {
                winHeight = screenHeight - 50;
            }
            if (winWidth > screenWidth - 50)
            {
                winWidth = screenWidth - 50;
            }

            this.Height = winHeight;
            this.Width = winWidth;
        }

        private void CodeForm_Shown(object sender, EventArgs e)
        {
            highlightTimer.Start();
        }

        private void highlightTimer_Tick(object sender, EventArgs e)
        {
            highlightTimer.Stop();
            if (highlighting)
            {
                return;
            }

            highlighting = true;
            try
            {
                if (pendingCode != null)
                {
                    if (!LoadNextCodeChunk())
                    {
                        highlightTimer.Start();
                        return;
                    }

                    highlightTimer.Interval = 100;
                }

                if (!searchMatchesInitialized)
                {
                    UpdateSearchMatches();
                    if (codeText.Length <= MAX_AUTOMATIC_SYNTAX_LENGTH)
                    {
                        sqlHL.Highlight(rtbCode, searchRegex);
                        codeText = rtbCode.Text;
                        UpdateSearchMatches();
                    }
                    else
                    {
                        syntaxColorsEnabled = false;
                        lblPosition.Text = "Syntax colors skipped; right-click to highlight";
                    }

                    searchMatchesInitialized = true;
                    SelectNext();
                    UpdatePositionStatus();
                }
            }
            finally
            {
                highlighting = false;
            }
        }

        private bool LoadNextCodeChunk()
        {
            int chunkLength = Math.Min(CODE_CHUNK_SIZE, pendingCode.Length - pendingCodePosition);
            if (chunkLength > 0)
            {
                rtbCode.AppendText(pendingCode.Substring(pendingCodePosition, chunkLength));
                pendingCodePosition += chunkLength;
            }

            if (pendingCodePosition < pendingCode.Length)
            {
                return false;
            }

            codeText = rtbCode.Text;
            pendingCode = null;
            pendingCodePosition = 0;
            rtbCode.Select(0, 0);
            return true;
        }

        private void UpdateSearchMatches()
        {
            searchRegex = null;
            matchColl = null;
            currentMatch = -1;

            if (searchText.Length == 0)
            {
                return;
            }

            string search = useRegex ? searchText : Regex.Escape(searchText);
            searchRegex = new Regex(search, RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);
            matchColl = searchRegex.Matches(codeText);
        }

        private void rtbCode_SelectionChanged(object sender, EventArgs e)
        {
            if (highlighting)
            {
                return;
            }

            UpdatePositionStatus();
        }

        private void UpdatePositionStatus()
        {
            string position = string.Format("Line {0} Column {1}",
                rtbCode.GetLineFromCharIndex(rtbCode.SelectionStart) + 1,
                rtbCode.SelectionStart - rtbCode.GetFirstCharIndexOfCurrentLine() + 1);
            lblPosition.Text = syntaxColorsEnabled
                ? position
                : position + " | syntax colors off; right-click to highlight";
        }

        private void syntaxHighlightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pendingCode != null || syntaxColorsEnabled)
            {
                return;
            }

            int selectionStart = rtbCode.SelectionStart;
            highlighting = true;
            try
            {
                codeText = rtbCode.Text;
                UpdateSearchMatches();
                sqlHL.Highlight(rtbCode, searchRegex);
                codeText = rtbCode.Text;
                syntaxColorsEnabled = true;
                syntaxHighlightToolStripMenuItem.Enabled = false;
                syntaxHighlightToolStripMenuItem.Text = "SQL highlighted";

                UpdateSearchMatches();
                searchMatchesInitialized = true;
                if (matchColl != null && matchColl.Count > 0)
                {
                    SelectNext();
                }
                else
                {
                    rtbCode.Select(Math.Min(selectionStart, codeText.Length), 0);
                }

                UpdatePositionStatus();
                rtbCode.Invalidate();
            }
            finally
            {
                highlighting = false;
            }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(rtbCode.SelectedText);
            rtbCode.SelectedText = "";
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(rtbCode.SelectedText, true);
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText(TextDataFormat.Text))
            {
                rtbCode.SelectedText = Clipboard.GetData(DataFormats.Text).ToString();
            }
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtbCode.SelectAll();
        }

        private void rtbCode_MouseDown(object sender, MouseEventArgs e)
        {
            if (rtbCode.SelectedText.Length > 0)
            {
                copyToolStripMenuItem.Enabled = true;
            }
            else
            {
                copyToolStripMenuItem.Enabled = false;
            }
        }

        private void rtbCode_TextChanged(object sender, EventArgs e)
        {
            if (highlighting)
            {
                return;
            }

            int start = rtbCode.GetFirstCharIndexOfCurrentLine();
            int end = start;
            int currentLineNumber = rtbCode.GetLineFromCharIndex(start);
            if (currentLineNumber < rtbCode.Lines.Length - 1)
            {
                end = rtbCode.GetFirstCharIndexFromLine(currentLineNumber + 1) - 1;
            }
            else
            {
                end = rtbCode.Text.Length - 1;
            }

            if (start >= end)
            {
                return;
            }

            string currentLine = rtbCode.Text.Substring(start, end - start);

            char[] trim = { ' ', '\t' };

            if (string.IsNullOrEmpty(currentLine.Trim(trim)))
            {
                return;
            }

            int selectionStart = rtbCode.SelectionStart;
            int selectionLength = rtbCode.SelectionLength;

            SyntaxHighlight();

            rtbCode.SelectionStart = selectionStart;
            rtbCode.SelectionLength = selectionLength;
        }

        private void rtbCode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
            {
                if (InputBox("Find", "Find what:", ref searchText, ref useRegex) == DialogResult.OK)
                {
                    SyntaxHighlight();

                    SelectNext();
                }
            }
            else if (e.Shift && e.KeyCode == Keys.F3 && matchColl != null && matchColl.Count > 0)
            {
                SelectPrev();
            }
            else if (e.KeyCode == Keys.F3 && matchColl != null && matchColl.Count > 0)
            {
                SelectNext();
            }
        }
    }
}
