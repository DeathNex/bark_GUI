using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using bark_GUI.CustomControls;
using bark_GUI.Forms.Dialogs;
using bark_GUI.Simulation;
using bark_GUI.Structure.Items;
using bark_GUI.XmlHandling;
using bark_GUI.Properties;

namespace bark_GUI
{
    public partial class ViewerForm : Form
    {
        readonly XmlHandler _xmlHandler;

        private const string Title = "Viewer";

        private bool _elementViewerIsInitialized = false;


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

            if (Settings.Default.WindowSize != Size.Empty)
            {
                WindowState = Settings.Default.WindowState;
                Location = Settings.Default.WindowLocation;
                Size = Settings.Default.WindowSize;
                viewerSplit.SplitterDistance = Settings.Default.WindowSeparatorPosition;
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
            Settings.Default.WindowSeparatorPosition = viewerSplit.SplitterDistance;
            Settings.Default.PathCurrentFile = "";
            Settings.Default.Save();

            statusMain.Text = "Ready";
        }

        private void viewerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Dispose();
        }

        private void elementViewer_MouseClick(object sender, MouseEventArgs e)
        {
            elementViewer.Focus();
        }

        #endregion

        #region MenuStrip Clicks
        /*File*/
        //File New
        private void newToolStripMenuItem_Click(object sender, EventArgs e) { _newFile(); }
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
        private void _newFile()
        {
            var path = Settings.Default.PathSamples + '\\' + Settings.Default.XSDValidatorName;

            //Change the action status label
            statusMain.Text = "Loading XML...";

            _closeFile();

            _elementViewerIsInitialized = false;

            //Set the new current file path
            Settings.Default.PathCurrentFile = null;

            //Load the XML file
            if (!_xmlHandler.New(path))
            {
                statusMain.Text = "Error";
                return;
            }

            //Load the viewers
            _InitializeTreeViewer();
            _InitializeElementViewer();

            //Update Status label at the bottom of the window
            Text = "*Unsaved*";
            statusMain.Text = "Ready";
        }

        private void _saveFile() { _saveAsFile(Settings.Default.PathCurrentFile); }

        private void _saveAsFile(string filepath) { _xmlHandler.Save(filepath); }

