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
            Text = String.Format("About {0}", AssemblyTitle);
            labelProductName.Text = AssemblyProduct;
            labelVersion.Text = String.Format("Version {0}", AssemblyVersion);
            labelCopyright.Text = AssemblyCopyright;
            labelCompanyName.Text = AssemblyCompany;

            var description = "";
            description += "-------------------\n";
            description += "Bark .....\n";
            description += "\n";
            description += "Λογισμικό Προσομοίωσης Φαινομένων Μετάδοσης Θερμότητας\n";
            description += "-------------------\n";
            description += "*             Μονοδιάστατος Επιλυτής Φαινομένων Μετάδοσης Θερμότητας\n";
            description += "\n";
            description += "TEI Λάρισας, Τμήμα Μηχανολογίας, Εργαστήριο Μετάδοσης Θερμότητας, www.heatlab.teilar.gr\n";
            description += "-------------------\n";
            description += "·         Γραφικό Περιβάλλον (bark_gui). Πτυχιακή εργασία Γιάννη Χαντζή, υπό την επίβλεψη του καθηγητή Όμηρου Ιατρέλλη.\n";
            description += "\n";
            description += "\n";
            description += "ΤΕΙ Λάρισας, Τμήμα Πληροφορικής και Τηλεπικοινωνιών.\n";
            description += "-------------------\n";
            description += "Copyright  ... TEI Λάρισας\n";
            description += "\n";
            description += "Υπεύθυνος επικοινωνίας. Ονούφριος Χαραλάμπους onoufrios@teilar.gr\n";

            labelDescription.Text = description;
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
