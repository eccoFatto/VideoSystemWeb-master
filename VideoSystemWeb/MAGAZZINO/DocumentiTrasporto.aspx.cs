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
namespace VideoSystemWeb.Magazzino
{
    public partial class DocumentiTrasporto : BasePage
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

                Session["NOME_FILE"] = "";

                ddlTipoDocumentoTrasporto.Items.Clear();
                cmbMod_Tipologia.Items.Clear();
                ddlTipoDocumentoTrasporto.Items.Add("");
                CaricaCombo();

                // SE UTENTE ABILITATO ALLE MODIFICHE FACCIO VEDERE I PULSANTI DI MODIFICA
                abilitaBottoni(basePage.AbilitazioneInScrittura());

            }

            ScriptManager.RegisterStartupScript(this, typeof(Page), "coerenzaDate", "controlloCoerenzaDate('" + tbDataLavorazione.ClientID + "', '" + tbDataLavorazioneA.ClientID + "');", true);
            ScriptManager.RegisterStartupScript(this, typeof(Page), "coerenzaDate2", "controlloCoerenzaDate('" + tbDataDocumentoTrasporto.ClientID + "', '" + tbDataDocumentoTrasportoA.ClientID + "');", true);

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "chiudiLoader", script: "$('.loader').hide();", addScriptTags: true);
        }

        private void CaricaCombo()
        {
            Esito esito = new Esito();

            #region TIPO DocumentoTrasporto
            foreach (Tipologica tipologiaDocumentoTrasporto in SessionManager.ListaTipiProtocolli)
            {
                ListItem item = new ListItem();
                item.Text = tipologiaDocumentoTrasporto.nome;
                item.Value = tipologiaDocumentoTrasporto.nome;

                ddlTipoDocumentoTrasporto.Items.Add(item);

                ListItem itemMod = new ListItem();
                itemMod.Text = tipologiaDocumentoTrasporto.nome;
                itemMod.Value = tipologiaDocumentoTrasporto.id.ToString();

                cmbMod_Tipologia.Items.Add(itemMod);
            }
            #endregion
        }

        protected void btnEditDocumentoTrasporto_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hf_idDocTras.Value) || (!string.IsNullOrEmpty((string)ViewState["idDocumentoTrasporto"])))
            {
                Session["NOME_FILE"] = "";
                if (!string.IsNullOrEmpty(hf_idDocTras.Value)) ViewState["idDocumentoTrasporto"] = hf_idDocTras.Value;
                editDocumentoTrasporto();
                AttivaDisattivaModificaDocumentoTrasporto(true);
                gestisciPulsantiDocumentoTrasporto("VISUALIZZAZIONE");
                pnlContainer.Visible = true;
            }

        }

        private void gestisciPulsantiDocumentoTrasporto(string stato)
        {
            switch (stato)
            {
                case "VISUALIZZAZIONE":

                    btnInserisciDocumentoTrasporto.Visible = false;
                    btnModificaDocumentoTrasporto.Visible = false;
                    btnEliminaDocumentoTrasporto.Visible = false;
                    btnAnnullaDocumentoTrasporto.Visible = false;
                    if (!basePage.AbilitazioneInScrittura())
                    {
                        btnGestisciDocumentoTrasporto.Visible = false;
                    }
                    else
                    {
                        btnGestisciDocumentoTrasporto.Visible = true;
                    }
                    imgbtnSelectCodLav.Attributes.Add("disabled", "");
                    imgbtnSelectCliente.Attributes.Add("disabled", "");
                    btnAnnullaCaricamento.Visible = false;
                    fuFileProt.Visible = false;
                    lblStatus.Visible = fuFileProt.Visible;

                    break;
                case "INSERIMENTO":
                    btnInserisciDocumentoTrasporto.Visible = true;
                    btnModificaDocumentoTrasporto.Visible = false;
                    btnEliminaDocumentoTrasporto.Visible = false;
                    btnAnnullaDocumentoTrasporto.Visible = false;
                    btnGestisciDocumentoTrasporto.Visible = false;

                    imgbtnSelectCodLav.Attributes.Remove("disabled");
                    imgbtnSelectCliente.Attributes.Remove("disabled");
                    btnAnnullaCaricamento.Visible = true;
                    fuFileProt.Visible = true;
                    lblStatus.Visible = fuFileProt.Visible;
                    break;
                case "MODIFICA":
                    btnInserisciDocumentoTrasporto.Visible = false;
                    btnModificaDocumentoTrasporto.Visible = true;
                    btnEliminaDocumentoTrasporto.Visible = true;
                    btnAnnullaDocumentoTrasporto.Visible = true;
                    btnGestisciDocumentoTrasporto.Visible = false;

                    imgbtnSelectCodLav.Attributes.Remove("disabled");
                    imgbtnSelectCliente.Attributes.Remove("disabled");
                    btnAnnullaCaricamento.Visible = true;
                    fuFileProt.Visible = true;
                    lblStatus.Visible = fuFileProt.Visible;

                    break;
                case "ANNULLAMENTO":
                    btnInserisciDocumentoTrasporto.Visible = false;
                    btnModificaDocumentoTrasporto.Visible = false;
                    btnEliminaDocumentoTrasporto.Visible = false;
                    btnAnnullaDocumentoTrasporto.Visible = false;
                    btnGestisciDocumentoTrasporto.Visible = true;

                    imgbtnSelectCodLav.Attributes.Add("disabled", "");
                    imgbtnSelectCliente.Attributes.Add("disabled", "");
                    btnAnnullaCaricamento.Visible = false;
                    fuFileProt.Visible = false;
                    lblStatus.Visible = fuFileProt.Visible;

                    break;
                default:
                    btnInserisciDocumentoTrasporto.Visible = false;
                    btnModificaDocumentoTrasporto.Visible = false;
                    btnEliminaDocumentoTrasporto.Visible = false;
                    btnAnnullaDocumentoTrasporto.Visible = false;
                    btnGestisciDocumentoTrasporto.Visible = true;

                    imgbtnSelectCodLav.Attributes.Add("disabled", "");
                    imgbtnSelectCliente.Attributes.Add("disabled", "");
                    btnAnnullaCaricamento.Visible = false;
                    fuFileProt.Visible = false;
                    lblStatus.Visible = fuFileProt.Visible;
                    break;
            }

        }

        protected void btnInsDocumentoTrasporto_Click(object sender, EventArgs e)
        {
            // VISUALIZZO FORM INSERIMENTO NUOVO DocumentoTrasporto
            Session["NOME_FILE"] = "";
            ViewState["idDocumentoTrasporto"] = "";
            editDocumentoTrasportoVuoto();
            AttivaDisattivaModificaDocumentoTrasporto(false);
            gestisciPulsantiDocumentoTrasporto("INSERIMENTO");

            tbMod_NumeroDocumentoTrasporto.Text = Protocolli_BLL.Instance.getNumeroDocumentoTrasporto();

            pnlContainer.Visible = true;
        }

        protected void btnInserisciDocumentoTrasporto_Click(object sender, EventArgs e)
        {
            // INSERISCO DocumentoTrasporto
            Esito esito = new Esito();
            VideoSystemWeb.Entity.DocumentiTrasporto documentoTrasporto = CreaOggettoDocumentoTrasporto(ref esito);

            if (esito.Codice == Esito.ESITO_OK)
            {
                NascondiErroriValidazione();

                int iRet = DocumentiTrasporto_BLL.Instance.CreaDocumentoTrasporto(documentoTrasporto, ref esito);
                if (iRet > 0)
                {
                    // UNA VOLTA INSERITO CORRETTAMENTE PUO' ESSERE MODIFICATO
                    hf_idDocTras.Value = iRet.ToString();
                    ViewState["idDocumentoTrasporto"] = hf_idDocTras.Value;
                    hf_tipoOperazione.Value = "VISUALIZZAZIONE";
                }

                if (esito.Codice != Esito.ESITO_OK)
                {
                    log.Error(esito.Descrizione);
                    basePage.ShowError(esito.Descrizione);
                }
                ShowPopMessage("Inserito DocumentoTrasporto n. " + documentoTrasporto.NumeroDocumentoTrasporto);

                btnEditDocumentoTrasporto_Click(null, null);
                
                
            }
        }

        protected void btnModificaDocumentoTrasporto_Click(object sender, EventArgs e)
        {
            // SALVO MODIFICHE PROTOCOLL
            Esito esito = new Esito();
            VideoSystemWeb.Entity.DocumentiTrasporto documentoTrasporto = CreaOggettoDocumentoTrasporto(ref esito);

            if (esito.Codice != Esito.ESITO_OK)
            {
                log.Error(esito.Descrizione);
            }
            else
            {

                esito = DocumentiTrasporto_BLL.Instance.AggiornaDocumentoTrasporto(documentoTrasporto);

                if (esito.Codice != Esito.ESITO_OK)
                {
                    log.Error(esito.Descrizione);
                    basePage.ShowError(esito.Descrizione);

                }
                btnEditDocumentoTrasporto_Click(null, null);
            }

        }

        protected void btnEliminaDocumentoTrasporto_Click(object sender, EventArgs e)
        {
            Esito esito = new Esito();
            if (!string.IsNullOrEmpty((string)ViewState["idDocumentoTrasporto"]))
            {
                Entity.DocumentiTrasporto documentoTrasporto = DocumentiTrasporto_BLL.Instance.getDocumentoTrasportoById(ref esito, Convert.ToInt64((string)ViewState["idDocumentoTrasporto"]));
                if (esito.Codice == 0)
                {
                    esito = DocumentiTrasporto_BLL.Instance.EliminaDocumentoTrasporto(Convert.ToInt32(ViewState["idDocumentoTrasporto"].ToString()));
                    if (esito.Codice != Esito.ESITO_OK)
                    {
                        basePage.ShowError(esito.Descrizione);
                        AttivaDisattivaModificaDocumentoTrasporto(true);
                    }
                    else
                    {
                        AttivaDisattivaModificaDocumentoTrasporto(true);
                        pnlContainer.Visible = false;
                        btnRicercaDocumentoTrasporto_Click(null, null);
                    }
                    
                }
            }

        }

        protected void btnAnnullaDocumentoTrasporto_Click(object sender, EventArgs e)
        {
            AttivaDisattivaModificaDocumentoTrasporto(true);
            gestisciPulsantiDocumentoTrasporto("ANNULLAMENTO");

        }

        protected void btnRicercaDocumentoTrasporto_Click(object sender, EventArgs e)
        {
            string queryRicerca = ConfigurationManager.AppSettings["QUERY_SEARCH_DOCUMENTI_TRASPORTO"];

            queryRicerca = queryRicerca.Replace("@numeroDocumentoTrasporto", tbNumeroDocumentoTrasporto.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@codiceLavoro", tbCodiceLavoro.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@produzione", tbProduzione.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@descrizione", tbDescrizione.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@tipoDocumentoTrasporto", ddlTipoDocumentoTrasporto.SelectedValue.ToString().Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@DocumentoTrasportoRiferimento", tbDocumentoTrasportoRiferimento.Text.Trim().Replace("'","''"));
            queryRicerca = queryRicerca.Replace("@destinatario", ddlDestinatario.SelectedValue.ToString().Trim().Replace("'", "''"));

            string queryDocumentoTrasportoDataTras = "";
            if (!string.IsNullOrEmpty(tbDataDocumentoTrasporto.Text))
            {
                DateTime dataDa = Convert.ToDateTime(tbDataDocumentoTrasporto.Text);
                DateTime dataA = DateTime.Now;
                queryDocumentoTrasportoDataTras = " and data_DocumentoTrasporto between '@dataDa' and '@DataA' ";
                if (!string.IsNullOrEmpty(tbDataDocumentoTrasportoA.Text))
                {
                    dataA = Convert.ToDateTime(tbDataDocumentoTrasportoA.Text);
                }
                queryDocumentoTrasportoDataTras = queryDocumentoTrasportoDataTras.Replace("@dataDa", dataDa.ToString("yyyy-MM-ddT00:00:00.000"));
                queryDocumentoTrasportoDataTras = queryDocumentoTrasportoDataTras.Replace("@DataA", dataA.ToString("yyyy-MM-ddT23:59:59.999"));
            }
            queryRicerca = queryRicerca.Replace("@dataDocumentoTrasporto", queryDocumentoTrasportoDataTras);

            string queryDocumentoTrasportoDataLav = "";
            if (!string.IsNullOrEmpty(tbDataLavorazione.Text))
            {
                DateTime dataDa = Convert.ToDateTime(tbDataLavorazione.Text);
                DateTime dataA = DateTime.Now;
                queryDocumentoTrasportoDataLav = " and data_inizio_lavorazione between '@dataDa' and '@DataA' ";
                if (!string.IsNullOrEmpty(tbDataLavorazioneA.Text))
                {
                    dataA = Convert.ToDateTime(tbDataLavorazioneA.Text);
                }
                queryDocumentoTrasportoDataLav = queryDocumentoTrasportoDataLav.Replace("@dataDa", dataDa.ToString("yyyy-MM-ddT00:00:00.000"));
                queryDocumentoTrasportoDataLav = queryDocumentoTrasportoDataLav.Replace("@DataA", dataA.ToString("yyyy-MM-ddT00:00:00.000"));
            }
            queryRicerca = queryRicerca.Replace("@dataLavorazione", queryDocumentoTrasportoDataLav);

            Esito esito = new Esito();
            DataTable dtProtocolli = Base_DAL.GetDatiBySql(queryRicerca, ref esito);

            Session["TaskTable"] = dtProtocolli;
            gv_documenti_trasporto.DataSource = Session["TaskTable"];
            gv_documenti_trasporto.DataBind();

        }

        protected void gv_documenti_trasporto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 1) e.Row.Cells[1].Visible = false;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                // PRENDO IL PATH DELL'ALLEGATO SE C'E'
                string pathDocumento = e.Row.Cells[GetColumnIndexByName(e.Row, "Nome File")].Text;

                string preg = e.Row.Cells[GetColumnIndexByName(e.Row, "Pregresso")].Text;

                ImageButton myButton = e.Row.FindControl("btnOpenDoc") as ImageButton;
                if (!string.IsNullOrEmpty(pathDocumento) && !pathDocumento.Equals("&nbsp;"))
                {
                    string pathRelativo = "";
                    if (preg=="True" || preg=="Si")
                    {
                        pathRelativo = ConfigurationManager.AppSettings["PATH_DOCUMENTI_PREGRESSO"].Replace("~", "");
                    }
                    else
                    {
                        pathRelativo = ConfigurationManager.AppSettings["PATH_DOCUMENTI_DocumentoTrasporto"].Replace("~", "");
                    }

                    string pathCompleto = pathRelativo + pathDocumento;
                    myButton.Attributes.Add("onclick", "window.open('" + pathCompleto + "');");
                }
                else
                {
                    myButton.Attributes.Add("disabled", "true");
                }

                // PRENDO L'ID DEL DocumentoTrasporto SELEZIONATO
                string idDocumentoTrasportoSelezionato = e.Row.Cells[GetColumnIndexByName(e.Row, "id")].Text;
                ImageButton myButtonEdit = e.Row.FindControl("imgEdit") as ImageButton;
                myButtonEdit.Attributes.Add("onclick", "mostraDocumentoTrasporto('" + idDocumentoTrasportoSelezionato + "');");
            }

        }
  
        protected void gv_documenti_trasporto_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_documenti_trasporto.PageIndex = e.NewPageIndex;
            btnRicercaDocumentoTrasporto_Click(null, null);
        }

        private void pulisciCampiDettaglio()
        {
            tbMod_CodiceLavoro.Text = "";
            tbMod_NumeroDocumentoTrasporto.Text = "";
            tbMod_DocumentoTrasportoRiferimento.Text = "";
            tbMod_DataDocumentoTrasporto.Text = "";
            tbMod_DataLavorazione.Text = "";
            tbMod_Produzione.Text = "";
            tbMod_Cliente.Text = "";
            tbMod_NomeFile.Text = "";
            tbMod_Lavorazione.Text = "";
            tbMod_Descrizione.Text = "";
            cmbMod_Tipologia.SelectedIndex = 0;
            cmbMod_Destinatario.SelectedIndex = 0;
            cbMod_Pregresso.Checked = false;

        }

        private void AttivaDisattivaModificaDocumentoTrasporto(bool attivaModifica)
        {
            tbMod_CodiceLavoro.ReadOnly = attivaModifica;
            //tbMod_CodiceLavoro.ReadOnly = true;
            tbMod_NumeroDocumentoTrasporto.ReadOnly = true;
            tbMod_DocumentoTrasportoRiferimento.ReadOnly = attivaModifica;
            tbMod_DataDocumentoTrasporto.ReadOnly = true;
            tbMod_DataLavorazione.ReadOnly = attivaModifica;
           // CalendarExtender_DataLavorazione.Enabled = !attivaModifica;
            tbMod_Produzione.ReadOnly = attivaModifica;
            //tbMod_Cliente.ReadOnly = attivaModifica;
            tbMod_Cliente.ReadOnly = true;
            tbMod_Lavorazione.ReadOnly = attivaModifica;
            tbMod_Descrizione.ReadOnly = attivaModifica;
            tbMod_NomeFile.ReadOnly = true;
            //tbMod_NomeFile.ReadOnly = attivaModifica;

            if (attivaModifica)
            {
                cmbMod_Tipologia.Attributes.Add("disabled", "");
                cmbMod_Destinatario.Attributes.Add("disabled", "");
            }
            else
            {
                cmbMod_Tipologia.Attributes.Remove("disabled");
                cmbMod_Destinatario.Attributes.Remove("disabled");
            }
        }

        private void editDocumentoTrasportoVuoto()
        {
            Esito esito = new Esito();
            pulisciCampiDettaglio();
            tbMod_CodiceLavoro.Text = "generico";
            //btnViewAttachement.Attributes.Add("disabled", "true");
            btnViewAttachement.Enabled = false;
        }
        private void editDocumentoTrasporto()
        {
            string idDocumentoTrasporto = (string)ViewState["idDocumentoTrasporto"];
            Esito esito = new Esito();

            if (!string.IsNullOrEmpty(idDocumentoTrasporto))
            {
                Session["NOME_FILE"] = "";
                Entity.DocumentiTrasporto documentoTrasporto = DocumentiTrasporto_BLL.Instance.getDocumentoTrasportoById(ref esito, Convert.ToInt64(idDocumentoTrasporto));
                if (esito.Codice == 0)
                {
                    pulisciCampiDettaglio();

                    // RIEMPIO I CAMPI DEL DETTAGLIO DocumentoTrasporto
                    tbMod_NumeroDocumentoTrasporto.Text = documentoTrasporto.NumeroDocumentoTrasporto;
                    tbMod_DataDocumentoTrasporto.Text = "";
                    if (documentoTrasporto.DataTrasporto != null)
                    {
                        tbMod_DataDocumentoTrasporto.Text = ((DateTime)documentoTrasporto.DataTrasporto).ToString("dd/MM/yyyy");
                    }


                    //tbMod_NomeFile.Text = DocumentoTrasporto.PathDocumento;
                    //Session["NOME_FILE"] = DocumentoTrasporto.PathDocumento;
                    //tbMod_Lavorazione.Text = DocumentoTrasporto.Lavorazione;
                    //tbMod_Descrizione.Text = documentoTrasporto.Descrizione;


                    //if (!string.IsNullOrEmpty(documentoTrasporto.PathDocumento))
                    //{
                    //    string pathRelativo = "";

                    //    pathRelativo = ConfigurationManager.AppSettings["PATH_DOCUMENTI_DocumentoTrasporto"].Replace("~", "");
                        

                    //    string pathCompleto = pathRelativo + documentoTrasporto.PathDocumento;
                    //    btnViewAttachement.Attributes.Add("onclick", "window.open('" + pathCompleto + "');");
                    //    btnViewAttachement.Enabled = true;
                    //}
                    //else
                    //{
                    //    btnViewAttachement.Enabled = false;
                    //}

                }
                else
                {
                    Session["ErrorPageText"] = esito.Descrizione;
                    string url = String.Format("~/pageError.aspx");
                    Response.Redirect(url, true);
                }
            }

        }

        protected void btnGestisciDocumentoTrasporto_Click(object sender, EventArgs e)
        {
            if (!cbMod_Pregresso.Checked) {
                AttivaDisattivaModificaDocumentoTrasporto(false);
                gestisciPulsantiDocumentoTrasporto("MODIFICA");
            }
            else
            {
                basePage.ShowWarning("I Protocolli pregressi non sono modificabili!");
            }

        }

        private VideoSystemWeb.Entity.DocumentiTrasporto CreaOggettoDocumentoTrasporto(ref Esito esito)
        {
            VideoSystemWeb.Entity.DocumentiTrasporto documentoTrasporto = new VideoSystemWeb.Entity.DocumentiTrasporto();

            if (string.IsNullOrEmpty((string)ViewState["idDocumentoTrasporto"]))
            {
                ViewState["idDocumentoTrasporto"] = "0";
            }

            documentoTrasporto.Id = Convert.ToInt64(ViewState["idDocumentoTrasporto"].ToString());

            documentoTrasporto.Destinatario = cmbMod_Destinatario.SelectedValue;

            documentoTrasporto.Peso = "1";

            if (string.IsNullOrEmpty(tbMod_NumeroDocumentoTrasporto.Text.Trim()))
            {
                tbMod_NumeroDocumentoTrasporto.Text = Protocolli_BLL.Instance.getNumeroDocumentoTrasporto();
            }
            documentoTrasporto.NumeroDocumentoTrasporto = BasePage.ValidaCampo(tbMod_NumeroDocumentoTrasporto, "", true, ref esito);

            return documentoTrasporto;
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
                //string nomeFileToSave = DateTime.Now.Ticks.ToString() + "_" + Path.GetFileName(e.filename);
                if (!string.IsNullOrEmpty(tbMod_NumeroDocumentoTrasporto.Text.Trim())){
                    nomeFileToSave = "DocumentoTrasporto_" + tbMod_NumeroDocumentoTrasporto.Text.Trim() + Path.GetExtension(e.filename);
                }
                
                string strPath = MapPath(ConfigurationManager.AppSettings["PATH_DOCUMENTI_DocumentoTrasporto"]) + nomeFileToSave;
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
            tbMod_DataDocumentoTrasporto.CssClass = tbMod_DataDocumentoTrasporto.CssClass.Replace("erroreValidazione", "");
            tbMod_DataLavorazione.CssClass = tbMod_DataLavorazione.CssClass.Replace("erroreValidazione", "");
            tbMod_Produzione.CssClass = tbMod_Produzione.CssClass.Replace("erroreValidazione", "");
            tbMod_Lavorazione.CssClass = tbMod_Lavorazione.CssClass.Replace("erroreValidazione", "");
            tbMod_Descrizione.CssClass = tbMod_Descrizione.CssClass.Replace("erroreValidazione", "");
            tbMod_NomeFile.CssClass = tbMod_NomeFile.CssClass.Replace("erroreValidazione", "");
            tbMod_NumeroDocumentoTrasporto.CssClass = tbMod_NumeroDocumentoTrasporto.CssClass.Replace("erroreValidazione", "");
            tbMod_DocumentoTrasportoRiferimento.CssClass = tbMod_DocumentoTrasportoRiferimento.CssClass.Replace("erroreValidazione", "");
        }

        private void abilitaBottoni(bool utenteAbilitatoInScrittura)
        {
            //annullaModifiche();
            if (!utenteAbilitatoInScrittura)
            {
                divBtnInserisciDocumentoTrasporto.Visible = false;
                btnModificaDocumentoTrasporto.Visible = false;
                btnAnnullaDocumentoTrasporto.Visible = false;
                btnAnnullaCaricamento.Visible = false;
                btnEliminaDocumentoTrasporto.Visible = false;
                btnGestisciDocumentoTrasporto.Visible = false;
                fuFileProt.Visible = false;

            }
            else
            {
                divBtnInserisciDocumentoTrasporto.Visible = true;
                btnModificaDocumentoTrasporto.Visible = true;
                btnAnnullaDocumentoTrasporto.Visible = false;
                btnAnnullaCaricamento.Visible = true;
                btnEliminaDocumentoTrasporto.Visible = false;
                btnGestisciDocumentoTrasporto.Visible = true;
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
            DataTable dtLavorazioni = Base_DAL.GetDatiBySql(queryRicerca, ref esito);
            gvLavorazioni.DataSource = dtLavorazioni;
            gvLavorazioni.DataBind();


        }

        protected void gvLavorazioni_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // PRENDO CODICE LAVORAZIONE E CLIENTE/FORNITORE E LI PASSO ALLA FUNZIONE
                string codLavSelezionato = e.Row.Cells[2].Text.Replace("&nbsp;", "").Replace("&#224;", "à");
                string clienteFornitoreSelezionato = e.Row.Cells[3].Text.Replace("&nbsp;", "").Replace("&#224;", "à");
                string produzioneSelezionata = e.Row.Cells[6].Text.Replace("&nbsp;", "").Replace("&#224;", "à");
                string lavorazioneSelezionata = e.Row.Cells[7].Text.Replace("&nbsp;", "").Replace("&#224;", "à");

                ImageButton myButtonEdit = e.Row.FindControl("imgSelect") as ImageButton;
                //myButtonEdit.Attributes.Add("onclick", "associaCodiceLavorazione('" + codLavSelezionato.Replace("&nbsp;","") + "','" + clienteFornitoreSelezionato.Replace("&nbsp;", "") + "');");
                myButtonEdit.Attributes.Add("onclick", "associaCodiceLavorazione('" + codLavSelezionato + "','" + clienteFornitoreSelezionato + "','" + produzioneSelezionata + "','" + lavorazioneSelezionata + "');");
            }

            //associaCodiceLavorazione(codLav, cliente)
        }

        protected void gvLavorazioni_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvLavorazioni.PageIndex = e.NewPageIndex;
            btnRicercaLavorazioni_Click(null, null);
        }

        protected void btnRicercaClienti_Click(object sender, EventArgs e)
        {
            string queryRicerca = "";
            switch (ddlSceltaClienteCollaboratore.Text)
            {
                case "Cliente":
                    queryRicerca = "SELECT ID, RAGIONESOCIALE as [Ragione Sociale] FROM anag_clienti_fornitori WHERE ragioneSociale LIKE '%@ragioneSociale%'";
                    queryRicerca = queryRicerca.Replace("@ragioneSociale", tbSearch_RagioneSociale.Text.Trim().Replace("'", "''"));
                    break;
                case "Collaboratore":
                    queryRicerca = "SELECT ID, COGNOME + ' ' + NOME as [Ragione Sociale] FROM anag_collaboratori WHERE cognome LIKE '%@ragioneSociale%'";
                    queryRicerca = queryRicerca.Replace("@ragioneSociale", tbSearch_RagioneSociale.Text.Trim().Replace("'", "''"));

                    break;
                default:
                    break;
            }

            Esito esito = new Esito();
            DataTable dtClienti = Base_DAL.GetDatiBySql(queryRicerca, ref esito);
            gvClienti.DataSource = dtClienti;
            gvClienti.DataBind();

        }

        protected void gvClienti_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // PRENDO ID E CLIENTE/FORNITORE E LI PASSO ALLA FUNZIONE
                string idClienteSelezionato = e.Row.Cells[1].Text;
                string clienteSelezionato = e.Row.Cells[2].Text;
                ImageButton myButtonEdit = e.Row.FindControl("imgSelect") as ImageButton;
                myButtonEdit.Attributes.Add("onclick", "associaCliente('" + idClienteSelezionato.Replace("&nbsp;", "") + "','" + clienteSelezionato.Replace("&nbsp;", "") + "');");
            }

        }

        protected void gvClienti_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvClienti.PageIndex = e.NewPageIndex;
            btnRicercaClienti_Click(null, null);

        }

        protected void imgbtnSelectCliente_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void btnAssociaClienteServer_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbMod_IdCliente.Value) || tbMod_IdCliente.Value.Equals("0"))
            {
                tbMod_Cliente.Text = tbSearch_RagioneSociale.Text.Trim();
                tbMod_IdCliente.Value = null;
                PanelClienti.Visible = false;
            }
            else {

                switch (ddlSceltaClienteCollaboratore.Text)
                {
                    case "Cliente":
                        
                        Esito esito = new Esito();
                        Anag_Clienti_Fornitori cliente = Anag_Clienti_Fornitori_BLL.Instance.getAziendaById(Convert.ToInt32(tbMod_IdCliente.Value), ref esito);
                        if (esito.Codice != 0)
                        {
                            ShowError(esito.Descrizione);
                        }
                        else
                        {
                            tbMod_Cliente.Text = cliente.RagioneSociale;
                            PanelClienti.Visible = false;
                        }
                        break;
                    case "Collaboratore":
                        esito = new Esito();
                        Anag_Collaboratori collaboratore = Anag_Collaboratori_BLL.Instance.getCollaboratoreById(Convert.ToInt32(tbMod_IdCliente.Value), ref esito);
                        if (esito.Codice != 0)
                        {
                            ShowError(esito.Descrizione);
                        }
                        else
                        {
                            tbMod_Cliente.Text = collaboratore.Cognome + " " + collaboratore.Nome;
                            PanelClienti.Visible = false;
                        }
                        break;
                    default:
                        break;
                }


            }
        }

        protected void gv_documenti_trasporto_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Retrieve the table from the session object.
            DataTable dt = Session["TaskTable"] as DataTable;

            if (dt != null)
            {

                //Sort the data.
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gv_documenti_trasporto.DataSource = Session["TaskTable"];
                gv_documenti_trasporto.DataBind();
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
            // DA CONFIGURAZIONE SCELGO SE VISUALIZZARE SUBITO GLI ULTIMI PROTOCOLLI
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["VISUALIZZA_ULTIMI_PROTOCOLLI"]))
            {
                btnRicercaDocumentoTrasporto_Click(null, null);
            }
        }
    }
}