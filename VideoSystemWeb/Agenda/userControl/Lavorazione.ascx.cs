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

        public List<DatiArticoliLavorazione> listaDatiArticoliLavorazione
        {
            get
            {
                return (List<DatiArticoliLavorazione>)ViewState["listaDatiArticoliLavorazione"];
            }
            set
            {
                ViewState["listaDatiArticoliLavorazione"] = value;
            }
        }
        public DatiLavorazione lavorazioneCorrente
        {
            get
            {
                return (DatiLavorazione)ViewState["lavorazioneCorrente"];
            }
            set
            {
                ViewState["lavorazioneCorrente"] = value;
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

        List<FiguraProfessionale> listaCompletaFigProf
        {
            get
            {
                if (ViewState["listaCompletaFigProf"] == null || ((List<FiguraProfessionale>)ViewState["listaCompletaFigProf"]).Count() == 0)
                {
                    return new List<FiguraProfessionale>();
                }
                return (List<FiguraProfessionale>)ViewState["listaCompletaFigProf"];
            }

            set
            {
                ViewState["listaCompletaFigProf"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //spostato in popolaLavorazione
                //gvGruppi.DataSource = listaArticoliGruppi;
                //gvGruppi.DataBind();

                //PopolaCombo();
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
            //    ddl_Referente.Items.Insert(0, new ListItem("<selezionare>", "0"));
            //}
        }

        #region COMPORTAMENTO ELEMENTI PAGINA

        private void PopolaCombo()
        {
            Esito esito = new Esito();

            List<Anag_Qualifiche_Collaboratori> listaQualificheCollaboratori = Anag_Qualifiche_Collaboratori_BLL.Instance.getAllQualifiche(ref esito, true);
            List<Anag_Collaboratori> listaAnagraficheCollaboratori = Anag_Collaboratori_BLL.Instance.getAllCollaboratori(ref esito);
            List<Anag_Clienti_Fornitori> listaAnagraficheFornitori = Anag_Clienti_Fornitori_BLL.Instance.CaricaListaFornitori(ref esito);


            #region CAPITECNICI
            List<Anag_Qualifiche_Collaboratori> listaCapiTecnici = listaQualificheCollaboratori.Where(x => x.Qualifica == "Capo Tecnico").ToList<Anag_Qualifiche_Collaboratori>();

            List<Anag_Collaboratori> listaAnagraficheCapiTecnici = (from Item1 in listaAnagraficheCollaboratori
                                                                    join Item2 in listaCapiTecnici
                                                                    on Item1.Id equals Item2.Id_collaboratore
                                                                    select Item1).ToList();
            ddl_Capotecnico.Items.Clear();
            foreach (Anag_Collaboratori capoTecnico in listaAnagraficheCapiTecnici)
            {
                ddl_Capotecnico.Items.Add(new ListItem(capoTecnico.Nome + " " + capoTecnico.Cognome, capoTecnico.Id.ToString()));
            }
            ddl_Capotecnico.Items.Insert(0, new ListItem("<selezionare>", "0"));
            #endregion

            #region PRODUTTORI
            List<Anag_Qualifiche_Collaboratori> listaProduttori = listaQualificheCollaboratori.Where(x => x.Qualifica == "Produttore").ToList<Anag_Qualifiche_Collaboratori>();

            List<Anag_Collaboratori> listaAnagraficheProduttori = (from Item1 in listaAnagraficheCollaboratori
                                                                   join Item2 in listaProduttori on Item1.Id equals Item2.Id_collaboratore
                                                                   select Item1).ToList();
            ddl_Produttore.Items.Clear();
            foreach (Anag_Collaboratori produttore in listaAnagraficheProduttori)
            {
                ddl_Produttore.Items.Add(new ListItem(produttore.Nome + " " + produttore.Cognome, produttore.Id.ToString()));
            }
            ddl_Produttore.Items.Insert(0, new ListItem("<selezionare>", "0"));
            #endregion

            #region TIPOPAGAMENTO
            List<Tipologica> listaTipiPagamento = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_PAGAMENTO);
            
            ddl_FPtipoPagamento.Items.Clear();
            foreach (Tipologica tipoPagamento in listaTipiPagamento)
            {
                ddl_FPtipoPagamento.Items.Add(new ListItem(tipoPagamento.nome, tipoPagamento.id.ToString()));
            }
            ddl_FPtipoPagamento.Items.Insert(0, new ListItem("<selezionare>", "0"));
            #endregion

            #region QUALIFICHE
            List<Tipologica> listaQualifiche = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_QUALIFICHE);

            ddl_FPqualifica.Items.Clear();
            foreach (Tipologica qualifica in listaQualifiche)
            {
                ddl_FPqualifica.Items.Add(new ListItem(qualifica.nome, qualifica.id.ToString()));
            }
            ddl_FPqualifica.Items.Insert(0, new ListItem("<selezionare>", "0"));
            #endregion

            #region CITTA'
            List<string> listaCompletaCitta = new List<string>();
            listaCompletaCitta.AddRange((from record in listaAnagraficheCollaboratori select record.ComuneRiferimento.Trim().ToLower()).ToList());
            //listaCompletaCitta.AddRange((from record in listaAnagraficheFornitori select record.ComuneLegale.Trim().ToLower()).ToList());
            listaCompletaCitta = listaCompletaCitta.Distinct().ToList<string>();
            listaCompletaCitta.Sort();

            ddl_FPcitta.Items.Clear();
            foreach (string citta in listaCompletaCitta)
            {
                ddl_FPcitta.Items.Add(new ListItem(citta.ToUpper(), citta));
            }
            ddl_FPcitta.Items.Insert(0, new ListItem("<selezionare>", "0"));
            #endregion

            #region NOMINATIVO
            //List<FiguraProfessionale> listaCompletaFigProf = new List<FiguraProfessionale>();

            List<FiguraProfessionale> _listaCompletaFigProf = new List<FiguraProfessionale>();
            foreach (Anag_Collaboratori collaboratore in listaAnagraficheCollaboratori)
            {
                _listaCompletaFigProf.Add(new FiguraProfessionale()
                {
                    Id = collaboratore.Id,
                    Nome = collaboratore.Nome,
                    Cognome = collaboratore.Cognome,
                    Citta = collaboratore.ComuneRiferimento.Trim().ToLower(),

                    Telefono = collaboratore.Telefoni.Count == 0 ? "" : collaboratore.Telefoni.FirstOrDefault(x => x.Priorita == 1).Pref_naz + collaboratore.Telefoni.FirstOrDefault(x => x.Priorita == 1).Numero,
                    Qualifiche = collaboratore.Qualifiche,
                    Tipo = 0,
                    Nota = collaboratore.Note
                });

            }

            foreach (Anag_Clienti_Fornitori fornitore in listaAnagraficheFornitori)
            {
                _listaCompletaFigProf.Add(new FiguraProfessionale()
                {
                    Id = fornitore.Id,
                    Cognome = fornitore.RagioneSociale,
                    Citta = fornitore.ComuneLegale.Trim().ToLower(),
                    Telefono = fornitore.Telefono,
                    Tipo = 1,
                    Nota = fornitore.Note
                });
            }
            listaCompletaFigProf = _listaCompletaFigProf.OrderBy(x => x.Cognome).ToList<FiguraProfessionale>();
            PopolaNominativi(listaCompletaFigProf.Where(x => x.Tipo == 0).ToList());
            #endregion
        }

        protected void filtraFP(object sender, EventArgs e)
        {
            Esito esito = new Esito();

            int tipoFP = int.Parse(ddl_FPtipo.SelectedValue);
            int qualificaFP = int.Parse(ddl_FPqualifica.SelectedValue);
            string cittaFP = ddl_FPcitta.SelectedValue;
            
            if (tipoFP == 1)
            {
                ddl_FPqualifica.SelectedValue = "0";
                ddl_FPqualifica.CssClass = "w3-input w3-border";
                ddl_FPqualifica.Style.Add("cursor", "not-allowed;");
                ddl_FPqualifica.Enabled = false;

                ddl_FPtipoPagamento.SelectedValue = "0";
                ddl_FPtipoPagamento.CssClass = "w3-input w3-border";
                ddl_FPtipoPagamento.Style.Add("cursor", "not-allowed;");
                ddl_FPtipoPagamento.Enabled = false;


                List<Anag_Clienti_Fornitori> listaAnagraficheFornitori = Anag_Clienti_Fornitori_BLL.Instance.CaricaListaFornitori(ref esito);

                List<string> listaCompletaCitta = new List<string>();
                //listaCompletaCitta.AddRange((from record in listaAnagraficheCollaboratori select record.ComuneRiferimento.Trim().ToLower()).ToList());
                listaCompletaCitta.AddRange((from record in listaAnagraficheFornitori select record.ComuneLegale.Trim().ToLower()).ToList());
                listaCompletaCitta = listaCompletaCitta.Distinct().ToList<string>();
                listaCompletaCitta.Sort();

                ddl_FPcitta.Items.Clear();
                foreach (string citta in listaCompletaCitta)
                {
                    ddl_FPcitta.Items.Add(new ListItem(citta.ToUpper(), citta));
                }
                ddl_FPcitta.Items.Insert(0, new ListItem("<selezionare>", "0"));
            }
            else
            {
                ddl_FPqualifica.CssClass = "w3-input w3-border";
                ddl_FPqualifica.Style.Remove("cursor");
                ddl_FPqualifica.Enabled = true;

                ddl_FPtipoPagamento.CssClass = "w3-input w3-border";
                ddl_FPtipoPagamento.Style.Remove("cursor");
                ddl_FPtipoPagamento.Enabled = true;

                List<Anag_Collaboratori> listaAnagraficheCollaboratori = Anag_Collaboratori_BLL.Instance.getAllCollaboratori(ref esito);

                List<string> listaCompletaCitta = new List<string>();
                listaCompletaCitta.AddRange((from record in listaAnagraficheCollaboratori select record.ComuneRiferimento.Trim().ToLower()).ToList());
                //listaCompletaCitta.AddRange((from record in listaAnagraficheFornitori select record.ComuneLegale.Trim().ToLower()).ToList());
                listaCompletaCitta = listaCompletaCitta.Distinct().ToList<string>();
                listaCompletaCitta.Sort();

                ddl_FPcitta.Items.Clear();
                foreach (string citta in listaCompletaCitta)
                {
                    ddl_FPcitta.Items.Add(new ListItem(citta.ToUpper(), citta));
                }
                ddl_FPcitta.Items.Insert(0, new ListItem("<selezionare>", "0"));
            }

            List<FiguraProfessionale> listaFPfiltrata = listaCompletaFigProf.Where(x => x.Tipo == tipoFP).ToList();

            if (tipoFP == 0 && qualificaFP != 0)
            {
                listaFPfiltrata = listaFPfiltrata.Where(x => x.Qualifiche.Where(y=> y.Id == qualificaFP).Count()>0).ToList();
            }

            if (cittaFP != "0")
            {
                listaFPfiltrata = listaFPfiltrata.Where(x => x.Citta.ToLower().Trim().Contains(cittaFP)).ToList();
            }

            PopolaNominativi(listaFPfiltrata);

            upModificaArticolo.Update();
        }

        protected void visualizzaFP(object sender, EventArgs e)
        {
            FiguraProfessionale fpSelezionata = new FiguraProfessionale();
            if (ddl_FPnominativo.SelectedValue != "0")
            {
                fpSelezionata = listaCompletaFigProf.FirstOrDefault(x => x.Id == int.Parse(ddl_FPnominativo.SelectedValue));
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
            ddl_FPnominativo.Items.Insert(0, new ListItem("<selezionare>", "0"));
        }

        private void PopolaDettagliFP(FiguraProfessionale figuraProfessionale)
        {
            txt_FPtelefono.Text = figuraProfessionale.Telefono;
            txt_FPnotaCollaboratore.Text = figuraProfessionale.Nota;
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

            listaDatiArticoliLavorazione = (List<DatiArticoliLavorazione>)ViewState["listaDatiArticoliLavorazione"];
            if (listaDatiArticoliLavorazione == null)
            {
                listaDatiArticoliLavorazione = new List<DatiArticoliLavorazione>();
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

            ViewState["listaDatiArticoliLavorazione"] = listaDatiArticoliLavorazione;

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

                    FiguraProfessionale figuraProfessionale = listaCompletaFigProf.FirstOrDefault(x => x.Id == articoloSelezionato.IdCollaboratori);
                    if (figuraProfessionale == null)
                    {
                        figuraProfessionale = listaCompletaFigProf.FirstOrDefault(x => x.Id == articoloSelezionato.IdFornitori);
                    }
                    if (figuraProfessionale != null)
                    {
                        if (figuraProfessionale.Tipo == 1)
                        {
                            ddl_FPqualifica.SelectedValue = "0";
                            ddl_FPqualifica.CssClass = "w3-input w3-border";
                            ddl_FPqualifica.Style.Add("cursor", "not-allowed;");
                            ddl_FPqualifica.Enabled = false;

                            ddl_FPtipoPagamento.SelectedValue = "0";
                            ddl_FPtipoPagamento.CssClass = "w3-input w3-border";
                            ddl_FPtipoPagamento.Style.Add("cursor", "not-allowed;");
                            ddl_FPtipoPagamento.Enabled = false;
                        }
                        else
                        {
                            ddl_FPqualifica.CssClass = "w3-input w3-border";
                            ddl_FPqualifica.Style.Remove("cursor");
                            ddl_FPqualifica.Enabled = true;

                            ddl_FPtipoPagamento.CssClass = "w3-input w3-border";
                            ddl_FPtipoPagamento.Style.Remove("cursor");
                            ddl_FPtipoPagamento.Enabled = true;
                        }

                        PopolaNominativi(listaCompletaFigProf.Where(x => x.Tipo == figuraProfessionale.Tipo).ToList());
                        ddl_FPtipo.SelectedValue = figuraProfessionale.Tipo.ToString();
                        txt_FPnotaCollaboratore.Text = figuraProfessionale.Nota;
                        ddl_FPnominativo.SelectedValue = figuraProfessionale.Id.ToString();
                        txt_FPtelefono.Text = figuraProfessionale.Telefono;
                    }
                    else
                    {
                        PopolaNominativi(listaCompletaFigProf.Where(x => x.Tipo == 0).ToList());
                        ddl_FPtipo.SelectedValue = "0";

                        ddl_FPqualifica.SelectedValue = "0";
                        ddl_FPqualifica.CssClass = "w3-input w3-border";
                        ddl_FPqualifica.Style.Remove("cursor");
                        ddl_FPqualifica.Enabled = true;

                        ddl_FPtipoPagamento.SelectedValue = "0";
                        ddl_FPtipoPagamento.CssClass = "w3-input w3-border";
                        ddl_FPtipoPagamento.Style.Remove("cursor");
                        ddl_FPtipoPagamento.Enabled = true;

                        ddl_FPnominativo.SelectedValue = "0";

                        txt_FPnotaCollaboratore.Text = "";
                        txt_FPtelefono.Text = "";

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
            articoloSelezionato.Costo = decimal.Parse(txt_Costo.Text);
            articoloSelezionato.Prezzo = decimal.Parse(txt_Prezzo.Text);
            articoloSelezionato.Iva = int.Parse(txt_Iva.Text);
            articoloSelezionato.Stampa = ddl_Stampa.SelectedValue == "1";
            articoloSelezionato.Data = DateTime.Now;
            articoloSelezionato.Tv = 0;

            if (ddl_FPtipo.SelectedValue == "0")
            {
                articoloSelezionato.IdCollaboratori = ddl_FPnominativo.SelectedValue == "0" ? null : (int?)int.Parse(ddl_FPnominativo.SelectedValue);
            }
            else
            {
                articoloSelezionato.IdFornitori = ddl_FPnominativo.SelectedValue == "0" ? null : (int?)int.Parse(ddl_FPnominativo.SelectedValue);
            }

            articoloSelezionato.IdTipoPagamento = ddl_FPtipoPagamento.SelectedValue == "0" ? null : (int?)int.Parse(ddl_FPtipoPagamento.SelectedValue);


            listaDatiArticoliLavorazione = listaDatiArticoliLavorazione.OrderByDescending(x => x.Prezzo).ToList();

            ResetPanelLavorazione();

            RichiediOperazionePopup("UPDATE");
        }

        protected void gvArticoliLavorazione_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DatiArticoliLavorazione rigaCorrente = (DatiArticoliLavorazione)e.Row.DataItem;

                if (rigaCorrente.IdCollaboratori.HasValue && rigaCorrente.IdCollaboratori != null)
                {
                    FiguraProfessionale figuraProfessionale = listaCompletaFigProf.FirstOrDefault(x => x.Id == rigaCorrente.IdCollaboratori);
                    ((Label)e.Row.FindControl("lbl_Riferimento")).Text = figuraProfessionale.Nome + " " + figuraProfessionale.Cognome;
                }

                if (rigaCorrente.IdFornitori.HasValue && rigaCorrente.IdFornitori != null)
                {
                    FiguraProfessionale figuraProfessionale = listaCompletaFigProf.FirstOrDefault(x => x.Id == rigaCorrente.IdFornitori);
                    ((Label)e.Row.FindControl("lbl_Riferimento")).Text = figuraProfessionale.Nome + " " + figuraProfessionale.Cognome;
                }

                if (rigaCorrente.IdTipoPagamento.HasValue && rigaCorrente.IdTipoPagamento != null)
                {
                    List<Tipologica> listaTipiPagamento = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_PAGAMENTO);

                    ((Label)e.Row.FindControl("lbl_TipoPagamento")).Text = listaTipiPagamento.FirstOrDefault(x => x.id == rigaCorrente.IdTipoPagamento).nome;
                }

                
            }


        }

        protected void btnImporta_Click(object sender, EventArgs e)
        { }
        #endregion

        #region OPERAZIONI LAVORAZIONE
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
                    totPrezzo += art.Prezzo;
                    totCosto += art.Costo;
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
            // ClearModificaArticoli();

            gvArticoliLavorazione.DataSource = listaDatiArticoliLavorazione;
            gvArticoliLavorazione.DataBind();

            lbl_selezionareArticolo.Visible = (listaDatiArticoliLavorazione == null || listaDatiArticoliLavorazione.Count == 0);

            AggiornaTotali();
        }

        public void PopolaLavorazione(int idDatiAgenda, int idCliente)
        {
            Esito esito = new Esito();

            gvGruppi.DataSource = listaArticoliGruppi;
            gvGruppi.DataBind();
            PopolaCombo();

            DatiAgenda eventoSelezionato = (DatiAgenda)ViewState["eventoSelezionato"];
            if (eventoSelezionato != null)
            {
                //Esito esito = new Esito();
                List<Anag_Referente_Clienti_Fornitori> listaReferenti = Anag_Referente_Clienti_Fornitori_BLL.Instance.getReferentiByIdAzienda(ref esito, eventoSelezionato.id_cliente);
                foreach (Anag_Referente_Clienti_Fornitori referente in listaReferenti)
                {
                    ddl_Referente.Items.Add(new ListItem(referente.Nome + " " + referente.Cognome, referente.Id.ToString()));
                }
                ddl_Referente.Items.Insert(0, new ListItem("<selezionare>", "0"));
            }

            lavorazioneCorrente = Dati_Lavorazione_BLL.Instance.getDatiLavorazioneByIdEvento(idDatiAgenda, ref esito);

            if (esito.codice != Esito.ESITO_OK)
            {
                basePage.ShowError(esito.descrizione);
            }
            else
            {
                ddl_Referente.Items.Clear();
                List<Anag_Referente_Clienti_Fornitori> listaReferenti = Anag_Referente_Clienti_Fornitori_BLL.Instance.getReferentiByIdAzienda(ref esito, idCliente);
                foreach (Anag_Referente_Clienti_Fornitori referente in listaReferenti)
                {
                    ddl_Referente.Items.Add(new ListItem(referente.Nome + " " + referente.Cognome, referente.Id.ToString()));
                }
                ddl_Referente.Items.Insert(0, new ListItem("<selezionare>", "0"));


                if (lavorazioneCorrente != null)
                {
                    txt_Ordine.Text = lavorazioneCorrente.Ordine;
                    txt_Fattura.Text = lavorazioneCorrente.Fattura;
                    ddl_Contratto.SelectedValue = lavorazioneCorrente.IdContratto == null ? "0": lavorazioneCorrente.IdContratto.ToString();
                    ddl_Referente.SelectedValue = lavorazioneCorrente.IdReferente == null ? "0" : lavorazioneCorrente.IdReferente.ToString();
                    ddl_Capotecnico.SelectedValue = lavorazioneCorrente.IdCapoTecnico == null ? "0" : lavorazioneCorrente.IdCapoTecnico.ToString();
                    ddl_Produttore.SelectedValue = lavorazioneCorrente.IdProduttore == null ? "0" : lavorazioneCorrente.IdProduttore.ToString();

                    listaDatiArticoliLavorazione = lavorazioneCorrente.ListaArticoliLavorazione;

                    gvArticoliLavorazione.DataSource = listaDatiArticoliLavorazione;
                    gvArticoliLavorazione.DataBind();
                }
                //ResetPanelLavorazione();
                AggiornaTotali();
            }
        }

        public void CreaNuovaLavorazione(DatiAgenda eventoSelezionato, List<DatiArticoliLavorazione> listaArticoliLavorazione)
        {
            Esito esito = new Esito();

            ddl_Referente.Items.Clear();
            List<Anag_Referente_Clienti_Fornitori> listaReferenti = Anag_Referente_Clienti_Fornitori_BLL.Instance.getReferentiByIdAzienda(ref esito, eventoSelezionato.id_cliente);
            foreach (Anag_Referente_Clienti_Fornitori referente in listaReferenti)
            {
                ddl_Referente.Items.Add(new ListItem(referente.Nome + " " + referente.Cognome, referente.Id.ToString()));
            }
            ddl_Referente.Items.Insert(0, new ListItem("<selezionare>", "0"));


            listaDatiArticoliLavorazione = listaArticoliLavorazione;

            gvArticoliLavorazione.DataSource = listaDatiArticoliLavorazione;
            gvArticoliLavorazione.DataBind();
        }

        public void ClearLavorazione()
        {
            //ClearModificaArticoli();
            lbl_selezionareArticolo.Visible = true;
            ViewState["listaDatiArticoliLavorazione"] = null;
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