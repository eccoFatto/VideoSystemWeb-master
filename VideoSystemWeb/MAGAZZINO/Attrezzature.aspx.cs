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

namespace VideoSystemWeb.MAGAZZINO
{
    public partial class Attrezzature : BasePage
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

                // CARICO LE COMBO
                if (string.IsNullOrEmpty(esito.descrizione))
                {
                    ddlTipoCategoria.Items.Clear();
                    cmbMod_Categoria.Items.Clear();
                    ddlTipoCategoria.Items.Add("");
                    foreach (Tipologica tipologiaCategoria in SessionManager.ListaTipiCategorieMagazzino)
                    {
                        ListItem item = new ListItem();
                        item.Text = tipologiaCategoria.nome;
                        item.Value = tipologiaCategoria.nome;

                        ddlTipoCategoria.Items.Add(item);

                        ListItem itemMod = new ListItem();
                        itemMod.Text = tipologiaCategoria.nome;
                        itemMod.Value = tipologiaCategoria.id.ToString();

                        cmbMod_Categoria.Items.Add(itemMod);
                    }

                    ddlTipoSubCategoria.Items.Clear();
                    cmbMod_SubCategoria.Items.Clear();
                    ddlTipoSubCategoria.Items.Add("");
                    cmbMod_SubCategoria.Items.Add("");
                    foreach (Tipologica tipologiaSubCategoria in SessionManager.ListaTipiSubCategorieMagazzino)
                    {
                        ListItem item = new ListItem();
                        item.Text = tipologiaSubCategoria.nome;
                        item.Value = tipologiaSubCategoria.nome;

                        ddlTipoSubCategoria.Items.Add(item);

                        ListItem itemMod = new ListItem();
                        itemMod.Text = tipologiaSubCategoria.nome;
                        itemMod.Value = tipologiaSubCategoria.id.ToString();

                        cmbMod_SubCategoria.Items.Add(itemMod);
                    }

                    ddlTipoPosizioneMagazzino.Items.Clear();
                    cmbMod_Posizione.Items.Clear();
                    ddlTipoPosizioneMagazzino.Items.Add("");
                    foreach (Tipologica tipologiaPosizione in SessionManager.ListaTipiPosizioniMagazzino)
                    {
                        ListItem item = new ListItem();
                        item.Text = tipologiaPosizione.nome;
                        item.Value = tipologiaPosizione.nome;

                        ddlTipoPosizioneMagazzino.Items.Add(item);

                        ListItem itemMod = new ListItem();
                        itemMod.Text = tipologiaPosizione.nome;
                        itemMod.Value = tipologiaPosizione.id.ToString();

                        cmbMod_Posizione.Items.Add(itemMod);
                    }

                    // SE UTENTE ABILITATO ALLE MODIFICHE FACCIO VEDERE I PULSANTI DI MODIFICA
                    abilitaBottoni(basePage.AbilitazioneInScrittura());

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

        protected void btnRicercaAttrezzatura_Click(object sender, EventArgs e)
        {
            string queryRicerca = ConfigurationManager.AppSettings["QUERY_SEARCH_ATTREZZATURE"];

            queryRicerca = queryRicerca.Replace("@codiceVS", tbCodiceVS.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@descrizione", tbDescrizione.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@marca", tbMarca.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@modello", tbModello.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@seriale", tbSeriale.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@dataAcquisto", tbDataAcquisto.Text.Trim().Replace("'", "''"));

            queryRicerca = queryRicerca.Replace("@categoria", ddlTipoCategoria.SelectedItem.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@subcategoria", ddlTipoSubCategoria.SelectedItem.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@posizione", ddlTipoPosizioneMagazzino.SelectedItem.Text.Trim().Replace("'", "''"));

            Esito esito = new Esito();
            DataTable dtAttrezzature = Base_DAL.getDatiBySql(queryRicerca, ref esito);
            gv_attrezzature.DataSource = dtAttrezzature;
            gv_attrezzature.DataBind();

        }

        protected void gv_attrezzature_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // PRENDO L'ID DELL' ATTREZZATURA SELEZIONATA
                string idAttrezzaturaSelezionata = e.Row.Cells[1].Text;
                ImageButton myButtonEdit = e.Row.FindControl("imgEdit") as ImageButton;
                myButtonEdit.Attributes.Add("onclick", "mostraAttrezzatura('" + idAttrezzaturaSelezionata + "');");
            }

        }

