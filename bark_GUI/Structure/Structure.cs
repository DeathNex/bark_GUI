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

        private static Dictionary<string, List<Item>> _referenceLists;

        private static List<GroupItem> _functions;

        #region Public Initiliazation Methods

        public static void InitializeTypes()
        {
            // Initialize collection objects.
            _units = new List<Unit>();
            _types = new List<ItemType>();
            _simpleTypes = new List<SimpleType>();
            _complexTypes = new List<ComplexType>();
            _referenceLists = new Dictionary<string, List<Item>>();
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
        // These methods are used during the Creation of the Structure. (When loading the XSD)

        public static void Add(Unit unit) { _units.Add(unit); }

        public static void Add(SimpleType type) { _types.Add(type); _simpleTypes.Add(type); }

        public static void Add(ComplexType type) { _types.Add(type); _complexTypes.Add(type); }

        public static void Add(ElementItem eItem) { ElementItems.Add(eItem); Items.Add(eItem); }

        public static void Add(GroupItem gItem) { if (gItem.IsFunction) _functions.Add(gItem); else { GroupItems.Add(gItem); Items.Add(gItem); } }

        public static void AddReferenceList(string name) { _referenceLists[name] = new List<Item>(); }

        public static void AddReference(Item item){if (_referenceLists.ContainsKey(item.Name)) _referenceLists[item.Name].Add(item); }
        #endregion

        #region Find
        // These methods are used during the Info Population on the already created Structure. (When loading the XML)

        public static Unit FindUnit(string unitName) { return _units.FirstOrDefault(u => u.Name == unitName); }

        public static ItemType FindType(string typeName) { return _types.FirstOrDefault(t => t.Name == typeName); }

        public static SimpleType FindSimpleType(string typeName) { return _simpleTypes.FirstOrDefault(t => t.Name == typeName); }

        public static ComplexType FindComplexType(string typeName) { return _complexTypes.FirstOrDefault(t => t.Name == typeName); }

        // ElementItems require special treatment because duplicates exist.
        public static Item FindItem(XmlNode xmlItem)    // CHECK: Xml dependency. Can be removed?
        {
            const string errorMsg = "Structure - FindItem:\n - ";

            if (xmlItem.Name == Root.Name) return Root;

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

        // Returns a list of references on that element name as a string for controls to show.
        public static List<string> FindReferenceListOptions(string name)
        {
            return !_referenceLists.ContainsKey(name) ? null : _referenceLists[name].Select(item => item.NewName ?? item.Name).ToList();
        }

        #endregion

        #region Remove

        public static void Remove(Item item)
        {
            if (_referenceLists.ContainsKey(item.Name) && _referenceLists[item.Name].Contains(item))
                _referenceLists[item.Name].Remove(item);

            if (!item.IsFunction)
            {
                Items.Remove(item);

                if (item.IsElementItem)
                    ElementItems.Remove((ElementItem)item);
                else
                    GroupItems.Remove((GroupItem)item);
            }
        }

        #endregion

        #endregion


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
