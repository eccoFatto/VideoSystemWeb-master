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
namespace VideoSystemWeb.Anagrafiche.userControl
{
    public partial class AnagCollaboratori : System.Web.UI.UserControl
    {

        BasePage basePage = new BasePage();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (basePage.AbilitazioneInScrittura())
            {
                // ASSOCIO L'EVENTO DOUBLECLICK ALLE LISTBOX
                if (Request["__EVENTARGUMENT"] != null && Request["__EVENTARGUMENT"] == "move")
                {
                    lbMod_Email_DoubleClick();
                    lbMod_Indirizzi_DoubleClick();
                    lbMod_Telefoni_DoubleClick();
                }
                lbMod_Email.Attributes.Add("ondblclick", Page.ClientScript.GetPostBackEventReference(lbMod_Email, "move"));
                lbMod_Indirizzi.Attributes.Add("ondblclick", Page.ClientScript.GetPostBackEventReference(lbMod_Indirizzi, "move"));
                lbMod_Telefoni.Attributes.Add("ondblclick", Page.ClientScript.GetPostBackEventReference(lbMod_Telefoni, "move"));
            }

            if (!Page.IsPostBack)
            {

                BasePage p = new BasePage();
                Esito esito = p.caricaListeTipologiche();

                if (string.IsNullOrEmpty(esito.descrizione))
                {
                    ddlQualifiche.Items.Clear();
                    ddlQualifiche.Items.Add("");

                    ddlQualificheDaAggiungere.Items.Clear();
                    foreach (Tipologica qualifica in p.listaQualifiche)
                    {
                        ListItem item = new ListItem();
                        ListItem itemDaAggiungere = new ListItem();
                        item.Text = qualifica.nome;
                        itemDaAggiungere.Text = qualifica.nome;
                        // metto comunque il nome e non l'id perchè la ricerca sulla tabella anag_qualifiche_collaboratori la faccio sul nome
                        item.Value = qualifica.nome;
                        itemDaAggiungere.Value = qualifica.id.ToString();
                        ddlQualifiche.Items.Add(item);

                        ddlQualificheDaAggiungere.Items.Add(itemDaAggiungere);
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
            // SELEZIONO L'ULTIMA TAB SELEZIONATA
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriTabGiusta", script: "openDettaglioAnagrafica('" + hf_tabChiamata.Value + "')", addScriptTags: true);

        }
        // ABILITO I BOTTONI DI MODIFICA IN BASE ALLA TIPOLOGIA DI UTENTE
        private void abilitaBottoni(bool utenteAbilitatoInScrittura)
        {
            annullaModifiche();
            if (!utenteAbilitatoInScrittura)
            {
                btnInserisciCollaboratori.Visible = false;
                btnModifica.Visible = false;
                btnAnnulla.Visible = false;
                btnSalva.Visible = false;
                btnElimina.Visible = false;
                uploadButton.Visible = false;

                btnApriQualifiche.Visible = false;
                btnApriEmail.Visible = false;
                btnApriIndirizzi.Visible = false;
            }
            else
            {
                btnInserisciCollaboratori.Visible = true;
                btnModifica.Visible = true;
                btnAnnulla.Visible = false;
                btnSalva.Visible = false;
                btnElimina.Visible = false;
                uploadButton.Visible = true;

                btnApriQualifiche.Visible = true;
                btnApriEmail.Visible = true;
                btnApriIndirizzi.Visible = true;
            }
        }
        // RICERCA COLLABORATORI
        protected void btnRicercaCollaboratori_Click(object sender, EventArgs e)
        {

            string queryRicerca = ConfigurationManager.AppSettings["QUERY_SEARCH_COLLABORATORI"];

            queryRicerca = queryRicerca.Replace("@cognome", tbCognome.Text.Trim().Replace("'", "''"));
            //queryRicerca = queryRicerca.Replace("@nome", tbNome.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@codiceFiscale", tbCF.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@comuneRiferimento", tbCitta.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@partitaIva", TbPiva.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@nomeSocieta", TbSocieta.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@qualifica", ddlQualifiche.SelectedValue.ToString().Trim().Replace("'","''"));
            queryRicerca = queryRicerca.Replace("@nome", tbNome.Text.Trim().Replace("'", "''"));
            Esito esito = new Esito();
            DataTable dtCollaboratori = Base_DAL.getDatiBySql(queryRicerca,ref esito);
            gv_collaboratori.DataSource = dtCollaboratori;
            gv_collaboratori.DataBind();
        }

        protected void ddlQualifiche_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        // AGGIORNO LE RIGHE DEL DATAGRID CON LA FUNZIONE DI APERTURA DETTAGLIO
        protected void gv_collaboratori_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // PRENDO L'ID DEL COLLABORATORE SELEZIONATO

                string idCollaboratoreSelezionato = e.Row.Cells[0].Text;

                foreach (TableCell item in e.Row.Cells)
                {
                    item.Attributes["onclick"] = "mostraCollaboratore('" + idCollaboratoreSelezionato + "');";
                }

                
            }
        }
        // APRO POPUP DETTAGLIO COLLABORATORE
        protected void EditCollaboratore_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hf_idColl.Value) || (!string.IsNullOrEmpty((string)ViewState["idColl"]))) {
                if (!string.IsNullOrEmpty(hf_idColl.Value)) ViewState["idColl"] = hf_idColl.Value;
                editCollaboratore();
                AttivaDisattivaModificaAnagrafica(true);
                gestisciPulsantiAnagrafica("VISUALIZZAZIONE");
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriTabGiusta", script: "openDettaglioAnagrafica('Anagrafica')", addScriptTags: true);
                pnlContainer.Visible = true;
            }
        }
        // GESTIONE PULSANTI MODIFICA COLLABORATORE
        protected void btnModifica_Click(object sender, EventArgs e)
        {
            AttivaDisattivaModificaAnagrafica(false);
            gestisciPulsantiAnagrafica("MODIFICA");
        }
        // DETTAGLIO GESTIONE PULSANTI ANAGRAFICA
        private void gestisciPulsantiAnagrafica(string stato)
        {
            switch (stato)
            {
                case "VISUALIZZAZIONE":

                    btnModifica.Visible = basePage.AbilitazioneInScrittura();
                    btnSalva.Visible = false;
                    btnAnnulla.Visible = false;
                    btnElimina.Visible = false;
                    btnConfermaInserimento.Visible = false;
                    break;
                case "INSERIMENTO":
                    btnModifica.Visible = false;
                    btnSalva.Visible = false;
                    btnAnnulla.Visible = false;
                    btnElimina.Visible = false;
                    btnConfermaInserimento.Visible = true;
                    break;
                case "MODIFICA":
                    btnModifica.Visible = false;
                    btnSalva.Visible = true;
                    btnAnnulla.Visible = true;
                    btnElimina.Visible = true;
                    btnConfermaInserimento.Visible = false;
                    break;
                case "ANNULLAMENTO":
                    btnModifica.Visible = true;
                    btnSalva.Visible = false;
                    btnAnnulla.Visible = false;
                    btnElimina.Visible = false;
                    btnConfermaInserimento.Visible = false;
                    break;
                default:
                    btnModifica.Visible = true;
                    btnSalva.Visible = false;
                    btnAnnulla.Visible = false;
                    btnElimina.Visible = false;
                    btnConfermaInserimento.Visible = false;
                    break;
            }

        }

        protected void btnSalva_Click(object sender, EventArgs e)
        {
            // SALVO MODIFICHE COLLABORATORE
            Esito esito = new Esito();
            Anag_Collaboratori collaboratore = CreaOggettoSalvataggio(ref esito);

            if (string.IsNullOrEmpty(Path.GetFileName(imgCollaboratore.ImageUrl))) {
                collaboratore.PathFoto = ConfigurationManager.AppSettings["IMMAGINE_DUMMY_COLLABORATORE"];
            }
            else { 
                collaboratore.PathFoto = Path.GetFileName(imgCollaboratore.ImageUrl);
            }

            if (esito.codice != Esito.ESITO_OK)
            {
                panelErrore.Style.Remove("display");
                lbl_MessaggioErrore.Text = "Controllare i campi evidenziati";
            }
            else
            {
                NascondiErroriValidazione();

                esito = Anag_Collaboratori_BLL.Instance.AggiornaCollaboratore(collaboratore);

                
                if (esito.codice != Esito.ESITO_OK)
                {
                    panelErrore.Style.Remove("display");
                    lbl_MessaggioErrore.Text = esito.descrizione;
                }
                EditCollaboratore_Click(null, null);
            }
        }

        protected void btnAnnulla_Click(object sender, EventArgs e)
        {
            annullaModifiche();
        }

        private void annullaModifiche()
        {
            NascondiErroriValidazione();
            AttivaDisattivaModificaAnagrafica(true);
            editCollaboratore();
            gestisciPulsantiAnagrafica("ANNULLAMENTO");
        }
        
        protected void btn_chiudi_Click(object sender, EventArgs e)
        {
            //abilitaBottoni(basePage.AbilitazioneInScrittura());
            pnlContainer.Visible = false;
        }

        protected void InserisciCollaboratori_Click(object sender, EventArgs e)
        {
            ViewState["idColl"] = "";
            editCollaboratoreVuoto();
            AttivaDisattivaModificaAnagrafica(false);
            gestisciPulsantiAnagrafica("INSERIMENTO");
            pnlContainer.Visible = true;
        }

        private void pulisciCampiDettaglio()
        {
            tbMod_CF.Text = "";
            tbMod_Cognome.Text = "";
            tbMod_Nome.Text = "";
            tbMod_Nazione.Text = "";
            tbMod_ComuneNascita.Text = "";
            tbMod_ProvinciaNascita.Text = "";
            tbMod_ComuneRiferimento.Text = "";
            tbMod_DataNascita.Text = "";
            tbMod_NomeSocieta.Text = "";
            tbMod_PartitaIva.Text = "";
            cbMod_Assunto.Checked = false;
            cbMod_Attivo.Checked = false;
            tbMod_Note.Text = "";

            lbMod_Qualifiche.Items.Clear();
            lbMod_Qualifiche.Rows = 1;

            lbMod_Indirizzi.Items.Clear();
            lbMod_Indirizzi.Rows = 1;

            lbMod_Email.Items.Clear();
            lbMod_Email.Rows = 1;

            lbMod_Telefoni.Items.Clear();
            lbMod_Telefoni.Rows = 1;

        }
        // CARICA IMMAGINE COLLABORATORE
        protected void CaricaImmagine(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ViewState["idColl"].ToString()) && !ViewState["idColl"].ToString().Equals("0")) {
                // SE L'UTENTE E' IN MODIFICA CARICO L'IMMAGINE
                if (fuImg.HasFile)
                {
                    if (".JPEG|.JPG|.BMP|.PNG|.GIF".IndexOf(Path.GetExtension(fuImg.FileName).ToUpper()) >= 0)
                    {
                        try
                        {
                            string nomefileNew = DateTime.Now.Ticks.ToString() + Path.GetExtension(fuImg.FileName);
                            string fullPath = Server.MapPath(ConfigurationManager.AppSettings["PATH_IMMAGINI_COLLABORATORI"]) + Path.GetFileName(nomefileNew);
                            fuImg.SaveAs(fullPath);
                            imgCollaboratore.ImageUrl = fullPath;
                            lblImage.Text = nomefileNew;

                            string queryUpdateImg = "UPDATE ANAG_COLLABORATORI SET pathFoto = '" + nomefileNew + "' WHERE ID = " + ViewState["idColl"].ToString();

                            Esito esito = new Esito();
                            int iRighe = Base_DAL.executeUpdateBySql(queryUpdateImg, ref esito);
                            EditCollaboratore_Click(null, null);
                        }
                        catch (Exception ex)
                        {
                            lblImage.Text = ex.Message;
                        }
                    }
                    else
                    {
                        lblImage.Text = "Caricare un file di tipo immagine";
                    }
                }
                else
                {
                    lblImage.Text = "Caricare un'immagine";
                }
            }
        }

        private void AttivaDisattivaModificaAnagrafica(bool attivaModifica)
        {
            tbMod_CF.ReadOnly = attivaModifica;
            tbMod_Cognome.ReadOnly = attivaModifica;
            tbMod_ComuneNascita.ReadOnly = attivaModifica;
            tbMod_ComuneRiferimento.ReadOnly = attivaModifica;
            tbMod_DataNascita.ReadOnly = attivaModifica;
            tbMod_Nazione.ReadOnly = attivaModifica;
            tbMod_Nome.ReadOnly = attivaModifica;
            tbMod_NomeSocieta.ReadOnly = attivaModifica;
            tbMod_Note.ReadOnly = attivaModifica;
            tbMod_PartitaIva.ReadOnly = attivaModifica;
            tbMod_ProvinciaNascita.ReadOnly = attivaModifica;
            cbMod_Assunto.Enabled = !attivaModifica;
            cbMod_Attivo.Enabled = !attivaModifica;



        }

        private void editCollaboratore()
        {
            string idCollaboratore = (string) ViewState["idColl"];
            Esito esito = new Esito();

            if (!string.IsNullOrEmpty(idCollaboratore))
            {
                Entity.Anag_Collaboratori collaboratore = Anag_Collaboratori_DAL.Instance.getCollaboratoreById(Convert.ToInt16(idCollaboratore), ref esito);
                if (esito.codice == 0)
                {
                    pulisciCampiDettaglio();

                    // RIEMPIO I CAMPI DEL DETTAGLIO COLLABORATORE
                    tbMod_CF.Text = collaboratore.CodiceFiscale;
                    tbMod_Cognome.Text = collaboratore.Cognome;
                    tbMod_Nome.Text = collaboratore.Nome;
                    tbMod_Nazione.Text = collaboratore.Nazione;
                    tbMod_ComuneNascita.Text = collaboratore.ComuneNascita;
                    tbMod_ProvinciaNascita.Text = collaboratore.ProvinciaNascita;
                    tbMod_ComuneRiferimento.Text = collaboratore.ComuneRiferimento;
                    tbMod_DataNascita.Text = collaboratore.DataNascita.ToShortDateString();
                    tbMod_NomeSocieta.Text = collaboratore.NomeSocieta;
                    tbMod_PartitaIva.Text = collaboratore.PartitaIva;
                    cbMod_Assunto.Checked = collaboratore.Assunto;
                    cbMod_Attivo.Checked = collaboratore.Attivo;
                    tbMod_Note.Text = collaboratore.Note;

                    // QUALIFICHE
                    if (collaboratore.Qualifiche != null) { 
                        foreach (Anag_Qualifiche_Collaboratori qualifica in collaboratore.Qualifiche)
                        {
                            ListItem itemQualifica = new ListItem(qualifica.Qualifica, qualifica.Id.ToString());
                            lbMod_Qualifiche.Items.Add(itemQualifica);
                        }
                    }
                    if (collaboratore.Qualifiche != null && collaboratore.Qualifiche.Count > 0)
                    {
                        lbMod_Qualifiche.Rows = collaboratore.Qualifiche.Count;
                    }
                    else
                    {
                        lbMod_Qualifiche.Rows = 1;
                    }

                    // INDIRIZZI
                    if (collaboratore.Indirizzi != null) { 
                    foreach (Anag_Indirizzi_Collaboratori indirizzo in collaboratore.Indirizzi)
                    {
                        ListItem itemIndirizzi = new ListItem(indirizzo.Descrizione + " - " + indirizzo.Tipo + " " + indirizzo.Indirizzo + " " + indirizzo.NumeroCivico + " " + indirizzo.Cap + " " + indirizzo.Comune + " " + indirizzo.Provincia, indirizzo.Id.ToString());
                        lbMod_Indirizzi.Items.Add(itemIndirizzi);
                    }
                    }
                    if (collaboratore.Indirizzi != null && collaboratore.Indirizzi.Count > 0)
                    {
                        lbMod_Indirizzi.Rows = collaboratore.Indirizzi.Count;
                    }
                    else
                    {
                        lbMod_Indirizzi.Rows = 1;
                    }

                    // EMAIL
                    if (collaboratore.Email != null) { 
                        foreach (Anag_Email_Collaboratori email in collaboratore.Email)
                        {
                            ListItem itemEmail = new ListItem(email.Descrizione + " - " + email.IndirizzoEmail, email.Id.ToString());
                            lbMod_Email.Items.Add(itemEmail);
                        }
                    }
                    if (collaboratore.Email != null && collaboratore.Email.Count > 0)
                    {
                        lbMod_Email.Rows = collaboratore.Email.Count;
                    }
                    else
                    {
                        lbMod_Email.Rows = 1;
                    }

                    // TELEFONI
                    if (collaboratore.Telefoni != null) { 
                        foreach (Anag_Telefoni_Collaboratori telefono in collaboratore.Telefoni)
                        {
                            ListItem itemTelefono = new ListItem(telefono.Descrizione + " - " + telefono.Tipo + " - " + telefono.Pref_int + telefono.Pref_naz + telefono.Numero + " - whatsapp: " + Convert.ToString(telefono.Whatsapp), telefono.Id.ToString());
                            lbMod_Telefoni.Items.Add(itemTelefono);
                        }
                    }
                    if (collaboratore.Telefoni != null && collaboratore.Telefoni.Count > 0)
                    {
                        lbMod_Telefoni.Rows = collaboratore.Telefoni.Count;
                    }
                    else
                    {
                        lbMod_Telefoni.Rows = 1;
                    }

                    // IMMAGINE COLLABORATORE
                    if (string.IsNullOrEmpty(collaboratore.PathFoto))
                    {
                        imgCollaboratore.ImageUrl = ConfigurationManager.AppSettings["PATH_IMMAGINI_COLLABORATORI"] + ConfigurationManager.AppSettings["IMMAGINE_DUMMY_COLLABORATORE"];
                    }
                    else
                    {
                        imgCollaboratore.ImageUrl = ConfigurationManager.AppSettings["PATH_IMMAGINI_COLLABORATORI"] + collaboratore.PathFoto;
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

        private Anag_Collaboratori CreaOggettoSalvataggio(ref Esito esito)
        {
            Anag_Collaboratori collaboratore = new Anag_Collaboratori();

            if (string.IsNullOrEmpty((string)ViewState["idColl"])) {
                ViewState["idColl"] = 0;
            }

            collaboratore.Id = Convert.ToInt16(ViewState["idColl"].ToString());

            collaboratore.Nazione = BasePage.validaCampo(tbMod_Nazione, "", false, ref esito);
            collaboratore.Nome = BasePage.validaCampo(tbMod_Nome, "", false, ref esito);
            collaboratore.Cognome = BasePage.validaCampo(tbMod_Cognome, "", true, ref esito);
            collaboratore.ComuneNascita = BasePage.validaCampo(tbMod_ComuneNascita, "", false, ref esito);
            collaboratore.ComuneRiferimento = BasePage.validaCampo(tbMod_ComuneRiferimento, "", false, ref esito);
            collaboratore.DataNascita = BasePage.validaCampo(tbMod_DataNascita, DateTime.Now, true, ref esito);
            collaboratore.CodiceFiscale = BasePage.validaCampo(tbMod_CF, "", true, ref esito);
            collaboratore.Assunto = Convert.ToBoolean(BasePage.validaCampo(cbMod_Assunto, "false", false, ref esito));
            collaboratore.Attivo = Convert.ToBoolean(BasePage.validaCampo(cbMod_Attivo, "true", false, ref esito));
            collaboratore.NomeSocieta = BasePage.validaCampo(tbMod_NomeSocieta, "", false, ref esito);
            collaboratore.Note = BasePage.validaCampo(tbMod_Note, "", false, ref esito);
            collaboratore.PartitaIva = BasePage.validaCampo(tbMod_PartitaIva, "", false, ref esito);
            collaboratore.ProvinciaNascita = BasePage.validaCampo(tbMod_ProvinciaNascita, "", false, ref esito);

            return collaboratore;
        }

        private void editCollaboratoreVuoto()
        {
            Esito esito = new Esito();
            pulisciCampiDettaglio();
            // IMMAGINE COLLABORATORE
            imgCollaboratore.ImageUrl = ConfigurationManager.AppSettings["PATH_IMMAGINI_COLLABORATORI"] + ConfigurationManager.AppSettings["IMMAGINE_DUMMY_COLLABORATORE"];
            
        }

        protected void btnElimina_Click(object sender, EventArgs e)
        {
            Esito esito = new Esito();

            if (!string.IsNullOrEmpty((string)ViewState["idColl"]))
            {
                esito = Anag_Collaboratori_BLL.Instance.EliminaCollaboratore(Convert.ToInt32(ViewState["idColl"].ToString()));
                if (esito.codice != Esito.ESITO_OK)
                {
                    panelErrore.Style.Remove("display");
                    lbl_MessaggioErrore.Text = esito.descrizione;
                    AttivaDisattivaModificaAnagrafica(true);
                }
                else
                {
                    AttivaDisattivaModificaAnagrafica(true);
                    btn_chiudi_Click(null, null);
                    btnRicercaCollaboratori_Click(null, null);
                }

            }
        }

        private void NascondiErroriValidazione()
        {
            panelErrore.Style.Add("display", "none");

            tbMod_CF.CssClass = tbMod_CF.CssClass.Replace("erroreValidazione", "");
            tbMod_Cognome.CssClass = tbMod_Cognome.CssClass.Replace("erroreValidazione", "");
            tbMod_ComuneNascita.CssClass = tbMod_ComuneNascita.CssClass.Replace("erroreValidazione", "");
            tbMod_ComuneRiferimento.CssClass = tbMod_ComuneRiferimento.CssClass.Replace("erroreValidazione", "");
            tbMod_DataNascita.CssClass = tbMod_DataNascita.CssClass.Replace("erroreValidazione", "");
            tbMod_Nazione.CssClass = tbMod_Nazione.CssClass.Replace("erroreValidazione", "");
            tbMod_Nome.CssClass = tbMod_Nome.CssClass.Replace("erroreValidazione", "");
            tbMod_NomeSocieta.CssClass = tbMod_NomeSocieta.CssClass.Replace("erroreValidazione", "");
            tbMod_Note.CssClass = tbMod_Note.CssClass.Replace("erroreValidazione", "");
            tbMod_PartitaIva.CssClass = tbMod_PartitaIva.CssClass.Replace("erroreValidazione", "");
            tbMod_ProvinciaNascita.CssClass = tbMod_ProvinciaNascita.CssClass.Replace("erroreValidazione", "");
        }

        protected void btnConfermaInserimento_Click(object sender, EventArgs e)
        {
            // INSERISCO COLLABORATORE
            Esito esito = new Esito();
            Anag_Collaboratori collaboratore = CreaOggettoSalvataggio(ref esito);

            collaboratore.PathFoto = ConfigurationManager.AppSettings["IMMAGINE_DUMMY_COLLABORATORE"];

            if (esito.codice != Esito.ESITO_OK)
            {
                panelErrore.Style.Remove("display");
                lbl_MessaggioErrore.Text = "Controllare i campi evidenziati";
            }
            else
            {
                NascondiErroriValidazione();

                int iRet = Anag_Collaboratori_BLL.Instance.CreaCollaboratore(collaboratore, ref esito);
                if (iRet > 0)
                {
                    // UNA VOLTA INSERITO CORRETTAMENTE PUO' ESSERE MODIFICATO
                    hf_idColl.Value = iRet.ToString();
                    ViewState["idColl"] = hf_idColl.Value;
                    hf_tipoOperazione.Value = "VISUALIZZAZIONE";
                }

                if (esito.codice != Esito.ESITO_OK)
                {
                    panelErrore.Style.Remove("display");
                    lbl_MessaggioErrore.Text = esito.descrizione;
                }
                EditCollaboratore_Click(null, null);
            }

        }

        protected void btnEliminaQualifica_Click(object sender, EventArgs e)
        {
            //ELIMINO LA QUALIFICA SE SELEZIONATA
            if (lbMod_Qualifiche.SelectedIndex >= 0)
            {
                try
                {
                    NascondiErroriValidazione();
                    ListItem item = lbMod_Qualifiche.Items[lbMod_Qualifiche.SelectedIndex];
                    string value = item.Value;
                    string qualificaSelezionata = item.Text;
                    Esito esito = Anag_Qualifiche_Collaboratori_BLL.Instance.EliminaQualificaCollaboratore(Convert.ToInt32(item.Value));

                    if (esito.codice != Esito.ESITO_OK)
                    {
                        panelErrore.Style.Remove("display");
                        lbl_MessaggioErrore.Text = esito.descrizione;
                    }
                    else
                    {
                        //lbMod_Qualifiche.Items.Remove(item);
                        editCollaboratore();
                    }
                }
                catch (Exception ex)
                {
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

        protected void btnEliminaEmail_Click(object sender, EventArgs e)
        {
            //ELIMINO L'INDIRIZZO E-MAIL SE SELEZIONATO
            if (lbMod_Email.SelectedIndex >= 0)
            {
                try
                {
                    NascondiErroriValidazione();
                    ListItem item = lbMod_Email.Items[lbMod_Email.SelectedIndex];
                    string value = item.Value;
                    string emailSelezionata = item.Text;
                    Esito esito = Anag_Email_Collaboratori_BLL.Instance.EliminaEmailCollaboratore(Convert.ToInt32(item.Value));

                    if (esito.codice != Esito.ESITO_OK)
                    {
                        panelErrore.Style.Remove("display");
                        lbl_MessaggioErrore.Text = esito.descrizione;
                    }
                    else
                    {
                        tbInsEmail.Text = "";
                        tbInsPrioritaEmail.Text = "1";
                        tbInsTipoEmail.Text = "";

                        editCollaboratore();
                    }
                }
                catch (Exception ex)
                {
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

        protected void btnEliminaIndirizzo_Click(object sender, EventArgs e)
        {
            //ELIMINO L'INDIRIZZO SE SELEZIONATO
            if (lbMod_Indirizzi.SelectedIndex >= 0)
            {
                try
                {
                    NascondiErroriValidazione();
                    ListItem item = lbMod_Indirizzi.Items[lbMod_Indirizzi.SelectedIndex];
                    string value = item.Value;
                    string indirizzoSelezionato = item.Text;
                    Esito esito = Anag_Indirizzi_Collaboratori_BLL.Instance.EliminaIndirizziCollaboratore(Convert.ToInt32(item.Value));

                    if (esito.codice != Esito.ESITO_OK)
                    {
                        panelErrore.Style.Remove("display");
                        lbl_MessaggioErrore.Text = esito.descrizione;
                    }
                    else
                    {
                        tbInsIndirizzoIndirizzo.Text = "";
                        tbInsPrioritaIndirizzo.Text = "1";
                        //tbInsTipoIndirizzo.Text = "";
                        cmbInsTipoIndirizzo.Text = "";
                        tbInsComuneIndirizzo.Text = "";
                        tbInsProvinciaIndirizzo.Text = "";
                        tbInsCapIndirizzo.Text = "";
                        tbInsCivicoIndirizzo.Text = "";
                        tbInsDescrizioneIndirizzo.Text = "";
                        btnModificaIndirizzo.Visible = false;
                        btnInserisciIndirizzo.Visible = true;

                        editCollaboratore();
                    }
                }
                catch (Exception ex)
                {
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

        protected void btnEliminaTelefono_Click(object sender, EventArgs e) {
            //ELIMINO IL TELEFONO SE SELEZIONATO
            if (lbMod_Telefoni.SelectedIndex >= 0)
            {
                try
                {
                    NascondiErroriValidazione();
                    ListItem item = lbMod_Telefoni.Items[lbMod_Telefoni.SelectedIndex];
                    string value = item.Value;
                    string telefonoSelezionato = item.Text;
                    Esito esito = Anag_Telefoni_Collaboratori_BLL.Instance.EliminaTelefonoCollaboratore(Convert.ToInt32(item.Value));

                    if (esito.codice != Esito.ESITO_OK)
                    {
                        panelErrore.Style.Remove("display");
                        lbl_MessaggioErrore.Text = esito.descrizione;
                    }
                    else
                    {
                        tbInsPrefIntTelefono.Text = "+39";
                        tbInsPrefNazTelefono.Text = "";
                        tbInsNumeroTelefono.Text = "";
                        cmbInsTipoTelefono.Text = "";
                        cbInsWhatsappTelefono.Checked = false;

                        btnModificaTelefono.Visible = false;
                        btnInserisciTelefono.Visible = true;

                        editCollaboratore();
                    }
                }
                catch (Exception ex)
                {
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

        protected void lbMod_Email_DoubleClick()
        {
            //SCARICO L'INDIRIZZO E-MAIL SE DOPPIO CLICK
            if (lbMod_Email.SelectedIndex >= 0)
            {
                try
                {
                    NascondiErroriValidazione();
                    ListItem item = lbMod_Email.Items[lbMod_Email.SelectedIndex];
                    string value = item.Value;
                    string emailSelezionata = item.Text;
                    Esito esito = new Esito();
                    Anag_Email_Collaboratori email = Anag_Email_Collaboratori_BLL.Instance.getEmailById(Convert.ToInt32(item.Value),ref esito);

                    if (esito.codice != Esito.ESITO_OK)
                    {
                        btnModificaEmail.Visible = false;
                        btnInserisciEmail.Visible = true;
                        tbIdEmailDaModificare.Text = "";
                        panelErrore.Style.Remove("display");
                        lbl_MessaggioErrore.Text = esito.descrizione;
                    }
                    else
                    {
                        btnModificaEmail.Visible = true;
                        btnInserisciEmail.Visible = false;
                        tbInsEmail.Text = email.IndirizzoEmail;
                        tbInsPrioritaEmail.Text = email.Priorita.ToString();
                        tbInsTipoEmail.Text = email.Descrizione;
                        tbIdEmailDaModificare.Text = email.Id.ToString();
                    }
                }
                catch (Exception ex)
                {
                    btnModificaEmail.Visible = false;
                    btnInserisciEmail.Visible = true;
                    tbIdEmailDaModificare.Text = "";
                    panelErrore.Style.Remove("display");
                    lbl_MessaggioErrore.Text = ex.Message;
                }
            }

        }

        protected void lbMod_Indirizzi_DoubleClick()
        {
            //SCARICO L'INDIRIZZO SE DOPPIO CLICK
            if (lbMod_Indirizzi.SelectedIndex >= 0)
            {
                try
                {
                    NascondiErroriValidazione();
                    ListItem item = lbMod_Indirizzi.Items[lbMod_Indirizzi.SelectedIndex];
                    string value = item.Value;
                    string indirizzoSelezionato = item.Text;
                    Esito esito = new Esito();
                    Anag_Indirizzi_Collaboratori indirizzo = Anag_Indirizzi_Collaboratori_BLL.Instance.getIndirizzoById(ref esito,Convert.ToInt32(item.Value));

                    if (esito.codice != Esito.ESITO_OK)
                    {
                        btnModificaIndirizzo.Visible = false;
                        btnInserisciIndirizzo.Visible = true;
                        tbIdIndirizzoDaModificare.Text = "";
                        panelErrore.Style.Remove("display");
                        lbl_MessaggioErrore.Text = esito.descrizione;
                    }
                    else
                    {
                        btnModificaIndirizzo.Visible = true;
                        btnInserisciIndirizzo.Visible = false;
                        //tbInsTipoIndirizzo.Text = indirizzo.Tipo;

                        ListItem trovati = cmbInsTipoIndirizzo.Items.FindByText(indirizzo.Tipo);
                        if (trovati != null) { 
                            cmbInsTipoIndirizzo.SelectedValue = trovati.Value;
                        }
                        else
                        {
                            cmbInsTipoIndirizzo.Text = "";
                        }

                        tbInsIndirizzoIndirizzo.Text = indirizzo.Indirizzo;
                        tbInsCapIndirizzo.Text = indirizzo.Cap;
                        tbInsCivicoIndirizzo.Text = indirizzo.NumeroCivico;
                        tbInsComuneIndirizzo.Text = indirizzo.Comune;
                        tbInsProvinciaIndirizzo.Text = indirizzo.Provincia;
                        tbInsPrioritaIndirizzo.Text = indirizzo.Priorita.ToString();
                        tbInsDescrizioneIndirizzo.Text = indirizzo.Descrizione;
                        tbIdIndirizzoDaModificare.Text = indirizzo.Id.ToString();
                    }
                }
                catch (Exception ex)
                {
                    btnModificaIndirizzo.Visible = false;
                    btnInserisciIndirizzo.Visible = true;
                    tbIdIndirizzoDaModificare.Text = "";
                    panelErrore.Style.Remove("display");
                    lbl_MessaggioErrore.Text = ex.Message;
                }
            }

        }
               
        protected void lbMod_Telefoni_DoubleClick()
        {
            //SCARICO IL TELEFONO SE DOPPIO CLICK
            if (lbMod_Telefoni.SelectedIndex >= 0)
            {
                try
                {
                    NascondiErroriValidazione();
                    ListItem item = lbMod_Telefoni.Items[lbMod_Telefoni.SelectedIndex];
                    string value = item.Value;
                    string telefonoSelezionato = item.Text;
                    Esito esito = new Esito();
                    Anag_Telefoni_Collaboratori telefono = Anag_Telefoni_Collaboratori_BLL.Instance.getTelefonoById(ref esito, Convert.ToInt32(item.Value));

                    if (esito.codice != Esito.ESITO_OK)
                    {
                        btnModificaTelefono.Visible = false;
                        btnInserisciTelefono.Visible = true;
                        tbIdTelefonoDaModificare.Text = "";
                        panelErrore.Style.Remove("display");
                        lbl_MessaggioErrore.Text = esito.descrizione;
                    }
                    else
                    {
                        btnModificaTelefono.Visible = true;
                        btnInserisciTelefono.Visible = false;
                        tbIdTelefonoDaModificare.Text = telefono.Id.ToString();
                        ListItem trovati = cmbInsTipoTelefono.Items.FindByText(telefono.Tipo);
                        if (trovati != null)
                        {
                            cmbInsTipoTelefono.Text = telefono.Tipo;
                        }
                        else
                        {
                            cmbInsTipoTelefono.Text = "";
                        }
                        

                        tbInsPrefIntTelefono.Text = telefono.Pref_int;
                        tbInsPrefNazTelefono.Text = telefono.Pref_naz;
                        tbInsNumeroTelefono.Text = telefono.Numero;
                        tbInsPrioritaTelefono.Text = telefono.Priorita.ToString();
                        cbInsWhatsappTelefono.Checked = telefono.Whatsapp;
                        tbInsDescrizioneTelefono.Text = telefono.Descrizione;
                    }
                }
                catch (Exception ex)
                {
                    btnModificaTelefono.Visible = false;
                    btnInserisciTelefono.Visible = true;
                    tbIdTelefonoDaModificare.Text = "";
                    panelErrore.Style.Remove("display");
                    lbl_MessaggioErrore.Text = ex.Message;
                }
            }
        }

        protected void btnConfermaInserimentoQualifica_Click(object sender, EventArgs e)
        {
            //INSERISCO LA QUALIFICA SE SELEZIONATA
            if (ddlQualificheDaAggiungere.SelectedIndex >= 0)
            {
                try
                {
                    NascondiErroriValidazione();
                    ListItem item = ddlQualificheDaAggiungere.Items[ddlQualificheDaAggiungere.SelectedIndex];
                    string value = item.Value;
                    string qualificaSelezionata = item.Text;
                    Esito esito = new Esito();
                    Anag_Qualifiche_Collaboratori nuovaQualifica = new Anag_Qualifiche_Collaboratori();
                    nuovaQualifica.Id_collaboratore = Convert.ToInt32(ViewState["idColl"]);
                    nuovaQualifica.Priorita = Convert.ToInt16(tbInsPrioritaQualifica.Text.Trim());
                    nuovaQualifica.Qualifica = qualificaSelezionata;
                    nuovaQualifica.Attivo = true;
                    nuovaQualifica.Descrizione = "";
                    int iNuovaQualifica = Anag_Qualifiche_Collaboratori_BLL.Instance.CreaQualificaCollaboratore(nuovaQualifica, ref esito);

                    if (esito.codice != Esito.ESITO_OK)
                    {
                        panelErrore.Style.Remove("display");
                        lbl_MessaggioErrore.Text = esito.descrizione;
                    }
                    else
                    {
                        editCollaboratore();
                    }
                }
                catch (Exception ex)
                {
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

        protected void btnConfermaInserimentoEmail_Click(object sender, EventArgs e)
        {
            //INSERISCO L'E-MAIL
            if (!string.IsNullOrEmpty(tbInsEmail.Text) && validaIndirizzoEmail(tbInsEmail.Text))
            {
                try
                {
                    NascondiErroriValidazione();

                    Esito esito = new Esito();
                    Anag_Email_Collaboratori nuovaEmail = new Anag_Email_Collaboratori();
                    nuovaEmail.Id_collaboratore = Convert.ToInt32(ViewState["idColl"]);
                    nuovaEmail.Priorita = Convert.ToInt16(tbInsPrioritaEmail.Text.Trim());
                    nuovaEmail.IndirizzoEmail = tbInsEmail.Text.Trim();
                    nuovaEmail.Attivo = true;
                    nuovaEmail.Descrizione = tbInsTipoEmail.Text.Trim();
                    int iNuovaEmail = Anag_Email_Collaboratori_BLL.Instance.CreaEmailCollaboratore(nuovaEmail, ref esito);

                    if (esito.codice != Esito.ESITO_OK)
                    {
                        panelErrore.Style.Remove("display");
                        lbl_MessaggioErrore.Text = esito.descrizione;
                    }
                    else
                    {
                        tbInsEmail.Text = "";
                        tbInsPrioritaEmail.Text = "1";
                        tbInsTipoEmail.Text = "";
                        editCollaboratore();
                    }
                }
                catch (Exception ex)
                {
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

        protected void btnConfermaInserimentoIndirizzo_Click(object sender, EventArgs e)
        {
            //INSERISCO L'INDIRIZZO
            if (!string.IsNullOrEmpty(tbInsIndirizzoIndirizzo.Text))
            {
                try
                {
                    NascondiErroriValidazione();

                    Esito esito = new Esito();
                    Anag_Indirizzi_Collaboratori nuovoIndirizzo = new Anag_Indirizzi_Collaboratori();
                    nuovoIndirizzo.Id_Collaboratore = Convert.ToInt32(ViewState["idColl"]);
                    nuovoIndirizzo.Priorita = Convert.ToInt16(tbInsPrioritaIndirizzo.Text.Trim());
                    nuovoIndirizzo.Indirizzo = tbInsIndirizzoIndirizzo.Text.Trim();
                    nuovoIndirizzo.Attivo = true;
                    nuovoIndirizzo.Descrizione = tbInsDescrizioneIndirizzo.Text.Trim();
                    //nuovoIndirizzo.Tipo = tbInsTipoIndirizzo.Text.Trim();
                    nuovoIndirizzo.Tipo = cmbInsTipoIndirizzo.Text.Trim();
                    nuovoIndirizzo.Comune = tbInsComuneIndirizzo.Text.Trim();
                    nuovoIndirizzo.Provincia = tbInsProvinciaIndirizzo.Text.Trim();
                    nuovoIndirizzo.Cap = tbInsCapIndirizzo.Text.Trim();
                    nuovoIndirizzo.NumeroCivico = tbInsCivicoIndirizzo.Text.Trim();
                    int iNuovoIndirizzo = Anag_Indirizzi_Collaboratori_BLL.Instance.CreaIndirizziCollaboratore(nuovoIndirizzo, ref esito);

                    if (esito.codice != Esito.ESITO_OK)
                    {
                        panelErrore.Style.Remove("display");
                        lbl_MessaggioErrore.Text = esito.descrizione;
                    }
                    else
                    {
                        tbInsIndirizzoIndirizzo.Text = "";
                        tbInsPrioritaIndirizzo.Text = "1";
                        tbInsDescrizioneIndirizzo.Text = "";
                        tbInsCapIndirizzo.Text = "";
                        tbInsCivicoIndirizzo.Text = "";
                        tbInsComuneIndirizzo.Text = "";
                        tbInsProvinciaIndirizzo.Text = "";
                        //tbInsTipoIndirizzo.Text = "";
                        cmbInsTipoIndirizzo.Text = "";
                        editCollaboratore();
                    }
                }
                catch (Exception ex)
                {
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

        protected void btnConfermaInserimentoTelefono_Click(object sender, EventArgs e) {
            //INSERISCO IL TELEFONO
            if (!string.IsNullOrEmpty(tbInsNumeroTelefono.Text) && !string.IsNullOrEmpty(tbInsPrefNazTelefono.Text))
            {
                try
                {
                    NascondiErroriValidazione();

                    Esito esito = new Esito();
                    Anag_Telefoni_Collaboratori nuovoTelefono = new Anag_Telefoni_Collaboratori();
                    nuovoTelefono.Id_collaboratore = Convert.ToInt32(ViewState["idColl"]);
                    nuovoTelefono.Priorita = Convert.ToInt16(tbInsPrioritaIndirizzo.Text.Trim());
                    nuovoTelefono.Attivo = true;
                    nuovoTelefono.Descrizione = tbInsDescrizioneTelefono.Text.Trim();
                    nuovoTelefono.Tipo = cmbInsTipoTelefono.Text.Trim();
                    nuovoTelefono.Pref_int = tbInsPrefIntTelefono.Text.Trim();
                    nuovoTelefono.Pref_naz = tbInsPrefNazTelefono.Text.Trim();
                    nuovoTelefono.Numero = tbInsNumeroTelefono.Text.Trim();
                    nuovoTelefono.Whatsapp = cbInsWhatsappTelefono.Checked;

                    int iNuovoTelefono = Anag_Telefoni_Collaboratori_BLL.Instance.CreaTelefonoCollaboratore(nuovoTelefono, ref esito);

                    if (esito.codice != Esito.ESITO_OK)
                    {
                        panelErrore.Style.Remove("display");
                        lbl_MessaggioErrore.Text = esito.descrizione;
                    }
                    else
                    {
                        tbInsPrioritaTelefono.Text = "";
                        tbInsDescrizioneTelefono.Text = "";
                        //tbInsTipoIndirizzo.Text = "";
                        cmbInsTipoIndirizzo.Text = "";
                        tbInsPrefIntTelefono.Text = "";
                        tbInsPrefNazTelefono.Text = "";
                        tbInsNumeroTelefono.Text = "";
                        cbInsWhatsappTelefono.Checked=false;
                        editCollaboratore();
                    }
                }
                catch (Exception ex)
                {
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

        protected void btnConfermaModificaEmail_Click(object sender, EventArgs e)
        {
            //MODIFICO L'E-MAIL
            if (!string.IsNullOrEmpty(tbInsEmail.Text) && validaIndirizzoEmail(tbInsEmail.Text) && !string.IsNullOrEmpty(tbIdEmailDaModificare.Text))
            {
                try
                {
                    NascondiErroriValidazione();

                    Esito esito = new Esito();
                    Anag_Email_Collaboratori nuovaEmail = new Anag_Email_Collaboratori();
                    nuovaEmail.Id = Convert.ToInt32(tbIdEmailDaModificare.Text);
                    nuovaEmail.Id_collaboratore = Convert.ToInt32(ViewState["idColl"]);
                    nuovaEmail.Priorita = Convert.ToInt16(tbInsPrioritaEmail.Text.Trim());
                    nuovaEmail.IndirizzoEmail = tbInsEmail.Text.Trim();
                    nuovaEmail.Attivo = true;
                    nuovaEmail.Descrizione = tbInsTipoEmail.Text.Trim();
                    esito = Anag_Email_Collaboratori_BLL.Instance.AggiornaEmailCollaboratore(nuovaEmail);

                    btnModificaEmail.Visible = false;
                    btnInserisciEmail.Visible = true;

                    if (esito.codice != Esito.ESITO_OK)
                    {
                        panelErrore.Style.Remove("display");
                        lbl_MessaggioErrore.Text = esito.descrizione;
                    }
                    else
                    {
                        tbInsEmail.Text = "";
                        tbInsPrioritaEmail.Text = "1";
                        tbInsTipoEmail.Text = "";
                        editCollaboratore();
                    }
                }
                catch (Exception ex)
                {
                    btnModificaEmail.Visible = false;
                    btnInserisciEmail.Visible = true;
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

        protected void btnConfermaModificaIndirizzo_Click(object sender, EventArgs e)
        {
            //MODIFICO L'INDIRIZZO
            if (!string.IsNullOrEmpty(tbInsIndirizzoIndirizzo.Text) && !string.IsNullOrEmpty(tbInsComuneIndirizzo.Text) && !string.IsNullOrEmpty(tbIdIndirizzoDaModificare.Text))
            {
                try
                {
                    NascondiErroriValidazione();

                    Esito esito = new Esito();
                    Anag_Indirizzi_Collaboratori nuovoIndirizzo = new Anag_Indirizzi_Collaboratori();
                    nuovoIndirizzo.Id = Convert.ToInt32(tbIdIndirizzoDaModificare.Text);
                    nuovoIndirizzo.Id_Collaboratore = Convert.ToInt32(ViewState["idColl"]);
                    nuovoIndirizzo.Priorita = Convert.ToInt16(tbInsPrioritaIndirizzo.Text.Trim());
                    nuovoIndirizzo.Indirizzo = tbInsIndirizzoIndirizzo.Text.Trim();
                    nuovoIndirizzo.Attivo = true;
                    nuovoIndirizzo.Descrizione = tbInsDescrizioneIndirizzo.Text.Trim();
                    //nuovoIndirizzo.Tipo = tbInsTipoIndirizzo.Text.Trim();
                    nuovoIndirizzo.Tipo = cmbInsTipoIndirizzo.Text.Trim();
                    nuovoIndirizzo.Nazione = "Italia";
                    nuovoIndirizzo.NumeroCivico = tbInsCivicoIndirizzo.Text.Trim();
                    nuovoIndirizzo.Cap = tbInsCapIndirizzo.Text.Trim();
                    nuovoIndirizzo.Comune = tbInsComuneIndirizzo.Text.Trim();
                    nuovoIndirizzo.Provincia = tbInsProvinciaIndirizzo.Text.Trim();

                    esito = Anag_Indirizzi_Collaboratori_BLL.Instance.AggiornaIndirizziCollaboratore(nuovoIndirizzo);

                    btnModificaIndirizzo.Visible = false;
                    btnInserisciIndirizzo.Visible = true;

                    if (esito.codice != Esito.ESITO_OK)
                    {
                        panelErrore.Style.Remove("display");
                        lbl_MessaggioErrore.Text = esito.descrizione;
                    }
                    else
                    {
                        tbIdIndirizzoDaModificare.Text = "";
                        tbInsIndirizzoIndirizzo.Text = "";
                        tbInsPrioritaIndirizzo.Text = "1";
                        //tbInsTipoIndirizzo.Text = "";
                        cmbInsTipoIndirizzo.Text = "";
                        tbInsDescrizioneIndirizzo.Text = "";
                        tbInsComuneIndirizzo.Text = "";
                        tbInsProvinciaIndirizzo.Text = "";
                        tbInsCapIndirizzo.Text = "";
                        tbInsCivicoIndirizzo.Text = "";
                        editCollaboratore();
                    }
                }
                catch (Exception ex)
                {
                    btnModificaIndirizzo.Visible = false;
                    btnInserisciIndirizzo.Visible = true;
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

        protected void btnConfermaModificaTelefono_Click(object sender, EventArgs e) {
            //MODIFICO L'INDIRIZZO
            if (!string.IsNullOrEmpty(tbInsNumeroTelefono.Text) && !string.IsNullOrEmpty(tbInsPrefNazTelefono.Text) && !string.IsNullOrEmpty(tbIdTelefonoDaModificare.Text))
            {
                try
                {
                    NascondiErroriValidazione();

                    Esito esito = new Esito();
                    Anag_Telefoni_Collaboratori nuovoTelefono = new Anag_Telefoni_Collaboratori();
                    nuovoTelefono.Id = Convert.ToInt32(tbIdTelefonoDaModificare.Text);
                    nuovoTelefono.Id_collaboratore = Convert.ToInt32(ViewState["idColl"]);
                    nuovoTelefono.Priorita = Convert.ToInt16(tbInsPrioritaTelefono.Text.Trim());
                    nuovoTelefono.Pref_int = tbInsPrefIntTelefono.Text.Trim();
                    nuovoTelefono.Pref_naz = tbInsPrefNazTelefono.Text.Trim();
                    nuovoTelefono.Tipo = cmbInsTipoTelefono.Text.Trim();
                    nuovoTelefono.Numero = tbInsNumeroTelefono.Text.Trim();
                    nuovoTelefono.Whatsapp = cbInsWhatsappTelefono.Checked;
                    nuovoTelefono.Attivo = true;
                    nuovoTelefono.Descrizione = tbInsDescrizioneTelefono.Text.Trim();

                    esito = Anag_Telefoni_Collaboratori_BLL.Instance.AggiornaTelefonoCollaboratore(nuovoTelefono);

                    btnModificaTelefono.Visible = false;
                    btnInserisciTelefono.Visible = true;

                    if (esito.codice != Esito.ESITO_OK)
                    {
                        panelErrore.Style.Remove("display");
                        lbl_MessaggioErrore.Text = esito.descrizione;
                    }
                    else
                    {
                        tbIdTelefonoDaModificare.Text = "";
                        tbInsNumeroTelefono.Text = "";
                        tbInsPrefIntTelefono.Text = "";
                        tbInsPrioritaTelefono.Text = "1";
                        tbInsPrefNazTelefono.Text = "";
                        cmbInsTipoTelefono.Text = "";
                        tbInsDescrizioneTelefono.Text = "";
                        cbInsWhatsappTelefono.Checked = false;
                        editCollaboratore();
                    }
                }
                catch (Exception ex)
                {
                    btnModificaTelefono.Visible = false;
                    btnInserisciTelefono.Visible = true;
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

        protected void btnAnnullaIndirizzo_Click(object sender, EventArgs e)
        {
            tbIdIndirizzoDaModificare.Text = "";
            tbInsIndirizzoIndirizzo.Text = "";
            tbInsPrioritaIndirizzo.Text = "1";
            cmbInsTipoIndirizzo.Text = "";
            tbInsDescrizioneIndirizzo.Text = "";
            tbInsComuneIndirizzo.Text = "";
            tbInsProvinciaIndirizzo.Text = "";
            tbInsCapIndirizzo.Text = "";
            tbInsCivicoIndirizzo.Text = "";

            btnModificaIndirizzo.Visible = false;
            btnInserisciIndirizzo.Visible = true;
        }

        protected void btnAnnullaTelefono_Click(object sender, EventArgs e)
        {
            tbInsPrefIntTelefono.Text = "+39";
            tbInsPrefNazTelefono.Text = "";
            tbInsNumeroTelefono.Text = "";
            cmbInsTipoTelefono.Text = "";
            cbInsWhatsappTelefono.Checked = false;

            btnModificaTelefono.Visible = false;
            btnInserisciTelefono.Visible = true;
        }

        protected void btnApriQualifiche_Click(object sender, EventArgs e)
        {
            if (phQualifiche.Visible)
            {
                phQualifiche.Visible = false;
            }
            else
            {
                phQualifiche.Visible = true;
                
            }
        }

        protected void btnApriEmail_Click(object sender, EventArgs e)
        {
            if (phEmail.Visible)
            {
                phEmail.Visible = false;
            }
            else
            {
                phEmail.Visible = true;
            }
        }

        protected void btnApriIndirizzi_Click(object sender, EventArgs e)
        {
            if (phIndirizzi.Visible)
            {
                phIndirizzi.Visible = false;
            }
            else
            {
                phIndirizzi.Visible = true;
            }
        }

        protected void btnApriTelefoni_Click(object sender, EventArgs e)
        {
            if (phTelefoni.Visible)
            {
                phTelefoni.Visible = false;
            }
            else
            {
                phTelefoni.Visible = true;
            }
        }

        private bool validaIndirizzoEmail(string indirizzo)
        {
            Regex regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            Match match = regex.Match(indirizzo);
            if (match.Success) { 
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}