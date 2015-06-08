using System;
using System.Linq;
using System.Windows.Forms;

namespace bark_GUI
{
    static class Program
    {
        //forms
        static public ViewerForm FormMain;
        static public FormDiagram FormDiagram1;
        static public PreferencesForm FormPref;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FormMain = args.Count() == 1 ? new ViewerForm(args[0]) : new ViewerForm();
            Application.Run(FormMain);
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
