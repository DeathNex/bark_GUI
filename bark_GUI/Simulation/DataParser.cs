using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace bark_GUI.Simulation
{
    public static class DataParser
    {
        public static string[] ReadData(string filepath)
        {
            string[] lines;

            // Checks
            if (string.IsNullOrEmpty(filepath))
            {
                MessageBox.Show("No path specified for the .dat file.");
                return null;
            }
            if (!File.Exists(filepath))
            {
                MessageBox.Show("The given path is incorrect for the .dat file.\n File does not exist.");
                return null;
            }

            // Read file.
            lines = File.ReadAllLines(filepath);

            // Check
            if (lines.Length < 0)
            {
                MessageBox.Show("No data found in the .dat file.");
                return null;
            }

            // Ignore Empty Lines
            var x = lines.Select(line => line.Trim()).Where(l => l.Trim().Length > 0).ToArray();
            return x;
        }
    }
}
