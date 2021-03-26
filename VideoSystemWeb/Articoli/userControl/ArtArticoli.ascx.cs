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

namespace VideoSystemWeb.Articoli.userControl
{
    public partial class ArtArticoli : System.Web.UI.UserControl
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        BasePage basePage = new BasePage();
        public string dettaglioModifica = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            // FUNZIONA SE NELLA PAGINA ASPX CHIAMANTE C'E' UN CAMPO HIDDENFIELD COL TIPO ARTICOLO (GENERI/GRUPPI/SOTTOGRUPPI/ARTICOLI)
            HiddenField tipoArticolo = this.Parent.FindControl("HF_TIPO_ARTICOLO") as HiddenField;
            if (tipoArticolo.Value.ToUpper().Equals("ARTICOLI")) {
                Esito esito = new Esito();

                if (!Page.IsPostBack)
                {
                    lblIntestazionePagina.Text = "ARTICOLI";

                    //BasePage p = new BasePage();
                    //Esito esito = p.CaricaListeTipologiche();

                    // CARICO LE COMBO
                    //if (string.IsNullOrEmpty(esito.descrizione))
                    //{
                        // GENERI
                        cmbMod_Genere.Items.Clear();
                        cmbMod_Genere.Items.Add("");
                        ddlGenere.Items.Clear();
                        ddlGenere.Items.Add("");
                        foreach (Tipologica tipologiaGenere in SessionManager.ListaTipiGeneri)
                        {
                            ListItem item = new ListItem();
                            item.Text = tipologiaGenere.nome;
                            item.Value = tipologiaGenere.id.ToString();
                            cmbMod_Genere.Items.Add(item);
                            ddlGenere.Items.Add(item);
                        }
                        //GRUPPI
                        cmbMod_Gruppo.Items.Clear();
                        cmbMod_Gruppo.Items.Add("");
                        ddlGruppo.Items.Clear();
                        ddlGruppo.Items.Add("");
                        foreach (Tipologica tipologiaGruppo in SessionManager.ListaTipiGruppi)
                        {
                            ListItem item = new ListItem();
                            item.Text = tipologiaGruppo.nome;
                            item.Value = tipologiaGruppo.id.ToString();
                            cmbMod_Gruppo.Items.Add(item);
                            ddlGruppo.Items.Add(item);
                        }
                        //SOTTOGRUPPI
                        cmbMod_Sottogruppo.Items.Clear();
                        cmbMod_Sottogruppo.Items.Add("");
                        ddlSottoGruppo.Items.Clear();
                        ddlSottoGruppo.Items.Add("");
                        foreach (Tipologica tipologiaSottogruppo in SessionManager.ListaTipiSottogruppi)
                        {
                            ListItem item = new ListItem();
                            item.Text = tipologiaSottogruppo.nome;
                            item.Value = tipologiaSottogruppo.id.ToString();
                            cmbMod_Sottogruppo.Items.Add(item);
                            ddlSottoGruppo.Items.Add(item);
                        }

                        //GRUPPI ARTICOLI
                        ddlGruppiDaAggiungere.Items.Clear();
                        List<Art_Gruppi> listaGruppiMain = Art_Gruppi_BLL.Instance.CaricaListaGruppi(ref esito, true);
                        foreach (Art_Gruppi gruppoMain in listaGruppiMain)
                        {
                            ListItem item = new ListItem();
                            string stringaVisualizzata = gruppoMain.Nome.Trim().PadRight(50);
                            if (!string.IsNullOrEmpty(gruppoMain.Descrizione)) stringaVisualizzata += " | " + gruppoMain.Descrizione.Trim();
                            item.Text = stringaVisualizzata;
                            item.Value = gruppoMain.Id.ToString();
                            ddlGruppiDaAggiungere.Items.Add(item);
                        }

                        // SE UTENTE ABILITATO ALLE MODIFICHE FACCIO VEDERE I PULSANTI DI MODIFICA
                        abilitaBottoni(basePage.AbilitazioneInScrittura());

                    //}
                    //else
                    //{
                    //    Session["ErrorPageText"] = esito.descrizione;
                    //    string url = String.Format("~/pageError.aspx");
                    //    Response.Redirect(url, true);
                    //}

                }
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriTabGiusta", script: "openDettaglioArticolo('" + hf_tabChiamata.Value + "');", addScriptTags: true);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "chiudiLoader", script: "$('.loader').hide();", addScriptTags: true);
            }
        }

        private void abilitaBottoni(bool utenteAbilitatoInScrittura)
        {
            if (!utenteAbilitatoInScrittura)
            {
                divBtnInserisciArticoli.Visible = false;
                btnModifica.Visible = false;
                btnAnnulla.Visible = false;
                btnSalva.Visible = false;
                btnElimina.Visible = false;
                btnApriGruppi.Visible = false;
            }
            else
            {
                divBtnInserisciArticoli.Visible = true;
                btnModifica.Visible = true;
                btnAnnulla.Visible = true;
                btnSalva.Visible = true;
                btnElimina.Visible = true;
                btnApriGruppi.Visible = true;
            }
        }

        protected void EditArticolo_Click(object sendere, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hf_idArticolo.Value) || (!string.IsNullOrEmpty((string)ViewState["idAzienda"])))
            {
                if (!string.IsNullOrEmpty(hf_idArticolo.Value)) ViewState["idArticolo"] = hf_idArticolo.Value;
                editArticolo();
                AttivaDisattivaModificaArticolo(true);
                gestisciPulsantiArticolo("VISUALIZZAZIONE");
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriTabGiusta", script: "openDettaglioArticolo('Articolo');", addScriptTags: true);
                pnlContainer.Visible = true;
            }
        }

        private void editArticolo()
        {
            string idArticolo = (string)ViewState["idArticolo"];
            Esito esito = new Esito();

            if (!string.IsNullOrEmpty(idArticolo))
            {
                Entity.Art_Articoli articolo = Art_Articoli_BLL.Instance.getArticoloById(Convert.ToInt16(idArticolo), ref esito);
                if (esito.Codice == 0)
                {
                    pulisciCampiDettaglio();

                    dettaglioModifica = articolo.DefaultDescrizione.Trim();
                    lblDettaglioModifica.Text = dettaglioModifica;
                    // RIEMPIO I CAMPI DEL DETTAGLIO COLLABORATORE
                    tbMod_Descrizione.Text = articolo.DefaultDescrizioneLunga;
                    tbMod_DescrizioneBreve.Text = articolo.DefaultDescrizione;
                    tbMod_Prezzo.Text = articolo.DefaultPrezzo.ToString();
                    tbMod_Costo.Text = articolo.DefaultCosto.ToString();
                    //tbMod_IVA.Text = articolo.DefaultIva.ToString();
                    tbMod_Note.Text = articolo.Note;


                    //cbMod_Attivo.Checked = articolo.Attivo;
                    cbMod_Stampa.Checked = articolo.DefaultStampa;

                    //GENERE
                    if (articolo.DefaultTipoGenere != null)
                    {
                        ListItem trovati = cmbMod_Genere.Items.FindByText(articolo.DefaultTipoGenere.nome.ToString());
                        if (trovati != null)
                        {
                            cmbMod_Genere.SelectedValue = trovati.Value;
                        }
                        else
                        {
                            cmbMod_Genere.Text = "";
                        }
                    }
                    else
                    {
                        cmbMod_Genere.Text = "";
                    }

                    //GRUPPO
                    if (articolo.DefaultTipoGruppo != null) { 
                        ListItem trovati = cmbMod_Gruppo.Items.FindByText(articolo.DefaultTipoGruppo.nome.ToString());
                        if (trovati != null)
                        {
                            cmbMod_Gruppo.SelectedValue = trovati.Value;
                        }
                        else
                        {
                            cmbMod_Gruppo.Text = "";
                        }
                    }
                    else
                    {
                        cmbMod_Gruppo.Text = "";
                    }

                    //SOTTOGRUPPO
                    if (articolo.DefaultTipoSottogruppo != null)
                    {
                        ListItem trovati = cmbMod_Sottogruppo.Items.FindByText(articolo.DefaultTipoSottogruppo.nome.ToString());
                        if (trovati != null)
                        {
                            cmbMod_Sottogruppo.SelectedValue = trovati.Value;
                        }
                        else
                        {
                            cmbMod_Sottogruppo.Text = "";
                        }
                    }
                    else
                    {
                        cmbMod_Sottogruppo.Text = "";
                    }

                    // GRUPPI
                    gvMod_Gruppi.DataSource = null;
                    esito = new Esito();
                    DataTable dtGruppi = Base_DAL.GetDatiBySql("SELECT gruppi.id,nome,descrizione FROM art_gruppi_articoli artgruppi " +
                    "join art_articoli articoli " +
                    "on artgruppi.idArtArticoli = articoli.id " +
                    "join art_gruppi gruppi " +
                    "on idArtGruppi = gruppi.id " +
                    "where idArtArticoli = " + articolo.Id.ToString(), ref esito);

                    if (esito.Codice == 0)
                    {
                        gvMod_Gruppi.DataSource = dtGruppi;
                        gvMod_Gruppi.DataBind();
                    }
                    else
                    {
                        Session["ErrorPageText"] = esito.Descrizione;
                        string url = String.Format("~/pageError.aspx");
                        Response.Redirect(url, true);
                    }

                }
                else
                {
                    Session["ErrorPageText"] = esito.Descrizione;
                    string url = String.Format("~/pageError.aspx");
                    Response.Redirect(url, true);
                }
            }

        }

        private void pulisciCampiDettaglio()
        {
            tbMod_Descrizione.Text = "";
            tbMod_DescrizioneBreve.Text = "";
            tbMod_Prezzo.Text = "";
            tbMod_Costo.Text = "";
            //tbMod_IVA.Text = "";
            tbMod_Note.Text = "";

            cmbMod_Genere.Text = "";
            cmbMod_Gruppo.Text = "";
            cmbMod_Sottogruppo.Text = "";

            cbMod_Stampa.Checked = false;
        }

        protected void InserisciArticoli_Click(object sendere, EventArgs e)
        {
            ViewState["idArticolo"] = "";
            editArticoloVuoto();
            AttivaDisattivaModificaArticolo(false);
            gestisciPulsantiArticolo("INSERIMENTO");

            // SVUOTO IL DATAGRID DEI GRUPPI
            DataTable dt = new DataTable();
            gvMod_Gruppi.DataSource = dt;
            gvMod_Gruppi.DataBind();
            
            // PULISCO I PH DI MODIFICA

            phGruppi.Visible = false;

            pnlContainer.Visible = true;

        }

        private void editArticoloVuoto()
        {
            Esito esito = new Esito();
            dettaglioModifica = "";
            lblDettaglioModifica.Text = dettaglioModifica;
            pulisciCampiDettaglio();
        }
        protected void btnRicercaArticoli_Click(object sender, EventArgs e)
        {
            NascondiErroriValidazione();

            string queryRicerca = ConfigurationManager.AppSettings["QUERY_SEARCH_ARTICOLI"];

            queryRicerca = queryRicerca.Replace("@defaultDescrizioneLunga", tbDescrizione.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@defaultDescrizione", tbDescrizioneBreve.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@defaultPrezzo", tbPrezzo.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@defaultCosto", TbCosto.Text.Trim().Replace("'", "''"));

            queryRicerca = queryRicerca.Replace("@genere", ddlGenere.SelectedItem.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@gruppo", ddlGruppo.SelectedItem.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@sottoGruppo", ddlSottoGruppo.SelectedItem.Text.Trim().Replace("'", "''"));

            //queryRicerca = queryRicerca.Replace("@defaultIva", tbIva.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@defaultIva", "");

            Esito esito = new Esito();
            DataTable dtArticoli = Base_DAL.GetDatiBySql(queryRicerca, ref esito);
            gv_articoli.DataSource = dtArticoli;
            gv_articoli.DataBind();
            tbTotElementiGriglia.Text = dtArticoli.Rows.Count.ToString("###,##0");
        }

        protected void gv_articoli_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // PRENDO L'ID DELL'AZIENDA SELEZIONATA

                string idArticoloSelezionato = e.Row.Cells[0].Text;

                foreach (TableCell item in e.Row.Cells)
                {
                    item.Attributes["onclick"] = "mostraArticolo('" + idArticoloSelezionato + "');";
                }
            }

        }

        protected void gv_articoli_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_articoli.PageIndex = e.NewPageIndex;
            btnRicercaArticoli_Click(null, null);
        }

        protected void btnModifica_Click(object sender, EventArgs e)
        {
            AttivaDisattivaModificaArticolo(false);
            gestisciPulsantiArticolo("MODIFICA");
        }

        private void AttivaDisattivaModificaArticolo(bool attivaModifica)
        {
            tbMod_Descrizione.ReadOnly = attivaModifica;
            tbMod_DescrizioneBreve.ReadOnly = attivaModifica;
            tbMod_Costo.ReadOnly = attivaModifica;
            //tbMod_IVA.ReadOnly = attivaModifica;
            tbMod_Note.ReadOnly = attivaModifica;
            tbMod_Prezzo.ReadOnly = attivaModifica;

            //cbMod_Attivo.Enabled = !attivaModifica;
            cbMod_Stampa.Enabled = !attivaModifica;

            if (attivaModifica)
            {
                cmbMod_Gruppo.Attributes.Add("disabled", "");
                cmbMod_Genere.Attributes.Add("disabled", "");
                cmbMod_Sottogruppo.Attributes.Add("disabled", "");
            }
            else
            {
                cmbMod_Gruppo.Attributes.Remove("disabled");
                cmbMod_Genere.Attributes.Remove("disabled");
                cmbMod_Sottogruppo.Attributes.Remove("disabled");
            }

        }

        private void gestisciPulsantiArticolo(string stato)
        {
            switch (stato)
            {
                case "VISUALIZZAZIONE":

                    btnModifica.Visible = basePage.AbilitazioneInScrittura();
                    btnSalva.Visible = false;
                    btnAnnulla.Visible = false;
                    btnElimina.Visible = false;
                    btnConfermaInserimento.Visible = false;

                    if (basePage.AbilitazioneInScrittura())
                    {
                        phGruppi.Visible = false;
                    }
                    break;
                case "INSERIMENTO":
                    btnModifica.Visible = false;
                    btnSalva.Visible = false;
                    btnAnnulla.Visible = false;
                    btnElimina.Visible = false;
                    btnConfermaInserimento.Visible = true;
                    break;
                case "MODIFICA":
                    btnModifica.Visible = false;
                    btnSalva.Visible = true;
                    btnAnnulla.Visible = true;
                    btnElimina.Visible = true;
                    btnConfermaInserimento.Visible = false;
                    break;
                case "ANNULLAMENTO":
                    btnModifica.Visible = true;
                    btnSalva.Visible = false;
                    btnAnnulla.Visible = false;
                    btnElimina.Visible = false;
                    btnConfermaInserimento.Visible = false;
                    break;
                default:
                    btnModifica.Visible = true;
                    btnSalva.Visible = false;
                    btnAnnulla.Visible = false;
                    btnElimina.Visible = false;
                    btnConfermaInserimento.Visible = false;
                    break;
            }

        }

        protected void btnElimina_Click(object sender, EventArgs e)
        {
            Esito esito = new Esito();

            if (!string.IsNullOrEmpty((string)ViewState["idArticolo"]))
            {
                esito = Art_Articoli_BLL.Instance.EliminaArticolo(Convert.ToInt32(ViewState["idArticolo"].ToString()), ((Anag_Utenti)Session[SessionManager.UTENTE]));
                //esito = Art_Articoli_BLL.Instance.RemoveArticolo(Convert.ToInt32(ViewState["idArticolo"].ToString()));
                if (esito.Codice != Esito.ESITO_OK)
                {
                    //panelErrore.Style.Remove("display");
                    //panelErrore.Style.Add("display","block");
                    //lbl_MessaggioErrore.Text = esito.descrizione;
                    basePage.ShowError(esito.Descrizione);
                    AttivaDisattivaModificaArticolo(true);
                }
                else
                {
                    AttivaDisattivaModificaArticolo(true);
                    //btn_chiudi_Click(null, null);
                    pnlContainer.Visible = false;
                    btnRicercaArticoli_Click(null, null);
                }

            }

        }

        protected void btnSalva_Click(object sender, EventArgs e)
        {
            // SALVO MODIFICHE ARTICOLO
            Esito esito = new Esito();
            Art_Articoli articolo = CreaOggettoSalvataggio(ref esito);

            if (esito.Codice != Esito.ESITO_OK)
            {
                //panelErrore.Style.Remove("display");
                //lbl_MessaggioErrore.Text = "Controllare i campi evidenziati";
                basePage.ShowWarning("Controllare i campi evidenziati");
            }
            else
            {
                NascondiErroriValidazione();

                esito = Art_Articoli_BLL.Instance.AggiornaArticolo(articolo, ((Anag_Utenti)Session[SessionManager.UTENTE]));


                if (esito.Codice != Esito.ESITO_OK)
                {
                    //panelErrore.Style.Remove("display");
                    //lbl_MessaggioErrore.Text = esito.descrizione;
                    basePage.ShowError(esito.Descrizione);
                }
                EditArticolo_Click(null, null);
            }

        }
        private Art_Articoli CreaOggettoSalvataggio(ref Esito esito)
        {
            Art_Articoli articolo = new Art_Articoli();
            try
            {

                if (string.IsNullOrEmpty((string)ViewState["idArticolo"]))
                {
                    ViewState["idArticolo"] = 0;
                }

                articolo.Id = Convert.ToInt16(ViewState["idArticolo"].ToString());

                //articolo.Attivo = Convert.ToBoolean(BasePage.ValidaCampo(cbMod_Attivo, "true", false, ref esito));
                articolo.Attivo = true;
                articolo.DefaultStampa = Convert.ToBoolean(BasePage.ValidaCampo(cbMod_Stampa, "true", false, ref esito));
                articolo.DefaultDescrizione = BasePage.ValidaCampo(tbMod_DescrizioneBreve, "", false, ref esito);
                articolo.DefaultDescrizioneLunga = BasePage.ValidaCampo(tbMod_Descrizione, "", false, ref esito);
                articolo.DefaultIdTipoGenere = Convert.ToInt16(cmbMod_Genere.SelectedValue);
                articolo.DefaultIdTipoGruppo = Convert.ToInt16(cmbMod_Gruppo.SelectedValue);
                articolo.DefaultIdTipoSottogruppo = Convert.ToInt16(cmbMod_Sottogruppo.SelectedValue);
                //articolo.DefaultIva = Convert.ToInt16(BasePage.ValidaCampo(tbMod_IVA, "0", false, ref esito));
                articolo.DefaultIva = 0;
                articolo.DefaultPrezzo = Convert.ToDecimal(BasePage.ValidaCampo(tbMod_Prezzo, "0", false, ref esito));
                articolo.DefaultCosto = Convert.ToDecimal(BasePage.ValidaCampo(tbMod_Costo, "0", false, ref esito));
                articolo.Note = BasePage.ValidaCampo(tbMod_Note, "", false, ref esito);

                //azienda.TipoIndirizzoLegale = cmbMod_TipoIndirizzoLegale.SelectedValue;

                return articolo;

            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.Descrizione = ex.Message + Environment.NewLine + ex.StackTrace;
                
                return articolo;
            }
        }

        protected void btnConfermaInserimento_Click(object sender, EventArgs e)
        {
            // INSERISCO ARTICOLO
            Esito esito = new Esito();
            Art_Articoli articolo = CreaOggettoSalvataggio(ref esito);

            if (esito.Codice != Esito.ESITO_OK)
            {
                //panelErrore.Style.Remove("display");
                //lbl_MessaggioErrore.Text = esito.descrizione;
                basePage.ShowError(esito.Descrizione);
            }
            else
            {
                NascondiErroriValidazione();

                int iRet = Art_Articoli_BLL.Instance.CreaArticolo(articolo, ((Anag_Utenti)Session[SessionManager.UTENTE]), ref esito);
                if (iRet > 0)
                {
                    // UNA VOLTA INSERITO CORRETTAMENTE PUO' ESSERE MODIFICATO
                    hf_idArticolo.Value = iRet.ToString();
                    ViewState["idArticolo"] = hf_idArticolo.Value;
                    hf_tipoOperazione.Value = "VISUALIZZAZIONE";
                }

                if (esito.Codice != Esito.ESITO_OK)
                {
                    //panelErrore.Style.Remove("display");
                    //panelErrore.Style.Add("display","block");
                    //lbl_MessaggioErrore.Text = esito.descrizione;
                    basePage.ShowError(esito.Descrizione);
                }
                else
                {
                    EditArticolo_Click(null, null);
                }
                
            }

        }
        protected void btnAnnulla_Click(object sender, EventArgs e)
        {
            annullaModifiche();
        }
        private void annullaModifiche()
        {
            NascondiErroriValidazione();
            AttivaDisattivaModificaArticolo(true);
            editArticolo();
            gestisciPulsantiArticolo("ANNULLAMENTO");
        }
        private void NascondiErroriValidazione()
        {
            //panelErrore.Style.Add("display", "none");

            tbMod_Descrizione.CssClass = tbMod_Descrizione.CssClass.Replace("erroreValidazione", "");
            tbMod_DescrizioneBreve.CssClass = tbMod_DescrizioneBreve.CssClass.Replace("erroreValidazione", "");
            //tbMod_IVA.CssClass = tbMod_IVA.CssClass.Replace("erroreValidazione", "");
            tbMod_Costo.CssClass = tbMod_Costo.CssClass.Replace("erroreValidazione", "");
            tbMod_Prezzo.CssClass = tbMod_Prezzo.CssClass.Replace("erroreValidazione", "");
            tbMod_Note.CssClass = tbMod_Note.CssClass.Replace("erroreValidazione", "");
        }
        protected void btnApriGruppi_Click(object sender, EventArgs e)
        {
            if (phGruppi.Visible)
            {
                phGruppi.Visible = false;
            }
            else
            {
                phGruppi.Visible = true;
            }
        }
        protected void btnConfermaInserimentoGruppo_Click(object sender, EventArgs e)
        {
            //INSERISCO IL GRUPPO ARTICOLO SE SELEZIONATO
            if (ddlGruppiDaAggiungere.SelectedIndex >= 0)
            {
                Esito esito = new Esito();
                try
                {
                    NascondiErroriValidazione();
                    ListItem item = ddlGruppiDaAggiungere.Items[ddlGruppiDaAggiungere.SelectedIndex];
                    string value = item.Value;
                    string gruppoSelezionato = item.Text;

                    Art_Gruppi_Articoli nuovoGruppoArticolo = new Art_Gruppi_Articoli();

                    nuovoGruppoArticolo.IdArtGruppi = Convert.ToInt16(item.Value.Trim());

                    nuovoGruppoArticolo.IdArtArticoli = Convert.ToInt32(ViewState["idArticolo"]);

                    int iNuovoArtGruppo = Art_Gruppi_Articoli_BLL.Instance.CreaGruppoArticolo(nuovoGruppoArticolo, ((Anag_Utenti)Session[SessionManager.UTENTE]), ref esito);

                    if (esito.Codice != Esito.ESITO_OK)
                    {
                        basePage.ShowError(esito.Descrizione);
                    }
                    else
                    {
                        editArticolo();
                    }
                }
                catch (Exception ex)
                {
                    log.Error("btnConfermaInserimentoGruppo_Click", ex);
                    if (esito.Codice == Esito.ESITO_OK)
                    {
                        esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                        esito.Descrizione = ex.Message + Environment.NewLine + ex.StackTrace;
                    }
                    basePage.ShowError(ex.Message);
                }
            }
            else
            {
                basePage.ShowError("Verificare il corretto inserimento dei campi");
            }

        }
        protected void btnEliminaGruppo_Click(object sender, EventArgs e)
        {
            //ELIMINO IL GRUPPO ARTICOLO SE SELEZIONATO
            //if (lbMod_Gruppi.SelectedIndex >= 0)
            if (gvMod_Gruppi.SelectedRow != null && gvMod_Gruppi.SelectedRow.RowIndex >= 0)
            {
                Esito esito = new Esito();
                try
                {
                    NascondiErroriValidazione();

                    string value = gvMod_Gruppi.SelectedRow.Cells[0].Text.Trim();
                    // DEVO TROVARE PRIMA IL GRUPPO ARTICOLO FORMATO DA ID GRUPPO E ID ARTICOLO

                    string query = "SELECT id FROM art_gruppi_articoli where idArtGruppi = " + value + " AND idArtArticoli = " + ViewState["idArticolo"].ToString();
                    DataTable dtGruppiArticoli = Base_DAL.GetDatiBySql(query, ref esito);

                    if (dtGruppiArticoli == null || dtGruppiArticoli.Rows == null)
                    {
                        esito.Codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                        esito.Descrizione = "btnEliminaGruppo_Click - Nessun risultato restituito dalla query " + query;
                    }

                    if (esito.Codice != Esito.ESITO_OK )
                    {
                        log.Error(esito.Descrizione);
                        basePage.ShowError(esito.Descrizione);
                    }
                    else
                    {
                        foreach (DataRow riga in dtGruppiArticoli.Rows)
                        {
                            int idGruppoArticolo = Convert.ToInt16(riga["id"]);
                            esito = Art_Gruppi_Articoli_BLL.Instance.EliminaGruppoArticolo(idGruppoArticolo, ((Anag_Utenti)Session[SessionManager.UTENTE]));
                        }
                        if (esito.Codice != Esito.ESITO_OK)
                        {
                            log.Error(esito.Descrizione);
                            basePage.ShowError(esito.Descrizione);
                        }
                        else
                        {
                            editArticolo();
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    log.Error("btnEliminaGruppo_Click", ex);
                    if (esito.Codice == Esito.ESITO_OK)
                    {
                        esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                        esito.Descrizione = ex.Message + Environment.NewLine + ex.StackTrace;
                    }
                    basePage.ShowError(ex.Message);
                }
            }
            else
            {
                basePage.ShowError("Verificare il corretto inserimento dei campi");
            }

        }

        protected void gvMod_Gruppi_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gvMod_Gruppi_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void imgElimina_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "EliminaGruppo"  && basePage.AbilitazioneInScrittura())
            {
                string value = e.CommandArgument.ToString();
                Esito esito = new Esito();
                try
                {
                    NascondiErroriValidazione();
                    // DEVO TROVARE PRIMA IL GRUPPO ARTICOLO FORMATO DA ID GRUPPO E ID ARTICOLO

                    string query = "SELECT id FROM art_gruppi_articoli where idArtGruppi = " + value + " AND idArtArticoli = " + ViewState["idArticolo"].ToString();
                    DataTable dtGruppiArticoli = Base_DAL.GetDatiBySql(query, ref esito);

                    if (dtGruppiArticoli == null || dtGruppiArticoli.Rows == null)
                    {
                        esito.Codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                        esito.Descrizione = "imgElimina_Command - Nessun risultato restituito dalla query " + query;
                    }

                    if (esito.Codice != Esito.ESITO_OK)
                    {
                        log.Error(esito.Descrizione);
                        basePage.ShowError(esito.Descrizione);
                    }
                    else
                    {
                        foreach (DataRow riga in dtGruppiArticoli.Rows)
                        {
                            int idGruppoArticolo = Convert.ToInt16(riga["id"]);
                            esito = Art_Gruppi_Articoli_BLL.Instance.EliminaGruppoArticolo(idGruppoArticolo, ((Anag_Utenti)Session[SessionManager.UTENTE]));
                        }
                        if (esito.Codice != Esito.ESITO_OK)
                        {
                            log.Error(esito.Descrizione);
                            basePage.ShowError(esito.Descrizione);
                        }
                        else
                        {
                            editArticolo();
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("imgElimina_Command", ex);
                    if (esito.Codice == Esito.ESITO_OK)
                    {
                        esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                        esito.Descrizione = ex.Message + Environment.NewLine + ex.StackTrace;
                    }
                    basePage.ShowError(ex.Message);
                }
            }

        }
    }
}