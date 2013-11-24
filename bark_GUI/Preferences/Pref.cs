using System.Collections.Generic;
using System.IO;
using System;

namespace bark_GUI.Preferences
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
            public static string BarkExe = @"C:\Program Files (x86)\bark\0.5\bin";
            public static string CurrentFile = @"";

            internal static List<string> _ToSave()
            {
                return new List<string>
                    {
                        "Samples=\"" + Samples + "\"",
                        "Materials=\"" + Materials + "\"",
                        "ErrorLog=\"" + ErrorLog + "\"",
                        "BarkExe=\"" + BarkExe + "\""
                    };
            }
        }


        /* PRIVATE VARIABLES */


        //Private variable assissting in load function
        private enum NowReading { None, Path, Recent };










        /* UTILITY PUBLIC METHODS */








        /// <summary>
        /// The preferences are saved in a certain pattern. The pattern is:
        /// $ListName \r\n Variable="Value" \r\n Variable="Value" \r\n ...
        /// </summary>
        public static void Save()
        {
            var paths = Path._ToSave();

            using (FileStream fsWriter = new FileStream(Path.SavePref, FileMode.Create))
            {
                StreamWriter sw = new StreamWriter(fsWriter);
                sw.WriteLine("$PATHS");
                foreach (var s in paths)
                    sw.WriteLine(s);
                sw.WriteLine("$RECENT LIST");
                foreach (var s in Recent)
                    sw.WriteLine("Recent=\"" + s + "\"");
            }
        }

        /// <summary>
        /// The preferences are saved in a certain pattern. The pattern is:
        /// $ListName \r\n Variable="Value" \r\n Variable="Value" \r\n ...
        /// </summary>
        public static bool Load()
        {
            var nowReading = NowReading.None;

            try
            {
                using (FileStream fsLoader = new FileStream(Path.SavePref, FileMode.Open))
                {
                    var sr = new StreamReader(fsLoader);
                    var line = sr.ReadLine();
                    while (line != null)
                    {
                        //Check if the line is a ListName and find out which one
                        if (line.StartsWith("$"))
                            switch (line)
                            {
                                case "$PATHS":
                                    nowReading = NowReading.Path;
                                    break;
                                case "$RECENT LIST":
                                    nowReading = NowReading.Recent;
                                    break;
                            }
                        //Check Which variable name is this lane and input the correct value
                        else
                        {
                            if (nowReading == NowReading.Path)
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
                            else if (nowReading == NowReading.Recent)
                            {
                                if (line.StartsWith("Recent"))
                                    Recent.Add(_GetValue(line));
                            }
                        }
                        line = sr.ReadLine();
                    }
                }
            }
            catch (FileNotFoundException)
            {
                return false;
            }
            catch (Exception e)
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
