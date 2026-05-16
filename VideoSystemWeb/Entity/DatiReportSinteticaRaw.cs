using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{
    public class DatiReportSinteticaRaw
    {
        private int idCollaboratore;
        private string nomeCollaboratore;
        private string indirizzoCollaboratore;
        private string cittaCollaboratore;
        private string telefonoCollaboratore;
        private string codFiscaleCollaboratore;
       // private string lavorazione;

        private string intervalloDate;
        private decimal assunzione;
        private decimal rimborsoKm;
        private decimal diaria;
        private int albergo;

        public int IdCollaboratore { get => idCollaboratore; set => idCollaboratore = value; }
        public string NomeCollaboratore { get => nomeCollaboratore; set => nomeCollaboratore = value; }
        public string IndirizzoCollaboratore { get => indirizzoCollaboratore; set => indirizzoCollaboratore = value; }
        public string CittaCollaboratore { get => cittaCollaboratore; set => cittaCollaboratore = value; }
        public string TelefonoCollaboratore { get => telefonoCollaboratore; set => telefonoCollaboratore = value; }
        public string CodFiscaleCollaboratore { get => codFiscaleCollaboratore; set => codFiscaleCollaboratore = value; }
        //public string Lavorazione { get => lavorazione; set => lavorazione = value; }

        public string IntervalloDate { get => intervalloDate; set => intervalloDate = value; }
        public decimal Assunzione { get => assunzione; set => assunzione = value; }
        public decimal RimborsoKm { get => rimborsoKm; set => rimborsoKm = value; }
        public decimal Diaria { get => diaria; set => diaria = value; }
        public int Albergo { get => albergo; set => albergo = value; }
        
    }
}