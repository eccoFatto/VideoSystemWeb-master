using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoSystemWeb.BLL;
using VideoSystemWeb.Entity;

namespace VideoSystemWeb.Agenda.userControl
{
    
    public partial class Lavorazione : System.Web.UI.UserControl
    {
        public delegate void PopupHandler(string operazionePopup); // delegato per l'evento
        public event PopupHandler RichiediOperazionePopup; //evento
        BasePage basePage = new BasePage();

        private const int COLLABORATORE = 0;
        private const int FORNITORE = 1;

        List<DatiArticoliLavorazione> listaDatiArticoliLavorazione
        {
            get
            {
                if (ViewState["listaDatiArticoliLavorazione"] == null || ((List<DatiArticoliLavorazione>)ViewState["listaDatiArticoliLavorazione"]).Count() == 0)
                {
                    ViewState["listaDatiArticoliLavorazione"] = new List<DatiArticoliLavorazione>();
                }
                return (List<DatiArticoliLavorazione>)ViewState["listaDatiArticoliLavorazione"];
            }
            set
            {
                ViewState["listaDatiArticoliLavorazione"] = value;
            }
        }
        DatiLavorazione lavorazioneCorrente
        {
            get
            {
                if (ViewState["lavorazioneCorrente"] == null)
                {
                    ViewState["lavorazioneCorrente"] = new DatiLavorazione();
                }
                return (DatiLavorazione)ViewState["lavorazioneCorrente"];
            }
            set
            {
                ViewState["lavorazioneCorrente"] = value;
            }
        }
        List<ArticoliGruppi> listaArticoliGruppiLavorazione
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                gvGruppi.DataSource = listaArticoliGruppiLavorazione;
                gvGruppi.DataBind();

