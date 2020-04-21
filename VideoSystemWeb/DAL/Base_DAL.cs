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

        public static int ExecuteUpdateBySql(string querySql, ref Esito esito)
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
                            esito.Codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                            esito.Descrizione = "Nessun dato affetto dalla query " + querySql;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.Descrizione = "Base_DAL.cs - executeUpdateBySql " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return iReturn;

        }
        public static DataTable GetDatiBySql(string querySql, ref Esito esito)
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
                            //if (dtReturn == null || dtReturn.Rows == null || dtReturn.Rows.Count == 0)
                            //{
                            //    esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                            //    esito.descrizione = "Nessun dato trovato nella ricerca generica " + querySql;
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.Descrizione = "Base_DAL.cs - getDatiBySql " + Environment.NewLine + ex.Message;

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
                                    esito.Codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                    esito.Descrizione = "Nessun dato trovato nella ricerca della tipologica "+tipologica;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.Descrizione = "Base_DAL.cs - CaricaTipologica " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return listaTipologiche;
        }

        public static List<ColonneAgenda> CaricaColonneAgenda(bool soloElemAttivi, ref Esito esito)
        {
            List<ColonneAgenda> listaColonneAgenda = new List<ColonneAgenda>();
            string soloAttivi = soloElemAttivi ? " WHERE attivo = 1 " : "";
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM tipo_colonne_agenda " + soloAttivi + " ORDER BY sottotipo,ordinamento,nome"))
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
                                        ColonneAgenda tipologicaCorrente = new ColonneAgenda
                                        {
                                            id = int.Parse(riga["id"].ToString()),
                                            nome = riga["nome"].ToString(),
                                            descrizione = riga["descrizione"].ToString(),
                                            sottotipo = riga["sottotipo"].ToString(),
                                            Ordinamento = int.Parse(riga["ordinamento"].ToString()),
                                            parametri = riga["parametri"].ToString(),
                                            attivo = bool.Parse(riga["attivo"].ToString())
                                        };

                                        listaColonneAgenda.Add(tipologicaCorrente);
                                    }
                                }
                                else
                                {
                                    esito.Codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                    esito.Descrizione = "Nessun dato trovato nella ricerca della tipologica tipo_colonne_agenda";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.Descrizione = "Base_DAL.cs - CaricaColonneAgenda " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return listaColonneAgenda;
        }

        public static Tipologica GetTipologicaById(EnumTipologiche tipoTipologica,int idTipologica, ref Esito esito)
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
                                    esito.Codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                    esito.Descrizione = "Nessun dato trovato nella ricerca della tipologica " + tipoTipologica;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.Descrizione = "Base_DAL.cs - getTipologicaById " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return tipologica;
        }

        public static ColonneAgenda GetColonneAgendaById(int idColonnaAgenda, ref Esito esito)
        {
            ColonneAgenda tipologica = new ColonneAgenda();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM tipo_colonne_agenda where id =" + idColonnaAgenda.ToString()))
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
                                    tipologica.Ordinamento = (Int32)dt.Rows[0]["ordinamento"];
                                    tipologica.parametri = dt.Rows[0]["parametri"].ToString();
                                    tipologica.attivo = bool.Parse(dt.Rows[0]["attivo"].ToString());

                                }
                                else
                                {
                                    esito.Codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                    esito.Descrizione = "Nessun dato trovato nella ricerca della tipologica tipo_colonne_agenda";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.Descrizione = "Base_DAL.cs - getColonneAgendaById " + Environment.NewLine + ex.Message;

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
                case EnumTipologiche.TIPO_QUALIFICHE:
                    nomeSP = "InsertTipoQualifiche";
                    break;
                case EnumTipologiche.TIPO_CLIENTI_FORNITORI:
                    nomeSP = "InsertTipoClientiFornitori";
                    break;
                case EnumTipologiche.TIPO_PROTOCOLLO:
                    nomeSP = "InsertTipoProtocollo";
                    break;
                case EnumTipologiche.TIPO_TIPOLOGIE:
                    nomeSP = "InsertTipoTipologie";
                    break;
                case EnumTipologiche.TIPO_INTERVENTO:
                    nomeSP = "InsertTipoIntervento";
                    break;
                case EnumTipologiche.TIPO_CATEGORIE_MAGAZZINO:
                    nomeSP = "InsertTipoCategorieMagazzino";
                    break;
                case EnumTipologiche.TIPO_SUB_CATEGORIE_MAGAZZINO:
                    nomeSP = "InsertTipoSubCategorieMagazzino";
                    break;
                case EnumTipologiche.TIPO_POSIZIONE_MAGAZZINO:
                    nomeSP = "InsertTipoPosizioneMagazzino";
                    break;
                case EnumTipologiche.TIPO_GRUPPO_MAGAZZINO:
                    nomeSP = "InsertTipoGruppoMagazzino";
                    break;
                case EnumTipologiche.TIPO_BANCA:
                    nomeSP = "InsertTipoBanca";
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
                    esito.Codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                    esito.Descrizione = "Base_DAL.cs - CreaTipologia " + Environment.NewLine + ex.Message;

                    log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }
            else
            {
                esito.Descrizione = "Tipologia non censita!";
            }

            return 0;
        }

        public static int CreaColonneAgenda(ColonneAgenda colonnaAgenda, ref Esito esito)
        {
            string nomeSP = "InsertTipoColonneAgenda";
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

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

                            SqlParameter nome = new SqlParameter("@nome", colonnaAgenda.nome);
                            nome.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome);

                            SqlParameter descrizione = new SqlParameter("@descrizione", colonnaAgenda.descrizione);
                            descrizione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(descrizione);

                            SqlParameter sottotipo = new SqlParameter("@sottotipo", colonnaAgenda.sottotipo);
                            sottotipo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(sottotipo);

                            SqlParameter ordinamento = new SqlParameter("@ordinamento", colonnaAgenda.Ordinamento);
                            ordinamento.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(ordinamento);

                            SqlParameter parametri = new SqlParameter("@parametri", colonnaAgenda.parametri);
                            parametri.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(parametri);

                            SqlParameter attivo = new SqlParameter("@attivo", colonnaAgenda.attivo);
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
                esito.Codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.Descrizione = "Base_DAL.cs - CreaColonneAgenda " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
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
                case EnumTipologiche.TIPO_QUALIFICHE:
                    nomeSP = "UpdateTipoQualifiche";
                    break;
                case EnumTipologiche.TIPO_CLIENTI_FORNITORI:
                    nomeSP = "UpdateTipoClientiFornitori";
                    break;
                case EnumTipologiche.TIPO_PROTOCOLLO:
                    nomeSP = "UpdateTipoProtocollo";
                    break;
                case EnumTipologiche.TIPO_TIPOLOGIE:
                    nomeSP = "UpdateTipoTipologie";
                    break;
                case EnumTipologiche.TIPO_INTERVENTO:
                    nomeSP = "UpdateTipoIntervento";
                    break;
                case EnumTipologiche.TIPO_CATEGORIE_MAGAZZINO:
                    nomeSP = "UpdateTipoCategorieMagazzino";
                    break;
                case EnumTipologiche.TIPO_SUB_CATEGORIE_MAGAZZINO:
                    nomeSP = "UpdateTipoSubCategorieMagazzino";
                    break;
                case EnumTipologiche.TIPO_POSIZIONE_MAGAZZINO:
                    nomeSP = "UpdateTipoPosizioneMagazzino";
                    break;
                case EnumTipologiche.TIPO_GRUPPO_MAGAZZINO:
                    nomeSP = "UpdateTipoGruppoMagazzino";
                    break;
                case EnumTipologiche.TIPO_BANCA:
                    nomeSP = "UpdateTipoBanca";
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
                    esito.Codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                    esito.Descrizione = "Base_DAL.cs - AggiornaTipologia " + Environment.NewLine + ex.Message;

                    log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
                }
            }
            else
            {
                esito.Descrizione = "Tipologia non censita!";
            }
            return esito;
        }

        public static Esito AggiornaColonneAgenda(ColonneAgenda colonnaAgenda)
        {
            string nomeSP = "UpdateTipoColonneAgenda";
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

            Esito esito = new Esito();
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

                            System.Data.SqlClient.SqlParameter id = new System.Data.SqlClient.SqlParameter("@id", colonnaAgenda.id);
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

                            SqlParameter nome = new SqlParameter("@nome", colonnaAgenda.nome);
                            nome.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(nome);

                            SqlParameter descrizione = new SqlParameter("@descrizione", colonnaAgenda.descrizione);
                            descrizione.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(descrizione);

                            SqlParameter sottotipo = new SqlParameter("@sottotipo", colonnaAgenda.sottotipo);
                            sottotipo.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(sottotipo);

                            SqlParameter ordinamento = new SqlParameter("@ordinamento", colonnaAgenda.Ordinamento);
                            ordinamento.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(ordinamento);

                            SqlParameter parametri = new SqlParameter("@parametri", colonnaAgenda.parametri);
                            parametri.Direction = ParameterDirection.Input;
                            StoreProc.Parameters.Add(parametri);

                            SqlParameter attivo = new SqlParameter("@attivo", colonnaAgenda.attivo);
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
                esito.Codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.Descrizione = "Base_DAL.cs - AggiornaColonneAgenda " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
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
                case EnumTipologiche.TIPO_QUALIFICHE:
                    nomeSP = "DeleteTipoQualifiche";
                    break;
                case EnumTipologiche.TIPO_CLIENTI_FORNITORI:
                    nomeSP = "DeleteTipoClientiFornitori";
                    break;
                case EnumTipologiche.TIPO_PROTOCOLLO:
                    nomeSP = "DeleteTipoProtocollo";
                    break;
                case EnumTipologiche.TIPO_TIPOLOGIE:
                    nomeSP = "DeleteTipoTipologie";
                    break;
                case EnumTipologiche.TIPO_INTERVENTO:
                    nomeSP = "DeleteTipoIntervento";
                    break;
                case EnumTipologiche.TIPO_CATEGORIE_MAGAZZINO:
                    nomeSP = "DeleteTipoCategorieMagazzino";
                    break;
                case EnumTipologiche.TIPO_SUB_CATEGORIE_MAGAZZINO:
                    nomeSP = "DeleteTipoSubCategorieMagazzino";
                    break;
                case EnumTipologiche.TIPO_POSIZIONE_MAGAZZINO:
                    nomeSP = "DeleteTipoPosizioneMagazzino";
                    break;
                case EnumTipologiche.TIPO_GRUPPO_MAGAZZINO:
                    nomeSP = "DeleteTipoGruppoMagazzino";
                    break;
                case EnumTipologiche.TIPO_BANCA:
                    nomeSP = "DeleteTipoBanca";
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
                esito.Codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.Descrizione = "Base_DAL.cs - EliminaTipologia " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return esito;
        }

        public static Esito RemoveTipologia(EnumTipologiche tipoTipologica, int idTipologica)
        {
            string nomeSP = "";
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

            switch (tipoTipologica)
            {
                case EnumTipologiche.TIPO_GENERE:
                    nomeSP = "RemoveTipoGenere";
                    break;
                case EnumTipologiche.TIPO_GRUPPO:
                    nomeSP = "RemoveTipoGruppo";
                    break;
                case EnumTipologiche.TIPO_SOTTOGRUPPO:
                    nomeSP = "RemoveTipoSottogruppo";
                    break;
                case EnumTipologiche.TIPO_COLONNE_AGENDA:
                    nomeSP = "RemoveTipoColonneAgenda";
                    break;
                case EnumTipologiche.TIPO_TENDER:
                    nomeSP = "RemoveTipoTender";
                    break;
                case EnumTipologiche.TIPO_QUALIFICHE:
                    nomeSP = "RemoveTipoQualifiche";
                    break;
                case EnumTipologiche.TIPO_CLIENTI_FORNITORI:
                    nomeSP = "RemoveTipoClientiFornitori";
                    break;
                case EnumTipologiche.TIPO_PROTOCOLLO:
                    nomeSP = "RemoveTipoProtocollo";
                    break;
                case EnumTipologiche.TIPO_TIPOLOGIE:
                    nomeSP = "RemoveTipoTipologie";
                    break;
                case EnumTipologiche.TIPO_INTERVENTO:
                    nomeSP = "RemoveTipoIntervento";
                    break;
                case EnumTipologiche.TIPO_CATEGORIE_MAGAZZINO:
                    nomeSP = "RemoveTipoCategorieMagazzino";
                    break;
                case EnumTipologiche.TIPO_SUB_CATEGORIE_MAGAZZINO:
                    nomeSP = "RemoveTipoSubCategorieMagazzino";
                    break;
                case EnumTipologiche.TIPO_POSIZIONE_MAGAZZINO:
                    nomeSP = "RemoveTipoPosizioneMagazzino";
                    break;
                case EnumTipologiche.TIPO_GRUPPO_MAGAZZINO:
                    nomeSP = "RemoveTipoGruppoMagazzino";
                    break;
                case EnumTipologiche.TIPO_BANCA:
                    nomeSP = "RemoveTipoBanca";
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
                esito.Codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.Descrizione = "Base_DAL.cs - RemoveTipologia " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return esito;
        }

        public static string GetNumeroFattura(ref Esito esito)
        {
            string ret = "";
            try
            {
                string queryVerificaPresenzaFattureAnno = "select isnull(max(numero_Fattura),'0') as numFatt from tab_numero_fattura where anno_fattura=" + DateTime.Today.Year.ToString();
                DataTable dtProtocolli = Base_DAL.GetDatiBySql(queryVerificaPresenzaFattureAnno, ref esito);
                int newNumFatt = 0;
                if (dtProtocolli.Rows[0]["numFatt"].ToString() == "0")
                {
                    newNumFatt = 1;
                    // INSERISCO NUOVO ANNO E NUMERO_FATTURA = 1 E RESTITUISCO 20200001

                    using (SqlConnection con = new SqlConnection(sqlConstr))
                    {
                        using (SqlCommand StoreProc = new SqlCommand("InsertProgressivoFattura"))
                        {
                            using (SqlDataAdapter sda = new SqlDataAdapter())
                            {
                                StoreProc.Connection = con;
                                sda.SelectCommand = StoreProc;
                                StoreProc.CommandType = CommandType.StoredProcedure;

                                SqlParameter anno_fattura = new SqlParameter("@anno_fattura", DateTime.Today.Year);
                                anno_fattura.Direction = ParameterDirection.Input;
                                StoreProc.Parameters.Add(anno_fattura);

                                SqlParameter numero_fattura = new SqlParameter("@numero_fattura", newNumFatt);
                                numero_fattura.Direction = ParameterDirection.Input;
                                StoreProc.Parameters.Add(numero_fattura);

                                StoreProc.Connection.Open();

                                StoreProc.ExecuteNonQuery();

                            }
                        }
                    }
                }
                else
                {
                    newNumFatt=Convert.ToInt16(dtProtocolli.Rows[0]["numFatt"].ToString()) + 1;
                    // AGGIORNO RIGA ANNO CON NUMERO FATTURA + 1
                    using (SqlConnection con = new SqlConnection(sqlConstr))
                    {
                        using (SqlCommand StoreProc = new SqlCommand("UpdateProgressivoFattura"))
                        {
                            using (SqlDataAdapter sda = new SqlDataAdapter())
                            {
                                StoreProc.Connection = con;
                                sda.SelectCommand = StoreProc;
                                StoreProc.CommandType = CommandType.StoredProcedure;

                                SqlParameter anno = new SqlParameter("@anno", DateTime.Today.Year);
                                anno.Direction = ParameterDirection.Input;
                                StoreProc.Parameters.Add(anno);

                                SqlParameter anno_fattura = new SqlParameter("@anno_fattura", DateTime.Today.Year);
                                anno_fattura.Direction = ParameterDirection.Input;
                                StoreProc.Parameters.Add(anno_fattura);

                                SqlParameter numero_fattura = new SqlParameter("@numero_fattura", newNumFatt);
                                numero_fattura.Direction = ParameterDirection.Input;
                                StoreProc.Parameters.Add(numero_fattura);

                                StoreProc.Connection.Open();

                                StoreProc.ExecuteNonQuery();

                            }
                        }
                    }
                }
                ret = DateTime.Today.Year.ToString() + newNumFatt.ToString("0000");

            }
            catch (Exception ex)
            {

                esito.Descrizione = ex.Message;
            }
            return ret;
        }

        public static int GetProtocollo(ref Esito esito)
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
                esito.Codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.Descrizione = "Base_DAL.cs - getProtocollo " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }
            return 0;
        }

        public static Esito ResetProtocollo(int protIniziale)
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
                esito.Codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.Descrizione = "Base_DAL.cs - resetProtocollo " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return esito;

        }

        public static int GetCodiceLavorazione(ref Esito esito)
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
                esito.Codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.Descrizione = "Base_DAL.cs - getCodiceLavorazione " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }
            return 0;
        }

        public static Esito ResetCodiceLavorazione(int codLavIniziale)
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
                esito.Codice = Esito.ESITO_KO_ERRORE_SCRITTURA_TABELLA;
                esito.Descrizione = "Base_DAL.cs - resetCodiceLavorazione " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return esito;
        }

        protected static void CostruisciSP_InsertDatiArticoli(SqlCommand StoreProc, SqlDataAdapter sda, int iDatiAgendaReturn, DatiArticoli datoArticolo)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

            StoreProc.CommandType = CommandType.StoredProcedure;
            StoreProc.CommandText = "InsertDatiArticoli";
            StoreProc.Parameters.Clear();
            sda.SelectCommand = StoreProc;

            StoreProc.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

            // PARAMETRI PER LOG UTENTE
            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
            idUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idUtente);

            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
            nomeUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(nomeUtente);
            // FINE PARAMETRI PER LOG UTENTE

            SqlParameter costo = new SqlParameter("@costo", datoArticolo.Costo);
            costo.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(costo);

            SqlParameter idArtArticoli = new SqlParameter("@idArtArticoli", datoArticolo.IdArtArticoli);
            idArtArticoli.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idArtArticoli);

            // INSERISCO ID RECUPERATO DALLA SP PRECEDENTE
            SqlParameter idDatiAgenda = new SqlParameter("@idDatiAgenda", iDatiAgendaReturn);
            idDatiAgenda.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idDatiAgenda);

            SqlParameter idTipoGenere = new SqlParameter("@idTipoGenere", datoArticolo.IdTipoGenere);
            idTipoGenere.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idTipoGenere);

            SqlParameter idTipoGruppo = new SqlParameter("@idTipoGruppo", datoArticolo.IdTipoGruppo);
            idTipoGruppo.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idTipoGruppo);

            SqlParameter idTipoSottogruppo = new SqlParameter("@idTipoSottogruppo", datoArticolo.IdTipoSottogruppo);
            idTipoSottogruppo.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idTipoSottogruppo);

            SqlParameter descrizione = new SqlParameter("@descrizione", datoArticolo.Descrizione);
            descrizione.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(descrizione);

            SqlParameter descrizioneLunga = new SqlParameter("@descrizioneLunga", datoArticolo.DescrizioneLunga);
            descrizioneLunga.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(descrizioneLunga);

            SqlParameter iva = new SqlParameter("@iva", datoArticolo.Iva);
            iva.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(iva);

            SqlParameter quantita = new SqlParameter("@quantita", datoArticolo.Quantita);
            quantita.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(quantita);

            SqlParameter prezzo = new SqlParameter("@prezzo", datoArticolo.Prezzo);
            prezzo.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(prezzo);

            SqlParameter stampa = new SqlParameter("@stampa", datoArticolo.Stampa);
            stampa.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(stampa);
        }

        protected static void CostruisciSP_InsertDatiTender(SqlCommand StoreProc, SqlDataAdapter sda, int iDatiAgendaReturn, string idTender)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

            StoreProc.CommandType = CommandType.StoredProcedure;
            StoreProc.CommandText = "InsertDatiTender";
            StoreProc.Parameters.Clear();
            sda.SelectCommand = StoreProc;

            StoreProc.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

            // PARAMETRI PER LOG UTENTE
            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
            idUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idUtente);

            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
            nomeUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(nomeUtente);
            // FINE PARAMETRI PER LOG UTENTE

            SqlParameter idTenderSP = new SqlParameter("@idTender", idTender);
            idTenderSP.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idTenderSP);

            // INSERISCO ID RECUPERATO DALLA SP PRECEDENTE
            SqlParameter idDatiAgenda = new SqlParameter("@idDatiAgenda", iDatiAgendaReturn);
            idDatiAgenda.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idDatiAgenda);
        }

        protected static void CostruisciSP_InsertProtocollo(SqlCommand StoreProc, SqlDataAdapter sda, int iDatiAgendaReturn, Protocolli protocollo)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

            StoreProc.CommandType = CommandType.StoredProcedure;
            StoreProc.CommandText = "[InsertProtocollo]";
            StoreProc.Parameters.Clear();
            sda.SelectCommand = StoreProc;

            StoreProc.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

            // PARAMETRI PER LOG UTENTE
            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
            idUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idUtente);

            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
            nomeUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(nomeUtente);
            // FINE PARAMETRI PER LOG UTENTE

            SqlParameter codice_lavoro = new SqlParameter("@codice_lavoro", protocollo.Codice_lavoro);
            codice_lavoro.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(codice_lavoro);

            SqlParameter numero_protocollo = new SqlParameter("@numero_protocollo", protocollo.Numero_protocollo);
            numero_protocollo.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(numero_protocollo);

            SqlParameter cliente = new SqlParameter("@cliente", protocollo.Cliente);
            cliente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(cliente);

            SqlParameter id_cliente = new SqlParameter("@id_cliente", protocollo.Id_cliente);
            id_cliente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(id_cliente);

            SqlParameter id_tipo_protocollo = new SqlParameter("@id_tipo_protocollo", protocollo.Id_tipo_protocollo);
            id_tipo_protocollo.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(id_tipo_protocollo);

            SqlParameter protocollo_riferimento = new SqlParameter("@protocollo_riferimento", protocollo.Protocollo_riferimento);
            protocollo_riferimento.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(protocollo_riferimento);

            SqlParameter pathDocumento = new SqlParameter("@pathDocumento", protocollo.PathDocumento);
            pathDocumento.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(pathDocumento);

            SqlParameter descrizione = new SqlParameter("@descrizione", protocollo.Descrizione);
            descrizione.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(descrizione);

            SqlParameter produzione = new SqlParameter("@produzione", protocollo.Produzione);
            produzione.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(produzione);

            SqlParameter lavorazione = new SqlParameter("@lavorazione", protocollo.Lavorazione);
            lavorazione.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(lavorazione);

            SqlParameter data_inizio_lavorazione = new SqlParameter("@data_inizio_lavorazione", protocollo.Data_inizio_lavorazione);
            data_inizio_lavorazione.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(data_inizio_lavorazione);

            SqlParameter attivo = new SqlParameter("@attivo", protocollo.Attivo);
            attivo.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(attivo);

            SqlParameter pregresso = new SqlParameter("@pregresso", protocollo.Pregresso);
            pregresso.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(pregresso);

            SqlParameter destinatario = new SqlParameter("@destinatario", protocollo.Destinatario);
            destinatario.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(destinatario);
        }

        protected static void CostruisciSP_InsertEvento(DatiAgenda evento, SqlCommand StoreProc, SqlDataAdapter sda)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

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

            SqlParameter data_inizio_lavorazione = new SqlParameter("@data_inizio_lavorazione", SqlDbType.DateTime);
            data_inizio_lavorazione.Direction = ParameterDirection.Input;
            data_inizio_lavorazione.Value = evento.data_inizio_lavorazione;
            StoreProc.Parameters.Add(data_inizio_lavorazione);

            SqlParameter data_fine_lavorazione = new SqlParameter("@data_fine_lavorazione", SqlDbType.DateTime);
            data_fine_lavorazione.Direction = ParameterDirection.Input;
            data_fine_lavorazione.Value = evento.data_fine_lavorazione;
            StoreProc.Parameters.Add(data_fine_lavorazione);

            SqlParameter durata_lavorazione = new SqlParameter("@durata_lavorazione", SqlDbType.Int);
            durata_lavorazione.Direction = ParameterDirection.Input;
            durata_lavorazione.Value = evento.durata_lavorazione;
            StoreProc.Parameters.Add(durata_lavorazione);

            SqlParameter id_colonne_agenda = new SqlParameter("@id_colonne_agenda", SqlDbType.Int);
            id_colonne_agenda.Direction = ParameterDirection.Input;
            id_colonne_agenda.Value = evento.id_colonne_agenda;
            StoreProc.Parameters.Add(id_colonne_agenda);

            SqlParameter id_tipologia = new SqlParameter("@id_tipologia", SqlDbType.Int);
            id_tipologia.Direction = ParameterDirection.Input;
            id_tipologia.Value = evento.id_tipologia == 0 ? null : evento.id_tipologia;
            StoreProc.Parameters.Add(id_tipologia);

            SqlParameter id_stato = new SqlParameter("@id_stato", SqlDbType.Int);
            id_stato.Direction = ParameterDirection.Input;
            id_stato.Value = evento.id_stato;
            StoreProc.Parameters.Add(id_stato);

            SqlParameter id_cliente = new SqlParameter("@id_cliente", SqlDbType.Int);
            id_cliente.Direction = ParameterDirection.Input;
            id_cliente.Value = evento.id_cliente;
            StoreProc.Parameters.Add(id_cliente);

            SqlParameter durata_viaggio_andata = new SqlParameter("@durata_viaggio_andata", SqlDbType.Int);
            durata_viaggio_andata.Direction = ParameterDirection.Input;
            durata_viaggio_andata.Value = evento.durata_viaggio_andata;
            StoreProc.Parameters.Add(durata_viaggio_andata);

            SqlParameter durata_viaggio_ritorno = new SqlParameter("@durata_viaggio_ritorno", SqlDbType.Int);
            durata_viaggio_ritorno.Direction = ParameterDirection.Input;
            durata_viaggio_ritorno.Value = evento.durata_viaggio_ritorno;
            StoreProc.Parameters.Add(durata_viaggio_ritorno);

            SqlParameter data_inizio_impegno = new SqlParameter("@data_inizio_impegno", SqlDbType.DateTime);
            data_inizio_impegno.Direction = ParameterDirection.Input;
            data_inizio_impegno.Value = evento.data_inizio_impegno;
            StoreProc.Parameters.Add(data_inizio_impegno);

            SqlParameter data_fine_impegno = new SqlParameter("@data_fine_impegno", SqlDbType.DateTime);
            data_fine_impegno.Direction = ParameterDirection.Input;
            data_fine_impegno.Value = evento.data_fine_impegno;
            StoreProc.Parameters.Add(data_fine_impegno);

            SqlParameter impegnoOrario = new SqlParameter("@impegnoOrario", SqlDbType.Bit);
            impegnoOrario.Direction = ParameterDirection.Input;
            impegnoOrario.Value = evento.impegnoOrario;
            StoreProc.Parameters.Add(impegnoOrario);

            SqlParameter impegnoOrario_da = new SqlParameter("@impegnoOrario_da", SqlDbType.VarChar);
            impegnoOrario_da.Direction = ParameterDirection.Input;
            impegnoOrario_da.Value = evento.impegnoOrario_da;
            StoreProc.Parameters.Add(impegnoOrario_da);

            SqlParameter impegnoOrario_a = new SqlParameter("@impegnoOrario_a", SqlDbType.VarChar);
            impegnoOrario_a.Direction = ParameterDirection.Input;
            impegnoOrario_a.Value = evento.impegnoOrario_a;
            StoreProc.Parameters.Add(impegnoOrario_a);

            SqlParameter produzione = new SqlParameter("@produzione", SqlDbType.VarChar);
            produzione.Direction = ParameterDirection.Input;
            produzione.Value = evento.produzione;
            StoreProc.Parameters.Add(produzione);

            SqlParameter lavorazione = new SqlParameter("@lavorazione", SqlDbType.VarChar);
            lavorazione.Direction = ParameterDirection.Input;
            lavorazione.Value = evento.lavorazione;
            StoreProc.Parameters.Add(lavorazione);

            SqlParameter indirizzo = new SqlParameter("@indirizzo", SqlDbType.VarChar);
            indirizzo.Direction = ParameterDirection.Input;
            indirizzo.Value = evento.indirizzo;
            StoreProc.Parameters.Add(indirizzo);

            SqlParameter luogo = new SqlParameter("@luogo", SqlDbType.VarChar);
            luogo.Direction = ParameterDirection.Input;
            luogo.Value = evento.luogo;
            StoreProc.Parameters.Add(luogo);

            SqlParameter codice_lavoro = new SqlParameter("@codice_lavoro", SqlDbType.VarChar);
            codice_lavoro.Direction = ParameterDirection.Input;
            codice_lavoro.Value = evento.codice_lavoro;
            StoreProc.Parameters.Add(codice_lavoro);

            SqlParameter nota = new SqlParameter("@nota", SqlDbType.VarChar);
            nota.Direction = ParameterDirection.Input;
            nota.Value = evento.nota;
            StoreProc.Parameters.Add(nota);
        }

        protected static void CostruisciSP_UpdateEvento(DatiAgenda evento, SqlCommand StoreProc, SqlDataAdapter sda)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

            sda.SelectCommand = StoreProc;
            StoreProc.CommandType = CommandType.StoredProcedure;

            SqlParameter id = new SqlParameter("@id", SqlDbType.Int);
            id.Direction = ParameterDirection.Input;
            id.Value = evento.id;
            StoreProc.Parameters.Add(id);

            // PARAMETRI PER LOG UTENTE
            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
            idUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idUtente);

            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
            nomeUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(nomeUtente);
            // FINE PARAMETRI PER LOG UTENTE

            SqlParameter data_inizio_lavorazione = new SqlParameter("@data_inizio_lavorazione", SqlDbType.DateTime);
            data_inizio_lavorazione.Direction = ParameterDirection.Input;
            data_inizio_lavorazione.Value = evento.data_inizio_lavorazione;
            StoreProc.Parameters.Add(data_inizio_lavorazione);

            SqlParameter data_fine_lavorazione = new SqlParameter("@data_fine_lavorazione", SqlDbType.DateTime);
            data_fine_lavorazione.Direction = ParameterDirection.Input;
            data_fine_lavorazione.Value = evento.data_fine_lavorazione;
            StoreProc.Parameters.Add(data_fine_lavorazione);

            SqlParameter durata_lavorazione = new SqlParameter("@durata_lavorazione", SqlDbType.Int);
            durata_lavorazione.Direction = ParameterDirection.Input;
            durata_lavorazione.Value = evento.durata_lavorazione;
            StoreProc.Parameters.Add(durata_lavorazione);

            SqlParameter id_colonne_agenda = new SqlParameter("@id_colonne_agenda", SqlDbType.Int);
            id_colonne_agenda.Direction = ParameterDirection.Input;
            id_colonne_agenda.Value = evento.id_colonne_agenda;
            StoreProc.Parameters.Add(id_colonne_agenda);

            SqlParameter id_tipologia = new SqlParameter("@id_tipologia", SqlDbType.Int);
            id_tipologia.Direction = ParameterDirection.Input;
            id_tipologia.Value = evento.id_tipologia == 0 ? null : evento.id_tipologia;
            StoreProc.Parameters.Add(id_tipologia);

            SqlParameter id_stato = new SqlParameter("@id_stato", SqlDbType.Int);
            id_stato.Direction = ParameterDirection.Input;
            id_stato.Value = evento.id_stato;
            StoreProc.Parameters.Add(id_stato);

            SqlParameter id_cliente = new SqlParameter("@id_cliente", SqlDbType.Int);
            id_cliente.Direction = ParameterDirection.Input;
            id_cliente.Value = evento.id_cliente;
            StoreProc.Parameters.Add(id_cliente);

            SqlParameter durata_viaggio_andata = new SqlParameter("@durata_viaggio_andata", SqlDbType.Int);
            durata_viaggio_andata.Direction = ParameterDirection.Input;
            durata_viaggio_andata.Value = evento.durata_viaggio_andata;
            StoreProc.Parameters.Add(durata_viaggio_andata);

            SqlParameter durata_viaggio_ritorno = new SqlParameter("@durata_viaggio_ritorno", SqlDbType.Int);
            durata_viaggio_ritorno.Direction = ParameterDirection.Input;
            durata_viaggio_ritorno.Value = evento.durata_viaggio_ritorno;
            StoreProc.Parameters.Add(durata_viaggio_ritorno);

            SqlParameter data_inizio_impegno = new SqlParameter("@data_inizio_impegno", SqlDbType.DateTime);
            data_inizio_impegno.Direction = ParameterDirection.Input;
            data_inizio_impegno.Value = evento.data_inizio_impegno;
            StoreProc.Parameters.Add(data_inizio_impegno);

            SqlParameter data_fine_impegno = new SqlParameter("@data_fine_impegno", SqlDbType.DateTime);
            data_fine_impegno.Direction = ParameterDirection.Input;
            data_fine_impegno.Value = evento.data_fine_impegno;
            StoreProc.Parameters.Add(data_fine_impegno);

            SqlParameter impegnoOrario = new SqlParameter("@impegnoOrario", SqlDbType.Bit);
            impegnoOrario.Direction = ParameterDirection.Input;
            impegnoOrario.Value = evento.impegnoOrario;
            StoreProc.Parameters.Add(impegnoOrario);

            SqlParameter impegnoOrario_da = new SqlParameter("@impegnoOrario_da", SqlDbType.VarChar);
            impegnoOrario_da.Direction = ParameterDirection.Input;
            impegnoOrario_da.Value = evento.impegnoOrario_da;
            StoreProc.Parameters.Add(impegnoOrario_da);

            SqlParameter impegnoOrario_a = new SqlParameter("@impegnoOrario_a", SqlDbType.VarChar);
            impegnoOrario_a.Direction = ParameterDirection.Input;
            impegnoOrario_a.Value = evento.impegnoOrario_a;
            StoreProc.Parameters.Add(impegnoOrario_a);

            SqlParameter produzione = new SqlParameter("@produzione", SqlDbType.VarChar);
            produzione.Direction = ParameterDirection.Input;
            produzione.Value = evento.produzione;
            StoreProc.Parameters.Add(produzione);

            SqlParameter lavorazione = new SqlParameter("@lavorazione", SqlDbType.VarChar);
            lavorazione.Direction = ParameterDirection.Input;
            lavorazione.Value = evento.lavorazione;
            StoreProc.Parameters.Add(lavorazione);

            SqlParameter indirizzo = new SqlParameter("@indirizzo", SqlDbType.VarChar);
            indirizzo.Direction = ParameterDirection.Input;
            indirizzo.Value = evento.indirizzo;
            StoreProc.Parameters.Add(indirizzo);

            SqlParameter luogo = new SqlParameter("@luogo", SqlDbType.VarChar);
            luogo.Direction = ParameterDirection.Input;
            luogo.Value = evento.luogo;
            StoreProc.Parameters.Add(luogo);

            SqlParameter codice_lavoro = new SqlParameter("@codice_lavoro", SqlDbType.VarChar);
            codice_lavoro.Direction = ParameterDirection.Input;
            codice_lavoro.Value = evento.codice_lavoro;
            StoreProc.Parameters.Add(codice_lavoro);

            SqlParameter nota = new SqlParameter("@nota", SqlDbType.VarChar);
            nota.Direction = ParameterDirection.Input;
            nota.Value = evento.nota;
            StoreProc.Parameters.Add(nota);
        }

        protected static void CostruisciSP_DeleteDatiArticoli(SqlCommand StoreProc, SqlDataAdapter sda, int idDatiAgenda)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

            StoreProc.CommandType = CommandType.StoredProcedure;
            StoreProc.CommandText = "DeleteDatiArticoliByIdDatiAgenda";
            StoreProc.Parameters.Clear();
            sda.SelectCommand = StoreProc;

            SqlParameter parIdDatiAgenda = new SqlParameter("@idDatiAgenda", idDatiAgenda);
            parIdDatiAgenda.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(parIdDatiAgenda);

            // PARAMETRI PER LOG UTENTE
            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
            idUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idUtente);

            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
            nomeUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(nomeUtente);
            // FINE PARAMETRI PER LOG UTENTE

        }

        protected static void CostruisciSP_DeleteDatiTender(SqlCommand StoreProc, SqlDataAdapter sda, int idDatiAgenda)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

            StoreProc.CommandType = CommandType.StoredProcedure;
            StoreProc.CommandText = "DeleteDatiTenderByIdDatiAgenda";
            StoreProc.Parameters.Clear();
            sda.SelectCommand = StoreProc;

            SqlParameter parIdDatiAgenda = new SqlParameter("@idDatiAgenda", idDatiAgenda);
            parIdDatiAgenda.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(parIdDatiAgenda);

            // PARAMETRI PER LOG UTENTE
            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
            idUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idUtente);

            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
            nomeUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(nomeUtente);
            // FINE PARAMETRI PER LOG UTENTE

        }

        protected static void CostruisciSP_InsertNoteOfferta(SqlCommand StoreProc, SqlDataAdapter sda, int iDatiAgendaReturn, NoteOfferta noteOfferta)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

            StoreProc.CommandType = CommandType.StoredProcedure;
            StoreProc.CommandText = "InsertNoteOfferta";
            StoreProc.Parameters.Clear();
            sda.SelectCommand = StoreProc;

            StoreProc.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

            // PARAMETRI PER LOG UTENTE
            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
            idUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idUtente);

            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
            nomeUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(nomeUtente);
            // FINE PARAMETRI PER LOG UTENTE


            // INSERISCO ID RECUPERATO DALLA SP PRECEDENTE
            SqlParameter idDatiAgenda = new SqlParameter("@id_dati_agenda", iDatiAgendaReturn);
            idDatiAgenda.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idDatiAgenda);

            SqlParameter parBanca = new SqlParameter("@banca", noteOfferta.Banca);
            parBanca.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(parBanca);

            SqlParameter parPagamento = new SqlParameter("@pagamento", noteOfferta.Pagamento);
            parPagamento.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(parPagamento);

            SqlParameter notaPagamento = new SqlParameter("@notaPagamento", noteOfferta.NotaPagamento);
            notaPagamento.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(notaPagamento);

            SqlParameter parConsegna = new SqlParameter("@consegna", noteOfferta.Consegna);
            parConsegna.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(parConsegna);

            SqlParameter note = new SqlParameter("@note", noteOfferta.Note);
            note.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(note);

        }

        protected static void CostruisciSP_DeleteDatiLavorazione(SqlCommand StoreProc, SqlDataAdapter sda, int idDatiAgenda)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

            StoreProc.CommandType = CommandType.StoredProcedure;
            StoreProc.CommandText = "DeleteDatiLavorazioneByIdDatiAgenda";
            StoreProc.Parameters.Clear();
            sda.SelectCommand = StoreProc;

            SqlParameter parIdDatiAgenda = new SqlParameter("@idDatiAgenda", idDatiAgenda);
            parIdDatiAgenda.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(parIdDatiAgenda);

            // PARAMETRI PER LOG UTENTE
            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
            idUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idUtente);

            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
            nomeUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(nomeUtente);
            // FINE PARAMETRI PER LOG UTENTE

        }

        protected static void CostruisciSP_InsertDatiLavorazione(SqlCommand StoreProc, SqlDataAdapter sda, int iDatiAgendaReturn, DatiLavorazione datoLavorazione)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

            StoreProc.CommandType = CommandType.StoredProcedure;
            StoreProc.CommandText = "InsertDatiLavorazione";
            StoreProc.Parameters.Clear();
            sda.SelectCommand = StoreProc;

            StoreProc.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

            // PARAMETRI PER LOG UTENTE
            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
            idUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idUtente);

            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
            nomeUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(nomeUtente);
            // FINE PARAMETRI PER LOG UTENTE

            // INSERISCO ID RECUPERATO DALLA SP PRECEDENTE
            SqlParameter idDatiAgenda = new SqlParameter("@idDatiAgenda", iDatiAgendaReturn);
            idDatiAgenda.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idDatiAgenda);

            SqlParameter idContratto = new SqlParameter("@idContratto", DBNull.Value);
            if (datoLavorazione.IdContratto != null)
            {
                idContratto = new SqlParameter("@idContratto", datoLavorazione.IdContratto);
            }
            idContratto.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idContratto);

            SqlParameter idReferente = new SqlParameter("@idReferente", DBNull.Value);
            if (datoLavorazione.IdReferente != null)
            {
                idReferente = new SqlParameter("@idReferente", datoLavorazione.IdReferente);
            }
            idReferente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idReferente);

            SqlParameter idCapoTecnico = new SqlParameter("@idCapoTecnico", DBNull.Value);
            if (datoLavorazione.IdCapoTecnico != null)
            {
                idCapoTecnico = new SqlParameter("@idCapoTecnico", datoLavorazione.IdCapoTecnico);
            }
            idCapoTecnico.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idCapoTecnico);


            SqlParameter idProduttore = new SqlParameter("@idProduttore", DBNull.Value);
            if (datoLavorazione.IdProduttore != null)
            {
                idProduttore = new SqlParameter("@idProduttore", datoLavorazione.IdProduttore);
            }
            idProduttore.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idProduttore);

            SqlParameter ordine = new SqlParameter("@ordine", datoLavorazione.Ordine);
            ordine.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(ordine);

            SqlParameter fattura = new SqlParameter("@fattura", datoLavorazione.Fattura);
            fattura.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(fattura);

            SqlParameter notePianoEsterno = new SqlParameter("@notePianoEsterno", datoLavorazione.NotePianoEsterno);
            notePianoEsterno.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(notePianoEsterno);
        }

        protected static void CostruisciSP_InsertDatiArticoliLavorazione(SqlCommand StoreProc, SqlDataAdapter sda, int idDatiLavorazioneReturn, DatiArticoliLavorazione datoArticoloLavorazione)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

            StoreProc.CommandType = CommandType.StoredProcedure;
            StoreProc.CommandText = "InsertDatiArticoliLavorazione";
            StoreProc.Parameters.Clear();
            sda.SelectCommand = StoreProc;

            StoreProc.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

            // PARAMETRI PER LOG UTENTE
            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
            idUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idUtente);

            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
            nomeUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(nomeUtente);
            // FINE PARAMETRI PER LOG UTENTE


            // INSERISCO ID RECUPERATO DALLA SP PRECEDENTE
            SqlParameter idDatiLavorazione = new SqlParameter("@idDatiLavorazione", idDatiLavorazioneReturn);
            idDatiLavorazione.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idDatiLavorazione);

            SqlParameter idArtArticoli = new SqlParameter("@idArtArticoli", datoArticoloLavorazione.IdArtArticoli);
            idArtArticoli.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idArtArticoli);

            SqlParameter idTipoGenere = new SqlParameter("@idTipoGenere", datoArticoloLavorazione.IdTipoGenere);
            idTipoGenere.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idTipoGenere);

            SqlParameter idTipoGruppo = new SqlParameter("@idTipoGruppo", datoArticoloLavorazione.IdTipoGruppo);
            idTipoGruppo.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idTipoGruppo);

            SqlParameter idTipoSottogruppo = new SqlParameter("@idTipoSottogruppo", datoArticoloLavorazione.IdTipoSottogruppo);
            idTipoSottogruppo.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idTipoSottogruppo);

            SqlParameter idCollaboratori = new SqlParameter("@idCollaboratori", DBNull.Value);
            if (datoArticoloLavorazione.IdCollaboratori != null)
            {
                idCollaboratori = new SqlParameter("@idCollaboratori", datoArticoloLavorazione.IdCollaboratori);
            }
            idCollaboratori.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idCollaboratori);

            SqlParameter idFornitori = new SqlParameter("@idFornitori", DBNull.Value);
            if (datoArticoloLavorazione.IdFornitori != null)
            {
                idFornitori = new SqlParameter("@idFornitori", datoArticoloLavorazione.IdFornitori);
            }
            idFornitori.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idFornitori);

            SqlParameter idTipoPagamento = new SqlParameter("@idTipoPagamento", DBNull.Value);
            if (datoArticoloLavorazione.IdTipoPagamento != null)
            {
                idTipoPagamento = new SqlParameter("@idTipoPagamento", datoArticoloLavorazione.IdTipoPagamento);
            }
            idTipoPagamento.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idTipoPagamento);

            SqlParameter descrizione = new SqlParameter("@descrizione", datoArticoloLavorazione.Descrizione);
            descrizione.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(descrizione);

            SqlParameter descrizioneLunga = new SqlParameter("@descrizioneLunga", datoArticoloLavorazione.DescrizioneLunga);
            descrizioneLunga.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(descrizioneLunga);

            SqlParameter stampa = new SqlParameter("@stampa", datoArticoloLavorazione.Stampa);
            stampa.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(stampa);

            SqlParameter prezzo = new SqlParameter("@prezzo", datoArticoloLavorazione.Prezzo);
            prezzo.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(prezzo);

            SqlParameter costo = new SqlParameter("@costo", datoArticoloLavorazione.Costo);
            costo.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(costo);

            SqlParameter iva = new SqlParameter("@iva", datoArticoloLavorazione.Iva);
            iva.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(iva);

            SqlParameter data = new SqlParameter("@data", DBNull.Value);
            if (datoArticoloLavorazione.Data != null)
            {
                data = new SqlParameter("@data", datoArticoloLavorazione.Data);
            }
            data.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(data);

            SqlParameter tv = new SqlParameter("@tv", DBNull.Value);
            if (datoArticoloLavorazione.Tv != null)
            {
                tv = new SqlParameter("@tv", datoArticoloLavorazione.Tv);
            }
            tv.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(tv);

            SqlParameter nota = new SqlParameter("@nota", DBNull.Value);
            if (datoArticoloLavorazione.Nota != null)
            {
                nota = new SqlParameter("@nota", datoArticoloLavorazione.Nota);
            }
            nota.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(nota);

            SqlParameter fp_netto = new SqlParameter("@fp_netto", DBNull.Value);
            if (datoArticoloLavorazione.FP_netto != null)
            {
                fp_netto = new SqlParameter("@fp_netto", datoArticoloLavorazione.FP_netto);
            }
            fp_netto.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(fp_netto);

            SqlParameter fp_lordo = new SqlParameter("@fp_lordo", DBNull.Value);
            if (datoArticoloLavorazione.FP_lordo != null)
            {
                fp_lordo = new SqlParameter("@fp_lordo", datoArticoloLavorazione.FP_lordo);
            }
            fp_lordo.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(fp_lordo);

            SqlParameter usaCostoFP = new SqlParameter("@usaCostoFP", DBNull.Value);
            if (datoArticoloLavorazione.UsaCostoFP != null)
            {
                usaCostoFP = new SqlParameter("@usaCostoFP", datoArticoloLavorazione.UsaCostoFP);
            }
            fp_lordo.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(usaCostoFP);

            SqlParameter consuntivo = new SqlParameter("@consuntivo", DBNull.Value);
            if (datoArticoloLavorazione.Consuntivo != null)
            {
                consuntivo = new SqlParameter("@consuntivo", datoArticoloLavorazione.Consuntivo);
            }
            fp_lordo.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(consuntivo);

            SqlParameter numOccorrenza = new SqlParameter("@numOccorrenza", datoArticoloLavorazione.NumOccorrenza);
            fp_lordo.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(numOccorrenza);
        }

        protected static void CostruisciSP_DeleteDatiArticoliLavorazione(SqlCommand StoreProc, SqlDataAdapter sda, int idDatiLavorazione)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

            StoreProc.CommandType = CommandType.StoredProcedure;
            StoreProc.CommandText = "DeleteDatiArticoliLavorazioneByIdDatiLavorazione";
            StoreProc.Parameters.Clear();
            sda.SelectCommand = StoreProc;

            SqlParameter parIdDatiLavorazione = new SqlParameter("@idDatiLavorazione", idDatiLavorazione);
            parIdDatiLavorazione.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(parIdDatiLavorazione);

            // PARAMETRI PER LOG UTENTE
            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
            idUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idUtente);

            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
            nomeUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(nomeUtente);
            // FINE PARAMETRI PER LOG UTENTE

        }

        protected static void CostruisciSP_DeleteDatiPianoEsternoLavorazione(SqlCommand StoreProc, SqlDataAdapter sda, int idDatiLavorazione)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

            StoreProc.CommandType = CommandType.StoredProcedure;
            StoreProc.CommandText = "DeleteDatiPianoEsternoLavorazioneByIdDatiLavorazione";
            StoreProc.Parameters.Clear();
            sda.SelectCommand = StoreProc;

            SqlParameter parIdDatiLavorazione = new SqlParameter("@idDatiLavorazione", idDatiLavorazione);
            parIdDatiLavorazione.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(parIdDatiLavorazione);

            // PARAMETRI PER LOG UTENTE
            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
            idUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idUtente);

            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
            nomeUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(nomeUtente);
            // FINE PARAMETRI PER LOG UTENTE

        }

        protected static void CostruisciSP_InsertDatiPianoEsternoLavorazione(SqlCommand StoreProc, SqlDataAdapter sda, int idDatiLavorazione, DatiPianoEsternoLavorazione datoPianoEsternoLavorazione)
        {
            Anag_Utenti utente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]);

            StoreProc.CommandType = CommandType.StoredProcedure;
            StoreProc.CommandText = "InsertDatiPianoEsternoLavorazione";
            StoreProc.Parameters.Clear();
            sda.SelectCommand = StoreProc;

            StoreProc.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

            // PARAMETRI PER LOG UTENTE
            SqlParameter idUtente = new SqlParameter("@idUtente", utente.id);
            idUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idUtente);

            SqlParameter nomeUtente = new SqlParameter("@nomeUtente", utente.username);
            nomeUtente.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(nomeUtente);
            // FINE PARAMETRI PER LOG UTENTE


            // INSERISCO ID RECUPERATO DALLA SP PRECEDENTE
            SqlParameter par_idDatiLavorazione = new SqlParameter("@idDatiLavorazione", idDatiLavorazione);
            par_idDatiLavorazione.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(par_idDatiLavorazione);

            SqlParameter idCollaboratori = new SqlParameter("@idCollaboratori", DBNull.Value);
            if (datoPianoEsternoLavorazione.IdCollaboratori != null)
            {
                idCollaboratori = new SqlParameter("@idCollaboratori", datoPianoEsternoLavorazione.IdCollaboratori);
            }
            idCollaboratori.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idCollaboratori);

            SqlParameter idFornitori = new SqlParameter("@idFornitori", DBNull.Value);
            if (datoPianoEsternoLavorazione.IdFornitori != null)
            {
                idFornitori = new SqlParameter("@idFornitori", datoPianoEsternoLavorazione.IdFornitori);
            }
            idFornitori.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idFornitori);

            SqlParameter idIntervento = new SqlParameter("@idIntervento", DBNull.Value);
            if (datoPianoEsternoLavorazione.IdIntervento != null)
            {
                idIntervento = new SqlParameter("@idIntervento", datoPianoEsternoLavorazione.IdIntervento);
            }
            idIntervento.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(idIntervento);

            SqlParameter diaria = new SqlParameter("@diaria", DBNull.Value);
            if (datoPianoEsternoLavorazione.Diaria != null)
            {
                diaria = new SqlParameter("@diaria", datoPianoEsternoLavorazione.Diaria);
            }
            diaria.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(diaria);

            SqlParameter importoDiaria = new SqlParameter("@importoDiaria", DBNull.Value);
            if (datoPianoEsternoLavorazione.ImportoDiaria != null)
            {
                importoDiaria = new SqlParameter("@importoDiaria", datoPianoEsternoLavorazione.ImportoDiaria);
            }
            importoDiaria.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(importoDiaria);

            SqlParameter albergo = new SqlParameter("@albergo", DBNull.Value);
            if (datoPianoEsternoLavorazione.Albergo != null)
            {
                albergo = new SqlParameter("@albergo", datoPianoEsternoLavorazione.Albergo);
            }
            albergo.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(albergo);

            SqlParameter data = new SqlParameter("@data", DBNull.Value);
            if (datoPianoEsternoLavorazione.Data != null)
            {
                data = new SqlParameter("@data", datoPianoEsternoLavorazione.Data);
            }
            data.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(data);

            SqlParameter orario = new SqlParameter("@orario", DBNull.Value);
            if (datoPianoEsternoLavorazione.Orario != null)
            {
                orario = new SqlParameter("@orario", datoPianoEsternoLavorazione.Orario);
            }
            orario.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(orario);

            SqlParameter nota = new SqlParameter("@nota", DBNull.Value);
            if (datoPianoEsternoLavorazione.Nota != null)
            {
                nota = new SqlParameter("@nota", datoPianoEsternoLavorazione.Nota);
            }
            nota.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(nota);

            SqlParameter numOccorrenza = new SqlParameter("@numOccorrenza", datoPianoEsternoLavorazione.NumOccorrenza);
            numOccorrenza.Direction = ParameterDirection.Input;
            StoreProc.Parameters.Add(numOccorrenza);
        }

    }
}