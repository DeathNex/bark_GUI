#region using
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using bark_GUI.Structure;
using bark_GUI.Structure.ElementType;
using bark_GUI.Structure.ItemTypes;
using bark_GUI.Structure.Items;
#endregion

namespace bark_GUI.XmlHandling
{
    class XsdHandler
    {
        public XmlDocument XsdMain;

        private XmlDocument _xsdComplexTypes;

        private XmlDocument _xsdFunctions;

        private XmlDocument _xsdUnits;

        private XmlDocument _xsdUtility;

        readonly XmlReaderSettings _settings;


        #region Constructor
        public XsdHandler()
        {
            _settings = new XmlReaderSettings { IgnoreComments = true };
        }
        #endregion


        #region Public Methods
        /// <summary> Using utility methods loads a full structure using the Main XSD Validator. </summary>
        /// <param name="pathXsd"> The Main XSD Validator's path. </param>
        /// <returns> If there were no errors, all the files were loaded successfuly. </returns>
        public bool Load(string pathXsd)
        {
            XsdMain = new XmlDocument();

            List<string> partsFilepath;

            Structure.Structure.InitializeTypes();

            try
            {
                // Load the main XSDValidator to get it's dependencies (parts).
                using (var reader = XmlReader.Create(pathXsd, _settings))
                {
                    XsdMain.Load(reader);
                }

                // Load filepaths of xsd parts.
                partsFilepath = GetXsdPartPathsOf(XsdMain, pathXsd);

                // Load dynamically the xsd part files that contains the appropriate word.
                // The order is important due to dependencies.
                if (!partsFilepath.Where(path => GetFileNameOf(path).Contains("Utility")).Any(_LoadXsdUtility))
                    return false;
                if (!partsFilepath.Where(path => GetFileNameOf(path).Contains("Units")).Any(_LoadXsdUnits))
                    return false;
                if (!partsFilepath.Where(path => GetFileNameOf(path).Contains("ComplexTypes")).Any(_LoadXsdComplexTypes))
                    return false;
                if (!partsFilepath.Where(path => GetFileNameOf(path).Contains("Functions")).Any(_LoadXsdFunctions))
                    return false;
                
                // Load Main.
                if (!LoadXsdMain()) return false;
            }
            catch (XmlException e)
            {
                Debug.Fail(e.Message, e.StackTrace);
            }
            catch (Exception e)
            {
                Debug.Fail(e.Message, e.StackTrace);
            }

            return true;
        }
        #endregion

        #region Private Loading XSD Methods

        /// <summary> Utility method that creates the structure of the Main XSD Validator. </summary>
        /// <returns> If there were no errors, success. </returns>
        private bool LoadXsdMain()
        {
            Debug.Assert(XsdMain.DocumentElement != null, "XsdMain.DocumentElement != null");

            try
            {
                // Find the root node (first element of the document).
                foreach (XmlNode xNode in XsdMain.DocumentElement.ChildNodes)
                {
                    if (xNode.LocalName != "element")
                        continue;

                    Structure.Structure.SetRoot(new GroupItem(xNode));
                    break;
                }
            }
            catch (XmlException e)
            {
                Debug.Fail(e.Message, e.StackTrace);
            }
            catch (Exception e)
            {
                Debug.Fail(e.Message, e.StackTrace);
            }

            return true;
        }

        /// <summary> Utility method that creates the structure of Functions. </summary>
        /// <returns> If there were no errors, success. </returns>
        private bool _LoadXsdFunctions(string filepath)
        {
            _xsdFunctions = new XmlDocument();

            try
            {
                using (var reader = XmlReader.Create(filepath, _settings))
                {
                    // Load document.
                    _xsdFunctions.Load(reader);
                    Debug.Assert(_xsdFunctions.DocumentElement != null, "_xsdFunctions.DocumentElement != null");

                    // Load elements.
                    foreach (XmlNode xNode in _xsdFunctions.DocumentElement.ChildNodes)
                        if (xNode.LocalName == "element")
                            Structure.Structure.Add(new GroupItem(xNode, true));
                }
            }
            catch (XmlException e)
            {
                Debug.Fail(e.Message, e.StackTrace);
            }
            catch (Exception e)
            {
                Debug.Fail(e.Message, e.StackTrace);
            }

            return true;
        }

