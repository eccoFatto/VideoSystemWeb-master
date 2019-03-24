using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;
namespace VideoSystemWeb.BLL
{
    public class Protocolli_BLL
    {
        //singleton
        private static volatile Protocolli_BLL instance;
        private static object objForLock = new Object();
        private Protocolli_BLL() { }
        public static Protocolli_BLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Protocolli_BLL();
                    }
                }
                return instance;
            }
        }

        public Protocolli getProtocolloById(ref Esito esito, int id)
        {
            Protocolli protocolloREt = Protocolli_DAL.Instance.getProtocolloById(ref esito,id);

            return protocolloREt;
        }

        public int CreaProtocollo(Protocolli protocollo, ref Esito esito)
        {
            int iREt = Protocolli_DAL.Instance.CreaProtocollo(protocollo, ref esito);

            return iREt;
        }

        public Esito AggiornaProtocollo(Protocolli protocollo)
        {
            Esito esito = Protocolli_DAL.Instance.AggiornaProtocollo(protocollo);

            return esito;
        }

        public Esito EliminaProtocollo(int idProtocollo)
        {
            Esito esito = Protocolli_DAL.Instance.EliminaProtocollo(idProtocollo);

            return esito;
        }

    }
}