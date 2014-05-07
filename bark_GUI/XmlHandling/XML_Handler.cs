#region using
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Windows.Forms;
using System.IO;
using bark_GUI.Properties;
using bark_GUI.Structure.Items;

#endregion

namespace bark_GUI.XmlHandling
{
    public class XmlHandler
    {
        private List<string> _errors;

        private XmlDocument _xmlDocument;

        private XElement _lastSavedXml;

        private readonly XsdHandler _xsdHandler;


        #region Constructor
        public XmlHandler()
        {
            _xsdHandler = new XsdHandler();
        }
        #endregion


        #region Public Methods


        public bool New(string xsdFilepath)
        {
            //Check if the file exists before loading it
            if (string.IsNullOrEmpty(xsdFilepath) || !File.Exists(xsdFilepath))
            {
                MessageBox.Show("Error!!!\nXSD file not found.\n" + xsdFilepath);
                return false;
            }

            if (!_xsdHandler.Load(xsdFilepath))
                throw new Exception("Could not load XSD files.");


            // Find the root item from our built Structure to create it for data representation.
            Structure.Structure.DataRootItem =
                (GroupItem)(Structure.Structure.StructureRoot).DuplicateStructure();

            return true;
        }


        #region Load Files
        public bool Load() { return Load(Settings.Default.PathCurrentFile); }

