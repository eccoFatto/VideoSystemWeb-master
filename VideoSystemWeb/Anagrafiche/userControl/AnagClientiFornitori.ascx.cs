using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VideoSystemWeb.Anagrafiche.userControl
{
    public partial class AnagClientiFornitori : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                // FUNZIONA SE NELLA PAGINA ASPX CHIAMANTE C'E' UN CAMPO HIDDENFIELD COL TIPO AZIENDA (CLIENTI/FORNITORI)
                HiddenField tipoAzienda = this.Parent.FindControl("HF_TIPO_AZIENDA") as HiddenField;
                if (tipoAzienda != null) { 
                    ViewState["TIPO_AZIENDA"] = tipoAzienda.Value;
                }
                else
                {
                    ViewState["TIPO_AZIENDA"] = "CLIENTI";
                }
                lblTipoAzienda.Text = ViewState["TIPO_AZIENDA"].ToString();
            }
        }

        
        protected void btnInserisciAzienda_Click(object sender, EventArgs e)
        {

        }

        protected void btnRicercaAziende_Click(object sender, EventArgs e)
        {

        }

        protected void gv_aziende_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void ddlTipoAzienda_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}