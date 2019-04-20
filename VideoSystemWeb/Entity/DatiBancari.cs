using System;

namespace VideoSystemWeb.Entity
{
    [Serializable]
    public class DatiBancari
    {
        private string banca;
        private string iban;

        public string Banca { get => banca; set => banca = value; }
        public string Iban { get => iban; set => iban = value; }
    }

    
}