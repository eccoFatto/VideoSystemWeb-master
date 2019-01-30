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
    }
}