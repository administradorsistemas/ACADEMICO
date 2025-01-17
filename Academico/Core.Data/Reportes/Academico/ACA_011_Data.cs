﻿using Core.Data.Base;
using Core.Info.Reportes.Academico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Reportes.Academico
{
    public class ACA_011_Data
    {
        public List<ACA_011_Info> get_list(int IdEmpresa, int IdAnio, int IdSede, int IdNivel, int IdJornada, int IdCurso, int IdParalelo)
        {
            try
            {
                List<ACA_011_Info> Lista = new List<ACA_011_Info>();
                using (EntitiesReportes Context = new EntitiesReportes())
                {
                    int IdJornadaFin = IdJornada == 0 ? 99999 : IdJornada;
                    int IdNivelFin = IdNivel == 0 ? 99999 : IdNivel;
                    int IdCursoFin = IdCurso == 0 ? 99999 : IdCurso;
                    int IdParaleloFin = IdParalelo == 0 ? 99999 : IdParalelo;
                    

                    Context.Database.CommandTimeout = 5000;
                    /*var lst = Context.VWACA_011.Where(q => q.IdEmpresa == IdEmpresa && q.IdAnio == IdAnio && q.IdSede == IdSede
                    && IdNivel <= q.IdNivel && q.IdNivel <= IdNivelFin
                    && IdJornada <= q.IdJornada && q.IdJornada <= IdJornadaFin
                    && IdCurso <= q.IdCurso && q.IdCurso <= IdCursoFin
                    && IdParalelo <= q.IdParalelo && q.IdParalelo <= IdParaleloFin).ToList();
                    */
                    var lst = Context.SPACA_011(IdEmpresa, IdAnio, IdSede, IdNivel, IdJornada, IdCurso, IdParalelo).ToList();

                    lst.ForEach(q =>
                    {
                        Lista.Add(new ACA_011_Info
                        {
                            RowNumber = q.RowNumber,
                            IdEmpresa = q.IdEmpresa,
                            IdMatricula = q.IdMatricula,
                            IdAlumno = q.IdAlumno,
                            pe_nombreCompleto = q.pe_nombreCompleto,
                            LugarNacimiento = q.LugarNacimiento,
                            pe_fechaNacimiento = q.pe_fechaNacimiento,
                            Edad = q.Edad,
                            IdPersonaR = q.IdPersonaR,
                            Parentezco = q.Parentezco,
                            Representante = q.Representante,
                            Direccion = q.Direccion,
                            Celular = q.Celular,
                            IdAnio = q.IdAnio,
                            IdSede = q.IdSede,
                            IdNivel = q.IdNivel,
                            IdJornada = q.IdJornada,
                            IdCurso = q.IdCurso,
                            IdParalelo = q.IdParalelo,
                            IdMateria = q.IdMateria,
                            NomMateria = q.NomMateria,
                            OrdenMateria = q.OrdenMateria,
                            IdProfesor = q.IdProfesor,
                            CalificacionP1 = q.CalificacionP1,
                            CalificacionP2 = q.CalificacionP2,
                            CalificacionP3 = q.CalificacionP3,
                            PromedioQ1 = q.PromedioQ1,
                            ExamenQ1 = q.ExamenQ1,
                            PromedioFinalQ1 = q.PromedioFinalQ1,
                            CalificacionP4 = q.CalificacionP4,
                            CalificacionP5 = q.CalificacionP5,
                            CalificacionP6 = q.CalificacionP6,
                            PromedioQ2 = q.PromedioQ2,
                            ExamenQ2 = q.ExamenQ2,
                            PromedioFinalQ2 = q.PromedioFinalQ2,
                            ExamenMejoramiento = q.ExamenMejoramiento,
                            CampoMejoramiento = q.CampoMejoramiento,
                            PromedioQuimestral = q.PromedioQuimestral,
                            ExamenSupletorio = q.ExamenSupletorio,
                            ExamenRemedial = q.ExamenRemedial,
                            ExamenGracia = q.ExamenGracia,
                            PromedioFinal = q.PromedioFinal,
                            FaltaInjustificada=q.FaltaInjustificada,
                            FaltaJustificada=q.FaltaJustificada,
                            CondFinal=q.CondFinal,
                            CondQ1=q.CondQ1,
                            CondQ2=q.CondQ2
                        });
                    });
                }
                return Lista;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
