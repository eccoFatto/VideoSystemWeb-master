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
    public class Art_Gruppi_DAL : Base_DAL
    {
        //singleton
        private static volatile Art_Gruppi_DAL instance;
        private static object objForLock = new Object();

        private Art_Gruppi_DAL() { }

        public static Art_Gruppi_DAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Art_Gruppi_DAL();
                    }
                }
                return instance;
            }
        }

        public Art_Gruppi getGruppiById(int idGruppo, ref Esito esito)
        {
            Art_Gruppi gruppo = new Art_Gruppi();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM art_gruppi where id = " + idGruppo.ToString();
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
                                    gruppo.Id = dt.Rows[0].Field<int>("id");
                                    gruppo.Attivo = dt.Rows[0].Field<bool>("attivo");
                                    gruppo.Descrizione = dt.Rows[0].Field<string>("descrizione");
                                    gruppo.Nome = dt.Rows[0].Field<string>("nome");
                                    gruppo.Parametri = dt.Rows[0].Field<string>("parametri");
                                    gruppo.SottoTipo = dt.Rows[0].Field<string>("sottoTipo");

                                }
                                else
                                {
                                    esito.Codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                    esito.Descrizione = "Gruppo con id " + idGruppo.ToString() + " non trovato in tabella art_gruppi ";
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
            return gruppo;
        }

        public List<Art_Gruppi> CaricaListaGruppi(ref Esito esito, bool soloAttivi = true)
        {
            List<Art_Gruppi> listaGruppi = new List<Art_Gruppi>();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM art_gruppi";
                    if (soloAttivi) query += " WHERE ATTIVO = 1";
                    query += " ORDER BY nome";
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
                                        Art_Gruppi gruppo = new Art_Gruppi();
                                        gruppo.Id = riga.Field<int>("id");
                                        gruppo.Attivo = riga.Field<bool>("attivo");
                                        gruppo.Descrizione = riga.Field<string>("descrizione");
                                        gruppo.Nome = riga.Field<string>("nome");
                                        gruppo.Parametri = riga.Field<string>("parametri");
                                        gruppo.SottoTipo = riga.Field<string>("sottoTipo");

                                        listaGruppi.Add(gruppo);
                                    }
                                }
                                else
                                {
                                    esito.Codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                    esito.Descrizione = "Nessun dato trovato nella tabella art_gruppi ";
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

            return listaGruppi;
        }

        public int CreaGruppo(Art_Gruppi gruppo, Anag_Utenti utente, ref Esito esito)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("InsertArtGruppi"))
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

                            SqlParameter attivo = new SqlParameter("@attivo", gruppo.Attivo);
                            attivo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(attivo);

                            SqlParameter descrizione = new SqlParameter("@descrizione", gruppo.Descrizione);
                            descrizione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(descrizione);

                            SqlParameter nome = new SqlParameter("@nome", gruppo.Nome);
                            nome.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome);

                            SqlParameter parametri = new SqlParameter("@parametri", gruppo.Parametri);
                            parametri.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(parametri);

                            SqlParameter sottoTipo = new SqlParameter("@sottoTipo", gruppo.SottoTipo);
                            sottoTipo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(sottoTipo);

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
                esito.Descrizione = "Art_Gruppi_DAL.cs - CreaGruppo " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return 0;
        }

        public Esito AggiornaGruppo(Art_Gruppi gruppo, Anag_Utenti utente)
        {
            Esito esito = new Esito();
            try
            {
                using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(sqlConstr))
                {
                    using (System.Data.SqlClient.SqlCommand StoreProc = new System.Data.SqlClient.SqlCommand("UpdateArtGruppi"))
                    {
                        using (System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            System.Data.SqlClient.SqlParameter id = new System.Data.SqlClient.SqlParameter("@id", gruppo.Id);
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

                            SqlParameter attivo = new SqlParameter("@attivo", gruppo.Attivo);
                            attivo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(attivo);

                            SqlParameter descrizione = new SqlParameter("@descrizione", gruppo.Descrizione);
                            descrizione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(descrizione);

                            SqlParameter nome = new SqlParameter("@nome", gruppo.Nome);
                            nome.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome);

                            SqlParameter parametri = new SqlParameter("@parametri", gruppo.Parametri);
                            parametri.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(parametri);

                            SqlParameter sottoTipo = new SqlParameter("@sottoTipo", gruppo.SottoTipo);
                            sottoTipo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(sottoTipo);

                            StoreProc.Connection.Open();

                            int iReturn = StoreProc.ExecuteNonQuery();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.Descrizione = "Art_Gruppi_DAL.cs - AggiornaGruppo " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

        public Esito EliminaGruppo(int idGruppo, Anag_Utenti utente)
        {
            Esito esito = new Esito();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("DeleteArtGruppi"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            SqlParameter id = new SqlParameter("@id", SqlDbType.Int);
                            id.Direction = ParameterDirection.Input;
                            id.Value = idGruppo;
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
                esito.Descrizione = "Art_Gruppi_DAL.cs - EliminaGruppo " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }
        public Esito RemoveGruppo(int idGruppo)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);
            Esito esito = new Esito();
            try
            {
                using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(sqlConstr))
                {
                    using (System.Data.SqlClient.SqlCommand StoreProc = new System.Data.SqlClient.SqlCommand("RemoveArtGruppi"))
                    {
                        using (System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            System.Data.SqlClient.SqlParameter id = new System.Data.SqlClient.SqlParameter("@id", idGruppo);
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


                            StoreProc.Connection.Open();

                            int iReturn = StoreProc.ExecuteNonQuery();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.Descrizione = "Art_Gruppi_DAL.cs - RemoveGruppo " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

    }
}