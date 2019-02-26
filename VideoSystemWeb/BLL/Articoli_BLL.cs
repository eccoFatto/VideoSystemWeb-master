using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;

namespace VideoSystemWeb.BLL
{
    public class Articoli_BLL
    {
        //singleton
        private static volatile Articoli_BLL instance;
        private static object objForLock = new Object();
        private Articoli_BLL() { }
        public static Articoli_BLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Articoli_BLL();
                    }
                }
                return instance;
            }
        }
        #region ART_ARTICOLI
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
        #endregion

        #region ART_GRUPPI
        public List<Art_Gruppi> CaricaListaGruppi(ref Esito esito, bool soloAttivi = true)
        {
            List<Art_Gruppi> listaGruppi = Art_Gruppi_DAL.Instance.CaricaListaGruppi(ref esito, soloAttivi);
            return listaGruppi;
        }

        public Art_Gruppi getGruppiById(int idGruppo, ref Esito esito)
        {
            Art_Gruppi articolo = Art_Gruppi_DAL.Instance.getGruppiById(idGruppo, ref esito);
            return articolo;
        }

        public int CreaGruppo(Art_Gruppi gruppo, ref Esito esito)
        {
            int iREt = Art_Gruppi_DAL.Instance.CreaGruppo(gruppo, ref esito);

            return iREt;
        }

        public Esito AggiornaGruppo(Art_Gruppi gruppo)
        {
            Esito esito = Art_Gruppi_DAL.Instance.AggiornaGruppo(gruppo);

            return esito;
        }

        public Esito EliminaGruppo(int idGruppo)
        {
            Esito esito = Art_Gruppi_DAL.Instance.EliminaGruppo(idGruppo);

            return esito;
        }
        #endregion

        #region ART_GRUPPI_ARTICOLI
        public List<Art_Gruppi_Articoli> CaricaListaGruppiArticoli(ref Esito esito)
        {
            List<Art_Gruppi_Articoli> listaGruppiArticoli = Art_Gruppi_Articoli_DAL.Instance.CaricaListaGruppiArticoli(ref esito);
            return listaGruppiArticoli;
        }

        public List<Art_Gruppi_Articoli> CaricaListaGruppiArticoliByIDgruppo(int idGruppo, ref Esito esito)
        {
            List<Art_Gruppi_Articoli> listaGruppiArticoli = Art_Gruppi_Articoli_DAL.Instance.CaricaListaGruppiArticoli(ref esito);
            listaGruppiArticoli = listaGruppiArticoli.Where(x => x.IdArtGruppi == idGruppo).ToList<Art_Gruppi_Articoli>();
            return listaGruppiArticoli;
        }

        public Art_Gruppi_Articoli getGruppiArticoliById(int idGruppoArticolo, ref Esito esito)
        {
            Art_Gruppi_Articoli gruppoArticolo = Art_Gruppi_Articoli_DAL.Instance.getGruppiArticoliById(idGruppoArticolo, ref esito);
            return gruppoArticolo;
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
        #endregion

        public List<DatiArticoli> CaricaListaArticoliByIDGruppo(int idEvento, int idGruppo, ref Esito esito, bool soloAttivi = true)
        {
            List<DatiArticoli> listaArticoliDelGruppo = new List<DatiArticoli>();

            List<int> listaIDArticoli = CaricaListaGruppiArticoliByIDgruppo(idGruppo, ref esito).Select(x => x.IdArtArticoli).ToList<int>();

            foreach (int idArticolo in listaIDArticoli)
            {
                Art_Articoli articoloTemplate = getArticoloById(idArticolo, ref esito);

                DatiArticoli articolo = new DatiArticoli();
                articolo.IdArtArticoli = articoloTemplate.Id;
                articolo.IdDatiAgenda = idEvento;
                articolo.Stampa = articoloTemplate.DefaultStampa;
                articolo.Prezzo = articoloTemplate.DefaultPrezzo;
                articolo.Costo = articoloTemplate.DefaultCosto;
                articolo.Iva = articoloTemplate.DefaultIva;

                articolo.ArtArticoli = articoloTemplate;

                listaArticoliDelGruppo.Add(articolo);
            }
            
            return listaArticoliDelGruppo;
        }
    }
}