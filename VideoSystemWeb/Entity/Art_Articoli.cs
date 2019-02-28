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
        private int defaultIdTipoGenere;
        private int defaultIdTipoGruppo;
        private int defaultIdTipoSottogruppo;
        private string defaultDescrizione;
        private string defaultDescrizioneLunga;
        private bool defaultStampa;
        private decimal defaultPrezzo;
        private decimal defaultCosto;
        private int defaultIva;
        private bool attivo;

        public int Id { get => id; set => id = value; }
        public int DefaultIdTipoGenere { get => defaultIdTipoGenere; set => defaultIdTipoGenere = value; }
        public int DefaultIdTipoGruppo { get => defaultIdTipoGruppo; set => defaultIdTipoGruppo = value; }
        public int DefaultIdTipoSottogruppo { get => defaultIdTipoSottogruppo; set => defaultIdTipoSottogruppo = value; }
        public string DefaultDescrizione { get => defaultDescrizione; set => defaultDescrizione = value; }
        public string DefaultDescrizioneLunga { get => defaultDescrizioneLunga; set => defaultDescrizioneLunga = value; }
        public bool DefaultStampa { get => defaultStampa; set => defaultStampa = value; }
        public decimal DefaultPrezzo { get => defaultPrezzo; set => defaultPrezzo = value; }
        public decimal DefaultCosto { get => defaultCosto; set => defaultCosto = value; }
        public int DefaultIva { get => defaultIva; set => defaultIva = value; }
        public bool Attivo { get => attivo; set => attivo = value; }
    }
}