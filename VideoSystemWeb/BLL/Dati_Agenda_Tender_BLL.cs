using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;
namespace VideoSystemWeb.BLL
{
    public class Dati_Agenda_Tender_BLL
    {
        //singleton
        private static volatile Dati_Agenda_Tender_BLL instance;
        private static object objForLock = new Object();
        private Dati_Agenda_Tender_BLL() { }
        public static Dati_Agenda_Tender_BLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Dati_Agenda_Tender_BLL();
                    }
                }
                return instance;
            }
        }

        public List<Dati_Agenda_Tender> getDatiAgendaTenderByIdAgenda(int idAgenda, ref Esito esito)
        {
            List<Dati_Agenda_Tender> listaDatiAgendaTender = Dati_Agenda_Tender_DAL.Instance.getDatiAgendaTenderByIdAgenda(idAgenda, ref esito);
            return listaDatiAgendaTender;
        }

        public Dati_Agenda_Tender getDatiAgendaTenderById(int idAgendaTender, ref Esito esito)
        {
            Dati_Agenda_Tender datiAgendaTender = Dati_Agenda_Tender_DAL.Instance.getDatiAgendaTenderById(idAgendaTender, ref esito);
            return datiAgendaTender;
        }

        public int CreaDatiAgendaTender(Dati_Agenda_Tender datiAgendaTender, Anag_Utenti utente, ref Esito esito)
        {
            int iREt = Dati_Agenda_Tender_DAL.Instance.CreaDatiAgendaTender(datiAgendaTender, utente, ref esito);

            return iREt;
        }

        public Esito AggiornaDatiAgendaTender(Dati_Agenda_Tender datiAgendaTender, Anag_Utenti utente)
        {
            Esito esito = Dati_Agenda_Tender_DAL.Instance.AggiornaDatiAgendaTender(datiAgendaTender, utente);

            return esito;
        }

        public Esito EliminaDatiAgendaTender(int idDatiAgendaTender, Anag_Utenti utente)
        {
            Esito esito = Dati_Agenda_Tender_DAL.Instance.EliminaDatiAgendaTender(idDatiAgendaTender, utente);

            return esito;
        }
    }
}