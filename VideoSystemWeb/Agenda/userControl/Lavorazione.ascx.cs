using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoSystemWeb.BLL;
using VideoSystemWeb.Entity;

namespace VideoSystemWeb.Agenda.userControl
{
    
    public partial class Lavorazione : System.Web.UI.UserControl
    {
        ObjectIDGenerator IDGenerator = new ObjectIDGenerator();

        public delegate void PopupHandler(string operazionePopup); // delegato per l'evento
        public event PopupHandler RichiediOperazionePopup; //evento
        BasePage basePage = new BasePage();

        private const int COLLABORATORE = 0;
        private const int FORNITORE = 1;

        private const int ID_TIPO_PAGAMENTO_ASSUNZIONE = 1;
        private const int ID_TIPO_PAGAMENTO_MISTA = 2;
        private const int ID_TIPO_PAGAMENTO_RITENUTA_ACCONTO = 3;
        private const int ID_TIPO_PAGAMENTO_FATTURA = 4;

        #region ELENCO CHIAVI VIEWSTATE
        private const string VIEWSTATE_FP_DETTAGLIOECONOMICO = "FPDettaglioEconomico"; // Figura Professionale selezionata in pannello modifica
        private const string VIEWSTATE_LISTAARTICOLIGRUPPILAVORAZIONE = "listaArticoliGruppiLavorazione";
        private const string VIEWSTATE_LISTAFIGUREPROFESSIONALI = "listaFigureProfessionali";
        private const string VIEWSTATE_IDARTICOLOLAVORAZIONE = "idArticoloLavorazione";
        private const string VIEWSTATE_IDENTIFICATOREARTICOLOLAVORAZIONE = "identificatoreArticoloLavorazione";
        private const string VIEWSTATE_IDFIGURAPROFESSIONALE = "idFiguraProfessionale";
        private const string VIEWSTATE_IDENTIFICATOREFIGURAPROFESSIONALE = "identificatoreFiguraProfessionale";
        #endregion

        List<ArticoliGruppi> ListaArticoliGruppiLavorazione
        {
            get
            {
                if (ViewState[VIEWSTATE_LISTAARTICOLIGRUPPILAVORAZIONE] == null || ((List<ArticoliGruppi>)ViewState[VIEWSTATE_LISTAARTICOLIGRUPPILAVORAZIONE]).Count == 0)
                {
                    ViewState[VIEWSTATE_LISTAARTICOLIGRUPPILAVORAZIONE] = Articoli_BLL.Instance.CaricaListaArticoliGruppi();
                }
                return (List<ArticoliGruppi>)ViewState[VIEWSTATE_LISTAARTICOLIGRUPPILAVORAZIONE];
            }
            set
            {
                ViewState[VIEWSTATE_LISTAARTICOLIGRUPPILAVORAZIONE] = value;
            }
        }
        List<FiguraProfessionale> ListaFigureProfessionali
        {
            get
            {
                if (ViewState[VIEWSTATE_LISTAFIGUREPROFESSIONALI] == null)
                {
                    ViewState[VIEWSTATE_LISTAFIGUREPROFESSIONALI] = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaFigureProfessionali;
                }
                return (List<FiguraProfessionale>)ViewState[VIEWSTATE_LISTAFIGUREPROFESSIONALI];
            }
            set
            {
                List<FiguraProfessionale> _listaFigureProfessionali = value;
                if (_listaFigureProfessionali != null)
                {
                    foreach (FiguraProfessionale figuraProfessionale in _listaFigureProfessionali)
                    {
                        bool firstTime;
                        figuraProfessionale.IdentificatoreOggetto = IDGenerator.GetId(figuraProfessionale, out firstTime);

                    }
                }

                ViewState[VIEWSTATE_LISTAFIGUREPROFESSIONALI] = _listaFigureProfessionali;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                gvGruppiLavorazione.DataSource = ListaArticoliGruppiLavorazione;
                gvGruppiLavorazione.DataBind();

                PopolaCombo();
            }
            else
            {
                // SELEZIONO L'ULTIMA TAB SELEZIONATA
                ScriptManager.RegisterStartupScript(Page, GetType(), "apriTabGiusta", script: "openTabEventoLavorazione(event,'" + hf_tabSelezionataLavorazione.Value + "')", addScriptTags: true);
            }
        }

        #region COMPORTAMENTO ELEMENTI PAGINA
        protected void gvGruppiLavorazione_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            long idSelezione = Convert.ToInt64(e.CommandArgument);

            ArticoliGruppi articoloGruppo = ListaArticoliGruppiLavorazione.FirstOrDefault(X => X.Id == idSelezione);

            if (articoloGruppo.Isgruppo)
            {
                AggiungiArticoliDelGruppoAListaArticoli(articoloGruppo.IdOggetto);
            }
            else
            {
                AggiungiArticoloAListaArticoli(articoloGruppo.IdOggetto);
            }

            if (SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione != null && SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Count > 0)
            {
                AggiornaTotali();
                ResetPanelLavorazione();
                RichiediOperazionePopup("UPDATE");
            }
        }

        // DETTAGLIO ECONOMICO
        protected void gvArticoliLavorazione_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            int id = Convert.ToInt32(commandArgs[0]);

            DatiArticoliLavorazione articoloSelezionato;
            if (id == 0)
            {
                long identificatoreOggetto = Convert.ToInt64(commandArgs[1]);
                ViewState[VIEWSTATE_IDENTIFICATOREARTICOLOLAVORAZIONE] = identificatoreOggetto;
                ViewState[VIEWSTATE_IDARTICOLOLAVORAZIONE] = 0;
                articoloSelezionato = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.FirstOrDefault(x => x.IdentificatoreOggetto == identificatoreOggetto);

            }
            else
            {
                ViewState[VIEWSTATE_IDARTICOLOLAVORAZIONE] = id;
                articoloSelezionato = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.FirstOrDefault(x => x.Id == id);

                ViewState[VIEWSTATE_IDENTIFICATOREARTICOLOLAVORAZIONE] = articoloSelezionato.IdentificatoreOggetto;
            }

            int indexArticolo = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.IndexOf(articoloSelezionato);

