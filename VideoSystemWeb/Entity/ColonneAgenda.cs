using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{
    [Serializable]
    public class ColonneAgenda: Tipologica
    {
        private int ordinamento;

        public int Ordinamento { get => ordinamento; set => ordinamento = value; }
    }
}