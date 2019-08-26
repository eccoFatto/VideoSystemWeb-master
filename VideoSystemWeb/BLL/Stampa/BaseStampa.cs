using iText;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Font;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Configuration;
namespace VideoSystemWeb.BLL.Stampa
{
    public class BaseStampa
    {
        //singleton
        private static volatile BaseStampa instance;
        private static object objForLock = new Object();

        private BaseStampa() { }

        public static BaseStampa Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new BaseStampa();
                    }
                }
                return instance;
            }
        }

        public MemoryStream GeneraPdf(string frammentoHtml)
        {
            string prefissoUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority;
            var htmlCompleto = "<html><head><link rel='stylesheet' type='text/css' href='" + prefissoUrl + "/Css/w3.css' /></head><body>" + frammentoHtml + "</body></html>";

            htmlCompleto = htmlCompleto.Replace("\r", "");
            htmlCompleto = htmlCompleto.Replace("\n", "");
            htmlCompleto = htmlCompleto.Replace("\t", "");

            var workStream = new MemoryStream();

            using (var pdfWriter = new PdfWriter(workStream))
            {
                pdfWriter.SetCloseStream(false);

                var document = HtmlConverter.ConvertToDocument(htmlCompleto, pdfWriter);

                document.Close();
            }

            return workStream;
        }
        public string AddPageNumber(string nomeFileIn,string nomeFileOut, ref Esito esito)
        {
            
            string ret = nomeFileOut;
            try
            {
                FileInfo file = new FileInfo(nomeFileOut);
                //file.Directory.Create();
                
                PdfDocument pdfDoc = new PdfDocument(new PdfReader(nomeFileIn), new PdfWriter(nomeFileOut));
                Document doc = new Document(pdfDoc);
                int n = pdfDoc.GetNumberOfPages();

               

                for (int i = 1; i <= n; i++)
                {
                    doc.ShowTextAligned(new Paragraph("pagina " + i.ToString() + " di " + n.ToString()).SetFontSize(7),
                            520, 815, i, iText.Layout.Properties.TextAlignment.CENTER, iText.Layout.Properties.VerticalAlignment.TOP, 0);
                }
                doc.Close();
                ret = ConfigurationManager.AppSettings["PATH_DOCUMENTI_PROTOCOLLO"] + Path.GetFileName(nomeFileOut);
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.Descrizione = "AddPageNumber: " + nomeFileIn + "," + nomeFileOut + Environment.NewLine + ex.Message;
                BasePage b = new BasePage();
                b.ShowError(esito.Descrizione);
                return ret;
            }
            
            return ret;
        }
    }
}