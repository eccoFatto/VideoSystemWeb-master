using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{
    [Serializable]
    public class DatiArticoliLavorazione
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
        //[quantita] [int] NOT NULL,

        private int id;
        private int idDatiLavorazione;
        private int idArtArticoli;
        private int idTipoGenere;
        private int idTipoGruppo;
        private int idTipoSottogruppo;
        private int? idCollaboratori;
        private int? idFornitori;
        private int? idTipoPagamento;
        private string descrizione;
        private string descrizioneLunga;
        private bool stampa;
        private decimal prezzo;
        private decimal costo;
        private int iva;
        private DateTime? data;
        private int? tv;

        private long identificatoreOggetto;


        public int Id { get => id; set => id = value; }
        public int IdDatiLavorazione { get => idDatiLavorazione; set => idDatiLavorazione = value; }
        public int IdArtArticoli { get => idArtArticoli; set => idArtArticoli = value; }
        public int IdTipoGenere { get => idTipoGenere; set => idTipoGenere = value; }
        public int IdTipoGruppo { get => idTipoGruppo; set => idTipoGruppo = value; }
        public int IdTipoSottogruppo { get => idTipoSottogruppo; set => idTipoSottogruppo = value; }
        public int? IdCollaboratori { get => idCollaboratori; set => idCollaboratori = value; }
        public int? IdFornitori { get => idFornitori; set => idFornitori = value; }
        public int? IdTipoPagamento { get => idTipoPagamento; set => idTipoPagamento = value; }
        public string Descrizione { get => descrizione; set => descrizione = value; }
        public string DescrizioneLunga { get => descrizioneLunga; set => descrizioneLunga = value; }
        public bool Stampa { get => stampa; set => stampa = value; }
        public decimal Prezzo { get => prezzo; set => prezzo = value; }
        public decimal Costo { get => costo; set => costo = value; }
        public int Iva { get => iva; set => iva = value; }
        public DateTime? Data { get => data; set => data = value; }
        public int? Tv { get => tv; set => tv = value; }

        public long IdentificatoreOggetto { get => identificatoreOggetto; set => identificatoreOggetto = value; }
    }
}