using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoSystemWeb.Entity;

namespace VideoSystemWeb.BLL
{
    public class BasePage: System.Web.UI.Page
    {
        public List<Tipologica> listaRisorse
        {
            get
            {
                List <Tipologica> _listaRisorse = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_COLONNE_AGENDA);
                _listaRisorse = _listaRisorse.OrderBy(c => c.sottotipo == "dipendenti").ThenBy(c => c.sottotipo=="extra").ThenBy(c => c.sottotipo).ToList<Tipologica>();
                return _listaRisorse;
            }
        }
        public List<Tipologica> listaStati
        {
            get
            {
                return UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_STATO);
            }
        }

        public List<DatiAgenda> listaDatiAgenda;

        public List<Tipologica> listaTipiUtente
        {
            get
            {
                return UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_UTENTE);
            }
        }
        public List<Tipologica> listaTipiTipologie
        {
            get
            {
                return UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_TIPOLOGIE);
            }
        }
        public List<Tipologica> listaQualifiche
        {
            get
            {
                return UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_QUALIFICHE);
            }
        }

        public List<Tipologica> listaTipiClientiFornitori
        {
            get
            {
                return UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_CLIENTI_FORNITORI);
            }
        }

        public Esito caricaListeTipologiche()
        {
            Esito esito = new Esito();

            List<Tipologica> listaRisorse = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_COLONNE_AGENDA); //Agenda_BLL.Instance.CaricaColonne(ref esito);
            //UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_COLONNE_AGENDA, ref esito); //Tipologie.getListaRisorse();

            List<Tipologica> listaStati = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_STATO);

            List<Tipologica> listaTipiUtente = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_UTENTE);

            List<Tipologica> listaTipiTipologie = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_TIPOLOGIE);

            List<Tipologica> listaQualifiche = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_QUALIFICHE);

            List<Tipologica> listaTipiClientiFornitori = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_CLIENTI_FORNITORI);

            return esito;
        }

        public static T validaCampo<T>(WebControl campo, T defaultValue, bool isRequired, ref Esito esito) 
        {
            T result = defaultValue;

            string valore = "";

            if (campo is TextBox)
            {
                valore = ((TextBox)campo).Text;
            }
            else if (campo is DropDownList)
            {
                valore = ((DropDownList)campo).SelectedValue;
            }
            else if (campo is CheckBox)
            {
                valore = Convert.ToString(((CheckBox)campo).Checked);
            }

            if (isRequired && string.IsNullOrEmpty(valore))
            {
                campo.CssClass += " erroreValidazione";
                esito.codice = Esito.ESITO_KO_ERRORE_VALIDAZIONE;
                esito.descrizione = "Campo obbligatorio";
            }
            else
            {
                try
                {
                    if (!isRequired && string.IsNullOrEmpty(valore))
                    {
                        valore = defaultValue.ToString();
                    }

                    campo.CssClass = campo.CssClass.Replace("erroreValidazione", "");
                    result = (T)Convert.ChangeType(valore, typeof(T));
                }
                catch
                {
                    campo.CssClass += " erroreValidazione";
                    esito.codice = Esito.ESITO_KO_ERRORE_VALIDAZIONE;
                    esito.descrizione = "Controllare il campo";
                }
            }

            return result;
        }

        public bool AbilitazioneInScrittura()
        {
            Esito esito = new Esito();

            bool abilitazioneScrittura = false;

            int idUtente = ((Anag_Utenti)HttpContext.Current.Session[SessionManager.UTENTE]).id_tipoUtente;
            Tipologica tipoUtenteLoggato = UtilityTipologiche.getElementByID(UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_UTENTE), idUtente, ref esito);
            abilitazioneScrittura = UtilityTipologiche.getParametroDaTipologica(tipoUtenteLoggato, "SCRITTURA", ref esito) == "1";

            return abilitazioneScrittura;
        }

        public void popolaDDLTipologica(DropDownList ddl, List<Tipologica> listaTipologica)
        {
            ddl.Items.Add(new ListItem("<seleziona>", "", true));
            foreach (Tipologica tipologica in listaTipologica)
            {
                ddl.Items.Add(new ListItem(tipologica.nome, tipologica.id.ToString(), true));
            }
        }

    }
}
