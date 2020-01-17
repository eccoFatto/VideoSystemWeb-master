using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{
    public class DatiScadenzario
    {
        private int id;
        private int idDatiProtocollo;
        private string banca;
        private DateTime? dataScadenza;
        private decimal importoDare;
        private decimal importoDareIva;
        private decimal importoVersato;
        private DateTime? dataVersamento;
        private decimal importoAvere;
        private decimal importoAvereIva;
        private decimal importoRiscosso;
        private DateTime? dataRiscossione;
        private string note;
        private decimal iva;

        private string ragioneSocialeClienteFornitore;
        private string protocolloRiferimento;
        private DateTime? dataProtocollo;
        private decimal cassa;

        public int Id { get => id; set => id = value; }
        public int IdDatiProtocollo { get => idDatiProtocollo; set => idDatiProtocollo = value; }
        public string Banca { get => banca; set => banca = value; }
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

        public decimal Iva { get => iva; set => iva = value; }
    }
}