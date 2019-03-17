using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{
    [Serializable]
    public class Tipologica
    {
        public int id { get; set; }
        public string nome { get; set; }
        public string descrizione { get; set; }
        public string sottotipo { get; set; }
        public string parametri { get; set; }
        public bool attivo { get; set; }

        public Tipologica() { }
        public Tipologica(int id, string nome, string descrizione, string sottotipo, string parametri, bool attivo)
        {
            this.id = id;
            this.nome = nome;
            this.descrizione = descrizione;
            this.sottotipo = sottotipo;
            this.parametri = parametri;
            this.attivo = attivo;
        }
    }
}