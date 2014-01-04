using System;
using System.Collections.Generic;
using System.Xml;

namespace bark_GUI.CustomControls
{
    public partial class ControlVariable : CustomControl
    {
        public ControlVariable(string name, ICollection<string> typeOptions, ICollection<string> unitOptions,
            ICollection<string> unitXOptions, bool isRequired, string help, GeneralControl generalControl)
            : base(name, isRequired, help, generalControl)
        {
            InitializeComponent();

            labelName.Text = name.Trim();
            if (typeOptions != null)
            {
                foreach (var s in typeOptions)
                    comboBoxType.Items.Add(s);
                SelectVariable();
                if (typeOptions.Count == 1)
                    comboBoxType.Enabled = false;
            }
            if (unitXOptions != null)
            {
                foreach (var s in unitXOptions)
                    comboBoxUnit.Items.Add(s);
                comboBoxUnit.SelectedIndex = 0;
                if (unitXOptions.Count == 1)
                    comboBoxUnit.Enabled = false;
            }
            if (unitOptions != null)
            {
                foreach (var s in unitOptions)
                    comboBoxUnit2.Items.Add(s);
                comboBoxUnit2.SelectedIndex = 0;
                if (unitOptions.Count == 1)
                    comboBoxUnit2.Enabled = false;
            }

            if (help != null)
                toolTipHelp.SetToolTip(labelName, help);
        }





        /* PUBLIC METHODS */
        public override void SetValue(string value) { control_variable_table.Fill(value); }
        public override void SetUnit(string unit) { comboBoxUnit2.Text = unit; }
        public override void SetX_Unit(string xUnit) { comboBoxUnit.Text = xUnit; }






        private void comboBoxUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Tag == null) return;
            var attributes = ((XmlNode) Tag).Attributes;
            if (attributes != null)
                attributes["unit"].Value = comboBoxUnit.SelectedText;  //TODO: Remove Xml Dependency.
        }
        private void comboBoxUnit2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Tag == null) return;
            XmlNode xmlNode = Tag as XmlNode;
            if (xmlNode != null)
                if (xmlNode.Attributes != null)
                    xmlNode.Attributes["x_unit"].Value = comboBoxUnit2.SelectedText;  //TODO: Remove Xml Dependency.
        }

        private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxType.SelectedItem.ToString() == "Variable")
                return;
            GeneralControl.ReplaceWith(ConvertToCustomControl_Type(comboBoxType.SelectedItem.ToString()));
            SelectVariable();
        }











        private void SelectVariable()
        {
            for (int i = 0; i < comboBoxType.Items.Count; i++)
                if (comboBoxType.Items[i].ToString() == "Variable")
                    comboBoxType.SelectedIndex = i;
        }
    }
}
