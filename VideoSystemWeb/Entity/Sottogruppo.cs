using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{
    [Serializable]
    public class Sottogruppo : Tipologica
    {
        private int? idTipoGruppo;

        public int? IdTipoGruppo { get => idTipoGruppo; set => idTipoGruppo = value; }
    }
}