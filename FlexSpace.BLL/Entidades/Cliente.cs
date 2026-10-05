using System;
using System.Collections.Generic;
using System.Text;

namespace FlexSpace.BLL.Entidades
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string TipoCliente { get; set; }
        public int SancionesActivas { get; set; }
    }
}