#region using

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using bark_GUI.Properties;

#endregion

namespace bark_GUI
{
    public partial class PreferencesForm : Form
    {
        readonly List<ControlPrefPath> _controlPaths;


        #region Constructor
        public PreferencesForm()
        {
            InitializeComponent();
            _controlPaths = new List<ControlPrefPath>
                {
                    new ControlPrefPath("Samples", Settings.Default.PathSamples),
                    new ControlPrefPath("Materials", Settings.Default.PathMaterials),
                    new ControlPrefPath("Error Log", Settings.Default.PathErrorLog),
                    new ControlPrefPath("Bark.exe", Settings.Default.PathBarkExe)
                };

            foreach (var c in _controlPaths)
                flowLayoutPanel1.Controls.Add(c);
        }
        #endregion


        private void _Apply()
        {
            Settings.Default.PathSamples = _controlPaths[0].GetPath();
            Settings.Default.PathMaterials = _controlPaths[1].GetPath();
            Settings.Default.PathErrorLog = _controlPaths[2].GetPath();
            Settings.Default.PathBarkExe = _controlPaths[3].GetPath();
        }

        #region Events
        private void buttonConfirm_Click(object sender, EventArgs e)
        {
            _Apply();
            Settings.Default.Save();
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void preferencesForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }
        #endregion
    }
}
