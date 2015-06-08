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

        private void labelSelected_TextChanged(object sender, EventArgs e)
        {
            labelSelected.Invalidate();
        }

        private void treeViewer_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // Change the action status label
            statusMain.Text = "Updating Controls...";
            labelSelected.Text = e.Node.FullPath;

            _UpdateElementViewer();

            statusMain.Text = "Ready";
        }

        private void checkBoxTreeShowHidden_CheckedChanged(object sender, EventArgs e)
        {
            // Change the action status label
            statusMain.Text = "Updating Controls...";

            _UpdateElementViewer();

            statusMain.Text = "Ready";
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
        //Simulation Create Graph
        private void createGraphToolStripMenuItem_Click(object sender, EventArgs e) { _CreateGraph(); }
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
            var pathNewFile = Settings.Default.PathSamples + '\\' + Settings.Default.PathNew;

            _loadFile(pathNewFile);
        }

        private void _saveFile() { _saveAsFile(Settings.Default.PathCurrentFile); }

        private void _saveAsFile(string filepath)
        {
            // Change the action status label
            statusMain.Text = "Saving...";

            if (!AllControlsAreValid())
            {
                MessageBox.Show(
                    "One or more values are not valid (Red Color).\n" +
                    "Please fill the controls with proper values before saving.");
                statusMain.Text = "Error";
                return;
            }

            // If called from 'Save' on a new document, call 'save as'.
            if (string.IsNullOrEmpty(filepath))
            {
                saveFileDialog.ShowDialog();
                return;
            }

            var success = _xmlHandler.Save(filepath);

            if (!success)
            {
                statusMain.Text = "Error";
                return;
            }

            // Set saved file path as the current file.
            Settings.Default.PathCurrentFile = filepath;

            // Set Viewer title.
            Text = _getFileNameOf(filepath) + " - " + Title;

            // Change the action status label
            statusMain.Text = "Ready";
        }

        private void _loadFile(string filepath)
        {
            if (!_closeFile()) return;

            // Change the action status label
            statusMain.Text = "Loading XML...";

            _elementViewerIsInitialized = false;

            var pathNewFile = Settings.Default.PathSamples + '\\' + Settings.Default.PathNew;

            // Load file
            if (filepath != pathNewFile)
            {
                // Set the new current file path
                Settings.Default.PathCurrentFile = filepath;

                //Load the XML file
                if (_xmlHandler.Load())
                {
                    // Change the action status label
                    statusMain.Text = "Updating Controls...";

                    // Load the viewers
                    _InitializeTreeViewer();
                    _InitializeElementViewer();

                    // Add to the recent files list
                    _AddToRecent(filepath);
                    Settings.Default.Save();

                    // Update Status label at the bottom of the window
                    Text = _getFileNameOf(Settings.Default.PathCurrentFile) + " - " + Title;
                    statusMain.Text = "Ready";
                }
                else
                {
                    _UpdateRecent();
                    statusMain.Text = "Error";
                }
            }
            else
            {
                // Create New File.

                //Set the new current file path
                Settings.Default.PathCurrentFile = "";

                //Load the XML file
                if (_xmlHandler.Load(pathNewFile))
                {
                    // Change the action status label
                    statusMain.Text = "Updating Controls...";

                    //Load the viewers
                    _InitializeTreeViewer();
                    _InitializeElementViewer();

                    Text = "*New*" + " - " + Title;
                    statusMain.Text = "Ready";
                }
                else
                {
                    statusMain.Text = "Error";
                }
            }
        }

        private bool _closeFile()
        {
            // Change the action status label
            statusMain.Text = "Closing...";

            //Handle any dirty files
            if (_xmlHandler.HasDirtyFiles() && !string.IsNullOrEmpty(Settings.Default.PathCurrentFile))
            {
                //Save File? Yes/No/Cancel
                var formSaveOnExit = new SaveOnExitForm();
                var result = formSaveOnExit.ShowDialog();

                switch (result)
                {
                    case DialogResult.Cancel:
                        statusMain.Text = "Canceled";
                        return false;
                    case DialogResult.No:
                        break;
                    case DialogResult.Yes:
                        _saveFile();
                        break;
                }
            }

            // Close
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
            // Change the action status label
            statusMain.Text = "Executing Simulation...";

            if (!AllControlsAreValid())
            {
                MessageBox.Show(
                    "One or more values are not valid (Red Color).\n" +
                    "Please fill the controls with proper values before saving.");
                return;
            }

            // Check.
            if (string.IsNullOrEmpty(Settings.Default.PathCurrentFile))
            {
                MessageBox.Show("No file loaded to simulate. Please first open a file.");
                return;
            }

            // Save XML file.
            _saveFile();

            // Check.
            if (!File.Exists(Settings.Default.PathBarkExe + "\\bark.exe") ||
                !File.Exists(Settings.Default.PathCurrentFile))
            {
                MessageBox.Show("Filepath of bark.exe or current xml file was wrong. No file exists.");
                return;
            }

            statusMain.Text = "Executing Simulation...";

            // Simulation.
            Process.Start(Settings.Default.PathBarkExe + "\\bark.exe",
                          '\"' + Settings.Default.PathCurrentFile + '\"');

            statusMain.Text = "Ready";
        }

        private void _CreateGraph()
        {
            // Change the action status label
            statusMain.Text = "Creating Graph...";

            var dataFileName = _getFileNameOf(Settings.Default.PathCurrentFile).Split('.')[0];
            var dataPath = Settings.Default.PathSamples + '\\' + dataFileName + ".dat";

            // Check.
            if (string.IsNullOrEmpty(Settings.Default.PathCurrentFile))
            {
                MessageBox.Show("No file loaded to create graph from. Please first open a file.");
                statusMain.Text = "Error";
                return;
            }

            // Check.
            if (!File.Exists(dataPath))
            {
                MessageBox.Show("No data file found for " + dataFileName +
                    ".\nStart the simulation before creating a graph.");
                statusMain.Text = "Error";
                return;
            }

            // Read .dat file.
            var data = DataParser.ReadData(dataPath);

            // Check.
            if (data == null || data.Length < 1)
            {
                statusMain.Text = "Error";
                return;
            }

            // Use data from .dat file and prompt user to select x/y axis'.
            string xAxis = "";
            string[] yAxis = new string[1];
            var date = File.GetLastWriteTime(dataPath);
            var lastSimulationDate = string.Format("{0}:{1} at {2}/{3}/{4}", date.Hour, date.Minute, date.Day, date.Month, date.Year);

            using (var form = new DiagramDataDialog(data[0].Split('\t'), lastSimulationDate))
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    xAxis = form.XAxisSelected;
                    yAxis = form.YAxisSelected;
                }
                else
                {
                    statusMain.Text = "Canceled";
                    return;
                }
            }

            // Check user selection.
            if (string.IsNullOrEmpty(xAxis) || yAxis == null || yAxis.Length == 0)
            {
                MessageBox.Show("X-Axis or Y-Axis was not selected.");
                statusMain.Text = "Error";
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

        private bool AllControlsAreValid()
        { return elementViewer.Controls.OfType<CustomControl>().All(control => control.IsValid); }

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
                if (item.IsRequired || (showAll || item.Control.HasNewValue))
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
            AddItem((Item)_activeNode.Tag);
        }

        private void RenameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var item = ((Item)_activeNode.Tag);

            var newName = Rename(item);

            if (string.IsNullOrEmpty(newName)) return;

            RefreshViewers();
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var item = ((Item)_activeNode.Tag);

            // Make sure this isn't the last item.
            if (item.Parent != null && item.Parent.Children.Select(i =>
                i.IsElementItem == item.IsElementItem && i.Name == item.Name).Count() <= 1)
            {
                MessageBox.Show(item.NewName + " cannot be deleted.\nAt least one " + item.Name + " is required.");
                return;
            }

            item.Remove();

            RefreshViewers();
        }

        private void AddParentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var groupItemParent = ((GroupItem)_activeNode.Tag);

            // Condition: At least one 'multiple' child already exists to create a new one.
            // To be release from the above condition you need to:
            // Change the 'first child that already exists' part to 'first child of that type from Structure/XSD'.

            // Get first child that can have Right-Click actions.
            var childItem = groupItemParent.Children.First(child =>
                child.IsGroupItem && ((GroupItem)child).HasRightClickActions);

            AddItem(childItem);
        }

        private void AddItem(Item item)
        {
            var newName = GetNewName(item);

            // Check.
            if (string.IsNullOrEmpty(newName)) return;

            //var xml = XmlParser.ConvertToXml(item);

            //var newItem = Structure.Structure.CreateItem(xml);

            item.Duplicate(newName);

            //newItem.NewName = newName;

            RefreshViewers();
        }

        private string Rename(Item item)
        {
            if (item == null) return null;

            var oldName = item.NewName;
            var newName = GetNewName(item, item.NewName);

            if (string.IsNullOrEmpty(newName)) return null;

            item.NewName = newName;

            UpdateReferencesWithRename(elementViewer, oldName, newName);

            return newName;
        }

        private string GetNewName(Item item, string oldName = "")
        {
            if (item == null) return null;

            var newName = InputBox.ShowDialog(("New Name for " + item.Name), "New Name", oldName);

            // Validation
            var nameExists = Structure.Structure.DataRootItem.InnerChildren.Any(child =>
                item.Name == child.Name && child.NewName == newName);
            if (!string.IsNullOrEmpty(newName) && nameExists)
                MessageBox.Show("Item could not be named.\nThe name " + newName + " already exists.");
            if (string.IsNullOrEmpty(newName) || nameExists)
                return "";

            return newName;
        }

        private void RefreshViewers()
        {
            // Change the action status label
            statusMain.Text = "Updating Controls...";

            _InitializeElementViewer();
            _UpdateElementViewer();

            statusMain.Text = "Ready";
        }

        private void UpdateReferencesWithRename(Control parent, string oldName, string newName)
        {
            // Update ReferenceControls selection.
            foreach (var control in parent.Controls)
            {
                if (control is ControlReference)
                {
                    var refControl = ((ControlReference)control);
                    if (refControl.GetValue() == oldName)
                        refControl.SetValue(newName);
                }
                else if (control is ControlGroup)
                    UpdateReferencesWithRename(((ControlGroup)control).GetPanel(), oldName, newName);
            }
        }
        #endregion

        #region DragNDrop TreeViewer

        private void treeViewer_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void treeViewer_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void treeViewer_DragDrop(object sender, DragEventArgs e)
        {
            // Check data is correct.
            if (!e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false)) return;

            TreeNode node = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
            int newIndex = -1;

            // Get target (Drop on Node).
            Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
            TreeNode targetNode = ((TreeView)sender).GetNodeAt(pt);

            // Get target index for insertion.
            newIndex = targetNode.Index;

            // Move (Remove & Add/Insert) Node.
            if (newIndex < targetNode.Parent.Nodes.Count)
            {
                // Move TreeNode
                node.Remove();
                targetNode.Parent.Nodes.Insert(newIndex, node);

                // Move Item
                var item = ((GroupItem)node.Tag);
                var parent = item.Parent;
                parent.Children.Remove(item);
                parent.Children.Insert(newIndex, item);
            }
            else
            {
                // Move TreeNode
                node.Remove();
                targetNode.Parent.Nodes.Add(node);

                // Move Item
                var item = ((GroupItem)node.Tag);
                var parent = item.Parent;
                parent.Children.Remove(item);
                parent.Children.Add(item);
            }
        }

        private void treeViewer_DragOver(object sender, DragEventArgs e)
        {
            // Check data is correct.
            if (!e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false)) return;

            TreeNode node = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");

            // Check item CAN be moved.
            if (node == null || node.Parent == null || node.Parent.Nodes.Count <= 1)
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            // Check target item can be used for move.

            // Get target (Drop on Node).
            Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
            TreeNode targetNode = ((TreeView)sender).GetNodeAt(pt);

            // Check target node exists, drag drop is inside the treeview,
            // their parent is mutual (move inside parent only), is group item and has right click actions.
            if (targetNode == null || targetNode == node || targetNode.TreeView != node.TreeView ||
                targetNode.Parent != node.Parent || !(node.Tag is GroupItem) || !((GroupItem)node.Tag).HasRightClickActions ||
                !(targetNode.Tag is GroupItem) || !((GroupItem)targetNode.Tag).HasRightClickActions)
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            e.Effect = DragDropEffects.Move;
        }

        #endregion

    }
}
