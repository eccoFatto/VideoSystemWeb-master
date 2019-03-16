using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;
namespace VideoSystemWeb.BLL
{
    public class Art_Gruppi_BLL
    {
        //singleton
        private static volatile Art_Gruppi_BLL instance;
        private static object objForLock = new Object();
        private Art_Gruppi_BLL() { }
        public static Art_Gruppi_BLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Art_Gruppi_BLL();
                    }
                }
                return instance;
            }
        }

        public List<Art_Gruppi> CaricaListaGruppi(ref Esito esito, bool soloAttivi = true)
        {
            List<Art_Gruppi> listaGruppi = Art_Gruppi_DAL.Instance.CaricaListaGruppi(ref esito, soloAttivi);
            return listaGruppi;
        }

        public Art_Gruppi getGruppiById(int idGruppo, ref Esito esito)
        {
            Art_Gruppi articolo = Art_Gruppi_DAL.Instance.getGruppiById(idGruppo, ref esito);
            return articolo;
        }

        public int CreaGruppo(Art_Gruppi gruppo, Anag_Utenti utente, ref Esito esito)
        {
            int iREt = Art_Gruppi_DAL.Instance.CreaGruppo(gruppo, utente, ref esito);

            return iREt;
        }

        public Esito AggiornaGruppo(Art_Gruppi gruppo, Anag_Utenti utente)
        {
            Esito esito = Art_Gruppi_DAL.Instance.AggiornaGruppo(gruppo, utente);

            return esito;
        }

        public Esito EliminaGruppo(int idGruppo, Anag_Utenti utente)
        {
            Esito esito = Art_Gruppi_DAL.Instance.EliminaGruppo(idGruppo, utente);

            return esito;
        }
    }
}