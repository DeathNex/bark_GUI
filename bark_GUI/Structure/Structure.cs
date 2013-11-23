using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using bark_GUI.Structure.ItemTypes;
using bark_GUI.Structure.Items;

namespace bark_GUI.Structure
{
    static class Structure
    {
        /* PUBLIC VARIABLES */
        public static GroupItem Root { get; private set; }
        public static List<Item> Items { get; private set; }
        public static List<ElementItem> ElementItems { get; private set; }
        public static List<GroupItem> GroupItems { get; private set; }


        /* PRIVATE VARIABLES */
        private static List<Unit> _units;
        private static List<ItemType> _types;
        private static List<SimpleType> _simpleTypes;
        private static List<ComplexType> _complexTypes;
        private static List<GroupItem> _functions;


        /* PUBLIC INITIALIZATION METHODS */









        public static void InitializeTypes()
        {
            _units = new List<Unit>();
            _types = new List<ItemType>();
            _simpleTypes = new List<SimpleType>();
            _complexTypes = new List<ComplexType>();
            _functions = new List<GroupItem>();
            Items = new List<Item>();
            ElementItems = new List<ElementItem>();
            GroupItems = new List<GroupItem>();

            //These types pre-exist in the XSD
            Add(new SimpleType("xs:string", BasicType.String));
            Add(new SimpleType("xs:integer", BasicType.Integer));
            Add(new SimpleType("xs:PositiveInteger", BasicType.Integer, 1, double.MaxValue));
            Add(new SimpleType("xs:nonPositiveInteger", BasicType.Integer, double.MinValue, 0));
            Add(new SimpleType("xs:nonNegativeInteger", BasicType.Integer, 0, double.MaxValue));
            Add(new SimpleType("xs:negativeInteger", BasicType.Integer, double.MinValue, -1));
            Add(new SimpleType("xs:decimal", BasicType.Decimal));
            Add(new SimpleType("decimal_positive", BasicType.Decimal, 0, double.MaxValue));
        }


        public static void SetRoot(GroupItem newRoot) { Root = newRoot; }














        /* PUBLIC METHODS */





        public static void Add(Unit unit) { _units.Add(unit); }
        public static void Add(SimpleType type) { _types.Add(type); _simpleTypes.Add(type); }
        public static void Add(ComplexType type) { _types.Add(type); _complexTypes.Add(type); }
        public static void Add(ElementItem e_Item) { ElementItems.Add(e_Item); Items.Add(e_Item); }
        public static void Add(GroupItem g_Item) { if (g_Item.IsFunction) _functions.Add(g_Item); else { GroupItems.Add(g_Item); Items.Add(g_Item); } }
        public static Unit FindUnit(string unitName) { foreach (Unit u in _units) if (u.Name == unitName)return u; return null; }
        public static ItemType FindType(string typeName) { foreach (ItemType t in _types) if (t.Name == typeName)return t; return null; }
        public static SimpleType FindSimpleType(string typeName) { foreach (SimpleType t in _simpleTypes) if (t.Name == typeName)return t; return null; }
        public static ComplexType FindComplexType(string typeName) { foreach (ComplexType t in _complexTypes) if (t.Name == typeName)return t; return null; }

        //ElementItems require special treatment because duplicates exist.
        public static Item FindItem(XmlNode xmlItem)
        {
            List<Item> results = new List<Item>();

            foreach (Item i in Items)
                if (i.Name == xmlItem.Name)
                    results.Add(i);

            if (results.Count == 0)
                return null;
            else if (results.Count == 1)
            {
                if (results[0].Name == "boundary")      // TODO Multiple items handling.
                    Debug.Print("### Element boundary hit! Results:\n{0}", results);
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
