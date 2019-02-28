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

            //if (ViewState["listaSelezione"] != null)
            //{
            //    listaSelezione = (List<ProvaElementiOfferta>)ViewState["listaSelezione"];

            //    gvSelezioneOfferta.DataSource = listaSelezione;
            //    gvSelezioneOfferta.DataBind();
            //}
        }

        //private void caricaListaGenerale()
        //{
        //    listaGenerale.Add(new ProvaElementiOfferta(1, "19 20 21", "Personale tecnico monocamera", "", 1, 100, 40));
        //    listaGenerale.Add(new ProvaElementiOfferta(2, "22 23 24", "Personale tecnico bicamera", "", 1, 200, 70));
        //    listaGenerale.Add(new ProvaElementiOfferta(3, "25 26 27", "Personale tecnico tricamera", "", 2, 300, 100));
        //    listaGenerale.Add(new ProvaElementiOfferta(4, "Acquisti", "Costi acquisti", "Acquisti", 1, 80, 70));
        //}

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
            
        }

        protected void btnAnnulla_Click(object sender, EventArgs e)
        {
            
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

            
        }
        #endregion

        public void ClearOfferta()
        {
            lbl_selezionareArticolo.Visible = true;
            ViewState["listaArticoli"] = null;
            gvArticoli.DataSource = null;
            gvArticoli.DataBind();

            //NascondiErroriValidazione();
        }
    }
}