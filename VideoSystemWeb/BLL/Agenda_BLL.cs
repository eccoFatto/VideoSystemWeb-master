using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using VideoSystemWeb.Agenda.userControl;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;

namespace VideoSystemWeb.BLL
{
    public class Agenda_BLL
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //singleton
        private static volatile Agenda_BLL instance;
        private static object objForLock = new Object();

        private Agenda_BLL() { }

        public static Agenda_BLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Agenda_BLL();
                    }
                }
                return instance;
            }
        }

        public List<ColonneAgenda> CaricaColonne(ref Esito esito)
        {
            List<ColonneAgenda> listaColonne = new List<ColonneAgenda>();

            //if (Convert.ToBoolean(ConfigurationManager.AppSettings["USA_DB"]))
            //{
                listaColonne = Agenda_DAL.Instance.CaricaColonne(ref esito);
            //}
            //else
            //{
            //    listaColonne = Tipologie.caricaTipologica(EnumTipologiche.TIPO_COLONNE_AGENDA);
            //}

            return listaColonne;
        }

        public List<DatiAgenda> CaricaDatiAgenda(ref Esito esito)
        {
            List<DatiAgenda> listaDatiAgenda = new List<DatiAgenda>();
            listaDatiAgenda = Agenda_DAL.Instance.CaricaDatiAgenda(ref esito);

            //if (Convert.ToBoolean(ConfigurationManager.AppSettings["USA_DB"]))
            //{
            //    listaDatiAgenda = Agenda_DAL.Instance.CaricaDatiAgenda(ref esito);
            //}
            //else
            //{
            //    listaDatiAgenda = Tipologie.getListaDatiAgenda();
            //}

            return listaDatiAgenda;
        }

        public List<DatiAgenda> CaricaDatiAgenda(DateTime dataInizio, ref Esito esito)
        {
            List<DatiAgenda> listaDatiAgenda = new List<DatiAgenda>();

            listaDatiAgenda = Agenda_DAL.Instance.CaricaDatiAgenda(dataInizio, dataInizio.AddDays(31), ref esito);

            return listaDatiAgenda;
        }

        public List<string> CaricaElencoProduzioni(ref Esito esito)
        {
            return Agenda_DAL.Instance.CaricaElencoProduzioni(null, ref esito);
        }

        public List<string> CaricaElencoLavorazioni(ref Esito esito)
        {
            return Agenda_DAL.Instance.CaricaElencoLavorazioni(null, null, ref esito).Select(x => x.lavorazione).ToList();
        }

        public DatiAgenda GetDatiAgendaById(List<DatiAgenda> listaDatiAgenda, int id)
        {
            return listaDatiAgenda.Where(x => x.id == id).FirstOrDefault();
        }

        public DatiAgenda GetDatiAgendaByDataRisorsa(List<DatiAgenda> listaDatiAgenda, DateTime data, int id_risorsa)
        {
            return listaDatiAgenda.Where(x => x.data_inizio_impegno.Date <= data.Date && x.data_fine_impegno.Date >= data.Date && x.id_colonne_agenda == id_risorsa).FirstOrDefault();
        }

        public Esito CreaEvento(DatiAgenda evento, List<string> listaIdTender, NoteOfferta noteOfferta)
        {
            Esito esito = new Esito();
            esito = Agenda_DAL.Instance.CreaEvento(evento, listaIdTender, noteOfferta);
            
            return esito;
        }

        public Esito AggiornaEvento(DatiAgenda evento, List<string> listaIdTender)
        {
            Esito esito = new Esito();
            esito = Agenda_DAL.Instance.AggiornaEvento(evento, listaIdTender);

            return esito;
        }

        public Esito EliminaEvento(int idEvento)
        {
            Esito esito = Dati_Articoli_DAL.Instance.EliminaDatiArticoloByIdDatiAgenda(idEvento);
            if (esito.Codice == Esito.ESITO_OK)
            {
                esito = Dati_Tender_DAL.Instance.EliminaDatiTenderByIdDatiAgenda(idEvento);
            }
            if (esito.Codice == Esito.ESITO_OK)
            {
                esito = Agenda_DAL.Instance.EliminaEvento(idEvento);
            }
            return esito;
        }

        public Esito EliminaLavorazione(DatiAgenda evento)
        {
            Esito esito = Agenda_DAL.Instance.EliminaLavorazione(evento);
            return esito;
        }

        public List<int> GetTenderImpiegatiInPeriodo(DatiAgenda eventoDaControllare, ref Esito esito)
        {
            DateTime dataInizio = eventoDaControllare.data_inizio_impegno;
            DateTime dataFine = eventoDaControllare.data_fine_impegno;
            int idEventoDaControllare = eventoDaControllare.id;

            return Agenda_DAL.Instance.GetTenderImpiegatiInPeriodo(dataInizio, dataFine, idEventoDaControllare,  ref esito);
        }

        public DataTable CercaLavorazione(string numeroProtocollo, string codiceLavoro, string ragioneSociale, string produzione, string lavorazione, string descrizione, string tipoProtocollo, string protocolloRiferimento, string destinatario, string dataProtocolloDa, string dataProtocolloA, string dataLavorazioneDa, string dataLavorazioneA, ref Esito esito)
        {
            string queryRicerca = "select distinct agenda.id, agenda.data_inizio_impegno, agenda.data_inizio_lavorazione, agenda.id_colonne_agenda, stato.descrizione, agenda.codice_lavoro as [Cod. Lav.], agenda.produzione, agenda.lavorazione, cliente.ragioneSociale as Cliente " +
                                    " from  tab_dati_agenda agenda " +
                                    " left join anag_clienti_fornitori cliente on agenda.id_cliente=cliente.id " +
                                    " left join dati_protocollo prot   on prot.codice_lavoro = agenda.codice_lavoro " +
                                    " left join tipo_stato stato on stato.id=agenda.id_stato " +
                                    " left join tipo_protocollo tipo on prot.id_tipo_protocollo = tipo.id " +
                                    " where  agenda.id_stato <> 5 " +
                                    " and agenda.id_stato <> 1 " +
                                    //" and prot.attivo = 1 " +
                                    " and (prot.numero_protocollo like '%@numeroProtocollo%' or prot.numero_protocollo is null) " +
                                    " and (agenda.codice_lavoro like '%@codiceLavoro%' or agenda.codice_lavoro is null) " +
                                    " and (cliente.ragioneSociale like '%@cliente%' or cliente.ragioneSociale is null) " +    
                                    " and (agenda.produzione like '%@produzione%' or agenda.produzione is null) " +
                                    " and (agenda.lavorazione like '%@lavorazione%' or agenda.lavorazione is null) " +        
                                    " and (prot.descrizione like '%@descrizione%' or prot.descrizione is null) " +
                                    " and (prot.destinatario like '%@destinatario%' or prot.destinatario is null) " +
                                    " and (prot.protocollo_riferimento like '%@protocolloRiferimento%' or prot.protocollo_riferimento is null) " +
                                    " @dataProtocollo " +
                                    " @dataLavorazione " +
                                    " and isnull(tipo.nome,'') like '%@tipoProtocollo%'  " +
                                    " order by agenda.id";

            queryRicerca = queryRicerca.Replace("@numeroProtocollo", numeroProtocollo);
            queryRicerca = queryRicerca.Replace("@codiceLavoro", codiceLavoro);
            queryRicerca = queryRicerca.Replace("@cliente", ragioneSociale);
            queryRicerca = queryRicerca.Replace("@produzione", produzione);
            queryRicerca = queryRicerca.Replace("@lavorazione", lavorazione);
            queryRicerca = queryRicerca.Replace("@descrizione", descrizione);
            queryRicerca = queryRicerca.Replace("@tipoProtocollo", tipoProtocollo);
            queryRicerca = queryRicerca.Replace("@protocolloRiferimento", protocolloRiferimento);
            queryRicerca = queryRicerca.Replace("@destinatario", destinatario);

            string queryProtocolloDataProt = "";
            if (!string.IsNullOrEmpty(dataProtocolloDa)) 
            {
                DateTime dataDa = Convert.ToDateTime(dataProtocolloDa);
                DateTime dataA = DateTime.Now;
                queryProtocolloDataProt = " and data_protocollo between '@dataDa' and '@DataA' ";
                if (!string.IsNullOrEmpty(dataProtocolloA))
                {
                    dataA = Convert.ToDateTime(dataProtocolloA);
                }
                queryProtocolloDataProt = queryProtocolloDataProt.Replace("@dataDa", dataDa.ToString("yyyy-MM-ddT00:00:00.000"));
                queryProtocolloDataProt = queryProtocolloDataProt.Replace("@DataA", dataA.ToString("yyyy-MM-ddT23:59:59.999"));
            }
            queryRicerca = queryRicerca.Replace("@dataProtocollo", queryProtocolloDataProt);

            string queryProtocolloDataLav = "";
            if (!string.IsNullOrEmpty(dataLavorazioneDa))
            {
                DateTime dataDa = Convert.ToDateTime(dataLavorazioneDa);
                DateTime dataA = DateTime.Now;
                queryProtocolloDataLav = " and agenda.data_inizio_lavorazione between '@dataDa' and '@DataA' ";
                if (!string.IsNullOrEmpty(dataLavorazioneA))
                {
                    dataA = Convert.ToDateTime(dataLavorazioneA);
                }
                queryProtocolloDataLav = queryProtocolloDataLav.Replace("@dataDa", dataDa.ToString("yyyy-MM-ddT00:00:00.000"));
                // LA BETWEEN PRENDE PURE IL GIORNO DOPO SE METTI 23:59 e le date non contengono l'orario
                queryProtocolloDataLav = queryProtocolloDataLav.Replace("@DataA", dataA.ToString("yyyy-MM-ddT00:00:00.000"));
            }
            queryRicerca = queryRicerca.Replace("@dataLavorazione", queryProtocolloDataLav);

            DataTable dtProtocolli = Base_DAL.GetDatiBySql(queryRicerca, ref esito);

            return dtProtocolli;
        }

    }
}