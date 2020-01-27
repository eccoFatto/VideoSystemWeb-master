using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{
    public class DatiReport
    {
        private int idCollaboratore;
        private string nomeCollaboratore;
        private string qualificaCollaboratore;
        private string indirizzoCollaboratore;
        private string cittaCollaboratore;
        private string telefonoCollaboratore;
        private string cellulareCollaboratore;
        private string iscrizioneCollaboratore;
        private string codFiscaleCollaboratore;
        private List<DatiFiscaliLavorazione> listaDatiFiscali;


        public int IdCollaboratore { get => idCollaboratore; set => idCollaboratore = value; }
        public string NomeCollaboratore { get => nomeCollaboratore; set => nomeCollaboratore = value; }
        public string QualificaCollaboratore { get => qualificaCollaboratore; set => qualificaCollaboratore = value; }
        public string IndirizzoCollaboratore { get => indirizzoCollaboratore; set => indirizzoCollaboratore = value; }
        public string CittaCollaboratore { get => cittaCollaboratore; set => cittaCollaboratore = value; }
        public string TelefonoCollaboratore { get => telefonoCollaboratore; set => telefonoCollaboratore = value; }
        public string CellulareCollaboratore { get => cellulareCollaboratore; set => cellulareCollaboratore = value; }
        public string IscrizioneCollaboratore { get => iscrizioneCollaboratore; set => iscrizioneCollaboratore = value; }
        public string CodFiscaleCollaboratore { get => codFiscaleCollaboratore; set => codFiscaleCollaboratore = value; }
        public List<DatiFiscaliLavorazione> ListaDatiFiscali { get => listaDatiFiscali; set => listaDatiFiscali = value; }
        
    }
}