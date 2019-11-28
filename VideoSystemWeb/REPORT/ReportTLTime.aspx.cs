using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using VideoSystemWeb.BLL;
using VideoSystemWeb.Entity;
using VideoSystemWeb.DAL;
using System.IO;
using System.Text.RegularExpressions;

namespace VideoSystemWeb.REPORT
{
    public partial class ReportTLTime : BasePage
    {
        BasePage basePage = new BasePage();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Page_PreInit(object sender, EventArgs e)
        {
            CheckIsMobile();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                // IMPOSTO LE DATE AL PRIMO E ALL'ULTIMO GIORNO DEL MESE CORRENTE
                DateTime dataDa = DateTime.Today.AddDays(-(DateTime.Today.Day + 1));
                DateTime dataA = DateTime.Today.AddDays(DateTime.DaysInMonth(DateTime.Today.Year,DateTime.Today.Month)-DateTime.Today.Day);
                tbDataDa.Text = "01/" + DateTime.Today.Month.ToString("00") + "/" + DateTime.Today.Year.ToString("0000");
                tbDataA.Text = DateTime.DaysInMonth(DateTime.Now.Year,DateTime.Now.Month).ToString("00") + "/" + DateTime.Today.Month.ToString("00") + "/" + DateTime.Today.Year.ToString("0000");
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "chiudiLoader", script: "$('.loader').hide();", addScriptTags: true);
        }

        protected void btnRicercaTLTime_Click(object sender, EventArgs e)
        {
            string queryRicerca = ConfigurationManager.AppSettings["QUERY_SEARCH_TLTIME"];

            string strDataDa = tbDataDa.Text.Trim().Substring(6,4) + "-" + tbDataDa.Text.Trim().Substring(3, 2) + "-" + tbDataDa.Text.Trim().Substring(0,2);
            string strDataA = tbDataA.Text.Trim().Substring(6, 4) + "-" + tbDataA.Text.Trim().Substring(3, 2) + "-" + tbDataA.Text.Trim().Substring(0, 2);


            queryRicerca = queryRicerca.Replace("@dataDa", strDataDa);
            queryRicerca = queryRicerca.Replace("@dataA", strDataA);

            Esito esito = new Esito();
            DataTable dtTLTime = Base_DAL.GetDatiBySql(queryRicerca, ref esito);

            if (dtTLTime!=null && dtTLTime.Rows!=null && dtTLTime.Rows.Count > 0)
            {
                btnCreaFileTLTime.Visible = true;
            }
            else
            {
                btnCreaFileTLTime.Visible = false;
            }
            Session["TaskTableTLTime"] = dtTLTime;
            gv_TLTime.DataSource = Session["TaskTableTLTime"];
            gv_TLTime.DataBind();

        }

        protected void gv_TLTime_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gv_TLTime_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void gv_TLTime_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void btnCreaFileTLTime_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["TaskTableTLTime"] != null)
                {
                    DataTable dtTLT = (DataTable)Session["TaskTableTLTime"];
                    if (dtTLT != null && dtTLT.Rows != null && dtTLT.Rows.Count > 0)
                    {

                        string dataFile = Server.MapPath("~/DOCUMENTI/TLTIME/tltime.txt");

                        // GESTIONE NOMI FILE TLTIME
                        string nomeFile = "Export_TLTime" + tbDataDa.Text.Replace("/", "-") + "_" + tbDataA.Text.Replace("/", "-") + ".txt";
                        string pathTLTime = ConfigurationManager.AppSettings["PATH_DOCUMENTI_TLTIME"] + nomeFile;
                        string mapPathTLTime = MapPath(ConfigurationManager.AppSettings["PATH_DOCUMENTI_TLTIME"]) + nomeFile;


                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(mapPathTLTime))
                        {
                            foreach (DataRow rigaTLT in dtTLT.Rows)
                            {
                                string riga = rigaTLT[1].ToString() + "," + rigaTLT[2].ToString() + "," + rigaTLT[3].ToString() + "," + rigaTLT[4].ToString() + "," + rigaTLT[5].ToString();
                                file.WriteLine(riga);
                            }
                            file.Flush();
                            file.Close();
                            if (File.Exists(mapPathTLTime))
                            {
                                //System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                                //response.ClearContent();
                                //response.Clear();
                                //response.ContentType = "text/plain";
                                //response.AddHeader("Content-Disposition",
                                //                   "attachment; filename=" + nomeFile + ";");
                                //response.TransmitFile(mapPathTLTime);

                                //Response.Buffer = true;
                                //Response.Charset = "";
                                //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                //Response.ContentType = "text/plain";
                                //Response.AddHeader("content-disposition", "attachment;filename=" + nomeFile);
                                //Response.TransmitFile(pathTLTime);
                                //Response.End();

                                //Response.Redirect(dataFile);
                                Page page = HttpContext.Current.Handler as Page;
                                ScriptManager.RegisterStartupScript(page, page.GetType(), "apriTLTime", script: "window.open('" + pathTLTime.Replace("~", "") + "')", addScriptTags: true);




                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                base.ShowError(ex.Message);
            }
        
        }
    }
}