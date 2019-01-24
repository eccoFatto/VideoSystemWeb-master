using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.Entity;

namespace VideoSystemWeb.BLL
{
    public class BasePage: System.Web.UI.Page
    {
        public List<Tipologica> listaRisorse;
        public List<Tipologica> listaStati;
        public List<DatiAgenda> listaDatiAgenda;
        public List<Tipologica> listaTipiUtente;

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

            return esito;
        }
    }

    
}