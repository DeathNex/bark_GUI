using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace bark_GUI
{
    static class Program
    {
        //forms
        static public viewerForm formMain;
        static public FormDiagram formDiagram;
        static public preferencesForm formPref;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Count() == 1)
                formMain = new viewerForm(args[0]);
            else
                formMain = new viewerForm();
            Application.Run(formMain);
        }

        static void Initialization()
        {
            //Load Preferences
            //Report if any errors on loading

            //Load Default Project if preferences allow it
            //
        }
    }
}
