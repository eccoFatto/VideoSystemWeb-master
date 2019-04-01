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

        protected void Page_Load(object sender, EventArgs e)
        {
            //ScriptManager scrManager = (ScriptManager)this.Master.FindControl("ScriptManager1");
            //scrManager.RegisterPostBackControl(fuFileProt);

            if (!Page.IsPostBack)
            {
                BasePage p = new BasePage();
                Esito esito = p.CaricaListeTipologiche();

                // CARICO LE COMBO
                if (string.IsNullOrEmpty(esito.descrizione))
                {
                    ddlTipoProtocollo.Items.Clear();
                    cmbMod_Tipologia.Items.Clear();
                    ddlTipoProtocollo.Items.Add("");
                    foreach (Tipologica tipologiaProtocollo in p.listaTipiProtocolli)
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
                    //abilitaBottoni(p.AbilitazioneInScrittura());


                }
                else
                {
                    Session["ErrorPageText"] = esito.descrizione;
                    string url = String.Format("~/pageError.aspx");
                    Response.Redirect(url, true);
                }

            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriTabGiusta", script: "openDettaglioProtocollo('" + hf_tabChiamata.Value + "');", addScriptTags: true);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "chiudiLoader", script: "$('.loader').hide();", addScriptTags: true);

        }

        protected void btnEditProtocollo_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hf_idProt.Value) || (!string.IsNullOrEmpty((string)ViewState["idProtocollo"])))
            {
                if (!string.IsNullOrEmpty(hf_idProt.Value)) ViewState["idProtocollo"] = hf_idProt.Value;
                editProtocollo();
                AttivaDisattivaModificaProtocollo(true);
                gestisciPulsantiProtocollo("VISUALIZZAZIONE");
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriTabGiusta", script: "openDettaglioProtocollo('Protocollo');", addScriptTags: true);
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
                    btnGestisciProtocollo.Visible = true;

                    imgbtnCreateNewCodLav.Attributes.Add("disabled","");
                    imgbtnSelectCodLav.Attributes.Add("disabled", "");
                    btnAnnullaCaricamento.Attributes.Add("disabled", "");
                    fuFileProt.Attributes.Add("disabled", "");

                    break;
                case "INSERIMENTO":
                    btnInserisciProtocollo.Visible = true;
                    btnModificaProtocollo.Visible = false;
                    btnEliminaProtocollo.Visible = false;
                    btnAnnullaProtocollo.Visible = false;
                    btnGestisciProtocollo.Visible = false;

                    imgbtnCreateNewCodLav.Attributes.Remove("disabled");
                    imgbtnSelectCodLav.Attributes.Remove("disabled");
                    btnAnnullaCaricamento.Attributes.Remove("disabled");
                    fuFileProt.Attributes.Remove("disabled");
                    break;
                case "MODIFICA":
                    btnInserisciProtocollo.Visible = false;
                    btnModificaProtocollo.Visible = true;
                    btnEliminaProtocollo.Visible = true;
                    btnAnnullaProtocollo.Visible = true;
                    btnGestisciProtocollo.Visible = false;

                    imgbtnCreateNewCodLav.Attributes.Remove("disabled");
                    imgbtnSelectCodLav.Attributes.Remove("disabled");
                    btnAnnullaCaricamento.Attributes.Remove("disabled");
                    fuFileProt.Attributes.Remove("disabled");

                    break;
                case "ANNULLAMENTO":
                    btnInserisciProtocollo.Visible = false;
                    btnModificaProtocollo.Visible = false;
                    btnEliminaProtocollo.Visible = false;
                    btnAnnullaProtocollo.Visible = false;
                    btnGestisciProtocollo.Visible = true;

                    imgbtnCreateNewCodLav.Attributes.Add("disabled", "");
                    imgbtnSelectCodLav.Attributes.Add("disabled", "");
                    btnAnnullaCaricamento.Attributes.Add("disabled", "");
                    fuFileProt.Attributes.Add("disabled", "");

                    break;
                default:
                    btnInserisciProtocollo.Visible = false;
                    btnModificaProtocollo.Visible = false;
                    btnEliminaProtocollo.Visible = false;
                    btnAnnullaProtocollo.Visible = false;
                    btnGestisciProtocollo.Visible = true;

                    imgbtnCreateNewCodLav.Attributes.Add("disabled", "");
                    imgbtnSelectCodLav.Attributes.Add("disabled", "");
                    btnAnnullaCaricamento.Attributes.Add("disabled", "");
                    fuFileProt.Attributes.Add("disabled", "");
                    break;
            }

        }

        protected void btnInsProtocollo_Click(object sender, EventArgs e)
        {
            // VISUALIZZO FORM INSERIMENTO NUOVO PROTOCOLLO
            ViewState["idProtocollo"] = "";
            editProtocolloVuoto();
            AttivaDisattivaModificaProtocollo(false);
            gestisciPulsantiProtocollo("INSERIMENTO");

            pnlContainer.Visible = true;
        }

        protected void btnInserisciProtocollo_Click(object sender, EventArgs e)
        {

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
            queryRicerca = queryRicerca.Replace("@codiceLavoro", tbCodiceLavoro.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@cliente", tbRagioneSociale.Text.Trim().Replace("'", "''"));
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


                // PRENDO L'ID DEL COLLABORATORE SELEZIONATO

                string idProtocolloSelezionato = e.Row.Cells[1].Text;

                //foreach (TableCell item in e.Row.Cells)
                //{
                //    if (!string.IsNullOrEmpty(item.ID) && item.ID.Equals("imgEdit"))
                //    {
                //        item.Attributes["onclick"] = "mostraProtocollo('" + idProtocolloSelezionato + "');";
                //    }
                //}

                ImageButton myButton = e.Row.FindControl("imgEdit") as ImageButton;
                myButton.Attributes.Add("onclick", "mostraProtocollo('" + idProtocolloSelezionato + "');");
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
            tbMod_Cliente.Text = "";
            tbMod_NomeFile.Text = "";
            tbMod_Descrizione.Text = "";
            cmbMod_Tipologia.SelectedIndex = 0;

        }

        private void AttivaDisattivaModificaProtocollo(bool attivaModifica)
        {
            tbMod_CodiceLavoro.ReadOnly = attivaModifica;
            tbMod_NumeroProtocollo.ReadOnly = true;
            tbMod_ProtocolloRiferimento.ReadOnly = attivaModifica;
            tbMod_DataProtocollo.ReadOnly = true;
            tbMod_Cliente.ReadOnly = attivaModifica;
            tbMod_Cliente.ReadOnly = attivaModifica;
            tbMod_Descrizione.ReadOnly = attivaModifica;
            
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
        }
        private void editProtocollo()
        {
            string idProtocollo = (string)ViewState["idProtocollo"];
            Esito esito = new Esito();

            if (!string.IsNullOrEmpty(idProtocollo))
            {
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
                        tbMod_DataProtocollo.Text = protocollo.Data_protocollo.ToString("dd/MM/yyyy");
                    }
                    tbMod_ProtocolloRiferimento.Text = protocollo.Protocollo_riferimento;
                    tbMod_Cliente.Text = protocollo.Cliente;
                    tbMod_NomeFile.Text = protocollo.PathDocumento;
                    tbMod_Descrizione.Text = protocollo.Descrizione;

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

                }
                else
                {
                    Session["ErrorPageText"] = esito.descrizione;
                    string url = String.Format("~/pageError.aspx");
                    Response.Redirect(url, true);
                }
            }

        }

        protected void gv_protocolli_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "modifica":

                    break;
                default:
                    break;
            }

        }

        protected void uploadButton_Click(object sender, EventArgs e)
        {

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
                ViewState["idProtocollo"] = 0;
            }

            protocollo.Id = Convert.ToInt16(ViewState["idProtocollo"].ToString());

            protocollo.Id_tipo_protocollo = Convert.ToInt32(cmbMod_Tipologia.SelectedValue);
            protocollo.Numero_protocollo = BasePage.ValidaCampo(tbMod_NumeroProtocollo, "", true, ref esito);
            protocollo.PathDocumento = "";
            protocollo.Protocollo_riferimento = BasePage.ValidaCampo(tbMod_ProtocolloRiferimento, "", false, ref esito);
            protocollo.Cliente = BasePage.ValidaCampo(tbMod_Cliente, "", false, ref esito);
            protocollo.Codice_lavoro = BasePage.ValidaCampo(tbMod_CodiceLavoro, "", false, ref esito);
            protocollo.Descrizione = BasePage.ValidaCampo(tbMod_Descrizione,"", false, ref esito);
            protocollo.Attivo = true;

            return protocollo;
        }

        protected void imgbtnCreateNewCodLav_Click(object sender, ImageClickEventArgs e)
        {
            tbMod_CodiceLavoro.Text = Protocolli_BLL.Instance.getCodLavFormattato();
        }

        protected void btnAnnullaCaricamento_Click(object sender, EventArgs e)
        {
            //    if (fuFileProt.HasFile)
            //    {
            //        fuFileProt.Dispose();
            //    }
        }

        protected void AsyncFileUpload1_UploadedComplete (object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            System.Threading.Thread.Sleep(2000);
            if (fuFileProt.HasFile)
            {
                string strPath = MapPath("~/DOCUMENTI/PROTOCOLLI/") + Path.GetFileName(e.filename);
                fuFileProt.SaveAs(strPath);
            }
        }

        protected void AsyncFileUpload1_UploadedFileError(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            lblStatus.Text = e.statusMessage;
            lblStatus.Visible = true;
        }
    }
}