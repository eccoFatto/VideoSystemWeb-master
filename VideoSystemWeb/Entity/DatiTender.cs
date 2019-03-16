using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{
    [Serializable]
    public class DatiTender
    {
        private int id;
        private int idDatiAgenda;
        private int idTender;

        public int Id { get => id; set => id = value; }
        public int IdDatiAgenda { get => idDatiAgenda; set => idDatiAgenda = value; }
        public int IdTender { get => idTender; set => idTender = value; }
    }
}