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
    public partial class Offerta : System.Web.UI.UserControl
    {
        public delegate void PopupHandler(string operazionePopup); // delegato per l'evento
        public event PopupHandler RichiediOperazionePopup; //evento
        BasePage basePage = new BasePage();

        #region ELENCO CHIAVI VIEWSTATE
        private const string VIEWSTATE_LISTAARTICOLIGRUPPI = "listaArticoliGruppi";
        private const string VIEWSTATE_IDARTICOLO = "idArticolo";
        private const string VIEWSTATE_IDENTIFICATOREARTICOLO = "identificatoreArticolo";
        private const string VIEWSTATE_LISTACODICILAVORO = "listaCodiciLavoro";
        private const string VIEWSTATE_LISTAPRODUZIONI = "listaProduzioni";
        private const string VIEWSTATE_LISTALAVORAZIONI = "listaLavorazioni";
        private const string VIEWSTATE_LISTALUOGHI = "listaLuoghi";
        #endregion

        List<ArticoliGruppi> ListaArticoliGruppi
        {
            get
            {
                if (ViewState[VIEWSTATE_LISTAARTICOLIGRUPPI] == null || ((List<ArticoliGruppi>)ViewState[VIEWSTATE_LISTAARTICOLIGRUPPI]).Count == 0)
                {
                    ViewState[VIEWSTATE_LISTAARTICOLIGRUPPI] = Articoli_BLL.Instance.CaricaListaArticoliGruppi();
                }

                return (List<ArticoliGruppi>)ViewState[VIEWSTATE_LISTAARTICOLIGRUPPI];
            }
            set
            {
                ViewState[VIEWSTATE_LISTAARTICOLIGRUPPI] = value;
            }
        }
        List<string> ListaCodiciLavoro
        {
            get
            {
                if (ViewState[VIEWSTATE_LISTACODICILAVORO] == null || ((List<ArticoliGruppi>)ViewState[VIEWSTATE_LISTACODICILAVORO]).Count == 0)
                {
                    ViewState[VIEWSTATE_LISTACODICILAVORO] = Offerta_BLL.Instance.GetAllCodiciLavoro();
                }

                return (List<string>)ViewState[VIEWSTATE_LISTACODICILAVORO];
            }
            set
            {
                ViewState[VIEWSTATE_LISTACODICILAVORO] = value;
            }
        }
        List<string> ListaProduzioni
        {
            get
            {
                if (ViewState[VIEWSTATE_LISTAPRODUZIONI] == null || ((List<ArticoliGruppi>)ViewState[VIEWSTATE_LISTAPRODUZIONI]).Count == 0)
                {
                    ViewState[VIEWSTATE_LISTAPRODUZIONI] = Offerta_BLL.Instance.GetAllProduzioni();
                }

                return (List<string>)ViewState[VIEWSTATE_LISTAPRODUZIONI];
            }
            set
            {
                ViewState[VIEWSTATE_LISTAPRODUZIONI] = value;
            }
        }
        List<string> ListaLavorazioni
        {
            get
            {
                if (ViewState[VIEWSTATE_LISTALAVORAZIONI] == null || ((List<ArticoliGruppi>)ViewState[VIEWSTATE_LISTALAVORAZIONI]).Count == 0)
                {
                    ViewState[VIEWSTATE_LISTALAVORAZIONI] = Offerta_BLL.Instance.GetAllLavorazioni();
                }

                return (List<string>)ViewState[VIEWSTATE_LISTALAVORAZIONI];
            }
            set
            {
                ViewState[VIEWSTATE_LISTALAVORAZIONI] = value;
            }
        }
        List<string> ListaLuoghi
        {
            get
            {
                if (ViewState[VIEWSTATE_LISTALUOGHI] == null || ((List<ArticoliGruppi>)ViewState[VIEWSTATE_LISTALUOGHI]).Count == 0)
                {
                    ViewState[VIEWSTATE_LISTALUOGHI] = Offerta_BLL.Instance.GetAllLuoghi();
                }

                return (List<string>)ViewState[VIEWSTATE_LISTALUOGHI];
            }
            set
            {
                ViewState[VIEWSTATE_LISTALUOGHI] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                gvGruppi.DataSource = ListaArticoliGruppi;
                gvGruppi.DataBind();

                PopolaCombo();
            }
        }

        #region COMPORTAMENTO ELEMENTI PAGINA
        protected void btnOK_Click(object sender, EventArgs e)
        {
            DatiArticoli articoloSelezionato;

            if (ViewState[VIEWSTATE_IDARTICOLO] == null)
            {
                long identificatoreOggetto = (long)ViewState[VIEWSTATE_IDENTIFICATOREARTICOLO];
                articoloSelezionato = SessionManager.EventoSelezionato.ListaDatiArticoli.FirstOrDefault(x => x.IdentificatoreOggetto == identificatoreOggetto);
            }
            else
            {
                int idArticolo = (int)ViewState[VIEWSTATE_IDARTICOLO];
                articoloSelezionato = SessionManager.EventoSelezionato.ListaDatiArticoli.FirstOrDefault(x => x.Id == idArticolo);
            }

            var index = SessionManager.EventoSelezionato.ListaDatiArticoli.IndexOf(articoloSelezionato);

            articoloSelezionato.Descrizione = txt_Descrizione.Text;
            articoloSelezionato.DescrizioneLunga = txt_DescrizioneLunga.Text;
            articoloSelezionato.Costo = decimal.Parse(txt_Costo.Text);
            articoloSelezionato.Prezzo = decimal.Parse(txt_Prezzo.Text);
            articoloSelezionato.Iva = int.Parse(txt_Iva.Text);
            articoloSelezionato.Stampa = ddl_Stampa.SelectedValue == "1";
            articoloSelezionato.Quantita = int.Parse(txt_Quantita.Text);

            SessionManager.EventoSelezionato.ListaDatiArticoli = SessionManager.EventoSelezionato.ListaDatiArticoli.OrderByDescending(x => x.Prezzo).ToList();

            ResetPanelOfferta();

            RichiediOperazionePopup("UPDATE");
        }

        protected void gvGruppi_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            long idSelezione = Convert.ToInt64(e.CommandArgument);

            if (SessionManager.EventoSelezionato.ListaDatiArticoli == null)
            {
                SessionManager.EventoSelezionato.ListaDatiArticoli = new List<DatiArticoli>();
            }
            ArticoliGruppi articoloGruppo = ListaArticoliGruppi.FirstOrDefault(X => X.Id == idSelezione);

            if (articoloGruppo.Isgruppo)
            {
                AggiungiArticoliDelGruppoAListaArticoli(articoloGruppo.IdOggetto);
            }
            else
            {
                AggiungiArticoloAListaArticoli(articoloGruppo.IdOggetto);
            }

            if (SessionManager.EventoSelezionato.ListaDatiArticoli != null && SessionManager.EventoSelezionato.ListaDatiArticoli.Count > 0)
            {
                lbl_selezionareArticolo.Visible = false;
                gvArticoli.DataSource = SessionManager.EventoSelezionato.ListaDatiArticoli;
                gvArticoli.DataBind();

                AggiornaTotali();

                ResetPanelOfferta();

                RichiediOperazionePopup("UPDATE");
            }
        }

        protected void gvGruppi_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (((ArticoliGruppi)e.Row.DataItem).Isgruppo)
                {
                    e.Row.Font.Bold = true;
                }
            }
        }

        protected void gvArticoli_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DatiArticoli articoloSelezionato;

            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            int id = Convert.ToInt32(commandArgs[0]);

            if (id == 0)
            {
                long identificatoreOggetto = Convert.ToInt64(commandArgs[1]);
                ViewState[VIEWSTATE_IDENTIFICATOREARTICOLO] = identificatoreOggetto;
                articoloSelezionato = SessionManager.EventoSelezionato.ListaDatiArticoli.FirstOrDefault(x => x.IdentificatoreOggetto == identificatoreOggetto);
            }
            else
            {
                ViewState[VIEWSTATE_IDARTICOLO] = id;
                articoloSelezionato = SessionManager.EventoSelezionato.ListaDatiArticoli.FirstOrDefault(x => x.Id == id);
            }

            int indexArticolo = SessionManager.EventoSelezionato.ListaDatiArticoli.IndexOf(articoloSelezionato);

            switch (e.CommandName)
            {
                case "modifica":
                    txt_Descrizione.Text = articoloSelezionato.Descrizione;
                    txt_DescrizioneLunga.Text = articoloSelezionato.DescrizioneLunga;
                    txt_Costo.Text = articoloSelezionato.Costo.ToString();
                    txt_Prezzo.Text = articoloSelezionato.Prezzo.ToString();
                    txt_Iva.Text = articoloSelezionato.Iva.ToString();
                    txt_Quantita.Text = articoloSelezionato.Quantita.ToString();

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriModificaArticolo", script: "javascript: document.getElementById('" + panelModificaArticolo.ClientID + "').style.display='block'", addScriptTags: true);

                    break;
                case "elimina":
                    SessionManager.EventoSelezionato.ListaDatiArticoli.Remove(articoloSelezionato);
                    gvArticoli.DataSource = SessionManager.EventoSelezionato.ListaDatiArticoli;
                    gvArticoli.DataBind();

                    AggiornaTotali();
                    ResetPanelOfferta();

                    break;
                case "moveUp":
                    if (indexArticolo > 0)
                    {
                        SessionManager.EventoSelezionato.ListaDatiArticoli.Remove(articoloSelezionato);
                        SessionManager.EventoSelezionato.ListaDatiArticoli.Insert(indexArticolo - 1, articoloSelezionato);
                        gvArticoli.DataSource = SessionManager.EventoSelezionato.ListaDatiArticoli;
                        gvArticoli.DataBind();
                    }
                    break;
                case "moveDown":
                    if (indexArticolo < SessionManager.EventoSelezionato.ListaDatiArticoli.Count - 1)
                    {
                        SessionManager.EventoSelezionato.ListaDatiArticoli.Remove(articoloSelezionato);
                        SessionManager.EventoSelezionato.ListaDatiArticoli.Insert(indexArticolo + 1, articoloSelezionato);
                        gvArticoli.DataSource = SessionManager.EventoSelezionato.ListaDatiArticoli;
                        gvArticoli.DataBind();
                    }
                    break;
            }

            RichiediOperazionePopup("UPDATE");
        }

        protected void gvArticoli_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[5].Text = ((DatiArticoli)e.Row.DataItem).Stampa ? "Si" : "No";
            }
        }

        protected void btnRecuperaOfferta_Click(object sender, EventArgs e)
        {
            ClearRecuperaOfferta();
            panelModificaArticolo.Style.Add("display", "none");
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriModificaArticolo", script: "javascript: document.getElementById('" + panelRecuperaOfferta.ClientID + "').style.display='block'", addScriptTags: true);
            RichiediOperazionePopup("UPDATE");
        }

        protected void btnEliminaArticoli_Click(object sender, EventArgs e)
        {
            SessionManager.EventoSelezionato.ListaDatiArticoli = null;

            ResetPanelOfferta();

            RichiediOperazionePopup("UPDATE");
        }

        protected void btn_CancellazioneMassiva_Click(object sender, EventArgs e)
        {
            List<DatiArticoli> listaArticoliDaEliminare = new List<DatiArticoli>();
            for (int i = 0; i < gvArticoli.Rows.Count; i++)
            {
                CheckBox checkboxdelete = ((CheckBox)gvArticoli.Rows[i].FindControl("chkDelete"));

                if (checkboxdelete.Checked == true)
                {
                    listaArticoliDaEliminare.Add(SessionManager.EventoSelezionato.ListaDatiArticoli.ElementAt(i));
                }
            }

            foreach (DatiArticoli articoloSelezionato in listaArticoliDaEliminare)
            {
                SessionManager.EventoSelezionato.ListaDatiArticoli.Remove(articoloSelezionato);
            }

            gvArticoli.DataSource = SessionManager.EventoSelezionato.ListaDatiArticoli;
            gvArticoli.DataBind();

            // AggiornaTotali();
            ResetPanelOfferta();
            
            RichiediOperazionePopup("UPDATE");
        }

        protected void btn_cerca_FiltroRecuperaOfferta_Click(object sender, EventArgs e)
        {
            List<DatiAgenda> listaDatiAgenda = new List<DatiAgenda>();
            Esito esito = Offerta_BLL.Instance.GetListaDatiAgendaByFiltri(txt_dataLavorazione_FiltroRecuperaOfferta.Text,
                                                                          ddl_tipologia_FiltroRecuperaOfferta.SelectedValue,
                                                                          ddl_cliente_FiltroRecuperaOfferta.SelectedValue,
                                                                          ddl_produzione_FiltroRecuperaOfferta.SelectedValue,
                                                                          ddl_lavorazione_FiltroRecuperaOfferta.SelectedValue,
                                                                          ddl_luogo_FiltroRecuperaOfferta.SelectedValue,
                                                                          ddl_codiceLavoro_FiltroRecuperaOfferta.SelectedValue,
                                                                          ref listaDatiAgenda);

            gv_OfferteRecuperate.DataSource = listaDatiAgenda;
            gv_OfferteRecuperate.DataBind();

            upRecuperaOfferta.Update();
        }

        protected void gv_OfferteRecuperate_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_OfferteRecuperate.PageIndex = e.NewPageIndex;
            btn_cerca_FiltroRecuperaOfferta_Click(null, null);
        }

        protected void gv_OfferteRecuperate_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.background='yellow';";
                e.Row.Attributes["onmouseout"] = "this.style.background='transparent';";
            }
        }

        protected void gv_OfferteRecuperate_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string idDatiAgenda = e.CommandArgument.ToString();
            List<DatiArticoli> listaDatiArticoli = new List<DatiArticoli>();
            Esito esito = new Esito();
            switch (e.CommandName)
            {
                case "seleziona":
                    esito = Offerta_BLL.Instance.GetListaDatiArticoliByIdDatiAgenda(idDatiAgenda, ref listaDatiArticoli);

                    if (esito.Codice != Esito.ESITO_OK)
                    {
                        basePage.ShowError("Errore nel recupero dell'offerta selezionata");
                    }
                    else
                    {
                        if (SessionManager.EventoSelezionato.ListaDatiArticoli == null)
                        {
                            SessionManager.EventoSelezionato.ListaDatiArticoli = new List<DatiArticoli>();
                        }
                        SessionManager.EventoSelezionato.ListaDatiArticoli.AddRange(listaDatiArticoli);

                        basePage.ShowSuccess("Offerta recuperata correttamente");
                        ResetPanelOfferta();
                        RichiediOperazionePopup("UPDATE");
                    }

                    break;
            }
        }

        #endregion

        #region OPERAZIONI PAGINA
        private void PopolaCombo()
        {
            List<Tipologica> listaTipologie = SessionManager.ListaTipiTipologie;
            ddl_tipologia_FiltroRecuperaOfferta.Items.Add(new ListItem("<seleziona>", ""));
            foreach (Tipologica tipologia in listaTipologie)
            {
                ddl_tipologia_FiltroRecuperaOfferta.Items.Add(new ListItem(tipologia.nome, tipologia.id.ToString()));
            }

            List<Anag_Clienti_Fornitori> listaCienti = SessionManager.ListaClientiFornitori.Where(x => x.Cliente == true).ToList<Anag_Clienti_Fornitori>();
            ddl_cliente_FiltroRecuperaOfferta.Items.Add(new ListItem("<seleziona>", ""));
            foreach (Anag_Clienti_Fornitori cliente in listaCienti)
            {
                ddl_cliente_FiltroRecuperaOfferta.Items.Add(new ListItem(cliente.RagioneSociale, cliente.Id.ToString()));
            }

            ddl_codiceLavoro_FiltroRecuperaOfferta.Items.Add(new ListItem("<seleziona>", ""));
            foreach (string codiceLavoro in ListaCodiciLavoro)
            {
                ddl_codiceLavoro_FiltroRecuperaOfferta.Items.Add(new ListItem(codiceLavoro, codiceLavoro));
            }

            ddl_produzione_FiltroRecuperaOfferta.Items.Add(new ListItem("<seleziona>", ""));
            foreach (string produzione in ListaProduzioni)
            {
                ddl_produzione_FiltroRecuperaOfferta.Items.Add(new ListItem(produzione, produzione));
            }

            ddl_lavorazione_FiltroRecuperaOfferta.Items.Add(new ListItem("<seleziona>", ""));
            foreach (string lavorazione in ListaLavorazioni)
            {
                ddl_lavorazione_FiltroRecuperaOfferta.Items.Add(new ListItem(lavorazione, lavorazione));
            }

            ddl_luogo_FiltroRecuperaOfferta.Items.Add(new ListItem("<seleziona>", ""));
            foreach (string luogo in ListaLuoghi)
            {
                ddl_luogo_FiltroRecuperaOfferta.Items.Add(new ListItem(luogo, luogo));
            }
        }
        #endregion

        #region OPERAZIONI OFFERTA
        private void AggiornaTotali()
        {
            decimal totPrezzo = 0;
            decimal totCosto = 0;
            decimal totIva = 0;
            decimal percRicavo = 0;

            if (SessionManager.EventoSelezionato.ListaDatiArticoli != null && SessionManager.EventoSelezionato.ListaDatiArticoli.Count > 0)
            {
                foreach (DatiArticoli art in SessionManager.EventoSelezionato.ListaDatiArticoli)
                {
                    totPrezzo += art.Prezzo * art.Quantita;
                    totCosto += art.Costo * art.Quantita;
                    totIva += (art.Prezzo * art.Iva / 100) * art.Quantita;
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

        public void ClearOfferta()
        {
            ClearModificaArticoli();
            lbl_selezionareArticolo.Visible = true;
            SessionManager.EventoSelezionato.ListaDatiArticoli = null;

            gvArticoli.DataSource = null;
            gvArticoli.DataBind();

            txt_TotPrezzo.Text = "";
            txt_TotCosto.Text = "";
            txt_TotIva.Text = "";
            txt_PercRicavo.Text = "";

            ResetPanelOfferta();
        }

        public void ClearRecuperaOfferta()
        {
            txt_dataLavorazione_FiltroRecuperaOfferta.Text = string.Empty;
            ddl_cliente_FiltroRecuperaOfferta.SelectedIndex =
            ddl_codiceLavoro_FiltroRecuperaOfferta.SelectedIndex =
            ddl_lavorazione_FiltroRecuperaOfferta.SelectedIndex =
            ddl_luogo_FiltroRecuperaOfferta.SelectedIndex =
            ddl_produzione_FiltroRecuperaOfferta.SelectedIndex =
            ddl_tipologia_FiltroRecuperaOfferta.SelectedIndex = 0;

            gv_OfferteRecuperate.DataSource = null;
            gv_OfferteRecuperate.DataBind();
        }

        private void ClearModificaArticoli()
        {
            ViewState[VIEWSTATE_IDENTIFICATOREARTICOLO] = null;
            txt_Descrizione.Text = "";
            txt_DescrizioneLunga.Text = "";
            txt_Costo.Text = "";
            txt_Prezzo.Text = "";
            txt_Iva.Text = "";
            txt_Quantita.Text = "";
        }

        private void ResetPanelOfferta()
        {
            ClearModificaArticoli();

            gvArticoli.DataSource = SessionManager.EventoSelezionato.ListaDatiArticoli;
            gvArticoli.DataBind();

            btnRecuperaOfferta.Visible = true;
            btnEliminaArticoli.Visible = (SessionManager.EventoSelezionato.ListaDatiArticoli != null && SessionManager.EventoSelezionato.ListaDatiArticoli.Count > 0);
            lbl_selezionareArticolo.Visible = (SessionManager.EventoSelezionato.ListaDatiArticoli == null || SessionManager.EventoSelezionato.ListaDatiArticoli.Count == 0);

            ViewState[VIEWSTATE_IDENTIFICATOREARTICOLO] = null;
            ViewState[VIEWSTATE_IDARTICOLO] = null;

           AggiornaTotali();
        }

        public void PopolaOfferta()
        {
            Esito esito = new Esito();
            int idDatiAgenda = SessionManager.EventoSelezionato.id;

            SessionManager.EventoSelezionato.ListaDatiArticoli = Articoli_BLL.Instance.CaricaListaArticoliByIDEvento(idDatiAgenda, ref esito);
            if (SessionManager.EventoSelezionato.ListaDatiArticoli != null && SessionManager.EventoSelezionato.ListaDatiArticoli.Count > 0)
            {
                lbl_selezionareArticolo.Visible = false;
                gvArticoli.DataSource = SessionManager.EventoSelezionato.ListaDatiArticoli;
                gvArticoli.DataBind();

                AggiornaTotali();
            }
            ResetPanelOfferta();
        }

        private void AggiungiArticoliDelGruppoAListaArticoli(int idGruppo)
        {
            Esito esito = new Esito();
            int idEvento = 0;
            SessionManager.EventoSelezionato.ListaDatiArticoli.AddRange(Articoli_BLL.Instance.CaricaListaArticoliByIDGruppo(idEvento, idGruppo, ref esito));
            SessionManager.EventoSelezionato.ListaDatiArticoli = SessionManager.EventoSelezionato.ListaDatiArticoli.OrderByDescending(x => x.Prezzo).ToList();
        }

        private void AggiungiArticoloAListaArticoli(int idArticolo)
        {
            Esito esito = new Esito();
            int idEvento = 0;
            SessionManager.EventoSelezionato.ListaDatiArticoli.Add(Articoli_BLL.Instance.CaricaArticoloByID(idEvento, idArticolo, ref esito));
            SessionManager.EventoSelezionato.ListaDatiArticoli = SessionManager.EventoSelezionato.ListaDatiArticoli.OrderByDescending(x => x.Prezzo).ToList();
        }

        public void AbilitaComponentiPopup(int statoEvento)
        {
            panelOfferta.Enabled = basePage.AbilitazioneInScrittura();

            if (basePage.AbilitazioneInScrittura())
            {
                if (statoEvento == Stato.Instance.STATO_PREVISIONE_IMPEGNO)
                {
                    panelOfferta.Enabled = false;
                }
                else if (statoEvento == Stato.Instance.STATO_OFFERTA)
                {
                    panelOfferta.Enabled = true;
                }
                else if (statoEvento == Stato.Instance.STATO_LAVORAZIONE)
                {
                    panelOfferta.Enabled = false;
                }
                else if (statoEvento == Stato.Instance.STATO_FATTURA)
                {
                    panelOfferta.Enabled = false;
                }
                else if (statoEvento == Stato.Instance.STATO_RIPOSO)
                {
                    panelOfferta.Enabled = false;
                }
            }
        }

        public void TrasformaInOfferta()
        {
            SessionManager.EventoSelezionato.id_stato = Stato.Instance.STATO_OFFERTA;
            SessionManager.EventoSelezionato.codice_lavoro = Protocolli_BLL.Instance.getCodLavFormattato();
        }
        #endregion
    }
}