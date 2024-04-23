using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gsb_m3
{
    public partial class Form1 : Form
    {
        private gsbrapportsEntities entities;
        public Form1()
        {
            InitializeComponent();
            entities = new gsbrapportsEntities();
        }

        private void dernierRapportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dRapport r = new dRapport(this.entities);
            r.MdiParent = this;
            r.Show();
        }

        private void gérerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            gerer g = new gerer(this.entities);
            g.MdiParent = this;
            g.Show();
        }

        private void rechercheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            liste l = new liste(this.entities);
            l.MdiParent = this; 
            l.Show();
        }
    }
}
