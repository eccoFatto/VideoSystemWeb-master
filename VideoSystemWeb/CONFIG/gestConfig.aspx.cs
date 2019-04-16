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

namespace VideoSystemWeb.CONFIG
{
    public partial class gestConfig : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Esito esito = new Esito();
            List<Config> listaConf = BLL.Config_BLL.Instance.getListaConfig(ref esito);
            if (esito.codice == 0)
            {
                int top = 0;
                foreach (Config item in listaConf)
                {
                    top += 10;

                    Label Label1 = new Label();
                    Label1.ID = "lbl_" + item.Chiave;
                    Label1.Text = item.Chiave;
                    Label1.Style["Position"] = "Relative";
                    Label1.Style["Top"] = top.ToString() + "px";
                    Label1.Style["Left"] = "100px";
                    //Label1.CssClass = "w3-border";
                    phCampiConfig.Controls.Add(Label1);

                    top += 10;
                    TextBox TextBox1 = new TextBox();
                    TextBox1.ID = "tb_" + item.valore;
                    TextBox1.Text = item.valore;
                    TextBox1.Style["Position"] = "Relative";
                    TextBox1.Style["Top"] = top.ToString() + "px";
                    TextBox1.Style["Left"] = "100px";
                    TextBox1.Style["Width"] = "300px";

                    TextBox1.CssClass = "w3-input w3-border";

                    TextBox1.TextChanged += new System.EventHandler(TextBox_TextChanged);
                    phCampiConfig.Controls.Add(TextBox1);
                }
            }
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "chiudiLoader", script: "$('.loader').hide();", addScriptTags: true);
        }
        private void TextBox_TextChanged(object sender, System.EventArgs e)
        {
            TextBox txtBoxSender = (TextBox)sender;
            string strTextBoxID = txtBoxSender.ID;

            switch (strTextBoxID)
            {
                case "TextBox1":
                    //Label3.Text = "TextBox1 text was changed";
                    break;
                case "TextBox2":
                    //Label4.Text = "TextBox2 text was changed";
                    break;
            }
        }
    }
}