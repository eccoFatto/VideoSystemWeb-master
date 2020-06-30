using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;

namespace VideoSystemWeb.BLL
{
    public class Statistiche_BLL
    {
        #region SINGLETON
        private static volatile Statistiche_BLL instance;
        private static object objForLock = new Object();

        private Statistiche_BLL() { }

        public static Statistiche_BLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Statistiche_BLL();
                    }
                }
                return instance;
            }
        }
        #endregion

        public List<string> CaricaElencoProduzioni(string filtroCliente, ref Esito esito)
        {
            return Agenda_DAL.Instance.CaricaElencoProduzioni(filtroCliente, ref esito);
        }

        public List<DatiLavorazione> CaricaElencoLavorazioniECodice(string filtroCliente, string filtroLavorazione, ref Esito esito)
        {
            List<DatiAgenda> listaLavorazioniInAgenda = Agenda_DAL.Instance.CaricaElencoLavorazioni(filtroCliente, filtroLavorazione, ref esito).Where(x => !string.IsNullOrEmpty(x.codice_lavoro)).ToList<DatiAgenda>();
            List<DatiLavorazione> listaLavorazioni = listaLavorazioniInAgenda.Select(x => new DatiLavorazione
            {
                DescrizioneLavorazione = x.lavorazione,
                CodiceLavorazione = x.codice_lavoro
            }).ToList();

            return listaLavorazioni;
        }

        public List<Protocolli> GetAllContratti(string filtroCliente, string filtroProduzione, string filtroLavorazione, ref Esito esito, bool soloAttivi = true)
        {
            Tipologica tipologicaContratto = SessionManager.ListaTipiProtocolli.FirstOrDefault(x => x.nome.ToLower() == "contratto");
            int idContratto = tipologicaContratto.id;

            List<Protocolli> listaContratti = Protocolli_DAL.Instance.getProtocolli(filtroCliente, filtroProduzione, filtroLavorazione, ref esito, soloAttivi).Where(x => x.Id_tipo_protocollo == idContratto).ToList(); ;

            return listaContratti;
        }

        public List<StatisticheRicavi> GetStatisticheRicavi(string filtroCliente, string filtroProduzione, string filtroLavorazione, string filtroContratto, bool? fatturato, string dataInizio, string dataFine, ref Esito esito)
        { 
            return Statistiche_DAL.Instance.GetStatisticheRicavi(filtroCliente, filtroProduzione, filtroLavorazione, filtroContratto, fatturato, dataInizio, dataFine, ref esito);
        }

        public List<StatisticheCosti> GetStatisticheCosti(string filtroCliente, string filtroProduzione, string filtroLavorazione, string filtroContratto, string filtroGenere, string filtroGruppo, string filtroSottogruppo, bool? fatturato, string dataInizio, string dataFine, string filtroFornitore, ref Esito esito)
        {
            return Statistiche_DAL.Instance.GetStatisticheCosti(filtroCliente, filtroProduzione, filtroLavorazione, filtroContratto, filtroGenere, filtroGruppo, filtroSottogruppo, fatturato, dataInizio, dataFine, filtroFornitore, ref esito);
        }
    }
}