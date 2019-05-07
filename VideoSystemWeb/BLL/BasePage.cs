using System;
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
        public static string versione = "1.27";
        public static string dataVersione = "05/05/2019";
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        #region ELEMENTI COMUNI IN VIEWSTATE

        //public List<ArticoliGruppi> listaArticoliGruppi
        //{
        //    get
        //    {
        //        if (ViewState["listaArticoliGruppi"] == null || ((List<ArticoliGruppi>)ViewState["listaArticoliGruppi"]).Count == 0)
        //        {
        //            ViewState["listaArticoliGruppi"] = Articoli_BLL.Instance.CaricaListaArticoliGruppi();
        //        }
        //        return (List<ArticoliGruppi>)ViewState["listaArticoliGruppi"];
        //    }
        //    set
        //    {
        //        ViewState["listaArticoliGruppi"] = value;
        //    }
        //}
        public List<FiguraProfessionale> listaCompletaFigProf
        {
            get
            {
                if (ViewState["listaCompletaFigProf"] == null || ((List<FiguraProfessionale>)ViewState["listaCompletaFigProf"]).Count() == 0)
                {
                    List<FiguraProfessionale> _listaCompletaFigProf = new List<FiguraProfessionale>();
                    foreach (Anag_Collaboratori collaboratore in listaAnagraficheCollaboratori)
                    {

                        try
                        {
                            _listaCompletaFigProf.Add(new FiguraProfessionale()
                            {
                                Id = collaboratore.Id,
                                Nome = collaboratore.Nome,
                                Cognome = collaboratore.Cognome,
                                Citta = collaboratore.ComuneRiferimento.Trim().ToLower(),
                                Telefono = collaboratore.Telefoni.Count == 0 ? "" : collaboratore.Telefoni.FirstOrDefault(x => x.Priorita == 1).Pref_naz + collaboratore.Telefoni.FirstOrDefault(x => x.Priorita == 1).Numero,
                                Qualifiche = collaboratore.Qualifiche,
                                Tipo = 0,
                                Nota = collaboratore.Note
                            });
                        }
                        catch (Exception ex)
                        {
                            //ShowError(collaboratore.Id.ToString() + " " + collaboratore.Cognome + Environment.NewLine + ex.Message);
                            log.Error(collaboratore.Id.ToString() + " " + collaboratore.Cognome, ex);
                        }

                    }

                    foreach (Anag_Clienti_Fornitori fornitore in listaAnagraficheFornitori)
                    {
                        try
                        {
                            _listaCompletaFigProf.Add(new FiguraProfessionale()
                            {
                                Id = fornitore.Id,
                                Cognome = fornitore.RagioneSociale,
                                Citta = fornitore.ComuneLegale.Trim().ToLower(),
                                Telefono = fornitore.Telefono,
                                Tipo = 1,
                                Nota = fornitore.Note
                            });
                        }
                        catch (Exception ex)
                        {
                            log.Error(fornitore.Id.ToString() + " " + fornitore.RagioneSociale, ex);
                        }

                    }
                    ViewState["listaCompletaFigProf"] = _listaCompletaFigProf.OrderBy(x => x.Cognome).ToList<FiguraProfessionale>();

                    //return new List<FiguraProfessionale>();
                }
                return (List<FiguraProfessionale>)ViewState["listaCompletaFigProf"];
            }

            set
            {
                ViewState["listaCompletaFigProf"] = value;
            }
        }
        public List<Anag_Qualifiche_Collaboratori> listaQualificheCollaboratori
        {
            get
            {
                if (ViewState["listaQualificheCollaboratori"] == null || ((List<Anag_Qualifiche_Collaboratori>)ViewState["listaQualificheCollaboratori"]).Count() == 0)
                {
                    Esito esito = new Esito();
                    ViewState["listaQualificheCollaboratori"] = Anag_Qualifiche_Collaboratori_BLL.Instance.getAllQualifiche(ref esito, true);
                }
                return (List<Anag_Qualifiche_Collaboratori>)ViewState["listaQualificheCollaboratori"];
            }

            set
            {
                ViewState["listaQualificheCollaboratori"] = value;
            }
        }
        public List<Anag_Collaboratori> listaAnagraficheCollaboratori
        {
            get
            {
                if (ViewState["listaAnagraficheCollaboratori"] == null || ((List<Anag_Collaboratori>)ViewState["listaAnagraficheCollaboratori"]).Count() == 0)
                {
                    Esito esito = new Esito();
                    ViewState["listaAnagraficheCollaboratori"] = Anag_Collaboratori_BLL.Instance.getAllCollaboratori(ref esito);
                    //return new List<Anag_Collaboratori>();
                }
                return (List<Anag_Collaboratori>)ViewState["listaAnagraficheCollaboratori"];
            }

            set
            {
                ViewState["listaAnagraficheCollaboratori"] = value;
            }
        }
        public List<Anag_Clienti_Fornitori> listaAnagraficheFornitori
        {
            get
            {
                if (ViewState["listaAnagraficheFornitori"] == null || ((List<Anag_Clienti_Fornitori>)ViewState["listaAnagraficheFornitori"]).Count() == 0)
                {
                    Esito esito = new Esito();
                    ViewState["listaAnagraficheFornitori"] = Anag_Clienti_Fornitori_BLL.Instance.CaricaListaFornitori(ref esito);
                    //return new List<Anag_Clienti_Fornitori>();
                }
                return (List<Anag_Clienti_Fornitori>)ViewState["listaAnagraficheFornitori"];
            }

            set
            {
                ViewState["listaAnagraficheFornitori"] = value;
            }
        }
        public List<Tipologica> listaTipiPagamento
        {
            get
            {
                if (ViewState["listaTipiPagamento"] == null || ((List<Tipologica>)ViewState["listaTipiPagamento"]).Count() == 0)
                {
                    ViewState["listaTipiPagamento"] = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_PAGAMENTO);
                }
                return (List<Tipologica>)ViewState["listaTipiPagamento"];
            }
            set
            {
                ViewState["listaTipiPagamento"] = value;
            }
        }
        public List<string> listaCittaCollaboratori
        {
            get
            {
                if (ViewState["listaCittaCollaboratori"] == null || ((List<string>)ViewState["listaCittaCollaboratori"]).Count() == 0)
                {
                    List<string> listaCittaCollaboratori = (from record in listaAnagraficheCollaboratori select record.ComuneRiferimento.Trim().ToLower()).ToList();
                    ViewState["listaCittaCollaboratori"] = listaCittaCollaboratori.Distinct().ToList<string>();
                }
                return (List<string>)ViewState["listaCittaCollaboratori"];
            }
            set
            {
                ViewState["listaCittaCollaboratori"] = value;
            }
        }
        public List<string> listaCittaFornitori
        {
            get
            {
                if (ViewState["listaCittaFornitori"] == null || ((List<string>)ViewState["listaCittaFornitori"]).Count() == 0)
                {
                    List<string> _listaCittaFornitori = (from record in listaAnagraficheFornitori select record.ComuneLegale.Trim().ToLower()).ToList();
                    ViewState["listaCittaFornitori"] = _listaCittaFornitori.Distinct().ToList<string>();
                }
                return (List<string>)ViewState["listaCittaFornitori"];
            }
            set
            {
                ViewState["listaCittaFornitori"] = value;
            }
        }
        public List<Anag_Referente_Clienti_Fornitori> listaReferenti
        {
            get
            {
                if (ViewState["listaReferenti"] == null || ((List<Anag_Referente_Clienti_Fornitori>)ViewState["listaReferenti"]).Count() == 0)
                {
                    ViewState["listaReferenti"] = new List<Anag_Referente_Clienti_Fornitori>();
                }
                return (List<Anag_Referente_Clienti_Fornitori>)ViewState["listaReferenti"];
            }
            set
            {
                ViewState["listaReferenti"] = value;
            }
        }
        public List<GiorniPagamentoFatture> listaGPF
        {
            get
            {
                Esito esito = new Esito();
                if (ViewState["listaGPF"] == null || ((List<GiorniPagamentoFatture>)ViewState["listaGPF"]).Count() == 0)
                {
                    ViewState["listaGPF"] = Config_BLL.Instance.getListaGiorniPagamentoFatture(ref esito);
                }
                return (List<GiorniPagamentoFatture>)ViewState["listaGPF"];
            }
            set
            {
                ViewState["listaGPF"] = value;
            }
        }
        public List<DatiBancari> listaDatiBancari
        {
            get
            {
                Esito esito = new Esito();
                if (ViewState["listaDatiBancari"] == null || ((List<DatiBancari>)ViewState["listaDatiBancari"]).Count() == 0)
                {
                    ViewState["listaDatiBancari"] = Config_BLL.Instance.getListaDatiBancari(ref esito);
                }
                return (List<DatiBancari>)ViewState["listaDatiBancari"];
            }
            set
            {
                ViewState["listaDatiBancari"] = value;
            }
        }
        //public List<string> listaIdTender
        //{
        //    get
        //    {
                
        //        if (ViewState["listaIdTender"] == null || ((List<string>)ViewState["listaIdTender"]).Count() == 0)
        //        {
        //            ViewState["listaIdTender"] = Config_BLL.Instance.getListaDatiBancari(ref esito);
        //        }
        //        return (List<string>)ViewState["listaIdTender"];
        //    }
        //    set
        //    {
        //        ViewState["listaIdTender"] = value;
        //    }
        //}
        public List<Tipologica> listaTender
        {
            get
            {
                if (ViewState["listaTender"] == null || ((List<Tipologica>)ViewState["listaTender"]).Count() == 0)
                {
                    ViewState["listaTender"] = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_TENDER);
                }
                return (List<Tipologica>)ViewState["listaTender"];
            }
            set
            {
                ViewState["listaTender"] = value;
            }
        }

        //public List<DatiArticoli> listaDatiArticoli
        //{
        //    get
        //    {
        //        if (ViewState["listaDatiArticoli"] == null || ((List<ArticoliGruppi>)ViewState["listaDatiArticoli"]).Count == 0)
        //        {
        //            ViewState["listaDatiArticoli"] = Articoli_BLL.Instance.CaricaListaArticoliGruppi();
        //        }
        //        return (List<DatiArticoli>)ViewState["listaDatiArticoli"];
        //    }
        //    set
        //    {
        //        ViewState["listaDatiArticoli"] = value;
        //    }
        //}


        #endregion



        public List<ColonneAgenda> listaRisorse
        {
            get
            {
                Esito esito = new Esito();
                List<ColonneAgenda> _listaRisorse = UtilityTipologiche.CaricaColonneAgenda(true,ref esito);
                _listaRisorse = _listaRisorse.OrderBy(c => c.sottotipo == "dipendenti").ThenBy(c => c.sottotipo == "extra").ThenBy(c => c.sottotipo).ToList<ColonneAgenda>();
                return _listaRisorse;
            }
        }
        public List<Tipologica> listaStati
        {
            get
            {
                return UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_STATO);
            }
        }

        public List<DatiAgenda> listaDatiAgenda;

        public List<Tipologica> listaTipiUtente
        {
            get
            {
                return UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_UTENTE);
            }
        }
        public List<Tipologica> listaTipiTipologie
        {
            get
            {
                return UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_TIPOLOGIE);
            }
        }
        public List<Tipologica> listaQualifiche
        {
            get
            {
                return UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_QUALIFICHE);
            }
        }
        public List<Tipologica> listaTipiClientiFornitori
        {
            get
            {
                return UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_CLIENTI_FORNITORI);
            }
        }
        public List<Tipologica> listaTipiGeneri
        {
            get
            {
                return UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_GENERE);
            }
        }
        public List<Tipologica> listaTipiGruppi
        {
            get
            {
                return UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_GRUPPO);
            }
        }
        public List<Tipologica> listaTipiSottogruppi
        {
            get
            {
                return UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_SOTTOGRUPPO);
            }
        }
        
        public List<Anag_Clienti_Fornitori> listaClientiFornitori;
        public List<Tipologica> listaTipiProtocolli
        {
            get
            {
                return UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_PROTOCOLLO);
            }
        }

        public Esito CaricaListeTipologiche()
        {
            Esito esito = new Esito();

            
            List<Tipologica> listaRisorse = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_COLONNE_AGENDA); //Agenda_BLL.Instance.CaricaColonne(ref esito);
            //UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_COLONNE_AGENDA, ref esito); //Tipologie.getListaRisorse();

            List<Tipologica> listaStati = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_STATO);

            List<Tipologica> listaTipiUtente = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_UTENTE);

            List<Tipologica> listaTipiTipologie = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_TIPOLOGIE);

            List<Tipologica> listaQualifiche = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_QUALIFICHE);

            List<Tipologica> listaTipiClientiFornitori = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_CLIENTI_FORNITORI);

            return esito;
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
                esito.codice = Esito.ESITO_KO_ERRORE_VALIDAZIONE;
                esito.descrizione = "Campo obbligatorio";
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
                    esito.codice = Esito.ESITO_KO_ERRORE_VALIDAZIONE;
                    esito.descrizione = "Controllare il campo";
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
                esito.codice = Esito.ESITO_KO_ERRORE_VALIDAZIONE;
                esito.descrizione = "Campo obbligatorio";
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
                    esito.codice = Esito.ESITO_KO_ERRORE_VALIDAZIONE;
                    esito.descrizione = "Controllare il campo";
                }
            }

            return result;
        }

        public bool validaIndirizzoEmail(string indirizzo)
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

        public static string validaIndirizzoEmail(TextBox t,bool isRequired, ref Esito esito)
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
                    esito.codice = Esito.ESITO_KO_ERRORE_VALIDAZIONE;
                    esito.descrizione = "Campo obbligatorio";
                    return t.Text;
                }
            }
        }
        public bool AbilitazioneInScrittura()
        {
            Esito esito = new Esito();

            bool abilitazioneScrittura = false;

            int idUtente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]).id_tipoUtente;
            Tipologica tipoUtenteLoggato = UtilityTipologiche.getElementByID(UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_UTENTE), idUtente, ref esito);
            abilitazioneScrittura = UtilityTipologiche.getParametroDaTipologica(tipoUtenteLoggato, "SCRITTURA", ref esito) == "1";

            return abilitazioneScrittura;
        }

        public void PopolaDDLTipologica(HtmlGenericControl listaDaPopolare, List<Tipologica> listaTipologica)
        {
            string elementi = listaDaPopolare.InnerHtml;
            foreach (Tipologica tipologica in listaTipologica)
            {
                elementi += "<li class='dropdown-item'><a class='elemLista' href='#' val='" + tipologica.id.ToString() + "'>" + tipologica.nome + "</a></li>";
            }
            listaDaPopolare.InnerHtml = elementi;
        }

        public void PopolaDDLTipologica(HtmlGenericControl listaDaPopolare, List<ColonneAgenda> listaColonne)
        {
            string elementi = listaDaPopolare.InnerHtml;
            foreach (ColonneAgenda tipologica in listaColonne)
            {
                elementi += "<li class='dropdown-item'><a class='elemLista' href='#' val='" + tipologica.id.ToString() + "'>" + tipologica.nome + "</a></li>";
            }
            listaDaPopolare.InnerHtml = elementi;
        }

        public void PopolaDDLGenerico<T>(HtmlGenericControl listaDaPopolare, List<T> lista)
        {
            string elementi = listaDaPopolare.InnerHtml;

            if (lista is List<Anag_Clienti_Fornitori>)
            {
                foreach (T elem in lista)
                {
                    Anag_Clienti_Fornitori cliente = ConvertValue<Anag_Clienti_Fornitori>(elem);
                    elementi += "<li class='dropdown-item'><a class='elemLista' href='#' val='" + cliente.Id.ToString() + "'>" + cliente.RagioneSociale + "</a></li>";
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
                esito.codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.descrizione = ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }
        public void ShowSuccess(string messaggio)
        {
            messaggio = messaggio.Replace("'", "\\'");
            messaggio = messaggio.Replace("\r\n", "<br/>");

            Page page = HttpContext.Current.Handler as Page;

           // ClientScript.RegisterStartupScript(typeof(Page), "apriModalSuccess", "<script>openSuccess('" + messaggio + "');</script>");
            ScriptManager.RegisterStartupScript(page, page.GetType(), "apriModalSuccess", script: "openSuccess('" + messaggio + "')", addScriptTags: true);
        }

        public void ShowWarning(string messaggio)
        {
            messaggio = messaggio.Replace("'", "\\'");
            messaggio = messaggio.Replace("\r\n", "<br/>");

            Page page = HttpContext.Current.Handler as Page;

            //ClientScript.RegisterStartupScript(typeof(Page), "apriModalWarning", "<script>openWarning('" + messaggio + "');</script>");
            ScriptManager.RegisterStartupScript(page, page.GetType(), "apriModalWarning", script: "openWarning('" + messaggio + "')", addScriptTags: true);
        }

        public void ShowError(string messaggio)
        {
            messaggio = messaggio.Replace("'", "\\'");
            messaggio = messaggio.Replace("\r\n", "<br/>");

            Page page = HttpContext.Current.Handler as Page;

            //ClientScript.RegisterStartupScript(typeof(Page), "apriModalError",  "<script>openError('" + messaggio + "');</script>");
            ScriptManager.RegisterStartupScript(page, page.GetType(), "apriModalError", script: "openError('" + messaggio + "')", addScriptTags: true);
            
        }

        

        public void GestisciErrore(Esito esito, string messaggioErroreSpecifico = null)
        {
            if (esito.codice != Esito.ESITO_OK)
            {
                if (messaggioErroreSpecifico != null)
                {
                    ShowError(messaggioErroreSpecifico);
                }
                else
                {
                    ShowError(esito.descrizione);
                }
            }
        }
    }
}
