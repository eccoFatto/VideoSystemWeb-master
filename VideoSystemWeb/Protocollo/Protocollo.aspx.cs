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
namespace VideoSystemWeb.Protocollo
{
    public partial class Protocollo : BasePage
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
                Esito esito = new Esito();

                Session["NOME_FILE"] = "";
                //BasePage p = new BasePage();
                //Esito esito = basePage.CaricaListeTipologiche();

                //basePage.listaClientiFornitori = Anag_Clienti_Fornitori_BLL.Instance.CaricaListaAziende(ref esito).Where(x => x.Cliente == true).ToList<Anag_Clienti_Fornitori>();
                //ViewState["listaClientiFornitori"] = basePage.listaClientiFornitori;
                //basePage.PopolaDDLGenerico(elencoClienti, basePage.listaClientiFornitori);

                // CARICO LE COMBO
                if (string.IsNullOrEmpty(esito.descrizione))
                {
                    ddlTipoProtocollo.Items.Clear();
                    cmbMod_Tipologia.Items.Clear();
                    ddlTipoProtocollo.Items.Add("");
                    foreach (Tipologica tipologiaProtocollo in SessionManager.ListaTipiProtocolli)
                    {
                        ListItem item = new ListItem();
                        item.Text = tipologiaProtocollo.nome;
                        item.Value = tipologiaProtocollo.nome;
                        
                        ddlTipoProtocollo.Items.Add(item);

                        ListItem itemMod = new ListItem();
                        itemMod.Text = tipologiaProtocollo.nome;
                        itemMod.Value = tipologiaProtocollo.id.ToString();

                        cmbMod_Tipologia.Items.Add(itemMod);
                    }

                    // SE UTENTE ABILITATO ALLE MODIFICHE FACCIO VEDERE I PULSANTI DI MODIFICA
                    abilitaBottoni(basePage.AbilitazioneInScrittura());
                    // DA CONFIGURAZIONE SCELGO SE VISUALIZZARE SUBITO GLI ULTIMI PROTOCOLLI
                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["VISUALIZZA_ULTIMI_PROTOCOLLI"]))
                    {
                        btnRicercaProtocollo_Click(null, null);
                    }


                }
                else
                {
                    Session["ErrorPageText"] = esito.descrizione;
                    string url = String.Format("~/pageError.aspx");
                    Response.Redirect(url, true);
                }

            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "chiudiLoader", script: "$('.loader').hide();", addScriptTags: true);
        }

        protected void btnEditProtocollo_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hf_idProt.Value) || (!string.IsNullOrEmpty((string)ViewState["idProtocollo"])))
            {
                Session["NOME_FILE"] = "";
                if (!string.IsNullOrEmpty(hf_idProt.Value)) ViewState["idProtocollo"] = hf_idProt.Value;
                editProtocollo();
                AttivaDisattivaModificaProtocollo(true);
                gestisciPulsantiProtocollo("VISUALIZZAZIONE");
                //ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriTabGiusta", script: "openDettaglioProtocollo('Protocollo');", addScriptTags: true);
                pnlContainer.Visible = true;
            }

        }

        private void gestisciPulsantiProtocollo(string stato)
        {
            switch (stato)
            {
                case "VISUALIZZAZIONE":

                    btnInserisciProtocollo.Visible = false;
                    btnModificaProtocollo.Visible = false;
                    btnEliminaProtocollo.Visible = false;
                    btnAnnullaProtocollo.Visible = false;
                    if (!basePage.AbilitazioneInScrittura())
                    {
                        btnGestisciProtocollo.Visible = false;
                    }
                    else
                    {
                        btnGestisciProtocollo.Visible = true;
                    }
                    //imgbtnCreateNewCodLav.Attributes.Add("disabled","");
                    imgbtnSelectCodLav.Attributes.Add("disabled", "");
                    //imbCercaCliente.Attributes.Add("disabled", "");
                    //btnAnnullaCaricamento.Attributes.Add("disabled", "");
                    //fuFileProt.Attributes.Add("disabled", "");
                    //fuFileProt.Enabled = false;
                    btnAnnullaCaricamento.Visible = false;
                    fuFileProt.Visible = false;
                    lblStatus.Visible = fuFileProt.Visible;

                    break;
                case "INSERIMENTO":
                    btnInserisciProtocollo.Visible = true;
                    btnModificaProtocollo.Visible = false;
                    btnEliminaProtocollo.Visible = false;
                    btnAnnullaProtocollo.Visible = false;
                    btnGestisciProtocollo.Visible = false;

                    //imgbtnCreateNewCodLav.Attributes.Remove("disabled");
                    imgbtnSelectCodLav.Attributes.Remove("disabled");
                    //imbCercaCliente.Attributes.Remove("disabled");
                    //btnAnnullaCaricamento.Attributes.Remove("disabled");
                    //fuFileProt.Attributes.Remove("disabled");
                    //fuFileProt.Enabled = true;
                    btnAnnullaCaricamento.Visible = true;
                    fuFileProt.Visible = true;
                    lblStatus.Visible = fuFileProt.Visible;
                    break;
                case "MODIFICA":
                    btnInserisciProtocollo.Visible = false;
                    btnModificaProtocollo.Visible = true;
                    btnEliminaProtocollo.Visible = true;
                    btnAnnullaProtocollo.Visible = true;
                    btnGestisciProtocollo.Visible = false;

                    //imgbtnCreateNewCodLav.Attributes.Remove("disabled");
                    imgbtnSelectCodLav.Attributes.Remove("disabled");
                    //imbCercaCliente.Attributes.Remove("disabled");
                    //btnAnnullaCaricamento.Attributes.Remove("disabled");
                    //fuFileProt.Attributes.Remove("disabled");
                    //fuFileProt.Enabled = true;
                    btnAnnullaCaricamento.Visible = true;
                    fuFileProt.Visible = true;
                    lblStatus.Visible = fuFileProt.Visible;

                    break;
                case "ANNULLAMENTO":
                    btnInserisciProtocollo.Visible = false;
                    btnModificaProtocollo.Visible = false;
                    btnEliminaProtocollo.Visible = false;
                    btnAnnullaProtocollo.Visible = false;
                    btnGestisciProtocollo.Visible = true;

                    //imgbtnCreateNewCodLav.Attributes.Add("disabled", "");
                    imgbtnSelectCodLav.Attributes.Add("disabled", "");
                    //imbCercaCliente.Attributes.Add("disabled","");
                    //btnAnnullaCaricamento.Attributes.Add("disabled", "");
                    //fuFileProt.Attributes.Add("disabled", "");
                    //fuFileProt.Enabled = false;
                    btnAnnullaCaricamento.Visible = false;
                    fuFileProt.Visible = false;
                    lblStatus.Visible = fuFileProt.Visible;

                    break;
                default:
                    btnInserisciProtocollo.Visible = false;
                    btnModificaProtocollo.Visible = false;
                    btnEliminaProtocollo.Visible = false;
                    btnAnnullaProtocollo.Visible = false;
                    btnGestisciProtocollo.Visible = true;

                    //imgbtnCreateNewCodLav.Attributes.Add("disabled", "");
                    imgbtnSelectCodLav.Attributes.Add("disabled", "");
                    //imbCercaCliente.Attributes.Add("disabled","");

                    //btnAnnullaCaricamento.Attributes.Add("disabled", "");
                    //fuFileProt.Attributes.Add("disabled", "");
                    //fuFileProt.Enabled = false;
                    btnAnnullaCaricamento.Visible = false;
                    fuFileProt.Visible = false;
                    lblStatus.Visible = fuFileProt.Visible;
                    break;
            }

        }

        protected void btnInsProtocollo_Click(object sender, EventArgs e)
        {
            // VISUALIZZO FORM INSERIMENTO NUOVO PROTOCOLLO
            Session["NOME_FILE"] = "";
            ViewState["idProtocollo"] = "";
            editProtocolloVuoto();
            AttivaDisattivaModificaProtocollo(false);
            gestisciPulsantiProtocollo("INSERIMENTO");

            pnlContainer.Visible = true;
        }

        protected void btnInserisciProtocollo_Click(object sender, EventArgs e)
        {
            // INSERISCO PROTOCOLLO
            Esito esito = new Esito();
            Protocolli protocollo = CreaOggettoProtocollo(ref esito);

            if (esito.codice != Esito.ESITO_OK)
            {
                basePage.ShowError("Controllare i campi evidenziati");
            }
            else
            {
                NascondiErroriValidazione();

                int iRet = Protocolli_BLL.Instance.CreaProtocollo(protocollo, ref esito);
                if (iRet > 0)
                {
                    // UNA VOLTA INSERITO CORRETTAMENTE PUO' ESSERE MODIFICATO
                    hf_idProt.Value = iRet.ToString();
                    ViewState["idProtocollo"] = hf_idProt.Value;
                    hf_tipoOperazione.Value = "VISUALIZZAZIONE";
                }

                if (esito.codice != Esito.ESITO_OK)
                {
                    log.Error(esito.descrizione);
                    basePage.ShowError(esito.descrizione);
                }
                basePage.ShowSuccess("Inserito Protocollo n. " + protocollo.Numero_protocollo);
                btnEditProtocollo_Click(null, null);
            }
        }

        protected void btnModificaProtocollo_Click(object sender, EventArgs e)
        {
            // SALVO MODIFICHE PROTOCOLLO
            Esito esito = new Esito();
            Protocolli protocollo = CreaOggettoProtocollo(ref esito);

            if (esito.codice != Esito.ESITO_OK)
            {
                log.Error(esito.descrizione);
                basePage.ShowError("Controllare i campi evidenziati!");
            }
            else
            {
                //Nascondierro();

                esito = Protocolli_BLL.Instance.AggiornaProtocollo(protocollo);

                if (esito.codice != Esito.ESITO_OK)
                {
                    log.Error(esito.descrizione);
                    basePage.ShowError(esito.descrizione);

                }
                btnEditProtocollo_Click(null, null);
            }

        }

        protected void btnEliminaProtocollo_Click(object sender, EventArgs e)
        {
            Esito esito = new Esito();

            if (!string.IsNullOrEmpty((string)ViewState["idProtocollo"]))
            {
                esito = Protocolli_BLL.Instance.EliminaProtocollo(Convert.ToInt32(ViewState["idProtocollo"].ToString()));
                if (esito.codice != Esito.ESITO_OK)
                {
                    basePage.ShowError(esito.descrizione);
                    AttivaDisattivaModificaProtocollo(true);
                }
                else
                {
                    AttivaDisattivaModificaProtocollo(true);
                    pnlContainer.Visible = false;
                    btnRicercaProtocollo_Click(null, null);
                }

            }

        }

        protected void btnAnnullaProtocollo_Click(object sender, EventArgs e)
        {
            AttivaDisattivaModificaProtocollo(true);
            gestisciPulsantiProtocollo("ANNULLAMENTO");

        }

        protected void btnRicercaProtocollo_Click(object sender, EventArgs e)
        {
            string queryRicerca = ConfigurationManager.AppSettings["QUERY_SEARCH_PROTOCOLLI"];

            queryRicerca = queryRicerca.Replace("@numeroProtocollo", tbNumeroProtocollo.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@dataProtocollo", tbDataProtocollo.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@dataLavorazione", tbDataLavorazione.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@codiceLavoro", tbCodiceLavoro.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@cliente", tbRagioneSociale.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@produzione", tbProduzione.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@lavorazione", tbLavorazione.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@tipoProtocollo", ddlTipoProtocollo.SelectedValue.ToString().Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@protocolloRiferimento", tbProtocolloRiferimento.Text.Trim().Replace("'","''"));

            Esito esito = new Esito();
            DataTable dtProtocolli = Base_DAL.getDatiBySql(queryRicerca, ref esito);
            gv_protocolli.DataSource = dtProtocolli;
            gv_protocolli.DataBind();

        }

        protected void gv_protocolli_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                // PRENDO IL PATH DELL'ALLEGATO SE C'E'
                string pathDocumento = e.Row.Cells[9].Text.Trim();
                ImageButton myButton = e.Row.FindControl("btnOpenDoc") as ImageButton;
                if (!string.IsNullOrEmpty(pathDocumento) && !pathDocumento.Equals("&nbsp;"))
                {
                    string pathCompleto = ConfigurationManager.AppSettings["PATH_DOCUMENTI_PROTOCOLLO"].Replace("~", "") + pathDocumento;
                    myButton.Attributes.Add("onclick", "window.open('" + pathCompleto + "');");
                }
                else
                {
                    myButton.Attributes.Add("disabled", "true");
                }

                // PRENDO L'ID DEL PROTOCOLLO SELEZIONATO
                string idProtocolloSelezionato = e.Row.Cells[2].Text;
                ImageButton myButtonEdit = e.Row.FindControl("imgEdit") as ImageButton;
                myButtonEdit.Attributes.Add("onclick", "mostraProtocollo('" + idProtocolloSelezionato + "');");
            }

        }
  
        protected void gv_protocolli_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_protocolli.PageIndex = e.NewPageIndex;
            btnRicercaProtocollo_Click(null, null);

        }

        private void pulisciCampiDettaglio()
        {
            tbMod_CodiceLavoro.Text = "";
            tbMod_NumeroProtocollo.Text = "";
            tbMod_ProtocolloRiferimento.Text = "";
            tbMod_DataProtocollo.Text = "";
            tbMod_DataLavorazione.Text = "";
            tbMod_Produzione.Text = "";
            tbMod_Cliente.Text = "";
            tbMod_NomeFile.Text = "";
            tbMod_Lavorazione.Text = "";
            cmbMod_Tipologia.SelectedIndex = 0;

        }

        private void AttivaDisattivaModificaProtocollo(bool attivaModifica)
        {
            tbMod_CodiceLavoro.ReadOnly = attivaModifica;
            tbMod_NumeroProtocollo.ReadOnly = true;
            tbMod_ProtocolloRiferimento.ReadOnly = attivaModifica;
            tbMod_DataProtocollo.ReadOnly = true;
            tbMod_DataLavorazione.ReadOnly = attivaModifica;
           // CalendarExtender_DataLavorazione.Enabled = !attivaModifica;
            tbMod_Produzione.ReadOnly = attivaModifica;
            tbMod_Cliente.ReadOnly = attivaModifica;
            tbMod_Lavorazione.ReadOnly = attivaModifica;
            tbMod_NomeFile.ReadOnly = true;
            //tbMod_NomeFile.ReadOnly = attivaModifica;

            if (attivaModifica)
            {
                cmbMod_Tipologia.Attributes.Add("disabled", "");
            }
            else
            {
                cmbMod_Tipologia.Attributes.Remove("disabled");
            }
        }

        private void editProtocolloVuoto()
        {
            Esito esito = new Esito();
            pulisciCampiDettaglio();
            //btnViewAttachement.Attributes.Add("disabled", "true");
            btnViewAttachement.Enabled = false;
        }
        private void editProtocollo()
        {
            string idProtocollo = (string)ViewState["idProtocollo"];
            Esito esito = new Esito();

            if (!string.IsNullOrEmpty(idProtocollo))
            {
                Session["NOME_FILE"] = "";
                Entity.Protocolli protocollo = Protocolli_BLL.Instance.getProtocolloById(ref esito, Convert.ToInt16(idProtocollo));
                if (esito.codice == 0)
                {
                    pulisciCampiDettaglio();

                    // RIEMPIO I CAMPI DEL DETTAGLIO PROTOCOLLO
                    tbMod_CodiceLavoro.Text = protocollo.Codice_lavoro;
                    tbMod_NumeroProtocollo.Text = protocollo.Numero_protocollo;
                    tbMod_DataProtocollo.Text = "";
                    if (protocollo.Data_protocollo != null)
                    {
                        tbMod_DataProtocollo.Text = ((DateTime)protocollo.Data_protocollo).ToString("dd/MM/yyyy");
                    }
                    tbMod_DataLavorazione.Text = "";
                    if (protocollo.Data_inizio_lavorazione != null)
                    {
                        tbMod_DataLavorazione.Text = ((DateTime)protocollo.Data_inizio_lavorazione).ToString("dd/MM/yyyy");
                    }
                    tbMod_Produzione.Text = protocollo.Produzione;
                    tbMod_ProtocolloRiferimento.Text = protocollo.Protocollo_riferimento;
                    tbMod_Cliente.Text = protocollo.Cliente;
                    tbMod_NomeFile.Text = protocollo.PathDocumento;
                    Session["NOME_FILE"] = protocollo.PathDocumento;
                    tbMod_Lavorazione.Text = protocollo.Descrizione;

                    //TIPI PROTOCOLLO
                    ListItem trovati = cmbMod_Tipologia.Items.FindByValue(protocollo.Id_tipo_protocollo.ToString());
                    if (trovati != null)
                    {
                        cmbMod_Tipologia.SelectedValue = trovati.Value;
                    }
                    else
                    {
                        cmbMod_Tipologia.Text = "";
                    }

                    if (!string.IsNullOrEmpty(protocollo.PathDocumento))
                    {
                        string pathCompleto = ConfigurationManager.AppSettings["PATH_DOCUMENTI_PROTOCOLLO"].Replace("~", "") + protocollo.PathDocumento;
                        btnViewAttachement.Attributes.Add("onclick", "window.open('" + pathCompleto + "');");
                        btnViewAttachement.Enabled = true;
                    }
                    else
                    {
                        //btnViewAttachement.Attributes.Add("disabled", "true");
                        btnViewAttachement.Enabled = false;
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

        protected void btnGestisciProtocollo_Click(object sender, EventArgs e)
        {
            AttivaDisattivaModificaProtocollo(false);
            gestisciPulsantiProtocollo("MODIFICA");

        }

        private Protocolli CreaOggettoProtocollo(ref Esito esito)
        {
            Protocolli protocollo = new Protocolli();

            if (string.IsNullOrEmpty((string)ViewState["idProtocollo"]))
            {
                ViewState["idProtocollo"] = "0";
            }

            protocollo.Id = Convert.ToInt16(ViewState["idProtocollo"].ToString());

            protocollo.Id_tipo_protocollo = Convert.ToInt32(cmbMod_Tipologia.SelectedValue);
            if (string.IsNullOrEmpty(tbMod_NumeroProtocollo.Text.Trim()))
            {
                tbMod_NumeroProtocollo.Text = Protocolli_BLL.Instance.getNumeroProtocollo();
            }
            protocollo.Numero_protocollo = BasePage.ValidaCampo(tbMod_NumeroProtocollo, "", true, ref esito);
            //protocollo.PathDocumento = BasePage.ValidaCampo(tbMod_NomeFile, "", false, ref esito); 
            protocollo.PathDocumento = (string)Session["NOME_FILE"];
            protocollo.Protocollo_riferimento = BasePage.ValidaCampo(tbMod_ProtocolloRiferimento, "", false, ref esito);
            protocollo.Cliente = BasePage.ValidaCampo(tbMod_Cliente, "", false, ref esito);
            protocollo.Produzione = BasePage.ValidaCampo(tbMod_Produzione, "", false, ref esito);
            protocollo.Data_inizio_lavorazione = BasePage.ValidaCampo(tbMod_DataLavorazione, DateTime.Now, true, ref esito);
            protocollo.Codice_lavoro = BasePage.ValidaCampo(tbMod_CodiceLavoro, "", false, ref esito);
            protocollo.Descrizione = BasePage.ValidaCampo(tbMod_Lavorazione, "", true, ref esito);
            protocollo.Attivo = true;

            return protocollo;
        }

        protected void imgbtnCreateNewCodLav_Click(object sender, ImageClickEventArgs e)
        {
            tbMod_CodiceLavoro.Text = Protocolli_BLL.Instance.getCodLavFormattato();
        }

        protected void btnAnnullaCaricamento_Click(object sender, EventArgs e)
        {

            //fuFileProt.Dispose();
            if (!string.IsNullOrEmpty(tbMod_NomeFile.Text.Trim()))
            {
                //SE ESISTE IL FILE LO CANCELLO
                try
                {
                    File.Delete(tbMod_NomeFile.Text.Trim());
                    tbMod_NomeFile.Text = "";
                    Session["NOME_FILE"] = "";
                }
                catch (Exception)
                {
                }
                
            }
        }

        protected void AsyncFileUpload1_UploadedComplete (object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            System.Threading.Thread.Sleep(5000);
            if (fuFileProt.HasFile)
            {
                string nomeFileToSave = DateTime.Now.Ticks.ToString() + "_" + Path.GetFileName(e.filename);
                string strPath = MapPath(ConfigurationManager.AppSettings["PATH_DOCUMENTI_PROTOCOLLO"]) + nomeFileToSave;
                fuFileProt.SaveAs(strPath);
                if (File.Exists(strPath))
                {
                    Session["NOME_FILE"] = nomeFileToSave;
                    //tbMod_NomeFile.Text = nomeFileToSave;
                }
                else
                {
                    Session["NOME_FILE"] = "";
                    tbMod_NomeFile.Text = "";
                    basePage.ShowWarning("Attenzione, il file: " + strPath + " non è stato creato!");
                }
            }
        }

        protected void AsyncFileUpload1_UploadedFileError(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            lblStatus.Text = e.statusMessage;
            lblStatus.Visible = true;
        }

        protected void btnChiudiPopup_Click(object sender, EventArgs e)
        {
            pnlContainer.Visible = false;
        }

        private void NascondiErroriValidazione()
        {
            tbMod_Cliente.CssClass = tbMod_Cliente.CssClass.Replace("erroreValidazione", "");
            tbMod_CodiceLavoro.CssClass = tbMod_CodiceLavoro.CssClass.Replace("erroreValidazione", "");
            tbMod_DataProtocollo.CssClass = tbMod_DataProtocollo.CssClass.Replace("erroreValidazione", "");
            tbMod_DataLavorazione.CssClass = tbMod_DataLavorazione.CssClass.Replace("erroreValidazione", "");
            tbMod_Produzione.CssClass = tbMod_Produzione.CssClass.Replace("erroreValidazione", "");
            tbMod_Lavorazione.CssClass = tbMod_Lavorazione.CssClass.Replace("erroreValidazione", "");
            tbMod_NomeFile.CssClass = tbMod_NomeFile.CssClass.Replace("erroreValidazione", "");
            tbMod_NumeroProtocollo.CssClass = tbMod_NumeroProtocollo.CssClass.Replace("erroreValidazione", "");
            tbMod_ProtocolloRiferimento.CssClass = tbMod_ProtocolloRiferimento.CssClass.Replace("erroreValidazione", "");
        }

        private void abilitaBottoni(bool utenteAbilitatoInScrittura)
        {
            //annullaModifiche();
            if (!utenteAbilitatoInScrittura)
            {
                divBtnInserisciProtocollo.Visible = false;
                btnModificaProtocollo.Visible = false;
                btnAnnullaProtocollo.Visible = false;
                btnAnnullaCaricamento.Visible = false;
                btnEliminaProtocollo.Visible = false;
                btnGestisciProtocollo.Visible = false;
                fuFileProt.Visible = false;

            }
            else
            {
                divBtnInserisciProtocollo.Visible = true;
                btnModificaProtocollo.Visible = true;
                btnAnnullaProtocollo.Visible = false;
                btnAnnullaCaricamento.Visible = true;
                btnEliminaProtocollo.Visible = false;
                btnGestisciProtocollo.Visible = true;
                fuFileProt.Visible = true;

            }
        }

        protected void btnCercaCliente_Click(object sender, EventArgs e)
        {
            PanelClienti.Visible = true;
        }

        protected void btnChiudiPopupClientiServer_Click(object sender, EventArgs e)
        {
            PanelClienti.Visible = false;
        }

        protected void imgbtnSelectCodLav_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void btnChiudiPopupLavorazioniServer_Click(object sender, EventArgs e)
        {
            PanelLavorazioni.Visible = false;
        }

        protected void btnCercaLavorazione_Click(object sender, EventArgs e)
        {
            PanelLavorazioni.Visible = true;
        }

        protected void btnRicercaLavorazioni_Click(object sender, EventArgs e)
        {
            string queryRicerca = ConfigurationManager.AppSettings["QUERY_SEARCH_LAVORAZIONI"];

            queryRicerca = queryRicerca.Replace("@ragioneSociale", tbSearch_Cliente.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@codiceLavorazione", tbSearch_CodiceLavoro.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@lavorazione", tbSearch_Lavorazione.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@luogo", tbSearch_Luogo.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@produzione", tbSearch_Produzione.Text.Trim().Replace("'", "''"));

            // SE DATA DA E' VALORIZZATA 
            if (!string.IsNullOrEmpty(tbSearch_DataInizio.Text.Trim()))
            {
                // CONTROLLO SE E' UNA DATA VALIDA
                try
                {
                    DateTime dataPartenza = Convert.ToDateTime(tbSearch_DataInizio.Text.Trim());
                    // SE DATA DA E' VALIDA CONTROLLO DATA A 
                    if (!string.IsNullOrEmpty(tbSearch_DataFine.Text.Trim()))
                    {
                        // CONTROLLO SE E' UNA DATA VALIDA
                        try
                        {
                            DateTime dataArrivo = Convert.ToDateTime(tbSearch_DataFine.Text.Trim());
                            // E' UNA DATA VALIDA, FACCIO BETWEEN TRA DATA DA E DATA A 
                            string sDataPartenza = dataPartenza.ToString("yyyy-MM-ddTHH:mm:ss");
                            string sDataArrivo = dataArrivo.ToString("yyyy-MM-ddTHH:mm:ss");
                            queryRicerca = queryRicerca.Replace("@queryRangeDate", " and data_inizio_lavorazione between '" + sDataPartenza + "' and '" + dataArrivo + "' ");
                        }
                        catch (Exception)
                        {
                            // NON E' UNA DATA VALIDA, FACCIO CONTROLLO SU DATA DA PRECISA
                            queryRicerca = queryRicerca.Replace("@queryRangeDate", " and convert(varchar, data_inizio_lavorazione, 103) = '" + dataPartenza.ToString("dd/MM/yyyy") + "' ");
                            
                        }
                    }
                    else
                    {
                        // FACCIO CONTROLLO SU DATA PRECISA SE DATA A NON E' VALIDA
                        queryRicerca = queryRicerca.Replace("@queryRangeDate", " and convert(varchar, data_inizio_lavorazione, 103) = '" + dataPartenza.ToString("dd/MM/yyyy") + "' ");
                    }

                }
                catch (Exception)
                {
                    // NON E' UNA DATA VALIDA QUINDI TENTO DI FARE LA LIKE CON IL TESTO INSERITO ED IGNORO tbSearch_DataFine
                    queryRicerca = queryRicerca.Replace("@queryRangeDate", " and (isnull(convert(varchar, data_inizio_lavorazione, 103), '') like '%" + tbSearch_DataInizio.Text.Trim() + "%') ");
                    
                }
            }
            else
            {
                queryRicerca = queryRicerca.Replace("@queryRangeDate", "");
            }
            
            
            Esito esito = new Esito();
            DataTable dtLavorazioni = Base_DAL.getDatiBySql(queryRicerca, ref esito);
            gvLavorazioni.DataSource = dtLavorazioni;
            gvLavorazioni.DataBind();


        }

        protected void gvLavorazioni_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {


                // PRENDO CODICE LAVORAZIONE E CLIENTE/FORNITORE E LI PASSO ALLA FUNZIONE
                string codLavSelezionato = e.Row.Cells[2].Text;
                string clienteFornitoreSelezionato = e.Row.Cells[3].Text;
                ImageButton myButtonEdit = e.Row.FindControl("imgSelect") as ImageButton;
                myButtonEdit.Attributes.Add("onclick", "associaCodiceLavorazione('" + codLavSelezionato.Replace("&nbsp;","") + "','" + clienteFornitoreSelezionato.Replace("&nbsp;", "") + "');");
            }

            //associaCodiceLavorazione(codLav, cliente)
        }

        protected void gvLavorazioni_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvLavorazioni.PageIndex = e.NewPageIndex;
            btnRicercaLavorazioni_Click(null, null);
        }
    }
}