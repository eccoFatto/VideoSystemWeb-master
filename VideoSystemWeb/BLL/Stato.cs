using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.Entity;

namespace VideoSystemWeb.BLL
{
    public class Stato
    {
        //singleton
        private static volatile Stato instance;
        private static object objForLock = new Object();

        private Stato() { }

        public static Stato Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Stato();
                    }
                }
                return instance;
            }
        }

        BasePage basePage = new BasePage();
        public int STATO_PREVISIONE_IMPEGNO
        {
            get
            {
                return ((Tipologica)basePage.listaStati.Where(x => x.nome.ToUpper() == "previsione impegno".ToUpper()).FirstOrDefault()).id;
            }
        }
        public int STATO_OFFERTA
        {
            get
            {
                return ((Tipologica)basePage.listaStati.Where(x => x.nome.ToUpper() == "Offerta".ToUpper()).FirstOrDefault()).id;
            }
        }
        public int STATO_LAVORAZIONE
        {
            get
            {
                return ((Tipologica)basePage.listaStati.Where(x => x.nome.ToUpper() == "Lavorazione".ToUpper()).FirstOrDefault()).id;
            }
        }
        public int STATO_FATTURA
        {
            get
            {
                return ((Tipologica)basePage.listaStati.Where(x => x.nome.ToUpper() == "Fattura".ToUpper()).FirstOrDefault()).id;
            }
        }
        public int STATO_RIPOSO
        {
            get
            {
                return ((Tipologica)basePage.listaStati.Where(x => x.nome.ToUpper() == "Riposo".ToUpper()).FirstOrDefault()).id;
            }
        }
        public int STATO_VIAGGIO
        {
            get
            {
                return ((Tipologica)basePage.listaStati.Where(x => x.nome.ToUpper() == "Viaggio / Installazione".ToUpper()).FirstOrDefault()).id;
            }
        }
    }
}