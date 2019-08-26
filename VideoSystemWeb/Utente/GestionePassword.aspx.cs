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
            Anag_Utenti utente = (Anag_Utenti)Session[SessionManager.UTENTE];
            Esito esito = ValidazioneCampi(utente);
            if (esito.Codice == Esito.ESITO_OK)
            {
                // AGGIORNO LA NUOVA PASSWORD SU ANAG_UTENTI

                MD5 md5Hash = MD5.Create();
                string pwdEncrypted = GetMd5Hash(md5Hash, tbNewPassword.Text.Trim());
                md5Hash.Dispose();
                utente.password = pwdEncrypted;

                esito = Login_BLL.Instance.AggiornaUtente(utente);
                if (esito.Codice != Esito.ESITO_OK)
                {
                    ShowWarning(esito.Descrizione);
                }
                else
                {
                    Session[SessionManager.UTENTE] = utente;

                    ShowSuccess("Password modificata correttamente");
                }
            }
            else
            {
                ShowWarning(esito.Descrizione);
            }
        }

        private Esito ValidazioneCampi(Anag_Utenti utente)
        {
            Esito esito = new Esito();
            esito = ControlloCampiObbligatori();
            if (esito.Codice != Esito.ESITO_OK)
            {
                esito.Descrizione = "Controllare i campi evidenziati";
            }
            else if (!ControlloNuovaPassword())
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_VALIDAZIONE;
                esito.Descrizione = "Errore nella convalida delle nuove Password";
            }
            else if (!ControlloVecchiaPassword(utente))
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_VALIDAZIONE;
                esito.Descrizione = "La password inserita non è valida";
            }

            return esito;
        }

        private Esito ControlloCampiObbligatori()
        {
            Esito esito = new Esito();

            tbOldPassword.Text = BasePage.ValidaCampo(tbOldPassword, "", true, ref esito);
            tbNewPassword.Text = BasePage.ValidaCampo(tbNewPassword, "", true, ref esito);
            tbConfirmNewPassword.Text = BasePage.ValidaCampo(tbConfirmNewPassword, "", true, ref esito);

            return esito;
        }

        private bool ControlloNuovaPassword()
        {
            return tbNewPassword.Text == tbConfirmNewPassword.Text;
        }

        private bool ControlloVecchiaPassword(Anag_Utenti utente)
        {
            // TROVO IL CODICE MD5 DELLA PASSWORD
            MD5 md5Hash = MD5.Create();
            string pwdEncrypted = GetMd5Hash(md5Hash, tbOldPassword.Text.Trim());
            md5Hash.Dispose();

            return utente.password.Equals(pwdEncrypted);
        }
    }
}