using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;
namespace VideoSystemWeb.BLL
{
    public class Anag_Collaboratori_BLL
    {
        //singleton
        private static volatile Anag_Collaboratori_BLL instance;
        private static object objForLock = new Object();
        private Anag_Collaboratori_BLL() { }
        public static Anag_Collaboratori_BLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Anag_Collaboratori_BLL();
                    }
                }
                return instance;
            }
        }
        public int CreaCollaboratore(Anag_Collaboratori collaboratore, Anag_Utenti utente, ref Esito esito)
        {
            int iREt = Anag_Collaboratori_DAL.Instance.CreaCollaboratore(collaboratore, utente, ref esito);

            return iREt;
        }

        public Esito AggiornaCollaboratore(Anag_Collaboratori collaboratore, Anag_Utenti utente)
        {
            Esito esito = Anag_Collaboratori_DAL.Instance.AggiornaCollaboratore(collaboratore, utente);

            return esito;
        }

        public Esito EliminaCollaboratore(int idCollaboratore, Anag_Utenti utente)
        {
            Esito esito = Anag_Collaboratori_DAL.Instance.EliminaCollaboratore(idCollaboratore, utente);

            return esito;
        }

        public Anag_Collaboratori getCollaboratoreById(int idCollaboratore,ref Esito esito)
        {
            Anag_Collaboratori collaboratore = Anag_Collaboratori_DAL.Instance.getCollaboratoreById(idCollaboratore, ref esito);
            return collaboratore;
        }
    }
}