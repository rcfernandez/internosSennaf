using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace internosSennaf.Models
{
    public class ListInterno
    {
        public string idDependencia { get; set; }
        public string Piso { get; set; }
        public string Sector { get; set; }
        public string Referente { get; set; }
        public IEnumerable<Interno> Internos { get; set; }
    }
}