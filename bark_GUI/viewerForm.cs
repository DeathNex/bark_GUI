using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using bark_GUI.CustomControls;
using bark_GUI.Preferences;
using bark_GUI.Structure.Items;
using bark_GUI.XmlHandling;

namespace bark_GUI
{
    public partial class ViewerForm : Form
    {
        /* PRIVATE VARIABLES */
        readonly XmlHandler _xmlHandler;
        private const string Title = "Viewer";

        //Constructor
        public ViewerForm()
        {
            InitializeComponent();

            _xmlHandler = new XmlHandler();
            //Set the 'Open File' Directory to the Samples folder
            openFileDialog.InitialDirectory = @Pref.Path.MainDirectory + '\\' + @Pref.Path.Samples;

            statusMain.Text = "Ready";
        }
        public ViewerForm(string argument)
        {
            InitializeComponent();

            _xmlHandler = new XmlHandler();
            //Set the 'Open File' Directory to the Samples folder
            openFileDialog.InitialDirectory = @Pref.Path.MainDirectory + '\\' + @Pref.Path.Samples;
            _loadFile(argument);
        }













        /* Clicks Events and Triggers */











        private void viewerForm_Load(object sender, EventArgs e)
        {
            statusMain.Text = "Loading...";

            if (Size.Empty != Properties.Settings.Default.windowSize)
            {
                WindowState = Properties.Settings.Default.windowState;
                Location = Properties.Settings.Default.windowLocation;
                Size = Properties.Settings.Default.windowSize;
            }

            var success = Pref.Load();
            if (!success)
                MessageBox.Show("Error loading preferences file.\n-File does not exist.");
            _UpdateRecent();
            statusMain.Text = "Ready";
        }

