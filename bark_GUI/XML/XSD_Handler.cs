using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace bark_GUI
{
    class XSD_Handler
    {
        /* VARIABLES */
        public XmlDocument _XsdMain;
        private XmlDocument _XsdComplexTypes;
        private XmlDocument _XsdFunctions;
        private XmlDocument _XsdUnits;
        private XmlDocument _XsdUtility;












        /* PUBLIC METHODS */









        /// <summary> Using utility methods loads a full structure using the Main XSD Validator. </summary>
        /// <param name="pathXSD"> The Main XSD Validator's path. </param>
        /// <returns> If there were no errors, all the files were loaded successfuly. </returns>
        public bool Load(string pathXSD)
        {
            Structure.InitializeTypes();
            List<string> parts;

            try
            {
                //Load the main XSDValidator
                _XsdMain = new XmlDocument();
                _XsdMain.Load(pathXSD);

                //Load the XSDValidator parts in order
                parts = _getXsdPartPathsOf(_XsdMain, pathXSD);
                //Load Utility
                foreach (string path in parts)
                    if (_getFileNameOf(path).Contains("Utility"))
                    {
                        if (!_LoadXsdUtility(path)) return false;
                    }
                //Load Units
                foreach (string path in parts)
                    if (_getFileNameOf(path).Contains("Units"))
                    {
                        if (!_LoadXsdUnits(path)) return false;
                    }
                //Load ComplexTypes
                foreach (string path in parts)
                    if (_getFileNameOf(path).Contains("ComplexTypes"))
                    {
                        if (!_LoadXsdComplexTypes(path)) return false;
                    }
                //Load Functions
                foreach (string path in parts)
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
                foreach (XmlNode xNode in _XsdMain.DocumentElement.ChildNodes)
                    if (xNode.LocalName == "element")
                    {
                        Structure.SetRoot(new GroupItem(xNode));
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
                _XsdFunctions = new XmlDocument();
                _XsdFunctions.Load(filepath);
                foreach (XmlNode xNode in _XsdFunctions.DocumentElement.ChildNodes)
                    if (xNode.LocalName == "element")
                        Structure.Add(new GroupItem(xNode, true));
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
                _XsdComplexTypes = new XmlDocument();
                _XsdComplexTypes.Load(filepath);
                foreach (XmlNode xNode in _XsdComplexTypes.DocumentElement.ChildNodes)
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
                _XsdUnits = new XmlDocument();
                _XsdUnits.Load(filepath);
                foreach (XmlNode xNode in _XsdUnits.DocumentElement.ChildNodes)
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
                _XsdUtility = new XmlDocument();
                _XsdUtility.Load(filepath);
                foreach (XmlNode xNode in _XsdUtility.DocumentElement.ChildNodes)
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
        /// <param name="XsdPath"> The path of the Main XSD. </param>
        /// <returns> List of smaller XSD paths that are included in the Main XSD. </returns>
        private List<string> _getXsdPartPathsOf(XmlDocument xsd, string XsdPath)
        {
            List<string> paths = new List<string>();
            string _currentDirectory = _getDirectoryOf(XsdPath);
            foreach (XmlNode xNode in xsd.DocumentElement.ChildNodes)
                if (xNode.LocalName == "include")
                {
                    paths.Add(_currentDirectory + '\\' + xNode.Attributes.GetNamedItem("schemaLocation").Value);
                }
            return paths;
        }
        /// <summary> Utility method that returns the directory of a given path. </summary>
        private string _getDirectoryOf(string filepath) { return filepath.Remove(filepath.LastIndexOf('\\')); }
        /// <summary> Utility method that returns the filename of a given path. </summary>
        private string _getFileNameOf(string filePath) { return filePath.Substring(filePath.LastIndexOf('\\') + 1); }
















        /// <summary> Using an XmlNode creates a Unit using the appropriate info. </summary>
        /// <param name="xNode"> The XmlNode that was spotted to be a Unit. </param>
        private void _createUnit(XmlNode xNode)
        {
            //Avoid unexpected situations
            if (!xNode.HasChildNodes)
                return;


            //Information Structure & Draw Info
            //Name
            string _name = XSD_Parser.GetName(xNode);
            //Enumeration
            List<string> _options = XSD_Parser.DrawUnits(xNode);
            

            //Finish
            Structure.Add(new Unit(_name, _options));
        }

        /// <summary> Using an XmlNode creates a SimpleType using the appropriate info. </summary>
        /// <param name="xNode"> The XmlNode that was spotted to be a SimpleType. </param>
        private void _createSimpleType(XmlNode xNode)
        {
            //Information Structure
            //Name
            string _name = XSD_Parser.GetName(xNode);
            List<string> _options;

            //Avoid unexpected situations
            if (!xNode.HasChildNodes)
                return;

            //Draw information
            if (xNode.FirstChild.LocalName == "restriction")
            {
                if (xNode.FirstChild.FirstChild.LocalName == "enumeration")
                {
                    _options = new List<string>();
                    foreach (XmlNode option in xNode.FirstChild.ChildNodes)
                        if (option.LocalName == "enumeration")
                            _options.Add(option.Attributes.GetNamedItem("value").Value.Trim());

                    //Finish
                    Structure.Add(new SimpleType(_name, _options));
                }
                else
                {
                    double max = double.MaxValue;
                    double min = double.MinValue;
                    BasicType basicType = BasicType._string;

                    string xsValue = xNode.FirstChild.Attributes.GetNamedItem("base").Value;
                    switch (xsValue)
                    {
                        case "xs:string":
                            basicType = BasicType._string;
                            break;
                        case "xs:integer":
                            basicType = BasicType._integer;
                            break;
                        case "xs:positiveInteger":
                            basicType = BasicType._integer;
                            min = 1;
                            break;
                        case "xs:nonPositiveInteger":
                            basicType = BasicType._integer;
                            max = 0;
                            break;
                        case "xs:nonNegativeInteger":
                            basicType = BasicType._integer;
                            min = 0;
                            break;
                        case "xs:negativeInteger":
                            basicType = BasicType._integer;
                            max = -1;
                            break;
                        case "xs:decimal":
                            basicType = BasicType._decimal;
                            break;
                        case "decimal_positive":
                            basicType = BasicType._decimal;
                            min = 0;
                            break;
                        default:
                            break;
                    }
                    foreach (XmlNode maxMin in xNode.FirstChild.ChildNodes)
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

                    //Finish
                    Structure.Add(new SimpleType(_name, basicType, min, max));
                }
            }
            else if (xNode.FirstChild.LocalName == "list")
            {
                BasicType basicType = BasicType._string;
                Restriction restriction = Restriction.basic;
                string xsValue = xNode.FirstChild.Attributes.GetNamedItem("itemType").Value;
                switch (xsValue)
                {
                    case "xs:string":
                        basicType = BasicType._string;
                        break;
                    case "xs:integer":
                        basicType = BasicType._integer;
                        break;
                    case "xs:positiveInteger":
                        basicType = BasicType._integer;
                        restriction = Restriction.maxMin;
                        //Finish
                        Structure.Add(new SimpleType(_name, basicType, 1, double.MaxValue));
                        break;
                    case "xs:nonPositiveInteger":
                        basicType = BasicType._integer;
                        restriction = Restriction.maxMin;
                        //Finish
                        Structure.Add(new SimpleType(_name, basicType, double.MinValue, 0));
                        break;
                    case "xs:nonNegativeInteger":
                        basicType = BasicType._integer;
                        restriction = Restriction.maxMin;
                        //Finish
                        Structure.Add(new SimpleType(_name, basicType, 0, double.MaxValue));
                        break;
                    case "xs:negativeInteger":
                        basicType = BasicType._integer;
                        restriction = Restriction.maxMin;
                        //Finish
                        Structure.Add(new SimpleType(_name, basicType, double.MinValue, -1));
                        break;
                    case "xs:decimal":
                        basicType = BasicType._decimal;
                        break;
                    case "decimal_positive":
                        basicType = BasicType._decimal;
                        restriction = Restriction.maxMin;
                        //Finish
                        Structure.Add(new SimpleType(_name, basicType, 0, double.MaxValue));
                        break;
                    default:
                        break;
                }
                //Finish Alternative
                if (restriction == Restriction.basic)
                    Structure.Add(new SimpleType(_name, basicType));
            }
        }

        /// <summary> Using an XmlNode creates a ComplexType using the appropriate info. </summary>
        /// <param name="xNode"> The XmlNode that was spotted to be a ComplexType. </param>
        private void _createComplexType(XmlNode xNode)
        {
            //Information Structure
            //Name
            string _name = XSD_Parser.GetName(xNode);
            Unit unit = null;
            string defaultUnit = "";
            SimpleType sType;
            string defaultValue;
            List<string> _functionNames = null;
            ElementConstant _constant = null;
            ElementVariable _variable = null;
            ElementFunction _function = null;
            ElementKeyword _keyword = null;
            ElementReference _reference = null;

            //Avoid unexpected situations
            if (!xNode.HasChildNodes)
                return;

            //Draw information
            foreach (XmlNode x in xNode.ChildNodes)
            {
                if (x.LocalName == "choice")
                {
                    foreach (XmlNode xc in x.ChildNodes)
                    {
                        if (xc.LocalName == "element")
                        {
                            switch (xc.Attributes.GetNamedItem("name").Value.Trim())
                            {
                                case "constant":
                                    sType = Structure.FindSimpleType(xc.Attributes.GetNamedItem("type").Value.Trim());
                                    if (xc.Attributes.GetNamedItem("default") != null)
                                        defaultValue = xc.Attributes.GetNamedItem("default").Value.Trim();
                                    else
                                        defaultValue = "";
                                    _constant = new ElementConstant(sType, defaultValue);
                                    break;
                                case "variable":
                                    sType = Structure.FindSimpleType(xc.Attributes.GetNamedItem("type").Value.Trim());
                                    _variable = new ElementVariable(sType);
                                    break;
                                case "function":
                                    if (xc.HasChildNodes && xc.FirstChild.HasChildNodes && xc.FirstChild.FirstChild.HasChildNodes)
                                        if (xc.FirstChild.FirstChild.LocalName == "choice")
                                        {
                                            _functionNames = new List<string>();
                                            foreach (XmlNode xcf in xc.FirstChild.FirstChild.ChildNodes)
                                                if (xcf.LocalName == "element")
                                                    _functionNames.Add(xcf.Attributes.GetNamedItem("ref").Value.Trim());
                                            _function = new ElementFunction(_functionNames);
                                        }
                                    break;
                                case "keyword":
                                    sType = Structure.FindSimpleType(xc.Attributes.GetNamedItem("type").Value.Trim());
                                    if (xc.Attributes.GetNamedItem("default") != null)
                                        defaultValue = xc.Attributes.GetNamedItem("default").Value.Trim();
                                    else
                                        defaultValue = "";
                                    _keyword = new ElementKeyword(sType, defaultValue);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                //Get Units & Default values
                else if (x.LocalName == "attribute" && x.Attributes.GetNamedItem("name").Value.Trim() == "unit")
                {
                    unit = Structure.FindUnit(x.Attributes.GetNamedItem("type").Value.Trim());
                    defaultUnit = x.Attributes.GetNamedItem("default").Value.Trim();
                }
                else if (x.LocalName == "attribute" && x.Attributes.GetNamedItem("name").Value.Trim() == "x_unit")
                {
                    if (_variable != null)
                    {
                        Unit x_unit = Structure.FindUnit(x.Attributes.GetNamedItem("type").Value.Trim());
                        string defaultX_Unit = x.Attributes.GetNamedItem("default").Value.Trim();
                        _variable.SetX_Unit(x_unit, defaultX_Unit);
                    }
                    else { }
                }
                //Handle References
                else if (x.LocalName == "attribute" && x.Attributes.GetNamedItem("name").Value.Trim() == "reference")
                {
                    sType = Structure.FindSimpleType(x.Attributes.GetNamedItem("type").Value.Trim());
                    _reference = new ElementReference(sType);
                }
            }

            //Finish
            Structure.Add(new ComplexType(_name, unit, defaultUnit, _constant, _variable, _function, _keyword, _reference));
        }
    }
}
