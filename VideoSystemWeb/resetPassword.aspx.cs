using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoSystemWeb.BLL;
using VideoSystemWeb.Entity;
using VideoSystemWeb.DAL;
using System.Data;
using System.Security.Cryptography;

namespace VideoSystemWeb
{
    public partial class resetPassword : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            lblErrorLogin.Visible = false;
            BasePage bp = new BasePage();
            if (bp.validaIndirizzoEmail(tbEmail.Text.Trim()))
            {
                Esito esito = new Esito();
                Utenti utente = Anag_Utenti_BLL.Instance.getUtenteByUserAndEmail(tbUser.Text.Trim(), tbEmail.Text.Trim(), ref esito);
                if (esito.codice == 0 && utente.Id > 0) {

                    string nuovaPassword = DateTime.Now.Ticks.ToString().Substring(13);

                    MD5 md5Hash = MD5.Create();
                    string nuovaPasswordCriptata = BasePage.GetMd5Hash(md5Hash, nuovaPassword);
                    md5Hash.Dispose();

                    // AGGIORNO LA PASSWORD
                    esito = Anag_Utenti_BLL.Instance.AggiornaPassword(utente.Id, nuovaPasswordCriptata);
                    if (esito.codice == 0)
                    {
                        // DESTINATARIO
                        List<string> destinatari = new List<string>();
                        destinatari.Add(utente.Email);
                        // MESSAGGIO
                        string bodyMessage = "<table style='width: 100 %; '><tr><td style='width: 10 %; align - content: center; text - align: center; '><img style='height:100px' src='http://www.videosystemproduction.it/Images/logoVSP.png' /></td><td style='width: 90%; align-content: center; text-align: left;'>&nbsp;</td></table><br />" +
                        "<h2 style='text-align:center;color:blue;margin:5px;padding:5px;'>Come da te richiesto e&acute; stata generata una nuova password per la tua utenza Videosystem (@utenza) </h2><br />" +
                        "<h3 style = 'text-align:center;color:blue;margin:5px;padding:5px;'> La nuova password e&acute; la seguente: (@password) </h3><br />" +
                        "<h3 style = 'text-align:center;color:blue;margin:5px;padding:5px;'> Si consiglia di modificarla dopo il primo accesso sul sito <a>http://www.videosystemproduction.it/Login.aspx</a></h3><br />";
                        bodyMessage = bodyMessage.Replace("@utenza", utente.Username);
                        bodyMessage = bodyMessage.Replace("@password", nuovaPassword);

                        // MANDO NUOVA PASSWORD VIA MAIL
                        esito = SendEmail(destinatari, "VIDEOSYSTEM - Notifica cambio Password", bodyMessage, null);
                        if (esito.codice == 0)
                        {
                            lblErrorLogin.Visible = true;
                            lblErrorLogin.ForeColor = System.Drawing.Color.Green;
                            lblErrorLogin.Text = "Nuova Password creata correttamente, controllare la mail e seguire le istruzioni.";

                        }
                        else
                        {
                            lblErrorLogin.Visible = true;
                            lblErrorLogin.ForeColor = System.Drawing.Color.Red;
                            lblErrorLogin.Text = "Attenzione, errore durante l'invio mail." + Environment.NewLine + esito.descrizione;
                        }
                    }
                    else
                    {
                        lblErrorLogin.Visible = true;
                        lblErrorLogin.ForeColor = System.Drawing.Color.Red;
                        lblErrorLogin.Text = "Attenzione, errore durante l'aggiornamento Password." + Environment.NewLine + esito.descrizione;
                    }

                }
                else
                {
                    lblErrorLogin.Visible = true;
                    lblErrorLogin.Text = "Attenzione, utenza ed email non trovate, verificare." + Environment.NewLine + esito.descrizione;
                }
            }
            else
            {
                lblErrorLogin.Visible = true;
                lblErrorLogin.Text = "Attenzione, indirizzo E-mail non valido, verificare.";
            }
        }
    }
}