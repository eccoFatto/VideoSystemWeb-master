using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;
namespace VideoSystemWeb.BLL
{
    public class Anag_Utenti_BLL
    {
        //singleton
        private static volatile Anag_Utenti_BLL instance;
        private static object objForLock = new Object();
        private Anag_Utenti_BLL() { }
        public static Anag_Utenti_BLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Anag_Utenti_BLL();
                    }
                }
                return instance;
            }
        }
        public int CreaUtente(Utenti utente, ref Esito esito)
        {
            int iREt = Anag_Utenti_DAL.Instance.CreaUtente(utente, ref esito);

            return iREt;
        }

        public Esito AggiornaUtente(Utenti utente)
        {
            Esito esito = Anag_Utenti_DAL.Instance.AggiornaUtente(utente);

            return esito;
        }

        public Esito EliminaUtente(int idUtente)
        {
            Esito esito = Anag_Utenti_DAL.Instance.EliminaUtente(idUtente);

            return esito;
        }

        public Utenti getUtenteById(int idUtente,ref Esito esito)
        {
            Utenti utente = Anag_Utenti_DAL.Instance.getUtenteById(idUtente, ref esito);
            return utente;
        }
    }
}