using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoSystemWeb.BLL;
using VideoSystemWeb.Entity;

using iText;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iTextSharp;

using System.Configuration;
namespace VideoSystemWeb.REPORT
{
    public partial class StampaConsulenteLavoro : BasePage
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            CheckIsMobile();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnStampa.CssClass = "w3-btn w3-white w3-border w3-border-blue w3-round-large w3-disabled";
                CaricaCombo();
            }

            #region GRIGLIA CON RAGGRUPPAMENTO RIGHE
            GridViewHelper helper = new GridViewHelper(this.gv_DatiStampa);

            string[] cols = new string[4];
            cols[0] = "IndirizzoCollaboratore";
            cols[1] = "CittaCollaboratore";
            cols[2] = "TelefonoCollaboratore";
            cols[3] = "CodFiscaleCollaboratore";

            helper.RegisterGroup("NomeCollaboratore", true, true);
            helper.RegisterGroup(cols, true, true);
            helper.GroupHeader += new GroupEvent(Helper_GroupHeader);
            helper.GroupSummary += new GroupEvent(Helper_GroupSummary);
            //helper.GeneralSummary += new FooterEvent(Helper_GeneralSummary);

            //SUBTOTALE
            helper.RegisterSummary("Mista", SummaryOperation.Sum, "NomeCollaboratore");
            helper.RegisterSummary("Assunzione", SummaryOperation.Sum, "NomeCollaboratore");
            helper.RegisterSummary("RimborsoKm", SummaryOperation.Sum, "NomeCollaboratore");
            helper.RegisterSummary("Diaria", SummaryOperation.Sum, "NomeCollaboratore");
            helper.RegisterSummary("Albergo", SummaryOperation.Sum, "NomeCollaboratore");

            //TOTALE
            //helper.RegisterSummary("Mista", SummaryOperation.Sum);
            //helper.RegisterSummary("Assunzione", SummaryOperation.Sum);
            //helper.RegisterSummary("RimborsoKm", SummaryOperation.Sum);
            //helper.RegisterSummary("Diaria", SummaryOperation.Sum);

            helper.ApplyGroupSort();
            #endregion

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "chiudiLoader", script: "$('.loader').hide();", addScriptTags: true);
        }

        private void CaricaCombo()
        {
            for (var i = DateTime.Now.Year; i >=  DateTime.Now.Year - 10; i--)
            {
                ddl_Anno.Items.Add(new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString()));
            }
        }

        protected void btnRicerca_Click(object sender, EventArgs e)
        {
            Esito esito = new Esito();
            DateTime dataInizio = DateTime.Parse(txt_DataInizio.Text);
            DateTime dataFine = DateTime.Parse(txt_DataFine.Text);
            string nominativo = txt_Nominativo.Text;

            

            List<DatiReportRaw> listaDatiReport = Report_BLL.Instance.GetListaDatiReportRawConsulenteLavoro(dataInizio, dataFine, nominativo, ref esito);

            gv_DatiStampa.DataSource = listaDatiReport;
            gv_DatiStampa.DataBind();


            if (listaDatiReport.Count > 0)
            {
                btnStampa.CssClass = btnStampa.CssClass.Replace("w3-disabled", "");

                lbl_TotAssunzione.Text = string.Format("{0:C}", decimal.Parse(listaDatiReport.Sum(x=>x.Assunzione).ToString()));
                lbl_TotMista.Text = string.Format("{0:C}", decimal.Parse(listaDatiReport.Sum(x => x.Mista).ToString()));
                lbl_TotRimbKm.Text = string.Format("{0:C}", decimal.Parse(listaDatiReport.Sum(x => x.RimborsoKm).ToString()));
                lbl_TotDiaria.Text = listaDatiReport.Sum(x => x.Diaria).ToString();
                lbl_TotAlbergo.Text = listaDatiReport.Sum(x => x.Albergo).ToString();
            }
            else
            {
                btnStampa.CssClass = "w3-btn w3-white w3-border w3-border-blue w3-round-large w3-disabled";

                lbl_TotAssunzione.Text = "-";
                lbl_TotMista.Text = "-";
                lbl_TotRimbKm.Text = "-";
                lbl_TotDiaria.Text = "-";
                lbl_TotAlbergo.Text = "-";
            }

            #region VECCHIA GESTIONE
            //List<DatiFiscaliLavorazione> listaDatiFiscaliLavorazione = new List<DatiFiscaliLavorazione>();
            //foreach (DatiReport datiReport in listaDatiReport)
            //{
            //    listaDatiFiscaliLavorazione.AddRange(datiReport.ListaDatiFiscali);
            //}

            //gv_DatiStampa.DataSource = listaDatiFiscaliLavorazione;
            //gv_DatiStampa.DataBind();

            //if (listaDatiFiscaliLavorazione.Count > 0)
            //{
            //    btnStampa.CssClass = btnStampa.CssClass.Replace("w3-disabled", "");
            //}
            //else
            //{
            //    btnStampa.CssClass = "w3-btn w3-white w3-border w3-border-blue w3-round-large w3-disabled";
            //}
            #endregion
        }

        protected void gv_DatiStampa_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_DatiStampa.PageIndex = e.NewPageIndex;
            btnRicerca_Click(null, null);
        }

        protected void gv_DatiStampa_OnSorting(object sender, EventArgs e)
        {
            // do something here or do nothing
        }

        private void Helper_GroupHeader(string groupName, object[] values, GridViewRow row)
        {
            if (groupName == "NomeCollaboratore")
            {
                row.BackColor = Color.FromArgb(0, 64, 128);
                row.ForeColor = Color.White;
            }
            else
            {
                row.BackColor = Color.LightGray;
            }
            row.Cells[0].Text = "&nbsp;&nbsp;<b>" + row.Cells[0].Text + "</b>";
        }

        private void Helper_GroupSummary(string groupName, object[] values, GridViewRow row)
        {
            row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
            row.Cells[0].Text = "<b><i>Subtotale</i></b>";
            row.Cells[1].Text = "<b><i>" + row.Cells[1].Text + "</i></b>";
            row.Cells[2].Text = "<b><i>" + row.Cells[2].Text + "</i></b>";
            row.Cells[3].Text = "<b><i>" + row.Cells[3].Text + "</i></b>";
            row.Cells[4].Text = "<b><i>" + row.Cells[4].Text + "</i></b>";
            row.Cells[5].Text = "<b><i>" + row.Cells[5].Text + "</i></b>";
        }

        //private void Helper_GeneralSummary(GridViewRow row)
        //{
        //    row.BackColor = Color.Gray;
        //    row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
        //    row.Cells[0].Text = "<b>Totale</b>";
        //    row.Cells[1].Text = "<b>" + row.Cells[1].Text + "</b>";
        //    row.Cells[2].Text = "<b>" + row.Cells[2].Text + "</b>";
        //    row.Cells[3].Text = "<b>" + row.Cells[3].Text + "</b>";
        //    row.Cells[4].Text = "<b>" + row.Cells[4].Text + "</b>";
        //}

        protected void btnStampa_Click(object sender, EventArgs e)
        {
            try
            {
                Esito esito = new Esito();
                DateTime dataInizio = DateTime.Parse(txt_DataInizio.Text);
                DateTime dataFine = DateTime.Parse(txt_DataFine.Text);
                string nominativo = txt_Nominativo.Text;

                List<DatiReport> listaDatiReport = Report_BLL.Instance.GetListaDatiReportConsulenteLavoro(dataInizio, dataFine, nominativo, ref esito);
                if (esito.Codice==0 && listaDatiReport!=null && listaDatiReport.Count > 0)
                {
                    // LEGGO I PARAMETRI DI VS
                    Config cfAppo = Config_BLL.Instance.getConfig(ref esito, "PARTITA_IVA");
                    string pIvaVs = cfAppo.valore;
                    cfAppo = Config_BLL.Instance.getConfig(ref esito, "DENOMINAZIONE");
                    string denominazioneVs = cfAppo.valore;
                    cfAppo = Config_BLL.Instance.getConfig(ref esito, "TOPONIMO");
                    string toponimoVs = cfAppo.valore;
                    cfAppo = Config_BLL.Instance.getConfig(ref esito, "INDIRIZZO");
                    string indirizzoVs = cfAppo.valore;
                    cfAppo = Config_BLL.Instance.getConfig(ref esito, "CIVICO");
                    string civicoVs = cfAppo.valore;
                    cfAppo = Config_BLL.Instance.getConfig(ref esito, "CAP");
                    string capVs = cfAppo.valore;
                    cfAppo = Config_BLL.Instance.getConfig(ref esito, "CITTA");
                    string cittaVs = cfAppo.valore;
                    cfAppo = Config_BLL.Instance.getConfig(ref esito, "PROVINCIA");
                    string provinciaVs = cfAppo.valore;
                    cfAppo = Config_BLL.Instance.getConfig(ref esito, "EMAIL");
                    string emailVs = cfAppo.valore;

                    // export SU PDF

                    string nomeFile = "Report_Cons_Lavoro.pdf";
                    string pathReport = ConfigurationManager.AppSettings["PATH_DOCUMENTI_REPORT"] + nomeFile;
                    string mapPathReport = MapPath(ConfigurationManager.AppSettings["PATH_DOCUMENTI_REPORT"]) + nomeFile;

                    iText.IO.Image.ImageData imageData = iText.IO.Image.ImageDataFactory.Create(MapPath("~/Images/logoVSP_trim.png"));


                    PdfWriter wr = new PdfWriter(mapPathReport);
                    PdfDocument doc = new PdfDocument(wr);
                    doc.SetDefaultPageSize(iText.Kernel.Geom.PageSize.A4.Rotate());
                    //doc.SetDefaultPageSize(iText.Kernel.Geom.PageSize.A4);
                    Document document = new Document(doc, iText.Kernel.Geom.PageSize.A4.Rotate(), false);
                
                    document.SetMargins(50, 30, 50, 30);

                    //iText.Kernel.Colors.Color coloreIntestazioni = new iText.Kernel.Colors.DeviceRgb(0, 225, 0);
                    // COLORE BLU VIDEOSYSTEM
                    iText.Kernel.Colors.Color coloreIntestazioni = new iText.Kernel.Colors.DeviceRgb(33, 150, 243);

                    //CICLO LISTA DATI REPORT
                    int totCollaboratoriUtilizzati = 0;

                    foreach (DatiReport collaboratore  in listaDatiReport)
                    {
                        totCollaboratoriUtilizzati++;
                        // AGGIUNGO TABLE PER LAYOUT INTESTAZIONE
                        iText.Layout.Element.Table tbIntestazione = new iText.Layout.Element.Table(new float[] { 1, 9 }).UseAllAvailableWidth().SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        iText.Layout.Element.Image image = new iText.Layout.Element.Image(imageData).ScaleAbsolute(100, 70).SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
                        Cell cellaImmagine = new Cell().SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE).SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
                        cellaImmagine.Add(image);
                        tbIntestazione.AddCell(cellaImmagine);

                        iText.Layout.Element.Table tbIntestazioneDx = new iText.Layout.Element.Table(new float[] { 4, 6 }).UseAllAvailableWidth().SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                        Paragraph pTitolo = new Paragraph("Nome").SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        Paragraph pValore = new Paragraph(collaboratore.NomeCollaboratore).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE);
                        tbIntestazioneDx.AddCell(pValore).SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                        pTitolo = new Paragraph("Qualifica").SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        pValore = new Paragraph(collaboratore.QualificaCollaboratore).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE);
                        tbIntestazioneDx.AddCell(pValore).SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                        pTitolo = new Paragraph("Indirizzo").SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        pValore = new Paragraph(collaboratore.IndirizzoCollaboratore).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE);
                        tbIntestazioneDx.AddCell(pValore).SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                        pTitolo = new Paragraph("Città").SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        pValore = new Paragraph(collaboratore.CittaCollaboratore).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE);
                        tbIntestazioneDx.AddCell(pValore).SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                        pTitolo = new Paragraph("Telefono").SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        if (collaboratore.TelefonoCollaboratore != null) {
                            pValore = new Paragraph(collaboratore.TelefonoCollaboratore).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE);
                        }
                        else
                        {
                            pValore = new Paragraph("").SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE);
                        }
                        tbIntestazioneDx.AddCell(pValore).SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                        pTitolo = new Paragraph("Cod.Fiscale/P.Iva").SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        pValore = new Paragraph(collaboratore.CodFiscaleCollaboratore).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE);
                        tbIntestazioneDx.AddCell(pValore).SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                        // AGGIUNGO INTESTAZIONE COLLABORATORE
                        tbIntestazione.AddCell(tbIntestazioneDx).SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                        document.Add(tbIntestazione);

                        Paragraph pSpazio = new Paragraph(" ");
                        document.Add(pSpazio);

                        Paragraph pLuogoData = new Paragraph(cittaVs + ", " + DateTime.Today.ToLongDateString());
                        document.Add(pLuogoData);

                        document.Add(pSpazio);

                        // CREAZIONE GRIGLIA
                        iText.Layout.Element.Table tbGrigla = new iText.Layout.Element.Table(new float[] { 60, 123, 103, 108, 107, 30, 65, 50, 50, 38, 46 }).SetWidth(780).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetFixedLayout();
                        Paragraph pGriglia;
                        Cell cellaGriglia;

                        // INTESTAZIONE GRIGLIA
                        pGriglia = new Paragraph("Data").SetFontSize(10);
                        cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddHeaderCell(cellaGriglia);

                        pGriglia = new Paragraph("Lavorazione").SetFontSize(10);
                        cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddHeaderCell(cellaGriglia);

                        pGriglia = new Paragraph("Produzione").SetFontSize(10);
                        cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddHeaderCell(cellaGriglia);

                        pGriglia = new Paragraph("Cliente").SetFontSize(10);
                        cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddHeaderCell(cellaGriglia);

                        pGriglia = new Paragraph("Descrizione").SetFontSize(10);
                        cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddHeaderCell(cellaGriglia);

                        pGriglia = new Paragraph("Qta").SetFontSize(10);
                        cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddHeaderCell(cellaGriglia);

                        pGriglia = new Paragraph("Assunzione").SetFontSize(10);
                        cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddHeaderCell(cellaGriglia);

                        pGriglia = new Paragraph("Mista").SetFontSize(10);
                        cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddHeaderCell(cellaGriglia);

                        pGriglia = new Paragraph("Rimb. KM").SetFontSize(10);
                        cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddHeaderCell(cellaGriglia);

                        pGriglia = new Paragraph("Diaria").SetFontSize(10);
                        cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddHeaderCell(cellaGriglia);

                        pGriglia = new Paragraph("Albergo").SetFontSize(10);
                        cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddHeaderCell(cellaGriglia);

                        // TOTALI PARZIALI
                        int quantita = 0;
                        decimal assunzione = 0;
                        decimal mista = 0;
                        decimal rimborsoKm = 0;
                        int diaria = 0;
                        int albergo = 0;

                        foreach (DatiFiscaliLavorazione datiFiscali in collaboratore.ListaDatiFiscali)
                        {
                            pGriglia = new Paragraph(datiFiscali.DataLavorazione.ToShortDateString()).SetFontSize(9);
                            cellaGriglia = new Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10);
                            cellaGriglia.Add(pGriglia);
                            tbGrigla.AddCell(cellaGriglia);

                            pGriglia = new Paragraph(datiFiscali.Lavorazione).SetFontSize(9);
                            cellaGriglia = new Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10);
                            cellaGriglia.Add(pGriglia);
                            tbGrigla.AddCell(cellaGriglia);

                            pGriglia = new Paragraph(datiFiscali.Produzione).SetFontSize(9);
                            cellaGriglia = new Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10);
                            cellaGriglia.Add(pGriglia);
                            tbGrigla.AddCell(cellaGriglia);

                            pGriglia = new Paragraph(datiFiscali.Cliente).SetFontSize(9);
                            cellaGriglia = new Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10);
                            cellaGriglia.Add(pGriglia);
                            tbGrigla.AddCell(cellaGriglia);

                            pGriglia = new Paragraph(datiFiscali.Descrizione).SetFontSize(9);
                            cellaGriglia = new Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10);
                            cellaGriglia.Add(pGriglia);
                            tbGrigla.AddCell(cellaGriglia);

                            quantita += datiFiscali.Quantita;
                            pGriglia = new Paragraph(datiFiscali.Quantita.ToString("###")).SetFontSize(9);
                            cellaGriglia = new Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                            cellaGriglia.Add(pGriglia);
                            tbGrigla.AddCell(cellaGriglia);

                            assunzione += datiFiscali.Assunzione;
                            pGriglia = new Paragraph(datiFiscali.Assunzione.ToString("###,##0.00")).SetFontSize(9);
                            cellaGriglia = new Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                            cellaGriglia.Add(pGriglia);
                            tbGrigla.AddCell(cellaGriglia);

                            mista += datiFiscali.Mista;
                            pGriglia = new Paragraph(datiFiscali.Mista.ToString("###,##0.00")).SetFontSize(9);
                            cellaGriglia = new Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                            cellaGriglia.Add(pGriglia);
                            tbGrigla.AddCell(cellaGriglia);

                            rimborsoKm += datiFiscali.RimborsoKm;
                            pGriglia = new Paragraph(datiFiscali.RimborsoKm.ToString("###,##0.00")).SetFontSize(9);
                            cellaGriglia = new Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                            cellaGriglia.Add(pGriglia);
                            tbGrigla.AddCell(cellaGriglia);

                            diaria += datiFiscali.Diaria;
                            pGriglia = new Paragraph(datiFiscali.Diaria.ToString("###")).SetFontSize(9);
                            cellaGriglia = new Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                            cellaGriglia.Add(pGriglia);
                            tbGrigla.AddCell(cellaGriglia);

                            albergo += datiFiscali.Albergo;
                            pGriglia = new Paragraph(datiFiscali.Albergo.ToString("###")).SetFontSize(9);
                            cellaGriglia = new Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                            cellaGriglia.Add(pGriglia);
                            tbGrigla.AddCell(cellaGriglia);
                        }

                        pGriglia = new Paragraph("TOTALI").SetFontSize(9);
                        cellaGriglia = new Cell(1,5).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        pGriglia = new Paragraph(quantita.ToString("##0")).SetFontSize(9);
                        cellaGriglia = new Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                    
                        pGriglia = new Paragraph(assunzione.ToString("###,##0.00")).SetFontSize(9);
                        cellaGriglia = new Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                    
                        pGriglia = new Paragraph(mista.ToString("###,##0.00")).SetFontSize(9);
                        cellaGriglia = new Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        pGriglia = new Paragraph(rimborsoKm.ToString("###,##0.00")).SetFontSize(9);
                        cellaGriglia = new Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        pGriglia = new Paragraph(diaria.ToString("##0")).SetFontSize(9);
                        cellaGriglia = new Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        pGriglia = new Paragraph(albergo.ToString("##0")).SetFontSize(9);
                        cellaGriglia = new Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        // AGGIUNGO TABELLA
                        document.Add(tbGrigla);
                    
                        if (totCollaboratoriUtilizzati < listaDatiReport.Count)
                        {
                            // AGGIUNGO SALTO PAGINA
                            document.Add(new AreaBreak());
                        }
                    }

                    int n = doc.GetNumberOfPages();
                    iText.Kernel.Geom.Rectangle pageSize = doc.GetPage(n).GetPageSize();

                    // AGGIUNGO CONTEGGIO PAGINE E FOOTER PER OGNI PAGINA
                    for (int i = 1; i <= n; i++)
                    {
                        //AGGIUNGO NUM.PAGINA
                        document.ShowTextAligned(new Paragraph("pagina " + i.ToString() + " di " + n.ToString()).SetFontSize(7),
                            pageSize.GetWidth() - 60, pageSize.GetHeight() - 20, i, iText.Layout.Properties.TextAlignment.CENTER, iText.Layout.Properties.VerticalAlignment.TOP, 0);
                        //AGGIUNGO FOOTER
                        document.ShowTextAligned(new Paragraph(denominazioneVs + " P.IVA " + pIvaVs + Environment.NewLine + "Sede legale: " + toponimoVs + " " + indirizzoVs + " " + civicoVs + " - " + capVs + " " + cittaVs + " " + provinciaVs + " e-mail: " + emailVs).SetFontSize(7).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER),
                            pageSize.GetWidth() / 2, 30, i, iText.Layout.Properties.TextAlignment.CENTER, iText.Layout.Properties.VerticalAlignment.TOP, 0);

                    }
                
                    // CHIUDO IL PDF
                    document.Flush();
                    document.Close();
                    wr.Close();

                    btnRicerca_Click(null, null);

                    // CREO STRINGA PER IL JAVASCRIPT DI VISUALIZZAZIONE
                    if (System.IO.File.Exists(mapPathReport))
                    {
                        Page page = HttpContext.Current.Handler as Page;
                        ScriptManager.RegisterStartupScript(page, page.GetType(), "apriPdf", script: "window.open('" + pathReport.Replace("~", "") + "')", addScriptTags: true);
                    }
                    else
                    {
                        esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                        esito.Descrizione = "Il File " + pathReport.Replace("~", "") + " non è stato creato correttamente!";
                        BasePage p = new BasePage();
                        p.ShowError(esito.Descrizione);
                    }
                }

            }
            catch (Exception ex)
            {
                BasePage p = new BasePage();
                p.ShowError(ex.Message + " " + ex.StackTrace);
            }
        }
    }
}
    
