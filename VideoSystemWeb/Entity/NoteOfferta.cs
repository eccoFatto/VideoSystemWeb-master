using System;

namespace VideoSystemWeb.Entity
{
    [Serializable]
    public class NoteOfferta
    {
        private int id;
        private int id_dati_agenda;
        private string banca;
        private int pagamento;
        private string consegna;
        private string note;

        public int Id { get => id; set => id = value; }
        public int Id_dati_agenda { get => id_dati_agenda; set => id_dati_agenda = value; }
        public string Banca { get => banca; set => banca = value; }
        public int Pagamento { get => pagamento; set => pagamento = value; }
        public string Consegna { get => consegna; set => consegna = value; }
        public string Note { get => note; set => note = value; }
    }

    
}