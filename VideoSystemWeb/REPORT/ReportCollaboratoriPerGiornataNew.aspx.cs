using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoSystemWeb.BLL;
namespace VideoSystemWeb.REPORT
{
    public partial class ReportCollaboratoriPerGiornataNew : BasePage
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            CheckIsMobile();
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}