using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace bark_GUI
{
    public static class XSD_Parser
    {
        public static string GetName(XmlNode xsdNode)
        {
            if (xsdNode.Attributes != null && xsdNode.Attributes.GetNamedItem("name") != null)
                return xsdNode.Attributes.GetNamedItem("name").Value.Trim();
            return null;
        }
        public static string GetType(XmlNode xsdNode)
        {
            return xsdNode.Attributes.GetNamedItem("type").Value.Trim();
        }
        public static string GetHelp(XmlNode xsdNode)
        {
            //Find help text if exists
            if (xsdNode.HasChildNodes)
            {
                foreach (XmlNode xc in xsdNode.ChildNodes)
                    if (xc.HasChildNodes)
                        //Element Item Help
                        if (xc.LocalName == "annotation")
                        {
                            foreach (XmlNode xcc in xc.ChildNodes)
                                if (xcc.LocalName == "documentation")
                                {
                                    return xcc.InnerText;
                                }
                        }
                        //Group Item Help
                        else if (xc.LocalName == "complexType")
                        {
                            foreach (XmlNode xcc in xc.ChildNodes)
                                if (xcc.HasChildNodes && xcc.LocalName == "all" || xcc.LocalName == "sequence" || xcc.LocalName == "choice")
                                    foreach (XmlNode xccc in xcc.ChildNodes)
                                        if (xccc.HasChildNodes && xccc.LocalName == "annotation")
                                        {
                                            foreach (XmlNode xcccc in xccc.ChildNodes)
                                                if (xcccc.LocalName == "documentation")
                                                {
                                                    return xcccc.InnerText;
                                                }
                                        }
                        }
            }
            return null;
        }
        public static List<Item> GetChildren(XmlNode xNode, GroupItem parent, bool isFunction)
        {
            List<Item> list = null;
            if (xNode.HasChildNodes)
            {
                list = new List<Item>(xNode.ChildNodes.Count);
                foreach (XmlNode x in xNode.ChildNodes)
                {
                    if (x.LocalName == "element")
                        if (IsElementItem(x))
                            list.Add(new ElementItem(x, parent, isFunction));
                        else
                            list.Add(new GroupItem(x, parent, isFunction));
                    else if (x.LocalName == "complexType")
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
                }
            }
            return list;
        }















        public static bool IsGroupItem(XmlNode xsdNode) { return !IsElementItem(xsdNode); }
        public static bool IsElementItem(XmlNode xsdNode)
        {
            if (xsdNode.Attributes != null && xsdNode.Attributes.GetNamedItem("type") != null)
                return true;
            if (xsdNode.FirstChild != null && xsdNode.FirstChild.FirstChild != null &&
                xsdNode.FirstChild.FirstChild.LocalName == "attribute" && xsdNode.FirstChild.FirstChild.Attributes.GetNamedItem("name") != null &&
                xsdNode.FirstChild.FirstChild.Attributes.GetNamedItem("name").Value == "reference")
                return true;
            return false;
        }
        public static bool IsRequired(XmlNode xsdNode)
        {
            XmlAttribute xAtt;
            int number;
            if (xsdNode.Attributes != null)
            {
                xAtt = xsdNode.Attributes.GetNamedItem("minOccurs") as XmlAttribute;
                if (xAtt != null && int.TryParse(xAtt.Value, out number) && number == 0)
                    return false;
            }
            return true;
        }
        public static bool IsMupltiple(XmlNode xsdNode)
        {
            XmlAttribute xAtt;
            int number;
            if (xsdNode.Attributes != null)
            {
                xAtt = xsdNode.Attributes.GetNamedItem("maxOccurs") as XmlAttribute;
                if (xAtt != null)
                    if (int.TryParse(xAtt.Value, out number))
                    {
                        if (number > 1)
                            return true;
                    }
                    else if (xAtt.Value == "unbounded")
                        return true;
            }
            return false;
        }













        public static bool HasType(XmlNode xsdNode)
        {
            return (xsdNode.Attributes != null && xsdNode.Attributes.GetNamedItem("type") != null);
        }
        public static bool HasAttributes(XmlNode xsdNode)
        {
            return (xsdNode.Attributes != null);
        }
        public static bool HasReference(XmlNode xsdNode)
        {
            if (xsdNode.HasChildNodes)
                foreach (XmlNode xc in xsdNode.ChildNodes)
                    if (xc.LocalName == "complexType" && xc.HasChildNodes)
                        foreach (XmlNode xcc in xc.ChildNodes)
                            if (xcc.LocalName == "attribute")
                                if (xcc.Attributes.GetNamedItem("name") != null && xcc.Attributes.GetNamedItem("name").Value == "reference")
                                    return true;
                return false;
        }




































        public static List<string> DrawUnits(XmlNode xsdNode)
        {
            List<string> options = new List<string>(); 
            foreach (XmlNode option in xsdNode.FirstChild.ChildNodes)
                if (option.LocalName == "enumeration")
                    options.Add(option.Attributes.GetNamedItem("value").Value.Trim());
            return options;
        }
    }
}
