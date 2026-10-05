using System;
using System.Collections.Generic;
using System.Text;

namespace FlexSpace.DAL.DTOs
{
    public class ClienteDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string TipoCliente { get; set; }
        public int SancionesActivas { get; set; }
    }
}

