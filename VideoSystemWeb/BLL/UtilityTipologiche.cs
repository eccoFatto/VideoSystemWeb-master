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

        public static List<Tipologica> caricaTipologica(EnumTipologiche tipologica)
        {
            Esito esito = new Esito();
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
                case EnumTipologiche.TIPO_CLIENTI_FORNITORI:
                    return "tipo_clienti_fornitori";
                case EnumTipologiche.TIPO_GENERE:
                    return "tipo_genere";
                case EnumTipologiche.TIPO_GRUPPO:
                    return "tipo_gruppo";
                case EnumTipologiche.TIPO_SOTTOGRUPPO:
                    return "tipo_sottogruppo";
                case EnumTipologiche.TIPO_TENDER:
                    return "tipo_tender";
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

        public static Tipologica getElementByNome(List<Tipologica> listaTipologiche, string nome, ref Esito esito)
        {
            Tipologica tipologica = new Tipologica();

            tipologica = listaTipologiche.Where(x => x.nome.ToUpper().Trim() == nome.ToUpper().Trim()).FirstOrDefault();
            if (tipologica == null)
            {
                esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                esito.descrizione = "Nessuna Tipologica trovata per nome = '" + nome + "'";
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

        public static List<Tipologica> CaricaTipologica(EnumTipologiche tipologica, bool soloElemAttivi, ref Esito esito)
        {
            List<Tipologica> lista = Base_DAL.CaricaTipologica(tipologica, soloElemAttivi, ref esito);
            return lista;
        }

        public static Tipologica getTipologicaById(EnumTipologiche eTipo, int idTipologica, ref Esito esito)
        {
            Tipologica tipologica = Base_DAL.getTipologicaById(eTipo, idTipologica, ref esito);
            return tipologica;
        }

        public static int CreaTipologia(EnumTipologiche tipoTipologica, Tipologica tipologica, ref Esito esito)
        {
            int iRet = Base_DAL.CreaTipologia(tipoTipologica, tipologica, ref esito);
            return iRet;
        }

        public static Esito AggiornaTipologia(EnumTipologiche tipoTipologica, Tipologica tipologica)
        {
            Esito esito = Base_DAL.AggiornaTipologia(tipoTipologica, tipologica);
            return esito;
        }

        public static Esito EliminaTipologia(EnumTipologiche tipoTipologica, int idTipologica)
        {
            Esito esito = Base_DAL.EliminaTipologia(tipoTipologica, idTipologica);
            return esito;
        }
 }



    public enum EnumTipologiche { TIPO_COLONNE_AGENDA, TIPO_QUALIFICHE, TIPO_UTENTE, TIPO_STATO, TIPO_TIPOLOGIE, TIPO_CLIENTI_FORNITORI, TIPO_GENERE, TIPO_GRUPPO, TIPO_SOTTOGRUPPO, TIPO_TENDER }

    public enum EnumSottotipiRisorse {DIPENDENTI, REGIE, EXTRA}
}