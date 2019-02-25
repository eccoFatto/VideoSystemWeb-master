using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{
    [Serializable]
    public class Art_Gruppi
    {
        /*
	    [id] [int] IDENTITY(1,1) NOT NULL,
	    [nome] [varchar](50) NOT NULL,
	    [descrizione] [varchar](100) NULL,
	    [sottotipo] [varchar](50) NULL,
	    [parametri] [varchar](100) NULL,
	    [attivo] [bit] NOT NULL,
        */
        private int id;
        private string nome;
        private string descrizione;
        private string sottoTipo;
        private string parametri;
        private bool attivo;

        public int Id { get => id; set => id = value; }
        public string Descrizione { get => descrizione; set => descrizione = value; }
        public bool Attivo { get => attivo; set => attivo = value; }
        public string Nome { get => nome; set => nome = value; }
        public string SottoTipo { get => sottoTipo; set => sottoTipo = value; }
        public string Parametri { get => parametri; set => parametri = value; }
    }
}