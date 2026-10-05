using System;
using MySql.Data.MySqlClient;
using FlexSpace.DAL.Conexion;
using FlexSpace.DAL.DTOs;

namespace FlexSpace.DAL.Repositorios
{
    public class PuestoDAL
    {
        private ConexionBD conexion = new ConexionBD();

        public PuestoDTO ObtenerPorId(int id)
        {
            PuestoDTO puesto = null;

            using (MySqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"SELECT Id, Codigo, TipoPuesto, TarifaBasePorHora
                               FROM puestos
                               WHERE Id = @Id";

                using (MySqlCommand cmd = new MySqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            puesto = new PuestoDTO
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Codigo = reader["Codigo"].ToString(),
                                TipoPuesto = reader["TipoPuesto"].ToString(),
                                TarifaBasePorHora =
                                    Convert.ToDecimal(reader["TarifaBasePorHora"])
                            };
                        }
                    }
                }
            }

            return puesto;
        }

        public PuestoDTO ObtenerPorCodigo(string codigo)
        {
            PuestoDTO puesto = null;

            using (MySqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"SELECT Id, Codigo, TipoPuesto, TarifaBasePorHora
                               FROM puestos
                               WHERE Codigo = @Codigo";

                using (MySqlCommand cmd = new MySqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Codigo", codigo);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            puesto = new PuestoDTO
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Codigo = reader["Codigo"].ToString(),
                                TipoPuesto = reader["TipoPuesto"].ToString(),
                                TarifaBasePorHora =
                                    Convert.ToDecimal(reader["TarifaBasePorHora"])
                            };
                        }
                    }
                }
            }

            return puesto;
        }
    }
}
