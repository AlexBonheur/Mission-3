using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace gsb_m3
{
    public partial class gerer : Form
    {
        private gsbrapportsEntities entities;
        public gerer(gsbrapportsEntities entities)
        {
            InitializeComponent();
            this.entities = entities;
            this.bddonnesMed.DataSource = entities.medecins.ToList();
        }

        private int getNumMed()
        {
            var reqDernier = (from el in this.entities.medecins
                              orderby el.id descending
                              select el);
            medecin dernierMed = reqDernier.First();
            int n = dernierMed.id + 1;
            return n;
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            this.id.Text = this.getNumMed().ToString();
        }

        private medecin newMedecin()
        {

            medecin newMedecin1 = new medecin();
            newMedecin1.nom = nom.Text;
            newMedecin1.prenom = prenom.Text;
            newMedecin1.adresse = adr.Text;
            newMedecin1.tel = tel.Text;
            newMedecin1.specialiteComplementaire = spe.Text;
            newMedecin1.departement = Convert.ToInt32(dep.Text);
            return newMedecin1;

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.bddonnesMed.EndEdit();

            int medecinId;
            if (int.TryParse(id.Text, out medecinId))
            {
                var existingMedecin = this.entities.medecins.Find(medecinId);

                if (existingMedecin != null)
                {
                    // Mise a jour
                    existingMedecin.nom = nom.Text;
                    existingMedecin.prenom = prenom.Text;
                    existingMedecin.adresse = adr.Text;
                    existingMedecin.tel = tel.Text;
                    existingMedecin.specialiteComplementaire = spe.Text;
                    existingMedecin.departement = Convert.ToInt32(dep.Text);
                }
                else
                {
                    //  nouveau médecin
                    var sql = @"INSERT INTO medecin (id,nom, prenom, adresse, tel, specialiteComplementaire, departement) 
                        VALUES (@id, @nom, @prenom, @adresse, @tel, @specialiteComplementaire, @departement)";

                    this.entities.Database.ExecuteSqlCommand(sql,
                        new SqlParameter("@id", Convert.ToInt32(id.Text)),
                        new SqlParameter("@nom", nom.Text),
                        new SqlParameter("@prenom", prenom.Text),
                        new SqlParameter("@adresse", adr.Text),
                        new SqlParameter("@tel", tel.Text),
                        new SqlParameter("@specialiteComplementaire", spe.Text),
                        new SqlParameter("@departement", Convert.ToInt32(dep.Text)));
                }

                try
                {
                    this.entities.SaveChanges();
                    MessageBox.Show("L'opération s'est terminée avec succès.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Une erreur s'est produite lors de la mise à jour de la base de données : " + ex.Message);
                    if (ex.InnerException != null)
                    {
                        MessageBox.Show("Détails de l'erreur interne : " + ex.InnerException.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("ID fourni n'est pas valide");
            }


        }
    
    }
}
