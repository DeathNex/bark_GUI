using System.Collections.Generic;
using System.Xml;
using System.Windows.Forms;
using bark_GUI.XmlHandling;

namespace bark_GUI.Structure.Items
{
    public class GroupItem : Item
    {
        /* PUBLIC VARIABLES */
        public List<Item> Children { get; private set; }
        public List<Item> InnerChildren { get; private set; }
        public TreeNode Tnode { get; private set; }

        /* PRIVATE VARIABLES */
        private bool _multiple;
        private static int _multiplesCount;
        private int _mID;


        //Constructors
        /// <summary> Creates the Root GroupItem of the XSD file that is not a function. </summary>
        /// <param name="xsdNode"> The root XmlNode of the XSD file. </param>
        public GroupItem(XmlNode xsdNode)
            : base(xsdNode)
        {
            CreateGroupItem(xsdNode, null, false);
        }
        /// <summary> Creates a chile GroupItem from the XSD file that is not a function. </summary>
        /// <param name="xsdNode"> The XmlNode of the XSD file. </param>
        /// <param name="parent"> The parent of this Node. </param>
        public GroupItem(XmlNode xsdNode, GroupItem parent)
            : base(xsdNode, parent)
        {
            CreateGroupItem(xsdNode, parent, false);
        }
        /// <summary> Creates a Root GroupItem from the XSD Functions file. (used for functions) </summary>
        /// <param name="xsdNode"> A root XmlNode from the XSD Functions file. (used for functions) </param>
        /// <param name="isFunction"> Note that this is a Function. </param>
        public GroupItem(XmlNode xsdNode, bool isFunction)
            : base(xsdNode, isFunction)
        {
            CreateGroupItem(xsdNode, null, isFunction);
        }
        /// <summary> Creates a child GroupItem from the XSD Functions file. (used for functions) </summary>
        /// <param name="xsdNode"> A XmlNode from the XSD Functions file. (used for functions) </param>
        /// <param name="parent"> The parent of this Node. </param>
        /// <param name="isFunction"> Note that this is a Function. </param>
        public GroupItem(XmlNode xsdNode, GroupItem parent, bool isFunction)
            : base(xsdNode, parent, isFunction)
        {
            CreateGroupItem(xsdNode, parent, isFunction);
        }
        // UNUSED! UNUSED! UNUSED! UNUSED! UNUSED! UNUSED! UNUSED! UNUSED! UNUSED! UNUSED! UNUSED! UNUSED!
        /// <summary> Creates a child GroupItem from the XSD Functions file. (used for functions) </summary>
        /// <param name="xsdNode"> A XmlNode from the XSD Functions file. (used for functions) </param>
        /// <param name="parent"> The parent of this Node. </param>
        /// <param name="isFunction"> Note if this is a Function. </param>
        /// <param name="mID"> Multiple ID. </param>
        public GroupItem(XmlNode xsdNode, GroupItem parent, bool isFunction, int mID)
            : base(xsdNode, parent, isFunction)
        {
            CreateGroupItem(xsdNode, parent, isFunction);
        }

        //Utility Function for Constructors
        private void CreateGroupItem(XmlNode xsdNode, GroupItem parent, bool isFunction)
        {
            //TreeNode of TreeViewer
            Tnode = new TreeNode(Name) { ToolTipText = Help };
            if (parent != null)
                parent.Tnode.Nodes.Add(Tnode);

            //Can exist multiple times?
            _multiple = XsdParser.IsMupltiple(xsdNode);

            //Children
            Children = XsdParser.GetChildren(xsdNode, this, isFunction);
            InnerChildren = new List<Item>();
            _getInnerChildren(this, this);

            //Include this in the Structure
            if (!isFunction)
                bark_GUI.Structure.Structure.Add(this);
        }






        /* PUBLIC METHODS */


        public GroupItem Duplicate() { return new GroupItem(XsdNode, Parent); }







        /* PRIVATE METHODS */


        private void _getInnerChildren(GroupItem g, GroupItem source)
        {
            if (g.Children != null)
                foreach (Item i in g.Children)
                {
                    source.InnerChildren.Add(i);
                    if (i.IsGroupItem && ((GroupItem)i).Children != null)
                        _getInnerChildren((GroupItem)i, source);
                }
        }
    }
}
