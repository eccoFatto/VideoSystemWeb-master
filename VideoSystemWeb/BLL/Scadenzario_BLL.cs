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
        #region SINGLETON
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
        #endregion  

        #region TIPI
        private const string TIPO_CLIENTE = "CLIENTE";
        private const string TIPO_FORNITORE = "FORNITORE";
        #endregion

        public void AggiornaDatiScadenzario(DatiScadenzario scadenza, string tipoScadenza, string importo, string importoIva, string iva, string dataScadenza, string dataVersamentoRiscossione, int idTipoBanca, ref Esito esito)
        {
            if (tipoScadenza.ToUpper() == TIPO_CLIENTE)
            {
                scadenza.DataRiscossione = string.IsNullOrEmpty(dataVersamentoRiscossione) ? null : (DateTime?)DateTime.Parse(dataVersamentoRiscossione);
                scadenza.ImportoRiscosso = string.IsNullOrEmpty(importo) ? 0 : decimal.Parse(importo);
                scadenza.ImportoRiscossoIva = string.IsNullOrEmpty(importoIva) ? 0 : decimal.Parse(importoIva);
            }
            else
            {
                scadenza.DataVersamento = string.IsNullOrEmpty(dataVersamentoRiscossione) ? null : (DateTime?)DateTime.Parse(dataVersamentoRiscossione);
                scadenza.ImportoVersato = string.IsNullOrEmpty(importo) ? 0 : decimal.Parse(importo);
                scadenza.ImportoVersatoIva = string.IsNullOrEmpty(importoIva) ? 0 : decimal.Parse(importoIva);
            }
            scadenza.Iva = string.IsNullOrEmpty(iva) ? 0 : decimal.Parse(iva);
            scadenza.DataScadenza = DateTime.Parse(dataScadenza);
            scadenza.IdTipoBanca = idTipoBanca;

            Scadenzario_DAL.Instance.AggiornaDatiScadenzario(scadenza, ref esito);
        }

        
        public void CancellaFigli_AggiornaDatiScadenzario(List<DatiScadenzario> listaFigliScadenza, DatiScadenzario scadenza, string tipoScadenza, string importo, string importoIva, string iva, string dataScadenza, string dataVersamentoRiscossione, int idTipoBanca, ref Esito esito)
        {
            if (tipoScadenza.ToUpper() == TIPO_CLIENTE)
            {
                scadenza.DataRiscossione = string.IsNullOrEmpty(dataVersamentoRiscossione) ? null : (DateTime?)DateTime.Parse(dataVersamentoRiscossione);
                scadenza.ImportoRiscosso = string.IsNullOrEmpty(importo) ? 0 : decimal.Parse(importo);
                scadenza.ImportoRiscossoIva = string.IsNullOrEmpty(importoIva) ? 0 : decimal.Parse(importoIva);
            }
            else
            {
                scadenza.DataVersamento = string.IsNullOrEmpty(dataVersamentoRiscossione) ? null : (DateTime?)DateTime.Parse(dataVersamentoRiscossione);
                scadenza.ImportoVersato = string.IsNullOrEmpty(importo) ? 0 : decimal.Parse(importo);
                scadenza.ImportoVersatoIva = string.IsNullOrEmpty(importoIva) ? 0 : decimal.Parse(importoIva);
            }
            scadenza.Iva = string.IsNullOrEmpty(iva) ? 0 : decimal.Parse(iva);
            scadenza.DataScadenza = DateTime.Parse(dataScadenza);
            scadenza.IdTipoBanca = idTipoBanca;

            Scadenzario_DAL.Instance.CancellaFigli_AggiornaDatiScadenzario(listaFigliScadenza, scadenza, ref esito);
        }

        public void CreaDatiScadenzario(DatiScadenzario scadenza, string _acconto, string _accontoIva, string _iva, string _numeroRate, string tipoScadenza, DateTime dataPartenzaPagamento, int cadenzaGiorni, ref Esito esito)
        {
            decimal accontoIva = string.IsNullOrEmpty(_accontoIva) ? 0 : decimal.Parse(_accontoIva);
            decimal ivaDecimal = string.IsNullOrEmpty(_iva) ? 0 : decimal.Parse(_iva);
            decimal acconto = string.IsNullOrEmpty(_acconto) ? 0 : decimal.Parse(_acconto);

            int numeroRate = string.IsNullOrEmpty(_numeroRate) ? 1 : int.Parse(_numeroRate);

            if (acconto > 0)
            {
                DatiScadenzario scadenzaAcconto = new DatiScadenzario
                {
                    IdPadre = null,
                    IdDatiProtocollo = scadenza.IdDatiProtocollo,
                    IdTipoBanca = scadenza.IdTipoBanca,
                    DataScadenza = DateTime.Now,
                    ImportoDare = 0,
                    ImportoDareIva = 0,
                    ImportoVersato = 0,
                    ImportoVersatoIva = 0,
                    DataVersamento = null,
                    ImportoAvere = 0,
                    ImportoAvereIva = 0,
                    ImportoRiscosso = 0,
                    ImportoRiscossoIva = 0,
                    DataRiscossione = null,
                    Note = scadenza.Note, 
                    Iva = scadenza.Iva
                };

                if (tipoScadenza.ToUpper() == TIPO_CLIENTE)
                {
                    scadenzaAcconto.ImportoRiscosso = acconto;
                    scadenzaAcconto.ImportoRiscossoIva = accontoIva;
                    scadenzaAcconto.DataRiscossione = DateTime.Now;
                    scadenzaAcconto.ImportoAvere = acconto;
                    scadenzaAcconto.ImportoAvereIva = accontoIva;
                }
                else
                {
                    scadenzaAcconto.ImportoVersato = acconto;
                    scadenzaAcconto.ImportoVersatoIva = accontoIva;
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
                //scadenza.DataScadenza = ((DateTime)dataPartenzaPagamento).AddDays(cadenzaGiorni * i);
                DateTime scadenzaRata = new DateTime(dataPartenzaPagamento.Year, dataPartenzaPagamento.Month, 1).AddMonths(i + 1);// ((DateTime)dataPartenzaPagamento).AddMonths(i+1);
                scadenza.DataScadenza = scadenzaRata.AddDays(-1);

                Scadenzario_DAL.Instance.CreaDatiScadenzario(scadenza, ref esito);
            }
        }

        //public void CancellaFigli_CreaDatiScadenzario(List<DatiScadenzario> listaFigliScadenza, DatiScadenzario scadenza, string _acconto, string _accontoIva, string _iva, string _numeroRate, string tipoScadenza, DateTime? dataPartenzaPagamento, int cadenzaGiorni, ref Esito esito)
        //{
        //    decimal accontoIva = string.IsNullOrEmpty(_accontoIva) ? 0 : decimal.Parse(_accontoIva);
        //    decimal ivaDecimal = string.IsNullOrEmpty(_iva) ? 0 : decimal.Parse(_iva);
        //    decimal acconto = string.IsNullOrEmpty(_acconto) ? 0 : decimal.Parse(_acconto);

        //    int numeroRate = string.IsNullOrEmpty(_numeroRate) ? 1 : int.Parse(_numeroRate);

        //    if (acconto > 0)
        //    {
        //        DatiScadenzario scadenzaAcconto = new DatiScadenzario
        //        {
        //            IdPadre = null,
        //            IdDatiProtocollo = scadenza.IdDatiProtocollo,
        //            IdTipoBanca = scadenza.IdTipoBanca,
        //            DataScadenza = DateTime.Now,
        //            ImportoDare = 0,
        //            ImportoDareIva = 0,
        //            ImportoVersato = 0,
        //            ImportoVersatoIva = 0,
        //            DataVersamento = null,
        //            ImportoAvere = 0,
        //            ImportoAvereIva = 0,
        //            ImportoRiscosso = 0,
        //            ImportoRiscossoIva = 0,
        //            DataRiscossione = null,
        //            Note = scadenza.Note,
        //            Iva = scadenza.Iva
        //        };

        //        if (tipoScadenza.ToUpper() == TIPO_CLIENTE)
        //        {
        //            scadenzaAcconto.ImportoRiscosso = acconto;
        //            scadenzaAcconto.ImportoRiscossoIva = accontoIva;
        //            scadenzaAcconto.DataRiscossione = DateTime.Now;
        //            scadenzaAcconto.ImportoAvere = acconto;
        //            scadenzaAcconto.ImportoAvereIva = accontoIva;
        //        }
        //        else
        //        {
        //            scadenzaAcconto.ImportoVersato = acconto;
        //            scadenzaAcconto.ImportoVersatoIva = accontoIva;
        //            scadenzaAcconto.DataVersamento = DateTime.Now;
        //            scadenzaAcconto.ImportoDare = acconto;
        //            scadenzaAcconto.ImportoDareIva = accontoIva;
        //        }

        //        Scadenzario_DAL.Instance.CreaDatiScadenzario(scadenzaAcconto, ref esito); // SCRITTURA ACCONTO
        //    }

        //    if (tipoScadenza.ToUpper() == TIPO_CLIENTE)
        //    {
        //        scadenza.ImportoAvere = (scadenza.ImportoAvere - acconto) / decimal.Parse(numeroRate.ToString());
        //        scadenza.ImportoAvereIva = (scadenza.ImportoAvereIva - accontoIva) / decimal.Parse(numeroRate.ToString());
        //    }
        //    else
        //    {
        //        scadenza.ImportoDare = (scadenza.ImportoDare - acconto) / decimal.Parse(numeroRate.ToString());
        //        scadenza.ImportoDareIva = (scadenza.ImportoDareIva - accontoIva) / decimal.Parse(numeroRate.ToString());
        //    }

        //    for (int i = 1; i <= numeroRate; i++)
        //    {
        //        scadenza.DataScadenza = ((DateTime)dataPartenzaPagamento).AddDays(cadenzaGiorni * i);

        //        Scadenzario_DAL.Instance.CancellaFigli_CreaDatiScadenzario(listaFigliScadenza, scadenza, ref esito);
        //    }
        //}

            //aggiungere banca e idPadre
        public void CancellaFigli_CreaDatiScadenzario(List<DatiScadenzario> listaFigliScadenza, DatiScadenzario scadenza,  int idTipoBanca, ref Esito esito)
        {
            
            
            scadenza.IdTipoBanca = idTipoBanca;

            Scadenzario_DAL.Instance.CancellaFigli_CreaDatiScadenzario(listaFigliScadenza, scadenza, ref esito);

        }

        // USATO PER AGGIUNGERE UN PAGAMENTO AD UNA SCADENZA GIA' ESISTENTE
        public void AggiungiPagamento(DatiScadenzario scadenzaDaAggiornare, string tipoScadenza, decimal importoVersato, decimal importoVersatoIva, decimal differenzaImportoIva, decimal iva, DateTime dataScadenza, DateTime dataVersamentoRiscossione, int idTipoBanca, DateTime dataAggiungiPagamento, ref Esito esito)
        {
            decimal differenzaImporto = (scadenzaDaAggiornare.ImportoAvere + scadenzaDaAggiornare.ImportoDare) - importoVersato;


            #region SCADENZA DA AGGIORNARE
            scadenzaDaAggiornare.Iva = iva;
            scadenzaDaAggiornare.DataScadenza = dataScadenza;
            scadenzaDaAggiornare.IdTipoBanca = idTipoBanca;

            if (tipoScadenza.ToUpper() == TIPO_CLIENTE)
            {
                scadenzaDaAggiornare.DataRiscossione = dataVersamentoRiscossione;
                scadenzaDaAggiornare.ImportoRiscosso = importoVersato;
                scadenzaDaAggiornare.ImportoRiscossoIva = importoVersatoIva;
                scadenzaDaAggiornare.ImportoAvere = scadenzaDaAggiornare.ImportoRiscosso; // La scadenza viene aggiornata con un importo parziale
                scadenzaDaAggiornare.ImportoAvereIva = scadenzaDaAggiornare.ImportoRiscossoIva; 
            }
            else
            {
                scadenzaDaAggiornare.DataVersamento = dataVersamentoRiscossione;
                scadenzaDaAggiornare.ImportoVersato = importoVersato;
                scadenzaDaAggiornare.ImportoVersatoIva = importoVersatoIva;
                scadenzaDaAggiornare.ImportoDare = scadenzaDaAggiornare.ImportoVersato;  // La scadenza viene aggiornata con un importo parziale
                scadenzaDaAggiornare.ImportoDareIva = scadenzaDaAggiornare.ImportoVersatoIva; 
            }
            #endregion

            #region SCADENZA DA INSERIRE
            DatiScadenzario scadenzaDaInserire = new DatiScadenzario();
            scadenzaDaInserire.IdPadre = scadenzaDaAggiornare.Id;
            scadenzaDaInserire.IdTipoBanca = scadenzaDaAggiornare.IdTipoBanca;
            scadenzaDaInserire.IdDatiProtocollo = scadenzaDaAggiornare.IdDatiProtocollo;
            scadenzaDaInserire.DataScadenza = dataAggiungiPagamento;
            scadenzaDaInserire.DataVersamento = null;
            scadenzaDaInserire.DataRiscossione = null;
            scadenzaDaInserire.Iva = scadenzaDaAggiornare.Iva;

            scadenzaDaInserire.ImportoAvere = 0;
            scadenzaDaInserire.ImportoAvereIva = 0;
            scadenzaDaInserire.ImportoRiscosso = 0;
            scadenzaDaInserire.ImportoRiscossoIva = 0;

            scadenzaDaInserire.ImportoDare = 0;
            scadenzaDaInserire.ImportoDareIva = 0;
            scadenzaDaInserire.ImportoVersato = 0;
            scadenzaDaInserire.ImportoVersatoIva = 0;

            if (tipoScadenza.ToUpper() == TIPO_CLIENTE)
            {
                scadenzaDaInserire.ImportoAvere = differenzaImporto;
                scadenzaDaInserire.ImportoAvereIva = differenzaImportoIva;
            }
            else
            {
                scadenzaDaInserire.ImportoDare = differenzaImporto;
                scadenzaDaInserire.ImportoDareIva = differenzaImportoIva;
            }

            scadenzaDaInserire.Note = scadenzaDaAggiornare.Note;
            scadenzaDaInserire.RagioneSocialeClienteFornitore = scadenzaDaAggiornare.RagioneSocialeClienteFornitore;
            scadenzaDaInserire.ProtocolloRiferimento = scadenzaDaAggiornare.ProtocolloRiferimento;
            scadenzaDaInserire.DataProtocollo = scadenzaDaAggiornare.DataProtocollo;
            scadenzaDaInserire.Cassa = scadenzaDaAggiornare.Cassa;
            #endregion

            esito = Scadenzario_DAL.Instance.AggiungiPagamento(scadenzaDaAggiornare, scadenzaDaInserire);
        }


        public void CancellaFigli_AggiungiPagamento(List<DatiScadenzario> listaFigliScadenza, DatiScadenzario scadenzaDaAggiornare, string tipoScadenza, decimal importoVersato, decimal importoVersatoIva, decimal differenzaImportoIva, decimal iva, DateTime dataScadenza, DateTime dataVersamentoRiscossione, int idTipoBanca, DateTime dataAggiungiPagamento, ref Esito esito)
        {
            decimal differenzaImporto = differenzaImportoIva / (1 + (iva / 100));


            #region SCADENZA DA AGGIORNARE
            scadenzaDaAggiornare.Iva = iva;
            scadenzaDaAggiornare.DataScadenza = dataScadenza;
            scadenzaDaAggiornare.IdTipoBanca = idTipoBanca;

            if (tipoScadenza.ToUpper() == TIPO_CLIENTE)
            {
                scadenzaDaAggiornare.DataRiscossione = dataVersamentoRiscossione;
                scadenzaDaAggiornare.ImportoRiscosso = importoVersato;
                scadenzaDaAggiornare.ImportoRiscossoIva = importoVersatoIva;
                scadenzaDaAggiornare.ImportoAvere = scadenzaDaAggiornare.ImportoRiscosso; 
                scadenzaDaAggiornare.ImportoAvereIva = scadenzaDaAggiornare.ImportoRiscossoIva; 
            }
            else
            {
                scadenzaDaAggiornare.DataVersamento = dataVersamentoRiscossione;
                scadenzaDaAggiornare.ImportoVersato = importoVersato;
                scadenzaDaAggiornare.ImportoVersatoIva = importoVersatoIva;
                scadenzaDaAggiornare.ImportoDare = scadenzaDaAggiornare.ImportoVersato;
                scadenzaDaAggiornare.ImportoDareIva = scadenzaDaAggiornare.ImportoVersatoIva; 
            }
            #endregion

            #region SCADENZA DA INSERIRE
            DatiScadenzario scadenzaDaInserire = new DatiScadenzario();
            scadenzaDaInserire.IdPadre = scadenzaDaAggiornare.Id;
            scadenzaDaInserire.IdTipoBanca = scadenzaDaAggiornare.IdTipoBanca;
            scadenzaDaInserire.IdDatiProtocollo = scadenzaDaAggiornare.IdDatiProtocollo;
            scadenzaDaInserire.DataScadenza = dataAggiungiPagamento;
            scadenzaDaInserire.DataVersamento = null;
            scadenzaDaInserire.DataRiscossione = null;
            scadenzaDaInserire.Iva = scadenzaDaAggiornare.Iva;

            scadenzaDaInserire.ImportoAvere = 0;
            scadenzaDaInserire.ImportoAvereIva = 0;
            scadenzaDaInserire.ImportoRiscosso = 0;
            scadenzaDaInserire.ImportoRiscossoIva = 0;

            scadenzaDaInserire.ImportoDare = 0;
            scadenzaDaInserire.ImportoDareIva = 0;
            scadenzaDaInserire.ImportoVersato = 0;
            scadenzaDaInserire.ImportoVersatoIva = 0;

            if (tipoScadenza.ToUpper() == TIPO_CLIENTE)
            {
                scadenzaDaInserire.ImportoAvere = differenzaImporto;
                scadenzaDaInserire.ImportoAvereIva = differenzaImportoIva;
            }
            else
            {
                scadenzaDaInserire.ImportoDare = differenzaImporto;
                scadenzaDaInserire.ImportoDareIva = differenzaImportoIva;
            }

            scadenzaDaInserire.Note = scadenzaDaAggiornare.Note;
            scadenzaDaInserire.RagioneSocialeClienteFornitore = scadenzaDaAggiornare.RagioneSocialeClienteFornitore;
            scadenzaDaInserire.ProtocolloRiferimento = scadenzaDaAggiornare.ProtocolloRiferimento;
            scadenzaDaInserire.DataProtocollo = scadenzaDaAggiornare.DataProtocollo;
            scadenzaDaInserire.Cassa = scadenzaDaAggiornare.Cassa;
            #endregion

            Scadenzario_DAL.Instance.CancellaFigli_AggiungiPagamento(listaFigliScadenza, scadenzaDaAggiornare, scadenzaDaInserire, ref esito);
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
                                                           string filtroBanca,
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
                                                                  filtroBanca,
                                                                  ref esito);
        }

        public void DeleteDatiScadenzario(int idDatiScadenzario, ref Esito esito)
        {
            List<DatiScadenzario> listaDatiScadenzarioDaCancellare = Scadenzario_DAL.Instance.GetDatiTotaliFatturaByIdDatiScadenzario(idDatiScadenzario, ref esito);

            Scadenzario_DAL.Instance.DeleteDatiScadenzarioById(idDatiScadenzario, ref esito);

            // decommentare le righe seguenti se dopo la cancellazione si intende aprire la maschera di inserimento per inserire di nuovo la fattura eliminata
            //if (esito.Codice == Esito.ESITO_OK && listaDatiScadenzarioDaCancellare.Count > 0)
            //{ 
            //    int idProtocollo = listaDatiScadenzarioDaCancellare.ElementAt(0).IdDatiProtocollo;
            //    string tipoScadenza = "Cliente";
            //    if (listaDatiScadenzarioDaCancellare.ElementAt(0).ImportoDare > 0) tipoScadenza = "Fornitore";

            //    HttpContext.Current.Response.Redirect("Scadenzario?TIPO=" + tipoScadenza + "&ID_PROTOCOLLO=" + idProtocollo);
            //} 

        }

        public void DeleteFigliScadenza(List<DatiScadenzario> listaFigliScadenza, ref Esito esito)
        {
            Scadenzario_DAL.Instance.DeleteFigliScadenza(listaFigliScadenza, ref esito);
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

        public void SaldoTotale(DatiScadenzario scadenza, string tipoScadenza, string dataVersamentoRiscossione, int idTipoBanca, ref Esito esito)
        {
           
            if (tipoScadenza.ToUpper() == TIPO_CLIENTE)
            {
                decimal imponibile = scadenza.ImportoAvere;
                decimal imponibileIva = scadenza.ImportoAvereIva;

                scadenza.DataRiscossione = DateTime.Parse(dataVersamentoRiscossione);
                scadenza.ImportoRiscosso = imponibile;
                scadenza.ImportoRiscossoIva = imponibileIva;
            }
            else
            {
                decimal imponibile = scadenza.ImportoDare;
                decimal imponibileIva = scadenza.ImportoDareIva;

                scadenza.DataVersamento = DateTime.Parse(dataVersamentoRiscossione);
                scadenza.ImportoVersato = imponibile;
                scadenza.ImportoVersatoIva = imponibileIva;
            }

            scadenza.IdTipoBanca = idTipoBanca;

            Scadenzario_DAL.Instance.AggiornaDatiScadenzario(scadenza, ref esito);
        }

        // metodo ricorsivo che restituisce una lista di tutti gli ID dei figli di una data scadenza (compreso l'id della scadenza di partenza) in una lista di scadenze con lo stesso protocollo

        public void RicercaFigli(DatiScadenzario scadenzaPadre, List<DatiScadenzario> listaScadenzeStessoProtocollo, ref List<DatiScadenzario> listaFigli)
        {
            listaFigli.Add(scadenzaPadre);
            List<DatiScadenzario> listaScadenzeFigli = listaScadenzeStessoProtocollo.Where(x => x.IdPadre == scadenzaPadre.Id).ToList<DatiScadenzario>();
            foreach (DatiScadenzario scadenzaFiglio in listaScadenzeFigli)
            {
                RicercaFigli(scadenzaFiglio, listaScadenzeStessoProtocollo, ref listaFigli);
            }
        }

        // restituisce la lista degli id dei figli
        public void RicercaFigli(int idScadenza, List<DatiScadenzario> listaScadenzeStessoProtocollo, ref List<int> listaIdFigli)
        {
            listaIdFigli.Add(idScadenza);
            List<DatiScadenzario> listaScadenzeFigli = listaScadenzeStessoProtocollo.Where(x => x.IdPadre == idScadenza).ToList<DatiScadenzario>();
            foreach(DatiScadenzario scadenzaFiglio in listaScadenzeFigli)
            {
                RicercaFigli(scadenzaFiglio.Id, listaScadenzeStessoProtocollo, ref listaIdFigli);
            }
        }
    }
}