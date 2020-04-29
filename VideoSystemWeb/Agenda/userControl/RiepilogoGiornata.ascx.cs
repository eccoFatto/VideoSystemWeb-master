using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoSystemWeb.BLL;
using VideoSystemWeb.BLL.Stampa;
using VideoSystemWeb.Entity;
using iText;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iTextSharp;
using System.Configuration;

namespace VideoSystemWeb.Agenda.userControl
{
    public partial class RiepilogoGiornata : System.Web.UI.UserControl
    {
        BasePage basePage = new BasePage();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(this.btnStampaGiornata);
            }
        }
        public Esito popolaPannelloGiornata()
        {
            Esito esito = new Esito();
            try
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

                // GESTIONE NOMI FILE PDF
                string nomeFile = "Giornata_" + tbDataElaborazione.Text.Replace("/","-") + ".pdf";
                string pathGiornata = ConfigurationManager.AppSettings["PATH_DOCUMENTI_PROTOCOLLO"] + nomeFile;
                string mapPathGiornata = MapPath(ConfigurationManager.AppSettings["PATH_DOCUMENTI_PROTOCOLLO"]) + nomeFile;

                iText.IO.Image.ImageData imageData = iText.IO.Image.ImageDataFactory.Create(MapPath("~/Images/logoVSP_trim.png"));

                iText.IO.Image.ImageData imageDNV = iText.IO.Image.ImageDataFactory.Create(MapPath("~/Images/DNV_2008_ITA2.jpg"));


                PdfWriter wr = new PdfWriter(mapPathGiornata);
                PdfDocument doc = new PdfDocument(wr);
                doc.SetDefaultPageSize(iText.Kernel.Geom.PageSize.A4);
                //Document document = new Document(doc);
                Document document = new Document(doc, iText.Kernel.Geom.PageSize.A4, false);

                document.SetMargins(245, 30, 110, 30);

                Paragraph pSpazio = new Paragraph(" ");
                document.Add(pSpazio);

                // CREAZIONE GRIGLIA
                iText.Layout.Element.Table tbGrigla = new iText.Layout.Element.Table(new float[] { 80, 80, 80, 80, 80, 80, 50 }).SetWidth(530).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetFixedLayout();
                Paragraph pGriglia;
                Cell cellaGriglia;

                // COLORE BLU VIDEOSYSTEM
                iText.Kernel.Colors.Color coloreIntestazioni = new iText.Kernel.Colors.DeviceRgb(33, 150, 243);

                // INTESTAZIONE FATTURA
                pGriglia = new Paragraph("colonna 1").SetFontSize(10).SetBold();
                cellaGriglia = new Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                cellaGriglia.Add(pGriglia);
                tbGrigla.AddHeaderCell(cellaGriglia);

                pGriglia = new Paragraph("colonna 2").SetFontSize(10).SetBold();
                cellaGriglia = new iText.Layout.Element.Cell(1,2).SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                cellaGriglia.Add(pGriglia);
                tbGrigla.AddHeaderCell(cellaGriglia);

                pGriglia = new Paragraph("colonna 3").SetFontSize(10).SetBold();
                cellaGriglia = new iText.Layout.Element.Cell(1, 4).SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                cellaGriglia.Add(pGriglia);
                tbGrigla.AddHeaderCell(cellaGriglia);

                // INTESTAZIONE GRIGLIA
                pGriglia = new Paragraph("int1").SetFontSize(10);
                cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetPadding(5).SetBold();
                cellaGriglia.Add(pGriglia);
                tbGrigla.AddHeaderCell(cellaGriglia);

                pGriglia = new Paragraph("int2.3").SetFontSize(10);
                cellaGriglia = new iText.Layout.Element.Cell(1, 2).SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetBold();
                cellaGriglia.Add(pGriglia);
                tbGrigla.AddHeaderCell(cellaGriglia);

                pGriglia = new Paragraph("int4").SetFontSize(10);
                cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetBold();
                cellaGriglia.Add(pGriglia);
                tbGrigla.AddHeaderCell(cellaGriglia);

                pGriglia = new Paragraph("int5").SetFontSize(10);
                cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetBold();
                cellaGriglia.Add(pGriglia);
                tbGrigla.AddHeaderCell(cellaGriglia);

                pGriglia = new Paragraph("int6").SetFontSize(10);
                cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetBold();
                cellaGriglia.Add(pGriglia);
                tbGrigla.AddHeaderCell(cellaGriglia);

                pGriglia = new Paragraph("int7").SetFontSize(10);
                cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetBold();
                cellaGriglia.Add(pGriglia);
                tbGrigla.AddHeaderCell(cellaGriglia);

                // CICLO LA TABELLA

                // AGGIUNGO UNO SPAZIO
                pGriglia = new Paragraph(" ").SetFontSize(9);
                cellaGriglia = new Cell(1,7).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                cellaGriglia.Add(pGriglia);
                tbGrigla.AddCell(cellaGriglia);



                document.Add(tbGrigla);

                //iText.Kernel.Geom.Rectangle pageSize = doc.GetPage(1).GetPageSize();

                int n = doc.GetNumberOfPages();
                iText.Kernel.Geom.Rectangle pageSize = doc.GetPage(n).GetPageSize();


                // AGGIUNGO CONTEGGIO PAGINE E FOOTER PER OGNI PAGINA
                for (int i = 1; i <= n; i++)
                {
                    // AGGIUNGO LOGO
                    iText.Layout.Element.Image image = new iText.Layout.Element.Image(imageData).ScaleAbsolute(90, 80).SetFixedPosition(i, 30, 740);
                    document.Add(image);


                    // CREAZIONE GRIGLIA INFORMAZIONI
                    iText.Layout.Element.Table tbGriglaInfo = new iText.Layout.Element.Table(new float[] { 70, 230 }).SetWidth(300).SetFixedPosition(i,30, 640, 300);
                    Paragraph pGrigliaInfo = new Paragraph(cittaVs).SetFontSize(9).SetBold();
                    iText.Layout.Element.Cell cellaGrigliaInfo = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                    cellaGrigliaInfo.Add(pGrigliaInfo);
                    tbGriglaInfo.AddCell(cellaGrigliaInfo);

                    if (string.IsNullOrEmpty(tbDataElaborazione.Text)) tbDataElaborazione.Text = DateTime.Today.ToShortDateString();
                    pGrigliaInfo = new Paragraph(Convert.ToDateTime(tbDataElaborazione.Text).ToLongDateString()).SetFontSize(9);
                    cellaGrigliaInfo = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                    cellaGrigliaInfo.Add(pGrigliaInfo);
                    tbGriglaInfo.AddCell(cellaGrigliaInfo);

                    pGrigliaInfo = new Paragraph("Produzione:").SetFontSize(9).SetBold();
                    cellaGrigliaInfo = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                    cellaGrigliaInfo.Add(pGrigliaInfo);
                    tbGriglaInfo.AddCell(cellaGrigliaInfo);

                    pGrigliaInfo = new Paragraph("").SetFontSize(9);
                    cellaGrigliaInfo = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                    cellaGrigliaInfo.Add(pGrigliaInfo);
                    tbGriglaInfo.AddCell(cellaGrigliaInfo);

                    pGrigliaInfo = new Paragraph("Lavorazione:").SetFontSize(9).SetBold();
                    cellaGrigliaInfo = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                    cellaGrigliaInfo.Add(pGrigliaInfo);
                    tbGriglaInfo.AddCell(cellaGrigliaInfo);

                    pGrigliaInfo = new Paragraph("").SetFontSize(9);
                    cellaGrigliaInfo = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                    cellaGrigliaInfo.Add(pGrigliaInfo);
                    tbGriglaInfo.AddCell(cellaGrigliaInfo);

                    pGrigliaInfo = new Paragraph("Data Lav.ne:").SetFontSize(9).SetBold();
                    cellaGrigliaInfo = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                    cellaGrigliaInfo.Add(pGrigliaInfo);
                    tbGriglaInfo.AddCell(cellaGrigliaInfo);

                    pGrigliaInfo = new Paragraph(tbDataElaborazione.Text).SetFontSize(9);
                    cellaGrigliaInfo = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                    cellaGrigliaInfo.Add(pGrigliaInfo);
                    tbGriglaInfo.AddCell(cellaGrigliaInfo);

                    document.Add(tbGriglaInfo);


                    // CREAZIONE GRIGLIA DESTINATARIO
                    iText.Layout.Element.Table tbGriglaDest = new iText.Layout.Element.Table(new float[] { 70, 230 }).SetWidth(300).SetFixedPosition(i,350, 650, 300);
                    Paragraph pGrigliaDest = new Paragraph("Spettabile").SetFontSize(9).SetBold();
                    iText.Layout.Element.Cell cellaGrigliaDest = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                    cellaGrigliaDest.Add(pGrigliaDest);
                    tbGriglaDest.AddCell(cellaGrigliaDest);

                    pGrigliaDest = new Paragraph("ragione sociale").SetFontSize(9);
                    cellaGrigliaDest = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                    cellaGrigliaDest.Add(pGrigliaDest);
                    tbGriglaDest.AddCell(cellaGrigliaDest);

                    // INDIRIZZO DESTINATARIO
                    pGrigliaDest = new Paragraph("Indirizzo").SetFontSize(9).SetBold();
                    cellaGrigliaDest = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                    cellaGrigliaDest.Add(pGrigliaDest);
                    tbGriglaDest.AddCell(cellaGrigliaDest);

                    pGrigliaDest = new Paragraph("indirizzo" + Environment.NewLine + "cliente operativo").SetFontSize(9);
                    cellaGrigliaDest = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                    cellaGrigliaDest.Add(pGrigliaDest);
                    tbGriglaDest.AddCell(cellaGrigliaDest);

                    // PARTITA IVA DESTINATARIO
                    pGrigliaDest = new Paragraph("P.Iva/C.F.").SetFontSize(9).SetBold();
                    cellaGrigliaDest = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                    cellaGrigliaDest.Add(pGrigliaDest);
                    tbGriglaDest.AddCell(cellaGrigliaDest);

                    string pIvaCF = "123456";
                    if (string.IsNullOrEmpty(pIvaCF)) pIvaCF = "CLDCLD73L11H501Z";

                    pGrigliaDest = new Paragraph(pIvaCF).SetFontSize(9);
                    cellaGrigliaDest = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                    cellaGrigliaDest.Add(pGrigliaDest);
                    tbGriglaDest.AddCell(cellaGrigliaDest);

                    document.Add(tbGriglaDest);


                    // AGGIUNGO LOGO DNV
                    iText.Layout.Element.Image logoDnv = new iText.Layout.Element.Image(imageDNV).ScaleAbsolute(40, 40).SetFixedPosition(i, 518, 8);
                    document.Add(logoDnv);

                    //AGGIUNGO NUM.PAGINA
                    document.ShowTextAligned(new Paragraph("pagina " + i.ToString() + " di " + n.ToString()).SetFontSize(7),
                        pageSize.GetWidth() - 60, pageSize.GetHeight() - 20, i, iText.Layout.Properties.TextAlignment.CENTER, iText.Layout.Properties.VerticalAlignment.TOP, 0);
                    //AGGIUNGO FOOTER
                    document.ShowTextAligned(new Paragraph(denominazioneVs + " P.IVA " + pIvaVs + Environment.NewLine + "Sede legale: " + toponimoVs + " " + indirizzoVs + " " + civicoVs + " - " + capVs + " " + cittaVs + " " + provinciaVs + " e-mail: " + emailVs ).SetFontSize(7).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER),
                                                    pageSize.GetWidth()/2, 30, i, iText.Layout.Properties.TextAlignment.CENTER, iText.Layout.Properties.VerticalAlignment.TOP, 0);

                    if (i == n)
                    {
                        // NELL'ULTIMA PAGINA AGGIUNGO LA GRIGLIA CON LE NOTE E IL TIMBRO
                        // CREAZIONE GRIGLIA
                        iText.Layout.Element.Table tbGriglaNoteFooter = new iText.Layout.Element.Table(new float[] { 80, 70, 180, 70, 30, 30, 70 }).SetWidth(530).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetFixedPosition(30, 50,530).SetFixedLayout();

                        // PRIMA RIGA GRIGLIA NOTE FOOTER
                        Paragraph pGrigliaNoteFooter = new Paragraph("Banca").SetFontSize(9);
                        iText.Layout.Element.Cell cellaGrigliaNoteFooter = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                        tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                        pGrigliaNoteFooter = new Paragraph("BANCA").SetFontSize(9);
                        //cellaGrigliaNoteFooter = new iText.Layout.Element.Cell(1,2).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(2);
                        cellaGrigliaNoteFooter = new iText.Layout.Element.Cell(1, 2).SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorderRight(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 2, 50)).SetBorderTop(iText.Layout.Borders.Border.NO_BORDER).SetBorderLeft(iText.Layout.Borders.Border.NO_BORDER).SetBorderBottom(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                                
                        cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                        tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                        pGrigliaNoteFooter = new Paragraph("Timbro e firma per accettazione").SetFontSize(9);
                        cellaGrigliaNoteFooter = new iText.Layout.Element.Cell(1, 4).SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                        tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                        // SECONDA RIGA GRIGLIA NOTE FOOTER
                        pGrigliaNoteFooter = new Paragraph("Pagamento").SetFontSize(9);
                        cellaGrigliaNoteFooter = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                        tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                        pGrigliaNoteFooter = new Paragraph("NOTEPAGAMENTO").SetFontSize(9);
                        cellaGrigliaNoteFooter = new iText.Layout.Element.Cell(1, 2).SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorderRight(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 2, 50)).SetBorderTop(iText.Layout.Borders.Border.NO_BORDER).SetBorderLeft(iText.Layout.Borders.Border.NO_BORDER).SetBorderBottom(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                        tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                        pGrigliaNoteFooter = new Paragraph(" ").SetFontSize(9);
                        cellaGrigliaNoteFooter = new iText.Layout.Element.Cell(1, 4).SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                        tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                        // TERZA RIGA GRIGLIA NOTE FOOTER
                        pGrigliaNoteFooter = new Paragraph("Consegna").SetFontSize(9);
                        cellaGrigliaNoteFooter = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                        tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                        pGrigliaNoteFooter = new Paragraph("CONSEGNA").SetFontSize(9);
                        cellaGrigliaNoteFooter = new iText.Layout.Element.Cell(1, 2).SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorderRight(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 2, 50)).SetBorderTop(iText.Layout.Borders.Border.NO_BORDER).SetBorderLeft(iText.Layout.Borders.Border.NO_BORDER).SetBorderBottom(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                        tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                        pGrigliaNoteFooter = new Paragraph(" ").SetFontSize(9);
                        cellaGrigliaNoteFooter = new iText.Layout.Element.Cell(1, 4).SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                        tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                        document.Add(tbGriglaNoteFooter);
                    }
                }

                document.Close();
                wr.Close();

                if (File.Exists(mapPathGiornata))
                {

                    framePdfGiornata.Attributes.Remove("src");
                    framePdfGiornata.Attributes.Add("src", pathGiornata.Replace("~", ""));

                    DivFramePdfGiornata.Visible = true;
                    framePdfGiornata.Visible = true;

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "aggiornaFrame", script: "javascript: document.getElementById('" + framePdfGiornata.ClientID + "').contentDocument.location.reload(true);", addScriptTags: true);
                    btnStampaGiornata.Attributes.Add("onclick", "window.open('" + pathGiornata.Replace("~", "") + "');");
                    //}
                }
                else
                {
                    esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                    esito.Descrizione = "Il File " + pathGiornata.Replace("~", "") + " non è stato creato correttamente!";
                }
                    
                
            }
            catch (Exception ex)
            {

                esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.Descrizione = "popolaPannelloGiornata() " + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }



        #region COMPORTAMENTO ELEMENTI PAGINA
        protected void btnStampaGiornata_Click(object sender, EventArgs e)
        {
        }

        #endregion

        #region OPERAZIONI POPUP

        #endregion

        protected void btnCreaGiornata_Click(object sender, EventArgs e)
        {
            Esito esito = popolaPannelloGiornata();

            //Esito esito = popolaPannelloFattura(SessionManager.EventoSelezionato);
            if (esito.Codice == Esito.ESITO_OK)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "aggiornaFrame", script: "javascript: document.getElementById('" + framePdfGiornata.ClientID + "').contentDocument.location.reload(true);", addScriptTags: true);
            }
        }
    }

}