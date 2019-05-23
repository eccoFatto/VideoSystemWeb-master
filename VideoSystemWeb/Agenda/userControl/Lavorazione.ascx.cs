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
        
        List<ArticoliGruppi> ListaArticoliGruppiLavorazione
        {
            get
            {
                if (ViewState["listaArticoliGruppiLavorazione"] == null || ((List<ArticoliGruppi>)ViewState["listaArticoliGruppiLavorazione"]).Count == 0)
                {
                    ViewState["listaArticoliGruppiLavorazione"] = Articoli_BLL.Instance.CaricaListaArticoliGruppi();
                }
                return (List<ArticoliGruppi>)ViewState["listaArticoliGruppiLavorazione"];
            }
            set
            {
                ViewState["listaArticoliGruppiLavorazione"] = value;
            }
        }
        List<FiguraProfessionale> ListaFigureProfessionali
        {
            get
            {
                if (ViewState["listaFigureProfessionali"] == null || ((List<FiguraProfessionale>)ViewState["listaFigureProfessionali"]).Count == 0)
                {
                    ViewState["listaFigureProfessionali"] = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaFigureProfessionali;
                }
                return (List<FiguraProfessionale>)ViewState["listaFigureProfessionali"];
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

                ViewState["listaFigureProfessionali"] = _listaFigureProfessionali;
            }
        }
        List<Tipologica> ListaTipoIntervento
        {
            get
            {
                if (ViewState["listaTipoIntervento"] == null || ((List<Tipologica>)ViewState["listaTipoIntervento"]).Count() == 0)
                {
                    ViewState["listaTipoIntervento"] = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_INTERVENTO);
                }
                return (List<Tipologica>)ViewState["listaTipoIntervento"];
            }
            set
            {
                ViewState["listaTipoIntervento"] = value;
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
                aggiungiArticoliDelGruppoAListaArticoli(articoloGruppo.IdOggetto);
            }
            else
            {
                aggiungiArticoloAListaArticoli(articoloGruppo.IdOggetto);
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
                ViewState["identificatoreArticoloLavorazione"] = identificatoreOggetto;
                articoloSelezionato = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.FirstOrDefault(x => x.IdentificatoreOggetto == identificatoreOggetto);
            }
            else
            {
                ViewState["idArticoloLavorazione"] = id;
                articoloSelezionato = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.FirstOrDefault(x => x.Id == id);
            }

            int indexArticolo = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.IndexOf(articoloSelezionato);

            switch (e.CommandName)
            {
                case "modifica":
                    txt_Descrizione.Text = articoloSelezionato.Descrizione;
                    txt_DescrizioneLunga.Text = articoloSelezionato.DescrizioneLunga;
                    txt_Costo.Text = articoloSelezionato.Costo.ToString();
                    txt_Prezzo.Text = articoloSelezionato.Prezzo.ToString();
                    txt_Iva.Text = articoloSelezionato.Iva.ToString();
                    ddl_FPtipoPagamento.SelectedValue = articoloSelezionato.IdTipoPagamento != null ? articoloSelezionato.IdTipoPagamento.ToString() : "";

                    //Cerco tra Collaboratori o Fornitori
                    FiguraProfessionale figuraProfessionale = SessionManager.ListaCompletaFigProf.FirstOrDefault(x => x.Id == articoloSelezionato.IdCollaboratori);
                    if (figuraProfessionale == null)
                    {
                        figuraProfessionale = SessionManager.ListaCompletaFigProf.FirstOrDefault(x => x.Id == articoloSelezionato.IdFornitori);
                    }

                    AbilitaComponentiCosto(articoloSelezionato);
                    AbilitaComponentiFiguraProfessionale(figuraProfessionale, true);

                    if (figuraProfessionale != null)
                    {
                        PopolaNominativi(SessionManager.ListaCompletaFigProf.Where(x => x.Tipo == figuraProfessionale.Tipo).ToList());

                        ddl_FPtipo.SelectedValue = figuraProfessionale.Tipo.ToString();
                        txt_FPnotaCollaboratore.Text = articoloSelezionato.Nota;
                        ddl_FPnominativo.SelectedValue = figuraProfessionale.Id.ToString();
                        txt_FPtelefono.Text = figuraProfessionale.Telefono;
                    }
                    else
                    {
                        AbilitaComponentiFiguraProfessionale(null, false);

                        SessionManager.ListaCittaCollaboratori.Sort();
                        ddl_FPcitta.Items.Clear();
                        foreach (string citta in SessionManager.ListaCittaCollaboratori)
                        {
                            ddl_FPcitta.Items.Add(new ListItem(citta.ToUpper(), citta));
                        }
                        ddl_FPcitta.Items.Insert(0, new ListItem("<seleziona>", ""));

                        PopolaNominativi(SessionManager.ListaCompletaFigProf.Where(x => x.Tipo == COLLABORATORE).ToList());
                        ddl_FPnominativo.SelectedValue = "";

                        ddl_FPtipo.SelectedValue = COLLABORATORE.ToString();
                        
                        txt_FPtelefono.Text = "";
                    }

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriModificaArticolo", script: "javascript: document.getElementById('" + panelModificaArticolo.ClientID + "').style.display='block'", addScriptTags: true);

                    break;
                case "elimina":
                    SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Remove(articoloSelezionato);
                    gvArticoliLavorazione.DataSource = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione;
                    gvArticoliLavorazione.DataBind();

                    AggiornaTotali();
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

        protected void gvFigProfessionali_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            int id = Convert.ToInt32(commandArgs[0]);

            FiguraProfessionale figuraProfessionaleSelezionata;
            if (id == 0)
            {
                long identificatoreOggetto = Convert.ToInt64(commandArgs[1]);
                ViewState["identificatoreFiguraProfessionale"] = identificatoreOggetto;
                figuraProfessionaleSelezionata = ListaFigureProfessionali.FirstOrDefault(x => x.IdentificatoreOggetto == identificatoreOggetto);
            }
            else
            {
                ViewState["idFiguraProfessionale"] = id;
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

                    txt_data.Text = datiPianoEsterno.Data == null ? "" : ((DateTime)datiPianoEsterno.Data).ToString("dd/MM/yyyy");
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

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriModificaFigProf", script: "javascript: document.getElementById('" + panelModificaPianoEsterno.ClientID + "').style.display='block'", addScriptTags: true);

                    break;
                case "elimina":
                    ListaFigureProfessionali.Remove(figuraProfessionaleSelezionata);
                    gvFigProfessionali.DataSource = ListaFigureProfessionali;
                    gvFigProfessionali.DataBind();

                    ResetPanelLavorazione();

                    break;
                case "moveUp":
                    if (indexArticolo > 0)
                    {
                        ListaFigureProfessionali.Remove(figuraProfessionaleSelezionata);
                        ListaFigureProfessionali.Insert(indexArticolo - 1, figuraProfessionaleSelezionata);
                        gvFigProfessionali.DataSource = ListaFigureProfessionali;
                        gvFigProfessionali.DataBind();
                    }
                    break;
                case "moveDown":
                    if (indexArticolo < SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Count - 1)
                    {
                        ListaFigureProfessionali.Remove(figuraProfessionaleSelezionata);
                        ListaFigureProfessionali.Insert(indexArticolo + 1, figuraProfessionaleSelezionata);
                        gvFigProfessionali.DataSource = ListaFigureProfessionali;
                        gvFigProfessionali.DataBind();
                    }
                    break;
            }

            RichiediOperazionePopup("UPDATE");
        }

        protected void btnOKModificaArtLavorazione_Click(object sender, EventArgs e)
        {
            DatiArticoliLavorazione articoloSelezionato;

            if (ViewState["idArticoloLavorazione"] == null)
            {
                long identificatoreOggetto = (long)ViewState["identificatoreArticoloLavorazione"];
                articoloSelezionato = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.FirstOrDefault(x => x.IdentificatoreOggetto == identificatoreOggetto);
            }
            else
            {
                int idArticolo = (int)ViewState["idArticoloLavorazione"];
                articoloSelezionato = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.FirstOrDefault(x => x.Id == idArticolo);
            }

            var index = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.IndexOf(articoloSelezionato);

            articoloSelezionato.Descrizione = txt_Descrizione.Text;
            articoloSelezionato.DescrizioneLunga = txt_DescrizioneLunga.Text;

            articoloSelezionato.UsaCostoFP = chk_ModCosto.Checked;

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
            articoloSelezionato.Data = DateTime.Now;
            articoloSelezionato.Tv = 0;

            if (ddl_FPtipo.SelectedValue == COLLABORATORE.ToString()) 
            {
                articoloSelezionato.IdCollaboratori = ddl_FPnominativo.SelectedValue == "" ? null : (int?)int.Parse(ddl_FPnominativo.SelectedValue);
            }
            else if (ddl_FPtipo.SelectedValue == FORNITORE.ToString())
            {
                articoloSelezionato.IdFornitori = ddl_FPnominativo.SelectedValue == "" ? null : (int?)int.Parse(ddl_FPnominativo.SelectedValue);
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
            if (ViewState["idFiguraProfessionale"] == null)
            {
                long identificatoreOggetto = (long)ViewState["identificatoreFiguraProfessionale"];
                figuraProfessionale = ListaFigureProfessionali.FirstOrDefault(x => x.IdentificatoreOggetto == identificatoreOggetto);

            }
            else
            {
                int idFiguraProfessionale = (int)ViewState["idFiguraProfessionale"];
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

        protected void gvArticoliLavorazione_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DatiArticoliLavorazione rigaCorrente = (DatiArticoliLavorazione)e.Row.DataItem;

                if (rigaCorrente.IdCollaboratori != null && rigaCorrente.IdCollaboratori.HasValue)
                {
                    FiguraProfessionale figuraProfessionale = SessionManager.ListaCompletaFigProf.FirstOrDefault(x => x.Id == rigaCorrente.IdCollaboratori);
                    ((Label)e.Row.FindControl("lbl_Riferimento")).Text = figuraProfessionale.Nome + " " + figuraProfessionale.Cognome;
                }

                if (rigaCorrente.IdFornitori != null && rigaCorrente.IdFornitori.HasValue)
                {
                    FiguraProfessionale figuraProfessionale = SessionManager.ListaCompletaFigProf.FirstOrDefault(x => x.Id == rigaCorrente.IdFornitori);
                    ((Label)e.Row.FindControl("lbl_Riferimento")).Text = figuraProfessionale.Nome + " " + figuraProfessionale.Cognome;
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

                    _listaFigureProfessionali.Add(figProf);
                    _listaDatiPianoEsterno.Add(datiPianoEsterno);
                }

                ListaFigureProfessionali = _listaFigureProfessionali;
                SessionManager.EventoSelezionato.LavorazioneCorrente.ListaDatiPianoEsternoLavorazione = _listaDatiPianoEsterno;

                if (ListaFigureProfessionali.Count > 0)
                {
                    lbl_nessunaFiguraProf.Visible = false;

                    gvFigProfessionali.DataSource = ListaFigureProfessionali;
                    gvFigProfessionali.DataBind();
                    
                    RichiediOperazionePopup("UPDATE");
                }
            }
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

        protected void filtraFP(object sender, EventArgs e)
        {
            DropDownList ddlSelezionata = (DropDownList)sender;

            Esito esito = new Esito();

            int tipoFP = int.Parse(ddl_FPtipo.SelectedValue);
            string qualificaFP = ddl_FPqualifica.SelectedItem.Text;
            string cittaFP = ddl_FPcitta.SelectedItem.Text.ToLower().Trim();

            FiguraProfessionale figuraProfessionaleDummy = new FiguraProfessionale() { Tipo = tipoFP };

            AbilitaComponentiFiguraProfessionale(figuraProfessionaleDummy, ddlSelezionata.ID == "ddl_FPtipo");

            List <FiguraProfessionale> listaFPfiltrata = SessionManager.ListaCompletaFigProf.Where(x => x.Tipo == tipoFP).ToList();

            if (tipoFP == 0 && !string.IsNullOrEmpty(qualificaFP) && qualificaFP != "<seleziona>")
            {
                listaFPfiltrata = listaFPfiltrata.Where(x => x.Qualifiche.Where(y => y.Qualifica == qualificaFP).Count() > 0).ToList();
            }

            if (cittaFP != "<seleziona>")
            {
                listaFPfiltrata = listaFPfiltrata.Where(x => x.Citta.ToLower().Trim().Contains(cittaFP)).ToList();
            }

            PopolaNominativi(listaFPfiltrata);

            upModificaArticolo.Update();
        }

        protected void visualizzaFP(object sender, EventArgs e)
        {
            FiguraProfessionale fpSelezionata = new FiguraProfessionale();
            if (ddl_FPnominativo.SelectedValue != "")
            {
                fpSelezionata = SessionManager.ListaCompletaFigProf.FirstOrDefault(x => x.Id == int.Parse(ddl_FPnominativo.SelectedValue));

                AbilitaComponentiFiguraProfessionale(fpSelezionata, true);
            }
            else
            {
                AbilitaComponentiFiguraProfessionale(null, true);
            }

            PopolaDettagliFP(fpSelezionata);
            upModificaArticolo.Update();
        }

        private void PopolaNominativi(List<FiguraProfessionale> listaNominativi)
        {
            ddl_FPnominativo.Items.Clear();
            foreach (FiguraProfessionale figPro in listaNominativi)
            {
                ddl_FPnominativo.Items.Add(new ListItem(figPro.Cognome + " " + figPro.Nome, figPro.Id.ToString()));
            }
            ddl_FPnominativo.Items.Insert(0, new ListItem("<seleziona>", ""));
        }

        private void PopolaDettagliFP(FiguraProfessionale figuraProfessionale)
        {
            txt_FPtelefono.Text = figuraProfessionale.Telefono;
            txt_FPnotaCollaboratore.Text = figuraProfessionale.Nota;
        }

        private void aggiungiArticoliDelGruppoAListaArticoli(int idGruppo)
        {
            Esito esito = new Esito();
            int idLavorazione = SessionManager.EventoSelezionato.LavorazioneCorrente == null ? 0 : SessionManager.EventoSelezionato.LavorazioneCorrente.Id;

            SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.AddRange(Articoli_BLL.Instance.CaricaListaArticoliLavorazioneByIDGruppo(idLavorazione, idGruppo, ref esito));
            SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.OrderByDescending(x => x.Prezzo).ToList();

            lbl_selezionareArticolo.Visible = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione == null || SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Count == 0;
        }

        private void aggiungiArticoloAListaArticoli(int idArticolo)
        {
            Esito esito = new Esito();
            int idLavorazione = SessionManager.EventoSelezionato.LavorazioneCorrente == null ? 0 : SessionManager.EventoSelezionato.LavorazioneCorrente.Id;

            DatiArticoliLavorazione articoloLavorazione = Articoli_BLL.Instance.CaricaArticoloLavorazioneByID(idLavorazione, idArticolo, ref esito);

            SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Add(articoloLavorazione);
            SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.OrderByDescending(x => x.Prezzo).ToList();

            lbl_selezionareArticolo.Visible = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione == null || SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Count == 0;
        }

        private void AggiornaTotali()
        {
            decimal totPrezzo = 0;
            decimal totCosto = 0;
            decimal totIva = 0;
            decimal percRicavo = 0;

            if (SessionManager.EventoSelezionato.LavorazioneCorrente != null && SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione != null && SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Count > 0)
            {
                foreach (DatiArticoliLavorazione art in SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione)
                {
                    if (art.UsaCostoFP != null )
                    {
                        if ((bool)art.UsaCostoFP)
                        {
                            totCosto += art.FP_netto != null ? (decimal)art.FP_netto : 0;
                        }
                        else
                        {
                            totCosto +=  (decimal)art.Costo ;
                        }
                    }
                    else
                    {
                        totCosto += (decimal)art.Costo;
                    }

                    totPrezzo += art.Prezzo;
                    totIva += (art.Prezzo * art.Iva / 100);
                }

                if (totPrezzo != 0)
                {
                    percRicavo = ((totPrezzo - totCosto) / totPrezzo) * 100;
                }
            }

            txt_TotPrezzo.Text = string.Format("{0:N2}", totPrezzo);
            txt_TotCosto.Text = string.Format("{0:N2}", totCosto);
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

            AggiornaTotali();
        }

        public void PopolaLavorazione()
        {
            Esito esito = new Esito();

            int idDatiAgenda = SessionManager.EventoSelezionato.id;
            int idCliente = SessionManager.EventoSelezionato.id_cliente;

            #region INIZIALIZZAZIONE OGGETTI LAVORAZIONE CORRENTE
            SessionManager.EventoSelezionato.LavorazioneCorrente = Dati_Lavorazione_BLL.Instance.getDatiLavorazioneByIdEvento(idDatiAgenda, ref esito);
            SessionManager.ListaReferenti = Anag_Referente_Clienti_Fornitori_BLL.Instance.getReferentiByIdAzienda(ref esito, idCliente);
            ListaFigureProfessionali = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaFigureProfessionali; //in questo modo assegno identificatoreOggetto

            ddl_intervento.DataSource = ListaTipoIntervento.OrderBy(x=>x.id);
            ddl_intervento.DataTextField = "nome";
            ddl_intervento.DataValueField = "id";
            ddl_intervento.DataBind();
            #endregion

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
                    ddl_Contratto.SelectedValue = SessionManager.EventoSelezionato.LavorazioneCorrente.IdContratto == null ? "": SessionManager.EventoSelezionato.LavorazioneCorrente.IdContratto.ToString();
                    ddl_Referente.SelectedValue = SessionManager.EventoSelezionato.LavorazioneCorrente.IdReferente == null ? "" : SessionManager.EventoSelezionato.LavorazioneCorrente.IdReferente.ToString();
                    ddl_Capotecnico.SelectedValue = SessionManager.EventoSelezionato.LavorazioneCorrente.IdCapoTecnico == null ? "" : SessionManager.EventoSelezionato.LavorazioneCorrente.IdCapoTecnico.ToString();
                    ddl_Produttore.SelectedValue = SessionManager.EventoSelezionato.LavorazioneCorrente.IdProduttore == null ? "" : SessionManager.EventoSelezionato.LavorazioneCorrente.IdProduttore.ToString();

                    SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione;

                    gvArticoliLavorazione.DataSource = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione;
                    gvArticoliLavorazione.DataBind();

                    gvFigProfessionali.DataSource = ListaFigureProfessionali;
                    gvFigProfessionali.DataBind();
                }
                lbl_selezionareArticolo.Visible = (SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione == null || SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Count == 0);
                AggiornaTotali();
            }
        }

        public void CreaNuovaLavorazione(List<DatiArticoliLavorazione> listaArticoliLavorazione)
        {
            Esito esito = new Esito();

            val_Cliente.Text = SessionManager.ListaClientiFornitori.FirstOrDefault(x => x.Id == SessionManager.EventoSelezionato.id_cliente).RagioneSociale;
            val_Produzione.Text = SessionManager.EventoSelezionato.produzione;
            val_Lavorazione.Text = SessionManager.EventoSelezionato.lavorazione;
            val_Tipologia.Text = SessionManager.ListaTipiTipologie.FirstOrDefault(X => X.id == SessionManager.EventoSelezionato.id_tipologia).nome;
            val_DataInizio.Text = SessionManager.EventoSelezionato.data_inizio_lavorazione.ToString("dd/MM/yyyy");
            val_DataFine.Text = SessionManager.EventoSelezionato.data_fine_lavorazione.ToString("dd/MM/yyyy");

            txt_Ordine.Text = string.Empty;
            txt_Fattura.Text = string.Empty;
            //ddl_Contratto.SelectedValue = 0;  //DA ABILITARE QUANDO SARA' POPOLATO
            ddl_Referente.SelectedIndex = 0;
            ddl_Capotecnico.SelectedIndex = 0;
            ddl_Produttore.SelectedIndex = 0;



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
            txt_TotIva.Text = "";
            txt_PercRicavo.Text = "";

            ResetPanelLavorazione();
        }

        public DatiLavorazione CreaDatiLavorazione()
        {
            DatiLavorazione datiLavorazione = new DatiLavorazione();
            datiLavorazione.Id = SessionManager.EventoSelezionato.LavorazioneCorrente == null ? 0 : SessionManager.EventoSelezionato.LavorazioneCorrente.Id;

            datiLavorazione.Ordine = txt_Ordine.Text;
            datiLavorazione.Fattura = txt_Fattura.Text;
            datiLavorazione.IdContratto = string.IsNullOrEmpty(ddl_Contratto.SelectedValue) ? null : (int?)int.Parse(ddl_Contratto.SelectedValue);
            datiLavorazione.IdReferente = string.IsNullOrEmpty(ddl_Referente.SelectedValue) ? null : (int?)int.Parse(ddl_Referente.SelectedValue); //int.Parse(ddl_Referente.SelectedValue);
            datiLavorazione.IdCapoTecnico = string.IsNullOrEmpty(ddl_Capotecnico.SelectedValue) ? null : (int?)int.Parse(ddl_Capotecnico.SelectedValue); //int.Parse(ddl_Capotecnico.SelectedValue);
            datiLavorazione.IdProduttore = string.IsNullOrEmpty(ddl_Produttore.SelectedValue) ? null : (int?)int.Parse(ddl_Produttore.SelectedValue); //int.Parse(ddl_Produttore.SelectedValue);
            datiLavorazione.ListaArticoliLavorazione = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione;
            datiLavorazione.ListaDatiPianoEsternoLavorazione = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaDatiPianoEsternoLavorazione;
            return datiLavorazione;
        }

        private void AbilitaComponentiCosto(DatiArticoliLavorazione articoloSelezionato)
        {
            // GESTIONE COSTO FIG PROF
            if (articoloSelezionato.UsaCostoFP != null && (bool)articoloSelezionato.UsaCostoFP)
            {
                chk_ModCosto.Checked = true;

                txt_Costo.Attributes.Add("readonly", "readonly");
                txt_Costo.CssClass = "w3-input w3-border w3-disabled";

                txt_FPnetto.Text = articoloSelezionato.FP_netto.ToString();
                txt_FPnetto.Attributes.Remove("readonly");
                txt_FPnetto.CssClass = "w3-input w3-border";

                txt_FPlordo.Text = articoloSelezionato.FP_lordo.ToString();
            }
            else
            {
                chk_ModCosto.Checked = false;

                txt_Costo.Attributes.Remove("readonly");
                txt_Costo.CssClass = "w3-input w3-border";

                txt_FPnetto.Attributes.Add("readonly", "readonly");
                txt_FPnetto.CssClass = "w3-input w3-border w3-disabled";
                txt_FPnetto.Text = "";

                txt_FPlordo.Text = "";
            }
        }

        private void AbilitaComponentiFiguraProfessionale(FiguraProfessionale figuraProfessionale, bool ricaricaCitta)
        {
            

            if (figuraProfessionale==null || figuraProfessionale.Id == 0)
            {
                ddl_FPtipoPagamento.Attributes.Add("readonly", "readonly");
                ddl_FPtipoPagamento.CssClass = "w3-input w3-border w3-disabled";

                chk_ModCosto.Attributes.Add("readonly", "readonly");
                chk_ModCosto.CssClass = "w3-input w3-border w3-disabled";
                chk_ModCosto.Checked = false;

                txt_Costo.Attributes.Remove("readonly");
                txt_Costo.CssClass = "w3-input w3-border";

                txt_FPnetto.Attributes.Add("readonly", "readonly");
                txt_FPnetto.CssClass = "w3-input w3-border w3-disabled";

                txt_FPnotaCollaboratore.Attributes.Add("readonly", "readonly");
                txt_FPnotaCollaboratore.CssClass = "w3-input w3-border w3-disabled";
                txt_FPnotaCollaboratore.Text = string.Empty;
            }
            else
            {
                ddl_FPtipoPagamento.Attributes.Remove("readonly");
                ddl_FPtipoPagamento.CssClass = "w3-input w3-border";

                chk_ModCosto.Attributes.Remove("readonly");
                chk_ModCosto.CssClass = "w3-input w3-border";

                txt_FPnetto.Attributes.Remove("readonly");
                txt_FPnetto.CssClass = "w3-input w3-border";

                txt_FPnotaCollaboratore.Attributes.Remove("readonly");
                txt_FPnotaCollaboratore.CssClass = "w3-input w3-border";
            }

            if (figuraProfessionale == null || figuraProfessionale.Tipo == COLLABORATORE)
            {
                ddl_FPqualifica.CssClass = "w3-input w3-border";
                ddl_FPqualifica.Style.Remove("cursor");
                ddl_FPqualifica.Enabled = true;

                if (figuraProfessionale != null)
                {
                    ddl_FPtipoPagamento.CssClass = "w3-input w3-border";
                    ddl_FPtipoPagamento.Style.Remove("cursor");
                    ddl_FPtipoPagamento.Enabled = true;
                }

                if (ricaricaCitta)
                {
                    ddl_FPcitta.Items.Clear();
                    SessionManager.ListaCittaCollaboratori.Sort();
                    foreach (string citta in SessionManager.ListaCittaCollaboratori)
                    {
                        ddl_FPcitta.Items.Add(new ListItem(citta.ToUpper(), citta));
                    }
                    ddl_FPcitta.Items.Insert(0, new ListItem("<seleziona>", ""));
                }
            }
            else if (figuraProfessionale.Tipo == FORNITORE)
            {
                ddl_FPqualifica.SelectedValue = "";
                ddl_FPqualifica.CssClass = "w3-input w3-border";
                ddl_FPqualifica.Style.Add("cursor", "not-allowed;");
                ddl_FPqualifica.Enabled = false;

                ddl_FPtipoPagamento.SelectedValue = "";
                ddl_FPtipoPagamento.CssClass = "w3-input w3-border";
                ddl_FPtipoPagamento.Style.Add("cursor", "not-allowed;");
                ddl_FPtipoPagamento.Enabled = false;

                if (ricaricaCitta)
                {
                    ddl_FPcitta.Items.Clear();
                    SessionManager.ListaCittaFornitori.Sort();
                    foreach (string citta in SessionManager.ListaCittaFornitori)
                    {
                        ddl_FPcitta.Items.Add(new ListItem(citta.ToUpper(), citta));
                    }
                    ddl_FPcitta.Items.Insert(0, new ListItem("<seleziona>", ""));
                }
            }

            
        }
        #endregion
    }
}