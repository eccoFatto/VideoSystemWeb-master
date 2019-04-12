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
    public class Config_DAL : Base_DAL
    {
        //singleton
        private static volatile Config_DAL instance;
        private static object objForLock = new Object();

        private Config_DAL() { }

        public static Config_DAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Config_DAL();
                    }
                }
                return instance;
            }
        }
        public List<Config> getListaConfig(ref Esito esito)
        {
            List<Config> listaConfig = new List<Config>();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM tab_config";
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
                                        Config config = new Config();
                                        config.Chiave = riga.Field<string>("chiave");
                                        config.Valore = riga.Field<string>("valore");
                                        config.Descrizione = riga.Field<string>("descrizione");
                                        listaConfig.Add(config);
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

            return listaConfig;
        }

        public Config getConfig(ref Esito esito, string chiave)
        {
            Config config = new Config();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM tab_config WHERE chiave = " + chiave;
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
                                    config.Chiave = dt.Rows[0].Field<string>("chiave");
                                    config.Valore = dt.Rows[0].Field<string>("valore");
                                    config.Descrizione = dt.Rows[0].Field<string>("descrizione");
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

            return config;

        }
        public Esito CreaConfig(Config config)
        {
            Esito esito = new Esito();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("InsertConfig"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;


                            SqlParameter chiave = new SqlParameter("@chiave", config.Chiave);
                            chiave.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(chiave);

                            SqlParameter valore = new SqlParameter("@valore", config.Valore);
                            valore.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(valore);

                            SqlParameter descrizione = new SqlParameter("@descrizione", config.Descrizione);
                            descrizione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(descrizione);

                            StoreProc.Connection.Open();

                            StoreProc.ExecuteNonQuery();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.descrizione = "Config_DAL.cs - CreaConfig " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

        public Esito AggiornaConfig(Config config)
        {
            Esito esito = new Esito();
            try
            {
                using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(sqlConstr))
                {
                    using (System.Data.SqlClient.SqlCommand StoreProc = new System.Data.SqlClient.SqlCommand("UpdateConfig"))
                    {
                        using (System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            System.Data.SqlClient.SqlParameter key = new System.Data.SqlClient.SqlParameter("@key", config.Chiave);
                            key.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(key);

                            SqlParameter chiave = new SqlParameter("@chiave", config.Chiave);
                            chiave.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(chiave);

                            SqlParameter valore = new SqlParameter("@valore", config.Valore);
                            valore.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(valore);

                            SqlParameter descrizione = new SqlParameter("@descrizione", config.Descrizione);
                            descrizione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(descrizione);

                            StoreProc.Connection.Open();

                            int iReturn = StoreProc.ExecuteNonQuery();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.descrizione = "Config_DAL.cs - AggiornaConfig " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

        public Esito EliminaConfig(string chiave)
        {
            Esito esito = new Esito();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("DeleteConfig"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            SqlParameter key = new SqlParameter("@chiave", SqlDbType.VarChar);
                            key.Direction = ParameterDirection.Input;
                            key.Value = chiave;
                            StoreProc.Parameters.Add(key);

                            StoreProc.Connection.Open();

                            int iReturn = StoreProc.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.descrizione = "Config_DAL.cs - EliminaConfig " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }
    }
}