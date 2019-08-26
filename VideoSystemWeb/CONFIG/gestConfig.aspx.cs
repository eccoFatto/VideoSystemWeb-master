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
        protected void Page_PreInit(object sender, EventArgs e)
        {
            CheckIsMobile();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Esito esito = new Esito();
            if (!Page.IsPostBack) {
                if (!AbilitazioneInScrittura())
                {
                    btnModifica.Visible = false;
                }

                //List<DatiBancari> listaDatiBancari = Config_BLL.Instance.getListaDatiBancari(ref esito);
                //List<GiorniPagamentoFatture> listaGPF = Config_BLL.Instance.getListaGiorniPagamentoFatture(ref esito);
            }
            
            List<Config> listaConf = BLL.Config_BLL.Instance.getListaConfig(ref esito);
            if (esito.Codice == 0)
            {
                //int top = 0;
                //int totValori = listaConf.Count;
                foreach (Config item in listaConf)
                {
                    //System.Web.UI.HtmlControls.HtmlGenericControl divRiga = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
                    //divRiga.Attributes.Add("class", "w3-container w3-border w3-border-teal w3-margin");
                    //divRiga.Attributes.Add("id", "div_" + item.Chiave);

                    // INSERISCO IL DIV RIGA
                    System.Web.UI.HtmlControls.HtmlGenericControl divRiga = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
                    divRiga.Attributes.Add("class", "w3-row-padding w3-border w3-border-teal w3-margin w3-round");
                    divRiga.Attributes.Add("id", "div_" + item.Chiave);

                    // INSERISCO IL DIV PRIMO TERZO DELLA RIGA
                    System.Web.UI.HtmlControls.HtmlGenericControl divPrimoTerzo = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
                    divPrimoTerzo.Attributes.Add("class", "w3-quarter w3-teal w3-round w3-center");
                    //divRiga.Attributes.Add("id", "div_" + item.Chiave);
                    divRiga.Controls.Add(divPrimoTerzo);

                    //phCampiConfig.Controls.Add(divRiga);

                    //top += 10;

                    Label Label1 = new Label();
                    Label1.ID = "lbl_" + item.Chiave;
                    Label1.Text = item.Chiave;
                    //Label1.Style["Position"] = "Relative";
                    //Label1.Style["Top"] = top.ToString() + "px";
                    //Label1.Style["Left"] = "100px";
                    Label1.CssClass = "w3-container";
                    Label1.Font.Bold = true;
                    //phCampiConfig.Controls.Add(Label1);
                    //divRiga.Controls.Add(Label1);
                    divPrimoTerzo.Controls.Add(Label1);

                    // INSERISCO IL DIV SECONDO TERZO DELLA RIGA
                    System.Web.UI.HtmlControls.HtmlGenericControl divSecondoTerzo = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
                    divSecondoTerzo.Attributes.Add("class", "w3-quarter");
                    //divRiga.Attributes.Add("id", "div_" + item.Chiave);
                    divRiga.Controls.Add(divSecondoTerzo);

                    //top += 10;
                    TextBox TextBox1 = new TextBox();
                    TextBox1.ID = "tb_" + item.Chiave;
                    TextBox1.Text = item.valore;
                    //TextBox1.Style["Position"] = "Relative";
                    //TextBox1.Style["Top"] = top.ToString() + "px";
                    //TextBox1.Style["Left"] = "100px";
                    TextBox1.Style["Width"] = "99%";
                    TextBox1.ReadOnly = true;
                    TextBox1.CssClass = "w3-input w3-border w3-margin";
                    TextBox1.TextChanged += new System.EventHandler(TextBox_TextChanged);
                    //phCampiConfig.Controls.Add(TextBox1);
                    //divRiga.Controls.Add(TextBox1);
                    divSecondoTerzo.Controls.Add(TextBox1);

                    // INSERISCO IL DIV TERZO TERZO DELLA RIGA
                    System.Web.UI.HtmlControls.HtmlGenericControl divTerzoTerzo = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
                    divTerzoTerzo.Attributes.Add("class", "w3-half w3-center");
                    divRiga.Controls.Add(divTerzoTerzo);

                    Label Label2 = new Label();
                    Label2.ID = "lbl2_" + item.Chiave;
                    Label2.Text = item.Descrizione;
                    Label2.Font.Italic = true;
                    Label2.CssClass = "w3-container";
                    divTerzoTerzo.Controls.Add(Label2);


                    phCampiConfig.Controls.Add(divRiga);
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

        protected void btnModifica_Click(object sender, EventArgs e)
        {
            abilitaTextBoxes(this,false);

            btnSalva.Visible = true;
            btnAnnulla.Visible = true;
            btnModifica.Visible = false;

            btnSalvaUp.Visible = true;
            btnAnnullaUp.Visible = true;
            btnModificaUp.Visible = false;

        }


        protected void btnSalva_Click(object sender, EventArgs e)
        {
            aggiornaValori(this);
            btnAnnulla_Click(null, null);
        }

        protected void btnAnnulla_Click(object sender, EventArgs e)
        {
            abilitaTextBoxes(this, true);
            btnSalva.Visible = false;
            btnAnnulla.Visible = false;
            btnModifica.Visible = true;

            btnSalvaUp.Visible = false;
            btnAnnullaUp.Visible = false;
            btnModificaUp.Visible = true;
        }


        public void abilitaTextBoxes(Control parent, bool ro)
        {
            foreach (Control x in parent.Controls)
            {
                if ((x.GetType() == typeof(TextBox)))
                {
                    ((TextBox)(x)).ReadOnly = ro;
                }
                if (x.HasControls())
                {
                    abilitaTextBoxes(x,ro);
                }
            }
        }

        public void aggiornaValori(Control parent)
        {
            foreach (Control x in parent.Controls)
            {
                if ((x.GetType() == typeof(TextBox)))
                {
                    TextBox tb = ((TextBox)(x));
                    string chiave = tb.ID.Substring(3);
                    string valore = tb.Text.Trim();

                    Esito esito = new Esito();
                    Config cfg = Config_BLL.Instance.getConfig(ref esito, chiave);
                    if (esito.Codice == 0)
                    {
                        if (!valore.Equals(cfg.valore)) { 
                            cfg.valore = valore;
                            esito = Config_BLL.Instance.AggiornaConfig(cfg);
                        }
                    }
                }
                if (x.HasControls())
                {
                    aggiornaValori(x);
                }
            }
        }

    }

}