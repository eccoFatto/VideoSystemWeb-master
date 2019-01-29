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

        public DatiAgenda getDatiAgendaById(List<DatiAgenda> listaDatiAgenda, int id)
        {
            return listaDatiAgenda.Where(x => x.id == id).FirstOrDefault();
        }

        public DatiAgenda getDatiAgendaByDataRisorsa(List<DatiAgenda> listaDatiAgenda, DateTime data, int id_risorsa)
        {
            return listaDatiAgenda.Where(x => x.data_inizio_lavorazione <= data && x.data_fine_lavorazione >= data && x.id_colonne_agenda == id_risorsa).FirstOrDefault();
        }

        public Esito scriviEvento(DatiAgenda evento)
        {
            Esito esito = new Esito();


            return esito;
        }
    }
}