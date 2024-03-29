﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.BLL;

namespace VideoSystemWeb.Entity
{
    [Serializable]
    public class DatiAgenda
    {
        public int id { get; set; }
        public DateTime data_inizio_lavorazione { get; set; }
        public DateTime data_fine_lavorazione { get; set; }
        public int durata_lavorazione { get; set; }
        public int id_colonne_agenda { get; set; }
        public int? id_tipologia { get; set; }
        public int id_cliente { get; set; }
        public int durata_viaggio_andata { get; set; }
        public int durata_viaggio_ritorno { get; set; }
        public DateTime data_inizio_impegno { get; set; }
        public DateTime data_fine_impegno { get; set; }
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
        public string numeroFattura { get; set; }

        public List<DatiArticoli> ListaDatiArticoli { get; set; }
        public DatiLavorazione LavorazioneCorrente { get; set; }

        public string DecodificaTipologia
        {
            get
            {
                if (id_tipologia == 0)
                {
                    return "";
                }
                else
                {
                    return (((List<Tipologica>)SessionManager.ListaTipiTipologie).FirstOrDefault(x => x.id == id_tipologia)).nome;
                }
            }
        }

        public string DecodificaCliente
        {
            get
            {
                if (id_cliente == 0)
                {
                    return "";
                }
                else
                {
                    return (((List<Anag_Clienti_Fornitori>)SessionManager.ListaClientiFornitori).FirstOrDefault(x => x.Id == id_cliente)).RagioneSociale;
                }
            }
        }

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