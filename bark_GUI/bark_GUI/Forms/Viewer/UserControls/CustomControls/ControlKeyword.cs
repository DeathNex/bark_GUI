﻿using System;
using System.Collections.Generic;

namespace bark_GUI.CustomControls
{
    public partial class ControlKeyword : CustomControl
    {
        public override string Value
        {
            get { return comboBoxValue.Text.Trim(); }
            set { comboBoxValue.Text = value; }
        }

        public ControlKeyword(string name, IEnumerable<string> options, bool isRequired, string help, GeneralControl generalControl)
            : base(name, isRequired, help, generalControl)
        {
            InitializeComponent();

            labelName.Text = name.Trim();
            if (options != null)
            {
                foreach (var s in options)
                    comboBoxValue.Items.Add(s.Trim());
                comboBoxValue.SelectedIndex = 0;
            }

            if (help != null)
                toolTipHelp.SetToolTip(labelName, help);
        }




        /* PUBLIC METHODS */
        public override void SetValue(string value) { if (!string.IsNullOrEmpty(value)) comboBoxValue.Text = value; }
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
            // SimpleType Validation & Save
            if (Validator != null)
                IsValid = Validator(comboBoxValue.Text.Trim());
        }




    }
}
