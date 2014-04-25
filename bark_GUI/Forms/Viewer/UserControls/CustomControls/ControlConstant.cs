using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;

namespace bark_GUI.CustomControls
{
    public partial class ControlConstant : CustomControl
    {
        // Public Variables
        public string DefaultUnit {
            get { return _defaultUnit; }
            set { _defaultUnit = value; SetUnit(value); }
        }

        // Private Variables
        private string _defaultUnit;

        // Constructor
        public ControlConstant(string name, List<string> typeOptions, List<string> unitOptions,
            bool isRequired, string help, GeneralControl generalControl)
            :base(name, isRequired, help, generalControl)
        {
            InitializeComponent();

            // Checks
            Debug.Assert(typeOptions != null, "Control_Constant for element {0} constructor argument typeOptions is null.", name);
            Debug.Assert(unitOptions != null, "Control_Constant for element {0} constructor argument unitOptions is null.", name);

            // Set name.
            labelName.Text = name.Trim();

            // Set possible types.
            foreach (string s in typeOptions)
                comboBoxType.Items.Add(s);

            // Since this is the control for 'Constant' type, have it selected.
            SelectConstant();

            // Visual candy.
            if (typeOptions.Count == 1)
                comboBoxType.Enabled = false;
            
            // Set unit.
            foreach (string s in unitOptions)
                    comboBoxUnit.Items.Add(s);
                comboBoxUnit.SelectedIndex = 0;
                if (unitOptions.Count == 1)
                    comboBoxUnit.Enabled = false;
            
            // Set help
            if (!string.IsNullOrEmpty(help))
                toolTipHelp.SetToolTip(labelName, help);
        }







        /* PUBLIC METHODS */
        public override void SetValue(string value) { if(!string.IsNullOrEmpty(value)) textBoxValue.Text = value; }
        public override void SetUnit(string unit) { if (!string.IsNullOrEmpty(unit)) comboBoxUnit.Text = unit; }
        // Set the Control's name for the Element Viewer.
        public override void SetControlName(string name) { Name = name; labelName.Text = name; }
        public override bool HasNewValue()
        {
            // Check if the value is not empty and is not the default.
            var valueIsNew = !string.IsNullOrEmpty(textBoxValue.Text.Trim()) &&
                               (textBoxValue.Text.Trim() != DefaultValue);

            // Check if the unit is not empty and is not the default.
            var unitIsNew = !string.IsNullOrEmpty(comboBoxUnit.Text) &&
                               (comboBoxUnit.Text != DefaultUnit);

            // Return true if ANYTHING changed.
            return valueIsNew || unitIsNew;
        }







        /* Apply Changes to the XmlDocument */

        private void textBoxValue_TextChanged(object sender, EventArgs e)
        {
            // Check.
            if (Tag == null) return;

            // Update the XML Element inside the XML Document.
            ((XmlNode) Tag).FirstChild.FirstChild.Value = textBoxValue.Text.Trim().TrimStart(new char[1] { '0' });
        }

        private void comboBoxUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check.
            if (Tag == null) return;

            // Update the XML Element inside the XML Document.
            var attributes = ((XmlNode) Tag).Attributes;
            if (attributes != null)
                attributes["unit"].Value = comboBoxUnit.SelectedText.Trim();
        }

        private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxType.SelectedItem.ToString() == "Constant")
                return;
            GeneralControl.ReplaceWith(ConvertToCustomControl_Type(comboBoxType.SelectedItem.ToString()));
            SelectConstant();
        }






        /* PRIVATE UTILITY METHODS */

        private void SelectConstant()
        {
            for (int i = 0; i < comboBoxType.Items.Count; i++)
                if (comboBoxType.Items[i].ToString() == "Constant")
                    comboBoxType.SelectedIndex = i;
        }
    }
}
