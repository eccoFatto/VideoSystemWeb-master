﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using VideoSystemWeb.BLL;
using VideoSystemWeb.Entity;
using VideoSystemWeb.DAL;
using System.IO;
using System.Text.RegularExpressions;

namespace VideoSystemWeb.REPORT
{
    public partial class ReportCollaboratoriPerGiornataOld : BasePage
    {
        BasePage basePage = new BasePage();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_PreInit(object sender, EventArgs e)
        {
            CheckIsMobile();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                // IMPOSTO LA DATA AL GIORNO DI OGGI
                DateTime dataRicerca = DateTime.Today;
                tbDataRicerca.Text = dataRicerca.ToShortDateString();
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "chiudiLoader", script: "$('.loader').hide();", addScriptTags: true);
        }

        DataTable getLavorazioniDelGiorno(string sDataTmp)
        {
            DataTable dtRet = new DataTable();
            try
            {
                string queryRicercaLavorazioniDelGiorno = "select da.codice_lavoro,produzione,lavorazione " +
                    "from[dbo].[tab_dati_agenda] da " +
                    "left join tipo_colonne_agenda ca " +
                    "on da.id_colonne_agenda = ca.id " +
                    "left join tipo_stato ts " +
                    "on da.id_stato = ts.id " +
                    "left join tipo_tipologie tt " +
                    "on da.id_tipologia = tt.id " +
                    "left join dati_lavorazione dl " +
                    "on dl.idDatiAgenda = da.id " +
                    "left join dati_articoli_lavorazione dal " +
                    "on dl.id = dal.idDatiLavorazione " +
                    "left join anag_collaboratori ac " +
                    "on dal.idCollaboratori = ac.id " +
                    "where ac.cognome is not null " +
                    "and dal.descrizione <> 'Diaria' " +
                    "and data_inizio_lavorazione <= '@dataElaborazione' and data_fine_lavorazione >= '@dataElaborazione' " +
                    "group by da.codice_lavoro,produzione,lavorazione " +
                    "order by codice_lavoro";
                queryRicercaLavorazioniDelGiorno = queryRicercaLavorazioniDelGiorno.Replace("@dataElaborazione", sDataTmp);
                Esito esito = new Esito();
                dtRet = Base_DAL.GetDatiBySql(queryRicercaLavorazioniDelGiorno, ref esito);
                
            }
            catch (Exception ex)
            {
                ShowError("Errore durante la ricerca delle lavorazioni del giorno " + sDataTmp + Environment.NewLine + ex.Message);
            }
            return dtRet;
        }

        DataTable getQualificheDelGiorno(string sDataTmp)
        {
            DataTable dtRet = new DataTable();
            try
            {
                string ricercaQualifiche = "select dal.descrizione as qualifica " +
                     "from[dbo].[tab_dati_agenda] da " +
                     "left join tipo_colonne_agenda ca " +
                     "on da.id_colonne_agenda = ca.id " +
                     "left join tipo_stato ts " +
                     "on da.id_stato = ts.id " +
                     "left join tipo_tipologie tt " +
                     "on da.id_tipologia = tt.id " +
                     "left join dati_lavorazione dl " +
                     "on dl.idDatiAgenda = da.id " +
                     "left join dati_articoli_lavorazione dal " +
                     "on dl.id = dal.idDatiLavorazione " +
                     "left join anag_collaboratori ac " +
                     "on dal.idCollaboratori = ac.id " +
                     "where ac.cognome is not null " +
                     "and dal.descrizione <> 'Diaria' " +
                     "and data_inizio_lavorazione <= '@dataElaborazione' and data_fine_lavorazione >= '@dataElaborazione' " +
                     "group by dal.descrizione " +
                     "order by dal.descrizione";
                ricercaQualifiche = ricercaQualifiche.Replace("@dataElaborazione", sDataTmp);
                Esito esito = new Esito();

                dtRet = Base_DAL.GetDatiBySql(ricercaQualifiche, ref esito);

            }
            catch (Exception ex)
            {
                ShowError("Errore durante la ricerca delle Qualifiche del giorno " + sDataTmp + Environment.NewLine + ex.Message);
            }
            return dtRet;
        }

