using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;
namespace VideoSystemWeb.BLL
{
    public class Offerta_BLL
    {
        //singleton
        private static volatile Offerta_BLL instance;
        private static object objForLock = new Object();
        private Offerta_BLL() { }
        public static Offerta_BLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Offerta_BLL();
                    }
                }
                return instance;
            }
        }

        public NoteOfferta getNoteOffertaByIdDatiAgenda(int idDatiAgenda, ref Esito esito)
        {
            NoteOfferta noteOfferta = Offerta_DAL.Instance.GetNoteOffertaByIdDatiAgenda(ref esito, idDatiAgenda);
            return noteOfferta;
        }

        public NoteOfferta getNoteOffertaById(int idNoteOfferta, ref Esito esito)
        {
            NoteOfferta noteOfferta = Offerta_DAL.Instance.GetNoteOffertaById( ref esito, idNoteOfferta);
            return noteOfferta;
        }

        public int CreaNoteOfferta(NoteOfferta noteOfferta, ref Esito esito)
        {
            int iREt = Offerta_DAL.Instance.CreaNoteOfferta(noteOfferta, ref esito);

            return iREt;
        }

        public Esito AggiornaNoteOfferta(NoteOfferta noteOfferta)
        {
            Esito esito = Offerta_DAL.Instance.AggiornaNoteOfferta(noteOfferta);

            return esito;
        }

        public Esito EliminaNoteOfferta(int idNoteOfferta)
        {
            Esito esito = Offerta_DAL.Instance.EliminaNoteOfferta(idNoteOfferta);

            return esito;
        }

        #region RECUPERA OFFERTA
        public List<string> GetAllCodiciLavoro()
        {
            return Offerta_DAL.Instance.GetAllCodicilavoro();
        }

        public List<string> GetAllProduzioni()
        {
            return Offerta_DAL.Instance.GetAllProduzioni();
        }

        public List<string> GetAllLavorazioni()
        {
            return Offerta_DAL.Instance.GetAllLavorazioni();
        }

        public List<string> GetAllLuoghi()
        {
            return Offerta_DAL.Instance.GetAllLuoghi();
        }

        public Esito GetListaDatiAgendaByFiltri(string dataLavorazione, string idTipologia, string idCliente, string produzione, string lavorazione, string luogo, string codiceLavoro, ref List<DatiAgenda> listaDatiAgenda)
        {
            return Offerta_DAL.Instance.GetListaDatiAgendaByFiltri(dataLavorazione, idTipologia, idCliente, produzione, lavorazione, luogo, codiceLavoro, ref listaDatiAgenda);
        }

        public Esito GetListaDatiArticoliByIdDatiAgenda(string idDatiAgenda, ref List<DatiArticoli> listaDatiArticoli)
        {
            return Offerta_DAL.Instance.GetListaDatiArticoliByIdDatiAgenda(idDatiAgenda, ref listaDatiArticoli);
        }

        public Esito InserisciOffertaRecuperata(List<DatiArticoli> listaDatiArticoli)
        {
            return Offerta_DAL.Instance.InserisciOffertaRecuperata(listaDatiArticoli);
        }
        #endregion
    }
}