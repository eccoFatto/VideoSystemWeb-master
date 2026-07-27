using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.BLL;
using VideoSystemWeb.Entity;

namespace VideoSystemWeb.DAL
{
    public class Gestione_Semaforo_DAL
    {
        #region SINGLETON
        private static volatile Gestione_Semaforo_DAL instance;
        private static object objForLock = new Object();
        private Gestione_Semaforo_DAL() { }
        public static Gestione_Semaforo_DAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Gestione_Semaforo_DAL();
                    }
                }
                return instance;
            }
        }
        #endregion  

        public bool IsAccessoLavorazioneBloccato(ref Tab_Semaforo_Lavorazioni semaforo, ref Esito esito)
        {
            return true;
        }

        public Esito InserisciAccessoLavorazione(ref Tab_Semaforo_Lavorazioni semaforo)
        {
            return new Esito();
        }

        public Esito ModificaAccessoLavorazione(ref Tab_Semaforo_Lavorazioni semaforo)
        {
            return new Esito();
        }

        public Esito BloccaAccessoLavorazione(ref Tab_Semaforo_Lavorazioni semaforo)
        {
            return new Esito();
        }

        public Esito EliminaAccessoLavorazione(ref Tab_Semaforo_Lavorazioni semaforo)
        {
            return new Esito();
        }
    }
}