        private void _loadFile(string filepath)
        {
            //Change the action status label
            statusMain.Text = "Loading XML...";

            _closeFile();

            _elementViewerIsInitialized = false;

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

            // Check.
            if (string.IsNullOrEmpty(Settings.Default.PathCurrentFile))
            {
                MessageBox.Show("No file loaded to simulate. Please first open a file.");
                return;
            }

            // Save XML file.
            _saveFile();

            // Check.
            if(!File.Exists(Settings.Default.PathBarkExe + "\\bark.exe") ||
                !File.Exists(Settings.Default.PathCurrentFile))
            {
                MessageBox.Show("Filepath of bark.exe or current xml file was wrong. No file exists.");
                return;
            }

            // Simulation.
            Process.Start(Settings.Default.PathBarkExe + "\\bark.exe",
                          '\"' + Settings.Default.PathCurrentFile + '\"');

            // Read .dat file.
            var dataPath = @"D:\Projects\bark_GUI\bark_GUI\bin\Debug\Samples\transparent.dat";
            var data = DataParser.ReadData(dataPath);

            // Check.
            if (data == null || data.Length < 1) return;

            // Use data from .dat file and prompt user to select x/y axis'.
            string xAxis = "";
            string[] yAxis = new string[1];

            using (var form = new DiagramDataDialog(data[0].Split('\t')))
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    xAxis = form.XAxisSelected;
                    yAxis = form.YAxisSelected;
                }
                else
                    return;
            }

            // Check user selection.
            if (string.IsNullOrEmpty(xAxis) || yAxis == null || yAxis.Length == 0)
            {
                MessageBox.Show("X-Axis or Y-Axis was not selected.");
                return;
            }

            // Show diagram.
            var filenameData = dataPath.Substring(dataPath.LastIndexOf('\\') + 1);
            Program.FormDiagram1 = new FormDiagram(filenameData, xAxis, yAxis, data);
            Program.FormDiagram1.Show();

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
            // Check: The Viewer has been Initialized.
            if (!_elementViewerIsInitialized || treeViewer.SelectedNode == null) return;

            // Convert selected TreeNode to GroupItem
            GroupItem treeRoot = (GroupItem)treeViewer.TopNode.Tag;
            GroupItem selectedRoot = (GroupItem)treeViewer.SelectedNode.Tag;

            // Check.
            Debug.Assert(selectedRoot != null, "ViewerForm - UpdateElementViewer - Selected GroupItem could not match the TreeNode.");

            // Stop the ElementViewer from drawing to avoid flickering.
            elementViewer.SuspendLayout();

            //Avoid useless time consuming by taking the worst case scenario.
            if (selectedRoot.Children == null)
            {
                foreach (Control c in elementViewer.Controls)
                    c.Hide();

                // Continue drawing.
                elementViewer.ResumeLayout();
                return;
            }

            //Show every element that is a child of the selected group, else hide.
            UpdateElementViewerControls(selectedRoot, treeRoot);

            // Continue drawing.
            elementViewer.ResumeLayout();
        }

        /// <summary> Uses the existing structure loaded by XSD to create the TreeViewer nodes. </summary>
        private void _InitializeTreeViewer()
        {
            // Initialize nodes.
            treeViewer.Nodes.Clear();
            treeViewer.Nodes.Add(Structure.Structure.DataRootItem.Tnode);
            treeViewer.ExpandAll();
            treeViewer.SelectedNode = treeViewer.Nodes[0];
        }

        /// <summary> Uses the existing structure loaded by XML to create the ElementViewer nodes. </summary>
        private void _InitializeElementViewer()
        {
            // Clear.
            elementViewer.Controls.Clear();

            // Iterate through the Group Items and append their children in the Viewer.
            PopulateElementViewer(Structure.Structure.DataRootItem, elementViewer);

            _elementViewerIsInitialized = true;
        }

        // Recursive function that hides/shows the controls of the element viewer.
        private void UpdateElementViewerControls(GroupItem selectedRoot, GroupItem root)
        {
            // Iterate this element's children and show/hide each control.
            foreach (var child in root.Children.Where(child => child.Control != null && child.Control.CurrentControl != null))
            {
                // Recursive call on the group items that have a child with value.
                if (child.IsGroupItem)
                {
                    var item = child as GroupItem;
                    var groupControl = child.Control.CurrentControl as ControlGroup;

                    // Check.
                    if (item == null || groupControl == null) continue;

                    // Recursive function.
                    UpdateElementViewerControls(selectedRoot, item);
                }

                ShowOrHideControl(child, checkBoxTreeShowHidden.Checked, selectedRoot);
            }
        }

        // Recursive function that populates the element viewer with the appropriate elements.
        private void PopulateElementViewer(GroupItem root, Control parent)
        {
            foreach (var child in root.Children)
            {
                // Check for Controls.
                if (child.Control == null || child.Control.CurrentControl == null) continue;

                // Do ElementItem specific actions.
                if (child.IsElementItem)
                {
                    var item = child as ElementItem;

                    // Check.
                    if (item == null) continue;

                    // Add reference options in the reference controls (must be done after the XML is read)
                    if (item.Control.CurrentControl is ControlReference)
                    {
                        var options = Structure.Structure.FindReferenceListOptions(item.Name);
                        (item.Control.CurrentControl as ControlReference).SetOptions(options);
                    }
                }

                // Append the item's Control in the Element Viewer.
                // Append the GroupItem's Control too, to allow a better view 
                // of the elements' position in the hierarchy in the Element Viewer.
                parent.Controls.Add(child.Control.CurrentControl);


                // Do GroupItem specific actions.
                if (child.IsGroupItem)
                {
                    var item = child as GroupItem;
                    var groupControl = child.Control.CurrentControl as ControlGroup;

                    // Check.
                    if (item == null || groupControl == null) continue;

                    // Recursive function.
                    PopulateElementViewer(item, groupControl.GetPanel());
                }

                // If the item is optional (not required) hide it.
                ShowOrHideControl(child, checkBoxTreeShowHidden.Checked, root);
            }
        }

        /// <summary>
        /// Encapsulates the Show or Hide control logic.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="showAll"></param>
        /// <param name="selectedRoot"></param>
        private void ShowOrHideControl(Item item, bool showAll, GroupItem selectedRoot)
        {
            var show = false;

            // Show the Element if it has a value or show all option is enabled.
            if ((item.IsElementItem) && (selectedRoot.InnerChildren.Contains(item) ||
                selectedRoot == item))
            {
                if (item.IsRequired || (showAll || item.Control.HasValue))
                    show = true;
            }
            // Show the Group if it has any children Element and that child has a value or show all option is enabled.
            else if ((item.IsGroupItem) && (selectedRoot.InnerChildren.Contains(item) ||
                selectedRoot == item || ((GroupItem)item).InnerChildren.Contains(selectedRoot)))
            {
                var groupItem = (GroupItem)item;
                var groupControl = ((ControlGroup)groupItem.Control.CurrentControl);

                if (groupControl.HasChildrenOfElementItem() && (groupControl.IsRequired ||
                    (showAll || groupControl.HasNewValue())))
                    show = true;
            }


            if (show) item.Control.CurrentControl.Show();
            else item.Control.CurrentControl.Hide();
        }
        #endregion

        #region Right-Click TreeViewer

        private TreeNode _activeNode = null;

        private void treeViewer_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;

            _activeNode = e.Node;
            var item = _activeNode.Tag as GroupItem;

            if (item != null && item.HasRightClickActions)
                _activeNode.ContextMenuStrip = TreeNodeContextMenuStrip;
            else if (item != null && item.IsGroupItem)
            {
                foreach (var child in item.Children.Where(child =>
                    child.IsGroupItem && ((GroupItem)child).HasRightClickActions))
                {
                    _activeNode.ContextMenuStrip = TreeNodeParentContextMenuStrip;
                }

            }
        }

        private void AddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var newName = InputBox.ShowDialog(("New Name for " + ((Item)_activeNode.Tag).Name), "Add");

            // Validation
            if (!string.IsNullOrEmpty(newName))
                ((Item)_activeNode.Tag).Duplicate(newName);

            _InitializeElementViewer();
            _UpdateElementViewer();
        }

        private void DuplicateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var newName = InputBox.ShowDialog(("New Name for " + ((Item)_activeNode.Tag).Name), "Duplicate",
                ((Item)_activeNode.Tag).NewName + " New");

            // Validation
            if (!string.IsNullOrEmpty(newName))
                ((Item)_activeNode.Tag).Duplicate(newName, true);

            _InitializeElementViewer();
            _UpdateElementViewer();
        }

        private void RenameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var item = ((Item)_activeNode.Tag);
            var newName = InputBox.ShowDialog(("New Name for " + item.Name), "Rename", item.NewName);

            item.NewName = newName;

            _InitializeElementViewer();
            _UpdateElementViewer();
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((GroupItem)_activeNode.Tag).Remove();
            _InitializeElementViewer();
            _UpdateElementViewer();
        }

        private void AddParentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var groupItemParent = ((GroupItem)_activeNode.Tag);

            // Get first child that can have Right-Click actions.
            //!!! TODO: Change first child that already exists to:
            //!!!       first child from Structure/XSD. (in case all materials/layers are deleted)
            var childItem = groupItemParent.Children.First(child =>
                child.IsGroupItem && ((GroupItem)child).HasRightClickActions);

            // Check.
            if (childItem == null) return;

            var newName = InputBox.ShowDialog(("New Name for " + childItem.Name), "Add");

            // Validation
            if (!string.IsNullOrEmpty(newName))
                childItem.Duplicate(newName, true);

            _InitializeElementViewer();
            _UpdateElementViewer();
        }

        #endregion

    }
}
