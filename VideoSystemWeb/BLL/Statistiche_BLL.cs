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

        
        //public List<StatisticheCosti> GetStatisticheCosti_NomeLavorazione(string filtroCliente, string filtroProduzione, string filtroLavorazione, string filtroContratto, string filtroGenere, string filtroGruppo, string filtroSottogruppo, bool? fatturato, string dataInizio, string dataFine, string filtroFornitore, ref Esito esito)
        public List<StatisticheCosti> GetStatisticheCosti_NomeLavorazione(string filtroCliente, string filtroProduzione, string filtroLavorazione, string filtroContratto, string filtroGenere, string filtroGruppo, string filtroSottogruppo, string filtroCodiceLavorazione, string dataInizio, string dataFine, string filtroFornitore, ref Esito esito)
        {
            List<StatisticheCosti> listaStatisticheCosti = new List<StatisticheCosti>();

            string filtriLavorazione = string.Empty;
            filtriLavorazione += string.IsNullOrWhiteSpace(filtroCliente) ? "" : " AND b.ragioneSociale like '%" + filtroCliente + "%' ";
            filtriLavorazione += string.IsNullOrWhiteSpace(filtroProduzione) ? "" : " AND a.produzione like '%" + filtroProduzione + "%' ";
            filtriLavorazione += string.IsNullOrWhiteSpace(filtroLavorazione) ? "" : " AND a.lavorazione like '%" + filtroLavorazione + "%' ";
            filtriLavorazione += string.IsNullOrWhiteSpace(filtroContratto) ? "" : " AND f.descrizione like '%" + filtroContratto + "%' ";

            //if (fatturato != null)
            //{
            //    filtriLavorazione += (bool)fatturato ? "and e.protocollo_riferimento is not null " : "and e.protocollo_riferimento is null ";
            //}
            filtriLavorazione += string.IsNullOrWhiteSpace(filtroCodiceLavorazione) ? "" : " AND a.codice_lavoro = '" + filtroCodiceLavorazione + "' ";

            filtriLavorazione += string.IsNullOrWhiteSpace(dataInizio) ? "" : " AND a.data_inizio_lavorazione >= '" + dataInizio.Substring(6) + "-" + dataInizio.Substring(3, 2) + "-" + dataInizio.Substring(0, 2) + "T00:00:00.000'";
            // il filtro dataFine viene eseguito su dataInizioLavorazione, e non su dataFineLavorazione
            filtriLavorazione += string.IsNullOrWhiteSpace(dataFine) ? "" : " AND a.data_inizio_lavorazione <= '" + dataFine.Substring(6) + "-" + dataFine.Substring(3, 2) + "-" + dataFine.Substring(0, 2) + "T00:00:00.000'";


            string filtriCosti = string.Empty; ; // i filtri seguenti devono essere applicati solo alle categorie costi

            filtriCosti += string.IsNullOrWhiteSpace(filtroGenere) ? "" : " AND d.idTipoGenere = '" + filtroGenere + "' ";
            filtriCosti += string.IsNullOrWhiteSpace(filtroGruppo) ? "" : " AND d.idTipoGruppo = '" + filtroGruppo + "' ";
            filtriCosti += string.IsNullOrWhiteSpace(filtroSottogruppo) ? "" : " AND d.idTipoSottogruppo = '" + filtroSottogruppo + "' ";
            filtriCosti += string.IsNullOrWhiteSpace(filtroFornitore) ? "" : "AND forn.ragioneSociale like '%" + filtroFornitore + "%' ";



            if (string.IsNullOrEmpty(filtriCosti))
            {
                listaStatisticheCosti = Statistiche_DAL.Instance.GetStatisticheCosti(filtriLavorazione, ref esito); 
            }
            else
            {
                listaStatisticheCosti = Statistiche_DAL.Instance.GetStatisticheCosti(filtriLavorazione, filtriCosti, ref esito);

                if (listaStatisticheCosti.Count > 0)
                {
                    List<string> listaLavorazioni = listaStatisticheCosti.GroupBy(p => p.CodiceLavoro).Select(g => g.FirstOrDefault()).Select(c => c.CodiceLavoro).ToList();
                    string elencoLavorazioni = string.Empty;
                    foreach (string codLavorazione in listaLavorazioni)
                    {
                        elencoLavorazioni += "'" + codLavorazione.Trim() + "', ";
                    }
                    elencoLavorazioni = elencoLavorazioni.Substring(0, elencoLavorazioni.Length - 2);


                    List<StatisticheCosti> listaStatisticheListino = Statistiche_DAL.Instance.GetStatisticheCostiListino(elencoLavorazioni, ref esito);

                    listaStatisticheCosti.AddRange(listaStatisticheListino);

                    listaStatisticheCosti = listaStatisticheCosti.OrderBy(x => x.Cliente).ThenBy(y => y.CodiceLavoro).ThenBy(z=>z.Progressivo).ToList();
                }
            }


            return listaStatisticheCosti;    
        }


        public List<StatisticheCosti> GetStatisticheCosti_CodiceLavorazione(string filtroCliente, string filtroProduzione, string filtroLavorazione, string filtroContratto, string filtroGenere, string filtroGruppo, string filtroSottogruppo, bool? fatturato, string dataInizio, string dataFine, string filtroFornitore, ref Esito esito)
        {
            //return Statistiche_DAL.Instance.GetStatisticheCosti(filtriLavorazione, filtriCosti, ref esito); //(filtroCliente, filtroProduzione, filtroLavorazione, filtroContratto, filtroGenere, filtroGruppo, filtroSottogruppo, fatturato, dataInizio, dataFine, filtroFornitore, ref esito);


            List<StatisticheCosti> listaStatisticheCosti = new List<StatisticheCosti>();

            string filtriLavorazione = string.Empty;
            filtriLavorazione += string.IsNullOrWhiteSpace(filtroCliente) ? "" : " AND b.ragioneSociale like '%" + filtroCliente + "%' ";
            filtriLavorazione += string.IsNullOrWhiteSpace(filtroProduzione) ? "" : " AND a.produzione like '%" + filtroProduzione + "%' ";
            filtriLavorazione += string.IsNullOrWhiteSpace(filtroLavorazione) ? "" : " AND a.codice_lavoro = '" + filtroLavorazione + "' ";
            //filtriLavorazione += string.IsNullOrWhiteSpace(filtroLavorazione) ? "" : " AND a.lavorazione like '%" + filtroLavorazione + "%' ";
            filtriLavorazione += string.IsNullOrWhiteSpace(filtroContratto) ? "" : " AND f.descrizione like '%" + filtroContratto + "%' ";

            if (fatturato != null)
            {
                filtriLavorazione += (bool)fatturato ? "and e.protocollo_riferimento is not null " : "and e.protocollo_riferimento is null ";
            }
            filtriLavorazione += string.IsNullOrWhiteSpace(dataInizio) ? "" : " AND a.data_inizio_lavorazione >= '" + dataInizio.Substring(6) + "-" + dataInizio.Substring(3, 2) + "-" + dataInizio.Substring(0, 2) + "T00:00:00.000'";
            // il filtro dataFine viene eseguito su dataInizioLavorazione, e non su dataFineLavorazione
            filtriLavorazione += string.IsNullOrWhiteSpace(dataFine) ? "" : " AND a.data_inizio_lavorazione <= '" + dataFine.Substring(6) + "-" + dataFine.Substring(3, 2) + "-" + dataFine.Substring(0, 2) + "T00:00:00.000'";


            string filtriCosti = string.Empty; ; // i filtri seguenti devono essere applicati solo alle categorie costi

            filtriCosti += string.IsNullOrWhiteSpace(filtroGenere) ? "" : " AND d.idTipoGenere = '" + filtroGenere + "' ";
            filtriCosti += string.IsNullOrWhiteSpace(filtroGruppo) ? "" : " AND d.idTipoGruppo = '" + filtroGruppo + "' ";
            filtriCosti += string.IsNullOrWhiteSpace(filtroSottogruppo) ? "" : " AND d.idTipoSottogruppo = '" + filtroSottogruppo + "' ";
            filtriCosti += string.IsNullOrWhiteSpace(filtroFornitore) ? "" : "AND forn.ragioneSociale like '%" + filtroFornitore + "%' ";



            if (string.IsNullOrEmpty(filtriCosti))
            {
                listaStatisticheCosti = Statistiche_DAL.Instance.GetStatisticheCosti(filtriLavorazione, ref esito);
            }
            else
            {
                listaStatisticheCosti = Statistiche_DAL.Instance.GetStatisticheCosti(filtriLavorazione, filtriCosti, ref esito);

                if (listaStatisticheCosti.Count > 0)
                {
                    List<string> listaLavorazioni = listaStatisticheCosti.GroupBy(p => p.CodiceLavoro).Select(g => g.FirstOrDefault()).Select(c => c.CodiceLavoro).ToList();
                    string elencoLavorazioni = string.Empty;
                    foreach (string codLavorazione in listaLavorazioni)
                    {
                        elencoLavorazioni += "'" + codLavorazione.Trim() + "', ";
                    }
                    elencoLavorazioni = elencoLavorazioni.Substring(0, elencoLavorazioni.Length - 2);


                    List<StatisticheCosti> listaStatisticheListino = Statistiche_DAL.Instance.GetStatisticheCostiListino(elencoLavorazioni, ref esito);

                    listaStatisticheCosti.AddRange(listaStatisticheListino);

                    listaStatisticheCosti = listaStatisticheCosti.OrderBy(x => x.Cliente).ThenBy(y => y.CodiceLavoro).ThenBy(z => z.Progressivo).ToList();
                }
            }


            return listaStatisticheCosti;
        }
    }
}