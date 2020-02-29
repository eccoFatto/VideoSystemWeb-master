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
    public class Note_Agenda_Magazzino_DAL : Base_DAL
    {
        //singleton
        private static volatile Note_Agenda_Magazzino_DAL instance;
        private static object objForLock = new Object();

        private Note_Agenda_Magazzino_DAL() { }

        public static Note_Agenda_Magazzino_DAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Note_Agenda_Magazzino_DAL();
                    }
                }
                return instance;
            }
        }

        public NoteAgendaMagazzino getNoteAgendaMagazzinoById(int idNoteAgendaMagazzino, ref Esito esito)
        {
            NoteAgendaMagazzino noteAgendaMagazzino = new NoteAgendaMagazzino();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM note_agenda_magazzino where id = " + idNoteAgendaMagazzino.ToString();
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
                                    noteAgendaMagazzino.Id = dt.Rows[0].Field<int>("id");
                                    noteAgendaMagazzino.Id_Agenda = dt.Rows[0].Field<int>("id_Agenda");
                                    noteAgendaMagazzino.Note = dt.Rows[0].Field<string>("note");

                                    noteAgendaMagazzino.Attivo = dt.Rows[0].Field<bool>("attivo");
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
            return noteAgendaMagazzino;
        }

        public NoteAgendaMagazzino getNoteAgendaMagazzinoByIdAgenda(int idAgenda, ref Esito esito)
        {
            NoteAgendaMagazzino noteAgendaMagazzino = new NoteAgendaMagazzino();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT TOP (1) * FROM note_agenda_magazzino where id_Agenda = " + idAgenda.ToString() +
                                   " and attivo = 1" +
                                   " order by id desc";
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
                                    noteAgendaMagazzino.Id = dt.Rows[0].Field<int>("id");
                                    noteAgendaMagazzino.Id_Agenda = dt.Rows[0].Field<int>("id_Agenda");
                                    noteAgendaMagazzino.Note = dt.Rows[0].Field<string>("note");

                                    noteAgendaMagazzino.Attivo = dt.Rows[0].Field<bool>("attivo");
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
            return noteAgendaMagazzino;
        }

        public int CreaNoteAgendaMagazzino(NoteAgendaMagazzino noteAgendaMagazzino, ref Esito esito)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("InsertNoteAgendaMagazzino"))
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

                            SqlParameter id_Agenda = new SqlParameter("@id_Agenda", noteAgendaMagazzino.Id_Agenda);
                            id_Agenda.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Agenda);

                            SqlParameter note = new SqlParameter("@note", noteAgendaMagazzino.Note);
                            note.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(note);

                            SqlParameter attivo = new SqlParameter("@attivo", noteAgendaMagazzino.Attivo);
                            attivo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(attivo);

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
                esito.Descrizione = "Note_Agenda_Magazzino_DAL.cs - CreaNoteAgendaMagazzino " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return 0;
        }

        public Esito AggiornaNoteAgendaMagazzino(NoteAgendaMagazzino noteAgendaMagazzino)
        {
            Esito esito = new Esito();
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);
            try
            {
                using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(sqlConstr))
                {
                    using (System.Data.SqlClient.SqlCommand StoreProc = new System.Data.SqlClient.SqlCommand("UpdateNoteAgendaMagazzino"))
                    {
                        using (System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            System.Data.SqlClient.SqlParameter id = new System.Data.SqlClient.SqlParameter("@id", noteAgendaMagazzino.Id);
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

                            SqlParameter id_Agenda = new SqlParameter("@id_Agenda", noteAgendaMagazzino.Id_Agenda);
                            id_Agenda.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Agenda);



                            SqlParameter note = new SqlParameter("@note", noteAgendaMagazzino.Note);
                            note.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(note);



                            SqlParameter attivo = new SqlParameter("@attivo", noteAgendaMagazzino.Attivo);
                            attivo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(attivo);


                            StoreProc.Connection.Open();

                            int iReturn = StoreProc.ExecuteNonQuery();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.Descrizione = "Note_Agenda_Magazzino_DAL.cs - AggiornaNoteAgendaMagazzino " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

        public Esito EliminaNoteAgendaMagazzino(int idNoteAgendaMagazzino)
        {
            Esito esito = new Esito();
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("DeleteNoteAgendaMagazzino"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            SqlParameter id = new SqlParameter("@id", SqlDbType.Int);
                            id.Direction = ParameterDirection.Input;
                            id.Value = idNoteAgendaMagazzino;
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
                esito.Codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.Descrizione = "Note_Agenda_Magazzino_DAL.cs - EliminaNoteAgendaMagazzino " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

    }
}