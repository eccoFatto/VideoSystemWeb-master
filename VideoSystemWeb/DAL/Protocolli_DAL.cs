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
    public class Protocolli_DAL : Base_DAL
    {
        //singleton
        private static volatile Protocolli_DAL instance;
        private static object objForLock = new Object();

        private Protocolli_DAL() { }

        public static Protocolli_DAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Protocolli_DAL();
                    }
                }
                return instance;
            }
        }
        public List<Protocolli> getProtocolli(ref Esito esito, bool soloAttivi = true)
        {
            List<Protocolli> listaProtocolli = new List<Protocolli>();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM dati_protocollo";
                    if (soloAttivi) query += " WHERE ATTIVO = 1";
                    query += " ORDER BY codice_lavoro,numero_protocollo";
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
                                        Protocolli protocollo = new Protocolli();
                                        protocollo.Id = riga.Field<int>("id");
                                        protocollo.Codice_lavoro = riga.Field<string>("codice_lavoro");
                                        protocollo.Numero_protocollo = riga.Field<string>("numero_protocollo");
                                        if (!DBNull.Value.Equals(riga["data_protocollo"])) protocollo.Data_protocollo = riga.Field<DateTime>("data_protocollo");
                                        protocollo.Cliente = riga.Field<string>("cliente");
                                        if (!DBNull.Value.Equals(riga["id_cliente"])) protocollo.Id_cliente = riga.Field<int>("id_cliente");
                                        protocollo.Id_tipo_protocollo = riga.Field<int>("id_tipo_protocollo");
                                        protocollo.Protocollo_riferimento = riga.Field<string>("protocollo_riferimento");
                                        protocollo.PathDocumento = riga.Field<string>("pathDocumento");
                                        protocollo.Descrizione = riga.Field<string>("descrizione");
                                        protocollo.Lavorazione = riga.Field<string>("lavorazione");
                                        protocollo.Produzione = riga.Field<string>("produzione");
                                        if (!DBNull.Value.Equals(riga["data_inizio_lavorazione"])) protocollo.Data_inizio_lavorazione = riga.Field<DateTime>("data_inizio_lavorazione");
                                        protocollo.Attivo = riga.Field<bool>("attivo");
                                        protocollo.Pregresso= riga.Field<bool>("pregresso");
                                        protocollo.Destinatario = riga.Field<string>("destinatario");
                                        listaProtocolli.Add(protocollo);
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

            return listaProtocolli;
        }

        public List<Protocolli> getProtocolliByCodLavIdTipoProtocollo(string codiceLavorazione, int IdTipoProtocollo, ref Esito esito, bool soloAttivi = true)
        {
            List<Protocolli> listaProtocolli = new List<Protocolli>();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM dati_protocollo WHERE codice_lavoro = '" +codiceLavorazione.Trim() + "' AND id_tipo_protocollo = " + IdTipoProtocollo.ToString();
                    
                    if (soloAttivi) query += " AND ATTIVO = 1";
                    query += " ORDER BY codice_lavoro,numero_protocollo";
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
                                        Protocolli protocollo = new Protocolli();
                                        protocollo.Id = riga.Field<int>("id");
                                        protocollo.Codice_lavoro = riga.Field<string>("codice_lavoro");
                                        protocollo.Numero_protocollo = riga.Field<string>("numero_protocollo");
                                        if (!DBNull.Value.Equals(riga["data_protocollo"]))protocollo.Data_protocollo = riga.Field<DateTime?>("data_protocollo");
                                        protocollo.Cliente = riga.Field<string>("cliente");
                                        if (!DBNull.Value.Equals(riga["id_cliente"])) protocollo.Id_cliente = riga.Field<int>("id_cliente");
                                        protocollo.Id_tipo_protocollo = riga.Field<int>("id_tipo_protocollo");
                                        protocollo.Protocollo_riferimento = riga.Field<string>("protocollo_riferimento");
                                        protocollo.PathDocumento = riga.Field<string>("pathDocumento");
                                        protocollo.Descrizione = riga.Field<string>("descrizione");
                                        protocollo.Lavorazione = riga.Field<string>("lavorazione");
                                        protocollo.Produzione = riga.Field<string>("produzione");
                                        if (!DBNull.Value.Equals(riga["data_inizio_lavorazione"]))protocollo.Data_inizio_lavorazione = riga.Field<DateTime>("data_inizio_lavorazione");
                                        protocollo.Attivo = riga.Field<bool>("attivo");
                                        protocollo.Pregresso = riga.Field<bool>("pregresso");
                                        protocollo.Destinatario = riga.Field<string>("destinatario");

                                        listaProtocolli.Add(protocollo);
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

            return listaProtocolli;
        }

        public Protocolli getProtocolloById(ref Esito esito, Int64 id)
        {
            Protocolli protocollo = new Protocolli();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM dati_protocollo WHERE id = " + id.ToString();
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
                                    protocollo.Id = dt.Rows[0].Field<int>("id");
                                    protocollo.Codice_lavoro = dt.Rows[0].Field<string>("codice_lavoro");
                                    protocollo.Numero_protocollo = dt.Rows[0].Field<string>("numero_protocollo");
                                    //protocollo.Data_protocollo = dt.Rows[0].Field<DateTime>("data_protocollo");
                                    if (!DBNull.Value.Equals(dt.Rows[0]["data_protocollo"]))protocollo.Data_protocollo = dt.Rows[0].Field<DateTime>("data_protocollo");
                                    protocollo.Cliente = dt.Rows[0].Field<string>("cliente");
                                    if (!DBNull.Value.Equals(dt.Rows[0]["id_cliente"])) protocollo.Id_cliente = dt.Rows[0].Field<int>("id_cliente");
                                    protocollo.Id_tipo_protocollo = dt.Rows[0].Field<int>("id_tipo_protocollo");
                                    protocollo.Protocollo_riferimento = dt.Rows[0].Field<string>("protocollo_riferimento");
                                    protocollo.PathDocumento = dt.Rows[0].Field<string>("pathDocumento");
                                    protocollo.Descrizione = dt.Rows[0].Field<string>("descrizione");
                                    protocollo.Lavorazione = dt.Rows[0].Field<string>("lavorazione");
                                    protocollo.Produzione = dt.Rows[0].Field<string>("produzione");
                                    if (!DBNull.Value.Equals(dt.Rows[0]["data_inizio_lavorazione"]))protocollo.Data_inizio_lavorazione = dt.Rows[0].Field<DateTime>("data_inizio_lavorazione");
                                    protocollo.Attivo = dt.Rows[0].Field<bool>("attivo");
                                    protocollo.Pregresso = dt.Rows[0].Field<bool>("pregresso");
                                    protocollo.Destinatario = dt.Rows[0].Field<string>("destinatario");

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

            return protocollo;

        }

        public List<Protocolli> GetProtocolliByIdCliente(ref Esito esito, int idCliente, bool soloAttivi = true)
        {
            List<Protocolli> listaProtocolli = new List<Protocolli>();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT a.* FROM dati_protocollo a " +
                                   "LEFT JOIN anag_clienti_fornitori b on a.cliente = b.ragioneSociale " + //  DA MODIFICARE CON ID
                                   "WHERE b.id = " + idCliente;

                    if (soloAttivi) query += " AND a.attivo = 1";
                    query += " ORDER BY a.codice_lavoro, a.numero_protocollo";

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
                                        Protocolli protocollo = new Protocolli();
                                        protocollo.Id = riga.Field<int>("id");
                                        protocollo.Codice_lavoro = riga.Field<string>("codice_lavoro");
                                        protocollo.Numero_protocollo = riga.Field<string>("numero_protocollo");
                                        if (!DBNull.Value.Equals(riga["data_protocollo"])) protocollo.Data_protocollo = riga.Field<DateTime?>("data_protocollo");
                                        protocollo.Cliente = riga.Field<string>("cliente");
                                        protocollo.Id_tipo_protocollo = riga.Field<int>("id_tipo_protocollo");
                                        protocollo.Protocollo_riferimento = riga.Field<string>("protocollo_riferimento");
                                        protocollo.PathDocumento = riga.Field<string>("pathDocumento");
                                        protocollo.Descrizione = riga.Field<string>("descrizione");
                                        protocollo.Lavorazione = riga.Field<string>("lavorazione");
                                        protocollo.Produzione = riga.Field<string>("produzione");
                                        if (!DBNull.Value.Equals(riga["data_inizio_lavorazione"])) protocollo.Data_inizio_lavorazione = riga.Field<DateTime>("data_inizio_lavorazione");
                                        protocollo.Attivo = riga.Field<bool>("attivo");
                                        protocollo.Pregresso = riga.Field<bool>("pregresso");
                                        protocollo.Destinatario = riga.Field<string>("destinatario");

                                        listaProtocolli.Add(protocollo);
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

            return listaProtocolli;

        }

        public int CreaProtocollo(Protocolli protocollo, ref Esito esito)
        {
            //@codice_lavoro varchar(20),
            //@numero_protocollo varchar(20),
            //@cliente varchar(60),
            //@tipologia varchar(30),
            //@pathDocumento varchar(100),
            //@descrizione varchar(200),
            //@attivo bit,
            //@id int

            try
            {
                Anag_Utenti utente = (Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE];
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("InsertProtocollo"))
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

                            SqlParameter codice_lavoro = new SqlParameter("@codice_lavoro", protocollo.Codice_lavoro);
                            codice_lavoro.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(codice_lavoro);

                            SqlParameter numero_protocollo = new SqlParameter("@numero_protocollo", protocollo.Numero_protocollo);
                            numero_protocollo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(numero_protocollo);

                            SqlParameter cliente = new SqlParameter("@cliente", protocollo.Cliente);
                            cliente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(cliente);

                            SqlParameter id_cliente = new SqlParameter("@id_cliente", DBNull.Value);
                            if (protocollo.Id_cliente>0) id_cliente = new SqlParameter("@id_cliente", protocollo.Id_cliente);
                            id_cliente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_cliente);

                            SqlParameter id_tipo_protocollo = new SqlParameter("@id_tipo_protocollo", protocollo.Id_tipo_protocollo);
                            id_tipo_protocollo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_tipo_protocollo);

                            SqlParameter protocollo_riferimento = new SqlParameter("@protocollo_riferimento", protocollo.Protocollo_riferimento);
                            protocollo_riferimento.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(protocollo_riferimento);

                            SqlParameter pathDocumento = new SqlParameter("@pathDocumento", protocollo.PathDocumento);
                            pathDocumento.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(pathDocumento);

                            SqlParameter descrizione = new SqlParameter("@descrizione", protocollo.Descrizione);
                            descrizione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(descrizione);

                            SqlParameter lavorazione = new SqlParameter("@lavorazione", protocollo.Lavorazione);
                            lavorazione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(lavorazione);

                            SqlParameter produzione = new SqlParameter("@produzione", protocollo.Produzione);
                            produzione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(produzione);

                            SqlParameter data_inizio_lavorazione = new SqlParameter("@data_inizio_lavorazione", protocollo.Data_inizio_lavorazione);
                            data_inizio_lavorazione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(data_inizio_lavorazione);

                            SqlParameter attivo = new SqlParameter("@attivo", protocollo.Attivo);
                            attivo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(attivo);

                            SqlParameter pregresso = new SqlParameter("@pregresso", protocollo.Pregresso);
                            pregresso.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(pregresso);

                            SqlParameter destinatario = new SqlParameter("@destinatario", protocollo.Destinatario);
                            destinatario.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(destinatario);

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
                esito.Descrizione = "Protocolli_DAL.cs - CreaProtocollo " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return 0;
        }

        public Esito AggiornaProtocollo(Protocolli protocollo)
        {
            Anag_Utenti utente = (Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE];
            Esito esito = new Esito();
            try
            {
                using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(sqlConstr))
                {
                    using (System.Data.SqlClient.SqlCommand StoreProc = new System.Data.SqlClient.SqlCommand("UpdateProtocollo"))
                    {
                        using (System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            System.Data.SqlClient.SqlParameter id = new System.Data.SqlClient.SqlParameter("@id", protocollo.Id);
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

                            SqlParameter codice_lavoro = new SqlParameter("@codice_lavoro", protocollo.Codice_lavoro);
                            codice_lavoro.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(codice_lavoro);

                            SqlParameter numero_protocollo = new SqlParameter("@numero_protocollo", protocollo.Numero_protocollo);
                            numero_protocollo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(numero_protocollo);

                            SqlParameter cliente = new SqlParameter("@cliente", protocollo.Cliente);
                            cliente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(cliente);

                            SqlParameter id_cliente = new SqlParameter("@id_cliente", DBNull.Value);
                            if (protocollo.Id_cliente != null) id_cliente = new SqlParameter("@id_cliente", protocollo.Id_cliente);
                            id_cliente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_cliente);

                            SqlParameter id_tipo_protocollo = new SqlParameter("@id_tipo_protocollo", protocollo.Id_tipo_protocollo);
                            id_tipo_protocollo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_tipo_protocollo);

                            SqlParameter protocollo_riferimento = new SqlParameter("@protocollo_riferimento", protocollo.Protocollo_riferimento);
                            protocollo_riferimento.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(protocollo_riferimento);

                            SqlParameter pathDocumento = new SqlParameter("@pathDocumento", protocollo.PathDocumento);
                            pathDocumento.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(pathDocumento);

                            SqlParameter descrizione = new SqlParameter("@descrizione", protocollo.Descrizione);
                            descrizione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(descrizione);

                            SqlParameter lavorazione = new SqlParameter("@lavorazione", protocollo.Lavorazione);
                            lavorazione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(lavorazione);

                            SqlParameter produzione = new SqlParameter("@produzione", protocollo.Produzione);
                            produzione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(produzione);

                            SqlParameter data_inizio_lavorazione = new SqlParameter("@data_inizio_lavorazione", protocollo.Data_inizio_lavorazione);
                            data_inizio_lavorazione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(data_inizio_lavorazione);

                            SqlParameter attivo = new SqlParameter("@attivo", protocollo.Attivo);
                            attivo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(attivo);

                            SqlParameter pregresso = new SqlParameter("@pregresso", protocollo.Pregresso);
                            pregresso.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(pregresso);

                            SqlParameter destinatario = new SqlParameter("@destinatario", protocollo.Destinatario);
                            destinatario.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(destinatario);

                            StoreProc.Connection.Open();

                            int iReturn = StoreProc.ExecuteNonQuery();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.Descrizione = "Protocolli_DAL.cs - AggiornaProtocollo " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

        public Esito EliminaProtocollo(int idProtocollo)
        {
            Anag_Utenti utente = (Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE];
            Esito esito = new Esito();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("DeleteProtocollo"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            SqlParameter id = new SqlParameter("@id", SqlDbType.Int);
                            id.Direction = ParameterDirection.Input;
                            id.Value = idProtocollo;
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
                esito.Descrizione = "Protocolli_DAL.cs - EliminaProtocollo " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

        public Esito RemoveProtocollo(int idProtocollo)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);
            Esito esito = new Esito();
            try
            {
                using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(sqlConstr))
                {
                    using (System.Data.SqlClient.SqlCommand StoreProc = new System.Data.SqlClient.SqlCommand("RemoveDatiProtocollo"))
                    {
                        using (System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            System.Data.SqlClient.SqlParameter id = new System.Data.SqlClient.SqlParameter("@id", idProtocollo);
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
                esito.Codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.Descrizione = "Protocolli_DAL.cs - RemoveProtocollo " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

        

    }
}