using System;
using System.Collections.Generic;
using System.Xml;
using bark_GUI.CustomControls;
using bark_GUI.Structure.ElementType;
using bark_GUI.Structure.ItemTypes;
using bark_GUI.XmlHandling;

namespace bark_GUI.Structure.Items
{
    public class ElementItem : Item
    {
        /* PUBLIC PROPERTIES */
        public GeneralControl Control { get; private set; }
        public ElementType.ElementType SelectedType { get; set; }

        /* PRIVATE VARIABLES */
        // The control that is shown on the windows form.
        private ComplexType _complexType;




        #region Constructors

        /// <summary> Creates the Root GroupItem of the XSD file that is not a function. </summary>
        /// <param name="xsdNode"> The root XmlNode of the XSD file. </param>
        public ElementItem(XmlNode xsdNode)
            : base(xsdNode)
        {
            CreateElementItem(xsdNode, isFunction);
        }
        /// <summary> Creates a chile GroupItem from the XSD file that is not a function. </summary>
        /// <param name="xsdNode"> The XmlNode of the XSD file. </param>
        /// <param name="parent"> The parent of this Node. </param>
        public ElementItem(XmlNode xsdNode, GroupItem parent)
            : base(xsdNode, parent)
        {
            CreateElementItem(xsdNode, isFunction);
        }
        //Used in functions
        /// <summary> Creates a Root GroupItem from the XSD Functions file. (used for functions) </summary>
        /// <param name="xsdNode"> A root XmlNode from the XSD Functions file. (used for functions) </param>
        /// <param name="isFunction"> Note that this is a Function. </param>
        public ElementItem(XmlNode xsdNode, bool isFunction)
            : base(xsdNode, isFunction)
        {
            CreateElementItem(xsdNode, isFunction);
        }
        /// <summary> Creates a child GroupItem from the XSD Functions file. (used for functions) </summary>
        /// <param name="xsdNode"> A XmlNode from the XSD Functions file. (used for functions) </param>
        /// <param name="parent"> The parent of this Node. </param>
        /// <param name="isFunction"> Note that this is a Function. </param>
        public ElementItem(XmlNode xsdNode, GroupItem parent, bool isFunction)
            : base(xsdNode, parent, isFunction)
        {
            CreateElementItem(xsdNode, isFunction);
        }
        
        //Utility Function for Constructors
        private void CreateElementItem(XmlNode xsdNode, bool isFunction)
        {
            List<CustomControlType> list = new List<CustomControlType>();
            List<string> typeOptions = null;        //Constant/Variable/Function
            List<string> unitOptions = null;        //Constant/Variable
            List<string> xUnitOptions = null;       //Variable
            List<string> functionOptions = null;    //Function
            List<string> keyOptions = null;         //Keyword
            List<string> refOptions = null;         //Reference

            if (XsdParser.HasAttributes(xsdNode))
            {
                //Find what kind of simpleType or complexType this element item is
                if (XsdParser.HasType(xsdNode))
                {
                    ItemType itemType = Structure.FindType(XsdParser.GetType(xsdNode));
                    if (itemType.IsSimpleType())
                        return;
                    _complexType = itemType as ComplexType;

                    //Gather information for the controls creation
                    if (_complexType != null)
                    {
                        unitOptions = _complexType.GetUnitOptions();
                        xUnitOptions = _complexType.GetX_UnitOptions();
                        functionOptions = _complexType.GetFunctionNames();
                        typeOptions = _complexType.GetTypeOptions();

                        // Handle types selection on this element
                        if (typeOptions != null && typeOptions.Count > 0)
                        {
                            SelectedType = _complexType.Constant;
                            if (_complexType.Constant != null)
                                list.Add(CustomControlType.Constant);
                            if (_complexType.Variable != null)
                                list.Add(CustomControlType.Variable);
                            if (_complexType.Function != null)
                                list.Add(CustomControlType.Function);
                        }
                        else if (_complexType.Keyword != null)
                        {
                            SelectedType = _complexType.Keyword;
                            list.Add(CustomControlType.Keyword);
                            keyOptions = _complexType.Keyword.SimpleType.GetOptions();
                        }
                        else if (_complexType.Reference != null)
                        {
                            SelectedType = _complexType.Reference;
                            list.Add(CustomControlType.Reference);
                            refOptions = _complexType.Reference.SimpleType.GetOptions();
                        }
                        else { throw new Exception(""); }
                    }
                }
                else { }

                //Create the Controls for this ElementItem with the gathered information
                Control = new GeneralControl(XmlNode, Name, IsRequired, Help, list,
                    typeOptions, unitOptions, xUnitOptions, functionOptions, keyOptions);
            }
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
    }
}
