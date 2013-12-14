using System;
using System.Xml;

namespace bark_GUI.CustomControls
{
    public partial class ControlReference : CustomControl
    {
        public ControlReference(string name, bool isRequired, string help, GeneralControl generalControl)
            : base(name, isRequired, help)
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
            this.isRequired = isRequired;
            this.GeneralControl = generalControl;
        }





        /* PUBLIC METHODS */
        public override void SetValue(string value) { comboBoxValue.Text = value; }






        private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Tag == null) return;
            var attributes = ((XmlNode) Tag).Attributes;
            if (attributes != null)
                attributes["reference"].Value = comboBoxValue.SelectedItem.ToString();  //TODO: Remove Xml Dependency.
        }
    }
}
