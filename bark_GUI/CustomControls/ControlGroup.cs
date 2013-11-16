namespace bark_GUI.CustomControls
{
    public partial class ControlGroup : CustomControl
    {
        public ControlGroup(string name, bool isRequired, GeneralControl generalControl)
        {
            InitializeComponent();
            labelGroup.Text = name;
            this.isRequired = isRequired;
            this.GeneralControl = generalControl;
        }
    }
}
