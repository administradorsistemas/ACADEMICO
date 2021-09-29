using Core.Data.Base;
using Core.Info.Academico;
using Core.Info.Helps;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Academico
{
    public class aca_MatriculaCalificacion_Data
    {
        aca_Matricula_Data odata_matricula = new aca_Matricula_Data();
        aca_AnioLectivoEquivalenciaPromedio_Data odata_promedio_equivalencia = new aca_AnioLectivoEquivalenciaPromedio_Data();
        public List<aca_MatriculaCalificacion_Info> getList(int IdEmpresa, int IdSede, int IdAnio, int IdNivel, int IdJornada, int IdCurso, int IdParalelo, int IdMateria)
        {
            try
            {
                List<aca_MatriculaCalificacion_Info> Lista = new List<aca_MatriculaCalificacion_Info>();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();

                    #region Query
                    string query = "SELECT mc.IdEmpresa, mc.IdMatricula, m.IdAlumno, a.Codigo, per.pe_nombreCompleto AS pe_nombreCompletoAlumno, m.IdAnio, m.IdSede, m.IdNivel, m.IdJornada, m.IdCurso, m.IdParalelo, mc.IdMateria, mc.IdProfesor, pprof.pe_nombreCompleto, "
                    + " mc.CalificacionP1, mc.CalificacionP2, mc.CalificacionP3, mc.PromedioQ1, mc.ExamenQ1, mc.PromedioFinalQ1, mc.CausaQ1, mc.ResolucionQ1, mc.CalificacionP4, mc.CalificacionP5, mc.CalificacionP6, mc.PromedioQ2, mc.ExamenQ2, "
                    + " mc.PromedioFinalQ2, mc.CausaQ2, mc.ResolucionQ2, mc.PromedioQuimestres, mc.ExamenMejoramiento, mc.CampoMejoramiento, mc.ExamenSupletorio, mc.ExamenRemedial, mc.ExamenGracia, mc.PromedioFinal, equiv.Descripcion, "
                    + " equiv.Codigo AS CodigoEquivalencia, mc.IdEquivalenciaPromedioP1, mc.IdEquivalenciaPromedioP2, mc.IdEquivalenciaPromedioP3, mc.IdEquivalenciaPromedioEQ1, mc.IdEquivalenciaPromedioQ1, mc.IdEquivalenciaPromedioP4, "
                    + " mc.IdEquivalenciaPromedioP5, mc.IdEquivalenciaPromedioP6, mc.IdEquivalenciaPromedioEQ2, mc.IdEquivalenciaPromedioQ2, mc.IdEquivalenciaPromedioPF "
                    + " FROM     dbo.tb_persona AS pprof WITH (nolock) INNER JOIN "
                    + " .aca_Profesor AS pro WITH (nolock) ON pprof.IdPersona = pro.IdPersona RIGHT OUTER JOIN "
                    + " dbo.aca_Matricula AS m WITH (nolock) INNER JOIN "
                    + " dbo.aca_Alumno AS a WITH (nolock) ON m.IdEmpresa = a.IdEmpresa AND m.IdAlumno = a.IdAlumno INNER JOIN "
                    + " dbo.tb_persona AS per WITH (nolock) ON a.IdPersona = per.IdPersona INNER JOIN "
                    + " dbo.aca_MatriculaCalificacion AS mc WITH (nolock) ON m.IdEmpresa = mc.IdEmpresa AND m.IdMatricula = mc.IdMatricula AND m.IdEmpresa = mc.IdEmpresa ON pro.IdEmpresa = mc.IdEmpresa AND pro.IdProfesor = mc.IdProfesor LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivoEquivalenciaPromedio AS equiv WITH (nolock) ON mc.IdEmpresa = equiv.IdEmpresa AND mc.IdEquivalenciaPromedioPF = equiv.IdEquivalenciaPromedio "
                    + " WHERE(NOT EXISTS "
                    + " (SELECT IdEmpresa "
                    + " FROM      dbo.aca_AlumnoRetiro AS f WITH (nolock) "
                    + " WHERE(IdEmpresa = mc.IdEmpresa) AND(IdMatricula = mc.IdMatricula) AND(Estado = 1))) "
                    + " AND mc.IdEmpresa = " + IdEmpresa.ToString() + " and m.IdAnio = " + IdAnio.ToString() + " and m.IdSede = " + IdSede.ToString()
                    + " AND m.IdNivel = " + IdNivel.ToString() + " and m.IdJornada = " + IdJornada.ToString() + " and m.IdCurso = " + IdCurso.ToString()
                    + " AND m.IdParalelo = " + IdParalelo.ToString() + " and mc.IdMateria = " + IdMateria.ToString();
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 0;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new aca_MatriculaCalificacion_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdAlumno = Convert.ToDecimal(reader["IdAlumno"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            IdMatricula = Convert.ToDecimal(reader["IdMatricula"]),
                            IdMateria = Convert.ToInt32(reader["IdMateria"]),
                            IdProfesor = string.IsNullOrEmpty(reader["IdProfesor"].ToString()) ? (decimal?)null : Convert.ToInt32(reader["IdProfesor"]),
                            Codigo = string.IsNullOrEmpty(reader["Codigo"].ToString()) ? null : reader["Codigo"].ToString(),
                            pe_nombreCompletoAlumno = reader["pe_nombreCompletoAlumno"].ToString(),
                            pe_nombreCompleto = string.IsNullOrEmpty(reader["pe_nombreCompleto"].ToString()) ? null : reader["pe_nombreCompleto"].ToString(),
                        });
                    }
                    reader.Close();
                }
                
                return Lista;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<aca_MatriculaCalificacion_Info> getList_PaseAnio(int IdEmpresa, int IdSede, int IdAnio, int IdNivel, int IdJornada, int IdCurso, int IdParalelo, decimal IdAlumno)
        {
            try
            {
                List<aca_MatriculaCalificacion_Info> Lista = new List<aca_MatriculaCalificacion_Info>();
                int IdNivelIni = IdNivel;
                int IdNivelFin = IdNivel == 0 ? 9999999 : IdNivel;

                int IdJornadaIni = IdJornada;
                int IdJornadaFin = IdJornada == 0 ? 9999999 : IdJornada;

                int IdCursoIni = IdCurso;
                int IdCursoFin = IdCurso == 0 ? 9999999 : IdCurso;

                int IdParaleloIni = IdParalelo;
                int IdParaleloFin = IdParalelo == 0 ? 9999999 : IdParalelo;

                decimal IdAlumnoIni = IdAlumno;
                decimal IdAlumnoFin = IdAlumno == 0 ? 9999999 : IdAlumno;
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();

                    #region Query
                    string query = "SELECT mc.IdEmpresa, mc.IdMatricula, m.IdAlumno, a.Codigo, per.pe_nombreCompleto AS pe_nombreCompletoAlumno, m.IdAnio, m.IdSede, m.IdNivel, m.IdJornada, m.IdCurso, m.IdParalelo, mc.IdMateria, mc.IdProfesor, pprof.pe_nombreCompleto, "
                    + " mc.CalificacionP1, mc.CalificacionP2, mc.CalificacionP3, mc.PromedioQ1, mc.ExamenQ1, mc.PromedioFinalQ1, mc.CausaQ1, mc.ResolucionQ1, mc.CalificacionP4, mc.CalificacionP5, mc.CalificacionP6, mc.PromedioQ2, mc.ExamenQ2, "
                    + " mc.PromedioFinalQ2, mc.CausaQ2, mc.ResolucionQ2, mc.PromedioQuimestres, mc.ExamenMejoramiento, mc.CampoMejoramiento, mc.ExamenSupletorio, mc.ExamenRemedial, mc.ExamenGracia, mc.PromedioFinal, equiv.Descripcion, "
                    + " equiv.Codigo AS CodigoEquivalencia, mc.IdEquivalenciaPromedioP1, mc.IdEquivalenciaPromedioP2, mc.IdEquivalenciaPromedioP3, mc.IdEquivalenciaPromedioEQ1, mc.IdEquivalenciaPromedioQ1, mc.IdEquivalenciaPromedioP4, "
                    + " mc.IdEquivalenciaPromedioP5, mc.IdEquivalenciaPromedioP6, mc.IdEquivalenciaPromedioEQ2, mc.IdEquivalenciaPromedioQ2, mc.IdEquivalenciaPromedioPF "
                    + " FROM     dbo.tb_persona AS pprof WITH (nolock) INNER JOIN "
                    + " .aca_Profesor AS pro WITH (nolock) ON pprof.IdPersona = pro.IdPersona RIGHT OUTER JOIN "
                    + " dbo.aca_Matricula AS m WITH (nolock) INNER JOIN "
                    + " dbo.aca_Alumno AS a WITH (nolock) ON m.IdEmpresa = a.IdEmpresa AND m.IdAlumno = a.IdAlumno INNER JOIN "
                    + " dbo.tb_persona AS per WITH (nolock) ON a.IdPersona = per.IdPersona INNER JOIN "
                    + " dbo.aca_MatriculaCalificacion AS mc WITH (nolock) ON m.IdEmpresa = mc.IdEmpresa AND m.IdMatricula = mc.IdMatricula AND m.IdEmpresa = mc.IdEmpresa ON pro.IdEmpresa = mc.IdEmpresa AND pro.IdProfesor = mc.IdProfesor LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivoEquivalenciaPromedio AS equiv WITH (nolock) ON mc.IdEmpresa = equiv.IdEmpresa AND mc.IdEquivalenciaPromedioPF = equiv.IdEquivalenciaPromedio "
                    + " WHERE(NOT EXISTS "
                    + " (SELECT IdEmpresa "
                    + " FROM      dbo.aca_AlumnoRetiro AS f WITH (nolock) "
                    + " WHERE(IdEmpresa = mc.IdEmpresa) AND(IdMatricula = mc.IdMatricula) AND(Estado = 1))) "
                    + " AND mc.IdEmpresa = " + IdEmpresa.ToString() + " and m.IdAnio = " + IdAnio.ToString() + " and m.IdSede = " + IdSede.ToString()
                    + " AND m.IdNivel between " + IdNivelIni.ToString() + " and " + IdNivelFin.ToString()
                    + " and m.IdJornada between " + IdJornadaIni.ToString() + " and " + IdJornadaFin.ToString()
                    + " and m.IdCurso between " + IdCursoIni.ToString() + " and " + IdCursoFin.ToString()
                    + " AND m.IdParalelo between " + IdParaleloIni.ToString() + " and " + IdParaleloFin.ToString()
                    + " and m.IdAlumno between " + IdAlumnoIni.ToString() + " and " + IdAlumnoFin.ToString();
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 0;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new aca_MatriculaCalificacion_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdAlumno = Convert.ToDecimal(reader["IdAlumno"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            IdMatricula = Convert.ToDecimal(reader["IdMatricula"]),
                            IdSede = Convert.ToInt32(reader["IdSede"]),
                            IdJornada = Convert.ToInt32(reader["IdJornada"]),
                            IdNivel = Convert.ToInt32(reader["IdNivel"]),
                            IdCurso = Convert.ToInt32(reader["IdCurso"]),
                            IdParalelo = Convert.ToInt32(reader["IdParalelo"]),
                            IdMateria = Convert.ToInt32(reader["IdMateria"]),
                            IdProfesor = string.IsNullOrEmpty(reader["IdProfesor"].ToString()) ? (decimal?)null : Convert.ToInt32(reader["IdProfesor"]),
                            Codigo = string.IsNullOrEmpty(reader["Codigo"].ToString()) ? null : reader["Codigo"].ToString(),
                            pe_nombreCompletoAlumno = reader["pe_nombreCompletoAlumno"].ToString(),
                            pe_nombreCompleto = string.IsNullOrEmpty(reader["pe_nombreCompleto"].ToString()) ? null : reader["pe_nombreCompleto"].ToString(),
                            CalificacionP1 = string.IsNullOrEmpty(reader["CalificacionP1"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionP1"]),
                            CalificacionP2 = string.IsNullOrEmpty(reader["CalificacionP2"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionP2"]),
                            CalificacionP3 = string.IsNullOrEmpty(reader["CalificacionP3"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionP3"]),
                            CalificacionP4 = string.IsNullOrEmpty(reader["CalificacionP4"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionP4"]),
                            CalificacionP5 = string.IsNullOrEmpty(reader["CalificacionP5"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionP5"]),
                            CalificacionP6 = string.IsNullOrEmpty(reader["CalificacionP6"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionP6"]),
                            ExamenQ1 = string.IsNullOrEmpty(reader["ExamenQ1"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["ExamenQ1"]),
                            PromedioFinalQ1 = string.IsNullOrEmpty(reader["PromedioFinalQ1"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["PromedioFinalQ1"]),
                            CausaQ1 = string.IsNullOrEmpty(reader["CausaQ1"].ToString()) ? null : reader["CausaQ1"].ToString(),
                            ResolucionQ1 = string.IsNullOrEmpty(reader["ResolucionQ1"].ToString()) ? null : reader["ResolucionQ1"].ToString(),
                            ExamenQ2 = string.IsNullOrEmpty(reader["ExamenQ2"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["ExamenQ2"]),
                            PromedioFinalQ2 = string.IsNullOrEmpty(reader["PromedioFinalQ2"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["PromedioFinalQ2"]),
                            CausaQ2 = string.IsNullOrEmpty(reader["CausaQ2"].ToString()) ? null : reader["CausaQ2"].ToString(),
                            ResolucionQ2 = string.IsNullOrEmpty(reader["ResolucionQ2"].ToString()) ? null : reader["ResolucionQ2"].ToString(),
                            ExamenMejoramiento = string.IsNullOrEmpty(reader["ExamenMejoramiento"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["ExamenMejoramiento"]),
                            CampoMejoramiento = string.IsNullOrEmpty(reader["CampoMejoramiento"].ToString()) ? null : reader["CampoMejoramiento"].ToString(),
                            ExamenSupletorio = string.IsNullOrEmpty(reader["ExamenSupletorio"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["ExamenSupletorio"]),
                            ExamenRemedial = string.IsNullOrEmpty(reader["ExamenRemedial"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["ExamenRemedial"]),
                            ExamenGracia = string.IsNullOrEmpty(reader["ExamenGracia"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["ExamenGracia"]),
                            PromedioFinal = string.IsNullOrEmpty(reader["PromedioFinal"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["PromedioFinal"]),
                            RegistroValido = true,
                            CodigoEquivalencia = string.IsNullOrEmpty(reader["CodigoEquivalencia"].ToString()) ? null : reader["CodigoEquivalencia"].ToString(),
                            DescripcionEquivalencia = string.IsNullOrEmpty(reader["Descripcion"].ToString()) ? null : reader["Descripcion"].ToString(),
                            IdEquivalenciaPromedioP1 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioP1"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioP1"]),
                            IdEquivalenciaPromedioP2 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioP2"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioP2"]),
                            IdEquivalenciaPromedioP3 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioP3"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioP3"]),
                            IdEquivalenciaPromedioEQ1 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioEQ1"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioEQ1"]),
                            IdEquivalenciaPromedioQ1 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioQ1"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioQ1"]),
                            IdEquivalenciaPromedioP4 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioP4"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioP4"]),
                            IdEquivalenciaPromedioP5 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioP5"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioP5"]),
                            IdEquivalenciaPromedioP6 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioP6"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioP6"]),
                            IdEquivalenciaPromedioEQ2 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioEQ2"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioEQ2"]),
                            IdEquivalenciaPromedioQ2 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioQ2"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioQ2"]),
                            IdEquivalenciaPromedioPF = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioPF"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioPF"]),
                            PromedioQ1 = string.IsNullOrEmpty(reader["PromedioQ1"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["PromedioQ1"]),
                            PromedioQ2 = string.IsNullOrEmpty(reader["PromedioQ2"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["PromedioQ2"]),
                            PromedioQuimestres = string.IsNullOrEmpty(reader["PromedioQuimestres"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["PromedioQuimestres"])

                        });
                    }
                    reader.Close();
                }
                
                return Lista;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public aca_MatriculaCalificacion_Info getInfo_modificar(int IdEmpresa, int IdSede, int IdAnio, int IdNivel, int IdJornada, int IdCurso, int IdParalelo, int IdMateria, decimal IdAlumno)
        {
            try
            {
                aca_MatriculaCalificacion_Info info = new aca_MatriculaCalificacion_Info();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("", connection);
                    command.CommandText = "SELECT mc.IdEmpresa, mc.IdMatricula, m.IdAlumno, a.Codigo, per.pe_nombreCompleto AS pe_nombreCompletoAlumno, m.IdAnio, m.IdSede, m.IdNivel, m.IdJornada, m.IdCurso, m.IdParalelo, mc.IdMateria, mc.IdProfesor, pprof.pe_nombreCompleto, "
                    + " mc.CalificacionP1, mc.CalificacionP2, mc.CalificacionP3, mc.PromedioQ1, mc.ExamenQ1, mc.PromedioFinalQ1, mc.CausaQ1, mc.ResolucionQ1, mc.CalificacionP4, mc.CalificacionP5, mc.CalificacionP6, mc.PromedioQ2, mc.ExamenQ2, "
                    + " mc.PromedioFinalQ2, mc.CausaQ2, mc.ResolucionQ2, mc.PromedioQuimestres, mc.ExamenMejoramiento, mc.CampoMejoramiento, mc.ExamenSupletorio, mc.ExamenRemedial, mc.ExamenGracia, mc.PromedioFinal, equiv.Descripcion, "
                    + " equiv.Codigo AS CodigoEquivalencia, mc.IdEquivalenciaPromedioP1, mc.IdEquivalenciaPromedioP2, mc.IdEquivalenciaPromedioP3, mc.IdEquivalenciaPromedioEQ1, mc.IdEquivalenciaPromedioQ1, mc.IdEquivalenciaPromedioP4, "
                    + " mc.IdEquivalenciaPromedioP5, mc.IdEquivalenciaPromedioP6, mc.IdEquivalenciaPromedioEQ2, mc.IdEquivalenciaPromedioQ2, mc.IdEquivalenciaPromedioPF "
                    + " FROM dbo.tb_persona AS pprof WITH (nolock) INNER JOIN "
                    + " .aca_Profesor AS pro WITH (nolock) ON pprof.IdPersona = pro.IdPersona RIGHT OUTER JOIN "
                    + " dbo.aca_Matricula AS m WITH (nolock) INNER JOIN "
                    + " dbo.aca_Alumno AS a WITH (nolock) ON m.IdEmpresa = a.IdEmpresa AND m.IdAlumno = a.IdAlumno INNER JOIN "
                    + " dbo.tb_persona AS per WITH (nolock) ON a.IdPersona = per.IdPersona INNER JOIN "
                    + " dbo.aca_MatriculaCalificacion AS mc WITH (nolock) ON m.IdEmpresa = mc.IdEmpresa AND m.IdMatricula = mc.IdMatricula AND m.IdEmpresa = mc.IdEmpresa ON pro.IdEmpresa = mc.IdEmpresa AND pro.IdProfesor = mc.IdProfesor LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivoEquivalenciaPromedio AS equiv WITH (nolock) ON mc.IdEmpresa = equiv.IdEmpresa AND mc.IdEquivalenciaPromedioPF = equiv.IdEquivalenciaPromedio "
                    + " WHERE(NOT EXISTS "
                    + " (SELECT IdEmpresa "
                    + " FROM      dbo.aca_AlumnoRetiro AS f WITH (nolock) "
                    + " WHERE(IdEmpresa = mc.IdEmpresa) AND(IdMatricula = mc.IdMatricula) AND(Estado = 1))) "
                    + " AND mc.IdEmpresa = " + IdEmpresa.ToString() + " and m.IdAnio = " + IdAnio.ToString() + " and m.IdSede = " + IdSede.ToString()
                    + " AND m.IdNivel = " + IdNivel.ToString()
                    + " and m.IdJornada = " + IdJornada.ToString()
                    + " and m.IdCurso = " + IdCurso.ToString()
                    + " AND m.IdParalelo = " + IdParalelo.ToString()
                    + " and m.IdAlumno = " + IdAlumno.ToString()
                    + " and mc.IdMateria = " + IdMateria.ToString();
                    var ResultValue = command.ExecuteScalar();

                    if (ResultValue == null)
                        return null;

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        info = new aca_MatriculaCalificacion_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdAlumno = Convert.ToDecimal(reader["IdAlumno"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            IdMatricula = Convert.ToDecimal(reader["IdMatricula"]),
                            IdSede = Convert.ToInt32(reader["IdSede"]),
                            IdJornada = Convert.ToInt32(reader["IdJornada"]),
                            IdNivel = Convert.ToInt32(reader["IdNivel"]),
                            IdCurso = Convert.ToInt32(reader["IdCurso"]),
                            IdParalelo = Convert.ToInt32(reader["IdParalelo"]),
                            IdMateria = Convert.ToInt32(reader["IdMateria"]),
                            IdProfesor = string.IsNullOrEmpty(reader["IdProfesor"].ToString()) ? (decimal?)null : Convert.ToInt32(reader["IdProfesor"]),
                            Codigo = string.IsNullOrEmpty(reader["Codigo"].ToString()) ? null : reader["Codigo"].ToString(),
                            pe_nombreCompletoAlumno = reader["pe_nombreCompletoAlumno"].ToString(),
                            pe_nombreCompleto = string.IsNullOrEmpty(reader["pe_nombreCompleto"].ToString()) ? null : reader["pe_nombreCompleto"].ToString(),
                            CalificacionP1 = string.IsNullOrEmpty(reader["CalificacionP1"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionP1"]),
                            CalificacionP2 = string.IsNullOrEmpty(reader["CalificacionP2"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionP2"]),
                            CalificacionP3 = string.IsNullOrEmpty(reader["CalificacionP3"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionP3"]),
                            CalificacionP4 = string.IsNullOrEmpty(reader["CalificacionP4"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionP4"]),
                            CalificacionP5 = string.IsNullOrEmpty(reader["CalificacionP5"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionP5"]),
                            CalificacionP6 = string.IsNullOrEmpty(reader["CalificacionP6"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionP6"]),
                            ExamenQ1 = string.IsNullOrEmpty(reader["ExamenQ1"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["ExamenQ1"]),
                            PromedioFinalQ1 = string.IsNullOrEmpty(reader["PromedioFinalQ1"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["PromedioFinalQ1"]),
                            CausaQ1 = string.IsNullOrEmpty(reader["CausaQ1"].ToString()) ? null : reader["CausaQ1"].ToString(),
                            ResolucionQ1 = string.IsNullOrEmpty(reader["ResolucionQ1"].ToString()) ? null : reader["ResolucionQ1"].ToString(),
                            ExamenQ2 = string.IsNullOrEmpty(reader["ExamenQ2"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["ExamenQ2"]),
                            PromedioFinalQ2 = string.IsNullOrEmpty(reader["PromedioFinalQ2"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["PromedioFinalQ2"]),
                            CausaQ2 = string.IsNullOrEmpty(reader["CausaQ2"].ToString()) ? null : reader["CausaQ2"].ToString(),
                            ResolucionQ2 = string.IsNullOrEmpty(reader["ResolucionQ2"].ToString()) ? null : reader["ResolucionQ2"].ToString(),
                            ExamenMejoramiento = string.IsNullOrEmpty(reader["ExamenMejoramiento"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["ExamenMejoramiento"]),
                            CampoMejoramiento = string.IsNullOrEmpty(reader["CampoMejoramiento"].ToString()) ? null : reader["CampoMejoramiento"].ToString(),
                            ExamenSupletorio = string.IsNullOrEmpty(reader["ExamenSupletorio"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["ExamenSupletorio"]),
                            ExamenRemedial = string.IsNullOrEmpty(reader["ExamenRemedial"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["ExamenRemedial"]),
                            ExamenGracia = string.IsNullOrEmpty(reader["ExamenGracia"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["ExamenGracia"]),
                            PromedioFinal = string.IsNullOrEmpty(reader["PromedioFinal"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["PromedioFinal"]),
                            RegistroValido = true,
                            CodigoEquivalencia = string.IsNullOrEmpty(reader["CodigoEquivalencia"].ToString()) ? null : reader["CodigoEquivalencia"].ToString(),
                            DescripcionEquivalencia = string.IsNullOrEmpty(reader["Descripcion"].ToString()) ? null : reader["Descripcion"].ToString(),
                            IdEquivalenciaPromedioP1 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioP1"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioP1"]),
                            IdEquivalenciaPromedioP2 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioP2"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioP2"]),
                            IdEquivalenciaPromedioP3 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioP3"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioP3"]),
                            IdEquivalenciaPromedioEQ1 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioEQ1"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioEQ1"]),
                            IdEquivalenciaPromedioQ1 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioQ1"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioQ1"]),
                            IdEquivalenciaPromedioP4 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioP4"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioP4"]),
                            IdEquivalenciaPromedioP5 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioP5"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioP5"]),
                            IdEquivalenciaPromedioP6 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioP6"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioP6"]),
                            IdEquivalenciaPromedioEQ2 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioEQ2"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioEQ2"]),
                            IdEquivalenciaPromedioQ2 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioQ2"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioQ2"]),
                            IdEquivalenciaPromedioPF = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioPF"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioPF"]),
                            PromedioQ1 = string.IsNullOrEmpty(reader["PromedioQ1"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["PromedioQ1"]),
                            PromedioQ2 = string.IsNullOrEmpty(reader["PromedioQ2"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["PromedioQ2"]),
                            PromedioQuimestres = string.IsNullOrEmpty(reader["PromedioQuimestres"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["PromedioQuimestres"]),
                        };
                    }
                }
                
                return info;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<aca_MatriculaCalificacion_Info> getList_x_Profesor(int IdEmpresa, int IdSede, int IdAnio, int IdNivel, int IdJornada, int IdCurso, int IdParalelo, int IdMateria, decimal IdProfesor)
        {
            try
            {
                List<aca_MatriculaCalificacion_Info> Lista = new List<aca_MatriculaCalificacion_Info>();
                List<aca_MatriculaCalificacion_Info> ListaFinal = new List<aca_MatriculaCalificacion_Info>();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();

                    #region Query
                    string query = "SELECT mc.IdEmpresa, mc.IdMatricula, m.IdAlumno, a.Codigo, per.pe_nombreCompleto AS pe_nombreCompletoAlumno, m.IdAnio, m.IdSede, m.IdNivel, m.IdJornada, m.IdCurso, m.IdParalelo, mc.IdMateria, mc.IdProfesor, pprof.pe_nombreCompleto, "
                    + " mc.CalificacionP1, mc.CalificacionP2, mc.CalificacionP3, mc.PromedioQ1, mc.ExamenQ1, mc.PromedioFinalQ1, mc.CausaQ1, mc.ResolucionQ1, mc.CalificacionP4, mc.CalificacionP5, mc.CalificacionP6, mc.PromedioQ2, mc.ExamenQ2, "
                    + " mc.PromedioFinalQ2, mc.CausaQ2, mc.ResolucionQ2, mc.PromedioQuimestres, mc.ExamenMejoramiento, mc.CampoMejoramiento, mc.ExamenSupletorio, mc.ExamenRemedial, mc.ExamenGracia, mc.PromedioFinal, equiv.Descripcion, "
                    + " equiv.Codigo AS CodigoEquivalencia, mc.IdEquivalenciaPromedioP1, mc.IdEquivalenciaPromedioP2, mc.IdEquivalenciaPromedioP3, mc.IdEquivalenciaPromedioEQ1, mc.IdEquivalenciaPromedioQ1, mc.IdEquivalenciaPromedioP4, "
                    + " mc.IdEquivalenciaPromedioP5, mc.IdEquivalenciaPromedioP6, mc.IdEquivalenciaPromedioEQ2, mc.IdEquivalenciaPromedioQ2, mc.IdEquivalenciaPromedioPF "
                    + " FROM dbo.tb_persona AS pprof WITH (nolock) INNER JOIN "
                    + " .aca_Profesor AS pro WITH (nolock) ON pprof.IdPersona = pro.IdPersona RIGHT OUTER JOIN "
                    + " dbo.aca_Matricula AS m WITH (nolock) INNER JOIN "
                    + " dbo.aca_Alumno AS a WITH (nolock) ON m.IdEmpresa = a.IdEmpresa AND m.IdAlumno = a.IdAlumno INNER JOIN "
                    + " dbo.tb_persona AS per WITH (nolock) ON a.IdPersona = per.IdPersona INNER JOIN "
                    + " dbo.aca_MatriculaCalificacion AS mc WITH (nolock) ON m.IdEmpresa = mc.IdEmpresa AND m.IdMatricula = mc.IdMatricula AND m.IdEmpresa = mc.IdEmpresa ON pro.IdEmpresa = mc.IdEmpresa AND pro.IdProfesor = mc.IdProfesor LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivoEquivalenciaPromedio AS equiv WITH (nolock) ON mc.IdEmpresa = equiv.IdEmpresa AND mc.IdEquivalenciaPromedioPF = equiv.IdEquivalenciaPromedio "
                    + " WHERE(NOT EXISTS "
                    + " (SELECT IdEmpresa "
                    + " FROM      dbo.aca_AlumnoRetiro AS f WITH (nolock) "
                    + " WHERE(IdEmpresa = mc.IdEmpresa) AND(IdMatricula = mc.IdMatricula) AND(Estado = 1))) "
                    + " AND mc.IdEmpresa = " + IdEmpresa.ToString() + " and m.IdAnio = " + IdAnio.ToString() + " and m.IdSede = " + IdSede.ToString()
                    + " AND m.IdNivel = " + IdNivel.ToString() + " and m.IdJornada = " + IdJornada.ToString() + " and m.IdCurso = " + IdCurso.ToString()
                    + " AND m.IdParalelo = " + IdParalelo.ToString() + " and mc.IdMateria = " + IdMateria.ToString() + " and mc.IdProfesor = " + IdProfesor.ToString()
                    + " ORDER by per.pe_nombreCompleto ";
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 0;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new aca_MatriculaCalificacion_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdAlumno = Convert.ToDecimal(reader["IdAlumno"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            IdMatricula = Convert.ToDecimal(reader["IdMatricula"]),
                            IdSede = Convert.ToInt32(reader["IdSede"]),
                            IdJornada = Convert.ToInt32(reader["IdJornada"]),
                            IdNivel = Convert.ToInt32(reader["IdNivel"]),
                            IdCurso = Convert.ToInt32(reader["IdCurso"]),
                            IdParalelo = Convert.ToInt32(reader["IdParalelo"]),
                            IdMateria = Convert.ToInt32(reader["IdMateria"]),
                            IdProfesor = string.IsNullOrEmpty(reader["IdProfesor"].ToString()) ? (decimal?)null : Convert.ToInt32(reader["IdProfesor"]),
                            Codigo = string.IsNullOrEmpty(reader["Codigo"].ToString()) ? null : reader["Codigo"].ToString(),
                            pe_nombreCompletoAlumno = reader["pe_nombreCompletoAlumno"].ToString(),
                            pe_nombreCompleto = string.IsNullOrEmpty(reader["pe_nombreCompleto"].ToString()) ? null : reader["pe_nombreCompleto"].ToString(),
                            CalificacionP1 = string.IsNullOrEmpty(reader["CalificacionP1"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionP1"]),
                            CalificacionP2 = string.IsNullOrEmpty(reader["CalificacionP2"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionP2"]),
                            CalificacionP3 = string.IsNullOrEmpty(reader["CalificacionP3"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionP3"]),
                            CalificacionP4 = string.IsNullOrEmpty(reader["CalificacionP4"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionP4"]),
                            CalificacionP5 = string.IsNullOrEmpty(reader["CalificacionP5"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionP5"]),
                            CalificacionP6 = string.IsNullOrEmpty(reader["CalificacionP6"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionP6"]),
                            ExamenQ1 = string.IsNullOrEmpty(reader["ExamenQ1"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["ExamenQ1"]),
                            PromedioFinalQ1 = string.IsNullOrEmpty(reader["PromedioFinalQ1"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["PromedioFinalQ1"]),
                            CausaQ1 = string.IsNullOrEmpty(reader["CausaQ1"].ToString()) ? null : reader["CausaQ1"].ToString(),
                            ResolucionQ1 = string.IsNullOrEmpty(reader["ResolucionQ1"].ToString()) ? null : reader["ResolucionQ1"].ToString(),
                            ExamenQ2 = string.IsNullOrEmpty(reader["ExamenQ2"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["ExamenQ2"]),
                            PromedioFinalQ2 = string.IsNullOrEmpty(reader["PromedioFinalQ2"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["PromedioFinalQ2"]),
                            CausaQ2 = string.IsNullOrEmpty(reader["CausaQ2"].ToString()) ? null : reader["CausaQ2"].ToString(),
                            ResolucionQ2 = string.IsNullOrEmpty(reader["ResolucionQ2"].ToString()) ? null : reader["ResolucionQ2"].ToString(),
                            ExamenMejoramiento = string.IsNullOrEmpty(reader["ExamenMejoramiento"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["ExamenMejoramiento"]),
                            CampoMejoramiento = string.IsNullOrEmpty(reader["CampoMejoramiento"].ToString()) ? null : reader["CampoMejoramiento"].ToString(),
                            ExamenSupletorio = string.IsNullOrEmpty(reader["ExamenSupletorio"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["ExamenSupletorio"]),
                            ExamenRemedial = string.IsNullOrEmpty(reader["ExamenRemedial"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["ExamenRemedial"]),
                            ExamenGracia = string.IsNullOrEmpty(reader["ExamenGracia"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["ExamenGracia"]),
                            PromedioFinal = string.IsNullOrEmpty(reader["PromedioFinal"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["PromedioFinal"]),
                            RegistroValido = true,
                            CodigoEquivalencia = string.IsNullOrEmpty(reader["CodigoEquivalencia"].ToString()) ? null : reader["CodigoEquivalencia"].ToString(),
                            DescripcionEquivalencia = string.IsNullOrEmpty(reader["Descripcion"].ToString()) ? null : reader["Descripcion"].ToString(),
                            IdEquivalenciaPromedioP1 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioP1"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioP1"]),
                            IdEquivalenciaPromedioP2 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioP2"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioP2"]),
                            IdEquivalenciaPromedioP3 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioP3"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioP3"]),
                            IdEquivalenciaPromedioEQ1 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioEQ1"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioEQ1"]),
                            IdEquivalenciaPromedioQ1 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioQ1"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioQ1"]),
                            IdEquivalenciaPromedioP4 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioP4"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioP4"]),
                            IdEquivalenciaPromedioP5 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioP5"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioP5"]),
                            IdEquivalenciaPromedioP6 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioP6"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioP6"]),
                            IdEquivalenciaPromedioEQ2 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioEQ2"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioEQ2"]),
                            IdEquivalenciaPromedioQ2 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioQ2"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioQ2"]),
                            IdEquivalenciaPromedioPF = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioPF"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioPF"]),
                            PromedioQ1 = string.IsNullOrEmpty(reader["PromedioQ1"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["PromedioQ1"]),
                            PromedioQ2 = string.IsNullOrEmpty(reader["PromedioQ2"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["PromedioQ2"]),
                            PromedioQuimestres = string.IsNullOrEmpty(reader["PromedioQuimestres"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["PromedioQuimestres"]),
                        });
                    }
                    reader.Close();
                }

                ListaFinal = Lista.OrderBy(q => q.pe_nombreCompleto).ToList();
                return ListaFinal;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<aca_MatriculaCalificacion_Info> getList_SuperAdmin(int IdEmpresa, int IdSede, int IdAnio, int IdNivel, int IdJornada, int IdCurso, int IdParalelo, int IdMateria)
        {
            try
            {
                List<aca_MatriculaCalificacion_Info> Lista = new List<aca_MatriculaCalificacion_Info>();
                List<aca_MatriculaCalificacion_Info> ListaFinal = new List<aca_MatriculaCalificacion_Info>();

                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();

                    #region Query
                    string query = "SELECT mc.IdEmpresa, mc.IdMatricula, m.IdAlumno, a.Codigo, per.pe_nombreCompleto AS pe_nombreCompletoAlumno, m.IdAnio, m.IdSede, m.IdNivel, m.IdJornada, m.IdCurso, m.IdParalelo, mc.IdMateria, mc.IdProfesor, pprof.pe_nombreCompleto, "
                    + " mc.CalificacionP1, mc.CalificacionP2, mc.CalificacionP3, mc.PromedioQ1, mc.ExamenQ1, mc.PromedioFinalQ1, mc.CausaQ1, mc.ResolucionQ1, mc.CalificacionP4, mc.CalificacionP5, mc.CalificacionP6, mc.PromedioQ2, mc.ExamenQ2, "
                    + " mc.PromedioFinalQ2, mc.CausaQ2, mc.ResolucionQ2, mc.PromedioQuimestres, mc.ExamenMejoramiento, mc.CampoMejoramiento, mc.ExamenSupletorio, mc.ExamenRemedial, mc.ExamenGracia, mc.PromedioFinal, equiv.Descripcion, "
                    + " equiv.Codigo AS CodigoEquivalencia, mc.IdEquivalenciaPromedioP1, mc.IdEquivalenciaPromedioP2, mc.IdEquivalenciaPromedioP3, mc.IdEquivalenciaPromedioEQ1, mc.IdEquivalenciaPromedioQ1, mc.IdEquivalenciaPromedioP4, "
                    + " mc.IdEquivalenciaPromedioP5, mc.IdEquivalenciaPromedioP6, mc.IdEquivalenciaPromedioEQ2, mc.IdEquivalenciaPromedioQ2, mc.IdEquivalenciaPromedioPF "
                    + " FROM     dbo.tb_persona AS pprof WITH (nolock) INNER JOIN "
                    + " .aca_Profesor AS pro WITH (nolock) ON pprof.IdPersona = pro.IdPersona RIGHT OUTER JOIN "
                    + " dbo.aca_Matricula AS m WITH (nolock) INNER JOIN "
                    + " dbo.aca_Alumno AS a WITH (nolock) ON m.IdEmpresa = a.IdEmpresa AND m.IdAlumno = a.IdAlumno INNER JOIN "
                    + " dbo.tb_persona AS per WITH (nolock) ON a.IdPersona = per.IdPersona INNER JOIN "
                    + " dbo.aca_MatriculaCalificacion AS mc WITH (nolock) ON m.IdEmpresa = mc.IdEmpresa AND m.IdMatricula = mc.IdMatricula AND m.IdEmpresa = mc.IdEmpresa ON pro.IdEmpresa = mc.IdEmpresa AND pro.IdProfesor = mc.IdProfesor LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivoEquivalenciaPromedio AS equiv WITH (nolock) ON mc.IdEmpresa = equiv.IdEmpresa AND mc.IdEquivalenciaPromedioPF = equiv.IdEquivalenciaPromedio "
                    + " WHERE(NOT EXISTS "
                    + " (SELECT IdEmpresa "
                    + " FROM      dbo.aca_AlumnoRetiro AS f WITH (nolock) "
                    + " WHERE(IdEmpresa = mc.IdEmpresa) AND(IdMatricula = mc.IdMatricula) AND(Estado = 1))) "
                    + " AND mc.IdEmpresa = " + IdEmpresa.ToString() + " and m.IdAnio = " + IdAnio.ToString() + " and m.IdSede = " + IdSede.ToString()
                    + " AND m.IdNivel = " + IdNivel.ToString() + " and m.IdJornada = " + IdJornada.ToString() + " and m.IdCurso = " + IdCurso.ToString()
                    + " AND m.IdParalelo = " + IdParalelo.ToString() + " and mc.IdMateria = " + IdMateria.ToString()
                    + " ORDER by per.pe_nombreCompleto ";
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 0;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new aca_MatriculaCalificacion_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdAlumno = Convert.ToDecimal(reader["IdAlumno"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            IdMatricula = Convert.ToDecimal(reader["IdMatricula"]),
                            IdSede = Convert.ToInt32(reader["IdSede"]),
                            IdJornada = Convert.ToInt32(reader["IdJornada"]),
                            IdNivel = Convert.ToInt32(reader["IdNivel"]),
                            IdCurso = Convert.ToInt32(reader["IdCurso"]),
                            IdParalelo = Convert.ToInt32(reader["IdParalelo"]),
                            IdMateria = Convert.ToInt32(reader["IdMateria"]),
                            IdProfesor = string.IsNullOrEmpty(reader["IdProfesor"].ToString()) ? (decimal?)null : Convert.ToInt32(reader["IdProfesor"]),
                            Codigo = string.IsNullOrEmpty(reader["Codigo"].ToString()) ? null : reader["Codigo"].ToString(),
                            pe_nombreCompletoAlumno = reader["pe_nombreCompletoAlumno"].ToString(),
                            pe_nombreCompleto = string.IsNullOrEmpty(reader["pe_nombreCompleto"].ToString()) ? null : reader["pe_nombreCompleto"].ToString(),
                            CalificacionP1 = string.IsNullOrEmpty(reader["CalificacionP1"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionP1"]),
                            CalificacionP2 = string.IsNullOrEmpty(reader["CalificacionP2"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionP2"]),
                            CalificacionP3 = string.IsNullOrEmpty(reader["CalificacionP3"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionP3"]),
                            CalificacionP4 = string.IsNullOrEmpty(reader["CalificacionP4"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionP4"]),
                            CalificacionP5 = string.IsNullOrEmpty(reader["CalificacionP5"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionP5"]),
                            CalificacionP6 = string.IsNullOrEmpty(reader["CalificacionP6"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionP6"]),
                            ExamenQ1 = string.IsNullOrEmpty(reader["ExamenQ1"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["ExamenQ1"]),
                            PromedioFinalQ1 = string.IsNullOrEmpty(reader["PromedioFinalQ1"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["PromedioFinalQ1"]),
                            CausaQ1 = string.IsNullOrEmpty(reader["CausaQ1"].ToString()) ? null : reader["CausaQ1"].ToString(),
                            ResolucionQ1 = string.IsNullOrEmpty(reader["ResolucionQ1"].ToString()) ? null : reader["ResolucionQ1"].ToString(),
                            ExamenQ2 = string.IsNullOrEmpty(reader["ExamenQ2"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["ExamenQ2"]),
                            PromedioFinalQ2 = string.IsNullOrEmpty(reader["PromedioFinalQ2"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["PromedioFinalQ2"]),
                            CausaQ2 = string.IsNullOrEmpty(reader["CausaQ2"].ToString()) ? null : reader["CausaQ2"].ToString(),
                            ResolucionQ2 = string.IsNullOrEmpty(reader["ResolucionQ2"].ToString()) ? null : reader["ResolucionQ2"].ToString(),
                            ExamenMejoramiento = string.IsNullOrEmpty(reader["ExamenMejoramiento"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["ExamenMejoramiento"]),
                            CampoMejoramiento = string.IsNullOrEmpty(reader["CampoMejoramiento"].ToString()) ? null : reader["CampoMejoramiento"].ToString(),
                            ExamenSupletorio = string.IsNullOrEmpty(reader["ExamenSupletorio"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["ExamenSupletorio"]),
                            ExamenRemedial = string.IsNullOrEmpty(reader["ExamenRemedial"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["ExamenRemedial"]),
                            ExamenGracia = string.IsNullOrEmpty(reader["ExamenGracia"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["ExamenGracia"]),
                            PromedioFinal = string.IsNullOrEmpty(reader["PromedioFinal"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["PromedioFinal"]),
                            RegistroValido = true,
                            CodigoEquivalencia = string.IsNullOrEmpty(reader["CodigoEquivalencia"].ToString()) ? null : reader["CodigoEquivalencia"].ToString(),
                            DescripcionEquivalencia = string.IsNullOrEmpty(reader["Descripcion"].ToString()) ? null : reader["Descripcion"].ToString(),
                            IdEquivalenciaPromedioP1 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioP1"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioP1"]),
                            IdEquivalenciaPromedioP2 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioP2"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioP2"]),
                            IdEquivalenciaPromedioP3 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioP3"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioP3"]),
                            IdEquivalenciaPromedioEQ1 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioEQ1"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioEQ1"]),
                            IdEquivalenciaPromedioQ1 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioQ1"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioQ1"]),
                            IdEquivalenciaPromedioP4 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioP4"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioP4"]),
                            IdEquivalenciaPromedioP5 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioP5"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioP5"]),
                            IdEquivalenciaPromedioP6 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioP6"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioP6"]),
                            IdEquivalenciaPromedioEQ2 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioEQ2"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioEQ2"]),
                            IdEquivalenciaPromedioQ2 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioQ2"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioQ2"]),
                            IdEquivalenciaPromedioPF = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioPF"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioPF"]),
                            PromedioQ1 = string.IsNullOrEmpty(reader["PromedioQ1"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["PromedioQ1"]),
                            PromedioQ2 = string.IsNullOrEmpty(reader["PromedioQ2"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["PromedioQ2"]),
                            PromedioQuimestres = string.IsNullOrEmpty(reader["PromedioQuimestres"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["PromedioQuimestres"]),
                        });
                    }
                    reader.Close();
                }
                ListaFinal = Lista.OrderBy(q => q.pe_nombreCompleto).ToList();
                return ListaFinal;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public aca_MatriculaCalificacion_Info getInfo(int IdEmpresa, int IdSede, int IdAnio, int IdNivel, int IdJornada, int IdCurso, int IdParalelo, int IdMateria, decimal IdAlumno)
        {
            try
            {
                aca_MatriculaCalificacion_Info info = new aca_MatriculaCalificacion_Info();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("", connection);
                    command.CommandText = "SELECT mc.IdEmpresa, mc.IdMatricula, m.IdAlumno, a.Codigo, per.pe_nombreCompleto AS pe_nombreCompletoAlumno, m.IdAnio, m.IdSede, m.IdNivel, m.IdJornada, m.IdCurso, m.IdParalelo, mc.IdMateria, mc.IdProfesor, pprof.pe_nombreCompleto, "
                    + " mc.CalificacionP1, mc.CalificacionP2, mc.CalificacionP3, mc.PromedioQ1, mc.ExamenQ1, mc.PromedioFinalQ1, mc.CausaQ1, mc.ResolucionQ1, mc.CalificacionP4, mc.CalificacionP5, mc.CalificacionP6, mc.PromedioQ2, mc.ExamenQ2, "
                    + " mc.PromedioFinalQ2, mc.CausaQ2, mc.ResolucionQ2, mc.PromedioQuimestres, mc.ExamenMejoramiento, mc.CampoMejoramiento, mc.ExamenSupletorio, mc.ExamenRemedial, mc.ExamenGracia, mc.PromedioFinal, equiv.Descripcion, "
                    + " equiv.Codigo AS CodigoEquivalencia, mc.IdEquivalenciaPromedioP1, mc.IdEquivalenciaPromedioP2, mc.IdEquivalenciaPromedioP3, mc.IdEquivalenciaPromedioEQ1, mc.IdEquivalenciaPromedioQ1, mc.IdEquivalenciaPromedioP4, "
                    + " mc.IdEquivalenciaPromedioP5, mc.IdEquivalenciaPromedioP6, mc.IdEquivalenciaPromedioEQ2, mc.IdEquivalenciaPromedioQ2, mc.IdEquivalenciaPromedioPF "
                    + " FROM     dbo.tb_persona AS pprof WITH (nolock) INNER JOIN "
                    + " dbo.aca_Profesor AS pro WITH (nolock) ON pprof.IdPersona = pro.IdPersona RIGHT OUTER JOIN "
                    + " dbo.aca_Matricula AS m WITH (nolock) INNER JOIN "
                    + " dbo.aca_Alumno AS a WITH (nolock) ON m.IdEmpresa = a.IdEmpresa AND m.IdAlumno = a.IdAlumno INNER JOIN "
                    + " dbo.tb_persona AS per WITH (nolock) ON a.IdPersona = per.IdPersona INNER JOIN "
                    + " dbo.aca_MatriculaCalificacion AS mc WITH (nolock) ON m.IdEmpresa = mc.IdEmpresa AND m.IdMatricula = mc.IdMatricula AND m.IdEmpresa = mc.IdEmpresa ON pro.IdEmpresa = mc.IdEmpresa AND pro.IdProfesor = mc.IdProfesor LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivoEquivalenciaPromedio AS equiv WITH (nolock) ON mc.IdEmpresa = equiv.IdEmpresa AND mc.IdEquivalenciaPromedioPF = equiv.IdEquivalenciaPromedio "
                    + " WHERE(NOT EXISTS "
                    + " (SELECT IdEmpresa "
                    + " FROM      dbo.aca_AlumnoRetiro AS f WITH (nolock) "
                    + " WHERE(IdEmpresa = mc.IdEmpresa) AND(IdMatricula = mc.IdMatricula) AND(Estado = 1))) "
                    + " AND mc.IdEmpresa = " + IdEmpresa.ToString() + " and m.IdAnio = " + IdAnio.ToString() + " and m.IdSede = " + IdSede.ToString()
                    + " AND m.IdNivel = " + IdNivel.ToString() + " and m.IdJornada = " + IdJornada.ToString() + " and m.IdCurso = " + IdCurso.ToString()
                    + " AND m.IdParalelo = " + IdParalelo.ToString() + " and mc.IdMateria = " + IdMateria.ToString() + " and m.IdAlumno = " + IdAlumno.ToString()
                    + " ORDER by per.pe_nombreCompleto ";
                    var ResultValue = command.ExecuteScalar();

                    if (ResultValue == null)
                        return null;

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        info = new aca_MatriculaCalificacion_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdAlumno = Convert.ToDecimal(reader["IdAlumno"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            IdMatricula = Convert.ToDecimal(reader["IdMatricula"]),
                            IdSede = Convert.ToInt32(reader["IdSede"]),
                            IdJornada = Convert.ToInt32(reader["IdJornada"]),
                            IdNivel = Convert.ToInt32(reader["IdNivel"]),
                            IdCurso = Convert.ToInt32(reader["IdCurso"]),
                            IdParalelo = Convert.ToInt32(reader["IdParalelo"]),
                            IdMateria = Convert.ToInt32(reader["IdMateria"]),
                            IdProfesor = string.IsNullOrEmpty(reader["IdProfesor"].ToString()) ? (decimal?)null : Convert.ToInt32(reader["IdProfesor"]),
                            Codigo = string.IsNullOrEmpty(reader["Codigo"].ToString()) ? null : reader["Codigo"].ToString(),
                            pe_nombreCompletoAlumno = reader["pe_nombreCompletoAlumno"].ToString(),
                            pe_nombreCompleto = string.IsNullOrEmpty(reader["pe_nombreCompleto"].ToString()) ? null : reader["pe_nombreCompleto"].ToString(),
                            CalificacionP1 = string.IsNullOrEmpty(reader["CalificacionP1"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionP1"]),
                            CalificacionP2 = string.IsNullOrEmpty(reader["CalificacionP2"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionP2"]),
                            CalificacionP3 = string.IsNullOrEmpty(reader["CalificacionP3"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionP3"]),
                            CalificacionP4 = string.IsNullOrEmpty(reader["CalificacionP4"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionP4"]),
                            CalificacionP5 = string.IsNullOrEmpty(reader["CalificacionP5"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionP5"]),
                            CalificacionP6 = string.IsNullOrEmpty(reader["CalificacionP6"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionP6"]),
                            ExamenQ1 = string.IsNullOrEmpty(reader["ExamenQ1"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["ExamenQ1"]),
                            PromedioFinalQ1 = string.IsNullOrEmpty(reader["PromedioFinalQ1"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["PromedioFinalQ1"]),
                            CausaQ1 = string.IsNullOrEmpty(reader["CausaQ1"].ToString()) ? null : reader["CausaQ1"].ToString(),
                            ResolucionQ1 = string.IsNullOrEmpty(reader["ResolucionQ1"].ToString()) ? null : reader["ResolucionQ1"].ToString(),
                            ExamenQ2 = string.IsNullOrEmpty(reader["ExamenQ2"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["ExamenQ2"]),
                            PromedioFinalQ2 = string.IsNullOrEmpty(reader["PromedioFinalQ2"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["PromedioFinalQ2"]),
                            CausaQ2 = string.IsNullOrEmpty(reader["CausaQ2"].ToString()) ? null : reader["CausaQ2"].ToString(),
                            ResolucionQ2 = string.IsNullOrEmpty(reader["ResolucionQ2"].ToString()) ? null : reader["ResolucionQ2"].ToString(),
                            ExamenMejoramiento = string.IsNullOrEmpty(reader["ExamenMejoramiento"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["ExamenMejoramiento"]),
                            CampoMejoramiento = string.IsNullOrEmpty(reader["CampoMejoramiento"].ToString()) ? null : reader["CampoMejoramiento"].ToString(),
                            ExamenSupletorio = string.IsNullOrEmpty(reader["ExamenSupletorio"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["ExamenSupletorio"]),
                            ExamenRemedial = string.IsNullOrEmpty(reader["ExamenRemedial"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["ExamenRemedial"]),
                            ExamenGracia = string.IsNullOrEmpty(reader["ExamenGracia"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["ExamenGracia"]),
                            PromedioFinal = string.IsNullOrEmpty(reader["PromedioFinal"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["PromedioFinal"]),
                            RegistroValido = true,
                            CodigoEquivalencia = string.IsNullOrEmpty(reader["CodigoEquivalencia"].ToString()) ? null : reader["CodigoEquivalencia"].ToString(),
                            DescripcionEquivalencia = string.IsNullOrEmpty(reader["Descripcion"].ToString()) ? null : reader["Descripcion"].ToString(),
                            IdEquivalenciaPromedioP1 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioP1"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioP1"]),
                            IdEquivalenciaPromedioP2 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioP2"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioP2"]),
                            IdEquivalenciaPromedioP3 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioP3"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioP3"]),
                            IdEquivalenciaPromedioEQ1 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioEQ1"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioEQ1"]),
                            IdEquivalenciaPromedioQ1 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioQ1"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioQ1"]),
                            IdEquivalenciaPromedioP4 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioP4"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioP4"]),
                            IdEquivalenciaPromedioP5 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioP5"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioP5"]),
                            IdEquivalenciaPromedioP6 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioP6"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioP6"]),
                            IdEquivalenciaPromedioEQ2 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioEQ2"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioEQ2"]),
                            IdEquivalenciaPromedioQ2 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioQ2"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioQ2"]),
                            IdEquivalenciaPromedioPF = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioPF"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioPF"]),
                            PromedioQ1 = string.IsNullOrEmpty(reader["PromedioQ1"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["PromedioQ1"]),
                            PromedioQ2 = string.IsNullOrEmpty(reader["PromedioQ2"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["PromedioQ2"]),
                            PromedioQuimestres = string.IsNullOrEmpty(reader["PromedioQuimestres"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["PromedioQuimestres"])
                        };
                    }
                }
                
                return info;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public aca_MatriculaCalificacion_Info getInfo_X_Matricula(int IdEmpresa, decimal IdMatricula, decimal IdMateria)
        {
            try
            {
                aca_MatriculaCalificacion_Info info = new aca_MatriculaCalificacion_Info();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("", connection);
                    command.CommandText = "SELECT * FROM aca_MatriculaCalificacion WITH (nolock) "
                    + " WHERE IdEmpresa = " + IdEmpresa.ToString() + " and IdMatricula = " + IdMatricula.ToString() + " and IdMateria = " + IdMateria.ToString();
                    var ResultValue = command.ExecuteScalar();

                    if (ResultValue == null)
                        return null;

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        info = new aca_MatriculaCalificacion_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdMatricula = Convert.ToDecimal(reader["IdMatricula"]),
                            IdMateria = Convert.ToInt32(reader["IdMateria"]),
                            IdProfesor = string.IsNullOrEmpty(reader["IdProfesor"].ToString()) ? (decimal?)null : Convert.ToInt32(reader["IdProfesor"]),
                            CalificacionP1 = string.IsNullOrEmpty(reader["CalificacionP1"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionP1"]),
                            CalificacionP2 = string.IsNullOrEmpty(reader["CalificacionP2"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionP2"]),
                            CalificacionP3 = string.IsNullOrEmpty(reader["CalificacionP3"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionP3"]),
                            CalificacionP4 = string.IsNullOrEmpty(reader["CalificacionP4"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionP4"]),
                            CalificacionP5 = string.IsNullOrEmpty(reader["CalificacionP5"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionP5"]),
                            CalificacionP6 = string.IsNullOrEmpty(reader["CalificacionP6"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["CalificacionP6"]),
                            ExamenQ1 = string.IsNullOrEmpty(reader["ExamenQ1"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["ExamenQ1"]),
                            PromedioFinalQ1 = string.IsNullOrEmpty(reader["PromedioFinalQ1"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["PromedioFinalQ1"]),
                            CausaQ1 = string.IsNullOrEmpty(reader["CausaQ1"].ToString()) ? null : reader["CausaQ1"].ToString(),
                            ResolucionQ1 = string.IsNullOrEmpty(reader["ResolucionQ1"].ToString()) ? null : reader["ResolucionQ1"].ToString(),
                            ExamenQ2 = string.IsNullOrEmpty(reader["ExamenQ2"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["ExamenQ2"]),
                            PromedioFinalQ2 = string.IsNullOrEmpty(reader["PromedioFinalQ2"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["PromedioFinalQ2"]),
                            CausaQ2 = string.IsNullOrEmpty(reader["CausaQ2"].ToString()) ? null : reader["CausaQ2"].ToString(),
                            ResolucionQ2 = string.IsNullOrEmpty(reader["ResolucionQ2"].ToString()) ? null : reader["ResolucionQ2"].ToString(),
                            ExamenMejoramiento = string.IsNullOrEmpty(reader["ExamenMejoramiento"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["ExamenMejoramiento"]),
                            CampoMejoramiento = string.IsNullOrEmpty(reader["CampoMejoramiento"].ToString()) ? null : reader["CampoMejoramiento"].ToString(),
                            ExamenSupletorio = string.IsNullOrEmpty(reader["ExamenSupletorio"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["ExamenSupletorio"]),
                            ExamenRemedial = string.IsNullOrEmpty(reader["ExamenRemedial"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["ExamenRemedial"]),
                            ExamenGracia = string.IsNullOrEmpty(reader["ExamenGracia"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["ExamenGracia"]),
                            PromedioFinal = string.IsNullOrEmpty(reader["PromedioFinal"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["PromedioFinal"]),
                            RegistroValido = true,
                            IdEquivalenciaPromedioP1 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioP1"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioP1"]),
                            IdEquivalenciaPromedioP2 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioP2"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioP2"]),
                            IdEquivalenciaPromedioP3 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioP3"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioP3"]),
                            IdEquivalenciaPromedioEQ1 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioEQ1"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioEQ1"]),
                            IdEquivalenciaPromedioQ1 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioQ1"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioQ1"]),
                            IdEquivalenciaPromedioP4 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioP4"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioP4"]),
                            IdEquivalenciaPromedioP5 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioP5"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioP5"]),
                            IdEquivalenciaPromedioP6 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioP6"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioP6"]),
                            IdEquivalenciaPromedioEQ2 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioEQ2"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioEQ2"]),
                            IdEquivalenciaPromedioQ2 = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioQ2"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioQ2"]),
                            IdEquivalenciaPromedioPF = string.IsNullOrEmpty(reader["IdEquivalenciaPromedioPF"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEquivalenciaPromedioPF"]),
                            PromedioQ1 = string.IsNullOrEmpty(reader["PromedioQ1"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["PromedioQ1"]),
                            PromedioQ2 = string.IsNullOrEmpty(reader["PromedioQ2"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["PromedioQ2"]),
                            PromedioQuimestres = string.IsNullOrEmpty(reader["PromedioQuimestres"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["PromedioQuimestres"])
                        };
                    }
                }

                return info;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool guardarDB(int IdEmpresa, int IdSede, int IdAnio, int IdNivel, int IdJornada, int IdCurso, int IdParalelo, int IdMateria, List<aca_MatriculaCalificacion_Info> lista)
        {
            try
            {
                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    var info_anio_curso = Context.aca_AnioLectivo.Where(q => q.IdEmpresa == IdEmpresa && q.EnCurso == true).FirstOrDefault();
                    if (info_anio_curso != null)
                    {
                        var lst_matricula = Context.aca_Matricula.Where(q => q.IdEmpresa == IdEmpresa && q.IdSede == IdSede && q.IdAnio == IdAnio && q.IdNivel == IdNivel && q.IdJornada == IdJornada && q.IdCurso == IdCurso && q.IdParalelo == IdParalelo).ToList();
                        if (lst_matricula != null && lst_matricula.Count > 0)
                        {
                            foreach (var item in lista)
                            {
                                var info_matricula = lst_matricula.Where(q => q.IdEmpresa == IdEmpresa && q.IdSede == IdSede && q.IdAnio == IdAnio && q.IdNivel == IdNivel && q.IdJornada == IdJornada && q.IdCurso == IdCurso && q.IdParalelo == IdParalelo && q.IdAlumno == item.IdAlumno).FirstOrDefault();
                                if (info_matricula!=null)
                                {
                                    var lst_MatriculaCalificacionParcial = Context.aca_MatriculaCalificacionParcial.Where(q => q.IdEmpresa == IdEmpresa && q.IdMatricula == info_matricula.IdMatricula && q.IdMateria == IdMateria).ToList();
                                    if (lst_MatriculaCalificacionParcial.Count > 0)
                                    {
                                        lst_MatriculaCalificacionParcial.ForEach(q => q.IdProfesor = item.IdProfesor);
                                    }

                                    var lst_MatriculaCalificacion = Context.aca_MatriculaCalificacion.Where(q => q.IdEmpresa == IdEmpresa && q.IdMatricula == item.IdMatricula && q.IdMateria == IdMateria).ToList();
                                    if (lst_MatriculaCalificacion.Count > 0)
                                    {
                                        lst_MatriculaCalificacion.ForEach(q => q.IdProfesor = item.IdProfesor);
                                    }
                                }
                                
                            }
                        }
                    }
                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<aca_MatriculaCalificacion_Info> getList(int IdEmpresa, decimal IdMatricula)
        {
            try
            {
                List<aca_MatriculaCalificacion_Info> Lista = new List<aca_MatriculaCalificacion_Info>();

                using (EntitiesAcademico odata = new EntitiesAcademico())
                {
                    var lst = odata.aca_MatriculaCalificacion.Where(q => q.IdEmpresa == IdEmpresa && q.IdMatricula== IdMatricula).ToList();

                    lst.ForEach(q =>
                    {
                        Lista.Add(new aca_MatriculaCalificacion_Info
                        {
                            IdEmpresa = q.IdEmpresa,
                            IdMatricula = q.IdMatricula,
                            IdMateria = q.IdMateria,
                            IdProfesor = q.IdProfesor,
                            CalificacionP1 = q.CalificacionP1,
                            CalificacionP2 = q.CalificacionP2,
                            CalificacionP3=q.CalificacionP3,
                            CalificacionP4 = q.CalificacionP4,
                            CalificacionP5 = q.CalificacionP5,
                            CalificacionP6=q.CalificacionP6,
                            ExamenQ1=q.ExamenQ1,
                            ExamenQ2=q.ExamenQ2,
                            ExamenRemedial=q.ExamenRemedial,
                            ExamenMejoramiento = q.ExamenMejoramiento,
                            CampoMejoramiento = q.CampoMejoramiento,
                            ExamenSupletorio = q.ExamenSupletorio,
                            ExamenGracia=q.ExamenGracia,
                            PromedioQ1 = q.PromedioQ1,
                            PromedioQ2=q.PromedioQ2,
                            PromedioFinalQ1=q.PromedioFinalQ1,
                            PromedioFinalQ2=q.PromedioFinalQ2,
                            PromedioFinal =q.PromedioFinal,
                            IdEquivalenciaPromedioP1 = q.IdEquivalenciaPromedioP1,
                            IdEquivalenciaPromedioP2 = q.IdEquivalenciaPromedioP2,
                            IdEquivalenciaPromedioP3 = q.IdEquivalenciaPromedioP2,
                            IdEquivalenciaPromedioEQ1 = q.IdEquivalenciaPromedioEQ1,
                            IdEquivalenciaPromedioQ1 = q.IdEquivalenciaPromedioQ1,
                            IdEquivalenciaPromedioP4 = q.IdEquivalenciaPromedioP4,
                            IdEquivalenciaPromedioP5 = q.IdEquivalenciaPromedioP5,
                            IdEquivalenciaPromedioP6 = q.IdEquivalenciaPromedioP6,
                            IdEquivalenciaPromedioEQ2 = q.IdEquivalenciaPromedioEQ2,
                            IdEquivalenciaPromedioQ2 = q.IdEquivalenciaPromedioQ2,
                            IdEquivalenciaPromedioPF = q.IdEquivalenciaPromedioPF
                        });
                    });
                }

                return Lista;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool generarCalificacion(List<aca_MatriculaCalificacion_Info> lst_calificacion)
        {
            try
            {
                List<aca_MatriculaCalificacion_Info> Lista = new List<aca_MatriculaCalificacion_Info>();

                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    var lst_matricula = (from q in lst_calificacion
                                         group q by new
                                         {
                                             q.IdEmpresa,
                                             q.IdMatricula
                                         } into mat
                                         select new aca_Matricula_Info
                                         {
                                             IdEmpresa = mat.Key.IdEmpresa,
                                             IdMatricula = mat.Key.IdMatricula
                                         }).ToList();

                    foreach (var item in lst_matricula)
                    {
                        var lista_calificacion = Context.aca_MatriculaCalificacion.Where(q => q.IdEmpresa == item.IdEmpresa && q.IdMatricula == item.IdMatricula).ToList();
                        Context.aca_MatriculaCalificacion.RemoveRange(lista_calificacion);

                        var lst_x_matricula = lst_calificacion.Where(q => q.IdEmpresa == item.IdEmpresa && q.IdMatricula == item.IdMatricula).ToList();

                        if (lst_x_matricula != null)
                        {
                            foreach (var info in lst_x_matricula)
                            {
                                aca_MatriculaCalificacion Entity = new aca_MatriculaCalificacion
                                {
                                    IdEmpresa = info.IdEmpresa,
                                    IdMatricula = info.IdMatricula,
                                    IdMateria = info.IdMateria,
                                    IdProfesor = info.IdProfesor,
                                    CalificacionP1 = info.CalificacionP1,
                                    CalificacionP2= info.CalificacionP2,
                                    CalificacionP3 = info.CalificacionP3,
                                    CalificacionP4 = info.CalificacionP4,
                                    CalificacionP5 = info.CalificacionP5,
                                    CalificacionP6 = info.CalificacionP6,
                                    PromedioQ1 = info.PromedioQ1,
                                    ExamenQ1 = info.ExamenQ1,
                                    PromedioFinalQ1 = info.PromedioFinalQ1,
                                    PromedioQ2 = info.PromedioQ2,
                                    ExamenQ2 = info.ExamenQ2,
                                    PromedioFinalQ2 = info.PromedioFinalQ2,
                                    PromedioQuimestres =info.PromedioQuimestres,
                                    ExamenMejoramiento = info.ExamenMejoramiento,
                                    CampoMejoramiento = info.CampoMejoramiento,
                                    ExamenSupletorio = info.ExamenSupletorio,
                                    ExamenRemedial = info.ExamenRemedial,
                                    ExamenGracia = info.ExamenGracia,
                                    PromedioFinal = info.PromedioFinal,
                                    IdEquivalenciaPromedioP1 = info.IdEquivalenciaPromedioP1,
                                    IdEquivalenciaPromedioP2 = info.IdEquivalenciaPromedioP2,
                                    IdEquivalenciaPromedioP3 = info.IdEquivalenciaPromedioP2,
                                    IdEquivalenciaPromedioEQ1 = info.IdEquivalenciaPromedioEQ1,
                                    IdEquivalenciaPromedioQ1 = info.IdEquivalenciaPromedioQ1,
                                    IdEquivalenciaPromedioP4 = info.IdEquivalenciaPromedioP4,
                                    IdEquivalenciaPromedioP5 = info.IdEquivalenciaPromedioP5,
                                    IdEquivalenciaPromedioP6 = info.IdEquivalenciaPromedioP6,
                                    IdEquivalenciaPromedioEQ2 = info.IdEquivalenciaPromedioEQ2,
                                    IdEquivalenciaPromedioQ2 = info.IdEquivalenciaPromedioQ2,
                                    IdEquivalenciaPromedioPF = info.IdEquivalenciaPromedioPF,
                                };

                                Context.aca_MatriculaCalificacion.Add(Entity);
                            }
                        }
                        Context.SaveChanges();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<aca_MatriculaCalificacion_Info> getList_Combos(int IdEmpresa, int IdAnio, int IdSede)
        {
            try
            {
                List<aca_MatriculaCalificacion_Info> Lista = new List<aca_MatriculaCalificacion_Info>();

                using (EntitiesAcademico odata = new EntitiesAcademico())
                {
                    odata.Database.CommandTimeout = 5000;
                    var lst = odata.vwaca_AnioLectivo_Paralelo_Profesor_Calificaciones.Where(q => q.IdEmpresa == IdEmpresa
                    && q.IdAnio == IdAnio && q.IdSede== IdSede).ToList();

                    lst.ForEach(q =>
                    {
                        Lista.Add(new aca_MatriculaCalificacion_Info
                        {
                            IdEmpresa = q.IdEmpresa,
                            IdMatricula = q.IdMatricula,
                            IdMateria = q.IdMateria,
                            IdProfesor = q.IdProfesor,
                            IdAnio = q.IdAnio,
                            IdSede = q.IdSede,
                            IdNivel = q.IdNivel,
                            IdJornada = q.IdJornada,
                            IdCurso = q.IdCurso,
                            IdParalelo = q.IdParalelo,
                            Descripcion = q.Descripcion,
                            NomSede = q.NomSede,
                            NomNivel = q.NomNivel,
                            OrdenNivel = q.OrdenNivel ?? 0,
                            NomJornada = q.NomJornada,
                            OrdenJornada = q.OrdenJornada ?? 0,
                            NomCurso = q.NomCurso,
                            OrdenCurso = q.OrdenCurso ?? 0,
                            NomParalelo = q.NomParalelo,
                            OrdenParalelo = q.OrdenParalelo ?? 0,
                            CodigoParalelo = q.CodigoParalelo,
                            IdProfesorTutor = q.IdProfesorTutor ?? 0,
                            IdProfesorInspector = q.IdProfesorInspector ?? 0,
                            NomMateria = q.NomMateria,
                            OrdenMateria = q.OrdenMateria,
                            EsObligatorio = q.EsObligatorio
                        });
                    });
                }

                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<aca_MatriculaCalificacion_Info> getList_CombosCalificaciones(int IdEmpresa, int IdSede)
        {
            try
            {
                List<aca_MatriculaCalificacion_Info> Lista = new List<aca_MatriculaCalificacion_Info>();

                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();

                    #region Query
                    string query = "SELECT mc.IdEmpresa, mc.IdMatricula, mc.IdMateria, mc.IdProfesor, c.IdAnio, c.IdSede, c.IdNivel, c.IdJornada, c.IdCurso, c.IdParalelo, a.Descripcion, sn.NomSede, sn.NomNivel, sn.OrdenNivel, nj.NomJornada, nj.OrdenJornada, jc.NomCurso, "
                    + " jc.OrdenCurso, cp.CodigoParalelo, cp.NomParalelo, cp.OrdenParalelo, cp.IdProfesorTutor, cp.IdProfesorInspector, cm.NomMateria, cm.EsObligatorio, cm.OrdenMateria "
                    + " FROM     dbo.aca_MatriculaCalificacion AS mc WITH (nolock) INNER JOIN "
                    + " dbo.aca_Matricula AS c WITH (nolock) ON mc.IdEmpresa = c.IdEmpresa AND mc.IdMatricula = c.IdMatricula INNER JOIN "
                    + " dbo.aca_AnioLectivo AS a WITH (nolock) ON c.IdAnio = a.IdAnio AND c.IdEmpresa = a.IdEmpresa INNER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Materia AS cm WITH (nolock) ON c.IdEmpresa = cm.IdEmpresa AND c.IdAnio = cm.IdAnio AND c.IdSede = cm.IdSede AND c.IdNivel = cm.IdNivel AND c.IdJornada = cm.IdJornada AND c.IdCurso = cm.IdCurso AND "
                    + " mc.IdMateria = cm.IdMateria LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_NivelAcademico_Jornada AS nj WITH (nolock) LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Sede_NivelAcademico AS sn WITH (nolock) ON nj.IdEmpresa = sn.IdEmpresa AND nj.IdAnio = sn.IdAnio AND nj.IdSede = sn.IdSede AND nj.IdNivel = sn.IdNivel RIGHT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Jornada_Curso AS jc WITH (nolock) ON nj.IdEmpresa = jc.IdEmpresa AND nj.IdAnio = jc.IdAnio AND nj.IdSede = jc.IdSede AND nj.IdNivel = jc.IdNivel AND nj.IdJornada = jc.IdJornada RIGHT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Paralelo AS cp WITH (nolock) ON jc.IdEmpresa = cp.IdEmpresa AND jc.IdAnio = cp.IdAnio AND jc.IdSede = cp.IdSede AND jc.IdNivel = cp.IdNivel AND jc.IdJornada = cp.IdJornada AND jc.IdCurso = cp.IdCurso ON "
                    + " c.IdEmpresa = cp.IdEmpresa AND c.IdAnio = cp.IdAnio AND c.IdSede = cp.IdSede AND c.IdNivel = cp.IdNivel AND c.IdJornada = cp.IdJornada AND c.IdCurso = cp.IdCurso AND c.IdParalelo = cp.IdParalelo "
                    + " WHERE mc.IdEmpresa = " + IdEmpresa.ToString() + " and c.IdSede = " + IdSede.ToString();
                    query += " GROUP BY mc.IdEmpresa, mc.IdMatricula, mc.IdMateria, mc.IdProfesor, c.IdAnio, c.IdSede, c.IdNivel, c.IdJornada, c.IdCurso, c.IdParalelo, a.Descripcion, sn.NomSede, sn.NomNivel, sn.OrdenNivel, nj.NomJornada, nj.OrdenJornada, jc.NomCurso,  "
                    + " jc.OrdenCurso, cp.CodigoParalelo, cp.NomParalelo, cp.OrdenParalelo, cp.IdProfesorTutor, cp.IdProfesorInspector, cm.NomMateria, cm.OrdenMateria, cm.EsObligatorio ";
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 0;
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Lista.Add(new aca_MatriculaCalificacion_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdMatricula = Convert.ToDecimal(reader["IdMatricula"]),
                            IdMateria = Convert.ToInt32(reader["IdMateria"]),
                            IdProfesor = string.IsNullOrEmpty(reader["IdProfesor"].ToString()) ? (decimal?)null : Convert.ToInt32(reader["IdProfesor"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            IdSede = Convert.ToInt32(reader["IdSede"]),
                            IdNivel = Convert.ToInt32(reader["IdNivel"]),
                            IdJornada = Convert.ToInt32(reader["IdJornada"]),
                            IdCurso = Convert.ToInt32(reader["IdCurso"]),
                            IdParalelo = Convert.ToInt32(reader["IdParalelo"]),
                            Descripcion = string.IsNullOrEmpty(reader["Descripcion"].ToString()) ? null : reader["Descripcion"].ToString(),
                            NomSede = string.IsNullOrEmpty(reader["NomSede"].ToString()) ? null : reader["NomSede"].ToString(),
                            NomNivel = string.IsNullOrEmpty(reader["NomNivel"].ToString()) ? null : reader["NomNivel"].ToString(),
                            OrdenNivel = string.IsNullOrEmpty(reader["OrdenNivel"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenNivel"]),
                            NomJornada = string.IsNullOrEmpty(reader["NomJornada"].ToString()) ? null : reader["NomJornada"].ToString(),
                            OrdenJornada = string.IsNullOrEmpty(reader["OrdenJornada"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenJornada"]),
                            NomCurso = string.IsNullOrEmpty(reader["NomCurso"].ToString()) ? null : reader["NomCurso"].ToString(),
                            OrdenCurso = string.IsNullOrEmpty(reader["OrdenCurso"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenCurso"]),
                            NomParalelo = string.IsNullOrEmpty(reader["NomParalelo"].ToString()) ? null : reader["NomParalelo"].ToString(),
                            OrdenParalelo = string.IsNullOrEmpty(reader["OrdenParalelo"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenParalelo"]),
                            CodigoParalelo = string.IsNullOrEmpty(reader["CodigoParalelo"].ToString()) ? null : reader["CodigoParalelo"].ToString(),
                            IdProfesorTutor = string.IsNullOrEmpty(reader["IdProfesorTutor"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["IdProfesorTutor"]),
                            IdProfesorInspector = string.IsNullOrEmpty(reader["IdProfesorInspector"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["IdProfesorInspector"]),
                            NomMateria = string.IsNullOrEmpty(reader["NomMateria"].ToString()) ? null : reader["NomMateria"].ToString(),
                            OrdenMateria = string.IsNullOrEmpty(reader["OrdenMateria"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenMateria"]),
                            EsObligatorio = string.IsNullOrEmpty(reader["EsObligatorio"].ToString()) ? false : Convert.ToBoolean(reader["EsObligatorio"]),
                        });
                    }
                    reader.Close();
                }

                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<aca_MatriculaCalificacion_Info> getList_Combos(int IdEmpresa, int IdAnio, int IdSede, decimal IdProfesor, bool EsSuperAdmin)
        {
            try
            {
                List<aca_MatriculaCalificacion_Info> Lista = new List<aca_MatriculaCalificacion_Info>();

                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();

                    #region Query
                    string query = "SELECT mc.IdEmpresa, mc.IdMatricula, mc.IdMateria, mc.IdProfesor, c.IdAnio, c.IdSede, c.IdNivel, c.IdJornada, c.IdCurso, c.IdParalelo, a.Descripcion, sn.NomSede, sn.NomNivel, sn.OrdenNivel, nj.NomJornada, nj.OrdenJornada, jc.NomCurso, "
                    + " jc.OrdenCurso, cp.CodigoParalelo, cp.NomParalelo, cp.OrdenParalelo, cp.IdProfesorTutor, cp.IdProfesorInspector, cm.NomMateria, cm.EsObligatorio, cm.OrdenMateria "
                    + " FROM     dbo.aca_MatriculaCalificacion AS mc WITH (nolock) INNER JOIN "
                    + " dbo.aca_Matricula AS c WITH (nolock) ON mc.IdEmpresa = c.IdEmpresa AND mc.IdMatricula = c.IdMatricula INNER JOIN "
                    + " dbo.aca_AnioLectivo AS a WITH (nolock) ON c.IdAnio = a.IdAnio AND c.IdEmpresa = a.IdEmpresa INNER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Materia AS cm WITH (nolock) ON c.IdEmpresa = cm.IdEmpresa AND c.IdAnio = cm.IdAnio AND c.IdSede = cm.IdSede AND c.IdNivel = cm.IdNivel AND c.IdJornada = cm.IdJornada AND c.IdCurso = cm.IdCurso AND "
                    + " mc.IdMateria = cm.IdMateria LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_NivelAcademico_Jornada AS nj WITH (nolock) LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Sede_NivelAcademico AS sn WITH (nolock) ON nj.IdEmpresa = sn.IdEmpresa AND nj.IdAnio = sn.IdAnio AND nj.IdSede = sn.IdSede AND nj.IdNivel = sn.IdNivel RIGHT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Jornada_Curso AS jc WITH (nolock) ON nj.IdEmpresa = jc.IdEmpresa AND nj.IdAnio = jc.IdAnio AND nj.IdSede = jc.IdSede AND nj.IdNivel = jc.IdNivel AND nj.IdJornada = jc.IdJornada RIGHT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Paralelo AS cp WITH (nolock) ON jc.IdEmpresa = cp.IdEmpresa AND jc.IdAnio = cp.IdAnio AND jc.IdSede = cp.IdSede AND jc.IdNivel = cp.IdNivel AND jc.IdJornada = cp.IdJornada AND jc.IdCurso = cp.IdCurso ON "
                    + " c.IdEmpresa = cp.IdEmpresa AND c.IdAnio = cp.IdAnio AND c.IdSede = cp.IdSede AND c.IdNivel = cp.IdNivel AND c.IdJornada = cp.IdJornada AND c.IdCurso = cp.IdCurso AND c.IdParalelo = cp.IdParalelo "
                    + " WHERE mc.IdEmpresa = " + IdEmpresa.ToString() + " and c.IdAnio = " + IdAnio.ToString() + " and c.IdSede = " + IdSede.ToString();
                    if (EsSuperAdmin == true)
                    {
                        query += " and mc.IdProfesor = mc.IdProfesor";
                    }
                    else
                    {
                        query += " and mc.IdProfesor = " + IdProfesor.ToString();
                    }
                    query += " GROUP BY mc.IdEmpresa, mc.IdMatricula, mc.IdMateria, mc.IdProfesor, c.IdAnio, c.IdSede, c.IdNivel, c.IdJornada, c.IdCurso, c.IdParalelo, a.Descripcion, sn.NomSede, sn.NomNivel, sn.OrdenNivel, nj.NomJornada, nj.OrdenJornada, jc.NomCurso,  "
                    + " jc.OrdenCurso, cp.CodigoParalelo, cp.NomParalelo, cp.OrdenParalelo, cp.IdProfesorTutor, cp.IdProfesorInspector, cm.NomMateria, cm.OrdenMateria, cm.EsObligatorio ";
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 0;
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Lista.Add(new aca_MatriculaCalificacion_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdMatricula = Convert.ToDecimal(reader["IdMatricula"]),
                            IdMateria = Convert.ToInt32(reader["IdMateria"]),
                            IdProfesor = string.IsNullOrEmpty(reader["IdProfesor"].ToString()) ? (decimal?)null : Convert.ToInt32(reader["IdProfesor"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            IdSede = Convert.ToInt32(reader["IdSede"]),
                            IdNivel = Convert.ToInt32(reader["IdNivel"]),
                            IdJornada = Convert.ToInt32(reader["IdJornada"]),
                            IdCurso = Convert.ToInt32(reader["IdCurso"]),
                            IdParalelo = Convert.ToInt32(reader["IdParalelo"]),
                            Descripcion = string.IsNullOrEmpty(reader["Descripcion"].ToString()) ? null : reader["Descripcion"].ToString(),
                            NomSede = string.IsNullOrEmpty(reader["NomSede"].ToString()) ? null : reader["NomSede"].ToString(),
                            NomNivel = string.IsNullOrEmpty(reader["NomNivel"].ToString()) ? null : reader["NomNivel"].ToString(),
                            OrdenNivel = string.IsNullOrEmpty(reader["OrdenNivel"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenNivel"]),
                            NomJornada = string.IsNullOrEmpty(reader["NomJornada"].ToString()) ? null : reader["NomJornada"].ToString(),
                            OrdenJornada = string.IsNullOrEmpty(reader["OrdenJornada"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenJornada"]),
                            NomCurso = string.IsNullOrEmpty(reader["NomCurso"].ToString()) ? null : reader["NomCurso"].ToString(),
                            OrdenCurso = string.IsNullOrEmpty(reader["OrdenCurso"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenCurso"]),
                            NomParalelo = string.IsNullOrEmpty(reader["NomParalelo"].ToString()) ? null : reader["NomParalelo"].ToString(),
                            OrdenParalelo = string.IsNullOrEmpty(reader["OrdenParalelo"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenParalelo"]),
                            CodigoParalelo = string.IsNullOrEmpty(reader["CodigoParalelo"].ToString()) ? null : reader["CodigoParalelo"].ToString(),
                            IdProfesorTutor = string.IsNullOrEmpty(reader["IdProfesorTutor"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["IdProfesorTutor"]),
                            IdProfesorInspector = string.IsNullOrEmpty(reader["IdProfesorInspector"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["IdProfesorInspector"]),
                            NomMateria = string.IsNullOrEmpty(reader["NomMateria"].ToString()) ? null : reader["NomMateria"].ToString(),
                            OrdenMateria = string.IsNullOrEmpty(reader["OrdenMateria"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenMateria"]),
                            EsObligatorio = string.IsNullOrEmpty(reader["EsObligatorio"].ToString()) ? false : Convert.ToBoolean(reader["EsObligatorio"]),
                        });
                    }
                    reader.Close();
                }
                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<aca_MatriculaCalificacion_Info> getList_CombosCalificaciones(int IdEmpresa, int IdSede, decimal IdProfesor, bool EsSuperAdmin)
        {
            try
            {
                List<aca_MatriculaCalificacion_Info> Lista = new List<aca_MatriculaCalificacion_Info>();

                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();

                    #region Query
                    string query = "SELECT mc.IdEmpresa, mc.IdMatricula, mc.IdMateria, mc.IdProfesor, c.IdAnio, c.IdSede, c.IdNivel, c.IdJornada, c.IdCurso, c.IdParalelo, a.Descripcion, sn.NomSede, sn.NomNivel, sn.OrdenNivel, nj.NomJornada, nj.OrdenJornada, jc.NomCurso, "
                    + " jc.OrdenCurso, cp.CodigoParalelo, cp.NomParalelo, cp.OrdenParalelo, cp.IdProfesorTutor, cp.IdProfesorInspector, cm.NomMateria, cm.EsObligatorio, cm.OrdenMateria "
                    + " FROM     dbo.aca_MatriculaCalificacion AS mc WITH (nolock) INNER JOIN "
                    + " dbo.aca_Matricula AS c WITH (nolock) ON mc.IdEmpresa = c.IdEmpresa AND mc.IdMatricula = c.IdMatricula INNER JOIN "
                    + " dbo.aca_AnioLectivo AS a WITH (nolock) ON c.IdAnio = a.IdAnio AND c.IdEmpresa = a.IdEmpresa INNER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Materia AS cm WITH (nolock) ON c.IdEmpresa = cm.IdEmpresa AND c.IdAnio = cm.IdAnio AND c.IdSede = cm.IdSede AND c.IdNivel = cm.IdNivel AND c.IdJornada = cm.IdJornada AND c.IdCurso = cm.IdCurso AND "
                    + " mc.IdMateria = cm.IdMateria LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_NivelAcademico_Jornada AS nj WITH (nolock) LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Sede_NivelAcademico AS sn WITH (nolock) ON nj.IdEmpresa = sn.IdEmpresa AND nj.IdAnio = sn.IdAnio AND nj.IdSede = sn.IdSede AND nj.IdNivel = sn.IdNivel RIGHT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Jornada_Curso AS jc WITH (nolock) ON nj.IdEmpresa = jc.IdEmpresa AND nj.IdAnio = jc.IdAnio AND nj.IdSede = jc.IdSede AND nj.IdNivel = jc.IdNivel AND nj.IdJornada = jc.IdJornada RIGHT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Paralelo AS cp WITH (nolock) ON jc.IdEmpresa = cp.IdEmpresa AND jc.IdAnio = cp.IdAnio AND jc.IdSede = cp.IdSede AND jc.IdNivel = cp.IdNivel AND jc.IdJornada = cp.IdJornada AND jc.IdCurso = cp.IdCurso ON "
                    + " c.IdEmpresa = cp.IdEmpresa AND c.IdAnio = cp.IdAnio AND c.IdSede = cp.IdSede AND c.IdNivel = cp.IdNivel AND c.IdJornada = cp.IdJornada AND c.IdCurso = cp.IdCurso AND c.IdParalelo = cp.IdParalelo "
                    + " WHERE mc.IdEmpresa = " + IdEmpresa.ToString() + " and c.IdSede = " + IdSede.ToString();
                    if (EsSuperAdmin == true)
                    {
                        query += " and mc.IdProfesor = mc.IdProfesor";
                    }
                    else
                    {
                        query += " and mc.IdProfesor = " + IdProfesor.ToString();
                    }
                    query += " GROUP BY mc.IdEmpresa, mc.IdMatricula, mc.IdMateria, mc.IdProfesor, c.IdAnio, c.IdSede, c.IdNivel, c.IdJornada, c.IdCurso, c.IdParalelo, a.Descripcion, sn.NomSede, sn.NomNivel, sn.OrdenNivel, nj.NomJornada, nj.OrdenJornada, jc.NomCurso,  "
                    + " jc.OrdenCurso, cp.CodigoParalelo, cp.NomParalelo, cp.OrdenParalelo, cp.IdProfesorTutor, cp.IdProfesorInspector, cm.NomMateria, cm.OrdenMateria, cm.EsObligatorio ";
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 0;
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Lista.Add(new aca_MatriculaCalificacion_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdMatricula = Convert.ToDecimal(reader["IdMatricula"]),
                            IdMateria = Convert.ToInt32(reader["IdMateria"]),
                            IdProfesor = string.IsNullOrEmpty(reader["IdProfesor"].ToString()) ? (decimal?)null : Convert.ToInt32(reader["IdProfesor"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            IdSede = Convert.ToInt32(reader["IdSede"]),
                            IdNivel = Convert.ToInt32(reader["IdNivel"]),
                            IdJornada = Convert.ToInt32(reader["IdJornada"]),
                            IdCurso = Convert.ToInt32(reader["IdCurso"]),
                            IdParalelo = Convert.ToInt32(reader["IdParalelo"]),
                            Descripcion = string.IsNullOrEmpty(reader["Descripcion"].ToString()) ? null : reader["Descripcion"].ToString(),
                            NomSede = string.IsNullOrEmpty(reader["NomSede"].ToString()) ? null : reader["NomSede"].ToString(),
                            NomNivel = string.IsNullOrEmpty(reader["NomNivel"].ToString()) ? null : reader["NomNivel"].ToString(),
                            OrdenNivel = string.IsNullOrEmpty(reader["OrdenNivel"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenNivel"]),
                            NomJornada = string.IsNullOrEmpty(reader["NomJornada"].ToString()) ? null : reader["NomJornada"].ToString(),
                            OrdenJornada = string.IsNullOrEmpty(reader["OrdenJornada"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenJornada"]),
                            NomCurso = string.IsNullOrEmpty(reader["NomCurso"].ToString()) ? null : reader["NomCurso"].ToString(),
                            OrdenCurso = string.IsNullOrEmpty(reader["OrdenCurso"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenCurso"]),
                            NomParalelo = string.IsNullOrEmpty(reader["NomParalelo"].ToString()) ? null : reader["NomParalelo"].ToString(),
                            OrdenParalelo = string.IsNullOrEmpty(reader["OrdenParalelo"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenParalelo"]),
                            CodigoParalelo = string.IsNullOrEmpty(reader["CodigoParalelo"].ToString()) ? null : reader["CodigoParalelo"].ToString(),
                            IdProfesorTutor = string.IsNullOrEmpty(reader["IdProfesorTutor"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["IdProfesorTutor"]),
                            IdProfesorInspector = string.IsNullOrEmpty(reader["IdProfesorInspector"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["IdProfesorInspector"]),
                            NomMateria = string.IsNullOrEmpty(reader["NomMateria"].ToString()) ? null : reader["NomMateria"].ToString(),
                            OrdenMateria = string.IsNullOrEmpty(reader["OrdenMateria"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenMateria"]),
                            EsObligatorio = string.IsNullOrEmpty(reader["EsObligatorio"].ToString()) ? false : Convert.ToBoolean(reader["EsObligatorio"]),
                        });
                    }
                    reader.Close();
                }
                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<aca_MatriculaCalificacion_Info> getList_Combos_Tutor(int IdEmpresa, int IdAnio, int IdSede, decimal IdProfesor, bool EsSuperAdmin)
        {
            try
            {
                List<aca_MatriculaCalificacion_Info> Lista = new List<aca_MatriculaCalificacion_Info>();

                using (EntitiesAcademico odata = new EntitiesAcademico())
                {
                    var lst = odata.vwaca_AnioLectivo_Paralelo_Profesor_Calificaciones.Where(q => q.IdEmpresa == IdEmpresa
                    && q.IdAnio == IdAnio && q.IdSede == IdSede && q.IdProfesorTutor == (EsSuperAdmin == true ? q.IdProfesorTutor : IdProfesor)).ToList();

                    lst.ForEach(q =>
                    {
                        Lista.Add(new aca_MatriculaCalificacion_Info
                        {
                            IdEmpresa = q.IdEmpresa,
                            IdMatricula = q.IdMatricula,
                            IdMateria = q.IdMateria,
                            IdProfesor = q.IdProfesor,
                            IdAnio = q.IdAnio,
                            IdSede = q.IdSede,
                            IdNivel = q.IdNivel,
                            IdJornada = q.IdJornada,
                            IdCurso = q.IdCurso,
                            IdParalelo = q.IdParalelo,
                            Descripcion = q.Descripcion,
                            NomSede = q.NomSede,
                            NomNivel = q.NomNivel,
                            OrdenNivel = q.OrdenNivel ?? 0,
                            NomJornada = q.NomJornada,
                            OrdenJornada = q.OrdenJornada ?? 0,
                            NomCurso = q.NomCurso,
                            OrdenCurso = q.OrdenCurso ?? 0,
                            NomParalelo = q.NomParalelo,
                            OrdenParalelo = q.OrdenParalelo ?? 0,
                            CodigoParalelo = q.CodigoParalelo,
                            IdProfesorTutor = q.IdProfesorTutor ?? 0,
                            IdProfesorInspector = q.IdProfesorInspector ?? 0,
                            NomMateria = q.NomMateria,
                            OrdenMateria = q.OrdenMateria,
                            EsObligatorio = q.EsObligatorio
                        });
                    });
                }

                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<aca_MatriculaCalificacion_Info> getList_Combos_TutorCalificacion(int IdEmpresa, int IdAnio, int IdSede, decimal IdProfesor, bool EsSuperAdmin)
        {
            try
            {
                List<aca_MatriculaCalificacion_Info> Lista = new List<aca_MatriculaCalificacion_Info>();

                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();

                    #region Query
                    string query = "SELECT c.IdEmpresa, c.IdAnio, c.IdSede, c.IdNivel, c.IdJornada, c.IdCurso, c.IdParalelo, a.Descripcion, sn.NomSede, sn.NomNivel, sn.OrdenNivel, nj.NomJornada, nj.OrdenJornada,"
                    + " jc.NomCurso,  jc.OrdenCurso, cp.CodigoParalelo, cp.NomParalelo, cp.OrdenParalelo, cp.IdProfesorTutor, cp.IdProfesorInspector"
                    + " FROM     dbo.aca_Matricula AS c WITH (nolock)"
                    + " LEFT OUTER JOIN  dbo.aca_AnioLectivo_Sede_NivelAcademico AS sn WITH(nolock) ON sn.IdEmpresa = c.IdEmpresa AND sn.IdAnio = c.IdAnio"
                    + " AND sn.IdSede = c.IdSede AND sn.IdNivel = c.IdNivel"
                    + " LEFT OUTER JOIN  dbo.aca_AnioLectivo_NivelAcademico_Jornada AS nj WITH(nolock)"
                    + " ON nj.IdEmpresa = c.IdEmpresa AND nj.IdAnio = c.IdAnio AND nj.IdSede = c.IdSede AND nj.IdNivel = c.IdNivel  and nj.IdJornada = c.IdJornada"
                    + " LEFT OUTER JOIN  dbo.aca_AnioLectivo_Jornada_Curso AS jc WITH(nolock)"
                    + " ON jc.IdEmpresa = c.IdEmpresa AND jc.IdAnio = c.IdAnio AND jc.IdSede = c.IdSede AND jc.IdNivel = c.IdNivel AND jc.IdJornada = c.IdJornada"
                    + " and jc.IdCurso = c.IdCurso"
                    + " LEFT OUTER JOIN  dbo.aca_AnioLectivo_Curso_Paralelo AS cp WITH(nolock)"
                    + " ON  cp.IdEmpresa = c.IdEmpresa AND cp.IdAnio = c.IdAnio AND cp.IdSede = c.IdSede AND cp.IdNivel = c.IdNivel AND cp.IdJornada = c.IdJornada"
                    + " AND cp.IdCurso = c.IdCurso AND cp.IdParalelo = c.IdParalelo"
                    + " LEFT OUTER JOIN  dbo.aca_AnioLectivo AS a WITH(nolock) ON c.IdAnio = a.IdAnio AND c.IdEmpresa = a.IdEmpresa"
                    + " LEFT OUTER JOIN  dbo.aca_MatriculaCalificacion AS mc WITH(nolock) ON mc.IdEmpresa = c.IdEmpresa AND mc.IdMatricula = c.IdMatricula"
                    + " WHERE mc.IdEmpresa = " + IdEmpresa.ToString() + " and c.IdSede = " + IdSede.ToString() + " and c.IdAnio = " + IdAnio.ToString();
                    
                    if (EsSuperAdmin == true)
                    {
                        
                    }
                    else
                    {
                        query += " and cp.IdProfesorTutor = " + IdProfesor.ToString();
                    }
                    query += " GROUP BY c.IdEmpresa, c.IdAnio, c.IdSede, c.IdNivel, c.IdJornada, c.IdCurso, c.IdParalelo, "
                    + " a.Descripcion, sn.NomSede, sn.NomNivel, sn.OrdenNivel, nj.NomJornada, nj.OrdenJornada, jc.NomCurso, jc.OrdenCurso, cp.CodigoParalelo, "
                    + " cp.NomParalelo, cp.OrdenParalelo, cp.IdProfesorTutor, cp.IdProfesorInspector";
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 0;
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Lista.Add(new aca_MatriculaCalificacion_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            IdSede = Convert.ToInt32(reader["IdSede"]),
                            IdNivel = Convert.ToInt32(reader["IdNivel"]),
                            IdJornada = Convert.ToInt32(reader["IdJornada"]),
                            IdCurso = Convert.ToInt32(reader["IdCurso"]),
                            IdParalelo = Convert.ToInt32(reader["IdParalelo"]),
                            Descripcion = string.IsNullOrEmpty(reader["Descripcion"].ToString()) ? null : reader["Descripcion"].ToString(),
                            NomSede = string.IsNullOrEmpty(reader["NomSede"].ToString()) ? null : reader["NomSede"].ToString(),
                            NomNivel = string.IsNullOrEmpty(reader["NomNivel"].ToString()) ? null : reader["NomNivel"].ToString(),
                            OrdenNivel = string.IsNullOrEmpty(reader["OrdenNivel"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenNivel"]),
                            NomJornada = string.IsNullOrEmpty(reader["NomJornada"].ToString()) ? null : reader["NomJornada"].ToString(),
                            OrdenJornada = string.IsNullOrEmpty(reader["OrdenJornada"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenJornada"]),
                            NomCurso = string.IsNullOrEmpty(reader["NomCurso"].ToString()) ? null : reader["NomCurso"].ToString(),
                            OrdenCurso = string.IsNullOrEmpty(reader["OrdenCurso"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenCurso"]),
                            NomParalelo = string.IsNullOrEmpty(reader["NomParalelo"].ToString()) ? null : reader["NomParalelo"].ToString(),
                            OrdenParalelo = string.IsNullOrEmpty(reader["OrdenParalelo"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenParalelo"]),
                            CodigoParalelo = string.IsNullOrEmpty(reader["CodigoParalelo"].ToString()) ? null : reader["CodigoParalelo"].ToString(),
                            IdProfesorTutor = string.IsNullOrEmpty(reader["IdProfesorTutor"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["IdProfesorTutor"]),
                            IdProfesorInspector = string.IsNullOrEmpty(reader["IdProfesorInspector"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["IdProfesorInspector"]),                            
                        });
                    }
                    reader.Close();
                }                

                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<aca_MatriculaCalificacion_Info> getList_Combos_Inspector(int IdEmpresa, int IdAnio, int IdSede, decimal IdProfesor, bool EsSuperAdmin)
        {
            try
            {
                List<aca_MatriculaCalificacion_Info> Lista = new List<aca_MatriculaCalificacion_Info>();

                using (EntitiesAcademico odata = new EntitiesAcademico())
                {
                    var lst = odata.vwaca_AnioLectivo_Paralelo_Profesor_Calificaciones.Where(q => q.IdEmpresa == IdEmpresa
                    && q.IdAnio == IdAnio && q.IdSede == IdSede && q.IdProfesorInspector == (EsSuperAdmin == true ? q.IdProfesorInspector : IdProfesor)).ToList();

                    lst.ForEach(q =>
                    {
                        Lista.Add(new aca_MatriculaCalificacion_Info
                        {
                            IdEmpresa = q.IdEmpresa,
                            IdMatricula = q.IdMatricula,
                            IdMateria = q.IdMateria,
                            IdProfesor = q.IdProfesor,
                            IdAnio = q.IdAnio,
                            IdSede = q.IdSede,
                            IdNivel = q.IdNivel,
                            IdJornada = q.IdJornada,
                            IdCurso = q.IdCurso,
                            IdParalelo = q.IdParalelo,
                            Descripcion = q.Descripcion,
                            NomSede = q.NomSede,
                            NomNivel = q.NomNivel,
                            OrdenNivel = q.OrdenNivel ?? 0,
                            NomJornada = q.NomJornada,
                            OrdenJornada = q.OrdenJornada ?? 0,
                            NomCurso = q.NomCurso,
                            OrdenCurso = q.OrdenCurso ?? 0,
                            NomParalelo = q.NomParalelo,
                            OrdenParalelo = q.OrdenParalelo ?? 0,
                            CodigoParalelo = q.CodigoParalelo,
                            IdProfesorTutor = q.IdProfesorTutor ?? 0,
                            IdProfesorInspector = q.IdProfesorInspector ?? 0,
                            NomMateria = q.NomMateria,
                            OrdenMateria = q.OrdenMateria,
                            EsObligatorio = q.EsObligatorio
                        });
                    });
                }

                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<aca_MatriculaCalificacion_Info> getList_Combos_InspectorCalificacion(int IdEmpresa, int IdSede, decimal IdProfesor, bool EsSuperAdmin)
        {
            try
            {
                List<aca_MatriculaCalificacion_Info> Lista = new List<aca_MatriculaCalificacion_Info>();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();

                    #region Query
                    string query = "SELECT mc.IdEmpresa, mc.IdMatricula, mc.IdMateria, mc.IdProfesor, c.IdAnio, c.IdSede, c.IdNivel, c.IdJornada, c.IdCurso, c.IdParalelo, a.Descripcion, sn.NomSede, sn.NomNivel, sn.OrdenNivel, nj.NomJornada, nj.OrdenJornada, jc.NomCurso, "
                    + " jc.OrdenCurso, cp.CodigoParalelo, cp.NomParalelo, cp.OrdenParalelo, cp.IdProfesorTutor, cp.IdProfesorInspector, cm.NomMateria, cm.EsObligatorio, cm.OrdenMateria "
                    + " FROM     dbo.aca_MatriculaCalificacion AS mc WITH (nolock) INNER JOIN "
                    + " dbo.aca_Matricula AS c WITH (nolock) ON mc.IdEmpresa = c.IdEmpresa AND mc.IdMatricula = c.IdMatricula INNER JOIN "
                    + " dbo.aca_AnioLectivo AS a WITH (nolock) ON c.IdAnio = a.IdAnio AND c.IdEmpresa = a.IdEmpresa INNER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Materia AS cm WITH (nolock) ON c.IdEmpresa = cm.IdEmpresa AND c.IdAnio = cm.IdAnio AND c.IdSede = cm.IdSede AND c.IdNivel = cm.IdNivel AND c.IdJornada = cm.IdJornada AND c.IdCurso = cm.IdCurso AND "
                    + " mc.IdMateria = cm.IdMateria LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_NivelAcademico_Jornada AS nj WITH (nolock) LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Sede_NivelAcademico AS sn WITH (nolock) ON nj.IdEmpresa = sn.IdEmpresa AND nj.IdAnio = sn.IdAnio AND nj.IdSede = sn.IdSede AND nj.IdNivel = sn.IdNivel RIGHT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Jornada_Curso AS jc WITH (nolock) ON nj.IdEmpresa = jc.IdEmpresa AND nj.IdAnio = jc.IdAnio AND nj.IdSede = jc.IdSede AND nj.IdNivel = jc.IdNivel AND nj.IdJornada = jc.IdJornada RIGHT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Paralelo AS cp WITH (nolock) ON jc.IdEmpresa = cp.IdEmpresa AND jc.IdAnio = cp.IdAnio AND jc.IdSede = cp.IdSede AND jc.IdNivel = cp.IdNivel AND jc.IdJornada = cp.IdJornada AND jc.IdCurso = cp.IdCurso ON "
                    + " c.IdEmpresa = cp.IdEmpresa AND c.IdAnio = cp.IdAnio AND c.IdSede = cp.IdSede AND c.IdNivel = cp.IdNivel AND c.IdJornada = cp.IdJornada AND c.IdCurso = cp.IdCurso AND c.IdParalelo = cp.IdParalelo "
                    + " WHERE mc.IdEmpresa = " + IdEmpresa.ToString() + " and c.IdSede = " + IdSede.ToString();
                    if (EsSuperAdmin == true)
                    {
                        query += " and mc.IdProfesor = mc.IdProfesor";
                    }
                    else
                    {
                        query += " and mc.IdProfesor = " + IdProfesor.ToString();
                    }
                    query += " GROUP BY mc.IdEmpresa, mc.IdMatricula, mc.IdMateria, mc.IdProfesor, c.IdAnio, c.IdSede, c.IdNivel, c.IdJornada, c.IdCurso, c.IdParalelo, a.Descripcion, sn.NomSede, sn.NomNivel, sn.OrdenNivel, nj.NomJornada, nj.OrdenJornada, jc.NomCurso,  "
                    + " jc.OrdenCurso, cp.CodigoParalelo, cp.NomParalelo, cp.OrdenParalelo, cp.IdProfesorTutor, cp.IdProfesorInspector, cm.NomMateria, cm.OrdenMateria, cm.EsObligatorio ";
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 0;
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Lista.Add(new aca_MatriculaCalificacion_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdMatricula = Convert.ToDecimal(reader["IdMatricula"]),
                            IdMateria = Convert.ToInt32(reader["IdMateria"]),
                            IdProfesor = string.IsNullOrEmpty(reader["IdProfesor"].ToString()) ? (decimal?)null : Convert.ToInt32(reader["IdProfesor"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            IdSede = Convert.ToInt32(reader["IdSede"]),
                            IdNivel = Convert.ToInt32(reader["IdNivel"]),
                            IdJornada = Convert.ToInt32(reader["IdJornada"]),
                            IdCurso = Convert.ToInt32(reader["IdCurso"]),
                            IdParalelo = Convert.ToInt32(reader["IdParalelo"]),
                            Descripcion = string.IsNullOrEmpty(reader["Descripcion"].ToString()) ? null : reader["Descripcion"].ToString(),
                            NomSede = string.IsNullOrEmpty(reader["NomSede"].ToString()) ? null : reader["NomSede"].ToString(),
                            NomNivel = string.IsNullOrEmpty(reader["NomNivel"].ToString()) ? null : reader["NomNivel"].ToString(),
                            OrdenNivel = string.IsNullOrEmpty(reader["OrdenNivel"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenNivel"]),
                            NomJornada = string.IsNullOrEmpty(reader["NomJornada"].ToString()) ? null : reader["NomJornada"].ToString(),
                            OrdenJornada = string.IsNullOrEmpty(reader["OrdenJornada"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenJornada"]),
                            NomCurso = string.IsNullOrEmpty(reader["NomCurso"].ToString()) ? null : reader["NomCurso"].ToString(),
                            OrdenCurso = string.IsNullOrEmpty(reader["OrdenCurso"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenCurso"]),
                            NomParalelo = string.IsNullOrEmpty(reader["NomParalelo"].ToString()) ? null : reader["NomParalelo"].ToString(),
                            OrdenParalelo = string.IsNullOrEmpty(reader["OrdenParalelo"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenParalelo"]),
                            CodigoParalelo = string.IsNullOrEmpty(reader["CodigoParalelo"].ToString()) ? null : reader["CodigoParalelo"].ToString(),
                            IdProfesorTutor = string.IsNullOrEmpty(reader["IdProfesorTutor"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["IdProfesorTutor"]),
                            IdProfesorInspector = string.IsNullOrEmpty(reader["IdProfesorInspector"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["IdProfesorInspector"]),
                            NomMateria = string.IsNullOrEmpty(reader["NomMateria"].ToString()) ? null : reader["NomMateria"].ToString(),
                            OrdenMateria = string.IsNullOrEmpty(reader["OrdenMateria"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenMateria"]),
                            EsObligatorio = string.IsNullOrEmpty(reader["EsObligatorio"].ToString()) ? false : Convert.ToBoolean(reader["EsObligatorio"]),
                        });
                    }
                    reader.Close();
                }
                
                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool modificarDB(aca_MatriculaCalificacion_Info info)
        {
            try
            {
                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    var info_matricula = odata_matricula.getInfo(info.IdEmpresa, info.IdMatricula);
                    aca_MatriculaCalificacion EntityCalificacion = Context.aca_MatriculaCalificacion.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa
                    && q.IdMatricula == info.IdMatricula && q.IdProfesor == info.IdProfesor && q.IdMateria == info.IdMateria);
                    if (EntityCalificacion == null)
                        return false;

                    if (info.IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademicoExamen.EXQUI1))
                    {
                        EntityCalificacion.ExamenQ1 = info.CalificacionExamen;
                        EntityCalificacion.PromedioFinalQ1 = info.Promedio;
                        EntityCalificacion.IdEquivalenciaPromedioQ1 = info.IdEquivalenciaPromedio;
                        EntityCalificacion.CausaQ1 = info.Causa;
                        EntityCalificacion.ResolucionQ1 = info.Resolucion;
                        EntityCalificacion.IdEquivalenciaPromedioEQ1 = info.IdEquivalenciaCalificacionExamen;

                        var PromedioQuimestral = (decimal?)null;
                        if (EntityCalificacion.PromedioFinalQ1 != null && EntityCalificacion.PromedioFinalQ2 != null)
                        {
                            PromedioQuimestral = Math.Round(((Convert.ToDecimal(EntityCalificacion.PromedioFinalQ1 + EntityCalificacion.PromedioFinalQ2)) / 2), 2, MidpointRounding.AwayFromZero);
                        }

                        EntityCalificacion.PromedioQuimestres = PromedioQuimestral;
                    }
                        
                    if (info.IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademicoExamen.EXQUI2))
                    {
                        EntityCalificacion.ExamenQ2 = info.CalificacionExamen;
                        EntityCalificacion.PromedioFinalQ2 = info.Promedio;
                        EntityCalificacion.IdEquivalenciaPromedioQ2 = info.IdEquivalenciaPromedio;
                        EntityCalificacion.CausaQ2 = info.Causa;
                        EntityCalificacion.ResolucionQ2 = info.Resolucion;
                        EntityCalificacion.IdEquivalenciaPromedioEQ2 = info.IdEquivalenciaCalificacionExamen;

                        var PromedioQuimestral = (decimal?)null;
                        if (EntityCalificacion.PromedioFinalQ1!= null && EntityCalificacion.PromedioFinalQ2!=null)
                        {
                            PromedioQuimestral = Math.Round(((Convert.ToDecimal(EntityCalificacion.PromedioFinalQ1 + EntityCalificacion.PromedioFinalQ2)) / 2), 2, MidpointRounding.AwayFromZero);
                        }

                        EntityCalificacion.PromedioQuimestres = PromedioQuimestral; 
                    }

                    if (info.IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademicoExamen.EXMEJ))
                    {
                        EntityCalificacion.ExamenMejoramiento = info.CalificacionExamen;
                        EntityCalificacion.PromedioFinal = info.Promedio;
                        EntityCalificacion.IdEquivalenciaPromedioPF = info.IdEquivalenciaPromedio;
                    }
                        

                    if (info.IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademicoExamen.EXSUP))
                    {
                        EntityCalificacion.ExamenSupletorio = info.CalificacionExamen;
                        EntityCalificacion.PromedioFinal = info.Promedio;
                        EntityCalificacion.IdEquivalenciaPromedioPF = info.IdEquivalenciaPromedio;
                    }

                    if (info.IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademicoExamen.EXREM))
                    {
                        EntityCalificacion.ExamenRemedial = info.CalificacionExamen;
                        EntityCalificacion.PromedioFinal = info.Promedio;
                        EntityCalificacion.IdEquivalenciaPromedioPF = info.IdEquivalenciaPromedio;
                    }

                    if (info.IdCatalogoParcial == Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademicoExamen.EXGRA))
                    {
                        EntityCalificacion.ExamenGracia = info.CalificacionExamen;
                        EntityCalificacion.PromedioFinal = info.Promedio;
                        EntityCalificacion.IdEquivalenciaPromedioPF = info.IdEquivalenciaPromedio;
                    }

                    Context.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool modicarPaseAnioDB(aca_MatriculaCalificacion_Info info)
        {
            try
            {
                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    aca_MatriculaCalificacion EntityCalificacion = Context.aca_MatriculaCalificacion.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa
                    && q.IdMatricula == info.IdMatricula && q.IdProfesor == info.IdProfesor && q.IdMateria == info.IdMateria);
                    if (EntityCalificacion == null)
                        return false;

                    EntityCalificacion.PromedioQuimestres = info.PromedioQuimestres;
                    EntityCalificacion.CampoMejoramiento = info.CampoMejoramiento;
                    EntityCalificacion.PromedioFinal = info.PromedioFinal;
                    EntityCalificacion.IdEquivalenciaPromedioPF = info.IdEquivalenciaPromedioPF;

                    Context.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<aca_MatriculaCalificacion_Info> getList_CombosParticipacion(int IdEmpresa, int IdAnio, int IdSede)
        {
            try
            {
                List<aca_MatriculaCalificacion_Info> Lista = new List<aca_MatriculaCalificacion_Info>();

                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();

                    #region Query
                    string query = "SELECT mc.IdEmpresa, mc.IdMatricula, mc.IdProfesor, c.IdAnio, c.IdSede, c.IdNivel, c.IdJornada, c.IdCurso, c.IdParalelo, a.Descripcion, sn.NomSede, sn.NomNivel, sn.OrdenNivel, nj.NomJornada, nj.OrdenJornada, jc.NomCurso, "
                    + " jc.OrdenCurso, cp.CodigoParalelo, cp.NomParalelo, cp.OrdenParalelo "
                    + " FROM dbo.aca_MatriculaCalificacionParticipacion AS mc WITH (nolock) INNER JOIN "
                    + " dbo.aca_Matricula AS c WITH (nolock) ON mc.IdEmpresa = c.IdEmpresa AND mc.IdMatricula = c.IdMatricula INNER JOIN "
                    + " dbo.aca_AnioLectivo_Tematica t WITH (nolock) on t.IdEmpresa = mc.IdEmpresa and t.IdAnio = c.IdAnio and mc.IdTematica = t.IdTematica LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo AS a WITH (nolock) ON c.IdAnio = a.IdAnio AND c.IdEmpresa = a.IdEmpresa LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_NivelAcademico_Jornada AS nj WITH (nolock) LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Sede_NivelAcademico AS sn WITH (nolock) ON nj.IdEmpresa = sn.IdEmpresa AND nj.IdAnio = sn.IdAnio AND nj.IdSede = sn.IdSede AND nj.IdNivel = sn.IdNivel RIGHT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Jornada_Curso AS jc WITH (nolock) ON nj.IdEmpresa = jc.IdEmpresa AND nj.IdAnio = jc.IdAnio AND nj.IdSede = jc.IdSede AND nj.IdNivel = jc.IdNivel AND nj.IdJornada = jc.IdJornada RIGHT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Paralelo AS cp WITH (nolock) ON jc.IdEmpresa = cp.IdEmpresa AND jc.IdAnio = cp.IdAnio AND jc.IdSede = cp.IdSede AND jc.IdNivel = cp.IdNivel AND jc.IdJornada = cp.IdJornada AND jc.IdCurso = cp.IdCurso ON "
                    + " c.IdEmpresa = cp.IdEmpresa AND c.IdAnio = cp.IdAnio AND c.IdSede = cp.IdSede AND c.IdNivel = cp.IdNivel AND c.IdJornada = cp.IdJornada AND c.IdCurso = cp.IdCurso AND c.IdParalelo = cp.IdParalelo "
                    + " GROUP BY mc.IdEmpresa, mc.IdMatricula, mc.IdProfesor, c.IdAnio, c.IdSede, c.IdNivel, c.IdJornada, c.IdCurso, c.IdParalelo, a.Descripcion, sn.NomSede, sn.NomNivel, sn.OrdenNivel, nj.NomJornada, nj.OrdenJornada, jc.NomCurso, "
                    + " jc.OrdenCurso, cp.CodigoParalelo, cp.NomParalelo, cp.OrdenParalelo ";
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 0;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new aca_MatriculaCalificacion_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdMatricula = Convert.ToInt32(reader["IdMatricula"]),
                            IdProfesor = string.IsNullOrEmpty(reader["IdProfesor"].ToString()) ? (decimal?)null :  Convert.ToDecimal(reader["IdProfesor"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            IdSede = Convert.ToInt32(reader["IdSede"]),
                            IdNivel = Convert.ToInt32(reader["IdNivel"]),
                            IdJornada = Convert.ToInt32(reader["IdJornada"]),
                            IdCurso = Convert.ToInt32(reader["IdCurso"]),
                            IdParalelo = Convert.ToInt32(reader["IdParalelo"]),
                            Descripcion = string.IsNullOrEmpty(reader["Descripcion"].ToString()) ? null : reader["Descripcion"].ToString(),
                            NomSede = string.IsNullOrEmpty(reader["NomSede"].ToString()) ? null : reader["NomSede"].ToString(),
                            NomNivel = string.IsNullOrEmpty(reader["NomNivel"].ToString()) ? null : reader["NomNivel"].ToString(),
                            OrdenNivel = string.IsNullOrEmpty(reader["OrdenNivel"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenNivel"]),
                            NomJornada = string.IsNullOrEmpty(reader["NomJornada"].ToString()) ? null : reader["NomJornada"].ToString(),
                            OrdenJornada = string.IsNullOrEmpty(reader["OrdenJornada"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenJornada"]),
                            NomCurso = string.IsNullOrEmpty(reader["NomCurso"].ToString()) ? null : reader["NomCurso"].ToString(),
                            OrdenCurso = string.IsNullOrEmpty(reader["OrdenCurso"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenCurso"]),
                            NomParalelo = string.IsNullOrEmpty(reader["NomParalelo"].ToString()) ? null : reader["NomParalelo"].ToString(),
                            OrdenParalelo = string.IsNullOrEmpty(reader["OrdenParalelo"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenParalelo"]),
                            CodigoParalelo = string.IsNullOrEmpty(reader["CodigoParalelo"].ToString()) ? null : reader["CodigoParalelo"].ToString()
                        });
                    }
                    reader.Close();
                }

                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<aca_MatriculaCalificacion_Info> getList_CombosCalificacionesGrado(int IdEmpresa, int IdSede, int IdCurso)
        {
            try
            {
                List<aca_MatriculaCalificacion_Info> Lista = new List<aca_MatriculaCalificacion_Info>();

                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();

                    #region Query
                    string query = "SELECT mc.IdEmpresa, mc.IdMatricula, mc.IdMateria, mc.IdProfesor, c.IdAnio, c.IdSede, c.IdNivel, c.IdJornada, c.IdCurso, c.IdParalelo, a.Descripcion, sn.NomSede, sn.NomNivel, sn.OrdenNivel, nj.NomJornada, nj.OrdenJornada, jc.NomCurso, "
                    + " jc.OrdenCurso, cp.CodigoParalelo, cp.NomParalelo, cp.OrdenParalelo, cp.IdProfesorTutor, cp.IdProfesorInspector, cm.NomMateria, cm.EsObligatorio, cm.OrdenMateria "
                    + " FROM     dbo.aca_MatriculaCalificacion AS mc WITH (nolock) INNER JOIN "
                    + " dbo.aca_Matricula AS c WITH (nolock) ON mc.IdEmpresa = c.IdEmpresa AND mc.IdMatricula = c.IdMatricula INNER JOIN "
                    + " dbo.aca_AnioLectivo AS a WITH (nolock) ON c.IdAnio = a.IdAnio AND c.IdEmpresa = a.IdEmpresa INNER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Materia AS cm WITH (nolock) ON c.IdEmpresa = cm.IdEmpresa AND c.IdAnio = cm.IdAnio AND c.IdSede = cm.IdSede AND c.IdNivel = cm.IdNivel AND c.IdJornada = cm.IdJornada AND c.IdCurso = cm.IdCurso AND "
                    + " mc.IdMateria = cm.IdMateria LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_NivelAcademico_Jornada AS nj WITH (nolock) LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Sede_NivelAcademico AS sn WITH (nolock) ON nj.IdEmpresa = sn.IdEmpresa AND nj.IdAnio = sn.IdAnio AND nj.IdSede = sn.IdSede AND nj.IdNivel = sn.IdNivel RIGHT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Jornada_Curso AS jc WITH (nolock) ON nj.IdEmpresa = jc.IdEmpresa AND nj.IdAnio = jc.IdAnio AND nj.IdSede = jc.IdSede AND nj.IdNivel = jc.IdNivel AND nj.IdJornada = jc.IdJornada RIGHT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Paralelo AS cp WITH (nolock) ON jc.IdEmpresa = cp.IdEmpresa AND jc.IdAnio = cp.IdAnio AND jc.IdSede = cp.IdSede AND jc.IdNivel = cp.IdNivel AND jc.IdJornada = cp.IdJornada AND jc.IdCurso = cp.IdCurso ON "
                    + " c.IdEmpresa = cp.IdEmpresa AND c.IdAnio = cp.IdAnio AND c.IdSede = cp.IdSede AND c.IdNivel = cp.IdNivel AND c.IdJornada = cp.IdJornada AND c.IdCurso = cp.IdCurso AND c.IdParalelo = cp.IdParalelo "
                    + " WHERE mc.IdEmpresa = " + IdEmpresa.ToString() + " and c.IdSede = " + IdSede.ToString() + " AND c.IdCurso= "+ IdCurso.ToString()
                    + " GROUP BY mc.IdEmpresa, mc.IdMatricula, mc.IdMateria, mc.IdProfesor, c.IdAnio, c.IdSede, c.IdNivel, c.IdJornada, c.IdCurso, c.IdParalelo, a.Descripcion, sn.NomSede, sn.NomNivel, sn.OrdenNivel, nj.NomJornada, nj.OrdenJornada, jc.NomCurso,  "
                    + " jc.OrdenCurso, cp.CodigoParalelo, cp.NomParalelo, cp.OrdenParalelo, cp.IdProfesorTutor, cp.IdProfesorInspector, cm.NomMateria, cm.OrdenMateria, cm.EsObligatorio ";
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 0;
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Lista.Add(new aca_MatriculaCalificacion_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdMatricula = Convert.ToDecimal(reader["IdMatricula"]),
                            IdMateria = Convert.ToInt32(reader["IdMateria"]),
                            IdProfesor = string.IsNullOrEmpty(reader["IdProfesor"].ToString()) ? (decimal?)null : Convert.ToInt32(reader["IdProfesor"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            IdSede = Convert.ToInt32(reader["IdSede"]),
                            IdNivel = Convert.ToInt32(reader["IdNivel"]),
                            IdJornada = Convert.ToInt32(reader["IdJornada"]),
                            IdCurso = Convert.ToInt32(reader["IdCurso"]),
                            IdParalelo = Convert.ToInt32(reader["IdParalelo"]),
                            Descripcion = string.IsNullOrEmpty(reader["Descripcion"].ToString()) ? null : reader["Descripcion"].ToString(),
                            NomSede = string.IsNullOrEmpty(reader["NomSede"].ToString()) ? null : reader["NomSede"].ToString(),
                            NomNivel = string.IsNullOrEmpty(reader["NomNivel"].ToString()) ? null : reader["NomNivel"].ToString(),
                            OrdenNivel = string.IsNullOrEmpty(reader["OrdenNivel"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenNivel"]),
                            NomJornada = string.IsNullOrEmpty(reader["NomJornada"].ToString()) ? null : reader["NomJornada"].ToString(),
                            OrdenJornada = string.IsNullOrEmpty(reader["OrdenJornada"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenJornada"]),
                            NomCurso = string.IsNullOrEmpty(reader["NomCurso"].ToString()) ? null : reader["NomCurso"].ToString(),
                            OrdenCurso = string.IsNullOrEmpty(reader["OrdenCurso"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenCurso"]),
                            NomParalelo = string.IsNullOrEmpty(reader["NomParalelo"].ToString()) ? null : reader["NomParalelo"].ToString(),
                            OrdenParalelo = string.IsNullOrEmpty(reader["OrdenParalelo"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenParalelo"]),
                            CodigoParalelo = string.IsNullOrEmpty(reader["CodigoParalelo"].ToString()) ? null : reader["CodigoParalelo"].ToString(),
                            IdProfesorTutor = string.IsNullOrEmpty(reader["IdProfesorTutor"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["IdProfesorTutor"]),
                            IdProfesorInspector = string.IsNullOrEmpty(reader["IdProfesorInspector"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["IdProfesorInspector"]),
                            NomMateria = string.IsNullOrEmpty(reader["NomMateria"].ToString()) ? null : reader["NomMateria"].ToString(),
                            OrdenMateria = string.IsNullOrEmpty(reader["OrdenMateria"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenMateria"]),
                            EsObligatorio = string.IsNullOrEmpty(reader["EsObligatorio"].ToString()) ? false : Convert.ToBoolean(reader["EsObligatorio"]),
                        });
                    }
                    reader.Close();
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
