using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace bark_GUI.Custom_Controls
{
    public partial class custom_control : UserControl
    {
        //Public Variables
        public bool IsRequired {get{return isRequired;}}

        //Protected Variables
        protected bool isRequired;
        protected General_Control generalControl;

        //Private Variables
        private string name;
        private string help;
        

        //Constructors
        protected custom_control() { }
        protected custom_control(string name, bool isRequired, string help)
        {
            this.name = name;
            this.isRequired = isRequired;
            this.help = help;
        }

        protected custom_control_type convertToCC_Type(string stype)
        {
            switch (stype)
            {
                case "Constant":
                case "constant":
                    return custom_control_type.constant;
                case "Variable":
                case "variable":
                    return custom_control_type.variable;
                case "Function":
                case "function":
                    return custom_control_type.function;
                case "Group":
                case "group":
                    return custom_control_type.group;
                case "Reference":
                case "reference":
                    return custom_control_type.reference;
                case "Keyword":
                case "keyword":
                    return custom_control_type.keyword;
            }
            //Error
            return custom_control_type.group;
        }

        /* INHERITING METHODS */
        public virtual void SetValue(string value) { }
        public virtual void SetUnit(string unit) { }
        public virtual void SetX_Unit(string unit) { }
    }
}
