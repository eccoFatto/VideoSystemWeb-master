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
    public partial class RiepilogoConsuntivo : System.Web.UI.UserControl
    {
        BasePage basePage = new BasePage();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(this.btnStampaConsuntivo);
            }
        }
        public Esito popolaPannelloConsuntivo(DatiAgenda eventoSelezionato)
        {
            Esito esito = new Esito();
            try
            {
                if (eventoSelezionato != null && eventoSelezionato.LavorazioneCorrente != null)
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

                    //List<DatiArticoliLavorazione> listaArticoliLavorazione = eventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Where(x => x.Stampa).OrderBy(x => x.Consuntivo).ToList<DatiArticoliLavorazione>();
                    List<DatiArticoliLavorazione> listaArticoliLavorazione = eventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.Where(x => x.Stampa).ToList<DatiArticoliLavorazione>();

                    if (listaArticoliLavorazione!=null)
                    {

                        Protocolli protocolloConsuntivo = new Protocolli();
                        int idTipoProtocollo = UtilityTipologiche.getElementByNome(UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_PROTOCOLLO), "Consuntivo", ref esito).id;
                        List<Protocolli> listaProtocolli = Protocolli_BLL.Instance.getProtocolliByCodLavIdTipoProtocollo(eventoSelezionato.codice_lavoro, idTipoProtocollo, ref esito, true);
                        string numeroProtocollo = "";
                        if (listaProtocolli.Count == 0)
                        {
                            numeroProtocollo = Protocolli_BLL.Instance.getNumeroProtocollo();
                        }
                        else
                        {
                            protocolloConsuntivo = listaProtocolli.First();
                            numeroProtocollo = protocolloConsuntivo.Numero_protocollo;
                        }

                        // GESTIONE NOMI FILE PDF
                        string nomeFile = "Consuntivo_" + eventoSelezionato.codice_lavoro + ".pdf";
                        string pathConsuntivo = ConfigurationManager.AppSettings["PATH_DOCUMENTI_PROTOCOLLO"] + nomeFile;
                        string mapPathConsuntivo = MapPath(ConfigurationManager.AppSettings["PATH_DOCUMENTI_PROTOCOLLO"]) + nomeFile;

                        string prefissoUrl = Request.Url.Scheme + "://" + Request.Url.Authority;
                        iText.IO.Image.ImageData imageData = iText.IO.Image.ImageDataFactory.Create(prefissoUrl + "/Images/logoVSP_trim.png");

                        iText.IO.Image.ImageData imageDNV = iText.IO.Image.ImageDataFactory.Create(MapPath("~/Images/DNV_2008_ITA2.jpg"));


                        PdfWriter wr = new PdfWriter(mapPathConsuntivo);
                        PdfDocument doc = new PdfDocument(wr);
                        doc.SetDefaultPageSize(iText.Kernel.Geom.PageSize.A4);
                        Document document = new Document(doc);

                        document.SetMargins(245, 30, 100, 30);



                        // ESTRAPOLO IL CLIENTE
                        Anag_Clienti_Fornitori cliente = Anag_Clienti_Fornitori_BLL.Instance.getAziendaById(eventoSelezionato.id_cliente, ref esito);

                        Paragraph pSpazio = new Paragraph(" ");
                        document.Add(pSpazio);

                        // CREAZIONE GRIGLIA
                        iText.Layout.Element.Table tbGrigla = new iText.Layout.Element.Table(new float[] { 80, 50, 200, 70, 25, 25, 80 }).SetWidth(530).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10);
                        Paragraph pGriglia;
                        Cell cellaGriglia;

                        // INTESTAZIONE CONSUNTIVO
                        pGriglia = new Paragraph("Consuntivo").SetFontSize(10).SetBold();
                        cellaGriglia = new Cell().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddHeaderCell(cellaGriglia);

                        pGriglia = new Paragraph(eventoSelezionato.codice_lavoro).SetFontSize(10).SetBold();
                        cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddHeaderCell(cellaGriglia);

                        pGriglia = new Paragraph("").SetFontSize(10);
                        cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddHeaderCell(cellaGriglia);

                        pGriglia = new Paragraph("Rif.Prot. " + numeroProtocollo).SetFontSize(10).SetBold();
                        cellaGriglia = new iText.Layout.Element.Cell(1, 4).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddHeaderCell(cellaGriglia);

                        // INTESTAZIONE GRIGLIA
                        pGriglia = new Paragraph("Codice").SetFontSize(10);
                        cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetPadding(5).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddHeaderCell(cellaGriglia);

                        pGriglia = new Paragraph("Descrizione Offerte").SetFontSize(10);
                        cellaGriglia = new iText.Layout.Element.Cell(1, 2).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetPadding(5).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddHeaderCell(cellaGriglia);

                        pGriglia = new Paragraph("Prezzo").SetFontSize(10);
                        cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddHeaderCell(cellaGriglia);

                        pGriglia = new Paragraph("Qta").SetFontSize(10);
                        cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddHeaderCell(cellaGriglia);

                        pGriglia = new Paragraph("Iva").SetFontSize(10);
                        cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddHeaderCell(cellaGriglia);

                        pGriglia = new Paragraph("Totale").SetFontSize(10);
                        cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddHeaderCell(cellaGriglia);


                        decimal totPrezzo = 0;
                        decimal totIVA = 0;

                        // CICLO GLI ARTICOLI
                        //foreach (DatiArticoli da in listaDatiArticoli)
                        foreach (DatiArticoliLavorazione da in listaArticoliLavorazione)
                        {
                            // CALCOLO I TOTALI
                            //totPrezzo += da.Prezzo * da.Quantita;
                            totPrezzo += da.Prezzo * 1;
                            //totIVA += (da.Prezzo * da.Iva / 100) * da.Quantita;
                            totIVA += (da.Prezzo * da.Iva / 100) * 1;

                            string descrizione = da.Descrizione;
                            string descrizioneLunga = da.DescrizioneLunga;
                            if (da.Consuntivo==true)
                            {
                                //descrizione = "(c)" + descrizione;
                                descrizioneLunga = "(Consuntivo)" + Environment.NewLine + descrizioneLunga;
                            }

                            pGriglia = new Paragraph(descrizione).SetFontSize(9);
                            cellaGriglia = new Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10);
                            cellaGriglia.Add(pGriglia);
                            tbGrigla.AddCell(cellaGriglia);

                            pGriglia = new Paragraph(descrizioneLunga).SetFontSize(9);
                            cellaGriglia = new Cell(1, 2).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10);
                            cellaGriglia.Add(pGriglia);
                            tbGrigla.AddCell(cellaGriglia);

                            pGriglia = new Paragraph(da.Prezzo.ToString("###,##0.00")).SetFontSize(9);
                            cellaGriglia = new Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                            cellaGriglia.Add(pGriglia);
                            tbGrigla.AddCell(cellaGriglia);

                            //pGriglia = new Paragraph(da.Quantita.ToString("##0")).SetFontSize(9);
                            pGriglia = new Paragraph(1.ToString("##0")).SetFontSize(9);
                            cellaGriglia = new Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                            cellaGriglia.Add(pGriglia);
                            tbGrigla.AddCell(cellaGriglia);

                            pGriglia = new Paragraph(da.Iva.ToString("##")).SetFontSize(9);
                            cellaGriglia = new Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                            cellaGriglia.Add(pGriglia);
                            tbGrigla.AddCell(cellaGriglia);

                            //decimal totale = da.Prezzo * da.Quantita;
                            decimal totale = da.Prezzo * 1;

                            pGriglia = new Paragraph(totale.ToString("###,##0.00")).SetFontSize(9);
                            cellaGriglia = new Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                            cellaGriglia.Add(pGriglia);
                            tbGrigla.AddCell(cellaGriglia);
                        }

                        // AGGIUNGO UNO SPAZIO
                        pGriglia = new Paragraph(" ").SetFontSize(9);
                        cellaGriglia = new Cell(1,7).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);


                        // ESTRAPOLO NOTEOFFERTA
                        NoteOfferta noteOfferta = Offerta_BLL.Instance.getNoteOffertaByIdDatiAgenda(eventoSelezionato.id, ref esito);

                        // NOTE
                        Text first = new Text("Note:").SetFontSize(9).SetBold();
                        //Text second = new Text(Environment.NewLine + "Gli articoli con la dicitura 'Cons' sono da ritenersi a CONSUNTIVO" + Environment.NewLine + noteOfferta.Note.Trim()).SetFontSize(9);
                        Text second = new Text(Environment.NewLine + noteOfferta.Note.Trim()).SetFontSize(9);
                        Paragraph paragraphNote = new Paragraph().Add(first).Add(second);

                        cellaGriglia = new iText.Layout.Element.Cell(3, 3).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10);
                        cellaGriglia.Add(paragraphNote);
                        tbGrigla.AddCell(cellaGriglia);

                        pGriglia = new Paragraph("Imponibile").SetFontSize(9);
                        cellaGriglia = new iText.Layout.Element.Cell(1,3).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        pGriglia = new Paragraph(totPrezzo.ToString("###,##0.00")).SetFontSize(9);
                        cellaGriglia = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        // TOTALE IVA
                        pGriglia = new Paragraph("i.v.a.").SetFontSize(9);
                        cellaGriglia = new iText.Layout.Element.Cell(1,3).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        pGriglia = new Paragraph(totIVA.ToString("###,##0.00")).SetFontSize(9);
                        cellaGriglia = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        // TOTALE EURO
                        pGriglia = new Paragraph("Totale Euro").SetFontSize(9);
                        cellaGriglia = new iText.Layout.Element.Cell(1,3).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        pGriglia = new Paragraph((totPrezzo + totIVA).ToString("###,##0.00")).SetFontSize(9);
                        cellaGriglia = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        document.Add(tbGrigla);

                        iText.Kernel.Geom.Rectangle pageSize = doc.GetPage(1).GetPageSize();

                        int n = doc.GetNumberOfPages();


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

                            pGrigliaInfo = new Paragraph(DateTime.Today.ToLongDateString()).SetFontSize(9);
                            cellaGrigliaInfo = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                            cellaGrigliaInfo.Add(pGrigliaInfo);
                            tbGriglaInfo.AddCell(cellaGrigliaInfo);

                            pGrigliaInfo = new Paragraph("Produzione:").SetFontSize(9).SetBold();
                            cellaGrigliaInfo = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                            cellaGrigliaInfo.Add(pGrigliaInfo);
                            tbGriglaInfo.AddCell(cellaGrigliaInfo);

                            pGrigliaInfo = new Paragraph(eventoSelezionato.produzione).SetFontSize(9);
                            cellaGrigliaInfo = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                            cellaGrigliaInfo.Add(pGrigliaInfo);
                            tbGriglaInfo.AddCell(cellaGrigliaInfo);

                            pGrigliaInfo = new Paragraph("Lavorazione:").SetFontSize(9).SetBold();
                            cellaGrigliaInfo = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                            cellaGrigliaInfo.Add(pGrigliaInfo);
                            tbGriglaInfo.AddCell(cellaGrigliaInfo);

                            pGrigliaInfo = new Paragraph(eventoSelezionato.lavorazione).SetFontSize(9);
                            cellaGrigliaInfo = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                            cellaGrigliaInfo.Add(pGrigliaInfo);
                            tbGriglaInfo.AddCell(cellaGrigliaInfo);

                            pGrigliaInfo = new Paragraph("Data Lav.ne:").SetFontSize(9).SetBold();
                            cellaGrigliaInfo = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                            cellaGrigliaInfo.Add(pGrigliaInfo);
                            tbGriglaInfo.AddCell(cellaGrigliaInfo);

                            pGrigliaInfo = new Paragraph(eventoSelezionato.data_inizio_lavorazione.ToString("dd/MM/yyyy")).SetFontSize(9);
                            cellaGrigliaInfo = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                            cellaGrigliaInfo.Add(pGrigliaInfo);
                            tbGriglaInfo.AddCell(cellaGrigliaInfo);

                            document.Add(tbGriglaInfo);

                            document.Add(pSpazio);
                            document.Add(pSpazio);

                            // CREAZIONE GRIGLIA DESTINATARIO
                            iText.Layout.Element.Table tbGriglaDest = new iText.Layout.Element.Table(new float[] { 70, 230 }).SetWidth(300).SetFixedPosition(i,350, 650, 300);
                            Paragraph pGrigliaDest = new Paragraph("Spettabile").SetFontSize(9).SetBold();
                            iText.Layout.Element.Cell cellaGrigliaDest = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                            cellaGrigliaDest.Add(pGrigliaDest);
                            tbGriglaDest.AddCell(cellaGrigliaDest);

                            pGrigliaDest = new Paragraph(cliente.RagioneSociale).SetFontSize(9);
                            cellaGrigliaDest = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                            cellaGrigliaDest.Add(pGrigliaDest);
                            tbGriglaDest.AddCell(cellaGrigliaDest);

                            // INDIRIZZO DESTINATARIO
                            pGrigliaDest = new Paragraph("Indirizzo").SetFontSize(9).SetBold();
                            cellaGrigliaDest = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                            cellaGrigliaDest.Add(pGrigliaDest);
                            tbGriglaDest.AddCell(cellaGrigliaDest);

                            pGrigliaDest = new Paragraph(cliente.TipoIndirizzoOperativo + " " + cliente.IndirizzoOperativo + " " + cliente.NumeroCivicoOperativo + Environment.NewLine + cliente.CapOperativo + " " + cliente.ComuneOperativo + " " + cliente.ProvinciaOperativo).SetFontSize(9);
                            cellaGrigliaDest = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                            cellaGrigliaDest.Add(pGrigliaDest);
                            tbGriglaDest.AddCell(cellaGrigliaDest);

                            // PARTITA IVA DESTINATARIO
                            pGrigliaDest = new Paragraph("P.Iva/C.F.").SetFontSize(9).SetBold();
                            cellaGrigliaDest = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                            cellaGrigliaDest.Add(pGrigliaDest);
                            tbGriglaDest.AddCell(cellaGrigliaDest);

                            string pIvaCF = cliente.PartitaIva;
                            if (string.IsNullOrEmpty(pIvaCF)) pIvaCF = cliente.CodiceFiscale;

                            pGrigliaDest = new Paragraph(pIvaCF).SetFontSize(9);
                            cellaGrigliaDest = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                            cellaGrigliaDest.Add(pGrigliaDest);
                            tbGriglaDest.AddCell(cellaGrigliaDest);

                            document.Add(tbGriglaDest);


                            // CREAZIONE INTESTAZIONE GRIGLIA
                            //iText.Layout.Element.Table tbGriglaInt = new iText.Layout.Element.Table(new float[] { 80, 50, 200, 80, 20, 20, 80 }).SetWidth(530).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetFixedPosition(i, 30, 595, 530); ;



                            //document.Add(tbGriglaInt);


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
                                iText.Layout.Element.Table tbGriglaNoteFooter = new iText.Layout.Element.Table(new float[] { 80, 50, 180, 100, 20, 20, 80 }).SetWidth(530).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetFixedPosition(30, 50,530);
                                
                                // PRIMA RIGA GRIGLIA NOTE FOOTER
                                Paragraph pGrigliaNoteFooter = new Paragraph("Banca").SetFontSize(9);
                                iText.Layout.Element.Cell cellaGrigliaNoteFooter = new iText.Layout.Element.Cell().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                                cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                                tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                                pGrigliaNoteFooter = new Paragraph(noteOfferta.Banca).SetFontSize(8);
                                //cellaGrigliaNoteFooter = new iText.Layout.Element.Cell(1,2).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(2);
                                cellaGrigliaNoteFooter = new iText.Layout.Element.Cell(1, 2).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorderRight(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetBorderTop(iText.Layout.Borders.Border.NO_BORDER).SetBorderLeft(iText.Layout.Borders.Border.NO_BORDER).SetBorderBottom(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                                
                                cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                                tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                                pGrigliaNoteFooter = new Paragraph("Timbro e firma per accettazione").SetFontSize(9);
                                cellaGrigliaNoteFooter = new iText.Layout.Element.Cell(1, 4).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                                cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                                tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                                // SECONDA RIGA GRIGLIA NOTE FOOTER
                                pGrigliaNoteFooter = new Paragraph("Pagamento").SetFontSize(9);
                                cellaGrigliaNoteFooter = new iText.Layout.Element.Cell().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                                cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                                tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                                pGrigliaNoteFooter = new Paragraph(noteOfferta.NotaPagamento).SetFontSize(9);
                                cellaGrigliaNoteFooter = new iText.Layout.Element.Cell(1, 2).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorderRight(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetBorderTop(iText.Layout.Borders.Border.NO_BORDER).SetBorderLeft(iText.Layout.Borders.Border.NO_BORDER).SetBorderBottom(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                                cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                                tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                                pGrigliaNoteFooter = new Paragraph("").SetFontSize(9);
                                cellaGrigliaNoteFooter = new iText.Layout.Element.Cell(1, 4).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                                cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                                tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                                // TERZA RIGA GRIGLIA NOTE FOOTER
                                pGrigliaNoteFooter = new Paragraph("Consegna").SetFontSize(9);
                                cellaGrigliaNoteFooter = new iText.Layout.Element.Cell().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                                cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                                tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                                pGrigliaNoteFooter = new Paragraph(noteOfferta.Consegna.Replace("\r\n"," ")).SetFontSize(9);
                                cellaGrigliaNoteFooter = new iText.Layout.Element.Cell(1, 2).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorderRight(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetBorderTop(iText.Layout.Borders.Border.NO_BORDER).SetBorderLeft(iText.Layout.Borders.Border.NO_BORDER).SetBorderBottom(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                                cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                                tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                                pGrigliaNoteFooter = new Paragraph("").SetFontSize(9);
                                cellaGrigliaNoteFooter = new iText.Layout.Element.Cell(1, 4).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                                cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                                tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                                document.Add(tbGriglaNoteFooter);
                            }
                        }

                        document.Close();
                        wr.Close();

                        if (File.Exists(mapPathConsuntivo))
                        {

                            // SE FILE OK INSERISCO O AGGIORNO PROTOCOLLO DI TIPO PIANO ESTERNO
                            if (listaProtocolli.Count == 0)
                            {
                                //INSERISCO
                                protocolloConsuntivo.Attivo = true;
                                protocolloConsuntivo.Cliente = cliente.RagioneSociale.Trim();
                                protocolloConsuntivo.Codice_lavoro = eventoSelezionato.codice_lavoro;
                                protocolloConsuntivo.Data_inizio_lavorazione = eventoSelezionato.data_inizio_impegno;
                                protocolloConsuntivo.Data_protocollo = DateTime.Today;
                                protocolloConsuntivo.Descrizione = "Consuntivo";
                                protocolloConsuntivo.Id_cliente = eventoSelezionato.id_cliente;
                                protocolloConsuntivo.Id_tipo_protocollo = idTipoProtocollo;
                                protocolloConsuntivo.Lavorazione = eventoSelezionato.lavorazione;
                                protocolloConsuntivo.PathDocumento = Path.GetFileName(mapPathConsuntivo);
                                protocolloConsuntivo.Produzione = eventoSelezionato.produzione;
                                protocolloConsuntivo.Protocollo_riferimento = "";
                                protocolloConsuntivo.Numero_protocollo = numeroProtocollo;
                                int idProtPianoEsterno = Protocolli_BLL.Instance.CreaProtocollo(protocolloConsuntivo, ref esito);
                            }
                            else
                            {
                                // AGGIORNO
                                protocolloConsuntivo.PathDocumento = Path.GetFileName(mapPathConsuntivo);
                                esito = Protocolli_BLL.Instance.AggiornaProtocollo(protocolloConsuntivo);
                            }

                            framePdfConsuntivo.Attributes.Remove("src");
                                framePdfConsuntivo.Attributes.Add("src", pathConsuntivo.Replace("~", ""));

                                DivFramePdfConsuntivo.Visible = true;
                                framePdfConsuntivo.Visible = true;

                                ScriptManager.RegisterStartupScript(Page, typeof(Page), "aggiornaFrame", script: "javascript: document.getElementById('" + framePdfConsuntivo.ClientID + "').contentDocument.location.reload(true);", addScriptTags: true);
                                btnStampaConsuntivo.Attributes.Add("onclick", "window.open('" + pathConsuntivo.Replace("~", "") + "');");
                            //}
                        }
                        else
                        {
                            esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                            esito.Descrizione = "Il File " + pathConsuntivo.Replace("~", "") + " non è stato creato correttamente!";
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.Descrizione = "popolaPannelloConsuntivo(DatiAgenda eventoSelezionato) " + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }



        #region COMPORTAMENTO ELEMENTI PAGINA
        protected void btnStampaConsuntivo_Click(object sender, EventArgs e)
        {
        }


        #endregion

        #region OPERAZIONI POPUP

        #endregion

    }

}