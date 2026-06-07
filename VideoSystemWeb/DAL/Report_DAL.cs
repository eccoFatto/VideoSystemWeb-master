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
        private int idTipoRimborsoKm;

        private Report_DAL() {
            Esito esito = new Esito();

            idTipoAssunzione = UtilityTipologiche.getElementByNome(UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_PAGAMENTO), "Assunzione", ref esito).id; 
            idTipoMista = UtilityTipologiche.getElementByNome(UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_PAGAMENTO), "Assunzione mista", ref esito).id; 
            idTipoRitenutaAcconto = UtilityTipologiche.getElementByNome(UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_PAGAMENTO), "Ritenuta acconto", ref esito).id; 
            idTipoFattura = UtilityTipologiche.getElementByNome(UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_PAGAMENTO), "Fattura", ref esito).id; 
            idDiaria = Art_Articoli_DAL.Instance.CaricaListaArticoli(ref esito).FirstOrDefault(x => x.DefaultDescrizione.Trim().ToUpper() == "DIARIA").Id;
            idTipoRimborsoKm = UtilityTipologiche.getElementByNome(UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_PAGAMENTO), "Rimborso km", ref esito).id;
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

        public DataTable GetDatiReportConsulenteLavoro(DateTime dataInizio, DateTime dataFine, string nominativo, string isAssunto, ref Esito esito)
        {
            string quotaFissaMisto = Config_BLL.Instance.getConfig(ref esito, "QUOTA_FISSA_PAGAMENTO_MISTO").Valore;

            DataTable dtReturn = new DataTable();

            if (esito.Codice == Esito.ESITO_OK)
            {
                //string filtroNominativo = string.Empty;
                //if (!string.IsNullOrEmpty(nominativo))
                //{
                //    filtroNominativo = "collab.cognome + ' ' + collab.nome like '%" + nominativo + "%' and ";
                //}

                //string filtroAssunzione = string.Empty;
                //if (!string.IsNullOrEmpty(isAssunto))
                //{
                //    filtroAssunzione = " collab.assunto = " + isAssunto + " and ";
                //}
                
                try
                {
                    using (SqlConnection con = new SqlConnection(sqlConstr))
                    {
                        #region VECCHIA QUERY
                        //string querySql = "select collab.id as ID, " +
                        //                         "collab.cognome + ' ' +  collab.nome as Nome, " +
                        //                         "indColl.tipo + ' ' + indColl.indirizzo + ' ' + indColl.numeroCivico as Indirizzo, " +
                        //                         "indColl.comune + ' (' + indColl.provincia + ')' as Citta,  " +
                        //                         "telColl.naz_pref + telColl.numero as Telefono, " +
                        //                         "collab.codiceFiscale as CodiceFiscale, " +
                        //                         "artLav.data as Data, " +
                        //                         "datiAgenda.lavorazione as Lavorazione, " +
                        //                         "datiAgenda.produzione as Produzione, " +
                        //                         "clienti.ragioneSociale as Cliente, " +
                        //                         "artLav.descrizione as Descrizione, " +


                        //                         //"CASE WHEN artLav.idTipoPagamento = " + idTipoAssunzione + " THEN artLav.fp_netto ELSE 0 END as Assunzione, " +
                        //                         //"CASE WHEN artLav.idTipoPagamento = " + idTipoMista + " THEN 45 ELSE 0 END as Mista, " +
                        //                         //"CASE WHEN artLav.idTipoPagamento = " + idTipoMista + " THEN artLav.fp_netto - 45 ELSE 0 END as RimborsoKm, " +
                        //                         ////"CASE WHEN artLav.idTipoPagamento = " + idTipoRimborsoKm + " THEN artLav.fp_netto - 45 ELSE 0 END as RimborsoKm, " +

                        //                         "CASE " +
                        //                         "WHEN idTipoPagamento = " + idTipoAssunzione + " THEN fp_netto " +
                        //                         "WHEN idTipoPagamento = " + idTipoMista + " THEN 45 " +
                        //                         "ELSE 0 " +
                        //                         "END AS Assunzione, " +

                        //                         "CASE  " +
                        //                         "WHEN idTipoPagamento = " + idTipoMista + " THEN fp_netto - 45 " +
                        //                         "ELSE 0 " +
                        //                         "END AS Mista, " +

                        //                         "CASE  " +
                        //                         "WHEN idTipoPagamento = " + idTipoRimborsoKm + " THEN fp_netto " +
                        //                         "ELSE 0 " +
                        //                         "END AS RimborsoKm, " +

                        //                         "CASE  WHEN idTipoPagamento = 5 THEN 0 ELSE  pianoEst.importoDiaria END as Diaria, " +





                        //                         //"pianoEst.importoDiaria as Diaria, " +


                        //                         "CASE WHEN(select count(*) from dati_pianoEsterno_lavorazione where idCollaboratori = collab.id and data = artLav.data and albergo = 1) > 0 THEN 1 ELSE 0 END as Albergo, " +

                        //                         "artLav.idTipoPagamento as TipoPagamento, " +
                        //                         "tipoPagam.nome as DescrizioneTipoPagamento " +

                        //                   "from  " +
                        //                   "dati_articoli_lavorazione artLav  " +
                        //                   "left join dati_lavorazione datiLav on datiLav.id = artLav.idDatiLavorazione " +
                        //                   "left join tab_dati_agenda datiAgenda on datiAgenda.id = datiLav.idDatiAgenda " +
                        //                   "left join anag_collaboratori collab on collab.id=artLav.idCollaboratori " +

                        //                   "left join dati_pianoEsterno_lavorazione pianoEst on pianoEst.idCollaboratori = collab.id and pianoEst.data = artLav.data and diaria = 1 " +

                        //                   "left join anag_indirizzi_collaboratori indColl on indColl.id = (select top 1 id from anag_indirizzi_collaboratori where id_collaboratore = collab.id ) " +
                        //                   "left join anag_telefoni_collaboratori telColl on telColl.id = (select top 1 id from anag_telefoni_collaboratori where id_collaboratore = collab.id ) " +
                        //                   "left join anag_clienti_fornitori clienti on clienti.id = datiAgenda.id_cliente " +
                        //                   "left join tipo_pagamento tipoPagam on artLav.idTipoPagamento = tipoPagam.id " +

                        //                   "where  " +
                        //                   filtroNominativo + 
                        //                   "artLav.idCollaboratori is not null and " +
                        //                   "(artLav.idTipoPagamento = " + idTipoAssunzione + " or artLav.idTipoPagamento = " + idTipoMista + " or artLav.idTipoPagamento = " + idTipoRimborsoKm + ") and " +
                        //                   "artLav.data between '" + dataInizio.ToString("yyyy-MM-ddT00:00:00.000") + "' and '" + dataFine.ToString("yyyy-MM-ddT00:00:00.000") + "' "+ //"' and " +

                        //                   "order by Nome, data";
                        #endregion

                        #region NUOVA QUERY RISULTATI IN UNA SOLA RIGA
                        //string querySql = "SELECT collab.id as ID, collab.cognome + ' ' +  collab.nome as Nome, indColl.tipo + ' ' + indColl.indirizzo + ' ' + indColl.numeroCivico as Indirizzo, indColl.comune + ' (' + indColl.provincia + ')' as Citta,  telColl.naz_pref + telColl.numero as Telefono, collab.codiceFiscale as CodiceFiscale, artLav.data as Data, datiAgenda.lavorazione as Lavorazione, datiAgenda.produzione as Produzione, clienti.ragioneSociale as Cliente, artLav.descrizione as Descrizione, " +

                        //                    "CASE  " +
                        //                    "WHEN artLav.idTipoPagamento = " + idTipoAssunzione + " THEN artLav.fp_netto  " +
                        //                    "WHEN artLav.idTipoPagamento = " + idTipoMista + " THEN " + quotaFissaMisto + "  " +
                        //                    "ELSE 0  " +
                        //                    "END AS Assunzione,  " +

                        //                    "CASE " +
                        //                    "WHEN artLav.idTipoPagamento = " + idTipoMista + " THEN artLav.fp_netto - " + quotaFissaMisto + " ELSE 0  " +
                        //                    "END AS Mista,  " +

                        //                    // RimborsoKm preso dalla riga con tipoPagamento = 5 "
                        //                    "ISNULL(( " +
                        //                    "SELECT TOP 1 artLav2.fp_netto FROM dati_articoli_lavorazione artLav2 WHERE artLav2.idCollaboratori = artLav.idCollaboratori AND artLav2.data = artLav.data AND artLav2.idTipoPagamento = " + idTipoRimborsoKm + "), 0) " +
                        //                    "AS RimborsoKm, " +

                        //                    "CASE " +
                        //                    "WHEN artLav.idTipoPagamento = " + idTipoRimborsoKm + " THEN 0 ELSE pianoEst.importoDiaria  " +
                        //                    "END as Diaria,  " +

                        //                    "CASE  " +
                        //                    "WHEN ( " +
                        //                    "SELECT COUNT(*)  " +
                        //                    "FROM dati_pianoEsterno_lavorazione  " +
                        //                    "WHERE idCollaboratori = collab.id AND data = artLav.data AND albergo = 1) > 0 THEN 1 ELSE 0  " +
                        //                    "END as Albergo,  " +

                        //                    "artLav.idTipoPagamento as TipoPagamento, tipoPagam.nome as DescrizioneTipoPagamento  " +

                        //                    "FROM dati_articoli_lavorazione artLav  " +
                        //                    "LEFT JOIN dati_lavorazione datiLav ON datiLav.id = artLav.idDatiLavorazione " +
                        //                    "LEFT JOIN tab_dati_agenda datiAgenda ON datiAgenda.id = datiLav.idDatiAgenda " +
                        //                    "LEFT JOIN anag_collaboratori collab ON collab.id = artLav.idCollaboratori " +
                        //                    "LEFT JOIN dati_pianoEsterno_lavorazione pianoEst ON pianoEst.idCollaboratori = collab.id AND pianoEst.data = artLav.data AND pianoEst.diaria = 1 " +
                        //                    "LEFT JOIN anag_indirizzi_collaboratori indColl ON indColl.id = (SELECT TOP 1 id FROM anag_indirizzi_collaboratori WHERE id_collaboratore = collab.id ) " +
                        //                    "LEFT JOIN anag_telefoni_collaboratori telColl ON telColl.id = (SELECT TOP 1 id FROM anag_telefoni_collaboratori WHERE id_collaboratore = collab.id ) " +
                        //                    "LEFT JOIN anag_clienti_fornitori clienti ON clienti.id = datiAgenda.id_cliente " +
                        //                    "LEFT JOIN tipo_pagamento tipoPagam ON artLav.idTipoPagamento = tipoPagam.id  " +

                        //                    "WHERE  " +
                        //                    filtroNominativo +
                        //                    filtroAssunzione +
                        //                    " artLav.idCollaboratori IS NOT NULL and " +

                        //                    //SOLO riga principale (niente tipo 5) 
                        //                    "artLav.idTipoPagamento IN (" + idTipoAssunzione + ", " + idTipoMista + ")  and " +
                        //                    "artLav.data between '" + dataInizio.ToString("yyyy-MM-ddT00:00:00.000") + "' and '" + dataFine.ToString("yyyy-MM-ddT00:00:00.000") + "' " +
                        //                    "ORDER BY Nome, Data; ";
                        #endregion

                        #region VERSIONE 07_06_2026 DOPO SEGNALAZIONE FEDERICO CHIAPPA

                        string dataInizioString = dataInizio.ToString("yyyy-MM-ddT00:00:00.000");
                        string dataFineString = dataFine.ToString("yyyy-MM-ddT00:00:00.000");

                        string filtroNominativo = string.Empty;
                        if (!string.IsNullOrEmpty(nominativo))
                        {
                            filtroNominativo = " a.Nome LIKE '%" + nominativo + "%' and ";
                        }

                        string filtroAssunzione = string.Empty;
                        if (!string.IsNullOrEmpty(isAssunto))
                        {
                            filtroAssunzione = " a.assunto = " + isAssunto + " and ";
                        }

                        string querySql = "WITH " +
                                            "Base AS ( " +
                                            "    SELECT artLav.idCollaboratori, artLav.data, " +
                                            // 💰 Assunzione
                                            "          SUM(CASE WHEN artLav.idTipoPagamento = "+ idTipoAssunzione + " THEN artLav.fp_netto  WHEN artLav.idTipoPagamento = 2 THEN " + quotaFissaMisto + "  ELSE 0  END) AS Assunzione, " +
                                            // 💰 Mista
                                            "          SUM(CASE WHEN artLav.idTipoPagamento = " + idTipoMista + " THEN artLav.fp_netto - " + quotaFissaMisto + " ELSE 0 END) AS Mista, " +
                                            // 🚗 Rimborso KM
                                            "          SUM(CASE WHEN artLav.idTipoPagamento = " + idTipoRimborsoKm + " THEN artLav.fp_netto ELSE 0 END) AS RimborsoKm " +

                                            "    FROM dati_articoli_lavorazione artLav " +
	
                                            "    WHERE  " +
                                            "        artLav.idTipoPagamento IN (" + idTipoAssunzione + ", " + idTipoMista + ", " + idTipoRimborsoKm+ ") " +
                                            "        AND artLav.data BETWEEN '" + dataInizioString + "' AND '" + dataFineString + "' " +
		
                                            "    GROUP BY artLav.idCollaboratori, artLav.data " +
                                            "), " +

                                            "Piano AS ( " +
                                            "    SELECT idCollaboratori, data, " +
                                            "	        MAX(importoDiaria) as Diaria,  " +
                                            "	        MAX(CASE WHEN albergo = 1 THEN 1 ELSE 0 END) as Albergo " +
	
                                            "    FROM dati_pianoEsterno_lavorazione " +
	
                                            "    GROUP BY idCollaboratori, data " +
                                            "), " +

                                            "Agenda AS ( " +
                                            "    SELECT artLav.idCollaboratori, artLav.data, " +

                                            "           MAX(clienti.ragioneSociale) as Cliente, " +
                                            "           MAX(datiAgenda.lavorazione) as Lavorazione, " +
                                            "           MAX(datiAgenda.produzione) as Produzione, " +
                                            "           MAX(artLav.descrizione) as Descrizione " +

                                            "    FROM dati_articoli_lavorazione artLav " +
                                            "         LEFT JOIN dati_lavorazione datiLav ON datiLav.id = artLav.idDatiLavorazione  " +
                                            "         LEFT JOIN tab_dati_agenda datiAgenda ON datiAgenda.id = datiLav.idDatiAgenda  " +
                                            "         LEFT JOIN anag_clienti_fornitori clienti ON clienti.id = datiAgenda.id_cliente  " +

                                            "    WHERE artLav.data BETWEEN '" + dataInizioString + "' AND '" + dataFineString + "' " +

                                            "    GROUP BY artLav.idCollaboratori, artLav.data " +
                                            "), " +

                                            "Anagrafica AS ( " +
                                            "    SELECT collab.id, collab.cognome + ' ' + collab.nome as Nome, collab.codiceFiscale, collab.assunto, " +

                                            "           MAX(indColl.tipo + ' ' + indColl.indirizzo + ' ' + indColl.numeroCivico) as Indirizzo, " +
                                            "           MAX(indColl.comune + ' (' + indColl.provincia + ')') as Citta, " +
                                            "           MAX(telColl.naz_pref + telColl.numero) as Telefono " +

                                            "    FROM anag_collaboratori collab " +
                                            "         LEFT JOIN anag_indirizzi_collaboratori indColl ON indColl.id = (SELECT TOP 1 id FROM anag_indirizzi_collaboratori WHERE id_collaboratore = collab.id) " +
                                            "         LEFT JOIN anag_telefoni_collaboratori telColl ON telColl.id = (SELECT TOP 1 id FROM anag_telefoni_collaboratori WHERE id_collaboratore = collab.id) " +

                                            "    GROUP BY collab.id, collab.cognome, collab.nome, collab.codiceFiscale, collab.assunto " +
                                            ") " +

                                            "SELECT a.id as ID, a.Nome, a.Indirizzo, a.Citta, a.Telefono, a.codiceFiscale, b.data as Data, ag.Lavorazione, ag.Produzione, ag.Cliente, ag.Descrizione, b.Assunzione, b.Mista, b.RimborsoKm, ISNULL(p.Diaria, 0) as Diaria, ISNULL(p.Albergo, 0) as Albergo " +

                                            "FROM Base b " +
                                            "     JOIN Anagrafica a ON a.id = b.idCollaboratori " +
                                            "     LEFT JOIN Piano p ON p.idCollaboratori = b.idCollaboratori AND p.data = b.data " +
                                            "     LEFT JOIN Agenda ag ON ag.idCollaboratori = b.idCollaboratori AND ag.data = b.data " +

                                            "WHERE " +
                                            filtroNominativo +
                                            filtroAssunzione +
                                            "1=1 " +
                                            "ORDER BY a.Nome, b.data ";

                        #endregion

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

                
            }
            else
            {
                esito.Codice = Esito.ESITO_KO_ERRORE_LETTURA_TABELLA;
                esito.Descrizione = "Report_DAL.cs - GetDatiReportConsulenteLavoro " + Environment.NewLine + "Errore nel recupero della quota fissa per il pagamento misto";
            }

            return dtReturn;
        }

        // LA SEZIONE STAMPA COLLABORATORI È STATA ELIMINATA (NASCOSTA). IN CASO DI RIPRISTINO RIVEDERE LA QUERY CONFRONTANDOLA CON QUELLA SOPRA
        public DataTable GetDatiReportCollaboratoriFornitori(DateTime? dataInizio, DateTime? dataFine, string nominativo, string lavorazione, string produzione, bool soloFornitori, string cliente, ref Esito esito)
        {
            string intervalloDate = string.Empty;
            string filtroNominativo = string.Empty;
            string filtroRagioneSociale = string.Empty;

            if (dataInizio != null && dataFine != null)
            {
                intervalloDate = " and artLav.data between '" + ((DateTime)dataInizio).ToString("yyyy-MM-ddT00:00:00.000") + "' and '" + ((DateTime)dataFine).ToString("yyyy-MM-ddT00:00:00.000") + "' ";
            }
            else if (dataInizio != null)
            {
                intervalloDate = " and artLav.data >= '" + ((DateTime)dataInizio).ToString("yyyy-MM-ddT00:00:00.000") + "' ";
            }
            else if (dataFine != null)
            {
                intervalloDate = " and artLav.data <= '" + ((DateTime)dataFine).ToString("yyyy-MM-ddT00:00:00.000") + "' ";
            }

            if (!string.IsNullOrEmpty(nominativo))
            {
                filtroNominativo = "collab.cognome + ' ' + collab.nome like '%" + nominativo + "%' and ";
                filtroRagioneSociale = "clientiFornitori.ragioneSociale like '%" + nominativo + "%' and ";
            }

            string filtroLavorazione = string.IsNullOrEmpty(lavorazione) ? string.Empty : "datiAgenda.lavorazione like '%" + lavorazione + "%' and ";
            string filtroProduzione = string.IsNullOrEmpty(produzione) ? string.Empty : "datiAgenda.produzione like '%" + produzione + "%' and ";
            string filtroCliente = string.IsNullOrEmpty(cliente) ? string.Empty : "clienti.ragioneSociale like '%" + cliente + "%' and ";// or clienti.fornitore = 'true') and ";
            string filtroClienteFornitore = string.IsNullOrEmpty(cliente) ? string.Empty : "clientiFornitori2.ragioneSociale like '%" + cliente + "%' and ";// or clientiFornitori.fornitore = 'true') and "; 

            DataTable dtReturn = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(sqlConstr))
                {
                    string querySql = "";
                    string filtroSoloFornitori = "";

                    if (!soloFornitori)
                    {
                        //COLLABORATORI
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
                                                 "clienti.ragioneSociale as Cliente, " + //************************
                                                 //"datiAgenda.id_cliente as Cliente, " +

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
                                                 "(artLav.idTipoPagamento = " + idTipoAssunzione + " or artLav.idTipoPagamento = " + idTipoMista + " or artLav.idTipoPagamento = " + idTipoRitenutaAcconto + ") " +

                                                 //*********************************
                                                 " and (artLav.idTipoGruppo != 6 or artLav.idTipoSottogruppo != 29) " + // FILTRO RIMBORSO KM

                                                 intervalloDate +

                                                 "UNION ";





                        //*********************************
                        //***PROVA RIMBORSO KM

                        querySql += "select collab.id as ID, " +
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

                                                 "0 as Assunzione, " +
                                                 "0 as Mista, " +
                                                 "artLav.fp_netto as RimborsoKm, " +
                                                 "0 as RitenutaAcconto, " +

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
                                                 "(artLav.idTipoGruppo = 6 and artLav.idTipoSottogruppo = 29) " + // FILTRO RIMBORSO KM

                                                 intervalloDate +

                                                 "UNION ";
                    }
                    //CLIENTI FORNITORI
                    querySql += "select clientiFornitori.id as ID, " +
                                             "clientiFornitori.ragioneSociale as Nome, " +
                                             "clientiFornitori.tipoIndirizzoLegale + ' ' + clientiFornitori.indirizzoLegale + ' ' + clientiFornitori.numeroCivicoLegale as Indirizzo, " +
                                             "clientiFornitori.comuneLegale + ' (' + clientiFornitori.provinciaLegale + ')' as Citta,  " +
                                             "clientiFornitori.telefono as Telefono, " +
                                             "clientiFornitori.codiceFiscale as CodiceFiscale, " +
                                             "artLav.data as Data, " +
                                             "datiAgenda.lavorazione as Lavorazione, " +
                                             "datiAgenda.produzione as Produzione, " +
                                             "clientiFornitori2.ragioneSociale as Cliente, " + //************************
                                             //"datiAgenda.id_cliente as Cliente, " +

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
                                             "left join anag_clienti_fornitori clientiFornitori2 on clientiFornitori2.id=datiAgenda.id_cliente " +

                                             "where  " +
                                             "clientiFornitori.tipo = 'Tecnici' and " + // serve a discriminare i fornitori che sono anche collaboratori
                                             filtroRagioneSociale +
                                             filtroLavorazione + 
                                             filtroProduzione +
                                             filtroClienteFornitore +
                                             "artLav.idFornitori is not null and " +
                                             "(artLav.idTipoPagamento is not null) " +
                                             
                                             //"artLav.data between '" + dataInizio.ToString("yyyy-MM-ddT00:00:00.000") + "' and '" + dataFine.ToString("yyyy-MM-ddT00:00:00.000") + "' " +
                                             intervalloDate +

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