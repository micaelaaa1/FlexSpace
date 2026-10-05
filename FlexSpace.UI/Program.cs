using System;
using System.Collections.Generic;
using FlexSpace.BLL.Entidades;
using FlexSpace.BLL.Servicios;

namespace FlexSpace.UI
{
    class Program
    {
        static ClienteBLL clienteBLL = new ClienteBLL();
        static PuestoBLL puestoBLL = new PuestoBLL();
        static ReservaBLL reservaBLL = new ReservaBLL();

        static void Main(string[] args)
        {
            int opcion;

            do
            {
                MostrarMenu();

                Console.Write("Seleccione una opción: ");

                if (!int.TryParse(Console.ReadLine(), out opcion))
                {
                    Console.WriteLine("Opción inválida.");
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }

                Console.Clear();

                try
                {
                    switch (opcion)
                    {
                        case 1:
                            RegistrarReserva();
                            break;

                        case 2:
                            CancelarReserva();
                            break;

                        case 3:
                            ConsultarReservasPorPuesto();
                            break;

                        case 4:
                            ListarClientesSancionados();
                            break;

                        case 0:
                            Console.WriteLine("Saliendo del sistema...");
                            break;

                        default:
                            Console.WriteLine("Opción inválida.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    Console.WriteLine("ERROR: " + ex.Message);
                }

                if (opcion != 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("Presione una tecla para continuar...");
                    Console.ReadKey();
                    Console.Clear();
                }

            } while (opcion != 0);
        }

        static void MostrarMenu()
        {
            Console.WriteLine("========================================");
            Console.WriteLine("          SISTEMA FLEXSPACE");
            Console.WriteLine("========================================");
            Console.WriteLine("1. Registrar Nueva Reserva");
            Console.WriteLine("2. Cancelar Reserva");
            Console.WriteLine("3. Consultar Reservas Activas por Puesto");
            Console.WriteLine("4. Listar Clientes Sancionados");
            Console.WriteLine("0. Salir");
            Console.WriteLine("========================================");
        }

        static void RegistrarReserva()
        {
            Console.WriteLine("=== REGISTRAR NUEVA RESERVA ===");
            Console.WriteLine();

            Console.Write("Cliente ID: ");
            int clienteId = int.Parse(Console.ReadLine());

            Console.Write("Puesto ID: ");
            int puestoId = int.Parse(Console.ReadLine());

            Console.Write("Fecha y hora de inicio (dd/MM/yyyy HH:mm): ");
            DateTime fechaInicio =
                DateTime.Parse(Console.ReadLine());

            Console.Write("Fecha y hora de fin (dd/MM/yyyy HH:mm): ");
            DateTime fechaFin =
                DateTime.Parse(Console.ReadLine());

            Reserva reserva = reservaBLL.PrepararReserva(
                clienteId,
                puestoId,
                fechaInicio,
                fechaFin);

            Console.WriteLine();
            Console.WriteLine("========================================");
            Console.WriteLine("        RESUMEN DE LA RESERVA");
            Console.WriteLine("========================================");
            Console.WriteLine("Cliente ID: " + reserva.ClienteId);
            Console.WriteLine("Puesto ID: " + reserva.PuestoId);
            Console.WriteLine("Inicio: " + reserva.FechaInicio);
            Console.WriteLine("Fin: " + reserva.FechaFin);
            Console.WriteLine("Costo total: $" + reserva.CostoTotal);
            Console.WriteLine("========================================");

            Console.WriteLine();
            Console.Write("¿Desea confirmar la reserva? (S/N): ");
            string respuesta = Console.ReadLine();

            if (respuesta.ToUpper() == "S")
            {
                reservaBLL.ConfirmarReserva(reserva);

                Console.WriteLine();
                Console.WriteLine("Reserva registrada correctamente.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Reserva cancelada. No se guardó en la base de datos.");
            }
        }

        static void CancelarReserva()
        {
            Console.WriteLine("=== CANCELAR RESERVA ===");
            Console.WriteLine();

            Console.Write("Ingrese el ID de la reserva: ");
            int reservaId = int.Parse(Console.ReadLine());

            Reserva reserva = reservaBLL.ObtenerPorId(reservaId);

            if (reserva == null)
            {
                Console.WriteLine("La reserva no existe.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Reserva encontrada:");
            Console.WriteLine("ID: " + reserva.Id);
            Console.WriteLine("Cliente ID: " + reserva.ClienteId);
            Console.WriteLine("Puesto ID: " + reserva.PuestoId);
            Console.WriteLine("Inicio: " + reserva.FechaInicio);
            Console.WriteLine("Fin: " + reserva.FechaFin);
            Console.WriteLine("Estado: " + reserva.Estado);
            Console.WriteLine("Costo: $" + reserva.CostoTotal);

            Console.WriteLine();
            Console.Write("¿Confirma la cancelación? (S/N): ");
            string respuesta = Console.ReadLine();

            if (respuesta.ToUpper() != "S")
            {
                Console.WriteLine("Cancelación anulada.");
                return;
            }

            reservaBLL.CancelarReserva(reservaId);

            Console.WriteLine();
            Console.WriteLine("Reserva cancelada correctamente.");
        }

        static void ConsultarReservasPorPuesto()
        {
            Console.WriteLine("=== RESERVAS ACTIVAS POR PUESTO ===");
            Console.WriteLine();

            Console.Write("Ingrese el código del puesto: ");
            string codigo = Console.ReadLine();

            List<Reserva> reservas =
            reservaBLL.ObtenerFuturasPorCodigoPuesto(codigo);


            if (reservas.Count == 0)
            {
                Console.WriteLine(
                    "No hay reservas futuras para este puesto.");

                return;
            }

            foreach (Reserva reserva in reservas)
            {
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("Reserva ID: " + reserva.Id);
                Console.WriteLine("Cliente ID: " + reserva.ClienteId);
                Console.WriteLine("Puesto ID: " + reserva.PuestoId);
                Console.WriteLine("Inicio: " + reserva.FechaInicio);
                Console.WriteLine("Fin: " + reserva.FechaFin);
                Console.WriteLine("Estado: " + reserva.Estado);
                Console.WriteLine("Costo: $" + reserva.CostoTotal);
            }
        }

        static void ListarClientesSancionados()
        {
            Console.WriteLine("=== CLIENTES SANCIONADOS ===");
            Console.WriteLine();

            List<Cliente> clientes =
                clienteBLL.ObtenerSancionados();

            if (clientes.Count == 0)
            {
                Console.WriteLine("No hay clientes sancionados.");
                return;
            }

            foreach (Cliente cliente in clientes)
            {
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("ID: " + cliente.Id);
                Console.WriteLine("Nombre: " + cliente.Nombre);
                Console.WriteLine("Email: " + cliente.Email);
                Console.WriteLine("Tipo: " + cliente.TipoCliente);
                Console.WriteLine(
                    "Sanciones: " + cliente.SancionesActivas);
            }
        }
    }
}
