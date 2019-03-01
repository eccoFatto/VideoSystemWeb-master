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
        public List<DatiArticoli> listaArticoli
        {
            get
            {
                return (List<DatiArticoli>)ViewState["listaArticoli"];
            }
            set
            {
                ViewState["listaArticoli"] = value;
            }
        }

        public delegate void PopupHandler(string operazionePopup); // delegato per l'evento
        public event PopupHandler RichiediOperazionePopup; //evento

        List<Art_Gruppi> listaGruppi = new List<Art_Gruppi>();
        //List<DatiArticoli> listaArticoli = new List<DatiArticoli>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                caricaDdlGenere();
                caricaDdlGruppo();
                caricaDdlSottogruppo();
                caricaListaGruppiArticoli();

                gvGruppi.DataSource = listaGruppi;
                gvGruppi.DataBind();
            }
        }

        private void caricaListaGruppiArticoli()
        {
            Esito esito = new Esito();
            listaGruppi = Articoli_BLL.Instance.CaricaListaGruppi(ref esito);
        }

        private void caricaDdlGenere()
        {
            Esito esito = new Esito();
            List<Tipologica> listaTipologicaGenere = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_GENERE);

            ddl_Genere.DataSource = listaTipologicaGenere;
            ddl_Genere.DataTextField = "nome";
            ddl_Genere.DataValueField = "id";
            ddl_Genere.DataBind();
        }

        private void caricaDdlGruppo()
        {
            Esito esito = new Esito();
            List<Tipologica> listaTipologicaGruppo = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_GRUPPO);

            ddl_Gruppo.DataSource = listaTipologicaGruppo;
            ddl_Gruppo.DataTextField = "nome";
            ddl_Gruppo.DataValueField = "id";
            ddl_Gruppo.DataBind();
        }

        private void caricaDdlSottogruppo()
        {
            Esito esito = new Esito();
            List<Tipologica> listaTipologicaSottogruppo = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_SOTTOGRUPPO);

            ddl_Sottogruppo.DataSource = listaTipologicaSottogruppo;
            ddl_Sottogruppo.DataTextField = "nome";
            ddl_Sottogruppo.DataValueField = "id";
            ddl_Sottogruppo.DataBind();
        }

        private void aggiungiAListaArticoli(int idArticolo)
        {
            Esito esito = new Esito();
            int idEvento = 0;
            listaArticoli.AddRange(Articoli_BLL.Instance.CaricaListaArticoliByIDGruppo(idEvento, idArticolo, ref esito));
        }

        #region COMPORTAMENTO ELEMENTI PAGINA
        protected void btnOK_Click(object sender, EventArgs e)
        {
            List<DatiArticoli> listaArticoli = (List<DatiArticoli>)ViewState["listaArticoli"];
            DatiArticoli articoloSelezionato;

            if (ViewState["idArticolo"]==null)
            {
                long identificatoreOggetto = (long)ViewState["identificatoreArticolo"];
                articoloSelezionato = listaArticoli.FirstOrDefault(x => x.IdentificatoreOggetto == identificatoreOggetto);
            }
            else
            {
                int idArticolo = (int)ViewState["idArticolo"];
                articoloSelezionato = listaArticoli.FirstOrDefault(x => x.Id == idArticolo);
            }
            
            var index = listaArticoli.IndexOf(articoloSelezionato);

            articoloSelezionato.Descrizione = txt_Descrizione.Text;
            articoloSelezionato.DescrizioneLunga = txt_DescrizioneLunga.Text;
            articoloSelezionato.Costo = decimal.Parse(txt_Costo.Text);
            articoloSelezionato.Prezzo = decimal.Parse(txt_Prezzo.Text);
            articoloSelezionato.Iva = int.Parse(txt_Iva.Text);
            articoloSelezionato.IdTipoGenere = int.Parse(ddl_Genere.SelectedValue);
            articoloSelezionato.IdTipoGruppo = int.Parse(ddl_Gruppo.SelectedValue);
            articoloSelezionato.IdTipoSottogruppo = int.Parse(ddl_Sottogruppo.SelectedValue);
            articoloSelezionato.Stampa = ddl_Stampa.SelectedValue == "1";


            if (index != -1)
            { 
                listaArticoli[index] = articoloSelezionato;
            }

            gvArticoli.DataSource = listaArticoli;
            gvArticoli.DataBind();

            ClearModificaArticoli();
            RichiediOperazionePopup("UPDATE");
        }

        protected void btnAnnullaModifiche_Click(object sender, EventArgs e)
        {
            ClearModificaArticoli();
            RichiediOperazionePopup("UPDATE");
        }

        protected void gvGruppi_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idSelezione = Convert.ToInt32(e.CommandArgument);

            caricaListaGruppiArticoli();
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
            List<DatiArticoli> listaArticoli = (List<DatiArticoli>)ViewState["listaArticoli"];
            DatiArticoli articoloSelezionato;

            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            int id = Convert.ToInt32(commandArgs[0]);

            if (id == 0)
            {
                long identificatoreOggetto = Convert.ToInt64(commandArgs[1]);
                ViewState["identificatoreArticolo"] = identificatoreOggetto;
                articoloSelezionato = listaArticoli.FirstOrDefault(x => x.IdentificatoreOggetto == identificatoreOggetto);

            }
            else
            {
                ViewState["idArticolo"] = id;
                articoloSelezionato = listaArticoli.FirstOrDefault(x => x.Id == id);
            }

            switch (e.CommandName)
            {
                case "modifica":
                    txt_Descrizione.Text = articoloSelezionato.Descrizione;
                    txt_DescrizioneLunga.Text = articoloSelezionato.DescrizioneLunga;
                    txt_Costo.Text = articoloSelezionato.Costo.ToString();
                    txt_Prezzo.Text = articoloSelezionato.Prezzo.ToString();
                    txt_Iva.Text = articoloSelezionato.Iva.ToString();
                    ddl_Genere.SelectedValue = articoloSelezionato.IdTipoGenere.ToString();
                    ddl_Gruppo.SelectedValue = articoloSelezionato.IdTipoGruppo.ToString();
                    ddl_Sottogruppo.SelectedValue = articoloSelezionato.IdTipoSottogruppo.ToString();

                    panelModificaArticolo.Style.Remove("display");

                    break;
                case "elimina":
                    listaArticoli.Remove(articoloSelezionato);
                    gvArticoli.DataSource = listaArticoli;
                    gvArticoli.DataBind();
                    break;
            }
            

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
            panelModificaArticolo.Style.Add("display", "none");
            txt_Descrizione.Text = "";
            txt_DescrizioneLunga.Text = "";
            txt_Costo.Text = "";
            txt_Prezzo.Text = "";
            txt_Iva.Text = "";
        }

        public void PopolaOfferta(int idDatiAgenda)
        {
            Esito esito = new Esito();
            listaArticoli = Articoli_BLL.Instance.CaricaListaArticoliByIDEvento(idDatiAgenda, ref esito);
            if (listaArticoli != null && listaArticoli.Count > 0)
            {
                lbl_selezionareArticolo.Visible = false;
                gvArticoli.DataSource = listaArticoli;
                gvArticoli.DataBind();
            }
        }
    }
}