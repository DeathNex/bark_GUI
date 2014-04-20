#region using
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using bark_GUI.CustomControls;
using bark_GUI.Structure.Items;
using bark_GUI.XmlHandling;
using bark_GUI.Properties;
#endregion

namespace bark_GUI
{
    public partial class ViewerForm : Form
    {
        readonly XmlHandler _xmlHandler;

        private const string Title = "Viewer";


        #region Constructors
        public ViewerForm()
        {
            InitializeComponent();

            _xmlHandler = new XmlHandler();
            //Set the 'Open File' Directory to the Samples folder
            openFileDialog.InitialDirectory = Settings.Default.PathSamples;

            statusMain.Text = "Ready";
        }
        public ViewerForm(string argument)
        {
            InitializeComponent();

            _xmlHandler = new XmlHandler();
            //Set the 'Open File' Directory to the Samples folder
            openFileDialog.InitialDirectory = Settings.Default.PathSamples;
            _loadFile(argument);
        }
        #endregion


        #region Clicks Events and Triggers
        #region ViewerForm Events
        private void viewerForm_Load(object sender, EventArgs e)
        {
            statusMain.Text = "Loading...";

            if (Size.Empty != Settings.Default.WindowSize)
            {
                WindowState = Settings.Default.WindowState;
                Location = Settings.Default.WindowLocation;
                Size = Settings.Default.WindowSize;
            }

            _UpdateRecent();
            statusMain.Text = "Ready";
        }

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
            Settings.Default.WindowState = WindowState;
            Settings.Default.WindowLocation = Location;
            Settings.Default.WindowSize = Size;
            Settings.Default.PathCurrentFile = "";
            Settings.Default.Save();

            statusMain.Text = "Ready";
        }

        private void viewerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Dispose();
        }
        #endregion

        #region MenuStrip Clicks
        /*File*/
        //File Open
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();
        }
        //File Save
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _saveFile();
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
            Settings.Default.MenuRecentFiles.Clear();
            Settings.Default.Save();
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

        #region Dialog Events
        //File Open
        private void openFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            _loadFile(openFileDialog.FileName);
        }
        //File Save As
        private void saveFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            _saveAsFile(saveFileDialog.FileName);
            saveFileDialog.Reset();
        }
        #endregion
        #endregion

        #region Action Methods
        private void _saveFile() { _saveAsFile(Settings.Default.PathCurrentFile); }

        private void _saveAsFile(string filepath) { _xmlHandler.Save(filepath); }

        private void _loadFile(string filepath)
        {
            //Change the action status label
            statusMain.Text = "Loading XML...";

            _closeFile();

            //Set the new current file path
            Settings.Default.PathCurrentFile = filepath;

            //Load the XML file
            if (_xmlHandler.Load())
            {
                //Load the viewers
                _InitializeTreeViewer();
                _InitializeElementViewer();

                //Add to the recent files list
                _AddToRecent(filepath);
                Settings.Default.Save();

                //Update Status label at the bottom of the window
                Text = _getFileNameOf(Settings.Default.PathCurrentFile) + " - " + Title;
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

            if (Settings.Default.PathCurrentFile != string.Empty)
            {
                _saveFile();
                // TODO: Validate path before executing.
                Process.Start(Settings.Default.PathBarkExe + "\\bark.exe", '\"' + Settings.Default.PathCurrentFile + '\"');
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

            Program.FormPref = new PreferencesForm();
            Program.FormPref.Show();

            statusMain.Text = "Ready";
        }

        /// <summary> Adds the file to the recent list. </summary>
        /// <param name="filePath"> The file's path. </param>
        private void _AddToRecent(string filePath)
        {
            if (!Settings.Default.MenuRecentFiles.Contains(filePath))
                Settings.Default.MenuRecentFiles.Add(filePath);
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

            foreach (var s in Settings.Default.MenuRecentFiles)
            {
                var item = recentToolStripMenuItem.DropDownItems.Add(_getFileNameOf(s.ToString()));
                item.ToolTipText = s.ToString();
            }
        }
        #endregion

        #region Viewers
        /// <summary> Show/Hide the appropriate elements in the elementViewer acording to the input XmlNode. </summary>
        private void _UpdateElementViewer()
        {
            var tNode = treeViewer.SelectedNode;
            GroupItem r = null;
            var showAll = checkBoxTreeShowHidden.Checked;

            //Convert selected TreeNode to GroupItem
            foreach (var g in Structure.Structure.GroupItems.Where(g => g.Tnode == tNode))
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
                        continue;   //TODO: Unfinished, handle reference controls
                    if (r.InnerChildren.Contains(i))
                        i.Control.CurrentControl.Show();
                    else
                        i.Control.CurrentControl.Hide();
                }
            else
                foreach (var i in Structure.Structure.ElementItems)
                {
                    if (i.Control == null || i.Control.CurrentControl == null)
                        continue;   //TODO: Unfinished, handle reference controls
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
            var tmp = "case";

            // Clear.
            elementViewer.Controls.Clear();

            // Iterate through the Element Items and append them in the Viewer. (including their parents)
            foreach (var item in Structure.Structure.ElementItems)
            {
                // Check.
                if (item.Control == null || item.Control.CurrentControl == null) continue;

                // Create this item's parent as GROUP if it doesn't already exist.
                // (to allow a better view of the elements' position in the hierarchy)
                var parentViewName = item.Parent.Name;

                // If a custom name exists, append it to the viewName.
                if (!string.IsNullOrEmpty(item.Parent.NewName))
                    parentViewName = string.Format("({0}) {1}", item.Parent.Name, item.Parent.NewName);

                // If the parent of this element doesnt already exist, add it.
                if (item.Parent != null && parentViewName != tmp && parentViewName != tmp)
                {
                    tmp = parentViewName;
                    elementViewer.Controls.Add((new GeneralControl(item, tmp, true, "help text")).CurrentControl);
                }

                // Add reference options in the reference controls (must be done after the XML is read)
                if (item.Control.CurrentControl is ControlReference)
                {
                    var options = Structure.Structure.FindReferenceListOptions(item.Name);
                    (item.Control.CurrentControl as ControlReference).SetOptions(options);
                }

                // Append the item in the Viewer.
                elementViewer.Controls.Add(item.Control.CurrentControl);

                // If the item is optional (not required) hide it.
                if (!item.Control.IsRequired)
                    item.Control.CurrentControl.Hide();
            }
        }
        #endregion
    }
}
