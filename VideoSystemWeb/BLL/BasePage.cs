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
namespace VideoSystemWeb.BLL
{
    public class BasePage : System.Web.UI.Page
    {
        public static string versione = "1.20";
        public static string dataVersione = "11/04/2019";

        public List<Tipologica> listaRisorse
        {
            get
            {
                List<Tipologica> _listaRisorse = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_COLONNE_AGENDA);
                _listaRisorse = _listaRisorse.OrderBy(c => c.sottotipo == "dipendenti").ThenBy(c => c.sottotipo == "extra").ThenBy(c => c.sottotipo).ToList<Tipologica>();
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

        public List<Tipologica> listaTender
        {
            get
            {
                return UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_TENDER);
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