        protected void gv_attrezzature_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_attrezzature.PageIndex = e.NewPageIndex;
            btnRicercaAttrezzatura_Click(null, null);

        }

        protected void btnEditAttrezzatura_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hf_idAttrezzatura.Value) || (!string.IsNullOrEmpty((string)ViewState["idAttrezzatura"])))
            {
                if (!string.IsNullOrEmpty(hf_idAttrezzatura.Value)) ViewState["idAttrezzatura"] = hf_idAttrezzatura.Value;
                editAttrezzatura();
                AttivaDisattivaModificaAttrezzatura(true);
                gestisciPulsantiAttrezzatura("VISUALIZZAZIONE");
                pnlContainer.Visible = true;
            }
        }

        protected void btnInsAttrezzatura_Click(object sender, EventArgs e)
        {
            // VISUALIZZO FORM INSERIMENTO NUOVA ATTREZZATURA
            ViewState["idAttrezzatura"] = "";
            editAttrezzaturaVuota();
            AttivaDisattivaModificaAttrezzatura(false);
            gestisciPulsantiAttrezzatura("INSERIMENTO");

            pnlContainer.Visible = true;
        }

        protected void btnChiudiPopupServer_Click(object sender, EventArgs e)
        {
            pnlContainer.Visible = false;
        }

        protected void btnGestisciAttrezzatura_Click(object sender, EventArgs e)
        {
            AttivaDisattivaModificaAttrezzatura(false);
            gestisciPulsantiAttrezzatura("MODIFICA");
        }

        protected void btnInserisciAttrezzatura_Click(object sender, EventArgs e)
        {
            // INSERISCO ATTREZZATURA
            Esito esito = new Esito();
            AttrezzatureMagazzino attrezzatura = CreaOggettoAttrezzatura(ref esito);

            if (esito.codice != Esito.ESITO_OK)
            {
                basePage.ShowError("Controllare i campi evidenziati");
            }
            else
            {
                NascondiErroriValidazione();

                int iRet = AttrezzatureMagazzino_BLL.Instance.CreaAttrezzatura(attrezzatura,ref esito);
                if (iRet > 0)
                {
                    // UNA VOLTA INSERITO CORRETTAMENTE PUO' ESSERE MODIFICATO
                    hf_idAttrezzatura.Value = iRet.ToString();
                    ViewState["idAttrezzatura"] = hf_idAttrezzatura.Value;
                    hf_tipoOperazione.Value = "VISUALIZZAZIONE";
                }

                if (esito.codice != Esito.ESITO_OK)
                {
                    log.Error(esito.descrizione);
                    basePage.ShowError(esito.descrizione);
                }
                else { 
                    basePage.ShowSuccess("Inserita Attrezzatura " + attrezzatura.Descrizione);
                    btnEditAttrezzatura_Click(null, null);
                }
            }

        }

        protected void btnModificaAttrezzatura_Click(object sender, EventArgs e)
        {
            // SALVO MODIFICHE ATTREZZATURA
            Esito esito = new Esito();
            AttrezzatureMagazzino attrezzatura = CreaOggettoAttrezzatura(ref esito);

            if (esito.codice != Esito.ESITO_OK)
            {
                log.Error(esito.descrizione);
                basePage.ShowError("Controllare i campi evidenziati!");
            }
            else
            {
                esito = AttrezzatureMagazzino_BLL.Instance.AggiornaAttrezzatura(attrezzatura);

                if (esito.codice != Esito.ESITO_OK)
                {
                    log.Error(esito.descrizione);
                    basePage.ShowError(esito.descrizione);

                }
                btnEditAttrezzatura_Click(null, null);
            }

        }

        protected void btnEliminaAttrezzatura_Click(object sender, EventArgs e)
        {
            Esito esito = new Esito();

            if (!string.IsNullOrEmpty((string)ViewState["idAttrezzatura"]))
            {
                esito = AttrezzatureMagazzino_BLL.Instance.EliminaAttrezzatura(Convert.ToInt32(ViewState["idAttrezzatura"].ToString()));
                //esito = AttrezzatureMagazzino_BLL.Instance.RemoveAttrezzatura(Convert.ToInt32(ViewState["idAttrezzatura"].ToString()));
                if (esito.codice != Esito.ESITO_OK)
                {
                    basePage.ShowError(esito.descrizione);
                    AttivaDisattivaModificaAttrezzatura(true);
                }
                else
                {
                    AttivaDisattivaModificaAttrezzatura(true);
                    pnlContainer.Visible = false;
                    btnRicercaAttrezzatura_Click(null, null);
                }

            }

        }

        protected void btnAnnullaAttrezzatura_Click(object sender, EventArgs e)
        {
            AttivaDisattivaModificaAttrezzatura(true);
            gestisciPulsantiAttrezzatura("ANNULLAMENTO");

        }

        private void abilitaBottoni(bool utenteAbilitatoInScrittura)
        {
            //annullaModifiche();
            if (!utenteAbilitatoInScrittura)
            {
                divBtnInserisciAttrezzatura.Visible = false;
                btnModificaAttrezzatura.Visible = false;
                btnAnnullaAttrezzatura.Visible = false;
                btnEliminaAttrezzatura.Visible = false;
                btnGestisciAttrezzatura.Visible = false;
            }
            else
            {
                divBtnInserisciAttrezzatura.Visible = true;
                btnModificaAttrezzatura.Visible = true;
                btnAnnullaAttrezzatura.Visible = false;
                btnEliminaAttrezzatura.Visible = false;
                btnGestisciAttrezzatura.Visible = true;
            }
        }
        private void NascondiErroriValidazione()
        {
            tbMod_CodiceVideoSystem.CssClass = tbMod_CodiceVideoSystem.CssClass.Replace("erroreValidazione", "");
            tbMod_DataAcquisto.CssClass = tbMod_DataAcquisto.CssClass.Replace("erroreValidazione", "");
            tbMod_Descrizione.CssClass = tbMod_Descrizione.CssClass.Replace("erroreValidazione", "");
            tbMod_Marca.CssClass = tbMod_Marca.CssClass.Replace("erroreValidazione", "");
            tbMod_Modello.CssClass = tbMod_Modello.CssClass.Replace("erroreValidazione", "");
            tbMod_Note.CssClass = tbMod_Note.CssClass.Replace("erroreValidazione", "");
            tbMod_Seriale.CssClass = tbMod_Seriale.CssClass.Replace("erroreValidazione", "");
            cmbMod_Categoria.CssClass = cmbMod_Categoria.CssClass.Replace("erroreValidazione", "");
            cmbMod_SubCategoria.CssClass = cmbMod_SubCategoria.CssClass.Replace("erroreValidazione", "");
            cmbMod_Posizione.CssClass = cmbMod_Posizione.CssClass.Replace("erroreValidazione", "");
        }
        private void editAttrezzatura()
        {
            string idAttrezzatura = (string)ViewState["idAttrezzatura"];
            Esito esito = new Esito();

            if (!string.IsNullOrEmpty(idAttrezzatura))
            {
                Entity.AttrezzatureMagazzino attrezzatura = AttrezzatureMagazzino_BLL.Instance.getAttrezzaturaById(ref esito, Convert.ToInt16(idAttrezzatura));
                if (esito.codice == 0)
                {
                    pulisciCampiDettaglio();

                    // RIEMPIO I CAMPI DEL DETTAGLIO ATTREZZATURA
                    tbMod_CodiceVideoSystem.Text = attrezzatura.Cod_vs;
                    
                    tbMod_DataAcquisto.Text = "";
                    if (attrezzatura.Data_acquisto != null)
                    {
                        tbMod_DataAcquisto.Text = ((DateTime)attrezzatura.Data_acquisto).ToString("dd/MM/yyyy");
                    }

                    tbMod_Descrizione.Text = attrezzatura.Descrizione;
                    tbMod_Marca.Text = attrezzatura.Marca;
                    tbMod_Modello.Text = attrezzatura.Modello;
                    tbMod_Note.Text = attrezzatura.Note;
                    tbMod_Seriale.Text = attrezzatura.Seriale;

                    //TIPI CATEGORIA
                    ListItem trovati = cmbMod_Categoria.Items.FindByValue(attrezzatura.Id_categoria.ToString());
                    if (trovati != null)
                    {
                        cmbMod_Categoria.SelectedValue = trovati.Value;
                    }
                    else
                    {
                        cmbMod_Categoria.Text = "";
                    }

                    //TIPI SUBCATEGORIA
                    trovati = cmbMod_SubCategoria.Items.FindByValue(attrezzatura.Id_subcategoria.ToString());
                    if (trovati != null)
                    {
                        cmbMod_SubCategoria.SelectedValue = trovati.Value;
                    }
                    else
                    {
                        cmbMod_SubCategoria.Text = "";
                    }

                    //TIPI POSIZIONI
                    trovati = cmbMod_Posizione.Items.FindByValue(attrezzatura.Id_posizione_magazzino.ToString());
                    if (trovati != null)
                    {
                        cmbMod_Posizione.SelectedValue = trovati.Value;
                    }
                    else
                    {
                        cmbMod_Posizione.Text = "";
                    }

                    cbMod_Disponibile.Checked = attrezzatura.Disponibile;
                    cbMod_Garanzia.Checked = attrezzatura.Garanzia;
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
            tbMod_CodiceVideoSystem.Text = "";
            tbMod_DataAcquisto.Text = "";
            tbMod_Descrizione.Text = "";
            tbMod_Marca.Text = "";
            tbMod_Modello.Text = "";
            tbMod_Note.Text = "";
            tbMod_Seriale.Text = "";
            cmbMod_Categoria.SelectedIndex = 0;
            cmbMod_SubCategoria.SelectedIndex = 0;
            cmbMod_Posizione.SelectedIndex = 0;
            cbMod_Disponibile.Checked = false;
            cbMod_Garanzia.Checked = false;

        }
        private void AttivaDisattivaModificaAttrezzatura(bool attivaModifica)
        {
            tbMod_CodiceVideoSystem.ReadOnly = attivaModifica;
            tbMod_DataAcquisto.ReadOnly = attivaModifica;
            tbMod_Descrizione.ReadOnly = attivaModifica;
            tbMod_Marca.ReadOnly = attivaModifica;
            tbMod_Modello.ReadOnly = attivaModifica;
            tbMod_Marca.ReadOnly = attivaModifica;
            tbMod_Modello.ReadOnly = attivaModifica;
            tbMod_Note.ReadOnly = attivaModifica;
            tbMod_Seriale.ReadOnly = attivaModifica;

            if (attivaModifica)
            {
                cmbMod_Categoria.Attributes.Add("disabled", "");
                cmbMod_SubCategoria.Attributes.Add("disabled", "");
                cmbMod_Posizione.Attributes.Add("disabled", "");
            }
            else
            {
                cmbMod_Categoria.Attributes.Remove("disabled");
                cmbMod_SubCategoria.Attributes.Remove("disabled");
                cmbMod_Posizione.Attributes.Remove("disabled");
            }

            cbMod_Disponibile.Enabled = !attivaModifica;
            cbMod_Garanzia.Enabled = !attivaModifica;
        }
        private void gestisciPulsantiAttrezzatura(string stato)
        {
            switch (stato)
            {
                case "VISUALIZZAZIONE":

                    btnInserisciAttrezzatura.Visible = false;
                    btnModificaAttrezzatura.Visible = false;
                    btnEliminaAttrezzatura.Visible = false;
                    btnAnnullaAttrezzatura.Visible = false;
                    if (!basePage.AbilitazioneInScrittura())
                    {
                        btnGestisciAttrezzatura.Visible = false;
                    }
                    else
                    {
                        btnGestisciAttrezzatura.Visible = true;
                    }

                    break;
                case "INSERIMENTO":
                    btnInserisciAttrezzatura.Visible = true;
                    btnModificaAttrezzatura.Visible = false;
                    btnEliminaAttrezzatura.Visible = false;
                    btnAnnullaAttrezzatura.Visible = false;
                    btnGestisciAttrezzatura.Visible = false;
                    break;
                case "MODIFICA":
                    btnInserisciAttrezzatura.Visible = false;
                    btnModificaAttrezzatura.Visible = true;
                    btnEliminaAttrezzatura.Visible = true;
                    btnAnnullaAttrezzatura.Visible = true;
                    btnGestisciAttrezzatura.Visible = false;

                    break;
                case "ANNULLAMENTO":
                    btnInserisciAttrezzatura.Visible = false;
                    btnModificaAttrezzatura.Visible = false;
                    btnEliminaAttrezzatura.Visible = false;
                    btnAnnullaAttrezzatura.Visible = false;
                    btnGestisciAttrezzatura.Visible = true;

                    break;
                default:
                    btnInserisciAttrezzatura.Visible = false;
                    btnModificaAttrezzatura.Visible = false;
                    btnEliminaAttrezzatura.Visible = false;
                    btnAnnullaAttrezzatura.Visible = false;
                    btnGestisciAttrezzatura.Visible = true;

                    break;
            }

        }
        private void editAttrezzaturaVuota()
        {
            Esito esito = new Esito();
            pulisciCampiDettaglio();
        }
        private AttrezzatureMagazzino CreaOggettoAttrezzatura(ref Esito esito)
        {
            AttrezzatureMagazzino attrezzatura = new AttrezzatureMagazzino();

            if (string.IsNullOrEmpty((string)ViewState["idAttrezzatura"]))
            {
                ViewState["idAttrezzatura"] = "0";
            }

            attrezzatura.Id = Convert.ToInt16(ViewState["idAttrezzatura"].ToString());

            attrezzatura.Cod_vs = BasePage.ValidaCampo(tbMod_CodiceVideoSystem, "",false, ref esito);
            attrezzatura.Data_acquisto = BasePage.ValidaCampo(tbMod_DataAcquisto, DateTime.Now, true, ref esito);
            attrezzatura.Descrizione = BasePage.ValidaCampo(tbMod_Descrizione, "", true, ref esito);
            attrezzatura.Disponibile = cbMod_Disponibile.Checked;
            attrezzatura.Garanzia = cbMod_Garanzia.Checked;
            attrezzatura.Id_categoria = Convert.ToInt32(cmbMod_Categoria.SelectedValue);
            attrezzatura.Id_posizione_magazzino = Convert.ToInt32(cmbMod_Posizione.SelectedValue);
            if (!string.IsNullOrEmpty(cmbMod_SubCategoria.SelectedValue)) attrezzatura.Id_subcategoria = Convert.ToInt32(cmbMod_SubCategoria.SelectedValue);
            attrezzatura.Marca = BasePage.ValidaCampo(tbMod_Marca, "", true, ref esito);
            attrezzatura.Modello = BasePage.ValidaCampo(tbMod_Modello, "", true, ref esito);
            attrezzatura.Seriale = BasePage.ValidaCampo(tbMod_Seriale, "", false, ref esito);
            attrezzatura.Note = BasePage.ValidaCampo(tbMod_Note, "", false, ref esito);
            attrezzatura.Attivo = true;

            return attrezzatura;
        }

    }
}