using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace EndersEditor
{
    public partial class ManageEntities : Form
    {

        ReadPoint point = new ReadPoint();

       public string spawnType;
       public string spawnName;

       public string spawnNumber;
 
        public ManageEntities()
        {
            InitializeComponent();

            
            point.ReadFile(point.PlayerFile);
            point.ReadFile(point.EnemyFile);
            point.ReadFile(point.MiscFile);

            listBox1.DataSource = new BindingSource(point.Players.Values, null);
            listBox2.DataSource = new BindingSource(point.Enemies.Values, null);
            listBox3.DataSource = new BindingSource(point.Miscellaneous.Values, null);

            listBox1.SelectedItem = null;
            listBox2.SelectedItem = null;
            listBox3.SelectedItem = null;
        }

       

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            spawnType = "Player";

            if (listBox1.SelectedItem != null)
            {
                listBox2.SelectedItem = null;
                listBox3.SelectedItem = null;

                foreach (KeyValuePair<string, string> value in point.Players)
                {
                    if (value.Value == listBox1.SelectedItem.ToString())
                    {
                        spawnName = value.Value;
                        spawnNumber = value.Key;
                    }
                }
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            spawnType = "Enemies";

            if (listBox2.SelectedItem != null)
            {
                listBox1.SelectedItem = null;
                listBox3.SelectedItem = null;

                foreach (KeyValuePair<string, string> value in point.Enemies)
                {
                    if (value.Value == listBox2.SelectedItem.ToString())
                    {
                        spawnName = value.Value;
                        spawnNumber = value.Key;
                    }
                }
            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            spawnType = "Miscellaneous";

            if (listBox3.SelectedItem != null)
            {
                listBox1.SelectedItem = null;
                listBox2.SelectedItem = null;

                foreach (KeyValuePair<string, string> value in point.Miscellaneous)
                {
                    if (value.Value == listBox3.SelectedItem.ToString())
                    {
                        spawnName = value.Value;
                        spawnNumber = value.Key;
                    }
                } 
            }
        }
    }
}
