using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;
namespace VideoSystemWeb.BLL
{
    public class Anag_Indirizzi_Collaboratori_BLL
    {    
        //singleton
        private static volatile Anag_Indirizzi_Collaboratori_BLL instance;
        private static object objForLock = new Object();
        private Anag_Indirizzi_Collaboratori_BLL() { }
        public static Anag_Indirizzi_Collaboratori_BLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Anag_Indirizzi_Collaboratori_BLL();
                    }
                }
                return instance;
            }
        }

        public Anag_Indirizzi_Collaboratori getIndirizzoById(ref Esito esito, int id)
        {
            Anag_Indirizzi_Collaboratori indirizzo = Anag_Indirizzi_Collaboratori_DAL.Instance.getIndirizzoById(ref esito, id);
            return indirizzo;
        }
        public int CreaIndirizziCollaboratore(Anag_Indirizzi_Collaboratori indirizzoCollaboratore, Anag_Utenti utente, ref Esito esito)
        {
            int iREt = Anag_Indirizzi_Collaboratori_DAL.Instance.CreaIndirizziCollaboratore(indirizzoCollaboratore,utente, ref esito);

            return iREt;
        }

        public Esito AggiornaIndirizziCollaboratore(Anag_Indirizzi_Collaboratori indirizzoCollaboratore, Anag_Utenti utente)
        {
            Esito esito = Anag_Indirizzi_Collaboratori_DAL.Instance.AggiornaIndirizziCollaboratore(indirizzoCollaboratore, utente);

            return esito;
        }

        public Esito EliminaIndirizziCollaboratore(int idIndirizzoCollaboratore, Anag_Utenti utente)
        {
            Esito esito = Anag_Indirizzi_Collaboratori_DAL.Instance.EliminaIndirizzoCollaboratore(idIndirizzoCollaboratore, utente);

            return esito;
        }

    }
}