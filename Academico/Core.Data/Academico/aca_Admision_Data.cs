using Core.Data.Base;
using Core.Data.General;
using Core.Info.Academico;
using Core.Info.General;
using Core.Info.Helps;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Academico
{
    public class aca_Admision_Data
    {
        tb_ColaCorreo_Data odata_correo = new tb_ColaCorreo_Data();
        aca_Catalogo_Data odata_catalogo = new aca_Catalogo_Data();
        public List<aca_Admision_Info> getList(int IdEmpresa, int IdSede, int IdAnio)
        {
            try
            {
                List<aca_Admision_Info> Lista = new List<aca_Admision_Info>();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    #region Query
                    string query = "SELECT a.IdEmpresa, a.IdAdmision, a.IdSede, a.IdAnio, a.IdJornada, a.IdNivel, a.IdCurso, a.CedulaRuc_Aspirante, a.NombreCompleto_Aspirante, a.IdCatalogoESTADM, an.Descripcion, sn.NomSede, nj.NomJornada, nj.OrdenJornada, sn.NomNivel, "
                    + " sn.OrdenNivel, jc.NomCurso, jc.OrdenCurso, c.Codigo EstadoAdmision, c.NomCatalogo, a.Estado, a.IdUsuarioRevision, a.FechaIngreso_Aspirante "
                    + " FROM     dbo.aca_Admision AS a WITH (nolock) LEFT OUTER JOIN "
                    + " dbo.aca_Catalogo AS c WITH (nolock) ON a.IdCatalogoESTADM = c.IdCatalogo LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Jornada_Curso AS jc WITH (nolock) ON a.IdEmpresa = jc.IdEmpresa AND a.IdAnio = jc.IdAnio AND a.IdSede = jc.IdSede AND a.IdNivel = jc.IdNivel AND a.IdJornada = jc.IdJornada AND a.IdCurso = jc.IdCurso LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_NivelAcademico_Jornada AS nj WITH (nolock) ON a.IdEmpresa = nj.IdEmpresa AND a.IdAnio = nj.IdAnio AND a.IdSede = nj.IdSede AND a.IdNivel = nj.IdNivel AND a.IdJornada = nj.IdJornada LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Sede_NivelAcademico AS sn WITH (nolock) ON a.IdEmpresa = sn.IdEmpresa AND a.IdAnio = sn.IdAnio AND a.IdSede = sn.IdSede AND a.IdNivel = sn.IdNivel LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo AS an WITH (nolock) ON a.IdEmpresa = an.IdEmpresa AND a.IdAnio = an.IdAnio "
                    + " WHERE a.IdEmpresa = " + IdEmpresa.ToString()
                    + " and a.IdSede = " + IdSede.ToString()
                    +" and a.IdAnio = " + IdAnio.ToString();
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 0;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new aca_Admision_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdAdmision = Convert.ToDecimal(reader["IdAdmision"]),
                            IdSede = Convert.ToInt32(reader["IdSede"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            IdJornada = Convert.ToInt32(reader["IdJornada"]),
                            IdNivel = Convert.ToInt32(reader["IdNivel"]),
                            IdCurso = Convert.ToInt32(reader["IdCurso"]),
                            CedulaRuc_Aspirante = reader["CedulaRuc_Aspirante"].ToString(),
                            NombreCompleto_Aspirante = string.IsNullOrEmpty(reader["NombreCompleto_Aspirante"].ToString()) ? null : reader["NombreCompleto_Aspirante"].ToString(),
                            Descripcion = string.IsNullOrEmpty(reader["Descripcion"].ToString()) ? null : reader["Descripcion"].ToString(),
                            NomSede = string.IsNullOrEmpty(reader["NomSede"].ToString()) ? null : reader["NomSede"].ToString(),
                            NomJornada = string.IsNullOrEmpty(reader["NomJornada"].ToString()) ? null : reader["NomJornada"].ToString(),
                            NomNivel = string.IsNullOrEmpty(reader["NomNivel"].ToString()) ? null : reader["NomNivel"].ToString(),
                            NomCurso = string.IsNullOrEmpty(reader["NomCurso"].ToString()) ? null : reader["NomCurso"].ToString(),
                            OrdenJornada = string.IsNullOrEmpty(reader["OrdenJornada"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenJornada"]),
                            OrdenNivel = string.IsNullOrEmpty(reader["OrdenNivel"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenNivel"]),
                            OrdenCurso = string.IsNullOrEmpty(reader["OrdenCurso"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenCurso"]),
                            Estado = string.IsNullOrEmpty(reader["Estado"].ToString()) ? false : Convert.ToBoolean(reader["Estado"]),
                            EstadoAdmision = string.IsNullOrEmpty(reader["EstadoAdmision"].ToString()) ? null : reader["EstadoAdmision"].ToString(),
                            IdCatalogoESTADM = Convert.ToInt32(reader["IdCatalogoESTADM"]),
                            FechaIngreso_Aspirante = Convert.ToDateTime(reader["FechaIngreso_Aspirante"]),
                            IdUsuarioRevision = string.IsNullOrEmpty(reader["IdUsuarioRevision"].ToString()) ? null : reader["IdUsuarioRevision"].ToString(),
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
        public List<aca_Admision_Info> getList_Admisiones(int IdEmpresa, int IdSede)
        {
            try
            {
                List<aca_Admision_Info> Lista = new List<aca_Admision_Info>();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    #region Query
                    string query = "SELECT a.IdEmpresa, a.IdAdmision, a.IdSede, a.IdAnio, a.IdJornada, a.IdNivel, a.IdCurso, a.CedulaRuc_Aspirante, a.NombreCompleto_Aspirante, a.IdCatalogoESTADM, an.Descripcion, sn.NomSede, nj.NomJornada, nj.OrdenJornada, sn.NomNivel, "
                    + " sn.OrdenNivel, jc.NomCurso, jc.OrdenCurso, a.IdCatalogoESTADM, c.Codigo EstadoAdmision, c.NomCatalogo, a.Estado, a.IdUsuarioRevision, a.FechaIngreso_Aspirante "
                    + " FROM     dbo.aca_Admision AS a WITH (nolock) LEFT OUTER JOIN "
                    + " dbo.aca_Catalogo AS c WITH (nolock) ON a.IdCatalogoESTADM = c.IdCatalogo LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Jornada_Curso AS jc WITH (nolock) ON a.IdEmpresa = jc.IdEmpresa AND a.IdAnio = jc.IdAnio AND a.IdSede = jc.IdSede AND a.IdNivel = jc.IdNivel AND a.IdJornada = jc.IdJornada AND a.IdCurso = jc.IdCurso LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_NivelAcademico_Jornada AS nj WITH (nolock) ON a.IdEmpresa = nj.IdEmpresa AND a.IdAnio = nj.IdAnio AND a.IdSede = nj.IdSede AND a.IdNivel = nj.IdNivel AND a.IdJornada = nj.IdJornada LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Sede_NivelAcademico AS sn WITH (nolock) ON a.IdEmpresa = sn.IdEmpresa AND a.IdAnio = sn.IdAnio AND a.IdSede = sn.IdSede AND a.IdNivel = sn.IdNivel LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo AS an WITH (nolock) ON a.IdEmpresa = an.IdEmpresa AND a.IdAnio = an.IdAnio "
                    + " WHERE a.IdEmpresa = " + IdEmpresa.ToString()
                    + " and a.IdSede = " + IdSede.ToString()
                    + " and a.IdCatalogoESTADM = "+ Convert.ToInt32(cl_enumeradores.eTipoCatalogoAdmision.REGISTRADO);
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 0;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new aca_Admision_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdAdmision = Convert.ToDecimal(reader["IdAdmision"]),
                            IdSede = Convert.ToInt32(reader["IdSede"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            IdJornada = Convert.ToInt32(reader["IdJornada"]),
                            IdNivel = Convert.ToInt32(reader["IdNivel"]),
                            IdCurso = Convert.ToInt32(reader["IdCurso"]),
                            CedulaRuc_Aspirante = reader["CedulaRuc_Aspirante"].ToString(),
                            NombreCompleto_Aspirante = string.IsNullOrEmpty(reader["NombreCompleto_Aspirante"].ToString()) ? null : reader["NombreCompleto_Aspirante"].ToString(),
                            Descripcion = string.IsNullOrEmpty(reader["Descripcion"].ToString()) ? null : reader["Descripcion"].ToString(),
                            NomSede = string.IsNullOrEmpty(reader["NomSede"].ToString()) ? null : reader["NomSede"].ToString(),
                            NomJornada = string.IsNullOrEmpty(reader["NomJornada"].ToString()) ? null : reader["NomJornada"].ToString(),
                            NomNivel = string.IsNullOrEmpty(reader["NomNivel"].ToString()) ? null : reader["NomNivel"].ToString(),
                            NomCurso = string.IsNullOrEmpty(reader["NomCurso"].ToString()) ? null : reader["NomCurso"].ToString(),
                            OrdenJornada = string.IsNullOrEmpty(reader["OrdenJornada"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenJornada"]),
                            OrdenNivel = string.IsNullOrEmpty(reader["OrdenNivel"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenNivel"]),
                            OrdenCurso = string.IsNullOrEmpty(reader["OrdenCurso"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenCurso"]),
                            Estado = string.IsNullOrEmpty(reader["Estado"].ToString()) ? false : Convert.ToBoolean(reader["Estado"]),
                            EstadoAdmision = string.IsNullOrEmpty(reader["EstadoAdmision"].ToString()) ? null : reader["EstadoAdmision"].ToString(),
                            IdCatalogoESTADM = Convert.ToInt32(reader["IdCatalogoESTADM"]),
                            IdUsuarioRevision = string.IsNullOrEmpty(reader["IdUsuarioRevision"].ToString()) ? null : reader["IdUsuarioRevision"].ToString(),
                            FechaIngreso_Aspirante = Convert.ToDateTime(reader["FechaIngreso_Aspirante"].ToString()),
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
        public List<aca_Admision_Info> getList_Academico(int IdEmpresa, int IdSede, int IdAnio)
        {
            try
            {
                List<aca_Admision_Info> Lista = new List<aca_Admision_Info>();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    #region Query
                    string query = "SELECT a.IdEmpresa, a.IdAdmision, a.IdSede, a.IdAnio, a.IdJornada, a.IdNivel, a.IdCurso, a.CedulaRuc_Aspirante, a.NombreCompleto_Aspirante, a.IdCatalogoESTADM, an.Descripcion, sn.NomSede, nj.NomJornada, nj.OrdenJornada, sn.NomNivel, "
                    + " sn.OrdenNivel, jc.NomCurso, jc.OrdenCurso, a.IdCatalogoESTADM, c.Codigo EstadoAdmision, c.NomCatalogo, a.Estado, a.IdUsuarioRevision, a.FechaIngreso_Aspirante "
                    + " FROM     dbo.aca_Admision AS a WITH (nolock) LEFT OUTER JOIN "
                    + " dbo.aca_Catalogo AS c WITH (nolock) ON a.IdCatalogoESTADM = c.IdCatalogo LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Jornada_Curso AS jc WITH (nolock) ON a.IdEmpresa = jc.IdEmpresa AND a.IdAnio = jc.IdAnio AND a.IdSede = jc.IdSede AND a.IdNivel = jc.IdNivel AND a.IdJornada = jc.IdJornada AND a.IdCurso = jc.IdCurso LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_NivelAcademico_Jornada AS nj WITH (nolock) ON a.IdEmpresa = nj.IdEmpresa AND a.IdAnio = nj.IdAnio AND a.IdSede = nj.IdSede AND a.IdNivel = nj.IdNivel AND a.IdJornada = nj.IdJornada LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Sede_NivelAcademico AS sn WITH (nolock) ON a.IdEmpresa = sn.IdEmpresa AND a.IdAnio = sn.IdAnio AND a.IdSede = sn.IdSede AND a.IdNivel = sn.IdNivel LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo AS an WITH (nolock) ON a.IdEmpresa = an.IdEmpresa AND a.IdAnio = an.IdAnio "
                    + " WHERE a.IdEmpresa = " + IdEmpresa.ToString()
                    + " and a.IdSede = " + IdSede.ToString()
                    + " and a.IdAnio = " + IdAnio.ToString()
                    + " and (a.IdCatalogoESTADM = "+ Convert.ToInt32(cl_enumeradores.eTipoCatalogoAdmision.REGISTRADO)
                    + " or a.IdCatalogoESTADM = " + Convert.ToInt32(cl_enumeradores.eTipoCatalogoAdmision.ENPROCESO) + ")";
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 0;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new aca_Admision_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdAdmision = Convert.ToDecimal(reader["IdAdmision"]),
                            IdSede = Convert.ToInt32(reader["IdSede"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            IdJornada = Convert.ToInt32(reader["IdJornada"]),
                            IdNivel = Convert.ToInt32(reader["IdNivel"]),
                            IdCurso = Convert.ToInt32(reader["IdCurso"]),
                            CedulaRuc_Aspirante = reader["CedulaRuc_Aspirante"].ToString(),
                            NombreCompleto_Aspirante = string.IsNullOrEmpty(reader["NombreCompleto_Aspirante"].ToString()) ? null : reader["NombreCompleto_Aspirante"].ToString(),
                            Descripcion = string.IsNullOrEmpty(reader["Descripcion"].ToString()) ? null : reader["Descripcion"].ToString(),
                            NomSede = string.IsNullOrEmpty(reader["NomSede"].ToString()) ? null : reader["NomSede"].ToString(),
                            NomJornada = string.IsNullOrEmpty(reader["NomJornada"].ToString()) ? null : reader["NomJornada"].ToString(),
                            NomNivel = string.IsNullOrEmpty(reader["NomNivel"].ToString()) ? null : reader["NomNivel"].ToString(),
                            NomCurso = string.IsNullOrEmpty(reader["NomCurso"].ToString()) ? null : reader["NomCurso"].ToString(),
                            OrdenJornada = string.IsNullOrEmpty(reader["OrdenJornada"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenJornada"]),
                            OrdenNivel = string.IsNullOrEmpty(reader["OrdenNivel"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenNivel"]),
                            OrdenCurso = string.IsNullOrEmpty(reader["OrdenCurso"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenCurso"]),
                            Estado = string.IsNullOrEmpty(reader["Estado"].ToString()) ? false : Convert.ToBoolean(reader["Estado"]),
                            EstadoAdmision = string.IsNullOrEmpty(reader["EstadoAdmision"].ToString()) ? null : reader["EstadoAdmision"].ToString(),
                            IdCatalogoESTADM = Convert.ToInt32(reader["IdCatalogoESTADM"]),
                            IdUsuarioRevision = string.IsNullOrEmpty(reader["IdUsuarioRevision"].ToString()) ? null : reader["IdUsuarioRevision"].ToString(),
                            FechaIngreso_Aspirante = Convert.ToDateTime(reader["FechaIngreso_Aspirante"].ToString()),
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
        public List<aca_Admision_Info> getList_PreMatriculaAcademico(int IdEmpresa, int IdSede, int IdAnio)
        {
            try
            {
                List<aca_Admision_Info> Lista = new List<aca_Admision_Info>();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    #region Query
                    string query = "SELECT p.IdEmpresa, p.IdPreMatricula, a.IdAdmision, a.IdSede, a.IdAnio, a.IdJornada, a.IdNivel, a.IdCurso, pe.pe_cedulaRuc CedulaRuc_Aspirante, pe.pe_nombreCompleto NombreCompleto_Aspirante, a.IdCatalogoESTADM, an.Descripcion, sn.NomSede, nj.NomJornada, nj.OrdenJornada, sn.NomNivel, "
                    + " sn.OrdenNivel, jc.NomCurso, jc.OrdenCurso, cp.NomParalelo, cp.OrdenParalelo, a.IdCatalogoESTADM, c.Codigo EstadoAdmision, c.NomCatalogo, a.Estado, a.IdUsuarioRevision, a.FechaIngreso_Aspirante "
                    + " FROM dbo.aca_PreMatricula as p LEFT OUTER JOIN "
                    + " dbo.aca_Admision AS a WITH (nolock)ON p.IdEmpresa = 1 and p.IdAdmision = a.IdAdmision LEFT OUTER JOIN "
                    + " dbo.aca_Alumno al WITH(nolock) ON al.IdEmpresa = p.IdEmpresa and al.IdAlumno = p.IdAlumno INNER JOIN "
                    + " dbo.tb_persona pe WITH(nolock) ON pe.IdPersona = al.IdPersona LEFT OUTER JOIN "
                   + " dbo.aca_Catalogo AS c WITH(nolock) ON a.IdCatalogoESTADM = c.IdCatalogo LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Curso_Paralelo AS cp WITH(nolock) ON p.IdEmpresa = cp.IdEmpresa AND p.IdAnio = cp.IdAnio AND p.IdSede = cp.IdSede AND p.IdNivel = cp.IdNivel AND p.IdJornada = cp.IdJornada AND p.IdCurso = cp.IdCurso AND p.IdParalelo = cp.IdParalelo LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Jornada_Curso AS jc WITH(nolock) ON p.IdEmpresa = jc.IdEmpresa AND p.IdAnio = jc.IdAnio AND p.IdSede = jc.IdSede AND p.IdNivel = jc.IdNivel AND p.IdJornada = jc.IdJornada AND p.IdCurso = jc.IdCurso LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_NivelAcademico_Jornada AS nj WITH(nolock) ON p.IdEmpresa = nj.IdEmpresa AND p.IdAnio = nj.IdAnio AND p.IdSede = nj.IdSede AND p.IdNivel = nj.IdNivel AND p.IdJornada = nj.IdJornada LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo_Sede_NivelAcademico AS sn WITH(nolock) ON p.IdEmpresa = sn.IdEmpresa AND p.IdAnio = sn.IdAnio AND p.IdSede = sn.IdSede AND p.IdNivel = sn.IdNivel LEFT OUTER JOIN "
                    + " dbo.aca_AnioLectivo AS an WITH(nolock) ON p.IdEmpresa = an.IdEmpresa AND a.IdAnio = an.IdAnio "
                    + " WHERE a.IdEmpresa = " + IdEmpresa.ToString()
                    + " and a.IdSede = " + IdSede.ToString()
                    + " and a.IdAnio = " + IdAnio.ToString()
                    + " order by p.IdPreMatricula desc";
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 0;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new aca_Admision_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdPreMatricula = Convert.ToDecimal(reader["IdPreMatricula"]),
                            IdAdmision = Convert.ToDecimal(reader["IdAdmision"]),
                            IdSede = Convert.ToInt32(reader["IdSede"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            IdJornada = Convert.ToInt32(reader["IdJornada"]),
                            IdNivel = Convert.ToInt32(reader["IdNivel"]),
                            IdCurso = Convert.ToInt32(reader["IdCurso"]),
                            CedulaRuc_Aspirante = reader["CedulaRuc_Aspirante"].ToString(),
                            NombreCompleto_Aspirante = string.IsNullOrEmpty(reader["NombreCompleto_Aspirante"].ToString()) ? null : reader["NombreCompleto_Aspirante"].ToString(),
                            Descripcion = string.IsNullOrEmpty(reader["Descripcion"].ToString()) ? null : reader["Descripcion"].ToString(),
                            NomSede = string.IsNullOrEmpty(reader["NomSede"].ToString()) ? null : reader["NomSede"].ToString(),
                            NomJornada = string.IsNullOrEmpty(reader["NomJornada"].ToString()) ? null : reader["NomJornada"].ToString(),
                            NomNivel = string.IsNullOrEmpty(reader["NomNivel"].ToString()) ? null : reader["NomNivel"].ToString(),
                            NomCurso = string.IsNullOrEmpty(reader["NomCurso"].ToString()) ? null : reader["NomCurso"].ToString(),
                            NomParalelo = string.IsNullOrEmpty(reader["NomParalelo"].ToString()) ? null : reader["NomParalelo"].ToString(),
                            OrdenJornada = string.IsNullOrEmpty(reader["OrdenJornada"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenJornada"]),
                            OrdenNivel = string.IsNullOrEmpty(reader["OrdenNivel"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenNivel"]),
                            OrdenCurso = string.IsNullOrEmpty(reader["OrdenCurso"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenCurso"]),
                            OrdenParalelo = string.IsNullOrEmpty(reader["OrdenParalelo"].ToString()) ? 0 : Convert.ToInt32(reader["OrdenParalelo"]),
                            Estado = string.IsNullOrEmpty(reader["Estado"].ToString()) ? false : Convert.ToBoolean(reader["Estado"]),
                            EstadoAdmision = string.IsNullOrEmpty(reader["EstadoAdmision"].ToString()) ? null : reader["EstadoAdmision"].ToString(),
                            IdCatalogoESTADM = Convert.ToInt32(reader["IdCatalogoESTADM"]),
                            IdUsuarioRevision = string.IsNullOrEmpty(reader["IdUsuarioRevision"].ToString()) ? null : reader["IdUsuarioRevision"].ToString(),
                            FechaIngreso_Aspirante = Convert.ToDateTime(reader["FechaIngreso_Aspirante"].ToString()),
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
        public decimal getId(int IdEmpresa)
        {
            try
            {
                decimal ID = 1;

                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    var cont = Context.aca_Admision.Where(q => q.IdEmpresa == IdEmpresa).Count();
                    if (cont > 0)
                        ID = Context.aca_Admision.Where(q => q.IdEmpresa == IdEmpresa).Max(q => q.IdAdmision) + 1;
                }

                return ID;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool guardarDB(aca_Admision_Info info)
        {
            try
            {
                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    aca_Admision Entity = new aca_Admision
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdAdmision = info.IdAdmision = getId(info.IdEmpresa),
                        IdSede = info.IdSede,
                        IdAnio = info.IdAnio,
                        IdJornada = info.IdJornada,
                        IdNivel = info.IdNivel,
                        IdCurso = info.IdCurso,
                        Naturaleza_Aspirante = info.Naturaleza_Aspirante,
                        IdTipoDocumento_Aspirante = info.IdTipoDocumento_Aspirante,
                        CedulaRuc_Aspirante = info.CedulaRuc_Aspirante,
                        Nombres_Aspirante = info.Nombres_Aspirante,
                        Apellidos_Aspirante = info.Apellidos_Aspirante,
                        NombreCompleto_Aspirante = info.NombreCompleto_Aspirante,
                        Direccion_Aspirante = info.Direccion_Aspirante,
                        Telefono_Aspirante = info.Telefono_Aspirante,
                        Celular_Aspirante = info.Celular_Aspirante,
                        Correo_Aspirante = info.Correo_Aspirante,
                        Sexo_Aspirante = (info.Sexo_Aspirante== "" ? null : info.Sexo_Aspirante),
                        FechaNacimiento_Aspirante = info.FechaNacimiento_Aspirante??(DateTime?)null,
                        CodCatalogoSangre_Aspirante = (info.CodCatalogoSangre_Aspirante == "" ? null : info.CodCatalogoSangre_Aspirante),
                        CodCatalogoCONADIS_Aspirante = (info.CodCatalogoCONADIS_Aspirante == "" ? null : info.CodCatalogoCONADIS_Aspirante),
                        PorcentajeDiscapacidad_Aspirante = info.PorcentajeDiscapacidad_Aspirante,
                        NumeroCarnetConadis_Aspirante = info.NumeroCarnetConadis_Aspirante,
                        IdGrupoEtnico_Aspirante = (info.IdGrupoEtnico_Aspirante==0 ? (int?)null : info.IdGrupoEtnico_Aspirante),
                        IdReligion_Aspirante = (info.IdReligion_Aspirante == 0 ? (int?)null : info.IdReligion_Aspirante),
                        AsisteCentroCristiano_Aspirante = info.AsisteCentroCristiano_Aspirante,
                        LugarNacimiento_Aspirante = string.IsNullOrEmpty(info.LugarNacimiento_Aspirante)?null : info.LugarNacimiento_Aspirante,
                        IdPais_Aspirante = (info.IdPais_Aspirante == "" ? null : info.IdPais_Aspirante),
                        Cod_Region_Aspirante = (info.Cod_Region_Aspirante == "" ? null : info.Cod_Region_Aspirante),
                        IdProvincia_Aspirante = (info.IdProvincia_Aspirante == "" ? null : info.IdProvincia_Aspirante),
                        IdCiudad_Aspirante = (info.IdCiudad_Aspirante == "" ? null : info.IdCiudad_Aspirante),
                        IdParroquia_Aspirante = (info.IdParroquia_Aspirante == "" ? null : info.IdParroquia_Aspirante),
                        Sector_Aspirante = info.Sector_Aspirante,
                        FechaIngreso_Aspirante = DateTime.Now,
                        IdCatalogoFichaTipoViv_Aspirante = info.IdCatalogoFichaTipoViv_Aspirante,
                        IdCatalogoFichaViv_Aspirante = info.IdCatalogoFichaViv_Aspirante,
                        IdCatalogoFichaAgua_Aspirante = info.IdCatalogoFichaAgua_Aspirante,
                        TieneElectricidad_Aspirante = info.TieneElectricidad_Aspirante,
                        TieneHermanos_Aspirante = info.TieneHermanos_Aspirante,
                        CantidadHermanos = info.CantidadHermanos,
                        IdCatalogoFichaMotivo_Aspirante = info.IdCatalogoFichaMotivo_Aspirante,
                        IdCatalogoFichaInst_Aspirante = info.IdCatalogoFichaInst_Aspirante,
                        IdCatalogoFichaFinanc_Aspirante = info.IdCatalogoFichaFinanc_Aspirante,
                        IdCatalogoFichaVive_Aspirante = info.IdCatalogoFichaVive_Aspirante,
                        OtroMotivoIngreso_Aspirante = info.OtroMotivoIngreso_Aspirante,
                        OtroInformacionInst_Aspirante = info.OtroInformacionInst_Aspirante,
                        OtroFinanciamiento_Aspirante = info.OtroFinanciamiento_Aspirante,
                        Naturaleza_Padre = info.Naturaleza_Padre,
                        IdTipoDocumento_Padre = info.IdTipoDocumento_Padre,
                        CedulaRuc_Padre = info.CedulaRuc_Padre,
                        Nombres_Padre = info.Nombres_Padre,
                        Apellidos_Padre = info.Apellidos_Padre,
                        NombreCompleto_Padre = info.NombreCompleto_Padre,
                        RazonSocial_Padre = info.RazonSocial_Padre,
                        Direccion_Padre = info.Direccion_Padre,
                        Telefono_Padre = info.Telefono_Padre,
                        Celular_Padre = info.Celular_Padre,
                        Correo_Padre = info.Correo_Padre,
                        Sexo_Padre = (info.Sexo_Padre=="" ? null : info.Sexo_Padre),
                        FechaNacimiento_Padre = info.FechaNacimiento_Padre,
                        CodCatalogoCONADIS_Padre = (info.CodCatalogoCONADIS_Padre == "" ? null : info.CodCatalogoCONADIS_Padre),
                        PorcentajeDiscapacidad_Padre = info.PorcentajeDiscapacidad_Padre,
                        NumeroCarnetConadis_Padre = info.NumeroCarnetConadis_Padre,
                        IdGrupoEtnico_Padre = (info.IdGrupoEtnico_Padre == 0 ? (int?)null : info.IdGrupoEtnico_Padre),
                        IdReligion_Padre = (info.IdReligion_Padre == 0 ? (int?)null : info.IdReligion_Padre),
                        IdEstadoCivil_Padre = (info.IdEstadoCivil_Padre == "" ? null : info.IdEstadoCivil_Padre),
                        AsisteCentroCristiano_Padre = info.AsisteCentroCristiano_Padre,
                        IdPais_Padre = (info.IdPais_Padre == "" ? null : info.IdPais_Padre),
                        Cod_Region_Padre = (info.Cod_Region_Padre == "" ? null : info.Cod_Region_Padre),
                        IdProvincia_Padre = (info.IdProvincia_Padre == "" ? null : info.IdProvincia_Padre),
                        IdCiudad_Padre = (info.IdCiudad_Padre == "" ? null : info.IdCiudad_Padre),
                        IdParroquia_Padre = (info.IdParroquia_Padre == "" ? null : info.IdParroquia_Padre),
                        Sector_Padre = info.Sector_Padre,
                        IdCatalogoPAREN_Padre = info.IdCatalogoPAREN_Padre,
                        IdCatalogoFichaInst_Padre = (info.IdCatalogoFichaInst_Padre == 0 ? (int?)null : info.IdCatalogoFichaInst_Padre),
                        EmpresaTrabajo_Padre = info.EmpresaTrabajo_Padre,
                        IdProfesion_Padre = (info.IdProfesion_Padre == 0 ? (int?)null : info.IdProfesion_Padre),
                        DireccionTrabajo_Padre = info.DireccionTrabajo_Padre,
                        TelefonoTrabajo_Padre = info.TelefonoTrabajo_Padre,
                        CargoTrabajo_Padre = info.CargoTrabajo_Padre,
                        AniosServicio_Padre = info.AniosServicio_Padre,
                        IngresoMensual_Padre = info.IngresoMensual_Padre,
                        VehiculoPropio_Padre = info.VehiculoPropio_Padre,
                        Marca_Padre = info.Marca_Padre,
                        Modelo_Padre = info.Modelo_Padre,
                        AnioVehiculo_Padre = info.AnioVehiculo_Padre,
                        CasaPropia_Padre = info.CasaPropia_Padre,
                        EstaFallecido_Padre = info.EstaFallecido_Padre,
                        SeFactura_Padre = info.SeFactura_Padre,
                        Idtipo_cliente_Padre = info.Idtipo_cliente_Padre,
                        IdTipoCredito_Padre = info.IdTipoCredito_Padre,
                        IdCiudad_Padre_Fact = info.IdCiudad_Padre_Fact,
                        IdParroquia_Padre_Fact = info.IdParroquia_Padre_Fact,
                        Naturaleza_Madre = info.Naturaleza_Madre,
                        IdTipoDocumento_Madre = info.IdTipoDocumento_Madre,
                        CedulaRuc_Madre = info.CedulaRuc_Madre,
                        Nombres_Madre = info.Nombres_Madre,
                        Apellidos_Madre = info.Apellidos_Madre,
                        NombreCompleto_Madre = info.NombreCompleto_Madre,
                        RazonSocial_Madre = info.RazonSocial_Madre,
                        Direccion_Madre = info.Direccion_Madre,
                        Telefono_Madre = info.Telefono_Madre,
                        Celular_Madre = info.Celular_Madre,
                        Correo_Madre = info.Correo_Madre,
                        Sexo_Madre = (info.Sexo_Madre=="" ? null : info.Sexo_Madre),
                        FechaNacimiento_Madre = info.FechaNacimiento_Madre,
                        CodCatalogoCONADIS_Madre = (info.CodCatalogoCONADIS_Madre == "" ? null : info.CodCatalogoCONADIS_Madre),
                        PorcentajeDiscapacidad_Madre = info.PorcentajeDiscapacidad_Madre,
                        NumeroCarnetConadis_Madre = info.NumeroCarnetConadis_Madre,
                        IdGrupoEtnico_Madre = (info.IdGrupoEtnico_Madre == 0 ? (int?)null : info.IdGrupoEtnico_Madre),
                        IdReligion_Madre = (info.IdReligion_Madre == 0 ? (int?)null : info.IdReligion_Madre),
                        IdEstadoCivil_Madre = (info.IdEstadoCivil_Madre == "" ? null : info.IdEstadoCivil_Madre),
                        AsisteCentroCristiano_Madre = info.AsisteCentroCristiano_Madre,
                        IdPais_Madre = (info.IdPais_Madre == "" ? null : info.IdPais_Madre),
                        Cod_Region_Madre = (info.Cod_Region_Madre == "" ? null : info.Cod_Region_Madre),
                        IdProvincia_Madre = (info.IdProvincia_Madre == "" ? null : info.IdProvincia_Madre),
                        IdCiudad_Madre = (info.IdCiudad_Madre == "" ? null : info.IdCiudad_Madre),
                        IdParroquia_Madre = (info.IdParroquia_Madre == "" ? null : info.IdParroquia_Madre),
                        Sector_Madre = info.Sector_Madre,
                        IdCatalogoPAREN_Madre = info.IdCatalogoPAREN_Madre,
                        IdCatalogoFichaInst_Madre = (info.IdCatalogoFichaInst_Madre == 0 ? null : info.IdCatalogoFichaInst_Madre),
                        EmpresaTrabajo_Madre = info.EmpresaTrabajo_Madre,
                        IdProfesion_Madre = (info.IdProfesion_Madre == 0 ? (int?)null : info.IdProfesion_Madre),
                        DireccionTrabajo_Madre = info.DireccionTrabajo_Madre,
                        TelefonoTrabajo_Madre = info.TelefonoTrabajo_Madre,
                        CargoTrabajo_Madre = info.CargoTrabajo_Madre,
                        AniosServicio_Madre = info.AniosServicio_Madre,
                        IngresoMensual_Madre = info.IngresoMensual_Madre,
                        VehiculoPropio_Madre = info.VehiculoPropio_Madre,
                        Marca_Madre = info.Marca_Madre,
                        Modelo_Madre = info.Modelo_Madre,
                        AnioVehiculo_Madre = info.AnioVehiculo_Madre,
                        CasaPropia_Madre = info.CasaPropia_Madre,
                        EstaFallecido_Madre = info.EstaFallecido_Madre,
                        SeFactura_Madre = info.SeFactura_Madre,
                        Idtipo_cliente_Madre = info.Idtipo_cliente_Madre,
                        IdTipoCredito_Madre = info.IdTipoCredito_Madre,
                        IdCiudad_Madre_Fact = info.IdCiudad_Madre_Fact,
                        IdParroquia_Madre_Fact = info.IdParroquia_Madre_Fact,
                        Naturaleza_Representante = info.Naturaleza_Representante,
                        IdTipoDocumento_Representante = info.IdTipoDocumento_Representante,
                        CedulaRuc_Representante = info.CedulaRuc_Representante,
                        Nombres_Representante = info.Nombres_Representante,
                        Apellidos_Representante = info.Apellidos_Representante,
                        NombreCompleto_Representante = info.NombreCompleto_Representante,
                        RazonSocial_Representante = info.RazonSocial_Representante,
                        Direccion_Representante = info.Direccion_Representante,
                        Telefono_Representante = info.Telefono_Representante,
                        Celular_Representante = info.Celular_Representante,
                        Correo_Representante = info.Correo_Representante,
                        Sexo_Representante = (info.Sexo_Representante == "" ? null : info.Sexo_Representante),
                        FechaNacimiento_Representante = info.FechaNacimiento_Representante,
                        CodCatalogoCONADIS_Representante = (info.CodCatalogoCONADIS_Representante == "" ? null : info.CodCatalogoCONADIS_Representante),
                        PorcentajeDiscapacidad_Representante = info.PorcentajeDiscapacidad_Representante,
                        NumeroCarnetConadis_Representante = info.NumeroCarnetConadis_Representante,
                        IdGrupoEtnico_Representante = (info.IdGrupoEtnico_Representante == 0 ? (int?)null : info.IdGrupoEtnico_Representante),
                        IdReligion_Representante = (info.IdReligion_Representante == 0 ? (int?)null : info.IdReligion_Representante),
                        IdEstadoCivil_Representante = (info.IdEstadoCivil_Representante == "" ? null : info.IdEstadoCivil_Representante),
                        AsisteCentroCristiano_Representante = info.AsisteCentroCristiano_Representante,
                        IdPais_Representante = (info.IdPais_Representante == "" ? null : info.IdPais_Representante),
                        Cod_Region_Representante = (info.Cod_Region_Representante == "" ? null : info.Cod_Region_Representante),
                        IdProvincia_Representante = (info.IdProvincia_Representante == "" ? null : info.IdProvincia_Representante),
                        IdCiudad_Representante = (info.IdCiudad_Representante == "" ? null : info.IdCiudad_Representante),
                        IdParroquia_Representante = (info.IdParroquia_Representante == "" ? null : info.IdParroquia_Representante),
                        Sector_Representante = info.Sector_Representante,
                        IdCatalogoPAREN_Representante = info.IdCatalogoPAREN_Representante,
                        IdCatalogoFichaInst_Representante = (info.IdCatalogoFichaInst_Representante == 0 ? (int?)null : info.IdCatalogoFichaInst_Representante),
                        EmpresaTrabajo_Representante = info.EmpresaTrabajo_Representante,
                        IdProfesion_Representante = (info.IdProfesion_Representante == 0 ? (int?)null : info.IdProfesion_Representante),
                        DireccionTrabajo_Representante = info.DireccionTrabajo_Representante,
                        TelefonoTrabajo_Representante = info.TelefonoTrabajo_Representante,
                        CargoTrabajo_Representante = info.CargoTrabajo_Representante,
                        AniosServicio_Representante = info.AniosServicio_Representante,
                        IngresoMensual_Representante = info.IngresoMensual_Representante,
                        VehiculoPropio_Representante = info.VehiculoPropio_Representante,
                        Marca_Representante = info.Marca_Representante,
                        Modelo_Representante = info.Modelo_Representante,
                        AnioVehiculo_Representante = info.AnioVehiculo_Representante,
                        CasaPropia_Representante = info.CasaPropia_Representante,
                        EstaFallecido_Representante = info.EstaFallecido_Representante,
                        SeFactura_Representante = info.SeFactura_Representante,
                        Idtipo_cliente_Representante = info.Idtipo_cliente_Representante,
                        IdTipoCredito_Representante = info.IdTipoCredito_Representante,
                        IdCiudad_Representante_Fact = info.IdCiudad_Representante_Fact,
                        IdParroquia_Representante_Fact = info.IdParroquia_Representante_Fact,
                        SueldoPadre = info.SueldoPadre,
                        SueldoMadre = info.SueldoMadre,
                        OtroIngresoPadre = info.OtroIngresoPadre,
                        OtroIngresoMadre = info.OtroIngresoMadre,
                        GastoAlimentacion = info.GastoAlimentacion,
                        GastoEducacion = info.GastoEducacion,
                        GastoServicioBasico = info.GastoServicioBasico,
                        GastoSalud = info.GastoSalud,
                        GastoArriendo = info.GastoArriendo,
                        GastoPrestamo = info.GastoPrestamo,
                        OtroGasto = info.OtroGasto,
                        IdCatalogoESTADM = info.IdCatalogoESTADM,
                        AceptaTerminos = info.AceptaTerminos,
                        Dificultad_Escritura = info.Dificultad_Escritura,
                        Dificultad_Lectura=info.Dificultad_Lectura,
                        Dificultad_Matematicas=info.Dificultad_Matematicas,
                        Estado = true,
                        FechaCreacion = info.FechaCreacion = DateTime.Now
                    };
                    Context.aca_Admision.Add(Entity);

                    var info_catalogo = odata_catalogo.getInfo(Convert.ToInt32(info.IdCatalogoESTADM));
                    var Destinatarios = (info == null ? "" : (info.SeFactura_Padre == true ? info.Correo_Padre : (info.SeFactura_Madre ? info.Correo_Madre : info.Correo_Representante)) + ";" + info.Correo_Padre + ";" + info.Correo_Madre + ";" + info.Correo_Representante);
                    var info_correo = new tb_ColaCorreo_Info
                    {
                        IdEmpresa = info.IdEmpresa,
                        Destinatarios = Destinatarios,
                        Asunto = "REGISTRO DE ADMISION",
                        Parametros = "",
                        Codigo="",
                        IdUsuarioCreacion = "",
                        Cuerpo = (info_catalogo==null ? "" : info_catalogo.NomCatalogo),
                        FechaCreacion = DateTime.Now
                    };
                    odata_correo.GuardarDB(info_correo);

                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception EX)
            {

                throw;
            }
        }
        public aca_Admision_Info getInfo_CedulaAspirante(int IdEmpresa, string CedulaRuc_Aspirante)
        {
            try
            {
                aca_Admision_Info info = new aca_Admision_Info();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("", connection);
                    command.CommandText = "SELECT * FROM aca_Admision a WITH (nolock) "
                    + " WHERE a.IdEmpresa = " + IdEmpresa.ToString() + " and a.CedulaRuc_Aspirante = '" + CedulaRuc_Aspirante.ToString()+"'";
                    var ResultValue = command.ExecuteScalar();

                    if (ResultValue == null)
                        return null;

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        info = new aca_Admision_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdAdmision = Convert.ToDecimal(reader["IdAdmision"]),
                            CedulaRuc_Aspirante = reader["CedulaRuc_Aspirante"].ToString(),
                            Nombres_Aspirante = reader["Nombres_Aspirante"].ToString(),
                            Apellidos_Aspirante = reader["Apellidos_Aspirante"].ToString(),
                            NombreCompleto_Aspirante = reader["NombreCompleto_Aspirante"].ToString(),
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
        public aca_Admision_Info consultaAdmision(int IdEmpresa, int IdAnio, string CedulaRuc_Aspirante)
        {
            try
            {
                aca_Admision_Info info = new aca_Admision_Info();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("", connection);
                    command.CommandText = "SELECT a.IdEmpresa, a.IdAdmision, a.NombreCompleto_Aspirante, a.CedulaRuc_Aspirante, a.FechaIngreso_Aspirante,b.Codigo CodigoEstadoAdmision, b.NomCatalogo EstadoAdmision "
                    + " FROM aca_Admision a WITH (nolock) "
                    + " left join aca_Catalogo b WITH (nolock) on a.IdCatalogoESTADM=b.IdCatalogo"
                    + " WHERE a.IdEmpresa = " + IdEmpresa.ToString() + " and a.CedulaRuc_Aspirante = '" + CedulaRuc_Aspirante.ToString()+"'"
                    + " AND a.IdAnio = "+IdAnio;
                    var ResultValue = command.ExecuteScalar();

                    if (ResultValue == null)
                        return null;

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        info = new aca_Admision_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdAdmision = Convert.ToDecimal(reader["IdAdmision"]),
                            CedulaRuc_Aspirante = reader["CedulaRuc_Aspirante"].ToString(),
                            CodigoEstadoAdmision = reader["CodigoEstadoAdmision"].ToString(),
                            NombreCompleto_Aspirante = reader["NombreCompleto_Aspirante"].ToString(),
                            FechaIngreso_Aspirante = Convert.ToDateTime(reader["FechaIngreso_Aspirante"]),
                            EstadoAdmision = reader["EstadoAdmision"].ToString(),
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

        public aca_Admision_Info getInfo(int IdEmpresa, decimal IdAdmision)
        {
            try
            {
                aca_Admision_Info info = new aca_Admision_Info();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("", connection);
                    command.CommandText = "SELECT * FROM aca_Admision a WITH (nolock) "
                    + " WHERE a.IdEmpresa = " + IdEmpresa.ToString() + " and a.IdAdmision = '" + IdAdmision.ToString() + "'";
                    var ResultValue = command.ExecuteScalar();

                    if (ResultValue == null)
                        return null;

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        info = new aca_Admision_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdAdmision = Convert.ToDecimal(reader["IdAdmision"]),
                            IdSede = Convert.ToInt32(reader["IdSede"]),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            IdJornada = Convert.ToInt32(reader["IdJornada"]),
                            IdNivel = Convert.ToInt32(reader["IdNivel"]),
                            IdCurso = Convert.ToInt32(reader["IdCurso"]),
                            Naturaleza_Aspirante = string.IsNullOrEmpty(reader["Naturaleza_Aspirante"].ToString()) ? null : reader["Naturaleza_Aspirante"].ToString(),
                            IdTipoDocumento_Aspirante = string.IsNullOrEmpty(reader["IdTipoDocumento_Aspirante"].ToString()) ? null : reader["IdTipoDocumento_Aspirante"].ToString(),
                            CedulaRuc_Aspirante = reader["CedulaRuc_Aspirante"].ToString(),
                            Nombres_Aspirante = reader["Nombres_Aspirante"].ToString(),
                            Apellidos_Aspirante = reader["Apellidos_Aspirante"].ToString(),
                            NombreCompleto_Aspirante = reader["NombreCompleto_Aspirante"].ToString(),
                            Direccion_Aspirante = string.IsNullOrEmpty(reader["Direccion_Aspirante"].ToString()) ? null : reader["Direccion_Aspirante"].ToString(),
                            Telefono_Aspirante = string.IsNullOrEmpty(reader["Telefono_Aspirante"].ToString()) ? null : reader["Telefono_Aspirante"].ToString(),
                            Celular_Aspirante = string.IsNullOrEmpty(reader["Celular_Aspirante"].ToString()) ? null : reader["Celular_Aspirante"].ToString(),
                            Correo_Aspirante = string.IsNullOrEmpty(reader["Correo_Aspirante"].ToString()) ? null : reader["Correo_Aspirante"].ToString(),
                            Sexo_Aspirante = string.IsNullOrEmpty(reader["Sexo_Aspirante"].ToString()) ? null : reader["Sexo_Aspirante"].ToString(),
                            FechaNacimiento_Aspirante = string.IsNullOrEmpty(reader["FechaNacimiento_Aspirante"].ToString()) ? (DateTime?)null : Convert.ToDateTime(reader["FechaNacimiento_Aspirante"]),
                            CodCatalogoSangre_Aspirante = string.IsNullOrEmpty(reader["CodCatalogoSangre_Aspirante"].ToString()) ? null : reader["CodCatalogoSangre_Aspirante"].ToString(),
                            CodCatalogoCONADIS_Aspirante = string.IsNullOrEmpty(reader["CodCatalogoCONADIS_Aspirante"].ToString()) ? null : reader["CodCatalogoCONADIS_Aspirante"].ToString(),
                            PorcentajeDiscapacidad_Aspirante = string.IsNullOrEmpty(reader["PorcentajeDiscapacidad_Aspirante"].ToString()) ? (double?)null : Convert.ToDouble(reader["PorcentajeDiscapacidad_Aspirante"]),
                            NumeroCarnetConadis_Aspirante = string.IsNullOrEmpty(reader["NumeroCarnetConadis_Aspirante"].ToString()) ? null : reader["NumeroCarnetConadis_Aspirante"].ToString(),
                            IdGrupoEtnico_Aspirante = string.IsNullOrEmpty(reader["IdGrupoEtnico_Aspirante"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdGrupoEtnico_Aspirante"]),
                            IdReligion_Aspirante = string.IsNullOrEmpty(reader["IdReligion_Aspirante"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdReligion_Aspirante"]),
                            AsisteCentroCristiano_Aspirante = string.IsNullOrEmpty(reader["AsisteCentroCristiano_Aspirante"].ToString()) ? false : Convert.ToBoolean(reader["AsisteCentroCristiano_Aspirante"]),
                            LugarNacimiento_Aspirante = string.IsNullOrEmpty(reader["LugarNacimiento_Aspirante"].ToString()) ? null : reader["LugarNacimiento_Aspirante"].ToString(),
                            IdPais_Aspirante = string.IsNullOrEmpty(reader["IdPais_Aspirante"].ToString()) ? null : reader["IdPais_Aspirante"].ToString(),
                            Cod_Region_Aspirante = string.IsNullOrEmpty(reader["Cod_Region_Aspirante"].ToString()) ? null : reader["Cod_Region_Aspirante"].ToString(),
                            IdProvincia_Aspirante = string.IsNullOrEmpty(reader["IdProvincia_Aspirante"].ToString()) ? null : reader["IdProvincia_Aspirante"].ToString(),
                            IdCiudad_Aspirante = string.IsNullOrEmpty(reader["IdCiudad_Aspirante"].ToString()) ? null : reader["IdCiudad_Aspirante"].ToString(),
                            IdParroquia_Aspirante = string.IsNullOrEmpty(reader["IdParroquia_Aspirante"].ToString()) ? null : reader["IdParroquia_Aspirante"].ToString(),
                            Sector_Aspirante = string.IsNullOrEmpty(reader["Sector_Aspirante"].ToString()) ? null : reader["Sector_Aspirante"].ToString(),
                            FechaIngreso_Aspirante = Convert.ToDateTime(reader["FechaIngreso_Aspirante"]),
                            Dificultad_Lectura = string.IsNullOrEmpty(reader["Dificultad_Lectura"].ToString()) ? false : Convert.ToBoolean(reader["Dificultad_Lectura"]),
                            Dificultad_Escritura = string.IsNullOrEmpty(reader["Dificultad_Escritura"].ToString()) ? false : Convert.ToBoolean(reader["Dificultad_Escritura"]),
                            Dificultad_Matematicas = string.IsNullOrEmpty(reader["Dificultad_Matematicas"].ToString()) ? false : Convert.ToBoolean(reader["Dificultad_Matematicas"]),
                            AceptaTerminos = string.IsNullOrEmpty(reader["AceptaTerminos"].ToString()) ? false : Convert.ToBoolean(reader["AceptaTerminos"]),
                            IdCatalogoFichaTipoViv_Aspirante = Convert.ToInt32(reader["IdCatalogoFichaTipoViv_Aspirante"]),
                            IdCatalogoFichaViv_Aspirante = Convert.ToInt32(reader["IdCatalogoFichaViv_Aspirante"]),
                            IdCatalogoFichaAgua_Aspirante = Convert.ToInt32(reader["IdCatalogoFichaAgua_Aspirante"]),
                            TieneElectricidad_Aspirante = string.IsNullOrEmpty(reader["TieneElectricidad_Aspirante"].ToString()) ? false : Convert.ToBoolean(reader["TieneElectricidad_Aspirante"]),
                            TieneHermanos_Aspirante = string.IsNullOrEmpty(reader["TieneHermanos_Aspirante"].ToString()) ? false : Convert.ToBoolean(reader["TieneHermanos_Aspirante"]),
                            CantidadHermanos = string.IsNullOrEmpty(reader["CantidadHermanos"].ToString()) ? (int?)null : Convert.ToInt32(reader["CantidadHermanos"]),
                            IdCatalogoFichaMotivo_Aspirante = Convert.ToInt32(reader["IdCatalogoFichaMotivo_Aspirante"]),
                            IdCatalogoFichaInst_Aspirante = Convert.ToInt32(reader["IdCatalogoFichaInst_Aspirante"]),
                            IdCatalogoFichaFinanc_Aspirante = Convert.ToInt32(reader["IdCatalogoFichaFinanc_Aspirante"]),
                            IdCatalogoFichaVive_Aspirante = Convert.ToInt32(reader["IdCatalogoFichaVive_Aspirante"]),
                            OtroMotivoIngreso_Aspirante = string.IsNullOrEmpty(reader["OtroMotivoIngreso_Aspirante"].ToString()) ? null : reader["OtroMotivoIngreso_Aspirante"].ToString(),
                            OtroInformacionInst_Aspirante = string.IsNullOrEmpty(reader["OtroInformacionInst_Aspirante"].ToString()) ? null : reader["OtroInformacionInst_Aspirante"].ToString(),
                            OtroFinanciamiento_Aspirante = string.IsNullOrEmpty(reader["OtroFinanciamiento_Aspirante"].ToString()) ? null : reader["OtroFinanciamiento_Aspirante"].ToString(),
                            Naturaleza_Padre = string.IsNullOrEmpty(reader["Naturaleza_Padre"].ToString()) ? null : reader["Naturaleza_Padre"].ToString(),
                            IdTipoDocumento_Padre = string.IsNullOrEmpty(reader["IdTipoDocumento_Padre"].ToString()) ? null : reader["IdTipoDocumento_Padre"].ToString(),
                            CedulaRuc_Padre = string.IsNullOrEmpty(reader["CedulaRuc_Padre"].ToString()) ? null : reader["CedulaRuc_Padre"].ToString(),
                            Nombres_Padre = string.IsNullOrEmpty(reader["Nombres_Padre"].ToString()) ? null : reader["Nombres_Padre"].ToString(),
                            Apellidos_Padre = string.IsNullOrEmpty(reader["Apellidos_Padre"].ToString()) ? null : reader["Apellidos_Padre"].ToString(),
                            NombreCompleto_Padre = string.IsNullOrEmpty(reader["NombreCompleto_Padre"].ToString()) ? null : reader["NombreCompleto_Padre"].ToString(),
                            RazonSocial_Padre = string.IsNullOrEmpty(reader["RazonSocial_Padre"].ToString()) ? null : reader["RazonSocial_Padre"].ToString(),
                            Direccion_Padre = string.IsNullOrEmpty(reader["Direccion_Padre"].ToString()) ? null : reader["Direccion_Padre"].ToString(),
                            Telefono_Padre = string.IsNullOrEmpty(reader["Telefono_Padre"].ToString()) ? null : reader["Telefono_Padre"].ToString(),
                            Celular_Padre = string.IsNullOrEmpty(reader["Celular_Padre"].ToString()) ? null : reader["Celular_Padre"].ToString(),
                            Correo_Padre = string.IsNullOrEmpty(reader["Correo_Padre"].ToString()) ? null : reader["Correo_Padre"].ToString(),
                            Sexo_Padre = string.IsNullOrEmpty(reader["Sexo_Padre"].ToString()) ? null : reader["Sexo_Padre"].ToString(),
                            FechaNacimiento_Padre = string.IsNullOrEmpty(reader["FechaNacimiento_Padre"].ToString()) ? (DateTime?)null : Convert.ToDateTime(reader["FechaNacimiento_Padre"]),
                            CodCatalogoCONADIS_Padre = string.IsNullOrEmpty(reader["CodCatalogoCONADIS_Padre"].ToString()) ? null : reader["CodCatalogoCONADIS_Padre"].ToString(),
                            PorcentajeDiscapacidad_Padre = string.IsNullOrEmpty(reader["PorcentajeDiscapacidad_Padre"].ToString()) ? (double?)null : Convert.ToDouble(reader["PorcentajeDiscapacidad_Padre"]),
                            NumeroCarnetConadis_Padre = string.IsNullOrEmpty(reader["NumeroCarnetConadis_Padre"].ToString()) ? null : reader["NumeroCarnetConadis_Padre"].ToString(),
                            IdGrupoEtnico_Padre = string.IsNullOrEmpty(reader["IdGrupoEtnico_Padre"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdGrupoEtnico_Padre"]),
                            IdReligion_Padre = string.IsNullOrEmpty(reader["IdReligion_Padre"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdReligion_Padre"]),
                            IdEstadoCivil_Padre = string.IsNullOrEmpty(reader["IdEstadoCivil_Padre"].ToString()) ? null : reader["IdEstadoCivil_Padre"].ToString(),
                            AsisteCentroCristiano_Padre = string.IsNullOrEmpty(reader["AsisteCentroCristiano_Padre"].ToString()) ? false : Convert.ToBoolean(reader["AsisteCentroCristiano_Padre"]),
                            IdPais_Padre = string.IsNullOrEmpty(reader["IdPais_Padre"].ToString()) ? null : reader["IdPais_Padre"].ToString(),
                            Cod_Region_Padre = string.IsNullOrEmpty(reader["Cod_Region_Padre"].ToString()) ? null : reader["Cod_Region_Padre"].ToString(),
                            IdProvincia_Padre = string.IsNullOrEmpty(reader["IdProvincia_Padre"].ToString()) ? null : reader["IdProvincia_Padre"].ToString(),
                            IdCiudad_Padre = string.IsNullOrEmpty(reader["IdCiudad_Padre"].ToString()) ? null : reader["IdCiudad_Padre"].ToString(),
                            IdParroquia_Padre = string.IsNullOrEmpty(reader["IdParroquia_Padre"].ToString()) ? null : reader["IdParroquia_Padre"].ToString(),
                            Sector_Padre = string.IsNullOrEmpty(reader["Sector_Padre"].ToString()) ? null : reader["Sector_Padre"].ToString(),
                            IdCatalogoPAREN_Padre = Convert.ToInt32(reader["IdCatalogoPAREN_Padre"]),
                            IdCatalogoFichaInst_Padre = string.IsNullOrEmpty(reader["IdCatalogoFichaInst_Padre"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdCatalogoFichaInst_Padre"]),
                            EmpresaTrabajo_Padre = string.IsNullOrEmpty(reader["EmpresaTrabajo_Padre"].ToString()) ? null : reader["EmpresaTrabajo_Padre"].ToString(),
                            DireccionTrabajo_Padre = string.IsNullOrEmpty(reader["DireccionTrabajo_Padre"].ToString()) ? null : reader["DireccionTrabajo_Padre"].ToString(),
                            TelefonoTrabajo_Padre = string.IsNullOrEmpty(reader["TelefonoTrabajo_Padre"].ToString()) ? null : reader["TelefonoTrabajo_Padre"].ToString(),
                            CargoTrabajo_Padre = string.IsNullOrEmpty(reader["CargoTrabajo_Padre"].ToString()) ? null : reader["CargoTrabajo_Padre"].ToString(),
                            AniosServicio_Padre = string.IsNullOrEmpty(reader["AniosServicio_Padre"].ToString()) ? (int?)null : Convert.ToInt32(reader["AniosServicio_Padre"]),
                            IngresoMensual_Padre = string.IsNullOrEmpty(reader["IngresoMensual_Padre"].ToString()) ? (double?)null : Convert.ToDouble(reader["IngresoMensual_Padre"]),
                            VehiculoPropio_Padre = string.IsNullOrEmpty(reader["VehiculoPropio_Padre"].ToString()) ? false : Convert.ToBoolean(reader["VehiculoPropio_Padre"]),
                            Marca_Padre = string.IsNullOrEmpty(reader["Marca_Padre"].ToString()) ? null : reader["Marca_Padre"].ToString(),
                            Modelo_Padre = string.IsNullOrEmpty(reader["Modelo_Padre"].ToString()) ? null : reader["Modelo_Padre"].ToString(),
                            AnioVehiculo_Padre = string.IsNullOrEmpty(reader["AnioVehiculo_Padre"].ToString()) ? (int?)null : Convert.ToInt32(reader["AnioVehiculo_Padre"]),
                            CasaPropia_Padre = string.IsNullOrEmpty(reader["CasaPropia_Padre"].ToString()) ? false : Convert.ToBoolean(reader["CasaPropia_Padre"]),
                            EstaFallecido_Padre = string.IsNullOrEmpty(reader["EstaFallecido_Padre"].ToString()) ? false : Convert.ToBoolean(reader["EstaFallecido_Padre"]),
                            IdProfesion_Padre = string.IsNullOrEmpty(reader["IdProfesion_Padre"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdProfesion_Padre"]),
                            SeFactura_Padre = string.IsNullOrEmpty(reader["SeFactura_Padre"].ToString()) ? false : Convert.ToBoolean(reader["SeFactura_Padre"]),
                            IdCiudad_Padre_Fact = string.IsNullOrEmpty(reader["IdCiudad_Padre_Fact"].ToString()) ? null : reader["IdCiudad_Padre_Fact"].ToString(),
                            IdTipoCredito_Padre = string.IsNullOrEmpty(reader["IdTipoCredito_Padre"].ToString()) ? null : reader["IdTipoCredito_Padre"].ToString(),
                            IdParroquia_Padre_Fact = string.IsNullOrEmpty(reader["IdParroquia_Padre_Fact"].ToString()) ? null : reader["IdParroquia_Padre_Fact"].ToString(),
                            Idtipo_cliente_Padre = string.IsNullOrEmpty(reader["Idtipo_cliente_Padre"].ToString()) ? (int?)null : Convert.ToInt32(reader["Idtipo_cliente_Padre"]),     
                            Naturaleza_Madre = string.IsNullOrEmpty(reader["Naturaleza_Madre"].ToString()) ? null : reader["Naturaleza_Madre"].ToString(),
                            IdTipoDocumento_Madre = string.IsNullOrEmpty(reader["IdTipoDocumento_Madre"].ToString()) ? null : reader["IdTipoDocumento_Madre"].ToString(),
                            CedulaRuc_Madre = string.IsNullOrEmpty(reader["CedulaRuc_Madre"].ToString()) ? null : reader["CedulaRuc_Madre"].ToString(),
                            Nombres_Madre = string.IsNullOrEmpty(reader["Nombres_Madre"].ToString()) ? null : reader["Nombres_Madre"].ToString(),
                            Apellidos_Madre = string.IsNullOrEmpty(reader["Apellidos_Madre"].ToString()) ? null : reader["Apellidos_Madre"].ToString(),
                            NombreCompleto_Madre = string.IsNullOrEmpty(reader["NombreCompleto_Madre"].ToString()) ? null : reader["NombreCompleto_Madre"].ToString(),
                            RazonSocial_Madre = string.IsNullOrEmpty(reader["RazonSocial_Madre"].ToString()) ? null : reader["RazonSocial_Madre"].ToString(),
                            Direccion_Madre = string.IsNullOrEmpty(reader["Direccion_Madre"].ToString()) ? null : reader["Direccion_Madre"].ToString(),
                            Telefono_Madre = string.IsNullOrEmpty(reader["Telefono_Madre"].ToString()) ? null : reader["Telefono_Madre"].ToString(),
                            Celular_Madre = string.IsNullOrEmpty(reader["Celular_Madre"].ToString()) ? null : reader["Celular_Madre"].ToString(),
                            Correo_Madre = string.IsNullOrEmpty(reader["Correo_Madre"].ToString()) ? null : reader["Correo_Madre"].ToString(),
                            Sexo_Madre = string.IsNullOrEmpty(reader["Sexo_Madre"].ToString()) ? null : reader["Sexo_Madre"].ToString(),
                            FechaNacimiento_Madre = string.IsNullOrEmpty(reader["FechaNacimiento_Madre"].ToString()) ? (DateTime?)null : Convert.ToDateTime(reader["FechaNacimiento_Madre"]),
                            CodCatalogoCONADIS_Madre = string.IsNullOrEmpty(reader["CodCatalogoCONADIS_Madre"].ToString()) ? null : reader["CodCatalogoCONADIS_Madre"].ToString(),
                            PorcentajeDiscapacidad_Madre = string.IsNullOrEmpty(reader["PorcentajeDiscapacidad_Madre"].ToString()) ? (double?)null : Convert.ToDouble(reader["PorcentajeDiscapacidad_Madre"]),
                            NumeroCarnetConadis_Madre = string.IsNullOrEmpty(reader["NumeroCarnetConadis_Madre"].ToString()) ? null : reader["NumeroCarnetConadis_Madre"].ToString(),
                            IdGrupoEtnico_Madre = string.IsNullOrEmpty(reader["IdGrupoEtnico_Madre"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdGrupoEtnico_Madre"]),
                            IdReligion_Madre = string.IsNullOrEmpty(reader["IdReligion_Madre"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdReligion_Madre"]),
                            IdEstadoCivil_Madre = string.IsNullOrEmpty(reader["IdEstadoCivil_Madre"].ToString()) ? null : reader["IdEstadoCivil_Madre"].ToString(),
                            AsisteCentroCristiano_Madre = string.IsNullOrEmpty(reader["AsisteCentroCristiano_Madre"].ToString()) ? false : Convert.ToBoolean(reader["AsisteCentroCristiano_Madre"]),
                            IdPais_Madre = string.IsNullOrEmpty(reader["IdPais_Madre"].ToString()) ? null : reader["IdPais_Madre"].ToString(),
                            Cod_Region_Madre = string.IsNullOrEmpty(reader["Cod_Region_Madre"].ToString()) ? null : reader["Cod_Region_Madre"].ToString(),
                            IdProvincia_Madre = string.IsNullOrEmpty(reader["IdProvincia_Madre"].ToString()) ? null : reader["IdProvincia_Madre"].ToString(),
                            IdCiudad_Madre = string.IsNullOrEmpty(reader["IdCiudad_Madre"].ToString()) ? null : reader["IdCiudad_Madre"].ToString(),
                            IdParroquia_Madre = string.IsNullOrEmpty(reader["IdParroquia_Madre"].ToString()) ? null : reader["IdParroquia_Madre"].ToString(),
                            Sector_Madre = string.IsNullOrEmpty(reader["Sector_Madre"].ToString()) ? null : reader["Sector_Madre"].ToString(),
                            IdCatalogoPAREN_Madre = Convert.ToInt32(reader["IdCatalogoPAREN_Madre"]),
                            IdCatalogoFichaInst_Madre = string.IsNullOrEmpty(reader["IdCatalogoFichaInst_Madre"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdCatalogoFichaInst_Madre"]),
                            EmpresaTrabajo_Madre = string.IsNullOrEmpty(reader["EmpresaTrabajo_Madre"].ToString()) ? null : reader["EmpresaTrabajo_Madre"].ToString(),
                            DireccionTrabajo_Madre = string.IsNullOrEmpty(reader["DireccionTrabajo_Madre"].ToString()) ? null : reader["DireccionTrabajo_Madre"].ToString(),
                            TelefonoTrabajo_Madre = string.IsNullOrEmpty(reader["TelefonoTrabajo_Madre"].ToString()) ? null : reader["TelefonoTrabajo_Madre"].ToString(),
                            CargoTrabajo_Madre = string.IsNullOrEmpty(reader["CargoTrabajo_Madre"].ToString()) ? null : reader["CargoTrabajo_Madre"].ToString(),
                            AniosServicio_Madre = string.IsNullOrEmpty(reader["AniosServicio_Madre"].ToString()) ? (int?)null : Convert.ToInt32(reader["AniosServicio_Madre"]),
                            IngresoMensual_Madre = string.IsNullOrEmpty(reader["IngresoMensual_Madre"].ToString()) ? (double?)null : Convert.ToDouble(reader["IngresoMensual_Madre"]),
                            VehiculoPropio_Madre = string.IsNullOrEmpty(reader["VehiculoPropio_Madre"].ToString()) ? false : Convert.ToBoolean(reader["VehiculoPropio_Madre"]),
                            Marca_Madre = string.IsNullOrEmpty(reader["Marca_Madre"].ToString()) ? null : reader["Marca_Madre"].ToString(),
                            Modelo_Madre = string.IsNullOrEmpty(reader["Modelo_Madre"].ToString()) ? null : reader["Modelo_Madre"].ToString(),
                            AnioVehiculo_Madre = string.IsNullOrEmpty(reader["AnioVehiculo_Madre"].ToString()) ? (int?)null : Convert.ToInt32(reader["AnioVehiculo_Madre"]),
                            CasaPropia_Madre = string.IsNullOrEmpty(reader["CasaPropia_Madre"].ToString()) ? false : Convert.ToBoolean(reader["CasaPropia_Madre"]),
                            EstaFallecido_Madre = string.IsNullOrEmpty(reader["EstaFallecido_Madre"].ToString()) ? false : Convert.ToBoolean(reader["EstaFallecido_Madre"]),
                            IdProfesion_Madre = string.IsNullOrEmpty(reader["IdProfesion_Madre"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdProfesion_Madre"]),
                            SeFactura_Madre = string.IsNullOrEmpty(reader["SeFactura_Madre"].ToString()) ? false : Convert.ToBoolean(reader["SeFactura_Madre"]),
                            IdCiudad_Madre_Fact = string.IsNullOrEmpty(reader["IdCiudad_Madre_Fact"].ToString()) ? null : reader["IdCiudad_Madre_Fact"].ToString(),
                            IdTipoCredito_Madre = string.IsNullOrEmpty(reader["IdTipoCredito_Madre"].ToString()) ? null : reader["IdTipoCredito_Madre"].ToString(),
                            IdParroquia_Madre_Fact = string.IsNullOrEmpty(reader["IdParroquia_Madre_Fact"].ToString()) ? null : reader["IdParroquia_Madre_Fact"].ToString(),
                            Idtipo_cliente_Madre = string.IsNullOrEmpty(reader["Idtipo_cliente_Madre"].ToString()) ? (int?)null : Convert.ToInt32(reader["Idtipo_cliente_Madre"]),
                            Naturaleza_Representante = string.IsNullOrEmpty(reader["Naturaleza_Representante"].ToString()) ? null : reader["Naturaleza_Representante"].ToString(),
                            IdTipoDocumento_Representante = string.IsNullOrEmpty(reader["IdTipoDocumento_Representante"].ToString()) ? null : reader["IdTipoDocumento_Representante"].ToString(),
                            CedulaRuc_Representante = string.IsNullOrEmpty(reader["CedulaRuc_Representante"].ToString()) ? null : reader["CedulaRuc_Representante"].ToString(),
                            Nombres_Representante = string.IsNullOrEmpty(reader["Nombres_Representante"].ToString()) ? null : reader["Nombres_Representante"].ToString(),
                            Apellidos_Representante = string.IsNullOrEmpty(reader["Apellidos_Representante"].ToString()) ? null : reader["Apellidos_Representante"].ToString(),
                            NombreCompleto_Representante = string.IsNullOrEmpty(reader["NombreCompleto_Representante"].ToString()) ? null : reader["NombreCompleto_Representante"].ToString(),
                            RazonSocial_Representante = string.IsNullOrEmpty(reader["RazonSocial_Representante"].ToString()) ? null : reader["RazonSocial_Representante"].ToString(),
                            Direccion_Representante = string.IsNullOrEmpty(reader["Direccion_Representante"].ToString()) ? null : reader["Direccion_Representante"].ToString(),
                            Telefono_Representante = string.IsNullOrEmpty(reader["Telefono_Representante"].ToString()) ? null : reader["Telefono_Representante"].ToString(),
                            Celular_Representante = string.IsNullOrEmpty(reader["Celular_Representante"].ToString()) ? null : reader["Celular_Representante"].ToString(),
                            Correo_Representante = string.IsNullOrEmpty(reader["Correo_Representante"].ToString()) ? null : reader["Correo_Representante"].ToString(),
                            Sexo_Representante = string.IsNullOrEmpty(reader["Sexo_Representante"].ToString()) ? null : reader["Sexo_Representante"].ToString(),
                            FechaNacimiento_Representante = string.IsNullOrEmpty(reader["FechaNacimiento_Representante"].ToString()) ? (DateTime?)null : Convert.ToDateTime(reader["FechaNacimiento_Representante"]),
                            CodCatalogoCONADIS_Representante = string.IsNullOrEmpty(reader["CodCatalogoCONADIS_Representante"].ToString()) ? null : reader["CodCatalogoCONADIS_Representante"].ToString(),
                            PorcentajeDiscapacidad_Representante = string.IsNullOrEmpty(reader["PorcentajeDiscapacidad_Representante"].ToString()) ? (double?)null : Convert.ToDouble(reader["PorcentajeDiscapacidad_Representante"]),
                            NumeroCarnetConadis_Representante = string.IsNullOrEmpty(reader["NumeroCarnetConadis_Representante"].ToString()) ? null : reader["NumeroCarnetConadis_Representante"].ToString(),
                            IdGrupoEtnico_Representante = string.IsNullOrEmpty(reader["IdGrupoEtnico_Representante"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdGrupoEtnico_Representante"]),
                            IdReligion_Representante = string.IsNullOrEmpty(reader["IdReligion_Representante"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdReligion_Representante"]),
                            IdEstadoCivil_Representante = string.IsNullOrEmpty(reader["IdEstadoCivil_Representante"].ToString()) ? null : reader["IdEstadoCivil_Representante"].ToString(),
                            AsisteCentroCristiano_Representante = string.IsNullOrEmpty(reader["AsisteCentroCristiano_Representante"].ToString()) ? false : Convert.ToBoolean(reader["AsisteCentroCristiano_Representante"]),
                            IdPais_Representante = string.IsNullOrEmpty(reader["IdPais_Representante"].ToString()) ? null : reader["IdPais_Representante"].ToString(),
                            Cod_Region_Representante = string.IsNullOrEmpty(reader["Cod_Region_Representante"].ToString()) ? null : reader["Cod_Region_Representante"].ToString(),
                            IdProvincia_Representante = string.IsNullOrEmpty(reader["IdProvincia_Representante"].ToString()) ? null : reader["IdProvincia_Representante"].ToString(),
                            IdCiudad_Representante = string.IsNullOrEmpty(reader["IdCiudad_Representante"].ToString()) ? null : reader["IdCiudad_Representante"].ToString(),
                            IdParroquia_Representante = string.IsNullOrEmpty(reader["IdParroquia_Representante"].ToString()) ? null : reader["IdParroquia_Representante"].ToString(),
                            Sector_Representante = string.IsNullOrEmpty(reader["Sector_Representante"].ToString()) ? null : reader["Sector_Representante"].ToString(),
                            IdCatalogoPAREN_Representante = Convert.ToInt32(reader["IdCatalogoPAREN_Representante"]),
                            IdCatalogoFichaInst_Representante = string.IsNullOrEmpty(reader["IdCatalogoFichaInst_Representante"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdCatalogoFichaInst_Representante"]),
                            EmpresaTrabajo_Representante = string.IsNullOrEmpty(reader["EmpresaTrabajo_Representante"].ToString()) ? null : reader["EmpresaTrabajo_Representante"].ToString(),
                            DireccionTrabajo_Representante = string.IsNullOrEmpty(reader["DireccionTrabajo_Representante"].ToString()) ? null : reader["DireccionTrabajo_Representante"].ToString(),
                            TelefonoTrabajo_Representante = string.IsNullOrEmpty(reader["TelefonoTrabajo_Representante"].ToString()) ? null : reader["TelefonoTrabajo_Representante"].ToString(),
                            CargoTrabajo_Representante = string.IsNullOrEmpty(reader["CargoTrabajo_Representante"].ToString()) ? null : reader["CargoTrabajo_Representante"].ToString(),
                            AniosServicio_Representante = string.IsNullOrEmpty(reader["AniosServicio_Representante"].ToString()) ? (int?)null : Convert.ToInt32(reader["AniosServicio_Representante"]),
                            IngresoMensual_Representante = string.IsNullOrEmpty(reader["IngresoMensual_Representante"].ToString()) ? (double?)null : Convert.ToDouble(reader["IngresoMensual_Representante"]),
                            VehiculoPropio_Representante = string.IsNullOrEmpty(reader["VehiculoPropio_Representante"].ToString()) ? false : Convert.ToBoolean(reader["VehiculoPropio_Representante"]),
                            Marca_Representante = string.IsNullOrEmpty(reader["Marca_Representante"].ToString()) ? null : reader["Marca_Representante"].ToString(),
                            Modelo_Representante = string.IsNullOrEmpty(reader["Modelo_Representante"].ToString()) ? null : reader["Modelo_Representante"].ToString(),
                            AnioVehiculo_Representante = string.IsNullOrEmpty(reader["AnioVehiculo_Representante"].ToString()) ? (int?)null : Convert.ToInt32(reader["AnioVehiculo_Representante"]),
                            CasaPropia_Representante = string.IsNullOrEmpty(reader["CasaPropia_Representante"].ToString()) ? false : Convert.ToBoolean(reader["CasaPropia_Representante"]),
                            EstaFallecido_Representante = string.IsNullOrEmpty(reader["EstaFallecido_Representante"].ToString()) ? false : Convert.ToBoolean(reader["EstaFallecido_Representante"]),
                            IdProfesion_Representante = string.IsNullOrEmpty(reader["IdProfesion_Representante"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdProfesion_Representante"]),
                            SeFactura_Representante = string.IsNullOrEmpty(reader["SeFactura_Representante"].ToString()) ? false : Convert.ToBoolean(reader["SeFactura_Representante"]),
                            IdCiudad_Representante_Fact = string.IsNullOrEmpty(reader["IdCiudad_Representante_Fact"].ToString()) ? null : reader["IdCiudad_Representante_Fact"].ToString(),
                            IdTipoCredito_Representante = string.IsNullOrEmpty(reader["IdTipoCredito_Representante"].ToString()) ? null : reader["IdTipoCredito_Representante"].ToString(),
                            IdParroquia_Representante_Fact = string.IsNullOrEmpty(reader["IdParroquia_Representante_Fact"].ToString()) ? null : reader["IdParroquia_Representante_Fact"].ToString(),
                            Idtipo_cliente_Representante = string.IsNullOrEmpty(reader["Idtipo_cliente_Representante"].ToString()) ? (int?)null : Convert.ToInt32(reader["Idtipo_cliente_Representante"]),
                            SueldoPadre = string.IsNullOrEmpty(reader["SueldoPadre"].ToString()) ? 0 : Convert.ToDouble(reader["SueldoPadre"]),
                            SueldoMadre = string.IsNullOrEmpty(reader["SueldoMadre"].ToString()) ? 0 : Convert.ToDouble(reader["SueldoMadre"]),
                            OtroIngresoPadre = string.IsNullOrEmpty(reader["OtroIngresoPadre"].ToString()) ? 0 : Convert.ToDouble(reader["OtroIngresoPadre"]),
                            OtroIngresoMadre = string.IsNullOrEmpty(reader["OtroIngresoMadre"].ToString()) ? 0 : Convert.ToDouble(reader["OtroIngresoMadre"]),
                            GastoAlimentacion = string.IsNullOrEmpty(reader["GastoAlimentacion"].ToString()) ? 0 : Convert.ToDouble(reader["GastoAlimentacion"]),
                            GastoEducacion = string.IsNullOrEmpty(reader["GastoEducacion"].ToString()) ? 0 : Convert.ToDouble(reader["GastoEducacion"]),
                            GastoServicioBasico = string.IsNullOrEmpty(reader["GastoServicioBasico"].ToString()) ? 0 : Convert.ToDouble(reader["GastoServicioBasico"]),
                            GastoArriendo = string.IsNullOrEmpty(reader["GastoArriendo"].ToString()) ? 0 : Convert.ToDouble(reader["GastoArriendo"]),
                            GastoSalud = string.IsNullOrEmpty(reader["GastoSalud"].ToString()) ? 0 : Convert.ToDouble(reader["GastoSalud"]),
                            GastoPrestamo = string.IsNullOrEmpty(reader["GastoPrestamo"].ToString()) ? 0 : Convert.ToDouble(reader["GastoPrestamo"]),
                            OtroGasto = string.IsNullOrEmpty(reader["OtroGasto"].ToString()) ? 0 : Convert.ToDouble(reader["OtroGasto"]),
                            Estado = string.IsNullOrEmpty(reader["Estado"].ToString()) ? false : Convert.ToBoolean(reader["Estado"]),
                            IdCatalogoESTADM = Convert.ToInt32(reader["IdCatalogoESTADM"]),
                            IdUsuarioRevision = string.IsNullOrEmpty(reader["IdUsuarioRevision"].ToString()) ? null : reader["IdUsuarioRevision"].ToString(),
                            FechaRevision = string.IsNullOrEmpty(reader["FechaRevision"].ToString()) ? (DateTime?)null : Convert.ToDateTime(reader["FechaRevision"]),
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

        public bool modificarEstadoEnProceso(aca_Admision_Info info)
        {
            try
            {
                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    aca_Admision Entity = Context.aca_Admision.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdAdmision == info.IdAdmision);
                    if (Entity == null) return false;

                    Entity.IdUsuarioRevision = info.IdUsuarioRevision;
                    Entity.FechaRevision = DateTime.Now;
                    Entity.IdUsuarioModificacion = info.IdUsuarioModificacion;
                    Entity.FechaModificacion = DateTime.Now;
                    Entity.IdCatalogoESTADM = info.IdCatalogoESTADM;

                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool modificarEstado(aca_Admision_Info info)
        {
            try
            {
                using (EntitiesAcademico Context = new EntitiesAcademico())
                {
                    aca_Admision Entity = Context.aca_Admision.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdAdmision == info.IdAdmision);
                    if (Entity == null) return false;

                    Entity.IdUsuarioModificacion = info.IdUsuarioModificacion;
                    Entity.FechaModificacion = DateTime.Now;
                    Entity.IdCatalogoESTADM = info.IdCatalogoESTADM;

                    Context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
