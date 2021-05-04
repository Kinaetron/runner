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
    public partial class newLayerForm : Form
    {
        public bool OKpressed = false;
        public bool CollisionCheck = false;
        
        public newLayerForm()
        {
            InitializeComponent();
        }
        private void okButton_Click(object sender, EventArgs e)
        {
            OKpressed = true;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            OKpressed = false;
            Close();
        }
    }
}
