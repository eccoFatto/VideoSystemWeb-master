using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{

    
//  [id] [int] IDENTITY(1,1) NOT NULL,
//  [cognome] [varchar] (50) NOT NULL,
//  [nome] [varchar] (50) NOT NULL,
//  [codiceFiscale] [varchar] (16) NOT NULL,
//  [pathFoto] [varchar] (100) NULL,
//  [nazione] [varchar] (50) NOT NULL,
//  [comuneNascita] [varchar] (50) NOT NULL,
//  [provinciaNascita] [varchar] (2) NOT NULL,
//  [dataNascita] [datetime] NOT NULL,
//  [comuneRiferimento] [varchar] (50) NOT NULL,
//  [partitaIva] [varchar] (11) NULL,
//  [nomeSocieta] [varchar] (50) NULL,
//  [assunto] [bit] NOT NULL,
//  [note] [varchar](max) NULL,
//  [attivo] [bit] NOT NULL,

    public class Anag_Collaboratori
    {
        private int id;
        private string cognome;
        private string nome;
        private string codiceFiscale;
        private string pathFoto;
        private string nazione;
        private string comuneNascita;
        private string provinciaNascita;
        private DateTime dataNascita;
        private string comuneRiferimento;
        private string partitaIva;
        private string nomeSocieta;
        private bool assunto;
        private string note;
        private bool attivo;
        private List<Anag_Qualifiche_Collaboratori> qualifiche;

        public bool Attivo { get => attivo; set => attivo = value; }
        public string Note { get => note; set => note = value; }
        public bool Assunto { get => assunto; set => assunto = value; }
        public string NomeSocieta { get => nomeSocieta; set => nomeSocieta = value; }
        public string PartitaIva { get => partitaIva; set => partitaIva = value; }
        public string ComuneRiferimento { get => comuneRiferimento; set => comuneRiferimento = value; }
        public DateTime DataNascita { get => dataNascita; set => dataNascita = value; }
        public string ProvinciaNascita { get => provinciaNascita; set => provinciaNascita = value; }
        public string ComuneNascita { get => comuneNascita; set => comuneNascita = value; }
        public string Nazione { get => nazione; set => nazione = value; }
        public string PathFoto { get => pathFoto; set => pathFoto = value; }
        public string CodiceFiscale { get => codiceFiscale; set => codiceFiscale = value; }
        public string Nome { get => nome; set => nome = value; }
        public string Cognome { get => cognome; set => cognome = value; }
        public int Id { get => id; set => id = value; }
        public List<Anag_Qualifiche_Collaboratori> Qualifiche { get => qualifiche; set => qualifiche = value; }
    }
}