using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{
	//[id] [int] IDENTITY(1,1) NOT NULL,
	//[idMagAttrezzature] [int] NULL,
	//[idDocumentoTrasporto] [int] NOT NULL,
	//[cod_vs] [varchar](50) NOT NULL,
	//[descrizione] [varchar](100) NOT NULL,
	//[quantita] [int] NOT NULL

    public class AttrezzatureTrasporto
    {
        private Int64 id;
        private Int64 idMagAttrezzature;
        private Int64 idDocumentoTrasporto;
        private string cod_vs;
        private string descrizione;
        private int quantita;



        public Int64 Id { get => id; set => id = value; }
        public long IdMagAttrezzature { get => idMagAttrezzature; set => idMagAttrezzature = value; }
        public long IdDocumentoTrasporto { get => idDocumentoTrasporto; set => idDocumentoTrasporto = value; }
        public string Cod_vs { get => cod_vs; set => cod_vs = value; }
        public string Descrizione { get => descrizione; set => descrizione = value; }
        public int Quantita { get => quantita; set => quantita = value; }
    }
}