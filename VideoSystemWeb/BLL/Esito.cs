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
        
        public static int ESITO_KO_ERRORE_LETTURA_TABELLA = 1;
        public static int ESITO_KO_ERRORE_SCRITTURA_TABELLA = 2;
        public static int ESITO_KO_ERRORE_UPDATE_TABELLA = 3;
        public static int ESITO_KO_ERRORE_UTENTE_NON_RICONOSCIUTO = 4;
        public static int ESITO_KO_ERRORE_VALIDAZIONE = 5;
        public static int ESITO_KO_ERRORE_NO_RISULTATI = 6;
        public static int ESITO_KO_ERRORE_GENERICO = 99;

        private int _codice;
        private string _descrizione;

        public int codice { get; set; }
       
        public string descrizione
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
                    try
                    {
                        utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);
                    }
                    catch (Exception)
                    {
                        utente.username = "ANONIMO";
                    }

                    log.Error(utente.username + " - " + _descrizione);

                    if (SessionManager.VisualizzazioneAutomaticaPopupErrore)
                    {
                        basePage.ShowError(_descrizione);
                    }
                }
            }
        }

        public Esito()
        {
            this.codice = ESITO_OK;
            this.descrizione = string.Empty;
        }
    }
}