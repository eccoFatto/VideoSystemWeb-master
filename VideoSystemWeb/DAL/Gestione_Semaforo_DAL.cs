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
    public class Gestione_Semaforo_DAL : Base_DAL
    {
        #region SINGLETON
        private static volatile Gestione_Semaforo_DAL instance;
        private static object objForLock = new Object();
        private Gestione_Semaforo_DAL() { }
        public static Gestione_Semaforo_DAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Gestione_Semaforo_DAL();
                    }
                }
                return instance;
            }
        }
        #endregion  

        public bool IsAccessoLavorazioneBloccato(int idAgenda, out Tab_Semaforo_Lavorazioni semaforo, ref Esito esito)
        {
            semaforo = null;

            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                using (SqlCommand cmd = new SqlCommand(
                    @"SELECT TOP(1) id, id_agenda, id_utente, nome_utente, data_accesso FROM tab_semaforo_lavorazioni WHERE id_agenda = @idAgenda", con))
                {
                    cmd.Parameters.AddWithValue("@idAgenda", idAgenda);

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            semaforo = new Tab_Semaforo_Lavorazioni
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Id_Agenda = reader.GetInt32(reader.GetOrdinal("id_agenda")),
                                Id_Utente = reader.GetInt32(reader.GetOrdinal("id_utente")),
                                Nome_Utente = reader.GetString(reader.GetOrdinal("nome_utente")),
                                Data_Accesso = reader.GetDateTime(reader.GetOrdinal("data_accesso"))
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.Descrizione = ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return semaforo != null;
        }

        public Esito InserisciAccessoLavorazione(Tab_Semaforo_Lavorazioni semaforo)
        {
            Esito esito = new Esito();

            if (semaforo.Id_Agenda != 0)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(sqlConstr))
                    using (SqlCommand cmd = new SqlCommand(@"INSERT INTO tab_semaforo_lavorazioni (id_agenda, id_utente, nome_utente, data_accesso) 
                                                        VALUES (@id_agenda, @id_utente, @nome_utente, @data_accesso);
                                                        SELECT SCOPE_IDENTITY();", con))
                    {
                        // Parametri (NO AddWithValue)
                        cmd.Parameters.Add("@id_agenda", SqlDbType.Int).Value = semaforo.Id_Agenda;
                        cmd.Parameters.Add("@id_utente", SqlDbType.Int).Value = semaforo.Id_Utente;
                        cmd.Parameters.Add("@nome_utente", SqlDbType.VarChar, 50).Value = semaforo.Nome_Utente;
                        cmd.Parameters.Add("@data_accesso", SqlDbType.DateTime).Value = semaforo.Data_Accesso;

                        con.Open();

                        // Recupero ID generato
                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            semaforo.Id = Convert.ToInt32(result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                    esito.Descrizione = ex.Message + Environment.NewLine + ex.StackTrace;
                }
            }
            return esito;
        }

        public Esito ModificaAccessoLavorazione(Tab_Semaforo_Lavorazioni semaforo)
        {
            Esito esito = new Esito();

            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                using (SqlCommand cmd = new SqlCommand(@"UPDATE tab_semaforo_lavorazioni
                                                        SET 
                                                            id_utente = @id_utente,
                                                            nome_utente = @nome_utente,
                                                            data_accesso = @data_accesso
                                                        WHERE id_agenda = @id_agenda", con))
                {
                    // Parametri tipizzati (no AddWithValue)
                    cmd.Parameters.Add("@id_agenda", SqlDbType.Int).Value = semaforo.Id_Agenda;
                    cmd.Parameters.Add("@id_utente", SqlDbType.Int).Value = semaforo.Id_Utente;
                    cmd.Parameters.Add("@nome_utente", SqlDbType.VarChar, 50).Value = semaforo.Nome_Utente;
                    cmd.Parameters.Add("@data_accesso", SqlDbType.DateTime).Value = semaforo.Data_Accesso;

                    con.Open();

                    int rowsAffected = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.Descrizione = ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

        public Esito EliminaAccessoLavorazione(int id_agenda)
        {
            Esito esito = new Esito();
            int idUtente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]).id;

            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                using (SqlCommand cmd = new SqlCommand(@"DELETE FROM tab_semaforo_lavorazioni WHERE id_agenda = @id_agenda AND id_utente = @id_utente", con))
                {
                    // Parametri tipizzati
                    cmd.Parameters.Add("@id_agenda", SqlDbType.Int).Value = id_agenda;
                    cmd.Parameters.Add("@id_utente", SqlDbType.Int).Value = idUtente;

                    con.Open();

                    int rowsAffected = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.Descrizione = ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }
    }
}