using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoSystemWeb.BLL;

namespace VideoSystemWeb.Articoli
{
    public partial class GestioneArticoli : BasePage
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            CheckIsMobile();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            // ARTICOLI
            // GENERI
            // GRUPPI
            // SOTTOGRUPPI
            string tipo = "ARTICOLI";
            if (!string.IsNullOrEmpty(Request.QueryString["TIPO"]))
            {
                tipo = Request.QueryString["TIPO"];
            }
            HF_TIPO_ARTICOLO.Value = tipo;
            Control loadControl = new Control();
            switch (tipo)
            {
                case "ARTICOLI":
                    loadControl = this.LoadControl("~/Articoli/userControl/ArtArticoli.ascx");
                    PH.Controls.Add(loadControl);
                    break;
                default:
                    loadControl = this.LoadControl("~/Articoli/userControl/ArtTipologie.ascx");
                    PH.Controls.Add(loadControl);
                    break;
            }
        }
    }
}