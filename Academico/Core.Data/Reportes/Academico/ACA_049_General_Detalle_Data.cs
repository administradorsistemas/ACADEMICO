using Core.Data.Academico;
using Core.Data.Base;
using Core.Info.Helps;
using Core.Info.Reportes.Academico;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Core.Data.Reportes.Academico
{
    public class ACA_049_General_Detalle_Data
    {
        cl_funciones funciones = new cl_funciones();
        aca_AnioLectivoConductaEquivalencia_Data odata_conducta_equiv = new aca_AnioLectivoConductaEquivalencia_Data();
        aca_AnioLectivoCalificacionCualitativa_Data odata_proyectos_equiv = new aca_AnioLectivoCalificacionCualitativa_Data();
        public List<ACA_049_General_Detalle_Info> GetList(int IdEmpresa, int IdAnio, decimal IdMatricula)
        {
            try
            {
                List<ACA_049_General_Detalle_Info> Lista = new List<ACA_049_General_Detalle_Info>();
                List<ACA_049_General_Detalle_Info> ListaFinal = new List<ACA_049_General_Detalle_Info>();
                List<ACA_049_General_Detalle_Info> Lista_Comportamiento = new List<ACA_049_General_Detalle_Info>();
                List<ACA_049_General_Detalle_Info> Lista_Proyectos = new List<ACA_049_General_Detalle_Info>();
                List<ACA_049_General_Detalle_Info> ListaObligatorias = new List<ACA_049_General_Detalle_Info>();
                List<ACA_049_General_Detalle_Info> ListaComplementarias = new List<ACA_049_General_Detalle_Info>();
                List<ACA_049_General_Detalle_Info> ListaPromediadaComplementarias = new List<ACA_049_General_Detalle_Info>();

                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();

                    #region Query
                    string query = "DECLARE @IdEmpresa int = " + IdEmpresa.ToString() + ", @IdAnio int = " + IdAnio.ToString() + ", @IdMatricula decimal = " + IdMatricula.ToString()
                    + " /*COMPORTAMIENTO*/ "
                    + " SELECT m.IdEmpresa, m.IdMatricula, m.IdAnio, m.IdSede, m.IdNivel, m.IdJornada, m.IdCurso, m.IdParalelo, m.IdAlumno, AN.Descripcion, sn.NomSede, nj.NomJornada, nj.OrdenJornada, sn.NomNivel, sn.OrdenNivel, jc.NomCurso, jc.OrdenCurso,  "
                    + " cp.NomParalelo, cp.OrdenParalelo,alu.Codigo, pa.pe_nombreCompleto AS NombreAlumno, 0 AS IdMateria, NULL AS NombreMateria, '' NomMateriaArea, 'COMPORTAMIENTO' AS NombreGrupo, 99999 AS OrdenMateria, 99999 AS OrdenGrupo, 0 AS PromediarGrupo, NULL "
                    + " AS IdCatalogoTipoCalificacion, CAST(equiv.Letra AS varchar) AS Calificacion, CAST(equiv.Calificacion AS numeric(18, 2)) AS CalificacionNumerica, 'EVALUACION DEL COMPORTAMIENTO' AS Columna, 1 AS OrdenColumna, "
                    + " pp.pe_nombreCompleto AS NombreTutor "
                    + " FROM     dbo.aca_Matricula AS m WITH (nolock) INNER JOIN "
                    + " dbo.aca_MatriculaConducta AS mco WITH (nolock) ON m.IdEmpresa = mco.IdEmpresa AND m.IdMatricula = mco.IdMatricula LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivoConductaEquivalencia AS equiv WITH (nolock) ON mco.IdEmpresa = equiv.IdEmpresa AND mco.SecuenciaPromedioFinal = equiv.Secuencia AND m.IdAnio = equiv.IdAnio LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo AS AN WITH (nolock) ON m.IdEmpresa = AN.IdEmpresa AND m.IdAnio = AN.IdAnio LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Sede_NivelAcademico AS sn WITH (nolock) ON sn.IdEmpresa = m.IdEmpresa AND sn.IdSede = m.IdSede AND sn.IdAnio = m.IdAnio AND sn.IdNivel = m.IdNivel LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_NivelAcademico_Jornada AS nj WITH (nolock) ON nj.IdEmpresa = m.IdEmpresa AND nj.IdAnio = m.IdAnio AND nj.IdSede = m.IdSede AND nj.IdNivel = m.IdNivel AND nj.IdJornada = m.IdJornada LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Jornada_Curso AS jc WITH (nolock) ON jc.IdEmpresa = m.IdEmpresa AND jc.IdAnio = m.IdAnio AND jc.IdSede = m.IdSede AND jc.IdNivel = m.IdNivel AND jc.IdJornada = m.IdJornada AND jc.IdCurso = m.IdCurso LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Paralelo AS cp WITH (nolock) ON cp.IdEmpresa = m.IdEmpresa AND cp.IdAnio = m.IdAnio AND cp.IdSede = m.IdSede AND cp.IdNivel = m.IdNivel AND cp.IdJornada = m.IdJornada AND cp.IdCurso = m.IdCurso AND "
                    + " cp.IdParalelo = m.IdParalelo LEFT OUTER JOIN "
                    + " dbo.aca_Alumno AS alu WITH (nolock) ON m.IdEmpresa = alu.IdEmpresa AND m.IdAlumno = alu.IdAlumno LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pa WITH (nolock) ON alu.IdPersona = pa.IdPersona LEFT OUTER JOIN "
                    + " dbo.aca_Profesor AS pro WITH (nolock) ON pro.IdEmpresa = m.IdEmpresa AND pro.IdProfesor = cp.IdProfesorTutor LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pp WITH (nolock) ON pro.IdPersona = pp.IdPersona LEFT OUTER JOIN "
                    + " (SELECT IdEmpresa, IdMatricula "
                    + " FROM      dbo.aca_AlumnoRetiro AS r WITH (nolock) "
                    + " WHERE(Estado = 1)) AS ret ON m.IdEmpresa = ret.IdEmpresa AND m.IdMatricula = ret.IdMatricula "
                    + " where mco.IdEmpresa = @IdEmpresa "
                    + " and m.IdAnio = @IdAnio "
                    + " and m.IdMatricula = @IdMatricula "
                    + " UNION ALL "
                    + " ( "
                    + " /*PROYECTOS*/ "
                    + " SELECT m.IdEmpresa, m.IdMatricula, m.IdAnio, m.IdSede, m.IdNivel, m.IdJornada, m.IdCurso, m.IdParalelo, m.IdAlumno, AN.Descripcion, sn.NomSede, nj.NomJornada, nj.OrdenJornada, sn.NomNivel, sn.OrdenNivel, jc.NomCurso, jc.OrdenCurso, "
                    + " cp.NomParalelo, cp.OrdenParalelo,alu.Codigo, pa.pe_nombreCompleto AS NombreAlumno, 0 AS IdMateria, NULL AS NombreMateria, '' NomMateriaArea, 'PROYECTOS' AS NombreGrupo, 999999 AS OrdenMateria, 999999 AS OrdenGrupo, 0 AS PromediarGrupo, "
                    //+ " mc.IdCatalogoTipoCalificacion AS IdCatalogoTipoCalificacion, CAST(lp.Codigo AS varchar) AS Calificacion, CAST(ep.Calificacion AS numeric(18, 2)) AS CalificacionNumerica, 'EVALUACION DE PROYECTOS ESCOLARES' AS Columna, "
                    + " mc.IdCatalogoTipoCalificacion AS IdCatalogoTipoCalificacion, CAST(lp.Codigo AS varchar) AS Calificacion, CAST(ep.Calificacion AS numeric(18, 2)) AS CalificacionNumerica, isnull(mc.NomMateriaArea,'') AS Columna, "
                    + " 1 AS OrdenColumna, pp.pe_nombreCompleto AS NombreTutor "
                    + " FROM     dbo.aca_Matricula AS m WITH (nolock) LEFT OUTER JOIN "
                    + " dbo.aca_MatriculaCalificacionCualitativaPromedio AS mp WITH (nolock) ON mp.IdEmpresa = m.IdEmpresa AND mp.IdMatricula = m.IdMatricula LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivoCalificacionCualitativa AS ep WITH (nolock) ON ep.IdAnio = m.IdAnio AND ep.IdEmpresa = m.IdEmpresa AND ep.IdCalificacionCualitativa = mp.IdCalificacionCualitativaFinal LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo AS AN WITH (nolock) ON m.IdEmpresa = AN.IdEmpresa AND m.IdAnio = AN.IdAnio LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Sede_NivelAcademico AS sn WITH (nolock) ON sn.IdEmpresa = m.IdEmpresa AND sn.IdSede = m.IdSede AND sn.IdAnio = m.IdAnio AND sn.IdNivel = m.IdNivel LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_NivelAcademico_Jornada AS nj WITH (nolock) ON nj.IdEmpresa = m.IdEmpresa AND nj.IdAnio = m.IdAnio AND nj.IdSede = m.IdSede AND nj.IdNivel = m.IdNivel AND nj.IdJornada = m.IdJornada LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Jornada_Curso AS jc WITH (nolock) ON jc.IdEmpresa = m.IdEmpresa AND jc.IdAnio = m.IdAnio AND jc.IdSede = m.IdSede AND jc.IdNivel = m.IdNivel AND jc.IdJornada = m.IdJornada AND jc.IdCurso = m.IdCurso LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Paralelo AS cp WITH (nolock) ON cp.IdEmpresa = m.IdEmpresa AND cp.IdAnio = m.IdAnio AND cp.IdSede = m.IdSede AND cp.IdNivel = m.IdNivel AND cp.IdJornada = m.IdJornada AND cp.IdCurso = m.IdCurso AND "
                    + " cp.IdParalelo = m.IdParalelo LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Materia AS mc WITH (nolock) ON mc.IdEmpresa = m.IdEmpresa AND mc.IdAnio = m.IdAnio AND mc.IdSede = m.IdSede AND mc.IdNivel = m.IdNivel AND mc.IdJornada = m.IdJornada AND mc.IdCurso = m.IdCurso AND mc.IdMateria = mp.IdMateria LEFT OUTER JOIN "
                    + " dbo.aca_Alumno AS alu WITH (nolock) ON m.IdEmpresa = alu.IdEmpresa AND m.IdAlumno = alu.IdAlumno LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pa WITH (nolock) ON alu.IdPersona = pa.IdPersona LEFT OUTER JOIN "
                    + " dbo.aca_Profesor AS pro WITH (nolock) ON pro.IdEmpresa = m.IdEmpresa AND pro.IdProfesor = cp.IdProfesorTutor LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pp WITH (nolock) ON pro.IdPersona = pp.IdPersona LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivoCalificacionCualitativa AS lp WITH (nolock) ON lp.IdEmpresa = m.IdEmpresa AND lp.IdAnio = m.IdAnio AND lp.IdCalificacionCualitativa = mp.IdCalificacionCualitativaFinal LEFT OUTER JOIN "
                    + " (SELECT IdEmpresa, IdMatricula "
                    + " FROM      dbo.aca_AlumnoRetiro AS r WITH (nolock) "
                    + " WHERE(Estado = 1)) AS ret ON m.IdEmpresa = ret.IdEmpresa AND m.IdMatricula = ret.IdMatricula "
                    + " WHERE mp.IdEmpresa = @IdEmpresa "
                    + " and m.IdAnio = @IdAnio "
                    + " and m.IdMatricula = @IdMatricula "
                    + " ) "
                    + " UNION ALL "
                    + " ( "
                    + " /*PROMEDIO FINAL*/ "
                    + " SELECT m.IdEmpresa, m.IdMatricula, m.IdAnio, m.IdSede, m.IdNivel, m.IdJornada, m.IdCurso, m.IdParalelo, m.IdAlumno, AN.Descripcion, sn.NomSede, nj.NomJornada, nj.OrdenJornada, sn.NomNivel, sn.OrdenNivel, jc.NomCurso, jc.OrdenCurso, "
                    + " cp.NomParalelo, cp.OrdenParalelo, alu.Codigo, pa.pe_nombreCompleto AS NombreAlumno, mc.IdMateria, mc.NomMateria NombreMateria, mc.NomMateriaArea, mc.NomMateriaGrupo NombreGrupo, mc.OrdenMateria, mc.OrdenMateriaGrupo OrdenGrupo, mc.PromediarGrupo, mc.IdCatalogoTipoCalificacion, "
                    + " CAST(mco.PromedioFinal AS varchar) AS Calificacion, CAST(mco.PromedioFinal AS numeric(18, 2)) AS CalificacionNumerica, 'PROMEDIO FINAL' AS Columna, 7 AS OrdenColumna, pp.pe_nombreCompleto AS NombreTutor "
                    + " FROM     dbo.aca_Matricula AS m WITH (nolock) INNER JOIN "
                    + " dbo.aca_MatriculaCalificacion AS mco WITH (nolock) ON m.IdEmpresa = mco.IdEmpresa AND m.IdMatricula = mco.IdMatricula LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo AS AN WITH (nolock) ON m.IdEmpresa = AN.IdEmpresa AND m.IdAnio = AN.IdAnio LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Sede_NivelAcademico AS sn WITH (nolock) ON sn.IdEmpresa = m.IdEmpresa AND sn.IdSede = m.IdSede AND sn.IdAnio = m.IdAnio AND sn.IdNivel = m.IdNivel LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_NivelAcademico_Jornada AS nj WITH (nolock) ON nj.IdEmpresa = m.IdEmpresa AND nj.IdAnio = m.IdAnio AND nj.IdSede = m.IdSede AND nj.IdNivel = m.IdNivel AND nj.IdJornada = m.IdJornada LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Jornada_Curso AS jc WITH (nolock) ON jc.IdEmpresa = m.IdEmpresa AND jc.IdAnio = m.IdAnio AND jc.IdSede = m.IdSede AND jc.IdNivel = m.IdNivel AND jc.IdJornada = m.IdJornada AND jc.IdCurso = m.IdCurso LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Paralelo AS cp WITH (nolock) ON cp.IdEmpresa = m.IdEmpresa AND cp.IdAnio = m.IdAnio AND cp.IdSede = m.IdSede AND cp.IdNivel = m.IdNivel AND cp.IdJornada = m.IdJornada AND cp.IdCurso = m.IdCurso AND "
                    + " cp.IdParalelo = m.IdParalelo LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Materia AS mc WITH (nolock) ON mc.IdEmpresa = m.IdEmpresa AND mc.IdAnio = m.IdAnio AND mc.IdSede = m.IdSede AND mc.IdNivel = m.IdNivel AND mc.IdJornada = m.IdJornada AND mc.IdCurso = m.IdCurso AND "
                    + " mc.IdMateria = mco.IdMateria LEFT OUTER JOIN "
                    + " dbo.aca_Alumno AS alu WITH (nolock) ON m.IdEmpresa = alu.IdEmpresa AND m.IdAlumno = alu.IdAlumno LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pa WITH (nolock) ON alu.IdPersona = pa.IdPersona LEFT OUTER JOIN "
                    + " dbo.aca_Profesor AS pro WITH (nolock) ON pro.IdEmpresa = m.IdEmpresa AND pro.IdProfesor = cp.IdProfesorTutor LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pp WITH (nolock) ON pro.IdPersona = pp.IdPersona LEFT OUTER JOIN "
                    + " (SELECT IdEmpresa, IdMatricula "
                    + " FROM      dbo.aca_AlumnoRetiro AS r WITH (nolock) "
                    + " WHERE(Estado = 1)) AS ret ON m.IdEmpresa = ret.IdEmpresa AND m.IdMatricula = ret.IdMatricula "
                    + " WHERE mco.IdEmpresa = @IdEmpresa "
                    + " and m.IdAnio = @IdAnio "
                    + " and m.IdMatricula = @IdMatricula "
                    + " and mc.PromediarGrupo = 0 "
                    + " and mc.IdCatalogoTipoCalificacion = 40 "
                    + " ) "
                    + " UNION ALL "
                    + " ( "
                    + " /*MATERIAS QUE SE PROMEDIAN*/ "
                    + " /*PROMEDIO FINAL*/ "
                    + " SELECT m.IdEmpresa, m.IdMatricula, m.IdAnio, m.IdSede, m.IdNivel, m.IdJornada, m.IdCurso, m.IdParalelo, m.IdAlumno, AN.Descripcion, sn.NomSede, nj.NomJornada, nj.OrdenJornada, sn.NomNivel, sn.OrdenNivel, jc.NomCurso, jc.OrdenCurso, "
                    + " cp.NomParalelo, cp.OrdenParalelo, alu.Codigo, pa.pe_nombreCompleto AS NombreAlumno, mc.IdMateria, mc.NomMateria, mc.NomMateriaArea, mc.NomMateriaGrupo, mc.OrdenMateria, mc.OrdenMateriaGrupo, mc.PromediarGrupo, mc.IdCatalogoTipoCalificacion, "
                    + " CAST(mco.PromedioFinal AS varchar) AS Calificacion, CAST(mco.PromedioFinal AS numeric(18, 2)) AS CalificacionNumerica, 'PROMEDIO FINAL' AS Columna, 7 AS OrdenColumna, pp.pe_nombreCompleto AS NombreTutor "
                    + " FROM     dbo.aca_Matricula AS m WITH (nolock) INNER JOIN "
                    + " dbo.aca_MatriculaCalificacion AS mco WITH (nolock) ON m.IdEmpresa = mco.IdEmpresa AND m.IdMatricula = mco.IdMatricula LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo AS AN WITH (nolock) ON m.IdEmpresa = AN.IdEmpresa AND m.IdAnio = AN.IdAnio LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Sede_NivelAcademico AS sn WITH (nolock) ON sn.IdEmpresa = m.IdEmpresa AND sn.IdSede = m.IdSede AND sn.IdAnio = m.IdAnio AND sn.IdNivel = m.IdNivel LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_NivelAcademico_Jornada AS nj WITH (nolock) ON nj.IdEmpresa = m.IdEmpresa AND nj.IdAnio = m.IdAnio AND nj.IdSede = m.IdSede AND nj.IdNivel = m.IdNivel AND nj.IdJornada = m.IdJornada LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Jornada_Curso AS jc WITH (nolock) ON jc.IdEmpresa = m.IdEmpresa AND jc.IdAnio = m.IdAnio AND jc.IdSede = m.IdSede AND jc.IdNivel = m.IdNivel AND jc.IdJornada = m.IdJornada AND jc.IdCurso = m.IdCurso LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Paralelo AS cp WITH (nolock) ON cp.IdEmpresa = m.IdEmpresa AND cp.IdAnio = m.IdAnio AND cp.IdSede = m.IdSede AND cp.IdNivel = m.IdNivel AND cp.IdJornada = m.IdJornada AND cp.IdCurso = m.IdCurso AND "
                    + " cp.IdParalelo = m.IdParalelo LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Materia AS mc WITH (nolock) ON mc.IdEmpresa = m.IdEmpresa AND mc.IdAnio = m.IdAnio AND mc.IdSede = m.IdSede AND mc.IdNivel = m.IdNivel AND mc.IdJornada = m.IdJornada AND mc.IdCurso = m.IdCurso AND "
                    + " mc.IdMateria = mco.IdMateria LEFT OUTER JOIN "
                    + " dbo.aca_Alumno AS alu WITH (nolock) ON m.IdEmpresa = alu.IdEmpresa AND m.IdAlumno = alu.IdAlumno LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pa WITH (nolock) ON alu.IdPersona = pa.IdPersona LEFT OUTER JOIN "
                    + " dbo.aca_Profesor AS pro WITH (nolock) ON pro.IdEmpresa = m.IdEmpresa AND pro.IdProfesor = cp.IdProfesorTutor LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pp WITH (nolock) ON pro.IdPersona = pp.IdPersona LEFT OUTER JOIN "
                    + " (SELECT IdEmpresa, IdMatricula "
                    + " FROM      dbo.aca_AlumnoRetiro AS r WITH (nolock) "
                    + " WHERE(Estado = 1)) AS ret ON m.IdEmpresa = ret.IdEmpresa AND m.IdMatricula = ret.IdMatricula "
                    + " WHERE mco.IdEmpresa = @IdEmpresa "
                    + " and m.IdAnio = @IdAnio "
                    + " and m.IdMatricula = @IdMatricula "
                    + " and mc.PromediarGrupo = 1 "
                    + " and mc.IdCatalogoTipoCalificacion = 40 "
                    + " ) ";
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 0;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new ACA_049_General_Detalle_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdMatricula = Convert.ToDecimal(reader["IdMatricula"]),
                            IdMateria = string.IsNullOrEmpty(reader["IdMateria"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdMateria"]),
                            IdAlumno = Convert.ToDecimal(reader["IdAlumno"]),
                            Codigo = string.IsNullOrEmpty(reader["Codigo"].ToString()) ? null : reader["Codigo"].ToString(),
                            NombreAlumno = string.IsNullOrEmpty(reader["NombreAlumno"].ToString()) ? null : reader["NombreAlumno"].ToString(),
                            NombreTutor = string.IsNullOrEmpty(reader["NombreTutor"].ToString()) ? null : reader["NombreTutor"].ToString(),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            IdSede = Convert.ToInt32(reader["IdSede"]),
                            IdJornada = Convert.ToInt32(reader["IdJornada"]),
                            IdCurso = Convert.ToInt32(reader["IdCurso"]),
                            IdParalelo = Convert.ToInt32(reader["IdParalelo"]),
                            IdNivel = Convert.ToInt32(reader["IdNivel"]),
                            Descripcion = string.IsNullOrEmpty(reader["Descripcion"].ToString()) ? null : reader["Descripcion"].ToString(),
                            NomSede = string.IsNullOrEmpty(reader["NomSede"].ToString()) ? null : reader["NomSede"].ToString(),
                            NomNivel = string.IsNullOrEmpty(reader["NomNivel"].ToString()) ? null : reader["NomNivel"].ToString(),
                            NomJornada = string.IsNullOrEmpty(reader["NomJornada"].ToString()) ? null : reader["NomJornada"].ToString(),
                            NomCurso = string.IsNullOrEmpty(reader["NomCurso"].ToString()) ? null : reader["NomCurso"].ToString(),
                            NomParalelo = string.IsNullOrEmpty(reader["NomParalelo"].ToString()) ? null : reader["NomParalelo"].ToString(),
                            OrdenNivel = string.IsNullOrEmpty(reader["OrdenNivel"].ToString()) ? (int?)null : Convert.ToInt32(reader["OrdenNivel"]),
                            OrdenJornada = string.IsNullOrEmpty(reader["OrdenJornada"].ToString()) ? (int?)null : Convert.ToInt32(reader["OrdenJornada"]),
                            OrdenCurso = string.IsNullOrEmpty(reader["OrdenCurso"].ToString()) ? (int?)null : Convert.ToInt32(reader["OrdenCurso"]),
                            OrdenParalelo = string.IsNullOrEmpty(reader["OrdenParalelo"].ToString()) ? (int?)null : Convert.ToInt32(reader["OrdenParalelo"]),
                            OrdenMateria = string.IsNullOrEmpty(reader["OrdenMateria"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenMateria"]),
                            Calificacion = string.IsNullOrEmpty(reader["Calificacion"].ToString()) ? null : reader["Calificacion"].ToString(),
                            CalificacionNumerica = string.IsNullOrEmpty(reader["CalificacionNumerica"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionNumerica"]),
                            IdCatalogoTipoCalificacion = string.IsNullOrEmpty(reader["IdCatalogoTipoCalificacion"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdCatalogoTipoCalificacion"]),
                            Columna = string.IsNullOrEmpty(reader["Columna"].ToString()) ? null : reader["Columna"].ToString(),
                            NombreGrupo = string.IsNullOrEmpty(reader["NombreGrupo"].ToString()) ? null : reader["NombreGrupo"].ToString(),
                            NombreMateria = string.IsNullOrEmpty(reader["NombreMateria"].ToString()) ? null : reader["NombreMateria"].ToString(),
                            OrdenColumna = Convert.ToInt32(reader["OrdenColumna"]),
                            OrdenGrupo = string.IsNullOrEmpty(reader["OrdenGrupo"].ToString()) ? (int?)null : Convert.ToInt32(reader["OrdenGrupo"]),
                            PromediarGrupo = string.IsNullOrEmpty(reader["PromediarGrupo"].ToString()) ? 0 : Convert.ToInt32(reader["PromediarGrupo"]),
                            NomMateriaArea = string.IsNullOrEmpty(reader["NomMateriaArea"].ToString()) ? null : reader["NomMateriaArea"].ToString()
                        });
                    }
                    reader.Close();
                }

                Lista.ForEach(q=> q.CalificacionCualitativa = (q.CalificacionNumerica==null ? "" : funciones.NumeroALetras_Certificado(q.CalificacionNumerica.ToString())) );
                //ListaObligatorias = Lista.Where(q => q.PromediarGrupo == 0 && q.IdCatalogoTipoCalificacion == Convert.ToInt32(cl_enumeradores.eCatalogoTipoCalificacion.CUANTI)).ToList();
                var ListaObligatorias_ValidaCalificaciones = Lista.Where(q => q.Columna == "PROMEDIO FINAL").ToList();
                ListaObligatorias_ValidaCalificaciones.ForEach(q => q.NoTieneCalificacion = (q.Calificacion == null ? 1 : 0));
                ListaFinal.AddRange(ListaObligatorias_ValidaCalificaciones);

                //ListaComplementarias = Lista.Where(q => q.PromediarGrupo == 1 && q.IdMateria != null && q.IdCatalogoTipoCalificacion == Convert.ToInt32(cl_enumeradores.eCatalogoTipoCalificacion.CUANTI)).ToList();
                //var ListaComplementarias_PromedioFinal = ListaComplementarias.Where(q => q.Columna == "PROMEDIO FINAL").ToList();

                return ListaFinal;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