            switch (e.CommandName)
            {
                case "modifica":

                    ClearPanelModificaArticolo();

                    txt_Descrizione.Text = articoloSelezionato.Descrizione;
                    txt_DescrizioneLunga.Text = articoloSelezionato.DescrizioneLunga;
                    txt_Costo.Text = articoloSelezionato.Costo.ToString();
                    txt_Prezzo.Text = articoloSelezionato.Prezzo.ToString();
                    txt_Iva.Text = articoloSelezionato.Iva.ToString();
                    ddl_Stampa.SelectedValue = articoloSelezionato.Stampa ? "1" : "0";
                    ddl_FPtipoPagamento.SelectedValue = articoloSelezionato.IdTipoPagamento != null ? articoloSelezionato.IdTipoPagamento.ToString() : "";

                    //Cerco tra Collaboratori o Fornitori
                    FiguraProfessionale figuraProfessionale = SessionManager.ListaCompletaFigProf.FirstOrDefault(x => x.Id == articoloSelezionato.IdCollaboratori && x.Tipo == COLLABORATORE);
                    if (figuraProfessionale != null) // TROVATO UN COLLABORATORE
                    {
                        figuraProfessionale.IdCollaboratori = articoloSelezionato.IdCollaboratori;
                        figuraProfessionale.IdFornitori = null;
                    }
                    else // TROVATO UN FORNITORE
                    {
                        figuraProfessionale = SessionManager.ListaCompletaFigProf.FirstOrDefault(x => x.Id == articoloSelezionato.IdFornitori && x.Tipo == FORNITORE);
                        if (figuraProfessionale != null)
                        {
                            figuraProfessionale.IdCollaboratori = null;
                            figuraProfessionale.IdFornitori = articoloSelezionato.IdFornitori;
                        }
                    }

                    //AbilitaComponentiCosto(articoloSelezionato);

                    if (figuraProfessionale != null)
                    {
                        div_FiguraProfessionale.Visible = true;
                        hf_IdFiguraProfessionale.Value = figuraProfessionale.Id.ToString();
                        ddl_FPtipoPagamento.SelectedValue = articoloSelezionato.IdTipoPagamento == null ? "" : articoloSelezionato.IdTipoPagamento.ToString();

                        AbilitaCostoFP(!string.IsNullOrEmpty(ddl_FPtipoPagamento.SelectedValue));

                        if (articoloSelezionato.UsaCostoFP != null && (bool)articoloSelezionato.UsaCostoFP)
                        {
                            txt_FPnetto.Text = articoloSelezionato.FP_netto.ToString();
                            txt_FPlordo.Text = articoloSelezionato.FP_lordo.ToString();

                            if (articoloSelezionato.IdTipoPagamento == ID_TIPO_PAGAMENTO_MISTA)
                            {
                                txt_FPRimborsoKM.Text = (articoloSelezionato.FP_netto - 45).ToString();
                            }
                        }

                        ddl_FPtipo.SelectedValue = "";// figuraProfessionale.Tipo.ToString();
                        txt_FPnotaCollaboratore.Text = articoloSelezionato.Nota;
                        lbl_NominativoFiguraProfessionale.Text = figuraProfessionale.NominativoCompleto;
                        txt_FPtelefono.Text = figuraProfessionale.Telefono;
                    }
                    else
                    {
                        AbilitaCostoFP(false);
                    }

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriModificaArticolo", script: "javascript: document.getElementById('" + panelModificaArticolo.ClientID + "').style.display='block'", addScriptTags: true);

                    break;
                case "elimina":
                    SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Remove(articoloSelezionato);
                    gvArticoliLavorazione.DataSource = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione;
                    gvArticoliLavorazione.DataBind();

                   // AggiornaTotali();
                    ResetPanelLavorazione();

                    break;
                case "moveUp":
                    if (indexArticolo > 0)
                    {
                        SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Remove(articoloSelezionato);
                        SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Insert(indexArticolo - 1, articoloSelezionato);
                        gvArticoliLavorazione.DataSource = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione;
                        gvArticoliLavorazione.DataBind();
                    }
                    break;
                case "moveDown":
                    if (indexArticolo < SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Count - 1)
                    {
                        SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Remove(articoloSelezionato);
                        SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Insert(indexArticolo + 1, articoloSelezionato);
                        gvArticoliLavorazione.DataSource = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione;
                        gvArticoliLavorazione.DataBind();
                    }
                    break;
            }

