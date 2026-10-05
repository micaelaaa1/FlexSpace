using System;
using System.Collections.Generic;
using System.Text;

namespace FlexSpace.BLL.Entidades
{
    public class Puesto
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string TipoPuesto { get; set; }
        public decimal TarifaBasePorHora { get; set; }
    }
}