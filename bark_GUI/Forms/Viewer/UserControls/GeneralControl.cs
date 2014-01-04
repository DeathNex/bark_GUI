using System.Collections.Generic;

namespace bark_GUI.CustomControls
{
    public enum CustomControlType { Constant, Variable, Function, Group, Reference, Keyword }

    public class GeneralControl
    {
        //Public Variables
        public string Name;
        public bool IsRequired;
        public string Help;
        public CustomControl CurrentControl;


        //Private Variables
        private List<ICustomControl> _customControls;

        ControlConstant _controlConstant;
        ControlVariable _controlVariable;
        ControlFunction _controlFunction;
        ControlGroup _controlGroup;
        ControlReference _controlReference;
        ControlKeyword _controlKeyword;

        #region Constructors

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
        /// <param name="xUnitOptions"> What options to show in the x_unit dropdown. (Used in Variable only) </param>
        /// <param name="funcOptions"> What options to show in the functions dropdown. (Used in Function only) </param>
        /// <param name="keyOptions"> What options to show in the keywords dropdown. (Used in Keywords only) </param>
        public GeneralControl(string name, bool isRequired, string help, List<CustomControlType> controlTypes,
            List<string> typeOptions, List<string> unitOptions, List<string> xUnitOptions,
            List<string> funcOptions, List<string> keyOptions)
        {
            CreateGeneralControl(name, isRequired, help, controlTypes, typeOptions, unitOptions, xUnitOptions, funcOptions, keyOptions);
        }
        /// <summary> Create a custom group control. </summary>
        /// <param name="name"> The name of the group. (Mandatory) </param>
        /// <param name="isRequired"> If the element is optional or mandatory. (Optional) </param>
        /// <param name="help"> Some help text if exists. (Optional) </param>
        public GeneralControl(string name, bool isRequired, string help = null)
        {
            List<CustomControlType> controlTypes = new List<CustomControlType>(1) { CustomControlType.Group };
            CreateGeneralControl(name, isRequired, help, controlTypes, null, null, null, null, null);
        }

        private void CreateGeneralControl(string name, bool isRequired, string help, List<CustomControlType> controlTypes,
            List<string> typeOptions, List<string> unitOptions, List<string> xUnitOptions,
            List<string> funcOptions, List<string> keyOptions)
        {
            //Handle bad situations
            if (controlTypes == null || controlTypes.Count < 1)
                return;

            //Initialize simple values
            Name = name;
            IsRequired = isRequired;
            Help = help ?? "";

            //Create Controls
            foreach (var type in controlTypes)
                switch (type)
                {
                    case CustomControlType.Constant:
                        if (typeOptions != null && typeOptions.Contains("Constant") && unitOptions != null)
                            _controlConstant = new ControlConstant(name, typeOptions, unitOptions, isRequired, help, this);
                        break;
                    case CustomControlType.Variable:
                        if (typeOptions != null && typeOptions.Contains("Variable") && unitOptions != null) // && x_unitOptions != null
                            _controlVariable = new ControlVariable(name, typeOptions, unitOptions, xUnitOptions, isRequired, help, this);
                        break;
                    case CustomControlType.Function:
                        if (typeOptions != null && typeOptions.Contains("Function") && funcOptions != null)
                            _controlFunction = new ControlFunction(name, typeOptions, funcOptions, isRequired, help, this);
                        break;
                    case CustomControlType.Group:
                        _controlGroup = new ControlGroup(name, isRequired, this);
                        break;
                    case CustomControlType.Reference:
                        _controlReference = new ControlReference(name, isRequired, help, this);
                        break;
                    case CustomControlType.Keyword:
                        _controlKeyword = new ControlKeyword(name, keyOptions, isRequired, help, this);
                        break;
                }


            //Find Default Control
            switch (controlTypes[0])
            {
                case CustomControlType.Constant:
                    CurrentControl = _controlConstant;
                    break;
                case CustomControlType.Variable:
                    CurrentControl = _controlVariable;
                    break;
                case CustomControlType.Function:
                    CurrentControl = _controlFunction;
                    break;
                case CustomControlType.Group:
                    CurrentControl = _controlGroup;
                    break;
                case CustomControlType.Reference:
                    CurrentControl = _controlReference;
                    break;
                case CustomControlType.Keyword:
                    CurrentControl = _controlKeyword;
                    break;
            }
        }

        #endregion

        public void ReplaceWith(CustomControlType customControlType)
        {
            System.Windows.Forms.Control viewer = CurrentControl.Parent;
            CustomControl newMe = CurrentControl;
            CustomControl me = CurrentControl;

            switch (customControlType)
            {
                case CustomControlType.Constant:
                    if (CurrentControl == _controlConstant)
                        return;
                    newMe = _controlConstant;
                    break;
                case CustomControlType.Variable:
                    if (CurrentControl == _controlVariable)
                        return;
                    newMe = _controlVariable;
                    break;
                case CustomControlType.Function:
                    if (CurrentControl == _controlFunction)
                        return;
                    newMe = _controlFunction;
                    break;
                case CustomControlType.Group:
                    if (CurrentControl == _controlGroup)
                        return;
                    newMe = _controlGroup;
                    break;
                case CustomControlType.Reference:
                    if (CurrentControl == _controlReference)
                        return;
                    newMe = _controlReference;
                    break;
                case CustomControlType.Keyword:
                    if (CurrentControl == _controlKeyword)
                        return;
                    newMe = _controlKeyword;
                    break;
            }
            viewer.Controls.Add(newMe);
            viewer.Controls.SetChildIndex(newMe, viewer.Controls.GetChildIndex(me));
            viewer.Controls.Remove(me);
            CurrentControl = newMe;
        }
        
        public void Select(CustomControlType customControlType)
        {
            switch (customControlType)
            {
                case CustomControlType.Constant:
                    CurrentControl = _controlConstant;
                    break;
                case CustomControlType.Variable:
                    CurrentControl = _controlVariable;
                    break;
                case CustomControlType.Function:
                    CurrentControl = _controlFunction;
                    break;
                case CustomControlType.Group:
                    CurrentControl = _controlGroup;
                    break;
                case CustomControlType.Reference:
                    CurrentControl = _controlReference;
                    break;
                case CustomControlType.Keyword:
                    CurrentControl = _controlKeyword;
                    break;
            }
        }

        public void SetValue(string value) { CurrentControl.SetValue(value); }

        public void SetUnit(string unit) { CurrentControl.SetUnit(unit); }

        public void SetX_Unit(string xUnit) { CurrentControl.SetX_Unit(xUnit); }
    }
}
