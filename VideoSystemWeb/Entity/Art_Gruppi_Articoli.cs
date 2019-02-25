using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{
    [Serializable]
    public class Art_Gruppi_Articoli
    {
        /*
	    [id] [int] IDENTITY(1,1) NOT NULL,
	    [idArtGruppi] [int] NOT NULL,
	    [idArtArticoli] [int] NOT NULL,
        */
        private int id;
        private int idArtGruppi;
        private int idArtArticoli;

        public int Id { get => id; set => id = value; }
        public int IdArtGruppi { get => idArtGruppi; set => idArtGruppi = value; }
        public int IdArtArticoli { get => idArtArticoli; set => idArtArticoli = value; }
    }
}