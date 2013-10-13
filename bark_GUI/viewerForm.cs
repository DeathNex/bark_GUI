using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace bark_GUI
{
    public partial class viewerForm : Form
    {
        /* PRIVATE VARIABLES */
        XML_Handler _XML_Handler;
        string title = "Viewer";

        //Constructor
        public viewerForm()
        {
            InitializeComponent();

            _XML_Handler = new XML_Handler();
            //Set the 'Open File' Directory to the Samples folder
            this.openFileDialog.InitialDirectory = @Pref.Path.MainDirectory + '\\' + @Pref.Path.Samples;

            statusMain.Text = "Ready";
        }
        public viewerForm(string argument)
        {
            InitializeComponent();

            _XML_Handler = new XML_Handler();
            //Set the 'Open File' Directory to the Samples folder
            this.openFileDialog.InitialDirectory = @Pref.Path.MainDirectory + '\\' + @Pref.Path.Samples;
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

            bool success;
            success = Pref.Load();
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
            this.Close();
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
            aboutBox about = new aboutBox();
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
            this.Dispose();
        }

















        /* UTILITY PRIVATE METHODS */


        private void _saveFile() { _saveAsFile(Pref.Path.CurrentFile); }
        private void _saveAsFile(string filepath)
        {
            _XML_Handler.Save(filepath);
        }

        private void _loadFile(string filepath)
        {
            //Change the action status label
            statusMain.Text = "Loading XML...";

            _closeFile();

            //Set the new current file path
            Pref.Path.CurrentFile = filepath;

            //Load the XML file
            if (_XML_Handler.Load())
            {
                //Load the viewers
                _InitializeTreeViewer();
                _InitializeElementViewer();

                //Add to the recent files list
                _AddToRecent(filepath);

                //Update Status label at the bottom of the window
                Text = _getFileNameOf(Pref.Path.CurrentFile) + " - " + title;
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
            if (_XML_Handler.HasDirtyFiles())
            {
                //Save File? Yes/No/Cancel
                saveOnExitForm formSaveOnExit = new saveOnExitForm();
                DialogResult result = formSaveOnExit.ShowDialog();

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
            _XML_Handler.Clear();
            Text = title;
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
                Program.formDiagram = new FormDiagram();
                Program.formDiagram.Show();
            }
            else
                MessageBox.Show("No file loaded to simulate. Please first open a file.");

            statusMain.Text = "Ready";
        }

        private void _PreferencesShow()
        {
            statusMain.Text = "Opening Preferences...";

            Program.formPref = new preferencesForm();
            Program.formPref.Show();

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
            int i = filePath.LastIndexOf('\\');
            return filePath.Substring(i + 1);
        }


        /// <summary> Updates the Recent List of the menu strip, using the recent list of the preferences. </summary>
        private void _UpdateRecent()
        {
            ToolStripItem item;
            recentToolStripMenuItem.DropDownItems.Clear();

            foreach (string s in Pref.Recent)
            {
                item = recentToolStripMenuItem.DropDownItems.Add(_getFileNameOf(s));
                item.ToolTipText = s;
            }
        }







        /* VIEWERS */





        /// <summary> Show/Hide the appropriate elements in the elementViewer acording to the input XmlNode. </summary>
        /// <param name="xNode"> Root XmlNode used to Show/Hide elements. To show all use 'XmlDocument.DocumentElement'. </param>
        private void _UpdateElementViewer()
        {
            TreeNode tNode = treeViewer.SelectedNode;
            GroupItem r = null;
            bool showAll = checkBoxTreeShowHidden.Checked;

            //Convert selected TreeNode to GroupItem
            foreach (GroupItem g in Structure.GroupItems)
                if (g.TNode == tNode)
                    r = g;

            //Avoid useless time consuming by taking the worst case scenarios
            if (r == Structure.Root || r == null)
            {
                if (showAll)
                    foreach (Control c in elementViewer.Controls)
                        c.Show();
                else
                    foreach (Control c in elementViewer.Controls)
                        if ((c as bark_GUI.Custom_Controls.custom_control).IsRequired)
                            c.Show();
                        else
                            c.Hide();
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
                foreach (ElementItem i in Structure.ElementItems)
                {
                    if (i.Control == null || i.Control.currentControl == null)
                        continue;   //Unfinished, handle reference controls
                    if (r.InnerChildren.Contains(i))
                        i.Control.currentControl.Show();
                    else
                        i.Control.currentControl.Hide();
                }
            else
                foreach (ElementItem i in Structure.ElementItems)
                {
                    if (i.Control == null || i.Control.currentControl == null)
                        continue;   //Unfinished, handle reference controls
                    if (r.InnerChildren.Contains(i) && i.Control.isRequired)
                        i.Control.currentControl.Show();
                    else
                        i.Control.currentControl.Hide();
                }
        }
        /// <summary> Uses the existing structure loaded by XSD to create the TreeViewer nodes. </summary>
        private void _InitializeTreeViewer()
        {
            treeViewer.Nodes.Clear();
            treeViewer.Nodes.Add(Structure.Root.TNode);
            treeViewer.ExpandAll();
            treeViewer.SelectedNode = treeViewer.Nodes[0];
        }
        /// <summary> Uses the existing structure loaded by XML to create the ElementViewer nodes. </summary>
        private void _InitializeElementViewer()
        {
            elementViewer.Controls.Clear();
            string tmp = "case";
            foreach (ElementItem item in Structure.ElementItems)
            {
                if (item.Control != null && item.Control.currentControl != null)
                {
                    if (item.Parent != null && item.Parent.Name != tmp)
                    {
                        tmp = item.Parent.Name;
                        List<custom_control_type> list = new List<custom_control_type>();
                        list.Add(custom_control_type.group);
                        elementViewer.Controls.Add((new General_Control(tmp, true)).currentControl);
                    }
                    elementViewer.Controls.Add(item.Control.currentControl);
                    if (!item.Control.isRequired)
                        item.Control.currentControl.Hide();
                }
            }
        }
    }
}
