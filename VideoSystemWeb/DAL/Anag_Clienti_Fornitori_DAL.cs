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
    public class Anag_Clienti_Fornitori_DAL : Base_DAL
    {
        //singleton
        private static volatile Anag_Clienti_Fornitori_DAL instance;
        private static object objForLock = new Object();

        private Anag_Clienti_Fornitori_DAL() { }

        public static Anag_Clienti_Fornitori_DAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Anag_Clienti_Fornitori_DAL();
                    }
                }
                return instance;
            }
        }

        public Anag_Clienti_Fornitori getAziendaById(int idAzienda, ref Esito esito)
        {
            Anag_Clienti_Fornitori azienda = new Anag_Clienti_Fornitori();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM anag_clienti_fornitori where id = " + idAzienda.ToString();
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
                                    azienda.Id = dt.Rows[0].Field<int>("id");
                                    azienda.Attivo = dt.Rows[0].Field<bool>("attivo");
                                    azienda.CapLegale = dt.Rows[0].Field<string>("capLegale");
                                    azienda.CapOperativo = dt.Rows[0].Field<string>("capOperativo");
                                    azienda.Cliente = dt.Rows[0].Field<bool>("cliente");
                                    azienda.CodiceFiscale = dt.Rows[0].Field<string>("codiceFiscale");
                                    azienda.CodiceIdentificativo = dt.Rows[0].Field<string>("codiceIdentificativo");
                                    azienda.ComuneLegale = dt.Rows[0].Field<string>("comuneLegale");
                                    azienda.ComuneOperativo = dt.Rows[0].Field<string>("comuneOperativo");
                                    azienda.Email = dt.Rows[0].Field<string>("email");
                                    azienda.Fax = dt.Rows[0].Field<string>("fax");
                                    azienda.Fornitore = dt.Rows[0].Field<bool>("fornitore");
                                    azienda.Iban = dt.Rows[0].Field<string>("iban");
                                    azienda.TipoIndirizzoLegale = dt.Rows[0].Field<string>("tipoIndirizzoLegale");
                                    azienda.IndirizzoLegale = dt.Rows[0].Field<string>("indirizzoLegale");
                                    azienda.TipoIndirizzoOperativo = dt.Rows[0].Field<string>("tipoIndirizzoOperativo");
                                    azienda.IndirizzoOperativo = dt.Rows[0].Field<string>("indirizzoOperativo");
                                    azienda.NazioneLegale = dt.Rows[0].Field<string>("nazioneLegale");
                                    azienda.NazioneOperativo = dt.Rows[0].Field<string>("nazioneOperativo");
                                    azienda.Note = dt.Rows[0].Field<string>("note");
                                    azienda.NumeroCivicoLegale = dt.Rows[0].Field<string>("numeroCivicoLegale");
                                    azienda.NumeroCivicoOperativo = dt.Rows[0].Field<string>("numeroCivicoOperativo");
                                    azienda.Pagamento = dt.Rows[0].Field<int>("pagamento");
                                    azienda.PartitaIva = dt.Rows[0].Field<string>("partitaIva");
                                    azienda.Pec = dt.Rows[0].Field<string>("pec");
                                    azienda.ProvinciaLegale = dt.Rows[0].Field<string>("provinciaLegale");
                                    azienda.ProvinciaOperativo = dt.Rows[0].Field<string>("provinciaOperativo");
                                    azienda.RagioneSociale = dt.Rows[0].Field<string>("ragioneSociale");
                                    azienda.Tipo = dt.Rows[0].Field<string>("tipo");
                                    azienda.WebSite = dt.Rows[0].Field<string>("webSite");

                                    azienda.Referenti = Anag_Referente_Clienti_Fornitori_DAL.Instance.getReferentiByIdAzienda(ref esito, azienda.Id);
                                }
                                else
                                {
                                    esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                    esito.descrizione = "Azienda con id " + idAzienda.ToString() + " non trovato in tabella anag_clienti_fornitori ";
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
            return azienda;
        }

        public List<Anag_Clienti_Fornitori> CaricaListaAziende(ref Esito esito, bool soloAttivi = true)
        {
            List<Anag_Clienti_Fornitori> listaAziende = new List<Anag_Clienti_Fornitori>();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM anag_clienti_fornitori";
                    if (soloAttivi) query += " WHERE ATTIVO = 1";
                    query += " ORDER BY ragioneSociale";
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
                                        Anag_Clienti_Fornitori azienda = new Anag_Clienti_Fornitori();
                                        azienda.Id = riga.Field<int>("id");
                                        azienda.Attivo = riga.Field<bool>("attivo");
                                        azienda.CapLegale = riga.Field<string>("capLegale");
                                        azienda.CapOperativo = riga.Field<string>("capOperativo");
                                        azienda.Cliente = riga.Field<bool>("cliente");
                                        azienda.CodiceFiscale = riga.Field<string>("codiceFiscale");
                                        azienda.CodiceIdentificativo = riga.Field<string>("codiceIdentificativo");
                                        azienda.ComuneLegale = riga.Field<string>("comuneLegale");
                                        azienda.ComuneOperativo = riga.Field<string>("comuneOperativo");
                                        azienda.Email = riga.Field<string>("email");
                                        azienda.Fax = riga.Field<string>("fax");
                                        azienda.Fornitore = riga.Field<bool>("fornitore");
                                        azienda.Iban = riga.Field<string>("iban");
                                        azienda.TipoIndirizzoLegale = riga.Field<string>("tipoIndirizzoLegale");
                                        azienda.IndirizzoLegale = riga.Field<string>("indirizzoLegale");
                                        azienda.TipoIndirizzoOperativo = riga.Field<string>("tipoIndirizzoOperativo");
                                        azienda.IndirizzoOperativo = riga.Field<string>("indirizzoOperativo");
                                        azienda.NazioneLegale = riga.Field<string>("nazioneLegale");
                                        azienda.NazioneOperativo = riga.Field<string>("nazioneOperativo");
                                        azienda.Note = riga.Field<string>("note");
                                        azienda.NumeroCivicoLegale = riga.Field<string>("numeroCivicoLegale");
                                        azienda.NumeroCivicoOperativo = riga.Field<string>("numeroCivicoOperativo");
                                        azienda.Pagamento = riga.Field<int>("pagamento");
                                        azienda.PartitaIva = riga.Field<string>("partitaIva");
                                        azienda.Pec = riga.Field<string>("pec");
                                        azienda.ProvinciaLegale = riga.Field<string>("provinciaLegale");
                                        azienda.ProvinciaOperativo = riga.Field<string>("provinciaOperativo");
                                        azienda.RagioneSociale = riga.Field<string>("ragioneSociale");
                                        azienda.WebSite = riga.Field<string>("webSite");

                                        azienda.Referenti = Anag_Referente_Clienti_Fornitori_DAL.Instance.getReferentiByIdAzienda(ref esito, azienda.Id);

                                        listaAziende.Add(azienda);
                                    }
                                }
                                else
                                {
                                    esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                    esito.descrizione = "Nessun dato trovato nella tabella anag_clienti_fornitori ";
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

            return listaAziende;
        }

        public int CreaAzienda(Anag_Clienti_Fornitori azienda, Anag_Utenti utente, ref Esito esito)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("InsertClienteFornitore"))
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

                            SqlParameter attivo = new SqlParameter("@attivo", azienda.Attivo);
                            attivo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(attivo);

                            SqlParameter capLegale = new SqlParameter("@capLegale", azienda.CapLegale);
                            capLegale.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(capLegale);

                            SqlParameter capOperativo = new SqlParameter("@capOperativo", azienda.CapOperativo);
                            capOperativo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(capOperativo);

                            SqlParameter cliente = new SqlParameter("@cliente", azienda.Cliente);
                            cliente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(cliente);

                            SqlParameter codiceFiscale = new SqlParameter("@codiceFiscale", azienda.CodiceFiscale);
                            codiceFiscale.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(codiceFiscale);

                            SqlParameter codiceIdentificativo = new SqlParameter("@codiceIdentificativo", azienda.CodiceIdentificativo);
                            codiceIdentificativo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(codiceIdentificativo);

                            SqlParameter comuneLegale = new SqlParameter("@comuneLegale", azienda.ComuneLegale);
                            comuneLegale.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(comuneLegale);

                            SqlParameter comuneOperativo = new SqlParameter("@comuneOperativo", azienda.ComuneOperativo);
                            comuneOperativo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(comuneOperativo);

                            SqlParameter email = new SqlParameter("@email", azienda.Email);
                            email.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(email);

                            SqlParameter fax = new SqlParameter("@fax", azienda.Fax);
                            fax.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(fax);

                            SqlParameter fornitore = new SqlParameter("@fornitore", azienda.Fornitore);
                            fornitore.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(fornitore);

                            SqlParameter iban = new SqlParameter("@iban", azienda.Iban);
                            iban.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(iban);

                            SqlParameter indirizzoLegale = new SqlParameter("@indirizzoLegale", azienda.IndirizzoLegale);
                            indirizzoLegale.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(indirizzoLegale);

                            SqlParameter indirizzoOperativo = new SqlParameter("@indirizzoOperativo", azienda.IndirizzoOperativo);
                            indirizzoOperativo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(indirizzoOperativo);

                            SqlParameter nazioneLegale = new SqlParameter("@nazioneLegale", azienda.NazioneLegale);
                            nazioneLegale.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nazioneLegale);

                            SqlParameter nazioneOperativo = new SqlParameter("@nazioneOperativo", azienda.NazioneOperativo);
                            nazioneOperativo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nazioneOperativo);

                            SqlParameter note = new SqlParameter("@note", azienda.Note);
                            note.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(note);

                            SqlParameter numeroCivicoLegale = new SqlParameter("@numeroCivicoLegale", azienda.NumeroCivicoLegale);
                            numeroCivicoLegale.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(numeroCivicoLegale);

                            SqlParameter numeroCivicoOperativo = new SqlParameter("@numeroCivicoOperativo", azienda.NumeroCivicoOperativo);
                            numeroCivicoOperativo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(numeroCivicoOperativo);

                            SqlParameter pagamento = new SqlParameter("@pagamento", azienda.Pagamento);
                            pagamento.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(pagamento);

                            SqlParameter partitaIva = new SqlParameter("@partitaIva", azienda.PartitaIva);
                            partitaIva.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(partitaIva);

                            SqlParameter pec = new SqlParameter("@pec", azienda.Pec);
                            pec.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(pec);

                            SqlParameter provinciaLegale = new SqlParameter("@provinciaLegale", azienda.ProvinciaLegale);
                            provinciaLegale.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(provinciaLegale);

                            SqlParameter provinciaOperativo = new SqlParameter("@provinciaOperativo", azienda.ProvinciaOperativo);
                            provinciaOperativo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(provinciaOperativo);

                            SqlParameter ragioneSociale = new SqlParameter("@ragioneSociale", azienda.RagioneSociale);
                            ragioneSociale.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(ragioneSociale);

                            SqlParameter telefono = new SqlParameter("@telefono", azienda.Telefono);
                            telefono.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(telefono);

                            SqlParameter tipo = new SqlParameter("@tipo", azienda.Tipo);
                            tipo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(tipo);

                            SqlParameter tipoIndirizzoLegale = new SqlParameter("@tipoIndirizzoLegale", azienda.TipoIndirizzoLegale);
                            tipoIndirizzoLegale.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(tipoIndirizzoLegale);

                            SqlParameter tipoIndirizzoOperativo = new SqlParameter("@tipoIndirizzoOperativo", azienda.TipoIndirizzoOperativo);
                            tipoIndirizzoOperativo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(tipoIndirizzoOperativo);

                            SqlParameter webSite = new SqlParameter("@webSite", azienda.WebSite);
                            webSite.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(webSite);

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
                esito.descrizione = "Anag_Clienti_Fornitori_DAL.cs - CreaAzienda " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return 0;
        }

        public Esito AggiornaAzienda(Anag_Clienti_Fornitori azienda, Anag_Utenti utente)
        {
            Esito esito = new Esito();
            try
            {
                using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(sqlConstr))
                {
                    using (System.Data.SqlClient.SqlCommand StoreProc = new System.Data.SqlClient.SqlCommand("UpdateClienteFornitore"))
                    {
                        using (System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            System.Data.SqlClient.SqlParameter id = new System.Data.SqlClient.SqlParameter("@id", azienda.Id);
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

                            SqlParameter attivo = new SqlParameter("@attivo", azienda.Attivo);
                            attivo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(attivo);

                            SqlParameter capLegale = new SqlParameter("@capLegale", azienda.CapLegale);
                            capLegale.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(capLegale);

                            SqlParameter capOperativo = new SqlParameter("@capOperativo", azienda.CapOperativo);
                            capOperativo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(capOperativo);

                            SqlParameter cliente = new SqlParameter("@cliente", azienda.Cliente);
                            cliente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(cliente);

                            SqlParameter codiceFiscale = new SqlParameter("@codiceFiscale", azienda.CodiceFiscale);
                            codiceFiscale.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(codiceFiscale);

                            SqlParameter codiceIdentificativo = new SqlParameter("@codiceIdentificativo", azienda.CodiceIdentificativo);
                            codiceIdentificativo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(codiceIdentificativo);

                            SqlParameter comuneLegale = new SqlParameter("@comuneLegale", azienda.ComuneLegale);
                            comuneLegale.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(comuneLegale);

                            SqlParameter comuneOperativo = new SqlParameter("@comuneOperativo", azienda.ComuneOperativo);
                            comuneOperativo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(comuneOperativo);

                            SqlParameter email = new SqlParameter("@email", azienda.Email);
                            email.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(email);

                            SqlParameter fax = new SqlParameter("@fax", azienda.Fax);
                            fax.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(fax);

                            SqlParameter fornitore = new SqlParameter("@fornitore", azienda.Fornitore);
                            fornitore.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(fornitore);

                            SqlParameter iban = new SqlParameter("@iban", azienda.Iban);
                            iban.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(iban);

                            SqlParameter indirizzoLegale = new SqlParameter("@indirizzoLegale", azienda.IndirizzoLegale);
                            indirizzoLegale.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(indirizzoLegale);

                            SqlParameter indirizzoOperativo = new SqlParameter("@indirizzoOperativo", azienda.IndirizzoOperativo);
                            indirizzoOperativo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(indirizzoOperativo);

                            SqlParameter nazioneLegale = new SqlParameter("@nazioneLegale", azienda.NazioneLegale);
                            nazioneLegale.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nazioneLegale);

                            SqlParameter nazioneOperativo = new SqlParameter("@nazioneOperativo", azienda.NazioneOperativo);
                            nazioneOperativo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nazioneOperativo);

                            SqlParameter note = new SqlParameter("@note", azienda.Note);
                            note.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(note);

                            SqlParameter numeroCivicoLegale = new SqlParameter("@numeroCivicoLegale", azienda.NumeroCivicoLegale);
                            numeroCivicoLegale.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(numeroCivicoLegale);

                            SqlParameter numeroCivicoOperativo = new SqlParameter("@numeroCivicoOperativo", azienda.NumeroCivicoOperativo);
                            numeroCivicoOperativo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(numeroCivicoOperativo);

                            SqlParameter pagamento = new SqlParameter("@pagamento", azienda.Pagamento);
                            pagamento.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(pagamento);

                            SqlParameter partitaIva = new SqlParameter("@partitaIva", azienda.PartitaIva);
                            partitaIva.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(partitaIva);

                            SqlParameter pec = new SqlParameter("@pec", azienda.Pec);
                            pec.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(pec);

                            SqlParameter provinciaLegale = new SqlParameter("@provinciaLegale", azienda.ProvinciaLegale);
                            provinciaLegale.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(provinciaLegale);

                            SqlParameter provinciaOperativo = new SqlParameter("@provinciaOperativo", azienda.ProvinciaOperativo);
                            provinciaOperativo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(provinciaOperativo);

                            SqlParameter ragioneSociale = new SqlParameter("@ragioneSociale", azienda.RagioneSociale);
                            ragioneSociale.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(ragioneSociale);

                            SqlParameter telefono = new SqlParameter("@telefono", azienda.Telefono);
                            telefono.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(telefono);

                            SqlParameter tipo = new SqlParameter("@tipo", azienda.Tipo);
                            tipo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(tipo);

                            SqlParameter tipoIndirizzoLegale = new SqlParameter("@tipoIndirizzoLegale", azienda.TipoIndirizzoLegale);
                            tipoIndirizzoLegale.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(tipoIndirizzoLegale);

                            SqlParameter tipoIndirizzoOperativo = new SqlParameter("@tipoIndirizzoOperativo", azienda.TipoIndirizzoOperativo);
                            tipoIndirizzoOperativo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(tipoIndirizzoOperativo);

                            SqlParameter webSite = new SqlParameter("@webSite", azienda.WebSite);
                            webSite.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(webSite);

                            StoreProc.Connection.Open();

                            int iReturn = StoreProc.ExecuteNonQuery();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.descrizione = "Anag_Clienti_Fornitori_DAL.cs - aggiornaAzienda " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

        public Esito EliminaAzienda(int idAzienda, Anag_Utenti utente)
        {
            Esito esito = new Esito();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("DeleteClienteFornitore"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            SqlParameter id = new SqlParameter("@id", SqlDbType.Int);
                            id.Direction = ParameterDirection.Input;
                            id.Value = idAzienda;
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
                esito.descrizione = "Anag_Clienti_Fornitori_DAL.cs - EliminaAzienda " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

        public Esito RemoveAzienda(int idAzienda)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);
            Esito esito = new Esito();
            try
            {
                using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(sqlConstr))
                {
                    using (System.Data.SqlClient.SqlCommand StoreProc = new System.Data.SqlClient.SqlCommand("RemoveClienteFornitore"))
                    {
                        using (System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            System.Data.SqlClient.SqlParameter id = new System.Data.SqlClient.SqlParameter("@id", idAzienda);
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


                            StoreProc.Connection.Open();

                            int iReturn = StoreProc.ExecuteNonQuery();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.descrizione = "Anag_Clienti_Fornitori_DAL.cs - RemoveAzienda " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

    }
}