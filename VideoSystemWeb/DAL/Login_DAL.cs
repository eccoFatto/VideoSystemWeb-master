using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
                using (System.Data.OleDb.OleDbConnection con = new System.Data.OleDb.OleDbConnection(constr))
                {
                    string queryRicercaUtente = "select * from anag_utenti u" +
                    " left join tipo_utente tu" +
                    " on u.id_tipoUtente = tu.id"+
                    " where username = '" + tbUser.Trim() + "' and password = '" + tbPassword.Trim() + "' and u.attivo = 1";
                    using (System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand(queryRicercaUtente))
                    {
                        using (System.Data.OleDb.OleDbDataAdapter sda = new System.Data.OleDb.OleDbDataAdapter())
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
                                    HttpContext.Current.Session[SessionManager.UTENTE] = utente;
                                }
                                else
                                {
                                    esito.codice = Esito.ESITO_KO_ERRORE_UTENTE_NON_RICONOSCIUTO;
                                    esito.descrizione = "Utenza/Password non riconosciute!";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.descrizione = "Login_DAL.cs - Connetti " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace;
            }
        }
    }
}