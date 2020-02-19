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

    }
}