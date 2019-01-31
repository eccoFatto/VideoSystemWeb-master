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
        public List<Tipologica> listaRisorse;
        public List<Tipologica> listaStati;
        public List<DatiAgenda> listaDatiAgenda;
        public List<Tipologica> listaTipiUtente;
        public List<Tipologica> listaTipiTipologie;
        public List<Tipologica> listaQualifiche;

        public Esito caricaListeTipologiche()
        {
            Esito esito = new Esito();
           
            listaRisorse = Agenda_BLL.Instance.CaricaColonne(ref esito);
                //UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_COLONNE_AGENDA, ref esito); //Tipologie.getListaRisorse();

            if (esito.codice != Esito.ESITO_OK)
            {
                return esito;
            }
            listaStati = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_STATO, ref esito);

            if (esito.codice != Esito.ESITO_OK)
            {
                return esito;
            }
            listaTipiUtente = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_UTENTE, ref esito);

            if (esito.codice != Esito.ESITO_OK)
            {
                return esito;
            }
            listaTipiTipologie = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_TIPOLOGIE, ref esito);

            if (esito.codice != Esito.ESITO_OK)
            {
                return esito;
            }
            listaQualifiche = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_QUALIFICHE, ref esito);

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
    }
}