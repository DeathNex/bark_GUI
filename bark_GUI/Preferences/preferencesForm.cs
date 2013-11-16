using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using bark_GUI.Preferences;

namespace bark_GUI
{
    public partial class preferencesForm : Form
    {
        List<ControlPrefPath> controlPaths;
        public preferencesForm()
        {
            InitializeComponent();
            controlPaths = new List<ControlPrefPath>();
            controlPaths.Add(new ControlPrefPath("Samples", Pref.Path.Samples));
            controlPaths.Add(new ControlPrefPath("Materials", Pref.Path.Materials));
            controlPaths.Add(new ControlPrefPath("Error Log", Pref.Path.ErrorLog));
            controlPaths.Add(new ControlPrefPath("Bark.exe", Pref.Path.BarkExe));

            foreach (ControlPrefPath c in controlPaths)
                flowLayoutPanel1.Controls.Add(c);
        }


        private void buttonConfirm_Click(object sender, EventArgs e)
        {
            _Apply();
            Pref.Save();
            this.Close();
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }









        private void _Apply()
        {
            Pref.Path.Samples = controlPaths[0].GetPath();
            Pref.Path.Materials = controlPaths[1].GetPath();
            Pref.Path.ErrorLog = controlPaths[2].GetPath();
            Pref.Path.BarkExe = controlPaths[3].GetPath();
        }

        private void preferencesForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }
    }
}
