using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using FlexSpace.DAL.DTOs;
using FlexSpace.DAL.Conexion;

namespace FlexSpace.DAL.Repositorios
{
    public class ClienteDAL
    {
        private ConexionBD conexion = new ConexionBD();

        public ClienteDTO ObtenerPorId(int id)
        {
            ClienteDTO cliente = null;

            using (MySqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"SELECT Id, Nombre, Email, TipoCliente, SancionesActivas
                               FROM clientes
                               WHERE Id = @Id";

                using (MySqlCommand cmd = new MySqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cliente = new ClienteDTO
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nombre = reader["Nombre"].ToString(),
                                Email = reader["Email"].ToString(),
                                TipoCliente = reader["TipoCliente"].ToString(),
                                SancionesActivas = Convert.ToInt32(reader["SancionesActivas"])
                            };
                        }
                    }
                }
            }

            return cliente;
        }

        public List<ClienteDTO> ObtenerSancionados()
        {
            List<ClienteDTO> clientes = new List<ClienteDTO>();

            using (MySqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"SELECT Id, Nombre, Email, TipoCliente, SancionesActivas
                               FROM clientes
                               WHERE SancionesActivas > 0";

                using (MySqlCommand cmd = new MySqlCommand(sql, cn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        clientes.Add(new ClienteDTO
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nombre = reader["Nombre"].ToString(),
                            Email = reader["Email"].ToString(),
                            TipoCliente = reader["TipoCliente"].ToString(),
                            SancionesActivas = Convert.ToInt32(reader["SancionesActivas"])
                        });
                    }
                }
            }

            return clientes;
        }

        public void AumentarSancion(int clienteId)
        {
            using (MySqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"UPDATE clientes
                               SET SancionesActivas = SancionesActivas + 1
                               WHERE Id = @Id";

                using (MySqlCommand cmd = new MySqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Id", clienteId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}