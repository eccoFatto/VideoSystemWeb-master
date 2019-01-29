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
                gv_scheduler.DataSource = CreateDataTable(dataPartenza);
                gv_scheduler.DataBind();

                popolaDDLTipologiche(ddl_Risorse, listaRisorse);
                popolaDDLTipologiche(ddl_Tipologia, listaTipiTipologie);
            }
        }

        private void popolaDDLTipologiche(DropDownList ddl, List<Tipologica> listaTipologica)
        {
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
                    List<DatiAgenda> datiAgendaFiltrati = listaDatiAgenda.Where(x => x.data_inizio_lavorazione <= dataRiga && x.data_fine_lavorazione >= dataRiga && x.id_colonne_agenda == risorsa.id).ToList<DatiAgenda>();
                    if (datiAgendaFiltrati.Count == 1)
                    {
                        DatiAgenda datoCorrente = datiAgendaFiltrati.FirstOrDefault();

                        row[indiceColonna++] = datoCorrente.id.ToString(); // inserisco id datoAgenda per poi formattare la cella in RowDataBound 
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
                        e.Row.Cells[indiceColonna].Attributes.Add("style", "font-weight:bold;background-color:" + colore);
                        //e.Row.Cells[indiceColonna].CssClass = "cella";
                    }
                    else
                    {
                        //e.Row.Cells[indiceColonna].CssClass = "cellaVuota";
                    }

                    string data = e.Row.Cells[0].Text;
                    string risorsa = ((Tipologica)listaRisorse.ElementAt(indiceColonna - 1)).nome;

                    e.Row.Cells[indiceColonna].Attributes["onclick"] = "mostracella('" + data + "', '" + risorsa + "');";
                }

                e.Row.Attributes.Add("style", "text-align:center;");
            }
            #endregion
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            gv_scheduler.DataSource = CreateDataTable(DateTime.Parse(hf_valoreData.Value));
            gv_scheduler.DataBind();

            ScriptManager.RegisterStartupScript(this, typeof(Page), "passaggioMouse", "registraPassaggioMouse();", true);
        }

        protected void btnModifica_Click(object sender, EventArgs e)
        {
            lbl_MessaggioErrore.Visible = false;

            txt_DataInizioLavorazione.Text = val_DataInizioLavorazione.Text;
            txt_FineLavorazione.Text = val_FineLavorazione.Text;
            txt_DurataLavorazione.Text = val_DurataLavorazione.Text;
            //ddl_Risorse.SelectedValue = val_Risorse.Text;
            //ddl_Tipologia.SelectedValue = val_Tipologia.Text;
            //ddl_cliente.SelectedValue = val_cliente.Text;
            txt_DurataViaggioAndata.Text = val_DurataViaggioAndata.Text;
            txt_DurataViaggioRitorno.Text = val_DurataViaggioRitorno.Text;
            txt_DataInizioImpegno.Text = val_DataInizioImpegno.Text;
            txt_DataFineImpegno.Text = val_DataFineImpegno.Text;
            txt_ImpegnoOrarioDa.Text = val_ImpegnoOrarioDa.Text;
            txt_ImpegnoOrarioA.Text = val_ImpegnoOrarioA.Text;
            txt_Produzione.Text = val_Produzione.Text;
            txt_Lavorazione.Text = val_Lavorazione.Text;
            txt_Indirizzo.Text = val_Indirizzo.Text;
            txt_Luogo.Text = val_Luogo.Text;
            txt_CodiceLavoro.Text = val_CodiceLavoro.Text;

            tb_Nota.Text = val_Nota.Text;

            AttivaDisattivaModifica(true);
        }

        protected void btnSalva_Click(object sender, EventArgs e)
        {
            Esito esito = new Esito();

            DatiAgenda datiAgenda = new DatiAgenda();
            datiAgenda.data_inizio_lavorazione = validaCampo(txt_DataInizioLavorazione, DateTime.Now, true, ref esito);// DateTime.Parse(txt_DataInizioLavorazione.Text);
            datiAgenda.data_fine_lavorazione = validaCampo(txt_FineLavorazione, DateTime.Now, true, ref esito); //DateTime.Parse(txt_FineLavorazione.Text);
            datiAgenda.durata_lavorazione = validaCampo(txt_DurataLavorazione, 0, true, ref esito); //int.Parse(txt_DurataLavorazione.Text);
            datiAgenda.id_colonne_agenda = validaCampo(ddl_Risorse, 0, true, ref esito); //int.Parse(ddl_Risorse.SelectedValue);
            datiAgenda.id_tipologia = validaCampo(ddl_Tipologia, 0, false, ref esito);
            datiAgenda.id_cliente = validaCampo(ddl_cliente, 0, false, ref esito); //int.Parse(ddl_cliente.SelectedValue);
            datiAgenda.durata_viaggio_andata = validaCampo(txt_DurataViaggioAndata, 0, false, ref esito); //int.Parse(txt_DurataViaggioAndata.Text);
            datiAgenda.durata_viaggio_ritorno = validaCampo(txt_DurataViaggioRitorno, 0, false, ref esito); //int.Parse(txt_DurataViaggioRitorno.Text);
            datiAgenda.data_inizio_impegno = validaCampo(txt_DataInizioImpegno, DateTime.Now, false, ref esito); //DateTime.Parse(txt_DataInizioImpegno.Text);
            datiAgenda.data_fine_impegno = validaCampo(txt_DataFineImpegno, DateTime.Now, false, ref esito); //DateTime.Parse(txt_DataFineImpegno.Text);
            datiAgenda.impegnoOrario = chk_ImpegnoOrario.Checked;
            datiAgenda.impegnoOrario_da = validaCampo(txt_ImpegnoOrarioDa, DateTime.Now, chk_ImpegnoOrario.Checked, ref esito); //DateTime.Parse(txt_ImpegnoOrarioDa.Text);
            datiAgenda.impegnoOrario_a = validaCampo(txt_ImpegnoOrarioA, DateTime.Now, chk_ImpegnoOrario.Checked, ref esito); //DateTime.Parse(txt_ImpegnoOrarioA.Text);
            datiAgenda.produzione = validaCampo(txt_Produzione, "", true, ref esito); //txt_Produzione.Text;
            datiAgenda.lavorazione = validaCampo(txt_Lavorazione, "", true, ref esito); //txt_Lavorazione.Text;
            datiAgenda.indirizzo = validaCampo(txt_Indirizzo, "", false, ref esito); //txt_Indirizzo.Text;
            datiAgenda.luogo = validaCampo(txt_Luogo, "", false, ref esito); //txt_Luogo.Text;
            datiAgenda.codice_lavoro = validaCampo(txt_CodiceLavoro, "", true, ref esito); //txt_CodiceLavoro.Text;
            datiAgenda.nota = validaCampo(tb_Nota, "", false, ref esito); //tb_Nota.Text;

            if (esito.codice != Esito.ESITO_OK)
            {
                lbl_MessaggioErrore.Visible = true;
                upEvento.Update();
            }
            else
            {
                nascondiErroriValidazione();
                ScriptManager.RegisterStartupScript(this, typeof(Page), "closePopup", "chiudiPopup();", true);
            }

        }



        protected void btnAnnulla_Click(object sender, EventArgs e)
        {

            nascondiErroriValidazione();
            AttivaDisattivaModifica(false);
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

        protected void btnEditEvent_Click(object sender, EventArgs e)
        {
            string dataEvento = hf_data.Value;
            string risorsaEvento = hf_risorsa.Value;

            val_DataInizioLavorazione.Text = dataEvento;
            val_Risorse.Text = risorsaEvento;
            pnlContainer.Visible = true;
        }

        protected void btn_chiudi_Click(object sender, EventArgs e)
        {

            pnlContainer.Visible = false;
        }
    }
}