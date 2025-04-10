﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using VideoSystemWeb.Entity;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using MailKit;
using MimeKit;
using System.Configuration;
namespace VideoSystemWeb.BLL
{
    public class BasePage : System.Web.UI.Page
    {
        public static string versione = "2.05";
        public static string dataVersione = "04/04/2025";

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private void Page_PreInit(object sender, EventArgs e)
        {
            Response.AppendHeader("Cache-Control", "no-cache, no-store, must-revalidate"); // HTTP 1.1.
            Response.AppendHeader("Pragma", "no-cache"); // HTTP 1.0.
            Response.AppendHeader("Expires", "0"); // Proxies.

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.AppendCacheExtension("must-revalidate");
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.Now.AddDays(-1));
        }

        public static T ValidaCampo<T>(WebControl campo, T defaultValue, bool isRequired, ref Esito esito)
        {
            T result = defaultValue;

            string valore = "";

            if (campo is TextBox)
            {
                valore = ((TextBox)campo).Text;
            }
            else if (campo is DropDownList)
            {
                valore = ((DropDownList)campo).SelectedValue;
            }
            else if (campo is CheckBox)
            {
                valore = Convert.ToString(((CheckBox)campo).Checked);
            }

            if (isRequired && string.IsNullOrEmpty(valore))
            {
                campo.CssClass += " erroreValidazione";
                esito.Codice = Esito.ESITO_KO_ERRORE_VALIDAZIONE;
                esito.Descrizione = "Controllare i campi evidenziati";
            }
            else
            {
                try
                {
                    if (!isRequired && string.IsNullOrEmpty(valore))
                    {
                        valore = defaultValue.ToString();
                    }

                    campo.CssClass = campo.CssClass.Replace("erroreValidazione", "");
                    result = (T)Convert.ChangeType(valore, typeof(T));
                }
                catch
                {
                    campo.CssClass += " erroreValidazione";
                    esito.Codice = Esito.ESITO_KO_ERRORE_VALIDAZIONE;
                    esito.Descrizione = "Controllare i campi evidenziati";
                }
            }

            return result;
        }

        public static T ValidaCampo<T>(WebControl campo, HiddenField campoValore, T defaultValue, bool isRequired, ref Esito esito)
        {
            T result = defaultValue;

            string valore = campoValore.Value;

            if (isRequired && string.IsNullOrEmpty(valore))
            {
                campo.CssClass += " erroreValidazione";
                esito.Codice = Esito.ESITO_KO_ERRORE_VALIDAZIONE;
                esito.Descrizione = "Controllare i campi evidenziati";
            }
            else
            {
                try
                {
                    if (!isRequired && string.IsNullOrEmpty(valore))
                    {
                        valore = defaultValue.ToString();
                    }

                    campo.CssClass = campo.CssClass.Replace("erroreValidazione", "");
                    result = (T)Convert.ChangeType(valore, typeof(T));
                }
                catch
                {
                    campo.CssClass += " erroreValidazione";
                    esito.Codice = Esito.ESITO_KO_ERRORE_VALIDAZIONE;
                    esito.Descrizione = "Controllare i campi evidenziati";
                }
            }

            return result;
        }

        public bool ValidaIndirizzoEmail(string indirizzo)
        {
            Regex regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            Match match = regex.Match(indirizzo);
            if (match.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string ValidaIndirizzoEmail(TextBox t,bool isRequired, ref Esito esito)
        {
            if (string.IsNullOrEmpty(t.Text.Trim()) && !isRequired){
                t.CssClass = t.CssClass.Replace("erroreValidazione", "");
                return "";
            }
            else { 
                Regex regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                Match match = regex.Match(t.Text);
                if (match.Success)
                {
                    t.CssClass = t.CssClass.Replace("erroreValidazione", "");
                    return t.Text;
                }
                else
                {
                    t.CssClass += " erroreValidazione";
                    esito.Codice = Esito.ESITO_KO_ERRORE_VALIDAZIONE;
                    esito.Descrizione = "Controllare i campi evidenziati";
                    return t.Text;
                }
            }
        }

        public bool AbilitazioneInScrittura()
        {
            Esito esito = new Esito();

            bool abilitazioneScrittura = false;
            if (HttpContext.Current.Session[SessionManager.UTENTE] != null)
            {
                int idUtente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]).id_tipoUtente;
                Tipologica tipoUtenteLoggato = UtilityTipologiche.getElementByID(UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_UTENTE), idUtente, ref esito);
                abilitazioneScrittura = UtilityTipologiche.getParametroDaTipologica(tipoUtenteLoggato, "SCRITTURA", ref esito) == "1";
            }

