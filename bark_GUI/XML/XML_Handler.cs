using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Windows.Forms;
using System.IO;

namespace bark_GUI
{
    public class XML_Handler
    {
        /* PRIVATE VARIABLES */
        private List<string> _errors;
        private XmlDocument _XmlDocument;
        private XmlDocument _LastSavedXmlDocument;
        private XSD_Handler _XSD_Handler;



        //Constructor
        public XML_Handler()
        {
            _XSD_Handler = new XSD_Handler();
        }






        /* PUBLIC METHODS */











        public bool Load() { return Load(Pref.Path.CurrentFile); }
        /// <summary> Loads the XML file in the treeView. </summary>
        public bool Load(string pathXML)
        {
            //Check if the file exists before loading it
            if (!File.Exists(pathXML))
            {
                MessageBox.Show("Error!!!\nFile not found.");
                Pref.Path.CurrentFile = null;
                Pref.Recent.Remove(pathXML);
                return false;
            }

            try
            {
                //Validate the XML against the XSD file
                if (!_ValidateXML(pathXML))
                    throw new Exception("Could not validate XML file.");

                //Load the XML superfically just to make sure it can be loaded & to get the XSD paths
                if (!_LoadXml(pathXML))
                    throw new Exception("Could not load XML file.");

                //Load the XSD & build the basic Structure
                if (!_XSD_Handler.Load(_getXsdPathOf(_XmlDocument, pathXML)))
                    throw new Exception("Could not load XSD files.");

                //Draw all the information from the XML file
                XML_Parser.DrawInfo(_XmlDocument.DocumentElement);
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
                _XmlDocument = new XmlDocument();
                _XmlDocument.Load(filepath);
                _LastSavedXmlDocument = (XmlDocument)_XmlDocument.Clone();
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
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;
            try
            {
                using (XmlWriter xWriter = XmlWriter.Create(filepath, xmlWriterSettings))
                {
                    _XmlDocument.Save(xWriter);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Could not overwrite file." + "\n" + e.Message);
            }
            _LastSavedXmlDocument = (XmlDocument)_XmlDocument.Clone();
        }

        public bool HasDirtyFiles()
        {
            return false;

            //Compare the data from tree with the _XmlDocument & if any action was made on any element
            if (_XmlDocument != null)
                return true;
        }
        #endregion

        /// <summary> Removes any link to previous files. </summary>
        public void Clear()
        {
            _XmlDocument = null;
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


        private string _getXsdPathOf(XmlDocument xml, string XMLpath)
        {
            string pathXSD = _getDirectoryOf(XMLpath);
            return pathXSD += '\\' + xml.DocumentElement.Attributes.GetNamedItem("xsi:noNamespaceSchemaLocation").Value;
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
            settings.ValidationEventHandler += new ValidationEventHandler(_ValidationCallBack);

            // Parse the file.
            try
            {
                using (XmlReader reader = XmlReader.Create(filePath, settings))
                    while (reader.Read()) ;
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
            MessageBox.Show(_errors.Count.ToString() + " Errors occured upon " + action);

            //Create the error log's name
            string errorPath = String.Format("{0}{1}_{2}_error.log", Pref.Path.ErrorLog, DateTime.Now.ToString("yyyy-MM-dd"), action);

            //Create the error Log
            using (FileStream fs = new FileStream(errorPath, FileMode.Create))
            {
                StreamWriter sw = new StreamWriter(fs);
                foreach (string error in _errors)
                    foreach (char c in error)
                        if (c == '\n')  //The linebreaks are not handled correctly by the streamwriter
                            sw.WriteLine();
                        else
                            sw.Write(c);
                sw.Close();
            }

            //Show the created error Log
            System.Diagnostics.Process.Start(errorPath);

            //Clear errors list
            _errors.Clear();
            _errors.TrimExcess();
            _errors = null;
            return false;
        }
        #endregion
    }
}
