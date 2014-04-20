using System;
using System.Collections.Generic;
using System.Xml;

namespace bark_GUI.CustomControls
{
    public partial class ControlReference : CustomControl
    {
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
        public override void SetValue(string value) { comboBoxValue.Text = value; }

        public void SetOptions(List<string> options)
        {
            var selectedText = comboBoxValue.Text;

            foreach (var s in options)
                comboBoxValue.Items.Add(s.Trim());
            comboBoxValue.SelectedIndex = 0;

            comboBoxValue.Text = selectedText;
        }






        private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Tag == null) return;
            var attributes = ((XmlNode) Tag).Attributes;
            if (attributes != null)
                attributes["reference"].Value = comboBoxValue.SelectedItem.ToString();  //TODO: Remove Xml Dependency.
        }
    }
}
