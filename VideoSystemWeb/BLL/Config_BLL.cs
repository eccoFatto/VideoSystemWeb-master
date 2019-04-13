using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;
using System.Configuration;
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

    }
}