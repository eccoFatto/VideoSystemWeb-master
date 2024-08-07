﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoSystemWeb.BLL;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;

namespace VideoSystemWeb.CercaLavorazione
{
    public partial class CercaLavorazione : BasePage
    {
        BasePage basePage = new BasePage();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void ShowPopMessage(string messaggio)
        {
            messaggio = messaggio.Replace("'", "\\'");
            messaggio = messaggio.Replace("\r\n", "<br/>");

            Page page = HttpContext.Current.Handler as Page;
            ScriptManager.RegisterStartupScript(page, page.GetType(), "apripopupProt", script: "popupProt('" + messaggio + "')", addScriptTags: true);
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            CheckIsMobile();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                //Session["NOME_FILE"] = "";

                ddlTipoProtocollo.Items.Clear();
                ddlTipoProtocollo.Items.Add("");
                CaricaCombo();

                if (SessionManager.CercaLavorazione_Risultati != null)
                {
                    PopolaFiltri_Risultati();
                }
            }

            ScriptManager.RegisterStartupScript(this, typeof(Page), "coerenzaDate", "controlloCoerenzaDate('" + tbDataLavorazione.ClientID + "', '" + tbDataLavorazioneA.ClientID + "');", true);
            ScriptManager.RegisterStartupScript(this, typeof(Page), "coerenzaDate2", "controlloCoerenzaDate('" + tbDataProtocollo.ClientID + "', '" + tbDataProtocolloA.ClientID + "');", true);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "chiudiLoader", script: "$('.loader').hide();", addScriptTags: true);
        }

        private void CaricaCombo()
        {
            Esito esito = new Esito();

            #region TIPO PROTOCOLLO
            foreach (Tipologica tipologiaProtocollo in SessionManager.ListaTipiProtocolli)
            {
                ListItem item = new ListItem();
                item.Text = tipologiaProtocollo.nome;
                item.Value = tipologiaProtocollo.nome;

                ddlTipoProtocollo.Items.Add(item);
            }
            #endregion
        }

        protected void btnRicercaLavorazione_Click(object sender, EventArgs e)
        {
            Esito esito = new Esito();
            FiltriCercaLavorazione filtri = new FiltriCercaLavorazione();

            string numeroProtocollo = filtri.NumeroProtocollo = tbNumeroProtocollo.Text.Trim();
            string codiceLavoro = filtri.CodiceLavoro = tbCodiceLavoro.Text.Trim();
            string ragioneSociale = filtri.ClienteFornitore = tbRagioneSociale.Text.Trim();
            string produzione = filtri.Produzione = tbProduzione.Text.Trim();
            string lavorazione = filtri.Lavorazione = tbLavorazione.Text.Trim();
            string descrizione = filtri.Descrizione = tbDescrizione.Text.Trim();
            string tipoProtocollo = filtri.Tipo = ddlTipoProtocollo.SelectedValue;
            string protocolloRiferimento = filtri.RiferimentoDocumento = tbProtocolloRiferimento.Text.Trim();
            string destinatario = filtri.Destinatario = ddlDestinatario.SelectedValue;
            string dataProtocolloDa = filtri.DataProtocollo_Da = tbDataProtocollo.Text;
            string dataProtocolloA = filtri.DataProtocollo_A = tbDataProtocolloA.Text;
            string dataLavorazioneDa = filtri.DataLavorazione_Da = tbDataLavorazione.Text;
            string dataLavorazioneA = filtri.DataLavorazione_A = tbDataLavorazioneA.Text;

            DataTable dtLavorazioni = Agenda_BLL.Instance.CercaLavorazione(numeroProtocollo, codiceLavoro, ragioneSociale, produzione, lavorazione, descrizione, tipoProtocollo, protocolloRiferimento, destinatario, dataProtocolloDa, dataProtocolloA, dataLavorazioneDa, dataLavorazioneA, ref esito);
            SessionManager.CercaLavorazione_Risultati = dtLavorazioni;
            SessionManager.CercaLavorazione_NumPagina = 0;
            SessionManager.CercaLavorazione_Filtri = filtri;

            //Session["TaskTable"] = dtLavorazioni;
            gv_protocolli.DataSource = dtLavorazioni;
            gv_protocolli.DataBind();
            tbTotElementiGriglia.Text = dtLavorazioni.Rows.Count.ToString("###,##0");
        }

        protected void gv_protocolli_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 1)
            {
                e.Row.Cells[0].Visible = false; // id
                e.Row.Cells[1].Visible = false; // data inizio impegno
                e.Row.Cells[2].Visible = false; // id colonne agenda
            }

            // PRENDO L'ID DEL PROTOCOLLO SELEZIONATO
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string dataInizioImpegno = e.Row.Cells[GetColumnIndexByName(e.Row, "data_inizio_impegno")].Text;
                string idColonneAgenda = e.Row.Cells[GetColumnIndexByName(e.Row, "id_colonne_agenda")].Text;
                ImageButton myButtonEdit = e.Row.FindControl("imgEdit") as ImageButton;
                if (myButtonEdit != null)
                {
                    myButtonEdit.Attributes.Add("onclick", "apriLavorazione('" + dataInizioImpegno + "','" + idColonneAgenda + "');");
                }
            }
        }

        protected void btnVaiALavorazione_Click(object sender, EventArgs e)
        {
            string dataInizioImpegno = hf_dataInizioImpegno.Value;
            string idColonneAgenda = hf_idColonneAgenda.Value;

            SessionManager.CercaLavorazione_Data = dataInizioImpegno;
            SessionManager.CercaLavorazione_Colonna = idColonneAgenda;

            Response.Redirect("/Agenda/Agenda");
        }

        protected void gv_protocolli_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_protocolli.PageIndex = e.NewPageIndex;
            SessionManager.CercaLavorazione_NumPagina = e.NewPageIndex;
            //btnRicercaLavorazione_Click(null, null);

            gv_protocolli.DataSource = SessionManager.CercaLavorazione_Risultati;
            gv_protocolli.DataBind();
            tbTotElementiGriglia.Text = SessionManager.CercaLavorazione_Risultati.Rows.Count.ToString("###,##0");

        }

        protected void gv_protocolli_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Retrieve the table from the session object.
            DataTable dt = Session["TaskTable"] as DataTable;

            if (dt != null)
            {
                //Sort the data.
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gv_protocolli.DataSource = Session["TaskTable"];
                gv_protocolli.DataBind();
            }
        }
        private string GetSortDirection(string column)
        {
            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";

            // Retrieve the last column that was sorted.
            string sortExpression = ViewState["SortExpression"] as string;

            if (sortExpression != null)
            {
                // Check if the same column is being sorted.
                // Otherwise, the default value can be returned.
                if (sortExpression == column)
                {
                    string lastDirection = ViewState["SortDirection"] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }

            // Save new values in ViewState.
            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = column;

            return sortDirection;
        }

        protected void BtnPulisciCampiRicerca_Click(object sender, EventArgs e)
        {
            tbNumeroProtocollo.Text = string.Empty;
            tbCodiceLavoro.Text = string.Empty;
            tbRagioneSociale.Text = string.Empty;
            tbProduzione.Text = string.Empty;
            tbLavorazione.Text = string.Empty;
            tbDescrizione.Text = string.Empty;
            ddlTipoProtocollo.SelectedIndex = 0;
            tbProtocolloRiferimento.Text = string.Empty;
            ddlDestinatario.SelectedIndex = 0;
            tbDataProtocollo.Text = string.Empty;
            tbDataProtocolloA.Text = string.Empty;
            tbDataLavorazione.Text = string.Empty;
            tbDataLavorazioneA.Text = string.Empty;

            gv_protocolli.DataSource = null;
            gv_protocolli.DataBind();
            tbTotElementiGriglia.Text = "";
            SessionManager.CercaLavorazione_Risultati = null;
            SessionManager.CercaLavorazione_NumPagina = 0;
            SessionManager.CercaLavorazione_Filtri = null;
        }

        private void PopolaFiltri_Risultati()
        {
            #region FILTRI
            FiltriCercaLavorazione filtri = SessionManager.CercaLavorazione_Filtri;

            if (filtri != null)
            {
                tbRagioneSociale.Text = filtri.ClienteFornitore.Trim();
                tbNumeroProtocollo.Text = filtri.NumeroProtocollo.Trim();
                tbProtocolloRiferimento.Text = filtri.RiferimentoDocumento.Trim();
                tbDataLavorazione.Text = filtri.DataLavorazione_Da.Trim();
                tbDataLavorazioneA.Text = filtri.DataLavorazione_A.Trim();

                tbProduzione.Text = filtri.Produzione.Trim();
                tbLavorazione.Text = filtri.Lavorazione.Trim();
                tbDescrizione.Text = filtri.Descrizione.Trim();
                tbDataProtocollo.Text = filtri.DataProtocollo_Da.Trim();
                tbDataProtocolloA.Text = filtri.DataProtocollo_A.Trim();

                tbCodiceLavoro.Text = filtri.CodiceLavoro.Trim();
                ddlDestinatario.SelectedValue = filtri.Destinatario;
                ddlTipoProtocollo.SelectedValue = filtri.Tipo;
            }
            #endregion

            #region RISULTATI
            gv_protocolli.DataSource = SessionManager.CercaLavorazione_Risultati;
            gv_protocolli.PageIndex = SessionManager.CercaLavorazione_NumPagina;
            gv_protocolli.DataBind();
            tbTotElementiGriglia.Text = SessionManager.CercaLavorazione_Risultati.Rows.Count.ToString("###,##0");
            #endregion
        }
    }
}