using MySql.Data.MySqlClient;

namespace FlexSpace.DAL.Conexion
{
    public class ConexionBD
    {
        private string cadenaConexion =
            "Server=localhost;Database=flexspace;Uid=root;Pwd=;";

        public MySqlConnection ObtenerConexion()
        {
            return new MySqlConnection(cadenaConexion);
        }
    }
}