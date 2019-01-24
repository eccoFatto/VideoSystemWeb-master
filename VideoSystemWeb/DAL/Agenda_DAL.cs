using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using VideoSystemWeb.BLL;
using VideoSystemWeb.Entity;

namespace VideoSystemWeb.DAL
{
    public class Agenda_DAL: Base_DAL
    {
        //singleton
        private static volatile Agenda_DAL instance;
        private static object objForLock = new Object();

        private Agenda_DAL() { }

        public static Agenda_DAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Agenda_DAL();
                    }
                }
                return instance;
            }
        }

        public List<Tipologica> CaricaColonne(ref Esito esito)
        {
            return CaricaTipologica(EnumTipologiche.TIPO_COLONNE_AGENDA, true, ref esito);
        }

        public List<DatiAgenda> CaricaDatiAgenda(ref Esito esito)
        {
            List<DatiAgenda> listaDatiAgenda = new List<DatiAgenda>();
            try
            {
                using (System.Data.OleDb.OleDbConnection con = new System.Data.OleDb.OleDbConnection(constr))
                {
                    using (System.Data.OleDb.OleDbCommand cmd = new System.Data.OleDb.OleDbCommand("SELECT * FROM tab_dati_agenda"))
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
                                        DatiAgenda datoAgenda = new DatiAgenda();
                                        datoAgenda.id = int.Parse(riga["id"].ToString());
                                        datoAgenda.id_colonne_agenda = int.Parse(riga["id_colonne_agenda"].ToString());
                                        datoAgenda.id_stato = int.Parse(riga["id_stato"].ToString());
                                        datoAgenda.data_inizio_lavorazione = DateTime.Parse(riga["data_inizio_lavorazione"].ToString());
                                        datoAgenda.data_fine_lavorazione = DateTime.Parse(riga["data_fine_lavorazione"].ToString());
                                        datoAgenda.durata_lavorazione = int.Parse(riga["durata_lavorazione"].ToString());
                                        datoAgenda.id_tipologia = riga["id_tipologia"] != DBNull.Value ? riga.Field<int>("id_tipologia") : 0 ;
                                        datoAgenda.id_cliente = riga["id_cliente"] != DBNull.Value ? riga.Field<int>("id_cliente") : 0; 
                                        datoAgenda.durata_viaggio_andata = int.Parse(riga["durata_viaggio_andata"].ToString());
                                        datoAgenda.durata_viaggio_ritorno = int.Parse(riga["durata_viaggio_ritorno"].ToString());
                                        datoAgenda.data_inizio_impegno = riga["data_inizio_impegno"] != DBNull.Value ? riga.Field<DateTime>("data_inizio_impegno") : DateTime.MinValue;
                                        datoAgenda.data_fine_impegno = riga["data_fine_impegno"] != DBNull.Value ? riga.Field<DateTime>("data_fine_impegno") : DateTime.MaxValue; 
                                        datoAgenda.impegnoOrario = bool.Parse(riga["impegnoOrario"].ToString());
                                        datoAgenda.impegnoOrario_da = riga["impegnoOrario_da"] != DBNull.Value ? riga.Field<DateTime>("impegnoOrario_da") : DateTime.MinValue;
                                        datoAgenda.impegnoOrario_a = riga["impegnoOrario_a"] != DBNull.Value ? riga.Field<DateTime>("impegnoOrario_a") : DateTime.MaxValue; 
                                        datoAgenda.produzione = riga["produzione"].ToString();
                                        datoAgenda.lavorazione = riga["lavorazione"].ToString();
                                        datoAgenda.indirizzo = riga["indirizzo"].ToString();
                                        datoAgenda.luogo = riga["luogo"].ToString();
                                        datoAgenda.codice_lavoro = riga["codice_lavoro"].ToString();
                                        datoAgenda.nota = riga["nota"].ToString();

                                        listaDatiAgenda.Add(datoAgenda);
                                    }
                                }
                                else
                                {
                                    esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                                    esito.descrizione = "Nessun dato trovato nella tabella tab_dati_agenda ";
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

            return listaDatiAgenda;
        }
    }
}