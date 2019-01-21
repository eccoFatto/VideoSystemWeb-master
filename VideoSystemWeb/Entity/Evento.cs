using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{
    public class Evento
    {
        public int id;
        public string title;
        public DateTime start;
        public DateTime end;
        public bool allDay;
        public string description;
        public string url;
        public Tipo_Evento type;
    }
}