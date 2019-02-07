using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using VideoSystemWeb.BLL;
using VideoSystemWeb.Entity;
using System.Data.SqlClient;
namespace VideoSystemWeb.DAL
{
    public class Anag_Collaboratori_DAL : Base_DAL
    {
        //singleton
        private static volatile Anag_Collaboratori_DAL instance;
        private static object objForLock = new Object();

        private Anag_Collaboratori_DAL() { }

        public static Anag_Collaboratori_DAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Anag_Collaboratori_DAL();
                    }
                }
                return instance;
            }
        }

        public Anag_Collaboratori getCollaboratoreById(int idCollaboratore, ref Esito esito)
        {
            Anag_Collaboratori collaboratore = new Anag_Collaboratori();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM anag_collaboratori where id = " + idCollaboratore.ToString();
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
                                    collaboratore.Id = dt.Rows[0].Field<int>("id");
                                    collaboratore.Cognome = dt.Rows[0].Field<string>("cognome");
                                    collaboratore.Nome = dt.Rows[0].Field<string>("nome");
                                    collaboratore.CodiceFiscale = dt.Rows[0].Field<string>("codiceFiscale");
                                    collaboratore.PathFoto = dt.Rows[0].Field<string>("pathFoto");
                                    collaboratore.Nazione = dt.Rows[0].Field<string>("nazione");
                                    collaboratore.ComuneNascita = dt.Rows[0].Field<string>("comuneNascita");
                                    collaboratore.ProvinciaNascita = dt.Rows[0].Field<string>("provinciaNascita");
                                    collaboratore.DataNascita = dt.Rows[0]["dataNascita"] != DBNull.Value ? dt.Rows[0].Field<DateTime>("dataNascita") : DateTime.MinValue;
                                    collaboratore.ComuneRiferimento = dt.Rows[0].Field<string>("comuneRiferimento");
                                    collaboratore.PartitaIva = dt.Rows[0].Field<string>("partitaIva");
                                    collaboratore.NomeSocieta = dt.Rows[0].Field<string>("nomeSocieta");
                                    collaboratore.Assunto = dt.Rows[0].Field<bool>("assunto");
                                    collaboratore.Note = dt.Rows[0].Field<string>("note");
                                    collaboratore.Attivo = dt.Rows[0].Field<bool>("attivo");
                                    collaboratore.Qualifiche = Anag_Qualifiche_Collaboratori_DAL.Instance.getQualificheByIdCollaboratore(ref esito, collaboratore.Id);
                                    collaboratore.Indirizzi = Anag_Indirizzi_Collaboratori_DAL.Instance.getIndirizziByIdCollaboratore(ref esito, collaboratore.Id);
                                    collaboratore.Email = Anag_Email_Collaboratori_DAL.Instance.getEmailByIdCollaboratore(ref esito, collaboratore.Id);
                                    collaboratore.Telefoni = Anag_Telefoni_Collaboratori_DAL.Instance.getTelefoniByIdCollaboratore(ref esito, collaboratore.Id);
                                }
                                else
                                {
                                    esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                    esito.descrizione = "Collaboratore con id " + idCollaboratore.ToString() + " non trovato in tabella anag_collaboratori ";
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
            return collaboratore;
        }

        public List<Anag_Collaboratori> CaricaListaCollaboratori(ref Esito esito, bool soloAttivi = true)
        {
            List<Anag_Collaboratori> listaCollaboratori = new List<Anag_Collaboratori>();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM anag_collaboratori";
                    if (soloAttivi) query+= " WHERE ATTIVO = 1";
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

                                        // private int id;
                                        //private string cognome;
                                        //private string nome;
                                        //private string codiceFiscale;
                                        //private string pathFoto;
                                        //private string nazione;
                                        //private string comuneNascita;
                                        //private string provinciaNascita;
                                        //private DateTime dataNascita;
                                        //private string comuneRiferimento;
                                        //private string partitaIva;
                                        //private string nomeSocieta;
                                        //private bool assunto;
                                        //private string note;
                                        //private bool attivo;

                                        Anag_Collaboratori collaboratore = new Anag_Collaboratori();
                                        collaboratore.Id = riga.Field<int>("id");
                                        collaboratore.Cognome = riga.Field<string>("cognome");
                                        collaboratore.Nome = riga.Field<string>("nome");
                                        collaboratore.CodiceFiscale = riga.Field<string>("codiceFiscale");
                                        collaboratore.PathFoto = riga.Field<string>("pathFoto");
                                        collaboratore.Nazione = riga.Field<string>("nazione");
                                        collaboratore.ComuneNascita = riga.Field<string>("comuneNascita");
                                        collaboratore.ProvinciaNascita = riga.Field<string>("provinciaNascita");
                                        collaboratore.DataNascita = riga["dataNascita"] != DBNull.Value ? riga.Field<DateTime>("dataNascita") : DateTime.MinValue;
                                        collaboratore.ComuneRiferimento = riga.Field<string>("comuneRiferimento");
                                        collaboratore.PartitaIva = riga.Field<string>("partitaIva");
                                        collaboratore.NomeSocieta = riga.Field<string>("nomeSocieta");
                                        collaboratore.Assunto = riga.Field<bool>("assunto");
                                        collaboratore.Note = riga.Field<string>("note");
                                        collaboratore.Attivo = riga.Field<bool>("attivo");
                                        collaboratore.Qualifiche = Anag_Qualifiche_Collaboratori_DAL.Instance.getQualificheByIdCollaboratore(ref esito, collaboratore.Id);
                                        collaboratore.Indirizzi = Anag_Indirizzi_Collaboratori_DAL.Instance.getIndirizziByIdCollaboratore(ref esito, collaboratore.Id);
                                        collaboratore.Email = Anag_Email_Collaboratori_DAL.Instance.getEmailByIdCollaboratore(ref esito, collaboratore.Id);
                                        collaboratore.Telefoni = Anag_Telefoni_Collaboratori_DAL.Instance.getTelefoniByIdCollaboratore(ref esito, collaboratore.Id);

                                        listaCollaboratori.Add(collaboratore);
                                    }
                                }
                                else
                                {
                                    esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                    esito.descrizione = "Nessun dato trovato nella tabella anag_collaboratori ";
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

            return listaCollaboratori;
        }

        public int CreaCollaboratore(Anag_Collaboratori collaboratore, ref Esito esito)
        {
            //@cognome varchar(50),
            //@nome varchar(50),
            //@codiceFiscale varchar(16),
            //@pathFoto varchar(100) = null,
            //@nazione varchar(50),
            //@comuneNascita varchar(50),
            //@provinciaNascita varchar(2),
            //@dataNascita datetime,
            //@comuneRiferimento varchar(50),
            //@partitaIva varchar(11) = null,
            //@nomeSocieta varchar(50) = null,
            //@assunto bit,
            //@note varchar(max) = null,
            //@attivo bit

            //Esito esito = new Esito();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("InsertCollaboratore"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            SqlParameter cognome = new SqlParameter("@cognome", collaboratore.Cognome);
                            cognome.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(cognome);

                            SqlParameter nome = new SqlParameter("@nome", collaboratore.Nome);
                            nome.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome);

                            SqlParameter codiceFiscale = new SqlParameter("@codiceFiscale", collaboratore.CodiceFiscale);
                            codiceFiscale.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(codiceFiscale);

                            SqlParameter pathFoto = new SqlParameter("@pathFoto", collaboratore.PathFoto);
                            pathFoto.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(pathFoto);

                            SqlParameter nazione = new SqlParameter("@nazione", collaboratore.Nazione);
                            nazione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nazione);

                            SqlParameter comuneNascita = new SqlParameter("@comuneNascita", collaboratore.ComuneNascita);
                            comuneNascita.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(comuneNascita);

                            SqlParameter provinciaNascita = new SqlParameter("@provinciaNascita", collaboratore.ProvinciaNascita);
                            provinciaNascita.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(provinciaNascita);

                            SqlParameter dataNascita = new SqlParameter("@dataNascita", collaboratore.DataNascita);
                            dataNascita.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(dataNascita);

                            SqlParameter comuneRiferimento = new SqlParameter("@comuneRiferimento", collaboratore.ComuneRiferimento);
                            comuneRiferimento.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(comuneRiferimento);

                            SqlParameter partitaIva = new SqlParameter("@partitaIva", collaboratore.PartitaIva);
                            partitaIva.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(partitaIva);

                            SqlParameter nomeSocieta = new SqlParameter("@nomeSocieta", collaboratore.NomeSocieta);
                            nomeSocieta.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nomeSocieta);

                            SqlParameter note = new SqlParameter("@note", collaboratore.Note);
                            note.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(note);

                            SqlParameter assunto = new SqlParameter("@assunto", collaboratore.Assunto);
                            assunto.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(assunto);

                            SqlParameter attivo = new SqlParameter("@attivo", collaboratore.Attivo);
                            attivo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(attivo);

                            StoreProc.Connection.Open();

                            //int iReturn = StoreProc.ExecuteNonQuery();
                            int iReturn = (int) StoreProc.ExecuteScalar();
                            return iReturn;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.descrizione = "Anag_Collaboratori_DAL.cs - CreaCollaboratore " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return 0;
        }

        public Esito AggiornaCollaboratore(Anag_Collaboratori collaboratore)
        {
            Esito esito = new Esito();
            try
            {
                using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(sqlConstr))
                {
                    using (System.Data.SqlClient.SqlCommand StoreProc = new System.Data.SqlClient.SqlCommand("UpdateCollaboratore"))
                    {
                        using (System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            System.Data.SqlClient.SqlParameter id = new System.Data.SqlClient.SqlParameter("@id", collaboratore.Id);
                            id.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id);

                            System.Data.SqlClient.SqlParameter cognome = new System.Data.SqlClient.SqlParameter("@cognome", collaboratore.Cognome);
                            cognome.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(cognome);

                            System.Data.SqlClient.SqlParameter nome = new System.Data.SqlClient.SqlParameter("@nome", collaboratore.Nome);
                            nome.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome);

                            System.Data.SqlClient.SqlParameter codiceFiscale = new System.Data.SqlClient.SqlParameter("@codiceFiscale", collaboratore.CodiceFiscale);
                            codiceFiscale.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(codiceFiscale);

                            System.Data.SqlClient.SqlParameter pathFoto = new System.Data.SqlClient.SqlParameter("@pathFoto", collaboratore.PathFoto);
                            pathFoto.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(pathFoto);

                            System.Data.SqlClient.SqlParameter nazione = new System.Data.SqlClient.SqlParameter("@nazione", collaboratore.Nazione);
                            nazione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nazione);

                            System.Data.SqlClient.SqlParameter comuneNascita = new System.Data.SqlClient.SqlParameter("@comuneNascita", collaboratore.ComuneNascita);
                            comuneNascita.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(comuneNascita);

                            System.Data.SqlClient.SqlParameter provinciaNascita = new System.Data.SqlClient.SqlParameter("@provinciaNascita", collaboratore.ProvinciaNascita);
                            provinciaNascita.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(provinciaNascita);

                            System.Data.SqlClient.SqlParameter dataNascita = new System.Data.SqlClient.SqlParameter("@dataNascita", collaboratore.DataNascita);
                            dataNascita.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(dataNascita);

                            System.Data.SqlClient.SqlParameter comuneRiferimento = new System.Data.SqlClient.SqlParameter("@comuneRiferimento", collaboratore.ComuneRiferimento);
                            comuneRiferimento.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(comuneRiferimento);

                            System.Data.SqlClient.SqlParameter partitaIva = new System.Data.SqlClient.SqlParameter("@partitaIva", collaboratore.PartitaIva);
                            partitaIva.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(partitaIva);

                            System.Data.SqlClient.SqlParameter nomeSocieta = new System.Data.SqlClient.SqlParameter("@nomeSocieta", collaboratore.NomeSocieta);
                            nomeSocieta.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nomeSocieta);

                            System.Data.SqlClient.SqlParameter note = new System.Data.SqlClient.SqlParameter("@note", collaboratore.Note);
                            note.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(note);

                            System.Data.SqlClient.SqlParameter assunto = new System.Data.SqlClient.SqlParameter("@assunto", collaboratore.Assunto);
                            assunto.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(assunto);

                            System.Data.SqlClient.SqlParameter attivo = new System.Data.SqlClient.SqlParameter("@attivo", collaboratore.Attivo);
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
                esito.descrizione = "Anag_Collaboratori_DAL.cs - aggiornaCollaboratore " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

        public Esito EliminaCollaboratore(int idCollaboratore)
        {
            Esito esito = new Esito();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("DeleteCollaboratore"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            SqlParameter id = new SqlParameter("@id", SqlDbType.Int);
                            id.Direction = ParameterDirection.Input;
                            id.Value = idCollaboratore;
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
                esito.descrizione = "Anag_Collaboratori_DAL.cs - EliminaCollaboratore " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

    }
}