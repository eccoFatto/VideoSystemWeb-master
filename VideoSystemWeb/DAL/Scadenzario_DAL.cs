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
        #region SINGLETON
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
        #endregion

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

                            #region PARAMETRI PER LOG UTENTE
                            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
                            idUtente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idUtente);

                            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
                            nomeUtente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nomeUtente);
                            #endregion

                            SqlParameter idPadre = new SqlParameter("@idPadre", DBNull.Value); // idPadre viene usato solo quando si splitta una scadenza (AggiungiPagamento)
                            idPadre.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idPadre);

                            SqlParameter idDatiProtocollo = new SqlParameter("@idDatiProtocollo", scadenza.IdDatiProtocollo);
                            idDatiProtocollo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idDatiProtocollo);

                            SqlParameter idTipoBanca = new SqlParameter("@idTipoBanca", DBNull.Value);
                            if (scadenza.IdTipoBanca != null) idTipoBanca = new SqlParameter("@idTipoBanca", scadenza.IdTipoBanca);
                            idTipoBanca.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idTipoBanca);

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

                            SqlParameter importoVersatoIva = new SqlParameter("@importoVersatoIva", scadenza.ImportoVersatoIva);
                            importoVersatoIva.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(importoVersatoIva);

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

                            SqlParameter importoRiscossoIva = new SqlParameter("@importoRiscossoIva", scadenza.ImportoRiscossoIva);
                            importoRiscossoIva.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(importoRiscossoIva);

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

                            #region PARAMETRI PER LOG UTENTE
                            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
                            idUtente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idUtente);

                            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
                            nomeUtente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nomeUtente);
                            #endregion

                            SqlParameter id = new SqlParameter("@id", scadenza.Id);
                            id.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id);

                            SqlParameter idPadre = new SqlParameter("@idPadre", DBNull.Value);
                            if (scadenza.IdPadre != null) idPadre = new SqlParameter("@idPadre", scadenza.IdPadre);
                            idPadre.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idPadre);

                            SqlParameter idDatiProtocollo = new SqlParameter("@idDatiProtocollo", scadenza.IdDatiProtocollo);
                            idDatiProtocollo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idDatiProtocollo);

                            SqlParameter idTipoBanca = new SqlParameter("@idTipoBanca", DBNull.Value);
                            if (scadenza.IdTipoBanca != null) idTipoBanca = new SqlParameter("@idTipoBanca", scadenza.IdTipoBanca);
                            idTipoBanca.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idTipoBanca);

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

                            SqlParameter importoVersatoIva = new SqlParameter("@importoVersatoIva", scadenza.ImportoVersatoIva);
                            importoVersatoIva.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(importoVersatoIva);

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

                            SqlParameter importoRiscossoIva = new SqlParameter("@importoRiscossoIva", scadenza.ImportoRiscossoIva);
                            importoRiscossoIva.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(importoRiscossoIva);

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

                            #region PARAMETRI PER LOG UTENTE
                            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
                            idUtente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idUtente);

                            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
                            nomeUtente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nomeUtente);
                            #endregion

                            SqlParameter id = new SqlParameter("@id", scadenzaDaAggiornare.Id);
                            id.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id);

                            SqlParameter idPadre = new SqlParameter("@idPadre", DBNull.Value);
                            if (scadenzaDaAggiornare.IdPadre != null) idPadre = new SqlParameter("@idPadre", scadenzaDaAggiornare.IdPadre);
                            idPadre.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idPadre);

                            SqlParameter idDatiProtocollo = new SqlParameter("@idDatiProtocollo", scadenzaDaAggiornare.IdDatiProtocollo);
                            idDatiProtocollo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idDatiProtocollo);

                            SqlParameter idTipoBanca = new SqlParameter("@idTipoBanca", DBNull.Value);
                            if (scadenzaDaAggiornare.IdTipoBanca != null) idTipoBanca = new SqlParameter("@idTipoBanca", scadenzaDaAggiornare.IdTipoBanca);
                            idTipoBanca.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idTipoBanca);

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

                            SqlParameter importoVersatoIva = new SqlParameter("@importoVersatoIva", scadenzaDaAggiornare.ImportoVersatoIva);
                            importoVersatoIva.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(importoVersatoIva);

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

                            SqlParameter importoRiscossoIva = new SqlParameter("@importoRiscossoIva", scadenzaDaAggiornare.ImportoRiscossoIva);
                            importoRiscossoIva.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(importoRiscossoIva);

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

                            #region PARAMETRI PER LOG UTENTE
                            idUtente = new SqlParameter("@idUtente", utente.id);
                            idUtente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idUtente);

                            nomeUtente = new SqlParameter("@nomeUtente", utente.username);
                            nomeUtente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nomeUtente);
                            #endregion

                            idPadre = new SqlParameter("@idPadre", scadenzaDaInserire.IdPadre);
                            idPadre.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idPadre);

                            idDatiProtocollo = new SqlParameter("@idDatiProtocollo", scadenzaDaInserire.IdDatiProtocollo);
                            idDatiProtocollo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idDatiProtocollo);

                            idTipoBanca = new SqlParameter("@idTipoBanca", DBNull.Value);
                            if (scadenzaDaInserire.IdTipoBanca != null) idTipoBanca = new SqlParameter("@idTipoBanca", scadenzaDaInserire.IdTipoBanca);
                            idTipoBanca.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idTipoBanca);

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

                            importoVersatoIva = new SqlParameter("@importoVersatoIva", scadenzaDaInserire.ImportoVersatoIva);
                            importoVersatoIva.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(importoVersatoIva);

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

                            importoRiscossoIva = new SqlParameter("@importoRiscossoIva", scadenzaDaInserire.ImportoRiscossoIva);
                            importoRiscossoIva.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(importoRiscossoIva);

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
                            esito.Descrizione = "Scadenzario_DAL.cs - AggiungiPagamento " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;

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

                            #region PARAMETRI PER LOG UTENTE
                            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
                            idUtente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idUtente);

                            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
                            nomeUtente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nomeUtente);
                            #endregion

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

        public void DeleteFigliScadenza(List<DatiScadenzario> listaFigliScadenza, ref Esito esito)
        {
            string elencoIdFigli = string.Empty;
            foreach (DatiScadenzario scadenza in listaFigliScadenza)
            {
                elencoIdFigli += scadenza.Id + ", ";
            }
            elencoIdFigli = elencoIdFigli.Substring(0, elencoIdFigli.Length - 2);

            using (SqlConnection con = new SqlConnection(sqlConstr))
            {
                using (SqlCommand sql = new SqlCommand("Delete from dati_scadenzario where id in (" + elencoIdFigli + ")"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        SqlTransaction transaction;
                        sql.Connection = con;
                        sql.Connection.Open();
                        // Start a local transaction.
                        transaction = con.BeginTransaction("ScadenzarioTransaction");

                        try
                        {
                            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

                            #region CANCELLAZIONE SCADENZE
                            sql.Connection = con;
                            sda.SelectCommand = sql;

                            sql.Transaction = transaction;
                            sql.ExecuteNonQuery();
                            #endregion

                            #region SCRITTURA LOG UTENTE
                            sql.CommandType = CommandType.StoredProcedure;
                            sql.CommandText = "InsertLog_utenti";
                            sql.Parameters.Clear();
                            sda.SelectCommand = sql;

                            #region PARAMETRI PER LOG UTENTE
                            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
                            idUtente.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(idUtente);

                            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
                            nomeUtente.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(nomeUtente);

                            SqlParameter nomeTabella = new SqlParameter("@nomeTabella", "dati_scadenzario");
                            nomeTabella.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(nomeTabella);

                            SqlParameter tipoOperazione = new SqlParameter("@tipoOperazione", "DELETE");
                            tipoOperazione.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(tipoOperazione);

                            SqlParameter idTabella = new SqlParameter("@idTabella", listaFigliScadenza[0].Id);
                            idTabella.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(idTabella);

                            sql.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                            #endregion

                            sql.Transaction = transaction;
                            sql.ExecuteNonQuery();
                            #endregion

                            // Attempt to commit the transaction.
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            esito.Codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                            esito.Descrizione = "Scadenzario_DAL.cs - DeleteFigliScadenza " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
                        }

                    }
                }
            }
                 
        }

        public DatiScadenzario GetDatiScadenzarioById(ref Esito esito, int id)
        {
            DatiScadenzario scadenza = null;
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
                                    scadenza = new DatiScadenzario(); 

                                    scadenza.Id = dt.Rows[0].Field<int>("id");
                                    scadenza.IdPadre = dt.Rows[0].Field<int?>("idPadre");
                                    scadenza.IdDatiProtocollo = dt.Rows[0].Field<int>("idDatiProtocollo");
                                    scadenza.IdTipoBanca = dt.Rows[0].Field<int>("idTipoBanca");
                                    scadenza.DataScadenza = dt.Rows[0].Field<DateTime?>("dataScadenza");
                                    scadenza.ImportoDare = dt.Rows[0].Field<decimal>("importoDare");
                                    scadenza.ImportoDareIva = dt.Rows[0].Field<decimal>("importoDareIva");
                                    scadenza.ImportoVersato = dt.Rows[0].Field<decimal>("importoVersato");
                                    scadenza.ImportoVersatoIva = dt.Rows[0].Field<decimal>("importoVersatoIva");
                                    scadenza.DataVersamento = dt.Rows[0].Field<DateTime?>("dataVersamento");
                                    scadenza.ImportoAvere = dt.Rows[0].Field<decimal>("importoAvere");
                                    scadenza.ImportoAvereIva = dt.Rows[0].Field<decimal>("importoAvereIva");
                                    scadenza.ImportoRiscosso = dt.Rows[0].Field<decimal>("importoRiscosso");
                                    scadenza.ImportoRiscossoIva = dt.Rows[0].Field<decimal>("importoRiscossoIva");
                                    scadenza.DataRiscossione = dt.Rows[0].Field<DateTime?>("dataRiscossione");
                                    scadenza.Note = dt.Rows[0].Field<string>("note");
                                    scadenza.Iva = dt.Rows[0].Field<decimal>("iva");
                                    scadenza.RagioneSocialeClienteFornitore = dt.Rows[0].Field<string>("cliente");
                                    scadenza.ProtocolloRiferimento = dt.Rows[0].Field<string>("protocollo_riferimento");
                                    scadenza.DataFattura = dt.Rows[0].Field<DateTime?>("data_inizio_lavorazione");
                                    scadenza.DataProtocollo = dt.Rows[0].Field<DateTime?>("data_protocollo");
                                    
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
                                                           string filtroBanca,
                                                           ref Esito esito)
        {
            List<DatiScadenzario> listaDatiScadenzario = new List<DatiScadenzario>();
            try
            {
                string filtriRicerca = string.Empty;
                filtriRicerca += string.IsNullOrWhiteSpace(tipoAnagrafica) ? "" : " and b.destinatario = '" + tipoAnagrafica + "'";
                filtriRicerca += string.IsNullOrWhiteSpace(ragioneSociale) ? "" : " and b.cliente like '%" + ragioneSociale.Trim() + "%'";
                filtriRicerca += string.IsNullOrWhiteSpace(numeroFattura) ? "" : " and b.protocollo_riferimento like '%" + numeroFattura + "%'";

                if (!string.IsNullOrEmpty(fatturaPagata))
                {
                    filtriRicerca += fatturaPagata == "1" ? " and (a.importoAvere = a.importoRiscosso and a.importoDare = a.importoVersato)" : " and (a.importoAvere <> a.importoRiscosso or a.importoDare != a.importoVersato)";
                }

                filtriRicerca += string.IsNullOrWhiteSpace(dataFatturaDa) ? "" : " and b.data_inizio_lavorazione >= '" + (DateTime.Parse(dataFatturaDa)).ToString("yyyy-MM-ddT00:00:00.000") + "'";
                filtriRicerca += string.IsNullOrWhiteSpace(dataFatturaA) ? "" : " and b.data_inizio_lavorazione < '" + (DateTime.Parse(dataFatturaA)).ToString("yyyy-MM-ddT23:59:59.999") + "'";
                filtriRicerca += string.IsNullOrWhiteSpace(dataScadenzaDa) ? "" : " and a.dataScadenza >= '" + (DateTime.Parse(dataScadenzaDa)).ToString("yyyy-MM-ddT00:00:00.000") + "'";
                filtriRicerca += string.IsNullOrWhiteSpace(dataScadenzaA) ? "" : " and a.dataScadenza < '" + (DateTime.Parse(dataScadenzaA)).ToString("yyyy-MM-ddT23:59:59.999") + "'";
                filtriRicerca += string.IsNullOrWhiteSpace(filtroBanca) ? "" : " and a.idTipoBanca = " + filtroBanca;

                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM dati_scadenzario a left join dati_protocollo b on a.idDatiProtocollo = b.id where 1=1 ";
                    query += filtriRicerca;
                    query += " order by a.id desc";

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
                                            IdPadre = riga.Field<int?>("idPadre"),
                                            IdDatiProtocollo = riga.Field<int>("idDatiProtocollo"),
                                            IdTipoBanca = riga.Field<int>("idTipoBanca"),
                                            DataScadenza = riga.Field<DateTime?>("dataScadenza"),
                                            ImportoDare = riga.Field<decimal>("importoDare"),
                                            ImportoDareIva = riga.Field<decimal>("importoDareIva"),
                                            ImportoVersato = riga.Field<decimal>("importoVersato"),
                                            ImportoVersatoIva = riga.Field<decimal>("importoVersatoIva"),
                                            DataVersamento = riga.Field<DateTime?>("dataVersamento"),
                                            ImportoAvere = riga.Field<decimal>("importoAvere"),
                                            ImportoAvereIva = riga.Field<decimal>("importoAvereIva"),
                                            ImportoRiscosso = riga.Field<decimal>("importoRiscosso"),
                                            ImportoRiscossoIva = riga.Field<decimal>("importoRiscossoIva"),
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
                                            IdPadre = riga.Field<int?>("idPadre"),
                                            IdDatiProtocollo = riga.Field<int>("idDatiProtocollo"),
                                            IdTipoBanca = riga.Field<int>("idTipoBanca"),
                                            DataScadenza = riga.Field<DateTime?>("dataScadenza"),
                                            ImportoDare = riga.Field<decimal>("importoDare"),
                                            ImportoDareIva = riga.Field<decimal>("importoDareIva"),
                                            ImportoVersato = riga.Field<decimal>("importoVersato"),
                                            ImportoVersatoIva = riga.Field<decimal>("importoVersatoIva"),
                                            DataVersamento = riga.Field<DateTime?>("dataVersamento"),
                                            ImportoAvere = riga.Field<decimal>("importoAvere"),
                                            ImportoAvereIva = riga.Field<decimal>("importoAvereIva"),
                                            ImportoRiscosso = riga.Field<decimal>("importoRiscosso"),
                                            ImportoRiscossoIva = riga.Field<decimal>("importoRiscossoIva"),
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
                                            IdPadre = riga.Field<int?>("idPadre"),
                                            IdDatiProtocollo = riga.Field<int>("idDatiProtocollo"),
                                            IdTipoBanca = riga.Field<int>("idTipoBanca"),
                                            DataScadenza = riga.Field<DateTime?>("dataScadenza"),
                                            ImportoDare = riga.Field<decimal>("importoDare"),
                                            ImportoDareIva = riga.Field<decimal>("importoDareIva"),
                                            ImportoVersato = riga.Field<decimal>("importoVersato"),
                                            ImportoVersatoIva = riga.Field<decimal>("importoVersatoIva"),
                                            DataVersamento = riga.Field<DateTime?>("dataVersamento"),
                                            ImportoAvere = riga.Field<decimal>("importoAvere"),
                                            ImportoAvereIva = riga.Field<decimal>("importoAvereIva"),
                                            ImportoRiscosso = riga.Field<decimal>("importoRiscosso"),
                                            ImportoRiscossoIva = riga.Field<decimal>("importoRiscossoIva"),
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

        public List<Protocolli> GetFattureNonInScadenzario(string tipo, ref Esito esito)
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

        public List<Anag_Clienti_Fornitori> GetClientiFornitoriInScadenzario(ref Esito esito)
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

        public int CancellaFigli_CreaDatiScadenzario(List<DatiScadenzario> listaFigliScadenza, DatiScadenzario scadenza, ref Esito esito)
        {
            string elencoIdFigli = string.Empty;
            foreach (DatiScadenzario scadenzaFiglio in listaFigliScadenza)
            {
                elencoIdFigli += scadenzaFiglio.Id + ", ";
            }
            elencoIdFigli = elencoIdFigli.Substring(0, elencoIdFigli.Length - 2);

            using (SqlConnection con = new SqlConnection(sqlConstr))
            {
                using (SqlCommand sql = new SqlCommand("Delete from dati_scadenzario where id in (" + elencoIdFigli + ")"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        SqlTransaction transaction;
                        sql.Connection = con;
                        sql.Connection.Open();
                        // Start a local transaction.
                        transaction = con.BeginTransaction("ScadenzarioTransaction");

                        try
                        {
                            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

                            #region CANCELLAZIONE SCADENZE
                            //sql.Connection = con;
                            sda.SelectCommand = sql;

                            sql.Transaction = transaction;
                            sql.ExecuteNonQuery();
                            #endregion

                            #region SCRITTURA LOG UTENTE CANCELLAZIONE FIGLI
                            sql.CommandType = CommandType.StoredProcedure;
                            sql.CommandText = "InsertLog_utenti";
                            sql.Parameters.Clear();
                            sda.SelectCommand = sql;

                            #region PARAMETRI PER LOG UTENTE
                            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
                            idUtente.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(idUtente);

                            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
                            nomeUtente.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(nomeUtente);

                            SqlParameter nomeTabella = new SqlParameter("@nomeTabella", "dati_scadenzario");
                            nomeTabella.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(nomeTabella);

                            SqlParameter tipoOperazione = new SqlParameter("@tipoOperazione", "DELETE");
                            tipoOperazione.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(tipoOperazione);

                            SqlParameter idTabella = new SqlParameter("@idTabella", listaFigliScadenza[0].Id);
                            idTabella.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(idTabella);

                            sql.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                            #endregion

                            sql.Transaction = transaction;
                            sql.ExecuteNonQuery();
                            #endregion

                            #region CREAZIONE NUOVA SCADENZA
                            sql.CommandType = CommandType.StoredProcedure;
                            sql.CommandText = "InsertDatiScadenzario";
                            sql.Parameters.Clear();
                            sda.SelectCommand = sql;

                            sql.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

                            #region PARAMETRI PER LOG UTENTE
                            idUtente = new SqlParameter("@idUtente", utente.id);
                            idUtente.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(idUtente);

                            nomeUtente = new SqlParameter("@nomeUtente", utente.username);
                            nomeUtente.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(nomeUtente);
                            #endregion

                            SqlParameter idPadre = new SqlParameter("@idPadre", DBNull.Value); // idPadre viene usato solo quando si splitta una scadenza (AggiungiPagamento)
                            idPadre.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(idPadre);

                            SqlParameter idDatiProtocollo = new SqlParameter("@idDatiProtocollo", scadenza.IdDatiProtocollo);
                            idDatiProtocollo.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(idDatiProtocollo);

                            SqlParameter idTipoBanca = new SqlParameter("@idTipoBanca", DBNull.Value);
                            if (scadenza.IdTipoBanca != null) idTipoBanca = new SqlParameter("@idTipoBanca", scadenza.IdTipoBanca);
                            idTipoBanca.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(idTipoBanca);

                            SqlParameter dataScadenza = new SqlParameter("@dataScadenza", scadenza.DataScadenza);
                            dataScadenza.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(dataScadenza);

                            SqlParameter importoDare = new SqlParameter("@importoDare", scadenza.ImportoDare);
                            importoDare.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(importoDare);

                            SqlParameter importoDareIva = new SqlParameter("@importoDareIva", scadenza.ImportoDareIva);
                            importoDareIva.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(importoDareIva);

                            SqlParameter importoVersato = new SqlParameter("@importoVersato", scadenza.ImportoVersato);
                            importoVersato.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(importoVersato);

                            SqlParameter importoVersatoIva = new SqlParameter("@importoVersatoIva", scadenza.ImportoVersatoIva);
                            importoVersatoIva.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(importoVersatoIva);

                            SqlParameter dataVersamento = new SqlParameter("@dataVersamento", DBNull.Value);
                            if (scadenza.DataVersamento != null) dataVersamento = new SqlParameter("@dataVersamento", scadenza.DataVersamento);
                            dataVersamento.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(dataVersamento);

                            SqlParameter importoAvere = new SqlParameter("@importoAvere", scadenza.ImportoAvere);
                            importoAvere.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(importoAvere);

                            SqlParameter importoAvereIva = new SqlParameter("@importoAvereIva", scadenza.ImportoAvereIva);
                            importoAvereIva.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(importoAvereIva);

                            SqlParameter importoRiscosso = new SqlParameter("@importoRiscosso", scadenza.ImportoRiscosso);
                            importoRiscosso.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(importoRiscosso);

                            SqlParameter importoRiscossoIva = new SqlParameter("@importoRiscossoIva", scadenza.ImportoRiscossoIva);
                            importoRiscossoIva.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(importoRiscossoIva);

                            SqlParameter dataRiscossione = new SqlParameter("@dataRiscossione", DBNull.Value);
                            if (scadenza.DataRiscossione != null) dataRiscossione = new SqlParameter("@dataRiscossione", scadenza.DataRiscossione);
                            dataRiscossione.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(dataRiscossione);

                            SqlParameter note = new SqlParameter("@note", DBNull.Value);
                            if (scadenza.Note != null) note = new SqlParameter("@note", scadenza.Note);
                            note.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(note);

                            SqlParameter iva = new SqlParameter("@iva", scadenza.Iva);
                            iva.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(iva);

                            sql.Transaction = transaction;
                            sql.ExecuteNonQuery();

                            int iReturn = Convert.ToInt32(sql.Parameters["@id"].Value);
                            #endregion

                            // Attempt to commit the transaction.
                            transaction.Commit();

                            return iReturn;
                        }
                        catch (Exception ex)
                        {
                            esito.Codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                            esito.Descrizione = "Scadenzario_DAL.cs - CancellaFigli_CreaDatiScadenzario " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
                        }
                    }
                }
            }

            return 0;
        }

        public void CancellaFigli_AggiornaDatiScadenzario(List<DatiScadenzario> listaFigliScadenza, DatiScadenzario scadenza, ref Esito esito)
        {
            string elencoIdFigli = string.Empty;
            foreach (DatiScadenzario scadenzaFiglio in listaFigliScadenza)
            {
                elencoIdFigli += scadenzaFiglio.Id + ", ";
            }
            elencoIdFigli = elencoIdFigli.Substring(0, elencoIdFigli.Length - 2);

            using (SqlConnection con = new SqlConnection(sqlConstr))
            {
                using (SqlCommand sql = new SqlCommand("Delete from dati_scadenzario where id in (" + elencoIdFigli + ")"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        SqlTransaction transaction;
                        sql.Connection = con;
                        sql.Connection.Open();
                        // Start a local transaction.
                        transaction = con.BeginTransaction("ScadenzarioTransaction");

                        try
                        {
                            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

                            #region CANCELLAZIONE SCADENZE
                            //sql.Connection = con;
                            sda.SelectCommand = sql;

                            sql.Transaction = transaction;
                            sql.ExecuteNonQuery();
                            #endregion

                            #region SCRITTURA LOG UTENTE CANCELLAZIONE FIGLI
                            sql.CommandType = CommandType.StoredProcedure;
                            sql.CommandText = "InsertLog_utenti";
                            sql.Parameters.Clear();
                            sda.SelectCommand = sql;

                            #region PARAMETRI PER LOG UTENTE
                            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
                            idUtente.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(idUtente);

                            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
                            nomeUtente.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(nomeUtente);

                            SqlParameter nomeTabella = new SqlParameter("@nomeTabella", "dati_scadenzario");
                            nomeTabella.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(nomeTabella);

                            SqlParameter tipoOperazione = new SqlParameter("@tipoOperazione", "DELETE");
                            tipoOperazione.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(tipoOperazione);

                            SqlParameter idTabella = new SqlParameter("@idTabella", listaFigliScadenza[0].Id);
                            idTabella.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(idTabella);

                            sql.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                            #endregion

                            sql.Transaction = transaction;
                            sql.ExecuteNonQuery();
                            #endregion

                            #region MODIFICA SCADENZA
                            sql.CommandType = CommandType.StoredProcedure;
                            sql.CommandText = "UpdateDatiScadenzario";
                            sql.Parameters.Clear();
                            sda.SelectCommand = sql;

                            //sql.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

                            #region PARAMETRI PER LOG UTENTE
                            idUtente = new SqlParameter("@idUtente", utente.id);
                            idUtente.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(idUtente);

                            nomeUtente = new SqlParameter("@nomeUtente", utente.username);
                            nomeUtente.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(nomeUtente);
                            #endregion

                            SqlParameter id = new SqlParameter("@id", scadenza.Id);
                            id.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(id);

                            SqlParameter idPadre = new SqlParameter("@idPadre", DBNull.Value);
                            if (scadenza.IdPadre != null) idPadre = new SqlParameter("@idPadre", scadenza.IdPadre);
                            idPadre.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(idPadre);

                            SqlParameter idDatiProtocollo = new SqlParameter("@idDatiProtocollo", scadenza.IdDatiProtocollo);
                            idDatiProtocollo.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(idDatiProtocollo);

                            SqlParameter idTipoBanca = new SqlParameter("@idTipoBanca", DBNull.Value);
                            if (scadenza.IdTipoBanca != null) idTipoBanca = new SqlParameter("@idTipoBanca", scadenza.IdTipoBanca);
                            idTipoBanca.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(idTipoBanca);

                            SqlParameter dataScadenza = new SqlParameter("@dataScadenza", scadenza.DataScadenza);
                            dataScadenza.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(dataScadenza);

                            SqlParameter importoDare = new SqlParameter("@importoDare", scadenza.ImportoDare);
                            importoDare.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(importoDare);

                            SqlParameter importoDareIva = new SqlParameter("@importoDareIva", scadenza.ImportoDareIva);
                            importoDareIva.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(importoDareIva);

                            SqlParameter importoVersato = new SqlParameter("@importoVersato", scadenza.ImportoVersato);
                            importoVersato.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(importoVersato);

                            SqlParameter importoVersatoIva = new SqlParameter("@importoVersatoIva", scadenza.ImportoVersatoIva);
                            importoVersatoIva.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(importoVersatoIva);

                            SqlParameter dataVersamento = new SqlParameter("@dataVersamento", DBNull.Value);
                            if (scadenza.DataVersamento != null) dataVersamento = new SqlParameter("@dataVersamento", scadenza.DataVersamento);
                            dataVersamento.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(dataVersamento);

                            SqlParameter importoAvere = new SqlParameter("@importoAvere", scadenza.ImportoAvere);
                            importoAvere.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(importoAvere);

                            SqlParameter importoAvereIva = new SqlParameter("@importoAvereIva", scadenza.ImportoAvereIva);
                            importoAvereIva.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(importoAvereIva);

                            SqlParameter importoRiscosso = new SqlParameter("@importoRiscosso", scadenza.ImportoRiscosso);
                            importoRiscosso.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(importoRiscosso);

                            SqlParameter importoRiscossoIva = new SqlParameter("@importoRiscossoIva", scadenza.ImportoRiscossoIva);
                            importoRiscossoIva.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(importoRiscossoIva);

                            SqlParameter dataRiscossione = new SqlParameter("@dataRiscossione", DBNull.Value);
                            if (scadenza.DataRiscossione != null) dataRiscossione = new SqlParameter("@dataRiscossione", scadenza.DataRiscossione);
                            dataRiscossione.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(dataRiscossione);

                            SqlParameter note = new SqlParameter("@note", DBNull.Value);
                            if (scadenza.Note != null) note = new SqlParameter("@note", scadenza.Note);
                            note.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(note);

                            SqlParameter iva = new SqlParameter("@iva", scadenza.Iva);
                            iva.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(iva);

                            sql.Transaction = transaction;
                            sql.ExecuteNonQuery();

                            int iReturn = Convert.ToInt32(sql.Parameters["@id"].Value);
                            #endregion

                            // Attempt to commit the transaction.
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            esito.Codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                            esito.Descrizione = "Scadenzario_DAL.cs - CancellaFigli_AggiornaDatiScadenzario " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
                        }
                    }
                }
            }
        }

        public void CancellaFigli_AggiungiPagamento(List<DatiScadenzario> listaFigliScadenza, DatiScadenzario scadenzaDaAggiornare, DatiScadenzario scadenzaDaInserire, ref Esito esito)
        {
            string elencoIdFigli = string.Empty;
            foreach (DatiScadenzario scadenzaFiglio in listaFigliScadenza)
            {
                elencoIdFigli += scadenzaFiglio.Id + ", ";
            }
            elencoIdFigli = elencoIdFigli.Substring(0, elencoIdFigli.Length - 2);

            using (SqlConnection con = new SqlConnection(sqlConstr))
            {
                using (SqlCommand sql = new SqlCommand("Delete from dati_scadenzario where id in (" + elencoIdFigli + ")"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        SqlTransaction transaction;
                        sql.Connection = con;
                        sql.Connection.Open();
                        // Start a local transaction.
                        transaction = con.BeginTransaction("ScadenzarioTransaction");

                        try
                        {
                            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

                            #region CANCELLAZIONE SCADENZE
                            //sql.Connection = con;
                            sda.SelectCommand = sql;

                            sql.Transaction = transaction;
                            sql.ExecuteNonQuery();
                            #endregion

                            #region SCRITTURA LOG UTENTE CANCELLAZIONE FIGLI
                            sql.CommandType = CommandType.StoredProcedure;
                            sql.CommandText = "InsertLog_utenti";
                            sql.Parameters.Clear();
                            sda.SelectCommand = sql;

                            #region PARAMETRI PER LOG UTENTE
                            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
                            idUtente.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(idUtente);

                            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
                            nomeUtente.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(nomeUtente);

                            SqlParameter nomeTabella = new SqlParameter("@nomeTabella", "dati_scadenzario");
                            nomeTabella.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(nomeTabella);

                            SqlParameter tipoOperazione = new SqlParameter("@tipoOperazione", "DELETE");
                            tipoOperazione.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(tipoOperazione);

                            SqlParameter idTabella = new SqlParameter("@idTabella", listaFigliScadenza[0].Id);
                            idTabella.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(idTabella);

                            sql.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;
                            #endregion

                            sql.Transaction = transaction;
                            sql.ExecuteNonQuery();
                            #endregion

                            #region AGGIORNAMENTO SCADENZA ESISTENTE
                            sql.CommandType = CommandType.StoredProcedure;
                            sql.CommandText = "UpdateDatiScadenzario";
                            sql.Parameters.Clear();
                            sda.SelectCommand = sql;

                            #region PARAMETRI PER LOG UTENTE
                            idUtente = new SqlParameter("@idUtente", utente.id);
                            idUtente.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(idUtente);

                            nomeUtente = new SqlParameter("@nomeUtente", utente.username);
                            nomeUtente.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(nomeUtente);
                            #endregion

                            SqlParameter id = new SqlParameter("@id", scadenzaDaAggiornare.Id);
                            id.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(id);

                            SqlParameter idPadre = new SqlParameter("@idPadre", DBNull.Value);
                            if (scadenzaDaAggiornare.IdPadre != null) idPadre = new SqlParameter("@idPadre", scadenzaDaAggiornare.IdPadre);
                            idPadre.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(idPadre);

                            SqlParameter idDatiProtocollo = new SqlParameter("@idDatiProtocollo", scadenzaDaAggiornare.IdDatiProtocollo);
                            idDatiProtocollo.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(idDatiProtocollo);

                            SqlParameter idTipoBanca = new SqlParameter("@idTipoBanca", DBNull.Value);
                            if (scadenzaDaAggiornare.IdTipoBanca != null) idTipoBanca = new SqlParameter("@idTipoBanca", scadenzaDaAggiornare.IdTipoBanca);
                            idTipoBanca.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(idTipoBanca);

                            SqlParameter dataScadenza = new SqlParameter("@dataScadenza", scadenzaDaAggiornare.DataScadenza);
                            dataScadenza.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(dataScadenza);

                            SqlParameter importoDare = new SqlParameter("@importoDare", scadenzaDaAggiornare.ImportoDare);
                            importoDare.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(importoDare);

                            SqlParameter importoDareIva = new SqlParameter("@importoDareIva", scadenzaDaAggiornare.ImportoDareIva);
                            importoDareIva.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(importoDareIva);

                            SqlParameter importoVersato = new SqlParameter("@importoVersato", scadenzaDaAggiornare.ImportoVersato);
                            importoVersato.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(importoVersato);

                            SqlParameter importoVersatoIva = new SqlParameter("@importoVersatoIva", scadenzaDaAggiornare.ImportoVersatoIva);
                            importoVersatoIva.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(importoVersatoIva);

                            SqlParameter dataVersamento = new SqlParameter("@dataVersamento", DBNull.Value);
                            if (scadenzaDaAggiornare.DataVersamento != null) dataVersamento = new SqlParameter("@dataVersamento", scadenzaDaAggiornare.DataVersamento);
                            dataVersamento.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(dataVersamento);

                            SqlParameter importoAvere = new SqlParameter("@importoAvere", scadenzaDaAggiornare.ImportoAvere);
                            importoAvere.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(importoAvere);

                            SqlParameter importoAvereIva = new SqlParameter("@importoAvereIva", scadenzaDaAggiornare.ImportoAvereIva);
                            importoAvereIva.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(importoAvereIva);

                            SqlParameter importoRiscosso = new SqlParameter("@importoRiscosso", scadenzaDaAggiornare.ImportoRiscosso);
                            importoRiscosso.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(importoRiscosso);

                            SqlParameter importoRiscossoIva = new SqlParameter("@importoRiscossoIva", scadenzaDaAggiornare.ImportoRiscossoIva);
                            importoRiscossoIva.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(importoRiscossoIva);

                            SqlParameter dataRiscossione = new SqlParameter("@dataRiscossione", DBNull.Value);
                            if (scadenzaDaAggiornare.DataRiscossione != null) dataRiscossione = new SqlParameter("@dataRiscossione", scadenzaDaAggiornare.DataRiscossione);
                            dataRiscossione.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(dataRiscossione);

                            SqlParameter note = new SqlParameter("@note", DBNull.Value);
                            if (scadenzaDaAggiornare.Note != null) note = new SqlParameter("@note", scadenzaDaAggiornare.Note);
                            note.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(note);

                            SqlParameter iva = new SqlParameter("@iva", scadenzaDaAggiornare.Iva);
                            iva.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(iva);


                            sql.Transaction = transaction;
                            sql.ExecuteNonQuery();
                            #endregion

                            #region CREAZIONE NUOVA SCADENZA
                            sql.CommandType = CommandType.StoredProcedure;
                            sql.CommandText = "InsertDatiScadenzario";
                            sql.Parameters.Clear();
                            sda.SelectCommand = sql;

                            sql.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

                            #region PARAMETRI PER LOG UTENTE
                            idUtente = new SqlParameter("@idUtente", utente.id);
                            idUtente.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(idUtente);

                            nomeUtente = new SqlParameter("@nomeUtente", utente.username);
                            nomeUtente.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(nomeUtente);
                            #endregion

                            idPadre = new SqlParameter("@idPadre", scadenzaDaInserire.IdPadre);
                            idPadre.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(idPadre);

                            idDatiProtocollo = new SqlParameter("@idDatiProtocollo", scadenzaDaInserire.IdDatiProtocollo);
                            idDatiProtocollo.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(idDatiProtocollo);

                            idTipoBanca = new SqlParameter("@idTipoBanca", DBNull.Value);
                            if (scadenzaDaInserire.IdTipoBanca != null) idTipoBanca = new SqlParameter("@idTipoBanca", scadenzaDaInserire.IdTipoBanca);
                            idTipoBanca.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(idTipoBanca);

                            dataScadenza = new SqlParameter("@dataScadenza", scadenzaDaInserire.DataScadenza);
                            dataScadenza.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(dataScadenza);

                            importoDare = new SqlParameter("@importoDare", scadenzaDaInserire.ImportoDare);
                            importoDare.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(importoDare);

                            importoDareIva = new SqlParameter("@importoDareIva", scadenzaDaInserire.ImportoDareIva);
                            importoDareIva.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(importoDareIva);

                            importoVersato = new SqlParameter("@importoVersato", scadenzaDaInserire.ImportoVersato);
                            importoVersato.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(importoVersato);

                            importoVersatoIva = new SqlParameter("@importoVersatoIva", scadenzaDaInserire.ImportoVersatoIva);
                            importoVersatoIva.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(importoVersatoIva);

                            dataVersamento = new SqlParameter("@dataVersamento", DBNull.Value);
                            if (scadenzaDaInserire.DataVersamento != null) dataVersamento = new SqlParameter("@dataVersamento", scadenzaDaInserire.DataVersamento);
                            dataVersamento.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(dataVersamento);

                            importoAvere = new SqlParameter("@importoAvere", scadenzaDaInserire.ImportoAvere);
                            importoAvere.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(importoAvere);

                            importoAvereIva = new SqlParameter("@importoAvereIva", scadenzaDaInserire.ImportoAvereIva);
                            importoAvereIva.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(importoAvereIva);

                            importoRiscosso = new SqlParameter("@importoRiscosso", scadenzaDaInserire.ImportoRiscosso);
                            importoRiscosso.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(importoRiscosso);

                            importoRiscossoIva = new SqlParameter("@importoRiscossoIva", scadenzaDaInserire.ImportoRiscossoIva);
                            importoRiscossoIva.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(importoRiscossoIva);

                            dataRiscossione = new SqlParameter("@dataRiscossione", DBNull.Value);
                            if (scadenzaDaInserire.DataRiscossione != null) dataRiscossione = new SqlParameter("@dataRiscossione", scadenzaDaInserire.DataRiscossione);
                            dataRiscossione.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(dataRiscossione);

                            note = new SqlParameter("@note", DBNull.Value);
                            if (scadenzaDaInserire.Note != null) note = new SqlParameter("@note", scadenzaDaInserire.Note);
                            note.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(note);

                            iva = new SqlParameter("@iva", scadenzaDaInserire.Iva);
                            iva.Direction = ParameterDirection.Input;
                            sql.Parameters.Add(iva);

                            sql.Transaction = transaction;
                            sql.ExecuteNonQuery();
                            #endregion

                            // Attempt to commit the transaction.
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            esito.Codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                            esito.Descrizione = "Scadenzario_DAL.cs - CancellaFigli_AggiungiPagamento " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
                        }
                    }
                }
            }
        }
    }
}