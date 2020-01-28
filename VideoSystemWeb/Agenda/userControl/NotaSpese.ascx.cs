using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoSystemWeb.BLL;
using VideoSystemWeb.Entity;

namespace VideoSystemWeb.Agenda.userControl
{
    public partial class NotaSpese : System.Web.UI.UserControl
    {
        BasePage basePage = new BasePage();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(this.btnStampaNotaSpese);
            }
        }

        public Esito PopolaPannelloNotaSpese(DatiAgenda eventoSelezionato, FiguraProfessionale figuraProfessionaleSelezionata)
        {
            Esito esito = new Esito();
            try
            {
                if (eventoSelezionato != null && eventoSelezionato.LavorazioneCorrente != null)
                {
                    #region LEGGO I PARAMETRI DI VS
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
                    #endregion

                    List<DatiPianoEsternoLavorazione> listaDatiPianoEsternoLavorazione = eventoSelezionato.LavorazioneCorrente.ListaDatiPianoEsternoLavorazione;
                    if (listaDatiPianoEsternoLavorazione != null)
                    {
                        string nomeFile = "NotaSpese_" + figuraProfessionaleSelezionata.Cognome + "_" + figuraProfessionaleSelezionata.Nome + eventoSelezionato.codice_lavoro + ".pdf";
                        string pathNotaSpese = ConfigurationManager.AppSettings["PATH_DOCUMENTI_PROTOCOLLO"] + nomeFile;
                        string mapPathNotaSpese = MapPath(ConfigurationManager.AppSettings["PATH_DOCUMENTI_PROTOCOLLO"]) + nomeFile;

                        string prefissoUrl = Request.Url.Scheme + "://" + Request.Url.Authority;
                        iText.IO.Image.ImageData imageData = iText.IO.Image.ImageDataFactory.Create(prefissoUrl + "/Images/logoVSP_trim.png");

                        PdfWriter wr = new PdfWriter(mapPathNotaSpese);

                        PdfDocument doc = new PdfDocument(wr);
                        doc.SetDefaultPageSize(iText.Kernel.Geom.PageSize.A4.Rotate());
                        Document document = new Document(doc);

                        document.SetMargins(50, 30, 50, 30);

                        // COLORE BLU VIDEOSYSTEM
                        iText.Kernel.Colors.Color coloreIntestazioni = new iText.Kernel.Colors.DeviceRgb(33, 150, 243);

                        Paragraph pIntestazioneNotaSpese = new Paragraph("Nota Spese di: " + figuraProfessionaleSelezionata.Nome + " " + figuraProfessionaleSelezionata.Cognome);
                        pIntestazioneNotaSpese.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        pIntestazioneNotaSpese.SetFontSize(20);
                        pIntestazioneNotaSpese.SetBold();
                        document.Add(pIntestazioneNotaSpese);

                        // AGGIUNGO TABLE PER LAYOUT INTESTAZIONE
                        iText.Layout.Element.Table tbIntestazione = new iText.Layout.Element.Table(new float[] { 1, 9 }).UseAllAvailableWidth().SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        iText.Layout.Element.Image image = new iText.Layout.Element.Image(imageData).ScaleAbsolute(90, 80).SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
                        Cell cellaImmagine = new Cell().SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE).SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
                        cellaImmagine.Add(image);
                        tbIntestazione.AddCell(cellaImmagine);

                        iText.Layout.Element.Table tbIntestazioneDx = new iText.Layout.Element.Table(new float[] { 2, 3, 2, 3 }).UseAllAvailableWidth().SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                        Anag_Clienti_Fornitori cliente = Anag_Clienti_Fornitori_BLL.Instance.getAziendaById(eventoSelezionato.id_cliente, ref esito);

                        Paragraph pTitolo = new Paragraph("Cliente").SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        Paragraph pValore = new Paragraph(cliente.RagioneSociale.Trim()).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE);
                        tbIntestazioneDx.AddCell(pValore).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        pTitolo = new Paragraph("Referente").SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        string nomeReferente = "";
                        if (eventoSelezionato.LavorazioneCorrente.IdReferente != null)
                        {
                            Anag_Referente_Clienti_Fornitori referente = Anag_Referente_Clienti_Fornitori_BLL.Instance.getReferenteById(ref esito, Convert.ToInt32(eventoSelezionato.LavorazioneCorrente.IdReferente.Value));
                            nomeReferente = referente.Nome + " " + referente.Cognome;
                        }
                        pValore = new Paragraph(nomeReferente).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(pValore).SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                        pTitolo = new Paragraph("Produzione").SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        pValore = new Paragraph(eventoSelezionato.produzione).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(pValore).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        pTitolo = new Paragraph("Capotecnico").SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        string nomeCapotecnico = "";
                        if (eventoSelezionato.LavorazioneCorrente.IdCapoTecnico != null)
                        {
                            Anag_Collaboratori coll = Anag_Collaboratori_BLL.Instance.getCollaboratoreById(eventoSelezionato.LavorazioneCorrente.IdCapoTecnico.Value, ref esito);
                            nomeCapotecnico = coll.Nome + " " + coll.Cognome;
                        }
                        pValore = new Paragraph(nomeCapotecnico).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(pValore).SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                        pTitolo = new Paragraph("Lavorazione").SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(eventoSelezionato.lavorazione).SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                        pTitolo = new Paragraph("Data Inizio").SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(eventoSelezionato.data_inizio_impegno.ToShortDateString()).SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                        pTitolo = new Paragraph("Luogo").SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(eventoSelezionato.luogo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                        pTitolo = new Paragraph("Data Lavoraz.").SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(eventoSelezionato.data_inizio_lavorazione.ToShortDateString()).SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                        pTitolo = new Paragraph("Indirizzo").SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(eventoSelezionato.indirizzo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                        pTitolo = new Paragraph("Cod.Lavor.").SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(eventoSelezionato.codice_lavoro).SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                        tbIntestazione.AddCell(tbIntestazioneDx).SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                        document.Add(tbIntestazione);

                        Paragraph pSpazio = new Paragraph(" ");
                        document.Add(pSpazio);

                        Paragraph pLuogoData = new Paragraph(cittaVs + ", " + DateTime.Today.ToLongDateString());
                        pLuogoData.SetFontSize(8);
                        document.Add(pLuogoData);

                        document.Add(pSpazio);

                        // INTESTAZIONE GRIGLIA
                        iText.Layout.Element.Table table = new iText.Layout.Element.Table(new float[] { 90, 70, 70, 70, 70, 70, 70, 70, 200 }).SetWidth(780);

                        Border bordoDoppio = new DoubleBorder(1);

                        Cell cella = new Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        table.AddHeaderCell(cella);

                        Paragraph intestazioneMain = new Paragraph("Tipologia di pagamento").SetFontSize(10).SetBold().SetBackgroundColor(coloreIntestazioni, 0.7f);
                        cella = new Cell(1, 3).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        cella.SetBorderLeft(bordoDoppio);
                        cella.Add(intestazioneMain);
                        table.AddHeaderCell(cella);

                        intestazioneMain = new Paragraph("Costi in Euro").SetFontSize(10).SetBold().SetBackgroundColor(coloreIntestazioni, 0.7f);
                        cella = new Cell(1, 5).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        cella.SetBorderLeft(bordoDoppio);
                        cella.Add(intestazioneMain);
                        table.AddHeaderCell(cella);

                        Paragraph intestazione = new Paragraph("Data").SetFontSize(10).SetBold().SetBackgroundColor(coloreIntestazioni, 0.7f);
                        table.AddHeaderCell(intestazione).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER); 
                        intestazione = new Paragraph("Sofid").SetFontSize(10).SetBold().SetFontSize(10).SetBold().SetBackgroundColor(coloreIntestazioni, 0.7f);
                        cella = new Cell().SetBorderLeft(bordoDoppio);
                        cella.Add(intestazione);
                        table.AddHeaderCell(cella).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER); 
                        intestazione = new Paragraph("Carta").SetFontSize(10).SetFontSize(10).SetBold().SetBackgroundColor(coloreIntestazioni, 0.7f);
                        table.AddHeaderCell(intestazione).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        intestazione = new Paragraph("Contanti").SetFontSize(10).SetFontSize(10).SetBold().SetBackgroundColor(coloreIntestazioni, 0.7f);
                        table.AddHeaderCell(intestazione).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        intestazione = new Paragraph("Carburante").SetFontSize(10).SetFontSize(10).SetBold().SetBackgroundColor(coloreIntestazioni, 0.7f);
                        cella = new Cell().SetBorderLeft(bordoDoppio);
                        cella.Add(intestazione);
                        table.AddHeaderCell(cella).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        intestazione = new Paragraph("Albergo").SetFontSize(10).SetFontSize(10).SetBold().SetBackgroundColor(coloreIntestazioni, 0.7f);
                        table.AddHeaderCell(intestazione).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        intestazione = new Paragraph("Trasporti").SetFontSize(10).SetFontSize(10).SetBold().SetBackgroundColor(coloreIntestazioni, 0.7f);
                        table.AddHeaderCell(intestazione).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        intestazione = new Paragraph("Pasti").SetFontSize(10).SetFontSize(10).SetBold().SetBackgroundColor(coloreIntestazioni, 0.7f);
                        table.AddHeaderCell(intestazione).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        intestazione = new Paragraph("Varie").SetFontSize(10).SetBold().SetBackgroundColor(coloreIntestazioni, 0.7f);
                        table.AddHeaderCell(intestazione).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER); 

                        // CELLE VUOTE
                        for (int i = 0; i < 12; i++)
                        {
                            table.AddCell(new Cell().SetHeight(15));

                            cella = new Cell().SetBorderLeft(bordoDoppio);
                            table.AddCell(cella);

                            table.AddCell(" ");
                            table.AddCell(" ");

                            cella = new Cell().SetBorderLeft(bordoDoppio);
                            table.AddCell(cella);

                            table.AddCell(" ");
                            table.AddCell(" ");
                            table.AddCell(" ");
                            table.AddCell(" ");

                        }
                        document.Add(table);

                        Paragraph pFirma = new Paragraph("in fede");
                        pFirma.SetFontSize(9);
                        pFirma.SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                        pFirma.SetMarginRight(90);
                        pFirma.SetMarginTop(13);
                        document.Add(pFirma);


                        iText.Kernel.Geom.Rectangle pageSize = doc.GetPage(1).GetPageSize();
                        int n = doc.GetNumberOfPages();


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

                        document.Close();
                        wr.Close();

                        framePdfNotaSpese.Attributes.Remove("src");
                        framePdfNotaSpese.Attributes.Add("src", pathNotaSpese.Replace("~", ""));

                        DivFramePdfNotaSpese.Visible = true;
                        framePdfNotaSpese.Visible = true;

                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "aggiornaFrame", script: "javascript: document.getElementById('" + framePdfNotaSpese.ClientID + "').contentDocument.location.reload(true);", addScriptTags: true);
                        btnStampaNotaSpese.Attributes.Add("onclick", "window.open('" + pathNotaSpese.Replace("~", "") + "');");
                    }
                }
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.Descrizione = "PopolaPannelloNotaSpese(DatiAgenda eventoSelezionato, FiguraProfessionale figuraProfessionaleSelezionata) " + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }


        public Esito PopolaPannelloNotaSpese2(DatiAgenda eventoSelezionato, FiguraProfessionale figuraProfessionaleSelezionata)
        {
            Esito esito = new Esito();
            try
            {
                if (eventoSelezionato != null && eventoSelezionato.LavorazioneCorrente != null)
                {
                    #region LEGGO I PARAMETRI DI VS
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
                    #endregion

                    List<DatiPianoEsternoLavorazione> listaDatiPianoEsternoLavorazione = eventoSelezionato.LavorazioneCorrente.ListaDatiPianoEsternoLavorazione;
                    if (listaDatiPianoEsternoLavorazione != null)
                    {
                        string nomeFile = "NotaSpese_" + figuraProfessionaleSelezionata.Cognome + "_" + figuraProfessionaleSelezionata.Nome + eventoSelezionato.codice_lavoro + ".pdf";
                        //string pathNotaSpese = ConfigurationManager.AppSettings["PATH_DOCUMENTI_PROTOCOLLO"] + nomeFile;
                        //string mapPathNotaSpese = MapPath(ConfigurationManager.AppSettings["PATH_DOCUMENTI_PROTOCOLLO"]) + nomeFile;

                        string prefissoUrl = Request.Url.Scheme + "://" + Request.Url.Authority;
                        iText.IO.Image.ImageData imageData = iText.IO.Image.ImageDataFactory.Create(prefissoUrl + "/Images/logoVSP_trim.png");

                        //PdfWriter wr = new PdfWriter(mapPathNotaSpese);




                        //System.IO.FileStream fs = new System.IO.FileStream(Server.MapPath("pdf") + "\\" + nomeFile, System.IO.FileMode.Create);
                        //PdfWriter wr = new PdfWriter(fs);


                        using (MemoryStream ms = new MemoryStream())
                        {


                            PdfWriter wr = new PdfWriter(ms);



                            PdfDocument doc = new PdfDocument(wr);
                            doc.SetDefaultPageSize(iText.Kernel.Geom.PageSize.A4.Rotate());
                            Document document = new Document(doc);

                            document.SetMargins(50, 30, 50, 30);

                            // COLORE BLU VIDEOSYSTEM
                            iText.Kernel.Colors.Color coloreIntestazioni = new iText.Kernel.Colors.DeviceRgb(33, 150, 243);

                            Paragraph pIntestazioneNotaSpese = new Paragraph("Nota Spese di: " + figuraProfessionaleSelezionata.Nome + " " + figuraProfessionaleSelezionata.Cognome);
                            pIntestazioneNotaSpese.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                            pIntestazioneNotaSpese.SetFontSize(20);
                            pIntestazioneNotaSpese.SetBold();
                            document.Add(pIntestazioneNotaSpese);

                            // AGGIUNGO TABLE PER LAYOUT INTESTAZIONE
                            iText.Layout.Element.Table tbIntestazione = new iText.Layout.Element.Table(new float[] { 1, 9 }).UseAllAvailableWidth().SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                            iText.Layout.Element.Image image = new iText.Layout.Element.Image(imageData).ScaleAbsolute(90, 80).SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
                            Cell cellaImmagine = new Cell().SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE).SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
                            cellaImmagine.Add(image);
                            tbIntestazione.AddCell(cellaImmagine);

                            iText.Layout.Element.Table tbIntestazioneDx = new iText.Layout.Element.Table(new float[] { 2, 3, 2, 3 }).UseAllAvailableWidth().SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                            Anag_Clienti_Fornitori cliente = Anag_Clienti_Fornitori_BLL.Instance.getAziendaById(eventoSelezionato.id_cliente, ref esito);

                            Paragraph pTitolo = new Paragraph("Cliente").SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                            tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                            Paragraph pValore = new Paragraph(cliente.RagioneSociale.Trim()).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE);
                            tbIntestazioneDx.AddCell(pValore).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                            pTitolo = new Paragraph("Referente").SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                            tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                            string nomeReferente = "";
                            if (eventoSelezionato.LavorazioneCorrente.IdReferente != null)
                            {
                                Anag_Referente_Clienti_Fornitori referente = Anag_Referente_Clienti_Fornitori_BLL.Instance.getReferenteById(ref esito, Convert.ToInt32(eventoSelezionato.LavorazioneCorrente.IdReferente.Value));
                                nomeReferente = referente.Nome + " " + referente.Cognome;
                            }
                            pValore = new Paragraph(nomeReferente).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                            tbIntestazioneDx.AddCell(pValore).SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                            pTitolo = new Paragraph("Produzione").SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                            tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                            pValore = new Paragraph(eventoSelezionato.produzione).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                            tbIntestazioneDx.AddCell(pValore).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                            pTitolo = new Paragraph("Capotecnico").SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                            tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                            string nomeCapotecnico = "";
                            if (eventoSelezionato.LavorazioneCorrente.IdCapoTecnico != null)
                            {
                                Anag_Collaboratori coll = Anag_Collaboratori_BLL.Instance.getCollaboratoreById(eventoSelezionato.LavorazioneCorrente.IdCapoTecnico.Value, ref esito);
                                nomeCapotecnico = coll.Nome + " " + coll.Cognome;
                            }
                            pValore = new Paragraph(nomeCapotecnico).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                            tbIntestazioneDx.AddCell(pValore).SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                            pTitolo = new Paragraph("Lavorazione").SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                            tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                            tbIntestazioneDx.AddCell(eventoSelezionato.lavorazione).SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                            pTitolo = new Paragraph("Data Inizio").SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                            tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                            tbIntestazioneDx.AddCell(eventoSelezionato.data_inizio_impegno.ToShortDateString()).SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                            pTitolo = new Paragraph("Luogo").SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                            tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                            tbIntestazioneDx.AddCell(eventoSelezionato.luogo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                            pTitolo = new Paragraph("Data Lavoraz.").SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                            tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                            tbIntestazioneDx.AddCell(eventoSelezionato.data_inizio_lavorazione.ToShortDateString()).SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                            pTitolo = new Paragraph("Indirizzo").SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                            tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                            tbIntestazioneDx.AddCell(eventoSelezionato.indirizzo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                            pTitolo = new Paragraph("Cod.Lavor.").SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                            tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                            tbIntestazioneDx.AddCell(eventoSelezionato.codice_lavoro).SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                            tbIntestazione.AddCell(tbIntestazioneDx).SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                            document.Add(tbIntestazione);

                            Paragraph pSpazio = new Paragraph(" ");
                            document.Add(pSpazio);

                            Paragraph pLuogoData = new Paragraph(cittaVs + ", " + DateTime.Today.ToLongDateString());
                            pLuogoData.SetFontSize(8);
                            document.Add(pLuogoData);

                            document.Add(pSpazio);

                            // INTESTAZIONE GRIGLIA
                            iText.Layout.Element.Table table = new iText.Layout.Element.Table(new float[] { 90, 70, 70, 70, 70, 70, 70, 70, 200 }).SetWidth(780);

                            Border bordoDoppio = new DoubleBorder(1);

                            Cell cella = new Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                            table.AddHeaderCell(cella);

                            Paragraph intestazioneMain = new Paragraph("Tipologia di pagamento").SetFontSize(10).SetBold().SetBackgroundColor(coloreIntestazioni, 0.7f);
                            cella = new Cell(1, 3).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                            cella.SetBorderLeft(bordoDoppio);
                            cella.Add(intestazioneMain);
                            table.AddHeaderCell(cella);

                            intestazioneMain = new Paragraph("Costi in Euro").SetFontSize(10).SetBold().SetBackgroundColor(coloreIntestazioni, 0.7f);
                            cella = new Cell(1, 5).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                            cella.SetBorderLeft(bordoDoppio);
                            cella.Add(intestazioneMain);
                            table.AddHeaderCell(cella);

                            Paragraph intestazione = new Paragraph("Data").SetFontSize(10).SetBold().SetBackgroundColor(coloreIntestazioni, 0.7f);
                            table.AddHeaderCell(intestazione).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                            intestazione = new Paragraph("Sofid").SetFontSize(10).SetBold().SetFontSize(10).SetBold().SetBackgroundColor(coloreIntestazioni, 0.7f);
                            cella = new Cell().SetBorderLeft(bordoDoppio);
                            cella.Add(intestazione);
                            table.AddHeaderCell(cella).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                            intestazione = new Paragraph("Carta").SetFontSize(10).SetFontSize(10).SetBold().SetBackgroundColor(coloreIntestazioni, 0.7f);
                            table.AddHeaderCell(intestazione).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                            intestazione = new Paragraph("Contanti").SetFontSize(10).SetFontSize(10).SetBold().SetBackgroundColor(coloreIntestazioni, 0.7f);
                            table.AddHeaderCell(intestazione).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                            intestazione = new Paragraph("Carburante").SetFontSize(10).SetFontSize(10).SetBold().SetBackgroundColor(coloreIntestazioni, 0.7f);
                            cella = new Cell().SetBorderLeft(bordoDoppio);
                            cella.Add(intestazione);
                            table.AddHeaderCell(cella).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                            intestazione = new Paragraph("Albergo").SetFontSize(10).SetFontSize(10).SetBold().SetBackgroundColor(coloreIntestazioni, 0.7f);
                            table.AddHeaderCell(intestazione).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                            intestazione = new Paragraph("Trasporti").SetFontSize(10).SetFontSize(10).SetBold().SetBackgroundColor(coloreIntestazioni, 0.7f);
                            table.AddHeaderCell(intestazione).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                            intestazione = new Paragraph("Pasti").SetFontSize(10).SetFontSize(10).SetBold().SetBackgroundColor(coloreIntestazioni, 0.7f);
                            table.AddHeaderCell(intestazione).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                            intestazione = new Paragraph("Varie").SetFontSize(10).SetBold().SetBackgroundColor(coloreIntestazioni, 0.7f);
                            table.AddHeaderCell(intestazione).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);

                            // CELLE VUOTE
                            for (int i = 0; i < 12; i++)
                            {
                                table.AddCell(new Cell().SetHeight(15));

                                cella = new Cell().SetBorderLeft(bordoDoppio);
                                table.AddCell(cella);

                                table.AddCell(" ");
                                table.AddCell(" ");

                                cella = new Cell().SetBorderLeft(bordoDoppio);
                                table.AddCell(cella);

                                table.AddCell(" ");
                                table.AddCell(" ");
                                table.AddCell(" ");
                                table.AddCell(" ");

                            }
                            document.Add(table);

                            Paragraph pFirma = new Paragraph("in fede");
                            pFirma.SetFontSize(9);
                            pFirma.SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                            pFirma.SetMarginRight(90);
                            pFirma.SetMarginTop(13);
                            document.Add(pFirma);


                            iText.Kernel.Geom.Rectangle pageSize = doc.GetPage(1).GetPageSize();
                            int n = doc.GetNumberOfPages();


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

                            document.Close();
                            wr.Close();

                            //framePdfNotaSpese.Attributes.Remove("src");
                            //framePdfNotaSpese.Attributes.Add("src", pathNotaSpese.Replace("~", ""));

                            DivFramePdfNotaSpese.Visible = true;
                            framePdfNotaSpese.Visible = true;

                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "aggiornaFrame", script: "javascript: document.getElementById('" + framePdfNotaSpese.ClientID + "').contentDocument.location.reload(true);", addScriptTags: true);
                            //btnStampaNotaSpese.Attributes.Add("onclick", "window.open('" + pathNotaSpese.Replace("~", "") + "');");


                            Response.ContentType = "pdf/application";
                            Response.AddHeader("content-disposition", "attachment;filename=" + nomeFile);
                            Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.Descrizione = "PopolaPannelloNotaSpese(DatiAgenda eventoSelezionato, FiguraProfessionale figuraProfessionaleSelezionata) " + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

        #region COMPORTAMENTO ELEMENTI PAGINA
        protected void btnStampaNotaSpese_Click(object sender, EventArgs e)
        {
        }
        #endregion

    }


}