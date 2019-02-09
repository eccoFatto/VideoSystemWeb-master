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
        protected void Page_Load(object sender, EventArgs e)
        {
            popupAppuntamento.RichiediOperazionePopup += OperazioniPopup;
            
            if (!IsPostBack)
            {
                Esito esito = new Esito();
                DateTime dataPartenza = DateTime.Now;
                listaDatiAgenda = Agenda_BLL.Instance.CaricaDatiAgenda(dataPartenza, ref esito);
                ViewState["listaDatiAgenda"] = listaDatiAgenda;
                hf_valoreData.Value = dataPartenza.ToString("dd/MM/yyyy");
                gv_scheduler.DataSource = CreateDataTable(dataPartenza);
                gv_scheduler.DataBind();
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
            //ViewState["eventoSelezionato"] = eventoSelezionato;

            AbilitaComponentiPopup();

            MostraPopup(eventoSelezionato);
        }

        protected void btnSalva_Click(object sender, EventArgs e)
        {
            Esito esito = SalvaEvento();
            if (esito.codice == Esito.ESITO_OK)
            {
                ChiudiPopup();
            }
            else
            {
                panelErrore.Style.Remove("display");
                lbl_MessaggioErrore.Text = esito.descrizione;
                UpdatePopup();
            }
        }

        protected void btnElimina_Click(object sender, EventArgs e)
        {
            panelErrore.Style.Add("display", "none");

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
            DatiAgenda eventoSelezionato = (DatiAgenda)ViewState["eventoSelezionato"];
            popupAppuntamento.CreaOggettoSalvataggio(ref eventoSelezionato);
            popupAppuntamento.PopolaPopup(eventoSelezionato);
            AbilitaComponentiPopup();
            btnLavorazione.Visible = false;
            UpdatePopup();
        }

        protected void btnLavorazione_Click(object sender, EventArgs e)
        {
            popupAppuntamento.SetStato(DatiAgenda.STATO_LAVORAZIONE);
            DatiAgenda eventoSelezionato = (DatiAgenda)ViewState["eventoSelezionato"];
            popupAppuntamento.CreaOggettoSalvataggio(ref eventoSelezionato);
            popupAppuntamento.PopolaPopup(eventoSelezionato);
            AbilitaComponentiPopup();
            UpdatePopup();
        }

        protected void btnRiposo_Click(object sender, EventArgs e)
        {
            popupAppuntamento.SetStato(DatiAgenda.STATO_RIPOSO);
            DatiAgenda eventoSelezionato = (DatiAgenda)ViewState["eventoSelezionato"];
            popupAppuntamento.CreaOggettoSalvataggio(ref eventoSelezionato);
            popupAppuntamento.PopolaPopup(eventoSelezionato);
            AbilitaComponentiPopup();
            UpdatePopup();
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
                    DatiAgenda datiAgendaFiltrati = Agenda_BLL.Instance.GetDatiAgendaByDataRisorsa(listaDatiAgenda, dataRiga, risorsa.id); listaDatiAgenda.Where(x => x.data_inizio_lavorazione <= dataRiga && x.data_fine_lavorazione >= dataRiga && x.id_colonne_agenda == risorsa.id).ToList<DatiAgenda>();
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
            bool isUtenteAbilitatoInScrittura = AbilitazioneInScrittura();

            e.Row.Cells[0].Attributes.Add("class", "first");

            #region intestazione tabella
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int indiceColonna = 1; indiceColonna <= listaRisorse.Count; indiceColonna++)
                {
                    string idRisorsa = (e.Row.Cells[indiceColonna].Text.Trim());

                    Tipologica risorsaCorrente = Tipologie.getRisorsaById(int.Parse(idRisorsa));

                    Esito esito = new Esito();
                    string colore = UtilityTipologiche.getParametroDaTipologica(risorsaCorrente, "color", ref esito);

                    e.Row.Cells[indiceColonna].Attributes.Add("style", "background-color:" + colore + ";font-size:10pt;text-align:center;width:100px;");
                    e.Row.Cells[indiceColonna].Text = risorsaCorrente.nome;
                }
            }
            #endregion

            #region dati agenda
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Attributes.Add("style", "font-weight:bold;background-color:#FDEDB5;width:100px;height:40px;");
                e.Row.Cells[0].Attributes.Add("class", "first");

                for (int indiceColonna = 1; indiceColonna <= listaRisorse.Count; indiceColonna++)
                {
                    string data = e.Row.Cells[0].Text;
                    int risorsa = ((Tipologica)listaRisorse.ElementAt(indiceColonna - 1)).id;

                    if (!string.IsNullOrEmpty(e.Row.Cells[indiceColonna].Text.Trim()))
                    {
                        DatiAgenda datoAgendaCorrente = Agenda_BLL.Instance.GetDatiAgendaById(listaDatiAgenda, int.Parse(e.Row.Cells[indiceColonna].Text.Trim()));

                        Esito esito = new Esito();
                        Tipologica statoCorrente = UtilityTipologiche.getElementByID(listaStati, datoAgendaCorrente.id_stato, ref esito);

                        string colore;



                        colore = UtilityTipologiche.getParametroDaTipologica(statoCorrente, "color", ref esito);

                        e.Row.Cells[indiceColonna].CssClass = "evento";

                        // EVENTO GIORNO SINGOLO
                        if (IsPrimoGiorno(datoAgendaCorrente, DateTime.Parse(data)) && IsUltimoGiorno(datoAgendaCorrente, DateTime.Parse(data)))
                        {
                            Panel mainPanel = new Panel();
                            mainPanel.Controls.Add(new LiteralControl(datoAgendaCorrente.produzione));
                            mainPanel.CssClass = "round-corners-6px";
                            mainPanel.Attributes.Add("style", "border: 2px solid " + colore + "; background-color:" + colore);
                            e.Row.Cells[indiceColonna].Controls.Add(mainPanel);
                        }
                        else
                        {
                            if (IsPrimoGiorno(datoAgendaCorrente, DateTime.Parse(data)) && !IsUltimoGiorno(datoAgendaCorrente, DateTime.Parse(data)))
                            {
                                if (IsViaggioAndata(datoAgendaCorrente, DateTime.Parse(data)))
                                {
                                    colore = "#FFFF00";
                                }

                                Panel mainPanel = new Panel();
                                mainPanel.Controls.Add(new LiteralControl(datoAgendaCorrente.produzione));
                                mainPanel.CssClass = "round-corners-6px unround-bottom-corners";
                                mainPanel.Attributes.Add("style", "border: 2px solid " + colore + "; background-color:" + colore);
                                e.Row.Cells[indiceColonna].Controls.Add(mainPanel);

                                e.Row.Cells[indiceColonna].Attributes.Add("style", "border-bottom: 0px; vertical-align: bottom");
                            }
                            else if (!IsPrimoGiorno(datoAgendaCorrente, DateTime.Parse(data)) && IsUltimoGiorno(datoAgendaCorrente, DateTime.Parse(data)))
                            {
                                if (IsViaggioRitorno(datoAgendaCorrente, DateTime.Parse(data)))
                                {
                                    colore = "#FFFF00";
                                }

                                Panel mainPanel = new Panel();
                                mainPanel.Controls.Add(new LiteralControl("&nbsp;"));
                                mainPanel.CssClass = "round-corners-6px unround-top-corners";
                                mainPanel.Attributes.Add("style", "border: 2px solid " + colore + "; background-color:" + colore);
                                e.Row.Cells[indiceColonna].Controls.Add(mainPanel);

                                e.Row.Cells[indiceColonna].Attributes.Add("style", "border-top: 0px; vertical-align: top");
                            }
                            else if (!IsPrimoGiorno(datoAgendaCorrente, DateTime.Parse(data)) && !IsUltimoGiorno(datoAgendaCorrente, DateTime.Parse(data)))
                            {
                                if (IsViaggioAndata(datoAgendaCorrente, DateTime.Parse(data)) || IsViaggioRitorno(datoAgendaCorrente, DateTime.Parse(data)))
                                {
                                    colore = "#FFFF00";
                                }

                                e.Row.Cells[indiceColonna].Text = "";
                                e.Row.Cells[indiceColonna].Attributes.Add("style", "border-top: 0px; border-bottom: 0px; background-color:" + colore);
                            }
                        }
                        e.Row.Cells[indiceColonna].Attributes["onclick"] = "mostracella('" + data + "', '" + risorsa + "');";
                    }
                    else
                    {
                        if (isUtenteAbilitatoInScrittura)
                        {
                            e.Row.Cells[indiceColonna].Attributes["onclick"] = "mostracella('" + data + "', '" + risorsa + "');";
                        }
                    }
                }
            }
            #endregion
        }

        private void AggiornaAgenda()
        {
            Esito esito = new Esito();

            listaDatiAgenda = Agenda_BLL.Instance.CaricaDatiAgenda(DateTime.Parse(hf_valoreData.Value), ref esito);
            gv_scheduler.DataSource = CreateDataTable(DateTime.Parse(hf_valoreData.Value));
            gv_scheduler.DataBind();

            ScriptManager.RegisterStartupScript(this, typeof(Page), "passaggioMouse", "registraPassaggioMouse();", true);
        }

        private bool IsDisponibileDataRisorsa(DatiAgenda eventoDaControllare)
        {
            listaDatiAgenda = (List<DatiAgenda>)ViewState["listaDatiAgenda"];
            DatiAgenda eventoEsistente = listaDatiAgenda.Where(x => x.id != eventoDaControllare.id &&
                                                         x.id_colonne_agenda == eventoDaControllare.id_colonne_agenda &&
                                                        ((x.data_inizio_lavorazione <= eventoDaControllare.data_inizio_lavorazione && x.data_fine_lavorazione >= eventoDaControllare.data_inizio_lavorazione) ||
                                                        (x.data_inizio_lavorazione <= eventoDaControllare.data_fine_lavorazione && x.data_fine_lavorazione >= eventoDaControllare.data_fine_lavorazione) ||
                                                        (x.data_inizio_lavorazione >= eventoDaControllare.data_inizio_lavorazione && x.data_fine_lavorazione <= eventoDaControllare.data_fine_lavorazione)
                                                        )).FirstOrDefault();

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
            upEvento.Update();
        }

        private void AbilitaComponentiPopup()
        {
            Esito esito = new Esito();
            DatiAgenda eventoSelezionato = (DatiAgenda)ViewState["eventoSelezionato"];
            string sottotipoRisorsa = "";
            if (eventoSelezionato != null)
            {
                sottotipoRisorsa = UtilityTipologiche.getElementByID(listaRisorse, eventoSelezionato.id_colonne_agenda, ref esito).sottotipo;
            }

            btnOfferta.Visible = eventoSelezionato != null && sottotipoRisorsa != "dipendenti" && eventoSelezionato.id != 0 && eventoSelezionato.id_stato == DatiAgenda.STATO_PREVISIONE_IMPEGNO;
            btnLavorazione.Visible = eventoSelezionato != null && sottotipoRisorsa != "dipendenti" && eventoSelezionato.id != 0 && eventoSelezionato.id_stato == DatiAgenda.STATO_OFFERTA;
            btnElimina.Visible = eventoSelezionato != null && eventoSelezionato.id != 0 && eventoSelezionato.id_stato == DatiAgenda.STATO_PREVISIONE_IMPEGNO;

            btnRiposo.Visible = sottotipoRisorsa == "dipendenti";

            popupAppuntamento.AbilitaComponentiPopup(eventoSelezionato);
        }

        private void MostraPopup(DatiAgenda eventoSelezionato)
        {
            pnlContainer.Style.Remove("display");

            panelErrore.Style.Add("display", "none");
            lbl_MessaggioErrore.Text = string.Empty;
            popupAppuntamento.ClearPopupEventi();
            popupAppuntamento.PopolaPopup(eventoSelezionato);
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
            bool isUtenteAbilitatoInScrittura = AbilitazioneInScrittura();

            listaDatiAgenda = (List<DatiAgenda>)ViewState["listaDatiAgenda"];

            DatiAgenda eventoSelezionato = Agenda_BLL.Instance.GetDatiAgendaByDataRisorsa(listaDatiAgenda, dataEvento, risorsaEvento);

            if (eventoSelezionato == null)
            {
                eventoSelezionato = new DatiAgenda
                {
                    data_inizio_lavorazione = dataEvento,
                    id_colonne_agenda = risorsaEvento,
                    id_stato = DatiAgenda.STATO_PREVISIONE_IMPEGNO
                };
            }

            ViewState["eventoSelezionato"] = eventoSelezionato;

            return eventoSelezionato;
        }

        private Esito SalvaEvento()
        {
            Esito esito = new Esito();
            DatiAgenda eventoSelezionato = (DatiAgenda)ViewState["eventoSelezionato"];

            #region DATI DELL'APPUNTAMENTO

            esito = popupAppuntamento.CreaOggettoSalvataggio(ref eventoSelezionato);
            if (esito.codice != Esito.ESITO_OK)
            {
                esito.descrizione = "Controllare i campi evidenziati";
                //panelErrore.Style.Remove("display");
                //lbl_MessaggioErrore.Text = "Controllare i campi evidenziati";
               // UpdatePopup();
            }
            else
            {
                panelErrore.Style.Add("display", "none");
                popupAppuntamento.NascondiErroriValidazione();
                if (IsDisponibileDataRisorsa(eventoSelezionato))
                {
                    if (eventoSelezionato.id == 0)
                    {
                        Agenda_BLL.Instance.CreaEvento(eventoSelezionato);
                    }
                    else
                    {
                        Agenda_BLL.Instance.AggiornaEvento(eventoSelezionato);
                    }
                    ViewState["listaDatiAgenda"] = Agenda_BLL.Instance.CaricaDatiAgenda(DateTime.Parse(hf_valoreData.Value), ref esito);
                   // ChiudiPopup();

                }
                else
                {
                    esito.codice = Esito.ESITO_KO_ERRORE_VALIDAZIONE;
                    esito.descrizione = "Non è possibile salvare l'evento perché la risorsa è già impiegata nel periodo selezionato";
                    //panelErrore.Style.Remove("display");
                    //lbl_MessaggioErrore.Text = "Non è possibile salvare l'evento perché la risorsa è già impiegata nel periodo selezionato";
                   // UpdatePopup();
                }
            }
            #endregion
            return esito;
        }

        private bool IsPrimoGiorno(DatiAgenda evento, DateTime data)
        {
            return evento.data_inizio_lavorazione.Date == data.Date;
        }

        private bool IsUltimoGiorno(DatiAgenda evento, DateTime data)
        {
            return evento.data_fine_lavorazione.Date == data.Date;
        }

        private bool IsViaggioAndata(DatiAgenda evento, DateTime data)
        {
            if (evento.durata_viaggio_andata > 0)
            {
                return (data.Date >= evento.data_inizio_lavorazione.Date && data.Date < evento.data_inizio_lavorazione.AddDays(evento.durata_viaggio_andata).Date);
            }

            return false;
        }

        private bool IsViaggioRitorno(DatiAgenda evento, DateTime data)
        {
            if (evento.durata_viaggio_ritorno > 0)
            {
                return (data.Date > evento.data_fine_lavorazione.AddDays(evento.durata_viaggio_ritorno * -1).Date && data <= evento.data_fine_lavorazione.Date);
            }

            return false;
        }


        #endregion
    }
}