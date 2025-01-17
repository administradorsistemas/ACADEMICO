﻿using Core.Data.Reportes.Caja;
using Core.Info.Reportes.Caja;
using System;
using System.Collections.Generic;

namespace Core.Bus.Reportes.Caja
{
    public class CAJ_001_Bus
    {
        CAJ_001_Data odata = new CAJ_001_Data();

        public List<CAJ_001_Info> GetList(int IdEmpresa, int IdTipoCbte, decimal IdCbteCble)
        {
            try
            {
                return odata.GetList(IdEmpresa, IdTipoCbte, IdCbteCble);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
