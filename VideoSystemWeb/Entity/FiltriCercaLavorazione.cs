using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{
    public class FiltriCercaLavorazione
    {
        public string ClienteFornitore { get; set; }
        public string NumeroProtocollo { get; set; }
        public string RiferimentoDocumento { get; set; }
        public string Produzione { get; set; }
        public string Lavorazione { get; set; }
        public string Descrizione { get; set; }
        public string CodiceLavoro { get; set; }
        public string Destinatario { get; set; }
        public string Tipo { get; set; }
        public string DataLavorazione_Da { get; set; }
        public string DataLavorazione_A { get; set; }
        public string DataProtocollo_Da { get; set; }
        public string DataProtocollo_A { get; set; }
    }
}