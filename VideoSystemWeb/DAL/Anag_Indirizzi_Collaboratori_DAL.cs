using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using VideoSystemWeb.BLL;
using VideoSystemWeb.Entity;
namespace VideoSystemWeb.DAL
{
    public class Anag_Indirizzi_Collaboratori_DAL : Base_DAL
    {
        //singleton
        private static volatile Anag_Indirizzi_Collaboratori_DAL instance;
        private static object objForLock = new Object();

        private Anag_Indirizzi_Collaboratori_DAL() { }

        public static Anag_Indirizzi_Collaboratori_DAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Anag_Indirizzi_Collaboratori_DAL();
                    }
                }
                return instance;
            }
        }

        public List<Anag_Indirizzi_Collaboratori> getIndirizziByIdCollaboratore(ref Esito esito, int idCollaboratore, bool soloAttivi = true)
        {
            List<Anag_Indirizzi_Collaboratori> listaIndirizzi = new List<Anag_Indirizzi_Collaboratori>();
            try
            {
                using (System.Data.OleDb.OleDbConnection con = new System.Data.OleDb.OleDbConnection(constr))
                {
                    string query = "SELECT * FROM anag_indirizzi_collaboratori WHERE id_Collaboratore = " + idCollaboratore.ToString();
                    if (soloAttivi) query += " AND ATTIVO = 1";
                    query += " ORDER BY priorita,indirizzo";
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

                                        Anag_Indirizzi_Collaboratori indirizzo = new Anag_Indirizzi_Collaboratori();
                                        indirizzo.Id = riga.Field<int>("id");
                                        indirizzo.Id_Collaboratore = riga.Field<int>("id_collaboratore");

                                        indirizzo.Tipo = riga.Field<string>("tipo");
                                        indirizzo.Indirizzo = riga.Field<string>("indirizzo");
                                        indirizzo.Nazione = riga.Field<string>("nazione");
                                        indirizzo.NumeroCivico = riga.Field<string>("numeroCivico");
                                        indirizzo.Provincia = riga.Field<string>("provincia");
                                        indirizzo.Cap = riga.Field<string>("cap");
                                        indirizzo.Comune = riga.Field<string>("comune");
                                        indirizzo.Descrizione = riga.Field<string>("descrizione");
                                        indirizzo.Priorita = riga.Field<int>("priorita");
                                        indirizzo.Attivo = riga.Field<bool>("attivo");

                                        listaIndirizzi.Add(indirizzo);
                                    }
                                }
                                else
                                {
                                    esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                    esito.descrizione = "Nessun dato trovato nella tabella anag_indirizzi_collaboratori ";
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

            return listaIndirizzi;
        }

    }
}