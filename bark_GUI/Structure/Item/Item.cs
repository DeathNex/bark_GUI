using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace bark_GUI
{
    public class Item
    {
        /* PUBLIC VARIABLES */
        public string Name { get { return name; } }
        public string NewName;
        public GroupItem Parent { get { return parent; } }
        public bool IsGroupItem { get { return isGroupItem; } }
        public bool IsFunction { get { return isFunction; } }

        /* INHERITING VARIABLES */
        protected bool IsElementItem { get { return isElementItem; } }
        protected bool IsRequired { get { return isRequired; } }
        protected bool isFunction;
        protected XmlNode xsdNode;
        protected XmlNode xmlNode;
        protected string Help { get { return help; } }

        /* PRIVATE VARIABLES */
        private string name;
        private GroupItem parent;
        private static int count;
        private int id;
        private bool isRequired;
        private bool isGroupItem;
        private bool isElementItem;
        private string help;


        //Constructors
        protected Item(XmlNode xsdNode)
        {
            createItem(xsdNode, null, false);
        }
        protected Item(XmlNode xsdNode, GroupItem parent)
        {
            createItem(xsdNode, parent, false);
        }
        //Used in functions
        protected Item(XmlNode xsdNode, bool isFunction)
        {
            createItem(xsdNode, null, isFunction);
        }
        protected Item(XmlNode xsdNode, GroupItem parent, bool isFunction)
        {
            createItem(xsdNode, parent, isFunction);
        }

        //Utility Function for Constructors
        private void createItem(XmlNode xsdNode, GroupItem parent, bool isFunction)
        {
            if (!isFunction)
                id = ++count;
            this.xsdNode = xsdNode;
            this.isFunction = isFunction;
            this.parent = parent;
            isRequired = XSD_Parser.IsRequired(xsdNode);
            name = XSD_Parser.GetName(xsdNode);
            help = XSD_Parser.GetHelp(xsdNode);
            isGroupItem = XSD_Parser.IsGroupItem(xsdNode);
            isElementItem = XSD_Parser.IsElementItem(xsdNode);
        }







        /* PUBLIC METHODS */

        public void SetXMLNode(XmlNode xNode) { xmlNode = xNode; }







        
    }
}
