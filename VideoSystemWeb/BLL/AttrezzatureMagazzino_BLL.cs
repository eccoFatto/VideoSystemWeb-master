using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;
using System.Configuration;
namespace VideoSystemWeb.BLL
{
    public class AttrezzatureMagazzino_BLL
    {
        //singleton
        private static volatile AttrezzatureMagazzino_BLL instance;
        private static object objForLock = new Object();
        private AttrezzatureMagazzino_BLL() { }
        public static AttrezzatureMagazzino_BLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new AttrezzatureMagazzino_BLL();
                    }
                }
                return instance;
            }
        }

        public AttrezzatureMagazzino getAttrezzaturaById(ref Esito esito, int id)
        {
            AttrezzatureMagazzino attrezzaturaREt = AttrezzatureMagazzino_DAL.Instance.getAttrezzaturaById(ref esito,id);

            return attrezzaturaREt;
        }

        public int CreaAttrezzatura(AttrezzatureMagazzino attrezzatura, ref Esito esito)
        {
            int iREt = AttrezzatureMagazzino_DAL.Instance.CreaAttrezzatura(attrezzatura, ref esito);

            return iREt;
        }

        public Esito AggiornaAttrezzatura(AttrezzatureMagazzino attrezzatura)
        {
            Esito esito = AttrezzatureMagazzino_DAL.Instance.AggiornaAttrezzatura(attrezzatura);

            return esito;
        }

        public Esito EliminaProtocollo(int idAttrezzatura)
        {
            Esito esito = AttrezzatureMagazzino_DAL.Instance.EliminaAttrezzatura(idAttrezzatura);

            return esito;
        }

    }
}