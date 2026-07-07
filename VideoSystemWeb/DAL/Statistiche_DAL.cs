using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using VideoSystemWeb.BLL;
using VideoSystemWeb.Entity;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace VideoSystemWeb.DAL
{
    public class Statistiche_DAL : Base_DAL
    {
        #region SINGLETON
        private static volatile Statistiche_DAL instance;
        private static object objForLock = new Object();

        private Statistiche_DAL() { }

        public static Statistiche_DAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Statistiche_DAL();
                    }
                }
                return instance;
            }
        }
        #endregion

        #region STATISTICHE RICAVI
        public List<StatisticheRicavi> GetStatisticheRicavi_OLD(string filtroCliente, string filtroProduzione, string filtroLavorazione, string filtroContratto, bool? fatturato, string dataInizio, string dataFine, ref Esito esito)
        {
            List<StatisticheRicavi> listaStatisticheRicavi = new List<StatisticheRicavi>();
            HashSet<string> lavorazioniAnomale = new HashSet<string>();

            string filtri = string.Empty;
            filtri += string.IsNullOrWhiteSpace(filtroCliente) ? "" : " AND b.ragioneSociale like '%" + filtroCliente + "%' ";
            filtri += string.IsNullOrWhiteSpace(filtroProduzione) ? "" : " AND a.produzione like '%" + filtroProduzione + "%' ";
            filtri += string.IsNullOrWhiteSpace(filtroLavorazione) ? "" : " AND a.lavorazione like '%" + filtroLavorazione + "%' ";
            filtri += string.IsNullOrWhiteSpace(filtroContratto) ? "" : " AND f.descrizione like '%" + filtroContratto + "%' ";

            // eliminato campo docFattura in seguito alla segnalazione 02/07/2024 di risultati duplicati 
            string campiQuery = "select distinct a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, min(a.data_inizio_lavorazione) data, a.lavorazione, a.produzione, SUM(d.prezzo) listino, sum(d.fp_lordo)  costo, f.descrizione contratto, h.pathDocumento 'docOfferta', h.pregresso ";

            if (fatturato != null)
            {
                if ((bool)fatturato)
                {
                    filtri += "and e.protocollo_riferimento is not null ";

                    filtri += string.IsNullOrWhiteSpace(dataInizio) ? "" : " AND e.data_fattura >= '" + dataInizio.Substring(6) + "-" + dataInizio.Substring(3, 2) + "-" + dataInizio.Substring(0, 2) + "T00:00:00.000'";
                    filtri += string.IsNullOrWhiteSpace(dataFine) ? "" : " AND e.data_fattura <= '" + dataFine.Substring(6) + "-" + dataFine.Substring(3, 2) + "-" + dataFine.Substring(0, 2) + "T00:00:00.000'";

                    // eliminato campo docOfferta in seguito alla segnalazione 02/07/2024 di risultati duplicati 
                    campiQuery = "select distinct a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, min(e.data_fattura) data, a.lavorazione, a.produzione, SUM(d.prezzo) listino, sum(d.fp_lordo)  costo, f.descrizione contratto, e.pathDocumento 'docFattura', h.pregresso ";
                }
                else
                {
                    filtri += "and e.protocollo_riferimento is null ";

                    filtri += string.IsNullOrWhiteSpace(dataInizio) ? "" : " AND a.data_inizio_lavorazione >= '" + dataInizio.Substring(6) + "-" + dataInizio.Substring(3, 2) + "-" + dataInizio.Substring(0, 2) + "T00:00:00.000'";
                    // il filtro dataFine viene eseguito su dataInizioLavorazione, e non su dataFineLavorazione
                    filtri += string.IsNullOrWhiteSpace(dataFine) ? "" : " AND a.data_inizio_lavorazione <= '" + dataFine.Substring(6) + "-" + dataFine.Substring(3, 2) + "-" + dataFine.Substring(0, 2) + "T00:00:00.000'";
                }
            }
            else // Fatturato = <tutti>
            {
                filtri += string.IsNullOrWhiteSpace(dataInizio) ? "" : " AND ((e.protocollo_riferimento is null AND a.data_inizio_lavorazione >= '" + dataInizio.Substring(6) + "-" + dataInizio.Substring(3, 2) + "-" + dataInizio.Substring(0, 2) + "T00:00:00.000') OR (e.protocollo_riferimento is not null AND e.data_fattura >= '" + dataInizio.Substring(6) + "-" + dataInizio.Substring(3, 2) + "-" + dataInizio.Substring(0, 2) + "T00:00:00.000'))";
                // il filtro dataFine viene eseguito su dataInizioLavorazione, e non su dataFineLavorazione
                filtri += string.IsNullOrWhiteSpace(dataFine) ? "" : " AND (( e.protocollo_riferimento is null AND a.data_inizio_lavorazione <= '" + dataFine.Substring(6) + "-" + dataFine.Substring(3, 2) + "-" + dataFine.Substring(0, 2) + "T00:00:00.000') OR (e.protocollo_riferimento is not null AND e.data_fattura <= '" + dataFine.Substring(6) + "-" + dataFine.Substring(3, 2) + "-" + dataFine.Substring(0, 2) + "T00:00:00.000'))";
            }




            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query =  campiQuery +
                                    "from tab_dati_agenda a " +
                                    "left join anag_clienti_fornitori b on b.id = a.id_cliente " +
                                    "left join dati_lavorazione c on c.idDatiAgenda = a.id " +
                                    "left join dati_articoli_lavorazione d on d.idDatiLavorazione = c.id " +  
                                    "left join tipo_protocollo g on  g.nome = 'Fattura' " +
                                    "left join dati_protocollo e on e.codice_lavoro = a.codice_lavoro and e.attivo = 1 and e.id_tipo_protocollo = g.id and destinatario = 'Cliente' " +
                                    "left join tipo_protocollo i on  i.nome = 'Contratto' " +
                                    "left join dati_protocollo f on f.id=c.idContratto and f.id_tipo_protocollo = i.id " +
                                    "left join tipo_protocollo j on  j.nome = 'Offerta' " +
                                    "left join dati_protocollo h on h.codice_lavoro = a.codice_lavoro and h.id_tipo_protocollo = j.id and h.destinatario = 'Cliente' " + 


                                    "where a.codice_lavoro is not null and a.id_stato >= 3" + filtri +
                                    "group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, f.descrizione, e.pathDocumento, h.pathDocumento, h.pregresso";

                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                                {
                                    foreach (DataRow riga in dt.Rows)
                                    {
                                        try
                                        { 
                                        StatisticheRicavi statisticheRicavi = new StatisticheRicavi();

                                        statisticheRicavi.IdCliente = riga.Field<int>("id_cliente");
                                        statisticheRicavi.Cliente = riga.Field<string>("cliente");
                                        statisticheRicavi.NumeroFattura = riga.Field<string>("numeroFattura");
                                        statisticheRicavi.Ordine = riga.Field<string>("ordine");
                                        statisticheRicavi.CodiceLavoro = riga.Field<string>("codice_lavoro");
                                        statisticheRicavi.Data = riga.Field<DateTime?>("data");
                                        statisticheRicavi.Lavorazione = riga.Field<string>("lavorazione");
                                        statisticheRicavi.Produzione = riga.Field<string>("produzione");
                                        statisticheRicavi.Contratto = riga.Field<string>("contratto");
                                        statisticheRicavi.Listino = riga.Field<decimal?>("listino");
                                        statisticheRicavi.Costo = riga.Field<decimal?>("costo");
                                        //statisticheRicavi.DocumentoAllegato = riga.Field<string>("docFattura") != null ? riga.Field<string>("docFattura") : riga.Field<string>("docOfferta");
                                        statisticheRicavi.DocumentoAllegato = (fatturato != null && (bool)fatturato) ? riga.Field<string>("docFattura") : riga.Field<string>("docOfferta");
                                        statisticheRicavi.Pregresso = riga.Field<bool>("pregresso");

                                        listaStatisticheRicavi.Add(statisticheRicavi);
                                        }
                                        catch (Exception ex)
                                        {
                                            lavorazioniAnomale.Add(riga.Field<string>("codice_lavoro"));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.Descrizione = "Statistiche_DAL.cs - GetStatisticheRicavi " + ex.Message + Environment.NewLine + ex.StackTrace;
            }
            if (lavorazioniAnomale.Count > 0)
            {
                BasePage basePge = new BasePage();
                string messaggioLavorazioniAnomale = "Alcuni elementi non sono stati visualizzati perché le seguenti lavorazioni presentano anomalie:<ul>";
                foreach (string lavAnomala in lavorazioniAnomale)
                {
                    messaggioLavorazioniAnomale += "<li>" + lavAnomala + "</li>";
                }
                basePge.ShowWarning(messaggioLavorazioniAnomale + "</ul>");
            }

            return listaStatisticheRicavi;
        }

        public List<StatisticheRicavi> GetStatisticheRicavi(string filtroCliente, string filtroProduzione, string filtroLavorazione, string filtroContratto, bool? fatturato, string dataInizio, string dataFine, ref Esito esito)
        {
            List<StatisticheRicavi> listaStatisticheRicavi = new List<StatisticheRicavi>();
            HashSet<string> lavorazioniAnomale = new HashSet<string>();

            string query = string.Empty;

            if (fatturato != null)
            {
                if ((bool)fatturato)
                {
                    query = CreaQuery_StatisticheRicavi_Fatturato_True(filtroCliente, filtroProduzione, filtroLavorazione, filtroContratto, fatturato, dataInizio, dataFine);
                }
                else
                {
                    query = CreaQuery_StatisticheRicavi_Fatturato_False(filtroCliente, filtroProduzione, filtroLavorazione, filtroContratto, fatturato, dataInizio, dataFine);
                }
            }
            else // Fatturato = <tutti>
            {
                query = CreaQuery_StatisticheRicavi_Fatturato_Tutti(filtroCliente, filtroProduzione, filtroLavorazione, filtroContratto, fatturato, dataInizio, dataFine);
            }

            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                                {
                                    foreach (DataRow riga in dt.Rows)
                                    {
                                        try
                                        {
                                            StatisticheRicavi statisticheRicavi = new StatisticheRicavi();

                                            statisticheRicavi.IdCliente = riga.Field<int>("id_cliente");
                                            statisticheRicavi.Cliente = riga.Field<string>("cliente");
                                            statisticheRicavi.NumeroFattura = riga.Field<string>("numeroFattura");
                                            statisticheRicavi.Ordine = riga.Field<string>("ordine");
                                            statisticheRicavi.CodiceLavoro = riga.Field<string>("codice_lavoro");
                                            statisticheRicavi.Data = riga.Field<DateTime?>("data");
                                            statisticheRicavi.Lavorazione = riga.Field<string>("lavorazione");
                                            statisticheRicavi.Produzione = riga.Field<string>("produzione");
                                            statisticheRicavi.Contratto = riga.Field<string>("contratto");
                                            statisticheRicavi.Listino = riga.Field<decimal?>("listino");
                                            statisticheRicavi.Costo = riga.Field<decimal?>("costo");
                                            statisticheRicavi.DocumentoAllegato = (fatturato != null && (bool)fatturato) ? riga.Field<string>("docFattura") : riga.Field<string>("docOfferta");
                                            statisticheRicavi.Pregresso = riga.Field<bool>("pregresso");

                                            listaStatisticheRicavi.Add(statisticheRicavi);
                                        }
                                        catch (Exception ex)
                                        {
                                            lavorazioniAnomale.Add(riga.Field<string>("codice_lavoro"));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.Descrizione = "Statistiche_DAL.cs - GetStatisticheRicavi " + ex.Message + Environment.NewLine + ex.StackTrace;
            }
            if (lavorazioniAnomale.Count > 0)
            {
                BasePage basePge = new BasePage();
                string messaggioLavorazioniAnomale = "Alcuni elementi non sono stati visualizzati perché le seguenti lavorazioni presentano anomalie:<ul>";
                foreach (string lavAnomala in lavorazioniAnomale)
                {
                    messaggioLavorazioniAnomale += "<li>" + lavAnomala + "</li>";
                }
                basePge.ShowWarning(messaggioLavorazioniAnomale + "</ul>");
            }

            return listaStatisticheRicavi;
        }

        private string CreaQuery_StatisticheRicavi_Fatturato_Tutti(string filtroCliente, string filtroProduzione, string filtroLavorazione, string filtroContratto, bool? fatturato, string dataInizio, string dataFine)
        {
            string filtri = string.Empty;
            filtri += string.IsNullOrWhiteSpace(filtroCliente) ? "" : " AND b.ragioneSociale like '%" + filtroCliente + "%' ";
            filtri += string.IsNullOrWhiteSpace(filtroProduzione) ? "" : " AND a.produzione like '%" + filtroProduzione + "%' ";
            filtri += string.IsNullOrWhiteSpace(filtroLavorazione) ? "" : " AND a.lavorazione like '%" + filtroLavorazione + "%' ";
            filtri += string.IsNullOrWhiteSpace(filtroContratto) ? "" : " AND f.descrizione like '%" + filtroContratto + "%' ";

            filtri += string.IsNullOrWhiteSpace(dataInizio) ? "" : " AND ((e.protocollo_riferimento is null AND a.data_inizio_lavorazione >= '" + dataInizio.Substring(6) + "-" + dataInizio.Substring(3, 2) + "-" + dataInizio.Substring(0, 2) + "T00:00:00.000') OR (e.protocollo_riferimento is not null AND e.data_fattura >= '" + dataInizio.Substring(6) + "-" + dataInizio.Substring(3, 2) + "-" + dataInizio.Substring(0, 2) + "T00:00:00.000')) ";
            // il filtro dataFine viene eseguito su dataInizioLavorazione, e non su dataFineLavorazione
            filtri += string.IsNullOrWhiteSpace(dataFine) ? "" : " AND (( e.protocollo_riferimento is null AND a.data_inizio_lavorazione <= '" + dataFine.Substring(6) + "-" + dataFine.Substring(3, 2) + "-" + dataFine.Substring(0, 2) + "T00:00:00.000') OR (e.protocollo_riferimento is not null AND e.data_fattura <= '" + dataFine.Substring(6) + "-" + dataFine.Substring(3, 2) + "-" + dataFine.Substring(0, 2) + "T00:00:00.000')) ";

            string query =
                "SELECT a.id_cliente, b.ragioneSociale AS cliente, e.protocollo_riferimento AS numeroFattura, c.ordine, a.codice_lavoro, COALESCE(e.data_fattura, a.data_inizio_lavorazione) AS data, a.lavorazione, a.produzione, d_agg.listino, d_agg.costo, f.descrizione AS contratto, h.pathDocumento AS docOfferta, h.pregresso " +
                "FROM tab_dati_agenda a " +
                "LEFT JOIN anag_clienti_fornitori b ON b.id = a.id_cliente " +
                "LEFT JOIN dati_lavorazione c ON c.idDatiAgenda = a.id " +
                "LEFT JOIN (SELECT c.idDatiAgenda, SUM(d.prezzo) AS listino, SUM(d.fp_lordo) AS costo FROM dati_lavorazione c LEFT JOIN dati_articoli_lavorazione d ON d.idDatiLavorazione = c.id GROUP BY c.idDatiAgenda) d_agg ON d_agg.idDatiAgenda = a.id " +
                "LEFT JOIN tipo_protocollo g ON g.nome = 'Fattura' " +
                "LEFT JOIN tipo_protocollo i ON i.nome = 'Contratto' " +
                "LEFT JOIN tipo_protocollo j ON j.nome = 'Offerta' " +
                "OUTER APPLY (SELECT TOP 1 e2.protocollo_riferimento, e2.data_fattura FROM dati_protocollo e2 WHERE e2.codice_lavoro = a.codice_lavoro AND e2.attivo = 1 AND e2.id_tipo_protocollo = g.id AND e2.destinatario = 'Cliente' ORDER BY e2.data_fattura DESC) e " +
                "LEFT JOIN dati_protocollo f ON f.id = c.idContratto AND f.id_tipo_protocollo = i.id " +
                "OUTER APPLY (SELECT TOP 1 h2.pathDocumento, h2.pregresso FROM dati_protocollo h2 WHERE h2.codice_lavoro = a.codice_lavoro AND h2.id_tipo_protocollo = j.id AND h2.destinatario = 'Cliente' ORDER BY h2.id) h " +
                "WHERE a.codice_lavoro IS NOT NULL AND a.id_stato >= 3 " +
                filtri;

            return query;
        }

        private string CreaQuery_StatisticheRicavi_Fatturato_True(string filtroCliente, string filtroProduzione, string filtroLavorazione, string filtroContratto, bool? fatturato, string dataInizio, string dataFine)
        {
            string filtri = string.Empty;
            filtri += string.IsNullOrWhiteSpace(filtroCliente) ? "" : " AND b.ragioneSociale like '%" + filtroCliente + "%' ";
            filtri += string.IsNullOrWhiteSpace(filtroProduzione) ? "" : " AND a.produzione like '%" + filtroProduzione + "%' ";
            filtri += string.IsNullOrWhiteSpace(filtroLavorazione) ? "" : " AND a.lavorazione like '%" + filtroLavorazione + "%' ";
            filtri += string.IsNullOrWhiteSpace(filtroContratto) ? "" : " AND f.descrizione like '%" + filtroContratto + "%' ";

            string filtriOuterApply = string.Empty;
            filtriOuterApply += string.IsNullOrWhiteSpace(dataInizio) ? "" : " AND e2.data_fattura >= '" + dataInizio.Substring(6) + "-" + dataInizio.Substring(3, 2) + "-" + dataInizio.Substring(0, 2) + "T00:00:00.000' ";
            filtriOuterApply += string.IsNullOrWhiteSpace(dataFine) ? "" : " AND e2.data_fattura <= '" + dataFine.Substring(6) + "-" + dataFine.Substring(3, 2) + "-" + dataFine.Substring(0, 2) + "T00:00:00.000' ";

            string query =
                "SELECT a.id_cliente, b.ragioneSociale AS cliente, e.protocollo_riferimento AS numeroFattura, c.ordine, a.codice_lavoro, e.data_fattura AS data, a.lavorazione, a.produzione, d_agg.listino, d_agg.costo, f.descrizione AS contratto, e.pathDocumento AS docFattura, h.pregresso " +
                "FROM tab_dati_agenda a " +
                "LEFT JOIN anag_clienti_fornitori b ON b.id = a.id_cliente " +
                "LEFT JOIN dati_lavorazione c ON c.idDatiAgenda = a.id " +
                "LEFT JOIN (SELECT c.idDatiAgenda, SUM(d.prezzo) AS listino, SUM(d.fp_lordo) AS costo FROM dati_lavorazione c LEFT JOIN dati_articoli_lavorazione d ON d.idDatiLavorazione = c.id GROUP BY c.idDatiAgenda) d_agg ON d_agg.idDatiAgenda = a.id " +
                "LEFT JOIN tipo_protocollo g ON g.nome = 'Fattura' " +
                "LEFT JOIN tipo_protocollo i ON i.nome = 'Contratto' " +
                "LEFT JOIN tipo_protocollo j ON j.nome = 'Offerta' " +
                "OUTER APPLY (SELECT TOP 1 e2.protocollo_riferimento, e2.data_fattura, e2.pathDocumento FROM dati_protocollo e2 WHERE e2.codice_lavoro = a.codice_lavoro AND e2.attivo = 1 AND e2.id_tipo_protocollo = g.id AND e2.destinatario = 'Cliente' AND e2.protocollo_riferimento IS NOT NULL " +
                filtriOuterApply +
                "ORDER BY e2.data_fattura DESC) e " +
                "LEFT JOIN dati_protocollo f ON f.id = c.idContratto AND f.id_tipo_protocollo = i.id " +
                "OUTER APPLY (SELECT TOP 1 h2.pregresso FROM dati_protocollo h2 WHERE h2.codice_lavoro = a.codice_lavoro AND h2.id_tipo_protocollo = j.id AND h2.destinatario = 'Cliente' ORDER BY h2.id) h " +
                "WHERE a.codice_lavoro IS NOT NULL AND a.id_stato >= 3 " +
                filtri +
                "AND e.protocollo_riferimento IS NOT NULL ";

            return query;
        }

        private string CreaQuery_StatisticheRicavi_Fatturato_False(string filtroCliente, string filtroProduzione, string filtroLavorazione, string filtroContratto, bool? fatturato, string dataInizio, string dataFine)
        {
            string filtri = string.Empty;
            filtri += string.IsNullOrWhiteSpace(filtroCliente) ? "" : " AND b.ragioneSociale like '%" + filtroCliente + "%' ";
            filtri += string.IsNullOrWhiteSpace(filtroProduzione) ? "" : " AND a.produzione like '%" + filtroProduzione + "%' ";
            filtri += string.IsNullOrWhiteSpace(filtroLavorazione) ? "" : " AND a.lavorazione like '%" + filtroLavorazione + "%' ";
            filtri += string.IsNullOrWhiteSpace(filtroContratto) ? "" : " AND f.descrizione like '%" + filtroContratto + "%' ";

            filtri += string.IsNullOrWhiteSpace(dataInizio) ? "" : " AND a.data_inizio_lavorazione >= '" + dataInizio.Substring(6) + "-" + dataInizio.Substring(3, 2) + "-" + dataInizio.Substring(0, 2) + "T00:00:00.000' ";
            // il filtro dataFine viene eseguito su dataInizioLavorazione, e non su dataFineLavorazione
            filtri += string.IsNullOrWhiteSpace(dataFine) ? "" : " AND a.data_inizio_lavorazione <= '" + dataFine.Substring(6) + "-" + dataFine.Substring(3, 2) + "-" + dataFine.Substring(0, 2) + "T00:00:00.000' ";

            string query =
                "SELECT a.id_cliente, b.ragioneSociale AS cliente, NULL AS numeroFattura, c.ordine, a.codice_lavoro, a_min.data_minima AS data, a.lavorazione, a.produzione, d_agg.listino, d_agg.costo, f.descrizione AS contratto, h.pathDocumento AS docOfferta, h.pregresso " +
                "FROM tab_dati_agenda a " +
                "LEFT JOIN anag_clienti_fornitori b ON b.id = a.id_cliente " +
                "LEFT JOIN dati_lavorazione c ON c.idDatiAgenda = a.id " +
                "LEFT JOIN (SELECT id_cliente, codice_lavoro, MIN(data_inizio_lavorazione) AS data_minima    FROM tab_dati_agenda GROUP BY id_cliente, codice_lavoro) a_min ON a_min.id_cliente = a.id_cliente AND a_min.codice_lavoro = a.codice_lavoro " +
                "LEFT JOIN (SELECT c.idDatiAgenda, SUM(d.prezzo) AS listino, SUM(d.fp_lordo) AS costo FROM dati_lavorazione c LEFT JOIN dati_articoli_lavorazione d ON d.idDatiLavorazione = c.id GROUP BY c.idDatiAgenda) d_agg ON d_agg.idDatiAgenda = a.id " +
                "LEFT JOIN tipo_protocollo i ON i.nome = 'Contratto' " +
                "LEFT JOIN dati_protocollo f ON f.id = c.idContratto AND f.id_tipo_protocollo = i.id " +
                "LEFT JOIN tipo_protocollo j ON j.nome = 'Offerta' " +
                "left join tipo_protocollo g on  g.nome = 'Fattura' " +
                "left join dati_protocollo e on e.codice_lavoro = a.codice_lavoro and e.attivo = 1 and e.id_tipo_protocollo = g.id and e.destinatario = 'Cliente' " +
                "OUTER APPLY (SELECT TOP 1 h2.pathDocumento, h2.pregresso FROM dati_protocollo h2 WHERE h2.codice_lavoro = a.codice_lavoro AND h2.id_tipo_protocollo = j.id AND h2.destinatario = 'Cliente' ORDER BY h2.id) h " +
                "WHERE a.codice_lavoro IS NOT NULL AND a.id_stato >= 3 " +
                "and e.protocollo_riferimento is null  " +
                filtri;

            return query;
        }
        #endregion

        #region STATISTICHE COSTI
        private string CreaQuerySenzaFiltriCosti(string filtriLavorazione)
        {
            //NELLA PRIMA QUERY SOLO I PREZZO DI LISTINO, NELLE ALTRE SOLTANTO I COSTI
            //string query = "select distinct 1 progressivo, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, min(a.data_inizio_lavorazione) data, a.lavorazione, a.produzione, '' gruppo, '' sottogruppo, '' fornitore, sum(d.prezzo) listino, sum(d.fp_lordo)  costo, f.descrizione contratto, e.pathDocumento 'docFattura', h.pathDocumento 'docOfferta', h.pregresso " +
            //string query = "select top(1) 1 progressivo, 0, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, min(a.data_inizio_lavorazione) data, a.lavorazione, a.produzione, '' gruppo, '' sottogruppo, '' fornitore, d.prezzo listino, 0.00  costo, f.descrizione contratto, COALESCE(e.pathDocumento,h.pathDocumento) AS documento, h.pregresso " +
            string query = "select  1 progressivo, 0, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, min(a.data_inizio_lavorazione) data, a.lavorazione, a.produzione, '' gruppo, '' sottogruppo, '' fornitore, d.prezzo listino, 0.00  costo, MIN(COALESCE(e.pathDocumento,h.pathDocumento)) AS documento, h.pregresso, a.codice_lavoro as codiceLavoro2 " +

                                    "from tab_dati_agenda a  " +
                                    "left join anag_clienti_fornitori b on b.id = a.id_cliente  " +
                                    "left join dati_lavorazione c on c.idDatiAgenda = a.id  " +
                                    "LEFT JOIN (SELECT idDatiLavorazione, SUM(prezzo) AS prezzo FROM dati_articoli_lavorazione GROUP BY idDatiLavorazione) d ON d.idDatiLavorazione = c.id " +
                                    "left join tipo_protocollo g on  g.nome = 'Fattura'  " +
                                    "left join dati_protocollo e on e.codice_lavoro = a.codice_lavoro and e.attivo = 1 and e.id_tipo_protocollo = g.id and destinatario = 'Cliente'  " +
                                    //"left join tipo_protocollo i on  i.nome = 'Contratto'  " +
                                    //"left join dati_protocollo f on f.id=c.idContratto and f.id_tipo_protocollo = i.id  " +
                                    "left join tipo_protocollo j on  j.nome = 'Offerta'  " +
                                    "left join dati_protocollo h on h.codice_lavoro = a.codice_lavoro and h.id_tipo_protocollo = j.id and h.destinatario = 'Cliente'  and h.attivo = 1 " +
                                    
                                    "where a.codice_lavoro is not null and a.id_stato >= 3  and d.prezzo <> 0 " + filtriLavorazione +
                                    //" group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, f.descrizione, e.pathDocumento, h.pathDocumento, h.pregresso, d.prezzo " +
                                    " group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, h.pregresso, d.prezzo " +
            #region COLLABORATORI ASSUNTI
                                    "UNION " +
                                    //"select  2 progressivo, d.id, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, d.data data, a.lavorazione, a.produzione, 'Collaboratori' gruppo, k.nome sottogruppo, COALESCE (coll.cognome+' '+coll.nome , forn.ragioneSociale) fornitore, 0.00 listino, d.costo costo, f.descrizione contratto, e.pathDocumento  as documento, h.pregresso  " +
                                    "select  2 progressivo, d.id, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, d.data data, a.lavorazione, a.produzione, 'Collaboratori' gruppo, k.nome sottogruppo, COALESCE (coll.cognome+' '+coll.nome , forn.ragioneSociale) fornitore, 0.00 listino, d.costo costo, e.pathDocumento  as documento, h.pregresso, '0' as codiceLavoro2   " +

                                    "from tab_dati_agenda a  " +
                                    "left join anag_clienti_fornitori b on b.id = a.id_cliente  " +
                                    "left join dati_lavorazione c on c.idDatiAgenda = a.id  " +
                                    "LEFT JOIN (SELECT idDatiLavorazione, fp_netto AS costo, idFornitori, idCollaboratori, idTipoSottoGruppo, id, data FROM dati_articoli_lavorazione ) d ON d.idDatiLavorazione = c.id " +
                                    "left join tipo_protocollo g on  g.nome = 'Fattura'  " +
                                    "left join dati_protocollo e on e.codice_lavoro = a.codice_lavoro and e.attivo = 1 and e.id_tipo_protocollo = g.id and destinatario = 'Fornitore' and  e.id_cliente=d.idFornitori and e.id_dati_articoli_lavorazione=d.id " +
                                    //"left join tipo_protocollo i on  i.nome = 'Contratto'  " +
                                    //"left join dati_protocollo f on f.id=c.idContratto and f.id_tipo_protocollo = i.id  " +
                                    "left join tipo_protocollo j on  j.nome = 'Offerta'  " +
                                    "left join dati_protocollo h on h.codice_lavoro = a.codice_lavoro and h.id_tipo_protocollo = j.id and h.destinatario = 'Cliente'  and h.attivo = 1 " +
                                    "left join tipo_sottogruppo k on k.nome = 'Collaboratori Assunti' " +
                                    "left join anag_clienti_fornitori forn on d.idFornitori is not NULL AND forn.id = d.idFornitori " +
                                    "left join anag_collaboratori coll on d.idCollaboratori is NOT NULL AND coll.id = d.idCollaboratori " +
                                    
                                    "where a.codice_lavoro is not null and a.id_stato >= 3 and d.costo <>0 and d.idTipoSottogruppo = k.id " + filtriLavorazione +
                                    //" group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, f.descrizione, e.pathDocumento, h.pathDocumento, h.pregresso, k.nome, forn.ragioneSociale, coll.cognome, coll.nome, d.id, d.costo, d.data " +
                                    " group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, e.pathDocumento, h.pathDocumento, h.pregresso, k.nome, forn.ragioneSociale, coll.cognome, coll.nome, d.id, d.costo, d.data " +
            #endregion

            #region COLLABORATORI A FATTURA
                                    "UNION " +
                                    //"select  3 progressivo, d.id, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, d.data data, a.lavorazione, a.produzione, 'Collaboratori' gruppo, k.nome sottogruppo, COALESCE(NULLIF(coll.nomeSocieta, ''),NULLIF(forn.ragioneSociale, ''), NULLIF(coll.cognome, '') + ' ' + NULLIF(coll.nome, '')) fornitore, 0.00 listino, d.costo costo, f.descrizione contratto, e.pathDocumento as documento, h.pregresso  " +
                                    "select  3 progressivo, d.id, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, d.data data, a.lavorazione, a.produzione, 'Collaboratori' gruppo, k.nome sottogruppo, COALESCE(NULLIF(coll.nomeSocieta, ''),NULLIF(forn.ragioneSociale, ''), NULLIF(coll.cognome, '') + ' ' + NULLIF(coll.nome, '')) fornitore, 0.00 listino, d.costo costo, e.pathDocumento as documento, h.pregresso, '0' as codiceLavoro2   " +

                                    "from tab_dati_agenda a  " +
                                    "left join anag_clienti_fornitori b on b.id = a.id_cliente  " +
                                    "left join dati_lavorazione c on c.idDatiAgenda = a.id  " +
                                    "LEFT JOIN (SELECT idDatiLavorazione, fp_netto AS costo, idFornitori, idCollaboratori, idTipoSottoGruppo, id, data FROM dati_articoli_lavorazione ) d ON d.idDatiLavorazione = c.id " +
                                    "left join tipo_protocollo g on  g.nome = 'Fattura'  " +
                                    "left join dati_protocollo e on e.codice_lavoro = a.codice_lavoro and e.attivo = 1 and e.id_tipo_protocollo = g.id and destinatario = 'Fornitore' and  e.id_cliente=d.idFornitori and e.id_dati_articoli_lavorazione=d.id " +
                                    //"left join tipo_protocollo i on  i.nome = 'Contratto'  " +
                                    //"left join dati_protocollo f on f.id=c.idContratto and f.id_tipo_protocollo = i.id  " +
                                    "left join tipo_protocollo j on  j.nome = 'Offerta'  " +
                                    "left join dati_protocollo h on h.codice_lavoro = a.codice_lavoro and h.id_tipo_protocollo = j.id and h.destinatario = 'Cliente'  and h.attivo = 1 " +
                                    "left join tipo_sottogruppo k on k.nome = 'Collaboratori a Fattura' " +
                                    "left join anag_clienti_fornitori forn on d.idFornitori is not NULL AND forn.id = d.idFornitori " +
                                    "left join anag_collaboratori coll on d.idCollaboratori is NOT NULL AND coll.id = d.idCollaboratori " +

                                    "where a.codice_lavoro is not null and a.id_stato >= 3 and d.costo <>0 and d.idTipoSottoGruppo = k.id " + filtriLavorazione +
                                    //" group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, f.descrizione, e.pathDocumento, h.pathDocumento, h.pregresso, k.nome, forn.ragioneSociale, coll.nomeSocieta, coll.cognome, coll.nome, d.id, d.costo, d.data " +
                                    " group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, e.pathDocumento, h.pathDocumento, h.pregresso, k.nome, forn.ragioneSociale, coll.nomeSocieta, coll.cognome, coll.nome, d.id, d.costo, d.data " +
            #endregion

            #region DIARIA
                                    "UNION " +
                                    //"select  4 progressivo, d.id, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, d.data data, a.lavorazione, a.produzione, l.nome gruppo, k.nome sottogruppo, COALESCE (coll.cognome+' '+coll.nome , forn.ragioneSociale) fornitore, 0.00 listino, d.costo costo, f.descrizione contratto, e.pathDocumento  as documento, h.pregresso  " +
                                    "select  4 progressivo, d.id, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, d.data data, a.lavorazione, a.produzione, l.nome gruppo, k.nome sottogruppo, COALESCE (coll.cognome+' '+coll.nome , forn.ragioneSociale) fornitore, 0.00 listino, d.costo costo, e.pathDocumento  as documento, h.pregresso, '0' as codiceLavoro2   " +

                                    "from tab_dati_agenda a  " +
                                    "left join anag_clienti_fornitori b on b.id = a.id_cliente  " +
                                    "left join dati_lavorazione c on c.idDatiAgenda = a.id  " +
                                    "LEFT JOIN (SELECT idDatiLavorazione, fp_lordo AS costo, idFornitori, idCollaboratori, idTipoGruppo, idTipoSottoGruppo, id, data  FROM dati_articoli_lavorazione ) d ON d.idDatiLavorazione = c.id  " +
                                    "left join tipo_protocollo g on  g.nome = 'Fattura'  " +
                                    "left join dati_protocollo e on e.codice_lavoro = a.codice_lavoro and e.attivo = 1 and e.id_tipo_protocollo = g.id and destinatario = 'Fornitore' and  e.id_cliente=d.idFornitori " +
                                    //"left join tipo_protocollo i on  i.nome = 'Contratto'  " +
                                    //"left join dati_protocollo f on f.id=c.idContratto and f.id_tipo_protocollo = i.id  " +
                                    "left join tipo_protocollo j on  j.nome = 'Offerta'  " +
                                    "left join dati_protocollo h on h.codice_lavoro = a.codice_lavoro and h.id_tipo_protocollo = j.id and h.destinatario = 'Cliente'  and h.attivo = 1 " +
                                    "left join tipo_sottogruppo k on k.nome = 'Diaria' " +
                                    "left join tipo_gruppo l on l.id = k.idTipoGruppo " +
                                    "left join anag_clienti_fornitori forn on d.idFornitori is not NULL AND forn.id = d.idFornitori " +
                                    "left join anag_collaboratori coll on d.idCollaboratori is NOT NULL AND coll.id = d.idCollaboratori " +
                                    
                                    "where a.codice_lavoro is not null and a.id_stato >= 3 and  d.idTipoSottoGruppo=k.id " + filtriLavorazione +
                                    //" group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, f.descrizione, e.pathDocumento, h.pathDocumento, h.pregresso, k.nome, l.nome, forn.ragioneSociale, coll.cognome, coll.nome, d.id, d.costo, d.data " +
                                    " group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, e.pathDocumento, h.pathDocumento, h.pregresso, k.nome, l.nome, forn.ragioneSociale, coll.cognome, coll.nome, d.id, d.costo, d.data " +
            #endregion

            #region ELIMINATI
            #region TRASFERIMENTI
            //"UNION " +
            //"select distinct 4 progressivo, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, min(a.data_inizio_lavorazione) data, a.lavorazione, a.produzione, k.nome gruppo, '' fornitore, sum(d.prezzo) listino, sum(d.fp_lordo)  costo, f.descrizione contratto, e.pathDocumento 'docFattura', h.pathDocumento 'docOfferta', h.pregresso  " +
            //"from tab_dati_agenda a  " +
            //"left join anag_clienti_fornitori b on b.id = a.id_cliente  " +
            //"left join dati_lavorazione c on c.idDatiAgenda = a.id  " +
            //"left join dati_articoli_lavorazione d on d.idDatiLavorazione = c.id  " +
            //"left join tipo_protocollo g on  g.nome = 'Fattura'  " +
            //"left join dati_protocollo e on e.codice_lavoro = a.codice_lavoro and e.attivo = 1 and e.id_tipo_protocollo = g.id and destinatario = 'Cliente'  " +
            //"left join tipo_protocollo i on  i.nome = 'Contratto'  " +
            //"left join dati_protocollo f on f.id=c.idContratto and f.id_tipo_protocollo = i.id  " +
            //"left join tipo_protocollo j on  j.nome = 'Offerta'  " +
            //"left join dati_protocollo h on h.codice_lavoro = a.codice_lavoro and h.id_tipo_protocollo = j.id and h.destinatario = 'Cliente'  " +
            //"left join tipo_gruppo k on k.nome = 'Trasferimenti' " +
            //"where a.codice_lavoro is not null and a.id_stato >= 3 and d.fp_lordo >0 and d.idTipoGruppo=k.id " + filtri +
            //" group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, f.descrizione, e.pathDocumento, h.pathDocumento, h.pregresso, k.nome " +
            #endregion

            #region ALBERGO
            //"UNION " +
            //"select distinct 5 progressivo, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, min(a.data_inizio_lavorazione) data, a.lavorazione, a.produzione, l.nome gruppo, k.ragioneSociale fornitore, sum(d.prezzo) listino, sum(d.fp_lordo)  costo, f.descrizione contratto, e.pathDocumento 'docFattura', h.pathDocumento 'docOfferta', h.pregresso  " +
            //"from tab_dati_agenda a  " +
            //"left join anag_clienti_fornitori b on b.id = a.id_cliente  " +
            //"left join dati_lavorazione c on c.idDatiAgenda = a.id  " +
            //"left join dati_articoli_lavorazione d on d.idDatiLavorazione = c.id  " +
            //"left join tipo_protocollo g on  g.nome = 'Fattura'  " +
            //"left join dati_protocollo e on e.codice_lavoro = a.codice_lavoro and e.attivo = 1 and e.id_tipo_protocollo = g.id and destinatario = 'Cliente'  " +
            //"left join tipo_protocollo i on  i.nome = 'Contratto'  " +
            //"left join dati_protocollo f on f.id=c.idContratto and f.id_tipo_protocollo = i.id  " +
            //"left join tipo_protocollo j on  j.nome = 'Offerta'  " +
            //"left join dati_protocollo h on h.codice_lavoro = a.codice_lavoro and h.id_tipo_protocollo = j.id and h.destinatario = 'Cliente'  " +
            //"left join anag_clienti_fornitori k on d.idFornitori = k.id " +
            //"left join tipo_gruppo l on l.nome = 'Albergo' " +
            //"where a.codice_lavoro is not null and a.id_stato >= 3 and d.fp_lordo >0 and d.idTipoGruppo=l.id " + filtri +
            //" group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, f.descrizione, e.pathDocumento, h.pathDocumento, h.pregresso, l.nome, k.ragioneSociale " +
            #endregion
            #endregion

   
            #region TUTTO IL RESTO
                        "UNION " +
                        //"select distinct 5 progressivo, 0, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, min(a.data_inizio_lavorazione) data, a.lavorazione, a.produzione, l.nome gruppo, k.nome sottogruppo, COALESCE (coll.cognome+' '+coll.nome , forn.ragioneSociale) fornitore, 0.00 listino, d.costo  costo, f.descrizione contratto, e.pathDocumento  as documento, h.pregresso  " +
                        "select distinct 5 progressivo, 0, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, min(a.data_inizio_lavorazione) data, a.lavorazione, a.produzione, l.nome gruppo, k.nome sottogruppo, COALESCE (coll.cognome+' '+coll.nome , forn.ragioneSociale) fornitore, 0.00 listino, d.costo  costo, e.pathDocumento  as documento, h.pregresso, '0' as codiceLavoro2   " +

                        "from tab_dati_agenda a  " +
                        "left join anag_clienti_fornitori b on b.id = a.id_cliente  " +
                        "left join dati_lavorazione c on c.idDatiAgenda = a.id  " +
                        "LEFT JOIN (SELECT idDatiLavorazione, sum(fp_netto) AS costo, idFornitori,idTipoGruppo,idTipoSottoGruppo, idCollaboratori, id FROM dati_articoli_lavorazione GROUP BY idDatiLavorazione, idFornitori,idTipoGruppo,idTipoSottoGruppo, idCollaboratori, id) d ON d.idDatiLavorazione = c.id " +
                        "left join tipo_protocollo g on  g.nome = 'Fattura'  " +
                        "left join dati_protocollo e on e.codice_lavoro = a.codice_lavoro and e.attivo = 1 and e.id_tipo_protocollo = g.id and destinatario = 'Fornitore' and  e.id_cliente=d.idFornitori and  e.id_dati_articoli_lavorazione = d.id " +
                        //"left join tipo_protocollo i on  i.nome = 'Contratto'  " +
                        //"left join dati_protocollo f on f.id=c.idContratto and f.id_tipo_protocollo = i.id  " +
                        "left join tipo_protocollo j on  j.nome = 'Offerta'  " +
                        "left join dati_protocollo h on h.codice_lavoro = a.codice_lavoro and h.id_tipo_protocollo = j.id and h.destinatario = 'Cliente'  and h.attivo = 1 " +
                        "left join anag_clienti_fornitori forn on d.idFornitori = forn.id " +
                        "left join tipo_gruppo l on l.nome != 'Collaboratori' " +
                        "left join tipo_sottogruppo k on k.nome != 'Diaria' " +
                        "left join anag_collaboratori coll on d.idCollaboratori is NOT NULL AND coll.id = d.idCollaboratori " +

                        "where a.codice_lavoro is not null and a.id_stato >= 3 and d.costo <>0 and d.idTipoGruppo=l.id and d.idTipoSottoGruppo=k.id " + filtriLavorazione +

                        " group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, e.pathDocumento, h.pathDocumento, h.pregresso, k.nome, l.nome, forn.ragioneSociale, coll.cognome, coll.nome, d.costo " +
            #endregion
                                    " order by cliente, a.codice_lavoro, progressivo";

            return query;
        }

        private string CreaQueryCosti(string filtriLavorazione, string filtriCosti)
        {
            // SOLO I COSTI
            string query =
            #region COLLABORATORI ASSUNTI
                                    //"select distinct 2 progressivo, d.id, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, d.data data, a.lavorazione, a.produzione, 'Collaboratori' gruppo, k.nome sottogruppo, COALESCE (coll.cognome+' '+coll.nome , forn.ragioneSociale) fornitore, 0.00 listino, d.costo costo, f.descrizione contratto, e.pathDocumento as documento, h.pregresso  " +
                                    "select distinct 2 progressivo, d.id, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, d.data data, a.lavorazione, a.produzione, 'Collaboratori' gruppo, k.nome sottogruppo, COALESCE (coll.cognome+' '+coll.nome , forn.ragioneSociale) fornitore, 0.00 listino, d.costo costo, e.pathDocumento as documento, h.pregresso, '0' as codiceLavoro2   " +

                                    "from tab_dati_agenda a  " +
                                    "left join anag_clienti_fornitori b on b.id = a.id_cliente  " +
                                    "left join dati_lavorazione c on c.idDatiAgenda = a.id  " +
                                    "LEFT JOIN (SELECT idDatiLavorazione, fp_netto AS costo, idFornitori, idTipoSottogruppo, idCollaboratori, id, idTipoGruppo, idTipoGenere, data FROM dati_articoli_lavorazione ) d ON d.idDatiLavorazione = c.id " +
                                    "left join tipo_protocollo g on  g.nome = 'Fattura'  " +
                                    "left join dati_protocollo e on e.codice_lavoro = a.codice_lavoro and e.attivo = 1 and e.id_tipo_protocollo = g.id and destinatario = 'Fornitore' and  e.id_cliente=d.idFornitori and e.id_dati_articoli_lavorazione=d.id " +
                                    //"left join tipo_protocollo i on  i.nome = 'Contratto'  " +
                                    //"left join dati_protocollo f on f.id=c.idContratto and f.id_tipo_protocollo = i.id  " +
                                    "left join tipo_protocollo j on  j.nome = 'Offerta'  " +
                                    "left join dati_protocollo h on h.codice_lavoro = a.codice_lavoro and h.id_tipo_protocollo = j.id and h.destinatario = 'Cliente'  and h.attivo = 1 " +
                                    "left join tipo_sottogruppo k on k.nome = 'Collaboratori Assunti' " +
                                    "left join anag_clienti_fornitori forn on d.idFornitori is not NULL AND forn.id = d.idFornitori " +
                                    "left join anag_collaboratori coll on d.idCollaboratori is NOT NULL AND coll.id = d.idCollaboratori " +
                                    
                                    "where a.codice_lavoro is not null and a.id_stato >= 3 and d.costo <>0 and d.idTipoSottogruppo = k.id " + filtriLavorazione + filtriCosti +
                                    //" group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, f.descrizione, e.pathDocumento, h.pathDocumento, h.pregresso, k.nome, forn.ragioneSociale, coll.cognome, coll.nome, d.id, d.costo, d.data " +
                                    " group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, e.pathDocumento, h.pathDocumento, h.pregresso, k.nome, forn.ragioneSociale, coll.cognome, coll.nome, d.id, d.costo, d.data " +
            #endregion

            #region COLLABORATORI A FATTURA
                                    "UNION " +
                                    //"select  3 progressivo, d.id, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, d.data data, a.lavorazione, a.produzione, 'Collaboratori' gruppo, k.nome sottogruppo, COALESCE(NULLIF(coll.nomeSocieta, ''),NULLIF(forn.ragioneSociale, ''), NULLIF(coll.cognome, '') + ' ' + NULLIF(coll.nome, '')) fornitore, 0.00 listino, d.costo costo, f.descrizione contratto, e.pathDocumento  as documento, h.pregresso  " +
                                    "select  3 progressivo, d.id, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, d.data data, a.lavorazione, a.produzione, 'Collaboratori' gruppo, k.nome sottogruppo, COALESCE(NULLIF(coll.nomeSocieta, ''),NULLIF(forn.ragioneSociale, ''), NULLIF(coll.cognome, '') + ' ' + NULLIF(coll.nome, '')) fornitore, 0.00 listino, d.costo costo, e.pathDocumento  as documento, h.pregresso, '0' as codiceLavoro2   " +

                                    "from tab_dati_agenda a  " +
                                    "left join anag_clienti_fornitori b on b.id = a.id_cliente  " +
                                    "left join dati_lavorazione c on c.idDatiAgenda = a.id  " +
                                    "LEFT JOIN (SELECT idDatiLavorazione, fp_netto AS costo, idFornitori, idTipoSottogruppo,idCollaboratori, id, idTipoGruppo, idTipoGenere, data FROM dati_articoli_lavorazione) d ON d.idDatiLavorazione = c.id " +
                                    "left join tipo_protocollo g on  g.nome = 'Fattura'  " +
                                    "left join dati_protocollo e on e.codice_lavoro = a.codice_lavoro and e.attivo = 1 and e.id_tipo_protocollo = g.id and destinatario = 'Fornitore' and  e.id_cliente=d.idFornitori and e.id_dati_articoli_lavorazione=d.id " +
                                    //"left join tipo_protocollo i on  i.nome = 'Contratto'  " +
                                    //"left join dati_protocollo f on f.id=c.idContratto and f.id_tipo_protocollo = i.id  " +
                                    "left join tipo_protocollo j on  j.nome = 'Offerta'  " +
                                    "left join dati_protocollo h on h.codice_lavoro = a.codice_lavoro and h.id_tipo_protocollo = j.id and h.destinatario = 'Cliente'  and h.attivo = 1 " +
                                    "left join tipo_sottogruppo k on k.nome = 'Collaboratori a Fattura' " +
                                    "left join anag_clienti_fornitori forn on d.idFornitori is not NULL AND forn.id = d.idFornitori " +
                                    "left join anag_collaboratori coll on d.idCollaboratori is NOT NULL AND coll.id = d.idCollaboratori " +

                                    "where a.codice_lavoro is not null and a.id_stato >= 3 and d.costo <>0 and d.idTipoSottogruppo = k.id " + filtriLavorazione + filtriCosti +
                                    //" group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, f.descrizione, e.pathDocumento, h.pathDocumento, h.pregresso, k.nome, forn.ragioneSociale, coll.cognome, coll.nome, coll.nomeSocieta, d.id, d.costo, d.data " +
                                    " group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, e.pathDocumento, h.pathDocumento, h.pregresso, k.nome, forn.ragioneSociale, coll.cognome, coll.nome, coll.nomeSocieta, d.id, d.costo, d.data " +
            #endregion

            #region DIARIA
                                    "UNION " +
                                    //"select  4 progressivo, d.id, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, d.data data, a.lavorazione, a.produzione, l.nome gruppo, k.nome sottogruppo, COALESCE (coll.cognome+' '+coll.nome , forn.ragioneSociale) fornitore, 0.00 listino, d.costo costo, f.descrizione contratto, e.pathDocumento as documento, h.pregresso  " +
                                    "select  4 progressivo, d.id, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, d.data data, a.lavorazione, a.produzione, l.nome gruppo, k.nome sottogruppo, COALESCE (coll.cognome+' '+coll.nome , forn.ragioneSociale) fornitore, 0.00 listino, d.costo costo, e.pathDocumento as documento, h.pregresso, '0' as codiceLavoro2   " +

                                    "from tab_dati_agenda a  " +
                                    "left join anag_clienti_fornitori b on b.id = a.id_cliente  " +
                                    "left join dati_lavorazione c on c.idDatiAgenda = a.id  " +
                                    "LEFT JOIN (SELECT idDatiLavorazione, fp_lordo AS costo, idFornitori,idCollaboratori, idTipoGruppo, idTipoSottoGruppo, id, idTipoGenere, data  FROM dati_articoli_lavorazione ) d ON d.idDatiLavorazione = c.id  " +
                                    "left join tipo_protocollo g on  g.nome = 'Fattura'  " +
                                    "left join dati_protocollo e on e.codice_lavoro = a.codice_lavoro and e.attivo = 1 and e.id_tipo_protocollo = g.id and destinatario = 'Fornitore' and  e.id_cliente=d.idFornitori " +
                                    //"left join tipo_protocollo i on  i.nome = 'Contratto'  " +
                                    //"left join dati_protocollo f on f.id=c.idContratto and f.id_tipo_protocollo = i.id  " +
                                    "left join tipo_protocollo j on  j.nome = 'Offerta'  " +
                                    "left join dati_protocollo h on h.codice_lavoro = a.codice_lavoro and h.id_tipo_protocollo = j.id and h.destinatario = 'Cliente'  and h.attivo = 1 " +
                                    "left join tipo_sottogruppo k on k.nome = 'Diaria' " +
                                    "left join tipo_gruppo l on l.id = k.idTipoGruppo " +
                                    "left join anag_clienti_fornitori forn on d.idFornitori is not NULL AND forn.id = d.idFornitori  " +
                                    "left join anag_collaboratori coll on d.idCollaboratori is NOT NULL AND coll.id = d.idCollaboratori " +
                                    
                                    "where a.codice_lavoro is not null and a.id_stato >= 3 and  d.idTipoSottogruppo=k.id " + filtriLavorazione + filtriCosti +
                                    //" group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, f.descrizione, e.pathDocumento, h.pathDocumento, h.pregresso, k.nome, l.nome, forn.ragioneSociale, coll.cognome, coll.nome, d.id, d.costo, d.data " +
                                    " group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, e.pathDocumento, h.pathDocumento, h.pregresso, k.nome, l.nome, forn.ragioneSociale, coll.cognome, coll.nome, d.id, d.costo, d.data " +
            #endregion

            #region ELIMINATI
            #region TRASFERIMENTI
            //"UNION " +
            //"select distinct 4 progressivo, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, min(a.data_inizio_lavorazione) data, a.lavorazione, a.produzione, k.nome gruppo, '' fornitore, sum(d.prezzo) listino, sum(d.fp_lordo)  costo, f.descrizione contratto, e.pathDocumento 'docFattura', h.pathDocumento 'docOfferta', h.pregresso  " +
            //"from tab_dati_agenda a  " +
            //"left join anag_clienti_fornitori b on b.id = a.id_cliente  " +
            //"left join dati_lavorazione c on c.idDatiAgenda = a.id  " +
            //"left join dati_articoli_lavorazione d on d.idDatiLavorazione = c.id  " +
            //"left join tipo_protocollo g on  g.nome = 'Fattura'  " +
            //"left join dati_protocollo e on e.codice_lavoro = a.codice_lavoro and e.attivo = 1 and e.id_tipo_protocollo = g.id and destinatario = 'Cliente'  " +
            //"left join tipo_protocollo i on  i.nome = 'Contratto'  " +
            //"left join dati_protocollo f on f.id=c.idContratto and f.id_tipo_protocollo = i.id  " +
            //"left join tipo_protocollo j on  j.nome = 'Offerta'  " +
            //"left join dati_protocollo h on h.codice_lavoro = a.codice_lavoro and h.id_tipo_protocollo = j.id and h.destinatario = 'Cliente'  " +
            //"left join tipo_gruppo k on k.nome = 'Trasferimenti' " +
            //"where a.codice_lavoro is not null and a.id_stato >= 3 and d.fp_lordo >0 and d.idTipoGruppo=k.id " + filtri +
            //" group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, f.descrizione, e.pathDocumento, h.pathDocumento, h.pregresso, k.nome " +
            #endregion

            #region ALBERGO
            //"UNION " +
            //"select distinct 5 progressivo, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, min(a.data_inizio_lavorazione) data, a.lavorazione, a.produzione, l.nome gruppo, k.ragioneSociale fornitore, sum(d.prezzo) listino, sum(d.fp_lordo)  costo, f.descrizione contratto, e.pathDocumento 'docFattura', h.pathDocumento 'docOfferta', h.pregresso  " +
            //"from tab_dati_agenda a  " +
            //"left join anag_clienti_fornitori b on b.id = a.id_cliente  " +
            //"left join dati_lavorazione c on c.idDatiAgenda = a.id  " +
            //"left join dati_articoli_lavorazione d on d.idDatiLavorazione = c.id  " +
            //"left join tipo_protocollo g on  g.nome = 'Fattura'  " +
            //"left join dati_protocollo e on e.codice_lavoro = a.codice_lavoro and e.attivo = 1 and e.id_tipo_protocollo = g.id and destinatario = 'Cliente'  " +
            //"left join tipo_protocollo i on  i.nome = 'Contratto'  " +
            //"left join dati_protocollo f on f.id=c.idContratto and f.id_tipo_protocollo = i.id  " +
            //"left join tipo_protocollo j on  j.nome = 'Offerta'  " +
            //"left join dati_protocollo h on h.codice_lavoro = a.codice_lavoro and h.id_tipo_protocollo = j.id and h.destinatario = 'Cliente'  " +
            //"left join anag_clienti_fornitori k on d.idFornitori = k.id " +
            //"left join tipo_gruppo l on l.nome = 'Albergo' " +
            //"where a.codice_lavoro is not null and a.id_stato >= 3 and d.fp_lordo >0 and d.idTipoGruppo=l.id " + filtri +
            //" group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, f.descrizione, e.pathDocumento, h.pathDocumento, h.pregresso, l.nome, k.ragioneSociale " +
            #endregion
            #endregion

            #region TUTTO IL RESTO
                        "UNION " +
                        //"select distinct 5 progressivo, 0, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, min(a.data_inizio_lavorazione) data, a.lavorazione, a.produzione, l.nome gruppo, k.nome sottogruppo, COALESCE (coll.cognome+' '+coll.nome , forn.ragioneSociale) fornitore, 0.00 listino, d.costo costo, f.descrizione contratto, e.pathDocumento as documento, h.pregresso  " +
                        "select distinct 5 progressivo, 0, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, min(a.data_inizio_lavorazione) data, a.lavorazione, a.produzione, l.nome gruppo, k.nome sottogruppo, COALESCE (coll.cognome+' '+coll.nome , forn.ragioneSociale) fornitore, 0.00 listino, d.costo costo, e.pathDocumento as documento, h.pregresso, '0' as codiceLavoro2   " +

                        "from tab_dati_agenda a  " +
                        "left join anag_clienti_fornitori b on b.id = a.id_cliente  " +
                        "left join dati_lavorazione c on c.idDatiAgenda = a.id  " +
                        "LEFT JOIN (SELECT idDatiLavorazione, sum(fp_netto) AS costo, idFornitori,idTipoGruppo,idTipoSottogruppo, idTipoGenere, idCollaboratori, id  FROM dati_articoli_lavorazione GROUP BY idDatiLavorazione, idFornitori,idTipoGruppo,idTipoSottogruppo, idCollaboratori, idTipoGenere, id) d ON d.idDatiLavorazione = c.id " +
                        "left join tipo_protocollo g on  g.nome = 'Fattura'  " +
                        "left join dati_protocollo e on e.codice_lavoro = a.codice_lavoro and e.attivo = 1 and e.id_tipo_protocollo = g.id and destinatario = 'Fornitore' and  e.id_cliente=d.idFornitori and e.id_dati_articoli_lavorazione = d.id " +
                        //"left join tipo_protocollo i on  i.nome = 'Contratto'  " +
                        //"left join dati_protocollo f on f.id=c.idContratto and f.id_tipo_protocollo = i.id  " +
                        "left join tipo_protocollo j on  j.nome = 'Offerta'  " +
                        "left join dati_protocollo h on h.codice_lavoro = a.codice_lavoro and h.id_tipo_protocollo = j.id and h.destinatario = 'Cliente'  and h.attivo = 1 " +
                        "left join anag_clienti_fornitori forn on d.idFornitori = forn.id " +
                        "left join tipo_gruppo l on l.nome != 'Collaboratori' " +
                        "left join tipo_sottogruppo k on k.nome != 'Diaria' " +
                        "left join anag_collaboratori coll on d.idCollaboratori is NOT NULL AND coll.id = d.idCollaboratori " +

                        "where a.codice_lavoro is not null and a.id_stato >= 3 and d.costo <>0 and d.idTipoGruppo=l.id and d.idTipoSottogruppo=k.id " + filtriLavorazione + filtriCosti +
                        //" group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, f.descrizione, e.pathDocumento, h.pathDocumento, h.pregresso, l.nome, k.nome, forn.ragioneSociale, coll.cognome, coll.nome, d.costo" +
                        " group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, e.pathDocumento, h.pathDocumento, h.pregresso, l.nome, k.nome, forn.ragioneSociale, coll.cognome, coll.nome, d.costo" +
            #endregion
                                    " order by cliente, a.codice_lavoro, progressivo";

            return query;
        }

        private string CreaQueryListino(string elencoLavorazioni)
        {
            // SOLO IL PREZZO DI LISTINO
            //string query = "select distinct 1 progressivo, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, min(a.data_inizio_lavorazione) data, a.lavorazione, a.produzione, '' gruppo, '' sottogruppo, '' fornitore, sum(d.prezzo) listino, sum(d.fp_lordo)  costo, f.descrizione contratto, e.pathDocumento 'docFattura', h.pathDocumento 'docOfferta', h.pregresso " +
            //string query = "select top(1) 1 progressivo, 0, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, min(a.data_inizio_lavorazione) data, a.lavorazione, a.produzione, '' gruppo, '' sottogruppo, '' fornitore, d.prezzo listino, 0.00  costo, f.descrizione contratto, COALESCE(e.pathDocumento,h.pathDocumento) AS documento, h.pregresso " +
            string query = "select  1 progressivo, 0, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, min(a.data_inizio_lavorazione) data, a.lavorazione, a.produzione, '' gruppo, '' sottogruppo, '' fornitore, d.prezzo listino, 0.00  costo, MIN(COALESCE(e.pathDocumento,h.pathDocumento)) AS documento, h.pregresso, a.codice_lavoro as codiceLavoro2  " +
                                    "from tab_dati_agenda a  " +
                                    "left join anag_clienti_fornitori b on b.id = a.id_cliente  " +
                                    "left join dati_lavorazione c on c.idDatiAgenda = a.id  " +
                                    "LEFT JOIN (SELECT idDatiLavorazione, SUM(prezzo) AS prezzo FROM dati_articoli_lavorazione GROUP BY idDatiLavorazione) d ON d.idDatiLavorazione = c.id " +
                                    "left join tipo_protocollo g on  g.nome = 'Fattura'  " +
                                    "left join dati_protocollo e on e.codice_lavoro = a.codice_lavoro and e.attivo = 1 and e.id_tipo_protocollo = g.id and destinatario = 'Cliente'  " +
                                    //"left join tipo_protocollo i on  i.nome = 'Contratto'  " +
                                    //"left join dati_protocollo f on f.id=c.idContratto and f.id_tipo_protocollo = i.id  " +
                                    "left join tipo_protocollo j on  j.nome = 'Offerta'  " +
                                    "left join dati_protocollo h on h.codice_lavoro = a.codice_lavoro and h.id_tipo_protocollo = j.id and h.destinatario = 'Cliente'  and h.attivo = 1 " +
                                    "where a.codice_lavoro is not null and a.id_stato >= 3  and d.prezzo <> 0 AND a.codice_lavoro in (" + elencoLavorazioni + ") " +
                                    //"group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, f.descrizione, e.pathDocumento, h.pathDocumento, h.pregresso, d.prezzo ";
                                    "group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, h.pregresso, d.prezzo ";



            return query;
        }

        public List<StatisticheCosti> GetStatisticheCosti(string filtriLavorazione, ref Esito esito) 
        {
            List<StatisticheCosti> listaStatisticheCosti = new List<StatisticheCosti>();
            HashSet<string> lavorazioniAnomale = new HashSet<string>();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = CreaQuerySenzaFiltriCosti(filtriLavorazione);
                    
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                                {
                                    
                                    foreach (DataRow riga in dt.Rows)
                                    {
                                        try
                                        {
                                            StatisticheCosti statisticheCosti = new StatisticheCosti();

                                            statisticheCosti.Progressivo = riga.Field<int>("progressivo");

                                            statisticheCosti.IdCliente = riga.Field<int>("id_cliente");
                                            statisticheCosti.Cliente = riga.Field<string>("cliente");
                                            statisticheCosti.NumeroFattura = riga.Field<string>("numeroFattura");
                                            statisticheCosti.Ordine = riga.Field<string>("ordine");
                                            statisticheCosti.CodiceLavoro = riga.Field<string>("codice_lavoro");
                                            statisticheCosti.Data = riga.Field<DateTime?>("data");
                                            statisticheCosti.Lavorazione = riga.Field<string>("lavorazione");
                                            statisticheCosti.Produzione = riga.Field<string>("produzione");
                                            //statisticheCosti.Contratto = riga.Field<string>("contratto");
                                            statisticheCosti.Listino = riga.Field<decimal?>("listino");
                                            statisticheCosti.Costo = riga.Field<decimal?>("costo");

                                            //if (statisticheCosti.Progressivo == 1)
                                            //{
                                            //    statisticheCosti.DocumentoAllegato = riga.Field<string>("docFattura") != null ? riga.Field<string>("docFattura") : riga.Field<string>("docOfferta");
                                            //}
                                            //else
                                            //{
                                            //    statisticheCosti.DocumentoAllegato = riga.Field<string>("docFattura") != null ? riga.Field<string>("docFattura") : "";
                                            //}

                                            statisticheCosti.DocumentoAllegato = riga.Field<string>("documento");


                                            statisticheCosti.Pregresso = riga.Field<bool?>("pregresso");
                                            statisticheCosti.Gruppo = riga.Field<string>("gruppo") ?? "";
                                            statisticheCosti.Sottogruppo = riga.Field<string>("sottogruppo") ?? "";
                                            statisticheCosti.Fornitore = riga.Field<string>("fornitore");

                                            statisticheCosti.CodiceLavoro2 = riga.Field<string>("codiceLavoro2");

                                            listaStatisticheCosti.Add(statisticheCosti);
                                        }
                                        catch (Exception ex)
                                        {
                                            lavorazioniAnomale.Add(riga.Field<string>("codice_lavoro"));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.Descrizione = "Statistiche_DAL.cs - GetStatisticheCosti " + ex.Message + Environment.NewLine + ex.StackTrace;
            }
            if (lavorazioniAnomale.Count>0)
            {
                BasePage basePge = new BasePage();
                string messaggioLavorazioniAnomale = "Alcuni elementi non sono stati visualizzati perché le seguenti lavorazioni presentano anomalie:<ul>";
                foreach (string lavAnomala in lavorazioniAnomale)
                {
                    messaggioLavorazioniAnomale += "<li>" + lavAnomala + "</li>";
                }
                basePge.ShowWarning(messaggioLavorazioniAnomale + "</ul>");
            }
            return listaStatisticheCosti;
        }

        public List<StatisticheCosti> GetStatisticheCosti(string filtriLavorazione, string filtriCosti, ref Esito esito) 
        {
            List<StatisticheCosti> listaStatisticheCosti = new List<StatisticheCosti>();

            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = CreaQueryCosti(filtriLavorazione, filtriCosti);
                        
                    
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                                {
                                    foreach (DataRow riga in dt.Rows)
                                    {
                                        StatisticheCosti statisticheCosti = new StatisticheCosti();

                                        statisticheCosti.Progressivo = riga.Field<int>("progressivo");

                                        statisticheCosti.IdCliente = riga.Field<int>("id_cliente");
                                        statisticheCosti.Cliente = riga.Field<string>("cliente");
                                        statisticheCosti.NumeroFattura = riga.Field<string>("numeroFattura");
                                        statisticheCosti.Ordine = riga.Field<string>("ordine");
                                        statisticheCosti.CodiceLavoro = riga.Field<string>("codice_lavoro");
                                        statisticheCosti.Data = riga.Field<DateTime?>("data");
                                        statisticheCosti.Lavorazione = riga.Field<string>("lavorazione");
                                        statisticheCosti.Produzione = riga.Field<string>("produzione");
                                        //statisticheCosti.Contratto = riga.Field<string>("contratto");
                                        statisticheCosti.Listino = riga.Field<decimal?>("listino");
                                        statisticheCosti.Costo = riga.Field<decimal?>("costo");

                                        //if (statisticheCosti.Progressivo == 1)
                                        //{
                                        //    statisticheCosti.DocumentoAllegato = riga.Field<string>("docFattura") != null ? riga.Field<string>("docFattura") : riga.Field<string>("docOfferta");
                                        //}
                                        //else
                                        //{
                                        //    statisticheCosti.DocumentoAllegato = riga.Field<string>("docFattura") != null ? riga.Field<string>("docFattura") : "";// riga.Field<string>("docOfferta"),
                                        //}

                                        statisticheCosti.DocumentoAllegato = riga.Field<string>("documento");

                                        statisticheCosti.Pregresso = riga.Field<bool?>("pregresso");
                                        statisticheCosti.Gruppo = riga.Field<string>("gruppo") ?? "";
                                        statisticheCosti.Sottogruppo = riga.Field<string>("sottogruppo") ?? "";
                                        statisticheCosti.Fornitore = riga.Field<string>("fornitore");

                                        statisticheCosti.CodiceLavoro2 = riga.Field<string>("codiceLavoro2");

                                        listaStatisticheCosti.Add(statisticheCosti);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.Descrizione = "Statistiche_DAL.cs - GetStatisticheCosti " + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return listaStatisticheCosti;
        }

        public List<StatisticheCosti> GetStatisticheCostiListino(string elencoLavorazioni, ref Esito esito) 
        {
            List<StatisticheCosti> listaStatisticheCosti = new List<StatisticheCosti>();

            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = CreaQueryListino(elencoLavorazioni);
                    
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                                {
                                    foreach (DataRow riga in dt.Rows)
                                    {
                                        StatisticheCosti statisticheCosti = new StatisticheCosti();

                                        statisticheCosti.IdCliente = riga.Field<int>("id_cliente");
                                        statisticheCosti.Cliente = riga.Field<string>("cliente");
                                        statisticheCosti.NumeroFattura = riga.Field<string>("numeroFattura");
                                        statisticheCosti.Ordine = riga.Field<string>("ordine");
                                        statisticheCosti.CodiceLavoro = riga.Field<string>("codice_lavoro");
                                        statisticheCosti.Data = riga.Field<DateTime?>("data");
                                        statisticheCosti.Lavorazione = riga.Field<string>("lavorazione");
                                        statisticheCosti.Produzione = riga.Field<string>("produzione");
                                        //statisticheCosti.Contratto = riga.Field<string>("contratto");
                                        statisticheCosti.Listino = riga.Field<decimal?>("listino");
                                        statisticheCosti.Costo = riga.Field<decimal?>("costo");
                                        statisticheCosti.DocumentoAllegato = riga.Field<string>("documento"); //riga.Field<string>("docFattura") != null ? riga.Field<string>("docFattura") : riga.Field<string>("docOfferta");
                                        statisticheCosti.Pregresso = riga.Field<bool?>("pregresso");
                                        statisticheCosti.Gruppo = riga.Field<string>("gruppo") ?? "";
                                        statisticheCosti.Sottogruppo = riga.Field<string>("sottogruppo") ?? "";
                                        statisticheCosti.Fornitore = riga.Field<string>("fornitore");
                                        statisticheCosti.Progressivo = riga.Field<int>("progressivo");

                                        statisticheCosti.CodiceLavoro2 = riga.Field<string>("codiceLavoro2"); 

                                        listaStatisticheCosti.Add(statisticheCosti);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.Descrizione = "Statistiche_DAL.cs - GetStatisticheCosti " + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return listaStatisticheCosti;
        }
        #endregion
    }
}