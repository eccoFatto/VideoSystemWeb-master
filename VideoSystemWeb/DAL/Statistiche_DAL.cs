using System;
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

            string filtri = string.Empty;
            filtri += string.IsNullOrWhiteSpace(filtroCliente) ? "" : " AND b.ragioneSociale = '" + filtroCliente + "' ";
            filtri += string.IsNullOrWhiteSpace(filtroProduzione) ? "" : " AND a.produzione = '" + filtroProduzione + "' ";
            filtri += string.IsNullOrWhiteSpace(filtroLavorazione) ? "" : " AND a.lavorazione = '" + filtroLavorazione + "' ";
            filtri += string.IsNullOrWhiteSpace(filtroContratto) ? "" : " AND f.descrizione = '" + filtroContratto + "' ";
            if (fatturato != null)
            {
                filtri += (bool)fatturato ? "and e.protocollo_riferimento is not null " : "and e.protocollo_riferimento is null ";
            }
            filtri += string.IsNullOrWhiteSpace(dataInizio) ? "" : " AND a.data_inizio_lavorazione >= '"+ dataInizio.Substring(6) + "-" + dataInizio.Substring(3,2) + "-" + dataInizio.Substring(0, 2) + "T00:00:00.000'";
            // il filtro dataFine viene eseguito su dataInizioLavorazione, e non su dataFineLavorazione
            filtri += string.IsNullOrWhiteSpace(dataFine) ? "" : " AND a.data_inizio_lavorazione <= '" + dataFine.Substring(6) + "-" + dataFine.Substring(3, 2) + "-" + dataFine.Substring(0, 2) + "T00:00:00.000'";
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "select distinct a.id_cliente, b.ragioneSociale cliente, e.protocollo_riferimento 'numeroFattura', c.ordine, a.codice_lavoro, min(a.data_inizio_lavorazione) data, a.lavorazione, a.produzione, SUM(d.prezzo) listino, sum(d.fp_lordo)  costo, f.descrizione contratto, e.pathDocumento 'docFattura', h.pathDocumento 'docOfferta', h.pregresso " +
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
                                        StatisticheRicavi statisticheRicavi = new StatisticheRicavi
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
                                            Pregresso = riga.Field<bool>("pregresso")
                                        };

                                        listaStatisticheRicavi.Add(statisticheRicavi);
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

            return listaStatisticheRicavi;
        }
    }
}