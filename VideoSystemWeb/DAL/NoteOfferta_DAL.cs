using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using VideoSystemWeb.BLL;
using VideoSystemWeb.Entity;
namespace VideoSystemWeb.DAL
{
    public class NoteOfferta_DAL : Base_DAL
    {
        //singleton
        private static volatile NoteOfferta_DAL instance;
        private static object objForLock = new Object();

        private NoteOfferta_DAL() { }

        public static NoteOfferta_DAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new NoteOfferta_DAL();
                    }
                }
                return instance;
            }
        }
        public NoteOfferta getNoteOffertaByIdDatiAgenda(ref Esito esito, int idDatiAgenda)
        {
            NoteOfferta noteOfferta = new NoteOfferta();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM tab_note_offerta WHERE id_dati_agenda = " + idDatiAgenda.ToString();
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
                                    noteOfferta.Id = dt.Rows[0].Field<int>("id");
                                    noteOfferta.Id_dati_agenda = dt.Rows[0].Field<int>("id_dati_agenda");
                                    noteOfferta.Banca = dt.Rows[0].Field<string>("banca");
                                    noteOfferta.Pagamento = dt.Rows[0].Field<int>("pagamento");
                                    noteOfferta.Consegna = dt.Rows[0].Field<string>("consegna");
                                    noteOfferta.Note = "";
                                    if (!string.IsNullOrEmpty(dt.Rows[0].Field<string>("note"))) noteOfferta.Note = dt.Rows[0].Field<string>("note");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.descrizione = "NoteOfferta_DAL.cs - getNoteOffertaByIdDatiAgenda " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return noteOfferta;
        }

        public NoteOfferta getNoteOffertaById(ref Esito esito, int id)
        {
            NoteOfferta noteOfferta = new NoteOfferta();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM tab_note_offerta WHERE id = " + id.ToString();
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
                                    noteOfferta.Id = dt.Rows[0].Field<int>("id");
                                    noteOfferta.Id_dati_agenda = dt.Rows[0].Field<int>("id_dati_agenda");
                                    noteOfferta.Banca = dt.Rows[0].Field<string>("banca");
                                    noteOfferta.Pagamento = dt.Rows[0].Field<int>("pagamento");
                                    noteOfferta.Consegna = dt.Rows[0].Field<string>("consegna");
                                    noteOfferta.Note = dt.Rows[0].Field<string>("note");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.descrizione = "NoteOfferta_DAL.cs - getNoteOffertaById " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return noteOfferta;

        }

        public int CreaNoteOfferta(NoteOfferta noteOfferta, ref Esito esito)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("InsertNoteOfferta"))
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

                            SqlParameter id_dati_agenda = new SqlParameter("@id_dati_agenda", noteOfferta.Id_dati_agenda);
                            id_dati_agenda.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_dati_agenda);

                            SqlParameter banca = new SqlParameter("@banca", noteOfferta.Banca);
                            banca.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(banca);

                            SqlParameter pagamento = new SqlParameter("@pagamento", noteOfferta.Pagamento);
                            pagamento.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(pagamento);

                            SqlParameter consegna = new SqlParameter("@consegna", noteOfferta.Consegna);
                            consegna.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(consegna);

                            if (string.IsNullOrEmpty(noteOfferta.Note)) noteOfferta.Note = string.Empty;
                            SqlParameter note = new SqlParameter("@note", noteOfferta.Note);
                            note.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(note);

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
                esito.codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.descrizione = "NoteOfferta_DAL.cs - CreaNoteOfferta " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return 0;
        }

        public Esito AggiornaNoteOfferta(NoteOfferta noteOfferta)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);
            Esito esito = new Esito();
            try
            {
                using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(sqlConstr))
                {
                    using (System.Data.SqlClient.SqlCommand StoreProc = new System.Data.SqlClient.SqlCommand("UpdateNoteOfferta"))
                    {
                        using (System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            System.Data.SqlClient.SqlParameter id = new System.Data.SqlClient.SqlParameter("@id", noteOfferta.Id);
                            id.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id);

                            // PARAMETRI PER LOG UTENTE
                            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
                            idUtente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idUtente);

                            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
                            nomeUtente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nomeUtente);
                            // FINE PARAMETRI PER LOG UTENTE

                            SqlParameter id_dati_agenda = new SqlParameter("@id_dati_agenda", noteOfferta.Id_dati_agenda);
                            id_dati_agenda.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_dati_agenda);

                            SqlParameter banca = new SqlParameter("@banca", noteOfferta.Banca);
                            banca.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(banca);

                            SqlParameter pagamento = new SqlParameter("@pagamento", noteOfferta.Pagamento);
                            pagamento.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(pagamento);

                            SqlParameter consegna = new SqlParameter("@consegna", noteOfferta.Consegna);
                            consegna.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(consegna);

                            SqlParameter note = new SqlParameter("@note", noteOfferta.Note);
                            note.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(note);

                            StoreProc.Connection.Open();

                            int iReturn = StoreProc.ExecuteNonQuery();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.descrizione = "NoteOfferta_DAL.cs - AggiornaNoteOfferta " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return esito;
        }

        public Esito EliminaNoteOfferta(int idNotaOfferta)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);
            Esito esito = new Esito();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("DeleteNoteOfferta"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            SqlParameter id = new SqlParameter("@id", SqlDbType.Int);
                            id.Direction = ParameterDirection.Input;
                            id.Value = idNotaOfferta;
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
                esito.descrizione = "NoteOfferta_DAL.cs - EliminaNoteOfferta " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return esito;
        }

    }
}