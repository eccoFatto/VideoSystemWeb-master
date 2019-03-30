using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using VideoSystemWeb.BLL;
using VideoSystemWeb.Entity;
using System.Data.SqlClient;
namespace VideoSystemWeb.DAL
{
    public class Base_DAL
    {
        // STRINGA DI CONNESSIONE PER CLASSI SQLCLIENT (NON SERVE SPECIFICARE IL PROVIDER)
        public static string sqlConstr = ConfigurationManager.ConnectionStrings["sqlConstrMSSQL"].ConnectionString;
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static int executeUpdateBySql(string querySql, ref Esito esito)
        {
            int iReturn = 0;
            try
            {
                using ( SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand cmd = new SqlCommand(querySql))
                    {
                        cmd.Connection = con;
                        con.Open();
                        iReturn = cmd.ExecuteNonQuery();

                        if (iReturn == 0) { 
                            esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                            esito.descrizione = "Nessun dato affetto dalla query " + querySql;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.descrizione = "Base_DAL.cs - executeUpdateBySql " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return iReturn;

        }
        public static DataTable getDatiBySql(string querySql, ref Esito esito)
        {
            DataTable dtReturn = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand cmd = new SqlCommand(querySql))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            sda.Fill(dtReturn);
                            if (dtReturn == null || dtReturn.Rows == null || dtReturn.Rows.Count == 0)
                            {
                                esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                esito.descrizione = "Nessun dato trovato nella ricerca generica " + querySql;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.descrizione = "Base_DAL.cs - getDatiBySql " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return dtReturn;
        }
        public static List<Tipologica> CaricaTipologica(EnumTipologiche tipologica, bool soloElemAttivi, ref Esito esito)
        {
            List<Tipologica> listaTipologiche = new List<Tipologica>();
            string nomeTipologica = UtilityTipologiche.getNomeTipologica(tipologica);
            string soloAttivi = soloElemAttivi ? " WHERE attivo = 1 " : "";
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM " + nomeTipologica + soloAttivi + " ORDER BY nome"))
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
                                        Tipologica tipologicaCorrente = new Tipologica();
                                        tipologicaCorrente.id = int.Parse(riga["id"].ToString());
                                        tipologicaCorrente.nome = riga["nome"].ToString();
                                        tipologicaCorrente.descrizione = riga["descrizione"].ToString();
                                        tipologicaCorrente.sottotipo = riga["sottotipo"].ToString();
                                        tipologicaCorrente.parametri = riga["parametri"].ToString();
                                        tipologicaCorrente.attivo = bool.Parse(riga["attivo"].ToString());

                                        listaTipologiche.Add(tipologicaCorrente);
                                    }
                                }
                                else
                                {
                                    esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                    esito.descrizione = "Nessun dato trovato nella ricerca della tipologica "+tipologica;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.descrizione = "Base_DAL.cs - CaricaTipologica " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return listaTipologiche;
        }

        public static Tipologica getTipologicaById(EnumTipologiche tipoTipologica,int idTipologica, ref Esito esito)
        {
            Tipologica tipologica = new Tipologica();
            string nomeTipologica = UtilityTipologiche.getNomeTipologica(tipoTipologica);
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM " + nomeTipologica + " where id =" + idTipologica.ToString()))
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
                                    tipologica.id = int.Parse(dt.Rows[0]["id"].ToString());
                                    tipologica.nome = dt.Rows[0]["nome"].ToString();
                                    tipologica.descrizione = dt.Rows[0]["descrizione"].ToString();
                                    tipologica.sottotipo = dt.Rows[0]["sottotipo"].ToString();
                                    tipologica.parametri = dt.Rows[0]["parametri"].ToString();
                                    tipologica.attivo = bool.Parse(dt.Rows[0]["attivo"].ToString());

                                }
                                else
                                {
                                    esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                    esito.descrizione = "Nessun dato trovato nella ricerca della tipologica " + tipoTipologica;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.descrizione = "Base_DAL.cs - getTipologicaById " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return tipologica;
        }

        public static int CreaTipologia(EnumTipologiche tipoTipologica, Tipologica tipologica, ref Esito esito)
        {
            string nomeSP = "";
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

            switch (tipoTipologica)
            {
                case EnumTipologiche.TIPO_GENERE:
                    nomeSP = "InsertTipoGenere";
                    break;
                case EnumTipologiche.TIPO_GRUPPO:
                    nomeSP = "InsertTipoGruppo";
                    break;
                case EnumTipologiche.TIPO_SOTTOGRUPPO:
                    nomeSP = "InsertTipoSottogruppo";
                    break;
                case EnumTipologiche.TIPO_COLONNE_AGENDA:
                    nomeSP = "InsertTipoColonneAgenda";
                    break;
                case EnumTipologiche.TIPO_TENDER:
                    nomeSP = "InsertTipoTender";
                    break;

                default:
                    break;
            }

            if (!string.IsNullOrEmpty(nomeSP)) { 
                try
                {
                    using (SqlConnection con = new SqlConnection(sqlConstr))
                    {
                        using (SqlCommand StoreProc = new SqlCommand(nomeSP))
                        {
                            using (SqlDataAdapter sda = new SqlDataAdapter())
                            {
                                StoreProc.Connection = con;
                                sda.SelectCommand = StoreProc;
                                StoreProc.CommandType = CommandType.StoredProcedure;

                                StoreProc.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

                                // PARAMETRI PER LOG UTENTE
                                System.Data.SqlClient.SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
                                idUtente.Direction = ParameterDirection.Input;
                                StoreProc.Parameters.Add(idUtente);

                                System.Data.SqlClient.SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
                                nomeUtente.Direction = ParameterDirection.Input;
                                StoreProc.Parameters.Add(nomeUtente);
                                // FINE PARAMETRI PER LOG UTENTE

                                SqlParameter nome = new SqlParameter("@nome", tipologica.nome);
                                nome.Direction = ParameterDirection.Input;
                                StoreProc.Parameters.Add(nome);

                                SqlParameter descrizione = new SqlParameter("@descrizione", tipologica.descrizione);
                                descrizione.Direction = ParameterDirection.Input;
                                StoreProc.Parameters.Add(descrizione);

                                SqlParameter sottotipo = new SqlParameter("@sottotipo", tipologica.sottotipo);
                                sottotipo.Direction = ParameterDirection.Input;
                                StoreProc.Parameters.Add(sottotipo);

                                SqlParameter parametri = new SqlParameter("@parametri", tipologica.parametri);
                                parametri.Direction = ParameterDirection.Input;
                                StoreProc.Parameters.Add(parametri);

                                SqlParameter attivo = new SqlParameter("@attivo", tipologica.attivo);
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
                    esito.descrizione = "Base_DAL.cs - CreaTipologia " + Environment.NewLine + ex.Message;

                    log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }
            else
            {
                esito.descrizione = "Tipologia non censita!";
            }

            return 0;
        }

        public static Esito AggiornaTipologia(EnumTipologiche tipoTipologica, Tipologica tipologica)
        {
            string nomeSP = "";
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

            switch (tipoTipologica)
            {
                case EnumTipologiche.TIPO_GENERE:
                    nomeSP = "UpdateTipoGenere";
                    break;
                case EnumTipologiche.TIPO_GRUPPO:
                    nomeSP = "UpdateTipoGruppo";
                    break;
                case EnumTipologiche.TIPO_SOTTOGRUPPO:
                    nomeSP = "UpdateTipoSottogruppo";
                    break;
                case EnumTipologiche.TIPO_COLONNE_AGENDA:
                    nomeSP = "UpdateTipoColonneAgenda";
                    break;
                case EnumTipologiche.TIPO_TENDER:
                    nomeSP = "UpdateTipoTender";
                    break;
                default:
                    break;
            }

            Esito esito = new Esito();
            if (!string.IsNullOrEmpty(nomeSP))
            {

                try
                {
                    using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(sqlConstr))
                    {
                        using (System.Data.SqlClient.SqlCommand StoreProc = new System.Data.SqlClient.SqlCommand(nomeSP))
                        {
                            using (System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter())
                            {
                                StoreProc.Connection = con;
                                sda.SelectCommand = StoreProc;
                                StoreProc.CommandType = CommandType.StoredProcedure;

                                System.Data.SqlClient.SqlParameter id = new System.Data.SqlClient.SqlParameter("@id", tipologica.id);
                                id.Direction = ParameterDirection.Input;
                                StoreProc.Parameters.Add(id);

                                // PARAMETRI PER LOG UTENTE
                                System.Data.SqlClient.SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
                                idUtente.Direction = ParameterDirection.Input;
                                StoreProc.Parameters.Add(idUtente);

                                System.Data.SqlClient.SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
                                nomeUtente.Direction = ParameterDirection.Input;
                                StoreProc.Parameters.Add(nomeUtente);
                                // FINE PARAMETRI PER LOG UTENTE

                                SqlParameter nome = new SqlParameter("@nome", tipologica.nome);
                                nome.Direction = ParameterDirection.Input;
                                StoreProc.Parameters.Add(nome);

                                SqlParameter descrizione = new SqlParameter("@descrizione", tipologica.descrizione);
                                descrizione.Direction = ParameterDirection.Input;
                                StoreProc.Parameters.Add(descrizione);

                                SqlParameter sottotipo = new SqlParameter("@sottotipo", tipologica.sottotipo);
                                sottotipo.Direction = ParameterDirection.Input;
                                StoreProc.Parameters.Add(sottotipo);

                                SqlParameter parametri = new SqlParameter("@parametri", tipologica.parametri);
                                parametri.Direction = ParameterDirection.Input;
                                StoreProc.Parameters.Add(parametri);

                                SqlParameter attivo = new SqlParameter("@attivo", tipologica.attivo);
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
                    esito.descrizione = "Base_DAL.cs - AggiornaTipologia " + Environment.NewLine + ex.Message;

                    log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }
            else
            {
                esito.descrizione = "Tipologia non censita!";
            }
            return esito;
        }

        public static Esito EliminaTipologia(EnumTipologiche tipoTipologica, int idTipologica)
        {
            string nomeSP = "";
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

            switch (tipoTipologica)
            {
                case EnumTipologiche.TIPO_GENERE:
                    nomeSP = "DeleteTipoGenere";
                    break;
                case EnumTipologiche.TIPO_GRUPPO:
                    nomeSP = "DeleteTipoGruppo";
                    break;
                case EnumTipologiche.TIPO_SOTTOGRUPPO:
                    nomeSP = "DeleteTipoSottogruppo";
                    break;
                case EnumTipologiche.TIPO_COLONNE_AGENDA:
                    nomeSP = "DeleteTipoColonneAgenda";
                    break;
                case EnumTipologiche.TIPO_TENDER:
                    nomeSP = "DeleteTipoTender";
                    break;

                default:
                    break;
            }
            Esito esito = new Esito();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand(nomeSP))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            SqlParameter id = new SqlParameter("@id", SqlDbType.Int);
                            id.Direction = ParameterDirection.Input;
                            id.Value = idTipologica;
                            StoreProc.Parameters.Add(id);

                            // PARAMETRI PER LOG UTENTE
                            System.Data.SqlClient.SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
                            idUtente.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(idUtente);

                            System.Data.SqlClient.SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
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
                esito.descrizione = "Base_DAL.cs - EliminaTipologia " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return esito;
        }

        public static int getProtocollo(ref Esito esito)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("getProtocollo"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            StoreProc.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

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
                esito.descrizione = "Base_DAL.cs - getProtocollo " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }
            return 0;
        }

        public static Esito resetProtocollo(int protIniziale)
        {
            Esito esito = new Esito();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("resetProtocollo"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            SqlParameter protocolloIniziale = new SqlParameter("@protocolloIniziale", SqlDbType.Int);
                            protocolloIniziale.Direction = ParameterDirection.Input;
                            protocolloIniziale.Value = protIniziale;
                            StoreProc.Parameters.Add(protocolloIniziale);

                            StoreProc.Connection.Open();

                            int iReturn = StoreProc.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.descrizione = "Base_DAL.cs - resetProtocollo " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return esito;

        }

        public static int getCodiceLavorazione(ref Esito esito)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("getCodiceLavorazione"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            StoreProc.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

                            StoreProc.Connection.Open();

                            StoreProc.ExecuteNonQuery();

                            //int iReturn = (Int32)o;
                            int iReturn = Convert.ToInt32(StoreProc.Parameters["@id"].Value);


                            return iReturn;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.descrizione = "Base_DAL.cs - getCodiceLavorazione " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }
            return 0;
        }

        public static Esito resetCodiceLavorazione(int codLavIniziale)
        {
            Esito esito = new Esito();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand StoreProc = new SqlCommand("resetCodiceLavorazione"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            StoreProc.Connection = con;
                            sda.SelectCommand = StoreProc;
                            StoreProc.CommandType = CommandType.StoredProcedure;

                            SqlParameter codiceLavorazioneIniziale = new SqlParameter("@codiceLavorazioneIniziale", SqlDbType.Int);
                            codiceLavorazioneIniziale.Direction = ParameterDirection.Input;
                            codiceLavorazioneIniziale.Value = codLavIniziale;
                            StoreProc.Parameters.Add(codiceLavorazioneIniziale);

                            StoreProc.Connection.Open();

                            int iReturn = StoreProc.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.descrizione = "Base_DAL.cs - resetCodiceLavorazione " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return esito;
        }
    }
}