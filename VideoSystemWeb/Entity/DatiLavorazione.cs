using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.BLL;

namespace VideoSystemWeb.Entity
{
    [Serializable]
    public class DatiLavorazione
    {
        private int id;
        private int idDatiAgenda;
        private int? idContratto;
        private int? idReferente;
        private int? idCapoTecnico;
        private int? idProduttore;
        private string ordine;
        private string fattura;
        private List<DatiArticoliLavorazione> listaArticoliLavorazione;

        public int Id { get => id; set => id = value; }
        public int IdDatiAgenda { get => idDatiAgenda; set => idDatiAgenda = value; }
        public int? IdContratto { get => idContratto; set => idContratto = value; }
        public int? IdReferente { get => idReferente; set => idReferente = value; }
        public int? IdCapoTecnico { get => idCapoTecnico; set => idCapoTecnico = value; }
        public int? IdProduttore { get => idProduttore; set => idProduttore = value; }
        public string Ordine { get => ordine; set => ordine = value; }
        public string Fattura { get => fattura; set => fattura = value; }
        public List<DatiArticoliLavorazione> ListaArticoliLavorazione { get => listaArticoliLavorazione; set => listaArticoliLavorazione = value; }
    }
}