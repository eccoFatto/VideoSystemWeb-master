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

                //Esito esito = p.caricaListeTipologiche();
                //if (string.IsNullOrEmpty(esito.descrizione)) {
                //    ddlQualifiche.Items.Clear();
                //    ddlQualifiche.Items.Add("");
                //    foreach (Tipologica qualifica in p.listaQualifiche)
                //    {
                //        ListItem item = new ListItem();
                //        item.Text = qualifica.nome;
                //        // metto comunque il nome e non l'id perchè la ricerca sulla tabella anag_qualifiche_collaboratori la faccio sul nome
                //        item.Value = qualifica.nome;
                //        ddlQualifiche.Items.Add(item);
                //    }
                //}
                //else
                //{
                //    Session["ErrorPageText"] = esito.descrizione;
                //    string url = String.Format("~/pageError.aspx");
                //    Response.Redirect(url, true);
                //}
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

        protected void gv_collaboratori_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // PRENDO L'ID DEL COLLABORATORE SELEZIONATO

                string idCollaboratoreSelezionato = e.Row.Cells[0].Text;

                foreach (TableCell item in e.Row.Cells)
                {
                    item.Attributes["onclick"] = "mostraCollaboratore('" + idCollaboratoreSelezionato + "');";
                }

                
            }
        }

        protected void EditCollaboratore_Click(object sender, EventArgs e)
        {
            string idCollaboratore = hf_idColl.Value;
            Esito esito = new Esito();
            Entity.Anag_Collaboratori collaboratore = Anag_Collaboratori_DAL.Instance.getCollaboratoreById(Convert.ToInt16(idCollaboratore),ref esito);
            if (esito.codice == 0) {
                pulisciCampiDettaglio();
                
                // RIEMPIO I CAMPI DEL DETTAGLIO COLLABORATORE
                tbMod_CF.Text = collaboratore.CodiceFiscale;
                tbMod_Cognome.Text = collaboratore.Cognome;
                tbMod_Nome.Text = collaboratore.Nome;
                tbMod_Nazione.Text = collaboratore.Nazione;
                tbMod_ComuneNascita.Text = collaboratore.ComuneNascita;
                tbMod_ProvinciaNascita.Text = collaboratore.ProvinciaNascita;
                tbMod_ComuneRiferimento.Text = collaboratore.ComuneRiferimento;
                tbMod_DataNascita.Text = collaboratore.DataNascita.ToShortDateString();
                tbMod_NomeSocieta.Text = collaboratore.NomeSocieta;
                tbMod_PartitaIva.Text = collaboratore.PartitaIva;
                cbMod_Assunto.Checked = collaboratore.Assunto;
                cbMod_Attivo.Checked = collaboratore.Attivo;
                tbMod_Note.Text = collaboratore.Note;

                // QUALIFICHE
                foreach (Anag_Qualifiche_Collaboratori qualifica in collaboratore.Qualifiche)
                {
                    ListItem itemQualifica = new ListItem(qualifica.Qualifica, qualifica.Id.ToString());
                    lbMod_Qualifiche.Items.Add(itemQualifica);
                }
                if (collaboratore.Qualifiche.Count > 0)
                {
                    lbMod_Qualifiche.Rows = collaboratore.Qualifiche.Count;
                }
                else
                {
                    lbMod_Qualifiche.Rows = 1;
                }

                // INDIRIZZI
                foreach (Anag_Indirizzi_Collaboratori indirizzo in collaboratore.Indirizzi)
                {
                    ListItem itemIndirizzi = new ListItem(indirizzo.Descrizione + " - " + indirizzo.Tipo + " " + indirizzo.Indirizzo + " " + indirizzo.NumeroCivico + " " + indirizzo.Cap + " " + indirizzo.Comune + " " + indirizzo.Provincia, indirizzo.Id.ToString());
                    lbMod_Indirizzi.Items.Add(itemIndirizzi);
                }
                if (collaboratore.Indirizzi.Count > 0)
                {
                    lbMod_Indirizzi.Rows = collaboratore.Indirizzi.Count;
                }
                else
                {
                    lbMod_Indirizzi.Rows = 1;
                }

                // EMAIL
                foreach (Anag_Email_Collaboratori email in collaboratore.Email)
                {
                    ListItem itemEmail = new ListItem(email.Descrizione + " - " + email.IndirizzoEmail, email.Id.ToString());
                    lbMod_Email.Items.Add(itemEmail);
                }
                if (collaboratore.Email.Count > 0)
                {
                    lbMod_Email.Rows = collaboratore.Email.Count;
                }
                else
                {
                    lbMod_Email.Rows = 1;
                }

                // TELEFONI
                foreach (Anag_Telefoni_Collaboratori telefono in collaboratore.Telefoni)
                {
                    ListItem itemTelefono = new ListItem(telefono.Descrizione + " - " + telefono.Tipo + " - " + telefono.Pref_int + telefono.Pref_naz + telefono.Numero + " - whatsapp: " + Convert.ToString(telefono.Whatsapp)  , telefono.Id.ToString());
                    lbMod_Telefoni.Items.Add(itemTelefono);
                }
                if (collaboratore.Telefoni.Count > 0)
                {
                    lbMod_Telefoni.Rows = collaboratore.Telefoni.Count;
                }
                else
                {
                    lbMod_Telefoni.Rows = 1;
                }

                // IMMAGINE COLLABORATORE
                if (string.IsNullOrEmpty(collaboratore.PathFoto))
                {
                    imgCollaboratore.ImageUrl = ConfigurationManager.AppSettings["PATH_IMMAGINI_COLLABORATORI"] + ConfigurationManager.AppSettings["IMMAGINE_DUMMY_COLLABORATORE"];
                }
                else
                { 
                    imgCollaboratore.ImageUrl = ConfigurationManager.AppSettings["PATH_IMMAGINI_COLLABORATORI"] + collaboratore.PathFoto;
                }
                pnlContainer.Visible = true;
            }
            else
            {
                Session["ErrorPageText"] = esito.descrizione;
                string url = String.Format("~/pageError.aspx");
                Response.Redirect(url, true);
            }
        }

        protected void btnModifica_Click(object sender, EventArgs e)
        {

        }

        protected void btnSalva_Click(object sender, EventArgs e)
        {

        }

        protected void btnAnnulla_Click(object sender, EventArgs e)
        {

        }

        protected void btn_chiudi_Click(object sender, EventArgs e)
        {
            pnlContainer.Visible = false;
        }

        protected void lbInserisciCollaboratori_Click(object sender, EventArgs e)
        {

        }

        private void pulisciCampiDettaglio()
        {
            tbMod_CF.Text = "";
            tbMod_Cognome.Text = "";
            tbMod_Nome.Text = "";
            tbMod_Nazione.Text = "";
            tbMod_ComuneNascita.Text = "";
            tbMod_ProvinciaNascita.Text = "";
            tbMod_ComuneRiferimento.Text = "";
            tbMod_DataNascita.Text = "";
            tbMod_NomeSocieta.Text = "";
            tbMod_PartitaIva.Text = "";
            cbMod_Assunto.Checked = false;
            cbMod_Attivo.Checked = false;
            tbMod_Note.Text = "";

            lbMod_Qualifiche.Items.Clear();
            lbMod_Qualifiche.Rows = 1;

            lbMod_Indirizzi.Items.Clear();
            lbMod_Indirizzi.Rows = 1;

            lbMod_Email.Items.Clear();
            lbMod_Email.Rows = 1;

            lbMod_Telefoni.Items.Clear();
            lbMod_Telefoni.Rows = 1;

        }
    }
}