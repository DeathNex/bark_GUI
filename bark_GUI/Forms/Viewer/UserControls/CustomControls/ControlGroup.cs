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
    }
}
