using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoSystemWeb.BLL;
using VideoSystemWeb.BLL.Stampa;
using VideoSystemWeb.Entity;

namespace VideoSystemWeb.Agenda.userControl
{
    public partial class RiepilogoOfferta : System.Web.UI.UserControl
    {
        public delegate void PopupHandler(string operazionePopup); // delegato per l'evento
        public event PopupHandler RichiediOperazionePopup; //evento

        public delegate List<DatiArticoli> ListaArticoliHandler(); // delegato per l'evento
        public event ListaArticoliHandler RichiediListaArticoli; //evento

        public delegate string CodiceLavoroHandler(); // delegato per l'evento
        public event CodiceLavoroHandler RichiediCodiceLavoro; //evento

        // public List<DatiArticoli> listaDatiArticoli { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(this.btnStampaOfferta);
            }
        }

        public Esito popolaPannelloRiepilogo(DatiAgenda eventoSelezionato)
        {
            Esito esito = new Esito();

            AbilitaVisualizzazioneStampa(false);

            lbl_Data.Text = lbl_DataStampa.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lbl_Produzione.Text = lbl_ProduzioneStampa.Text = eventoSelezionato.produzione;
            lbl_Lavorazione.Text = lbl_LavorazioneStampa.Text = eventoSelezionato.lavorazione;
            lbl_DataLavorazione.Text = lbl_DataLavorazioneStampa.Text = eventoSelezionato.data_inizio_lavorazione.ToString("dd/MM/yyyy");

            Anag_Clienti_Fornitori cliente = Anag_Clienti_Fornitori_BLL.Instance.getAziendaById(eventoSelezionato.id_cliente, ref esito);
            lbl_Cliente.Text = lbl_ClienteStampa.Text = cliente.RagioneSociale;
            lbl_IndirizzoCliente.Text = lbl_IndirizzoClienteStampa.Text = cliente.IndirizzoOperativo;
            lbl_PIvaCliente.Text = lbl_PIvaClienteStampa.Text = string.IsNullOrEmpty(cliente.PartitaIva) ? cliente.CodiceFiscale : cliente.PartitaIva;

            lbl_CodLavorazione.Text = lbl_CodLavorazioneStampa.Text = eventoSelezionato.codice_lavoro;

            List<DatiArticoli> listaDatiArticoli = RichiediListaArticoli().Where(x => x.Stampa).ToList<DatiArticoli>();  //listaDatiArticoli.Where(x => x.Stampa).ToList<DatiArticoli>();  //popupOfferta.listaDatiArticoli.Where(x => x.Stampa).ToList<DatiArticoli>();


            gvArticoli.DataSource = listaDatiArticoli;
            gvArticoli.DataBind();

            decimal totPrezzo = 0;
            decimal totIVA = 0;

            foreach (DatiArticoli art in listaDatiArticoli)
            {
                totPrezzo += art.Prezzo * art.Quantita;
                totIVA += (art.Prezzo * art.Iva / 100) * art.Quantita;
            }

            totale.Text = totaleStampa.Text = string.Format("{0:N2}", totPrezzo);
            totaleIVA.Text = totaleIVAStampa.Text = string.Format("{0:N2}", totIVA);
            totaleEuro.Text = totaleEuroStampa.Text = string.Format("{0:N2}", totPrezzo + totIVA);

            int idTipoProtocollo = UtilityTipologiche.getElementByNome(UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_PROTOCOLLO), "offerta", ref esito).id;
            List<Protocolli> listaProtocolli = Protocolli_BLL.Instance.getProtocolliByCodLavIdTipoProtocollo(eventoSelezionato.codice_lavoro, idTipoProtocollo, ref esito, true);
            string protocollo = listaProtocolli.Count == 0 ? "N.D." : eventoSelezionato.codice_lavoro + " - " + listaProtocolli.First().Numero_protocollo;
            lbl_Protocollo.Text = lbl_ProtocolloStampa.Text = protocollo;

            val_pagamentoSchermo.Text = val_pagamentoStampa.Text = cliente.Pagamento + " gg DFFM";
            val_consegnaSchermo.Text = val_consegnaStampa.Text = cliente.IndirizzoLegale + " " + cliente.ComuneLegale;

            return esito;
        }

        public MemoryStream GeneraPdf()
        {
            string prefissoUrl = Request.Url.Scheme + "://" + Request.Url.Authority;
            imgLogo.ImageUrl = prefissoUrl + "/Images/logoVSP_trim.png";
            imgDNV.ImageUrl = prefissoUrl + "/Images/DNV_2008_ITA2.jpg";

            AbilitaVisualizzazioneStampa(true);


            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            modalRiepilogoContent.RenderControl(hw);


            MemoryStream workStream = BaseStampa.Instance.GeneraPdf(sw.ToString());

            sw.Flush();
            hw.Flush();

            return workStream;
        }

        private void AbilitaVisualizzazioneStampa(bool isVisualizzazioneStampa)
        {
            gvArticoli.Columns[4].Visible = !isVisualizzazioneStampa;

            intestazioneSchermo.Visible = !isVisualizzazioneStampa;
            protocolloSchermo.Visible = !isVisualizzazioneStampa;
            totaliSchermo.Visible = !isVisualizzazioneStampa;
            footerSchermo.Visible = !isVisualizzazioneStampa;

            intestazioneStampa.Visible = isVisualizzazioneStampa;
            totaliStampa.Visible = isVisualizzazioneStampa;
            footerStampa.Visible = isVisualizzazioneStampa;
        }

        protected void btnStampa_Click(object sender, EventArgs e)
        {
            string codiceLavoro = RichiediCodiceLavoro();

            string nomeFile = "Offerta_" + codiceLavoro + ".pdf";
            MemoryStream workStream = GeneraPdf();

            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + nomeFile);
            Response.AddHeader("Content-Length", workStream.Length.ToString());
            Response.BinaryWrite(workStream.ToArray());
            Response.Flush();
            Response.Close();
            Response.End();
        }

        protected void btnModificaNote_Click(object sender, EventArgs e)
        {
            //panelModificaNote.Style.Add("display", "none");
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriModificaNote", script: "javascript: document.getElementById('panelModificaNote').style.display='block'", addScriptTags: true);
            //ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriModificaArticolo", script: "javascript: document.getElementById('" + panelRecuperaOfferta.ClientID + "').style.display='block'", addScriptTags: true);
            //RichiediOperazionePopup("UPDATE");
        }

        protected void btnOKModificaNote_Click(object sender, EventArgs e)
        {
            val_pagamentoSchermo.Text = val_pagamentoStampa.Text = txt_Pagamento.Text;
            val_consegnaSchermo.Text = val_consegnaStampa.Text = txt_Consegna.Text;


            ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriModificaNote", script: "javascript: document.getElementById('panelModificaNote').style.display='none'", addScriptTags: true);
            //RichiediOperazionePopup("UPDATE_RIEPILOGO");
        }

        protected void gvArticoli_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblDescrizione = (Label)e.Row.FindControl("lblDescrizione");
                lblDescrizione.Text = lblDescrizione.Text.Replace("\n", "<br/>");

                Label totaleRiga = (Label)e.Row.FindControl("totaleRiga");
                totaleRiga.Text = string.Format("{0:N2}", (int.Parse(e.Row.Cells[2].Text) * int.Parse(e.Row.Cells[3].Text)));

                e.Row.Cells[3].Text = string.Format("{0:N2}", (int.Parse(e.Row.Cells[3].Text)));
                e.Row.Cells[4].Text = string.Format("{0:N2}", (int.Parse(e.Row.Cells[4].Text)));
            }
        }
    }
}