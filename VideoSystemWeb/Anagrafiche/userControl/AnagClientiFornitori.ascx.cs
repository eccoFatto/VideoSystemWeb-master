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

namespace VideoSystemWeb.Anagrafiche.userControl
{
    public partial class AnagClientiFornitori : System.Web.UI.UserControl
    {
        BasePage basePage = new BasePage();
        public string dettaglioModifica = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                // GLI ASCX POSSONO LEGGERE LA QUERYSTRING DELLA PAGINA CONTENITRICE
                //if (!string.IsNullOrEmpty(Request.QueryString["TIPO"]))
                //{
                //    ViewState["TIPO_AZIENDA"] = Request.QueryString["TIPO"];
                //}

                // FUNZIONA SE NELLA PAGINA ASPX CHIAMANTE C'E' UN CAMPO HIDDENFIELD COL TIPO AZIENDA (CLIENTI/FORNITORI)
                HiddenField tipoAzienda = this.Parent.FindControl("HF_TIPO_AZIENDA") as HiddenField;
                if (tipoAzienda != null) { 
                    ViewState["TIPO_AZIENDA"] = tipoAzienda.Value;
                }
                else
                {
                    ViewState["TIPO_AZIENDA"] = "CLIENTI";
                }
                lblTipoAzienda.Text = ViewState["TIPO_AZIENDA"].ToString();

                if (ViewState["TIPO_AZIENDA"].ToString().Equals("CLIENTI"))
                {
                    lblTipoAzienda.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    lblTipoAzienda.ForeColor = System.Drawing.Color.Green;
                }


                BasePage p = new BasePage();
                Esito esito = p.CaricaListeTipologiche();

                // CARICO LE COMBO
                if (string.IsNullOrEmpty(esito.descrizione))
                {
                    ddlTipoAzienda.Items.Clear();
                    ddlTipoAzienda.Items.Add("");
                    cmbMod_TipoAzienda.Items.Clear();
                    cmbMod_TipoAzienda.Items.Add("");
                    foreach (Tipologica tipologiaAzienda in p.listaTipiClientiFornitori)
                    {
                        ListItem item = new ListItem();
                        item.Text = tipologiaAzienda.nome;
                        item.Value = tipologiaAzienda.nome;
                        ddlTipoAzienda.Items.Add(item);
                        cmbMod_TipoAzienda.Items.Add(item);
                    }

                    // SE UTENTE ABILITATO ALLE MODIFICHE FACCIO VEDERE I PULSANTI DI MODIFICA
                    abilitaBottoni(p.AbilitazioneInScrittura());


                }
                else
                {
                    Session["ErrorPageText"] = esito.descrizione;
                    string url = String.Format("~/pageError.aspx");
                    Response.Redirect(url, true);
                }

            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriTabGiusta", script: "openDettaglioAzienda('" + hf_tabChiamata.Value + "');", addScriptTags: true);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "chiudiLoader", script: "$('.loader').hide();", addScriptTags: true);
        }

        private void abilitaBottoni(bool utenteAbilitatoInScrittura)
        {
            if (!utenteAbilitatoInScrittura)
            {
                divBtnInserisciAzienda.Visible = false;
                btnModifica.Visible = false;
                btnAnnulla.Visible = false;
                btnSalva.Visible = false;
                btnElimina.Visible = false;
                btnApriReferenti.Visible = false;

            }
            else
            {
                divBtnInserisciAzienda.Visible = true;
                btnModifica.Visible = true;
                btnAnnulla.Visible = true;
                btnSalva.Visible = true;
                btnElimina.Visible = true;
                btnApriReferenti.Visible = true;
            }
        }

