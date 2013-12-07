﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using bark_GUI.Structure.Items;

namespace bark_GUI.XmlHandling
{
    public static class XsdParser
    {
        #region Get
        /// <summary>
        /// Gets the name attribute of the given xml element if exists.
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
        /// Gets the type of the given xml element if exists.
        /// </summary>
        /// <param name="xsdNode"> Xml element that might contain a type attribute. </param>
        /// <returns>The type if exists or null.</returns>
        public static string GetType(XmlNode xsdNode)
        {
            if (xsdNode != null && xsdNode.Attributes != null && xsdNode.Attributes["type"] != null)
                return xsdNode.Attributes["type"].Value.Trim();
            return null;
        }

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

        public static List<Item> GetChildren(XmlNode xNode, GroupItem parent, bool isFunction)
        {
            List<Item> list = null;

            if (!xNode.HasChildNodes) return null;

            list = new List<Item>(xNode.ChildNodes.Count);

            foreach (XmlNode x in xNode.ChildNodes)
            {
                switch (x.LocalName)
                {
                    case "element":
                        if (IsElementItem(x))
                            list.Add(new ElementItem(x, parent, isFunction));
                        else
                            list.Add(new GroupItem(x, parent, isFunction));
                        break;
                    case "complexType":
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
                }
            }
            return list;
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
        #endregion
    }
}
