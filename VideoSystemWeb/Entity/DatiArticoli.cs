using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{
    [Serializable]
    public class DatiArticoli
    {

        //[id] [int] IDENTITY(1,1) NOT NULL,
        //[idArtArticoli] [int] NOT NULL,
        //[idTipoGenere] [int] NOT NULL,
        //[idTipoGruppo] [int] NOT NULL,
        //[idTipoSottogruppo] [int] NOT NULL,
        //[idDatiAgenda] [int] NOT NULL,
        //[stampa] [bit] NOT NULL,
        //[prezzo] [decimal](18, 0) NOT NULL,
        //[costo] [decimal](18, 0) NOT NULL,
        //[iva] [int] NOT NULL,

        private int id;
        private int idArtArticoli;
        private int idTipoGenere;
        private int idTipoGruppo;
        private int idTipoSottogruppo;
        private int idDatiAgenda;
        private bool stampa;
        private decimal prezzo;
        private decimal costo;
        private int iva;

        public int Id { get => id; set => id = value; }
        public int IdArtArticoli { get => idArtArticoli; set => idArtArticoli = value; }
        public int IdTipoGenere { get => idTipoGenere; set => idTipoGenere = value; }
        public int IdTipoGruppo { get => idTipoGruppo; set => idTipoGruppo = value; }
        public int IdTipoSottogruppo { get => idTipoSottogruppo; set => idTipoSottogruppo = value; }
        public int IdDatiAgenda { get => idDatiAgenda; set => idDatiAgenda = value; }
        public bool Stampa { get => stampa; set => stampa = value; }
        public decimal Prezzo { get => prezzo; set => prezzo = value; }
        public decimal Costo { get => costo; set => costo = value; }
        public int Iva { get => iva; set => iva = value; }
    }
}