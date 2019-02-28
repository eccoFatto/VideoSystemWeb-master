using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoSystemWeb.BLL;
using VideoSystemWeb.Entity;

namespace VideoSystemWeb.Agenda.userControl
{
    public partial class Offerta : System.Web.UI.UserControl
    {
        public delegate void PopupHandler(string operazionePopup); // delegato per l'evento
        public event PopupHandler RichiediOperazionePopup; //evento

        List<Art_Gruppi> listaGruppi = new List<Art_Gruppi>();
        List<DatiArticoli> listaArticoli = new List<DatiArticoli>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                caricaListaGruppi();

                gvGruppi.DataSource = listaGruppi;
                gvGruppi.DataBind();

            }
        }

        private void caricaListaGruppi()
        {
            Esito esito = new Esito();
            listaGruppi = Articoli_BLL.Instance.CaricaListaGruppi(ref esito);
        }

        private void aggiungiAListaArticoli(int idArticolo)
        {
            Esito esito = new Esito();
            int idEvento = 0;// ((DatiAgenda)ViewState["eventoSelezionato"]).id;
            listaArticoli.AddRange(Articoli_BLL.Instance.CaricaListaArticoliByIDGruppo(idEvento, idArticolo, ref esito));
        }

        #region COMPORTAMENTO ELEMENTI PAGINA
        protected void btnSalva_Click(object sender, EventArgs e)
        {
            long identificatoreOggetto = (long)ViewState["identificatoreArticolo"];
            List<DatiArticoli> listaArticoli = (List<DatiArticoli>)ViewState["listaArticoli"];
            DatiArticoli articoloSelezionato = listaArticoli.FirstOrDefault(x => x.IdentificatoreOggetto == identificatoreOggetto);
            var index = listaArticoli.IndexOf(articoloSelezionato);

            articoloSelezionato.Descrizione = txt_Descrizione.Text;
            articoloSelezionato.DescrizioneLunga = txt_DescrizioneLunga.Text;
            articoloSelezionato.Costo = decimal.Parse(txt_Costo.Text);
            articoloSelezionato.Prezzo = decimal.Parse(txt_Prezzo.Text);
            articoloSelezionato.Iva = int.Parse(txt_Iva.Text);

            if (index != -1)
            { 
                listaArticoli[index] = articoloSelezionato;
            }

            gvArticoli.DataSource = listaArticoli;
            gvArticoli.DataBind();

            ClearModificaArticoli();
            panelModificaArticolo.Style.Add("display", "none");
            RichiediOperazionePopup("UPDATE");
        }

        protected void btnAnnulla_Click(object sender, EventArgs e)
        {
            ClearModificaArticoli();
            panelModificaArticolo.Style.Add("display", "none");
            RichiediOperazionePopup("UPDATE");
        }

        protected void gvGruppi_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idSelezione = Convert.ToInt32(e.CommandArgument);

            caricaListaGruppi();
            listaArticoli = (List<DatiArticoli>)ViewState["listaArticoli"];
            if (listaArticoli == null)
            {
                listaArticoli = new List<DatiArticoli>();
            }
            aggiungiAListaArticoli(idSelezione);

            ViewState["listaArticoli"] = listaArticoli;

            if (listaArticoli != null && listaArticoli.Count > 0)
            {
                lbl_selezionareArticolo.Visible = false;
                gvArticoli.DataSource = listaArticoli;
                gvArticoli.DataBind();

                RichiediOperazionePopup("UPDATE");
            }
        }

        protected void gvArticoli_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            long identificatoreOggetto = Convert.ToInt64(e.CommandArgument);
            ViewState["identificatoreArticolo"] = identificatoreOggetto;

            List<DatiArticoli> listaArticoli = (List<DatiArticoli>)ViewState["listaArticoli"];
            DatiArticoli articoloSelezionato = listaArticoli.FirstOrDefault(x => x.IdentificatoreOggetto == identificatoreOggetto);

            txt_Descrizione.Text = articoloSelezionato.Descrizione;
            txt_DescrizioneLunga.Text = articoloSelezionato.DescrizioneLunga;
            txt_Costo.Text = articoloSelezionato.Costo.ToString();
            txt_Prezzo.Text = articoloSelezionato.Prezzo.ToString();
            txt_Iva.Text = articoloSelezionato.Iva.ToString();

            panelModificaArticolo.Style.Remove("display");

            RichiediOperazionePopup("UPDATE");
        }
        #endregion

        public void ClearOfferta()
        {
            ClearModificaArticoli();
            lbl_selezionareArticolo.Visible = true;
            ViewState["listaArticoli"] = null;
            gvArticoli.DataSource = null;
            gvArticoli.DataBind();

            //NascondiErroriValidazione();
        }

        private void ClearModificaArticoli()
        {
            ViewState["identificatoreArticolo"] = null;

            txt_Descrizione.Text = "";
            txt_DescrizioneLunga.Text = "";
            txt_Costo.Text = "";
            txt_Prezzo.Text = "";
            txt_Iva.Text = "";
        }
    }
}