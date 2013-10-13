using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace bark_GUI
{
    static class XML_Parser
    {
        /// <summary> Recursive method that fills the element items with values (info).</summary>
        /// <param name="inXmlNode"> Which XML Node you wish to include in the elements. </param>
        public static void DrawInfo(XmlNode inXmlNode)
        {
            if (inXmlNode == null)
                return;

            //Find the element item from our built Structure to connect it
            Item item = Structure.FindItem(inXmlNode);
            ElementItem e_item;
            if (item == null)
                return;
            item.SetXMLNode(inXmlNode);

            //Check for NewName
            if (inXmlNode.Attributes != null && inXmlNode.Attributes.GetNamedItem("name") != null)
                item.NewName = inXmlNode.Attributes.GetNamedItem("name").Value;

            // Loop through Element Item XML Nodes
            if (inXmlNode.HasChildNodes)
                foreach (XmlNode xc in inXmlNode.ChildNodes)
                    switch (xc.Name)
                    {
                        case "constant":
                            e_item = item as ElementItem;
                            if (item.Name == "temperature")
                                item = item;
                            e_item.SelectType(e_type.constant);
                            ElementConstant e_constant = e_item.SelectedType as ElementConstant;
                            e_constant.Value = inXmlNode.InnerText;
                            e_constant.Unit.Select(inXmlNode.Attributes.GetNamedItem("unit").Value.Trim());
                            e_item.Control.Select(custom_control_type.constant);
                            e_item.Control.SetValue(e_constant.Value);
                            e_item.Control.SetUnit(inXmlNode.Attributes.GetNamedItem("unit").Value.Trim());
                            break;
                        case "variable":
                            e_item = item as ElementItem;
                            if (item.Name == "temperature")
                                item = item;
                            e_item.SelectType(e_type.variable);
                            ElementVariable e_variable = e_item.SelectedType as ElementVariable;
                            e_variable.Value = inXmlNode.InnerText;
                            e_variable.Unit.Select(inXmlNode.Attributes.GetNamedItem("unit").Value.Trim());
                            if (inXmlNode.Attributes.GetNamedItem("x_unit") != null)
                            {
                                e_variable.X_Unit.Select(inXmlNode.Attributes.GetNamedItem("x_unit").Value.Trim());
                                e_item.Control.SetX_Unit(inXmlNode.Attributes.GetNamedItem("x_unit").Value.Trim());
                            }
                            e_item.Control.Select(custom_control_type.variable);
                            e_item.Control.SetValue(inXmlNode.InnerText);
                            e_item.Control.SetUnit(inXmlNode.Attributes.GetNamedItem("unit").Value.Trim());
                            
                            break;
                        case "function":
                            string fvalue = xc.FirstChild.Name;
                            break;
                        case "keyword":
                            string kvalue = inXmlNode.InnerText;
                            break;
                        default:
                            DrawInfo(xc);
                            break;
                    }
            //Handle references
            else if (inXmlNode.Attributes != null && inXmlNode.Attributes.GetNamedItem("reference") != null)
            {
                string rvalue = inXmlNode.Attributes.GetNamedItem("reference").Value;
            }
        }
    }
}
