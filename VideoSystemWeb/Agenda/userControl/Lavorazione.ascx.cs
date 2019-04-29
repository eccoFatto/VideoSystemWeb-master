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
    public partial class Lavorazione : System.Web.UI.UserControl
    {
        public delegate void PopupHandler(string operazionePopup); // delegato per l'evento
        public event PopupHandler RichiediOperazionePopup; //evento

        public List<DatiArticoli> listaDatiArticoli
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

        List<ArticoliGruppi> listaArticoliGruppi
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
                gvGruppi.DataSource = listaArticoliGruppi;
                gvGruppi.DataBind();
            }
            else
            {
                // SELEZIONO L'ULTIMA TAB SELEZIONATA
                ScriptManager.RegisterStartupScript(Page, GetType(), "apriTabGiusta", script: "openTabEventoLavorazione(event,'" + hf_tabSelezionataLavorazione.Value + "')", addScriptTags: true);
            }
        }

        #region COMPORTAMENTO ELEMENTI PAGINA

        #endregion

        protected void gvGruppi_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            long idSelezione = Convert.ToInt64(e.CommandArgument);

            listaDatiArticoli = (List<DatiArticoli>)ViewState["listaDatiArticoli"];
            if (listaDatiArticoli == null)
            {
                listaDatiArticoli = new List<DatiArticoli>();
            }
            ArticoliGruppi articoloGruppo = listaArticoliGruppi.FirstOrDefault(X => X.Id == idSelezione);

            if (articoloGruppo.Isgruppo)
            {
                aggiungiArticoliDelGruppoAListaArticoli(articoloGruppo.IdOggetto);
            }
            else
            {
                aggiungiArticoloAListaArticoli(articoloGruppo.IdOggetto);
            }

            ViewState["listaDatiArticoli"] = listaDatiArticoli;

            if (listaDatiArticoli != null && listaDatiArticoli.Count > 0)
            {
                lbl_selezionareArticolo.Visible = false;
                gvArticoli.DataSource = listaDatiArticoli;
                gvArticoli.DataBind();

                AggiornaTotali();

                ResetPanelLavorazione();

                RichiediOperazionePopup("UPDATE");
            }
        }

        private void aggiungiArticoliDelGruppoAListaArticoli(int idGruppo)
        {
            Esito esito = new Esito();
            int idEvento = 0;
            listaDatiArticoli.AddRange(Articoli_BLL.Instance.CaricaListaArticoliByIDGruppo(idEvento, idGruppo, ref esito));
            listaDatiArticoli = listaDatiArticoli.OrderByDescending(x => x.Prezzo).ToList();
        }

        private void aggiungiArticoloAListaArticoli(int idArticolo)
        {
            Esito esito = new Esito();
            int idEvento = 0;
            listaDatiArticoli.Add(Articoli_BLL.Instance.CaricaArticoloByID(idEvento, idArticolo, ref esito));
            listaDatiArticoli = listaDatiArticoli.OrderByDescending(x => x.Prezzo).ToList();
        }

        private void AggiornaTotali()
        {
            decimal totPrezzo = 0;
            decimal totCosto = 0;
            decimal totIva = 0;
            decimal percRicavo = 0;

            if (listaDatiArticoli != null && listaDatiArticoli.Count > 0)
            {
                foreach (DatiArticoli art in listaDatiArticoli)
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

        private void ResetPanelLavorazione()
        {
           // ClearModificaArticoli();

            gvArticoli.DataSource = listaDatiArticoli;
            gvArticoli.DataBind();

            lbl_selezionareArticolo.Visible = (listaDatiArticoli == null || listaDatiArticoli.Count == 0);

            AggiornaTotali();
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
                articoloSelezionato = listaDatiArticoli.FirstOrDefault(x => x.IdentificatoreOggetto == identificatoreOggetto);

            }
            else
            {
                ViewState["idArticolo"] = id;
                articoloSelezionato = listaDatiArticoli.FirstOrDefault(x => x.Id == id);
            }

            int indexArticolo = listaDatiArticoli.IndexOf(articoloSelezionato);

            switch (e.CommandName)
            {
                case "modifica":
                    //txt_Descrizione.Text = articoloSelezionato.Descrizione;
                    //txt_DescrizioneLunga.Text = articoloSelezionato.DescrizioneLunga;
                    //txt_Costo.Text = articoloSelezionato.Costo.ToString();
                    //txt_Prezzo.Text = articoloSelezionato.Prezzo.ToString();
                    //txt_Iva.Text = articoloSelezionato.Iva.ToString();
                    //txt_Quantita.Text = articoloSelezionato.Quantita.ToString();

                    //ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriModificaArticolo", script: "javascript: document.getElementById('" + panelModificaArticolo.ClientID + "').style.display='block'", addScriptTags: true);

                    break;
                case "elimina":
                    listaDatiArticoli.Remove(articoloSelezionato);
                    gvArticoli.DataSource = listaDatiArticoli;
                    gvArticoli.DataBind();

                    AggiornaTotali();
                    ResetPanelLavorazione();

                    break;
                case "moveUp":
                    if (indexArticolo > 0)
                    {
                        listaDatiArticoli.Remove(articoloSelezionato);
                        listaDatiArticoli.Insert(indexArticolo - 1, articoloSelezionato);
                        gvArticoli.DataSource = listaDatiArticoli;
                        gvArticoli.DataBind();
                    }
                    break;
                case "moveDown":
                    if (indexArticolo < listaDatiArticoli.Count - 1)
                    {
                        listaDatiArticoli.Remove(articoloSelezionato);
                        listaDatiArticoli.Insert(indexArticolo + 1, articoloSelezionato);
                        gvArticoli.DataSource = listaDatiArticoli;
                        gvArticoli.DataBind();
                    }
                    break;
            }

            RichiediOperazionePopup("UPDATE");
        }

        public void PopolaLavorazione(int idDatiAgenda)
        {
            Esito esito = new Esito();
            listaDatiArticoli = Articoli_BLL.Instance.CaricaListaArticoliByIDEvento(idDatiAgenda, ref esito);
            if (listaDatiArticoli != null && listaDatiArticoli.Count > 0)
            {
                lbl_selezionareArticolo.Visible = false;
                gvArticoli.DataSource = listaDatiArticoli;
                gvArticoli.DataBind();

                AggiornaTotali();
            }
            ResetPanelLavorazione();
        }

        public void ClearLavorazione()
        {
            //ClearModificaArticoli();
            lbl_selezionareArticolo.Visible = true;
            ViewState["listaDatiArticoli"] = null;
            gvArticoli.DataSource = null;
            gvArticoli.DataBind();

            txt_TotPrezzo.Text = "";
            txt_TotCosto.Text = "";
            txt_TotIva.Text = "";
            txt_PercRicavo.Text = "";

            ResetPanelLavorazione();
        }

        protected void btnImporta_Click(object sender, EventArgs e)
        {
           
        }
        
        protected void btn_SwitchArtPers_Click(object sender, EventArgs e)
        {
            if (btn_SwitchArtPers.Text == "Inserimento articoli")
            {
                btn_SwitchArtPers.Text = "Inserimento personale/fornitore";
            }
            else
            {
                btn_SwitchArtPers.Text = "Inserimento articoli";
            }

            RichiediOperazionePopup("UPDATE");
        }
    }
}