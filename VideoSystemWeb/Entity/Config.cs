using System;

namespace VideoSystemWeb.Entity
{
    [Serializable]
    public class Config
    {
        private string chiave;
        public string valore;
        private string descrizione;
        private int ordinamento;

        public string Chiave { get => chiave; set => chiave = value; }
        public string Valore { get => valore; set => valore = value; }
        public string Descrizione { get => descrizione; set => descrizione = value; }
        public int Ordinamento { get => ordinamento; set => ordinamento = value; }
    }

    
}