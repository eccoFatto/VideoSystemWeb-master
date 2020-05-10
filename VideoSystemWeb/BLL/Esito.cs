using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.Entity;

namespace VideoSystemWeb.BLL
{
    public class Esito
    {
        BasePage basePage = new BasePage();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static int ESITO_OK = 0;
        
        public const int ESITO_KO_ERRORE_LETTURA_TABELLA = 1;
        public const int ESITO_KO_ERRORE_SCRITTURA_TABELLA = 2;
        public const int ESITO_KO_ERRORE_UPDATE_TABELLA = 3;
        public const int ESITO_KO_ERRORE_UTENTE_NON_RICONOSCIUTO = 4;
        public const int ESITO_KO_ERRORE_VALIDAZIONE = 5;
        public const int ESITO_KO_ERRORE_NO_RISULTATI = 6;
        public const int ESITO_KO_ERRORE_GENERICO = 99;

        private string _descrizione;

        public Esito()
        {
            this.Codice = ESITO_OK;
            this.Descrizione = string.Empty;
        }

        public int Codice { get; set; }
       
        public string Descrizione
        {
            get
            {
                return _descrizione;
            }
            set 
            {
                _descrizione = value;
                if (!string.IsNullOrEmpty(_descrizione))
                {
                    Anag_Utenti utente = new Anag_Utenti();
                    utente.username = "ANONIMO";
                    try
                    {
                        if (HttpContext.Current.Session[SessionManager.UTENTE] != null)
                        {
                            utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);
                        }
                    }
                    catch (Exception)
                    {
                        utente.username = "ANONIMO";
                    }

                    //log.Error(utente.username + " - " + _descrizione);

                    if (SessionManager.VisualizzazioneAutomaticaPopupErrore)
                    {
                       // basePage.ShowError(_descrizione);

                        switch (this.Codice)
                        {
                            case ESITO_KO_ERRORE_VALIDAZIONE:
                                basePage.ShowWarning(_descrizione);
                                break;
                            case ESITO_KO_ERRORE_NO_RISULTATI:
                                basePage.ShowWarning(_descrizione);
                                break;
                            default:
                                basePage.ShowError(_descrizione);

                                log.Error(utente.username + " - " + _descrizione);
                                break;
                        }
                    }
                }
            }
        }

        
    }
}