        protected void EditAzienda_Click(object sendere, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hf_idAzienda.Value) || (!string.IsNullOrEmpty((string)ViewState["idAzienda"])))
            {
                if (!string.IsNullOrEmpty(hf_idAzienda.Value)) ViewState["idAzienda"] = hf_idAzienda.Value;
                editAzienda();
                AttivaDisattivaModificaAzienda(true);
                gestisciPulsantiAzienda("VISUALIZZAZIONE");
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriTabGiusta", script: "openDettaglioAzienda('Azienda');", addScriptTags: true);
                pnlContainer.Visible = true;
            }
        }

        private void editAzienda()
        {
            string idAzienda = (string)ViewState["idAzienda"];
            Esito esito = new Esito();

            if (!string.IsNullOrEmpty(idAzienda))
            {
                Entity.Anag_Clienti_Fornitori azienda = Anag_Clienti_Fornitori_BLL.Instance.getAziendaById(Convert.ToInt16(idAzienda), ref esito);
                if (esito.codice == 0)
                {
                    pulisciCampiDettaglio();

                    dettaglioModifica = azienda.RagioneSociale;
                    // RIEMPIO I CAMPI DEL DETTAGLIO COLLABORATORE
                    tbMod_CF.Text = azienda.CodiceFiscale;
                    tbMod_PartitaIva.Text = azienda.PartitaIva;
                    tbMod_RagioneSociale.Text = azienda.RagioneSociale;
                    tbMod_CodiceIdentificativo.Text = azienda.CodiceIdentificativo;
                    tbMod_Iban.Text = azienda.Iban;
                    tbMod_Note.Text = azienda.Note;
                    tbMod_CapLegale.Text = azienda.CapLegale;
                    tbMod_CapOperativo.Text = azienda.CapOperativo;
                    tbMod_CivicoLegale.Text = azienda.NumeroCivicoLegale;
                    tbMod_CivicoOperativo.Text = azienda.NumeroCivicoOperativo;
                    tbMod_ComuneLegale.Text = azienda.ComuneLegale;
                    tbMod_ComuneOperativo.Text = azienda.ComuneOperativo;
                    tbMod_Email.Text = azienda.Email;
                    tbMod_Fax.Text = azienda.Fax;
                    tbMod_Iban.Text = azienda.Iban;
                    tbMod_IndirizzoLegale.Text = azienda.IndirizzoLegale;
                    tbMod_IndirizzoOperativo.Text = azienda.IndirizzoOperativo;
                    tbMod_NazioneLegale.Text = azienda.NazioneLegale;
                    tbMod_NazioneOperativo.Text = azienda.NazioneOperativo;
                    tbMod_Pec.Text = azienda.Pec;
                    tbMod_ProvinciaLegale.Text = azienda.ProvinciaLegale;
                    tbMod_ProvinciaOperativo.Text = azienda.ProvinciaOperativo;
                    tbMod_Telefono.Text = azienda.Telefono;
                    tbMod_WebSite.Text = azienda.WebSite;


                    //cbMod_Attivo.Checked = azienda.Attivo;
                    cbMod_Cliente.Checked = azienda.Cliente;
                    cbMod_Fornitore.Checked = azienda.Fornitore;

                    //TIPI PAGAMENTO
                    ListItem trovati = cmbMod_Pagamento.Items.FindByValue(azienda.Pagamento.ToString());
                    if (trovati != null)
                    {
                        cmbMod_Pagamento.SelectedValue = trovati.Value;
                    }
                    else
                    {
                        cmbMod_Pagamento.Text = "";
                    }

                    // TIPO AZIENDA
                    if (azienda.Tipo != null) { 
                        trovati = cmbMod_TipoAzienda.Items.FindByValue(azienda.Tipo.ToString());
                        if (trovati != null)
                        {
                            cmbMod_TipoAzienda.SelectedValue = trovati.Value;
                        }
                        else
                        {
                            cmbMod_TipoAzienda.Text = "";
                        }
                    }
                    else
                    {
                        cmbMod_TipoAzienda.Text = "";
                    }

                    // TIPO INDIRIZZO LEGALE
                    if (azienda.TipoIndirizzoLegale != null)
                    {
                        trovati = cmbMod_TipoIndirizzoLegale.Items.FindByValue(azienda.TipoIndirizzoLegale.ToString());
                        if (trovati != null)
                        {
                            cmbMod_TipoIndirizzoLegale.SelectedValue = trovati.Value;
                        }
                        else
                        {
                            cmbMod_TipoIndirizzoLegale.Text = "";
                        }
                    }
                    else
                    {
                        cmbMod_TipoIndirizzoLegale.Text = "";
                    }

                    // TIPO INDIRIZZO OPERATIVO
                    if (azienda.TipoIndirizzoOperativo != null)
                    {
                        trovati = cmbMod_TipoIndirizzoOperativo.Items.FindByValue(azienda.TipoIndirizzoOperativo.ToString());
                        if (trovati != null)
                        {
                            cmbMod_TipoIndirizzoOperativo.SelectedValue = trovati.Value;
                        }
                        else
                        {
                            cmbMod_TipoIndirizzoOperativo.Text = "";
                        }
                    }
                    else
                    {
                        cmbMod_TipoIndirizzoOperativo.Text = "";
                    }

                    // REFERENTI
                    DataTable dtReferenti = new DataTable();
                    if (azienda.Referenti != null)
                    {
                        dtReferenti.Columns.Add("id");
                        dtReferenti.Columns.Add("Cognome");
                        dtReferenti.Columns.Add("Nome");
                        dtReferenti.Columns.Add("Settore");
                        dtReferenti.Columns.Add("Telefono1");
                        dtReferenti.Columns.Add("Telefono2");
                        dtReferenti.Columns.Add("Cellulare");
                        dtReferenti.Columns.Add("Email");
                        foreach (Anag_Referente_Clienti_Fornitori referente in azienda.Referenti)
                        {
                            ListItem itemReferente = new ListItem(referente.Cognome + " " + referente.Nome + " - " + referente.Settore + " - " + referente.Telefono1, referente.Id.ToString());
                            lbMod_Referenti.Items.Add(itemReferente);
                            DataRow dr = dtReferenti.NewRow();
                            dr["id"] = referente.Id.ToString();
                            dr["Cognome"] = referente.Cognome;
                            dr["Nome"] = referente.Nome;
                            dr["Settore"] = referente.Settore;
                            dr["Telefono1"] = referente.Telefono1;
                            dr["Telefono2"] = referente.Telefono2;
                            dr["Cellulare"] = referente.Cellulare;
                            dr["Email"] = referente.Email;
                            dtReferenti.Rows.Add(dr);
                        }
                        gvMod_Referenti.DataSource = dtReferenti;
                        gvMod_Referenti.DataBind();
                    }
                    if (azienda.Referenti != null && azienda.Referenti.Count > 0)
                    {
                        lbMod_Referenti.Rows = azienda.Referenti.Count;
                    }
                    else
                    {
                        lbMod_Referenti.Rows = 1;
                        gvMod_Referenti.DataSource = null;
                    }

                }
                else
                {
                    Session["ErrorPageText"] = esito.descrizione;
                    string url = String.Format("~/pageError.aspx");
                    Response.Redirect(url, true);
                }
            }

        }

        private void pulisciCampiDettaglio()
        {
            tbMod_CF.Text = "";
            tbMod_CodiceIdentificativo.Text = "";
            tbMod_Iban.Text = "";
            tbMod_RagioneSociale.Text = "";
            tbMod_PartitaIva.Text = "";
            tbMod_Note.Text = "";
            tbMod_CapLegale.Text = "";
            tbMod_CapOperativo.Text = "";
            tbMod_CivicoLegale.Text = "";
            tbMod_CivicoOperativo.Text = "";
            tbMod_ComuneLegale.Text = "";
            tbMod_ComuneOperativo.Text = "";
            tbMod_Email.Text = "";
            tbMod_Fax.Text = "";
            tbMod_Iban.Text = "";
            tbMod_IndirizzoLegale.Text = "";
            tbMod_IndirizzoOperativo.Text = "";
            tbMod_NazioneLegale.Text = "";
            tbMod_NazioneOperativo.Text = "";
            tbMod_Pec.Text = "";
            tbMod_ProvinciaLegale.Text = "";
            tbMod_ProvinciaOperativo.Text = "";
            tbMod_Telefono.Text = "";
            tbMod_WebSite.Text = "";

            cmbMod_TipoIndirizzoLegale.Text = "";
            cmbMod_TipoIndirizzoOperativo.Text = "";
            cmbMod_Pagamento.Text = "";
            cmbMod_TipoAzienda.Text = "";

            cbMod_Cliente.Checked = false;
            cbMod_Fornitore.Checked = false;
            //cbMod_Attivo.Checked = false;
            tbMod_Note.Text = "";

            lbMod_Referenti.Items.Clear();
            lbMod_Referenti.Rows = 1;

            gvMod_Referenti.DataSource = null;

        }

        protected void InserisciAzienda_Click(object sendere, EventArgs e)
        {
            ViewState["idAzienda"] = "";
            editAziendaVuota();
            AttivaDisattivaModificaAzienda(false);
            gestisciPulsantiAzienda("INSERIMENTO");

            // PULISCO I PH DI MODIFICA
            btnAnnullaReferente_Click(null, null);

            phReferenti.Visible = false;

            pnlContainer.Visible = true;

        }

        private void editAziendaVuota()
        {
            Esito esito = new Esito();
            dettaglioModifica = "";
            pulisciCampiDettaglio();
        }
        protected void btnRicercaAziende_Click(object sender, EventArgs e)
        {
            string queryRicerca = ConfigurationManager.AppSettings["QUERY_SEARCH_AZIENDE"];

            string ClienteFornitore = "";
            //string fornitore = "";

            if (ViewState["TIPO_AZIENDA"].ToString() == "CLIENTI")
            {
                ClienteFornitore = " cliente = 1 ";
            }
            else
            {
                ClienteFornitore = " fornitore = 1 ";
            }

            queryRicerca = queryRicerca.Replace("@ClienteFornitore", ClienteFornitore);

            queryRicerca = queryRicerca.Replace("@cognome", TbReferente.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@codiceFiscale", tbCF.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@partitaIva", TbPiva.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@ragioneSociale", tbRagioneSociale.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@tipo", ddlTipoAzienda.SelectedValue.ToString().Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@codiceIdentificativo", tbCodiceIdentificativo.Text.Trim().Replace("'", "''"));

            Esito esito = new Esito();
            DataTable dtAziende = Base_DAL.getDatiBySql(queryRicerca, ref esito);
            gv_aziende.DataSource = dtAziende;
            gv_aziende.DataBind();

        }

        protected void gv_aziende_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // PRENDO L'ID DELL'AZIENDA SELEZIONATA

                string idAziendaSelezionata = e.Row.Cells[0].Text;

                foreach (TableCell item in e.Row.Cells)
                {
                    item.Attributes["onclick"] = "mostraAzienda('" + idAziendaSelezionata + "');";
                }
            }

        }

        protected void gv_aziende_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_aziende.PageIndex = e.NewPageIndex;
            btnRicercaAziende_Click(null, null);
        }

        protected void ddlTipoAzienda_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnModifica_Click(object sender, EventArgs e)
        {
            AttivaDisattivaModificaAzienda(false);
            gestisciPulsantiAzienda("MODIFICA");
        }

        private void AttivaDisattivaModificaAzienda(bool attivaModifica)
        {
            tbMod_CF.ReadOnly = attivaModifica;
            tbMod_Note.ReadOnly = attivaModifica;
            tbMod_PartitaIva.ReadOnly = attivaModifica;
            tbMod_RagioneSociale.ReadOnly = attivaModifica;
            tbMod_CodiceIdentificativo.ReadOnly = attivaModifica;
            tbMod_CapLegale.ReadOnly = attivaModifica;
            tbMod_CapOperativo.ReadOnly = attivaModifica;
            tbMod_CivicoLegale.ReadOnly = attivaModifica;
            tbMod_CivicoOperativo.ReadOnly = attivaModifica;
            tbMod_ComuneLegale.ReadOnly = attivaModifica;
            tbMod_ComuneOperativo.ReadOnly = attivaModifica;
            tbMod_Email.ReadOnly = attivaModifica;
            tbMod_Fax.ReadOnly = attivaModifica;
            tbMod_Iban.ReadOnly = attivaModifica;
            tbMod_IndirizzoLegale.ReadOnly = attivaModifica;
            tbMod_IndirizzoOperativo.ReadOnly = attivaModifica;
            tbMod_NazioneLegale.ReadOnly = attivaModifica;
            tbMod_NazioneOperativo.ReadOnly = attivaModifica;
            tbMod_Pec.ReadOnly = attivaModifica;
            tbMod_ProvinciaLegale.ReadOnly = attivaModifica;
            tbMod_ProvinciaOperativo.ReadOnly = attivaModifica;
            tbMod_Telefono.ReadOnly = attivaModifica;
            tbMod_WebSite.ReadOnly = attivaModifica;

            //cbMod_Attivo.Enabled = !attivaModifica;
            cbMod_Cliente.Enabled = !attivaModifica;
            cbMod_Fornitore.Enabled = !attivaModifica;

            if (attivaModifica)
            {
                cmbMod_TipoAzienda.Attributes.Add("disabled", "");
                cmbMod_Pagamento.Attributes.Add("disabled", "");
                cmbMod_TipoIndirizzoLegale.Attributes.Add("disabled", "");
                cmbMod_TipoIndirizzoOperativo.Attributes.Add("disabled", "");
            }
            else
            {
                cmbMod_TipoAzienda.Attributes.Remove("disabled");
                cmbMod_Pagamento.Attributes.Remove("disabled");
                cmbMod_TipoIndirizzoLegale.Attributes.Remove("disabled");
                cmbMod_TipoIndirizzoOperativo.Attributes.Remove("disabled");
            }

            //cmbMod_TipoAzienda.Enabled = !attivaModifica;
            //cmbMod_Pagamento.Enabled = !attivaModifica;
            //cmbMod_TipoIndirizzoLegale.Enabled = !attivaModifica;
            //cmbMod_TipoIndirizzoOperativo.Enabled = !attivaModifica;
        }

        private void gestisciPulsantiAzienda(string stato)
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
                        btnAnnullaReferente_Click(null, null);
                        phReferenti.Visible = false;
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

            if (!string.IsNullOrEmpty((string)ViewState["idAzienda"]))
            {
                esito = Anag_Clienti_Fornitori_BLL.Instance.EliminaAzienda(Convert.ToInt32(ViewState["idAzienda"].ToString()), ((Anag_Utenti)Session[SessionManager.UTENTE]));
                if (esito.codice != Esito.ESITO_OK)
                {
                    //panelErrore.Style.Remove("display");
                    //panelErrore.Style.Add("display","block");
                    //lbl_MessaggioErrore.Text = esito.descrizione;
                    basePage.ShowError(esito.descrizione);
                    AttivaDisattivaModificaAzienda(true);
                }
                else
                {
                    AttivaDisattivaModificaAzienda(true);
                    //btn_chiudi_Click(null, null);
                    pnlContainer.Visible = false;
                    btnRicercaAziende_Click(null, null);
                }

            }

        }

        protected void btnSalva_Click(object sender, EventArgs e)
        {
            // SALVO MODIFICHE AZIENDA
            Esito esito = new Esito();
            Anag_Clienti_Fornitori azienda = CreaOggettoSalvataggio(ref esito);

            if (esito.codice != Esito.ESITO_OK)
            {
                //panelErrore.Style.Remove("display");
                //lbl_MessaggioErrore.Text = "Controllare i campi evidenziati";
                basePage.ShowError("Controllare i campi evidenziati");
            }
            else
            {
                NascondiErroriValidazione();

                esito = Anag_Clienti_Fornitori_BLL.Instance.AggiornaAzienda(azienda, ((Anag_Utenti)Session[SessionManager.UTENTE]));


                if (esito.codice != Esito.ESITO_OK)
                {
                    //panelErrore.Style.Remove("display");
                    //lbl_MessaggioErrore.Text = esito.descrizione;
                    basePage.ShowError(esito.descrizione);
                }
                EditAzienda_Click(null, null);
            }

        }
        private Anag_Clienti_Fornitori CreaOggettoSalvataggio(ref Esito esito)
        {
            Anag_Clienti_Fornitori azienda = new Anag_Clienti_Fornitori();
            try
            {

                if (string.IsNullOrEmpty((string)ViewState["idAzienda"]))
                {
                    ViewState["idAzienda"] = 0;
                }

                azienda.Id = Convert.ToInt16(ViewState["idAzienda"].ToString());

                //azienda.Attivo = Convert.ToBoolean(BasePage.ValidaCampo(cbMod_Attivo, "true", false, ref esito));
                azienda.Attivo = true;
                azienda.Cliente = Convert.ToBoolean(BasePage.ValidaCampo(cbMod_Cliente, "true", false, ref esito));
                azienda.Fornitore = Convert.ToBoolean(BasePage.ValidaCampo(cbMod_Fornitore, "true", false, ref esito));

                azienda.RagioneSociale = BasePage.ValidaCampo(tbMod_RagioneSociale, "", false, ref esito);
                azienda.CapLegale = BasePage.ValidaCampo(tbMod_CapLegale, "", false, ref esito);
                azienda.CapOperativo = BasePage.ValidaCampo(tbMod_CapOperativo, "", false, ref esito);
                azienda.CodiceFiscale = BasePage.ValidaCampo(tbMod_CF, "", false, ref esito);
                azienda.CodiceIdentificativo = BasePage.ValidaCampo(tbMod_CodiceIdentificativo, "", false, ref esito);
                azienda.ComuneLegale = BasePage.ValidaCampo(tbMod_ComuneLegale, "", false, ref esito);
                azienda.ComuneOperativo = BasePage.ValidaCampo(tbMod_ComuneOperativo, "", false, ref esito);
                azienda.Email = BasePage.ValidaCampo(tbMod_Email, "", false, ref esito);
                azienda.Fax = BasePage.ValidaCampo(tbMod_Fax, "", false, ref esito);
                azienda.Iban = BasePage.ValidaCampo(tbMod_Iban, "", false, ref esito);
                azienda.IndirizzoLegale = BasePage.ValidaCampo(tbMod_IndirizzoLegale, "", false, ref esito);
                azienda.IndirizzoOperativo = BasePage.ValidaCampo(tbMod_IndirizzoOperativo, "", false, ref esito);
                azienda.NazioneLegale = BasePage.ValidaCampo(tbMod_NazioneLegale, "", false, ref esito);
                azienda.NazioneOperativo = BasePage.ValidaCampo(tbMod_NazioneOperativo, "", false, ref esito);
                azienda.Note = BasePage.ValidaCampo(tbMod_Note, "", false, ref esito);
                azienda.NumeroCivicoLegale = BasePage.ValidaCampo(tbMod_CivicoLegale, "", false, ref esito);
                azienda.NumeroCivicoOperativo = BasePage.ValidaCampo(tbMod_CivicoOperativo, "", false, ref esito);
                azienda.Pagamento = Convert.ToInt16(cmbMod_Pagamento.SelectedValue);
                azienda.PartitaIva = BasePage.ValidaCampo(tbMod_PartitaIva, "", false, ref esito);
                azienda.Pec = BasePage.ValidaCampo(tbMod_Pec, "", false, ref esito);
                azienda.ProvinciaLegale = BasePage.ValidaCampo(tbMod_ProvinciaLegale, "", false, ref esito);
                azienda.ProvinciaOperativo = BasePage.ValidaCampo(tbMod_ProvinciaOperativo, "", false, ref esito);
                azienda.Telefono = BasePage.ValidaCampo(tbMod_Telefono, "", false, ref esito);
                azienda.Tipo = cmbMod_TipoAzienda.SelectedValue;
                azienda.TipoIndirizzoLegale = cmbMod_TipoIndirizzoLegale.SelectedValue;
                azienda.TipoIndirizzoOperativo = cmbMod_TipoIndirizzoOperativo.SelectedValue;
                azienda.WebSite = BasePage.ValidaCampo(tbMod_WebSite, "", false, ref esito);

                return azienda;

            }
            catch (Exception ex)
            {
                esito.codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.descrizione = ex.Message;
                return azienda;
            }
        }

        protected void btnConfermaInserimento_Click(object sender, EventArgs e)
        {
            // INSERISCO AZIENDA
            Esito esito = new Esito();
            Anag_Clienti_Fornitori azienda = CreaOggettoSalvataggio(ref esito);

            if (esito.codice != Esito.ESITO_OK)
            {
                //panelErrore.Style.Remove("display");
                //lbl_MessaggioErrore.Text = esito.descrizione;
                basePage.ShowError(esito.descrizione);
            }
            else
            {
                NascondiErroriValidazione();

                int iRet = Anag_Clienti_Fornitori_BLL.Instance.CreaAzienda(azienda, ((Anag_Utenti)Session[SessionManager.UTENTE]), ref esito);
                if (iRet > 0)
                {
                    // UNA VOLTA INSERITO CORRETTAMENTE PUO' ESSERE MODIFICATO
                    hf_idAzienda.Value = iRet.ToString();
                    ViewState["idAzienda"] = hf_idAzienda.Value;
                    hf_tipoOperazione.Value = "VISUALIZZAZIONE";
                }

                if (esito.codice != Esito.ESITO_OK)
                {
                    //panelErrore.Style.Remove("display");
                    //panelErrore.Style.Add("display","block");
                    //lbl_MessaggioErrore.Text = esito.descrizione;
                    basePage.ShowError(esito.descrizione);
                }
                else
                {
                    EditAzienda_Click(null, null);
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
            AttivaDisattivaModificaAzienda(true);
            editAzienda();
            gestisciPulsantiAzienda("ANNULLAMENTO");
        }
        private void NascondiErroriValidazione()
        {
            tbMod_CF.CssClass = tbMod_CF.CssClass.Replace("erroreValidazione", "");
            tbMod_CodiceIdentificativo.CssClass = tbMod_CodiceIdentificativo.CssClass.Replace("erroreValidazione", "");
            tbMod_Iban.CssClass = tbMod_Iban.CssClass.Replace("erroreValidazione", "");
            tbMod_RagioneSociale.CssClass = tbMod_RagioneSociale.CssClass.Replace("erroreValidazione", "");
            tbMod_PartitaIva.CssClass = tbMod_PartitaIva.CssClass.Replace("erroreValidazione", "");
            tbMod_Note.CssClass = tbMod_Note.CssClass.Replace("erroreValidazione", "");
            tbMod_CapLegale.CssClass = tbMod_CapLegale.CssClass.Replace("erroreValidazione", "");
            tbMod_CapOperativo.CssClass = tbMod_CapOperativo.CssClass.Replace("erroreValidazione", "");
            tbMod_CivicoLegale.CssClass = tbMod_CivicoLegale.CssClass.Replace("erroreValidazione", "");
            tbMod_CivicoOperativo.CssClass = tbMod_CivicoOperativo.CssClass.Replace("erroreValidazione", "");
            tbMod_ComuneLegale.CssClass = tbMod_ComuneLegale.CssClass.Replace("erroreValidazione", "");
            tbMod_ComuneOperativo.CssClass = tbMod_ComuneOperativo.CssClass.Replace("erroreValidazione", "");
            tbMod_Email.CssClass = tbMod_Email.CssClass.Replace("erroreValidazione", "");
            tbMod_Fax.CssClass = tbMod_Fax.CssClass.Replace("erroreValidazione", "");
            tbMod_Iban.CssClass = tbMod_Iban.CssClass.Replace("erroreValidazione", "");
            tbMod_IndirizzoLegale.CssClass = tbMod_IndirizzoLegale.CssClass.Replace("erroreValidazione", "");
            tbMod_IndirizzoOperativo.CssClass = tbMod_IndirizzoOperativo.CssClass.Replace("erroreValidazione", "");
            tbMod_NazioneLegale.CssClass = tbMod_NazioneLegale.CssClass.Replace("erroreValidazione", "");
            tbMod_NazioneOperativo.CssClass = tbMod_NazioneOperativo.CssClass.Replace("erroreValidazione", "");
            tbMod_Pec.CssClass = tbMod_Pec.CssClass.Replace("erroreValidazione", "");
            tbMod_ProvinciaLegale.CssClass = tbMod_ProvinciaLegale.CssClass.Replace("erroreValidazione", "");
            tbMod_ProvinciaOperativo.CssClass = tbMod_ProvinciaOperativo.CssClass.Replace("erroreValidazione", "");
            tbMod_Telefono.CssClass = tbMod_Telefono.CssClass.Replace("erroreValidazione", "");
            tbMod_WebSite.CssClass = tbMod_WebSite.CssClass.Replace("erroreValidazione", "");
        }
        protected void btnApriReferenti_Click(object sender, EventArgs e)
        {
            if (phReferenti.Visible)
            {
                phReferenti.Visible = false;
            }
            else
            {
                phReferenti.Visible = true;
            }
        }
        protected void btnConfermaInserimentoReferente_Click(object sender, EventArgs e)
        {
            //INSERISCO IL REFERENTE
            if (!string.IsNullOrEmpty(tbInsCognomeReferente.Text) && (string.IsNullOrEmpty(tbInsEmailReferente.Text)|| basePage.validaIndirizzoEmail(tbInsEmailReferente.Text.Trim())))
            {
                try
                {
                    NascondiErroriValidazione();

                    Esito esito = new Esito();
                    Anag_Referente_Clienti_Fornitori nuovoReferente = new Anag_Referente_Clienti_Fornitori();
                    nuovoReferente.Id_azienda = Convert.ToInt32(ViewState["idAzienda"]);
                    nuovoReferente.Attivo = cbInsAttivoReferente.Checked;
                    nuovoReferente.Cognome = tbInsCognomeReferente.Text.Trim();
                    nuovoReferente.Nome = tbInsNomeReferente.Text.Trim();
                    nuovoReferente.Cellulare = tbInsCellulareReferente.Text.Trim();
                    nuovoReferente.Email = tbInsEmailReferente.Text.Trim();
                    nuovoReferente.Note = tbInsNoteReferente.Text.Trim();
                    nuovoReferente.Settore = tbInsSettoreReferente.Text.Trim();
                    nuovoReferente.Telefono1 = tbInsTelefono1Referente.Text.Trim();
                    nuovoReferente.Telefono2 = tbInsTelefono2Referente.Text.Trim();


                    int iNuovoReferente = Anag_Referente_Clienti_Fornitori_BLL.Instance.CreaReferente(nuovoReferente, ((Anag_Utenti)Session[SessionManager.UTENTE]), ref esito);

                    if (esito.codice != Esito.ESITO_OK)
                    {
                        //panelErrore.Style.Remove("display");
                        //lbl_MessaggioErrore.Text = esito.descrizione;
                        basePage.ShowError(esito.descrizione);
                    }
                    else
                    {
                        tbInsCognomeReferente.Text = "";
                        tbInsNomeReferente.Text = "";
                        tbInsCellulareReferente.Text = "";
                        tbInsEmailReferente.Text = "";
                        tbInsNoteReferente.Text = "";
                        tbInsSettoreReferente.Text = "";
                        tbInsTelefono1Referente.Text = "";
                        tbInsTelefono2Referente.Text = "";
                        cbInsAttivoReferente.Checked = true;
                        editAzienda();
                    }
                }
                catch (Exception ex)
                {
                    //panelErrore.Style.Remove("display");
                    //lbl_MessaggioErrore.Text = ex.Message;
                    basePage.ShowError(ex.Message);
                }
            }
            else
            {
                //panelErrore.Style.Remove("display");
                //lbl_MessaggioErrore.Text = "Verificare il corretto inserimento dei campi!";
                basePage.ShowError("Verificare il corretto inserimento dei campi");
            }

        }
        protected void btnConfermaModificaReferente_Click(object sender, EventArgs e)
        {
            //MODIFICO IL REFERENTE
            if (!string.IsNullOrEmpty(tbIdReferenteDaModificare.Text) && !string.IsNullOrEmpty(tbInsCognomeReferente.Text) && (string.IsNullOrEmpty(tbInsEmailReferente.Text) || basePage.validaIndirizzoEmail(tbInsEmailReferente.Text.Trim())))
            {

                try
                {
                    NascondiErroriValidazione();

                    Esito esito = new Esito();
                    Anag_Referente_Clienti_Fornitori nuovoReferente = new Anag_Referente_Clienti_Fornitori();
                    nuovoReferente.Id = Convert.ToInt32(tbIdReferenteDaModificare.Text);
                    nuovoReferente.Id_azienda = Convert.ToInt32(ViewState["idAzienda"]);
                    nuovoReferente.Attivo = cbInsAttivoReferente.Checked;
                    nuovoReferente.Cognome = tbInsCognomeReferente.Text.Trim();
                    nuovoReferente.Nome = tbInsNomeReferente.Text.Trim();
                    nuovoReferente.Cellulare = tbInsCellulareReferente.Text.Trim();
                    nuovoReferente.Email = tbInsEmailReferente.Text.Trim();
                    nuovoReferente.Note = tbInsNoteReferente.Text.Trim();
                    nuovoReferente.Settore = tbInsSettoreReferente.Text.Trim();
                    nuovoReferente.Telefono1 = tbInsTelefono1Referente.Text.Trim();
                    nuovoReferente.Telefono2 = tbInsTelefono2Referente.Text.Trim();

                    esito = Anag_Referente_Clienti_Fornitori_BLL.Instance.AggiornaReferente(nuovoReferente, ((Anag_Utenti)Session[SessionManager.UTENTE]));

                    btnModificaReferente.Visible = false;
                    btnInserisciReferente.Visible = true;

                    if (esito.codice != Esito.ESITO_OK)
                    {
                        //panelErrore.Style.Remove("display");
                        //lbl_MessaggioErrore.Text = esito.descrizione;
                        basePage.ShowError(esito.descrizione);
                    }
                    else
                    {
                        tbIdReferenteDaModificare.Text = "";
                        tbInsCognomeReferente.Text = "";
                        tbInsNomeReferente.Text = "";
                        tbInsCellulareReferente.Text = "";
                        tbInsEmailReferente.Text = "";
                        tbInsNoteReferente.Text = "";
                        tbInsSettoreReferente.Text = "";
                        tbInsTelefono1Referente.Text = "";
                        tbInsTelefono2Referente.Text = "";
                        cbInsAttivoReferente.Checked = true;
                        editAzienda();
                    }
                }
                catch (Exception ex)
                {
                    btnModificaReferente.Visible = false;
                    btnInserisciReferente.Visible = true;
                    //panelErrore.Style.Remove("display");
                    //lbl_MessaggioErrore.Text = ex.Message;
                    basePage.ShowError(ex.Message);
                }
            }
            else
            {
                //panelErrore.Style.Remove("display");
                //lbl_MessaggioErrore.Text = "Verificare il corretto inserimento dei campi!";
                basePage.ShowError("Verificare il corretto inserimento dei campi");
            }

        }
        protected void btnEliminaReferente_Click(object sender, EventArgs e)
        {
            //ELIMINO IL REFERENTE SE SELEZIONATO
            if (gvMod_Referenti.SelectedIndex >= 0)
            {
                try
                {
                    NascondiErroriValidazione();
                    string referenteSelezionato = gvMod_Referenti.Rows[gvMod_Referenti.SelectedIndex].Cells[1].Text;
                    Esito esito = Anag_Referente_Clienti_Fornitori_BLL.Instance.EliminaReferente(Convert.ToInt32(referenteSelezionato.Trim()), ((Anag_Utenti)Session[SessionManager.UTENTE]));

                    if (esito.codice != Esito.ESITO_OK)
                    {
                        //panelErrore.Style.Remove("display");
                        //lbl_MessaggioErrore.Text = esito.descrizione;
                        basePage.ShowError(esito.descrizione);
                    }
                    else
                    {
                        tbIdReferenteDaModificare.Text = "";
                        tbInsCognomeReferente.Text = "";
                        tbInsNomeReferente.Text = "";
                        tbInsCellulareReferente.Text = "";
                        tbInsEmailReferente.Text = "";
                        tbInsNoteReferente.Text = "";
                        tbInsSettoreReferente.Text = "";
                        tbInsTelefono1Referente.Text = "";
                        tbInsTelefono2Referente.Text = "";
                        cbInsAttivoReferente.Checked = true;

                        btnModificaReferente.Visible = false;
                        btnInserisciReferente.Visible = true;

                        editAzienda();
                    }
                }
                catch (Exception ex)
                {
                    //panelErrore.Style.Remove("display");
                    //lbl_MessaggioErrore.Text = ex.Message;
                    basePage.ShowError(ex.Message);
                }
            }
            else
            {
                //panelErrore.Style.Remove("display");
                //lbl_MessaggioErrore.Text = "Verificare il corretto inserimento dei campi!";
                basePage.ShowError("Verificare il corretto inserimento dei campi");
            }

        }

        protected void btnAnnullaReferente_Click(object sender, EventArgs e)
        {
            tbIdReferenteDaModificare.Text = "";
            tbInsCognomeReferente.Text = "";
            tbInsNomeReferente.Text = "";
            tbInsCellulareReferente.Text = "";
            tbInsEmailReferente.Text = "";
            tbInsNoteReferente.Text = "";
            tbInsSettoreReferente.Text = "";
            tbInsTelefono1Referente.Text = "";
            tbInsTelefono2Referente.Text = "";
            cbInsAttivoReferente.Checked = true;

            btnModificaReferente.Visible = false;
            btnInserisciReferente.Visible = true;

        }

        protected void gvMod_Referenti_RigaSelezionata(object sender, EventArgs e)
        {
            //SCARICO IL REFERENTE SE SELEZIONATO
            if (gvMod_Referenti.SelectedIndex >= 0)
            {
                try
                {
                    NascondiErroriValidazione();

                    string referenteSelezionato = gvMod_Referenti.Rows[gvMod_Referenti.SelectedIndex].Cells[1].Text;
                    Esito esito = new Esito();
                    Anag_Referente_Clienti_Fornitori referente = Anag_Referente_Clienti_Fornitori_BLL.Instance.getReferenteById(ref esito, Convert.ToInt32(referenteSelezionato));

                    if (esito.codice != Esito.ESITO_OK)
                    {
                        btnModificaReferente.Visible = false;
                        btnInserisciReferente.Visible = true;
                        tbIdReferenteDaModificare.Text = "";
                        //panelErrore.Style.Remove("display");
                        //lbl_MessaggioErrore.Text = esito.descrizione;
                        basePage.ShowError(esito.descrizione);
                    }
                    else
                    {
                        btnModificaReferente.Visible = true;
                        btnInserisciReferente.Visible = false;

                        tbIdReferenteDaModificare.Text = referente.Id.ToString();
                        tbInsCognomeReferente.Text = referente.Cognome;
                        tbInsNomeReferente.Text = referente.Nome;
                        tbInsSettoreReferente.Text = referente.Settore;
                        tbInsEmailReferente.Text = referente.Email;
                        tbInsTelefono1Referente.Text = referente.Telefono1;
                        tbInsTelefono2Referente.Text = referente.Telefono2;
                        tbInsNoteReferente.Text = referente.Note;
                        tbInsCellulareReferente.Text = referente.Cellulare;
                        cbInsAttivoReferente.Checked = referente.Attivo;
                    }
                }
                catch (Exception ex)
                {
                    btnModificaReferente.Visible = false;
                    btnInserisciReferente.Visible = true;
                    tbIdReferenteDaModificare.Text = "";
                    //panelErrore.Style.Remove("display");
                    //lbl_MessaggioErrore.Text = ex.Message;
                    basePage.ShowError(ex.Message);
                }
            }

        }

    }
}