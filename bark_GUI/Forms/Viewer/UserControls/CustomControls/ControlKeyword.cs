using System;
using System.Collections.Generic;
using System.Xml;

namespace bark_GUI.CustomControls
{
    public partial class ControlKeyword : CustomControl
    {
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
        public override void SetValue(string value) { comboBoxValue.Text = value; }






        private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Tag != null)
            {
                var attributes = ((XmlNode)Tag).Attributes;
                if (attributes != null)
                    attributes["reference"].Value = comboBoxValue.SelectedItem.ToString();  //TODO: Remove Xml Dependency.
            }
        }
    }
}
