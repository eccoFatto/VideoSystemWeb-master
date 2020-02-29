using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;
namespace VideoSystemWeb.BLL
{
    public class Note_Agenda_Magazzino_BLL
    {
        //singleton
        private static volatile Note_Agenda_Magazzino_BLL instance;
        private static object objForLock = new Object();
        private Note_Agenda_Magazzino_BLL() { }
        public static Note_Agenda_Magazzino_BLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Note_Agenda_Magazzino_BLL();
                    }
                }
                return instance;
            }
        }

        public NoteAgendaMagazzino getNoteAgendaMagazzinoById(int idNoteAgendaMagazzino, ref Esito esito)
        {
            NoteAgendaMagazzino noteAgendaMagazzino = Note_Agenda_Magazzino_DAL.Instance.getNoteAgendaMagazzinoById(idNoteAgendaMagazzino, ref esito);
            return noteAgendaMagazzino;
        }

        public NoteAgendaMagazzino getNoteAgendaMagazzinoByIdAgenda(int idAgenda, ref Esito esito)
        {
            NoteAgendaMagazzino noteAgendaMagazzino = Note_Agenda_Magazzino_DAL.Instance.getNoteAgendaMagazzinoByIdAgenda(idAgenda, ref esito);
            return noteAgendaMagazzino;
        }

        public int CreaNoteAgendaMagazzino(NoteAgendaMagazzino noteAgendaMagazzino, ref Esito esito)
        {
            int iREt = Note_Agenda_Magazzino_DAL.Instance.CreaNoteAgendaMagazzino(noteAgendaMagazzino, ref esito);

            return iREt;
        }

        public Esito AggiornaNoteAgendaMagazzino(NoteAgendaMagazzino noteAgendaMagazzino)
        {
            Esito esito = Note_Agenda_Magazzino_DAL.Instance.AggiornaNoteAgendaMagazzino(noteAgendaMagazzino);

            return esito;
        }

        public Esito EliminaNoteAgendaMagazzino(int idNoteAgendaMagazzino)
        {
            Esito esito = Note_Agenda_Magazzino_DAL.Instance.EliminaNoteAgendaMagazzino(idNoteAgendaMagazzino);

            return esito;
        }
    }
}