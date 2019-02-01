using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
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
                using (System.Data.OleDb.OleDbConnection con = new System.Data.OleDb.OleDbConnection(constr))
                {
                    using (System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand("SELECT * FROM "+ TABELLA_DATI_AGENDA))
                    {
                        using (System.Data.OleDb.OleDbDataAdapter sda = new System.Data.OleDb.OleDbDataAdapter())
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

        public Esito creaEvento(DatiAgenda evento)
        {
            Esito esito = new Esito();
            try
            {
                using (System.Data.OleDb.OleDbConnection con = new System.Data.OleDb.OleDbConnection(constr))
                {
                    using (System.Data.OleDb.OleDbCommand StoreProc = new System.Data.OleDb.OleDbCommand("InsertEvento"))
                    {
                        using (System.Data.OleDb.OleDbDataAdapter sda = new System.Data.OleDb.OleDbDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            OleDbParameter data_inizio_lavorazione = new OleDbParameter("@data_inizio_lavorazione", OleDbType.Date);
                            data_inizio_lavorazione.Direction = ParameterDirection.Input;
                            data_inizio_lavorazione.Value = evento.data_inizio_lavorazione;
                            StoreProc.Parameters.Add(data_inizio_lavorazione);

                            OleDbParameter data_fine_lavorazione = new OleDbParameter("@data_fine_lavorazione", OleDbType.Date);
                            data_fine_lavorazione.Direction = ParameterDirection.Input;
                            data_fine_lavorazione.Value = evento.data_fine_lavorazione;
                            StoreProc.Parameters.Add(data_fine_lavorazione);

                            OleDbParameter durata_lavorazione = new OleDbParameter("@durata_lavorazione", OleDbType.Integer);
                            durata_lavorazione.Direction = ParameterDirection.Input;
                            durata_lavorazione.Value = evento.durata_lavorazione;
                            StoreProc.Parameters.Add(durata_lavorazione);

                            OleDbParameter id_colonne_agenda = new OleDbParameter("@id_colonne_agenda", OleDbType.Integer);
                            id_colonne_agenda.Direction = ParameterDirection.Input;
                            id_colonne_agenda.Value = evento.id_colonne_agenda;
                            StoreProc.Parameters.Add(id_colonne_agenda);

                            OleDbParameter id_tipologia = new OleDbParameter("@id_tipologia", OleDbType.Integer);
                            id_tipologia.Direction = ParameterDirection.Input;
                            id_tipologia.Value = evento.id_tipologia;
                            StoreProc.Parameters.Add(id_tipologia);

                            OleDbParameter id_stato = new OleDbParameter("@id_stato", OleDbType.Integer);
                            id_stato.Direction = ParameterDirection.Input;
                            id_stato.Value = evento.id_stato;
                            StoreProc.Parameters.Add(id_stato);

                            OleDbParameter id_cliente = new OleDbParameter("@id_cliente", OleDbType.Integer);
                            id_cliente.Direction = ParameterDirection.Input;
                            id_cliente.Value = evento.id_cliente;
                            StoreProc.Parameters.Add(id_cliente);

                            OleDbParameter durata_viaggio_andata = new OleDbParameter("@durata_viaggio_andata", OleDbType.Integer);
                            durata_viaggio_andata.Direction = ParameterDirection.Input;
                            durata_viaggio_andata.Value = evento.durata_viaggio_andata;
                            StoreProc.Parameters.Add(durata_viaggio_andata);

                            OleDbParameter durata_viaggio_ritorno = new OleDbParameter("@durata_viaggio_ritorno", OleDbType.Integer);
                            durata_viaggio_ritorno.Direction = ParameterDirection.Input;
                            durata_viaggio_ritorno.Value = evento.durata_viaggio_ritorno;
                            StoreProc.Parameters.Add(durata_viaggio_ritorno);

                            OleDbParameter data_inizio_impegno = new OleDbParameter("@data_inizio_impegno", OleDbType.Date);
                            data_inizio_impegno.Direction = ParameterDirection.Input;
                            data_inizio_impegno.Value = evento.data_inizio_impegno;
                            StoreProc.Parameters.Add(data_inizio_impegno);

                            OleDbParameter data_fine_impegno = new OleDbParameter("@data_fine_impegno", OleDbType.Date);
                            data_fine_impegno.Direction = ParameterDirection.Input;
                            data_fine_impegno.Value = evento.data_fine_impegno;
                            StoreProc.Parameters.Add(data_fine_impegno);

                            OleDbParameter impegnoOrario = new OleDbParameter("@impegnoOrario", OleDbType.Boolean);
                            impegnoOrario.Direction = ParameterDirection.Input;
                            impegnoOrario.Value = evento.impegnoOrario;
                            StoreProc.Parameters.Add(impegnoOrario);

                            OleDbParameter impegnoOrario_da = new OleDbParameter("@impegnoOrario_da", OleDbType.VarChar);
                            impegnoOrario_da.Direction = ParameterDirection.Input;
                            impegnoOrario_da.Value = evento.impegnoOrario_da;
                            StoreProc.Parameters.Add(impegnoOrario_da);

                            OleDbParameter impegnoOrario_a = new OleDbParameter("@impegnoOrario_a", OleDbType.VarChar);
                            impegnoOrario_a.Direction = ParameterDirection.Input;
                            impegnoOrario_a.Value = evento.impegnoOrario_a;
                            StoreProc.Parameters.Add(impegnoOrario_a);

                            OleDbParameter produzione = new OleDbParameter("@produzione", OleDbType.VarChar);
                            produzione.Direction = ParameterDirection.Input;
                            produzione.Value = evento.produzione;
                            StoreProc.Parameters.Add(produzione);

                            OleDbParameter lavorazione = new OleDbParameter("@lavorazione", OleDbType.VarChar);
                            lavorazione.Direction = ParameterDirection.Input;
                            lavorazione.Value = evento.lavorazione;
                            StoreProc.Parameters.Add(lavorazione);

                            OleDbParameter indirizzo = new OleDbParameter("@indirizzo", OleDbType.VarChar);
                            indirizzo.Direction = ParameterDirection.Input;
                            indirizzo.Value = evento.indirizzo;
                            StoreProc.Parameters.Add(indirizzo);

                            OleDbParameter luogo = new OleDbParameter("@luogo", OleDbType.VarChar);
                            luogo.Direction = ParameterDirection.Input;
                            luogo.Value = evento.luogo;
                            StoreProc.Parameters.Add(luogo);

                            OleDbParameter codice_lavoro = new OleDbParameter("@codice_lavoro", OleDbType.VarChar);
                            codice_lavoro.Direction = ParameterDirection.Input;
                            codice_lavoro.Value = evento.codice_lavoro;
                            StoreProc.Parameters.Add(codice_lavoro);

                            OleDbParameter nota = new OleDbParameter("@nota", OleDbType.VarChar);
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

        public Esito aggiornaEvento(DatiAgenda evento)
        {
            Esito esito = new Esito();
            try
            {
                using (System.Data.OleDb.OleDbConnection con = new System.Data.OleDb.OleDbConnection(constr))
                {
                    using (System.Data.OleDb.OleDbCommand StoreProc = new System.Data.OleDb.OleDbCommand("UpdateEvento"))
                    {
                        using (System.Data.OleDb.OleDbDataAdapter sda = new System.Data.OleDb.OleDbDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            OleDbParameter id = new OleDbParameter("@id", OleDbType.Integer);
                            id.Direction = ParameterDirection.Input;
                            id.Value = evento.id;
                            StoreProc.Parameters.Add(id);

                            OleDbParameter data_inizio_lavorazione = new OleDbParameter("@data_inizio_lavorazione", OleDbType.Date);
                            data_inizio_lavorazione.Direction = ParameterDirection.Input;
                            data_inizio_lavorazione.Value = evento.data_inizio_lavorazione;
                            StoreProc.Parameters.Add(data_inizio_lavorazione);

                            OleDbParameter data_fine_lavorazione = new OleDbParameter("@data_fine_lavorazione", OleDbType.Date);
                            data_fine_lavorazione.Direction = ParameterDirection.Input;
                            data_fine_lavorazione.Value = evento.data_fine_lavorazione;
                            StoreProc.Parameters.Add(data_fine_lavorazione);

                            OleDbParameter durata_lavorazione = new OleDbParameter("@durata_lavorazione", OleDbType.Integer);
                            durata_lavorazione.Direction = ParameterDirection.Input;
                            durata_lavorazione.Value = evento.durata_lavorazione;
                            StoreProc.Parameters.Add(durata_lavorazione);

                            OleDbParameter id_colonne_agenda = new OleDbParameter("@id_colonne_agenda", OleDbType.Integer);
                            id_colonne_agenda.Direction = ParameterDirection.Input;
                            id_colonne_agenda.Value = evento.id_colonne_agenda;
                            StoreProc.Parameters.Add(id_colonne_agenda);

                            OleDbParameter id_tipologia = new OleDbParameter("@id_tipologia", OleDbType.Integer);
                            id_tipologia.Direction = ParameterDirection.Input;
                            id_tipologia.Value = evento.id_tipologia;
                            StoreProc.Parameters.Add(id_tipologia);

                            OleDbParameter id_stato = new OleDbParameter("@id_stato", OleDbType.Integer);
                            id_stato.Direction = ParameterDirection.Input;
                            id_stato.Value = evento.id_stato;
                            StoreProc.Parameters.Add(id_stato);

                            OleDbParameter id_cliente = new OleDbParameter("@id_cliente", OleDbType.Integer);
                            id_cliente.Direction = ParameterDirection.Input;
                            id_cliente.Value = evento.id_cliente;
                            StoreProc.Parameters.Add(id_cliente);

                            OleDbParameter durata_viaggio_andata = new OleDbParameter("@durata_viaggio_andata", OleDbType.Integer);
                            durata_viaggio_andata.Direction = ParameterDirection.Input;
                            durata_viaggio_andata.Value = evento.durata_viaggio_andata;
                            StoreProc.Parameters.Add(durata_viaggio_andata);

                            OleDbParameter durata_viaggio_ritorno = new OleDbParameter("@durata_viaggio_ritorno", OleDbType.Integer);
                            durata_viaggio_ritorno.Direction = ParameterDirection.Input;
                            durata_viaggio_ritorno.Value = evento.durata_viaggio_ritorno;
                            StoreProc.Parameters.Add(durata_viaggio_ritorno);

                            OleDbParameter data_inizio_impegno = new OleDbParameter("@data_inizio_impegno", OleDbType.Date);
                            data_inizio_impegno.Direction = ParameterDirection.Input;
                            data_inizio_impegno.Value = evento.data_inizio_impegno;
                            StoreProc.Parameters.Add(data_inizio_impegno);

                            OleDbParameter data_fine_impegno = new OleDbParameter("@data_fine_impegno", OleDbType.Date);
                            data_fine_impegno.Direction = ParameterDirection.Input;
                            data_fine_impegno.Value = evento.data_fine_impegno;
                            StoreProc.Parameters.Add(data_fine_impegno);

                            OleDbParameter impegnoOrario = new OleDbParameter("@impegnoOrario", OleDbType.Boolean);
                            impegnoOrario.Direction = ParameterDirection.Input;
                            impegnoOrario.Value = evento.impegnoOrario;
                            StoreProc.Parameters.Add(impegnoOrario);

                            OleDbParameter impegnoOrario_da = new OleDbParameter("@impegnoOrario_da", OleDbType.VarChar);
                            impegnoOrario_da.Direction = ParameterDirection.Input;
                            impegnoOrario_da.Value = evento.impegnoOrario_da;
                            StoreProc.Parameters.Add(impegnoOrario_da);

                            OleDbParameter impegnoOrario_a = new OleDbParameter("@impegnoOrario_a", OleDbType.VarChar);
                            impegnoOrario_a.Direction = ParameterDirection.Input;
                            impegnoOrario_a.Value = evento.impegnoOrario_a;
                            StoreProc.Parameters.Add(impegnoOrario_a);

                            OleDbParameter produzione = new OleDbParameter("@produzione", OleDbType.VarChar);
                            produzione.Direction = ParameterDirection.Input;
                            produzione.Value = evento.produzione;
                            StoreProc.Parameters.Add(produzione);

                            OleDbParameter lavorazione = new OleDbParameter("@lavorazione", OleDbType.VarChar);
                            lavorazione.Direction = ParameterDirection.Input;
                            lavorazione.Value = evento.lavorazione;
                            StoreProc.Parameters.Add(lavorazione);

                            OleDbParameter indirizzo = new OleDbParameter("@indirizzo", OleDbType.VarChar);
                            indirizzo.Direction = ParameterDirection.Input;
                            indirizzo.Value = evento.indirizzo;
                            StoreProc.Parameters.Add(indirizzo);

                            OleDbParameter luogo = new OleDbParameter("@luogo", OleDbType.VarChar);
                            luogo.Direction = ParameterDirection.Input;
                            luogo.Value = evento.luogo;
                            StoreProc.Parameters.Add(luogo);

                            OleDbParameter codice_lavoro = new OleDbParameter("@codice_lavoro", OleDbType.VarChar);
                            codice_lavoro.Direction = ParameterDirection.Input;
                            codice_lavoro.Value = evento.codice_lavoro;
                            StoreProc.Parameters.Add(codice_lavoro);

                            OleDbParameter nota = new OleDbParameter("@nota", OleDbType.VarChar);
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
    }
}