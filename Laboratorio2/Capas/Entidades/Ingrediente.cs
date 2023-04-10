using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capas.Entidades
{
    public class Ingrediente
    {
        public int id { get; set; }
        public string ingrediente { get; set; }
        public string status { get; set; }
        public int idReceta { get; set; }
    }
}