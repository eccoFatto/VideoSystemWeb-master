using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{
    public class DatiAgenda
    {
        public int id;

        public DateTime data_inizio_lavorazione;
        public DateTime data_fine_lavorazione;
        public int durata_lavorazione;

        public int id_colonne_agenda;
        public int id_tipologia;
        public int id_cliente;

        public int durata_viaggio_andata;
        public int durata_viaggio_ritorno;
        public DateTime data_inizio_impegno;
        public DateTime data_fine_impegno;
        public bool impegnoOrario;
        public DateTime impegnoOrario_da;
        public DateTime impegnoOrario_a;
        
        public string produzione;
        public string lavorazione;
        public string indirizzo;
        public string luogo;
        public string codice_lavoro;

        public string nota;

        public int id_stato;

        public DatiAgenda() { }

        public DatiAgenda(int id, int id_colonne_agenda, int id_stato, DateTime data_inizio_lavorazione, DateTime data_fine_lavorazione, string produzione)
        {
            this.id = id;
            this.id_colonne_agenda = id_colonne_agenda;
            this.id_stato = id_stato;
            this.data_inizio_lavorazione = data_inizio_lavorazione;
            this.data_fine_lavorazione = data_fine_lavorazione;

            this.produzione = produzione;
        }
    }
}