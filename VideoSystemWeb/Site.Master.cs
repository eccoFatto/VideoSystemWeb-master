using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoSystemWeb.BLL;
using VideoSystemWeb.Entity;

namespace VideoSystemWeb
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // lbl_benvenuto.Text = "Bentornato " + ((Anag_Utente)Session[SessionManager.UTENTE]).Nome + " " + ((Anag_Utente)Session[SessionManager.UTENTE]).Cognome;
            try
            {

                if (Session[SessionManager.UTENTE] != null) {
                    Anag_Utenti utenteConnesso = (Anag_Utenti)Session[SessionManager.UTENTE];
                    lbl_benvenuto.Text = "Utente: " + utenteConnesso.Nome + " " + utenteConnesso.Cognome + " - Ruolo: " + utenteConnesso.tipoUtente;

                    lblVersione.Text = BasePage.versione;

                    lblDataVersione.Text = BasePage.dataVersione;

                    // IMPOSTO LA VISIBILITA' DEI MENU
                    switch (utenteConnesso.tipoUtente)
                    {
                        case "VISUALIZZATORE":
                            div_ANAGRAFICHE.Visible = false;
                            div_ARTICOLI.Visible = false;
                            div_DOCUMENTITRASPORTO.Visible = false;
                            div_MAGAZZINO.Visible = false;
                            div_PROTOCOLLI.Visible = false;
                            div_REPORT.Visible = false;
                            div_SCADENZARIO.Visible = false;
                            div_STATISTICHE.Visible = false;
                            div_TABELLE.Visible = false;
                            div_UTENTI.Visible = false;
                            break;
                        default:
                            div_ANAGRAFICHE.Visible = true;
                            div_ARTICOLI.Visible = true;
                            div_DOCUMENTITRASPORTO.Visible = true;
                            div_MAGAZZINO.Visible = true;
                            div_PROTOCOLLI.Visible = true;
                            div_REPORT.Visible = true;
                            div_SCADENZARIO.Visible = true;
                            div_STATISTICHE.Visible = true;
                            div_TABELLE.Visible = true;
                            div_UTENTI.Visible = true;
                            break;
                    }
                }
                else
                {
                    //Session["ErrorPageText"] = "TimeOut Sessione";
                    BasePage b = new BasePage();
                    b.ShowError("TimeOut Sessione");
                    string url = String.Format("~/Login.aspx");
                    Response.Redirect(url, true);
                }
                //}
            }
            catch (Exception ex)
            {

                string eccezione = "Site.Master.cs - Page_Load " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;

                //Session["ErrorPageText"] = eccezione;
                //string url = String.Format("~/pageError.aspx");
                //Response.Redirect(url, true);

                //BasePage b = new BasePage();
                //b.ShowError("TimeOut Sessione");
                string url = String.Format("~/Login.aspx");
                Response.Redirect(url, true);

            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            SessionManager.ClearSession();
            Response.Redirect("~/Login.aspx");
        }
    }
}