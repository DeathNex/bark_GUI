using System;
using System.Collections.Generic;
using System.Xml;

namespace bark_GUI.Custom_Controls
{
    public partial class control_reference : custom_control
    {
        public control_reference(string name, bool required, string help, General_Control generalControl)
            : base(name, required, help)
        {
            InitializeComponent();

            labelName.Text = name.Trim();
            /* USE IN XML LOAD NOT XSD
            if (options != null)
            {
                foreach (string s in options)
                    comboBoxValue.Items.Add(s.Trim());
                comboBoxValue.SelectedIndex = 0;
            }
            */

            if (help != null)
                toolTipHelp.SetToolTip(labelName, help);
            this.isRequired = required;
            this.generalControl = generalControl;
        }





        /* PUBLIC METHODS */
        public override void SetValue(string value) { comboBoxValue.Text = value; }






        private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Tag != null)
            {
                (Tag as XmlNode).Attributes.GetNamedItem("reference").Value = comboBoxValue.SelectedItem.ToString();
            }
        }
    }
}
