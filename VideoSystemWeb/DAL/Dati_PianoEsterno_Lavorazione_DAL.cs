using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using VideoSystemWeb.BLL;
using VideoSystemWeb.Entity;

namespace VideoSystemWeb.DAL
{
    public class Dati_PianoEsterno_Lavorazione_DAL: Base_DAL
    {
            //singleton
            private static volatile Dati_PianoEsterno_Lavorazione_DAL instance;
            private static object objForLock = new Object();

            private Dati_PianoEsterno_Lavorazione_DAL() { }

            public static Dati_PianoEsterno_Lavorazione_DAL Instance
            {
                get
                {
                    if (instance == null)
                    {
                        lock (objForLock)
                        {
                            if (instance == null)
                                instance = new Dati_PianoEsterno_Lavorazione_DAL();
                        }
                    }
                    return instance;
                }
            }

            public List<DatiPianoEsternoLavorazione> getDatiPianoEsternoLavorazioneByIdDatiLavorazione(ref Esito esito, int idDatiLavorazione)
            {
                List<DatiPianoEsternoLavorazione> listaDatiPianoEsterno = new List<DatiPianoEsternoLavorazione>();
                try
                {
                    using (SqlConnection con = new SqlConnection(sqlConstr))
                    {
                        string query = "SELECT * FROM dati_pianoEsterno_lavorazione WHERE idDatiLavorazione = " + idDatiLavorazione.ToString();
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
                                        DatiPianoEsternoLavorazione datiPianoEsterno = new DatiPianoEsternoLavorazione
                                        {
                                            Id = riga.Field<int>("id"),
                                            IdDatiLavorazione = riga.Field<int>("idDatiLavorazione"),
                                            IdCollaboratori = riga.Field<int?>("idCollaboratori"),
                                            IdFornitori = riga.Field<int?>("idFornitori"),
                                            IdIntervento = riga.Field<int?>("idIntervento"),
                                            Diaria = riga.Field<bool?>("diaria"),
                                            ImportoDiaria = riga.Field<decimal?>("importoDiaria"),
                                            Albergo = riga.Field<bool?>("albergo"),
                                            Data = riga.Field<DateTime?>("data"),
                                            Orario = riga.Field<DateTime?>("orario"),
                                            Nota = riga.Field<string>("nota"),
                                            NumOccorrenza = riga.Field<int?>("numOccorrenza") == null ? 0 : riga.Field<int>("numOccorrenza")
                                        };

                                        listaDatiPianoEsterno.Add(datiPianoEsterno);
                                        }
                                    }
                                    else
                                    {
                                        esito.Codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                        esito.Descrizione = "Nessun dato trovato nella tabella dati_pianoEsterno_lavorazione";
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                    esito.Descrizione = "Dati_PianoEsterno_LavorazioneDAL.cs - getDatiPianoEsternoLavorazioneByIdDatiLavorazione " + Environment.NewLine + ex.Message;

                    log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
                }

                return listaDatiPianoEsterno;
            }

            public Esito EliminaDatiPianoEsternoLavorazioneByIdDatiLavorazione(int idDatiLavorazione)
            {
                Esito esito = new Esito();
                Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);
                try
                {
                    using (SqlConnection con = new SqlConnection(sqlConstr))
                    {
                        using (SqlCommand StoreProc = new SqlCommand("DeleteDatiPianoEsternoLavorazioneByIdDatiLavorazione"))
                        {
                            using (SqlDataAdapter sda = new SqlDataAdapter())
                            {
                                StoreProc.Connection = con;
                                sda.SelectCommand = StoreProc;
                                StoreProc.CommandType = CommandType.StoredProcedure;

                                SqlParameter parIdDatiLavorazione = new SqlParameter("@idDatiLavorazione", SqlDbType.Int);
                                parIdDatiLavorazione.Direction = ParameterDirection.Input;
                                parIdDatiLavorazione.Value = idDatiLavorazione;
                                StoreProc.Parameters.Add(parIdDatiLavorazione);

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
                    esito.Descrizione = "Dati_Articoli_Lavorazione_DAL.cs - EliminaDatiPianoEsternoLavorazioneByIdDatiLavorazione " + Environment.NewLine + ex.Message;

                    log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
                }

                return esito;
            }
    }
}