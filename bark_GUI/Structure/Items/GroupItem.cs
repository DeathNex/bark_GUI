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

        /* PRIVATE VARIABLES */
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
            // Can exist multiple times?
            IsMultiple = XsdParser.IsMupltiple(xsdNode);

            // Can the user take Right-Click actions on this element?
            HasRightClickActions = XsdParser.HasRightClickActions(xsdNode);

            // Create the TreeNode of TreeViewer.
            Tnode = new TreeNode(Name) { Tag = this, ToolTipText = Help };
            if (parent != null)
                parent.Tnode.Nodes.Add(Tnode);

            // Children
            Children = XsdParser.CreateChildren(xsdNode, this, isFunction);

            //Create the Controls for this ElementItem with the gathered information
            Control = new GeneralControl(XmlNode, Name, IsRequired, Help);

            // The following actions do not apply on 'function' element types.
            if (isFunction) return;

            // Include this in the Structure
            Structure.Add(this);
            //if (!IsMultiple) Structure.Add(this);
            //else Structure.Insert(this);
        }
        #endregion


        #region Public Methods
        /// <summary>
        /// Duplicates the current GroupItem and adds it to the Structure.
        /// </summary>
        /// <returns> The new clone of GroupItem. </returns>
        private GroupItem DuplicateEmpty()
        {
            var clone = new GroupItem(XsdNode, Parent);
            var position = Parent.Children.IndexOf(this);

            // Find the correct position for this item.
            // (to keep the order of the XML and not the XSD + extra of the XML at the end)
            // If this step is skipped every subsequent multiple item will be appended in the end of the Structure and
            // the Viewer(s).
            if (position >= 0)
            {
                do { position++; } while (position < Parent.Children.Count && Parent.Children[position].Name == Name);
            }

            // If no such child already exists, just place it, else insert it.
            if (position == Parent.Children.Count) Parent.Children.Add(clone);
            else Parent.Children.Insert(position, clone);

            return clone;
        }
        
        public void DuplicateDeep()
        {
            // Get the data from the XML.
            if (XmlNode == null) return;
            var newXml = XmlNode.CloneNode(true);
            Debug.Assert(Parent.XmlNode == XmlNode.ParentNode, "Inconsistency Between Parent 'Item' and Parent 'XmlNode'.");
            var x = Parent.XmlNode.GetHashCode(); //DELETEME
            var y = XmlNode.ParentNode.GetHashCode(); //DELETEME
            if (XmlNode.ParentNode != null)
                XmlNode.ParentNode.InsertAfter(newXml, XmlNode);
            Debug.Assert(newXml == XmlNode.NextSibling, "Inconsistency Caused by INSERTAFTER");
            newXml.AppendChild(newXml.FirstChild);
            var x2 = newXml.GetHashCode(); //DELETEME
            var y2 = XmlNode.NextSibling.GetHashCode(); //DELETEME
            XmlParser.DrawInfo(newXml);
        }

        public GroupItem DuplicateEmptyMultiple()
        {
            // Check if this instance is set, if it's not set use this instance (to avoid empty item in structure).
            return IsSet ? DuplicateEmpty() : this;
        }

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
