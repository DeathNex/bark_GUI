using System;
using System.Collections.Generic;
using System.Xml;

namespace bark_GUI.CustomControls
{
    public partial class ControlConstant : CustomControl
    {
        public ControlConstant(string name, List<string> typeOptions, List<string> unitOptions, bool isRequired, string help, GeneralControl generalControl)
            :base(name, isRequired, help, generalControl)
        {
            InitializeComponent();

            labelName.Text = name.Trim();
            if (typeOptions != null)
            {
                foreach (string s in typeOptions)
                    comboBoxType.Items.Add(s);
                SelectConstant();
                if (typeOptions.Count == 1)
                    comboBoxType.Enabled = false;
            }
            else
            { } //TODO: Unfinished, "Keyword" element doesnt have types because it's not constant! (<function><ASHRAE..><side><keyword>front)
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
        }







        /* PUBLIC METHODS */
        public override void SetValue(string value) { textBoxValue.Text = value; }
        public override void SetUnit(string unit) { comboBoxUnit.Text = unit; }








        /* Apply Changes to the XmlDocument */

        private void textBoxValue_TextChanged(object sender, EventArgs e)
        {
            if (Tag != null)
            {
                ((XmlNode) Tag).FirstChild.FirstChild.Value = textBoxValue.Text.Trim().TrimStart(new char[1] { '0' });
            }
        }
        private void comboBoxUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Tag == null) return;
            XmlAttributeCollection attributes = ((XmlNode) Tag).Attributes;
            if (attributes != null)
                attributes["unit"].Value = comboBoxUnit.SelectedText.Trim();  //TODO: Remove Xml Dependency.
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
