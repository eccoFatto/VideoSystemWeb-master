﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;
using System.Configuration;
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

        public List<Protocolli> getProtocolliByCodLavIdTipoProtocollo(string codiceLavorazione, int IdTipoProtocollo, ref Esito esito, bool soloAttivi = true)
        {
            List<Protocolli> listaProtocolli = Protocolli_DAL.Instance.getProtocolliByCodLavIdTipoProtocollo(codiceLavorazione, IdTipoProtocollo, ref esito, soloAttivi);

            return listaProtocolli;
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