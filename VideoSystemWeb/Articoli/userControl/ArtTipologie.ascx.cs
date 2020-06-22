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
using System.Data.SqlClient;

namespace VideoSystemWeb.Articoli.userControl
{
    public partial class ArtTipologie : System.Web.UI.UserControl
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
                    CaricaTipologia(false);

                }
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "chiudiLoader", script: "$('.loader').hide();", addScriptTags: true);
            }
        }

        private List<Tipologica> CaricaTipologia(bool clearLista)
        {
            List<Tipologica> lista;
            Esito esito = new Esito();
            switch (ViewState["TIPO_ARTICOLO"].ToString())
            {
                case "GENERI":
                    if (clearLista)
                    {
                        SessionManager.ListaTipiGeneri.Clear();
                    }
                    lblTipoArticolo.ForeColor = System.Drawing.Color.Red;
                    lista = SessionManager.ListaTipiGeneri;// UtilityTipologiche.CaricaTipologica(EnumTipologiche.TIPO_GENERE, true, ref esito);
                    ViewState["TABELLA_SELEZIONATA"] = EnumTipologiche.TIPO_GENERE;
                    break;
                case "GRUPPI":
                    if (clearLista)
                    {
                        SessionManager.ListaTipiGruppi.Clear();
                    }
                    lblTipoArticolo.ForeColor = System.Drawing.Color.Green;
                    lista = SessionManager.ListaTipiGruppi;// UtilityTipologiche.CaricaTipologica(EnumTipologiche.TIPO_GRUPPO, true, ref esito);
                    ViewState["TABELLA_SELEZIONATA"] = EnumTipologiche.TIPO_GRUPPO;
                    break;
                case "SOTTOGRUPPI":
                    if (clearLista)
                    {
                        SessionManager.ListaTipiSottogruppi.Clear();
                    }
                    lblTipoArticolo.ForeColor = System.Drawing.Color.Blue;
                    lista = SessionManager.ListaTipiSottogruppi; // UtilityTipologiche.CaricaTipologica(EnumTipologiche.TIPO_SOTTOGRUPPO, true, ref esito);
                    ViewState["TABELLA_SELEZIONATA"] = EnumTipologiche.TIPO_SOTTOGRUPPO;
                    break;
                case "TENDER":
                    if (clearLista)
                    {
                        SessionManager.ListaTender.Clear();
                    }
                    lblTipoArticolo.ForeColor = System.Drawing.Color.Brown;
                    //lista = UtilityTipologiche.CaricaTipologica(EnumTipologiche.TIPO_TENDER, true, ref esito);
                    lista = SessionManager.ListaTender;
                    ViewState["TABELLA_SELEZIONATA"] = EnumTipologiche.TIPO_TENDER;
                    break;
                case "QUALIFICHE":
                    if (clearLista)
                    {
                        SessionManager.ListaQualifiche.Clear();
                    }
                    lblTipoArticolo.ForeColor = System.Drawing.Color.Yellow;
                    lista = SessionManager.ListaQualifiche;// UtilityTipologiche.CaricaTipologica(EnumTipologiche.TIPO_QUALIFICHE, true, ref esito);
                    ViewState["TABELLA_SELEZIONATA"] = EnumTipologiche.TIPO_QUALIFICHE;
                    break;
                case "CLIENTI/FORNITORI":
                    if (clearLista)
                    {
                        SessionManager.ListaClientiFornitori.Clear();
                    }
                    lblTipoArticolo.ForeColor = System.Drawing.Color.OrangeRed;
                    lista = SessionManager.ListaTipiClientiFornitori;   //UtilityTipologiche.CaricaTipologica(EnumTipologiche.TIPO_CLIENTI_FORNITORI, true, ref esito);
                    ViewState["TABELLA_SELEZIONATA"] = EnumTipologiche.TIPO_CLIENTI_FORNITORI;
                    break;
                case "PROTOCOLLI":
                    if (clearLista)
                    {
                        SessionManager.ListaTipiProtocolli.Clear();
                    }
                    lblTipoArticolo.ForeColor = System.Drawing.Color.Orange;
                    lista = SessionManager.ListaTipiProtocolli; //UtilityTipologiche.CaricaTipologica(EnumTipologiche.TIPO_PROTOCOLLO, true, ref esito);
                    ViewState["TABELLA_SELEZIONATA"] = EnumTipologiche.TIPO_PROTOCOLLO;
                    break;
                case "LAVORAZIONI":
                    if (clearLista)
                    {
                        SessionManager.ListaTipiTipologie.Clear();
                    }
                    lblTipoArticolo.ForeColor = System.Drawing.Color.LightSkyBlue;
                    lista = SessionManager.ListaTipiTipologie;  //UtilityTipologiche.CaricaTipologica(EnumTipologiche.TIPO_TIPOLOGIE, true, ref esito);
                    ViewState["TABELLA_SELEZIONATA"] = EnumTipologiche.TIPO_TIPOLOGIE;
                    break;
                case "INTERVENTO":
                    if (clearLista)
                    {
                        SessionManager.ListaTipiIntervento.Clear();
                    }
                    lblTipoArticolo.ForeColor = System.Drawing.Color.BlueViolet;
                    lista = SessionManager.ListaTipiIntervento;  //UtilityTipologiche.CaricaTipologica(EnumTipologiche.TIPO_TIPOLOGIE, true, ref esito);
                    ViewState["TABELLA_SELEZIONATA"] = EnumTipologiche.TIPO_INTERVENTO;
                    break;
                case "CATEGORIE":
                    if (clearLista)
                    {
                        SessionManager.ListaTipiCategorieMagazzino.Clear();
                    }
                    lblTipoArticolo.ForeColor = System.Drawing.Color.Bisque;
                    lista = SessionManager.ListaTipiCategorieMagazzino;
                    ViewState["TABELLA_SELEZIONATA"] = EnumTipologiche.TIPO_CATEGORIE_MAGAZZINO;
                    break;
                case "SUBCATEGORIE":
                    if (clearLista)
                    {
                        SessionManager.ListaTipiSubCategorieMagazzino.Clear();
                    }
                    lblTipoArticolo.ForeColor = System.Drawing.Color.BurlyWood;
                    lista = SessionManager.ListaTipiSubCategorieMagazzino;
                    ViewState["TABELLA_SELEZIONATA"] = EnumTipologiche.TIPO_SUB_CATEGORIE_MAGAZZINO;
                    break;
                case "POSIZIONI":
                    if (clearLista)
                    {
                        SessionManager.ListaTipiPosizioniMagazzino.Clear();
                    }
                    lblTipoArticolo.ForeColor = System.Drawing.Color.Coral;
                    lista = SessionManager.ListaTipiPosizioniMagazzino;
                    ViewState["TABELLA_SELEZIONATA"] = EnumTipologiche.TIPO_POSIZIONE_MAGAZZINO;
                    break;
                case "GRUPPO_MAGAZZINO":
                    if (clearLista)
                    {
                        SessionManager.ListaTipiGruppoMagazzino.Clear();
                    }
                    lblTipoArticolo.ForeColor = System.Drawing.Color.DarkKhaki;
                    lista = SessionManager.ListaTipiGruppoMagazzino;
                    ViewState["TABELLA_SELEZIONATA"] = EnumTipologiche.TIPO_GRUPPO_MAGAZZINO;
                    break;
                case "BANCA":
                    if (clearLista)
                    {
                        SessionManager.ListaTipiBanca.Clear();
                    }
                    lblTipoArticolo.ForeColor = System.Drawing.Color.DarkViolet;
                    lista = SessionManager.ListaTipiBanca;
                    ViewState["TABELLA_SELEZIONATA"] = EnumTipologiche.TIPO_BANCA;
                    break;
                default:
                    if (clearLista)
                    {
                        SessionManager.ListaTipiGeneri.Clear();
                    }
                    lblTipoArticolo.ForeColor = System.Drawing.Color.Red;
                    lista = SessionManager.ListaTipiGeneri;// UtilityTipologiche.CaricaTipologica(EnumTipologiche.TIPO_GENERE, true, ref esito);
                    ViewState["TABELLA_SELEZIONATA"] = EnumTipologiche.TIPO_GENERE;
                    break;
            }


            BasePage p = new BasePage();

            // CARICO LA COMBO
            if (string.IsNullOrEmpty(esito.Descrizione))
            {
                lbMod_Tipologia.Items.Clear();
                foreach (Tipologica tipologia in lista)
                {
                    ListItem item = new ListItem
                    {
                        Text = tipologia.nome,
                        Value = tipologia.id.ToString()
                    };
                    lbMod_Tipologia.Items.Add(item);
                }

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
            Tipologica tipologia = new Tipologica
            {
                nome = tbInsNomeTipologia.Text.Trim(),
                descrizione = tbInsDescrizioneTipologia.Text.Trim(),
                parametri = tbInsParametriTipologia.Text.Trim(),
                sottotipo = tbInsSottotipoTipologia.Text.Trim(),
                attivo = true
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

                int iRet = UtilityTipologiche.CreaTipologia((EnumTipologiche)ViewState["TABELLA_SELEZIONATA"], tipologia, ref esito);
                
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
                    List<Tipologica> lista = CaricaTipologia(true);
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
                    esito = UtilityTipologiche.EliminaTipologia((EnumTipologiche)ViewState["TABELLA_SELEZIONATA"], Convert.ToInt32(tbIdTipologiaDaModificare.Text.Trim()));
                    if (esito.Codice != Esito.ESITO_OK)
                    {
                        if(esito.Descrizione.IndexOf("conflitto con il vincolo REFERENCE") > -1)
                        {
                            basePage.ShowWarning("Attenzione, la tipologia selezionata è associata ad altri record, prima di eliminarla è necessario eliminare i record associati");
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

                        btnModificaTipologia.Visible = false;
                        btnInserisciTipologia.Visible = true;
                        btnEliminaTipologia.Visible = false;

                        List<Tipologica> lista = CaricaTipologia(true);
                        HttpContext.Current.Session[ViewState["TABELLA_SELEZIONATA"].ToString()] = lista;

                    }
                }
                catch (Exception ex)
                {
                    log.Error("btnEliminaDocumento_Click", ex);
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
            //SCARICO LA TIPOLOGIA SELEZIONATO
            if (lbMod_Tipologia.SelectedIndex >= 0)
            {
                Esito esito = new Esito();
                try
                {
                    NascondiErroriValidazione();

                    string tipologiaSelezionata = lbMod_Tipologia.SelectedValue;
                    
                    Tipologica tipologica = UtilityTipologiche.getTipologicaById((EnumTipologiche)ViewState["TABELLA_SELEZIONATA"], Convert.ToInt32(tipologiaSelezionata), ref esito);

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
                        tbInsDescrizioneTipologia.Text = tipologica.descrizione;
                        tbInsNomeTipologia.Text = tipologica.nome;
                        tbInsParametriTipologia.Text = tipologica.parametri;
                        tbInsSottotipoTipologia.Text = tipologica.sottotipo;
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

                    Tipologica nuovaTipologia = new Tipologica
                    {
                        id = Convert.ToInt32(tbIdTipologiaDaModificare.Text),
                        nome = tbInsNomeTipologia.Text.Trim(),
                        descrizione = tbInsDescrizioneTipologia.Text.Trim(),
                        parametri = tbInsParametriTipologia.Text.Trim(),
                        sottotipo = tbInsSottotipoTipologia.Text.Trim(),
                        attivo = true
                    };
                    esito = UtilityTipologiche.AggiornaTipologia((EnumTipologiche)ViewState["TABELLA_SELEZIONATA"], nuovaTipologia);

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
                        List<Tipologica> lista = CaricaTipologia(true);
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

            btnModificaTipologia.Visible = false;
            btnInserisciTipologia.Visible = true;
            btnEliminaTipologia.Visible = false;
        }
    }
}