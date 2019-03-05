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
        BasePage basePage = new BasePage();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                lblIntestazionePagina.Text = "ARTICOLI";

                BasePage p = new BasePage();
                Esito esito = p.CaricaListeTipologiche();

                // CARICO LE COMBO
                if (string.IsNullOrEmpty(esito.descrizione))
                {
                    cmbMod_Genere.Items.Clear();
                    cmbMod_Genere.Items.Add("");
                    foreach (Tipologica tipologiaGenere in p.listaTipiGeneri)
                    {
                        ListItem item = new ListItem();
                        item.Text = tipologiaGenere.nome;
                        item.Value = tipologiaGenere.id.ToString();
                        cmbMod_Genere.Items.Add(item);
                    }

                    cmbMod_Gruppo.Items.Clear();
                    cmbMod_Gruppo.Items.Add("");
                    foreach (Tipologica tipologiaGruppo in p.listaTipiGruppi)
                    {
                        ListItem item = new ListItem();
                        item.Text = tipologiaGruppo.nome;
                        item.Value = tipologiaGruppo.id.ToString();
                        cmbMod_Gruppo.Items.Add(item);
                    }

                    cmbMod_Sottogruppo.Items.Clear();
                    cmbMod_Sottogruppo.Items.Add("");
                    foreach (Tipologica tipologiaSottogruppo in p.listaTipiSottogruppi)
                    {
                        ListItem item = new ListItem();
                        item.Text = tipologiaSottogruppo.nome;
                        item.Value = tipologiaSottogruppo.id.ToString();
                        cmbMod_Sottogruppo.Items.Add(item);
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
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriTabGiusta", script: "openDettaglioArticolo('" + hf_tabChiamata.Value + "');", addScriptTags: true);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "chiudiLoader", script: "$('.loader').hide();", addScriptTags: true);
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

            }
            else
            {
                divBtnInserisciArticoli.Visible = true;
                btnModifica.Visible = true;
                btnAnnulla.Visible = true;
                btnSalva.Visible = true;
                btnElimina.Visible = true;

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
                if (esito.codice == 0)
                {
                    pulisciCampiDettaglio();

                    // RIEMPIO I CAMPI DEL DETTAGLIO COLLABORATORE
                    tbMod_Descrizione.Text = articolo.DefaultDescrizioneLunga;
                    tbMod_DescrizioneBreve.Text = articolo.DefaultDescrizione;
                    tbMod_Prezzo.Text = articolo.DefaultPrezzo.ToString();
                    tbMod_Costo.Text = articolo.DefaultCosto.ToString();
                    tbMod_IVA.Text = articolo.DefaultIva.ToString();
                    tbMod_Note.Text = articolo.Note;


                    cbMod_Attivo.Checked = articolo.Attivo;
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
                    lbMod_Gruppi.Items.Clear();
                    List<Art_Gruppi> listaGruppi = Art_Gruppi_Articoli_BLL.Instance.getGruppiByIdArticolo(articolo.Id, ref esito);
                    if (esito.codice == 0)
                    {
                        if (listaGruppi!=null && listaGruppi.Count>0) {
                            lbMod_Gruppi.Rows = listaGruppi.Count;
                            foreach (Art_Gruppi gruppo in listaGruppi)
                            {
                                ListItem item = new ListItem();
                                item.Text = gruppo.Nome;
                                item.Value = gruppo.Id.ToString();
                                lbMod_Gruppi.Items.Add(item);
                            }
                        }
                        else
                        {
                            lbMod_Gruppi.Rows = 1;
                        }
                    }
                    else
                    {
                        Session["ErrorPageText"] = esito.descrizione;
                        string url = String.Format("~/pageError.aspx");
                        Response.Redirect(url, true);
                    }
                    // REFERENTI
                    //DataTable dtGruppi = new DataTable();
                    //if (articolo.Referenti != null)
                    //{
                    //    dtReferenti.Columns.Add("id");
                    //    dtReferenti.Columns.Add("Cognome");
                    //    dtReferenti.Columns.Add("Nome");
                    //    dtReferenti.Columns.Add("Settore");
                    //    dtReferenti.Columns.Add("Telefono1");
                    //    dtReferenti.Columns.Add("Telefono2");
                    //    dtReferenti.Columns.Add("Cellulare");
                    //    dtReferenti.Columns.Add("Email");
                    //    foreach (Anag_Referente_Clienti_Fornitori referente in azienda.Referenti)
                    //    {
                    //        ListItem itemReferente = new ListItem(referente.Cognome + " " + referente.Nome + " - " + referente.Settore + " - " + referente.Telefono1, referente.Id.ToString());
                    //        lbMod_Referenti.Items.Add(itemReferente);
                    //        DataRow dr = dtReferenti.NewRow();
                    //        dr["id"] = referente.Id.ToString();
                    //        dr["Cognome"] = referente.Cognome;
                    //        dr["Nome"] = referente.Nome;
                    //        dr["Settore"] = referente.Settore;
                    //        dr["Telefono1"] = referente.Telefono1;
                    //        dr["Telefono2"] = referente.Telefono2;
                    //        dr["Cellulare"] = referente.Cellulare;
                    //        dr["Email"] = referente.Email;
                    //        dtReferenti.Rows.Add(dr);
                    //    }
                    //    gvMod_Referenti.DataSource = dtReferenti;
                    //    gvMod_Referenti.DataBind();
                    //}
                    //if (azienda.Referenti != null && azienda.Referenti.Count > 0)
                    //{
                    //    lbMod_Referenti.Rows = azienda.Referenti.Count;
                    //}
                    //else
                    //{
                    //    lbMod_Referenti.Rows = 1;
                    //    gvMod_Referenti.DataSource = null;
                    //}

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
            tbMod_Descrizione.Text = "";
            tbMod_DescrizioneBreve.Text = "";
            tbMod_Prezzo.Text = "";
            tbMod_Costo.Text = "";
            tbMod_IVA.Text = "";
            tbMod_Note.Text = "";

            cmbMod_Genere.Text = "";
            cmbMod_Gruppo.Text = "";
            cmbMod_Sottogruppo.Text = "";

            cbMod_Stampa.Checked = false;
            cbMod_Attivo.Checked = true;

            //lbMod_Referenti.Items.Clear();
            //lbMod_Referenti.Rows = 1;

            //gvMod_Referenti.DataSource = null;

        }

        protected void InserisciArticoli_Click(object sendere, EventArgs e)
        {
            ViewState["idArticolo"] = "";
            editAziendaVuota();
            AttivaDisattivaModificaArticolo(false);
            gestisciPulsantiArticolo("INSERIMENTO");

            // PULISCO I PH DI MODIFICA
            btnAnnullaReferente_Click(null, null);

            phReferenti.Visible = false;

            pnlContainer.Visible = true;

        }

        private void editAziendaVuota()
        {
            Esito esito = new Esito();
            pulisciCampiDettaglio();
        }
        protected void btnRicercaArticoli_Click(object sender, EventArgs e)
        {
            string queryRicerca = ConfigurationManager.AppSettings["QUERY_SEARCH_ARTICOLI"];

            queryRicerca = queryRicerca.Replace("@defaultDescrizioneLunga", tbDescrizione.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@defaultDescrizione", tbDescrizioneBreve.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@defaultPrezzo", tbPrezzo.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@defaultCosto", TbCosto.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@defaultIva", tbIva.Text.Trim().Replace("'", "''"));

            Esito esito = new Esito();
            DataTable dtArticoli = Base_DAL.getDatiBySql(queryRicerca, ref esito);
            gv_articoli.DataSource = dtArticoli;
            gv_articoli.DataBind();

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
            tbMod_IVA.ReadOnly = attivaModifica;
            tbMod_Note.ReadOnly = attivaModifica;
            tbMod_Prezzo.ReadOnly = attivaModifica;

            cbMod_Attivo.Enabled = !attivaModifica;
            cbMod_Stampa.Enabled = !attivaModifica;

            if (attivaModifica)
            {
                cmbMod_Gruppo.Attributes.Add("disabled", "");
                cmbMod_Genere.Attributes.Add("disabled", "");
                cmbMod_Sottogruppo.Attributes.Add("disabled", "");
                lbMod_Gruppi.Attributes.Add("disabled", "");
            }
            else
            {
                cmbMod_Gruppo.Attributes.Remove("disabled");
                cmbMod_Genere.Attributes.Remove("disabled");
                cmbMod_Sottogruppo.Attributes.Remove("disabled");
                lbMod_Gruppi.Attributes.Remove("disabled");
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

            if (!string.IsNullOrEmpty((string)ViewState["idArticolo"]))
            {
                esito = Art_Articoli_BLL.Instance.EliminaArticolo(Convert.ToInt32(ViewState["idArticolo"].ToString()));
                if (esito.codice != Esito.ESITO_OK)
                {
                    //panelErrore.Style.Remove("display");
                    panelErrore.Style.Add("display","block");
                    lbl_MessaggioErrore.Text = esito.descrizione;
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

            if (esito.codice != Esito.ESITO_OK)
            {
                panelErrore.Style.Remove("display");
                lbl_MessaggioErrore.Text = "Controllare i campi evidenziati";
            }
            else
            {
                NascondiErroriValidazione();

                esito = Art_Articoli_BLL.Instance.AggiornaArticolo(articolo);


                if (esito.codice != Esito.ESITO_OK)
                {
                    panelErrore.Style.Remove("display");
                    lbl_MessaggioErrore.Text = esito.descrizione;
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

                articolo.Id = Convert.ToInt16(ViewState["idAzienda"].ToString());

                articolo.Attivo = Convert.ToBoolean(BasePage.ValidaCampo(cbMod_Attivo, "true", false, ref esito));
                articolo.DefaultStampa = Convert.ToBoolean(BasePage.ValidaCampo(cbMod_Stampa, "true", false, ref esito));
                articolo.DefaultDescrizione = BasePage.ValidaCampo(tbMod_DescrizioneBreve, "", false, ref esito);
                articolo.DefaultDescrizioneLunga = BasePage.ValidaCampo(tbMod_Descrizione, "", false, ref esito);
                articolo.DefaultIdTipoGenere = 1;
                articolo.DefaultIdTipoGruppo = 1;
                articolo.DefaultIdTipoSottogruppo = 1;
                articolo.DefaultIva = Convert.ToInt16(BasePage.ValidaCampo(tbMod_IVA, "", false, ref esito));
                articolo.DefaultPrezzo = Convert.ToDecimal(BasePage.ValidaCampo(tbMod_Prezzo, "", false, ref esito));
                articolo.DefaultCosto = Convert.ToDecimal(BasePage.ValidaCampo(tbMod_Costo, "", false, ref esito));

                //azienda.TipoIndirizzoLegale = cmbMod_TipoIndirizzoLegale.SelectedValue;

                return articolo;

            }
            catch (Exception ex)
            {
                esito.codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.descrizione = ex.Message;
                return articolo;
            }
        }

        protected void btnConfermaInserimento_Click(object sender, EventArgs e)
        {
            // INSERISCO ARTICOLO
            Esito esito = new Esito();
            Art_Articoli articolo = CreaOggettoSalvataggio(ref esito);

            if (esito.codice != Esito.ESITO_OK)
            {
                panelErrore.Style.Remove("display");
                lbl_MessaggioErrore.Text = esito.descrizione;
            }
            else
            {
                NascondiErroriValidazione();

                int iRet = Art_Articoli_BLL.Instance.CreaArticolo(articolo, ref esito);
                if (iRet > 0)
                {
                    // UNA VOLTA INSERITO CORRETTAMENTE PUO' ESSERE MODIFICATO
                    hf_idArticolo.Value = iRet.ToString();
                    ViewState["idArticolo"] = hf_idArticolo.Value;
                    hf_tipoOperazione.Value = "VISUALIZZAZIONE";
                }

                if (esito.codice != Esito.ESITO_OK)
                {
                    //panelErrore.Style.Remove("display");
                    panelErrore.Style.Add("display","block");
                    lbl_MessaggioErrore.Text = esito.descrizione;
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
            panelErrore.Style.Add("display", "none");

            tbMod_Descrizione.CssClass = tbMod_Descrizione.CssClass.Replace("erroreValidazione", "");
            tbMod_DescrizioneBreve.CssClass = tbMod_DescrizioneBreve.CssClass.Replace("erroreValidazione", "");
            tbMod_IVA.CssClass = tbMod_IVA.CssClass.Replace("erroreValidazione", "");
            tbMod_Costo.CssClass = tbMod_Costo.CssClass.Replace("erroreValidazione", "");
            tbMod_Prezzo.CssClass = tbMod_Prezzo.CssClass.Replace("erroreValidazione", "");
            tbMod_Note.CssClass = tbMod_Note.CssClass.Replace("erroreValidazione", "");
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


                    int iNuovoReferente = Anag_Referente_Clienti_Fornitori_BLL.Instance.CreaReferente(nuovoReferente, ref esito);

                    if (esito.codice != Esito.ESITO_OK)
                    {
                        panelErrore.Style.Remove("display");
                        lbl_MessaggioErrore.Text = esito.descrizione;
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
                        editArticolo();
                    }
                }
                catch (Exception ex)
                {
                    panelErrore.Style.Remove("display");
                    lbl_MessaggioErrore.Text = ex.Message;
                }
            }
            else
            {
                panelErrore.Style.Remove("display");
                lbl_MessaggioErrore.Text = "Verificare il corretto inserimento dei campi!";
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

                    esito = Anag_Referente_Clienti_Fornitori_BLL.Instance.AggiornaReferente(nuovoReferente);

                    btnModificaReferente.Visible = false;
                    btnInserisciReferente.Visible = true;

                    if (esito.codice != Esito.ESITO_OK)
                    {
                        panelErrore.Style.Remove("display");
                        lbl_MessaggioErrore.Text = esito.descrizione;
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
                        editArticolo();
                    }
                }
                catch (Exception ex)
                {
                    btnModificaReferente.Visible = false;
                    btnInserisciReferente.Visible = true;
                    panelErrore.Style.Remove("display");
                    lbl_MessaggioErrore.Text = ex.Message;
                }
            }
            else
            {
                panelErrore.Style.Remove("display");
                lbl_MessaggioErrore.Text = "Verificare il corretto inserimento dei campi!";
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
                    Esito esito = Anag_Referente_Clienti_Fornitori_BLL.Instance.EliminaReferente(Convert.ToInt32(referenteSelezionato.Trim()));

                    if (esito.codice != Esito.ESITO_OK)
                    {
                        panelErrore.Style.Remove("display");
                        lbl_MessaggioErrore.Text = esito.descrizione;
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

                        editArticolo();
                    }
                }
                catch (Exception ex)
                {
                    panelErrore.Style.Remove("display");
                    lbl_MessaggioErrore.Text = ex.Message;
                }
            }
            else
            {
                panelErrore.Style.Remove("display");
                lbl_MessaggioErrore.Text = "Verificare il corretto inserimento dei campi!";
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
                        panelErrore.Style.Remove("display");
                        lbl_MessaggioErrore.Text = esito.descrizione;
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
                    panelErrore.Style.Remove("display");
                    lbl_MessaggioErrore.Text = ex.Message;
                }
            }

        }

    }
}