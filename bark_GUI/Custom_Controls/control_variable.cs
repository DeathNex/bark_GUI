using System;
using System.Collections.Generic;
using System.Xml;

namespace bark_GUI.Custom_Controls
{
    public partial class control_variable : custom_control
    {
        public control_variable(string name, List<string> typeOptions, List<string> unitOptions, List<string> unitXOptions, bool required, string help, General_Control generalControl)
        {
            InitializeComponent();

            labelName.Text = name.Trim();
            if (typeOptions != null)
            {
                foreach (string s in typeOptions)
                    comboBoxType.Items.Add(s);
                selectVariable();
                if (typeOptions.Count == 1)
                    comboBoxType.Enabled = false;
            }
            if (unitXOptions != null)
            {
                foreach (string s in unitXOptions)
                    comboBoxUnit.Items.Add(s);
                comboBoxUnit.SelectedIndex = 0;
                if (unitXOptions.Count == 1)
                    comboBoxUnit.Enabled = false;
            }
            if (unitOptions != null)
            {
                foreach (string s in unitOptions)
                    comboBoxUnit2.Items.Add(s);
                comboBoxUnit2.SelectedIndex = 0;
                if (unitOptions.Count == 1)
                    comboBoxUnit2.Enabled = false;
            }

            if (help != null)
                toolTipHelp.SetToolTip(labelName, help);
            this.isRequired = required;
            this.generalControl = generalControl;
        }





        /* PUBLIC METHODS */
        public override void SetValue(string value) { control_variable_table.Fill(value); }
        public override void SetUnit(string unit) { comboBoxUnit2.Text = unit; }
        public override void SetX_Unit(string x_unit) { comboBoxUnit.Text = x_unit; }






        private void comboBoxUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Tag != null)
            {
                (Tag as XmlNode).Attributes.GetNamedItem("unit").Value = comboBoxUnit.SelectedText;
            }
        }
        private void comboBoxUnit2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Tag != null)
            {
                (Tag as XmlNode).Attributes.GetNamedItem("unit2").Value = comboBoxUnit2.SelectedText;
            }
        }

        private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxType.SelectedItem.ToString() == "Variable")
                return;
            generalControl.ReplaceWith(convertToCC_Type(comboBoxType.SelectedItem.ToString()));
            selectVariable();
        }











        private void selectVariable()
        {
            for (int i = 0; i < comboBoxType.Items.Count; i++)
                if (comboBoxType.Items[i].ToString() == "Variable")
                    comboBoxType.SelectedIndex = i;
        }
    }
}
