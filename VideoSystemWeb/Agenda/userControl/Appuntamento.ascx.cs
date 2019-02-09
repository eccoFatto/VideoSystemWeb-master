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

            ScriptManager.RegisterStartupScript(this, typeof(Page), "campiImpegnoOrario", "checkImpegnoOrario();", true);
            ScriptManager.RegisterStartupScript(this, typeof(Page), "coerenzaDate", "controlloCoerenzaDate('" + txt_DataInizioLavorazione.ClientID + "', '" + txt_DataFineLavorazione.ClientID + "');", true);
            ScriptManager.RegisterStartupScript(this, typeof(Page), "coerenzaDate2", "controlloCoerenzaDate('" + txt_DataInizioImpegno.ClientID + "', '" + txt_DataFineImpegno.ClientID + "');", true);

        }

        public Esito CreaOggettoSalvataggio(ref DatiAgenda datiAgenda)
        {
            Esito esito = new Esito();
           
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

            datiAgenda.id_stato = int.Parse(hf_IdStato.Value);

            return esito;
        }

        public void NascondiErroriValidazione()
        {
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

        public void PopolaPopup(DatiAgenda evento)
        {
            txt_DataInizioLavorazione.Text = evento.data_inizio_lavorazione.ToString("dd/MM/yyyy");
            txt_DataFineLavorazione.Text = evento.data_fine_lavorazione == DateTime.MinValue ? "" : evento.data_fine_lavorazione.ToString("dd/MM/yyyy");
            txt_DurataLavorazione.Text = evento.durata_lavorazione.ToString();
            ddl_Risorse.SelectedValue = evento.id_colonne_agenda.ToString();
            ddl_Tipologia.SelectedValue = evento.id_tipologia == 0 ? "": evento.id_tipologia.ToString();
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

            hf_IdStato.Value = evento.id_stato.ToString();
            Esito esito = new Esito();
            txt_Stato.Text = UtilityTipologiche.getElementByID(basePage.listaStati, evento.id_stato, ref esito).nome;

            txt_ImpegnoOrarioDa.Enabled = txt_ImpegnoOrarioA.Enabled = evento.impegnoOrario;
        }

        public void ClearPopupEventi()
        {
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

        public void SetStato(int stato)
        {
            Esito esito = new Esito();
            hf_IdStato.Value = stato.ToString();
            txt_Stato.Text = UtilityTipologiche.getElementByID(basePage.listaStati, stato, ref esito).nome;
        }

        public void AbilitaComponentiPopup(DatiAgenda evento)
        {
            txt_DataInizioLavorazione.Enabled = evento.id_stato == DatiAgenda.STATO_PREVISIONE_IMPEGNO;
            ddl_cliente.Enabled = evento.id_stato == DatiAgenda.STATO_PREVISIONE_IMPEGNO;
            txt_Produzione.Enabled = evento.id_stato == DatiAgenda.STATO_PREVISIONE_IMPEGNO;
            txt_Lavorazione.Enabled = evento.id_stato == DatiAgenda.STATO_PREVISIONE_IMPEGNO;
            txt_Indirizzo.Enabled = evento.id_stato == DatiAgenda.STATO_PREVISIONE_IMPEGNO;
            txt_Luogo.Enabled = evento.id_stato == DatiAgenda.STATO_PREVISIONE_IMPEGNO;
            txt_CodiceLavoro.Enabled = evento.id_stato == DatiAgenda.STATO_PREVISIONE_IMPEGNO;
        }
    }
}