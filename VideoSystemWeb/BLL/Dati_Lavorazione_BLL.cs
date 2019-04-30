using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;
namespace VideoSystemWeb.BLL
{
    public class Dati_Lavorazione_BLL
    {
        //singleton
        private static volatile Dati_Lavorazione_BLL instance;
        private static object objForLock = new Object();
        private Dati_Lavorazione_BLL() { }
        public static Dati_Lavorazione_BLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Dati_Lavorazione_BLL();
                    }
                }
                return instance;
            }
        }


        public DatiLavorazione getDatiLavorazioneById(int idDatiLavorazione, ref Esito esito)
        {
            DatiLavorazione datiLavorazione = Dati_Lavorazione_DAL.Instance.getDatiLavorazioneById(idDatiLavorazione, ref esito);
            return datiLavorazione;
        }

        public int CreaDatiLavorazione(DatiLavorazione datiLavorazione, ref Esito esito)
        {
            int iREt = Dati_Lavorazione_DAL.Instance.CreaDatiLavorazione(datiLavorazione, ref esito);

            return iREt;
        }

        public Esito AggiornaDatiLavorazione(DatiLavorazione datiLavorazione)
        {
            Esito esito = Dati_Lavorazione_DAL.Instance.AggiornaDatiLavorazione(datiLavorazione);

            return esito;
        }

        public Esito EliminaDatiLavorazione(int idDatiLavorazione)
        {
            Esito esito = Dati_Lavorazione_DAL.Instance.EliminaDatiLavorazione(idDatiLavorazione);

            return esito;
        }
    }
}