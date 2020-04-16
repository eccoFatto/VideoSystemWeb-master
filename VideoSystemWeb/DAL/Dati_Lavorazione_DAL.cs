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
    public class Dati_Lavorazione_DAL : Base_DAL
    {
        //singleton
        private static volatile Dati_Lavorazione_DAL instance;
        private static object objForLock = new Object();

        private Dati_Lavorazione_DAL() { }

        public static Dati_Lavorazione_DAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Dati_Lavorazione_DAL();
                    }
                }
                return instance;
            }
        }

        public DatiLavorazione getDatiLavorazioneById(int idDatiLavorazione, ref Esito esito)
        {
            DatiLavorazione datiLavorazione = new DatiLavorazione();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM dati_lavorazione where id = " + idDatiLavorazione.ToString();
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
                                    datiLavorazione.Id = dt.Rows[0].Field<int>("id");
                                    datiLavorazione.IdDatiAgenda = dt.Rows[0].Field<int>("idDatiAgenda");
                                    datiLavorazione.IdContratto = dt.Rows[0].Field<int?>("idContratto");
                                    datiLavorazione.IdReferente = dt.Rows[0].Field<int?>("idReferente");
                                    datiLavorazione.IdCapoTecnico = dt.Rows[0].Field<int?>("idCapoTecnico");
                                    datiLavorazione.IdProduttore = dt.Rows[0].Field<int?>("idProduttore");
                                    datiLavorazione.Fattura = dt.Rows[0].Field<string>("fattura");
                                    datiLavorazione.Ordine = dt.Rows[0].Field<string>("ordine");
                                    datiLavorazione.NotePianoEsterno = dt.Rows[0].Field<string>("notePianoEsterno");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.Descrizione = "Dati_Lavorazione_DAL.cs - getDatiLavorazioneById " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }
            return datiLavorazione;
        }

        public DatiLavorazione getDatiLavorazioneByIdEvento(int idDatiAgenda, ref Esito esito)
        {
            DatiLavorazione datiLavorazione = new DatiLavorazione();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM dati_lavorazione where idDatiAgenda = " + idDatiAgenda.ToString();
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
                                    datiLavorazione.Id = dt.Rows[0].Field<int>("id");
                                    datiLavorazione.IdDatiAgenda = dt.Rows[0].Field<int>("idDatiAgenda");
                                    datiLavorazione.IdContratto = dt.Rows[0].Field<int?>("idContratto");
                                    datiLavorazione.IdReferente = dt.Rows[0].Field<int?>("idReferente");
                                    datiLavorazione.IdCapoTecnico = dt.Rows[0].Field<int?>("idCapoTecnico");
                                    datiLavorazione.IdProduttore = dt.Rows[0].Field<int?>("idProduttore");
                                    datiLavorazione.Fattura = dt.Rows[0].Field<string>("fattura");
                                    datiLavorazione.Ordine = dt.Rows[0].Field<string>("ordine");
                                    datiLavorazione.NotePianoEsterno = dt.Rows[0].Field<string>("notePianoEsterno");
                                }
                                else
                                {
                                    datiLavorazione = null;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.Descrizione = "Dati_Lavorazione_DAL.cs - getDatiLavorazioneByIdEvento " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }
            return datiLavorazione;
        }

        public List<DatiLavorazione> GetAllLavorazioni(ref Esito esito)
        {
            List<DatiLavorazione> listaDatiLavorazione = new List<DatiLavorazione>();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT a.*, b.lavorazione FROM dati_lavorazione a left join tab_dati_agenda b on a.idDatiAgenda=b.id";
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
                                        DatiLavorazione datiLavorazione = new DatiLavorazione
                                        {
                                            Id = riga.Field<int>("id"),
                                            IdDatiAgenda = riga.Field<int>("idDatiAgenda"),
                                            IdContratto = riga.Field<int?>("idContratto"),
                                            IdReferente = riga.Field<int?>("idReferente"),
                                            IdCapoTecnico = riga.Field<int?>("idCapoTecnico"),
                                            IdProduttore = riga.Field<int?>("idProduttore"),
                                            Fattura = riga.Field<string>("fattura"),
                                            Ordine = riga.Field<string>("ordine"),
                                            NotePianoEsterno = riga.Field<string>("notePianoEsterno"),
                                            DescrizioneLavorazione = riga.Field<string>("lavorazione")
                                        };

                                        listaDatiLavorazione.Add(datiLavorazione);
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
                esito.Descrizione = "Dati_Lavorazione_DAL.cs - getAllLavorazioni " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }
            return listaDatiLavorazione;
        }
        public int CreaDatiLavorazione(DatiLavorazione datiLavorazione, ref Esito esito)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("InsertDatiLavorazione"))
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

                            SqlParameter idDatiAgenda = new SqlParameter("@idDatiAgenda", datiLavorazione.IdDatiAgenda);
                            idDatiAgenda.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idDatiAgenda);

                            SqlParameter idContratto = new SqlParameter("@idContratto", datiLavorazione.IdContratto);
                            idContratto.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idContratto);

                            SqlParameter idReferente = new SqlParameter("@idReferente", datiLavorazione.IdReferente);
                            idReferente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idReferente);

                            SqlParameter idCapoTecnico = new SqlParameter("@idCapoTecnico", datiLavorazione.IdCapoTecnico);
                            idCapoTecnico.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idCapoTecnico);

                            SqlParameter idProduttore = new SqlParameter("@idProduttore", datiLavorazione.IdProduttore);
                            idProduttore.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idProduttore);

                            SqlParameter fattura = new SqlParameter("@fattura", datiLavorazione.Fattura);
                            fattura.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(fattura);

                            SqlParameter ordine = new SqlParameter("@ordine", datiLavorazione.Ordine);
                            ordine.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(ordine);

                            SqlParameter notePianoEsterno = new SqlParameter("@notePianoEsterno", datiLavorazione.NotePianoEsterno);
                            notePianoEsterno.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(notePianoEsterno);

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
                esito.Descrizione = "Dati_Lavorazione_DAL.cs - CreaDatiLavorazione " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return 0;
        }

        public Esito AggiornaDatiLavorazione(DatiLavorazione datiLavorazione)
        {
            Esito esito = new Esito();
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);
            try
            {
                using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(sqlConstr))
                {
                    using (System.Data.SqlClient.SqlCommand StoreProc = new System.Data.SqlClient.SqlCommand("UpdateDatiLavorazione"))
                    {
                        using (System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            System.Data.SqlClient.SqlParameter id = new System.Data.SqlClient.SqlParameter("@id", datiLavorazione.Id);
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

                            SqlParameter idDatiAgenda = new SqlParameter("@idDatiAgenda", datiLavorazione.IdDatiAgenda);
                            idDatiAgenda.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idDatiAgenda);

                            SqlParameter idContratto = new SqlParameter("@idContratto", datiLavorazione.IdContratto);
                            idContratto.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idContratto);

                            SqlParameter idReferente = new SqlParameter("@idReferente", datiLavorazione.IdReferente);
                            idReferente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idReferente);

                            SqlParameter idCapoTecnico = new SqlParameter("@idCapoTecnico", datiLavorazione.IdCapoTecnico);
                            idCapoTecnico.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idCapoTecnico);

                            SqlParameter idProduttore = new SqlParameter("@idProduttore", datiLavorazione.IdProduttore);
                            idProduttore.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idProduttore);

                            SqlParameter fattura = new SqlParameter("@fattura", datiLavorazione.Fattura);
                            fattura.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(fattura);

                            SqlParameter ordine = new SqlParameter("@ordine", datiLavorazione.Ordine);
                            ordine.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(ordine);

                            SqlParameter notePianoEsterno = new SqlParameter("@notePianoEsterno", datiLavorazione.NotePianoEsterno);
                            notePianoEsterno.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(notePianoEsterno);

                            StoreProc.Connection.Open();

                            int iReturn = StoreProc.ExecuteNonQuery();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.Descrizione = "Dati_Lavorazione_DAL.cs - AggiornaDatiLavorazione " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

        public Esito EliminaDatiLavorazione(int idDatiLavorazione)
        {
            Esito esito = new Esito();
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("DeleteDatiLavorazione"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            SqlParameter id = new SqlParameter("@id", SqlDbType.Int);
                            id.Direction = ParameterDirection.Input;
                            id.Value = idDatiLavorazione;
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
                esito.Descrizione = "Dati_Lavorazione_DAL.cs - EliminaDatiLavorazione " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

    }
}