using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.Entity;

namespace VideoSystemWeb.BLL
{
    public static class Utility
    {
        public static string getParametroDaTipologica(Tipologica tipologica, string nomeParametro)
        {
            string[] elencoParametri = tipologica.parametri.Split(';');
            foreach (string param in elencoParametri)
            {
                if (param.ToUpper().StartsWith(nomeParametro.ToUpper()))
                {
                    int index = param.IndexOf("=");
                    return param.Substring(index+1).Trim();
                }
            }
            return "";
        }
    }
}