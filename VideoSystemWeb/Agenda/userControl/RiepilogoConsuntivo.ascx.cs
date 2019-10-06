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

                    //List<DatiPianoEsternoLavorazione> listaDatiPianoEsternoLavorazione = eventoSelezionato.LavorazioneCorrente.ListaDatiPianoEsternoLavorazione;
                    List<DatiArticoli> listaDatiArticoli = eventoSelezionato.ListaDatiArticoli.Where(x => x.Stampa).ToList<DatiArticoli>();

                    if (listaDatiArticoli != null)
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

                        document.SetMargins(100, 30, 50, 30);

                        //// AGGIUNGO TABLE PER LAYOUT INTESTAZIONE
                        //iText.Layout.Element.Table tbIntestazione = new iText.Layout.Element.Table(new float[] { 1, 9 }).UseAllAvailableWidth().SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        ////iText.Layout.Element.Table tbIntestazione = new iText.Layout.Element.Table(3).UseAllAvailableWidth();
                        //iText.Layout.Element.Image image = new iText.Layout.Element.Image(imageData).ScaleAbsolute(80, 80).SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        //iText.Layout.Element.Cell cellaImgLogo = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        //cellaImgLogo.Add(image);
                        //tbIntestazione.AddCell(cellaImgLogo);

                        //Paragraph pRightLogo = new Paragraph(" ");
                        //iText.Layout.Element.Cell cellaRightLogo = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        //cellaRightLogo.Add(pRightLogo);
                        //tbIntestazione.AddCell(cellaRightLogo);
                        //document.Add(tbIntestazione);

                        // AGGIUNGO LOGO
                        iText.Layout.Element.Image image = new iText.Layout.Element.Image(imageData).ScaleAbsolute(80, 80).SetFixedPosition(1, 30, 740);
                        document.Add(image);


                        // ESTRAPOLO IL CLIENTE
                        Anag_Clienti_Fornitori cliente = Anag_Clienti_Fornitori_BLL.Instance.getAziendaById(eventoSelezionato.id_cliente, ref esito);

                        Paragraph pSpazio = new Paragraph(" ");
                        document.Add(pSpazio);

                        Paragraph pDataOdierna = new Paragraph(cittaVs + ", " + DateTime.Today.ToLongDateString());
                        document.Add(pDataOdierna);
                        document.Add(pSpazio);

                        Paragraph pProduzione = new Paragraph("Produzione: " + eventoSelezionato.produzione);
                        document.Add(pProduzione);
                        Paragraph pLavorazione = new Paragraph("Lavorazione: " + eventoSelezionato.lavorazione);
                        document.Add(pLavorazione);

                        Paragraph pLuogoData = new Paragraph("Data Lavorazione: " + eventoSelezionato.data_inizio_lavorazione.ToString("dd/MM/yyyy"));
                        document.Add(pLuogoData);
                        document.Add(pSpazio);

                        // CREAZIONE GRIGLIA DESTINATARIO
                        iText.Layout.Element.Table tbGriglaDest = new iText.Layout.Element.Table(new float[] { 70, 230 }).SetWidth(300).SetFixedPosition(350, 650, 300); ;
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

                        // CREAZIONE GRIGLIA
                        iText.Layout.Element.Table tbGrigla = new iText.Layout.Element.Table(new float[] { 1, 1, 4, 1, 1, 1, 1 }).SetWidth(530).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10);

                        // INTESTAZIONE CONSUNTIVO
                        Paragraph pGriglia = new Paragraph("Consuntivo").SetFontSize(10);
                        iText.Layout.Element.Cell cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        pGriglia = new Paragraph(eventoSelezionato.codice_lavoro).SetFontSize(10);
                        cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        pGriglia = new Paragraph("elenco offerte").SetFontSize(10);
                        cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        pGriglia = new Paragraph("Rif.Prot. " + numeroProtocollo).SetFontSize(10);
                        cellaGriglia = new iText.Layout.Element.Cell(1,4).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        // INTESTAZIONE GRIGLIA
                        pGriglia = new Paragraph("Codice").SetFontSize(10);
                        cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetPadding(5).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        pGriglia = new Paragraph("Descrizione").SetFontSize(10);
                        cellaGriglia = new iText.Layout.Element.Cell(1,2).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetPadding(5).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        pGriglia = new Paragraph("Prezzo").SetFontSize(10);
                        cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        pGriglia = new Paragraph("Qta").SetFontSize(10);
                        cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        pGriglia = new Paragraph("Iva").SetFontSize(10);
                        cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        pGriglia = new Paragraph("Totale").SetFontSize(10);
                        cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        decimal totPrezzo = 0;
                        decimal totIVA = 0;

                        // CICLO GLI ARTICOLI
                        foreach (DatiArticoli da in listaDatiArticoli)
                        {
                            // CALCOLO I TOTALI
                            totPrezzo += da.Prezzo * da.Quantita;
                            totIVA += (da.Prezzo * da.Iva / 100) * da.Quantita;

                            pGriglia = new Paragraph(da.Descrizione).SetFontSize(10);
                            cellaGriglia = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10);
                            cellaGriglia.Add(pGriglia);
                            tbGrigla.AddCell(cellaGriglia);

                            pGriglia = new Paragraph(da.DescrizioneLunga).SetFontSize(10);
                            cellaGriglia = new iText.Layout.Element.Cell(1, 2).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10);
                            cellaGriglia.Add(pGriglia);
                            tbGrigla.AddCell(cellaGriglia);

                            pGriglia = new Paragraph(da.Prezzo.ToString("###,##0.00")).SetFontSize(10);
                            cellaGriglia = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                            cellaGriglia.Add(pGriglia);
                            tbGrigla.AddCell(cellaGriglia);

                            pGriglia = new Paragraph(da.Quantita.ToString("##0")).SetFontSize(10);
                            cellaGriglia = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                            cellaGriglia.Add(pGriglia);
                            tbGrigla.AddCell(cellaGriglia);

                            pGriglia = new Paragraph(da.Iva.ToString("##")).SetFontSize(10);
                            cellaGriglia = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                            cellaGriglia.Add(pGriglia);
                            tbGrigla.AddCell(cellaGriglia);

                            decimal totale = da.Prezzo * da.Quantita;

                            pGriglia = new Paragraph(totale.ToString("###,##0.00")).SetFontSize(10);
                            cellaGriglia = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                            cellaGriglia.Add(pGriglia);
                            tbGrigla.AddCell(cellaGriglia);
                        }

                        // ESTRAPOLO NOTEOFFERTA
                        NoteOfferta noteOfferta = Offerta_BLL.Instance.getNoteOffertaByIdDatiAgenda(eventoSelezionato.id, ref esito);

                        // NOTE
                        pGriglia = new Paragraph("Note:").SetFontSize(10);
                        cellaGriglia = new iText.Layout.Element.Cell(1,3).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        // TOTALE
                        pGriglia = new Paragraph("Totale").SetFontSize(10);
                        cellaGriglia = new iText.Layout.Element.Cell(1, 3).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        pGriglia = new Paragraph(totPrezzo.ToString("###,##0.00")).SetFontSize(10);
                        cellaGriglia = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        // NOTE
                        pGriglia = new Paragraph("Gli articoli con la dicitura 'Cons' sono da ritenersi a CONSUNTIVO" + Environment.NewLine + eventoSelezionato.nota).SetFontSize(10);
                        cellaGriglia = new iText.Layout.Element.Cell(1, 3).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10);
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);
                        
                        // TOTALE IVA
                        pGriglia = new Paragraph("Totale i.v.a.").SetFontSize(10);
                        cellaGriglia = new iText.Layout.Element.Cell(1, 3).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        pGriglia = new Paragraph(totIVA.ToString("###,##0.00")).SetFontSize(10);
                        cellaGriglia = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        // NOTE
                        pGriglia = new Paragraph(noteOfferta.Note).SetFontSize(10);
                        cellaGriglia = new iText.Layout.Element.Cell(1, 3).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10);
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        // TOTALE EURO
                        pGriglia = new Paragraph("Totale Euro").SetFontSize(10);
                        cellaGriglia = new iText.Layout.Element.Cell(1, 3).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetBold();
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        pGriglia = new Paragraph((totPrezzo + totIVA).ToString("###,##0.00")).SetFontSize(10);
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
                            //iText.Layout.Element.Image image = new iText.Layout.Element.Image(imageData).ScaleAbsolute(60, 60).SetFixedPosition(i, 20, pageSize.GetHeight() - 80);
                            //document.Add(image);

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
                                iText.Layout.Element.Table tbGriglaNoteFooter = new iText.Layout.Element.Table(new float[] { 1, 1, 4, 1, 1, 1, 1 }).SetWidth(530).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetFixedPosition(30, 50,530);
                                
                                // PRIMA RIGA GRIGLIA NOTE FOOTER
                                Paragraph pGrigliaNoteFooter = new Paragraph("Banca").SetFontSize(9);
                                iText.Layout.Element.Cell cellaGrigliaNoteFooter = new iText.Layout.Element.Cell().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(2);
                                cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                                tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                                pGrigliaNoteFooter = new Paragraph(noteOfferta.Banca).SetFontSize(9);
                                cellaGrigliaNoteFooter = new iText.Layout.Element.Cell(1,2).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(2);
                                cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                                tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                                pGrigliaNoteFooter = new Paragraph("Timbro e firma per accettazione").SetFontSize(9);
                                cellaGrigliaNoteFooter = new iText.Layout.Element.Cell(1, 4).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(2);
                                cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                                tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                                // SECONDA RIGA GRIGLIA NOTE FOOTER
                                pGrigliaNoteFooter = new Paragraph("Pagamento").SetFontSize(9);
                                cellaGrigliaNoteFooter = new iText.Layout.Element.Cell().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(2);
                                cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                                tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                                pGrigliaNoteFooter = new Paragraph(noteOfferta.NotaPagamento).SetFontSize(9);
                                cellaGrigliaNoteFooter = new iText.Layout.Element.Cell(1, 2).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(2);
                                cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                                tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                                pGrigliaNoteFooter = new Paragraph("").SetFontSize(9);
                                cellaGrigliaNoteFooter = new iText.Layout.Element.Cell(1, 4).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(2);
                                cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                                tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                                // TERZA RIGA GRIGLIA NOTE FOOTER
                                pGrigliaNoteFooter = new Paragraph("Consegna").SetFontSize(9);
                                cellaGrigliaNoteFooter = new iText.Layout.Element.Cell().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(2);
                                cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                                tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                                pGrigliaNoteFooter = new Paragraph(noteOfferta.Consegna).SetFontSize(9);
                                cellaGrigliaNoteFooter = new iText.Layout.Element.Cell(1, 2).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(2);
                                cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                                tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                                pGrigliaNoteFooter = new Paragraph("").SetFontSize(9);
                                cellaGrigliaNoteFooter = new iText.Layout.Element.Cell(1, 4).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(2);
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