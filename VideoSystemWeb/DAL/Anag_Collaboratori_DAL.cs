using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using VideoSystemWeb.BLL;
using VideoSystemWeb.Entity;
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
                using (System.Data.OleDb.OleDbConnection con = new System.Data.OleDb.OleDbConnection(constr))
                {
                    string query = "SELECT * FROM anag_collaboratori where id = " + idCollaboratore.ToString();
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
                using (System.Data.OleDb.OleDbConnection con = new System.Data.OleDb.OleDbConnection(constr))
                {
                    string query = "SELECT * FROM anag_collaboratori";
                    if (soloAttivi) query+= " WHERE ATTIVO = 1";
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

    }
}