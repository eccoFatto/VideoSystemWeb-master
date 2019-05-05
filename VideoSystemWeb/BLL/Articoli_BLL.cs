using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;

namespace VideoSystemWeb.BLL
{
    public class Articoli_BLL
    {

        ObjectIDGenerator IDGenerator = new ObjectIDGenerator();

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

        public int CreaArticolo(Art_Articoli articolo, Anag_Utenti utente, ref Esito esito)
        {
            int iREt = Art_Articoli_DAL.Instance.CreaArticolo(articolo, utente, ref esito);

            return iREt;
        }

        public Esito AggiornaArticolo(Art_Articoli articolo, Anag_Utenti utente)
        {
            Esito esito = Art_Articoli_DAL.Instance.AggiornaArticolo(articolo, utente);

            return esito;
        }

        public Esito EliminaArticolo(int idArticolo, Anag_Utenti utente)
        {
            Esito esito = Art_Articoli_DAL.Instance.EliminaArticolo(idArticolo, utente);

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

        public int CreaGruppo(Art_Gruppi gruppo, Anag_Utenti utente, ref Esito esito)
        {
            int iREt = Art_Gruppi_DAL.Instance.CreaGruppo(gruppo, utente, ref esito);

            return iREt;
        }

        public Esito AggiornaGruppo(Art_Gruppi gruppo, Anag_Utenti utente)
        {
            Esito esito = Art_Gruppi_DAL.Instance.AggiornaGruppo(gruppo, utente);

            return esito;
        }

        public Esito EliminaGruppo(int idGruppo, Anag_Utenti utente)
        {
            Esito esito = Art_Gruppi_DAL.Instance.EliminaGruppo(idGruppo, utente);

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

        public int CreaGruppoArticolo(Art_Gruppi_Articoli gruppoArticolo, Anag_Utenti utente, ref Esito esito)
        {
            int iREt = Art_Gruppi_Articoli_DAL.Instance.CreaGruppoArticolo(gruppoArticolo, utente, ref esito);

            return iREt;
        }

        public Esito AggiornaGruppoArticolo(Art_Gruppi_Articoli gruppoArticolo, Anag_Utenti utente)
        {
            Esito esito = Art_Gruppi_Articoli_DAL.Instance.AggiornaGruppoArticolo(gruppoArticolo, utente);

            return esito;
        }

        public Esito EliminaGruppoArticolo(int idGruppoArticolo, Anag_Utenti utente)
        {
            Esito esito = Art_Gruppi_Articoli_DAL.Instance.EliminaGruppoArticolo(idGruppoArticolo, utente);

            return esito;
        }
        #endregion

        public List<DatiArticoli>  CaricaListaArticoliByIDGruppo(int idEvento, int idGruppo, ref Esito esito, bool soloAttivi = true)
        {
            List<DatiArticoli> listaArticoliDelGruppo = new List<DatiArticoli>();

            List<int> listaIDArticoli = CaricaListaGruppiArticoliByIDgruppo(idGruppo, ref esito).Select(x => x.IdArtArticoli).ToList<int>();
            int iva = int.Parse(Config_DAL.Instance.getConfig(ref esito, SessionManager.CFG_IVA).valore);

            
            foreach (int idArticolo in listaIDArticoli)
            {
                Art_Articoli articoloTemplate = getArticoloById(idArticolo, ref esito);

                DatiArticoli articolo = new DatiArticoli();
                
                bool firstTime;
                articolo.IdentificatoreOggetto = IDGenerator.GetId(articolo, out firstTime);

                articolo.IdArtArticoli = articoloTemplate.Id;
                articolo.IdDatiAgenda = idEvento;
                articolo.Descrizione = articoloTemplate.DefaultDescrizione;
                articolo.DescrizioneLunga = articoloTemplate.DefaultDescrizioneLunga;
                articolo.Stampa = articoloTemplate.DefaultStampa;
                articolo.Prezzo = articoloTemplate.DefaultPrezzo;
                articolo.Costo = articoloTemplate.DefaultCosto;
                articolo.Iva = iva;// articoloTemplate.DefaultIva;
                articolo.IdTipoGenere = articoloTemplate.DefaultIdTipoGenere;
                articolo.IdTipoGruppo = articoloTemplate.DefaultIdTipoGruppo;
                articolo.IdTipoSottogruppo = articoloTemplate.DefaultIdTipoSottogruppo;
                articolo.Quantita = 1;

                listaArticoliDelGruppo.Add(articolo);
            }
            
            return listaArticoliDelGruppo;
        }

        public List<DatiArticoliLavorazione> CaricaListaArticoliLavorazioneByIDGruppo(int idDatiLavorazione, int idGruppo, ref Esito esito, bool soloAttivi = true)
        {
            List<DatiArticoliLavorazione> listaArticoliDelGruppo = new List<DatiArticoliLavorazione>();

            List<int> listaIDArticoli = CaricaListaGruppiArticoliByIDgruppo(idGruppo, ref esito).Select(x => x.IdArtArticoli).ToList<int>();
            int iva = int.Parse(Config_DAL.Instance.getConfig(ref esito, SessionManager.CFG_IVA).valore);


            foreach (int idArticolo in listaIDArticoli)
            {
                Art_Articoli articoloTemplate = getArticoloById(idArticolo, ref esito);

                DatiArticoliLavorazione articoloLavorazione = new DatiArticoliLavorazione();

                bool firstTime;
                articoloLavorazione.IdentificatoreOggetto = IDGenerator.GetId(articoloLavorazione, out firstTime);

                articoloLavorazione.IdDatiLavorazione = idDatiLavorazione;
                articoloLavorazione.IdArtArticoli = articoloTemplate.Id;
                articoloLavorazione.IdTipoGenere = articoloTemplate.DefaultIdTipoGenere;
                articoloLavorazione.IdTipoGruppo = articoloTemplate.DefaultIdTipoGruppo;
                articoloLavorazione.IdTipoSottogruppo = articoloTemplate.DefaultIdTipoSottogruppo;

                articoloLavorazione.Descrizione = articoloTemplate.DefaultDescrizione;
                articoloLavorazione.DescrizioneLunga = articoloTemplate.DefaultDescrizioneLunga;
                articoloLavorazione.Stampa = articoloTemplate.DefaultStampa;
                articoloLavorazione.Prezzo = articoloTemplate.DefaultPrezzo;
                articoloLavorazione.Costo = articoloTemplate.DefaultCosto;
                articoloLavorazione.Iva = iva;
                
                listaArticoliDelGruppo.Add(articoloLavorazione);
            }

            return listaArticoliDelGruppo;
        }

        public DatiArticoli CaricaArticoloByID(int idEvento, int idArticolo, ref Esito esito, bool soloAttivi = true)
        {
            Art_Articoli articoloTemplate = getArticoloById(idArticolo, ref esito);
            int iva = int.Parse(Config_DAL.Instance.getConfig(ref esito, SessionManager.CFG_IVA).valore);

            DatiArticoli articolo = new DatiArticoli();

            bool firstTime;
            articolo.IdentificatoreOggetto = IDGenerator.GetId(articolo, out firstTime);

            articolo.IdArtArticoli = articoloTemplate.Id;
            articolo.IdDatiAgenda = idEvento;
            articolo.Descrizione = articoloTemplate.DefaultDescrizione;
            articolo.DescrizioneLunga = articoloTemplate.DefaultDescrizioneLunga;
            articolo.Stampa = articoloTemplate.DefaultStampa;
            articolo.Prezzo = articoloTemplate.DefaultPrezzo;
            articolo.Costo = articoloTemplate.DefaultCosto;
            articolo.Iva = iva;// articoloTemplate.DefaultIva;
            articolo.IdTipoGenere = articoloTemplate.DefaultIdTipoGenere;
            articolo.IdTipoGruppo = articoloTemplate.DefaultIdTipoGruppo;
            articolo.IdTipoSottogruppo = articoloTemplate.DefaultIdTipoSottogruppo;
            articolo.Quantita = 1;


            return articolo;
        }

        public DatiArticoliLavorazione CaricaArticoloLavorazioneByID(int idDatiLavorazione, int idArticolo, ref Esito esito, bool soloAttivi = true)
        {
            Art_Articoli articoloTemplate = getArticoloById(idArticolo, ref esito);
            int iva = int.Parse(Config_DAL.Instance.getConfig(ref esito, SessionManager.CFG_IVA).valore);

            DatiArticoliLavorazione articoloLavorazione = new DatiArticoliLavorazione();

            bool firstTime;
            articoloLavorazione.IdentificatoreOggetto = IDGenerator.GetId(articoloLavorazione, out firstTime);

            articoloLavorazione.IdDatiLavorazione= idDatiLavorazione;
            articoloLavorazione.IdArtArticoli = articoloTemplate.Id;
            articoloLavorazione.IdTipoGenere = articoloTemplate.DefaultIdTipoGenere;
            articoloLavorazione.IdTipoGruppo = articoloTemplate.DefaultIdTipoGruppo;
            articoloLavorazione.IdTipoSottogruppo = articoloTemplate.DefaultIdTipoSottogruppo;
            articoloLavorazione.Descrizione = articoloTemplate.DefaultDescrizione;
            articoloLavorazione.DescrizioneLunga = articoloTemplate.DefaultDescrizioneLunga;
            articoloLavorazione.Stampa = articoloTemplate.DefaultStampa;
            articoloLavorazione.Prezzo = articoloTemplate.DefaultPrezzo;
            articoloLavorazione.Costo = articoloTemplate.DefaultCosto;
            articoloLavorazione.Iva = iva;// articoloTemplate.DefaultIva;
            articoloLavorazione.Data = DateTime.Now;
            articoloLavorazione.Tv = 0;

            return articoloLavorazione;
        }

        public List<DatiArticoli> CaricaListaArticoliByIDEvento(int idDatiAgenda, ref Esito esito)
        {
            return Dati_Articoli_DAL.Instance.getDatiArticoliByIdDatiAgenda(ref esito, idDatiAgenda);
        }

        public List<ArticoliGruppi> CaricaListaArticoliGruppi()
        {
            List<ArticoliGruppi> listaArticoliGruppi = new List<ArticoliGruppi>();

            Esito esito = new Esito();
            List<Art_Gruppi> listaArt_Gruppi = Articoli_BLL.Instance.CaricaListaGruppi(ref esito);
            List<Art_Articoli> listaArt_Articoli = Articoli_BLL.Instance.CaricaListaArticoli(ref esito);
            foreach (Art_Gruppi gruppo in listaArt_Gruppi)
            {
                ArticoliGruppi articoloGruppo = new ArticoliGruppi();
                bool firstTime;

                articoloGruppo.Id = IDGenerator.GetId(articoloGruppo, out firstTime);
                articoloGruppo.IdOggetto = gruppo.Id;
                articoloGruppo.Nome = gruppo.Nome;
                articoloGruppo.Descrizione = gruppo.Descrizione;
                articoloGruppo.Isgruppo = true;

                listaArticoliGruppi.Add(articoloGruppo);
            }
            foreach (Art_Articoli articolo in listaArt_Articoli)
            {
                ArticoliGruppi articoloGruppo = new ArticoliGruppi();
                bool firstTime;

                articoloGruppo.Id = IDGenerator.GetId(articoloGruppo, out firstTime);
                articoloGruppo.IdOggetto = articolo.Id;
                articoloGruppo.Nome = articolo.DefaultDescrizione;
                articoloGruppo.Descrizione = articolo.DefaultDescrizioneLunga;
                articoloGruppo.Isgruppo = false;

                listaArticoliGruppi.Add(articoloGruppo);
            }

            return listaArticoliGruppi;
        }
    }
}