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
    public class Anag_Telefoni_Collaboratori_DAL : Base_DAL
    {
        //singleton
        private static volatile Anag_Telefoni_Collaboratori_DAL instance;
        private static object objForLock = new Object();

        private Anag_Telefoni_Collaboratori_DAL() { }

        public static Anag_Telefoni_Collaboratori_DAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Anag_Telefoni_Collaboratori_DAL();
                    }
                }
                return instance;
            }
        }
        public List<Anag_Telefoni_Collaboratori> getTelefoniByIdCollaboratore(ref Esito esito, int idCollaboratore, bool soloAttivi = true)
        {
            List<Anag_Telefoni_Collaboratori> listaTelefoni = new List<Anag_Telefoni_Collaboratori>();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string query = "SELECT * FROM anag_telefoni_collaboratori WHERE id_Collaboratore = " + idCollaboratore.ToString();
                    if (soloAttivi) query += " AND ATTIVO = 1";
                    query += " ORDER BY priorita,tipo";
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

                                        Anag_Telefoni_Collaboratori telefono = new Anag_Telefoni_Collaboratori();
                                        telefono.Id = riga.Field<int>("id");
                                        telefono.Id_collaboratore = riga.Field<int>("id_collaboratore");

                                        telefono.Numero = riga.Field<string>("numero");
                                        telefono.Pref_int = riga.Field<string>("int_pref");
                                        telefono.Pref_naz = riga.Field<string>("naz_pref");
                                        telefono.Tipo = riga.Field<string>("tipo");
                                        telefono.Whatsapp = riga.Field<bool>("whatsapp");

                                        telefono.Descrizione = riga.Field<string>("descrizione");
                                        telefono.Priorita = riga.Field<int>("priorita");
                                        telefono.Attivo = riga.Field<bool>("attivo");

                                        listaTelefoni.Add(telefono);
                                    }
                                }
                                else
                                {
                                    esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                    esito.descrizione = "Nessun dato trovato nella tabella anag_telefoni_collaboratori ";
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

            return listaTelefoni;
        }

    }
}