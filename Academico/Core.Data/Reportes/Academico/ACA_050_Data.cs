﻿using Core.Data.Academico;
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
    public class ACA_050_Data
    {
        aca_AnioLectivo_Data odata_anio = new aca_AnioLectivo_Data();
        public List<ACA_050_Info> get_list(int IdEmpresa, int IdAnio, int IdSede, int IdNivel, int IdJornada, int IdCurso, int IdParalelo, decimal IdAlumno=0, bool MostrarRetirados=false  )
        {
            try
            {
                decimal IdAlumnoIni = IdAlumno;
                decimal IdAlumnoFin = IdAlumno == 0 ? 9999999 : IdAlumno;
                List<ACA_050_Info> Lista = new List<ACA_050_Info>();
                List<ACA_050_Info> Lista_Final = new List<ACA_050_Info>();

                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();

                    #region Query
                    string query = "SELECT m.IdEmpresa, m.IdMatricula, m.IdAnio, m.IdSede, m.IdNivel, m.IdJornada, m.IdCurso, m.IdParalelo, m.IdAlumno, "
                    + " a.Descripcion, sn.NomSede, sn.NomNivel, sn.OrdenNivel, nj.NomJornada, nj.OrdenJornada, jc.NomCurso, jc.OrdenCurso, cp.NomParalelo, cp.OrdenParalelo, al.Codigo,  "
                    + " dbo.tb_persona.pe_nombreCompleto NombreAlumno, a.IdCursoBachiller, m.IdCatalogoESTMAT "
                    + " FROM     dbo.aca_Matricula AS m WITH (nolock) LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Sede_NivelAcademico AS sn WITH (nolock) ON m.IdEmpresa = sn.IdEmpresa AND m.IdAnio = sn.IdAnio AND m.IdSede = sn.IdSede AND m.IdNivel = sn.IdNivel LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo AS a WITH (nolock) ON m.IdEmpresa = a.IdEmpresa AND m.IdAnio = a.IdAnio LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Jornada_Curso AS jc WITH (nolock) ON m.IdEmpresa = jc.IdEmpresa AND m.IdAnio = jc.IdAnio AND m.IdSede = jc.IdSede AND m.IdNivel = jc.IdNivel AND m.IdJornada = jc.IdJornada AND m.IdCurso = jc.IdCurso LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Paralelo AS cp WITH (nolock) ON m.IdEmpresa = cp.IdEmpresa AND m.IdAnio = cp.IdAnio AND m.IdSede = cp.IdSede AND m.IdNivel = cp.IdNivel AND m.IdJornada = cp.IdJornada AND m.IdCurso = cp.IdCurso AND "
                    + " m.IdParalelo = cp.IdParalelo LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_NivelAcademico_Jornada AS nj WITH (nolock) ON m.IdEmpresa = nj.IdEmpresa AND m.IdAnio = nj.IdAnio AND m.IdSede = nj.IdSede AND m.IdNivel = nj.IdNivel AND m.IdJornada = nj.IdJornada LEFT OUTER JOIN "
                    + " dbo.tb_persona INNER JOIN "
                    + " dbo.aca_Alumno AS al WITH (nolock) ON dbo.tb_persona.IdPersona = al.IdPersona ON m.IdEmpresa = al.IdEmpresa AND m.IdAlumno = al.IdAlumno "
                    + " LEFT OUTER JOIN(select r.IdEmpresa, r.IdMatricula  from aca_AlumnoRetiro as r WITH (nolock) where r.Estado = 1  ) as ret "
                    + " on m.IdEmpresa = ret.IdEmpresa and m.IdMatricula = ret.IdMatricula "
                    + " WHERE "
                    + " m.IdEmpresa = " + IdEmpresa.ToString()
                    + " and m.IdAnio = " + IdAnio.ToString()
                    + " and m.IdSede = " + IdSede.ToString()
                    + " and m.IdJornada = " + IdJornada.ToString()
                    + " and m.IdNivel = " + IdNivel.ToString()
                    + " and m.IdCurso = " + IdCurso.ToString()
                    + " and m.IdParalelo = " + IdParalelo.ToString()
                    + " and m.IdAlumno between " + IdAlumnoIni.ToString() + " and " + IdAlumnoFin.ToString()
                    + " and(a.Estado = 1) AND(al.Estado = 1) "
                    + " and isnull(ret.IdMatricula, 0) = case when " + (MostrarRetirados == false ? 0 : 1) + " = 1 then isnull(ret.IdMatricula, 0) else 0 end ";
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new ACA_050_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            Codigo = reader["Codigo"].ToString(),
                            IdAlumno = Convert.ToDecimal(reader["IdAlumno"]),
                            IdMatricula = Convert.ToDecimal(reader["IdMatricula"]),
                            NombreAlumno = reader["NombreAlumno"].ToString(),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            IdSede = Convert.ToInt32(reader["IdSede"]),
                            IdNivel = Convert.ToInt32(reader["IdNivel"]),
                            IdJornada = Convert.ToInt32(reader["IdJornada"]),
                            IdCurso = Convert.ToInt32(reader["IdCurso"]),
                            IdParalelo = Convert.ToInt32(reader["IdParalelo"]),
                            Descripcion = reader["Descripcion"].ToString(),
                            NomSede = reader["NomSede"].ToString(),
                            NomNivel = reader["NomNivel"].ToString(),
                            NomJornada = reader["NomJornada"].ToString(),
                            NomCurso = reader["NomCurso"].ToString(),
                            NomParalelo = reader["NomParalelo"].ToString(),
                            OrdenNivel = Convert.ToInt32(reader["OrdenNivel"]),
                            OrdenJornada = Convert.ToInt32(reader["OrdenJornada"]),
                            OrdenCurso = Convert.ToInt32(reader["OrdenCurso"]),
                            OrdenParalelo = Convert.ToInt32(reader["OrdenParalelo"]),
                            IdCatalogoESTMAT = Convert.ToInt32(reader["IdCatalogoESTMAT"]),
                            FechaActual = DateTime.Now.ToString("d' de 'MMMM' de 'yyyy"),
                            EstadoCertificado = (Convert.ToInt32(reader["IdCatalogoESTMAT"]) == Convert.ToInt32(cl_enumeradores.eCatalogoAcademicoMatricula.APROBADO) ? " se incorporó " :
                                                  Convert.ToInt32(reader["IdCatalogoESTMAT"]) == Convert.ToInt32(cl_enumeradores.eCatalogoAcademicoMatricula.REPROBADO) ? " no se incorporó " : "")
                        });
                    }
                    reader.Close();
                }

                var IdCatalogoEstado = Convert.ToInt32(cl_enumeradores.eCatalogoAcademicoMatricula.APROBADO);
                var info_anio = odata_anio.getInfo(IdEmpresa, IdAnio);
                Lista_Final = Lista.Where(q => q.IdCurso == info_anio.IdCursoBachiller && q.IdCatalogoESTMAT == Convert.ToInt32(IdCatalogoEstado)).ToList();

                return Lista_Final;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
