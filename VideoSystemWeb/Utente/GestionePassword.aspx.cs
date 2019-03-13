using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoSystemWeb.BLL;
using VideoSystemWeb.Entity;
using System.Security.Cryptography;
using System.Text;
namespace VideoSystemWeb.Utente
{
    public partial class GestionePassword : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnConfermaCambioPwd_Click(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
            if (tbNewPassword.Text.Equals(tbConfirmNewPassword.Text))
            {
                // TROVO IL CODICE MD5 DELLA PASSWORD
                MD5 md5Hash = MD5.Create();
                string pwdEncrypted = GetMd5Hash(md5Hash, tbOldPassword.Text.Trim());
                md5Hash.Dispose();

                Anag_Utenti u = (Anag_Utenti)Session[SessionManager.UTENTE];
                if (u.password.Equals(pwdEncrypted))
                {
                    md5Hash = MD5.Create();
                    string newPwdEncrypted = GetMd5Hash(md5Hash, tbNewPassword.Text.Trim());
                    md5Hash.Dispose();
                    u.password = newPwdEncrypted;
                    // AGGIORNO LA NUOVA PASSWORD SU ANAG_UTENTI
                    Esito esito = Login_BLL.Instance.AggiornaUtente(u);
                    if (esito.codice != Esito.ESITO_OK)
                    {
                        ShowError(esito.descrizione);
                        //panelErrore.Style.Remove("display");
                        //panelErrore.Style.Add("display", "block");
                        //lbl_MessaggioErrore.Text = esito.descrizione;
                    }
                    else
                    {
                        Session[SessionManager.UTENTE] = u;
                        lblMessage.Text = "PASSWORD MODIFICATA CORRETTAMENTE";
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                        lblMessage.Visible = true;
                        ShowSuccess("PASSWORD MODIFICATA CORRETTAMENTE");
                    }
                }
                else
                {
                    lblMessage.Text = "Errore nella convalida delle nuove Password";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Visible = true;
                    ShowError("La password inserita non è valida!");
                }

            }
            else
            {
                lblMessage.Text = "Errore nella convalida delle nuove Password";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Visible = true;
                ShowWarning("Errore nella convalida delle nuove Password");
            }
        }
    }
}