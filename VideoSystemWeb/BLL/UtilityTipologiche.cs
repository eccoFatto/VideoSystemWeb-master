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

            if (HttpContext.Current.Session[tipologica.ToString()] == null || ((List<Tipologica>)HttpContext.Current.Session[tipologica.ToString()]).Count()==0)
            {
                listaTipologiche = Base_DAL.CaricaTipologica(tipologica, true, ref esito);

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
                case EnumTipologiche.TIPO_PROTOCOLLO:
                    return "tipo_protocollo";
                case EnumTipologiche.TIPO_PAGAMENTO:
                    return "tipo_pagamento";
                case EnumTipologiche.TIPO_INTERVENTO:
                    return "tipo_intervento";
                case EnumTipologiche.TIPO_CATEGORIE_MAGAZZINO:
                    return "tipo_categoria_magazzino";
                case EnumTipologiche.TIPO_SUB_CATEGORIE_MAGAZZINO:
                    return "tipo_subcategoria_magazzino";
                case EnumTipologiche.TIPO_POSIZIONE_MAGAZZINO:
                    return "tipo_posizione_magazzino";
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

        // solo per le colonne dell'agenda
        public static ColonneAgenda getElementByID(List<ColonneAgenda> listaColonne, int id, ref Esito esito)
        {
            ColonneAgenda colonna = new ColonneAgenda();

            colonna = listaColonne.Where(x => x.id == id).FirstOrDefault();
            if (colonna == null)
            {
                esito.codice = Esito.ESITO_KO_ERRORE_NO_RISULTATI;
                esito.descrizione = "Nessuna Colonna Agenda trovata per id = '" + id + "'";
            }

            return colonna;
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

        public static List<ColonneAgenda> CaricaColonneAgenda(bool soloElemAttivi, ref Esito esito)
        {
            List<ColonneAgenda> lista = Base_DAL.CaricaColonneAgenda(soloElemAttivi, ref esito);
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

        public static Esito RemoveTipologia(EnumTipologiche tipoTipologica, int idTipologica)
        {
            Esito esito = Base_DAL.RemoveTipologia(tipoTipologica, idTipologica);
            return esito;
        }

        public static ColonneAgenda getColonneAgendaById(int idColonnaAgenda, ref Esito esito)
        {
            ColonneAgenda colonnaAgenda = Base_DAL.getColonneAgendaById(idColonnaAgenda, ref esito);
            return colonnaAgenda;
        }
        public static int CreaColonneAgenda(ColonneAgenda colonnaAgenda, ref Esito esito)
        {
            int iRet = Base_DAL.CreaColonneAgenda(colonnaAgenda, ref esito);
            return iRet;
        }

        public static Esito AggiornaColonneAgenda(ColonneAgenda colonnaAgenda)
        {
            Esito esito = Base_DAL.AggiornaColonneAgenda(colonnaAgenda);
            return esito;
        }

    }



    public enum EnumTipologiche { TIPO_COLONNE_AGENDA,
                                    TIPO_QUALIFICHE,
                                    TIPO_UTENTE,
                                    TIPO_STATO,
                                    TIPO_TIPOLOGIE,
                                    TIPO_CLIENTI_FORNITORI,
                                    TIPO_GENERE,
                                    TIPO_GRUPPO,
                                    TIPO_SOTTOGRUPPO,
                                    TIPO_TENDER,
                                    TIPO_PROTOCOLLO,
                                    TIPO_PAGAMENTO,
                                    TIPO_INTERVENTO,
                                    TIPO_CATEGORIE_MAGAZZINO,
                                    TIPO_SUB_CATEGORIE_MAGAZZINO,
                                    TIPO_POSIZIONE_MAGAZZINO}

    public enum EnumSottotipiRisorse {DIPENDENTI, REGIE, EXTRA}
}