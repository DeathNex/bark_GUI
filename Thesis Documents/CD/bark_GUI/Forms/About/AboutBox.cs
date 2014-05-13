using System;
using System.Reflection;
using System.Windows.Forms;

namespace bark_GUI
{
    sealed partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();

            var barkGuiText = "";
            var barkText = "";

            barkGuiText += AssemblyProduct + "\n\n";
            barkGuiText += String.Format("Version: {0}\n\n", AssemblyVersion.Substring(0, AssemblyVersion.LastIndexOf('.')));
            barkGuiText += "Γραφικό Περιβάλλον (Bark_GUI). Πτυχιακή εργασία Γιάννη Χαντζή, υπό την επίβλεψη του καθηγητή Όμηρου Ιατρέλλη.\n\n";
            barkGuiText += "Τμήμα Μηχανικών Πληροφορικής Τ.Ε.\n\n";
            barkGuiText += "Επικοινωνία: JohnChantz@gmail.com\n\n";

            barkText += "bark (Solver)\n\n";
            barkText += "Λογισμικό Προσομοίωσης Φαινομένων Μετάδοσης Θερμότητας\n\n";
            barkText += "Τμήμα Μηχανολογίας - Μηχανικών Τ.Ε.\n\n";
            barkText += "Επικοινωνία: Εργαστήριο Μετάδοσης Θερμότητας, www.heatlab.teilar.gr\n";
            barkText += "Υπεύθυνος επικοινωνίας. Ονούφριος Χαραλάμπους onoufrios@teilar.gr";

            var copyright = "Copyright 2014 TEI Θεσσαλίας";

            labelGUI.Text = barkGuiText;
            labelBark.Text = barkText;
            labelCopyright.Text = copyright;
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        private void okButton_Click(object sender, EventArgs e)
        {
            Dispose();
        }
    }
}
