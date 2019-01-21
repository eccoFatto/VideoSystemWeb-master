using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoSystemWeb.BLL;
using VideoSystemWeb.Entity;
using VideoSystemWeb.Entity.Json;

namespace VideoSystemWeb
{
    public partial class Agenda : System.Web.UI.Page
    {
        private bool _isCalendarEditable;
        //private string _eventi;
        
        public string isCalendarEditable
        {
            get {
                return _isCalendarEditable.ToString().ToLower();
            }
            set { }
        }

        //public string eventi
        //{
        //    get
        //    {
        //        return _eventi.ToString();
        //    }
        //    set { }
        //}
        protected void Page_Load(object sender, EventArgs e)
        {
            _isCalendarEditable = ((Anag_Utente)Session["Utente"]).tipo==Tipologie.TIPO_OPERATORE;
            setStringaEventi();
        }

        private void setStringaEventi()
        {
            //DateTime oggi = DateTime.Now;
            //int year = oggi.Year;
            //int month = oggi.Month;
            //int day = oggi.Day;
            //int hour = oggi.Hour;
            //int minute = oggi.Minute;
            
            //Evento evento1 = new Evento(){ id= 1, title = "Ballando con le stelle", start = new DateTime(year,month,1), allDay=true, type = Tipologie.EVENTO_VIAGGIO };
            //Evento evento2 = new Evento() { id = 2, title = "Serie A - Roma-Udinese", start = new DateTime(year, month, day).AddDays(-5), end = new DateTime(year, month, day).AddDays(-2), type = Tipologie.EVENTO_VIAGGIO };
            //Evento evento3 = new Evento() { id = 999, title = "Evento ripetuto", start = new DateTime(year, month, day, 16, 0, 0).AddDays(-3), allDay = false, type = Tipologie.EVENTO_VIAGGIO };
            //Evento evento4 = new Evento() { id = 999, title = "Evento ripetuto", start = new DateTime(year, month, day, 16, 0, 0).AddDays(4), allDay = false, type = Tipologie.EVENTO_VIAGGIO };
            //Evento evento5 = new Evento() { id = 5, title = "Grande Fratello VIP", start = new DateTime(year, month, day, 10, 30, 0), allDay = false, description= "prova descrizione evento", type = Tipologie.EVENTO_PREVISIONE };
            //Evento evento6 = new Evento() { id = 6, title = "A1 - Basket", start = new DateTime(year, month, day, 12, 0, 0), end = new DateTime(year, month, day, 14, 0, 0), allDay = false, type = Tipologie.EVENTO_VIAGGIO };
            //Evento evento7 = new Evento() { id = 7, title = "A1 - Volley", start = new DateTime(year, month, day, 19, 0, 0).AddDays(1), end = new DateTime(year, month, day, 22, 30, 0).AddDays(1), allDay = false, type = Tipologie.EVENTO_VIAGGIO };
            //Evento evento8 = new Evento() { id = 8, title = "Riposo Regia1", start = new DateTime(year, month, 28), end = new DateTime(year, month, 29), url = "www.google.it", type = Tipologie.EVENTO_RIPOSI };

            //List<Evento_Json> listaEventi = new List<Evento_Json>();
            //listaEventi.Add(new Evento_Json(evento1));
            //listaEventi.Add(new Evento_Json(evento2));
            //listaEventi.Add(new Evento_Json(evento3));
            //listaEventi.Add(new Evento_Json(evento4));
            //listaEventi.Add(new Evento_Json(evento5));
            //listaEventi.Add(new Evento_Json(evento6));
            //listaEventi.Add(new Evento_Json(evento7));
            //listaEventi.Add(new Evento_Json(evento8));

            //_eventi = JsonConvert.SerializeObject(listaEventi);

            
        }
    }
}