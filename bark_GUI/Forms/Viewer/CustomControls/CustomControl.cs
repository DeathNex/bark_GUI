using System.Windows.Forms;

namespace bark_GUI.CustomControls
{
    public class CustomControl : UserControl
    {
        //Public Variables
        public bool IsRequired {get{return isRequired;}}

        //Protected Variables
        protected bool isRequired;
        protected GeneralControl GeneralControl;

        //Private Variables
        private string _name;
        private string _help;
        

        //Constructors
        protected CustomControl() { }
        protected CustomControl(string name, bool isRequired, string help)
        {
            this._name = name;
            this.isRequired = isRequired;
            this._help = help;
        }

        protected CustomControlType convertToCustomControl_Type(string stringType)
        {
            switch (stringType)
            {
                case "Constant":
                case "constant":
                    return CustomControlType.Constant;
                case "Variable":
                case "variable":
                    return CustomControlType.Variable;
                case "Function":
                case "function":
                    return CustomControlType.Function;
                case "Group":
                case "group":
                    return CustomControlType.Group;
                case "Reference":
                case "reference":
                    return CustomControlType.Reference;
                case "Keyword":
                case "keyword":
                    return CustomControlType.Keyword;
            }
            //Error
            return CustomControlType.Group;
        }

        /* INHERITING METHODS */
        public virtual void SetValue(string value) { }
        public virtual void SetUnit(string unit) { }
        public virtual void SetX_Unit(string xUnit) { }
    }
}
