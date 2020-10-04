using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.DAL;
using VideoSystemWeb.Entity;
using System.Configuration;
namespace VideoSystemWeb.BLL
{
    public class DocumentiTrasporto_BLL
    {
        //singleton
        private static volatile DocumentiTrasporto_BLL instance;
        private static object objForLock = new Object();
        private DocumentiTrasporto_BLL() { }
        public static DocumentiTrasporto_BLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (objForLock)
                    {
                        if (instance == null)
                            instance = new DocumentiTrasporto_BLL();
                    }
                }
                return instance;
            }
        }

        public DocumentiTrasporto getDocumentoTrasportoById(ref Esito esito, Int64 id)
        {
            DocumentiTrasporto documentiTrasportoREt = DocumentiTrasporto_DAL.Instance.getDocumentoTrasportoById(ref esito,id);

            return documentiTrasportoREt;
        }


        public int CreaDocumentoTrasporto(DocumentiTrasporto documentoTrasporto, ref Esito esito)
        {
            int iREt = DocumentiTrasporto_DAL.Instance.CreaDocumentoTrasporto(documentoTrasporto, ref esito);

            return iREt;
        }

        public Esito AggiornaDocumentoTrasporto(DocumentiTrasporto documentoTrasporto)
        {
            Esito esito = DocumentiTrasporto_DAL.Instance.AggiornaDocumentoTrasporto(documentoTrasporto);

            return esito;
        }

        public Esito EliminaDocumentoTrasporto(int idDocumentoTrasporto)
        {
            Esito esito = DocumentiTrasporto_DAL.Instance.EliminaDocumentoTrasporto(idDocumentoTrasporto);

            return esito;
        }

        public int CreaAttrezzaturaTrasporto(AttrezzatureTrasporto attrezzatura, ref Esito esito)
        {
            int iREt = DocumentiTrasporto_DAL.Instance.CreaAttrezzaturaTrasporto(attrezzatura, ref esito);
            return iREt;
        }

        public List<AttrezzatureTrasporto> getAttrezzatureTrasportoByIdDocumentoTrasporto(ref Esito esito, Int64 idDocumentoTrasporto)
        {
            List<AttrezzatureTrasporto> listaAttrezzatureTrasporto = DocumentiTrasporto_DAL.Instance.getAttrezzatureTrasportoByIdDocumentoTrasporto(ref esito, idDocumentoTrasporto);
            return listaAttrezzatureTrasporto;
        }

        public Esito EliminaAttrezzaturaTrasporto(int idAttrezzaturaTrasporto)
        {
            Esito esito = DocumentiTrasporto_DAL.Instance.EliminaAttrezzaturaTrasporto(idAttrezzaturaTrasporto);

            return esito;
        }

    }
}