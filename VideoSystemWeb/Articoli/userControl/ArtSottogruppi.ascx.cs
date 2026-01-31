using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoSystemWeb.BLL;
using VideoSystemWeb.Entity;

namespace VideoSystemWeb.Articoli.userControl
{
    public partial class ArtSottogruppi : System.Web.UI.UserControl
    {
        BasePage basePage = new BasePage();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_Load(object sender, EventArgs e)
        {
            // FUNZIONA SE NELLA PAGINA ASPX CHIAMANTE C'E' UN CAMPO HIDDENFIELD COL TIPO ARTICOLO (GENERI/GRUPPI/SOTTOGRUPPI/GRUPPO_MAGAZZINO)
            HiddenField tipoArticolo = this.Parent.FindControl("HF_TIPO_ARTICOLO") as HiddenField;
            if (tipoArticolo != null)
            {
                ViewState["TIPO_ARTICOLO"] = tipoArticolo.Value;
            }
            else
            {
                ViewState["TIPO_ARTICOLO"] = "GENERI";
            }
            if (!tipoArticolo.Value.ToUpper().Equals("ARTICOLI"))
            {
                if (!Page.IsPostBack)
                {
                    lblTipoArticolo.Text = ViewState["TIPO_ARTICOLO"].ToString();
                    CaricaSottogruppi(false);
                }
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "chiudiLoader", script: "$('.loader').hide();", addScriptTags: true);
            }
        }

        private List<Sottogruppo> CaricaSottogruppi(bool clearLista)
        {
            List<Sottogruppo> lista;
            Esito esito = new Esito();
            
            if (clearLista)
            {
                SessionManager.ListaTipiSottogruppi.Clear();
            }
            lblTipoArticolo.ForeColor = System.Drawing.Color.Blue;
            lista = SessionManager.ListaTipiSottogruppi;// UtilityTipologiche.CaricaTipologica(EnumTipologiche.TIPO_SOTTOGRUPPO, true, ref esito);
            ViewState["TABELLA_SELEZIONATA"] = EnumTipologiche.TIPO_SOTTOGRUPPO;
                    
            BasePage p = new BasePage();

            // CARICO LA COMBO
            if (string.IsNullOrEmpty(esito.Descrizione))
            {
                lbMod_Tipologia.Items.Clear();
                foreach (Sottogruppo tipologia in lista)
                {
                    ListItem item = new ListItem
                    {
                        Text = tipologia.nome,
                        Value = tipologia.id.ToString()
                    };
                    lbMod_Tipologia.Items.Add(item);
                }

                #region GRUPPI
                ddl_Gruppo.Items.Clear();
                ddl_Gruppo.Items.Add("");
                foreach (Tipologica tipologiaGruppo in SessionManager.ListaTipiGruppi)
                {
                    ListItem item = new ListItem();
                    item.Text = tipologiaGruppo.nome;
                    item.Value = tipologiaGruppo.id.ToString();
                    ddl_Gruppo.Items.Add(item);
                }
                #endregion

                // SE UTENTE ABILITATO ALLE MODIFICHE FACCIO VEDERE I PULSANTI DI MODIFICA
                AbilitaBottoni(p.AbilitazioneInScrittura());
            }
            else
            {
                Session["ErrorPageText"] = esito.Descrizione;
                string url = String.Format("~/pageError.aspx");
                Response.Redirect(url, true);
            }
            return lista;
        }

        private void AbilitaBottoni(bool utenteAbilitatoInScrittura)
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
            Sottogruppo sottogruppo = new Sottogruppo
            {
                nome = tbInsNomeTipologia.Text.Trim(),
                descrizione = tbInsDescrizioneTipologia.Text.Trim(),
                parametri = tbInsParametriTipologia.Text.Trim(),
                sottotipo = tbInsSottotipoTipologia.Text.Trim(),
                attivo = true,
                IdTipoGruppo = int.Parse(ddl_Gruppo.SelectedValue)
            };

