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
    public class Anag_Documenti_Collaboratori_DAL : Base_DAL
    {
        //singleton
        private static volatile Anag_Documenti_Collaboratori_DAL instance;
        private static object objForLock = new Object();

        private Anag_Documenti_Collaboratori_DAL() { }

        public static Anag_Documenti_Collaboratori_DAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Anag_Documenti_Collaboratori_DAL();
                    }
                }
                return instance;
            }
        }
        public List<Anag_Documenti_Collaboratori> getDocumentiByIdCollaboratore(ref Esito esito, int idCollaboratore, bool soloAttivi = true)
        {
            List<Anag_Documenti_Collaboratori> listaDocumenti = new List<Anag_Documenti_Collaboratori>();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM anag_documenti_collaboratori WHERE id_Collaboratore = " + idCollaboratore.ToString();
                    if (soloAttivi) query += " AND ATTIVO = 1";
                    query += " ORDER BY tipoDocumento,numeroDocumento";
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

                                        Anag_Documenti_Collaboratori documento = new Anag_Documenti_Collaboratori();
                                        documento.Id = riga.Field<int>("id");
                                        documento.Id_collaboratore = riga.Field<int>("id_collaboratore");

                                        documento.TipoDocumento = riga.Field<string>("tipoDocumento");
                                        documento.NumeroDocumento = riga.Field<string>("numeroDocumento");
                                        documento.PathDocumento = riga.Field<string>("pathDocumento");
                                        documento.Attivo = riga.Field<bool>("attivo");

                                        listaDocumenti.Add(documento);
                                    }
                                }
                                //else
                                //{
                                //    esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                //    esito.descrizione = "Nessun dato trovato nella tabella anag_documenti_collaboratori ";
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

            return listaDocumenti;
        }

        public Anag_Documenti_Collaboratori getDocumentoById(ref Esito esito, int id)
        {
            Anag_Documenti_Collaboratori documento = new Anag_Documenti_Collaboratori();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM anag_documenti_collaboratori WHERE id = " + id.ToString();
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
                                    documento.Id = dt.Rows[0].Field<int>("id");
                                    documento.Id_collaboratore = dt.Rows[0].Field<int>("id_collaboratore");

                                    documento.TipoDocumento = dt.Rows[0].Field<string>("tipoDocumento");
                                    documento.NumeroDocumento = dt.Rows[0].Field<string>("numeroDocumento");
                                    documento.PathDocumento = dt.Rows[0].Field<string>("pathDocumento");
                                    documento.Attivo = dt.Rows[0].Field<bool>("attivo");
                                }
                                else
                                {
                                    esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                    esito.descrizione = "Nessun dato trovato nella tabella anag_documenti_collaboratori ";
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

            return documento;

        }

        public int CreaDocumentoCollaboratore(Anag_Documenti_Collaboratori documentoCollaboratore, ref Esito esito)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("InsertDocumentiCollaboratore"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            StoreProc.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

                            SqlParameter id_collaboratore = new SqlParameter("@id_collaboratore", documentoCollaboratore.Id_collaboratore);
                            id_collaboratore.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_collaboratore);

                            SqlParameter tipoDocumento = new SqlParameter("@tipoDocumento", documentoCollaboratore.TipoDocumento);
                            tipoDocumento.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(tipoDocumento);

                            SqlParameter numeroDocumento = new SqlParameter("@numeroDocumento", documentoCollaboratore.NumeroDocumento);
                            numeroDocumento.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(numeroDocumento);

                            SqlParameter pathDocumento = new SqlParameter("@pathDocumento", documentoCollaboratore.PathDocumento);
                            pathDocumento.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(pathDocumento);

                            SqlParameter attivo = new SqlParameter("@attivo", documentoCollaboratore.Attivo);
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
                esito.descrizione = "Anag_Documenti_Collaboratori_DAL.cs - CreaDocumentoCollaboratore " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return 0;
        }

        public Esito AggiornaDocumentoCollaboratore(Anag_Documenti_Collaboratori documentoCollaboratore)
        {
            Esito esito = new Esito();
            try
            {
                using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(sqlConstr))
                {
                    using (System.Data.SqlClient.SqlCommand StoreProc = new System.Data.SqlClient.SqlCommand("UpdateDocumentiCollaboratore"))
                    {
                        using (System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            System.Data.SqlClient.SqlParameter id = new System.Data.SqlClient.SqlParameter("@id", documentoCollaboratore.Id);
                            id.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id);

                            SqlParameter id_collaboratore = new SqlParameter("@id_collaboratore", documentoCollaboratore.Id_collaboratore);
                            id_collaboratore.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_collaboratore);

                            SqlParameter tipoDocumento = new SqlParameter("@tipoDocumento", documentoCollaboratore.TipoDocumento);
                            tipoDocumento.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(tipoDocumento);

                            SqlParameter numeroDocumento = new SqlParameter("@numeroDocumento", documentoCollaboratore.NumeroDocumento);
                            numeroDocumento.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(numeroDocumento);

                            SqlParameter pathDocumento = new SqlParameter("@pathDocumento", documentoCollaboratore.PathDocumento);
                            pathDocumento.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(pathDocumento);

                            SqlParameter attivo = new SqlParameter("@attivo", documentoCollaboratore.Attivo);
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
                esito.descrizione = "Anag_Documenti_Collaboratori_DAL.cs - AggiornaDocumentoCollaboratore " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

        public Esito EliminaDocumentoCollaboratore(int idDocuementoCollaboratore)
        {
            Esito esito = new Esito();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("DeleteDocumentiCollaboratore"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            SqlParameter id = new SqlParameter("@id", SqlDbType.Int);
                            id.Direction = ParameterDirection.Input;
                            id.Value = idDocuementoCollaboratore;
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
                esito.descrizione = "Anag_Documenti_Collaboratori_DAL.cs - EliminaDocuementoCollaboratore " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

    }
}