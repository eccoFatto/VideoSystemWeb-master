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
using System.Security.Cryptography;

namespace VideoSystemWeb.Utente
{
    public partial class GestioneUtenti : BasePage
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
                //Esito esito = basePage.CaricaListeTipologiche();
                // CARICO LE COMBO
                if (string.IsNullOrEmpty(esito.Descrizione))
                {
                    ddlTipoUtente.Items.Clear();
                    cmbMod_TipoUtente.Items.Clear();
                    ddlTipoUtente.Items.Add("");
                    foreach (Tipologica tipologiaUtente in SessionManager.ListaTipiUtente)
                    {
                        ListItem item = new ListItem();
                        item.Text = tipologiaUtente.nome;
                        item.Value = tipologiaUtente.nome;

                        ddlTipoUtente.Items.Add(item);

                        ListItem itemMod = new ListItem();
                        itemMod.Text = tipologiaUtente.nome;
                        itemMod.Value = tipologiaUtente.id.ToString();

                        cmbMod_TipoUtente.Items.Add(itemMod);
                    }

                    // SE UTENTE ABILITATO ALLE MODIFICHE FACCIO VEDERE I PULSANTI DI MODIFICA
                    abilitaBottoni(basePage.AbilitazioneInScrittura());

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

        private void abilitaBottoni(bool utenteAbilitatoInScrittura)
        {
            if (!utenteAbilitatoInScrittura)
            {
                divBtnInserisciUtente.Visible = false;
                btnModificaUtente.Visible = false;
                btnAnnullaUtente.Visible = false;
                btnEliminaUtente.Visible = false;
                btnGestisciUtente.Visible = false;
            }
            else
            {
                divBtnInserisciUtente.Visible = true;
                btnModificaUtente.Visible = true;
                btnAnnullaUtente.Visible = false;
                btnEliminaUtente.Visible = false;
                btnGestisciUtente.Visible = true;
            }
        }
        protected void btnRicercaUtente_Click(object sender, EventArgs e)
        {
            string queryRicerca = ConfigurationManager.AppSettings["QUERY_SEARCH_UTENTI"];

            queryRicerca = queryRicerca.Replace("@cognome", tbCognome.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@nome", tbNome.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@username", tbUserName.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@tipologia", ddlTipoUtente.SelectedValue.ToString().Trim().Replace("'", "''"));

            Esito esito = new Esito();
            DataTable dtUtenti = Base_DAL.GetDatiBySql(queryRicerca, ref esito);
            gv_utenti.DataSource = dtUtenti;
            gv_utenti.DataBind();

        }

        protected void gv_utenti_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                // PRENDO L'ID DELL' UTENTE SELEZIONATO
                string idUtenteSelezionato = e.Row.Cells[1].Text;
                ImageButton myButtonEdit = e.Row.FindControl("imgEdit") as ImageButton;
                myButtonEdit.Attributes.Add("onclick", "mostraUtente('" + idUtenteSelezionato + "');");
            }

        }

