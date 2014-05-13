using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using bark_GUI.Structure.ItemTypes;
using bark_GUI.Structure.Items;
using bark_GUI.XmlHandling;

namespace bark_GUI.Structure
{
    static class Structure
    {
        // Public Variables
        public static GroupItem DataRootItem { get; set; }

        /* PRIVATE VARIABLES */
        public static GroupItem StructureRoot { get; private set; }

        private static List<Item> _items;

        private static List<ElementItem> _elementItems;

        private static List<GroupItem> _groupItems;

        private static List<Unit> _units;

        private static List<ItemType> _types;

        private static List<SimpleType> _simpleTypes;

        private static List<ComplexType> _complexTypes;

        private static Dictionary<string, List<Item>> _referenceLists;

        private static Dictionary<string, GroupItem> _functions;

        #region Public Initialization Methods

        public static void InitializeTypes()
        {
            // Initialize collection objects.
            _units = new List<Unit>();
            _types = new List<ItemType>();
            _simpleTypes = new List<SimpleType>();
            _complexTypes = new List<ComplexType>();
            _referenceLists = new Dictionary<string, List<Item>>();
            _functions = new Dictionary<string, GroupItem>();
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
            StructureRoot = newRoot;

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
                    _functions.Add(item.Name, (GroupItem)item);
                }
            }
        }

        public static void AddReferenceList(string name) { _referenceLists[name] = new List<Item>(); }

        public static void AddReference(Item item) { if (_referenceLists.ContainsKey(item.Name)) _referenceLists[item.Name].Add(item); }
        #endregion

        #region Find
        // These methods are used during the Info Population on the already created Structure. (When loading the XML)

        public static Unit FindUnit(string unitName) { return _units.FirstOrDefault(u => u.Name == unitName); }

        public static SimpleType FindSimpleType(string typeName) { return _simpleTypes.FirstOrDefault(t => t.Name == typeName); }

        public static ComplexType FindComplexType(string typeName) { return _complexTypes.FirstOrDefault(t => t.Name == typeName); }

        // Returns a list of references on that element name as a string for controls to show.
        public static List<string> FindReferenceListOptions(string name)
        {
            return !_referenceLists.ContainsKey(name) ? null : _referenceLists[name].Select(item => item.NewName ?? item.Name).ToList();
        }

        // Returns a list of references on that element name as a string for controls to show.
        public static GroupItem FindFunction(string name)
        {
            // Check
            if (!_functions.ContainsKey(name)) return null;

            return _functions[name];
        }

        // Items require special treatment because duplicates exist.
        // Returns the 'Data' item that was created after XML.
        public static Item FindDataItem(XmlNode xmlItem)
        {
            const string errorMsg = "Structure - FindDataItem:\n - ";
            Item result = null;

            // Searched null?
            if (xmlItem == null || xmlItem.NodeType == XmlNodeType.Document) return null;

            // Searched is DataRootItem?
            if (xmlItem.Name == DataRootItem.Name) return DataRootItem;

            var results = DataRootItem.InnerChildren.Where(item => item.Name == xmlItem.Name).ToList();

            Debug.Assert(results.Count > 0, "No matches found for item '" + xmlItem.Name +
                "' in the DataRootItem.\n     Please make sure the names are correct.");

            // Handle Multiple GroupItems.
            if (xmlItem.Attributes != null && xmlItem.Attributes["name"] != null)
            {
                // Check if the multiple item already exists. Unique exception (not C# error) is the case
                // where the item in the results is multiple but has no newName value. That means that it
                // doesn't exist yet.
                return results.FirstOrDefault(item => item.IsGroupItem && (item.NewName == xmlItem.Attributes["name"].Value || item.NewName == null));
            }
            // If multiple results found, use filters to further indentify the correct result.
            result = results.Count == 1 ? results[0] : FindItemWithFilters(xmlItem, results);

            // Check for error Filters were not sufficient or no results were found.
            Debug.Assert(result != null, errorMsg + "Filters were incapable of distinguishing a single item '"
                    + xmlItem.Name + "' in the DataRootItem.\n     Please make sure no exact duplicates exist.");

            return result;
        }

        #endregion

        #region Create

        // Returns a clone of the 'Structure' item that was loaded from XSD.
        public static Item CreateItem(XmlNode xmlItem)
        {
            var item = FindStructureItem(xmlItem);
            Debug.Assert(item != null, "Find Structure Item failed in CreateItem.");
            var parentResult = FindDataItem(xmlItem.ParentNode);
            var parent = parentResult != null ? (GroupItem)parentResult : null;
            var newItem = item.DuplicateStructure();

            // Check parent's existance.
            if (parent == null && xmlItem.ParentNode != null && xmlItem.ParentNode.NodeType != XmlNodeType.Document)
                throw new Exception("Could not create Item 'cause no XmlNode.Parent was found.");


            // Add this item to parent's children
            if (parent != null)
            {
                var position = parent.Children.IndexOf(item);

                // Find the correct position for this item.
                // (to keep the order of the XML and not the XSD + extra of the XML at the end)
                // If this step is skipped every subsequent multiple item will be appended in the end of the Structure and
                // the Viewer(s).

                if (position >= 0)
                {
                    do
                    {
                        position++;
                    } while (position < parent.Children.Count && parent.Children[position].Name == item.Name);
                }

                // If no such child already exists, just place it, else insert it.
                if (position >= 0 && position < parent.Children.Count) parent.Children.Insert(position, newItem);
                else parent.Children.Add(newItem);
            }


            newItem.SetParent(parent);

            return newItem;
        }

        // Overload converting XElement to XmlElement.
        public static Item CreateItem(XElement xElement)
        {
            XmlDocument xD = new XmlDocument();
            xD.LoadXml(xElement.ToString());

            return CreateItem(xD.FirstChild);
        }

        public static ComplexType CreateComplexType(string typeName)
        {
            var complexType = FindComplexType(typeName);

            Debug.Assert(complexType != null, "FindComplexType failed!");

            var newComplexType = complexType.DuplicateStructure();

            return newComplexType;
        }

        #endregion

        public static void RemoveReference(Item item) { if (_referenceLists.ContainsKey(item.Name)) _referenceLists[item.Name].Remove(item); }


        #endregion


        // Items require special treatment because duplicates exist.
        // Returns the 'Structure' item that was loaded from XSD.
        internal static Item FindStructureItem(XmlNode xmlItem)
        {
            const string errorMsg = "Structure - FindStructureItem:\n - ";
            Item result = null;

            if (xmlItem == null) return null;

            if (xmlItem.Name == StructureRoot.Name) return StructureRoot;

            var isElementItem = XmlParser.IsElementItem(xmlItem);

            var results = _items.Where(i => (i.Name == xmlItem.Name) && (i.IsElementItem == isElementItem)).ToList();

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
            if (filteredResults != null && filteredResults.Count == 1) return filteredResults[0];

            // 2. Check parent existance.
            filteredResults = FindItemWithParentExistanceFilter(risingXmlItem, results);
            if (filteredResults != null && filteredResults.Count == 1) return filteredResults[0];

            // If no parent exists, the search cannot continue.
            if (filteredResults != null && filteredResults.Count <= 0) return null;
            // Continue the search with elements that have a parent.
            results = filteredResults;

            // 3. Check parent name.
            filteredResults = FindItemWithParentNameFilter(risingXmlItem, results);
            if (filteredResults != null && filteredResults.Count == 1) return filteredResults[0];

            // 4. Check parent custom name.
            filteredResults = FindItemWithParentCustomNameFilter(risingXmlItem, results);

            if (filteredResults != null && filteredResults.Count == 1) return filteredResults[0];

            // Rise in parent.
            var newXmlItem = xmlItem.ParentNode;
            var newResults = results.Select(t => t.Parent).Distinct().Cast<Item>().ToList();

            Debug.Assert(newResults.Count > 0, "Filters were incapable of distinguishing a single item '"
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
