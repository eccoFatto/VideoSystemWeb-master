using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;
namespace VideoSystemWeb.BLL
{
    public class Anag_Qualifiche_Collaboratori_BLL
    {
        //singleton
        private static volatile Anag_Qualifiche_Collaboratori_BLL instance;
        private static object objForLock = new Object();
        private Anag_Qualifiche_Collaboratori_BLL() { }
        public static Anag_Qualifiche_Collaboratori_BLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Anag_Qualifiche_Collaboratori_BLL();
                    }
                }
                return instance;
            }
        }

        public List<Anag_Qualifiche_Collaboratori> getAllQualifiche(ref Esito esito, bool soloAttivi = true)
        {
            return Anag_Qualifiche_Collaboratori_DAL.Instance.getAllQualifiche(ref esito, true);
        }

        public int CreaQualificaCollaboratore(Anag_Qualifiche_Collaboratori qualificaCollaboratore, Anag_Utenti utente, ref Esito esito)
        {
            int iREt = Anag_Qualifiche_Collaboratori_DAL.Instance.CreaQualificaCollaboratore(qualificaCollaboratore, utente, ref esito);

            return iREt;
        }

        public Esito AggiornaQualificaCollaboratore(Anag_Qualifiche_Collaboratori qualificaCollaboratore, Anag_Utenti utente)
        {
            Esito esito = Anag_Qualifiche_Collaboratori_DAL.Instance.AggiornaQualificaCollaboratore(qualificaCollaboratore, utente);

            return esito;
        }

        public Esito EliminaQualificaCollaboratore(int idQualificaCollaboratore, Anag_Utenti utente)
        {
            Esito esito = Anag_Qualifiche_Collaboratori_DAL.Instance.EliminaQualificaCollaboratore(idQualificaCollaboratore, utente);

            return esito;
        }

    }
}