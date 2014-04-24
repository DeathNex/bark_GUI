using System.Linq;
using System.Windows.Forms;

namespace bark_GUI.CustomControls
{
    public partial class ControlGroup : CustomControl
    {
        public ControlGroup(string name, bool isRequired, GeneralControl generalControl)
            : base(name, isRequired, null, generalControl)
        {
            InitializeComponent();
            labelGroup.Text = name;
        }

        // Set the Control's name for the Element Viewer.
        public override void SetControlName(string name)
        {
            Name = name;
            labelGroup.Text = name;
        }

        public Control GetPanel() { return panelGroup; }

        // Public methods
        public override bool HasValue()
        {
            var anyChildHasValue = false;

            // Iterate the child controls to check if any of these have a value.
            foreach (var control in panelGroup.Controls)
            {
                var childHasValue = false;
                if (!(control is CustomControl)) continue;

                childHasValue = (control as CustomControl).HasValue();

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
