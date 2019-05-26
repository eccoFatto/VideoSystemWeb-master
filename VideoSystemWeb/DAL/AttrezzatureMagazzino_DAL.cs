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
    public class AttrezzatureMagazzino_DAL : Base_DAL
    {
        //singleton
        private static volatile AttrezzatureMagazzino_DAL instance;
        private static object objForLock = new Object();

        private AttrezzatureMagazzino_DAL() { }

        public static AttrezzatureMagazzino_DAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new AttrezzatureMagazzino_DAL();
                    }
                }
                return instance;
            }
        }
        public List<AttrezzatureMagazzino> getAttrezzatureMagazzino(ref Esito esito)
        {
            List<AttrezzatureMagazzino> listaAttrezzature = new List<AttrezzatureMagazzino>();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM mag_attrezzature";
                    query += " ORDER BY marca,modello,descrizione";
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
                                        AttrezzatureMagazzino attrezzature = new AttrezzatureMagazzino();
                                        attrezzature.Id = riga.Field<int>("id");
                                        attrezzature.Cod_vs = riga.Field<string>("cod_vs");
                                        attrezzature.Data_acquisto = riga.Field<DateTime>("data_acquisto");
                                        attrezzature.Descrizione = riga.Field<string>("descrizione");
                                        attrezzature.Disponibile = riga.Field<bool>("disponibile");
                                        attrezzature.Garanzia = riga.Field<bool>("garanzia");
                                        attrezzature.Id_categoria = riga.Field<int>("id_categoria");
                                        attrezzature.Id_subcategoria = riga.Field<int>("id_subcategoria");
                                        attrezzature.Marca = riga.Field<string>("marca");
                                        attrezzature.Modello = riga.Field<string>("modello");
                                        attrezzature.Note = riga.Field<string>("note");
                                        attrezzature.Seriale = riga.Field<string>("seriale");
                                        listaAttrezzature.Add(attrezzature);
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

            return listaAttrezzature;
        }


        public AttrezzatureMagazzino getAttrezzaturaById(ref Esito esito, int id)
        {
            AttrezzatureMagazzino attrezzatura = new AttrezzatureMagazzino();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM mag_attrezzature WHERE id = " + id.ToString();
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
                                    attrezzatura.Cod_vs = dt.Rows[0].Field<string>("cod_vs");
                                    attrezzatura.Data_acquisto = dt.Rows[0].Field<DateTime>("data_acquisto");
                                    attrezzatura.Id = dt.Rows[0].Field<int>("id");
                                    attrezzatura.Descrizione = dt.Rows[0].Field<string>("descrizione");
                                    attrezzatura.Disponibile = dt.Rows[0].Field<bool>("disponibile");
                                    attrezzatura.Garanzia = dt.Rows[0].Field<bool>("garanzia");
                                    attrezzatura.Id_categoria = dt.Rows[0].Field<int>("id_categoria");
                                    attrezzatura.Id_subcategoria = dt.Rows[0].Field<int>("id_subcategoria");
                                    attrezzatura.Marca = dt.Rows[0].Field<string>("marca");
                                    attrezzatura.Modello = dt.Rows[0].Field<string>("modello");
                                    attrezzatura.Note = dt.Rows[0].Field<string>("note");
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

            return attrezzatura;

        }
        public int CreaAttrezzatura(AttrezzatureMagazzino attrezzatura, ref Esito esito)
        {

            try
            {
                Anag_Utenti utente = (Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE];
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("InsertAttrezzatura"))
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

                            SqlParameter Cod_vs = new SqlParameter("@Cod_vs", attrezzatura.Cod_vs);
                            Cod_vs.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(Cod_vs);

                            SqlParameter Data_acquisto = new SqlParameter("@Data_acquisto", attrezzatura.Data_acquisto);
                            Data_acquisto.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(Data_acquisto);

                            SqlParameter Descrizione = new SqlParameter("@Descrizione", attrezzatura.Descrizione);
                            Descrizione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(Descrizione);

                            SqlParameter Disponibile = new SqlParameter("@Disponibile", attrezzatura.Disponibile);
                            Disponibile.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(Disponibile);

                            SqlParameter Garanzia = new SqlParameter("@Garanzia", attrezzatura.Garanzia);
                            Garanzia.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(Garanzia);

                            SqlParameter Id_categoria = new SqlParameter("@Id_categoria", attrezzatura.Id_categoria);
                            Id_categoria.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(Id_categoria);

                            SqlParameter Id_posizione_magazzino = new SqlParameter("@Id_posizione_magazzino", attrezzatura.Id_posizione_magazzino);
                            Id_posizione_magazzino.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(Id_posizione_magazzino);

                            SqlParameter Id_subcategoria = new SqlParameter("@Id_subcategoria", attrezzatura.Id_subcategoria);
                            Id_subcategoria.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(Id_subcategoria);

                            SqlParameter Marca = new SqlParameter("@Marca", attrezzatura.Marca);
                            Marca.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(Marca);

                            SqlParameter Modello = new SqlParameter("@Modello", attrezzatura.Modello);
                            Modello.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(Modello);

                            SqlParameter Note = new SqlParameter("@Note", attrezzatura.Note);
                            Note.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(Note);

                            SqlParameter Seriale = new SqlParameter("@Seriale", attrezzatura.Seriale);
                            Seriale.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(Seriale);

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
                esito.descrizione = "AttrezzatureMagazzino_DAL.cs - CreaAttrezzatura " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return 0;
        }

        public Esito AggiornaAttrezzatura(AttrezzatureMagazzino attrezzatura)
        {
            Anag_Utenti utente = (Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE];
            Esito esito = new Esito();
            try
            {
                using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(sqlConstr))
                {
                    using (System.Data.SqlClient.SqlCommand StoreProc = new System.Data.SqlClient.SqlCommand("UpdateAttrezzatura"))
                    {
                        using (System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            System.Data.SqlClient.SqlParameter id = new System.Data.SqlClient.SqlParameter("@id", attrezzatura.Id);
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

                            SqlParameter Cod_vs = new SqlParameter("@Cod_vs", attrezzatura.Cod_vs);
                            Cod_vs.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(Cod_vs);

                            SqlParameter Data_acquisto = new SqlParameter("@Data_acquisto", attrezzatura.Data_acquisto);
                            Data_acquisto.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(Data_acquisto);

                            SqlParameter Descrizione = new SqlParameter("@Descrizione", attrezzatura.Descrizione);
                            Descrizione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(Descrizione);

                            SqlParameter Disponibile = new SqlParameter("@Disponibile", attrezzatura.Disponibile);
                            Disponibile.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(Disponibile);

                            SqlParameter Garanzia = new SqlParameter("@Garanzia", attrezzatura.Garanzia);
                            Garanzia.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(Garanzia);

                            SqlParameter Id_categoria = new SqlParameter("@Id_categoria", attrezzatura.Id_categoria);
                            Id_categoria.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(Id_categoria);

                            SqlParameter Id_posizione_magazzino = new SqlParameter("@Id_posizione_magazzino", attrezzatura.Id_posizione_magazzino);
                            Id_posizione_magazzino.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(Id_posizione_magazzino);

                            SqlParameter Id_subcategoria = new SqlParameter("@Id_subcategoria", attrezzatura.Id_subcategoria);
                            Id_subcategoria.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(Id_subcategoria);

                            SqlParameter Marca = new SqlParameter("@Marca", attrezzatura.Marca);
                            Marca.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(Marca);

                            SqlParameter Modello = new SqlParameter("@Modello", attrezzatura.Modello);
                            Modello.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(Modello);

                            SqlParameter Note = new SqlParameter("@Note", attrezzatura.Note);
                            Note.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(Note);

                            SqlParameter Seriale = new SqlParameter("@Seriale", attrezzatura.Seriale);
                            Seriale.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(Seriale);

                            StoreProc.Connection.Open();

                            int iReturn = StoreProc.ExecuteNonQuery();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.descrizione = "AttrezzatureMagazzino_DAL.cs - AggiornaAttrezzatura " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

        public Esito EliminaAttrezzatura(int idAttrezzatura)
        {
            Anag_Utenti utente = (Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE];
            Esito esito = new Esito();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("DeleteAttrezzatura"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            SqlParameter id = new SqlParameter("@id", SqlDbType.Int);
                            id.Direction = ParameterDirection.Input;
                            id.Value = idAttrezzatura;
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
                esito.descrizione = "AttrezzatureMagazzino_DAL.cs - EliminaAttrezzatura " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }
    }
}