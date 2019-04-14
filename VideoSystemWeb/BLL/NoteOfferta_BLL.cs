using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;
namespace VideoSystemWeb.BLL
{
    public class NoteOfferta_BLL
    {
        //singleton
        private static volatile NoteOfferta_BLL instance;
        private static object objForLock = new Object();
        private NoteOfferta_BLL() { }
        public static NoteOfferta_BLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new NoteOfferta_BLL();
                    }
                }
                return instance;
            }
        }

        public NoteOfferta getNoteOffertaByIdDatiAgenda(int idDatiAgenda, ref Esito esito)
        {
            NoteOfferta noteOfferta = NoteOfferta_DAL.Instance.getNoteOffertaByIdDatiAgenda(ref esito, idDatiAgenda);
            return noteOfferta;
        }

        public NoteOfferta getNoteOffertaById(int idNoteOfferta, ref Esito esito)
        {
            NoteOfferta noteOfferta = NoteOfferta_DAL.Instance.getNoteOffertaById( ref esito, idNoteOfferta);
            return noteOfferta;
        }

        public int CreaNoteOfferta(NoteOfferta noteOfferta, ref Esito esito)
        {
            int iREt = NoteOfferta_DAL.Instance.CreaNoteOfferta(noteOfferta, ref esito);

            return iREt;
        }

        public Esito AggiornaNoteOfferta(NoteOfferta noteOfferta)
        {
            Esito esito = NoteOfferta_DAL.Instance.AggiornaNoteOfferta(noteOfferta);

            return esito;
        }

        public Esito EliminaNoteOfferta(int idNoteOfferta)
        {
            Esito esito = NoteOfferta_DAL.Instance.EliminaNoteOfferta(idNoteOfferta);

            return esito;
        }
    }
}