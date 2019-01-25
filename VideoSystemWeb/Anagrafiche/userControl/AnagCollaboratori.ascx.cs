using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;

namespace VideoSystemWeb.Anagrafiche.userControl
{
    public partial class AnagCollaboratori : System.Web.UI.UserControl
    {
        public static string constr = ConfigurationManager.ConnectionStrings["constrMSSQL_NIC"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lbRicercaCollaboratori_Click(object sender, EventArgs e)
        {
            //string constr = Session["connectionString"].ToString();

            using (System.Data.OleDb.OleDbConnection con = new System.Data.OleDb.OleDbConnection(constr))
            {
                using (System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand("SELECT Cognome,Nome,codicefiscale as CF,Nazione,comunenascita as [Comune Nascita],provinciaNascita as Prov,convert(varchar,datanascita,103) as [Data Nascita],comuneriferimento as [Comune Rif], nomesocieta as Societa, partitaiva as [P.Iva],convert(bit,assunto) as Assunto FROM anag_collaboratori where Cognome like '%" + tbCognome.Text.Trim() + "%'"))
                {
                    using (System.Data.OleDb.OleDbDataAdapter sda = new System.Data.OleDb.OleDbDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            gv_collaboratori.DataSource = dt;
                            gv_collaboratori.DataBind();
                        }
                    }
                }
            }
        }
        protected void ddlQualifiche_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void lbPulisciCampiRicerca_Click(object sender, EventArgs e)
        {
            tbCognome.Text = "";
            tbNome.Text = "";
            tbCF.Text = "";
            tbCitta.Text = "";
            TbPiva.Text = "";
            TbSocieta.Text = "";
            ddlQualifiche.SelectedIndex = 0;
        }
    }
}