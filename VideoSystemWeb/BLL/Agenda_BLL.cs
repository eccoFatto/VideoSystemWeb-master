using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;

namespace VideoSystemWeb.BLL
{
    public class Agenda_BLL
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //singleton
        private static volatile Agenda_BLL instance;
        private static object objForLock = new Object();

        private Agenda_BLL() { }

        public static Agenda_BLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Agenda_BLL();
                    }
                }
                return instance;
            }
        }

        public List<Tipologica> CaricaColonne(ref Esito esito)
        {
            List<Tipologica> listaColonne = new List<Tipologica>();

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["USA_DB"]))
            {
                listaColonne = Agenda_DAL.Instance.CaricaColonne(ref esito);
            }
            else
            {
                listaColonne = Tipologie.caricaTipologica(EnumTipologiche.TIPO_COLONNE_AGENDA);
            }

            return listaColonne;
        }

        public List<DatiAgenda> CaricaDatiAgenda(ref Esito esito)
        {
            List<DatiAgenda> listaDatiAgenda = new List<DatiAgenda>();

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["USA_DB"]))
            {
                listaDatiAgenda = Agenda_DAL.Instance.CaricaDatiAgenda(ref esito);
            }
            else
            {
                listaDatiAgenda = Tipologie.getListaDatiAgenda();
            }

            return listaDatiAgenda;
        }

        public List<DatiAgenda> CaricaDatiAgenda(DateTime dataInizio, ref Esito esito)
        {
            List<DatiAgenda> listaDatiAgenda = new List<DatiAgenda>();

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["USA_DB"]))
            {
                listaDatiAgenda = Agenda_DAL.Instance.CaricaDatiAgenda(dataInizio, dataInizio.AddDays(31), ref esito);
            }
            else
            {
                listaDatiAgenda = Tipologie.getListaDatiAgenda();
            }

            log.Error(esito.descrizione);

            return listaDatiAgenda;
        }

        public string[] CaricaElencoProduzioni(ref Esito esito)
        {
            return Agenda_DAL.Instance.CaricaElencoProduzioni(ref esito).ToArray();
        }

        public string[] CaricaElencoLavorazioni(ref Esito esito)
        {
            return Agenda_DAL.Instance.CaricaElencoLavorazioni(ref esito).ToArray();
        }

        public DatiAgenda GetDatiAgendaById(List<DatiAgenda> listaDatiAgenda, int id)
        {
            return listaDatiAgenda.Where(x => x.id == id).FirstOrDefault();
        }

        public DatiAgenda GetDatiAgendaByDataRisorsa(List<DatiAgenda> listaDatiAgenda, DateTime data, int id_risorsa)
        {
            return listaDatiAgenda.Where(x => x.data_inizio_impegno.Date <= data.Date && x.data_fine_impegno.Date >= data.Date && x.id_colonne_agenda == id_risorsa).FirstOrDefault();
        }

        public Esito CreaEvento(DatiAgenda evento, List<DatiArticoli> listaDatiArticoli, List<string> listaIdTender)
        {
            Esito esito = new Esito();


            if (evento.id_stato == Stato.Instance.STATO_PREVISIONE_IMPEGNO)
            {
                esito = Agenda_DAL.Instance.CreaEvento(evento, listaIdTender);
            }
            else if (evento.id_stato == Stato.Instance.STATO_OFFERTA)
            {
                esito = Agenda_DAL.Instance.creaEventoConArticoli(evento, listaDatiArticoli, listaIdTender);
            }
            else if (evento.id_stato == Stato.Instance.STATO_RIPOSO)
            {
                esito = Agenda_DAL.Instance.CreaEvento(evento, listaIdTender);
            }

            return esito;
        }

        public Esito AggiornaEvento(DatiAgenda evento, List<DatiArticoli> listaDatiArticoli, List<string> listaIdTender)
        {
            Esito esito = new Esito();


            if (evento.id_stato == Stato.Instance.STATO_PREVISIONE_IMPEGNO)
            {
                esito = Agenda_DAL.Instance.AggiornaEvento(evento, listaIdTender);
            }
            else if (evento.id_stato == Stato.Instance.STATO_OFFERTA)
            {
                esito = Agenda_DAL.Instance.AggiornaEventoConArticoli(evento, listaDatiArticoli, listaIdTender);
            }
            else if (evento.id_stato == Stato.Instance.STATO_RIPOSO)
            {
                esito = Agenda_DAL.Instance.AggiornaEvento(evento, listaIdTender);
            }

            return esito;
        }

        public Esito EliminaEvento(int idEvento)
        {
            Esito esito = Dati_Articoli_DAL.Instance.EliminaDatiArticoloByIdDatiAgenda(idEvento);
            if (esito.codice == Esito.ESITO_OK)
            {
                esito = Dati_Tender_DAL.Instance.EliminaDatiTenderByIdDatiAgenda(idEvento);
            }
            if (esito.codice == Esito.ESITO_OK)
            {
                esito = Agenda_DAL.Instance.EliminaEvento(idEvento);
            }
            return esito;
        }
    }
}