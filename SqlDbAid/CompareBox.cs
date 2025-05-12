using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SqlDbAid
{
    class RichTextBoxSynchronizedScroll : RichTextBox
    {
        private const int WM_VSCROLL = 0x115;
        private const int WM_HSCROLL = 0x114;

        private List<RichTextBoxSynchronizedScroll> peers = new List<RichTextBoxSynchronizedScroll>();

        /// <summary>
        /// Establish a 2-way binding between RTBs for scrolling.
        /// </summary>
        /// <param name="arg">Another RTB</param>
        public void BindScroll(RichTextBoxSynchronizedScroll arg)
        {
            if (peers.Contains(arg) || arg == this) { return; }
            peers.Add(arg);
            arg.BindScroll(this);
        }

        private void DirectWndProc(ref Message m)
        {
            base.WndProc(ref m);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_VSCROLL || m.Msg == WM_HSCROLL)
            {
                foreach (RichTextBoxSynchronizedScroll peer in this.peers)
                {
                    Message peerMessage = Message.Create(peer.Handle, m.Msg, m.WParam, m.LParam);
                    peer.DirectWndProc(ref peerMessage);
                }
            }

            base.WndProc(ref m);
        }
    }

    public partial class CompareBox : UserControl
    {
        private SqlHighlighter sqlHL = new SqlHighlighter();


        #region Properties

        string mLeftText = string.Empty;
        string mRightText = string.Empty;
        bool mCaseSensitive = false;

        public bool CaseSensitive
        {
            get
            {
                return mCaseSensitive;
            }
            set
            {
                mCaseSensitive = value;
            }
        }

        public string LeftText
        {
            get
            {
                return mLeftText;
            }
            set
            {
                mLeftText = value.Replace("\r", string.Empty);
            }
        }

        public string RightText
        {
            get
            {
                return mRightText;
            }
            set
            {
                mRightText = value.Replace("\r", string.Empty);
            }
        }
        #endregion

        #region Constructor

        public CompareBox()
        {
            InitializeComponent();

            // hook up to the scroll handlers for the rich text boxes
            rtbLeft.BindScroll(rtbRight);
            rtbRight.BindScroll(rtbLeft);
        }

        #endregion

        #region Methods

        public void Clear()
        {
            mLeftText = "";
            mRightText = "";
            rtbLeft.Text = "";
            rtbRight.Text = "";
        }

        public void CompareText()
        {
            int firstDiffLine = 0;

            if (mLeftText != "" && mRightText != "")
            {
                List<int> status = new List<int>();
                StringBuilder sbLeft = new StringBuilder();
                StringBuilder sbRight = new StringBuilder();

                // load the left and right as TextFile objects
                TextDiffList tfLeft = new TextDiffList(mLeftText, mCaseSensitive);
                TextDiffList tfRight = new TextDiffList(mRightText, mCaseSensitive);

                // use DiffEngine to generate the difference list
                DiffEngine de = new DiffEngine();
                de.ProcessDiff(tfLeft, tfRight, DiffEngineLevel.SlowPerfect);
                ArrayList DiffLines = de.DiffReport();

                // now go through the result spans
                foreach (DiffResultSpan drs in DiffLines)
                {
                    switch (drs.Status)
                    {
                        // a source line was deleted
                        case DiffResultSpanStatus.DestinationMissing:
                            {
                                // for each line in the diff
                                for (int i = 0; i < drs.Length; i++)
                                {
                                    sbLeft.AppendLine(((TextLine)tfLeft.GetByIndex(drs.SourceIndex + i)).Line.ToString());
                                    sbRight.AppendLine();

                                    status.Add(-1);
                                }
                            }
                            break;

                        // there was no change
                        case DiffResultSpanStatus.NoChange:
                            {
                                for (int i = 0; i < drs.Length; i++)
                                {
                                    sbLeft.AppendLine(((TextLine)tfLeft.GetByIndex(drs.SourceIndex + i)).Line.ToString());
                                    sbRight.AppendLine(((TextLine)tfRight.GetByIndex(drs.DestIndex + i)).Line.ToString());

                                    status.Add(0);
                                }
                            }
                            break;

                        // a destination line was added
                        case DiffResultSpanStatus.SourceMissing:
                            {
                                for (int i = 0; i < drs.Length; i++)
                                {
                                    sbLeft.AppendLine();
                                    sbRight.AppendLine(((TextLine)tfRight.GetByIndex(drs.DestIndex + i)).Line.ToString());

                                    status.Add(1);
                                }
                            }
                            break;

                        // the text of the line changed
                        case DiffResultSpanStatus.Changed:
                            {
                                for (int i = 0; i < drs.Length; i++)
                                {
                                    sbLeft.AppendLine(((TextLine)tfLeft.GetByIndex(drs.SourceIndex + i)).Line.ToString());
                                    sbRight.AppendLine(((TextLine)tfRight.GetByIndex(drs.DestIndex + i)).Line.ToString());

                                    status.Add(2);
                                }
                            }
                            break;
                    }

                    if (firstDiffLine == 0 && status.Count > 0 && drs.Status != DiffResultSpanStatus.NoChange)
                    {
                        firstDiffLine = status.Count - 1;
                    }
                }

                rtbLeft.Text = sbLeft.ToString();
                rtbRight.Text = sbRight.ToString();

                sqlHL.Highlight(rtbLeft);
                sqlHL.Highlight(rtbRight);

                for (int i = 0; i < status.Count; i++)
                {
                    if (status[i] == -1 || status[i] == 2)
                    {
                        int start = rtbLeft.GetFirstCharIndexFromLine(i);
                        int length = rtbLeft.Lines[i].Length;
                        rtbLeft.Select(start, length);
                        if (status[i] == 2)
                        {
                            rtbLeft.SelectionBackColor = Color.Gold;
                        }
                        else
                        {
                            rtbLeft.SelectionBackColor = Color.LightPink;
                        }
                    }
                    if (status[i] == 1 || status[i] == 2)
                    {
                        int start = rtbRight.GetFirstCharIndexFromLine(i);
                        int length = rtbRight.Lines[i].Length;
                        rtbRight.Select(start, length);
                        if (status[i] == 2)
                        {
                            rtbRight.SelectionBackColor = Color.Gold;
                        }
                        else
                        {
                            rtbRight.SelectionBackColor = Color.LightPink;
                        }
                    }
                }
            }
            else
            {
                rtbLeft.Text = mLeftText;
                rtbRight.Text = mRightText;

                sqlHL.Highlight(rtbLeft);
                sqlHL.Highlight(rtbRight);
            }

            int lineStart = 0;

            lineStart = rtbLeft.GetFirstCharIndexFromLine(firstDiffLine);
            rtbLeft.Select(lineStart, 0);
            lineStart = rtbRight.GetFirstCharIndexFromLine(firstDiffLine);
            rtbRight.Select(lineStart, 0);

            rtbRight.Focus();
            rtbLeft.Focus();
        }

        #endregion
    }
}
