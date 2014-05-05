#region using
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using bark_GUI.CustomControls;
using bark_GUI.Structure.ElementTypes;
using bark_GUI.Structure.Items;
#endregion

namespace bark_GUI.XmlHandling
{
    static class XmlParser
    {
        #region Public Methods

        /// <summary> Recursive method that fills the element items with values (info).</summary>
        /// <param name="inXmlNode"> Which XML Node you wish to include in the elements. </param>
        public static void DrawInfo(XmlNode inXmlNode)
        {
            const string errorMsg = "XmlHandling - XmlParser - DrawInfo:\n - ";

            // Check.
            Debug.Assert(inXmlNode != null, errorMsg + "Given argument 'inXmlNode' was null.");

            try
            {
                // Find or Create Item.
                var item = Structure.Structure.FindDataItem(inXmlNode);

                if (item == null)
                    item = Structure.Structure.CreateItem(inXmlNode);
                // Check.
                Debug.Assert(item != null, errorMsg + "Item " + inXmlNode.LocalName + " not found in children.");

                // Check for NewName
                item.NewName = GetNewName(inXmlNode);

                // Loop through Element Item XML Nodes.
                if (inXmlNode.HasChildNodes)
                {
                    // Add to the reference list if can be referenced to.
                    Structure.Structure.AddReference(item);

                    // Iterate through the child xml nodes to draw their info using the appropriate method.
                    foreach (XmlNode xc in inXmlNode.ChildNodes)
                    {
                        switch (xc.Name)
                        {
                            case "constant":
                                DrawAConstant(inXmlNode, item);
                                break;
                            case "variable":
                                DrawAVariable(inXmlNode, item);
                                break;
                            case "function":
                                DrawAFunction(inXmlNode, item, xc);
                                break;
                            case "keyword":
                                DrawAKeyword(inXmlNode, item);
                                break;
                            default:
                                // Recursive call.
                                // child item not current item
                                DrawInfo(xc);
                                break;
                        }
                    }
                }
                else
                {
                    DrawAReference(inXmlNode, item);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static XElement ConvertToXml(Item item)
        {
            XElement element = null;
            List<object> content = new List<object>();

            // Check.
            if (item == null) return null;

            // Handle GroupItems.
            if (item.IsGroupItem)
            {
                var groupItem = (GroupItem)item;

                // Create XmlAttributes.
                if (!string.IsNullOrEmpty(groupItem.NewName))
                    content.Add(new XAttribute("name", groupItem.NewName));

                // Create children XmlElements.
                content.AddRange(groupItem.Children.Select(ConvertToXml));

                // Create XmlElement.
                element = new XElement(groupItem.Name, content.ToArray());

            } // Handle ElementItems.
            else if (item.IsElementItem)
            {
                var elementItem = (ElementItem)item;
                var elementValue = elementItem.SelectedType.Value ?? "";

                // Create XmlAttributes and get the element value.
                switch (elementItem.SelectedType.CurrentElementType)
                {
                    case EType.Constant:
                        content.Add(new XAttribute("unit", ((ElementConstant)elementItem.SelectedType).Unit.Selected));
                        content.Add(new XElement("constant", elementValue));
                        break;
                    case EType.Variable:
                        if (((ElementVariable)elementItem.SelectedType).XUnit != null)
                            content.Add(new XAttribute("x_unit", ((ElementVariable)elementItem.SelectedType).XUnit.Selected));
                        content.Add(new XAttribute("unit", ((ElementVariable)elementItem.SelectedType).Unit.Selected));
                        content.Add(new XElement("variable", elementValue));
                        break;
                    case EType.Function:
                        var functionItem = Structure.Structure.FindFunction(elementValue);

                        if (functionItem == null)
                        {
                            content.Add(new XElement("function", new XElement(elementValue)));
                            break;
                        }

                        var functionElement = ConvertToXml(functionItem);

                        // !!! TODO: Change/Remove this when functions are handled properly.
                        foreach (var childConstant in functionElement.Descendants("constant"))
                            childConstant.Value = "0";

                        content.Add(new XElement("function", functionElement));
                        break;
                    case EType.Keyword:
                        content.Add(new XElement("keyword", elementValue));
                        break;
                    case EType.Reference:
                        content.Add(new XAttribute("reference", elementValue));
                        break;
                }

                // Create XmlElement.
                element = new XElement(elementItem.Name, content.ToArray());
            }

            return element;
        }

        #endregion

        #region Private Draw Methods
        private static void DrawAConstant(XmlNode inXmlNode, Item item)
        {
            const string errorMsg = "XmlHandling - XmlParser - DrawAConstant:\n - ";

            // Check item.
            Debug.Assert(item.IsElementItem, errorMsg + "XmlItem is 'constant' but is not of type ElementItem.");

            var elementItem = item as ElementItem;

            // Check elementItem.
            Debug.Assert(elementItem != null, errorMsg + "Variable elementItem was null.");

            elementItem.SelectType(EType.Constant);

            var eConstant = elementItem.SelectedType as ElementConstant;

            // Check eConstant.
            Debug.Assert(eConstant != null, errorMsg + "Variable eConstant was null.");

            // Set the value.
            eConstant.Value = inXmlNode.InnerText;

            // Check inXmlNode.Attributes.
            Debug.Assert(inXmlNode.Attributes != null, errorMsg + "The 'Attributes' of 'inXmlNode' were null.");
            Debug.Assert(inXmlNode.Attributes["unit"] != null, errorMsg + "The unit of 'inXmlNode' was null.");

            // Set the selected unit.
            eConstant.Unit.Select(inXmlNode.Attributes["unit"].Value.Trim());

            // Set the item's Control.
            elementItem.Control.Select(CustomControlType.Constant);
            elementItem.Control.SetValue(eConstant.Value);
            elementItem.Control.SetUnit(inXmlNode.Attributes["unit"].Value.Trim());
        }

        private static void DrawAVariable(XmlNode inXmlNode, Item item)
        {
            const string errorMsg = "XmlHandling - XmlParser - DrawAVariable:\n - ";

            // Check item.
            Debug.Assert(item.IsElementItem, errorMsg + "XmlItem is 'constant' but is not of type ElementItem.");

            var elementItem = item as ElementItem;

            // Check elementItem.
            Debug.Assert(elementItem != null, errorMsg + "Variable elementItem was null.");

            elementItem.SelectType(EType.Variable);

            var eVariable = elementItem.SelectedType as ElementVariable;

            // Check eConstant.
            Debug.Assert(eVariable != null, errorMsg + "Variable eVariable was null.");

            // Set the value.
            eVariable.Value = inXmlNode.InnerText;

            // Check inXmlNode.Attributes.
            Debug.Assert(inXmlNode.Attributes != null, errorMsg + "The 'Attributes' of 'inXmlNode' were null.");

            // Set the selected unit.
            eVariable.Unit.Select(inXmlNode.Attributes["unit"].Value.Trim());

            // Set the selected x_unit.
            if (inXmlNode.Attributes["x_unit"] != null)
            {
                eVariable.XUnit.Select(inXmlNode.Attributes["x_unit"].Value.Trim());
                elementItem.Control.SetX_Unit(inXmlNode.Attributes["x_unit"].Value.Trim());
            }
            else
            {
                Debug.WriteLine("#CUSTOM WARNING\n" + errorMsg + "Item " + inXmlNode.LocalName +
                    " is of type 'variable' but has no x_unit value.");
            }

            // Set the item's Control.
            elementItem.Control.Select(CustomControlType.Variable);
            elementItem.Control.SetValue(inXmlNode.InnerText);
            elementItem.Control.SetUnit(inXmlNode.Attributes["unit"].Value.Trim());
            var xUnit = inXmlNode.Attributes["x_unit"];
            if (xUnit != null)
                elementItem.Control.SetX_Unit(xUnit.Value.Trim());
        }

        // TODO: Handle functions.
        private static void DrawAFunction(XmlNode inXmlNode, Item item, XmlNode xc)
        {
            // (THE ITEM PARAMETER MIGHT BE THE PARENT ITEM AND NOT THE CURRENT)
            // (Check RECURSIVE DRAWINFO CALL FOR HOW-TO-USE)

            const string errorMsg = "XmlHandling - XmlParser - DrawAFunction:\n - ";

            var fvalue = xc.FirstChild.Name;


            // Check item.
            Debug.Assert(item.IsElementItem, errorMsg + "XmlItem is 'function' but is not of type ElementItem.");

            var elementItem = item as ElementItem;

            // Check elementItem.
            Debug.Assert(elementItem != null, errorMsg + "Function elementItem was null.");

            elementItem.SelectType(EType.Function);

            var eFunction = elementItem.SelectedType as ElementFunction;

            // Check eConstant.
            Debug.Assert(eFunction != null, errorMsg + "Function eFunction was null.");

            // Set the value.
            eFunction.Value = fvalue;

            // Set the item's Control.
            elementItem.Control.Select(CustomControlType.Function);
            elementItem.Control.SetValue(fvalue);
        }

        private static void DrawAKeyword(XmlNode inXmlNode, Item item)
        {
            var kvalue = inXmlNode.InnerText;

            const string errorMsg = "XmlHandling - XmlParser - DrawAKeyword:\n - ";

            // Check item.
            Debug.Assert(item.IsElementItem, errorMsg + "XmlItem is 'keyword' but is not of type ElementItem.");

            var elementItem = item as ElementItem;

            // Check elementItem.
            Debug.Assert(elementItem != null, errorMsg + "Variable elementItem was null.");

            elementItem.SelectType(EType.Keyword);

            var eKeyword = elementItem.SelectedType as ElementKeyword;

            // Check eKeyword.
            Debug.Assert(eKeyword != null, errorMsg + "Variable eKeyword was null.");

            // Set the value.
            eKeyword.Value = kvalue;

            // Check inXmlNode.Attributes.
            Debug.Assert(inXmlNode.Attributes != null, errorMsg + "The 'Attributes' of 'inXmlNode' were null.");

            // Set the item's Control.
            elementItem.Control.Select(CustomControlType.Keyword);
            elementItem.Control.SetValue(kvalue);
        }

        private static void DrawAReference(XmlNode inXmlNode, Item item)
        {
            // Checks.
            if (inXmlNode.Attributes == null || inXmlNode.Attributes["reference"] == null)
                return;

            var rvalue = inXmlNode.Attributes["reference"].Value;

            const string errorMsg = "XmlHandling - XmlParser - DrawAReference:\n - ";

            // Check item.
            Debug.Assert(item.IsElementItem, errorMsg + "XmlItem is 'reference' but is not of type ElementItem.");

            var elementItem = item as ElementItem;

            // Check elementItem.
            Debug.Assert(elementItem != null, errorMsg + "Variable elementItem was null.");

            elementItem.SelectType(EType.Reference);

            var eReference = elementItem.SelectedType as ElementReference;

            // Check eReference.
            Debug.Assert(eReference != null, errorMsg + "Variable eReference was null.");

            // Set the value.
            eReference.Value = rvalue;

            // Set the item's Control.
            elementItem.Control.Select(CustomControlType.Reference);
            elementItem.Control.SetValue(rvalue);
        }
        #endregion

        #region Private Utility Methods
        private static string GetNewName(XmlNode inXmlNode)
        {
            if (inXmlNode.Attributes != null && inXmlNode.Attributes["name"] != null)
                return inXmlNode.Attributes["name"].Value;
            return null;
        }
        #endregion
    }
}


