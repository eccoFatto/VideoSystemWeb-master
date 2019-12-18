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

        public void AggiornaDatiScadenzario(DatiScadenzario scadenza, string tipoScadenza, string importo, string iva, string dataScadenza, string banca, ref Esito esito)
        {
            if (tipoScadenza.ToUpper() == "CLIENTE")
            {
                scadenza.DataRiscossione = DateTime.Now;
                scadenza.ImportoRiscosso = string.IsNullOrEmpty(importo) ? 0 : decimal.Parse(importo);
                
            }
            else
            {
                scadenza.DataVersamento = DateTime.Now;
                scadenza.ImportoVersato = string.IsNullOrEmpty(importo) ? 0 : decimal.Parse(importo); ;
            }
            scadenza.Iva = string.IsNullOrEmpty(iva) ? 0 : decimal.Parse(iva);
            scadenza.DataScadenza = DateTime.Parse(dataScadenza);
            scadenza.Banca = banca;

            Scadenzario_DAL.Instance.AggiornaDatiScadenzario(scadenza, ref esito);
        }

        public void CreaDatiScadenzario(DatiScadenzario scadenza, string _acconto, string _iva, string _numeroRate, string tipoScadenza, DateTime? dataPartenzaPagamento, int cadenzaGiorni, ref Esito esito)
        {
            decimal acconto = string.IsNullOrEmpty(_acconto) ? 0 : decimal.Parse(_acconto);
            decimal accontoIva = acconto * (1 + (decimal.Parse(_iva) / 100));
            int numeroRate = string.IsNullOrEmpty(_numeroRate) ? 1 : int.Parse(_numeroRate);


            if (acconto > 0)
            {
                DatiScadenzario scadenzaAcconto = new DatiScadenzario()
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

                if (tipoScadenza.ToUpper() == "CLIENTE")
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

            if (tipoScadenza.ToUpper() == "CLIENTE")
            {
                scadenza.ImportoAvere = (scadenza.ImportoAvere - acconto) / numeroRate;
                scadenza.ImportoAvereIva = (scadenza.ImportoAvereIva - accontoIva) / numeroRate;
            }
            else
            {
                scadenza.ImportoDare = (scadenza.ImportoDare - acconto) / numeroRate;
                scadenza.ImportoDareIva = (scadenza.ImportoDareIva - accontoIva) / numeroRate;
            }

            for (int i = 1; i <= numeroRate; i++)
            {
                scadenza.DataScadenza = ((DateTime)dataPartenzaPagamento).AddDays(cadenzaGiorni * i);

                Scadenzario_DAL.Instance.CreaDatiScadenzario(scadenza, ref esito);
            }
        }

        public DatiScadenzario GetDatiScadenzarioById(ref Esito esito, int id)
        { 
            return Scadenzario_DAL.Instance.GetDatiScadenzarioById(ref esito, id);
        }

        public List<DatiScadenzario> GetAllDatiScadenzario(string tipoAnagrafica,
                                                           string codiceAnagrafica,
                                                           string numeroFattura,
                                                           string fatturaPagata,
                                                           string dataFatturaDa,
                                                           string dataFatturaA,
                                                           string dataScadenzaDa,
                                                           string dataScadenzaA, 
                                                           ref Esito esito)
        {
            return Scadenzario_DAL.Instance.GetAllDatiScadenzario(tipoAnagrafica,
                                                                  codiceAnagrafica,
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
            return Scadenzario_DAL.Instance.getFattureNonInScadenzario(tipo, ref esito);
        }
        
        public List<DatiScadenzario> GetDatiScadenzarioByIdDatiProtocollo(int idDatiProtocollo, ref Esito esito)
        {
            return Scadenzario_DAL.Instance.GetDatiScadenzarioByIdDatiProtocollo(idDatiProtocollo, ref esito);
        }
    }
}