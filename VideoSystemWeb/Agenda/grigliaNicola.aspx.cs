using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoSystemWeb.BLL;
using VideoSystemWeb.Entity;

namespace VideoSystemWeb
{
    public partial class grigliaNicola : System.Web.UI.Page
    {
        List<DatiAgenda> listaDatiAgenda = Tipologie.getListaDatiAgenda();
        List<Tipologica> listaRisorse = Tipologie.getListaRisorse();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DateTime dataPartenza = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                gv_scheduler.DataSource = CreateDataTable(dataPartenza);
                gv_scheduler.DataBind();
            }
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
                    List<DatiAgenda> datiAgendaFiltrati = listaDatiAgenda.Where(x => x.data_inizio <= dataRiga && x.data_fine >= dataRiga && x.id_risorsa == risorsa.id).ToList<DatiAgenda>();
                    if (datiAgendaFiltrati.Count == 1)
                    {
                        DatiAgenda datoCorrente = datiAgendaFiltrati.FirstOrDefault();
                       
                        row[indiceColonna++] = datoCorrente.id.ToString(); // inserisco id datoAgenda per poi formattare la cella in RowDataBound 
                    }
                    else
                    {
                        row[indiceColonna++] = " ";
                    }

                    //row[indiceColonna++] = listaDatiAgenda.Where(x => x.data_inizio <= dataRiga && x.data_fine >= dataRiga && x.id_risorsa == risorsa.id).Count() > 0 ? "X" : " ";

                }
                table.Rows.Add(row);
            }
            #endregion
            
            return table;
        }

        protected void gv_scheduler_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int indiceColonna = 1; indiceColonna <= listaRisorse.Count; indiceColonna++)
                {
                    string idRisorsa = (e.Row.Cells[indiceColonna].Text.Trim());
                    
                    Tipologica risorsaCorrente = Tipologie.getRisorsaById(int.Parse(idRisorsa));
                    string colore = Utility.getParametroDaTipologica(risorsaCorrente, "color");
                    e.Row.Cells[indiceColonna].Attributes.Add("style", "background-color:" + colore + ";font-size:10pt;text-align:center;");
                    e.Row.Cells[indiceColonna].Text = risorsaCorrente.nome;
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Attributes.Add("style", "font-weight:bold;background-color:#FDEDB5;");

                for (int indiceColonna = 1; indiceColonna <= listaRisorse.Count; indiceColonna++)
                {
                    if (!string.IsNullOrEmpty(e.Row.Cells[indiceColonna].Text.Trim()))
                    {
                        DatiAgenda datoAgendaCorrente = Tipologie.getDatiAgendaById(int.Parse(e.Row.Cells[indiceColonna].Text.Trim()));
                        string colore = Utility.getParametroDaTipologica(Tipologie.getStatoById(datoAgendaCorrente.id_stato), "color");
                        string descrizione = datoAgendaCorrente.descrizione;

                        e.Row.Cells[indiceColonna].Text = descrizione;
                        e.Row.Cells[indiceColonna].Attributes.Add("style", "font-weight:bold;background-color:" + colore);
                    }
                }
                
                e.Row.Attributes.Add("style", "text-align:center;");
            }
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            gv_scheduler.DataSource = CreateDataTable(DateTime.Parse(hf_valoreData.Value));
            gv_scheduler.DataBind();
        }
    }
}