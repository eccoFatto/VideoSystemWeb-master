using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{
    //@id int
	//@numeroDocumentoTrasporto [varchar](20) ,
	//@dataTrasporto [datetime] ,
	//@causale [varchar](50) ,
	//@destinatario [varchar](60) ,
	//@tipoIndirizzo [varchar](10) ,
	//@indirizzo [varchar](60) ,
	//@numeroCivico [varchar](10) ,
	//@cap [varchar](5) ,
	//@comune [varchar](50) ,
	//@provincia [varchar](2) ,
	//@nazione [varchar](20) ,
	//@partitaIva [varchar](11) ,
	//@numeroColli [int] ,
	//@peso [varchar] (10) ,
	//@trasportatore [varchar] (60) ,

    public class DocumentiTrasporto
    {
        private Int64 id;
        private string numeroDocumentoTrasporto;       
        private DateTime dataTrasporto;
        private string causale;
        private string destinatario;
        private string tipoIndirizzo;
        private string indirizzo;
        private string numeroCivico;
        private string cap;
        private string comune;
        private string provincia;
        private string nazione;
        private string partitaIva;
        private int numeroColli;
        private string peso;
        private string trasportatore;


        public Int64 Id { get => id; set => id = value; }
        public string NumeroDocumentoTrasporto { get => numeroDocumentoTrasporto; set => numeroDocumentoTrasporto = value; }
        public DateTime DataTrasporto { get => dataTrasporto; set => dataTrasporto = value; }
        public string Causale { get => causale; set => causale = value; }
        public string Destinatario { get => destinatario; set => destinatario = value; }
        public string TipoIndirizzo { get => tipoIndirizzo; set => tipoIndirizzo = value; }
        public string Indirizzo { get => indirizzo; set => indirizzo = value; }
        public string NumeroCivico { get => numeroCivico; set => numeroCivico = value; }
        public string Cap { get => cap; set => cap = value; }
        public string Comune { get => comune; set => comune = value; }
        public string Provincia { get => provincia; set => provincia = value; }
        public string Nazione { get => nazione; set => nazione = value; }
        public string PartitaIva { get => partitaIva; set => partitaIva = value; }
        public int NumeroColli { get => numeroColli; set => numeroColli = value; }
        public string Peso { get => peso; set => peso = value; }
        public string Trasportatore { get => trasportatore; set => trasportatore = value; }
    }
}