using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.Entity;

namespace VideoSystemWeb.BLL
{
    public class UtilityLavorazione
    {
        public decimal totalePrezzo;
        public decimal totaleCosto;
        public decimal? totaleLordo;
        public decimal totaleIva;
        public decimal percentualeRicavo;

        public UtilityLavorazione(Entity.DatiAgenda datiAgenda) 
        {
            SetValori(datiAgenda.LavorazioneCorrente);
        }

        public UtilityLavorazione(Entity.DatiLavorazione datiLavorazione)
        {
            SetValori(datiLavorazione);
        }

        private void SetValori(Entity.DatiLavorazione datiLavorazione)
        {
            this.totalePrezzo = 0;
            this.totaleCosto = 0;
            this.totaleLordo = 0;
            this.totaleIva = 0;
            this.percentualeRicavo = 0;

            if (datiLavorazione != null &&
                datiLavorazione.ListaArticoliLavorazione != null &&
                datiLavorazione.ListaArticoliLavorazione.Count > 0)
            {
                foreach (DatiArticoliLavorazione art in datiLavorazione.ListaArticoliLavorazione)
                {
                    if (art.UsaCostoFP != null)
                    {
                        if ((bool)art.UsaCostoFP)
                        {
                            this.totaleCosto += art.FP_netto != null ? (decimal)art.FP_netto : 0;
                        }
                        else
                        {
                            this.totaleCosto += (decimal)art.Costo;
                        }
                    }
                    else
                    {
                        this.totaleCosto += (decimal)art.Costo;
                    }
                    this.totaleLordo += art.FP_lordo != null ? (decimal)art.FP_lordo : 0;
                    this.totalePrezzo += art.Prezzo;
                    this.totaleIva += (art.Prezzo * art.Iva / 100);
                }

                if (this.totalePrezzo != 0)
                {
                    this.percentualeRicavo = ((this.totalePrezzo - (decimal)this.totaleLordo) / this.totalePrezzo) * 100;
                }
            }
        }

        public bool IsMargineInsufficiente
        {
            get { 
                return this.percentualeRicavo <= 50;
            }
        }
    }
}