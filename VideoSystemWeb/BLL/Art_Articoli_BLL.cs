using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;
namespace VideoSystemWeb.BLL
{
    public class Art_Articoli_BLL
    {
        //singleton
        private static volatile Art_Articoli_BLL instance;
        private static object objForLock = new Object();
        private Art_Articoli_BLL() { }
        public static Art_Articoli_BLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Art_Articoli_BLL();
                    }
                }
                return instance;
            }
        }

        public List<Art_Articoli> CaricaListaArticoli(ref Esito esito, bool soloAttivi = true)
        {
            List<Art_Articoli> listaArticoli = Art_Articoli_DAL.Instance.CaricaListaArticoli(ref esito, soloAttivi);
            return listaArticoli;
        }

        public Art_Articoli getArticoloById(int idArticolo, ref Esito esito)
        {
            Art_Articoli articolo = Art_Articoli_DAL.Instance.getArticoloById(idArticolo, ref esito);
            return articolo;
        }

        public int CreaArticolo(Art_Articoli articolo, ref Esito esito)
        {
            int iREt = Art_Articoli_DAL.Instance.CreaArticolo(articolo, ref esito);

            return iREt;
        }

        public Esito AggiornaArticolo(Art_Articoli articolo)
        {
            Esito esito = Art_Articoli_DAL.Instance.AggiornaArticolo(articolo);

            return esito;
        }

        public Esito EliminaArticolo(int idArticolo)
        {
            Esito esito = Art_Articoli_DAL.Instance.EliminaArticolo(idArticolo);

            return esito;
        }
    }
}