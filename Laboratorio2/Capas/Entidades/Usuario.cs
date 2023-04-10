﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capas.Entidades
{
    public class Usuario
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int status { get; set; }
        public int isAdmin { get; set; }
        public string email { get; set; }
    }
}