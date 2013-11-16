using System;
using System.Windows.Forms;

namespace bark_GUI
{
    public partial class SaveOnExitForm : Form
    {
        public SaveOnExitForm()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Dispose();
        }


        private void saveOnExitForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Dispose();
        }
    }
}
