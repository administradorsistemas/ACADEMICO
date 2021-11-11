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
    public class ACA_043_Data
    {
        aca_AnioLectivo_Data odata_anio = new aca_AnioLectivo_Data();
        public List<ACA_043_Info> get_list(int IdEmpresa, int IdAnio, int IdSede, int IdNivel, int IdJornada, int IdCurso, int IdParalelo, bool MostrarRetirados)
        {
            try
            {
                int IdNivelIni = IdNivel;
                int IdNivelFin = IdNivel == 0 ? 9999999 : IdNivel;

                int IdJornadaIni = IdJornada;
                int IdJornadaFin = IdJornada == 0 ? 9999999 : IdJornada;

                int IdCursoIni = IdCurso;
                int IdCursoFin = IdCurso == 0 ? 9999999 : IdCurso;

                int IdParaleloIni = IdParalelo;
                int IdParaleloFin = IdParalelo == 0 ? 9999999 : IdParalelo;

                List<ACA_043_Info> Lista = new List<ACA_043_Info>();
                List<ACA_043_Info> ListaPromedio = new List<ACA_043_Info>();
                List<ACA_043_Info> ListaHoras = new List<ACA_043_Info>();
                List<ACA_043_Info> ListaAlumosPromedio = new List<ACA_043_Info>();
                List<ACA_043_Info> ListaFinal = new List<ACA_043_Info>();

                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();

                    #region Query
                    string query = "DECLARE @MostrarRetirados int = " + (MostrarRetirados == true ? 1 : 0) + ";"
                    + " SELECT m.IdEmpresa, m.IdMatricula, m.IdAnio, m.IdSede, m.IdNivel, m.IdJornada, m.IdCurso, m.IdParalelo, m.IdAlumno, CASE WHEN r.IdRetiro IS NULL THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END AS EsRetirado, sn.NomSede, sn.NomNivel, "
                    + " sn.OrdenNivel, nj.NomJornada, nj.OrdenJornada, jc.NomCurso, jc.OrdenCurso, cp.NomParalelo, cp.OrdenParalelo, mc.IdCampoAccion, mc.IdTematica, t.NombreCampoAccion, t.NombreTematica, p.pe_nombreCompleto,  p.pe_cedulaRuc, s.NombreRector, s.NombreSecretaria,an.Descripcion,  "
                    + " s.TelefonoRector, s.CelularRector, s.CorreoRector, mc.IdProfesor, per.pe_nombreCompleto NombreProfesor,per.pe_telfono_Contacto TelefonoProfesor, pro.Telefonos CelularProfesor, pro.Correo CorreoProfesor "
                    + " FROM dbo.aca_Matricula AS m WITH (nolock) RIGHT OUTER JOIN "
                    + " dbo.aca_MatriculaCalificacionParticipacion AS mc WITH (nolock) ON m.IdEmpresa = mc.IdEmpresa AND m.IdAlumno = mc.IdAlumno AND m.IdMatricula = mc.IdMatricula "
                    + " LEFT OUTER JOIN dbo.aca_AnioLectivo_Tematica t WITH (nolock) on t.IdEmpresa= mc.IdEmpresa and t.IdAnio = m.IdAnio and t.IdCampoAccion=mc.IdCampoAccion and t.IdTematica=mc.IdTematica "
                    + " LEFT OUTER JOIN dbo.aca_Profesor pro WITH (nolock) on pro.IdEmpresa = mc.IdEmpresa and pro.IdProfesor = mc.IdProfesor "
                    + " LEFT OUTER JOIN dbo.tb_persona AS per WITH (nolock) ON per.IdPersona = pro.IdPersona LEFT OUTER JOIN"
                    + " dbo.aca_Alumno AS al WITH (nolock) ON m.IdEmpresa = al.IdEmpresa AND m.IdAlumno = al.IdAlumno LEFT OUTER JOIN "
                    + " dbo.tb_persona AS p WITH (nolock) ON al.IdPersona = p.IdPersona LEFT OUTER JOIN "
                    + " dbo.aca_AlumnoRetiro AS r WITH (nolock) ON m.IdEmpresa = r.IdEmpresa AND m.IdMatricula = r.IdMatricula AND r.Estado = 1 INNER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Paralelo AS cp WITH (nolock) ON m.IdEmpresa = cp.IdEmpresa AND m.IdAnio = cp.IdAnio AND m.IdSede = cp.IdSede AND m.IdNivel = cp.IdNivel AND m.IdJornada = cp.IdJornada AND m.IdCurso = cp.IdCurso AND "
                    + " m.IdParalelo = cp.IdParalelo LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Jornada_Curso AS jc WITH (nolock) ON m.IdEmpresa = jc.IdEmpresa AND m.IdAnio = jc.IdAnio AND m.IdSede = jc.IdSede AND m.IdNivel = jc.IdNivel AND m.IdJornada = jc.IdJornada AND m.IdCurso = jc.IdCurso LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_NivelAcademico_Jornada AS nj WITH (nolock) ON m.IdEmpresa = nj.IdEmpresa AND m.IdAnio = nj.IdAnio AND m.IdSede = nj.IdSede AND m.IdNivel = nj.IdNivel AND m.IdJornada = nj.IdJornada LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Sede_NivelAcademico AS sn WITH (nolock) ON m.IdEmpresa = sn.IdEmpresa AND m.IdAnio = sn.IdAnio AND m.IdSede = sn.IdSede AND m.IdNivel = sn.IdNivel LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo AS an WITH (nolock) ON m.IdEmpresa = an.IdEmpresa AND m.IdAnio = an.IdAnio "
                    + " LEFT OUTER JOIN aca_Sede s WITH (nolock) on s.IdEmpresa=m.IdEmpresa and s.IdSede = m.IdSede "
                    + " WHERE m.IdEmpresa = " + IdEmpresa.ToString()
                    + " and m.IdAnio = " + IdAnio.ToString()
                    + " and m.IdSede = " + IdSede.ToString()
                    + " and m.IdJornada = " + IdJornada.ToString()
                    + " and m.IdNivel between " + IdNivelIni.ToString() + " and " + IdNivelFin.ToString()
                    + " and m.IdCurso between " + IdCursoIni.ToString() + " and " + IdCursoFin.ToString()
                    + " and m.IdParalelo between " + IdParaleloIni.ToString() + " and " + IdParaleloFin.ToString()
                    + " and isnull(r.IdMatricula,0) = case when @MostrarRetirados = 1 then isnull(r.IdMatricula,0) else 0 end ";
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new ACA_043_Info
                        {
                            Num = 1,
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdMatricula = Convert.ToDecimal(reader["IdMatricula"]),
                            IdAlumno = Convert.ToDecimal(reader["IdAlumno"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            IdSede = Convert.ToInt32(reader["IdSede"]),
                            IdJornada = Convert.ToInt32(reader["IdJornada"]),
                            IdNivel = Convert.ToInt32(reader["IdNivel"]),
                            IdCurso = Convert.ToInt32(reader["IdCurso"]),
                            IdParalelo = Convert.ToInt32(reader["IdParalelo"]),
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
                            pe_nombreCompleto = string.IsNullOrEmpty(reader["pe_nombreCompleto"].ToString()) ? null : reader["pe_nombreCompleto"].ToString(),
                            pe_cedulaRuc = string.IsNullOrEmpty(reader["pe_cedulaRuc"].ToString()) ? null : reader["pe_cedulaRuc"].ToString(),
                            NombreProfesor = string.IsNullOrEmpty(reader["NombreProfesor"].ToString()) ? null : reader["NombreProfesor"].ToString(),
                            TelefonoProfesor = string.IsNullOrEmpty(reader["TelefonoProfesor"].ToString()) ? null : reader["TelefonoProfesor"].ToString(),
                            CelularProfesor = string.IsNullOrEmpty(reader["CelularProfesor"].ToString()) ? null : reader["CelularProfesor"].ToString(),
                            CelularRector = string.IsNullOrEmpty(reader["CelularRector"].ToString()) ? null : reader["CelularRector"].ToString(),
                            CorreoProfesor = string.IsNullOrEmpty(reader["CorreoProfesor"].ToString()) ? null : reader["CorreoProfesor"].ToString(),
                            CorreoRector = string.IsNullOrEmpty(reader["CorreoRector"].ToString()) ? null : reader["CorreoRector"].ToString(),
                            NombreRector = string.IsNullOrEmpty(reader["NombreRector"].ToString()) ? null : reader["NombreRector"].ToString(),
                            TelefonoRector = string.IsNullOrEmpty(reader["TelefonoRector"].ToString()) ? null : reader["TelefonoRector"].ToString(),
                            NombreCampoAccion = string.IsNullOrEmpty(reader["NombreCampoAccion"].ToString()) ? null : reader["NombreCampoAccion"].ToString(),
                            NombreTematica = string.IsNullOrEmpty(reader["NombreTematica"].ToString()) ? null : reader["NombreTematica"].ToString(),
                        });
                    }
                    reader.Close();
                }

                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();

                    #region Query
                    //string query = "declare @IdEmpresa int =" + IdEmpresa.ToString() + ", "
                    //+ " @IdAnio int =" + IdAnio.ToString() + ","
                    //+ " @IdSede int = " + IdSede.ToString() + ","
                    //+ " @IdNivel int = " + IdNivel.ToString() + ","
                    //+ " @IdJornada int = " + IdJornada.ToString() + ","
                    //+ " @wIdNivel int, "
                    //+ " @wIdCurso int, "
                    //+ " @wOrden1 int, "
                    //+ " @wOrden2 int, "
                    //+ " @wOrden3 int "
                    //+ " select @wIdCurso = IdCursoBachiller from aca_AnioLectivo where IdEmpresa = @IdEmpresa and IdAnio = @IdAnio;select @wIdNivel = a.IdNivel "
                    //+ " from aca_AnioLectivo_Jornada_Curso as a WITH (nolock) "
                    //+ " join "
                    //+ " aca_AnioLectivo_NivelAcademico_Jornada as b WITH (nolock) on a.IdEmpresa = b.IdEmpresa and a.IdAnio = b.IdAnio and a.IdSede = b.IdSede and a.IdNivel = b.IdNivel and a.IdJornada = b.IdJornada "
                    //+ " where a.IdEmpresa = @IdEmpresa and a.IdAnio = @IdAnio and a.IdCurso = @wIdCurso "
                    //+ " select @wOrden1 = min(jc.OrdenCurso) from aca_AnioLectivo_Jornada_Curso as jc "
                    //+ " where jc.IdEmpresa = @IdEmpresa and jc.IdAnio = @IdAnio and jc.IdSede = @IdSede and jc.IdJornada = @IdJornada and jc.IdNivel = @wIdNivel "
                    //+ " set @wOrden2 = @wOrden1 + 1 "
                    //+ " set @wOrden3 = @wOrden2 + 1 "
                    //+ " select a.IdEmpresa, a.IdAlumno, max(a.Promedio1) Promedio1, max(a.Promedio2) Promedio2, max(a.Promedio3) Promedio3, avg(a.PromedioFinal) PromedioFinal "
                    //+ " from("
                    //+ " select a.IdEmpresa, a.IdAlumno, "
                    //+ " case when jc.OrdenCurso = @wOrden1 then b.PromedioFinal else null end as Promedio1, "
                    //+ " case when jc.OrdenCurso = @wOrden2 then b.PromedioFinal else null end as Promedio2, "
                    //+ " case when jc.OrdenCurso = @wOrden3 then b.PromedioFinal else null end as Promedio3, "
                    //+ " case when jc.OrdenCurso in (@wOrden1, @wOrden2, @wOrden3) then b.PromedioFinal else null end as PromedioFinal "
                    //+ " from aca_matricula a WITH (nolock) join "
                    //+ " aca_MatriculaCalificacionParticipacion as b WITH (nolock) on a.IdEmpresa = b.IdEmpresa and a.IdAlumno = b.IdAlumno join "
                    //+ " aca_AnioLectivo_Jornada_Curso as jc WITH (nolock) on a.IdEmpresa = jc.IdEmpresa and a.IdAnio = jc.IdAnio and a.IdSede = jc.IdSede and a.IdNivel = jc.IdNivel and a.IdJornada = jc.IdJornada and a.IdCurso = jc.IdCurso "
                    //+ " where a.IdEmpresa = @IdEmpresa and a.IdAnio= @IdAnio and a.IdNivel = @wIdNivel and jc.OrdenCurso in (@wOrden1, @wOrden2, @wOrden3) "
                    //+ " ) a "
                    //+ " group by a.IdEmpresa, a.IdAlumno";

                    string query = "declare @IdEmpresa int =" + IdEmpresa.ToString() + ", "
                    + " @IdAnio int =" + IdAnio.ToString() + ","
                    + " @IdSede int = " + IdSede.ToString() + ","
                    + " @IdNivel int = " + IdNivel.ToString() + ","
                    + " @IdJornada int = " + IdJornada.ToString() + ","
                    + " @wIdNivel int, "
                    + " @wIdCurso int, "
                    + " @wOrden1 int, "
                    + " @wOrden2 int, "
                    + " @wOrden3 int "
                    + " select @wIdCurso = IdCursoBachiller from aca_AnioLectivo where IdEmpresa = @IdEmpresa and IdAnio = @IdAnio; select @wIdNivel = a.IdNivel "
                    + " from aca_AnioLectivo_Jornada_Curso as a WITH(nolock) "
                    + " join "
                    + " aca_AnioLectivo_NivelAcademico_Jornada as b WITH(nolock) on a.IdEmpresa = b.IdEmpresa and a.IdAnio = b.IdAnio and a.IdSede = b.IdSede and a.IdNivel = b.IdNivel and a.IdJornada = b.IdJornada "
                    + " where a.IdEmpresa = @IdEmpresa and a.IdAnio = @IdAnio and a.IdCurso = @wIdCurso "
                    + " select @wOrden1 = min(jc.OrdenCurso) from aca_AnioLectivo_Jornada_Curso as jc WITH(nolock) "
                    + " where jc.IdEmpresa = @IdEmpresa and jc.IdAnio = @IdAnio and jc.IdSede = @IdSede and jc.IdJornada = @IdJornada and jc.IdNivel = @wIdNivel "
                    + " set @wOrden2 = @wOrden1 + 1 "
                    + " set @wOrden3 = @wOrden2 + 1 "
                    + " select a.IdEmpresa, a.IdAlumno,max(a.Promedio1) Promedio1, max(a.Promedio2) Promedio2, max(a.Promedio3) Promedio3, avg(a.PromedioFinal) PromedioFinal "
                    + " from("
                    + " /**/ "
                    + " select a.IdEmpresa, a.IdAlumno, "
                    + " case when jc.OrdenCurso = @wOrden1 then b.PromedioFinal else null end as Promedio1, "
                    + " case when jc.OrdenCurso = @wOrden2 then b.PromedioFinal else null end as Promedio2, "
                    + " case when jc.OrdenCurso = @wOrden3 then b.PromedioFinal else null end as Promedio3, "
                    + " case when jc.OrdenCurso in (@wOrden1, @wOrden2, @wOrden3) then b.PromedioFinal else null end as PromedioFinal "
                    + " from aca_matricula a WITH(nolock) "
                    + " join "
                    + " aca_MatriculaCalificacionParticipacion as b WITH(nolock) on a.IdEmpresa = b.IdEmpresa and a.IdMatricula = b.IdMatricula and a.IdAlumno = b.IdAlumno join "
                    + " aca_AnioLectivo_Jornada_Curso as jc WITH(nolock) on a.IdEmpresa = jc.IdEmpresa and a.IdAnio = jc.IdAnio and a.IdSede = jc.IdSede and a.IdNivel = jc.IdNivel and a.IdJornada = jc.IdJornada and a.IdCurso = jc.IdCurso "
                    + " where a.IdEmpresa = @IdEmpresa and a.IdNivel = @wIdNivel and jc.OrdenCurso in (@wOrden1, @wOrden2, @wOrden3) "
                    + " union all("
                    + " select c.IdEmpresa, c.IdAlumno, "
                    + " case when jc1.OrdenCurso = @wOrden1 then c.PromedioFinal else null end as Promedio1, "
                    + " case when jc1.OrdenCurso = @wOrden2 then c.PromedioFinal else null end as Promedio2, "
                    + " case when jc1.OrdenCurso = @wOrden3 then c.PromedioFinal else null end as Promedio3, "
                    + " case when jc1.OrdenCurso in (@wOrden1, @wOrden2, @wOrden3) then c.PromedioFinal  else null end as PromedioFinal "
                    + " /*HISTORICO*/ "
                    + " from aca_AnioLectivoCalificacionParticipacionHistorico as c WITH(nolock) join "
                    + " aca_AnioLectivo_Jornada_Curso as jc1 WITH(nolock) on c.IdEmpresa = jc1.IdEmpresa and c.IdAnio = jc1.IdAnio and c.IdSede = jc1.IdSede "
                    + " and c.IdNivel = jc1.IdNivel and c.IdJornada = jc1.IdJornada and c.IdCurso = jc1.IdCurso "
                    + " /***************/ "
                    + " where c.IdEmpresa = @IdEmpresa and c.IdNivel = @wIdNivel and jc1.OrdenCurso in (@wOrden1, @wOrden2, @wOrden3)  "
                    + " ) "
                    + " /**/ "
                    + " ) a "
                    + " group by a.IdEmpresa, a.IdAlumno";
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        ListaPromedio.Add(new ACA_043_Info
                        {
                            Num = 1,
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdAlumno = Convert.ToDecimal(reader["IdAlumno"]),
                            Promedio1 = string.IsNullOrEmpty(reader["Promedio1"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["Promedio1"]),
                            Promedio2 = string.IsNullOrEmpty(reader["Promedio2"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["Promedio2"]),
                            Promedio3 = string.IsNullOrEmpty(reader["Promedio3"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["Promedio3"]),
                            PromedioFinal = string.IsNullOrEmpty(reader["PromedioFinal"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["PromedioFinal"])
                        });
                    }
                    reader.Close();
                }


                ListaAlumosPromedio = (from a in Lista
                                       join b in ListaPromedio
                                       on new { a.IdEmpresa, a.IdAlumno } equals new { b.IdEmpresa, b.IdAlumno }
                                       select new ACA_043_Info
                                       {
                                           Num = 1,
                                           IdEmpresa = a.IdEmpresa,
                                           IdAlumno = a.IdAlumno,
                                           pe_nombreCompleto = a.pe_nombreCompleto,
                                           pe_cedulaRuc = a.pe_cedulaRuc,
                                           IdAnio = a.IdAnio,
                                           IdSede = a.IdSede,
                                           IdNivel = a.IdNivel,
                                           IdJornada = a.IdJornada,
                                           IdCurso = a.IdCurso,
                                           IdParalelo = a.IdParalelo,
                                           Descripcion = a.Descripcion,
                                           NomSede = a.NomSede,
                                           NomNivel = a.NomNivel,
                                           NomJornada = a.NomJornada,
                                           NomCurso = a.NomCurso,
                                           NomParalelo = a.NomParalelo,
                                           OrdenNivel = a.OrdenNivel,
                                           OrdenJornada = a.OrdenJornada,
                                           OrdenCurso = a.OrdenCurso,
                                           OrdenParalelo = a.OrdenParalelo,
                                           NombreCampoAccion = a.NombreCampoAccion,
                                           NombreTematica = a.NombreTematica,
                                           NombreProfesor = a.NombreProfesor,
                                           TelefonoProfesor = a.TelefonoProfesor,
                                           CelularProfesor = a.CelularProfesor,
                                           NombreRector = a.NombreRector,
                                           TelefonoRector = a.TelefonoRector,
                                           CelularRector = a.CelularRector,
                                           Promedio1 = b.Promedio1,
                                           Promedio2 = b.Promedio2,
                                           Promedio3 = b.Promedio3,
                                           PromedioFinal = b.PromedioFinal
                                       }).ToList();


                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();

                    foreach (var item in Lista)
                    {
                        #region Query
                        //string query = "select a.IdEmpresa, a.IdAlumno, count(*) NumCalificaciones, count(*) *100 NumHoras"
                        //+ " from aca_MatriculaCalificacionParticipacion as a WITH (nolock) "
                        //+ " where a.IdEmpresa = " + item.IdEmpresa + " and a.IdAlumno = " + item.IdAlumno.ToString()
                        //+ " group by a.IdEmpresa, a.IdAlumno ";
                        string query = "select c.IdEmpresa, c.IdAlumno, count(*) NumCalificaciones, count(*) *100 NumHoras"
                        + " from"
                        + " ("
                        + " select a.IdEmpresa, a.IdAlumno, a.PromedioFinal"
                        + " from aca_MatriculaCalificacionParticipacion as a WITH(nolock)"
                        + " where a.IdEmpresa = " + item.IdEmpresa + " and a.IdAlumno = " + item.IdAlumno.ToString()
                        + " union( "
                        + " select b.IdEmpresa, b.IdAlumno, b.PromedioFinal"
                        + " from aca_AnioLectivoCalificacionParticipacionHistorico b"
                        + " where b.IdEmpresa = " + item.IdEmpresa + " and b.IdAlumno = " + item.IdAlumno.ToString()
                        + " )"
                        + " )c"
                        + " group by c.IdEmpresa, c.IdAlumno ";
                        #endregion

                        SqlCommand command = new SqlCommand(query, connection);
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            ListaHoras.Add(new ACA_043_Info
                            {
                                IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                                IdAlumno = Convert.ToDecimal(reader["IdAlumno"]),
                                NumCalificaciones = string.IsNullOrEmpty(reader["NumCalificaciones"].ToString()) ? 0 : Convert.ToInt32(reader["NumCalificaciones"]),
                                NumHoras = string.IsNullOrEmpty(reader["NumHoras"].ToString()) ? 0 : Convert.ToInt32(reader["NumHoras"])
                            });
                        }
                        reader.Close();
                    }
                }

                ListaFinal = (from a in ListaAlumosPromedio
                              join b in ListaHoras
                              on new { a.IdEmpresa, a.IdAlumno } equals new { b.IdEmpresa, b.IdAlumno }
                              select new ACA_043_Info
                              {
                                  Num = 1,
                                  IdEmpresa = a.IdEmpresa,
                                  IdAlumno = a.IdAlumno,
                                  pe_nombreCompleto = a.pe_nombreCompleto,
                                  pe_cedulaRuc = a.pe_cedulaRuc,
                                  IdAnio = a.IdAnio,
                                  IdSede = a.IdSede,
                                  IdNivel = a.IdNivel,
                                  IdJornada = a.IdJornada,
                                  IdCurso = a.IdCurso,
                                  IdParalelo = a.IdParalelo,
                                  Descripcion = a.Descripcion,
                                  NomSede = a.NomSede,
                                  NomNivel = a.NomNivel,
                                  NomJornada = a.NomJornada,
                                  NomCurso = a.NomCurso,
                                  NomParalelo = a.NomParalelo,
                                  OrdenNivel = a.OrdenNivel,
                                  OrdenJornada = a.OrdenJornada,
                                  OrdenCurso = a.OrdenCurso,
                                  OrdenParalelo = a.OrdenParalelo,
                                  NombreCampoAccion = a.NombreCampoAccion,
                                  NombreTematica = a.NombreTematica,
                                  NombreProfesor = a.NombreProfesor,
                                  TelefonoProfesor = a.TelefonoProfesor,
                                  CelularProfesor = a.CelularProfesor,
                                  NombreRector = a.NombreRector,
                                  TelefonoRector = a.TelefonoRector,
                                  CelularRector = a.CelularRector,
                                  Promedio1 = a.Promedio1,
                                  Promedio2 = a.Promedio2,
                                  Promedio3 = a.Promedio3,
                                  PromedioFinal = a.PromedioFinal,
                                  NumHoras = b.NumHoras,
                                  NumCalificaciones = b.NumCalificaciones
                              }).ToList();

                return ListaFinal;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
