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
    public partial class RiepilogoPianoEsterno : System.Web.UI.UserControl
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


                    // GESTIONE NOMI FILE PDF
                    string nomeFile = "PianoEsterno_" + eventoSelezionato.LavorazioneCorrente.Id.ToString() + ".pdf";
                    string pathPianoEsterno = ConfigurationManager.AppSettings["PATH_DOCUMENTI_PIANOESTERNO"] + nomeFile;
                    string mapPathPianoEsterno = MapPath(ConfigurationManager.AppSettings["PATH_DOCUMENTI_PIANOESTERNO"]) + nomeFile;
                    //string mapPathPdfSenzaNumeroPagina = MapPath(ConfigurationManager.AppSettings["PATH_DOCUMENTI_PIANOESTERNO"]) + "tmp_" + nomeFile;

                    List<DatiPianoEsternoLavorazione> listaDatiPianoEsternoLavorazione = eventoSelezionato.LavorazioneCorrente.ListaDatiPianoEsternoLavorazione;
                    if (listaDatiPianoEsternoLavorazione != null)
                    {
                        string prefissoUrl = Request.Url.Scheme + "://" + Request.Url.Authority;
                        iText.IO.Image.ImageData imageData = iText.IO.Image.ImageDataFactory.Create(prefissoUrl + "/Images/logoVSP_trim.png");
                        

                        PdfWriter wr = new PdfWriter(mapPathPianoEsterno);
                        PdfDocument doc = new PdfDocument(wr);
                        doc.SetDefaultPageSize(iText.Kernel.Geom.PageSize.A4.Rotate());
                        //doc.SetDefaultPageSize(iText.Kernel.Geom.PageSize.A4);
                        Document document = new Document(doc);

                        //document.SetMargins(90, 30, 50, 30);
                        document.SetMargins(50, 30, 50, 30);

                        //// AGGIUNGO TABLE TEST BORDI
                        //iText.Layout.Element.Table tbTest = new iText.Layout.Element.Table(new float[] { 4, 6 }).SetMargin(10).SetWidth(300).SetHeight(200).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.YELLOW, 30);

                        //Paragraph pTest = new Paragraph("in alto a sinistra").SetFontSize(10);
                        //iText.Layout.Element.Cell cellaTest = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        //cellaTest.Add(pTest);
                        //tbTest.AddCell(cellaTest);

                        //pTest = new Paragraph("in alto a destra").SetFontSize(9);
                        //cellaTest = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        //cellaTest.Add(pTest);
                        //tbTest.AddCell(cellaTest);

                        //pTest = new Paragraph("in basso a sinistra").SetFontSize(8);
                        //cellaTest = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        //cellaTest.Add(pTest);
                        //tbTest.AddCell(cellaTest);

                        //pTest = new Paragraph("in basso a destra").SetFontSize(12);
                        //cellaTest = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        //cellaTest.Add(pTest);
                        //tbTest.AddCell(cellaTest);

                        //document.Add(tbTest);


                        //// AGGIUNGO TABLE TEST BORDI
                        //iText.Layout.Element.Table tbTest2 = new iText.Layout.Element.Table(new float[] { 4, 6 }).SetMargin(10).SetWidth(300).SetHeight(200).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.RED, 30).SetFixedPosition(355,335,300);

                        //Paragraph pTest2 = new Paragraph("in alto a sinistra").SetFontSize(10);
                        //iText.Layout.Element.Cell cellaTest2 = new iText.Layout.Element.Cell().SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE,3,100)).SetPadding(5);
                        //cellaTest2.Add(pTest2);
                        //tbTest2.AddCell(cellaTest2);

                        //pTest2 = new Paragraph("in alto a destra").SetFontSize(9);
                        //cellaTest2 = new iText.Layout.Element.Cell().SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 3,100)).SetPadding(5);
                        //cellaTest2.Add(pTest2);
                        //tbTest2.AddCell(cellaTest2);

                        //pTest2 = new Paragraph("in basso a sinistra").SetFontSize(8);
                        //cellaTest2 = new iText.Layout.Element.Cell().SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 3,100)).SetPadding(5);
                        //cellaTest2.Add(pTest2);
                        //tbTest2.AddCell(cellaTest2);

                        //pTest2 = new Paragraph("in basso a destra").SetFontSize(12);
                        //cellaTest2 = new iText.Layout.Element.Cell().SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 3,100)).SetPadding(5);
                        //cellaTest2.Add(pTest2);
                        //tbTest2.AddCell(cellaTest2);

                        //document.Add(tbTest2);



                        // AGGIUNGO TABLE PER LAYOUT INTESTAZIONE
                        iText.Layout.Element.Table tbIntestazione = new iText.Layout.Element.Table(new float[] { 1, 9 }).UseAllAvailableWidth().SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        //iText.Layout.Element.Table tbIntestazione = new iText.Layout.Element.Table(3).UseAllAvailableWidth();
                        iText.Layout.Element.Image image = new iText.Layout.Element.Image(imageData).ScaleAbsolute(80, 80).SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazione.AddCell(image);

                        iText.Layout.Element.Table tbIntestazioneDx = new iText.Layout.Element.Table(new float[] { 2, 3, 2, 3 }).UseAllAvailableWidth().SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                        Anag_Clienti_Fornitori cliente = Anag_Clienti_Fornitori_BLL.Instance.getAziendaById(eventoSelezionato.id_cliente, ref esito);

                        Paragraph pTitolo = new Paragraph("Cliente").SetBackgroundColor(iText.Kernel.Colors.ColorConstants.BLUE).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        Paragraph pValore = new Paragraph(cliente.RagioneSociale.Trim()).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE);
                        tbIntestazioneDx.AddCell(pValore).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        pTitolo = new Paragraph("Referente").SetBackgroundColor(iText.Kernel.Colors.ColorConstants.BLUE).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        string nomeReferente = "";
                        if (eventoSelezionato.LavorazioneCorrente.IdReferente != null)
                        {
                            Anag_Referente_Clienti_Fornitori referente = Anag_Referente_Clienti_Fornitori_BLL.Instance.getReferenteById(ref esito, Convert.ToInt32(eventoSelezionato.LavorazioneCorrente.IdReferente.Value));
                            nomeReferente = referente.Nome + " " + referente.Cognome;
                        }
                        pValore = new Paragraph(nomeReferente).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(pValore).SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                        pTitolo = new Paragraph("Produzione").SetBackgroundColor(iText.Kernel.Colors.ColorConstants.BLUE).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        pValore = new Paragraph(eventoSelezionato.produzione).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(pValore).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        pTitolo = new Paragraph("Capotecnico").SetBackgroundColor(iText.Kernel.Colors.ColorConstants.BLUE).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        string nomeCapotecnico = "";
                        if (eventoSelezionato.LavorazioneCorrente.IdCapoTecnico != null)
                        {
                            Anag_Collaboratori coll = Anag_Collaboratori_BLL.Instance.getCollaboratoreById(eventoSelezionato.LavorazioneCorrente.IdCapoTecnico.Value, ref esito);
                            nomeCapotecnico = coll.Nome + " " + coll.Cognome;
                        }
                        pValore = new Paragraph(nomeCapotecnico).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(pValore).SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                        pTitolo = new Paragraph("Lavorazione").SetBackgroundColor(iText.Kernel.Colors.ColorConstants.BLUE).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(eventoSelezionato.lavorazione).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        pTitolo = new Paragraph("Data Inizio").SetBackgroundColor(iText.Kernel.Colors.ColorConstants.BLUE).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(eventoSelezionato.data_inizio_impegno.ToShortDateString()).SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                        pTitolo = new Paragraph("Luogo").SetBackgroundColor(iText.Kernel.Colors.ColorConstants.BLUE).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(eventoSelezionato.luogo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        pTitolo = new Paragraph("Cod.Lavor.").SetBackgroundColor(iText.Kernel.Colors.ColorConstants.BLUE).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(eventoSelezionato.codice_lavoro).SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                        pTitolo = new Paragraph("Indirizzo").SetBackgroundColor(iText.Kernel.Colors.ColorConstants.BLUE).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(eventoSelezionato.indirizzo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        pTitolo = new Paragraph("Data Lavoraz.").SetBackgroundColor(iText.Kernel.Colors.ColorConstants.BLUE).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(pTitolo).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(eventoSelezionato.data_inizio_lavorazione.ToShortDateString()).SetBorder(iText.Layout.Borders.Border.NO_BORDER);


                        iText.Layout.Element.Cell cellaNote = new iText.Layout.Element.Cell(2, 4).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        string notePianoEsterno = "";
                        if (eventoSelezionato.LavorazioneCorrente.NotePianoEsterno != null) notePianoEsterno = eventoSelezionato.LavorazioneCorrente.NotePianoEsterno;
                        Paragraph pNotePiano = new Paragraph("Note: " + notePianoEsterno.Trim()).SetFontSize(10).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        cellaNote.Add(pNotePiano).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE).SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        tbIntestazioneDx.AddCell(cellaNote).SetBorder(iText.Layout.Borders.Border.NO_BORDER);


                        tbIntestazione.AddCell(tbIntestazioneDx).SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                        document.Add(tbIntestazione);

                        //Paragraph pIntestazione = new Paragraph("Consuntivo Piano Esterno - Cliente: " + cliente.RagioneSociale.Trim() + " - " + eventoSelezionato.codice_lavoro).SetFontSize(12).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        //document.Add(pIntestazione);

                        //Paragraph pIntestazione2 = new Paragraph("Lavorazione: " + eventoSelezionato.lavorazione + " " + eventoSelezionato.produzione + " del " + eventoSelezionato.data_inizio_lavorazione.ToShortDateString() + " - " + eventoSelezionato.data_fine_lavorazione.ToShortDateString()).SetFontSize(12).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        //document.Add(pIntestazione2);


                        Paragraph pSpazio = new Paragraph(" ");
                        document.Add(pSpazio);

                        Paragraph pLuogoData = new Paragraph(cittaVs +", " + DateTime.Today.ToLongDateString());
                        document.Add(pLuogoData);

                        document.Add(pSpazio);

                        // INTESTAZIONE GRIGLIA

                        iText.Layout.Element.Table table = new iText.Layout.Element.Table(8).UseAllAvailableWidth(); //.SetBorderTop(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.ORANGE, 5)).SetBorderBottom(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.YELLOW, 5)).SetBorderLeft(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.GREEN, 5)).SetBorderRight(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.RED, 5));
                        Paragraph intestazione = new Paragraph("Data").SetFontSize(10).SetBold().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.BLUE);
                        table.AddHeaderCell(intestazione);
                        intestazione = new Paragraph("Orario").SetFontSize(10).SetBold().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.BLUE);
                        table.AddHeaderCell(intestazione);
                        intestazione = new Paragraph("Collaboratore").SetFontSize(10).SetBold().SetFontSize(10).SetBold().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.BLUE);
                        table.AddHeaderCell(intestazione);
                        intestazione = new Paragraph("Qualifica").SetFontSize(10).SetFontSize(10).SetBold().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.BLUE);
                        table.AddHeaderCell(intestazione);
                        intestazione = new Paragraph("Importo Diaria").SetFontSize(10).SetFontSize(10).SetBold().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.BLUE);
                        table.AddHeaderCell(intestazione);
                        intestazione = new Paragraph("Intervento").SetFontSize(10).SetFontSize(10).SetBold().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.BLUE);
                        table.AddHeaderCell(intestazione);
                        intestazione = new Paragraph("Albergo").SetFontSize(10).SetFontSize(10).SetBold().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.BLUE);
                        table.AddHeaderCell(intestazione);
                        intestazione = new Paragraph("Note").SetFontSize(10).SetFontSize(10).SetBold().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.BLUE);
                        table.AddHeaderCell(intestazione);
                        foreach (DatiPianoEsternoLavorazione dpe in listaDatiPianoEsternoLavorazione)
                        {

                            string collaboratoreFornitore = "";
                            string qualifica = "";
                            if (dpe.IdCollaboratori != null)
                            {
                                Anag_Collaboratori coll = Anag_Collaboratori_BLL.Instance.getCollaboratoreById(dpe.IdCollaboratori.Value, ref esito);
                                collaboratoreFornitore = coll.Cognome.Trim() + " " + coll.Nome.Trim();

                                FiguraProfessionale fp = coll.CreaFiguraProfessionale();
                                if (fp!=null && !string.IsNullOrEmpty(fp.ElencoQualifiche)) qualifica = fp.ElencoQualifiche;
                            }
                            else if (dpe.IdFornitori != null)
                            {
                                Anag_Clienti_Fornitori clienteFornitore = Anag_Clienti_Fornitori_BLL.Instance.getAziendaById(dpe.IdFornitori.Value, ref esito);
                                collaboratoreFornitore = clienteFornitore.RagioneSociale.Trim();
                                FiguraProfessionale fp = clienteFornitore.CreaFiguraProfessionale();
                                if (fp != null && !string.IsNullOrEmpty(fp.ElencoQualifiche)) qualifica = fp.ElencoQualifiche;
                            }

                            string importoDiaria = "0,00";
                            if (dpe.ImportoDiaria != null)
                            {
                                importoDiaria = dpe.ImportoDiaria.Value.ToString("###,###.00");
                            }

                            string nota = "";
                            if (!string.IsNullOrEmpty(dpe.Nota)) nota = dpe.Nota;

                            string dataPiano = "";
                            if (dpe.Data != null) dataPiano = dpe.Data.Value.ToLongDateString();

                            string orario = "";
                            if (dpe.Orario != null) orario = dpe.Orario.Value.ToShortTimeString();

                            string intervento = "";
                            if (dpe.IdIntervento != null)
                            {
                                intervento = SessionManager.ListaTipiIntervento.FirstOrDefault(x => x.id == dpe.IdIntervento).nome;
                            }

                            string albergo = "no";
                            if (dpe.Albergo != null && dpe.Albergo == true) albergo = "si";

                            //Paragraph p = new Paragraph(dpe.Data.Value.ToLongDateString() + " " + orario + " " + collaboratoreFornitore + " " + nota).SetFontSize(8);
                            //document.Add(p);
                            table.AddCell(dataPiano).SetFontSize(8).SetFontSize(10);
                            table.AddCell(orario).SetFontSize(8);
                            table.AddCell(collaboratoreFornitore);
                            table.AddCell(qualifica).SetFontSize(8);
                            Paragraph pImportoDiaria = new Paragraph(importoDiaria).SetFontSize(8).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                            table.AddCell(pImportoDiaria);
                            table.AddCell(intervento).SetFontSize(8);
                            table.AddCell(albergo).SetFontSize(8);
                            table.AddCell(nota).SetFontSize(8);

                        }
                        document.Add(table);

                        //document.Add(pSpazio);

                        //Paragraph pIntNote = new Paragraph("Note:").SetFontSize(12).SetBold();
                        //document.Add(pIntNote);

                        //document.Add(pSpazio);


                        //Paragraph pNotePiano = new Paragraph(notePianoEsterno.Trim()).SetFontSize(10);
                        //document.Add(pNotePiano);

                        iText.Kernel.Geom.Rectangle pageSize = doc.GetPage(1).GetPageSize();
                        //iText.Layout.Element.Image image = new iText.Layout.Element.Image(imageData).ScaleAbsolute(60, 60).SetFixedPosition(1,20,pageSize.GetHeight()-80);
                        //document.Add(image);

                        int n = doc.GetNumberOfPages();


                        // AGGIUNGO CONTEGGIO PAGINE E FOOTER PER OGNI PAGINA
                        for (int i = 1; i <= n; i++)
                        {
                            // AGGIUNGO LOGO
                            //iText.Layout.Element.Image image = new iText.Layout.Element.Image(imageData).ScaleAbsolute(60, 60).SetFixedPosition(i, 20, pageSize.GetHeight() - 80);
                            //document.Add(image);
                            //AGGIUNGO NUM.PAGINA
                            document.ShowTextAligned(new Paragraph("pagina " + i.ToString() + " di " + n.ToString()).SetFontSize(7),
                                pageSize.GetWidth() - 60, pageSize.GetHeight() - 20, i, iText.Layout.Properties.TextAlignment.CENTER, iText.Layout.Properties.VerticalAlignment.TOP, 0);
                            //AGGIUNGO FOOTER
                            document.ShowTextAligned(new Paragraph(denominazioneVs + " P.IVA " + pIvaVs + Environment.NewLine + "Sede legale: " + toponimoVs + " " + indirizzoVs + " " + civicoVs + " - " + capVs + " " + cittaVs + " " + provinciaVs + " e-mail: " + emailVs ).SetFontSize(7).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER),
                                                            pageSize.GetWidth()/2, 30, i, iText.Layout.Properties.TextAlignment.CENTER, iText.Layout.Properties.VerticalAlignment.TOP, 0);
                        }



                        document.Close();
                        wr.Close();

                        if (File.Exists(mapPathPianoEsterno))
                        {
                            //string nomeFileToDisplay = BaseStampa.Instance.AddPageNumber(mapPathPdfSenzaNumeroPagina, mapPianoEsterno, ref esito);
                            //if (File.Exists(mapPathPdfSenzaNumeroPagina)) File.Delete(mapPathPdfSenzaNumeroPagina);
                            //if (esito.codice == Esito.ESITO_OK) { 
                            framePdfConsuntivo.Attributes.Remove("src");
                                framePdfConsuntivo.Attributes.Add("src", pathPianoEsterno.Replace("~", ""));

                                DivFramePdfConsuntivo.Visible = true;
                                framePdfConsuntivo.Visible = true;

                                ScriptManager.RegisterStartupScript(Page, typeof(Page), "aggiornaFrame", script: "javascript: document.getElementById('" + framePdfConsuntivo.ClientID + "').contentDocument.location.reload(true);", addScriptTags: true);
                                btnStampaConsuntivo.Attributes.Add("onclick", "window.open('" + pathPianoEsterno.Replace("~", "") + "');");
                            //}
                        }
                        else
                        {
                            esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                            esito.Descrizione = "Il File " + pathPianoEsterno.Replace("~", "") + " non è stato creato correttamente!";
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