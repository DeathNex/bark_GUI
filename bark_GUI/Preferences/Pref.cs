using System.Collections.Generic;
using System.IO;
using System;

namespace bark_GUI
{
    static class Pref
    {
        /* PUBLIC VARIABLES */
        public static List<string> Recent = new List<string>();
        public struct Path
        {
            public static string MainDirectory = @Directory.GetCurrentDirectory();

            public static string Samples = @"Samples";
            public static string Materials = @"Materials";
            public static string ErrorLog = @"";
            public static string SavePref = @"prefs.txt";
            public static string BarkExe = @"C:\Program Files (x86)\bark\0.4\bin";
            public static string CurrentFile = @"";

            internal static List<string> _ToSave()
            {
                List<string> save = new List<string>();
                save.Add("Samples=\"" + Samples + "\"");
                save.Add("Materials=\"" + Materials + "\"");
                save.Add("ErrorLog=\"" + ErrorLog + "\"");
                save.Add("BarkExe=\"" + BarkExe + "\"");

                return save;
            }
        }


        /* PRIVATE VARIABLES */


        //Private variable assissting in load function
        private enum nowReading { None, Path, Recent };










        /* UTILITY PUBLIC METHODS */








        /// <summary>
        /// The preferences are saved in a certain pattern. The pattern is:
        /// $ListName \r\n Variable="Value" \r\n Variable="Value" \r\n ...
        /// </summary>
        public static void Save()
        {
            List<string> paths = Path._ToSave();

            using (FileStream fs = new FileStream(Pref.Path.SavePref, FileMode.Create))
            {
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine("$PATHS");
                foreach (string s in paths)
                    sw.WriteLine(s);
                sw.WriteLine("$RECENT LIST");
                foreach (string s in Recent)
                    sw.WriteLine("Recent=\"" + s + "\"");
                sw.Close();
            }
        }

        /// <summary>
        /// The preferences are saved in a certain pattern. The pattern is:
        /// $ListName \r\n Variable="Value" \r\n Variable="Value" \r\n ...
        /// </summary>
        public static bool Load()
        {
            string line = "";
            nowReading _nowReading = nowReading.None;

            try
            {
                using (FileStream fs = new FileStream(Pref.Path.SavePref, FileMode.Open))
                {
                    StreamReader sr = new StreamReader(fs);
                    line = sr.ReadLine();
                    while (line != null)
                    {
                        //Check if the line is a ListName and find out which one
                        if (line.StartsWith("$"))
                            switch (line)
                            {
                                case "$PATHS":
                                    _nowReading = nowReading.Path;
                                    break;
                                case "$RECENT LIST":
                                    _nowReading = nowReading.Recent;
                                    break;
                                default:
                                    break;
                            }
                        //Check Which variable name is this lane and input the correct value
                        else
                        {
                            if (_nowReading == nowReading.Path)
                            {
                                if (line.StartsWith("Samples"))
                                    Path.Samples = _GetValue(line);
                                else if (line.StartsWith("Materials"))
                                    Path.Materials = _GetValue(line);
                                else if (line.StartsWith("ErrorLog"))
                                    Path.ErrorLog = _GetValue(line);
                                else if (line.StartsWith("BarkExe"))
                                    Path.BarkExe = _GetValue(line);
                            }
                            else if (_nowReading == nowReading.Recent)
                            {
                                if (line.StartsWith("Recent"))
                                    Recent.Add(_GetValue(line));
                            }
                        }
                        line = sr.ReadLine();
                    }
                    sr.Close();
                }
            }
            catch (FileNotFoundException)
            {
                return false;
            }
            catch(Exception e)
            {
                throw e;
            }
            return true;
        }










        /* UTILITY PRIVATE METHODS */






        /// <summary> Gets the value inside quotation marks ("value"). </summary>
        /// <param name="line"> Input string of a line. </param>
        /// <returns> The value of that line's variable. </returns>
        private static string _GetValue(string line)
        {
            int start = 0;
            int length = 0;

            start = line.IndexOf('"') + 1;
            length = line.LastIndexOf('"') - start;
            return line.Substring(start, length);
        }
    }
}
