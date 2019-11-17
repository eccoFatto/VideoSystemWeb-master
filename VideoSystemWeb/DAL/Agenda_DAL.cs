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
    public class Agenda_DAL : Base_DAL
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
            return CaricaColonneAgenda(true, ref esito);
        }

        public List<DatiAgenda> CaricaDatiAgenda(ref Esito esito)
        {
            List<DatiAgenda> listaDatiAgenda = new List<DatiAgenda>();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM " + TABELLA_DATI_AGENDA))
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
                                        DatiAgenda datoAgenda = new DatiAgenda
                                        {
                                            id = riga.Field<int>("id"),
                                            id_colonne_agenda = riga.Field<int>("id_colonne_agenda"),
                                            id_stato = riga.Field<int>("id_stato"),
                                            data_inizio_lavorazione = riga.Field<DateTime>("data_inizio_lavorazione"),
                                            data_fine_lavorazione = riga.Field<DateTime>("data_fine_lavorazione"),
                                            durata_lavorazione = riga.Field<int>("durata_lavorazione"),
                                            id_tipologia = riga.Field<int?>("id_tipologia"),
                                            id_cliente = riga.Field<int>("id_cliente"),
                                            durata_viaggio_andata = riga.Field<int>("durata_viaggio_andata"),
                                            durata_viaggio_ritorno = riga.Field<int>("durata_viaggio_ritorno"),
                                            data_inizio_impegno = riga.Field<DateTime>("data_inizio_impegno"),
                                            data_fine_impegno = riga.Field<DateTime>("data_fine_impegno"),
                                            impegnoOrario = riga.Field<bool>("impegnoOrario"),
                                            impegnoOrario_da = riga.Field<string>("impegnoOrario_da"),
                                            impegnoOrario_a = riga.Field<string>("impegnoOrario_a"),
                                            produzione = riga.Field<string>("produzione"),
                                            lavorazione = riga.Field<string>("lavorazione"),
                                            indirizzo = riga.Field<string>("indirizzo"),
                                            luogo = riga.Field<string>("luogo"),
                                            codice_lavoro = riga.Field<string>("codice_lavoro"),
                                            nota = riga.Field<string>("nota")
                                        };

                                        listaDatiAgenda.Add(datoAgenda);
                                    }
                                }
                                else
                                {
                                    esito.Codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                    esito.Descrizione = "Nessun dato trovato nella tabella tab_dati_agenda ";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.Descrizione = "Agenda_DAL.cs - CaricaDatiAgenda " + ex.Message + Environment.NewLine + ex.StackTrace;

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
                                        DatiAgenda datoAgenda = new DatiAgenda
                                        {
                                            id = riga.Field<int>("id"),
                                            id_colonne_agenda = riga.Field<int>("id_colonne_agenda"),
                                            id_stato = riga.Field<int>("id_stato"),
                                            data_inizio_lavorazione = riga.Field<DateTime>("data_inizio_lavorazione"),
                                            data_fine_lavorazione = riga.Field<DateTime>("data_fine_lavorazione"),
                                            durata_lavorazione = riga.Field<int>("durata_lavorazione"),
                                            id_tipologia = riga.Field<int?>("id_tipologia"),
                                            id_cliente = riga.Field<int>("id_cliente"),
                                            durata_viaggio_andata = riga.Field<int>("durata_viaggio_andata"),
                                            durata_viaggio_ritorno = riga.Field<int>("durata_viaggio_ritorno"),
                                            data_inizio_impegno = riga.Field<DateTime>("data_inizio_impegno"),
                                            data_fine_impegno = riga.Field<DateTime>("data_fine_impegno"),
                                            impegnoOrario = riga.Field<bool>("impegnoOrario"),
                                            impegnoOrario_da = riga.Field<string>("impegnoOrario_da"),
                                            impegnoOrario_a = riga.Field<string>("impegnoOrario_a"),
                                            produzione = riga.Field<string>("produzione"),
                                            lavorazione = riga.Field<string>("lavorazione"),
                                            indirizzo = riga.Field<string>("indirizzo"),
                                            luogo = riga.Field<string>("luogo"),
                                            codice_lavoro = riga.Field<string>("codice_lavoro"),
                                            nota = riga.Field<string>("nota")
                                        };

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
                esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.Descrizione = "Agenda_DAL.cs - CaricaDatiAgenda " + ex.Message + Environment.NewLine + ex.StackTrace;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return listaDatiAgenda;
        }

        // Controllo la disponibilità dei tender nel periodo selezionato, ESCLUDENDO quelli dell'evento corrente
        public List<int> GetTenderImpiegatiInPeriodo(DateTime dataInizio, DateTime dataFine, int idEventoCorrente, ref Esito esito)
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
                esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.Descrizione = ex.Message + Environment.NewLine + ex.StackTrace;
            }
            return listaIdTender;
        }

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
                                    string nomeFile = "Offerta_" + evento.codice_lavoro + ".pdf";
                                    Protocolli protocolloOfferta = new Protocolli
                                    {
                                        Codice_lavoro = evento.codice_lavoro,
                                        Numero_protocollo = protocollo,
                                        Cliente = Anag_Clienti_Fornitori_BLL.Instance.getAziendaById(evento.id_cliente, ref esito).RagioneSociale,
                                        Id_cliente = evento.id_cliente,
                                        Id_tipo_protocollo = UtilityTipologiche.getElementByNome(UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_PROTOCOLLO), "offerta", ref esito).id,
                                        Protocollo_riferimento = "",
                                        PathDocumento = nomeFile,
                                        Lavorazione = evento.lavorazione.Trim(),
                                        Descrizione = "",
                                        Produzione = evento.produzione.Trim(),
                                        Data_inizio_lavorazione = evento.data_inizio_lavorazione,
                                        Attivo = true
                                    };
                                    
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
                            esito.Codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                            esito.Descrizione = "Agenda_DAL.cs - creaEvento " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;

                            try
                            {
                                transaction.Rollback();
                            }
                            catch (Exception ex2)
                            {
                                esito.Descrizione += Environment.NewLine + "ERRORE ROLLBACK: " + ex2.Message;
                                log.Error(ex2.Message + Environment.NewLine + ex2.StackTrace);
                            }

                            log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
                        }

                    }
                }
            }
            return esito;
        }

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
                                        string nomeFile = "Offerta_" + evento.codice_lavoro + ".pdf";
                                        Protocolli protocolloOfferta = new Protocolli
                                        {
                                            Codice_lavoro = evento.codice_lavoro,
                                            Numero_protocollo = protocollo,
                                            Cliente = Anag_Clienti_Fornitori_BLL.Instance.getAziendaById(evento.id_cliente, ref esito).RagioneSociale,
                                            Id_cliente = evento.id_cliente,
                                            Id_tipo_protocollo = idTipoProtocollo,
                                            Protocollo_riferimento = "",
                                            PathDocumento = nomeFile,
                                            Lavorazione = evento.lavorazione.Trim(),
                                            Descrizione = "",
                                            Produzione = evento.produzione.Trim(),
                                            Data_inizio_lavorazione = evento.data_inizio_lavorazione,
                                            Attivo = true
                                        };

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
                                    if (evento.LavorazioneCorrente.Id != 0) 
                                    {
                                        CostruisciSP_DeleteDatiArticoliLavorazione(StoreProc, sda, evento.LavorazioneCorrente.Id);
                                        StoreProc.ExecuteNonQuery();

                                        CostruisciSP_DeleteDatiPianoEsternoLavorazione(StoreProc, sda, evento.LavorazioneCorrente.Id);
                                        StoreProc.ExecuteNonQuery();
                                    }
                                    else // caso in cui la lavorazione sia stata appena creata ma non salvata (passaggio da offerta a lavorazione). Questo caso non dovrebbe sussistere, ma si è verificato. Probabilmente dati sporchi nel db
                                    {
                                        
                                        DatiLavorazione lavorazioneCorrente = Dati_Lavorazione_DAL.Instance.getDatiLavorazioneByIdEvento(evento.id, ref esito);

                                        if (lavorazioneCorrente != null)
                                        {
                                            CostruisciSP_DeleteDatiArticoliLavorazione(StoreProc, sda, lavorazioneCorrente.Id);
                                            StoreProc.ExecuteNonQuery();

                                            CostruisciSP_DeleteDatiPianoEsternoLavorazione(StoreProc, sda, lavorazioneCorrente.Id);
                                            StoreProc.ExecuteNonQuery();
                                        }
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
                                esito.Codice = Esito.ESITO_KO_ERRORE_UPDATE_TABELLA;
                                esito.Descrizione = "Agenda_DAL.cs - AggiornaEvento" + Environment.NewLine + ex.Message;

                                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);

                                try
                                {
                                    transaction.Rollback();
                                }
                                catch (Exception ex2)
                                {
                                    esito.Descrizione += Environment.NewLine + "ERRORE ROLLBACK: " + ex2.Message;
                                    log.Error(ex2.Message + Environment.NewLine + ex2.StackTrace);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.Descrizione = "Agenda_DAL.cs - AggiornaEventoConArticoli " + Environment.NewLine + ex.Message;

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
                    using (SqlCommand StoreProc = new SqlCommand("DeleteNoteOffertaByIdDatiAgenda"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            SqlTransaction transaction;
                            StoreProc.Connection = con;
                            StoreProc.Connection.Open();
                            // Start a local transaction.
                            transaction = con.BeginTransaction("DeleteEvento");

                            try
                            {
                                StoreProc.Transaction = transaction;

                                #region NOTE_OFFERTA
                                StoreProc.Parameters.Clear();
                                StoreProc.CommandType = CommandType.StoredProcedure;
                                sda.SelectCommand = StoreProc;

                                SqlParameter id_dati_agenda = new SqlParameter("@id_dati_agenda", SqlDbType.Int);
                                id_dati_agenda.Direction = ParameterDirection.Input;
                                id_dati_agenda.Value = idEvento;
                                StoreProc.Parameters.Add(id_dati_agenda);

                                // PARAMETRI PER LOG UTENTE
                                SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
                                idUtente.Direction = ParameterDirection.Input;
                                StoreProc.Parameters.Add(idUtente);

                                SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
                                nomeUtente.Direction = ParameterDirection.Input;
                                StoreProc.Parameters.Add(nomeUtente);
                                // FINE PARAMETRI PER LOG UTENTE

                                StoreProc.ExecuteNonQuery();
                                #endregion

                                #region EVENTO
                                StoreProc.CommandText = "DeleteEvento";
                                StoreProc.Parameters.Clear();
                                StoreProc.CommandType = CommandType.StoredProcedure;
                                sda.SelectCommand = StoreProc;

                                SqlParameter id = new SqlParameter("@id", SqlDbType.Int);
                                id.Direction = ParameterDirection.Input;
                                id.Value = idEvento;
                                StoreProc.Parameters.Add(id);

                                // PARAMETRI PER LOG UTENTE
                                idUtente = new SqlParameter("@idUtente", utente.id);
                                idUtente.Direction = ParameterDirection.Input;
                                StoreProc.Parameters.Add(idUtente);

                                nomeUtente = new SqlParameter("@nomeUtente", utente.username);
                                nomeUtente.Direction = ParameterDirection.Input;
                                StoreProc.Parameters.Add(nomeUtente);
                                // FINE PARAMETRI PER LOG UTENTE

                                //StoreProc.Connection.Open();

                                StoreProc.ExecuteNonQuery();
                                #endregion

                                // Attempt to commit the transaction.
                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                esito.Codice = Esito.ESITO_KO_ERRORE_UPDATE_TABELLA;
                                esito.Descrizione = "Agenda_DAL.cs - EliminaEvento" + Environment.NewLine + ex.Message;

                                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);

                                try
                                {
                                    transaction.Rollback();
                                }
                                catch (Exception ex2)
                                {
                                    esito.Descrizione += Environment.NewLine + "ERRORE ROLLBACK: " + ex2.Message;
                                    log.Error(ex2.Message + Environment.NewLine + ex2.StackTrace);
                                }
                            }



                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.Descrizione = "Agenda_DAL.cs - EliminaEvento " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return esito;
        }

        // Elimina tutti i dati della lavorazione quando si elimina un evento in fase Lavorazione (lo riporta in fase Offerta)
        public Esito EliminaLavorazione(DatiAgenda evento)
        {
            Esito esito = new Esito();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("DeleteDatiArticoliLavorazioneByIdDatiLavorazione"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            SqlTransaction transaction;
                            StoreProc.Connection = con;
                            StoreProc.Connection.Open();
                            // Start a local transaction.
                            transaction = con.BeginTransaction("DeleteLavorazione");

                            try
                            {
                                StoreProc.Transaction = transaction;
                                if (evento.LavorazioneCorrente != null)
                                {
                                    if (evento.LavorazioneCorrente.Id != 0) 
                                    {
                                        CostruisciSP_DeleteDatiArticoliLavorazione(StoreProc, sda, evento.LavorazioneCorrente.Id);
                                        StoreProc.ExecuteNonQuery();

                                        CostruisciSP_DeleteDatiPianoEsternoLavorazione(StoreProc, sda, evento.LavorazioneCorrente.Id);
                                        StoreProc.ExecuteNonQuery();
                                    }
                                    else // caso in cui la lavorazione sia stata appena creata ma non salvata (passaggio da offerta a lavorazione). Questo caso non dovrebbe sussistere, ma si è verificato. Probabilmente dati sporchi nel db
                                    {

                                        DatiLavorazione lavorazioneCorrente = Dati_Lavorazione_DAL.Instance.getDatiLavorazioneByIdEvento(evento.id, ref esito);
                                        if (lavorazioneCorrente != null)
                                        {
                                            CostruisciSP_DeleteDatiArticoliLavorazione(StoreProc, sda, lavorazioneCorrente.Id);
                                            StoreProc.ExecuteNonQuery();

                                            CostruisciSP_DeleteDatiPianoEsternoLavorazione(StoreProc, sda, lavorazioneCorrente.Id);
                                            StoreProc.ExecuteNonQuery();
                                        }
                                    }

                                    CostruisciSP_DeleteDatiLavorazione(StoreProc, sda, evento.id);
                                    StoreProc.ExecuteNonQuery();
                                }

                                // Attempt to commit the transaction.
                                transaction.Commit();

                                // aggiorno id della lavorazione corrente 
                                SessionManager.EventoSelezionato.LavorazioneCorrente.Id = 0;
                            }
                            catch (Exception ex)
                            {
                                esito.Codice = Esito.ESITO_KO_ERRORE_UPDATE_TABELLA;
                                esito.Descrizione = "Agenda_DAL.cs - EliminaLavorazione" + Environment.NewLine + ex.Message;

                                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);

                                try
                                {
                                    transaction.Rollback();
                                }
                                catch (Exception ex2)
                                {
                                    esito.Descrizione += Environment.NewLine + "ERRORE ROLLBACK: " + ex2.Message;
                                    log.Error(ex2.Message + Environment.NewLine + ex2.StackTrace);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.Descrizione = "Agenda_DAL.cs - Lavorazione " + Environment.NewLine + ex.Message;

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
                esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.Descrizione = "Agenda_DAL.cs - CaricaElencoProduzioni " + Environment.NewLine + ex.Message;

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
                esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.Descrizione = "Agenda_DAL.cs - CaricaElencoLavorazioni " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return listaLavorazioni;
        }

        
    }
}