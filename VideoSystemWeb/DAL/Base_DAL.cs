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
        //public static string constr = ConfigurationManager.ConnectionStrings["constrMSSQL_NIC"].ConnectionString;
        //string static constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
        public static string constr = ConfigurationManager.ConnectionStrings["constrMSSQL"].ConnectionString;
        // STRINGA DI CONNESSIONE PER CLASSI SQLCLIENT (NON SERVE SPECIFICARE IL DATA SOURCE)
        public static string sqlConstr = ConfigurationManager.ConnectionStrings["sqlConstrMSSQL"].ConnectionString;

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
                esito.descrizione = ex.Message + Environment.NewLine + ex.StackTrace;
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
                esito.descrizione = ex.Message + Environment.NewLine + ex.StackTrace;
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
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM " + nomeTipologica + soloAttivi + " ORDER BY id"))
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
                esito.descrizione = ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return listaTipologiche;
        }
    }
}