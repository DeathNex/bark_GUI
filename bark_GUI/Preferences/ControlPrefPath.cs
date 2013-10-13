using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace bark_GUI
{
    public partial class ControlPrefPath : UserControl
    {
        public ControlPrefPath(string name, string path)
        {
            InitializeComponent();

            labelName.Text = name;
            textBoxPath.Text = path;
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
            folderBrowserDialog.SelectedPath = textBoxPath.Text;
        }
    }
}
