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
    public class Anag_Qualifiche_Collaboratori_DAL : Base_DAL
    {
        //singleton
        private static volatile Anag_Qualifiche_Collaboratori_DAL instance;
        private static object objForLock = new Object();

        private Anag_Qualifiche_Collaboratori_DAL() { }

        public static Anag_Qualifiche_Collaboratori_DAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Anag_Qualifiche_Collaboratori_DAL();
                    }
                }
                return instance;
            }
        }

        public List<Anag_Qualifiche_Collaboratori> getQualificheByIdCollaboratore(ref Esito esito, int idCollaboratore, bool soloAttivi = true)
        {
            List<Anag_Qualifiche_Collaboratori> listaQUalifiche = new List<Anag_Qualifiche_Collaboratori>();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM anag_qualifiche_collaboratori WHERE id_Collaboratore = " + idCollaboratore.ToString();
                    if (soloAttivi) query += " AND ATTIVO = 1";
                    query += " ORDER BY priorita,qualifica";
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

                                        Anag_Qualifiche_Collaboratori qualifica = new Anag_Qualifiche_Collaboratori();
                                        qualifica.Id = riga.Field<int>("id");
                                        qualifica.Descrizione = riga.Field<string>("Descrizione");
                                        qualifica.Id_collaboratore = riga.Field<int>("id_Collaboratore");
                                        qualifica.Qualifica = riga.Field<string>("Qualifica");
                                        qualifica.Priorita = riga.Field<int>("priorita");
                                        qualifica.Attivo = riga.Field<bool>("attivo");

                                        listaQUalifiche.Add(qualifica);
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

            return listaQUalifiche;
        }

        public List<Anag_Qualifiche_Collaboratori> getAllQualifiche(ref Esito esito, bool soloAttivi = true)
        {
            List<Anag_Qualifiche_Collaboratori> listaQUalifiche = new List<Anag_Qualifiche_Collaboratori>();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM anag_qualifiche_collaboratori WHERE ATTIVO = 1";
                    query += " ORDER BY priorita,qualifica";
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

                                        Anag_Qualifiche_Collaboratori qualifica = new Anag_Qualifiche_Collaboratori();
                                        qualifica.Id = riga.Field<int>("id");
                                        qualifica.Descrizione = riga.Field<string>("Descrizione");
                                        qualifica.Id_collaboratore = riga.Field<int>("id_Collaboratore");
                                        qualifica.Qualifica = riga.Field<string>("Qualifica");
                                        qualifica.Priorita = riga.Field<int>("priorita");
                                        qualifica.Attivo = riga.Field<bool>("attivo");

                                        listaQUalifiche.Add(qualifica);
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

            return listaQUalifiche;
        }

        public int CreaQualificaCollaboratore(Anag_Qualifiche_Collaboratori qualificaCollaboratore, Anag_Utenti utente, ref Esito esito)
        {
            //@id int,
            //@id_collaboratore int,
            //@priorita int,
            //@qualifica varchar(60),
	        //@descrizione varchar(60),
	        //@attivo bit
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("InsertQualificheCollaboratore"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            StoreProc.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

                            SqlParameter id_collaboratore = new SqlParameter("@id_collaboratore", qualificaCollaboratore.Id_collaboratore);
                            id_collaboratore.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_collaboratore);

                            // PARAMETRI PER LOG UTENTE
                            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
                            idUtente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idUtente);

                            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
                            nomeUtente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nomeUtente);
                            // FINE PARAMETRI PER LOG UTENTE

                            SqlParameter priorita = new SqlParameter("@priorita", qualificaCollaboratore.Priorita);
                            priorita.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(priorita);

                            SqlParameter qualifica = new SqlParameter("@qualifica", qualificaCollaboratore.Qualifica);
                            qualifica.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(qualifica);

                            SqlParameter descrizione = new SqlParameter("@descrizione", qualificaCollaboratore.Descrizione);
                            descrizione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(descrizione);

                            SqlParameter attivo = new SqlParameter("@attivo", qualificaCollaboratore.Attivo);
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
                esito.codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.descrizione = "Anag_Qualifiche_Collaboratori_DAL.cs - CreaQualificaCollaboratore " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return 0;
        }

        public Esito AggiornaQualificaCollaboratore(Anag_Qualifiche_Collaboratori qualificaCollaboratore, Anag_Utenti utente)
        {
            Esito esito = new Esito();
            try
            {
                using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(sqlConstr))
                {
                    using (System.Data.SqlClient.SqlCommand StoreProc = new System.Data.SqlClient.SqlCommand("UpdateQualificheCollaboratore"))
                    {
                        using (System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            System.Data.SqlClient.SqlParameter id = new System.Data.SqlClient.SqlParameter("@id", qualificaCollaboratore.Id);
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

                            SqlParameter id_collaboratore = new SqlParameter("@id_collaboratore", qualificaCollaboratore.Id_collaboratore);
                            id_collaboratore.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_collaboratore);

                            SqlParameter priorita = new SqlParameter("@priorita", qualificaCollaboratore.Priorita);
                            priorita.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(priorita);

                            SqlParameter qualifica = new SqlParameter("@qualifica", qualificaCollaboratore.Qualifica);
                            qualifica.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(qualifica);

                            SqlParameter descrizione = new SqlParameter("@descrizione", qualificaCollaboratore.Descrizione);
                            descrizione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(descrizione);

                            SqlParameter attivo = new SqlParameter("@attivo", qualificaCollaboratore.Attivo);
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
                esito.descrizione = "Anag_Qualifiche_Collaboratori_DAL.cs - AggiornaQualificaCollaboratore " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

        public Esito EliminaQualificaCollaboratore(int idQualificaCollaboratore, Anag_Utenti utente)
        {
            Esito esito = new Esito();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("DeleteQualificheCollaboratore"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            SqlParameter id = new SqlParameter("@id", SqlDbType.Int);
                            id.Direction = ParameterDirection.Input;
                            id.Value = idQualificaCollaboratore;
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
                esito.descrizione = "Anag_Qualifiche_Collaboratori_DAL.cs - EliminaQualificaCollaboratore " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }
    }
}