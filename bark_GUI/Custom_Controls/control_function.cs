using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;

namespace bark_GUI.Custom_Controls
{
    public partial class control_function : custom_control
    {
        public control_function(string name, List<string> typeOptions, List<string> refOptions, bool required, string help, General_Control generalControl)
            :base(name, required, help)
        {
            InitializeComponent();

            labelName.Text = name.Trim();

            if (typeOptions != null)
            {
                foreach (string s in typeOptions)
                    comboBoxType.Items.Add(s);
                selectFunction();
                if (typeOptions.Count == 1)
                    comboBoxType.Enabled = false;
            }
            if (refOptions != null)
            {
                foreach (string s in refOptions)
                    comboBoxFunc.Items.Add(s);
                comboBoxFunc.SelectedIndex = 0;
                if (refOptions.Count == 1)
                    comboBoxFunc.Enabled = false;
            }

            if (help != null)
                toolTipHelp.SetToolTip(labelName, help);
            this.isRequired = required;
            this.generalControl = generalControl;
        }




        /* PUBLIC METHODS */
        public override void SetValue(string value) { comboBoxFunc.Text = value; }











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
            generalControl.ReplaceWith(convertToCC_Type(comboBoxType.SelectedItem.ToString()));
            selectFunction();
        }










        private void selectFunction()
        {
            for (int i = 0; i < comboBoxType.Items.Count; i++)
                if (comboBoxType.Items[i].ToString() == "Function")
                    comboBoxType.SelectedIndex = i;
        }
    }
}
