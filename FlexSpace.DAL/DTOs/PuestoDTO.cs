using System;
using System.Collections.Generic;
using System.Text;

namespace FlexSpace.DAL.DTOs
{
    public class PuestoDTO
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string TipoPuesto { get; set; }
        public decimal TarifaBasePorHora { get; set; }
    }
}
