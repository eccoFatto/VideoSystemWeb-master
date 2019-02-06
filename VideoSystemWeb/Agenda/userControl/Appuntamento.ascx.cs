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
    public partial class Appuntamento : System.Web.UI.UserControl
    {
        BasePage basePage = new BasePage();
        public delegate void PopupHandler(string operazionePopup); // delegato per l'evento
        public event PopupHandler RichiediOperazionePopup; //evento

        protected void Page_Load(object sender, EventArgs e)
        {
            Esito esito = new Esito();
            basePage.listaDatiAgenda = Agenda_BLL.Instance.CaricaDatiAgenda(ref esito);

            if (!IsPostBack)
            {
                basePage.popolaDDLTipologica(ddl_Risorse, basePage.listaRisorse);
                basePage.popolaDDLTipologica(ddl_Tipologia, basePage.listaTipiTipologie);
            }
        }

        #region COMPORTAMENTO ELEMENTI PAGINA
        protected void btnModifica_Click(object sender, EventArgs e)
        {
            panelErrore.Style.Add("display", "none");
            PopolaEditPopupEventi((DatiAgenda)ViewState["eventoSelezionato"]);
            AttivaDisattivaModifica(true);
        }

        protected void btnSalva_Click(object sender, EventArgs e)
        {
            Esito esito = new Esito();
            DatiAgenda datiAgenda = CreaOggettoSalvataggio(ref esito);

            if (esito.codice != Esito.ESITO_OK)
            {
                panelErrore.Style.Remove("display");
                lbl_MessaggioErrore.Text = "Controllare i campi evidenziati";
                UpdatePopup();
            }
            else
            {
                NascondiErroriValidazione();
                if (IsDisponibileDataRisorsa(datiAgenda.data_inizio_lavorazione, datiAgenda.data_fine_lavorazione, datiAgenda))
                {
                    if (datiAgenda.id == 0)
                    {
                        Agenda_BLL.Instance.CreaEvento(datiAgenda);
                    }
                    else
                    {
                        Agenda_BLL.Instance.AggiornaEvento(datiAgenda);
                    }
                    RichiediOperazionePopup("CLOSE");
                }
                else
                {
                    panelErrore.Style.Remove("display");
                    lbl_MessaggioErrore.Text = "Non è possibile salvare l'evento perché la risorsa è già impiegata nel periodo selezionato";
                    UpdatePopup();
                }
            }
        }

        protected void btnAnnulla_Click(object sender, EventArgs e)
        {
            NascondiErroriValidazione();
            AttivaDisattivaModifica(false);
        }

        protected void btnElimina_Click(object sender, EventArgs e)
        {
            panelErrore.Style.Add("display", "none");

            Agenda_BLL.Instance.EliminaEvento((int)ViewState["idEvento"]);

            RichiediOperazionePopup("CLOSE");
        }

        protected void btnOfferta_Click(object sender, EventArgs e)
        {
            RichiediOperazionePopup("OFFERTA");
        }

        protected void btnLavorazione_Click(object sender, EventArgs e)
        {
            RichiediOperazionePopup("LAVORAZIONE");
        }
        #endregion

        public void EditEvent(DateTime dataEvento, int risorsaEvento)
        {
            bool isUtenteAbilitatoInScrittura = basePage.AbilitazioneInScrittura();

            ClearPopupEventi();

            DatiAgenda eventoSelezionato = Agenda_BLL.Instance.GetDatiAgendaByDataRisorsa(basePage.listaDatiAgenda, dataEvento, risorsaEvento);
            ViewState["eventoSelezionato"] = eventoSelezionato;

            if (eventoSelezionato == null)
            {
                ViewState["idEvento"] = 0;
                AttivaDisattivaModifica(true);
                btnAnnulla.Visible = false;
                txt_DataInizioLavorazione.Text = dataEvento.ToString("dd/MM/yyyy");
                ddl_Risorse.SelectedValue = risorsaEvento.ToString();
            }
            else
            {
                ViewState["idEvento"] = eventoSelezionato.id;
                AttivaDisattivaModifica(false);
                PopolaPopupEventi(eventoSelezionato);

                btnModifica.Visible = isUtenteAbilitatoInScrittura;

                
            }

            ScriptManager.RegisterStartupScript(this, typeof(Page), "campiImpegnoOrario", "checkImpegnoOrario();", true);
            ScriptManager.RegisterStartupScript(this, typeof(Page), "coerenzaDate", "controlloCoerenzaDate('" + txt_DataInizioLavorazione.ClientID + "', '" + txt_DataFineLavorazione.ClientID + "');", true);
            ScriptManager.RegisterStartupScript(this, typeof(Page), "coerenzaDate2", "controlloCoerenzaDate('" + txt_DataInizioImpegno.ClientID + "', '" + txt_DataFineImpegno.ClientID + "');", true);
        }

        private DatiAgenda CreaOggettoSalvataggio(ref Esito esito)
        {
            DatiAgenda datiAgenda = new DatiAgenda();
            datiAgenda.id = (int)ViewState["idEvento"];
            datiAgenda.data_inizio_lavorazione = BasePage.validaCampo(txt_DataInizioLavorazione, DateTime.Now, true, ref esito);
            datiAgenda.data_fine_lavorazione = BasePage.validaCampo(txt_DataFineLavorazione, DateTime.Now, true, ref esito);
            datiAgenda.durata_lavorazione = BasePage.validaCampo(txt_DurataLavorazione, 0, false, ref esito);
            datiAgenda.id_colonne_agenda = BasePage.validaCampo(ddl_Risorse, 0, true, ref esito);
            datiAgenda.id_tipologia = BasePage.validaCampo(ddl_Tipologia, 0, true, ref esito);
            datiAgenda.id_cliente = BasePage.validaCampo(ddl_cliente, 0, false, ref esito);
            datiAgenda.durata_viaggio_andata = BasePage.validaCampo(txt_DurataViaggioAndata, 0, false, ref esito);
            datiAgenda.durata_viaggio_ritorno = BasePage.validaCampo(txt_DurataViaggioRitorno, 0, false, ref esito);
            datiAgenda.data_inizio_impegno = BasePage.validaCampo(txt_DataInizioImpegno, DateTime.MinValue, false, ref esito);
            datiAgenda.data_fine_impegno = BasePage.validaCampo(txt_DataFineImpegno, DateTime.MinValue, false, ref esito);
            datiAgenda.impegnoOrario = chk_ImpegnoOrario.Checked;
            datiAgenda.impegnoOrario_da = BasePage.validaCampo(txt_ImpegnoOrarioDa, "", chk_ImpegnoOrario.Checked, ref esito);
            datiAgenda.impegnoOrario_a = BasePage.validaCampo(txt_ImpegnoOrarioA, "", chk_ImpegnoOrario.Checked, ref esito);
            datiAgenda.produzione = BasePage.validaCampo(txt_Produzione, "", true, ref esito);
            datiAgenda.lavorazione = BasePage.validaCampo(txt_Lavorazione, "", false, ref esito);
            datiAgenda.indirizzo = BasePage.validaCampo(txt_Indirizzo, "", false, ref esito);
            datiAgenda.luogo = BasePage.validaCampo(txt_Luogo, "", false, ref esito);
            datiAgenda.codice_lavoro = BasePage.validaCampo(txt_CodiceLavoro, "", false, ref esito);
            datiAgenda.nota = BasePage.validaCampo(tb_Nota, "", false, ref esito);

            return datiAgenda;
        }

        private void NascondiErroriValidazione()
        {
            panelErrore.Style.Add("display", "none");

            txt_DataInizioLavorazione.CssClass = txt_DataInizioLavorazione.CssClass.Replace("erroreValidazione", "");
            txt_DataFineLavorazione.CssClass = txt_DataFineLavorazione.CssClass.Replace("erroreValidazione", "");
            txt_DurataLavorazione.CssClass = txt_DurataLavorazione.CssClass.Replace("erroreValidazione", "");
            ddl_Risorse.CssClass = ddl_Risorse.CssClass.Replace("erroreValidazione", "");
            ddl_Tipologia.CssClass = ddl_Tipologia.CssClass.Replace("erroreValidazione", "");
            ddl_cliente.CssClass = ddl_cliente.CssClass.Replace("erroreValidazione", "");
            txt_DurataViaggioAndata.CssClass = txt_DurataViaggioAndata.CssClass.Replace("erroreValidazione", "");
            txt_DurataViaggioRitorno.CssClass = txt_DurataViaggioRitorno.CssClass.Replace("erroreValidazione", "");
            txt_DataInizioImpegno.CssClass = txt_DataInizioImpegno.CssClass.Replace("erroreValidazione", "");
            txt_DataFineImpegno.CssClass = txt_DataFineImpegno.CssClass.Replace("erroreValidazione", "");
            txt_ImpegnoOrarioDa.CssClass = txt_ImpegnoOrarioDa.CssClass.Replace("erroreValidazione", "");
            txt_ImpegnoOrarioA.CssClass = txt_ImpegnoOrarioA.CssClass.Replace("erroreValidazione", "");
            txt_Produzione.CssClass = txt_Produzione.CssClass.Replace("erroreValidazione", "");
            txt_Lavorazione.CssClass = txt_Lavorazione.CssClass.Replace("erroreValidazione", "");
            txt_Indirizzo.CssClass = txt_Indirizzo.CssClass.Replace("erroreValidazione", "");
            txt_Luogo.CssClass = txt_Luogo.CssClass.Replace("erroreValidazione", "");
            txt_CodiceLavoro.CssClass = txt_CodiceLavoro.CssClass.Replace("erroreValidazione", "");
            tb_Nota.CssClass = tb_Nota.CssClass.Replace("erroreValidazione", "");
        }

        private void AttivaDisattivaModifica(bool attivaModifica)
        {
            val_DataInizioLavorazione.Visible = !attivaModifica;
            txt_DataInizioLavorazione.Visible = attivaModifica;
            val_DataFineLavorazione.Visible = !attivaModifica;
            txt_DataFineLavorazione.Visible = attivaModifica;
            val_DurataLavorazione.Visible = !attivaModifica;
            txt_DurataLavorazione.Visible = attivaModifica;
            val_Risorse.Visible = !attivaModifica;
            ddl_Risorse.Visible = attivaModifica;
            val_Tipologia.Visible = !attivaModifica;
            ddl_Tipologia.Visible = attivaModifica;
            val_cliente.Visible = !attivaModifica;
            ddl_cliente.Visible = attivaModifica;
            val_DurataViaggioAndata.Visible = !attivaModifica;
            txt_DurataViaggioAndata.Visible = attivaModifica;
            val_DurataViaggioRitorno.Visible = !attivaModifica;
            txt_DurataViaggioRitorno.Visible = attivaModifica;
            val_DataInizioImpegno.Visible = !attivaModifica;
            txt_DataInizioImpegno.Visible = attivaModifica;
            val_DataFineImpegno.Visible = !attivaModifica;
            txt_DataFineImpegno.Visible = attivaModifica;
            val_ImpegnoOrario.Visible = !attivaModifica;
            chk_ImpegnoOrario.Visible = attivaModifica;
            val_ImpegnoOrarioDa.Visible = !attivaModifica;
            txt_ImpegnoOrarioDa.Visible = attivaModifica;
            val_ImpegnoOrarioA.Visible = !attivaModifica;
            txt_ImpegnoOrarioA.Visible = attivaModifica;
            val_Produzione.Visible = !attivaModifica;
            txt_Produzione.Visible = attivaModifica;
            val_Lavorazione.Visible = !attivaModifica;
            txt_Lavorazione.Visible = attivaModifica;
            val_Indirizzo.Visible = !attivaModifica;
            txt_Indirizzo.Visible = attivaModifica;
            val_Luogo.Visible = !attivaModifica;
            txt_Luogo.Visible = attivaModifica;
            val_CodiceLavoro.Visible = !attivaModifica;
            txt_CodiceLavoro.Visible = attivaModifica;
            val_Nota.Visible = !attivaModifica;
            tb_Nota.Visible = attivaModifica;

            btnModifica.Visible = btnElimina.Visible = !attivaModifica;
            btnSalva.Visible = btnAnnulla.Visible = attivaModifica;

            
            GestionePulsantiStato(attivaModifica);

            UpdatePopup();
        }

        private void GestionePulsantiStato(bool attivaModifica)
        {
            DatiAgenda eventoCorrente = (DatiAgenda)ViewState["eventoSelezionato"];
            btnOfferta.Visible = !attivaModifica && eventoCorrente != null && eventoCorrente.id_stato == 1;
            btnLavorazione.Visible = !attivaModifica && eventoCorrente != null && eventoCorrente.id_stato == 2;
        }

        private void PopolaPopupEventi(DatiAgenda evento)
        {
            Esito esito = new Esito();

            val_DataInizioLavorazione.Text = evento.data_inizio_lavorazione.ToString("dd/MM/yyyy");
            val_DataFineLavorazione.Text = evento.data_fine_lavorazione.ToString("dd/MM/yyyy");
            val_DurataLavorazione.Text = evento.durata_lavorazione.ToString();
            val_Risorse.Text = UtilityTipologiche.getElementByID(basePage.listaRisorse, evento.id_colonne_agenda, ref esito).nome;
            val_Tipologia.Text = UtilityTipologiche.getElementByID(basePage.listaTipiTipologie, evento.id_tipologia, ref esito).nome;
            val_cliente.Text = evento.id_cliente.ToString();
            val_DurataViaggioAndata.Text = evento.durata_viaggio_andata.ToString();
            val_DurataViaggioRitorno.Text = evento.durata_viaggio_ritorno.ToString();
            val_DataInizioImpegno.Text = evento.data_inizio_impegno.ToString();
            val_DataFineImpegno.Text = evento.data_fine_impegno.ToString();
            val_ImpegnoOrario.Checked = evento.impegnoOrario;
            val_ImpegnoOrarioDa.Text = evento.impegnoOrario_da;
            val_ImpegnoOrarioA.Text = evento.impegnoOrario_a;
            val_Produzione.Text = evento.produzione;
            val_Lavorazione.Text = evento.lavorazione;
            val_Indirizzo.Text = evento.indirizzo;
            val_Luogo.Text = evento.luogo;
            val_CodiceLavoro.Text = evento.codice_lavoro;
            val_Nota.Text = evento.nota;

            PopolaEditPopupEventi(evento);
        }

        private void PopolaEditPopupEventi(DatiAgenda evento)
        {
            txt_DataInizioLavorazione.Text = evento.data_inizio_lavorazione.ToString("dd/MM/yyyy");
            txt_DataFineLavorazione.Text = evento.data_fine_lavorazione.ToString("dd/MM/yyyy");
            txt_DurataLavorazione.Text = evento.durata_lavorazione.ToString();
            ddl_Risorse.SelectedValue = evento.id_colonne_agenda.ToString();
            ddl_Tipologia.SelectedValue = evento.id_tipologia.ToString();
            ddl_cliente.SelectedValue = evento.id_cliente.ToString();
            txt_DurataViaggioAndata.Text = evento.durata_viaggio_andata.ToString();
            txt_DurataViaggioRitorno.Text = evento.durata_viaggio_ritorno.ToString();
            txt_DataInizioImpegno.Text = evento.data_inizio_impegno.ToString();
            txt_DataFineImpegno.Text = evento.data_fine_impegno.ToString();
            chk_ImpegnoOrario.Checked = evento.impegnoOrario;
            txt_ImpegnoOrarioDa.Text = evento.impegnoOrario_da;
            txt_ImpegnoOrarioA.Text = evento.impegnoOrario_a;
            txt_Produzione.Text = evento.produzione;
            txt_Lavorazione.Text = evento.lavorazione;
            txt_Indirizzo.Text = evento.indirizzo;
            txt_Luogo.Text = evento.luogo;
            txt_CodiceLavoro.Text = evento.codice_lavoro;
            tb_Nota.Text = evento.nota;

            txt_ImpegnoOrarioDa.Enabled = txt_ImpegnoOrarioA.Enabled = evento.impegnoOrario;
        }

        private void ClearPopupEventi()
        {
            lbl_MessaggioErrore.Text = string.Empty;
            panelErrore.Style.Add("display", "none");

            val_DataInizioLavorazione.Text = string.Empty;
            val_DataFineLavorazione.Text = string.Empty;
            val_DurataLavorazione.Text = string.Empty;
            val_Risorse.Text = string.Empty;
            val_Tipologia.Text = string.Empty;
            val_cliente.Text = string.Empty;
            val_DurataViaggioAndata.Text = string.Empty;
            val_DurataViaggioRitorno.Text = string.Empty;
            val_DataInizioImpegno.Text = string.Empty;
            val_DataFineImpegno.Text = string.Empty;
            val_ImpegnoOrario.Checked = false;
            val_ImpegnoOrarioDa.Text = string.Empty;
            val_ImpegnoOrarioA.Text = string.Empty;
            val_Produzione.Text = string.Empty;
            val_Lavorazione.Text = string.Empty;
            val_Indirizzo.Text = string.Empty;
            val_Luogo.Text = string.Empty;
            val_CodiceLavoro.Text = string.Empty;
            val_Nota.Text = string.Empty;

            txt_DataInizioLavorazione.Text = string.Empty;
            txt_DataFineLavorazione.Text = string.Empty;
            txt_DurataLavorazione.Text = string.Empty;
            ddl_Risorse.SelectedValue = "";
            ddl_Tipologia.SelectedValue = "";
            ddl_cliente.SelectedValue = "";
            txt_DurataViaggioAndata.Text = string.Empty;
            txt_DurataViaggioRitorno.Text = string.Empty;
            txt_DataInizioImpegno.Text = string.Empty;
            txt_DataFineImpegno.Text = string.Empty;
            chk_ImpegnoOrario.Checked = false;
            txt_ImpegnoOrarioDa.Text = string.Empty;
            txt_ImpegnoOrarioA.Text = string.Empty;
            txt_Produzione.Text = string.Empty;
            txt_Lavorazione.Text = string.Empty;
            txt_Indirizzo.Text = string.Empty;
            txt_Luogo.Text = string.Empty;
            txt_CodiceLavoro.Text = string.Empty;
            tb_Nota.Text = string.Empty;

            NascondiErroriValidazione();
        }

        private bool IsDisponibileDataRisorsa(DateTime inizioLavorazione, DateTime fineLavorazione, DatiAgenda eventoDaControllare)
        {
            DatiAgenda eventoEsistente = basePage.listaDatiAgenda.Where(x => x.id != eventoDaControllare.id &&
                                                         x.id_colonne_agenda == eventoDaControllare.id_colonne_agenda &&
                                                        ((x.data_inizio_lavorazione <= inizioLavorazione && x.data_fine_lavorazione >= inizioLavorazione) ||
                                                        (x.data_inizio_lavorazione <= fineLavorazione && x.data_fine_lavorazione >= fineLavorazione) ||
                                                        (x.data_inizio_lavorazione >= inizioLavorazione && x.data_fine_lavorazione <= fineLavorazione)
                                                        )).FirstOrDefault();

            return eventoEsistente == null;
        }

        private void UpdatePopup()
        {
            RichiediOperazionePopup("UPDATE");
            ScriptManager.RegisterStartupScript(this, typeof(Page), "abilitazioneImpegnoOrario", "checkImpegnoOrario();", true);
        }
    }
}