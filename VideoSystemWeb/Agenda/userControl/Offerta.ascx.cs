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
            DatiArticoli articoloSelezionato;

            if (ViewState["idArticolo"]==null)
            {
                long identificatoreOggetto = (long)ViewState["identificatoreArticolo"];
                articoloSelezionato = SessionManager.EventoSelezionato.ListaDatiArticoli.FirstOrDefault(x => x.IdentificatoreOggetto == identificatoreOggetto);
            }
            else
            {
                int idArticolo = (int)ViewState["idArticolo"];
                articoloSelezionato = SessionManager.EventoSelezionato.ListaDatiArticoli.FirstOrDefault(x => x.Id == idArticolo);
            }
            
            var index = SessionManager.EventoSelezionato.ListaDatiArticoli.IndexOf(articoloSelezionato);

            articoloSelezionato.Descrizione = txt_Descrizione.Text;
            articoloSelezionato.DescrizioneLunga = txt_DescrizioneLunga.Text;
            articoloSelezionato.Costo = decimal.Parse(txt_Costo.Text);
            articoloSelezionato.Prezzo = decimal.Parse(txt_Prezzo.Text);
            articoloSelezionato.Iva = int.Parse(txt_Iva.Text);
            articoloSelezionato.Stampa = ddl_Stampa.SelectedValue == "1";
            articoloSelezionato.Quantita = int.Parse(txt_Quantita.Text);

            SessionManager.EventoSelezionato.ListaDatiArticoli = SessionManager.EventoSelezionato.ListaDatiArticoli.OrderByDescending(x => x.Prezzo).ToList();

            ResetPanelOfferta();

            RichiediOperazionePopup("UPDATE");
        }

        protected void gvGruppi_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            long idSelezione = Convert.ToInt64(e.CommandArgument);

            if (SessionManager.EventoSelezionato.ListaDatiArticoli == null)
            {
                SessionManager.EventoSelezionato.ListaDatiArticoli = new List<DatiArticoli>();
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

            if (SessionManager.EventoSelezionato.ListaDatiArticoli != null && SessionManager.EventoSelezionato.ListaDatiArticoli.Count > 0)
            {
                lbl_selezionareArticolo.Visible = false;
                gvArticoli.DataSource = SessionManager.EventoSelezionato.ListaDatiArticoli;
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
                articoloSelezionato = SessionManager.EventoSelezionato.ListaDatiArticoli.FirstOrDefault(x => x.IdentificatoreOggetto == identificatoreOggetto);

            }
            else
            {
                ViewState["idArticolo"] = id;
                articoloSelezionato = SessionManager.EventoSelezionato.ListaDatiArticoli.FirstOrDefault(x => x.Id == id);
            }

            int indexArticolo = SessionManager.EventoSelezionato.ListaDatiArticoli.IndexOf(articoloSelezionato);

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
                    SessionManager.EventoSelezionato.ListaDatiArticoli.Remove(articoloSelezionato);
                    gvArticoli.DataSource = SessionManager.EventoSelezionato.ListaDatiArticoli;
                    gvArticoli.DataBind();

                    AggiornaTotali();
                    ResetPanelOfferta();

                    break;
                case "moveUp":
                    if (indexArticolo > 0)
                    {
                        SessionManager.EventoSelezionato.ListaDatiArticoli.Remove(articoloSelezionato);
                        SessionManager.EventoSelezionato.ListaDatiArticoli.Insert(indexArticolo - 1, articoloSelezionato);
                        gvArticoli.DataSource = SessionManager.EventoSelezionato.ListaDatiArticoli;
                        gvArticoli.DataBind();
                    }
                    break;
                case "moveDown":
                    if (indexArticolo < SessionManager.EventoSelezionato.ListaDatiArticoli.Count-1)
                    {
                        SessionManager.EventoSelezionato.ListaDatiArticoli.Remove(articoloSelezionato);
                        SessionManager.EventoSelezionato.ListaDatiArticoli.Insert(indexArticolo + 1, articoloSelezionato);
                        gvArticoli.DataSource = SessionManager.EventoSelezionato.ListaDatiArticoli;
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
            SessionManager.EventoSelezionato.ListaDatiArticoli = null;

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

            if (SessionManager.EventoSelezionato.ListaDatiArticoli != null && SessionManager.EventoSelezionato.ListaDatiArticoli.Count > 0)
            {
                foreach (DatiArticoli art in SessionManager.EventoSelezionato.ListaDatiArticoli)
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
            SessionManager.EventoSelezionato.ListaDatiArticoli = null;

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

            gvArticoli.DataSource = SessionManager.EventoSelezionato.ListaDatiArticoli;
            gvArticoli.DataBind();

            btnRecuperaOfferta.Visible = true;
            btnEliminaArticoli.Visible = (SessionManager.EventoSelezionato.ListaDatiArticoli != null && SessionManager.EventoSelezionato.ListaDatiArticoli.Count > 0);
            lbl_selezionareArticolo.Visible = (SessionManager.EventoSelezionato.ListaDatiArticoli == null || SessionManager.EventoSelezionato.ListaDatiArticoli.Count == 0);

            AggiornaTotali();
        }

        public void PopolaOfferta()
        {
            Esito esito = new Esito();
            int idDatiAgenda = SessionManager.EventoSelezionato.id;

            SessionManager.EventoSelezionato.ListaDatiArticoli = Articoli_BLL.Instance.CaricaListaArticoliByIDEvento(idDatiAgenda, ref esito);
            if (SessionManager.EventoSelezionato.ListaDatiArticoli != null && SessionManager.EventoSelezionato.ListaDatiArticoli.Count > 0)
            {
                lbl_selezionareArticolo.Visible = false;
                gvArticoli.DataSource = SessionManager.EventoSelezionato.ListaDatiArticoli;
                gvArticoli.DataBind();

                AggiornaTotali();
            }
            ResetPanelOfferta();
        }

        private void aggiungiArticoliDelGruppoAListaArticoli(int idGruppo)
        {
            Esito esito = new Esito();
            int idEvento = 0;
            SessionManager.EventoSelezionato.ListaDatiArticoli.AddRange(Articoli_BLL.Instance.CaricaListaArticoliByIDGruppo(idEvento, idGruppo, ref esito));
            SessionManager.EventoSelezionato.ListaDatiArticoli = SessionManager.EventoSelezionato.ListaDatiArticoli.OrderByDescending(x => x.Prezzo).ToList();
        }

        private void aggiungiArticoloAListaArticoli(int idArticolo)
        {
            Esito esito = new Esito();
            int idEvento = 0;
            SessionManager.EventoSelezionato.ListaDatiArticoli.Add(Articoli_BLL.Instance.CaricaArticoloByID(idEvento, idArticolo, ref esito));
            SessionManager.EventoSelezionato.ListaDatiArticoli = SessionManager.EventoSelezionato.ListaDatiArticoli.OrderByDescending(x => x.Prezzo).ToList();
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