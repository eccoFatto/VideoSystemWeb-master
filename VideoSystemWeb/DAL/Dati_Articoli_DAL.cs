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
    public class Dati_Articoli_DAL : Base_DAL
    {
        //singleton
        private static volatile Dati_Articoli_DAL instance;
        private static object objForLock = new Object();

        private Dati_Articoli_DAL() { }

        public static Dati_Articoli_DAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Dati_Articoli_DAL();
                    }
                }
                return instance;
            }
        }
        public List<DatiArticoli> getDatiArticoliByIdDatiAgenda(ref Esito esito, int idDatiAgenda)
        {
            List<DatiArticoli> listaDatiArticoli = new List<DatiArticoli>();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM dati_articoli WHERE idDatiAgenda = " + idDatiAgenda.ToString();
                    query += " ORDER BY idArtArticoli";
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

                                        DatiArticoli datiArticoli = new DatiArticoli();
                                        datiArticoli.Id = riga.Field<int>("id");
                                        datiArticoli.IdArtArticoli = riga.Field<int>("idArtArticoli");
                                        datiArticoli.IdDatiAgenda = riga.Field<int>("idDatiAgenda");
                                        datiArticoli.IdTipoGenere = riga.Field<int>("idTipoGenere");
                                        datiArticoli.IdTipoGruppo = riga.Field<int>("idTipoGruppo");
                                        datiArticoli.IdTipoSottogruppo = riga.Field<int>("idTipoSottogruppo");

                                        datiArticoli.Iva = riga.Field<int>("iva");

                                        datiArticoli.Prezzo = riga.Field<decimal>("prezzo");
                                        datiArticoli.Costo = riga.Field<decimal>("costo");

                                        datiArticoli.Stampa = riga.Field<bool>("stampa");

                                        listaDatiArticoli.Add(datiArticoli);
                                    }
                                }
                                else
                                {
                                    esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                    esito.descrizione = "Nessun dato trovato nella tabella dati_articoli ";
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

            return listaDatiArticoli;
        }

        public DatiArticoli getDatiArticoliById(ref Esito esito, int id)
        {
            DatiArticoli datiArticoli = new DatiArticoli();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM dati_articoli WHERE id = " + id.ToString();
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
                                    datiArticoli.Id = dt.Rows[0].Field<int>("id");
                                    datiArticoli.IdArtArticoli = dt.Rows[0].Field<int>("idArtArticoli");
                                    datiArticoli.IdDatiAgenda = dt.Rows[0].Field<int>("idDatiAgenda");
                                    datiArticoli.IdTipoGenere = dt.Rows[0].Field<int>("idTipoGenere");
                                    datiArticoli.IdTipoGruppo = dt.Rows[0].Field<int>("idTipoGruppo");
                                    datiArticoli.IdTipoSottogruppo = dt.Rows[0].Field<int>("idTipoSottogruppo");

                                    datiArticoli.Iva = dt.Rows[0].Field<int>("iva");

                                    datiArticoli.Prezzo = dt.Rows[0].Field<decimal>("prezzo");
                                    datiArticoli.Costo = dt.Rows[0].Field<decimal>("costo");

                                    datiArticoli.Stampa = dt.Rows[0].Field<bool>("stampa");

                                }
                                else
                                {
                                    esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                    esito.descrizione = "Nessun dato trovato nella tabella dati_articoli ";
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

            return datiArticoli;

        }

        public int CreaDatoArticolo(DatiArticoli datoArticolo, ref Esito esito)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("InsertDatiArticoli"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            StoreProc.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

                            SqlParameter costo = new SqlParameter("@costo", datoArticolo.Costo);
                            costo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(costo);

                            SqlParameter idArtArticoli = new SqlParameter("@idArtArticoli", datoArticolo.IdArtArticoli);
                            idArtArticoli.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idArtArticoli);

                            SqlParameter idDatiAgenda = new SqlParameter("@idDatiAgenda", datoArticolo.IdDatiAgenda);
                            idDatiAgenda.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idDatiAgenda);

                            SqlParameter idTipoGenere = new SqlParameter("@idTipoGenere", datoArticolo.IdTipoGenere);
                            idTipoGenere.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idTipoGenere);

                            SqlParameter idTipoGruppo = new SqlParameter("@idTipoGruppo", datoArticolo.IdTipoGruppo);
                            idTipoGruppo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idTipoGruppo);

                            SqlParameter idTipoSottogruppo = new SqlParameter("@idTipoSottogruppo", datoArticolo.IdTipoSottogruppo);
                            idTipoSottogruppo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idTipoSottogruppo);

                            SqlParameter iva = new SqlParameter("@iva", datoArticolo.Iva);
                            iva.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(iva);

                            SqlParameter prezzo = new SqlParameter("@prezzo", datoArticolo.Prezzo);
                            prezzo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(prezzo);

                            SqlParameter stampa = new SqlParameter("@stampa", datoArticolo.Stampa);
                            stampa.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(stampa);

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
                esito.descrizione = "Dati_Articoli_DAL.cs - CreaDatoArticolo " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return 0;
        }

        public Esito AggiornaDatoArticolo(DatiArticoli datoArticolo)
        {
            Esito esito = new Esito();
            try
            {
                using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(sqlConstr))
                {
                    using (System.Data.SqlClient.SqlCommand StoreProc = new System.Data.SqlClient.SqlCommand("UpdateDatiArticoli"))
                    {
                        using (System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            System.Data.SqlClient.SqlParameter id = new System.Data.SqlClient.SqlParameter("@id", datoArticolo.Id);
                            id.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id);

                            SqlParameter costo = new SqlParameter("@costo", datoArticolo.Costo);
                            costo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(costo);

                            SqlParameter idArtArticoli = new SqlParameter("@idArtArticoli", datoArticolo.IdArtArticoli);
                            idArtArticoli.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idArtArticoli);

                            SqlParameter idDatiAgenda = new SqlParameter("@idDatiAgenda", datoArticolo.IdDatiAgenda);
                            idDatiAgenda.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idDatiAgenda);

                            SqlParameter idTipoGenere = new SqlParameter("@idTipoGenere", datoArticolo.IdTipoGenere);
                            idTipoGenere.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idTipoGenere);

                            SqlParameter idTipoGruppo = new SqlParameter("@idTipoGruppo", datoArticolo.IdTipoGruppo);
                            idTipoGruppo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idTipoGruppo);

                            SqlParameter idTipoSottogruppo = new SqlParameter("@idTipoSottogruppo", datoArticolo.IdTipoSottogruppo);
                            idTipoSottogruppo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idTipoSottogruppo);

                            SqlParameter iva = new SqlParameter("@iva", datoArticolo.Iva);
                            iva.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(iva);

                            SqlParameter prezzo = new SqlParameter("@prezzo", datoArticolo.Prezzo);
                            prezzo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(prezzo);

                            SqlParameter stampa = new SqlParameter("@stampa", datoArticolo.Stampa);
                            stampa.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(stampa);

                            StoreProc.Connection.Open();

                            int iReturn = StoreProc.ExecuteNonQuery();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.descrizione = "Dati_Articoli_DAL.cs - AggiornaDatoArticolo " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

        public Esito EliminaDatoArticolo(int idDatoArticolo)
        {
            Esito esito = new Esito();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("DeleteDatiArticoli"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            SqlParameter id = new SqlParameter("@id", SqlDbType.Int);
                            id.Direction = ParameterDirection.Input;
                            id.Value = idDatoArticolo;
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
                esito.descrizione = "Dati_Articoli_DAL.cs - EliminaDatoArticolo " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

    }
}