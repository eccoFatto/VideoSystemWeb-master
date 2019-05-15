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
        BasePage basePage = new BasePage();

        public List<DatiArticoli> ListaDatiArticoli
        {
            get
            {
                return (List<DatiArticoli>)ViewState["listaDatiArticoli"];
            }
            set
            {
                ViewState["listaDatiArticoli"] = value;
            }
        }

        List<ArticoliGruppi> ListaArticoliGruppi
        {
            get
            {
                if (ViewState["listaArticoliGruppi"] == null || ((List<ArticoliGruppi>)ViewState["listaArticoliGruppi"]).Count == 0)
                {
                    ViewState["listaArticoliGruppi"] = Articoli_BLL.Instance.CaricaListaArticoliGruppi();
                }

                return (List<ArticoliGruppi>)ViewState["listaArticoliGruppi"];
            }
            set
            {
                ViewState["listaArticoliGruppi"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                gvGruppi.DataSource = ListaArticoliGruppi;
                gvGruppi.DataBind();
            }
        }


        #region COMPORTAMENTO ELEMENTI PAGINA
        protected void btnOK_Click(object sender, EventArgs e)
        {
            //List<DatiArticoli> listaDatiArticoli = (List<DatiArticoli>)ViewState["listaDatiArticoli"];
            DatiArticoli articoloSelezionato;

            if (ViewState["idArticolo"]==null)
            {
                long identificatoreOggetto = (long)ViewState["identificatoreArticolo"];
                articoloSelezionato = ListaDatiArticoli.FirstOrDefault(x => x.IdentificatoreOggetto == identificatoreOggetto);
            }
            else
            {
                int idArticolo = (int)ViewState["idArticolo"];
                articoloSelezionato = ListaDatiArticoli.FirstOrDefault(x => x.Id == idArticolo);
            }
            
            var index = ListaDatiArticoli.IndexOf(articoloSelezionato);

            articoloSelezionato.Descrizione = txt_Descrizione.Text;
            articoloSelezionato.DescrizioneLunga = txt_DescrizioneLunga.Text;
            articoloSelezionato.Costo = decimal.Parse(txt_Costo.Text);
            articoloSelezionato.Prezzo = decimal.Parse(txt_Prezzo.Text);
            articoloSelezionato.Iva = int.Parse(txt_Iva.Text);
            articoloSelezionato.Stampa = ddl_Stampa.SelectedValue == "1";
            articoloSelezionato.Quantita = int.Parse(txt_Quantita.Text);

            ViewState["listaDatiArticoli"] = ListaDatiArticoli.OrderByDescending(x => x.Prezzo).ToList();

            ResetPanelOfferta();

            RichiediOperazionePopup("UPDATE");
        }

        protected void gvGruppi_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            long idSelezione = Convert.ToInt64(e.CommandArgument);

            //listaDatiArticoli = (List<DatiArticoli>)ViewState["listaDatiArticoli"];
            if (ListaDatiArticoli == null)
            {
                ListaDatiArticoli = new List<DatiArticoli>();
            }
            ArticoliGruppi articoloGruppo = ListaArticoliGruppi.FirstOrDefault(X => X.Id == idSelezione);

            if (articoloGruppo.Isgruppo)
            {
                aggiungiArticoliDelGruppoAListaArticoli(articoloGruppo.IdOggetto);
            }
            else
            {
                aggiungiArticoloAListaArticoli(articoloGruppo.IdOggetto);
            }

            //ViewState["listaDatiArticoli"] = listaDatiArticoli;

            if (ListaDatiArticoli != null && ListaDatiArticoli.Count > 0)
            {
                lbl_selezionareArticolo.Visible = false;
                gvArticoli.DataSource = ListaDatiArticoli;
                gvArticoli.DataBind();

                AggiornaTotali();

                ResetPanelOfferta();

                RichiediOperazionePopup("UPDATE");
            }
        }

        protected void gvArticoli_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DatiArticoli articoloSelezionato;

            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            int id = Convert.ToInt32(commandArgs[0]);

            if (id == 0)
            {
                long identificatoreOggetto = Convert.ToInt64(commandArgs[1]);
                ViewState["identificatoreArticolo"] = identificatoreOggetto;
                articoloSelezionato = ListaDatiArticoli.FirstOrDefault(x => x.IdentificatoreOggetto == identificatoreOggetto);

            }
            else
            {
                ViewState["idArticolo"] = id;
                articoloSelezionato = ListaDatiArticoli.FirstOrDefault(x => x.Id == id);
            }

            int indexArticolo = ListaDatiArticoli.IndexOf(articoloSelezionato);

            switch (e.CommandName)
            {
                case "modifica":
                    txt_Descrizione.Text = articoloSelezionato.Descrizione;
                    txt_DescrizioneLunga.Text = articoloSelezionato.DescrizioneLunga;
                    txt_Costo.Text = articoloSelezionato.Costo.ToString();
                    txt_Prezzo.Text = articoloSelezionato.Prezzo.ToString();
                    txt_Iva.Text = articoloSelezionato.Iva.ToString();
                    txt_Quantita.Text = articoloSelezionato.Quantita.ToString();
                    
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriModificaArticolo", script: "javascript: document.getElementById('" + panelModificaArticolo.ClientID + "').style.display='block'", addScriptTags: true);

                    break;
                case "elimina":
                    ListaDatiArticoli.Remove(articoloSelezionato);
                    gvArticoli.DataSource = ListaDatiArticoli;
                    gvArticoli.DataBind();

                    AggiornaTotali();
                    ResetPanelOfferta();

                    break;
                case "moveUp":
                    if (indexArticolo > 0)
                    {
                        ListaDatiArticoli.Remove(articoloSelezionato);
                        ListaDatiArticoli.Insert(indexArticolo - 1, articoloSelezionato);
                        gvArticoli.DataSource = ListaDatiArticoli;
                        gvArticoli.DataBind();
                    }
                    break;
                case "moveDown":
                    if (indexArticolo < ListaDatiArticoli.Count-1)
                    {
                        ListaDatiArticoli.Remove(articoloSelezionato);
                        ListaDatiArticoli.Insert(indexArticolo + 1, articoloSelezionato);
                        gvArticoli.DataSource = ListaDatiArticoli;
                        gvArticoli.DataBind();
                    }
                    break;
            }

            RichiediOperazionePopup("UPDATE");
        }

        protected void btnRecuperaOfferta_Click(object sender, EventArgs e)
        {
            panelModificaArticolo.Style.Add("display", "none");
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriModificaArticolo", script: "javascript: document.getElementById('" + panelRecuperaOfferta.ClientID + "').style.display='block'", addScriptTags: true);
            RichiediOperazionePopup("UPDATE");
        }

        protected void btnEliminaArticoli_Click(object sender, EventArgs e)
        {
            ListaDatiArticoli = null;

            ResetPanelOfferta();

            RichiediOperazionePopup("UPDATE");
        }
        #endregion

        #region OPERAZIONI OFFERTA
        private void AggiornaTotali()
        {
            decimal totPrezzo = 0;
            decimal totCosto = 0;
            decimal totIva = 0;
            decimal percRicavo = 0;

            if (ListaDatiArticoli != null && ListaDatiArticoli.Count > 0)
            {
                foreach (DatiArticoli art in ListaDatiArticoli)
                {
                    totPrezzo += art.Prezzo * art.Quantita;
                    totCosto += art.Costo * art.Quantita;
                    totIva += (art.Prezzo * art.Iva / 100) * art.Quantita;
                }

                if (totPrezzo != 0)
                {
                    percRicavo = ((totPrezzo - totCosto) / totPrezzo) * 100;
                }
            }

            txt_TotPrezzo.Text = string.Format("{0:N2}", totPrezzo);
            txt_TotCosto.Text = string.Format("{0:N2}", totCosto);
            txt_TotIva.Text = string.Format("{0:N2}", totIva);
            txt_PercRicavo.Text = string.Format("{0:N2}", percRicavo);
        }

        public void ClearOfferta()
        {
            ClearModificaArticoli();
            lbl_selezionareArticolo.Visible = true;
            ViewState["listaDatiArticoli"] = null;
            gvArticoli.DataSource = null;
            gvArticoli.DataBind();

            txt_TotPrezzo.Text = "";
            txt_TotCosto.Text = "";
            txt_TotIva.Text = "";
            txt_PercRicavo.Text = "";

            ResetPanelOfferta();
        }

        private void ClearModificaArticoli()
        {
            ViewState["identificatoreArticolo"] = null;
            txt_Descrizione.Text = "";
            txt_DescrizioneLunga.Text = "";
            txt_Costo.Text = "";
            txt_Prezzo.Text = "";
            txt_Iva.Text = "";
            txt_Quantita.Text = "";
        }

        private void ResetPanelOfferta()
        {
            ClearModificaArticoli();

            gvArticoli.DataSource = ListaDatiArticoli;
            gvArticoli.DataBind();

            btnRecuperaOfferta.Visible = true;
            btnEliminaArticoli.Visible = (ListaDatiArticoli != null && ListaDatiArticoli.Count > 0);
            lbl_selezionareArticolo.Visible = (ListaDatiArticoli == null || ListaDatiArticoli.Count == 0);

            AggiornaTotali();
        }

        public void PopolaOfferta(int idDatiAgenda)
        {
            Esito esito = new Esito();

            ListaDatiArticoli = Articoli_BLL.Instance.CaricaListaArticoliByIDEvento(idDatiAgenda, ref esito);
            if (ListaDatiArticoli != null && ListaDatiArticoli.Count > 0)
            {
                lbl_selezionareArticolo.Visible = false;
                gvArticoli.DataSource = ListaDatiArticoli;
                gvArticoli.DataBind();

                AggiornaTotali();
            }
            ResetPanelOfferta();
        }

        private void aggiungiArticoliDelGruppoAListaArticoli(int idGruppo)
        {
            Esito esito = new Esito();
            int idEvento = 0;
            ListaDatiArticoli.AddRange(Articoli_BLL.Instance.CaricaListaArticoliByIDGruppo(idEvento, idGruppo, ref esito));
            ListaDatiArticoli = ListaDatiArticoli.OrderByDescending(x => x.Prezzo).ToList();
        }

        private void aggiungiArticoloAListaArticoli(int idArticolo)
        {
            Esito esito = new Esito();
            int idEvento = 0;
            ListaDatiArticoli.Add(Articoli_BLL.Instance.CaricaArticoloByID(idEvento, idArticolo, ref esito));
            ListaDatiArticoli = ListaDatiArticoli.OrderByDescending(x => x.Prezzo).ToList();
        }

        public void AbilitaComponentiPopup(int statoEvento)
        {
            panelOfferta.Enabled = basePage.AbilitazioneInScrittura();

            if (basePage.AbilitazioneInScrittura())
            {
                if (statoEvento == Stato.Instance.STATO_PREVISIONE_IMPEGNO)
                {
                    panelOfferta.Enabled  = false;
                }
                else if (statoEvento == Stato.Instance.STATO_OFFERTA)
                {
                    panelOfferta.Enabled = true;
                }
                else if (statoEvento == Stato.Instance.STATO_LAVORAZIONE)
                {
                    panelOfferta.Enabled = false;
                }
                else if (statoEvento == Stato.Instance.STATO_FATTURA)
                {
                    panelOfferta.Enabled = false;
                }
                else if (statoEvento == Stato.Instance.STATO_RIPOSO)
                {
                    panelOfferta.Enabled = false;
                }
            }
        }
        #endregion
    }
}