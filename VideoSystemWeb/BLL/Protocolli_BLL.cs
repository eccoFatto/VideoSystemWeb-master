using System;
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

        public Protocolli getProtocolloById(ref Esito esito, Int64 id)
        {
            Protocolli protocolloREt = Protocolli_DAL.Instance.getProtocolloById(ref esito,id);

            return protocolloREt;
        }

        public List<Protocolli> getProtocolliByCodLavIdTipoProtocollo(string codiceLavorazione, int IdTipoProtocollo, ref Esito esito, bool soloAttivi = true)
        {
            List<Protocolli> listaProtocolli = Protocolli_DAL.Instance.getProtocolliByCodLavIdTipoProtocollo(codiceLavorazione, IdTipoProtocollo, ref esito, soloAttivi);

            return listaProtocolli;
        }

        public List<Protocolli> GetProtocolliByIdCliente(ref Esito esito, int idCliente, bool soloAttivi = true)
        {
            List<Protocolli> listaProtocolli = Protocolli_DAL.Instance.GetProtocolliByIdCliente(ref esito, idCliente, soloAttivi);

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

        public Esito RemoveProtocollo(int idProtocollo)
        {
            Esito esito = Protocolli_DAL.Instance.RemoveProtocollo(idProtocollo);

            return esito;
        }

        public int getProtocollo(ref Esito esito)
        {
            int iREt = Base_DAL.GetProtocollo(ref esito);

            return iREt;
        }

        public string getNumeroFattura()
        {
            Esito esito = new Esito();
            string sREt = Base_DAL.GetNumeroFattura(ref esito);

            return sREt;
        }

        public Esito resetNumeroFattura(int annoFattura, int numeroFattura)
        {
            Esito esito = Base_DAL.ResetNumeroFattura(annoFattura, numeroFattura);

            return esito;
        }

        public Esito resetProcotollo(int protocolloIniziale)
        {
            Esito esito = Base_DAL.ResetProtocollo(protocolloIniziale);

            return esito;
        }

        public string getNumeroProtocollo()
        {
            string ret = "";

            ret = ConfigurationManager.AppSettings["NUMERO_PROTOCOLLO"];
            Esito esito = new Esito();
            int nProt = getProtocollo(ref esito);
            if (esito.Codice == Esito.ESITO_OK)
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
            int iREt = Base_DAL.GetCodiceLavorazione(ref esito);

            return iREt;
        }

        public Esito resetCodiceLavorazione(int codiceLavorazioneIniziale)
        {
            Esito esito = Base_DAL.ResetCodiceLavorazione(codiceLavorazioneIniziale);

            return esito;
        }

        public string getCodLavFormattato()
        {
            string ret = "";

            ret = ConfigurationManager.AppSettings["CODICE_LAVORAZIONE"];
            Esito esito = new Esito();
            int codLav = getCodiceLavorazione(ref esito);
            if (esito.Codice == Esito.ESITO_OK)
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

        public List<string> GetElencoClientiFornitori(ref Esito esito)
        {
            return Protocolli_DAL.Instance.GetElencoClientiFornitori(ref esito);
        }

        public List<string> GetElencoProduzioni(ref Esito esito)
        {
            return Protocolli_DAL.Instance.GetElencoProduzioni(ref esito);
        }

        public List<string> GetElencoLavorazioni(ref Esito esito)
        {
            return Protocolli_DAL.Instance.GetElencoLavorazioni(ref esito);
        }

        public Esito EliminaFattura(int idProtocollo, string numeroFattura)
        {
            Esito esito = Protocolli_DAL.Instance.EliminaFattura(idProtocollo, numeroFattura);

            return esito;
        }
    }
}