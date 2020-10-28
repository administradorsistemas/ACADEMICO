﻿using Core.Data.Reportes.Academico;
using Core.Info.Reportes.Academico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Bus.Reportes.Academico
{
    public class ACA_050_Promedios_Bus
    {
        ACA_050_Promedios_Data odata = new ACA_050_Promedios_Data();
        public List<ACA_050_Promedios_Info> GetList(int IdEmpresa, decimal IdAlumno)
        {
            try
            {
                return odata.get_list(IdEmpresa, IdAlumno);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
