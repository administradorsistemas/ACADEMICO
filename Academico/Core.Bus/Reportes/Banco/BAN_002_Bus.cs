﻿using Core.Data.Reportes.Banco;
using Core.Info.Reportes.Banco;
using System;
using System.Collections.Generic;

namespace Core.Bus.Reportes.Banco
{
    public class BAN_002_Bus
    {
        BAN_002_Data odata = new BAN_002_Data();
    
        public List<BAN_002_Info> get_list(int IdEmpresa, int IdTipocbte, decimal IdCbteCble)
        {
            try
            {
                return odata.get_list(IdEmpresa, IdTipocbte, IdCbteCble);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
