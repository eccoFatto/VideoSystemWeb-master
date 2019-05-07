using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{
    //[id] [int] IDENTITY(1,1) NOT NULL,
    //[id_collaboratore] [int]    NOT NULL,
    //[priorita] [int]    NOT NULL,
    //[indirizzoEmail] [varchar] (60) NOT NULL,
    //[descrizione] [varchar] (60) NULL,
    //[attivo]    [bit]    NOT NULL,
    [Serializable]
    public class Anag_Email_Collaboratori
    {
        private int id;
        private int id_collaboratore;
        private int priorita;
        private string indirizzoEmail;
        private string descrizione;
        private bool attivo;

        public int Id { get => id; set => id = value; }
        public int Id_collaboratore { get => id_collaboratore; set => id_collaboratore = value; }
        public int Priorita { get => priorita; set => priorita = value; }
        public string IndirizzoEmail { get => indirizzoEmail; set => indirizzoEmail = value; }
        public string Descrizione { get => descrizione; set => descrizione = value; }
        public bool Attivo { get => attivo; set => attivo = value; }
    }
}