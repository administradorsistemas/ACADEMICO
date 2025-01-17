﻿using Core.Data.Base;
using Core.Info.CuentasPorCobrar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.CuentasPorCobrar
{
    public class cxc_LiquidacionTarjeta_x_ba_TipoFlujo_Data
    {
        public List<cxc_LiquidacionTarjeta_x_ba_TipoFlujo_Info> GetList(int IdEmpresa, int IdSucursal, decimal IdLiquidacion)
        {
            try
            {
                List<cxc_LiquidacionTarjeta_x_ba_TipoFlujo_Info> Lista;

                using (EntitiesCuentasPorCobrar db = new EntitiesCuentasPorCobrar())
                {
                    Lista = db.vwcxc_LiquidacionTarjeta_x_ba_TipoFlujo.Where(q => q.IdEmpresa == IdEmpresa
                    && q.IdSucursal == IdSucursal && q.IdLiquidacion == IdLiquidacion).Select(q => new cxc_LiquidacionTarjeta_x_ba_TipoFlujo_Info
                    {
                        IdEmpresa = q.IdEmpresa,
                        IdSucursal = q.IdSucursal,
                        IdLiquidacion = q.IdLiquidacion,
                        Secuencia = q.Secuencia,
                        IdTipoFlujo = q.IdTipoFlujo,
                        Descripcion = q.Descricion,
                        Porcentaje = q.Porcentaje,
                        Valor = q.Valor
                    }).ToList();
                }

                return Lista;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
