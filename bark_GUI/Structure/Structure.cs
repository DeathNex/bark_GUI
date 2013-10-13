using System;
using System.Collections.Generic;
using System.Xml;
using System.Windows.Forms;

namespace bark_GUI
{
    static class Structure
    {
        /* PUBLIC VARIABLES */
        public static GroupItem Root { get { return root; } }
        public static List<Item> Items { get { return items; } }
        public static List<ElementItem> ElementItems { get { return elementItems; } }
        public static List<GroupItem> GroupItems { get { return groupItems; } }


        /* PRIVATE VARIABLES */
        private static GroupItem root;
        private static List<Unit> units;
        private static List<ItemType> types;
        private static List<SimpleType> simpleTypes;
        private static List<ComplexType> complexTypes;
        private static List<GroupItem> functions;
        private static List<Item> items;
        private static List<ElementItem> elementItems;
        private static List<GroupItem> groupItems;








        /* PUBLIC INITIALIZATION METHODS */









        public static void InitializeTypes()
        {
            units = new List<Unit>();
            types = new List<ItemType>();
            simpleTypes = new List<SimpleType>();
            complexTypes = new List<ComplexType>();
            functions = new List<GroupItem>();
            items = new List<Item>();
            elementItems = new List<ElementItem>();
            groupItems = new List<GroupItem>();

            //These types pre-exist in the XSD
            Add(new SimpleType("xs:string", BasicType._string));
            Add(new SimpleType("xs:integer", BasicType._integer));
            Add(new SimpleType("xs:PositiveInteger", BasicType._integer, 1, double.MaxValue));
            Add(new SimpleType("xs:nonPositiveInteger", BasicType._integer, double.MinValue, 0));
            Add(new SimpleType("xs:nonNegativeInteger", BasicType._integer, 0, double.MaxValue));
            Add(new SimpleType("xs:negativeInteger", BasicType._integer, double.MinValue, -1));
            Add(new SimpleType("xs:decimal", BasicType._decimal));
            Add(new SimpleType("decimal_positive", BasicType._decimal, 0, double.MaxValue));
        }


        public static void SetRoot(GroupItem newRoot) { root = newRoot; }

        

        










        /* PUBLIC METHODS */





        public static void Add(Unit unit) { units.Add(unit); }
        public static void Add(SimpleType type) { types.Add(type); simpleTypes.Add(type); }
        public static void Add(ComplexType type) { types.Add(type); complexTypes.Add(type); }
        public static void Add(ElementItem e_Item) { elementItems.Add(e_Item); items.Add(e_Item); }
        public static void Add(GroupItem g_Item) { if (g_Item.IsFunction) functions.Add(g_Item); else { groupItems.Add(g_Item); items.Add(g_Item); } }
        public static Unit FindUnit(string unitName) { foreach (Unit u in units) if (u.name == unitName)return u; return null; }
        public static ItemType FindType(string typeName) { foreach (ItemType t in types) if (t.name == typeName)return t; return null; }
        public static SimpleType FindSimpleType(string typeName) { foreach (SimpleType t in simpleTypes) if (t.name == typeName)return t; return null; }
        public static ComplexType FindComplexType(string typeName) { foreach (ComplexType t in complexTypes) if (t.name == typeName)return t; return null; }

        //ElementItems require special treatment because duplicates exist.
        public static Item FindItem(XmlNode xmlItem)
        {
            List<Item> results = new List<Item>();

            foreach (Item i in items)
                if (i.Name == xmlItem.Name)
                    results.Add(i);

            if (results.Count == 0)
                return null;
            else if (results.Count == 1)
            {
                if (results[0].Name == "boundary")
                    results = results;
                return results[0];
            }
            else
                return _findItemResult(xmlItem, results);
        }

































        /* UTILITY METHODS */


        private static Item _findItemResult(XmlNode xmlItem, List<Item> results)
        {
            //Distinguish by parent-name
            List<Item> resultsParentName = new List<Item>(results.Count);
            foreach (Item r in results)
                if (xmlItem.ParentNode.Name == r.Parent.Name)
                    resultsParentName.Add(r);
            if (resultsParentName.Count == 1)
                return resultsParentName[0];

            //Distinguish by parent's attribute name
            return null;
            //Distinguish by parent's parent name
        }










    }
}
