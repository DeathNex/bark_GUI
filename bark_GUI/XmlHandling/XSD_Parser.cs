using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using bark_GUI.Structure.Items;

namespace bark_GUI.XmlHandling
{
    public static class XsdParser
    {
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















        public static bool IsGroupItem(XmlNode xsdNode)
        {
            return !IsElementItem(xsdNode);
        }
        public static bool IsElementItem(XmlNode xsdNode)
        {
            return (HasTypeAttribute(xsdNode) || IsElementReferenceItem(xsdNode));
        }
        public static bool IsElementReferenceItem(XmlNode xsdNode)
        {
            if (xsdNode["xs:complexType"] != null && xsdNode["xs:complexType"].FirstChild != null &&
                xsdNode["xs:complexType"].FirstChild.LocalName == "attribute" &&
                xsdNode["xs:complexType"].FirstChild.Attributes != null &&
                xsdNode["xs:complexType"].FirstChild.Attributes["xs:name"] != null &&
                xsdNode["xs:complexType"].FirstChild.Attributes["xs:name"].Value == "reference")
                return true;
            return false;
        }
        public static bool HasTypeAttribute(XmlNode xsdNode)
        {
            return (xsdNode != null && xsdNode.Attributes != null && xsdNode.Attributes.GetNamedItem("type") != null);
        }
        public static bool IsRequired(XmlNode xsdNode)
        {
            int number;

            if (xsdNode.Attributes == null) return true;

            var xAtt = xsdNode.Attributes.GetNamedItem("minOccurs") as XmlAttribute;

            return xAtt == null || !int.TryParse(xAtt.Value, out number) || number != 0;
        }
        public static bool IsMupltiple(XmlNode xsdNode)
        {
            int number;

            if (xsdNode.Attributes == null) return false;

            var xAtt = xsdNode.Attributes.GetNamedItem("maxOccurs") as XmlAttribute;

            if (xAtt == null) return false;

            if (int.TryParse(xAtt.Value, out number))
            {
                if (number > 1)
                    return true;
            }
            else if (xAtt.Value == "unbounded")
                return true;
            return false;
        }













        public static bool HasType(XmlNode xsdNode)
        {
            return (GetType(xsdNode) != null);
        }
        public static bool HasAttributes(XmlNode xsdNode)
        {
            return xsdNode.NodeType == XmlNodeType.Element && ((XmlElement)xsdNode).HasAttributes;
        }
        public static bool HasReference(XmlNode xsdNode)
        {
            if (xsdNode == null || xsdNode["xs:complexType"] == null ||
                xsdNode["xs:complexType"]["xs:attribute"] == null ||
                xsdNode["xs:complexType"]["xs:attribute"].HasAttributes ||
                xsdNode["xs:complexType"]["xs:attribute"]["@name"] == null)
                return false;

            return xsdNode["xs:complexType"]["xs:attribute"]["@name"].Value == "reference";
        }


        public static List<string> DrawUnits(XmlNode xsdNode)
        {
            List<string> options = new List<string>();
            foreach (XmlNode option in xsdNode.FirstChild.ChildNodes)
                if (option.LocalName == "enumeration")
                {
                    Debug.Assert(option.Attributes != null, "option.Attributes != null");
                    options.Add(option.Attributes.GetNamedItem("value").Value.Trim());
                }
            return options;
        }

    }
}
