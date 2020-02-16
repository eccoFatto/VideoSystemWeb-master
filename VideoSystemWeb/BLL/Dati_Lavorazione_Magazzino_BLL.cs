using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;
namespace VideoSystemWeb.BLL
{
    public class Dati_Lavorazione_Magazzino_BLL
    {
        //singleton
        private static volatile Dati_Lavorazione_Magazzino_BLL instance;
        private static object objForLock = new Object();
        private Dati_Lavorazione_Magazzino_BLL() { }
        public static Dati_Lavorazione_Magazzino_BLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Dati_Lavorazione_Magazzino_BLL();
                    }
                }
                return instance;
            }
        }

        public DatiLavorazioneMagazzino getDatiLavorazioneMagazzinoById(int idDatiLavorazioneMagazzino, ref Esito esito)
        {
            DatiLavorazioneMagazzino datiLavorazioneMagazzino = Dati_Lavorazione_Magazzino_DAL.Instance.getDatiLavorazioneMagazzinoById(idDatiLavorazioneMagazzino, ref esito);
            return datiLavorazioneMagazzino;
        }

        public int CreaDatiLavorazioneMagazzino(DatiLavorazioneMagazzino datiLavorazioneMagazzino, ref Esito esito)
        {
            int iREt = Dati_Lavorazione_Magazzino_DAL.Instance.CreaDatiLavorazioneMagazzino(datiLavorazioneMagazzino, ref esito);

            return iREt;
        }

        public Esito AggiornaDatiLavorazioneMagazzino(DatiLavorazioneMagazzino datiLavorazioneMagazzino)
        {
            Esito esito = Dati_Lavorazione_Magazzino_DAL.Instance.AggiornaDatiLavorazioneMagazzino(datiLavorazioneMagazzino);

            return esito;
        }

        public Esito EliminaDatiLavorazioneMagazzino(int idDatiLavorazioneMagazzino)
        {
            Esito esito = Dati_Lavorazione_Magazzino_DAL.Instance.EliminaDatiLavorazioneMagazzino(idDatiLavorazioneMagazzino);

            return esito;
        }
    }
}