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

        public List<DatiReport> GetListaDatiReportConsulenteLavoro(DateTime dataInizio, DateTime dataFine, ref Esito esito)
        {
            List<DatiReport> listaReportConsulenteLavoro = new List<DatiReport>();

            DataTable dtReportConsulenteLavoro = Report_DAL.Instance.GetDatiReportConsulenteLavoro(dataInizio, dataFine, ref esito);
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
                datiFiscaliLavorazione.Mista = riga.Field<decimal>("Mista");
                datiFiscaliLavorazione.Diaria = riga.Field<int>("Diaria");

                datiReport.ListaDatiFiscali.Add(datiFiscaliLavorazione);
            }                       

            return listaReportConsulenteLavoro;
        }

        public List<DatiReportRaw> GetListaDatiReportRawConsulenteLavoro(DateTime dataInizio, DateTime dataFine, ref Esito esito)
        {
            List<DatiReportRaw> listaReportConsulenteLavoro = new List<DatiReportRaw>();

            DataTable dtReportConsulenteLavoro = Report_DAL.Instance.GetDatiReportConsulenteLavoro(dataInizio, dataFine, ref esito);
            foreach (DataRow riga in dtReportConsulenteLavoro.Rows)
            {
                DatiReportRaw datiReport = new DatiReportRaw();

                datiReport.IdCollaboratore = riga.Field<int>("ID");
                datiReport.NomeCollaboratore = riga.Field<string>("Nome");
                datiReport.QualificaCollaboratore = riga.Field<string>("Descrizione");
                datiReport.IndirizzoCollaboratore = riga.Field<string>("Indirizzo");
                datiReport.CittaCollaboratore = riga.Field<string>("Citta");
                datiReport.TelefonoCollaboratore = riga.Field<string>("Telefono");
                //datiReport.CellulareCollaboratore = riga.Field<string>("");
                //datiReport.IscrizioneCollaboratore = riga.Field<string>("");
                datiReport.CodFiscaleCollaboratore = riga.Field<string>("CodiceFiscale");

                datiReport.DataLavorazione = riga.Field<DateTime>("Data");
                datiReport.Lavorazione = riga.Field<string>("Lavorazione");
                datiReport.Produzione = riga.Field<string>("Produzione");
                datiReport.Cliente = riga.Field<string>("Cliente");
                datiReport.Descrizione = riga.Field<string>("Descrizione");
                datiReport.Assunzione = riga.Field<decimal>("Assunzione");
                datiReport.Mista = riga.Field<decimal>("Mista");
                datiReport.Diaria = riga.Field<int>("Diaria");

                listaReportConsulenteLavoro.Add(datiReport);

            }

            return listaReportConsulenteLavoro;
        }

        public List<DatiReport> GetListaDatiReportCollaboratoriFornitori(DateTime dataInizio, DateTime dataFine, ref Esito esito)
        {
            List<DatiReport> listaReportCollaboratoriFornitori = new List<DatiReport>();

            DataTable dtReportConsulenteLavoro = Report_DAL.Instance.GetDatiReportCollaboratoriFornitori(dataInizio, dataFine, ref esito);
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
                datiFiscaliLavorazione.Mista = riga.Field<decimal>("Mista");
                datiFiscaliLavorazione.RitenutaAcconto = riga.Field<decimal>("RitenutaAcconto");
                datiFiscaliLavorazione.Fattura = riga.Field<decimal>("Fattura");
                datiFiscaliLavorazione.Diaria = riga.Field<int>("Diaria");

                datiReport.ListaDatiFiscali.Add(datiFiscaliLavorazione);
            }

            return listaReportCollaboratoriFornitori;
        }

        public List<DatiReportRaw> GetListaDatiReportRawCollaboratoriFornitori(DateTime dataInizio, DateTime dataFine, ref Esito esito)
        {
            List<DatiReportRaw> listaReportCollaboratoriFornitori = new List<DatiReportRaw>();

            DataTable dtReportConsulenteLavoro = Report_DAL.Instance.GetDatiReportCollaboratoriFornitori(dataInizio, dataFine, ref esito);
            foreach (DataRow riga in dtReportConsulenteLavoro.Rows)
            {
                DatiReportRaw datiReport = new DatiReportRaw();

                
                datiReport.IdCollaboratore = riga.Field<int>("ID");
                datiReport.NomeCollaboratore = riga.Field<string>("Nome");
                datiReport.QualificaCollaboratore = riga.Field<string>("Descrizione");
                datiReport.IndirizzoCollaboratore = riga.Field<string>("Indirizzo");
                datiReport.CittaCollaboratore = riga.Field<string>("Citta");
                datiReport.TelefonoCollaboratore = riga.Field<string>("Telefono");
                //datiReport.CellulareCollaboratore = riga.Field<string>("");
                //datiReport.IscrizioneCollaboratore = riga.Field<string>("");
                datiReport.CodFiscaleCollaboratore = riga.Field<string>("CodiceFiscale");

                datiReport.DataLavorazione = riga.Field<DateTime>("Data");
                datiReport.Lavorazione = riga.Field<string>("Lavorazione");
                datiReport.Produzione = riga.Field<string>("Produzione");
                datiReport.Cliente = riga.Field<string>("Cliente");
                datiReport.Descrizione = riga.Field<string>("Descrizione");
                datiReport.Assunzione = riga.Field<decimal>("Assunzione");
                datiReport.Mista = riga.Field<decimal>("Mista");
                datiReport.RitenutaAcconto = riga.Field<decimal>("RitenutaAcconto");
                datiReport.Fattura = riga.Field<decimal>("Fattura");
                datiReport.Diaria = riga.Field<int>("Diaria");

                listaReportCollaboratoriFornitori.Add(datiReport);
            }

            return listaReportCollaboratoriFornitori;
        }
    }
}