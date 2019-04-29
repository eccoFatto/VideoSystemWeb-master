using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using MimeKit;
using MailKit;
namespace VideoSystemWeb
{
    public partial class testMail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSendMail_Click(object sender, EventArgs e)
        {
            lblInfo.Text = "";
            // TEST INVIO MAIL DA ARUBA
            try
            {
                #region send con Mailkit
                var message = new MimeMessage();

                message.From.Add(new MailboxAddress(tbMailFrom.Text.Trim(), tbMailFrom.Text.Trim()));
                message.To.Add(new MailboxAddress(tbMailTo.Text.Trim(), tbMailTo.Text.Trim()));
                message.Subject = tbSubject.Text.Trim();

                var builder = new BodyBuilder();

                //builder.TextBody = tbBody.Text.Trim();

                builder.Attachments.Add(@"C:\TEMP\web.config");
                builder.HtmlBody = "<H1>" + tbBody.Text.Trim() + "</H1>";

                message.Body = builder.ToMessageBody();
                //message.HtmlBody = builder.ToMessageBody();
                //message.HtmlBody = builder.HtmlBody;

                try
                {
                    var client = new MailKit.Net.Smtp.SmtpClient();

                    client.Connect(tbClient.Text.Trim(),Convert.ToInt32(tbPorta.Text.Trim()), Convert.ToBoolean(ddlSSL.SelectedValue));
                    client.Authenticate(tbUser.Text.Trim(), tbPassword.Text.Trim());
                    client.Send(message);
                    client.Disconnect(true);

                    lblInfo.ForeColor = System.Drawing.Color.Green;
                    lblInfo.Text = "Mail Inviata";
                }
                catch (Exception ex)
                {
                    lblInfo.ForeColor = System.Drawing.Color.Red;
                    lblInfo.Text = "Errore invio mail" + Environment.NewLine + ex.Message;
                }

                #endregion

                #region send con Classi Net
                //MailMessage mail = new MailMessage();

                //string smtpClient = tbClient.Text.Trim();
                //// SERVER SMTP
                //SmtpClient SmtpServer = new SmtpClient(smtpClient);

                //// MITTENTE
                //mail.From = new MailAddress(tbMailFrom.Text.Trim());

                //// DESTINATARIO

                //char[] divisore = { ';' };
                //string[] arDestinatari = tbMailTo.Text.Trim().Split(divisore,StringSplitOptions.RemoveEmptyEntries);
                //foreach (string item in arDestinatari)
                //{
                //    mail.To.Add(item);
                //}

                //// OGGETTO MAIL
                //mail.Subject = tbSubject.Text.Trim();

                //// BODY MAIL
                //string body = tbBody.Text.Trim();

                //mail.Body = body;

                //string smtpServerCredentialsId = tbUser.Text.Trim();
                //string smtpServerCredentialsPassWord = tbPassword.Text.Trim();

                //// CREDENZIALI SERVER
                ////SmtpServer.Port = Convert.ToInt32(smtpServerPort);
                //SmtpServer.Port = Convert.ToInt32(tbPorta.Text.Trim());
                //SmtpServer.Credentials = new System.Net.NetworkCredential(smtpServerCredentialsId, smtpServerCredentialsPassWord);
                //SmtpServer.EnableSsl = Convert.ToBoolean(ddlSSL.SelectedValue);

                ////INVIO EMAIL
                //SmtpServer.Send(mail);

                //lblInfo.ForeColor = System.Drawing.Color.Green;
                //lblInfo.Text = "Mail Inviata";
                //// FINE TEST INVIO EMAIL
                # endregion
            }
            catch (Exception ex)
            {
                lblInfo.ForeColor = System.Drawing.Color.Red;
                lblInfo.Text= "Errore invio mail" + Environment.NewLine + ex.Message;
            }
        }
    }
}