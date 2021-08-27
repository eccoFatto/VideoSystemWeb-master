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
using iText;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iTextSharp;
using System.Collections;
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

                CaricaCombo();


                ddlTipoCategoria.Items.Clear();
                ddlTipoCategoria.Items.Add("");
                foreach (Tipologica tipologiaCategoria in SessionManager.ListaTipiCategorieMagazzino)
                {
                    System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem
                    {
                        Text = tipologiaCategoria.nome,
                        Value = tipologiaCategoria.id.ToString()
                    };

                    ddlTipoCategoria.Items.Add(item);

                }

                riempiComboSubCategoria(0);
                riempiComboGruppo(0);

                // SE UTENTE ABILITATO ALLE MODIFICHE FACCIO VEDERE I PULSANTI DI MODIFICA
                abilitaBottoni(basePage.AbilitazioneInScrittura());
                btnRicercaDocumentoTrasporto_Click(null, null);
            }

            ScriptManager.RegisterStartupScript(this, typeof(Page), "coerenzaDate", "controlloCoerenzaDate('" + tbDataTrasporto.ClientID + "', '" + tbDataTrasportoA.ClientID + "');", true);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "chiudiLoader", script: "$('.loader').hide();", addScriptTags: true);
        }

        private void CaricaCombo()
        {
            Esito esito = new Esito();

            #region TIPO DocumentoTrasporto
            foreach (Tipologica tipologiaDocumentoTrasporto in SessionManager.ListaTipiProtocolli)
            {
                System.Web.UI.WebControls.ListItem itemMod = new System.Web.UI.WebControls.ListItem();
                itemMod.Text = tipologiaDocumentoTrasporto.nome;
                itemMod.Value = tipologiaDocumentoTrasporto.id.ToString();

                //cmbMod_Tipologia.Items.Add(itemMod);
            }
            #endregion
        }

        private void riempiComboSubCategoria(int idCategoria)
        {
            ddlTipoSubCategoria.Items.Clear();
            ddlTipoSubCategoria.Items.Add("");

            string queryRicercaSubcategoria = "select * from tipo_subcategoria_magazzino where attivo = 1 ";
            if (idCategoria > 0)
            {
                queryRicercaSubcategoria += "and id in (select distinct id_subcategoria from mag_attrezzature where id_categoria=" + idCategoria.ToString() + ") ";
            }
            queryRicercaSubcategoria += "order by nome";
            Esito esito = new Esito();
            DataTable dtSubCategorie = Base_DAL.GetDatiBySql(queryRicercaSubcategoria, ref esito);

            foreach (DataRow tipologiaSubCategoria in dtSubCategorie.Rows)
            {
                System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem
                {
                    Text = tipologiaSubCategoria["nome"].ToString(),
                    Value = tipologiaSubCategoria["id"].ToString()
                };

                ddlTipoSubCategoria.Items.Add(item);

            }
        }

        private void riempiComboGruppo(int idSubCategoria)
        {
            ddlTipoGruppoMagazzino.Items.Clear();
            ddlTipoGruppoMagazzino.Items.Add("");

            string queryRicercaGruppo = "select * from tipo_gruppo_magazzino where attivo = 1 ";
            if (idSubCategoria > 0)
            {
                queryRicercaGruppo += "and id in (select distinct id_gruppo_magazzino from mag_attrezzature where id_subcategoria=" + idSubCategoria.ToString() + ") ";
            }
            queryRicercaGruppo += "order by nome";
            Esito esito = new Esito();
            DataTable dtGruppi = Base_DAL.GetDatiBySql(queryRicercaGruppo, ref esito);

            foreach (DataRow tipologiaGruppo in dtGruppi.Rows)
            {
                System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem
                {
                    Text = tipologiaGruppo["nome"].ToString(),
                    Value = tipologiaGruppo["id"].ToString()
                };

                ddlTipoGruppoMagazzino.Items.Add(item);

            }
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
                    divBtnInserisciAttrezzatura.Visible = false;
                    if (!basePage.AbilitazioneInScrittura())
                    {
                        btnGestisciDocumentoTrasporto.Visible = false;
                    }
                    else
                    {
                        btnGestisciDocumentoTrasporto.Visible = true;
                    }
                    imgbtnSelectCliente.Attributes.Add("disabled", "");

                    break;
                case "INSERIMENTO":
                    btnInserisciDocumentoTrasporto.Visible = true;
                    btnModificaDocumentoTrasporto.Visible = false;
                    btnEliminaDocumentoTrasporto.Visible = false;
                    btnAnnullaDocumentoTrasporto.Visible = false;
                    btnGestisciDocumentoTrasporto.Visible = false;
                    divBtnInserisciAttrezzatura.Visible = false;
                    imgbtnSelectCliente.Attributes.Remove("disabled");
                    break;
                case "MODIFICA":
                    btnInserisciDocumentoTrasporto.Visible = false;
                    btnModificaDocumentoTrasporto.Visible = true;
                    btnEliminaDocumentoTrasporto.Visible = true;
                    btnAnnullaDocumentoTrasporto.Visible = true;
                    btnGestisciDocumentoTrasporto.Visible = false;
                    divBtnInserisciAttrezzatura.Visible = true;
                    imgbtnSelectCliente.Attributes.Remove("disabled");
                    break;
                case "ANNULLAMENTO":
                    btnInserisciDocumentoTrasporto.Visible = false;
                    btnModificaDocumentoTrasporto.Visible = false;
                    btnEliminaDocumentoTrasporto.Visible = false;
                    btnAnnullaDocumentoTrasporto.Visible = false;
                    btnGestisciDocumentoTrasporto.Visible = true;
                    divBtnInserisciAttrezzatura.Visible = false;
                    imgbtnSelectCliente.Attributes.Add("disabled", "");
                    break;
                default:
                    btnInserisciDocumentoTrasporto.Visible = false;
                    btnModificaDocumentoTrasporto.Visible = false;
                    btnEliminaDocumentoTrasporto.Visible = false;
                    btnAnnullaDocumentoTrasporto.Visible = false;
                    btnGestisciDocumentoTrasporto.Visible = true;
                    divBtnInserisciAttrezzatura.Visible = false;
                    imgbtnSelectCliente.Attributes.Add("disabled", "");
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
            gv_attrezzature.DataSource = null;
            gv_attrezzature.DataBind();
            //tbMod_NumeroDocumentoTrasporto.Text = Protocolli_BLL.Instance.getNumeroDocumentoTrasporto();

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
                    hf_idDocTras.Value = "";
                    log.Error(esito.Descrizione);
                    basePage.ShowError(esito.Descrizione);
                }
                //ShowPopMessage("Inserito DocumentoTrasporto n. " + documentoTrasporto.NumeroDocumentoTrasporto);
                ShowSuccess("Inserito DocumentoTrasporto n. " + documentoTrasporto.NumeroDocumentoTrasporto + " Prot: " + documentoTrasporto.Numero_protocollo);

                btnEditDocumentoTrasporto_Click(null, null);

                gestisciPulsantiDocumentoTrasporto("MODIFICA");

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

            queryRicerca = queryRicerca.Replace("@numeroDocumentoTrasporto", tbNumeroDocTrasporto.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@causale", tbCausale.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@destinatario", tbDestinatario.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@indirizzo", tbIndirizzo.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@comune", tbComune.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@partitaIva", tbPartitaIva.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@trasportatore", tbTrasportatore.Text.Trim().Replace("'", "''"));

            string queryDocumentoTrasportoDataTras = "";
            if (!string.IsNullOrEmpty(tbDataTrasporto.Text))
            {
                DateTime dataDa = Convert.ToDateTime(tbDataTrasporto.Text);
                DateTime dataA = DateTime.Now;
                queryDocumentoTrasportoDataTras = " and dataTrasporto between '@dataDa' and '@DataA' ";
                if (!string.IsNullOrEmpty(tbDataTrasportoA.Text))
                {
                    dataA = Convert.ToDateTime(tbDataTrasportoA.Text);
                }
                queryDocumentoTrasportoDataTras = queryDocumentoTrasportoDataTras.Replace("@dataDa", dataDa.ToString("yyyy-MM-ddT00:00:00.000"));
                queryDocumentoTrasportoDataTras = queryDocumentoTrasportoDataTras.Replace("@DataA", dataA.ToString("yyyy-MM-ddT23:59:59.999"));
            }
            queryRicerca = queryRicerca.Replace("@dataTrasporto", queryDocumentoTrasportoDataTras);


            Esito esito = new Esito();
            DataTable dtDocumentiTrasporto = Base_DAL.GetDatiBySql(queryRicerca, ref esito);

            Session["TaskTable"] = dtDocumentiTrasporto;
            gv_documenti_trasporto.DataSource = Session["TaskTable"];
            gv_documenti_trasporto.DataBind();
            tbTotElementiGriglia.Text = dtDocumentiTrasporto.Rows.Count.ToString("###,##0");
        }

        protected void gv_documenti_trasporto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 1) e.Row.Cells[1].Visible = false;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                // PRENDO IL PATH DELL'ALLEGATO SE C'E'
                //string pathDocumento = "";
                //ImageButton myButton = e.Row.FindControl("btnOpenDoc") as ImageButton;
                //if (!string.IsNullOrEmpty(pathDocumento) && !pathDocumento.Equals("&nbsp;"))
                //{

                //    string   pathRelativo = ConfigurationManager.AppSettings["PATH_DOCUMENTI_TRASPORTO"].Replace("~", "");
                //    string pathCompleto = pathRelativo + pathDocumento;
                //    myButton.Attributes.Add("onclick", "window.open('" + pathCompleto + "');");
                //}
                //else
                //{
                //    myButton.Attributes.Add("disabled", "true");
                //}

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
            tbMod_NumeroDocumentoTrasporto.Text = "";
            tbMod_NumeroProtocollo.Text = "";
            tbMod_Cap.Text = "";
            tbMod_Causale.Text = "";
            tbMod_Comune.Text = "";
            tbMod_DataTrasporto.Text = DateTime.Today.ToShortDateString();
            tbMod_destinatario.Text = "";
            tbMod_Indirizzo.Text = "";
            tbMod_Nazione.Text = "";
            tbMod_NumeroCivico.Text = "";
            tbMod_NumeroColli.Text = "";
            tbMod_PartitaIva.Text = "";
            tbMod_Peso.Text = "";
            tbMod_Provincia.Text = "";
            tbMod_Trasportatore.Text = "";
            cmbMod_TipoIndirizzo.SelectedIndex = 0;

        }

        private void AttivaDisattivaModificaDocumentoTrasporto(bool attivaModifica)
        {
            tbMod_NumeroDocumentoTrasporto.ReadOnly = true;
            tbMod_Causale.ReadOnly = attivaModifica;
            //tbMod_Cap.ReadOnly = attivaModifica;
            //tbMod_Comune.ReadOnly = attivaModifica;
            tbMod_DataTrasporto.ReadOnly = attivaModifica;
            //tbMod_destinatario.ReadOnly = attivaModifica;
            //tbMod_Indirizzo.ReadOnly = attivaModifica;
            //tbMod_Nazione.ReadOnly = attivaModifica;
            //tbMod_NumeroCivico.ReadOnly = attivaModifica;
            tbMod_NumeroColli.ReadOnly = attivaModifica;
            //tbMod_PartitaIva.ReadOnly = attivaModifica;
            tbMod_Peso.ReadOnly = attivaModifica;
            //tbMod_Provincia.ReadOnly = attivaModifica;
            tbMod_Trasportatore.ReadOnly = attivaModifica;

            //tbMod_Cap.ReadOnly = true;
            //tbMod_Comune.ReadOnly = true;
            //tbMod_destinatario.ReadOnly = true;
            //tbMod_Indirizzo.ReadOnly = true;
            //tbMod_Nazione.ReadOnly = true;
            //tbMod_NumeroCivico.ReadOnly = true;
            //tbMod_PartitaIva.ReadOnly = true;
            //tbMod_Provincia.ReadOnly = true;

            tbMod_Cap.ReadOnly = attivaModifica;
            tbMod_Comune.ReadOnly = attivaModifica;
            
            tbMod_Indirizzo.ReadOnly = attivaModifica;
            tbMod_Nazione.ReadOnly = attivaModifica;
            tbMod_NumeroCivico.ReadOnly = attivaModifica;
            
            tbMod_Provincia.ReadOnly = attivaModifica;

            if (attivaModifica)
            {
                cmbMod_TipoIndirizzo.Attributes.Add("disabled", "");
            }
            else
            {
                cmbMod_TipoIndirizzo.Attributes.Remove("disabled");
            }
            //cmbMod_TipoIndirizzo.Attributes.Add("disabled", "");
        }

        private void editDocumentoTrasportoVuoto()
        {
            Esito esito = new Esito();
            pulisciCampiDettaglio();
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
                    tbMod_NumeroProtocollo.Text = documentoTrasporto.Numero_protocollo;
                    tbMod_DataTrasporto.Text = "";
                    if (documentoTrasporto.DataTrasporto != null)
                    {
                        tbMod_DataTrasporto.Text = ((DateTime)documentoTrasporto.DataTrasporto).ToString("dd/MM/yyyy");
                    }
                    tbMod_Cap.Text = documentoTrasporto.Cap;
                    tbMod_Causale.Text = documentoTrasporto.Causale;
                    tbMod_Comune.Text = documentoTrasporto.Comune;
                    tbMod_destinatario.Text = documentoTrasporto.Destinatario;
                    tbMod_Indirizzo.Text = documentoTrasporto.Indirizzo;
                    tbMod_Nazione.Text = documentoTrasporto.Nazione;
                    tbMod_NumeroCivico.Text = documentoTrasporto.NumeroCivico;
                    tbMod_NumeroColli.Text = documentoTrasporto.NumeroColli.ToString();
                    tbMod_PartitaIva.Text = documentoTrasporto.PartitaIva;
                    tbMod_Peso.Text = documentoTrasporto.Peso;
                    tbMod_Provincia.Text = documentoTrasporto.Provincia;
                    tbMod_Trasportatore.Text = documentoTrasporto.Trasportatore;
                    cmbMod_TipoIndirizzo.Text = documentoTrasporto.TipoIndirizzo;

                    List<AttrezzatureTrasporto> listaAttrezzature = documentoTrasporto.AttrezzatureTrasporto;
                    gv_attrezzature.DataSource = listaAttrezzature;
                    gv_attrezzature.DataBind();
                    btnViewAttachement.Enabled = true;
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
            AttivaDisattivaModificaDocumentoTrasporto(false);
            gestisciPulsantiDocumentoTrasporto("MODIFICA");
        }

        private VideoSystemWeb.Entity.DocumentiTrasporto CreaOggettoDocumentoTrasporto(ref Esito esito)
        {
            VideoSystemWeb.Entity.DocumentiTrasporto documentoTrasporto = new VideoSystemWeb.Entity.DocumentiTrasporto();

            if (string.IsNullOrEmpty((string)ViewState["idDocumentoTrasporto"]))
            {
                ViewState["idDocumentoTrasporto"] = "0";
            }

            documentoTrasporto.Id = Convert.ToInt64(ViewState["idDocumentoTrasporto"].ToString());

            documentoTrasporto.Cap = tbMod_Cap.Text.Trim();
            documentoTrasporto.Causale = tbMod_Causale.Text.Trim();
            documentoTrasporto.Comune = tbMod_Comune.Text.Trim();
            //documentoTrasporto.DataTrasporto =  Convert.ToDateTime(tbMod_DataTrasporto.Text.Trim());
            if (!string.IsNullOrEmpty(BasePage.ValidaCampo(tbMod_DataTrasporto, "", true, ref esito))){
                documentoTrasporto.DataTrasporto = Convert.ToDateTime(tbMod_DataTrasporto.Text.Trim());
            }
            documentoTrasporto.Indirizzo = BasePage.ValidaCampo(tbMod_Indirizzo, "", true, ref esito);
            documentoTrasporto.Nazione = tbMod_Nazione.Text.Trim();
            documentoTrasporto.NumeroCivico = tbMod_NumeroCivico.Text.Trim();

            //documentoTrasporto.NumeroColli = Convert.ToInt64(tbMod_NumeroColli.Text.Trim());
            string numeroColli = BasePage.ValidaCampo(tbMod_NumeroColli, "1", true, ref esito);
            if (!string.IsNullOrEmpty(numeroColli))
            {
                documentoTrasporto.NumeroColli = Convert.ToInt64(numeroColli);
            }
            documentoTrasporto.PartitaIva = tbMod_PartitaIva.Text.Trim();
            documentoTrasporto.Provincia = tbMod_Provincia.Text.Trim();
            documentoTrasporto.TipoIndirizzo = cmbMod_TipoIndirizzo.Text;
            documentoTrasporto.Trasportatore = BasePage.ValidaCampo(tbMod_Trasportatore, "", true, ref esito);
            documentoTrasporto.Destinatario = BasePage.ValidaCampo(tbMod_destinatario, "", true, ref esito);
            documentoTrasporto.Peso = BasePage.ValidaCampo(tbMod_Peso, "", true, ref esito);

            if (string.IsNullOrEmpty(tbMod_NumeroDocumentoTrasporto.Text.Trim()))
            {
                tbMod_NumeroDocumentoTrasporto.Text = Protocolli_BLL.Instance.getNumeroDocumentoTrasporto();
            }
            documentoTrasporto.NumeroDocumentoTrasporto = BasePage.ValidaCampo(tbMod_NumeroDocumentoTrasporto, "", true, ref esito);

            if (string.IsNullOrEmpty(tbMod_NumeroProtocollo.Text.Trim()))
            {
                tbMod_NumeroProtocollo.Text = Protocolli_BLL.Instance.getNumeroProtocollo();
            }
            documentoTrasporto.Numero_protocollo = tbMod_NumeroProtocollo.Text.Trim();


            return documentoTrasporto;
        }

        protected void imgbtnCreateNewCodLav_Click(object sender, ImageClickEventArgs e)
        {
            //tbMod_CodiceLavoro.Text = Protocolli_BLL.Instance.getCodLavFormattato();
        }

        protected void btnChiudiPopup_Click(object sender, EventArgs e)
        {
            pnlContainer.Visible = false;
            btnRicercaDocumentoTrasporto_Click(null, null);
        }

        private void NascondiErroriValidazione()
        {
            tbMod_NumeroDocumentoTrasporto.CssClass = tbMod_NumeroDocumentoTrasporto.CssClass.Replace("erroreValidazione", "");
            tbMod_Causale.CssClass = tbMod_Causale.CssClass.Replace("erroreValidazione", "");
            tbMod_Cap.CssClass = tbMod_Cap.CssClass.Replace("erroreValidazione", "");
            tbMod_Comune.CssClass = tbMod_Comune.CssClass.Replace("erroreValidazione", "");
            tbMod_DataTrasporto.CssClass = tbMod_DataTrasporto.CssClass.Replace("erroreValidazione", "");
            tbMod_destinatario.CssClass = tbMod_destinatario.CssClass.Replace("erroreValidazione", "");
            tbMod_Indirizzo.CssClass = tbMod_Indirizzo.CssClass.Replace("erroreValidazione", "");
            tbMod_Nazione.CssClass = tbMod_Nazione.CssClass.Replace("erroreValidazione", "");
            tbMod_NumeroCivico.CssClass = tbMod_NumeroCivico.CssClass.Replace("erroreValidazione", "");
            tbMod_NumeroColli.CssClass = tbMod_NumeroColli.CssClass.Replace("erroreValidazione", "");
            tbMod_PartitaIva.CssClass = tbMod_PartitaIva.CssClass.Replace("erroreValidazione", "");
            tbMod_Peso.CssClass = tbMod_Peso.CssClass.Replace("erroreValidazione", "");
            tbMod_Provincia.CssClass = tbMod_Provincia.CssClass.Replace("erroreValidazione", "");
            tbMod_Trasportatore.CssClass = tbMod_Trasportatore.CssClass.Replace("erroreValidazione", "");
        }

        private void abilitaBottoni(bool utenteAbilitatoInScrittura)
        {
            //annullaModifiche();
            if (!utenteAbilitatoInScrittura)
            {
                divBtnInserisciDocumentoTrasporto.Visible = false;
                btnModificaDocumentoTrasporto.Visible = false;
                btnAnnullaDocumentoTrasporto.Visible = false;
                btnEliminaDocumentoTrasporto.Visible = false;
                btnGestisciDocumentoTrasporto.Visible = false;

            }
            else
            {
                divBtnInserisciDocumentoTrasporto.Visible = true;
                btnModificaDocumentoTrasporto.Visible = true;
                btnAnnullaDocumentoTrasporto.Visible = false;
                 btnEliminaDocumentoTrasporto.Visible = false;
                btnGestisciDocumentoTrasporto.Visible = true;
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

        protected void btnRicercaClienti_Click(object sender, EventArgs e)
        {
            string queryRicerca = "";
            switch (ddlSceltaClienteCollaboratore.Text)
            {
                case "Cliente":
                    queryRicerca = "SELECT ID, RAGIONESOCIALE as [Ragione Sociale] FROM anag_clienti_fornitori WHERE attivo = 1 AND ragioneSociale LIKE '%@ragioneSociale%'";
                    queryRicerca = queryRicerca.Replace("@ragioneSociale", tbSearch_RagioneSociale.Text.Trim().Replace("'", "''"));
                    break;
                case "Collaboratore":
                    queryRicerca = "SELECT ID, COGNOME + ' ' + NOME as [Ragione Sociale] FROM anag_collaboratori WHERE attivo = 1 AND cognome LIKE '%@ragioneSociale%'";
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
            try
            {
                if (string.IsNullOrEmpty(tbMod_IdCliente.Value) || tbMod_IdCliente.Value.Equals("0"))
                {
                    tbMod_destinatario.Text = tbSearch_RagioneSociale.Text.Trim();
                    tbMod_IdCliente.Value = null;
                    PanelClienti.Visible = false;
                }
                else
                {

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
                                tbMod_destinatario.Text = cliente.RagioneSociale;

                                cmbMod_TipoIndirizzo.Text = cliente.TipoIndirizzoLegale;
                                tbMod_Indirizzo.Text = cliente.IndirizzoLegale;
                                tbMod_Cap.Text = cliente.CapLegale;
                                tbMod_Comune.Text = cliente.ComuneLegale;
                                tbMod_NumeroCivico.Text = cliente.NumeroCivicoLegale;
                                tbMod_Provincia.Text = cliente.ProvinciaLegale;
                                tbMod_PartitaIva.Text = cliente.PartitaIva;

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
                                tbMod_destinatario.Text = collaboratore.Cognome + " " + collaboratore.Nome;
                                if (collaboratore.Indirizzi.Count > 0)
                                {
                                    cmbMod_TipoIndirizzo.Text = collaboratore.Indirizzi[0].Tipo;
                                    tbMod_Indirizzo.Text = collaboratore.Indirizzi[0].Indirizzo;
                                    tbMod_Cap.Text = collaboratore.Indirizzi[0].Cap;
                                    tbMod_Comune.Text = collaboratore.Indirizzi[0].Comune;
                                    tbMod_NumeroCivico.Text = collaboratore.Indirizzi[0].NumeroCivico;
                                    tbMod_Provincia.Text = collaboratore.Indirizzi[0].Provincia;
                                }

                                PanelClienti.Visible = false;
                            }
                            break;
                        default:
                            break;
                    }


                }

            }
            catch (Exception ex)
            {

                ShowError("btnAssociaClienteServer_Click" + Environment.NewLine + ex.Message);
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

        protected void gvMagazzino_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // PRENDO ID,COD_VS, DESCRIZIONE E LI PASSO ALLA FUNZIONE
                //string idAttrezzaturaSelezionata = e.Row.Cells[1].Text;
                //string codVsSelezionato = e.Row.Cells[2].Text;
                //string descrizioneAttrezzaturaSelezionata = e.Row.Cells[3].Text;
            }

        }

        protected void gvMagazzino_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMagazzino.PageIndex = e.NewPageIndex;
            btnRicercaMagazzino_Click(null, null);

        }

        protected void btnChiudiPopupMagazzinoServer_Click(object sender, EventArgs e)
        {
            PanelMagazzino.Visible = false;
        }

        protected void btnAssociaMagazzinoServer_Click(object sender, EventArgs e)
        {
            PanelMagazzino.Visible = false;
        }

        protected void btnRicercaMagazzino_Click(object sender, EventArgs e)
        {
            Session[SessionManager.LISTA_ATTREZZATURE_SELEZIONATE_DOC_TRASPORTO] = null;
            
            //queryRicerca = "SELECT ID, COD_VS, SERIALE, DESCRIZIONE + ' ' + MODELLO as DESCRIZIONE FROM mag_attrezzature WHERE descrizione LIKE '%@descrizione%' AND COD_VS LIKE '%@codiceVideoSystem%' AND SERIALE LIKE '%@seriale%' @campiTendina ";
            string queryRicerca = ConfigurationManager.AppSettings["QUERY_SEARCH_ATTREZZATURE"];
            queryRicerca = queryRicerca.Replace("@codiceVS", tbSearch_CodiceVideosystem.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@descrizione", tbSearch_DescMagazzino.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@seriale", tbSearch_Seriale.Text.Trim().Replace("'", "''"));

            queryRicerca = queryRicerca.Replace("@marca", "");
            queryRicerca = queryRicerca.Replace("@modello", "");

            // SELEZIONO I CAMPI DROPDOWN SE VALORIZZATI
            string queryRicercaCampiDropDown = "";
            if (!string.IsNullOrEmpty(ddlTipoCategoria.SelectedItem.Text))
            {
                queryRicercaCampiDropDown += " and cat.id=" + ddlTipoCategoria.SelectedValue + " ";
            }
            if (!string.IsNullOrEmpty(ddlTipoSubCategoria.SelectedItem.Text))
            {
                queryRicercaCampiDropDown += " and sub.id=" + ddlTipoSubCategoria.SelectedValue + " ";
            }
            if (!string.IsNullOrEmpty(ddlTipoGruppoMagazzino.SelectedItem.Text))
            {
                queryRicercaCampiDropDown += " and gruppo.id=" + ddlTipoGruppoMagazzino.SelectedValue + " ";
            }
            queryRicerca = queryRicerca.Replace("@campiTendina", queryRicercaCampiDropDown.Trim());

            queryRicerca = queryRicerca.Replace("@dataAcquisto", "");

            Esito esito = new Esito();
            DataTable dtMagazzino = Base_DAL.GetDatiBySql(queryRicerca, ref esito);
            gvMagazzino.DataSource = dtMagazzino;
            gvMagazzino.DataBind();

        }

        protected void btnInsAttrezzaturaMagazzino_Click(object sender, EventArgs e)
        {
            Session[SessionManager.LISTA_ATTREZZATURE_SELEZIONATE_DOC_TRASPORTO] = null;
            gvMagazzino.DataSource = null;
            gvMagazzino.DataBind();
            PanelMagazzino.Visible = true;
        }

        protected void gv_attrezzature_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gv_attrezzature_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void gv_attrezzature_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "ELIMINA_RIGA":
                    int id = Convert.ToInt32(e.CommandArgument);
                    Esito esito = DocumentiTrasporto_BLL.Instance.EliminaAttrezzaturaTrasporto(id);
                    if (esito.Codice == 0)
                    {

                        List<AttrezzatureTrasporto> listaAttrezzature = DocumentiTrasporto_BLL.Instance.getAttrezzatureTrasportoByIdDocumentoTrasporto(ref esito, Convert.ToInt64(ViewState["idDocumentoTrasporto"]));
                        gv_attrezzature.DataSource = listaAttrezzature;
                        gv_attrezzature.DataBind();

                    }
                    else
                    {
                        basePage.ShowError(esito.Descrizione);
                    }
                    break;

                default:
                    break;
            }
        }

        protected void imgSelectMagazzino_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                int rowIndex = ((sender as ImageButton).NamingContainer as GridViewRow).RowIndex;
                int id = Convert.ToInt32(gvMagazzino.DataKeys[rowIndex].Values[0]);
                Esito esito = new Esito();
                AttrezzatureMagazzino attrezzaturaMagazzino = AttrezzatureMagazzino_BLL.Instance.getAttrezzaturaById(ref esito, id);
                if (esito.Codice == 0)
                {
                    string codvs = "";
                    if (!string.IsNullOrEmpty(attrezzaturaMagazzino.Cod_vs)) codvs = attrezzaturaMagazzino.Cod_vs;
                    string seriale = "";
                    if (!string.IsNullOrEmpty(attrezzaturaMagazzino.Seriale)) seriale = attrezzaturaMagazzino.Seriale;
                    string descrizione = "";
                    if (!string.IsNullOrEmpty(attrezzaturaMagazzino.Descrizione)) descrizione = attrezzaturaMagazzino.Descrizione;
                    string modello = "";
                    if (!string.IsNullOrEmpty(attrezzaturaMagazzino.Modello)) modello = attrezzaturaMagazzino.Modello;

                    AttrezzatureTrasporto attrezzaturaTrasporto = new AttrezzatureTrasporto();
                    attrezzaturaTrasporto.Cod_vs = codvs;
                    attrezzaturaTrasporto.Seriale = seriale;
                    attrezzaturaTrasporto.Descrizione = descrizione + " " + modello;
                    attrezzaturaTrasporto.IdMagAttrezzature = attrezzaturaMagazzino.Id;
                    //attrezzaturaTrasporto.IdDocumentoTrasporto = Convert.ToInt64(hf_idDocTras.Value);
                    attrezzaturaTrasporto.IdDocumentoTrasporto = Convert.ToInt64(ViewState["idDocumentoTrasporto"]);
                    attrezzaturaTrasporto.Quantita = Convert.ToInt32(tbIns_Quantita.Text.Trim());
                    int idAttrezzaturaTrasportoNew = DocumentiTrasporto_BLL.Instance.CreaAttrezzaturaTrasporto(attrezzaturaTrasporto, ref esito);
                    if (esito.Codice == 0)
                    {
                        List<AttrezzatureTrasporto> listaAttrezzature = DocumentiTrasporto_BLL.Instance.getAttrezzatureTrasportoByIdDocumentoTrasporto(ref esito, Convert.ToInt64(ViewState["idDocumentoTrasporto"]));
                        gv_attrezzature.DataSource = listaAttrezzature;
                        gv_attrezzature.DataBind();
                        PanelMagazzino.Visible = false;
                    }
                    else
                    {
                        basePage.ShowError(esito.Descrizione);
                    }
                }
                else
                {
                    basePage.ShowError(esito.Descrizione);
                }
            }
            catch (Exception ex)
            {

                basePage.ShowError(ex.Message);
            }
        }

        protected void btnInsertMagazzino_Click(object sender, EventArgs e)
        {
            try
            {
                Esito esito = new Esito();
                AttrezzatureTrasporto attrezzaturaTrasporto = new AttrezzatureTrasporto();
                attrezzaturaTrasporto.Cod_vs = tbSearch_CodiceVideosystem.Text;
                attrezzaturaTrasporto.Descrizione = tbSearch_DescMagazzino.Text;
                attrezzaturaTrasporto.Seriale = tbSearch_Seriale.Text;
                attrezzaturaTrasporto.IdMagAttrezzature = 0;
                attrezzaturaTrasporto.IdDocumentoTrasporto = Convert.ToInt64(ViewState["idDocumentoTrasporto"]);
                attrezzaturaTrasporto.Quantita = Convert.ToInt32(tbIns_Quantita.Text.Trim());
                int idAttrezzaturaTrasportoNew = DocumentiTrasporto_BLL.Instance.CreaAttrezzaturaTrasporto(attrezzaturaTrasporto, ref esito);
                if (esito.Codice == 0)
                {
                    
                    List<AttrezzatureTrasporto> listaAttrezzature = DocumentiTrasporto_BLL.Instance.getAttrezzatureTrasportoByIdDocumentoTrasporto(ref esito, Convert.ToInt64(ViewState["idDocumentoTrasporto"]));
                    gv_attrezzature.DataSource = listaAttrezzature;
                    gv_attrezzature.DataBind();
                    PanelMagazzino.Visible = false;
                }
                else
                {
                    basePage.ShowError(esito.Descrizione);
                }

            }
            catch (Exception ex)
            {

                basePage.ShowError(ex.Message);
            }
        }


        private void stampaDocumentoTrasporto(int idDocumentoTrasporto)
        {
            try
            {
                Esito esito = new Esito();

                Entity.DocumentiTrasporto documentoTrasporto = DocumentiTrasporto_BLL.Instance.getDocumentoTrasportoById(ref esito,idDocumentoTrasporto);

                if (esito.Codice == 0 && documentoTrasporto.AttrezzatureTrasporto != null && documentoTrasporto.AttrezzatureTrasporto.Count > 0)
                {
                    string descrizioneProtocolloDDT = "DDT - ";
                    // LEGGO I PARAMETRI DI VS
                    Config cfAppo = Config_BLL.Instance.getConfig(ref esito, "PARTITA_IVA");
                    string pIvaVs = cfAppo.valore;
                    cfAppo = Config_BLL.Instance.getConfig(ref esito, "DENOMINAZIONE");
                    string denominazioneVs = cfAppo.valore;
                    cfAppo = Config_BLL.Instance.getConfig(ref esito, "TOPONIMO");
                    string toponimoVs = cfAppo.valore;
                    cfAppo = Config_BLL.Instance.getConfig(ref esito, "INDIRIZZO");
                    string indirizzoVs = cfAppo.valore;
                    cfAppo = Config_BLL.Instance.getConfig(ref esito, "CIVICO");
                    string civicoVs = cfAppo.valore;
                    cfAppo = Config_BLL.Instance.getConfig(ref esito, "CAP");
                    string capVs = cfAppo.valore;
                    cfAppo = Config_BLL.Instance.getConfig(ref esito, "CITTA");
                    string cittaVs = cfAppo.valore;
                    cfAppo = Config_BLL.Instance.getConfig(ref esito, "PROVINCIA");
                    string provinciaVs = cfAppo.valore;
                    cfAppo = Config_BLL.Instance.getConfig(ref esito, "EMAIL");
                    string emailVs = cfAppo.valore;

                    Protocolli protocolloTrasporto = new Protocolli();
                    int idTipoProtocollo = UtilityTipologiche.getElementByNome(UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_PROTOCOLLO), "DDT", ref esito).id;
                    List<Protocolli> listaProtocolli = Protocolli_BLL.Instance.getProtocolliByCodLavIdTipoProtocolloNumeroprotocollo("generico", idTipoProtocollo, documentoTrasporto.Numero_protocollo, ref esito, true);
                    if (listaProtocolli.Count > 0)
                    {
                        foreach (Protocolli protocollo in listaProtocolli)
                        {
                            protocolloTrasporto = protocollo;
                        }
                        //listaProtocolli.Clear();
                    }

                    string nomeFile = "DocumentoTrasporto_" + documentoTrasporto.NumeroDocumentoTrasporto + ".pdf";
                    string pathReport = ConfigurationManager.AppSettings["PATH_DOCUMENTI_PROTOCOLLO"] + nomeFile;
                    string mapPathReport = MapPath(ConfigurationManager.AppSettings["PATH_DOCUMENTI_PROTOCOLLO"]) + nomeFile;

                    //string prefissoUrl = Request.Url.Scheme + "://" + Request.Url.Authority;
                    iText.IO.Image.ImageData imageData = iText.IO.Image.ImageDataFactory.Create(MapPath("~/Images/logoVSP_trim.png"));
                    iText.IO.Image.ImageData imageDNV = iText.IO.Image.ImageDataFactory.Create(MapPath("~/Images/DNV_2008_ITA2.jpg"));


                    PdfWriter wr = new PdfWriter(mapPathReport);
                    PdfDocument doc = new PdfDocument(wr);
                    doc.SetDefaultPageSize(iText.Kernel.Geom.PageSize.A4);
                    //Document document = new Document(doc);
                    Document document = new Document(doc, iText.Kernel.Geom.PageSize.A4, false);

                    document.SetMargins(260, 30, 150, 30);

                    Paragraph pSpazio = new Paragraph(" ");
                    document.Add(pSpazio);


                    //iText.Kernel.Colors.Color coloreIntestazioni = new iText.Kernel.Colors.DeviceRgb(0, 225, 0);
                    // COLORE BLU VIDEOSYSTEM
                    iText.Kernel.Colors.Color coloreIntestazioni = new iText.Kernel.Colors.DeviceRgb(33, 150, 243);


                    // CREAZIONE GRIGLIA
                    iText.Layout.Element.Table tbGrigla = new iText.Layout.Element.Table(new float[] { 15, 25, 50, 10 }).SetWidth(530).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetFixedLayout();
                    Paragraph pGriglia;
                    Cell cellaGriglia;

                    // DETTAGLIO DOCUMENTO
                    pGriglia = new Paragraph("DOCUMENTO DI TRASPORTO nr " + documentoTrasporto.NumeroDocumentoTrasporto + " del " + documentoTrasporto.DataTrasporto.ToShortDateString()).SetFontSize(10).SetBold();
                    cellaGriglia = new iText.Layout.Element.Cell(1, 4).SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                    cellaGriglia.Add(pGriglia);
                    tbGrigla.AddHeaderCell(cellaGriglia);


                    // INTESTAZIONE CAMPI GRIGLIA ATTREZZATURE
                    pGriglia = new Paragraph("Codice").SetFontSize(10);
                    cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetPadding(5).SetBold();
                    cellaGriglia.Add(pGriglia);
                    tbGrigla.AddHeaderCell(cellaGriglia);

                    pGriglia = new Paragraph("Seriale").SetFontSize(10);
                    cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetBold();
                    cellaGriglia.Add(pGriglia);
                    tbGrigla.AddHeaderCell(cellaGriglia);

                    pGriglia = new Paragraph("Descrizione").SetFontSize(10);
                    cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetBold();
                    cellaGriglia.Add(pGriglia);
                    tbGrigla.AddHeaderCell(cellaGriglia);

                    pGriglia = new Paragraph("Quantità").SetFontSize(10);
                    cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetBold();
                    cellaGriglia.Add(pGriglia);
                    tbGrigla.AddHeaderCell(cellaGriglia);

                    //CICLO LISTA ATTREZZATURE
                    foreach (AttrezzatureTrasporto attrezzaturaTrasporto in documentoTrasporto.AttrezzatureTrasporto)
                    {
                        descrizioneProtocolloDDT += " " + attrezzaturaTrasporto.Descrizione;
                        pGriglia = new Paragraph(attrezzaturaTrasporto.Cod_vs).SetFontSize(9);
                        cellaGriglia = new Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10);
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        pGriglia = new Paragraph(attrezzaturaTrasporto.Seriale).SetFontSize(9);
                        cellaGriglia = new Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10);
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        pGriglia = new Paragraph(attrezzaturaTrasporto.Descrizione).SetFontSize(9);
                        cellaGriglia = new Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10);
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        pGriglia = new Paragraph(attrezzaturaTrasporto.Quantita.ToString("###")).SetFontSize(9);
                        cellaGriglia = new Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        
                    }

                    document.Add(tbGrigla);

                    int n = doc.GetNumberOfPages();
                    iText.Kernel.Geom.Rectangle pageSize = doc.GetPage(n).GetPageSize();

                    // AGGIUNGO CONTEGGIO PAGINE E FOOTER PER OGNI PAGINA
                    for (int i = 1; i <= n; i++)
                    {
                        iText.Layout.Element.Image image = new iText.Layout.Element.Image(imageData).ScaleAbsolute(90, 80).SetFixedPosition(i, 30, 740);
                        document.Add(image);

                        // CREAZIONE GRIGLIA INFORMAZIONI
                        iText.Layout.Element.Table tbGriglaInfo = new iText.Layout.Element.Table(new float[] { 70, 230 }).SetWidth(300).SetFixedPosition(i, 30, 620, 300);
                        Paragraph pGrigliaInfo = new Paragraph(cittaVs).SetFontSize(9).SetBold();
                        iText.Layout.Element.Cell cellaGrigliaInfo = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGrigliaInfo.Add(pGrigliaInfo);
                        tbGriglaInfo.AddCell(cellaGrigliaInfo);

                        pGrigliaInfo = new Paragraph(documentoTrasporto.DataTrasporto.ToLongDateString()).SetFontSize(9);
                        cellaGrigliaInfo = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGrigliaInfo.Add(pGrigliaInfo);
                        tbGriglaInfo.AddCell(cellaGrigliaInfo);

                        pGrigliaInfo = new Paragraph("Causale:").SetFontSize(9).SetBold();
                        cellaGrigliaInfo = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGrigliaInfo.Add(pGrigliaInfo);
                        tbGriglaInfo.AddCell(cellaGrigliaInfo);

                        pGrigliaInfo = new Paragraph(documentoTrasporto.Causale).SetFontSize(9);
                        cellaGrigliaInfo = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGrigliaInfo.Add(pGrigliaInfo);
                        tbGriglaInfo.AddCell(cellaGrigliaInfo);

                        pGrigliaInfo = new Paragraph("Numero Colli:").SetFontSize(9).SetBold();
                        cellaGrigliaInfo = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGrigliaInfo.Add(pGrigliaInfo);
                        tbGriglaInfo.AddCell(cellaGrigliaInfo);

                        pGrigliaInfo = new Paragraph(documentoTrasporto.NumeroColli.ToString()).SetFontSize(9);
                        cellaGrigliaInfo = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGrigliaInfo.Add(pGrigliaInfo);
                        tbGriglaInfo.AddCell(cellaGrigliaInfo);

                        pGrigliaInfo = new Paragraph("Peso:").SetFontSize(9).SetBold();
                        cellaGrigliaInfo = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGrigliaInfo.Add(pGrigliaInfo);
                        tbGriglaInfo.AddCell(cellaGrigliaInfo);

                        pGrigliaInfo = new Paragraph(documentoTrasporto.Peso).SetFontSize(9);
                        cellaGrigliaInfo = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGrigliaInfo.Add(pGrigliaInfo);
                        tbGriglaInfo.AddCell(cellaGrigliaInfo);

                        pGrigliaInfo = new Paragraph("Trasportatore:").SetFontSize(9).SetBold();
                        cellaGrigliaInfo = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGrigliaInfo.Add(pGrigliaInfo);
                        tbGriglaInfo.AddCell(cellaGrigliaInfo);

                        pGrigliaInfo = new Paragraph(documentoTrasporto.Trasportatore).SetFontSize(9);
                        cellaGrigliaInfo = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGrigliaInfo.Add(pGrigliaInfo);
                        tbGriglaInfo.AddCell(cellaGrigliaInfo);

                        document.Add(tbGriglaInfo);


                        // CREAZIONE GRIGLIA DESTINATARIO
                        iText.Layout.Element.Table tbGriglaDest = new iText.Layout.Element.Table(new float[] { 70, 230 }).SetWidth(300).SetFixedPosition(i, 350, 655, 300);
                        Paragraph pGrigliaDest = new Paragraph("Spettabile").SetFontSize(9).SetBold();
                        iText.Layout.Element.Cell cellaGrigliaDest = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGrigliaDest.Add(pGrigliaDest);
                        tbGriglaDest.AddCell(cellaGrigliaDest);

                        pGrigliaDest = new Paragraph(documentoTrasporto.Destinatario).SetFontSize(9);
                        cellaGrigliaDest = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGrigliaDest.Add(pGrigliaDest);
                        tbGriglaDest.AddCell(cellaGrigliaDest);

                        // INDIRIZZO DESTINATARIO
                        pGrigliaDest = new Paragraph("Indirizzo").SetFontSize(9).SetBold();
                        cellaGrigliaDest = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGrigliaDest.Add(pGrigliaDest);
                        tbGriglaDest.AddCell(cellaGrigliaDest);

                        pGrigliaDest = new Paragraph(documentoTrasporto.TipoIndirizzo + " " + documentoTrasporto.Indirizzo + " " + documentoTrasporto.NumeroCivico + Environment.NewLine + documentoTrasporto.Cap + " " + documentoTrasporto.Comune + " " + documentoTrasporto.Provincia).SetFontSize(9);
                        cellaGrigliaDest = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGrigliaDest.Add(pGrigliaDest);
                        tbGriglaDest.AddCell(cellaGrigliaDest);

                        // PARTITA IVA DESTINATARIO
                        pGrigliaDest = new Paragraph("P.Iva/C.F.").SetFontSize(9).SetBold();
                        cellaGrigliaDest = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGrigliaDest.Add(pGrigliaDest);
                        tbGriglaDest.AddCell(cellaGrigliaDest);

                        string pIvaCF = documentoTrasporto.PartitaIva;
                        
                        pGrigliaDest = new Paragraph(pIvaCF).SetFontSize(9);
                        cellaGrigliaDest = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGrigliaDest.Add(pGrigliaDest);
                        tbGriglaDest.AddCell(cellaGrigliaDest);

                        document.Add(tbGriglaDest);



                        //AGGIUNGO NUM.PAGINA
                        document.ShowTextAligned(new Paragraph("pagina " + i.ToString() + " di " + n.ToString()).SetFontSize(7),
                            pageSize.GetWidth() - 60, pageSize.GetHeight() - 20, i, iText.Layout.Properties.TextAlignment.CENTER, iText.Layout.Properties.VerticalAlignment.TOP, 0);
                        //AGGIUNGO FOOTER
                        document.ShowTextAligned(new Paragraph(denominazioneVs + " P.IVA " + pIvaVs + Environment.NewLine + "Sede legale: " + toponimoVs + " " + indirizzoVs + " " + civicoVs + " - " + capVs + " " + cittaVs + " " + provinciaVs + " e-mail: " + emailVs).SetFontSize(7).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER),
                            pageSize.GetWidth() / 2, 30, i, iText.Layout.Properties.TextAlignment.CENTER, iText.Layout.Properties.VerticalAlignment.TOP, 0);

                        if (i == n)
                        {
                            // NELL'ULTIMA PAGINA AGGIUNGO LA GRIGLIA CON LE NOTE E IL TIMBRO
                            // CREAZIONE GRIGLIA
                            iText.Layout.Element.Table tbGriglaNoteFooter = new iText.Layout.Element.Table(new float[] { 33, 34, 33 }).SetWidth(530).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetFixedPosition(30, 80, 530).SetFixedLayout();

                            // PRIMA RIGA GRIGLIA NOTE FOOTER
                            Paragraph pGrigliaNoteFooter = new Paragraph("DATA E FIRMA MITTENTE").SetFontSize(9);
                            iText.Layout.Element.Cell cellaGrigliaNoteFooter = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                            cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                            tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                            pGrigliaNoteFooter = new Paragraph("DATA E FIRMA CORRIERE").SetFontSize(9);
                            //cellaGrigliaNoteFooter = new iText.Layout.Element.Cell(1,2).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(2);
                            cellaGrigliaNoteFooter = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorderRight(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 2, 50)).SetBorderTop(iText.Layout.Borders.Border.NO_BORDER).SetBorderLeft(iText.Layout.Borders.Border.NO_BORDER).SetBorderBottom(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                            cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                            tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                            pGrigliaNoteFooter = new Paragraph("DATA E FIRMA DESTINATARIO").SetFontSize(9);
                            cellaGrigliaNoteFooter = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                            cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                            tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                            //// SECONDA RIGA GRIGLIA NOTE FOOTER
                            //pGrigliaNoteFooter = new Paragraph("").SetFontSize(9);
                            //cellaGrigliaNoteFooter = new iText.Layout.Element.Cell().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                            //cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                            //tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                            //pGrigliaNoteFooter = new Paragraph("").SetFontSize(9);
                            ////cellaGrigliaNoteFooter = new iText.Layout.Element.Cell(1,2).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(2);
                            //cellaGrigliaNoteFooter = new iText.Layout.Element.Cell().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 0.7f).SetBorderRight(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 2, 50)).SetBorderTop(iText.Layout.Borders.Border.NO_BORDER).SetBorderLeft(iText.Layout.Borders.Border.NO_BORDER).SetBorderBottom(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                            //cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                            //tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                            //pGrigliaNoteFooter = new Paragraph("").SetFontSize(9);
                            //cellaGrigliaNoteFooter = new iText.Layout.Element.Cell().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                            //cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                            //tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);


                            document.Add(tbGriglaNoteFooter);
                        }


                    }

                    // CHIUDO IL PDF
                    document.Flush();
                    document.Close();
                    wr.Close();

                    // SE FILE OK INSERISCO O AGGIORNO PROTOCOLLO DI FATTURA
                    if (listaProtocolli.Count == 0)
                    {

                        //INSERISCO
                        protocolloTrasporto.Attivo = true;
                        protocolloTrasporto.Cliente = documentoTrasporto.Destinatario;
                        protocolloTrasporto.Codice_lavoro = "generico";
                        protocolloTrasporto.Data_inizio_lavorazione = documentoTrasporto.DataTrasporto;
                        //protocolloFattura.Data_protocollo = DateTime.Today;
                        protocolloTrasporto.Data_protocollo = documentoTrasporto.DataTrasporto;
                        protocolloTrasporto.Descrizione = descrizioneProtocolloDDT; // "Documento di trasporto";
                        //protocolloTrasporto.Id_cliente = eventoSelezionato.id_cliente;
                        protocolloTrasporto.Id_tipo_protocollo = idTipoProtocollo;
                        protocolloTrasporto.Lavorazione = "generico";
                        protocolloTrasporto.PathDocumento = Path.GetFileName(mapPathReport);
                        protocolloTrasporto.Produzione = "";
                        protocolloTrasporto.Protocollo_riferimento = documentoTrasporto.NumeroDocumentoTrasporto;

                        if (!string.IsNullOrEmpty(documentoTrasporto.Numero_protocollo)) { 
                            protocolloTrasporto.Numero_protocollo = documentoTrasporto.Numero_protocollo;
                        }
                        else
                        {
                            // SE NON TROVO IL NUMERO PROTOCOLLO NEL DOCUMENTO DI TRASPORTO (NON DOVREBBE SUCCEDERE!!!) LO CREO E AGGIORNO ANCHE IL DOCUMENTO TRASPORTO
                            protocolloTrasporto.Numero_protocollo = Protocolli_BLL.Instance.getNumeroProtocollo();
                            documentoTrasporto.Numero_protocollo = protocolloTrasporto.Numero_protocollo;
                            esito = DocumentiTrasporto_BLL.Instance.AggiornaDocumentoTrasporto(documentoTrasporto);
                        }
                        protocolloTrasporto.Pregresso = false;
                        protocolloTrasporto.Destinatario = "Fornitore";

                        int idProtTrasporto = Protocolli_BLL.Instance.CreaProtocollo(protocolloTrasporto, ref esito);

                        ViewState["idProtocollo"] = idProtTrasporto;

                    }
                    else
                    {
                        // AGGIORNO
                        protocolloTrasporto.PathDocumento = Path.GetFileName(mapPathReport);
                        protocolloTrasporto.Data_protocollo = Convert.ToDateTime(documentoTrasporto.DataTrasporto);
                        esito = Protocolli_BLL.Instance.AggiornaProtocollo(protocolloTrasporto);

                        ViewState["idProtocollo"] = protocolloTrasporto.Id;

                    }

                    btnRicercaDocumentoTrasporto_Click(null, null);

                    // CREO STRINGA PER IL JAVASCRIPT DI VISUALIZZAZIONE
                    if (System.IO.File.Exists(mapPathReport))
                    {
                        Page page = HttpContext.Current.Handler as Page;
                        ScriptManager.RegisterStartupScript(page, page.GetType(), "apriPdf", script: "window.open('" + pathReport.Replace("~", "") + "')", addScriptTags: true);
                    }
                    else
                    {
                        esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                        esito.Descrizione = "Il File " + pathReport.Replace("~", "") + " non è stato creato correttamente!";
                        BasePage p = new BasePage();
                        p.ShowError(esito.Descrizione);
                    }
                }

            }
            catch (Exception ex)
            {
                BasePage p = new BasePage();
                p.ShowError(ex.Message + " " + ex.StackTrace);
            }

        }

        protected void btnViewAttachement_Click(object sender, ImageClickEventArgs e)
        {
            int idDocumentoTrasporto = Convert.ToInt32(ViewState["idDocumentoTrasporto"]);
            stampaDocumentoTrasporto(idDocumentoTrasporto);
        }

        protected void btnOpenDoc_Click(object sender, ImageClickEventArgs e)
        {
            int rowIndex = ((sender as ImageButton).NamingContainer as GridViewRow).RowIndex;
            int idDocumentoTrasporto = Convert.ToInt32(gv_documenti_trasporto.DataKeys[rowIndex].Values[0]);
            stampaDocumentoTrasporto(idDocumentoTrasporto);

        }

        protected void ddlTipoCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            string idCategoria = ddlTipoCategoria.SelectedItem.Value;
            if (string.IsNullOrEmpty(idCategoria)) idCategoria = "0";
            riempiComboSubCategoria(Convert.ToInt32(idCategoria));
        }

        protected void ddlTipoSubCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            string idSubCategoria = ddlTipoSubCategoria.SelectedItem.Value;
            if (string.IsNullOrEmpty(idSubCategoria)) idSubCategoria = "0";
            riempiComboGruppo(Convert.ToInt32(idSubCategoria));
        }

        protected void cbMultiSelect_CheckedChanged(object sender, EventArgs e)
        {
            Hashtable htAttrezzatureSelezionate = new Hashtable();
            if (Session[SessionManager.LISTA_ATTREZZATURE_SELEZIONATE_DOC_TRASPORTO] != null) { 
                htAttrezzatureSelezionate = (Hashtable)Session[SessionManager.LISTA_ATTREZZATURE_SELEZIONATE_DOC_TRASPORTO];
            }
            GridViewRow row = (GridViewRow)(((CheckBox)sender).NamingContainer);
            string idSelezionato = row.Cells[2].Text;

            CheckBox chkSelect = (CheckBox)sender;
            if (chkSelect.Checked)
            {
                try
                {
                    if (!htAttrezzatureSelezionate.ContainsKey(idSelezionato))
                    {
                        Esito esito = new Esito();
                        AttrezzatureMagazzino attrezzatura = AttrezzatureMagazzino_BLL.Instance.getAttrezzaturaById(ref esito,Convert.ToInt32(idSelezionato));
                        if (esito.Codice == 0)
                        {
                            htAttrezzatureSelezionate.Add(idSelezionato, attrezzatura.Descrizione + " " + attrezzatura.Cod_vs + "|" + attrezzatura.Modello);
                            Session[SessionManager.LISTA_ATTREZZATURE_SELEZIONATE_DOC_TRASPORTO] = htAttrezzatureSelezionate;
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
                    htAttrezzatureSelezionate.Remove(idSelezionato);
                    Session[SessionManager.LISTA_ATTREZZATURE_SELEZIONATE_DOC_TRASPORTO] = htAttrezzatureSelezionate;
                }
                catch (Exception ex)
                {

                    basePage.ShowError(ex.Message);
                }
            }

        }

        protected void btnInseMagazzinoSelezionati_Click(object sender, EventArgs e)
        {
            Hashtable htAttrezzatureSelezionate = new Hashtable();
            if (Session[SessionManager.LISTA_ATTREZZATURE_SELEZIONATE_DOC_TRASPORTO] != null)
            {
                Esito esito = new Esito();
                htAttrezzatureSelezionate = (Hashtable)Session[SessionManager.LISTA_ATTREZZATURE_SELEZIONATE_DOC_TRASPORTO];
                foreach (DictionaryEntry s in htAttrezzatureSelezionate)
                {
                    System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem(s.Value.ToString(), s.Key.ToString());
                    try
                    {
                        int id = Convert.ToInt32(li.Value);
                        
                        AttrezzatureMagazzino attrezzaturaMagazzino = AttrezzatureMagazzino_BLL.Instance.getAttrezzaturaById(ref esito, id);
                        if (esito.Codice == 0)
                        {

                            string codvs = "";
                            if (!string.IsNullOrEmpty(attrezzaturaMagazzino.Cod_vs)) codvs = attrezzaturaMagazzino.Cod_vs;
                            string seriale = "";
                            if (!string.IsNullOrEmpty(attrezzaturaMagazzino.Seriale)) seriale = attrezzaturaMagazzino.Seriale;
                            string descrizione = "";
                            if (!string.IsNullOrEmpty(attrezzaturaMagazzino.Descrizione)) descrizione = attrezzaturaMagazzino.Descrizione;
                            string modello = "";
                            if (!string.IsNullOrEmpty(attrezzaturaMagazzino.Modello)) modello = attrezzaturaMagazzino.Modello;

                            AttrezzatureTrasporto attrezzaturaTrasporto = new AttrezzatureTrasporto();
                            attrezzaturaTrasporto.Cod_vs = codvs;
                            attrezzaturaTrasporto.Seriale = seriale;
                            attrezzaturaTrasporto.Descrizione = descrizione + " " + modello;
                            attrezzaturaTrasporto.IdMagAttrezzature = attrezzaturaMagazzino.Id;
                            //attrezzaturaTrasporto.IdDocumentoTrasporto = Convert.ToInt64(hf_idDocTras.Value);
                            attrezzaturaTrasporto.IdDocumentoTrasporto = Convert.ToInt64(ViewState["idDocumentoTrasporto"]);
                            attrezzaturaTrasporto.Quantita = Convert.ToInt32(tbIns_Quantita.Text.Trim());
                            int idAttrezzaturaTrasportoNew = DocumentiTrasporto_BLL.Instance.CreaAttrezzaturaTrasporto(attrezzaturaTrasporto, ref esito);
                            if (esito.Codice != 0)
                            {
                                basePage.ShowError(esito.Descrizione);
                            }
                        }
                        else
                        {
                            basePage.ShowError(esito.Descrizione);
                        }
                    }
                    catch (Exception ex)
                    {

                        basePage.ShowError(ex.Message);
                    }

                }
                if (esito.Codice == 0) { 

                    List<AttrezzatureTrasporto> listaAttrezzature = DocumentiTrasporto_BLL.Instance.getAttrezzatureTrasportoByIdDocumentoTrasporto(ref esito, Convert.ToInt64(ViewState["idDocumentoTrasporto"]));
                    gv_attrezzature.DataSource = listaAttrezzature;
                    gv_attrezzature.DataBind();
                    PanelMagazzino.Visible = false;
                }
            }
        }
    }
}