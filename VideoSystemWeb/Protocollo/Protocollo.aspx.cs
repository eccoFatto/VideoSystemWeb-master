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
                Esito esito = new Esito();

                Session["NOME_FILE"] = "";
                //BasePage p = new BasePage();
                //Esito esito = basePage.CaricaListeTipologiche();

                //basePage.listaClientiFornitori = Anag_Clienti_Fornitori_BLL.Instance.CaricaListaAziende(ref esito).Where(x => x.Cliente == true).ToList<Anag_Clienti_Fornitori>();
                //ViewState["listaClientiFornitori"] = basePage.listaClientiFornitori;
                //basePage.PopolaDDLGenerico(elencoClienti, basePage.listaClientiFornitori);

                // CARICO LE COMBO
                if (string.IsNullOrEmpty(esito.Descrizione))
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
                    Session["ErrorPageText"] = esito.Descrizione;
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
                    imgbtnSelectCodLav.Attributes.Add("disabled", "");
                    imgbtnSelectCliente.Attributes.Add("disabled", "");
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

                    imgbtnSelectCodLav.Attributes.Remove("disabled");
                    imgbtnSelectCliente.Attributes.Remove("disabled");
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

                    imgbtnSelectCodLav.Attributes.Remove("disabled");
                    imgbtnSelectCliente.Attributes.Remove("disabled");
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

                    imgbtnSelectCodLav.Attributes.Add("disabled", "");
                    imgbtnSelectCliente.Attributes.Add("disabled", "");
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

                    imgbtnSelectCodLav.Attributes.Add("disabled", "");
                    imgbtnSelectCliente.Attributes.Add("disabled", "");
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

            tbMod_NumeroProtocollo.Text = Protocolli_BLL.Instance.getNumeroProtocollo();

            pnlContainer.Visible = true;
        }

        protected void btnInserisciProtocollo_Click(object sender, EventArgs e)
        {
            // INSERISCO PROTOCOLLO
            Esito esito = new Esito();
            Protocolli protocollo = CreaOggettoProtocollo(ref esito);

            if (esito.Codice != Esito.ESITO_OK)
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

                if (esito.Codice != Esito.ESITO_OK)
                {
                    log.Error(esito.Descrizione);
                    basePage.ShowError(esito.Descrizione);
                }
                //basePage.ShowSuccess("Inserito Protocollo n. " + protocollo.Numero_protocollo);
                ShowPopMessage("Inserito Protocollo n. " + protocollo.Numero_protocollo);



                if (cmbMod_Tipologia.SelectedItem.Text.ToUpper() == "FATTURA")
                {
                    Response.Redirect("/Scadenzario/Scadenzario.aspx?TIPO=" + cmbMod_Destinatario.SelectedValue + "&ID_PROTOCOLLO=" + iRet + "&NUM_DOC=" + protocollo.Protocollo_riferimento);
                }
                else
                {
                    btnEditProtocollo_Click(null, null);
                }
                
            }
        }

        protected void btnModificaProtocollo_Click(object sender, EventArgs e)
        {
            // SALVO MODIFICHE PROTOCOLL
            Esito esito = new Esito();
            Protocolli protocollo = CreaOggettoProtocollo(ref esito);

            if (esito.Codice != Esito.ESITO_OK)
            {
                log.Error(esito.Descrizione);
                basePage.ShowError("Controllare i campi evidenziati!");
            }
            else
            {
                //Nascondierro();

                esito = Protocolli_BLL.Instance.AggiornaProtocollo(protocollo);

                if (esito.Codice != Esito.ESITO_OK)
                {
                    log.Error(esito.Descrizione);
                    basePage.ShowError(esito.Descrizione);

                }
                btnEditProtocollo_Click(null, null);
            }

        }

        protected void btnEliminaProtocollo_Click(object sender, EventArgs e)
        {
            Esito esito = new Esito();

            if (!string.IsNullOrEmpty((string)ViewState["idProtocollo"]))
            {
                //esito = Protocolli_BLL.Instance.EliminaProtocollo(Convert.ToInt32(ViewState["idProtocollo"].ToString()));
                esito = Protocolli_BLL.Instance.RemoveProtocollo(Convert.ToInt32(ViewState["idProtocollo"].ToString()));
                if (esito.Codice != Esito.ESITO_OK)
                {
                    basePage.ShowError(esito.Descrizione);
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
            queryRicerca = queryRicerca.Replace("@codiceLavoro", tbCodiceLavoro.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@cliente", tbRagioneSociale.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@produzione", tbProduzione.Text.Trim().Replace("'", "''"));

            queryRicerca = queryRicerca.Replace("@lavorazione", tbLavorazione.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@descrizione", tbDescrizione.Text.Trim().Replace("'", "''"));

            queryRicerca = queryRicerca.Replace("@tipoProtocollo", ddlTipoProtocollo.SelectedValue.ToString().Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@protocolloRiferimento", tbProtocolloRiferimento.Text.Trim().Replace("'","''"));

            queryRicerca = queryRicerca.Replace("@destinatario", ddlDestinatario.SelectedValue.ToString().Trim().Replace("'", "''"));


            //queryRicerca = queryRicerca.Replace("@dataProtocollo", tbDataProtocollo.Text.Trim().Replace("'", "''"));
            //queryRicerca = queryRicerca.Replace("@dataLavorazione", tbDataLavorazione.Text.Trim().Replace("'", "''"));

            string queryProtocolloDataProt = "";
            if (!string.IsNullOrEmpty(tbDataProtocollo.Text))
            {
                DateTime dataDa = Convert.ToDateTime(tbDataProtocollo.Text);
                DateTime dataA = DateTime.Now;
                queryProtocolloDataProt = " and data_protocollo between '@dataDa' and '@DataA' ";
                if (!string.IsNullOrEmpty(tbDataProtocolloA.Text))
                {
                    dataA = Convert.ToDateTime(tbDataProtocolloA.Text);
                }
                queryProtocolloDataProt = queryProtocolloDataProt.Replace("@dataDa", dataDa.ToString("yyyy-MM-ddT00:00:00.000"));
                queryProtocolloDataProt = queryProtocolloDataProt.Replace("@DataA", dataA.ToString("yyyy-MM-ddT23:59:59.999"));
            }
            queryRicerca = queryRicerca.Replace("@dataProtocollo", queryProtocolloDataProt);

            string queryProtocolloDataLav = "";
            if (!string.IsNullOrEmpty(tbDataLavorazione.Text))
            {
                DateTime dataDa = Convert.ToDateTime(tbDataLavorazione.Text);
                DateTime dataA = DateTime.Now;
                queryProtocolloDataLav = " and data_inizio_lavorazione between '@dataDa' and '@DataA' ";
                if (!string.IsNullOrEmpty(tbDataLavorazioneA.Text))
                {
                    dataA = Convert.ToDateTime(tbDataLavorazioneA.Text);
                }
                queryProtocolloDataLav = queryProtocolloDataLav.Replace("@dataDa", dataDa.ToString("yyyy-MM-ddT00:00:00.000"));
                queryProtocolloDataLav = queryProtocolloDataLav.Replace("@DataA", dataA.ToString("yyyy-MM-ddT23:59:59.999"));
            }
            queryRicerca = queryRicerca.Replace("@dataLavorazione", queryProtocolloDataLav);

            Esito esito = new Esito();
            DataTable dtProtocolli = Base_DAL.GetDatiBySql(queryRicerca, ref esito);

            Session["TaskTable"] = dtProtocolli;
            //gv_protocolli.DataSource = dtProtocolli;
            gv_protocolli.DataSource = Session["TaskTable"];
            gv_protocolli.DataBind();

        }

        protected void gv_protocolli_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                // PRENDO IL PATH DELL'ALLEGATO SE C'E'
                string pathDocumento = e.Row.Cells[9].Text.Trim();

                //CheckBox cbPregresso = (CheckBox)e.Row.Cells[11].Controls[0];
                string preg = e.Row.Cells[11].Text.Trim();
                ImageButton myButton = e.Row.FindControl("btnOpenDoc") as ImageButton;
                if (!string.IsNullOrEmpty(pathDocumento) && !pathDocumento.Equals("&nbsp;"))
                {
                    string pathRelativo = "";
                    if (preg=="True")
                    {
                        pathRelativo = ConfigurationManager.AppSettings["PATH_DOCUMENTI_PREGRESSO"].Replace("~", "");
                    }
                    else
                    {
                        pathRelativo = ConfigurationManager.AppSettings["PATH_DOCUMENTI_PROTOCOLLO"].Replace("~", "");
                    }

                    string pathCompleto = pathRelativo + pathDocumento;
                    myButton.Attributes.Add("onclick", "window.open('" + pathCompleto + "');");
                }
                else
                {
                    myButton.Attributes.Add("disabled", "true");
                }

                // PRENDO L'ID DEL PROTOCOLLO SELEZIONATO
                string idProtocolloSelezionato = e.Row.Cells[1].Text;
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
            tbMod_Descrizione.Text = "";
            cmbMod_Tipologia.SelectedIndex = 0;
            cmbMod_Destinatario.SelectedIndex = 0;
            cbMod_Pregresso.Checked = false;

        }

        private void AttivaDisattivaModificaProtocollo(bool attivaModifica)
        {
            tbMod_CodiceLavoro.ReadOnly = attivaModifica;
            //tbMod_CodiceLavoro.ReadOnly = true;
            tbMod_NumeroProtocollo.ReadOnly = true;
            tbMod_ProtocolloRiferimento.ReadOnly = attivaModifica;
            tbMod_DataProtocollo.ReadOnly = true;
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

        private void editProtocolloVuoto()
        {
            Esito esito = new Esito();
            pulisciCampiDettaglio();
            tbMod_CodiceLavoro.Text = "generico";
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
                Entity.Protocolli protocollo = Protocolli_BLL.Instance.getProtocolloById(ref esito, Convert.ToInt64(idProtocollo));
                if (esito.Codice == 0)
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

                    if (protocollo.Id_cliente != null) { 
                        tbMod_IdCliente.Value = protocollo.Id_cliente.ToString();
                    }
                    else
                    {
                        tbMod_IdCliente.Value = "";
                    }
                    tbMod_NomeFile.Text = protocollo.PathDocumento;
                    Session["NOME_FILE"] = protocollo.PathDocumento;
                    tbMod_Lavorazione.Text = protocollo.Lavorazione;
                    tbMod_Descrizione.Text = protocollo.Descrizione;

                    //TIPI PROTOCOLLO
                    ListItem trovati = cmbMod_Tipologia.Items.FindByValue(protocollo.Id_tipo_protocollo.ToString());
                    if (trovati != null)
                    {
                        cmbMod_Tipologia.SelectedValue = trovati.Value;
                    }
                    else
                    {
                        //cmbMod_Tipologia.Text = "";
                        ListItem item = new ListItem
                        {
                            Text = protocollo.Id_tipo_protocollo.ToString(),
                            Value = protocollo.Id_tipo_protocollo.ToString()
                        };
                        cmbMod_Tipologia.Items.Add(item);

                        cmbMod_Tipologia.Text = protocollo.Id_tipo_protocollo.ToString();
                    }

                    //DESTINATARI
                    trovati = cmbMod_Destinatario.Items.FindByValue(protocollo.Destinatario.ToString());
                    if (trovati != null)
                    {
                        cmbMod_Destinatario.SelectedValue = trovati.Value;
                    }
                    else
                    {
                        cmbMod_Destinatario.Text = "Cliente";
                    }

                    cbMod_Pregresso.Checked = protocollo.Pregresso;

                    if (!string.IsNullOrEmpty(protocollo.PathDocumento))
                    {
                        string pathRelativo = "";
                        // PER IL PREGRESSO IL PATH CAMBIA
                        if (protocollo.Pregresso)
                        {
                            pathRelativo = ConfigurationManager.AppSettings["PATH_DOCUMENTI_PREGRESSO"].Replace("~", "");
                        }
                        else
                        {
                            pathRelativo = ConfigurationManager.AppSettings["PATH_DOCUMENTI_PROTOCOLLO"].Replace("~", "");
                        }

                        string pathCompleto = pathRelativo + protocollo.PathDocumento;
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
                    Session["ErrorPageText"] = esito.Descrizione;
                    string url = String.Format("~/pageError.aspx");
                    Response.Redirect(url, true);
                }
            }

        }

        protected void btnGestisciProtocollo_Click(object sender, EventArgs e)
        {
            if (!cbMod_Pregresso.Checked) { 
                AttivaDisattivaModificaProtocollo(false);
                gestisciPulsantiProtocollo("MODIFICA");
            }
            else
            {
                basePage.ShowWarning("I Protocolli pregressi non sono modificabili!");
            }

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

            protocollo.Destinatario = cmbMod_Destinatario.SelectedValue;

            protocollo.Pregresso = false;

            if (string.IsNullOrEmpty(tbMod_NumeroProtocollo.Text.Trim()))
            {
                tbMod_NumeroProtocollo.Text = Protocolli_BLL.Instance.getNumeroProtocollo();
            }
            protocollo.Numero_protocollo = BasePage.ValidaCampo(tbMod_NumeroProtocollo, "", true, ref esito);
            //protocollo.PathDocumento = BasePage.ValidaCampo(tbMod_NomeFile, "", false, ref esito); 
            protocollo.PathDocumento = (string)Session["NOME_FILE"];
            protocollo.Protocollo_riferimento = BasePage.ValidaCampo(tbMod_ProtocolloRiferimento, "", false, ref esito);
            protocollo.Cliente = BasePage.ValidaCampo(tbMod_Cliente, "", false, ref esito);
            if (string.IsNullOrEmpty(tbMod_IdCliente.Value))
            {
                protocollo.Id_cliente = null;
            }
            else
            {
                protocollo.Id_cliente = Convert.ToInt32(tbMod_IdCliente.Value);
            }
            protocollo.Produzione = BasePage.ValidaCampo(tbMod_Produzione, "", false, ref esito);
            protocollo.Data_inizio_lavorazione = BasePage.ValidaCampo(tbMod_DataLavorazione, DateTime.Now, true, ref esito);
            protocollo.Codice_lavoro = BasePage.ValidaCampo(tbMod_CodiceLavoro, "", false, ref esito);
            protocollo.Lavorazione = BasePage.ValidaCampo(tbMod_Lavorazione, "", true, ref esito);
            protocollo.Descrizione = BasePage.ValidaCampo(tbMod_Descrizione, "", true, ref esito);
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
                //string nomeFileToSave = DateTime.Now.Ticks.ToString() + "_" + Path.GetFileName(e.filename);
                if (!string.IsNullOrEmpty(tbMod_NumeroProtocollo.Text.Trim())){
                    nomeFileToSave = "Protocollo_" + tbMod_NumeroProtocollo.Text.Trim() + Path.GetExtension(e.filename);
                }
                
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
            tbMod_Descrizione.CssClass = tbMod_Descrizione.CssClass.Replace("erroreValidazione", "");
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
            DataTable dtLavorazioni = Base_DAL.GetDatiBySql(queryRicerca, ref esito);
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
                string produzioneSelezionata = e.Row.Cells[6].Text;
                string lavorazioneSelezionata = e.Row.Cells[7].Text;


                ImageButton myButtonEdit = e.Row.FindControl("imgSelect") as ImageButton;
                //myButtonEdit.Attributes.Add("onclick", "associaCodiceLavorazione('" + codLavSelezionato.Replace("&nbsp;","") + "','" + clienteFornitoreSelezionato.Replace("&nbsp;", "") + "');");
                myButtonEdit.Attributes.Add("onclick", "associaCodiceLavorazione('" + codLavSelezionato.Replace("&nbsp;", "") + "','" + clienteFornitoreSelezionato.Replace("&nbsp;", "") + "','" + produzioneSelezionata.Replace("&nbsp;", "") + "','" + lavorazioneSelezionata.Replace("&nbsp;", "") + "');");
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
            string queryRicerca = "SELECT ID, RAGIONESOCIALE FROM anag_clienti_fornitori WHERE ragioneSociale LIKE '%@ragioneSociale%'";

            queryRicerca = queryRicerca.Replace("@ragioneSociale", tbSearch_RagioneSociale.Text.Trim().Replace("'", "''"));


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
            }
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
            // DA CONFIGURAZIONE SCELGO SE VISUALIZZARE SUBITO GLI ULTIMI PROTOCOLLI
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["VISUALIZZA_ULTIMI_PROTOCOLLI"]))
            {
                btnRicercaProtocollo_Click(null, null);
            }
        }
    }
}