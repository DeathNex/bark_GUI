using System.Xml;
using bark_GUI.XmlHandling;

namespace bark_GUI.Structure.Items
{
    public class Item
    {
        /* PUBLIC VARIABLES */
        public string Name { get; private set; }
        public string NewName;
        public GroupItem Parent { get; private set; }
        public bool IsGroupItem { get; private set; }
        public bool IsFunction { get { return isFunction; } }

        /* INHERITING VARIABLES */
        protected bool IsElementItem { get; private set; }
        protected bool IsRequired { get; private set; }
        protected bool isFunction;
        protected XmlNode XsdNode;
        protected XmlNode XmlNode;
        protected string Help { get; private set; }

        /* PRIVATE VARIABLES */
        private static int _count;
        private int _id;


        //Constructors
        protected Item(XmlNode xsdNode)
        {
            CreateItem(xsdNode, null, false);
        }
        protected Item(XmlNode xsdNode, GroupItem parent)
        {
            CreateItem(xsdNode, parent, false);
        }
        //Used in functions
        protected Item(XmlNode xsdNode, bool isFunction)
        {
            CreateItem(xsdNode, null, isFunction);
        }
        protected Item(XmlNode xsdNode, GroupItem parent, bool isFunction)
        {
            CreateItem(xsdNode, parent, isFunction);
        }

        //Utility Function for Constructors
        private void CreateItem(XmlNode xsdNode, GroupItem parent, bool isFunction)
        {
            if (!isFunction)
                _id = ++_count;
            this.XsdNode = xsdNode;
            this.isFunction = isFunction;
            this.Parent = parent;
            IsRequired = XsdParser.IsRequired(xsdNode);
            Name = XsdParser.GetName(xsdNode);
            Help = XsdParser.GetHelp(xsdNode);
            IsGroupItem = XsdParser.IsGroupItem(xsdNode);
            IsElementItem = XsdParser.IsElementItem(xsdNode);
        }







        /* PUBLIC METHODS */

        public void SetXmlNode(XmlNode xNode) { XmlNode = xNode; }







        
    }
}
