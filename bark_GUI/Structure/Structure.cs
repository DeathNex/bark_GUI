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
        // Public Variables
        public static GroupItem DataRootItem { get; set; }

        /* PRIVATE VARIABLES */
        private static GroupItem _root;

        private static List<Item> _items;

        private static List<ElementItem> _elementItems;

        private static List<GroupItem> _groupItems;

        private static List<Unit> _units;

        private static List<ItemType> _types;

        private static List<SimpleType> _simpleTypes;

        private static List<ComplexType> _complexTypes;

        private static Dictionary<string, List<Item>> _referenceLists;

        private static List<GroupItem> _functions;

        #region Public Initialization Methods

        public static void InitializeTypes()
        {
            // Initialize collection objects.
            _units = new List<Unit>();
            _types = new List<ItemType>();
            _simpleTypes = new List<SimpleType>();
            _complexTypes = new List<ComplexType>();
            _referenceLists = new Dictionary<string, List<Item>>();
            _functions = new List<GroupItem>();
            _items = new List<Item>();
            _elementItems = new List<ElementItem>();
            _groupItems = new List<GroupItem>();

            // These types pre-exist in the XSD. (primitive types)
            Add(new SimpleType("xs:string", BasicType.String));
            Add(new SimpleType("xs:integer", BasicType.Integer));
            Add(new SimpleType("xs:PositiveInteger", BasicType.Integer, 1, decimal.MaxValue));
            Add(new SimpleType("xs:nonPositiveInteger", BasicType.Integer, decimal.MinValue, 0));
            Add(new SimpleType("xs:nonNegativeInteger", BasicType.Integer, 0, decimal.MaxValue));
            Add(new SimpleType("xs:negativeInteger", BasicType.Integer, decimal.MinValue, -1));
            Add(new SimpleType("xs:decimal", BasicType.Decimal));
            Add(new SimpleType("decimal_positive", BasicType.Decimal, 0, decimal.MaxValue));
        }

        public static void SetRoot(GroupItem newRoot)
        {
            _root = newRoot;

            foreach (var innerChild in newRoot.InnerChildren)
            {
                Add(innerChild);
            }
        }

        #endregion

        #region Public Methods

        #region Add
        // These methods are used during the Creation of the Structure. (When loading the XSD)

        public static void Add(Unit unit) { _units.Add(unit); }

        public static void Add(SimpleType type) { _types.Add(type); _simpleTypes.Add(type); }

        public static void Add(ComplexType type) { _types.Add(type); _complexTypes.Add(type); }

        public static void Add(Item item)
        {
            if (item.IsElementItem)
            {
                _items.Add(item);
                _elementItems.Add((ElementItem)item);
            }
            else if (item.IsGroupItem)
            {
                if (!item.IsFunction)
                {
                    _items.Add(item);
                    _groupItems.Add((GroupItem)item);
                }
                else
                {
                    _functions.Add((GroupItem)item);
                }
            }
        }

        public static void AddReferenceList(string name) { _referenceLists[name] = new List<Item>(); }

        public static void AddReference(Item item){if (_referenceLists.ContainsKey(item.Name)) _referenceLists[item.Name].Add(item); }
        #endregion

        #region Find
        // These methods are used during the Info Population on the already created Structure. (When loading the XML)

        public static Unit FindUnit(string unitName) { return _units.FirstOrDefault(u => u.Name == unitName); }

        public static ItemType FindType(string typeName) { return _types.FirstOrDefault(t => t.Name == typeName); }

        public static SimpleType FindSimpleType(string typeName) { return _simpleTypes.FirstOrDefault(t => t.Name == typeName); }

        public static ComplexType FindComplexType(string typeName) { return _complexTypes.FirstOrDefault(t => t.Name == typeName); }

        // Returns a list of references on that element name as a string for controls to show.
        public static List<string> FindReferenceListOptions(string name)
        {
            return !_referenceLists.ContainsKey(name) ? null : _referenceLists[name].Select(item => item.NewName ?? item.Name).ToList();
        }

        // Items require special treatment because duplicates exist.
        // Returns the 'Data' item that was created after XML.
        public static Item FindDataItem(XmlNode xmlItem)
        {
            const string errorMsg = "Structure - FindDataItem:\n - ";
            Item result = null;

            if (xmlItem.Name == DataRootItem.Name) return DataRootItem;

            var results = DataRootItem.InnerChildren.Where(item => item.Name == xmlItem.Name).ToList();

            Debug.Assert(results.Count > 0, "No matches found for item '" + xmlItem.Name +
                "' in the DataRootItem.\n     Please make sure the names are correct.");

            // If multiple results found, use filters to further indentify the correct result.
            result = results.Count == 1 ? results[0] : FindItemWithFilters(xmlItem, results);

            // Check for error Filters were not sufficient or no results were found.
            Debug.Assert(result != null, errorMsg + "Filters were incapable of distinguishing a single item '"
                    + xmlItem.Name + "' in the DataRootItem.\n     Please make sure no exact duplicates exist.");

            return result;
        }

        #endregion

        // Returns a clone of the 'Structure' item that was loaded from XSD.
        public static Item CreateItem(XmlNode xmlItem)
        {
            var item = FindStructureItem(xmlItem);

            return item.DuplicateStructure();
        }


        #endregion


        // Items require special treatment because duplicates exist.
        // Returns the 'Structure' item that was loaded from XSD.
        internal static Item FindStructureItem(XmlNode xmlItem)
        {
            const string errorMsg = "Structure - FindStructureItem:\n - ";
            Item result = null;

            if (xmlItem.Name == _root.Name) return _root;

            var results = _items.Where(i => i.Name == xmlItem.Name).ToList();

            Debug.Assert(results.Count > 0, "No matches found for item '" + xmlItem.Name +
                "' in the Structure.\n     Please make sure the names are correct.");

            // If multiple results found, use filters to further indentify the correct result.
            result = results.Count == 1 ? results[0] : FindItemWithFilters(xmlItem, results);

            // Check for error Filters were not sufficient or no results were found.
            Debug.Assert(result != null, errorMsg + "Filters were incapable of distinguishing a single item '"
                    + xmlItem.Name + "' in the Structure.\n     Please make sure no exact duplicates exist.");

            return result;
        }

        #region Private Find Item With Filter Methods

        private static Item FindItemWithFilters(XmlNode xmlItem, List<Item> results)
        {
            var risingXmlItem = xmlItem;
            List<Item> filteredResults = null;

            // Rise in parents 'till a different parent was found using filters. //!!!

            // 1. Check custom name.
            filteredResults = FindItemWithNewNameFilter(risingXmlItem, results);
            if (filteredResults.Count == 1) return filteredResults[0];

            // 2. Check parent existance.
            filteredResults = FindItemWithParentExistanceFilter(risingXmlItem, results);
            if (filteredResults.Count == 1) return filteredResults[0];
            // If no parent exists, the search cannot continue.
            if (filteredResults.Count <= 0) return null;

            // 3. Check parent name.
            filteredResults = FindItemWithParentNameFilter(risingXmlItem, results);
            if (filteredResults.Count == 1) return filteredResults[0];

            // 4. Check parent custom name.
            filteredResults = FindItemWithParentCustomNameFilter(risingXmlItem, results);

            if (filteredResults.Count == 1) return filteredResults[0];

            // Rise in parent.
            var newXmlItem = xmlItem.ParentNode;
            var newResults = results.Select(t => t.Parent).Distinct().Cast<Item>().ToList();

            Debug.Assert(newResults.Count>0, "Filters were incapable of distinguishing a single item '"
                    + xmlItem.Name + "' in the Structure.\n     Please make sure no exact duplicates exist.");

            var finalResult = FindItemWithFilters(newXmlItem, newResults);
            if (finalResult != null && finalResult.IsGroupItem)
            {
                var groupItem = finalResult as GroupItem;
                if (groupItem != null) return groupItem.Children.Find(results.Contains);
            }
            return null;
        }

        private static List<Item> FindItemWithNewNameFilter(XmlNode xmlItem, List<Item> results)
        {
            List<Item> resultsFiltered = new List<Item>(results.Count);

            // 1. Filter by custom name.
            resultsFiltered.AddRange(results.Where(r =>
                (xmlItem.Attributes != null && xmlItem.Attributes["name"] != null) &&
                r.NewName != null && r.NewName == xmlItem.Attributes["name"].Value));

            return resultsFiltered;
        }

        private static List<Item> FindItemWithParentExistanceFilter(XmlNode xmlItem, List<Item> results)
        {
            List<Item> resultsFiltered = new List<Item>(results.Count);

            // 2. Filter by parent existance.
            resultsFiltered.AddRange(results.Where(r => xmlItem.ParentNode != null && r.Parent != null));

            return resultsFiltered;
        }

        private static List<Item> FindItemWithParentNameFilter(XmlNode xmlItem, List<Item> results)
        {
            List<Item> resultsFiltered = new List<Item>(results.Count);

            // 3. Filter by parent-name.
            resultsFiltered.AddRange(results.Where(r => xmlItem.ParentNode != null && xmlItem.ParentNode.Name == r.Parent.Name));

            return resultsFiltered;
        }

        private static List<Item> FindItemWithParentCustomNameFilter(XmlNode xmlItem, List<Item> results)
        {
            List<Item> resultsFiltered = new List<Item>(results.Count);

            // 4. Filter by parent's attribute (custom) name.
            resultsFiltered.AddRange(results.Where(r => xmlItem.ParentNode != null &&
                xmlItem.ParentNode.Attributes != null && xmlItem.ParentNode.Attributes["name"] != null &&
                xmlItem.ParentNode.Attributes["name"].Value.Trim() == r.Parent.NewName));

            return resultsFiltered;
        }
        #endregion

    }
}
