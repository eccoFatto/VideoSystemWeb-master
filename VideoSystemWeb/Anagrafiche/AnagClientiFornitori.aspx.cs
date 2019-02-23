using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoSystemWeb.BLL;

namespace VideoSystemWeb.Anagrafiche
{
    public partial class AnagClientiFornitori : BasePage
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            CheckIsMobile();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string tipo = "CLIENTI";
            if (!string.IsNullOrEmpty(Request.QueryString["TIPO"]))
            {
                tipo = Request.QueryString["TIPO"];
            }
            HF_TIPO_AZIENDA.Value = tipo;
            
        }
    }
}