            return abilitazioneScrittura;
        }

        public void PopolaDDLTipologica(HtmlGenericControl listaDaPopolare, List<Tipologica> listaTipologica, string nomeFiltro)
        {
            //string elementi = listaDaPopolare.InnerHtml;

            string elementi = "<input class='form-control' id='" + nomeFiltro + "' type='text' placeholder='Cerca..'>"; // aggiungo elemento vuoto
            foreach (Tipologica tipologica in listaTipologica)
            {
                elementi += "<li class='dropdown-item'><a class='elemLista' href='#' val='" + tipologica.id.ToString() + "'>" + tipologica.nome + "</a></li>";
            }
            listaDaPopolare.InnerHtml = elementi;
        }

        public void PopolaDDLTipologica(HtmlGenericControl listaDaPopolare, List<ColonneAgenda> listaColonne, string nomeFiltro)
        {
            //string elementi = listaDaPopolare.InnerHtml;

            string elementi = "<input class='form-control' id='" + nomeFiltro + "' type='text' placeholder='Cerca..'>"; // aggiungo elemento vuoto
            foreach (ColonneAgenda tipologica in listaColonne)
            {
                elementi += "<li class='dropdown-item'><a class='elemLista' href='#' val='" + tipologica.id.ToString() + "'>" + tipologica.nome + "</a></li>";
            }
            listaDaPopolare.InnerHtml = elementi;
        }

        public void PopolaDDLGenerico<T>(HtmlGenericControl listaDaPopolare, List<T> lista, string nomeFiltro)
        {
            //string elementi =  listaDaPopolare.InnerHtml;

            string elementi = "<input class='form-control' id='" + nomeFiltro + "' type='text' placeholder='Cerca..'><li class='dropdown-item'><a class='elemLista' href='#' val=''>&nbsp;</a></li>"; // aggiungo elemento vuoto
            if (lista is List<Anag_Clienti_Fornitori>)
            {
                foreach (T elem in lista)
                {
                    Anag_Clienti_Fornitori cliente = ConvertValue<Anag_Clienti_Fornitori>(elem);
                    elementi += "<li class='dropdown-item'><a class='elemLista' href='#' val='" + cliente.Id.ToString() + "'>" + cliente.RagioneSociale + "</a></li>";
                }
                listaDaPopolare.InnerHtml = elementi;
            }
            else if (lista is List<DatiLavorazione>)
            {
                foreach (T elem in lista)
                {
                    DatiLavorazione lavorazione = ConvertValue<DatiLavorazione>(elem);
                    elementi += "<li class='dropdown-item'><a class='elemLista' href='#' val='" + lavorazione.CodiceLavorazione + "'>" + lavorazione.CodiceLavorazione + " - " + lavorazione.DescrizioneLavorazione + "</a></li>";
                }
                listaDaPopolare.InnerHtml = elementi;
            }
            else if (lista is List<Protocolli>)
            {
                foreach (T elem in lista)
                {
                    Protocolli protocollo = ConvertValue<Protocolli>(elem);
                    elementi += "<li class='dropdown-item'><a class='elemLista' href='#' val='" + protocollo.Id.ToString() + "'>" + protocollo.Descrizione + "</a></li>";
                }
                listaDaPopolare.InnerHtml = elementi;
            }
            else if (lista is List<string>) // caso generico di una lista di astringhe, quindi senza ID
            {
                foreach (T elem in lista)
                {
                    string stringa = ConvertValue<string>(elem);
                    elementi += "<li class='dropdown-item'><a class='elemLista' href='#' val='" + stringa + "'>" + stringa + "</a></li>";
                }
                listaDaPopolare.InnerHtml = elementi;
            }
        }

        private static T ConvertValue<T>(object value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public void CheckIsMobile() { 

            HttpBrowserCapabilities bc = Request.Browser;
            if (bc.IsMobileDevice) {
                this.MasterPageFile = "/Site.Master";
            }
        }

        public static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        public static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string trimNote(string note, int nRighe)
        {

            if (string.IsNullOrEmpty(note))
            {
                return "";
            }

            if (note.IndexOf("\n") < 0)
            {
                return note;
            }
            else
            {
                string sRet = note;
                string appoNote = note;
                string ret = "";
                for (int i = 0; i < nRighe; i++)
                {
                    int indice = appoNote.IndexOf("\n");
                    if (indice > -1)
                    {
                        sRet = appoNote.Substring(0, indice+1);
                        ret += sRet;
                        appoNote = appoNote.Substring(indice+1);
                    }
                    else
                    {
                        //NON TROVO A CAPO QUINDI ESCO DALLA ROUTINE
                        return note;
                    }
                }
                return ret;
            }
            
        }


        public static Esito SendEmail(List<string> receivers, string subject, string body, List<string> attachments)
        {
            Esito esito = new Esito();
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(ConfigurationManager.AppSettings["EMAIL_FROM"], ConfigurationManager.AppSettings["EMAIL_FROM"]));

            foreach (string receiver in receivers)
            {
                message.To.Add(new MailboxAddress(receiver, receiver));
            }
            

            message.Subject = subject;

            var builder = new BodyBuilder();


            if (attachments != null) { 
                foreach (string attachment in attachments)
                {
                    builder.Attachments.Add(attachment);
                }
            }
            //builder.TextBody = tbBody.Text.Trim();
            builder.HtmlBody = body;
            message.Body = builder.ToMessageBody();
            //message.HtmlBody = builder.ToMessageBody();
            //message.HtmlBody = builder.HtmlBody;

            try
            {
                var client = new MailKit.Net.Smtp.SmtpClient();

                client.Connect(ConfigurationManager.AppSettings["EMAIL_CLIENT"], Convert.ToInt32(ConfigurationManager.AppSettings["EMAIL_PORT"]), Convert.ToBoolean(ConfigurationManager.AppSettings["EMAIL_SSL"]));
                client.Authenticate(ConfigurationManager.AppSettings["EMAIL_USER"], ConfigurationManager.AppSettings["EMAIL_PASSWORD"]);
                client.Send(message);
                client.Disconnect(true);

            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.Descrizione = ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }
        public void ShowSuccess(string messaggio)
        {
            messaggio = messaggio.Replace("\\", "/");
            messaggio = messaggio.Replace("'", "\\'");
            messaggio = messaggio.Replace("\r\n", "<br/>");

            Page page = HttpContext.Current.Handler as Page;
            ScriptManager.RegisterStartupScript(page, page.GetType(), "apriModalSuccess", script: "openSuccess('" + messaggio + "')", addScriptTags: true);
        }

        public void ShowWarning(string messaggio)
        {
            messaggio = messaggio.Replace("\\", "/");
            messaggio = messaggio.Replace("'", "\\'");
            messaggio = messaggio.Replace("\r\n", "<br/>");

            Page page = HttpContext.Current.Handler as Page;
            ScriptManager.RegisterStartupScript(page, page.GetType(), "apriModalWarning", script: "openWarning('" + messaggio + "')", addScriptTags: true);
        }

        public void ShowError(string messaggio)
        {
            messaggio = messaggio.Replace("\\", "/");
            messaggio = messaggio.Replace("'", "\\'");
            messaggio = messaggio.Replace("\r\n", "<br/>");

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["DEBUG_MODE"]))
            {
                Page page = HttpContext.Current.Handler as Page;
                ScriptManager.RegisterStartupScript(page, page.GetType(), "apriModalError", script: "openError('" + messaggio + "')", addScriptTags: true);
            }
        }

        public void GestisciErrore(Esito esito, string messaggioErroreSpecifico = null)
        {
            if (esito.Codice != Esito.ESITO_OK)
            {
                if (messaggioErroreSpecifico != null)
                {
                    ShowError(messaggioErroreSpecifico);
                }
                else
                {
                    ShowError(esito.Descrizione);
                }
            }
        }
        public int GetColumnIndexByName(GridViewRow row, string SearchColumnName)
        {
            int columnIndex = 0;
            foreach (DataControlFieldCell cell in row.Cells)
            {
                if (cell.ContainingField is BoundField)
                {
                    if (((BoundField)cell.ContainingField).DataField.Equals(SearchColumnName))
                    {
                        break;
                    }
                }
                columnIndex++;
            }
            return columnIndex;
        }


        public string GetColumnNameByIndex(GridViewRow row, int index)
        {
            string ret = "";
            int columnIndex = 0;
            foreach (DataControlFieldCell cell in row.Cells)
            {

                if (cell.ContainingField is BoundField)
                {
                    if (columnIndex == index)
                    {
                        ret = ((BoundField)cell.ContainingField).DataField + ";" + ((BoundField)cell.ContainingField).HeaderText;

                        break;
                    }
                }
                columnIndex++;
            }
            return ret;
        }
    }
}
