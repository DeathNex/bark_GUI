using System.Windows.Forms;

namespace bark_GUI
{
    public partial class FormDiagram : Form
    {
        public FormDiagram()
        {
            InitializeComponent();
        }

        private void FormDiagram_FormClosed(object sender, FormClosedEventArgs e)
        {
            Dispose();
        }
    }
}
