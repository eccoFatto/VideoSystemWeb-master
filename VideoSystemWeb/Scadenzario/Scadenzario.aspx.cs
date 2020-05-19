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

        #region TIPI
        private const string TIPO_CLIENTE = "CLIENTE";
        private const string TIPO_FORNITORE = "FORNITORE";
        private enum TipoModifica { IMPORTO_ZERO, IMPORTO_TRA_0_E_MAX, IMPORTO_MAX, IMPORTO_UGUALE, NO_FIGLI }
        #endregion

        #region ELENCO CHIAVI VIEWSTATE
        private const string VIEWSTATE_ID_SCADENZA = "idScadenza";
        private const string VIEWSTATE_ID_PROTOCOLLO = "ID_PROTOCOLLO";
        private const string VIEWSTATE_TIPOMODIFICA = "TIPO_MODOFICA";
        #endregion

        protected void Page_PreInit(object sender, EventArgs e)
        {
            CheckIsMobile();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Esito esito = new Esito();

            string valoreIVA = Config_BLL.Instance.getConfig(ref esito, "IVA").Valore;

            if (!IsPostBack)
            {
                CaricaCombo();
                PopolaGrigliaScadenze();
                AbilitaBottoni(AbilitazioneInScrittura());

                #region GESTIONE PARAMETRI URL
                string tipo = string.Empty;
                int idDatiProtocollo = 0;

                string dataDocumento = string.Empty;
                string numeroDocumento = string.Empty;
                string importo = string.Empty;
                string iva = valoreIVA;
                string importoIva = string.Empty;
                string banca = string.Empty;

                if (!string.IsNullOrEmpty(Request.QueryString["TIPO"])) tipo = Request.QueryString["TIPO"];
                if (!string.IsNullOrEmpty(Request.QueryString["ID_PROTOCOLLO"])) idDatiProtocollo = int.Parse(Request.QueryString["ID_PROTOCOLLO"]);

                if (!string.IsNullOrEmpty(Request.QueryString["DATA_DOC"])) dataDocumento = Request.QueryString["DATA_DOC"];
                if (!string.IsNullOrEmpty(Request.QueryString["NUM_DOC"])) numeroDocumento = Request.QueryString["NUM_DOC"];
                if (!string.IsNullOrEmpty(Request.QueryString["IMPORTO"])) importo = Request.QueryString["IMPORTO"];
                if (!string.IsNullOrEmpty(Request.QueryString["IVA"])) iva = Request.QueryString["IVA"];
                if (!string.IsNullOrEmpty(Request.QueryString["IMPORTO_IVA"])) importoIva = Request.QueryString["IMPORTO_IVA"];
                if (!string.IsNullOrEmpty(Request.QueryString["BANCA"])) banca = Request.QueryString["BANCA"];

                if (!string.IsNullOrEmpty(tipo) && idDatiProtocollo != 0 && Scadenzario_BLL.Instance.GetDatiScadenzarioByIdDatiProtocollo(idDatiProtocollo, ref esito).Count == 0)
                {
                    div_Fattura.Visible = false;
                    div_DatiCreazioneScadenza.Visible = true;
                    ddl_Tipo.Attributes.Add("Disabled", "Disabled");

                    ViewState[VIEWSTATE_ID_PROTOCOLLO] = idDatiProtocollo;

                    Protocolli protocollo = Protocolli_BLL.Instance.getProtocolloById(ref esito, idDatiProtocollo);
                    ddl_Tipo.SelectedValue = tipo;
                    txt_ClienteFornitore.Text = protocollo.Cliente;
                    txt_DataDocumento.Text = protocollo.Data_protocollo == null ? DateTime.Now.ToString("dd/MM/yyyy") : ((DateTime)protocollo.Data_protocollo).ToString("dd/MM/yyyy"); //protocollo.Data_inizio_lavorazione == null ? DateTime.Now.ToString("dd/MM/yyyy") : ((DateTime)protocollo.Data_inizio_lavorazione).ToString("dd/MM/yyyy");
                    txt_NumeroDocumento.Text = numeroDocumento;

                    txt_ImportoDocumento.Text = importo;
                    txt_Iva.Text = iva;
                    txt_ImportoIva.Text = importoIva;
                    ddl_Banca.SelectedValue = banca;

                    pnlContainer.Visible = true;
                }
                #endregion
            }

            ScriptManager.RegisterStartupScript(this, typeof(Page), "coerenzaDate", "controlloCoerenzaDate('" + txt_DataFatturaDa.ClientID + "', '" + txt_DataFatturaA.ClientID + "');", true);
            ScriptManager.RegisterStartupScript(this, typeof(Page), "coerenzaDate2", "controlloCoerenzaDate('" + txt_DataDa.ClientID + "', '" + txt_DataA.ClientID + "');", true);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "chiudiLoader", script: "$('.loader').hide();", addScriptTags: true);
        }


        #region OPERAZIONI AL CARICAMENTO PAGINA
        private void CaricaCombo()
        {
            Esito esito = new Esito();

            #region BANCA
            ddl_FiltroBanca.Items.Add(new ListItem("<tutti>", ""));
            foreach (Tipologica tipologiaBanca in SessionManager.ListaTipiBanca)
            {
                ddl_Banca.Items.Add(new ListItem(tipologiaBanca.nome, tipologiaBanca.id.ToString()));

                ddl_BancaModifica.Items.Add(new ListItem(tipologiaBanca.nome, tipologiaBanca.id.ToString()));

                ddl_FiltroBanca.Items.Add(new ListItem(tipologiaBanca.nome, tipologiaBanca.id.ToString()));
            }

            #endregion
        }

        private void CalcolaTotali(List<DatiScadenzario> listaScadenzario)
        {
            decimal dare = listaScadenzario.Sum(x => x.ImportoDare);
            decimal dareIva = listaScadenzario.Sum(x => x.ImportoDareIva);
            decimal versato = listaScadenzario.Sum(x => x.ImportoVersato);
            decimal versatoIva = listaScadenzario.Sum(x => x.ImportoVersatoIva); 
            decimal totaleDare = dare - versato;
            decimal totaleDareIva = dareIva - versatoIva;

            decimal avere = listaScadenzario.Sum(x => x.ImportoAvere);
            decimal avereIva = listaScadenzario.Sum(x => x.ImportoAvereIva);
            decimal riscosso = listaScadenzario.Sum(x => x.ImportoRiscosso);
            decimal riscossoIva = listaScadenzario.Sum(x => x.ImportoRiscossoIva);
            decimal totaleAvere = avere - riscosso;
            decimal totaleAvereIva = avereIva - riscossoIva;

            lbl_dare.Text = string.Format("{0:C}", dare);
            lbl_dare_iva.Text = string.Format("{0:C}", dareIva);
            lbl_versato.Text = string.Format("{0:C}", versato);
            lbl_versato_iva.Text = string.Format("{0:C}", versatoIva);
            lbl_totale_dare.Text = string.Format("{0:C}", totaleDare);
            lbl_totale_dare_iva.Text = string.Format("{0:C}", totaleDareIva);

            lbl_avere.Text = string.Format("{0:C}", avere);
            lbl_avere_iva.Text = string.Format("{0:C}", avereIva);
            lbl_riscosso.Text = string.Format("{0:C}", riscosso);
            lbl_riscosso_iva.Text = string.Format("{0:C}", riscossoIva);
            lbl_totale_avere.Text = string.Format("{0:C}", totaleAvere);
            lbl_totale_avere_iva.Text = string.Format("{0:C}", totaleAvereIva);
        }

        private void PopolaGrigliaScadenze()
        {
            Esito esito = new Esito();
            List<DatiScadenzario> listaDatiScadenzario = Scadenzario_BLL.Instance.GetAllDatiScadenzario("", "", "", "0", "", "", "", "", "", ref esito);

            CalcolaTotali(listaDatiScadenzario);
            gv_scadenze.DataSource = listaDatiScadenzario;
            gv_scadenze.DataBind();
        }
        #endregion

        #region COMPORTAMENTO COMPONENTI PAGINA
        protected void btnEditScadenza_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hf_idScadenza.Value) || (!string.IsNullOrEmpty((string)ViewState[VIEWSTATE_ID_SCADENZA])))
            {
                if (!string.IsNullOrEmpty(hf_idScadenza.Value)) ViewState[VIEWSTATE_ID_SCADENZA] = hf_idScadenza.Value;
                gestisciPulsantiScadenza("MODIFICA");

                pnlContainer.Visible = true;
            }
        }

        protected void ddl_fattura_SelectedIndexChanged(object sender, EventArgs e)
        {
            Esito esito = new Esito();

            if (!string.IsNullOrEmpty(ddl_fattura.SelectedValue))
            {
                int idDatiProtocollo = int.Parse(ddl_fattura.SelectedValue);

                ViewState[VIEWSTATE_ID_PROTOCOLLO] = idDatiProtocollo;

                Protocolli protocollo = Protocolli_BLL.Instance.getProtocolloById(ref esito, idDatiProtocollo);
                txt_ClienteFornitore.Text = protocollo.Cliente;
                txt_DataDocumento.Text = protocollo.Data_protocollo == null ? DateTime.Now.ToString("dd/MM/yyyy") : ((DateTime)protocollo.Data_protocollo).ToString("dd/MM/yyyy");//protocollo.Data_inizio_lavorazione == null ? DateTime.Now.ToString("dd/MM/yyyy") : ((DateTime)protocollo.Data_inizio_lavorazione).ToString("dd/MM/yyyy");
                txt_NumeroDocumento.Text = protocollo.Protocollo_riferimento;
            }
            else
            {
                ViewState[VIEWSTATE_ID_PROTOCOLLO] = null;

                txt_ClienteFornitore.Text = "";
                txt_DataDocumento.Text = "";
                txt_NumeroDocumento.Text = "";
            }
        }

        protected void ddl_Tipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddl_fattura.Items.Clear();
            txt_ClienteFornitore.Text = "";
            txt_DataDocumento.Text = "";
            txt_NumeroDocumento.Text = "";

            Esito esito = new Esito();
            List<Protocolli> listaProtocolloNonInScadenzario = Scadenzario_BLL.Instance.getFattureNonInScadenzario(ddl_Tipo.SelectedValue, ref esito);

            string tipoAnagrafica = ddl_Tipo.SelectedItem.Text;

            if (listaProtocolloNonInScadenzario.Count == 0)
            {
                ddl_fattura.Items.Add(new ListItem("<nessuna fattura trovata>", ""));
            }
            else
            {
                ddl_fattura.Items.Add(new ListItem("<seleziona una fattura>", ""));
                foreach (Protocolli protocollo in listaProtocolloNonInScadenzario)
                {
                    ddl_fattura.Items.Add(new ListItem("Protocollo: " + protocollo.Numero_protocollo + " - " + tipoAnagrafica + ": " + protocollo.Cliente + " - Lavorazione: " + protocollo.Lavorazione, protocollo.Id.ToString()));
                }
            }
            ddl_fattura.SelectedIndex = 0;
        }

        protected void btnRicercaScadenza_Click(object sender, EventArgs e)
        {
            
            RicercaScadenze();
        }

        protected void gv_scadenze_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            ViewState[VIEWSTATE_TIPOMODIFICA] = null;
            Esito esito = new Esito();

            int idScadenzaSelezionata = int.Parse(e.CommandArgument.ToString());

            ViewState[VIEWSTATE_ID_SCADENZA] = idScadenzaSelezionata;

            List<DatiScadenzario> listaDatiFattura = Scadenzario_BLL.Instance.GetDatiTotaliFatturaByIdDatiScadenzario(idScadenzaSelezionata, ref esito);

            DatiScadenzario scadenzaSelezionata = PopolaPopupModifica(idScadenzaSelezionata, listaDatiFattura, ref esito); // popolo il popup fuori dallo switch perché mi i valori mi servono pure per l'eliminazione rata

            switch (e.CommandName)
            {
                case "modifica":
                    if (scadenzaSelezionata.IsImportoEstinto == "Pagata")
                    {
                        btnAcconto.CssClass += " w3-disabled";
                        btnSaldoTotale.CssClass += " w3-disabled";

                        btnAcconto.ToolTip = "La rata risulta saldata. Non è possibile registrare un acconto";
                        btnSaldoTotale.ToolTip = "La rata risulta saldata. Non è possibile effettuare il saldo";
                    }
                    else
                    {
                        btnAcconto.CssClass = btnAcconto.CssClass.Replace("w3-disabled", "");
                        btnSaldoTotale.CssClass += btnSaldoTotale.CssClass.Replace("w3-disabled", "");

                        btnAcconto.ToolTip = string.Empty;
                        btnSaldoTotale.ToolTip = string.Empty;
                    }

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriModificaScadenza", script: "javascript: mostraScadenza('" + idScadenzaSelezionata + "');", addScriptTags: true);

                    break;
                case "elimina":
                    List<DatiScadenzario> listaFigli = CaricaListaFigli(scadenzaSelezionata);
                    decimal maxImportoIvaDaVersare = listaFigli.Sum(x => x.ImportoAvereIva + x.ImportoDareIva);
                    decimal importoScadenzaFigli = maxImportoIvaDaVersare;

                    ViewState[VIEWSTATE_TIPOMODIFICA] = TipoModifica.IMPORTO_ZERO;

                    lblMessaggioPopup.Text = "La scadenza selezionata verrà eliminata.<br/>Tutte le rate precedentemente immesse e legate alla scadenza selezionata verranno sostituite da una rata unica di importo pari a <b>" + importoScadenzaFigli + " €</b>, con scadenza <b>" + ((DateTime)scadenzaSelezionata.DataScadenza).ToString("dd/MM/yyyy") + "</b>"; 
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriPanelModificaScadenzaConFigli", script: "javascript: document.getElementById('" + panelModificaScadenzaConFigli.ClientID + "').style.display='block'", addScriptTags: true);

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
            int idDatiProtocollo = 0;

            if (ViewState[VIEWSTATE_ID_SCADENZA] != null)
            {
                int idScadenza = Convert.ToInt32(ViewState[VIEWSTATE_ID_SCADENZA].ToString());
                idDatiProtocollo = Scadenzario_BLL.Instance.GetDatiTotaliFatturaByIdDatiScadenzario(idScadenza, ref esito).ElementAt(0).IdDatiProtocollo;
            }
            else if (ViewState[VIEWSTATE_ID_PROTOCOLLO] != null)
            {
                idDatiProtocollo = Convert.ToInt32(ViewState[VIEWSTATE_ID_PROTOCOLLO].ToString());
            }

            ApriDocumento(idDatiProtocollo);
        }

        protected void gv_scadenze_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_scadenze.PageIndex = e.NewPageIndex;
            btnRicercaScadenza_Click(null, null);
        }

        protected void btnChiudiPopup_Click(object sender, EventArgs e)
        {
            pnlContainer.Visible = false;
        }
        #endregion

        #region RICERCHE E CARICAMENTI
        private void RicercaScadenze()
        {
            Esito esito = new Esito();

            List<DatiScadenzario> listaDatiScadenzario = Scadenzario_BLL.Instance.GetAllDatiScadenzario(ddl_TipoAnagrafica.SelectedValue,
                                                                                                        //hf_RagioneSociale.Value,
                                                                                                        txt_RagioneSociale.Text,
                                                                                                        txt_NumeroFattura.Text,
                                                                                                        ddlFatturaPagata.SelectedValue,
                                                                                                        txt_DataFatturaDa.Text,
                                                                                                        txt_DataFatturaA.Text,
                                                                                                        txt_DataDa.Text,
                                                                                                        txt_DataA.Text,
                                                                                                        ddl_FiltroBanca.SelectedValue,
                                                                                                        ref esito);
            CalcolaTotali(listaDatiScadenzario);
            gv_scadenze.DataSource = listaDatiScadenzario;
            gv_scadenze.DataBind();
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

        private DatiScadenzario PopolaPopupModifica(int idScadenzaSelezionata, List<DatiScadenzario> listaDatiFattura, ref Esito esito)
        {
            lbl_IntestazionePopup.Text = "Modifica scadenza";
            NascondiErroriValidazione();
            div_Fattura.Visible = false;
            div_DatiCreazioneScadenza.Visible = false;
            ddl_Tipo.Attributes.Add("Disabled", "Disabled");

            DatiScadenzario scadenza = Scadenzario_BLL.Instance.GetDatiScadenzarioById(ref esito, idScadenzaSelezionata);

            decimal importoDocumentoDare = listaDatiFattura.Sum(x => x.ImportoDare);
            decimal importoDocumentoDareIva = listaDatiFattura.Sum(x => x.ImportoDareIva);
            decimal importoDocumentoAvere = listaDatiFattura.Sum(x => x.ImportoAvere);
            decimal importoDocumentoAvereIva = listaDatiFattura.Sum(x => x.ImportoAvereIva);

            if (importoDocumentoDare > 0) // FORNITORE
            {
                lbl_VersatoRiscosso.Text = "Versato netto";
                lbl_VersatoRiscossoIVA.Text = lbl_VersatoRiscossoAccontoIVA.Text = "Versato + IVA";
                ddl_Tipo.SelectedValue = "Fornitore";

                lbl_DataVersamentoRiscossione.Text = "Data versamento";
                txt_DataVersamentoRiscossione.Text = scadenza.DataVersamento == null ? string.Empty : ((DateTime)scadenza.DataVersamento).ToString("dd/MM/yyyy");
            }
            else // CLIENTE
            {
                lbl_VersatoRiscosso.Text = "Riscosso netto";
                lbl_VersatoRiscossoIVA.Text = lbl_VersatoRiscossoAccontoIVA.Text = "Riscosso + IVA";
                ddl_Tipo.SelectedValue = "Cliente";

                lbl_DataVersamentoRiscossione.Text = "Data riscossione";
                txt_DataVersamentoRiscossione.Text = scadenza.DataRiscossione == null ? string.Empty : ((DateTime)scadenza.DataRiscossione).ToString("dd/MM/yyyy");
            }

            txt_ClienteFornitore.Text = scadenza.RagioneSocialeClienteFornitore;
            txt_DataDocumento.Text = scadenza.DataProtocollo.ToString();
            txt_NumeroDocumento.Text = scadenza.ProtocolloRiferimento;
            txt_Imponibile.Text = (scadenza.ImportoDare + scadenza.ImportoAvere).ToString("###,##0.00");
            txt_ImponibileIva.Text = (scadenza.ImportoDareIva + scadenza.ImportoAvereIva).ToString("###,##0.00");

            decimal versatoOriscosso = scadenza.ImportoVersato + scadenza.ImportoRiscosso;
            txt_Versato.Text = versatoOriscosso.ToString("###,##0.00");

            decimal versatoOriscossoIva = scadenza.ImportoVersatoIva + scadenza.ImportoRiscossoIva;
            txt_VersatoIva.Text = versatoOriscossoIva.ToString("###,##0.00");

            txt_IvaModifica.Text = scadenza.Iva.ToString();
            txt_Totale.Text = ((scadenza.ImportoDare + scadenza.ImportoAvere) - (scadenza.ImportoVersato + scadenza.ImportoRiscosso)).ToString("###,##0.00");
            txt_IvaModifica.Text = Math.Truncate(scadenza.Iva).ToString();
            txt_TotaleIva.Text = ((scadenza.ImportoDareIva + scadenza.ImportoAvereIva) - versatoOriscossoIva).ToString("###,##0.00"); 
            txt_TotaleDocumento.Text = (importoDocumentoDare + importoDocumentoAvere).ToString("###,##0.00");
            txt_TotDocumentoIva.Text = (importoDocumentoDareIva + importoDocumentoAvereIva).ToString("###,##0.00");
            txt_DataDocumentoModifica.Text = scadenza.DataProtocollo != null ? ((DateTime)scadenza.DataProtocollo).ToString("dd/MM/yyyy") : "";
            txt_ScadenzaDocumento.Text = scadenza.DataScadenza != null ? ((DateTime)scadenza.DataScadenza).ToString("dd/MM/yyyy") : "";

            ddl_BancaModifica.SelectedValue = scadenza.IdTipoBanca.ToString();

            return scadenza;
        }

        protected List<DatiScadenzario> CaricaListaFigli(DatiScadenzario scadenza)
        {
            Esito esito = new Esito();

            List<DatiScadenzario> listaScadenzeStessoProtocollo = Scadenzario_BLL.Instance.GetDatiScadenzarioByIdDatiProtocollo(scadenza.IdDatiProtocollo, ref esito);
            List<DatiScadenzario> listaFigli = new List<DatiScadenzario>();

            Scadenzario_BLL.Instance.RicercaFigli(scadenza, listaScadenzeStessoProtocollo, ref listaFigli);

            return listaFigli;
        }

        private DatiScadenzario CreaOggettoDatiScadenzario(ref Esito esito)
        {
            int _idDatiProtocollo = ViewState[VIEWSTATE_ID_PROTOCOLLO] != null ? int.Parse(ViewState[VIEWSTATE_ID_PROTOCOLLO].ToString()) : 0;

            Protocolli protocollo = Protocolli_BLL.Instance.getProtocolloById(ref esito, _idDatiProtocollo);
            DatiScadenzario scadenza = new DatiScadenzario();

            if (string.IsNullOrEmpty((string)ViewState[VIEWSTATE_ID_SCADENZA]))
            {
                ViewState[VIEWSTATE_ID_SCADENZA] = "0";
            }

            DateTime? dataFattura = protocollo.Data_inizio_lavorazione;

            scadenza.Id = Convert.ToInt16(ViewState[VIEWSTATE_ID_SCADENZA].ToString());
            scadenza.IdDatiProtocollo = _idDatiProtocollo;
            scadenza.IdTipoBanca = int.Parse(ddl_Banca.SelectedValue);
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

            //ValidaCampo(txt_CadenzaGiorni, 0, true, ref esito);
            ValidaCampo(txt_ClienteFornitore, "", true, ref esito);
            ValidaCampo(txt_DataDocumento, DateTime.Now, true, ref esito);

            if (ddl_Tipo.SelectedValue.ToUpper() == TIPO_CLIENTE)
            {
                scadenza.ImportoAvereIva = ValidaCampo(txt_ImportoIva, decimal.Parse("0"), true, ref esito);
                scadenza.ImportoAvere = string.IsNullOrWhiteSpace(txt_ImportoDocumento.Text) ? 0 : decimal.Parse(txt_ImportoDocumento.Text);
                scadenza.ImportoRiscosso = 0;
            }
            else
            {
                scadenza.ImportoDareIva = ValidaCampo(txt_ImportoIva, decimal.Parse("0"), true, ref esito);
                scadenza.ImportoDare = string.IsNullOrWhiteSpace(txt_ImportoDocumento.Text) ? 0 : decimal.Parse(txt_ImportoDocumento.Text);
                scadenza.ImportoVersato = 0;
            }

            scadenza.Note = string.Empty;
            scadenza.RagioneSocialeClienteFornitore = string.Empty;
            scadenza.ProtocolloRiferimento = string.Empty;
            scadenza.DataProtocollo = dataFattura;
            scadenza.Cassa = 0;

            return scadenza;
        }
        #endregion

        #region VALIDAZIONI E PULIZIA CAMPI
        private void gestisciPulsantiScadenza(string stato)
        {
            switch (stato)
            {
                case "INSERIMENTO":
                    btnInserisciScadenza.Visible = true;
                    btnModificaScadenza.Visible = btnAcconto.Visible = btnSaldoTotale.Visible = btnEliminaFattura.Visible = false;

                    div_CampiModifica.Visible = false;
                    div_CampiInserimento.Visible = true;

                    txt_ImportoDocumento.Attributes.Remove("ReadOnly");
                    txt_Iva.Attributes.Remove("disabled");
                    txt_NumeroRate.Attributes.Remove("disabled");
                    //txt_AnticipoImporto.Attributes.Remove("disabled");
                    //txt_CadenzaGiorni.Attributes.Remove("disabled");
                    ddl_APartireDa.Attributes.Remove("disabled");

                    break;
                case "MODIFICA":
                    btnInserisciScadenza.Visible = false;
                    btnModificaScadenza.Visible = btnAcconto.Visible = btnSaldoTotale.Visible = btnEliminaFattura.Visible = true;

                    div_CampiModifica.Visible = true;
                    div_CampiInserimento.Visible = false;

                    txt_ImportoDocumento.Attributes.Add("ReadOnly", "ReadOnly");
                    txt_Iva.Attributes.Add("disabled", "disabled");
                    txt_NumeroRate.Attributes.Add("disabled", "disabled");
                    //txt_AnticipoImporto.Attributes.Add("disabled", "disabled");
                    //txt_CadenzaGiorni.Attributes.Add("disabled", "disabled");
                    ddl_APartireDa.Attributes.Add("disabled", "disabled");

                    break;
                default:
                    btnInserisciScadenza.Visible = false;
                    btnModificaScadenza.Visible = btnAcconto.Visible = btnSaldoTotale.Visible = false;

                    break;
            }
        }

        private Esito ValidaDatiAcconto()
        {
            Esito esito = new Esito();

            ValidaCampo(txt_DataVersamentoRiscossione, "", true, ref esito);
            ValidaCampo(txt_VersatoAccontoIva, "", true, ref esito);
            return esito;
        }

        private Esito ValidaDatiScadenzaDaSalvare()
        {
            Esito esito = new Esito();

            ValidaCampo(txt_IvaModifica, "", true, ref esito);
            ValidaCampo(txt_VersatoIva, "", true, ref esito);
            ValidaCampo(txt_ScadenzaDocumento, "", true, ref esito);
            if (!string.IsNullOrWhiteSpace(txt_VersatoIva.Text) && (decimal.Parse(txt_VersatoIva.Text) != 0)) ValidaCampo(txt_DataVersamentoRiscossione, "", true, ref esito);

            return esito;
        }
        

        private void NascondiErroriValidazione()
        {
            txt_ClienteFornitore.CssClass = txt_ClienteFornitore.CssClass.Replace("erroreValidazione", "");
            txt_DataDocumento.CssClass = txt_DataDocumento.CssClass.Replace("erroreValidazione", "");
            txt_ImportoIva.CssClass = txt_ImportoDocumento.CssClass.Replace("erroreValidazione", "");
            //txt_CadenzaGiorni.CssClass = txt_CadenzaGiorni.CssClass.Replace("erroreValidazione", "");

            txt_VersatoIva.CssClass = txt_Versato.CssClass.Replace("erroreValidazione", "");
            txt_IvaModifica.CssClass = txt_IvaModifica.CssClass.Replace("erroreValidazione", "");
            txt_ScadenzaDocumento.CssClass = txt_ScadenzaDocumento.CssClass.Replace("erroreValidazione", "");

            txt_VersatoAccontoIva.CssClass = txt_VersatoAccontoIva.CssClass.Replace("erroreValidazione", "");
            txt_DataVersamentoRiscossione.CssClass = txt_DataVersamentoRiscossione.CssClass.Replace("erroreValidazione", "");
        }

        private void PulisciCampiAcconto()
        {
            txt_VersatoAccontoIva.Text = "";
            txt_DataVersamentoRiscossione.Text = "";
        }

        private void PulisciCampiDettaglio()
        {
            ddl_Tipo.SelectedIndex =
            ddl_APartireDa.SelectedIndex =
            ddl_Banca.SelectedIndex = 0;
            ddl_PosticipoPagamento.SelectedIndex = 0;

            txt_ClienteFornitore.Text =
            txt_DataDocumento.Text =
            txt_NumeroDocumento.Text =
            txt_ImportoDocumento.Text =
            txt_ImportoIva.Text =
            txt_NumeroRate.Text = string.Empty;
            //txt_AnticipoImporto.Text = 
            //txt_CadenzaGiorni.Text = string.Empty;
        }

        private void PulisciFiltriRicerca()
        {
            ddl_TipoAnagrafica.SelectedValue = "";
            ddlFatturaPagata.SelectedValue = "0";

            txt_RagioneSociale.Text =
            txt_NumeroFattura.Text =
            txt_DataFatturaDa.Text =
            txt_DataFatturaA.Text =
            txt_DataDa.Text =
            txt_DataA.Text = string.Empty;
        }

        private void AbilitaBottoni(bool isUtenteAbilitatoInScrittura)
        {
            clbtnInserisciScadenza.Visible = isUtenteAbilitatoInScrittura;
            gv_scadenze.Columns[11].Visible = isUtenteAbilitatoInScrittura;
        }
        #endregion

        #region INSERIMENTO
        protected void btnInsScadenza_Click(object sender, EventArgs e)
        {
            lbl_IntestazionePopup.Text = "Nuova scadenza";
            Esito esito = new Esito();
            txt_Iva.Text = Config_BLL.Instance.getConfig(ref esito, "IVA").Valore;

            div_Fattura.Visible = true;
            div_DatiCreazioneScadenza.Visible = true;
            ddl_Tipo.Attributes.Remove("Disabled");

            ddl_fattura.Items.Clear();

            ViewState[VIEWSTATE_ID_SCADENZA] = "";
            PulisciCampiDettaglio();
            NascondiErroriValidazione();
            gestisciPulsantiScadenza("INSERIMENTO");

            ddl_Tipo.Attributes.Remove("Disabled");

            List<Protocolli> listaProtocolloNonInScadenzario = Scadenzario_BLL.Instance.getFattureNonInScadenzario(ddl_Tipo.SelectedValue, ref esito);

            string tipoAnagrafica = ddl_Tipo.SelectedItem.Text;

            if (listaProtocolloNonInScadenzario.Count == 0)
            {
                ddl_fattura.Items.Add(new ListItem("<nessuna fattura trovata>", ""));
            }
            else
            {
                ddl_fattura.Items.Add(new ListItem("<seleziona una fattura>", ""));
                foreach (Protocolli protocollo in listaProtocolloNonInScadenzario)
                {
                    ddl_fattura.Items.Add(new ListItem("Protocollo: " + protocollo.Numero_protocollo + " - " + tipoAnagrafica + ": " + protocollo.Cliente + " - Lavorazione: " + protocollo.Lavorazione, protocollo.Id.ToString()));
                }
            }
            ddl_fattura.SelectedIndex = 0;

            ddl_APartireDa.SelectedIndex = 1;

            pnlContainer.Visible = true;
        }

        protected void btnInserisciScadenza_Click(object sender, EventArgs e)
        {
            Esito esito = new Esito();
            DatiScadenzario datiScadenzario = CreaOggettoDatiScadenzario(ref esito);

            //anticipo importo è stato disattivato
            string anticipoImportoIva = "0";// string.IsNullOrWhiteSpace(txt_AnticipoImporto.Text) ? "0" : txt_AnticipoImporto.Text;
            decimal ivaDecimal = decimal.Parse(txt_Iva.Text);
            string anticipoImporto = (decimal.Parse(anticipoImportoIva) / (1 + (ivaDecimal / 100))).ToString();

            if (esito.Codice == Esito.ESITO_OK)
            {
                if (decimal.Parse(anticipoImportoIva) >= (datiScadenzario.ImportoDareIva + datiScadenzario.ImportoAvereIva))
                {
                    ShowWarning("L'anticipo deve essere inferiore all'intero importo");
                }
                else
                {
                    NascondiErroriValidazione();

                    if (string.IsNullOrEmpty(txt_NumeroRate.Text) || txt_NumeroRate.Text == "0") txt_NumeroRate.Text = "1";

                    int posticipoPagamento = int.Parse(ddl_PosticipoPagamento.SelectedValue);
                    //DateTime? dataPartenzaPagamento = datiScadenzario.DataProtocollo == null ? DateTime.Now : datiScadenzario.DataProtocollo;
                    
                    DateTime dataPartenzaPagamento;
                    
                    if (ddl_APartireDa.SelectedValue == "0") // INIZIO CALCOLO SCADENZA DATA FATTURA
                    {
                        dataPartenzaPagamento = datiScadenzario.DataProtocollo == null ? DateTime.Now : (DateTime)datiScadenzario.DataProtocollo;
                    }
                    else  // INIZIO CALCOLO SCADENZA DA FINE MESE
                    {
                        //dataPartenzaPagamento = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1);
                        dataPartenzaPagamento = DateTime.Now;
                    }
                    //prendo ultimo giorno del mese successivo
                    dataPartenzaPagamento = new DateTime(dataPartenzaPagamento.Year, dataPartenzaPagamento.Month, 1).AddMonths(posticipoPagamento);
                    dataPartenzaPagamento = ((DateTime)dataPartenzaPagamento).AddDays(-1);

                    int cadenzaGiorni = 30;
                    //if (!string.IsNullOrEmpty(txt_CadenzaGiorni.Text)) cadenzaGiorni = int.Parse(txt_CadenzaGiorni.Text);



                    Scadenzario_BLL.Instance.CreaDatiScadenzario(datiScadenzario,
                                                                 anticipoImporto,
                                                                 anticipoImportoIva,
                                                                 txt_Iva.Text,
                                                                 txt_NumeroRate.Text,
                                                                 ddl_Tipo.SelectedValue,
                                                                 dataPartenzaPagamento,
                                                                 cadenzaGiorni,
                                                                 ref esito);

                    if (esito.Codice != Esito.ESITO_OK)
                    {
                        if (esito.Codice != Esito.ESITO_KO_ERRORE_VALIDAZIONE) log.Error(esito.Descrizione);
                        ShowError(esito.Descrizione);
                    }

                    ShowSuccess("Scadenza inserita correttamente");
                    btnChiudiPopup_Click(null, null);

                    ViewState[VIEWSTATE_ID_PROTOCOLLO] = null;

                    RicercaScadenze();
                }
            }
        }
        #endregion

        #region MODIFICA
        protected void btnModificaScadenza_Click(object sender, EventArgs e)
        {
            ViewState[VIEWSTATE_TIPOMODIFICA] = null;
            Esito esito = new Esito();

            int idScadenza = Convert.ToInt16(ViewState[VIEWSTATE_ID_SCADENZA].ToString());
            DatiScadenzario scadenzaCorrente = Scadenzario_BLL.Instance.GetDatiScadenzarioById(ref esito, idScadenza);

            if (esito.Codice == Esito.ESITO_OK)
            {
                ViewState[VIEWSTATE_TIPOMODIFICA] = TipoModifica.IMPORTO_UGUALE;

                lblMessaggioPopup.Text = "Confermi modifica rata?";
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriPanelModificaScadenzaConFigli", script: "javascript: document.getElementById('" + panelModificaScadenzaConFigli.ClientID + "').style.display='block'", addScriptTags: true);

                #region VECCHIO CODICE
                //esito = ValidaDatiScadenzaDaSalvare();

                //if (esito.Codice == Esito.ESITO_OK)
                //{
                //List<DatiScadenzario> listaFigli = CaricaListaFigli(scadenzaCorrente);

                //if (listaFigli.Count > 1)
                //{
                //    decimal maxImportoIvaDaVersare = listaFigli.Sum(x => x.ImportoAvereIva + x.ImportoDareIva);
                //    decimal maxImportoDaVersare = listaFigli.Sum(x => x.ImportoAvere + x.ImportoDare);
                //    decimal importoVersatoIvaDecimal = decimal.Parse(txt_VersatoIva.Text);

                //    if (importoVersatoIvaDecimal > maxImportoIvaDaVersare)
                //    {
                //        esito.Codice = Esito.ESITO_KO_ERRORE_VALIDAZIONE;
                //        esito.Descrizione = "L'importo in fase di registrazione eccede la rata corrente e le rate ad essa legate." + Environment.NewLine + "È possibile impostare una valore massimo di " + maxImportoIvaDaVersare + "€.";
                //    }
                //    else
                //    {
                //        //DatiScadenzario scadenzaOriginale = Scadenzario_BLL.Instance.GetDatiScadenzarioById(ref esito, scadenzaCorrente.Id);
                //        //decimal importoOriginaleScadenzaIva = scadenzaOriginale.ImportoRiscossoIva + scadenzaOriginale.ImportoVersatoIva; //scadenzaCorrente.ImportoRiscossoIva + scadenzaCorrente.ImportoVersatoIva;
                //        decimal importoOriginaleScadenzaIva = scadenzaCorrente.ImportoRiscossoIva + scadenzaCorrente.ImportoVersatoIva;

                //        if (importoOriginaleScadenzaIva != importoVersatoIvaDecimal) // AGGIORNAMENTO IMPORTO SCADENZA
                //        {
                //            DateTime dataNuovaScadenza = (DateTime)listaFigli[1].DataScadenza;
                //            decimal importoScadenzaFigli = maxImportoIvaDaVersare - importoVersatoIvaDecimal;

                //            if (importoVersatoIvaDecimal == 0) // SE IMPORTO È PARI A 0 VIENE CANCELLATA LA RATA CORRENTE E RIPRISTINATA UN'UNICA RATA DA SALDARE CON IMPORTI DEI FIGLI 
                //            {
                //                ViewState[VIEWSTATE_TIPOMODIFICA] = TipoModifica.IMPORTO_ZERO;

                //                lblMessaggioPopup.Text = "La scadenza selezionata verrà eliminata.<br/>Tutte le rate precedentemente immesse e legate alla scadenza selezionata verranno sostituite da una rata unica di importo pari a " + importoScadenzaFigli + "€, con scadenza " + ((DateTime)scadenzaCorrente.DataScadenza).ToString("dd/MM/yyyy"); ;
                //                ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriPanelModificaScadenzaConFigli", script: "javascript: document.getElementById('" + panelModificaScadenzaConFigli.ClientID + "').style.display='block'", addScriptTags: true);
                //            }
                //            else if (importoScadenzaFigli > 0) // MODIFICA ALLA SCADENZA NON COPRE IMPORTO TOTALE DELLE ALTRE RATE
                //            {
                //                ViewState[VIEWSTATE_TIPOMODIFICA] = TipoModifica.IMPORTO_TRA_0_E_MAX;

                //                lblMessaggioPopup.Text = "La scadenza selezionata verrà aggiornata.<br/>Tutte le rate precedentemente immesse e legate alla scadenza selezionata verranno sostituite da una rata unica di importo pari a " + importoScadenzaFigli + "€, con scadenza " + dataNuovaScadenza.ToString("dd/MM/yyyy");
                //                ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriPanelModificaScadenzaConFigli", script: "javascript: document.getElementById('" + panelModificaScadenzaConFigli.ClientID + "').style.display='block'", addScriptTags: true);
                //            }
                //            else // MODIFICA ALLA SCADENZA COPRE TOTALEMENTE IMPORTO ALTRE RATE
                //            {
                //                ViewState[VIEWSTATE_TIPOMODIFICA] = TipoModifica.IMPORTO_MAX;

                //                lblMessaggioPopup.Text = "La scadenza selezionata verrà aggiornata.<br/>Il suo importo copre l'ammontare delle altre rate precedentemente immesse, che verranno quindi eliminate.";
                //                ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriPanelModificaScadenzaConFigli", script: "javascript: document.getElementById('" + panelModificaScadenzaConFigli.ClientID + "').style.display='block'", addScriptTags: true);
                //            }
                //        }
                //        else // SOLO AGGIORNAMENTO SCADENZA (IMPORTO UGUALE A QUELLO ORIGINALE)
                //        {
                //            ViewState[VIEWSTATE_TIPOMODIFICA] = TipoModifica.IMPORTO_UGUALE;

                //            lblMessaggioPopup.Text = "Confermi modifica Scadenza?";
                //            ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriPanelModificaScadenzaConFigli", script: "javascript: document.getElementById('" + panelModificaScadenzaConFigli.ClientID + "').style.display='block'", addScriptTags: true);
                //        }
                //    }
                //}
                //else // se la scadenza non ha figli deve necessariamente coprire l'importo oppure generare figli
                //{
                //    decimal differenzaImportoIva = decimal.Parse(txt_TotaleIva.Text);

                //    if ((!string.IsNullOrEmpty(txt_VersatoIva.Text) && decimal.Parse(txt_VersatoIva.Text) != 0) && differenzaImportoIva > 0)
                //    {
                //        esito.Codice = Esito.ESITO_KO_ERRORE_VALIDAZIONE;
                //        esito.Descrizione = "L'importo in fase di registrazione non copre totalmente la rata corrente." + Environment.NewLine + "Modificare il valore in " + (scadenzaCorrente.ImportoAvereIva + scadenzaCorrente.ImportoDareIva) + " oppure selezionare 'Acconto'.";
                //    }
                //    else if ((!string.IsNullOrEmpty(txt_VersatoIva.Text) && decimal.Parse(txt_VersatoIva.Text) != 0) && differenzaImportoIva < 0)
                //    {
                //        esito.Codice = Esito.ESITO_KO_ERRORE_VALIDAZIONE;
                //        esito.Descrizione = "L'importo in fase di registrazione eccede la rata corrente." + Environment.NewLine + "È possibile impostare una valore massimo di " + (scadenzaCorrente.ImportoAvereIva + scadenzaCorrente.ImportoDareIva) + "€.";
                //    }
                //    else
                //    {
                //        ViewState[VIEWSTATE_TIPOMODIFICA] = TipoModifica.NO_FIGLI;

                //        lblMessaggioPopup.Text = "Confermi modifica Scadenza?";
                //        ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriPanelModificaScadenzaConFigli", script: "javascript: document.getElementById('" + panelModificaScadenzaConFigli.ClientID + "').style.display='block'", addScriptTags: true);
                //    }
                //}
                // }
                #endregion
            }
            if (esito.Codice != Esito.ESITO_OK)
            {
                if (esito.Codice != Esito.ESITO_KO_ERRORE_VALIDAZIONE) log.Error(esito.Descrizione);
                ShowError(esito.Descrizione);
            }
        }

        // PULIRE IL CODICE SEGUENTE
        protected void btnOKModificaScadenzaConFigli_Click(object sender, EventArgs e)
        {
            Esito esito = new Esito();

            int idScadenza = Convert.ToInt16(ViewState[VIEWSTATE_ID_SCADENZA].ToString());
            DatiScadenzario scadenzaCorrente = Scadenzario_BLL.Instance.GetDatiScadenzarioById(ref esito, idScadenza);

            List<DatiScadenzario> listaFigli = CaricaListaFigli(scadenzaCorrente);
            decimal maxImportoIvaDaVersare = listaFigli.Sum(x => x.ImportoAvereIva + x.ImportoDareIva);
            decimal maxImportoDaVersare = listaFigli.Sum(x => x.ImportoAvere + x.ImportoDare);

            switch ((TipoModifica)ViewState[VIEWSTATE_TIPOMODIFICA])
            {
                case TipoModifica.IMPORTO_ZERO: // scadenza corrente e figli eliminati; viene creato un unico figlio con importo pari alla somma totale 
                    scadenzaCorrente.ImportoVersato = 0;
                    scadenzaCorrente.ImportoVersatoIva = 0;
                    scadenzaCorrente.ImportoRiscosso = 0;
                    scadenzaCorrente.ImportoRiscossoIva = 0;

                    scadenzaCorrente.DataVersamento = null;
                    scadenzaCorrente.DataRiscossione = null;

                    scadenzaCorrente.ImportoAvereIva = 0;
                    scadenzaCorrente.ImportoAvere = 0;
                    scadenzaCorrente.ImportoDareIva = 0;
                    scadenzaCorrente.ImportoDare = 0;

                    if (ddl_Tipo.SelectedValue.ToUpper() == TIPO_CLIENTE)
                    {
                        scadenzaCorrente.ImportoAvereIva = maxImportoIvaDaVersare;
                        scadenzaCorrente.ImportoAvere = maxImportoDaVersare;
                    }
                    else
                    {
                        scadenzaCorrente.ImportoDareIva = maxImportoIvaDaVersare;
                        scadenzaCorrente.ImportoDare = maxImportoDaVersare;
                    }
                    scadenzaCorrente.IdTipoBanca = int.Parse(ddl_Banca.SelectedValue);

                    Scadenzario_BLL.Instance.CancellaFigli_CreaDatiScadenzario(listaFigli, scadenzaCorrente, int.Parse(ddl_BancaModifica.SelectedValue), ref esito);
                    break;
                //case TipoModifica.IMPORTO_TRA_0_E_MAX: // scadenza corrente aggiornata; figli eliminati e sostituiti da un unico figlio 
                //    if (ddl_Tipo.SelectedValue.ToUpper() == TIPO_CLIENTE)
                //    {
                //        scadenzaCorrente.ImportoAvereIva = decimal.Parse(txt_VersatoIva.Text);
                //        scadenzaCorrente.ImportoAvere = decimal.Parse(txt_Versato.Text);
                //    }
                //    else
                //    {
                //        scadenzaCorrente.ImportoDareIva = decimal.Parse(txt_VersatoIva.Text);
                //        scadenzaCorrente.ImportoDare = decimal.Parse(txt_Versato.Text);
                //    }

                //    listaFigli = listaFigli.Where(x => x.Id != scadenzaCorrente.Id).ToList<DatiScadenzario>();
                //    decimal importoVersatoIvaDecimal = decimal.Parse(txt_VersatoIva.Text);
                //    decimal importoScadenzaFigli = maxImportoIvaDaVersare - importoVersatoIvaDecimal;
                //    DateTime dataNuovaScadenza = (DateTime)listaFigli[0].DataScadenza;

                //    Scadenzario_BLL.Instance.CancellaFigli_AggiungiPagamento(listaFigli, scadenzaCorrente, ddl_Tipo.SelectedValue, txt_Versato.Text, txt_VersatoIva.Text, importoScadenzaFigli.ToString(), txt_IvaModifica.Text, txt_ScadenzaDocumento.Text, txt_DataVersamentoRiscossione.Text, int.Parse(ddl_BancaModifica.SelectedValue), dataNuovaScadenza.ToString("dd/MM/yyyy"), ref esito);
                //    break;
                case TipoModifica.IMPORTO_MAX: // scadenza corrente aggiornata; figli eliminati
                    if (ddl_Tipo.SelectedValue.ToUpper() == TIPO_CLIENTE)
                    {
                        scadenzaCorrente.ImportoAvereIva = decimal.Parse(txt_VersatoIva.Text);
                        scadenzaCorrente.ImportoAvere = decimal.Parse(txt_Versato.Text);
                    }
                    else
                    {
                        scadenzaCorrente.ImportoDareIva = decimal.Parse(txt_VersatoIva.Text);
                        scadenzaCorrente.ImportoDare = decimal.Parse(txt_Versato.Text);
                    }

                    listaFigli = listaFigli.Where(x => x.Id != scadenzaCorrente.Id).ToList<DatiScadenzario>();

                    Scadenzario_BLL.Instance.CancellaFigli_AggiornaDatiScadenzario(listaFigli, scadenzaCorrente, ddl_Tipo.SelectedValue, txt_Versato.Text, txt_VersatoIva.Text, txt_IvaModifica.Text, txt_ScadenzaDocumento.Text, txt_DataVersamentoRiscossione.Text, int.Parse(ddl_BancaModifica.SelectedValue), ref esito);
                    break;
                case TipoModifica.IMPORTO_UGUALE: // modifica senza variazione di importo: figli rimangono invariati 
                    decimal importoOringinale = scadenzaCorrente.ImportoVersato + scadenzaCorrente.ImportoRiscosso;
                    decimal importoOriginaleIva = scadenzaCorrente.ImportoVersatoIva + scadenzaCorrente.ImportoRiscossoIva;


                    Scadenzario_BLL.Instance.AggiornaDatiScadenzario(scadenzaCorrente, ddl_Tipo.SelectedValue, importoOringinale.ToString(), importoOriginaleIva.ToString(), txt_IvaModifica.Text, txt_ScadenzaDocumento.Text, txt_DataVersamentoRiscossione.Text, int.Parse(ddl_BancaModifica.SelectedValue), ref esito);
                    break;
                case TipoModifica.NO_FIGLI: // scadenza senza figli: importo deve essere coperto totalemnte
                    if (ddl_Tipo.SelectedValue.ToUpper() == TIPO_CLIENTE)
                    {
                        scadenzaCorrente.ImportoAvereIva = decimal.Parse(txt_VersatoIva.Text);
                        scadenzaCorrente.ImportoAvere = decimal.Parse(txt_Versato.Text);
                    }
                    else
                    {
                        scadenzaCorrente.ImportoDareIva = decimal.Parse(txt_VersatoIva.Text);
                        scadenzaCorrente.ImportoDare = decimal.Parse(txt_Versato.Text);
                    }


                    Scadenzario_BLL.Instance.AggiornaDatiScadenzario(scadenzaCorrente, ddl_Tipo.SelectedValue, txt_Versato.Text, txt_VersatoIva.Text, txt_IvaModifica.Text, txt_ScadenzaDocumento.Text, txt_DataVersamentoRiscossione.Text, int.Parse(ddl_BancaModifica.SelectedValue), ref esito);
                    break;
                default:
                    break;
            }

            if (esito.Codice != Esito.ESITO_OK)
            {
                if (esito.Codice != Esito.ESITO_KO_ERRORE_VALIDAZIONE) log.Error(esito.Descrizione);
                ShowError(esito.Descrizione);
            }
            else
            {
                ShowSuccess("La scadenza selezionata è stata aggiornata correttemente");
                btnChiudiPopup_Click(null, null);

                RicercaScadenze();
            }
        }
        #endregion

        #region ACCONTO
        protected void btnAcconto_Click(object sender, EventArgs e)
        {
            NascondiErroriValidazione();
            PulisciCampiAcconto();
            Esito esito = new Esito();

            int idScadenza = Convert.ToInt16(ViewState[VIEWSTATE_ID_SCADENZA].ToString());
            DatiScadenzario scadenzaDaAggiornare = Scadenzario_BLL.Instance.GetDatiScadenzarioById(ref esito, idScadenza);

            if (esito.Codice == Esito.ESITO_OK)
            {

                List<DatiScadenzario> listaFigli = CaricaListaFigli(scadenzaDaAggiornare);
                decimal importoScadenzaFigli = listaFigli.Sum(x => x.ImportoAvereIva + x.ImportoDareIva); ;
                hf_importoScadenzaFigli.Value = importoScadenzaFigli.ToString();

                lbl_MesaggioAcconto.Text = "È in corso la registrazione di un acconto della rata selezionata.<br/>Tutte le rate precedentemente immesse e legate alla scadenza selezionata verranno sostituite da una rata unica.<br/>Il suo importo massimo è pari a <b>" + importoScadenzaFigli + " €</b>."; ;

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriAcconto", script: "javascript: document.getElementById('" + panelAcconto.ClientID + "').style.display='block'", addScriptTags: true);
            }
            else
            {
                if (esito.Codice != Esito.ESITO_KO_ERRORE_VALIDAZIONE) log.Error(esito.Descrizione);
                ShowError(esito.Descrizione);
            }
        }

        protected void btn_OkAcconto_Click(object sender, EventArgs e)
        {
            Esito esito = new Esito();

            int idScadenza = Convert.ToInt16(ViewState[VIEWSTATE_ID_SCADENZA].ToString());
            DatiScadenzario scadenzaDaAggiornare = Scadenzario_BLL.Instance.GetDatiScadenzarioById(ref esito, idScadenza);

            if (esito.Codice == Esito.ESITO_OK)
            {

                esito = ValidaDatiAcconto();


                if (esito.Codice == Esito.ESITO_OK)
                {
                    List<DatiScadenzario> listaFigli = CaricaListaFigli(scadenzaDaAggiornare);

                    decimal maxImportoIvaDaVersare = listaFigli.Sum(x => x.ImportoAvereIva + x.ImportoDareIva);
                    decimal versatoAccontoIvaDecimal = decimal.Parse(txt_VersatoAccontoIva.Text);
                    decimal ivaDecimal = decimal.Parse(txt_IvaModifica.Text);
                    decimal versatoAccontoDecimal = versatoAccontoIvaDecimal / (1 + (ivaDecimal / 100));
                    decimal differenzaImportoIva = maxImportoIvaDaVersare - versatoAccontoIvaDecimal;
                    DateTime dataVersamentoRiscossione = DateTime.Parse(txt_DataVersamentoRiscossione.Text);
                    DateTime dataScadenzaDocumento = DateTime.Parse(txt_ScadenzaDocumento.Text);

                    if (versatoAccontoIvaDecimal == 0)
                    {
                        esito.Codice = Esito.ESITO_KO_ERRORE_VALIDAZIONE;
                        esito.Descrizione = "L'importo dell'acconto non può essere pari a zero. <br/>Se si desidera eliminare la rata usare il pulsante nella griglia dei risultati della ricerca";
                    }
                    else if (versatoAccontoIvaDecimal >= maxImportoIvaDaVersare)
                    {
                        esito.Codice = Esito.ESITO_KO_ERRORE_VALIDAZIONE;
                        esito.Descrizione = "L'acconto copre l'intero importo delle rate. <br/>Se si desidera effettuare il saldo utilizzare l'apposito pulsante.";
                    }
                    else
                    {
                        DateTime dataNuovaScadenza = dataVersamentoRiscossione.AddDays(30);

                        if (listaFigli.Count > 1)
                        {
                            listaFigli = listaFigli.Where(x => x.Id != scadenzaDaAggiornare.Id).ToList<DatiScadenzario>();
                            Scadenzario_BLL.Instance.CancellaFigli_AggiungiPagamento(listaFigli, scadenzaDaAggiornare, ddl_Tipo.SelectedValue, versatoAccontoDecimal, versatoAccontoIvaDecimal, differenzaImportoIva, ivaDecimal, dataScadenzaDocumento, dataVersamentoRiscossione, int.Parse(ddl_BancaModifica.SelectedValue), dataNuovaScadenza, ref esito);
                        }
                        else
                        {
                            Scadenzario_BLL.Instance.AggiungiPagamento(scadenzaDaAggiornare, ddl_Tipo.SelectedValue, versatoAccontoDecimal, versatoAccontoIvaDecimal, differenzaImportoIva, ivaDecimal, dataScadenzaDocumento, dataVersamentoRiscossione, int.Parse(ddl_BancaModifica.SelectedValue), dataNuovaScadenza, ref esito);
                        }

                    }
                }

                if (esito.Codice == Esito.ESITO_OK)
                {
                    ShowSuccess("La registrazione dell'acconto è stata effettuata");
                    btnChiudiPopup_Click(null, null);

                    RicercaScadenze();
                }
                else
                {
                    if (esito.Codice != Esito.ESITO_KO_ERRORE_VALIDAZIONE) log.Error(esito.Descrizione);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriAcconto", script: "javascript: document.getElementById('" + panelAcconto.ClientID + "').style.display='block'", addScriptTags: true);
                }
            }
        }
        #endregion

        #region SALDO
        protected void btnSaldoTotale_Click(object sender, EventArgs e)
        {
            Esito esito = new Esito();
            int idScadenza = Convert.ToInt16(ViewState[VIEWSTATE_ID_SCADENZA].ToString());
            DatiScadenzario scadenza = Scadenzario_BLL.Instance.GetDatiScadenzarioById(ref esito, Convert.ToInt16(idScadenza));

            if (esito.Codice == Esito.ESITO_OK)
            {
                lbl_importoSaldo.Text = (scadenza.ImportoDareIva + scadenza.ImportoAvereIva).ToString();
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriSaldo", script: "javascript: document.getElementById('" + panelSaldo.ClientID + "').style.display='block'", addScriptTags: true);
            }
        }

        protected void btn_OkSaldo_Click(object sender, EventArgs e)
        {
            Esito esito = new Esito();

            int idScadenza = Convert.ToInt16(ViewState[VIEWSTATE_ID_SCADENZA].ToString());
            DatiScadenzario scadenzaDaSaldare = Scadenzario_BLL.Instance.GetDatiScadenzarioById(ref esito, idScadenza);

            if (esito.Codice == Esito.ESITO_OK)
            {
                ValidaCampo(txt_DataSaldo, "", true, ref esito);

                if (esito.Codice == Esito.ESITO_OK)
                {
                    Scadenzario_BLL.Instance.SaldoTotale(scadenzaDaSaldare, ddl_Tipo.SelectedValue, txt_DataSaldo.Text, int.Parse(ddl_BancaModifica.SelectedValue), ref esito);
                }

                if (esito.Codice == Esito.ESITO_OK)
                {
                    ShowSuccess("Il saldo della rata è stato registrato");

                    btnChiudiPopup_Click(null, null);

                    RicercaScadenze();
                }
                else
                {
                    if (esito.Codice != Esito.ESITO_KO_ERRORE_VALIDAZIONE) log.Error(esito.Descrizione);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriSaldo", script: "javascript: document.getElementById('" + panelSaldo.ClientID + "').style.display='block'", addScriptTags: true);
                }
            }

        }

        // ELIMINARE QUESTO METODO
        private Esito SaldoTotaleScadenza(ref DatiScadenzario scadenza, ref string dataVersamentoRiscossione)
        {
            Esito esito = new Esito();

            int idScadenza = Convert.ToInt16(ViewState[VIEWSTATE_ID_SCADENZA].ToString());
            scadenza = Scadenzario_BLL.Instance.GetDatiScadenzarioById(ref esito, Convert.ToInt16(idScadenza));

            dataVersamentoRiscossione = ValidaCampo(txt_DataVersamentoRiscossione, "", true, ref esito);

            return esito;
        }
        #endregion

        #region ELIMNAZIONE FATTURA
        protected void btnEliminaFattura_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "eliminaFattura", script: "javascript: document.getElementById('" + panelEliminaFattura.ClientID + "').style.display='block'", addScriptTags: true);
        }

        protected void btnOkEliminaFattura_Click(object sender, EventArgs e)
        {
            Esito esito = new Esito();
            int idScadenzaSelezionata = Convert.ToInt16(ViewState[VIEWSTATE_ID_SCADENZA].ToString());

            Scadenzario_BLL.Instance.DeleteDatiScadenzario(idScadenzaSelezionata, ref esito);

            if (esito.Codice == Esito.ESITO_OK)
            {
                ShowSuccess("Eliminazione avvenuta con successo");

                // commentare le righe seguenti se dopo la cancellazione si intende aprire la maschera di inserimento per inserire di nuovo la fattura eliminata
                btnChiudiPopup_Click(null, null);
                RicercaScadenze();
            }
            else
            {
                log.Error(esito.Descrizione);
            }
        }
        #endregion
    }
}