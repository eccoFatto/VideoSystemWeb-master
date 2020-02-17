using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoSystemWeb.BLL;
using VideoSystemWeb.Entity;

namespace VideoSystemWeb.MAGAZZINO
{
    public partial class Magazzino : BasePage
    {
        #region ELENCO CHIAVI VIEWSTATE
        private const string VIEWSTATE_NOTELAVORAZIONEMAGAZZINO_CORRENTE = "noteLavorazioneMagazzinoCorrente";
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
                        NoteLavorazioneMagazzinoCorrente = GetNoteLavorazioneMagazzino(idLavorazione);
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
    }
}