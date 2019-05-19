using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{
    [Serializable]
    public class DatiPianoEsternoLavorazione
    {
        private int id;
        private int idDatiLavorazione;
        private int? idCollaboratori;
        private int? idFornitori;
        private int? idIntervento;
        private bool? diaria;
        private decimal? importoDiaria;
        private bool? albergo;
        private DateTime? data;
        private DateTime? orario;
        private string nota;


        public int Id { get => id; set => id = value; }
        public int IdDatiLavorazione { get => idDatiLavorazione; set => idDatiLavorazione = value; }
        public int? IdCollaboratori { get => idCollaboratori; set => idCollaboratori = value; }
        public int? IdFornitori { get => idFornitori; set => idFornitori = value; }
        public int? IdIntervento { get => idIntervento; set => idIntervento = value; }
        public bool? Diaria { get => diaria; set => diaria = value; }
        public decimal? ImportoDiaria { get => importoDiaria; set => importoDiaria = value; }
        public bool? Albergo { get => albergo; set => albergo = value; }
        public DateTime? Data { get => data; set => data = value; }
        public DateTime? Orario { get => orario; set => orario = value; }
        public string Nota { get => nota; set => nota = value; }

    }
}