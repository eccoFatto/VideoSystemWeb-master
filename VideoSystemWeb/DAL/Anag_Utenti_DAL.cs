using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using VideoSystemWeb.BLL;
using VideoSystemWeb.Entity;
using System.Data.SqlClient;
namespace VideoSystemWeb.DAL
{
    public class Anag_Utenti_DAL : Base_DAL
    {
        //singleton
        private static volatile Anag_Utenti_DAL instance;
        private static object objForLock = new Object();

        private Anag_Utenti_DAL() { }

        public static Anag_Utenti_DAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Anag_Utenti_DAL();
                    }
                }
                return instance;
            }
        }

        public Utenti getUtenteById(int idUtente, ref Esito esito)
        {
            Utenti utente = new Utenti();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM anag_utenti where id = " + idUtente.ToString();
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
                                    utente.Id = dt.Rows[0].Field<int>("id");
                                    utente.Cognome = dt.Rows[0].Field<string>("cognome");
                                    utente.Nome = dt.Rows[0].Field<string>("nome");
                                    utente.Descrizione = dt.Rows[0].Field<string>("descrizione");
                                    utente.Username = dt.Rows[0].Field<string>("username");
                                    utente.Password = dt.Rows[0].Field<string>("password");
                                    utente.Id_tipoUtente = dt.Rows[0].Field<int>("id_tipoUtente");
                                    utente.Email = dt.Rows[0].Field<string>("email");
                                    utente.Attivo = dt.Rows[0].Field<bool>("attivo");
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
            return utente;
        }

        public Utenti getUtenteByUserAndEmail(string username, string email, ref Esito esito)
        {
            Utenti utente = new Utenti();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM ANAG_UTENTI WHERE USERNAME = '" + username.Replace("'", "''") + "' AND EMAIL = '" + email.Replace("'", "''") + "'";
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
                                    utente.Id = dt.Rows[0].Field<int>("id");
                                    utente.Cognome = dt.Rows[0].Field<string>("cognome");
                                    utente.Nome = dt.Rows[0].Field<string>("nome");
                                    utente.Descrizione = dt.Rows[0].Field<string>("descrizione");
                                    utente.Username = dt.Rows[0].Field<string>("username");
                                    utente.Password = dt.Rows[0].Field<string>("password");
                                    utente.Id_tipoUtente = dt.Rows[0].Field<int>("id_tipoUtente");
                                    utente.Email = dt.Rows[0].Field<string>("email");
                                    utente.Attivo = dt.Rows[0].Field<bool>("attivo");
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
            return utente;
        }

        public List<Utenti> CaricaListaUtenti(ref Esito esito, bool soloAttivi = true)
        {
            List<Utenti> listaUtenti = new List<Utenti>();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM anag_utenti";
                    if (soloAttivi) query+= " WHERE ATTIVO = 1";
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

                                        Utenti utente = new Utenti();
                                        utente.Id = riga.Field<int>("id");
                                        utente.Cognome = riga.Field<string>("cognome");
                                        utente.Nome = riga.Field<string>("nome");
                                        utente.Descrizione = riga.Field<string>("descrizione");
                                        utente.Username = riga.Field<string>("username");
                                        utente.Password = riga.Field<string>("password");
                                        utente.Id_tipoUtente = riga.Field<int>("id_tipoUtente");
                                        utente.Email = riga.Field<string>("email");
                                        utente.Attivo = riga.Field<bool>("attivo");

                                        listaUtenti.Add(utente);
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

            return listaUtenti;
        }

        public int CreaUtente(Utenti newUtente, ref Esito esito)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("InsertUtente"))
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

                            SqlParameter cognome = new SqlParameter("@cognome", newUtente.Cognome);
                            cognome.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(cognome);

                            SqlParameter nome = new SqlParameter("@nome", newUtente.Nome);
                            nome.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome);

                            SqlParameter descrizione = new SqlParameter("@descrizione", newUtente.Descrizione);
                            descrizione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(descrizione);

                            SqlParameter username = new SqlParameter("@username", newUtente.Username);
                            username.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(username);

                            SqlParameter password = new SqlParameter("@password", newUtente.Password);
                            password.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(password);

                            SqlParameter id_tipoUtente = new SqlParameter("@id_tipoUtente", newUtente.Id_tipoUtente);
                            id_tipoUtente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_tipoUtente);

                            SqlParameter email = new SqlParameter("@email", newUtente.Email);
                            email.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(email);

                            SqlParameter attivo = new SqlParameter("@attivo", newUtente.Attivo);
                            attivo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(attivo);

                            StoreProc.Connection.Open();

                            StoreProc.ExecuteNonQuery();

                            // read output value from @NewId
                            int iReturn = Convert.ToInt32(StoreProc.Parameters["@id"].Value);


                            return iReturn;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.descrizione = "Anag_Utenti_DAL.cs - CreaUtente " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return 0;
        }

        public Esito AggiornaUtente(Utenti newUtente)
        {
            Anag_Utenti utente = ((Anag_Utenti) HttpContext.Current.Session[SessionManager.UTENTE]);
            Esito esito = new Esito();
            try
            {
                using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(sqlConstr))
                {
                    using (System.Data.SqlClient.SqlCommand StoreProc = new System.Data.SqlClient.SqlCommand("UpdateUtente"))
                    {
                        using (System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            // PARAMETRI PER LOG UTENTE
                            System.Data.SqlClient.SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
                            idUtente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idUtente);

                            System.Data.SqlClient.SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
                            nomeUtente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nomeUtente);
                            // FINE PARAMETRI PER LOG UTENTE

                            System.Data.SqlClient.SqlParameter id = new System.Data.SqlClient.SqlParameter("@id", newUtente.Id);
                            id.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id);

                            SqlParameter cognome = new SqlParameter("@cognome", newUtente.Cognome);
                            cognome.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(cognome);

                            SqlParameter nome = new SqlParameter("@nome", newUtente.Nome);
                            nome.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome);

                            SqlParameter descrizione = new SqlParameter("@descrizione", newUtente.Descrizione);
                            descrizione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(descrizione);

                            SqlParameter username = new SqlParameter("@username", newUtente.Username);
                            username.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(username);

                            SqlParameter password = new SqlParameter("@password", newUtente.Password);
                            password.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(password);

                            SqlParameter id_tipoUtente = new SqlParameter("@id_tipoUtente", newUtente.Id_tipoUtente);
                            id_tipoUtente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_tipoUtente);

                            SqlParameter email = new SqlParameter("@email", newUtente.Email);
                            email.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(email);

                            SqlParameter attivo = new SqlParameter("@attivo", newUtente.Attivo);
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
                esito.codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.descrizione = "Anag_Utenti_DAL.cs - AggiornaUtente " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

        public Esito EliminaUtente(int idUtenteToDelete)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);
            Esito esito = new Esito();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("DeleteUtente"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            SqlParameter id = new SqlParameter("@id", SqlDbType.Int);
                            id.Direction = ParameterDirection.Input;
                            id.Value = idUtenteToDelete;

                            // PARAMETRI PER LOG UTENTE
                            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
                            idUtente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idUtente);

                            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
                            nomeUtente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nomeUtente);
                            // FINE PARAMETRI PER LOG UTENTE


                            StoreProc.Parameters.Add(id);

                            StoreProc.Connection.Open();

                            int iReturn = StoreProc.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.descrizione = "Anag_Utenti_DAL.cs - EliminaUtente " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

        public Esito AggiornaPassword(int idUtenteToUpdate, string nuovaPassword)
        {
            Esito esito = new Esito();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("UpdatePasswordUtente"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            SqlParameter id = new SqlParameter("@id", SqlDbType.Int);
                            id.Direction = ParameterDirection.Input;
                            id.Value = idUtenteToUpdate;

                            SqlParameter password = new SqlParameter("@password", nuovaPassword);
                            password.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(password);

                            StoreProc.Parameters.Add(id);

                            StoreProc.Connection.Open();

                            int iReturn = StoreProc.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.descrizione = "Anag_Utenti_DAL.cs - AggiornaPassword " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

    }
}