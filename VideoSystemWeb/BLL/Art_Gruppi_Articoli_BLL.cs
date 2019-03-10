using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;
namespace VideoSystemWeb.BLL
{
    public class Art_Gruppi_Articoli_BLL
    {
        //singleton
        private static volatile Art_Gruppi_Articoli_BLL instance;
        private static object objForLock = new Object();
        private Art_Gruppi_Articoli_BLL() { }
        public static Art_Gruppi_Articoli_BLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Art_Gruppi_Articoli_BLL();
                    }
                }
                return instance;
            }
        }

        public List<Art_Gruppi_Articoli> CaricaListaGruppiArticoli(ref Esito esito)
        {
            List<Art_Gruppi_Articoli> listaGruppiArticoli = Art_Gruppi_Articoli_DAL.Instance.CaricaListaGruppiArticoli(ref esito);
            return listaGruppiArticoli;
        }

        public List<Art_Gruppi> getGruppiByIdArticolo(int idArticolo, ref Esito esito)
        {
            List<Art_Gruppi> listaGruppi = Art_Gruppi_Articoli_DAL.Instance.getGruppiByIdArticolo(idArticolo,ref esito);
            return listaGruppi;
        }

        public Art_Gruppi_Articoli getGruppiArticoliById(int idGruppoArticolo, ref Esito esito)
        {
            Art_Gruppi_Articoli gruppoArticolo = Art_Gruppi_Articoli_DAL.Instance.getGruppiArticoliById(idGruppoArticolo, ref esito);
            return gruppoArticolo;
        }

        public List<Art_Articoli> getArticoliByIdGruppo(int idGruppo, ref Esito esito)
        {
            List<Art_Articoli> listaArticoli = Art_Gruppi_Articoli_DAL.Instance.getArticoliByIdGruppo(idGruppo, ref esito);
            return listaArticoli;
        }

        public int CreaGruppoArticolo(Art_Gruppi_Articoli gruppoArticolo, ref Esito esito)
        {
            int iREt = Art_Gruppi_Articoli_DAL.Instance.CreaGruppoArticolo(gruppoArticolo, ref esito);

            return iREt;
        }

        public Esito AggiornaGruppoArticolo(Art_Gruppi_Articoli gruppoArticolo)
        {
            Esito esito = Art_Gruppi_Articoli_DAL.Instance.AggiornaGruppoArticolo(gruppoArticolo);

            return esito;
        }

        public Esito EliminaGruppoArticolo(int idGruppoArticolo)
        {
            Esito esito = Art_Gruppi_Articoli_DAL.Instance.EliminaGruppoArticolo(idGruppoArticolo);

            return esito;
        }
    }
}