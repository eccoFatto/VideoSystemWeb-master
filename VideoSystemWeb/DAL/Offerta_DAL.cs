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
    public class Offerta_DAL : Base_DAL
    {
        //singleton
        private static volatile Offerta_DAL instance;
        private static object objForLock = new Object();

        private Offerta_DAL() { }

        public static Offerta_DAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Offerta_DAL();
                    }
                }
                return instance;
            }
        }
        public NoteOfferta GetNoteOffertaByIdDatiAgenda(ref Esito esito, int idDatiAgenda)
        {
            NoteOfferta noteOfferta = new NoteOfferta();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM tab_note_offerta WHERE id_dati_agenda = " + idDatiAgenda.ToString();
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
                                    noteOfferta.Id = dt.Rows[0].Field<int>("id");
                                    noteOfferta.Id_dati_agenda = dt.Rows[0].Field<int>("id_dati_agenda");
                                    noteOfferta.Banca = dt.Rows[0].Field<string>("banca");
                                    noteOfferta.Pagamento = dt.Rows[0].Field<int>("pagamento");
                                    noteOfferta.NotaPagamento = dt.Rows[0].Field<string>("notaPagamento");
                                    noteOfferta.Consegna = dt.Rows[0].Field<string>("consegna");
                                    noteOfferta.Note = "";
                                    if (!string.IsNullOrEmpty(dt.Rows[0].Field<string>("note"))) noteOfferta.Note = dt.Rows[0].Field<string>("note");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.Descrizione = "Offerta_DAL.cs - getNoteOffertaByIdDatiAgenda " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return noteOfferta;
        }

        public NoteOfferta GetNoteOffertaById(ref Esito esito, int id)
        {
            NoteOfferta noteOfferta = new NoteOfferta();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM tab_note_offerta WHERE id = " + id.ToString();
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
                                    noteOfferta.Id = dt.Rows[0].Field<int>("id");
                                    noteOfferta.Id_dati_agenda = dt.Rows[0].Field<int>("id_dati_agenda");
                                    noteOfferta.Banca = dt.Rows[0].Field<string>("banca");
                                    noteOfferta.Pagamento = dt.Rows[0].Field<int>("pagamento");
                                    noteOfferta.NotaPagamento = dt.Rows[0].Field<string>("notaPagamento");
                                    noteOfferta.Consegna = dt.Rows[0].Field<string>("consegna");
                                    noteOfferta.Note = dt.Rows[0].Field<string>("note");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.Descrizione = "Offerta_DAL.cs - getNoteOffertaById " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return noteOfferta;

        }

        public int CreaNoteOfferta(NoteOfferta noteOfferta, ref Esito esito)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("InsertNoteOfferta"))
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

                            SqlParameter id_dati_agenda = new SqlParameter("@id_dati_agenda", noteOfferta.Id_dati_agenda);
                            id_dati_agenda.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_dati_agenda);

                            SqlParameter banca = new SqlParameter("@banca", noteOfferta.Banca);
                            banca.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(banca);

                            SqlParameter pagamento = new SqlParameter("@pagamento", noteOfferta.Pagamento);
                            pagamento.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(pagamento);

                            SqlParameter consegna = new SqlParameter("@consegna", noteOfferta.Consegna);
                            consegna.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(consegna);

                            if (string.IsNullOrEmpty(noteOfferta.Note)) noteOfferta.Note = string.Empty;
                            SqlParameter note = new SqlParameter("@note", noteOfferta.Note);
                            note.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(note);

                            SqlParameter notaPagamento = new SqlParameter("@notaPagamento", noteOfferta.NotaPagamento);
                            notaPagamento.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(notaPagamento);


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
                esito.Descrizione = "Offerta_DAL.cs - CreaNoteOfferta " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return 0;
        }

        public Esito AggiornaNoteOfferta(NoteOfferta noteOfferta)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);
            Esito esito = new Esito();
            try
            {
                using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(sqlConstr))
                {
                    using (System.Data.SqlClient.SqlCommand StoreProc = new System.Data.SqlClient.SqlCommand("UpdateNoteOfferta"))
                    {
                        using (System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            System.Data.SqlClient.SqlParameter id = new System.Data.SqlClient.SqlParameter("@id", noteOfferta.Id);
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

                            SqlParameter id_dati_agenda = new SqlParameter("@id_dati_agenda", noteOfferta.Id_dati_agenda);
                            id_dati_agenda.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_dati_agenda);

                            SqlParameter banca = new SqlParameter("@banca", noteOfferta.Banca);
                            banca.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(banca);

                            SqlParameter pagamento = new SqlParameter("@pagamento", noteOfferta.Pagamento);
                            pagamento.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(pagamento);

                            SqlParameter notaPagamento = new SqlParameter("@notaPagamento", noteOfferta.NotaPagamento);
                            notaPagamento.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(notaPagamento);


                            SqlParameter consegna = new SqlParameter("@consegna", noteOfferta.Consegna);
                            consegna.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(consegna);

                            SqlParameter note = new SqlParameter("@note", noteOfferta.Note);
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
                esito.Codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.Descrizione = "Offerta_DAL.cs - AggiornaNoteOfferta " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return esito;
        }

        public Esito EliminaNoteOfferta(int idNotaOfferta)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);
            Esito esito = new Esito();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("DeleteNoteOfferta"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            SqlParameter id = new SqlParameter("@id", SqlDbType.Int);
                            id.Direction = ParameterDirection.Input;
                            id.Value = idNotaOfferta;
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
                esito.Descrizione = "Offerta_DAL.cs - EliminaNoteOfferta " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return esito;
        }

        #region RECUPERA OFFERTA
        public List<string> GetAllCodicilavoro()
        {
            List<string> listaCodiciLavoro = new List<string>();
            Esito esito = new Esito();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT codice_lavoro FROM tab_dati_agenda WHERE codice_lavoro <> '' AND codice_lavoro IS NOT NULL"))
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
                                        listaCodiciLavoro.Add(riga.Field<string>("codice_lavoro"));
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
                esito.Descrizione = "Offerta_DAL.cs - GetAllCodiciLavoro " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return listaCodiciLavoro;
        }

        public List<string> GetAllProduzioni()
        {
            List<string> listaProduzioni = new List<string>();
            Esito esito = new Esito();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT produzione FROM tab_dati_agenda WHERE codice_lavoro <> '' AND codice_lavoro IS NOT NULL"))
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
                                        listaProduzioni.Add(riga.Field<string>("produzione"));
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
                esito.Descrizione = "Offerta_DAL.cs - GetAllProduzioni " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return listaProduzioni;
        }

        public List<string> GetAllLavorazioni()
        {
            List<string> listaLavorazioni = new List<string>();
            Esito esito = new Esito();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT lavorazione FROM tab_dati_agenda WHERE codice_lavoro <> '' AND codice_lavoro IS NOT NULL"))
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
                                        listaLavorazioni.Add(riga.Field<string>("lavorazione"));
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
                esito.Descrizione = "Offerta_DAL.cs - GetAllLavorazioni " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return listaLavorazioni;
        }

        public List<string> GetAllLuoghi()
        {
            List<string> listaLuoghi = new List<string>();
            Esito esito = new Esito();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT luogo FROM tab_dati_agenda WHERE codice_lavoro <> '' AND codice_lavoro IS NOT NULL"))
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
                                        listaLuoghi.Add(riga.Field<string>("luogo"));
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
                esito.Descrizione = "Offerta_DAL.cs - GetAllLuoghi " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return listaLuoghi;
        }

        public Esito GetListaDatiAgendaByFiltri(string dataLavorazione, string idTipologia, string idCliente, string produzione, string lavorazione, string luogo, string codiceLavoro, ref List<DatiAgenda> listaDatiAgenda)
        {
            Esito esito = new Esito();

            string filtri = string.Empty;
            if (!string.IsNullOrEmpty(dataLavorazione))
            {
                filtri += " and data_inizio_lavorazione = '" + dataLavorazione + "' ";
            }
            if (!string.IsNullOrEmpty(idTipologia))
            {
                filtri += " and id_tipologia = " + idTipologia;
            }
            if (!string.IsNullOrEmpty(idCliente))
            {
                filtri += " and id_cliente = " + idCliente;
            }
            if (!string.IsNullOrEmpty(produzione))
            {
                filtri += " and produzione = '" + produzione + "' ";
            }
            if (!string.IsNullOrEmpty(lavorazione))
            {
                filtri += " and lavorazione = '" + lavorazione + "' ";
            }
            if (!string.IsNullOrEmpty(luogo))
            {
                filtri += " and luogo = '" + luogo + "' ";
            }
            if (!string.IsNullOrEmpty(codiceLavoro))
            {
                filtri += " and codice_lavoro = '" + codiceLavoro + "'";
            }

            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "select * from tab_dati_agenda where id_stato > 1 " + filtri;
                    query += " order by data_inizio_lavorazione desc";
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
                                        DatiAgenda datoAgenda = new DatiAgenda
                                        {
                                            id = riga.Field<int>("id"),
                                            id_colonne_agenda = riga.Field<int>("id_colonne_agenda"),
                                            id_stato = riga.Field<int>("id_stato"),
                                            data_inizio_lavorazione = riga["data_inizio_lavorazione"] != DBNull.Value ? riga.Field<DateTime>("data_inizio_lavorazione") : DateTime.MinValue,
                                            data_fine_lavorazione = riga["data_fine_lavorazione"] != DBNull.Value ? riga.Field<DateTime>("data_fine_lavorazione") : DateTime.MaxValue,
                                            durata_lavorazione = riga.Field<int>("durata_lavorazione"),
                                            id_tipologia = riga.Field<int?>("id_tipologia"),
                                            id_cliente = riga.Field<int>("id_cliente"),
                                            durata_viaggio_andata = riga.Field<int>("durata_viaggio_andata"),
                                            durata_viaggio_ritorno = riga.Field<int>("durata_viaggio_ritorno"),
                                            data_inizio_impegno = riga["data_inizio_impegno"] != DBNull.Value ? riga.Field<DateTime>("data_inizio_impegno") : DateTime.MinValue,
                                            data_fine_impegno = riga["data_fine_impegno"] != DBNull.Value ? riga.Field<DateTime>("data_fine_impegno") : DateTime.MaxValue,
                                            impegnoOrario = riga.Field<bool>("impegnoOrario"),
                                            impegnoOrario_da = riga.Field<string>("impegnoOrario_da"),
                                            impegnoOrario_a = riga.Field<string>("impegnoOrario_a"),
                                            produzione = riga.Field<string>("produzione"),
                                            lavorazione = riga.Field<string>("lavorazione"),
                                            indirizzo = riga.Field<string>("indirizzo"),
                                            luogo = riga.Field<string>("luogo"),
                                            codice_lavoro = riga.Field<string>("codice_lavoro"),
                                            nota = riga.Field<string>("nota")
                                        };

                                        listaDatiAgenda.Add(datoAgenda);
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
                esito.Descrizione = "Offerta_DAL.cs - GetListaDatiAgendaByFiltri " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return esito;
        }

        public Esito GetListaDatiArticoliByIdDatiAgenda(string idDatiAgenda, ref List<DatiArticoli> listaDatiArticoli)
        {
            Esito esito = new Esito();

            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "select * from dati_articoli where idDatiAgenda = " + idDatiAgenda;
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
                                        DatiArticoli datoArticoli = new DatiArticoli
                                        {
                                            Id = riga.Field<int>("id"),
                                            IdArtArticoli = riga.Field<int>("idArtArticoli"),
                                            IdTipoGenere = riga.Field<int>("idTipoGenere"),
                                            IdTipoGruppo = riga.Field<int>("idTipoGruppo"),
                                            IdTipoSottogruppo = riga.Field<int>("idTipoSottogruppo"),
                                            IdDatiAgenda = riga.Field<int>("idDatiAgenda"),
                                            Descrizione = riga.Field<string>("descrizione"),
                                            DescrizioneLunga = riga.Field<string>("descrizioneLunga"),
                                            Stampa = riga.Field<bool>("stampa"),
                                            Prezzo = riga.Field<decimal>("prezzo"),
                                            Costo = riga.Field<decimal>("costo"),
                                            Iva = riga.Field<int>("iva"),
                                            Quantita = riga.Field<int>("quantita")
                                        };

                                        listaDatiArticoli.Add(datoArticoli);
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
                esito.Descrizione = "Offerta_DAL.cs - GetListaDatiArticoliByIdDatiAgenda " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return esito;
        }

        public Esito InserisciOffertaRecuperata(List<DatiArticoli> listaDatiArticoli)
        {
            Esito esito = new Esito();
            int idDatiAgenda = SessionManager.EventoSelezionato.id;
            using (SqlConnection con = new SqlConnection(sqlConstr))
            {

                using (SqlCommand StoreProc = new SqlCommand("InsertDatiArticoli"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        StoreProc.Connection = con;
                        StoreProc.Connection.Open();
                        try
                        {
                            foreach (DatiArticoli datoArticoli in listaDatiArticoli)
                            {
                                CostruisciSP_InsertDatiArticoli(StoreProc, sda, idDatiAgenda, datoArticoli);
                                StoreProc.ExecuteNonQuery();
                            }
                        }
                        catch (Exception ex)
                        {
                            esito.Codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                            esito.Descrizione = "Offerta_DAL.cs - InserisciOffertaRecuperata " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;

                            log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
                        }
                    }
                }
            }
            return esito;
        }
        #endregion
    }
}