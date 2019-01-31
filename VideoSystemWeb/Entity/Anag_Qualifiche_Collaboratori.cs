using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{
//[id] [int] IDENTITY(1,1) NOT NULL,
//[id_collaboratore] [int]NOT NULL,
//[priorita] [int] NOT NULL,
//[qualifica] [varchar] (50) NOT NULL,
//[descrizione] [varchar] (60) NULL,
//[attivo][bit] NOT NULL,

    public class Anag_Qualifiche_Collaboratori
    {
        private int id;
        private int id_collaboratore;
        private int priorita;
        private string qualifica;
        private string descrizione;
        private bool attivo;

        public bool Attivo { get => attivo; set => attivo = value; }
        public string Descrizione { get => descrizione; set => descrizione = value; }
        public string Qualifica { get => qualifica; set => qualifica = value; }
        public int Priorita { get => priorita; set => priorita = value; }
        public int Id_collaboratore { get => id_collaboratore; set => id_collaboratore = value; }
        public int Id { get => id; set => id = value; }
    }
}