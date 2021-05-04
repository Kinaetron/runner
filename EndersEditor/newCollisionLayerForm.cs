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
    public partial class newCollisionLayerForm : Form
    {
        public bool OKColpressed = false;

        public newCollisionLayerForm()
        {
            InitializeComponent();
        }

        private void OKpressed_Click(object sender, EventArgs e)
        {
            OKColpressed = true;
            Close();
        }

        private void cancelPressed_Click(object sender, EventArgs e)
        {
            OKColpressed = false;
            Close();
        }
    }
}
