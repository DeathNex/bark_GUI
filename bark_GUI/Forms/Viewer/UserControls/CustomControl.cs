using System.Windows.Forms;
using bark_GUI.Structure.Items;

namespace bark_GUI.CustomControls
{
    public class CustomControl : UserControl        //TODO: Replace CustomControl with ICustomControl. Clean dependencies.
    {
        //Public Variables
        public new string Name { get; set; }

        public string DefaultValue
        {
            get { return _defaultValue; }
            set { _defaultValue = value; SetValue(value); }
        }

        public string Help { get; set; }

        // Inheriting Variables
        public bool IsRequired { get; set; }

        public virtual bool IsValid { get; protected set; }

        public virtual string Value { get; set; }

        //Protected Variables
        protected GeneralControl GeneralControl;

        // Private Variables
        private string _defaultValue;

        #region Constructors

        protected CustomControl() { }

        protected CustomControl(string name, bool isRequired, string help, GeneralControl generalControl)
        {
            Name = name;
            IsRequired = isRequired;
            Help = help;
            GeneralControl = generalControl;
        }

        #endregion

        protected CustomControlType ConvertToCustomControl_Type(string stringType)
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
        public ValueValidator Validator;
        public virtual void SetValue(string value) { }
        public virtual void SetUnit(string unit) { }
        public virtual void SetX_Unit(string xUnit) { }
        public virtual void SetControlName(string name) { }
        public virtual bool HasNewValue() { return false; }
        public virtual void UpdateValues() { }
        public void Remove()
        {
            if (Parent != null && Parent.Controls.Contains(this))
                Parent.Controls.Remove(this);
            Dispose(true);
        }
    }
}
