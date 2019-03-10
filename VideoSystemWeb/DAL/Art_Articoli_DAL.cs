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
    public class Art_Articoli_DAL : Base_DAL
    {
        //singleton
        private static volatile Art_Articoli_DAL instance;
        private static object objForLock = new Object();

        private Art_Articoli_DAL() { }

        public static Art_Articoli_DAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Art_Articoli_DAL();
                    }
                }
                return instance;
            }
        }

        public Art_Articoli getArticoloById(int idArticolo, ref Esito esito)
        {
            Art_Articoli articolo = new Art_Articoli();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM art_articoli where id = " + idArticolo.ToString();
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
                                    articolo.Id = dt.Rows[0].Field<int>("id");
                                    articolo.DefaultIdTipoGenere = dt.Rows[0].Field<int>("defaultIdTipoGenere");
                                    articolo.DefaultIdTipoGruppo = dt.Rows[0].Field<int>("defaultIdTipoGruppo");
                                    articolo.DefaultIdTipoSottogruppo = dt.Rows[0].Field<int>("defaultIdTipoSottogruppo");
                                    articolo.DefaultCosto = dt.Rows[0].Field<decimal>("defaultCosto");
                                    articolo.DefaultIva = dt.Rows[0].Field<int>("defaultIva");
                                    articolo.DefaultPrezzo = dt.Rows[0].Field<decimal>("defaultPrezzo");
                                    articolo.DefaultStampa = dt.Rows[0].Field<bool>("defaultStampa");
                                    articolo.DefaultDescrizione = dt.Rows[0].Field<string>("defaultDescrizione");
                                    articolo.DefaultDescrizioneLunga = dt.Rows[0].Field<string>("defaultDescrizioneLunga");
                                    articolo.Note = dt.Rows[0].Field<string>("Note");
                                    articolo.Attivo = dt.Rows[0].Field<bool>("attivo");

                                    articolo.DefaultTipoGenere = getTipologicaById(EnumTipologiche.TIPO_GENERE, articolo.DefaultIdTipoGenere, ref esito);
                                    articolo.DefaultTipoGruppo = getTipologicaById(EnumTipologiche.TIPO_GRUPPO, articolo.DefaultIdTipoGruppo, ref esito);
                                    articolo.DefaultTipoSottogruppo = getTipologicaById(EnumTipologiche.TIPO_SOTTOGRUPPO, articolo.DefaultIdTipoSottogruppo, ref esito);

                                }
                                else
                                {
                                    esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                    esito.descrizione = "Articolo con id " + idArticolo.ToString() + " non trovato in tabella art_articoli ";
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
            return articolo;
        }

        public List<Art_Articoli> CaricaListaArticoli(ref Esito esito, bool soloAttivi = true)
        {
            List<Art_Articoli> listaArticoli = new List<Art_Articoli>();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM art_articoli";
                    if (soloAttivi) query += " WHERE ATTIVO = 1";
                    query += " ORDER BY defaultDescrizioneLunga";
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
                                        Art_Articoli articolo = new Art_Articoli();
                                        articolo.Id = riga.Field<int>("id");
                                        articolo.DefaultCosto = riga.Field<decimal>("defaultCosto");
                                        articolo.DefaultIva = riga.Field<int>("defaultIva");
                                        articolo.DefaultPrezzo = riga.Field<decimal>("defaultPrezzo");
                                        articolo.DefaultStampa = riga.Field<bool>("defaultStampa");
                                        articolo.DefaultDescrizione = riga.Field<string>("defaultDescrizione");
                                        articolo.DefaultDescrizioneLunga = riga.Field<string>("defaultDescrizioneLunga");
                                        articolo.Attivo = riga.Field<bool>("attivo");

                                        listaArticoli.Add(articolo);
                                    }
                                }
                                else
                                {
                                    esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                    esito.descrizione = "Nessun dato trovato nella tabella art_articoli ";
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

        public int CreaArticolo(Art_Articoli articolo, ref Esito esito)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("InsertArtArticoli"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            StoreProc.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

                            SqlParameter attivo = new SqlParameter("@attivo", articolo.Attivo);
                            attivo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(attivo);

                            SqlParameter defaultCosto = new SqlParameter("@defaultCosto", articolo.DefaultCosto);
                            defaultCosto.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(defaultCosto);

                            SqlParameter defaultIva = new SqlParameter("@defaultIva", articolo.DefaultIva);
                            defaultIva.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(defaultIva);

                            SqlParameter defaultPrezzo = new SqlParameter("@defaultPrezzo", articolo.DefaultPrezzo);
                            defaultPrezzo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(defaultPrezzo);

                            SqlParameter defaultStampa = new SqlParameter("@defaultStampa", articolo.DefaultStampa);
                            defaultStampa.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(defaultStampa);

                            SqlParameter defaultDescrizione = new SqlParameter("@defaultDescrizione", articolo.DefaultDescrizione);
                            defaultDescrizione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(defaultDescrizione);

                            SqlParameter defaultDescrizioneLunga = new SqlParameter("@defaultDescrizioneLunga", articolo.DefaultDescrizioneLunga);
                            defaultDescrizioneLunga.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(defaultDescrizioneLunga);

                            SqlParameter defaultIdTipoGenere = new SqlParameter("@defaultIdTipoGenere", articolo.DefaultIdTipoGenere);
                            defaultIdTipoGenere.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(defaultIdTipoGenere);

                            SqlParameter defaultIdTipoGruppo = new SqlParameter("@defaultIdTipoGruppo", articolo.DefaultIdTipoGruppo);
                            defaultIdTipoGruppo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(defaultIdTipoGruppo);

                            SqlParameter defaultIdTipoSottogruppo = new SqlParameter("@defaultIdTipoSottogruppo", articolo.DefaultIdTipoSottogruppo);
                            defaultIdTipoSottogruppo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(defaultIdTipoSottogruppo);

                            SqlParameter note = new SqlParameter("@note", articolo.Note);
                            note.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(note);

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
                esito.descrizione = "Art_Articoli_DAL.cs - CreaArticolo " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return 0;
        }

        public Esito AggiornaArticolo(Art_Articoli articolo)
        {
            Esito esito = new Esito();
            try
            {
                using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(sqlConstr))
                {
                    using (System.Data.SqlClient.SqlCommand StoreProc = new System.Data.SqlClient.SqlCommand("UpdateArtArticoli"))
                    {
                        using (System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            System.Data.SqlClient.SqlParameter id = new System.Data.SqlClient.SqlParameter("@id", articolo.Id);
                            id.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id);

                            SqlParameter attivo = new SqlParameter("@attivo", articolo.Attivo);
                            attivo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(attivo);

                            SqlParameter defaultCosto = new SqlParameter("@defaultCosto", articolo.DefaultCosto);
                            defaultCosto.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(defaultCosto);

                            SqlParameter defaultIva = new SqlParameter("@defaultIva", articolo.DefaultIva);
                            defaultIva.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(defaultIva);

                            SqlParameter defaultPrezzo = new SqlParameter("@defaultPrezzo", articolo.DefaultPrezzo);
                            defaultPrezzo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(defaultPrezzo);

                            SqlParameter defaultStampa = new SqlParameter("@defaultStampa", articolo.DefaultStampa);
                            defaultStampa.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(defaultStampa);

                            SqlParameter defaultDescrizione = new SqlParameter("@defaultDescrizione", articolo.DefaultDescrizione);
                            defaultDescrizione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(defaultDescrizione);

                            SqlParameter defaultDescrizioneLunga = new SqlParameter("@defaultDescrizioneLunga", articolo.DefaultDescrizioneLunga);
                            defaultDescrizioneLunga.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(defaultDescrizioneLunga);

                            SqlParameter defaultIdTipoGenere = new SqlParameter("@defaultIdTipoGenere", articolo.DefaultIdTipoGenere);
                            defaultIdTipoGenere.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(defaultIdTipoGenere);

                            SqlParameter defaultIdTipoGruppo = new SqlParameter("@defaultIdTipoGruppo", articolo.DefaultIdTipoGruppo);
                            defaultIdTipoGruppo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(defaultIdTipoGruppo);

                            SqlParameter defaultIdTipoSottogruppo = new SqlParameter("@defaultIdTipoSottogruppo", articolo.DefaultIdTipoSottogruppo);
                            defaultIdTipoSottogruppo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(defaultIdTipoSottogruppo);

                            SqlParameter note = new SqlParameter("@note", articolo.Note);
                            note.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(note);

                            StoreProc.Connection.Open();

                            int iReturn = StoreProc.ExecuteNonQuery();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.descrizione = "Art_Articoli_DAL.cs - aggiornaArticolo " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

        public Esito EliminaArticolo(int idArticolo)
        {
            Esito esito = new Esito();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("DeleteArtArticoli"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            SqlParameter id = new SqlParameter("@id", SqlDbType.Int);
                            id.Direction = ParameterDirection.Input;
                            id.Value = idArticolo;
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
                esito.descrizione = "Art_Articoli_DAL.cs - EliminaArticolo " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }
    }
}