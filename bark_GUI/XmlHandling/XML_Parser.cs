#region using
using System;
using System.Diagnostics;
using System.Xml;
using bark_GUI.CustomControls;
using bark_GUI.Structure.ElementType;
using bark_GUI.Structure.Items;
#endregion

namespace bark_GUI.XmlHandling
{
    static class XmlParser
    {
        private enum XmlValueType{constant, variable, function, keyword, reference}

        /// <summary> Recursive method that fills the element items with values (info).</summary>
        /// <param name="inXmlNode"> Which XML Node you wish to include in the elements. </param>
        public static void DrawInfo(XmlNode inXmlNode)
        {
            try
            {
            if (inXmlNode == null)
                return;

            //Find the element item from our built Structure to connect it
            var item = Structure.Structure.FindItem(inXmlNode);
            ElementItem elementItem;
            if (item == null)
                return;
            item.SetXmlNode(inXmlNode);

            //Check for NewName
            if (inXmlNode.Attributes != null && inXmlNode.Attributes.GetNamedItem("name") != null)
                item.NewName = inXmlNode.Attributes.GetNamedItem("name").Value;

            // Loop through Element Item XML Nodes
            if (inXmlNode.HasChildNodes)
                foreach (XmlNode xc in inXmlNode.ChildNodes)
                    switch (xc.Name)
                    {
                        case "constant":
                            elementItem = item as ElementItem;
                            if (item.Name == "temperature")      // TODO Multiple items handling.
                                Debug.Print("### Element with multiple items hit!\n{0}", item);
                            Debug.Assert(elementItem != null, "elementItem != null");
                            elementItem.SelectType(EType.Constant);
                            var eConstant = elementItem.SelectedType as ElementConstant;
                            Debug.Assert(eConstant != null, "eConstant != null");
                            eConstant.Value = inXmlNode.InnerText;
                            Debug.Assert(inXmlNode.Attributes != null, "inXmlNode.Attributes != null");
                            eConstant.Unit.Select(inXmlNode.Attributes.GetNamedItem("unit").Value.Trim());
                            elementItem.Control.Select(CustomControlType.Constant);
                            elementItem.Control.SetValue(eConstant.Value);
                            elementItem.Control.SetUnit(inXmlNode.Attributes.GetNamedItem("unit").Value.Trim());
                            break;
                        case "variable":
                            elementItem = item as ElementItem;
                            if (item.Name == "temperature")      // TODO Multiple items handling.
                                Debug.Print("### Element with multiple items hit!\n{0}", item);
                            Debug.Assert(elementItem != null, "elementItem != null");
                            elementItem.SelectType(EType.Variable);
                            var eVariable = elementItem.SelectedType as ElementVariable;
                            Debug.Assert(eVariable != null, "eVariable != null");
                            eVariable.Value = inXmlNode.InnerText;
                            Debug.Assert(inXmlNode.Attributes != null, "inXmlNode.Attributes != null");
                            eVariable.Unit.Select(inXmlNode.Attributes.GetNamedItem("unit").Value.Trim());
                            if (inXmlNode.Attributes.GetNamedItem("x_unit") != null)
                            {
                                eVariable.X_Unit.Select(inXmlNode.Attributes.GetNamedItem("x_unit").Value.Trim());
                                elementItem.Control.SetX_Unit(inXmlNode.Attributes.GetNamedItem("x_unit").Value.Trim());
                            }
                            elementItem.Control.Select(CustomControlType.Variable);
                            elementItem.Control.SetValue(inXmlNode.InnerText);
                            elementItem.Control.SetUnit(inXmlNode.Attributes.GetNamedItem("unit").Value.Trim());
                            
                            break;
                        case "function":
                            var fvalue = xc.FirstChild.Name;
                            break;
                        case "keyword":
                            var kvalue = inXmlNode.InnerText;
                            break;
                        default:
                            DrawInfo(xc);
                            break;
                    }
            //Handle references
            else if (inXmlNode.Attributes != null && inXmlNode.Attributes.GetNamedItem("reference") != null)
            {
                var rvalue = inXmlNode.Attributes.GetNamedItem("reference").Value;
            }

            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
