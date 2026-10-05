using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using FlexSpace.DAL.Conexion;
using FlexSpace.DAL.DTOs;

namespace FlexSpace.DAL.Repositorios
{
    public class ReservaDAL
    {
        private ConexionBD conexion = new ConexionBD();

        public void Insertar(ReservaDTO reserva)
        {
            using (MySqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"INSERT INTO reservas
                               (ClienteId, PuestoId, FechaInicio, FechaFin, Estado, CostoTotal)
                               VALUES
                               (@ClienteId, @PuestoId, @FechaInicio, @FechaFin, @Estado, @CostoTotal)";

                using (MySqlCommand cmd = new MySqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@ClienteId", reserva.ClienteId);
                    cmd.Parameters.AddWithValue("@PuestoId", reserva.PuestoId);
                    cmd.Parameters.AddWithValue("@FechaInicio", reserva.FechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", reserva.FechaFin);
                    cmd.Parameters.AddWithValue("@Estado", reserva.Estado);
                    cmd.Parameters.AddWithValue("@CostoTotal", reserva.CostoTotal);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public ReservaDTO ObtenerPorId(int id)
        {
            ReservaDTO reserva = null;

            using (MySqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"SELECT Id, ClienteId, PuestoId,
                                      FechaInicio, FechaFin,
                                      Estado, CostoTotal
                               FROM reservas
                               WHERE Id = @Id";

                using (MySqlCommand cmd = new MySqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            reserva = new ReservaDTO
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                ClienteId = Convert.ToInt32(reader["ClienteId"]),
                                PuestoId = Convert.ToInt32(reader["PuestoId"]),
                                FechaInicio = Convert.ToDateTime(reader["FechaInicio"]),
                                FechaFin = Convert.ToDateTime(reader["FechaFin"]),
                                Estado = reader["Estado"].ToString(),
                                CostoTotal = Convert.ToDecimal(reader["CostoTotal"])
                            };
                        }
                    }
                }
            }

            return reserva;
        }
        public List<ReservaDTO> ObtenerConfirmadasPorPuesto(int puestoId)
        {
            List<ReservaDTO> reservas = new List<ReservaDTO>();

            using (MySqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"SELECT Id, ClienteId, PuestoId,
                                  FechaInicio, FechaFin,
                                  Estado, CostoTotal
                           FROM reservas
                           WHERE PuestoId = @PuestoId
                           AND Estado = 'Confirmada'";

                using (MySqlCommand cmd = new MySqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@PuestoId", puestoId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reservas.Add(new ReservaDTO
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                ClienteId = Convert.ToInt32(reader["ClienteId"]),
                                PuestoId = Convert.ToInt32(reader["PuestoId"]),
                                FechaInicio = Convert.ToDateTime(reader["FechaInicio"]),
                                FechaFin = Convert.ToDateTime(reader["FechaFin"]),
                                Estado = reader["Estado"].ToString(),
                                CostoTotal = Convert.ToDecimal(reader["CostoTotal"])
                            });
                        }
                    }
                }
            }

            return reservas;
        }

        public void Cancelar(int reservaId)
        {
            using (MySqlConnection cn = conexion.ObtenerConexion())
            {
                cn.Open();

                string sql = @"UPDATE reservas
                               SET Estado = 'Cancelada'
                               WHERE Id = @Id";

                using (MySqlCommand cmd = new MySqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Id", reservaId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
