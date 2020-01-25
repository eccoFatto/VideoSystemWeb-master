using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoSystemWeb.BLL;
using VideoSystemWeb.Entity;

namespace VideoSystemWeb.REPORT
{
    public partial class StampaCollabortori : BasePage
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            CheckIsMobile();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnStampa.CssClass = "w3-btn w3-white w3-border w3-border-blue w3-round-large w3-disabled";
                CaricaCombo();
            }

            #region GRIGLIA CON RAGGRUPPAMENTO RIGHE
            GridViewHelper helper = new GridViewHelper(this.gv_DatiStampa);

            string[] cols = new string[4];
            cols[0] = "IndirizzoCollaboratore";
            cols[1] = "CittaCollaboratore";
            cols[2] = "TelefonoCollaboratore";
            cols[3] = "CodFiscaleCollaboratore";

            helper.RegisterGroup("NomeCollaboratore", true, true);
            helper.RegisterGroup(cols, true, true);
            helper.GroupHeader += new GroupEvent(Helper_GroupHeader);
            helper.GroupSummary += new GroupEvent(Helper_GroupSummary);
            helper.GeneralSummary += new FooterEvent(Helper_GeneralSummary);

            //SUBTOTALE
            helper.RegisterSummary("Mista", SummaryOperation.Sum, "NomeCollaboratore");
            helper.RegisterSummary("Assunzione", SummaryOperation.Sum, "NomeCollaboratore");
            helper.RegisterSummary("RitenutaAcconto", SummaryOperation.Sum, "NomeCollaboratore");
            helper.RegisterSummary("Fattura", SummaryOperation.Sum, "NomeCollaboratore");
            helper.RegisterSummary("Diaria", SummaryOperation.Sum, "NomeCollaboratore");

            //TOTALE
            helper.RegisterSummary("Mista", SummaryOperation.Sum);
            helper.RegisterSummary("Assunzione", SummaryOperation.Sum);
            helper.RegisterSummary("RitenutaAcconto", SummaryOperation.Sum);
            helper.RegisterSummary("Fattura", SummaryOperation.Sum);
            helper.RegisterSummary("Diaria", SummaryOperation.Sum);

            helper.ApplyGroupSort();
            #endregion

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "chiudiLoader", script: "$('.loader').hide();", addScriptTags: true);
        }

        private void CaricaCombo()
        {
            for (var i = DateTime.Now.Year; i >= DateTime.Now.Year - 10; i--)
            {
                ddl_Anno.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
        }

        protected void btnRicerca_Click(object sender, EventArgs e)
        {
            Esito esito = new Esito();
            DateTime dataInizio = DateTime.Parse(txt_DataInizio.Text);
            DateTime dataFine = DateTime.Parse(txt_DataFine.Text);

            List<DatiReportRaw> listaDatiReport = Report_BLL.Instance.GetListaDatiReportRawCollaboratoriFornitori(dataInizio, dataFine, ref esito);

            gv_DatiStampa.DataSource = listaDatiReport;
            gv_DatiStampa.DataBind();


            if (listaDatiReport.Count > 0)
            {
                btnStampa.CssClass = btnStampa.CssClass.Replace("w3-disabled", "");
            }
            else
            {
                btnStampa.CssClass = "w3-btn w3-white w3-border w3-border-blue w3-round-large w3-disabled";
            }
        }

        protected void gv_DatiStampa_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_DatiStampa.PageIndex = e.NewPageIndex;
            btnRicerca_Click(null, null);
        }

        protected void gv_DatiStampa_OnSorting(object sender, EventArgs e)
        {
            // do something here or do nothing
        }

        private void Helper_GroupHeader(string groupName, object[] values, GridViewRow row)
        {
            //if (groupName != "NomeCollaboratore")
            //{
            //    values[0] = "Indirizzo: " + values[0];
            //    values[1] = " - Città: " + values[1];
            //    values[2] = " - Telefono: " + values[2];
            //    values[3] = " - Partita IVA: " + values[3];

            //    row.Cells[0].Text = values[0].ToString() + values[1].ToString() + values[2].ToString() + values[3].ToString();
            //}

            row.BackColor = Color.LightGray;
            row.Cells[0].Text = "&nbsp;&nbsp;<b>" + row.Cells[0].Text + "</b>";
        }

        private void Helper_GroupSummary(string groupName, object[] values, GridViewRow row)
        {
            row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
            row.Cells[0].Text = "<b><i>Subtotale</i></b>";
            row.Cells[1].Text = "<b><i>" + row.Cells[1].Text + "</i></b>";
            row.Cells[2].Text = "<b><i>" + row.Cells[2].Text + "</i></b>";
            row.Cells[3].Text = "<b><i>" + row.Cells[3].Text + "</i></b>";
            row.Cells[4].Text = "<b><i>" + row.Cells[4].Text + "</i></b>";
            row.Cells[5].Text = "<b><i>" + row.Cells[5].Text + "</i></b>";
        }

        private void Helper_GeneralSummary(GridViewRow row)
        {
            row.BackColor = Color.Gray;
            row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
            row.Cells[0].Text = "<b>Totale</b>";
            row.Cells[1].Text = "<b>" + row.Cells[1].Text + "</b>";
            row.Cells[2].Text = "<b>" + row.Cells[2].Text + "</b>";
            row.Cells[3].Text = "<b>" + row.Cells[3].Text + "</b>";
            row.Cells[4].Text = "<b>" + row.Cells[4].Text + "</b>";
            row.Cells[5].Text = "<b>" + row.Cells[5].Text + "</b>";
        }
    }
}