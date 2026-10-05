using System.Collections.Generic;
using FlexSpace.BLL.Entidades;
using FlexSpace.DAL.DTOs;
using FlexSpace.DAL.Repositorios;

namespace FlexSpace.BLL.Servicios
{
    public class ClienteBLL
    {
        private ClienteDAL clienteDAL;

        public ClienteBLL()
        {
            clienteDAL = new ClienteDAL();
        }

        public Cliente ObtenerPorId(int id)
        {
            ClienteDTO dto = clienteDAL.ObtenerPorId(id);

            if (dto == null)
            {
                return null;
            }

            return new Cliente
            {
                Id = dto.Id,
                Nombre = dto.Nombre,
                Email = dto.Email,
                TipoCliente = dto.TipoCliente,
                SancionesActivas = dto.SancionesActivas
            };
        }

        public List<Cliente> ObtenerSancionados()
        {
            List<ClienteDTO> dtos = clienteDAL.ObtenerSancionados();

            List<Cliente> clientes = new List<Cliente>();

            foreach (ClienteDTO dto in dtos)
            {
                clientes.Add(new Cliente
                {
                    Id = dto.Id,
                    Nombre = dto.Nombre,
                    Email = dto.Email,
                    TipoCliente = dto.TipoCliente,
                    SancionesActivas = dto.SancionesActivas
                });
            }

            return clientes;
        }
    }
}
