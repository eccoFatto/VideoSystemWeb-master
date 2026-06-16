using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;

namespace VideoSystemWeb.BLL
{
    public class Report_BLL
    {
        //singleton
        private static volatile Report_BLL instance;
        private static object objForLock = new Object();
        private Report_BLL() { }
        public static Report_BLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Report_BLL();
                    }
                }
                return instance;
            }
        }

        public List<DatiReport> GetListaDatiReportConsulenteLavoro(string cliente, string produzione, string lavorazione, string codiceLavorazione, string genere, string gruppo, string sottogruppo, DateTime dataInizio, DateTime dataFine, string nominativo, string isAssunto, ref Esito esito)
        {
            List<DatiReport> listaReportConsulenteLavoro = new List<DatiReport>();

            DataTable dtReportConsulenteLavoro = Report_DAL.Instance.GetDatiReportConsulenteLavoro(cliente, produzione, lavorazione, codiceLavorazione, genere, gruppo, sottogruppo, dataInizio, dataFine, nominativo,  isAssunto, ref esito);
            foreach(DataRow riga in dtReportConsulenteLavoro.Rows)
            {
                DatiReport datiReport = new DatiReport();

                if (listaReportConsulenteLavoro.Count == 0 || listaReportConsulenteLavoro.FirstOrDefault(x => x.IdCollaboratore == riga.Field<int>("ID"))==null)
                {
                    datiReport.IdCollaboratore = riga.Field<int>("ID");
                    datiReport.NomeCollaboratore = riga.Field<string>("Nome");
                    datiReport.QualificaCollaboratore = riga.Field<string>("Descrizione");
                    datiReport.IndirizzoCollaboratore = riga.Field<string>("Indirizzo");
                    datiReport.CittaCollaboratore = riga.Field<string>("Citta");
                    datiReport.TelefonoCollaboratore = riga.Field<string>("Telefono");
                    //datiReport.CellulareCollaboratore = riga.Field<string>("");
                    //datiReport.IscrizioneCollaboratore = riga.Field<string>("");
                    datiReport.CodFiscaleCollaboratore = riga.Field<string>("CodiceFiscale");
                    datiReport.ListaDatiFiscali = new List<DatiFiscaliLavorazione>();

                    listaReportConsulenteLavoro.Add(datiReport);
                }

                datiReport = listaReportConsulenteLavoro.FirstOrDefault(x => x.IdCollaboratore == riga.Field<int>("ID"));

                DatiFiscaliLavorazione datiFiscaliLavorazione = new DatiFiscaliLavorazione();

                datiFiscaliLavorazione.DataLavorazione = riga.Field<DateTime>("Data");
                datiFiscaliLavorazione.Lavorazione = riga.Field<string>("Lavorazione");
                datiFiscaliLavorazione.Produzione = riga.Field<string>("Produzione");
                datiFiscaliLavorazione.Cliente = riga.Field<string>("Cliente");
                datiFiscaliLavorazione.Descrizione = riga.Field<string>("Descrizione");
                datiFiscaliLavorazione.Assunzione = riga.Field<decimal>("Assunzione");
                datiFiscaliLavorazione.Mista = (decimal)riga.Field<int>("Mista");
                datiFiscaliLavorazione.Diaria = riga.Field<int>("Diaria");
                datiFiscaliLavorazione.Albergo = riga.Field<int>("Albergo");
                datiFiscaliLavorazione.RimborsoKm = riga.Field<decimal>("RimborsoKm");
                datiFiscaliLavorazione.TipoPagamento = riga.Field<int>("TipoPagamento");
                datiFiscaliLavorazione.DescrizionePagamento = riga.Field<string>("DescrizioneTipoPagamento");


                datiReport.ListaDatiFiscali.Add(datiFiscaliLavorazione);
            }                       

            return listaReportConsulenteLavoro;
        }

        public List<DatiReportRaw> GetListaDatiReportRawConsulenteLavoro(string cliente, string produzione, string lavorazione, string codiceLavorazione, string genere, string gruppo, string sottogruppo, DateTime dataInizio, DateTime dataFine, string nominativo, string isAssunto, ref Esito esito)
        {
            List<DatiReportRaw> listaReportConsulenteLavoro = new List<DatiReportRaw>();

            DataTable dtReportConsulenteLavoro = Report_DAL.Instance.GetDatiReportConsulenteLavoro(cliente, produzione, lavorazione, codiceLavorazione, genere, gruppo, sottogruppo, dataInizio, dataFine, nominativo, isAssunto, ref esito);
            foreach (DataRow riga in dtReportConsulenteLavoro.Rows)
            {
                DatiReportRaw datiReport = new DatiReportRaw();

                datiReport.IdCollaboratore = riga.Field<int>("ID");
                datiReport.NomeCollaboratore = riga.Field<string>("Nome");
                datiReport.QualificaCollaboratore = riga.Field<string>("Descrizione");
                datiReport.IndirizzoCollaboratore = riga.Field<string>("Indirizzo");
                datiReport.CittaCollaboratore = riga.Field<string>("Citta");
                datiReport.TelefonoCollaboratore = riga.Field<string>("Telefono");
                datiReport.CodFiscaleCollaboratore = riga.Field<string>("CodiceFiscale");
                datiReport.DataLavorazione = riga.Field<DateTime>("Data");
                datiReport.Lavorazione = riga.Field<string>("Lavorazione");
                datiReport.Produzione = riga.Field<string>("Produzione");
                datiReport.Cliente = riga.Field<string>("Cliente");
                datiReport.Descrizione = riga.Field<string>("Descrizione");
                datiReport.Assunzione = riga.Field<decimal>("Assunzione");
                datiReport.Mista = (decimal)riga.Field<decimal>("Mista");
                datiReport.Diaria = riga["Diaria"] != DBNull.Value ? riga.Field<decimal>("Diaria") : 0m;

                datiReport.Albergo = riga.Field<int>("Albergo");
                datiReport.RimborsoKm = riga.Field<decimal>("RimborsoKm");
                //datiReport.TipoPagamento = riga.Field<int>("TipoPagamento");
                //datiReport.DescrizionePagamento = riga.Field<string>("DescrizioneTipoPagamento");

                listaReportConsulenteLavoro.Add(datiReport);
            }

            return listaReportConsulenteLavoro;
        }

        public List<DatiReport> GetListaDatiReportCollaboratoriFornitori(DateTime? dataInizio, DateTime? dataFine, string nominativo, string lavorazione, string produzione, bool soloFornitori, string cliente, ref Esito esito)
        {
            List<DatiReport> listaReportCollaboratoriFornitori = new List<DatiReport>();

            DataTable dtReportConsulenteLavoro = Report_DAL.Instance.GetDatiReportCollaboratoriFornitori(dataInizio, dataFine, nominativo, lavorazione, produzione, soloFornitori, cliente, ref esito);
            foreach (DataRow riga in dtReportConsulenteLavoro.Rows)
            {
                DatiReport datiReport = new DatiReport();

                if (listaReportCollaboratoriFornitori.Count == 0 || listaReportCollaboratoriFornitori.FirstOrDefault(x => x.IdCollaboratore == riga.Field<int>("ID")) == null)
                {
                    datiReport.IdCollaboratore = riga.Field<int>("ID");
                    datiReport.NomeCollaboratore = riga.Field<string>("Nome");
                    datiReport.QualificaCollaboratore = riga.Field<string>("Descrizione");
                    datiReport.IndirizzoCollaboratore = riga.Field<string>("Indirizzo");
                    datiReport.CittaCollaboratore = riga.Field<string>("Citta");
                    datiReport.TelefonoCollaboratore = riga.Field<string>("Telefono");
                    //datiReport.CellulareCollaboratore = riga.Field<string>("");
                    //datiReport.IscrizioneCollaboratore = riga.Field<string>("");
                    datiReport.CodFiscaleCollaboratore = riga.Field<string>("CodiceFiscale");
                    datiReport.ListaDatiFiscali = new List<DatiFiscaliLavorazione>();

                    listaReportCollaboratoriFornitori.Add(datiReport);
                }

                datiReport = listaReportCollaboratoriFornitori.FirstOrDefault(x => x.IdCollaboratore == riga.Field<int>("ID"));

                DatiFiscaliLavorazione datiFiscaliLavorazione = new DatiFiscaliLavorazione();

                datiFiscaliLavorazione.DataLavorazione = riga.Field<DateTime>("Data");
                datiFiscaliLavorazione.Lavorazione = riga.Field<string>("Lavorazione");
                datiFiscaliLavorazione.Produzione = riga.Field<string>("Produzione");
                datiFiscaliLavorazione.Cliente = riga.Field<string>("Cliente");
                datiFiscaliLavorazione.Descrizione = riga.Field<string>("Descrizione");
                datiFiscaliLavorazione.Assunzione = riga.Field<decimal>("Assunzione");
                datiFiscaliLavorazione.Mista = (decimal)riga.Field<int>("Mista");
                datiFiscaliLavorazione.RitenutaAcconto = riga.Field<decimal>("RitenutaAcconto");
                datiFiscaliLavorazione.Fattura = riga.Field<decimal>("Fattura");
                datiFiscaliLavorazione.Diaria = riga.Field<int>("Diaria");
                datiFiscaliLavorazione.RimborsoKm = riga.Field<decimal>("RimborsoKm");
                datiFiscaliLavorazione.TipoPagamento = riga.Field<int>("TipoPagamento");
                datiFiscaliLavorazione.DescrizionePagamento = riga.Field<string>("DescrizioneTipoPagamento");

                datiReport.ListaDatiFiscali.Add(datiFiscaliLavorazione);
            }

            return listaReportCollaboratoriFornitori;
        }

        public List<DatiReportRaw> GetListaDatiReportRawCollaboratoriFornitori(DateTime? dataInizio, DateTime? dataFine, string nominativo, string lavorazione, string produzione, bool soloFornitori, string cliente, ref Esito esito)
        {
            List<DatiReportRaw> listaReportCollaboratoriFornitori = new List<DatiReportRaw>();

            DataTable dtReportConsulenteLavoro = Report_DAL.Instance.GetDatiReportCollaboratoriFornitori(dataInizio, dataFine, nominativo, lavorazione, produzione, soloFornitori, cliente, ref esito);
            foreach (DataRow riga in dtReportConsulenteLavoro.Rows)
            {
                DatiReportRaw datiReport = new DatiReportRaw();
                
                datiReport.IdCollaboratore = riga.Field<int>("ID");
                datiReport.NomeCollaboratore = riga.Field<string>("Nome");
                datiReport.QualificaCollaboratore = riga.Field<string>("Descrizione");
                datiReport.IndirizzoCollaboratore = riga.Field<string>("Indirizzo") == null ? "" : riga.Field<string>("Indirizzo");
                datiReport.CittaCollaboratore = riga.Field<string>("Citta") == null ? "" : riga.Field<string>("Citta");
                datiReport.TelefonoCollaboratore = riga.Field<string>("Telefono") == null ? "": riga.Field<string>("Telefono");
                //datiReport.CellulareCollaboratore = riga.Field<string>("");
                //datiReport.IscrizioneCollaboratore = riga.Field<string>("");
                datiReport.CodFiscaleCollaboratore = riga.Field<string>("CodiceFiscale");

                datiReport.DataLavorazione = riga.Field<DateTime>("Data");
                datiReport.Lavorazione = riga.Field<string>("Lavorazione");
                datiReport.Produzione = riga.Field<string>("Produzione");
                datiReport.Cliente = riga.Field<string>("Cliente");
                datiReport.Descrizione = riga.Field<string>("Descrizione");
                datiReport.Assunzione = riga.Field<decimal>("Assunzione");
                datiReport.Mista = (decimal)riga.Field<int>("Mista");
                datiReport.RitenutaAcconto = riga.Field<decimal>("RitenutaAcconto");
                datiReport.Fattura = riga.Field<decimal>("Fattura");
                datiReport.FatturaLordo = riga.Field<decimal>("FatturaLordo");
                datiReport.Diaria = riga.Field<int>("Diaria");
                datiReport.RimborsoKm = riga.Field<decimal>("RimborsoKm");
                datiReport.TipoPagamento = riga.Field<int>("TipoPagamento");
                datiReport.DescrizionePagamento = riga.Field<string>("DescrizioneTipoPagamento");

                listaReportCollaboratoriFornitori.Add(datiReport);
            }

            return listaReportCollaboratoriFornitori;
        }

        public void EliminaCollaboratoriImportoZero(ref List<DatiReportRaw> listaDatiReport)
        {
            var somma =
                from collab in listaDatiReport
                group collab by collab.IdCollaboratore into gruppoCollab
                select new string[2]
                {
                    gruppoCollab.Key.ToString(),
                    (gruppoCollab.Sum(x => x.Assunzione) + gruppoCollab.Sum(x => x.Mista) + gruppoCollab.Sum(x => x.RimborsoKm) + gruppoCollab.Sum(x => x.RitenutaAcconto) + gruppoCollab.Sum(x => x.Fattura)).ToString()
                };

            foreach (string[] elem in somma)
            {
                if (elem[1] == "0,00") listaDatiReport.RemoveAll(x => x.IdCollaboratore == int.Parse(elem[0]));
            }
        }

        public void EliminaCollaboratoriImportoZero(ref List<DatiReport> listaDatiReport)
        {
            List<DatiReport> listaDatiReport_APPO = new List<DatiReport>();
            foreach (DatiReport elem in listaDatiReport)
            {
                if ((elem.ListaDatiFiscali.Sum(x => x.Assunzione) + elem.ListaDatiFiscali.Sum(x => x.Mista) + elem.ListaDatiFiscali.Sum(x => x.RimborsoKm) + elem.ListaDatiFiscali.Sum(x => x.RitenutaAcconto) + elem.ListaDatiFiscali.Sum(x => x.Fattura)) > 0)
                {
                    listaDatiReport_APPO.Add(elem);
                }
            }

            listaDatiReport = listaDatiReport_APPO;
        }

        public List<DatiReportSinteticaRaw> Converti(List<DatiReportRaw> lista, string dataInizio, string dataFine)
        {
            return lista
                .GroupBy(x => x.IdCollaboratore)
                .Select(g => new DatiReportSinteticaRaw
                {
                    IdCollaboratore = g.Key,
                    NomeCollaboratore = g.First().NomeCollaboratore,
                    IndirizzoCollaboratore = g.First().IndirizzoCollaboratore,
                    CittaCollaboratore = g.First().CittaCollaboratore,
                    TelefonoCollaboratore = g.First().TelefonoCollaboratore,
                    CodFiscaleCollaboratore = g.First().CodFiscaleCollaboratore,
                    //Lavorazione = g.First().Lavorazione,

                    IntervalloDate = dataInizio + " - " + dataFine,
                    //Assunzione = g.Sum(x => x.Assunzione + x.Mista),
                    //RimborsoKm = g.Sum(x => x.RimborsoKm),

                    Assunzione = g.Sum(x => x.Assunzione),
                    RimborsoKm = g.Sum(x => x.RimborsoKm + x.Mista),

                    Diaria = g.Sum(x => x.Diaria),
                    Albergo = g.Sum(x => x.Albergo)
                })
                .ToList();
        }
    }
}