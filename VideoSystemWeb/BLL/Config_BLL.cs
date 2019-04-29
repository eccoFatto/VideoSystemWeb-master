using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;
using System.Configuration;
using System.Data;
namespace VideoSystemWeb.BLL
{
    public class Config_BLL
    {
        //singleton
        private static volatile Config_BLL instance;
        private static object objForLock = new Object();
        private Config_BLL() { }
        public static Config_BLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Config_BLL();
                    }
                }
                return instance;
            }
        }

        public Config getConfig(ref Esito esito, string chiave)
        {
            Config configRet = Config_DAL.Instance.getConfig(ref esito, chiave);

            return configRet;
        }

        public List<Config> getListaConfig(ref Esito esito)
        {
            List<Config> listaConfig = Config_DAL.Instance.getListaConfig(ref esito);

            return listaConfig;
        }

        public Esito CreaConfig(Config config)
        {
            Esito esito = Config_DAL.Instance.CreaConfig(config);

            return esito;
        }

        public Esito AggiornaConfig(Config config)
        {
            Esito esito = Config_DAL.Instance.AggiornaConfig(config);

            return esito;
        }

        public Esito EliminaConfig(string chiave)
        {
            Esito esito = Config_DAL.Instance.EliminaConfig(chiave);

            return esito;
        }

        public List<DatiBancari> getListaDatiBancari(ref Esito esito)
        {
            List<DatiBancari> listaDatiBancari = new List<DatiBancari>();
            string query = "select * from tab_config"+
            " where chiave like 'BANCA%' AND valore <> '' AND valore is not null"+
            " order by ordinamento";
            DataTable dtBanche = Base_DAL.getDatiBySql(query, ref esito);
            if (esito.codice==0 && dtBanche!=null && dtBanche.Rows!=null && dtBanche.Rows.Count > 0)
            {
                foreach (DataRow rigaBanca in dtBanche.Rows)
                {
                    DatiBancari datiBancari = new DatiBancari();
                    datiBancari.Banca = rigaBanca["valore"].ToString();
                    Config cfg = getConfig(ref esito, "Iban" + rigaBanca["chiave"].ToString().Substring(rigaBanca["chiave"].ToString().Length-2));
                    if (esito.codice == 0)
                    {
                        datiBancari.Iban = cfg.valore;
                        listaDatiBancari.Add(datiBancari);
                    }
                }
            }
            return listaDatiBancari;
        }

        public List<GiorniPagamentoFatture> getListaGiorniPagamentoFatture(ref Esito esito)
        {
            List<GiorniPagamentoFatture> listaGiorniPagamentoFatture = new List<GiorniPagamentoFatture>();

            Config cfg = getConfig(ref esito, "GIORNI_PAGAMENTO");

            if (esito.codice == 0) { 
                string sGiorniEsito = cfg.valore;
                char[] separator = { '-' };
                string[] arGiorniEsito = sGiorniEsito.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < arGiorniEsito.Length; i++)
                {
                    GiorniPagamentoFatture gpf = new GiorniPagamentoFatture();
                    gpf.Giorni = arGiorniEsito[i];
                    gpf.Descrizione = gpf.Giorni + " Giorni";
                    listaGiorniPagamentoFatture.Add(gpf);
                }
            }

            return listaGiorniPagamentoFatture;
        }

    }
}