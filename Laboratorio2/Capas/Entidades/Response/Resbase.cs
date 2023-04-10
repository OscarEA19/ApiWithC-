using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capas.Entidades
{
    public class Resbase
    {
        public bool result { set; get; }
        public List<String> listaDeErrores { set; get; }

        public Resbase() { 
            this.listaDeErrores = new List<String>();  
        }
    }
}