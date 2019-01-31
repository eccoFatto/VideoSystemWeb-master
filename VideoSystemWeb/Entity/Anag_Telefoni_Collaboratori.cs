using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{
    //[id] [int] IDENTITY(1,1) NOT NULL,
    //[id_collaboratore] [int]NOT NULL,
    //[priorita] [int]NOT NULL,
    //[int_pref] [varchar] (5) NOT NULL,
    //[naz_pref] [varchar] (5) NOT NULL,
    //[numero] [varchar] (15) NOT NULL,
    //[tipo] [varchar] (30) NOT NULL,
    //[descrizione] [varchar] (60) NULL,
    //[whatsapp][bit]NOT NULL,
    //[attivo] [bit]NOT NULL,

    public class Anag_Telefoni_Collaboratori
    {
        private int id;
        private int id_collaboratore;
        private int priorita;
        private string pref_int;
        private string pref_naz;
        private string numero;
        private string tipo;
        private string descrizione;
        private bool whatsapp;
        private bool attivo;

        public int Id { get => id; set => id = value; }
        public int Id_collaboratore { get => id_collaboratore; set => id_collaboratore = value; }
        public int Priorita { get => priorita; set => priorita = value; }
        public string Pref_int { get => pref_int; set => pref_int = value; }
        public string Pref_naz { get => pref_naz; set => pref_naz = value; }
        public string Numero { get => numero; set => numero = value; }
        public string Tipo { get => tipo; set => tipo = value; }
        public string Descrizione { get => descrizione; set => descrizione = value; }
        public bool Whatsapp { get => whatsapp; set => whatsapp = value; }
        public bool Attivo { get => attivo; set => attivo = value; }
    }
}