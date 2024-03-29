﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;
namespace VideoSystemWeb.BLL
{
    public class Anag_Referente_Clienti_Fornitori_BLL
    {
        //singleton
        private static volatile Anag_Referente_Clienti_Fornitori_BLL instance;
        private static object objForLock = new Object();
        private Anag_Referente_Clienti_Fornitori_BLL() { }
        public static Anag_Referente_Clienti_Fornitori_BLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new Anag_Referente_Clienti_Fornitori_BLL();
                    }
                }
                return instance;
            }
        }

        public Anag_Referente_Clienti_Fornitori getReferenteById(ref Esito esito, int id)
        {
            Anag_Referente_Clienti_Fornitori referenteREt = Anag_Referente_Clienti_Fornitori_DAL.Instance.getReferenteById(ref esito,id);

            return referenteREt;
        }

        public List<Anag_Referente_Clienti_Fornitori> getReferentiByIdAzienda(ref Esito esito, int idCliente)
        {
            return Anag_Referente_Clienti_Fornitori_DAL.Instance.getReferentiByIdAzienda(ref esito, idCliente);
        }

        public int CreaReferente(Anag_Referente_Clienti_Fornitori referente, Anag_Utenti utente, ref Esito esito)
        {
            int iREt = Anag_Referente_Clienti_Fornitori_DAL.Instance.CreaReferente(referente, utente, ref esito);

            return iREt;
        }

        public Esito AggiornaReferente(Anag_Referente_Clienti_Fornitori referente, Anag_Utenti utente)
        {
            Esito esito = Anag_Referente_Clienti_Fornitori_DAL.Instance.AggiornaReferente(referente, utente);

            return esito;
        }

        public Esito EliminaReferente(int idReferente, Anag_Utenti utente)
        {
            Esito esito = Anag_Referente_Clienti_Fornitori_DAL.Instance.EliminaReferente(idReferente, utente);

            return esito;
        }

    }
}