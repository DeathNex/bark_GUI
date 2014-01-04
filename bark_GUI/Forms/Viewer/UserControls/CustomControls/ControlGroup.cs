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
    }
}
