using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using VideoSystemWeb.BLL;
using VideoSystemWeb.Entity;
using VideoSystemWeb.DAL;
namespace VideoSystemWeb.Anagrafiche.userControl
{
    public partial class AnagCollaboratori : System.Web.UI.UserControl
    {
        //public static string constr = ConfigurationManager.ConnectionStrings["constrMSSQL_NIC"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                
                BasePage p = new BasePage();
                Esito esito = p.caricaListeTipologiche();

                if (string.IsNullOrEmpty(esito.descrizione)) {
                    ddlQualifiche.Items.Clear();
                    ddlQualifiche.Items.Add("");
                    foreach (Tipologica qualifica in p.listaQualifiche)
                    {
                        ListItem item = new ListItem();
                        item.Text = qualifica.nome;
                        // metto comunque il nome e non l'id perchè la ricerca sulla tabella anag_qualifiche_collaboratori la faccio sul nome
                        item.Value = qualifica.nome;
                        ddlQualifiche.Items.Add(item);
                    }
                }
                else
                {
                    Session["ErrorPageText"] = esito.descrizione;
                    string url = String.Format("~/pageError.aspx");
                    Response.Redirect(url, true);
                }
            }
        }

        protected void lbRicercaCollaboratori_Click(object sender, EventArgs e)
        {

            string queryRicerca = ConfigurationManager.AppSettings["QUERY_SEARCH_COLLABORATORI"];

            queryRicerca = queryRicerca.Replace("@cognome", tbCognome.Text.Trim().Replace("'", "''"));
            //queryRicerca = queryRicerca.Replace("@nome", tbNome.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@codiceFiscale", tbCF.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@comuneRiferimento", tbCitta.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@partitaIva", TbPiva.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@nomeSocieta", TbSocieta.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@qualifica", ddlQualifiche.SelectedValue.ToString().Trim().Replace("'","''"));
            queryRicerca = queryRicerca.Replace("@nome", tbNome.Text.Trim().Replace("'", "''"));
            Esito esito = new Esito();
            DataTable dtCollaboratori = Base_DAL.getDatiBySql(queryRicerca,ref esito);
            gv_collaboratori.DataSource = dtCollaboratori;
            gv_collaboratori.DataBind();
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