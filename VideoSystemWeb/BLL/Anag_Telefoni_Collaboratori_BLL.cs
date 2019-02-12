using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;
namespace VideoSystemWeb.BLL
{
    public class Anag_Telefoni_Collaboratori_BLL
    {
        //singleton
        private static volatile Anag_Telefoni_Collaboratori_BLL instance;
        private static object objForLock = new Object();
        private Anag_Telefoni_Collaboratori_BLL() { }
        public static Anag_Telefoni_Collaboratori_BLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Anag_Telefoni_Collaboratori_BLL();
                    }
                }
                return instance;
            }
        }

        public Anag_Telefoni_Collaboratori getTelefonoById(ref Esito esito, int id)
        {
            Anag_Telefoni_Collaboratori telefonoREt = Anag_Telefoni_Collaboratori_DAL.Instance.getTelefonoById(ref esito,id);

            return telefonoREt;
        }

        public int CreaTelefonoCollaboratore(Anag_Telefoni_Collaboratori telefonoCollaboratore, ref Esito esito)
        {
            int iREt = Anag_Telefoni_Collaboratori_DAL.Instance.CreaTelefonoCollaboratore(telefonoCollaboratore, ref esito);

            return iREt;
        }

        public Esito AggiornaTelefonoCollaboratore(Anag_Telefoni_Collaboratori telefonoCollaboratore)
        {
            Esito esito = Anag_Telefoni_Collaboratori_DAL.Instance.AggiornaTelefonoCollaboratore(telefonoCollaboratore);

            return esito;
        }

        public Esito EliminaTelefonoCollaboratore(int idTelefonoCollaboratore)
        {
            Esito esito = Anag_Telefoni_Collaboratori_DAL.Instance.EliminaTelefonoCollaboratore(idTelefonoCollaboratore);

            return esito;
        }

    }
}