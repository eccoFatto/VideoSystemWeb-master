using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;
namespace VideoSystemWeb.BLL
{
    public class Anag_Email_Collaboratori_BLL
    {
        //singleton
        private static volatile Anag_Email_Collaboratori_BLL instance;
        private static object objForLock = new Object();
        private Anag_Email_Collaboratori_BLL() { }
        public static Anag_Email_Collaboratori_BLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Anag_Email_Collaboratori_BLL();
                    }
                }
                return instance;
            }
        }

        public int CreaEmailCollaboratore(Anag_Email_Collaboratori emailCollaboratore, ref Esito esito)
        {
            int iREt = Anag_Email_Collaboratori_DAL.Instance.CreaEmailCollaboratore(emailCollaboratore, ref esito);

            return iREt;
        }

        public Esito AggiornaEmailCollaboratore(Anag_Email_Collaboratori emailCollaboratore)
        {
            Esito esito = Anag_Email_Collaboratori_DAL.Instance.AggiornaEmailCollaboratore(emailCollaboratore);

            return esito;
        }

        public Esito EliminaEmailCollaboratore(int idEmailCollaboratore)
        {
            Esito esito = Anag_Email_Collaboratori_DAL.Instance.EliminaEmailCollaboratore(idEmailCollaboratore);

            return esito;
        }
    }
}