using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.BLL;

namespace VideoSystemWeb.Entity
{
    public class DatiScadenzario
    {
        private int id;
        private int? idPadre;
        private int idDatiProtocollo;
        //private string banca;
        private DateTime? dataScadenza;
        private decimal importoDare;
        private decimal importoDareIva;
        private decimal importoVersato;
        private decimal importoVersatoIva;
        private DateTime? dataVersamento;
        private decimal importoAvere;
        private decimal importoAvereIva;
        private decimal importoRiscosso;
        private decimal importoRiscossoIva;
        private DateTime? dataRiscossione;
        private string note;
        private decimal iva;

        private string ragioneSocialeClienteFornitore;
        private string protocolloRiferimento;
        private DateTime? dataProtocollo;
        private DateTime? dataFattura;
        private decimal cassa;
        private int idTipoBanca;

        public int Id { get => id; set => id = value; }
        public int IdDatiProtocollo { get => idDatiProtocollo; set => idDatiProtocollo = value; }
        public string Banca 
        {
            get
            {
                return (SessionManager.ListaTipiBanca.FirstOrDefault(x => x.id == IdTipoBanca)).nome;
            }
        }
        public DateTime? DataScadenza { get => dataScadenza; set => dataScadenza = value; }
        public decimal ImportoDare { get => importoDare; set => importoDare = value; }
        public decimal ImportoDareIva { get => importoDareIva; set => importoDareIva = value; }
        public decimal ImportoVersato { get => importoVersato; set => importoVersato = value; }
        public DateTime? DataVersamento { get => dataVersamento; set => dataVersamento = value; }
        public decimal ImportoAvere { get => importoAvere; set => importoAvere = value; }
        public decimal ImportoAvereIva { get => importoAvereIva; set => importoAvereIva = value; }
        public decimal ImportoRiscosso { get => importoRiscosso; set => importoRiscosso = value; }
        public DateTime? DataRiscossione { get => dataRiscossione; set => dataRiscossione = value; }
        public string Note { get => note; set => note = value; }

        public string RagioneSocialeClienteFornitore { get => ragioneSocialeClienteFornitore; set => ragioneSocialeClienteFornitore = value; }
        public string ProtocolloRiferimento { get => protocolloRiferimento; set => protocolloRiferimento = value; }
        public DateTime? DataProtocollo { get => dataProtocollo; set => dataProtocollo = value; }
        public string IsImportoEstinto
        {
            get 
            {
                if ((ImportoDare - importoVersato) == 0 && (ImportoAvere - importoRiscosso) == 0)
                {
                    return "Pagata";
                }
                else
                {
                    return "Non pagata";
                }
            }
        }
        public decimal Cassa { get => cassa; set => cassa = value; }
        public DateTime? DataPagamento
        {
            get 
            {
                if (DataVersamento != null) return DataVersamento;
                else return DataRiscossione;
            }
        }

        //public decimal ImportoVersatoIva
        //{
        //    get
        //    {
        //        return ImportoVersato + (ImportoVersato / 100 * Iva);
        //    }
        //}

        //public decimal ImportoRiscossoIva
        //{
        //    get
        //    {
        //        return ImportoRiscosso + (ImportoRiscosso / 100 * Iva);
        //    }
        //}

        public decimal Iva { get => iva; set => iva = value; }
        public int IdTipoBanca { get => idTipoBanca; set => idTipoBanca = value; }
        public int? IdPadre { get => idPadre; set => idPadre = value; }
        public decimal ImportoVersatoIva { get => importoVersatoIva; set => importoVersatoIva = value; }
        public decimal ImportoRiscossoIva { get => importoRiscossoIva; set => importoRiscossoIva = value; }
        public DateTime? DataFattura { get => dataFattura; set => dataFattura = value; }
    }
}