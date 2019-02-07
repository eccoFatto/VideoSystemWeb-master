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
    public class Anag_Qualifiche_Collaboratori_DAL : Base_DAL
    {
        //singleton
        private static volatile Anag_Qualifiche_Collaboratori_DAL instance;
        private static object objForLock = new Object();

        private Anag_Qualifiche_Collaboratori_DAL() { }

        public static Anag_Qualifiche_Collaboratori_DAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Anag_Qualifiche_Collaboratori_DAL();
                    }
                }
                return instance;
            }
        }

        public List<Anag_Qualifiche_Collaboratori> getQualificheByIdCollaboratore(ref Esito esito, int idCollaboratore, bool soloAttivi = true)
        {
            List<Anag_Qualifiche_Collaboratori> listaQUalifiche = new List<Anag_Qualifiche_Collaboratori>();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM anag_qualifiche_collaboratori WHERE id_Collaboratore = " + idCollaboratore.ToString();
                    if (soloAttivi) query += " AND ATTIVO = 1";
                    query += " ORDER BY priorita,qualifica";
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

                                        Anag_Qualifiche_Collaboratori qualifica = new Anag_Qualifiche_Collaboratori();
                                        qualifica.Id = riga.Field<int>("id");
                                        qualifica.Descrizione = riga.Field<string>("Descrizione");
                                        qualifica.Id_collaboratore = riga.Field<int>("id_Collaboratore");
                                        qualifica.Qualifica = riga.Field<string>("Qualifica");
                                        qualifica.Priorita = riga.Field<int>("priorita");
                                        qualifica.Attivo = riga.Field<bool>("attivo");

                                        listaQUalifiche.Add(qualifica);
                                    }
                                }
                                else
                                {
                                    esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                    esito.descrizione = "Nessun dato trovato nella tabella anag_qualifiche_collaboratori ";
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

            return listaQUalifiche;
        }

    }
}