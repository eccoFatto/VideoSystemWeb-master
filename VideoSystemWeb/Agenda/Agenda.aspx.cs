using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoSystemWeb.BLL;
using VideoSystemWeb.BLL.Stampa;
using VideoSystemWeb.Entity;

namespace VideoSystemWeb.Agenda
{
    public partial class Agenda : BasePage
    {
        ObjectIDGenerator IDGenerator = new ObjectIDGenerator();

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        bool isUtenteAbilitatoInScrittura;
        string coloreViaggio;

        #region ELENCO CHIAVI VIEWSTATE
        private const string VIEWSTATE_LISTADATIAGENDA = "listaDatiAgenda"; 
        #endregion

        public List<DatiAgenda> ListaDatiAgenda
        {
            get
            {
                return (List<DatiAgenda>)ViewState[VIEWSTATE_LISTADATIAGENDA];
            }
            set
            {
                ViewState[VIEWSTATE_LISTADATIAGENDA] = value;
            }
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            CheckIsMobile();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Esito esito = new Esito();

            popupAppuntamento.RichiediOperazionePopup += OperazioniPopup;
            popupOfferta.RichiediOperazionePopup += OperazioniPopup;

            popupLavorazione.RichiediOperazionePopup += OperazioniPopup;

            popupRiepilogoOfferta.RichiediListaArticoli += GetListaArticoli;

            isUtenteAbilitatoInScrittura = AbilitazioneInScrittura();
            Tipologica viaggio  = UtilityTipologiche.getElementByID(SessionManager.ListaStati, Stato.Instance.STATO_VIAGGIO, ref esito);
            coloreViaggio = UtilityTipologiche.getParametroDaTipologica(viaggio, "color", ref esito);

            if (!IsPostBack)
            {
                DateTime dataPartenza = DateTime.Now;

                ListaDatiAgenda = Agenda_BLL.Instance.CaricaDatiAgenda(dataPartenza, ref esito); //CARICO SOLO EVENTI VISUALIZZATI

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

            SessionManager.EventoSelezionato = CreaEventoDaSelezioneAgenda(dataEvento, risorsaEvento);

            if (SessionManager.EventoSelezionato.id_stato == Stato.Instance.STATO_PREVISIONE_IMPEGNO)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "apritab", "openTabEvento(event,'Appuntamento');", true);
            }
            else if (SessionManager.EventoSelezionato.id_stato == Stato.Instance.STATO_OFFERTA)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "apritab", "openTabEvento(event,'Offerta');", true);
            }
            else if (SessionManager.EventoSelezionato.id_stato == Stato.Instance.STATO_LAVORAZIONE)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "apritab", "openTabEvento(event,'Lavorazione');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "apritab", "openTabEvento(event,'Appuntamento');", true);
            }

            AbilitaComponentiPopup();

            MostraPopup();
        }

        protected void btnSalva_Click(object sender, EventArgs e)
        {
            Esito esito = SalvaEvento();
            if (esito.Codice == Esito.ESITO_OK)
            {
               // ChiudiPopup();
               // ShowSuccess("Salvataggio eseguito correttamente");
            }
            else
            {                
                UpdatePopup();
            }
        }

        protected void btnRiepilogo_Click(object sender, EventArgs e)
        {
            Esito esito = SalvaEvento(); // l'apertura del riepilogo comporta il salvataggio dell'offerta per la generazione del protocollo
            esito = popupRiepilogoOfferta.popolaPannelloRiepilogo(SessionManager.EventoSelezionato);

            upRiepilogoOfferta.Update();

            if (esito.Codice == Esito.ESITO_OK)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "aggiornaAgenda", "aggiornaAgenda();", true);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriRiepilogo", script: "javascript: document.getElementById('modalRiepilogoOfferta').style.display='block'", addScriptTags: true);

                //temporaneamente eliminato perché è una gran rottura di cazzo
                //ShowSuccess("L'evento è stato salvato automaticamente");
            }
            else
            {
                UpdatePopup();
            }
        }

        protected void btnStampaPianoEsterno_Click(object sender, EventArgs e)
        {
            Esito esito = SalvaEvento();
            if (esito.Codice == Esito.ESITO_OK)
            {
                esito = popupRiepilogoPianoEsterno.popolaPannelloPianoEsterno(SessionManager.EventoSelezionato);
                if (esito.Codice == Esito.ESITO_OK)
                {
                    upRiepilogoPianoEsterno.Update();

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "aggiornaAgenda", "aggiornaAgenda();", true);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriPianoEsterno", script: "javascript: document.getElementById('modalPianoEsterno').style.display='block'", addScriptTags: true);
                }
                else
                {
                    ShowError(esito.Descrizione);
                }
            }
            else
            {
                ShowError(esito.Descrizione);
                UpdatePopup();
            }


        }

        protected void btnElimina_Click(object sender, EventArgs e)
        {
            Esito esito = new Esito();
            if (SessionManager.EventoSelezionato.id_stato == Stato.Instance.STATO_LAVORAZIONE) // Torna allo stato Offerta
            {

                SessionManager.EventoSelezionato.id_stato = Stato.Instance.STATO_OFFERTA;
                esito = Agenda_BLL.Instance.EliminaLavorazione(SessionManager.EventoSelezionato);

                if (esito.Codice == Esito.ESITO_OK)
                {
                    esito = SalvaEvento();
                    if (esito.Codice == Esito.ESITO_OK)
                    {
                        btnLavorazione.Visible = true;
                        UpdatePopup();

                        ShowSuccess("La lavorazione è stata eliminata e l'evento è stato riportato allo stato Offerta");


                        ScriptManager.RegisterStartupScript(this, typeof(Page), "passaALavorazione", "openTabEvento(event,'Offerta');", true);
                    }
                    else
                    {
                        UpdatePopup();
                        ShowError(esito.Descrizione);
                    }

                    val_Stato.Text = UtilityTipologiche.getElementByID(SessionManager.ListaStati, SessionManager.EventoSelezionato.id_stato, ref esito).nome;
                    val_CodiceLavoro.Text = SessionManager.EventoSelezionato.codice_lavoro;
                }
                else
                {
                    ShowError(esito.Descrizione);
                }
            }
            else
            {
                esito = Agenda_BLL.Instance.EliminaEvento(SessionManager.EventoSelezionato.id);

                if (esito.Codice == Esito.ESITO_OK)
                {
                    ChiudiPopup();
                    ShowSuccess("Eliminazione eseguita correttamente");
                }
                else
                {
                    ShowError(esito.Descrizione);
                    UpdatePopup();
                }
            }
        }

        protected void btn_chiudi_Click(object sender, EventArgs e)
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["DEBUG_MODE"]))
            {
                ChiudiPopup();
            }
            else
            {
                Esito esito = SalvaEvento();
                if (esito.Codice == Esito.ESITO_OK)
                {
                    ChiudiPopup();

                    //ShowSuccess("Salvataggio eseguito correttamente");
                }
                else
                {
                    UpdatePopup();
                }
            }


            //if (hf_Salvataggio.Value == "1")
            //{
            //    Esito esito = SalvaEvento();
            //    if (esito.Codice == Esito.ESITO_OK)
            //    {
            //        ChiudiPopup();

            //        //ShowSuccess("Salvataggio eseguito correttamente");
            //    }
            //    else
            //    {
            //        UpdatePopup();
            //    }
            //}
            //else
            //{
            //    ChiudiPopup();
            //}
        }

        protected void btnOfferta_Click(object sender, EventArgs e)
        {
            Esito esito = new Esito();

            List<string> listaIdTender = popupAppuntamento.ListaIdTender;

            esito = ValidazioneSalvataggio( listaIdTender);
            if (esito.Codice == Esito.ESITO_OK)
            {
                popupOfferta.TrasformaInOfferta();

                val_Stato.Text = UtilityTipologiche.getElementByID(SessionManager.ListaStati, SessionManager.EventoSelezionato.id_stato, ref esito).nome;
                val_CodiceLavoro.Text = SessionManager.EventoSelezionato.codice_lavoro;

                btnLavorazione.Visible = false;
                UpdatePopup();
                ScriptManager.RegisterStartupScript(this, typeof(Page), "passaAOfferta", "openTabEvento(event,'Offerta');", true);
            }
            else
            {
                ShowWarning(esito.Descrizione);
                popupAppuntamento.PopolaAppuntamento();
                UpdatePopup();
            }
        }

        protected void btnLavorazione_Click(object sender, EventArgs e)
        {
            Esito esito = new Esito();
            List<string> listaIdTender = popupAppuntamento.ListaIdTender;

            esito = ValidazioneSalvataggio(listaIdTender);

            if (esito.Codice == Esito.ESITO_OK)
            {
                popupLavorazione.TrasformaInLavorazione();

                val_Stato.Text = UtilityTipologiche.getElementByID(SessionManager.ListaStati, SessionManager.EventoSelezionato.id_stato, ref esito).nome;
                val_CodiceLavoro.Text = SessionManager.EventoSelezionato.codice_lavoro;

                RiempiCampiIntestazioneEvento();

                btnLavorazione.Visible = false;
                UpdatePopup();
                ScriptManager.RegisterStartupScript(this, typeof(Page), "passaALavorazione", "openTabEvento(event,'Lavorazione');", true);
            }
            else
            {
                ShowWarning(esito.Descrizione);
                popupAppuntamento.PopolaAppuntamento();
                UpdatePopup();
            }
            
        }

        protected void gv_scheduler_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Esito esito = new Esito();
            e.Row.Cells[0].Attributes.Add("class", "first");

            #region intestazione tabella
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int indiceColonna = 1; indiceColonna <= SessionManager.ListaRisorse.Count; indiceColonna++)
                {
                    string idRisorsa = (e.Row.Cells[indiceColonna].Text.Trim());
                    ColonneAgenda risorsaCorrente = UtilityTipologiche.getElementByID(SessionManager.ListaRisorse, int.Parse(idRisorsa), ref esito);

                    string colore = UtilityTipologiche.getParametroDaTipologica(risorsaCorrente, "color", ref esito);

                    e.Row.Cells[indiceColonna].Attributes.Add("class", risorsaCorrente.sottotipo);
                    e.Row.Cells[indiceColonna].Attributes.Add("style", "background-color:" + colore + ";font-size:9pt;text-align:center;width:100px;");
                    e.Row.Cells[indiceColonna].Text = risorsaCorrente.nome;
                }
            }
            #endregion

            #region dati agenda
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Attributes.Add("class", "first");

                //if ((int)DateTime.Parse(e.Row.Cells[0].Text).DayOfWeek == 0 || (int)DateTime.Parse(e.Row.Cells[0].Text).DayOfWeek == 6)
                if ((int)DateTime.Parse(e.Row.Cells[0].Text.Substring(6)).DayOfWeek == 0 || (int)DateTime.Parse(e.Row.Cells[0].Text.Substring(6)).DayOfWeek == 6)
                {
                    e.Row.Cells[0].Attributes.Add("class", "first festivo");
                }

                for (int indiceColonna = 1; indiceColonna <= SessionManager.ListaRisorse.Count; indiceColonna++)
                {
                    string data = e.Row.Cells[0].Text.Substring(6);
                    ColonneAgenda risorsa = SessionManager.ListaRisorse.ElementAt(indiceColonna - 1);
                    int id_risorsa = risorsa.id;// ((ColonneAgenda)listaRisorse.ElementAt(indiceColonna - 1)).id;

                    e.Row.Cells[indiceColonna].Attributes.Add("class", risorsa.sottotipo);

                    if (!string.IsNullOrEmpty(e.Row.Cells[indiceColonna].Text.Trim()))
                    {
                        DatiAgenda datoAgendaCorrente = Agenda_BLL.Instance.GetDatiAgendaById(ListaDatiAgenda, int.Parse(e.Row.Cells[indiceColonna].Text.Trim()));
                        Tipologica statoCorrente = UtilityTipologiche.getElementByID(SessionManager.ListaStati, datoAgendaCorrente.id_stato, ref esito);

                        #region COLORE EVENTO
                        string colore;
                        colore = UtilityTipologiche.getParametroDaTipologica(statoCorrente, "color", ref esito);
                        string coloreFont;
                        coloreFont = UtilityTipologiche.getParametroDaTipologica(statoCorrente, "font_color", ref esito);
                        #endregion

                        e.Row.Cells[indiceColonna].CssClass = "cellaEvento " + risorsa.sottotipo;

                        #region TITOLO EVENTO
                        string tipologia = "";
                        if (datoAgendaCorrente.id_tipologia != null && datoAgendaCorrente.id_tipologia != 0)
                        {
                            tipologia = UtilityTipologiche.getTipologicaById(EnumTipologiche.TIPO_TIPOLOGIE, (int)datoAgendaCorrente.id_tipologia, ref esito).nome;
                        }
                        string titoloEvento = datoAgendaCorrente.id_stato == Stato.Instance.STATO_RIPOSO ? "Riposo" : "<div class='titoloEvento'>"+datoAgendaCorrente.lavorazione + "</div><div class='titoloEvento'>" + tipologia+"</div>";
                        #endregion

                        // EVENTO GIORNO SINGOLO
                        if (IsPrimoGiorno(datoAgendaCorrente, DateTime.Parse(data)) && IsUltimoGiorno(datoAgendaCorrente, DateTime.Parse(data)))
                        {
                            Panel mainPanel = new Panel();
                            mainPanel.Controls.Add(new LiteralControl(titoloEvento));
                            mainPanel.CssClass = "round-corners-6px w3-tooltip";
                            mainPanel.Attributes.Add("style", "border: 2px solid " + colore + "; background-color:" + colore + "; color:" + coloreFont);

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
                                mainPanel.Attributes.Add("style", "border: 2px solid " + colore + "; background-color:" + colore + "; color:" + coloreFont);
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
                                mainPanel.Attributes.Add("style", "border: 2px solid " + colore + "; background-color:" + colore + "; color:" + coloreFont);
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
                                mainPanel.Attributes.Add("style", "border: 2px solid " + colore + "; background-color:" + colore + "; color:" + coloreFont);
                                mainPanel.Controls.Add(AnteprimaEvento(datoAgendaCorrente));
                                e.Row.Cells[indiceColonna].Controls.Add(mainPanel);
                                e.Row.Cells[indiceColonna].Attributes.Add("style", "border-top: 0px; border-bottom: 0px; background-color:" + colore + "; color:" + coloreFont);
                            }
                        }
                    }
                    if (isUtenteAbilitatoInScrittura)
                    {
                        e.Row.Cells[indiceColonna].Attributes["ondblclick"] = "mostracella('" + data + "', '" + id_risorsa + "');";
                        e.Row.Cells[indiceColonna].Attributes["oncontextmenu"] = "mostracella('" + data + "', '" + id_risorsa + "');return false;";
                    }
                }
            }
            #endregion
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

            foreach (ColonneAgenda risorsa in SessionManager.ListaRisorse)
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

                row[0] = dataRiga.ToString("ddd - dd/MM/yyyy",new System.Globalization.CultureInfo("it-IT")); //dataRiga.ToString("dd/MM/yyyy");

                int indiceColonna = 1;
                foreach (ColonneAgenda risorsa in SessionManager.ListaRisorse)
                {
                    DatiAgenda datiAgendaFiltrati = Agenda_BLL.Instance.GetDatiAgendaByDataRisorsa(ListaDatiAgenda, dataRiga, risorsa.id);
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

        private void AggiornaAgenda()
        {
            //CARICO SOLO GLI EVENTI VISUALIZZATI
            Esito esito = new Esito();
            ListaDatiAgenda = Agenda_BLL.Instance.CaricaDatiAgenda(DateTime.Parse(hf_valoreData.Value), ref esito);

            gv_scheduler.DataSource = CreateDataTable(DateTime.Parse(hf_valoreData.Value));
            gv_scheduler.DataBind();

            ScriptManager.RegisterStartupScript(this, typeof(Page), "passaggioMouse", "registraPassaggioMouse();", true);
        }

        private string CreaLegenda()
        {
            Esito esito = new Esito();

            string legenda = "<ul style='list-style-type: none;padding-left:0px;'>";

            foreach (Tipologica stato in SessionManager.ListaStati)
            {
                string colore = UtilityTipologiche.getParametroDaTipologica(stato, "COLOR", ref esito);

                legenda += "<li><div class='boxLegenda' style='background:" + colore + "'/>&nbsp;</div>&nbsp;- " + stato.nome + "</li>";
            }

            legenda += "</ul>";

            return legenda;
        }

        private string CreaFiltriColonneAgenda()
        {
            List<ColonneAgenda> listaSottotipiColonne = SessionManager.ListaRisorse.GroupBy(x => x.sottotipo).Select(x => x.FirstOrDefault()).ToList<ColonneAgenda>();

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
            if (evento.id_stato != Stato.Instance.STATO_RIPOSO)
            {
                Esito esito = new Esito();

                string cliente = "";
                if (evento.id_cliente != 0)
                {
                    cliente = Anag_Clienti_Fornitori_BLL.Instance.getAziendaById(evento.id_cliente, ref esito).RagioneSociale;
                }
                string stato = SessionManager.ListaStati.Where(x => x.id == evento.id_stato).FirstOrDefault().nome;
                string tipologia = SessionManager.ListaTipiTipologie.Where(x => x.id == evento.id_tipologia).FirstOrDefault().nome;
                string produzione = evento.produzione;
                string dataInizio = evento.data_inizio_lavorazione.ToString("dd/MM/yyyy");
                string datatFine = evento.data_fine_lavorazione.ToString("dd/MM/yyyy");
                string lavorazione = evento.lavorazione;
                string luogo = evento.luogo;

                string tender = "-";
                List<DatiTender> listaTenderEvento = Dati_Tender_BLL.Instance.getDatiAgendaTenderByIdAgenda(evento.id, ref esito);
                if (listaTenderEvento != null && listaTenderEvento.Count > 0)
                {
                    tender = "";
                    foreach (DatiTender tenderCurr in listaTenderEvento)
                    {
                        string nomeTenderCurr = SessionManager.ListaTender.Where(x => x.id == tenderCurr.IdTender).Select(y => y.nome).FirstOrDefault();
                        tender += nomeTenderCurr + "; ";
                    }
                    tender = tender.Substring(0, tender.Length - 2);
                }

                string intestazioneAnteprima = "Anteprima appuntamento";
                if (!string.IsNullOrEmpty(evento.codice_lavoro))
                {
                    intestazioneAnteprima = "Codice lavoro: " + evento.codice_lavoro;
                }

                string contenuto = "<p style='text-align:center;font-weight:bold; background-color:white;color:black;'>" + intestazioneAnteprima + "</p><p style='text-align:left;font-weight:normal;background-color:white;color:black;'><b>Tipo impegno:</b> " + stato + "<br/><b>Cliente:</b> " + cliente + "<br/><b>Lavorazione:</b> " + lavorazione + "<br/><b>Produzione:</b> " + produzione + "<br/><b>Tipologia:</b> " + tipologia + "<br/><b>Data inizio:</b> " + dataInizio + "&nbsp;&nbsp;<b>Data fine:</b> " + datatFine + "<br/><b>Luogo:</b> " + luogo + "<br/><b>Tender:</b> " + tender + "</p>";
                LiteralControl anteprima = new LiteralControl(contenuto);

                innerPanel.Controls.Add(anteprima);
                innerPanel.CssClass = "round-corners-6px w3-text innerPanel";

            }
            return innerPanel;
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
                case "SAVE_PDF_OFFERTA":
                    //SalvaPdfOffertaSuFile();
                    break;
                case "SAVE_PDFCONSUNTIVO":
                    //SalvaPdfConsuntivoSuFile();
                    break;
            }
        }     

        private void UpdatePopup()
        {
            DatiAgenda _eventoSelezionato = SessionManager.EventoSelezionato;
            popupAppuntamento.CreaOggettoSalvataggio(ref _eventoSelezionato);
            SessionManager.EventoSelezionato = _eventoSelezionato;
            popupAppuntamento.PopolaAppuntamento();
            AbilitaComponentiPopup();

            upEvento.Update();
        }

        private void AbilitaComponentiPopup()
        {
            Esito esito = new Esito();
            string sottotipoRisorsa = "";

            if (SessionManager.EventoSelezionato != null)
            {
                sottotipoRisorsa = UtilityTipologiche.getElementByID(SessionManager.ListaRisorse, SessionManager.EventoSelezionato.id_colonne_agenda, ref esito).sottotipo.ToUpper();

                if (SessionManager.EventoSelezionato.id_stato == Stato.Instance.STATO_PREVISIONE_IMPEGNO)
                {
                    tab_Offerta.Attributes["onclick"] = "return false;";
                    tab_Offerta.Style.Add("cursor", "not-allowed;");

                    tab_Lavorazione.Attributes["onclick"] = "return false;";
                    tab_Lavorazione.Style.Add("cursor", "not-allowed;");

                    btnOfferta.Visible = sottotipoRisorsa != EnumSottotipiRisorse.DIPENDENTI.ToString();
                    btnLavorazione.Visible = false;
                    btnElimina.Visible = SessionManager.EventoSelezionato.id != 0;
                    btnRiepilogo.Visible = btnStampaPianoEsterno.Visible = btnStampaConsuntivo.Visible = btnStampaFattura.Visible = btnMagazzino.Visible = false;

                    popupAppuntamento.AbilitaComponentiPopup(Stato.Instance.STATO_PREVISIONE_IMPEGNO);
                }
                else if (SessionManager.EventoSelezionato.id_stato == Stato.Instance.STATO_OFFERTA)
                {
                    tab_Offerta.Attributes["onclick"] = "openTabEvento(event, 'Offerta');";
                    tab_Offerta.Style.Remove("cursor");

                    tab_Lavorazione.Attributes["onclick"] = "return false;";
                    tab_Lavorazione.Style.Add("cursor", "not-allowed;");

                    btnOfferta.Visible = false;
                    btnRiepilogo.Visible = true;
                    btnMagazzino.Visible = true;
                    btnLavorazione.Visible = sottotipoRisorsa != EnumSottotipiRisorse.DIPENDENTI.ToString();
                    btnElimina.Visible = true;
                    btnStampaPianoEsterno.Visible = false;
                    btnStampaConsuntivo.Visible = false;
                    btnStampaFattura.Visible = false;

                    popupAppuntamento.AbilitaComponentiPopup(Stato.Instance.STATO_OFFERTA);
                    popupOfferta.AbilitaComponentiPopup(Stato.Instance.STATO_OFFERTA);
                }
                else if (SessionManager.EventoSelezionato.id_stato == Stato.Instance.STATO_LAVORAZIONE)
                {
                    tab_Offerta.Attributes["onclick"] = "openTabEvento(event, 'Offerta');";
                    tab_Offerta.Style.Remove("cursor");

                    tab_Lavorazione.Attributes["onclick"] = "openTabEvento(event, 'Lavorazione');";
                    tab_Lavorazione.Style.Remove("cursor");

                    btnOfferta.Visible = false;
                    btnLavorazione.Visible = false;
                    btnElimina.Visible = true;
                    btnRiepilogo.Visible = btnStampaPianoEsterno.Visible = btnStampaConsuntivo.Visible = btnStampaFattura.Visible = btnMagazzino.Visible = true;

                    popupAppuntamento.AbilitaComponentiPopup(Stato.Instance.STATO_LAVORAZIONE);
                    popupOfferta.AbilitaComponentiPopup(Stato.Instance.STATO_LAVORAZIONE);
                }
                else if (SessionManager.EventoSelezionato.id_stato == Stato.Instance.STATO_FATTURA)
                {
                    tab_Offerta.Attributes["onclick"] = "openTabEvento(event, 'Offerta');";
                    tab_Offerta.Style.Remove("cursor");

                    tab_Lavorazione.Attributes["onclick"] = "openTabEvento(event, 'Lavorazione');";
                    tab_Lavorazione.Style.Remove("cursor");

                    btnOfferta.Visible = false;
                    btnLavorazione.Visible = false;
                    btnElimina.Visible = false;
                    btnRiepilogo.Visible = false;
                    btnMagazzino.Visible = false;

                    popupAppuntamento.AbilitaComponentiPopup(Stato.Instance.STATO_FATTURA);
                }
                else if (SessionManager.EventoSelezionato.id_stato == Stato.Instance.STATO_RIPOSO)
                {
                    tab_Offerta.Attributes["onclick"] = "return false;";
                    tab_Offerta.Style.Add("cursor", "not-allowed;");

                    tab_Lavorazione.Attributes["onclick"] = "return false;";
                    tab_Lavorazione.Style.Add("cursor", "not-allowed;");

                    btnOfferta.Visible = false;
                    btnLavorazione.Visible = false;
                    btnElimina.Visible = SessionManager.EventoSelezionato.id != 0;
                    btnRiepilogo.Visible = false;
                    btnMagazzino.Visible = false;

                    popupAppuntamento.AbilitaComponentiPopup(Stato.Instance.STATO_RIPOSO);
                }
            }
        }

        private void MostraPopup()
        {
            
            pnlContainer.Style.Remove("display");

            Esito esito = new Esito();   
            val_Stato.Text = UtilityTipologiche.getElementByID(SessionManager.ListaStati, SessionManager.EventoSelezionato.id_stato, ref esito).nome;
            val_CodiceLavoro.Text = string.IsNullOrEmpty(SessionManager.EventoSelezionato.codice_lavoro) ? "-" : SessionManager.EventoSelezionato.codice_lavoro;

            popupAppuntamento.ClearAppuntamento();
            popupAppuntamento.PopolaAppuntamento();

            popupOfferta.ClearOfferta();
            if (SessionManager.EventoSelezionato.id_stato == Stato.Instance.STATO_OFFERTA || SessionManager.EventoSelezionato.id_stato == Stato.Instance.STATO_LAVORAZIONE)
            {
                popupOfferta.PopolaOfferta();
            }

            popupLavorazione.ClearLavorazione();
            if (SessionManager.EventoSelezionato.id_stato == Stato.Instance.STATO_LAVORAZIONE)
            {
                popupLavorazione.PopolaLavorazione();
            }
            RiempiCampiIntestazioneEvento();
            
        }

        private void ChiudiPopup()
        {
            pnlContainer.Style.Add("display", "none");
            UpdatePopup();

            SessionManager.ClearEventoSelezionato();

            ScriptManager.RegisterStartupScript(this, typeof(Page), "aggiornaAgenda", "aggiornaAgenda();", true);
        }
        
        #endregion

        #region OPERAZIONI EVENTO
        // Crea evento selezionando una cella (può essere nuovo o no)
        private DatiAgenda CreaEventoDaSelezioneAgenda(DateTime dataEvento, int risorsaEvento)
        {
            Esito esito = new Esito();
            popupAppuntamento.ListaIdTender = null;

            SessionManager.EventoSelezionato = Agenda_BLL.Instance.GetDatiAgendaByDataRisorsa(ListaDatiAgenda, dataEvento, risorsaEvento);
            string sottotipoRisorsa = UtilityTipologiche.getElementByID(SessionManager.ListaRisorse, risorsaEvento, ref esito).sottotipo.ToUpper();

            if (SessionManager.EventoSelezionato == null)
            {
                SessionManager.EventoSelezionato = new DatiAgenda();

                SessionManager.EventoSelezionato.data_inizio_lavorazione = dataEvento;
                SessionManager.EventoSelezionato.data_fine_lavorazione = dataEvento;
                SessionManager.EventoSelezionato.durata_lavorazione = 1;
                SessionManager.EventoSelezionato.id_colonne_agenda = risorsaEvento;
                SessionManager.EventoSelezionato.data_inizio_impegno = dataEvento;
                SessionManager.EventoSelezionato.data_fine_impegno = dataEvento;
                SessionManager.EventoSelezionato.durata_viaggio_andata = 0;
                SessionManager.EventoSelezionato.durata_viaggio_ritorno = 0;
                SessionManager.EventoSelezionato.id_stato = sottotipoRisorsa == EnumSottotipiRisorse.DIPENDENTI.ToString() ? Stato.Instance.STATO_RIPOSO : Stato.Instance.STATO_PREVISIONE_IMPEGNO;
            }

            return SessionManager.EventoSelezionato;
        }

        private Esito SalvaEvento()
        {
            popupAppuntamento.NascondiErroriValidazione();

            Esito esito = new Esito();
            List<string> listaIdTender = popupAppuntamento.ListaIdTender;
            DatiLavorazione datiLavorazione = SessionManager.EventoSelezionato.id_stato != Stato.Instance.STATO_LAVORAZIONE ? null :  popupLavorazione.CreaDatiLavorazione();
            SessionManager.EventoSelezionato.LavorazioneCorrente = datiLavorazione;

            esito = ValidazioneSalvataggio(listaIdTender);

            if (esito.Codice == Esito.ESITO_OK)
            {
                if (SessionManager.EventoSelezionato.id == 0)
                {
                    int risorsaEvento = int.Parse(hf_risorsa.Value);
                    string sottotipoRisorsa = UtilityTipologiche.getElementByID(SessionManager.ListaRisorse, risorsaEvento, ref esito).sottotipo.ToUpper();

                    NoteOfferta noteOfferta = new NoteOfferta();
                    if (sottotipoRisorsa != EnumSottotipiRisorse.DIPENDENTI.ToString())
                    {
                        Anag_Clienti_Fornitori cliente = Anag_Clienti_Fornitori_BLL.Instance.getAziendaById(SessionManager.EventoSelezionato.id_cliente, ref esito);
                        List<DatiBancari> datiBancari = Config_BLL.Instance.getListaDatiBancari(ref esito);
                        noteOfferta.Banca = datiBancari[0].DatiCompleti;
                        noteOfferta.Pagamento = cliente.Pagamento;
                        noteOfferta.NotaPagamento = cliente.Pagamento.ToString();
                        noteOfferta.Consegna = cliente.TipoIndirizzoLegale + " " + cliente.IndirizzoLegale + " " + cliente.NumeroCivicoLegale + " " + cliente.CapLegale + " " + cliente.ComuneLegale + " " + cliente.ProvinciaLegale + " ";
                        noteOfferta.Note = "";// "Unicredit Banca: IBAN: IT39H0200805198000103515620", Pagamento = cliente.Pagamento, Consegna = cliente.TipoIndirizzoLegale + " " + cliente.IndirizzoLegale + " " + cliente.NumeroCivicoLegale + " " + cliente.CapLegale + " " + cliente.ProvinciaLegale + " " };
                    }
                    
                    esito = Agenda_BLL.Instance.CreaEvento(SessionManager.EventoSelezionato, listaIdTender, noteOfferta);
                    // QUANDO CREO L'EVENTO E HO INSERITO CORRETTAMENTE I DATI IN TABELLA, CREO IL PDF OFFERTA
                    if (esito.Codice == 0) { 
                        esito = popupRiepilogoOfferta.popolaPannelloRiepilogo(SessionManager.EventoSelezionato);
                    }
                }
                else
                {
                    esito = Agenda_BLL.Instance.AggiornaEvento(SessionManager.EventoSelezionato, listaIdTender);
                }

                GestisciErrore(esito);

                ListaDatiAgenda = Agenda_BLL.Instance.CaricaDatiAgenda(DateTime.Parse(hf_valoreData.Value), ref esito);

                #region SALVATAGGIO PDF
                if (!string.IsNullOrEmpty(SessionManager.EventoSelezionato.codice_lavoro))
                {
                    //SalvaPdfOffertaSuFile();
                    //SalvaPdfConsuntivoSuFile();
                }
                #endregion
            }
            else
            {
                ShowWarning(esito.Descrizione);
                popupAppuntamento.PopolaAppuntamento();
            }

            return esito;
        }

        private void SalvaPdfOffertaSuFile()
        {
            Esito esito = popupRiepilogoOfferta.popolaPannelloRiepilogo(SessionManager.EventoSelezionato);
            if (esito.Codice == Esito.ESITO_OK) { 
                string nomeFile = "Offerta_" + val_CodiceLavoro.Text + ".pdf";
            
                //MemoryStream workStream = popupRiepilogoOfferta.GeneraPdf();

                //string pathOfferta = MapPath(ConfigurationManager.AppSettings["PATH_DOCUMENTI_PROTOCOLLO"]) + nomeFile;
                //string pathPdfSenzaNumeroPagina = MapPath(ConfigurationManager.AppSettings["PATH_DOCUMENTI_PROTOCOLLO"]) + "tmp_"+nomeFile;
                //File.WriteAllBytes(pathPdfSenzaNumeroPagina, workStream.ToArray());

                //string nomeFileToDisplay = BaseStampa.Instance.AddPageNumber(pathPdfSenzaNumeroPagina, pathOfferta, ref esito);

                //if (File.Exists(pathPdfSenzaNumeroPagina)) File.Delete(pathPdfSenzaNumeroPagina);
                
                //if (esito.Codice == Esito.ESITO_OK) { 
                //    // RIPORTA IL NOME DEL FILE PDF DA VISUALIZZARE SULLA FINESTRA RIEPILOGO
                //    popupRiepilogoOfferta.associaNomePdf(nomeFileToDisplay);
                //}
                //else
                //{
                //    ShowError(esito.Descrizione);
                //}
            }
            else
            {
                ShowError(esito.Descrizione);
            }
        }

        private void SalvaPdfConsuntivoSuFile()
        {
            //Esito esito = popupConsuntivo.popolaPannelloRiepilogo(SessionManager.EventoSelezionato);

            string nomeFile = "Consuntivo_" + val_CodiceLavoro.Text + ".pdf";
            //MemoryStream workStream = popupConsuntivo.GeneraPdf();

            string pathConsuntivo = MapPath(ConfigurationManager.AppSettings["PATH_DOCUMENTI_PROTOCOLLO"]) + nomeFile;
            //File.WriteAllBytes(pathConsuntivo, workStream.ToArray());
        }

        #endregion

        #region OPERAZIONI SUI DATI
        private bool IsDisponibileDataRisorsa(DatiAgenda eventoDaControllare)
        {
            DatiAgenda eventoEsistente = ListaDatiAgenda.FirstOrDefault(x => x.id != eventoDaControllare.id &&
                                                         x.id_colonne_agenda == eventoDaControllare.id_colonne_agenda &&
                                                        ((x.data_inizio_impegno <= eventoDaControllare.data_inizio_impegno && x.data_fine_impegno >= eventoDaControllare.data_inizio_impegno) ||
                                                        (x.data_inizio_impegno <= eventoDaControllare.data_fine_impegno && x.data_fine_impegno >= eventoDaControllare.data_fine_impegno) ||
                                                        (x.data_inizio_impegno >= eventoDaControllare.data_inizio_impegno && x.data_fine_impegno <= eventoDaControllare.data_fine_impegno)
                                                        ));

            return eventoEsistente == null;
        }

        private bool IsDisponibileTender(DatiAgenda eventoDaControllare, ref List<string> listaTenderNonDisponibili, List<string> listaIdTenderSelezionati)
        {
            if (listaIdTenderSelezionati == null)
            {
                return true;
            }
            else
            {
                Esito esito = new Esito();
                List<int> listaIdTenderImpiegatiInPeriodo = Agenda_BLL.Instance.GetTenderImpiegatiInPeriodo(eventoDaControllare, ref esito);


                foreach (string idTenderCorrente in listaIdTenderSelezionati)
                {
                    if (listaIdTenderImpiegatiInPeriodo.Contains(int.Parse(idTenderCorrente)))
                    {
                        listaTenderNonDisponibili.Add(SessionManager.ListaTender.Where(x => x.id == int.Parse(idTenderCorrente)).FirstOrDefault<Tipologica>().nome);
                    }
                }

                return listaTenderNonDisponibili.Count == 0;
            }
        }

        public List<DatiArticoli> GetListaArticoli()
        {
            return SessionManager.EventoSelezionato.ListaDatiArticoli;
        }

        public string GetCodiceLavoro()
        {
            return val_CodiceLavoro.Text;
        }

        private Esito ValidazioneSalvataggio(List<string> listaIdTender)
        {
            DatiAgenda _eventoSelezionato = SessionManager.EventoSelezionato;
            Esito esito = new Esito();
            esito = popupAppuntamento.CreaOggettoSalvataggio(ref _eventoSelezionato);

            List<string> listaTenderNonDisponibili = new List<string>();

            if (esito.Codice != Esito.ESITO_OK)
            {
                esito.Descrizione = "Controllare i campi evidenziati";

            }
            else if (!IsDisponibileDataRisorsa(_eventoSelezionato))
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_VALIDAZIONE;
                esito.Descrizione = "Non è possibile salvare perché la risorsa è già impiegata nel periodo selezionato";
            }
            else if (!IsDisponibileTender(_eventoSelezionato, ref listaTenderNonDisponibili, listaIdTender))
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_VALIDAZIONE;
                esito.Descrizione = "Non è possibile salvare perché le seguenti unità appoggio sono già impiegate nel periodo selezionato:<br/><ul>";

                foreach (string tender in listaTenderNonDisponibili)
                {
                    esito.Descrizione += "<li>" + tender + "</li>";
                }
                esito.Descrizione += "</ul>";

            }
            else if (SessionManager.EventoSelezionato.id_stato == Stato.Instance.STATO_OFFERTA && 
                    (SessionManager.EventoSelezionato.ListaDatiArticoli == null || SessionManager.EventoSelezionato.ListaDatiArticoli.Count == 0))
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_VALIDAZIONE;
                esito.Descrizione = "Non è possibile salvare senza aver associato gli articoli";
            }
            SessionManager.EventoSelezionato = _eventoSelezionato;
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

        private void RiempiCampiIntestazioneEvento()
        {
            if (SessionManager.EventoSelezionato.produzione != null && SessionManager.EventoSelezionato.lavorazione!=null && SessionManager.EventoSelezionato.id_cliente!=0)
            {
                val_Cliente.Text = SessionManager.ListaClientiFornitori.FirstOrDefault(x => x.Id == SessionManager.EventoSelezionato.id_cliente).RagioneSociale;
                val_Produzione.Text = SessionManager.EventoSelezionato.produzione;
                val_Lavorazione.Text = SessionManager.EventoSelezionato.lavorazione;
                val_Tipologia.Text = SessionManager.ListaTipiTipologie.FirstOrDefault(X => X.id == SessionManager.EventoSelezionato.id_tipologia).nome;
                val_DataInizio.Text = SessionManager.EventoSelezionato.data_inizio_lavorazione.ToString("dd/MM/yyyy");
                val_DataFine.Text = SessionManager.EventoSelezionato.data_fine_lavorazione.ToString("dd/MM/yyyy");
            }
        }
        #endregion

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Non eliminare questo metodo. Risolve il problema della stampa. */
        }

        protected void btnStampaConsuntivo_Click(object sender, EventArgs e)
        {
            Esito esito = SalvaEvento();
            if (esito.Codice == Esito.ESITO_OK)
            {
                esito = popupRiepilogoConsuntivo.popolaPannelloConsuntivo(SessionManager.EventoSelezionato);
                if (esito.Codice == Esito.ESITO_OK)
                {
                    upRiepilogoConsuntivo.Update();

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "aggiornaAgenda", "aggiornaAgenda();", true);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriConsuntivo", script: "javascript: document.getElementById('modalConsuntivo').style.display='block'", addScriptTags: true);
                }
                else
                {
                    ShowError(esito.Descrizione);
                }
            }
            else
            {
                ShowError(esito.Descrizione);
                UpdatePopup();
            }

        }

        protected void btnStampaFattura_Click(object sender, EventArgs e)
        {
            Esito esito = SalvaEvento();
            if (esito.Codice == Esito.ESITO_OK)
            {
                esito = popupRiepilogoFattura.popolaPannelloFattura(SessionManager.EventoSelezionato);
                if (esito.Codice == Esito.ESITO_OK)
                {
                    upRiepilogoFattura.Update();

                    ScriptManager.RegisterStartupScript(this, typeof(Page), "aggiornaAgenda", "aggiornaAgenda();", true);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriFattura", script: "javascript: document.getElementById('modalFattura').style.display='block'", addScriptTags: true);
                }
                else
                {
                    ShowError(esito.Descrizione);
                }
            }
            else
            {
                ShowError(esito.Descrizione);
                UpdatePopup();
            }
        }
    }
}