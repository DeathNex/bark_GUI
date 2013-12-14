using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using bark_GUI.Structure.ItemTypes;
using bark_GUI.Structure.Items;

namespace bark_GUI.Structure
{
    static class Structure
    {
        /* PUBLIC PROPERTIES */
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

        #region Public Initiliazation Methods

        public static void InitializeTypes()
        {
            // Initialize collection objects.
            _units = new List<Unit>();
            _types = new List<ItemType>();
            _simpleTypes = new List<SimpleType>();
            _complexTypes = new List<ComplexType>();
            _functions = new List<GroupItem>();
            Items = new List<Item>();
            ElementItems = new List<ElementItem>();
            GroupItems = new List<GroupItem>();

            // These types pre-exist in the XSD. (primitive types)
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

        #endregion

        #region Public Methods

        #region Add
        public static void Add(Unit unit) { _units.Add(unit); }

        public static void Add(SimpleType type) { _types.Add(type); _simpleTypes.Add(type); }

        public static void Add(ComplexType type) { _types.Add(type); _complexTypes.Add(type); }

        public static void Add(ElementItem eItem) { ElementItems.Add(eItem); Items.Add(eItem); }

        public static void Add(GroupItem gItem) { if (gItem.IsFunction) _functions.Add(gItem); else { GroupItems.Add(gItem); Items.Add(gItem); } }
        #endregion

        #region Find
        public static Unit FindUnit(string unitName) { return _units.FirstOrDefault(u => u.Name == unitName); }

        public static ItemType FindType(string typeName) { return _types.FirstOrDefault(t => t.Name == typeName); }

        public static SimpleType FindSimpleType(string typeName) { return _simpleTypes.FirstOrDefault(t => t.Name == typeName); }

        public static ComplexType FindComplexType(string typeName) { return _complexTypes.FirstOrDefault(t => t.Name == typeName); }

        // ElementItems require special treatment because duplicates exist.
        public static Item FindItem(XmlNode xmlItem)    // CHECK: Xml dependency. Can be removed?
        {
            const string errorMsg = "Structure - FindItem:\n - ";
            var results = Items.Where(i => i.Name == xmlItem.Name).ToList();

            Debug.Assert(results.Count > 0, "No matches found for item '" + xmlItem.Name +
                "' in the Structure.\n     Please make sure the names are correct.");

            // One result found, return it.
            if (results.Count == 1)
                return results[0];

            // Multiple result found, use filters to further indentify the corrent result.
            var result = FindItemWithFilters(xmlItem, results);

            // Check for error Filters were not sufficient or no results were found.
            Debug.Assert(result != null, errorMsg + "Filters were incapable of distinguishing a single item '"
                    + xmlItem.Name + "' in the Structure.\n     Please make sure no exact duplicates exist.");

            return result;
        }

        #endregion

        #endregion


        #region Private Find Item With Filter Methods
        private static Item FindItemWithFilters(XmlNode xmlItem, List<Item> results)
        {

            // Rise in parents 'till a different parent was found using filters. //!!!

        }

        private static List<Item> FindItemWithParentExistanceFilter(XmlNode xmlItem, List<Item> results)
        {
            List<Item> resultsFiltered = new List<Item>(results.Count);

            // 0. Filter by parent existance.
            resultsFiltered.AddRange(results.Where(r => xmlItem.ParentNode != null && r.Parent != null));

            return resultsFiltered;
        }

        private static List<Item> FindItemWithParentNameFilter(XmlNode xmlItem, List<Item> results)
        {
            List<Item> resultsFiltered = new List<Item>(results.Count);

            // 1. Filter by parent-name.
            resultsFiltered.AddRange(results.Where(r => xmlItem.ParentNode != null && xmlItem.ParentNode.Name == r.Parent.Name));

            return resultsFiltered;
        }

        private static List<Item> FindItemWithParentCustomNameFilter(XmlNode xmlItem, List<Item> results)
        {
            List<Item> resultsFiltered = new List<Item>(results.Count);

            // 2. Filter by parent's attribute (custom) name.
            resultsFiltered.AddRange(results.Where(r => xmlItem.ParentNode != null &&
                xmlItem.ParentNode.Attributes != null && xmlItem.ParentNode.Attributes["name"] != null &&
                xmlItem.ParentNode.Attributes["name"].Value.Trim() == r.Parent.NewName));

            return resultsFiltered;
        }
        #endregion
    }
}
