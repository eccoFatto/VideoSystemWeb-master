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

        #region PARAMETRI CONFIGURAZIONE
        public string aliquota_RitenutaAcconto;
        public string quotaFissa_PagamentoMisto;
        public string diariaLorda;
        #endregion

        #region VARIABILI LOCALI
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
        List<FiguraProfessionale> ListaFigureProfessionali  // Figure Professionali piano esterno
        {
            get
            {
                if (ViewState[VIEWSTATE_LISTAFIGUREPROFESSIONALI] == null)
                {
                    ViewState[VIEWSTATE_LISTAFIGUREPROFESSIONALI] = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaFigureProfessionali;
                }
                return ((List<FiguraProfessionale>)ViewState[VIEWSTATE_LISTAFIGUREPROFESSIONALI]);
            }
            set
            {
                List<FiguraProfessionale> _listaFigureProfessionali = value;
                if (_listaFigureProfessionali != null)
                {
                    foreach (FiguraProfessionale figuraProfessionale in _listaFigureProfessionali)
                    {
                        figuraProfessionale.IdentificatoreOggetto = IDGenerator.GetId(figuraProfessionale, out bool firstTime);
                    }
                }

                ViewState[VIEWSTATE_LISTAFIGUREPROFESSIONALI] = _listaFigureProfessionali;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                gvGruppiLavorazione.DataSource = ListaArticoliGruppiLavorazione;
                gvGruppiLavorazione.DataBind();

                PopolaCombo();

                PopolaParametriConfigurazione();
            }
            else
            {
                // SELEZIONO L'ULTIMA TAB SELEZIONATA
                ScriptManager.RegisterStartupScript(Page, GetType(), "apriTabGiusta", script: "openTabEventoLavorazione(event,'" + hf_tabSelezionataLavorazione.Value + "')", addScriptTags: true);
            }
        }


        #region DETTAGLIO ECONOMICO
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

                    txt_DataArticolo.Text = ((DateTime)articoloSelezionato.Data).ToString("dd/MM/yyyy");
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
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "abilitaAnimazione", script: "javascript: abilitaAnimazione('true');", addScriptTags: true);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriModificaArticolo", script: "javascript: document.getElementById('" + panelModificaArticolo.ClientID + "').style.display='block'", addScriptTags: true);


                    break;
                case "elimina":
                    SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Remove(articoloSelezionato);
                    gvArticoliLavorazione.DataSource = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione;
                    gvArticoliLavorazione.DataBind();

                    // AggiornaTotali();
                    ResetPanelLavorazione();
                    if (ListaFigureProfessionali.Count > 0)
                    {
                        ImportaFigProfInPianoEsterno(); // elimino elemento anche in piano esterno
                    }

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
            ddl_FiltroGiorniLavorazione.SelectedIndex = 0;

            RichiediOperazionePopup("UPDATE");
        }

        protected void ddl_FiltroGiorniLavorazioneDettEcon_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_filtroGiorniLavorazioneDettEcon.SelectedItem.Value == "")
            {
                gvArticoliLavorazione.DataSource = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione;
                gvArticoliLavorazione.DataBind();
            }
            else
            {
                DateTime giornoLavorazione = SessionManager.EventoSelezionato.data_inizio_lavorazione.AddDays(int.Parse(ddl_filtroGiorniLavorazioneDettEcon.SelectedItem.Value));
                List<DatiArticoliLavorazione> listaAtricoliLavorazioneFiltrati = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Where(x => x.Data == giornoLavorazione).ToList<DatiArticoliLavorazione>();

                gvArticoliLavorazione.DataSource = listaAtricoliLavorazioneFiltrati;
                gvArticoliLavorazione.DataBind();
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

            articoloSelezionato.Data = DateTime.Parse(txt_DataArticolo.Text);
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
            articoloSelezionato.FP_lordo = string.IsNullOrEmpty(txt_FPlordo.Text) ? 0 : decimal.Parse(txt_FPlordo.Text);

            FiguraProfessionale figuraProfessionaleSelezionata = (FiguraProfessionale)ViewState[VIEWSTATE_FP_DETTAGLIOECONOMICO];

            if (figuraProfessionaleSelezionata != null && figuraProfessionaleSelezionata.Tipo == COLLABORATORE)
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

            //SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.OrderBy(y => y.Data).ThenByDescending(x => x.Prezzo).ToList();

            ResetPanelLavorazione();
            if (ListaFigureProfessionali.Count > 0)
            {
                ImportaFigProfInPianoEsterno(); // modifico elemento anche in piano esterno
            }

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

        protected void btn_Cerca_Click(object sender, EventArgs e)
        {
            CercaFP();
        }





        protected void CercaFP()
        {
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
                listaFPfiltrata = listaFPfiltrata.Where(x => x.Qualifiche != null && x.Qualifiche.Where(y => y.Qualifica == qualificaFP).Count() > 0).ToList();
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
            int numGiorniLavorazione = SessionManager.EventoSelezionato.durata_lavorazione;
            Esito esito = new Esito();
            int idLavorazione = SessionManager.EventoSelezionato.LavorazioneCorrente == null ? 0 : SessionManager.EventoSelezionato.LavorazioneCorrente.Id;

            DatiArticoliLavorazione articoloLavorazione = Articoli_BLL.Instance.CaricaArticoloLavorazioneByID(idLavorazione, idArticolo, null, ref esito);

            for (int giornoLav = 0; giornoLav < numGiorniLavorazione; giornoLav++)
            {
                long maxIdentificatoreOggetto = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Max(x => x.IdentificatoreOggetto);

                DateTime dataGiornoLav = SessionManager.EventoSelezionato.data_inizio_lavorazione.AddDays(giornoLav);
                DatiArticoliLavorazione datiArticoliLav = new DatiArticoliLavorazione();

                datiArticoliLav.Id = articoloLavorazione.Id;
                datiArticoliLav.IdentificatoreOggetto = IDGenerator.GetId(datiArticoliLav, out bool firstTime) + maxIdentificatoreOggetto;
                datiArticoliLav.IdDatiLavorazione = articoloLavorazione.IdDatiLavorazione;
                datiArticoliLav.IdArtArticoli = articoloLavorazione.IdArtArticoli;
                datiArticoliLav.IdTipoGenere = articoloLavorazione.IdTipoGenere;
                datiArticoliLav.IdTipoGruppo = articoloLavorazione.IdTipoGruppo;
                datiArticoliLav.IdTipoSottogruppo = articoloLavorazione.IdTipoSottogruppo;
                datiArticoliLav.IdCollaboratori = articoloLavorazione.IdCollaboratori;
                datiArticoliLav.IdFornitori = articoloLavorazione.IdFornitori;
                datiArticoliLav.IdTipoPagamento = articoloLavorazione.IdTipoPagamento;
                datiArticoliLav.Descrizione = articoloLavorazione.Descrizione;
                datiArticoliLav.DescrizioneLunga = articoloLavorazione.DescrizioneLunga;
                datiArticoliLav.Stampa = articoloLavorazione.Stampa;
                datiArticoliLav.Prezzo = articoloLavorazione.Prezzo;
                datiArticoliLav.Costo = articoloLavorazione.Costo;
                datiArticoliLav.Iva = articoloLavorazione.Iva;
                datiArticoliLav.Data = dataGiornoLav;
                datiArticoliLav.Tv = articoloLavorazione.Tv;
                datiArticoliLav.Nota = articoloLavorazione.Nota;
                datiArticoliLav.FP_netto = articoloLavorazione.FP_netto;
                datiArticoliLav.FP_lordo = articoloLavorazione.FP_lordo;
                datiArticoliLav.UsaCostoFP = articoloLavorazione.UsaCostoFP;

                SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Add(datiArticoliLav);
                SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.OrderBy(y => y.Data).ThenByDescending(x => x.Prezzo).ToList();
            }
            lbl_selezionareArticolo.Visible = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione == null || SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Count == 0;
        }

        private void ClearPanelModificaArticolo()
        {
            txt_DataArticolo.Text =
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

        //Replica elementi dettaglio economico per ogni giorno di lavorazione
        //private void CaricaListaDatiDettaglioEconomico()
        //{
        //    int numGiorniLavorazione = SessionManager.EventoSelezionato.durata_lavorazione;

        //    List<DatiArticoliLavorazione> _listaCollaboratoriFornitori = new List<DatiArticoliLavorazione>();

        //    foreach (DatiArticoliLavorazione collabForn in SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione)
        //    {
        //        for (int giornoLav = 0; giornoLav < numGiorniLavorazione; giornoLav++)
        //        {
        //            DateTime dataGiornoLav = SessionManager.EventoSelezionato.data_inizio_lavorazione.AddDays(giornoLav);

        //            DatiArticoliLavorazione datiArticoliLav = new DatiArticoliLavorazione();

        //            datiArticoliLav.Id = collabForn.Id;
        //            datiArticoliLav.IdentificatoreOggetto = IDGenerator.GetId(datiArticoliLav, out bool firstTime);
        //            datiArticoliLav.IdDatiLavorazione = collabForn.IdDatiLavorazione;
        //            datiArticoliLav.IdArtArticoli = collabForn.IdArtArticoli;
        //            datiArticoliLav.IdTipoGenere = collabForn.IdTipoGenere;
        //            datiArticoliLav.IdTipoGruppo = collabForn.IdTipoGruppo;
        //            datiArticoliLav.IdTipoSottogruppo = collabForn.IdTipoSottogruppo;
        //            datiArticoliLav.IdCollaboratori = collabForn.IdCollaboratori;
        //            datiArticoliLav.IdFornitori = collabForn.IdFornitori;
        //            datiArticoliLav.IdTipoPagamento = collabForn.IdTipoPagamento;
        //            datiArticoliLav.Descrizione = collabForn.Descrizione;
        //            datiArticoliLav.DescrizioneLunga = collabForn.DescrizioneLunga;
        //            datiArticoliLav.Stampa = collabForn.Stampa;
        //            datiArticoliLav.Prezzo = collabForn.Prezzo;
        //            datiArticoliLav.Costo = collabForn.Costo;
        //            datiArticoliLav.Iva = collabForn.Iva;
        //            datiArticoliLav.Data = dataGiornoLav;
        //            datiArticoliLav.Tv = collabForn.Tv;
        //            datiArticoliLav.Nota = collabForn.Nota;
        //            datiArticoliLav.FP_netto = collabForn.FP_netto;
        //            datiArticoliLav.FP_lordo = collabForn.FP_lordo;
        //            datiArticoliLav.UsaCostoFP = collabForn.UsaCostoFP;


        //            _listaCollaboratoriFornitori.Add(datiArticoliLav);
        //        }
        //    }
        //    _listaCollaboratoriFornitori = _listaCollaboratoriFornitori.OrderBy(x => x.Data).ThenByDescending(y => y.Costo).ToList<DatiArticoliLavorazione>();
        //    SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione = _listaCollaboratoriFornitori;
        //}

        #endregion

        #region PIANO ESTERNO
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
                        datiPianoEsterno = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaDatiPianoEsternoLavorazione.FirstOrDefault(x => x.IdCollaboratori == figuraProfessionaleSelezionata.IdCollaboratori && x.Data == figuraProfessionaleSelezionata.Data);
                    }
                    else if (figuraProfessionaleSelezionata.IdFornitori != null && figuraProfessionaleSelezionata.IdFornitori != 0)
                    {
                        datiPianoEsterno = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaDatiPianoEsternoLavorazione.FirstOrDefault(x => x.IdFornitori == figuraProfessionaleSelezionata.IdFornitori && x.Data == figuraProfessionaleSelezionata.Data);
                    }

                    txt_data.Text = figuraProfessionaleSelezionata.Data != null ? ((DateTime)figuraProfessionaleSelezionata.Data).ToString("dd/MM/yyyy") : ((DateTime)SessionManager.EventoSelezionato.data_inizio_lavorazione).ToString("dd/MM/yyyy");
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

                    ddl_intervento.SelectedValue = datiPianoEsterno.IdIntervento == null ? "1" : ((int)datiPianoEsterno.IdIntervento).ToString();
                    chk_albergo.Checked = datiPianoEsterno.Albergo != null && (bool)datiPianoEsterno.Albergo;
                    txt_notaPianoEsterno.Text = figuraProfessionaleSelezionata.Nota;

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
                datiPianoEsterno = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaDatiPianoEsternoLavorazione.FirstOrDefault(x => x.IdCollaboratori == figuraProfessionale.IdCollaboratori && x.Data == figuraProfessionale.Data);
            }
            else if (figuraProfessionale.IdFornitori != null && figuraProfessionale.IdFornitori != 0)
            {
                datiPianoEsterno = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaDatiPianoEsternoLavorazione.FirstOrDefault(x => x.IdFornitori == figuraProfessionale.IdFornitori && x.Data == figuraProfessionale.Data);
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
                AggiungiDiariaAListaArticoli(importoDiaria, figuraProfessionale, DateTime.Parse(txt_data.Text));
            }
            else // Elimino eventuale diaria da listaAricoliLavorazione in dettaglio economico
            {
                CancellaDiariaDaListaArticoli(figuraProfessionale);
            }

            AggiornaTotali();
            ResetPanelLavorazione();



            // prendo indice dell'elemento in dattaglio economico per eventuale modifica
            DatiArticoliLavorazione datoArticoloDaModificare = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.FirstOrDefault(x => x.IdCollaboratori == figuraProfessionale.IdCollaboratori && x.IdFornitori == figuraProfessionale.IdFornitori && x.Data == figuraProfessionale.Data);
            int indiceArticoloDaModificare = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.IndexOf(datoArticoloDaModificare);
            //fine




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

            // eseguo modifica a elemento in dettaglio economico
            if (indiceArticoloDaModificare != -1)
            {
                datoArticoloDaModificare.Data = datiPianoEsterno.Data;
                SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione[indiceArticoloDaModificare] = datoArticoloDaModificare;

                gvArticoliLavorazione.DataSource = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione;
                gvArticoliLavorazione.DataBind();
            }
            // fine

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
                FiguraProfessionale figuraProfessionale = ListaFigureProfessionali.FirstOrDefault(x => x.IdCollaboratori == datiPianoEsternoLavorazione.IdCollaboratori && x.IdFornitori == datiPianoEsternoLavorazione.IdFornitori && x.Data == datiPianoEsternoLavorazione.Data);


                if (importoDiaria > 0) // Aggiungo diaria a listaAricoliLavorazione in dettaglio economico
                {
                    AggiungiDiariaAListaArticoli(importoDiaria, figuraProfessionale, figuraProfessionale.Data);
                }
                else // Elimino eventuale diaria da listaAricoliLavorazione in dettaglio economico
                {
                    CancellaDiariaDaListaArticoli(figuraProfessionale);
                }

                // datiPianoEsternoLavorazione.Data = string.IsNullOrEmpty(txt_data_InsGenerale.Text) ? null : (DateTime?)DateTime.Parse(txt_data_InsGenerale.Text);
                datiPianoEsternoLavorazione.Orario = string.IsNullOrEmpty(txt_orario_InsGenerale.Text) ? null : (DateTime?)DateTime.Parse(txt_orario_InsGenerale.Text);
                datiPianoEsternoLavorazione.Diaria = chk_diaria_InsGenerale.Checked;
                datiPianoEsternoLavorazione.ImportoDiaria = importoDiaria;
                datiPianoEsternoLavorazione.IdIntervento = int.Parse(ddl_intervento_InsGenerale.SelectedValue);
                datiPianoEsternoLavorazione.Albergo = chk_albergo_InsGenerale.Checked;

                figuraProfessionale.Intervento = ddl_intervento_InsGenerale.SelectedItem.Text;
                figuraProfessionale.Diaria = importoDiaria;
                // figuraProfessionale.Data = string.IsNullOrEmpty(txt_data_InsGenerale.Text) ? null : (DateTime?)DateTime.Parse(txt_data_InsGenerale.Text);
            }

            AggiornaTotali();
            ResetPanelLavorazione();

            gvFigProfessionali.DataSource = ListaFigureProfessionali;
            gvFigProfessionali.DataBind();

            RichiediOperazionePopup("UPDATE");
        }

        protected void btnImporta_Click(object sender, EventArgs e)
        {
            ImportaFigProfInPianoEsterno();
        }

        protected void btnInserimentoGenerale_Click(object sender, EventArgs e)
        {
            ClearPanelInserimentoGenerale();

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriModificaFigProf", script: "javascript: document.getElementById('" + panelInserimentoGeneralePianoEsterno.ClientID + "').style.display='block'", addScriptTags: true);
        }

        protected void ddl_FiltroGiorniLavorazione_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_FiltroGiorniLavorazione.SelectedItem.Value == "")
            {
                gvFigProfessionali.DataSource = ListaFigureProfessionali;
                gvFigProfessionali.DataBind();
            }
            else
            {
                DateTime giornoLavorazione = SessionManager.EventoSelezionato.data_inizio_lavorazione.AddDays(int.Parse(ddl_FiltroGiorniLavorazione.SelectedItem.Value));
                List<FiguraProfessionale> listaFigProfFiltrate = ListaFigureProfessionali.Where(x => x.Data == giornoLavorazione).ToList<FiguraProfessionale>();

                gvFigProfessionali.DataSource = listaFigProfFiltrate;
                gvFigProfessionali.DataBind();
            }
            RichiediOperazionePopup("UPDATE");
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




        private void ImportaFigProfInPianoEsterno()
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
                List<FiguraProfessionale> listaFigureProfessionali_OLD = ListaFigureProfessionali; // lista FP per il confronto delle figure non presenti già nell'elenco

                CaricaListaDatiPianoEsternoLavorazione(_listaCollaboratoriFornitori);

                // se un elemento non è presente in dett. economico non lo reinserisco (caso di elementi modificati che risulterebbero duplicati)
                List<FiguraProfessionale> listaFigureProfessionali_DA_ELIMINARE = new List<FiguraProfessionale>();
                foreach (FiguraProfessionale figProf_OLD in listaFigureProfessionali_OLD)
                {
                    if (_listaCollaboratoriFornitori.Where(x => x.Data == figProf_OLD.Data && x.IdCollaboratori == figProf_OLD.IdCollaboratori && x.IdFornitori == figProf_OLD.IdFornitori).ToList().Count == 0)
                    {
                        listaFigureProfessionali_DA_ELIMINARE.Add(figProf_OLD);
                    }
                }
                foreach (FiguraProfessionale figProf_ELIM in listaFigureProfessionali_DA_ELIMINARE)
                {
                    listaFigureProfessionali_OLD.Remove(figProf_ELIM);
                }
                //fine

                // sostituisco alle Fig Prof caricate dal dettaglio economico quelle precedentemente presenti nel piano esterno
                if (listaFigureProfessionali_OLD.Count > 0)
                {
                    ListaFigureProfessionali.AddRange(listaFigureProfessionali_OLD);
                }

                if (ListaFigureProfessionali.Count > 0)
                {
                    lbl_nessunaFiguraProf.Visible = false;

                    ddl_FiltroGiorniLavorazione.Attributes.Remove("readonly");
                    ddl_FiltroGiorniLavorazione.CssClass = "w3-white w3-border w3-hover-shadow w3-round";

                    btnInserimentoGenerale.Attributes.Remove("readonly");
                    btnInserimentoGenerale.CssClass = "w3-btn w3-white w3-border w3-border-blue w3-round-large";

                    ListaFigureProfessionali = ListaFigureProfessionali.OrderBy(x => x.Data).ThenByDescending(x => x.Netto).ToList<FiguraProfessionale>();

                    gvFigProfessionali.DataSource = ListaFigureProfessionali;
                    gvFigProfessionali.DataBind();

                    RichiediOperazionePopup("UPDATE");
                }
            }
        }

        private void ClearPanelInserimentoGenerale()
        {
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

        private void CaricaListaDatiPianoEsternoLavorazione(List<DatiArticoliLavorazione> _listaCollaboratoriFornitori)
        {
            List<FiguraProfessionale> _listaFigureProfessionali = new List<FiguraProfessionale>();
            List<DatiPianoEsternoLavorazione> _listaDatiPianoEsterno = new List<DatiPianoEsternoLavorazione>();

            int numGiorniLavorazione = SessionManager.EventoSelezionato.durata_lavorazione;

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

                figProf.IdentificatoreOggetto = IDGenerator.GetId(figProf, out bool firstTime);

                figProf.Nota = collabForn.Nota;
                figProf.Netto = collabForn.FP_netto;
                figProf.Lordo = collabForn.FP_lordo;
                figProf.Data = datiPianoEsterno.Data = collabForn.Data;


                FiguraProfessionale figProfGiaPresente = ListaFigureProfessionali.FirstOrDefault(x => x.Data == figProf.Data && x.IdCollaboratori == figProf.IdCollaboratori && x.IdFornitori == figProf.IdFornitori);
                if (figProfGiaPresente == null)
                {
                    _listaFigureProfessionali.Add(figProf);
                }
                else
                {
                    if (figProfGiaPresente.Nome != figProf.Nome ||
                        figProfGiaPresente.Cognome != figProf.Cognome)
                    {
                        ListaFigureProfessionali.Remove(figProfGiaPresente);
                        _listaFigureProfessionali.Add(figProf);
                    }
                }
                _listaDatiPianoEsterno.Add(datiPianoEsterno);
            }

            ListaFigureProfessionali = _listaFigureProfessionali;
            SessionManager.EventoSelezionato.LavorazioneCorrente.ListaDatiPianoEsternoLavorazione = _listaDatiPianoEsterno;
        }

        #endregion

        #region OPERAZIONI COMUNI
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

            #region TIPI INTERVENTO
            ddl_intervento.DataSource = SessionManager.ListaTipiIntervento.OrderBy(x => x.id);
            ddl_intervento.DataTextField = "nome";
            ddl_intervento.DataValueField = "id";
            ddl_intervento.DataBind();

            ddl_intervento_InsGenerale.DataSource = SessionManager.ListaTipiIntervento.OrderBy(x => x.id);
            ddl_intervento_InsGenerale.DataTextField = "nome";
            ddl_intervento_InsGenerale.DataValueField = "id";
            ddl_intervento_InsGenerale.DataBind();
            #endregion

        }

        private void PopolaParametriConfigurazione()
        {
            Esito esito = new Esito();
            string erroriConversione = string.Empty;

            #region RITENUTA ACCONTO
            aliquota_RitenutaAcconto = Config_BLL.Instance.getConfig(ref esito, "ALIQUOTA_RITENUTA_ACCONTO").Valore;
            if (esito.Codice != Esito.ESITO_OK)
            {
                basePage.ShowError(esito.Descrizione);
                return;
            }
            Decimal.TryParse(aliquota_RitenutaAcconto, out decimal aliquotaRitAcc);
            if (aliquotaRitAcc == 0)
            {
                erroriConversione += "<li>L'aliquota per la ritenuta di acconto non è nel formato corretto</li>";
                aliquota_RitenutaAcconto = "1";
            }
            #endregion

            #region MISTA
            quotaFissa_PagamentoMisto = Config_BLL.Instance.getConfig(ref esito, "QUOTA_FISSA_PAGAMENTO_MISTO").Valore;
            if (esito.Codice != Esito.ESITO_OK)
            {
                basePage.ShowError(esito.Descrizione);
                return;
            }
            Decimal.TryParse(quotaFissa_PagamentoMisto, out decimal quotaPagFisso);
            if (quotaPagFisso == 0)
            {
                erroriConversione += "<li>La quota per la tipologia di pagamento mista non è nel formato corretto</li>";
                quotaFissa_PagamentoMisto = "0";
            }
            #endregion

            #region DIARIA LORDA
            diariaLorda = Config_BLL.Instance.getConfig(ref esito, "DIARIA_LORDA").Valore;
            if (esito.Codice != Esito.ESITO_OK)
            {
                basePage.ShowError(esito.Descrizione);
                return;
            }
            Decimal.TryParse(diariaLorda, out decimal diaria);
            if (diaria == 0)
            {
                erroriConversione += "<li>Il valore della diaria lorda non è nel formato corretto</li>";
                diariaLorda = "0";
            }
            #endregion

            if (!string.IsNullOrEmpty(erroriConversione))
            {
                basePage.ShowWarning("Si sono verificati i seguenti errori:<ul>" + erroriConversione + "</ul>Modificare il valore nella sezione Tabelle > Configurazione");
            }
        }

        private void AggiungiDiariaAListaArticoli(decimal importoDiaria, FiguraProfessionale figuraProfessionale, DateTime? data)
        {
            Esito esito = new Esito();
            Art_Articoli articoloDiaria = Articoli_BLL.Instance.getDiaria(ref esito);

            if (esito.Codice != Esito.ESITO_OK || articoloDiaria == null)
            {
                basePage.ShowWarning("La diaria non è stata aggiunta agli articoli del dettaglio economico");
            }
            else
            {
                int idDiaria = articoloDiaria.Id;
                int idLavorazione = SessionManager.EventoSelezionato.LavorazioneCorrente == null ? 0 : SessionManager.EventoSelezionato.LavorazioneCorrente.Id;
                int indiceInserimentoDiaria = 0;

                DatiArticoliLavorazione articoloLavorazioneDiaria = Articoli_BLL.Instance.CaricaArticoloLavorazioneByID(idLavorazione, idDiaria, data, ref esito);
                articoloLavorazioneDiaria.Costo = importoDiaria;
                articoloLavorazioneDiaria.FP_lordo = importoDiaria;
                articoloLavorazioneDiaria.Prezzo = articoloDiaria.DefaultPrezzo;

                if (figuraProfessionale.Tipo == COLLABORATORE)
                {
                    articoloLavorazioneDiaria.IdCollaboratori = figuraProfessionale.IdCollaboratori;
                    articoloLavorazioneDiaria.IdFornitori = null;

                    // elimino l'eventuale diaria già presente
                    DatiArticoliLavorazione diariaDaEliminare = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.FirstOrDefault(x => x.IdArtArticoli == idDiaria && x.IdCollaboratori == figuraProfessionale.IdCollaboratori && x.Data == data);
                    SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Remove(diariaDaEliminare);

                    // seleziono l'indice del collaboratore per cui inserire la diaria
                    indiceInserimentoDiaria = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.IndexOf(SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Where(x => x.IdArtArticoli != idDiaria && x.IdCollaboratori == figuraProfessionale.IdCollaboratori && x.Data == data).FirstOrDefault());
                }
                else if (figuraProfessionale.Tipo == FORNITORE)
                {
                    articoloLavorazioneDiaria.IdCollaboratori = null;
                    articoloLavorazioneDiaria.IdFornitori = figuraProfessionale.IdFornitori;

                    // elimino l'eventuale diaria già presente
                    DatiArticoliLavorazione diariaDaEliminare = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.FirstOrDefault(x => x.IdArtArticoli == idDiaria && x.IdFornitori == figuraProfessionale.IdFornitori && x.Data == data);
                    SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Remove(diariaDaEliminare);

                    // seleziono l'indice del fornitore per cui inserire la diaria
                    indiceInserimentoDiaria = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.IndexOf(SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Where(x => x.IdArtArticoli != idDiaria && x.IdFornitori == figuraProfessionale.IdFornitori && x.Data == data).FirstOrDefault());
                }

                if (indiceInserimentoDiaria == -1)
                {
                    indiceInserimentoDiaria = 0;
                }

                SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Insert(indiceInserimentoDiaria + 1, articoloLavorazioneDiaria);
                //SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.OrderByDescending(x => x.Prezzo).ToList();

                lbl_selezionareArticolo.Visible = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione == null || SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Count == 0;
            }
        }

        private void CancellaDiariaDaListaArticoli(FiguraProfessionale figuraProfessionale)
        {
            Esito esito = new Esito();
            Art_Articoli articoloDiaria = Articoli_BLL.Instance.getDiaria(ref esito);

            if (esito.Codice != Esito.ESITO_OK || articoloDiaria == null)
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

            try
            {
                // FILTRO GIORNI LAVORAZIONE
                PopolaComboFiltroGiorniLavorazione();

                int idDatiAgenda = SessionManager.EventoSelezionato.id;
                int idCliente = SessionManager.EventoSelezionato.id_cliente;

                SessionManager.EventoSelezionato.LavorazioneCorrente = Dati_Lavorazione_BLL.Instance.getDatiLavorazioneByIdEvento(idDatiAgenda, ref esito);
                if (esito.Codice != Esito.ESITO_OK)
                {
                    basePage.ShowError(esito.Descrizione);
                }
                else if (SessionManager.EventoSelezionato.LavorazioneCorrente == null)
                {
                    basePage.ShowWarning("La lavorazione corrente non contiene dati.<br/> Verrà creata una nuova lavorazione a partire dai dati dell'offerta.");
                    TrasformaInLavorazione();
                }
                else
                {
                    int idContratto = SessionManager.ListaTipiProtocolli.FirstOrDefault(x => x.nome.ToLower() == "contratto").id;
                    List<Protocolli> listaContratti = Protocolli_BLL.Instance.GetProtocolliByIdCliente(ref esito, idCliente).Where(x => x.Id_tipo_protocollo == idContratto).ToList();

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

                    if (esito.Codice != Esito.ESITO_OK)
                    {
                        basePage.ShowError(esito.Descrizione);
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

                            //SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione;


                            //CaricaListaDatiDettaglioEconomico();


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
            catch (Exception ex)
            {
                if (esito.Codice == Esito.ESITO_OK)
                {
                    esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                    esito.Descrizione = ex.Message + Environment.NewLine + ex.StackTrace;
                }
                basePage.ShowError(ex.Message);
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

            // REFERENTE
            ddl_Referente.Items.Clear();
            foreach (Anag_Referente_Clienti_Fornitori referente in SessionManager.ListaReferenti)
            {
                ddl_Referente.Items.Add(new ListItem(referente.Nome + " " + referente.Cognome, referente.Id.ToString()));
            }
            ddl_Referente.Items.Insert(0, new ListItem("<seleziona>", ""));

            // FILTRO DATA LAVORAZIONE
            PopolaComboFiltroGiorniLavorazione();

            if (SessionManager.EventoSelezionato.LavorazioneCorrente == null)
            {
                SessionManager.EventoSelezionato.LavorazioneCorrente = new DatiLavorazione
                {
                    ListaArticoliLavorazione = new List<DatiArticoliLavorazione>(),
                    ListaDatiPianoEsternoLavorazione = new List<DatiPianoEsternoLavorazione>()
                };
            }

            SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione = listaArticoliLavorazione;

            lbl_selezionareArticolo.Visible = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione == null || SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Count == 0;

            if (SessionManager.EventoSelezionato.LavorazioneCorrente.ListaFigureProfessionali == null || SessionManager.EventoSelezionato.LavorazioneCorrente.ListaFigureProfessionali.Count == 0)
            {
                lbl_nessunaFiguraProf.Visible = true;

                ddl_FiltroGiorniLavorazione.Attributes.Add("readonly", "readonly");
                ddl_FiltroGiorniLavorazione.CssClass = "w3-white w3-border w3-hover-shadow w3-round w3-disabled";

                btnInserimentoGenerale.Attributes.Add("readonly", "readonly");
                btnInserimentoGenerale.CssClass = "w3-btn w3-white w3-border w3-border-blue w3-round-large w3-disabled";
            }
            else
            {
                lbl_nessunaFiguraProf.Visible = false;

                ddl_FiltroGiorniLavorazione.Attributes.Remove("readonly");
                ddl_FiltroGiorniLavorazione.CssClass = "w3-white w3-border w3-hover-shadow w3-round";

                btnInserimentoGenerale.Attributes.Remove("readonly");
                btnInserimentoGenerale.CssClass = "w3-btn w3-white w3-border w3-border-blue w3-round-large";
            }


            gvArticoliLavorazione.DataSource = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione;
            gvArticoliLavorazione.DataBind();
        }

        public DatiLavorazione CreaDatiLavorazione()
        {
            DatiLavorazione datiLavorazione = new DatiLavorazione
            {
                Id = SessionManager.EventoSelezionato.LavorazioneCorrente == null ? 0 : SessionManager.EventoSelezionato.LavorazioneCorrente.Id,

                Ordine = txt_Ordine.Text,
                Fattura = txt_Fattura.Text,
                IdContratto = string.IsNullOrEmpty(ddl_Contratto.SelectedValue) ? null : (int?)int.Parse(ddl_Contratto.SelectedValue),
                IdReferente = string.IsNullOrEmpty(ddl_Referente.SelectedValue) ? null : (int?)int.Parse(ddl_Referente.SelectedValue),
                IdCapoTecnico = string.IsNullOrEmpty(ddl_Capotecnico.SelectedValue) ? null : (int?)int.Parse(ddl_Capotecnico.SelectedValue),
                IdProduttore = string.IsNullOrEmpty(ddl_Produttore.SelectedValue) ? null : (int?)int.Parse(ddl_Produttore.SelectedValue),
                ListaArticoliLavorazione = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione,
                ListaDatiPianoEsternoLavorazione = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaDatiPianoEsternoLavorazione,
                NotePianoEsterno = txt_notaGeneralePianoEsterno.Text
            };
            return datiLavorazione;
        }

        public void TrasformaInLavorazione()
        {
            int numGiorniLavorazione = SessionManager.EventoSelezionato.durata_lavorazione;

            SessionManager.EventoSelezionato.id_stato = Stato.Instance.STATO_LAVORAZIONE;

            // COSTRUISCO LISTA DATI ARTICOLI LAVORAZIONE
            List<DatiArticoliLavorazione> listaDatiArticoliLavorazione = new List<DatiArticoliLavorazione>();

            foreach (DatiArticoli datoArticolo in SessionManager.EventoSelezionato.ListaDatiArticoli)
            {

                for (int giornoLav = 0; giornoLav < numGiorniLavorazione; giornoLav++)
                {
                    DateTime dataGiornoLav = SessionManager.EventoSelezionato.data_inizio_lavorazione.AddDays(giornoLav);

                    for (int i = 0; i < datoArticolo.Quantita; i++)
                    {
                        DatiArticoliLavorazione datoArticoloLavorazione = new DatiArticoliLavorazione
                        {
                            Id = 0,
                            IdDatiLavorazione = 0,
                            IdArtArticoli = datoArticolo.IdArtArticoli,
                            IdTipoGenere = datoArticolo.IdTipoGenere,
                            IdTipoGruppo = datoArticolo.IdTipoGruppo,
                            IdTipoSottogruppo = datoArticolo.IdTipoSottogruppo,
                            IdCollaboratori = null,
                            IdFornitori = null,
                            IdTipoPagamento = null,
                            Descrizione = datoArticolo.Descrizione,
                            DescrizioneLunga = datoArticolo.DescrizioneLunga,
                            Stampa = datoArticolo.Stampa,
                            Prezzo = datoArticolo.Prezzo,
                            Costo = datoArticolo.Costo,
                            Iva = datoArticolo.Iva,
                            Data = dataGiornoLav// SessionManager.EventoSelezionato.data_inizio_lavorazione
                        };

                        datoArticoloLavorazione.IdentificatoreOggetto = IDGenerator.GetId(datoArticoloLavorazione, out bool firstTime);
                        listaDatiArticoliLavorazione.Add(datoArticoloLavorazione);
                    }

                }
            }
            listaDatiArticoliLavorazione = listaDatiArticoliLavorazione.OrderBy(x => x.Data).ThenByDescending(y => y.Costo).ToList<DatiArticoliLavorazione>();
            CreaNuovaLavorazione(listaDatiArticoliLavorazione);
        }

        public void ClearLavorazione()
        {
            lbl_selezionareArticolo.Visible = true;
            lbl_nessunaFiguraProf.Visible = true;

            ddl_FiltroGiorniLavorazione.Items.Clear();
            ddl_FiltroGiorniLavorazione.Attributes.Add("readonly", "readonly");
            ddl_FiltroGiorniLavorazione.CssClass = "w3-white w3-border w3-hover-shadow w3-round w3-disabled";

            btnInserimentoGenerale.Attributes.Add("readonly", "readonly");
            btnInserimentoGenerale.CssClass = "w3-btn w3-white w3-border w3-border-blue w3-round-large w3-disabled";

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
                    totLordo += art.FP_lordo != null ? (decimal)art.FP_lordo : 0;
                    totPrezzo += art.Prezzo;
                    totIva += (art.Prezzo * art.Iva / 100);
                }

                if (totPrezzo != 0)
                {
                    percRicavo = ((totPrezzo - (decimal)totLordo) / totPrezzo) * 100;
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
            gvArticoliLavorazione.DataSource = SessionManager.EventoSelezionato.LavorazioneCorrente?.ListaArticoliLavorazione;
            gvArticoliLavorazione.DataBind();

            lbl_selezionareArticolo.Visible = (SessionManager.EventoSelezionato.LavorazioneCorrente == null ||
                                               SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione == null ||
                                               SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Count == 0);

            if (SessionManager.EventoSelezionato.LavorazioneCorrente != null &&
                ListaFigureProfessionali != null &&
                ListaFigureProfessionali.Count > 0)
            {
                lbl_nessunaFiguraProf.Visible = false;

                ddl_FiltroGiorniLavorazione.Attributes.Remove("readonly");
                ddl_FiltroGiorniLavorazione.CssClass = "w3-white w3-border w3-hover-shadow w3-round";

                btnInserimentoGenerale.Attributes.Remove("readonly");
                btnInserimentoGenerale.CssClass = "w3-btn w3-white w3-border w3-border-blue w3-round-large";
            }
            else
            {
                lbl_nessunaFiguraProf.Visible = true;

                ddl_FiltroGiorniLavorazione.Attributes.Add("readonly", "readonly");
                ddl_FiltroGiorniLavorazione.CssClass = "w3-white w3-border w3-hover-shadow w3-round w3-disabled";

                btnInserimentoGenerale.Attributes.Add("readonly", "readonly");
                btnInserimentoGenerale.CssClass = "w3-btn w3-white w3-border w3-border-blue w3-round-large w3-disabled";
            }

            AggiornaTotali();
        }

        private void PopolaComboFiltroGiorniLavorazione()
        {
            int numGiorniLavorazione = SessionManager.EventoSelezionato.durata_lavorazione;

            ddl_filtroGiorniLavorazioneDettEcon.Items.Clear();
            ddl_FiltroGiorniLavorazione.Items.Clear();

            ddl_filtroGiorniLavorazioneDettEcon.Items.Add(new ListItem("<tutte le date>", ""));
            ddl_FiltroGiorniLavorazione.Items.Add(new ListItem("<tutte le date>", ""));
            for (int giornoLav = 0; giornoLav < numGiorniLavorazione; giornoLav++)
            {
                DateTime dataGiornoLav = SessionManager.EventoSelezionato.data_inizio_lavorazione.AddDays(giornoLav);
                ddl_filtroGiorniLavorazioneDettEcon.Items.Add(new ListItem(dataGiornoLav.ToString("dd/MM/yyyy"), giornoLav.ToString()));
                ddl_FiltroGiorniLavorazione.Items.Add(new ListItem(dataGiornoLav.ToString("dd/MM/yyyy"), giornoLav.ToString()));
            }
            ddl_filtroGiorniLavorazioneDettEcon.SelectedIndex = 0;
            ddl_FiltroGiorniLavorazione.SelectedIndex = 0;
        }
        #endregion

    }
}