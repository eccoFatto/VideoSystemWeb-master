using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{
    //[id] [int] IDENTITY(1,1) NOT NULL,
    //[id_collaboratore] [int] NOT NULL,
    //[priorita] [int] NULL,
    //[tipo] [varchar] (10) NOT NULL,
    //[indirizzo] [varchar] (60) NOT NULL,
    //[numeroCivico] [varchar] (10) NULL,
    //[cap] [varchar] (5) NOT NULL,
    //[comune] [varchar] (50) NOT NULL,
    //[provincia] [varchar] (2) NULL,
    //[nazione] [varchar] (20) NULL,
    //[descrizione] [varchar] (60) NULL,
    //[attivo] [bit]NOT NULL,

    public class Anag_Indirizzi_Collaboratori
    {
        private int id;
        private int id_Collaboratore;
        private int priorita;
        private string tipo;
        private string indirizzo;
        private string numeroCivico;
        private string cap;
        private string comune;
        private string provincia;
        private string nazione;
        private string descrizione;
        private bool attivo;

        public int Id { get => id; set => id = value; }
        public int Id_Collaboratore { get => id_Collaboratore; set => id_Collaboratore = value; }
        public int Priorita { get => priorita; set => priorita = value; }
        public string Tipo { get => tipo; set => tipo = value; }
        public string Indirizzo { get => indirizzo; set => indirizzo = value; }
        public string NumeroCivico { get => numeroCivico; set => numeroCivico = value; }
        public string Cap { get => cap; set => cap = value; }
        public string Comune { get => comune; set => comune = value; }
        public string Provincia { get => provincia; set => provincia = value; }
        public string Nazione { get => nazione; set => nazione = value; }
        public string Descrizione { get => descrizione; set => descrizione = value; }
        public bool Attivo { get => attivo; set => attivo = value; }
    }
}