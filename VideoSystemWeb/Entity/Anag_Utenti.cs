using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{
    public class Anag_Utenti
    {
        public int id;
        public string Nome;
        public string Cognome;
        public string Descrizione;
        public string username;
        public string password;
        public int id_tipoUtente;
        public string tipoUtente;
        public bool attivo;
    }
}