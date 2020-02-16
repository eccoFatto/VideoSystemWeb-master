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
    public class Note_Lavorazione_Magazzino_DAL : Base_DAL
    {
        //singleton
        private static volatile Note_Lavorazione_Magazzino_DAL instance;
        private static object objForLock = new Object();

        private Note_Lavorazione_Magazzino_DAL() { }

        public static Note_Lavorazione_Magazzino_DAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Note_Lavorazione_Magazzino_DAL();
                    }
                }
                return instance;
            }
        }

        public NoteLavorazioneMagazzino getNoteLavorazioneMagazzinoById(int idNoteLavorazioneMagazzino, ref Esito esito)
        {
            NoteLavorazioneMagazzino noteLavorazioneMagazzino = new NoteLavorazioneMagazzino();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM note_lavorazione_magazzino where id = " + idNoteLavorazioneMagazzino.ToString();
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
                                    noteLavorazioneMagazzino.Id = dt.Rows[0].Field<int>("id");
                                    noteLavorazioneMagazzino.Id_Lavorazione = dt.Rows[0].Field<int>("id_Lavorazione");
                                    noteLavorazioneMagazzino.Note = dt.Rows[0].Field<string>("note");

                                    noteLavorazioneMagazzino.Attivo = dt.Rows[0].Field<bool>("attivo");
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
            return noteLavorazioneMagazzino;
        }

         public int CreaNoteLavorazioneMagazzino(NoteLavorazioneMagazzino noteLavorazioneMagazzino, ref Esito esito)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("InsertNoteLavorazioneMagazzino"))
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

                            SqlParameter id_Lavorazione = new SqlParameter("@id_Lavorazione", noteLavorazioneMagazzino.Id_Lavorazione);
                            id_Lavorazione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Lavorazione);

                            SqlParameter note = new SqlParameter("@note", noteLavorazioneMagazzino.Note);
                            note.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(note);

                            SqlParameter attivo = new SqlParameter("@attivo", noteLavorazioneMagazzino.Attivo);
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
                esito.Descrizione = "Note_Lavorazione_Magazzino_DAL.cs - CreaNoteLavorazioneMagazzino " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return 0;
        }

        public Esito AggiornaNoteLavorazioneMagazzino(NoteLavorazioneMagazzino noteLavorazioneMagazzino)
        {
            Esito esito = new Esito();
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);
            try
            {
                using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(sqlConstr))
                {
                    using (System.Data.SqlClient.SqlCommand StoreProc = new System.Data.SqlClient.SqlCommand("UpdateNoteLavorazioneMagazzino"))
                    {
                        using (System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            System.Data.SqlClient.SqlParameter id = new System.Data.SqlClient.SqlParameter("@id", noteLavorazioneMagazzino.Id);
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

                            SqlParameter id_Lavorazione = new SqlParameter("@id_Lavorazione", noteLavorazioneMagazzino.Id_Lavorazione);
                            id_Lavorazione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Lavorazione);



                            SqlParameter note = new SqlParameter("@note", noteLavorazioneMagazzino.Note);
                            note.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(note);



                            SqlParameter attivo = new SqlParameter("@attivo", noteLavorazioneMagazzino.Attivo);
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
                esito.Descrizione = "Note_Lavorazione_Magazzino_DAL.cs - AggiornaNoteLavorazioneMagazzino " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

        public Esito EliminaNoteLavorazioneMagazzino(int idNoteLavorazioneMagazzino)
        {
            Esito esito = new Esito();
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("DeleteNoteLavorazioneMagazzino"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            SqlParameter id = new SqlParameter("@id", SqlDbType.Int);
                            id.Direction = ParameterDirection.Input;
                            id.Value = idNoteLavorazioneMagazzino;
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
                esito.Descrizione = "Note_Lavorazione_Magazzino_DAL.cs - EliminaNoteLavorazioneMagazzino " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

    }
}