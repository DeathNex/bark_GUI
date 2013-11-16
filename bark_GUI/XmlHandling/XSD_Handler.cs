using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using bark_GUI.Structure;
using bark_GUI.Structure.ElementType;
using bark_GUI.Structure.ItemTypes;
using bark_GUI.Structure.Items;

namespace bark_GUI.XmlHandling
{
    class XsdHandler
    {
        /* VARIABLES */
        public XmlDocument XsdMain;
        private XmlDocument _xsdComplexTypes;
        private XmlDocument _xsdFunctions;
        private XmlDocument _xsdUnits;
        private XmlDocument _xsdUtility;












        /* PUBLIC METHODS */









        /// <summary> Using utility methods loads a full structure using the Main XSD Validator. </summary>
        /// <param name="pathXsd"> The Main XSD Validator's path. </param>
        /// <returns> If there were no errors, all the files were loaded successfuly. </returns>
        public bool Load(string pathXsd)
        {
            Structure.Structure.InitializeTypes();

            try
            {
                //Load the main XSDValidator
                XsdMain = new XmlDocument();
                XsdMain.Load(pathXsd);

                //Load the XSDValidator parts in order
                var parts = _getXsdPartPathsOf(XsdMain, pathXsd);
                //Load Utility
                foreach (var path in parts)
                    if (_getFileNameOf(path).Contains("Utility"))
                    {
                        if (!_LoadXsdUtility(path)) return false;
                    }
                //Load Units
                foreach (var path in parts)
                    if (_getFileNameOf(path).Contains("Units"))
                    {
                        if (!_LoadXsdUnits(path)) return false;
                    }
                //Load ComplexTypes
                foreach (var path in parts)
                    if (_getFileNameOf(path).Contains("ComplexTypes"))
                    {
                        if (!_LoadXsdComplexTypes(path)) return false;
                    }
                //Load Functions
                foreach (var path in parts)
                    if (_getFileNameOf(path).Contains("Functions"))
                    {
                        if (!_LoadXsdFunctions(path)) return false;
                    }
                //Load Main
                if (!_LoadXsdMain()) return false;
            }
            catch
            {
                return false;
            }
            return true;
        }












        /* PRIVATE METHODS */










