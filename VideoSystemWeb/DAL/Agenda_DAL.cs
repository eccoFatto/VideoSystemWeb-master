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

        public List<Tipologica> CaricaColonne(ref Esito esito)
        {
            return CaricaTipologica(EnumTipologiche.TIPO_COLONNE_AGENDA, true, ref esito);
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

        public List<int> getTenderImpiegatiInPeriodo(DateTime dataInizio, DateTime dataFine, ref Esito esito)
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
                                    " where a.data_inizio_impegno >= '" + dataDa + "' " +
                                    " and a.data_fine_impegno < '"+ dataA + "'";

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

        public Esito CreaEvento(DatiAgenda evento, List<string> listaIdTender)
        {
            Esito esito = new Esito();
            try
            {
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
                                CostruisciSP_InsertEvento(evento, StoreProc, sda);
                                StoreProc.Transaction = transaction;

                                int iReturn = StoreProc.ExecuteNonQuery();

                                // RECUPERO L'ID DELL'EVENTO AGENDA INSERITO
                                int iDatiAgendaReturn = Convert.ToInt32(StoreProc.Parameters["@id"].Value);


                                // SE E' ANDATO TUTTO BENE FACCIO INSERT DEI TENDER DELLA LISTA IN INPUT
                                if (listaIdTender != null)
                                {
                                    foreach (string idTender in listaIdTender)
                                    {
                                        CostruisciSP_InsertDatiTender(StoreProc, sda, iDatiAgendaReturn, idTender);
                                        StoreProc.ExecuteNonQuery();

                                        iReturn = Convert.ToInt32(StoreProc.Parameters["@id"].Value);
                                    }
                                }
                                // Attempt to commit the transaction.
                                transaction.Commit();

                            }
                            catch (Exception ex)
                            {
                                esito.codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                                esito.descrizione = "Agenda_DAL.cs - CreaEvento " + Environment.NewLine + ex.Message;

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
            }
            catch (Exception ex)
            {
                esito.codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.descrizione = "Agenda_DAL.cs - creaEvento " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return esito;
        }

        public Esito creaEventoConArticoli(DatiAgenda evento, List<DatiArticoli> listaDatiArticoli, List<string> listaIdTender)
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
                            CostruisciSP_InsertEvento(evento, StoreProc, sda);

                            StoreProc.Transaction = transaction;
                            StoreProc.ExecuteNonQuery();

                            // RECUPERO L'ID DELL'EVENTO AGENDA INSERITO
                            int iDatiAgendaReturn = Convert.ToInt32(StoreProc.Parameters["@id"].Value);

                            // SE E' ANDATO TUTTO BENE FACCIO INSERT DEGLI ARTICOLI EVENTO DELLA LISTA IN INPUT
                            if (listaDatiArticoli != null)
                            {
                                foreach (DatiArticoli datoArticolo in listaDatiArticoli)
                                {
                                    CostruisciSP_InsertDatiArticoli(StoreProc, sda, iDatiAgendaReturn, datoArticolo);
                                    StoreProc.ExecuteNonQuery();

                                    int iReturn = Convert.ToInt32(StoreProc.Parameters["@id"].Value);
                                }
                            }


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
                                    protocolloOfferta.PathDocumento = "";
                                    protocolloOfferta.Descrizione = evento.produzione.Trim() + " " + evento.lavorazione.Trim();
                                    protocolloOfferta.Attivo = true;

                                    CostruisciSP_InsertProtocollo(StoreProc, sda, iDatiAgendaReturn, protocolloOfferta);
                                    StoreProc.ExecuteNonQuery();

                                    int iReturn = Convert.ToInt32(StoreProc.Parameters["@id"].Value);
                                }
                            }

                            // Attempt to commit the transaction.
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            esito.codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                            esito.descrizione = "Agenda_DAL.cs - creaEventoConArticoli " + Environment.NewLine + ex.Message;

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
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;
                            // Start a local transaction.
                            StoreProc.Connection.Open();
                            transaction = con.BeginTransaction("UpdateAgendaTransaction");

                            Anag_Utenti utente = (Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE];

                            try
                            {
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
                                if (evento.id_tipologia == 0)
                                {
                                    id_tipologia.Value = null;
                                }
                                else
                                {
                                    id_tipologia.Value = evento.id_tipologia;
                                }
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

                                //StoreProc.Connection.Open();
                                StoreProc.Transaction = transaction;
                                int iReturn = StoreProc.ExecuteNonQuery();

                                // ELIMINO GLI EVENTUALI TENDER ASSOCIATI ALL'EVENTO PER SOSTITUIRLI COI NUOVI
                                CostruisciSP_DeleteDatiTender(StoreProc, sda, evento.id);
                                //StoreProc.Transaction = transaction;

                                StoreProc.ExecuteNonQuery();


                                // SE E' ANDATO TUTTO BENE FACCIO  INSERT DEI TENDER
                                if (listaIdTender != null)
                                {
                                    foreach (string idTender in listaIdTender)
                                    {
                                        CostruisciSP_InsertDatiTender(StoreProc, sda, evento.id, idTender);
                                        StoreProc.ExecuteNonQuery();

                                        //int iDatiArticoloReturn = Convert.ToInt32(StoreProc.Parameters["@id"].Value);
                                    }
                                }

                                // Attempt to commit the transaction.
                                transaction.Commit();

                            }
                            catch (Exception ex)
                            {
                                esito.codice = Esito.ESITO_KO_ERRORE_UPDATE_TABELLA;
                                esito.descrizione = "Agenda_DAL.cs - AggiornaEvento " + Environment.NewLine + ex.Message;

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
                esito.descrizione = "Agenda_DAL.cs - aggiornaEvento " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return esito;
        }

        public Esito AggiornaEventoConArticoli(DatiAgenda evento, List<DatiArticoli> listaDatiArticoli, List<string> listaIdTender)
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

                            try
                            {
                                CostruisciSP_UpdateEvento(evento, StoreProc, sda);
                                StoreProc.Transaction = transaction;
                                int iDatiAgendaReturn = StoreProc.ExecuteNonQuery();

                                // ELIMINO GLI EVENTUALI ARTICOLI ASSOCIATI ALL'EVENTO PER SOSTITUIRLI COI NUOVI
                                CostruisciSP_DeleteDatiArticoli(StoreProc, sda, evento.id);
                                StoreProc.ExecuteNonQuery();

                                // SE E' ANDATO TUTTO BENE FACCIO INSERT DEGLI ARTICOLI EVENTO DELLA LISTA IN INPUT
                                if (listaDatiArticoli != null)
                                {
                                    foreach (DatiArticoli datoArticolo in listaDatiArticoli)
                                    {
                                        CostruisciSP_InsertDatiArticoli(StoreProc, sda, evento.id, datoArticolo);
                                        StoreProc.ExecuteNonQuery();

                                        int iDatiArticoloReturn = Convert.ToInt32(StoreProc.Parameters["@id"].Value);
                                    }
                                }


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

                                        //int iDatiArticoloReturn = Convert.ToInt32(StoreProc.Parameters["@id"].Value);
                                    }
                                }

                                //GESTIONE PROTOCOLLO
                                int idTipoProtocollo = UtilityTipologiche.getElementByNome(UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_PROTOCOLLO), "offerta", ref esito).id;
                                if (Protocolli_BLL.Instance.getProtocolliByCodLavIdTipoProtocollo(evento.codice_lavoro, idTipoProtocollo,ref esito,true).Count == 0  
                                    &&  evento.id_stato == Stato.Instance.STATO_OFFERTA)
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
                                        protocolloOfferta.PathDocumento = "";
                                        protocolloOfferta.Descrizione = evento.produzione.Trim() + " " + evento.lavorazione.Trim();
                                        protocolloOfferta.Attivo = true;

                                        CostruisciSP_InsertProtocollo(StoreProc, sda, iDatiAgendaReturn, protocolloOfferta);
                                        StoreProc.ExecuteNonQuery();

                                        int iReturn = Convert.ToInt32(StoreProc.Parameters["@id"].Value);
                                    }
                                }

                                // Attempt to commit the transaction.
                                transaction.Commit();

                            }
                            catch (Exception ex)
                            {
                                esito.codice = Esito.ESITO_KO_ERRORE_UPDATE_TABELLA;
                                esito.descrizione = "Agenda_DAL.cs - AggiornaEventoConArticoli " + Environment.NewLine + ex.Message;

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
                                //else
                                //{
                                //    esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                //    esito.descrizione = "Nessun dato di produzione nella tabella tab_dati_agenda ";
                                //}
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
                                //else
                                //{
                                //    esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                //    esito.descrizione = "Nessun dato di lavorazione nella tabella tab_dati_agenda ";
                                //}
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

        //public string GetMaxCodiceLavorazione(string anno, ref Esito esito)
        //{
        //    string maxCodiceLavorazione = "";
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(sqlConstr))
        //        {
        //            using (SqlCommand cmd = new SqlCommand("SELECT MAX(codice_lavoro) FROM " + TABELLA_DATI_AGENDA + " WHERE codice_lavoro like '" + anno + "%'"))
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
        //                            maxCodiceLavorazione = dt.Rows[0].Field<string>(0);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        esito.codice = Esito.ESITO_KO_ERRORE_GENERICO;
        //        esito.descrizione = "Agenda_DAL.cs - GetMaxCodiceLavorazione " + Environment.NewLine + ex.Message;

        //        log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
        //    }

        //    return maxCodiceLavorazione;
        //}
    }
}