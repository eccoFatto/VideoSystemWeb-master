using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace VideoSystemWeb
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Codice eseguito all'avvio dell'applicazione
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //Session["IS_AUTHENTICATED"] = true;
            Application.Set("IS_AUTHENTICATED", "true");
        }
        void Session_End(object sender, EventArgs e)
        {
            //Session["ErrorPageText"] = "TimeOut Sessione";
            //string url = String.Format("~/pageError.aspx");
            //Response.Redirect(url, true);
            Application.Set("IS_AUTHENTICATED", "false");
        }
        void Session_Start(object sender, EventArgs e)
        {
            Session.Timeout = 50;
        }
    }
}