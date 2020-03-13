﻿using Core.Data.Base;
using Core.Info.Academico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Academico
{
    public class aca_PlantillaTipo_Data
    {
        public List<aca_PlantillaTipo_Info> getList(int IdEmpresa, bool MostrarAnulados)
        {
            try
            {
                List<aca_PlantillaTipo_Info> Lista = new List<aca_PlantillaTipo_Info>();

                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    Lista = Context.aca_PlantillaTipo.Where(q => q.IdEmpresa == IdEmpresa && q.Estado == (MostrarAnulados == true ? q.Estado : true)).Select(q => new aca_PlantillaTipo_Info
                    {
                        IdEmpresa = q.IdEmpresa,
                        IdTipoPlantilla = q.IdTipoPlantilla,
                        NomPlantillaTipo = q.NomPlantillaTipo,
                        Estado = q.Estado
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