        /// <summary> Loads the XML file in the treeView. </summary>
        public bool Load(string pathXml)
        {
            //Check if the file exists before loading it
            if (!File.Exists(pathXml))
            {
                MessageBox.Show("Error!!!\nFile not found.");
                Settings.Default.PathCurrentFile = null;
                Settings.Default.MenuRecentFiles.Remove(pathXml);
                return false;
            }

            try
            {
                // Validate the XML against the XSD file.
                if (!_ValidateXml(pathXml))
                    throw new Exception("Could not validate XML file.");

                // Load the XML superfically just to make sure it can be loaded & to get the XSD paths.
                if (!_LoadXml(pathXml))
                    throw new Exception("Could not load XML file.");

                // Load the XSD & build the basic Structure.
                if (!_xsdHandler.Load(_getXsdPathOf(_xmlDocument, pathXml)))
                    throw new Exception("Could not load XSD files.");

                // Find the root item from our built Structure to create it for data representation.
                Structure.Structure.DataRootItem =
                    (GroupItem)Structure.Structure.CreateItem(_xmlDocument.DocumentElement);

                // Draw all the information from the XML file
                XmlParser.DrawInfo(_xmlDocument.DocumentElement);

                _lastSavedXml = XmlParser.ConvertToXml(Structure.Structure.DataRootItem);
            }
            catch (XmlException xmlEx)
            {
                MessageBox.Show(xmlEx.Message);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            return true;
        }
        #endregion

        #region Save Files
        public bool Save(string filepath)
        {
            // Save on temporary file and validate it before saving.
            var tmpFile = Settings.Default.PathSamples + "\\tmp.brk";

            var tmpLastSavedXml = _lastSavedXml;
            Save(tmpFile);
            _lastSavedXml = tmpLastSavedXml;

            if (!_ValidateXml(tmpFile))
            {
                File.Delete(tmpFile);
                return false;
            }
            File.Delete(tmpFile);


            // Check for empty path.
            if (string.IsNullOrEmpty(filepath)) return false;

            // Modify root element to meet the required conditions for XSD Validation.
            var rootElement = XmlParser.ConvertToXml(Structure.Structure.DataRootItem);
            XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
            rootElement.Add(new XAttribute(XNamespace.Xmlns + "xsi", xsi));
            rootElement.Add(new XAttribute(xsi+"noNamespaceSchemaLocation", Settings.Default.XSDValidatorName));

            // Write the current XML Document to file.
            try
            {
                using (var xWriter = XmlWriter.Create(filepath, new XmlWriterSettings
                { Indent = true, IndentChars = "\t" }))
                {
                    rootElement.Save(xWriter);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Could not overwrite file." + "\n" + e.Message);
            }

            // Keep the last saved XML Document for 'Dirty file' comparison.
            _lastSavedXml = XmlParser.ConvertToXml(Structure.Structure.DataRootItem);

            return true;
        }

        public bool HasDirtyFiles()
        {
            var currentXml = XmlParser.ConvertToXml(Structure.Structure.DataRootItem);

            if (currentXml == null || _lastSavedXml == null) return false;

            if (currentXml.ToString() != _lastSavedXml.ToString())
                return true;

            return false;

        }
        #endregion

        /// <summary> Removes any link to previous files. </summary>
        public void Clear() { _xmlDocument = null; }
        #endregion

        #region Private Methods
        private bool _LoadXml(string filepath)
        {
            try
            {
                _xmlDocument = new XmlDocument();
                _xmlDocument.Load(filepath);
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion

        #region Utility Methods
        private string _clearZeros(string s)
        {
            while (s.EndsWith("\n0 0"))
            {
                s = s.Remove(s.Length - 4);
            }
            while (s.StartsWith("0 0\n0 0\n"))
            {
                s = s.Remove(0, 8);
            }
            return s;
        }

        private bool _isValueElement(string value)
        {
            if (value.Trim() == "constant" || value.Trim() == "variable")
                return true;
            return false;
        }

        private bool _isValueElement(TreeNode t) { return _isValueElement(t.Name); }

        private bool _isValueElement(XmlNode x) { return _isValueElement(x.Name); }

        private string _getXsdPathOf(XmlDocument xml, string xmlPath)
        {
            var pathXsd = _getDirectoryOf(xmlPath);
            try
            {
                Debug.Assert(xml.DocumentElement != null, "xml.DocumentElement != null");
                pathXsd += '\\' + xml.DocumentElement.Attributes["xsi:noNamespaceSchemaLocation"].Value;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n\nMake sure the XML file includes the XSD Validator.");
            }
            return pathXsd;
        }

        private string _getDirectoryOf(string filepath) { return filepath.Remove(filepath.LastIndexOf('\\')); }

        private string _getFileNameOf(string filePath) { return filePath.Substring(filePath.LastIndexOf('\\') + 1); }
        #endregion

        #region XML Validation Methods
        /// <summary> Validates an XML file against the XSD schema.
        /// The XSD schema's path is included in the XML file.</summary>
        /// <param name="filePath">The XML file's path.</param>
        private bool _ValidateXml(string filePath)
        {
            // Set the validation settings.
            XmlReaderSettings settings = new XmlReaderSettings();
            try
            {
                using (XmlReader reader = XmlReader.Create(filePath, settings))
                    while (reader.Read())
                        if (reader.LocalName == "include")
                            settings.Schemas.Add(null, reader);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }

            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += _ValidationCallBack;

            // Parse the file.
            try
            {
                using (var reader = XmlReader.Create(filePath, settings))
                    while (reader.Read()) { }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }

            //Handle if any gathered errors
            return _HandleErrors("XML Validation");
        }

        // Gather any warnings or errors upon the XML Validation
        private void _ValidationCallBack(object sender, ValidationEventArgs args)
        {
            if (_errors == null)
                _errors = new List<string>();
            if (args.Severity != XmlSeverityType.Warning)
                _errors.Add(String.Format("Error {0}:\n{1}\n\n", _errors.Count + 1, args.Message));
            //_errors.Add(String.Format("Warning {0}:\n{1}\n\n", _errors.Count + 1, args.Message));
            //else


        }

        /// <summary> Creates a Log of the errors that occured during an action and shows them to the user. </summary>
        /// <param name="action">On which action is the error refering to. (e.g. XML Validation)</param>
        private bool _HandleErrors(string action)
        {
            //Check if the errors list is empty
            if (_errors == null || _errors.Count < 1) return true;

            //Show a message box indicating the number of errors occured on that action.
            MessageBox.Show(_errors.Count + " Errors occured upon " + action);

            //Create the error log's name
            string errorPath = String.Format("{0}{1}_{2}_error.log", Settings.Default.PathErrorLog, DateTime.Now.ToString("yyyy-MM-dd"), action);

            //Create the error Log
            using (StreamWriter sw = new StreamWriter(new FileStream(errorPath, FileMode.Create)))
            {
                foreach (string error in _errors)
                    foreach (var c in error)
                        if (c == '\n')  //The linebreaks are not handled correctly by the streamwriter
                            sw.WriteLine();
                        else
                            sw.Write(c);
            }

            //Show the created error Log
            Process.Start(errorPath);

            //Clear errors list
            _errors.Clear();
            _errors.TrimExcess();
            _errors = null;
            return false;
        }
        #endregion

    }
}
