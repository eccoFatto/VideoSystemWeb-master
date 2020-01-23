using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{
    public class DatiFiscaliLavorazione
    {
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

        public DateTime DataLavorazione { get => dataLavorazione; set => dataLavorazione = value; }
        public string Lavorazione { get => lavorazione; set => lavorazione = value; }
        public string Produzione { get => produzione; set => produzione = value; }
        public string Cliente { get => cliente; set => cliente = value; }
        public string Descrizione { get => descrizione; set => descrizione = value; }
        public int Quantita { get => quantita; set => quantita = value; }
        public decimal Assunzione { get => assunzione; set => assunzione = value; }
        public decimal Mista { get => mista; set => mista = value; }
        public int Diaria { get => diaria; set => diaria = value; }
        public decimal RitenutaAcconto { get => ritenutaAcconto; set => ritenutaAcconto = value; }
        public decimal Fattura { get => fattura; set => fattura = value; }
    }
}