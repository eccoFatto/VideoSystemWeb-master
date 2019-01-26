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

                popolaListaRisorse();
            }
        }

        private void popolaListaRisorse()
        {
            ddl_Risorse.DataSource = listaRisorse;
            ddl_Risorse.DataTextField = "nome";
            ddl_Risorse.DataValueField = "id";
            ddl_Risorse.DataBind();
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
            AttivaDisattivaModifica(true);
        }

        protected void btnSalva_Click(object sender, EventArgs e)
        {
            DatiAgenda datiAgenda = new DatiAgenda();
            datiAgenda.data_inizio_lavorazione = DateTime.Parse(txt_DataInizioLavorazione.Text);
            datiAgenda.data_fine_lavorazione = DateTime.Parse(txt_FineLavorazione.Text);
            datiAgenda.durata_lavorazione = int.Parse(txt_DurataLavorazione.Text);
            datiAgenda.id_colonne_agenda = int.Parse(ddl_Risorse.SelectedValue);
            datiAgenda.id_cliente = int.Parse(ddl_cliente.SelectedValue);
            datiAgenda.durata_viaggio_andata = int.Parse(txt_DurataViaggioAndata.Text);
            datiAgenda.durata_viaggio_ritorno = int.Parse(txt_DurataViaggioRitorno.Text);
            datiAgenda.data_inizio_impegno = DateTime.Parse(txt_DataInizioImpegno.Text);
            datiAgenda.data_fine_impegno = DateTime.Parse(txt_DataFineImpegno.Text);
            datiAgenda.impegnoOrario = chk_ImpegnoOrario.Checked;
            datiAgenda.impegnoOrario_da = DateTime.Parse(txt_ImpegnoOrarioDa.Text);
            datiAgenda.impegnoOrario_a = DateTime.Parse(txt_ImpegnoOrarioA.Text);
            datiAgenda.produzione = txt_Produzione.Text;
            datiAgenda.lavorazione = txt_Lavorazione.Text;
            datiAgenda.indirizzo = txt_Indirizzo.Text;
            datiAgenda.luogo = txt_Luogo.Text;
            datiAgenda.codice_lavoro = txt_CodiceLavoro.Text;
            datiAgenda.nota = tb_Nota.Text;
        }

        protected void btnAnnulla_Click(object sender, EventArgs e)
        {
            AttivaDisattivaModifica(false);
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
    }
}