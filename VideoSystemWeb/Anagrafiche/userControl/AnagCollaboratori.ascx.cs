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
using AjaxControlToolkit;
using System.Collections;
namespace VideoSystemWeb.Anagrafiche.userControl
{
    public partial class AnagCollaboratori : System.Web.UI.UserControl
    {

        BasePage basePage = new BasePage();
        public string dettaglioModifica = "";
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {

            //if (basePage.AbilitazioneInScrittura())
            //{
            // ASSOCIO L'EVENTO DOUBLECLICK ALLE LISTBOX
            //if (Request["__EVENTARGUMENT"] != null && Request["__EVENTARGUMENT"] == "move")
            //{
            //lbMod_Email_DoubleClick();
            //    //lbMod_Indirizzi_DoubleClick();
            //    //lbMod_Telefoni_DoubleClick();
            //    //lbMod_Documenti_DoubleClick();
            //}
            //lbMod_Email.Attributes.Add("ondblclick", Page.ClientScript.GetPostBackEventReference(lbMod_Email, "move"));
            //lbMod_Indirizzi.Attributes.Add("ondblclick", Page.ClientScript.GetPostBackEventReference(lbMod_Indirizzi, "move"));
            //lbMod_Telefoni.Attributes.Add("ondblclick", Page.ClientScript.GetPostBackEventReference(lbMod_Telefoni, "move"));
            //lbMod_Documenti.Attributes.Add("ondblclick", Page.ClientScript.GetPostBackEventReference(lbMod_Documenti, "move"));
            //}

            //string prot = Protocollo_BLL.Instance.getNumeroProtocollo();
            //Protocollo_BLL.Instance.resetProcotollo(10);
            //prot = Protocollo_BLL.Instance.getNumeroProtocollo();

            //string codLav = Protocollo_BLL.Instance.getCodLavFormattato();
            //Protocollo_BLL.Instance.resetCodiceLavorazione(27);
            //codLav = Protocollo_BLL.Instance.getCodLavFormattato();

            if (!Page.IsPostBack)
            {

                //log.Info("PAGE AnagCollaboratori");
                //BasePage p = new BasePage();
                //Esito esito = p.CaricaListeTipologiche();

                //if (string.IsNullOrEmpty(esito.descrizione))
                //{
                    ddlQualifiche.Items.Clear();
                    ddlQualifiche.Items.Add("");

                    ddlQualificheDaAggiungere.Items.Clear();
                    foreach (Tipologica qualifica in SessionManager.ListaQualifiche)
                    {
                        ListItem item = new ListItem();
                        ListItem itemDaAggiungere = new ListItem();
                        item.Text = qualifica.nome;
                        itemDaAggiungere.Text = qualifica.nome;
                        // metto comunque il nome e non l'id perchè la ricerca sulla tabella anag_qualifiche_collaboratori la faccio sul nome
                        item.Value = qualifica.nome;
                        itemDaAggiungere.Value = qualifica.id.ToString();
                        ddlQualifiche.Items.Add(item);

                        ddlQualificheDaAggiungere.Items.Add(itemDaAggiungere);
                    }

                    // SE UTENTE ABILITATO ALLE MODIFICHE FACCIO VEDERE I PULSANTI DI MODIFICA
                    AbilitaBottoni(basePage.AbilitazioneInScrittura());


                // CREO LA SESSION DEI COLLABORATORI A CUI INVIARE MESSAGGI WHATSAPP
                Hashtable htCollaboratoriWhatsapp = new Hashtable();
                Session[SessionManager.LISTA_COLLABORATORI_PER_INVIO_WHATSAPP] = htCollaboratoriWhatsapp;

                //}
                //else
                //{
                //    log.Error(esito.descrizione);
                //    Session["ErrorPageText"] = esito.descrizione;
                //    string url = String.Format("~/pageError.aspx");
                //    Response.Redirect(url, true);
                //}


            }
            // SELEZIONO L'ULTIMA TAB SELEZIONATA
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriTabGiusta", script: "openDettaglioAnagrafica('" + hf_tabChiamata.Value + "');", addScriptTags: true);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "chiudiLoader", script: "$('.loader').hide();", addScriptTags: true);
        }
        // ABILITO I BOTTONI DI MODIFICA IN BASE ALLA TIPOLOGIA DI UTENTE
        private void AbilitaBottoni(bool utenteAbilitatoInScrittura)
        {
            AnnullaModifiche();
            if (!utenteAbilitatoInScrittura)
            {
                //btnInserisciCollaboratori.Visible = false;
                divBtnInserisciCollaboratori.Visible = false;
                btnModifica.Visible = false;
                btnAnnulla.Visible = false;
                btnSalva.Visible = false;
                btnElimina.Visible = false;
                uploadButton.Visible = false;

                btnApriQualifiche.Visible = false;
                btnApriEmail.Visible = false;
                btnApriIndirizzi.Visible = false;
                btnApriDocumenti.Visible = false;
            }
            else
            {
                //btnInserisciCollaboratori.Visible = true;
                divBtnInserisciCollaboratori.Visible = true;
                btnModifica.Visible = true;
                btnAnnulla.Visible = false;
                btnSalva.Visible = false;
                btnElimina.Visible = false;
                uploadButton.Visible = true;

                btnApriQualifiche.Visible = true;
                btnApriEmail.Visible = true;
                btnApriIndirizzi.Visible = true;
                btnApriDocumenti.Visible = true;
            }
        }
        // RICERCA COLLABORATORI
        protected void btnRicercaCollaboratori_Click(object sender, EventArgs e)
        {

            string queryRicerca = ConfigurationManager.AppSettings["QUERY_SEARCH_COLLABORATORI"];

            queryRicerca = queryRicerca.Replace("@cognome", tbCognome.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@codiceFiscale", tbCF.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@comuneRiferimento", tbCitta.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@regioneRiferimento", tbRegione.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@partitaIva", TbPiva.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@nomeSocieta", TbSocieta.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@qualifica", ddlQualifiche.SelectedValue.ToString().Trim().Replace("'","''"));
            queryRicerca = queryRicerca.Replace("@nome", tbNome.Text.Trim().Replace("'", "''"));
            Esito esito = new Esito();
            DataTable dtCollaboratori = Base_DAL.GetDatiBySql(queryRicerca,ref esito);
            gv_collaboratori.DataSource = dtCollaboratori;
            gv_collaboratori.DataBind();
        }

        protected void ddlQualifiche_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        // AGGIORNO LE RIGHE DEL DATAGRID CON LA FUNZIONE DI APERTURA DETTAGLIO
        protected void gv_collaboratori_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // PRENDO L'ID DEL COLLABORATORE SELEZIONATO

                CheckBox cbw = (CheckBox) e.Row.Cells[0].Controls[1];
                string idCollaboratoreSelezionato = e.Row.Cells[1].Text;

                Hashtable htCollaboratoriWhatsapp = (Hashtable)Session[SessionManager.LISTA_COLLABORATORI_PER_INVIO_WHATSAPP];