        #region MenuStrip Clicks
        /*File*/
        //File Open
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();
        }
        //File Save As
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.ShowDialog();
        }
        //File Close
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _closeFile();
        }
        //File Recent
        private void recentToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            _loadFile(e.ClickedItem.ToolTipText);
        }
        //File Exit
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        /* Simulation */
        //Simulation Start
        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _SimulationStart();
        }
        /* Options */
        //Options Clear Recent List
        private void clearRecentListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pref.Recent.Clear();
            Pref.Save();
            _UpdateRecent();
        }
        //Options Preferences
        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _PreferencesShow();
        }
        /* Help */
        //Help About
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox about = new AboutBox();
            about.Show();
        }
        #endregion

        #region Dialogs
        //File Open
        private void openFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            _loadFile(openFileDialog.FileName);
        }
        //File Save
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _saveFile();
        }
        //File Save As
        private void saveFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            _saveAsFile(saveFileDialog.FileName);
            saveFileDialog.Reset();
        }
        #endregion

        private void statusMain_TextChanged(object sender, EventArgs e)
        {
            statusStripMain.Refresh();
        }
        private void statusFileName_TextChanged(object sender, EventArgs e)
        {
            statusStripMain.Refresh();
        }
        private void labelSelected_TextChanged(object sender, EventArgs e)
        {
            labelSelected.Invalidate();
        }

        private void treeViewer_AfterSelect(object sender, TreeViewEventArgs e)
        {
            statusMain.Text = "Loading Items...";
            labelSelected.Text = e.Node.FullPath;

            _UpdateElementViewer();

            statusMain.Text = "Ready";
        }

        private void checkBoxTreeShowHidden_CheckedChanged(object sender, EventArgs e)
        {
            _UpdateElementViewer();
        }

        private void viewerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            statusMain.Text = "Closing...";

            if (!_closeFile())
                e.Cancel = true;

            //Save Maximized & Size
            Properties.Settings.Default.windowState = WindowState;
            Properties.Settings.Default.windowLocation = Location;
            Properties.Settings.Default.windowSize = Size;
            Properties.Settings.Default.Save();

            statusMain.Text = "Ready";
        }
        private void viewerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Dispose();
        }

















        /* UTILITY PRIVATE METHODS */


        private void _saveFile() { _saveAsFile(Pref.Path.CurrentFile); }
        private void _saveAsFile(string filepath)
        {
            _xmlHandler.Save(filepath);
        }

        private void _loadFile(string filepath)
        {
            //Change the action status label
            statusMain.Text = "Loading XML...";

            _closeFile();

            //Set the new current file path
            Pref.Path.CurrentFile = filepath;

            //Load the XML file
            if (_xmlHandler.Load())
            {
                //Load the viewers
                _InitializeTreeViewer();
                _InitializeElementViewer();

                //Add to the recent files list
                _AddToRecent(filepath);
                Pref.Save();

                //Update Status label at the bottom of the window
                Text = _getFileNameOf(Pref.Path.CurrentFile) + " - " + Title;
                statusMain.Text = "Ready";
            }
            else
            {
                _UpdateRecent();
                statusMain.Text = "Error";
            }
        }

        private bool _closeFile()
        {
            //Handle any dirty files
            if (_xmlHandler.HasDirtyFiles())
            {
                //Save File? Yes/No/Cancel
                var formSaveOnExit = new SaveOnExitForm();
                var result = formSaveOnExit.ShowDialog();

                switch (result)
                {
                    case DialogResult.Cancel:
                        return false;
                    case DialogResult.No:
                        break;
                    case DialogResult.Yes:
                        statusMain.Text = "Saving...";
                        _saveFile();
                        break;
                }
            }

            //Close
            treeViewer.Nodes.Clear();
            elementViewer.Controls.Clear();
            _xmlHandler.Clear();
            Text = Title;
            labelSelected.Text = "";
            statusMain.Text = "Ready";
            return true;
        }

        private void _SimulationStart()
        {
            statusMain.Text = "Simulation Begin...";

            if (Pref.Path.CurrentFile != string.Empty)
            {
                _saveFile();
                System.Diagnostics.Process.Start(Pref.Path.BarkExe + "\\bark.exe", '\"' + Pref.Path.CurrentFile + '\"');
                Program.FormDiagram1 = new FormDiagram();
                Program.FormDiagram1.Show();
            }
            else
                MessageBox.Show("No file loaded to simulate. Please first open a file.");

            statusMain.Text = "Ready";
        }

        private void _PreferencesShow()
        {
            statusMain.Text = "Opening Preferences...";

            Program.FormPref = new preferencesForm();
            Program.FormPref.Show();

            statusMain.Text = "Ready";
        }


        /// <summary> Adds the file to the recent list. </summary>
        /// <param name="filePath"> The file's path. </param>
        private void _AddToRecent(string filePath)
        {
            if (!Pref.Recent.Contains(filePath))
                Pref.Recent.Add(filePath);
            _UpdateRecent();
        }
        /// <summary> Gets the filename of any path as input. </summary>
        /// <param name="filePath"> The file's path. </param>
        /// <returns> The name of the file. </returns>
        private string _getFileNameOf(string filePath)
        {
            var i = filePath.LastIndexOf('\\');
            return filePath.Substring(i + 1);
        }


        /// <summary> Updates the Recent List of the menu strip, using the recent list of the preferences. </summary>
        private void _UpdateRecent()
        {
            recentToolStripMenuItem.DropDownItems.Clear();

            foreach (var s in Pref.Recent)
            {
                var item = recentToolStripMenuItem.DropDownItems.Add(_getFileNameOf(s));
                item.ToolTipText = s;
            }
        }







        /* VIEWERS */


        /// <summary> Show/Hide the appropriate elements in the elementViewer acording to the input XmlNode. </summary>
        private void _UpdateElementViewer()
        {
            var tNode = treeViewer.SelectedNode;
            GroupItem r = null;
            var showAll = checkBoxTreeShowHidden.Checked;

            //Convert selected TreeNode to GroupItem
            foreach (var g in Structure.Structure.GroupItems)
                if (g.Tnode == tNode)
                    r = g;

            //Avoid useless time consuming by taking the worst case scenarios
            if (r == Structure.Structure.Root || r == null)
            {
                if (showAll)
                    foreach (Control c in elementViewer.Controls)
                        c.Show();
                else
                    foreach (Control c in elementViewer.Controls)
                    {
                        var customControl = c as CustomControl;
                        Debug.Assert(customControl != null, "customControl != null");
                        if (customControl.IsRequired)
                            c.Show();
                        else
                            c.Hide();
                    }
                return;
            }
            if (r.Children == null)
            {
                foreach (Control c in elementViewer.Controls)
                    c.Hide();
                return;
            }

            //Show every element that is a child of the selected group, else hide
            if (showAll)
                foreach (var i in Structure.Structure.ElementItems)
                {
                    if (i.Control == null || i.Control.CurrentControl == null)
                        continue;   //Unfinished, handle reference controls
                    if (r.InnerChildren.Contains(i))
                        i.Control.CurrentControl.Show();
                    else
                        i.Control.CurrentControl.Hide();
                }
            else
                foreach (var i in Structure.Structure.ElementItems)
                {
                    if (i.Control == null || i.Control.CurrentControl == null)
                        continue;   //Unfinished, handle reference controls
                    if (r.InnerChildren.Contains(i) && i.Control.IsRequired)
                        i.Control.CurrentControl.Show();
                    else
                        i.Control.CurrentControl.Hide();
                }
        }
        /// <summary> Uses the existing structure loaded by XSD to create the TreeViewer nodes. </summary>
        private void _InitializeTreeViewer()
        {
            treeViewer.Nodes.Clear();
            treeViewer.Nodes.Add(Structure.Structure.Root.Tnode);
            treeViewer.ExpandAll();
            treeViewer.SelectedNode = treeViewer.Nodes[0];
        }
        /// <summary> Uses the existing structure loaded by XML to create the ElementViewer nodes. </summary>
        private void _InitializeElementViewer()
        {
            elementViewer.Controls.Clear();
            var tmp = "case";
            foreach (var item in Structure.Structure.ElementItems)
            {
                if (item.Control == null || item.Control.CurrentControl == null) continue;
                if (item.Parent != null && item.Parent.Name != tmp)
                {
                    tmp = item.Parent.Name;
                    //List<CustomControlType> list = new List<CustomControlType>();
                    //list.Add(CustomControlType.Group);
                    elementViewer.Controls.Add((new GeneralControl(tmp, true, "help text")).CurrentControl);
                }
                elementViewer.Controls.Add(item.Control.CurrentControl);
                if (!item.Control.IsRequired)
                    item.Control.CurrentControl.Hide();
            }
        }
    }
}
