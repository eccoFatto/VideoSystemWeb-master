using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VideoSystemWeb.Anagrafiche
{
    public partial class AnagClientiFornitori : System.Web.UI.Page
    {
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