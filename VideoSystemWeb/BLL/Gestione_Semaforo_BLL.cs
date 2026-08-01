using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;

namespace VideoSystemWeb.BLL
{
    public class Gestione_Semaforo_BLL
    {
        #region SINGLETON
        private static volatile Gestione_Semaforo_BLL instance;
        private static object objForLock = new Object();
        private Gestione_Semaforo_BLL() { }
        public static Gestione_Semaforo_BLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Gestione_Semaforo_BLL();
                    }
                }
                return instance;
            }
        }
        #endregion  

        public bool IsAccessoLavorazioneBloccato(int idAgenda, out Tab_Semaforo_Lavorazioni semaforo, ref Esito esito)
        {
            return Gestione_Semaforo_DAL.Instance.IsAccessoLavorazioneBloccato(idAgenda, out semaforo, ref esito);
        }

        public Esito InserisciAccessoLavorazione(Tab_Semaforo_Lavorazioni semaforo)
        {
            return Gestione_Semaforo_DAL.Instance.InserisciAccessoLavorazione(semaforo);
        }

        public Esito ModificaAccessoLavorazione(Tab_Semaforo_Lavorazioni semaforo)
        {
            return Gestione_Semaforo_DAL.Instance.ModificaAccessoLavorazione(semaforo);
        }

        public Esito EliminaAccessoLavorazione(int idAgenda)
        {
            return Gestione_Semaforo_DAL.Instance.EliminaAccessoLavorazione(idAgenda);
        }
    }
}