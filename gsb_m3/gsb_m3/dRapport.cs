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
    public partial class dRapport : Form
    {
        private gsbrapportsEntities entities;

        public dRapport(gsbrapportsEntities entities)
        {
            InitializeComponent();
            this.entities = entities;
            var query = @"
            SELECT [r].[id]
                ,[r].[date]
                ,[r].[motif]
                ,[r].[bilan]
                ,[r].[idVisiteur]
                ,[r].[idMedecin]
            FROM [dbo].[rapport] AS [r]
            INNER JOIN [dbo].[medecin] AS [m] ON [r].[idMedecin] = [m].[id]
            INNER JOIN (
                SELECT [idMedecin], MAX([date]) AS max_date
                FROM [dbo].[rapport]
                GROUP BY [idMedecin]
            ) AS [max_rapport] ON [r].[idMedecin] = [max_rapport].[idMedecin] AND [r].[date] = [max_rapport].[max_date]
            ORDER BY [r].[idMedecin];";

            // Créez une liste de rapports en utilisant la requête SQL
            var dernierRapports = entities.Database.SqlQuery<rapport>(query).ToList();
            this.bdRapport.DataSource = dernierRapports;

            dataGridView1.CellFormatting += dataGridView1_CellFormatting;
        }

        private void filtre2()
        {
            var query = entities.Set<medecin>().AsQueryable();

            if(!string.IsNullOrWhiteSpace(nom.Text))
            {
                query = query.Where(m => m.nom == nom.Text);
            }

            if (!string.IsNullOrWhiteSpace(prenom.Text))
            {
                query = query.Where(m => m.nom.StartsWith(prenom.Text));
            }

            bdRapport.DataSource = query.ToList();
        }
        
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView1.Columns["nomMed"].Index)
            {
                // Récupérer l'ID du médecin depuis la colonne idMedecin
                int idMedecin = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["idMedecinDataGridViewTextBoxColumn"].Value);

                // Récupérer le nom du médecin associé à partir de l'ID du médecin
                string nomMedecin = GetNomMedecinById(idMedecin);

                // Affecter le nom du médecin à la cellule nomMed
                e.Value = nomMedecin;
            }
            else if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView1.Columns["prenomMed"].Index)
            {
                // Récupérer l'ID du médecin depuis la colonne idMedecin
                int idMedecin = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["idMedecinDataGridViewTextBoxColumn"].Value);

                // Récupérer le prénom du médecin associé à partir de l'ID du médecin
                string prenomMedecin = GetPrenomMedecinById(idMedecin);

                // Affecter le prénom du médecin à la cellule prenomMed
                e.Value = prenomMedecin;
            }
        }

        // Méthode pour récupérer le nom du médecin à partir de l'ID du médecin
        private string GetNomMedecinById(int idMedecin)
        {
            // Remplacer "entities" par votre objet DbContext
            var medecin = entities.medecins.FirstOrDefault(m => m.id == idMedecin);
            if (medecin != null)
            {
                return medecin.nom;
            }
            return string.Empty;
        }

        // Méthode pour récupérer le prénom du médecin à partir de l'ID du médecin
        private string GetPrenomMedecinById(int idMedecin)
        {
            // Remplacer "entities" par votre objet DbContext
            var medecin = entities.medecins.FirstOrDefault(m => m.id == idMedecin);
            if (medecin != null)
            {
                return medecin.prenom;
            }
            return string.Empty;
        }

        private void filtre()
        {
            var query = @"
        SELECT [r].[id]
            ,[r].[date]
            ,[r].[motif]
            ,[r].[bilan]
            ,[r].[idVisiteur]
            ,[r].[idMedecin]
        FROM [dbo].[rapport] AS [r]
        INNER JOIN [dbo].[medecin] AS [m] ON [r].[idMedecin] = [m].[id]
        INNER JOIN (
            SELECT [idMedecin], MAX([date]) AS max_date
            FROM [dbo].[rapport]
            GROUP BY [idMedecin]
        ) AS [max_rapport] ON [r].[idMedecin] = [max_rapport].[idMedecin] AND [r].[date] = [max_rapport].[max_date]
        WHERE [m].[nom] LIKE @nom AND [m].[prenom] LIKE @prenom
        ORDER BY [r].[idMedecin];";

            // Remplacez "%{0}%" par "%{0}%" pour autoriser la recherche de sous-chaînes
            var nomParam = string.IsNullOrWhiteSpace(nom.Text) ? "%" : $"%{nom.Text}%";
            var prenomParam = string.IsNullOrWhiteSpace(prenom.Text) ? "%" : $"%{prenom.Text}%";

            var dernierRapports = entities.Database.SqlQuery<rapport>(query, new SqlParameter("@nom", nomParam), new SqlParameter("@prenom", prenomParam)).ToList();

            bdRapport.DataSource = dernierRapports;
        }

        private void nom_TextChanged(object sender, EventArgs e)
        {
            filtre();
            dataGridView1.AutoResizeColumns();
        }

        private void prenom_TextChanged(object sender, EventArgs e)
        {
            filtre();
            dataGridView1.AutoResizeColumns();
        }
    }

}
