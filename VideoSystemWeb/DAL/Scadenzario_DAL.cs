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

                            //StoreProc.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

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
                                    scadenza.ImportoDareIva = dt.Rows[0].Field<decimal>("importoDare");
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
                                                           string codiceAnagrafica, 
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
                if (!string.IsNullOrEmpty(codiceAnagrafica)) filtriRicerca += " and b.cliente like '%" + codiceAnagrafica + "%'";
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
                if (!string.IsNullOrEmpty(dataFatturaDa)) filtriRicerca += " and b.data_protocollo >= '" + dataFatturaDa + "'";
                if (!string.IsNullOrEmpty(dataFatturaA)) filtriRicerca += " and b.data_protocollo <= '" + dataFatturaA + "'";
                if (!string.IsNullOrEmpty(dataScadenzaDa)) filtriRicerca += " and a.dataScadenza >= '" + dataScadenzaDa + "'";
                if (!string.IsNullOrEmpty(dataScadenzaA)) filtriRicerca += " and a.dataScadenza <= '" + dataScadenzaA + "'";

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
                                            ImportoDareIva = riga.Field<decimal>("importoDare"),
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

        public List<DatiScadenzario> GetDatiTotaliFatturaByIdDatiScadenzario(int idSDatiScadenzario, ref Esito esito)
        {
            List<DatiScadenzario> listaDatiScadenzario = new List<DatiScadenzario>();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "select * from dati_scadenzario  a left join dati_protocollo b on a.idDatiProtocollo = b.id " +
                                   "where idDatiProtocollo = (select idDatiProtocollo from dati_scadenzario where id = " + idSDatiScadenzario.ToString() +")";

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
                                            ImportoDareIva = riga.Field<decimal>("importoDare"),
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
    }
}