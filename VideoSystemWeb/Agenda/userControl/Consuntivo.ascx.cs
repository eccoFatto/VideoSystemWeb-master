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
        public delegate void PopupHandler(string operazionePopup); // delegato per l'evento
        public event PopupHandler RichiediOperazionePopup; //evento

        public delegate List<DatiArticoli> ListaArticoliHandler(); // delegato per l'evento
        public event ListaArticoliHandler RichiediListaArticoli; //evento

        public delegate string CodiceLavoroHandler(); // delegato per l'evento
        public event CodiceLavoroHandler RichiediCodiceLavoro; //evento

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(this.btnStampaConsuntivo);
            }
            else
            {

                /*
                    select data,ac.cognome + ' ' + ac.nome, acf.ragioneSociale,dpel.orario,dpel.importoDiaria,dpel.idIntervento, dpel.nota from dbo.dati_pianoEsterno_lavorazione dpel
                    left join anag_collaboratori ac on dpel.idCollaboratori = ac.id
                    left join anag_clienti_fornitori acf
                    on dpel.idFornitori = acf.id
                    where dpel.idDatiLavorazione = 1173
                */
            }
        }
        public Esito popolaPannelloConsuntivo(DatiAgenda eventoSelezionato)
        {
            Esito esito = new Esito();
            try
            {
                if (eventoSelezionato != null && eventoSelezionato.LavorazioneCorrente != null)
                {

                    string nomeFile = "Consuntivo_" + eventoSelezionato.LavorazioneCorrente.Id.ToString() + ".pdf";

                    string pathConsuntivo = ConfigurationManager.AppSettings["PATH_DOCUMENTI_CONSUNTIVO"] + nomeFile;

                    string mapPathConsuntivo = MapPath(ConfigurationManager.AppSettings["PATH_DOCUMENTI_CONSUNTIVO"]) + nomeFile;

                    List<DatiPianoEsternoLavorazione> listaDatiPianoEsternoLavorazione = eventoSelezionato.LavorazioneCorrente.ListaDatiPianoEsternoLavorazione;
                    if (listaDatiPianoEsternoLavorazione != null)
                    {
                        PdfWriter wr = new PdfWriter(mapPathConsuntivo);
                        PdfDocument doc = new PdfDocument(wr);
                        doc.SetDefaultPageSize(iText.Kernel.Geom.PageSize.A4.Rotate());
                        //doc.SetDefaultPageSize(iText.Kernel.Geom.PageSize.A4);
                        Document document = new Document(doc);

                        Anag_Clienti_Fornitori cliente = Anag_Clienti_Fornitori_BLL.Instance.getAziendaById(eventoSelezionato.id_cliente, ref esito);

                        Paragraph pIntestazione = new Paragraph("Consuntivo Piano Esterno - Cliente: " + cliente.RagioneSociale.Trim() + " - " + eventoSelezionato.codice_lavoro).SetFontSize(12).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        document.Add(pIntestazione);

                        Paragraph pIntestazione2 = new Paragraph("Lavorazione: " + eventoSelezionato.lavorazione + " " + eventoSelezionato.produzione + " del " + eventoSelezionato.data_inizio_lavorazione.ToShortDateString() + " - " + eventoSelezionato.data_fine_lavorazione.ToShortDateString()).SetFontSize(12).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        document.Add(pIntestazione2);


                        Paragraph pSpazio = new Paragraph(" ");
                        document.Add(pSpazio);


                        

                        iText.Layout.Element.Table table = new iText.Layout.Element.Table(7).UseAllAvailableWidth();
                        Paragraph intestazione = new Paragraph("Data").SetFontSize(10).SetBold();
                        table.AddHeaderCell(intestazione);
                        intestazione = new Paragraph("Orario").SetFontSize(10).SetBold();
                        table.AddHeaderCell(intestazione);
                        intestazione = new Paragraph("Collaboratore").SetFontSize(10).SetBold();
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
                            
                            if (dpe.IdCollaboratori != null)
                            {
                                Anag_Collaboratori coll = Anag_Collaboratori_BLL.Instance.getCollaboratoreById(dpe.IdCollaboratori.Value,ref esito);
                                collaboratoreFornitore = coll.Cognome.Trim() + " " + coll.Nome.Trim();
                                
                            }
                            else if (dpe.IdFornitori != null)
                            {
                                Anag_Clienti_Fornitori clienteFornitore = Anag_Clienti_Fornitori_BLL.Instance.getAziendaById(dpe.IdFornitori.Value, ref esito);
                                collaboratoreFornitore = clienteFornitore.RagioneSociale.Trim();
                            }

                            string importoDiaria = "0,00";
                            if (dpe.ImportoDiaria != null)
                            {
                                importoDiaria = dpe.ImportoDiaria.Value.ToString("###,###.00");
                            }

                            string nota = "";
                            if (!string.IsNullOrEmpty(dpe.Nota)) nota = dpe.Nota;

                            string orario = "";
                            if (dpe.Orario != null) orario = dpe.Orario.Value.ToLongTimeString();

                            string intervento = "";
                            if (dpe.IdIntervento != null)
                            {
                                intervento = SessionManager.ListaTipiIntervento.FirstOrDefault(x => x.id == dpe.IdIntervento).nome;
                            }

                            string albergo = "no";
                            if (dpe.Albergo != null && dpe.Albergo == true) albergo = "si";

                            //Paragraph p = new Paragraph(dpe.Data.Value.ToLongDateString() + " " + orario + " " + collaboratoreFornitore + " " + nota).SetFontSize(8);
                            //document.Add(p);
                            table.AddCell(dpe.Data.Value.ToLongDateString()).SetFontSize(8);
                            table.AddCell(orario).SetFontSize(8);
                            table.AddCell(collaboratoreFornitore).SetFontSize(8);
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

                        document.Close();
                        wr.Close();

                        if (File.Exists(mapPathConsuntivo))
                        {
                            framePdfConsuntivo.Attributes.Remove("src");
                            framePdfConsuntivo.Attributes.Add("src", pathConsuntivo.Replace("~", ""));

                            DivFramePdfConsuntivo.Visible = true;
                            framePdfConsuntivo.Visible = true;

                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "aggiornaFrame", script: "javascript: document.getElementById('" + framePdfConsuntivo.ClientID + "').contentDocument.location.reload(true);", addScriptTags: true);
                            btnStampaConsuntivo.Attributes.Add("onclick", "window.open('" + pathConsuntivo.Replace("~", "") + "');");
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
            //string codiceLavoro = RichiediCodiceLavoro();

            //string nomeFile = "Consuntivo_" + codiceLavoro + ".pdf";
            //MemoryStream workStream = GeneraPdf();

            //Response.Clear();
            //Response.ClearContent();
            //Response.ClearHeaders();
            //Response.ContentType = "application/pdf";
            //Response.AddHeader("Content-Disposition", "attachment; filename=" + nomeFile);
            //Response.AddHeader("Content-Length", workStream.Length.ToString());
            //Response.BinaryWrite(workStream.ToArray());
            //Response.Flush();
            //Response.Close();
            //Response.End();
        }

        protected void btnModificaNote_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriModificaNote", script: "javascript: document.getElementById('panelModificaNote').style.display='block'", addScriptTags: true);
        }

        #endregion

        #region OPERAZIONI POPUP

        #endregion

    }
}