        /// <summary> Utility method that creates the structure of ComplexTypes. </summary>
        /// <returns> If there were no errors, success. </returns>
        private bool _LoadXsdComplexTypes(string filepath)
        {
            _xsdComplexTypes = new XmlDocument();

            try
            {
                using (var reader = XmlReader.Create(filepath, _settings))
                {
                    // Load document.
                    _xsdComplexTypes.Load(reader);
                    Debug.Assert(_xsdComplexTypes.DocumentElement != null, "_xsdComplexTypes.DocumentElement != null");

                    // Load elements.
                    foreach (XmlNode xNode in _xsdComplexTypes.DocumentElement.ChildNodes)
                        if (xNode.LocalName == "complexType")
                            _createComplexType(xNode);
                }
            }
            catch (XmlException e)
            {
                Debug.Fail(e.Message, e.StackTrace);
            }
            catch (Exception e)
            {
                Debug.Fail(e.Message, e.StackTrace);
            }

            return true;
        }

        /// <summary> Utility method that creates the structure of Units. </summary>
        /// <returns> If there were no errors, success. </returns>
        private bool _LoadXsdUnits(string filepath)
        {
            _xsdUnits = new XmlDocument();

            try
            {
                using (var reader = XmlReader.Create(filepath, _settings))
                {
                    // Load document.
                    _xsdUnits.Load(reader);
                    Debug.Assert(_xsdUnits.DocumentElement != null, "_xsdUnits.DocumentElement != null");

                    // Load elements.
                    foreach (XmlNode xNode in _xsdUnits.DocumentElement.ChildNodes)
                        if (xNode.LocalName == "simpleType")
                            _createUnit(xNode);
                }
            }
            catch (XmlException e)
            {
                Debug.Fail(e.Message, e.StackTrace);
            }
            catch (Exception e)
            {
                Debug.Fail(e.Message, e.StackTrace);
            }

            return true;
        }

        /// <summary> Utility method that creates the structure of SimpleTypes. </summary>
        /// <returns> If there were no errors, success. </returns>
        private bool _LoadXsdUtility(string filepath)
        {
            _xsdUtility = new XmlDocument();

            try
            {
                using (var reader = XmlReader.Create(filepath, _settings))
                {
                    // Load document.
                    _xsdUtility.Load(reader);
                    Debug.Assert(_xsdUtility.DocumentElement != null, "_xsdUtility.DocumentElement != null");

                    // Load elements.
                    foreach (XmlNode xNode in _xsdUtility.DocumentElement.ChildNodes)
                        if (xNode.LocalName == "simpleType")
                        {
                            _createSimpleType(xNode);
                        }
                }
            }
            catch (XmlException e)
            {
                Debug.Fail(e.Message, e.StackTrace);
            }
            catch (Exception e)
            {
                Debug.Fail(e.Message, e.StackTrace);
            }

            return true;
        }

        #endregion

        #region Utility File Methods
        /// <summary> Using the Main XSD draws the included smaller XSD paths. </summary>
        /// <param name="xsd"> The XmlDocument Main XSD that contains all the other xsd parts. </param>
        /// <param name="xsdPath"> The path of the Main XSD. </param>
        /// <returns> List of smaller XSD paths that are included in the Main XSD. </returns>
        private List<string> GetXsdPartPathsOf(XmlDocument xsd, string xsdPath)
        {
            List<string> paths = new List<string>();
            string currentDirectory = GetDirectoryOf(xsdPath);

            Debug.Assert(xsd.DocumentElement != null, "xsd.DocumentElement != null");

            foreach (XmlNode xNode in xsd.DocumentElement.ChildNodes)
                if (xNode.LocalName == "include")
                {
                    Debug.Assert(xNode.Attributes != null, "xNode.Attributes != null");
                    paths.Add(currentDirectory + '\\' + xNode.Attributes["schemaLocation"].Value);
                }

            return paths;
        }

        /// <summary> Utility method that returns the directory of a given path. </summary>
        private string GetDirectoryOf(string filepath) { return filepath.Remove(filepath.LastIndexOf('\\')); }

        /// <summary> Utility method that returns the filename of a given path. </summary>
        private string GetFileNameOf(string filePath) { return filePath.Substring(filePath.LastIndexOf('\\') + 1); }
        #endregion

        #region XSD Parser Methods
        /// <summary> Using an XmlNode creates a Unit using the appropriate info. </summary>
        /// <param name="xNode"> The XmlNode that was spotted to be a Unit. </param>
        private static void _createUnit(XmlNode xNode)
        {
            //Avoid unexpected situations
            if (!XsdParser.HasChildren(xNode))
                return;


            //Information Structure & Draw Info
            //Name
            var name = XsdParser.GetName(xNode);
            //Enumeration
            var options = XsdParser.GetUnitOptions(xNode);


            //Finish
            Structure.Structure.Add(new Unit(name, options));
        }

