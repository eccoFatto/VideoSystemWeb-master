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
    public class Dati_Lavorazione_Magazzino_DAL : Base_DAL
    {
        //singleton
        private static volatile Dati_Lavorazione_Magazzino_DAL instance;
        private static object objForLock = new Object();

        private Dati_Lavorazione_Magazzino_DAL() { }

        public static Dati_Lavorazione_Magazzino_DAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Dati_Lavorazione_Magazzino_DAL();
                    }
                }
                return instance;
            }
        }

        public DatiLavorazioneMagazzino getDatiLavorazioneMagazzinoById(int idDatiLavorazioneMagazzino, ref Esito esito)
        {
            DatiLavorazioneMagazzino datiLavorazioneMagazzino = new DatiLavorazioneMagazzino();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM dati_lavorazione_magazzino where id = " + idDatiLavorazioneMagazzino.ToString();
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
                                    datiLavorazioneMagazzino.Id = dt.Rows[0].Field<int>("id");
                                    datiLavorazioneMagazzino.Id_Lavorazione = dt.Rows[0].Field<int>("id_Lavorazione");
                                    datiLavorazioneMagazzino.Descrizione_Camera = dt.Rows[0].Field<string>("descrizione_Camera");

                                    datiLavorazioneMagazzino.Id_Altro1 = dt.Rows[0].Field<int>("id_Altro1");
                                    datiLavorazioneMagazzino.Id_Altro2 = dt.Rows[0].Field<int>("id_Altro2");
                                    datiLavorazioneMagazzino.Id_Camera = dt.Rows[0].Field<int>("id_Camera");
                                    datiLavorazioneMagazzino.Id_Cavalletto = dt.Rows[0].Field<int>("id_Cavalletto");
                                    datiLavorazioneMagazzino.Id_Cavi = dt.Rows[0].Field<int>("id_Cavi");
                                    datiLavorazioneMagazzino.Id_Fibra_Trax = dt.Rows[0].Field<int>("id_Fibra_Trax");
                                    datiLavorazioneMagazzino.Id_Lensholder = dt.Rows[0].Field<int>("id_Lensholder");
                                    datiLavorazioneMagazzino.Id_Loop = dt.Rows[0].Field<int>("id_Loop");
                                    datiLavorazioneMagazzino.Id_Mic = dt.Rows[0].Field<int>("id_Mic");
                                    datiLavorazioneMagazzino.Id_Ottica = dt.Rows[0].Field<int>("id_Ottica");
                                    datiLavorazioneMagazzino.Id_Testa = dt.Rows[0].Field<int>("id_Testa");
                                    datiLavorazioneMagazzino.Id_Viewfinder = dt.Rows[0].Field<int>("id_Viewfinder");

                                    datiLavorazioneMagazzino.Nome_Altro1 = dt.Rows[0].Field<string>("nome_Altro1");
                                    datiLavorazioneMagazzino.Nome_Altro2 = dt.Rows[0].Field<string>("nome_Altro2");
                                    datiLavorazioneMagazzino.Nome_Camera = dt.Rows[0].Field<string>("nome_Camera");
                                    datiLavorazioneMagazzino.Nome_Cavalletto = dt.Rows[0].Field<string>("nome_Cavalletto");
                                    datiLavorazioneMagazzino.Nome_Cavi = dt.Rows[0].Field<string>("nome_Cavi");
                                    datiLavorazioneMagazzino.Nome_Fibra_Trax = dt.Rows[0].Field<string>("nome_Fibra_Trax");
                                    datiLavorazioneMagazzino.Nome_Lensholder = dt.Rows[0].Field<string>("nome_Lensholder");
                                    datiLavorazioneMagazzino.Nome_Loop = dt.Rows[0].Field<string>("nome_Loop");
                                    datiLavorazioneMagazzino.Nome_Mic = dt.Rows[0].Field<string>("nome_Mic");
                                    datiLavorazioneMagazzino.Nome_Ottica = dt.Rows[0].Field<string>("nome_Ottica");
                                    datiLavorazioneMagazzino.Nome_Testa = dt.Rows[0].Field<string>("nome_Testa");
                                    datiLavorazioneMagazzino.Nome_Viewfinder = dt.Rows[0].Field<string>("nome_Viewfinder");

                                    datiLavorazioneMagazzino.Attivo = dt.Rows[0].Field<bool>("attivo");
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
            return datiLavorazioneMagazzino;
        }

         public int CreaDatiLavorazioneMagazzino(DatiLavorazioneMagazzino datiLavorazioneMagazzino, ref Esito esito)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("InsertDatiLavorazioneMagazzino"))
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

                            SqlParameter id_Lavorazione = new SqlParameter("@id_Lavorazione", datiLavorazioneMagazzino.Id_Lavorazione);
                            id_Lavorazione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Lavorazione);

                            SqlParameter descrizione_Camera = new SqlParameter("@descrizione_Camera", datiLavorazioneMagazzino.Descrizione_Camera);
                            descrizione_Camera.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(descrizione_Camera);

                            SqlParameter id_Altro1 = new SqlParameter("@id_Altro1", datiLavorazioneMagazzino.Id_Altro1);
                            id_Altro1.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Altro1);

                            SqlParameter id_Altro2 = new SqlParameter("@id_Altro2", datiLavorazioneMagazzino.Id_Altro2);
                            id_Altro2.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Altro2);

                            SqlParameter id_Camera = new SqlParameter("@id_Camera", datiLavorazioneMagazzino.Id_Camera);
                            id_Camera.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Camera);

                            SqlParameter id_Cavalletto = new SqlParameter("@id_Cavalletto", datiLavorazioneMagazzino.Id_Cavalletto);
                            id_Cavalletto.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Cavalletto);

                            SqlParameter id_Cavi = new SqlParameter("@id_Cavi", datiLavorazioneMagazzino.Id_Cavi);
                            id_Cavi.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Cavi);

                            SqlParameter id_Fibra_Trax = new SqlParameter("@id_Fibra_Trax", datiLavorazioneMagazzino.Id_Fibra_Trax);
                            id_Fibra_Trax.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Fibra_Trax);

                            SqlParameter id_Lensholder = new SqlParameter("@id_Lensholder", datiLavorazioneMagazzino.Id_Lensholder);
                            id_Lensholder.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Lensholder);

                            SqlParameter id_Loop = new SqlParameter("@id_Loop", datiLavorazioneMagazzino.Id_Loop);
                            id_Loop.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Loop);

                            SqlParameter id_Mic = new SqlParameter("@id_Mic", datiLavorazioneMagazzino.Id_Mic);
                            id_Mic.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Mic);

                            SqlParameter id_Ottica = new SqlParameter("@id_Ottica", datiLavorazioneMagazzino.Id_Ottica);
                            id_Ottica.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Ottica);

                            SqlParameter id_Testa = new SqlParameter("@id_Testa", datiLavorazioneMagazzino.Id_Testa);
                            id_Testa.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Testa);

                            SqlParameter id_Viewfinder = new SqlParameter("@id_Viewfinder", datiLavorazioneMagazzino.Id_Viewfinder);
                            id_Viewfinder.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Viewfinder);

                            SqlParameter nome_Altro1 = new SqlParameter("@nome_Altro1", datiLavorazioneMagazzino.Nome_Altro1);
                            nome_Altro1.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Altro1);

                            SqlParameter nome_Altro2 = new SqlParameter("@nome_Altro2", datiLavorazioneMagazzino.Nome_Altro2);
                            nome_Altro2.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Altro2);

                            SqlParameter nome_Camera = new SqlParameter("@nome_Camera", datiLavorazioneMagazzino.Nome_Camera);
                            nome_Camera.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Camera);

                            SqlParameter nome_Cavalletto = new SqlParameter("@nome_Cavalletto", datiLavorazioneMagazzino.Nome_Cavalletto);
                            nome_Cavalletto.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Cavalletto);

                            SqlParameter nome_Cavi = new SqlParameter("@nome_Cavi", datiLavorazioneMagazzino.Nome_Cavi);
                            nome_Cavi.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Cavi);

                            SqlParameter nome_Fibra_Trax = new SqlParameter("@nome_Fibra_Trax", datiLavorazioneMagazzino.Nome_Fibra_Trax);
                            nome_Fibra_Trax.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Fibra_Trax);

                            SqlParameter nome_Lensholder = new SqlParameter("@nome_Lensholder", datiLavorazioneMagazzino.Nome_Lensholder);
                            nome_Lensholder.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Lensholder);

                            SqlParameter nome_Loop = new SqlParameter("@nome_Loop", datiLavorazioneMagazzino.Nome_Loop);
                            nome_Loop.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Loop);

                            SqlParameter nome_Mic = new SqlParameter("@nome_Mic", datiLavorazioneMagazzino.Nome_Mic);
                            nome_Mic.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Mic);

                            SqlParameter nome_Ottica = new SqlParameter("@nome_Ottica", datiLavorazioneMagazzino.Nome_Ottica);
                            nome_Ottica.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Ottica);

                            SqlParameter nome_Testa = new SqlParameter("@nome_Testa", datiLavorazioneMagazzino.Nome_Testa);
                            nome_Testa.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Testa);

                            SqlParameter nome_Viewfinder = new SqlParameter("@nome_Viewfinder", datiLavorazioneMagazzino.Nome_Viewfinder);
                            nome_Viewfinder.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Viewfinder);

                            SqlParameter attivo = new SqlParameter("@attivo", datiLavorazioneMagazzino.Attivo);
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
                esito.Codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.Descrizione = "Dati_Lavorazione_Magazzino_DAL.cs - CreaDatiLavorazioneMagazzino " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return 0;
        }

        public Esito AggiornaDatiLavorazioneMagazzino(DatiLavorazioneMagazzino datiLavorazioneMagazzino)
        {
            Esito esito = new Esito();
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);
            try
            {
                using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(sqlConstr))
                {
                    using (System.Data.SqlClient.SqlCommand StoreProc = new System.Data.SqlClient.SqlCommand("UpdateDatiLavorazioneMagazzino"))
                    {
                        using (System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            System.Data.SqlClient.SqlParameter id = new System.Data.SqlClient.SqlParameter("@id", datiLavorazioneMagazzino.Id);
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

                            SqlParameter id_Lavorazione = new SqlParameter("@id_Lavorazione", datiLavorazioneMagazzino.Id_Lavorazione);
                            id_Lavorazione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Lavorazione);

                            SqlParameter descrizione_Camera = new SqlParameter("@descrizione_Camera", datiLavorazioneMagazzino.Descrizione_Camera);
                            descrizione_Camera.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(descrizione_Camera);

                            SqlParameter id_Altro1 = new SqlParameter("@id_Altro1", datiLavorazioneMagazzino.Id_Altro1);
                            id_Altro1.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Altro1);

                            SqlParameter id_Altro2 = new SqlParameter("@id_Altro2", datiLavorazioneMagazzino.Id_Altro2);
                            id_Altro2.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Altro2);

                            SqlParameter id_Camera = new SqlParameter("@id_Camera", datiLavorazioneMagazzino.Id_Camera);
                            id_Camera.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Camera);

                            SqlParameter id_Cavalletto = new SqlParameter("@id_Cavalletto", datiLavorazioneMagazzino.Id_Cavalletto);
                            id_Cavalletto.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Cavalletto);

                            SqlParameter id_Cavi = new SqlParameter("@id_Cavi", datiLavorazioneMagazzino.Id_Cavi);
                            id_Cavi.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Cavi);

                            SqlParameter id_Fibra_Trax = new SqlParameter("@id_Fibra_Trax", datiLavorazioneMagazzino.Id_Fibra_Trax);
                            id_Fibra_Trax.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Fibra_Trax);

                            SqlParameter id_Lensholder = new SqlParameter("@id_Lensholder", datiLavorazioneMagazzino.Id_Lensholder);
                            id_Lensholder.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Lensholder);

                            SqlParameter id_Loop = new SqlParameter("@id_Loop", datiLavorazioneMagazzino.Id_Loop);
                            id_Loop.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Loop);

                            SqlParameter id_Mic = new SqlParameter("@id_Mic", datiLavorazioneMagazzino.Id_Mic);
                            id_Mic.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Mic);

                            SqlParameter id_Ottica = new SqlParameter("@id_Ottica", datiLavorazioneMagazzino.Id_Ottica);
                            id_Ottica.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Ottica);

                            SqlParameter id_Testa = new SqlParameter("@id_Testa", datiLavorazioneMagazzino.Id_Testa);
                            id_Testa.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Testa);

                            SqlParameter id_Viewfinder = new SqlParameter("@id_Viewfinder", datiLavorazioneMagazzino.Id_Viewfinder);
                            id_Viewfinder.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Viewfinder);

                            SqlParameter nome_Altro1 = new SqlParameter("@nome_Altro1", datiLavorazioneMagazzino.Nome_Altro1);
                            nome_Altro1.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Altro1);

                            SqlParameter nome_Altro2 = new SqlParameter("@nome_Altro2", datiLavorazioneMagazzino.Nome_Altro2);
                            nome_Altro2.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Altro2);

                            SqlParameter nome_Camera = new SqlParameter("@nome_Camera", datiLavorazioneMagazzino.Nome_Camera);
                            nome_Camera.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Camera);

                            SqlParameter nome_Cavalletto = new SqlParameter("@nome_Cavalletto", datiLavorazioneMagazzino.Nome_Cavalletto);
                            nome_Cavalletto.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Cavalletto);

                            SqlParameter nome_Cavi = new SqlParameter("@nome_Cavi", datiLavorazioneMagazzino.Nome_Cavi);
                            nome_Cavi.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Cavi);

                            SqlParameter nome_Fibra_Trax = new SqlParameter("@nome_Fibra_Trax", datiLavorazioneMagazzino.Nome_Fibra_Trax);
                            nome_Fibra_Trax.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Fibra_Trax);

                            SqlParameter nome_Lensholder = new SqlParameter("@nome_Lensholder", datiLavorazioneMagazzino.Nome_Lensholder);
                            nome_Lensholder.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Lensholder);

                            SqlParameter nome_Loop = new SqlParameter("@nome_Loop", datiLavorazioneMagazzino.Nome_Loop);
                            nome_Loop.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Loop);

                            SqlParameter nome_Mic = new SqlParameter("@nome_Mic", datiLavorazioneMagazzino.Nome_Mic);
                            nome_Mic.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Mic);

                            SqlParameter nome_Ottica = new SqlParameter("@nome_Ottica", datiLavorazioneMagazzino.Nome_Ottica);
                            nome_Ottica.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Ottica);

                            SqlParameter nome_Testa = new SqlParameter("@nome_Testa", datiLavorazioneMagazzino.Nome_Testa);
                            nome_Testa.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Testa);

                            SqlParameter nome_Viewfinder = new SqlParameter("@nome_Viewfinder", datiLavorazioneMagazzino.Nome_Viewfinder);
                            nome_Viewfinder.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Viewfinder);

                            SqlParameter attivo = new SqlParameter("@attivo", datiLavorazioneMagazzino.Attivo);
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
                esito.Codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.Descrizione = "Dati_Lavorazione_Magazzino_DAL.cs - AggiornaDatiLavorazioneMagazzino " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

        public Esito EliminaDatiLavorazioneMagazzino(int idDatiLavorazioneMagazzino)
        {
            Esito esito = new Esito();
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("DeleteDatiLavorazioneMagazzino"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            SqlParameter id = new SqlParameter("@id", SqlDbType.Int);
                            id.Direction = ParameterDirection.Input;
                            id.Value = idDatiLavorazioneMagazzino;
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
                esito.Descrizione = "Dati_Lavorazione_Magazzino_DAL.cs - EliminaDatiLavorazioneMagazzino " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

    }
}