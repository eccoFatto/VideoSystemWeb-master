using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{

    public class Utenti
    {
        private int id;
        private string cognome;
        private string nome;
        private string descrizione;
        private int id_tipoUtente;
        private string username;
        private string password;
        private string email;
        private bool attivo;

        public int Id { get => id; set => id = value; }
        public string Cognome { get => cognome; set => cognome = value; }
        public string Nome { get => nome; set => nome = value; }
        public string Descrizione { get => descrizione; set => descrizione = value; }
        public int Id_tipoUtente { get => id_tipoUtente; set => id_tipoUtente = value; }
        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public bool Attivo { get => attivo; set => attivo = value; }
        public string Email { get => email; set => email = value; }
    }
}