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
            Esito esito = new Esito();
            listaDatiAgenda = Agenda_BLL.Instance.CaricaDatiAgenda(ref esito);//Tipologie.getListaDatiAgenda();
            caricaListeTipologiche();

            if (!IsPostBack)
            {
                DateTime dataPartenza = DateTime.Now;
                hf_valoreData.Value = dataPartenza.ToString("dd/MM/yyyy");
                gv_scheduler.DataSource = CreateDataTable(dataPartenza);
                gv_scheduler.DataBind();

                popolaDDLTipologica(ddl_Risorse, listaRisorse);
                popolaDDLTipologica(ddl_Tipologia, listaTipiTipologie);
            }
        }

        #region COMPORTAMENTO ELEMENTI PAGINA
        protected void btnsearch_Click(object sender, EventArgs e)
        {
            aggiornaAgenda();
        }

        protected void btnModifica_Click(object sender, EventArgs e)
        {
            lbl_MessaggioErrore.Visible = false;
            AttivaDisattivaModifica(true);
        }

        protected void btnSalva_Click(object sender, EventArgs e)
        {
            Esito esito = new Esito();
            DatiAgenda datiAgenda = creaOggettoSalvataggio(ref esito);

            if (esito.codice != Esito.ESITO_OK)
            {
                lbl_MessaggioErrore.Visible = true;
                upEvento.Update();
            }
            else
            {
                nascondiErroriValidazione();

                if (datiAgenda.id == 0)
                {
                    Agenda_BLL.Instance.creaEvento(datiAgenda);
                }
                else
                {
                    Agenda_BLL.Instance.aggiornaEvento(datiAgenda);
                }
                
                ScriptManager.RegisterStartupScript(this, typeof(Page), "closePopup", "chiudiPopup();", true);
            }

        }

        protected void btnAnnulla_Click(object sender, EventArgs e)
        {
            nascondiErroriValidazione();
            AttivaDisattivaModifica(false);
        }

        protected void btnEditEvent_Click(object sender, EventArgs e)
        {
            hf_idEvento.Value = "0";
            clearPopupEventi();

            DateTime dataEvento = DateTime.Parse(hf_data.Value);
            int risorsaEvento = int.Parse(hf_risorsa.Value);

            DatiAgenda eventoSelezionato = Agenda_BLL.Instance.getDatiAgendaByDataRisorsa(listaDatiAgenda, dataEvento, risorsaEvento);

            if (eventoSelezionato == null)
            {
                AttivaDisattivaModifica(true);
                btnAnnulla.Visible = false;
            }
            else
            {
                AttivaDisattivaModifica(false);
                popolaPopupEventi(eventoSelezionato);
            }

            pnlContainer.Visible = true;
        }

        protected void btn_chiudi_Click(object sender, EventArgs e)
        {
            aggiornaAgenda();
            pnlContainer.Visible = false;
        }
        #endregion

        private void popolaDDLTipologica(DropDownList ddl, List<Tipologica> listaTipologica)
        {
            listaTipologica.Insert(0, new Tipologica() { id = 0, nome = "<scegli>" });

            ddl.DataSource = listaTipologica;
            ddl.DataTextField = "nome";
            ddl.DataValueField = "id";
            ddl.DataBind();
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
                    DatiAgenda datiAgendaFiltrati = Agenda_BLL.Instance.getDatiAgendaByDataRisorsa(listaDatiAgenda, dataRiga, risorsa.id); listaDatiAgenda.Where(x => x.data_inizio_lavorazione <= dataRiga && x.data_fine_lavorazione >= dataRiga && x.id_colonne_agenda == risorsa.id).ToList<DatiAgenda>();
                    if (datiAgendaFiltrati != null)
                    {
                        //DatiAgenda datoCorrente = datiAgendaFiltrati.FirstOrDefault();

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
                e.Row.Cells[0].Attributes.Add("style", "font-weight:bold;background-color:#FDEDB5;width:100px;");
                e.Row.Cells[0].Attributes.Add("class", "first");

                for (int indiceColonna = 1; indiceColonna <= listaRisorse.Count; indiceColonna++)
                {
                    if (!string.IsNullOrEmpty(e.Row.Cells[indiceColonna].Text.Trim()))
                    {
                        DatiAgenda datoAgendaCorrente = Agenda_BLL.Instance.getDatiAgendaById(listaDatiAgenda, int.Parse(e.Row.Cells[indiceColonna].Text.Trim())); //Tipologie.getDatiAgendaById(int.Parse(e.Row.Cells[indiceColonna].Text.Trim()));

                        Esito esito = new Esito();
                        Tipologica statoCorrente = UtilityTipologiche.getElementByID(listaStati, datoAgendaCorrente.id_stato, ref esito);
                        string colore = UtilityTipologiche.getParametroDaTipologica(statoCorrente, "color", ref esito);

                        e.Row.Cells[indiceColonna].Text = datoAgendaCorrente.produzione;
                        e.Row.Cells[indiceColonna].Attributes.Add("style", "font-size:8pt;font-weight:normal;background-color:" + colore);
                        //e.Row.Cells[indiceColonna].CssClass = "cella";
                    }
                    else
                    {
                        //e.Row.Cells[indiceColonna].CssClass = "cellaVuota";
                    }

                    string data = e.Row.Cells[0].Text;
                    int risorsa = ((Tipologica)listaRisorse.ElementAt(indiceColonna - 1)).id;

                    e.Row.Cells[indiceColonna].Attributes["onclick"] = "mostracella('" + data + "', '" + risorsa + "');";
                }

                e.Row.Attributes.Add("style", "text-align:center;");
            }
            #endregion
        }


        private DatiAgenda creaOggettoSalvataggio(ref Esito esito)
        {
            DatiAgenda datiAgenda = new DatiAgenda();
            datiAgenda.id = int.Parse(hf_idEvento.Value);
            datiAgenda.data_inizio_lavorazione = validaCampo(txt_DataInizioLavorazione, DateTime.Now, true, ref esito);
            datiAgenda.data_fine_lavorazione = validaCampo(txt_FineLavorazione, DateTime.Now, true, ref esito);
            datiAgenda.durata_lavorazione = validaCampo(txt_DurataLavorazione, 0, false, ref esito);
            datiAgenda.id_colonne_agenda = validaCampo(ddl_Risorse, 0, true, ref esito);
            datiAgenda.id_tipologia = validaCampo(ddl_Tipologia, 0, true, ref esito);
            datiAgenda.id_cliente = validaCampo(ddl_cliente, 0, false, ref esito);
            datiAgenda.durata_viaggio_andata = validaCampo(txt_DurataViaggioAndata, 0, false, ref esito);
            datiAgenda.durata_viaggio_ritorno = validaCampo(txt_DurataViaggioRitorno, 0, false, ref esito);
            datiAgenda.data_inizio_impegno = validaCampo(txt_DataInizioImpegno, DateTime.MinValue, false, ref esito);
            datiAgenda.data_fine_impegno = validaCampo(txt_DataFineImpegno, DateTime.MinValue, false, ref esito);
            datiAgenda.impegnoOrario = chk_ImpegnoOrario.Checked;
            datiAgenda.impegnoOrario_da = validaCampo(txt_ImpegnoOrarioDa, DateTime.MinValue, chk_ImpegnoOrario.Checked, ref esito);
            datiAgenda.impegnoOrario_a = validaCampo(txt_ImpegnoOrarioA, DateTime.MinValue, chk_ImpegnoOrario.Checked, ref esito);
            datiAgenda.produzione = validaCampo(txt_Produzione, "", true, ref esito);
            datiAgenda.lavorazione = validaCampo(txt_Lavorazione, "", false, ref esito);
            datiAgenda.indirizzo = validaCampo(txt_Indirizzo, "", false, ref esito);
            datiAgenda.luogo = validaCampo(txt_Luogo, "", false, ref esito);
            datiAgenda.codice_lavoro = validaCampo(txt_CodiceLavoro, "", false, ref esito);
            datiAgenda.nota = validaCampo(tb_Nota, "", false, ref esito);

            return datiAgenda;
        }


        private void nascondiErroriValidazione()
        {
            lbl_MessaggioErrore.Visible = false;

            txt_DataInizioLavorazione.CssClass = txt_DataInizioLavorazione.CssClass.Replace("erroreValidazione", "");
            txt_FineLavorazione.CssClass = txt_FineLavorazione.CssClass.Replace("erroreValidazione", "");
            txt_DurataLavorazione.CssClass = txt_DurataLavorazione.CssClass.Replace("erroreValidazione", "");
            ddl_Risorse.CssClass = ddl_Risorse.CssClass.Replace("erroreValidazione", "");
            ddl_Tipologia.CssClass = ddl_Tipologia.CssClass.Replace("erroreValidazione", "");
            ddl_cliente.CssClass = ddl_cliente.CssClass.Replace("erroreValidazione", "");
            txt_DurataViaggioAndata.CssClass = txt_DurataViaggioAndata.CssClass.Replace("erroreValidazione", "");
            txt_DurataViaggioRitorno.CssClass = txt_DurataViaggioRitorno.CssClass.Replace("erroreValidazione", "");
            txt_DataInizioImpegno.CssClass = txt_DataInizioImpegno.CssClass.Replace("erroreValidazione", "");
            txt_DataFineImpegno.CssClass = txt_DataFineImpegno.CssClass.Replace("erroreValidazione", "");
            txt_ImpegnoOrarioDa.CssClass = txt_ImpegnoOrarioDa.CssClass.Replace("erroreValidazione", "");
            txt_ImpegnoOrarioA.CssClass = txt_ImpegnoOrarioA.CssClass.Replace("erroreValidazione", "");
            txt_Produzione.CssClass = txt_Produzione.CssClass.Replace("erroreValidazione", "");
            txt_Lavorazione.CssClass = txt_Lavorazione.CssClass.Replace("erroreValidazione", "");
            txt_Indirizzo.CssClass = txt_Indirizzo.CssClass.Replace("erroreValidazione", "");
            txt_Luogo.CssClass = txt_Luogo.CssClass.Replace("erroreValidazione", "");
            txt_CodiceLavoro.CssClass = txt_CodiceLavoro.CssClass.Replace("erroreValidazione", "");
            tb_Nota.CssClass = tb_Nota.CssClass.Replace("erroreValidazione", "");
        }


        private void AttivaDisattivaModifica(bool attivaModifica)
        {
            val_DataInizioLavorazione.Visible = !attivaModifica;
            txt_DataInizioLavorazione.Visible = attivaModifica;
            val_FineLavorazione.Visible = !attivaModifica;
            txt_FineLavorazione.Visible = attivaModifica;
            val_DurataLavorazione.Visible = !attivaModifica;
            txt_DurataLavorazione.Visible = attivaModifica;
            val_Risorse.Visible = !attivaModifica;
            ddl_Risorse.Visible = attivaModifica;
            val_Tipologia.Visible = !attivaModifica;
            ddl_Tipologia.Visible = attivaModifica;
            val_cliente.Visible = !attivaModifica;
            ddl_cliente.Visible = attivaModifica;
            val_DurataViaggioAndata.Visible = !attivaModifica;
            txt_DurataViaggioAndata.Visible = attivaModifica;
            val_DurataViaggioRitorno.Visible = !attivaModifica;
            txt_DurataViaggioRitorno.Visible = attivaModifica;
            val_DataInizioImpegno.Visible = !attivaModifica;
            txt_DataInizioImpegno.Visible = attivaModifica;
            val_DataFineImpegno.Visible = !attivaModifica;
            txt_DataFineImpegno.Visible = attivaModifica;
            val_ImpegnoOrario.Visible = !attivaModifica;
            chk_ImpegnoOrario.Visible = attivaModifica;
            val_ImpegnoOrarioDa.Visible = !attivaModifica;
            txt_ImpegnoOrarioDa.Visible = attivaModifica;
            val_ImpegnoOrarioA.Visible = !attivaModifica;
            txt_ImpegnoOrarioA.Visible = attivaModifica;
            val_Produzione.Visible = !attivaModifica;
            txt_Produzione.Visible = attivaModifica;
            val_Lavorazione.Visible = !attivaModifica;
            txt_Lavorazione.Visible = attivaModifica;
            val_Indirizzo.Visible = !attivaModifica;
            txt_Indirizzo.Visible = attivaModifica;
            val_Luogo.Visible = !attivaModifica;
            txt_Luogo.Visible = attivaModifica;
            val_CodiceLavoro.Visible = !attivaModifica;
            txt_CodiceLavoro.Visible = attivaModifica;
            val_Nota.Visible = !attivaModifica;
            tb_Nota.Visible = attivaModifica;

            btnModifica.Visible = !attivaModifica;
            btnSalva.Visible = btnAnnulla.Visible = attivaModifica;

            upEvento.Update();
        }

        

        private void popolaPopupEventi(DatiAgenda evento)
        {
            Esito esito = new Esito();

            hf_idEvento.Value = evento.id.ToString();

            val_DataInizioLavorazione.Text = evento.data_inizio_lavorazione.ToString("dd/MM/yyyy");
            val_FineLavorazione.Text = evento.data_fine_lavorazione.ToString("dd/MM/yyyy");
            val_DurataLavorazione.Text = evento.durata_lavorazione.ToString();
            val_Risorse.Text = UtilityTipologiche.getElementByID(listaRisorse, evento.id_colonne_agenda, ref esito).nome;
            val_Tipologia.Text = UtilityTipologiche.getElementByID(listaTipiTipologie, evento.id_tipologia, ref esito).nome;
            val_cliente.Text = evento.id_cliente.ToString();
            val_DurataViaggioAndata.Text = evento.durata_viaggio_andata.ToString();
            val_DurataViaggioRitorno.Text = evento.durata_viaggio_ritorno.ToString();
            val_DataInizioImpegno.Text = evento.data_inizio_impegno.ToString();
            val_DataFineImpegno.Text = evento.data_fine_impegno.ToString();
            val_ImpegnoOrario.Text = evento.impegnoOrario ? "1" : "0";
            val_ImpegnoOrarioDa.Text = evento.impegnoOrario_da.ToString();
            val_ImpegnoOrarioA.Text = evento.impegnoOrario_a.ToString();
            val_Produzione.Text = evento.produzione;
            val_Lavorazione.Text = evento.lavorazione;
            val_Indirizzo.Text = evento.indirizzo;
            val_Luogo.Text = evento.luogo;
            val_CodiceLavoro.Text = evento.codice_lavoro;
            val_Nota.Text = evento.nota;

            txt_DataInizioLavorazione.Text = evento.data_inizio_lavorazione.ToString("dd/MM/yyyy");
            txt_FineLavorazione.Text = evento.data_fine_lavorazione.ToString("dd/MM/yyyy");
            txt_DurataLavorazione.Text = evento.durata_lavorazione.ToString();
            ddl_Risorse.SelectedValue = evento.id_colonne_agenda.ToString();
            ddl_Tipologia.SelectedValue = evento.id_tipologia.ToString();
            ddl_cliente.SelectedValue = evento.id_cliente.ToString();
            txt_DurataViaggioAndata.Text = evento.durata_viaggio_andata.ToString();
            txt_DurataViaggioRitorno.Text = evento.durata_viaggio_ritorno.ToString();
            txt_DataInizioImpegno.Text = evento.data_inizio_impegno.ToString();
            txt_DataFineImpegno.Text = evento.data_fine_impegno.ToString();
            chk_ImpegnoOrario.Checked = evento.impegnoOrario;
            txt_ImpegnoOrarioDa.Text = evento.impegnoOrario_da.ToString();      
            txt_ImpegnoOrarioA.Text = evento.impegnoOrario_a.ToString();
            txt_Produzione.Text = evento.produzione;
            txt_Lavorazione.Text = evento.lavorazione;
            txt_Indirizzo.Text = evento.indirizzo;
            txt_Luogo.Text = evento.luogo;
            txt_CodiceLavoro.Text = evento.codice_lavoro;
            tb_Nota.Text = evento.nota;

            txt_ImpegnoOrarioDa.Enabled = txt_ImpegnoOrarioA.Enabled = evento.impegnoOrario;
        }

        //private DatiAgenda costruisciEvento()
        //{
        //    DatiAgenda evento = new DatiAgenda();
        //    evento.data_inizio_lavorazione = DateTime.Parse(txt_DataInizioLavorazione.Text);
        //    evento.data_fine_lavorazione = DateTime.Parse(txt_FineLavorazione.Text);
        //    evento.durata_lavorazione= int.Parse(txt_DurataLavorazione.Text);
        //    evento.id_colonne_agenda = int.Parse(ddl_Risorse.SelectedValue);
        //    evento.id_tipologia = int.Parse(ddl_Tipologia.SelectedValue);
        //    evento.id_cliente = int.Parse(ddl_cliente.SelectedValue);
        //    evento.durata_viaggio_andata = int.Parse(txt_DurataViaggioAndata.Text);
        //    evento.durata_viaggio_ritorno = int.Parse(txt_DurataViaggioRitorno.Text);
        //    evento.data_inizio_impegno = DateTime.Parse(txt_DataInizioImpegno.Text);
        //    evento.data_fine_impegno = DateTime.Parse(txt_DataFineImpegno.Text);
        //    evento.impegnoOrario = chk_ImpegnoOrario.Checked;
        //    evento.impegnoOrario_da = DateTime.Parse(txt_ImpegnoOrarioDa.Text);
        //    evento.impegnoOrario_a = DateTime.Parse(txt_ImpegnoOrarioA.Text);
        //    evento.produzione = txt_Produzione.Text;
        //    evento.lavorazione = txt_Lavorazione.Text;
        //    evento.indirizzo = txt_Indirizzo.Text;
        //    evento.luogo = txt_Luogo.Text;
        //    evento.codice_lavoro = txt_CodiceLavoro.Text;
        //    evento.nota = tb_Nota.Text;

        //    return evento;
        //}
        private void clearPopupEventi()
        {
            val_DataInizioLavorazione.Text = string.Empty;
            val_FineLavorazione.Text = string.Empty;
            val_DurataLavorazione.Text = string.Empty;
            val_Risorse.Text = string.Empty;
            val_Tipologia.Text = string.Empty;
            val_cliente.Text = string.Empty;
            val_DurataViaggioAndata.Text = string.Empty;
            val_DurataViaggioRitorno.Text = string.Empty;
            val_DataInizioImpegno.Text = string.Empty;
            val_DataFineImpegno.Text = string.Empty;
            val_ImpegnoOrario.Text = string.Empty;
            val_ImpegnoOrarioDa.Text = string.Empty;
            val_ImpegnoOrarioA.Text = string.Empty;
            val_Produzione.Text = string.Empty;
            val_Lavorazione.Text = string.Empty;
            val_Indirizzo.Text = string.Empty;
            val_Luogo.Text = string.Empty;
            val_CodiceLavoro.Text = string.Empty;
            val_Nota.Text = string.Empty;

            txt_DataInizioLavorazione.Text = string.Empty;
            txt_FineLavorazione.Text = string.Empty;
            txt_DurataLavorazione.Text = string.Empty;
            ddl_Risorse.SelectedValue = "0";
            ddl_Tipologia.SelectedValue = "0";
            ddl_cliente.SelectedValue = "0";
            txt_DurataViaggioAndata.Text = string.Empty;
            txt_DurataViaggioRitorno.Text = string.Empty;
            txt_DataInizioImpegno.Text = string.Empty;
            txt_DataFineImpegno.Text = string.Empty;
            chk_ImpegnoOrario.Checked = false;
            txt_ImpegnoOrarioDa.Text = string.Empty;
            txt_ImpegnoOrarioA.Text = string.Empty;
            txt_Produzione.Text = string.Empty;
            txt_Lavorazione.Text = string.Empty;
            txt_Indirizzo.Text = string.Empty;
            txt_Luogo.Text = string.Empty;
            txt_CodiceLavoro.Text = string.Empty;
            tb_Nota.Text = string.Empty;
        }

        private void aggiornaAgenda()
        {
            gv_scheduler.DataSource = CreateDataTable(DateTime.Parse(hf_valoreData.Value));
            gv_scheduler.DataBind();

            ScriptManager.RegisterStartupScript(this, typeof(Page), "passaggioMouse", "registraPassaggioMouse();", true);
        }
    }
}