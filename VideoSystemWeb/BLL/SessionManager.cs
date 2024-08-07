﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using VideoSystemWeb.Entity;

namespace VideoSystemWeb.BLL
{
    public static class SessionManager
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string UTENTE = "Utente";
        public static string LISTA_TIPI_UTENTE = "listaTipiUtente";
        public static string LISTA_RISORSE = "listaRisorse";
        public static string LISTA_TIPI_STATO = "listaStati";
        public static string LISTA_TIPI_TIPOLOGIA = "listaTipiTipologie";
        public static string LISTA_TIPI_QUALIFICA = "listaQualifiche";
        public static string LISTA_CLIENTI_FORNITORI = "listaClientiFornitori";
        public static string DATA_SELEZIONATA = "dataSelezionata";

        public static string CERCA_LAVORAZIONE_DATA = "cercaLavorazione_Data";
        public static string CERCA_LAVORAZIONE_COLONNA = "cercaLavorazione_Colonna";
        public static string CERCA_LAVORAZIONE_RISULTATI = "cercaLavorazione_Risultati";
        public static string CERCA_LAVORAZIONE_NUM_PAGINA = "cercaLavorazione_NumPagina";
        public static string CERCA_LAVORAZIONE_FILTRI = "cercaLavorazione_Filtri";


        // CHIAVI DI TAB_CONFIG
        public static string CFG_IVA = "IVA";
        public static string CFG_IBAN = "IBAN";
        public static string CFG_TOPONIMO = "TOPONIMO";
        public static string CFG_INDIRIZZO = "INDIRIZZO";
        public static string CFG_CIVICO = "CIVICO";
        public static string CFG_CAP = "CAP";
        public static string CFG_CITTA = "CITTA";
        public static string CFG_PROVINCIA = "PROVINCIA";

        public static string CFG_ALLINEAMENTO_DIPENDENTI_AGENDA = "ALLINEAMENTO_DIPENDENTI_AGENDA";


        public static string LISTA_COLLABORATORI_PER_INVIO_WHATSAPP = "LISTA_COLLABORATORI_PER_INVIO_WHATSAPP";
        public static string LISTA_CLIENTIFORNITORI_PER_INVIO_WHATSAPP = "LISTA_CLIENTIFORNITORI_PER_INVIO_WHATSAPP";

        public static string LISTA_ATTREZZATURE_SELEZIONATE_DOC_TRASPORTO = "LISTA_ATTREZZATURE_SELEZIONATE_DOC_TRASPORTO";

        public static DatiAgenda EventoSelezionato
        {
            get
            {
                return (DatiAgenda)HttpContext.Current.Session["eventoSelezionato"];
            }

            set
            {
                HttpContext.Current.Session["eventoSelezionato"] = value;
            }
        }

        public static List<FiguraProfessionale> ListaCompletaFigProf
        {
            get
            {
                if (HttpContext.Current.Session["listaCompletaFigProf"] == null || ((List<FiguraProfessionale>)HttpContext.Current.Session["listaCompletaFigProf"]).Count() == 0)
                {
                    
                    List<FiguraProfessionale> _listaCompletaFigProf = new List<FiguraProfessionale>();
                    foreach (Anag_Collaboratori collaboratore in ListaAnagraficheCollaboratori)
                    {
                        try
                        {
                            _listaCompletaFigProf.Add(new FiguraProfessionale()
                            {

                                Id = collaboratore.Id,
                                Nome = collaboratore.Nome,
                                Cognome = collaboratore.Cognome,
                                Citta = collaboratore.ComuneRiferimento.Trim().ToLower(),
                                //Telefono = collaboratore.Telefoni.Count == 0 ? "" : collaboratore.Telefoni.FirstOrDefault(x => x.Priorita == 1).Pref_naz + collaboratore.Telefoni.FirstOrDefault(x => x.Priorita == 1).Numero,
                                Telefono = collaboratore.Telefoni.Count == 0 ? "" : collaboratore.Telefoni.OrderBy(x => x.Priorita).ToList()[0].NumeroCompleto,
                                Qualifiche = collaboratore.Qualifiche,
                                Tipo = 0,
                                Nota = collaboratore.Note,
                                IsAssunto = collaboratore.Assunto
                            });

                        }
                        catch (Exception ex)
                        {
                            log.Error("ListaCompletaFigProf COLLABORATORE - " + collaboratore.Id.ToString(), ex);
                        }
                    }

                    foreach (Anag_Clienti_Fornitori fornitore in ListaAnagraficheFornitori)
                    {
                        try
                        {
                            _listaCompletaFigProf.Add(new FiguraProfessionale()
                            {
                                Id = fornitore.Id,
                                Cognome = fornitore.RagioneSociale,
                                Citta = fornitore.ComuneLegale.Trim().ToLower(),
                                Telefono = fornitore.Telefono,
                                Tipo = 1,
                                Nota = fornitore.Note,
                                IsAssunto = false
                            });

                        }
                        catch (Exception ex)
                        {
                            log.Error("ListaCompletaFigProf FORNITORE - " + fornitore.Id.ToString(), ex);
                        }
                    }
                    HttpContext.Current.Session["listaCompletaFigProf"] = _listaCompletaFigProf.OrderBy(x => x.Cognome).ToList<FiguraProfessionale>();
                }
                return (List<FiguraProfessionale>)HttpContext.Current.Session["listaCompletaFigProf"];
            }
            set
            {
                HttpContext.Current.Session["listaCompletaFigProf"] = value;
            }
        }

