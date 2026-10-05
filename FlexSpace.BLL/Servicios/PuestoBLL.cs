using System;
using System.Collections.Generic;
using FlexSpace.BLL.Entidades;
using FlexSpace.DAL.DTOs;
using FlexSpace.DAL.Repositorios;

namespace FlexSpace.BLL.Servicios
{
    public class PuestoBLL
    {
        private PuestoDAL puestoDAL;

        public PuestoBLL()
        {
            puestoDAL = new PuestoDAL();
        }

        public Puesto ObtenerPorId(int id)
        {
            PuestoDTO dto = puestoDAL.ObtenerPorId(id);

            if (dto == null)
            {
                return null;
            }

            return new Puesto
            {
                Id = dto.Id,
                Codigo = dto.Codigo,
                TipoPuesto = dto.TipoPuesto,
                TarifaBasePorHora = dto.TarifaBasePorHora
            };
        }

        public Puesto ObtenerPorCodigo(string codigo)
        {
            PuestoDTO dto = puestoDAL.ObtenerPorCodigo(codigo);

            if (dto == null)
            {
                return null;
            }

            return new Puesto
            {
                Id = dto.Id,
                Codigo = dto.Codigo,
                TipoPuesto = dto.TipoPuesto,
                TarifaBasePorHora = dto.TarifaBasePorHora
            };
        }
    }
}
