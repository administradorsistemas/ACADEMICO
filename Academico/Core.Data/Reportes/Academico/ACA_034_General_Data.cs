using Core.Data.Academico;
using Core.Data.Base;
using Core.Info.Helps;
using Core.Info.Reportes.Academico;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Reportes.Academico
{
    public class ACA_034_General_Data
    {
        aca_AnioLectivo_Data odata_anio = new aca_AnioLectivo_Data();
        aca_AnioLectivoEquivalenciaPromedio_Data odata_equivalencia = new aca_AnioLectivoEquivalenciaPromedio_Data();
        public List<ACA_034_General_Info> get_list(int IdEmpresa, int IdAnio, int IdSede, int IdNivel, int IdJornada, int IdCurso, int IdParalelo, decimal IdAlumno, bool MostrarRetirados)
        {
            try
            {

                List<ACA_034_General_Info> Lista = new List<ACA_034_General_Info>();
                List<ACA_034_General_Info> ListaFinal = new List<ACA_034_General_Info>();

                List<ACA_034_General_Info> Lista_Comportamiento_Proyectos = new List<ACA_034_General_Info>();
                List<ACA_034_General_Info> ListaObligatorias = new List<ACA_034_General_Info>();
                List<ACA_034_General_Info> ListaComplementarias = new List<ACA_034_General_Info>();
                List<ACA_034_General_Info> ListaComplementariasIndividuales = new List<ACA_034_General_Info>();

                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();

                    #region Query
                    string query = "DECLARE @IdEmpresa int =" + IdEmpresa.ToString() + ", @IdAnio int = " + IdAnio.ToString() + ", @IdSede int = " + IdSede.ToString() + ", @IdNivel int = " + IdNivel.ToString() + ", @IdJornada int = " + IdJornada.ToString() + ", @IdCurso int= " + IdCurso.ToString() + ", @IdParalelo int = " + IdParalelo.ToString() + ", @IdAlumno numeric = " + IdAlumno.ToString() + " , @MostrarRetirados bit = " + (MostrarRetirados == false ? 0 : 1)
                    + " /*COMPORTAMIENTO*/ "
                    + " SELECT m.IdEmpresa, m.IdMatricula, m.IdAnio, m.IdSede, m.IdNivel, m.IdJornada, m.IdCurso, m.IdParalelo, m.IdAlumno, AN.Descripcion, sn.NomSede, nj.NomJornada, nj.OrdenJornada, sn.NomNivel, sn.OrdenNivel, jc.NomCurso, jc.OrdenCurso,  "
                    + " cp.NomParalelo, cp.OrdenParalelo, pa.pe_nombreCompleto AS NombreAlumno, 0 AS IdMateria, NULL AS NombreMateria, NULL AS NombreGrupo, 99999 AS OrdenMateria, 99999 AS OrdenGrupo, 0 AS PromediarGrupo, NULL "
                    + " AS IdCatalogoTipoCalificacion, CAST(equiv.Letra AS varchar) AS Calificacion, CAST(equiv.Calificacion AS numeric(18, 2)) AS CalificacionNumerica, 'EVALUACION DEL COMPORTAMIENTO' AS Columna, 1 AS OrdenColumna, "
                    + " pp.pe_nombreCompleto AS NombreTutor "
                    + " FROM     dbo.aca_Matricula AS m with (nolock) INNER JOIN "
                    + " dbo.aca_MatriculaConducta AS mco with (nolock) ON m.IdEmpresa = mco.IdEmpresa AND m.IdMatricula = mco.IdMatricula LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivoConductaEquivalencia AS equiv with (nolock) ON mco.IdEmpresa = equiv.IdEmpresa AND mco.SecuenciaPromedioFinal = equiv.Secuencia AND m.IdAnio = equiv.IdAnio LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo AS AN with (nolock) ON m.IdEmpresa = AN.IdEmpresa AND m.IdAnio = AN.IdAnio LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Sede_NivelAcademico AS sn with (nolock) ON sn.IdEmpresa = m.IdEmpresa AND sn.IdSede = m.IdSede AND sn.IdAnio = m.IdAnio AND sn.IdNivel = m.IdNivel LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_NivelAcademico_Jornada AS nj with (nolock) ON nj.IdEmpresa = m.IdEmpresa AND nj.IdAnio = m.IdAnio AND nj.IdSede = m.IdSede AND nj.IdNivel = m.IdNivel AND nj.IdJornada = m.IdJornada LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Jornada_Curso AS jc with (nolock) ON jc.IdEmpresa = m.IdEmpresa AND jc.IdAnio = m.IdAnio AND jc.IdSede = m.IdSede AND jc.IdNivel = m.IdNivel AND jc.IdJornada = m.IdJornada AND jc.IdCurso = m.IdCurso LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Paralelo AS cp with (nolock) ON cp.IdEmpresa = m.IdEmpresa AND cp.IdAnio = m.IdAnio AND cp.IdSede = m.IdSede AND cp.IdNivel = m.IdNivel AND cp.IdJornada = m.IdJornada AND cp.IdCurso = m.IdCurso AND "
                    + " cp.IdParalelo = m.IdParalelo LEFT OUTER JOIN "
                    + " dbo.aca_Alumno AS alu with (nolock) ON m.IdEmpresa = alu.IdEmpresa AND m.IdAlumno = alu.IdAlumno LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pa with (nolock) ON alu.IdPersona = pa.IdPersona LEFT OUTER JOIN "
                    + " dbo.aca_Profesor AS pro with (nolock) ON pro.IdEmpresa = m.IdEmpresa AND pro.IdProfesor = cp.IdProfesorTutor LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pp with (nolock) ON pro.IdPersona = pp.IdPersona LEFT OUTER JOIN "
                    + " (SELECT IdEmpresa, IdMatricula "
                    + " FROM      dbo.aca_AlumnoRetiro AS r with (nolock) "
                    + " WHERE(Estado = 1)) AS ret ON m.IdEmpresa = ret.IdEmpresa AND m.IdMatricula = ret.IdMatricula "
                    + " where mco.IdEmpresa = @IdEmpresa "
                    + " and m.IdAnio = @IdAnio "
                    + " and m.IdSede = @IdSede "
                    + " and m.IdNivel = @IdNivel "
                    + " and m.IdJornada = @IdJornada "
                    + " and m.IdCurso = @IdCurso "
                    + " and m.IdParalelo = @IdParalelo "
                    + " and m.IdAlumno = case when @IdAlumno = 0 then m.IdAlumno else @IdAlumno end "
                    + " and isnull(ret.IdMatricula, 0) = case when @MostrarRetirados = 1 then isnull(ret.IdMatricula, 0) else 0 end "
                    + " UNION ALL "
                    + " ( "
                    + " /*PROYECTOS*/ "
                    + " SELECT m.IdEmpresa, m.IdMatricula, m.IdAnio, m.IdSede, m.IdNivel, m.IdJornada, m.IdCurso, m.IdParalelo, m.IdAlumno, AN.Descripcion, sn.NomSede, nj.NomJornada, nj.OrdenJornada, sn.NomNivel, sn.OrdenNivel, jc.NomCurso, jc.OrdenCurso, "
                    + " cp.NomParalelo, cp.OrdenParalelo, pa.pe_nombreCompleto AS NombreAlumno, 0 AS IdMateria, NULL AS NombreMateria, NULL AS NombreGrupo, 999999 AS OrdenMateria, 999999 AS OrdenGrupo, 0 AS PromediarGrupo, "
                    //+ " mc.IdCatalogoTipoCalificacion AS IdCatalogoTipoCalificacion, CAST(lp.Codigo AS varchar) AS Calificacion, CAST(ep.Calificacion AS numeric(18, 2)) AS CalificacionNumerica, 'EVALUACION DE PROYECTOS ESCOLARES' AS Columna, "
                    + " mc.IdCatalogoTipoCalificacion AS IdCatalogoTipoCalificacion, CAST(lp.Codigo AS varchar) AS Calificacion, CAST(ep.Calificacion AS numeric(18, 2)) AS CalificacionNumerica, isnull(mc.NomMateria,'') AS Columna, "
                    + " 1 AS OrdenColumna, pp.pe_nombreCompleto AS NombreTutor "
                    + " FROM     dbo.aca_Matricula AS m with (nolock) LEFT OUTER JOIN "
                    + " dbo.aca_MatriculaCalificacionCualitativaPromedio AS mp with (nolock) ON mp.IdEmpresa = m.IdEmpresa AND mp.IdMatricula = m.IdMatricula LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivoCalificacionCualitativa AS ep with (nolock) ON ep.IdAnio = m.IdAnio AND ep.IdEmpresa = m.IdEmpresa AND ep.IdCalificacionCualitativa = mp.IdCalificacionCualitativaFinal LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo AS AN with (nolock) ON m.IdEmpresa = AN.IdEmpresa AND m.IdAnio = AN.IdAnio LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Sede_NivelAcademico AS sn with (nolock) ON sn.IdEmpresa = m.IdEmpresa AND sn.IdSede = m.IdSede AND sn.IdAnio = m.IdAnio AND sn.IdNivel = m.IdNivel LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_NivelAcademico_Jornada AS nj with (nolock) ON nj.IdEmpresa = m.IdEmpresa AND nj.IdAnio = m.IdAnio AND nj.IdSede = m.IdSede AND nj.IdNivel = m.IdNivel AND nj.IdJornada = m.IdJornada LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Jornada_Curso AS jc with (nolock) ON jc.IdEmpresa = m.IdEmpresa AND jc.IdAnio = m.IdAnio AND jc.IdSede = m.IdSede AND jc.IdNivel = m.IdNivel AND jc.IdJornada = m.IdJornada AND jc.IdCurso = m.IdCurso LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Paralelo AS cp with (nolock) ON cp.IdEmpresa = m.IdEmpresa AND cp.IdAnio = m.IdAnio AND cp.IdSede = m.IdSede AND cp.IdNivel = m.IdNivel AND cp.IdJornada = m.IdJornada AND cp.IdCurso = m.IdCurso AND "
                    + " cp.IdParalelo = m.IdParalelo LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Materia AS mc with (nolock) ON mc.IdEmpresa = m.IdEmpresa AND mc.IdAnio = m.IdAnio AND mc.IdSede = m.IdSede AND mc.IdNivel = m.IdNivel AND mc.IdJornada = m.IdJornada AND mc.IdCurso = m.IdCurso AND mc.IdMateria = mp.IdMateria LEFT OUTER JOIN "
                    + " dbo.aca_Alumno AS alu with (nolock) ON m.IdEmpresa = alu.IdEmpresa AND m.IdAlumno = alu.IdAlumno LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pa with (nolock) ON alu.IdPersona = pa.IdPersona LEFT OUTER JOIN "
                    + " dbo.aca_Profesor AS pro with (nolock) ON pro.IdEmpresa = m.IdEmpresa AND pro.IdProfesor = cp.IdProfesorTutor LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pp with (nolock) ON pro.IdPersona = pp.IdPersona LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivoCalificacionCualitativa AS lp with (nolock) ON lp.IdEmpresa = m.IdEmpresa AND lp.IdAnio = m.IdAnio AND lp.IdCalificacionCualitativa = mp.IdCalificacionCualitativaFinal LEFT OUTER JOIN "
                    + " (SELECT IdEmpresa, IdMatricula "
                    + " FROM      dbo.aca_AlumnoRetiro AS r with (nolock) "
                    + " WHERE(Estado = 1)) AS ret ON m.IdEmpresa = ret.IdEmpresa AND m.IdMatricula = ret.IdMatricula "
                    + " WHERE mp.IdEmpresa = @IdEmpresa "
                    + " and m.IdAnio = @IdAnio "
                    + " and m.IdSede = @IdSede "
                    + " and m.IdNivel = @IdNivel "
                    + " and m.IdJornada = @IdJornada "
                    + " and m.IdCurso = @IdCurso "
                    + " and m.IdParalelo = @IdParalelo "
                    + " and m.IdAlumno = case when @IdAlumno = 0 then m.IdAlumno else @IdAlumno end "
                    + " and isnull(ret.IdMatricula, 0) = case when @MostrarRetirados = 1 then isnull(ret.IdMatricula, 0) else 0 end "
                    + " ) "
                    + " UNION ALL "
                    + " ( "
                    + " /*ESTADO MATRICULA*/ "
                    + " SELECT m.IdEmpresa, m.IdMatricula, m.IdAnio, m.IdSede, m.IdNivel, m.IdJornada, m.IdCurso, m.IdParalelo, m.IdAlumno, "
                    + " AN.Descripcion, sn.NomSede, nj.NomJornada, nj.OrdenJornada, sn.NomNivel, sn.OrdenNivel, jc.NomCurso, jc.OrdenCurso,   cp.NomParalelo, "
                    + " cp.OrdenParalelo, pa.pe_nombreCompleto AS NombreAlumno, 0 AS IdMateria, NULL AS NombreMateria, NULL AS NombreGrupo, 9999999 AS OrdenMateria, "
                    + " 9999999 AS OrdenGrupo, 0 AS PromediarGrupo, NULL  AS IdCatalogoTipoCalificacion, C.NomCatalogo AS Calificacion, "
                    + " NULL CalificacionNumerica, 'OBSERVACIÓN' AS Columna, 1 AS OrdenColumna, pp.pe_nombreCompleto AS NombreTutor "
                    + " FROM     dbo.aca_Matricula AS m with (nolock) LEFT OUTER JOIN  dbo.aca_AnioLectivo AS AN with (nolock) ON m.IdEmpresa = AN.IdEmpresa "
                    + " AND m.IdAnio = AN.IdAnio LEFT OUTER JOIN dbo.aca_AnioLectivo_Sede_NivelAcademico AS sn with (nolock) ON sn.IdEmpresa = m.IdEmpresa AND sn.IdSede = m.IdSede "
                    + " AND sn.IdAnio = m.IdAnio AND sn.IdNivel = m.IdNivel LEFT OUTER JOIN dbo.aca_AnioLectivo_NivelAcademico_Jornada AS nj with (nolock) ON nj.IdEmpresa = m.IdEmpresa "
                    + " AND nj.IdAnio = m.IdAnio AND nj.IdSede = m.IdSede AND nj.IdNivel = m.IdNivel AND nj.IdJornada = m.IdJornada "
                    + " LEFT OUTER JOIN dbo.aca_AnioLectivo_Jornada_Curso AS jc with (nolock) ON jc.IdEmpresa = m.IdEmpresa AND jc.IdAnio = m.IdAnio AND jc.IdSede = m.IdSede "
                    + " AND jc.IdNivel = m.IdNivel AND jc.IdJornada = m.IdJornada AND jc.IdCurso = m.IdCurso "
                    + " LEFT OUTER JOIN dbo.aca_AnioLectivo_Curso_Paralelo AS cp with (nolock) ON cp.IdEmpresa = m.IdEmpresa AND cp.IdAnio = m.IdAnio AND cp.IdSede = m.IdSede "
                    + " AND cp.IdNivel = m.IdNivel AND cp.IdJornada = m.IdJornada AND cp.IdCurso = m.IdCurso AND cp.IdParalelo = m.IdParalelo "
                    + " LEFT OUTER JOIN dbo.aca_Alumno AS alu with (nolock) ON m.IdEmpresa = alu.IdEmpresa AND m.IdAlumno = alu.IdAlumno "
                    + " LEFT OUTER JOIN dbo.tb_persona AS pa with (nolock) ON alu.IdPersona = pa.IdPersona "
                    + " LEFT OUTER JOIN dbo.aca_Profesor AS pro with (nolock) ON pro.IdEmpresa = m.IdEmpresa AND pro.IdProfesor = cp.IdProfesorTutor "
                    + " LEFT OUTER JOIN dbo.tb_persona AS pp with (nolock) ON pro.IdPersona = pp.IdPersona "
                    + " LEFT OUTER JOIN dbo.aca_Catalogo c with (nolock) on m.IdCatalogoESTMAT = c.IdCatalogo "
                    + " LEFT OUTER JOIN(SELECT IdEmpresa, IdMatricula  FROM      dbo.aca_AlumnoRetiro AS r  WHERE(Estado = 1)) AS ret "
                    + " ON m.IdEmpresa = ret.IdEmpresa AND m.IdMatricula = ret.IdMatricula "
                    + " where m.IdEmpresa = @IdEmpresa  and m.IdAnio = @IdAnio  and m.IdSede = @IdSede  and m.IdNivel = @IdNivel  and m.IdJornada = @IdJornada "
                    + " and m.IdCurso = @IdCurso  and m.IdParalelo = @IdParalelo  and m.IdAlumno = case when @IdAlumno = 0 then m.IdAlumno else @IdAlumno "
                    + " end  and isnull(ret.IdMatricula, 0) = case when @MostrarRetirados = 1 then isnull(ret.IdMatricula, 0) else 0 end "
                    + " ) "
                    + " UNION ALL "
                    + " ( "
                    + " /*MATERIAS QUE NO SE PROMEDIAN*/ "
                    + " /*I QUIMESTRE*/ "
                    + " SELECT m.IdEmpresa, m.IdMatricula, m.IdAnio, m.IdSede, m.IdNivel, m.IdJornada, m.IdCurso, m.IdParalelo, m.IdAlumno, AN.Descripcion, sn.NomSede, nj.NomJornada, nj.OrdenJornada, sn.NomNivel, sn.OrdenNivel, jc.NomCurso, jc.OrdenCurso, "
                    + " cp.NomParalelo, cp.OrdenParalelo, pa.pe_nombreCompleto AS NombreAlumno, mc.IdMateria, mc.NomMateria, mc.NomMateriaGrupo, mc.OrdenMateria, mc.OrdenMateriaGrupo, mc.PromediarGrupo, mc.IdCatalogoTipoCalificacion, "
                    + " CAST(mco.PromedioFinalQ1 AS varchar) AS Calificacion, CAST(mco.PromedioFinalQ1 AS numeric(18, 2)) AS CalificacionNumerica, 'I QUIMESTRE' AS Columna, 1 AS OrdenColumna, pp.pe_nombreCompleto AS NombreTutor "
                    + " FROM     dbo.aca_Matricula AS m with (nolock) INNER JOIN "
                    + " dbo.aca_MatriculaCalificacion AS mco with (nolock) ON m.IdEmpresa = mco.IdEmpresa AND m.IdMatricula = mco.IdMatricula LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo AS AN with (nolock) ON m.IdEmpresa = AN.IdEmpresa AND m.IdAnio = AN.IdAnio LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Sede_NivelAcademico AS sn with (nolock) ON sn.IdEmpresa = m.IdEmpresa AND sn.IdSede = m.IdSede AND sn.IdAnio = m.IdAnio AND sn.IdNivel = m.IdNivel LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_NivelAcademico_Jornada AS nj with (nolock) ON nj.IdEmpresa = m.IdEmpresa AND nj.IdAnio = m.IdAnio AND nj.IdSede = m.IdSede AND nj.IdNivel = m.IdNivel AND nj.IdJornada = m.IdJornada LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Jornada_Curso AS jc with (nolock) ON jc.IdEmpresa = m.IdEmpresa AND jc.IdAnio = m.IdAnio AND jc.IdSede = m.IdSede AND jc.IdNivel = m.IdNivel AND jc.IdJornada = m.IdJornada AND jc.IdCurso = m.IdCurso LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Paralelo AS cp with (nolock) ON cp.IdEmpresa = m.IdEmpresa AND cp.IdAnio = m.IdAnio AND cp.IdSede = m.IdSede AND cp.IdNivel = m.IdNivel AND cp.IdJornada = m.IdJornada AND cp.IdCurso = m.IdCurso AND "
                    + " cp.IdParalelo = m.IdParalelo LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Materia AS mc with (nolock) ON mc.IdEmpresa = m.IdEmpresa AND mc.IdAnio = m.IdAnio AND mc.IdSede = m.IdSede AND mc.IdNivel = m.IdNivel AND mc.IdJornada = m.IdJornada AND mc.IdCurso = m.IdCurso AND "
                    + " mc.IdMateria = mco.IdMateria LEFT OUTER JOIN "
                    + " dbo.aca_Alumno AS alu with (nolock) ON m.IdEmpresa = alu.IdEmpresa AND m.IdAlumno = alu.IdAlumno LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pa with (nolock) ON alu.IdPersona = pa.IdPersona LEFT OUTER JOIN "
                    + " dbo.aca_Profesor AS pro with (nolock) ON pro.IdEmpresa = m.IdEmpresa AND pro.IdProfesor = cp.IdProfesorTutor LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pp with (nolock) ON pro.IdPersona = pp.IdPersona LEFT OUTER JOIN "
                    + " (SELECT IdEmpresa, IdMatricula "
                    + " FROM      dbo.aca_AlumnoRetiro AS r with (nolock) "
                    + " WHERE(Estado = 1)) AS ret ON m.IdEmpresa = ret.IdEmpresa AND m.IdMatricula = ret.IdMatricula "
                    + " WHERE mco.IdEmpresa = @IdEmpresa "
                    + " and m.IdAnio = @IdAnio "
                    + " and m.IdSede = @IdSede "
                    + " and m.IdNivel = @IdNivel "
                    + " and m.IdJornada = @IdJornada "
                    + " and m.IdCurso = @IdCurso "
                    + " and m.IdParalelo = @IdParalelo "
                    + " and m.IdAlumno = case when @IdAlumno = 0 then m.IdAlumno else @IdAlumno end "
                    + " and isnull(ret.IdMatricula, 0) = case when @MostrarRetirados = 1 then isnull(ret.IdMatricula, 0) else 0 end "
                    + " and mc.PromediarGrupo = 0 "
                    + " and mc.IdCatalogoTipoCalificacion = 40 "
                    + " ) "
                    + " UNION ALL "
                    + " ( "
                    + " /*II QUIMESTRE*/ "
                    + " SELECT m.IdEmpresa, m.IdMatricula, m.IdAnio, m.IdSede, m.IdNivel, m.IdJornada, m.IdCurso, m.IdParalelo, m.IdAlumno, AN.Descripcion, sn.NomSede, nj.NomJornada, nj.OrdenJornada, sn.NomNivel, sn.OrdenNivel, jc.NomCurso, jc.OrdenCurso, "
                    + " cp.NomParalelo, cp.OrdenParalelo, pa.pe_nombreCompleto AS NombreAlumno, mc.IdMateria, mc.NomMateria, mc.NomMateriaGrupo, mc.OrdenMateria, mc.OrdenMateriaGrupo, mc.PromediarGrupo, mc.IdCatalogoTipoCalificacion, "
                    + " CAST(mco.PromedioFinalQ2 AS varchar) AS Calificacion, CAST(mco.PromedioFinalQ2 AS numeric(18, 2)) AS CalificacionNumerica, 'II QUIMESTRE' AS Columna, 2 AS OrdenColumna, pp.pe_nombreCompleto AS NombreTutor "
                    + " FROM     dbo.aca_Matricula AS m with (nolock) INNER JOIN "
                    + " dbo.aca_MatriculaCalificacion AS mco with (nolock) ON m.IdEmpresa = mco.IdEmpresa AND m.IdMatricula = mco.IdMatricula LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo AS AN with (nolock) ON m.IdEmpresa = AN.IdEmpresa AND m.IdAnio = AN.IdAnio LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Sede_NivelAcademico AS sn with (nolock) ON sn.IdEmpresa = m.IdEmpresa AND sn.IdSede = m.IdSede AND sn.IdAnio = m.IdAnio AND sn.IdNivel = m.IdNivel LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_NivelAcademico_Jornada AS nj with (nolock) ON nj.IdEmpresa = m.IdEmpresa AND nj.IdAnio = m.IdAnio AND nj.IdSede = m.IdSede AND nj.IdNivel = m.IdNivel AND nj.IdJornada = m.IdJornada LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Jornada_Curso AS jc with (nolock) ON jc.IdEmpresa = m.IdEmpresa AND jc.IdAnio = m.IdAnio AND jc.IdSede = m.IdSede AND jc.IdNivel = m.IdNivel AND jc.IdJornada = m.IdJornada AND jc.IdCurso = m.IdCurso LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Paralelo AS cp with (nolock) ON cp.IdEmpresa = m.IdEmpresa AND cp.IdAnio = m.IdAnio AND cp.IdSede = m.IdSede AND cp.IdNivel = m.IdNivel AND cp.IdJornada = m.IdJornada AND cp.IdCurso = m.IdCurso AND "
                    + " cp.IdParalelo = m.IdParalelo LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Materia AS mc with (nolock) ON mc.IdEmpresa = m.IdEmpresa AND mc.IdAnio = m.IdAnio AND mc.IdSede = m.IdSede AND mc.IdNivel = m.IdNivel AND mc.IdJornada = m.IdJornada AND mc.IdCurso = m.IdCurso AND "
                    + " mc.IdMateria = mco.IdMateria LEFT OUTER JOIN "
                    + " dbo.aca_Alumno AS alu with (nolock) ON m.IdEmpresa = alu.IdEmpresa AND m.IdAlumno = alu.IdAlumno LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pa with (nolock) ON alu.IdPersona = pa.IdPersona LEFT OUTER JOIN "
                    + " dbo.aca_Profesor AS pro with (nolock) ON pro.IdEmpresa = m.IdEmpresa AND pro.IdProfesor = cp.IdProfesorTutor LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pp with (nolock) ON pro.IdPersona = pp.IdPersona LEFT OUTER JOIN "
                    + " (SELECT IdEmpresa, IdMatricula "
                    + " FROM      dbo.aca_AlumnoRetiro AS r with (nolock) "
                    + " WHERE(Estado = 1)) AS ret ON m.IdEmpresa = ret.IdEmpresa AND m.IdMatricula = ret.IdMatricula "
                    + " WHERE mco.IdEmpresa = @IdEmpresa "
                    + " and m.IdAnio = @IdAnio "
                    + " and m.IdSede = @IdSede "
                    + " and m.IdNivel = @IdNivel "
                    + " and m.IdJornada = @IdJornada "
                    + " and m.IdCurso = @IdCurso "
                    + " and m.IdParalelo = @IdParalelo "
                    + " and m.IdAlumno = case when @IdAlumno = 0 then m.IdAlumno else @IdAlumno end "
                    + " and isnull(ret.IdMatricula, 0) = case when @MostrarRetirados = 1 then isnull(ret.IdMatricula, 0) else 0 end "
                    + " and mc.PromediarGrupo = 0 "
                    + " and mc.IdCatalogoTipoCalificacion = 40 "
                    + " ) "
                    + " UNION ALL "
                    + " ( "
                    + " /*PROMEDIO DE LOS 2 QUIMESTRES*/ "
                    + " SELECT m.IdEmpresa, m.IdMatricula, m.IdAnio, m.IdSede, m.IdNivel, m.IdJornada, m.IdCurso, m.IdParalelo, m.IdAlumno, AN.Descripcion, sn.NomSede, nj.NomJornada, nj.OrdenJornada, sn.NomNivel, sn.OrdenNivel, jc.NomCurso, jc.OrdenCurso, "
                    + " cp.NomParalelo, cp.OrdenParalelo, pa.pe_nombreCompleto AS NombreAlumno, mc.IdMateria, mc.NomMateria, mc.NomMateriaGrupo, mc.OrdenMateria, mc.OrdenMateriaGrupo, mc.PromediarGrupo, mc.IdCatalogoTipoCalificacion, "
                    + " CAST(mco.PromedioQuimestres AS varchar) AS Calificacion, CAST(mco.PromedioQuimestres AS numeric(18, 2)) AS CalificacionNumerica, 'PROMEDIO' AS Columna, 3 AS OrdenColumna, pp.pe_nombreCompleto AS NombreTutor "
                    + " FROM     dbo.aca_Matricula AS m with (nolock) INNER JOIN "
                    + " dbo.aca_MatriculaCalificacion AS mco with (nolock) ON m.IdEmpresa = mco.IdEmpresa AND m.IdMatricula = mco.IdMatricula LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo AS AN with (nolock) ON m.IdEmpresa = AN.IdEmpresa AND m.IdAnio = AN.IdAnio LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Sede_NivelAcademico AS sn with (nolock) ON sn.IdEmpresa = m.IdEmpresa AND sn.IdSede = m.IdSede AND sn.IdAnio = m.IdAnio AND sn.IdNivel = m.IdNivel LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_NivelAcademico_Jornada AS nj with (nolock) ON nj.IdEmpresa = m.IdEmpresa AND nj.IdAnio = m.IdAnio AND nj.IdSede = m.IdSede AND nj.IdNivel = m.IdNivel AND nj.IdJornada = m.IdJornada LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Jornada_Curso AS jc with (nolock) ON jc.IdEmpresa = m.IdEmpresa AND jc.IdAnio = m.IdAnio AND jc.IdSede = m.IdSede AND jc.IdNivel = m.IdNivel AND jc.IdJornada = m.IdJornada AND jc.IdCurso = m.IdCurso LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Paralelo AS cp with (nolock) ON cp.IdEmpresa = m.IdEmpresa AND cp.IdAnio = m.IdAnio AND cp.IdSede = m.IdSede AND cp.IdNivel = m.IdNivel AND cp.IdJornada = m.IdJornada AND cp.IdCurso = m.IdCurso AND "
                    + " cp.IdParalelo = m.IdParalelo LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Materia AS mc with (nolock) ON mc.IdEmpresa = m.IdEmpresa AND mc.IdAnio = m.IdAnio AND mc.IdSede = m.IdSede AND mc.IdNivel = m.IdNivel AND mc.IdJornada = m.IdJornada AND mc.IdCurso = m.IdCurso AND "
                    + " mc.IdMateria = mco.IdMateria LEFT OUTER JOIN "
                    + " dbo.aca_Alumno AS alu with (nolock) ON m.IdEmpresa = alu.IdEmpresa AND m.IdAlumno = alu.IdAlumno LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pa with (nolock) ON alu.IdPersona = pa.IdPersona LEFT OUTER JOIN "
                    + " dbo.aca_Profesor AS pro with (nolock) ON pro.IdEmpresa = m.IdEmpresa AND pro.IdProfesor = cp.IdProfesorTutor LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pp with (nolock) ON pro.IdPersona = pp.IdPersona LEFT OUTER JOIN "
                    + " (SELECT IdEmpresa, IdMatricula "
                    + " FROM      dbo.aca_AlumnoRetiro AS r with (nolock) "
                    + " WHERE(Estado = 1)) AS ret ON m.IdEmpresa = ret.IdEmpresa AND m.IdMatricula = ret.IdMatricula "
                    + " WHERE mco.IdEmpresa = @IdEmpresa "
                    + " and m.IdAnio = @IdAnio "
                    + " and m.IdSede = @IdSede "
                    + " and m.IdNivel = @IdNivel "
                    + " and m.IdJornada = @IdJornada "
                    + " and m.IdCurso = @IdCurso "
                    + " and m.IdParalelo = @IdParalelo "
                    + " and m.IdAlumno = case when @IdAlumno = 0 then m.IdAlumno else @IdAlumno end "
                    + " and isnull(ret.IdMatricula, 0) = case when @MostrarRetirados = 1 then isnull(ret.IdMatricula, 0) else 0 end "
                    + " and mc.PromediarGrupo = 0 "
                    + " and mc.IdCatalogoTipoCalificacion = 40 "
                    + " ) "
                    + " UNION ALL "
                    + " ( "
                    + " /*MEJORAMIENTO*/ "
                    + " SELECT m.IdEmpresa, m.IdMatricula, m.IdAnio, m.IdSede, m.IdNivel, m.IdJornada, m.IdCurso, m.IdParalelo, m.IdAlumno, AN.Descripcion, sn.NomSede, nj.NomJornada, nj.OrdenJornada, sn.NomNivel, sn.OrdenNivel, jc.NomCurso, jc.OrdenCurso, "
                    + " cp.NomParalelo, cp.OrdenParalelo, pa.pe_nombreCompleto AS NombreAlumno, mc.IdMateria, mc.NomMateria, mc.NomMateriaGrupo, mc.OrdenMateria, mc.OrdenMateriaGrupo, mc.PromediarGrupo, mc.IdCatalogoTipoCalificacion, "
                    + " CAST(mco.ExamenMejoramiento AS varchar) AS Calificacion, CAST(mco.ExamenMejoramiento AS numeric(18, 2)) AS CalificacionNumerica, 'MEJORAMIENTO' AS Columna, 4 AS OrdenColumna, pp.pe_nombreCompleto AS NombreTutor "
                    + " FROM     dbo.aca_Matricula AS m with (nolock) INNER JOIN "
                    + " dbo.aca_MatriculaCalificacion AS mco with (nolock) ON m.IdEmpresa = mco.IdEmpresa AND m.IdMatricula = mco.IdMatricula LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo AS AN with (nolock) ON m.IdEmpresa = AN.IdEmpresa AND m.IdAnio = AN.IdAnio LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Sede_NivelAcademico AS sn with (nolock) ON sn.IdEmpresa = m.IdEmpresa AND sn.IdSede = m.IdSede AND sn.IdAnio = m.IdAnio AND sn.IdNivel = m.IdNivel LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_NivelAcademico_Jornada AS nj with (nolock) ON nj.IdEmpresa = m.IdEmpresa AND nj.IdAnio = m.IdAnio AND nj.IdSede = m.IdSede AND nj.IdNivel = m.IdNivel AND nj.IdJornada = m.IdJornada LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Jornada_Curso AS jc with (nolock) ON jc.IdEmpresa = m.IdEmpresa AND jc.IdAnio = m.IdAnio AND jc.IdSede = m.IdSede AND jc.IdNivel = m.IdNivel AND jc.IdJornada = m.IdJornada AND jc.IdCurso = m.IdCurso LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Paralelo AS cp with (nolock) ON cp.IdEmpresa = m.IdEmpresa AND cp.IdAnio = m.IdAnio AND cp.IdSede = m.IdSede AND cp.IdNivel = m.IdNivel AND cp.IdJornada = m.IdJornada AND cp.IdCurso = m.IdCurso AND "
                    + " cp.IdParalelo = m.IdParalelo LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Materia AS mc with (nolock) ON mc.IdEmpresa = m.IdEmpresa AND mc.IdAnio = m.IdAnio AND mc.IdSede = m.IdSede AND mc.IdNivel = m.IdNivel AND mc.IdJornada = m.IdJornada AND mc.IdCurso = m.IdCurso AND "
                    + " mc.IdMateria = mco.IdMateria LEFT OUTER JOIN "
                    + " dbo.aca_Alumno AS alu with (nolock) ON m.IdEmpresa = alu.IdEmpresa AND m.IdAlumno = alu.IdAlumno LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pa with (nolock) ON alu.IdPersona = pa.IdPersona LEFT OUTER JOIN "
                    + " dbo.aca_Profesor AS pro with (nolock) ON pro.IdEmpresa = m.IdEmpresa AND pro.IdProfesor = cp.IdProfesorTutor LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pp with (nolock) ON pro.IdPersona = pp.IdPersona LEFT OUTER JOIN "
                    + " (SELECT IdEmpresa, IdMatricula "
                    + " FROM      dbo.aca_AlumnoRetiro AS r with (nolock) "
                    + " WHERE(Estado = 1)) AS ret ON m.IdEmpresa = ret.IdEmpresa AND m.IdMatricula = ret.IdMatricula "
                    + " WHERE mco.IdEmpresa = @IdEmpresa "
                    + " and m.IdAnio = @IdAnio "
                    + " and m.IdSede = @IdSede "
                    + " and m.IdNivel = @IdNivel "
                    + " and m.IdJornada = @IdJornada "
                    + " and m.IdCurso = @IdCurso "
                    + " and m.IdParalelo = @IdParalelo "
                    + " and m.IdAlumno = case when @IdAlumno = 0 then m.IdAlumno else @IdAlumno end "
                    + " and isnull(ret.IdMatricula, 0) = case when @MostrarRetirados = 1 then isnull(ret.IdMatricula, 0) else 0 end "
                    + " and mc.PromediarGrupo = 0 "
                    + " and mc.IdCatalogoTipoCalificacion = 40 "
                    + " ) "
                    + " UNION ALL "
                    + " ( "
                    + " /*SUPLETORIO*/ "
                    + " SELECT m.IdEmpresa, m.IdMatricula, m.IdAnio, m.IdSede, m.IdNivel, m.IdJornada, m.IdCurso, m.IdParalelo, m.IdAlumno, AN.Descripcion, sn.NomSede, nj.NomJornada, nj.OrdenJornada, sn.NomNivel, sn.OrdenNivel, jc.NomCurso, jc.OrdenCurso, "
                    + " cp.NomParalelo, cp.OrdenParalelo, pa.pe_nombreCompleto AS NombreAlumno, mc.IdMateria, mc.NomMateria, mc.NomMateriaGrupo, mc.OrdenMateria, mc.OrdenMateriaGrupo, mc.PromediarGrupo, mc.IdCatalogoTipoCalificacion, "
                    + " CAST(mco.ExamenSupletorio AS varchar) AS Calificacion, CAST(mco.ExamenSupletorio AS numeric(18, 2)) AS CalificacionNumerica, 'SUPLETORIO' AS Columna, 5 AS OrdenColumna, pp.pe_nombreCompleto AS NombreTutor "
                    + " FROM     dbo.aca_Matricula AS m with (nolock) INNER JOIN "
                    + " dbo.aca_MatriculaCalificacion AS mco with (nolock) ON m.IdEmpresa = mco.IdEmpresa AND m.IdMatricula = mco.IdMatricula LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo AS AN with (nolock) ON m.IdEmpresa = AN.IdEmpresa AND m.IdAnio = AN.IdAnio LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Sede_NivelAcademico AS sn with (nolock) ON sn.IdEmpresa = m.IdEmpresa AND sn.IdSede = m.IdSede AND sn.IdAnio = m.IdAnio AND sn.IdNivel = m.IdNivel LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_NivelAcademico_Jornada AS nj with (nolock) ON nj.IdEmpresa = m.IdEmpresa AND nj.IdAnio = m.IdAnio AND nj.IdSede = m.IdSede AND nj.IdNivel = m.IdNivel AND nj.IdJornada = m.IdJornada LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Jornada_Curso AS jc with (nolock) ON jc.IdEmpresa = m.IdEmpresa AND jc.IdAnio = m.IdAnio AND jc.IdSede = m.IdSede AND jc.IdNivel = m.IdNivel AND jc.IdJornada = m.IdJornada AND jc.IdCurso = m.IdCurso LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Paralelo AS cp with (nolock) ON cp.IdEmpresa = m.IdEmpresa AND cp.IdAnio = m.IdAnio AND cp.IdSede = m.IdSede AND cp.IdNivel = m.IdNivel AND cp.IdJornada = m.IdJornada AND cp.IdCurso = m.IdCurso AND "
                    + " cp.IdParalelo = m.IdParalelo LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Materia AS mc with (nolock) ON mc.IdEmpresa = m.IdEmpresa AND mc.IdAnio = m.IdAnio AND mc.IdSede = m.IdSede AND mc.IdNivel = m.IdNivel AND mc.IdJornada = m.IdJornada AND mc.IdCurso = m.IdCurso AND "
                    + " mc.IdMateria = mco.IdMateria LEFT OUTER JOIN "
                    + " dbo.aca_Alumno AS alu with (nolock) ON m.IdEmpresa = alu.IdEmpresa AND m.IdAlumno = alu.IdAlumno LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pa with (nolock) ON alu.IdPersona = pa.IdPersona LEFT OUTER JOIN "
                    + " dbo.aca_Profesor AS pro with (nolock) ON pro.IdEmpresa = m.IdEmpresa AND pro.IdProfesor = cp.IdProfesorTutor LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pp with (nolock) ON pro.IdPersona = pp.IdPersona LEFT OUTER JOIN "
                    + " (SELECT IdEmpresa, IdMatricula "
                    + " FROM      dbo.aca_AlumnoRetiro AS r with (nolock) "
                    + " WHERE(Estado = 1)) AS ret ON m.IdEmpresa = ret.IdEmpresa AND m.IdMatricula = ret.IdMatricula "
                    + " WHERE mco.IdEmpresa = @IdEmpresa "
                    + " and m.IdAnio = @IdAnio "
                    + " and m.IdSede = @IdSede "
                    + " and m.IdNivel = @IdNivel "
                    + " and m.IdJornada = @IdJornada "
                    + " and m.IdCurso = @IdCurso "
                    + " and m.IdParalelo = @IdParalelo "
                    + " and m.IdAlumno = case when @IdAlumno = 0 then m.IdAlumno else @IdAlumno end "
                    + " and isnull(ret.IdMatricula, 0) = case when @MostrarRetirados = 1 then isnull(ret.IdMatricula, 0) else 0 end "
                    + " and mc.PromediarGrupo = 0 "
                    + " and mc.IdCatalogoTipoCalificacion = 40 "
                    + " ) "
                    + " UNION ALL "
                    + " ( "
                    + " /*REMEDIAL*/ "
                    + " SELECT m.IdEmpresa, m.IdMatricula, m.IdAnio, m.IdSede, m.IdNivel, m.IdJornada, m.IdCurso, m.IdParalelo, m.IdAlumno, AN.Descripcion, sn.NomSede, nj.NomJornada, nj.OrdenJornada, sn.NomNivel, sn.OrdenNivel, jc.NomCurso, jc.OrdenCurso, "
                    + " cp.NomParalelo, cp.OrdenParalelo, pa.pe_nombreCompleto AS NombreAlumno, mc.IdMateria, mc.NomMateria, mc.NomMateriaGrupo, mc.OrdenMateria, mc.OrdenMateriaGrupo, mc.PromediarGrupo, mc.IdCatalogoTipoCalificacion, "
                    + " CAST(mco.ExamenRemedial AS varchar) AS Calificacion, CAST(mco.ExamenRemedial AS numeric(18, 2)) AS CalificacionNumerica, 'REMEDIAL' AS Columna, 6 AS OrdenColumna, pp.pe_nombreCompleto AS NombreTutor "
                    + " FROM     dbo.aca_Matricula AS m with (nolock) INNER JOIN "
                    + " dbo.aca_MatriculaCalificacion AS mco with (nolock) ON m.IdEmpresa = mco.IdEmpresa AND m.IdMatricula = mco.IdMatricula LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo AS AN with (nolock) ON m.IdEmpresa = AN.IdEmpresa AND m.IdAnio = AN.IdAnio LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Sede_NivelAcademico AS sn with (nolock) ON sn.IdEmpresa = m.IdEmpresa AND sn.IdSede = m.IdSede AND sn.IdAnio = m.IdAnio AND sn.IdNivel = m.IdNivel LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_NivelAcademico_Jornada AS nj with (nolock) ON nj.IdEmpresa = m.IdEmpresa AND nj.IdAnio = m.IdAnio AND nj.IdSede = m.IdSede AND nj.IdNivel = m.IdNivel AND nj.IdJornada = m.IdJornada LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Jornada_Curso AS jc with (nolock) ON jc.IdEmpresa = m.IdEmpresa AND jc.IdAnio = m.IdAnio AND jc.IdSede = m.IdSede AND jc.IdNivel = m.IdNivel AND jc.IdJornada = m.IdJornada AND jc.IdCurso = m.IdCurso LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Paralelo AS cp with (nolock) ON cp.IdEmpresa = m.IdEmpresa AND cp.IdAnio = m.IdAnio AND cp.IdSede = m.IdSede AND cp.IdNivel = m.IdNivel AND cp.IdJornada = m.IdJornada AND cp.IdCurso = m.IdCurso AND "
                    + " cp.IdParalelo = m.IdParalelo LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Materia AS mc with (nolock) ON mc.IdEmpresa = m.IdEmpresa AND mc.IdAnio = m.IdAnio AND mc.IdSede = m.IdSede AND mc.IdNivel = m.IdNivel AND mc.IdJornada = m.IdJornada AND mc.IdCurso = m.IdCurso AND "
                    + " mc.IdMateria = mco.IdMateria LEFT OUTER JOIN "
                    + " dbo.aca_Alumno AS alu with (nolock) ON m.IdEmpresa = alu.IdEmpresa AND m.IdAlumno = alu.IdAlumno LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pa with (nolock) ON alu.IdPersona = pa.IdPersona LEFT OUTER JOIN "
                    + " dbo.aca_Profesor AS pro with (nolock) ON pro.IdEmpresa = m.IdEmpresa AND pro.IdProfesor = cp.IdProfesorTutor LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pp with (nolock) ON pro.IdPersona = pp.IdPersona LEFT OUTER JOIN "
                    + " (SELECT IdEmpresa, IdMatricula "
                    + " FROM      dbo.aca_AlumnoRetiro AS r with (nolock) "
                    + " WHERE(Estado = 1)) AS ret ON m.IdEmpresa = ret.IdEmpresa AND m.IdMatricula = ret.IdMatricula "
                    + " WHERE mco.IdEmpresa = @IdEmpresa "
                    + " and m.IdAnio = @IdAnio "
                    + " and m.IdSede = @IdSede "
                    + " and m.IdNivel = @IdNivel "
                    + " and m.IdJornada = @IdJornada "
                    + " and m.IdCurso = @IdCurso "
                    + " and m.IdParalelo = @IdParalelo "
                    + " and m.IdAlumno = case when @IdAlumno = 0 then m.IdAlumno else @IdAlumno end "
                    + " and isnull(ret.IdMatricula, 0) = case when @MostrarRetirados = 1 then isnull(ret.IdMatricula, 0) else 0 end "
                    + " and mc.PromediarGrupo = 0 "
                    + " and mc.IdCatalogoTipoCalificacion = 40 "
                    + " ) "
                    + " UNION ALL "
                    + " ( "
                    + " /*GRACIA*/ "
                    + " SELECT m.IdEmpresa, m.IdMatricula, m.IdAnio, m.IdSede, m.IdNivel, m.IdJornada, m.IdCurso, m.IdParalelo, m.IdAlumno, AN.Descripcion, sn.NomSede, nj.NomJornada, nj.OrdenJornada, sn.NomNivel, sn.OrdenNivel, jc.NomCurso, jc.OrdenCurso, "
                    + " cp.NomParalelo, cp.OrdenParalelo, pa.pe_nombreCompleto AS NombreAlumno, mc.IdMateria, mc.NomMateria, mc.NomMateriaGrupo, mc.OrdenMateria, mc.OrdenMateriaGrupo, mc.PromediarGrupo, mc.IdCatalogoTipoCalificacion, "
                    + " CAST(mco.ExamenGracia AS varchar) AS Calificacion, CAST(mco.ExamenGracia AS numeric(18, 2)) AS CalificacionNumerica, 'GRACIA' AS Columna, 7 AS OrdenColumna, pp.pe_nombreCompleto AS NombreTutor "
                    + " FROM     dbo.aca_Matricula AS m with (nolock) INNER JOIN "
                    + " dbo.aca_MatriculaCalificacion AS mco with (nolock) ON m.IdEmpresa = mco.IdEmpresa AND m.IdMatricula = mco.IdMatricula LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo AS AN ON m.IdEmpresa = AN.IdEmpresa AND m.IdAnio = AN.IdAnio LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Sede_NivelAcademico AS sn with (nolock) ON sn.IdEmpresa = m.IdEmpresa AND sn.IdSede = m.IdSede AND sn.IdAnio = m.IdAnio AND sn.IdNivel = m.IdNivel LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_NivelAcademico_Jornada AS nj with (nolock) ON nj.IdEmpresa = m.IdEmpresa AND nj.IdAnio = m.IdAnio AND nj.IdSede = m.IdSede AND nj.IdNivel = m.IdNivel AND nj.IdJornada = m.IdJornada LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Jornada_Curso AS jc with (nolock) ON jc.IdEmpresa = m.IdEmpresa AND jc.IdAnio = m.IdAnio AND jc.IdSede = m.IdSede AND jc.IdNivel = m.IdNivel AND jc.IdJornada = m.IdJornada AND jc.IdCurso = m.IdCurso LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Paralelo AS cp with (nolock) ON cp.IdEmpresa = m.IdEmpresa AND cp.IdAnio = m.IdAnio AND cp.IdSede = m.IdSede AND cp.IdNivel = m.IdNivel AND cp.IdJornada = m.IdJornada AND cp.IdCurso = m.IdCurso AND "
                    + " cp.IdParalelo = m.IdParalelo LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Materia AS mc with (nolock) ON mc.IdEmpresa = m.IdEmpresa AND mc.IdAnio = m.IdAnio AND mc.IdSede = m.IdSede AND mc.IdNivel = m.IdNivel AND mc.IdJornada = m.IdJornada AND mc.IdCurso = m.IdCurso AND "
                    + " mc.IdMateria = mco.IdMateria LEFT OUTER JOIN "
                    + " dbo.aca_Alumno AS alu with (nolock) ON m.IdEmpresa = alu.IdEmpresa AND m.IdAlumno = alu.IdAlumno LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pa with (nolock) ON alu.IdPersona = pa.IdPersona LEFT OUTER JOIN "
                    + " dbo.aca_Profesor AS pro with (nolock) ON pro.IdEmpresa = m.IdEmpresa AND pro.IdProfesor = cp.IdProfesorTutor LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pp with (nolock) ON pro.IdPersona = pp.IdPersona LEFT OUTER JOIN "
                    + " (SELECT IdEmpresa, IdMatricula "
                    + " FROM      dbo.aca_AlumnoRetiro AS r with (nolock) "
                    + " WHERE(Estado = 1)) AS ret ON m.IdEmpresa = ret.IdEmpresa AND m.IdMatricula = ret.IdMatricula "
                    + " WHERE mco.IdEmpresa = @IdEmpresa "
                    + " and m.IdAnio = @IdAnio "
                    + " and m.IdSede = @IdSede "
                    + " and m.IdNivel = @IdNivel "
                    + " and m.IdJornada = @IdJornada "
                    + " and m.IdCurso = @IdCurso "
                    + " and m.IdParalelo = @IdParalelo "
                    + " and m.IdAlumno = case when @IdAlumno = 0 then m.IdAlumno else @IdAlumno end "
                    + " and isnull(ret.IdMatricula, 0) = case when @MostrarRetirados = 1 then isnull(ret.IdMatricula, 0) else 0 end "
                    + " and mc.PromediarGrupo = 0 "
                    + " and mc.IdCatalogoTipoCalificacion = 40 "
                    + " ) "
                    + " UNION ALL "
                    + " ( "
                    + " /*PROMEDIO FINAL*/ "
                    + " SELECT m.IdEmpresa, m.IdMatricula, m.IdAnio, m.IdSede, m.IdNivel, m.IdJornada, m.IdCurso, m.IdParalelo, m.IdAlumno, AN.Descripcion, sn.NomSede, nj.NomJornada, nj.OrdenJornada, sn.NomNivel, sn.OrdenNivel, jc.NomCurso, jc.OrdenCurso, "
                    + " cp.NomParalelo, cp.OrdenParalelo, pa.pe_nombreCompleto AS NombreAlumno, mc.IdMateria, mc.NomMateria, mc.NomMateriaGrupo, mc.OrdenMateria, mc.OrdenMateriaGrupo, mc.PromediarGrupo, mc.IdCatalogoTipoCalificacion, "
                    + " CAST(mco.PromedioFinal AS varchar) AS Calificacion, CAST(mco.PromedioFinal AS numeric(18, 2)) AS CalificacionNumerica, 'PROMEDIO FINAL' AS Columna, 7 AS OrdenColumna, pp.pe_nombreCompleto AS NombreTutor "
                    + " FROM     dbo.aca_Matricula AS m with (nolock) INNER JOIN "
                    + " dbo.aca_MatriculaCalificacion AS mco with (nolock) ON m.IdEmpresa = mco.IdEmpresa AND m.IdMatricula = mco.IdMatricula LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo AS AN with (nolock) ON m.IdEmpresa = AN.IdEmpresa AND m.IdAnio = AN.IdAnio LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Sede_NivelAcademico AS sn with (nolock) ON sn.IdEmpresa = m.IdEmpresa AND sn.IdSede = m.IdSede AND sn.IdAnio = m.IdAnio AND sn.IdNivel = m.IdNivel LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_NivelAcademico_Jornada AS nj with (nolock) ON nj.IdEmpresa = m.IdEmpresa AND nj.IdAnio = m.IdAnio AND nj.IdSede = m.IdSede AND nj.IdNivel = m.IdNivel AND nj.IdJornada = m.IdJornada LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Jornada_Curso AS jc with (nolock) ON jc.IdEmpresa = m.IdEmpresa AND jc.IdAnio = m.IdAnio AND jc.IdSede = m.IdSede AND jc.IdNivel = m.IdNivel AND jc.IdJornada = m.IdJornada AND jc.IdCurso = m.IdCurso LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Paralelo AS cp with (nolock) ON cp.IdEmpresa = m.IdEmpresa AND cp.IdAnio = m.IdAnio AND cp.IdSede = m.IdSede AND cp.IdNivel = m.IdNivel AND cp.IdJornada = m.IdJornada AND cp.IdCurso = m.IdCurso AND "
                    + " cp.IdParalelo = m.IdParalelo LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Materia AS mc with (nolock) ON mc.IdEmpresa = m.IdEmpresa AND mc.IdAnio = m.IdAnio AND mc.IdSede = m.IdSede AND mc.IdNivel = m.IdNivel AND mc.IdJornada = m.IdJornada AND mc.IdCurso = m.IdCurso AND "
                    + " mc.IdMateria = mco.IdMateria LEFT OUTER JOIN "
                    + " dbo.aca_Alumno AS alu with (nolock) ON m.IdEmpresa = alu.IdEmpresa AND m.IdAlumno = alu.IdAlumno LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pa with (nolock) ON alu.IdPersona = pa.IdPersona LEFT OUTER JOIN "
                    + " dbo.aca_Profesor AS pro with (nolock) ON pro.IdEmpresa = m.IdEmpresa AND pro.IdProfesor = cp.IdProfesorTutor LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pp with (nolock) ON pro.IdPersona = pp.IdPersona LEFT OUTER JOIN "
                    + " (SELECT IdEmpresa, IdMatricula "
                    + " FROM      dbo.aca_AlumnoRetiro AS r with (nolock) "
                    + " WHERE(Estado = 1)) AS ret ON m.IdEmpresa = ret.IdEmpresa AND m.IdMatricula = ret.IdMatricula "
                    + " WHERE mco.IdEmpresa = @IdEmpresa "
                    + " and m.IdAnio = @IdAnio "
                    + " and m.IdSede = @IdSede "
                    + " and m.IdNivel = @IdNivel "
                    + " and m.IdJornada = @IdJornada "
                    + " and m.IdCurso = @IdCurso "
                    + " and m.IdParalelo = @IdParalelo "
                    + " and m.IdAlumno = case when @IdAlumno = 0 then m.IdAlumno else @IdAlumno end "
                    + " and isnull(ret.IdMatricula, 0) = case when @MostrarRetirados = 1 then isnull(ret.IdMatricula, 0) else 0 end "
                    + " and mc.PromediarGrupo = 0 "
                    + " and mc.IdCatalogoTipoCalificacion = 40 "
                    + " ) "
                    + " UNION ALL "
                    + " ( "
                    + " /*MATERIAS QUE SE PROMEDIAN*/ "
                    + " /*I QUIMESTRE*/ "
                    + " SELECT m.IdEmpresa, m.IdMatricula, m.IdAnio, m.IdSede, m.IdNivel, m.IdJornada, m.IdCurso, m.IdParalelo, m.IdAlumno, AN.Descripcion, sn.NomSede, nj.NomJornada, nj.OrdenJornada, sn.NomNivel, sn.OrdenNivel, jc.NomCurso, jc.OrdenCurso, "
                    + " cp.NomParalelo, cp.OrdenParalelo, pa.pe_nombreCompleto AS NombreAlumno, mc.IdMateria, mc.NomMateria, mc.NomMateriaGrupo, mc.OrdenMateria, mc.OrdenMateriaGrupo, mc.PromediarGrupo, mc.IdCatalogoTipoCalificacion, "
                    + " CAST(mco.PromedioFinalQ1 AS varchar) AS Calificacion, CAST(mco.PromedioFinalQ1 AS numeric(18, 2)) AS CalificacionNumerica, 'I QUIMESTRE' AS Columna, 1 AS OrdenColumna, pp.pe_nombreCompleto AS NombreTutor "
                    + " FROM     dbo.aca_Matricula AS m with (nolock) INNER JOIN "
                    + " dbo.aca_MatriculaCalificacion AS mco with (nolock) ON m.IdEmpresa = mco.IdEmpresa AND m.IdMatricula = mco.IdMatricula LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo AS AN with (nolock) ON m.IdEmpresa = AN.IdEmpresa AND m.IdAnio = AN.IdAnio LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Sede_NivelAcademico AS sn with (nolock) ON sn.IdEmpresa = m.IdEmpresa AND sn.IdSede = m.IdSede AND sn.IdAnio = m.IdAnio AND sn.IdNivel = m.IdNivel LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_NivelAcademico_Jornada AS nj with (nolock) ON nj.IdEmpresa = m.IdEmpresa AND nj.IdAnio = m.IdAnio AND nj.IdSede = m.IdSede AND nj.IdNivel = m.IdNivel AND nj.IdJornada = m.IdJornada LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Jornada_Curso AS jc with (nolock) ON jc.IdEmpresa = m.IdEmpresa AND jc.IdAnio = m.IdAnio AND jc.IdSede = m.IdSede AND jc.IdNivel = m.IdNivel AND jc.IdJornada = m.IdJornada AND jc.IdCurso = m.IdCurso LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Paralelo AS cp with (nolock) ON cp.IdEmpresa = m.IdEmpresa AND cp.IdAnio = m.IdAnio AND cp.IdSede = m.IdSede AND cp.IdNivel = m.IdNivel AND cp.IdJornada = m.IdJornada AND cp.IdCurso = m.IdCurso AND "
                    + " cp.IdParalelo = m.IdParalelo LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Materia AS mc with (nolock) ON mc.IdEmpresa = m.IdEmpresa AND mc.IdAnio = m.IdAnio AND mc.IdSede = m.IdSede AND mc.IdNivel = m.IdNivel AND mc.IdJornada = m.IdJornada AND mc.IdCurso = m.IdCurso AND "
                    + " mc.IdMateria = mco.IdMateria LEFT OUTER JOIN "
                    + " dbo.aca_Alumno AS alu with (nolock) ON m.IdEmpresa = alu.IdEmpresa AND m.IdAlumno = alu.IdAlumno LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pa with (nolock) ON alu.IdPersona = pa.IdPersona LEFT OUTER JOIN "
                    + " dbo.aca_Profesor AS pro with (nolock) ON pro.IdEmpresa = m.IdEmpresa AND pro.IdProfesor = cp.IdProfesorTutor LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pp with (nolock) ON pro.IdPersona = pp.IdPersona LEFT OUTER JOIN "
                    + " (SELECT IdEmpresa, IdMatricula "
                    + " FROM      dbo.aca_AlumnoRetiro AS r with (nolock) "
                    + " WHERE(Estado = 1)) AS ret ON m.IdEmpresa = ret.IdEmpresa AND m.IdMatricula = ret.IdMatricula "
                    + " WHERE mco.IdEmpresa = @IdEmpresa "
                    + " and m.IdAnio = @IdAnio "
                    + " and m.IdSede = @IdSede "
                    + " and m.IdNivel = @IdNivel "
                    + " and m.IdJornada = @IdJornada "
                    + " and m.IdCurso = @IdCurso "
                    + " and m.IdParalelo = @IdParalelo "
                    + " and m.IdAlumno = case when @IdAlumno = 0 then m.IdAlumno else @IdAlumno end "
                    + " and isnull(ret.IdMatricula, 0) = case when @MostrarRetirados = 1 then isnull(ret.IdMatricula, 0) else 0 end "
                    + " and mc.PromediarGrupo = 1 "
                    + " and mc.IdCatalogoTipoCalificacion = 40 "
                    + " ) "
                    + " UNION ALL "
                    + " ( "
                    + " /*II QUIMESTRE*/ "
                    + " SELECT m.IdEmpresa, m.IdMatricula, m.IdAnio, m.IdSede, m.IdNivel, m.IdJornada, m.IdCurso, m.IdParalelo, m.IdAlumno, AN.Descripcion, sn.NomSede, nj.NomJornada, nj.OrdenJornada, sn.NomNivel, sn.OrdenNivel, jc.NomCurso, jc.OrdenCurso, "
                    + " cp.NomParalelo, cp.OrdenParalelo, pa.pe_nombreCompleto AS NombreAlumno, mc.IdMateria, mc.NomMateria, mc.NomMateriaGrupo, mc.OrdenMateria, mc.OrdenMateriaGrupo, mc.PromediarGrupo, mc.IdCatalogoTipoCalificacion, "
                    + " CAST(mco.PromedioFinalQ2 AS varchar) AS Calificacion, CAST(mco.PromedioFinalQ2 AS numeric(18, 2)) AS CalificacionNumerica, 'II QUIMESTRE' AS Columna, 2 AS OrdenColumna, pp.pe_nombreCompleto AS NombreTutor "
                    + " FROM     dbo.aca_Matricula AS m with (nolock) INNER JOIN "
                    + " dbo.aca_MatriculaCalificacion AS mco with (nolock) ON m.IdEmpresa = mco.IdEmpresa AND m.IdMatricula = mco.IdMatricula LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo AS AN with (nolock) ON m.IdEmpresa = AN.IdEmpresa AND m.IdAnio = AN.IdAnio LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Sede_NivelAcademico AS sn with (nolock) ON sn.IdEmpresa = m.IdEmpresa AND sn.IdSede = m.IdSede AND sn.IdAnio = m.IdAnio AND sn.IdNivel = m.IdNivel LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_NivelAcademico_Jornada AS nj with (nolock) ON nj.IdEmpresa = m.IdEmpresa AND nj.IdAnio = m.IdAnio AND nj.IdSede = m.IdSede AND nj.IdNivel = m.IdNivel AND nj.IdJornada = m.IdJornada LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Jornada_Curso AS jc with (nolock) ON jc.IdEmpresa = m.IdEmpresa AND jc.IdAnio = m.IdAnio AND jc.IdSede = m.IdSede AND jc.IdNivel = m.IdNivel AND jc.IdJornada = m.IdJornada AND jc.IdCurso = m.IdCurso LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Paralelo AS cp with (nolock) ON cp.IdEmpresa = m.IdEmpresa AND cp.IdAnio = m.IdAnio AND cp.IdSede = m.IdSede AND cp.IdNivel = m.IdNivel AND cp.IdJornada = m.IdJornada AND cp.IdCurso = m.IdCurso AND "
                    + " cp.IdParalelo = m.IdParalelo LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Materia AS mc with (nolock) ON mc.IdEmpresa = m.IdEmpresa AND mc.IdAnio = m.IdAnio AND mc.IdSede = m.IdSede AND mc.IdNivel = m.IdNivel AND mc.IdJornada = m.IdJornada AND mc.IdCurso = m.IdCurso AND "
                    + " mc.IdMateria = mco.IdMateria LEFT OUTER JOIN "
                    + " dbo.aca_Alumno AS alu with (nolock) ON m.IdEmpresa = alu.IdEmpresa AND m.IdAlumno = alu.IdAlumno LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pa with (nolock) ON alu.IdPersona = pa.IdPersona LEFT OUTER JOIN "
                    + " dbo.aca_Profesor AS pro with (nolock) ON pro.IdEmpresa = m.IdEmpresa AND pro.IdProfesor = cp.IdProfesorTutor LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pp with (nolock) ON pro.IdPersona = pp.IdPersona LEFT OUTER JOIN "
                    + " (SELECT IdEmpresa, IdMatricula "
                    + " FROM      dbo.aca_AlumnoRetiro AS r with (nolock) "
                    + " WHERE(Estado = 1)) AS ret ON m.IdEmpresa = ret.IdEmpresa AND m.IdMatricula = ret.IdMatricula "
                    + " WHERE mco.IdEmpresa = @IdEmpresa "
                    + " and m.IdAnio = @IdAnio "
                    + " and m.IdSede = @IdSede "
                    + " and m.IdNivel = @IdNivel "
                    + " and m.IdJornada = @IdJornada "
                    + " and m.IdCurso = @IdCurso "
                    + " and m.IdParalelo = @IdParalelo "
                    + " and m.IdAlumno = case when @IdAlumno = 0 then m.IdAlumno else @IdAlumno end "
                    + " and isnull(ret.IdMatricula, 0) = case when @MostrarRetirados = 1 then isnull(ret.IdMatricula, 0) else 0 end "
                    + " and mc.PromediarGrupo = 1 "
                    + " and mc.IdCatalogoTipoCalificacion = 40 "
                    + " ) "
                    + " UNION ALL "
                    + " ( "
                    + " /*PROMEDIO DE LOS 2 QUIMESTRES*/ "
                    + " SELECT m.IdEmpresa, m.IdMatricula, m.IdAnio, m.IdSede, m.IdNivel, m.IdJornada, m.IdCurso, m.IdParalelo, m.IdAlumno, AN.Descripcion, sn.NomSede, nj.NomJornada, nj.OrdenJornada, sn.NomNivel, sn.OrdenNivel, jc.NomCurso, jc.OrdenCurso, "
                    + " cp.NomParalelo, cp.OrdenParalelo, pa.pe_nombreCompleto AS NombreAlumno, mc.IdMateria, mc.NomMateria, mc.NomMateriaGrupo, mc.OrdenMateria, mc.OrdenMateriaGrupo, mc.PromediarGrupo, mc.IdCatalogoTipoCalificacion, "
                    + " CAST(mco.PromedioQuimestres AS varchar) AS Calificacion, CAST(mco.PromedioQuimestres AS numeric(18, 2)) AS CalificacionNumerica, 'PROMEDIO' AS Columna, 3 AS OrdenColumna, pp.pe_nombreCompleto AS NombreTutor "
                    + " FROM     dbo.aca_Matricula AS m with (nolock) INNER JOIN "
                    + " dbo.aca_MatriculaCalificacion AS mco with (nolock) ON m.IdEmpresa = mco.IdEmpresa AND m.IdMatricula = mco.IdMatricula LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo AS AN with (nolock) ON m.IdEmpresa = AN.IdEmpresa AND m.IdAnio = AN.IdAnio LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Sede_NivelAcademico AS sn with (nolock) ON sn.IdEmpresa = m.IdEmpresa AND sn.IdSede = m.IdSede AND sn.IdAnio = m.IdAnio AND sn.IdNivel = m.IdNivel LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_NivelAcademico_Jornada AS nj with (nolock) ON nj.IdEmpresa = m.IdEmpresa AND nj.IdAnio = m.IdAnio AND nj.IdSede = m.IdSede AND nj.IdNivel = m.IdNivel AND nj.IdJornada = m.IdJornada LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Jornada_Curso AS jc with (nolock) ON jc.IdEmpresa = m.IdEmpresa AND jc.IdAnio = m.IdAnio AND jc.IdSede = m.IdSede AND jc.IdNivel = m.IdNivel AND jc.IdJornada = m.IdJornada AND jc.IdCurso = m.IdCurso LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Paralelo AS cp with (nolock) ON cp.IdEmpresa = m.IdEmpresa AND cp.IdAnio = m.IdAnio AND cp.IdSede = m.IdSede AND cp.IdNivel = m.IdNivel AND cp.IdJornada = m.IdJornada AND cp.IdCurso = m.IdCurso AND "
                    + " cp.IdParalelo = m.IdParalelo LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Materia AS mc with (nolock) ON mc.IdEmpresa = m.IdEmpresa AND mc.IdAnio = m.IdAnio AND mc.IdSede = m.IdSede AND mc.IdNivel = m.IdNivel AND mc.IdJornada = m.IdJornada AND mc.IdCurso = m.IdCurso AND "
                    + " mc.IdMateria = mco.IdMateria LEFT OUTER JOIN "
                    + " dbo.aca_Alumno AS alu with (nolock) ON m.IdEmpresa = alu.IdEmpresa AND m.IdAlumno = alu.IdAlumno LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pa with (nolock) ON alu.IdPersona = pa.IdPersona LEFT OUTER JOIN "
                    + " dbo.aca_Profesor AS pro with (nolock) ON pro.IdEmpresa = m.IdEmpresa AND pro.IdProfesor = cp.IdProfesorTutor LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pp with (nolock) ON pro.IdPersona = pp.IdPersona LEFT OUTER JOIN "
                    + " (SELECT IdEmpresa, IdMatricula "
                    + " FROM      dbo.aca_AlumnoRetiro AS r with (nolock) "
                    + " WHERE(Estado = 1)) AS ret ON m.IdEmpresa = ret.IdEmpresa AND m.IdMatricula = ret.IdMatricula "
                    + " WHERE mco.IdEmpresa = @IdEmpresa "
                    + " and m.IdAnio = @IdAnio "
                    + " and m.IdSede = @IdSede "
                    + " and m.IdNivel = @IdNivel "
                    + " and m.IdJornada = @IdJornada "
                    + " and m.IdCurso = @IdCurso "
                    + " and m.IdParalelo = @IdParalelo "
                    + " and m.IdAlumno = case when @IdAlumno = 0 then m.IdAlumno else @IdAlumno end "
                    + " and isnull(ret.IdMatricula, 0) = case when @MostrarRetirados = 1 then isnull(ret.IdMatricula, 0) else 0 end "
                    + " and mc.PromediarGrupo = 1 "
                    + " and mc.IdCatalogoTipoCalificacion = 40 "
                    + " ) "
                    + " UNION ALL "
                    + " ( "
                    + " /*MEJORAMIENTO*/ "
                    + " SELECT m.IdEmpresa, m.IdMatricula, m.IdAnio, m.IdSede, m.IdNivel, m.IdJornada, m.IdCurso, m.IdParalelo, m.IdAlumno, AN.Descripcion, sn.NomSede, nj.NomJornada, nj.OrdenJornada, sn.NomNivel, sn.OrdenNivel, jc.NomCurso, jc.OrdenCurso, "
                    + " cp.NomParalelo, cp.OrdenParalelo, pa.pe_nombreCompleto AS NombreAlumno, mc.IdMateria, mc.NomMateria, mc.NomMateriaGrupo, mc.OrdenMateria, mc.OrdenMateriaGrupo, mc.PromediarGrupo, mc.IdCatalogoTipoCalificacion, "
                    + " CAST(mco.ExamenMejoramiento AS varchar) AS Calificacion, CAST(mco.ExamenMejoramiento AS numeric(18, 2)) AS CalificacionNumerica, 'MEJORAMIENTO' AS Columna, 4 AS OrdenColumna, pp.pe_nombreCompleto AS NombreTutor "
                    + " FROM     dbo.aca_Matricula AS m with (nolock) INNER JOIN "
                    + " dbo.aca_MatriculaCalificacion AS mco with (nolock) ON m.IdEmpresa = mco.IdEmpresa AND m.IdMatricula = mco.IdMatricula LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo AS AN with (nolock) ON m.IdEmpresa = AN.IdEmpresa AND m.IdAnio = AN.IdAnio LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Sede_NivelAcademico AS sn with (nolock) ON sn.IdEmpresa = m.IdEmpresa AND sn.IdSede = m.IdSede AND sn.IdAnio = m.IdAnio AND sn.IdNivel = m.IdNivel LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_NivelAcademico_Jornada AS nj with (nolock) ON nj.IdEmpresa = m.IdEmpresa AND nj.IdAnio = m.IdAnio AND nj.IdSede = m.IdSede AND nj.IdNivel = m.IdNivel AND nj.IdJornada = m.IdJornada LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Jornada_Curso AS jc with (nolock) ON jc.IdEmpresa = m.IdEmpresa AND jc.IdAnio = m.IdAnio AND jc.IdSede = m.IdSede AND jc.IdNivel = m.IdNivel AND jc.IdJornada = m.IdJornada AND jc.IdCurso = m.IdCurso LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Paralelo AS cp with (nolock) ON cp.IdEmpresa = m.IdEmpresa AND cp.IdAnio = m.IdAnio AND cp.IdSede = m.IdSede AND cp.IdNivel = m.IdNivel AND cp.IdJornada = m.IdJornada AND cp.IdCurso = m.IdCurso AND "
                    + " cp.IdParalelo = m.IdParalelo LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Materia AS mc with (nolock) ON mc.IdEmpresa = m.IdEmpresa AND mc.IdAnio = m.IdAnio AND mc.IdSede = m.IdSede AND mc.IdNivel = m.IdNivel AND mc.IdJornada = m.IdJornada AND mc.IdCurso = m.IdCurso AND "
                    + " mc.IdMateria = mco.IdMateria LEFT OUTER JOIN "
                    + " dbo.aca_Alumno AS alu with (nolock) ON m.IdEmpresa = alu.IdEmpresa AND m.IdAlumno = alu.IdAlumno LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pa with (nolock) ON alu.IdPersona = pa.IdPersona LEFT OUTER JOIN "
                    + " dbo.aca_Profesor AS pro with (nolock) ON pro.IdEmpresa = m.IdEmpresa AND pro.IdProfesor = cp.IdProfesorTutor LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pp with (nolock) ON pro.IdPersona = pp.IdPersona LEFT OUTER JOIN "
                    + " (SELECT IdEmpresa, IdMatricula "
                    + " FROM      dbo.aca_AlumnoRetiro AS r with (nolock) "
                    + " WHERE(Estado = 1)) AS ret ON m.IdEmpresa = ret.IdEmpresa AND m.IdMatricula = ret.IdMatricula "
                    + " WHERE mco.IdEmpresa = @IdEmpresa "
                    + " and m.IdAnio = @IdAnio "
                    + " and m.IdSede = @IdSede "
                    + " and m.IdNivel = @IdNivel "
                    + " and m.IdJornada = @IdJornada "
                    + " and m.IdCurso = @IdCurso "
                    + " and m.IdParalelo = @IdParalelo "
                    + " and m.IdAlumno = case when @IdAlumno = 0 then m.IdAlumno else @IdAlumno end "
                    + " and isnull(ret.IdMatricula, 0) = case when @MostrarRetirados = 1 then isnull(ret.IdMatricula, 0) else 0 end "
                    + " and mc.PromediarGrupo = 1 "
                    + " and mc.IdCatalogoTipoCalificacion = 40 "
                    + " ) "
                    + " UNION ALL "
                    + " ( "
                    + " /*SUPLETORIO*/ "
                    + " SELECT m.IdEmpresa, m.IdMatricula, m.IdAnio, m.IdSede, m.IdNivel, m.IdJornada, m.IdCurso, m.IdParalelo, m.IdAlumno, AN.Descripcion, sn.NomSede, nj.NomJornada, nj.OrdenJornada, sn.NomNivel, sn.OrdenNivel, jc.NomCurso, jc.OrdenCurso, "
                    + " cp.NomParalelo, cp.OrdenParalelo, pa.pe_nombreCompleto AS NombreAlumno, mc.IdMateria, mc.NomMateria, mc.NomMateriaGrupo, mc.OrdenMateria, mc.OrdenMateriaGrupo, mc.PromediarGrupo, mc.IdCatalogoTipoCalificacion, "
                    + " CAST(mco.ExamenSupletorio AS varchar) AS Calificacion, CAST(mco.ExamenSupletorio AS numeric(18, 2)) AS CalificacionNumerica, 'SUPLETORIO' AS Columna, 5 AS OrdenColumna, pp.pe_nombreCompleto AS NombreTutor "
                    + " FROM     dbo.aca_Matricula AS m with (nolock) INNER JOIN "
                    + " dbo.aca_MatriculaCalificacion AS mco with (nolock) ON m.IdEmpresa = mco.IdEmpresa AND m.IdMatricula = mco.IdMatricula LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo AS AN with (nolock) ON m.IdEmpresa = AN.IdEmpresa AND m.IdAnio = AN.IdAnio LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Sede_NivelAcademico AS sn with (nolock) ON sn.IdEmpresa = m.IdEmpresa AND sn.IdSede = m.IdSede AND sn.IdAnio = m.IdAnio AND sn.IdNivel = m.IdNivel LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_NivelAcademico_Jornada AS nj with (nolock) ON nj.IdEmpresa = m.IdEmpresa AND nj.IdAnio = m.IdAnio AND nj.IdSede = m.IdSede AND nj.IdNivel = m.IdNivel AND nj.IdJornada = m.IdJornada LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Jornada_Curso AS jc with (nolock) ON jc.IdEmpresa = m.IdEmpresa AND jc.IdAnio = m.IdAnio AND jc.IdSede = m.IdSede AND jc.IdNivel = m.IdNivel AND jc.IdJornada = m.IdJornada AND jc.IdCurso = m.IdCurso LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Paralelo AS cp with (nolock) ON cp.IdEmpresa = m.IdEmpresa AND cp.IdAnio = m.IdAnio AND cp.IdSede = m.IdSede AND cp.IdNivel = m.IdNivel AND cp.IdJornada = m.IdJornada AND cp.IdCurso = m.IdCurso AND "
                    + " cp.IdParalelo = m.IdParalelo LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Materia AS mc with (nolock) ON mc.IdEmpresa = m.IdEmpresa AND mc.IdAnio = m.IdAnio AND mc.IdSede = m.IdSede AND mc.IdNivel = m.IdNivel AND mc.IdJornada = m.IdJornada AND mc.IdCurso = m.IdCurso AND "
                    + " mc.IdMateria = mco.IdMateria LEFT OUTER JOIN "
                    + " dbo.aca_Alumno AS alu with (nolock) ON m.IdEmpresa = alu.IdEmpresa AND m.IdAlumno = alu.IdAlumno LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pa with (nolock) ON alu.IdPersona = pa.IdPersona LEFT OUTER JOIN "
                    + " dbo.aca_Profesor AS pro with (nolock) ON pro.IdEmpresa = m.IdEmpresa AND pro.IdProfesor = cp.IdProfesorTutor LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pp with (nolock) ON pro.IdPersona = pp.IdPersona LEFT OUTER JOIN "
                    + " (SELECT IdEmpresa, IdMatricula "
                    + " FROM      dbo.aca_AlumnoRetiro AS r with (nolock) "
                    + " WHERE(Estado = 1)) AS ret ON m.IdEmpresa = ret.IdEmpresa AND m.IdMatricula = ret.IdMatricula "
                    + " WHERE mco.IdEmpresa = @IdEmpresa "
                    + " and m.IdAnio = @IdAnio "
                    + " and m.IdSede = @IdSede "
                    + " and m.IdNivel = @IdNivel "
                    + " and m.IdJornada = @IdJornada "
                    + " and m.IdCurso = @IdCurso "
                    + " and m.IdParalelo = @IdParalelo "
                    + " and m.IdAlumno = case when @IdAlumno = 0 then m.IdAlumno else @IdAlumno end "
                    + " and isnull(ret.IdMatricula, 0) = case when @MostrarRetirados = 1 then isnull(ret.IdMatricula, 0) else 0 end "
                    + " and mc.PromediarGrupo = 1 "
                    + " and mc.IdCatalogoTipoCalificacion = 40 "
                    + " ) "
                    + " UNION ALL "
                    + " ( "
                    + " /*REMEDIAL*/ "
                    + " SELECT m.IdEmpresa, m.IdMatricula, m.IdAnio, m.IdSede, m.IdNivel, m.IdJornada, m.IdCurso, m.IdParalelo, m.IdAlumno, AN.Descripcion, sn.NomSede, nj.NomJornada, nj.OrdenJornada, sn.NomNivel, sn.OrdenNivel, jc.NomCurso, jc.OrdenCurso, "
                    + " cp.NomParalelo, cp.OrdenParalelo, pa.pe_nombreCompleto AS NombreAlumno, mc.IdMateria, mc.NomMateria, mc.NomMateriaGrupo, mc.OrdenMateria, mc.OrdenMateriaGrupo, mc.PromediarGrupo, mc.IdCatalogoTipoCalificacion, "
                    + " CAST(mco.ExamenRemedial AS varchar) AS Calificacion, CAST(mco.ExamenRemedial AS numeric(18, 2)) AS CalificacionNumerica, 'REMEDIAL' AS Columna, 6 AS OrdenColumna, pp.pe_nombreCompleto AS NombreTutor "
                    + " FROM     dbo.aca_Matricula AS m with (nolock) INNER JOIN "
                    + " dbo.aca_MatriculaCalificacion AS mco with (nolock) ON m.IdEmpresa = mco.IdEmpresa AND m.IdMatricula = mco.IdMatricula LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo AS AN with (nolock) ON m.IdEmpresa = AN.IdEmpresa AND m.IdAnio = AN.IdAnio LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Sede_NivelAcademico AS sn with (nolock) ON sn.IdEmpresa = m.IdEmpresa AND sn.IdSede = m.IdSede AND sn.IdAnio = m.IdAnio AND sn.IdNivel = m.IdNivel LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_NivelAcademico_Jornada AS nj with (nolock) ON nj.IdEmpresa = m.IdEmpresa AND nj.IdAnio = m.IdAnio AND nj.IdSede = m.IdSede AND nj.IdNivel = m.IdNivel AND nj.IdJornada = m.IdJornada LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Jornada_Curso AS jc with (nolock) ON jc.IdEmpresa = m.IdEmpresa AND jc.IdAnio = m.IdAnio AND jc.IdSede = m.IdSede AND jc.IdNivel = m.IdNivel AND jc.IdJornada = m.IdJornada AND jc.IdCurso = m.IdCurso LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Paralelo AS cp with (nolock) ON cp.IdEmpresa = m.IdEmpresa AND cp.IdAnio = m.IdAnio AND cp.IdSede = m.IdSede AND cp.IdNivel = m.IdNivel AND cp.IdJornada = m.IdJornada AND cp.IdCurso = m.IdCurso AND "
                    + " cp.IdParalelo = m.IdParalelo LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Materia AS mc with (nolock) ON mc.IdEmpresa = m.IdEmpresa AND mc.IdAnio = m.IdAnio AND mc.IdSede = m.IdSede AND mc.IdNivel = m.IdNivel AND mc.IdJornada = m.IdJornada AND mc.IdCurso = m.IdCurso AND "
                    + " mc.IdMateria = mco.IdMateria LEFT OUTER JOIN "
                    + " dbo.aca_Alumno AS alu with (nolock) ON m.IdEmpresa = alu.IdEmpresa AND m.IdAlumno = alu.IdAlumno LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pa with (nolock) ON alu.IdPersona = pa.IdPersona LEFT OUTER JOIN "
                    + " dbo.aca_Profesor AS pro with (nolock) ON pro.IdEmpresa = m.IdEmpresa AND pro.IdProfesor = cp.IdProfesorTutor LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pp with (nolock) ON pro.IdPersona = pp.IdPersona LEFT OUTER JOIN "
                    + " (SELECT IdEmpresa, IdMatricula "
                    + " FROM      dbo.aca_AlumnoRetiro AS r with (nolock) "
                    + " WHERE(Estado = 1)) AS ret ON m.IdEmpresa = ret.IdEmpresa AND m.IdMatricula = ret.IdMatricula "
                    + " WHERE mco.IdEmpresa = @IdEmpresa "
                    + " and m.IdAnio = @IdAnio "
                    + " and m.IdSede = @IdSede "
                    + " and m.IdNivel = @IdNivel "
                    + " and m.IdJornada = @IdJornada "
                    + " and m.IdCurso = @IdCurso "
                    + " and m.IdParalelo = @IdParalelo "
                    + " and m.IdAlumno = case when @IdAlumno = 0 then m.IdAlumno else @IdAlumno end "
                    + " and isnull(ret.IdMatricula, 0) = case when @MostrarRetirados = 1 then isnull(ret.IdMatricula, 0) else 0 end "
                    + " and mc.PromediarGrupo = 1 "
                    + " and mc.IdCatalogoTipoCalificacion = 40 "
                    + " ) "
                    + " UNION ALL "
                    + " ( "
                    + " /*GRACIA*/ "
                    + " SELECT m.IdEmpresa, m.IdMatricula, m.IdAnio, m.IdSede, m.IdNivel, m.IdJornada, m.IdCurso, m.IdParalelo, m.IdAlumno, AN.Descripcion, sn.NomSede, nj.NomJornada, nj.OrdenJornada, sn.NomNivel, sn.OrdenNivel, jc.NomCurso, jc.OrdenCurso, "
                    + " cp.NomParalelo, cp.OrdenParalelo, pa.pe_nombreCompleto AS NombreAlumno, mc.IdMateria, mc.NomMateria, mc.NomMateriaGrupo, mc.OrdenMateria, mc.OrdenMateriaGrupo, mc.PromediarGrupo, mc.IdCatalogoTipoCalificacion, "
                    + " CAST(mco.ExamenGracia AS varchar) AS Calificacion, CAST(mco.ExamenGracia AS numeric(18, 2)) AS CalificacionNumerica, 'GRACIA' AS Columna, 7 AS OrdenColumna, pp.pe_nombreCompleto AS NombreTutor "
                    + " FROM     dbo.aca_Matricula AS m with (nolock) INNER JOIN "
                    + " dbo.aca_MatriculaCalificacion AS mco with (nolock) ON m.IdEmpresa = mco.IdEmpresa AND m.IdMatricula = mco.IdMatricula LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo AS AN with (nolock) ON m.IdEmpresa = AN.IdEmpresa AND m.IdAnio = AN.IdAnio LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Sede_NivelAcademico AS sn with (nolock) ON sn.IdEmpresa = m.IdEmpresa AND sn.IdSede = m.IdSede AND sn.IdAnio = m.IdAnio AND sn.IdNivel = m.IdNivel LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_NivelAcademico_Jornada AS nj with (nolock) ON nj.IdEmpresa = m.IdEmpresa AND nj.IdAnio = m.IdAnio AND nj.IdSede = m.IdSede AND nj.IdNivel = m.IdNivel AND nj.IdJornada = m.IdJornada LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Jornada_Curso AS jc with (nolock) ON jc.IdEmpresa = m.IdEmpresa AND jc.IdAnio = m.IdAnio AND jc.IdSede = m.IdSede AND jc.IdNivel = m.IdNivel AND jc.IdJornada = m.IdJornada AND jc.IdCurso = m.IdCurso LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Paralelo AS cp with (nolock) ON cp.IdEmpresa = m.IdEmpresa AND cp.IdAnio = m.IdAnio AND cp.IdSede = m.IdSede AND cp.IdNivel = m.IdNivel AND cp.IdJornada = m.IdJornada AND cp.IdCurso = m.IdCurso AND "
                    + " cp.IdParalelo = m.IdParalelo LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Materia AS mc with (nolock) ON mc.IdEmpresa = m.IdEmpresa AND mc.IdAnio = m.IdAnio AND mc.IdSede = m.IdSede AND mc.IdNivel = m.IdNivel AND mc.IdJornada = m.IdJornada AND mc.IdCurso = m.IdCurso AND "
                    + " mc.IdMateria = mco.IdMateria LEFT OUTER JOIN "
                    + " dbo.aca_Alumno AS alu with (nolock) ON m.IdEmpresa = alu.IdEmpresa AND m.IdAlumno = alu.IdAlumno LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pa with (nolock) ON alu.IdPersona = pa.IdPersona LEFT OUTER JOIN "
                    + " dbo.aca_Profesor AS pro with (nolock) ON pro.IdEmpresa = m.IdEmpresa AND pro.IdProfesor = cp.IdProfesorTutor LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pp with (nolock) ON pro.IdPersona = pp.IdPersona LEFT OUTER JOIN "
                    + " (SELECT IdEmpresa, IdMatricula "
                    + " FROM      dbo.aca_AlumnoRetiro AS r with (nolock) "
                    + " WHERE(Estado = 1)) AS ret ON m.IdEmpresa = ret.IdEmpresa AND m.IdMatricula = ret.IdMatricula "
                    + " WHERE mco.IdEmpresa = @IdEmpresa "
                    + " and m.IdAnio = @IdAnio "
                    + " and m.IdSede = @IdSede "
                    + " and m.IdNivel = @IdNivel "
                    + " and m.IdJornada = @IdJornada "
                    + " and m.IdCurso = @IdCurso "
                    + " and m.IdParalelo = @IdParalelo "
                    + " and m.IdAlumno = case when @IdAlumno = 0 then m.IdAlumno else @IdAlumno end "
                    + " and isnull(ret.IdMatricula, 0) = case when @MostrarRetirados = 1 then isnull(ret.IdMatricula, 0) else 0 end "
                    + " and mc.PromediarGrupo = 1 "
                    + " and mc.IdCatalogoTipoCalificacion = 40 "
                    + " ) "
                    + " UNION ALL "
                    + " ( "
                    + " /*PROMEDIO FINAL*/ "
                    + " SELECT m.IdEmpresa, m.IdMatricula, m.IdAnio, m.IdSede, m.IdNivel, m.IdJornada, m.IdCurso, m.IdParalelo, m.IdAlumno, AN.Descripcion, sn.NomSede, nj.NomJornada, nj.OrdenJornada, sn.NomNivel, sn.OrdenNivel, jc.NomCurso, jc.OrdenCurso, "
                    + " cp.NomParalelo, cp.OrdenParalelo, pa.pe_nombreCompleto AS NombreAlumno, mc.IdMateria, mc.NomMateria, mc.NomMateriaGrupo, mc.OrdenMateria, mc.OrdenMateriaGrupo, mc.PromediarGrupo, mc.IdCatalogoTipoCalificacion, "
                    + " CAST(mco.PromedioFinal AS varchar) AS Calificacion, CAST(mco.PromedioFinal AS numeric(18, 2)) AS CalificacionNumerica, 'PROMEDIO FINAL' AS Columna, 7 AS OrdenColumna, pp.pe_nombreCompleto AS NombreTutor "
                    + " FROM     dbo.aca_Matricula AS m with (nolock) INNER JOIN "
                    + " dbo.aca_MatriculaCalificacion AS mco with (nolock) ON m.IdEmpresa = mco.IdEmpresa AND m.IdMatricula = mco.IdMatricula LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo AS AN with (nolock) ON m.IdEmpresa = AN.IdEmpresa AND m.IdAnio = AN.IdAnio LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Sede_NivelAcademico AS sn with (nolock) ON sn.IdEmpresa = m.IdEmpresa AND sn.IdSede = m.IdSede AND sn.IdAnio = m.IdAnio AND sn.IdNivel = m.IdNivel LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_NivelAcademico_Jornada AS nj with (nolock) ON nj.IdEmpresa = m.IdEmpresa AND nj.IdAnio = m.IdAnio AND nj.IdSede = m.IdSede AND nj.IdNivel = m.IdNivel AND nj.IdJornada = m.IdJornada LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Jornada_Curso AS jc with (nolock) ON jc.IdEmpresa = m.IdEmpresa AND jc.IdAnio = m.IdAnio AND jc.IdSede = m.IdSede AND jc.IdNivel = m.IdNivel AND jc.IdJornada = m.IdJornada AND jc.IdCurso = m.IdCurso LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Paralelo AS cp with (nolock) ON cp.IdEmpresa = m.IdEmpresa AND cp.IdAnio = m.IdAnio AND cp.IdSede = m.IdSede AND cp.IdNivel = m.IdNivel AND cp.IdJornada = m.IdJornada AND cp.IdCurso = m.IdCurso AND "
                    + " cp.IdParalelo = m.IdParalelo LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Materia AS mc with (nolock) ON mc.IdEmpresa = m.IdEmpresa AND mc.IdAnio = m.IdAnio AND mc.IdSede = m.IdSede AND mc.IdNivel = m.IdNivel AND mc.IdJornada = m.IdJornada AND mc.IdCurso = m.IdCurso AND "
                    + " mc.IdMateria = mco.IdMateria LEFT OUTER JOIN "
                    + " dbo.aca_Alumno AS alu with (nolock) ON m.IdEmpresa = alu.IdEmpresa AND m.IdAlumno = alu.IdAlumno LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pa with (nolock) ON alu.IdPersona = pa.IdPersona LEFT OUTER JOIN "
                    + " dbo.aca_Profesor AS pro with (nolock) ON pro.IdEmpresa = m.IdEmpresa AND pro.IdProfesor = cp.IdProfesorTutor LEFT OUTER JOIN "
                    + " dbo.tb_persona AS pp with (nolock) ON pro.IdPersona = pp.IdPersona LEFT OUTER JOIN "
                    + " (SELECT IdEmpresa, IdMatricula "
                    + " FROM      dbo.aca_AlumnoRetiro AS r with (nolock) "
                    + " WHERE(Estado = 1)) AS ret ON m.IdEmpresa = ret.IdEmpresa AND m.IdMatricula = ret.IdMatricula "
                    + " WHERE mco.IdEmpresa = @IdEmpresa "
                    + " and m.IdAnio = @IdAnio "
                    + " and m.IdSede = @IdSede "
                    + " and m.IdNivel = @IdNivel "
                    + " and m.IdJornada = @IdJornada "
                    + " and m.IdCurso = @IdCurso "
                    + " and m.IdParalelo = @IdParalelo "
                    + " and m.IdAlumno = case when @IdAlumno = 0 then m.IdAlumno else @IdAlumno end "
                    + " and isnull(ret.IdMatricula, 0) = case when @MostrarRetirados = 1 then isnull(ret.IdMatricula, 0) else 0 end "
                    + " and mc.PromediarGrupo = 1 "
                    + " and mc.IdCatalogoTipoCalificacion = 40 "
                    + " ) ";
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 0;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new ACA_034_General_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdMatricula = Convert.ToDecimal(reader["IdMatricula"]),
                            IdMateria = string.IsNullOrEmpty(reader["IdMateria"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdMateria"]),
                            IdAlumno = Convert.ToDecimal(reader["IdAlumno"]),
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
                        });
                    }
                    reader.Close();
                }

                ListaObligatorias = Lista.Where(q => q.PromediarGrupo == 0 && q.IdCatalogoTipoCalificacion == Convert.ToInt32(cl_enumeradores.eCatalogoTipoCalificacion.CUANTI)).ToList();
                var ListaObligatorias_ValidaCalificaciones = ListaObligatorias.Where(q => q.Columna == "PROMEDIO FINAL").ToList();
                ListaObligatorias_ValidaCalificaciones.ForEach(q => q.NoTieneCalificacion = (q.Calificacion == null ? 1 : 0));
                ListaFinal.AddRange(ListaObligatorias);

                ListaComplementarias = Lista.Where(q => q.PromediarGrupo == 1 && q.IdMateria != null && q.IdCatalogoTipoCalificacion == Convert.ToInt32(cl_enumeradores.eCatalogoTipoCalificacion.CUANTI)).ToList();
                var ListaComplementarias_ValidaCalificaciones = ListaComplementarias.Where(q => q.Columna == "PROMEDIO FINAL").ToList();
                var ListaComplementarias_IQuim = ListaComplementarias.Where(q => q.Columna == "I QUIMESTRE").ToList();
                var ListaComplementarias_IIQuim = ListaComplementarias.Where(q => q.Columna == "II QUIMESTRE").ToList();
                var ListaComplementarias_PromedioQuimestre = ListaComplementarias.Where(q => q.Columna == "PROMEDIO").ToList();
                var ListaComplementarias_Mejoramiento = ListaComplementarias.Where(q => q.Columna == "MEJORAMIENTO").ToList();
                var ListaComplementarias_PromedioMejoramiento = ListaComplementarias.Where(q => q.Columna == "PROMEDIO MEJORAMIENTO").ToList();
                var ListaComplementarias_Supletorio = ListaComplementarias.Where(q => q.Columna == "SUPLETORIO").ToList();
                var ListaComplementarias_Remedial = ListaComplementarias.Where(q => q.Columna == "REMEDIAL").ToList();
                var ListaComplementarias_Gracia = ListaComplementarias.Where(q => q.Columna == "GRACIA").ToList();
                var ListaComplementarias_PromedioFinal = ListaComplementarias.Where(q => q.Columna == "PROMEDIO FINAL").ToList();

                ListaComplementarias_IQuim.ForEach(q => q.NoTieneCalificacion = (q.Calificacion == null ? 1 : 0));
                ListaComplementarias_IIQuim.ForEach(q => q.NoTieneCalificacion = (q.Calificacion == null ? 1 : 0));
                ListaComplementarias_PromedioQuimestre.ForEach(q => q.NoTieneCalificacion = (q.Calificacion == null ? 1 : 0));
                ListaComplementarias_Mejoramiento.ForEach(q => q.NoTieneCalificacion = (q.Calificacion == null ? 1 : 0));
                ListaComplementarias_Supletorio.ForEach(q => q.NoTieneCalificacion = (q.Calificacion == null ? 1 : 0));
                ListaComplementarias_Remedial.ForEach(q => q.NoTieneCalificacion = (q.Calificacion == null ? 1 : 0));
                ListaComplementarias_Gracia.ForEach(q => q.NoTieneCalificacion = (q.Calificacion == null ? 1 : 0));
                ListaComplementarias_PromedioFinal.ForEach(q => q.NoTieneCalificacion = (q.Calificacion == null ? 1 : 0));

                ListaFinal.AddRange(ListaComplementarias_IQuim);
                ListaFinal.AddRange(ListaComplementarias_IIQuim);
                ListaFinal.AddRange(ListaComplementarias_PromedioQuimestre);
                ListaFinal.AddRange(ListaComplementarias_Mejoramiento);
                ListaFinal.AddRange(ListaComplementarias_Supletorio);
                ListaFinal.AddRange(ListaComplementarias_Remedial);
                ListaFinal.AddRange(ListaComplementarias_Gracia);
                ListaFinal.AddRange(ListaComplementarias_PromedioFinal);

                var ListaPromediar = ListaFinal.Where(q => q.Columna == "PROMEDIO FINAL").ToList();
                var ListaPromedioGeneral = ListaPromediar.GroupBy(q => new
                {
                    q.IdEmpresa,
                    q.IdMatricula,
                    q.IdAlumno,
                    q.NombreAlumno,
                    q.Codigo,
                    q.IdAnio,
                    q.IdSede,
                    q.IdJornada,
                    q.IdCurso,
                    q.IdParalelo,
                    q.IdNivel,
                    q.Descripcion,
                    q.NomSede,
                    q.NomNivel,
                    q.NomJornada,
                    q.NomCurso,
                    q.NomParalelo,
                    q.CodigoParalelo,
                    q.OrdenNivel,
                    q.OrdenJornada,
                    q.OrdenCurso,
                    q.OrdenParalelo,
                }).Select(q => new ACA_034_General_Info
                {
                    IdEmpresa = q.Key.IdEmpresa,
                    IdMatricula = q.Key.IdMatricula,
                    IdAlumno = q.Key.IdAlumno,
                    NombreAlumno = q.Key.NombreAlumno,
                    Codigo = q.Key.Codigo,
                    IdAnio = q.Key.IdAnio,
                    IdSede = q.Key.IdSede,
                    IdJornada = q.Key.IdJornada,
                    IdCurso = q.Key.IdCurso,
                    IdParalelo = q.Key.IdParalelo,
                    IdNivel = q.Key.IdNivel,
                    Descripcion = q.Key.Descripcion,
                    NomSede = q.Key.NomSede,
                    NomNivel = q.Key.NomNivel,
                    NomJornada = q.Key.NomJornada,
                    NomCurso = q.Key.NomCurso,
                    NomParalelo = q.Key.NomParalelo,
                    CodigoParalelo = q.Key.CodigoParalelo,
                    OrdenNivel = q.Key.OrdenNivel,
                    OrdenJornada = q.Key.OrdenJornada,
                    OrdenCurso = q.Key.OrdenCurso,
                    OrdenParalelo = q.Key.OrdenParalelo,
                    NoTieneCalificacion = q.Sum(g => g.NoTieneCalificacion),
                    SumaGeneral = q.Sum(g => Convert.ToDecimal(g.Calificacion)),
                    PromedioCalculado = q.Max(g => g.Calificacion) == null ? (decimal?)null : q.Sum(g => Convert.ToDecimal(g.Calificacion)) / q.Count(g => !string.IsNullOrEmpty(g.Calificacion))
                }).ToList();
                ListaPromedioGeneral.ForEach(q => { q.PromedioCalculado = (q.NoTieneCalificacion == 0 ? q.PromedioCalculado : (decimal?)null); q.SumaGeneral = (q.NoTieneCalificacion == 0 ? q.SumaGeneral : (decimal?)null); });

                
                var lst_suma_general = new List<ACA_034_General_Info>();
                var lst_promedio_general = new List<ACA_034_General_Info>();
                var lst_equivalencia_general = new List<ACA_034_General_Info>();
                foreach (var item in ListaPromedioGeneral)
                {
                    lst_promedio_general.Add( new ACA_034_General_Info
                    {
                        IdEmpresa = item.IdEmpresa,
                        IdMatricula = item.IdMatricula,
                        IdMateria = 0,
                        IdAlumno = item.IdAlumno,
                        NombreAlumno = item.NombreAlumno,
                        Codigo = item.Codigo,
                        IdAnio = item.IdAnio,
                        IdSede = item.IdSede,
                        IdJornada = item.IdJornada,
                        IdCurso = item.IdCurso,
                        IdParalelo = item.IdParalelo,
                        IdNivel = item.IdNivel,
                        Descripcion = item.Descripcion,
                        NomSede = item.NomSede,
                        NomNivel = item.NomNivel,
                        NomJornada = item.NomJornada,
                        NomCurso = item.NomCurso,
                        NomParalelo = item.NomParalelo,
                        CodigoParalelo = item.CodigoParalelo,
                        OrdenNivel = item.OrdenNivel,
                        OrdenJornada = item.OrdenJornada,
                        OrdenCurso = item.OrdenCurso,
                        OrdenParalelo = item.OrdenParalelo,
                        Calificacion = (item.SumaGeneral == null ? null : Convert.ToString(Math.Round(Convert.ToDecimal(item.SumaGeneral), 2, MidpointRounding.AwayFromZero))),
                        CalificacionNumerica = (item.SumaGeneral == null ? (decimal?)null : Math.Round(Convert.ToDecimal(item.SumaGeneral), 2, MidpointRounding.AwayFromZero)),
                        IdCatalogoTipoCalificacion = null,
                        Columna = "SUMA TOTAL",
                        NombreGrupo = "SUMA TOTAL",
                        NombreMateria = "",
                        OrdenGrupo = 999,
                        OrdenMateria = 999,
                        OrdenColumna = 1,
                        PromediarGrupo = 0
                    });

                    lst_promedio_general.Add(new ACA_034_General_Info
                    {
                        IdEmpresa = item.IdEmpresa,
                        IdMatricula = item.IdMatricula,
                        IdMateria = 0,
                        IdAlumno = item.IdAlumno,
                        NombreAlumno = item.NombreAlumno,
                        Codigo = item.Codigo,
                        IdAnio = item.IdAnio,
                        IdSede = item.IdSede,
                        IdJornada = item.IdJornada,
                        IdCurso = item.IdCurso,
                        IdParalelo = item.IdParalelo,
                        IdNivel = item.IdNivel,
                        Descripcion = item.Descripcion,
                        NomSede = item.NomSede,
                        NomNivel = item.NomNivel,
                        NomJornada = item.NomJornada,
                        NomCurso = item.NomCurso,
                        NomParalelo = item.NomParalelo,
                        CodigoParalelo = item.CodigoParalelo,
                        OrdenNivel = item.OrdenNivel,
                        OrdenJornada = item.OrdenJornada,
                        OrdenCurso = item.OrdenCurso,
                        OrdenParalelo = item.OrdenParalelo,
                        Calificacion = (item.PromedioCalculado==null ? null : Convert.ToString(Math.Round(Convert.ToDecimal(item.PromedioCalculado), 2 ,MidpointRounding.AwayFromZero))),
                        CalificacionNumerica = (item.PromedioCalculado == null ? (decimal?)null : Math.Round(Convert.ToDecimal(item.PromedioCalculado), 2, MidpointRounding.AwayFromZero)),
                        IdCatalogoTipoCalificacion = null,
                        Columna = "PROMEDIO GENERAL",
                        NombreGrupo = "PROMEDIO GENERAL",
                        NombreMateria = "",
                        OrdenGrupo = 9999,
                        OrdenMateria = 9999,
                        OrdenColumna = 1,
                        PromediarGrupo = 0
                    });

                    var PromedioGeneral = (item.PromedioCalculado == null ? (decimal?)null : item.PromedioCalculado);
                    var info_equivalencia = odata_equivalencia.getInfo_x_Promedio(IdEmpresa, IdAnio, PromedioGeneral);
                    lst_promedio_general.Add(new ACA_034_General_Info
                    {
                        IdEmpresa = item.IdEmpresa,
                        IdMatricula = item.IdMatricula,
                        IdMateria = 0,
                        IdAlumno = item.IdAlumno,
                        NombreAlumno = item.NombreAlumno,
                        Codigo = item.Codigo,
                        IdAnio = item.IdAnio,
                        IdSede = item.IdSede,
                        IdJornada = item.IdJornada,
                        IdCurso = item.IdCurso,
                        IdParalelo = item.IdParalelo,
                        IdNivel = item.IdNivel,
                        Descripcion = item.Descripcion,
                        NomSede = item.NomSede,
                        NomNivel = item.NomNivel,
                        NomJornada = item.NomJornada,
                        NomCurso = item.NomCurso,
                        NomParalelo = item.NomParalelo,
                        CodigoParalelo = item.CodigoParalelo,
                        OrdenNivel = item.OrdenNivel,
                        OrdenJornada = item.OrdenJornada,
                        OrdenCurso = item.OrdenCurso,
                        OrdenParalelo = item.OrdenParalelo,
                        Calificacion = (info_equivalencia == null ? null : info_equivalencia.Codigo),
                        CalificacionNumerica = (decimal?)null,
                        IdCatalogoTipoCalificacion = null,
                        Columna = "EQUIVALENCIA",
                        NombreGrupo = "EQUIVALENCIA",
                        NombreMateria = "",
                        OrdenGrupo = 99999999,
                        OrdenMateria = 99999999,
                        OrdenColumna = 1,
                        PromediarGrupo = 0
                    });
                }
                ListaFinal.AddRange(lst_promedio_general);
                Lista_Comportamiento_Proyectos = Lista.Where(q => q.IdMateria == 0).ToList();
                ListaFinal.AddRange(Lista_Comportamiento_Proyectos);

                #region Secuencial
                var lstAlumnos = ListaFinal.GroupBy(q => new { q.IdAlumno, q.NombreAlumno }).Select(q => new ACA_034_Info
                {
                    IdAlumno = q.Key.IdAlumno,
                    NombreAlumno = q.Key.NombreAlumno,
                    Secuencial = 0
                }).OrderBy(q => q.NombreAlumno).ToList();

                int Secuencial = 1;
                foreach (var item in lstAlumnos)
                {
                    item.Secuencial = Secuencial++;
                }
                ListaFinal = (from a in ListaFinal
                              join b in lstAlumnos
                              on a.IdAlumno equals b.IdAlumno
                              select new ACA_034_General_Info
                              {
                                  IdEmpresa = a.IdEmpresa,
                                  IdAnio = a.IdAnio,
                                  IdSede = a.IdSede,
                                  IdNivel = a.IdNivel,
                                  IdJornada = a.IdJornada,
                                  IdCurso = a.IdCurso,
                                  IdParalelo = a.IdParalelo,
                                  IdAlumno = a.IdAlumno,
                                  IdMatricula = a.IdMatricula,
                                  IdMateria = a.IdMateria,
                                  NombreMateria = a.NombreMateria,
                                  NombreGrupo = a.NombreGrupo,
                                  OrdenMateria = a.OrdenMateria,
                                  OrdenGrupo = a.OrdenGrupo,
                                  PromediarGrupo = a.PromediarGrupo,
                                  IdCatalogoTipoCalificacion = a.IdCatalogoTipoCalificacion,
                                  Codigo = a.Codigo,
                                  NombreAlumno = a.NombreAlumno,
                                  Descripcion = a.Descripcion,
                                  NomSede = a.NomSede,
                                  NomNivel = a.NomNivel,
                                  OrdenNivel = a.OrdenNivel,
                                  NomJornada = a.NomJornada,
                                  OrdenJornada = a.OrdenJornada,
                                  NomCurso = a.NomCurso,
                                  OrdenCurso = a.OrdenCurso,
                                  CodigoParalelo = a.CodigoParalelo,
                                  NomParalelo = a.NomParalelo,
                                  OrdenParalelo = a.OrdenParalelo,
                                  NombreTutor = a.NombreTutor,
                                  Calificacion = a.Calificacion,
                                  CalificacionNumerica = a.CalificacionNumerica,
                                  Columna = a.Columna,
                                  OrdenColumna = a.OrdenColumna,
                                  PromedioCalculado = a.PromedioCalculado,
                                  SupletorioCalculado = a.SupletorioCalculado,
                                  PromedioFinalCalculado = a.PromedioFinalCalculado,
                                  SumaGeneral = a.SumaGeneral,
                                  NoTieneCalificacion = a.NoTieneCalificacion,
                                  Secuencial = b.Secuencial
                              }).ToList();
                #endregion

                return ListaFinal;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
