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
    public partial class liste : Form
    {
        private gsbrapportsEntities entities;
        public liste(gsbrapportsEntities entities)
        {
            InitializeComponent();
            this.entities = entities;
            this.Med.DataSource = this.entities.medecins.ToList();
        }

        private void ApplyFilters()
        {
            var query = entities.Set<medecin>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(dep.Text) && int.TryParse(dep.Text, out int departmentId))
            {
                query = query.Where(m => m.departement == departmentId);
            }

            if (!string.IsNullOrWhiteSpace(Nom.Text))
            {
                query = query.Where(m => m.nom.StartsWith(Nom.Text));
            }

            Med.DataSource = query.ToList();
        }

        private string countMedic()
        {
            var query = (from m in entities.Set<medecin>()
                         where m.nom.StartsWith(Nom.Text)
                         select m.id).Count();
            string result = query.ToString();
            return result;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {


            ApplyFilters();
            dataGridView1.AutoResizeColumns();



        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
            dataGridView1.AutoResizeColumns();

        }

        private void Nom_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
            dataGridView1.AutoResizeColumns();
        }

        private void dep_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
            dataGridView1.AutoResizeColumns();
        }


    }
}
