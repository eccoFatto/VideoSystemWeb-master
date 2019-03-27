using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{
    //@id int
    //@codice_lavoro varchar(20),
    //@numero_protocollo varchar(20),
    //@cliente varchar(60),
    //@id_tipo_protocollo int,
    //@pathDocumento varchar(100),
    //@descrizione varchar(200),
    //@attivo bit

    public class Protocolli
    {
        private int id;
        private string codice_lavoro;       // OTTENUTO DALLA GET CODICE LAVORO DELLA TABELLA TAB_COD_LAV
        private string numero_protocollo;   // OTTENUTO DALLA GET PROTOCOLLO DELLA TABELLA TAB_PROTOCOLLO
        private string cliente;             // DALLA TABELLA ANAG_CLIENTI_FORNITORI, COLONNA ragioneSociale
        private int id_tipo_protocollo;           // TIPOLOGIA DEL DOCUMENTO PROTOCOLLATO, EVENTUALMENTE DA PRENDERE DA TIPOLOGICA APPOSITA
        private string protocollo_riferimento;
        private string pathDocumento;       // PATH DEL DOCUMENTO UPLOADATO - NOMENCLATURA = IDPROTOCOLLO_TICK.*
        private string descrizione;
        private bool attivo;

        public int Id { get => id; set => id = value; }
        public string Codice_lavoro { get => codice_lavoro; set => codice_lavoro = value; }
        public string Numero_protocollo { get => numero_protocollo; set => numero_protocollo = value; }
        public string Cliente { get => cliente; set => cliente = value; }
        public string PathDocumento { get => pathDocumento; set => pathDocumento = value; }
        public string Descrizione { get => descrizione; set => descrizione = value; }
        public bool Attivo { get => attivo; set => attivo = value; }
        public int Id_tipo_protocollo { get => id_tipo_protocollo; set => id_tipo_protocollo = value; }
        public string Protocollo_riferimento { get => protocollo_riferimento; set => protocollo_riferimento = value; }
    }
}