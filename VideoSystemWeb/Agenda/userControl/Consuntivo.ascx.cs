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

            }
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

        protected void btnOKModificaNote_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(Page, typeof(Page), "aggiornaNote", script: "javascript: aggiornaRiepilogo()", addScriptTags: true);
            //ScriptManager.RegisterStartupScript(Page, typeof(Page), "chiudiModificaNote", script: "javascript: document.getElementById('panelModificaNote').style.display='none'", addScriptTags: true);
        }

        #endregion

        #region OPERAZIONI POPUP

        #endregion

    }
}