            RichiediOperazionePopup("UPDATE");
        }

        // PIANO ESTERNO
        protected void gvFigProfessionali_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            int id = Convert.ToInt32(commandArgs[0]);

            FiguraProfessionale figuraProfessionaleSelezionata;
            if (id == 0)
            {
                long identificatoreOggetto = Convert.ToInt64(commandArgs[1]);
                ViewState[VIEWSTATE_IDENTIFICATOREFIGURAPROFESSIONALE] = identificatoreOggetto;
                figuraProfessionaleSelezionata = ListaFigureProfessionali.FirstOrDefault(x => x.IdentificatoreOggetto == identificatoreOggetto);
            }
            else
            {
                ViewState[VIEWSTATE_IDFIGURAPROFESSIONALE] = id;
                figuraProfessionaleSelezionata = ListaFigureProfessionali.FirstOrDefault(x => x.Id == id);
            }

            int indexArticolo = ListaFigureProfessionali.IndexOf(figuraProfessionaleSelezionata);

            switch (e.CommandName)
            {
                case "modifica":
                    DatiPianoEsternoLavorazione datiPianoEsterno = new DatiPianoEsternoLavorazione();
                    // prendo datiPianoEsterno a partire dalla figuraProfessionale selezionata
                    if (figuraProfessionaleSelezionata.IdCollaboratori != null && figuraProfessionaleSelezionata.IdCollaboratori != 0)
                    {
                        datiPianoEsterno = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaDatiPianoEsternoLavorazione.FirstOrDefault(x => x.IdCollaboratori == figuraProfessionaleSelezionata.IdCollaboratori);
                    }
                    else if (figuraProfessionaleSelezionata.IdFornitori != null && figuraProfessionaleSelezionata.IdFornitori != 0)
                    {
                        datiPianoEsterno = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaDatiPianoEsternoLavorazione.FirstOrDefault(x => x.IdFornitori == figuraProfessionaleSelezionata.IdFornitori);
                    }

                    txt_data.Text = ((DateTime)SessionManager.EventoSelezionato.data_inizio_lavorazione).ToString("dd/MM/yyyy");
                    txt_orario.Text = datiPianoEsterno.Orario == null ? "" : ((DateTime)datiPianoEsterno.Orario).ToString("HH:mm");
                    chk_diaria.Checked = datiPianoEsterno.Diaria != null && (bool)datiPianoEsterno.Diaria;
                    
                    diaria15.Checked = (datiPianoEsterno.ImportoDiaria != null && datiPianoEsterno.ImportoDiaria == 15);
                    diaria30.Checked = (datiPianoEsterno.ImportoDiaria != null && datiPianoEsterno.ImportoDiaria == 30);
                    if (datiPianoEsterno.ImportoDiaria != null &&
                        datiPianoEsterno.ImportoDiaria != 0 &&
                        datiPianoEsterno.ImportoDiaria != 15 &&
                        datiPianoEsterno.ImportoDiaria != 30)
                    {
                        diariaLibera.Checked = true;
                        txt_diaria.Text = ((Decimal)datiPianoEsterno.ImportoDiaria).ToString();
                    }
                    else
                    {
                        diariaLibera.Checked = false;
                        txt_diaria.Text = "";
                    }

                    ddl_intervento.SelectedValue = datiPianoEsterno.IdIntervento == null ? "1": ((int)datiPianoEsterno.IdIntervento).ToString();
                    chk_albergo.Checked = datiPianoEsterno.Albergo !=null && (bool)datiPianoEsterno.Albergo;
                    txt_notaPianoEsterno.Text = datiPianoEsterno.Nota;

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriModificaFigProf", script: "javascript: document.getElementById('" + panelModificaPianoEsterno.ClientID + "').style.display='block'", addScriptTags: true);

                    break;
                case "elimina":
                    ListaFigureProfessionali.Remove(figuraProfessionaleSelezionata);
                    gvFigProfessionali.DataSource = ListaFigureProfessionali;
                    gvFigProfessionali.DataBind();

                    // AGGIORNO LISTADATIPIANOESTERNOLAVORAIONE
                    if (figuraProfessionaleSelezionata.Tipo == COLLABORATORE)
                    {
                        SessionManager.EventoSelezionato.LavorazioneCorrente.ListaDatiPianoEsternoLavorazione = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaDatiPianoEsternoLavorazione.Where(x => x.IdCollaboratori != figuraProfessionaleSelezionata.IdCollaboratori).ToList();
                    }
                    else if (figuraProfessionaleSelezionata.Tipo == FORNITORE)
                    {
                        SessionManager.EventoSelezionato.LavorazioneCorrente.ListaDatiPianoEsternoLavorazione = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaDatiPianoEsternoLavorazione.Where(x => x.IdFornitori != figuraProfessionaleSelezionata.IdFornitori).ToList();
                    }

                    CancellaDiariaDaListaArticoli(figuraProfessionaleSelezionata);

                    ResetPanelLavorazione();

                    break;
                case "moveUp":
                    if (indexArticolo > 0)
                    {
                        ListaFigureProfessionali.Remove(figuraProfessionaleSelezionata);
                        ListaFigureProfessionali.Insert(indexArticolo - 1, figuraProfessionaleSelezionata);
                        gvFigProfessionali.DataSource = ListaFigureProfessionali;
                        gvFigProfessionali.DataBind();

                        // AGGIORNO LISTADATIPIANOESTERNOLAVORAIONE
                        DatiPianoEsternoLavorazione datiPianoEsternoLavorazioneSelezionato = null;
                        if (figuraProfessionaleSelezionata.Tipo == COLLABORATORE)
                        {
                            datiPianoEsternoLavorazioneSelezionato = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaDatiPianoEsternoLavorazione.First(x => x.IdCollaboratori == figuraProfessionaleSelezionata.IdCollaboratori);
                        }
                        else if (figuraProfessionaleSelezionata.Tipo == FORNITORE)
                        {
                            datiPianoEsternoLavorazioneSelezionato = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaDatiPianoEsternoLavorazione.First(x => x.IdFornitori == figuraProfessionaleSelezionata.IdFornitori);
                        }
                        SessionManager.EventoSelezionato.LavorazioneCorrente.ListaDatiPianoEsternoLavorazione.Remove(datiPianoEsternoLavorazioneSelezionato);
                        SessionManager.EventoSelezionato.LavorazioneCorrente.ListaDatiPianoEsternoLavorazione.Insert(indexArticolo - 1, datiPianoEsternoLavorazioneSelezionato);
                    }
                    break;
                case "moveDown":
                    if (indexArticolo < SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Count - 1)
                    {
                        ListaFigureProfessionali.Remove(figuraProfessionaleSelezionata);
                        ListaFigureProfessionali.Insert(indexArticolo + 1, figuraProfessionaleSelezionata);
                        gvFigProfessionali.DataSource = ListaFigureProfessionali;
                        gvFigProfessionali.DataBind();

                        // AGGIORNO LISTADATIPIANOESTERNOLAVORAIONE
                        DatiPianoEsternoLavorazione datiPianoEsternoLavorazioneSelezionato = null;
                        if (figuraProfessionaleSelezionata.Tipo == COLLABORATORE)
                        {
                            datiPianoEsternoLavorazioneSelezionato = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaDatiPianoEsternoLavorazione.First(x => x.IdCollaboratori == figuraProfessionaleSelezionata.IdCollaboratori);
                        }
                        else if (figuraProfessionaleSelezionata.Tipo == FORNITORE)
                        {
                            datiPianoEsternoLavorazioneSelezionato = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaDatiPianoEsternoLavorazione.First(x => x.IdFornitori == figuraProfessionaleSelezionata.IdFornitori);
                        }
                        SessionManager.EventoSelezionato.LavorazioneCorrente.ListaDatiPianoEsternoLavorazione.Remove(datiPianoEsternoLavorazioneSelezionato);
                        SessionManager.EventoSelezionato.LavorazioneCorrente.ListaDatiPianoEsternoLavorazione.Insert(indexArticolo + 1, datiPianoEsternoLavorazioneSelezionato);
                    }
                    break;
            }

            RichiediOperazionePopup("UPDATE");
        }

        protected void btnOKModificaArtLavorazione_Click(object sender, EventArgs e)
        {
            DatiArticoliLavorazione articoloSelezionato;

            if (ViewState[VIEWSTATE_IDARTICOLOLAVORAZIONE] == null)
            {
                long identificatoreOggetto = (long)ViewState[VIEWSTATE_IDENTIFICATOREARTICOLOLAVORAZIONE];
                articoloSelezionato = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.FirstOrDefault(x => x.IdentificatoreOggetto == identificatoreOggetto);
            }
            else
            {
                int idArticolo = (int)ViewState[VIEWSTATE_IDARTICOLOLAVORAZIONE];
                long identificatoreOggetto = (long)ViewState[VIEWSTATE_IDENTIFICATOREARTICOLOLAVORAZIONE];

                articoloSelezionato = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.FirstOrDefault(x => x.Id == idArticolo && x.IdentificatoreOggetto == identificatoreOggetto);
            }

            var index = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.IndexOf(articoloSelezionato);

            articoloSelezionato.Descrizione = txt_Descrizione.Text;
            articoloSelezionato.DescrizioneLunga = txt_DescrizioneLunga.Text;

            articoloSelezionato.UsaCostoFP = !string.IsNullOrEmpty(ddl_FPtipoPagamento.SelectedValue);

            if (articoloSelezionato.UsaCostoFP != null && (bool)articoloSelezionato.UsaCostoFP)
            {
                articoloSelezionato.FP_netto = string.IsNullOrEmpty(txt_FPnetto.Text) ? 0 : decimal.Parse(txt_FPnetto.Text);
                articoloSelezionato.FP_lordo = string.IsNullOrEmpty(txt_FPlordo.Text) ? 0 : decimal.Parse(txt_FPlordo.Text);
                articoloSelezionato.Costo = 0;
            }
            else
            {
                articoloSelezionato.Costo = decimal.Parse(txt_Costo.Text);
                articoloSelezionato.FP_netto = 0;
                articoloSelezionato.FP_lordo = 0;
            }

            articoloSelezionato.Prezzo = decimal.Parse(txt_Prezzo.Text);
            articoloSelezionato.Iva = int.Parse(txt_Iva.Text);
            articoloSelezionato.Stampa = ddl_Stampa.SelectedValue == "1";
            articoloSelezionato.Data = SessionManager.EventoSelezionato.data_inizio_lavorazione;
            articoloSelezionato.FP_lordo = string.IsNullOrEmpty(txt_FPlordo.Text) ? 0 : decimal.Parse(txt_FPlordo.Text);

            FiguraProfessionale figuraProfessionaleSelezionata = (FiguraProfessionale)ViewState[VIEWSTATE_FP_DETTAGLIOECONOMICO];

            if (figuraProfessionaleSelezionata!=null && figuraProfessionaleSelezionata.Tipo == COLLABORATORE)
            {
                articoloSelezionato.IdCollaboratori = figuraProfessionaleSelezionata.Id;
                articoloSelezionato.IdFornitori = null;
            }
            else if (figuraProfessionaleSelezionata != null && figuraProfessionaleSelezionata.Tipo == FORNITORE)
            {
                articoloSelezionato.IdFornitori = figuraProfessionaleSelezionata.Id;
                articoloSelezionato.IdCollaboratori = null;
            }

            articoloSelezionato.IdTipoPagamento = ddl_FPtipoPagamento.SelectedValue == "" ? null : (int?)int.Parse(ddl_FPtipoPagamento.SelectedValue);
            articoloSelezionato.Nota = txt_FPnotaCollaboratore.Text;

            SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.OrderByDescending(x => x.Prezzo).ToList();

            ResetPanelLavorazione();

            RichiediOperazionePopup("UPDATE");
        }

        protected void btnOKModificaPianoEsterno_Click(object sender, EventArgs e)
        {
            DatiPianoEsternoLavorazione datiPianoEsterno = new DatiPianoEsternoLavorazione();
            FiguraProfessionale figuraProfessionale = new FiguraProfessionale();

            if (ViewState[VIEWSTATE_IDFIGURAPROFESSIONALE] == null)
            {
                long identificatoreOggetto = (long)ViewState[VIEWSTATE_IDENTIFICATOREFIGURAPROFESSIONALE];
                figuraProfessionale = ListaFigureProfessionali.FirstOrDefault(x => x.IdentificatoreOggetto == identificatoreOggetto);

            }
            else
            {
                int idFiguraProfessionale = (int)ViewState[VIEWSTATE_IDFIGURAPROFESSIONALE];
                figuraProfessionale = ListaFigureProfessionali.FirstOrDefault(x => x.Id == idFiguraProfessionale);
            }

            //prendo datiPianoEsterno a partire dalla figuraProfessionale selezionata
            if (figuraProfessionale.IdCollaboratori != null && figuraProfessionale.IdCollaboratori != 0)
            {
                datiPianoEsterno = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaDatiPianoEsternoLavorazione.FirstOrDefault(x => x.IdCollaboratori == figuraProfessionale.IdCollaboratori);
            }
            else if (figuraProfessionale.IdFornitori != null && figuraProfessionale.IdFornitori != 0)
            {
                datiPianoEsterno = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaDatiPianoEsternoLavorazione.FirstOrDefault(x => x.IdFornitori == figuraProfessionale.IdFornitori);
            }

            decimal importoDiaria = 0;
            if (diaria15.Checked)
            {
                importoDiaria = 15;
            }
            else if (diaria30.Checked)
            {
                importoDiaria = 30;
            }
            else if (diariaLibera.Checked && !string.IsNullOrEmpty(txt_diaria.Text))
            {
                importoDiaria = decimal.Parse(txt_diaria.Text);
            }

            
            if (importoDiaria > 0) // Aggiungo diaria a listaAricoliLavorazione in dettaglio economico
            {
                AggiungiDiariaAListaArticoli(importoDiaria, figuraProfessionale);
            }
            else // Elimino eventuale diaria da listaAricoliLavorazione in dettaglio economico
            {
                CancellaDiariaDaListaArticoli(figuraProfessionale);
            }

            AggiornaTotali();
            ResetPanelLavorazione();

            datiPianoEsterno.Data = string.IsNullOrEmpty(txt_data.Text) ? null : (DateTime?)DateTime.Parse(txt_data.Text);
            datiPianoEsterno.Orario = string.IsNullOrEmpty(txt_orario.Text) ? null : (DateTime?)DateTime.Parse(txt_orario.Text);
            datiPianoEsterno.Diaria = chk_diaria.Checked;
            datiPianoEsterno.ImportoDiaria = importoDiaria;
            datiPianoEsterno.IdIntervento = int.Parse(ddl_intervento.SelectedValue);
            datiPianoEsterno.Albergo = chk_albergo.Checked;
            datiPianoEsterno.Nota = txt_notaPianoEsterno.Text;

            figuraProfessionale.Intervento = ddl_intervento.SelectedItem.Text;
            figuraProfessionale.Diaria = importoDiaria;
            figuraProfessionale.Nota = txt_notaPianoEsterno.Text;
            figuraProfessionale.Data = string.IsNullOrEmpty(txt_data.Text) ? null : (DateTime?)DateTime.Parse(txt_data.Text);

            gvFigProfessionali.DataSource = ListaFigureProfessionali;
            gvFigProfessionali.DataBind();

            RichiediOperazionePopup("UPDATE");
        }

        protected void btnOKInserimentoGenerale_Click(object sender, EventArgs e)
        {

            decimal importoDiaria = 0;
            if (diaria15_InsGenerale.Checked)
            {
                importoDiaria = 15;
            }
            else if (diaria30_InsGenerale.Checked)
            {
                importoDiaria = 30;
            }
            else if (diariaLibera_InsGenerale.Checked && !string.IsNullOrEmpty(txt_diaria_InsGenerale.Text))
            {
                importoDiaria = decimal.Parse(txt_diaria_InsGenerale.Text);
            }

            foreach (DatiPianoEsternoLavorazione datiPianoEsternoLavorazione in SessionManager.EventoSelezionato.LavorazioneCorrente.ListaDatiPianoEsternoLavorazione)
            {
                FiguraProfessionale figuraProfessionale = ListaFigureProfessionali.FirstOrDefault(x => x.IdCollaboratori == datiPianoEsternoLavorazione.IdCollaboratori && x.IdFornitori == datiPianoEsternoLavorazione.IdFornitori);

                if (importoDiaria > 0) // Aggiungo diaria a listaAricoliLavorazione in dettaglio economico
                {
                    AggiungiDiariaAListaArticoli(importoDiaria, figuraProfessionale);
                }
                else // Elimino eventuale diaria da listaAricoliLavorazione in dettaglio economico
                {
                    CancellaDiariaDaListaArticoli(figuraProfessionale);
                }

                datiPianoEsternoLavorazione.Data = string.IsNullOrEmpty(txt_data_InsGenerale.Text) ? null : (DateTime?)DateTime.Parse(txt_data_InsGenerale.Text);
                datiPianoEsternoLavorazione.Orario = string.IsNullOrEmpty(txt_orario_InsGenerale.Text) ? null : (DateTime?)DateTime.Parse(txt_orario_InsGenerale.Text);
                datiPianoEsternoLavorazione.Diaria = chk_diaria_InsGenerale.Checked;
                datiPianoEsternoLavorazione.ImportoDiaria = importoDiaria;
                datiPianoEsternoLavorazione.IdIntervento = int.Parse(ddl_intervento_InsGenerale.SelectedValue);
                datiPianoEsternoLavorazione.Albergo = chk_albergo_InsGenerale.Checked;

                figuraProfessionale.Intervento = ddl_intervento_InsGenerale.SelectedItem.Text;
                figuraProfessionale.Diaria = importoDiaria;
                figuraProfessionale.Data = string.IsNullOrEmpty(txt_data_InsGenerale.Text) ? null : (DateTime?)DateTime.Parse(txt_data_InsGenerale.Text);
            }

            AggiornaTotali();
            ResetPanelLavorazione();

            gvFigProfessionali.DataSource = ListaFigureProfessionali;
            gvFigProfessionali.DataBind();

            RichiediOperazionePopup("UPDATE");
        }

        protected void gvArticoliLavorazione_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DatiArticoliLavorazione rigaCorrente = (DatiArticoliLavorazione)e.Row.DataItem;

                if (rigaCorrente.IdCollaboratori != null && rigaCorrente.IdCollaboratori.HasValue)
                {
                    FiguraProfessionale figuraProfessionale = SessionManager.ListaCompletaFigProf.FirstOrDefault(x => x.Id == rigaCorrente.IdCollaboratori && x.Tipo == COLLABORATORE);
                    ((Label)e.Row.FindControl("lbl_Riferimento")).Text = figuraProfessionale.NominativoCompleto;
                }

                if (rigaCorrente.IdFornitori != null && rigaCorrente.IdFornitori.HasValue)
                {
                    FiguraProfessionale figuraProfessionale = SessionManager.ListaCompletaFigProf.FirstOrDefault(x => x.Id == rigaCorrente.IdFornitori && x.Tipo == FORNITORE);
                    ((Label)e.Row.FindControl("lbl_Riferimento")).Text = figuraProfessionale.NominativoCompleto;
                }

                if (rigaCorrente.IdTipoPagamento != null && rigaCorrente.IdTipoPagamento.HasValue)
                {
                    ((Label)e.Row.FindControl("lbl_TipoPagamento")).Text = SessionManager.ListaTipiPagamento.FirstOrDefault(x => x.id == rigaCorrente.IdTipoPagamento).nome;
                }

                if (rigaCorrente.UsaCostoFP != null && (bool)rigaCorrente.UsaCostoFP)
                {
                    ((Label)e.Row.FindControl("lbl_Costo")).Text = string.Format("{0:N2}", rigaCorrente.FP_netto);
                }
                else
                {
                    ((Label)e.Row.FindControl("lbl_Costo")).Text = string.Format("{0:N2}", rigaCorrente.Costo);
                }
            }
        }

        protected void btnImporta_Click(object sender, EventArgs e)
        {
            List<DatiArticoliLavorazione> _listaCollaboratoriFornitori;
            if (SessionManager.EventoSelezionato.LavorazioneCorrente == null ||
                SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione == null ||
                SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Count == 0 ||
                (_listaCollaboratoriFornitori = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Where(x => x.IdCollaboratori != null || x.IdFornitori != null).ToList()).Count() == 0)
            {
                basePage.ShowWarning("Nessuna Figura Professionale da importare");
            }
            else
            {
                CaricaListaDatiPianoEsternoLavorazione(_listaCollaboratoriFornitori);

                if (ListaFigureProfessionali.Count > 0)
                {
                    lbl_nessunaFiguraProf.Visible = false;

                    gvFigProfessionali.DataSource = ListaFigureProfessionali;
                    gvFigProfessionali.DataBind();
                    
                    RichiediOperazionePopup("UPDATE");
                }
            }
        }

        protected void btnInserimentoGenerale_Click(object sender, EventArgs e)
        {
            ClearPanelInserimentoGenerale();

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriModificaFigProf", script: "javascript: document.getElementById('" + panelInserimentoGeneralePianoEsterno.ClientID + "').style.display='block'", addScriptTags: true);
        }

        protected void btn_Cerca_Click(object sender, EventArgs e)
        {
            CercaFP();
        }

        protected void gv_FigureProfessionaliModifica_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_FigureProfessionaliModifica.PageIndex = e.NewPageIndex;
            btn_Cerca_Click(null, null);
        }

        protected void gv_FigureProfessionali_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.background='yellow';";
                e.Row.Attributes["onmouseout"] = "this.style.background='transparent';";
               // e.Row.ToolTip = "Seleziona questa Figura Professionale";
                //e.Row.Attributes["onclick"] = "$('#" + btn_SelezionaFP.ClientID + "').click();";//"alert('selezionata riga " + e.Row.RowIndex + "');";
            }
        }

        protected void gv_FigureProfessionaliModifica_RowCommand(object sender, GridViewCommandEventArgs e)
        {
           

            switch (e.CommandName)
            {
                case "seleziona":
                    string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
                    int id = Convert.ToInt32(commandArgs[0]);
                    int tipo = Convert.ToInt32(commandArgs[2]);

                    FiguraProfessionale figuraProfessionaleSelezionata;

                    if (id == 0)
                    {
                        long identificatoreOggetto = Convert.ToInt64(commandArgs[1]);
                        figuraProfessionaleSelezionata = SessionManager.ListaCompletaFigProf.FirstOrDefault(x => x.IdentificatoreOggetto == identificatoreOggetto);
                    }
                    else
                    {
                        figuraProfessionaleSelezionata = SessionManager.ListaCompletaFigProf.FirstOrDefault(x => x.Id == id && x.Tipo == tipo);
                    }

                    div_FiguraProfessionale.Visible = true;

                    lbl_NominativoFiguraProfessionale.Text = figuraProfessionaleSelezionata.NominativoCompleto;

                    txt_FPnotaCollaboratore.Text = figuraProfessionaleSelezionata.Nota;
                    txt_FPtelefono.Text = figuraProfessionaleSelezionata.Telefono;
                    ViewState[VIEWSTATE_FP_DETTAGLIOECONOMICO] = figuraProfessionaleSelezionata;
                    break;
            }

            upModificaArticolo.Update();
        }
        #endregion

        #region OPERAZIONI LAVORAZIONE
        private void PopolaCombo()
        {
            Esito esito = new Esito();

            #region CAPITECNICI
            List<Anag_Qualifiche_Collaboratori> listaCapiTecnici = SessionManager.ListaQualificheCollaboratori.Where(x => x.Qualifica == "Capo Tecnico").ToList<Anag_Qualifiche_Collaboratori>();

            List<Anag_Collaboratori> listaAnagraficheCapiTecnici = (from Item1 in SessionManager.ListaAnagraficheCollaboratori
                                                                    join Item2 in listaCapiTecnici
                                                                    on Item1.Id equals Item2.Id_collaboratore
                                                                    select Item1).ToList();
            ddl_Capotecnico.Items.Clear();
            foreach (Anag_Collaboratori capoTecnico in listaAnagraficheCapiTecnici)
            {
                ddl_Capotecnico.Items.Add(new ListItem(capoTecnico.Nome + " " + capoTecnico.Cognome, capoTecnico.Id.ToString()));
            }
            ddl_Capotecnico.Items.Insert(0, new ListItem("<seleziona>", ""));
            #endregion

            #region PRODUTTORI
            List<Anag_Qualifiche_Collaboratori> listaProduttori = SessionManager.ListaQualificheCollaboratori.Where(x => x.Qualifica == "Produttore").ToList<Anag_Qualifiche_Collaboratori>();

            List<Anag_Collaboratori> listaAnagraficheProduttori = (from Item1 in SessionManager.ListaAnagraficheCollaboratori
                                                                   join Item2 in listaProduttori on Item1.Id equals Item2.Id_collaboratore
                                                                   select Item1).ToList();
            ddl_Produttore.Items.Clear();
            foreach (Anag_Collaboratori produttore in listaAnagraficheProduttori)
            {
                ddl_Produttore.Items.Add(new ListItem(produttore.Nome + " " + produttore.Cognome, produttore.Id.ToString()));
            }
            ddl_Produttore.Items.Insert(0, new ListItem("<seleziona>", ""));
            #endregion

            #region TIPOPAGAMENTO

            ddl_FPtipoPagamento.Items.Clear();
            foreach (Tipologica tipoPagamento in SessionManager.ListaTipiPagamento)
            {
                ddl_FPtipoPagamento.Items.Add(new ListItem(tipoPagamento.nome, tipoPagamento.id.ToString()));
            }
            ddl_FPtipoPagamento.Items.Insert(0, new ListItem("<seleziona>", ""));
            #endregion

            #region QUALIFICHE

            ddl_FPqualifica.Items.Clear();
            foreach (Tipologica qualifica in SessionManager.ListaQualifiche)
            {
                ddl_FPqualifica.Items.Add(new ListItem(qualifica.nome, qualifica.id.ToString()));
            }
            ddl_FPqualifica.Items.Insert(0, new ListItem("<seleziona>", ""));
            #endregion
        }
        

        protected void CercaFP()
        {
            Esito esito = new Esito();

            string tipoFP = ddl_FPtipo.SelectedValue;
            string qualificaFP = ddl_FPqualifica.SelectedItem.Text;
            string cittaFP = txt_FPCitta.Text.ToLower().Trim();
            string nominativoFP = txt_FPNominativo.Text;

            List<FiguraProfessionale> listaFPfiltrata = SessionManager.ListaCompletaFigProf;

            if (tipoFP != "")
            {
                listaFPfiltrata = listaFPfiltrata.Where(x => x.Tipo == int.Parse(tipoFP)).ToList();
            }

            if (qualificaFP != "<seleziona>")
            {
                listaFPfiltrata = listaFPfiltrata.Where(x => x.Qualifiche.Where(y => y.Qualifica == qualificaFP).Count() > 0).ToList();
            }

            if (!string.IsNullOrEmpty(cittaFP))
            {
                listaFPfiltrata = listaFPfiltrata.Where(x => x.Citta.ToLower().Trim().Contains(cittaFP)).ToList();
            }

            if (!string.IsNullOrEmpty(nominativoFP))
            {
                listaFPfiltrata = listaFPfiltrata.Where(x => (x.NominativoCompleto).ToLower().Trim().Contains(nominativoFP)).ToList();
            }

            gv_FigureProfessionaliModifica.DataSource = listaFPfiltrata;
            gv_FigureProfessionaliModifica.DataBind();

            upModificaArticolo.Update();
        }

        //protected void visualizzaFP(object sender, EventArgs e)
        //{
        //    FiguraProfessionale fpSelezionata = new FiguraProfessionale();
        //    if (ddl_FPnominativo.SelectedValue != "")
        //    {
        //        fpSelezionata = SessionManager.ListaCompletaFigProf.FirstOrDefault(x => x.Id == int.Parse(ddl_FPnominativo.SelectedValue));
        //        //AbilitaComponentiFiguraProfessionale(fpSelezionata, true);
        //    }
        //    else
        //    {
        //        //AbilitaComponentiFiguraProfessionale(null, true);
        //    }

        //    txt_FPtelefono.Text = fpSelezionata.Telefono;
        //    txt_FPnotaCollaboratore.Text = fpSelezionata.Nota;

        //    upModificaArticolo.Update();
        //}

        //private void PopolaNominativi(List<FiguraProfessionale> listaNominativi)
        //{
        //    ddl_FPnominativo.Items.Clear();
        //    foreach (FiguraProfessionale figPro in listaNominativi)
        //    {
        //        ddl_FPnominativo.Items.Add(new ListItem(figPro.Cognome + " " + figPro.Nome, figPro.Id.ToString()));
        //    }
        //    ddl_FPnominativo.Items.Insert(0, new ListItem("<seleziona>", ""));
        //}

        private void AggiungiArticoliDelGruppoAListaArticoli(int idGruppo)
        {
            Esito esito = new Esito();
            int idLavorazione = SessionManager.EventoSelezionato.LavorazioneCorrente == null ? 0 : SessionManager.EventoSelezionato.LavorazioneCorrente.Id;

            SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.AddRange(Articoli_BLL.Instance.CaricaListaArticoliLavorazioneByIDGruppo(idLavorazione, idGruppo, ref esito));
            SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.OrderByDescending(x => x.Prezzo).ToList();

            lbl_selezionareArticolo.Visible = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione == null || SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Count == 0;
        }

        private void AggiungiArticoloAListaArticoli(int idArticolo)
        {
            Esito esito = new Esito();
            int idLavorazione = SessionManager.EventoSelezionato.LavorazioneCorrente == null ? 0 : SessionManager.EventoSelezionato.LavorazioneCorrente.Id;

            DatiArticoliLavorazione articoloLavorazione = Articoli_BLL.Instance.CaricaArticoloLavorazioneByID(idLavorazione, idArticolo, ref esito);

            SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Add(articoloLavorazione);
            SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.OrderByDescending(x => x.Prezzo).ToList();

            lbl_selezionareArticolo.Visible = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione == null || SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Count == 0;
        }

        private void AggiungiDiariaAListaArticoli(decimal importoDiaria, FiguraProfessionale figuraProfessionale)
        {
            Esito esito = new Esito();
            Art_Articoli articoloDiaria = Articoli_BLL.Instance.getDiaria(ref esito);

            if (esito.codice != Esito.ESITO_OK || articoloDiaria == null)
            {
                basePage.ShowWarning("La diaria non è stata aggiunta agli articoli del dettaglio economico");
            }
            else
            {
                int idDiaria = articoloDiaria.Id;

                int idLavorazione = SessionManager.EventoSelezionato.LavorazioneCorrente == null ? 0 : SessionManager.EventoSelezionato.LavorazioneCorrente.Id;

                DatiArticoliLavorazione articoloLavorazioneDiaria = Articoli_BLL.Instance.CaricaArticoloLavorazioneByID(idLavorazione, idDiaria, ref esito);
                articoloLavorazioneDiaria.Costo = articoloLavorazioneDiaria.Prezzo = importoDiaria;

                if (figuraProfessionale.Tipo == COLLABORATORE)
                {
                    articoloLavorazioneDiaria.IdCollaboratori = figuraProfessionale.IdCollaboratori;
                    articoloLavorazioneDiaria.IdFornitori = null;

                    // elimino l'eventuale diaria già presente
                    DatiArticoliLavorazione diariaDaEliminare = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.FirstOrDefault(x => x.IdArtArticoli == idDiaria && x.IdCollaboratori == figuraProfessionale.IdCollaboratori);
                    SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Remove(diariaDaEliminare);
                }
                else if (figuraProfessionale.Tipo == FORNITORE)
                {
                    articoloLavorazioneDiaria.IdCollaboratori = null;
                    articoloLavorazioneDiaria.IdFornitori = figuraProfessionale.IdFornitori;

                    // elimino l'eventuale diaria già presente
                    DatiArticoliLavorazione diariaDaEliminare = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.FirstOrDefault(x => x.IdArtArticoli == idDiaria && x.IdFornitori == figuraProfessionale.IdFornitori);
                    SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Remove(diariaDaEliminare);
                }

                SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Add(articoloLavorazioneDiaria);
                SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.OrderByDescending(x => x.Prezzo).ToList();

                lbl_selezionareArticolo.Visible = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione == null || SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Count == 0; }
        }

        private void CancellaDiariaDaListaArticoli(FiguraProfessionale figuraProfessionale)
        {
            Esito esito = new Esito();
            Art_Articoli articoloDiaria = Articoli_BLL.Instance.getDiaria(ref esito);

            if (esito.codice != Esito.ESITO_OK || articoloDiaria == null)
            {
                basePage.ShowWarning("La diaria non è stata rimossa dagli articoli del dettaglio economico");
            }
            else
            {
                int idDiaria = articoloDiaria.Id;
                int idLavorazione = SessionManager.EventoSelezionato.LavorazioneCorrente == null ? 0 : SessionManager.EventoSelezionato.LavorazioneCorrente.Id;

                if (figuraProfessionale.Tipo == COLLABORATORE)
                {
                    // elimino la diaria già presente
                    DatiArticoliLavorazione diariaDaEliminare = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.FirstOrDefault(x => x.IdArtArticoli == idDiaria && x.IdCollaboratori == figuraProfessionale.IdCollaboratori);
                    SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Remove(diariaDaEliminare);
                }
                else if (figuraProfessionale.Tipo == FORNITORE)
                {
                    // elimino la diaria già presente
                    DatiArticoliLavorazione diariaDaEliminare = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.FirstOrDefault(x => x.IdArtArticoli == idDiaria && x.IdFornitori == figuraProfessionale.IdFornitori);
                    SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Remove(diariaDaEliminare);
                }

                SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.OrderByDescending(x => x.Prezzo).ToList();

                lbl_selezionareArticolo.Visible = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione == null || SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Count == 0;
            }
        }

        public void PopolaLavorazione()
        {
            Esito esito = new Esito();

            int idDatiAgenda = SessionManager.EventoSelezionato.id;
            int idCliente = SessionManager.EventoSelezionato.id_cliente;

            SessionManager.EventoSelezionato.LavorazioneCorrente = Dati_Lavorazione_BLL.Instance.getDatiLavorazioneByIdEvento(idDatiAgenda, ref esito);
            if (esito.codice != Esito.ESITO_OK)
            {
                basePage.ShowError(esito.descrizione);
            }
            else if (SessionManager.EventoSelezionato.LavorazioneCorrente == null)
            {
                basePage.ShowWarning("La lavorazione corrente non contiene dati.<br/> Verrà creata una nuova lavorazione a partire dai dati dell'offerta.");
                TrasformaInLavorazione();
            }
            else
            {
                int idContratto = SessionManager.ListaTipiProtocolli.FirstOrDefault(x => x.nome.ToLower() == "contratto").id;
                List<Protocolli> listaContratti = Protocolli_BLL.Instance.GetProtocolliByIdCliente(ref esito, idCliente).Where(x=>x.Id_tipo_protocollo == idContratto).ToList();

                if (listaContratti.Count() == 0)
                {
                    ddl_Contratto.Items.Clear();
                    ddl_Contratto.Items.Add(new ListItem("<nessun contratto disponibile>", ""));
                }
                else
                {
                    ddl_Contratto.DataSource = listaContratti;
                    ddl_Contratto.DataTextField = "numero_protocollo";
                    ddl_Contratto.DataValueField = "id";
                    ddl_Contratto.DataBind();
                    ddl_Contratto.Items.Insert(0, new ListItem("<seleziona>", ""));
                    
                }

                SessionManager.ListaReferenti = Anag_Referente_Clienti_Fornitori_BLL.Instance.getReferentiByIdAzienda(ref esito, idCliente);
                ListaFigureProfessionali = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaFigureProfessionali; //in questo modo assegno identificatoreOggetto

                ddl_intervento.DataSource = SessionManager.ListaTipiIntervento.OrderBy(x => x.id);
                ddl_intervento.DataTextField = "nome";
                ddl_intervento.DataValueField = "id";
                ddl_intervento.DataBind();

                ddl_intervento_InsGenerale.DataSource = SessionManager.ListaTipiIntervento.OrderBy(x => x.id);
                ddl_intervento_InsGenerale.DataTextField = "nome";
                ddl_intervento_InsGenerale.DataValueField = "id";
                ddl_intervento_InsGenerale.DataBind();

                txt_data_InsGenerale.Text = ((DateTime)SessionManager.EventoSelezionato.data_inizio_lavorazione).ToString("dd/MM/yyyy");


                if (esito.codice != Esito.ESITO_OK)
                {
                    basePage.ShowError(esito.descrizione);
                }
                else
                {
                    CreaNuovaLavorazione(SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione);

                    if (SessionManager.EventoSelezionato.LavorazioneCorrente != null)
                    {
                        txt_Ordine.Text = SessionManager.EventoSelezionato.LavorazioneCorrente.Ordine;
                        txt_Fattura.Text = SessionManager.EventoSelezionato.LavorazioneCorrente.Fattura;
                        ddl_Contratto.SelectedValue = SessionManager.EventoSelezionato.LavorazioneCorrente.IdContratto == null ? "" : SessionManager.EventoSelezionato.LavorazioneCorrente.IdContratto.ToString();
                        ddl_Referente.SelectedValue = SessionManager.EventoSelezionato.LavorazioneCorrente.IdReferente == null ? "" : SessionManager.EventoSelezionato.LavorazioneCorrente.IdReferente.ToString();
                        ddl_Capotecnico.SelectedValue = SessionManager.EventoSelezionato.LavorazioneCorrente.IdCapoTecnico == null ? "" : SessionManager.EventoSelezionato.LavorazioneCorrente.IdCapoTecnico.ToString();
                        ddl_Produttore.SelectedValue = SessionManager.EventoSelezionato.LavorazioneCorrente.IdProduttore == null ? "" : SessionManager.EventoSelezionato.LavorazioneCorrente.IdProduttore.ToString();

                        SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione;

                        gvArticoliLavorazione.DataSource = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione;
                        gvArticoliLavorazione.DataBind();

                        gvFigProfessionali.DataSource = ListaFigureProfessionali;
                        gvFigProfessionali.DataBind();

                        txt_notaGeneralePianoEsterno.Text = SessionManager.EventoSelezionato.LavorazioneCorrente.NotePianoEsterno;
                    }
                    lbl_selezionareArticolo.Visible = (SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione == null ||
                                                       SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Count == 0);
                    AggiornaTotali();
                }
            }
        }

        public void CreaNuovaLavorazione(List<DatiArticoliLavorazione> listaArticoliLavorazione)
        {
            Esito esito = new Esito();

            txt_Ordine.Text = string.Empty;
            txt_Fattura.Text = string.Empty;
            ddl_Contratto.SelectedIndex = 0;  
            ddl_Referente.SelectedIndex = 0;
            ddl_Capotecnico.SelectedIndex = 0;
            ddl_Produttore.SelectedIndex = 0;
            txt_notaGeneralePianoEsterno.Text = string.Empty;

            ddl_Referente.Items.Clear();
            foreach (Anag_Referente_Clienti_Fornitori referente in SessionManager.ListaReferenti)
            {
                ddl_Referente.Items.Add(new ListItem(referente.Nome + " " + referente.Cognome, referente.Id.ToString()));
            }
            ddl_Referente.Items.Insert(0, new ListItem("<seleziona>", ""));

            if (SessionManager.EventoSelezionato.LavorazioneCorrente == null)
            {
                SessionManager.EventoSelezionato.LavorazioneCorrente = new DatiLavorazione();
                SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione = new List<DatiArticoliLavorazione>();
                SessionManager.EventoSelezionato.LavorazioneCorrente.ListaDatiPianoEsternoLavorazione = new List<DatiPianoEsternoLavorazione>();
            }

            SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione = listaArticoliLavorazione;

            lbl_selezionareArticolo.Visible = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione == null || SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Count == 0;
            lbl_nessunaFiguraProf.Visible = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaFigureProfessionali == null || SessionManager.EventoSelezionato.LavorazioneCorrente.ListaFigureProfessionali.Count == 0;
            gvArticoliLavorazione.DataSource = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione;
            gvArticoliLavorazione.DataBind();
        }

        public DatiLavorazione CreaDatiLavorazione()
        {
            DatiLavorazione datiLavorazione = new DatiLavorazione();
            datiLavorazione.Id = SessionManager.EventoSelezionato.LavorazioneCorrente == null ? 0 : SessionManager.EventoSelezionato.LavorazioneCorrente.Id;

            datiLavorazione.Ordine = txt_Ordine.Text;
            datiLavorazione.Fattura = txt_Fattura.Text;
            datiLavorazione.IdContratto = string.IsNullOrEmpty(ddl_Contratto.SelectedValue) ? null : (int?)int.Parse(ddl_Contratto.SelectedValue);
            datiLavorazione.IdReferente = string.IsNullOrEmpty(ddl_Referente.SelectedValue) ? null : (int?)int.Parse(ddl_Referente.SelectedValue); 
            datiLavorazione.IdCapoTecnico = string.IsNullOrEmpty(ddl_Capotecnico.SelectedValue) ? null : (int?)int.Parse(ddl_Capotecnico.SelectedValue); 
            datiLavorazione.IdProduttore = string.IsNullOrEmpty(ddl_Produttore.SelectedValue) ? null : (int?)int.Parse(ddl_Produttore.SelectedValue); 
            datiLavorazione.ListaArticoliLavorazione = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione;
            datiLavorazione.ListaDatiPianoEsternoLavorazione = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaDatiPianoEsternoLavorazione;
            datiLavorazione.NotePianoEsterno = txt_notaGeneralePianoEsterno.Text;
            return datiLavorazione;
        }

        public void TrasformaInLavorazione()
        {
            SessionManager.EventoSelezionato.id_stato = Stato.Instance.STATO_LAVORAZIONE;

            // COSTRUISCO LISTA DATI ARTICOLI LAVORAZIONE
            List<DatiArticoliLavorazione> listaDatiArticoliLavorazione = new List<DatiArticoliLavorazione>();

            foreach (DatiArticoli datoArticolo in SessionManager.EventoSelezionato.ListaDatiArticoli)
            {
                for (int i = 0; i < datoArticolo.Quantita; i++)
                {
                    bool firstTime;

                    DatiArticoliLavorazione datoArticoloLavorazione = new DatiArticoliLavorazione();
                    datoArticoloLavorazione.Id = 0;
                    datoArticoloLavorazione.IdentificatoreOggetto = IDGenerator.GetId(datoArticoloLavorazione, out firstTime);
                    datoArticoloLavorazione.IdDatiLavorazione = 0;
                    datoArticoloLavorazione.IdArtArticoli = datoArticolo.IdArtArticoli;
                    datoArticoloLavorazione.IdTipoGenere = datoArticolo.IdTipoGenere;
                    datoArticoloLavorazione.IdTipoGruppo = datoArticolo.IdTipoGruppo;
                    datoArticoloLavorazione.IdTipoSottogruppo = datoArticolo.IdTipoSottogruppo;

                    datoArticoloLavorazione.IdCollaboratori = null;
                    datoArticoloLavorazione.IdFornitori = null;
                    datoArticoloLavorazione.IdTipoPagamento = null;

                    datoArticoloLavorazione.Descrizione = datoArticolo.Descrizione;
                    datoArticoloLavorazione.DescrizioneLunga = datoArticolo.DescrizioneLunga;
                    datoArticoloLavorazione.Stampa = datoArticolo.Stampa;
                    datoArticoloLavorazione.Prezzo = datoArticolo.Prezzo;
                    datoArticoloLavorazione.Costo = datoArticolo.Costo;
                    datoArticoloLavorazione.Iva = datoArticolo.Iva;

                    datoArticoloLavorazione.Data = SessionManager.EventoSelezionato.data_inizio_lavorazione;

                    listaDatiArticoliLavorazione.Add(datoArticoloLavorazione);
                }
            }
            CreaNuovaLavorazione(listaDatiArticoliLavorazione);
        }
        #endregion

        #region OPERAZIONI PAGINA
        private void ClearPanelModificaArticolo()
        {
            txt_Descrizione.Text =
            txt_Prezzo.Text =
            txt_Costo.Text =
            txt_Iva.Text =
            txt_DescrizioneLunga.Text =
            txt_FPCitta.Text =
            txt_FPNominativo.Text =
            txt_FPnotaCollaboratore.Text =
            txt_FPtelefono.Text =
            txt_FPnetto.Text =
            txt_FPRimborsoKM.Text =
            txt_FPlordo.Text = "";

            ddl_Stampa.SelectedIndex = 
            ddl_FPtipo.SelectedIndex =
            ddl_FPqualifica.SelectedIndex =
            ddl_FPtipoPagamento.SelectedIndex = 0;

            hf_IdFiguraProfessionale.Value = "";

            gv_FigureProfessionaliModifica.DataSource = null;
            gv_FigureProfessionaliModifica.DataBind();

            div_FiguraProfessionale.Visible = false;
        }

        private void ClearPanelInserimentoGenerale()
        {
            txt_data_InsGenerale.Text = ((DateTime)SessionManager.EventoSelezionato.data_inizio_lavorazione).ToString("dd/MM/yyyy");
            txt_orario_InsGenerale.Text =  
            txt_diaria_InsGenerale.Text = "";

            chk_diaria_InsGenerale.Checked = 
            chk_albergo_InsGenerale.Checked = false;

            diaria15_InsGenerale.Checked =
            diaria30_InsGenerale.Checked =
            diariaLibera_InsGenerale.Checked = false;

            ddl_intervento_InsGenerale.SelectedIndex = 0;

            RichiediOperazionePopup("UPDATE");
        }

        private void AbilitaCostoFP(bool abilita)
        {
            if (abilita)
            {
                txt_FPnetto.Attributes.Remove("readonly");
                txt_FPnetto.CssClass = "w3-input w3-border";
                txt_Costo.Attributes.Add("readonly", "readonly");
                txt_Costo.CssClass = "w3-input w3-border w3-disabled";

                txt_Costo.Text = "";
            }
            else
            {
                txt_FPnetto.Attributes.Add("readonly", "readonly");
                txt_FPnetto.CssClass = "w3-input w3-border w3-disabled";
                txt_Costo.Attributes.Remove("readonly");
                txt_Costo.CssClass = "w3-input w3-border";

                txt_FPnetto.Text = "";
                txt_FPRimborsoKM.Text = "";
                txt_FPlordo.Text = "";
            }
        }

        public void ClearLavorazione()
        {
            lbl_selezionareArticolo.Visible = true;
            lbl_nessunaFiguraProf.Visible = true;
            if (SessionManager.EventoSelezionato.LavorazioneCorrente != null)
            {
                SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione = null;
            }
            gvArticoliLavorazione.DataSource = null;
            gvArticoliLavorazione.DataBind();

            gvFigProfessionali.DataSource = null;
            gvFigProfessionali.DataBind();

            txt_TotPrezzo.Text = "";
            txt_TotCosto.Text = "";
            txt_TotLordo.Text = "";
            txt_TotIva.Text = "";
            txt_PercRicavo.Text = "";

            ResetPanelLavorazione();
        }
        //private void AbilitaComponentiCosto(DatiArticoliLavorazione articoloSelezionato)
        //{
        //    // GESTIONE COSTO FIG PROF
        //    if (articoloSelezionato.UsaCostoFP != null && (bool)articoloSelezionato.UsaCostoFP)
        //    {
        //        txt_Costo.Attributes.Add("readonly", "readonly");
        //        txt_Costo.CssClass = "w3-input w3-border w3-disabled";
        //        txt_FPnetto.Attributes.Remove("readonly");
        //        txt_FPnetto.CssClass = "w3-input w3-border";

        //        txt_FPnetto.Text = articoloSelezionato.FP_netto.ToString();
        //        txt_FPlordo.Text = articoloSelezionato.FP_lordo.ToString();
        //    }
        //    else
        //    {
        //        txt_Costo.Attributes.Remove("readonly");
        //        txt_Costo.CssClass = "w3-input w3-border";
        //        txt_FPnetto.Attributes.Add("readonly", "readonly");
        //        txt_FPnetto.CssClass = "w3-input w3-border w3-disabled";

        //        txt_FPnetto.Text = "";
        //        txt_FPRimborsoKM.Text = "";
        //        txt_FPlordo.Text = "";
        //    }
        //}

        private void AggiornaTotali()
        {
            decimal totPrezzo = 0;
            decimal totCosto = 0;
            decimal? totLordo = 0;
            decimal totIva = 0;
            decimal percRicavo = 0;

            if (SessionManager.EventoSelezionato.LavorazioneCorrente != null && SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione != null && SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Count > 0)
            {
                foreach (DatiArticoliLavorazione art in SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione)
                {
                    if (art.UsaCostoFP != null)
                    {
                        if ((bool)art.UsaCostoFP)
                        {
                            totCosto += art.FP_netto != null ? (decimal)art.FP_netto : 0;
                        }
                        else
                        {
                            totCosto += (decimal)art.Costo;
                        }
                    }
                    else
                    {
                        totCosto += (decimal)art.Costo;
                    }

                    totPrezzo += art.Prezzo;
                    totLordo += art.FP_lordo;
                    totIva += (art.Prezzo * art.Iva / 100);
                }

                if (totPrezzo != 0)
                {
                    percRicavo = ((totPrezzo - totCosto) / totPrezzo) * 100;
                }
            }

            txt_TotPrezzo.Text = string.Format("{0:N2}", totPrezzo);
            txt_TotCosto.Text = string.Format("{0:N2}", totCosto);
            txt_TotLordo.Text = string.Format("{0:N2}", totLordo);
            txt_TotIva.Text = string.Format("{0:N2}", totIva);
            txt_PercRicavo.Text = string.Format("{0:N2}", percRicavo);
        }

        private void ResetPanelLavorazione()
        {
            gvArticoliLavorazione.DataSource = SessionManager.EventoSelezionato.LavorazioneCorrente == null ? null : SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione;
            gvArticoliLavorazione.DataBind();

            lbl_selezionareArticolo.Visible = (SessionManager.EventoSelezionato.LavorazioneCorrente == null ||
                                               SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione == null ||
                                               SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Count == 0);

            lbl_nessunaFiguraProf.Visible = (SessionManager.EventoSelezionato.LavorazioneCorrente == null ||
                                             ListaFigureProfessionali == null ||
                                             ListaFigureProfessionali.Count == 0);

            AggiornaTotali();
        }
        #endregion


        private void CaricaListaDatiPianoEsternoLavorazione(List<DatiArticoliLavorazione> _listaCollaboratoriFornitori)
        {
            List<FiguraProfessionale> _listaFigureProfessionali = new List<FiguraProfessionale>();
            List<DatiPianoEsternoLavorazione> _listaDatiPianoEsterno = new List<DatiPianoEsternoLavorazione>();
            foreach (DatiArticoliLavorazione collabForn in _listaCollaboratoriFornitori)
            {
                FiguraProfessionale figProf = new FiguraProfessionale();
                DatiPianoEsternoLavorazione datiPianoEsterno = new DatiPianoEsternoLavorazione();
                if (collabForn.IdCollaboratori != null) //COLLABORATORI
                {
                    Anag_Collaboratori collaboratore = SessionManager.ListaAnagraficheCollaboratori.FirstOrDefault(x => x.Id == collabForn.IdCollaboratori);

                    figProf = collaboratore.CreaFiguraProfessionale();
                    figProf.IdCollaboratori = collabForn.IdCollaboratori;

                    datiPianoEsterno.IdDatiLavorazione = SessionManager.EventoSelezionato.LavorazioneCorrente.Id;
                    datiPianoEsterno.IdCollaboratori = collaboratore.Id;
                }
                else //FORNITORI
                {
                    Anag_Clienti_Fornitori fornitore = SessionManager.ListaAnagraficheFornitori.FirstOrDefault(x => x.Id == collabForn.IdFornitori);

                    figProf = fornitore.CreaFiguraProfessionale();
                    figProf.IdFornitori = collabForn.IdFornitori;

                    datiPianoEsterno.IdDatiLavorazione = SessionManager.EventoSelezionato.LavorazioneCorrente.Id;
                    datiPianoEsterno.IdFornitori = fornitore.Id;
                }

                bool firstTime;
                figProf.IdentificatoreOggetto = IDGenerator.GetId(figProf, out firstTime);

                figProf.Nota = collabForn.Nota;
                figProf.Netto = collabForn.FP_netto;
                figProf.Lordo = collabForn.FP_lordo;
                figProf.Data = datiPianoEsterno.Data = SessionManager.EventoSelezionato.data_inizio_lavorazione;

                if (ListaFigureProfessionali.FirstOrDefault(x => x.IdentificatoreOggetto == figProf.IdentificatoreOggetto) == null)
                {
                    _listaFigureProfessionali.Add(figProf);
                }
                else
                {
                    FiguraProfessionale figProfGiaPresente = ListaFigureProfessionali.FirstOrDefault(x => x.IdentificatoreOggetto == figProf.IdentificatoreOggetto);

                    if (figProfGiaPresente.Nome != figProf.Nome ||
                        figProfGiaPresente.Cognome != figProf.Cognome)
                    {
                        ListaFigureProfessionali.Remove(figProfGiaPresente);
                        _listaFigureProfessionali.Add(figProf);
                    }
                }
                _listaDatiPianoEsterno.Add(datiPianoEsterno);
            }
            ListaFigureProfessionali =_listaFigureProfessionali;
            SessionManager.EventoSelezionato.LavorazioneCorrente.ListaDatiPianoEsternoLavorazione = _listaDatiPianoEsterno;
        }
    }
}