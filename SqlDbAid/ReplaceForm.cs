using System;
using System.Windows.Forms;

namespace SqlDbAid
{
    public partial class ReplaceForm : Form
    {
        public enum ReplaceChoice
        {
            Yes = 1,
            YesAll,
            No,
            NoAll,
            Cancel
        }

        private ReplaceChoice pvtChoice = ReplaceChoice.No;

        public ReplaceChoice Choice
        {
            get { return pvtChoice; }
        }

        public ReplaceForm()
        {
            InitializeComponent();
        }

        public ReplaceForm(string fileName) : this()
        {
            lblFileName.Text = fileName;
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            pvtChoice = ReplaceChoice.Yes;
            this.Close();
        }

        private void btnYesAll_Click(object sender, EventArgs e)
        {
            pvtChoice = ReplaceChoice.YesAll;
            this.Close();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            pvtChoice = ReplaceChoice.No;
            this.Close();
        }

        private void btnNoAll_Click(object sender, EventArgs e)
        {
            pvtChoice = ReplaceChoice.NoAll;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            pvtChoice = ReplaceChoice.Cancel;
            this.Close();
        }
    }
}
