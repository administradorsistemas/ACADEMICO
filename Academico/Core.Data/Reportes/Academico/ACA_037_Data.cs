﻿using Core.Data.Base;
using Core.Info.Helps;
using Core.Info.Reportes.Academico;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Core.Data.Reportes.Academico
{
    public class ACA_037_Data
    {
        cl_funciones funciones = new cl_funciones();
        public List<ACA_037_Info> GetList(int IdEmpresa, int IdAnio, int IdSede, int IdNivel, int IdJornada, int IdCurso, int IdParalelo, decimal IdProfesor, bool MostrarRetirados)
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

                List<ACA_037_Info> Lista = new List<ACA_037_Info>();
                List<ACA_037_Info> ListaObligatorias = new List<ACA_037_Info>();
                List<ACA_037_Info> ListaComplementarias = new List<ACA_037_Info>();
                List<ACA_037_Info> ListaPromediadaComplementarias = new List<ACA_037_Info>();

                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();

                    #region Query
                    string query = "DECLARE @MostrarRetirados int = " + (MostrarRetirados == true ? 1 : 0) + ";"
                    + " SELECT m.IdEmpresa, m.IdMatricula, m.IdAnio, m.IdSede, m.IdNivel, m.IdJornada, m.IdCurso, m.IdParalelo, m.IdAlumno, CASE WHEN r.IdRetiro IS NULL THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END AS EsRetirado, sn.NomSede, sn.NomNivel, "
                    + " sn.OrdenNivel, nj.NomJornada, nj.OrdenJornada, jc.NomCurso, jc.OrdenCurso, cp.NomParalelo, cp.OrdenParalelo, p.pe_nombreCompleto,  p.pe_cedulaRuc, s.NombreRector, s.NombreSecretaria,an.Descripcion,  "
                    + " s.TelefonoRector, s.CelularRector, s.CorreoRector, mc.IdProfesor, mc.IdCampoAccion, mc.IdTematica, t.NombreCampoAccion, t.NombreTematica, per.pe_nombreCompleto NombreProfesor,per.pe_telfono_Contacto TelefonoProfesor, pro.Telefonos CelularProfesor, pro.Correo CorreoProfesor "
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
                    command.CommandTimeout = 5000;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new ACA_037_Info
                        {
                            Num = 1,
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdMatricula = Convert.ToDecimal(reader["IdMatricula"]),
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
                return Lista;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
