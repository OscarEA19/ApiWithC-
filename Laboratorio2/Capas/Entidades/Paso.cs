using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capas.Entidades
{
    public class Paso
    {
        public int id { get; set; }
        public string paso { get; set; }
        public string status { get; set; }
        public int idReceta { get; set; }
    }
}