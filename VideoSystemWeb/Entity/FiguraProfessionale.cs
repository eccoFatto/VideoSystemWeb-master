﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoSystemWeb.Entity
{
    [Serializable]
    public class FiguraProfessionale
    {
        private int id;
        private string nome;
        private string cognome;
        private string telefono;
        private string citta;
        private List<Anag_Qualifiche_Collaboratori> qualifiche;
        private int tipo;
        private string nota;
       
        public int Id { get => id; set => id = value; }
        public string Nome { get => nome; set => nome = value; }
        public string Cognome { get => cognome; set => cognome = value; }
        public string Telefono { get => telefono; set => telefono = value; }
        public string Citta { get => citta; set => citta = value; }
        public List<Anag_Qualifiche_Collaboratori> Qualifiche { get => qualifiche; set => qualifiche = value; }
        public int Tipo { get => tipo; set => tipo = value; }
        public string Nota { get => nota; set => nota = value; }

    }
}