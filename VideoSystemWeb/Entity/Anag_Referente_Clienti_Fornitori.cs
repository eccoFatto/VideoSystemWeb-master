using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{
    //[id] [int] IDENTITY(1,1) NOT NULL,
    //[id_azienda] [int] NOT NULL,
    //[cognome] [varchar] (50) NOT NULL,
    //[nome] [varchar] (50) NOT NULL,
    //[settore] [varchar] (30) NOT NULL,
    //[telefono1] [varchar] (25) NOT NULL,
    //[telefono2] [varchar] (25) NOT NULL,
    //[cellulare] [varchar] (25) NOT NULL,
    //[email] [varchar] (60) NOT NULL,
    //[note] [varchar] (200) NOT NULL
    public class Anag_Referente_Clienti_Fornitori
    {
        private int id;
        private int id_azienda;
        private string cognome;
        private string nome;
        private string settore;
        private string telefono1;
        private string telefono2;
        private string cellulare;
        private string email;
        private string note;
        private bool attivo;

        public int Id { get => id; set => id = value; }
        public int Id_azienda { get => id_azienda; set => id_azienda = value; }
        public string Cognome { get => cognome; set => cognome = value; }
        public string Nome { get => nome; set => nome = value; }
        public string Settore { get => settore; set => settore = value; }
        public string Telefono1 { get => telefono1; set => telefono1 = value; }
        public string Telefono2 { get => telefono2; set => telefono2 = value; }
        public string Cellulare { get => cellulare; set => cellulare = value; }
        public string Email { get => email; set => email = value; }
        public string Note { get => note; set => note = value; }
        public bool Attivo { get => attivo; set => attivo = value; }
    }
}