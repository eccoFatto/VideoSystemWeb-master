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
using System.Configuration;
using iText;
using iText.Kernel.Pdf;
using iText.Layout;
//using iText.Layout.Element;
using iTextSharp;

namespace VideoSystemWeb.Agenda.userControl
{
    public partial class RiepilogoOfferta : System.Web.UI.UserControl
    {
        BasePage basePage = new BasePage();
        public delegate void PopupHandler(string operazionePopup); // delegato per l'evento
        //public event PopupHandler RichiediOperazionePopup; //evento

        public delegate List<DatiArticoli> ListaArticoliHandler(); // delegato per l'evento
        public event ListaArticoliHandler RichiediListaArticoli; //evento

        public delegate string CodiceLavoroHandler(); // delegato per l'evento
        //public event CodiceLavoroHandler RichiediCodiceLavoro; //evento

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(this.btnStampaOfferta);
            }
            else
            {
                ComboMod_Pagamento.Items.Clear();
                foreach (GiorniPagamentoFatture gpf in SessionManager.ListaGPF)
                {
                    ComboMod_Pagamento.Items.Add(new ListItem(gpf.Giorni, gpf.Giorni));
                }

                foreach (DatiBancari datiBancari in SessionManager.ListaDatiBancari)
                {
                    ddl_Banca.Items.Add(new ListItem(datiBancari.Banca, datiBancari.DatiCompleti));
                }


            }
        }

        #region COMPORTAMENTO ELEMENTI PAGINA
        //protected void btnStampa_Click(object sender, EventArgs e)
        //{
            

        //    string codiceLavoro = RichiediCodiceLavoro();

        //    string nomeFile = "Offerta_" + codiceLavoro + ".pdf";
        //    MemoryStream workStream = GeneraPdf();

        //    Response.Clear();
        //    Response.ClearContent();
        //    Response.ClearHeaders();
        //    Response.ContentType = "application/pdf";
        //    Response.AddHeader("Content-Disposition", "attachment; filename=" + nomeFile);
        //    Response.AddHeader("Content-Length", workStream.Length.ToString());
        //    Response.BinaryWrite(workStream.ToArray());
        //    Response.Flush();
        //    Response.Close();
        //    Response.End();
        //}

        protected void btnModificaNote_Click(object sender, EventArgs e)
        {
            DivFramePdf.Visible = false;
            framePdf.Visible = false;

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriModificaNote", script: "javascript: document.getElementById('panelModificaNote').style.display='block'", addScriptTags: true);
        }

        protected void btnOKModificaNote_Click(object sender, EventArgs e)
        {
            
            NoteOfferta noteOfferta = (NoteOfferta)ViewState["NoteOfferta"];
            noteOfferta.Banca = ddl_Banca.SelectedValue;
            noteOfferta.Pagamento = 30; // int.Parse(tbMod_Pagamento.Text); //int.Parse(ComboMod_Pagamento.SelectedValue); 
            noteOfferta.NotaPagamento = tbMod_Pagamento.Text.Trim();
            noteOfferta.Consegna = txt_Consegna.Text;
            noteOfferta.Note = txt_Note.Text.Trim();
            Offerta_BLL.Instance.AggiornaNoteOfferta(noteOfferta);

            //RichiediOperazionePopup("SAVE_PDF_OFFERTA");



            //DivFramePdf.Visible = true;
            //framePdf.Visible = true;
            DatiAgenda eventoSel = (DatiAgenda)ViewState["eventoSelezionato"];
            Esito esito = popolaPannelloRiepilogo(eventoSel);

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "aggiornaNote", script: "javascript: aggiornaRiepilogo()", addScriptTags: true);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "chiudiModificaNote", script: "javascript: document.getElementById('panelModificaNote').style.display='none'", addScriptTags: true);
            // FACCIO REFRESH SUL FRAME CHE VISUALIZZA IL PDF IN MODO DA VEDERE GLI AGGIORNAMENTI IN TEMPO REALE
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "aggiornaFrame", script: "javascript: document.getElementById('" + framePdf.ClientID + "').contentDocument.location.reload(true);", addScriptTags: true);
        }

        #endregion

        #region OPERAZIONI POPUP

        public Esito popolaPannelloRiepilogo(DatiAgenda eventoSelezionato)
        {
            ViewState["eventoSelezionato"]= eventoSelezionato;
            Esito esito = new Esito();
            try
            {

                List<DatiArticoli> listaDatiArticoli = RichiediListaArticoli() == null? null : RichiediListaArticoli().Where(x => x.Stampa).ToList<DatiArticoli>();
                if (listaDatiArticoli != null)
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

                    //TROVO IL PROTOCOLLO, SE NON PRESENTE LO CREO
                    Protocolli protocolloOfferta = new Protocolli();
                    int idTipoProtocollo = UtilityTipologiche.getElementByNome(UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_PROTOCOLLO), "Offerta", ref esito).id;
                    List<Protocolli> listaProtocolli = Protocolli_BLL.Instance.getProtocolliByCodLavIdTipoProtocollo(eventoSelezionato.codice_lavoro, idTipoProtocollo, ref esito, true);
                    string numeroProtocollo = "";
                    if (listaProtocolli.Count == 0)
                    {
                        numeroProtocollo = Protocolli_BLL.Instance.getNumeroProtocollo();
                    }
                    else
                    {
                        bool trovato = false;
                        foreach (Protocolli protocollo in listaProtocolli)
                        {
                            if (protocollo.Destinatario == "Cliente")
                            {
                                //protocolloOfferta = listaProtocolli.First();
                                numeroProtocollo = protocollo.Numero_protocollo;
                                //numeroProtocollo = protocollo.Protocollo_riferimento;
                                protocolloOfferta = protocollo;
                                trovato = true;
                                break;
                            }
                        }
                        if (!trovato)
                        {
                            protocolloOfferta = listaProtocolli.First();
                            numeroProtocollo = protocolloOfferta.Numero_protocollo;
                        }
                    }

                    // GESTIONE NOMI FILE PDF
                    string nomeFile = "Offerta_" + eventoSelezionato.codice_lavoro + ".pdf";
                    string pathOfferta = ConfigurationManager.AppSettings["PATH_DOCUMENTI_PROTOCOLLO"] + nomeFile;
                    string mapPathOfferta = MapPath(ConfigurationManager.AppSettings["PATH_DOCUMENTI_PROTOCOLLO"]) + nomeFile;

                    // string prefissoUrl = Request.Url.Scheme + "://" + Request.Url.Authority;
                    //iText.IO.Image.ImageData imageData = iText.IO.Image.ImageDataFactory.Create(prefissoUrl + "/Images/logoVSP_trim.png");
                    iText.IO.Image.ImageData imageData = iText.IO.Image.ImageDataFactory.Create(MapPath("~/Images/logoVSP_trim.png"));
                    iText.IO.Image.ImageData imageDNV = iText.IO.Image.ImageDataFactory.Create(MapPath("~/Images/DNV_2008_ITA2.jpg"));


                    PdfWriter wr = new PdfWriter(mapPathOfferta);
                    PdfDocument doc = new PdfDocument(wr);
                    doc.SetDefaultPageSize(iText.Kernel.Geom.PageSize.A4);
                    //Document document = new Document(doc);
                    Document document = new Document(doc, iText.Kernel.Geom.PageSize.A4, false);

                    document.SetMargins(245, 30, 110, 30);


                    // ESTRAPOLO IL CLIENTE
                    Anag_Clienti_Fornitori cliente = Anag_Clienti_Fornitori_BLL.Instance.getAziendaById(eventoSelezionato.id_cliente, ref esito);

                    iText.Layout.Element.Paragraph pSpazio = new iText.Layout.Element.Paragraph(" ");
                    document.Add(pSpazio);


                    // CREAZIONE GRIGLIA PRINCIPALE DETTAGLIO OFFERTA
                    iText.Layout.Element.Table tbGrigla = new iText.Layout.Element.Table(new float[] { 80, 70, 180, 70, 30, 30, 70 }).SetWidth(530).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetFixedLayout();
                    iText.Layout.Element.Paragraph pGriglia;
                    iText.Layout.Element.Cell cellaGriglia;

                    // COLORE INTESTAZIONI
                    //iText.Kernel.Colors.Color coloreIntestazioni = new iText.Kernel.Colors.DeviceRgb(0, 255, 255);
                    // COLORE BLU VIDEOSYSTEM
                    iText.Kernel.Colors.Color coloreIntestazioni = new iText.Kernel.Colors.DeviceRgb(33, 150, 243);


                    // INTESTAZIONE OFFERTA
                    pGriglia = new iText.Layout.Element.Paragraph("Offerta Numero").SetFontSize(10).SetBold();
                    cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                    cellaGriglia.Add(pGriglia);
                    tbGrigla.AddHeaderCell(cellaGriglia);

                    pGriglia = new iText.Layout.Element.Paragraph(eventoSelezionato.codice_lavoro).SetFontSize(10).SetBold();
                    cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                    cellaGriglia.Add(pGriglia);
                    tbGrigla.AddHeaderCell(cellaGriglia);

                    pGriglia = new iText.Layout.Element.Paragraph("").SetFontSize(10);
                    cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                    cellaGriglia.Add(pGriglia);
                    tbGrigla.AddHeaderCell(cellaGriglia);

                    pGriglia = new iText.Layout.Element.Paragraph("Rif.Prot. " + numeroProtocollo).SetFontSize(10).SetBold();
                    cellaGriglia = new iText.Layout.Element.Cell(1, 4).SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                    cellaGriglia.Add(pGriglia);
                    tbGrigla.AddHeaderCell(cellaGriglia);

                    // INTESTAZIONE GRIGLIA
                    pGriglia = new iText.Layout.Element.Paragraph("Codice").SetFontSize(10);
                    cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetPadding(5).SetBold();
                    cellaGriglia.Add(pGriglia);
                    tbGrigla.AddHeaderCell(cellaGriglia);

                    pGriglia = new iText.Layout.Element.Paragraph("Descrizione Offerta").SetFontSize(10);
                    cellaGriglia = new iText.Layout.Element.Cell(1, 2).SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetBold();
                    cellaGriglia.Add(pGriglia);
                    tbGrigla.AddHeaderCell(cellaGriglia);

                    pGriglia = new iText.Layout.Element.Paragraph("Prezzo").SetFontSize(10);
                    cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetBold();
                    cellaGriglia.Add(pGriglia);
                    tbGrigla.AddHeaderCell(cellaGriglia);

                    pGriglia = new iText.Layout.Element.Paragraph("Qta").SetFontSize(10);
                    cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetBold();
                    cellaGriglia.Add(pGriglia);
                    tbGrigla.AddHeaderCell(cellaGriglia);

                    pGriglia = new iText.Layout.Element.Paragraph("Iva").SetFontSize(10);
                    cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetBold();
                    cellaGriglia.Add(pGriglia);
                    tbGrigla.AddHeaderCell(cellaGriglia);

                    pGriglia = new iText.Layout.Element.Paragraph("Totale").SetFontSize(10);
                    cellaGriglia = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 1, 100)).SetPadding(5).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetBold();
                    cellaGriglia.Add(pGriglia);
                    tbGrigla.AddHeaderCell(cellaGriglia);

                    decimal totPrezzo = 0;
                    decimal totIVA = 0;

                    // CICLO GLI ARTICOLI
                    foreach (DatiArticoli da in listaDatiArticoli)
                    {
                        // CALCOLO I TOTALI
                        totPrezzo += da.Prezzo * da.Quantita;
                        totIVA += (da.Prezzo * da.Iva / 100) * da.Quantita;

                        string descrizione = da.Descrizione;
                        string descrizioneLunga = da.DescrizioneLunga;

                        pGriglia = new iText.Layout.Element.Paragraph(descrizione).SetFontSize(9);
                        cellaGriglia = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10);
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        pGriglia = new iText.Layout.Element.Paragraph(descrizioneLunga).SetFontSize(9);
                        cellaGriglia = new iText.Layout.Element.Cell(1, 2).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10);
                        cellaGriglia.Add(pGriglia);
                        tbGrigla.AddCell(cellaGriglia);

                        if (!cbCopriImporti.Checked) { 
                            pGriglia = new iText.Layout.Element.Paragraph(da.Prezzo.ToString("###,##0.00")).SetFontSize(9);
                            cellaGriglia = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                            cellaGriglia.Add(pGriglia);
                            tbGrigla.AddCell(cellaGriglia);

                            pGriglia = new iText.Layout.Element.Paragraph(da.Quantita.ToString("##0")).SetFontSize(9);
                            //pGriglia = new iText.Layout.Element.Paragraph(1.ToString("##0")).SetFontSize(9);
                            cellaGriglia = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                            cellaGriglia.Add(pGriglia);
                            tbGrigla.AddCell(cellaGriglia);

                            pGriglia = new iText.Layout.Element.Paragraph(da.Iva.ToString("##")).SetFontSize(9);
                            cellaGriglia = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                            cellaGriglia.Add(pGriglia);
                            tbGrigla.AddCell(cellaGriglia);

                            decimal totale = da.Prezzo * da.Quantita;
                            //decimal totale = da.Prezzo * 1;

                            pGriglia = new iText.Layout.Element.Paragraph(totale.ToString("###,##0.00")).SetFontSize(9);
                            cellaGriglia = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                            cellaGriglia.Add(pGriglia);
                            tbGrigla.AddCell(cellaGriglia);
                        }
                        else
                        {
                            // NON VISUALIZZO IMPORTI
                            pGriglia = new iText.Layout.Element.Paragraph("").SetFontSize(9);
                            cellaGriglia = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                            cellaGriglia.Add(pGriglia);
                            tbGrigla.AddCell(cellaGriglia);

                            pGriglia = new iText.Layout.Element.Paragraph(da.Quantita.ToString("##0")).SetFontSize(9);
                            //pGriglia = new iText.Layout.Element.Paragraph(1.ToString("##0")).SetFontSize(9);
                            cellaGriglia = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                            cellaGriglia.Add(pGriglia);
                            tbGrigla.AddCell(cellaGriglia);

                            pGriglia = new iText.Layout.Element.Paragraph("").SetFontSize(9);
                            cellaGriglia = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                            cellaGriglia.Add(pGriglia);
                            tbGrigla.AddCell(cellaGriglia);

                            decimal totale = da.Prezzo * da.Quantita;
                            //decimal totale = da.Prezzo * 1;

                            pGriglia = new iText.Layout.Element.Paragraph("").SetFontSize(9);
                            cellaGriglia = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                            cellaGriglia.Add(pGriglia);
                            tbGrigla.AddCell(cellaGriglia);
                        }
                    }

                    // AGGIUNGO UNO SPAZIO
                    pGriglia = new iText.Layout.Element.Paragraph(" ").SetFontSize(9);
                    cellaGriglia = new iText.Layout.Element.Cell(1, 7).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT);
                    cellaGriglia.Add(pGriglia);
                    tbGrigla.AddCell(cellaGriglia);


                    // ESTRAPOLO NOTEOFFERTA

                    NoteOfferta noteOfferta = Offerta_BLL.Instance.getNoteOffertaByIdDatiAgenda(eventoSelezionato.id, ref esito);

                    // se non viene trovata una notaOfferta (vecchi eventi) viene creata e salvata
                    if (noteOfferta.Id == 0)
                    {
                        List<DatiBancari> datiBancari = Config_BLL.Instance.getListaDatiBancari(ref esito);
                        noteOfferta = new NoteOfferta { Id_dati_agenda = eventoSelezionato.id, Banca = datiBancari[0].DatiCompleti, Pagamento = cliente.Pagamento, NotaPagamento = cliente.NotaPagamento, Consegna = cliente.TipoIndirizzoLegale + " " + cliente.IndirizzoLegale + " " + cliente.NumeroCivicoLegale + Environment.NewLine + cliente.CapLegale + " " + cliente.ComuneLegale + " " + cliente.ProvinciaLegale + " " };// "Unicredit Banca: IBAN: IT39H0200805198000103515620", Pagamento = cliente.Pagamento, Consegna = cliente.TipoIndirizzoLegale + " " + cliente.IndirizzoLegale + " " + cliente.NumeroCivicoLegale + " " + cliente.CapLegale + " " + cliente.ProvinciaLegale + " " };

                        Offerta_BLL.Instance.CreaNoteOfferta(noteOfferta, ref esito);
                    }

                    ViewState["NoteOfferta"] = noteOfferta;

                    txt_Consegna.Text = noteOfferta.Consegna;
                    tbMod_Pagamento.Text = noteOfferta.NotaPagamento.ToString();

                    if (string.IsNullOrEmpty(noteOfferta.Note))
                    {
                        txt_Note.Text = "";
                    }
                    else
                    {
                        txt_Note.Text = noteOfferta.Note.Trim();
                    }

                    // NOTE
                    iText.Layout.Element.Text first = new iText.Layout.Element.Text("Note:").SetFontSize(9).SetBold();
                    iText.Layout.Element.Text second = new iText.Layout.Element.Text(Environment.NewLine + noteOfferta.Note.Trim()).SetFontSize(9);
                    iText.Layout.Element.Paragraph paragraphNote = new iText.Layout.Element.Paragraph().Add(first).Add(second);

                    cellaGriglia = new iText.Layout.Element.Cell(3, 3).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10);
                    cellaGriglia.Add(paragraphNote);
                    tbGrigla.AddCell(cellaGriglia);

                    pGriglia = new iText.Layout.Element.Paragraph("Imponibile").SetFontSize(9);
                    cellaGriglia = new iText.Layout.Element.Cell(1, 3).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetBold();
                    cellaGriglia.Add(pGriglia);
                    tbGrigla.AddCell(cellaGriglia);

                    if (!cbCopriImporti.Checked)
                    {
                        pGriglia = new iText.Layout.Element.Paragraph(totPrezzo.ToString("###,##0.00")).SetFontSize(9);
                    }
                    else
                    {
                        pGriglia = new iText.Layout.Element.Paragraph("").SetFontSize(9);
                    }
                    cellaGriglia = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetBold();
                    cellaGriglia.Add(pGriglia);
                    tbGrigla.AddCell(cellaGriglia);

                    // TOTALE IVA
                    pGriglia = new iText.Layout.Element.Paragraph("i.v.a.").SetFontSize(9);
                    cellaGriglia = new iText.Layout.Element.Cell(1, 3).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetBold();
                    cellaGriglia.Add(pGriglia);
                    tbGrigla.AddCell(cellaGriglia);
                    if (!cbCopriImporti.Checked)
                    {
                        pGriglia = new iText.Layout.Element.Paragraph(totIVA.ToString("###,##0.00")).SetFontSize(9);
                    }
                    else {
                        pGriglia = new iText.Layout.Element.Paragraph("").SetFontSize(9);
                    }
                    cellaGriglia = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetBold();
                    cellaGriglia.Add(pGriglia);
                    tbGrigla.AddCell(cellaGriglia);

                    // TOTALE EURO
                    pGriglia = new iText.Layout.Element.Paragraph("Totale Euro").SetFontSize(9);
                    cellaGriglia = new iText.Layout.Element.Cell(1, 3).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetBold();
                    cellaGriglia.Add(pGriglia);
                    tbGrigla.AddCell(cellaGriglia);
                    if (!cbCopriImporti.Checked)
                    {
                        pGriglia = new iText.Layout.Element.Paragraph((totPrezzo + totIVA).ToString("###,##0.00")).SetFontSize(9);
                    }
                    else
                    {
                        pGriglia = new iText.Layout.Element.Paragraph("").SetFontSize(9);
                    }
                    cellaGriglia = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE, 10).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetBold();
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
                        iText.Layout.Element.Table tbGriglaInfo = new iText.Layout.Element.Table(new float[] { 70, 230 }).SetWidth(300).SetFixedPosition(i, 30, 640, 300);
                        iText.Layout.Element.Paragraph pGrigliaInfo = new iText.Layout.Element.Paragraph(cittaVs).SetFontSize(9).SetBold();
                        iText.Layout.Element.Cell cellaGrigliaInfo = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGrigliaInfo.Add(pGrigliaInfo);
                        tbGriglaInfo.AddCell(cellaGrigliaInfo);

                        pGrigliaInfo = new iText.Layout.Element.Paragraph(DateTime.Today.ToLongDateString()).SetFontSize(9);
                        cellaGrigliaInfo = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGrigliaInfo.Add(pGrigliaInfo);
                        tbGriglaInfo.AddCell(cellaGrigliaInfo);

                        pGrigliaInfo = new iText.Layout.Element.Paragraph("Produzione:").SetFontSize(9).SetBold();
                        cellaGrigliaInfo = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGrigliaInfo.Add(pGrigliaInfo);
                        tbGriglaInfo.AddCell(cellaGrigliaInfo);

                        pGrigliaInfo = new iText.Layout.Element.Paragraph(eventoSelezionato.produzione).SetFontSize(9);
                        cellaGrigliaInfo = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGrigliaInfo.Add(pGrigliaInfo);
                        tbGriglaInfo.AddCell(cellaGrigliaInfo);

                        pGrigliaInfo = new iText.Layout.Element.Paragraph("Lavorazione:").SetFontSize(9).SetBold();
                        cellaGrigliaInfo = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGrigliaInfo.Add(pGrigliaInfo);
                        tbGriglaInfo.AddCell(cellaGrigliaInfo);

                        pGrigliaInfo = new iText.Layout.Element.Paragraph(eventoSelezionato.lavorazione).SetFontSize(9);
                        cellaGrigliaInfo = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGrigliaInfo.Add(pGrigliaInfo);
                        tbGriglaInfo.AddCell(cellaGrigliaInfo);

                        pGrigliaInfo = new iText.Layout.Element.Paragraph("Data Lav.ne:").SetFontSize(9).SetBold();
                        cellaGrigliaInfo = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGrigliaInfo.Add(pGrigliaInfo);
                        tbGriglaInfo.AddCell(cellaGrigliaInfo);

                        pGrigliaInfo = new iText.Layout.Element.Paragraph(eventoSelezionato.data_inizio_lavorazione.ToString("dd/MM/yyyy")).SetFontSize(9);
                        cellaGrigliaInfo = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGrigliaInfo.Add(pGrigliaInfo);
                        tbGriglaInfo.AddCell(cellaGrigliaInfo);

                        document.Add(tbGriglaInfo);


                        // CREAZIONE GRIGLIA DESTINATARIO
                        iText.Layout.Element.Table tbGriglaDest = new iText.Layout.Element.Table(new float[] { 70, 230 }).SetWidth(300).SetFixedPosition(i, 350, 650, 300);
                        iText.Layout.Element.Paragraph pGrigliaDest = new iText.Layout.Element.Paragraph("Spettabile").SetFontSize(9).SetBold();
                        iText.Layout.Element.Cell cellaGrigliaDest = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGrigliaDest.Add(pGrigliaDest);
                        tbGriglaDest.AddCell(cellaGrigliaDest);

                        pGrigliaDest = new iText.Layout.Element.Paragraph(cliente.RagioneSociale).SetFontSize(9);
                        cellaGrigliaDest = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGrigliaDest.Add(pGrigliaDest);
                        tbGriglaDest.AddCell(cellaGrigliaDest);

                        // INDIRIZZO DESTINATARIO
                        pGrigliaDest = new iText.Layout.Element.Paragraph("Indirizzo").SetFontSize(9).SetBold();
                        cellaGrigliaDest = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGrigliaDest.Add(pGrigliaDest);
                        tbGriglaDest.AddCell(cellaGrigliaDest);

                        //pGrigliaDest = new iText.Layout.Element.Paragraph(cliente.TipoIndirizzoOperativo + " " + cliente.IndirizzoOperativo + " " + cliente.NumeroCivicoOperativo + Environment.NewLine + cliente.CapOperativo + " " + cliente.ComuneOperativo + " " + cliente.ProvinciaOperativo).SetFontSize(9);
                        //cellaGrigliaDest = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        //cellaGrigliaDest.Add(pGrigliaDest);
                        //tbGriglaDest.AddCell(cellaGrigliaDest);

                        pGrigliaDest = new iText.Layout.Element.Paragraph(cliente.TipoIndirizzoLegale + " " + cliente.IndirizzoLegale + " " + cliente.NumeroCivicoLegale + Environment.NewLine + cliente.CapLegale + " " + cliente.ComuneLegale + " " + cliente.ProvinciaLegale).SetFontSize(9);
                        cellaGrigliaDest = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGrigliaDest.Add(pGrigliaDest);
                        tbGriglaDest.AddCell(cellaGrigliaDest);

                        // PARTITA IVA DESTINATARIO
                        pGrigliaDest = new iText.Layout.Element.Paragraph("P.Iva/C.F.").SetFontSize(9).SetBold();
                        cellaGrigliaDest = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGrigliaDest.Add(pGrigliaDest);
                        tbGriglaDest.AddCell(cellaGrigliaDest);

                        string pIvaCF = cliente.PartitaIva;
                        if (string.IsNullOrEmpty(pIvaCF)) pIvaCF = cliente.CodiceFiscale;

                        pGrigliaDest = new iText.Layout.Element.Paragraph(pIvaCF).SetFontSize(9);
                        cellaGrigliaDest = new iText.Layout.Element.Cell().SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                        cellaGrigliaDest.Add(pGrigliaDest);
                        tbGriglaDest.AddCell(cellaGrigliaDest);

                        document.Add(tbGriglaDest);

                        // AGGIUNGO LOGO DNV
                        iText.Layout.Element.Image logoDnv = new iText.Layout.Element.Image(imageDNV).ScaleAbsolute(40, 40).SetFixedPosition(i, 518, 8);
                        document.Add(logoDnv);

                        //AGGIUNGO NUM.PAGINA
                        document.ShowTextAligned(new iText.Layout.Element.Paragraph("pagina " + i.ToString() + " di " + n.ToString()).SetFontSize(7),
                            pageSize.GetWidth() - 60, pageSize.GetHeight() - 20, i, iText.Layout.Properties.TextAlignment.CENTER, iText.Layout.Properties.VerticalAlignment.TOP, 0);
                        //AGGIUNGO FOOTER
                        document.ShowTextAligned(new iText.Layout.Element.Paragraph(denominazioneVs + " P.IVA " + pIvaVs + Environment.NewLine + "Sede legale: " + toponimoVs + " " + indirizzoVs + " " + civicoVs + " - " + capVs + " " + cittaVs + " " + provinciaVs + " e-mail: " + emailVs).SetFontSize(7).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER),
                                                        pageSize.GetWidth() / 2, 30, i, iText.Layout.Properties.TextAlignment.CENTER, iText.Layout.Properties.VerticalAlignment.TOP, 0);

                        if (i == n)
                        {
                            // NELL'ULTIMA PAGINA AGGIUNGO LA GRIGLIA CON LE NOTE E IL TIMBRO
                            // CREAZIONE GRIGLIA
                            iText.Layout.Element.Table tbGriglaNoteFooter = new iText.Layout.Element.Table(new float[] { 80, 70, 180, 70, 30, 30, 70 }).SetWidth(530).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetFixedPosition(30, 50, 530).SetFixedLayout();

                            // PRIMA RIGA GRIGLIA NOTE FOOTER
                            iText.Layout.Element.Paragraph pGrigliaNoteFooter = new iText.Layout.Element.Paragraph("Banca").SetFontSize(9);
                            iText.Layout.Element.Cell cellaGrigliaNoteFooter = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                            cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                            tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                            pGrigliaNoteFooter = new iText.Layout.Element.Paragraph(noteOfferta.Banca).SetFontSize(9);
                            //cellaGrigliaNoteFooter = new iText.Layout.Element.Cell(1,2).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY, 10).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(2);
                            cellaGrigliaNoteFooter = new iText.Layout.Element.Cell(1, 2).SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorderRight(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 2, 50)).SetBorderTop(iText.Layout.Borders.Border.NO_BORDER).SetBorderLeft(iText.Layout.Borders.Border.NO_BORDER).SetBorderBottom(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);

                            cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                            tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                            pGrigliaNoteFooter = new iText.Layout.Element.Paragraph("Timbro e firma per accettazione").SetFontSize(9);
                            cellaGrigliaNoteFooter = new iText.Layout.Element.Cell(1, 4).SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                            cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                            tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                            // SECONDA RIGA GRIGLIA NOTE FOOTER
                            pGrigliaNoteFooter = new iText.Layout.Element.Paragraph("Pagamento").SetFontSize(9);
                            cellaGrigliaNoteFooter = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                            cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                            tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                            pGrigliaNoteFooter = new iText.Layout.Element.Paragraph(noteOfferta.NotaPagamento).SetFontSize(9);
                            cellaGrigliaNoteFooter = new iText.Layout.Element.Cell(1, 2).SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorderRight(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 2, 50)).SetBorderTop(iText.Layout.Borders.Border.NO_BORDER).SetBorderLeft(iText.Layout.Borders.Border.NO_BORDER).SetBorderBottom(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                            cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                            tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                            pGrigliaNoteFooter = new iText.Layout.Element.Paragraph(" ").SetFontSize(9);
                            cellaGrigliaNoteFooter = new iText.Layout.Element.Cell(1, 4).SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                            cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                            tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                            // TERZA RIGA GRIGLIA NOTE FOOTER
                            pGrigliaNoteFooter = new iText.Layout.Element.Paragraph("Consegna").SetFontSize(9);
                            cellaGrigliaNoteFooter = new iText.Layout.Element.Cell().SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                            cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                            tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                            pGrigliaNoteFooter = new iText.Layout.Element.Paragraph(noteOfferta.Consegna.Replace("\r\n", " ")).SetFontSize(9);
                            cellaGrigliaNoteFooter = new iText.Layout.Element.Cell(1, 2).SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorderRight(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.WHITE, 2, 50)).SetBorderTop(iText.Layout.Borders.Border.NO_BORDER).SetBorderLeft(iText.Layout.Borders.Border.NO_BORDER).SetBorderBottom(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                            cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                            tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                            pGrigliaNoteFooter = new iText.Layout.Element.Paragraph(" ").SetFontSize(9);
                            cellaGrigliaNoteFooter = new iText.Layout.Element.Cell(1, 4).SetBackgroundColor(coloreIntestazioni, 0.7f).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetPadding(5);
                            cellaGrigliaNoteFooter.Add(pGrigliaNoteFooter);
                            tbGriglaNoteFooter.AddCell(cellaGrigliaNoteFooter);

                            document.Add(tbGriglaNoteFooter);
                        }
                    }

                    document.Close();
                    wr.Close();

                    if (File.Exists(mapPathOfferta))
                    {

                        // SE FILE OK INSERISCO O AGGIORNO PROTOCOLLO DI TIPO PIANO ESTERNO
                        if (listaProtocolli.Count == 0)
                        {
                            //INSERISCO
                            protocolloOfferta.Attivo = true;
                            protocolloOfferta.Cliente = cliente.RagioneSociale.Trim();
                            protocolloOfferta.Codice_lavoro = eventoSelezionato.codice_lavoro;
                            protocolloOfferta.Data_inizio_lavorazione = eventoSelezionato.data_inizio_impegno;
                            protocolloOfferta.Data_protocollo = DateTime.Today;
                            protocolloOfferta.Descrizione = "Offerta";
                            protocolloOfferta.Id_cliente = eventoSelezionato.id_cliente;
                            protocolloOfferta.Id_tipo_protocollo = idTipoProtocollo;
                            protocolloOfferta.Lavorazione = eventoSelezionato.lavorazione;
                            protocolloOfferta.PathDocumento = Path.GetFileName(mapPathOfferta);
                            protocolloOfferta.Produzione = eventoSelezionato.produzione;
                            protocolloOfferta.Protocollo_riferimento = "";
                            protocolloOfferta.Numero_protocollo = numeroProtocollo;
                            protocolloOfferta.Pregresso = false;
                            protocolloOfferta.Destinatario = "Cliente";

                            int idProtPianoEsterno = Protocolli_BLL.Instance.CreaProtocollo(protocolloOfferta, ref esito);
                        }
                        else
                        {
                            // AGGIORNO
                            protocolloOfferta.PathDocumento = Path.GetFileName(mapPathOfferta);
                            esito = Protocolli_BLL.Instance.AggiornaProtocollo(protocolloOfferta);
                        }

                        framePdf.Attributes.Remove("src");
                        framePdf.Attributes.Add("src", pathOfferta.Replace("~", ""));

                        DivFramePdf.Visible = true;
                        framePdf.Visible = true;

                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "aggiornaFrame", script: "javascript: document.getElementById('" + framePdf.ClientID + "').contentDocument.location.reload(true);", addScriptTags: true);
                        btnStampaOfferta.Attributes.Add("onclick", "window.open('" + pathOfferta.Replace("~", "") + "');");
                        //}
                    }
                    else
                    {
                        esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                        esito.Descrizione = "Il File " + pathOfferta.Replace("~", "") + " non è stato creato correttamente!";
                    }
                }
            }
            catch (Exception ex)
            {

                esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.Descrizione = "popolaPannelloRiepilogo(DatiAgenda eventoSelezionato) " + ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return esito;
        }

        //public MemoryStream GeneraPdf()
        //{
        //    string prefissoUrl = Request.Url.Scheme + "://" + Request.Url.Authority;
        //    //imgLogo.ImageUrl = prefissoUrl + "/Images/logoVSP_trim.png";
        //    //imgDNV.ImageUrl = prefissoUrl + "/Images/DNV_2008_ITA2.jpg";

        //    //AbilitaVisualizzazioneStampa(true);


        //    StringWriter sw = new StringWriter();
        //    HtmlTextWriter hw = new HtmlTextWriter(sw);

        //    try
        //    {
        //        //modalRiepilogoContent.RenderControl(hw);
        //    }
        //    catch (Exception ex)
        //    {
        //        basePage.ShowError(ex.Message);


        //    }


        //    MemoryStream workStream = BaseStampa.Instance.GeneraPdf(sw.ToString());

            
        //    sw.Flush();
        //    hw.Flush();

        //    return workStream;
        //}

        private void AbilitaVisualizzazioneStampa(bool isVisualizzazioneStampa)
        {

        }
        #endregion

        protected void cbCopriImporti_CheckedChanged(object sender, EventArgs e)
        {
            DatiAgenda eventoSel = (DatiAgenda)ViewState["eventoSelezionato"];
            Esito esito = popolaPannelloRiepilogo(eventoSel);

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "aggiornaNote", script: "javascript: aggiornaRiepilogo()", addScriptTags: true);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "chiudiModificaNote", script: "javascript: document.getElementById('panelModificaNote').style.display='none'", addScriptTags: true);
            // FACCIO REFRESH SUL FRAME CHE VISUALIZZA IL PDF IN MODO DA VEDERE GLI AGGIORNAMENTI IN TEMPO REALE
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "aggiornaFrame", script: "javascript: document.getElementById('" + framePdf.ClientID + "').contentDocument.location.reload(true);", addScriptTags: true);

        }
    }
}