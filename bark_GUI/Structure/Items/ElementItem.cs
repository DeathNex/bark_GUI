using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using bark_GUI.CustomControls;
using bark_GUI.Structure.ElementTypes;
using bark_GUI.Structure.ItemTypes;
using bark_GUI.XmlHandling;

namespace bark_GUI.Structure.Items
{

    public class ElementItem : Item
    {
        /* PUBLIC PROPERTIES */
        // Current type (constant/variable/...) & value of this item.
        public ElementType SelectedType { get; set; }

        /* PRIVATE VARIABLES */
        // All the possible values & types (constant/variable/...) that this element item can have.
        private readonly ComplexType _complexType;



        // Constructor
        /// <summary> Creates a child ElementItem from the XSD file. </summary>
        /// <param name="xsdNode"> The XmlNode from the XSD Functions file that represents this item. </param>
        /// <param name="parent"> The parent of this item. </param>
        /// <param name="isFunction"> [Optional] Note if this is a Function. </param>
        public ElementItem(XmlNode xsdNode, GroupItem parent, bool isFunction = false)
            : base(xsdNode, parent, isFunction)
        {
            // Check
            Debug.Assert(XsdParser.HasAttributes(xsdNode), "XSD Node '" + Name + "' had no attributes.");
            Debug.Assert(XsdParser.HasType(xsdNode), "XSD Node '" + Name + "' had no type.");

            _complexType = Structure.CreateComplexType(XsdParser.GetType(xsdNode));

            // Check
            Debug.Assert(_complexType != null, "Alternative: Structure.FindComplexType(xsdNode) Failed!");

            // Set the (default) selected type.
            SelectType(_complexType);

            // Create this item's control
            CreateGeneralControl(_complexType);
        }




        /* PUBLIC METHODS */

        public void SelectType(EType type)
        {
            switch (type)
            {
                case EType.Constant:
                    SelectedType = _complexType.Constant;
                    break;
                case EType.Variable:
                    SelectedType = _complexType.Variable;
                    break;
                case EType.Function:
                    SelectedType = _complexType.Function;
                    break;
                case EType.Reference:
                    SelectedType = _complexType.Reference;
                    break;
                case EType.Keyword:
                    SelectedType = _complexType.Keyword;
                    break;
            }
        }

        public void SelectType(string type)
        {
            switch (type)
            {
                case "constant":
                    SelectedType = _complexType.Constant;
                    break;
                case "variable":
                    SelectedType = _complexType.Variable;
                    break;
                case "function":
                    SelectedType = _complexType.Function;
                    break;
                case "keyword":
                    SelectedType = _complexType.Keyword;
                    break;
                case "reference":
                    SelectedType = _complexType.Reference;
                    break;
            }
        }

        public bool ValidateAndSave(string value)
        {
            // Validate
            if (SelectedType.CurrentElementType != EType.Function)
            {
                var isValid = SelectedType.ValueIsValid(value);

                if (!isValid) return false;
            }

            // Save value
            SelectedType.Value = value;

            return true;
        }

        public void UnitChange(string value)
        {
            // Save unit value
            if (SelectedType.CurrentElementType == EType.Constant)
            {
                ((ElementConstant)SelectedType).Unit.Select(value);
            }
            if (SelectedType.CurrentElementType == EType.Variable)
            {
                ((ElementVariable)SelectedType).Unit.Select(value);
            }
        }

        public void XUnitChange(string value)
        {
            if (SelectedType.CurrentElementType == EType.Variable)
            {
                ((ElementVariable)SelectedType).XUnit.Select(value);
            }
        }

        public void SaveVariable(string value)
        {
            if(SelectedType.CurrentElementType != EType.Variable) return;

            // Save value
            SelectedType.Value = value;
        }

        /* PRIVATE METHODS */

        private void SelectType(ComplexType complexType)
        {
            if (complexType.Constant != null)
                SelectedType = complexType.Constant;
            else if (complexType.Variable != null)
                SelectedType = complexType.Variable;
            else if (complexType.Function != null)
                SelectedType = complexType.Function;
            else if (complexType.Keyword != null)
                SelectedType = complexType.Keyword;
            else if (complexType.Reference != null)
                SelectedType = complexType.Reference;
            else
                throw new Exception("ComplexType of item '" + Name + "' does not contain types.");
        }

        private void CreateGeneralControl(ComplexType complexType)
        {
            var list = new List<CustomControlType>();

            // Possible Options
            List<string> typeOptions = complexType.GetTypeOptions();//Constant/Variable/Function
            List<string> unitOptions = null;                        //Constant/Variable
            List<string> xUnitOptions = null;                       //Variable
            List<string> functionOptions = null;                    //Function
            List<string> keyOptions = null;                         //Keyword
            // Reference Options cannot be gathered during XSD Load.//Reference

            // Default values
            var defaultValues = new Dictionary<CustomControlType, string>();
            string defaultUnitValue = null;
            string defaultXUnitValue = null;



            //Gather information for the controls creation
            unitOptions = complexType.GetUnitOptions();
            xUnitOptions = complexType.GetX_UnitOptions();
            functionOptions = complexType.GetFunctionNames();
            keyOptions = complexType.GetKeywordOptions();

            // Handle types selection on this element
            if (complexType.Constant != null)
            {
                list.Add(CustomControlType.Constant);
                defaultValues[CustomControlType.Constant] = complexType.Constant.DefaultValue;
                defaultUnitValue = complexType.Constant.DefaultUnit;
            }
            if (complexType.Variable != null)
            {
                list.Add(CustomControlType.Variable);
                defaultValues[CustomControlType.Variable] = complexType.Variable.DefaultValue;
                defaultUnitValue = complexType.Variable.DefaultUnit;
                defaultXUnitValue = complexType.Variable.DefaultXUnit;
            }
            if (complexType.Function != null)
            {
                list.Add(CustomControlType.Function);
            }
            if (complexType.Keyword != null)
            {
                list.Add(CustomControlType.Keyword);
                defaultValues[CustomControlType.Keyword] = complexType.Keyword.DefaultValue;
            }
            if (complexType.Reference != null)
            {
                list.Add(CustomControlType.Reference);
            }


            // Create the Controls for this ElementItem with the gathered information.
            Control = new GeneralControl(Name, IsRequired, Help, list,
                typeOptions, unitOptions, xUnitOptions, functionOptions, keyOptions,
                defaultValues, defaultUnitValue, defaultXUnitValue,
                ValidateAndSave, SaveVariable, SelectType, UnitChange, XUnitChange);
        }
    }
}
