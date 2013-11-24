using System;
using System.Windows.Forms;

namespace bark_GUI
{
    public partial class ControlPrefPath : UserControl
    {
        public ControlPrefPath(string name, string path,
            Environment.SpecialFolder root = Environment.SpecialFolder.MyComputer)
        {
            InitializeComponent();

            labelName.Text = name;
            textBoxPath.Text = path;
            folderBrowserDialog.RootFolder = root;
            folderBrowserDialog.SelectedPath = path;
            folderBrowserDialog.Description = "Please select the desired folder.";
        }
        public string GetPath() { return textBoxPath.Text.Trim(); }

        private void buttonDirectory_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                textBoxPath.Text = folderBrowserDialog.SelectedPath;
        }

        private void textBoxPath_TextChanged(object sender, EventArgs e)
        {
            // TODO: Validate path before accepting.
            folderBrowserDialog.SelectedPath = textBoxPath.Text;
        }
    }
}