                PopolaCombo();
            }
            else
            {
                // SELEZIONO L'ULTIMA TAB SELEZIONATA
                ScriptManager.RegisterStartupScript(Page, GetType(), "apriTabGiusta", script: "openTabEventoLavorazione(event,'" + hf_tabSelezionataLavorazione.Value + "')", addScriptTags: true);
            }
        }

        #region COMPORTAMENTO ELEMENTI PAGINA
        

        protected void gvGruppi_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            long idSelezione = Convert.ToInt64(e.CommandArgument);

            ArticoliGruppi articoloGruppo = listaArticoliGruppiLavorazione.FirstOrDefault(X => X.Id == idSelezione);

            if (articoloGruppo.Isgruppo)
            {
                aggiungiArticoliDelGruppoAListaArticoli(articoloGruppo.IdOggetto);
            }
            else
            {
                aggiungiArticoloAListaArticoli(articoloGruppo.IdOggetto);
            }

            if (listaDatiArticoliLavorazione != null && listaDatiArticoliLavorazione.Count > 0)
            {
                lbl_selezionareArticolo.Visible = false;
                gvArticoliLavorazione.DataSource = listaDatiArticoliLavorazione;
                gvArticoliLavorazione.DataBind();

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
                articoloSelezionato = listaDatiArticoliLavorazione.FirstOrDefault(x => x.IdentificatoreOggetto == identificatoreOggetto);
            }
            else
            {
                ViewState["idArticoloLavorazione"] = id;
                articoloSelezionato = listaDatiArticoliLavorazione.FirstOrDefault(x => x.Id == id);
            }

            int indexArticolo = listaDatiArticoliLavorazione.IndexOf(articoloSelezionato);

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
                    FiguraProfessionale figuraProfessionale = SessionManager.listaCompletaFigProf.FirstOrDefault(x => x.Id == articoloSelezionato.IdCollaboratori);
                    if (figuraProfessionale == null)
                    {
                        figuraProfessionale = SessionManager.listaCompletaFigProf.FirstOrDefault(x => x.Id == articoloSelezionato.IdFornitori);
                    }

                    AbilitaComponentiCosto(articoloSelezionato);
                    AbilitaComponentiFiguraProfessionale(figuraProfessionale);

                    if (figuraProfessionale != null)
                    {
                        PopolaNominativi(SessionManager.listaCompletaFigProf.Where(x => x.Tipo == figuraProfessionale.Tipo).ToList());

                        ddl_FPtipo.SelectedValue = figuraProfessionale.Tipo.ToString();
                        txt_FPnotaCollaboratore.Text = articoloSelezionato.Nota;
                        ddl_FPnominativo.SelectedValue = figuraProfessionale.Id.ToString();
                        txt_FPtelefono.Text = figuraProfessionale.Telefono;
                    }
                    else
                    {
                        AbilitaComponentiFiguraProfessionale(null);

                        SessionManager.listaCittaCollaboratori.Sort();
                        foreach (string citta in SessionManager.listaCittaCollaboratori)
                        {
                            ddl_FPcitta.Items.Add(new ListItem(citta.ToUpper(), citta));
                        }
                        ddl_FPcitta.Items.Insert(0, new ListItem("<seleziona>", ""));

                        PopolaNominativi(SessionManager.listaCompletaFigProf.Where(x => x.Tipo == COLLABORATORE).ToList());
                        ddl_FPnominativo.SelectedValue = "";

                        ddl_FPtipo.SelectedValue = COLLABORATORE.ToString();
                        
                        txt_FPtelefono.Text = "";
                    }

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriModificaArticolo", script: "javascript: document.getElementById('" + panelModificaArticolo.ClientID + "').style.display='block'", addScriptTags: true);

                    break;
                case "elimina":
                    listaDatiArticoliLavorazione.Remove(articoloSelezionato);
                    gvArticoliLavorazione.DataSource = listaDatiArticoliLavorazione;
                    gvArticoliLavorazione.DataBind();

                    AggiornaTotali();
                    ResetPanelLavorazione();

                    break;
                case "moveUp":
                    if (indexArticolo > 0)
                    {
                        listaDatiArticoliLavorazione.Remove(articoloSelezionato);
                        listaDatiArticoliLavorazione.Insert(indexArticolo - 1, articoloSelezionato);
                        gvArticoliLavorazione.DataSource = listaDatiArticoliLavorazione;
                        gvArticoliLavorazione.DataBind();
                    }
                    break;
                case "moveDown":
                    if (indexArticolo < listaDatiArticoliLavorazione.Count - 1)
                    {
                        listaDatiArticoliLavorazione.Remove(articoloSelezionato);
                        listaDatiArticoliLavorazione.Insert(indexArticolo + 1, articoloSelezionato);
                        gvArticoliLavorazione.DataSource = listaDatiArticoliLavorazione;
                        gvArticoliLavorazione.DataBind();
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
                articoloSelezionato = listaDatiArticoliLavorazione.FirstOrDefault(x => x.IdentificatoreOggetto == identificatoreOggetto);
            }
            else
            {
                int idArticolo = (int)ViewState["idArticoloLavorazione"];
                articoloSelezionato = listaDatiArticoliLavorazione.FirstOrDefault(x => x.Id == idArticolo);
            }

            var index = listaDatiArticoliLavorazione.IndexOf(articoloSelezionato);

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


            listaDatiArticoliLavorazione = listaDatiArticoliLavorazione.OrderByDescending(x => x.Prezzo).ToList();

            ResetPanelLavorazione();

            RichiediOperazionePopup("UPDATE");
        }

        protected void gvArticoliLavorazione_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DatiArticoliLavorazione rigaCorrente = (DatiArticoliLavorazione)e.Row.DataItem;

                if (rigaCorrente.IdCollaboratori != null && rigaCorrente.IdCollaboratori.HasValue)
                {
                    FiguraProfessionale figuraProfessionale = SessionManager.listaCompletaFigProf.FirstOrDefault(x => x.Id == rigaCorrente.IdCollaboratori);
                    ((Label)e.Row.FindControl("lbl_Riferimento")).Text = figuraProfessionale.Nome + " " + figuraProfessionale.Cognome;
                }

                if (rigaCorrente.IdFornitori != null && rigaCorrente.IdFornitori.HasValue)
                {
                    FiguraProfessionale figuraProfessionale = SessionManager.listaCompletaFigProf.FirstOrDefault(x => x.Id == rigaCorrente.IdFornitori);
                    ((Label)e.Row.FindControl("lbl_Riferimento")).Text = figuraProfessionale.Nome + " " + figuraProfessionale.Cognome;
                }

                if (rigaCorrente.IdTipoPagamento != null && rigaCorrente.IdTipoPagamento.HasValue)
                {
                    ((Label)e.Row.FindControl("lbl_TipoPagamento")).Text = SessionManager.listaTipiPagamento.FirstOrDefault(x => x.id == rigaCorrente.IdTipoPagamento).nome;
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
        { }
        #endregion

        #region OPERAZIONI LAVORAZIONE
        private void PopolaCombo()
        {
            Esito esito = new Esito();

            #region CAPITECNICI
            List<Anag_Qualifiche_Collaboratori> listaCapiTecnici = SessionManager.listaQualificheCollaboratori.Where(x => x.Qualifica == "Capo Tecnico").ToList<Anag_Qualifiche_Collaboratori>();

            List<Anag_Collaboratori> listaAnagraficheCapiTecnici = (from Item1 in SessionManager.listaAnagraficheCollaboratori
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
            List<Anag_Qualifiche_Collaboratori> listaProduttori = SessionManager.listaQualificheCollaboratori.Where(x => x.Qualifica == "Produttore").ToList<Anag_Qualifiche_Collaboratori>();

            List<Anag_Collaboratori> listaAnagraficheProduttori = (from Item1 in SessionManager.listaAnagraficheCollaboratori
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
            foreach (Tipologica tipoPagamento in SessionManager.listaTipiPagamento)
            {
                ddl_FPtipoPagamento.Items.Add(new ListItem(tipoPagamento.nome, tipoPagamento.id.ToString()));
            }
            ddl_FPtipoPagamento.Items.Insert(0, new ListItem("<seleziona>", ""));
            #endregion

            #region QUALIFICHE

            ddl_FPqualifica.Items.Clear();
            foreach (Tipologica qualifica in SessionManager.listaQualifiche)
            {
                ddl_FPqualifica.Items.Add(new ListItem(qualifica.nome, qualifica.id.ToString()));
            }
            ddl_FPqualifica.Items.Insert(0, new ListItem("<seleziona>", ""));
            #endregion
        }

        protected void filtraFP(object sender, EventArgs e)
        {
            Esito esito = new Esito();

            int tipoFP = int.Parse(ddl_FPtipo.SelectedValue);
            string qualificaFP = ddl_FPqualifica.SelectedValue;
            string cittaFP = ddl_FPcitta.SelectedValue;

            FiguraProfessionale figuraProfessionaleDummy = new FiguraProfessionale() { Tipo = tipoFP };

            AbilitaComponentiFiguraProfessionale(figuraProfessionaleDummy);

            List<FiguraProfessionale> listaFPfiltrata = SessionManager.listaCompletaFigProf.Where(x => x.Tipo == tipoFP).ToList();

            if (tipoFP == 0 &&  !string.IsNullOrEmpty(qualificaFP))
            {
                listaFPfiltrata = listaFPfiltrata.Where(x => x.Qualifiche.Where(y => y.Id == int.Parse(qualificaFP)).Count() > 0).ToList();
            }

            if (cittaFP != "")
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
                fpSelezionata = SessionManager.listaCompletaFigProf.FirstOrDefault(x => x.Id == int.Parse(ddl_FPnominativo.SelectedValue));

                AbilitaComponentiFiguraProfessionale(fpSelezionata);
            }
            else
            {
                AbilitaComponentiFiguraProfessionale(null);
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
            int idLavorazione = lavorazioneCorrente == null ? 0 : lavorazioneCorrente.Id;

            listaDatiArticoliLavorazione.AddRange(Articoli_BLL.Instance.CaricaListaArticoliLavorazioneByIDGruppo(idLavorazione, idGruppo, ref esito));
            listaDatiArticoliLavorazione = listaDatiArticoliLavorazione.OrderByDescending(x => x.Prezzo).ToList();

            lbl_selezionareArticolo.Visible = listaDatiArticoliLavorazione == null || listaDatiArticoliLavorazione.Count == 0;
        }

        private void aggiungiArticoloAListaArticoli(int idArticolo)
        {
            Esito esito = new Esito();
            int idLavorazione = lavorazioneCorrente == null ? 0 : lavorazioneCorrente.Id;

            DatiArticoliLavorazione articoloLavorazione = Articoli_BLL.Instance.CaricaArticoloLavorazioneByID(idLavorazione, idArticolo, ref esito);

            listaDatiArticoliLavorazione.Add(articoloLavorazione);
            listaDatiArticoliLavorazione = listaDatiArticoliLavorazione.OrderByDescending(x => x.Prezzo).ToList();

            lbl_selezionareArticolo.Visible = listaDatiArticoliLavorazione == null || listaDatiArticoliLavorazione.Count == 0;
        }

        private void AggiornaTotali()
        {
            decimal totPrezzo = 0;
            decimal totCosto = 0;
            decimal totIva = 0;
            decimal percRicavo = 0;

            if (listaDatiArticoliLavorazione != null && listaDatiArticoliLavorazione.Count > 0)
            {
                foreach (DatiArticoliLavorazione art in listaDatiArticoliLavorazione)
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
            gvArticoliLavorazione.DataSource = listaDatiArticoliLavorazione;
            gvArticoliLavorazione.DataBind();

            lbl_selezionareArticolo.Visible = (listaDatiArticoliLavorazione == null || listaDatiArticoliLavorazione.Count == 0);

            AggiornaTotali();
        }

        public void PopolaLavorazione(int idDatiAgenda, int idCliente)
        {
            Esito esito = new Esito();

            #region INIZIALIZZAZIONE OGGETTI LAVORAZIONE CORRENTE
            lavorazioneCorrente = Dati_Lavorazione_BLL.Instance.getDatiLavorazioneByIdEvento(idDatiAgenda, ref esito);
            SessionManager.listaReferenti = Anag_Referente_Clienti_Fornitori_BLL.Instance.getReferentiByIdAzienda(ref esito, idCliente);
            #endregion

            if (esito.codice != Esito.ESITO_OK)
            {
                basePage.ShowError(esito.descrizione);
            }
            else
            {
                CreaNuovaLavorazione(listaDatiArticoliLavorazione);

                if (lavorazioneCorrente != null)
                {
                    txt_Ordine.Text = lavorazioneCorrente.Ordine;
                    txt_Fattura.Text = lavorazioneCorrente.Fattura;
                    ddl_Contratto.SelectedValue = lavorazioneCorrente.IdContratto == null ? "": lavorazioneCorrente.IdContratto.ToString();
                    ddl_Referente.SelectedValue = lavorazioneCorrente.IdReferente == null ? "" : lavorazioneCorrente.IdReferente.ToString();
                    ddl_Capotecnico.SelectedValue = lavorazioneCorrente.IdCapoTecnico == null ? "" : lavorazioneCorrente.IdCapoTecnico.ToString();
                    ddl_Produttore.SelectedValue = lavorazioneCorrente.IdProduttore == null ? "" : lavorazioneCorrente.IdProduttore.ToString();

                    listaDatiArticoliLavorazione = lavorazioneCorrente.ListaArticoliLavorazione;

                    gvArticoliLavorazione.DataSource = listaDatiArticoliLavorazione;
                    gvArticoliLavorazione.DataBind();
                }
                lbl_selezionareArticolo.Visible = (listaDatiArticoliLavorazione == null || listaDatiArticoliLavorazione.Count == 0);
                AggiornaTotali();
            }
        }

        public void CreaNuovaLavorazione(List<DatiArticoliLavorazione> listaArticoliLavorazione)
        {
            Esito esito = new Esito();

            ddl_Referente.Items.Clear();
            foreach (Anag_Referente_Clienti_Fornitori referente in SessionManager.listaReferenti)
            {
                ddl_Referente.Items.Add(new ListItem(referente.Nome + " " + referente.Cognome, referente.Id.ToString()));
            }
            ddl_Referente.Items.Insert(0, new ListItem("<seleziona>", ""));


            listaDatiArticoliLavorazione = listaArticoliLavorazione;

            lbl_selezionareArticolo.Visible = listaDatiArticoliLavorazione == null || listaDatiArticoliLavorazione.Count == 0;

            gvArticoliLavorazione.DataSource = listaDatiArticoliLavorazione;
            gvArticoliLavorazione.DataBind();
        }

        public void ClearLavorazione()
        {
            lbl_selezionareArticolo.Visible = true;
            listaDatiArticoliLavorazione = null;
            gvArticoliLavorazione.DataSource = null;
            gvArticoliLavorazione.DataBind();

            txt_TotPrezzo.Text = "";
            txt_TotCosto.Text = "";
            txt_TotIva.Text = "";
            txt_PercRicavo.Text = "";

            ResetPanelLavorazione();
        }

        public DatiLavorazione CreaDatiLavorazione()
        {
            DatiLavorazione datiLavorazione = new DatiLavorazione();
            datiLavorazione.Id = lavorazioneCorrente == null ? 0 : lavorazioneCorrente.Id;

            datiLavorazione.Ordine = txt_Ordine.Text;
            datiLavorazione.Fattura = txt_Fattura.Text;
            datiLavorazione.IdContratto = string.IsNullOrEmpty(ddl_Contratto.SelectedValue) ? null : (int?)int.Parse(ddl_Contratto.SelectedValue);
            datiLavorazione.IdReferente = string.IsNullOrEmpty(ddl_Referente.SelectedValue) ? null : (int?)int.Parse(ddl_Referente.SelectedValue); //int.Parse(ddl_Referente.SelectedValue);
            datiLavorazione.IdCapoTecnico = string.IsNullOrEmpty(ddl_Capotecnico.SelectedValue) ? null : (int?)int.Parse(ddl_Capotecnico.SelectedValue); //int.Parse(ddl_Capotecnico.SelectedValue);
            datiLavorazione.IdProduttore = string.IsNullOrEmpty(ddl_Produttore.SelectedValue) ? null : (int?)int.Parse(ddl_Produttore.SelectedValue); //int.Parse(ddl_Produttore.SelectedValue);
            datiLavorazione.ListaArticoliLavorazione = listaDatiArticoliLavorazione;
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

        private void AbilitaComponentiFiguraProfessionale(FiguraProfessionale figuraProfessionale)
        {
            ddl_FPcitta.Items.Clear();

            if (figuraProfessionale==null || figuraProfessionale.Id == 0)
            {
                ddl_FPtipoPagamento.Attributes.Add("readonly", "readonly");
                ddl_FPtipoPagamento.CssClass = "w3-input w3-border w3-disabled";

                chk_ModCosto.Attributes.Add("readonly", "readonly");
                chk_ModCosto.CssClass = "w3-input w3-border w3-disabled";

                txt_FPnetto.Attributes.Add("readonly", "readonly");
                txt_FPnetto.CssClass = "w3-input w3-border w3-disabled";

                txt_FPnotaCollaboratore.Attributes.Add("readonly", "readonly");
                txt_FPnotaCollaboratore.CssClass = "w3-input w3-border w3-disabled";
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

                SessionManager.listaCittaCollaboratori.Sort();
                foreach (string citta in SessionManager.listaCittaCollaboratori)
                {
                    ddl_FPcitta.Items.Add(new ListItem(citta.ToUpper(), citta));
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

                SessionManager.listaCittaFornitori.Sort();
                foreach (string citta in SessionManager.listaCittaFornitori)
                {
                    ddl_FPcitta.Items.Add(new ListItem(citta.ToUpper(), citta));
                }
            }

            ddl_FPcitta.Items.Insert(0, new ListItem("<seleziona>", ""));
        }
        #endregion
    }
}