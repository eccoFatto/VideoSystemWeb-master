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
                scriptManager.RegisterPostBackControl(this.btnStampaPianoEsterno);
            }
        }
        public Esito popolaPannelloPianoEsterno(DatiAgenda eventoSelezionato)
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

                    List<DatiPianoEsternoLavorazione> listaDatiPianoEsternoLavorazione = eventoSelezionato.LavorazioneCorrente.ListaDatiPianoEsternoLavorazione;
                    if (listaDatiPianoEsternoLavorazione != null)
                    {

                        Protocolli protocolloPianoEsterno = new Protocolli();
                        int idTipoProtocollo = UtilityTipologiche.getElementByNome(UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_PROTOCOLLO), "Piano Esterno", ref esito).id;
                        List<Protocolli> listaProtocolli = Protocolli_BLL.Instance.getProtocolliByCodLavIdTipoProtocollo(eventoSelezionato.codice_lavoro, idTipoProtocollo, ref esito, true);
                        string numeroProtocollo = "";
                        if (listaProtocolli.Count == 0)
                        {
                            numeroProtocollo = Protocolli_BLL.Instance.getNumeroProtocollo();
                        }
                        else
                        {
                            protocolloPianoEsterno = listaProtocolli.First();
                            numeroProtocollo = protocolloPianoEsterno.Numero_protocollo;
                        }

                        // GESTIONE NOMI FILE PDF
                        //string nomeFile = "PianoEsterno_" + eventoSelezionato.LavorazioneCorrente.Id.ToString() + ".pdf";
                        //string nomeFile = "PianoEsterno_" + numeroProtocollo + ".pdf";
                        string nomeFile = "PianoEsterno_" + eventoSelezionato.codice_lavoro + ".pdf";
                        string pathPianoEsterno = ConfigurationManager.AppSettings["PATH_DOCUMENTI_PROTOCOLLO"] + nomeFile;
                        string mapPathPianoEsterno = MapPath(ConfigurationManager.AppSettings["PATH_DOCUMENTI_PROTOCOLLO"]) + nomeFile;

                        string prefissoUrl = Request.Url.Scheme + "://" + Request.Url.Authority;
                        iText.IO.Image.ImageData imageData = iText.IO.Image.ImageDataFactory.Create(prefissoUrl + "/Images/logoVSP_trim.png");
                        

                        PdfWriter wr = new PdfWriter(mapPathPianoEsterno);
                        PdfDocument doc = new PdfDocument(wr);
                        doc.SetDefaultPageSize(iText.Kernel.Geom.PageSize.A4.Rotate());
                        //doc.SetDefaultPageSize(iText.Kernel.Geom.PageSize.A4);
                        Document document = new Document(doc);

                        //document.SetMargins(90, 30, 50, 30);
                        document.SetMargins(50, 30, 50, 30);

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

                        Paragraph pSpazio = new Paragraph(" ");
                        document.Add(pSpazio);

                        Paragraph pLuogoData = new Paragraph(cittaVs +", " + DateTime.Today.ToLongDateString());
                        document.Add(pLuogoData);

                        document.Add(pSpazio);

                        // INTESTAZIONE GRIGLIA

                        iText.Layout.Element.Table table = new iText.Layout.Element.Table(10).UseAllAvailableWidth(); //.SetBorderTop(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.ORANGE, 5)).SetBorderBottom(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.YELLOW, 5)).SetBorderLeft(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.GREEN, 5)).SetBorderRight(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.RED, 5));
                        Paragraph intestazione = new Paragraph("Data").SetFontSize(10).SetBold().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.BLUE);
                        table.AddHeaderCell(intestazione);
                        intestazione = new Paragraph("Personale").SetFontSize(10).SetBold().SetFontSize(10).SetBold().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.BLUE);
                        table.AddHeaderCell(intestazione);
                        intestazione = new Paragraph("Qualifica").SetFontSize(10).SetFontSize(10).SetBold().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.BLUE);
                        table.AddHeaderCell(intestazione);
                        intestazione = new Paragraph("Intervento").SetFontSize(10).SetFontSize(10).SetBold().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.BLUE);
                        table.AddHeaderCell(intestazione);
                        intestazione = new Paragraph("Telefono").SetFontSize(10).SetFontSize(10).SetBold().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.BLUE);
                        table.AddHeaderCell(intestazione);
                        intestazione = new Paragraph("Città").SetFontSize(10).SetFontSize(10).SetBold().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.BLUE);
                        table.AddHeaderCell(intestazione);
                        intestazione = new Paragraph("Albergo").SetFontSize(10).SetFontSize(10).SetBold().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.BLUE);
                        table.AddHeaderCell(intestazione);
                        intestazione = new Paragraph("Diaria").SetFontSize(10).SetFontSize(10).SetBold().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.BLUE);
                        table.AddHeaderCell(intestazione);
                        intestazione = new Paragraph("Orario").SetFontSize(10).SetBold().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.BLUE);
                        table.AddHeaderCell(intestazione);
                        intestazione = new Paragraph("Note").SetFontSize(10).SetFontSize(10).SetBold().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.BLUE);
                        table.AddHeaderCell(intestazione);
                        foreach (DatiPianoEsternoLavorazione dpe in listaDatiPianoEsternoLavorazione)
                        {

                            string collaboratoreFornitore = "";
                            string telefono = "";
                            string qualifica = "";
                            string citta = "";

                            string descrizioneArticoloAssociato = "";

                            if (dpe.IdCollaboratori != null)
                            {
                                Anag_Collaboratori coll = Anag_Collaboratori_BLL.Instance.getCollaboratoreById(dpe.IdCollaboratori.Value, ref esito);
                                collaboratoreFornitore = coll.Cognome.Trim() + " " + coll.Nome.Trim();

                                // prendo descrizione da datiArticoliLavorazione filtrando per data, idCollaboratore e idLavorazione
                                DatiArticoliLavorazione articoloAssociato = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.FirstOrDefault(x=>x.IdCollaboratori == coll.Id && x.Data == dpe.Data);
                                if (articoloAssociato != null)
                                {
                                    descrizioneArticoloAssociato = articoloAssociato.Descrizione;
                                }

                                FiguraProfessionale fp = coll.CreaFiguraProfessionale(descrizioneArticoloAssociato);
                                //if (fp!=null && !string.IsNullOrEmpty(fp.ElencoQualifiche)) qualifica = fp.ElencoQualifiche;
                                if (fp != null && !string.IsNullOrEmpty(fp.DescrizioneArticoloAssociato)) qualifica = fp.DescrizioneArticoloAssociato;

                                

                                if (fp != null && !string.IsNullOrEmpty(fp.Telefono)) telefono = fp.Telefono;
                                if (fp != null && !string.IsNullOrEmpty(fp.Citta)) citta = fp.Citta;
                            }
                            else if (dpe.IdFornitori != null)
                            {
                                Anag_Clienti_Fornitori clienteFornitore = Anag_Clienti_Fornitori_BLL.Instance.getAziendaById(dpe.IdFornitori.Value, ref esito);
                                collaboratoreFornitore = clienteFornitore.RagioneSociale.Trim();

                                // prendo descrizione da datiArticoliLavorazione filtrando per data, idFornitore e idLavorazione
                                DatiArticoliLavorazione articoloAssociato = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.FirstOrDefault(x => x.IdFornitori == clienteFornitore.Id && x.Data == dpe.Data);
                                if (articoloAssociato != null)
                                {
                                    descrizioneArticoloAssociato = articoloAssociato.Descrizione;
                                }

                                FiguraProfessionale fp = clienteFornitore.CreaFiguraProfessionale(descrizioneArticoloAssociato);
                                //if (fp != null && !string.IsNullOrEmpty(fp.ElencoQualifiche)) qualifica = fp.ElencoQualifiche;
                                if (fp != null && !string.IsNullOrEmpty(fp.DescrizioneArticoloAssociato)) qualifica = fp.DescrizioneArticoloAssociato;
                                if (fp != null && !string.IsNullOrEmpty(fp.Telefono)) telefono = fp.Telefono;
                                if (fp != null && !string.IsNullOrEmpty(fp.Citta)) citta = fp.Citta;
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
                            table.AddCell(collaboratoreFornitore);

                            
                            table.AddCell(qualifica).SetFontSize(8);

                            table.AddCell(intervento).SetFontSize(8);

                            table.AddCell(telefono).SetFontSize(8);
                            table.AddCell(citta).SetFontSize(8);

                            table.AddCell(albergo).SetFontSize(8);
                            Paragraph pImportoDiaria = new Paragraph(importoDiaria).SetFontSize(8).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                            table.AddCell(pImportoDiaria);
                            table.AddCell(orario).SetFontSize(8);
                            table.AddCell(nota).SetFontSize(8);

                        }
                        document.Add(table);


                        iText.Kernel.Geom.Rectangle pageSize = doc.GetPage(1).GetPageSize();
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

                            // SE FILE OK INSERISCO O AGGIORNO PROTOCOLLO DI TIPO PIANO ESTERNO
                            if (listaProtocolli.Count == 0)
                            {
                                //INSERISCO
                                protocolloPianoEsterno.Attivo = true;
                                protocolloPianoEsterno.Cliente = cliente.RagioneSociale.Trim();
                                protocolloPianoEsterno.Codice_lavoro = eventoSelezionato.codice_lavoro;
                                protocolloPianoEsterno.Data_inizio_lavorazione = eventoSelezionato.data_inizio_impegno;
                                protocolloPianoEsterno.Data_protocollo = DateTime.Today;
                                protocolloPianoEsterno.Descrizione = "Piano Esterno";
                                protocolloPianoEsterno.Id_cliente = eventoSelezionato.id_cliente;
                                protocolloPianoEsterno.Id_tipo_protocollo = idTipoProtocollo;
                                protocolloPianoEsterno.Lavorazione = eventoSelezionato.lavorazione;
                                protocolloPianoEsterno.PathDocumento = Path.GetFileName(mapPathPianoEsterno);
                                protocolloPianoEsterno.Produzione = eventoSelezionato.produzione;
                                protocolloPianoEsterno.Protocollo_riferimento = "";
                                protocolloPianoEsterno.Numero_protocollo = numeroProtocollo;
                                int idProtPianoEsterno = Protocolli_BLL.Instance.CreaProtocollo(protocolloPianoEsterno, ref esito);
                            }
                            else
                            {
                                // AGGIORNO
                                protocolloPianoEsterno.PathDocumento = Path.GetFileName(mapPathPianoEsterno);
                                esito = Protocolli_BLL.Instance.AggiornaProtocollo(protocolloPianoEsterno);
                            }

                            //string nomeFileToDisplay = BaseStampa.Instance.AddPageNumber(mapPathPdfSenzaNumeroPagina, mapPianoEsterno, ref esito);
                            //if (File.Exists(mapPathPdfSenzaNumeroPagina)) File.Delete(mapPathPdfSenzaNumeroPagina);
                            //if (esito.codice == Esito.ESITO_OK) { 
                            framePdfPianoEsterno.Attributes.Remove("src");
                                framePdfPianoEsterno.Attributes.Add("src", pathPianoEsterno.Replace("~", ""));

                                DivFramePdfPianoEsterno.Visible = true;
                                framePdfPianoEsterno.Visible = true;

                                ScriptManager.RegisterStartupScript(Page, typeof(Page), "aggiornaFrame", script: "javascript: document.getElementById('" + framePdfPianoEsterno.ClientID + "').contentDocument.location.reload(true);", addScriptTags: true);
                                btnStampaPianoEsterno.Attributes.Add("onclick", "window.open('" + pathPianoEsterno.Replace("~", "") + "');");
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
                esito.Descrizione = "popolaPannelloPianoEsterno(DatiAgenda eventoSelezionato) " + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }



        #region COMPORTAMENTO ELEMENTI PAGINA
        protected void btnStampaPianoEsterno_Click(object sender, EventArgs e)
        {
        }


        #endregion

        #region OPERAZIONI POPUP

        #endregion

    }

}