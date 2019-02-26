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

            if (!IsPostBack)
            {
                basePage.listaClientiFornitori = Anag_Clienti_Fornitori_BLL.Instance.CaricaListaAziende(ref esito).Where(x => x.Cliente == true).ToList<Anag_Clienti_Fornitori>();
                ViewState["listaClientiFornitori"] = basePage.listaClientiFornitori;

                basePage.PopolaDDLTipologica(elencoRisorse, basePage.listaRisorse);
                basePage.PopolaDDLTipologica(elencoTipologie, basePage.listaTipiTipologie);
                basePage.PopolaDDLGenerico(elencoClienti, basePage.listaClientiFornitori);
            }
            string[] elencoProduzioni = Agenda_BLL.Instance.CaricaElencoProduzioni(ref esito);
            string[] elencoLavorazioni = Agenda_BLL.Instance.CaricaElencoLavorazioni(ref esito);

            ScriptManager.RegisterStartupScript(this, typeof(Page), "coerenzaDate", "controlloCoerenzaDate('" + txt_DataInizioLavorazione.ClientID + "', '" + txt_DataFineLavorazione.ClientID + "');", true);
            ScriptManager.RegisterStartupScript(this, typeof(Page), "coerenzaDate2", "controlloCoerenzaDate('" + txt_DataInizioImpegno.ClientID + "', '" + txt_DataFineImpegno.ClientID + "');", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "setElenchi", "setElenchi(" + Newtonsoft.Json.JsonConvert.SerializeObject(elencoProduzioni) + ", " + Newtonsoft.Json.JsonConvert.SerializeObject(elencoLavorazioni) + ");", true);
        }

        public Esito CreaOggettoSalvataggio(ref DatiAgenda datiAgenda)
        {
            Esito esito = new Esito();

            bool campoObbligatorio = int.Parse(hf_IdStato.Value) != DatiAgenda.STATO_RIPOSO;

            datiAgenda.data_inizio_lavorazione = BasePage.ValidaCampo(txt_DataInizioLavorazione, DateTime.MinValue, true, ref esito);
            datiAgenda.data_fine_lavorazione = BasePage.ValidaCampo(txt_DataFineLavorazione, DateTime.MinValue, true, ref esito);
            datiAgenda.durata_lavorazione = BasePage.ValidaCampo(txt_DurataLavorazione, 0, campoObbligatorio, ref esito);
            datiAgenda.id_colonne_agenda = BasePage.ValidaCampo(ddl_Risorse, hf_Risorse, 0, true, ref esito);
            datiAgenda.id_tipologia = BasePage.ValidaCampo(ddl_Tipologie, hf_Tipologie, 0, campoObbligatorio, ref esito);
            datiAgenda.id_stato = int.Parse(hf_IdStato.Value);
            datiAgenda.id_cliente = BasePage.ValidaCampo(ddl_Clienti, hf_Clienti, 0, true, ref esito);
            datiAgenda.durata_viaggio_andata = BasePage.ValidaCampo(txt_DurataViaggioAndata, 0, campoObbligatorio, ref esito);
            datiAgenda.durata_viaggio_ritorno = BasePage.ValidaCampo(txt_DurataViaggioRitorno, 0, campoObbligatorio, ref esito);
            datiAgenda.data_inizio_impegno = BasePage.ValidaCampo(txt_DataInizioImpegno, DateTime.MinValue, campoObbligatorio, ref esito);
            datiAgenda.data_fine_impegno = BasePage.ValidaCampo(txt_DataFineImpegno, DateTime.MinValue, campoObbligatorio, ref esito);
            //datiAgenda.impegnoOrario = chk_ImpegnoOrario.Checked;
            //datiAgenda.impegnoOrario_da = BasePage.validaCampo(txt_ImpegnoOrarioDa, "", chk_ImpegnoOrario.Checked && campoObbligatorio, ref esito);
            //datiAgenda.impegnoOrario_a = BasePage.validaCampo(txt_ImpegnoOrarioA, "", chk_ImpegnoOrario.Checked && campoObbligatorio, ref esito);
            datiAgenda.produzione = BasePage.ValidaCampo(txt_Produzione, "", campoObbligatorio, ref esito);
            datiAgenda.lavorazione = BasePage.ValidaCampo(txt_Lavorazione, "", false, ref esito);
            datiAgenda.indirizzo = BasePage.ValidaCampo(txt_Indirizzo, "", false, ref esito);
            datiAgenda.luogo = BasePage.ValidaCampo(txt_Luogo, "", campoObbligatorio, ref esito);
            datiAgenda.codice_lavoro = BasePage.ValidaCampo(txt_CodiceLavoro, "", false, ref esito);
            datiAgenda.nota = BasePage.ValidaCampo(tb_Nota, "", false, ref esito);

            return esito;
        }

        public void NascondiErroriValidazione()
        {
            txt_DataInizioLavorazione.CssClass = txt_DataInizioLavorazione.CssClass.Replace("erroreValidazione", "");
            txt_DataFineLavorazione.CssClass = txt_DataFineLavorazione.CssClass.Replace("erroreValidazione", "");
            txt_DurataLavorazione.CssClass = txt_DurataLavorazione.CssClass.Replace("erroreValidazione", "");
            ddl_Risorse.CssClass = ddl_Risorse.CssClass.Replace("erroreValidazione", "");
            ddl_Tipologie.CssClass = ddl_Tipologie.CssClass.Replace("erroreValidazione", "");
            ddl_Clienti.CssClass = ddl_Clienti.CssClass.Replace("erroreValidazione", "");
            txt_DurataViaggioAndata.CssClass = txt_DurataViaggioAndata.CssClass.Replace("erroreValidazione", "");
            txt_DurataViaggioRitorno.CssClass = txt_DurataViaggioRitorno.CssClass.Replace("erroreValidazione", "");
            txt_DataInizioImpegno.CssClass = txt_DataInizioImpegno.CssClass.Replace("erroreValidazione", "");
            txt_DataFineImpegno.CssClass = txt_DataFineImpegno.CssClass.Replace("erroreValidazione", "");
            //txt_ImpegnoOrarioDa.CssClass = txt_ImpegnoOrarioDa.CssClass.Replace("erroreValidazione", "");
            //txt_ImpegnoOrarioA.CssClass = txt_ImpegnoOrarioA.CssClass.Replace("erroreValidazione", "");
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
            
            hf_Risorse.Value = evento.id_colonne_agenda.ToString();
            ddl_Risorse.Text = ddl_Risorse.ToolTip = basePage.listaRisorse.Where(x=>x.id == evento.id_colonne_agenda).FirstOrDefault().nome;

            if (evento.id_tipologia == 0 || evento.id_tipologia == null)
            {
                hf_Tipologie.Value = "";
                ddl_Tipologie.Text = "<Seleziona>";
            }
            else
            {
                hf_Tipologie.Value = evento.id_tipologia.ToString();
                ddl_Tipologie.Text = ddl_Tipologie.ToolTip = basePage.listaTipiTipologie.Where(x => x.id == evento.id_tipologia).FirstOrDefault().nome;
            }

            if (evento.id_cliente == 0)
            {
                hf_Clienti.Value = "";
                ddl_Clienti.Text = "<Seleziona>";
            }
            else
            {
                hf_Clienti.Value = evento.id_cliente.ToString();
                ddl_Clienti.Text = ddl_Clienti.ToolTip = ((List<Anag_Clienti_Fornitori>)ViewState["listaClientiFornitori"]).Where(x => x.Id == evento.id_cliente).FirstOrDefault().RagioneSociale;
            }

            txt_DurataViaggioAndata.Text = evento.durata_viaggio_andata.ToString();
            txt_DurataViaggioRitorno.Text = evento.durata_viaggio_ritorno.ToString();
            txt_DataInizioImpegno.Text = evento.data_inizio_impegno.ToString();
            txt_DataFineImpegno.Text = evento.data_fine_impegno.ToString();
            //chk_ImpegnoOrario.Checked = evento.impegnoOrario;
            //txt_ImpegnoOrarioDa.Text = evento.impegnoOrario_da;
            //txt_ImpegnoOrarioA.Text = evento.impegnoOrario_a;
            txt_Produzione.Text = evento.produzione;
            txt_Lavorazione.Text = evento.lavorazione;
            txt_Indirizzo.Text = evento.indirizzo;
            txt_Luogo.Text = evento.luogo;
            txt_CodiceLavoro.Text = evento.codice_lavoro;
            tb_Nota.Text = evento.nota;

            hf_IdStato.Value = evento.id_stato.ToString();
            Esito esito = new Esito();
            txt_Stato.Text = UtilityTipologiche.getElementByID(basePage.listaStati, evento.id_stato, ref esito).nome;

            //txt_ImpegnoOrarioDa.Enabled = txt_ImpegnoOrarioA.Enabled = evento.impegnoOrario;
        }

        public void ClearAppuntamento()
        {
            txt_DataInizioLavorazione.Text = string.Empty;
            txt_DataFineLavorazione.Text = string.Empty;
            txt_DurataLavorazione.Text = string.Empty;

            //ddl_Risorse.SelectedValue = "";
            hf_Risorse.Value = "";
            ddl_Risorse.Text = ddl_Risorse.ToolTip = "";

            hf_Tipologie.Value = "";
            ddl_Tipologie.Text = ddl_Tipologie.ToolTip = "";

            hf_Clienti.Value = "";
            ddl_Clienti.Text = ddl_Clienti.ToolTip = "";

            txt_DurataViaggioAndata.Text = string.Empty;
            txt_DurataViaggioRitorno.Text = string.Empty;
            txt_DataInizioImpegno.Text = string.Empty;
            txt_DataFineImpegno.Text = string.Empty;
            //chk_ImpegnoOrario.Checked = false;
            //txt_ImpegnoOrarioDa.Text = string.Empty;
            //txt_ImpegnoOrarioA.Text = string.Empty;
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

            panelAppuntamenti.Enabled = basePage.AbilitazioneInScrittura();

            if (basePage.AbilitazioneInScrittura())
            {
                txt_DataInizioLavorazione.Enabled = evento.id_stato == DatiAgenda.STATO_PREVISIONE_IMPEGNO || evento.id_stato ==  DatiAgenda.STATO_RIPOSO;
                ddl_Clienti.Enabled = evento.id_stato == DatiAgenda.STATO_PREVISIONE_IMPEGNO || evento.id_stato == DatiAgenda.STATO_RIPOSO;
                txt_Produzione.Enabled = evento.id_stato == DatiAgenda.STATO_PREVISIONE_IMPEGNO || evento.id_stato == DatiAgenda.STATO_RIPOSO;
                txt_Lavorazione.Enabled = evento.id_stato == DatiAgenda.STATO_PREVISIONE_IMPEGNO || evento.id_stato == DatiAgenda.STATO_RIPOSO;
                txt_Indirizzo.Enabled = evento.id_stato == DatiAgenda.STATO_PREVISIONE_IMPEGNO || evento.id_stato == DatiAgenda.STATO_RIPOSO;
                txt_Luogo.Enabled = evento.id_stato == DatiAgenda.STATO_PREVISIONE_IMPEGNO || evento.id_stato == DatiAgenda.STATO_RIPOSO;
                txt_CodiceLavoro.Enabled = evento.id_stato == DatiAgenda.STATO_PREVISIONE_IMPEGNO || evento.id_stato == DatiAgenda.STATO_RIPOSO;
            }
        }

        
    }
}