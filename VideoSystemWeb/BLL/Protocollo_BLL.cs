﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;
using System.Configuration;
namespace VideoSystemWeb.BLL
{
    public class Protocollo_BLL
    {
        //singleton
        private static volatile Protocollo_BLL instance;
        private static object objForLock = new Object();
        private Protocollo_BLL() { }
        public static Protocollo_BLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Protocollo_BLL();
                    }
                }
                return instance;
            }
        }

         public int getProtocollo(ref Esito esito)
        {
            int iREt = Base_DAL.getProtocollo(ref esito);

            return iREt;
        }


        public Esito resetProcotollo(int protocolloIniziale)
        {
            Esito esito = Base_DAL.resetProtocollo(protocolloIniziale);

            return esito;
        }

        public string getNumeroProtocollo()
        {
            string ret = "";

            ret = ConfigurationManager.AppSettings["NUMERO_PROTOCOLLO"];
            Esito esito = new Esito();
            int nProt = getProtocollo(ref esito);
            if (esito.codice == Esito.ESITO_OK)
            {
                ret = ret.Replace("@anno", DateTime.Today.Year.ToString("0000"));
                ret = ret.Replace("@protocollo", nProt.ToString(ConfigurationManager.AppSettings["FORMAT_NUMERO_PROTOCOLLO"]));
            }
            else
            {
                ret = "";
            }
            return ret;
        }
        public int getCodiceLavorazione(ref Esito esito)
        {
            int iREt = Base_DAL.getCodiceLavorazione(ref esito);

            return iREt;
        }

        public Esito resetCodiceLavorazione(int codiceLavorazioneIniziale)
        {
            Esito esito = Base_DAL.resetCodiceLavorazione(codiceLavorazioneIniziale);

            return esito;
        }

        public string getCodLavFormattato()
        {
            string ret = "";

            ret = ConfigurationManager.AppSettings["CODICE_LAVORAZIONE"];
            Esito esito = new Esito();
            int codLav = getCodiceLavorazione(ref esito);
            if (esito.codice == Esito.ESITO_OK)
            {
                ret = ret.Replace("@anno", DateTime.Today.Year.ToString("0000"));
                ret = ret.Replace("@codiceLavorazione", codLav.ToString(ConfigurationManager.AppSettings["FORMAT_CODICE_LAVORAZIONE"]));
            }
            else
            {
                ret = "";
            }
            return ret;
        }

    }
}