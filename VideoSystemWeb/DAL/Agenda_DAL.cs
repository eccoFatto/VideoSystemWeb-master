using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using VideoSystemWeb.BLL;
using VideoSystemWeb.Entity;

namespace VideoSystemWeb.DAL
{
    public class Agenda_DAL: Base_DAL
    {
        private const string TABELLA_DATI_AGENDA = "tab_dati_agenda";

        //singleton
        private static volatile Agenda_DAL instance;
        private static object objForLock = new Object();

        private Agenda_DAL() { }

        public static Agenda_DAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Agenda_DAL();
                    }
                }
                return instance;
            }
        }

        public List<Tipologica> CaricaColonne(ref Esito esito)
        {
            return CaricaTipologica(EnumTipologiche.TIPO_COLONNE_AGENDA, true, ref esito);
        }

        public List<DatiAgenda> CaricaDatiAgenda(ref Esito esito)
        {
            List<DatiAgenda> listaDatiAgenda = new List<DatiAgenda>();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM "+ TABELLA_DATI_AGENDA))
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
                                        DatiAgenda datoAgenda = new DatiAgenda();
                                        datoAgenda.id = riga.Field<int>("id");
                                        datoAgenda.id_colonne_agenda = riga.Field<int>("id_colonne_agenda"); 
                                        datoAgenda.id_stato = riga.Field<int>("id_stato");
                                        datoAgenda.data_inizio_lavorazione = riga.Field<DateTime>("data_inizio_lavorazione"); 
                                        datoAgenda.data_fine_lavorazione = riga.Field<DateTime>("data_fine_lavorazione"); 
                                        datoAgenda.durata_lavorazione = riga.Field<int>("durata_lavorazione"); 
                                        datoAgenda.id_tipologia = riga.Field<int>("id_tipologia");
                                        datoAgenda.id_cliente = riga.Field<int?>("id_cliente"); 
                                        datoAgenda.durata_viaggio_andata = riga.Field<int>("durata_viaggio_andata"); 
                                        datoAgenda.durata_viaggio_ritorno = riga.Field<int>("durata_viaggio_ritorno"); 
                                        datoAgenda.data_inizio_impegno = riga.Field<DateTime?>("data_inizio_impegno");
                                        datoAgenda.data_fine_impegno = riga.Field<DateTime?>("data_fine_impegno"); 
                                        datoAgenda.impegnoOrario = riga.Field <bool>("impegnoOrario");
                                        datoAgenda.impegnoOrario_da = riga.Field<string>("impegnoOrario_da");
                                        datoAgenda.impegnoOrario_a = riga.Field<string>("impegnoOrario_a"); 
                                        datoAgenda.produzione = riga.Field <string>("produzione");
                                        datoAgenda.lavorazione = riga.Field<string>("lavorazione");
                                        datoAgenda.indirizzo = riga.Field<string>("indirizzo");
                                        datoAgenda.luogo = riga.Field<string>("luogo");
                                        datoAgenda.codice_lavoro = riga.Field<string>("codice_lavoro");
                                        datoAgenda.nota = riga.Field<string>("nota");

                                        listaDatiAgenda.Add(datoAgenda);
                                    }
                                }
                                else
                                {
                                    esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                    esito.descrizione = "Nessun dato trovato nella tabella tab_dati_agenda ";
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

            return listaDatiAgenda;
        }

        public List<DatiAgenda> CaricaDatiAgenda(DateTime dataInizio, DateTime dataFine, ref Esito esito)
        {
            List<DatiAgenda> listaDatiAgenda = new List<DatiAgenda>();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM " + TABELLA_DATI_AGENDA + " WHERE data_inizio_lavorazione between '" + dataInizio.ToString("dd/MM/yyyy") + "' and '" + dataFine.ToString("dd/MM/yyyy") + "'"))
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
                                        DatiAgenda datoAgenda = new DatiAgenda();
                                        datoAgenda.id = riga.Field<int>("id");
                                        datoAgenda.id_colonne_agenda = riga.Field<int>("id_colonne_agenda");
                                        datoAgenda.id_stato = riga.Field<int>("id_stato");
                                        datoAgenda.data_inizio_lavorazione = riga.Field<DateTime>("data_inizio_lavorazione");
                                        datoAgenda.data_fine_lavorazione = riga.Field<DateTime>("data_fine_lavorazione");
                                        datoAgenda.durata_lavorazione = riga.Field<int>("durata_lavorazione");
                                        datoAgenda.id_tipologia = riga.Field<int>("id_tipologia");
                                        datoAgenda.id_cliente = riga.Field<int?>("id_cliente");
                                        datoAgenda.durata_viaggio_andata = riga.Field<int>("durata_viaggio_andata");
                                        datoAgenda.durata_viaggio_ritorno = riga.Field<int>("durata_viaggio_ritorno");
                                        datoAgenda.data_inizio_impegno = riga.Field<DateTime?>("data_inizio_impegno");
                                        datoAgenda.data_fine_impegno = riga.Field<DateTime?>("data_fine_impegno");
                                        datoAgenda.impegnoOrario = riga.Field<bool>("impegnoOrario");
                                        datoAgenda.impegnoOrario_da = riga.Field<string>("impegnoOrario_da");
                                        datoAgenda.impegnoOrario_a = riga.Field<string>("impegnoOrario_a");
                                        datoAgenda.produzione = riga.Field<string>("produzione");
                                        datoAgenda.lavorazione = riga.Field<string>("lavorazione");
                                        datoAgenda.indirizzo = riga.Field<string>("indirizzo");
                                        datoAgenda.luogo = riga.Field<string>("luogo");
                                        datoAgenda.codice_lavoro = riga.Field<string>("codice_lavoro");
                                        datoAgenda.nota = riga.Field<string>("nota");

                                        listaDatiAgenda.Add(datoAgenda);
                                    }
                                }
                                else
                                {
                                    esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                    esito.descrizione = "Nessun dato trovato nella tabella tab_dati_agenda ";
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

            return listaDatiAgenda;
        }

        public Esito CreaEvento(DatiAgenda evento)
        {
            Esito esito = new Esito();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("InsertEvento"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            SqlParameter data_inizio_lavorazione = new SqlParameter("@data_inizio_lavorazione", SqlDbType.Date);
                            data_inizio_lavorazione.Direction = ParameterDirection.Input;
                            data_inizio_lavorazione.Value = evento.data_inizio_lavorazione;
                            StoreProc.Parameters.Add(data_inizio_lavorazione);

                            SqlParameter data_fine_lavorazione = new SqlParameter("@data_fine_lavorazione", SqlDbType.Date);
                            data_fine_lavorazione.Direction = ParameterDirection.Input;
                            data_fine_lavorazione.Value = evento.data_fine_lavorazione;
                            StoreProc.Parameters.Add(data_fine_lavorazione);

                            SqlParameter durata_lavorazione = new SqlParameter("@durata_lavorazione", SqlDbType.Int);
                            durata_lavorazione.Direction = ParameterDirection.Input;
                            durata_lavorazione.Value = evento.durata_lavorazione;
                            StoreProc.Parameters.Add(durata_lavorazione);

                            SqlParameter id_colonne_agenda = new SqlParameter("@id_colonne_agenda", SqlDbType.Int);
                            id_colonne_agenda.Direction = ParameterDirection.Input;
                            id_colonne_agenda.Value = evento.id_colonne_agenda;
                            StoreProc.Parameters.Add(id_colonne_agenda);

                            SqlParameter id_tipologia = new SqlParameter("@id_tipologia", SqlDbType.Int);
                            id_tipologia.Direction = ParameterDirection.Input;
                            id_tipologia.Value = evento.id_tipologia;
                            StoreProc.Parameters.Add(id_tipologia);

                            SqlParameter id_stato = new SqlParameter("@id_stato", SqlDbType.Int);
                            id_stato.Direction = ParameterDirection.Input;
                            id_stato.Value = evento.id_stato;
                            StoreProc.Parameters.Add(id_stato);

                            SqlParameter id_cliente = new SqlParameter("@id_cliente", SqlDbType.Int);
                            id_cliente.Direction = ParameterDirection.Input;
                            id_cliente.Value = evento.id_cliente;
                            StoreProc.Parameters.Add(id_cliente);

                            SqlParameter durata_viaggio_andata = new SqlParameter("@durata_viaggio_andata", SqlDbType.Int);
                            durata_viaggio_andata.Direction = ParameterDirection.Input;
                            durata_viaggio_andata.Value = evento.durata_viaggio_andata;
                            StoreProc.Parameters.Add(durata_viaggio_andata);

                            SqlParameter durata_viaggio_ritorno = new SqlParameter("@durata_viaggio_ritorno", SqlDbType.Int);
                            durata_viaggio_ritorno.Direction = ParameterDirection.Input;
                            durata_viaggio_ritorno.Value = evento.durata_viaggio_ritorno;
                            StoreProc.Parameters.Add(durata_viaggio_ritorno);

                            SqlParameter data_inizio_impegno = new SqlParameter("@data_inizio_impegno", SqlDbType.Date);
                            data_inizio_impegno.Direction = ParameterDirection.Input;
                            data_inizio_impegno.Value = evento.data_inizio_impegno;
                            StoreProc.Parameters.Add(data_inizio_impegno);

                            SqlParameter data_fine_impegno = new SqlParameter("@data_fine_impegno", SqlDbType.Date);
                            data_fine_impegno.Direction = ParameterDirection.Input;
                            data_fine_impegno.Value = evento.data_fine_impegno;
                            StoreProc.Parameters.Add(data_fine_impegno);

                            SqlParameter impegnoOrario = new SqlParameter("@impegnoOrario", SqlDbType.Bit);
                            impegnoOrario.Direction = ParameterDirection.Input;
                            impegnoOrario.Value = evento.impegnoOrario;
                            StoreProc.Parameters.Add(impegnoOrario);

                            SqlParameter impegnoOrario_da = new SqlParameter("@impegnoOrario_da", SqlDbType.VarChar);
                            impegnoOrario_da.Direction = ParameterDirection.Input;
                            impegnoOrario_da.Value = evento.impegnoOrario_da;
                            StoreProc.Parameters.Add(impegnoOrario_da);

                            SqlParameter impegnoOrario_a = new SqlParameter("@impegnoOrario_a", SqlDbType.VarChar);
                            impegnoOrario_a.Direction = ParameterDirection.Input;
                            impegnoOrario_a.Value = evento.impegnoOrario_a;
                            StoreProc.Parameters.Add(impegnoOrario_a);

                            SqlParameter produzione = new SqlParameter("@produzione", SqlDbType.VarChar);
                            produzione.Direction = ParameterDirection.Input;
                            produzione.Value = evento.produzione;
                            StoreProc.Parameters.Add(produzione);

                            SqlParameter lavorazione = new SqlParameter("@lavorazione", SqlDbType.VarChar);
                            lavorazione.Direction = ParameterDirection.Input;
                            lavorazione.Value = evento.lavorazione;
                            StoreProc.Parameters.Add(lavorazione);

                            SqlParameter indirizzo = new SqlParameter("@indirizzo", SqlDbType.VarChar);
                            indirizzo.Direction = ParameterDirection.Input;
                            indirizzo.Value = evento.indirizzo;
                            StoreProc.Parameters.Add(indirizzo);

                            SqlParameter luogo = new SqlParameter("@luogo", SqlDbType.VarChar);
                            luogo.Direction = ParameterDirection.Input;
                            luogo.Value = evento.luogo;
                            StoreProc.Parameters.Add(luogo);

                            SqlParameter codice_lavoro = new SqlParameter("@codice_lavoro", SqlDbType.VarChar);
                            codice_lavoro.Direction = ParameterDirection.Input;
                            codice_lavoro.Value = evento.codice_lavoro;
                            StoreProc.Parameters.Add(codice_lavoro);

                            SqlParameter nota = new SqlParameter("@nota", SqlDbType.VarChar);
                            nota.Direction = ParameterDirection.Input;
                            nota.Value = evento.nota;
                            StoreProc.Parameters.Add(nota);


                            StoreProc.Connection.Open();

                            int iReturn = StoreProc.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.descrizione = "Agenda_DAL.cs - creaEvento " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

        public Esito AggiornaEvento(DatiAgenda evento)
        {
            Esito esito = new Esito();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("UpdateEvento"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            SqlParameter id = new SqlParameter("@id", SqlDbType.Int);
                            id.Direction = ParameterDirection.Input;
                            id.Value = evento.id;
                            StoreProc.Parameters.Add(id);

                            SqlParameter data_inizio_lavorazione = new SqlParameter("@data_inizio_lavorazione", SqlDbType.Date);
                            data_inizio_lavorazione.Direction = ParameterDirection.Input;
                            data_inizio_lavorazione.Value = evento.data_inizio_lavorazione;
                            StoreProc.Parameters.Add(data_inizio_lavorazione);

                            SqlParameter data_fine_lavorazione = new SqlParameter("@data_fine_lavorazione", SqlDbType.Date);
                            data_fine_lavorazione.Direction = ParameterDirection.Input;
                            data_fine_lavorazione.Value = evento.data_fine_lavorazione;
                            StoreProc.Parameters.Add(data_fine_lavorazione);

                            SqlParameter durata_lavorazione = new SqlParameter("@durata_lavorazione", SqlDbType.Int);
                            durata_lavorazione.Direction = ParameterDirection.Input;
                            durata_lavorazione.Value = evento.durata_lavorazione;
                            StoreProc.Parameters.Add(durata_lavorazione);

                            SqlParameter id_colonne_agenda = new SqlParameter("@id_colonne_agenda", SqlDbType.Int);
                            id_colonne_agenda.Direction = ParameterDirection.Input;
                            id_colonne_agenda.Value = evento.id_colonne_agenda;
                            StoreProc.Parameters.Add(id_colonne_agenda);

                            SqlParameter id_tipologia = new SqlParameter("@id_tipologia", SqlDbType.Int);
                            id_tipologia.Direction = ParameterDirection.Input;
                            id_tipologia.Value = evento.id_tipologia;
                            StoreProc.Parameters.Add(id_tipologia);

                            SqlParameter id_stato = new SqlParameter("@id_stato", SqlDbType.Int);
                            id_stato.Direction = ParameterDirection.Input;
                            id_stato.Value = evento.id_stato;
                            StoreProc.Parameters.Add(id_stato);

                            SqlParameter id_cliente = new SqlParameter("@id_cliente", SqlDbType.Int);
                            id_cliente.Direction = ParameterDirection.Input;
                            id_cliente.Value = evento.id_cliente;
                            StoreProc.Parameters.Add(id_cliente);

                            SqlParameter durata_viaggio_andata = new SqlParameter("@durata_viaggio_andata", SqlDbType.Int);
                            durata_viaggio_andata.Direction = ParameterDirection.Input;
                            durata_viaggio_andata.Value = evento.durata_viaggio_andata;
                            StoreProc.Parameters.Add(durata_viaggio_andata);

                            SqlParameter durata_viaggio_ritorno = new SqlParameter("@durata_viaggio_ritorno", SqlDbType.Int);
                            durata_viaggio_ritorno.Direction = ParameterDirection.Input;
                            durata_viaggio_ritorno.Value = evento.durata_viaggio_ritorno;
                            StoreProc.Parameters.Add(durata_viaggio_ritorno);

                            SqlParameter data_inizio_impegno = new SqlParameter("@data_inizio_impegno", SqlDbType.Date);
                            data_inizio_impegno.Direction = ParameterDirection.Input;
                            data_inizio_impegno.Value = evento.data_inizio_impegno;
                            StoreProc.Parameters.Add(data_inizio_impegno);

                            SqlParameter data_fine_impegno = new SqlParameter("@data_fine_impegno", SqlDbType.Date);
                            data_fine_impegno.Direction = ParameterDirection.Input;
                            data_fine_impegno.Value = evento.data_fine_impegno;
                            StoreProc.Parameters.Add(data_fine_impegno);

                            SqlParameter impegnoOrario = new SqlParameter("@impegnoOrario", SqlDbType.Bit);
                            impegnoOrario.Direction = ParameterDirection.Input;
                            impegnoOrario.Value = evento.impegnoOrario;
                            StoreProc.Parameters.Add(impegnoOrario);

                            SqlParameter impegnoOrario_da = new SqlParameter("@impegnoOrario_da", SqlDbType.VarChar);
                            impegnoOrario_da.Direction = ParameterDirection.Input;
                            impegnoOrario_da.Value = evento.impegnoOrario_da;
                            StoreProc.Parameters.Add(impegnoOrario_da);

                            SqlParameter impegnoOrario_a = new SqlParameter("@impegnoOrario_a", SqlDbType.VarChar);
                            impegnoOrario_a.Direction = ParameterDirection.Input;
                            impegnoOrario_a.Value = evento.impegnoOrario_a;
                            StoreProc.Parameters.Add(impegnoOrario_a);

                            SqlParameter produzione = new SqlParameter("@produzione", SqlDbType.VarChar);
                            produzione.Direction = ParameterDirection.Input;
                            produzione.Value = evento.produzione;
                            StoreProc.Parameters.Add(produzione);

                            SqlParameter lavorazione = new SqlParameter("@lavorazione", SqlDbType.VarChar);
                            lavorazione.Direction = ParameterDirection.Input;
                            lavorazione.Value = evento.lavorazione;
                            StoreProc.Parameters.Add(lavorazione);

                            SqlParameter indirizzo = new SqlParameter("@indirizzo", SqlDbType.VarChar);
                            indirizzo.Direction = ParameterDirection.Input;
                            indirizzo.Value = evento.indirizzo;
                            StoreProc.Parameters.Add(indirizzo);

                            SqlParameter luogo = new SqlParameter("@luogo", SqlDbType.VarChar);
                            luogo.Direction = ParameterDirection.Input;
                            luogo.Value = evento.luogo;
                            StoreProc.Parameters.Add(luogo);

                            SqlParameter codice_lavoro = new SqlParameter("@codice_lavoro", SqlDbType.VarChar);
                            codice_lavoro.Direction = ParameterDirection.Input;
                            codice_lavoro.Value = evento.codice_lavoro;
                            StoreProc.Parameters.Add(codice_lavoro);

                            SqlParameter nota = new SqlParameter("@nota", SqlDbType.VarChar);
                            nota.Direction = ParameterDirection.Input;
                            nota.Value = evento.nota;
                            StoreProc.Parameters.Add(nota);


                            StoreProc.Connection.Open();

                            int iReturn = StoreProc.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.descrizione = "Agenda_DAL.cs - aggiornaEvento " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

        public Esito EliminaEvento(int idEvento)
        {
            Esito esito = new Esito();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("DeleteEvento"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            SqlParameter id = new SqlParameter("@id", SqlDbType.Int);
                            id.Direction = ParameterDirection.Input;
                            id.Value = idEvento;
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
                esito.descrizione = "Agenda_DAL.cs - EliminaEvento " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }
    }
}