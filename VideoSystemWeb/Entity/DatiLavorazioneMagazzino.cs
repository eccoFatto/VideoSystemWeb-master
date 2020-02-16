using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideoSystemWeb.BLL;

namespace VideoSystemWeb.Entity
{
    //[id] [int] IDENTITY(1,1) NOT NULL,
    //[id_Lavorazione] [int] NOT NULL,
    //[descrizione_Camera][varchar] (100) NULL,
    //[id_Camera] [int] NULL,
    //[nome_Camera] [varchar] (100) NULL,
    //[id_Fibra_Trax] [int] NULL,
    //[nome_Fibra_Trax] [varchar] (100) NULL,
    //[id_Ottica] [int] NULL,
    //[nome_Ottica] [varchar] (100) NULL,
    //[id_Viewfinder] [int] NULL,
    //[nome_Viewfinder] [varchar] (100) NULL,
    //[id_Loop] [int] NULL,
    //[nome_Loop] [varchar] (100) NULL,
    //[id_Mic] [int] NULL,
    //[nome_Mic] [varchar] (100) NULL,
    //[id_Testa] [int] NULL,
    //[nome_Testa] [varchar] (100) NULL,
    //[id_Lensholder] [int] NULL,
    //[nome_Lensholder] [varchar] (100) NULL,
    //[id_Cavalletto] [int] NULL,
    //[nome_Cavalletto] [varchar] (100) NULL,
    //[id_Cavi] [int] NULL,
    //[nome_Cavi] [varchar] (100) NULL,
    //[id_Altro1] [int] NULL,
    //[nome_Altro1] [varchar] (100) NULL,
    //[id_Altro2] [int] NULL,
    //[nome_Altro2] [varchar] (100) NULL,
    //[attivo]
    //[bit] NOT NULL,

    [Serializable]
    public class DatiLavorazioneMagazzino
    {
        private int id;
        private int id_Lavorazione;
        private string descrizione_Camera;
        private int? id_Camera;
        private string nome_Camera;
        private int? id_Fibra_Trax;
        private string nome_Fibra_Trax;
        private int? id_Ottica;
        private string nome_Ottica;
        private int? id_Viewfinder;
        private string nome_Viewfinder;
        private int? id_Loop;
        private string nome_Loop;
        private int? id_Mic;
        private string nome_Mic;
        private int? id_Testa;
        private string nome_Testa;
        private int? id_Lensholder;
        private string nome_Lensholder;
        private int? id_Cavalletto;
        private string nome_Cavalletto;
        private int? id_Cavi;
        private string nome_Cavi;
        private int? id_Altro1;
        private string nome_Altro1;
        private int? id_Altro2;
        private string nome_Altro2;
        private bool attivo;

        public int Id { get => id; set => id = value; }
        public int Id_Lavorazione { get => id_Lavorazione; set => id_Lavorazione = value; }
        public string Descrizione_Camera { get => descrizione_Camera; set => descrizione_Camera = value; }
        public int? Id_Camera { get => id_Camera; set => id_Camera = value; }
        public string Nome_Camera { get => nome_Camera; set => nome_Camera = value; }
        public int? Id_Fibra_Trax { get => id_Fibra_Trax; set => id_Fibra_Trax = value; }
        public string Nome_Fibra_Trax { get => nome_Fibra_Trax; set => nome_Fibra_Trax = value; }
        public int? Id_Ottica { get => id_Ottica; set => id_Ottica = value; }
        public string Nome_Ottica { get => nome_Ottica; set => nome_Ottica = value; }
        public int? Id_Viewfinder { get => id_Viewfinder; set => id_Viewfinder = value; }
        public string Nome_Viewfinder { get => nome_Viewfinder; set => nome_Viewfinder = value; }
        public int? Id_Loop { get => id_Loop; set => id_Loop = value; }
        public string Nome_Loop { get => nome_Loop; set => nome_Loop = value; }
        public int? Id_Mic { get => id_Mic; set => id_Mic = value; }
        public string Nome_Mic { get => nome_Mic; set => nome_Mic = value; }
        public int? Id_Testa { get => id_Testa; set => id_Testa = value; }
        public string Nome_Testa { get => nome_Testa; set => nome_Testa = value; }
        public int? Id_Lensholder { get => id_Lensholder; set => id_Lensholder = value; }
        public string Nome_Lensholder { get => nome_Lensholder; set => nome_Lensholder = value; }
        public int? Id_Cavalletto { get => id_Cavalletto; set => id_Cavalletto = value; }
        public string Nome_Cavalletto { get => nome_Cavalletto; set => nome_Cavalletto = value; }
        public int? Id_Cavi { get => id_Cavi; set => id_Cavi = value; }
        public string Nome_Cavi { get => nome_Cavi; set => nome_Cavi = value; }
        public int? Id_Altro1 { get => id_Altro1; set => id_Altro1 = value; }
        public string Nome_Altro1 { get => nome_Altro1; set => nome_Altro1 = value; }
        public int? Id_Altro2 { get => id_Altro2; set => id_Altro2 = value; }
        public string Nome_Altro2 { get => nome_Altro2; set => nome_Altro2 = value; }
        public bool Attivo { get => attivo; set => attivo = value; }
    }
}