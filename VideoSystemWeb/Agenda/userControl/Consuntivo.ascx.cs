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
    public partial class Consuntivo : System.Web.UI.UserControl
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

                    // GESTIONE NOMI FILE PDF
                    string nomeFile = "Consuntivo_" + eventoSelezionato.LavorazioneCorrente.Id.ToString() + ".pdf";
                    string pathConsuntivo = ConfigurationManager.AppSettings["PATH_DOCUMENTI_CONSUNTIVO"] + nomeFile;
                    string mapPathConsuntivo = MapPath(ConfigurationManager.AppSettings["PATH_DOCUMENTI_CONSUNTIVO"]) + nomeFile;
                    //string mapPathPdfSenzaNumeroPagina = MapPath(ConfigurationManager.AppSettings["PATH_DOCUMENTI_CONSUNTIVO"]) + "tmp_" + nomeFile;

                    List<DatiPianoEsternoLavorazione> listaDatiPianoEsternoLavorazione = eventoSelezionato.LavorazioneCorrente.ListaDatiPianoEsternoLavorazione;
                    if (listaDatiPianoEsternoLavorazione != null)
                    {
                        string prefissoUrl = Request.Url.Scheme + "://" + Request.Url.Authority;
                        iText.IO.Image.ImageData imageData = iText.IO.Image.ImageDataFactory.Create(prefissoUrl + "/Images/logoVSP_trim.png");
                        

                        PdfWriter wr = new PdfWriter(mapPathConsuntivo);
                        PdfDocument doc = new PdfDocument(wr);
                        doc.SetDefaultPageSize(iText.Kernel.Geom.PageSize.A4.Rotate());
                        //doc.SetDefaultPageSize(iText.Kernel.Geom.PageSize.A4);
                        Document document = new Document(doc);

                        document.SetMargins(90, 30, 50, 30);

                        Anag_Clienti_Fornitori cliente = Anag_Clienti_Fornitori_BLL.Instance.getAziendaById(eventoSelezionato.id_cliente, ref esito);

                        Paragraph pIntestazione = new Paragraph("Consuntivo Piano Esterno - Cliente: " + cliente.RagioneSociale.Trim() + " - " + eventoSelezionato.codice_lavoro).SetFontSize(12).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        document.Add(pIntestazione);

                        Paragraph pIntestazione2 = new Paragraph("Lavorazione: " + eventoSelezionato.lavorazione + " " + eventoSelezionato.produzione + " del " + eventoSelezionato.data_inizio_lavorazione.ToShortDateString() + " - " + eventoSelezionato.data_fine_lavorazione.ToShortDateString()).SetFontSize(12).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        document.Add(pIntestazione2);


                        Paragraph pSpazio = new Paragraph(" ");
                        document.Add(pSpazio);

                        // INTESTAZIONE GRIGLIA
                        iText.Layout.Element.Table table = new iText.Layout.Element.Table(8).UseAllAvailableWidth();
                        Paragraph intestazione = new Paragraph("Data").SetFontSize(10).SetBold();
                        table.AddHeaderCell(intestazione);
                        intestazione = new Paragraph("Orario").SetFontSize(10).SetBold();
                        table.AddHeaderCell(intestazione);
                        intestazione = new Paragraph("Collaboratore").SetFontSize(10).SetBold();
                        table.AddHeaderCell(intestazione);
                        intestazione = new Paragraph("Qualifica").SetFontSize(10).SetBold();
                        table.AddHeaderCell(intestazione);
                        intestazione = new Paragraph("Importo Diaria").SetFontSize(10).SetBold();
                        table.AddHeaderCell(intestazione);
                        intestazione = new Paragraph("Intervento").SetFontSize(10).SetBold();
                        table.AddHeaderCell(intestazione);
                        intestazione = new Paragraph("Albergo").SetFontSize(10).SetBold();
                        table.AddHeaderCell(intestazione);
                        intestazione = new Paragraph("Note").SetFontSize(10).SetBold();
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
                            table.AddCell(dataPiano).SetFontSize(8);
                            table.AddCell(orario).SetFontSize(8);
                            table.AddCell(collaboratoreFornitore).SetFontSize(8);
                            table.AddCell(qualifica).SetFontSize(8);
                            Paragraph pImportoDiaria = new Paragraph(importoDiaria).SetFontSize(8).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                            table.AddCell(pImportoDiaria);
                            table.AddCell(intervento).SetFontSize(8);
                            table.AddCell(albergo).SetFontSize(8);
                            table.AddCell(nota).SetFontSize(8);

                        }
                        document.Add(table);

                        document.Add(pSpazio);

                        Paragraph pIntNote = new Paragraph("Note:").SetFontSize(12).SetBold();
                        document.Add(pIntNote);

                        document.Add(pSpazio);

                        string notePianoEsterno = "";
                        if (eventoSelezionato.LavorazioneCorrente.NotePianoEsterno != null) notePianoEsterno = eventoSelezionato.LavorazioneCorrente.NotePianoEsterno;

                        Paragraph pNotePiano = new Paragraph(notePianoEsterno.Trim()).SetFontSize(10);
                        document.Add(pNotePiano);

                        iText.Kernel.Geom.Rectangle pageSize = doc.GetPage(1).GetPageSize();
                        //iText.Layout.Element.Image image = new iText.Layout.Element.Image(imageData).ScaleAbsolute(60, 60).SetFixedPosition(1,20,pageSize.GetHeight()-80);
                        //document.Add(image);

                        int n = doc.GetNumberOfPages();

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

                        // AGGIUNGO CONTEGGIO PAGINE E FOOTER PER OGNI PAGINA
                        for (int i = 1; i <= n; i++)
                        {
                            // AGGIUNGO LOGO
                            iText.Layout.Element.Image image = new iText.Layout.Element.Image(imageData).ScaleAbsolute(60, 60).SetFixedPosition(i, 20, pageSize.GetHeight() - 80);
                            document.Add(image);
                            //AGGIUNGO NUM.PAGINA
                            document.ShowTextAligned(new Paragraph("pagina " + i.ToString() + " di " + n.ToString()).SetFontSize(7),
                                pageSize.GetWidth() - 60, pageSize.GetHeight() - 20, i, iText.Layout.Properties.TextAlignment.CENTER, iText.Layout.Properties.VerticalAlignment.TOP, 0);
                            //AGGIUNGO FOOTER
                            document.ShowTextAligned(new Paragraph(denominazioneVs + " P.IVA " + pIvaVs + Environment.NewLine + "Sede legale: " + toponimoVs + " " + indirizzoVs + " " + civicoVs + " - " + capVs + " " + cittaVs + " " + provinciaVs + " e-mail: " + emailVs ).SetFontSize(7).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER),
                                                            pageSize.GetWidth()/2, 30, i, iText.Layout.Properties.TextAlignment.CENTER, iText.Layout.Properties.VerticalAlignment.TOP, 0);
                        }



                        document.Close();
                        wr.Close();

                        if (File.Exists(mapPathConsuntivo))
                        {
                            //string nomeFileToDisplay = BaseStampa.Instance.AddPageNumber(mapPathPdfSenzaNumeroPagina, mapPathConsuntivo, ref esito);
                            //if (File.Exists(mapPathPdfSenzaNumeroPagina)) File.Delete(mapPathPdfSenzaNumeroPagina);
                            //if (esito.codice == Esito.ESITO_OK) { 
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
                            esito.codice = Esito.ESITO_KO_ERRORE_GENERICO;
                            esito.descrizione = "Il File " + pathConsuntivo.Replace("~", "") + " non è stato creato correttamente!";
                        }



                    }


                }
            }
            catch (Exception ex)
            {

                esito.codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.descrizione = "popolaPannelloConsuntivo(DatiAgenda eventoSelezionato) " + ex.Message + Environment.NewLine + ex.StackTrace;
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