        public static List<Anag_Qualifiche_Collaboratori> ListaQualificheCollaboratori
        {
            get
            {
                if (HttpContext.Current.Session["listaQualificheCollaboratori"] == null || ((List<Anag_Qualifiche_Collaboratori>)HttpContext.Current.Session["listaQualificheCollaboratori"]).Count() == 0)
                {
                    Esito esito = new Esito();
                    HttpContext.Current.Session["listaQualificheCollaboratori"] = Anag_Qualifiche_Collaboratori_BLL.Instance.getAllQualifiche(ref esito, true);
                }
                return (List<Anag_Qualifiche_Collaboratori>)HttpContext.Current.Session["listaQualificheCollaboratori"];
            }
            set
            {
                HttpContext.Current.Session["listaQualificheCollaboratori"] = value;
            }
        }
        public static List<Anag_Collaboratori> ListaAnagraficheCollaboratori
        {
            get
            {
                if (HttpContext.Current.Session["listaAnagraficheCollaboratori"] == null || ((List<Anag_Collaboratori>)HttpContext.Current.Session["listaAnagraficheCollaboratori"]).Count() == 0)
                {
                    Esito esito = new Esito();
                    HttpContext.Current.Session["listaAnagraficheCollaboratori"] = Anag_Collaboratori_BLL.Instance.getAllCollaboratori(ref esito);
                }
                return (List<Anag_Collaboratori>)HttpContext.Current.Session["listaAnagraficheCollaboratori"];
            }

            set
            {
                HttpContext.Current.Session["listaAnagraficheCollaboratori"] = value;
            }
        }

