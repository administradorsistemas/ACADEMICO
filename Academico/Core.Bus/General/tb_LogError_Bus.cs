﻿using Core.Data.General;
using Core.Info.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Bus.General
{
    public class tb_LogError_Bus
    {
        tb_LogError_Data odata = new tb_LogError_Data();
        public bool GuardarDB(tb_LogError_Info info)
        {
            try
            {
                return odata.GuardarDB(info);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
