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
    public class DocumentiTrasporto_DAL : Base_DAL
    {
        //singleton
        private static volatile DocumentiTrasporto_DAL instance;
        private static object objForLock = new Object();

        private DocumentiTrasporto_DAL() { }

        public static DocumentiTrasporto_DAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new DocumentiTrasporto_DAL();
                    }
                }
                return instance;
            }
        }


        public DocumentiTrasporto getDocumentoTrasportoById(ref Esito esito, Int64 id)
        {
            DocumentiTrasporto documentoTrasporto = new DocumentiTrasporto();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM dati_documenti_Trasporto WHERE id = " + id.ToString();
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
                                    documentoTrasporto.Id = dt.Rows[0].Field<int>("id");
                                    if (!DBNull.Value.Equals(dt.Rows[0]["dataTrasporto"])) documentoTrasporto.DataTrasporto = dt.Rows[0].Field<DateTime>("dataTrasporto");

                                    documentoTrasporto.NumeroDocumentoTrasporto = dt.Rows[0].Field<string>("numeroDocumentoTrasporto");
                                    documentoTrasporto.Causale = dt.Rows[0].Field<string>("causale");
                                    documentoTrasporto.Destinatario = dt.Rows[0].Field<string>("destinatario");
                                    documentoTrasporto.TipoIndirizzo = dt.Rows[0].Field<string>("tipoIndirizzo");
                                    documentoTrasporto.Indirizzo = dt.Rows[0].Field<string>("indirizzo");
                                    documentoTrasporto.NumeroCivico = dt.Rows[0].Field<string>("numeroCivico");
                                    documentoTrasporto.Cap = dt.Rows[0].Field<string>("cap");
                                    documentoTrasporto.Comune = dt.Rows[0].Field<string>("comune");
                                    documentoTrasporto.Provincia = dt.Rows[0].Field<string>("provincia");
                                    documentoTrasporto.Nazione = dt.Rows[0].Field<string>("nazione");
                                    documentoTrasporto.PartitaIva = dt.Rows[0].Field<string>("partitaIva");
                                    documentoTrasporto.Peso = dt.Rows[0].Field<string>("peso");
                                    documentoTrasporto.NumeroColli = dt.Rows[0].Field<int>("numeroColli");
                                    documentoTrasporto.Trasportatore = dt.Rows[0].Field<string>("trasportatore");
                                    documentoTrasporto.AttrezzatureTrasporto = getAttrezzatureTrasportoByIdDocumentoTrasporto(ref esito,documentoTrasporto.Id);
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

            return documentoTrasporto;

        }

        public int CreaDocumentoTrasporto(DocumentiTrasporto documentoTrasporto, ref Esito esito)
        {

            try
            {
                Anag_Utenti utente = (Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE];
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("InsertDocumentoTrasporto"))
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

                            SqlParameter numeroDocumentoTrasporto = new SqlParameter("@numeroDocumentoTrasporto", documentoTrasporto.NumeroDocumentoTrasporto);
                            numeroDocumentoTrasporto.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(numeroDocumentoTrasporto);

                            SqlParameter dataTrasporto = new SqlParameter("@dataTrasporto", documentoTrasporto.DataTrasporto);
                            dataTrasporto.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(dataTrasporto);

                            SqlParameter causale = new SqlParameter("@causale", documentoTrasporto.Causale);
                            causale.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(causale);

                            SqlParameter destinatario = new SqlParameter("@destinatario", documentoTrasporto.Destinatario);
                            destinatario.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(destinatario);

                            SqlParameter tipoIndirizzo = new SqlParameter("@tipoIndirizzo", documentoTrasporto.TipoIndirizzo);
                            tipoIndirizzo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(tipoIndirizzo);

                            SqlParameter indirizzo = new SqlParameter("@indirizzo", documentoTrasporto.Indirizzo);
                            indirizzo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(indirizzo);

                            SqlParameter numeroCivico = new SqlParameter("@numeroCivico", documentoTrasporto.NumeroCivico);
                            numeroCivico.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(numeroCivico);

                            SqlParameter cap = new SqlParameter("@cap", documentoTrasporto.Cap);
                            cap.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(cap);

                            SqlParameter comune = new SqlParameter("@comune", documentoTrasporto.Comune);
                            comune.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(comune);

                            SqlParameter provincia = new SqlParameter("@provincia", documentoTrasporto.Provincia);
                            provincia.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(provincia);

                            SqlParameter nazione = new SqlParameter("@nazione", documentoTrasporto.Nazione);
                            nazione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nazione);

                            SqlParameter partitaIva = new SqlParameter("@partitaIva", documentoTrasporto.PartitaIva);
                            partitaIva.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(partitaIva);

                            SqlParameter numeroColli = new SqlParameter("@numeroColli", documentoTrasporto.NumeroColli);
                            numeroColli.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(numeroColli);

                            SqlParameter peso = new SqlParameter("@peso", documentoTrasporto.Peso);
                            peso.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(peso);

                            SqlParameter trasportatore = new SqlParameter("@trasportatore", documentoTrasporto.Trasportatore);
                            trasportatore.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(trasportatore);

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
                esito.Descrizione = "DocumentiTrasporto_DAL.cs - CreaDocumentoTrasporto " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return 0;
        }

        public Esito AggiornaDocumentoTrasporto(DocumentiTrasporto documentoTrasporto)
        {
            Anag_Utenti utente = (Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE];
            Esito esito = new Esito();
            try
            {
                using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(sqlConstr))
                {
                    using (System.Data.SqlClient.SqlCommand StoreProc = new System.Data.SqlClient.SqlCommand("UpdateDocumentoTrasporto"))
                    {
                        using (System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            System.Data.SqlClient.SqlParameter id = new System.Data.SqlClient.SqlParameter("@id", documentoTrasporto.Id);
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

                            SqlParameter numeroDocumentoTrasporto = new SqlParameter("@numeroDocumentoTrasporto", documentoTrasporto.NumeroDocumentoTrasporto);
                            numeroDocumentoTrasporto.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(numeroDocumentoTrasporto);

                            SqlParameter dataTrasporto = new SqlParameter("@dataTrasporto", documentoTrasporto.DataTrasporto);
                            dataTrasporto.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(dataTrasporto);

                            SqlParameter causale = new SqlParameter("@causale", documentoTrasporto.Causale);
                            causale.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(causale);

                            SqlParameter destinatario = new SqlParameter("@destinatario", documentoTrasporto.Destinatario);
                            destinatario.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(destinatario);

                            SqlParameter tipoIndirizzo = new SqlParameter("@tipoIndirizzo", documentoTrasporto.TipoIndirizzo);
                            tipoIndirizzo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(tipoIndirizzo);

                            SqlParameter indirizzo = new SqlParameter("@indirizzo", documentoTrasporto.Indirizzo);
                            indirizzo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(indirizzo);

                            SqlParameter numeroCivico = new SqlParameter("@numeroCivico", documentoTrasporto.NumeroCivico);
                            numeroCivico.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(numeroCivico);

                            SqlParameter cap = new SqlParameter("@cap", documentoTrasporto.Cap);
                            cap.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(cap);

                            SqlParameter comune = new SqlParameter("@comune", documentoTrasporto.Comune);
                            comune.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(comune);

                            SqlParameter provincia = new SqlParameter("@provincia", documentoTrasporto.Provincia);
                            provincia.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(provincia);

                            SqlParameter nazione = new SqlParameter("@nazione", documentoTrasporto.Nazione);
                            nazione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nazione);

                            SqlParameter partitaIva = new SqlParameter("@partitaIva", documentoTrasporto.PartitaIva);
                            partitaIva.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(partitaIva);

                            SqlParameter numeroColli = new SqlParameter("@numeroColli", documentoTrasporto.NumeroColli);
                            numeroColli.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(numeroColli);

                            SqlParameter peso = new SqlParameter("@peso", documentoTrasporto.Peso);
                            peso.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(peso);

                            SqlParameter trasportatore = new SqlParameter("@trasportatore", documentoTrasporto.Trasportatore);
                            trasportatore.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(trasportatore);

                            StoreProc.Connection.Open();

                            int iReturn = StoreProc.ExecuteNonQuery();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.Descrizione = "DocumentiTrasporto_DAL.cs - AggiornaDocumentoTrasporto " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

        public Esito EliminaDocumentoTrasporto(int idDocumentoTrasporto)
        {
            Anag_Utenti utente = (Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE];
            Esito esito = new Esito();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("DeleteDocumentoTrasporto"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            SqlParameter id = new SqlParameter("@id", SqlDbType.Int);
                            id.Direction = ParameterDirection.Input;
                            id.Value = idDocumentoTrasporto;
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
                esito.Descrizione = "DocumentiTrasporto_DAL.cs - EliminaDocumentoTrasporto " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

        // ATTREZZATURE TRASPORTO

        public List<AttrezzatureTrasporto> getAttrezzatureTrasportoByIdDocumentoTrasporto(ref Esito esito, Int64 idDocumentoTrasporto)
        {
            List<AttrezzatureTrasporto> listaAttrezzatureTrasporto = new List<AttrezzatureTrasporto>();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM dati_attrezzature_Trasporto WHERE idDocumentoTrasporto = " + idDocumentoTrasporto.ToString();
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
                                        AttrezzatureTrasporto attrezzaturaTrasporto = new AttrezzatureTrasporto
                                        {
                                            Id = riga.Field<int>("id"),
                                            IdMagAttrezzature = riga.Field<int>("idMagAttrezzature"),
                                            IdDocumentoTrasporto = riga.Field<int>("idDocumentoTrasporto"),
                                            Cod_vs = riga.Field<string>("cod_vs"),
                                            Descrizione = riga.Field<string>("descrizione"),
                                            Quantita = riga.Field<int>("quantita"),
                                            Seriale = riga.Field<string>("seriale")
                                        };

                                        listaAttrezzatureTrasporto.Add(attrezzaturaTrasporto);

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

            return listaAttrezzatureTrasporto;

        }

        public Esito AggiornaAttrezzatureTrasporto(AttrezzatureTrasporto attrezzaturaTrasporto)
        {
            Anag_Utenti utente = (Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE];
            Esito esito = new Esito();
            try
            {
                using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(sqlConstr))
                {
                    using (System.Data.SqlClient.SqlCommand StoreProc = new System.Data.SqlClient.SqlCommand("UpdateAttrezzatureTrasporto"))
                    {
                        using (System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            System.Data.SqlClient.SqlParameter id = new System.Data.SqlClient.SqlParameter("@id", attrezzaturaTrasporto.Id);
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

                            SqlParameter IdDocumentoTrasporto = new SqlParameter("@IdDocumentoTrasporto", attrezzaturaTrasporto.IdDocumentoTrasporto);
                            IdDocumentoTrasporto.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(IdDocumentoTrasporto);

                            SqlParameter idMagAttrezzature = new SqlParameter("@idMagAttrezzature", attrezzaturaTrasporto.IdMagAttrezzature);
                            idMagAttrezzature.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idMagAttrezzature);

                            SqlParameter cod_vs = new SqlParameter("@cod_vs", attrezzaturaTrasporto.Cod_vs);
                            cod_vs.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(cod_vs);

                            SqlParameter descrizione = new SqlParameter("@cod_vs", attrezzaturaTrasporto.Descrizione);
                            descrizione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(descrizione);

                            SqlParameter quantita = new SqlParameter("@quantita", attrezzaturaTrasporto.Quantita);
                            quantita.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(quantita);

                            SqlParameter seriale = new SqlParameter("@seriale", attrezzaturaTrasporto.Seriale);
                            seriale.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(seriale);

                            StoreProc.Connection.Open();

                            int iReturn = StoreProc.ExecuteNonQuery();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.Descrizione = "DocumentiTrasporto_DAL.cs - AggiornaAttrezzatureTrasporto " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

        public int CreaAttrezzaturaTrasporto(AttrezzatureTrasporto attrezzaturaTrasporto, ref Esito esito)
        {

            try
            {
                Anag_Utenti utente = (Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE];
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("InsertAttrezzatureTrasporto"))
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

                            SqlParameter IdDocumentoTrasporto = new SqlParameter("@IdDocumentoTrasporto", attrezzaturaTrasporto.IdDocumentoTrasporto);
                            IdDocumentoTrasporto.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(IdDocumentoTrasporto);

                            SqlParameter idMagAttrezzature = new SqlParameter("@idMagAttrezzature", attrezzaturaTrasporto.IdMagAttrezzature);
                            idMagAttrezzature.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idMagAttrezzature);

                            SqlParameter cod_vs = new SqlParameter("@cod_vs", attrezzaturaTrasporto.Cod_vs);
                            cod_vs.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(cod_vs);

                            SqlParameter descrizione = new SqlParameter("@descrizione", attrezzaturaTrasporto.Descrizione);
                            descrizione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(descrizione);

                            SqlParameter quantita = new SqlParameter("@quantita", attrezzaturaTrasporto.Quantita);
                            quantita.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(quantita);

                            SqlParameter seriale = new SqlParameter("@seriale", attrezzaturaTrasporto.Seriale);
                            seriale.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(seriale);

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
                esito.Descrizione = "DocumentiTrasporto_DAL.cs - CreaAttrezzaturaTrasporto " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return 0;
        }

        public Esito EliminaAttrezzaturaTrasporto(int idAttrezzaturaTrasporto)
        {
            Anag_Utenti utente = (Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE];
            Esito esito = new Esito();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("DeleteAttrezzatureTrasporto"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            SqlParameter id = new SqlParameter("@id", SqlDbType.Int);
                            id.Direction = ParameterDirection.Input;
                            id.Value = idAttrezzaturaTrasporto;
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
                esito.Descrizione = "DocumentiTrasporto_DAL.cs - EliminaAttrezzaturaTrasporto " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

    }
}