        public static List<Anag_Clienti_Fornitori> ListaAnagraficheFornitori
        {
            get
            {
                if (HttpContext.Current.Session["listaAnagraficheFornitori"] == null || ((List<Anag_Clienti_Fornitori>)HttpContext.Current.Session["listaAnagraficheFornitori"]).Count() == 0)
                {
                    Esito esito = new Esito();
                    HttpContext.Current.Session["listaAnagraficheFornitori"] = Anag_Clienti_Fornitori_BLL.Instance.CaricaListaFornitori(ref esito, false);
                }
                return (List<Anag_Clienti_Fornitori>)HttpContext.Current.Session["listaAnagraficheFornitori"];
            }

            set
            {
                HttpContext.Current.Session["listaAnagraficheFornitori"] = value;
            }
        }
        public static List<Tipologica> ListaTipiPagamento
        {
            get
            {
                if (HttpContext.Current.Session["listaTipiPagamento"] == null || ((List<Tipologica>)HttpContext.Current.Session["listaTipiPagamento"]).Count() == 0)
                {
                    HttpContext.Current.Session["listaTipiPagamento"] = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_PAGAMENTO);
                }
                return (List<Tipologica>)HttpContext.Current.Session["listaTipiPagamento"];
            }
            set
            {
                HttpContext.Current.Session["listaTipiPagamento"] = value;
            }
        }
        public static List<Anag_Referente_Clienti_Fornitori> ListaReferenti
        {
            get
            {
                if (HttpContext.Current.Session["listaReferenti"] == null || ((List<Anag_Referente_Clienti_Fornitori>)HttpContext.Current.Session["listaReferenti"]).Count() == 0)
                {
                    HttpContext.Current.Session["listaReferenti"] = new List<Anag_Referente_Clienti_Fornitori>();
                }
                return (List<Anag_Referente_Clienti_Fornitori>)HttpContext.Current.Session["listaReferenti"];
            }
            set
            {
                HttpContext.Current.Session["listaReferenti"] = value;
            }
        }
        public static List<GiorniPagamentoFatture> ListaGPF
        {
            get
            {
                Esito esito = new Esito();
                if (HttpContext.Current.Session["listaGPF"] == null || ((List<GiorniPagamentoFatture>)HttpContext.Current.Session["listaGPF"]).Count() == 0)
                {
                    HttpContext.Current.Session["listaGPF"] = Config_BLL.Instance.getListaGiorniPagamentoFatture(ref esito);
                }
                return (List<GiorniPagamentoFatture>)HttpContext.Current.Session["listaGPF"];
            }
            set
            {
                HttpContext.Current.Session["listaGPF"] = value;
            }
        }
        public static List<DatiBancari> ListaDatiBancari
        {
            get
            {
                Esito esito = new Esito();
                if (HttpContext.Current.Session["listaDatiBancari"] == null || ((List<DatiBancari>)HttpContext.Current.Session["listaDatiBancari"]).Count() == 0)
                {
                    HttpContext.Current.Session["listaDatiBancari"] = Config_BLL.Instance.getListaDatiBancari(ref esito);
                }
                return (List<DatiBancari>)HttpContext.Current.Session["listaDatiBancari"];
            }
            set
            {
                HttpContext.Current.Session["listaDatiBancari"] = value;
            }
        }
        public static List<Tipologica> ListaTender
        {
            get
            {
                if (HttpContext.Current.Session["listaTender"] == null || ((List<Tipologica>)HttpContext.Current.Session["listaTender"]).Count() == 0)
                {
                    HttpContext.Current.Session["listaTender"] = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_TENDER);
                }
                return (List<Tipologica>)HttpContext.Current.Session["listaTender"];
            }
            set
            {
                HttpContext.Current.Session["listaTender"] = value;
            }
        }
        public static List<Tipologica> ListaQualifiche
        {
            get
            {
                if (HttpContext.Current.Session["listaQualifiche"] == null || ((List<Tipologica>)HttpContext.Current.Session["listaQualifiche"]).Count() == 0)
                {
                    HttpContext.Current.Session["listaQualifiche"] = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_QUALIFICHE);
                }
                return (List<Tipologica>)HttpContext.Current.Session["listaQualifiche"];
            }
            set
            {
                HttpContext.Current.Session["listaQualifiche"] = value;
            }
        }
        public static List<Tipologica> ListaTipiGeneri
        {
            get
            {
                if (HttpContext.Current.Session["listaTipiGeneri"] == null || ((List<Tipologica>)HttpContext.Current.Session["listaTipiGeneri"]).Count() == 0)
                {
                    HttpContext.Current.Session["listaTipiGeneri"] = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_GENERE);
                }
                return (List<Tipologica>)HttpContext.Current.Session["listaTipiGeneri"];
            }
            set
            {
                HttpContext.Current.Session["listaTipiGeneri"] = value;
            }
        }
        public static List<Tipologica> ListaTipiGruppi
        {
            get
            {
                if (HttpContext.Current.Session["listaTipiGruppi"] == null || ((List<Tipologica>)HttpContext.Current.Session["listaTipiGruppi"]).Count() == 0)
                {
                    HttpContext.Current.Session["listaTipiGruppi"] = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_GRUPPO);
                }
                return (List<Tipologica>)HttpContext.Current.Session["listaTipiGruppi"];
            }
            set
            {
                HttpContext.Current.Session["listaTipiGruppi"] = value;
            }
        }
        public static List<Tipologica> ListaTipiSottogruppi
        {
            get
            {
                if (HttpContext.Current.Session["listaTipiSottogruppi"] == null || ((List<Tipologica>)HttpContext.Current.Session["listaTipiSottogruppi"]).Count() == 0)
                {
                    HttpContext.Current.Session["listaTipiSottogruppi"] = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_SOTTOGRUPPO);
                }
                return (List<Tipologica>)HttpContext.Current.Session["listaTipiSottogruppi"];
            }
            set
            {
                HttpContext.Current.Session["listaTipiSottogruppi"] = value;
            }
        }
        public static List<Tipologica> ListaTipiProtocolli
        {
            get
            {
                if (HttpContext.Current.Session["listaTipiProtocolli"] == null || ((List<Tipologica>)HttpContext.Current.Session["listaTipiProtocolli"]).Count() == 0)
                {
                    HttpContext.Current.Session["listaTipiProtocolli"] = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_PROTOCOLLO);
                }
                return (List<Tipologica>)HttpContext.Current.Session["listaTipiProtocolli"];
            }
            set
            {
                HttpContext.Current.Session["listaTipiProtocolli"] = value;
            }
        }
        public static List<Tipologica> ListaTipiUtente
        {
            get
            {
                if (HttpContext.Current.Session["listaTipiUtente"] == null || ((List<Tipologica>)HttpContext.Current.Session["listaTipiUtente"]).Count() == 0)
                {
                    HttpContext.Current.Session["listaTipiUtente"] = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_UTENTE);
                }
                return (List<Tipologica>)HttpContext.Current.Session["listaTipiUtente"];
            }
            set
            {
                HttpContext.Current.Session["listaTipiUtente"] = value;
            }
        }
        public static List<Tipologica> ListaTipiClientiFornitori
        {
            get
            {
                if (HttpContext.Current.Session["listaTipiClientiFornitori"] == null || ((List<Tipologica>)HttpContext.Current.Session["listaTipiClientiFornitori"]).Count() == 0)
                {
                    HttpContext.Current.Session["listaTipiClientiFornitori"] = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_CLIENTI_FORNITORI);
                }
                return (List<Tipologica>)HttpContext.Current.Session["listaTipiClientiFornitori"];
            }
            set
            {
                HttpContext.Current.Session["listaTipiClientiFornitori"] = value;
            }
        }
        public static List<Tipologica> ListaTipiTipologie
        {
            get
            {
                if (HttpContext.Current.Session["listaTipiTipologie"] == null || ((List<Tipologica>)HttpContext.Current.Session["listaTipiTipologie"]).Count() == 0)
                {
                    HttpContext.Current.Session["listaTipiTipologie"] = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_TIPOLOGIE);
                }
                return (List<Tipologica>)HttpContext.Current.Session["listaTipiTipologie"];
            }
            set
            {
                HttpContext.Current.Session["listaTipiTipologie"] = value;
            }
        }
        public static List<Tipologica> ListaTipiIntervento
        {
            get
            {
                if (HttpContext.Current.Session["listaTipiIntervento"] == null || ((List<Tipologica>)HttpContext.Current.Session["listaTipiIntervento"]).Count() == 0)
                {
                    HttpContext.Current.Session["listaTipiIntervento"] = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_INTERVENTO);
                }
                return (List<Tipologica>)HttpContext.Current.Session["listaTipiIntervento"];
            }
            set
            {
                HttpContext.Current.Session["listaTipiIntervento"] = value;
            }
        }
        public static List<ColonneAgenda> ListaRisorse
        {
            get
            {
                if (HttpContext.Current.Session["listaRisorse"] == null || ((List<ColonneAgenda>)HttpContext.Current.Session["listaRisorse"]).Count() == 0)
                {
                    Esito esito = new Esito();
                    List<ColonneAgenda> _listaRisorse = UtilityTipologiche.CaricaColonneAgenda(true, ref esito);
                    if (esito.Codice == 0)
                    {
                        // CONTROLLO ALLINEAMENTO DIPENDENTI NELL'AGENDA
                        string allineamentoDipendentiInAgenda = VideoSystemWeb.DAL.Config_DAL.Instance.GetConfig(ref esito, SessionManager.CFG_ALLINEAMENTO_DIPENDENTI_AGENDA).valore;

                        if (allineamentoDipendentiInAgenda.Equals("DESTRA"))
                        {
                            _listaRisorse = _listaRisorse.OrderBy(c => c.sottotipo == "dipendenti").ThenBy(c => c.sottotipo == "extra").ThenBy(c => c.sottotipo).ToList<ColonneAgenda>();
                        }
                        else
                        {
                            _listaRisorse = _listaRisorse.OrderBy(c => c.sottotipo == "extra").ThenBy(c => c.sottotipo == "regie").ThenBy(c => c.sottotipo).ToList<ColonneAgenda>();
                        }
                    }
                    HttpContext.Current.Session["listaRisorse"] = _listaRisorse;
                }
                return (List<ColonneAgenda>)HttpContext.Current.Session["listaRisorse"];
            }
            set
            {
                HttpContext.Current.Session["listaRisorse"] = value;
            }
        }
        public static List<Tipologica> ListaStati
        {
            get
            {
                if (HttpContext.Current.Session["listaStati"] == null || ((List<Tipologica>)HttpContext.Current.Session["listaStati"]).Count() == 0)
                {
                    HttpContext.Current.Session["listaStati"] = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_STATO);
                }
                return (List<Tipologica>)HttpContext.Current.Session["listaStati"];
            }
            set
            {
                HttpContext.Current.Session["listaStati"] = value;
            }
        }
        public static List<Anag_Clienti_Fornitori> ListaClientiFornitori
        {
            get
            {
                if (HttpContext.Current.Session[LISTA_CLIENTI_FORNITORI] == null || ((List<Anag_Clienti_Fornitori>)HttpContext.Current.Session[LISTA_CLIENTI_FORNITORI]).Count() == 0)
                {
                    Esito esito = new Esito();
                   
                    HttpContext.Current.Session[LISTA_CLIENTI_FORNITORI] = Anag_Clienti_Fornitori_BLL.Instance.CaricaListaAziende(ref esito);
                }
                return (List<Anag_Clienti_Fornitori>)HttpContext.Current.Session[LISTA_CLIENTI_FORNITORI];
            }
            set
            {
                HttpContext.Current.Session[LISTA_CLIENTI_FORNITORI] = value;
            }
        }
        
        public static List<Tipologica> ListaTipiCategorieMagazzino
        {
            get
            {
                if (HttpContext.Current.Session["ListaTipiCategorieMagazzino"] == null || ((List<Tipologica>)HttpContext.Current.Session["ListaTipiCategorieMagazzino"]).Count() == 0)
                {
                    HttpContext.Current.Session["ListaTipiCategorieMagazzino"] = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_CATEGORIE_MAGAZZINO);
                }
                return (List<Tipologica>)HttpContext.Current.Session["ListaTipiCategorieMagazzino"];
            }
            set
            {
                HttpContext.Current.Session["ListaTipiCategorieMagazzino"] = value;
            }
        }
        public static List<Tipologica> ListaTipiSubCategorieMagazzino
        {
            get
            {
                if (HttpContext.Current.Session["ListaTipiSubCategorieMagazzino"] == null || ((List<Tipologica>)HttpContext.Current.Session["ListaTipiSubCategorieMagazzino"]).Count() == 0)
                {
                    HttpContext.Current.Session["ListaTipiSubCategorieMagazzino"] = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_SUB_CATEGORIE_MAGAZZINO);
                }
                return (List<Tipologica>)HttpContext.Current.Session["ListaTipiSubCategorieMagazzino"];
            }
            set
            {
                HttpContext.Current.Session["ListaTipiSubCategorieMagazzino"] = value;
            }
        }
        public static List<Tipologica> ListaTipiPosizioniMagazzino
        {
            get
            {
                if (HttpContext.Current.Session["ListaTipiPosizioniMagazzino"] == null || ((List<Tipologica>)HttpContext.Current.Session["ListaTipiPosizioniMagazzino"]).Count() == 0)
                {
                    HttpContext.Current.Session["ListaTipiPosizioniMagazzino"] = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_POSIZIONE_MAGAZZINO);
                }
                return (List<Tipologica>)HttpContext.Current.Session["ListaTipiPosizioniMagazzino"];
            }
            set
            {
                HttpContext.Current.Session["ListaTipiPosizioniMagazzino"] = value;
            }
        }
        public static List<Tipologica> ListaTipiGruppoMagazzino
        {
            get
            {
                if (HttpContext.Current.Session["ListaTipiGruppoMagazzino"] == null || ((List<Tipologica>)HttpContext.Current.Session["ListaTipiGruppoMagazzino"]).Count() == 0)
                {
                    HttpContext.Current.Session["ListaTipiGruppoMagazzino"] = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_GRUPPO_MAGAZZINO);
                }
                return (List<Tipologica>)HttpContext.Current.Session["ListaTipiGruppoMagazzino"];
            }
            set
            {
                HttpContext.Current.Session["ListaTipiGruppoMagazzino"] = value;
            }
        }
        public static List<Tipologica> ListaTipiBanca
        {
            get
            {
                if (HttpContext.Current.Session["listaTipiBanca"] == null || ((List<Tipologica>)HttpContext.Current.Session["listaTipiBanca"]).Count() == 0)
                {
                    HttpContext.Current.Session["listaTipiBanca"] = UtilityTipologiche.caricaTipologica(EnumTipologiche.TIPO_BANCA);
                }
                return (List<Tipologica>)HttpContext.Current.Session["listaTipiBanca"];
            }
            set
            {
                HttpContext.Current.Session["listaTipiBanca"] = value;
            }
        }
        public static bool VisualizzazioneAutomaticaPopupErrore
        {
            get
            {
                if (HttpContext.Current.Session["visualizzazioneAutomaticaPopupErrore"] == null )
                {
                    Esito esito = new Esito();
                    string popupErroreAutomatico = Config_BLL.Instance.getConfig(ref esito, "POPUP_ERRORE_AUTOMATICO").Valore;
                    HttpContext.Current.Session["visualizzazioneAutomaticaPopupErrore"] = popupErroreAutomatico.ToLower().Trim() == "true";
                }
                return (bool)HttpContext.Current.Session["visualizzazioneAutomaticaPopupErrore"];
            }
            set
            {
                HttpContext.Current.Session["visualizzazioneAutomaticaPopupErrore"] = value;
            }
        }

        public static void ClearSession()
        {
            HttpContext.Current.Session.Clear();
        }

        public static void ClearEventoSelezionato()
        {
            EventoSelezionato.ListaDatiArticoli = null;
            EventoSelezionato.LavorazioneCorrente = null;
            EventoSelezionato = null;
        }

        public static DateTime DataSelezionata
        {
            get
            {
                if (HttpContext.Current.Session[DATA_SELEZIONATA] == null)
                {
                    HttpContext.Current.Session[DATA_SELEZIONATA] = DateTime.Now;
                }
                return (DateTime)HttpContext.Current.Session[DATA_SELEZIONATA];
            }
            set
            {
                HttpContext.Current.Session[DATA_SELEZIONATA] = value;
            }
        }

        public static string CercaLavorazione_Data
        {
            get
            {
                if (HttpContext.Current.Session[CERCA_LAVORAZIONE_DATA] == null)
                {
                    HttpContext.Current.Session[CERCA_LAVORAZIONE_DATA] = string.Empty;
                }
                return HttpContext.Current.Session[CERCA_LAVORAZIONE_DATA].ToString();
            }
            set
            {
                HttpContext.Current.Session[CERCA_LAVORAZIONE_DATA] = value;
            }
        }

        public static string CercaLavorazione_Colonna
        {
            get
            {
                if (HttpContext.Current.Session[CERCA_LAVORAZIONE_COLONNA] == null)
                { 
                    HttpContext.Current.Session[CERCA_LAVORAZIONE_COLONNA] = string.Empty;
                }
                return HttpContext.Current.Session[CERCA_LAVORAZIONE_COLONNA].ToString();
            }
            set
            {
                HttpContext.Current.Session[CERCA_LAVORAZIONE_COLONNA] = value;
            }
        }

        public static DataTable CercaLavorazione_Risultati
        {
            get
            {
                return (DataTable)HttpContext.Current.Session[CERCA_LAVORAZIONE_RISULTATI];
            }
            set
            {
                HttpContext.Current.Session[CERCA_LAVORAZIONE_RISULTATI] = value;
            }
        }

        public static int CercaLavorazione_NumPagina
        {
            get
            {
                if (HttpContext.Current.Session[CERCA_LAVORAZIONE_NUM_PAGINA] == null)
                {
                    HttpContext.Current.Session[CERCA_LAVORAZIONE_NUM_PAGINA] = 0;
                }
                return (int)HttpContext.Current.Session[CERCA_LAVORAZIONE_NUM_PAGINA];
            }
            set
            {
                HttpContext.Current.Session[CERCA_LAVORAZIONE_NUM_PAGINA] = value;
            }
        }

        public static FiltriCercaLavorazione CercaLavorazione_Filtri
        {
            get
            {
                return (FiltriCercaLavorazione)HttpContext.Current.Session[CERCA_LAVORAZIONE_FILTRI];
            }
            set
            {
                HttpContext.Current.Session[CERCA_LAVORAZIONE_FILTRI] = value;
            }
        }
    }
}