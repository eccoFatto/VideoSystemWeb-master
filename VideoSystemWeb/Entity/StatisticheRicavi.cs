using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{
    public class StatisticheRicavi
    {
        private int idCliente;
        private string cliente;
        private string numeroFattura;
        private string ordine;
        private string codiceLavoro;
        private DateTime? data;
        private string lavorazione;
        private string produzione;
        private string contratto;
        private decimal? listino;
        private decimal? costo;
        private decimal ricavo;

        public int IdCliente { get => idCliente; set => idCliente = value; }
        public string Cliente { get => cliente; set => cliente = value; }
        public string NumeroFattura { get => numeroFattura; set => numeroFattura = value; }
        public string Ordine { get => ordine; set => ordine = value; }
        public string CodiceLavoro { get => codiceLavoro; set => codiceLavoro = value; }
        public DateTime? Data { get => data; set => data = value; }
        public string Lavorazione { get => lavorazione; set => lavorazione = value; }
        public string Produzione { get => produzione; set => produzione = value; }
        public string Contratto { get => contratto; set => contratto = value; }
        public decimal? Listino 
        {
            get
            {
                return listino == null ? 0 : listino;
            }
            set => listino = value; 
        }
        public decimal? Costo 
        {
            get
            {
                return costo == null ? 0 : costo;
            }
            set => costo = value; 
        }
        public decimal Ricavo 
        {
            get
            {
                decimal _listino;
                decimal _costo;

                bool isOkListino = decimal.TryParse(listino.ToString(), out _listino);
                bool isOkCosto = decimal.TryParse(costo.ToString(), out _costo);

                if (isOkListino && isOkCosto && listino > 0)
                    return (_listino - _costo) / _listino; // non moltiplicare per 100: viene fatto in fase di visualizzazione
                else
                    return 0;
            }
        }

        
    }
}