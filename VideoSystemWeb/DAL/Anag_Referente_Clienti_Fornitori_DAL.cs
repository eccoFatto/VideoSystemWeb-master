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
    public class Anag_Referente_Clienti_Fornitori_DAL : Base_DAL
    {
        //singleton
        private static volatile Anag_Referente_Clienti_Fornitori_DAL instance;
        private static object objForLock = new Object();

        private Anag_Referente_Clienti_Fornitori_DAL() { }

        public static Anag_Referente_Clienti_Fornitori_DAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Anag_Referente_Clienti_Fornitori_DAL();
                    }
                }
                return instance;
            }
        }

        public List<Anag_Referente_Clienti_Fornitori> getReferentiByIdAzienda(ref Esito esito, int idAzienda)
        {
            List<Anag_Referente_Clienti_Fornitori> listaReferenti = new List<Anag_Referente_Clienti_Fornitori>();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM anag_referente_clienti_fornitori WHERE id_Azienda = " + idAzienda.ToString();
                    query += " ORDER BY settore,cognome,nome";
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

                                        Anag_Referente_Clienti_Fornitori referente = new Anag_Referente_Clienti_Fornitori();
                                        referente.Id = riga.Field<int>("id");
                                        referente.Id_azienda = riga.Field<int>("id_azienda");
                                        referente.Nome = riga.Field<string>("nome");
                                        referente.Cognome = riga.Field<string>("cognome");
                                        referente.Email = riga.Field<string>("email");
                                        referente.Cellulare = riga.Field<string>("cellulare");
                                        referente.Settore = riga.Field<string>("settore");
                                        referente.Telefono1 = riga.Field<string>("telefono1");
                                        referente.Telefono2 = riga.Field<string>("telefono2");
                                        referente.Note = riga.Field<string>("note");
                                        referente.Attivo = riga.Field<bool>("attivo");
                                        listaReferenti.Add(referente);
                                    }
                                }
                                //else
                                //{
                                //    esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                //    esito.descrizione = "Nessun dato trovato nella tabella anag_referente_clienti_fornitori ";
                                //}
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

            return listaReferenti;
        }

        public int CreaReferente(Anag_Referente_Clienti_Fornitori referente, Anag_Utenti utente, ref Esito esito)
        {
            //@id_azienda int,
            //@cognome varchar(50),
            //@nome varchar(50),
            //@settore varchar(30),
            //@telefono1 varchar(25),
            //@telefono2 varchar(25),
            //@cellulare varchar(25),
            //@email varchar(60),
            //@note varchar(200),
            //@id int output,
            //@attivo bit
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("InsertReferenteClientiFornitori"))
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

                            SqlParameter id_azienda = new SqlParameter("@id_azienda", referente.Id_azienda);
                            id_azienda.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_azienda);

                            SqlParameter cognome = new SqlParameter("@cognome", referente.Cognome);
                            cognome.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(cognome);

                            SqlParameter nome = new SqlParameter("@nome", referente.Nome);
                            nome.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome);

                            SqlParameter settore = new SqlParameter("@settore", referente.Settore);
                            settore.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(settore);

                            SqlParameter telefono1 = new SqlParameter("@telefono1", referente.Telefono1);
                            telefono1.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(telefono1);

                            SqlParameter telefono2 = new SqlParameter("@telefono2", referente.Telefono2);
                            telefono2.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(telefono2);

                            SqlParameter cellulare = new SqlParameter("@cellulare", referente.Cellulare);
                            cellulare.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(cellulare);

                            SqlParameter email = new SqlParameter("@email", referente.Email);
                            email.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(email); 

                            SqlParameter note = new SqlParameter("@note", referente.Note);
                            note.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(note); 

                            SqlParameter attivo = new SqlParameter("@attivo", referente.Attivo);
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
                esito.codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.descrizione = "Anag_Referenti_Clienti_Fornitori_DAL.cs - CreaReferente " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return 0;
        }

        public Esito AggiornaReferente(Anag_Referente_Clienti_Fornitori referente, Anag_Utenti utente)
        {
            Esito esito = new Esito();
            try
            {
                using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(sqlConstr))
                {
                    using (System.Data.SqlClient.SqlCommand StoreProc = new System.Data.SqlClient.SqlCommand("UpdateReferenteClientiFornitori"))
                    {
                        using (System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            System.Data.SqlClient.SqlParameter id = new System.Data.SqlClient.SqlParameter("@id", referente.Id);
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

                            SqlParameter id_azienda = new SqlParameter("@id_azienda", referente.Id_azienda);
                            id_azienda.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_azienda);

                            SqlParameter cognome = new SqlParameter("@cognome", referente.Cognome);
                            cognome.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(cognome);

                            SqlParameter nome = new SqlParameter("@nome", referente.Nome);
                            nome.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome);

                            SqlParameter settore = new SqlParameter("@settore", referente.Settore);
                            settore.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(settore);

                            SqlParameter telefono1 = new SqlParameter("@telefono1", referente.Telefono1);
                            telefono1.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(telefono1);

                            SqlParameter telefono2 = new SqlParameter("@telefono2", referente.Telefono2);
                            telefono2.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(telefono2);

                            SqlParameter cellulare = new SqlParameter("@cellulare", referente.Cellulare);
                            cellulare.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(cellulare);

                            SqlParameter email = new SqlParameter("@email", referente.Email);
                            email.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(email); 

                            SqlParameter note = new SqlParameter("@note", referente.Note);
                            note.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(note); 

                            SqlParameter attivo = new SqlParameter("@attivo", referente.Attivo);
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
                esito.codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.descrizione = "Anag_Referente_Clienti_Fornitori_DAL.cs - AggiornaReferente " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

        public Esito EliminaReferente(int idReferente, Anag_Utenti utente)
        {
            Esito esito = new Esito();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("DeleteReferenteClientiFornitori"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            SqlParameter id = new SqlParameter("@id", SqlDbType.Int);
                            id.Direction = ParameterDirection.Input;
                            id.Value = idReferente;
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
                esito.descrizione = "Anag_Referente_Clienti_Fornitori_DAL.cs - EliminaReferente " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

        public Anag_Referente_Clienti_Fornitori getReferenteById(ref Esito esito, int id)
        {
            Anag_Referente_Clienti_Fornitori referente = new Anag_Referente_Clienti_Fornitori();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM anag_referente_clienti_fornitori WHERE id = " + id.ToString();
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

                                    referente.Id = dt.Rows[0].Field<int>("id");
                                    referente.Id_azienda = dt.Rows[0].Field<int>("id_azienda");
                                    referente.Nome = dt.Rows[0].Field<string>("nome");
                                    referente.Cognome = dt.Rows[0].Field<string>("cognome");
                                    referente.Email = dt.Rows[0].Field<string>("email");
                                    referente.Cellulare = dt.Rows[0].Field<string>("cellulare");
                                    referente.Settore = dt.Rows[0].Field<string>("settore");
                                    referente.Telefono1 = dt.Rows[0].Field<string>("telefono1");
                                    referente.Telefono2 = dt.Rows[0].Field<string>("telefono2");
                                    referente.Note = dt.Rows[0].Field<string>("note");
                                    referente.Attivo = dt.Rows[0].Field<bool>("attivo");

                                }
                                else
                                {
                                    esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                    esito.descrizione = "Nessun dato trovato nella tabella anag_referente_clienti_fornitori ";
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

            return referente;

        }

    }
}