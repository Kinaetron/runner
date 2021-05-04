using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EndersEditor
{
    public partial class Associate_Number : Form
    {
        public bool OKNumPressed = false;

        public Associate_Number()
        {
            InitializeComponent();
        }

        private void OK_Button_Click(object sender, EventArgs e)
        {
            OKNumPressed = true;
            Close();
        }

        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            OKNumPressed = false;
            Close();
        }
    }
}
