using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using bark_GUI.Structure.Items;

namespace bark_GUI.CustomControls
{
    public partial class ControlVariable : CustomControl
    {
        public override string Value
        {
            get
            {
                dataGridViewValue.SelectAll();
                var clipboardContent = dataGridViewValue.GetClipboardContent();
                dataGridViewValue.ClearSelection();
                if (clipboardContent != null)
                    return clipboardContent.GetText().Replace('\t', ' ');
                return "";
            }
            set
            {
                var lines = value.Replace("\t", " ").Split(new string[1] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string row in lines)
                    dataGridViewValue.Rows.Add(row.Split(new string[1] { " " }, StringSplitOptions.RemoveEmptyEntries));
                dataGridViewValue_CellValueChanged(null, null);
            }
        }

        // Public Variables
        public string DefaultUnit
        {
            get { return _defaultUnit; }
            set
            {
                _defaultUnit = value;
                SetUnit(value);
            }
        }

        public string DefaultXUnit
        {
            get { return _defaultXUnit; }
            set
            {
                _defaultXUnit = value;
                SetX_Unit(value);
            }
        }

        public SaveVariable SaveVariableTable;

        public UnitChange UnitChange;

        public XUnitChange XUnitChange;

        // Private Variables
        private string _defaultUnit;
        private string _defaultXUnit;

        public ControlVariable(string name, ICollection<string> typeOptions, ICollection<string> unitOptions,
                               ICollection<string> unitXOptions, bool isRequired, string help,
                               GeneralControl generalControl)
            : base(name, isRequired, help, generalControl)
        {
            InitializeComponent();

            IsRequired = isRequired;

            labelName.Text = name.Trim();
            if (typeOptions != null)
            {
                foreach (var s in typeOptions)
                    comboBoxType.Items.Add(s);
                SelectVariable();
                if (typeOptions.Count == 1)
                    comboBoxType.Enabled = false;
            }
            if (unitXOptions != null)
            {
                foreach (var s in unitXOptions)
                    comboBoxUnit.Items.Add(s);
                comboBoxUnit.SelectedIndex = 0;
                if (unitXOptions.Count == 1)
                    comboBoxUnit.Enabled = false;
            }
            if (unitOptions != null)
            {
                foreach (var s in unitOptions)
                    comboBoxUnit2.Items.Add(s);
                comboBoxUnit2.SelectedIndex = 0;
                if (unitOptions.Count == 1)
                    comboBoxUnit2.Enabled = false;
            }

            if (help != null)
                toolTipHelp.SetToolTip(labelName, help);

            dataGridViewValue_CellValueChanged(null, null);
        }





        /* PUBLIC METHODS */
        public override void SetValue(string value) { Value = value; }
        public override void SetUnit(string unit) { if (!string.IsNullOrEmpty(unit)) comboBoxUnit2.Text = unit; }
        public override void SetX_Unit(string xUnit) { if (!string.IsNullOrEmpty(xUnit)) comboBoxUnit.Text = xUnit; }
        // Set the Control's name for the Element Viewer.
        public override void SetControlName(string name)
        {
            Name = name;
            labelName.Text = name;
        }

        public override bool HasNewValue()
        {
            var hasValue = dataGridViewValue.Rows.Cast<DataGridViewRow>().Any(row => row.Cells.Cast<DataGridViewCell>().Any(cell => cell.Value != null && !string.IsNullOrEmpty(cell.Value.ToString().Trim())));

            var valueIsNew = hasValue && Value != DefaultValue;
            var unitIsNew = !string.IsNullOrEmpty(comboBoxUnit2.Text) &&
                            (comboBoxUnit2.Text != DefaultUnit);
            var xUnitIsNew = !string.IsNullOrEmpty(comboBoxUnit.Text) &&
                             (comboBoxUnit.Text != DefaultXUnit);

            return valueIsNew || unitIsNew || xUnitIsNew;
        }

