using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using VideoSystemWeb.BLL;
using VideoSystemWeb.Entity;

namespace VideoSystemWeb.DAL
{
    public class Agenda_DAL: Base_DAL
    {
        private const string TABELLA_DATI_AGENDA = "tab_dati_agenda";

        //singleton
        private static volatile Agenda_DAL instance;
        private static object objForLock = new Object();

        private Agenda_DAL() { }

        public static Agenda_DAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Agenda_DAL();
                    }
                }
                return instance;
            }
        }

        public List<ColonneAgenda> CaricaColonne(ref Esito esito)
        {
            //return CaricaTipologica(EnumTipologiche.TIPO_COLONNE_AGENDA, true, ref esito);
            return CaricaColonneAgenda(true, ref esito);
        }

        public List<DatiAgenda> CaricaDatiAgenda(ref Esito esito)
        {
            List<DatiAgenda> listaDatiAgenda = new List<DatiAgenda>();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM "+ TABELLA_DATI_AGENDA))
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
                                        DatiAgenda datoAgenda = new DatiAgenda();
                                        datoAgenda.id = riga.Field<int>("id");
                                        datoAgenda.id_colonne_agenda = riga.Field<int>("id_colonne_agenda"); 
                                        datoAgenda.id_stato = riga.Field<int>("id_stato");
                                        datoAgenda.data_inizio_lavorazione = riga.Field<DateTime>("data_inizio_lavorazione"); 
                                        datoAgenda.data_fine_lavorazione = riga.Field<DateTime>("data_fine_lavorazione"); 
                                        datoAgenda.durata_lavorazione = riga.Field<int>("durata_lavorazione"); 
                                        datoAgenda.id_tipologia = riga.Field<int?>("id_tipologia");
                                        datoAgenda.id_cliente = riga.Field<int>("id_cliente"); 
                                        datoAgenda.durata_viaggio_andata = riga.Field<int>("durata_viaggio_andata"); 
                                        datoAgenda.durata_viaggio_ritorno = riga.Field<int>("durata_viaggio_ritorno"); 
                                        datoAgenda.data_inizio_impegno = riga.Field<DateTime>("data_inizio_impegno");
                                        datoAgenda.data_fine_impegno = riga.Field<DateTime>("data_fine_impegno"); 
                                        datoAgenda.impegnoOrario = riga.Field <bool>("impegnoOrario");
                                        datoAgenda.impegnoOrario_da = riga.Field<string>("impegnoOrario_da");
                                        datoAgenda.impegnoOrario_a = riga.Field<string>("impegnoOrario_a"); 
                                        datoAgenda.produzione = riga.Field <string>("produzione");
                                        datoAgenda.lavorazione = riga.Field<string>("lavorazione");
                                        datoAgenda.indirizzo = riga.Field<string>("indirizzo");
                                        datoAgenda.luogo = riga.Field<string>("luogo");
                                        datoAgenda.codice_lavoro = riga.Field<string>("codice_lavoro");
                                        datoAgenda.nota = riga.Field<string>("nota");

                                        listaDatiAgenda.Add(datoAgenda);
                                    }
                                }
                                else
                                {
                                    esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                    esito.descrizione = "Nessun dato trovato nella tabella tab_dati_agenda ";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.descrizione = "Agenda_DAL.cs - CaricaDatiAgenda " + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return listaDatiAgenda;
        }

        public List<DatiAgenda> CaricaDatiAgenda(DateTime dataInizio, DateTime dataFine, ref Esito esito)
        {
            string dataDa = dataInizio.ToString("yyyy-MM-ddT00:00:00.000");
            string dataA = dataFine.ToString("yyyy-MM-ddT23:59:59.999");

            List<DatiAgenda> listaDatiAgenda = new List<DatiAgenda>();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM " + TABELLA_DATI_AGENDA + " WHERE data_inizio_impegno between '" + dataDa + "' and '" + dataA + "' OR data_fine_impegno between '" + dataDa + "' and '" + dataA + "'"))
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
                                        DatiAgenda datoAgenda = new DatiAgenda();
                                        datoAgenda.id = riga.Field<int>("id");
                                        datoAgenda.id_colonne_agenda = riga.Field<int>("id_colonne_agenda");
                                        datoAgenda.id_stato = riga.Field<int>("id_stato");
                                        datoAgenda.data_inizio_lavorazione = riga.Field<DateTime>("data_inizio_lavorazione");
                                        datoAgenda.data_fine_lavorazione = riga.Field<DateTime>("data_fine_lavorazione");
                                        datoAgenda.durata_lavorazione = riga.Field<int>("durata_lavorazione");
                                        datoAgenda.id_tipologia = riga.Field<int?>("id_tipologia");
                                        datoAgenda.id_cliente = riga.Field<int>("id_cliente");
                                        datoAgenda.durata_viaggio_andata = riga.Field<int>("durata_viaggio_andata");
                                        datoAgenda.durata_viaggio_ritorno = riga.Field<int>("durata_viaggio_ritorno");
                                        datoAgenda.data_inizio_impegno = riga.Field<DateTime>("data_inizio_impegno");
                                        datoAgenda.data_fine_impegno = riga.Field<DateTime>("data_fine_impegno");
                                        datoAgenda.impegnoOrario = riga.Field<bool>("impegnoOrario");
                                        datoAgenda.impegnoOrario_da = riga.Field<string>("impegnoOrario_da");
                                        datoAgenda.impegnoOrario_a = riga.Field<string>("impegnoOrario_a");
                                        datoAgenda.produzione = riga.Field<string>("produzione");
                                        datoAgenda.lavorazione = riga.Field<string>("lavorazione");
                                        datoAgenda.indirizzo = riga.Field<string>("indirizzo");
                                        datoAgenda.luogo = riga.Field<string>("luogo");
                                        datoAgenda.codice_lavoro = riga.Field<string>("codice_lavoro");
                                        datoAgenda.nota = riga.Field<string>("nota");

                                        listaDatiAgenda.Add(datoAgenda);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.descrizione = "Agenda_DAL.cs - CaricaDatiAgenda " + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return listaDatiAgenda;
        }

        // Controllo la disponibilità dei tender nel periodo selezionato, ESCLUDENDO quelli dell'evento corrente
        public List<int> getTenderImpiegatiInPeriodo(DateTime dataInizio, DateTime dataFine, int idEventoCorrente, ref Esito esito)
        {
            List<int> listaIdTender = new List<int>();

            string dataDa = dataInizio.ToString("yyyy-MM-ddT00:00:00.000");
            string dataA = dataFine.ToString("yyyy-MM-ddT23:59:59.999");

            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT DISTINCT c.id " +
                                    " FROM tab_dati_agenda a " +
                                    " join dati_tender b on a.id = b.idDatiAgenda " +
                                    " join tipo_tender c on b.idTender = c.id " +
                                    " WHERE b.idDatiAgenda <> " + idEventoCorrente + " AND " +
                                    " ((a.data_inizio_impegno <= '" + dataDa + "' AND a.data_fine_impegno >= '" + dataDa + "') OR " +
                                    " (a.data_inizio_impegno < '" + dataA + "' AND a.data_fine_impegno >= '" + dataA + "') OR " +
                                    " (a.data_inizio_impegno >= '" + dataDa + "' AND a.data_fine_impegno < '" + dataA + "')) ";


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
                                        listaIdTender.Add(riga.Field<int>(0));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.descrizione = ex.Message + Environment.NewLine + ex.StackTrace;
            }
            return listaIdTender;
        }

        //public Esito CreaEvento(DatiAgenda evento, List<string> listaIdTender, NoteOfferta noteOfferta)
        //{
        //    Esito esito = new Esito();
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(sqlConstr))
        //        {
        //            using (SqlCommand StoreProc = new SqlCommand("InsertEvento"))
        //            {
        //                using (SqlDataAdapter sda = new SqlDataAdapter())
        //                {
        //                    SqlTransaction transaction;
        //                    StoreProc.Connection = con;
        //                    StoreProc.Connection.Open();
        //                    // Start a local transaction.
        //                    transaction = con.BeginTransaction("AgendaTransaction");

        //                    try
        //                    {
        //                        CostruisciSP_InsertEvento(evento, StoreProc, sda);
        //                        StoreProc.Transaction = transaction;

        //                        int iReturn = StoreProc.ExecuteNonQuery();

        //                        // RECUPERO L'ID DELL'EVENTO AGENDA INSERITO
        //                        int iDatiAgendaReturn = Convert.ToInt32(StoreProc.Parameters["@id"].Value);


        //                        // SE E' ANDATO TUTTO BENE FACCIO INSERT DEI TENDER DELLA LISTA IN INPUT
        //                        if (listaIdTender != null)
        //                        {
        //                            foreach (string idTender in listaIdTender)
        //                            {
        //                                CostruisciSP_InsertDatiTender(StoreProc, sda, iDatiAgendaReturn, idTender);
        //                                StoreProc.ExecuteNonQuery();

        //                                iReturn = Convert.ToInt32(StoreProc.Parameters["@id"].Value);
        //                            }
        //                        }

        //                        // SE E' ANDATO TUTTO BENE FACCIO INSERT DELLA NOTA OFFERTA
        //                        if (noteOfferta != null)
        //                        {
                                    
        //                                CostruisciSP_InsertNoteOfferta(StoreProc, sda, iDatiAgendaReturn, noteOfferta);
        //                                StoreProc.ExecuteNonQuery();

        //                                iReturn = Convert.ToInt32(StoreProc.Parameters["@id"].Value);
        //                        }

        //                        // Attempt to commit the transaction.
        //                        transaction.Commit();
        //                        evento.id = iDatiAgendaReturn; // setto l'id generato x salvataggio dovuto a riepilogo (non viene chiuso il popup e un successivo salvataggio genera errore validazione)
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        esito.codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
        //                        esito.descrizione = "Agenda_DAL.cs - CreaEvento " + Environment.NewLine + ex.Message;

        //                        try
        //                        {
        //                            transaction.Rollback();
        //                        }
        //                        catch (Exception ex2)
        //                        {
        //                            esito.descrizione += Environment.NewLine + "ERRORE ROLLBACK: " + ex2.Message;
        //                            log.Error(ex2.Message + Environment.NewLine + ex2.StackTrace);
        //                        }

        //                        log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        esito.codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
        //        esito.descrizione = "Agenda_DAL.cs - creaEvento " + Environment.NewLine + ex.Message;

        //        log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
        //    }

        //    return esito;
        //}

        public Esito CreaEvento(DatiAgenda evento, List<string> listaIdTender, NoteOfferta noteOfferta)
        {
            Esito esito = new Esito();
            using (SqlConnection con = new SqlConnection(sqlConstr))
            {

                using (SqlCommand StoreProc = new SqlCommand("InsertEvento"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        SqlTransaction transaction;
                        StoreProc.Connection = con;
                        StoreProc.Connection.Open();
                        // Start a local transaction.
                        transaction = con.BeginTransaction("AgendaTransaction");

                        try
                        {
                            #region APPUNTAMENTO
                            CostruisciSP_InsertEvento(evento, StoreProc, sda);

                            StoreProc.Transaction = transaction;
                            StoreProc.ExecuteNonQuery();

                            // RECUPERO L'ID DELL'EVENTO AGENDA INSERITO
                            int iDatiAgendaReturn = Convert.ToInt32(StoreProc.Parameters["@id"].Value);

                            // SE E' ANDATO TUTTO BENE FACCIO INSERT DEI TENDER DELLA LISTA IN INPUT
                            if (listaIdTender != null)
                            {
                                foreach (string idTender in listaIdTender)
                                {
                                    CostruisciSP_InsertDatiTender(StoreProc, sda, iDatiAgendaReturn, idTender);
                                    StoreProc.ExecuteNonQuery();

                                    int iReturn = Convert.ToInt32(StoreProc.Parameters["@id"].Value);
                                }
                            }


                            #endregion

                            #region OFFERTA
                            // SE E' ANDATO TUTTO BENE FACCIO INSERT DEGLI ARTICOLI EVENTO DELLA LISTA IN INPUT
                            if (evento.ListaDatiArticoli != null)
                            {
                                foreach (DatiArticoli datoArticolo in evento.ListaDatiArticoli)
                                {
                                    CostruisciSP_InsertDatiArticoli(StoreProc, sda, iDatiAgendaReturn, datoArticolo);
                                    StoreProc.ExecuteNonQuery();

                                    int iReturn = Convert.ToInt32(StoreProc.Parameters["@id"].Value);
                                }
                            }

                            //GESTIONE PROTOCOLLO
                            if (evento.id_stato == Stato.Instance.STATO_OFFERTA)
                            {
                                string protocollo = Protocolli_BLL.Instance.getNumeroProtocollo();
                                // SE E' ANDATO TUTTO BENE FACCIO INSERT SU TABELLA DATI_PROTOCOLLO
                                if (!string.IsNullOrEmpty(protocollo))
                                {
                                    Protocolli protocolloOfferta = new Protocolli();
                                    protocolloOfferta.Codice_lavoro = evento.codice_lavoro;
                                    protocolloOfferta.Numero_protocollo = protocollo;
                                    protocolloOfferta.Cliente = Anag_Clienti_Fornitori_BLL.Instance.getAziendaById(evento.id_cliente, ref esito).RagioneSociale;
                                    protocolloOfferta.Id_tipo_protocollo = UtilityTipologiche.getElementByNome(UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_PROTOCOLLO), "offerta", ref esito).id;
                                    protocolloOfferta.Protocollo_riferimento = "";
                                    string nomeFile = "Offerta_" + evento.codice_lavoro + ".pdf";
                                    protocolloOfferta.PathDocumento = nomeFile;
                                    protocolloOfferta.Descrizione = evento.lavorazione.Trim();
                                    protocolloOfferta.Produzione = evento.produzione.Trim();
                                    protocolloOfferta.Data_inizio_lavorazione = evento.data_inizio_lavorazione;
                                    protocolloOfferta.Attivo = true;

                                    CostruisciSP_InsertProtocollo(StoreProc, sda, iDatiAgendaReturn, protocolloOfferta);
                                    StoreProc.ExecuteNonQuery();

                                    int iReturn = Convert.ToInt32(StoreProc.Parameters["@id"].Value);
                                }

                                // SE E' ANDATO TUTTO BENE FACCIO INSERT DELLA NOTA OFFERTA
                                if (noteOfferta != null)
                                {

                                    CostruisciSP_InsertNoteOfferta(StoreProc, sda, iDatiAgendaReturn, noteOfferta);
                                    StoreProc.ExecuteNonQuery();

                                    int iReturn = Convert.ToInt32(StoreProc.Parameters["@id"].Value);
                                }
                            }

                            
                            #endregion

                            #region LAVORAZIONE
                            // SE E' ANDATO TUTTO BENE FACCIO INSERT DATI LAVORAZIONE
                            if (evento.LavorazioneCorrente != null)
                            {
                                CostruisciSP_InsertDatiLavorazione(StoreProc, sda, iDatiAgendaReturn, evento.LavorazioneCorrente);
                                StoreProc.ExecuteNonQuery();

                                int iDatiLavorazioneReturn = Convert.ToInt32(StoreProc.Parameters["@id"].Value);

                                foreach (DatiArticoliLavorazione datoArticoloLavorazione in evento.LavorazioneCorrente.ListaArticoliLavorazione)
                                {
                                    CostruisciSP_InsertDatiArticoliLavorazione(StoreProc, sda, iDatiLavorazioneReturn, datoArticoloLavorazione);
                                    StoreProc.ExecuteNonQuery();
                                }

                                foreach (DatiPianoEsternoLavorazione datoPianoEsternoLavorazione in evento.LavorazioneCorrente.ListaDatiPianoEsternoLavorazione)
                                {
                                    CostruisciSP_InsertDatiPianoEsternoLavorazione(StoreProc, sda, iDatiLavorazioneReturn, datoPianoEsternoLavorazione);
                                    StoreProc.ExecuteNonQuery();
                                }
                            }
                            #endregion

                            // Attempt to commit the transaction.
                            transaction.Commit();
                            evento.id = iDatiAgendaReturn; // setto l'id generato x salvataggio dovuto a riepilogo (non viene chiuso il popup e un successivo salvataggio genera errore validazione)
                        }
                        catch (Exception ex)
                        {
                            esito.codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                            esito.descrizione = "Agenda_DAL.cs - creaEvento " + Environment.NewLine + ex.Message;

                            try
                            {
                                transaction.Rollback();
                            }
                            catch (Exception ex2)
                            {
                                esito.descrizione += Environment.NewLine + "ERRORE ROLLBACK: " + ex2.Message;
                                log.Error(ex2.Message + Environment.NewLine + ex2.StackTrace);
                            }

                            log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
                        }

                    }
                }
            }
            return esito;
        }

        //public Esito AggiornaEvento(DatiAgenda evento, List<string> listaIdTender)
        //{
        //    Esito esito = new Esito();
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(sqlConstr))
        //        {
        //            using (SqlCommand StoreProc = new SqlCommand("UpdateEvento"))
        //            {
        //                using (SqlDataAdapter sda = new SqlDataAdapter())
        //                {
        //                    SqlTransaction transaction;
        //                    StoreProc.Connection = con;
        //                    sda.SelectCommand = StoreProc;
        //                    StoreProc.CommandType = CommandType.StoredProcedure;
        //                    // Start a local transaction.
        //                    StoreProc.Connection.Open();
        //                    transaction = con.BeginTransaction("UpdateAgendaTransaction");

        //                    Anag_Utenti utente = (Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE];

        //                    try
        //                    {
        //                        SqlParameter id = new SqlParameter("@id", SqlDbType.Int);
        //                        id.Direction = ParameterDirection.Input;
        //                        id.Value = evento.id;
        //                        StoreProc.Parameters.Add(id);

        //                        // PARAMETRI PER LOG UTENTE
        //                        SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
        //                        idUtente.Direction = ParameterDirection.Input;
        //                        StoreProc.Parameters.Add(idUtente);

        //                        SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
        //                        nomeUtente.Direction = ParameterDirection.Input;
        //                        StoreProc.Parameters.Add(nomeUtente);
        //                        // FINE PARAMETRI PER LOG UTENTE

        //                        SqlParameter data_inizio_lavorazione = new SqlParameter("@data_inizio_lavorazione", SqlDbType.DateTime);
        //                        data_inizio_lavorazione.Direction = ParameterDirection.Input;
        //                        data_inizio_lavorazione.Value = evento.data_inizio_lavorazione;
        //                        StoreProc.Parameters.Add(data_inizio_lavorazione);

        //                        SqlParameter data_fine_lavorazione = new SqlParameter("@data_fine_lavorazione", SqlDbType.DateTime);
        //                        data_fine_lavorazione.Direction = ParameterDirection.Input;
        //                        data_fine_lavorazione.Value = evento.data_fine_lavorazione;
        //                        StoreProc.Parameters.Add(data_fine_lavorazione);

        //                        SqlParameter durata_lavorazione = new SqlParameter("@durata_lavorazione", SqlDbType.Int);
        //                        durata_lavorazione.Direction = ParameterDirection.Input;
        //                        durata_lavorazione.Value = evento.durata_lavorazione;
        //                        StoreProc.Parameters.Add(durata_lavorazione);

        //                        SqlParameter id_colonne_agenda = new SqlParameter("@id_colonne_agenda", SqlDbType.Int);
        //                        id_colonne_agenda.Direction = ParameterDirection.Input;
        //                        id_colonne_agenda.Value = evento.id_colonne_agenda;
        //                        StoreProc.Parameters.Add(id_colonne_agenda);

        //                        SqlParameter id_tipologia = new SqlParameter("@id_tipologia", SqlDbType.Int);
        //                        id_tipologia.Direction = ParameterDirection.Input;
        //                        if (evento.id_tipologia == 0)
        //                        {
        //                            id_tipologia.Value = null;
        //                        }
        //                        else
        //                        {
        //                            id_tipologia.Value = evento.id_tipologia;
        //                        }
        //                        StoreProc.Parameters.Add(id_tipologia);

        //                        SqlParameter id_stato = new SqlParameter("@id_stato", SqlDbType.Int);
        //                        id_stato.Direction = ParameterDirection.Input;
        //                        id_stato.Value = evento.id_stato;
        //                        StoreProc.Parameters.Add(id_stato);

        //                        SqlParameter id_cliente = new SqlParameter("@id_cliente", SqlDbType.Int);
        //                        id_cliente.Direction = ParameterDirection.Input;
        //                        id_cliente.Value = evento.id_cliente;
        //                        StoreProc.Parameters.Add(id_cliente);

        //                        SqlParameter durata_viaggio_andata = new SqlParameter("@durata_viaggio_andata", SqlDbType.Int);
        //                        durata_viaggio_andata.Direction = ParameterDirection.Input;
        //                        durata_viaggio_andata.Value = evento.durata_viaggio_andata;
        //                        StoreProc.Parameters.Add(durata_viaggio_andata);

        //                        SqlParameter durata_viaggio_ritorno = new SqlParameter("@durata_viaggio_ritorno", SqlDbType.Int);
        //                        durata_viaggio_ritorno.Direction = ParameterDirection.Input;
        //                        durata_viaggio_ritorno.Value = evento.durata_viaggio_ritorno;
        //                        StoreProc.Parameters.Add(durata_viaggio_ritorno);

        //                        SqlParameter data_inizio_impegno = new SqlParameter("@data_inizio_impegno", SqlDbType.DateTime);
        //                        data_inizio_impegno.Direction = ParameterDirection.Input;
        //                        data_inizio_impegno.Value = evento.data_inizio_impegno;
        //                        StoreProc.Parameters.Add(data_inizio_impegno);

        //                        SqlParameter data_fine_impegno = new SqlParameter("@data_fine_impegno", SqlDbType.DateTime);
        //                        data_fine_impegno.Direction = ParameterDirection.Input;
        //                        data_fine_impegno.Value = evento.data_fine_impegno;
        //                        StoreProc.Parameters.Add(data_fine_impegno);

        //                        SqlParameter impegnoOrario = new SqlParameter("@impegnoOrario", SqlDbType.Bit);
        //                        impegnoOrario.Direction = ParameterDirection.Input;
        //                        impegnoOrario.Value = evento.impegnoOrario;
        //                        StoreProc.Parameters.Add(impegnoOrario);

        //                        SqlParameter impegnoOrario_da = new SqlParameter("@impegnoOrario_da", SqlDbType.VarChar);
        //                        impegnoOrario_da.Direction = ParameterDirection.Input;
        //                        impegnoOrario_da.Value = evento.impegnoOrario_da;
        //                        StoreProc.Parameters.Add(impegnoOrario_da);

        //                        SqlParameter impegnoOrario_a = new SqlParameter("@impegnoOrario_a", SqlDbType.VarChar);
        //                        impegnoOrario_a.Direction = ParameterDirection.Input;
        //                        impegnoOrario_a.Value = evento.impegnoOrario_a;
        //                        StoreProc.Parameters.Add(impegnoOrario_a);

        //                        SqlParameter produzione = new SqlParameter("@produzione", SqlDbType.VarChar);
        //                        produzione.Direction = ParameterDirection.Input;
        //                        produzione.Value = evento.produzione;
        //                        StoreProc.Parameters.Add(produzione);

        //                        SqlParameter lavorazione = new SqlParameter("@lavorazione", SqlDbType.VarChar);
        //                        lavorazione.Direction = ParameterDirection.Input;
        //                        lavorazione.Value = evento.lavorazione;
        //                        StoreProc.Parameters.Add(lavorazione);

        //                        SqlParameter indirizzo = new SqlParameter("@indirizzo", SqlDbType.VarChar);
        //                        indirizzo.Direction = ParameterDirection.Input;
        //                        indirizzo.Value = evento.indirizzo;
        //                        StoreProc.Parameters.Add(indirizzo);

        //                        SqlParameter luogo = new SqlParameter("@luogo", SqlDbType.VarChar);
        //                        luogo.Direction = ParameterDirection.Input;
        //                        luogo.Value = evento.luogo;
        //                        StoreProc.Parameters.Add(luogo);

        //                        SqlParameter codice_lavoro = new SqlParameter("@codice_lavoro", SqlDbType.VarChar);
        //                        codice_lavoro.Direction = ParameterDirection.Input;
        //                        codice_lavoro.Value = evento.codice_lavoro;
        //                        StoreProc.Parameters.Add(codice_lavoro);

        //                        SqlParameter nota = new SqlParameter("@nota", SqlDbType.VarChar);
        //                        nota.Direction = ParameterDirection.Input;
        //                        nota.Value = evento.nota;
        //                        StoreProc.Parameters.Add(nota);

        //                        //StoreProc.Connection.Open();
        //                        StoreProc.Transaction = transaction;
        //                        int iReturn = StoreProc.ExecuteNonQuery();

        //                        // ELIMINO GLI EVENTUALI TENDER ASSOCIATI ALL'EVENTO PER SOSTITUIRLI COI NUOVI
        //                        CostruisciSP_DeleteDatiTender(StoreProc, sda, evento.id);
        //                        //StoreProc.Transaction = transaction;

        //                        StoreProc.ExecuteNonQuery();


        //                        // SE E' ANDATO TUTTO BENE FACCIO  INSERT DEI TENDER
        //                        if (listaIdTender != null)
        //                        {
        //                            foreach (string idTender in listaIdTender)
        //                            {
        //                                CostruisciSP_InsertDatiTender(StoreProc, sda, evento.id, idTender);
        //                                StoreProc.ExecuteNonQuery();
        //                            }
        //                        }

        //                        // Attempt to commit the transaction.
        //                        transaction.Commit();

        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        esito.codice = Esito.ESITO_KO_ERRORE_UPDATE_TABELLA;
        //                        esito.descrizione = "Agenda_DAL.cs - AggiornaEvento " + Environment.NewLine + ex.Message;

        //                        log.Error(ex.Message + Environment.NewLine + ex.StackTrace);

        //                        try
        //                        {
        //                            transaction.Rollback();
        //                        }
        //                        catch (Exception ex2)
        //                        {
        //                            esito.descrizione += Environment.NewLine + "ERRORE ROLLBACK: " + ex2.Message;
        //                            log.Error(ex2.Message + Environment.NewLine + ex2.StackTrace);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        esito.codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
        //        esito.descrizione = "Agenda_DAL.cs - aggiornaEvento " + Environment.NewLine + ex.Message;

        //        log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
        //    }

        //    return esito;
        //}

        public Esito AggiornaEvento(DatiAgenda evento, List<string> listaIdTender)
        {
            Esito esito = new Esito();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("UpdateEvento"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            SqlTransaction transaction;
                            StoreProc.Connection = con;
                            StoreProc.Connection.Open();
                            // Start a local transaction.
                            transaction = con.BeginTransaction("UpdateAgendaTransaction");

                            int iDatiLavorazioneReturn = 0;
                            try
                            {
                                #region APPUNTAMENTO
                                CostruisciSP_UpdateEvento(evento, StoreProc, sda);
                                StoreProc.Transaction = transaction;

                                int iDatiAgendaReturn = StoreProc.ExecuteNonQuery();

                                // ELIMINO GLI EVENTUALI TENDER ASSOCIATI ALL'EVENTO PER SOSTITUIRLI COI NUOVI
                                CostruisciSP_DeleteDatiTender(StoreProc, sda, evento.id);
                                StoreProc.ExecuteNonQuery();

                                // SE E' ANDATO TUTTO BENE FACCIO  INSERT DEI TENDER
                                if (listaIdTender != null)
                                {
                                    foreach (string idTender in listaIdTender)
                                    {
                                        CostruisciSP_InsertDatiTender(StoreProc, sda, evento.id, idTender);
                                        StoreProc.ExecuteNonQuery();
                                    }
                                }
                                #endregion

                                #region OFFERTA
                                // ELIMINO GLI EVENTUALI ARTICOLI ASSOCIATI ALL'EVENTO PER SOSTITUIRLI COI NUOVI
                                CostruisciSP_DeleteDatiArticoli(StoreProc, sda, evento.id);
                                StoreProc.ExecuteNonQuery();

                                // SE E' ANDATO TUTTO BENE FACCIO INSERT DEGLI ARTICOLI EVENTO DELLA LISTA IN INPUT
                                if (evento.ListaDatiArticoli != null)
                                {
                                    foreach (DatiArticoli datoArticolo in evento.ListaDatiArticoli)
                                    {
                                        CostruisciSP_InsertDatiArticoli(StoreProc, sda, evento.id, datoArticolo);
                                        StoreProc.ExecuteNonQuery();

                                        int iDatiArticoloReturn = Convert.ToInt32(StoreProc.Parameters["@id"].Value);
                                    }
                                }

                                //GESTIONE PROTOCOLLO
                                int idTipoProtocollo = UtilityTipologiche.getElementByNome(UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_PROTOCOLLO), "offerta", ref esito).id;
                                if (evento.id_stato == Stato.Instance.STATO_OFFERTA && Protocolli_BLL.Instance.getProtocolliByCodLavIdTipoProtocollo(evento.codice_lavoro, idTipoProtocollo, ref esito, true).Count == 0)
                                {
                                    string protocollo = Protocolli_BLL.Instance.getNumeroProtocollo();
                                    // SE E' ANDATO TUTTO BENE FACCIO INSERT SU TABELLA DATI_PROTOCOLLO
                                    if (!string.IsNullOrEmpty(protocollo))
                                    {
                                        Protocolli protocolloOfferta = new Protocolli();
                                        protocolloOfferta.Codice_lavoro = evento.codice_lavoro;
                                        protocolloOfferta.Numero_protocollo = protocollo;
                                        protocolloOfferta.Cliente = Anag_Clienti_Fornitori_BLL.Instance.getAziendaById(evento.id_cliente, ref esito).RagioneSociale;
                                        protocolloOfferta.Id_tipo_protocollo = idTipoProtocollo;
                                        protocolloOfferta.Protocollo_riferimento = "";
                                        string nomeFile = "Offerta_" + evento.codice_lavoro + ".pdf";
                                        protocolloOfferta.PathDocumento = nomeFile;
                                        protocolloOfferta.Descrizione = evento.lavorazione.Trim();
                                        protocolloOfferta.Produzione = evento.produzione.Trim();
                                        protocolloOfferta.Data_inizio_lavorazione = evento.data_inizio_lavorazione;

                                        protocolloOfferta.Attivo = true;

                                        CostruisciSP_InsertProtocollo(StoreProc, sda, iDatiAgendaReturn, protocolloOfferta);
                                        StoreProc.ExecuteNonQuery();

                                        int iReturn = Convert.ToInt32(StoreProc.Parameters["@id"].Value);
                                    }
                                }

                                #endregion

                                #region LAVORAZIONE
                                //GESTIONE DATI LAVORAZIONE
                                if (evento.LavorazioneCorrente != null)
                                {


                                    // ELIMINO GLI EVENTUALI DATI LAVORAZIONE ASSOCIATI ALL'EVENTO PER SOSTITUIRLI COI NUOVI
                                    if (evento.LavorazioneCorrente.Id != 0) // caso in cui la lavorazione sia stata appena creata ma non salvata (passaggio da offerta a lavorazione)
                                    { 
                                        CostruisciSP_DeleteDatiArticoliLavorazione(StoreProc, sda, evento.LavorazioneCorrente.Id);
                                        StoreProc.ExecuteNonQuery();

                                        CostruisciSP_DeleteDatiPianoEsternoLavorazione(StoreProc, sda, evento.LavorazioneCorrente.Id);
                                        StoreProc.ExecuteNonQuery();
                                    }

                                    CostruisciSP_DeleteDatiLavorazione(StoreProc, sda, evento.id);
                                    StoreProc.ExecuteNonQuery();


                                    CostruisciSP_InsertDatiLavorazione(StoreProc, sda, evento.id, evento.LavorazioneCorrente);
                                    StoreProc.ExecuteNonQuery();

                                    iDatiLavorazioneReturn = Convert.ToInt32(StoreProc.Parameters["@id"].Value);
                                    
                                    foreach (DatiArticoliLavorazione datoArticoloLavorazione in evento.LavorazioneCorrente.ListaArticoliLavorazione)
                                    {
                                        CostruisciSP_InsertDatiArticoliLavorazione(StoreProc, sda, iDatiLavorazioneReturn, datoArticoloLavorazione);
                                        StoreProc.ExecuteNonQuery();
                                    }

                                    foreach (DatiPianoEsternoLavorazione datoPianoEsternoLavorazione in evento.LavorazioneCorrente.ListaDatiPianoEsternoLavorazione)
                                    {
                                        CostruisciSP_InsertDatiPianoEsternoLavorazione(StoreProc, sda, iDatiLavorazioneReturn, datoPianoEsternoLavorazione);
                                        StoreProc.ExecuteNonQuery();
                                    }
                                }
                                #endregion

                                // Attempt to commit the transaction.
                                transaction.Commit();
                                if (iDatiLavorazioneReturn != 0)
                                {
                                    // aggiorno id della lavorazione corrente nel caso in cui sia stato modificato in fase di modifica evento
                                    SessionManager.EventoSelezionato.LavorazioneCorrente.Id = iDatiLavorazioneReturn;
                                }
                            }
                            catch (Exception ex)
                            {
                                esito.codice = Esito.ESITO_KO_ERRORE_UPDATE_TABELLA;
                                esito.descrizione = "Agenda_DAL.cs - AggiornaEvento" + Environment.NewLine + ex.Message;

                                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);

                                try
                                {
                                    transaction.Rollback();
                                }
                                catch (Exception ex2)
                                {
                                    esito.descrizione += Environment.NewLine + "ERRORE ROLLBACK: " + ex2.Message;
                                    log.Error(ex2.Message + Environment.NewLine + ex2.StackTrace);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.descrizione = "Agenda_DAL.cs - AggiornaEventoConArticoli " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return esito;
        }

        public Esito EliminaEvento(int idEvento)
        {
            Esito esito = new Esito();
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("DeleteEvento"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            SqlParameter id = new SqlParameter("@id", SqlDbType.Int);
                            id.Direction = ParameterDirection.Input;
                            id.Value = idEvento;
                            StoreProc.Parameters.Add(id);

                            // PARAMETRI PER LOG UTENTE
                            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
                            idUtente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idUtente);

                            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
                            nomeUtente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nomeUtente);
                            // FINE PARAMETRI PER LOG UTENTE

                            StoreProc.Connection.Open();

                            int iReturn = StoreProc.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.descrizione = "Agenda_DAL.cs - EliminaEvento " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return esito;
        }

        public List<string> CaricaElencoProduzioni(ref Esito esito)
        {
            List<string> listaProduzioni = new List<string>();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT produzione FROM " + TABELLA_DATI_AGENDA))
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
                                        listaProduzioni.Add(riga.Field<string>("produzione"));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.descrizione = "Agenda_DAL.cs - CaricaElencoProduzioni " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return listaProduzioni;
        }

        public List<string> CaricaElencoLavorazioni(ref Esito esito)
        {
            List<string> listaLavorazioni = new List<string>();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT lavorazione FROM " + TABELLA_DATI_AGENDA))
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
                                        listaLavorazioni.Add(riga.Field<string>("lavorazione"));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.descrizione = "Agenda_DAL.cs - CaricaElencoLavorazioni " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return listaLavorazioni;
        }
       
        private static void CostruisciSP_InsertDatiArticoli(SqlCommand StoreProc, SqlDataAdapter sda, int iDatiAgendaReturn, DatiArticoli datoArticolo)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

            StoreProc.CommandType = CommandType.StoredProcedure;
            StoreProc.CommandText = "InsertDatiArticoli";
            StoreProc.Parameters.Clear();
            sda.SelectCommand = StoreProc;

            StoreProc.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

            // PARAMETRI PER LOG UTENTE
            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
            idUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idUtente);

            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
            nomeUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(nomeUtente);
            // FINE PARAMETRI PER LOG UTENTE

            SqlParameter costo = new SqlParameter("@costo", datoArticolo.Costo);
            costo.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(costo);

            SqlParameter idArtArticoli = new SqlParameter("@idArtArticoli", datoArticolo.IdArtArticoli);
            idArtArticoli.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idArtArticoli);

            // INSERISCO ID RECUPERATO DALLA SP PRECEDENTE
            SqlParameter idDatiAgenda = new SqlParameter("@idDatiAgenda", iDatiAgendaReturn);
            idDatiAgenda.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idDatiAgenda);

            SqlParameter idTipoGenere = new SqlParameter("@idTipoGenere", datoArticolo.IdTipoGenere);
            idTipoGenere.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idTipoGenere);

            SqlParameter idTipoGruppo = new SqlParameter("@idTipoGruppo", datoArticolo.IdTipoGruppo);
            idTipoGruppo.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idTipoGruppo);

            SqlParameter idTipoSottogruppo = new SqlParameter("@idTipoSottogruppo", datoArticolo.IdTipoSottogruppo);
            idTipoSottogruppo.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idTipoSottogruppo);

            SqlParameter descrizione = new SqlParameter("@descrizione", datoArticolo.Descrizione);
            descrizione.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(descrizione);

            SqlParameter descrizioneLunga = new SqlParameter("@descrizioneLunga", datoArticolo.DescrizioneLunga);
            descrizioneLunga.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(descrizioneLunga);

            SqlParameter iva = new SqlParameter("@iva", datoArticolo.Iva);
            iva.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(iva);

            SqlParameter quantita = new SqlParameter("@quantita", datoArticolo.Quantita);
            quantita.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(quantita);

            SqlParameter prezzo = new SqlParameter("@prezzo", datoArticolo.Prezzo);
            prezzo.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(prezzo);

            SqlParameter stampa = new SqlParameter("@stampa", datoArticolo.Stampa);
            stampa.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(stampa);
        }

        private static void CostruisciSP_InsertDatiTender(SqlCommand StoreProc, SqlDataAdapter sda, int iDatiAgendaReturn, string idTender)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

            StoreProc.CommandType = CommandType.StoredProcedure;
            StoreProc.CommandText = "InsertDatiTender";
            StoreProc.Parameters.Clear();
            sda.SelectCommand = StoreProc;

            StoreProc.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

            // PARAMETRI PER LOG UTENTE
            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
            idUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idUtente);

            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
            nomeUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(nomeUtente);
            // FINE PARAMETRI PER LOG UTENTE

            SqlParameter idTenderSP = new SqlParameter("@idTender", idTender);
            idTenderSP.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idTenderSP);

            // INSERISCO ID RECUPERATO DALLA SP PRECEDENTE
            SqlParameter idDatiAgenda = new SqlParameter("@idDatiAgenda", iDatiAgendaReturn);
            idDatiAgenda.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idDatiAgenda);
        }

        private static void CostruisciSP_InsertProtocollo(SqlCommand StoreProc, SqlDataAdapter sda, int iDatiAgendaReturn, Protocolli protocollo)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

            StoreProc.CommandType = CommandType.StoredProcedure;
            StoreProc.CommandText = "[InsertProtocollo]";
            StoreProc.Parameters.Clear();
            sda.SelectCommand = StoreProc;

            StoreProc.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

            // PARAMETRI PER LOG UTENTE
            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
            idUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idUtente);

            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
            nomeUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(nomeUtente);
            // FINE PARAMETRI PER LOG UTENTE

            SqlParameter codice_lavoro = new SqlParameter("@codice_lavoro", protocollo.Codice_lavoro);
            codice_lavoro.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(codice_lavoro);

            SqlParameter numero_protocollo = new SqlParameter("@numero_protocollo", protocollo.Numero_protocollo);
            numero_protocollo.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(numero_protocollo);

            SqlParameter cliente = new SqlParameter("@cliente", protocollo.Cliente);
            cliente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(cliente);

            SqlParameter id_tipo_protocollo = new SqlParameter("@id_tipo_protocollo", protocollo.Id_tipo_protocollo);
            id_tipo_protocollo.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(id_tipo_protocollo);

            SqlParameter protocollo_riferimento = new SqlParameter("@protocollo_riferimento", protocollo.Protocollo_riferimento);
            protocollo_riferimento.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(protocollo_riferimento);

            SqlParameter pathDocumento = new SqlParameter("@pathDocumento", protocollo.PathDocumento);
            pathDocumento.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(pathDocumento);

            SqlParameter descrizione = new SqlParameter("@descrizione", protocollo.Descrizione);
            descrizione.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(descrizione);

            SqlParameter produzione = new SqlParameter("@produzione", protocollo.Produzione);
            produzione.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(produzione);

            SqlParameter data_inizio_lavorazione = new SqlParameter("@data_inizio_lavorazione", protocollo.Data_inizio_lavorazione);
            data_inizio_lavorazione.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(data_inizio_lavorazione);

            SqlParameter attivo = new SqlParameter("@attivo", protocollo.Attivo);
            attivo.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(attivo);
        }

        private static void CostruisciSP_InsertEvento(DatiAgenda evento, SqlCommand StoreProc, SqlDataAdapter sda)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

            sda.SelectCommand = StoreProc;
            StoreProc.CommandType = CommandType.StoredProcedure;

            StoreProc.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

            // PARAMETRI PER LOG UTENTE
            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
            idUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idUtente);

            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
            nomeUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(nomeUtente);
            // FINE PARAMETRI PER LOG UTENTE

            SqlParameter data_inizio_lavorazione = new SqlParameter("@data_inizio_lavorazione", SqlDbType.DateTime);
            data_inizio_lavorazione.Direction = ParameterDirection.Input;
            data_inizio_lavorazione.Value = evento.data_inizio_lavorazione;
            StoreProc.Parameters.Add(data_inizio_lavorazione);

            SqlParameter data_fine_lavorazione = new SqlParameter("@data_fine_lavorazione", SqlDbType.DateTime);
            data_fine_lavorazione.Direction = ParameterDirection.Input;
            data_fine_lavorazione.Value = evento.data_fine_lavorazione;
            StoreProc.Parameters.Add(data_fine_lavorazione);

            SqlParameter durata_lavorazione = new SqlParameter("@durata_lavorazione", SqlDbType.Int);
            durata_lavorazione.Direction = ParameterDirection.Input;
            durata_lavorazione.Value = evento.durata_lavorazione;
            StoreProc.Parameters.Add(durata_lavorazione);

            SqlParameter id_colonne_agenda = new SqlParameter("@id_colonne_agenda", SqlDbType.Int);
            id_colonne_agenda.Direction = ParameterDirection.Input;
            id_colonne_agenda.Value = evento.id_colonne_agenda;
            StoreProc.Parameters.Add(id_colonne_agenda);

            SqlParameter id_tipologia = new SqlParameter("@id_tipologia", SqlDbType.Int);
            id_tipologia.Direction = ParameterDirection.Input;
            id_tipologia.Value = evento.id_tipologia == 0 ? null : evento.id_tipologia;
            StoreProc.Parameters.Add(id_tipologia);

            SqlParameter id_stato = new SqlParameter("@id_stato", SqlDbType.Int);
            id_stato.Direction = ParameterDirection.Input;
            id_stato.Value = evento.id_stato;
            StoreProc.Parameters.Add(id_stato);

            SqlParameter id_cliente = new SqlParameter("@id_cliente", SqlDbType.Int);
            id_cliente.Direction = ParameterDirection.Input;
            id_cliente.Value = evento.id_cliente;
            StoreProc.Parameters.Add(id_cliente);

            SqlParameter durata_viaggio_andata = new SqlParameter("@durata_viaggio_andata", SqlDbType.Int);
            durata_viaggio_andata.Direction = ParameterDirection.Input;
            durata_viaggio_andata.Value = evento.durata_viaggio_andata;
            StoreProc.Parameters.Add(durata_viaggio_andata);

            SqlParameter durata_viaggio_ritorno = new SqlParameter("@durata_viaggio_ritorno", SqlDbType.Int);
            durata_viaggio_ritorno.Direction = ParameterDirection.Input;
            durata_viaggio_ritorno.Value = evento.durata_viaggio_ritorno;
            StoreProc.Parameters.Add(durata_viaggio_ritorno);

            SqlParameter data_inizio_impegno = new SqlParameter("@data_inizio_impegno", SqlDbType.DateTime);
            data_inizio_impegno.Direction = ParameterDirection.Input;
            data_inizio_impegno.Value = evento.data_inizio_impegno;
            StoreProc.Parameters.Add(data_inizio_impegno);

            SqlParameter data_fine_impegno = new SqlParameter("@data_fine_impegno", SqlDbType.DateTime);
            data_fine_impegno.Direction = ParameterDirection.Input;
            data_fine_impegno.Value = evento.data_fine_impegno;
            StoreProc.Parameters.Add(data_fine_impegno);

            SqlParameter impegnoOrario = new SqlParameter("@impegnoOrario", SqlDbType.Bit);
            impegnoOrario.Direction = ParameterDirection.Input;
            impegnoOrario.Value = evento.impegnoOrario;
            StoreProc.Parameters.Add(impegnoOrario);

            SqlParameter impegnoOrario_da = new SqlParameter("@impegnoOrario_da", SqlDbType.VarChar);
            impegnoOrario_da.Direction = ParameterDirection.Input;
            impegnoOrario_da.Value = evento.impegnoOrario_da;
            StoreProc.Parameters.Add(impegnoOrario_da);

            SqlParameter impegnoOrario_a = new SqlParameter("@impegnoOrario_a", SqlDbType.VarChar);
            impegnoOrario_a.Direction = ParameterDirection.Input;
            impegnoOrario_a.Value = evento.impegnoOrario_a;
            StoreProc.Parameters.Add(impegnoOrario_a);

            SqlParameter produzione = new SqlParameter("@produzione", SqlDbType.VarChar);
            produzione.Direction = ParameterDirection.Input;
            produzione.Value = evento.produzione;
            StoreProc.Parameters.Add(produzione);

            SqlParameter lavorazione = new SqlParameter("@lavorazione", SqlDbType.VarChar);
            lavorazione.Direction = ParameterDirection.Input;
            lavorazione.Value = evento.lavorazione;
            StoreProc.Parameters.Add(lavorazione);

            SqlParameter indirizzo = new SqlParameter("@indirizzo", SqlDbType.VarChar);
            indirizzo.Direction = ParameterDirection.Input;
            indirizzo.Value = evento.indirizzo;
            StoreProc.Parameters.Add(indirizzo);

            SqlParameter luogo = new SqlParameter("@luogo", SqlDbType.VarChar);
            luogo.Direction = ParameterDirection.Input;
            luogo.Value = evento.luogo;
            StoreProc.Parameters.Add(luogo);

            SqlParameter codice_lavoro = new SqlParameter("@codice_lavoro", SqlDbType.VarChar);
            codice_lavoro.Direction = ParameterDirection.Input;
            codice_lavoro.Value = evento.codice_lavoro;
            StoreProc.Parameters.Add(codice_lavoro);

            SqlParameter nota = new SqlParameter("@nota", SqlDbType.VarChar);
            nota.Direction = ParameterDirection.Input;
            nota.Value = evento.nota;
            StoreProc.Parameters.Add(nota);
        }

        private static void CostruisciSP_UpdateEvento(DatiAgenda evento, SqlCommand StoreProc, SqlDataAdapter sda)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

            sda.SelectCommand = StoreProc;
            StoreProc.CommandType = CommandType.StoredProcedure;

            SqlParameter id = new SqlParameter("@id", SqlDbType.Int);
            id.Direction = ParameterDirection.Input;
            id.Value = evento.id;
            StoreProc.Parameters.Add(id);

            // PARAMETRI PER LOG UTENTE
            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
            idUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idUtente);

            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
            nomeUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(nomeUtente);
            // FINE PARAMETRI PER LOG UTENTE

            SqlParameter data_inizio_lavorazione = new SqlParameter("@data_inizio_lavorazione", SqlDbType.DateTime);
            data_inizio_lavorazione.Direction = ParameterDirection.Input;
            data_inizio_lavorazione.Value = evento.data_inizio_lavorazione;
            StoreProc.Parameters.Add(data_inizio_lavorazione);

            SqlParameter data_fine_lavorazione = new SqlParameter("@data_fine_lavorazione", SqlDbType.DateTime);
            data_fine_lavorazione.Direction = ParameterDirection.Input;
            data_fine_lavorazione.Value = evento.data_fine_lavorazione;
            StoreProc.Parameters.Add(data_fine_lavorazione);

            SqlParameter durata_lavorazione = new SqlParameter("@durata_lavorazione", SqlDbType.Int);
            durata_lavorazione.Direction = ParameterDirection.Input;
            durata_lavorazione.Value = evento.durata_lavorazione;
            StoreProc.Parameters.Add(durata_lavorazione);

            SqlParameter id_colonne_agenda = new SqlParameter("@id_colonne_agenda", SqlDbType.Int);
            id_colonne_agenda.Direction = ParameterDirection.Input;
            id_colonne_agenda.Value = evento.id_colonne_agenda;
            StoreProc.Parameters.Add(id_colonne_agenda);

            SqlParameter id_tipologia = new SqlParameter("@id_tipologia", SqlDbType.Int);
            id_tipologia.Direction = ParameterDirection.Input;
            id_tipologia.Value = evento.id_tipologia == 0 ? null : evento.id_tipologia;
            StoreProc.Parameters.Add(id_tipologia);

            SqlParameter id_stato = new SqlParameter("@id_stato", SqlDbType.Int);
            id_stato.Direction = ParameterDirection.Input;
            id_stato.Value = evento.id_stato;
            StoreProc.Parameters.Add(id_stato);

            SqlParameter id_cliente = new SqlParameter("@id_cliente", SqlDbType.Int);
            id_cliente.Direction = ParameterDirection.Input;
            id_cliente.Value = evento.id_cliente;
            StoreProc.Parameters.Add(id_cliente);

            SqlParameter durata_viaggio_andata = new SqlParameter("@durata_viaggio_andata", SqlDbType.Int);
            durata_viaggio_andata.Direction = ParameterDirection.Input;
            durata_viaggio_andata.Value = evento.durata_viaggio_andata;
            StoreProc.Parameters.Add(durata_viaggio_andata);

            SqlParameter durata_viaggio_ritorno = new SqlParameter("@durata_viaggio_ritorno", SqlDbType.Int);
            durata_viaggio_ritorno.Direction = ParameterDirection.Input;
            durata_viaggio_ritorno.Value = evento.durata_viaggio_ritorno;
            StoreProc.Parameters.Add(durata_viaggio_ritorno);

            SqlParameter data_inizio_impegno = new SqlParameter("@data_inizio_impegno", SqlDbType.DateTime);
            data_inizio_impegno.Direction = ParameterDirection.Input;
            data_inizio_impegno.Value = evento.data_inizio_impegno;
            StoreProc.Parameters.Add(data_inizio_impegno);

            SqlParameter data_fine_impegno = new SqlParameter("@data_fine_impegno", SqlDbType.DateTime);
            data_fine_impegno.Direction = ParameterDirection.Input;
            data_fine_impegno.Value = evento.data_fine_impegno;
            StoreProc.Parameters.Add(data_fine_impegno);

            SqlParameter impegnoOrario = new SqlParameter("@impegnoOrario", SqlDbType.Bit);
            impegnoOrario.Direction = ParameterDirection.Input;
            impegnoOrario.Value = evento.impegnoOrario;
            StoreProc.Parameters.Add(impegnoOrario);

            SqlParameter impegnoOrario_da = new SqlParameter("@impegnoOrario_da", SqlDbType.VarChar);
            impegnoOrario_da.Direction = ParameterDirection.Input;
            impegnoOrario_da.Value = evento.impegnoOrario_da;
            StoreProc.Parameters.Add(impegnoOrario_da);

            SqlParameter impegnoOrario_a = new SqlParameter("@impegnoOrario_a", SqlDbType.VarChar);
            impegnoOrario_a.Direction = ParameterDirection.Input;
            impegnoOrario_a.Value = evento.impegnoOrario_a;
            StoreProc.Parameters.Add(impegnoOrario_a);

            SqlParameter produzione = new SqlParameter("@produzione", SqlDbType.VarChar);
            produzione.Direction = ParameterDirection.Input;
            produzione.Value = evento.produzione;
            StoreProc.Parameters.Add(produzione);

            SqlParameter lavorazione = new SqlParameter("@lavorazione", SqlDbType.VarChar);
            lavorazione.Direction = ParameterDirection.Input;
            lavorazione.Value = evento.lavorazione;
            StoreProc.Parameters.Add(lavorazione);

            SqlParameter indirizzo = new SqlParameter("@indirizzo", SqlDbType.VarChar);
            indirizzo.Direction = ParameterDirection.Input;
            indirizzo.Value = evento.indirizzo;
            StoreProc.Parameters.Add(indirizzo);

            SqlParameter luogo = new SqlParameter("@luogo", SqlDbType.VarChar);
            luogo.Direction = ParameterDirection.Input;
            luogo.Value = evento.luogo;
            StoreProc.Parameters.Add(luogo);

            SqlParameter codice_lavoro = new SqlParameter("@codice_lavoro", SqlDbType.VarChar);
            codice_lavoro.Direction = ParameterDirection.Input;
            codice_lavoro.Value = evento.codice_lavoro;
            StoreProc.Parameters.Add(codice_lavoro);

            SqlParameter nota = new SqlParameter("@nota", SqlDbType.VarChar);
            nota.Direction = ParameterDirection.Input;
            nota.Value = evento.nota;
            StoreProc.Parameters.Add(nota);
        }

        private static void CostruisciSP_DeleteDatiArticoli(SqlCommand StoreProc, SqlDataAdapter sda, int idDatiAgenda)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

            StoreProc.CommandType = CommandType.StoredProcedure;
            StoreProc.CommandText = "DeleteDatiArticoliByIdDatiAgenda";
            StoreProc.Parameters.Clear();
            sda.SelectCommand = StoreProc;

            SqlParameter parIdDatiAgenda = new SqlParameter("@idDatiAgenda", idDatiAgenda);
            parIdDatiAgenda.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(parIdDatiAgenda);
            
            // PARAMETRI PER LOG UTENTE
            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
            idUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idUtente);

            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
            nomeUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(nomeUtente);
            // FINE PARAMETRI PER LOG UTENTE

        }

        private static void CostruisciSP_DeleteDatiTender(SqlCommand StoreProc, SqlDataAdapter sda, int idDatiAgenda)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

            StoreProc.CommandType = CommandType.StoredProcedure;
            StoreProc.CommandText = "DeleteDatiTenderByIdDatiAgenda";
            StoreProc.Parameters.Clear();
            sda.SelectCommand = StoreProc;

            SqlParameter parIdDatiAgenda = new SqlParameter("@idDatiAgenda", idDatiAgenda);
            parIdDatiAgenda.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(parIdDatiAgenda);

            // PARAMETRI PER LOG UTENTE
            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
            idUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idUtente);

            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
            nomeUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(nomeUtente);
            // FINE PARAMETRI PER LOG UTENTE

        }

        private static void CostruisciSP_InsertNoteOfferta(SqlCommand StoreProc, SqlDataAdapter sda, int iDatiAgendaReturn, NoteOfferta noteOfferta)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

            StoreProc.CommandType = CommandType.StoredProcedure;
            StoreProc.CommandText = "InsertNoteOfferta";
            StoreProc.Parameters.Clear();
            sda.SelectCommand = StoreProc;

            StoreProc.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

            // PARAMETRI PER LOG UTENTE
            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
            idUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idUtente);

            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
            nomeUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(nomeUtente);
            // FINE PARAMETRI PER LOG UTENTE


            // INSERISCO ID RECUPERATO DALLA SP PRECEDENTE
            SqlParameter idDatiAgenda = new SqlParameter("@id_dati_agenda", iDatiAgendaReturn);
            idDatiAgenda.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idDatiAgenda);

            SqlParameter parBanca = new SqlParameter("@banca", noteOfferta.Banca);
            parBanca.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(parBanca);

            SqlParameter parPagamento = new SqlParameter("@pagamento", noteOfferta.Pagamento);
            parPagamento.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(parPagamento);

            SqlParameter parConsegna = new SqlParameter("@consegna", noteOfferta.Consegna);
            parConsegna.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(parConsegna);
        }

        private static void CostruisciSP_DeleteDatiLavorazione(SqlCommand StoreProc, SqlDataAdapter sda, int idDatiAgenda)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

            StoreProc.CommandType = CommandType.StoredProcedure;
            StoreProc.CommandText = "DeleteDatiLavorazioneByIdDatiAgenda";
            StoreProc.Parameters.Clear();
            sda.SelectCommand = StoreProc;

            SqlParameter parIdDatiAgenda = new SqlParameter("@idDatiAgenda", idDatiAgenda);
            parIdDatiAgenda.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(parIdDatiAgenda);

            // PARAMETRI PER LOG UTENTE
            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
            idUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idUtente);

            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
            nomeUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(nomeUtente);
            // FINE PARAMETRI PER LOG UTENTE

        }

        private static void CostruisciSP_InsertDatiLavorazione(SqlCommand StoreProc, SqlDataAdapter sda, int iDatiAgendaReturn, DatiLavorazione datoLavorazione)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

            StoreProc.CommandType = CommandType.StoredProcedure;
            StoreProc.CommandText = "InsertDatiLavorazione";
            StoreProc.Parameters.Clear();
            sda.SelectCommand = StoreProc;

            StoreProc.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

            // PARAMETRI PER LOG UTENTE
            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
            idUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idUtente);

            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
            nomeUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(nomeUtente);
            // FINE PARAMETRI PER LOG UTENTE

            // INSERISCO ID RECUPERATO DALLA SP PRECEDENTE
            SqlParameter idDatiAgenda = new SqlParameter("@idDatiAgenda", iDatiAgendaReturn);
            idDatiAgenda.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idDatiAgenda);

            SqlParameter idContratto = new SqlParameter("@idContratto", DBNull.Value);
            if (datoLavorazione.IdContratto != null)
            {
                idContratto = new SqlParameter("@idContratto", datoLavorazione.IdContratto);
            }
            idContratto.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idContratto);

            SqlParameter idReferente = new SqlParameter("@idReferente", DBNull.Value);
            if (datoLavorazione.IdReferente != null)
            {
                idReferente = new SqlParameter("@idReferente", datoLavorazione.IdReferente);
            }
            idReferente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idReferente);

            SqlParameter idCapoTecnico = new SqlParameter("@idCapoTecnico", DBNull.Value);
            if (datoLavorazione.IdCapoTecnico != null)
            {
                idCapoTecnico = new SqlParameter("@idCapoTecnico", datoLavorazione.IdCapoTecnico);
            }
            idCapoTecnico.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idCapoTecnico);


            SqlParameter idProduttore = new SqlParameter("@idProduttore", DBNull.Value);
            if (datoLavorazione.IdProduttore != null)
            {
                idProduttore = new SqlParameter("@idProduttore", datoLavorazione.IdProduttore);
            }
            idProduttore.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idProduttore);

            SqlParameter ordine = new SqlParameter("@ordine", datoLavorazione.Ordine);
            ordine.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(ordine);

            SqlParameter fattura = new SqlParameter("@fattura", datoLavorazione.Fattura);
            fattura.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(fattura);
        }

        private static void CostruisciSP_InsertDatiArticoliLavorazione(SqlCommand StoreProc, SqlDataAdapter sda, int idDatiLavorazioneReturn, DatiArticoliLavorazione datoArticoloLavorazione)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

            StoreProc.CommandType = CommandType.StoredProcedure;
            StoreProc.CommandText = "InsertDatiArticoliLavorazione";
            StoreProc.Parameters.Clear();
            sda.SelectCommand = StoreProc;

            StoreProc.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

            // PARAMETRI PER LOG UTENTE
            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
            idUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idUtente);

            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
            nomeUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(nomeUtente);
            // FINE PARAMETRI PER LOG UTENTE


            // INSERISCO ID RECUPERATO DALLA SP PRECEDENTE
            SqlParameter idDatiLavorazione = new SqlParameter("@idDatiLavorazione", idDatiLavorazioneReturn);
            idDatiLavorazione.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idDatiLavorazione);

            SqlParameter idArtArticoli = new SqlParameter("@idArtArticoli", datoArticoloLavorazione.IdArtArticoli);
            idArtArticoli.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idArtArticoli);

            SqlParameter idTipoGenere = new SqlParameter("@idTipoGenere", datoArticoloLavorazione.IdTipoGenere);
            idTipoGenere.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idTipoGenere);

            SqlParameter idTipoGruppo = new SqlParameter("@idTipoGruppo", datoArticoloLavorazione.IdTipoGruppo);
            idTipoGruppo.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idTipoGruppo);

            SqlParameter idTipoSottogruppo = new SqlParameter("@idTipoSottogruppo", datoArticoloLavorazione.IdTipoSottogruppo);
            idTipoSottogruppo.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idTipoSottogruppo);

            SqlParameter idCollaboratori = new SqlParameter("@idCollaboratori", DBNull.Value);
            if (datoArticoloLavorazione.IdCollaboratori != null)
            {
                idCollaboratori = new SqlParameter("@idCollaboratori", datoArticoloLavorazione.IdCollaboratori);
            }
            idCollaboratori.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idCollaboratori);

            SqlParameter idFornitori = new SqlParameter("@idFornitori", DBNull.Value);
            if (datoArticoloLavorazione.IdFornitori != null)
            {
                idFornitori = new SqlParameter("@idFornitori", datoArticoloLavorazione.IdFornitori);
            }
            idFornitori.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idFornitori);

            SqlParameter idTipoPagamento = new SqlParameter("@idTipoPagamento", DBNull.Value);
            if (datoArticoloLavorazione.IdTipoPagamento != null)
            {
                idTipoPagamento = new SqlParameter("@idTipoPagamento", datoArticoloLavorazione.IdTipoPagamento);
            }
            idTipoPagamento.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idTipoPagamento);

            SqlParameter descrizione = new SqlParameter("@descrizione", datoArticoloLavorazione.Descrizione);
            descrizione.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(descrizione);

            SqlParameter descrizioneLunga = new SqlParameter("@descrizioneLunga", datoArticoloLavorazione.DescrizioneLunga);
            descrizioneLunga.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(descrizioneLunga);

            SqlParameter stampa = new SqlParameter("@stampa", datoArticoloLavorazione.Stampa);
            stampa.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(stampa);

            SqlParameter prezzo = new SqlParameter("@prezzo", datoArticoloLavorazione.Prezzo);
            prezzo.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(prezzo);

            SqlParameter costo = new SqlParameter("@costo", datoArticoloLavorazione.Costo);
            costo.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(costo);

            SqlParameter iva = new SqlParameter("@iva", datoArticoloLavorazione.Iva);
            iva.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(iva);

            SqlParameter data = new SqlParameter("@data", DBNull.Value);
            if (datoArticoloLavorazione.Data != null)
            {
                data = new SqlParameter("@data", datoArticoloLavorazione.Data);
            }
            data.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(data);

            SqlParameter tv = new SqlParameter("@tv", DBNull.Value);
            if (datoArticoloLavorazione.Tv != null)
            {
                tv = new SqlParameter("@tv", datoArticoloLavorazione.Tv);
            }
            tv.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(tv);

            SqlParameter nota = new SqlParameter("@nota", DBNull.Value);
            if (datoArticoloLavorazione.Nota != null)
            {
                nota = new SqlParameter("@nota", datoArticoloLavorazione.Nota);
            }
            nota.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(nota);

            SqlParameter fp_netto = new SqlParameter("@fp_netto", DBNull.Value);
            if (datoArticoloLavorazione.FP_netto != null)
            {
                fp_netto = new SqlParameter("@fp_netto", datoArticoloLavorazione.FP_netto);
            }
            fp_netto.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(fp_netto);

            SqlParameter fp_lordo = new SqlParameter("@fp_lordo", DBNull.Value);
            if (datoArticoloLavorazione.FP_lordo != null)
            {
                fp_lordo = new SqlParameter("@fp_lordo", datoArticoloLavorazione.FP_lordo);
            }
            fp_lordo.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(fp_lordo);

            SqlParameter usaCostoFP = new SqlParameter("@usaCostoFP", DBNull.Value);
            if (datoArticoloLavorazione.UsaCostoFP != null)
            {
                usaCostoFP = new SqlParameter("@usaCostoFP", datoArticoloLavorazione.UsaCostoFP);
            }
            fp_lordo.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(usaCostoFP);
        }

        private static void CostruisciSP_DeleteDatiArticoliLavorazione(SqlCommand StoreProc, SqlDataAdapter sda, int idDatiLavorazione)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

            StoreProc.CommandType = CommandType.StoredProcedure;
            StoreProc.CommandText = "DeleteDatiArticoliLavorazioneByIdDatiLavorazione";
            StoreProc.Parameters.Clear();
            sda.SelectCommand = StoreProc;

            SqlParameter parIdDatiLavorazione = new SqlParameter("@idDatiLavorazione", idDatiLavorazione);
            parIdDatiLavorazione.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(parIdDatiLavorazione);

            // PARAMETRI PER LOG UTENTE
            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
            idUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idUtente);

            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
            nomeUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(nomeUtente);
            // FINE PARAMETRI PER LOG UTENTE

        }

        private static void CostruisciSP_DeleteDatiPianoEsternoLavorazione(SqlCommand StoreProc, SqlDataAdapter sda, int idDatiLavorazione)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

            StoreProc.CommandType = CommandType.StoredProcedure;
            StoreProc.CommandText = "DeleteDatiPianoEsternoLavorazioneByIdDatiLavorazione";
            StoreProc.Parameters.Clear();
            sda.SelectCommand = StoreProc;

            SqlParameter parIdDatiLavorazione = new SqlParameter("@idDatiLavorazione", idDatiLavorazione);
            parIdDatiLavorazione.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(parIdDatiLavorazione);

            // PARAMETRI PER LOG UTENTE
            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
            idUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idUtente);

            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
            nomeUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(nomeUtente);
            // FINE PARAMETRI PER LOG UTENTE

        }

        private static void CostruisciSP_InsertDatiPianoEsternoLavorazione(SqlCommand StoreProc, SqlDataAdapter sda, int idDatiLavorazione, DatiPianoEsternoLavorazione datoPianoEsternoLavorazione)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

            StoreProc.CommandType = CommandType.StoredProcedure;
            StoreProc.CommandText = "InsertDatiPianoEsternoLavorazione";
            StoreProc.Parameters.Clear();
            sda.SelectCommand = StoreProc;

            StoreProc.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

            // PARAMETRI PER LOG UTENTE
            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
            idUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idUtente);

            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
            nomeUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(nomeUtente);
            // FINE PARAMETRI PER LOG UTENTE


            // INSERISCO ID RECUPERATO DALLA SP PRECEDENTE
            SqlParameter par_idDatiLavorazione = new SqlParameter("@idDatiLavorazione", idDatiLavorazione);
            par_idDatiLavorazione.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(par_idDatiLavorazione);

            SqlParameter idCollaboratori = new SqlParameter("@idCollaboratori", DBNull.Value);
            if (datoPianoEsternoLavorazione.IdCollaboratori != null)
            {
                idCollaboratori = new SqlParameter("@idCollaboratori", datoPianoEsternoLavorazione.IdCollaboratori);
            }
            idCollaboratori.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idCollaboratori);

            SqlParameter idFornitori = new SqlParameter("@idFornitori", DBNull.Value);
            if (datoPianoEsternoLavorazione.IdFornitori != null)
            {
                idFornitori = new SqlParameter("@idFornitori", datoPianoEsternoLavorazione.IdFornitori);
            }
            idFornitori.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idFornitori);

            SqlParameter idIntervento = new SqlParameter("@idIntervento", DBNull.Value);
            if (datoPianoEsternoLavorazione.IdIntervento != null)
            {
                idIntervento = new SqlParameter("@idTipoPagamento", datoPianoEsternoLavorazione.IdIntervento);
            }
            idIntervento.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idIntervento);

            SqlParameter diaria = new SqlParameter("@diaria", DBNull.Value);
            if (datoPianoEsternoLavorazione.Diaria != null)
            {
                diaria = new SqlParameter("@diaria", datoPianoEsternoLavorazione.Diaria);
            }
            diaria.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(diaria);

            SqlParameter importoDiaria = new SqlParameter("@importoDiaria", DBNull.Value);
            if (datoPianoEsternoLavorazione.ImportoDiaria != null)
            {
                importoDiaria = new SqlParameter("@importoDiaria", datoPianoEsternoLavorazione.ImportoDiaria);
            }
            importoDiaria.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(importoDiaria);

            SqlParameter albergo = new SqlParameter("@albergo", DBNull.Value);
            if (datoPianoEsternoLavorazione.Albergo != null)
            {
                albergo = new SqlParameter("@albergo", datoPianoEsternoLavorazione.Albergo);
            }
            albergo.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(albergo);

            SqlParameter data = new SqlParameter("@data", DBNull.Value);
            if (datoPianoEsternoLavorazione.Data != null)
            {
                data = new SqlParameter("@data", datoPianoEsternoLavorazione.Data);
            }
            data.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(data);

            SqlParameter orario = new SqlParameter("@orario", DBNull.Value);
            if (datoPianoEsternoLavorazione.Orario != null)
            {
                orario = new SqlParameter("@orario", datoPianoEsternoLavorazione.Orario);
            }
            orario.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(orario);

            SqlParameter nota = new SqlParameter("@nota", DBNull.Value);
            if (datoPianoEsternoLavorazione.Nota != null)
            {
                nota = new SqlParameter("@nota", datoPianoEsternoLavorazione.Nota);
            }
            nota.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(nota);
        }
    }
}