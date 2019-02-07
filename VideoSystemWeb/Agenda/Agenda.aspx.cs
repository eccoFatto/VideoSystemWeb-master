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

            Esito esito = new Esito();
            
            if (!IsPostBack)
            {
                DateTime dataPartenza = DateTime.Now;
                listaDatiAgenda = Agenda_BLL.Instance.CaricaDatiAgenda(dataPartenza, ref esito);
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

            popupAppuntamento.EditEvent(dataEvento, risorsaEvento);
            
            pnlContainer.Style.Remove("display");
            panelAppuntamento.Style.Remove("display");
        }

        protected void btn_chiudi_Click(object sender, EventArgs e)
        {
            OperazioniPopup("CLOSE");
            //ChiudiPopup();
            //ScriptManager.RegisterStartupScript(this, typeof(Page), "aggiornaAgenda", "aggiornaAgenda();", true);
        }
        #endregion

        private void ChiudiPopup()
        {
            pnlContainer.Style.Add("display", "none");
            UpdatePopup();
        }

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

        private bool IsPrimoGiorno(DatiAgenda evento, DateTime data)
        {
            return evento.data_inizio_lavorazione.Date == data.Date;
        }

        private bool IsUltimoGiorno(DatiAgenda evento, DateTime data)
        {
            return evento.data_fine_lavorazione.Date == data.Date;
        }

        private void UpdatePopup()
        {
            upEvento.Update();
        }

        public void OperazioniPopup(string operazione)
        {
            switch (operazione)
            {
                case "UPDATE":
                    UpdatePopup();
                    break;
                case "CLOSE":
                    panelAppuntamento.Style.Add("display", "none");
                    panelOfferta.Style.Add("display", "none");
                    panelLavorazione.Style.Add("display", "none");
                    UpdatePopup();
                    ChiudiPopup();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "aggiornaAgenda", "aggiornaAgenda();", true);
                    break;
                case "OFFERTA":
                    panelAppuntamento.Style.Add("display", "none");
                    panelOfferta.Style.Remove("display");
                    UpdatePopup();
                    break;
                case "LAVORAZIONE":
                    panelAppuntamento.Style.Add("display", "none");
                    panelLavorazione.Style.Remove("display");
                    UpdatePopup();
                    break;
            }

        }
    }
}