using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{
    [Serializable]
    public class DatiAgenda
    {
        public const int STATO_PREVISIONE_IMPEGNO = 1;
        public const int STATO_OFFERTA = 2;
        public const int STATO_LAVORAZIONE = 3;
        public const int STATO_FATTURA = 4;
        public const int STATO_RIPOSO = 5;

        private DateTime? _data_inizio_impegno;
        private DateTime? _data_fine_impegno;

        public int id { get; set; }
        public DateTime data_inizio_lavorazione { get; set; }
        public DateTime data_fine_lavorazione { get; set; }
        public int durata_lavorazione { get; set; }
        public int id_colonne_agenda { get; set; }
        public int? id_tipologia { get; set; }
        public int? id_cliente { get; set; }
        public int durata_viaggio_andata { get; set; }
        public int durata_viaggio_ritorno { get; set; }
        public DateTime? data_inizio_impegno
        {
            get { return _data_inizio_impegno; }
            set
            {
                if (value == DateTime.MinValue)
                {
                    _data_inizio_impegno = null;
                }
                else
                {          
                    _data_inizio_impegno = value;                   
                }
            }
        }
        public DateTime? data_fine_impegno
        {
            get { return _data_fine_impegno; }
            set
            {
                if (value == DateTime.MinValue)
                {
                    _data_fine_impegno = null;
                }
                else
                {
                    if (value != null)
                    {
                        _data_fine_impegno = value;
                    }
                }
            }
        }
        public bool impegnoOrario { get; set; }
        public string impegnoOrario_da { get; set; }

        public string impegnoOrario_a { get; set; }

        public string produzione { get; set; }
        public string lavorazione { get; set; }
        public string indirizzo { get; set; }
        public string luogo { get; set; }
        public string codice_lavoro { get; set; }

        public string nota { get; set; }

        public int id_stato { get; set; }

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