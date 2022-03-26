using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using VideoSystemWeb.BLL;

namespace VideoSystemWeb.DAL
{
    public class Report_DAL:Base_DAL
    {
        //singleton
        private static volatile Report_DAL instance;
        private static object objForLock = new Object();

        private int idTipoAssunzione;
        private int idTipoMista;
        private int idTipoRitenutaAcconto;
        private int idTipoFattura;
        private int idDiaria;

        private Report_DAL() {
            Esito esito = new Esito();

            idTipoAssunzione = UtilityTipologiche.getElementByNome(UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_PAGAMENTO), "Assunzione", ref esito).id; 
            idTipoMista = UtilityTipologiche.getElementByNome(UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_PAGAMENTO), "Mista", ref esito).id; 
            idTipoRitenutaAcconto = UtilityTipologiche.getElementByNome(UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_PAGAMENTO), "Ritenuta acconto", ref esito).id; 
            idTipoFattura = UtilityTipologiche.getElementByNome(UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_PAGAMENTO), "Fattura", ref esito).id; 
            idDiaria = Art_Articoli_DAL.Instance.CaricaListaArticoli(ref esito).FirstOrDefault(x => x.DefaultDescrizione.Trim().ToUpper() == "DIARIA").Id;
        }

        public static Report_DAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Report_DAL();
                    }
                }
                return instance;
            }
        }

        

        private static int GetIdTipoPagamento(string tipoPagamento)
        {
            Esito esito = new Esito();
            return UtilityTipologiche.getElementByNome(UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_PAGAMENTO), tipoPagamento, ref esito).id;
        }

        public DataTable GetDatiReportConsulenteLavoro(DateTime dataInizio, DateTime dataFine, string nominativo, ref Esito esito)
        {
            string filtroNominativo = string.Empty;
            if (!string.IsNullOrEmpty(nominativo))
            {
                filtroNominativo = "collab.cognome + ' ' + collab.nome like '%" + nominativo + "%' and ";
            }
            DataTable dtReturn = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string querySql = "select collab.id as ID, " +
                                             "collab.cognome + ' ' +  collab.nome as Nome, " +
                                             "indColl.tipo + ' ' + indColl.indirizzo + ' ' + indColl.numeroCivico as Indirizzo, " +
                                             "indColl.comune + ' (' + indColl.provincia + ')' as Citta,  " +
                                             "telColl.naz_pref + telColl.numero as Telefono, " +
                                             "collab.codiceFiscale as CodiceFiscale, " +
                                             "artLav.data as Data, " +
                                             "datiAgenda.lavorazione as Lavorazione, " +
                                             "datiAgenda.produzione as Produzione, " +
                                             "clienti.ragioneSociale as Cliente, " +
                                             "artLav.descrizione as Descrizione, " +
                                             "CASE WHEN artLav.idTipoPagamento = " + idTipoAssunzione + " THEN artLav.fp_netto ELSE 0 END as Assunzione, " +
                                             "CASE WHEN artLav.idTipoPagamento = " + idTipoMista + " THEN 45 ELSE 0 END as Mista, " +

                                             "CASE WHEN artLav.idTipoPagamento = " + idTipoMista + " THEN artLav.fp_netto - 45 ELSE 0 END as RimborsoKm, " +

                                             //"CASE WHEN (select count(*) from dati_articoli_lavorazione where idCollaboratori = collab.id and data = artLav.data and idArtArticoli = " + idDiaria + ") > 0 THEN 1 ELSE 0 END as Diaria, " +
                                             "CASE WHEN (select count(*) from dati_pianoEsterno_lavorazione where idCollaboratori = collab.id and data = artLav.data and diaria = 1) > 0 THEN 1 ELSE 0 END as Diaria, " +
                                             "CASE WHEN(select count(*) from dati_pianoEsterno_lavorazione where idCollaboratori = collab.id and data = artLav.data and albergo = 1) > 0 THEN 1 ELSE 0 END as Albergo, " +

                                             "artLav.idTipoPagamento as TipoPagamento, " +
                                             "tipoPagam.nome as DescrizioneTipoPagamento " +

                                       "from  " +
                                       "dati_articoli_lavorazione artLav  " +
                                       "left join dati_lavorazione datiLav on datiLav.id = artLav.idDatiLavorazione " +
                                       "left join tab_dati_agenda datiAgenda on datiAgenda.id = datiLav.idDatiAgenda " +
                                       "left join anag_collaboratori collab on collab.id=artLav.idCollaboratori " +
                                       "left join anag_indirizzi_collaboratori indColl on indColl.id = (select top 1 id from anag_indirizzi_collaboratori where id_collaboratore = collab.id ) " +
                                       "left join anag_telefoni_collaboratori telColl on telColl.id = (select top 1 id from anag_telefoni_collaboratori where id_collaboratore = collab.id ) " +
                                       "left join anag_clienti_fornitori clienti on clienti.id = datiAgenda.id_cliente " +
                                       "left join tipo_pagamento tipoPagam on artLav.idTipoPagamento = tipoPagam.id " +
                                       
                                       "where  " +
                                       filtroNominativo + 
                                       "artLav.idCollaboratori is not null and " +
                                       "(artLav.idTipoPagamento = " + idTipoAssunzione + " or artLav.idTipoPagamento = " + idTipoMista + ") and " +
                                       "artLav.data between '" + dataInizio.ToString("yyyy-MM-ddT00:00:00.000") + "' and '" + dataFine.ToString("yyyy-MM-ddT00:00:00.000") + "' "+ //"' and " +
                                       
                                       "order by Nome, data";

                    using (SqlCommand cmd = new SqlCommand(querySql))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            sda.Fill(dtReturn);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.Descrizione = "Report_DAL.cs - GetDatiReportConsulenteLavoro " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return dtReturn;
        }

        public DataTable GetDatiReportCollaboratoriFornitori(DateTime dataInizio, DateTime dataFine, string nominativo, string lavorazione, string produzione, bool soloFornitori, string cliente, ref Esito esito)
        {
            string filtroNominativo = string.Empty;
            string filtroRagioneSociale = string.Empty;
            if (!string.IsNullOrEmpty(nominativo))
            {
                filtroNominativo = "collab.cognome + ' ' + collab.nome like '%" + nominativo + "%' and ";
                filtroRagioneSociale = "clientiFornitori.ragioneSociale like '%" + nominativo + "%' and ";
            }

            string filtroLavorazione = string.IsNullOrEmpty(lavorazione) ? string.Empty : "datiAgenda.lavorazione like '%" + lavorazione + "%' and ";
            string filtroProduzione = string.IsNullOrEmpty(produzione) ? string.Empty : "datiAgenda.produzione like '%" + produzione + "%' and ";
            string filtroCliente = string.IsNullOrEmpty(cliente) ? string.Empty : "clienti.ragioneSociale like '%" + cliente + "%' and ";
            string filtroClienteFornitore = string.IsNullOrEmpty(cliente) ? string.Empty : "clientiFornitori.ragioneSociale like '%" + cliente + "%' and "; 

            DataTable dtReturn = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string querySql = "";
                    string filtroSoloFornitori = "";

                    if (!soloFornitori)
                    {
                        filtroSoloFornitori = " and artLav.idTipoPagamento = " + idTipoFattura + " ";
                        querySql = "select collab.id as ID, " +
                                                 "collab.cognome + ' ' + collab.nome as Nome, " +
                                                 "indColl.tipo + ' ' + indColl.indirizzo + ' ' + indColl.numeroCivico as Indirizzo, " +
                                                 "indColl.comune + ' (' + indColl.provincia + ')' as Citta, " +
                                                 "telColl.naz_pref + telColl.numero as Telefono, " +
                                                 "collab.codiceFiscale as CodiceFiscale, " +
                                                 "artLav.data as Data, " +
                                                 "datiAgenda.lavorazione as Lavorazione, " +
                                                 "datiAgenda.produzione as Produzione, " +
                                                 "clienti.ragioneSociale as Cliente, " +
                                                 "artLav.descrizione as Descrizione, " +
                                                 "CASE WHEN artLav.idTipoPagamento = " + idTipoAssunzione + " THEN artLav.fp_netto ELSE 0 END as Assunzione, " +
                                                 "CASE WHEN artLav.idTipoPagamento = " + idTipoMista + " THEN 45 ELSE 0 END as Mista, " +

                                                 "CASE WHEN artLav.idTipoPagamento = " + idTipoMista + " THEN artLav.fp_netto - 45 ELSE 0 END as RimborsoKm, " +

                                                 "CASE WHEN artLav.idTipoPagamento = " + idTipoRitenutaAcconto + " THEN artLav.fp_netto ELSE 0 END as RitenutaAcconto, " +
                                                 "0 as Fattura, " +

                                                 "0 as FatturaLordo, " +

                                                 "CASE WHEN (select count(*) from dati_articoli_lavorazione where idCollaboratori = collab.id and data = artLav.data and idArtArticoli = " + idDiaria + ") > 0 THEN 1 ELSE 0 END as Diaria, " +
                                                 "artLav.idTipoPagamento as TipoPagamento, " +
                                                 "tipoPagam.nome as DescrizioneTipoPagamento " +

                                                 "from  " +
                                                 "dati_articoli_lavorazione artLav " +
                                                 "left join dati_lavorazione datiLav on datiLav.id = artLav.idDatiLavorazione " +
                                                 "left join tab_dati_agenda datiAgenda on datiAgenda.id = datiLav.idDatiAgenda " +
                                                 "left join anag_collaboratori collab on collab.id=artLav.idCollaboratori " +
                                                 "left join anag_indirizzi_collaboratori indColl on indColl.id = (select top 1 id from anag_indirizzi_collaboratori where id_collaboratore = collab.id ) " +
                                                 "left join anag_telefoni_collaboratori telColl on telColl.id = (select top 1 id from anag_telefoni_collaboratori where id_collaboratore = collab.id ) " +
                                                 "left join anag_clienti_fornitori clienti on clienti.id = datiAgenda.id_cliente " +
                                                 "left join tipo_pagamento tipoPagam on artLav.idTipoPagamento = tipoPagam.id " +

                                                 "where  " +
                                                 filtroNominativo +
                                                 filtroLavorazione +
                                                 filtroProduzione +
                                                 filtroCliente + 
                                                 "artLav.idCollaboratori is not null and " +
                                                 "(artLav.idTipoPagamento = " + idTipoAssunzione + " or artLav.idTipoPagamento = " + idTipoMista + " or artLav.idTipoPagamento = " + idTipoRitenutaAcconto + ") and " +
                                                 "artLav.data between '" + dataInizio.ToString("yyyy-MM-ddT00:00:00.000") + "' and '" + dataFine.ToString("yyyy-MM-ddT00:00:00.000") + "' " +//"' and " +

                                                 "UNION ";
                    }

                    querySql += "select clientiFornitori.id as ID, " +
                                             "clientiFornitori.ragioneSociale as Nome, " +
                                             "clientiFornitori.tipoIndirizzoLegale + ' ' + clientiFornitori.indirizzoLegale + ' ' + clientiFornitori.numeroCivicoLegale as Indirizzo, " +
                                             "clientiFornitori.comuneLegale + ' (' + clientiFornitori.provinciaLegale + ')' as Citta,  " +
                                             "clientiFornitori.telefono as Telefono, " +
                                             "clientiFornitori.codiceFiscale as CodiceFiscale, " +
                                             "artLav.data as Data, " +
                                             "datiAgenda.lavorazione as Lavorazione, " +
                                             "datiAgenda.produzione as Produzione, " +
                                             "clientiFornitori.ragioneSociale as Cliente, " +
                                             "artLav.descrizione as Descrizione, " +
                                             "CASE WHEN artLav.idTipoPagamento = " + idTipoAssunzione + " THEN artLav.fp_netto ELSE 0 END as Assunzione, " +
                                             "CASE WHEN artLav.idTipoPagamento = " + idTipoMista + " THEN 45 ELSE 0 END as Mista, " +

                                             "CASE WHEN artLav.idTipoPagamento = " + idTipoMista + " THEN artLav.fp_netto - 45 ELSE 0 END as RimborsoKm, " +

                                             "CASE WHEN artLav.idTipoPagamento = " + idTipoRitenutaAcconto + " THEN artLav.fp_netto ELSE 0 END as RitenutaAcconto, " +
                                             "CASE WHEN artLav.idTipoPagamento = " + idTipoFattura + " THEN artLav.fp_netto ELSE 0 END as Fattura, " +

                                             "CASE WHEN artLav.idTipoPagamento = " + idTipoFattura + " THEN (artLav.fp_netto + (artLav.fp_netto*artLav.Iva/100)) ELSE 0 END as FatturaLordo, " +

                                             "CASE WHEN (select count(*) from dati_articoli_lavorazione where idFornitori = clientiFornitori.id and data = artLav.data and idArtArticoli = " + idDiaria + ") > 0 THEN 1 ELSE 0 END as Diaria, " +
                                             "artLav.idTipoPagamento as TipoPagamento, " +
                                             "tipoPagam.nome as DescrizioneTipoPagamento " +

                                             "from  " +
                                             "dati_articoli_lavorazione artLav " +
                                             "left join dati_lavorazione datiLav on datiLav.id = artLav.idDatiLavorazione " +
                                             "left join tab_dati_agenda datiAgenda on datiAgenda.id = datiLav.idDatiAgenda " +
                                             "left join anag_clienti_fornitori clientiFornitori on clientiFornitori.id=artLav.idFornitori " +
                                             "left join tipo_pagamento tipoPagam on artLav.idTipoPagamento = tipoPagam.id " +

                                             "where  " +
                                             "clientiFornitori.tipo = 'Tecnici' and " + // serve a discriminare i fornitori che sono anche collaboratori
                                             filtroRagioneSociale +
                                             filtroLavorazione + 
                                             filtroProduzione +
                                             filtroClienteFornitore +
                                             "artLav.idFornitori is not null and " +
                                             "(artLav.idTipoPagamento is not null) and " +
                                             "artLav.data between '" + dataInizio.ToString("yyyy-MM-ddT00:00:00.000") + "' and '" + dataFine.ToString("yyyy-MM-ddT00:00:00.000") + "' " +
                                             filtroSoloFornitori + 
                                      "order by Nome, data ";


                    using (SqlCommand cmd = new SqlCommand(querySql))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            sda.Fill(dtReturn);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_GENERICO;
                esito.Descrizione = "Report_DAL.cs - GetDatiReportCollaboratoriFornitori " + Environment.NewLine + ex.Message;

                log.Error(ex.Message + Environment.NewLine + ex.StackTrace);
            }

            return dtReturn;
        }
    }
}