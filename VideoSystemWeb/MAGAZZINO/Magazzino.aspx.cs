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

        int GetColumnIndexByName(GridViewRow row, string SearchColumnName)
        {
            int columnIndex = 0;
            foreach (DataControlFieldCell cell in row.Cells)
            {
                if (cell.ContainingField is BoundField)
                {
                    if (((BoundField)cell.ContainingField).DataField.Equals(SearchColumnName))
                    {
                        break;
                    }
                }
                columnIndex++;
            }
            return columnIndex;
        }


        string GetColumnNameByIndex(GridViewRow row, int index)
        {
            string ret = "";
            int columnIndex = 0;
            foreach (DataControlFieldCell cell in row.Cells)
            {
                
                if (cell.ContainingField is BoundField)
                {
                    if (columnIndex == index)
                    {
                        ret = ((BoundField)cell.ContainingField).DataField + ";" + ((BoundField)cell.ContainingField).HeaderText;

                        break;
                    }
                }
                columnIndex++;
            }
            return ret;
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
                    phDescrizioneCamera.Visible = false;
                    phModificaAttrezzature.Visible = true;
                    break;
            }


            //val_Stato.Text = UtilityTipologiche.getElementByID(SessionManager.ListaStati, SessionManager.EventoSelezionato.id_stato, ref esito).nome;
            //val_CodiceLavoro.Text = string.IsNullOrEmpty(SessionManager.EventoSelezionato.codice_lavoro) ? "-" : SessionManager.EventoSelezionato.codice_lavoro;

            //popupAppuntamento.ClearAppuntamento();
            //popupAppuntamento.PopolaAppuntamento();

            //popupOfferta.ClearOfferta();
            //if (SessionManager.EventoSelezionato.id_stato == Stato.Instance.STATO_OFFERTA || SessionManager.EventoSelezionato.id_stato == Stato.Instance.STATO_LAVORAZIONE)
            //{
            //    popupOfferta.PopolaOfferta();
            //}

            //popupLavorazione.ClearLavorazione();
            //if (SessionManager.EventoSelezionato.id_stato == Stato.Instance.STATO_LAVORAZIONE)
            //{
            //    popupLavorazione.PopolaLavorazione();
            //}
            //RiempiCampiIntestazioneEvento();

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
        }

        protected void btnModificaAttrezzature_Click(object sender, EventArgs e)
        {

        }
    }
}