﻿using Core.Data.Base;
using Core.Info.Reportes.Academico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Reportes.Academico
{
  public  class ACA_008_Resumen_Data
    {
       public List< ACA_008_Resumen_Info> GetList(int IdEmpresa, int IdSede, int IdAnio)
        {
            try
            {
                List<ACA_008_Resumen_Info> Lista = new List<ACA_008_Resumen_Info>();
                int IdSedeIni = IdSede;
                int IdSedeFin = IdSede == 0 ? 9999999 : IdSede;

                using (EntitiesReportes db = new EntitiesReportes())
                {
                    Lista = db.VWACA_008.Where(q => q.IdEmpresa == IdEmpresa && q.IdAnio == IdAnio && IdSedeIni <= q.IdSede && q.IdSede <= IdSedeFin
                   ).Select(q => new ACA_008_Resumen_Info
                    {
                        IdEmpresa = q.IdEmpresa,
                        NomSede = q.NomSede,
                        NomNivel = q.NomNivel,
                        OrdenNivel = q.OrdenNivel,
                        NomJornada = q.NomJornada,
                        OrdenJornada = q.OrdenJornada,
                        OrdenCurso = q.OrdenCurso,
                        NomCurso = q.NomCurso,
                        CodigoParalelo = q.CodigoParalelo,
                        NomParalelo = q.NomParalelo,
                        OrdenParalelo = q.OrdenParalelo,
                        pe_sexo = q.pe_sexo,
                        Cantidad = q.Cantidad,
                        IdMatricula = q.IdMatricula,
                        IdAnio = q.IdAnio,
                        IdSede = q.IdSede,
                        IdNivel = q.IdNivel,
                        IdJornada = q.IdJornada,
                        IdCurso = q.IdCurso,
                        IdParalelo = q.IdParalelo,
                        Fecha = q.Fecha,
                        NomPlantilla = q.NomPlantilla,
                        IdPlantilla = q.IdPlantilla,
                        Descripcion = q.Descripcion,
                        pe_nombreCompleto = q.pe_nombreCompleto,
                        CodigoAlumno = q.CodigoAlumno


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