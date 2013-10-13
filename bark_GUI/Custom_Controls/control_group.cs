using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace bark_GUI.Custom_Controls
{
    public partial class control_group : custom_control
    {
        public control_group(string name, bool isRequired, General_Control generalControl)
        {
            InitializeComponent();
            labelGroup.Text = name;
            this.isRequired = isRequired;
            this.generalControl = generalControl;
        }
    }
}
