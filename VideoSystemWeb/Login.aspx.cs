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
using System.Security.Cryptography;
using System.Text;

namespace VideoSystemWeb
{
    
    public partial class Login : BasePage
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Esito esito = new Esito();
        protected void Page_Load(object sender, EventArgs e)
        {
            

            if (!this.IsPostBack)
            {
                //caricaListeTipologiche();
                //Session["connectionString"] = constr;
                Session[SessionManager.UTENTE] = null;
            }
        }

        protected void btnLogIn_Click(object sender, EventArgs e)
        {
            Esito esito = new Esito();
            lblErrorLogin.Visible = false;

            // TROVO IL CODICE MD5 DELLA PASSWORD
            MD5 md5Hash = MD5.Create();
            string pwdEncrypted = GetMd5Hash(md5Hash, tbPassword.Text.Trim());
            md5Hash.Dispose();

            //Login_BLL.Instance.Connetti(tbUser.Text.Trim(), tbPassword.Text.Trim(), ref esito);
            Login_BLL.Instance.Connetti(tbUser.Text.Trim(), pwdEncrypted, ref esito);

            if (esito.codice == Esito.ESITO_OK)
            {
                log.Info("UTENTE " + tbUser.Text.Trim() + " Loggato!");
                Response.Redirect("~/Agenda/Agenda.aspx");
            }
            else if (esito.codice == Esito.ESITO_KO_ERRORE_UTENTE_NON_RICONOSCIUTO)
            {
                lblErrorLogin.Text = esito.descrizione;
                lblErrorLogin.Visible = true;
            }
            else
            {
                Session["ErrorPageText"] = esito.descrizione;
                string url = String.Format("~/pageError.aspx");
                Response.Redirect(url, true);
            }
            
        }

    }
}