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
    public class Dati_Articoli_Lavorazione_DAL: Base_DAL
    {
        //singleton
        private static volatile Dati_Articoli_Lavorazione_DAL instance;
        private static object objForLock = new Object();

        private Dati_Articoli_Lavorazione_DAL() { }

        public static Dati_Articoli_Lavorazione_DAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Dati_Articoli_Lavorazione_DAL();
                    }
                }
                return instance;
            }
        }

        public List<DatiArticoliLavorazione> getDatiArticoliLavorazioneByIdDatiLavorazione(ref Esito esito, int idDatiLavorazione)
        {
            List<DatiArticoliLavorazione> listaDatiArticoli = new List<DatiArticoliLavorazione>();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM dati_articoli_lavorazione WHERE idDatiLavorazione = " + idDatiLavorazione.ToString();
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

                                        DatiArticoliLavorazione datiArticoliLavorazione = new DatiArticoliLavorazione();
                                        datiArticoliLavorazione.Id = riga.Field<int>("id");
                                        datiArticoliLavorazione.IdDatiLavorazione= riga.Field<int>("idDatiLavorazione");
                                        datiArticoliLavorazione.IdArtArticoli = riga.Field<int>("idArtArticoli");
                                        datiArticoliLavorazione.IdTipoGenere = riga.Field<int>("idTipoGenere");
                                        datiArticoliLavorazione.IdTipoGruppo = riga.Field<int>("idTipoGruppo");
                                        datiArticoliLavorazione.IdTipoSottogruppo = riga.Field<int>("idTipoSottogruppo");
                                        datiArticoliLavorazione.IdCollaboratori = riga.Field<int?>("idCollaboratori");
                                        datiArticoliLavorazione.IdFornitori = riga.Field<int?>("idFornitori");
                                        datiArticoliLavorazione.IdTipoPagamento = riga.Field<int?>("idTipoPagamento");
                                        datiArticoliLavorazione.Descrizione = riga.Field<string>("descrizione");
                                        datiArticoliLavorazione.DescrizioneLunga = riga.Field<string>("descrizioneLunga");
                                        datiArticoliLavorazione.Stampa = riga.Field<bool>("stampa");
                                        datiArticoliLavorazione.Prezzo = riga.Field<decimal>("prezzo");
                                        datiArticoliLavorazione.Costo = riga.Field<decimal>("costo");
                                        datiArticoliLavorazione.Iva = riga.Field<int>("iva");
                                        datiArticoliLavorazione.Data = riga.Field<DateTime?>("data");
                                        datiArticoliLavorazione.Tv = riga.Field<int?>("tv");
                                        datiArticoliLavorazione.Nota = riga.Field<string>("nota");
                                        datiArticoliLavorazione.FP_netto = riga.Field<decimal?>("fp_netto");
                                        datiArticoliLavorazione.FP_lordo = riga.Field<decimal?>("fp_lordo");
                                        datiArticoliLavorazione.UsaCostoFP = riga.Field<bool?>("usaCostoFP");

                                        listaDatiArticoli.Add(datiArticoliLavorazione);
                                    }
                                }
                                else
                                {
                                    esito.Codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                    esito.Descrizione = "Nessun dato trovato nella tabella dati_articoli_lavorazione";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.Descrizione = "Dati_Articoli_LavorazioneDAL.cs - getDatiArticoliByIdDatiLavorazione " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return listaDatiArticoli;
        }

        public Esito EliminaDatiArticoloLavorazioneByIdDatiLavorazione(int idDatiLavorazione)
        {
            Esito esito = new Esito();
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("DeleteDatiArticoliLavorazioneByIdDatiLavorazione"))
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
                esito.Descrizione = "Dati_Articoli_Lavorazione_DAL.cs - EliminaDatiArticoloByIdDatiLavorazione " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return esito;
        }
    }
}