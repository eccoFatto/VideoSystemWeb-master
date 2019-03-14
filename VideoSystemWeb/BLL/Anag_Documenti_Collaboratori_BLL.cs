using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;
namespace VideoSystemWeb.BLL
{
    public class Anag_Documenti_Collaboratori_BLL
    {
        //singleton
        private static volatile Anag_Documenti_Collaboratori_BLL instance;
        private static object objForLock = new Object();
        private Anag_Documenti_Collaboratori_BLL() { }
        public static Anag_Documenti_Collaboratori_BLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Anag_Documenti_Collaboratori_BLL();
                    }
                }
                return instance;
            }
        }

        public Anag_Documenti_Collaboratori getDocumentoById(ref Esito esito, int id)
        {
            Anag_Documenti_Collaboratori documentoREt = Anag_Documenti_Collaboratori_DAL.Instance.getDocumentoById(ref esito, id);

            return documentoREt;
        }

        public int CreaDocumentoCollaboratore(Anag_Documenti_Collaboratori documentoCollaboratore, Anag_Utenti utente, ref Esito esito)
        {
            int iREt = Anag_Documenti_Collaboratori_DAL.Instance.CreaDocumentoCollaboratore(documentoCollaboratore, utente, ref esito);

            return iREt;
        }

        public Esito AggiornaDocumentoCollaboratore(Anag_Documenti_Collaboratori documentoCollaboratore, Anag_Utenti utente)
        {
            Esito esito = Anag_Documenti_Collaboratori_DAL.Instance.AggiornaDocumentoCollaboratore(documentoCollaboratore, utente);

            return esito;
        }

        public Esito EliminaDocumentoCollaboratore(int idDocumentoCollaboratore, Anag_Utenti utente)
        {
            Esito esito = Anag_Documenti_Collaboratori_DAL.Instance.EliminaDocumentoCollaboratore(idDocumentoCollaboratore,utente);

            return esito;
        }

    }
}