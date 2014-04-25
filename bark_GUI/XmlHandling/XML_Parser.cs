#region using
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
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
            Item item;
            const string errorMsg = "XmlHandling - XmlParser - DrawInfo:\n - ";

            Debug.Assert(inXmlNode != null, errorMsg + "Given argument 'inXmlNode' was null.");

            try
            {
                // Find the element item from our built Structure to connect it.
                item = Structure.Structure.FindItem(inXmlNode);

                // Check.
                Debug.Assert(item != null, errorMsg + "Item " + inXmlNode.LocalName + " not found in structure.");

                // Handle Multiples.
                {
                    var groupItem = item as GroupItem;
                    if (groupItem != null && groupItem.IsMultiple)
                        item = groupItem.DuplicateEmptyMultiple();
                }

                // Set Item's XmlNode.
                item.SetXmlNode(inXmlNode);

                // Check for NewName
                item.NewName = GetNewName(inXmlNode);

                // Loop through Element Item XML Nodes.
                if (inXmlNode.HasChildNodes)
                {
                    // Add to the reference list if can be referenced to.
                    Structure.Structure.AddReference(item);

                    // Iterate through the child nodes to draw their info using the appropriate method.
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
        #endregion

        #region Private Draw Methods
        private static void DrawAConstant(XmlNode inXmlNode, Item item)
        {
            const string errorMsg = "XmlHandling - XmlParser - DrawAConstant:\n - ";

            // Check item.
            Debug.Assert(item.IsElementItem, errorMsg + "XmlItem is 'constant' but is not of type ElementItem.");

            var elementItem = item as ElementItem;

            if (item.Name == "temperature")
                Debug.Print("### Element with multiple items hit!\n{0}", item);

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

            if (item.Name == "temperature")
                Debug.Print("### Element with multiple items hit!\n{0}", item);

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
        }

        // TODO: Handle functions.
        private static void DrawAFunction(XmlNode inXmlNode, Item item, XmlNode xc)
        {
            var fvalue = xc.FirstChild.Name;

            //const string errorMsg = "XmlHandling - XmlParser - DrawAVariable:\n - ";

            //// Check item.
            //Debug.Assert(item.IsElementItem, errorMsg + "XmlItem is 'constant' but is not of type ElementItem.");

            //var elementItem = item as ElementItem;

            //if (item.Name == "temperature")      // TODO Multiple items handling.
            //    Debug.Print("### Element with multiple items hit!\n{0}", item);

            //// Check elementItem.
            //Debug.Assert(elementItem != null, errorMsg + "Variable elementItem was null.");

            //elementItem.SelectType(EType.Variable);

            //var eVariable = elementItem.SelectedType as ElementVariable;

            //// Check eConstant.
            //Debug.Assert(eVariable != null, errorMsg + "Variable eVariable was null.");

            //// Set the value.
            //eVariable.Value = inXmlNode.InnerText;

            //// Check inXmlNode.Attributes.
            //Debug.Assert(inXmlNode.Attributes != null, errorMsg + "The 'Attributes' of 'inXmlNode' were null.");

            //// Set the selected unit.
            //eVariable.Unit.Select(inXmlNode.Attributes["unit"].Value.Trim());

            //// Set the item's Control.
            //elementItem.Control.Select(CustomControlType.Variable);
            //elementItem.Control.SetValue(inXmlNode.InnerText);
            //elementItem.Control.SetUnit(inXmlNode.Attributes["unit"].Value.Trim());
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


