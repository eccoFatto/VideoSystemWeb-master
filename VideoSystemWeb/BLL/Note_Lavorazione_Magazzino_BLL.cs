using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;
namespace VideoSystemWeb.BLL
{
    public class Note_Lavorazione_Magazzino_BLL
    {
        //singleton
        private static volatile Note_Lavorazione_Magazzino_BLL instance;
        private static object objForLock = new Object();
        private Note_Lavorazione_Magazzino_BLL() { }
        public static Note_Lavorazione_Magazzino_BLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Note_Lavorazione_Magazzino_BLL();
                    }
                }
                return instance;
            }
        }

        public NoteLavorazioneMagazzino getNoteLavorazioneMagazzinoById(int idNoteLavorazioneMagazzino, ref Esito esito)
        {
            NoteLavorazioneMagazzino noteLavorazioneMagazzino = Note_Lavorazione_Magazzino_DAL.Instance.getNoteLavorazioneMagazzinoById(idNoteLavorazioneMagazzino, ref esito);
            return noteLavorazioneMagazzino;
        }

        public NoteLavorazioneMagazzino getNoteLavorazioneMagazzinoByIdLavorazione(int idLavorazione, ref Esito esito)
        {
            NoteLavorazioneMagazzino noteLavorazioneMagazzino = Note_Lavorazione_Magazzino_DAL.Instance.getNoteLavorazioneMagazzinoByIdLavorazione(idLavorazione, ref esito);
            return noteLavorazioneMagazzino;
        }

        public int CreaNoteLavorazioneMagazzino(NoteLavorazioneMagazzino noteLavorazioneMagazzino, ref Esito esito)
        {
            int iREt = Note_Lavorazione_Magazzino_DAL.Instance.CreaNoteLavorazioneMagazzino(noteLavorazioneMagazzino, ref esito);

            return iREt;
        }

        public Esito AggiornaNoteLavorazioneMagazzino(NoteLavorazioneMagazzino noteLavorazioneMagazzino)
        {
            Esito esito = Note_Lavorazione_Magazzino_DAL.Instance.AggiornaNoteLavorazioneMagazzino(noteLavorazioneMagazzino);

            return esito;
        }

        public Esito EliminaNoteLavorazioneMagazzino(int idNoteLavorazioneMagazzino)
        {
            Esito esito = Note_Lavorazione_Magazzino_DAL.Instance.EliminaNoteLavorazioneMagazzino(idNoteLavorazioneMagazzino);

            return esito;
        }
    }
}