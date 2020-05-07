using System;
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
    public partial class ReportCollaboratoriPerGiornata : BasePage
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

        protected void btnRicercaCollaboratori_Click(object sender, EventArgs e)
        {
            DateTime dataTmp = Convert.ToDateTime(tbDataRicerca.Text.Trim());
            string sDataTmp = dataTmp.ToString("yyyy-MM-dd") + "T00:00:00.000";

            gv_Collaboratori.DataSource = null;
            gv_Collaboratori.DataBind();

            // VEDO SE ESISTONO LAVORAZIONI CON COLLABORATORI INGAGGIATI PER LA GIORNATA SELEZIONATA
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
            DataTable dtLavorazioniDelGiorno = Base_DAL.GetDatiBySql(queryRicercaLavorazioniDelGiorno, ref esito);
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
                esito = new Esito();
                DataTable dtQualifiche = Base_DAL.GetDatiBySql(ricercaQualifiche, ref esito);
                if (dtQualifiche != null && dtQualifiche.Rows != null && dtQualifiche.Rows.Count > 0)
                {
                    // TIRO FUORI L'ELENCO DELLE RIGHE, UNA PER OGNI QUALIFICA TROVATA
                    foreach (DataRow rigaQualifiche in dtQualifiche.Rows)
                    {
                        DataRow dr;
                        dr = dt.NewRow();
                        dr[0] = rigaQualifiche["qualifica"].ToString();

                        for (int i = 1; i < dt.Columns.Count; i++)
                        {
                            dr[i] = "qualifiche Lavorazione " + i.ToString();
                        }

                        dt.Rows.Add(dr);
                    }
                    gv_Collaboratori.DataSource = dt;
                    gv_Collaboratori.DataBind();
                }


            }

        }

        protected void gv_Collaboratori_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gv_Collaboratori_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void gv_Collaboratori_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

    }
}