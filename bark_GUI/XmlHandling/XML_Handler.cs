using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using System.Xml.Schema;
using System.Windows.Forms;
using System.IO;
using bark_GUI.Preferences;

namespace bark_GUI.XmlHandling
{
    public class XmlHandler
    {
        /* PRIVATE VARIABLES */
        private List<string> _errors;
        private XmlDocument _xmlDocument;
        private XmlDocument _lastSavedXmlDocument;
        private readonly XsdHandler _xsdHandler;



        //Constructor
        public XmlHandler()
        {
            _xsdHandler = new XsdHandler();
        }






        /* PUBLIC METHODS */











        public bool Load() { return Load(Pref.Path.CurrentFile); }
        /// <summary> Loads the XML file in the treeView. </summary>
        public bool Load(string pathXml)
        {
            //Check if the file exists before loading it
            if (!File.Exists(pathXml))
            {
                MessageBox.Show("Error!!!\nFile not found.");
                Pref.Path.CurrentFile = null;
                Pref.Recent.Remove(pathXml);
                return false;
            }

            try
            {
                //Validate the XML against the XSD file
                if (!_ValidateXML(pathXml))
                    throw new Exception("Could not validate XML file.");

                //Load the XML superfically just to make sure it can be loaded & to get the XSD paths
                if (!_LoadXml(pathXml))
                    throw new Exception("Could not load XML file.");

                //Load the XSD & build the basic Structure
                if (!_xsdHandler.Load(_getXsdPathOf(_xmlDocument, pathXml)))
                    throw new Exception("Could not load XSD files.");

                //Draw all the information from the XML file
                XmlParser.DrawInfo(_xmlDocument.DocumentElement);
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

        private bool _LoadXml(string filepath)
        {
            try
            {
                _xmlDocument = new XmlDocument();
                _xmlDocument.Load(filepath);
                _lastSavedXmlDocument = (XmlDocument)_xmlDocument.Clone();
            }
            catch
            {
                return false;
            }
            return true;
        }













        #region Save Files
        public void Save(string filepath)
        {
            if (filepath == string.Empty)
                return;
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings { Indent = true };
            try
            {
                using (var xWriter = XmlWriter.Create(filepath, xmlWriterSettings))
                {
                    _xmlDocument.Save(xWriter);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Could not overwrite file." + "\n" + e.Message);
            }
            _lastSavedXmlDocument = (XmlDocument)_xmlDocument.Clone();
        }

        public bool HasDirtyFiles()
        {
            Debug.WriteLine("Method HasDirtyFiles not Implemented!");

            return false;

            //Compare the data from tree with the _XmlDocument & if any action was made on any element

        }
        #endregion

        /// <summary> Removes any link to previous files. </summary>
        public void Clear()
        {
            _xmlDocument = null;
        }













        /* UTILITY PRIVATE METHODS */











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
            string pathXsd = _getDirectoryOf(xmlPath);
            try
            {
                Debug.Assert(xml.DocumentElement != null, "xml.DocumentElement != null");
                pathXsd += '\\' + xml.DocumentElement.Attributes.GetNamedItem("xsi:noNamespaceSchemaLocation").Value;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n\nMake sure the XML file includes the XSD Validator.");
            }
            return pathXsd;
        }
        private string _getDirectoryOf(string filepath) { return filepath.Remove(filepath.LastIndexOf('\\')); }
        private string _getFileNameOf(string filePath) { return filePath.Substring(filePath.LastIndexOf('\\') + 1); }









        /* VALIDATION PRIVATE METHODS */









        #region XML Validation
        /// <summary> Validates an XML file against the XSD schema.
        /// The XSD schema's path is included in the XML file.</summary>
        /// <param name="filePath">The XML file's path.</param>
        private bool _ValidateXML(string filePath)
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
            string errorPath = String.Format("{0}{1}_{2}_error.log", Pref.Path.ErrorLog, DateTime.Now.ToString("yyyy-MM-dd"), action);

            //Create the error Log
            using (FileStream fs = new FileStream(errorPath, FileMode.Create))
            {
                StreamWriter sw = new StreamWriter(fs);
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
