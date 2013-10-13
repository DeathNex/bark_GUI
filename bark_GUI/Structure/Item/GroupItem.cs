using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace bark_GUI
{
    public class GroupItem : Item
    {
        /* PUBLIC VARIABLES */
        public List<Item> Children { get { return children; } }
        public List<Item> InnerChildren { get { return innerChildren; } }
        public TreeNode TNode { get { return tNode; } }

        /* PRIVATE VARIABLES */
        private bool multiple;
        private static int multiplesCount;
        private int mID;
        private List<Item> children;
        private List<Item> innerChildren;
        private TreeNode tNode;



        //Constructors
        /// <summary> Creates the Root GroupItem of the XSD file that is not a function. </summary>
        /// <param name="xsdNode"> The root XmlNode of the XSD file. </param>
        public GroupItem(XmlNode xsdNode)
            : base(xsdNode)
        {
            createGroupItem(xsdNode, null, false);
        }
        /// <summary> Creates a chile GroupItem from the XSD file that is not a function. </summary>
        /// <param name="xsdNode"> The XmlNode of the XSD file. </param>
        /// <param name="parent"> The parent of this Node. </param>
        public GroupItem(XmlNode xsdNode, GroupItem parent)
            : base(xsdNode, parent)
        {
            createGroupItem(xsdNode, parent, false);
        }
        /// <summary> Creates a Root GroupItem from the XSD Functions file. (used for functions) </summary>
        /// <param name="xsdNode"> A root XmlNode from the XSD Functions file. (used for functions) </param>
        /// <param name="isFunction"> Note that this is a Function. </param>
        public GroupItem(XmlNode xsdNode, bool isFunction)
            : base(xsdNode, isFunction)
        {
            createGroupItem(xsdNode, null, isFunction);
        }
        /// <summary> Creates a child GroupItem from the XSD Functions file. (used for functions) </summary>
        /// <param name="xsdNode"> A XmlNode from the XSD Functions file. (used for functions) </param>
        /// <param name="parent"> The parent of this Node. </param>
        /// <param name="isFunction"> Note that this is a Function. </param>
        public GroupItem(XmlNode xsdNode, GroupItem parent, bool isFunction)
            : base(xsdNode, parent, isFunction)
        {
            createGroupItem(xsdNode, parent, isFunction);
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
            createGroupItem(xsdNode, parent, isFunction);
        }

        //Utility Function for Constructors
        private void createGroupItem(XmlNode xsdNode, GroupItem parent, bool isFunction)
        {
            //TreeNode of TreeViewer
            tNode = new TreeNode(Name);
            tNode.ToolTipText = Help;
            if (parent != null)
                parent.tNode.Nodes.Add(tNode);

            //Can exist multiple times?
            multiple = XSD_Parser.IsMupltiple(xsdNode);

            //Children
            children = XSD_Parser.GetChildren(xsdNode, this, isFunction);
            innerChildren = new List<Item>();
            _getInnerChildren(this, this);

            //Include this in the Structure
            if (!isFunction)
                Structure.Add(this);
        }






        /* PUBLIC METHODS */


        public GroupItem Duplicate() { return new GroupItem(xsdNode, Parent); }







        /* PRIVATE METHODS */


        private void _getInnerChildren(GroupItem g, GroupItem source)
        {
            if (g.children != null)
                foreach (Item i in g.children)
                {
                    source.innerChildren.Add(i);
                    if (i.IsGroupItem && ((GroupItem)i).children != null)
                        _getInnerChildren((GroupItem)i, source);
                }
        }
    }
}
