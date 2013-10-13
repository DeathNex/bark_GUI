using System;
using System.Collections.Generic;
using System.Xml;

namespace bark_GUI.Custom_Controls
{
    public partial class control_constant : custom_control
    {
        public control_constant(string name, List<string> typeOptions, List<string> unitOptions, bool required, string help, General_Control generalControl)
        {
            InitializeComponent();

            labelName.Text = name.Trim();
            if (typeOptions != null)
            {
                foreach (string s in typeOptions)
                    comboBoxType.Items.Add(s);
                selectConstant();
                if (typeOptions.Count == 1)
                    comboBoxType.Enabled = false;
            }
            else
            { } //Unfinished, "Keyword" element doesnt have types because it's not constant! (<function><ASHRAE..><side><keyword>front)
            //textBoxValue.Text = value.Trim();
            if (unitOptions != null)
            {
                foreach (string s in unitOptions)
                    comboBoxUnit.Items.Add(s);
                comboBoxUnit.SelectedIndex = 0;
                if (unitOptions.Count == 1)
                    comboBoxUnit.Enabled = false;
            }
            if (help != null)
                toolTipHelp.SetToolTip(labelName, help);
            this.isRequired = required;
            this.generalControl = generalControl;
        }







        /* PUBLIC METHODS */
        public override void SetValue(string value) { textBoxValue.Text = value; }
        public override void SetUnit(string unit) { comboBoxUnit.Text = unit; }








        /* Apply Changes to the XmlDocument */

        private void textBoxValue_TextChanged(object sender, EventArgs e)
        {
            if (Tag != null)
            {
                (Tag as XmlNode).FirstChild.FirstChild.Value = textBoxValue.Text.Trim().TrimStart(new char[1] { '0' });
            }
        }
        private void comboBoxUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Tag != null)
            {
                (Tag as XmlNode).Attributes.GetNamedItem("unit").Value = comboBoxUnit.SelectedText.Trim();
            }
        }

        private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxType.SelectedItem.ToString() == "Constant")
                return;
            generalControl.ReplaceWith(convertToCC_Type(comboBoxType.SelectedItem.ToString()));
            selectConstant();
        }






        /* PRIVATE UTILITY METHODS */

        private void selectConstant()
        {
            for (int i = 0; i < comboBoxType.Items.Count; i++)
                if (comboBoxType.Items[i].ToString() == "Constant")
                    comboBoxType.SelectedIndex = i;
        }
    }
}