        /// <summary> Using an XmlNode creates a SimpleType using the appropriate info. </summary>
        /// <param name="xNode"> The XmlNode that was spotted to be a SimpleType. </param>
        private static void _createSimpleType(XmlNode xNode)
        {
            //Information Structure
            //Name
            var name = XsdParser.GetName(xNode);

            //Avoid unexpected situations
            if (!XsdParser.HasChildren(xNode))
                return;

            //Draw information
            if (xNode["xs:restriction"] != null)
            {
                if (xNode.FirstChild.FirstChild.LocalName == "enumeration")
                {
                    List<string> options = new List<string>();
                    foreach (XmlNode option in xNode.FirstChild.ChildNodes)
                        if (option.LocalName == "enumeration")
                        {
                            Debug.Assert(option.Attributes != null, "option.Attributes != null");
                            options.Add(option.Attributes["value"].Value.Trim());
                        }

                    //Finish
                    Structure.Structure.Add(new SimpleType(name, options));
                }
                else
                {
                    var max = double.MaxValue;
                    var min = double.MinValue;
                    BasicType basicType = BasicType.String;

                    Debug.Assert(xNode.FirstChild.Attributes != null, "xNode.FirstChild.Attributes != null");
                    var xsValue = xNode.FirstChild.Attributes["base"].Value;
                    switch (xsValue)
                    {
                        case "xs:string":
                            basicType = BasicType.String;
                            break;
                        case "xs:integer":
                            basicType = BasicType.Integer;
                            break;
                        case "xs:positiveInteger":
                            basicType = BasicType.Integer;
                            min = 1;
                            break;
                        case "xs:nonPositiveInteger":
                            basicType = BasicType.Integer;
                            max = 0;
                            break;
                        case "xs:nonNegativeInteger":
                            basicType = BasicType.Integer;
                            min = 0;
                            break;
                        case "xs:negativeInteger":
                            basicType = BasicType.Integer;
                            max = -1;
                            break;
                        case "xs:decimal":
                            basicType = BasicType.Decimal;
                            break;
                        case "decimal_positive":
                            basicType = BasicType.Decimal;
                            min = 0;
                            break;
                    }
                    foreach (XmlNode maxMin in xNode.FirstChild.ChildNodes)
                    {
                        Debug.Assert(maxMin.Attributes != null, "maxMin.Attributes != null");
                        switch (maxMin.LocalName)
                        {
                            case "maxInclusive":
                                max = double.Parse(maxMin.Attributes["value"].Value.Trim());
                                break;
                            case "minInclusive":
                                min = double.Parse(maxMin.Attributes["value"].Value.Trim());
                                break;
                            case "maxExclusive":
                                max = (double.Parse(maxMin.Attributes["value"].Value.Trim())) - 1;
                                break;
                            case "minExclusive":
                                min = (double.Parse(maxMin.Attributes["value"].Value.Trim())) + 1;
                                break;
                        }
                    }

                    //Finish
                    Structure.Structure.Add(new SimpleType(name, basicType, min, max));
                }
            }
            else if (xNode["list"] != null)
            {
                BasicType basicType = BasicType.String;
                Restriction restriction = Restriction.Basic;
                Debug.Assert(xNode["list"].HasAttributes, "xNode[list].Attributes != null");
                string xsValue = xNode["list"].Attributes["itemType"].Value;
                switch (xsValue)
                {
                    case "xs:string":
                        basicType = BasicType.String;
                        break;
                    case "xs:integer":
                        basicType = BasicType.Integer;
                        break;
                    case "xs:positiveInteger":
                        basicType = BasicType.Integer;
                        restriction = Restriction.MaxMin;
                        //Finish
                        Structure.Structure.Add(new SimpleType(name, basicType, 1, double.MaxValue));
                        break;
                    case "xs:nonPositiveInteger":
                        basicType = BasicType.Integer;
                        restriction = Restriction.MaxMin;
                        //Finish
                        Structure.Structure.Add(new SimpleType(name, basicType, double.MinValue, 0));
                        break;
                    case "xs:nonNegativeInteger":
                        basicType = BasicType.Integer;
                        restriction = Restriction.MaxMin;
                        //Finish
                        Structure.Structure.Add(new SimpleType(name, basicType, 0, double.MaxValue));
                        break;
                    case "xs:negativeInteger":
                        basicType = BasicType.Integer;
                        restriction = Restriction.MaxMin;
                        //Finish
                        Structure.Structure.Add(new SimpleType(name, basicType, double.MinValue, -1));
                        break;
                    case "xs:decimal":
                        basicType = BasicType.Decimal;
                        break;
                    case "decimal_positive":
                        basicType = BasicType.Decimal;
                        restriction = Restriction.MaxMin;
                        //Finish
                        Structure.Structure.Add(new SimpleType(name, basicType, 0, double.MaxValue));
                        break;
                }
                //Finish Alternative
                if (restriction == Restriction.Basic)
                    Structure.Structure.Add(new SimpleType(name, basicType));
            }
        }

