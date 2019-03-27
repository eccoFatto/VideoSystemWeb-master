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
                    ddlTipoProtocollo.Items.Add("");
                    foreach (Tipologica tipologiaProtocollo in p.listaTipiProtocolli)
                    {
                        ListItem item = new ListItem();
                        item.Text = tipologiaProtocollo.nome;
                        item.Value = tipologiaProtocollo.nome;
                        ddlTipoProtocollo.Items.Add(item);
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
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriTabGiusta", script: "openDettaglioAzienda('" + hf_tabChiamata.Value + "');", addScriptTags: true);
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

                    btnEditProtocollo.Visible = basePage.AbilitazioneInScrittura();
                    //btnSalva.Visible = false;
                    //btnAnnulla.Visible = false;
                    //btnElimina.Visible = false;
                    //btnConfermaInserimento.Visible = false;

                    //if (basePage.AbilitazioneInScrittura())
                    //{
                    //    btnAnnullaIndirizzo_Click(null, null);
                    //    btnAnnullaTelefono_Click(null, null);
                    //    btnAnnullaEmail_Click(null, null);
                    //    phEmail.Visible = false;
                    //    phTelefoni.Visible = false;
                    //    phQualifiche.Visible = false;
                    //    phIndirizzi.Visible = false;
                    //}
                    break;
                case "INSERIMENTO":
                    //btnModifica.Visible = false;
                    //btnSalva.Visible = false;
                    //btnAnnulla.Visible = false;
                    //btnElimina.Visible = false;
                    //btnConfermaInserimento.Visible = true;
                    break;
                case "MODIFICA":
                    //btnModifica.Visible = false;
                    //btnSalva.Visible = true;
                    //btnAnnulla.Visible = true;
                    //btnElimina.Visible = true;
                    //btnConfermaInserimento.Visible = false;
                    break;
                case "ANNULLAMENTO":
                    //btnModifica.Visible = true;
                    //btnSalva.Visible = false;
                    //btnAnnulla.Visible = false;
                    //btnElimina.Visible = false;
                    //btnConfermaInserimento.Visible = false;
                    break;
                default:
                    //btnModifica.Visible = true;
                    //btnSalva.Visible = false;
                    //btnAnnulla.Visible = false;
                    //btnElimina.Visible = false;
                    //btnConfermaInserimento.Visible = false;
                    break;
            }

        }

        protected void btnInsProtocollo_Click(object sender, EventArgs e)
        {

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

        }

        private void AttivaDisattivaModificaProtocollo(bool attivaModifica)
        {
            tbMod_CodiceLavoro.ReadOnly = attivaModifica;
            tbMod_NumeroProtocollo.ReadOnly = attivaModifica;

            //if (attivaModifica)
            //{
            //    cmbMod_RegioneRiferimento.Attributes.Add("disabled", "");
            //    cbMod_Assunto.Attributes.Add("disabled", "");
            //}
            //else
            //{
            //    cmbMod_RegioneRiferimento.Attributes.Remove("disabled");
            //    cbMod_Assunto.Attributes.Remove("disabled");
            //}
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


                    //TIPI PROTOCOLLO
                    //ListItem trovati = cbMod_TipoProtocollo.Items.FindByValue(tipoProtocollo.Pagamento.ToString());
                    //if (trovati != null)
                    //{
                    //    cbMod_TipoProtocollo.SelectedValue = trovati.Value;
                    //}
                    //else
                    //{
                    //    cmbMod_Pagamento.Text = "";
                    //}

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