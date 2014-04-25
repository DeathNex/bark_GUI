using System;
using System.Collections.Generic;

namespace bark_GUI.CustomControls
{
    public partial class ControlFunction : CustomControl
    {
        public ControlFunction(string name, ICollection<string> typeOptions, ICollection<string> refOptions,
            bool isRequired, string help, GeneralControl generalControl)
            :base(name, isRequired, help, generalControl)
        {
            InitializeComponent();

            labelName.Text = name.Trim();

            if (typeOptions != null)
            {
                foreach (var s in typeOptions)
                    comboBoxType.Items.Add(s);
                SelectFunction();
                if (typeOptions.Count == 1)
                    comboBoxType.Enabled = false;
            }
            if (refOptions != null)
            {
                foreach (var s in refOptions)
                    comboBoxFunc.Items.Add(s);
                comboBoxFunc.SelectedIndex = 0;
                if (refOptions.Count == 1)
                    comboBoxFunc.Enabled = false;
            }

            if (help != null)
                toolTipHelp.SetToolTip(labelName, help);
        }




        /* PUBLIC METHODS */
        public override void SetValue(string value) { if (!string.IsNullOrEmpty(value)) comboBoxFunc.Text = value; }
        // Set the Control's name for the Element Viewer.
        public override void SetControlName(string name)
        {
            Name = name;
            labelName.Text = name;
        }
        public override bool HasNewValue()
        {
            // Check if the control exists and has a value.
            if (comboBoxFunc == null || comboBoxFunc.SelectedItem == null) return false;

            // Check if the value is not empty and is not the default.
            var valueIsNew = !string.IsNullOrEmpty(comboBoxFunc.SelectedItem.ToString().Trim()) &&
                               (comboBoxFunc.SelectedItem.ToString().Trim() != DefaultValue);

            // Return true if value changed.
            return valueIsNew;
        }











        private void comboBoxFunc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Tag != null)
            {
                //(Tag as XmlNode).FirstChild.FirstChild.Name = comboBoxFunc.SelectedItem.ToString();
            }
        }

        private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxType.SelectedItem.ToString() == "Function" || comboBoxType.DroppedDown)
                return;
            GeneralControl.ReplaceWith(ConvertToCustomControl_Type(comboBoxType.SelectedItem.ToString()));
            SelectFunction();
        }










        private void SelectFunction()
        {
            for (int i = 0; i < comboBoxType.Items.Count; i++)
                if (comboBoxType.Items[i].ToString() == "Function")
                    comboBoxType.SelectedIndex = i;
        }
    }
}
