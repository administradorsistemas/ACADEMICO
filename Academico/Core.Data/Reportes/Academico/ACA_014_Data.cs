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
    public class ACA_014_Data
    {
        aca_AnioLectivo_Data odata_anio = new aca_AnioLectivo_Data();
        public List<ACA_014_Info> get_list(int IdEmpresa, int IdAnio, int IdSede, int IdNivel, int IdJornada, int IdCurso, int IdParalelo, int IdCatalogoParcial, decimal IdAlumno, bool MostrarRetirados, bool MostrarPromedios)
        {
            try
            {
                var info_anio = odata_anio.getInfo(IdEmpresa, IdAnio);
                double PromedioMinimoParcial = (info_anio==null ? 0 : info_anio.PromedioMinimoPromocion==null ? 0 : Convert.ToDouble(info_anio.PromedioMinimoPromocion));
                List<ACA_014_Info> Lista = new List<ACA_014_Info>();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();

                    #region Query
                    string query = "DECLARE @IdEmpresa int =" + IdEmpresa.ToString() + ", @IdAnio int = " + IdAnio.ToString() + ", @IdSede int = " + IdSede.ToString() + ", @IdNivel int = " + IdNivel.ToString() + ", @IdJornada int = " + IdJornada.ToString() + ", @IdCurso int= " + IdCurso.ToString() + ", @IdParalelo int = " + IdParalelo.ToString() + ", @IdAlumno numeric = " + IdAlumno.ToString() + " , @MostrarRetirados bit = " + (MostrarRetirados == false ? 0 : 1)
                    + " SELECT mc.IdEmpresa, m.IdAnio, m.IdSede, m.IdNivel, m.IdJornada, m.IdCurso, m.IdParalelo, m.IdAlumno, mc.IdMatricula, mc.IdMateria, "
                    + " cm.NomMateria, cm.NomMateriaArea, cm.NomMateriaGrupo, cm.EsObligatorio, cm.OrdenMateriaArea, cm.OrdenMateriaGrupo, cm.PromediarGrupo, "
                    + " cm.OrdenMateria, cm.IdCatalogoTipoCalificacion, alu.Codigo, p.pe_nombreCompleto, AN.Descripcion, sn.NomSede, sn.NomNivel, sn.OrdenNivel, "
                    + " nj.NomJornada, nj.OrdenJornada, jc.NomCurso, jc.OrdenCurso, cp.CodigoParalelo, cp.NomParalelo, cp.OrdenParalelo, "
                    + " pins.pe_nombreCompleto as NombreInspector, pre.pe_nombreCompleto as NombreRepresentante, "
                    + " cast(mc.CalificacionP1 as varchar) CalificacionP1, ep1.Codigo AS EquivalenciaPromedioP1, "
                    + " cast(mc.CalificacionP2 as varchar) CalificacionP2, ep2.Codigo AS EquivalenciaPromedioP2, "
                    + " cast(mc.CalificacionP3 as varchar) CalificacionP3, ep3.Codigo AS EquivalenciaPromedioP3, "
                    + " cast(dbo.BankersRounding((mc.PromedioQ1 * 0.80), 2) as varchar) PorcentajePromedioQ1, "
                    + " cast(dbo.BankersRounding((mc.ExamenQ1 * 0.20), 2) as varchar) PorcentajeExamenQ1, "
                    + " cast(mc.ExamenQ1 as varchar) ExamenQ1, epEQ1.Codigo AS EquivalenciaPromedioEQ1, "
                    + " cast(mc.PromedioFinalQ1 as varchar) PromedioFinalQ1, epQ1.Codigo AS EquivalenciaPromedioQ1, "
                    + " cast(mc.CalificacionP4 as varchar) CalificacionP4, ep4.Codigo AS EquivalenciaPromedioP4, "
                    + " cast(mc.CalificacionP5 as varchar) CalificacionP5, ep5.Codigo AS EquivalenciaPromedioP5, "
                    + " cast(mc.CalificacionP6 as varchar) CalificacionP6, ep6.Codigo AS EquivalenciaPromedioP6, "
                    + " cast(dbo.BankersRounding((mc.PromedioQ2 * 0.80), 2) as varchar) PorcentajePromedioQ2, "
                    + " cast(dbo.BankersRounding((mc.ExamenQ2 * 0.20), 2) as varchar) PorcentajeExamenQ2, "
                    + " cast(mc.ExamenQ2 as varchar) ExamenQ2, epEQ2.Codigo AS EquivalenciaPromedioEQ2, "
                    + " cast(mc.PromedioFinalQ2 as varchar) PromedioFinalQ2, epQ2.Codigo AS EquivalenciaPromedioQ2, "
                    + " cast(mc.PromedioQuimestres as varchar) as PromedioQuimestres_PF, "
                    + " case when(mc.ExamenMejoramiento is not null) then cast(mc.PromedioFinal as varchar) "
                    + " else  null end as Promedio_PR, "
                    + " cast(mc.ExamenMejoramiento as varchar) ExamenMejoramiento, "
                    + " cast(mc.CampoMejoramiento as varchar) CampoMejoramiento, "
                    + " cast(mc.ExamenSupletorio as varchar) ExamenSupletorio, "
                    + " cast(mc.ExamenRemedial as varchar) ExamenRemedial, "
                    + " cast(mc.ExamenGracia as varchar) ExamenGracia, "
                    + " cast(mc.PromedioFinal as varchar) PromedioFinal, mc.IdEquivalenciaPromedioPF, epPF.Codigo AS EquivalenciaPromedioPF "
                    + " FROM     dbo.aca_MatriculaCalificacion AS mc WITH (nolock) INNER JOIN "
                    + " dbo.aca_Matricula AS m WITH (nolock) ON mc.IdEmpresa = m.IdEmpresa AND mc.IdMatricula = m.IdMatricula INNER JOIN "
                   + " dbo.aca_Alumno AS alu WITH (nolock) ON m.IdEmpresa = alu.IdEmpresa AND m.IdAlumno = alu.IdAlumno INNER JOIN "
                   + " dbo.tb_persona AS p WITH (nolock) ON alu.IdPersona = p.IdPersona INNER JOIN "
                   + " dbo.aca_AnioLectivo_Curso_Materia AS cm WITH (nolock) ON m.IdEmpresa = cm.IdEmpresa AND m.IdAnio = cm.IdAnio AND m.IdSede = cm.IdSede AND "
                   + " m.IdNivel = cm.IdNivel AND m.IdJornada = cm.IdJornada AND m.IdCurso = cm.IdCurso AND "
                   + " mc.IdMateria = cm.IdMateria "
                   + " LEFT OUTER JOIN dbo.aca_AnioLectivoEquivalenciaPromedio AS ep1 WITH (nolock) ON m.IdAnio = ep1.IdAnio AND mc.IdEmpresa = ep1.IdEmpresa AND mc.IdEquivalenciaPromedioP1 = ep1.IdEquivalenciaPromedio "
                   + " LEFT OUTER JOIN dbo.aca_AnioLectivoEquivalenciaPromedio AS ep2 WITH (nolock) ON m.IdAnio = ep2.IdAnio AND mc.IdEmpresa = ep2.IdEmpresa AND mc.IdEquivalenciaPromedioP2 = ep2.IdEquivalenciaPromedio "
                   + " LEFT OUTER JOIN dbo.aca_AnioLectivoEquivalenciaPromedio AS ep3 WITH (nolock) ON m.IdAnio = ep3.IdAnio AND mc.IdEmpresa = ep3.IdEmpresa AND mc.IdEquivalenciaPromedioP3 = ep3.IdEquivalenciaPromedio "
                   + " LEFT OUTER JOIN dbo.aca_AnioLectivoEquivalenciaPromedio AS epEQ1 WITH (nolock) ON m.IdAnio = epEQ1.IdAnio AND mc.IdEmpresa = epEQ1.IdEmpresa AND mc.IdEquivalenciaPromedioEQ1 = epEQ1.IdEquivalenciaPromedio "
                   + " LEFT OUTER JOIN dbo.aca_AnioLectivoEquivalenciaPromedio AS epQ1 WITH (nolock) ON m.IdAnio = epQ1.IdAnio AND mc.IdEmpresa = epQ1.IdEmpresa AND mc.IdEquivalenciaPromedioQ1 = epQ1.IdEquivalenciaPromedio "
                   + " LEFT OUTER JOIN dbo.aca_AnioLectivoEquivalenciaPromedio AS ep4 WITH (nolock) ON m.IdAnio = ep4.IdAnio AND mc.IdEmpresa = ep4.IdEmpresa AND mc.IdEquivalenciaPromedioP4 = ep4.IdEquivalenciaPromedio "
                   + " LEFT OUTER JOIN dbo.aca_AnioLectivoEquivalenciaPromedio AS ep5 WITH (nolock) ON m.IdAnio = ep5.IdAnio AND mc.IdEmpresa = ep5.IdEmpresa AND mc.IdEquivalenciaPromedioP5 = ep5.IdEquivalenciaPromedio "
                   + " LEFT OUTER JOIN dbo.aca_AnioLectivoEquivalenciaPromedio AS ep6 WITH (nolock) ON m.IdAnio = ep6.IdAnio AND mc.IdEmpresa = ep6.IdEmpresa AND mc.IdEquivalenciaPromedioP6 = ep6.IdEquivalenciaPromedio "
                   + " LEFT OUTER JOIN dbo.aca_AnioLectivoEquivalenciaPromedio AS epEQ2 WITH (nolock) ON m.IdAnio = epEQ2.IdAnio AND mc.IdEmpresa = epEQ2.IdEmpresa AND mc.IdEquivalenciaPromedioEQ2 = epEQ2.IdEquivalenciaPromedio "
                   + " LEFT OUTER JOIN dbo.aca_AnioLectivoEquivalenciaPromedio AS epQ2 WITH (nolock) ON m.IdAnio = epQ2.IdAnio AND mc.IdEmpresa = epQ2.IdEmpresa AND mc.IdEquivalenciaPromedioQ2 = epQ2.IdEquivalenciaPromedio "
                   + " LEFT OUTER JOIN dbo.aca_AnioLectivoEquivalenciaPromedio AS epPF WITH (nolock) ON m.IdAnio = epPF.IdAnio AND mc.IdEmpresa = epPF.IdEmpresa AND mc.IdEquivalenciaPromedioPF = epPF.IdEquivalenciaPromedio "
                   + " LEFT OUTER JOIN "
                   + " dbo.aca_AnioLectivo AS AN WITH (nolock) ON m.IdEmpresa = AN.IdEmpresa AND m.IdAnio = AN.IdAnio LEFT OUTER JOIN "
                   + " dbo.aca_AnioLectivo_Sede_NivelAcademico AS sn WITH (nolock) RIGHT OUTER JOIN "
                   + " dbo.aca_AnioLectivo_NivelAcademico_Jornada AS nj WITH (nolock) ON sn.IdEmpresa = nj.IdEmpresa AND sn.IdAnio = nj.IdAnio AND sn.IdSede = nj.IdSede AND sn.IdNivel = nj.IdNivel RIGHT OUTER JOIN "
                   + " dbo.aca_AnioLectivo_Jornada_Curso AS jc WITH (nolock) ON nj.IdEmpresa = jc.IdEmpresa AND nj.IdAnio = jc.IdAnio AND nj.IdSede = jc.IdSede AND nj.IdNivel = jc.IdNivel AND nj.IdJornada = jc.IdJornada RIGHT OUTER JOIN "
                   + " dbo.aca_AnioLectivo_Curso_Paralelo AS cp WITH (nolock) ON jc.IdEmpresa = cp.IdEmpresa AND jc.IdAnio = cp.IdAnio AND jc.IdSede = cp.IdSede AND jc.IdNivel = cp.IdNivel AND jc.IdJornada = cp.IdJornada AND jc.IdCurso = cp.IdCurso ON "
                   + " m.IdEmpresa = cp.IdEmpresa AND m.IdAnio = cp.IdAnio AND m.IdSede = cp.IdSede AND m.IdNivel = cp.IdNivel AND m.IdJornada = cp.IdJornada AND m.IdCurso = cp.IdCurso AND m.IdParalelo = cp.IdParalelo "
                   + " LEFT OUTER JOIN aca_Profesor AS pro WITH (nolock) ON cp.IdEmpresa = pro.IdEmpresa and cp.IdProfesorInspector = pro.IdProfesor "
                   + " LEFT OUTER JOIN tb_persona as pins WITH (nolock) on pins.IdPersona = pro.IdPersona "
                   + " LEFT OUTER JOIN tb_persona as pre WITH (nolock) on pre.IdPersona = m.IdPersonaR "
                   + " LEFT JOIN "
                   + " ( "
                   + " select r.IdEmpresa, r.IdMatricula "
                   + " from aca_AlumnoRetiro as r WITH (nolock) "
                   + " where r.Estado = 1 "
                   + " ) as ret on m.IdEmpresa = ret.IdEmpresa and m.IdMatricula = ret.IdMatricula "
                   + " where mc.IdEmpresa = @IdEmpresa "
                   + " and m.IdAnio = @IdAnio "
                   + " and m.IdSede = @IdSede ";

                    if (IdAlumno==0)
                    {
                        query += " and m.IdJornada = @IdJornada "
                        + " and m.IdNivel = @IdNivel "
                        + " and m.IdCurso = @IdCurso "
                        + " and m.IdParalelo = case when @IdParalelo = 0 then m.IdParalelo else @IdParalelo end ";
                    }
                    else
                    {
                        query += " and m.IdAlumno = case when @IdAlumno = 0 then m.IdAlumno else @IdAlumno end ";
                    }

                    query += " and isnull(ret.IdMatricula, 0) = case when @MostrarRetirados = 1 then isnull(ret.IdMatricula, 0) else 0 end "
                    + " UNION ALL "
                    + " ( "
                    + " /*CALIFICACIONES CUALITATIVAS*/ "
                    + " select a.IdEmpresa, a.IdAnio, a.IdSede, a.IdNivel, a.IdJornada, a.IdCurso, a.IdParalelo, a.IdAlumno, a.IdMatricula, a.IdMateria, "
                    + " a.NomMateria, a.NomMateriaArea, a.NomMateriaGrupo, a.EsObligatorio, a.OrdenMateriaArea, a.OrdenMateriaGrupo, a.PromediarGrupo, "
                    + " a.OrdenMateria, a.IdCatalogoTipoCalificacion, a.Codigo, a.NombreAlumno, a.Descripcion, a.NomSede, a.NomNivel, a.OrdenNivel, a.NomJornada, "
                    + " a.OrdenJornada, a.NomCurso, a.OrdenCurso, a.CodigoParalelo, a.NomParalelo, a.OrdenParalelo, a.NombreInspector, a.NombreRepresentante, "
                    + " cast(max(a.CalificacionP1) as varchar) CalificacionP1, null EquivalenciaP1, "
                    + " cast(max(a.CalificacionP2) as varchar) CalificacionP2, null EquivalenciaP2, "
                    + " cast(max(a.CalificacionP3) as varchar) CalificacionP3, null EquivalenciaP3, "
                    + " null PorcentajePromedioQ1, null PorcentajeExamenQ1, null ExamenQ1, null EquivalenciaPromedioEQ1, "
                    + " cast(max(a.PromedioFinalQ1) as varchar) PromedioFinalQ1, null EquivalenciaPromedioQ1, "
                    + " cast(max(a.CalificacionP4) as varchar) CalificacionP4, null EquivalenciaP4, "
                    + " cast(max(a.CalificacionP5) as varchar) CalificacionP5, null EquivalenciaP5, "
                    + " cast(max(a.CalificacionP6) as varchar) CalificacionP6, null EquivalenciaP6, "
                    + " null PorcentajePromedioQ2, null PorcentajeExamenQ2, null ExamenQ2, null EquivalenciaPromedioEQ2, "
                    + " cast(max(a.PromedioFinalQ2) as varchar) PromedioFinalQ2, null EquivalenciaPromedioQ2, "
                    + " null PromedioQuimestres_PF, null Promedio_PR, null ExamenMejoramiento, null CampoMejoramiento, null ExamenSupletorio, null ExamenRemedial, null ExamenGracia, "
                    + " cast(max(a.PromedioFinal) as varchar) PromedioFinal, null IdEquivalenciaPromedioPF, null EquivalenciaPromedioPF "
                    + " from( "
                    + " SELECT a.IdEmpresa, a.IdMatricula, a.IdMateria, m.IdAnio, m.IdSede, m.IdNivel, m.IdJornada, m.IdCurso, m.IdParalelo, m.IdAlumno, MC.NomMateria, "
                    + " MC.NomMateriaArea, MC.NomMateriaGrupo, MC.PromediarGrupo, MC.EsObligatorio, MC.OrdenMateria, MC.IdCatalogoTipoCalificacion, "
                    + " MC.OrdenMateriaGrupo, MC.OrdenMateriaArea, alu.Codigo, p.pe_nombreCompleto NombreAlumno, AN.Descripcion, sn.NomSede, sn.NomNivel, "
                    + " sn.OrdenNivel, nj.NomJornada, nj.OrdenJornada, jc.NomCurso, jc.OrdenCurso, cp.CodigoParalelo, "
                    + " cp.NomParalelo, cp.OrdenParalelo, pins.pe_nombreCompleto AS NombreInspector, pre.pe_nombreCompleto NombreRepresentante, "
                    + " CASE WHEN IdCatalogoParcial = 28 THEN acc.Codigo END AS CalificacionP1, "
                    + " CASE WHEN IdCatalogoParcial = 29 THEN acc.Codigo END AS CalificacionP2, "
                    + "  CASE WHEN IdCatalogoParcial = 30 THEN acc.Codigo END AS CalificacionP3, "
                    + " CASE WHEN IdCatalogoParcial = 31 THEN acc.Codigo END AS CalificacionP4, "
                    + " CASE WHEN IdCatalogoParcial = 32 THEN acc.Codigo END AS CalificacionP5, "
                    + " CASE WHEN IdCatalogoParcial = 33 THEN acc.Codigo END AS CalificacionP6, "
                    + " pq1.Codigo as PromedioFinalQ1, "
                    + " pq2.Codigo as PromedioFinalQ2, "
                    + " pf.Codigo as PromedioFinal "
                    + " FROM     dbo.aca_MatriculaCalificacionCualitativa AS a WITH (nolock) INNER JOIN "
                    + " dbo.aca_Matricula AS m WITH (nolock) ON m.IdEmpresa = a.IdEmpresa AND m.IdMatricula = a.IdMatricula INNER JOIN "
                    + " dbo.aca_Alumno AS alu WITH (nolock) ON m.IdEmpresa = alu.IdEmpresa AND m.IdAlumno = alu.IdAlumno INNER JOIN "
                    + " dbo.tb_persona AS p WITH (nolock) ON alu.IdPersona = p.IdPersona LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivoCalificacionCualitativa AS acc WITH (nolock) ON acc.IdEmpresa = a.IdEmpresa AND acc.IdAnio = m.IdAnio AND acc.IdCalificacionCualitativa = a.IdCalificacionCualitativa LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo AS AN WITH (nolock) ON m.IdEmpresa = AN.IdEmpresa AND m.IdAnio = AN.IdAnio LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Paralelo AS cp WITH (nolock) RIGHT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Jornada_Curso AS jc WITH (nolock) LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Sede_NivelAcademico AS sn WITH (nolock) INNER JOIN "
                    + " dbo.aca_AnioLectivo_NivelAcademico_Jornada AS nj WITH (nolock) ON sn.IdEmpresa = nj.IdEmpresa AND sn.IdAnio = nj.IdAnio AND sn.IdSede = nj.IdSede AND sn.IdNivel = nj.IdNivel ON jc.IdJornada = nj.IdJornada AND jc.IdEmpresa = nj.IdEmpresa AND "
                    + " jc.IdAnio = nj.IdAnio AND jc.IdSede = nj.IdSede AND jc.IdNivel = nj.IdNivel LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Materia AS MC WITH (nolock) ON jc.IdEmpresa = MC.IdEmpresa AND jc.IdAnio = MC.IdAnio AND jc.IdSede = MC.IdSede AND jc.IdNivel = MC.IdNivel AND jc.IdJornada = MC.IdJornada AND jc.IdCurso = MC.IdCurso ON "
                    + " cp.IdEmpresa = jc.IdEmpresa AND cp.IdAnio = jc.IdAnio AND cp.IdSede = jc.IdSede AND cp.IdNivel = jc.IdNivel AND cp.IdJornada = jc.IdJornada ON m.IdEmpresa = MC.IdEmpresa AND m.IdAnio = MC.IdAnio AND m.IdSede = MC.IdSede AND "
                    + " m.IdNivel = MC.IdNivel AND m.IdJornada = MC.IdJornada AND m.IdCurso = MC.IdCurso AND a.IdMateria = MC.IdMateria AND m.IdEmpresa = sn.IdEmpresa AND m.IdAnio = sn.IdAnio AND m.IdSede = sn.IdSede AND m.IdNivel = sn.IdNivel AND "
                    + " m.IdEmpresa = cp.IdEmpresa AND m.IdAnio = cp.IdAnio AND m.IdSede = cp.IdSede AND m.IdNivel = cp.IdNivel AND m.IdJornada = cp.IdJornada AND m.IdCurso = cp.IdCurso AND m.IdParalelo = cp.IdParalelo "
                    + " LEFT OUTER JOIN aca_Profesor AS pro WITH (nolock) ON cp.IdEmpresa = pro.IdEmpresa and cp.IdProfesorInspector = pro.IdProfesor "
                    + " LEFT OUTER JOIN tb_persona as pins WITH (nolock) on pins.IdPersona = pro.IdPersona "
                    + " LEFT OUTER JOIN tb_persona as pre WITH (nolock) on pre.IdPersona = m.IdPersonaR "
                    + " LEFT JOIN "
                        + " ( "
                        + " select r.IdEmpresa, r.IdMatricula "
                        + " from aca_AlumnoRetiro as r WITH (nolock) "
                        + " where r.Estado = 1 "
                        + " ) as ret on m.IdEmpresa = ret.IdEmpresa and m.IdMatricula = ret.IdMatricula "
                    + " left join aca_MatriculaCalificacionCualitativaPromedio pr WITH (nolock) on pr.IdEmpresa = a.IdEmpresa and pr.IdMatricula = a.IdMatricula and pr.IdMateria = a.IdMateria "
                    + " left join aca_AnioLectivoCalificacionCualitativa pq1 WITH (nolock) on pq1.IdEmpresa = pr.IdEmpresa and pq1.IdAnio = m.IdAnio and pq1.IdCalificacionCualitativa = pr.IdCalificacionCualitativaQ1 "
                    + " left join aca_AnioLectivoCalificacionCualitativa pq2 WITH (nolock) on pq2.IdEmpresa = pr.IdEmpresa and pq2.IdAnio = m.IdAnio and pq2.IdCalificacionCualitativa = pr.IdCalificacionCualitativaQ2 "
                    + " left join aca_AnioLectivoCalificacionCualitativa pf WITH (nolock) on pf.IdEmpresa = pr.IdEmpresa and pf.IdAnio = m.IdAnio and pf.IdCalificacionCualitativa = pr.IdCalificacionCualitativaFinal "
                    + " where mc.IdEmpresa = @IdEmpresa "
                   + " and m.IdAnio = @IdAnio "
                   + " and m.IdSede = @IdSede ";

                    if (IdAlumno == 0)
                    {
                        query += " and m.IdJornada = @IdJornada "
                        + " and m.IdNivel = @IdNivel "
                        + " and m.IdCurso = @IdCurso "
                        + " and m.IdParalelo = case when @IdParalelo = 0 then m.IdParalelo else @IdParalelo end ";
                    }
                    else
                    {
                        query += " and m.IdAlumno = case when @IdAlumno = 0 then m.IdAlumno else @IdAlumno end ";
                    }

                    query += " and isnull(ret.IdMatricula, 0) = case when @MostrarRetirados = 1 then isnull(ret.IdMatricula, 0) else 0 end "
                    + " ) a "
                    + " group by "
                    + " a.IdEmpresa, a.IdAnio, a.IdSede, a.IdNivel, a.IdJornada, a.IdCurso, a.IdParalelo, a.IdAlumno, a.IdMatricula, a.IdMateria, "
                    + " a.NomMateria, a.NomMateriaArea, a.NomMateriaGrupo, a.EsObligatorio, a.OrdenMateriaArea, a.OrdenMateriaGrupo, a.PromediarGrupo, "
                    + " a.OrdenMateria, a.IdCatalogoTipoCalificacion, a.Codigo, a.NombreAlumno, a.Descripcion, a.NomSede, a.NomNivel, a.OrdenNivel, a.NomJornada, "
                    + " a.OrdenJornada, a.NomCurso, a.OrdenCurso, a.CodigoParalelo, a.NomParalelo, a.OrdenParalelo, a.NombreInspector, a.NombreRepresentante "
                    + " ) ";
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 5000;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new ACA_014_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdMatricula = Convert.ToDecimal(reader["IdMatricula"]),
                            IdMateria = Convert.ToInt32(reader["IdMateria"]),
                            IdAlumno = Convert.ToDecimal(reader["IdAlumno"]),
                            pe_nombreCompleto = string.IsNullOrEmpty(reader["pe_nombreCompleto"].ToString()) ? null : reader["pe_nombreCompleto"].ToString(),
                            Codigo = string.IsNullOrEmpty(reader["Codigo"].ToString()) ? null : reader["Codigo"].ToString(),
                            NombreInspector = string.IsNullOrEmpty(reader["NombreInspector"].ToString()) ? null : reader["NombreInspector"].ToString(),
                            NombreRepresentante = string.IsNullOrEmpty(reader["NombreRepresentante"].ToString()) ? null : reader["NombreRepresentante"].ToString(),
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
                            CodigoParalelo = string.IsNullOrEmpty(reader["CodigoParalelo"].ToString()) ? null : reader["CodigoParalelo"].ToString(),
                            OrdenNivel = string.IsNullOrEmpty(reader["OrdenNivel"].ToString()) ? (int?)null : Convert.ToInt32(reader["OrdenNivel"]),
                            OrdenJornada = string.IsNullOrEmpty(reader["OrdenJornada"].ToString()) ? (int?)null : Convert.ToInt32(reader["OrdenJornada"]),
                            OrdenCurso = string.IsNullOrEmpty(reader["OrdenCurso"].ToString()) ? (int?)null : Convert.ToInt32(reader["OrdenCurso"]),
                            OrdenParalelo = string.IsNullOrEmpty(reader["OrdenParalelo"].ToString()) ? (int?)null : Convert.ToInt32(reader["OrdenParalelo"]),
                            OrdenMateria = string.IsNullOrEmpty(reader["OrdenMateria"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenMateria"]),
                            OrdenMateriaArea = string.IsNullOrEmpty(reader["OrdenMateriaArea"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenMateriaArea"]),
                            OrdenMateriaGrupo = string.IsNullOrEmpty(reader["OrdenMateriaGrupo"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenMateriaGrupo"]),
                            NomMateriaGrupo = string.IsNullOrEmpty(reader["NomMateriaGrupo"].ToString()) ? null : reader["NomMateriaGrupo"].ToString(),
                            NomMateria = string.IsNullOrEmpty(reader["NomMateria"].ToString()) ? null : reader["NomMateria"].ToString(),
                            NomMateriaArea = string.IsNullOrEmpty(reader["NomMateriaArea"].ToString()) ? null : reader["NomMateriaArea"].ToString(),
                            IdCatalogoTipoCalificacion = string.IsNullOrEmpty(reader["IdCatalogoTipoCalificacion"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdCatalogoTipoCalificacion"]),
                            PromediarGrupo = string.IsNullOrEmpty(reader["PromediarGrupo"].ToString()) ? false : Convert.ToBoolean(reader["PromediarGrupo"]),
                            EsObligatorio = string.IsNullOrEmpty(reader["EsObligatorio"].ToString()) ? false : Convert.ToBoolean(reader["EsObligatorio"]),
                            CalificacionP1 = string.IsNullOrEmpty(reader["CalificacionP1"].ToString()) ? null : reader["CalificacionP1"].ToString(),
                            CalificacionP2 = string.IsNullOrEmpty(reader["CalificacionP2"].ToString()) ? null : reader["CalificacionP2"].ToString(),
                            CalificacionP3 = string.IsNullOrEmpty(reader["CalificacionP3"].ToString()) ? null : reader["CalificacionP3"].ToString(),
                            ExamenQ1 = string.IsNullOrEmpty(reader["ExamenQ1"].ToString()) ? null : reader["ExamenQ1"].ToString(),
                            PorcentajePromedioQ1 = string.IsNullOrEmpty(reader["PorcentajePromedioQ1"].ToString()) ? null : reader["PorcentajePromedioQ1"].ToString(),
                            PorcentajeExamenQ1 = string.IsNullOrEmpty(reader["PorcentajeExamenQ1"].ToString()) ? null : reader["PorcentajeExamenQ1"].ToString(),
                            PromedioFinalQ1 = string.IsNullOrEmpty(reader["PromedioFinalQ1"].ToString()) ? null : reader["PromedioFinalQ1"].ToString(),
                            EquivalenciaPromedioP1 = string.IsNullOrEmpty(reader["EquivalenciaPromedioP1"].ToString()) ? null : reader["EquivalenciaPromedioP1"].ToString(),
                            EquivalenciaPromedioP2 = string.IsNullOrEmpty(reader["EquivalenciaPromedioP2"].ToString()) ? null : reader["EquivalenciaPromedioP2"].ToString(),
                            EquivalenciaPromedioP3 = string.IsNullOrEmpty(reader["EquivalenciaPromedioP3"].ToString()) ? null : reader["EquivalenciaPromedioP3"].ToString(),
                            EquivalenciaPromedioEQ1 = string.IsNullOrEmpty(reader["EquivalenciaPromedioEQ1"].ToString()) ? null : reader["EquivalenciaPromedioEQ1"].ToString(),
                            EquivalenciaPromedioQ1 = string.IsNullOrEmpty(reader["EquivalenciaPromedioQ1"].ToString()) ? null : reader["EquivalenciaPromedioQ1"].ToString(),
                            CalificacionP4 = string.IsNullOrEmpty(reader["CalificacionP4"].ToString()) ? null : reader["CalificacionP4"].ToString(),
                            CalificacionP5 = string.IsNullOrEmpty(reader["CalificacionP5"].ToString()) ? null : reader["CalificacionP5"].ToString(),
                            CalificacionP6 = string.IsNullOrEmpty(reader["CalificacionP6"].ToString()) ? null : reader["CalificacionP6"].ToString(),
                            ExamenQ2 = string.IsNullOrEmpty(reader["ExamenQ2"].ToString()) ? null : reader["ExamenQ2"].ToString(),
                            PorcentajePromedioQ2 = string.IsNullOrEmpty(reader["PorcentajePromedioQ2"].ToString()) ? null : reader["PorcentajePromedioQ2"].ToString(),
                            PorcentajeExamenQ2 = string.IsNullOrEmpty(reader["PorcentajeExamenQ2"].ToString()) ? null : reader["PorcentajeExamenQ2"].ToString(),
                            PromedioFinalQ2 = string.IsNullOrEmpty(reader["PromedioFinalQ2"].ToString()) ? null : reader["PromedioFinalQ2"].ToString(),
                            ExamenSupletorio = string.IsNullOrEmpty(reader["ExamenSupletorio"].ToString()) ? null : reader["ExamenSupletorio"].ToString(),
                            ExamenMejoramiento = string.IsNullOrEmpty(reader["ExamenMejoramiento"].ToString()) ? null : reader["ExamenMejoramiento"].ToString(),
                            ExamenGracia = string.IsNullOrEmpty(reader["ExamenGracia"].ToString()) ? null : reader["ExamenGracia"].ToString(),
                            ExamenRemedial = string.IsNullOrEmpty(reader["ExamenRemedial"].ToString()) ? null : reader["ExamenRemedial"].ToString(),
                            CampoMejoramiento = string.IsNullOrEmpty(reader["CampoMejoramiento"].ToString()) ? null : reader["CampoMejoramiento"].ToString(),
                            PromedioFinal = string.IsNullOrEmpty(reader["PromedioFinal"].ToString()) ? null : reader["PromedioFinal"].ToString(),
                            EquivalenciaPromedioP4 = string.IsNullOrEmpty(reader["EquivalenciaPromedioP4"].ToString()) ? null : reader["EquivalenciaPromedioP4"].ToString(),
                            EquivalenciaPromedioP5 = string.IsNullOrEmpty(reader["EquivalenciaPromedioP5"].ToString()) ? null : reader["EquivalenciaPromedioP5"].ToString(),
                            EquivalenciaPromedioP6 = string.IsNullOrEmpty(reader["EquivalenciaPromedioP6"].ToString()) ? null : reader["EquivalenciaPromedioP6"].ToString(),
                            EquivalenciaPromedioEQ2 = string.IsNullOrEmpty(reader["EquivalenciaPromedioEQ2"].ToString()) ? null : reader["EquivalenciaPromedioEQ2"].ToString(),
                            EquivalenciaPromedioQ2 = string.IsNullOrEmpty(reader["EquivalenciaPromedioQ2"].ToString()) ? null : reader["EquivalenciaPromedioQ2"].ToString(),
                            PromedioQuimestres_PF = string.IsNullOrEmpty(reader["PromedioQuimestres_PF"].ToString()) ? null : reader["PromedioQuimestres_PF"].ToString(),
                            Promedio_PR = string.IsNullOrEmpty(reader["Promedio_PR"].ToString()) ? null : reader["Promedio_PR"].ToString(),
                            EquivalenciaPromedioPF = string.IsNullOrEmpty(reader["EquivalenciaPromedioPF"].ToString()) ? null : reader["EquivalenciaPromedioPF"].ToString(),
                            IdEquivalenciaPromedioPF = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioPF"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioPF"]),
                        });
                    }
                    reader.Close();
                }
                Lista.ForEach(q => {
                    q.NoMostrarPromedioQ1 = q.PromedioFinalQ1 == null ? 0 : 1;
                    q.NoMostrarPromedioQ1 = q.PromedioFinalQ1 == null ? 1 : 0;
                    q.NoMostrarPromedioQ2 = q.PromedioFinalQ2 == null ? 1 : 0;
                    q.NoMostrarPromedioQuim = q.PromedioQuimestres_PF == null ? 1 : 0;
                    q.NoMostrarPF = (q.IdCatalogoTipoCalificacion ==null ?  1 : (Convert.ToInt32(q.IdCatalogoTipoCalificacion) == Convert.ToInt32(cl_enumeradores.eCatalogoTipoCalificacion.CUANTI) ? q.PromedioFinal == null ? 1 : (Convert.ToDouble(q.PromedioFinal)< PromedioMinimoParcial ? 1 : 0) : 0));
                    q.NoMostrarPromedioFinal = q.PromedioFinal == null ? 1 : 0;
                    q.CalificacionP4 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? q.CalificacionP4 : null);
                    q.CalificacionP5 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? q.CalificacionP5 : null);
                    q.CalificacionP6 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? q.CalificacionP6 : null);
                    q.ExamenQ2 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? q.ExamenQ2 : null);
                    q.PorcentajePromedioQ2 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? q.PorcentajePromedioQ2 : null);
                    q.PorcentajeExamenQ2 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? q.PorcentajeExamenQ2 : null);
                    q.PromedioFinalQ2 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? q.PromedioFinalQ2 : null);
                    q.ExamenSupletorio = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? q.ExamenSupletorio : null);
                    q.ExamenMejoramiento = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? q.ExamenMejoramiento : null);
                    q.ExamenGracia = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? q.ExamenGracia : null);
                    q.ExamenRemedial = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? q.ExamenRemedial : null);
                    q.CampoMejoramiento = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? q.CampoMejoramiento : null);
                    q.PromedioFinal = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? q.PromedioFinal : null);
                    q.EquivalenciaPromedioP4 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? q.EquivalenciaPromedioP4 : "");
                    q.EquivalenciaPromedioP5 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? q.EquivalenciaPromedioP5 : "");
                    q.EquivalenciaPromedioP6 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? q.EquivalenciaPromedioP6 : "");
                    q.EquivalenciaPromedioEQ2 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? q.EquivalenciaPromedioEQ2 : "");
                    q.EquivalenciaPromedioQ2 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? q.EquivalenciaPromedioQ2 : "");
                    q.PromedioQuimestres_PF = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? q.PromedioQuimestres_PF : null);
                    q.Promedio_PR = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? q.Promedio_PR : null);
                    q.EquivalenciaPromedioPF = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? q.EquivalenciaPromedioPF : "");
                });
                
                #region Agrupar por "Promedio agrupado"
                int TipoCatalogoCuantitativo = Convert.ToInt32(cl_enumeradores.eCatalogoTipoCalificacion.CUANTI);
                var lstAgrupada = Lista.Where(q => q.IdCatalogoTipoCalificacion == TipoCatalogoCuantitativo && q.PromediarGrupo == true).GroupBy(q => new
                {
                    q.IdEmpresa,
                    q.IdMatricula,
                    q.NomMateriaGrupo
                }).Select(q=> new ACA_014_Info
                {
                    IdEmpresa = q.Key.IdEmpresa,
                    IdMatricula = q.Key.IdMatricula,
                    NomMateriaGrupo = q.Key.NomMateriaGrupo,
                    PromedioFinalQ1Double = q.Max(g => g.PromedioFinalQ1) == null ? (decimal?)null : q.Sum(g => Convert.ToDecimal(g.PromedioFinalQ1)) / q.Count(g=> !string.IsNullOrEmpty(g.PromedioFinalQ1)),
                    PromedioFinalQ2Double = q.Max(g => g.PromedioFinalQ2) == null ? (decimal?)null : q.Sum(g => Convert.ToDecimal(g.PromedioFinalQ2)) / q.Count(g => !string.IsNullOrEmpty(g.PromedioFinalQ2)),
                    PromedioQuimestresDouble = q.Max(g => g.PromedioQuimestres_PF) == null ? (decimal?)null : q.Sum(g => Convert.ToDecimal(g.PromedioQuimestres_PF)) / q.Count(g => !string.IsNullOrEmpty(g.PromedioQuimestres_PF)),
                    PromedioFinalDouble = q.Max(g => g.PromedioFinal) == null ? (decimal?)null : q.Sum(g => Convert.ToDecimal(g.PromedioFinal)) / q.Count(g => !string.IsNullOrEmpty(g.PromedioFinal)),
                    PromedioGrupoQ1Double = q.Max(g => g.PromedioFinalQ1) == null ? (decimal?)null : q.Sum(g => Convert.ToDecimal(g.PromedioFinalQ1)) / q.Count(g => !string.IsNullOrEmpty(g.PromedioFinalQ1)),
                    PromedioGrupoQ2Double = q.Max(g => g.PromedioFinalQ2) == null ? (decimal?)null : q.Sum(g => Convert.ToDecimal(g.PromedioFinalQ2)) / q.Count(g => !string.IsNullOrEmpty(g.PromedioFinalQ2)),
                    PromedioQuimestresGrupoDouble = q.Max(g => g.PromedioQuimestres_PF) == null ? (decimal?)null : q.Sum(g => Convert.ToDecimal(g.PromedioQuimestres_PF)) / q.Count(g => !string.IsNullOrEmpty(g.PromedioQuimestres_PF)),
                    PromedioFinalGrupoDouble = q.Max(g => g.PromedioFinal) == null ? (decimal?)null : q.Sum(g => Convert.ToDecimal(g.PromedioFinal)) / q.Count(g => !string.IsNullOrEmpty(g.PromedioFinal))
                }).ToList();
                #endregion

                var lstPromedioMateriasNoAgrupada = Lista.Where(q => q.IdCatalogoTipoCalificacion == TipoCatalogoCuantitativo && (q.PromediarGrupo ?? false) == false).GroupBy(q => new
                {
                    q.IdEmpresa,
                    q.IdMatricula,
                    q.NomMateriaGrupo
                }).Select(q => new ACA_014_Info
                {
                    IdEmpresa = q.Key.IdEmpresa,
                    IdMatricula = q.Key.IdMatricula,
                    NomMateriaGrupo = q.Key.NomMateriaGrupo,
                    PromedioGrupoQ1Double = q.Max(g => g.PromedioFinalQ1) == null ? (decimal?)null : q.Sum(g => Convert.ToDecimal(g.PromedioFinalQ1)) / q.Count(g => !string.IsNullOrEmpty(g.PromedioFinalQ1)),
                    PromedioGrupoQ2Double = q.Max(g => g.PromedioFinalQ2) == null ? (decimal?)null : q.Sum(g => Convert.ToDecimal(g.PromedioFinalQ2)) / q.Count(g => !string.IsNullOrEmpty(g.PromedioFinalQ2)),
                    PromedioQuimestresGrupoDouble = q.Max(g => g.PromedioQuimestres_PF) == null ? (decimal?)null : q.Sum(g => Convert.ToDecimal(g.PromedioQuimestres_PF)) / q.Count(g => !string.IsNullOrEmpty(g.PromedioQuimestres_PF)),
                    PromedioFinalGrupoDouble = q.Max(g => g.PromedioFinal) == null ? (decimal?)null : q.Sum(g => Convert.ToDecimal(g.PromedioFinal)) / q.Count(g => !string.IsNullOrEmpty(g.PromedioFinal))

                }).ToList();
                lstPromedioMateriasNoAgrupada.AddRange(lstAgrupada);

                Lista = (from a in Lista
                         join b in lstPromedioMateriasNoAgrupada
                         on new { a.IdEmpresa, a.IdMatricula, a.NomMateriaGrupo } equals new { b.IdEmpresa, b.IdMatricula, b.NomMateriaGrupo }
                         select new ACA_014_Info
                         {
                             IdEmpresa = a.IdEmpresa,
                             IdMatricula = a.IdMatricula,
                             IdMateria = a.IdMateria,
                             IdAlumno = a.IdAlumno,
                             pe_nombreCompleto = a.pe_nombreCompleto,
                             Codigo = a.Codigo,
                             IdAnio = a.IdAnio,
                             IdSede = a.IdSede,
                             IdNivel = a.IdNivel,
                             IdJornada = a.IdJornada,
                             IdCurso = a.IdCurso,
                             IdParalelo = a.IdParalelo,
                             CodigoParalelo = a.CodigoParalelo,
                             Descripcion = a.Descripcion,
                             NomSede = a.NomSede,
                             NomNivel = a.NomNivel,
                             NomJornada = a.NomJornada,
                             NomMateria = a.NomMateria,
                             NomCurso = a.NomCurso,
                             NomParalelo = a.NomParalelo,
                             OrdenNivel = a.OrdenNivel,
                             OrdenJornada = a.OrdenJornada,
                             OrdenCurso = a.OrdenCurso,
                             OrdenParalelo = a.OrdenParalelo,
                             OrdenMateriaGrupo = a.OrdenMateriaGrupo,
                             OrdenMateriaArea = a.OrdenMateriaArea,
                             OrdenMateria = a.OrdenMateria,
                             PromediarGrupo = a.PromediarGrupo,
                             EsObligatorio = a.EsObligatorio,
                             NomMateriaArea = a.NomMateriaArea,
                             NomMateriaGrupo = a.NomMateriaGrupo,
                             CalificacionP1 = a.CalificacionP1,
                             CalificacionP2 = a.CalificacionP2,
                             CalificacionP3 = a.CalificacionP3,
                             ExamenQ1 = a.ExamenQ1,
                             PorcentajePromedioQ1 = a.PorcentajePromedioQ1,
                             PorcentajeExamenQ1 = a.PorcentajeExamenQ1,
                             PromedioFinalQ1 = a.PromedioFinalQ1,
                             EquivalenciaPromedioP1 = a.EquivalenciaPromedioP1,
                             EquivalenciaPromedioP2 = a.EquivalenciaPromedioP2,
                             EquivalenciaPromedioP3 = a.EquivalenciaPromedioP3,
                             EquivalenciaPromedioEQ1 = a.EquivalenciaPromedioEQ1,
                             EquivalenciaPromedioQ1 = a.EquivalenciaPromedioQ1,
                             CalificacionP4 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.CalificacionP4 : null),
                             CalificacionP5 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.CalificacionP5 : null),
                             CalificacionP6 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.CalificacionP6 : null),
                             ExamenQ2 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.ExamenQ2 : null),
                             PorcentajePromedioQ2 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.PorcentajePromedioQ2 : null),
                             PorcentajeExamenQ2 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.PorcentajeExamenQ2 : null),
                             PromedioFinalQ2 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.PromedioFinalQ2 : null),
                             ExamenSupletorio = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.ExamenSupletorio : null),
                             ExamenMejoramiento = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.ExamenMejoramiento : null),
                             ExamenGracia = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.ExamenGracia : null),
                             ExamenRemedial = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.ExamenRemedial : null),
                             CampoMejoramiento = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.CampoMejoramiento : null),
                             PromedioFinal = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.PromedioFinal : null),
                             EquivalenciaPromedioP4 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.EquivalenciaPromedioP4 : ""),
                             EquivalenciaPromedioP5 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.EquivalenciaPromedioP5 : ""),
                             EquivalenciaPromedioP6 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.EquivalenciaPromedioP6 : ""),
                             EquivalenciaPromedioEQ2 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.EquivalenciaPromedioEQ2 : ""),
                             EquivalenciaPromedioQ2 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.EquivalenciaPromedioQ2 : ""),
                             PromedioQuimestres_PF = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.PromedioQuimestres_PF : null),
                             Promedio_PR = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.Promedio_PR : ""),
                             EquivalenciaPromedioPF = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.EquivalenciaPromedioPF : ""),
                             IdEquivalenciaPromedioPF = a.IdEquivalenciaPromedioPF,
                             IdCatalogoTipoCalificacion = a.IdCatalogoTipoCalificacion,
                             NombreRepresentante = a.NombreRepresentante,
                             NombreInspector = a.NombreInspector,
                             NoMostrarPromedioQ1 = a.NoMostrarPromedioQ1,
                             NoMostrarPromedioQ2 = a.NoMostrarPromedioQ2,
                             NoMostrarPromedioQuim = a.NoMostrarPromedioQuim,
                             NoMostrarPromedioFinal = a.NoMostrarPromedioFinal,
                             NoMostrarPF = a.NoMostrarPF,
                             PromedioGrupoQ1Double = b.PromedioGrupoQ1Double == null ? (decimal?)null : b.PromedioGrupoQ1Double ?? 0,
                             PromedioGrupoQ2Double = b.PromedioGrupoQ2Double == null ? (decimal?)null : b.PromedioGrupoQ2Double ?? 0,
                             PromedioQuimestresGrupoDouble = b.PromedioQuimestresGrupoDouble == null ? (decimal?)null : b.PromedioQuimestresGrupoDouble ?? 0,
                             PromedioFinalGrupoDouble = b.PromedioFinalGrupoDouble == null ? (decimal?)null : b.PromedioFinalGrupoDouble ?? 0
                         }).ToList();


                var lstMateriasNoAgrupada = Lista.Where(q => q.IdCatalogoTipoCalificacion == TipoCatalogoCuantitativo && (q.PromediarGrupo ?? false) == false).Select(q => new ACA_014_Info
                {
                    IdEmpresa = q.IdEmpresa,
                    IdMatricula = q.IdMatricula,
                    PromedioFinalQ1Double = q.PromedioFinalQ1 == null ? (decimal?)null : Convert.ToDecimal(q.PromedioFinalQ1),
                    PromedioFinalQ2Double = q.PromedioFinalQ2 == null ? (decimal?)null : Convert.ToDecimal(q.PromedioFinalQ2),
                    PromedioQuimestresDouble = q.PromedioQuimestres_PF == null ? (decimal?)null : Convert.ToDecimal(q.PromedioQuimestres_PF),
                    PromedioFinalDouble = q.PromedioFinal == null ? (decimal?)null : Convert.ToDecimal(q.PromedioFinal),
                }).ToList();
                lstMateriasNoAgrupada.AddRange(lstAgrupada);

                var lstFinal = lstMateriasNoAgrupada.GroupBy(q => new { q.IdEmpresa, q.IdMatricula }).Select(q => new ACA_014_Info
                {
                    IdEmpresa = q.Key.IdEmpresa,
                    IdMatricula = q.Key.IdMatricula,
                    PromedioFinalQ1Double = q.Max(g=> g.PromedioFinalQ1Double) == null ? (decimal?)null : (q.Sum(g=> g.PromedioFinalQ1Double) / q.Count(g => g.PromedioFinalQ1Double != null)),
                    PromedioFinalQ2Double = q.Max(g => g.PromedioFinalQ2Double) == null ? (decimal?)null : (q.Sum(g => g.PromedioFinalQ2Double) / q.Count(g => g.PromedioFinalQ2Double != null)),
                    PromedioQuimestresDouble = q.Max(g => g.PromedioQuimestresDouble) == null ? (decimal?)null : (q.Sum(g => g.PromedioQuimestresDouble) / q.Count(g => g.PromedioQuimestresDouble != null)),
                    PromedioFinalDouble = q.Max(g => g.PromedioFinalDouble) == null ? (decimal?)null : (q.Sum(g => g.PromedioFinalDouble) / q.Count(g => g.PromedioFinalDouble != null)),
                }).ToList();

                Lista = (from a in Lista
                         join b in lstFinal
                         on a.IdMatricula equals b.IdMatricula
                         select new ACA_014_Info
                         {
                             IdEmpresa = a.IdEmpresa,
                             IdMatricula = a.IdMatricula,
                             IdMateria = a.IdMateria,
                             IdAlumno = a.IdAlumno,
                             pe_nombreCompleto = a.pe_nombreCompleto,
                             Codigo = a.Codigo,
                             IdAnio = a.IdAnio,
                             IdSede = a.IdSede,
                             IdNivel = a.IdNivel,
                             IdJornada = a.IdJornada,
                             IdCurso = a.IdCurso,
                             IdParalelo = a.IdParalelo,
                             CodigoParalelo = a.CodigoParalelo,
                             Descripcion = a.Descripcion,
                             NomSede = a.NomSede,
                             NomNivel = a.NomNivel,
                             NomJornada = a.NomJornada,
                             NomMateria = a.NomMateria,
                             NomCurso = a.NomCurso,
                             NomParalelo = a.NomParalelo,
                             OrdenNivel = a.OrdenNivel,
                             OrdenJornada = a.OrdenJornada,
                             OrdenCurso = a.OrdenCurso,
                             OrdenParalelo = a.OrdenParalelo,
                             OrdenMateriaGrupo = a.OrdenMateriaGrupo,
                             OrdenMateriaArea = a.OrdenMateriaArea,
                             OrdenMateria = a.OrdenMateria,
                             PromediarGrupo = a.PromediarGrupo,
                             EsObligatorio = a.EsObligatorio,
                             NomMateriaArea = a.NomMateriaArea,
                             NomMateriaGrupo = a.NomMateriaGrupo,
                             CalificacionP1 = a.CalificacionP1,
                             CalificacionP2 = a.CalificacionP2,
                             CalificacionP3 = a.CalificacionP3,
                             ExamenQ1 = a.ExamenQ1,
                             PorcentajePromedioQ1 = a.PorcentajePromedioQ1,
                             PorcentajeExamenQ1 = a.PorcentajeExamenQ1,
                             PromedioFinalQ1 = a.PromedioFinalQ1,
                             EquivalenciaPromedioP1 = a.EquivalenciaPromedioP1,
                             EquivalenciaPromedioP2 = a.EquivalenciaPromedioP2,
                             EquivalenciaPromedioP3 = a.EquivalenciaPromedioP3,
                             EquivalenciaPromedioEQ1 = a.EquivalenciaPromedioEQ1,
                             EquivalenciaPromedioQ1 = a.EquivalenciaPromedioQ1,
                             CalificacionP4 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.CalificacionP4 : null),
                             CalificacionP5 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.CalificacionP5 : null),
                             CalificacionP6 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.CalificacionP6 : null),
                             ExamenQ2 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.ExamenQ2 : null),
                             PorcentajePromedioQ2 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.PorcentajePromedioQ2 : null),
                             PorcentajeExamenQ2 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.PorcentajeExamenQ2 : null),
                             PromedioFinalQ2 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.PromedioFinalQ2 : null),
                             ExamenSupletorio = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.ExamenSupletorio : null),
                             ExamenMejoramiento = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.ExamenMejoramiento : null),
                             ExamenGracia = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.ExamenGracia : null),
                             ExamenRemedial = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.ExamenRemedial : null),
                             CampoMejoramiento = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.CampoMejoramiento : null),
                             PromedioFinal = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.PromedioFinal : null),
                             EquivalenciaPromedioP4 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.EquivalenciaPromedioP4 : ""),
                             EquivalenciaPromedioP5 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.EquivalenciaPromedioP5 : ""),
                             EquivalenciaPromedioP6 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.EquivalenciaPromedioP6 : ""),
                             EquivalenciaPromedioEQ2 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.EquivalenciaPromedioEQ2 : ""),
                             EquivalenciaPromedioQ2 = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.EquivalenciaPromedioQ2 : ""),
                             PromedioQuimestres_PF = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.PromedioQuimestres_PF : ""),
                             Promedio_PR = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.Promedio_PR : ""),
                             EquivalenciaPromedioPF = (IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2) ? a.EquivalenciaPromedioPF : ""),
                             IdEquivalenciaPromedioPF = a.IdEquivalenciaPromedioPF,
                             IdCatalogoTipoCalificacion = a.IdCatalogoTipoCalificacion,
                             NombreRepresentante = a.NombreRepresentante,
                             NombreInspector = a.NombreInspector,
                             
                             NoMostrarPromedioQ1 = a.NoMostrarPromedioQ1,
                             NoMostrarPromedioQ2 = a.NoMostrarPromedioQ2,
                             NoMostrarPromedioQuim = a.NoMostrarPromedioQuim,
                             NoMostrarPromedioFinal = a.NoMostrarPromedioFinal,
                             NoMostrarPF = a.NoMostrarPF,
                             PromedioGrupoQ1Double = a.PromedioGrupoQ1Double,
                             PromedioGrupoQ2Double = a.PromedioGrupoQ2Double,
                             PromedioQuimestresGrupoDouble = a.PromedioQuimestresGrupoDouble,
                             PromedioFinalGrupoDouble = a.PromedioFinalGrupoDouble,

                             PFQ1 = b.PromedioFinalQ1Double == null ? (decimal?)null : Math.Round(b.PromedioFinalQ1Double ?? 0,2,MidpointRounding.AwayFromZero),
                             PFQ2 = b.PromedioFinalQ2Double == null ? (decimal?)null : Math.Round(b.PromedioFinalQ2Double ?? 0, 2, MidpointRounding.AwayFromZero),
                             PFQuim = b.PromedioQuimestresDouble == null ? (decimal?)null : Math.Round(b.PromedioQuimestresDouble ?? 0, 2, MidpointRounding.AwayFromZero),
                             PF = b.PromedioFinalDouble == null ? (decimal?)null : Math.Round(b.PromedioFinalDouble ?? 0, 2, MidpointRounding.AwayFromZero)
                         }).ToList();

                return Lista;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<ACA_014_EquivalenciaCualitativa_Info> get_list_EquivalenciaCualitativa(int IdEmpresa, int IdAnio)
        {
            try
            {

                List<ACA_014_EquivalenciaCualitativa_Info> Lista = new List<ACA_014_EquivalenciaCualitativa_Info>();
                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    var lst = Context.aca_AnioLectivoCalificacionCualitativa.Where(q => q.IdEmpresa == IdEmpresa && q.IdAnio == IdAnio).ToList();

                    foreach (var q in lst)
                    {
                        Lista.Add(new ACA_014_EquivalenciaCualitativa_Info
                        {
                            IdEmpresa = q.IdEmpresa,
                            IdAnio = q.IdAnio,
                            Codigo = q.Codigo,
                            IdCalificacionCualitativa = q.IdCalificacionCualitativa,
                            DescripcionCorta = q.DescripcionCorta,
                            DescripcionLarga = q.DescripcionLarga
                        });
                    }
                }

                return Lista;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<ACA_014_EquivalenciaConducta_Info> get_list_EquivalenciaConducta(int IdEmpresa, int IdAnio)
        {
            try
            {

                List<ACA_014_EquivalenciaConducta_Info> Lista = new List<ACA_014_EquivalenciaConducta_Info>();
                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    var lst = Context.aca_AnioLectivoConductaEquivalencia.Where(q => q.IdEmpresa == IdEmpresa && q.IdAnio == IdAnio).ToList();

                    foreach (var q in lst)
                    {
                        Lista.Add(new ACA_014_EquivalenciaConducta_Info
                        {
                            IdEmpresa = q.IdEmpresa,
                            IdAnio = q.IdAnio,
                            Letra = q.Letra,
                            Equivalencia = q.Equivalencia,
                            DescripcionEquivalencia = q.DescripcionEquivalencia
                        });
                    }
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
