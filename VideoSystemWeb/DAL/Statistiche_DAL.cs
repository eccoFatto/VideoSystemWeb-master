﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using VideoSystemWeb.BLL;
using VideoSystemWeb.Entity;

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

        public List<StatisticheRicavi> GetStatisticheRicavi(string filtroCliente, string filtroProduzione, string filtroLavorazione, string filtroContratto, bool? fatturato, string dataInizio, string dataFine, ref Esito esito)
        {
            List<StatisticheRicavi> listaStatisticheRicavi = new List<StatisticheRicavi>();
            HashSet<string> lavorazioniAnomale = new HashSet<string>();

            string filtri = string.Empty;
            filtri += string.IsNullOrWhiteSpace(filtroCliente) ? "" : " AND b.ragioneSociale like '%" + filtroCliente + "%' ";
            filtri += string.IsNullOrWhiteSpace(filtroProduzione) ? "" : " AND a.produzione like '%" + filtroProduzione + "%' ";
            filtri += string.IsNullOrWhiteSpace(filtroLavorazione) ? "" : " AND a.lavorazione like '%" + filtroLavorazione + "%' ";
            filtri += string.IsNullOrWhiteSpace(filtroContratto) ? "" : " AND f.descrizione like '%" + filtroContratto + "%' ";

            // eliminato campo docFattura in seguito alla segnalazione 02/07/2024 di risultati duplicati 
            //string campiQuery = "select distinct a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, min(a.data_inizio_lavorazione) data, a.lavorazione, a.produzione, SUM(d.prezzo) listino, sum(d.fp_lordo)  costo, f.descrizione contratto, e.pathDocumento 'docFattura', h.pathDocumento 'docOfferta', h.pregresso ";
            string campiQuery = "select distinct a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, min(a.data_inizio_lavorazione) data, a.lavorazione, a.produzione, SUM(d.prezzo) listino, sum(d.fp_lordo)  costo, f.descrizione contratto, h.pathDocumento 'docOfferta', h.pregresso ";

            if (fatturato != null)
            {
                if ((bool)fatturato)
                {
                    filtri +=  "and e.protocollo_riferimento is not null ";

                    filtri += string.IsNullOrWhiteSpace(dataInizio) ? "" : " AND e.data_fattura >= '" + dataInizio.Substring(6) + "-" + dataInizio.Substring(3, 2) + "-" + dataInizio.Substring(0, 2) + "T00:00:00.000'";
                    filtri += string.IsNullOrWhiteSpace(dataFine) ? "" : " AND e.data_fattura <= '" + dataFine.Substring(6) + "-" + dataFine.Substring(3, 2) + "-" + dataFine.Substring(0, 2) + "T00:00:00.000'";

                    // eliminato campo docOfferta in seguito alla segnalazione 02/07/2024 di risultati duplicati 
                    //campiQuery = "select distinct a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, min(e.data_fattura) data, a.lavorazione, a.produzione, SUM(d.prezzo) listino, sum(d.fp_lordo)  costo, f.descrizione contratto, e.pathDocumento 'docFattura', h.pathDocumento 'docOfferta', h.pregresso ";
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

            //if (fatturato != null)
            //{
            //    filtri += (bool)fatturato ? "and e.protocollo_riferimento is not null " : "and e.protocollo_riferimento is null ";
            //}
            //filtri += string.IsNullOrWhiteSpace(dataInizio) ? "" : " AND a.data_inizio_lavorazione >= '"+ dataInizio.Substring(6) + "-" + dataInizio.Substring(3,2) + "-" + dataInizio.Substring(0, 2) + "T00:00:00.000'";
            //// il filtro dataFine viene eseguito su dataInizioLavorazione, e non su dataFineLavorazione
            //filtri += string.IsNullOrWhiteSpace(dataFine) ? "" : " AND a.data_inizio_lavorazione <= '" + dataFine.Substring(6) + "-" + dataFine.Substring(3, 2) + "-" + dataFine.Substring(0, 2) + "T00:00:00.000'";
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

        //public List<StatisticheCosti> GetStatisticheCosti_OLD(string filtriLavorazione, string filtriCosti, ref Esito esito) //(string filtroCliente, string filtroProduzione, string filtroLavorazione, string filtroContratto, string filtroGenere, string filtroGruppo, string filtroSottogruppo, bool? fatturato, string dataInizio, string dataFine, string filtroFornitore, ref Esito esito)
        //{
        //    List<StatisticheCosti> listaStatisticheCosti = new List<StatisticheCosti>();

        //    //string filtriLavorazione = string.Empty;
        //    //filtriLavorazione += string.IsNullOrWhiteSpace(filtroCliente) ? "" : " AND b.ragioneSociale like '%" + filtroCliente + "%' ";
        //    //filtriLavorazione += string.IsNullOrWhiteSpace(filtroProduzione) ? "" : " AND a.produzione like '%" + filtroProduzione + "%' ";
        //    //filtriLavorazione += string.IsNullOrWhiteSpace(filtroLavorazione) ? "" : " AND a.codice_lavoro = '" + filtroLavorazione + "' ";
        //    //filtriLavorazione += string.IsNullOrWhiteSpace(filtroContratto) ? "" : " AND f.descrizione like '%" + filtroContratto + "%' ";

        //    //if (fatturato != null)
        //    //{
        //    //    filtriLavorazione += (bool)fatturato ? "and e.protocollo_riferimento is not null " : "and e.protocollo_riferimento is null ";
        //    //}
        //    //filtriLavorazione += string.IsNullOrWhiteSpace(dataInizio) ? "" : " AND a.data_inizio_lavorazione >= '" + dataInizio.Substring(6) + "-" + dataInizio.Substring(3, 2) + "-" + dataInizio.Substring(0, 2) + "T00:00:00.000'";
        //    //// il filtro dataFine viene eseguito su dataInizioLavorazione, e non su dataFineLavorazione
        //    //filtriLavorazione += string.IsNullOrWhiteSpace(dataFine) ? "" : " AND a.data_inizio_lavorazione <= '" + dataFine.Substring(6) + "-" + dataFine.Substring(3, 2) + "-" + dataFine.Substring(0, 2) + "T00:00:00.000'";


        //    //string filtriCosti = string.Empty; ; // i filtri seguenti devono essere applicati solo alle categorie costi

        //    //filtriCosti += string.IsNullOrWhiteSpace(filtroGenere) ? "" : " AND d.idTipoGenere = '" + filtroGenere + "' ";
        //    //filtriCosti += string.IsNullOrWhiteSpace(filtroGruppo) ? "" : " AND d.idTipoGruppo = '" + filtroGruppo + "' ";
        //    //filtriCosti += string.IsNullOrWhiteSpace(filtroSottogruppo) ? "" : " AND d.idTipoSottogruppo = '" + filtroContratto + "' ";
        //    //filtriCosti += string.IsNullOrWhiteSpace(filtroFornitore) ? "" : "AND forn.ragioneSociale like '%" + filtroFornitore + "%' ";

        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(sqlConstr))
        //        {
        //            string query = string.Empty;

        //            if (string.IsNullOrEmpty(filtriCosti))
        //            {
        //                query = CreaQuerySenzaFiltriCosti(filtriLavorazione);
        //            }
        //            else
        //            {
        //                query = CreaQuery(filtriLavorazione, filtriCosti);
        //                //query = "select distinct 1 progressivo, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, min(a.data_inizio_lavorazione) data, a.lavorazione, a.produzione, '' gruppo, '' fornitore, sum(d.prezzo) listino, sum(d.fp_lordo)  costo, f.descrizione contratto, e.pathDocumento 'docFattura', h.pathDocumento 'docOfferta', h.pregresso " +
        //                //            "from tab_dati_agenda a  " +
        //                //            "left join anag_clienti_fornitori b on b.id = a.id_cliente  " +
        //                //            "left join dati_lavorazione c on c.idDatiAgenda = a.id  " +
        //                //            "left join dati_articoli_lavorazione d on d.idDatiLavorazione = c.id  " +
        //                //            "left join tipo_protocollo g on  g.nome = 'Fattura'  " +
        //                //            "left join dati_protocollo e on e.codice_lavoro = a.codice_lavoro and e.attivo = 1 and e.id_tipo_protocollo = g.id and destinatario = 'Cliente'  " +
        //                //            "left join tipo_protocollo i on  i.nome = 'Contratto'  " +
        //                //            "left join dati_protocollo f on f.id=c.idContratto and f.id_tipo_protocollo = i.id  " +
        //                //            "left join tipo_protocollo j on  j.nome = 'Offerta'  " +
        //                //            "left join dati_protocollo h on h.codice_lavoro = a.codice_lavoro and h.id_tipo_protocollo = j.id and h.destinatario = 'Cliente'  " +
        //                //            "where a.codice_lavoro is not null and a.id_stato >= 3  and d.prezzo > 0 " + filtriLavorazione +
        //                //            " group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, f.descrizione, e.pathDocumento, h.pathDocumento, h.pregresso " +
        //                //#region COLLABORATORI
        //                //            "UNION " +
        //                //            "select distinct 2 progressivo, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, min(a.data_inizio_lavorazione) data, a.lavorazione, a.produzione, k.nome gruppo, '' fornitore, sum(d.prezzo) listino, sum(d.fp_lordo) costo, f.descrizione contratto, e.pathDocumento 'docFattura', h.pathDocumento 'docOfferta', h.pregresso  " +
        //                //            "from tab_dati_agenda a  " +
        //                //            "left join anag_clienti_fornitori b on b.id = a.id_cliente  " +
        //                //            "left join dati_lavorazione c on c.idDatiAgenda = a.id  " +
        //                //            "left join dati_articoli_lavorazione d on d.idDatiLavorazione = c.id  " +
        //                //            "left join tipo_protocollo g on  g.nome = 'Fattura'  " +
        //                //            "left join dati_protocollo e on e.codice_lavoro = a.codice_lavoro and e.attivo = 1 and e.id_tipo_protocollo = g.id and destinatario = 'Cliente'  " +
        //                //            "left join tipo_protocollo i on  i.nome = 'Contratto'  " +
        //                //            "left join dati_protocollo f on f.id=c.idContratto and f.id_tipo_protocollo = i.id  " +
        //                //            "left join tipo_protocollo j on  j.nome = 'Offerta'  " +
        //                //            "left join dati_protocollo h on h.codice_lavoro = a.codice_lavoro and h.id_tipo_protocollo = j.id and h.destinatario = 'Cliente'  " +
        //                //            "left join tipo_gruppo k on k.nome = 'Collaboratori' " +
        //                //            "left join anag_clienti_fornitori forn on d.idFornitori = forn.id " +
        //                //            "where a.codice_lavoro is not null and a.id_stato >= 3 and d.fp_lordo >0 and d.idTipoGruppo = k.id " + filtriLavorazione + filtriCosti +
        //                //            " group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, f.descrizione, e.pathDocumento, h.pathDocumento, h.pregresso, k.nome " +
        //                //#endregion

        //                //#region DIARIA
        //                //            "UNION " +
        //                //            "select distinct 3 progressivo, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, min(a.data_inizio_lavorazione) data, a.lavorazione, a.produzione, k.nome gruppo, '' fornitore, sum(d.prezzo) listino, sum(d.fp_lordo)  costo, f.descrizione contratto, e.pathDocumento 'docFattura', h.pathDocumento 'docOfferta', h.pregresso  " +
        //                //            "from tab_dati_agenda a  " +
        //                //            "left join anag_clienti_fornitori b on b.id = a.id_cliente  " +
        //                //            "left join dati_lavorazione c on c.idDatiAgenda = a.id  " +
        //                //            "left join dati_articoli_lavorazione d on d.idDatiLavorazione = c.id  " +
        //                //            "left join tipo_protocollo g on  g.nome = 'Fattura'  " +
        //                //            "left join dati_protocollo e on e.codice_lavoro = a.codice_lavoro and e.attivo = 1 and e.id_tipo_protocollo = g.id and destinatario = 'Cliente'  " +
        //                //            "left join tipo_protocollo i on  i.nome = 'Contratto'  " +
        //                //            "left join dati_protocollo f on f.id=c.idContratto and f.id_tipo_protocollo = i.id  " +
        //                //            "left join tipo_protocollo j on  j.nome = 'Offerta'  " +
        //                //            "left join dati_protocollo h on h.codice_lavoro = a.codice_lavoro and h.id_tipo_protocollo = j.id and h.destinatario = 'Cliente'  " +
        //                //            "left join tipo_gruppo k on k.nome = 'Diaria' " +
        //                //            "left join anag_clienti_fornitori forn on d.idFornitori = forn.id " +
        //                //            "where a.codice_lavoro is not null and a.id_stato >= 3 and  d.idTipoGruppo=k.id " + filtriLavorazione + filtriCosti +
        //                //            " group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, f.descrizione, e.pathDocumento, h.pathDocumento, h.pregresso, k.nome " +
        //                //#endregion

        //                //#region ELIMINATI
        //                //#region TRASFERIMENTI
        //                //            //"UNION " +
        //                //            //"select distinct 4 progressivo, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, min(a.data_inizio_lavorazione) data, a.lavorazione, a.produzione, k.nome gruppo, '' fornitore, sum(d.prezzo) listino, sum(d.fp_lordo)  costo, f.descrizione contratto, e.pathDocumento 'docFattura', h.pathDocumento 'docOfferta', h.pregresso  " +
        //                //            //"from tab_dati_agenda a  " +
        //                //            //"left join anag_clienti_fornitori b on b.id = a.id_cliente  " +
        //                //            //"left join dati_lavorazione c on c.idDatiAgenda = a.id  " +
        //                //            //"left join dati_articoli_lavorazione d on d.idDatiLavorazione = c.id  " +
        //                //            //"left join tipo_protocollo g on  g.nome = 'Fattura'  " +
        //                //            //"left join dati_protocollo e on e.codice_lavoro = a.codice_lavoro and e.attivo = 1 and e.id_tipo_protocollo = g.id and destinatario = 'Cliente'  " +
        //                //            //"left join tipo_protocollo i on  i.nome = 'Contratto'  " +
        //                //            //"left join dati_protocollo f on f.id=c.idContratto and f.id_tipo_protocollo = i.id  " +
        //                //            //"left join tipo_protocollo j on  j.nome = 'Offerta'  " +
        //                //            //"left join dati_protocollo h on h.codice_lavoro = a.codice_lavoro and h.id_tipo_protocollo = j.id and h.destinatario = 'Cliente'  " +
        //                //            //"left join tipo_gruppo k on k.nome = 'Trasferimenti' " +
        //                //            //"where a.codice_lavoro is not null and a.id_stato >= 3 and d.fp_lordo >0 and d.idTipoGruppo=k.id " + filtri +
        //                //            //" group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, f.descrizione, e.pathDocumento, h.pathDocumento, h.pregresso, k.nome " +
        //                //#endregion

        //                //#region ALBERGO
        //                //            //"UNION " +
        //                //            //"select distinct 5 progressivo, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, min(a.data_inizio_lavorazione) data, a.lavorazione, a.produzione, l.nome gruppo, k.ragioneSociale fornitore, sum(d.prezzo) listino, sum(d.fp_lordo)  costo, f.descrizione contratto, e.pathDocumento 'docFattura', h.pathDocumento 'docOfferta', h.pregresso  " +
        //                //            //"from tab_dati_agenda a  " +
        //                //            //"left join anag_clienti_fornitori b on b.id = a.id_cliente  " +
        //                //            //"left join dati_lavorazione c on c.idDatiAgenda = a.id  " +
        //                //            //"left join dati_articoli_lavorazione d on d.idDatiLavorazione = c.id  " +
        //                //            //"left join tipo_protocollo g on  g.nome = 'Fattura'  " +
        //                //            //"left join dati_protocollo e on e.codice_lavoro = a.codice_lavoro and e.attivo = 1 and e.id_tipo_protocollo = g.id and destinatario = 'Cliente'  " +
        //                //            //"left join tipo_protocollo i on  i.nome = 'Contratto'  " +
        //                //            //"left join dati_protocollo f on f.id=c.idContratto and f.id_tipo_protocollo = i.id  " +
        //                //            //"left join tipo_protocollo j on  j.nome = 'Offerta'  " +
        //                //            //"left join dati_protocollo h on h.codice_lavoro = a.codice_lavoro and h.id_tipo_protocollo = j.id and h.destinatario = 'Cliente'  " +
        //                //            //"left join anag_clienti_fornitori k on d.idFornitori = k.id " +
        //                //            //"left join tipo_gruppo l on l.nome = 'Albergo' " +
        //                //            //"where a.codice_lavoro is not null and a.id_stato >= 3 and d.fp_lordo >0 and d.idTipoGruppo=l.id " + filtri +
        //                //            //" group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, f.descrizione, e.pathDocumento, h.pathDocumento, h.pregresso, l.nome, k.ragioneSociale " +
        //                //#endregion
        //                //#endregion

        //                //#region TUTTO IL RESTO
        //                //            "UNION " +
        //                //            "select distinct 6 progressivo, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, min(a.data_inizio_lavorazione) data, a.lavorazione, a.produzione, d.descrizione gruppo, forn.ragioneSociale fornitore, d.prezzo listino, d.fp_lordo  costo, f.descrizione contratto, e.pathDocumento 'docFattura', h.pathDocumento 'docOfferta', h.pregresso  " +
        //                //            "from tab_dati_agenda a  " +
        //                //            "left join anag_clienti_fornitori b on b.id = a.id_cliente  " +
        //                //            "left join dati_lavorazione c on c.idDatiAgenda = a.id  " +
        //                //            "left join dati_articoli_lavorazione d on d.idDatiLavorazione = c.id  " +
        //                //            "left join tipo_protocollo g on  g.nome = 'Fattura'  " +
        //                //            "left join dati_protocollo e on e.codice_lavoro = a.codice_lavoro and e.attivo = 1 and e.id_tipo_protocollo = g.id and destinatario = 'Cliente'  " +
        //                //            "left join tipo_protocollo i on  i.nome = 'Contratto'  " +
        //                //            "left join dati_protocollo f on f.id=c.idContratto and f.id_tipo_protocollo = i.id  " +
        //                //            "left join tipo_protocollo j on  j.nome = 'Offerta'  " +
        //                //            "left join dati_protocollo h on h.codice_lavoro = a.codice_lavoro and h.id_tipo_protocollo = j.id and h.destinatario = 'Cliente'  " +
        //                //            "left join anag_clienti_fornitori forn on d.idFornitori = forn.id " +
        //                //            "left join tipo_gruppo l on l.nome not in ('Collaboratori', 'Diaria') " +
        //                //            "where a.codice_lavoro is not null and a.id_stato >= 3 and d.fp_lordo >0 and d.idTipoGruppo=l.id " + filtriLavorazione + filtriCosti +
        //                //            " group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, f.descrizione, e.pathDocumento, h.pathDocumento, h.pregresso, d.descrizione, forn.ragioneSociale, d.prezzo, d.fp_lordo " +
        //                //#endregion
        //                //            " order by cliente, a.codice_lavoro, progressivo";
        //            }
        //            using (SqlCommand cmd = new SqlCommand(query))
        //            {
        //                using (SqlDataAdapter sda = new SqlDataAdapter())
        //                {
        //                    cmd.Connection = con;
        //                    sda.SelectCommand = cmd;
        //                    using (DataTable dt = new DataTable())
        //                    {
        //                        sda.Fill(dt);
        //                        if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
        //                        {
        //                            foreach (DataRow riga in dt.Rows)
        //                            {
        //                                StatisticheCosti statisticheCosti = new StatisticheCosti
        //                                {
        //                                    IdCliente = riga.Field<int>("id_cliente"),
        //                                    Cliente = riga.Field<string>("cliente"),
        //                                    NumeroFattura = riga.Field<string>("numeroFattura"),
        //                                    Ordine = riga.Field<string>("ordine"),
        //                                    CodiceLavoro = riga.Field<string>("codice_lavoro"),
        //                                    Data = riga.Field<DateTime?>("data"),
        //                                    Lavorazione = riga.Field<string>("lavorazione"),
        //                                    Produzione = riga.Field<string>("produzione"),
        //                                    Contratto = riga.Field<string>("contratto"),
        //                                    Listino = riga.Field<decimal?>("listino"),
        //                                    Costo = riga.Field<decimal?>("costo"),
        //                                    DocumentoAllegato = riga.Field<string>("docFattura") != null ? riga.Field<string>("docFattura") : riga.Field<string>("docOfferta"),
        //                                    Pregresso = riga.Field<bool>("pregresso"),
        //                                    Gruppo = riga.Field<string>("gruppo"),
        //                                    Fornitore = riga.Field<string>("fornitore")
        //                                };

        //                                listaStatisticheCosti.Add(statisticheCosti);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
        //        esito.Descrizione = "Statistiche_DAL.cs - GetStatisticheCosti " + ex.Message + Environment.NewLine + ex.StackTrace;
        //    }

        //    return listaStatisticheCosti;
        //}

        private string CreaQuerySenzaFiltriCosti(string filtriLavorazione)
        {
            string query = "select distinct 1 progressivo, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, min(a.data_inizio_lavorazione) data, a.lavorazione, a.produzione, '' gruppo, '' fornitore, sum(d.prezzo) listino, sum(d.fp_lordo)  costo, f.descrizione contratto, e.pathDocumento 'docFattura', h.pathDocumento 'docOfferta', h.pregresso " +
                                    "from tab_dati_agenda a  " +
                                    "left join anag_clienti_fornitori b on b.id = a.id_cliente  " +
                                    "left join dati_lavorazione c on c.idDatiAgenda = a.id  " +
                                    "left join dati_articoli_lavorazione d on d.idDatiLavorazione = c.id  " +
                                    "left join tipo_protocollo g on  g.nome = 'Fattura'  " +
                                    "left join dati_protocollo e on e.codice_lavoro = a.codice_lavoro and e.attivo = 1 and e.id_tipo_protocollo = g.id and destinatario = 'Cliente'  " +
                                    "left join tipo_protocollo i on  i.nome = 'Contratto'  " +
                                    "left join dati_protocollo f on f.id=c.idContratto and f.id_tipo_protocollo = i.id  " +
                                    "left join tipo_protocollo j on  j.nome = 'Offerta'  " +
                                    "left join dati_protocollo h on h.codice_lavoro = a.codice_lavoro and h.id_tipo_protocollo = j.id and h.destinatario = 'Cliente'  " +
                                    "where a.codice_lavoro is not null and a.id_stato >= 3  and d.prezzo <> 0 " + filtriLavorazione +
                                    " group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, f.descrizione, e.pathDocumento, h.pathDocumento, h.pregresso " +
            #region COLLABORATORI
                                    "UNION " +
                                    "select distinct 2 progressivo, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, min(a.data_inizio_lavorazione) data, a.lavorazione, a.produzione, k.nome gruppo, '' fornitore, sum(d.prezzo) listino, sum(d.fp_lordo) costo, f.descrizione contratto, e.pathDocumento 'docFattura', h.pathDocumento 'docOfferta', h.pregresso  " +
                                    "from tab_dati_agenda a  " +
                                    "left join anag_clienti_fornitori b on b.id = a.id_cliente  " +
                                    "left join dati_lavorazione c on c.idDatiAgenda = a.id  " +
                                    "left join dati_articoli_lavorazione d on d.idDatiLavorazione = c.id  " +
                                    "left join tipo_protocollo g on  g.nome = 'Fattura'  " +
                                    "left join dati_protocollo e on e.codice_lavoro = a.codice_lavoro and e.attivo = 1 and e.id_tipo_protocollo = g.id and destinatario = 'Fornitore' and  e.id_cliente=d.idFornitori " +
                                    "left join tipo_protocollo i on  i.nome = 'Contratto'  " +
                                    "left join dati_protocollo f on f.id=c.idContratto and f.id_tipo_protocollo = i.id  " +
                                    "left join tipo_protocollo j on  j.nome = 'Offerta'  " +
                                    "left join dati_protocollo h on h.codice_lavoro = a.codice_lavoro and h.id_tipo_protocollo = j.id and h.destinatario = 'Cliente'  " +
                                    "left join tipo_gruppo k on k.nome = 'Collaboratori' " +
                                    "left join anag_clienti_fornitori forn on d.idFornitori = forn.id " +
                                    "where a.codice_lavoro is not null and a.id_stato >= 3 and d.fp_lordo <>0 and d.idTipoGruppo = k.id " + filtriLavorazione +
                                    " group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, f.descrizione, e.pathDocumento, h.pathDocumento, h.pregresso, k.nome " +
            #endregion

            #region DIARIA
                                    "UNION " +
                                    "select distinct 3 progressivo, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, min(a.data_inizio_lavorazione) data, a.lavorazione, a.produzione, k.nome gruppo, '' fornitore, sum(d.prezzo) listino, sum(d.fp_lordo)  costo, f.descrizione contratto, e.pathDocumento 'docFattura', h.pathDocumento 'docOfferta', h.pregresso  " +
                                    "from tab_dati_agenda a  " +
                                    "left join anag_clienti_fornitori b on b.id = a.id_cliente  " +
                                    "left join dati_lavorazione c on c.idDatiAgenda = a.id  " +
                                    "left join dati_articoli_lavorazione d on d.idDatiLavorazione = c.id  " +
                                    "left join tipo_protocollo g on  g.nome = 'Fattura'  " +
                                    "left join dati_protocollo e on e.codice_lavoro = a.codice_lavoro and e.attivo = 1 and e.id_tipo_protocollo = g.id and destinatario = 'Fornitore' and  e.id_cliente=d.idFornitori " +
                                    "left join tipo_protocollo i on  i.nome = 'Contratto'  " +
                                    "left join dati_protocollo f on f.id=c.idContratto and f.id_tipo_protocollo = i.id  " +
                                    "left join tipo_protocollo j on  j.nome = 'Offerta'  " +
                                    "left join dati_protocollo h on h.codice_lavoro = a.codice_lavoro and h.id_tipo_protocollo = j.id and h.destinatario = 'Cliente'  " +
                                    "left join tipo_gruppo k on k.nome = 'Diaria' " +
                                    "left join anag_clienti_fornitori forn on d.idFornitori = forn.id " +
                                    "where a.codice_lavoro is not null and a.id_stato >= 3 and  d.idTipoGruppo=k.id " + filtriLavorazione + 
                                    " group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, f.descrizione, e.pathDocumento, h.pathDocumento, h.pregresso, k.nome " +
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
                                    "select distinct 6 progressivo, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, min(a.data_inizio_lavorazione) data, a.lavorazione, a.produzione, d.descrizione gruppo, forn.ragioneSociale fornitore, d.prezzo listino, d.fp_lordo  costo, f.descrizione contratto, e.pathDocumento 'docFattura', h.pathDocumento 'docOfferta', h.pregresso  " +
                                    "from tab_dati_agenda a  " +
                                    "left join anag_clienti_fornitori b on b.id = a.id_cliente  " +
                                    "left join dati_lavorazione c on c.idDatiAgenda = a.id  " +
                                    "left join dati_articoli_lavorazione d on d.idDatiLavorazione = c.id  " +
                                    "left join tipo_protocollo g on  g.nome = 'Fattura'  " +
                                    "left join dati_protocollo e on e.codice_lavoro = a.codice_lavoro and e.attivo = 1 and e.id_tipo_protocollo = g.id and destinatario = 'Fornitore' and  e.id_cliente=d.idFornitori " +
                                    "left join tipo_protocollo i on  i.nome = 'Contratto'  " +
                                    "left join dati_protocollo f on f.id=c.idContratto and f.id_tipo_protocollo = i.id  " +
                                    "left join tipo_protocollo j on  j.nome = 'Offerta'  " +
                                    "left join dati_protocollo h on h.codice_lavoro = a.codice_lavoro and h.id_tipo_protocollo = j.id and h.destinatario = 'Cliente'  " +
                                    "left join anag_clienti_fornitori forn on d.idFornitori = forn.id " +
                                    "left join tipo_gruppo l on l.nome not in ('Collaboratori', 'Diaria') " +
                                    "where a.codice_lavoro is not null and a.id_stato >= 3 and d.fp_lordo <>0 and d.idTipoGruppo=l.id " + filtriLavorazione + 
                                    " group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, f.descrizione, e.pathDocumento, h.pathDocumento, h.pregresso, d.descrizione, forn.ragioneSociale, d.prezzo, d.fp_lordo " +
            #endregion
                                    " order by cliente, a.codice_lavoro, progressivo";

            return query;
        }

        private string CreaQuery(string filtriLavorazione, string filtriCosti)
        {
            string query = "select distinct 1 progressivo, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, min(a.data_inizio_lavorazione) data, a.lavorazione, a.produzione, '' gruppo, '' fornitore, sum(d.prezzo) listino, sum(d.fp_lordo)  costo, f.descrizione contratto, e.pathDocumento 'docFattura', h.pathDocumento 'docOfferta', h.pregresso " +
                                    "from tab_dati_agenda a  " +
                                    "left join anag_clienti_fornitori b on b.id = a.id_cliente  " +
                                    "left join dati_lavorazione c on c.idDatiAgenda = a.id  " +
                                    "left join dati_articoli_lavorazione d on d.idDatiLavorazione = c.id  " +
                                    "left join tipo_protocollo g on  g.nome = 'Fattura'  " +
                                    "left join dati_protocollo e on e.codice_lavoro = a.codice_lavoro and e.attivo = 1 and e.id_tipo_protocollo = g.id and destinatario = 'Cliente'  " +
                                    "left join tipo_protocollo i on  i.nome = 'Contratto'  " +
                                    "left join dati_protocollo f on f.id=c.idContratto and f.id_tipo_protocollo = i.id  " +
                                    "left join tipo_protocollo j on  j.nome = 'Offerta'  " +
                                    "left join dati_protocollo h on h.codice_lavoro = a.codice_lavoro and h.id_tipo_protocollo = j.id and h.destinatario = 'Cliente'  " +
                                    "where a.codice_lavoro is not null and a.id_stato >= 3  and d.prezzo <> 0 " + filtriLavorazione +
                                    " group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, f.descrizione, e.pathDocumento, h.pathDocumento, h.pregresso " +
            #region COLLABORATORI
                                    "UNION " +
                                    "select distinct 2 progressivo, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, min(a.data_inizio_lavorazione) data, a.lavorazione, a.produzione, k.nome gruppo, '' fornitore, sum(d.prezzo) listino, sum(d.fp_lordo) costo, f.descrizione contratto, e.pathDocumento 'docFattura', h.pathDocumento 'docOfferta', h.pregresso  " +
                                    "from tab_dati_agenda a  " +
                                    "left join anag_clienti_fornitori b on b.id = a.id_cliente  " +
                                    "left join dati_lavorazione c on c.idDatiAgenda = a.id  " +
                                    "left join dati_articoli_lavorazione d on d.idDatiLavorazione = c.id  " +
                                    "left join tipo_protocollo g on  g.nome = 'Fattura'  " +
                                    "left join dati_protocollo e on e.codice_lavoro = a.codice_lavoro and e.attivo = 1 and e.id_tipo_protocollo = g.id and destinatario = 'Fornitore' and  e.id_cliente=d.idFornitori " +
                                    "left join tipo_protocollo i on  i.nome = 'Contratto'  " +
                                    "left join dati_protocollo f on f.id=c.idContratto and f.id_tipo_protocollo = i.id  " +
                                    "left join tipo_protocollo j on  j.nome = 'Offerta'  " +
                                    "left join dati_protocollo h on h.codice_lavoro = a.codice_lavoro and h.id_tipo_protocollo = j.id and h.destinatario = 'Cliente'  " +
                                    "left join tipo_gruppo k on k.nome = 'Collaboratori' " +
                                    "left join anag_clienti_fornitori forn on d.idFornitori = forn.id " +
                                    "where a.codice_lavoro is not null and a.id_stato >= 3 and d.fp_lordo <>0 and d.idTipoGruppo = k.id " + filtriLavorazione + filtriCosti +
                                    " group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, f.descrizione, e.pathDocumento, h.pathDocumento, h.pregresso, k.nome " +
            #endregion

            #region DIARIA
                                    "UNION " +
                                    "select distinct 3 progressivo, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, min(a.data_inizio_lavorazione) data, a.lavorazione, a.produzione, k.nome gruppo, '' fornitore, sum(d.prezzo) listino, sum(d.fp_lordo)  costo, f.descrizione contratto, e.pathDocumento 'docFattura', h.pathDocumento 'docOfferta', h.pregresso  " +
                                    "from tab_dati_agenda a  " +
                                    "left join anag_clienti_fornitori b on b.id = a.id_cliente  " +
                                    "left join dati_lavorazione c on c.idDatiAgenda = a.id  " +
                                    "left join dati_articoli_lavorazione d on d.idDatiLavorazione = c.id  " +
                                    "left join tipo_protocollo g on  g.nome = 'Fattura'  " +
                                    "left join dati_protocollo e on e.codice_lavoro = a.codice_lavoro and e.attivo = 1 and e.id_tipo_protocollo = g.id and destinatario = 'Fornitore' and  e.id_cliente=d.idFornitori " +
                                    "left join tipo_protocollo i on  i.nome = 'Contratto'  " +
                                    "left join dati_protocollo f on f.id=c.idContratto and f.id_tipo_protocollo = i.id  " +
                                    "left join tipo_protocollo j on  j.nome = 'Offerta'  " +
                                    "left join dati_protocollo h on h.codice_lavoro = a.codice_lavoro and h.id_tipo_protocollo = j.id and h.destinatario = 'Cliente'  " +
                                    "left join tipo_gruppo k on k.nome = 'Diaria' " +
                                    "left join anag_clienti_fornitori forn on d.idFornitori = forn.id " +
                                    "where a.codice_lavoro is not null and a.id_stato >= 3 and  d.idTipoGruppo=k.id " + filtriLavorazione + filtriCosti +
                                    " group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, f.descrizione, e.pathDocumento, h.pathDocumento, h.pregresso, k.nome " +
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
                                    "select distinct 6 progressivo, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, min(a.data_inizio_lavorazione) data, a.lavorazione, a.produzione, d.descrizione gruppo, forn.ragioneSociale fornitore, d.prezzo listino, d.fp_lordo  costo, f.descrizione contratto, e.pathDocumento 'docFattura', h.pathDocumento 'docOfferta', h.pregresso  " +
                                    "from tab_dati_agenda a  " +
                                    "left join anag_clienti_fornitori b on b.id = a.id_cliente  " +
                                    "left join dati_lavorazione c on c.idDatiAgenda = a.id  " +
                                    "left join dati_articoli_lavorazione d on d.idDatiLavorazione = c.id  " +
                                    "left join tipo_protocollo g on  g.nome = 'Fattura'  " +
                                    "left join dati_protocollo e on e.codice_lavoro = a.codice_lavoro and e.attivo = 1 and e.id_tipo_protocollo = g.id and destinatario = 'Fornitore' and  e.id_cliente=d.idFornitori " +
                                    "left join tipo_protocollo i on  i.nome = 'Contratto'  " +
                                    "left join dati_protocollo f on f.id=c.idContratto and f.id_tipo_protocollo = i.id  " +
                                    "left join tipo_protocollo j on  j.nome = 'Offerta'  " +
                                    "left join dati_protocollo h on h.codice_lavoro = a.codice_lavoro and h.id_tipo_protocollo = j.id and h.destinatario = 'Cliente'  " +
                                    "left join anag_clienti_fornitori forn on d.idFornitori = forn.id " +
                                    "left join tipo_gruppo l on l.nome not in ('Collaboratori', 'Diaria') " +
                                    "where a.codice_lavoro is not null and a.id_stato >= 3 and d.fp_lordo >0 and d.idTipoGruppo=l.id " + filtriLavorazione + filtriCosti +
                                    " group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, f.descrizione, e.pathDocumento, h.pathDocumento, h.pregresso, d.descrizione, forn.ragioneSociale, d.prezzo, d.fp_lordo " +
            #endregion
                                    " order by cliente, a.codice_lavoro, progressivo";

            return query;
        }

        private string CreaQueryCosti(string filtriLavorazione, string filtriCosti)
        {
            string query = 
            #region COLLABORATORI
                                    "select distinct 2 progressivo, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, min(a.data_inizio_lavorazione) data, a.lavorazione, a.produzione, k.nome gruppo, '' fornitore, sum(d.prezzo) listino, sum(d.fp_lordo) costo, f.descrizione contratto, e.pathDocumento 'docFattura', h.pathDocumento 'docOfferta', h.pregresso  " +
                                    "from tab_dati_agenda a  " +
                                    "left join anag_clienti_fornitori b on b.id = a.id_cliente  " +
                                    "left join dati_lavorazione c on c.idDatiAgenda = a.id  " +
                                    "left join dati_articoli_lavorazione d on d.idDatiLavorazione = c.id  " +
                                    "left join tipo_protocollo g on  g.nome = 'Fattura'  " +
                                    "left join dati_protocollo e on e.codice_lavoro = a.codice_lavoro and e.attivo = 1 and e.id_tipo_protocollo = g.id and destinatario = 'Fornitore' and  e.id_cliente=d.idFornitori " +
                                    "left join tipo_protocollo i on  i.nome = 'Contratto'  " +
                                    "left join dati_protocollo f on f.id=c.idContratto and f.id_tipo_protocollo = i.id  " +
                                    "left join tipo_protocollo j on  j.nome = 'Offerta'  " +
                                    "left join dati_protocollo h on h.codice_lavoro = a.codice_lavoro and h.id_tipo_protocollo = j.id and h.destinatario = 'Cliente'  " +
                                    "left join tipo_gruppo k on k.nome = 'Collaboratori' " +
                                    "left join anag_clienti_fornitori forn on d.idFornitori = forn.id " +
                                    "where a.codice_lavoro is not null and a.id_stato >= 3 and d.fp_lordo <>0 and d.idTipoGruppo = k.id " + filtriLavorazione + filtriCosti +
                                    " group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, f.descrizione, e.pathDocumento, h.pathDocumento, h.pregresso, k.nome " +
            #endregion

            #region DIARIA
                                    "UNION " +
                                    "select distinct 3 progressivo, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, min(a.data_inizio_lavorazione) data, a.lavorazione, a.produzione, k.nome gruppo, '' fornitore, sum(d.prezzo) listino, sum(d.fp_lordo)  costo, f.descrizione contratto, e.pathDocumento 'docFattura', h.pathDocumento 'docOfferta', h.pregresso  " +
                                    "from tab_dati_agenda a  " +
                                    "left join anag_clienti_fornitori b on b.id = a.id_cliente  " +
                                    "left join dati_lavorazione c on c.idDatiAgenda = a.id  " +
                                    "left join dati_articoli_lavorazione d on d.idDatiLavorazione = c.id  " +
                                    "left join tipo_protocollo g on  g.nome = 'Fattura'  " +
                                    "left join dati_protocollo e on e.codice_lavoro = a.codice_lavoro and e.attivo = 1 and e.id_tipo_protocollo = g.id and destinatario = 'Fornitore' and  e.id_cliente=d.idFornitori " +
                                    "left join tipo_protocollo i on  i.nome = 'Contratto'  " +
                                    "left join dati_protocollo f on f.id=c.idContratto and f.id_tipo_protocollo = i.id  " +
                                    "left join tipo_protocollo j on  j.nome = 'Offerta'  " +
                                    "left join dati_protocollo h on h.codice_lavoro = a.codice_lavoro and h.id_tipo_protocollo = j.id and h.destinatario = 'Cliente'  " +
                                    "left join tipo_gruppo k on k.nome = 'Diaria' " +
                                    "left join anag_clienti_fornitori forn on d.idFornitori = forn.id " +
                                    "where a.codice_lavoro is not null and a.id_stato >= 3 and  d.idTipoGruppo=k.id " + filtriLavorazione + filtriCosti +
                                    " group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, f.descrizione, e.pathDocumento, h.pathDocumento, h.pregresso, k.nome " +
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
                                    "select distinct 6 progressivo, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, min(a.data_inizio_lavorazione) data, a.lavorazione, a.produzione, d.descrizione gruppo, forn.ragioneSociale fornitore, d.prezzo listino, d.fp_lordo  costo, f.descrizione contratto, e.pathDocumento 'docFattura', h.pathDocumento 'docOfferta', h.pregresso  " +
                                    "from tab_dati_agenda a  " +
                                    "left join anag_clienti_fornitori b on b.id = a.id_cliente  " +
                                    "left join dati_lavorazione c on c.idDatiAgenda = a.id  " +
                                    "left join dati_articoli_lavorazione d on d.idDatiLavorazione = c.id  " +
                                    "left join tipo_protocollo g on  g.nome = 'Fattura'  " +
                                    "left join dati_protocollo e on e.codice_lavoro = a.codice_lavoro and e.attivo = 1 and e.id_tipo_protocollo = g.id and destinatario = 'Fornitore' and  e.id_cliente=d.idFornitori " +
                                    "left join tipo_protocollo i on  i.nome = 'Contratto'  " +
                                    "left join dati_protocollo f on f.id=c.idContratto and f.id_tipo_protocollo = i.id  " +
                                    "left join tipo_protocollo j on  j.nome = 'Offerta'  " +
                                    "left join dati_protocollo h on h.codice_lavoro = a.codice_lavoro and h.id_tipo_protocollo = j.id and h.destinatario = 'Cliente'  " +
                                    "left join anag_clienti_fornitori forn on d.idFornitori = forn.id " +
                                    "left join tipo_gruppo l on l.nome not in ('Collaboratori', 'Diaria') " +
                                    "where a.codice_lavoro is not null and a.id_stato >= 3 and d.fp_lordo <>0 and d.idTipoGruppo=l.id " + filtriLavorazione + filtriCosti +
                                    " group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, f.descrizione, e.pathDocumento, h.pathDocumento, h.pregresso, d.descrizione, forn.ragioneSociale, d.prezzo, d.fp_lordo " +
            #endregion
                                    " order by cliente, a.codice_lavoro, progressivo";

            return query;
        }

        private string CreaQueryListino(string elencoLavorazioni)
        {
            string query = "select distinct 1 progressivo, a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, min(a.data_inizio_lavorazione) data, a.lavorazione, a.produzione, '' gruppo, '' fornitore, sum(d.prezzo) listino, sum(d.fp_lordo)  costo, f.descrizione contratto, e.pathDocumento 'docFattura', h.pathDocumento 'docOfferta', h.pregresso " +
                                    "from tab_dati_agenda a  " +
                                    "left join anag_clienti_fornitori b on b.id = a.id_cliente  " +
                                    "left join dati_lavorazione c on c.idDatiAgenda = a.id  " +
                                    "left join dati_articoli_lavorazione d on d.idDatiLavorazione = c.id  " +
                                    "left join tipo_protocollo g on  g.nome = 'Fattura'  " +
                                    "left join dati_protocollo e on e.codice_lavoro = a.codice_lavoro and e.attivo = 1 and e.id_tipo_protocollo = g.id and destinatario = 'Cliente'  " +
                                    "left join tipo_protocollo i on  i.nome = 'Contratto'  " +
                                    "left join dati_protocollo f on f.id=c.idContratto and f.id_tipo_protocollo = i.id  " +
                                    "left join tipo_protocollo j on  j.nome = 'Offerta'  " +
                                    "left join dati_protocollo h on h.codice_lavoro = a.codice_lavoro and h.id_tipo_protocollo = j.id and h.destinatario = 'Cliente'  " +
                                    "where a.codice_lavoro is not null and a.id_stato >= 3  and d.prezzo <> 0 AND a.codice_lavoro in (" + elencoLavorazioni + ") " +
                                    " group by a.id_cliente, b.ragioneSociale, a.produzione,a.codice_lavoro, a.lavorazione, c.ordine, e.protocollo_riferimento, f.descrizione, e.pathDocumento, h.pathDocumento, h.pregresso ";
            

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

                                            statisticheCosti.IdCliente = riga.Field<int>("id_cliente");
                                            statisticheCosti.Cliente = riga.Field<string>("cliente");
                                            statisticheCosti.NumeroFattura = riga.Field<string>("numeroFattura");
                                            statisticheCosti.Ordine = riga.Field<string>("ordine");
                                            statisticheCosti.CodiceLavoro = riga.Field<string>("codice_lavoro");
                                            statisticheCosti.Data = riga.Field<DateTime?>("data");
                                            statisticheCosti.Lavorazione = riga.Field<string>("lavorazione");
                                            statisticheCosti.Produzione = riga.Field<string>("produzione");
                                            statisticheCosti.Contratto = riga.Field<string>("contratto");
                                            statisticheCosti.Listino = riga.Field<decimal?>("listino");
                                            statisticheCosti.Costo = riga.Field<decimal?>("costo");
                                            statisticheCosti.DocumentoAllegato = riga.Field<string>("docFattura") != null ? riga.Field<string>("docFattura") : "";// riga.Field<string>("docOfferta");
                                            statisticheCosti.Pregresso = riga.Field<bool>("pregresso");
                                            statisticheCosti.Gruppo = riga.Field<string>("gruppo");
                                            statisticheCosti.Fornitore = riga.Field<string>("fornitore");
                                            statisticheCosti.Progressivo = riga.Field<int>("progressivo");

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
                                        StatisticheCosti statisticheCosti = new StatisticheCosti
                                        {
                                            IdCliente = riga.Field<int>("id_cliente"),
                                            Cliente = riga.Field<string>("cliente"),
                                            NumeroFattura = riga.Field<string>("numeroFattura"),
                                            Ordine = riga.Field<string>("ordine"),
                                            CodiceLavoro = riga.Field<string>("codice_lavoro"),
                                            Data = riga.Field<DateTime?>("data"),
                                            Lavorazione = riga.Field<string>("lavorazione"),
                                            Produzione = riga.Field<string>("produzione"),
                                            Contratto = riga.Field<string>("contratto"),
                                            Listino = riga.Field<decimal?>("listino"),
                                            Costo = riga.Field<decimal?>("costo"),
                                            DocumentoAllegato = riga.Field<string>("docFattura") != null ? riga.Field<string>("docFattura") : "",// riga.Field<string>("docOfferta"),
                                            Pregresso = riga.Field<bool>("pregresso"),
                                            Gruppo = riga.Field<string>("gruppo"),
                                            Fornitore = riga.Field<string>("fornitore"),
                                            Progressivo = riga.Field<int>("progressivo")
                                        };

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
                                        StatisticheCosti statisticheCosti = new StatisticheCosti
                                        {
                                            IdCliente = riga.Field<int>("id_cliente"),
                                            Cliente = riga.Field<string>("cliente"),
                                            NumeroFattura = riga.Field<string>("numeroFattura"),
                                            Ordine = riga.Field<string>("ordine"),
                                            CodiceLavoro = riga.Field<string>("codice_lavoro"),
                                            Data = riga.Field<DateTime?>("data"),
                                            Lavorazione = riga.Field<string>("lavorazione"),
                                            Produzione = riga.Field<string>("produzione"),
                                            Contratto = riga.Field<string>("contratto"),
                                            Listino = riga.Field<decimal?>("listino"),
                                            Costo = riga.Field<decimal?>("costo"),
                                            DocumentoAllegato = riga.Field<string>("docFattura") != null ? riga.Field<string>("docFattura") : riga.Field<string>("docOfferta"),
                                            Pregresso = riga.Field<bool>("pregresso"),
                                            Gruppo = riga.Field<string>("gruppo"),
                                            Fornitore = riga.Field<string>("fornitore"),
                                            Progressivo = riga.Field<int>("progressivo")
                                        };

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
    }
}