using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SqlDbAid
{
    public partial class CodeForm : Form
    {
        private string searchText = "";
        private bool useRegex = false;

        private SqlHighlighter sqlHL = new SqlHighlighter();

        private MatchCollection matchColl;
        private int currentMatch = -1;

        private bool highlighting = true;

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

        private const int WM_SETREDRAW = 0x000B;
        private const int WM_USER = 0x400;
        private const int EM_GETEVENTMASK = WM_USER + 59;
        private const int EM_SETEVENTMASK = WM_USER + 69;
        private const int EM_GETSCROLLPOS = WM_USER + 221;
        private const int EM_SETSCROLLPOS = WM_USER + 222;

        [DllImport("user32")]
        private extern static IntPtr SendMessage(IntPtr hWnd, Int32 wMsg, Int32 wParam, IntPtr lParam);

        [DllImport("user32")]
        private extern static IntPtr SendMessage(IntPtr hWnd, Int32 wMsg, Int32 wParam, ref Point lParam);

        private void SelectNext()
        {
            if (matchColl != null && matchColl.Count > 0)
            {
                currentMatch++;

                if (currentMatch == matchColl.Count)
                {
                    currentMatch = 0;
                }

                rtbCode.Select(matchColl[currentMatch].Index, 0);
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

                rtbCode.Select(matchColl[currentMatch].Index, 0);
            }
            else
            {
                rtbCode.Select(0, 0);
            }
        }

        private void SyntaxHighlight()
        {
            highlighting = true;
            Point pt = new Point();

            SendMessage(rtbCode.Handle, WM_SETREDRAW, 0, IntPtr.Zero);
            IntPtr eventMask = SendMessage(rtbCode.Handle, EM_GETEVENTMASK, 0, IntPtr.Zero);
            SendMessage(rtbCode.Handle, EM_GETSCROLLPOS, 0, ref pt);

            sqlHL.Highlight(rtbCode);

            //search text
            matchColl = null;
            currentMatch = -1;
            if (searchText != "")
            {
                string search = (useRegex) ? searchText : Regex.Escape(searchText);

                Regex s = new Regex(search, RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);

                matchColl = s.Matches(rtbCode.Text.ToString());
                currentMatch = -1;

                foreach (Match match in matchColl)
                {
                    rtbCode.Select(match.Index, match.Length);
                    //rtbCode.SelectionColor = Color.Black;
                    rtbCode.SelectionBackColor = Color.Yellow;
                }
            }

            SendMessage(rtbCode.Handle, EM_SETEVENTMASK, 0, eventMask);
            SendMessage(rtbCode.Handle, WM_SETREDRAW, 1, IntPtr.Zero);
            SendMessage(rtbCode.Handle, EM_SETSCROLLPOS, 0, ref pt);

            rtbCode.Invalidate();
            highlighting = false;
        }

        public CodeForm()
        {
            InitializeComponent();
        }

        public CodeForm(string code, string search, bool regex)
            : this()
        {
            rtbCode.SelectionStart = 0;
            rtbCode.Text = code;

            searchText = search;
            useRegex = regex;

            SyntaxHighlight();

            SelectNext();
        }

        private void CodeForm_Load(object sender, EventArgs e)
        {
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;

            int maxWidth = 0;
            int pos = 0;

            foreach (string line in rtbCode.Lines)
            {
                pos = pos + line.Length + 1;
                int width = rtbCode.GetPositionFromCharIndex(pos - 1).X;
                if (width > maxWidth)
                {
                    maxWidth = width;
                }
            }

            int winHeight = rtbCode.GetPositionFromCharIndex(rtbCode.Text.Length).Y + 90;
            int winWidth = maxWidth + 50;

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

        private void rtbCode_SelectionChanged(object sender, EventArgs e)
        {
            if (highlighting)
            {
                return;
            }

            lblPosition.Text = string.Format("Line {0} Column {1}", rtbCode.GetLineFromCharIndex(rtbCode.SelectionStart) + 1, rtbCode.SelectionStart - rtbCode.GetFirstCharIndexOfCurrentLine() + 1);
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
