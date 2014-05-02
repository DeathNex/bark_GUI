using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using System.Windows.Forms;
using bark_GUI.CustomControls;
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
        public List<Item> InnerChildren
        {
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

        public bool HasRightClickActions { get; private set; }

        // Constructor
        /// <summary> Creates a child GroupItem from the XSD Functions file. </summary>
        /// <param name="xsdNode"> A XmlNode from the XSD Functions file. </param>
        /// <param name="parent"> The parent of this item. </param>
        /// <param name="isFunction"> Note if this is a Function. </param>
        public GroupItem(XmlNode xsdNode, GroupItem parent = null, bool isFunction = false)
            : base(xsdNode, parent, isFunction)
        {
            // Can exist multiple times?
            IsMultiple = XsdParser.IsMupltiple(xsdNode);

            // Can the user do Right-Click actions on this element?
            HasRightClickActions = XsdParser.HasRightClickActions(xsdNode);

            // Create the TreeNode of TreeViewer.
            Tnode = new TreeNode(Name) { Tag = this, ToolTipText = Help };
            if (parent != null)
                parent.Tnode.Nodes.Add(Tnode);

            // Children
            Children = XsdParser.CreateChildren(xsdNode, this, isFunction);

            // Create the Controls for this ElementItem with the gathered information
            Control = new GeneralControl(Name, IsRequired, Help);
        }


        #region Public Methods
        
        public override void Remove()
        {
            Item child;

            Tnode.Remove();

            while (Children.Count > 0)
            {
                child = Children[0];
                child.Remove();
            }

            base.Remove();
        }

        #endregion
    }
}
