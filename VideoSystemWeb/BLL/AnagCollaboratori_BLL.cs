using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;
namespace VideoSystemWeb.BLL
{
    public class AnagCollaboratori_BLL
    {
        //singleton
        private static volatile AnagCollaboratori_BLL instance;
        private static object objForLock = new Object();
        private AnagCollaboratori_BLL() { }
        public static AnagCollaboratori_BLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new AnagCollaboratori_BLL();
                    }
                }
                return instance;
            }
        }

    }
}