            if (esito.Codice != Esito.ESITO_OK)
            {
                //panelErrore.Style.Add("display", "block");
                //lbl_MessaggioErrore.Text = "Controllare i campi evidenziati";
                basePage.ShowWarning("Controllare i campi evidenziati");
            }
            else
            {
                NascondiErroriValidazione();

                int iRet = UtilityTipologiche.CreaSottogruppo(sottogruppo, ref esito);

                if (esito.Codice != Esito.ESITO_OK)
                {
                    //panelErrore.Style.Add("display", "block");
                    //lbl_MessaggioErrore.Text = esito.descrizione;
                    basePage.ShowError(esito.Descrizione);
                }
                else
                {
                    tbInsNomeTipologia.Text = "";
                    tbInsDescrizioneTipologia.Text = "1";
                    tbInsParametriTipologia.Text = "";
                    tbInsSottotipoTipologia.Text = "";
                    List<Sottogruppo> lista = CaricaSottogruppi(true);
                    HttpContext.Current.Session[ViewState["TABELLA_SELEZIONATA"].ToString()] = lista;
                }
            }
        }


        protected void btnEliminaTipologia_Click(object sender, EventArgs e)
        {
            //ELIMINO LA TIPOLOGIA SELEZIONATA
            if (!string.IsNullOrEmpty(tbIdTipologiaDaModificare.Text.Trim()))
            {
                Esito esito = new Esito();
                try
                {
                    NascondiErroriValidazione();
                    //esito = UtilityTipologiche.RemoveTipologia((EnumTipologiche)ViewState["TABELLA_SELEZIONATA"], Convert.ToInt32(tbIdTipologiaDaModificare.Text.Trim()));
                    esito = UtilityTipologiche.EliminaSottogruppo(Convert.ToInt32(tbIdTipologiaDaModificare.Text.Trim()));
                    if (esito.Codice != Esito.ESITO_OK)
                    {
                        if (esito.Descrizione.IndexOf("conflitto con il vincolo REFERENCE") > -1 || esito.Descrizione.IndexOf("conflicted with the REFERENCE constraint") > -1)
                        {
                            basePage.ShowWarning("Attenzione, il sottogruppo selezionato è associato ad altri record, prima di eliminarlo è necessario eliminare i record associati");
                        }
                        else
                        {
                            basePage.ShowError(esito.Descrizione);
                        }
                    }
                    else
                    {
                        tbInsNomeTipologia.Text = "";
                        tbInsDescrizioneTipologia.Text = "";
                        tbInsParametriTipologia.Text = "";
                        tbInsSottotipoTipologia.Text = "";
                        tbIdTipologiaDaModificare.Text = "";
                        ddl_Gruppo.SelectedIndex = 0;

                        btnModificaTipologia.Visible = false;
                        btnInserisciTipologia.Visible = true;
                        btnEliminaTipologia.Visible = false;

                        List<Sottogruppo> lista = CaricaSottogruppi(true);
                        HttpContext.Current.Session[ViewState["TABELLA_SELEZIONATA"].ToString()] = lista;

                    }
                }
                catch (Exception ex)
                {
                    log.Error("btnEliminaTipologia_Click", ex);
                    if (esito.Codice == Esito.ESITO_OK)
                    {
                        esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                        esito.Descrizione = ex.Message + Environment.NewLine + ex.StackTrace;
                    }
                    basePage.ShowError(ex.Message);
                }
            }
            else
            {
                basePage.ShowError("Verificare il corretto inserimento dei campi");
            }
        }

        private void NascondiErroriValidazione()
        {
            tbInsDescrizioneTipologia.CssClass = tbInsDescrizioneTipologia.CssClass.Replace("erroreValidazione", "");
            tbInsNomeTipologia.CssClass = tbInsNomeTipologia.CssClass.Replace("erroreValidazione", "");
            tbInsParametriTipologia.CssClass = tbInsParametriTipologia.CssClass.Replace("erroreValidazione", "");
            tbInsSottotipoTipologia.CssClass = tbInsSottotipoTipologia.CssClass.Replace("erroreValidazione", "");
        }

        protected void btnSeleziona_Click(object sender, EventArgs e)
        {
            //SCARICO IL SOTTOGRUPPO SELEZIONATO
            if (lbMod_Tipologia.SelectedIndex >= 0)
            {
                Esito esito = new Esito();
                try
                {
                    NascondiErroriValidazione();

                    string sottogruppoSelezionata = lbMod_Tipologia.SelectedValue;

                    Sottogruppo sottogruppo = UtilityTipologiche.GetSottogruppoById(Convert.ToInt32(sottogruppoSelezionata), ref esito);

                    if (esito.Codice != Esito.ESITO_OK)
                    {
                        btnInserisciTipologia.Visible = true;

                        basePage.ShowError(esito.Descrizione);
                    }
                    else
                    {
                        btnInserisciTipologia.Visible = false;
                        btnModificaTipologia.Visible = true;
                        btnEliminaTipologia.Visible = true;
                        tbInsDescrizioneTipologia.Text = sottogruppo.descrizione;
                        tbInsNomeTipologia.Text = sottogruppo.nome;
                        tbInsParametriTipologia.Text = sottogruppo.parametri;
                        tbInsSottotipoTipologia.Text = sottogruppo.sottotipo;
                        ddl_Gruppo.SelectedValue = sottogruppo.IdTipoGruppo.ToString();
                        tbIdTipologiaDaModificare.Text = lbMod_Tipologia.SelectedValue;
                    }
                }
                catch (Exception ex)
                {
                    log.Error("btnSeleziona_Click", ex);
                    btnInserisciTipologia.Visible = true;
                    btnModificaTipologia.Visible = false;
                    btnEliminaTipologia.Visible = false;
                    if (esito.Codice == Esito.ESITO_OK)
                    {
                        esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                        esito.Descrizione = ex.Message + Environment.NewLine + ex.StackTrace;
                    }
                    basePage.ShowError(ex.Message);
                }
            }
        }

        protected void btnModificaTipologia_Click(object sender, EventArgs e)
        {
            //MODIFICO TIPOLOGIA
            if (!string.IsNullOrEmpty(tbInsNomeTipologia.Text))
            {
                Esito esito = new Esito();
                try
                {
                    NascondiErroriValidazione();

                    Sottogruppo nuovoSottogruppo = new Sottogruppo
                    {
                        id = Convert.ToInt32(tbIdTipologiaDaModificare.Text),
                        nome = tbInsNomeTipologia.Text.Trim(),
                        descrizione = tbInsDescrizioneTipologia.Text.Trim(),
                        parametri = tbInsParametriTipologia.Text.Trim(),
                        sottotipo = tbInsSottotipoTipologia.Text.Trim(),
                        attivo = true,
                        IdTipoGruppo = int.Parse(ddl_Gruppo.SelectedValue)
                    };
                    esito = UtilityTipologiche.AggiornaSottogruppo(nuovoSottogruppo);

                    btnModificaTipologia.Visible = false;
                    btnInserisciTipologia.Visible = true;
                    btnEliminaTipologia.Visible = false;
                    if (esito.Codice != Esito.ESITO_OK)
                    {
                        basePage.ShowError(esito.Descrizione);
                    }
                    else
                    {
                        tbIdTipologiaDaModificare.Text = "";
                        tbInsNomeTipologia.Text = "";
                        tbInsDescrizioneTipologia.Text = "";
                        tbInsParametriTipologia.Text = "";
                        tbInsSottotipoTipologia.Text = "";
                        ddl_Gruppo.SelectedIndex = 0;
                        List<Sottogruppo> lista = CaricaSottogruppi(true);
                        HttpContext.Current.Session[ViewState["TABELLA_SELEZIONATA"].ToString()] = lista;
                    }
                }
                catch (Exception ex)
                {
                    log.Error("btnModificaTipologia_Click", ex);
                    btnModificaTipologia.Visible = false;
                    btnInserisciTipologia.Visible = true;
                    btnEliminaTipologia.Visible = false;
                    if (esito.Codice == Esito.ESITO_OK)
                    {
                        esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                        esito.Descrizione = ex.Message + Environment.NewLine + ex.StackTrace;
                    }
                    basePage.ShowError(ex.Message);
                }
            }
            else
            {
                basePage.ShowError("Verificare il corretto inserimento dei campi");
            }
        }

        protected void btnAnnullaTipologia_Click(object sender, EventArgs e)
        {
            tbInsNomeTipologia.Text = "";
            tbInsDescrizioneTipologia.Text = "";
            tbInsParametriTipologia.Text = "";
            tbInsSottotipoTipologia.Text = "";
            tbIdTipologiaDaModificare.Text = "";
            ddl_Gruppo.SelectedIndex = 0;

            btnModificaTipologia.Visible = false;
            btnInserisciTipologia.Visible = true;
            btnEliminaTipologia.Visible = false;
        }
    }
}