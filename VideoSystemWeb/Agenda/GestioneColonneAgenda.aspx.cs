﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoSystemWeb.BLL;
using VideoSystemWeb.Entity;
namespace VideoSystemWeb.Agenda
{
    public partial class GestioneColonneAgenda : BasePage
    {
        BasePage basePage = new BasePage();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                lblColonneAgenda.ForeColor = System.Drawing.Color.Brown;
                caricaTipologia();

            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "chiudiLoader", script: "$('.loader').hide();", addScriptTags: true);
        }
        private void caricaTipologia()
        {
            List<Tipologica> lista;
            Esito esito = new Esito();
            lista = UtilityTipologiche.CaricaTipologica(EnumTipologiche.TIPO_COLONNE_AGENDA, true, ref esito);


            BasePage p = new BasePage();

            // CARICO LA COMBO
            if (string.IsNullOrEmpty(esito.descrizione))
            {
                lbMod_Tipologia.Items.Clear();
                foreach (Tipologica tipologia in lista)
                {
                    ListItem item = new ListItem();
                    item.Text = tipologia.nome;
                    item.Value = tipologia.id.ToString();
                    lbMod_Tipologia.Items.Add(item);
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
                btnInserisciTipologia.Visible = false;
                btnSeleziona.Visible = false;
                btnAnnullaTipologia.Visible = false;
            }
            else
            {
                btnInserisciTipologia.Visible = true;
                btnSeleziona.Visible = true;
                btnAnnullaTipologia.Visible = true;
            }
        }

        protected void btnConfermaInserimentoTipologia_Click(object sendere, EventArgs e)
        {
            // INSERISCO TIPOLOGIA
            Esito esito = new Esito();
            Tipologica tipologia = new Tipologica();

            tipologia.nome = tbInsNomeTipologia.Text.Trim();
            tipologia.descrizione = tbInsDescrizioneTipologia.Text.Trim();

            //tipologia.parametri = tbInsParametriTipologia.Text.Trim();
            //tipologia.sottotipo = tbInsSottotipoTipologia.Text.Trim();

            tipologia.parametri = cmbInsParametriTipologia.SelectedValue;
            tipologia.sottotipo = cmbInsSottotipoTipologia.SelectedValue;

            tipologia.attivo = true;

            if (esito.codice != Esito.ESITO_OK)
            {
                panelErrore.Style.Add("display", "block");
                lbl_MessaggioErrore.Text = "Controllare i campi evidenziati";
            }
            else
            {
                NascondiErroriValidazione();

                int iRet = UtilityTipologiche.CreaTipologia(EnumTipologiche.TIPO_COLONNE_AGENDA, tipologia, ref esito);
                if (esito.codice != Esito.ESITO_OK)
                {
                    panelErrore.Style.Add("display", "block");
                    lbl_MessaggioErrore.Text = esito.descrizione;
                }
                else
                {
                    tbInsNomeTipologia.Text = "";
                    tbInsDescrizioneTipologia.Text = "";

                    //tbInsParametriTipologia.Text = "";
                    //tbInsSottotipoTipologia.Text = "";

                    cmbInsParametriTipologia.SelectedIndex = 0;
                    cmbInsSottotipoTipologia.SelectedIndex = 0;

                    caricaTipologia();
                }

            }
        }

        protected void btnEliminaTipologia_Click(object sender, EventArgs e)
        {

            //ELIMINO LA TIPOLOGIA SELEZIONATA
            if (!string.IsNullOrEmpty(tbIdTipologiaDaModificare.Text.Trim()))
            {
                try
                {
                    NascondiErroriValidazione();
                    Esito esito = UtilityTipologiche.EliminaTipologia(EnumTipologiche.TIPO_COLONNE_AGENDA, Convert.ToInt32(tbIdTipologiaDaModificare.Text.Trim()));

                    if (esito.codice != Esito.ESITO_OK)
                    {
                        panelErrore.Style.Add("display", "block");
                        lbl_MessaggioErrore.Text = esito.descrizione;
                    }
                    else
                    {
                        tbInsNomeTipologia.Text = "";
                        tbInsDescrizioneTipologia.Text = "";

                        //tbInsParametriTipologia.Text = "";
                        //tbInsSottotipoTipologia.Text = "";

                        cmbInsParametriTipologia.SelectedIndex = 0;
                        cmbInsSottotipoTipologia.SelectedIndex = 0;

                        btnModificaTipologia.Visible =false;
                        btnInserisciTipologia.Visible = true;
                        btnEliminaTipologia.Visible = false;

                        caricaTipologia();
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

            tbInsDescrizioneTipologia.CssClass = tbInsDescrizioneTipologia.CssClass.Replace("erroreValidazione", "");
            tbInsNomeTipologia.CssClass = tbInsNomeTipologia.CssClass.Replace("erroreValidazione", "");
            //tbInsParametriTipologia.CssClass = tbInsParametriTipologia.CssClass.Replace("erroreValidazione", "");
            //tbInsSottotipoTipologia.CssClass = tbInsSottotipoTipologia.CssClass.Replace("erroreValidazione", "");
            cmbInsParametriTipologia.CssClass = cmbInsParametriTipologia.CssClass.Replace("erroreValidazione", "");
            cmbInsSottotipoTipologia.CssClass = cmbInsSottotipoTipologia.CssClass.Replace("erroreValidazione", "");

        }

        protected void btnSeleziona_Click(object sender, EventArgs e)
        {
            //SCARICO LA TIPOLOGIA SELEZIONATO
            if (lbMod_Tipologia.SelectedIndex >= 0)
            {
                try
                {
                    NascondiErroriValidazione();

                    string tipologiaSelezionata = lbMod_Tipologia.SelectedValue;
                    Esito esito = new Esito();
                    Tipologica tipologica = UtilityTipologiche.getTipologicaById(EnumTipologiche.TIPO_COLONNE_AGENDA, Convert.ToInt32(tipologiaSelezionata), ref esito);

                    if (esito.codice != Esito.ESITO_OK)
                    {
                        btnInserisciTipologia.Visible = true;
                        panelErrore.Style.Remove("display");
                        lbl_MessaggioErrore.Text = esito.descrizione;
                    }
                    else
                    {
                        btnInserisciTipologia.Visible = false;
                        btnModificaTipologia.Visible = true;
                        btnEliminaTipologia.Visible = true;
                        tbInsDescrizioneTipologia.Text = tipologica.descrizione;
                        tbInsNomeTipologia.Text = tipologica.nome;

                        if (!string.IsNullOrEmpty(tipologica.parametri))
                        {
                            ListItem itemTrovato = cmbInsParametriTipologia.Items.FindByValue(tipologica.parametri);
                            if (itemTrovato != null)
                            {
                                cmbInsParametriTipologia.SelectedValue = itemTrovato.Value;
                            }
                            else
                            {
                                cmbInsParametriTipologia.SelectedIndex = 0;
                            }
                            
                        }
                        else
                        {
                            cmbInsParametriTipologia.SelectedIndex = 0;
                        }

                        if (!string.IsNullOrEmpty(tipologica.sottotipo))
                        {
                            ListItem itemTrovato = cmbInsSottotipoTipologia.Items.FindByValue(tipologica.sottotipo);
                            if (itemTrovato != null)
                            {
                                cmbInsSottotipoTipologia.SelectedValue = itemTrovato.Value;
                            }
                            else
                            {
                                cmbInsSottotipoTipologia.SelectedIndex = 0;
                            }

                        }
                        else
                        {
                            cmbInsSottotipoTipologia.SelectedIndex = 0;
                        }

                        tbIdTipologiaDaModificare.Text = lbMod_Tipologia.SelectedValue;
                    }
                }
                catch (Exception ex)
                {
                    log.Error("btnSeleziona_Click", ex);
                    btnInserisciTipologia.Visible = true;
                    btnModificaTipologia.Visible = false;
                    btnEliminaTipologia.Visible = false;
                    panelErrore.Style.Add("display", "block");
                    lbl_MessaggioErrore.Text = ex.Message;
                }
            }

        }

        protected void btnModificaTipologia_Click(object sender, EventArgs e)
        {
            //MODIFICO TIPOLOGIA
            if (!string.IsNullOrEmpty(tbInsNomeTipologia.Text))
            {
                try
                {
                    NascondiErroriValidazione();

                    Esito esito = new Esito();
                    Tipologica nuovaTipologia = new Tipologica();
                    nuovaTipologia.id = Convert.ToInt32(tbIdTipologiaDaModificare.Text);
                    nuovaTipologia.nome = tbInsNomeTipologia.Text.Trim();
                    nuovaTipologia.descrizione = tbInsDescrizioneTipologia.Text.Trim();

                    nuovaTipologia.parametri = cmbInsParametriTipologia.SelectedValue;
                    nuovaTipologia.sottotipo = cmbInsSottotipoTipologia.SelectedValue;


                    nuovaTipologia.attivo = true;
                    esito = UtilityTipologiche.AggiornaTipologia(EnumTipologiche.TIPO_COLONNE_AGENDA, nuovaTipologia);

                    btnModificaTipologia.Visible = false;
                    btnInserisciTipologia.Visible = true;
                    btnEliminaTipologia.Visible = false;
                    if (esito.codice != Esito.ESITO_OK)
                    {
                        panelErrore.Style.Remove("display");
                        lbl_MessaggioErrore.Text = esito.descrizione;
                    }
                    else
                    {
                        tbIdTipologiaDaModificare.Text = "";
                        tbInsNomeTipologia.Text = "";
                        tbInsDescrizioneTipologia.Text = "";
                        cmbInsParametriTipologia.SelectedIndex = 0;
                        cmbInsSottotipoTipologia.SelectedIndex = 0;
                        caricaTipologia();
                    }
                }
                catch (Exception ex)
                {
                    log.Error("btnModificaTipologia_Click", ex);
                    btnModificaTipologia.Visible = false;
                    btnInserisciTipologia.Visible = true;
                    btnEliminaTipologia.Visible = false;
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

        protected void btnAnnullaTipologia_Click(object sender, EventArgs e)
        {
            tbInsNomeTipologia.Text = "";
            tbInsDescrizioneTipologia.Text = "";

            cmbInsParametriTipologia.SelectedIndex = 0;
            cmbInsSottotipoTipologia.SelectedIndex = 0;

            tbIdTipologiaDaModificare.Text = "";

            btnModificaTipologia.Visible = false;
            btnInserisciTipologia.Visible = true;
            btnEliminaTipologia.Visible = false;
        }

    }
}