using System;

namespace FlexSpace.BLL.Excepciones
{
    public class ClienteSancionadoException : Exception
    {
        public ClienteSancionadoException(string mensaje)
            : base(mensaje)
        {
        }
    }
}
