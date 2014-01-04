using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Windows.Forms;
using bark_GUI.XmlHandling;

namespace bark_GUI.Structure.Items
{
    public class GroupItem : Item
    {
        /* PUBLIC PROPERTIES */
        public List<Item> Children { get; private set; }

        /// <summary>
        /// Using the existing children, iterates in depth and adds all inner children through recursion.
        /// </summary>
        public List<Item> InnerChildren {
            get
            {
                var innerChildren = new List<Item>();

                if (Children != null)
                    foreach (Item i in Children)
                    {
                        innerChildren.Add(i);
                        if (i.IsGroupItem && ((GroupItem)i).Children != null)
                            innerChildren.AddRange(((GroupItem)i).InnerChildren);
                    }

                return innerChildren;
            }
        }

        public TreeNode Tnode { get; private set; }

        public bool IsMultiple { get; private set; }

        /* PRIVATE PROPERTIES */
        // Check used for multiples. If it's set create another instance. Used in DuplicateMultiple() method.
        private bool IsSet { get { return XmlNode != null; } }


        #region Constructors
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

        // Utility Function for Constructors. (to avoid code repeatance)
        private void CreateGroupItem(XmlNode xsdNode, GroupItem parent, bool isFunction)
        {
            //TreeNode of TreeViewer
            Tnode = new TreeNode(Name) { ToolTipText = Help };
            if (parent != null)
                parent.Tnode.Nodes.Add(Tnode);

            //Can exist multiple times?
            IsMultiple = XsdParser.IsMupltiple(xsdNode);

            //Children
            Children = XsdParser.CreateChildren(xsdNode, this, isFunction);

            //Include this in the Structure
            if (!isFunction)
                Structure.Add(this);
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Duplicates the current GroupItem and adds it to the Structure.
        /// </summary>
        /// <returns> The new clone of GroupItem. </returns>
        public GroupItem Duplicate()
        {
            var clone = new GroupItem(XsdNode, Parent);
            Parent.Children.Add(clone);
            return clone;
        }

        public GroupItem DuplicateMultiple()
        {
            // Check if this instance is set, if it's not set use this instance (to avoid empty item in structure).
            return IsSet ? Duplicate() : this;
        }

        #endregion


    }
}
