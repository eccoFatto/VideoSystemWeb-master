using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{
    public class Tipologica
    {
        public int id;
        public string nome;
        public string descrizione;
        public string sottotipo;
        public string parametri;

        public Tipologica(int id, string nome, string descrizione, string sottotipo, string parametri)
        {
            this.id = id;
            this.nome = nome;
            this.descrizione = descrizione;
            this.sottotipo = sottotipo;
            this.parametri = parametri;
        }
    }
}