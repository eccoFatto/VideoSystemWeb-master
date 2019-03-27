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
                    btnModificaProtocollo.Visible = true;
                    btnEliminaProtocollo.Visible = true;
                    btnAnnullaProtocollo.Visible = true;

                    break;
                case "INSERIMENTO":
                    btnInserisciProtocollo.Visible = true;
                    btnModificaProtocollo.Visible = false;
                    btnEliminaProtocollo.Visible = false;
                    btnAnnullaProtocollo.Visible = true;
                    break;
                case "MODIFICA":
                    btnInserisciProtocollo.Visible = false;
                    btnModificaProtocollo.Visible = true;
                    btnEliminaProtocollo.Visible = true;
                    btnAnnullaProtocollo.Visible = true;
                    break;
                case "ANNULLAMENTO":
                    btnInserisciProtocollo.Visible = false;
                    btnModificaProtocollo.Visible = false;
                    btnEliminaProtocollo.Visible = false;
                    btnAnnullaProtocollo.Visible = false;
                    break;
                default:
                    btnInserisciProtocollo.Visible = false;
                    btnModificaProtocollo.Visible = false;
                    btnEliminaProtocollo.Visible = false;
                    btnAnnullaProtocollo.Visible = false;
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

        }

        protected void btnEliminaProtocollo_Click(object sender, EventArgs e)
        {

        }

        protected void btnAnnullaProtocollo_Click(object sender, EventArgs e)
        {

        }

        protected void btnRicercaProtocollo_Click(object sender, EventArgs e)
        {
            string queryRicerca = ConfigurationManager.AppSettings["QUERY_SEARCH_PROTOCOLLI"];

            queryRicerca = queryRicerca.Replace("@numeroProtocollo", tbNumeroProtocollo.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@codiceLavoro", tbCodiceLavoro.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@cliente", tbRagioneSociale.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@tipoProtocollo", ddlTipoProtocollo.SelectedValue.ToString().Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@protocolloRiferimento", tbProtocolloRiferimento.Text.Trim().Replace("'","''"));

            Esito esito = new Esito();
            DataTable dtAziende = Base_DAL.getDatiBySql(queryRicerca, ref esito);
            gv_protocolli.DataSource = dtAziende;
            gv_protocolli.DataBind();

        }

        protected void gv_protocolli_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // PRENDO L'ID DEL COLLABORATORE SELEZIONATO

                string idProtocolloSelezionato = e.Row.Cells[0].Text;

                foreach (TableCell item in e.Row.Cells)
                {
                    item.Attributes["onclick"] = "mostraProtocollo('" + idProtocolloSelezionato + "');";
                }
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
            tbMod_Cliente.Text = "";
            tbMod_Descrizione.Text = "";
            cmbMod_Tipologia.SelectedIndex = 0;

        }

        private void AttivaDisattivaModificaProtocollo(bool attivaModifica)
        {
            tbMod_CodiceLavoro.ReadOnly = attivaModifica;
            tbMod_NumeroProtocollo.ReadOnly = attivaModifica;
            tbMod_ProtocolloRiferimento.ReadOnly = attivaModifica;
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
                    tbMod_ProtocolloRiferimento.Text = protocollo.Protocollo_riferimento;
                    tbMod_Cliente.Text = protocollo.Cliente;
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

    }
}