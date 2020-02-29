using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;
namespace VideoSystemWeb.BLL
{
    public class Dati_Agenda_Magazzino_BLL
    {
        //singleton
        private static volatile Dati_Agenda_Magazzino_BLL instance;
        private static object objForLock = new Object();
        private Dati_Agenda_Magazzino_BLL() { }
        public static Dati_Agenda_Magazzino_BLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Dati_Agenda_Magazzino_BLL();
                    }
                }
                return instance;
            }
        }

        public DatiAgendaMagazzino getDatiAgendaMagazzinoById(int idDatiAgendaMagazzino, ref Esito esito)
        {
            DatiAgendaMagazzino datiAgendaMagazzino = Dati_Agenda_Magazzino_DAL.Instance.getDatiAgendaMagazzinoById(idDatiAgendaMagazzino, ref esito);
            return datiAgendaMagazzino;
        }

        public int CreaDatiAgendaMagazzino(DatiAgendaMagazzino datiAgendaMagazzino, ref Esito esito)
        {
            int iREt = Dati_Agenda_Magazzino_DAL.Instance.CreaDatiAgendaMagazzino(datiAgendaMagazzino, ref esito);

            return iREt;
        }

        public Esito AggiornaDatiAgendaMagazzino(DatiAgendaMagazzino datiAgendaMagazzino)
        {
            Esito esito = Dati_Agenda_Magazzino_DAL.Instance.AggiornaDatiAgendaMagazzino(datiAgendaMagazzino);

            return esito;
        }

        public Esito EliminaDatiAgendaMagazzino(int idDatiAgendaMagazzino)
        {
            Esito esito = Dati_Agenda_Magazzino_DAL.Instance.EliminaDatiAgendaMagazzino(idDatiAgendaMagazzino);

            return esito;
        }

        public List<DatiAgendaMagazzino> getDatiAgendaMagazzinoByIdAgenda(int idAgenda, ref Esito esito)
        {
            List<DatiAgendaMagazzino> listaDatiAgendaMagazzino = Dati_Agenda_Magazzino_DAL.Instance.getDatiAgendaMagazzinoByIdAgenda(idAgenda, ref esito);
            return listaDatiAgendaMagazzino;
        }
    }
}