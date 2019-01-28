using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;

namespace VideoSystemWeb.BLL
{
    public static class UtilityTipologiche
    {

        public static List<Tipologica> caricaTipologica(EnumTipologiche tipologica, ref Esito esito)
        {
            List<Tipologica> listaTipologiche = new List<Tipologica>();

            if (HttpContext.Current.Session[tipologica.ToString()] == null)
            {
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["USA_DB"]))
                {
                    listaTipologiche = Base_DAL.CaricaTipologica(tipologica, true, ref esito);
                }
                else
                {
                    listaTipologiche = Tipologie.caricaTipologica(tipologica);
                }

                HttpContext.Current.Session[tipologica.ToString()] = listaTipologiche;
            }
            else
            {
                listaTipologiche = (List<Tipologica>)HttpContext.Current.Session[tipologica.ToString()];
            }

            return listaTipologiche;
        }

        public static string getNomeTipologica(EnumTipologiche tipologica)
        {
            switch (tipologica)
            {
                case EnumTipologiche.TIPO_COLONNE_AGENDA:
                    return "tipo_colonne_agenda";
                case EnumTipologiche.TIPO_QUALIFICHE:
                    return "tipo_qualifiche";
                case EnumTipologiche.TIPO_UTENTE:
                    return "tipo_utente";
                case EnumTipologiche.TIPO_STATO:
                    return "tipo_stato";
                case EnumTipologiche.TIPO_TIPOLOGIE:
                    return "tipo_tipologie";
                default:
                    return string.Empty;
            }
        }

        public static Tipologica getElementByID(List<Tipologica> listaTipologiche, int id, ref Esito esito)
        {
            Tipologica tipologica = new Tipologica();

            tipologica = listaTipologiche.Where(x => x.id == id).FirstOrDefault();
            if (tipologica == null)
            {
                esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                esito.descrizione = "Nessuna Tipologica trovata per id = '"+id+"'";
            }

            return tipologica;
        }

        public static string getParametroDaTipologica(Tipologica tipologica, string nomeParametro, ref Esito esito)
        {
            string valoreParametro = string.Empty;

            string[] elencoParametri = tipologica.parametri.Split(';');

            valoreParametro = string.Empty;
            foreach (string param in elencoParametri)
            {
                if (param.ToUpper().StartsWith(nomeParametro.ToUpper()))
                {
                    int index = param.IndexOf("=");
                    valoreParametro = param.Substring(index+1);
                    break;
                }
            }

            if (string.IsNullOrEmpty(valoreParametro))
            {
                esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                esito.descrizione = "Nessun parametro con nome = '" + nomeParametro + "'";
            }

            return valoreParametro;
        }

    }

    public enum EnumTipologiche { TIPO_COLONNE_AGENDA, TIPO_QUALIFICHE, TIPO_UTENTE, TIPO_STATO, TIPO_TIPOLOGIE }
}