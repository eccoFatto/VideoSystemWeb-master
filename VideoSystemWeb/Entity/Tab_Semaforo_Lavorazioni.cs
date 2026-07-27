using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{
    public class Tab_Semaforo_Lavorazioni
    {
        public int? Id { get; set; }
        public int? Id_Agenda { get; set; }
        public int? Id_Utente { get; set; }
        public string Nome_Utente { get; set; }
        public DateTime? Data_Accesso { get; set; }

        public Tab_Semaforo_Lavorazioni()
        {
            Id = null;
            Id_Agenda = null;
            Id_Utente = null;
            Nome_Utente = null;
            Data_Accesso = null;
        }
    }
}