using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{
    [Serializable]
    public class ArticoliGruppi
    {
        private long id;
        private int idOggetto;
        private string nome;
        private string descrizione;
        private bool isGruppo;

        public long Id { get => id; set => id = value; }
        public int IdOggetto { get => idOggetto; set => idOggetto = value; }
        public string Nome { get => nome; set => nome = value; }
        public string Descrizione { get => descrizione; set => descrizione = value; }
        public bool Isgruppo { get => isGruppo; set => isGruppo = value; }
    }
}