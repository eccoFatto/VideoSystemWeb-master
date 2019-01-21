using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{
    public class Anag_Utente
    {
        public int id;
        public string Nome;
        public string Cognome;
        public string Descrizione;
        public string username;
        public string password;
        public Tipo_Utente tipo;
    }
}