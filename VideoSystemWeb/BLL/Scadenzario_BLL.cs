using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;

namespace VideoSystemWeb.BLL
{
    public class Scadenzario_BLL
    {
        //singleton
        private static volatile Scadenzario_BLL instance;
        private static object objForLock = new Object();
        private Scadenzario_BLL() { }
        public static Scadenzario_BLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Scadenzario_BLL();
                    }
                }
                return instance;
            }
        }

        #region TIPI
        private const string TIPO_CLIENTE = "CLIENTE";
        private const string TIPO_FORNITORE = "FORNITORE";
        #endregion

        public void AggiornaDatiScadenzario(DatiScadenzario scadenza, string tipoScadenza, string importo, string iva, string dataScadenza, string dataVersamentoRiscossione, string banca, ref Esito esito)
        {
            if (tipoScadenza.ToUpper() == TIPO_CLIENTE)
            {
                scadenza.DataRiscossione = DateTime.Parse(dataVersamentoRiscossione); //DateTime.Now;
                scadenza.ImportoRiscosso = string.IsNullOrEmpty(importo) ? 0 : decimal.Parse(importo);
            }
            else
            {
                scadenza.DataVersamento = DateTime.Parse(dataVersamentoRiscossione); //DateTime.Now;
                scadenza.ImportoVersato = string.IsNullOrEmpty(importo) ? 0 : decimal.Parse(importo); 
            }
            scadenza.Iva = string.IsNullOrEmpty(iva) ? 0 : decimal.Parse(iva);
            scadenza.DataScadenza = DateTime.Parse(dataScadenza);
            scadenza.Banca = banca;

            Scadenzario_DAL.Instance.AggiornaDatiScadenzario(scadenza, ref esito);
        }

        public void CreaDatiScadenzario(DatiScadenzario scadenza, string _acconto, string _iva, string _numeroRate, string tipoScadenza, DateTime? dataPartenzaPagamento, int cadenzaGiorni, ref Esito esito)
        {
            decimal accontoIva = string.IsNullOrEmpty(_acconto) ? 0 : decimal.Parse(_acconto);
            decimal ivaDecimal = string.IsNullOrEmpty(_iva) ? 0 : decimal.Parse(_iva);
            decimal acconto = string.IsNullOrEmpty(_acconto) ? 0 : decimal.Parse(_acconto) / (1 + (ivaDecimal / 100));

            int numeroRate = string.IsNullOrEmpty(_numeroRate) ? 1 : int.Parse(_numeroRate);

            if (acconto > 0)
            {
                DatiScadenzario scadenzaAcconto = new DatiScadenzario
                {
                    IdDatiProtocollo = scadenza.IdDatiProtocollo,
                    Banca = scadenza.Banca,
                    DataScadenza = DateTime.Now,
                    ImportoDare = 0,
                    ImportoDareIva = 0,
                    ImportoVersato = 0,
                    DataVersamento = null,
                    ImportoAvere = 0,
                    ImportoAvereIva = 0,
                    ImportoRiscosso = 0,
                    DataRiscossione = null,
                    Note = scadenza.Note, 
                    Iva = scadenza.Iva
                };

                if (tipoScadenza.ToUpper() == TIPO_CLIENTE)
                {
                    scadenzaAcconto.ImportoRiscosso = acconto;
                    scadenzaAcconto.DataRiscossione = DateTime.Now;
                    scadenzaAcconto.ImportoAvere = acconto;
                    scadenzaAcconto.ImportoAvereIva = accontoIva;
                }
                else
                {
                    scadenzaAcconto.ImportoVersato = acconto;
                    scadenzaAcconto.DataVersamento = DateTime.Now;
                    scadenzaAcconto.ImportoDare = acconto;
                    scadenzaAcconto.ImportoDareIva = accontoIva;
                }

                Scadenzario_DAL.Instance.CreaDatiScadenzario(scadenzaAcconto, ref esito); // SCRITTURA ACCONTO
            }

            if (tipoScadenza.ToUpper() == TIPO_CLIENTE)
            {
                scadenza.ImportoAvere = (scadenza.ImportoAvere - acconto) / decimal.Parse(numeroRate.ToString());
                scadenza.ImportoAvereIva = (scadenza.ImportoAvereIva - accontoIva) / decimal.Parse(numeroRate.ToString());
            }
            else
            {
                scadenza.ImportoDare = (scadenza.ImportoDare - acconto) / decimal.Parse(numeroRate.ToString());
                scadenza.ImportoDareIva = (scadenza.ImportoDareIva - accontoIva) / decimal.Parse(numeroRate.ToString());
            }

            for (int i = 1; i <= numeroRate; i++)
            {
                scadenza.DataScadenza = ((DateTime)dataPartenzaPagamento).AddDays(cadenzaGiorni * i);

                Scadenzario_DAL.Instance.CreaDatiScadenzario(scadenza, ref esito);
            }
        }

        // USATO PER AGGIUNGERE UN PAGAMENTO AD UNA SCADENZA GIA' ESISTENTE
        public void AggiungiPagamento(DatiScadenzario scadenzaDaAggiornare, string tipoScadenza, string importoVersato, string differenzaImportoIva, string iva, string dataScadenza, string dataVersamentoRiscossione, string banca, string dataAggiungiDocumento, ref Esito esito)
        {
            decimal importoVersatoDecimal = string.IsNullOrEmpty(importoVersato) ? 0 : decimal.Parse(importoVersato);
            decimal differenzaImporto = (scadenzaDaAggiornare.ImportoAvere + scadenzaDaAggiornare.ImportoDare) - importoVersatoDecimal;

            
            decimal differenzaImportoIvaDecimal = decimal.Parse(differenzaImportoIva);

            #region SCADENZA DA AGGIORNARE
            scadenzaDaAggiornare.Iva = string.IsNullOrEmpty(iva) ? 0 : decimal.Parse(iva);
            scadenzaDaAggiornare.DataScadenza = DateTime.Parse(dataScadenza);
            scadenzaDaAggiornare.Banca = banca;

            if (tipoScadenza.ToUpper() == TIPO_CLIENTE)
            {
                scadenzaDaAggiornare.DataRiscossione = DateTime.Parse(dataVersamentoRiscossione);// DateTime.Now;
                scadenzaDaAggiornare.ImportoRiscosso = string.IsNullOrEmpty(importoVersato) ? 0 : decimal.Parse(importoVersato);
                scadenzaDaAggiornare.ImportoAvere = scadenzaDaAggiornare.ImportoRiscosso; // La scadenza viene aggiornata con un importo parziale
                scadenzaDaAggiornare.ImportoAvereIva = Math.Round(scadenzaDaAggiornare.ImportoRiscosso * (1 + (scadenzaDaAggiornare.Iva) / 100), 2);
            }
            else
            {
                scadenzaDaAggiornare.DataVersamento = DateTime.Parse(dataVersamentoRiscossione);// DateTime.Now;
                scadenzaDaAggiornare.ImportoVersato = string.IsNullOrEmpty(importoVersato) ? 0 : decimal.Parse(importoVersato);
                scadenzaDaAggiornare.ImportoDare = scadenzaDaAggiornare.ImportoVersato;  // La scadenza viene aggiornata con un importo parziale
                scadenzaDaAggiornare.ImportoDareIva = Math.Round(scadenzaDaAggiornare.ImportoVersato * (1 + (scadenzaDaAggiornare.Iva) / 100), 2);
            }
            #endregion

            #region SCADENZA DA INSERIRE
            DatiScadenzario scadenzaDaInserire = new DatiScadenzario();
            scadenzaDaInserire.Banca = scadenzaDaAggiornare.Banca;
            scadenzaDaInserire.IdDatiProtocollo = scadenzaDaAggiornare.IdDatiProtocollo;
            scadenzaDaInserire.DataScadenza = DateTime.Parse(dataAggiungiDocumento);
            scadenzaDaInserire.DataVersamento = null;
            scadenzaDaInserire.DataRiscossione = null;
            scadenzaDaInserire.Iva = scadenzaDaAggiornare.Iva;

            scadenzaDaInserire.ImportoAvere = 0;
            scadenzaDaInserire.ImportoAvereIva = 0;
            scadenzaDaInserire.ImportoRiscosso = 0;

            scadenzaDaInserire.ImportoDare = 0;
            scadenzaDaInserire.ImportoDareIva = 0;
            scadenzaDaInserire.ImportoVersato = 0;

            if (tipoScadenza.ToUpper() == TIPO_CLIENTE)
            {
                scadenzaDaInserire.ImportoAvere = differenzaImporto;
                scadenzaDaInserire.ImportoAvereIva = differenzaImportoIvaDecimal; Math.Round(scadenzaDaInserire.ImportoAvere * (1 + (scadenzaDaAggiornare.Iva) / 100), 2);
                scadenzaDaInserire.ImportoRiscosso = 0;
            }
            else
            {
                scadenzaDaInserire.ImportoDare = differenzaImporto;
                scadenzaDaInserire.ImportoDareIva = differenzaImportoIvaDecimal;// Math.Round(scadenzaDaInserire.ImportoDare * (1 + (scadenzaDaAggiornare.Iva) / 100), 2);// scadenzaDaInserire.ImportoDare * (1 + (scadenzaDaAggiornare.Iva) / 100);
                scadenzaDaInserire.ImportoVersato = 0;
            }

            scadenzaDaInserire.Note = scadenzaDaAggiornare.Note;
            scadenzaDaInserire.RagioneSocialeClienteFornitore = scadenzaDaAggiornare.RagioneSocialeClienteFornitore;
            scadenzaDaInserire.ProtocolloRiferimento = scadenzaDaAggiornare.ProtocolloRiferimento;
            scadenzaDaInserire.DataProtocollo = scadenzaDaAggiornare.DataProtocollo;
            scadenzaDaInserire.Cassa = scadenzaDaAggiornare.Cassa;
            #endregion

            esito = Scadenzario_DAL.Instance.AggiungiPagamento(scadenzaDaAggiornare, scadenzaDaInserire);
        }

        public DatiScadenzario GetDatiScadenzarioById(ref Esito esito, int id)
        { 
            return Scadenzario_DAL.Instance.GetDatiScadenzarioById(ref esito, id);
        }

        public List<DatiScadenzario> GetAllDatiScadenzario(string tipoAnagrafica,
                                                           string ragioneSociale,
                                                           string numeroFattura,
                                                           string fatturaPagata,
                                                           string dataFatturaDa,
                                                           string dataFatturaA,
                                                           string dataScadenzaDa,
                                                           string dataScadenzaA, 
                                                           ref Esito esito)
        {
            return Scadenzario_DAL.Instance.GetAllDatiScadenzario(tipoAnagrafica,
                                                                  ragioneSociale,
                                                                  numeroFattura,
                                                                  fatturaPagata,
                                                                  dataFatturaDa,
                                                                  dataFatturaA,
                                                                  dataScadenzaDa,
                                                                  dataScadenzaA,
                                                                  ref esito);
        }

        public void DeleteDatiScadenzario(int idSDatiScadenzario, ref Esito esito)
        {
            List<DatiScadenzario> listaDatiScadenzarioDaCancellare = Scadenzario_DAL.Instance.GetDatiTotaliFatturaByIdDatiScadenzario(idSDatiScadenzario, ref esito);

            Scadenzario_DAL.Instance.DeleteDatiScadenzarioById(idSDatiScadenzario, ref esito);

            if (esito.Codice == Esito.ESITO_OK && listaDatiScadenzarioDaCancellare.Count > 0)
            { 
                int idProtocollo = listaDatiScadenzarioDaCancellare.ElementAt(0).IdDatiProtocollo;
                string tipoScadenza = "Cliente";
                if (listaDatiScadenzarioDaCancellare.ElementAt(0).ImportoDare > 0) tipoScadenza = "Fornitore";

                HttpContext.Current.Response.Redirect("Scadenzario?TIPO=" + tipoScadenza + "&ID_PROTOCOLLO=" + idProtocollo);
            } 

        }

        public List<DatiScadenzario> GetDatiTotaliFatturaByIdDatiScadenzario(int idDatiScadenzario, ref Esito esito)
        {
            return Scadenzario_DAL.Instance.GetDatiTotaliFatturaByIdDatiScadenzario(idDatiScadenzario, ref esito);
        }

        public List<Protocolli> getFattureNonInScadenzario(string tipo, ref Esito esito)
        {
            return Scadenzario_DAL.Instance.GetFattureNonInScadenzario(tipo, ref esito);
        }
        
        public List<DatiScadenzario> GetDatiScadenzarioByIdDatiProtocollo(int idDatiProtocollo, ref Esito esito)
        {
            return Scadenzario_DAL.Instance.GetDatiScadenzarioByIdDatiProtocollo(idDatiProtocollo, ref esito);
        }

        public List<Anag_Clienti_Fornitori> getClientiFornitoriInScadenzario(ref Esito esito)
        {
            return Scadenzario_DAL.Instance.GetClientiFornitoriInScadenzario(ref esito);
        }
    }
}