using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{
    //[id] [int] IDENTITY(1,1) NOT NULL,
    //[ragioneSociale] [varchar] (60) NOT NULL,
    //[partitaIva] [varchar] (11) NOT NULL,
    //[codiceFiscale] [varchar] (16) NOT NULL,
    //[codiceIdentificativo] [varchar] (10) NOT NULL,
    //[tipoIndirizzoLegale] [varchar] (10) NOT NULL,
    //[indirizzoLegale] [varchar] (60) NOT NULL,
    //[numeroCivicoLegale] [varchar] (10) NOT NULL,
    //[capLegale] [varchar] (5) NOT NULL,
    //[comuneLegale] [varchar] (50) NOT NULL,
    //[provinciaLegale] [varchar] (2) NOT NULL,
    //[nazioneLegale] [varchar] (20) NOT NULL,
    //[tipoIndirizzoOperativo] [varchar] (10) NOT NULL,
    //[indirizzoOperativo] [varchar] (60) NOT NULL,
    //[numeroCivicoOperativo] [varchar] (10) NOT NULL,
    //[capOperativo] [varchar] (5) NOT NULL,
    //[comuneOperativo] [varchar] (50) NOT NULL,
    //[provinciaOperativo] [varchar] (2) NOT NULL,
    //[nazioneOperativo] [varchar] (20) NOT NULL,
    //[telefono] [varchar] (25) NOT NULL,
    //[fax] [varchar] (25) NOT NULL,
    //[email] [varchar] (60) NOT NULL,
    //[pec] [varchar] (60) NOT NULL,
    //[webSite] [varchar] (50) NOT NULL,
    //[iban] [varchar] (30) NOT NULL,
    //[pagamento] [int] NOT NULL,
    //[cliente] [bit]NOT NULL,
    //[fornitore] [bit]NOT NULL,
    //[note] [varchar] (200) NOT NULL,
    //[tipo] [varchar] (50) NOT NULL,  
    //[attivo] bit
    //[notaPagamento]

    [Serializable]
    public class Anag_Clienti_Fornitori
    {
        private int id;
        private string ragioneSociale;
        private string partitaIva;
        private string codiceFiscale;
        private string codiceIdentificativo;
        private string tipoIndirizzoLegale;
        private string indirizzoLegale;
        private string numeroCivicoLegale;
        private string capLegale;
        private string comuneLegale;
        private string provinciaLegale;
        private string nazioneLegale;
        private string tipoIndirizzoOperativo;
        private string indirizzoOperativo;
        private string numeroCivicoOperativo;
        private string capOperativo;
        private string comuneOperativo;
        private string provinciaOperativo;
        private string nazioneOperativo;
        private string telefono;
        private string fax;
        private string email;
        private string pec;
        private string webSite;
        private string iban;
        private int pagamento;
        private bool cliente;
        private bool fornitore;
        private string note;
        private string tipo;
        private bool attivo;
        private string notaPagamento;
        private List<Anag_Referente_Clienti_Fornitori> referenti;

        public int Id { get => id; set => id = value; }
        public string RagioneSociale { get => ragioneSociale; set => ragioneSociale = value; }
        public string PartitaIva { get => partitaIva; set => partitaIva = value; }
        public string CodiceFiscale { get => codiceFiscale; set => codiceFiscale = value; }
        public string CodiceIdentificativo { get => codiceIdentificativo; set => codiceIdentificativo = value; }
        public string TipoIndirizzoLegale { get => tipoIndirizzoLegale; set => tipoIndirizzoLegale = value; }
        public string IndirizzoLegale { get => indirizzoLegale; set => indirizzoLegale = value; }
        public string NumeroCivicoLegale { get => numeroCivicoLegale; set => numeroCivicoLegale = value; }
        public string CapLegale { get => capLegale; set => capLegale = value; }
        public string ComuneLegale { get => comuneLegale; set => comuneLegale = value; }
        public string ProvinciaLegale { get => provinciaLegale; set => provinciaLegale = value; }
        public string NazioneLegale { get => nazioneLegale; set => nazioneLegale = value; }
        public string TipoIndirizzoOperativo { get => tipoIndirizzoOperativo; set => tipoIndirizzoOperativo = value; }
        public string IndirizzoOperativo { get => indirizzoOperativo; set => indirizzoOperativo = value; }
        public string NumeroCivicoOperativo { get => numeroCivicoOperativo; set => numeroCivicoOperativo = value; }
        public string CapOperativo { get => capOperativo; set => capOperativo = value; }
        public string ComuneOperativo { get => comuneOperativo; set => comuneOperativo = value; }
        public string ProvinciaOperativo { get => provinciaOperativo; set => provinciaOperativo = value; }
        public string NazioneOperativo { get => nazioneOperativo; set => nazioneOperativo = value; }
        public string Telefono { get => telefono; set => telefono = value; }
        public string Fax { get => fax; set => fax = value; }
        public string Email { get => email; set => email = value; }
        public string Pec { get => pec; set => pec = value; }
        public string WebSite { get => webSite; set => webSite = value; }
        public string Iban { get => iban; set => iban = value; }
        public int Pagamento { get => pagamento; set => pagamento = value; }
        public bool Cliente { get => cliente; set => cliente = value; }
        public bool Fornitore { get => fornitore; set => fornitore = value; }
        public string Note { get => note; set => note = value; }
        public string Tipo { get => tipo; set => tipo = value; }
        public bool Attivo { get => attivo; set => attivo = value; }
        public List<Anag_Referente_Clienti_Fornitori> Referenti { get => referenti; set => referenti = value; }
        public string NotaPagamento { get => notaPagamento; set => notaPagamento = value; }

        public FiguraProfessionale CreaFiguraProfessionale(string descrizioneArticoloAssociato)
        {
            FiguraProfessionale figProf = new FiguraProfessionale();

            figProf.Id = 0;
            figProf.IdFornitori = this.Id;
            figProf.Nome = "";
            figProf.Cognome = this.RagioneSociale;
            figProf.Citta = this.ComuneOperativo;
            figProf.Qualifiche = null;
            figProf.Telefono = this.Telefono;

            figProf.DescrizioneArticoloAssociato = descrizioneArticoloAssociato;

            figProf.Tipo = 1;

            return figProf;
        }
    }
}