using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoSystemWeb.BLL;
using VideoSystemWeb.Entity;
namespace VideoSystemWeb.Articoli
{
    public partial class GestioneRaggruppamentiArticoli : BasePage
    {
        BasePage basePage = new BasePage();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                caricaRaggruppamenti();

            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "chiudiLoader", script: "$('.loader').hide();", addScriptTags: true);
        }
        private void caricaRaggruppamenti()
        {
            List<Art_Gruppi> lista;
            Esito esito = new Esito();
            lista = Art_Gruppi_BLL.Instance.CaricaListaGruppi(ref esito);

            BasePage p = new BasePage();

            // CARICO LA LISTA RAGGRUPPAMENTI
            if (string.IsNullOrEmpty(esito.descrizione))
            {
                lbMod_Raggruppamenti.Items.Clear();
                foreach (Art_Gruppi raggruppamento in lista)
                {
                    ListItem item = new ListItem();
                    item.Text = raggruppamento.Nome;
                    item.Value = raggruppamento.Id.ToString();
                    lbMod_Raggruppamenti.Items.Add(item);
                }

                if (lbMod_Raggruppamenti.Items.Count > 5)
                {
                    lbMod_Raggruppamenti.Rows = 5;
                }
                else
                {
                    lbMod_Raggruppamenti.Rows = lbMod_Raggruppamenti.Items.Count;
                }
                
                // SE UTENTE ABILITATO ALLE MODIFICHE FACCIO VEDERE I PULSANTI DI MODIFICA
                abilitaBottoni(p.AbilitazioneInScrittura());

            }
            else
            {
                Session["ErrorPageText"] = esito.descrizione;
                string url = String.Format("~/pageError.aspx");
                Response.Redirect(url, true);
            }

        }

        private void abilitaBottoni(bool utenteAbilitatoInScrittura)
        {
            if (!utenteAbilitatoInScrittura)
            {
                btnInserisciRaggruppamento.Visible = false;
                btnSeleziona.Visible = false;
                btnAnnullaRaggruppamento.Visible = false;
            }
            else
            {
                btnInserisciRaggruppamento.Visible = true;
                btnSeleziona.Visible = true;
                btnAnnullaRaggruppamento.Visible = true;
            }
        }

        protected void btnConfermaInserimentoRaggruppamento_Click(object sendere, EventArgs e)
        {
            // INSERISCO TIPOLOGIA
            Esito esito = new Esito();
            Art_Gruppi gruppo = new Art_Gruppi();

            gruppo.Nome = tbInsNomeRaggruppamento.Text.Trim();
            gruppo.Descrizione = tbInsDescrizioneRaggruppamento.Text.Trim();

            gruppo.Parametri = "";
            gruppo.SottoTipo = "";

            gruppo.Attivo = true;

            if (esito.codice != Esito.ESITO_OK)
            {
                panelErrore.Style.Add("display", "block");
                lbl_MessaggioErrore.Text = "Controllare i campi evidenziati";
            }
            else
            {
                NascondiErroriValidazione();

                int iRet = Art_Gruppi_BLL.Instance.CreaGruppo(gruppo, ref esito);
                if (esito.codice != Esito.ESITO_OK)
                {
                    panelErrore.Style.Add("display", "block");
                    lbl_MessaggioErrore.Text = esito.descrizione;
                }
                else
                {
                    tbInsNomeRaggruppamento.Text = "";
                    tbInsDescrizioneRaggruppamento.Text = "";

                    caricaRaggruppamenti();
                }

            }
        }

        protected void btnEliminaRaggruppamento_Click(object sender, EventArgs e)
        {

            //ELIMINO IL RAGGRUPPAMENTO SELEZIONATO
            if (!string.IsNullOrEmpty(tbIdRaggruppamentoDaModificare.Text.Trim()))
            {
                try
                {
                    NascondiErroriValidazione();
                    Esito esito = Art_Gruppi_BLL.Instance.EliminaGruppo(Convert.ToInt32(tbIdRaggruppamentoDaModificare.Text.Trim()));

                    if (esito.codice != Esito.ESITO_OK)
                    {
                        panelErrore.Style.Add("display", "block");
                        lbl_MessaggioErrore.Text = esito.descrizione;
                    }
                    else
                    {
                        tbInsNomeRaggruppamento.Text = "";
                        tbInsDescrizioneRaggruppamento.Text = "";

                        btnModificaRaggruppamento.Visible =false;
                        btnInserisciRaggruppamento.Visible = true;
                        btnEliminaRaggruppamento.Visible = false;

                        caricaRaggruppamenti();
                    }
                }
                catch (Exception ex)
                {
                    log.Error("btnEliminaDocumento_Click", ex);
                    panelErrore.Style.Add("display", "block");
                    lbl_MessaggioErrore.Text = ex.Message;
                }
            }
            else
            {
                panelErrore.Style.Add("display", "block");
                lbl_MessaggioErrore.Text = "Verificare il corretto inserimento dei campi!";
            }
        }

        private void NascondiErroriValidazione()
        {
            panelErrore.Style.Add("display", "none");

            tbInsDescrizioneRaggruppamento.CssClass = tbInsDescrizioneRaggruppamento.CssClass.Replace("erroreValidazione", "");
            tbInsNomeRaggruppamento.CssClass = tbInsNomeRaggruppamento.CssClass.Replace("erroreValidazione", "");
        }

        protected void btnSeleziona_Click(object sender, EventArgs e)
        {
            //SCARICO IL RAGGRUPPAMENTO SELEZIONATO
            if (lbMod_Raggruppamenti.SelectedIndex >= 0)
            {
                try
                {
                    NascondiErroriValidazione();

                    string gruppoSelezionato = lbMod_Raggruppamenti.SelectedValue;
                    Esito esito = new Esito();
                    Art_Gruppi gruppo = Art_Gruppi_BLL.Instance.getGruppiById(Convert.ToInt32(gruppoSelezionato), ref esito);

                    if (esito.codice != Esito.ESITO_OK)
                    {
                        btnInserisciRaggruppamento.Visible = true;
                        panelErrore.Style.Remove("display");
                        lbl_MessaggioErrore.Text = esito.descrizione;
                    }
                    else
                    {
                        btnInserisciRaggruppamento.Visible = false;
                        btnModificaRaggruppamento.Visible = true;
                        btnEliminaRaggruppamento.Visible = true;
                        tbInsDescrizioneRaggruppamento.Text = gruppo.Descrizione;
                        tbInsNomeRaggruppamento.Text = gruppo.Nome;

                        pnlContainer.Visible = true;

                        tbIdRaggruppamentoDaModificare.Text = lbMod_Raggruppamenti.SelectedValue;
                    }
                }
                catch (Exception ex)
                {
                    log.Error("btnSeleziona_Click", ex);
                    btnInserisciRaggruppamento.Visible = true;
                    btnModificaRaggruppamento.Visible = false;
                    btnEliminaRaggruppamento.Visible = false;
                    panelErrore.Style.Add("display", "block");
                    lbl_MessaggioErrore.Text = ex.Message;
                }
            }

        }

        protected void btnModificaRaggruppamento_Click(object sender, EventArgs e)
        {
            //MODIFICO RAGGRUPPAMENTO
            if (!string.IsNullOrEmpty(tbInsNomeRaggruppamento.Text))
            {
                try
                {
                    NascondiErroriValidazione();

                    Esito esito = new Esito();
                    Art_Gruppi nuovoGruppo = new Art_Gruppi();
                    nuovoGruppo.Id = Convert.ToInt32(tbIdRaggruppamentoDaModificare.Text);
                    nuovoGruppo.Nome = tbInsNomeRaggruppamento.Text.Trim();
                    nuovoGruppo.Descrizione = tbInsDescrizioneRaggruppamento.Text.Trim();
                    nuovoGruppo.Parametri = "";
                    nuovoGruppo.SottoTipo = "";

                    nuovoGruppo.Attivo = true;
                    esito = Art_Gruppi_BLL.Instance.AggiornaGruppo(nuovoGruppo);

                    btnModificaRaggruppamento.Visible = false;
                    btnInserisciRaggruppamento.Visible = true;
                    btnEliminaRaggruppamento.Visible = false;
                    if (esito.codice != Esito.ESITO_OK)
                    {
                        panelErrore.Style.Remove("display");
                        lbl_MessaggioErrore.Text = esito.descrizione;
                    }
                    else
                    {
                        tbIdRaggruppamentoDaModificare.Text = "";
                        tbInsNomeRaggruppamento.Text = "";
                        tbInsDescrizioneRaggruppamento.Text = "";
                        caricaRaggruppamenti();
                    }
                }
                catch (Exception ex)
                {
                    log.Error("btnModificaTipologia_Click", ex);
                    btnModificaRaggruppamento.Visible = false;
                    btnInserisciRaggruppamento.Visible = true;
                    btnEliminaRaggruppamento.Visible = false;
                    panelErrore.Style.Remove("display");
                    lbl_MessaggioErrore.Text = ex.Message;
                }
            }
            else
            {
                panelErrore.Style.Remove("display");
                lbl_MessaggioErrore.Text = "Verificare il corretto inserimento dei campi!";
            }

        }

        protected void btnAnnullaRaggruppamento_Click(object sender, EventArgs e)
        {
            tbInsNomeRaggruppamento.Text = "";
            tbInsDescrizioneRaggruppamento.Text = "";

            tbIdRaggruppamentoDaModificare.Text = "";

            btnModificaRaggruppamento.Visible = false;
            btnInserisciRaggruppamento.Visible = true;
            btnEliminaRaggruppamento.Visible = false;
        }

        protected void btnEditRaggruppamento_Click(object sender, EventArgs e)
        {

        }

        protected void btnInsRaggruppamento_Click(object sender, EventArgs e)
        {

        }
    }
}