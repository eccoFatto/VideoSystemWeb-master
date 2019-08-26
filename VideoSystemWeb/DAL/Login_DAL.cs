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
    public class Login_DAL : Base_DAL
    {
        //singleton
        private static volatile Login_DAL instance;
        private static object objForLock = new Object();

        private Login_DAL() { }

        public static Login_DAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Login_DAL();
                    }
                }
                return instance;
            }
        }

        public void Connetti(string tbUser, string tbPassword, ref Esito esito)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string queryRicercaUtente = "select * from anag_utenti u" +
                    " left join tipo_utente tu" +
                    " on u.id_tipoUtente = tu.id"+
                    " where username = '" + tbUser.Trim() + "' and password = '" + tbPassword.Trim() + "' and u.attivo = 1";
                    using (SqlCommand cmd = new SqlCommand(queryRicercaUtente))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                if (dt != null && dt.Rows != null && dt.Rows.Count == 1)
                                {
                                    Anag_Utenti utente = new Anag_Utenti();

                                    utente.Cognome = dt.Rows[0]["Cognome"].ToString();
                                    utente.Nome = dt.Rows[0]["Nome"].ToString();
                                    utente.username = dt.Rows[0]["Username"].ToString();
                                    utente.id_tipoUtente = Convert.ToInt16(dt.Rows[0]["id1"].ToString());
                                    utente.tipoUtente = dt.Rows[0]["Nome1"].ToString();
                                    utente.id = Convert.ToInt32(dt.Rows[0]["id"].ToString());
                                    utente.password = dt.Rows[0]["password"].ToString();
                                    utente.Descrizione = dt.Rows[0]["descrizione"].ToString();
                                    utente.attivo = Convert.ToBoolean(dt.Rows[0]["Attivo"].ToString());
                                    HttpContext.Current.Session[SessionManager.UTENTE] = utente;
                                }
                                else
                                {
                                    esito.Codice = Esito.ESITO_KO_ERRORE_UTENTE_NON_RICONOSCIUTO;
                                    esito.Descrizione = "Utenza/Password non riconosciute!";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.Descrizione = "Login_DAL.cs - Connetti " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }
        }

        public Esito AggiornaUtente(Anag_Utenti utente)
        {
            Esito esito = new Esito();
            
            try
            {
                using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(sqlConstr))
                {
                    using (System.Data.SqlClient.SqlCommand StoreProc = new System.Data.SqlClient.SqlCommand("UpdateAnagUtente"))
                    {
                        using (System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            System.Data.SqlClient.SqlParameter id = new System.Data.SqlClient.SqlParameter("@id", utente.id);
                            id.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id);

                            SqlParameter id_tipoUtente = new SqlParameter("@id_tipoUtente", utente.id_tipoUtente);
                            id_tipoUtente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(id_tipoUtente);

                            SqlParameter cognome = new SqlParameter("@cognome", utente.Cognome);
                            cognome.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(cognome);

                            SqlParameter nome = new SqlParameter("@nome", utente.Nome);
                            nome.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome);

                            SqlParameter descrizione = new SqlParameter("@descrizione", utente.Descrizione);
                            descrizione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(descrizione);

                            SqlParameter username = new SqlParameter("@username", utente.username);
                            username.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(username);

                            SqlParameter password = new SqlParameter("@password", utente.password);
                            password.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(password);

                            SqlParameter attivo = new SqlParameter("@attivo", utente.attivo);
                            attivo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(attivo);

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
                esito.Descrizione = "Login_DAL.cs - AggiornaUtente " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }
    }
}