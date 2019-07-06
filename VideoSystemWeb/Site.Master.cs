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
                //bool isAutenticated = Convert.ToBoolean(Application.Get("IS_AUTHENTICATED").ToString());
                //if (!isAutenticated)
                //{
                //    Session["ErrorPageText"] = "TimeOut Sessione";
                //    string url = String.Format("~/pageError.aspx");
                //    Response.Redirect(url, true);

                //}
                //else
                //{
                    if (Session[SessionManager.UTENTE] != null) { 
                        lbl_benvenuto.Text = "Utente: " + ((Anag_Utenti)Session[SessionManager.UTENTE]).Nome + " " + ((Anag_Utenti)Session[SessionManager.UTENTE]).Cognome + " - Ruolo: " + ((Anag_Utenti)Session[SessionManager.UTENTE]).tipoUtente;

                        lblVersione.Text = BasePage.versione;

                        lblDataVersione.Text = BasePage.dataVersione;
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

                Session["ErrorPageText"] = eccezione;
                string url = String.Format("~/pageError.aspx");
                Response.Redirect(url, true);

                //BasePage b = new BasePage();
                //b.ShowError("TimeOut Sessione");
                //string url = String.Format("~/Login.aspx");
                //Response.Redirect(url, true);

            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            SessionManager.ClearSession();
            Response.Redirect("~/Login.aspx");
        }
    }
}