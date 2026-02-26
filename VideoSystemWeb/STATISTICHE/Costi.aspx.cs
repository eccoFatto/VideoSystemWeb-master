using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoSystemWeb.BLL;
using VideoSystemWeb.Entity;

namespace VideoSystemWeb.STATISTICHE
{
    public partial class Costi : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
        
                CaricaCombo();
            }

            #region GRIGLIA CON RAGGRUPPAMENTO RIGHE
            GestioneRaggruppamentoRighe();

            #endregion

            ScriptManager.RegisterStartupScript(this, typeof(Page), "coerenzaDate", "controlloCoerenzaDate('" + txt_PeriodoDa.ClientID + "', '" + txt_PeriodoA.ClientID + "');", true);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "chiudiLoader", script: "$('.loader').hide();", addScriptTags: true);
        }

        private void GestioneRaggruppamentoRighe()
        {
            GridViewHelper helper = new GridViewHelper(this.gv_statistiche);

            #region RAGGRUPPAMENTO
            string[] colonneRaggruppate = new string[2];
            colonneRaggruppate[0] = "Cliente";
            colonneRaggruppate[1] = "CodiceLavoro";
            helper.RegisterGroup(colonneRaggruppate, true, true);

            //SUBTOTALE
            helper.RegisterSummary("Listino", SummaryOperation.Sum, "Cliente+CodiceLavoro");
            helper.RegisterSummary("Costo", SummaryOperation.Sum, "Cliente+CodiceLavoro");
            helper.RegisterSummary("Ricavo", SummaryOperation.Count, "Cliente+CodiceLavoro");
            #endregion

            #region SOTTOGRUPPO
            //helper.RegisterGroup("Cliente", true, true);
            //helper.RegisterGroup("CodiceLavoro", true, true);


            ////SUBTOTALE
            //helper.RegisterSummary("Listino", SummaryOperation.Sum, "CodiceLavoro");
            //helper.RegisterSummary("Costo", SummaryOperation.Sum, "CodiceLavoro");
            //helper.RegisterSummary("Ricavo", SummaryOperation.Count, "CodiceLavoro");
            #endregion

            ////TOTALE
            helper.RegisterSummary("Listino", SummaryOperation.Sum);
            helper.RegisterSummary("Costo", SummaryOperation.Sum);
            helper.RegisterSummary("Ricavo", SummaryOperation.Count);

            helper.GroupHeader += new GroupEvent(Helper_GroupHeader);
            helper.GroupSummary += new GroupEvent(Helper_GroupSummary);
            helper.GeneralSummary += new FooterEvent(Helper_GeneralSummary);
            
        }

        private void Helper_GroupHeader(string groupName, object[] values, GridViewRow row)
        {
            if (groupName == "Cliente")
            {
                row.BackColor = Color.FromArgb(0, 64, 128);
                row.ForeColor = Color.White;
                row.Cells[0].Text = "&nbsp;&nbsp;<b>Cliente:&nbsp;" + row.Cells[0].Text + "</b>";
            }
            else if (groupName == "CodiceLavoro")
            {
                row.BackColor = Color.LightGray;
                row.Cells[0].Text = "&nbsp;&nbsp;&nbsp;&nbsp;<i><b>Codice lavorazione:&nbsp;" + row.Cells[0].Text + "</b></i>";
            }
            else //raggruppamento
            {
                string titolo = row.Cells[0].Text;
                string cliente = titolo.Substring(0, titolo.LastIndexOf(" - "));
                string codiceLavorazione = titolo.Substring(titolo.LastIndexOf(" - ")+3);

                row.BackColor = Color.FromArgb(0, 64, 128);
                row.ForeColor = Color.White;
                row.Cells[0].Text = "&nbsp;&nbsp;<b>Cliente:&nbsp;" + cliente + "&nbsp;-&nbsp;Codice lavorazione:&nbsp;" + codiceLavorazione + "</b>";
            }
        }

        private void Helper_GroupSummary(string groupName, object[] values, GridViewRow row)
        {
            row.Cells[0].HorizontalAlign = HorizontalAlign.Right;

            decimal listino;
            decimal costo;

            bool isOkListino = decimal.TryParse(row.Cells[1].Text, out listino);
            bool isOkCosto = decimal.TryParse(row.Cells[2].Text, out costo);

            decimal ricavo = new decimal(0);

            if (isOkListino && isOkCosto && listino > 0)
            {
                ricavo = (listino - costo) / listino;
            }

            if (chk_Listino.Checked || chk_Costi.Checked || chk_Ricavo.Checked)
            {
                row.Cells[0].Text = "<b><i>Subtotale</i></b>";
                row.Cells[1].Text = "<b><i>" + row.Cells[1].Text + "</i></b>";
                row.Cells[2].Text = "<b><i>" + row.Cells[2].Text + "</i></b>";
                row.Cells[3].Text = "<b><i>" + string.Format("{0:P2}", ricavo) + "</i></b>";
            }
            row.Cells[1].Visible = chk_Listino.Checked;
            row.Cells[2].Visible = chk_Costi.Checked;
            row.Cells[3].Visible = chk_Ricavo.Checked &&
                                   string.IsNullOrWhiteSpace(txt_Fornitore.Text) &&
                                   ddl_Genere.SelectedValue == "" && 
                                   ddl_Gruppo.SelectedValue == "" && 
                                   ddl_Sottogruppo.SelectedValue == ""; // se questi filtri sono selezionati non mostro il margine, che darebbe un valore errato

            row.Cells[4].Visible = false;
        }

        private void Helper_GeneralSummary(GridViewRow row)
        {
            row.BackColor = Color.Gray;
            row.Cells[0].HorizontalAlign = HorizontalAlign.Right;

            decimal listino;
            decimal costo;

            bool isOkListino = decimal.TryParse(row.Cells[1].Text, out listino);
            bool isOkCosto = decimal.TryParse(row.Cells[2].Text, out costo);

            decimal ricavo = new decimal(0);

            if (isOkListino && isOkCosto && listino > 0)
            {
                ricavo = (listino - costo) / listino;
            }

            if (chk_Listino.Checked || chk_Costi.Checked || chk_Ricavo.Checked)
            {
                row.Cells[0].Text = "<b>Totale</b>";
                row.Cells[1].Text = "<b>" + row.Cells[1].Text + "</b>";
                row.Cells[2].Text = "<b>" + row.Cells[2].Text + "</b>";
                row.Cells[3].Text = "<b>" + string.Format("{0:P2}", ricavo) + "</b>";
            }
            row.Cells[1].Visible = chk_Listino.Checked;
            row.Cells[2].Visible = chk_Costi.Checked;
            row.Cells[3].Visible = chk_Ricavo.Checked &&
                                   string.IsNullOrWhiteSpace(txt_Fornitore.Text) &&
                                   ddl_Genere.SelectedValue == "" && 
                                   ddl_Gruppo.SelectedValue == "" && 
                                   ddl_Sottogruppo.SelectedValue == ""; // se questi filtri sono selezionati non mostro il margine, che darebbe un valore errato

            row.Cells[4].Visible = false;
        }

        private void CaricaCombo()
        {
            #region GENERE
            ddl_Genere.Items.Add(new ListItem("", ""));
            foreach (Tipologica tipoGenere in SessionManager.ListaTipiGeneri)
            {
                ddl_Genere.Items.Add(new ListItem(tipoGenere.nome, tipoGenere.id.ToString()));
            }
            #endregion

            #region GRUPPO
            ddl_Gruppo.Items.Add(new ListItem("", ""));
            foreach (Tipologica tipoGruppi in SessionManager.ListaTipiGruppi)
            {
                ddl_Gruppo.Items.Add(new ListItem(tipoGruppi.nome, tipoGruppi.id.ToString()));
            }
            #endregion

            #region SOTTOGRUPPO
            ddl_Sottogruppo.Items.Add(new ListItem("", ""));
            //foreach (Tipologica tipoSottogruppi in SessionManager.ListaTipiSottogruppi)
            //{
            //    ddl_Sottogruppo.Items.Add(new ListItem(tipoSottogruppi.nome, tipoSottogruppi.id.ToString()));
            //}
            #endregion
        }

        protected void btnEseguiStatistica_Click(object sender, EventArgs e)
        {
            Esito esito = new Esito();

            string filtroNomeCliente = txt_Cliente.Text;// hf_NomeCliente.Value;
            string filtroNomeProduzione = txt_Produzione.Text;// hf_NomeProduzione.Value;
            string filtroNomeLavorazione = txt_Lavorazione.Text;// hf_NomeLavorazione.Value;
            string filtroNomeContratto = txt_Contratto.Text;// hf_NomeContratto.Value;

            string filtroCodLavorazione = txt_CodLavorazione.Text;// hf_NomeLavorazione.Value;

            string filtroGenere = ddl_Genere.SelectedValue;
            string filtroGruppo = ddl_Gruppo.SelectedValue;
            string filtroSottogruppo = ddl_Sottogruppo.SelectedValue;

            string dataInizio = txt_PeriodoDa.Text;
            string dataFine = txt_PeriodoA.Text;

            string filtroFornitore = txt_Fornitore.Text;

            List<StatisticheCosti> listaStatisticheCosti = Statistiche_BLL.Instance.GetStatisticheCosti_NomeLavorazione(filtroNomeCliente, filtroNomeProduzione, filtroNomeLavorazione, filtroNomeContratto, filtroGenere, filtroGruppo, filtroSottogruppo, filtroCodLavorazione, dataInizio, dataFine, filtroFornitore, ref esito);

            if (listaStatisticheCosti.Count == 0)
            {
                ShowWarning("Nessuna voce trovata per i parametri immessi");
            }
            else
            {
                decimal? totaleListino = listaStatisticheCosti.Sum(x => x.Listino);
                decimal? totaleCosto = listaStatisticheCosti.Sum(x => x.Costo);
                decimal? totaleRicavo = (totaleListino - totaleCosto) / totaleListino;

                lbl_TotaleListino.Text = string.Format("{0:N2}", totaleListino);
                lbl_TotaleCosto.Text = string.Format("{0:N2}", totaleCosto);
                lbl_TotaleRicavo.Text = string.Format("{0:P2}", totaleRicavo);
                colonnaRicavo.Visible = chk_Ricavo.Checked &&
                                           string.IsNullOrWhiteSpace(txt_Fornitore.Text) &&
                                           ddl_Genere.SelectedValue == "" &&
                                           ddl_Gruppo.SelectedValue == "" &&
                                           ddl_Sottogruppo.SelectedValue == ""; // se questi filtri sono selezionati non mostro il margine, che darebbe un valore errato
                rigaTotali.Visible = true;
            }
            tbTotElementiGriglia.Text = listaStatisticheCosti.Count.ToString("###,##0");
            gv_statistiche.DataSource = listaStatisticheCosti;
            gv_statistiche.DataBind();
        }
         
        protected void btnPulisciCampiRicerca_Click(object sender, EventArgs e)
        {
            gv_statistiche.DataSource = null;
            gv_statistiche.DataBind();
            tbTotElementiGriglia.Text = 0.ToString("###,##0");
            rigaTotali.Visible = false;
        }

        protected void gv_statistiche_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 1)
            {
                e.Row.Cells[11].Visible = chk_Listino.Checked;
                e.Row.Cells[12].Visible = chk_Costi.Checked;
                e.Row.Cells[13].Visible = chk_Ricavo.Checked &&
                                          string.IsNullOrWhiteSpace(txt_Fornitore.Text) &&
                                          ddl_Genere.SelectedValue == "" && 
                                          ddl_Gruppo.SelectedValue == "" && 
                                          ddl_Sottogruppo.SelectedValue == ""; // se questi filtri sono selezionati non mostro il margine, che darebbe un valore errato
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string pathDocumento = e.Row.Cells[14].Text.Trim();
                bool pregresso = false;
                bool.TryParse(e.Row.Cells[15].Text.Trim(), out pregresso);

                ImageButton myButton = e.Row.FindControl("btnOpenDoc") as ImageButton;
                if (!string.IsNullOrEmpty(pathDocumento) && !pathDocumento.Equals("&nbsp;"))
                {

                    string pathRelativo = pregresso ? ConfigurationManager.AppSettings["PATH_DOCUMENTI_PREGRESSO"].Replace("~", "") : ConfigurationManager.AppSettings["PATH_DOCUMENTI_PROTOCOLLO"].Replace("~", "");


                    string pathCompleto = pathRelativo + pathDocumento + "?t=" + DateTime.Now.Ticks;
                    myButton.Attributes.Add("onclick", "window.open('" + pathCompleto + "');return false;");
                }
                else
                {
                    myButton.Visible = false;
                    myButton.Attributes.Add("disabled", "true");
                }
            }
            e.Row.Cells[14].Visible = false;
            e.Row.Cells[15].Visible = false;
        }

        protected void ddlGruppo_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            try
            {
                CaricaSottogruppiInBaseAlGruppo(ddl_Gruppo, ddl_Sottogruppo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        private void CaricaSottogruppiInBaseAlGruppo(DropDownList listaGruppo, DropDownList listaSottogruppo)
        {
            Esito esito = new Esito();

            string idTipoGruppoString = listaGruppo.SelectedValue;

            if (!string.IsNullOrEmpty(idTipoGruppoString))
            {
                int idTipoGruppo = int.Parse(idTipoGruppoString);

                listaSottogruppo.Items.Clear();
                listaSottogruppo.Items.Add("");

                List<Sottogruppo> listaSottogruppiFiltrata = (SessionManager.ListaTipiSottogruppi).Where(x => x.IdTipoGruppo == idTipoGruppo || x.IdTipoGruppo == null).ToList<Sottogruppo>();

                foreach (Tipologica tipologiaSottogruppo in listaSottogruppiFiltrata)
                {
                    ListItem item = new ListItem();
                    item.Text = tipologiaSottogruppo.nome;
                    item.Value = tipologiaSottogruppo.id.ToString();
                    listaSottogruppo.Items.Add(item);
                }
            }
            else
            {
                listaSottogruppo.Items.Clear();
                listaSottogruppo.Items.Add("");
            }
        }
    }
}