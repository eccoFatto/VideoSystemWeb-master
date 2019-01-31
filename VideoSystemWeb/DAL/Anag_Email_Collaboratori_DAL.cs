using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using VideoSystemWeb.BLL;
using VideoSystemWeb.Entity;
namespace VideoSystemWeb.DAL
{
    public class Anag_Email_Collaboratori_DAL : Base_DAL
    {
        //singleton
        private static volatile Anag_Email_Collaboratori_DAL instance;
        private static object objForLock = new Object();

        private Anag_Email_Collaboratori_DAL() { }

        public static Anag_Email_Collaboratori_DAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Anag_Email_Collaboratori_DAL();
                    }
                }
                return instance;
            }
        }
        public List<Anag_Email_Collaboratori> getEmailByIdCollaboratore(ref Esito esito, int idCollaboratore, bool soloAttivi = true)
        {
            List<Anag_Email_Collaboratori> listaEmail = new List<Anag_Email_Collaboratori>();
            try
            {
                using (System.Data.OleDb.OleDbConnection con = new System.Data.OleDb.OleDbConnection(constr))
                {
                    string query = "SELECT * FROM anag_email_collaboratori WHERE id_Collaboratore = " + idCollaboratore.ToString();
                    if (soloAttivi) query += " AND ATTIVO = 1";
                    query += " ORDER BY priorita,indirizzoEmail";
                    using (System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand(query))
                    {
                        using (System.Data.OleDb.OleDbDataAdapter sda = new System.Data.OleDb.OleDbDataAdapter())
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

                                        Anag_Email_Collaboratori email = new Anag_Email_Collaboratori();
                                        email.Id = riga.Field<int>("id");
                                        email.Id_collaboratore = riga.Field<int>("id_collaboratore");

                                        email.IndirizzoEmail = riga.Field<string>("indirizzoEmail");
                                        email.Descrizione = riga.Field<string>("descrizione");
                                        email.Priorita = riga.Field<int>("priorita");
                                        email.Attivo = riga.Field<bool>("attivo");

                                        listaEmail.Add(email);
                                    }
                                }
                                else
                                {
                                    esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                    esito.descrizione = "Nessun dato trovato nella tabella anag_email_collaboratori ";
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

            return listaEmail;
        }

    }
}