        /// <summary> Utility method that creates the structure of the Main XSD Validator. </summary>
        /// <returns> If there were no errors, success. </returns>
        private bool _LoadXsdMain()
        {
            try
            {
                Debug.Assert(XsdMain.DocumentElement != null, "XsdMain.DocumentElement != null");
                foreach (XmlNode xNode in XsdMain.DocumentElement.ChildNodes)
                    if (xNode.LocalName == "element")
                    {
                        Structure.Structure.SetRoot(new GroupItem(xNode));
                        break;
                    }
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary> Utility method that creates the structure of Functions. </summary>
        /// <returns> If there were no errors, success. </returns>
        private bool _LoadXsdFunctions(string filepath)
        {
            try
            {
                _xsdFunctions = new XmlDocument();
                _xsdFunctions.Load(filepath);
                Debug.Assert(_xsdFunctions.DocumentElement != null, "_xsdFunctions.DocumentElement != null");
                foreach (XmlNode xNode in _xsdFunctions.DocumentElement.ChildNodes)
                    if (xNode.LocalName == "element")
                        Structure.Structure.Add(new GroupItem(xNode, true));
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary> Utility method that creates the structure of ComplexTypes. </summary>
        /// <returns> If there were no errors, success. </returns>
        private bool _LoadXsdComplexTypes(string filepath)
        {
            try
            {
                _xsdComplexTypes = new XmlDocument();
                _xsdComplexTypes.Load(filepath);
                Debug.Assert(_xsdComplexTypes.DocumentElement != null, "_xsdComplexTypes.DocumentElement != null");
                foreach (XmlNode xNode in _xsdComplexTypes.DocumentElement.ChildNodes)
                    if (xNode.LocalName == "complexType")
                        _createComplexType(xNode);
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary> Utility method that creates the structure of Units. </summary>
        /// <returns> If there were no errors, success. </returns>
        private bool _LoadXsdUnits(string filepath)
        {
            try
            {
                _xsdUnits = new XmlDocument();
                _xsdUnits.Load(filepath);
                Debug.Assert(_xsdUnits.DocumentElement != null, "_xsdUnits.DocumentElement != null");
                foreach (XmlNode xNode in _xsdUnits.DocumentElement.ChildNodes)
                    if (xNode.LocalName == "simpleType")
                        _createUnit(xNode);
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary> Utility method that creates the structure of SimpleTypes. </summary>
        /// <returns> If there were no errors, success. </returns>
        private bool _LoadXsdUtility(string filepath)
        {
            try
            {
                _xsdUtility = new XmlDocument();
                _xsdUtility.Load(filepath);
                Debug.Assert(_xsdUtility.DocumentElement != null, "_xsdUtility.DocumentElement != null");
                foreach (XmlNode xNode in _xsdUtility.DocumentElement.ChildNodes)
                    if (xNode.LocalName == "simpleType")
                    {
                        _createSimpleType(xNode);
                    }
            }
            catch
            {
                return false;
            }
            return true;
        }















        /* UTILITY METHODS */

















        /// <summary> Using the Main XSD draws the smaller XSD paths that are included. </summary>
        /// <param name="xsd"> The XmlDocument Main XSD that contains all the other xsd parts. </param>
        /// <param name="xsdPath"> The path of the Main XSD. </param>
        /// <returns> List of smaller XSD paths that are included in the Main XSD. </returns>
        private List<string> _getXsdPartPathsOf(XmlDocument xsd, string xsdPath)
        {
            List<string> paths = new List<string>();
            string currentDirectory = _getDirectoryOf(xsdPath);
            Debug.Assert(xsd.DocumentElement != null, "xsd.DocumentElement != null");
            foreach (XmlNode xNode in xsd.DocumentElement.ChildNodes)
                if (xNode.LocalName == "include")
                {
                    Debug.Assert(xNode.Attributes != null, "xNode.Attributes != null");
                    paths.Add(currentDirectory + '\\' + xNode.Attributes.GetNamedItem("schemaLocation").Value);
                }
            return paths;
        }
        /// <summary> Utility method that returns the directory of a given path. </summary>
        private string _getDirectoryOf(string filepath) { return filepath.Remove(filepath.LastIndexOf('\\')); }
        /// <summary> Utility method that returns the filename of a given path. </summary>
        private string _getFileNameOf(string filePath) { return filePath.Substring(filePath.LastIndexOf('\\') + 1); }
















        /// <summary> Using an XmlNode creates a Unit using the appropriate info. </summary>
        /// <param name="xNode"> The XmlNode that was spotted to be a Unit. </param>
        private static void _createUnit(XmlNode xNode)
        {
            //Avoid unexpected situations
            if (!xNode.HasChildNodes)
                return;


            //Information Structure & Draw Info
            //Name
            var name = XsdParser.GetName(xNode);
            //Enumeration
            var options = XsdParser.DrawUnits(xNode);
            

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
            if (!xNode.HasChildNodes)
                return;

            //Draw information
            if (xNode.FirstChild.LocalName == "restriction")
            {
                if (xNode.FirstChild.FirstChild.LocalName == "enumeration")
                {
                    List<string> options = new List<string>();
                    foreach (XmlNode option in xNode.FirstChild.ChildNodes)
                        if (option.LocalName == "enumeration")
                        {
                            Debug.Assert(option.Attributes != null, "option.Attributes != null");
                            options.Add(option.Attributes.GetNamedItem("value").Value.Trim());
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
                    var xsValue = xNode.FirstChild.Attributes.GetNamedItem("base").Value;
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
                                max = double.Parse(maxMin.Attributes.GetNamedItem("value").Value.Trim());
                                break;
                            case "minInclusive":
                                min = double.Parse(maxMin.Attributes.GetNamedItem("value").Value.Trim());
                                break;
                            case "maxExclusive":
                                max = (double.Parse(maxMin.Attributes.GetNamedItem("value").Value.Trim())) - 1;
                                break;
                            case "minExclusive":
                                min = (double.Parse(maxMin.Attributes.GetNamedItem("value").Value.Trim())) + 1;
                                break;
                        }
                    }

                    //Finish
                    Structure.Structure.Add(new SimpleType(name, basicType, min, max));
                }
            }
            else if (xNode.FirstChild.LocalName == "list")
            {
                BasicType basicType = BasicType.String;
                Restriction restriction = Restriction.Basic;
                Debug.Assert(xNode.FirstChild.Attributes != null, "xNode.FirstChild.Attributes != null");
                string xsValue = xNode.FirstChild.Attributes.GetNamedItem("itemType").Value;
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
                if (x.LocalName == "choice")
                {
                    foreach (XmlNode xc in x.ChildNodes)
                    {
                        if (xc.LocalName != "element") continue;
                        string defaultValue;
                        Debug.Assert(xc.Attributes != null, "xc.Attributes != null");
                        switch (xc.Attributes.GetNamedItem("name").Value.Trim())
                        {
                            case "constant":
                                sType = Structure.Structure.FindSimpleType(xc.Attributes.GetNamedItem("type").Value.Trim());
                                defaultValue = xc.Attributes.GetNamedItem("default") != null ?
                                                   xc.Attributes.GetNamedItem("default").Value.Trim() : "";
                                constant = new ElementConstant(sType, defaultValue);
                                break;
                            case "variable":
                                sType = Structure.Structure.FindSimpleType(xc.Attributes.GetNamedItem("type").Value.Trim());
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
                                                functionNames.Add(xcf.Attributes.GetNamedItem("ref").Value.Trim());
                                            }
                                        function = new ElementFunction(functionNames);
                                    }
                                break;
                            case "keyword":
                                sType = Structure.Structure.FindSimpleType(xc.Attributes.GetNamedItem("type").Value.Trim());
                                defaultValue = xc.Attributes.GetNamedItem("default") != null ?
                                                   xc.Attributes.GetNamedItem("default").Value.Trim() : "";
                                keyword = new ElementKeyword(sType, defaultValue);
                                break;
                        }
                    }
                }
                //Get Units & Default values
                else
                {
                    Debug.Assert(x.Attributes != null, "x.Attributes != null");
                    if (x.LocalName == "attribute" && x.Attributes.GetNamedItem("name").Value.Trim() == "unit")
                    {
                        unit = Structure.Structure.FindUnit(x.Attributes.GetNamedItem("type").Value.Trim());
                        defaultUnit = x.Attributes.GetNamedItem("default").Value.Trim();
                    }
                    else if (x.LocalName == "attribute" && x.Attributes.GetNamedItem("name").Value.Trim() == "x_unit")
                    {
                        if (variable == null) continue;
                        var xUnit = Structure.Structure.FindUnit(x.Attributes.GetNamedItem("type").Value.Trim());
                        var defaultXUnit = x.Attributes.GetNamedItem("default").Value.Trim();
                        variable.SetX_Unit(xUnit, defaultXUnit);
                    }
                        //Handle References
                    else if (x.LocalName == "attribute" && x.Attributes.GetNamedItem("name").Value.Trim() == "reference")
                    {
                        sType = Structure.Structure.FindSimpleType(x.Attributes.GetNamedItem("type").Value.Trim());
                        reference = new ElementReference(sType);
                    }
                }
            }

            //Finish
            Structure.Structure.Add(new ComplexType(name, unit, defaultUnit, constant, variable, function, keyword, reference));
        }
    }
}
