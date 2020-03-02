﻿using Core.Data.Reportes.Caja;
using Core.Info.Reportes.Caja;
using System;
using System.Collections.Generic;

namespace Core.Bus.Reportes.Caja
{
    public class CAJ_002_ingresos_Bus
    {
        CAJ_002_ingresos_Data odata = new CAJ_002_ingresos_Data();

        public List<CAJ_002_ingresos_Info> get_list(int IdEmpresa, decimal IdConciliacionCaja)
        {
            try
            {
                return odata.get_list(IdEmpresa, IdConciliacionCaja);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}