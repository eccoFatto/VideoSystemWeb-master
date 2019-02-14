using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VideoSystemWeb.Agenda.userControl
{
    public partial class Offerta : System.Web.UI.UserControl
    {
        public delegate void PopupHandler(string operazionePopup); // delegato per l'evento
        public event PopupHandler RichiediOperazionePopup; //evento

        List<ProvaElementiOfferta> listaGenerale = new List<ProvaElementiOfferta>();
        List<ProvaElementiOfferta> listaSelezione = new List<ProvaElementiOfferta>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                caricaListaGenerale();

                gvElencoOfferta.DataSource = listaGenerale;
                gvElencoOfferta.DataBind();
            }

            //if (ViewState["listaSelezione"] != null)
            //{
            //    listaSelezione = (List<ProvaElementiOfferta>)ViewState["listaSelezione"];

            //    gvSelezioneOfferta.DataSource = listaSelezione;
            //    gvSelezioneOfferta.DataBind();
            //}
        }

        private void caricaListaGenerale()
        {
            listaGenerale.Add(new ProvaElementiOfferta(1, "19 20 21", "Personale tecnico monocamera", "", 1, 100, 40));
            listaGenerale.Add(new ProvaElementiOfferta(2, "22 23 24", "Personale tecnico bicamera", "", 1, 200, 70));
            listaGenerale.Add(new ProvaElementiOfferta(3, "25 26 27", "Personale tecnico tricamera", "", 2, 300, 100));
            listaGenerale.Add(new ProvaElementiOfferta(4, "Acquisti", "Costi acquisti", "Acquisti", 1, 80, 70));
        }

        private void aggiungiAListaSelezione(int idElemento)
        {
            ProvaElementiOfferta elemento = listaGenerale.Where(x => x.ID == idElemento).FirstOrDefault();

            listaSelezione.Add(elemento);
        }

        #region COMPORTAMENTO ELEMENTI PAGINA
        protected void btnSalva_Click(object sender, EventArgs e)
        {
            
        }

        protected void btnAnnulla_Click(object sender, EventArgs e)
        {
            
        }

        protected void gvElencoOfferta_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idSelezione = Convert.ToInt32(e.CommandArgument);

            caricaListaGenerale();
            listaSelezione = (List<ProvaElementiOfferta>)ViewState["listaSelezione"];
            if (listaSelezione == null)
            {
                listaSelezione = new List<ProvaElementiOfferta>();
            }
            aggiungiAListaSelezione(idSelezione);

            ViewState["listaSelezione"] = listaSelezione;
           

            gvSelezioneOfferta.DataSource = listaSelezione;
            gvSelezioneOfferta.DataBind();

            RichiediOperazionePopup("UPDATE");
        }
        #endregion
    }

    [Serializable]
    public class ProvaElementiOfferta
    {
        public int ID { get; set; }
        public string Codice { get; set; }
        public string Descrizione { get; set; }
        public string Misura { get; set; }
        public int Quantita { get; set; }
        public decimal Prezzo { get; set; }
        public decimal Costo { get; set; }

        public ProvaElementiOfferta() { }

        public ProvaElementiOfferta(int id, string codice, string descrizione, string misura, int quantita, decimal prezzo, decimal costo)
        {
            this.ID = id;
            this.Codice = codice;
            this.Descrizione = descrizione;
            this.Misura = misura;
            this.Quantita = quantita;
            this.Prezzo = prezzo;
            this.Costo = costo;
        }
    }
}