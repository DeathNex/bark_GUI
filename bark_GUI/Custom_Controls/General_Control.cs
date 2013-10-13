using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using bark_GUI.Custom_Controls;

namespace bark_GUI
{
    public enum custom_control_type { constant, variable, function, group, reference, keyword }

    public class General_Control
    {
        //Public Variables
        public string name;
        public bool isRequired;
        public string help;
        public custom_control currentControl;


        //Private Variables
        control_constant c_constant;
        control_variable c_variable;
        control_function c_function;
        control_group c_group;
        control_reference c_reference;
        control_keyword c_keyword;

        /// <summary>
        /// Create all proper controls for an ElementItem using all information gathered.
        /// Usually, not all parameters need values.
        /// </summary>
        /// <param name="name"> The name of the element. (Mandatory) </param>
        /// <param name="isRequired"> If the element is optional or mandatory. (Optional) </param>
        /// <param name="help"> Some help text if exists. (Optional) </param>
        /// <param name="controlTypes"> The special list that shows which controls should be made. (Mandatory) </param>
        /// <param name="typeOptions"> Can have these options: Constant/Variable/Function. (Used in Constant/Variable/Function only) </param>
        /// <param name="unitOptions"> What options to show in the unit dropdown. (Used in Constant/Variable only) </param>
        /// <param name="x_unitOptions"> What options to show in the x_unit dropdown. (Used in Variable only) </param>
        /// <param name="funcOptions"> What options to show in the functions dropdown. (Used in Function only) </param>
        /// <param name="keyOptions"> What options to show in the keywords dropdown. (Used in Keywords only) </param>
        public General_Control(string name, bool isRequired, string help, List<custom_control_type> controlTypes,
            List<string> typeOptions, List<string> unitOptions, List<string> x_unitOptions,
            List<string> funcOptions, List<string> keyOptions)
        {
            createGeneralControl(name, isRequired, help, controlTypes, typeOptions, unitOptions, x_unitOptions, funcOptions, keyOptions);
        }
        /// <summary> Create a custom group control. </summary>
        /// <param name="name"> The name of the group. (Mandatory) </param>
        /// <param name="isRequired"> If the element is optional or mandatory. (Optional) </param>
        /// <param name="help"> Some help text if exists. (Optional) </param>
        public General_Control(string name, bool isRequired)
        {
            List<custom_control_type> controlTypes = new List<custom_control_type>(1);
            controlTypes.Add(custom_control_type.group);
            createGeneralControl(name, isRequired, "", controlTypes, null, null, null, null, null);
        }

        private void createGeneralControl(string name, bool isRequired, string help, List<custom_control_type> controlTypes,
            List<string> typeOptions, List<string> unitOptions, List<string> x_unitOptions,
            List<string> funcOptions, List<string> keyOptions)
        {
            //Handle bad situations
            if (controlTypes == null || controlTypes.Count < 1)
                return;

            //Initialize simple values
            this.name = name;
            this.isRequired = isRequired;
            this.help = help;

            //Create Controls
            foreach (custom_control_type type in controlTypes)
                switch (type)
                {
                    case custom_control_type.constant:
                        if (typeOptions != null && typeOptions.Contains("Constant") && unitOptions != null)
                            c_constant = new control_constant(name, typeOptions, unitOptions, isRequired, help, this);
                        break;
                    case custom_control_type.variable:
                        if (typeOptions != null && typeOptions.Contains("Variable") && unitOptions != null) // && x_unitOptions != null
                            c_variable = new control_variable(name, typeOptions, unitOptions, x_unitOptions, isRequired, help, this);
                        break;
                    case custom_control_type.function:
                        if (typeOptions != null && typeOptions.Contains("Function") && funcOptions != null)
                            c_function = new control_function(name, typeOptions, funcOptions, isRequired, help, this);
                        break;
                    case custom_control_type.group:
                        c_group = new control_group(name, isRequired, this);
                        break;
                    case custom_control_type.reference:
                        c_reference = new control_reference(name, isRequired, help, this);
                        break;
                    case custom_control_type.keyword:
                        c_keyword = new control_keyword(name, keyOptions, isRequired, help, this);
                        break;
                    default:
                        break;
                }


            //Find Default Control
            switch (controlTypes[0])
            {
                case custom_control_type.constant:
                    currentControl = c_constant;
                    break;
                case custom_control_type.variable:
                    currentControl = c_variable;
                    break;
                case custom_control_type.function:
                    currentControl = c_function;
                    break;
                case custom_control_type.group:
                    currentControl = c_group;
                    break;
                case custom_control_type.reference:
                    currentControl = c_reference;
                    break;
                case custom_control_type.keyword:
                    currentControl = c_keyword;
                    break;
                default:
                    break;
            }
        }

        public void ReplaceWith(custom_control_type cc_type)
        {
            System.Windows.Forms.Control viewer = currentControl.Parent;
            custom_control newMe = currentControl;
            custom_control me = currentControl;

            switch (cc_type)
            {
                case custom_control_type.constant:
                    if (currentControl == c_constant)
                        return;
                    newMe = c_constant;
                    break;
                case custom_control_type.variable:
                    if (currentControl == c_variable)
                        return;
                    newMe = c_variable;
                    break;
                case custom_control_type.function:
                    if (currentControl == c_function)
                        return;
                    newMe = c_function;
                    break;
                case custom_control_type.group:
                    if (currentControl == c_group)
                        return;
                    newMe = c_group;
                    break;
                case custom_control_type.reference:
                    if (currentControl == c_reference)
                        return;
                    newMe = c_reference;
                    break;
                case custom_control_type.keyword:
                    if (currentControl == c_keyword)
                        return;
                    newMe = c_keyword;
                    break;
                default:
                    break;
            }
            viewer.Controls.Add(newMe);
            viewer.Controls.SetChildIndex(newMe, viewer.Controls.GetChildIndex(me));
            viewer.Controls.Remove(me);
            currentControl = newMe;
        }




        public void Select(custom_control_type cc_type)
        {
            switch (cc_type)
            {
                case custom_control_type.constant:
                    currentControl = c_constant;
                    break;
                case custom_control_type.variable:
                    currentControl = c_variable;
                    break;
                case custom_control_type.function:
                    currentControl = c_function;
                    break;
                case custom_control_type.group:
                    currentControl = c_group;
                    break;
                case custom_control_type.reference:
                    currentControl = c_reference;
                    break;
                case custom_control_type.keyword:
                    currentControl = c_keyword;
                    break;
                default:
                    break;
            }
        }


        public void SetValue(string value) { currentControl.SetValue(value); }
        public void SetUnit(string unit) { currentControl.SetUnit(unit); }
        public void SetX_Unit(string x_unit) { currentControl.SetX_Unit(x_unit); }
    }
}
