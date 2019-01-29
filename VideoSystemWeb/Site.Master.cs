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
                lbl_benvenuto.Text = "Utente: " + ((Anag_Utenti)Session[SessionManager.UTENTE]).Nome + " " + ((Anag_Utenti)Session[SessionManager.UTENTE]).Cognome + " - Ruolo: " + ((Anag_Utenti)Session[SessionManager.UTENTE]).tipoUtente;

            }
            catch (Exception ex)
            {

                string eccezione = "Site.Master.cs - Page_Load " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
                //string paginaErrore = "pageError.aspx?messaggio=" + eccezione;
                //string url = String.Format("~/pageError.aspx?messaggio={0}", Server.UrlEncode(eccezione));
                Session["ErrorPageText"]= eccezione;
                string url = String.Format("~/pageError.aspx");
                Response.Redirect(url, true);
            }
        }
    }
}