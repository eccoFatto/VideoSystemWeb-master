using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.BLL;

namespace VideoSystemWeb.Entity
{
    [Serializable]
    public class DatiLavorazione
    {
        private int id;
        private int idDatiAgenda;
        private int? idContratto;
        private int? idReferente;
        private int? idCapoTecnico;
        private int? idProduttore;
        private string ordine;
        private string fattura;
        private string notePianoEsterno;
        private List<DatiArticoliLavorazione> listaArticoliLavorazione;
        private List<FiguraProfessionale> listaFigureProfessionali;
        private List<DatiPianoEsternoLavorazione> listaDatiPianoEsternoLavorazione;
        private string descrizioneLavorazione;
        private string codiceLavorazione;

        public int Id { get => id; set => id = value; }
        public int IdDatiAgenda { get => idDatiAgenda; set => idDatiAgenda = value; }
        public int? IdContratto { get => idContratto; set => idContratto = value; }
        public int? IdReferente { get => idReferente; set => idReferente = value; }
        public int? IdCapoTecnico { get => idCapoTecnico; set => idCapoTecnico = value; }
        public int? IdProduttore { get => idProduttore; set => idProduttore = value; }
        public string Ordine { get => ordine; set => ordine = value; }
        public string Fattura { get => fattura; set => fattura = value; }
        public string NotePianoEsterno { get => notePianoEsterno; set => notePianoEsterno = value; }
        public List<DatiArticoliLavorazione> ListaArticoliLavorazione { get => listaArticoliLavorazione; set => listaArticoliLavorazione = value; }
        public List<FiguraProfessionale> ListaFigureProfessionali
        { get
            {
                listaFigureProfessionali = new List<FiguraProfessionale>();
                if (ListaDatiPianoEsternoLavorazione != null)
                {
                    foreach (DatiPianoEsternoLavorazione datoPianoEsterno in ListaDatiPianoEsternoLavorazione)
                    {
                        FiguraProfessionale figProf = new FiguraProfessionale();
                        DatiArticoliLavorazione datoArticoloLavorazione = new DatiArticoliLavorazione();

                        string descrizioneArticoloAssociato = "";
                        if (datoPianoEsterno.IdCollaboratori != null && datoPianoEsterno.IdCollaboratori != 0)
                        {
                            Anag_Collaboratori collaboratore = SessionManager.ListaAnagraficheCollaboratori.FirstOrDefault(x => x.Id == datoPianoEsterno.IdCollaboratori);
                            datoArticoloLavorazione = ListaArticoliLavorazione.FirstOrDefault(x => x.IdCollaboratori == datoPianoEsterno.IdCollaboratori);

                            // prendo descrizione da datiArticoliLavorazione filtrando per data, idCollaboratore e idLavorazione
                            DatiArticoliLavorazione articoloAssociato = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.FirstOrDefault(x => x.IdCollaboratori == datoPianoEsterno.IdCollaboratori && x.Data == datoPianoEsterno.Data);
                            if (articoloAssociato != null)
                            {
                                descrizioneArticoloAssociato = articoloAssociato.Descrizione;
                            }

                            figProf = collaboratore.CreaFiguraProfessionale(descrizioneArticoloAssociato);
                        }
                        else if (datoPianoEsterno.IdFornitori != null && datoPianoEsterno.IdFornitori != 0)
                        {
                            Anag_Clienti_Fornitori fornitore = SessionManager.ListaAnagraficheFornitori.FirstOrDefault(x => x.Id == datoPianoEsterno.IdFornitori);
                            datoArticoloLavorazione = ListaArticoliLavorazione.FirstOrDefault(x => x.IdFornitori == datoPianoEsterno.IdFornitori);

                            // prendo descrizione da datiArticoliLavorazione filtrando per data, idFornitore e idLavorazione
                            DatiArticoliLavorazione articoloAssociato = SessionManager.EventoSelezionato.LavorazioneCorrente.ListaArticoliLavorazione.FirstOrDefault(x => x.IdFornitori == datoPianoEsterno.IdFornitori && x.Data == datoPianoEsterno.Data);
                            if (articoloAssociato != null)
                            {
                                descrizioneArticoloAssociato = articoloAssociato.Descrizione;
                            }

                            figProf = fornitore.CreaFiguraProfessionale(descrizioneArticoloAssociato);
                        }

                        if (datoArticoloLavorazione != null)
                        {
                            figProf.Nota = datoArticoloLavorazione.Nota;
                            figProf.Lordo = datoArticoloLavorazione.FP_lordo;
                            figProf.Netto = datoArticoloLavorazione.FP_netto;
                            figProf.Data = datoPianoEsterno.Data;
                            figProf.Intervento = datoPianoEsterno.IdIntervento == null ? "" : SessionManager.ListaTipiIntervento.FirstOrDefault(x => x.id == datoPianoEsterno.IdIntervento).nome;
                            figProf.Diaria = datoPianoEsterno.ImportoDiaria;
                            figProf.Nota = datoPianoEsterno.Nota;
                            figProf.NumOccorrenza = datoPianoEsterno.NumOccorrenza;

                            listaFigureProfessionali.Add(figProf);
                        }
                    }
                }
                return listaFigureProfessionali;
            }
        }
        public List<DatiPianoEsternoLavorazione> ListaDatiPianoEsternoLavorazione { get => listaDatiPianoEsternoLavorazione; set => listaDatiPianoEsternoLavorazione = value; }
        public string DescrizioneLavorazione { get => descrizioneLavorazione; set => descrizioneLavorazione = value; }
        public string CodiceLavorazione { get => codiceLavorazione; set => codiceLavorazione = value; }
    }
}