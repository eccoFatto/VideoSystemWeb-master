using System;

namespace VideoSystemWeb.Entity
{
    [Serializable]
    public class GiorniPagamentoFatture
    {
        private string giorni;
        private string descrizione;

        public string Giorni { get => giorni; set => giorni = value; }
        public string Descrizione { get => descrizione; set => descrizione = value; }
    }

    
}