        public override void UpdateValues()
        {
            dataGridViewValue_CellValueChanged(null, null);
            comboBoxUnit_SelectedIndexChanged(null, null);
            comboBoxUnit2_SelectedIndexChanged(null, null);
        }






        private void comboBoxUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxUnit.SelectedItem == null) return;

            var value = comboBoxUnit.SelectedItem.ToString().Trim();

            if (string.IsNullOrEmpty(value)) return;

            if (XUnitChange != null)
                XUnitChange(value);
        }

        private void comboBoxUnit2_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBoxUnit2.SelectedItem == null) return;

            var value = comboBoxUnit2.SelectedItem.ToString().Trim();

            if (string.IsNullOrEmpty(value)) return;

            if (UnitChange != null)
                UnitChange(value);
        }

        private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxType.SelectedItem.ToString() == "Variable")
                return;
            GeneralControl.ReplaceWith(ConvertToCustomControl_Type(comboBoxType.SelectedItem.ToString()));
            SelectVariable();
        }

        private void dataGridViewValue_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var arrayIsValid = true;

            // Suppress empty rows
            for (int i = 0; i < dataGridViewValue.Rows.Count; i++)
            {
                DataGridViewRow row = dataGridViewValue.Rows[i];
                if (row.IsNewRow) continue;

                var removeRow = row.Cells.Cast<DataGridViewCell>().All(cell => cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString().Trim()));

                if (removeRow)
                    dataGridViewValue.Rows.Remove(row);
            }

            // Iterate cells and check if they are not empty and valid.
            foreach (DataGridViewRow row in dataGridViewValue.Rows)
            {
                var rowIsEmpty = true;

                foreach (DataGridViewCell cell in row.Cells)
                {
                    var value = cell.Value != null ? cell.Value.ToString().Trim() : "";

                    cell.Style.BackColor = Color.White;
                    cell.Style.ForeColor = Color.Black;
                    cell.Style.SelectionBackColor = Color.PaleTurquoise;
                    cell.Style.SelectionForeColor = Color.Navy;

                    if (row.IsNewRow) continue;

                    // Item Required Validation
                    if (string.IsNullOrEmpty(value) && (IsRequired || !rowIsEmpty))
                    {
                        cell.Style.BackColor = Color.Tomato;
                        cell.Style.SelectionBackColor = Color.DarkRed;
                        arrayIsValid = false;
                        continue;
                    }

                    rowIsEmpty = false;

                    // SimpleType Validation
                    if (Validator != null && Validator(value) == false)
                    {
                        cell.Style.ForeColor = Color.Red;
                        cell.Style.SelectionForeColor = Color.DeepPink;
                        arrayIsValid = false;
                    }
                }
            }

            // Check if all cells are empty and this item is required.
            if (IsRequired && dataGridViewValue.Rows.Count == 1)
            {
                arrayIsValid = false;

                foreach (DataGridViewCell cell in dataGridViewValue.Rows[0].Cells)
                    cell.Style.BackColor = Color.Tomato;
            }

            IsValid = arrayIsValid;

            // Check if the text boxes are not valid, don't save anything.
            if (!arrayIsValid) return;

            // Save variable table.
            if (SaveVariableTable != null)
                SaveVariableTable(Value);
        }

        private void dataGridViewValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.Control || e.KeyCode != Keys.V) return;

            var text = Clipboard.GetText(TextDataFormat.Text);

            if(!text.Contains("\n") && !text.Contains("\t")) return;

            string[] cb = text.Split(new string[1] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            dataGridViewValue.CancelEdit();
            dataGridViewValue.Rows.Clear();

            foreach (string row in cb)
                dataGridViewValue.Rows.Add(row.Split(new string[1] { "\t" }, StringSplitOptions.None));
            dataGridViewValue_CellValueChanged(null, null);
        }









        private void SelectVariable()
        {
            for (int i = 0; i < comboBoxType.Items.Count; i++)
                if (comboBoxType.Items[i].ToString() == "Variable")
                    comboBoxType.SelectedIndex = i;
        }
    }
}