        protected void gv_utenti_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_utenti.PageIndex = e.NewPageIndex;
            btnRicercaUtente_Click(null, null);

        }

        protected void btnEditUtente_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hf_idUtente.Value) || (!string.IsNullOrEmpty((string)ViewState["idUtente"])))
            {
                if (!string.IsNullOrEmpty(hf_idUtente.Value)) ViewState["idUtente"] = hf_idUtente.Value;
                editUtente();
                AttivaDisattivaModificaUtente(true);
                gestisciPulsantiUtente("VISUALIZZAZIONE");
                pnlContainer.Visible = true;
            }
        }

        protected void btnInsUtente_Click(object sender, EventArgs e)
        {
            // VISUALIZZO FORM INSERIMENTO NUOVO UTENTE
            ViewState["idUtente"] = "";
            editUtenteVuoto();
            AttivaDisattivaModificaUtente(false);
            gestisciPulsantiUtente("INSERIMENTO");

            pnlContainer.Visible = true;

        }

        protected void btnChiudiPopupServer_Click(object sender, EventArgs e)
        {
            pnlContainer.Visible = false;
        }

        protected void btnGestisciUtente_Click(object sender, EventArgs e)
        {
            AttivaDisattivaModificaUtente(false);
            gestisciPulsantiUtente("MODIFICA");

        }

        protected void btnInserisciUtente_Click(object sender, EventArgs e)
        {
            // INSERISCO UTENTE
            Esito esito = new Esito();
            Utenti utente = CreaOggettoUtente(ref esito);

            //if (esito.Codice != Esito.ESITO_OK)
            //{
            //    basePage.ShowWarning("Controllare i campi evidenziati");
            //}
            if (esito.Codice == Esito.ESITO_OK)
            {
                NascondiErroriValidazione();

                int iRet = Anag_Utenti_BLL.Instance.CreaUtente(utente, ref esito);
                if (iRet > 0)
                {
                    // UNA VOLTA INSERITO CORRETTAMENTE PUO' ESSERE MODIFICATO
                    hf_idUtente.Value = iRet.ToString();
                    ViewState["idUtente"] = hf_idUtente.Value;
                    hf_tipoOperazione.Value = "VISUALIZZAZIONE";
                }

                if (esito.Codice != Esito.ESITO_OK)
                {
                    log.Error(esito.Descrizione);
                    basePage.ShowError(esito.Descrizione);
                }
                basePage.ShowSuccess("Inserito Utente " + utente.Username);
                btnEditUtente_Click(null, null);
            }

        }

        protected void btnModificaUtente_Click(object sender, EventArgs e)
        {
            // SALVO MODIFICHE UTENTE
            Esito esito = new Esito();
            Utenti utente = CreaOggettoUtente(ref esito);

            if (esito.Codice != Esito.ESITO_OK)
            {
                log.Error(esito.Descrizione);
                //basePage.ShowWarning("Controllare i campi evidenziati!");
            }
            else
            {
                esito = Anag_Utenti_BLL.Instance.AggiornaUtente(utente);

                if (esito.Codice != Esito.ESITO_OK)
                {
                    log.Error(esito.Descrizione);
                    basePage.ShowError(esito.Descrizione);

                }
                btnEditUtente_Click(null, null);
            }

        }

        protected void btnEliminaUtente_Click(object sender, EventArgs e)
        {
            Esito esito = new Esito();

            if (!string.IsNullOrEmpty((string)ViewState["idUtente"]))
            {
                esito = Anag_Utenti_BLL.Instance.EliminaUtente(Convert.ToInt32(ViewState["idUtente"].ToString()));
                if (esito.Codice != Esito.ESITO_OK)
                {
                    basePage.ShowError(esito.Descrizione);
                    AttivaDisattivaModificaUtente(true);
                }
                else
                {
                    AttivaDisattivaModificaUtente(true);
                    pnlContainer.Visible = false;
                    btnRicercaUtente_Click(null, null);
                }

            }


        }

        protected void btnAnnullaUtente_Click(object sender, EventArgs e)
        {
            AttivaDisattivaModificaUtente(true);
            gestisciPulsantiUtente("ANNULLAMENTO");

        }

        private void editUtenteVuoto()
        {
            Esito esito = new Esito();
            pulisciCampiDettaglio();
        }
        private void editUtente()
        {
            string idUtente = (string)ViewState["idUtente"];
            Esito esito = new Esito();

            if (!string.IsNullOrEmpty(idUtente))
            {
                Entity.Utenti utente = Anag_Utenti_BLL.Instance.getUtenteById(Convert.ToInt16(idUtente), ref esito);
                if (esito.Codice == 0)
                {
                    pulisciCampiDettaglio();

                    // RIEMPIO I CAMPI DEL DETTAGLIO UTENTE
                    tbMod_Cognome.Text = utente.Cognome;
                    tbMod_Nome.Text = utente.Nome;
                    tbMod_Username.Text = utente.Username;
                    tbMod_Descrizione.Text = utente.Descrizione;
                    tbMod_Email.Text = utente.Email;


                    //TIPI UTENTE
                    ListItem trovati = cmbMod_TipoUtente.Items.FindByValue(utente.Id_tipoUtente.ToString());
                    if (trovati != null)
                    {
                        cmbMod_TipoUtente.SelectedValue = trovati.Value;
                    }
                    else
                    {
                        cmbMod_TipoUtente.Text = "";
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
        private void pulisciCampiDettaglio()
        {
            tbMod_Cognome.Text = "";
            tbMod_Nome.Text = "";
            tbMod_Username.Text = "";
            tbMod_Descrizione.Text = "";
            tbMod_Email.Text = "";
            cmbMod_TipoUtente.SelectedIndex = 0;

        }
        private void AttivaDisattivaModificaUtente(bool attivaModifica)
        {

            tbMod_Cognome.ReadOnly = attivaModifica;
            tbMod_Nome.ReadOnly = attivaModifica;
            tbMod_Username.ReadOnly = attivaModifica;
            tbMod_Descrizione.ReadOnly = attivaModifica;
            tbMod_Email.ReadOnly = attivaModifica;

            if (attivaModifica)
            {
                cmbMod_TipoUtente.Attributes.Add("disabled", "");
            }
            else
            {
                cmbMod_TipoUtente.Attributes.Remove("disabled");
            }
        }

        private void gestisciPulsantiUtente(string stato)
        {
            switch (stato)
            {
                case "VISUALIZZAZIONE":

                    btnInserisciUtente.Visible = false;
                    btnModificaUtente.Visible = false;
                    btnEliminaUtente.Visible = false;
                    btnAnnullaUtente.Visible = false;
                    if (!basePage.AbilitazioneInScrittura())
                    {
                        btnGestisciUtente.Visible = false;
                    }
                    else
                    {
                        btnGestisciUtente.Visible = true;
                    }

                    break;
                case "INSERIMENTO":
                    btnInserisciUtente.Visible = true;
                    btnModificaUtente.Visible = false;
                    btnEliminaUtente.Visible = false;
                    btnAnnullaUtente.Visible = false;
                    btnGestisciUtente.Visible = false;

                    break;
                case "MODIFICA":
                    btnInserisciUtente.Visible = false;
                    btnModificaUtente.Visible = true;
                    btnEliminaUtente.Visible = true;
                    btnAnnullaUtente.Visible = true;
                    btnGestisciUtente.Visible = false;

                    break;
                case "ANNULLAMENTO":
                    btnInserisciUtente.Visible = false;
                    btnModificaUtente.Visible = false;
                    btnEliminaUtente.Visible = false;
                    btnAnnullaUtente.Visible = false;
                    btnGestisciUtente.Visible = true;

                    break;
                default:
                    btnInserisciUtente.Visible = false;
                    btnModificaUtente.Visible = false;
                    btnEliminaUtente.Visible = false;
                    btnAnnullaUtente.Visible = false;
                    btnGestisciUtente.Visible = true;

                    break;
            }

        }
        private Utenti CreaOggettoUtente(ref Esito esito)
        {
            Utenti utente = new Utenti();

            if (string.IsNullOrEmpty((string)ViewState["idUtente"]))
            {
                ViewState["idUtente"] = "0";

                MD5 md5Hash = MD5.Create();
                string pwdEncrypted = GetMd5Hash(md5Hash, ConfigurationManager.AppSettings["DEFAULT_PASSWORD"]);
                md5Hash.Dispose();
                utente.Password = pwdEncrypted;
            }
            else
            {
                
                utente.Password = getPassword(Convert.ToInt16(ViewState["idUtente"].ToString()));
            }
            utente.Id = Convert.ToInt16(ViewState["idUtente"].ToString());
            utente.Id_tipoUtente = Convert.ToInt32(cmbMod_TipoUtente.SelectedValue);

            utente.Cognome = BasePage.ValidaCampo(tbMod_Cognome, "", true, ref esito);
            utente.Nome = BasePage.ValidaCampo(tbMod_Nome, "", false, ref esito);
            utente.Username = BasePage.ValidaCampo(tbMod_Username, "", true, ref esito);

            utente.Email = BasePage.ValidaIndirizzoEmail(tbMod_Email,true, ref esito);

            utente.Descrizione = BasePage.ValidaCampo(tbMod_Descrizione, "", false, ref esito);
            utente.Attivo = true;

            return utente;
        }

        private string getPassword(int idUtente)
        {
            string pwdRet = "";
            Esito esito = new Esito();
            DataTable dt = Base_DAL.GetDatiBySql("SELECT password from ANAG_UTENTI WHERE ID = " + idUtente.ToString(), ref esito);
            if (dt!=null && dt.Rows!=null && dt.Rows.Count == 1)
            {
                pwdRet = dt.Rows[0]["password"].ToString();
            }
            return pwdRet;
        }

        private void NascondiErroriValidazione()
        {
            tbMod_Cognome.CssClass = tbMod_Cognome.CssClass.Replace("erroreValidazione", "");
            tbMod_Nome.CssClass = tbMod_Nome.CssClass.Replace("erroreValidazione", "");
            tbMod_Username.CssClass = tbMod_Username.CssClass.Replace("erroreValidazione", "");
            tbMod_Descrizione.CssClass = tbMod_Descrizione.CssClass.Replace("erroreValidazione", "");
            tbMod_Email.CssClass = tbMod_Email.CssClass.Replace("erroreValidazione", "");
        }
    }
}