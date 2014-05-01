using System;
using System.Windows.Forms;

namespace bark_GUI.Forms.Dialogs
{
    public partial class DiagramDataDialog : Form
    {
        string _errorMsg = "";

        public string XAxisSelected { get; private set; }
        public string[] YAxisSelected { get; private set; }

        public DiagramDataDialog(string[] titles)
        {
            InitializeComponent();

            foreach (var title in titles)
            {
                xAxisDropdownList.Items.Add(title.Trim());
                yAxisListView.Items.Add(title.Trim());
            }
        }

        private bool DataIsValid()
        {
            var dataOk = true;
            string msg = "Invalid input:\n\n";

            // Checks.
            if (xAxisDropdownList.SelectedItem == null)
            {
                msg += " - Please select a X-Axis before proceeding.\n";
                dataOk = false;
            }
            if (yAxisListView.SelectedItems.Count == 0)
            {
                msg += " - Please select at least one Y-Axis before proceeding.\n";
                dataOk = false;
            }
            if (yAxisListView.SelectedItems.Count > FormDiagram.MaxNumberOfYAxis)
            {
                msg += "The maximum selection for Y-Axis' is " + FormDiagram.MaxNumberOfYAxis + ".\n";
                msg += " - Please select less Y-Axis' before proceeding.\n";
                dataOk = false;
            }

            if (!dataOk)
                _errorMsg = msg;
            else
                _errorMsg = "";

            return dataOk;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            XAxisSelected = null;
            YAxisSelected = null;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!DataIsValid()) return;

            // Save/Send selected axis'.
            XAxisSelected = xAxisDropdownList.SelectedItem.ToString();
            var items = yAxisListView.SelectedItems;
            YAxisSelected = new string[items.Count];

            for (int i = 0; i < items.Count; i++)
            {
                YAxisSelected[i] = items[i].Text;
            }
        }

        private void DiagramDataDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK && !DataIsValid())
            {
                MessageBox.Show(_errorMsg);
                e.Cancel = true;
            }
        }
    }
}
