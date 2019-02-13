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
    public class Anag_Email_Collaboratori_DAL : Base_DAL
    {
        //singleton
        private static volatile Anag_Email_Collaboratori_DAL instance;
        private static object objForLock = new Object();

        private Anag_Email_Collaboratori_DAL() { }

        public static Anag_Email_Collaboratori_DAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Anag_Email_Collaboratori_DAL();
                    }
                }
                return instance;
            }
        }

        public Anag_Email_Collaboratori getEmailById(ref Esito esito, int id)
        {
            Anag_Email_Collaboratori anagEmail = new Anag_Email_Collaboratori();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM anag_email_collaboratori WHERE id = " + id.ToString();
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                if (dt != null && dt.Rows != null && dt.Rows.Count == 1)
                                {
                                    anagEmail.Id = dt.Rows[0].Field<int>("id");
                                    anagEmail.Id_collaboratore = dt.Rows[0].Field<int>("id_collaboratore");

                                    anagEmail.IndirizzoEmail = dt.Rows[0].Field<string>("indirizzoEmail");
                                    anagEmail.Descrizione = dt.Rows[0].Field<string>("descrizione");
                                    anagEmail.Priorita = dt.Rows[0].Field<int>("priorita");
                                    anagEmail.Attivo = dt.Rows[0].Field<bool>("attivo");
                                }
                                else
                                {
                                    esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                    esito.descrizione = "Nessun dato trovato nella tabella anag_email_collaboratori ";
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

            return anagEmail;

        }
        public List<Anag_Email_Collaboratori> getEmailByIdCollaboratore(ref Esito esito, int idCollaboratore, bool soloAttivi = true)
        {
            List<Anag_Email_Collaboratori> listaEmail = new List<Anag_Email_Collaboratori>();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM anag_email_collaboratori WHERE id_Collaboratore = " + idCollaboratore.ToString();
                    if (soloAttivi) query += " AND ATTIVO = 1";
                    query += " ORDER BY priorita,indirizzoEmail";
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

                                        Anag_Email_Collaboratori email = new Anag_Email_Collaboratori();
                                        email.Id = riga.Field<int>("id");
                                        email.Id_collaboratore = riga.Field<int>("id_collaboratore");

                                        email.IndirizzoEmail = riga.Field<string>("indirizzoEmail");
                                        email.Descrizione = riga.Field<string>("descrizione");
                                        email.Priorita = riga.Field<int>("priorita");
                                        email.Attivo = riga.Field<bool>("attivo");

                                        listaEmail.Add(email);
                                    }
                                }
                                else
                                {
                                    esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                    esito.descrizione = "Nessun dato trovato nella tabella anag_email_collaboratori ";
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

            return listaEmail;
        }

        public int CreaEmailCollaboratore(Anag_Email_Collaboratori emailCollaboratore, ref Esito esito)
        {
            //@id_collaboratore int,
            //@priorita int,
            //@indirizzoEmail varchar(60),
            //@descrizione varchar(60),
            //@attivo bit,
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("InsertEmailCollaboratore"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            StoreProc.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

                            SqlParameter id_collaboratore = new SqlParameter("@id_collaboratore", emailCollaboratore.Id_collaboratore);
                            id_collaboratore.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_collaboratore);

                            SqlParameter priorita = new SqlParameter("@priorita", emailCollaboratore.Priorita);
                            priorita.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(priorita);

                            SqlParameter indirizzoEmail = new SqlParameter("@indirizzoEmail", emailCollaboratore.IndirizzoEmail);
                            indirizzoEmail.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(indirizzoEmail);

                            SqlParameter descrizione = new SqlParameter("@descrizione", emailCollaboratore.Descrizione);
                            descrizione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(descrizione);

                            SqlParameter attivo = new SqlParameter("@attivo", emailCollaboratore.Attivo);
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
                esito.descrizione = "Anag_Email_Collaboratori_DAL.cs - CreaEmailCollaboratore " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return 0;
        }

        public Esito AggiornaEmailCollaboratore(Anag_Email_Collaboratori emailCollaboratore)
        {
            Esito esito = new Esito();
            try
            {
                using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(sqlConstr))
                {
                    using (System.Data.SqlClient.SqlCommand StoreProc = new System.Data.SqlClient.SqlCommand("UpdateEmailCollaboratore"))
                    {
                        using (System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            System.Data.SqlClient.SqlParameter id = new System.Data.SqlClient.SqlParameter("@id", emailCollaboratore.Id);
                            id.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id);

                            SqlParameter id_collaboratore = new SqlParameter("@id_collaboratore", emailCollaboratore.Id_collaboratore);
                            id_collaboratore.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_collaboratore);

                            SqlParameter priorita = new SqlParameter("@priorita", emailCollaboratore.Priorita);
                            priorita.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(priorita);

                            SqlParameter indirizzoEmail = new SqlParameter("@indirizzoEmail", emailCollaboratore.IndirizzoEmail);
                            indirizzoEmail.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(indirizzoEmail);

                            SqlParameter descrizione = new SqlParameter("@descrizione", emailCollaboratore.Descrizione);
                            descrizione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(descrizione);

                            SqlParameter attivo = new SqlParameter("@attivo", emailCollaboratore.Attivo);
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
                esito.descrizione = "Anag_Email_Collaboratori_DAL.cs - aggiornaEmailCollaboratore " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

        public Esito EliminaEmailCollaboratore(int idEmailCollaboratore)
        {
            Esito esito = new Esito();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("DeleteEmailCollaboratore"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            SqlParameter id = new SqlParameter("@id", SqlDbType.Int);
                            id.Direction = ParameterDirection.Input;
                            id.Value = idEmailCollaboratore;
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
                esito.descrizione = "Anag_Email_Collaboratori_DAL.cs - EliminaEmailCollaboratore " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }


    }
}