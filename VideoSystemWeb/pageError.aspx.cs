using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoSystemWeb.BLL;
namespace VideoSystemWeb
{
    public partial class pageError : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //string messaggio = Request.QueryString["messaggio"];
            string messaggio = "";
            if (Session["ErrorPageText"] != null) messaggio = Session["ErrorPageText"].ToString();
            lblInfoErrore.Text = messaggio;
            Session["ErrorPageText"] = null;
            Session[SessionManager.UTENTE] = null;
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Login.aspx", true);
        }
    }
}