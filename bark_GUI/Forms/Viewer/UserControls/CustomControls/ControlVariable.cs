using System;
using System.Collections.Generic;
using System.Xml;
using bark_GUI.Structure.ItemTypes;

namespace bark_GUI.CustomControls
{
    public partial class ControlVariable : CustomControl
    {
        // Public Variables
        public string DefaultUnit
        {
            get { return _defaultUnit; }
            set { _defaultUnit = value; SetUnit(value); }
        }
        public string DefaultXUnit
        {
            get { return _defaultXUnit; }
            set { _defaultXUnit = value; SetX_Unit(value); }
        }

        public new ValueValidator Validator
        {
            get { return control_variable_table.Validator; }
            set { control_variable_table.Validator = value; }
        }

        // Private Variables
        private string _defaultUnit;
        private string _defaultXUnit;

        public ControlVariable(string name, ICollection<string> typeOptions, ICollection<string> unitOptions,
            ICollection<string> unitXOptions, bool isRequired, string help, GeneralControl generalControl)
            : base(name, isRequired, help, generalControl)
        {
            InitializeComponent();

            control_variable_table.IsRequired = isRequired;

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
        }





        /* PUBLIC METHODS */
        public override void SetValue(string value) { if (!string.IsNullOrEmpty(value)) control_variable_table.Fill(value); }
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
            var valueIsNew = control_variable_table.HasValue() && control_variable_table.GetValue() != DefaultValue;
            var unitIsNew = !string.IsNullOrEmpty(comboBoxUnit2.Text) &&
                               (comboBoxUnit2.Text != DefaultUnit);
            var xUnitIsNew = !string.IsNullOrEmpty(comboBoxUnit.Text) &&
                               (comboBoxUnit.Text != DefaultXUnit);

            return valueIsNew || unitIsNew || xUnitIsNew;
        }






        private void comboBoxUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Tag == null) return;
            var attributes = ((XmlNode) Tag).Attributes;
            if (attributes != null)
                attributes["unit"].Value = comboBoxUnit.SelectedText;  //TODO: Remove Xml Dependency.
        }
        private void comboBoxUnit2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Tag == null) return;
            XmlNode xmlNode = Tag as XmlNode;
            if (xmlNode != null)
                if (xmlNode.Attributes != null)
                    xmlNode.Attributes["x_unit"].Value = comboBoxUnit2.SelectedText;  //TODO: Remove Xml Dependency.
        }

        private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxType.SelectedItem.ToString() == "Variable")
                return;
            GeneralControl.ReplaceWith(ConvertToCustomControl_Type(comboBoxType.SelectedItem.ToString()));
            SelectVariable();
        }











        private void SelectVariable()
        {
            for (int i = 0; i < comboBoxType.Items.Count; i++)
                if (comboBoxType.Items[i].ToString() == "Variable")
                    comboBoxType.SelectedIndex = i;
        }
    }
}
