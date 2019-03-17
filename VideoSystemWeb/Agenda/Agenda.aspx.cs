using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoSystemWeb.BLL;
using VideoSystemWeb.Entity;

namespace VideoSystemWeb.Agenda
{
    public partial class Agenda : BasePage
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        bool isUtenteAbilitatoInScrittura;
        string coloreViaggio;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            CheckIsMobile();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Esito esito = new Esito();

            popupAppuntamento.RichiediOperazionePopup += OperazioniPopup;
            popupOfferta.RichiediOperazionePopup += OperazioniPopup;

            isUtenteAbilitatoInScrittura = AbilitazioneInScrittura();
            Tipologica viaggio  = UtilityTipologiche.getElementByID(listaStati, DatiAgenda.STATO_VIAGGIO, ref esito);
            coloreViaggio = UtilityTipologiche.getParametroDaTipologica(viaggio, "color", ref esito);


            if (!IsPostBack)
            {
                DateTime dataPartenza = DateTime.Now;

                listaDatiAgenda = Agenda_BLL.Instance.CaricaDatiAgenda(dataPartenza, ref esito); //CARICO SOLO EVENTI VISUALIZZATI
                //listaDatiAgenda = Agenda_BLL.Instance.CaricaDatiAgenda(ref esito); //CARICO TUTTI GLI EVENTI

                ViewState["listaDatiAgenda"] = listaDatiAgenda;

                hf_valoreData.Value = dataPartenza.ToString("dd/MM/yyyy");
                gv_scheduler.DataSource = CreateDataTable(dataPartenza);
                gv_scheduler.DataBind();

                divLegenda.Controls.Add(new LiteralControl(CreaLegenda()));
                divFiltroAgenda.Controls.Add(new LiteralControl(CreaFiltriColonneAgenda()));
            }
            else
            {
                
                // SELEZIONO L'ULTIMA TAB SELEZIONATA
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriTabGiusta", script: "openTabEvento(event,'" + hf_tabSelezionata.Value + "')", addScriptTags: true);
            }
        }

        #region COMPORTAMENTO ELEMENTI PAGINA
        protected void btnsearch_Click(object sender, EventArgs e)
        {
            AggiornaAgenda();
        }

        protected void btnEditEvent_Click(object sender, EventArgs e)
        {
            DateTime dataEvento = DateTime.Parse(hf_data.Value);
            int risorsaEvento = int.Parse(hf_risorsa.Value);

            DatiAgenda eventoSelezionato = CreaEventoDaSelezioneAgenda(dataEvento, risorsaEvento);

            switch (eventoSelezionato.id_stato)
            {
                case DatiAgenda.STATO_PREVISIONE_IMPEGNO:
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "apritab", "openTabEvento(event,'Appuntamento');", true);
                    break;
                case DatiAgenda.STATO_OFFERTA:
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "apritab", "openTabEvento(event,'Offerta');", true);
                    break;
                case DatiAgenda.STATO_LAVORAZIONE:
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "apritab", "openTabEvento(event,'Lavorazione');", true);
                    break;
                default:
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "apritab", "openTabEvento(event,'Appuntamento');", true);
                    break;
            }

            AbilitaComponentiPopup();

            MostraPopup(eventoSelezionato);
        }

        protected void btnSalva_Click(object sender, EventArgs e)
        {
            Esito esito = SalvaEvento();
            if (esito.codice == Esito.ESITO_OK)
            {
                ChiudiPopup();
                ShowSuccess("Salvataggio eseguito correttamente");
            }
            else
            {
                ShowWarning(esito.descrizione);
                UpdatePopup();
            }
        }

        protected void btnRiepilogo_Click(object sender, EventArgs e)
        {
            Esito esito = new Esito();
            DatiAgenda eventoSelezionato = (DatiAgenda)ViewState["eventoSelezionato"];

            lbl_Data.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lbl_Produzione.Text = eventoSelezionato.produzione;
            lbl_Lavorazione.Text = eventoSelezionato.lavorazione;
            lbl_DataLavorazione.Text = eventoSelezionato.data_inizio_lavorazione.ToString("dd/MM/yyyy");

            Anag_Clienti_Fornitori cliente = Anag_Clienti_Fornitori_BLL.Instance.getAziendaById(eventoSelezionato.id_cliente, ref esito);
            lbl_Cliente.Text = cliente.RagioneSociale;
            lbl_IndirizzoCliente.Text = cliente.IndirizzoOperativo;
            lbl_PIvaCliente.Text = string.IsNullOrEmpty(cliente.PartitaIva) ? cliente.CodiceFiscale : cliente.PartitaIva;

            lbl_CodLavorazione.Text = eventoSelezionato.codice_lavoro;
            lbl_Protocollo.Text = eventoSelezionato.codice_lavoro +" - 12345678";

            List<DatiArticoli> listaDatiArticoli = popupOfferta.listaDatiArticoli.Where(x => x.Stampa).ToList<DatiArticoli>();

            gvArticoli.DataSource = listaDatiArticoli;
            gvArticoli.DataBind();

            decimal totPrezzo = 0; 
            foreach (DatiArticoli art in listaDatiArticoli)
            {
                totPrezzo += art.Prezzo * art.Quantita;
            }

            totale.Text = string.Format("{0:0.00}", totPrezzo);

            upEvento.Update();

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriRiepilogo", script: "javascript: document.getElementById('modalRiepilogoOfferta').style.display='block'", addScriptTags: true);
        }

        protected void btnElimina_Click(object sender, EventArgs e)
        {
            //panelErrore.Style.Add("display", "none");

            Agenda_BLL.Instance.EliminaEvento(((DatiAgenda)ViewState["eventoSelezionato"]).id);

            ChiudiPopup();
        }

        protected void btn_chiudi_Click(object sender, EventArgs e)
        {
            ChiudiPopup();
        }

        protected void btnOfferta_Click(object sender, EventArgs e)
        {
            popupAppuntamento.SetStato(DatiAgenda.STATO_OFFERTA);
            btnLavorazione.Visible = false;

            UpdatePopup();

            ScriptManager.RegisterStartupScript(this, typeof(Page), "passaAOfferta", "openTabEvento(event,'Offerta');", true);
        }

        protected void btnLavorazione_Click(object sender, EventArgs e)
        {
            popupAppuntamento.SetStato(DatiAgenda.STATO_LAVORAZIONE);
            UpdatePopup();
        }

        protected void btnStampa_Click(object sender, EventArgs e)
        {
            
        }
        #endregion

        #region OPERAZIONI AGENDA
        private DataTable CreateDataTable(DateTime data)
        {
            DataTable table = new DataTable();

            #region intestazione tabella
            DataColumn column;
            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = " ";

            table.Columns.Add(column);

            foreach (Tipologica risorsa in listaRisorse)
            {
                column = new DataColumn();
                column.DataType = typeof(string);
                column.ColumnName = risorsa.id.ToString(); // inserisco id risorsa per poi formattare la cella in RowDataBound 
                table.Columns.Add(column);
            }
            #endregion

            #region dati agenda
            for (int indiceRiga = 0; indiceRiga < 31; indiceRiga++)
            {
                DataRow row = table.NewRow();
                DateTime dataRiga = data.AddDays(indiceRiga);
                row[0] = dataRiga.ToString("dd/MM/yyyy");

                int indiceColonna = 1;
                foreach (Tipologica risorsa in listaRisorse)
                {
                    DatiAgenda datiAgendaFiltrati = Agenda_BLL.Instance.GetDatiAgendaByDataRisorsa(listaDatiAgenda, dataRiga, risorsa.id);
                    if (datiAgendaFiltrati != null)
                    {
                        row[indiceColonna++] = datiAgendaFiltrati.id.ToString(); // inserisco id datoAgenda per poi formattare la cella in RowDataBound 
                    }
                    else
                    {
                        row[indiceColonna++] = " ";
                    }
                }
                table.Rows.Add(row);
            }
            #endregion

            return table;
        }

        protected void gv_scheduler_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Esito esito = new Esito();
            e.Row.Cells[0].Attributes.Add("class", "first");

            #region intestazione tabella
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int indiceColonna = 1; indiceColonna <= listaRisorse.Count; indiceColonna++)
                {
                    string idRisorsa = (e.Row.Cells[indiceColonna].Text.Trim());
                    Tipologica risorsaCorrente = UtilityTipologiche.getElementByID(listaRisorse, int.Parse(idRisorsa), ref esito);
                   
                    string colore = UtilityTipologiche.getParametroDaTipologica(risorsaCorrente, "color", ref esito);

                    e.Row.Cells[indiceColonna].Attributes.Add("class", risorsaCorrente.sottotipo);
                    e.Row.Cells[indiceColonna].Attributes.Add("style", "background-color:" + colore + ";font-size:10pt;text-align:center;width:100px;");
                    e.Row.Cells[indiceColonna].Text = risorsaCorrente.nome;
                }
            }
            #endregion

            #region dati agenda
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Attributes.Add("class", "first");

                if ((int)DateTime.Parse(e.Row.Cells[0].Text).DayOfWeek == 0 || (int) DateTime.Parse(e.Row.Cells[0].Text).DayOfWeek == 6)
                {
                    e.Row.Cells[0].Attributes.Add("class", "first festivo");
                }

                for (int indiceColonna = 1; indiceColonna <= listaRisorse.Count; indiceColonna++)
                {
                    string data = e.Row.Cells[0].Text;
                    Tipologica risorsa = ((Tipologica)listaRisorse.ElementAt(indiceColonna - 1));
                    int id_risorsa = ((Tipologica)listaRisorse.ElementAt(indiceColonna - 1)).id;

                    e.Row.Cells[indiceColonna].Attributes.Add("class", risorsa.sottotipo);

                    if (!string.IsNullOrEmpty(e.Row.Cells[indiceColonna].Text.Trim()))
                    {
                        DatiAgenda datoAgendaCorrente = Agenda_BLL.Instance.GetDatiAgendaById(listaDatiAgenda, int.Parse(e.Row.Cells[indiceColonna].Text.Trim()));
                        Tipologica statoCorrente = UtilityTipologiche.getElementByID(listaStati, datoAgendaCorrente.id_stato, ref esito);

                        #region COLORE EVENTO
                        string colore;
                        colore = UtilityTipologiche.getParametroDaTipologica(statoCorrente, "color", ref esito);
                        string coloreFont;
                        coloreFont = UtilityTipologiche.getParametroDaTipologica(statoCorrente, "font_color", ref esito);
                        #endregion

                        e.Row.Cells[indiceColonna].CssClass = "cellaEvento " + risorsa.sottotipo;

                        #region TITOLO EVENTO
                        string tipologia = "";
                        if (datoAgendaCorrente.id_tipologia != null && datoAgendaCorrente.id_tipologia!= 0)
                        {
                            tipologia = UtilityTipologiche.getTipologicaById(EnumTipologiche.TIPO_TIPOLOGIE, (int)datoAgendaCorrente.id_tipologia, ref esito).nome;
                        }
                        string titoloEvento = datoAgendaCorrente.id_stato == DatiAgenda.STATO_RIPOSO ? "Riposo" : datoAgendaCorrente.produzione + "<br/>" + tipologia;
                        #endregion

                        // EVENTO GIORNO SINGOLO
                        if (IsPrimoGiorno(datoAgendaCorrente, DateTime.Parse(data)) && IsUltimoGiorno(datoAgendaCorrente, DateTime.Parse(data)))
                        {
                            Panel mainPanel = new Panel();
                            mainPanel.Controls.Add(new LiteralControl(titoloEvento));
                            mainPanel.CssClass = "round-corners-6px w3-tooltip";
                            mainPanel.Attributes.Add("style", "border: 2px solid " + colore + "; background-color:" + colore+";color:" +coloreFont);
                            
                            mainPanel.Controls.Add(AnteprimaEvento(datoAgendaCorrente));

                            e.Row.Cells[indiceColonna].Controls.Add(mainPanel);
                        }
                        else
                        {
                            if (IsPrimoGiorno(datoAgendaCorrente, DateTime.Parse(data)) && !IsUltimoGiorno(datoAgendaCorrente, DateTime.Parse(data)))
                            {
                                if (IsViaggioAndata(datoAgendaCorrente, DateTime.Parse(data)))
                                {
                                    colore = coloreViaggio;
                                }

                                Panel mainPanel = new Panel();

                                if (IsPrimoGiornoLavorazione(datoAgendaCorrente, DateTime.Parse(data)))
                                {
                                    mainPanel.Controls.Add(new LiteralControl(titoloEvento));
                                }
                                else
                                {
                                    mainPanel.Controls.Add(new LiteralControl("&nbsp;"));
                                }
                                
                                mainPanel.CssClass = "round-corners-6px unround-bottom-corners w3-tooltip";
                                mainPanel.Attributes.Add("style", "border: 2px solid " + colore + "; background-color:" + colore + ";color:" + coloreFont);
                                mainPanel.Controls.Add(AnteprimaEvento(datoAgendaCorrente));

                                e.Row.Cells[indiceColonna].Controls.Add(mainPanel);
                                e.Row.Cells[indiceColonna].Attributes.Add("style", "border-bottom: 0px; vertical-align: bottom");
                            }
                            else if (!IsPrimoGiorno(datoAgendaCorrente, DateTime.Parse(data)) && IsUltimoGiorno(datoAgendaCorrente, DateTime.Parse(data)))
                            {
                                if (IsViaggioRitorno(datoAgendaCorrente, DateTime.Parse(data)))
                                {
                                    colore = coloreViaggio;
                                }

                                Panel mainPanel = new Panel();

                                if (IsPrimoGiornoLavorazione(datoAgendaCorrente, DateTime.Parse(data)))
                                {
                                    mainPanel.Controls.Add(new LiteralControl(titoloEvento));
                                }
                                else
                                {
                                    mainPanel.Controls.Add(new LiteralControl("&nbsp;"));
                                }
                                
                                mainPanel.CssClass = "round-corners-6px unround-top-corners w3-tooltip";
                                mainPanel.Attributes.Add("style", "border: 2px solid " + colore + "; background-color:" + colore + ";color:" + coloreFont);
                                mainPanel.Controls.Add(AnteprimaEvento(datoAgendaCorrente));

                                e.Row.Cells[indiceColonna].Controls.Add(mainPanel);
                                e.Row.Cells[indiceColonna].Attributes.Add("style", "border-top: 0px; vertical-align: top");                                
                            }
                            else if (!IsPrimoGiorno(datoAgendaCorrente, DateTime.Parse(data)) && !IsUltimoGiorno(datoAgendaCorrente, DateTime.Parse(data)))
                            {
                                if (IsViaggioAndata(datoAgendaCorrente, DateTime.Parse(data)) || IsViaggioRitorno(datoAgendaCorrente, DateTime.Parse(data)))
                                {
                                    colore = coloreViaggio;
                                }

                                Panel mainPanel = new Panel();

                                if (IsPrimoGiornoLavorazione(datoAgendaCorrente, DateTime.Parse(data)))
                                {
                                    mainPanel.Controls.Add(new LiteralControl(titoloEvento));
                                }
                                else
                                {
                                    mainPanel.Controls.Add(new LiteralControl("&nbsp;"));
                                }
                                
                                mainPanel.CssClass = "w3-tooltip";
                                mainPanel.Attributes.Add("style", "border: 2px solid " + colore + "; background-color:" + colore + ";color:" + coloreFont);
                                mainPanel.Controls.Add(AnteprimaEvento(datoAgendaCorrente));
                                e.Row.Cells[indiceColonna].Controls.Add(mainPanel);
                                e.Row.Cells[indiceColonna].Attributes.Add("style", "border-top: 0px; border-bottom: 0px; background-color:" + colore + ";color:" + coloreFont);
                            }
                        }
                    }
                    if (isUtenteAbilitatoInScrittura)
                    {
                        e.Row.Cells[indiceColonna].Attributes["ondblclick"] = "mostracella('" + data + "', '" + id_risorsa + "');";
                    }
                }
            }
            #endregion
        }

        protected void gvArticoli_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label totaleRiga = (Label)e.Row.FindControl("totaleRiga");
                totaleRiga.Text = string.Format("{0:0.00}", (int.Parse(e.Row.Cells[2].Text) * int.Parse(e.Row.Cells[3].Text)));

                e.Row.Cells[3].Text = string.Format("{0:0.00}", (int.Parse(e.Row.Cells[3].Text)));
                e.Row.Cells[4].Text = string.Format("{0:0.00}", (int.Parse(e.Row.Cells[4].Text)));
            }
        }
            private void AggiornaAgenda()
        {
            //listaDatiAgenda = (List<DatiAgenda>)ViewState["listaDatiAgenda"]; //CARICO TUTTI GLI EVENTI

            //CARICO SOLO GLI EVENTI VISUALIZZATI
            Esito esito = new Esito();
            listaDatiAgenda = Agenda_BLL.Instance.CaricaDatiAgenda(DateTime.Parse(hf_valoreData.Value), ref esito);

            ViewState["listaDatiAgenda"] = listaDatiAgenda;

            gv_scheduler.DataSource = CreateDataTable(DateTime.Parse(hf_valoreData.Value));
            gv_scheduler.DataBind();

            ScriptManager.RegisterStartupScript(this, typeof(Page), "passaggioMouse", "registraPassaggioMouse();", true);
        }

        private bool IsDisponibileDataRisorsa(DatiAgenda eventoDaControllare)
        {
            listaDatiAgenda = (List<DatiAgenda>)ViewState["listaDatiAgenda"];
            DatiAgenda eventoEsistente = listaDatiAgenda.FirstOrDefault(x => x.id != eventoDaControllare.id &&
                                                         x.id_colonne_agenda == eventoDaControllare.id_colonne_agenda &&
                                                        ((x.data_inizio_impegno <= eventoDaControllare.data_inizio_impegno && x.data_fine_impegno >= eventoDaControllare.data_inizio_impegno) ||
                                                        (x.data_inizio_impegno <= eventoDaControllare.data_fine_impegno && x.data_fine_impegno >= eventoDaControllare.data_fine_impegno) ||
                                                        (x.data_inizio_impegno >= eventoDaControllare.data_inizio_impegno && x.data_fine_impegno <= eventoDaControllare.data_fine_impegno)
                                                        ));

            return eventoEsistente == null;
        }
       
        #endregion
        
        #region OPERAZIONI POPUP
        public void OperazioniPopup(string operazione)
        {
            switch (operazione)
            {
                case "SHOW":
                    break;
                case "UPDATE":
                    UpdatePopup();
                    break;
                case "CLOSE":
                    ChiudiPopup();
                    break;
            }
        }

        private void UpdatePopup()
        {
            DatiAgenda eventoSelezionato = (DatiAgenda)ViewState["eventoSelezionato"];
            popupAppuntamento.CreaOggettoSalvataggio(ref eventoSelezionato);
            popupAppuntamento.PopolaPopup(eventoSelezionato);
            AbilitaComponentiPopup();

            upEvento.Update();
        }

        private void AbilitaComponentiPopup()
        {
            Esito esito = new Esito();
            DatiAgenda eventoSelezionato = (DatiAgenda)ViewState["eventoSelezionato"];
            string sottotipoRisorsa = "";

            if (eventoSelezionato != null)
            {
                sottotipoRisorsa = UtilityTipologiche.getElementByID(listaRisorse, eventoSelezionato.id_colonne_agenda, ref esito).sottotipo.ToUpper();

                switch (eventoSelezionato.id_stato)
                {
                    case DatiAgenda.STATO_PREVISIONE_IMPEGNO:
                        tab_Offerta.Attributes["onclick"] = "return false;";
                        tab_Offerta.Style.Add("cursor", "not-allowed;");

                        tab_Lavorazione.Attributes["onclick"] = "return false;";
                        tab_Lavorazione.Style.Add("cursor", "not-allowed;");

                        btnOfferta.Visible = sottotipoRisorsa != EnumSottotipiRisorse.DIPENDENTI.ToString();
                        btnLavorazione.Visible = false;
                        btnElimina.Visible = eventoSelezionato.id != 0;
                        btnSalva.Visible = true;
                        btnRiepilogo.Visible = false;

                        popupAppuntamento.AbilitaComponentiPopup(DatiAgenda.STATO_PREVISIONE_IMPEGNO);

                        break;
                    case DatiAgenda.STATO_OFFERTA:
                        tab_Offerta.Attributes["onclick"] = "openTabEvento(event, 'Offerta');";
                        tab_Offerta.Style.Remove("cursor");

                        tab_Lavorazione.Attributes["onclick"] = "return false;";
                        tab_Lavorazione.Style.Add("cursor", "not-allowed;");

                        btnOfferta.Visible = false;
                        btnSalva.Visible = false;
                        btnRiepilogo.Visible = true;
                        btnLavorazione.Visible = sottotipoRisorsa != EnumSottotipiRisorse.DIPENDENTI.ToString();
                        btnElimina.Visible = true;
                       

                        popupAppuntamento.AbilitaComponentiPopup(DatiAgenda.STATO_OFFERTA);

                        break;
                    case DatiAgenda.STATO_LAVORAZIONE:
                        tab_Offerta.Attributes["onclick"] = "openTabEvento(event, 'Offerta');";
                        tab_Offerta.Style.Remove("cursor");

                        tab_Lavorazione.Attributes["onclick"] = "openTabEvento(event, 'Lavorazione');";
                        tab_Lavorazione.Style.Remove("cursor");

                        btnOfferta.Visible = false;
                        btnLavorazione.Visible = false;
                        btnElimina.Visible = false;
                        btnSalva.Visible = true;
                        btnRiepilogo.Visible = false;

                        popupAppuntamento.AbilitaComponentiPopup(DatiAgenda.STATO_LAVORAZIONE);

                        break;
                    case DatiAgenda.STATO_FATTURA:
                        tab_Offerta.Attributes["onclick"] = "openTabEvento(event, 'Offerta');";
                        tab_Offerta.Style.Remove("cursor");

                        tab_Lavorazione.Attributes["onclick"] = "openTabEvento(event, 'Lavorazione');";
                        tab_Lavorazione.Style.Remove("cursor");

                        btnOfferta.Visible = false;
                        btnLavorazione.Visible = false;
                        btnElimina.Visible = false;
                        btnSalva.Visible = true;
                        btnRiepilogo.Visible = false;

                        popupAppuntamento.AbilitaComponentiPopup(DatiAgenda.STATO_FATTURA);

                        break;
                    case DatiAgenda.STATO_RIPOSO:
                        tab_Offerta.Attributes["onclick"] = "return false;";
                        tab_Offerta.Style.Add("cursor", "not-allowed;");

                        tab_Lavorazione.Attributes["onclick"] = "return false;";
                        tab_Lavorazione.Style.Add("cursor", "not-allowed;");

                        btnOfferta.Visible = false;
                        btnLavorazione.Visible = false;
                        btnElimina.Visible = eventoSelezionato.id != 0;
                        btnSalva.Visible = true;
                        btnRiepilogo.Visible = false;

                        popupAppuntamento.AbilitaComponentiPopup(DatiAgenda.STATO_RIPOSO);

                        break;
                }
            }
        }

        private void MostraPopup(DatiAgenda eventoSelezionato)
        {
            pnlContainer.Style.Remove("display");

            //panelErrore.Style.Add("display", "none");
            //lbl_MessaggioErrore.Text = string.Empty;

            popupAppuntamento.ClearAppuntamento();
            popupAppuntamento.PopolaPopup(eventoSelezionato);

            popupOfferta.ClearOfferta();
            popupOfferta.PopolaOfferta(eventoSelezionato.id);
        }

        private void ChiudiPopup()
        {
            pnlContainer.Style.Add("display", "none");
            UpdatePopup();
            ScriptManager.RegisterStartupScript(this, typeof(Page), "aggiornaAgenda", "aggiornaAgenda();", true);
        }
        #endregion

        #region OPERAZIONI EVENTO
        private DatiAgenda CreaEventoDaSelezioneAgenda(DateTime dataEvento, int risorsaEvento)
        {
            Esito esito = new Esito();
            listaDatiAgenda = (List<DatiAgenda>)ViewState["listaDatiAgenda"];

            DatiAgenda eventoSelezionato = Agenda_BLL.Instance.GetDatiAgendaByDataRisorsa(listaDatiAgenda, dataEvento, risorsaEvento);
            string sottotipoRisorsa = UtilityTipologiche.getElementByID(listaRisorse, risorsaEvento, ref esito).sottotipo.ToUpper();

            if (eventoSelezionato == null)
            {
                eventoSelezionato = new DatiAgenda();

                eventoSelezionato.data_inizio_lavorazione = dataEvento;
                eventoSelezionato.data_fine_lavorazione = dataEvento;
                eventoSelezionato.durata_lavorazione = 1;
                eventoSelezionato.id_colonne_agenda = risorsaEvento;
                eventoSelezionato.data_inizio_impegno = dataEvento;
                eventoSelezionato.data_fine_impegno = dataEvento;
                eventoSelezionato.durata_viaggio_andata = 0;
                eventoSelezionato.durata_viaggio_ritorno = 0;
                eventoSelezionato.id_stato = sottotipoRisorsa == EnumSottotipiRisorse.DIPENDENTI.ToString() ? DatiAgenda.STATO_RIPOSO : DatiAgenda.STATO_PREVISIONE_IMPEGNO;
            }

            ViewState["eventoSelezionato"] = eventoSelezionato;

            return eventoSelezionato;
        }

        private Esito SalvaEvento()
        {
            //panelErrore.Style.Add("display", "none");
            popupAppuntamento.NascondiErroriValidazione();

            Esito esito = new Esito();
            DatiAgenda eventoSelezionato = (DatiAgenda)ViewState["eventoSelezionato"];
            List<DatiArticoli> listaDatiArticoli = popupOfferta.listaDatiArticoli;

            List<string> listaIdTender = popupAppuntamento.listaIdTender;

            esito = ValidazioneSalvataggio(eventoSelezionato, listaDatiArticoli);

            if (esito.codice == Esito.ESITO_OK)
            {
                if (eventoSelezionato.id == 0)
                {          
                    Agenda_BLL.Instance.CreaEvento(eventoSelezionato, listaDatiArticoli, listaIdTender);
                }
                else
                {
                    Agenda_BLL.Instance.AggiornaEvento(eventoSelezionato, listaDatiArticoli, listaIdTender);
                }

                ViewState["listaDatiAgenda"] = Agenda_BLL.Instance.CaricaDatiAgenda(DateTime.Parse(hf_valoreData.Value), ref esito);
            }
            else
            {
                popupAppuntamento.PopolaPopup(eventoSelezionato);
            }

            return esito;
        }

        private Esito ValidazioneSalvataggio(DatiAgenda eventoSelezionato, List<DatiArticoli> listaDatiArticoli)
        {
            Esito esito = new Esito();
            esito = popupAppuntamento.CreaOggettoSalvataggio(ref eventoSelezionato);

            if (esito.codice != Esito.ESITO_OK)
            {
                esito.descrizione = "Controllare i campi evidenziati";

            }
            else if (!IsDisponibileDataRisorsa(eventoSelezionato))
            {
                esito.codice = Esito.ESITO_KO_ERRORE_VALIDAZIONE;
                esito.descrizione = "Non è possibile salvare perché la risorsa è già impiegata nel periodo selezionato";
            }
            else if (eventoSelezionato.id_stato == DatiAgenda.STATO_OFFERTA && (listaDatiArticoli==null || listaDatiArticoli.Count==0))
            {
                esito.codice = Esito.ESITO_KO_ERRORE_VALIDAZIONE;
                esito.descrizione = "Non è possibile salvare senza aver associato gli articoli";
            }

            return esito;
        }

        private bool IsPrimoGiorno(DatiAgenda evento, DateTime data)
        {
            return evento.data_inizio_impegno.Date == data.Date;
        }

        private bool IsUltimoGiorno(DatiAgenda evento, DateTime data)
        {
            return evento.data_fine_impegno.Date == data.Date;
        }

        private bool IsViaggioAndata(DatiAgenda evento, DateTime data)
        {
            if (evento.durata_viaggio_andata > 0)
            {
                return (data.Date >= evento.data_inizio_impegno.Date && data.Date < evento.data_inizio_lavorazione.Date);
            }

            return false;
        }

        private bool IsViaggioRitorno(DatiAgenda evento, DateTime data)
        {
            if (evento.durata_viaggio_ritorno > 0)
            {
                return (data.Date > evento.data_fine_lavorazione.Date && data <= evento.data_fine_impegno.Date);
            }

            return false;
        }

        private bool IsPrimoGiornoLavorazione(DatiAgenda evento, DateTime data)
        {
            return evento.data_inizio_lavorazione.Date == data.Date;
        }
        #endregion

        #region COSTRUZIONE PAGINA
        private string CreaLegenda()
        {
            Esito esito = new Esito();

            string legenda = "<ul style='list-style-type: none;padding-left:0px;'>";

            foreach (Tipologica stato in listaStati)
            {
                string colore = UtilityTipologiche.getParametroDaTipologica(stato, "COLOR", ref esito);

                legenda += "<li><div class='boxLegenda' style='background:" + colore + "'/>&nbsp;</div>&nbsp;- " + stato.nome + "</li>";
            }

            legenda += "</ul>";

            return legenda;
        }

        private string CreaFiltriColonneAgenda()
        {
            List<Tipologica> listaSottotipiColonne = listaRisorse.GroupBy(x => x.sottotipo).Select(x => x.FirstOrDefault()).ToList<Tipologica>();

            string check = "";
            foreach (Tipologica colonna in listaSottotipiColonne)
            {
                check += "<div class='checkbox'><label><input type='checkbox' class='filtroColonna' value='" + colonna.sottotipo + "' checked onchange=\"filtraColonna(this,'" + colonna.sottotipo + "');\">&nbsp;" + colonna.sottotipo + "</label></div>";
            }

            return check;
        }

        private Panel AnteprimaEvento(DatiAgenda evento)
        {
            Panel innerPanel = new Panel();
            if (evento.id_stato != DatiAgenda.STATO_RIPOSO)
            {
                Esito esito = new Esito();

                string cliente = "";
                if (evento.id_cliente != 0)
                {
                    cliente = Anag_Clienti_Fornitori_BLL.Instance.getAziendaById(evento.id_cliente, ref esito).RagioneSociale;
                }
                string stato = listaStati.Where(x => x.id == evento.id_stato).FirstOrDefault().nome;
                string tipologia = listaTipiTipologie.Where(x => x.id == evento.id_tipologia).FirstOrDefault().nome;
                string produzione = evento.produzione;
                string dataInizio = evento.data_inizio_lavorazione.ToString("dd/MM/yyyy");
                string datatFine = evento.data_fine_lavorazione.ToString("dd/MM/yyyy");
                string lavorazione = evento.lavorazione;
                string luogo = evento.luogo;

                string contenuto = "<p style='text-align:center;font-weight:bold; background-color:white'>Anteprima appuntamento</p><p style='text-align:left;font-weight:normal;background-color:white'><b>Tipo impegno:</b> " + stato + "<br/><b>Cliente:</b> " + cliente + "<br/><b>Lavorazione:</b> " + lavorazione + "<br/><b>Produzione:</b> " + produzione + "<br/><b>Tipologia:</b> " + tipologia + "<br/><b>Data inizio:</b> " + dataInizio + "&nbsp;&nbsp;<b>Data fine:</b> " + datatFine + "<br/><b>Luogo:</b> " + luogo + "</p>";
                LiteralControl anteprima = new LiteralControl(contenuto);

                innerPanel.Controls.Add(anteprima);
                innerPanel.CssClass = "round-corners-6px w3-text innerPanel";
                
            }
            return innerPanel;
        }
        #endregion
    }
}