        /// <summary> Using an XmlNode creates a ComplexType using the appropriate info. </summary>
        /// <param name="xNode"> The XmlNode that was spotted to be a ComplexType. </param>
        private static void _createComplexType(XmlNode xNode)
        {
            //Information Structure
            //Name
            string name = XsdParser.GetName(xNode);
            Unit unit = null;
            string defaultUnit = "";
            ElementConstant constant = null;
            ElementVariable variable = null;
            ElementFunction function = null;
            ElementKeyword keyword = null;
            ElementReference reference = null;

            //Avoid unexpected situations
            if (!xNode.HasChildNodes)
                return;

            //Draw information
            foreach (XmlNode x in xNode.ChildNodes)
            {
                SimpleType sType;

                // Ignore comments.
                if (x.NodeType == XmlNodeType.Comment)
                    continue;

                // Crete elements.
                if (x.LocalName == "choice")
                {
                    foreach (XmlNode xc in x.ChildNodes)
                    {
                        if (xc.LocalName != "element") continue;
                        string defaultValue;
                        Debug.Assert(xc.Attributes != null, "xc.Attributes != null");
                        switch (xc.Attributes["name"].Value.Trim())
                        {
                            case "constant":
                                sType = Structure.Structure.FindSimpleType(xc.Attributes["type"].Value.Trim());
                                defaultValue = xc.Attributes["default"] != null ?
                                                   xc.Attributes["default"].Value.Trim() : "";
                                constant = new ElementConstant(sType, defaultValue);
                                break;
                            case "variable":
                                sType = Structure.Structure.FindSimpleType(xc.Attributes["type"].Value.Trim());
                                variable = new ElementVariable(sType);
                                break;
                            case "function":
                                if (xc.HasChildNodes && xc.FirstChild.HasChildNodes && xc.FirstChild.FirstChild.HasChildNodes)
                                    if (xc.FirstChild.FirstChild.LocalName == "choice")
                                    {
                                        List<string> functionNames = new List<string>();
                                        foreach (XmlNode xcf in xc.FirstChild.FirstChild.ChildNodes)
                                            if (xcf.LocalName == "element")
                                            {
                                                Debug.Assert(xcf.Attributes != null, "xcf.Attributes != null");
                                                functionNames.Add(xcf.Attributes["ref"].Value.Trim());
                                            }
                                        function = new ElementFunction(functionNames);
                                    }
                                break;
                            case "keyword":
                                sType = Structure.Structure.FindSimpleType(xc.Attributes["type"].Value.Trim());
                                defaultValue = xc.Attributes["default"] != null ?
                                                   xc.Attributes["default"].Value.Trim() : "";
                                keyword = new ElementKeyword(sType, defaultValue);
                                break;
                        }
                    }
                }
                //Get Units & Default values
                else
                {
                    Debug.Assert(x.Attributes != null, "x.Attributes != null");
                    if (x.LocalName == "attribute" && x.Attributes["name"].Value.Trim() == "unit")
                    {
                        unit = Structure.Structure.FindUnit(x.Attributes["type"].Value.Trim());
                        defaultUnit = x.Attributes["default"].Value.Trim();
                    }
                    else if (x.LocalName == "attribute" && x.Attributes["name"].Value.Trim() == "x_unit")
                    {
                        if (variable == null) continue;
                        var xUnit = Structure.Structure.FindUnit(x.Attributes["type"].Value.Trim());
                        var defaultXUnit = x.Attributes["default"].Value.Trim();
                        variable.SetX_Unit(xUnit, defaultXUnit);
                    }
                    //Handle References
                    else if (x.LocalName == "attribute" && x.Attributes["name"].Value.Trim() == "reference")
                    {
                        sType = Structure.Structure.FindSimpleType(x.Attributes["type"].Value.Trim());
                        reference = new ElementReference(sType);
                    }
                }
            }

            //Finish
            Structure.Structure.Add(new ComplexType(name, unit, defaultUnit, constant, variable, function, keyword, reference));
        }
        #endregion
    }
}
