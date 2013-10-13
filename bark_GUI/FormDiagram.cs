using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
            this.Dispose();
        }
    }
}
