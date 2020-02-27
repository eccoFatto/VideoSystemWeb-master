using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoSystemWeb.BLL;
using VideoSystemWeb.Entity;
using System.Data;
using System.Configuration;
using VideoSystemWeb.DAL;

using iText;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iTextSharp;

namespace VideoSystemWeb.MAGAZZINO
{
    public partial class Magazzino : BasePage
    {
        #region ELENCO CHIAVI VIEWSTATE
        private const string VIEWSTATE_NOTELAVORAZIONEMAGAZZINO_CORRENTE = "noteLavorazioneMagazzinoCorrente";
        private const string VIEWSTATE_DATILAVORAZIONEMAGAZZINO_CORRENTE = "datiLavorazioneMagazzinoCorrente";
        private const string VIEWSTATE_IDLAVORAZIONE_CORRENTE = "idLavorazioneCorrente";
        #endregion

        public NoteLavorazioneMagazzino NoteLavorazioneMagazzinoCorrente
        {
            get
            {
                return (NoteLavorazioneMagazzino)ViewState[VIEWSTATE_NOTELAVORAZIONEMAGAZZINO_CORRENTE];
            }
            set
            {
                ViewState[VIEWSTATE_NOTELAVORAZIONEMAGAZZINO_CORRENTE] = value;
            }
        }

