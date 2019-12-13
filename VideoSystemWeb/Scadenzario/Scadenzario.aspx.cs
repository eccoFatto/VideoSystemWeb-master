using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoSystemWeb.BLL;
using VideoSystemWeb.Entity;

namespace VideoSystemWeb.Scadenzario.userControl
{
    public partial class Scadenzario : BasePage
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //public void ShowPopMessage(string messaggio)
        //{
        //    messaggio = messaggio.Replace("'", "\\'");
        //    messaggio = messaggio.Replace("\r\n", "<br/>");

        //    Page page = HttpContext.Current.Handler as Page;
        //    ScriptManager.RegisterStartupScript(page, page.GetType(), "apripopupScadenzario", script: "popupScadenzario('" + messaggio + "')", addScriptTags: true);
        //}

        protected void Page_PreInit(object sender, EventArgs e)
        {
            CheckIsMobile();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Esito esito = new Esito();
            txt_Iva.Text = Config_BLL.Instance.getConfig(ref esito, "IVA").Valore;

            if (!IsPostBack)
            {
                CaricaCombo();
                PopolaGrigliaScadenze();

                #region GESTIONE PARAMETRI URL
                string tipo = string.Empty;
                int idDatiProtocollo = 0;
                if (!string.IsNullOrEmpty(Request.QueryString["TIPO"])) tipo = Request.QueryString["TIPO"];
                if (!string.IsNullOrEmpty(Request.QueryString["ID_PROTOCOLLO"])) idDatiProtocollo = int.Parse(Request.QueryString["ID_PROTOCOLLO"]);

                if (!string.IsNullOrEmpty(tipo) && idDatiProtocollo != 0)
                {
                    ViewState["ID_PROTOCOLLO"] = idDatiProtocollo;

                    Protocolli protocollo = Protocolli_BLL.Instance.getProtocolloById(ref esito, idDatiProtocollo);
                    ddl_Tipo.SelectedValue = tipo;
                    txt_ClienteFornitore.Text = protocollo.Cliente;
                    txt_DataDocumento.Text = protocollo.Data_protocollo==null? DateTime.Now.ToString("dd/MM/yyyy") : ((DateTime)protocollo.Data_protocollo).ToString("dd/MM/yyyy");
                    txt_NumeroDocumento.Text = protocollo.Protocollo_riferimento;

                    pnlContainer.Visible = true;
                }
                #endregion
            }

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "chiudiLoader", script: "$('.loader').hide();", addScriptTags: true);
        }

        private void CaricaCombo()
        {
            Esito esito = new Esito();
            List<DatiBancari> listaDatiBancari = Config_BLL.Instance.getListaDatiBancari(ref esito);
            
            foreach (DatiBancari banca in listaDatiBancari)
            {
                ddl_Banca.Items.Add(new ListItem(banca.Banca, banca.Banca));
                ddl_BancaModifica.Items.Add(new ListItem(banca.Banca, banca.Banca));
            }
            ddl_Banca.Items.Add(new ListItem("Cassa", "Cassa"));
            ddl_BancaModifica.Items.Add(new ListItem("Cassa", "Cassa"));
        }

        private void PopolaGrigliaScadenze()
        {
            Esito esito = new Esito();

            gv_scadenze.DataSource = Scadenzario_BLL.Instance.GetAllDatiScadenzario("", "", "", "", "","","","",ref esito); 
            gv_scadenze.DataBind();
        }

        protected void btnEditScadenza_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hf_idScadenza.Value) || (!string.IsNullOrEmpty((string)ViewState["idScadenza"])))
            {
                if (!string.IsNullOrEmpty(hf_idScadenza.Value)) ViewState["idScadenza"] = hf_idScadenza.Value;
                gestisciPulsantiScadenza("MODIFICA");
              
                pnlContainer.Visible = true;
            }
        }

        protected void btnInsScadenza_Click(object sender, EventArgs e)
        {
            ViewState["idScadenza"] = "";
            PulisciCampiDettaglio();
            NascondiErroriValidazione();
            gestisciPulsantiScadenza("INSERIMENTO");

            pnlContainer.Visible = true;
        }

        //protected void btnCercaScadenza_Click(object sender, EventArgs e)
        //{
        //}

        protected void btnRicercaScadenza_Click(object sender, EventArgs e)
        {
            Esito esito = new Esito();

            gv_scadenze.DataSource = Scadenzario_BLL.Instance.GetAllDatiScadenzario(ddl_TipoAnagrafica.SelectedValue,
                                                                                    ddl_CodiceAnagrafica.SelectedValue,
                                                                                    txt_NumeroFattura.Text,
                                                                                    ddlFatturaPagata.SelectedValue,
                                                                                    txt_DataFatturaDa.Text,
                                                                                    txt_DataFatturaA.Text,
                                                                                    txt_DataScadenzaDa.Text,
                                                                                    txt_DataScadenzaA.Text, ref esito);
            gv_scadenze.DataBind();
        }

        protected void gv_scadenze_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Esito esito = new Esito();

            int idScadenzaSelezionata = int.Parse(e.CommandArgument.ToString());
            List<DatiScadenzario> listaDatiFattura = Scadenzario_BLL.Instance.GetDatiTotaliFatturaByIdDatiScadenzario(idScadenzaSelezionata, ref esito);

            switch (e.CommandName)
            {
                case "modifica":
                    NascondiErroriValidazione();
                    
                    DatiScadenzario scadenza = Scadenzario_BLL.Instance.GetDatiScadenzarioById(ref esito, idScadenzaSelezionata);
                    
                    decimal importoDocumentoDare = listaDatiFattura.Sum(x => x.ImportoDare);
                    decimal importoDocumentoDareIva = listaDatiFattura.Sum(x => x.ImportoDareIva);
                    decimal importoDocumentoAvere = listaDatiFattura.Sum(x => x.ImportoAvere);
                    decimal importoDocumentoAvereIva = listaDatiFattura.Sum(x => x.ImportoAvereIva);

                    if (importoDocumentoDare > 0) // FORNITORE
                    {
                        lbl_VersatoRiscosso.Text = "Versato";
                        lbl_VersatoRiscossoIVA.Text = "Versato + IVA";
                        ddl_Tipo.SelectedValue = "Fornitore";
                    }
                    else // CLIENTE
                    {
                        lbl_VersatoRiscosso.Text = "Riscosso";
                        lbl_VersatoRiscossoIVA.Text = "Riscosso + IVA";
                        ddl_Tipo.SelectedValue = "Cliente";
                    }

                    txt_ClienteFornitore.Text = scadenza.RagioneSocialeClienteFornitore;
                    txt_DataDocumento.Text = scadenza.DataProtocollo.ToString();
                    txt_NumeroDocumento.Text = scadenza.ProtocolloRiferimento;
                    txt_Imponibile.Text = (importoDocumentoDare + importoDocumentoAvere).ToString("###,##0.00");
                    txt_ImponibileIva.Text = (importoDocumentoDareIva + importoDocumentoAvereIva).ToString("###,##0.00");
                    decimal versatoOriscosso = scadenza.ImportoVersato + scadenza.ImportoRiscosso;
                    txt_Versato.Text = versatoOriscosso.ToString("###,##0.00");
                    txt_IvaModifica.Text = scadenza.Iva.ToString();// Config_BLL.Instance.getConfig(ref esito, "IVA").Valore;
                    txt_VersatoIva.Text = (versatoOriscosso * (1 + scadenza.Iva / 100)).ToString("###,##0.00");
                    txt_Totale.Text = ((importoDocumentoDare + importoDocumentoAvere) - (scadenza.ImportoVersato + scadenza.ImportoRiscosso)).ToString("###,##0.00");
                    txt_IvaModifica.Text = Config_BLL.Instance.getConfig(ref esito, "IVA").Valore;
                    txt_TotaleIva.Text = ((importoDocumentoDareIva + importoDocumentoAvereIva) - (versatoOriscosso * (1 + scadenza.Iva / 100))).ToString("###,##0.00");
                    txt_TotaleDocumento.Text = (importoDocumentoDare + importoDocumentoAvere).ToString("###,##0.00");
                    txt_TotDocumentoIva.Text = (importoDocumentoDareIva + importoDocumentoAvereIva).ToString("###,##0.00");
                    txt_DataDocumentoModifica.Text = scadenza.DataProtocollo != null ? ((DateTime)scadenza.DataProtocollo).ToString("dd/MM/yyyy") : "";
                    txt_ScadenzaDocumento.Text = scadenza.DataScadenza != null ? ((DateTime)scadenza.DataScadenza).ToString("dd/MM/yyyy") : "";
                    ddl_BancaModifica.SelectedValue = scadenza.Banca;

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriModificaScadenza", script: "javascript: mostraScadenza('" + idScadenzaSelezionata + "');", addScriptTags: true);
                    
                    break;
                case "elimina":
                    Scadenzario_BLL.Instance.DeleteDatiScadenzario(idScadenzaSelezionata, ref esito);

                    break;
                case "visualizzaDoc":
                    int idDatiProtocollo = listaDatiFattura.ElementAt(0).IdDatiProtocollo;
                    ApriDocumento(idDatiProtocollo);

                    break;
            }
        }

        
        protected void btnApriDocumento_Click(object sender, EventArgs e)
        {
            Esito esito = new Esito();
            int idScadenza = Convert.ToInt16(ViewState["idScadenza"].ToString());
            int idDatiProtocollo = Scadenzario_BLL.Instance.GetDatiTotaliFatturaByIdDatiScadenzario(idScadenza, ref esito).ElementAt(0).IdDatiProtocollo;
            ApriDocumento(idDatiProtocollo);
        }

        private void ApriDocumento(int idDatiProtocollo)
        {
            Esito esito = new Esito();
            Protocolli protocolloSelezionato = Protocolli_BLL.Instance.getProtocolloById(ref esito, idDatiProtocollo);

            if (string.IsNullOrEmpty(protocolloSelezionato.PathDocumento))
            {
                ShowWarning("Documento non presente");
            }
            else
            {
                string pathRelativo = "";
                if (protocolloSelezionato.Pregresso)
                {
                    pathRelativo = ConfigurationManager.AppSettings["PATH_DOCUMENTI_PREGRESSO"].Replace("~", "");
                }
                else
                {
                    pathRelativo = ConfigurationManager.AppSettings["PATH_DOCUMENTI_PROTOCOLLO"].Replace("~", "");
                }

                string pathCompleto = pathRelativo + protocolloSelezionato.PathDocumento;
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriDocumento", script: "javascript: window.open('" + pathCompleto + "');", addScriptTags: true);
            }
        }

        protected void gv_scadenze_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_scadenze.PageIndex = e.NewPageIndex;
            btnRicercaScadenza_Click(null, null);
        }

        protected void btnModificaScadenza_Click(object sender, EventArgs e)
        {
            Esito esito = new Esito();

            int idScadenza = Convert.ToInt16(ViewState["idScadenza"].ToString());
            DatiScadenzario scadenza = Scadenzario_BLL.Instance.GetDatiScadenzarioById(ref esito, Convert.ToInt16(idScadenza));

            string importoVersato = ValidaCampo(txt_Versato, "", true, ref esito);
            string iva = ValidaCampo(txt_IvaModifica, "", true, ref esito);
            string dataScadenza = ValidaCampo(txt_ScadenzaDocumento, "", true, ref esito);

            if (esito.Codice != Esito.ESITO_OK)
            {
                ShowError("Controllare i campi evidenziati");
            }
            else
            {
                Scadenzario_BLL.Instance.AggiornaDatiScadenzario(scadenza, ddl_Tipo.SelectedValue, importoVersato, iva, dataScadenza, ddl_BancaModifica.SelectedValue, ref esito);

                if (esito.Codice != Esito.ESITO_OK)
                {
                    log.Error(esito.Descrizione);
                    ShowError(esito.Descrizione);
                }
                else
                {
                    ShowSuccess("Scadenza aggiornata correttamente");
                    btnChiudiPopup_Click(null, null);
                    PopolaGrigliaScadenze();
                }
            }
        }


        private void gestisciPulsantiScadenza(string stato)
        {
            switch (stato)
            {
                case "INSERIMENTO":
                    btnInserisciScadenza.Visible = true;
                    btnModificaScadenza.Visible = false;

                    div_CampiModifica.Visible = false;
                    div_CampiInserimento.Visible = true;

                    txt_ImportoDocumento.Attributes.Remove("ReadOnly");
                    txt_Iva.Attributes.Remove("disabled");
                    txt_NumeroRate.Attributes.Remove("disabled");
                    txt_AnticipoImporto.Attributes.Remove("disabled");
                    txt_CadenzaGiorni.Attributes.Remove("disabled");
                    ddl_APartireDa.Attributes.Remove("disabled");

                    break;
                case "MODIFICA":
                    btnInserisciScadenza.Visible = false;
                    btnModificaScadenza.Visible = true;
                   
                    div_CampiModifica.Visible = true;
                    div_CampiInserimento.Visible = false;

                    txt_ImportoDocumento.Attributes.Add("ReadOnly", "ReadOnly");
                    txt_Iva.Attributes.Add("disabled", "disabled");
                    txt_NumeroRate.Attributes.Add("disabled", "disabled");
                    txt_AnticipoImporto.Attributes.Add("disabled", "disabled");
                    txt_CadenzaGiorni.Attributes.Add("disabled", "disabled");
                    ddl_APartireDa.Attributes.Add("disabled", "disabled");

                    break;
                default:
                    btnInserisciScadenza.Visible = false;
                    btnModificaScadenza.Visible = false;
                    
                    break;
            }
        }

        protected void btnInserisciScadenza_Click(object sender, EventArgs e)
        {
            Esito esito = new Esito();
            DatiScadenzario datiScadenzario = CreaOggettoDatiScadenzario(ref esito);

            if (esito.Codice != Esito.ESITO_OK)
            {
                ShowError("Controllare i campi evidenziati");
            }
            else
            {
                NascondiErroriValidazione();

                if (string.IsNullOrEmpty(txt_NumeroRate.Text) || txt_NumeroRate.Text == "0") txt_NumeroRate.Text = "1";

                DateTime? dataPartenzaPagamento = datiScadenzario.DataProtocollo == null ? DateTime.Now : datiScadenzario.DataProtocollo;
                if (ddl_APartireDa.SelectedValue == "1") // INIZIO CALCOLO SCADENZA DA FINE MESE
                {
                    dataPartenzaPagamento = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1);
                    dataPartenzaPagamento = ((DateTime)dataPartenzaPagamento).AddDays(-1);
                }

                int cadenzaGiorni = 30;
                if (!string.IsNullOrEmpty(txt_CadenzaGiorni.Text)) cadenzaGiorni = int.Parse(txt_CadenzaGiorni.Text);

                Scadenzario_BLL.Instance.CreaDatiScadenzario(datiScadenzario, 
                                                             txt_AnticipoImporto.Text, 
                                                             txt_Iva.Text, 
                                                             txt_NumeroRate.Text, 
                                                             ddl_Tipo.SelectedValue,
                                                             dataPartenzaPagamento,
                                                             cadenzaGiorni, 
                                                             ref esito);

                if (esito.Codice != Esito.ESITO_OK)
                {
                    log.Error(esito.Descrizione);
                    ShowError(esito.Descrizione);
                }

                ShowSuccess("Scadenza inserita correttamente");
                btnChiudiPopup_Click(null, null);

                ViewState["ID_PROTOCOLLO"] = null;

                PopolaGrigliaScadenze();
            }
        }

        private DatiScadenzario CreaOggettoDatiScadenzario(ref Esito esito)
        {
            int _idDatiProtocollo = ViewState["ID_PROTOCOLLO"] != null ? int.Parse(ViewState["ID_PROTOCOLLO"].ToString()) : 0;// int.Parse(txt_ClienteFornitore.Text); //DA MODIFICARE

            Protocolli protocollo = Protocolli_BLL.Instance.getProtocolloById(ref esito, _idDatiProtocollo);
            DatiScadenzario scadenza = new DatiScadenzario();

            if (string.IsNullOrEmpty((string)ViewState["idScadenza"]))
            {
                ViewState["idScadenza"] = "0";
            }

            DateTime? dataFattura = protocollo.Data_protocollo;// DateTime.Now; // DA MODIFICARE

            scadenza.Id = Convert.ToInt16(ViewState["idScadenza"].ToString());
            scadenza.IdDatiProtocollo = _idDatiProtocollo;
            scadenza.Banca = ddl_Banca.SelectedValue;
            scadenza.DataScadenza = null; // il valore viene calcolato in seguito
            
            scadenza.ImportoDare = 0;
            scadenza.ImportoDareIva = 0;
            scadenza.ImportoVersato = 0;
            scadenza.DataVersamento = null;
            scadenza.ImportoAvere = 0;
            scadenza.ImportoAvereIva = 0;
            scadenza.ImportoRiscosso = 0;
            scadenza.DataRiscossione = null;
            scadenza.Iva = decimal.Parse(txt_Iva.Text);

            ValidaCampo(txt_CadenzaGiorni, 0, true, ref esito);

            if (ddl_Tipo.SelectedValue.ToUpper() == "CLIENTE")
            {
                scadenza.ImportoAvere = ValidaCampo(txt_ImportoDocumento, 0, true, ref esito); 
                scadenza.ImportoAvereIva = scadenza.ImportoAvere * (1 + (decimal.Parse(txt_Iva.Text) / 100));
                scadenza.ImportoRiscosso = 0;
            }
            else
            {
                scadenza.ImportoDare = ValidaCampo(txt_ImportoDocumento, 0, true, ref esito); 
                scadenza.ImportoDareIva = scadenza.ImportoDare * (1 + (decimal.Parse(txt_Iva.Text) / 100));
                scadenza.ImportoVersato = 0;
            }
            
            scadenza.Note = string.Empty;

            scadenza.RagioneSocialeClienteFornitore = string.Empty;
            scadenza.ProtocolloRiferimento = string.Empty;
            scadenza.DataProtocollo = dataFattura;
            scadenza.Cassa = 0;

            return scadenza;
        }

        private void NascondiErroriValidazione()
        {
            txt_ImportoDocumento.CssClass = txt_ImportoDocumento.CssClass.Replace("erroreValidazione", "");
            txt_CadenzaGiorni.CssClass = txt_CadenzaGiorni.CssClass.Replace("erroreValidazione", "");

            txt_Versato.CssClass = txt_Versato.CssClass.Replace("erroreValidazione", "");
            txt_IvaModifica.CssClass = txt_IvaModifica.CssClass.Replace("erroreValidazione", "");
            txt_ScadenzaDocumento.CssClass = txt_ScadenzaDocumento.CssClass.Replace("erroreValidazione", "");
        }

        //private void editScadenza()
        //{
        //    string idScadenza = (string)ViewState["idScadenza"];
        //    Esito esito = new Esito();

        //    if (!string.IsNullOrEmpty(idScadenza))
        //    {
        //        DatiScadenzario scadenza = Scadenzario_BLL.Instance.GetDatiScadenzarioById(ref esito, Convert.ToInt16(idScadenza));
        //        if (esito.Codice == 0)
        //        {
        //            PulisciCampiDettaglio();
        //        }
        //        else
        //        {
        //            Session["ErrorPageText"] = esito.Descrizione;
        //            string url = String.Format("~/pageError.aspx");
        //            Response.Redirect(url, true);
        //        }
        //    }

        //}

        private void PulisciCampiDettaglio()
        {
            ddl_Tipo.SelectedIndex =
            ddl_APartireDa.SelectedIndex =
            ddl_Banca.SelectedIndex = 0;

            txt_ClienteFornitore.Text =
            txt_DataDocumento.Text =
            txt_NumeroDocumento.Text =
            txt_ImportoDocumento.Text =
            txt_ImportoIva.Text =
            txt_NumeroRate.Text =
            txt_AnticipoImporto.Text =
            txt_CadenzaGiorni.Text = string.Empty;
        }

        protected void btnChiudiPopup_Click(object sender, EventArgs e)
        {
            pnlContainer.Visible = false;
        }
    }
}