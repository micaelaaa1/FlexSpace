using System;
using System.Collections.Generic;
using FlexSpace.BLL.Entidades;
using FlexSpace.BLL.Excepciones;
using FlexSpace.DAL.DTOs;
using FlexSpace.DAL.Repositorios;

namespace FlexSpace.BLL.Servicios
{
    public class ReservaBLL
    {
        private ReservaDAL reservaDAL;
        private ClienteDAL clienteDAL;
        private PuestoDAL puestoDAL;

        public ReservaBLL()
        {
            reservaDAL = new ReservaDAL();
            clienteDAL = new ClienteDAL();
            puestoDAL = new PuestoDAL();
        }

        public Reserva ObtenerPorId(int id)
        {
            ReservaDTO dto = reservaDAL.ObtenerPorId(id);

            if (dto == null)
            {
                return null;
            }

            return ConvertirReserva(dto);
        }

        public List<Reserva> ObtenerPorPuesto(int puestoId)
        {
            List<ReservaDTO> dtos =
                reservaDAL.ObtenerConfirmadasPorPuesto(puestoId);

            List<Reserva> reservas = new List<Reserva>();

            foreach (ReservaDTO dto in dtos)
            {
                reservas.Add(ConvertirReserva(dto));
            }

            return reservas;
        }

        public List<Reserva> ObtenerFuturasPorCodigoPuesto(string codigo)
        {
            PuestoDTO puesto =
                puestoDAL.ObtenerPorCodigo(codigo);

            if (puesto == null)
            {
                throw new Exception(
                    "No existe un puesto con ese código.");
            }

            List<ReservaDTO> dtos =
                reservaDAL.ObtenerConfirmadasPorPuesto(puesto.Id);

            List<Reserva> reservas = new List<Reserva>();

            foreach (ReservaDTO dto in dtos)
            {
                if (dto.FechaInicio >= DateTime.Now)
                {
                    reservas.Add(ConvertirReserva(dto));
                }
            }

            return reservas;
        }

        public Reserva PrepararReserva(
            int clienteId,
            int puestoId,
            DateTime fechaInicio,
            DateTime fechaFin)
        {
            if (fechaInicio >= fechaFin)
            {
                throw new Exception(
                    "La fecha de inicio debe ser anterior a la fecha de fin.");
            }

            if (fechaInicio < DateTime.Now)
            {
                throw new Exception(
                    "No se puede realizar una reserva en el pasado.");
            }

            ClienteDTO cliente =
                clienteDAL.ObtenerPorId(clienteId);

            if (cliente == null)
            {
                throw new Exception(
                    "El cliente indicado no existe.");
            }

            if (cliente.SancionesActivas >= 3)
            {
                throw new ClienteSancionadoException(
                    "El cliente tiene 3 o más sanciones activas y no puede realizar reservas.");
            }

            PuestoDTO puesto =
                puestoDAL.ObtenerPorId(puestoId);

            if (puesto == null)
            {
                throw new Exception(
                    "El puesto indicado no existe.");
            }

            List<ReservaDTO> reservasExistentes =
                reservaDAL.ObtenerConfirmadasPorPuesto(puestoId);

            foreach (ReservaDTO reservaExistente in reservasExistentes)
            {
                bool haySolapamiento =
                    fechaInicio < reservaExistente.FechaFin &&
                    fechaFin > reservaExistente.FechaInicio;

                if (haySolapamiento)
                {
                    throw new Exception(
                        "El puesto no está disponible en el horario seleccionado.");
                }
            }

            decimal costoTotal = CalcularTarifa(
                cliente,
                puesto,
                fechaInicio,
                fechaFin);

            return new Reserva
            {
                ClienteId = clienteId,
                PuestoId = puestoId,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                Estado = "Confirmada",
                CostoTotal = costoTotal
            };
        }

        public void ConfirmarReserva(Reserva reserva)
        {
            ReservaDTO nuevaReserva = new ReservaDTO
            {
                ClienteId = reserva.ClienteId,
                PuestoId = reserva.PuestoId,
                FechaInicio = reserva.FechaInicio,
                FechaFin = reserva.FechaFin,
                Estado = "Confirmada",
                CostoTotal = reserva.CostoTotal
            };

            reservaDAL.Insertar(nuevaReserva);
        }

        public void CancelarReserva(int reservaId)
        {
            ReservaDTO reserva =
                reservaDAL.ObtenerPorId(reservaId);

            if (reserva == null)
            {
                throw new Exception(
                    "La reserva indicada no existe.");
            }

            if (reserva.Estado != "Confirmada")
            {
                throw new Exception(
                    "La reserva no puede ser cancelada.");
            }

            TimeSpan anticipacion =
                reserva.FechaInicio - DateTime.Now;

            if (anticipacion.TotalHours < 2)
            {
                clienteDAL.AumentarSancion(reserva.ClienteId);
            }

            reservaDAL.Cancelar(reservaId);
        }

        private decimal CalcularTarifa(
            ClienteDTO cliente,
            PuestoDTO puesto,
            DateTime fechaInicio,
            DateTime fechaFin)
        {
            decimal horas =
                (decimal)(fechaFin - fechaInicio).TotalHours;

            decimal subtotal =
                horas * puesto.TarifaBasePorHora;

            decimal total = subtotal;

            if (IncluyeFinDeSemana(fechaInicio, fechaFin))
            {
                total = total * 1.15m;
            }

            if (cliente.SancionesActivas > 0)
            {
                total = subtotal * 1.20m;
            }
            else
            {
                if (horas >= 5)
                {
                    total = total * 0.90m;
                }

                if (cliente.TipoCliente == "VIP")
                {
                    total = total * 0.95m;
                }
            }

            return Math.Round(total, 2);
        }

        private bool IncluyeFinDeSemana(
            DateTime fechaInicio,
            DateTime fechaFin)
        {
            DateTime fechaActual = fechaInicio.Date;

            while (fechaActual <= fechaFin.Date)
            {
                if (fechaActual.DayOfWeek == DayOfWeek.Saturday ||
                    fechaActual.DayOfWeek == DayOfWeek.Sunday)
                {
                    return true;
                }

                fechaActual = fechaActual.AddDays(1);
            }

            return false;
        }

        private Reserva ConvertirReserva(ReservaDTO dto)
        {
            return new Reserva
            {
                Id = dto.Id,
                ClienteId = dto.ClienteId,
                PuestoId = dto.PuestoId,
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin,
                Estado = dto.Estado,
                CostoTotal = dto.CostoTotal
            };
        }
    }
}

