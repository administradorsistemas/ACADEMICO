﻿using Core.Data.CuentasPorPagar;
using Core.Info.CuentasPorPagar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Bus.CuentasPorPagar
{
    public class cp_SolicitudPagoDet_Bus
    {
        cp_SolicitudPagoDet_Data odata = new cp_SolicitudPagoDet_Data();


        public List<cp_SolicitudPagoDet_Info> GetListPorPagar(int IdEmpresa, int IdSucursal)
        {
            try
            {
                return odata.GetListPorPagar(IdEmpresa, IdSucursal);
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
