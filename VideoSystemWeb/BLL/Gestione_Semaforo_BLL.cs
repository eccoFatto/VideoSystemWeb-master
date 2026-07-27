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

        public bool IsAccessoLavorazioneBloccato(ref Tab_Semaforo_Lavorazioni semaforo, ref Esito esito)
        {
            return Gestione_Semaforo_DAL.Instance.IsAccessoLavorazioneBloccato(ref semaforo, ref esito);
        }

        public Esito InserisciAccessoLavorazione(ref Tab_Semaforo_Lavorazioni semaforo)
        {
            return Gestione_Semaforo_DAL.Instance.InserisciAccessoLavorazione(ref semaforo);
        }

        public Esito ModificaAccessoLavorazione(ref Tab_Semaforo_Lavorazioni semaforo)
        {
            return Gestione_Semaforo_DAL.Instance.ModificaAccessoLavorazione(ref semaforo);
        }

        public Esito BloccaAccessoLavorazione(ref Tab_Semaforo_Lavorazioni semaforo)
        {
            return Gestione_Semaforo_DAL.Instance.BloccaAccessoLavorazione(ref semaforo);
        }

        public Esito EliminaAccessoLavorazione(ref Tab_Semaforo_Lavorazioni semaforo)
        {
            return Gestione_Semaforo_DAL.Instance.EliminaAccessoLavorazione(ref semaforo);
        }
    }
}