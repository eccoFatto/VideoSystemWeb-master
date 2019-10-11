using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{
    [Serializable]
    public class FiguraProfessionale
    {
        private int id;
        private long identificatoreOggetto;
        private int? idCollaboratori;
        private int? idFornitori;
        private string intervento;
        private DateTime? data;
        private decimal? diaria;
        private string nome;
        private string cognome;
        private string telefono;
        private string citta;
        private List<Anag_Qualifiche_Collaboratori> qualifiche;
        private int tipo;
        private string nota;
        private decimal? netto;
        private decimal? lordo;
        private int? tipoPagamento;
        private bool isAssunto;
        private string descrizioneArticoloAssociato;
       
        public int Id { get => id; set => id = value; }
        public long IdentificatoreOggetto { get => identificatoreOggetto; set => identificatoreOggetto = value; }
        public int? IdCollaboratori { get => idCollaboratori; set => idCollaboratori = value; }
        public int? IdFornitori { get => idFornitori; set => idFornitori = value; }
        public string Intervento { get => intervento; set => intervento = value; }
        public DateTime? Data { get => data; set => data = value; }
        public decimal? Diaria { get => diaria; set => diaria = value; }
        public string Nome { get => nome; set => nome = value; }
        public string Cognome { get => cognome; set => cognome = value; }
        public string Telefono { get => telefono; set => telefono = value; }
        public string Citta { get => citta; set => citta = value; }
        public List<Anag_Qualifiche_Collaboratori> Qualifiche { get => qualifiche; set => qualifiche = value; }
        public int Tipo { get => tipo; set => tipo = value; }
        public string Nota { get => nota; set => nota = value; }
        public decimal? Netto { get => netto; set => netto = value; }
        public decimal? Lordo { get => lordo; set => lordo = value; }
        public string NominativoCompleto
        {
            get
            {
                return cognome + " " + nome;
            }
        }
        public string DecodificaTipo
        {
            get
            {
                return tipo == 0 ? "Collaboratore" : "Fornitore";
            }
        }
        public string ElencoQualifiche
        {
            get
            {
                string elencoQualifiche = "";

                if (qualifiche != null)
                { 
                    if (qualifiche.Count == 1)
                    {
                        elencoQualifiche = qualifiche.ElementAt(0).Qualifica;
                    }
                    else if (qualifiche.Count > 1)
                    {
                        foreach (Anag_Qualifiche_Collaboratori qualColl in qualifiche)
                        {
                            elencoQualifiche += qualColl.Qualifica + "; ";
                        }

                        elencoQualifiche = elencoQualifiche.Substring(0, elencoQualifiche.Length - 2);
                    }
                }
                return elencoQualifiche;
            }
        }
        public int? TipoPagamento { get => tipoPagamento; set => tipoPagamento = value; }
        public bool IsAssunto { get => isAssunto; set => isAssunto = value; }
        public string DescrizioneArticoloAssociato { get => descrizioneArticoloAssociato; set => descrizioneArticoloAssociato = value; }
    }
}