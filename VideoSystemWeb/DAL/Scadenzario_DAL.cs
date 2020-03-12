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
    public class Scadenzario_DAL : Base_DAL
    {
        //singleton
        private static volatile Scadenzario_DAL instance;
        private static object objForLock = new Object();
        private Scadenzario_DAL() { }
        public static Scadenzario_DAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Scadenzario_DAL();
                    }
                }
                return instance;
            }
        }

        public int CreaDatiScadenzario(DatiScadenzario scadenza, ref Esito esito)
        {
            try
            {
                Anag_Utenti utente = (Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE];
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("InsertDatiScadenzario"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
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

                            SqlParameter idDatiProtocollo = new SqlParameter("@idDatiProtocollo", scadenza.IdDatiProtocollo);
                            idDatiProtocollo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idDatiProtocollo);

                            SqlParameter banca = new SqlParameter("@banca", DBNull.Value);
                            if (scadenza.Banca != null) banca = new SqlParameter("@banca", scadenza.Banca);
                            banca.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(banca);

                            SqlParameter dataScadenza = new SqlParameter("@dataScadenza", scadenza.DataScadenza);
                            dataScadenza.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(dataScadenza);

                            SqlParameter importoDare = new SqlParameter("@importoDare", scadenza.ImportoDare);
                            importoDare.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(importoDare);

                            SqlParameter importoDareIva = new SqlParameter("@importoDareIva", scadenza.ImportoDareIva);
                            importoDareIva.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(importoDareIva);

                            SqlParameter importoVersato = new SqlParameter("@importoVersato", scadenza.ImportoVersato);
                            importoVersato.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(importoVersato);

                            SqlParameter dataVersamento = new SqlParameter("@dataVersamento", DBNull.Value);
                            if (scadenza.DataVersamento != null) dataVersamento = new SqlParameter("@dataVersamento", scadenza.DataVersamento);
                            dataVersamento.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(dataVersamento);

                            SqlParameter importoAvere = new SqlParameter("@importoAvere", scadenza.ImportoAvere);
                            importoAvere.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(importoAvere);

                            SqlParameter importoAvereIva = new SqlParameter("@importoAvereIva", scadenza.ImportoAvereIva);
                            importoAvereIva.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(importoAvereIva);

                            SqlParameter importoRiscosso = new SqlParameter("@importoRiscosso", scadenza.ImportoRiscosso);
                            importoRiscosso.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(importoRiscosso);

                            SqlParameter dataRiscossione = new SqlParameter("@dataRiscossione", DBNull.Value);
                            if (scadenza.DataRiscossione != null) dataRiscossione = new SqlParameter("@dataRiscossione", scadenza.DataRiscossione);
                            dataRiscossione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(dataRiscossione);

                            SqlParameter note = new SqlParameter("@note", DBNull.Value);
                            if (scadenza.Note != null) note = new SqlParameter("@note", scadenza.Note);
                            note.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(note);

                            SqlParameter iva = new SqlParameter("@iva", scadenza.Iva);
                            iva.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(iva);

                            StoreProc.Connection.Open();

                            StoreProc.ExecuteNonQuery();

                            int iReturn = Convert.ToInt32(StoreProc.Parameters["@id"].Value);


                            return iReturn;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.Descrizione = "Scadenzario_DAL.cs - CreaDatiScadenzario " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return 0;
        }

        public void AggiornaDatiScadenzario(DatiScadenzario scadenza, ref Esito esito)
        {
            try
            {
                Anag_Utenti utente = (Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE];
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("UpdateDatiScadenzario"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            // PARAMETRI PER LOG UTENTE
                            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
                            idUtente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idUtente);

                            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
                            nomeUtente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nomeUtente);
                            // FINE PARAMETRI PER LOG UTENTE

                            SqlParameter id = new SqlParameter("@id", scadenza.Id);
                            id.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id);

                            SqlParameter idDatiProtocollo = new SqlParameter("@idDatiProtocollo", scadenza.IdDatiProtocollo);
                            idDatiProtocollo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idDatiProtocollo);

                            SqlParameter banca = new SqlParameter("@banca", DBNull.Value);
                            if (scadenza.Banca != null) banca = new SqlParameter("@banca", scadenza.Banca);
                            banca.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(banca);

                            SqlParameter dataScadenza = new SqlParameter("@dataScadenza", scadenza.DataScadenza);
                            dataScadenza.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(dataScadenza);

                            SqlParameter importoDare = new SqlParameter("@importoDare", scadenza.ImportoDare);
                            importoDare.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(importoDare);

                            SqlParameter importoDareIva = new SqlParameter("@importoDareIva", scadenza.ImportoDareIva);
                            importoDareIva.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(importoDareIva);

                            SqlParameter importoVersato = new SqlParameter("@importoVersato", scadenza.ImportoVersato);
                            importoVersato.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(importoVersato);

                            SqlParameter dataVersamento = new SqlParameter("@dataVersamento", DBNull.Value);
                            if (scadenza.DataVersamento != null) dataVersamento = new SqlParameter("@dataVersamento", scadenza.DataVersamento);
                            dataVersamento.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(dataVersamento);

                            SqlParameter importoAvere = new SqlParameter("@importoAvere", scadenza.ImportoAvere);
                            importoAvere.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(importoAvere);

                            SqlParameter importoAvereIva = new SqlParameter("@importoAvereIva", scadenza.ImportoAvereIva);
                            importoAvereIva.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(importoAvereIva);

                            SqlParameter importoRiscosso = new SqlParameter("@importoRiscosso", scadenza.ImportoRiscosso);
                            importoRiscosso.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(importoRiscosso);

                            SqlParameter dataRiscossione = new SqlParameter("@dataRiscossione", DBNull.Value);
                            if (scadenza.DataRiscossione != null) dataRiscossione = new SqlParameter("@dataRiscossione", scadenza.DataRiscossione);
                            dataRiscossione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(dataRiscossione);

                            SqlParameter note = new SqlParameter("@note", DBNull.Value);
                            if (scadenza.Note != null) note = new SqlParameter("@note", scadenza.Note);
                            note.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(note);

                            SqlParameter iva = new SqlParameter("@iva", scadenza.Iva);
                            iva.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(iva);

                            StoreProc.Connection.Open();

                            StoreProc.ExecuteNonQuery();

                            //Convert.ToInt32(StoreProc.Parameters["@id"].Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.Descrizione = "Scadenzario_DAL.cs - AggiornaDatiScadenzario " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }
        }

        // USATO PER AGGIUNGERE UN PAGAMENTO AD UNA SCADENZA GIA' ESISTENTE E AGGIORNARE LA SCADENZA (IN TRANSAZIONE)
        public Esito AggiungiPagamento(DatiScadenzario scadenzaDaAggiornare, DatiScadenzario scadenzaDaInserire)
        {
            Esito esito = new Esito();
            using (SqlConnection con = new SqlConnection(sqlConstr))
            {
                using (SqlCommand StoreProc = new SqlCommand("UpdateDatiScadenzario"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        SqlTransaction transaction;
                        StoreProc.Connection = con;
                        StoreProc.Connection.Open();
                        // Start a local transaction.
                        transaction = con.BeginTransaction("ScadenzarioTransaction");

                        try
                        {
                            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

                            #region AGGIORNAMENTO SCADENZA ESISTENTE
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            // PARAMETRI PER LOG UTENTE
                            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
                            idUtente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idUtente);

                            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
                            nomeUtente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nomeUtente);
                            // FINE PARAMETRI PER LOG UTENTE

                            SqlParameter id = new SqlParameter("@id", scadenzaDaAggiornare.Id);
                            id.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id);

                            SqlParameter idDatiProtocollo = new SqlParameter("@idDatiProtocollo", scadenzaDaAggiornare.IdDatiProtocollo);
                            idDatiProtocollo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idDatiProtocollo);

                            SqlParameter banca = new SqlParameter("@banca", DBNull.Value);
                            if (scadenzaDaAggiornare.Banca != null) banca = new SqlParameter("@banca", scadenzaDaAggiornare.Banca);
                            banca.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(banca);

                            SqlParameter dataScadenza = new SqlParameter("@dataScadenza", scadenzaDaAggiornare.DataScadenza);
                            dataScadenza.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(dataScadenza);

                            SqlParameter importoDare = new SqlParameter("@importoDare", scadenzaDaAggiornare.ImportoDare);
                            importoDare.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(importoDare);

                            SqlParameter importoDareIva = new SqlParameter("@importoDareIva", scadenzaDaAggiornare.ImportoDareIva);
                            importoDareIva.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(importoDareIva);

                            SqlParameter importoVersato = new SqlParameter("@importoVersato", scadenzaDaAggiornare.ImportoVersato);
                            importoVersato.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(importoVersato);

                            SqlParameter dataVersamento = new SqlParameter("@dataVersamento", DBNull.Value);
                            if (scadenzaDaAggiornare.DataVersamento != null) dataVersamento = new SqlParameter("@dataVersamento", scadenzaDaAggiornare.DataVersamento);
                            dataVersamento.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(dataVersamento);

                            SqlParameter importoAvere = new SqlParameter("@importoAvere", scadenzaDaAggiornare.ImportoAvere);
                            importoAvere.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(importoAvere);

                            SqlParameter importoAvereIva = new SqlParameter("@importoAvereIva", scadenzaDaAggiornare.ImportoAvereIva);
                            importoAvereIva.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(importoAvereIva);

                            SqlParameter importoRiscosso = new SqlParameter("@importoRiscosso", scadenzaDaAggiornare.ImportoRiscosso);
                            importoRiscosso.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(importoRiscosso);

                            SqlParameter dataRiscossione = new SqlParameter("@dataRiscossione", DBNull.Value);
                            if (scadenzaDaAggiornare.DataRiscossione != null) dataRiscossione = new SqlParameter("@dataRiscossione", scadenzaDaAggiornare.DataRiscossione);
                            dataRiscossione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(dataRiscossione);

                            SqlParameter note = new SqlParameter("@note", DBNull.Value);
                            if (scadenzaDaAggiornare.Note != null) note = new SqlParameter("@note", scadenzaDaAggiornare.Note);
                            note.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(note);

                            SqlParameter iva = new SqlParameter("@iva", scadenzaDaAggiornare.Iva);
                            iva.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(iva);


                            StoreProc.Transaction = transaction;
                            StoreProc.ExecuteNonQuery();
                            #endregion

                            #region CREAZIONE NUOVA SCADENZA
                            StoreProc.CommandType = CommandType.StoredProcedure;
                            StoreProc.CommandText = "InsertDatiScadenzario";
                            StoreProc.Parameters.Clear();
                            sda.SelectCommand = StoreProc;

                            StoreProc.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

                            // PARAMETRI PER LOG UTENTE
                            idUtente = new SqlParameter("@idUtente", utente.id);
                            idUtente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idUtente);

                            nomeUtente = new SqlParameter("@nomeUtente", utente.username);
                            nomeUtente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nomeUtente);
                            // FINE PARAMETRI PER LOG UTENTE

                            idDatiProtocollo = new SqlParameter("@idDatiProtocollo", scadenzaDaInserire.IdDatiProtocollo);
                            idDatiProtocollo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idDatiProtocollo);

                            banca = new SqlParameter("@banca", DBNull.Value);
                            if (scadenzaDaInserire.Banca != null) banca = new SqlParameter("@banca", scadenzaDaInserire.Banca);
                            banca.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(banca);

                            dataScadenza = new SqlParameter("@dataScadenza", scadenzaDaInserire.DataScadenza);
                            dataScadenza.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(dataScadenza);

                            importoDare = new SqlParameter("@importoDare", scadenzaDaInserire.ImportoDare);
                            importoDare.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(importoDare);

                            importoDareIva = new SqlParameter("@importoDareIva", scadenzaDaInserire.ImportoDareIva);
                            importoDareIva.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(importoDareIva);

                            importoVersato = new SqlParameter("@importoVersato", scadenzaDaInserire.ImportoVersato);
                            importoVersato.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(importoVersato);

                            dataVersamento = new SqlParameter("@dataVersamento", DBNull.Value);
                            if (scadenzaDaInserire.DataVersamento != null) dataVersamento = new SqlParameter("@dataVersamento", scadenzaDaInserire.DataVersamento);
                            dataVersamento.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(dataVersamento);

                            importoAvere = new SqlParameter("@importoAvere", scadenzaDaInserire.ImportoAvere);
                            importoAvere.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(importoAvere);

                            importoAvereIva = new SqlParameter("@importoAvereIva", scadenzaDaInserire.ImportoAvereIva);
                            importoAvereIva.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(importoAvereIva);

                            importoRiscosso = new SqlParameter("@importoRiscosso", scadenzaDaInserire.ImportoRiscosso);
                            importoRiscosso.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(importoRiscosso);

                            dataRiscossione = new SqlParameter("@dataRiscossione", DBNull.Value);
                            if (scadenzaDaInserire.DataRiscossione != null) dataRiscossione = new SqlParameter("@dataRiscossione", scadenzaDaInserire.DataRiscossione);
                            dataRiscossione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(dataRiscossione);

                            note = new SqlParameter("@note", DBNull.Value);
                            if (scadenzaDaInserire.Note != null) note = new SqlParameter("@note", scadenzaDaInserire.Note);
                            note.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(note);

                            iva = new SqlParameter("@iva", scadenzaDaInserire.Iva);
                            iva.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(iva);

                            StoreProc.Transaction = transaction;
                            StoreProc.ExecuteNonQuery();
                            #endregion

                            // Attempt to commit the transaction.
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            esito.Codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                            esito.Descrizione = "Scadenzario_DAL.cs - AggiornaDatiScadenzario " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;

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


        public void DeleteDatiScadenzarioById(int idScadenza, ref Esito esito)
        {
            try
            {
                Anag_Utenti utente = (Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE];
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("DeleteDatiScadenzario"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            // PARAMETRI PER LOG UTENTE
                            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
                            idUtente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idUtente);

                            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
                            nomeUtente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nomeUtente);
                            // FINE PARAMETRI PER LOG UTENTE

                            SqlParameter parIdScadenza = new SqlParameter("@id", idScadenza);
                            parIdScadenza.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(parIdScadenza);

                            StoreProc.Connection.Open();

                            StoreProc.ExecuteNonQuery();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.Descrizione = "Scadenzario_DAL.cs - DeleteDatiScadenzarioById " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }
        }

        public DatiScadenzario GetDatiScadenzarioById(ref Esito esito, int id)
        {
            DatiScadenzario scadenza = new DatiScadenzario();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM dati_scadenzario a left join dati_protocollo b on a.idDatiProtocollo = b.id WHERE a.id =  " + id.ToString();
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
                                    scadenza.Id = dt.Rows[0].Field<int>("id");
                                    scadenza.IdDatiProtocollo = dt.Rows[0].Field<int>("idDatiProtocollo");
                                    scadenza.Banca = dt.Rows[0].Field<string>("banca");
                                    scadenza.DataScadenza = dt.Rows[0].Field<DateTime?>("dataScadenza");
                                    scadenza.ImportoDare = dt.Rows[0].Field<decimal>("importoDare");
                                    scadenza.ImportoDareIva = dt.Rows[0].Field<decimal>("importoDareIva");
                                    scadenza.ImportoVersato = dt.Rows[0].Field<decimal>("importoVersato");
                                    scadenza.DataVersamento = dt.Rows[0].Field<DateTime?>("dataVersamento");
                                    scadenza.ImportoAvere = dt.Rows[0].Field<decimal>("importoAvere");
                                    scadenza.ImportoAvereIva = dt.Rows[0].Field<decimal>("importoAvereIva");
                                    scadenza.ImportoRiscosso = dt.Rows[0].Field<decimal>("importoRiscosso");
                                    scadenza.DataRiscossione = dt.Rows[0].Field<DateTime?>("dataRiscossione");

                                    scadenza.Note = dt.Rows[0].Field<string>("note");

                                    scadenza.Iva = dt.Rows[0].Field<decimal>("iva");

                                    scadenza.RagioneSocialeClienteFornitore = dt.Rows[0].Field<string>("cliente");
                                    scadenza.ProtocolloRiferimento = dt.Rows[0].Field<string>("protocollo_riferimento");
                                    scadenza.DataProtocollo = dt.Rows[0].Field<DateTime?>("data_inizio_lavorazione");
                                    
                                    //scadenza.ImportoTotale = 0;
                                    scadenza.Cassa = 0;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.Descrizione = "Scadenzario_DAL.cs - GetDatiScadenzarioById " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return scadenza;

        }

        public List<DatiScadenzario> GetAllDatiScadenzario(string tipoAnagrafica, 
                                                           string ragioneSociale, 
                                                           string numeroFattura, 
                                                           string fatturaPagata,
                                                           string dataFatturaDa, 
                                                           string dataFatturaA,
                                                           string dataScadenzaDa,
                                                           string dataScadenzaA,
                                                           ref Esito esito)
        {
            List<DatiScadenzario> listaDatiScadenzario = new List<DatiScadenzario>();
            try
            {
                string filtriRicerca = string.Empty;
                if (!string.IsNullOrEmpty(tipoAnagrafica)) filtriRicerca += " and b.destinatario = '" + tipoAnagrafica + "'";
                //if (!string.IsNullOrEmpty(anagraficaClienteFornitore)) filtriRicerca += " and b.cliente = '" + anagraficaClienteFornitore.Trim() + "'";
                if (!string.IsNullOrEmpty(ragioneSociale)) filtriRicerca += " and b.cliente like '%" + ragioneSociale.Trim() + "%'";

                if (!string.IsNullOrEmpty(numeroFattura)) filtriRicerca += " and b.protocollo_riferimento = '" + numeroFattura + "'";
                if (!string.IsNullOrEmpty(fatturaPagata))
                {
                    if (fatturaPagata == "1")
                    { 
                        filtriRicerca += " and (a.importoAvere = a.importoRiscosso and a.importoDare = a.importoVersato)"; 
                    }
                    else
                    {
                        filtriRicerca += " and (a.importoAvere <> a.importoRiscosso or a.importoDare != a.importoVersato)";
                    }
                }
                if (!string.IsNullOrEmpty(dataFatturaDa)) filtriRicerca += " and b.data_protocollo >= '" + (DateTime.Parse(dataFatturaDa)).ToString("yyyy-MM-ddT00:00:00.000") + "'";
                if (!string.IsNullOrEmpty(dataFatturaA)) filtriRicerca += " and b.data_protocollo <= '" + (DateTime.Parse(dataFatturaA)).ToString("yyyy-MM-ddT00:00:00.000") + "'";
                if (!string.IsNullOrEmpty(dataScadenzaDa)) filtriRicerca += " and a.dataScadenza >= '" +  (DateTime.Parse(dataScadenzaDa)).ToString("yyyy-MM-ddT00:00:00.000") + "'";
                if (!string.IsNullOrEmpty(dataScadenzaA)) filtriRicerca += " and a.dataScadenza <= '" + (DateTime.Parse(dataScadenzaA)).ToString("yyyy-MM-ddT00:00:00.000") + "'";

                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM dati_scadenzario a left join dati_protocollo b on a.idDatiProtocollo = b.id where 1=1";
                    query += filtriRicerca;

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
                                        DatiScadenzario scadenza = new DatiScadenzario()
                                        {
                                            Id = riga.Field<int>("id"),
                                            IdDatiProtocollo = riga.Field<int>("idDatiProtocollo"),
                                            Banca = riga.Field<string>("banca"),
                                            DataScadenza = riga.Field<DateTime?>("dataScadenza"),
                                            ImportoDare = riga.Field<decimal>("importoDare"),
                                            ImportoDareIva = riga.Field<decimal>("importoDareIva"),
                                            ImportoVersato = riga.Field<decimal>("importoVersato"),
                                            DataVersamento = riga.Field<DateTime?>("dataVersamento"),
                                            ImportoAvere = riga.Field<decimal>("importoAvere"),
                                            ImportoAvereIva = riga.Field<decimal>("importoAvereIva"),
                                            ImportoRiscosso = riga.Field<decimal>("importoRiscosso"),
                                            DataRiscossione = riga.Field<DateTime?>("dataRiscossione"),

                                            Note = riga.Field<string>("note"),
                                            Iva = riga.Field<decimal>("iva"),

                                        RagioneSocialeClienteFornitore = riga.Field<string>("cliente"),
                                            ProtocolloRiferimento = riga.Field<string>("protocollo_riferimento"),
                                            DataProtocollo = riga.Field<DateTime?>("data_protocollo"),

                                            //ImportoTotale = 0,
                                            Cassa = 0
                                        };
                                        listaDatiScadenzario.Add(scadenza);
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
                esito.Descrizione = "Scadenzario_DAL.cs - getAllDatiScadenzario " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return listaDatiScadenzario;
        }

        public List<DatiScadenzario> GetDatiTotaliFatturaByIdDatiScadenzario(int idDatiScadenzario, ref Esito esito)
        {
            List<DatiScadenzario> listaDatiScadenzario = new List<DatiScadenzario>();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "select * from dati_scadenzario  a left join dati_protocollo b on a.idDatiProtocollo = b.id " +
                                   "where idDatiProtocollo = (select idDatiProtocollo from dati_scadenzario where id = " + idDatiScadenzario.ToString() +")";

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
                                        DatiScadenzario scadenza = new DatiScadenzario()
                                        {
                                            Id = riga.Field<int>("id"),
                                            IdDatiProtocollo = riga.Field<int>("idDatiProtocollo"),
                                            Banca = riga.Field<string>("banca"),
                                            DataScadenza = riga.Field<DateTime?>("dataScadenza"),
                                            ImportoDare = riga.Field<decimal>("importoDare"),
                                            ImportoDareIva = riga.Field<decimal>("importoDareIva"),
                                            ImportoVersato = riga.Field<decimal>("importoVersato"),
                                            DataVersamento = riga.Field<DateTime?>("dataVersamento"),
                                            ImportoAvere = riga.Field<decimal>("importoAvere"),
                                            ImportoAvereIva = riga.Field<decimal>("importoAvereIva"),
                                            ImportoRiscosso = riga.Field<decimal>("importoRiscosso"),
                                            DataRiscossione = riga.Field<DateTime?>("dataRiscossione"),

                                            Note = riga.Field<string>("note"),
                                            Iva = riga.Field<decimal>("iva"),

                                            RagioneSocialeClienteFornitore = riga.Field<string>("cliente"),
                                            ProtocolloRiferimento = riga.Field<string>("protocollo_riferimento"),
                                            DataProtocollo = riga.Field<DateTime?>("data_protocollo"),

                                            //ImportoTotale = 0,
                                            Cassa = 0
                                        };
                                        listaDatiScadenzario.Add(scadenza);
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
                esito.Descrizione = "Scadenzario_DAL.cs - GetDatiTotaliFatturaByIdDatiScadenzario " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return listaDatiScadenzario;
        }

        public List<DatiScadenzario> GetDatiScadenzarioByIdDatiProtocollo(int idDatiProtocollo, ref Esito esito)
        {
            List<DatiScadenzario> listaDatiScadenzario = new List<DatiScadenzario>();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "select * from dati_scadenzario " +
                                   "where idDatiProtocollo = " + idDatiProtocollo;

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
                                        DatiScadenzario scadenza = new DatiScadenzario()
                                        {
                                            Id = riga.Field<int>("id"),
                                            IdDatiProtocollo = riga.Field<int>("idDatiProtocollo"),
                                            Banca = riga.Field<string>("banca"),
                                            DataScadenza = riga.Field<DateTime?>("dataScadenza"),
                                            ImportoDare = riga.Field<decimal>("importoDare"),
                                            ImportoDareIva = riga.Field<decimal>("importoDareIva"),
                                            ImportoVersato = riga.Field<decimal>("importoVersato"),
                                            DataVersamento = riga.Field<DateTime?>("dataVersamento"),
                                            ImportoAvere = riga.Field<decimal>("importoAvere"),
                                            ImportoAvereIva = riga.Field<decimal>("importoAvereIva"),
                                            ImportoRiscosso = riga.Field<decimal>("importoRiscosso"),
                                            DataRiscossione = riga.Field<DateTime?>("dataRiscossione"),

                                            Note = riga.Field<string>("note"),
                                            Iva = riga.Field<decimal>("iva"),

                                            //RagioneSocialeClienteFornitore = riga.Field<string>("cliente"),
                                            //ProtocolloRiferimento = riga.Field<string>("protocollo_riferimento"),
                                            //DataProtocollo = riga.Field<DateTime?>("data_protocollo"),

                                            //ImportoTotale = 0,
                                            Cassa = 0
                                        };
                                        listaDatiScadenzario.Add(scadenza);
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
                esito.Descrizione = "Scadenzario_DAL.cs - GetDatiScadenzarioByIdDatiProtocollo " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return listaDatiScadenzario;
        }

        public List<Protocolli> getFattureNonInScadenzario(string tipo, ref Esito esito)
        {
            List<Protocolli> listaProtocolli = new List<Protocolli>();
            int idTipoFattura =  UtilityTipologiche.getElementByNome(UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_PROTOCOLLO), "Fattura", ref esito).id;
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "Select * from dati_protocollo where attivo = 1 and id_tipo_protocollo = " + idTipoFattura + " and id not in (select idDatiProtocollo from dati_scadenzario) and (pregresso = 0 or pregresso is NULL) AND destinatario = '" + tipo + "'";

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
                                        Protocolli protocollo = new Protocolli();
                                        protocollo.Id = riga.Field<int>("id");
                                        protocollo.Codice_lavoro = riga.Field<string>("codice_lavoro");
                                        protocollo.Numero_protocollo = riga.Field<string>("numero_protocollo");
                                        if (!DBNull.Value.Equals(riga["data_protocollo"])) protocollo.Data_protocollo = riga.Field<DateTime?>("data_protocollo");
                                        protocollo.Cliente = riga.Field<string>("cliente");
                                        if (!DBNull.Value.Equals(riga["id_cliente"])) protocollo.Id_cliente = riga.Field<int>("id_cliente");
                                        protocollo.Id_tipo_protocollo = riga.Field<int>("id_tipo_protocollo");
                                        protocollo.Protocollo_riferimento = riga.Field<string>("protocollo_riferimento");
                                        protocollo.PathDocumento = riga.Field<string>("pathDocumento");
                                        protocollo.Descrizione = riga.Field<string>("descrizione");
                                        protocollo.Lavorazione = riga.Field<string>("lavorazione");
                                        protocollo.Produzione = riga.Field<string>("produzione");
                                        if (!DBNull.Value.Equals(riga["data_inizio_lavorazione"])) protocollo.Data_inizio_lavorazione = riga.Field<DateTime>("data_inizio_lavorazione");
                                        protocollo.Attivo = riga.Field<bool>("attivo");
                                        protocollo.Pregresso = riga.Field<bool>("pregresso");
                                        protocollo.Destinatario = riga.Field<string>("destinatario");

                                        listaProtocolli.Add(protocollo);
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

            return listaProtocolli;
        }

        public List<Anag_Clienti_Fornitori> getClientiFornitoriInScadenzario(ref Esito esito)
        {
            List<Anag_Clienti_Fornitori> listaClientiFornitori = new List<Anag_Clienti_Fornitori>();
            int idTipoFattura = UtilityTipologiche.getElementByNome(UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_PROTOCOLLO), "Fattura", ref esito).id;
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "Select distinct cliente from dati_protocollo where id_tipo_protocollo = " + idTipoFattura + " and (pregresso = 0 or pregresso is NULL) order by cliente ";

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
                                        Anag_Clienti_Fornitori clienteFornitore = new Anag_Clienti_Fornitori();
                                        clienteFornitore.Id = 0;
                                        clienteFornitore.RagioneSociale = riga.Field<string>("cliente");

                                        listaClientiFornitori.Add(clienteFornitore);
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

            return listaClientiFornitori;
        }
    }
}