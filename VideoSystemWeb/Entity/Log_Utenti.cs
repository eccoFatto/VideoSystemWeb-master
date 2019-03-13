using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{
    [Serializable]
    public class Log_Utenti
    {
        /*
	@id int output
    @idUtente int,
	@nomeUtente varchar(50),
	@nomeTabella varchar(50),
	@tipoOperazione varchar(50),
        */
        private int id;
        private int idUtente;
        private string nomeUtente;
        private string nomeTabella;
        private string tipoOperazione;

        public int Id { get => id; set => id = value; }
        public int IdUtente { get => idUtente; set => idUtente = value; }
        public string NomeUtente { get => nomeUtente; set => nomeUtente = value; }
        public string NomeTabella { get => nomeTabella; set => nomeTabella = value; }
        public string TipoOperazione { get => tipoOperazione; set => tipoOperazione = value; }
    }
}