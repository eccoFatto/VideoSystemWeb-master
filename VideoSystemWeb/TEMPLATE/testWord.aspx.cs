using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Office.Interop.Word;
using System.Configuration;
using System.IO;
namespace VideoSystemWeb.TEMPLATE
{
    public partial class testWord : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAggiornaModello_Click(object sender, EventArgs e)
        {
            lbBookmarks.Items.Clear();
            string nomeModello = Server.MapPath(ConfigurationManager.AppSettings["PATH_TEMPLATE"]) + "Modello Prova.dotx";
            Microsoft.Office.Interop.Word.Application oWord = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document oDoc = new Microsoft.Office.Interop.Word.Document();
            //// CREO L'OGGETTO WORD
            //oWord = create CreateObject("Word.Application")
            //// APRO IL MODELLO
            oDoc = oWord.Documents.Add(nomeModello);


            foreach (Bookmark bookmark in oDoc.Bookmarks)
            {
                lbBookmarks.Items.Add(bookmark.Name);
                oDoc.Bookmarks[bookmark.Name].Range.Text = tbParametro1.Text;
            }
            ////oDoc = oWord.Documents.Add("D:\PROGETTI VISUAL STUDIO\GestPei\MODELLI\Modello Pei.dot")
            //oDoc = oWord.Documents.Add(nomeModello)

            //'AGGIUNGO I PARAMETRI AL MODELLO
            
            //oDoc.Bookmarks.Item("cognomeStudente").Range.Text = stud.cognomeStudente
            //oDoc.Bookmarks.Item("cittaStudente").Range.Text = stud.cittaNataleStudente
            //oDoc.Bookmarks.Item("dataNascitaStudente").Range.Text = stud.dataNascitaStudente.ToShortDateString
            //oDoc.Bookmarks.Item("indirizzoStudente").Range.Text = stud.indirizzoStudente + ", " + stud.capStudente + " - " + stud.cittaResidenzaStudente
            //oDoc.Bookmarks.Item("annoScolasticoIscrizione").Range.Text = annoSc

            //'VISUALIZZO IL DOCUMENTO
            oWord.Visible = true;
            oWord.ShowMe();

        }
    }
}