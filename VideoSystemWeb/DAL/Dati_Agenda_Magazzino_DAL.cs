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
    public class Dati_Agenda_Magazzino_DAL : Base_DAL
    {
        //singleton
        private static volatile Dati_Agenda_Magazzino_DAL instance;
        private static object objForLock = new Object();

        private Dati_Agenda_Magazzino_DAL() { }

        public static Dati_Agenda_Magazzino_DAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Dati_Agenda_Magazzino_DAL();
                    }
                }
                return instance;
            }
        }

        public DatiAgendaMagazzino getDatiAgendaMagazzinoById(int idDatiAgendaMagazzino, ref Esito esito)
        {
            DatiAgendaMagazzino datiAgendaMagazzino = new DatiAgendaMagazzino();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM dati_agenda_magazzino where id = " + idDatiAgendaMagazzino.ToString();
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
                                    datiAgendaMagazzino.Id = dt.Rows[0].Field<int>("id");
                                    datiAgendaMagazzino.Id_Agenda = dt.Rows[0].Field<int>("id_Agenda");
                                    datiAgendaMagazzino.Descrizione_Camera = dt.Rows[0].Field<string>("descrizione_Camera");

                                    datiAgendaMagazzino.Id_Altro1 = dt.Rows[0].Field<int>("id_Altro1");
                                    datiAgendaMagazzino.Id_Altro2 = dt.Rows[0].Field<int>("id_Altro2");
                                    datiAgendaMagazzino.Id_Camera = dt.Rows[0].Field<int>("id_Camera");
                                    datiAgendaMagazzino.Id_Cavalletto = dt.Rows[0].Field<int>("id_Cavalletto");
                                    datiAgendaMagazzino.Id_Cavi = dt.Rows[0].Field<int>("id_Cavi");
                                    datiAgendaMagazzino.Id_Fibra_Trax = dt.Rows[0].Field<int>("id_Fibra_Trax");
                                    datiAgendaMagazzino.Id_Lensholder = dt.Rows[0].Field<int>("id_Lensholder");
                                    datiAgendaMagazzino.Id_Loop = dt.Rows[0].Field<int>("id_Loop");
                                    datiAgendaMagazzino.Id_Mic = dt.Rows[0].Field<int>("id_Mic");
                                    datiAgendaMagazzino.Id_Ottica = dt.Rows[0].Field<int>("id_Ottica");
                                    datiAgendaMagazzino.Id_Testa = dt.Rows[0].Field<int>("id_Testa");
                                    datiAgendaMagazzino.Id_Viewfinder = dt.Rows[0].Field<int>("id_Viewfinder");

                                    datiAgendaMagazzino.Nome_Altro1 = dt.Rows[0].Field<string>("nome_Altro1");
                                    datiAgendaMagazzino.Nome_Altro2 = dt.Rows[0].Field<string>("nome_Altro2");
                                    datiAgendaMagazzino.Nome_Camera = dt.Rows[0].Field<string>("nome_Camera");
                                    datiAgendaMagazzino.Nome_Cavalletto = dt.Rows[0].Field<string>("nome_Cavalletto");
                                    datiAgendaMagazzino.Nome_Cavi = dt.Rows[0].Field<string>("nome_Cavi");
                                    datiAgendaMagazzino.Nome_Fibra_Trax = dt.Rows[0].Field<string>("nome_Fibra_Trax");
                                    datiAgendaMagazzino.Nome_Lensholder = dt.Rows[0].Field<string>("nome_Lensholder");
                                    datiAgendaMagazzino.Nome_Loop = dt.Rows[0].Field<string>("nome_Loop");
                                    datiAgendaMagazzino.Nome_Mic = dt.Rows[0].Field<string>("nome_Mic");
                                    datiAgendaMagazzino.Nome_Ottica = dt.Rows[0].Field<string>("nome_Ottica");
                                    datiAgendaMagazzino.Nome_Testa = dt.Rows[0].Field<string>("nome_Testa");
                                    datiAgendaMagazzino.Nome_Viewfinder = dt.Rows[0].Field<string>("nome_Viewfinder");

                                    datiAgendaMagazzino.Attivo = dt.Rows[0].Field<bool>("attivo");
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
            return datiAgendaMagazzino;
        }

        public int CreaDatiAgendaMagazzino(DatiAgendaMagazzino datiAgendaMagazzino, ref Esito esito)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("InsertDatiAgendaMagazzino"))
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

                            SqlParameter id_Agenda = new SqlParameter("@id_Agenda", datiAgendaMagazzino.Id_Agenda);
                            id_Agenda.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Agenda);

                            SqlParameter descrizione_Camera = new SqlParameter("@descrizione_Camera", datiAgendaMagazzino.Descrizione_Camera);
                            descrizione_Camera.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(descrizione_Camera);

                            SqlParameter id_Altro1 = new SqlParameter("@id_Altro1", datiAgendaMagazzino.Id_Altro1);
                            id_Altro1.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Altro1);

                            SqlParameter id_Altro2 = new SqlParameter("@id_Altro2", datiAgendaMagazzino.Id_Altro2);
                            id_Altro2.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Altro2);

                            SqlParameter id_Camera = new SqlParameter("@id_Camera", datiAgendaMagazzino.Id_Camera);
                            id_Camera.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Camera);

                            SqlParameter id_Cavalletto = new SqlParameter("@id_Cavalletto", datiAgendaMagazzino.Id_Cavalletto);
                            id_Cavalletto.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Cavalletto);

                            SqlParameter id_Cavi = new SqlParameter("@id_Cavi", datiAgendaMagazzino.Id_Cavi);
                            id_Cavi.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Cavi);

                            SqlParameter id_Fibra_Trax = new SqlParameter("@id_Fibra_Trax", datiAgendaMagazzino.Id_Fibra_Trax);
                            id_Fibra_Trax.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Fibra_Trax);

                            SqlParameter id_Lensholder = new SqlParameter("@id_Lensholder", datiAgendaMagazzino.Id_Lensholder);
                            id_Lensholder.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Lensholder);

                            SqlParameter id_Loop = new SqlParameter("@id_Loop", datiAgendaMagazzino.Id_Loop);
                            id_Loop.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Loop);

                            SqlParameter id_Mic = new SqlParameter("@id_Mic", datiAgendaMagazzino.Id_Mic);
                            id_Mic.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Mic);

                            SqlParameter id_Ottica = new SqlParameter("@id_Ottica", datiAgendaMagazzino.Id_Ottica);
                            id_Ottica.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Ottica);

                            SqlParameter id_Testa = new SqlParameter("@id_Testa", datiAgendaMagazzino.Id_Testa);
                            id_Testa.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Testa);

                            SqlParameter id_Viewfinder = new SqlParameter("@id_Viewfinder", datiAgendaMagazzino.Id_Viewfinder);
                            id_Viewfinder.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Viewfinder);

                            SqlParameter nome_Altro1 = new SqlParameter("@nome_Altro1", datiAgendaMagazzino.Nome_Altro1);
                            nome_Altro1.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Altro1);

                            SqlParameter nome_Altro2 = new SqlParameter("@nome_Altro2", datiAgendaMagazzino.Nome_Altro2);
                            nome_Altro2.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Altro2);

                            SqlParameter nome_Camera = new SqlParameter("@nome_Camera", datiAgendaMagazzino.Nome_Camera);
                            nome_Camera.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Camera);

                            SqlParameter nome_Cavalletto = new SqlParameter("@nome_Cavalletto", datiAgendaMagazzino.Nome_Cavalletto);
                            nome_Cavalletto.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Cavalletto);

                            SqlParameter nome_Cavi = new SqlParameter("@nome_Cavi", datiAgendaMagazzino.Nome_Cavi);
                            nome_Cavi.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Cavi);

                            SqlParameter nome_Fibra_Trax = new SqlParameter("@nome_Fibra_Trax", datiAgendaMagazzino.Nome_Fibra_Trax);
                            nome_Fibra_Trax.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Fibra_Trax);

                            SqlParameter nome_Lensholder = new SqlParameter("@nome_Lensholder", datiAgendaMagazzino.Nome_Lensholder);
                            nome_Lensholder.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Lensholder);

                            SqlParameter nome_Loop = new SqlParameter("@nome_Loop", datiAgendaMagazzino.Nome_Loop);
                            nome_Loop.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Loop);

                            SqlParameter nome_Mic = new SqlParameter("@nome_Mic", datiAgendaMagazzino.Nome_Mic);
                            nome_Mic.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Mic);

                            SqlParameter nome_Ottica = new SqlParameter("@nome_Ottica", datiAgendaMagazzino.Nome_Ottica);
                            nome_Ottica.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Ottica);

                            SqlParameter nome_Testa = new SqlParameter("@nome_Testa", datiAgendaMagazzino.Nome_Testa);
                            nome_Testa.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Testa);

                            SqlParameter nome_Viewfinder = new SqlParameter("@nome_Viewfinder", datiAgendaMagazzino.Nome_Viewfinder);
                            nome_Viewfinder.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Viewfinder);

                            SqlParameter attivo = new SqlParameter("@attivo", datiAgendaMagazzino.Attivo);
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
                esito.Descrizione = "Dati_Agenda_Magazzino_DAL.cs - CreaDatiAgendaMagazzino " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return 0;
        }

        public Esito AggiornaDatiAgendaMagazzino(DatiAgendaMagazzino datiAgendaMagazzino)
        {
            Esito esito = new Esito();
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);
            try
            {
                using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(sqlConstr))
                {
                    using (System.Data.SqlClient.SqlCommand StoreProc = new System.Data.SqlClient.SqlCommand("UpdateDatiAgendaMagazzino"))
                    {
                        using (System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            System.Data.SqlClient.SqlParameter id = new System.Data.SqlClient.SqlParameter("@id", datiAgendaMagazzino.Id);
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

                            SqlParameter id_Agenda = new SqlParameter("@id_Agenda", datiAgendaMagazzino.Id_Agenda);
                            id_Agenda.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Agenda);

                            SqlParameter descrizione_Camera = new SqlParameter("@descrizione_Camera", datiAgendaMagazzino.Descrizione_Camera);
                            descrizione_Camera.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(descrizione_Camera);

                            SqlParameter id_Altro1 = new SqlParameter("@id_Altro1", datiAgendaMagazzino.Id_Altro1);
                            id_Altro1.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Altro1);

                            SqlParameter id_Altro2 = new SqlParameter("@id_Altro2", datiAgendaMagazzino.Id_Altro2);
                            id_Altro2.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Altro2);

                            SqlParameter id_Camera = new SqlParameter("@id_Camera", datiAgendaMagazzino.Id_Camera);
                            id_Camera.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Camera);

                            SqlParameter id_Cavalletto = new SqlParameter("@id_Cavalletto", datiAgendaMagazzino.Id_Cavalletto);
                            id_Cavalletto.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Cavalletto);

                            SqlParameter id_Cavi = new SqlParameter("@id_Cavi", datiAgendaMagazzino.Id_Cavi);
                            id_Cavi.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Cavi);

                            SqlParameter id_Fibra_Trax = new SqlParameter("@id_Fibra_Trax", datiAgendaMagazzino.Id_Fibra_Trax);
                            id_Fibra_Trax.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Fibra_Trax);

                            SqlParameter id_Lensholder = new SqlParameter("@id_Lensholder", datiAgendaMagazzino.Id_Lensholder);
                            id_Lensholder.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Lensholder);

                            SqlParameter id_Loop = new SqlParameter("@id_Loop", datiAgendaMagazzino.Id_Loop);
                            id_Loop.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Loop);

                            SqlParameter id_Mic = new SqlParameter("@id_Mic", datiAgendaMagazzino.Id_Mic);
                            id_Mic.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Mic);

                            SqlParameter id_Ottica = new SqlParameter("@id_Ottica", datiAgendaMagazzino.Id_Ottica);
                            id_Ottica.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Ottica);

                            SqlParameter id_Testa = new SqlParameter("@id_Testa", datiAgendaMagazzino.Id_Testa);
                            id_Testa.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Testa);

                            SqlParameter id_Viewfinder = new SqlParameter("@id_Viewfinder", datiAgendaMagazzino.Id_Viewfinder);
                            id_Viewfinder.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_Viewfinder);

                            SqlParameter nome_Altro1 = new SqlParameter("@nome_Altro1", datiAgendaMagazzino.Nome_Altro1);
                            nome_Altro1.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Altro1);

                            SqlParameter nome_Altro2 = new SqlParameter("@nome_Altro2", datiAgendaMagazzino.Nome_Altro2);
                            nome_Altro2.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Altro2);

                            SqlParameter nome_Camera = new SqlParameter("@nome_Camera", datiAgendaMagazzino.Nome_Camera);
                            nome_Camera.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Camera);

                            SqlParameter nome_Cavalletto = new SqlParameter("@nome_Cavalletto", datiAgendaMagazzino.Nome_Cavalletto);
                            nome_Cavalletto.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Cavalletto);

                            SqlParameter nome_Cavi = new SqlParameter("@nome_Cavi", datiAgendaMagazzino.Nome_Cavi);
                            nome_Cavi.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Cavi);

                            SqlParameter nome_Fibra_Trax = new SqlParameter("@nome_Fibra_Trax", datiAgendaMagazzino.Nome_Fibra_Trax);
                            nome_Fibra_Trax.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Fibra_Trax);

                            SqlParameter nome_Lensholder = new SqlParameter("@nome_Lensholder", datiAgendaMagazzino.Nome_Lensholder);
                            nome_Lensholder.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Lensholder);

                            SqlParameter nome_Loop = new SqlParameter("@nome_Loop", datiAgendaMagazzino.Nome_Loop);
                            nome_Loop.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Loop);

                            SqlParameter nome_Mic = new SqlParameter("@nome_Mic", datiAgendaMagazzino.Nome_Mic);
                            nome_Mic.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Mic);

                            SqlParameter nome_Ottica = new SqlParameter("@nome_Ottica", datiAgendaMagazzino.Nome_Ottica);
                            nome_Ottica.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Ottica);

                            SqlParameter nome_Testa = new SqlParameter("@nome_Testa", datiAgendaMagazzino.Nome_Testa);
                            nome_Testa.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Testa);

                            SqlParameter nome_Viewfinder = new SqlParameter("@nome_Viewfinder", datiAgendaMagazzino.Nome_Viewfinder);
                            nome_Viewfinder.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome_Viewfinder);

                            SqlParameter attivo = new SqlParameter("@attivo", datiAgendaMagazzino.Attivo);
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
                esito.Descrizione = "Dati_Agenda_Magazzino_DAL.cs - AggiornaDatiAgendaMagazzino " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

        public Esito EliminaDatiAgendaMagazzino(int idDatiAgendaMagazzino)
        {
            Esito esito = new Esito();
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("DeleteDatiAgendaMagazzino"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            SqlParameter id = new SqlParameter("@id", SqlDbType.Int);
                            id.Direction = ParameterDirection.Input;
                            id.Value = idDatiAgendaMagazzino;
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
                esito.Descrizione = "Dati_Agenda_Magazzino_DAL.cs - EliminaDatiAgendaMagazzino " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

        public List<DatiAgendaMagazzino> getDatiAgendaMagazzinoByIdAgenda(int idAgenda, ref Esito esito)
        {
            List<DatiAgendaMagazzino> listaDatiAgendaMagazzino = new List<DatiAgendaMagazzino>();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM dati_agenda_magazzino where id_Agenda = " + idAgenda.ToString();
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
                                        DatiAgendaMagazzino datiAgendaMagazzino = new DatiAgendaMagazzino();

                                        datiAgendaMagazzino.Id = riga.Field<int>("id");
                                        datiAgendaMagazzino.Id_Agenda = riga.Field<int>("id_Agenda");
                                        datiAgendaMagazzino.Descrizione_Camera = riga.Field<string>("descrizione_Camera");

                                        datiAgendaMagazzino.Id_Altro1 = riga.Field<int>("id_Altro1");
                                        datiAgendaMagazzino.Id_Altro2 = riga.Field<int>("id_Altro2");
                                        datiAgendaMagazzino.Id_Camera = riga.Field<int>("id_Camera");
                                        datiAgendaMagazzino.Id_Cavalletto = riga.Field<int>("id_Cavalletto");
                                        datiAgendaMagazzino.Id_Cavi = riga.Field<int>("id_Cavi");
                                        datiAgendaMagazzino.Id_Fibra_Trax = riga.Field<int>("id_Fibra_Trax");
                                        datiAgendaMagazzino.Id_Lensholder = riga.Field<int>("id_Lensholder");
                                        datiAgendaMagazzino.Id_Loop = riga.Field<int>("id_Loop");
                                        datiAgendaMagazzino.Id_Mic = riga.Field<int>("id_Mic");
                                        datiAgendaMagazzino.Id_Ottica = riga.Field<int>("id_Ottica");
                                        datiAgendaMagazzino.Id_Testa = riga.Field<int>("id_Testa");
                                        datiAgendaMagazzino.Id_Viewfinder = riga.Field<int>("id_Viewfinder");

                                        datiAgendaMagazzino.Nome_Altro1 = riga.Field<string>("nome_Altro1");
                                        datiAgendaMagazzino.Nome_Altro2 = riga.Field<string>("nome_Altro2");
                                        datiAgendaMagazzino.Nome_Camera = riga.Field<string>("nome_Camera");
                                        datiAgendaMagazzino.Nome_Cavalletto = riga.Field<string>("nome_Cavalletto");
                                        datiAgendaMagazzino.Nome_Cavi = riga.Field<string>("nome_Cavi");
                                        datiAgendaMagazzino.Nome_Fibra_Trax = riga.Field<string>("nome_Fibra_Trax");
                                        datiAgendaMagazzino.Nome_Lensholder = riga.Field<string>("nome_Lensholder");
                                        datiAgendaMagazzino.Nome_Loop = riga.Field<string>("nome_Loop");
                                        datiAgendaMagazzino.Nome_Mic = riga.Field<string>("nome_Mic");
                                        datiAgendaMagazzino.Nome_Ottica = riga.Field<string>("nome_Ottica");
                                        datiAgendaMagazzino.Nome_Testa = riga.Field<string>("nome_Testa");
                                        datiAgendaMagazzino.Nome_Viewfinder = riga.Field<string>("nome_Viewfinder");

                                        datiAgendaMagazzino.Attivo = dt.Rows[0].Field<bool>("attivo");

                                        listaDatiAgendaMagazzino.Add(datiAgendaMagazzino);
                                    }
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

            return listaDatiAgendaMagazzino;
        }

    }
}