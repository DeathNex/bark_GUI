using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using bark_GUI.Structure.Items;

namespace bark_GUI.XmlHandling
{
    public static class XsdParser
    {
        #region Get
        /// <summary>
        /// Gets the name attribute of the given xml element if exists. (XSD file)
        /// </summary>
        /// <param name="xsdNode">Xml element that might contain a name attribute.</param>
        /// <returns>The name if exists or null.</returns>
        public static string GetName(XmlNode xsdNode)
        {
            if (xsdNode.Attributes != null && xsdNode.Attributes["name"] != null)
                return xsdNode.Attributes["name"].Value.Trim();
            return null;
        }

        /// <summary>
        /// Gets the type of the given xml element if exists. (XSD file)
        /// </summary>
        /// <param name="xsdNode"> Xml element that might contain a type attribute. </param>
        /// <returns>The type if exists or null.</returns>
        public static string GetType(XmlNode xsdNode)
        {
            if (xsdNode != null && xsdNode.Attributes != null && xsdNode.Attributes["type"] != null)
                return xsdNode.Attributes["type"].Value.Trim();
            return null;
        }

        ///// <summary>
        ///// Gets the default attribute of the given xml element if exists. (XSD file)
        ///// </summary>
        ///// <param name="xsdNode">Xml element that might contain a default attribute.</param>
        ///// <returns>The default value if exists or null.</returns>
        //public static string GetDefault(XmlNode xsdNode)
        //{
        //    if (xsdNode.Attributes != null && xsdNode.Attributes["default"] != null)
        //        return xsdNode.Attributes["default"].Value.Trim();
        //    return null;
        //}

        /// <summary>
        /// Gets help from tag xs:documentation inside the tag xs:annotation that is a child of the given xml element.
        /// </summary>
        /// <param name="xsdNode">Xml element that might contain help text.</param>
        /// <returns>The help text if exists or null.</returns>
        public static string GetHelp(XmlNode xsdNode)
        {
            var elementNotes = xsdNode["xs:annotation"];
            if (elementNotes == null)
                return null;
            var xmlElement = elementNotes["xs:documentation"];
            return xmlElement != null ? xmlElement.InnerText : null;
        }

        public static List<string> GetUnitOptions(XmlNode xsdNode)
        {
            var options = new List<string>();
            var errorMsg = "Error!\n\n";
            errorMsg += "Folder: XmlHandling - Class: XSD_Parser - Method: DrawUnits()\n";
            errorMsg += "XSD - Units - " + xsdNode.Name + ": " + "\n\t";

            var xmlNodeList = xsdNode["xs:restriction"];

            Debug.Assert(xmlNodeList != null, errorMsg + "Expected 'xs:restriction' child not found.");

            foreach (XmlElement option in xmlNodeList)
            {
                Debug.Assert(option.Name == "xs:enumeration", errorMsg + "Expected 'xs:enumeration' child not found.");
                Debug.Assert(option.HasAttributes && option.Attributes["value"] != null, errorMsg + " - xs:enumeration: " +
                    "\n\tExpected 'value' attribute not found.");
                options.Add(option.Attributes["value"].Value.Trim());
            }

            return options;
        }
        #endregion

        #region Is
        public static bool IsElementItem(XmlNode xsdNode)
        {
            return HasType(xsdNode);
        }

        public static bool IsGroupItem(XmlNode xsdNode)
        {
            return !IsElementItem(xsdNode);
        }

        public static bool IsElementReferenceItem(XmlNode xsdNode)
        {
            return GetType(xsdNode) == "reference";
        }

        public static bool IsRequired(XmlNode xsdNode)
        {
            int number;

            if (xsdNode.Attributes != null && xsdNode.Attributes["minOccurs"] != null &&
                int.TryParse(xsdNode.Attributes["minOccurs"].Value.Trim(), out number) && number == 0)
                return false;

            return true;
        }

        public static bool IsMupltiple(XmlNode xsdNode)
        {
            int number;

            if (xsdNode.Attributes != null && xsdNode.Attributes["maxOccurs"] != null)
                if ((int.TryParse(xsdNode.Attributes["maxOccurs"].Value.Trim(), out number) && number > 1) ||
                    (xsdNode.Attributes["maxOccurs"].Value.Trim() == "unbounded"))
                    return true;

            return false;
        }
        #endregion

