﻿using Core.Data.Reportes.Academico;
using Core.Info.Reportes.Academico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Bus.Reportes.Academico
{
    public class ACA_013_Asistencia_Bus
    {
        ACA_013_Asistencia_Data odata = new ACA_013_Asistencia_Data();
        public List<ACA_013_Asistencia_Info> GetList(int IdEmpresa, decimal IdMatricula, int IdCatalogoParcial)
        {
            try
            {
                return odata.get_list(IdEmpresa, IdMatricula, IdCatalogoParcial);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
