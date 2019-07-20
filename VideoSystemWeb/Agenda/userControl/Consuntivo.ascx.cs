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
    public partial class Consuntivo : System.Web.UI.UserControl
    {
        BasePage basePage = new BasePage();
        public delegate void PopupHandler(string operazionePopup); // delegato per l'evento
        public event PopupHandler RichiediOperazionePopup; //evento

        public delegate List<DatiArticoli> ListaArticoliHandler(); // delegato per l'evento
        public event ListaArticoliHandler RichiediListaArticoli; //evento

        public delegate string CodiceLavoroHandler(); // delegato per l'evento
        public event CodiceLavoroHandler RichiediCodiceLavoro; //evento

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                scriptManager.RegisterPostBackControl(this.btnStampaConsuntivo);
            }
            else
            {
                foreach (GiorniPagamentoFatture gpf in SessionManager.ListaGPF)
                {
                    cmbMod_Pagamento.Items.Add(new ListItem(gpf.Descrizione, gpf.Giorni));
                }

                foreach (DatiBancari datiBancari in SessionManager.ListaDatiBancari)
                {
                    ddl_Banca.Items.Add(new ListItem(datiBancari.Banca, datiBancari.DatiCompleti));
                }
            }
        }

        #region COMPORTAMENTO ELEMENTI PAGINA
        protected void btnStampaConsuntivo_Click(object sender, EventArgs e)
        {
            string codiceLavoro = RichiediCodiceLavoro();

            string nomeFile = "Consuntivo_" + codiceLavoro + ".pdf";
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
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriModificaNote", script: "javascript: document.getElementById('panelModificaNote').style.display='block'", addScriptTags: true);
        }

        protected void btnOKModificaNote_Click(object sender, EventArgs e)
        {
            NoteOfferta noteOfferta = (NoteOfferta)ViewState["NoteOfferta"];
            noteOfferta.Banca = ddl_Banca.SelectedValue;
            noteOfferta.Pagamento = int.Parse(cmbMod_Pagamento.SelectedValue);
            noteOfferta.Consegna = txt_Consegna.Text;
            noteOfferta.Note = "";
            NoteOfferta_BLL.Instance.AggiornaNoteOfferta(noteOfferta);

            val_bancaStampa.Text = noteOfferta.Banca;
            val_pagamentoStampa.Text = noteOfferta.Pagamento.ToString() + " gg DFFM";
            val_consegnaStampa.Text = noteOfferta.Consegna;

            RichiediOperazionePopup("SAVE_PDF_CONSUNTIVO");

            ScriptManager.RegisterStartupScript(Page, typeof(Page), "aggiornaNote", script: "javascript: aggiornaRiepilogo()", addScriptTags: true);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "chiudiModificaNote", script: "javascript: document.getElementById('panelModificaNote').style.display='none'", addScriptTags: true);
        }

        protected void gvArticoli_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblDescrizione = (Label)e.Row.FindControl("lblDescrizione");
                lblDescrizione.Text = lblDescrizione.Text.Replace("\n", "<br/>");

                Label totaleRiga = (Label)e.Row.FindControl("totaleRiga");
                totaleRiga.Text = string.Format("{0:N2}", (int.Parse(e.Row.Cells[2].Text) * int.Parse(e.Row.Cells[4].Text)));

                e.Row.Cells[2].Text = string.Format("{0:N2}", (int.Parse(e.Row.Cells[2].Text)));
                // e.Row.Cells[3].Text = string.Format("{0:N2}", (int.Parse(e.Row.Cells[3].Text)));
            }
        }
        #endregion

        #region OPERAZIONI POPUP
        public Esito popolaPannelloRiepilogo(DatiAgenda eventoSelezionato)
        {
            Esito esito = new Esito();

            //AbilitaVisualizzazioneStampa(false);

            lbl_Data.Text = lbl_DataStampa.Text = DateTime.Now.ToString("dd/MM/yyyy ore HH:mm");
            lbl_Produzione.Text = lbl_ProduzioneStampa.Text = eventoSelezionato.produzione;
            lbl_Lavorazione.Text = lbl_LavorazioneStampa.Text = eventoSelezionato.lavorazione;
            lbl_DataLavorazione.Text = lbl_DataLavorazioneStampa.Text = eventoSelezionato.data_inizio_lavorazione.ToString("dd/MM/yyyy");

            Anag_Clienti_Fornitori cliente = Anag_Clienti_Fornitori_BLL.Instance.getAziendaById(eventoSelezionato.id_cliente, ref esito);
            lbl_Cliente.Text = lbl_ClienteStampa.Text = cliente.RagioneSociale;
            lbl_IndirizzoCliente.Text = lbl_IndirizzoClienteStampa.Text = cliente.IndirizzoOperativo + " " + cliente.ComuneOperativo;
            lbl_PIvaCliente.Text = lbl_PIvaClienteStampa.Text = string.IsNullOrEmpty(cliente.PartitaIva) ? cliente.CodiceFiscale : cliente.PartitaIva;

            lbl_CodLavorazione.Text = lbl_CodLavorazioneStampa.Text = eventoSelezionato.codice_lavoro;

            List<DatiArticoli> listaDatiArticoli = RichiediListaArticoli().Where(x => x.Stampa).ToList<DatiArticoli>();

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
            string protocollo = listaProtocolli.Count == 0 ? "N.D." : listaProtocolli.First().Numero_protocollo + "-" + eventoSelezionato.codice_lavoro;
            lbl_Protocollo.Text = lbl_ProtocolloStampa.Text = protocollo;

            NoteOfferta noteOfferta = NoteOfferta_BLL.Instance.getNoteOffertaByIdDatiAgenda(eventoSelezionato.id, ref esito);

            // se non viene trovata una notaOfferta (vecchi eventi) viene creata e salvata
            if (noteOfferta.Id == 0)
            {
                List<DatiBancari> datiBancari = Config_BLL.Instance.getListaDatiBancari(ref esito);
                noteOfferta = new NoteOfferta { Id_dati_agenda = eventoSelezionato.id, Banca = datiBancari[0].DatiCompleti, Pagamento = cliente.Pagamento, Consegna = cliente.TipoIndirizzoLegale + " " + cliente.IndirizzoLegale + " " + cliente.NumeroCivicoLegale + " " + cliente.CapLegale + " " + cliente.ProvinciaLegale + " " };// "Unicredit Banca: IBAN: IT39H0200805198000103515620", Pagamento = cliente.Pagamento, Consegna = cliente.TipoIndirizzoLegale + " " + cliente.IndirizzoLegale + " " + cliente.NumeroCivicoLegale + " " + cliente.CapLegale + " " + cliente.ProvinciaLegale + " " };

                NoteOfferta_BLL.Instance.CreaNoteOfferta(noteOfferta, ref esito);
            }

            ViewState["NoteOfferta"] = noteOfferta;

            val_bancaSchermo.Text = val_bancaStampa.Text = noteOfferta.Banca;// 
            val_pagamentoSchermo.Text = val_pagamentoStampa.Text = noteOfferta.Pagamento + " gg DFFM";
            val_consegnaSchermo.Text = val_consegnaStampa.Text = noteOfferta.Consegna;

            //ddl_Banca.SelectedValue = noteOfferta.Banca;// commentato perché se non trova l'elemento (e può succedere) schioda
            txt_Consegna.Text = noteOfferta.Consegna;
            cmbMod_Pagamento.SelectedValue = noteOfferta.Pagamento.ToString();

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
            modalConsuntivoContent.RenderControl(hw);


            MemoryStream workStream = BaseStampa.Instance.GeneraPdf(sw.ToString());

            sw.Flush();
            hw.Flush();

            return workStream;
        }

        private void AbilitaVisualizzazioneStampa(bool isVisualizzazioneStampa)
        {
            gvArticoli.Columns[3].Visible = !isVisualizzazioneStampa; // colonna costo

            intestazioneSchermo.Visible = !isVisualizzazioneStampa;
            protocolloSchermo.Visible = !isVisualizzazioneStampa;
            totaliSchermo.Visible = !isVisualizzazioneStampa;
            footerSchermo.Visible = !isVisualizzazioneStampa;

            intestazioneStampa.Visible = isVisualizzazioneStampa;
            totaliStampa.Visible = isVisualizzazioneStampa;
            footerStampa.Visible = isVisualizzazioneStampa;
        }
        #endregion

    }
}