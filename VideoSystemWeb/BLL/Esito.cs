using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.BLL
{
    public class Esito
    {
        public static int ESITO_OK = 0;
        public static int ESITO_KO_ERRORE_NO_RISULTATI = 0;
        public static int ESITO_KO_ERRORE_LETTURA_TABELLA = 1;
        public static int ESITO_KO_ERRORE_SCRITTURA_TABELLA = 2;
        public static int ESITO_KO_ERRORE_UPDATE_TABELLA = 3;
        public static int ESITO_KO_ERRORE_UTENTE_NON_RICONOSCIUTO = 4;
        public static int ESITO_KO_ERRORE_VALIDAZIONE = 5;
        public static int ESITO_KO_ERRORE_GENERICO = 99;

        public int codice { get; set; }
        public string descrizione { get; set; }

        public Esito()
        {
            this.codice = ESITO_OK;
            this.descrizione = string.Empty;
        }
    }
}