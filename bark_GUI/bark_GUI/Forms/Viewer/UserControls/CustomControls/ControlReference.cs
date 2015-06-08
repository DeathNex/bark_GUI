using System;
using System.Collections.Generic;
using System.Xml;

namespace bark_GUI.CustomControls
{
    public partial class ControlReference : CustomControl
    {
        public override string Value
        {
            get { return comboBoxValue.Text.Trim(); }
            set { comboBoxValue.Text = value; }
        }

        // This variable is used in the 'SetValue' and 'SetOptions' methods.
        // Because the SetValue can be called before the SetOptions, so a temporary save is required.
        private string _tmpSetValue = "";

        public ControlReference(string name, bool isRequired, string help, GeneralControl generalControl)
            : base(name, isRequired, help, generalControl)
        {
            InitializeComponent();

            labelName.Text = name.Trim();
            /* USE IN XML LOAD NOT XSD
            if (options != null)
            {
                
            }
            */

            if (help != null)
                toolTipHelp.SetToolTip(labelName, help);
        }





        /* PUBLIC METHODS */
        public string GetValue()
        {
            if (comboBoxValue.Items.Count > 0 && comboBoxValue.SelectedItem != null)
                return comboBoxValue.SelectedItem.ToString();
            return null;
        }

        public override void SetValue(string value)
        {
            if (string.IsNullOrEmpty(value)) return;

            // Because this method can be called even before the reference control has any options,
            // check if this option exists. If it doesn't save it temporarily to set it later.
            if (comboBoxValue.Items.Count > 0 && comboBoxValue.Items.Contains(value)) comboBoxValue.Text = value;
            else _tmpSetValue = value;
        }

        public void SetOptions(List<string> options)
        {
            // Clear previous values and 
            if (comboBoxValue.Items.Count > 0)
            {
                comboBoxValue.Items.Clear();
            }

            // Because this method can be called after the reference control was already set to a value,
            // check if a temporary saved value exists. If it does exist, use it.
            foreach (var s in options)
                comboBoxValue.Items.Add(s.Trim());

            if (!string.IsNullOrEmpty(_tmpSetValue)) comboBoxValue.Text = _tmpSetValue;
        }

        // Set the Control's name for the Element Viewer.
        public override void SetControlName(string name)
        {
            Name = name;
            labelName.Text = name;
        }

        public override bool HasNewValue()
        {
            // Check if the control exists and has a value.
            if (comboBoxValue == null || comboBoxValue.SelectedItem == null) return false;

            // Check if the value is not empty and is not the default.
            var valueIsNew = !string.IsNullOrEmpty(comboBoxValue.SelectedItem.ToString().Trim()) &&
                               (comboBoxValue.SelectedItem.ToString().Trim() != DefaultValue);

            // Return true if value changed.
            return valueIsNew;
        }
        public override void UpdateValues()
        {
            comboBoxValue_SelectedIndexChanged(null, null);
        }






        private void comboBoxValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check if the control exists and has a value.
            if (comboBoxValue == null || comboBoxValue.SelectedItem == null ||
                string.IsNullOrEmpty(comboBoxValue.Text)) return;

            // SimpleType Validation & Save
            if (Validator != null)
                IsValid = Validator(comboBoxValue.Text.Trim());
        }
    }
}
