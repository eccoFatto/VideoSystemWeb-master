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

namespace VideoSystemWeb.MAGAZZINO
{
    public partial class Attrezzature : BasePage
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

                // CARICO LE COMBO
                if (string.IsNullOrEmpty(esito.Descrizione))
                {
                    ddlTipoCategoria.Items.Clear();
                    cmbMod_Categoria.Items.Clear();
                    ddlTipoCategoria.Items.Add("");
                    foreach (Tipologica tipologiaCategoria in SessionManager.ListaTipiCategorieMagazzino)
                    {
                        ListItem item = new ListItem
                        {
                            Text = tipologiaCategoria.nome,
                            Value = tipologiaCategoria.id.ToString()
                        };

                        ddlTipoCategoria.Items.Add(item);

                    ListItem itemMod = new ListItem
                    {
                        Text = tipologiaCategoria.nome,
                        Value = tipologiaCategoria.id.ToString()
                        };

                        cmbMod_Categoria.Items.Add(itemMod);
                    }

                    
                    cmbMod_SubCategoria.Items.Clear();
                    cmbMod_SubCategoria.Items.Add("");
                    foreach (Tipologica tipologiaSubCategoria in SessionManager.ListaTipiSubCategorieMagazzino)
                    {

                        ListItem itemMod = new ListItem
                        {
                            Text = tipologiaSubCategoria.nome,
                            Value = tipologiaSubCategoria.id.ToString()
                        };

                        cmbMod_SubCategoria.Items.Add(itemMod);
                    }

                    riempiComboSubCategoria(0);

                    ddlTipoPosizioneMagazzino.Items.Clear();
                    cmbMod_Posizione.Items.Clear();
                    ddlTipoPosizioneMagazzino.Items.Add("");
                    foreach (Tipologica tipologiaPosizione in SessionManager.ListaTipiPosizioniMagazzino)
                    {
                        ListItem item = new ListItem
                        {
                            Text = tipologiaPosizione.nome,
                            Value = tipologiaPosizione.nome
                        };

                        ddlTipoPosizioneMagazzino.Items.Add(item);

                        ListItem itemMod = new ListItem
                        {
                            Text = tipologiaPosizione.nome,
                            Value = tipologiaPosizione.id.ToString()
                        };

                        cmbMod_Posizione.Items.Add(itemMod);
                    }

                    ddlTipoGruppoMagazzino.Items.Clear();
                    cmbMod_Gruppo.Items.Clear();
                    ddlTipoGruppoMagazzino.Items.Add("");
                    foreach (Tipologica tipologiaGruppo in SessionManager.ListaTipiGruppoMagazzino)
                    {
                        ListItem item = new ListItem
                        {
                            Text = tipologiaGruppo.nome,
                            Value = tipologiaGruppo.nome
                        };

                        ddlTipoGruppoMagazzino.Items.Add(item);

                        ListItem itemMod = new ListItem
                        {
                            Text = tipologiaGruppo.nome,
                            Value = tipologiaGruppo.id.ToString()
                        };

                        cmbMod_Gruppo.Items.Add(itemMod);
                    }
                    // SE UTENTE ABILITATO ALLE MODIFICHE FACCIO VEDERE I PULSANTI DI MODIFICA
                    AbilitaBottoni(basePage.AbilitazioneInScrittura());

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

        private void riempiComboSubCategoria(int idCategoria)
        {
            ddlTipoSubCategoria.Items.Clear();
            ddlTipoSubCategoria.Items.Add("");

            string queryRicercaSubcategoria = "select * from tipo_subcategoria_magazzino where attivo = 1 ";
            if (idCategoria > 0)
            {
                queryRicercaSubcategoria += "and id in (select distinct id_subcategoria from mag_attrezzature where id_categoria="+idCategoria.ToString()+") ";
            }
            queryRicercaSubcategoria += "order by nome";
            Esito esito = new Esito();
            DataTable dtSubCategorie = Base_DAL.GetDatiBySql(queryRicercaSubcategoria, ref esito);

            foreach (DataRow tipologiaSubCategoria in dtSubCategorie.Rows)
            {
                ListItem item = new ListItem
                {
                    Text = tipologiaSubCategoria["nome"].ToString(),
                    Value = tipologiaSubCategoria["nome"].ToString()
                };

                ddlTipoSubCategoria.Items.Add(item);

            }
        }

        protected void btnRicercaAttrezzatura_Click(object sender, EventArgs e)
        {
            string queryRicerca = ConfigurationManager.AppSettings["QUERY_SEARCH_ATTREZZATURE"];

            queryRicerca = queryRicerca.Replace("@codiceVS", tbCodiceVS.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@descrizione", tbDescrizione.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@marca", tbMarca.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@modello", tbModello.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@seriale", tbSeriale.Text.Trim().Replace("'", "''"));
            //queryRicerca = queryRicerca.Replace("@dataAcquisto", tbDataAcquisto.Text.Trim().Replace("'", "''"));

            // SELEZIONO I CAMPI DROPDOWN SE VALORIZZATI
            string queryRicercaCampiDropDown = "";
            if (!string.IsNullOrEmpty(ddlTipoCategoria.SelectedItem.Text))
            {
                queryRicercaCampiDropDown += " and cat.nome='" + ddlTipoCategoria.SelectedItem.Text.Replace("'", "''") + "' ";
            }
            if (!string.IsNullOrEmpty(ddlTipoSubCategoria.SelectedItem.Text))
            {
                queryRicercaCampiDropDown += " and sub.nome='" + ddlTipoSubCategoria.SelectedItem.Text.Replace("'", "''") + "' ";
            }
            if (!string.IsNullOrEmpty(ddlTipoPosizioneMagazzino.SelectedItem.Text))
            {
                queryRicercaCampiDropDown += " and pos.nome='" + ddlTipoPosizioneMagazzino.SelectedItem.Text.Replace("'", "''") + "' ";
            }
            if (!string.IsNullOrEmpty(ddlTipoGruppoMagazzino.SelectedItem.Text))
            {
                queryRicercaCampiDropDown += " and gruppo.nome='" + ddlTipoGruppoMagazzino.SelectedItem.Text.Replace("'", "''") + "' ";
            }

            queryRicerca = queryRicerca.Replace("@campiTendina", queryRicercaCampiDropDown.Trim());
            
            //queryRicerca = queryRicerca.Replace("@categoria", ddlTipoCategoria.SelectedItem.Text.Trim().Replace("'", "''"));
            //queryRicerca = queryRicerca.Replace("@subcategoria", ddlTipoSubCategoria.SelectedItem.Text.Trim().Replace("'", "''"));
            //queryRicerca = queryRicerca.Replace("@posizione", ddlTipoPosizioneMagazzino.SelectedItem.Text.Trim().Replace("'", "''"));
            //queryRicerca = queryRicerca.Replace("@gruppo", ddlTipoGruppoMagazzino.SelectedItem.Text.Trim().Replace("'", "''"));

            Esito esito = new Esito();
            DataTable dtAttrezzature = Base_DAL.GetDatiBySql(queryRicerca, ref esito);
            gv_attrezzature.DataSource = dtAttrezzature;
            gv_attrezzature.DataBind();
            //gv_attrezzature.Columns[1].Visible = false;

        }

        protected void gv_attrezzature_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.Cells.Count>1) e.Row.Cells[1].Visible = false;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // PRENDO L'ID DELL' ATTREZZATURA SELEZIONATA
                string idAttrezzaturaSelezionata = e.Row.Cells[1].Text;
                ImageButton myButtonEdit = e.Row.FindControl("imgEdit") as ImageButton;
                myButtonEdit.Attributes.Add("onclick", "mostraAttrezzatura('" + idAttrezzaturaSelezionata + "');");
            }

        }

        protected void gv_attrezzature_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_attrezzature.PageIndex = e.NewPageIndex;
            btnRicercaAttrezzatura_Click(null, null);

        }

        protected void btnEditAttrezzatura_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hf_idAttrezzatura.Value) || (!string.IsNullOrEmpty((string)ViewState["idAttrezzatura"])))
            {
                if (!string.IsNullOrEmpty(hf_idAttrezzatura.Value)) ViewState["idAttrezzatura"] = hf_idAttrezzatura.Value;
                EditAttrezzatura();
                AttivaDisattivaModificaAttrezzatura(true);
                GestisciPulsantiAttrezzatura("VISUALIZZAZIONE");
                pnlContainer.Visible = true;
            }
        }

        protected void btnInsAttrezzatura_Click(object sender, EventArgs e)
        {
            // VISUALIZZO FORM INSERIMENTO NUOVA ATTREZZATURA
            ViewState["idAttrezzatura"] = "";
            EditAttrezzaturaVuota();
            AttivaDisattivaModificaAttrezzatura(false);
            GestisciPulsantiAttrezzatura("INSERIMENTO");

            pnlContainer.Visible = true;
        }

        protected void btnChiudiPopupServer_Click(object sender, EventArgs e)
        {
            pnlContainer.Visible = false;
        }

        protected void btnGestisciAttrezzatura_Click(object sender, EventArgs e)
        {
            AttivaDisattivaModificaAttrezzatura(false);
            GestisciPulsantiAttrezzatura("MODIFICA");
        }

        protected void btnInserisciAttrezzatura_Click(object sender, EventArgs e)
        {
            // INSERISCO ATTREZZATURA
            Esito esito = new Esito();
            AttrezzatureMagazzino attrezzatura = CreaOggettoAttrezzatura(ref esito);

            if (esito.Codice != Esito.ESITO_OK)
            {
                basePage.ShowError("Controllare i campi evidenziati");
            }
            else
            {
                NascondiErroriValidazione();

                int iRet = AttrezzatureMagazzino_BLL.Instance.CreaAttrezzatura(attrezzatura,ref esito);
                if (iRet > 0)
                {
                    // UNA VOLTA INSERITO CORRETTAMENTE PUO' ESSERE MODIFICATO
                    hf_idAttrezzatura.Value = iRet.ToString();
                    ViewState["idAttrezzatura"] = hf_idAttrezzatura.Value;
                    hf_tipoOperazione.Value = "VISUALIZZAZIONE";
                }

                if (esito.Codice != Esito.ESITO_OK)
                {
                    log.Error(esito.Descrizione);
                    basePage.ShowError(esito.Descrizione);
                }
                else { 
                    basePage.ShowSuccess("Inserita Attrezzatura " + attrezzatura.Descrizione);
                    btnEditAttrezzatura_Click(null, null);
                }
            }

        }

        protected void btnModificaAttrezzatura_Click(object sender, EventArgs e)
        {
            // SALVO MODIFICHE ATTREZZATURA
            Esito esito = new Esito();
            AttrezzatureMagazzino attrezzatura = CreaOggettoAttrezzatura(ref esito);

            if (esito.Codice != Esito.ESITO_OK)
            {
                log.Error(esito.Descrizione);
                basePage.ShowError("Controllare i campi evidenziati!");
            }
            else
            {
                esito = AttrezzatureMagazzino_BLL.Instance.AggiornaAttrezzatura(attrezzatura);

                if (esito.Codice != Esito.ESITO_OK)
                {
                    log.Error(esito.Descrizione);
                    basePage.ShowError(esito.Descrizione);

                }
                btnEditAttrezzatura_Click(null, null);
            }

        }

        protected void btnEliminaAttrezzatura_Click(object sender, EventArgs e)
        {
            Esito esito = new Esito();

            if (!string.IsNullOrEmpty((string)ViewState["idAttrezzatura"]))
            {
                esito = AttrezzatureMagazzino_BLL.Instance.EliminaAttrezzatura(Convert.ToInt32(ViewState["idAttrezzatura"].ToString()));
                //esito = AttrezzatureMagazzino_BLL.Instance.RemoveAttrezzatura(Convert.ToInt32(ViewState["idAttrezzatura"].ToString()));
                if (esito.Codice != Esito.ESITO_OK)
                {
                    basePage.ShowError(esito.Descrizione);
                    AttivaDisattivaModificaAttrezzatura(true);
                }
                else
                {
                    AttivaDisattivaModificaAttrezzatura(true);
                    pnlContainer.Visible = false;
                    btnRicercaAttrezzatura_Click(null, null);
                }

            }

        }

        protected void btnAnnullaAttrezzatura_Click(object sender, EventArgs e)
        {
            AttivaDisattivaModificaAttrezzatura(true);
            GestisciPulsantiAttrezzatura("ANNULLAMENTO");

        }

        private void AbilitaBottoni(bool utenteAbilitatoInScrittura)
        {
            //annullaModifiche();
            if (!utenteAbilitatoInScrittura)
            {
                divBtnInserisciAttrezzatura.Visible = false;
                btnModificaAttrezzatura.Visible = false;
                btnAnnullaAttrezzatura.Visible = false;
                btnEliminaAttrezzatura.Visible = false;
                btnGestisciAttrezzatura.Visible = false;
            }
            else
            {
                divBtnInserisciAttrezzatura.Visible = true;
                btnModificaAttrezzatura.Visible = true;
                btnAnnullaAttrezzatura.Visible = false;
                btnEliminaAttrezzatura.Visible = false;
                btnGestisciAttrezzatura.Visible = true;
            }
        }
        private void NascondiErroriValidazione()
        {
            tbMod_CodiceVideoSystem.CssClass = tbMod_CodiceVideoSystem.CssClass.Replace("erroreValidazione", "");
            tbMod_DataAcquisto.CssClass = tbMod_DataAcquisto.CssClass.Replace("erroreValidazione", "");
            tbMod_Descrizione.CssClass = tbMod_Descrizione.CssClass.Replace("erroreValidazione", "");
            tbMod_Marca.CssClass = tbMod_Marca.CssClass.Replace("erroreValidazione", "");
            tbMod_Modello.CssClass = tbMod_Modello.CssClass.Replace("erroreValidazione", "");
            tbMod_Note.CssClass = tbMod_Note.CssClass.Replace("erroreValidazione", "");
            tbMod_Seriale.CssClass = tbMod_Seriale.CssClass.Replace("erroreValidazione", "");
            cmbMod_Categoria.CssClass = cmbMod_Categoria.CssClass.Replace("erroreValidazione", "");
            cmbMod_SubCategoria.CssClass = cmbMod_SubCategoria.CssClass.Replace("erroreValidazione", "");
            cmbMod_Posizione.CssClass = cmbMod_Posizione.CssClass.Replace("erroreValidazione", "");
            cmbMod_Gruppo.CssClass = cmbMod_Gruppo.CssClass.Replace("erroreValidazione", "");
        }
        private void EditAttrezzatura()
        {
            string idAttrezzatura = (string)ViewState["idAttrezzatura"];
            Esito esito = new Esito();

            if (!string.IsNullOrEmpty(idAttrezzatura))
            {
                Entity.AttrezzatureMagazzino attrezzatura = AttrezzatureMagazzino_BLL.Instance.getAttrezzaturaById(ref esito, Convert.ToInt16(idAttrezzatura));
                if (esito.Codice == 0)
                {
                    PulisciCampiDettaglio();

                    // RIEMPIO I CAMPI DEL DETTAGLIO ATTREZZATURA
                    tbMod_CodiceVideoSystem.Text = attrezzatura.Cod_vs;
                    
                    tbMod_DataAcquisto.Text = "";
                    if (attrezzatura.Data_acquisto != null)
                    {
                        tbMod_DataAcquisto.Text = ((DateTime)attrezzatura.Data_acquisto).ToString("dd/MM/yyyy");
                    }

                    tbMod_Descrizione.Text = attrezzatura.Descrizione;
                    tbMod_Marca.Text = attrezzatura.Marca;
                    tbMod_Modello.Text = attrezzatura.Modello;
                    tbMod_Note.Text = attrezzatura.Note;
                    tbMod_Seriale.Text = attrezzatura.Seriale;

                    //TIPI CATEGORIA
                    ListItem trovati = cmbMod_Categoria.Items.FindByValue(attrezzatura.Id_categoria.ToString());
                    if (trovati != null)
                    {
                        cmbMod_Categoria.SelectedValue = trovati.Value;
                    }
                    else
                    {
                        cmbMod_Categoria.Text = "";
                    }

                    //TIPI SUBCATEGORIA
                    trovati = cmbMod_SubCategoria.Items.FindByValue(attrezzatura.Id_subcategoria.ToString());
                    if (trovati != null)
                    {
                        cmbMod_SubCategoria.SelectedValue = trovati.Value;
                    }
                    else
                    {
                        cmbMod_SubCategoria.Text = "";
                    }

                    //TIPI POSIZIONI
                    trovati = cmbMod_Posizione.Items.FindByValue(attrezzatura.Id_posizione_magazzino.ToString());
                    if (trovati != null)
                    {
                        cmbMod_Posizione.SelectedValue = trovati.Value;
                    }
                    else
                    {
                        ListItem item = new ListItem
                        {
                            Text = attrezzatura.Id_posizione_magazzino.ToString(),
                            Value = attrezzatura.Id_posizione_magazzino.ToString()
                        };
                        cmbMod_Posizione.Items.Add(item);

                        cmbMod_Posizione.Text = attrezzatura.Id_posizione_magazzino.ToString();

                    }

                    //TIPI GRUPPO
                    trovati = cmbMod_Gruppo.Items.FindByValue(attrezzatura.Id_gruppo_magazzino.ToString());
                    if (trovati != null)
                    {
                        cmbMod_Gruppo.SelectedValue = trovati.Value;
                    }
                    else
                    {
                        ListItem item = new ListItem
                        {
                            Text = attrezzatura.Id_gruppo_magazzino.ToString(),
                            Value = attrezzatura.Id_gruppo_magazzino.ToString()
                        };
                        cmbMod_Gruppo.Items.Add(item);

                        cmbMod_Gruppo.Text = attrezzatura.Id_gruppo_magazzino.ToString();

                    }

                    cbMod_Disponibile.Checked = attrezzatura.Disponibile;
                    cbMod_Garanzia.Checked = attrezzatura.Garanzia;
                }
                else
                {
                    Session["ErrorPageText"] = esito.Descrizione;
                    string url = String.Format("~/pageError.aspx");
                    Response.Redirect(url, true);
                }
            }

        }
        private void PulisciCampiDettaglio()
        {
            tbMod_CodiceVideoSystem.Text = "";
            tbMod_DataAcquisto.Text = "";
            tbMod_Descrizione.Text = "";
            tbMod_Marca.Text = "";
            tbMod_Modello.Text = "";
            tbMod_Note.Text = "";
            tbMod_Seriale.Text = "";
            cmbMod_Categoria.SelectedIndex = 0;
            cmbMod_SubCategoria.SelectedIndex = 0;
            cmbMod_Posizione.SelectedIndex = 0;
            cmbMod_Gruppo.SelectedIndex = 0;
            cbMod_Disponibile.Checked = false;
            cbMod_Garanzia.Checked = false;

        }
        private void AttivaDisattivaModificaAttrezzatura(bool attivaModifica)
        {
            tbMod_CodiceVideoSystem.ReadOnly = attivaModifica;
            tbMod_DataAcquisto.ReadOnly = attivaModifica;
            tbMod_Descrizione.ReadOnly = attivaModifica;
            tbMod_Marca.ReadOnly = attivaModifica;
            tbMod_Modello.ReadOnly = attivaModifica;
            tbMod_Marca.ReadOnly = attivaModifica;
            tbMod_Modello.ReadOnly = attivaModifica;
            tbMod_Note.ReadOnly = attivaModifica;
            tbMod_Seriale.ReadOnly = attivaModifica;

            if (attivaModifica)
            {
                cmbMod_Categoria.Attributes.Add("disabled", "");
                cmbMod_SubCategoria.Attributes.Add("disabled", "");
                cmbMod_Posizione.Attributes.Add("disabled", "");
                cmbMod_Gruppo.Attributes.Add("disabled", "");
            }
            else
            {
                cmbMod_Categoria.Attributes.Remove("disabled");
                cmbMod_SubCategoria.Attributes.Remove("disabled");
                cmbMod_Posizione.Attributes.Remove("disabled");
                cmbMod_Gruppo.Attributes.Remove("disabled");
            }

            cbMod_Disponibile.Enabled = !attivaModifica;
            cbMod_Garanzia.Enabled = !attivaModifica;
        }
        private void GestisciPulsantiAttrezzatura(string stato)
        {
            switch (stato)
            {
                case "VISUALIZZAZIONE":

                    btnInserisciAttrezzatura.Visible = false;
                    btnModificaAttrezzatura.Visible = false;
                    btnEliminaAttrezzatura.Visible = false;
                    btnAnnullaAttrezzatura.Visible = false;
                    if (!basePage.AbilitazioneInScrittura())
                    {
                        btnGestisciAttrezzatura.Visible = false;
                    }
                    else
                    {
                        btnGestisciAttrezzatura.Visible = true;
                    }

                    break;
                case "INSERIMENTO":
                    btnInserisciAttrezzatura.Visible = true;
                    btnModificaAttrezzatura.Visible = false;
                    btnEliminaAttrezzatura.Visible = false;
                    btnAnnullaAttrezzatura.Visible = false;
                    btnGestisciAttrezzatura.Visible = false;
                    break;
                case "MODIFICA":
                    btnInserisciAttrezzatura.Visible = false;
                    btnModificaAttrezzatura.Visible = true;
                    btnEliminaAttrezzatura.Visible = true;
                    btnAnnullaAttrezzatura.Visible = true;
                    btnGestisciAttrezzatura.Visible = false;

                    break;
                case "ANNULLAMENTO":
                    btnInserisciAttrezzatura.Visible = false;
                    btnModificaAttrezzatura.Visible = false;
                    btnEliminaAttrezzatura.Visible = false;
                    btnAnnullaAttrezzatura.Visible = false;
                    btnGestisciAttrezzatura.Visible = true;

                    break;
                default:
                    btnInserisciAttrezzatura.Visible = false;
                    btnModificaAttrezzatura.Visible = false;
                    btnEliminaAttrezzatura.Visible = false;
                    btnAnnullaAttrezzatura.Visible = false;
                    btnGestisciAttrezzatura.Visible = true;

                    break;
            }

        }
        private void EditAttrezzaturaVuota()
        {
            Esito esito = new Esito();
            PulisciCampiDettaglio();
        }
        private AttrezzatureMagazzino CreaOggettoAttrezzatura(ref Esito esito)
        {
            AttrezzatureMagazzino attrezzatura = new AttrezzatureMagazzino();

            if (string.IsNullOrEmpty((string)ViewState["idAttrezzatura"]))
            {
                ViewState["idAttrezzatura"] = "0";
            }

            attrezzatura.Id = Convert.ToInt16(ViewState["idAttrezzatura"].ToString());

            attrezzatura.Cod_vs = BasePage.ValidaCampo(tbMod_CodiceVideoSystem, "",false, ref esito);
            attrezzatura.Data_acquisto = BasePage.ValidaCampo(tbMod_DataAcquisto, DateTime.Now, true, ref esito);
            attrezzatura.Descrizione = BasePage.ValidaCampo(tbMod_Descrizione, "", true, ref esito);
            attrezzatura.Disponibile = cbMod_Disponibile.Checked;
            attrezzatura.Garanzia = cbMod_Garanzia.Checked;
            attrezzatura.Id_categoria = Convert.ToInt32(cmbMod_Categoria.SelectedValue);
            attrezzatura.Id_posizione_magazzino = Convert.ToInt32(cmbMod_Posizione.SelectedValue);
            attrezzatura.Id_gruppo_magazzino = Convert.ToInt32(cmbMod_Gruppo.SelectedValue);
            if (!string.IsNullOrEmpty(cmbMod_SubCategoria.SelectedValue)) attrezzatura.Id_subcategoria = Convert.ToInt32(cmbMod_SubCategoria.SelectedValue);
            attrezzatura.Marca = BasePage.ValidaCampo(tbMod_Marca, "", true, ref esito);
            attrezzatura.Modello = BasePage.ValidaCampo(tbMod_Modello, "", true, ref esito);
            attrezzatura.Seriale = BasePage.ValidaCampo(tbMod_Seriale, "", false, ref esito);
            attrezzatura.Note = BasePage.ValidaCampo(tbMod_Note, "", false, ref esito);
            attrezzatura.Attivo = true;

            return attrezzatura;
        }

        protected void ddlTipoCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            string idCategoria = ddlTipoCategoria.SelectedItem.Value;
            if (string.IsNullOrEmpty(idCategoria)) idCategoria = "0";
            riempiComboSubCategoria(Convert.ToInt32(idCategoria));
        }
    }
}