using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;

namespace VideoSystemWeb.BLL
{
    public class Anag_Clienti_Fornitori_BLL
    {
        //singleton
        private static volatile Anag_Clienti_Fornitori_BLL instance;
        private static object objForLock = new Object();
        private Anag_Clienti_Fornitori_BLL() { }
        public static Anag_Clienti_Fornitori_BLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Anag_Clienti_Fornitori_BLL();
                    }
                }
                return instance;
            }
        }

        public Anag_Clienti_Fornitori getAziendaById(int idAzienda, ref Esito esito)
        {
            return Anag_Clienti_Fornitori_DAL.Instance.getAziendaById(idAzienda, ref esito);
        }

        public List<Anag_Clienti_Fornitori> CaricaListaAziende(ref Esito esito, bool soloAttivi = true)
        {
            return Anag_Clienti_Fornitori_DAL.Instance.CaricaListaAziende(ref esito, soloAttivi);
        }

        public List<Anag_Clienti_Fornitori> CaricaListaClienti(ref Esito esito, bool soloAttivi = true)
        {
            List<Anag_Clienti_Fornitori> listaClienti = Anag_Clienti_Fornitori_DAL.Instance.CaricaListaAziende(ref esito, soloAttivi);

            return listaClienti.Where(x => x.Cliente).ToList<Anag_Clienti_Fornitori>();
        }

        public List<Anag_Clienti_Fornitori> CaricaListaFornitori(ref Esito esito, bool soloAttivi = true)
        {
            List<Anag_Clienti_Fornitori> listaAziende = Anag_Clienti_Fornitori_DAL.Instance.CaricaListaAziende(ref esito, soloAttivi);

            return listaAziende.Where(x => x.Cliente).ToList<Anag_Clienti_Fornitori>();
        }

        public int CreaAzienda(Anag_Clienti_Fornitori azienda, Anag_Utenti utente, ref Esito esito)
        {
            return Anag_Clienti_Fornitori_DAL.Instance.CreaAzienda(azienda, utente, ref esito);
        }

        public Esito AggiornaAzienda(Anag_Clienti_Fornitori azienda, Anag_Utenti utente)
        {
            return Anag_Clienti_Fornitori_DAL.Instance.AggiornaAzienda(azienda, utente);
        }

        public Esito EliminaAzienda(int idAzienda, Anag_Utenti utente)
        {
            return Anag_Clienti_Fornitori_DAL.Instance.EliminaAzienda(idAzienda, utente);
        }
    }
}