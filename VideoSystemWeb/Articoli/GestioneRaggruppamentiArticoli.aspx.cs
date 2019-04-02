using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoSystemWeb.BLL;
using VideoSystemWeb.Entity;
using VideoSystemWeb.DAL;
using System.Data;
namespace VideoSystemWeb.Articoli
{
    public partial class GestioneRaggruppamentiArticoli : BasePage
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

                caricaRaggruppamenti();

            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "chiudiLoader", script: "$('.loader').hide();", addScriptTags: true);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriTabGiusta", script: "openDettaglioRaggruppamento('" + hf_tabChiamata.Value + "');", addScriptTags: true);

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

                if (lbMod_Raggruppamenti.Items.Count > 10)
                {
                    lbMod_Raggruppamenti.Rows = 10;
                }
                else
                {
                    lbMod_Raggruppamenti.Rows = lbMod_Raggruppamenti.Items.Count;
                }

                //ARTICOLI
                ddlArticoliDaAggiungere.Items.Clear();
                List<Art_Articoli> listaArticoliMain = Art_Articoli_BLL.Instance.CaricaListaArticoli(ref esito, true);
                foreach (Art_Articoli articoloMain in listaArticoliMain)
                {
                    ListItem item = new ListItem();
                    item.Text = articoloMain.DefaultDescrizioneLunga + " - " + articoloMain.DefaultDescrizione;
                    item.Value = articoloMain.Id.ToString();
                    ddlArticoliDaAggiungere.Items.Add(item);
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
                //btnInserisciRaggruppamento.Visible = false;
                divBtnInserisciRaggruppamento.Visible = false;
                divSelRaggruppamento.Visible = false;
                //btnAnnullaRaggruppamento.Visible = false;
            }
            else
            {
                //btnInserisciRaggruppamento.Visible = true;
                divBtnInserisciRaggruppamento.Visible = true;
                divSelRaggruppamento.Visible = true;
                //btnAnnullaRaggruppamento.Visible = true;
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

                int iRet = Art_Gruppi_BLL.Instance.CreaGruppo(gruppo, ((Anag_Utenti)Session[SessionManager.UTENTE]), ref esito);
                if (esito.codice != Esito.ESITO_OK)
                {
                    //panelErrore.Style.Add("display", "block");
                    //lbl_MessaggioErrore.Text = esito.descrizione;
                    ShowError(esito.descrizione);
                }
                else
                {
                    tbInsNomeRaggruppamento.Text = "";
                    tbInsDescrizioneRaggruppamento.Text = "";
                    tbIdRaggruppamentoDaModificare.Text = iRet.ToString();
                    caricaRaggruppamenti();
                    ShowSuccess("Raggruppamento Correttamente Inserito!");
                    //btnEditRaggruppamento_Click(null, null);
                    pnlContainer.Visible = false;
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
                    Esito esito = Art_Gruppi_BLL.Instance.EliminaGruppo(Convert.ToInt32(tbIdRaggruppamentoDaModificare.Text.Trim()), ((Anag_Utenti)Session[SessionManager.UTENTE]));

                    if (esito.codice != Esito.ESITO_OK)
                    {
                        //panelErrore.Style.Add("display", "block");
                        //lbl_MessaggioErrore.Text = esito.descrizione;
                        ShowError(esito.descrizione);
                    }
                    else
                    {
                        tbInsNomeRaggruppamento.Text = "";
                        tbInsDescrizioneRaggruppamento.Text = "";
                        tbIdRaggruppamentoDaModificare.Text = "";

                        btnModificaRaggruppamento.Visible =false;
                        btnInserisciRaggruppamento.Visible = true;
                        btnEliminaRaggruppamento.Visible = false;

                        caricaRaggruppamenti();
                        ShowSuccess("Raggruppamento Correttamente Eliminato!");
                        pnlContainer.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    log.Error("btnEliminaDocumento_Click", ex);
                    //panelErrore.Style.Add("display", "block");
                    //lbl_MessaggioErrore.Text = ex.Message;
                    ShowError(ex.Message);
                }
            }
            else
            {
                //panelErrore.Style.Add("display", "block");
                //lbl_MessaggioErrore.Text = "Verificare il corretto inserimento dei campi!";
                ShowError("Verificare il corretto inserimento dei campi!");
            }
        }

        private void NascondiErroriValidazione()
        {
            panelErrore.Style.Add("display", "none");

            tbInsDescrizioneRaggruppamento.CssClass = tbInsDescrizioneRaggruppamento.CssClass.Replace("erroreValidazione", "");
            tbInsNomeRaggruppamento.CssClass = tbInsNomeRaggruppamento.CssClass.Replace("erroreValidazione", "");
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
                    esito = Art_Gruppi_BLL.Instance.AggiornaGruppo(nuovoGruppo, ((Anag_Utenti)Session[SessionManager.UTENTE]));

                    btnModificaRaggruppamento.Visible = false;
                    btnInserisciRaggruppamento.Visible = true;
                    btnEliminaRaggruppamento.Visible = false;
                    if (esito.codice != Esito.ESITO_OK)
                    {
                        //panelErrore.Style.Remove("display");
                        //lbl_MessaggioErrore.Text = esito.descrizione;
                        ShowError(esito.descrizione);
                    }
                    else
                    {
                        tbIdRaggruppamentoDaModificare.Text = "";
                        tbInsNomeRaggruppamento.Text = "";
                        tbInsDescrizioneRaggruppamento.Text = "";
                        caricaRaggruppamenti();
                        ShowSuccess("Raggruppamento Correttamente Modificato!");
                        //btnEditRaggruppamento_Click(null, null);
                        pnlContainer.Visible = false;

                    }
                }
                catch (Exception ex)
                {
                    log.Error("btnModificaTipologia_Click", ex);
                    btnModificaRaggruppamento.Visible = false;
                    btnInserisciRaggruppamento.Visible = true;
                    btnEliminaRaggruppamento.Visible = false;
                    //panelErrore.Style.Remove("display");
                    //lbl_MessaggioErrore.Text = ex.Message;
                    ShowError(ex.Message);
                }
            }
            else
            {
                //panelErrore.Style.Remove("display");
                //lbl_MessaggioErrore.Text = "Verificare il corretto inserimento dei campi!";
                ShowError("Verificare il corretto inserimento dei campi");
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

                        tbIdRaggruppamentoDaModificare.Text = lbMod_Raggruppamenti.SelectedValue;


                        // ARTICOLI ASSOCIATI
                        lbMod_Articoli.Items.Clear();
                        List<Art_Articoli> listaArticoli = Art_Gruppi_Articoli_BLL.Instance.getArticoliByIdGruppo(gruppo.Id, ref esito);
                        if (esito.codice == 0)
                        {
                            if (listaArticoli != null && listaArticoli.Count > 0)
                            {
                                lbMod_Articoli.Rows = listaArticoli.Count;
                                foreach (Art_Articoli articolo in listaArticoli)
                                {
                                    ListItem item = new ListItem();
                                    item.Text = articolo.DefaultDescrizioneLunga + " - " + articolo.DefaultDescrizione;
                                    item.Value = articolo.Id.ToString();
                                    lbMod_Articoli.Items.Add(item);
                                }
                                if(listaArticoli.Count > 10)
                                {
                                    lbMod_Articoli.Rows = 10;
                                }
                                else
                                {
                                    lbMod_Articoli.Rows = listaArticoli.Count;
                                }
                            }
                            else
                            {
                                lbMod_Articoli.Rows = 1;
                            }
                        }
                        else
                        {
                            Session["ErrorPageText"] = esito.descrizione;
                            string url = String.Format("~/pageError.aspx");
                            Response.Redirect(url, true);
                        }

                        pnlContainer.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    log.Error("btnSeleziona_Click", ex);
                    btnInserisciRaggruppamento.Visible = true;
                    btnModificaRaggruppamento.Visible = false;
                    btnEliminaRaggruppamento.Visible = false;
                    //panelErrore.Style.Add("display", "block");
                    //lbl_MessaggioErrore.Text = ex.Message;
                    ShowError(ex.Message);
                }
            }

        }

        protected void btnInsRaggruppamento_Click(object sender, EventArgs e)
        {
            // ARTICOLI ASSOCIATI
            tbIdRaggruppamentoDaModificare.Text = "";
            tbInsNomeRaggruppamento.Text = "";
            tbInsDescrizioneRaggruppamento.Text = "";
            lbMod_Articoli.Items.Clear();
            lbMod_Articoli.Rows = 1;
            pnlContainer.Visible = true;
        }

        protected void btnInserisciArticolo_Click(object sender, EventArgs e)
        {
            //INSERISCO L'ARTICOLO SE SELEZIONATO
            if (ddlArticoliDaAggiungere.SelectedIndex >= 0)
            {
                try
                {
                    NascondiErroriValidazione();
                    ListItem item = ddlArticoliDaAggiungere.Items[ddlArticoliDaAggiungere.SelectedIndex];
                    string value = item.Value;
                    string articoloSelezionato = item.Text;
                    Esito esito = new Esito();

                    Art_Gruppi_Articoli nuovoGruppoArticolo = new Art_Gruppi_Articoli();

                    nuovoGruppoArticolo.IdArtArticoli = Convert.ToInt16(item.Value.Trim());

                    nuovoGruppoArticolo.IdArtGruppi = Convert.ToInt32(tbIdRaggruppamentoDaModificare.Text.Trim());

                    int nArtDaInserire = 1;
                    if (!string.IsNullOrEmpty(tbQtaArticoliDaAggiungere.Text))
                    {
                        try
                        {
                            int x = Convert.ToInt16(tbQtaArticoliDaAggiungere.Text.Trim());
                            nArtDaInserire = x;
                        }
                        catch (Exception)
                        {
                        }
                        
                    }

                    for (int i = 0; i < nArtDaInserire; i++)
                    {
                        int iNuovoArtGruppo = Art_Gruppi_Articoli_BLL.Instance.CreaGruppoArticolo(nuovoGruppoArticolo, ((Anag_Utenti)Session[SessionManager.UTENTE]), ref esito);
                        if (esito.codice != Esito.ESITO_OK)
                        {
                            break;
                        }
                    }
                    

                    if (esito.codice != Esito.ESITO_OK)
                    {
                        //panelErrore.Style.Remove("display");
                        //panelErrore.Style.Add("display", "block");
                        //lbl_MessaggioErrore.Text = esito.descrizione;
                        ShowError(esito.descrizione);
                    }
                    else
                    {
                        btnEditRaggruppamento_Click(null, null);

                    }
                }
                catch (Exception ex)
                {
                    log.Error("btnInserisciArticolo_Click", ex);
                    //panelErrore.Style.Add("display", "block");
                    //lbl_MessaggioErrore.Text = ex.Message;
                    ShowError(ex.Message);
                }
            }
            else
            {
                //panelErrore.Style.Add("display", "block");
                //lbl_MessaggioErrore.Text = "Verificare il corretto inserimento dei campi!";
                ShowError("Verificare il corretto inserimento dei campi!");
            }
        }

        protected void btnEliminaArticolo_Click(object sender, EventArgs e)
        {
            //ELIMINO L'ARTICOLO DAL GRUPPO SE SELEZIONATO
            if (lbMod_Articoli.SelectedIndex >= 0)
            {
                try
                {
                    NascondiErroriValidazione();
                    ListItem item = lbMod_Articoli.Items[lbMod_Articoli.SelectedIndex];
                    string value = item.Value;
                    string gruppoSelezionato = item.Text;

                    // DEVO TROVARE PRIMA IL GRUPPO ARTICOLO FORMATO DA ID GRUPPO E ID ARTICOLO

                    Esito esito = new Esito();
                    string query = "SELECT id FROM art_gruppi_articoli where idArtGruppi = " + tbIdRaggruppamentoDaModificare.Text.Trim() + " AND idArtArticoli = " + value;
                    DataTable dtGruppiArticoli = Base_DAL.getDatiBySql(query, ref esito);

                    if (dtGruppiArticoli == null || dtGruppiArticoli.Rows == null)
                    {
                        esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                        esito.descrizione = "btnEliminaArticolo_Click - Nessun risultato restituito dalla query " + query;
                    }

                    if (esito.codice != Esito.ESITO_OK)
                    {
                        log.Error(esito.descrizione);
                        //panelErrore.Style.Remove("display");
                        //lbl_MessaggioErrore.Text = esito.descrizione;
                        ShowError(esito.descrizione);
                    }
                    else
                    {
                        foreach (DataRow riga in dtGruppiArticoli.Rows)
                        {
                            int idGruppoArticolo = Convert.ToInt16(riga["id"]);
                            esito = Art_Gruppi_Articoli_BLL.Instance.EliminaGruppoArticolo(idGruppoArticolo, ((Anag_Utenti)Session[SessionManager.UTENTE]));
                        }
                        if (esito.codice != Esito.ESITO_OK)
                        {
                            log.Error(esito.descrizione);
                            //panelErrore.Style.Remove("display");
                            //lbl_MessaggioErrore.Text = esito.descrizione;
                            ShowError(esito.descrizione);
                        }
                        else
                        {
                            btnEditRaggruppamento_Click(null, null);
                        }

                    }
                }
                catch (Exception ex)
                {
                    log.Error("btnEliminaArticolo_Click", ex);
                    //panelErrore.Style.Add("display", "block");
                    //lbl_MessaggioErrore.Text = ex.Message;
                    ShowError(ex.Message);
                }
            }
            else
            {
                //panelErrore.Style.Add("display", "block");
                //lbl_MessaggioErrore.Text = "Verificare il corretto inserimento dei campi!";
                ShowError("Verificare il corretto inserimento dei campi!");
            }

        }

        protected void btnApriArticoli_Click(object sender, EventArgs e)
        {
            if (phArticoli.Visible)
            {
                phArticoli.Visible = false;
            }
            else
            {
                phArticoli.Visible = true;
            }
        }
    }
}