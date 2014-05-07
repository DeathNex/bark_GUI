using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace bark_GUI.CustomControls
{
    public partial class ControlGroup : CustomControl
    {
        public override bool IsValid
        {
            get
            {
                var controlValuesAreValid = true;

                // Iterate the children to check if they are valid.
                foreach (var control in panelGroup.Controls.OfType<CustomControl>())
                {
                    // IsValid logic.
                    if (control is ControlGroup)
                    {
                        controlValuesAreValid = ((ControlGroup)control).IsValid;
                    }
                    else
                    {
                        if (!control.IsValid && !(string.IsNullOrEmpty(control.Value) && !control.IsRequired))
                            controlValuesAreValid = false;
                    }
                }

                // Check if the GroupItem is not required and it's items are empty.
                if (IsRequired == false && controlValuesAreValid == false)
                {
                    controlValuesAreValid = true;

                    if(panelGroup.Controls.OfType<CustomControl>().Any(control =>
                        !string.IsNullOrEmpty(control.Value) || control is ControlGroup))
                    {
                        controlValuesAreValid = false;
                    }
                }

                return controlValuesAreValid;
            }
        }

        public ControlGroup(string name, bool isRequired, GeneralControl generalControl)
            : base(name, isRequired, null, generalControl)
        {
            InitializeComponent();
            labelGroup.Text = name;

            if (isRequired) labelGroup.Font = new Font(labelGroup.Font, FontStyle.Bold);
            else labelGroup.Font = new Font(labelGroup.Font, FontStyle.Regular);

            IsValid = true;
        }

        // Set the Control's name for the Element Viewer.
        public override void SetControlName(string name)
        {
            Name = name;
            labelGroup.Text = name;
        }

        public Control GetPanel() { return panelGroup; }

        // Public methods
        public override bool HasNewValue()
        {
            var anyChildHasValue = false;

            // Iterate the child controls to check if any of these have a value.
            foreach (var control in panelGroup.Controls)
            {
                var childHasValue = false;
                if (!(control is CustomControl)) continue;

                childHasValue = (control as CustomControl).HasNewValue();

                if (childHasValue)
                {
                    anyChildHasValue = true;
                    break;
                }
            }
            return anyChildHasValue;
        }

        public bool HasChildrenOfElementItem()
        {
            var anyChildElementExists = false;

            // Iterate the child controls to check if any element control exists.
            foreach (CustomControl control in panelGroup.Controls.OfType<CustomControl>())
            {
                if (control is ControlGroup)
                    anyChildElementExists = (control as ControlGroup).HasChildrenOfElementItem();
                else
                    anyChildElementExists = true;

                if (anyChildElementExists) break;
            }

            return anyChildElementExists;
        }
    }
}