                if (htCollaboratoriWhatsapp.ContainsKey(idCollaboratoreSelezionato))
                {
                    cbw.Checked = true;
                }
                else
                {
                    cbw.Checked = false;
                }
                int conta = 0;
                foreach (TableCell item in e.Row.Cells)
                {
                    if (conta > 0)
                    {
                        item.Attributes["onclick"] = "mostraCollaboratore('" + idCollaboratoreSelezionato + "');";
                    }
                    conta++;
                }
            }
        }
        // GESTIONE PAGINAZIONE GRIGLIA
        protected void gv_collaboratori_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_collaboratori.PageIndex = e.NewPageIndex;
            btnRicercaCollaboratori_Click(null,null);
        }

        // APRO POPUP DETTAGLIO COLLABORATORE
        protected void EditCollaboratore_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hf_idColl.Value) || (!string.IsNullOrEmpty((string)ViewState["idColl"]))) {
                if (!string.IsNullOrEmpty(hf_idColl.Value)) ViewState["idColl"] = hf_idColl.Value;
                EditCollaboratore();
                AttivaDisattivaModificaAnagrafica(true);
                GestisciPulsantiAnagrafica("VISUALIZZAZIONE");
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriTabGiusta", script: "openDettaglioAnagrafica('Anagrafica');", addScriptTags: true);
                pnlContainer.Visible = true;
            }
        }
        // GESTIONE PULSANTI MODIFICA COLLABORATORE
        protected void btnModifica_Click(object sender, EventArgs e)
        {
            AttivaDisattivaModificaAnagrafica(false);
            GestisciPulsantiAnagrafica("MODIFICA");
        }
        // DETTAGLIO GESTIONE PULSANTI ANAGRAFICA
        private void GestisciPulsantiAnagrafica(string stato)
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
                        btnAnnullaIndirizzo_Click(null, null);
                        btnAnnullaTelefono_Click(null, null);
                        btnAnnullaEmail_Click(null, null);
                        phEmail.Visible = false;
                        phTelefoni.Visible = false;
                        phQualifiche.Visible = false;
                        phIndirizzi.Visible = false;
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

        protected void btnSalva_Click(object sender, EventArgs e)
        {
            // SALVO MODIFICHE COLLABORATORE
            Esito esito = new Esito();
            Anag_Collaboratori collaboratore = CreaOggettoSalvataggio(ref esito);

            if (string.IsNullOrEmpty(Path.GetFileName(imgCollaboratore.ImageUrl))) {
                collaboratore.PathFoto = ConfigurationManager.AppSettings["IMMAGINE_DUMMY_COLLABORATORE"];
            }
            else { 
                collaboratore.PathFoto = Path.GetFileName(imgCollaboratore.ImageUrl);
            }

            if (esito.Codice != Esito.ESITO_OK)
            {
                log.Error(esito.Descrizione);
                //panelErrore.Style.Add("display","block");
                //lbl_MessaggioErrore.Text = "Controllare i campi evidenziati";
                basePage.ShowWarning("Controllare i campi evidenziati!");
                AttivaDisattivaModificaAnagrafica(false);
            }
            else
            {
                NascondiErroriValidazione();

                esito = Anag_Collaboratori_BLL.Instance.AggiornaCollaboratore(collaboratore);

                if (esito.Codice != Esito.ESITO_OK)
                {
                    log.Error(esito.Descrizione);
                    //panelErrore.Style.Remove("display");
                    //panelErrore.Style.Add("display", "block");
                    //lbl_MessaggioErrore.Text = esito.descrizione;
                    basePage.ShowError(esito.Descrizione);

                }
                else
                {
                    SessionManager.ListaAnagraficheCollaboratori.Clear();
                    //SessionManager.ListaCittaCollaboratori.Clear();
                    EditCollaboratore_Click(null, null);
                }
            }
        }

        protected void btnAnnulla_Click(object sender, EventArgs e)
        {
            AnnullaModifiche();
        }

        private void AnnullaModifiche()
        {
            NascondiErroriValidazione();
            AttivaDisattivaModificaAnagrafica(true);
            EditCollaboratore();
            GestisciPulsantiAnagrafica("ANNULLAMENTO");
        }
        
        protected void InserisciCollaboratori_Click(object sender, EventArgs e)
        {
            ViewState["idColl"] = "";
            EditCollaboratoreVuoto();
            AttivaDisattivaModificaAnagrafica(false);
            GestisciPulsantiAnagrafica("INSERIMENTO");

            // PULISCO I PH DI MODIFICA
            btnAnnullaIndirizzo_Click(null, null);
            btnAnnullaTelefono_Click(null, null);
            btnAnnullaEmail_Click(null, null);
            phEmail.Visible = false;
            phTelefoni.Visible = false;
            phQualifiche.Visible = false;
            phIndirizzi.Visible = false;
            phDocumenti.Visible = false;

            pnlContainer.Visible = true;
        }

        private void PulisciCampiDettaglio()
        {
            tbMod_CF.Text = "";
            tbMod_Cognome.Text = "";
            tbMod_Nome.Text = "";
            tbMod_Nazione.Text = "";
            tbMod_ComuneNascita.Text = "";
            tbMod_ProvinciaNascita.Text = "";
            tbMod_ComuneRiferimento.Text = "";
            cmbMod_RegioneRiferimento.Text = "";
            tbMod_DataNascita.Text = "";
            tbMod_NomeSocieta.Text = "";
            tbMod_Iban.Text = "";
            tbMod_PartitaIva.Text = "";
            cbMod_Assunto.Checked = false;
            //cbMod_Attivo.Checked = false;
            tbMod_Note.Text = "";

            lbMod_Qualifiche.Items.Clear();
            lbMod_Qualifiche.Rows = 1;

            //lbMod_Indirizzi.Items.Clear();
            //lbMod_Indirizzi.Rows = 1;

            gvMod_Indirizzi.DataSource = null;

            //lbMod_Email.Items.Clear();
            //lbMod_Email.Rows = 1;

            gvMod_Email.DataSource = null;

            //lbMod_Telefoni.Items.Clear();
            //lbMod_Telefoni.Rows = 1;

            gvMod_Telefoni.DataSource = null;

            //lbMod_Documenti.Items.Clear();
            //lbMod_Documenti.Rows = 1;

            gvMod_Documenti.DataSource = null;

        }
        // CARICA IMMAGINE COLLABORATORE
        protected void CaricaImmagine(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ViewState["idColl"].ToString()) && !ViewState["idColl"].ToString().Equals("0")) {
                // SE L'UTENTE E' IN MODIFICA CARICO L'IMMAGINE
                if (fuImg.HasFile)
                {
                    if (".JPEG|.JPG|.BMP|.PNG|.GIF".IndexOf(Path.GetExtension(fuImg.FileName).ToUpper()) >= 0)
                    {
                        Esito esito = new Esito();
                        try
                        {
                            string nomefileNew = DateTime.Now.Ticks.ToString() + Path.GetExtension(fuImg.FileName);
                            string fullPath = Server.MapPath(ConfigurationManager.AppSettings["PATH_IMMAGINI_COLLABORATORI"]) + Path.GetFileName(nomefileNew);
                            fuImg.SaveAs(fullPath);
                            imgCollaboratore.ImageUrl = fullPath;
                            lblImage.Text = nomefileNew;

                            string queryUpdateImg = "UPDATE ANAG_COLLABORATORI SET pathFoto = '" + nomefileNew + "' WHERE ID = " + ViewState["idColl"].ToString();
                           
                            int iRighe = Base_DAL.ExecuteUpdateBySql(queryUpdateImg, ref esito);
                            EditCollaboratore_Click(null, null);
                        }
                        catch (Exception ex)
                        {
                            lblImage.Text = ex.Message;

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
                        lblImage.Text = "Caricare un file di tipo immagine";
                        basePage.ShowWarning("Caricare un file di tipo immagine");
                    }
                }
                else
                {
                    lblImage.Text = "Caricare un'immagine";
                    basePage.ShowWarning("Caricare un'immagine");
                }
            }
        }

        protected void CaricaDocumento(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbIdDocumentoDaModificare.Text.Trim()))
            {

                // SE L'UTENTE E' IN MODIFICA CARICO L'IMMAGINE
                if (fuDoc.HasFile)
                {

                    if (".JPEG|.JPG|.BMP|.PDF".IndexOf(Path.GetExtension(fuDoc.FileName).ToUpper()) >= 0)
                    {
                        Esito esito = new Esito();
                        try
                        {
                            string nomefileDoc = DateTime.Now.Ticks.ToString() + Path.GetExtension(fuDoc.FileName);
                            string fullPath = Server.MapPath(ConfigurationManager.AppSettings["PATH_DOCUMENTI_COLLABORATORI"]) + Path.GetFileName(nomefileDoc);
                            fuDoc.SaveAs(fullPath);
                            string queryUpdateImg = "UPDATE anag_documenti_collaboratori SET pathDocumento = '" + nomefileDoc + "' WHERE ID = " + tbIdDocumentoDaModificare.Text.Trim();
                           
                            int iRighe = Base_DAL.ExecuteUpdateBySql(queryUpdateImg, ref esito);
                            lblDoc.Text = "Caricato file " + nomefileDoc;

                            btnCaricaDocumento.Visible = false;
                            fuDoc.Visible = false;

                            EditCollaboratore_Click(null, null);
                        }
                        catch (Exception ex)
                        {
                            lblDoc.Text = ex.Message;

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
                        lblDoc.Text = "Caricare un file di tipo Documento";
                        basePage.ShowWarning("Caricare un file di tipo Documento");
                    }
                }
                else
                {
                    lblDoc.Text = "Caricare un documento";
                    basePage.ShowWarning("Caricare un documento");
                }
            }


        }

        private void AttivaDisattivaModificaAnagrafica(bool attivaModifica)
        {
            tbMod_CF.ReadOnly = attivaModifica;
            tbMod_Cognome.ReadOnly = attivaModifica;
            tbMod_ComuneNascita.ReadOnly = attivaModifica;
            tbMod_ComuneRiferimento.ReadOnly = attivaModifica;
            tbMod_DataNascita.ReadOnly = attivaModifica;
            tbMod_Nazione.ReadOnly = attivaModifica;
            tbMod_Nome.ReadOnly = attivaModifica;
            tbMod_NomeSocieta.ReadOnly = attivaModifica;
            tbMod_Iban.ReadOnly = attivaModifica;
            tbMod_Note.ReadOnly = attivaModifica;
            tbMod_PartitaIva.ReadOnly = attivaModifica;
            tbMod_ProvinciaNascita.ReadOnly = attivaModifica;
            //cbMod_Assunto.Enabled = !attivaModifica;
            //cbMod_Attivo.Enabled = !attivaModifica;
            //cmbMod_RegioneRiferimento.Enabled
            if (attivaModifica)
            {
                cmbMod_RegioneRiferimento.Attributes.Add("disabled", "");
                cbMod_Assunto.Attributes.Add("disabled", "");
            }
            else
            {
                cmbMod_RegioneRiferimento.Attributes.Remove("disabled");
                cbMod_Assunto.Attributes.Remove("disabled");
            }
        }

        private void EditCollaboratore()
        {
            string idCollaboratore = (string) ViewState["idColl"];
            Esito esito = new Esito();

            if (!string.IsNullOrEmpty(idCollaboratore))
            {
                Entity.Anag_Collaboratori collaboratore = Anag_Collaboratori_BLL.Instance.getCollaboratoreById(Convert.ToInt16(idCollaboratore), ref esito);
                if (esito.Codice == 0)
                {
                    PulisciCampiDettaglio();

                    dettaglioModifica = collaboratore.Cognome.Trim() + " " + collaboratore.Nome.Trim();
                    lblDettaglioModifica.Text = dettaglioModifica;
                    // RIEMPIO I CAMPI DEL DETTAGLIO COLLABORATORE
                    tbMod_CF.Text = collaboratore.CodiceFiscale;
                    tbMod_Cognome.Text = collaboratore.Cognome;
                    tbMod_Nome.Text = collaboratore.Nome;
                    tbMod_Nazione.Text = collaboratore.Nazione;
                    tbMod_ComuneNascita.Text = collaboratore.ComuneNascita;
                    tbMod_ProvinciaNascita.Text = collaboratore.ProvinciaNascita;
                    tbMod_ComuneRiferimento.Text = collaboratore.ComuneRiferimento;

                    //tbMod_DataNascita.Text = collaboratore.DataNascita.ToShortDateString();
                    tbMod_DataNascita.Text = collaboratore.DataNascita.ToString("dd/MM/yyyy");

                    tbMod_NomeSocieta.Text = collaboratore.NomeSocieta;
                    tbMod_Iban.Text = collaboratore.Iban !=null? collaboratore.Iban.ToUpper() : "";
                    tbMod_PartitaIva.Text = collaboratore.PartitaIva;
                    cbMod_Assunto.Checked = collaboratore.Assunto;
                    //cbMod_Attivo.Checked = collaboratore.Attivo;
                    tbMod_Note.Text = collaboratore.Note;

                    //REGIONE RIFERIMENTO
                    ListItem trovati = cmbMod_RegioneRiferimento.Items.FindByValue(collaboratore.RegioneRiferimento);
                    if (trovati != null)
                    {
                        cmbMod_RegioneRiferimento.SelectedValue = trovati.Value;
                    }
                    else
                    {
                        cmbMod_RegioneRiferimento.Text = "";
                    }

                    // QUALIFICHE
                    if (collaboratore.Qualifiche != null) { 
                        foreach (Anag_Qualifiche_Collaboratori qualifica in collaboratore.Qualifiche)
                        {
                            ListItem itemQualifica = new ListItem(qualifica.Qualifica, qualifica.Id.ToString());
                            lbMod_Qualifiche.Items.Add(itemQualifica);
                        }
                    }
                    if (collaboratore.Qualifiche != null && collaboratore.Qualifiche.Count > 0)
                    {
                        lbMod_Qualifiche.Rows = collaboratore.Qualifiche.Count;
                    }
                    else
                    {
                        lbMod_Qualifiche.Rows = 1;
                    }

                    // INDIRIZZI
                    DataTable dtIndirizzi = new DataTable();
                    if (collaboratore.Indirizzi != null) {

                        dtIndirizzi.Columns.Add("id");
                        dtIndirizzi.Columns.Add("Descrizione");
                        dtIndirizzi.Columns.Add("Tipo");
                        dtIndirizzi.Columns.Add("Indirizzo");
                        dtIndirizzi.Columns.Add("NumeroCivico");
                        dtIndirizzi.Columns.Add("Cap");
                        dtIndirizzi.Columns.Add("Comune");
                        dtIndirizzi.Columns.Add("Provincia");

                        foreach (Anag_Indirizzi_Collaboratori indirizzo in collaboratore.Indirizzi)
                        {
                            //ListItem itemIndirizzi = new ListItem(indirizzo.Descrizione + " - " + indirizzo.Tipo + " " + indirizzo.Indirizzo + " " + indirizzo.NumeroCivico + " " + indirizzo.Cap + " " + indirizzo.Comune + " " + indirizzo.Provincia, indirizzo.Id.ToString());
                            //lbMod_Indirizzi.Items.Add(itemIndirizzi);
                            DataRow dr = dtIndirizzi.NewRow();
                            dr["id"] = indirizzo.Id.ToString();
                            dr["Descrizione"] = indirizzo.Descrizione;
                            dr["Tipo"] = indirizzo.Tipo;
                            dr["Indirizzo"] = indirizzo.Indirizzo;
                            dr["NumeroCivico"] = indirizzo.NumeroCivico;
                            dr["Cap"] = indirizzo.Cap;
                            dr["Comune"] = indirizzo.Comune;
                            dr["Provincia"] = indirizzo.Provincia;

                            dtIndirizzi.Rows.Add(dr);
                        }
                        gvMod_Indirizzi.DataSource = dtIndirizzi;
                        gvMod_Indirizzi.DataBind();
                    }
                    //if (collaboratore.Indirizzi != null && collaboratore.Indirizzi.Count > 0)
                    //{
                    //    lbMod_Indirizzi.Rows = collaboratore.Indirizzi.Count;
                    //}
                    //else
                    if (collaboratore.Indirizzi == null || collaboratore.Indirizzi.Count == 0)
                    {
                        //lbMod_Indirizzi.Rows = 1;
                        gvMod_Indirizzi.DataSource = null;
                    }

                    // EMAIL
                    DataTable dtEmail = new DataTable();
                    if (collaboratore.Email != null) {
                        dtEmail.Columns.Add("id");
                        dtEmail.Columns.Add("Descrizione");
                        dtEmail.Columns.Add("IndirizzoEmail");

                        foreach (Anag_Email_Collaboratori email in collaboratore.Email)
                        {
                            //ListItem itemEmail = new ListItem(email.Descrizione + " - " + email.IndirizzoEmail, email.Id.ToString());
                            //lbMod_Email.Items.Add(itemEmail);
                            DataRow dr = dtEmail.NewRow();
                            dr["id"] = email.Id.ToString();
                            dr["Descrizione"] = email.Descrizione;
                            dr["IndirizzoEmail"] = email.IndirizzoEmail;

                            dtEmail.Rows.Add(dr);
                        }
                        gvMod_Email.DataSource = dtEmail;
                        gvMod_Email.DataBind();
                    }
                    //if (collaboratore.Email != null && collaboratore.Email.Count > 0)
                    //{
                    //    //lbMod_Email.Rows = collaboratore.Email.Count;
                    //}
                    //else
                    if (collaboratore.Email == null || collaboratore.Email.Count == 0)
                    {
                        //lbMod_Email.Rows = 1;
                        gvMod_Documenti.DataSource = null;
                    }

                    // TELEFONI
                    DataTable dtTelefoni = new DataTable();
                    if (collaboratore.Telefoni != null) {
                        dtTelefoni.Columns.Add("id");
                        dtTelefoni.Columns.Add("Descrizione");
                        dtTelefoni.Columns.Add("Tipo");
                        dtTelefoni.Columns.Add("Pref_int");
                        dtTelefoni.Columns.Add("Pref_naz");
                        dtTelefoni.Columns.Add("Numero");
                        dtTelefoni.Columns.Add("Whatsapp");

                        foreach (Anag_Telefoni_Collaboratori telefono in collaboratore.Telefoni)
                        {
                            string whatsapp = "No";
                            if (telefono.Whatsapp) whatsapp = "Si";
                            //ListItem itemTelefono = new ListItem(telefono.Descrizione + " - " + telefono.Tipo + " - " + telefono.Pref_int + telefono.Pref_naz + telefono.Numero + " - whatsapp: " + whatsapp, telefono.Id.ToString());
                            //lbMod_Telefoni.Items.Add(itemTelefono);
                            DataRow dr = dtTelefoni.NewRow();
                            dr["id"] = telefono.Id.ToString();
                            dr["Descrizione"] = telefono.Descrizione;
                            dr["Tipo"] = telefono.Tipo;
                            dr["Pref_int"] = telefono.Pref_int;
                            dr["Pref_naz"] = telefono.Pref_naz;
                            dr["Numero"] = telefono.Numero;
                            dr["Whatsapp"] = whatsapp;

                            dtTelefoni.Rows.Add(dr);

                        }
                        gvMod_Telefoni.DataSource = dtTelefoni;
                        gvMod_Telefoni.DataBind();
                    }
                    //if (collaboratore.Telefoni != null && collaboratore.Telefoni.Count > 0)
                    //{
                    //    //lbMod_Telefoni.Rows = collaboratore.Telefoni.Count;
                    //}
                    //else
                    if (collaboratore.Telefoni == null || collaboratore.Telefoni.Count == 0)
                    {
                        //lbMod_Telefoni.Rows = 1;
                        gvMod_Telefoni.DataSource = null;
                    }

                    // DOCUMENTI
                    lblDoc.Text = "";
                    tbInsNumeroDocumento.Text = "";
                    cmbInsTipoDocumento.Text = "";
                    tbIdDocumentoDaModificare.Text = "";
                    phDocumenti.Visible = false;
                    DataTable dtDocumenti = new DataTable();
                    if (collaboratore.Documenti != null)
                    {
                        dtDocumenti.Columns.Add("id");
                        dtDocumenti.Columns.Add("TipoDocumento");
                        dtDocumenti.Columns.Add("NumeroDocumento");
                        dtDocumenti.Columns.Add("Documento");

                        foreach (Anag_Documenti_Collaboratori documento in collaboratore.Documenti)
                        {
                            //ListItem itemDocumento = new ListItem(documento.TipoDocumento + " - " + documento.NumeroDocumento , documento.Id.ToString());
                            //lbMod_Documenti.Items.Add(itemDocumento);
                            DataRow dr = dtDocumenti.NewRow();
                            dr["id"] = documento.Id.ToString();
                            dr["TipoDocumento"] = documento.TipoDocumento;
                            dr["NumeroDocumento"] = documento.NumeroDocumento;
                            dr["Documento"] = documento.PathDocumento;

                            dtDocumenti.Rows.Add(dr);
                        }
                        gvMod_Documenti.DataSource = dtDocumenti;
                        gvMod_Documenti.DataBind();
                    }
                    //if (collaboratore.Documenti != null && collaboratore.Documenti.Count > 0)
                    //{
                    //    lbMod_Documenti.Rows = collaboratore.Documenti.Count;
                    //}
                    //else
                    if (collaboratore.Documenti == null || collaboratore.Documenti.Count == 0)
                    {
                        //lbMod_Documenti.Rows = 1;
                        gvMod_Documenti.DataSource = null;
                    }

                    // IMMAGINE COLLABORATORE
                    if (string.IsNullOrEmpty(collaboratore.PathFoto))
                    {
                        imgCollaboratore.ImageUrl = ConfigurationManager.AppSettings["PATH_IMMAGINI_COLLABORATORI"] + ConfigurationManager.AppSettings["IMMAGINE_DUMMY_COLLABORATORE"];
                    }
                    else
                    {
                        imgCollaboratore.ImageUrl = ConfigurationManager.AppSettings["PATH_IMMAGINI_COLLABORATORI"] + collaboratore.PathFoto;
                    }
                }
                else
                {
                    log.Error(esito.Descrizione);
                    Session["ErrorPageText"] = esito.Descrizione;
                    string url = String.Format("~/pageError.aspx");
                    Response.Redirect(url, true);
                }
            }
        }

        private Anag_Collaboratori CreaOggettoSalvataggio(ref Esito esito)
        {
            Anag_Collaboratori collaboratore = new Anag_Collaboratori();

            if (string.IsNullOrEmpty((string)ViewState["idColl"])) {
                ViewState["idColl"] = 0;
            }

            collaboratore.Id = Convert.ToInt16(ViewState["idColl"].ToString());

            collaboratore.Nazione = BasePage.ValidaCampo(tbMod_Nazione, "", false, ref esito);
            collaboratore.Nome = BasePage.ValidaCampo(tbMod_Nome, "", false, ref esito);
            collaboratore.Cognome = BasePage.ValidaCampo(tbMod_Cognome, "", true, ref esito);
            collaboratore.ComuneNascita = BasePage.ValidaCampo(tbMod_ComuneNascita, "", false, ref esito);
            collaboratore.ComuneRiferimento = BasePage.ValidaCampo(tbMod_ComuneRiferimento, "", false, ref esito);
            collaboratore.RegioneRiferimento = BasePage.ValidaCampo(cmbMod_RegioneRiferimento, "", false, ref esito);
            collaboratore.DataNascita = BasePage.ValidaCampo(tbMod_DataNascita, DateTime.Now, false, ref esito);
            collaboratore.CodiceFiscale = BasePage.ValidaCampo(tbMod_CF, "", true, ref esito);
            collaboratore.Assunto = Convert.ToBoolean(BasePage.ValidaCampo(cbMod_Assunto, "false", false, ref esito));
            //collaboratore.Attivo = Convert.ToBoolean(BasePage.ValidaCampo(cbMod_Attivo, "true", false, ref esito));
            collaboratore.Attivo = true;
            collaboratore.NomeSocieta = BasePage.ValidaCampo(tbMod_NomeSocieta, "", false, ref esito);
            collaboratore.Iban = BasePage.ValidaCampo(tbMod_Iban, "", false, ref esito).ToUpper();
            collaboratore.Note = BasePage.ValidaCampo(tbMod_Note, "", false, ref esito);
            collaboratore.PartitaIva = BasePage.ValidaCampo(tbMod_PartitaIva, "", false, ref esito);
            collaboratore.ProvinciaNascita = BasePage.ValidaCampo(tbMod_ProvinciaNascita, "", false, ref esito);

            return collaboratore;
        }

        private void EditCollaboratoreVuoto()
        {
            Esito esito = new Esito();
            dettaglioModifica = "";
            lblDettaglioModifica.Text = dettaglioModifica;
            PulisciCampiDettaglio();
            // IMMAGINE COLLABORATORE
            imgCollaboratore.ImageUrl = ConfigurationManager.AppSettings["PATH_IMMAGINI_COLLABORATORI"] + ConfigurationManager.AppSettings["IMMAGINE_DUMMY_COLLABORATORE"];
            
        }

        protected void btnElimina_Click(object sender, EventArgs e)
        {
            Esito esito = new Esito();

            if (!string.IsNullOrEmpty((string)ViewState["idColl"]))
            {
                //esito = Anag_Collaboratori_BLL.Instance.EliminaCollaboratore(Convert.ToInt32(ViewState["idColl"].ToString()));
                esito = Anag_Collaboratori_BLL.Instance.RemoveCollaboratore(Convert.ToInt32(ViewState["idColl"].ToString()));
                if (esito.Codice != Esito.ESITO_OK)
                {
                    //panelErrore.Style.Remove("display");
                    //panelErrore.Style.Add("display", "block");
                    //lbl_MessaggioErrore.Text = esito.descrizione;
                    basePage.ShowError(esito.Descrizione);
                    AttivaDisattivaModificaAnagrafica(true);
                }
                else
                {
                    AttivaDisattivaModificaAnagrafica(true);
                    //btn_chiudi_Click(null, null);
                    pnlContainer.Visible = false;
                    SessionManager.ListaAnagraficheCollaboratori.Clear();
                    //SessionManager.ListaCittaCollaboratori.Clear();

                    btnRicercaCollaboratori_Click(null, null);
                }

            }
        }

        private void NascondiErroriValidazione()
        {
            //panelErrore.Style.Add("display", "none");

            tbMod_CF.CssClass = tbMod_CF.CssClass.Replace("erroreValidazione", "");
            tbMod_Cognome.CssClass = tbMod_Cognome.CssClass.Replace("erroreValidazione", "");
            tbMod_ComuneNascita.CssClass = tbMod_ComuneNascita.CssClass.Replace("erroreValidazione", "");
            tbMod_ComuneRiferimento.CssClass = tbMod_ComuneRiferimento.CssClass.Replace("erroreValidazione", "");
            tbMod_DataNascita.CssClass = tbMod_DataNascita.CssClass.Replace("erroreValidazione", "");
            tbMod_Nazione.CssClass = tbMod_Nazione.CssClass.Replace("erroreValidazione", "");
            tbMod_Nome.CssClass = tbMod_Nome.CssClass.Replace("erroreValidazione", "");
            tbMod_NomeSocieta.CssClass = tbMod_NomeSocieta.CssClass.Replace("erroreValidazione", "");
            tbMod_Iban.CssClass = tbMod_Iban.CssClass.Replace("erroreValidazione", "");
            tbMod_Note.CssClass = tbMod_Note.CssClass.Replace("erroreValidazione", "");
            tbMod_PartitaIva.CssClass = tbMod_PartitaIva.CssClass.Replace("erroreValidazione", "");
            tbMod_ProvinciaNascita.CssClass = tbMod_ProvinciaNascita.CssClass.Replace("erroreValidazione", "");
        }

        protected void btnConfermaInserimento_Click(object sender, EventArgs e)
        {
            // INSERISCO COLLABORATORE
            Esito esito = new Esito();
            Anag_Collaboratori collaboratore = CreaOggettoSalvataggio(ref esito);

            collaboratore.PathFoto = ConfigurationManager.AppSettings["IMMAGINE_DUMMY_COLLABORATORE"];

            if (esito.Codice != Esito.ESITO_OK)
            {
                //panelErrore.Style.Remove("display");
                //panelErrore.Style.Add("display", "block");
                //lbl_MessaggioErrore.Text = "Controllare i campi evidenziati";
                AttivaDisattivaModificaAnagrafica(false);
                basePage.ShowWarning("Controllare i campi evidenziati");
            }
            else
            {
                NascondiErroriValidazione();

                int iRet = Anag_Collaboratori_BLL.Instance.CreaCollaboratore(collaboratore, ref esito);
                if (iRet > 0)
                {
                    // UNA VOLTA INSERITO CORRETTAMENTE PUO' ESSERE MODIFICATO
                    hf_idColl.Value = iRet.ToString();
                    ViewState["idColl"] = hf_idColl.Value;
                    hf_tipoOperazione.Value = "VISUALIZZAZIONE";
                }

                if (esito.Codice != Esito.ESITO_OK)
                {
                    log.Error(esito.Descrizione);
                    //panelErrore.Style.Remove("display");
                    //panelErrore.Style.Add("display", "block");
                    //lbl_MessaggioErrore.Text = esito.descrizione;
                    basePage.ShowError(esito.Descrizione);
                }
                else
                {
                    SessionManager.ListaAnagraficheCollaboratori.Clear();
                    //SessionManager.ListaCittaCollaboratori.Clear();

                    EditCollaboratore_Click(null, null);
                }
            }

        }

        protected void btnEliminaQualifica_Click(object sender, EventArgs e)
        {
            //ELIMINO LA QUALIFICA SE SELEZIONATA
            if (lbMod_Qualifiche.SelectedIndex >= 0)
            {
                Esito esito = new Esito();
                try
                {
                    NascondiErroriValidazione();
                    ListItem item = lbMod_Qualifiche.Items[lbMod_Qualifiche.SelectedIndex];
                    string value = item.Value;
                    string qualificaSelezionata = item.Text;
                    esito = Anag_Qualifiche_Collaboratori_BLL.Instance.EliminaQualificaCollaboratore(Convert.ToInt32(item.Value), ((Anag_Utenti)Session[SessionManager.UTENTE]));

                    if (esito.Codice != Esito.ESITO_OK)
                    {
                        log.Error(esito.Descrizione);
                        basePage.ShowError(esito.Descrizione);
                    }
                    else
                    {
                        SessionManager.ListaQualificheCollaboratori.Clear();
                        
                        EditCollaboratore();
                    }
                }
                catch (Exception ex)
                {
                    log.Error("btnEliminaQualifica_Click", ex);

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

        protected void btnEliminaEmail_Click(object sender, EventArgs e)
        {

            //ELIMINO L'INDIRIZZO EMAIL SE SELEZIONATO
            if (!string.IsNullOrEmpty(tbIdEmailDaModificare.Text.Trim()))
            {
                Esito esito = new Esito();
                try
                {
                    NascondiErroriValidazione();
                    esito = Anag_Email_Collaboratori_BLL.Instance.EliminaEmailCollaboratore(Convert.ToInt32(tbIdEmailDaModificare.Text.Trim()), ((Anag_Utenti)Session[SessionManager.UTENTE]));

                    if (esito.Codice != Esito.ESITO_OK)
                    {
                        //panelErrore.Style.Add("display", "block");
                        //lbl_MessaggioErrore.Text = esito.descrizione;
                        basePage.ShowError(esito.Descrizione);
                    }
                    else
                    {
                        tbInsEmail.Text = "";
                        tbInsPrioritaEmail.Text = "";
                        tbInsTipoEmail.Text = "";
                        tbIdEmailDaModificare.Text = "";

                        btnModificaEmail.Visible = false;
                        btnInserisciEmail.Visible = true;

                        EditCollaboratore();
                    }
                }
                catch (Exception ex)
                {
                    log.Error("btnEliminaDocumento_Click", ex);
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

        protected void btnEliminaIndirizzo_Click(object sender, EventArgs e)
        {
            //ELIMINO L'INDIRIZZO SE SELEZIONATO
            if (!string.IsNullOrEmpty(tbIdIndirizzoDaModificare.Text.Trim()))
            {
                Esito esito = new Esito();
                try
                {
                    NascondiErroriValidazione();
                    esito = Anag_Indirizzi_Collaboratori_BLL.Instance.EliminaIndirizziCollaboratore(Convert.ToInt32(tbIdIndirizzoDaModificare.Text.Trim()), ((Anag_Utenti)Session[SessionManager.UTENTE]));

                    if (esito.Codice != Esito.ESITO_OK)
                    {
                        basePage.ShowError(esito.Descrizione);
                    }
                    else
                    {
                        tbInsIndirizzoIndirizzo.Text = "";
                        tbInsPrioritaIndirizzo.Text = "1";
                        cmbInsTipoIndirizzo.Text = "";
                        tbInsComuneIndirizzo.Text = "";
                        tbInsProvinciaIndirizzo.Text = "";
                        tbInsCapIndirizzo.Text = "";
                        tbInsCivicoIndirizzo.Text = "";
                        tbInsDescrizioneIndirizzo.Text = "";
                        btnModificaIndirizzo.Visible = false;
                        btnInserisciIndirizzo.Visible = true;
                        tbIdIndirizzoDaModificare.Text = "";

                        EditCollaboratore();
                    }
                }
                catch (Exception ex)
                {
                    log.Error("btnEliminaIndirizzo_Click", ex);
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

        protected void btnEliminaTelefono_Click(object sender, EventArgs e) {
            //ELIMINO IL TELEFONO SE SELEZIONATO

            if (!string.IsNullOrEmpty(tbIdTelefonoDaModificare.Text.Trim()))
            {
                Esito esito = new Esito();
                try
                {
                    NascondiErroriValidazione();
                    esito = Anag_Telefoni_Collaboratori_BLL.Instance.EliminaTelefonoCollaboratore(Convert.ToInt32(tbIdTelefonoDaModificare.Text.Trim()), ((Anag_Utenti)Session[SessionManager.UTENTE]));

                    if (esito.Codice != Esito.ESITO_OK)
                    {
                        //panelErrore.Style.Add("display", "block");
                        //lbl_MessaggioErrore.Text = esito.descrizione;
                        basePage.ShowError(esito.Descrizione);
                    }
                    else
                    {
                        tbInsPrefIntTelefono.Text = "+39";
                        tbInsPrefNazTelefono.Text = "";
                        tbInsNumeroTelefono.Text = "";
                        cmbInsTipoTelefono.Text = "";
                        cbInsWhatsappTelefono.Checked = false;

                        btnModificaTelefono.Visible = false;
                        btnInserisciTelefono.Visible = true;

                        EditCollaboratore();
                    }
                }
                catch (Exception ex)
                {
                    log.Error("btnEliminaDocumento_Click", ex);
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

        protected void btnEliminaDocumento_Click(object sender, EventArgs e)
        {
            //ELIMINO IL DOCUMENTO SE SELEZIONATO
            if (!string.IsNullOrEmpty(tbIdDocumentoDaModificare.Text.Trim()))
            {
                Esito esito = new Esito();
                try
                {
                    NascondiErroriValidazione();
                    esito = Anag_Documenti_Collaboratori_BLL.Instance.EliminaDocumentoCollaboratore(Convert.ToInt32(tbIdDocumentoDaModificare.Text.Trim()), ((Anag_Utenti)Session[SessionManager.UTENTE]));

                    if (esito.Codice != Esito.ESITO_OK)
                    {
                        basePage.ShowError(esito.Descrizione);
                    }
                    else
                    {
                        tbInsNumeroDocumento.Text = "";
                        cmbInsTipoDocumento.Text = "";
                        tbIdDocumentoDaModificare.Text = "";

                        btnModificaDocumento.Visible = false;
                        btnInserisciDocumento.Visible = true;
                        btnCaricaDocumento.Visible = false;
                        fuDoc.Visible = false;

                        EditCollaboratore();
                    }
                }
                catch (Exception ex)
                {
                    log.Error("btnEliminaDocumento_Click", ex);
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
        
        protected void btnConfermaInserimentoQualifica_Click(object sender, EventArgs e)
        {
            //INSERISCO LA QUALIFICA SE SELEZIONATA
            if (ddlQualificheDaAggiungere.SelectedIndex >= 0)
            {
                Esito esito = new Esito();
                try
                {
                    NascondiErroriValidazione();
                    ListItem item = ddlQualificheDaAggiungere.Items[ddlQualificheDaAggiungere.SelectedIndex];
                    string value = item.Value;
                    string qualificaSelezionata = item.Text;

                    Anag_Qualifiche_Collaboratori nuovaQualifica = new Anag_Qualifiche_Collaboratori
                    {
                        Id_collaboratore = Convert.ToInt32(ViewState["idColl"]),
                        Priorita = Convert.ToInt16(tbInsPrioritaQualifica.Text.Trim()),
                        Qualifica = qualificaSelezionata.Replace("'", "''"),
                        Attivo = true,
                        Descrizione = ""
                    };
                    int iNuovaQualifica = Anag_Qualifiche_Collaboratori_BLL.Instance.CreaQualificaCollaboratore(nuovaQualifica, ((Anag_Utenti)Session[SessionManager.UTENTE]), ref esito);

                    if (esito.Codice != Esito.ESITO_OK)
                    {
                        basePage.ShowError(esito.Descrizione);
                    }
                    else
                    {
                        SessionManager.ListaQualificheCollaboratori.Clear();
                        EditCollaboratore();
                    }
                }
                catch (Exception ex)
                {
                    log.Error("btnConfermaInserimentoQualifica_Click", ex);
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

        protected void btnConfermaInserimentoEmail_Click(object sender, EventArgs e)
        {
            //INSERISCO L'E-MAIL
            if (!string.IsNullOrEmpty(tbInsEmail.Text) && basePage.ValidaIndirizzoEmail(tbInsEmail.Text))
            {
                Esito esito = new Esito();
                try
                {
                    NascondiErroriValidazione();

                    Anag_Email_Collaboratori nuovaEmail = new Anag_Email_Collaboratori
                    {
                        Id_collaboratore = Convert.ToInt32(ViewState["idColl"]),
                        Priorita = Convert.ToInt16(tbInsPrioritaEmail.Text.Trim()),
                        IndirizzoEmail = tbInsEmail.Text.Trim(),
                        Attivo = true,
                        Descrizione = tbInsTipoEmail.Text.Trim()
                    };
                    int iNuovaEmail = Anag_Email_Collaboratori_BLL.Instance.CreaEmailCollaboratore(nuovaEmail, ((Anag_Utenti)Session[SessionManager.UTENTE]), ref esito);

                    if (esito.Codice != Esito.ESITO_OK)
                    {
                        basePage.ShowError(esito.Descrizione);
                    }
                    else
                    {
                        tbInsEmail.Text = "";
                        tbInsPrioritaEmail.Text = "1";
                        tbInsTipoEmail.Text = "";
                        EditCollaboratore();
                    }
                }
                catch (Exception ex)
                {
                    log.Error("btnConfermaInserimentoEmail_Click", ex);
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

        protected void btnConfermaInserimentoIndirizzo_Click(object sender, EventArgs e)
        {
            //INSERISCO L'INDIRIZZO
            if (!string.IsNullOrEmpty(tbInsIndirizzoIndirizzo.Text))
            {
                Esito esito = new Esito();
                try
                {
                    NascondiErroriValidazione();

                    Anag_Indirizzi_Collaboratori nuovoIndirizzo = new Anag_Indirizzi_Collaboratori
                    {
                        Id_Collaboratore = Convert.ToInt32(ViewState["idColl"]),
                        Priorita = Convert.ToInt16(tbInsPrioritaIndirizzo.Text.Trim()),
                        Indirizzo = tbInsIndirizzoIndirizzo.Text.Trim(),
                        Attivo = true,
                        Descrizione = tbInsDescrizioneIndirizzo.Text.Trim(),
                        //nuovoIndirizzo.Tipo = tbInsTipoIndirizzo.Text.Trim();
                        Tipo = cmbInsTipoIndirizzo.Text.Trim(),
                        Comune = tbInsComuneIndirizzo.Text.Trim(),
                        Provincia = tbInsProvinciaIndirizzo.Text.Trim(),
                        Cap = tbInsCapIndirizzo.Text.Trim(),
                        NumeroCivico = tbInsCivicoIndirizzo.Text.Trim()
                    };
                    int iNuovoIndirizzo = Anag_Indirizzi_Collaboratori_BLL.Instance.CreaIndirizziCollaboratore(nuovoIndirizzo, ((Anag_Utenti)Session[SessionManager.UTENTE]), ref esito);

                    if (esito.Codice != Esito.ESITO_OK)
                    {
                        basePage.ShowError(esito.Descrizione);
                    }
                    else
                    {
                        tbInsIndirizzoIndirizzo.Text = "";
                        tbInsPrioritaIndirizzo.Text = "1";
                        tbInsDescrizioneIndirizzo.Text = "";
                        tbInsCapIndirizzo.Text = "";
                        tbInsCivicoIndirizzo.Text = "";
                        tbInsComuneIndirizzo.Text = "";
                        tbInsProvinciaIndirizzo.Text = "";
                        //tbInsTipoIndirizzo.Text = "";
                        cmbInsTipoIndirizzo.Text = "";
                        EditCollaboratore();
                    }
                }
                catch (Exception ex)
                {
                    log.Error("btnConfermaInserimentoIndirizzo_Click", ex);
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

        protected void btnConfermaInserimentoTelefono_Click(object sender, EventArgs e) {
            //INSERISCO IL TELEFONO
            if (!string.IsNullOrEmpty(tbInsNumeroTelefono.Text) && !string.IsNullOrEmpty(tbInsPrefNazTelefono.Text))
            {
                Esito esito = new Esito();
                try
                {
                    NascondiErroriValidazione();

                    Anag_Telefoni_Collaboratori nuovoTelefono = new Anag_Telefoni_Collaboratori
                    {
                        Id_collaboratore = Convert.ToInt32(ViewState["idColl"]),
                        Priorita = Convert.ToInt16(tbInsPrioritaIndirizzo.Text.Trim()),
                        Attivo = true,
                        Descrizione = tbInsDescrizioneTelefono.Text.Trim(),
                        Tipo = cmbInsTipoTelefono.Text.Trim(),
                        Pref_int = tbInsPrefIntTelefono.Text.Trim(),
                        Pref_naz = tbInsPrefNazTelefono.Text.Trim(),
                        Numero = tbInsNumeroTelefono.Text.Trim(),
                        Whatsapp = cbInsWhatsappTelefono.Checked
                    };

                    int iNuovoTelefono = Anag_Telefoni_Collaboratori_BLL.Instance.CreaTelefonoCollaboratore(nuovoTelefono, ((Anag_Utenti)Session[SessionManager.UTENTE]), ref esito);

                    if (esito.Codice != Esito.ESITO_OK)
                    {
                        basePage.ShowError(esito.Descrizione);
                    }
                    else
                    {
                        tbInsPrioritaTelefono.Text = "";
                        tbInsDescrizioneTelefono.Text = "";
                        //tbInsTipoIndirizzo.Text = "";
                        cmbInsTipoIndirizzo.Text = "";
                        tbInsPrefIntTelefono.Text = "";
                        tbInsPrefNazTelefono.Text = "";
                        tbInsNumeroTelefono.Text = "";
                        cbInsWhatsappTelefono.Checked=false;
                        EditCollaboratore();
                    }
                }
                catch (Exception ex)
                {
                    log.Error("btnConfermaInserimentoTelefono_Click", ex);
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

        protected void btnConfermaInserimentoDocumento_Click(object sender, EventArgs e)
        {
            //INSERISCO IL DOCUMENTO
            if (!string.IsNullOrEmpty(tbInsNumeroDocumento.Text))
            {
                Esito esito = new Esito();
                try
                {
                    NascondiErroriValidazione();

                    Anag_Documenti_Collaboratori nuovoDocumento = new Anag_Documenti_Collaboratori
                    {
                        Id_collaboratore = Convert.ToInt32(ViewState["idColl"]),
                        Attivo = true,
                        TipoDocumento = cmbInsTipoDocumento.Text.Trim(),
                        NumeroDocumento = tbInsNumeroDocumento.Text.Trim(),
                        PathDocumento = ""
                    };

                    int iNuovoDocumento = Anag_Documenti_Collaboratori_BLL.Instance.CreaDocumentoCollaboratore(nuovoDocumento, ((Anag_Utenti)Session[SessionManager.UTENTE]), ref esito);

                    if (esito.Codice != Esito.ESITO_OK)
                    {
                        basePage.ShowError(esito.Descrizione);
                    }
                    else
                    {
                        tbInsNumeroDocumento.Text = "";
                        cmbInsTipoDocumento.Text = "";
                        EditCollaboratore();
                    }
                }
                catch (Exception ex)
                {
                    log.Error("btnConfermaInserimentoDocumento_Click", ex);
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

        protected void btnConfermaModificaEmail_Click(object sender, EventArgs e)
        {
            //MODIFICO L'E-MAIL
            if (!string.IsNullOrEmpty(tbInsEmail.Text) && basePage.ValidaIndirizzoEmail(tbInsEmail.Text) && !string.IsNullOrEmpty(tbIdEmailDaModificare.Text))
            {
                Esito esito = new Esito();
                try
                {
                    NascondiErroriValidazione();

                    Anag_Email_Collaboratori nuovaEmail = new Anag_Email_Collaboratori
                    {
                        Id = Convert.ToInt32(tbIdEmailDaModificare.Text),
                        Id_collaboratore = Convert.ToInt32(ViewState["idColl"]),
                        Priorita = Convert.ToInt16(tbInsPrioritaEmail.Text.Trim()),
                        IndirizzoEmail = tbInsEmail.Text.Trim(),
                        Attivo = true,
                        Descrizione = tbInsTipoEmail.Text.Trim()
                    };
                    esito = Anag_Email_Collaboratori_BLL.Instance.AggiornaEmailCollaboratore(nuovaEmail, ((Anag_Utenti)Session[SessionManager.UTENTE]));

                    btnModificaEmail.Visible = false;
                    btnInserisciEmail.Visible = true;

                    if (esito.Codice != Esito.ESITO_OK)
                    {
                        basePage.ShowError(esito.Descrizione);
                    }
                    else
                    {
                        tbInsEmail.Text = "";
                        tbInsPrioritaEmail.Text = "1";
                        tbInsTipoEmail.Text = "";
                        EditCollaboratore();
                    }
                }
                catch (Exception ex)
                {
                    log.Error("btnConfermaMdoficaEmail_Click", ex);
                    btnModificaEmail.Visible = false;
                    btnInserisciEmail.Visible = true;
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

        protected void btnConfermaModificaIndirizzo_Click(object sender, EventArgs e)
        {
            //MODIFICO L'INDIRIZZO
            if (!string.IsNullOrEmpty(tbInsIndirizzoIndirizzo.Text) && !string.IsNullOrEmpty(tbInsComuneIndirizzo.Text) && !string.IsNullOrEmpty(tbIdIndirizzoDaModificare.Text))
            {
                Esito esito = new Esito();
                try
                {
                    NascondiErroriValidazione();

                    Anag_Indirizzi_Collaboratori nuovoIndirizzo = new Anag_Indirizzi_Collaboratori
                    {
                        Id = Convert.ToInt32(tbIdIndirizzoDaModificare.Text),
                        Id_Collaboratore = Convert.ToInt32(ViewState["idColl"]),
                        Priorita = Convert.ToInt16(tbInsPrioritaIndirizzo.Text.Trim()),
                        Indirizzo = tbInsIndirizzoIndirizzo.Text.Trim(),
                        Attivo = true,
                        Descrizione = tbInsDescrizioneIndirizzo.Text.Trim(),
                        //nuovoIndirizzo.Tipo = tbInsTipoIndirizzo.Text.Trim();
                        Tipo = cmbInsTipoIndirizzo.Text.Trim(),
                        Nazione = "Italia",
                        NumeroCivico = tbInsCivicoIndirizzo.Text.Trim(),
                        Cap = tbInsCapIndirizzo.Text.Trim(),
                        Comune = tbInsComuneIndirizzo.Text.Trim(),
                        Provincia = tbInsProvinciaIndirizzo.Text.Trim()
                    };

                    esito = Anag_Indirizzi_Collaboratori_BLL.Instance.AggiornaIndirizziCollaboratore(nuovoIndirizzo, ((Anag_Utenti)Session[SessionManager.UTENTE]));

                    btnModificaIndirizzo.Visible = false;
                    btnInserisciIndirizzo.Visible = true;

                    if (esito.Codice != Esito.ESITO_OK)
                    {
                        basePage.ShowError(esito.Descrizione);
                    }
                    else
                    {
                        tbIdIndirizzoDaModificare.Text = "";
                        tbInsIndirizzoIndirizzo.Text = "";
                        tbInsPrioritaIndirizzo.Text = "1";
                        //tbInsTipoIndirizzo.Text = "";
                        cmbInsTipoIndirizzo.Text = "";
                        tbInsDescrizioneIndirizzo.Text = "";
                        tbInsComuneIndirizzo.Text = "";
                        tbInsProvinciaIndirizzo.Text = "";
                        tbInsCapIndirizzo.Text = "";
                        tbInsCivicoIndirizzo.Text = "";
                        EditCollaboratore();
                    }
                }
                catch (Exception ex)
                {
                    log.Error("btnConfermaMdoficaIndirizzo_Click", ex);
                    btnModificaIndirizzo.Visible = false;
                    btnInserisciIndirizzo.Visible = true;
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

        protected void btnConfermaModificaTelefono_Click(object sender, EventArgs e) {
            //MODIFICO L'INDIRIZZO
            if (!string.IsNullOrEmpty(tbInsNumeroTelefono.Text) && !string.IsNullOrEmpty(tbInsPrefNazTelefono.Text) && !string.IsNullOrEmpty(tbIdTelefonoDaModificare.Text))
            {
                Esito esito = new Esito();
                try
                {
                    NascondiErroriValidazione();

                    Anag_Telefoni_Collaboratori nuovoTelefono = new Anag_Telefoni_Collaboratori
                    {
                        Id = Convert.ToInt32(tbIdTelefonoDaModificare.Text),
                        Id_collaboratore = Convert.ToInt32(ViewState["idColl"]),
                        Priorita = Convert.ToInt16(tbInsPrioritaTelefono.Text.Trim()),
                        Pref_int = tbInsPrefIntTelefono.Text.Trim(),
                        Pref_naz = tbInsPrefNazTelefono.Text.Trim(),
                        Tipo = cmbInsTipoTelefono.Text.Trim(),
                        Numero = tbInsNumeroTelefono.Text.Trim(),
                        Whatsapp = cbInsWhatsappTelefono.Checked,
                        Attivo = true,
                        Descrizione = tbInsDescrizioneTelefono.Text.Trim()
                    };

                    esito = Anag_Telefoni_Collaboratori_BLL.Instance.AggiornaTelefonoCollaboratore(nuovoTelefono, ((Anag_Utenti)Session[SessionManager.UTENTE]));

                    btnModificaTelefono.Visible = false;
                    btnInserisciTelefono.Visible = true;

                    if (esito.Codice != Esito.ESITO_OK)
                    {
                        basePage.ShowError(esito.Descrizione);
                    }
                    else
                    {
                        tbIdTelefonoDaModificare.Text = "";
                        tbInsNumeroTelefono.Text = "";
                        tbInsPrefIntTelefono.Text = "";
                        tbInsPrioritaTelefono.Text = "1";
                        tbInsPrefNazTelefono.Text = "";
                        cmbInsTipoTelefono.Text = "";
                        tbInsDescrizioneTelefono.Text = "";
                        cbInsWhatsappTelefono.Checked = false;
                        EditCollaboratore();
                    }
                }
                catch (Exception ex)
                {
                    log.Error("btnConfermaMdoficaTelefono_Click", ex);
                    btnModificaTelefono.Visible = false;
                    btnInserisciTelefono.Visible = true;
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

        protected void btnConfermaModificaDocumento_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbInsNumeroDocumento.Text) && !string.IsNullOrEmpty(tbIdDocumentoDaModificare.Text))
            {
                Esito esito = new Esito();
                try
                {
                    NascondiErroriValidazione();

                    Anag_Documenti_Collaboratori nuovoDocumento = new Anag_Documenti_Collaboratori
                    {
                        Id = Convert.ToInt32(tbIdDocumentoDaModificare.Text),
                        Id_collaboratore = Convert.ToInt32(ViewState["idColl"]),
                        TipoDocumento = cmbInsTipoDocumento.Text.Trim(),
                        NumeroDocumento = tbInsNumeroDocumento.Text.Trim(),
                        PathDocumento = "",
                        Attivo = true
                    };

                    esito = Anag_Documenti_Collaboratori_BLL.Instance.AggiornaDocumentoCollaboratore(nuovoDocumento, ((Anag_Utenti)Session[SessionManager.UTENTE]));

                    btnModificaDocumento.Visible = false;
                    btnInserisciDocumento.Visible = true;

                    if (esito.Codice != Esito.ESITO_OK)
                    {
                        basePage.ShowError(esito.Descrizione);
                    }
                    else
                    {
                        tbIdDocumentoDaModificare.Text = "";
                        tbInsNumeroDocumento.Text = "";
                        cmbInsTipoDocumento.Text = "";

                        EditCollaboratore();
                    }
                }
                catch (Exception ex)
                {
                    log.Error("btnConfermaMdoficaDocumento_Click", ex);
                    btnModificaDocumento.Visible = false;
                    btnInserisciDocumento.Visible = true;
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

        protected void btnAnnullaIndirizzo_Click(object sender, EventArgs e)
        {
            tbIdIndirizzoDaModificare.Text = "";
            tbInsIndirizzoIndirizzo.Text = "";
            tbInsPrioritaIndirizzo.Text = "1";
            cmbInsTipoIndirizzo.Text = "";
            tbInsDescrizioneIndirizzo.Text = "";
            tbInsComuneIndirizzo.Text = "";
            tbInsProvinciaIndirizzo.Text = "";
            tbInsCapIndirizzo.Text = "";
            tbInsCivicoIndirizzo.Text = "";

            btnModificaIndirizzo.Visible = false;
            btnInserisciIndirizzo.Visible = true;
        }

        protected void btnAnnullaTelefono_Click(object sender, EventArgs e)
        {
            tbInsPrefIntTelefono.Text = "+39";
            tbInsPrefNazTelefono.Text = "";
            tbInsNumeroTelefono.Text = "";
            cmbInsTipoTelefono.Text = "";
            
            cbInsWhatsappTelefono.Checked = false;

            btnModificaTelefono.Visible = false;
            btnInserisciTelefono.Visible = true;
        }

        protected void btnAnnullaEmail_Click(object sender, EventArgs e)
        {
            tbInsEmail.Text = "";
            tbInsPrioritaEmail.Text = "1";
            tbInsTipoEmail.Text = "";
            tbIdEmailDaModificare.Text = "";

            btnModificaEmail.Visible = false;
            btnInserisciEmail.Visible = true;
        }

        protected void btnAnnullaDocumento_Click(object sender, EventArgs e)
        {
            tbInsNumeroDocumento.Text = "";
            cmbInsTipoDocumento.Text = "";

            btnModificaDocumento.Visible = false;
            btnInserisciDocumento.Visible = true;
            btnCaricaDocumento.Visible = false;
            fuDoc.Visible = false;
            lblDoc.Text = "";
        }

        protected void btnApriQualifiche_Click(object sender, EventArgs e)
        {
            if (phQualifiche.Visible)
            {
                phQualifiche.Visible = false;
            }
            else
            {
                phQualifiche.Visible = true;
                
            }
        }

        protected void btnApriEmail_Click(object sender, EventArgs e)
        {
            if (phEmail.Visible)
            {
                phEmail.Visible = false;
            }
            else
            {
                phEmail.Visible = true;
            }
        }

        protected void btnApriIndirizzi_Click(object sender, EventArgs e)
        {
            if (phIndirizzi.Visible)
            {
                phIndirizzi.Visible = false;
            }
            else
            {
                phIndirizzi.Visible = true;
            }
        }

        protected void btnApriTelefoni_Click(object sender, EventArgs e)
        {
            if (phTelefoni.Visible)
            {
                phTelefoni.Visible = false;
            }
            else
            {
                phTelefoni.Visible = true;
            }
        }

        protected void btnApriDocumenti_Click(object sender, EventArgs e)
        {
            if (phDocumenti.Visible)
            {
                phDocumenti.Visible = false;
            }
            else
            {
                phDocumenti.Visible = true;
            }
        }

        protected void gvMod_Telefoni_RigaSelezionata(object sender, EventArgs e)
        {
            //SCARICO IL TELEFONO SE SELEZIONATO
            if (gvMod_Telefoni.SelectedIndex >= 0)
            {
                Esito esito = new Esito();
                try
                {
                    NascondiErroriValidazione();

                    string telefonoSelezionato = gvMod_Telefoni.Rows[gvMod_Telefoni.SelectedIndex].Cells[1].Text;
                    
                    Anag_Telefoni_Collaboratori telefono = Anag_Telefoni_Collaboratori_BLL.Instance.getTelefonoById(ref esito,Convert.ToInt32(telefonoSelezionato));

                    if (esito.Codice != Esito.ESITO_OK)
                    {
                        btnModificaTelefono.Visible = false;
                        btnInserisciTelefono.Visible = true;
                        tbIdTelefonoDaModificare.Text = "";

                        basePage.ShowError(esito.Descrizione);
                    }
                    else
                    {
                        btnModificaTelefono.Visible = true;
                        btnInserisciTelefono.Visible = false;
                        tbIdTelefonoDaModificare.Text = telefono.Id.ToString();

                        tbInsNumeroTelefono.Text = telefono.Numero;
                        tbInsPrefIntTelefono.Text = telefono.Pref_int;
                        tbInsPrefNazTelefono.Text = telefono.Pref_naz;
                        tbInsPrioritaTelefono.Text = telefono.Priorita.ToString();
                        tbInsDescrizioneTelefono.Text = telefono.Descrizione;
                        cbInsWhatsappTelefono.Checked = telefono.Whatsapp;
                        ListItem trovati = cmbInsTipoTelefono.Items.FindByText(telefono.Tipo);
                        if (trovati != null)
                        {
                            cmbInsTipoTelefono.SelectedValue = trovati.Value;
                        }
                        else
                        {
                            cmbInsTipoTelefono.Text = "";
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("gvMod_Telefoni_RigaSelezionata", ex);
                    btnModificaTelefono.Visible = false;
                    btnInserisciTelefono.Visible = true;
                    tbIdTelefonoDaModificare.Text = "";
                    if (esito.Codice == Esito.ESITO_OK)
                    {
                        esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                        esito.Descrizione = ex.Message + Environment.NewLine + ex.StackTrace;
                    }
                    basePage.ShowError(ex.Message);
                }
            }
        }
        protected void gvMod_Indirizzi_RigaSelezionata(object sender, EventArgs e)
        {
            //SCARICO L'INDIRIZZO SE SELEZIONATO
            if (gvMod_Indirizzi.SelectedIndex >= 0)
            {
                Esito esito = new Esito();
                try
                {
                    NascondiErroriValidazione();

                    string indirizzoSelezionato = gvMod_Indirizzi.Rows[gvMod_Indirizzi.SelectedIndex].Cells[1].Text;
                    
                    Anag_Indirizzi_Collaboratori indirizzo = Anag_Indirizzi_Collaboratori_BLL.Instance.getIndirizzoById(ref esito, Convert.ToInt32(indirizzoSelezionato));

                    if (esito.Codice != Esito.ESITO_OK)
                    {
                        btnModificaIndirizzo.Visible = false;
                        btnInserisciIndirizzo.Visible = true;
                        tbIdIndirizzoDaModificare.Text = "";
                        
                        basePage.ShowError(esito.Descrizione);
                    }
                    else
                    {
                        btnModificaIndirizzo.Visible = true;
                        btnInserisciIndirizzo.Visible = false;
                        tbIdIndirizzoDaModificare.Text = indirizzo.Id.ToString();

                        ListItem trovati = cmbInsTipoIndirizzo.Items.FindByText(indirizzo.Tipo);
                        if (trovati != null)
                        {
                            cmbInsTipoIndirizzo.SelectedValue = trovati.Value;
                        }
                        else
                        {
                            cmbInsTipoIndirizzo.Text = "";
                        }

                        tbInsIndirizzoIndirizzo.Text = indirizzo.Indirizzo;
                        tbInsCapIndirizzo.Text = indirizzo.Cap;
                        tbInsCivicoIndirizzo.Text = indirizzo.NumeroCivico;
                        tbInsComuneIndirizzo.Text = indirizzo.Comune;
                        tbInsProvinciaIndirizzo.Text = indirizzo.Provincia;
                        tbInsPrioritaIndirizzo.Text = indirizzo.Priorita.ToString();
                        tbInsDescrizioneIndirizzo.Text = indirizzo.Descrizione;
                    }
                }
                catch (Exception ex)
                {
                    log.Error("gvMod_Indirizzi_RigaSelezionata", ex);
                    btnModificaIndirizzo.Visible = false;
                    btnInserisciIndirizzo.Visible = true;
                    tbIdIndirizzoDaModificare.Text = "";
                    if (esito.Codice == Esito.ESITO_OK)
                    {
                        esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                        esito.Descrizione = ex.Message + Environment.NewLine + ex.StackTrace;
                    }
                    basePage.ShowError(ex.Message);
                }
            }
        }
        protected void gvMod_Email_RigaSelezionata(object sender, EventArgs e)
        {
            //SCARICO LA MAIL SE SELEZIONATA
            if (gvMod_Email.SelectedIndex >= 0)
            {
                Esito esito = new Esito();
                try
                {
                    NascondiErroriValidazione();

                    string emailSelezionata = gvMod_Email.Rows[gvMod_Email.SelectedIndex].Cells[1].Text;
                    
                    Anag_Email_Collaboratori email = Anag_Email_Collaboratori_BLL.Instance.getEmailById(Convert.ToInt32(emailSelezionata), ref esito);

                    if (esito.Codice != Esito.ESITO_OK)
                    {
                        btnModificaEmail.Visible = false;
                        btnInserisciEmail.Visible = true;
                        tbIdEmailDaModificare.Text = "";

                        basePage.ShowError(esito.Descrizione);
                    }
                    else
                    {
                        btnModificaEmail.Visible = true;
                        btnInserisciEmail.Visible = false;
                        tbIdEmailDaModificare.Text = email.Id.ToString();

                        tbInsEmail.Text = email.IndirizzoEmail;
                        tbInsPrioritaEmail.Text = email.Priorita.ToString();
                        tbInsTipoEmail.Text = email.Descrizione;
                    }
                }
                catch (Exception ex)
                {
                    log.Error("gvMod_Email_RigaSelezionata", ex);
                    btnModificaEmail.Visible = false;
                    btnInserisciEmail.Visible = true;
                    tbIdEmailDaModificare.Text = "";
                    if (esito.Codice == Esito.ESITO_OK)
                    {
                        esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                        esito.Descrizione = ex.Message + Environment.NewLine + ex.StackTrace;
                    }
                    basePage.ShowError(ex.Message);
                }
            }

        }
        protected void gvMod_Documenti_RigaSelezionata(object sender, EventArgs e)
        {
            //SCARICO IL REFERENTE SE SELEZIONATO
            if (gvMod_Documenti.SelectedIndex >= 0)
            {
                Esito esito = new Esito();
                try
                {
                    NascondiErroriValidazione();

                    string documentoSelezionato = gvMod_Documenti.Rows[gvMod_Documenti.SelectedIndex].Cells[2].Text;
                    
                    Anag_Documenti_Collaboratori documento = Anag_Documenti_Collaboratori_BLL.Instance.getDocumentoById(ref esito, Convert.ToInt32(documentoSelezionato));

                    if (esito.Codice != Esito.ESITO_OK)
                    {
                        btnModificaDocumento.Visible = false;
                        btnInserisciDocumento.Visible = true;
                        btnCaricaDocumento.Visible = false;
                        fuDoc.Visible = false;
                        tbIdDocumentoDaModificare.Text = "";

                        basePage.ShowError(esito.Descrizione);
                    }
                    else
                    {
                        btnModificaDocumento.Visible = true;
                        btnInserisciDocumento.Visible = false;
                        btnCaricaDocumento.Visible = true;
                        fuDoc.Visible = true;
                        tbIdDocumentoDaModificare.Text = documento.Id.ToString();
                        ListItem trovati = cmbInsTipoDocumento.Items.FindByText(documento.TipoDocumento);
                        if (trovati != null)
                        {
                            cmbInsTipoDocumento.Text = documento.TipoDocumento;
                        }
                        else
                        {
                            cmbInsTipoDocumento.Text = "";
                        }
                        tbInsNumeroDocumento.Text = documento.NumeroDocumento;
                    }
                }
                catch (Exception ex)
                {
                    log.Error("gvMod_Documenti_RigaSelezionata", ex);
                    btnModificaDocumento.Visible = false;
                    btnInserisciDocumento.Visible = true;
                    btnCaricaDocumento.Visible = false;
                    fuDoc.Visible = false;
                    tbIdDocumentoDaModificare.Text = "";
                    if (esito.Codice == Esito.ESITO_OK)
                    {
                        esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                        esito.Descrizione = ex.Message + Environment.NewLine + ex.StackTrace;
                    }
                    basePage.ShowError(ex.Message);
                }
            }


        }
        protected void gvMod_Documenti_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // PRENDO L'ID DEL DOCUMENTO SELEZIONATO
                string pathDocumento = e.Row.Cells[5].Text.Trim();
                ImageButton myButton = e.Row.FindControl("btnOpenDoc") as ImageButton;
                if (!string.IsNullOrEmpty(pathDocumento) && !pathDocumento.Equals("&nbsp;")) {
                    string pathCompleto = ConfigurationManager.AppSettings["PATH_DOCUMENTI_COLLABORATORI"].Replace("~", "") + pathDocumento;
                    //string pathCompleto = "/Images/DOCUMENTI/" + pathDocumento;
                    myButton.Attributes.Add("onclick", "window.open('" + pathCompleto + "');");
                }
                else
                {
                    myButton.Attributes.Add("disabled","true");
                }
            }
        }

        protected void cbwhatsapp_CheckedChanged(object sender, EventArgs e)
        {

            Hashtable htCollaboratoriWhatsapp = (Hashtable)Session[SessionManager.LISTA_COLLABORATORI_PER_INVIO_WHATSAPP];

            GridViewRow row = (GridViewRow)(((CheckBox)sender).NamingContainer);
            string idSelezionato = row.Cells[1].Text;

            CheckBox chkSelect = (CheckBox)sender;
            if (chkSelect.Checked)
            {
                try
                {
                    if (!htCollaboratoriWhatsapp.ContainsKey(idSelezionato))
                    {
                        Esito esito = new Esito();
                        Anag_Collaboratori coll = Anag_Collaboratori_BLL.Instance.getCollaboratoreById(Convert.ToInt32(idSelezionato), ref esito);
                        if (esito.Codice == 0) { 
                            htCollaboratoriWhatsapp.Add(idSelezionato, coll.Cognome + " " + coll.Nome + "|" + coll.Telefoni[0].NumeroCompleto);
                            //basePage.ShowError("ho selezionato la checkbox " + idSelezionato + " " + coll.Cognome + " " + coll.Nome);
                        }
                        else
                        {
                            basePage.ShowError(esito.Descrizione);
                        }
                    }
                }
                catch (Exception ex)
                {

                    basePage.ShowError(ex.Message);
                }

            }
            else
            {
                try
                {
                    htCollaboratoriWhatsapp.Remove(idSelezionato);
                    //basePage.ShowError("ho annullato la selezione della checkbox " + idSelezionato);
                }
                catch (Exception ex)
                {

                    basePage.ShowError(ex.Message);
                }

            }

        }

        protected void btnApriElencoWhatsapp_Click(object sender, EventArgs e)
        {
            Hashtable htCollaboratoriWhatsapp = (Hashtable)Session[SessionManager.LISTA_COLLABORATORI_PER_INVIO_WHATSAPP];

            lbElencoDestinatariWhatsapp.Items.Clear();

            foreach (DictionaryEntry s in htCollaboratoriWhatsapp)
            {
                ListItem li = new ListItem(s.Value.ToString(), s.Key.ToString());
                lbElencoDestinatariWhatsapp.Items.Add(li);
            }

            
            pnlWhatsapp.Visible = true;
        }

        protected void btnExportWhatsapp_Click(object sender, EventArgs e)
        {
            // GESTIONE NOMI FILE WHATSAPP
            string nomeFile = "Export_Whatsapp.txt";
            string pathWhatsapp = ConfigurationManager.AppSettings["PATH_DOCUMENTI_WHATSAPP"] + nomeFile;
            string mapPathWhatsapp = MapPath(ConfigurationManager.AppSettings["PATH_DOCUMENTI_WHATSAPP"]) + nomeFile;
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(mapPathWhatsapp))
            {
                // INSERISCO RIGA INTESTAZIONE
                string riga = "NAME;NUMBER";
                file.WriteLine(riga);
                foreach (ListItem item in lbElencoDestinatariWhatsapp.Items)
                {
                    string[] arNominativo = item.Text.Split('|');
                    string numero = "";
                    string nome = "";

                    if (arNominativo.Length > 1)
                    {
                        nome = arNominativo[0];
                        numero = arNominativo[1];
                    }

                    riga = nome + ";" + numero;
                    file.WriteLine(riga);
                }
                file.Flush();
                file.Close();
                if (System.IO.File.Exists(mapPathWhatsapp))
                {
                    Page page = HttpContext.Current.Handler as Page;
                    ScriptManager.RegisterStartupScript(page, page.GetType(), "apriExportWhatsapp", script: "window.open('" + pathWhatsapp.Replace("~", "") + "')", addScriptTags: true);

                }

            }
        }

            protected void btnResetElenco_Click(object sender, EventArgs e)
        {
            Hashtable htCollaboratoriWhatsapp = new Hashtable();
            Session[SessionManager.LISTA_COLLABORATORI_PER_INVIO_WHATSAPP] = htCollaboratoriWhatsapp;
            lbElencoDestinatariWhatsapp.Items.Clear();
            btnRicercaCollaboratori_Click(null, null);
            pnlWhatsapp.Visible = false;
        }
    }
}