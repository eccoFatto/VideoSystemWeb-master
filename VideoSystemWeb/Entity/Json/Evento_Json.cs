using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity.Json
{
    [Serializable]
    public class Evento_Json
    {
        public int id;
        public string title;
        public DateTime start;
        public DateTime end;
        public bool allDay;
        public string description;
        public string url;
        public string color;


        public Evento_Json(Evento evento)
        {
            this.id = evento.id;
            this.title = evento.title;
            this.start = evento.start;
            this.end = evento.end;
            this.allDay = evento.allDay;
            this.description = evento.description;
            this.url = evento.url;
            this.color = evento.type.colore;
        }
    }
}