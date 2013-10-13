using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace bark_GUI
{
    public class ElementItem : Item
    {
        /* PUBLIC PROPERTIES */
        public General_Control Control { get { return control; } }
        public ElementType SelectedType { get { return selectedType; } set { selectedType = value; } }

        /* PRIVATE VARIABLES */
        // The control that is shown on the windows form.
        private General_Control control;
        private ComplexType complexType;
        private ElementType selectedType;




        #region Constructors
        /// <summary> Creates the Root GroupItem of the XSD file that is not a function. </summary>
        /// <param name="xsdNode"> The root XmlNode of the XSD file. </param>
        public ElementItem(XmlNode xsdNode)
            : base(xsdNode)
        {
            createElementItem(xsdNode, isFunction);
        }
        /// <summary> Creates a chile GroupItem from the XSD file that is not a function. </summary>
        /// <param name="xsdNode"> The XmlNode of the XSD file. </param>
        /// <param name="parent"> The parent of this Node. </param>
        public ElementItem(XmlNode xsdNode, GroupItem parent)
            : base(xsdNode, parent)
        {
            createElementItem(xsdNode, isFunction);
        }
        //Used in functions
        /// <summary> Creates a Root GroupItem from the XSD Functions file. (used for functions) </summary>
        /// <param name="xsdNode"> A root XmlNode from the XSD Functions file. (used for functions) </param>
        /// <param name="isFunction"> Note that this is a Function. </param>
        public ElementItem(XmlNode xsdNode, bool isFunction)
            : base(xsdNode, isFunction)
        {
            createElementItem(xsdNode, isFunction);
        }
        /// <summary> Creates a child GroupItem from the XSD Functions file. (used for functions) </summary>
        /// <param name="xsdNode"> A XmlNode from the XSD Functions file. (used for functions) </param>
        /// <param name="parent"> The parent of this Node. </param>
        /// <param name="isFunction"> Note that this is a Function. </param>
        public ElementItem(XmlNode xsdNode, GroupItem parent, bool isFunction)
            : base(xsdNode, parent, isFunction)
        {
            createElementItem(xsdNode, isFunction);
        }
        #endregion
        //Utility Function for Constructors
        private void createElementItem(XmlNode xsdNode, bool isFunction)
        {
            List<custom_control_type> list = new List<custom_control_type>();
            List<string> typeOptions = null;        //Constant/Variable/Function
            List<string> unitOptions = null;        //Constant/Variable
            List<string> x_UnitOptions = null;      //Variable
            List<string> functionOptions = null;    //Function
            List<string> keyOptions = null;         //Keyword
            List<string> refOptions = null;         //Reference

            if (XSD_Parser.HasAttributes(xsdNode))
            {
                //Find what kind of simpleType or complexType this element item is
                if (XSD_Parser.HasType(xsdNode))
                {
                    ItemType itemType = Structure.FindType(XSD_Parser.GetType(xsdNode));
                    if (itemType.isSimpleType())
                        return;
                    complexType = itemType as ComplexType;

                    //Gather information for the controls creation
                    unitOptions = complexType.GetUnitOptions();
                    x_UnitOptions = complexType.GetX_UnitOptions();
                    functionOptions = complexType.GetFunctionNames();
                    typeOptions = complexType.GetTypeOptions();
                    if (typeOptions != null && typeOptions.Count > 0)
                    {
                        selectedType = complexType.Constant;
                        if (complexType.Constant != null)
                            list.Add(custom_control_type.constant);
                        if (complexType.Variable != null)
                            list.Add(custom_control_type.variable);
                        if (complexType.Function != null)
                            list.Add(custom_control_type.function);
                    }
                    else if (complexType.Keyword != null)
                    {
                        selectedType = complexType.Keyword;
                        list.Add(custom_control_type.keyword);
                        keyOptions = complexType.Keyword.SimpleType.GetOptions();
                    }
                    else if (complexType.Reference != null)
                    {
                        selectedType = complexType.Reference;
                        list.Add(custom_control_type.reference);
                        refOptions = complexType.Reference.SimpleType.GetOptions();
                    }
                    else { }
                }//Handle references
                else { }

                //Create the Controls for this ElementItem with the gathered information
                control = new General_Control(Name, IsRequired, Help, list, typeOptions, unitOptions, x_UnitOptions, functionOptions, keyOptions);
            }
            //Include this in the Structure
            if (!isFunction)
                Structure.Add(this);
        }




        /* PUBLIC METHODS */

        public void SelectType(e_type type)
        {
            switch (type)
            {
                case e_type.constant:
                    selectedType = complexType.Constant;
                    break;
                case e_type.variable:
                    selectedType = complexType.Variable;
                    break;
                case e_type.function:
                    selectedType = complexType.Function;
                    break;
                case e_type.reference:
                    selectedType = complexType.Reference;
                    break;
                case e_type.keyword:
                    selectedType = complexType.Keyword;
                    break;
                default:
                    break;
            }
        }
    }
}
