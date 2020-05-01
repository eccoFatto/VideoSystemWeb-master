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
using VideoSystemWeb.DAL;
using iText;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iTextSharp;
using System.Configuration;
using System.Data;

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
                string nomeFile = "RiepilogoGiornata.pdf";
                string pathGiornata = ConfigurationManager.AppSettings["PATH_DOCUMENTI_PROTOCOLLO"] + nomeFile;
                string mapPathGiornata = MapPath(ConfigurationManager.AppSettings["PATH_DOCUMENTI_PROTOCOLLO"]) + nomeFile;

                iText.IO.Image.ImageData imageData = iText.IO.Image.ImageDataFactory.Create(MapPath("~/Images/logoVSP_trim.png"));
                iText.IO.Image.ImageData imageDNV = iText.IO.Image.ImageDataFactory.Create(MapPath("~/Images/DNV_2008_ITA2.jpg"));


                PdfWriter wr = new PdfWriter(mapPathGiornata);
                PdfDocument doc = new PdfDocument(wr);
                doc.SetDefaultPageSize(iText.Kernel.Geom.PageSize.A4.Rotate());
                //Document document = new Document(doc);
                Document document = new Document(doc, iText.Kernel.Geom.PageSize.A4.Rotate(), false);

                // IMPOSTO LA DATA DI OGGI SE PRIMA ELABORAZIONE
                if (string.IsNullOrEmpty(tbDataElaborazione.Text.Trim())) tbDataElaborazione.Text = DateTime.Today.ToShortDateString();

                document.SetMargins(90, 35, 50, 35);

                Paragraph pSpazio = new Paragraph("");
                document.Add(pSpazio);

                // COLORE BLU VIDEOSYSTEM
                iText.Kernel.Colors.Color coloreIntestazioni = new iText.Kernel.Colors.DeviceRgb(33, 150, 243);


                DateTime dataTmp = Convert.ToDateTime(tbDataElaborazione.Text.Trim());
                string sDataTmp = dataTmp.ToString("yyyy-MM-dd") + "T00:00:00.000";

                string queryRicerca = "select da.data_inizio_lavorazione as [Data Inizio] , da.data_fine_lavorazione as [Data Fine], da.durata_lavorazione as Durata " +
                ",produzione, lavorazione, codice_lavoro,ca.nome,ts.nome,tt.nome, " +
                "ac.cognome as cognome_operatore,ac.nome as nome_operatore, dal.descrizione as qualifica, dal.descrizioneLunga " +
                "from[dbo].[tab_dati_agenda] da " +
                "left join tipo_colonne_agenda ca " +
                "on da.id_colonne_agenda = ca.id " +
                "left join tipo_stato ts " +
                "on da.id_stato = ts.id " +
                "left join tipo_tipologie tt " +
                "on da.id_tipologia = tt.id " +
                "left join dati_lavorazione dl " +
                "on dl.idDatiAgenda = da.id " +
                "left join dati_articoli_lavorazione dal " +
                "on dl.id = dal.idDatiLavorazione " +
                "left join anag_collaboratori ac " +
                "on dal.idCollaboratori = ac.id " +
                "where ac.cognome is not null and dal.descrizione<> 'Diaria' " +
                "and data_inizio_lavorazione <= '@dataElaborazione' and data_fine_lavorazione >= '@dataElaborazione' " +
                "order by codice_lavoro, dal.descrizione, ac.cognome, ac.nome";

                queryRicerca = queryRicerca.Replace("@dataElaborazione", sDataTmp);

                esito = new Esito();
                DataTable dtProtocolli = Base_DAL.GetDatiBySql(queryRicerca, ref esito);
                if (dtProtocolli!=null && dtProtocolli.Rows!=null && dtProtocolli.Rows.Count > 0)
                {
                    foreach (DataRow riga in dtProtocolli.Rows)
                    {
                        Paragraph pRiga = new Paragraph(riga["produzione"].ToString() + " " + riga["lavorazione"].ToString() + " " + riga["qualifica"].ToString() + " " + riga["cognome_operatore"].ToString() + " " + riga["nome_operatore"].ToString());
                        document.Add(pRiga);
                    }
                }

                int n = doc.GetNumberOfPages();
                iText.Kernel.Geom.Rectangle pageSize = doc.GetPage(n).GetPageSize();

                // AGGIUNGO CONTEGGIO PAGINE E FOOTER PER OGNI PAGINA
                for (int i = 1; i <= n; i++)
                {
                    // AGGIUNGO LOGO
                    //iText.Layout.Element.Image image = new iText.Layout.Element.Image(imageData).ScaleAbsolute(90, 80).SetFixedPosition(i, 30, 500);
                    //document.Add(image);
                    // AGGIUNGO LOGO
                    iText.Layout.Element.Image image = new iText.Layout.Element.Image(imageData).ScaleAbsolute(60, 60).SetFixedPosition(i, 20, pageSize.GetHeight() - 80);
                    document.Add(image);

                    // AGGIUNGO LOGO DNV
                    iText.Layout.Element.Image logoDnv = new iText.Layout.Element.Image(imageDNV).ScaleAbsolute(40, 40).SetFixedPosition(i, 780, 8);
                    document.Add(logoDnv);

                    //AGGIUNGO NUM.PAGINA
                    document.ShowTextAligned(new Paragraph("pagina " + i.ToString() + " di " + n.ToString()).SetFontSize(7), pageSize.GetWidth() - 60, pageSize.GetHeight() - 20, i, iText.Layout.Properties.TextAlignment.CENTER, iText.Layout.Properties.VerticalAlignment.TOP, 0);

                    // AGGIUNGO TITOLO
                    document.ShowTextAligned(new Paragraph("OPERATORI IMPEGNATI PER IL " + tbDataElaborazione.Text.Trim()).SetFontSize(11), 400, pageSize.GetHeight() - 30, i, iText.Layout.Properties.TextAlignment.CENTER, iText.Layout.Properties.VerticalAlignment.TOP, 0);

                    //AGGIUNGO FOOTER
                    document.ShowTextAligned(new Paragraph(denominazioneVs + " P.IVA " + pIvaVs + Environment.NewLine + "Sede legale: " + toponimoVs + " " + indirizzoVs + " " + civicoVs + " - " + capVs + " " + cittaVs + " " + provinciaVs + " e-mail: " + emailVs ).SetFontSize(7).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER),
                                                    pageSize.GetWidth()/2, 30, i, iText.Layout.Properties.TextAlignment.CENTER, iText.Layout.Properties.VerticalAlignment.TOP, 0);

                    if (i == n)
                    {
                        // ULTIMA PAGINA
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

        #endregion

        #region OPERAZIONI POPUP

        #endregion

        protected void btnCreaGiornata_Click(object sender, EventArgs e)
        {
            Esito esito = popolaPannelloGiornata();

            if (esito.Codice == Esito.ESITO_OK)
            {
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "aggiornaFrame", script: "javascript: document.getElementById('" + framePdfGiornata.ClientID + "').contentDocument.location.reload(true);", addScriptTags: true);
            }
        }
    }

}