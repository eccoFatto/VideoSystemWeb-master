using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;
namespace VideoSystemWeb.BLL
{
    public class Anag_Email_Collaboratori_BLL
    {
        //singleton
        private static volatile Anag_Email_Collaboratori_BLL instance;
        private static object objForLock = new Object();
        private Anag_Email_Collaboratori_BLL() { }
        public static Anag_Email_Collaboratori_BLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Anag_Email_Collaboratori_BLL();
                    }
                }
                return instance;
            }
        }

        public List<Anag_Email_Collaboratori> getEmailByIdCollaboratore(int idEmailCollaboratore, ref Esito esito)
        {
            List<Anag_Email_Collaboratori> listretEmailCollaboratore = Anag_Email_Collaboratori_DAL.Instance.getEmailByIdCollaboratore(ref esito, idEmailCollaboratore, true);
            return listretEmailCollaboratore;
        }

        public Anag_Email_Collaboratori getEmailById(int id, ref Esito esito)
        {
            Anag_Email_Collaboratori retEmailCollaboratore = Anag_Email_Collaboratori_DAL.Instance.getEmailById(ref esito, id);
            return retEmailCollaboratore;
        }

        public int CreaEmailCollaboratore(Anag_Email_Collaboratori emailCollaboratore, Anag_Utenti utente, ref Esito esito)
        {
            int iREt = Anag_Email_Collaboratori_DAL.Instance.CreaEmailCollaboratore(emailCollaboratore,utente, ref esito);

            return iREt;
        }

        public Esito AggiornaEmailCollaboratore(Anag_Email_Collaboratori emailCollaboratore, Anag_Utenti utente)
        {
            Esito esito = Anag_Email_Collaboratori_DAL.Instance.AggiornaEmailCollaboratore(emailCollaboratore,utente);

            return esito;
        }

        public Esito EliminaEmailCollaboratore(int idEmailCollaboratore, Anag_Utenti utente)
        {
            Esito esito = Anag_Email_Collaboratori_DAL.Instance.EliminaEmailCollaboratore(idEmailCollaboratore,utente);

            return esito;
        }
    }
}