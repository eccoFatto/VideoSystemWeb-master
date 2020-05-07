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

            string queryRicerca = "select dal.descrizione as qualifica,ac.cognome as cognome_operatore, ac.nome as nome_operatore, da.codice_lavoro,produzione,lavorazione " +
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
            "where ac.cognome is not null and dal.descrizione<> 'Diaria' " +
            "and data_inizio_lavorazione <= '@dataElaborazione' and data_fine_lavorazione >= '@dataElaborazione' " +
            "group by dal.descrizione ,ac.cognome, ac.nome, da.codice_lavoro,produzione,lavorazione " +
            "order by qualifica,ac.cognome, ac.nome, codice_lavoro";
            queryRicerca = queryRicerca.Replace("@dataElaborazione", sDataTmp);

            queryRicerca = queryRicerca.Replace("@dataElaborazione", sDataTmp);

            Esito esito = new Esito();
            DataTable dtCollaboratori = Base_DAL.GetDatiBySql(queryRicerca, ref esito);

            Session["TaskTableCollaboratori"] = dtCollaboratori;
            gv_Collaboratori.DataSource = Session["TaskTableCollaboratori"];
            gv_Collaboratori.DataBind();

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