        #region Has
        public static bool HasType(XmlNode xsdNode)
        {
            return (GetType(xsdNode) != null);
        }

        public static bool HasAttributes(XmlNode xsdNode)
        {
            return xsdNode.NodeType == XmlNodeType.Element && ((XmlElement)xsdNode).HasAttributes;
        }

        public static bool HasChildren(XmlNode xsdNode)
        {
            return xsdNode.HasChildNodes;
        }

        public static bool HasRightClickActions(XmlNode xsdNode)
        {
            // Must be multiple.
            if (!IsMupltiple(xsdNode))
                return false;

            // Checks.
            if (xsdNode == null || xsdNode.Attributes == null ||
                xsdNode.Attributes["maxOccurs"] == null || xsdNode.Attributes["minOccurs"] == null)
                return false;

            // Must be able to be created freely, infinite times and deleted freely, down to 0.
            if (xsdNode.Attributes["maxOccurs"].Value.Trim() == "unbounded" &&
                (xsdNode.Attributes["minOccurs"].Value.Trim() == "1" || xsdNode.Attributes["minOccurs"].Value.Trim() == "0"))
                return true;

            return false;
        }
        #endregion

        #region Create
        /// <summary>
        /// Creates all the children of the given item and returns them as a list of items.
        ///  Includes inner children too (recursive through GroupItem constructors).
        /// </summary>
        /// <param name="inXsdNode"> The given item's XmlNode to create it's children. </param>
        /// <param name="parent"> The parent of the given item's XmlNode. </param>
        /// <param name="isFunction"> Keep track of function items. </param>
        /// <returns></returns>
        public static List<Item> CreateChildren(XmlNode inXsdNode, GroupItem parent, bool isFunction)
        {
            List<Item> list = null;

            // Check.
            if (!inXsdNode.HasChildNodes) return null;

            list = new List<Item>(inXsdNode.ChildNodes.Count);

            foreach (XmlNode x in inXsdNode.ChildNodes)
            {
                switch (x.LocalName)
                {
                    case "element":
                        if (IsElementItem(x))
                            list.Add(new ElementItem(x, parent, isFunction));
                        else
                            list.Add(new GroupItem(x, parent, isFunction));
                        break;
                    case "complexType":     // CHECK: Hardcoded XSD sequence.
                        foreach (XmlNode xc in x.ChildNodes)
                        {
                            if (xc.LocalName == "element")
                                if (IsElementItem(xc))
                                    list.Add(new ElementItem(xc, parent, isFunction));
                                else
                                    list.Add(new GroupItem(xc, parent, isFunction));
                            else if (xc.LocalName == "sequence" || xc.LocalName == "all" || xc.LocalName == "choice")
                                foreach (XmlNode xcc in xc.ChildNodes)
                                {
                                    if (xcc.LocalName == "element")
                                        if (IsElementItem(xcc))
                                            list.Add(new ElementItem(xcc, parent, isFunction));
                                        else
                                            list.Add(new GroupItem(xcc, parent, isFunction));
                                }
                        }
                        break;
                    case "key":
                        var referenceListName = "";

                        // Check valid syntax.
                        Debug.Assert(x.ChildNodes != null && x.ChildNodes != null,
                            "XML Handling - XSD Parser - CreateChildren - Found a 'key' without child elements.");

                        // Get the referenced item path.
                        foreach (var xc in x.ChildNodes.Cast<XmlElement>().Where(xc => xc.LocalName == "selector"))
                        {
                            Debug.Assert(xc.Attributes != null && xc.Attributes["xpath"] != null,
                            "XML Handling - XSD Parser - CreateChildren - Found a 'key' and child 'selector' without xpath.");
                            referenceListName = xc.Attributes["xpath"].Value.Trim();
                        }

                        // Check valid syntax.
                        Debug.Assert(!string.IsNullOrEmpty(referenceListName),
                            "XML Handling - XSD Parser - CreateChildren - Found a 'key' without a 'selector' child.");

                        // Get the name of the referenced element and create a reference list.
                        referenceListName = referenceListName.Substring(referenceListName.LastIndexOf('/')+1);
                        Structure.Structure.AddReferenceList(referenceListName);
                        break;
                    case "annotation":
                        break;
                    case "keyref":
                        break;
                    default:
                        throw new Exception("Stopped at XSD PARSING...\n Unhandled element: " + x.LocalName);
                }
            }
            return list;
        }
        #endregion
    }
}
