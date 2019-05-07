using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{
    //[id] [int] IDENTITY(1,1) NOT NULL,
    //[id_collaboratore] [int] NOT NULL,
    //[tipoDocumento] [varchar] (20) NOT NULL,
    //[numeroDocumento] [varchar] (20) NOT NULL,
    //[pathDocumento] [varchar] (50) NULL,
    //[attivo] [bit] NOT NULL,

    [Serializable]
    public class Anag_Documenti_Collaboratori
    {
        private int id;
        private int id_collaboratore;
        private string tipoDocumento;
        private string numeroDocumento;
        private string pathDocumento;
        private bool attivo;

        public int Id { get => id; set => id = value; }
        public int Id_collaboratore { get => id_collaboratore; set => id_collaboratore = value; }
        public string TipoDocumento { get => tipoDocumento; set => tipoDocumento = value; }
        public string NumeroDocumento { get => numeroDocumento; set => numeroDocumento = value; }
        public string PathDocumento { get => pathDocumento; set => pathDocumento = value; }
        public bool Attivo { get => attivo; set => attivo = value; }
    }
}