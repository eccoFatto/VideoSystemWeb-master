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
    public class Art_Gruppi_Articoli_DAL : Base_DAL
    {
        //singleton
        private static volatile Art_Gruppi_Articoli_DAL instance;
        private static object objForLock = new Object();

        private Art_Gruppi_Articoli_DAL() { }

        public static Art_Gruppi_Articoli_DAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Art_Gruppi_Articoli_DAL();
                    }
                }
                return instance;
            }
        }

        public Art_Gruppi_Articoli getGruppiArticoliById(int idGruppoArticolo, ref Esito esito)
        {
            Art_Gruppi_Articoli gruppoArticolo = new Art_Gruppi_Articoli();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM art_gruppi_articoli where id = " + idGruppoArticolo.ToString();
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
                                    gruppoArticolo.Id = dt.Rows[0].Field<int>("id");
                                    gruppoArticolo.IdArtGruppi = dt.Rows[0].Field<int>("idArtGruppi");
                                    gruppoArticolo.IdArtArticoli = dt.Rows[0].Field<int>("idArtArticoli");

                                }
                                else
                                {
                                    esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                    esito.descrizione = "GruppoArticolo con id " + idGruppoArticolo.ToString() + " non trovato in tabella art_gruppi_articoli ";
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
            return gruppoArticolo;
        }

        public List<Art_Gruppi> getGruppiByIdArticolo(int idArticolo, ref Esito esito)
        {
            List<Art_Gruppi> listaGruppi = new List<Art_Gruppi>();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM art_gruppi_articoli where idArtArticoli = " + idArticolo.ToString();
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
                                        Art_Gruppi_Articoli gruppoArticolo = new Art_Gruppi_Articoli();
                                        gruppoArticolo.Id = riga.Field<int>("id");
                                        gruppoArticolo.IdArtGruppi = riga.Field<int>("idArtGruppi");
                                        gruppoArticolo.IdArtArticoli = riga.Field<int>("idArtArticoli");
                                        Art_Gruppi gruppo = Art_Gruppi_DAL.Instance.getGruppiById(gruppoArticolo.IdArtGruppi, ref esito);
                                        listaGruppi.Add(gruppo);
                                    }

                                }
                                else
                                {
                                    esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                    esito.descrizione = "GruppoArticolo con idArtArticoli " + idArticolo.ToString() + " non trovato in tabella art_gruppi_articoli ";
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
            return listaGruppi;
        }

        public List<Art_Articoli> getArticoliByIdGruppo(int idGruppo, ref Esito esito)
        {
            List<Art_Articoli> listaArticoli = new List<Art_Articoli>();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM art_gruppi_articoli where idArtGruppi = " + idGruppo.ToString();
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
                                        Art_Gruppi_Articoli gruppoArticolo = new Art_Gruppi_Articoli();
                                        gruppoArticolo.Id = riga.Field<int>("id");
                                        gruppoArticolo.IdArtGruppi = riga.Field<int>("idArtGruppi");
                                        gruppoArticolo.IdArtArticoli = riga.Field<int>("idArtArticoli");
                                        Art_Articoli articolo = Art_Articoli_DAL.Instance.getArticoloById(gruppoArticolo.IdArtArticoli, ref esito);
                                        listaArticoli.Add(articolo);
                                    }

                                }
                                else
                                {
                                    esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                    esito.descrizione = "GruppoArticolo con idArtGruppo " + idGruppo.ToString() + " non trovato in tabella art_gruppi_articoli ";
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
            return listaArticoli;
        }


        public List<Art_Gruppi_Articoli> CaricaListaGruppiArticoli(ref Esito esito)
        {
            List<Art_Gruppi_Articoli> listaGruppiArticoli = new List<Art_Gruppi_Articoli>();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM art_gruppi_articoli";
                    query += " ORDER BY id";
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
                                        Art_Gruppi_Articoli gruppoArticolo = new Art_Gruppi_Articoli();
                                        gruppoArticolo.Id = riga.Field<int>("id");
                                        gruppoArticolo.IdArtArticoli = riga.Field<int>("idArtArticoli");
                                        gruppoArticolo.IdArtGruppi = riga.Field<int>("idArtGruppi");

                                        listaGruppiArticoli.Add(gruppoArticolo);
                                    }
                                }
                                else
                                {
                                    esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                    esito.descrizione = "Nessun dato trovato nella tabella art_gruppi_articoli ";
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

            return listaGruppiArticoli;
        }

        public int CreaGruppoArticolo(Art_Gruppi_Articoli gruppoArticolo, ref Esito esito)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("InsertArtGruppiArticoli"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            StoreProc.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

                            SqlParameter idArtArticoli = new SqlParameter("@idArtArticoli", gruppoArticolo.IdArtArticoli);
                            idArtArticoli.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idArtArticoli);

                            SqlParameter idArtGruppi = new SqlParameter("@idArtGruppi", gruppoArticolo.IdArtGruppi);
                            idArtGruppi.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idArtGruppi);

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
                esito.descrizione = "Art_Gruppi_Articoli_DAL.cs - CreaGruppoArticolo " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return 0;
        }

        public Esito AggiornaGruppoArticolo(Art_Gruppi_Articoli gruppoArticolo)
        {
            Esito esito = new Esito();
            try
            {
                using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(sqlConstr))
                {
                    using (System.Data.SqlClient.SqlCommand StoreProc = new System.Data.SqlClient.SqlCommand("UpdateArtGruppiArticoli"))
                    {
                        using (System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            System.Data.SqlClient.SqlParameter id = new System.Data.SqlClient.SqlParameter("@id", gruppoArticolo.Id);
                            id.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id);

                            SqlParameter idArtArticoli = new SqlParameter("@idArtArticoli", gruppoArticolo.IdArtArticoli);
                            idArtArticoli.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idArtArticoli);

                            SqlParameter idArtGruppi = new SqlParameter("@idArtGruppi", gruppoArticolo.IdArtGruppi);
                            idArtGruppi.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idArtGruppi);

                            StoreProc.Connection.Open();

                            int iReturn = StoreProc.ExecuteNonQuery();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.descrizione = "Art_Gruppi_Articoli_DAL.cs - AggiornaGruppoArticolo " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

        public Esito EliminaGruppoArticolo(int idGruppoArticolo)
        {
            Esito esito = new Esito();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("DeleteArtGruppiArticoli"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            SqlParameter id = new SqlParameter("@id", SqlDbType.Int);
                            id.Direction = ParameterDirection.Input;
                            id.Value = idGruppoArticolo;
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
                esito.descrizione = "Art_Gruppi_Articoli_DAL.cs - EliminaGruppoArticolo " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }
    }
}