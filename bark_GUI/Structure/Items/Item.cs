using System.Diagnostics;
using System.Xml;
using bark_GUI.CustomControls;
using bark_GUI.XmlHandling;

namespace bark_GUI.Structure.Items
{
    public class Item
    {
        /* PUBLIC VARIABLES */
        public string Name { get; private set; }
        public string NewName
        {
            get { return _newName; }
            set
            {
                _newName = value;

                // Update the Tree-Node of the Group Viewer with the custom new name.
                if (IsGroupItem)
                    ((GroupItem)this).Tnode.Text = CustomName;

                // Update the Control of the Element Viewer with the custom new name.
                Control.Name = CustomName;
            }
        }
        public string CustomName { get { return !string.IsNullOrEmpty(_newName) ? string.Format("({0}) {1}", Name, _newName) : Name; } }
        public GroupItem Parent { get; private set; }
        public GeneralControl Control { get; protected set; }
        public bool IsElementItem { get; private set; }
        public bool IsGroupItem { get; private set; }
        public bool IsFunction { get { return _isFunction; } protected set { _isFunction = value; } }

        /* INHERITING VARIABLES */
        protected bool IsRequired { get; private set; }
        protected XmlNode XsdNode;
        protected XmlNode XmlNode;
        protected string Help { get; private set; }

        /* PRIVATE VARIABLES */
        private static int _count;
        private int _id;
        private string _newName;
        private bool _isFunction;


        #region Constructors

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
            this._isFunction = isFunction;
            this.Parent = parent;
            IsRequired = XsdParser.IsRequired(xsdNode);
            Name = XsdParser.GetName(xsdNode);
            Help = XsdParser.GetHelp(xsdNode);
            IsGroupItem = XsdParser.IsGroupItem(xsdNode);
            IsElementItem = XsdParser.IsElementItem(xsdNode);
        }

        #endregion






        /* PUBLIC METHODS */

        public void SetXmlNode(XmlNode xNode) { XmlNode = xNode; }








    }
}
