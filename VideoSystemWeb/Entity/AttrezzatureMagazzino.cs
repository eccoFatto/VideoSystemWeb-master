using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{
    //[id]
    //[cod_vs]
    //[id_categoria]
    //[id_subcategoria]
    //[descrizione]
    //[seriale]
    //[data_acquisto]
    //[garanzia]
    //[disponibile]
    //[id_posizione_magazzino]
    //[marca]
    //[modello]
    //[note]

    public class AttrezzatureMagazzino
    {
        private int id;
        private string cod_vs;
        private int id_categoria;
        private int? id_subcategoria;
        private string descrizione;
        private string seriale;
        private DateTime data_acquisto;
        private bool garanzia;
        private bool disponibile;
        private int id_posizione_magazzino;
        private string marca;
        private string modello;
        private string note;
        private bool attivo;
        private int? id_gruppo_magazzino;

        public int Id { get => id; set => id = value; }
        public string Cod_vs { get => cod_vs; set => cod_vs = value; }
        public int Id_categoria { get => id_categoria; set => id_categoria = value; }
        public int? Id_subcategoria { get => id_subcategoria; set => id_subcategoria = value; }
        public string Descrizione { get => descrizione; set => descrizione = value; }
        public string Seriale { get => seriale; set => seriale = value; }
        public DateTime Data_acquisto { get => data_acquisto; set => data_acquisto = value; }
        public bool Garanzia { get => garanzia; set => garanzia = value; }
        public bool Disponibile { get => disponibile; set => disponibile = value; }
        public int Id_posizione_magazzino { get => id_posizione_magazzino; set => id_posizione_magazzino = value; }
        public string Marca { get => marca; set => marca = value; }
        public string Modello { get => modello; set => modello = value; }
        public string Note { get => note; set => note = value; }
        public bool Attivo { get => attivo; set => attivo = value; }
        public int? Id_gruppo_magazzino { get => id_gruppo_magazzino; set => id_gruppo_magazzino = value; }
    }
}