        public DatiLavorazioneMagazzino DatiLavorazioneMagazzinoCorrente
        {
            get
            {
                return (DatiLavorazioneMagazzino)ViewState[VIEWSTATE_DATILAVORAZIONEMAGAZZINO_CORRENTE];
            }
            set
            {
                ViewState[VIEWSTATE_DATILAVORAZIONEMAGAZZINO_CORRENTE] = value;
            }
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            CheckIsMobile();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["idDatiAgenda"] != null)
                {
                    try
                    {
                        Esito esito = new Esito();

                        int idDatiAgenda = int.Parse(Request.QueryString["idDatiAgenda"]);
                        DatiAgenda eventoCorrente = CaricaEventoCorrente(idDatiAgenda);

                        PopolaIntestazionePagina(eventoCorrente);

                        int idLavorazione = Dati_Lavorazione_BLL.Instance.getDatiLavorazioneByIdEvento(idDatiAgenda, ref esito).Id;
                        ViewState[VIEWSTATE_IDLAVORAZIONE_CORRENTE] = idLavorazione;
                        NoteLavorazioneMagazzinoCorrente = GetNoteLavorazioneMagazzino(idLavorazione);
                        txt_Note.Text = NoteLavorazioneMagazzinoCorrente.Note;
                        cercaRigheLavorazioneMagazzino();

                    }
                    catch (Exception ex)
                    {
                        Esito esito = new Esito();
                        esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                        esito.Descrizione = ex.Message + Environment.NewLine + ex.StackTrace;
                        ShowError(esito.Descrizione);
                    }
                }
                else
                {
                    ShowError("Nessuna lavorazione selezionata");
                }
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "chiudiLoader", script: "$('.loader').hide();", addScriptTags: true);
        }

        protected void cercaRigheLavorazioneMagazzino()
        {
            string queryRicerca = "SELECT * FROM dati_lavorazione_magazzino where attivo = 1 and id_Lavorazione = " + ViewState[VIEWSTATE_IDLAVORAZIONE_CORRENTE].ToString();
            Esito esito = new Esito();
            DataTable dtAttrezzature = Base_DAL.GetDatiBySql(queryRicerca, ref esito);
            gv_attrezzature.DataSource = dtAttrezzature;
            gv_attrezzature.DataBind();
        }
        // Prendo la NotaLavorazioneMagazzino per idLavorazione
        // Se non esiste la creo e la inserisco in tabella
        private NoteLavorazioneMagazzino GetNoteLavorazioneMagazzino(int idLavorazione)
        {
            Esito esito = new Esito();

            NoteLavorazioneMagazzino noteLavorazioneMagazzino = Note_Lavorazione_Magazzino_BLL.Instance.getNoteLavorazioneMagazzinoByIdLavorazione(idLavorazione, ref esito);
            if (noteLavorazioneMagazzino == null || noteLavorazioneMagazzino.Id == 0)
            {
                noteLavorazioneMagazzino = new NoteLavorazioneMagazzino
                {
                    Id_Lavorazione = idLavorazione,
                    Note = string.Empty,
                    Attivo = true
                };

                Note_Lavorazione_Magazzino_BLL.Instance.CreaNoteLavorazioneMagazzino(noteLavorazioneMagazzino, ref esito);
            }

            return noteLavorazioneMagazzino;
        }

        private DatiLavorazioneMagazzino GetDatiLavorazioneMagazzino(int idLavorazioneMagazzino)
        {
            Esito esito = new Esito();

            DatiLavorazioneMagazzino datiLavorazioneMagazzino = Dati_Lavorazione_Magazzino_BLL.Instance.getDatiLavorazioneMagazzinoById(idLavorazioneMagazzino, ref esito);
            if (datiLavorazioneMagazzino == null || datiLavorazioneMagazzino.Id == 0 || esito.Codice!=0)
            {
                ShowError("Errore durante la ricerca dei Dati Lavorazione Magazzino con ID " + idLavorazioneMagazzino.ToString() + " " + esito.Descrizione);
            }

            return datiLavorazioneMagazzino;
        }

        private DatiAgenda CaricaEventoCorrente(int idDatiAgenda)
        {
            Esito esito = new Esito();
            List<DatiAgenda> listaDatiAgenda = Agenda_BLL.Instance.CaricaDatiAgenda(ref esito);

            return Agenda_BLL.Instance.GetDatiAgendaById(listaDatiAgenda, idDatiAgenda);
        }

        private void PopolaIntestazionePagina(DatiAgenda eventoCorrente)
        {
            lbl_Cliente.Text = eventoCorrente.DecodificaCliente;
            lbl_Lavorazione.Text = eventoCorrente.lavorazione;
            lbl_Produzione.Text = eventoCorrente.produzione;
            lbl_CodLavoro.Text = eventoCorrente.codice_lavoro;
            lbl_DataInizio.Text = eventoCorrente.data_inizio_lavorazione.ToString("dd/MM/yyyy");
            lbl_DataFine.Text = eventoCorrente.data_fine_lavorazione.ToString("dd/MM/yyyy");
            lbl_Tipologia.Text = eventoCorrente.DecodificaTipologia;
        }

        protected void gv_attrezzature_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string id = e.Row.Cells[GetColumnIndexByName(e.Row,"id")].Text;
                
                for (int indiceColonna = 2; indiceColonna < gv_attrezzature.Columns.Count; indiceColonna++)
                {

                    string nomeCampo = GetColumnNameByIndex(e.Row, indiceColonna);
                    char[] separatore = new char[] {';'};
                    string[] arNomiCampo = nomeCampo.Split(separatore, StringSplitOptions.RemoveEmptyEntries);
                    string valoreSelezionato = e.Row.Cells[indiceColonna].Text.Replace("&nbsp;","");

                    e.Row.Cells[indiceColonna].Attributes["ondblclick"] = "mostracella('" + id + "', '" + indiceColonna.ToString() + "', '" + arNomiCampo[0] + "', '" + arNomiCampo[1] + "', '" + valoreSelezionato.Trim() + "'); ";
                }
            }
        }



        protected void gv_attrezzature_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void btnInserisciRiga_Click(object sender, EventArgs e)
        {
            DatiLavorazioneMagazzino datiLavorazioneMagazzino = new DatiLavorazioneMagazzino();

            int totRighe = gv_attrezzature.Rows.Count;

            datiLavorazioneMagazzino.Descrizione_Camera = "Camera " + (totRighe+1).ToString();
            datiLavorazioneMagazzino.Id_Lavorazione = (int)(ViewState[VIEWSTATE_IDLAVORAZIONE_CORRENTE]);
            datiLavorazioneMagazzino.Attivo = true;

            datiLavorazioneMagazzino.Id_Altro1 = 0;
            datiLavorazioneMagazzino.Id_Altro2 = 0;
            datiLavorazioneMagazzino.Id_Camera = 0;
            datiLavorazioneMagazzino.Id_Cavalletto = 0;
            datiLavorazioneMagazzino.Id_Cavi = 0;
            datiLavorazioneMagazzino.Id_Fibra_Trax = 0;
            datiLavorazioneMagazzino.Id_Lensholder = 0;
            datiLavorazioneMagazzino.Id_Loop = 0;
            datiLavorazioneMagazzino.Id_Mic = 0;
            datiLavorazioneMagazzino.Id_Ottica = 0;
            datiLavorazioneMagazzino.Id_Testa = 0;
            datiLavorazioneMagazzino.Id_Viewfinder = 0;

            datiLavorazioneMagazzino.Nome_Altro1 = "";
            datiLavorazioneMagazzino.Nome_Altro2 = "";
            datiLavorazioneMagazzino.Nome_Camera = "";
            datiLavorazioneMagazzino.Nome_Cavalletto = "";
            datiLavorazioneMagazzino.Nome_Cavi = "";
            datiLavorazioneMagazzino.Nome_Fibra_Trax = "";
            datiLavorazioneMagazzino.Nome_Lensholder = "";
            datiLavorazioneMagazzino.Nome_Loop = "";
            datiLavorazioneMagazzino.Nome_Mic = "";
            datiLavorazioneMagazzino.Nome_Ottica = "";
            datiLavorazioneMagazzino.Nome_Testa = "";
            datiLavorazioneMagazzino.Nome_Viewfinder = "";

            Esito esito = new Esito();
            Dati_Lavorazione_Magazzino_BLL.Instance.CreaDatiLavorazioneMagazzino(datiLavorazioneMagazzino, ref esito);

            if (esito.Codice == 0) {
                cercaRigheLavorazioneMagazzino();
            }
        }

        protected void btnAggiornaNote_Click(object sender, EventArgs e)
        {
            try
            {
                NoteLavorazioneMagazzinoCorrente.Note = txt_Note.Text.Trim();

                Esito esito = Note_Lavorazione_Magazzino_BLL.Instance.AggiornaNoteLavorazioneMagazzino(NoteLavorazioneMagazzinoCorrente);
                if (esito.Codice == 0)
                {
                    ShowSuccess("Note salvate correttamente!");
                }
                else
                {
                    ShowError("Errore nel salvataggio Note: " + esito.Descrizione);
                }
            }
            catch (Exception ex)
            {
                ShowError("Errore nel salvataggio Note: " + ex.Message);
            }

        }

        protected void gv_attrezzature_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ELIMINA_RIGA")
            {
                try
                {
                    Int64 id = Convert.ToInt64(e.CommandArgument);

                    Esito esito = Dati_Lavorazione_Magazzino_BLL.Instance.EliminaDatiLavorazioneMagazzino((int)id);
                    if (esito.Codice == 0)
                    {
                        cercaRigheLavorazioneMagazzino();
                        ShowSuccess("Riga eliminata correttamente!");
                    }
                    else
                    {
                        ShowError("Errore nella cancellazione riga: " + esito.Descrizione);
                    }
                }
                catch (Exception ex)
                {
                    ShowError("Errore nella cancellazione riga: " + ex.Message);
                }
            }
        }

        protected void btnOpenCell_Click(object sender, EventArgs e)
        {

        }

        protected void btnEditEvent_Click(object sender, EventArgs e)
        {
            int idLavorazioneMagazzino = int.Parse(hfIdRiga.Value);

            DatiLavorazioneMagazzinoCorrente = GetDatiLavorazioneMagazzino(idLavorazioneMagazzino);

            int idColonna = int.Parse(hfIdColonna.Value);


            ScriptManager.RegisterStartupScript(this, typeof(Page), "apritab", "openTabMagazzino('Magazzino');", true);


            MostraPopup();
        }

        private void MostraPopup()
        {

            pnlContainer.Style.Remove("display");

            Esito esito = new Esito();


            lblNumeroColonna.Text = hfIdColonna.Value;
            lblNumeroRiga.Text = hfIdRiga.Value;
            lblNomeCampo.Text = hfNomeCampo.Value;
            lblHeaderCampo.Text = hfHeaderCampo.Value;

            switch (hfNomeCampo.Value)
            {
                case "descrizione_Camera":
                    tbModDescrizioneCamera.Text = hfValoreGriglia.Value.Trim();
                    phDescrizioneCamera.Visible = true;
                    phModificaAttrezzature.Visible = false;
                    break;
                default:
                    caricaListe();
                    gvAttrezzatureDaInserire.DataSource = new DataTable();
                    gvAttrezzatureDaInserire.DataBind();
                    phDescrizioneCamera.Visible = false;
                    phModificaAttrezzature.Visible = true;
                    break;
            }

        }

        protected void btnChiudiPopupServer_Click(object sender, EventArgs e)
        {
            pnlContainer.Style.Add("display","none");
            cercaRigheLavorazioneMagazzino();
        }

        protected void btnAggiornaDescrizioneCamera_Click(object sender, EventArgs e)
        {
            try
            {
                DatiLavorazioneMagazzinoCorrente.Descrizione_Camera = tbModDescrizioneCamera.Text.Trim();

                Esito esito = Dati_Lavorazione_Magazzino_BLL.Instance.AggiornaDatiLavorazioneMagazzino(DatiLavorazioneMagazzinoCorrente);
                if (esito.Codice == 0)
                {
                    ShowSuccess("Descrizione riga Lavorazione Magazzino salvata correttamente!");
                }
                else
                {
                    ShowError("Errore nel salvataggio Descrizione riga Lavorazione Magazzino: " + esito.Descrizione);
                }
            }
            catch (Exception ex)
            {
                ShowError("Errore nel salvataggio Descrizione riga Lavorazione Magazzino: " + ex.Message);
            }

            btnChiudiPopupServer_Click(null, null);
        }

        protected void ddlTipoCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            string idCategoria = ddlTipoCategoria.SelectedItem.Value;
            if (string.IsNullOrEmpty(idCategoria)) idCategoria = "0";
            riempiComboSubCategoria(Convert.ToInt32(idCategoria));
        }

        protected void ddlTipoSubCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            string idSubCategoria = ddlTipoSubCategoria.SelectedItem.Value;
            if (string.IsNullOrEmpty(idSubCategoria)) idSubCategoria = "0";
            riempiComboGruppo(Convert.ToInt32(idSubCategoria));
        }

        protected void btnRicercaAttrezzatura_Click(object sender, EventArgs e)
        {
            string queryRicerca = ConfigurationManager.AppSettings["QUERY_SEARCH_LAVORAZIONE_MAGAZZINO"];

            queryRicerca = queryRicerca.Replace("@codiceVS", tbCodiceVS.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@descrizione", tbDescrizione.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@marca", tbMarca.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@modello", tbModello.Text.Trim().Replace("'", "''"));
            queryRicerca = queryRicerca.Replace("@seriale", tbSeriale.Text.Trim().Replace("'", "''"));

            // SELEZIONO I CAMPI DROPDOWN SE VALORIZZATI
            string queryRicercaCampiDropDown = "";
            if (!string.IsNullOrEmpty(ddlTipoCategoria.SelectedItem.Text))
            {
                queryRicercaCampiDropDown += " and cat.id=" + ddlTipoCategoria.SelectedValue + " ";
            }
            if (!string.IsNullOrEmpty(ddlTipoSubCategoria.SelectedItem.Text))
            {
                queryRicercaCampiDropDown += " and sub.id=" + ddlTipoSubCategoria.SelectedValue + " ";
            }
            if (!string.IsNullOrEmpty(ddlTipoGruppoMagazzino.SelectedItem.Text))
            {
                queryRicercaCampiDropDown += " and gruppo.id=" + ddlTipoGruppoMagazzino.SelectedValue + " ";
            }
            if (!string.IsNullOrEmpty(ddlTipoPosizioneMagazzino.SelectedItem.Text))
            {
                queryRicercaCampiDropDown += " and pos.nome='" + ddlTipoPosizioneMagazzino.SelectedItem.Text.Replace("'", "''") + "' ";
            }


            queryRicerca = queryRicerca.Replace("@campiTendina", queryRicercaCampiDropDown.Trim());

            // NON PROPONGO GLI ID ATTREZZATURA OCCUPATI IN QUESTO PERIODO
            int idDatiAgenda = int.Parse(Request.QueryString["idDatiAgenda"]);
            DatiAgenda eventoCorrente = CaricaEventoCorrente(idDatiAgenda);
            string elencoIdAttrezzatureOccupate = trovaIdAttrezzatureInUso(eventoCorrente.data_inizio_lavorazione, eventoCorrente.data_fine_lavorazione);
            string notInIdAttrezzatureOccupate = "";
            if (!string.IsNullOrEmpty(elencoIdAttrezzatureOccupate)) notInIdAttrezzatureOccupate = " and att.id not in (" + elencoIdAttrezzatureOccupate + ") ";
            queryRicerca = queryRicerca.Replace("@idAttrezzatureOccupate", notInIdAttrezzatureOccupate);
            


            Esito esito = new Esito();
            DataTable dtAttrezzature = Base_DAL.GetDatiBySql(queryRicerca, ref esito);
            gvAttrezzatureDaInserire.DataSource = dtAttrezzature;
            gvAttrezzatureDaInserire.DataBind();
        }

        protected void gvAttrezzatureDaInserire_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string id = e.Row.Cells[GetColumnIndexByName(e.Row, "id")].Text;

                for (int indiceColonna = 0; indiceColonna < e.Row.Cells.Count; indiceColonna++)
                {

                    string nomeCampo = GetColumnNameByIndex(e.Row, indiceColonna);
                    char[] separatore = new char[] { ';' };
                    string[] arNomiCampo = nomeCampo.Split(separatore, StringSplitOptions.RemoveEmptyEntries);
                    string valoreSelezionato = e.Row.Cells[indiceColonna].Text.Replace("&nbsp;", "");

                    if(arNomiCampo[0]== "Descrizione" || arNomiCampo[0] == "Modello" || arNomiCampo[0] == "Note") { 
                        e.Row.Cells[indiceColonna].Attributes["ondblclick"] = "selezionaAttrezzaturaDaInserire('" + id + "', '" + valoreSelezionato.Trim() + "'); ";
                    }
                }
            }
        }

        protected void gvAttrezzatureDaInserire_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAttrezzatureDaInserire.PageIndex = e.NewPageIndex;
            btnRicercaAttrezzatura_Click(null, null);
        }

        private void riempiComboSubCategoria(int idCategoria)
        {
            ddlTipoSubCategoria.Items.Clear();
            ddlTipoSubCategoria.Items.Add("");

            string queryRicercaSubcategoria = "select * from tipo_subcategoria_magazzino where attivo = 1 ";
            if (idCategoria > 0)
            {
                queryRicercaSubcategoria += "and id in (select distinct id_subcategoria from mag_attrezzature where id_categoria=" + idCategoria.ToString() + ") ";
            }
            queryRicercaSubcategoria += "order by nome";
            Esito esito = new Esito();
            DataTable dtSubCategorie = Base_DAL.GetDatiBySql(queryRicercaSubcategoria, ref esito);

            foreach (DataRow tipologiaSubCategoria in dtSubCategorie.Rows)
            {
                System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem
                {
                    Text = tipologiaSubCategoria["nome"].ToString(),
                    Value = tipologiaSubCategoria["id"].ToString()
                };

                ddlTipoSubCategoria.Items.Add(item);

            }
        }
        private void riempiComboGruppo(int idSubCategoria)
        {
            ddlTipoGruppoMagazzino.Items.Clear();
            ddlTipoGruppoMagazzino.Items.Add("");

            string queryRicercaGruppo = "select * from tipo_gruppo_magazzino where attivo = 1 ";
            if (idSubCategoria > 0)
            {
                queryRicercaGruppo += "and id in (select distinct id_gruppo_magazzino from mag_attrezzature where id_subcategoria=" + idSubCategoria.ToString() + ") ";
            }
            queryRicercaGruppo += "order by nome";
            Esito esito = new Esito();
            DataTable dtGruppi = Base_DAL.GetDatiBySql(queryRicercaGruppo, ref esito);

            foreach (DataRow tipologiaGruppo in dtGruppi.Rows)
            {
                System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem
                {
                    Text = tipologiaGruppo["nome"].ToString(),
                    Value = tipologiaGruppo["id"].ToString()
                };

                ddlTipoGruppoMagazzino.Items.Add(item);

            }
        }

        private void caricaListe()
        {
            ddlTipoCategoria.Items.Clear();
            ddlTipoCategoria.Items.Add("");
            foreach (Tipologica tipologiaCategoria in SessionManager.ListaTipiCategorieMagazzino)
            {
                System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem
                {
                    Text = tipologiaCategoria.nome,
                    Value = tipologiaCategoria.id.ToString()
                };

                ddlTipoCategoria.Items.Add(item);
            }

            riempiComboSubCategoria(0);

            ddlTipoPosizioneMagazzino.Items.Clear();
            ddlTipoPosizioneMagazzino.Items.Add("");
            foreach (Tipologica tipologiaPosizione in SessionManager.ListaTipiPosizioniMagazzino)
            {
                System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem
                {
                    Text = tipologiaPosizione.nome,
                    Value = tipologiaPosizione.nome
                };

                ddlTipoPosizioneMagazzino.Items.Add(item);

            }


            riempiComboGruppo(0);

        }

        protected void btnAggiornaRigaLavorazioneMagazzino_Click(object sender, EventArgs e)
        {
            //AGGIORNO I DATI SULLA RIGA SELEZIONATA
            string idMagazzino = hfIdAttrezzaturaDaModificare.Value;
            string valoreMagazzino = hfValoreAttrezzaturaDaModificare.Value.Replace("&quot;","").Replace("&nbsp;"," ");
            string campoDaModificare = hfNomeCampo.Value;
            string idCampoDaModificare = campoDaModificare.Replace("nome_", "id_");

            switch (campoDaModificare)
            {
                case "nome_Camera":
                    DatiLavorazioneMagazzinoCorrente.Nome_Camera = valoreMagazzino;
                    DatiLavorazioneMagazzinoCorrente.Id_Camera = int.Parse(idMagazzino);
                    break;
                case "nome_Fibra_Trax":
                    DatiLavorazioneMagazzinoCorrente.Nome_Fibra_Trax = valoreMagazzino;
                    DatiLavorazioneMagazzinoCorrente.Id_Fibra_Trax = int.Parse(idMagazzino);
                    break;
                case "nome_Ottica":
                    DatiLavorazioneMagazzinoCorrente.Nome_Ottica = valoreMagazzino;
                    DatiLavorazioneMagazzinoCorrente.Id_Ottica = int.Parse(idMagazzino);
                    break;
                case "nome_Viewfinder":
                    DatiLavorazioneMagazzinoCorrente.Nome_Viewfinder = valoreMagazzino;
                    DatiLavorazioneMagazzinoCorrente.Id_Viewfinder = int.Parse(idMagazzino);
                    break;
                case "nome_Loop":
                    DatiLavorazioneMagazzinoCorrente.Nome_Loop = valoreMagazzino;
                    DatiLavorazioneMagazzinoCorrente.Id_Loop = int.Parse(idMagazzino);
                    break;
                case "nome_Mic":
                    DatiLavorazioneMagazzinoCorrente.Nome_Mic = valoreMagazzino;
                    DatiLavorazioneMagazzinoCorrente.Id_Mic = int.Parse(idMagazzino);
                    break;
                case "nome_Testa":
                    DatiLavorazioneMagazzinoCorrente.Nome_Testa = valoreMagazzino;
                    DatiLavorazioneMagazzinoCorrente.Id_Testa = int.Parse(idMagazzino);
                    break;
                case "nome_Lensholder":
                    DatiLavorazioneMagazzinoCorrente.Nome_Lensholder = valoreMagazzino;
                    DatiLavorazioneMagazzinoCorrente.Id_Lensholder = int.Parse(idMagazzino);
                    break;
                case "nome_Cavalletto":
                    DatiLavorazioneMagazzinoCorrente.Nome_Cavalletto = valoreMagazzino;
                    DatiLavorazioneMagazzinoCorrente.Id_Cavalletto = int.Parse(idMagazzino);
                    break;
                case "nome_Cavi":
                    DatiLavorazioneMagazzinoCorrente.Nome_Cavi = valoreMagazzino;
                    DatiLavorazioneMagazzinoCorrente.Id_Cavi = int.Parse(idMagazzino);
                    break;
                case "nome_Altro1":
                    DatiLavorazioneMagazzinoCorrente.Nome_Altro1 = valoreMagazzino;
                    DatiLavorazioneMagazzinoCorrente.Id_Altro1 = int.Parse(idMagazzino);
                    break;
                case "nome_Altro2":
                    DatiLavorazioneMagazzinoCorrente.Nome_Altro2 = valoreMagazzino;
                    DatiLavorazioneMagazzinoCorrente.Id_Altro2 = int.Parse(idMagazzino);
                    break;
                default:
                    break;
            }
            try
            {

                Esito esito = Dati_Lavorazione_Magazzino_BLL.Instance.AggiornaDatiLavorazioneMagazzino(DatiLavorazioneMagazzinoCorrente);
                if (esito.Codice == 0)
                {
                    //cercaRigheLavorazioneMagazzino();
                    ShowSuccess("Riga aggiornata correttamente!");
                }
                else
                {
                    ShowError("Errore nell'aggiornamento riga: " + esito.Descrizione);
                }
            }
            catch (Exception ex)
            {
                ShowError("Errore nell'aggiornamento riga: " + ex.Message);
            }

            btnChiudiPopupServer_Click(null, null);
        }

        private string trovaIdAttrezzatureInUso(DateTime dataDa, DateTime dataA)
        {
            string ret = "";
            try
            {
                string queryRicercaIdInUso = "select lm.id_Altro1,lm.id_Altro2,lm.id_Camera,lm.id_Cavalletto,lm.id_Cavi,lm.id_Fibra_Trax,id_Lensholder,lm.id_Loop, " +
                "lm.id_Mic,lm.id_Ottica, lm.id_Testa, lm.id_Viewfinder " +
                ", ag.data_inizio_lavorazione,ag.data_fine_lavorazione,ag.lavorazione,ag.produzione " +
                "from[dbo].[dati_lavorazione_magazzino] lm " +
                "left join[dbo].[dati_lavorazione] lav " +
                "on lm.id_Lavorazione = lav.id " +
                "left join tab_dati_agenda ag " +
                "on lav.idDatiAgenda= ag.id " +
                "where lm.attivo=1 " +
                //"and ag.data_inizio_impegno>='2020-02-22T00:00:00.000' AND ag.data_fine_lavorazione<='2020-02-23T00:00:00.000'";
                "and ag.data_inizio_impegno>='" + dataDa.ToString("yyyy-MM-ddT00:00:00.000") + "' AND ag.data_fine_lavorazione<='" + dataA.ToString("yyyy-MM-ddT00:00:00.000") + "'";
                Esito esito = new Esito();
                DataTable dtIdAttrezzatureInUso = Base_DAL.GetDatiBySql(queryRicercaIdInUso, ref esito);
                if (esito.Codice == 0)
                {
                    if (dtIdAttrezzatureInUso != null && dtIdAttrezzatureInUso.Rows != null && dtIdAttrezzatureInUso.Rows.Count > 0)
                    {
                        foreach (DataRow rigaLavorazioneMagazzino in dtIdAttrezzatureInUso.Rows)
                        {
                            if (rigaLavorazioneMagazzino["id_Altro1"].ToString() != "0") ret += rigaLavorazioneMagazzino["id_Altro1"].ToString() + ",";
                            if (rigaLavorazioneMagazzino["id_Altro2"].ToString() != "0") ret += rigaLavorazioneMagazzino["id_Altro2"].ToString() + ",";
                            if (rigaLavorazioneMagazzino["id_Camera"].ToString() != "0") ret += rigaLavorazioneMagazzino["id_Camera"].ToString() + ",";
                            if (rigaLavorazioneMagazzino["id_Cavalletto"].ToString() != "0") ret += rigaLavorazioneMagazzino["id_Cavalletto"].ToString() + ",";
                            if (rigaLavorazioneMagazzino["id_Cavi"].ToString() != "0") ret += rigaLavorazioneMagazzino["id_Cavi"].ToString() + ",";
                            if (rigaLavorazioneMagazzino["id_Fibra_Trax"].ToString() != "0") ret += rigaLavorazioneMagazzino["id_Fibra_Trax"].ToString() + ",";
                            if (rigaLavorazioneMagazzino["id_Lensholder"].ToString() != "0") ret += rigaLavorazioneMagazzino["id_Lensholder"].ToString() + ",";
                            if (rigaLavorazioneMagazzino["id_Loop"].ToString() != "0") ret += rigaLavorazioneMagazzino["id_Loop"].ToString() + ",";
                            if (rigaLavorazioneMagazzino["id_Mic"].ToString() != "0") ret += rigaLavorazioneMagazzino["id_Mic"].ToString() + ",";
                            if (rigaLavorazioneMagazzino["id_Ottica"].ToString() != "0") ret += rigaLavorazioneMagazzino["id_Ottica"].ToString() + ",";
                            if (rigaLavorazioneMagazzino["id_Testa"].ToString() != "0") ret += rigaLavorazioneMagazzino["id_Testa"].ToString() + ",";
                            if (rigaLavorazioneMagazzino["id_Viewfinder"].ToString() != "0") ret += rigaLavorazioneMagazzino["id_Viewfinder"].ToString() + ",";
                        }
                        if(ret.Length>0 && ret.Substring(ret.Length - 1) == ",")
                        {
                            ret = ret.Substring(0, ret.Length - 1);
                        }
                    }
                }
                else
                {
                    ShowError("trovaIdAttrezzatureInUso: " + esito.Descrizione);
                }
            }
            catch (Exception ex)
            {

                ShowError("trovaIdAttrezzatureInUso: " + ex.Message);
            }
            return ret;
        }

        protected void btnStampa_Click(object sender, EventArgs e)
        {
            try
            {
                Esito esito = new Esito();

                int idDatiAgenda = int.Parse(Request.QueryString["idDatiAgenda"]);
                DatiAgenda eventoCorrente = CaricaEventoCorrente(idDatiAgenda);

                DateTime dataInizio = eventoCorrente.data_inizio_lavorazione;
                DateTime dataFine = eventoCorrente.data_fine_lavorazione;

                int idLavorazione = Dati_Lavorazione_BLL.Instance.getDatiLavorazioneByIdEvento(idDatiAgenda, ref esito).Id;
                List<DatiLavorazioneMagazzino> listaDatiLavorazioneMagazzino = Dati_Lavorazione_Magazzino_BLL.Instance.getDatiLavorazioneMagazzinoByIdLavorazione(idLavorazione, ref esito);

                if (esito.Codice == 0 && listaDatiLavorazioneMagazzino != null && listaDatiLavorazioneMagazzino.Count > 0)
                {
                    // LEGGO I PARAMETRI DI VS
                    Config cfAppo = Config_BLL.Instance.getConfig(ref esito, "PARTITA_IVA");
                    string pIvaVs = cfAppo.valore;
                    cfAppo = Config_BLL.Instance.getConfig(ref esito, "DENOMINAZIONE");
                    string denominazioneVs = cfAppo.valore;
                    cfAppo = Config_BLL.Instance.getConfig(ref esito, "TOPONIMO");
                    string toponimoVs = cfAppo.valore;
                    cfAppo = Config_BLL.Instance.getConfig(ref esito, "INDIRIZZO");
                    string indirizzoVs = cfAppo.valore;
                    cfAppo = Config_BLL.Instance.getConfig(ref esito, "CIVICO");
                    string civicoVs = cfAppo.valore;
                    cfAppo = Config_BLL.Instance.getConfig(ref esito, "CAP");
                    string capVs = cfAppo.valore;
                    cfAppo = Config_BLL.Instance.getConfig(ref esito, "CITTA");
                    string cittaVs = cfAppo.valore;
                    cfAppo = Config_BLL.Instance.getConfig(ref esito, "PROVINCIA");
                    string provinciaVs = cfAppo.valore;
                    cfAppo = Config_BLL.Instance.getConfig(ref esito, "EMAIL");
                    string emailVs = cfAppo.valore;

                    // export SU PDF

                    string nomeFile = "Report_Lavorazione_Magazzino.pdf";
                    string pathReport = ConfigurationManager.AppSettings["PATH_DOCUMENTI_REPORT"] + nomeFile;
                    string mapPathReport = MapPath(ConfigurationManager.AppSettings["PATH_DOCUMENTI_REPORT"]) + nomeFile;

                    //string prefissoUrl = Request.Url.Scheme + "://" + Request.Url.Authority;
                    iText.IO.Image.ImageData imageData = iText.IO.Image.ImageDataFactory.Create(MapPath("~/Images/logoVSP_trim.png"));


                    PdfWriter wr = new PdfWriter(mapPathReport);
                    PdfDocument doc = new PdfDocument(wr);
                    doc.SetDefaultPageSize(iText.Kernel.Geom.PageSize.A4.Rotate());
                    //doc.SetDefaultPageSize(iText.Kernel.Geom.PageSize.A4);
                    Document document = new Document(doc, iText.Kernel.Geom.PageSize.A4.Rotate(), false);

                    document.SetMargins(50, 30, 50, 30);

                    //iText.Kernel.Colors.Color coloreIntestazioni = new iText.Kernel.Colors.DeviceRgb(0, 225, 0);
                    // COLORE BLU VIDEOSYSTEM
                    iText.Kernel.Colors.Color coloreIntestazioni = new iText.Kernel.Colors.DeviceRgb(33, 150, 243);

                    // AGGIUNGO TABLE PER LAYOUT INTESTAZIONE
                    iText.Layout.Element.Table tbIntestazione = new iText.Layout.Element.Table(new float[] { 1, 9 }).UseAllAvailableWidth().SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    iText.Layout.Element.Image image = new iText.Layout.Element.Image(imageData).ScaleAbsolute(100, 70).SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
                    Cell cellaImmagine = new Cell().SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE).SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
                    cellaImmagine.Add(image);
                    tbIntestazione.AddCell(cellaImmagine);

                    iText.Layout.Element.Table tbIntestazioneDx = new iText.Layout.Element.Table(new float[] { 4, 6 }).UseAllAvailableWidth().SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                    Paragraph pTitolo = new Paragraph("Lavorazione").SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    Paragraph pValore = new Paragraph(eventoCorrente.lavorazione).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE);
                    tbIntestazioneDx.AddCell(pValore).SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                    pTitolo = new Paragraph("Unità").SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    pValore = new Paragraph("").SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE);
                    tbIntestazioneDx.AddCell(pValore).SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                    pTitolo = new Paragraph("Tender").SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    pValore = new Paragraph("").SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE);
                    tbIntestazioneDx.AddCell(pValore).SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                    pTitolo = new Paragraph("Città").SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    pValore = new Paragraph(eventoCorrente.luogo).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE);
                    tbIntestazioneDx.AddCell(pValore).SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                    pTitolo = new Paragraph("Produzione").SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    pValore = new Paragraph(eventoCorrente.produzione).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE);
                    tbIntestazioneDx.AddCell(pValore).SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                    pTitolo = new Paragraph("Cod.Lavoro").SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                    pValore = new Paragraph(eventoCorrente.codice_lavoro).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE);
                    tbIntestazioneDx.AddCell(pValore).SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                    // AGGIUNGO INTESTAZIONE STRUMENTI
                    tbIntestazione.AddCell(tbIntestazioneDx).SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                    document.Add(tbIntestazione);
                    Paragraph pSpazio = new Paragraph(" ");
                    document.Add(pSpazio);

                    // CREAZIONE GRIGLIA
                    iText.Layout.Element.Table tbGrigla = new iText.Layout.Element.Table(new float[] { 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60 }).SetWidth(780).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetFixedLayout();
                    Paragraph pGriglia;
                    Cell cellaGriglia;

                    // INTESTAZIONE GRIGLIA
                    pGriglia = new Paragraph("").SetFontSize(10);
                    cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.BLACK, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetBold();
                    cellaGriglia.Add(pGriglia);
                    tbGrigla.AddHeaderCell(cellaGriglia);

                    pGriglia = new Paragraph("Camera").SetFontSize(10);
                    cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.BLACK, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetBold();
                    cellaGriglia.Add(pGriglia);
                    tbGrigla.AddHeaderCell(cellaGriglia);

                    pGriglia = new Paragraph("Fibra/Triax").SetFontSize(10);
                    cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.BLACK, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetBold();
                    cellaGriglia.Add(pGriglia);
                    tbGrigla.AddHeaderCell(cellaGriglia);

                    pGriglia = new Paragraph("Ottica").SetFontSize(10);
                    cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.BLACK, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetBold();
                    cellaGriglia.Add(pGriglia);
                    tbGrigla.AddHeaderCell(cellaGriglia);

                    pGriglia = new Paragraph("Viewfinder").SetFontSize(10);
                    cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.BLACK, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetBold();
                    cellaGriglia.Add(pGriglia);
                    tbGrigla.AddHeaderCell(cellaGriglia);

                    pGriglia = new Paragraph("Loop").SetFontSize(10);
                    cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.BLACK, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetBold();
                    cellaGriglia.Add(pGriglia);
                    tbGrigla.AddHeaderCell(cellaGriglia);

                    pGriglia = new Paragraph("Mic").SetFontSize(10);
                    cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.BLACK, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetBold();
                    cellaGriglia.Add(pGriglia);
                    tbGrigla.AddHeaderCell(cellaGriglia);

                    pGriglia = new Paragraph("Testa").SetFontSize(10);
                    cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.BLACK, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetBold();
                    cellaGriglia.Add(pGriglia);
                    tbGrigla.AddHeaderCell(cellaGriglia);

                    pGriglia = new Paragraph("Lens Holder").SetFontSize(10);
                    cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.BLACK, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetBold();
                    cellaGriglia.Add(pGriglia);
                    tbGrigla.AddHeaderCell(cellaGriglia);

                    pGriglia = new Paragraph("Cavalletto").SetFontSize(10);
                    cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.BLACK, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetBold();
                    cellaGriglia.Add(pGriglia);
                    tbGrigla.AddHeaderCell(cellaGriglia);

                    pGriglia = new Paragraph("Cavi").SetFontSize(10);
                    cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.BLACK, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetBold();
                    cellaGriglia.Add(pGriglia);
                    tbGrigla.AddHeaderCell(cellaGriglia);

                    pGriglia = new Paragraph("Altro Carico").SetFontSize(10);
                    cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.BLACK, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetBold();
                    cellaGriglia.Add(pGriglia);
                    tbGrigla.AddHeaderCell(cellaGriglia);

                    pGriglia = new Paragraph("Altro Carico").SetFontSize(10);
                    cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.BLACK, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetBold();
                    cellaGriglia.Add(pGriglia);
                    tbGrigla.AddHeaderCell(cellaGriglia);

                    foreach (DatiLavorazioneMagazzino lavorazioneMagazzino in listaDatiLavorazioneMagazzino)
                    {
                        pGriglia = new Paragraph(lavorazioneMagazzino.Descrizione_Camera).SetFontSize(8);
                        cellaGriglia = new Cell().SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.BLACK, 1, 100)).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10);
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        pGriglia = new Paragraph(lavorazioneMagazzino.Nome_Camera).SetFontSize(6);
                        cellaGriglia = new Cell().SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.BLACK, 1, 100)).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10);
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        pGriglia = new Paragraph(lavorazioneMagazzino.Nome_Fibra_Trax).SetFontSize(6);
                        cellaGriglia = new Cell().SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.BLACK, 1, 100)).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10);
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        pGriglia = new Paragraph(lavorazioneMagazzino.Nome_Ottica).SetFontSize(6);
                        cellaGriglia = new Cell().SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.BLACK, 1, 100)).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10);
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        pGriglia = new Paragraph(lavorazioneMagazzino.Nome_Viewfinder).SetFontSize(6);
                        cellaGriglia = new Cell().SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.BLACK, 1, 100)).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10);
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        pGriglia = new Paragraph(lavorazioneMagazzino.Nome_Loop).SetFontSize(6);
                        cellaGriglia = new Cell().SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.BLACK, 1, 100)).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10);
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        pGriglia = new Paragraph(lavorazioneMagazzino.Nome_Mic).SetFontSize(6);
                        cellaGriglia = new Cell().SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.BLACK, 1, 100)).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10);
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        pGriglia = new Paragraph(lavorazioneMagazzino.Nome_Testa).SetFontSize(6);
                        cellaGriglia = new Cell().SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.BLACK, 1, 100)).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10);
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        pGriglia = new Paragraph(lavorazioneMagazzino.Nome_Lensholder).SetFontSize(6);
                        cellaGriglia = new Cell().SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.BLACK, 1, 100)).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10);
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        pGriglia = new Paragraph(lavorazioneMagazzino.Nome_Cavalletto).SetFontSize(6);
                        cellaGriglia = new Cell().SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.BLACK, 1, 100)).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10);
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        pGriglia = new Paragraph(lavorazioneMagazzino.Nome_Cavi).SetFontSize(6);
                        cellaGriglia = new Cell().SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.BLACK, 1, 100)).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10);
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        pGriglia = new Paragraph(lavorazioneMagazzino.Nome_Altro1).SetFontSize(6);
                        cellaGriglia = new Cell().SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.BLACK, 1, 100)).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10);
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        pGriglia = new Paragraph(lavorazioneMagazzino.Nome_Altro2).SetFontSize(6);
                        cellaGriglia = new Cell().SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.BLACK, 1, 100)).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10);
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);
                    }
                    
                    // AGGIUNGO TABELLA
                    document.Add(tbGrigla);

                    // AGGIUNGO LE NOTE
                    pSpazio = new Paragraph(" ");
                    document.Add(pSpazio);

                    NoteLavorazioneMagazzinoCorrente = GetNoteLavorazioneMagazzino(idLavorazione);

                    iText.Layout.Element.Text first = new iText.Layout.Element.Text("Note:").SetFontSize(9).SetBold();
                    iText.Layout.Element.Text second = new iText.Layout.Element.Text(Environment.NewLine + NoteLavorazioneMagazzinoCorrente.Note.Trim()).SetFontSize(9);
                    iText.Layout.Element.Paragraph paragraphNote = new iText.Layout.Element.Paragraph().SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.BLACK, 1, 100)).SetPadding(5).Add(first).Add(second);

                    document.Add(paragraphNote);

                    int n = doc.GetNumberOfPages();
                    iText.Kernel.Geom.Rectangle pageSize = doc.GetPage(n).GetPageSize();

                    // AGGIUNGO CONTEGGIO PAGINE E FOOTER PER OGNI PAGINA
                    for (int i = 1; i <= n; i++)
                    {
                        //AGGIUNGO NUM.PAGINA
                        document.ShowTextAligned(new Paragraph("pagina " + i.ToString() + " di " + n.ToString()).SetFontSize(7),
                            pageSize.GetWidth() - 60, pageSize.GetHeight() - 20, i, iText.Layout.Properties.TextAlignment.CENTER, iText.Layout.Properties.VerticalAlignment.TOP, 0);
                        //AGGIUNGO FOOTER
                        document.ShowTextAligned(new Paragraph(denominazioneVs + " P.IVA " + pIvaVs + Environment.NewLine + "Sede legale: " + toponimoVs + " " + indirizzoVs + " " + civicoVs + " - " + capVs + " " + cittaVs + " " + provinciaVs + " e-mail: " + emailVs).SetFontSize(7).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER),
                            pageSize.GetWidth() / 2, 30, i, iText.Layout.Properties.TextAlignment.CENTER, iText.Layout.Properties.VerticalAlignment.TOP, 0);

                    }

                    // CHIUDO IL PDF
                    document.Flush();
                    document.Close();
                    wr.Close();

                    //btnRicercaAttrezzatura_Click(null, null);

                    // CREO STRINGA PER IL JAVASCRIPT DI VISUALIZZAZIONE
                    if (System.IO.File.Exists(mapPathReport))
                    {
                        Page page = HttpContext.Current.Handler as Page;
                        ScriptManager.RegisterStartupScript(page, page.GetType(), "apriPdf", script: "window.open('" + pathReport.Replace("~", "") + "')", addScriptTags: true);
                    }
                    else
                    {
                        esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                        esito.Descrizione = "Il File " + pathReport.Replace("~", "") + " non è stato creato correttamente!";
                        BasePage p = new BasePage();
                        p.ShowError(esito.Descrizione);
                    }
                }

            }
            catch (Exception ex)
            {
                BasePage p = new BasePage();
                p.ShowError(ex.Message + " " + ex.StackTrace);
            }

        }
    }
}