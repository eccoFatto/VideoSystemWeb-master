using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{
    public class DatiReportRaw
    {
        private int idCollaboratore;
        private string nomeCollaboratore;
        private string qualificaCollaboratore;
        private string indirizzoCollaboratore;
        private string cittaCollaboratore;
        private string telefonoCollaboratore;
        private string cellulareCollaboratore;
        private string iscrizioneCollaboratore;
        private string codFiscaleCollaboratore;

        private DateTime dataLavorazione;
        private string lavorazione;
        private string produzione;
        private string cliente;
        private string descrizione;
        private int quantita;
        private decimal assunzione;
        private decimal mista;
        private decimal ritenutaAcconto;
        private decimal fattura;
        private int diaria;
        private int albergo;

        private int tipoPagamento;
        private string descrizionePagamento;
        private decimal rimborsoKm;

        public int IdCollaboratore { get => idCollaboratore; set => idCollaboratore = value; }
        public string NomeCollaboratore { get => nomeCollaboratore; set => nomeCollaboratore = value; }
        public string QualificaCollaboratore { get => qualificaCollaboratore; set => qualificaCollaboratore = value; }
        public string IndirizzoCollaboratore { get => indirizzoCollaboratore; set => indirizzoCollaboratore = value; }
        public string CittaCollaboratore { get => cittaCollaboratore; set => cittaCollaboratore = value; }
        public string TelefonoCollaboratore { get => telefonoCollaboratore; set => telefonoCollaboratore = value; }
        public string CellulareCollaboratore { get => cellulareCollaboratore; set => cellulareCollaboratore = value; }
        public string IscrizioneCollaboratore { get => iscrizioneCollaboratore; set => iscrizioneCollaboratore = value; }
        public string CodFiscaleCollaboratore { get => codFiscaleCollaboratore; set => codFiscaleCollaboratore = value; }
        public DateTime DataLavorazione { get => dataLavorazione; set => dataLavorazione = value; }
        public string Lavorazione { get => lavorazione; set => lavorazione = value; }
        public string Produzione { get => produzione; set => produzione = value; }
        public string Cliente { get => cliente; set => cliente = value; }
        public string Descrizione { get => descrizione; set => descrizione = value; }
        public int Quantita { get => quantita; set => quantita = value; }
        public decimal Assunzione { get => assunzione; set => assunzione = value; }
        public decimal Mista { get => mista; set => mista = value; }
        public decimal RitenutaAcconto { get => ritenutaAcconto; set => ritenutaAcconto = value; }
        public decimal Fattura { get => fattura; set => fattura = value; }
        public int Diaria { get => diaria; set => diaria = value; }
        public int Albergo { get => albergo; set => albergo = value; }
        public int TipoPagamento { get => tipoPagamento; set => tipoPagamento = value; }
        public string DescrizionePagamento { get => descrizionePagamento; set => descrizionePagamento = value; }
        public decimal RimborsoKm { get => rimborsoKm; set => rimborsoKm = value; }
    }
}