        string getCollaboratoriByCodiceQualifica(string sDataTmp, string codiceLavorazione, string qualifica)
        {
            string elencoCollaboratori = "";
            DataTable dtRet = new DataTable();
            try
            {
                string ricercaCollaboratori = "select ac.cognome + ' ' + ac.nome as nominativo " +
                    "from[dbo].[tab_dati_agenda] da " +
                    "left join tipo_colonne_agenda ca " +
                    "on da.id_colonne_agenda = ca.id " +
                    "left join tipo_stato ts " +
                    "on da.id_stato = ts.id " +
                    "left join tipo_tipologie tt " +
                    "on da.id_tipologia = tt.id " +
                    "left join dati_lavorazione dl " +
                    "on dl.idDatiAgenda = da.id " +
                    "left join dati_articoli_lavorazione dal " +
                    "on dl.id = dal.idDatiLavorazione " +
                    "left join anag_collaboratori ac " +
                    "on dal.idCollaboratori = ac.id " +
                    "where ac.cognome is not null " +
                    "and dal.descrizione <> 'Diaria' " +
                    "and data_inizio_lavorazione <= '@dataElaborazione' and data_fine_lavorazione >= '@dataElaborazione' " +
                    "and codice_lavoro = '@codiceLavorazione' " +
                    "and dal.descrizione = '@qualifica' " +
                    "group by ac.cognome,ac.nome " +
                    "order by ac.cognome,ac.nome";

                ricercaCollaboratori = ricercaCollaboratori.Replace("@dataElaborazione", sDataTmp);
                ricercaCollaboratori = ricercaCollaboratori.Replace("@codiceLavorazione", codiceLavorazione);
                ricercaCollaboratori = ricercaCollaboratori.Replace("@qualifica", qualifica);

                Esito esito = new Esito();

                dtRet = Base_DAL.GetDatiBySql(ricercaCollaboratori, ref esito);
                if(dtRet!=null && dtRet.Rows!=null && dtRet.Rows.Count > 0)
                {
                    foreach (DataRow rigaCollaboratore in dtRet.Rows)
                    {
                        //elencoCollaboratori += "," + rigaCollaboratore["nominativo"].ToString();
                        elencoCollaboratori += Environment.NewLine + rigaCollaboratore["nominativo"].ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                ShowError("Errore durante la ricerca dei collaboratori del giorno " + sDataTmp + Environment.NewLine + ex.Message);
            }

            //if (elencoCollaboratori.Length > 2 && elencoCollaboratori.Substring(0, 2).Equals("\r\n")) elencoCollaboratori = elencoCollaboratori.Substring(2, elencoCollaboratori.Length - 2);
            if (elencoCollaboratori.Length > 2 && elencoCollaboratori.Substring(0, 2).Equals(Environment.NewLine)) elencoCollaboratori = elencoCollaboratori.Substring(2, elencoCollaboratori.Length - 2);

            return elencoCollaboratori;
        }

        protected void btnRicercaCollaboratori_Click(object sender, EventArgs e)
        {
            DateTime dataTmp = Convert.ToDateTime(tbDataRicerca.Text.Trim());
            string sDataTmp = dataTmp.ToString("yyyy-MM-dd") + "T00:00:00.000";
            // RESETTO LA GRIGLIA
            gv_Collaboratori.DataSource = null;
            gv_Collaboratori.DataBind();

            // VEDO SE ESISTONO LAVORAZIONI CON COLLABORATORI INGAGGIATI PER LA GIORNATA SELEZIONATA
            DataTable dtLavorazioniDelGiorno = getLavorazioniDelGiorno(sDataTmp);
            Session["TaskTableLavorazioniDelGiorno"] = dtLavorazioniDelGiorno;

            if (dtLavorazioniDelGiorno!=null && dtLavorazioniDelGiorno.Rows!=null && dtLavorazioniDelGiorno.Rows.Count>0)
            {
                // CREO L'ELENCO DELLE COLONNE CON LE LAVORAZIONI TROVATE
                DataTable dt = new DataTable();
                dt.Columns.Add("Qualifica", System.Type.GetType("System.String"));
                foreach (DataRow rigaLavorazioni in dtLavorazioniDelGiorno.Rows)
                {
                    dt.Columns.Add(rigaLavorazioni["codice_lavoro"].ToString() + " - " + rigaLavorazioni["produzione"].ToString() + " - " + rigaLavorazioni["lavorazione"].ToString(), System.Type.GetType("System.String"));
                }
                // TIRO FUORI L'ELENCO DELLE QUALIFICHE PRESENTI
                DataTable dtQualifiche = getQualificheDelGiorno(sDataTmp);
                if (dtQualifiche != null && dtQualifiche.Rows != null && dtQualifiche.Rows.Count > 0)
                {
                    // TIRO FUORI L'ELENCO DELLE RIGHE, UNA PER OGNI QUALIFICA TROVATA
                    foreach (DataRow rigaQualifiche in dtQualifiche.Rows)
                    {
                        DataRow dr;
                        dr = dt.NewRow();
                        dr[0] = rigaQualifiche["qualifica"].ToString();

                        //PRENDO LA QUALIFICA
                        string qualifica = rigaQualifiche["qualifica"].ToString();
                        //PER OGNI LAVORAZIONE PRENDO LE FIGURE PROFESSIONALI CON QUELLA QUALIFICA

                        for (int i = 0; i < dtLavorazioniDelGiorno.Rows.Count; i++)
                        {
                            // ESTRAPOLO IL CODICE LAVORAZIONE
                            string codLav = dtLavorazioniDelGiorno.Rows[i]["codice_lavoro"].ToString();
                            // ESTRAPOLO LE FIGURE
                            string elencoCollaboratori = getCollaboratoriByCodiceQualifica(sDataTmp, codLav, qualifica);
                            dr[i+1] = elencoCollaboratori;
                        }
                        //for (int i = 1; i < dt.Columns.Count; i++)
                        //{
                        //    dr[i] = "qualifiche Lavorazione " + i.ToString();
                        //}
                        dt.Rows.Add(dr);

                    }
                    gv_Collaboratori.DataSource = dt;
                    gv_Collaboratori.DataBind();
                }


            }

        }

        protected void gv_Collaboratori_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Style.Add("font-weight", "bold");
                e.Row.Cells[0].Style.Add("vertical-align", "top");
                for (int i = 1; i < e.Row.Cells.Count; i++)
                {
                    string data = e.Row.Cells[i].Text;
                    data = data.Replace("\r\n", "<br>");
                    e.Row.Cells[i].Text = data;
                    e.Row.Cells[i].Style.Add("vertical-align", "top");
                }
            }
        }

        protected void gv_Collaboratori_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void gv_Collaboratori_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

    }
}