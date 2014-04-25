using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public ElementType SelectedType { get; set; }

        /* PRIVATE VARIABLES */
        // The control that is shown on the windows form.
        private ComplexType _complexType;




        #region Constructors

        /// <summary> Creates a child ElementItem from the XSD file. </summary>
        /// <param name="xsdNode"> The XmlNode from the XSD Functions file that represents this item. </param>
        /// <param name="parent"> The parent of this Node. </param>
        /// <param name="defaultValue"> [Optional] Sets the default value of this element. </param>
        /// <param name="isFunction"> [Optional] Note that this is a Function. </param>
        public ElementItem(XmlNode xsdNode, GroupItem parent, bool isFunction = false)
            : base(xsdNode, parent, isFunction)
        {
            CreateElementItem(xsdNode, isFunction);
        }
        
        //Utility Function for Constructors (XSD)
        private void CreateElementItem(XmlNode xsdNode, bool isFunction)
        {
            // Check
            Debug.Assert(XsdParser.HasAttributes(xsdNode), "XSD Node '" + Name + "' had no attributes.");
            Debug.Assert(XsdParser.HasType(xsdNode), "XSD Node '" + Name + "' had no type.");

            var itemType = Structure.FindType(XsdParser.GetType(xsdNode));

            // Ingore simple types. //TODO: Handle SimpleTypes
            if (itemType.IsSimpleType()) return;

            _complexType = itemType as ComplexType;

            // Check
            Debug.Assert(_complexType != null, "itemType.IsSimpleType() Failed!\n\n" +
                                                "Alternative: Structure.FindType(xsdNode) Failed!");

            // Set the (default) selected type.
            SelectType(_complexType);

            // Create this item's control
            CreateGeneralControl(_complexType);
            
            //Include this in the Structure
            if (!isFunction)
                Structure.Add(this);
        }

        #endregion




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
            Control = new GeneralControl(XmlNode, Name, IsRequired, Help, list,
                typeOptions, unitOptions, xUnitOptions, functionOptions, keyOptions,
                defaultValues, defaultUnitValue, defaultXUnitValue);
        }
    }
}
