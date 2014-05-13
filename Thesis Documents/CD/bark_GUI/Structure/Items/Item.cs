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
        public bool IsRequired { get; protected set; }
        public bool IsElementItem { get; private set; }
        public bool IsGroupItem { get; private set; }
        public bool IsFunction { get; protected set; }

        /* INHERITING VARIABLES */
        protected XmlNode XsdNode;
        //protected XmlNode XmlNode;
        protected string Help { get; private set; }

        /* PRIVATE VARIABLES */
        private static int _count;
        private int _id;
        private string _newName;


        // Constructor
        protected Item(XmlNode xsdNode, GroupItem parent = null, bool isFunction = false)
        {
            if (!isFunction)
                _id = ++_count;
            XsdNode = xsdNode;
            IsFunction = isFunction;
            Parent = parent;
            IsRequired = XsdParser.IsRequired(xsdNode);
            Name = XsdParser.GetName(xsdNode);
            Help = XsdParser.GetHelp(xsdNode);
            IsGroupItem = XsdParser.IsGroupItem(xsdNode);
            IsElementItem = XsdParser.IsElementItem(xsdNode);
        }







        /* PUBLIC METHODS */

        public Item DuplicateStructure()
        {
            Item clone = null;
            
            // Duplicate via constructor.
            if (IsGroupItem)
                clone = new GroupItem(XsdNode, null, IsFunction);
            else if (IsElementItem)
                clone = new ElementItem(XsdNode, null, IsFunction);

            return clone;
        }

        public Item Duplicate(string newName)
        {
            var newItem = DuplicateStructure();

            // Add this item to parent's children
            if (Parent != null)
            {
                var position = Parent.Children.IndexOf(this);

                // Find the correct position for this item.
                // (to keep the order of the XML and not the XSD + extra of the XML at the end)
                // If this step is skipped every subsequent multiple item will be appended in the end of the Structure and
                // the Viewer(s).

                if (position >= 0)
                {
                    do
                    {
                        position++;
                    } while (position < Parent.Children.Count && Parent.Children[position].Name == Name);
                }

                // If no such child already exists, just place it, else insert it.
                if (position >= 0 && position < Parent.Children.Count) Parent.Children.Insert(position, newItem);
                else Parent.Children.Add(newItem);
            }


            newItem.SetParent(Parent);
            newItem.NewName = newName;

            Structure.AddReference(newItem);

            return newItem;
        }


        public virtual void Remove()
        {
            Control.Remove(); // Doesn't work.

            Parent.Children.Remove(this);

            Structure.RemoveReference(this);
        }



        public void SetParent(GroupItem parent)
        {
            if (parent == null) return;

            Parent = parent;

            if(IsGroupItem)
                parent.Tnode.Nodes.Add(((GroupItem)this).Tnode);
        }




    }
}
