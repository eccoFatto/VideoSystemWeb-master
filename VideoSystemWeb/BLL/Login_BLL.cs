using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;

namespace VideoSystemWeb.BLL
{
    public class Login_BLL
    {
        //singleton
        private static volatile Login_BLL instance;
        private static object objForLock = new Object();

        private Login_BLL() { }

        public static Login_BLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Login_BLL();
                    }
                }
                return instance;
            }
        }

        public void Connetti(string tbUser, string tbPassword, ref Esito esito)
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["USA_DB"]))
            {
                Login_DAL.Instance.Connetti(tbUser, tbPassword, ref esito);
            }
            else
            {
                HttpContext.Current.Session[SessionManager.UTENTE] = new Anag_Utenti() { id = 1, Nome = "Nicola", Cognome = "Foti", id_tipoUtente = 2 };
            }
        }

    }
}