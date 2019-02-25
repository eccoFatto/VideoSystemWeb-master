using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{
    [Serializable]
    public class Art_Articoli
    {
        /*
        [id] [int] IDENTITY(1,1) NOT NULL,
        [descrizione] [varchar] (60) NOT NULL,
        [descrizioneLunga] [varchar] (100) NOT NULL,
        [defaultStampa] [bit] NOT NULL,
        [defaultPrezzo] [decimal](18, 0) NOT NULL,
        [defaultCosto] [decimal](18, 0) NOT NULL,
        [defaultIva] [int] NOT NULL,
        [attivo] [bit] NOT NULL,
        */
        private int id;
        private string descrizione;
        private string descrizioneLunga;
        private bool defaultStampa;
        private decimal defaultPrezzo;
        private decimal defaultCosto;
        private int defaultIva;
        private bool attivo;

        public int Id { get => id; set => id = value; }
        public string Descrizione { get => descrizione; set => descrizione = value; }
        public string DescrizioneLunga { get => descrizioneLunga; set => descrizioneLunga = value; }
        public bool DefaultStampa { get => defaultStampa; set => defaultStampa = value; }
        public decimal DefaultPrezzo { get => defaultPrezzo; set => defaultPrezzo = value; }
        public decimal DefaultCosto { get => defaultCosto; set => defaultCosto = value; }
        public int DefaultIva { get => defaultIva; set => defaultIva = value; }
        public bool Attivo { get => attivo; set => attivo = value; }
    }
}