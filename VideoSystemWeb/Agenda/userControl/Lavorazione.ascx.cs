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
        BasePage basePage = new BasePage();

        List<DatiArticoliLavorazione> listaDatiArticoliLavorazione
        {
            get
            {
                if (ViewState["listaDatiArticoliLavorazione"] == null || ((List<DatiArticoliLavorazione>)ViewState["listaDatiArticoliLavorazione"]).Count() == 0)
                {
                    return new List<DatiArticoliLavorazione>();
                }
                return (List<DatiArticoliLavorazione>)ViewState["listaDatiArticoliLavorazione"];
            }
            set
            {
                ViewState["listaDatiArticoliLavorazione"] = value;
            }
        }
        DatiLavorazione lavorazioneCorrente
        {
            get
            {
                if (ViewState["lavorazioneCorrente"] == null)
                {
                    return new DatiLavorazione();
                }
                return (DatiLavorazione)ViewState["lavorazioneCorrente"];
            }
            set
            {
                ViewState["lavorazioneCorrente"] = value;
            }
        }
        List<ArticoliGruppi> listaArticoliGruppiLavorazione
        {
            get
            {
                if (ViewState["listaArticoliGruppiLavorazione"] == null || ((List<ArticoliGruppi>)ViewState["listaArticoliGruppiLavorazione"]).Count == 0)
                {
                    ViewState["listaArticoliGruppiLavorazione"] = Articoli_BLL.Instance.CaricaListaArticoliGruppi();
                }
                return (List<ArticoliGruppi>)ViewState["listaArticoliGruppiLavorazione"];
            }
            set
            {
                ViewState["listaArticoliGruppiLavorazione"] = value;
            }
        }
        //List<FiguraProfessionale> listaCompletaFigProf
        //{
        //    get
        //    {
        //        if (ViewState["listaCompletaFigProf"] == null || ((List<FiguraProfessionale>)ViewState["listaCompletaFigProf"]).Count() == 0)
        //        {
        //            List<FiguraProfessionale> _listaCompletaFigProf = new List<FiguraProfessionale>();
        //            foreach (Anag_Collaboratori collaboratore in listaAnagraficheCollaboratori)
        //            {
        //                _listaCompletaFigProf.Add(new FiguraProfessionale()
        //                {
        //                    Id = collaboratore.Id,
        //                    Nome = collaboratore.Nome,
        //                    Cognome = collaboratore.Cognome,
        //                    Citta = collaboratore.ComuneRiferimento.Trim().ToLower(),
        //                    Telefono = collaboratore.Telefoni.Count == 0 ? "" : collaboratore.Telefoni.FirstOrDefault(x => x.Priorita == 1).Pref_naz + collaboratore.Telefoni.FirstOrDefault(x => x.Priorita == 1).Numero,
        //                    Qualifiche = collaboratore.Qualifiche,
        //                    Tipo = 0,
        //                    Nota = collaboratore.Note
        //                });
        //            }

        //            foreach (Anag_Clienti_Fornitori fornitore in listaAnagraficheFornitori)
        //            {
        //                _listaCompletaFigProf.Add(new FiguraProfessionale()
        //                {
        //                    Id = fornitore.Id,
        //                    Cognome = fornitore.RagioneSociale,
        //                    Citta = fornitore.ComuneLegale.Trim().ToLower(),
        //                    Telefono = fornitore.Telefono,
        //                    Tipo = 1,
        //                    Nota = fornitore.Note
        //                });
        //            }
        //            ViewState["listaCompletaFigProf"] = _listaCompletaFigProf.OrderBy(x => x.Cognome).ToList<FiguraProfessionale>();

        //            //return new List<FiguraProfessionale>();
        //        }
        //        return (List<FiguraProfessionale>)ViewState["listaCompletaFigProf"];
        //    }

        //    set
        //    {
        //        ViewState["listaCompletaFigProf"] = value;
        //    }
        //}
        //List<Anag_Qualifiche_Collaboratori> listaQualificheCollaboratori
        //{
        //    get
        //    {
        //        if (ViewState["listaQualificheCollaboratori"] == null || ((List<Anag_Qualifiche_Collaboratori>)ViewState["listaQualificheCollaboratori"]).Count() == 0)
        //        {
        //            Esito esito = new Esito();
        //            ViewState["listaQualificheCollaboratori"] = Anag_Qualifiche_Collaboratori_BLL.Instance.getAllQualifiche(ref esito, true);
        //        }
        //        return (List<Anag_Qualifiche_Collaboratori>)ViewState["listaQualificheCollaboratori"];
        //    }

        //    set
        //    {
        //        ViewState["listaQualificheCollaboratori"] = value;
        //    }
        //}
        //List<Anag_Collaboratori> listaAnagraficheCollaboratori
        //{
        //    get
        //    {
        //        if (ViewState["listaAnagraficheCollaboratori"] == null || ((List<Anag_Collaboratori>)ViewState["listaAnagraficheCollaboratori"]).Count() == 0)
        //        {
        //            Esito esito = new Esito();
        //            ViewState["listaAnagraficheCollaboratori"] = Anag_Collaboratori_BLL.Instance.getAllCollaboratori(ref esito);
        //            //return new List<Anag_Collaboratori>();
        //        }
        //        return (List<Anag_Collaboratori>)ViewState["listaAnagraficheCollaboratori"];
        //    }

        //    set
        //    {
        //        ViewState["listaAnagraficheCollaboratori"] = value;
        //    }
        //}
        //List<Anag_Clienti_Fornitori> listaAnagraficheFornitori
        //{
        //    get
        //    {
        //        if (ViewState["listaAnagraficheFornitori"] == null || ((List<Anag_Clienti_Fornitori>)ViewState["listaAnagraficheFornitori"]).Count() == 0)
        //        {
        //            Esito esito = new Esito();
        //            ViewState["listaAnagraficheFornitori"] = Anag_Clienti_Fornitori_BLL.Instance.CaricaListaFornitori(ref esito);
        //            //return new List<Anag_Clienti_Fornitori>();
        //        }
        //        return (List<Anag_Clienti_Fornitori>)ViewState["listaAnagraficheFornitori"];
        //    }

        //    set
        //    {
        //        ViewState["listaAnagraficheFornitori"] = value;
        //    }
        //}
        //List<Tipologica> listaTipiPagamento
        //{
        //    get
        //    {
        //        if (ViewState["listaTipiPagamento"] == null || ((List<Tipologica>)ViewState["listaTipiPagamento"]).Count() == 0)
        //        {
        //            ViewState["listaTipiPagamento"] = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_PAGAMENTO);
        //        }
        //        return (List<Tipologica>)ViewState["listaTipiPagamento"];
        //    }
        //    set
        //    {
        //        ViewState["listaTipiPagamento"] = value;
        //    }
        //}
        //List<string> listaCittaCollaboratori
        //{
        //    get
        //    {
        //        if (ViewState["listaCittaCollaboratori"] == null || ((List<string>)ViewState["listaCittaCollaboratori"]).Count() == 0)
        //        {
        //            ViewState["listaCittaCollaboratori"] = (from record in listaAnagraficheCollaboratori select record.ComuneRiferimento.Trim().ToLower()).ToList();
        //        }
        //        return (List<string>)ViewState["listaCittaCollaboratori"];
        //    }
        //    set
        //    {
        //        ViewState["listaCittaCollaboratori"] = value;
        //    }
        //}
        //List<string> listaCittaFornitori
        //{
        //    get
        //    {
        //        if (ViewState["listaCittaFornitori"] == null || ((List<string>)ViewState["listaCittaFornitori"]).Count() == 0)
        //        {
        //            ViewState["listaCittaFornitori"] = (from record in listaAnagraficheFornitori select record.ComuneLegale.Trim().ToLower()).ToList();
        //        }
        //        return (List<string>)ViewState["listaCittaFornitori"];
        //    }
        //    set
        //    {
        //        ViewState["listaCittaFornitori"] = value;
        //    }
        //}
        //List<Anag_Referente_Clienti_Fornitori> listaReferenti
        //{
        //    get
        //    {
        //        if (ViewState["listaReferenti"] == null || ((List<Anag_Referente_Clienti_Fornitori>)ViewState["listaReferenti"]).Count() == 0)
        //        {
        //            ViewState["listaReferenti"] = new List<Anag_Referente_Clienti_Fornitori>();
        //        }
        //        return (List<Anag_Referente_Clienti_Fornitori>)ViewState["listaReferenti"];
        //    }
        //    set
        //    {
        //        ViewState["listaReferenti"] = value;
        //    }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                gvGruppi.DataSource = listaArticoliGruppiLavorazione;
                gvGruppi.DataBind();

                PopolaCombo();
            }
            else
            {
                // SELEZIONO L'ULTIMA TAB SELEZIONATA
                ScriptManager.RegisterStartupScript(Page, GetType(), "apriTabGiusta", script: "openTabEventoLavorazione(event,'" + hf_tabSelezionataLavorazione.Value + "')", addScriptTags: true);
            }

            //DatiAgenda eventoSelezionato = (DatiAgenda)ViewState["eventoSelezionato"];
            //if (eventoSelezionato != null)
            //{
            //    Esito esito = new Esito();
            //    List<Anag_Referente_Clienti_Fornitori> listaReferenti = Anag_Referente_Clienti_Fornitori_BLL.Instance.getReferentiByIdAzienda(ref esito, eventoSelezionato.id_cliente);
            //    foreach (Anag_Referente_Clienti_Fornitori referente in listaReferenti)
            //    {
            //        ddl_Referente.Items.Add(new ListItem(referente.Nome + " " + referente.Cognome, referente.Id.ToString()));
            //    }
            //    ddl_Referente.Items.Insert(0, new ListItem("<seleziona>", "0"));
            //}
        }

        #region COMPORTAMENTO ELEMENTI PAGINA
        private void AbilitaComponentiModificaArticolo(int tipoFiguraProfessionale)
        {
            if (tipoFiguraProfessionale == 0)//COLLABORATORE
            {
                ddl_FPqualifica.CssClass = "w3-input w3-border";
                ddl_FPqualifica.Style.Remove("cursor");
                ddl_FPqualifica.Enabled = true;

                ddl_FPtipoPagamento.CssClass = "w3-input w3-border";
                ddl_FPtipoPagamento.Style.Remove("cursor");
                ddl_FPtipoPagamento.Enabled = true;
            }
            else//FORNITORE
            {
                ddl_FPqualifica.SelectedValue = "";
                ddl_FPqualifica.CssClass = "w3-input w3-border";
                ddl_FPqualifica.Style.Add("cursor", "not-allowed;");
                ddl_FPqualifica.Enabled = false;

                ddl_FPtipoPagamento.SelectedValue = "";
                ddl_FPtipoPagamento.CssClass = "w3-input w3-border";
                ddl_FPtipoPagamento.Style.Add("cursor", "not-allowed;");
                ddl_FPtipoPagamento.Enabled = false;
            }
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

        protected void gvGruppi_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            long idSelezione = Convert.ToInt64(e.CommandArgument);

            //listaDatiArticoliLavorazione = (List<DatiArticoliLavorazione>)ViewState["listaDatiArticoliLavorazione"];
            //if (listaDatiArticoliLavorazione == null)
            //{
            //    listaDatiArticoliLavorazione = new List<DatiArticoliLavorazione>();
            //}

            ArticoliGruppi articoloGruppo = listaArticoliGruppiLavorazione.FirstOrDefault(X => X.Id == idSelezione);

            if (articoloGruppo.Isgruppo)
            {
                aggiungiArticoliDelGruppoAListaArticoli(articoloGruppo.IdOggetto);
            }
            else
            {
                aggiungiArticoloAListaArticoli(articoloGruppo.IdOggetto);
            }

            //ViewState["listaDatiArticoliLavorazione"] = listaDatiArticoliLavorazione;

            if (listaDatiArticoliLavorazione != null && listaDatiArticoliLavorazione.Count > 0)
            {
                lbl_selezionareArticolo.Visible = false;
                gvArticoliLavorazione.DataSource = listaDatiArticoliLavorazione;
                gvArticoliLavorazione.DataBind();

                AggiornaTotali();

                ResetPanelLavorazione();

                RichiediOperazionePopup("UPDATE");
            }
        }

        protected void gvArticoliLavorazione_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DatiArticoliLavorazione articoloSelezionato;

            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            int id = Convert.ToInt32(commandArgs[0]);

            if (id == 0)
            {
                long identificatoreOggetto = Convert.ToInt64(commandArgs[1]);
                ViewState["identificatoreArticolo"] = identificatoreOggetto;
                articoloSelezionato = listaDatiArticoliLavorazione.FirstOrDefault(x => x.IdentificatoreOggetto == identificatoreOggetto);

            }
            else
            {
                ViewState["idArticolo"] = id;
                articoloSelezionato = listaDatiArticoliLavorazione.FirstOrDefault(x => x.Id == id);
            }

            int indexArticolo = listaDatiArticoliLavorazione.IndexOf(articoloSelezionato);

            switch (e.CommandName)
            {
                case "modifica":
                    txt_Descrizione.Text = articoloSelezionato.Descrizione;
                    txt_DescrizioneLunga.Text = articoloSelezionato.DescrizioneLunga;
                    txt_Costo.Text = articoloSelezionato.Costo.ToString();
                    txt_Prezzo.Text = articoloSelezionato.Prezzo.ToString();
                    txt_Iva.Text = articoloSelezionato.Iva.ToString();
                    ddl_FPtipoPagamento.SelectedValue = articoloSelezionato.IdTipoPagamento != null ? articoloSelezionato.IdTipoPagamento.ToString() : "";
                    //chk_ModCosto.Checked = articoloSelezionato.UsaCostoFP != null && (bool)articoloSelezionato.UsaCostoFP;
                    
                    //Cerco tra Collaboratori o Fornitori
                    FiguraProfessionale figuraProfessionale = SessionManager.listaCompletaFigProf.FirstOrDefault(x => x.Id == articoloSelezionato.IdCollaboratori);
                    if (figuraProfessionale == null)
                    {
                        figuraProfessionale = SessionManager.listaCompletaFigProf.FirstOrDefault(x => x.Id == articoloSelezionato.IdFornitori);
                    }

                    //Valori de Default
                    chk_ModCosto.Checked = false;

                    txt_Costo.Attributes.Remove("readonly");
                    txt_Costo.CssClass = "w3-input w3-border";

                    txt_FPnetto.Attributes.Add("readonly", "readonly");
                    txt_FPnetto.CssClass = "w3-input w3-border w3-disabled";

                    txt_FPnetto.Text = "";
                    txt_FPlordo.Text = "";

                    ddl_FPcitta.Items.Clear();
                    //

                    if (figuraProfessionale != null)
                    {
                        AbilitaComponentiModificaArticolo(figuraProfessionale.Tipo);

                        ddl_FPcitta.Items.Clear();
                        if (figuraProfessionale.Tipo == 0)
                        {
                            SessionManager.listaCittaCollaboratori.Sort();
                            foreach (string citta in SessionManager.listaCittaCollaboratori)
                            {
                                ddl_FPcitta.Items.Add(new ListItem(citta.ToUpper(), citta));
                            }
                        }
                        else
                        {
                            SessionManager.listaCittaFornitori.Sort();
                            foreach (string citta in SessionManager.listaCittaFornitori)
                            {
                                ddl_FPcitta.Items.Add(new ListItem(citta.ToUpper(), citta));
                            }
                        }
                        ddl_FPcitta.Items.Insert(0, new ListItem("<seleziona>", ""));

                        PopolaNominativi(SessionManager.listaCompletaFigProf.Where(x => x.Tipo == figuraProfessionale.Tipo).ToList());
                        ddl_FPtipo.SelectedValue = figuraProfessionale.Tipo.ToString();
                        txt_FPnotaCollaboratore.Text = articoloSelezionato.Nota;
                        ddl_FPnominativo.SelectedValue = figuraProfessionale.Id.ToString();
                        txt_FPtelefono.Text = figuraProfessionale.Telefono;

                        // GESTIONE COSTO FIG PROF
                        if (articoloSelezionato.UsaCostoFP != null && (bool)articoloSelezionato.UsaCostoFP)
                        {
                            chk_ModCosto.Checked = true;
                            txt_Costo.Attributes.Add("readonly", "readonly");
                            txt_Costo.CssClass = "w3-input w3-border w3-disabled";

                            txt_FPnetto.Text = articoloSelezionato.FP_netto.ToString();
                            txt_FPlordo.Text = articoloSelezionato.FP_lordo.ToString();
                            //txt_FPnetto.ReadOnly = false;
                            txt_FPnetto.Attributes.Remove("readonly");
                            txt_FPnetto.CssClass = "w3-input w3-border";
                        }
                    }
                    else
                    {
                        int tipoCollaboratore = 0;

                        SessionManager.listaCittaCollaboratori.Sort();
                        foreach (string citta in SessionManager.listaCittaCollaboratori)
                        {
                            ddl_FPcitta.Items.Add(new ListItem(citta.ToUpper(), citta));
                        }
                        ddl_FPcitta.Items.Insert(0, new ListItem("<seleziona>", ""));


                        PopolaNominativi(SessionManager.listaCompletaFigProf.Where(x => x.Tipo == tipoCollaboratore).ToList());

                        ddl_FPtipo.SelectedValue = tipoCollaboratore.ToString();

                        AbilitaComponentiModificaArticolo(tipoCollaboratore);

                        ddl_FPnominativo.SelectedValue = "";

                        
                        txt_FPtelefono.Text = "";

                        ddl_FPtipoPagamento.Attributes.Add("readonly", "readonly");
                        ddl_FPtipoPagamento.CssClass = "w3-input w3-border w3-disabled";

                        chk_ModCosto.Attributes.Add("readonly", "readonly");
                        chk_ModCosto.CssClass = "w3-input w3-border w3-disabled";

                        txt_FPnetto.Attributes.Add("readonly", "readonly");
                        txt_FPnetto.CssClass = "w3-input w3-border w3-disabled";

                        txt_FPnotaCollaboratore.Text = "";
                        txt_FPnotaCollaboratore.Attributes.Add("readonly", "readonly");
                        txt_FPnotaCollaboratore.CssClass = "w3-input w3-border w3-disabled";
                    }

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "apriModificaArticolo", script: "javascript: document.getElementById('" + panelModificaArticolo.ClientID + "').style.display='block'", addScriptTags: true);

                    break;
                case "elimina":
                    listaDatiArticoliLavorazione.Remove(articoloSelezionato);
                    gvArticoliLavorazione.DataSource = listaDatiArticoliLavorazione;
                    gvArticoliLavorazione.DataBind();

                    AggiornaTotali();
                    ResetPanelLavorazione();

                    break;
                case "moveUp":
                    if (indexArticolo > 0)
                    {
                        listaDatiArticoliLavorazione.Remove(articoloSelezionato);
                        listaDatiArticoliLavorazione.Insert(indexArticolo - 1, articoloSelezionato);
                        gvArticoliLavorazione.DataSource = listaDatiArticoliLavorazione;
                        gvArticoliLavorazione.DataBind();
                    }
                    break;
                case "moveDown":
                    if (indexArticolo < listaDatiArticoliLavorazione.Count - 1)
                    {
                        listaDatiArticoliLavorazione.Remove(articoloSelezionato);
                        listaDatiArticoliLavorazione.Insert(indexArticolo + 1, articoloSelezionato);
                        gvArticoliLavorazione.DataSource = listaDatiArticoliLavorazione;
                        gvArticoliLavorazione.DataBind();
                    }
                    break;
            }

            RichiediOperazionePopup("UPDATE");
        }

        protected void btnOKModificaArtLavorazione_Click(object sender, EventArgs e)
        {
            //List<DatiArticoliLavorazione> listaDatiArticoliLavorazione = (List<DatiArticoli>)ViewState["listaDatiArticoli"];
            DatiArticoliLavorazione articoloSelezionato;

            if (ViewState["idArticolo"] == null)
            {
                long identificatoreOggetto = (long)ViewState["identificatoreArticolo"];
                articoloSelezionato = listaDatiArticoliLavorazione.FirstOrDefault(x => x.IdentificatoreOggetto == identificatoreOggetto);
            }
            else
            {
                int idArticolo = (int)ViewState["idArticolo"];
                articoloSelezionato = listaDatiArticoliLavorazione.FirstOrDefault(x => x.Id == idArticolo);
            }

            var index = listaDatiArticoliLavorazione.IndexOf(articoloSelezionato);

            articoloSelezionato.Descrizione = txt_Descrizione.Text;
            articoloSelezionato.DescrizioneLunga = txt_DescrizioneLunga.Text;

            articoloSelezionato.UsaCostoFP = chk_ModCosto.Checked;

            if (articoloSelezionato.UsaCostoFP != null && (bool)articoloSelezionato.UsaCostoFP)
            {
                articoloSelezionato.FP_netto = string.IsNullOrEmpty(txt_FPnetto.Text) ? 0 : decimal.Parse(txt_FPnetto.Text);
                articoloSelezionato.FP_lordo = string.IsNullOrEmpty(txt_FPlordo.Text) ? 0 : decimal.Parse(txt_FPlordo.Text);
                articoloSelezionato.Costo = 0;
            }
            else
            {
                articoloSelezionato.Costo = decimal.Parse(txt_Costo.Text);
                articoloSelezionato.FP_netto = 0;
                articoloSelezionato.FP_lordo = 0;
            }

            articoloSelezionato.Prezzo = decimal.Parse(txt_Prezzo.Text);
            articoloSelezionato.Iva = int.Parse(txt_Iva.Text);
            articoloSelezionato.Stampa = ddl_Stampa.SelectedValue == "1";
            articoloSelezionato.Data = DateTime.Now;
            articoloSelezionato.Tv = 0;

            if (ddl_FPtipo.SelectedValue == "0") //COLLABORATORI
            {
                articoloSelezionato.IdCollaboratori = ddl_FPnominativo.SelectedValue == "" ? null : (int?)int.Parse(ddl_FPnominativo.SelectedValue);
            }
            else //FORNITORI
            {
                articoloSelezionato.IdFornitori = ddl_FPnominativo.SelectedValue == "" ? null : (int?)int.Parse(ddl_FPnominativo.SelectedValue);
            }

            articoloSelezionato.IdTipoPagamento = ddl_FPtipoPagamento.SelectedValue == "" ? null : (int?)int.Parse(ddl_FPtipoPagamento.SelectedValue);
            articoloSelezionato.Nota = txt_FPnotaCollaboratore.Text;


            listaDatiArticoliLavorazione = listaDatiArticoliLavorazione.OrderByDescending(x => x.Prezzo).ToList();

            ResetPanelLavorazione();

            RichiediOperazionePopup("UPDATE");
        }

        protected void gvArticoliLavorazione_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DatiArticoliLavorazione rigaCorrente = (DatiArticoliLavorazione)e.Row.DataItem;

                if (rigaCorrente.IdCollaboratori != null && rigaCorrente.IdCollaboratori.HasValue)
                {
                    FiguraProfessionale figuraProfessionale = SessionManager.listaCompletaFigProf.FirstOrDefault(x => x.Id == rigaCorrente.IdCollaboratori);
                    ((Label)e.Row.FindControl("lbl_Riferimento")).Text = figuraProfessionale.Nome + " " + figuraProfessionale.Cognome;
                }

                if (rigaCorrente.IdFornitori != null && rigaCorrente.IdFornitori.HasValue)
                {
                    FiguraProfessionale figuraProfessionale = SessionManager.listaCompletaFigProf.FirstOrDefault(x => x.Id == rigaCorrente.IdFornitori);
                    ((Label)e.Row.FindControl("lbl_Riferimento")).Text = figuraProfessionale.Nome + " " + figuraProfessionale.Cognome;
                }

                if (rigaCorrente.IdTipoPagamento != null && rigaCorrente.IdTipoPagamento.HasValue)
                {
                    ((Label)e.Row.FindControl("lbl_TipoPagamento")).Text = SessionManager.listaTipiPagamento.FirstOrDefault(x => x.id == rigaCorrente.IdTipoPagamento).nome;
                }

                if (rigaCorrente.UsaCostoFP != null && (bool)rigaCorrente.UsaCostoFP)
                {
                    ((Label)e.Row.FindControl("lbl_Costo")).Text = string.Format("{0:N2}", rigaCorrente.FP_netto);
                }
                else
                {
                    ((Label)e.Row.FindControl("lbl_Costo")).Text = string.Format("{0:N2}", rigaCorrente.Costo);
                }
            }
        }

        protected void btnImporta_Click(object sender, EventArgs e)
        { }
        #endregion

        #region OPERAZIONI LAVORAZIONE
        private void PopolaCombo()
        {
            Esito esito = new Esito();


            //List<Anag_Qualifiche_Collaboratori> listaQualificheCollaboratori = Anag_Qualifiche_Collaboratori_BLL.Instance.getAllQualifiche(ref esito, true);
            //List<Anag_Collaboratori> listaAnagraficheCollaboratori = Anag_Collaboratori_BLL.Instance.getAllCollaboratori(ref esito);
            //List<Anag_Clienti_Fornitori> listaAnagraficheFornitori = Anag_Clienti_Fornitori_BLL.Instance.CaricaListaFornitori(ref esito);


            #region CAPITECNICI
            List<Anag_Qualifiche_Collaboratori> listaCapiTecnici = SessionManager.listaQualificheCollaboratori.Where(x => x.Qualifica == "Capo Tecnico").ToList<Anag_Qualifiche_Collaboratori>();

            List<Anag_Collaboratori> listaAnagraficheCapiTecnici = (from Item1 in SessionManager.listaAnagraficheCollaboratori
                                                                    join Item2 in listaCapiTecnici
                                                                    on Item1.Id equals Item2.Id_collaboratore
                                                                    select Item1).ToList();
            ddl_Capotecnico.Items.Clear();
            foreach (Anag_Collaboratori capoTecnico in listaAnagraficheCapiTecnici)
            {
                ddl_Capotecnico.Items.Add(new ListItem(capoTecnico.Nome + " " + capoTecnico.Cognome, capoTecnico.Id.ToString()));
            }
            ddl_Capotecnico.Items.Insert(0, new ListItem("<seleziona>", ""));
            #endregion

            #region PRODUTTORI
            List<Anag_Qualifiche_Collaboratori> listaProduttori = SessionManager.listaQualificheCollaboratori.Where(x => x.Qualifica == "Produttore").ToList<Anag_Qualifiche_Collaboratori>();

            List<Anag_Collaboratori> listaAnagraficheProduttori = (from Item1 in SessionManager.listaAnagraficheCollaboratori
                                                                   join Item2 in listaProduttori on Item1.Id equals Item2.Id_collaboratore
                                                                   select Item1).ToList();
            ddl_Produttore.Items.Clear();
            foreach (Anag_Collaboratori produttore in listaAnagraficheProduttori)
            {
                ddl_Produttore.Items.Add(new ListItem(produttore.Nome + " " + produttore.Cognome, produttore.Id.ToString()));
            }
            ddl_Produttore.Items.Insert(0, new ListItem("<seleziona>", ""));
            #endregion

            #region TIPOPAGAMENTO
            //List<Tipologica> listaTipiPagamento = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_PAGAMENTO);

            ddl_FPtipoPagamento.Items.Clear();
            foreach (Tipologica tipoPagamento in SessionManager.listaTipiPagamento)
            {
                ddl_FPtipoPagamento.Items.Add(new ListItem(tipoPagamento.nome, tipoPagamento.id.ToString()));
            }
            ddl_FPtipoPagamento.Items.Insert(0, new ListItem("<seleziona>", ""));
            #endregion

            #region QUALIFICHE
            //List<Tipologica> listaQualifiche = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_QUALIFICHE);

            ddl_FPqualifica.Items.Clear();
            foreach (Tipologica qualifica in SessionManager.listaQualifiche)
            {
                ddl_FPqualifica.Items.Add(new ListItem(qualifica.nome, qualifica.id.ToString()));
            }
            ddl_FPqualifica.Items.Insert(0, new ListItem("<seleziona>", ""));
            #endregion

      
            
        }

        protected void filtraFP(object sender, EventArgs e)
        {
            Esito esito = new Esito();

            int tipoFP = int.Parse(ddl_FPtipo.SelectedValue);
            string qualificaFP = ddl_FPqualifica.SelectedValue;
            string cittaFP = ddl_FPcitta.SelectedValue;

            AbilitaComponentiModificaArticolo(tipoFP);

            if (tipoFP == 1) // FORNITORI
            {
                //ddl_FPqualifica.SelectedValue = "0";
                //ddl_FPqualifica.CssClass = "w3-input w3-border";
                //ddl_FPqualifica.Style.Add("cursor", "not-allowed;");
                //ddl_FPqualifica.Enabled = false;

                //ddl_FPtipoPagamento.SelectedValue = "0";
                //ddl_FPtipoPagamento.CssClass = "w3-input w3-border";
                //ddl_FPtipoPagamento.Style.Add("cursor", "not-allowed;");
                //ddl_FPtipoPagamento.Enabled = false;


                //List<Anag_Clienti_Fornitori> listaAnagraficheFornitori = Anag_Clienti_Fornitori_BLL.Instance.CaricaListaFornitori(ref esito);

                //List<string> listaCompletaCitta = new List<string>();
                //listaCompletaCitta.AddRange((from record in listaAnagraficheFornitori select record.ComuneLegale.Trim().ToLower()).ToList());
                //listaCompletaCitta = listaCompletaCitta.Distinct().ToList<string>();
                //listaCompletaCitta.Sort();

                SessionManager.listaCittaFornitori.Sort();
                ddl_FPcitta.Items.Clear();
                foreach (string citta in SessionManager.listaCittaFornitori)
                {
                    ddl_FPcitta.Items.Add(new ListItem(citta.ToUpper(), citta));
                }
                ddl_FPcitta.Items.Insert(0, new ListItem("<seleziona>", ""));
            }
            else // COLLABORATORI
            {
                //ddl_FPqualifica.CssClass = "w3-input w3-border";
                //ddl_FPqualifica.Style.Remove("cursor");
                //ddl_FPqualifica.Enabled = true;

                //ddl_FPtipoPagamento.CssClass = "w3-input w3-border";
                //ddl_FPtipoPagamento.Style.Remove("cursor");
                //ddl_FPtipoPagamento.Enabled = true;

                //List<Anag_Collaboratori> listaAnagraficheCollaboratori = Anag_Collaboratori_BLL.Instance.getAllCollaboratori(ref esito);

                //List<string> listaCompletaCitta = new List<string>();
                //listaCompletaCitta.AddRange((from record in listaAnagraficheCollaboratori select record.ComuneRiferimento.Trim().ToLower()).ToList());
                //listaCompletaCitta = listaCompletaCitta.Distinct().ToList<string>();
                //listaCompletaCitta.Sort();

                //basePage.listaCittaCollaboratori = basePage.listaCittaCollaboratori.Distinct().ToList();
                SessionManager.listaCittaCollaboratori.Sort();
                ddl_FPcitta.Items.Clear();
                foreach (string citta in SessionManager.listaCittaCollaboratori)
                {
                    ddl_FPcitta.Items.Add(new ListItem(citta.ToUpper(), citta));
                }
                ddl_FPcitta.Items.Insert(0, new ListItem("<seleziona>", ""));
            }

            List<FiguraProfessionale> listaFPfiltrata = SessionManager.listaCompletaFigProf.Where(x => x.Tipo == tipoFP).ToList();

            if (tipoFP == 0 &&  !string.IsNullOrEmpty(qualificaFP))
            {
                listaFPfiltrata = listaFPfiltrata.Where(x => x.Qualifiche.Where(y => y.Id == int.Parse(qualificaFP)).Count() > 0).ToList();
            }

            if (cittaFP != "")
            {
                listaFPfiltrata = listaFPfiltrata.Where(x => x.Citta.ToLower().Trim().Contains(cittaFP)).ToList();
            }

            PopolaNominativi(listaFPfiltrata);

            upModificaArticolo.Update();
        }

        protected void visualizzaFP(object sender, EventArgs e)
        {
            FiguraProfessionale fpSelezionata = new FiguraProfessionale();
            if (ddl_FPnominativo.SelectedValue != "")
            {
                fpSelezionata = SessionManager.listaCompletaFigProf.FirstOrDefault(x => x.Id == int.Parse(ddl_FPnominativo.SelectedValue));

                ddl_FPtipoPagamento.Attributes.Remove("readonly");
                ddl_FPtipoPagamento.CssClass = "w3-input w3-border";

                chk_ModCosto.Attributes.Remove("readonly");
                chk_ModCosto.CssClass = "w3-input w3-border";

                txt_FPnetto.Attributes.Remove("readonly");
                txt_FPnetto.CssClass = "w3-input w3-border";

                txt_FPnotaCollaboratore.Attributes.Remove("readonly");
                txt_FPnotaCollaboratore.CssClass = "w3-input w3-border";
            }
            else
            {
                ddl_FPtipoPagamento.Attributes.Add("readonly", "readonly");
                ddl_FPtipoPagamento.CssClass = "w3-input w3-border w3-disabled";

                chk_ModCosto.Attributes.Add("readonly", "readonly");
                chk_ModCosto.CssClass = "w3-input w3-border w3-disabled";

                txt_FPnetto.Attributes.Add("readonly", "readonly");
                txt_FPnetto.CssClass = "w3-input w3-border w3-disabled";

                txt_FPnotaCollaboratore.Attributes.Add("readonly", "readonly");
                txt_FPnotaCollaboratore.CssClass = "w3-input w3-border w3-disabled";
            }

            PopolaDettagliFP(fpSelezionata);

            upModificaArticolo.Update();
        }

        private void PopolaNominativi(List<FiguraProfessionale> listaNominativi)
        {
            ddl_FPnominativo.Items.Clear();
            foreach (FiguraProfessionale figPro in listaNominativi)
            {
                ddl_FPnominativo.Items.Add(new ListItem(figPro.Cognome + " " + figPro.Nome, figPro.Id.ToString()));
            }
            ddl_FPnominativo.Items.Insert(0, new ListItem("<seleziona>", ""));
        }

        private void PopolaDettagliFP(FiguraProfessionale figuraProfessionale)
        {
            txt_FPtelefono.Text = figuraProfessionale.Telefono;
            txt_FPnotaCollaboratore.Text = figuraProfessionale.Nota;
        }

        private void aggiungiArticoliDelGruppoAListaArticoli(int idGruppo)
        {
            Esito esito = new Esito();
            int idLavorazione = lavorazioneCorrente == null ? 0 : lavorazioneCorrente.Id;

            listaDatiArticoliLavorazione.AddRange(Articoli_BLL.Instance.CaricaListaArticoliLavorazioneByIDGruppo(idLavorazione, idGruppo, ref esito));
            listaDatiArticoliLavorazione = listaDatiArticoliLavorazione.OrderByDescending(x => x.Prezzo).ToList();
        }

        private void aggiungiArticoloAListaArticoli(int idArticolo)
        {
            Esito esito = new Esito();
            int idLavorazione = lavorazioneCorrente == null ? 0 : lavorazioneCorrente.Id;

            listaDatiArticoliLavorazione.Add(Articoli_BLL.Instance.CaricaArticoloLavorazioneByID(idLavorazione, idArticolo, ref esito));
            listaDatiArticoliLavorazione = listaDatiArticoliLavorazione.OrderByDescending(x => x.Prezzo).ToList();
        }

        private void AggiornaTotali()
        {
            decimal totPrezzo = 0;
            decimal totCosto = 0;
            decimal totIva = 0;
            decimal percRicavo = 0;

            if (listaDatiArticoliLavorazione != null && listaDatiArticoliLavorazione.Count > 0)
            {
                foreach (DatiArticoliLavorazione art in listaDatiArticoliLavorazione)
                {
                    if (art.UsaCostoFP != null )
                    {
                        if ((bool)art.UsaCostoFP)
                        {
                            totCosto += art.FP_netto != null ? (decimal)art.FP_netto : 0;
                        }
                        else
                        {
                            totCosto +=  (decimal)art.Costo ;
                        }
                    }
                    else
                    {
                        totCosto += (decimal)art.Costo;
                    }

                    totPrezzo += art.Prezzo;
                    //totCosto += (art.UsaCostoFP != null && (bool)art.UsaCostoFP) ? (decimal)art.FP_netto : art.Costo;
                    totIva += (art.Prezzo * art.Iva / 100);
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
            gvArticoliLavorazione.DataSource = listaDatiArticoliLavorazione;
            gvArticoliLavorazione.DataBind();

            lbl_selezionareArticolo.Visible = (listaDatiArticoliLavorazione == null || listaDatiArticoliLavorazione.Count == 0);

            AggiornaTotali();
        }

        public void PopolaLavorazione(int idDatiAgenda, int idCliente)
        {
            Esito esito = new Esito();

            #region INIZIALIZZAZIONE OGGETTI LAVORAZIONE CORRENTE
            //DatiAgenda eventoSelezionato = (DatiAgenda)ViewState["eventoSelezionato"];
            lavorazioneCorrente = Dati_Lavorazione_BLL.Instance.getDatiLavorazioneByIdEvento(idDatiAgenda, ref esito);
            SessionManager.listaReferenti = Anag_Referente_Clienti_Fornitori_BLL.Instance.getReferentiByIdAzienda(ref esito, idCliente);
            #endregion


            //gvGruppi.DataSource = listaArticoliGruppi;
            //gvGruppi.DataBind();

            //PopolaCombo();

            //if (eventoSelezionato != null)
            //{
            //    List<Anag_Referente_Clienti_Fornitori> listaReferenti = Anag_Referente_Clienti_Fornitori_BLL.Instance.getReferentiByIdAzienda(ref esito, eventoSelezionato.id_cliente);
            //    foreach (Anag_Referente_Clienti_Fornitori referente in listaReferenti)
            //    {
            //        ddl_Referente.Items.Add(new ListItem(referente.Nome + " " + referente.Cognome, referente.Id.ToString()));
            //    }
            //    ddl_Referente.Items.Insert(0, new ListItem("<seleziona>", "0"));
            //}listaDatiArticoliLavorazione

            if (esito.codice != Esito.ESITO_OK)
            {
                basePage.ShowError(esito.descrizione);
            }
            else
            {
                //ddl_Referente.Items.Clear();

                //foreach (Anag_Referente_Clienti_Fornitori referente in listaReferenti)
                //{
                //    ddl_Referente.Items.Add(new ListItem(referente.Nome + " " + referente.Cognome, referente.Id.ToString()));
                //}
                //ddl_Referente.Items.Insert(0, new ListItem("<seleziona>", "0"));

                CreaNuovaLavorazione(listaDatiArticoliLavorazione);

                if (lavorazioneCorrente != null)
                {
                    txt_Ordine.Text = lavorazioneCorrente.Ordine;
                    txt_Fattura.Text = lavorazioneCorrente.Fattura;
                    ddl_Contratto.SelectedValue = lavorazioneCorrente.IdContratto == null ? "": lavorazioneCorrente.IdContratto.ToString();
                    ddl_Referente.SelectedValue = lavorazioneCorrente.IdReferente == null ? "" : lavorazioneCorrente.IdReferente.ToString();
                    ddl_Capotecnico.SelectedValue = lavorazioneCorrente.IdCapoTecnico == null ? "" : lavorazioneCorrente.IdCapoTecnico.ToString();
                    ddl_Produttore.SelectedValue = lavorazioneCorrente.IdProduttore == null ? "" : lavorazioneCorrente.IdProduttore.ToString();

                    listaDatiArticoliLavorazione = lavorazioneCorrente.ListaArticoliLavorazione;

                    gvArticoliLavorazione.DataSource = listaDatiArticoliLavorazione;
                    gvArticoliLavorazione.DataBind();
                }
                lbl_selezionareArticolo.Visible = (listaDatiArticoliLavorazione == null || listaDatiArticoliLavorazione.Count == 0);
                AggiornaTotali();
            }
        }

        public void CreaNuovaLavorazione(List<DatiArticoliLavorazione> listaArticoliLavorazione)
        {
            Esito esito = new Esito();

            ddl_Referente.Items.Clear();
           // List<Anag_Referente_Clienti_Fornitori> listaReferenti = Anag_Referente_Clienti_Fornitori_BLL.Instance.getReferentiByIdAzienda(ref esito, eventoSelezionato.id_cliente);
            foreach (Anag_Referente_Clienti_Fornitori referente in SessionManager.listaReferenti)
            {
                ddl_Referente.Items.Add(new ListItem(referente.Nome + " " + referente.Cognome, referente.Id.ToString()));
            }
            ddl_Referente.Items.Insert(0, new ListItem("<seleziona>", ""));


            listaDatiArticoliLavorazione = listaArticoliLavorazione;

            gvArticoliLavorazione.DataSource = listaDatiArticoliLavorazione;
            gvArticoliLavorazione.DataBind();
        }

        public void ClearLavorazione()
        {
            //ClearModificaArticoli();
            lbl_selezionareArticolo.Visible = true;
            //ViewState["listaDatiArticoliLavorazione"] = null;
            listaDatiArticoliLavorazione = null;
            gvArticoliLavorazione.DataSource = null;
            gvArticoliLavorazione.DataBind();

            txt_TotPrezzo.Text = "";
            txt_TotCosto.Text = "";
            txt_TotIva.Text = "";
            txt_PercRicavo.Text = "";

            ResetPanelLavorazione();
        }

        public DatiLavorazione CreaDatiLavorazione()
        {
            DatiLavorazione datiLavorazione = new DatiLavorazione();
            datiLavorazione.Id = lavorazioneCorrente == null ? 0 : lavorazioneCorrente.Id;

            //BasePage.ValidaCampo(txt_DurataLavorazione, 0, campoObbligatorio, ref esito);

            datiLavorazione.Ordine = txt_Ordine.Text;
            datiLavorazione.Fattura = txt_Fattura.Text;
            datiLavorazione.IdContratto = string.IsNullOrEmpty(ddl_Contratto.SelectedValue) ? null : (int?)int.Parse(ddl_Contratto.SelectedValue);
            datiLavorazione.IdReferente = string.IsNullOrEmpty(ddl_Referente.SelectedValue) ? null : (int?)int.Parse(ddl_Referente.SelectedValue); //int.Parse(ddl_Referente.SelectedValue);
            datiLavorazione.IdCapoTecnico = string.IsNullOrEmpty(ddl_Capotecnico.SelectedValue) ? null : (int?)int.Parse(ddl_Capotecnico.SelectedValue); //int.Parse(ddl_Capotecnico.SelectedValue);
            datiLavorazione.IdProduttore = string.IsNullOrEmpty(ddl_Produttore.SelectedValue) ? null : (int?)int.Parse(ddl_Produttore.SelectedValue); //int.Parse(ddl_Produttore.SelectedValue);
            datiLavorazione.ListaArticoliLavorazione = listaDatiArticoliLavorazione;
            return datiLavorazione;
        }
        #endregion
    }
}