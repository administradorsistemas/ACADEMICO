﻿using Core.Data.Base;
using Core.Info.Reportes.Academico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Reportes.Academico
{
    public class ACA_002_Data
    {
        public List<ACA_002_Info> get_list(int IdEmpresa, decimal IdAlumno, int IdAnio)
        {
            try
            {
                List<ACA_002_Info> Lista;
                using (EntitiesReportes Context = new EntitiesReportes())
                {
                    Lista = Context.VWACA_002.Where(q => q.IdEmpresa == IdEmpresa && q.IdAlumno == IdAlumno && q.IdAnio == IdAnio).Select(q => new ACA_002_Info
                    {
                        IdEmpresa = q.IdEmpresa,
                        IdMatricula = q.IdMatricula,
                        IdAnio = q.IdAnio,
                        IdAlumno = q.IdAlumno,
                        CodigoAlumno = q.CodigoAlumno,
                        NombreAlumno = q.NombreAlumno,
                        Descripcion = q.Descripcion,
                        NomSede = q.NomSede,
                        NomNivel = q.NomNivel,
                        NomJornada = q.NomJornada,
                        NomCurso = q.NomCurso,
                        NomParalelo = q.NomParalelo,
                        NombreRep = q.NombreRep,
                        CedulaRep = q.CedulaRep,
                        NomPlantilla = q.NomPlantilla

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