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

                //PdfDocument pdfDoc = new PdfDocument(pdfWriter);
                //pdfDoc.SetTagged();
                //Document document = new Document(pdfDoc);

                //IList<IElement> elements = HtmlConverter.ConvertToElements(htmlCompleto);

                
                //document.SetMargins(400, 250, 250, 400);
                //FontSet fontSet = new FontSet();
                
                //FontProvider fontProvider = new FontProvider(fontSet);
                
                //document.SetFontProvider(fontProvider);

                //foreach (IElement element in elements)
                //{
                //    document.Add((IBlockElement)element);
                //}


                var document = HtmlConverter.ConvertToDocument(htmlCompleto, pdfWriter);

                document.Close();
            }

            return workStream;
        }
    }
}