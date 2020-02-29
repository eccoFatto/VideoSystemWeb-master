using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.BLL;

namespace VideoSystemWeb.Entity
{
    //[id] [int] IDENTITY(1,1) NOT NULL,
    //[id_Agenda] [int] NOT NULL,
    //[note][varchar] (MAX) NULL,
    //[attivo] [bit] NOT NULL,

    [Serializable]
    public class NoteAgendaMagazzino
    {
        private int id;
        private int id_Agenda;
        private string note;
        private bool attivo;

        public int Id { get => id; set => id = value; }
        public int Id_Agenda { get => id_Agenda; set => id_Agenda = value; }
        public string Note { get => note; set => note = value; }
        public bool Attivo { get => attivo; set => attivo = value; }
    }
}