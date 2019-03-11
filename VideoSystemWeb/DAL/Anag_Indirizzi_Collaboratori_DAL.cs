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
    public class Anag_Indirizzi_Collaboratori_DAL : Base_DAL
    {
        //singleton
        private static volatile Anag_Indirizzi_Collaboratori_DAL instance;
        private static object objForLock = new Object();

        private Anag_Indirizzi_Collaboratori_DAL() { }

        public static Anag_Indirizzi_Collaboratori_DAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Anag_Indirizzi_Collaboratori_DAL();
                    }
                }
                return instance;
            }
        }

        public List<Anag_Indirizzi_Collaboratori> getIndirizziByIdCollaboratore(ref Esito esito, int idCollaboratore, bool soloAttivi = true)
        {
            List<Anag_Indirizzi_Collaboratori> listaIndirizzi = new List<Anag_Indirizzi_Collaboratori>();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM anag_indirizzi_collaboratori WHERE id_Collaboratore = " + idCollaboratore.ToString();
                    if (soloAttivi) query += " AND ATTIVO = 1";
                    query += " ORDER BY priorita,indirizzo";
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

                                        Anag_Indirizzi_Collaboratori indirizzo = new Anag_Indirizzi_Collaboratori();
                                        indirizzo.Id = riga.Field<int>("id");
                                        indirizzo.Id_Collaboratore = riga.Field<int>("id_collaboratore");

                                        indirizzo.Tipo = riga.Field<string>("tipo");
                                        indirizzo.Indirizzo = riga.Field<string>("indirizzo");
                                        indirizzo.Nazione = riga.Field<string>("nazione");
                                        indirizzo.NumeroCivico = riga.Field<string>("numeroCivico");
                                        indirizzo.Provincia = riga.Field<string>("provincia");
                                        indirizzo.Cap = riga.Field<string>("cap");
                                        indirizzo.Comune = riga.Field<string>("comune");
                                        indirizzo.Descrizione = riga.Field<string>("descrizione");
                                        indirizzo.Priorita = riga.Field<int>("priorita");
                                        indirizzo.Attivo = riga.Field<bool>("attivo");

                                        listaIndirizzi.Add(indirizzo);
                                    }
                                }
                                //else
                                //{
                                //    esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                //    esito.descrizione = "Nessun dato trovato nella tabella anag_indirizzi_collaboratori ";
                                //}
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

            return listaIndirizzi;
        }

        public Anag_Indirizzi_Collaboratori getIndirizzoById(ref Esito esito, int id)
        {
            Anag_Indirizzi_Collaboratori indirizzo = new Anag_Indirizzi_Collaboratori();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM anag_indirizzi_collaboratori WHERE id = " + id.ToString();
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
                                    indirizzo.Id = dt.Rows[0].Field<int>("id");
                                    indirizzo.Id_Collaboratore = dt.Rows[0].Field<int>("id_collaboratore");

                                    indirizzo.Tipo = dt.Rows[0].Field<string>("tipo");
                                    indirizzo.Indirizzo = dt.Rows[0].Field<string>("indirizzo");
                                    indirizzo.Nazione = dt.Rows[0].Field<string>("nazione");
                                    indirizzo.NumeroCivico = dt.Rows[0].Field<string>("numeroCivico");
                                    indirizzo.Provincia = dt.Rows[0].Field<string>("provincia");
                                    indirizzo.Cap = dt.Rows[0].Field<string>("cap");
                                    indirizzo.Comune = dt.Rows[0].Field<string>("comune");
                                    indirizzo.Descrizione = dt.Rows[0].Field<string>("descrizione");
                                    indirizzo.Priorita = dt.Rows[0].Field<int>("priorita");
                                    indirizzo.Attivo = dt.Rows[0].Field<bool>("attivo");

                                }
                                //else
                                //{
                                //    esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                //    esito.descrizione = "Nessun dato trovato nella tabella anag_indirizzi_collaboratori ";
                                //}
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

            return indirizzo;
        }
        public int CreaIndirizziCollaboratore(Anag_Indirizzi_Collaboratori indirizzoCollaboratore, ref Esito esito)
        {
            //@id_collaboratore int,
            //@priorita int,
            //@tipo varchar(10),
	        //@indirizzo varchar(60),
	        //@numeroCivico varchar(10) = null,
	        //@cap varchar(5),
	        //@comune varchar(50),
	        //@provincia varchar(2) = null,
	        //@nazione varchar(20) = null,
	        //@descrizione varchar(60) = null,
	        //@attivo bit,
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("InsertIndirizziCollaboratore"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            StoreProc.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

                            SqlParameter id_collaboratore = new SqlParameter("@id_collaboratore", indirizzoCollaboratore.Id_Collaboratore);
                            id_collaboratore.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_collaboratore);

                            SqlParameter priorita = new SqlParameter("@priorita", indirizzoCollaboratore.Priorita);
                            priorita.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(priorita);

                            SqlParameter tipo = new SqlParameter("@tipo", indirizzoCollaboratore.Tipo);
                            tipo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(tipo);

                            SqlParameter indirizzo = new SqlParameter("@indirizzo", indirizzoCollaboratore.Indirizzo);
                            indirizzo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(indirizzo);

                            SqlParameter numeroCivico = new SqlParameter("@numeroCivico", indirizzoCollaboratore.NumeroCivico);
                            numeroCivico.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(numeroCivico);

                            SqlParameter cap = new SqlParameter("@cap", indirizzoCollaboratore.Cap);
                            cap.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(cap);

                            SqlParameter comune = new SqlParameter("@comune", indirizzoCollaboratore.Comune);
                            comune.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(comune);

                            SqlParameter provincia = new SqlParameter("@provincia", indirizzoCollaboratore.Provincia);
                            provincia.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(provincia);

                            SqlParameter nazione = new SqlParameter("@nazione", indirizzoCollaboratore.Nazione);
                            nazione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nazione);

                            SqlParameter descrizione = new SqlParameter("@descrizione", indirizzoCollaboratore.Descrizione);
                            descrizione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(descrizione);

                            SqlParameter attivo = new SqlParameter("@attivo", indirizzoCollaboratore.Attivo);
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
                esito.descrizione = "Anag_Indirizzi_Collaboratori_DAL.cs - CreaIndirizziCollaboratore " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return 0;
        }

        public Esito AggiornaIndirizziCollaboratore(Anag_Indirizzi_Collaboratori indirizzoCollaboratore)
        {
            Esito esito = new Esito();
            try
            {
                using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(sqlConstr))
                {
                    using (System.Data.SqlClient.SqlCommand StoreProc = new System.Data.SqlClient.SqlCommand("UpdateIndirizziCollaboratore"))
                    {
                        using (System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            System.Data.SqlClient.SqlParameter id = new System.Data.SqlClient.SqlParameter("@id", indirizzoCollaboratore.Id);
                            id.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id);

                            SqlParameter id_collaboratore = new SqlParameter("@id_collaboratore", indirizzoCollaboratore.Id_Collaboratore);
                            id_collaboratore.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_collaboratore);

                            SqlParameter priorita = new SqlParameter("@priorita", indirizzoCollaboratore.Priorita);
                            priorita.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(priorita);

                            SqlParameter tipo = new SqlParameter("@tipo", indirizzoCollaboratore.Tipo);
                            tipo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(tipo);

                            SqlParameter indirizzo = new SqlParameter("@indirizzo", indirizzoCollaboratore.Indirizzo);
                            indirizzo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(indirizzo);

                            SqlParameter numeroCivico = new SqlParameter("@numeroCivico", indirizzoCollaboratore.NumeroCivico);
                            numeroCivico.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(numeroCivico);

                            SqlParameter cap = new SqlParameter("@cap", indirizzoCollaboratore.Cap);
                            cap.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(cap);

                            SqlParameter comune = new SqlParameter("@comune", indirizzoCollaboratore.Comune);
                            comune.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(comune);

                            SqlParameter provincia = new SqlParameter("@provincia", indirizzoCollaboratore.Provincia);
                            provincia.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(provincia);

                            SqlParameter nazione = new SqlParameter("@nazione", indirizzoCollaboratore.Nazione);
                            nazione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nazione);

                            SqlParameter descrizione = new SqlParameter("@descrizione", indirizzoCollaboratore.Descrizione);
                            descrizione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(descrizione);

                            SqlParameter attivo = new SqlParameter("@attivo", indirizzoCollaboratore.Attivo);
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
                esito.descrizione = "Anag_Indirizzi_Collaboratori_DAL.cs - AggiornaIndirizziCollaboratore " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

        public Esito EliminaIndirizzoCollaboratore(int idIndirizzoCollaboratore)
        {
            Esito esito = new Esito();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("DeleteIndirizziCollaboratore"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            SqlParameter id = new SqlParameter("@id", SqlDbType.Int);
                            id.Direction = ParameterDirection.Input;
                            id.Value = idIndirizzoCollaboratore;
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
                esito.descrizione = "Anag_Indirizzi_Collaboratori_DAL.cs - EliminaIndirizzoCollaboratore " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }


    }

}