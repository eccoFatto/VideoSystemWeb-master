using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;
namespace VideoSystemWeb.BLL
{
    public class Dati_Tender_BLL
    {
        //singleton
        private static volatile Dati_Tender_BLL instance;
        private static object objForLock = new Object();
        private Dati_Tender_BLL() { }
        public static Dati_Tender_BLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Dati_Tender_BLL();
                    }
                }
                return instance;
            }
        }

        public List<DatiTender> getDatiAgendaTenderByIdAgenda(int idAgenda, ref Esito esito)
        {
            List<DatiTender> listaDatiAgendaTender = Dati_Tender_DAL.Instance.getDatiAgendaTenderByIdAgenda(idAgenda, ref esito);
            return listaDatiAgendaTender;
        }

        public DatiTender getDatiAgendaTenderById(int idAgendaTender, ref Esito esito)
        {
            DatiTender datiAgendaTender = Dati_Tender_DAL.Instance.getDatiAgendaTenderById(idAgendaTender, ref esito);
            return datiAgendaTender;
        }

        public int CreaDatiAgendaTender(DatiTender datiAgendaTender, ref Esito esito)
        {
            int iREt = Dati_Tender_DAL.Instance.CreaDatiAgendaTender(datiAgendaTender, ref esito);

            return iREt;
        }

        public Esito AggiornaDatiAgendaTender(DatiTender datiAgendaTender)
        {
            Esito esito = Dati_Tender_DAL.Instance.AggiornaDatiAgendaTender(datiAgendaTender);

            return esito;
        }

        public Esito EliminaDatiAgendaTender(int idDatiAgendaTender)
        {
            Esito esito = Dati_Tender_DAL.Instance.EliminaDatiAgendaTender(idDatiAgendaTender);

            return esito;
        }
    }
}