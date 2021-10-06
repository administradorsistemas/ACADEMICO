using Core.Data.Academico;
using Core.Data.Base;
using Core.Info.General;
using Core.Info.Helps;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.General
{
    public class tb_persona_Data
    {
        aca_AnioLectivo_Data odata_anio = new aca_AnioLectivo_Data();
        public decimal validar_existe_cedula(string pe_CedulaRuc)
        {
            try
            {
                using (EntitiesGeneral Context = new EntitiesGeneral())
                {
                    pe_CedulaRuc = pe_CedulaRuc == null ? "" : pe_CedulaRuc.Trim();

                    var lst = from q in Context.tb_persona
                              where q.pe_cedulaRuc == pe_CedulaRuc
                              select q;

                    if (lst.Count() > 0)
                        return lst.FirstOrDefault().IdPersona;
                    else
                        return 0;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public decimal validar_existe_cedula(string IdTipoDocumento, string pe_CedulaRuc, decimal IdPersona)
        {
            try
            {
                using (EntitiesGeneral Context = new EntitiesGeneral())
                {
                    pe_CedulaRuc = pe_CedulaRuc == null ? "" : pe_CedulaRuc.Trim();

                    var lst = from q in Context.tb_persona
                              where q.pe_cedulaRuc == pe_CedulaRuc
                              && q.IdTipoDocumento == IdTipoDocumento
                              && q.IdPersona != IdPersona
                              select q;

                    if (lst.Count() > 0)
                        return lst.FirstOrDefault().IdPersona;
                    else
                        return 0;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public tb_persona_Info get_info(decimal IdPersona)
        {
            try
            {
                tb_persona_Info info = new tb_persona_Info();

                using (EntitiesGeneral Context = new EntitiesGeneral())
                {
                    tb_persona Entity = Context.tb_persona.FirstOrDefault(q => q.IdPersona == IdPersona);
                    if (Entity == null) return null;
                    info = new tb_persona_Info
                    {
                        IdPersona = Entity.IdPersona,
                        CodPersona = Entity.CodPersona,
                        pe_Naturaleza = Entity.pe_Naturaleza,
                        pe_nombreCompleto = Entity.pe_nombreCompleto,
                        pe_razonSocial = Entity.pe_razonSocial,
                        pe_apellido = Entity.pe_apellido,
                        pe_nombre = Entity.pe_nombre,
                        IdTipoDocumento = Entity.IdTipoDocumento,
                        pe_cedulaRuc = Entity.pe_cedulaRuc,
                        pe_direccion = Entity.pe_direccion,
                        pe_telfono_Contacto = Entity.pe_telfono_Contacto,
                        pe_celular = Entity.pe_celular,
                        pe_correo = Entity.pe_correo,
                        pe_sexo = Entity.pe_sexo,
                        IdEstadoCivil = Entity.IdEstadoCivil,
                        pe_fechaNacimiento = Entity.pe_fechaNacimiento,
                        pe_estado = Entity.pe_estado,
                        IdTipoCta_acreditacion_cat = Entity.IdTipoCta_acreditacion_cat,
                        num_cta_acreditacion = Entity.num_cta_acreditacion,
                        IdBanco_acreditacion = Entity.IdBanco_acreditacion,
                        IdProfesion = Entity.IdProfesion,
                        IdReligion = Entity.IdReligion,
                        AsisteCentroCristiano = Entity.AsisteCentroCristiano,
                        CodCatalogoSangre = Entity.CodCatalogoSangre,
                        CodCatalogoCONADIS = Entity.CodCatalogoCONADIS,
                        NumeroCarnetConadis = Entity.NumeroCarnetConadis,
                        PorcentajeDiscapacidad = Entity.PorcentajeDiscapacidad,
                        IdGrupoEtnico = Entity.IdGrupoEtnico
                    };
                }

                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public tb_persona_Info get_info_x_num_cedula(string pe_cedulaRuc)
        {
            try
            {
                tb_persona_Info info = new tb_persona_Info();

                using (EntitiesGeneral Context = new EntitiesGeneral())
                {
                    tb_persona Entity = Context.tb_persona.FirstOrDefault(q => q.pe_cedulaRuc == pe_cedulaRuc);
                    if (Entity == null) return null;
                    info = new tb_persona_Info
                    {
                        IdPersona = Entity.IdPersona,
                        CodPersona = Entity.CodPersona,
                        pe_Naturaleza = Entity.pe_Naturaleza,
                        pe_nombreCompleto = Entity.pe_nombreCompleto,
                        pe_razonSocial = Entity.pe_razonSocial,
                        pe_apellido = Entity.pe_apellido,
                        pe_nombre = Entity.pe_nombre,
                        IdTipoDocumento = Entity.IdTipoDocumento,
                        pe_cedulaRuc = Entity.pe_cedulaRuc,
                        pe_direccion = Entity.pe_direccion,
                        pe_telfono_Contacto = Entity.pe_telfono_Contacto,
                        pe_celular = Entity.pe_celular,
                        pe_correo = Entity.pe_correo,
                        pe_sexo = Entity.pe_sexo,
                        IdEstadoCivil = Entity.IdEstadoCivil,
                        pe_fechaNacimiento = Entity.pe_fechaNacimiento,
                        pe_estado = Entity.pe_estado,
                        IdTipoCta_acreditacion_cat = Entity.IdTipoCta_acreditacion_cat,
                        num_cta_acreditacion = Entity.num_cta_acreditacion,
                        IdBanco_acreditacion = Entity.IdBanco_acreditacion,
                        IdProfesion = Entity.IdProfesion,
                        IdReligion = Entity.IdReligion,
                        AsisteCentroCristiano = Entity.AsisteCentroCristiano,
                        CodCatalogoSangre = Entity.CodCatalogoSangre,
                        CodCatalogoCONADIS = Entity.CodCatalogoCONADIS,
                        NumeroCarnetConadis = Entity.NumeroCarnetConadis,
                        PorcentajeDiscapacidad = Entity.PorcentajeDiscapacidad,
                        IdGrupoEtnico = Entity.IdGrupoEtnico
                    };
                }

                return info;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<tb_persona_Info> get_list(bool mostrar_anulados)
        {
            try
            {
                List<tb_persona_Info> Lista;

                using (EntitiesGeneral Context = new EntitiesGeneral())
                {
                    if (mostrar_anulados)
                        Lista = (from q in Context.tb_persona
                                 select new tb_persona_Info
                                 {
                                     IdPersona = q.IdPersona,
                                     pe_nombreCompleto = q.pe_nombreCompleto,
                                     IdTipoDocumento = q.IdTipoDocumento,
                                     pe_cedulaRuc = q.pe_cedulaRuc,
                                     pe_estado = q.pe_estado,
                                     CodPersona = q.CodPersona,
                                     pe_Naturaleza = q.pe_Naturaleza,
                                     pe_razonSocial = q.pe_razonSocial,
                                     pe_apellido = q.pe_apellido,
                                     pe_nombre = q.pe_nombre,
                                     pe_direccion = q.pe_direccion,
                                     pe_telfono_Contacto = q.pe_telfono_Contacto,
                                     pe_celular = q.pe_celular,
                                     pe_correo = q.pe_correo,
                                     pe_sexo = q.pe_sexo,
                                     IdEstadoCivil = q.IdEstadoCivil,
                                     pe_fechaNacimiento = q.pe_fechaNacimiento,
                                     IdTipoCta_acreditacion_cat = q.IdTipoCta_acreditacion_cat,
                                     num_cta_acreditacion = q.num_cta_acreditacion,
                                     IdBanco_acreditacion = q.IdBanco_acreditacion,
                                     IdProfesion = q.IdProfesion,
                                     IdReligion = q.IdReligion,
                                     AsisteCentroCristiano = q.AsisteCentroCristiano,
                                     CodCatalogoSangre = q.CodCatalogoSangre,
                                     CodCatalogoCONADIS = q.CodCatalogoCONADIS,
                                     NumeroCarnetConadis = q.NumeroCarnetConadis,
                                     PorcentajeDiscapacidad = q.PorcentajeDiscapacidad,
                                     IdGrupoEtnico = q.IdGrupoEtnico,
                                     EstadoBool = q.pe_estado == "A" ? true : false
                                 }).ToList();
                    else
                        Lista = (from q in Context.tb_persona
                                 where q.pe_estado == "A"
                                 select new tb_persona_Info
                                 {
                                     IdPersona = q.IdPersona,
                                     pe_nombreCompleto = q.pe_nombreCompleto,
                                     IdTipoDocumento = q.IdTipoDocumento,
                                     pe_cedulaRuc = q.pe_cedulaRuc,
                                     pe_estado = q.pe_estado,
                                     CodPersona = q.CodPersona,
                                     pe_Naturaleza = q.pe_Naturaleza,
                                     pe_razonSocial = q.pe_razonSocial,
                                     pe_apellido = q.pe_apellido,
                                     pe_nombre = q.pe_nombre,
                                     pe_direccion = q.pe_direccion,
                                     pe_telfono_Contacto = q.pe_telfono_Contacto,
                                     pe_celular = q.pe_celular,
                                     pe_correo = q.pe_correo,
                                     pe_sexo = q.pe_sexo,
                                     IdEstadoCivil = q.IdEstadoCivil,
                                     pe_fechaNacimiento = q.pe_fechaNacimiento,
                                     IdTipoCta_acreditacion_cat = q.IdTipoCta_acreditacion_cat,
                                     num_cta_acreditacion = q.num_cta_acreditacion,
                                     IdBanco_acreditacion = q.IdBanco_acreditacion,
                                     IdProfesion = q.IdProfesion,
                                     IdReligion = q.IdReligion,
                                     AsisteCentroCristiano = q.AsisteCentroCristiano,
                                     CodCatalogoSangre = q.CodCatalogoSangre,
                                     CodCatalogoCONADIS = q.CodCatalogoCONADIS,
                                     NumeroCarnetConadis = q.NumeroCarnetConadis,
                                     PorcentajeDiscapacidad = q.PorcentajeDiscapacidad,
                                     IdGrupoEtnico = q.IdGrupoEtnico,
                                     EstadoBool = q.pe_estado == "A" ? true : false
                                 }).ToList();
                }

                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public tb_persona_Info armar_info(tb_persona_Info info)
        {
            tb_persona_Info info_retorno = new tb_persona_Info
            {
                //Campos obligatorios en toda pantalla
                pe_nombre = info.pe_nombre,
                pe_apellido = info.pe_apellido,
                pe_nombreCompleto = info.pe_nombreCompleto,
                pe_cedulaRuc = info.pe_cedulaRuc,
                pe_Naturaleza = info.pe_Naturaleza,
                IdTipoDocumento = info.IdTipoDocumento,
                pe_razonSocial = info.pe_razonSocial,

                //Campos opcionales
                pe_direccion = info.pe_direccion,
                pe_telfono_Contacto = info.pe_telfono_Contacto,
                pe_celular = info.pe_celular,
                pe_correo = info.pe_correo,
                pe_fechaNacimiento = info.pe_fechaNacimiento,
                CodCatalogoSangre = (info.CodCatalogoSangre=="" ? null : info.CodCatalogoSangre),

                CodCatalogoCONADIS = (info.CodCatalogoCONADIS=="" ? null : info.CodCatalogoCONADIS),
                NumeroCarnetConadis = info.NumeroCarnetConadis,
                PorcentajeDiscapacidad = info.PorcentajeDiscapacidad,

                //Si vienen null se pone un valor default
                IdEstadoCivil = string.IsNullOrEmpty(info.IdEstadoCivil) ? "SOLTE" : info.IdEstadoCivil,
                IdProfesion = (info.IdProfesion== 0 || info.IdProfesion==null) ? null : info.IdProfesion,
                IdReligion = (info.IdReligion == 0 || info.IdReligion == null) ? null : info.IdReligion,
                IdGrupoEtnico = (info.IdGrupoEtnico == 0 || info.IdGrupoEtnico == null) ? null : info.IdGrupoEtnico,
                AsisteCentroCristiano = info.AsisteCentroCristiano,
                pe_sexo = string.IsNullOrEmpty(info.pe_sexo) ? "SEXO_MAS" : info.pe_sexo,
            };
            return info_retorno;
        }

        private decimal get_id()
        {
            try
            {
                decimal ID = 1;

                using (EntitiesGeneral Context = new EntitiesGeneral())
                {
                    var lst = from q in Context.tb_persona
                              select q;

                    if (lst.Count() > 0)
                        ID = lst.Max(q => q.IdPersona) + 1;
                }

                return ID;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool guardarDB(tb_persona_Info info)
        {
            try
            {
                using (EntitiesGeneral Context = new EntitiesGeneral())
                {
                    tb_persona Entity = new tb_persona
                    {
                        IdPersona = info.IdPersona = get_id(),
                        CodPersona = info.CodPersona,
                        pe_Naturaleza = info.pe_Naturaleza,
                        pe_nombreCompleto = (info.pe_nombreCompleto ==null ? null: info.pe_nombreCompleto),
                        pe_razonSocial = (info.pe_razonSocial == null ? null : info.pe_razonSocial),
                        pe_apellido = (info.pe_apellido == null ? null : info.pe_apellido),
                        pe_nombre = (info.pe_nombre == null ? null : info.pe_nombre),
                        IdTipoDocumento = info.IdTipoDocumento,
                        pe_cedulaRuc = info.pe_cedulaRuc.Trim(),
                        pe_direccion = info.pe_direccion,
                        pe_telfono_Contacto = info.pe_telfono_Contacto,
                        pe_celular = info.pe_celular,
                        pe_correo = info.pe_correo,
                        pe_sexo = info.pe_sexo,
                        IdEstadoCivil = info.IdEstadoCivil,
                        pe_fechaNacimiento = info.pe_fechaNacimiento,
                        pe_estado = info.pe_estado = "A",
                        pe_fechaCreacion = info.pe_fechaCreacion = DateTime.Now,
                        IdTipoCta_acreditacion_cat = info.IdTipoCta_acreditacion_cat,
                        num_cta_acreditacion = info.num_cta_acreditacion,
                        IdBanco_acreditacion = info.IdBanco_acreditacion,
                        CodCatalogoSangre = info.CodCatalogoSangre,
                        CodCatalogoCONADIS = info.CodCatalogoCONADIS,
                        NumeroCarnetConadis = info.NumeroCarnetConadis,
                        PorcentajeDiscapacidad = info.PorcentajeDiscapacidad,
                        IdProfesion = ((info.IdProfesion == 0 || info.IdProfesion== null) ? null : info.IdProfesion),
                        IdReligion = ((info.IdReligion == 0 || info.IdReligion == null) ? null : info.IdReligion),
                        IdGrupoEtnico = ((info.IdGrupoEtnico == 0 || info.IdGrupoEtnico == null) ? null : info.IdGrupoEtnico),
                        AsisteCentroCristiano = info.AsisteCentroCristiano
                    };
                    Context.tb_persona.Add(Entity);
                    Context.SaveChanges();

                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool modificarDB(tb_persona_Info info)
        {
            try
            {
                using (EntitiesGeneral Context = new EntitiesGeneral())
                {
                    tb_persona Entity = Context.tb_persona.FirstOrDefault(q => q.IdPersona == info.IdPersona);
                    if (Entity == null) return false;
                    Entity.pe_Naturaleza = info.pe_Naturaleza;
                    Entity.pe_nombreCompleto = (info.pe_nombreCompleto == null ? null : info.pe_nombreCompleto);
                    Entity.pe_razonSocial = (info.pe_razonSocial == null ? null : info.pe_razonSocial);
                    Entity.pe_apellido = (info.pe_apellido == null ? null : info.pe_apellido);
                    Entity.pe_nombre = (info.pe_nombre == null ? null : info.pe_nombre);
                    Entity.IdTipoDocumento = info.IdTipoDocumento;
                    Entity.pe_cedulaRuc = info.pe_cedulaRuc;
                    Entity.pe_direccion = info.pe_direccion;
                    Entity.pe_telfono_Contacto = info.pe_telfono_Contacto;
                    Entity.pe_celular = info.pe_celular;
                    Entity.pe_correo = info.pe_correo;
                    Entity.pe_sexo = info.pe_sexo;
                    Entity.IdEstadoCivil = info.IdEstadoCivil;
                    Entity.pe_fechaNacimiento = info.pe_fechaNacimiento;
                    Entity.IdTipoCta_acreditacion_cat = info.IdTipoCta_acreditacion_cat;
                    Entity.num_cta_acreditacion = info.num_cta_acreditacion;
                    Entity.IdBanco_acreditacion = info.IdBanco_acreditacion;
                    Entity.CodCatalogoSangre = info.CodCatalogoSangre;
                    Entity.CodCatalogoCONADIS = info.CodCatalogoCONADIS;
                    Entity.NumeroCarnetConadis = info.NumeroCarnetConadis;
                    Entity.PorcentajeDiscapacidad = info.PorcentajeDiscapacidad;
                    Entity.IdProfesion = (info.IdProfesion==0 ? null : info.IdProfesion);
                    Entity.IdReligion = (info.IdReligion == 0 ? null : info.IdReligion);
                    Entity.IdGrupoEtnico = (info.IdGrupoEtnico == 0 ? null : info.IdGrupoEtnico);
                    Entity.AsisteCentroCristiano = info.AsisteCentroCristiano;
                    Entity.pe_fechaModificacion = DateTime.Now;
                    Entity.pe_UltUsuarioModi = info.pe_UltUsuarioModi;
                    Context.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool anularDB(tb_persona_Info info)
        {
            try
            {
                using (EntitiesGeneral Context = new EntitiesGeneral())
                {
                    tb_persona Entity = Context.tb_persona.FirstOrDefault(q => q.IdPersona == info.IdPersona);
                    if (Entity == null) return false;
                    Entity.pe_estado = "I";
                    Entity.Fecha_UltAnu = info.Fecha_UltAnu = DateTime.Now;
                    Entity.IdUsuarioUltAnu = info.IdUsuarioUltAnu;
                    Context.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<tb_persona_Info> get_list_bajo_demanda(ListEditItemsRequestedByFilterConditionEventArgs args, int IdEmpresa, string IdTipoPersona)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;
            List<tb_persona_Info> Lista = new List<tb_persona_Info>();
            Lista = get_list(IdEmpresa, IdTipoPersona, skip, take, args.Filter);
            return Lista;
        }

        public tb_persona_Info get_info_bajo_demanda(ListEditItemRequestedByValueEventArgs args, int IdEmpresa, string IdTipoPersona)
        {
            decimal id;
            if (args.Value == null || !decimal.TryParse(args.Value.ToString(), out id))
                return null;
            return get_info(IdEmpresa, IdTipoPersona, (decimal)args.Value);
        }

        public List<tb_persona_Info> get_list(int IdEmpresa, string IdTipo_persona, int skip, int take, string filter)
        {
            try
            {
                List<tb_persona_Info> Lista = new List<tb_persona_Info>();

                EntitiesGeneral context_g = new EntitiesGeneral();
                switch (IdTipo_persona)
                {
                    case "PERSONA":
                        var lstg = context_g.tb_persona.Where(q => q.pe_estado == "A" && (q.IdPersona.ToString() + " " + q.pe_cedulaRuc + " " + q.pe_nombreCompleto).Contains(filter)).OrderBy(q => q.IdPersona).Skip(skip).Take(take);
                        foreach (var q in lstg)
                        {
                            Lista.Add(new tb_persona_Info
                            {
                                IdPersona = q.IdPersona,
                                pe_nombreCompleto = q.pe_nombreCompleto,
                                pe_cedulaRuc = q.pe_cedulaRuc,
                                IdEntidad = q.IdPersona
                            });
                        }
                        break;
                    case "ALUMNO":
                        using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                        {
                            connection.Open();
                            string query = "SELECT a.IdEmpresa, a.IdAlumno, a.Codigo, a.IdPersona, p.pe_Naturaleza, p.pe_nombreCompleto, p.pe_apellido, p.pe_nombre, p.IdTipoDocumento, p.pe_cedulaRuc, a.Direccion, a.Celular, a.Correo, p.pe_sexo, p.pe_fechaNacimiento, "
                            + " p.CodCatalogoSangre, p.CodCatalogoCONADIS, p.PorcentajeDiscapacidad, p.NumeroCarnetConadis, a.Estado, a.IdCatalogoESTMAT, a.IdCurso, a.IdCatalogoESTALU, p.pe_telfono_Contacto, "
                            + " aca_Catalogo_1.NomCatalogo AS NomCatalogoESTMAT, c.NomCatalogo AS NomCatalogoESTALU, a.FechaIngreso, a.LugarNacimiento, a.IdPais, a.Cod_Region, a.IdProvincia, a.IdCiudad, a.IdParroquia, a.Sector, p.IdReligion, "
                            + " p.AsisteCentroCristiano, p.IdGrupoEtnico, a.Dificultad_Lectura, a.Dificultad_Escritura, a.Dificultad_Matematicas "
                            + " FROM     dbo.aca_Alumno AS a WITH (nolock)INNER JOIN "
                            + " dbo.tb_persona AS p WITH(nolock) ON a.IdPersona = p.IdPersona LEFT OUTER JOIN "
                            + " dbo.aca_Catalogo AS c WITH(nolock) ON a.IdCatalogoESTALU = c.IdCatalogo LEFT OUTER JOIN "
                            + " dbo.aca_Catalogo AS aca_Catalogo_1 WITH(nolock) ON a.IdCatalogoESTMAT = aca_Catalogo_1.IdCatalogo "
                            + " WHERE a.IdEmpresa = " + IdEmpresa.ToString() + " AND a.Estado = 1 AND (cast(a.IdAlumno as varchar) + a.Codigo + p.pe_cedulaRuc + p.pe_nombreCompleto) LIKE '%" + filter + "%'"
                            + " ORDER BY a.IdAlumno"
                            + " OFFSET " + skip.ToString() + " ROWS FETCH NEXT " + take.ToString() + " ROWS ONLY";

                            SqlCommand command = new SqlCommand(query, connection);
                            SqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                Lista.Add(new tb_persona_Info
                                {
                                    IdPersona = Convert.ToDecimal(reader["IdPersona"]),
                                    IdEntidad = Convert.ToDecimal(reader["IdAlumno"]),
                                    CodAlumno = string.IsNullOrEmpty(reader["Codigo"].ToString()) ? null : reader["Codigo"].ToString(),
                                    pe_nombreCompleto = Convert.ToString(reader["pe_nombreCompleto"]),
                                    pe_cedulaRuc = Convert.ToString(reader["pe_cedulaRuc"])
                                });
                            }
                            reader.Close();
                        }
                        /*
                        EntitiesAcademico context_a = new EntitiesAcademico();
                        var lst_al = context_a.vwaca_Alumno.Where(q => q.IdEmpresa == IdEmpresa && q.Estado == true && (q.IdAlumno.ToString() + " " + q.Codigo + " " + " " + q.pe_cedulaRuc + " " + q.pe_nombreCompleto).Contains(filter)).OrderBy(q => q.IdAlumno).Skip(skip).Take(take);
                        foreach (var q in lst_al)
                        {
                            Lista.Add(new tb_persona_Info
                            {
                                IdPersona = q.IdPersona,
                                pe_nombreCompleto = q.pe_nombreCompleto,
                                pe_cedulaRuc = q.pe_cedulaRuc,
                                IdEntidad = q.IdAlumno,
                                CodAlumno = q.Codigo
                            });
                        }
                        context_a.Dispose();
                        */
                        break;
                    case "ALUMNO_MATRICULA":
                        using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                        {
                            connection.Open();
                            var IdCatalogoRetirado = Convert.ToInt32(cl_enumeradores.eCatalogoAcademicoAlumno.RETIRADO);
                            string query = "SELECT a.IdEmpresa, a.IdAlumno, a.Codigo, a.IdPersona, p.pe_Naturaleza, p.pe_nombreCompleto, p.pe_apellido, p.pe_nombre, p.IdTipoDocumento, p.pe_cedulaRuc, a.Direccion, a.Celular, a.Correo, p.pe_sexo, p.pe_fechaNacimiento, "
                            + " p.CodCatalogoSangre, p.CodCatalogoCONADIS, p.PorcentajeDiscapacidad, p.NumeroCarnetConadis, a.Estado, a.IdCatalogoESTMAT, a.IdCurso, a.IdCatalogoESTALU, p.pe_telfono_Contacto, "
                            + " aca_Catalogo_1.NomCatalogo AS NomCatalogoESTMAT, c.NomCatalogo AS NomCatalogoESTALU, a.FechaIngreso, a.LugarNacimiento, a.IdPais, a.Cod_Region, a.IdProvincia, a.IdCiudad, a.IdParroquia, a.Sector, p.IdReligion, "
                            + " p.AsisteCentroCristiano, p.IdGrupoEtnico, a.Dificultad_Lectura, a.Dificultad_Escritura, a.Dificultad_Matematicas "
                            + " FROM     dbo.aca_Alumno AS a WITH (nolock)INNER JOIN "
                            + " dbo.tb_persona AS p WITH(nolock) ON a.IdPersona = p.IdPersona LEFT OUTER JOIN "
                            + " dbo.aca_Catalogo AS c WITH(nolock) ON a.IdCatalogoESTALU = c.IdCatalogo LEFT OUTER JOIN "
                            + " dbo.aca_Catalogo AS aca_Catalogo_1 WITH(nolock) ON a.IdCatalogoESTMAT = aca_Catalogo_1.IdCatalogo "
                            + " WHERE a.IdEmpresa = " + IdEmpresa.ToString() + " AND a.Estado = 1 AND a.IdCatalogoESTALU != " + IdCatalogoRetirado .ToString() + " AND(cast(a.IdAlumno as varchar) + a.Codigo + p.pe_cedulaRuc + p.pe_nombreCompleto) LIKE '%" + filter + "%'"
                            + " ORDER BY a.IdAlumno"
                            + " OFFSET " + skip.ToString() + " ROWS FETCH NEXT " + take.ToString() + " ROWS ONLY";

                            SqlCommand command = new SqlCommand(query, connection);
                            SqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                Lista.Add(new tb_persona_Info
                                {
                                    IdPersona = Convert.ToDecimal(reader["IdPersona"]),
                                    IdEntidad = Convert.ToDecimal(reader["IdAlumno"]),
                                    CodAlumno = string.IsNullOrEmpty(reader["Codigo"].ToString()) ? null : reader["Codigo"].ToString(),
                                    pe_nombreCompleto = Convert.ToString(reader["pe_nombreCompleto"]),
                                    pe_cedulaRuc = Convert.ToString(reader["pe_cedulaRuc"])
                                });
                            }
                            reader.Close();
                        }
                        /*
                        EntitiesAcademico context_mat = new EntitiesAcademico();
                        var IdCatalogoRetirado = Convert.ToInt32(cl_enumeradores.eCatalogoAcademicoAlumno.RETIRADO);
                        var lst_mat = context_mat.vwaca_Alumno.Where(q => q.IdEmpresa == IdEmpresa && q.IdCatalogoESTALU != IdCatalogoRetirado && q.Estado == true 
                        && (q.IdAlumno.ToString() + " " + q.Codigo + " " + " " + q.pe_cedulaRuc + " " + q.pe_nombreCompleto).Contains(filter)).OrderBy(q => q.IdAlumno).Skip(skip).Take(take);
                        foreach (var q in lst_mat)
                        {
                            Lista.Add(new tb_persona_Info
                            {
                                IdPersona = q.IdPersona,
                                pe_nombreCompleto = q.pe_nombreCompleto,
                                pe_cedulaRuc = q.pe_cedulaRuc,
                                IdEntidad = q.IdAlumno,
                                CodAlumno = q.Codigo
                            });
                        }
                        context_mat.Dispose();
                        */
                        break;
                    case "ALUMNO_MATRICULADOS":
                        EntitiesAcademico context_matriculados = new EntitiesAcademico();
                        var info_anio = odata_anio.getInfo_AnioEnCurso(IdEmpresa, 0);
                        var IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
                        var IdCatRetirado = Convert.ToInt32(cl_enumeradores.eCatalogoAcademicoAlumno.RETIRADO);
                        var lst_matriculados = context_matriculados.vwaca_Matricula.Where(q => q.IdEmpresa == IdEmpresa && q.IdAnio ==IdAnio 
                        && (q.IdAlumno.ToString() + " " + q.Codigo + " " + " " + q.pe_cedulaRuc + " " + q.pe_nombreCompleto).Contains(filter)).OrderBy(q => q.IdAlumno).Skip(skip).Take(take);
                        foreach (var q in lst_matriculados)
                        {
                            Lista.Add(new tb_persona_Info
                            {
                                IdPersona = Convert.ToDecimal(q.IdPersona),
                                pe_nombreCompleto = q.pe_nombreCompleto,
                                pe_cedulaRuc = q.pe_cedulaRuc,
                                IdEntidad = q.IdAlumno,
                                CodAlumno = q.Codigo
                            });
                        }
                        context_matriculados.Dispose();
                        break;
                    case "TUTOR":
                        EntitiesAcademico context_t = new EntitiesAcademico();
                        var lst_tutor = context_t.vwaca_Profesor.Where(q => q.IdEmpresa == IdEmpresa && q.Estado == true && (q.IdProfesor.ToString() + " " + q.pe_cedulaRuc + " " + q.pe_nombreCompleto).Contains(filter)).OrderBy(q => q.IdProfesor).Skip(skip).Take(take);
                        foreach (var q in lst_tutor)
                        {
                            Lista.Add(new tb_persona_Info
                            {
                                IdPersona = q.IdPersona,
                                pe_nombreCompleto = q.pe_nombreCompleto,
                                pe_cedulaRuc = q.pe_cedulaRuc,
                                IdEntidad = q.IdProfesor
                            });
                        }
                        context_t.Dispose();
                        break;
                    case "INSPECTOR":
                        EntitiesAcademico context_i = new EntitiesAcademico();
                        var lst_inspector = context_i.vwaca_Profesor.Where(q => q.IdEmpresa == IdEmpresa && q.EsInspector == true && q.Estado == true && (q.IdProfesor.ToString() + " " + q.pe_cedulaRuc + " " + q.pe_nombreCompleto).Contains(filter)).OrderBy(q => q.IdProfesor).Skip(skip).Take(take);
                        foreach (var q in lst_inspector)
                        {
                            Lista.Add(new tb_persona_Info
                            {
                                IdPersona = q.IdPersona,
                                pe_nombreCompleto = q.pe_nombreCompleto,
                                pe_cedulaRuc = q.pe_cedulaRuc,
                                IdEntidad = q.IdProfesor
                            });
                        }
                        context_i.Dispose();
                        break;
                    case "PROFESOR":
                        EntitiesAcademico context_pro = new EntitiesAcademico();
                        var lst_profesor= context_pro.vwaca_Profesor.Where(q => q.IdEmpresa == IdEmpresa && q.EsProfesor == true && q.Estado == true && (q.IdProfesor.ToString() + " " + q.pe_cedulaRuc + " " + q.pe_nombreCompleto).Contains(filter)).OrderBy(q => q.IdProfesor).Skip(skip).Take(take);
                        foreach (var q in lst_profesor)
                        {
                            Lista.Add(new tb_persona_Info
                            {
                                IdPersona = q.IdPersona,
                                pe_nombreCompleto = q.pe_nombreCompleto,
                                pe_cedulaRuc = q.pe_cedulaRuc,
                                IdEntidad = q.IdProfesor
                            });
                        }
                        context_pro.Dispose();
                        break;
                    case "CLIENTE":
                        EntitiesFacturacion context_f = new EntitiesFacturacion();
                        var lstcli = context_f.vwfa_cliente_consulta.Where(q => q.IdEmpresa == IdEmpresa && q.Estado == "A" && (q.IdCliente.ToString() + " " + q.pe_cedulaRuc + " " + q.pe_nombreCompleto).Contains(filter)).OrderBy(q => q.IdCliente).Skip(skip).Take(take);
                        foreach (var q in lstcli)
                        {
                            Lista.Add(new tb_persona_Info
                            {
                                IdPersona = q.IdPersona,
                                pe_nombreCompleto = q.pe_nombreCompleto,
                                pe_cedulaRuc = q.pe_cedulaRuc,
                                IdEntidad = q.IdCliente,
                                CodPersona = q.Descripcion_tip_cliente
                            });
                        }
                        context_f.Dispose();
                        break;
                    case "EMPLEA":
                        EntitiesRRHH context_e = new EntitiesRRHH();
                        var lstr = context_e.vwro_empleados_consulta.Where(q => q.IdEmpresa == IdEmpresa && q.em_estado == "A" && (q.IdEmpleado.ToString() + " " + q.pe_cedulaRuc + " " + q.Empleado).Contains(filter)).OrderBy(q => q.IdEmpleado).Skip(skip).Take(take);
                        foreach (var q in lstr)
                        {
                            Lista.Add(new tb_persona_Info
                            {
                                IdPersona = q.IdPersona,
                                pe_nombreCompleto = q.Empleado,
                                pe_cedulaRuc = q.pe_cedulaRuc,
                                IdEntidad = q.IdEmpleado
                            });
                        }
                        context_e.Dispose();
                        break;
                    case "PROVEE":
                        EntitiesCuentasPorPagar context_p = new EntitiesCuentasPorPagar();
                        var lstp = context_p.vwcp_proveedor_consulta.Where(q => q.IdEmpresa == IdEmpresa && q.pr_estado == "A" && (q.IdProveedor.ToString() + " " + q.pe_cedulaRuc + " " + q.pe_nombreCompleto).Contains(filter)).OrderBy(q => q.IdProveedor).Skip(skip).Take(take);
                        foreach (var q in lstp)
                        {
                            Lista.Add(new tb_persona_Info
                            {
                                IdPersona = q.IdPersona,
                                pe_nombreCompleto = q.pe_nombreCompleto,
                                pe_cedulaRuc = q.pe_cedulaRuc,
                                IdEntidad = q.IdProveedor
                            });
                        }
                        context_p.Dispose();
                        break;
                }

                context_g.Dispose();
                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public tb_persona_Info get_info(int IdEmpresa, string IdTipoPersona, decimal IdEntidad)
        {
            tb_persona_Info info = new tb_persona_Info();

            EntitiesGeneral context_g = new EntitiesGeneral();
            switch (IdTipoPersona)
            {
                case "PERSONA":
                    info = (from q in context_g.tb_persona
                            where q.pe_estado == "A"
                            && q.IdPersona == IdEntidad
                            select new tb_persona_Info
                            {
                                IdPersona = q.IdPersona,
                                pe_nombreCompleto = q.pe_nombreCompleto,
                                pe_cedulaRuc = q.pe_cedulaRuc,
                                IdEntidad = q.IdPersona
                            }).FirstOrDefault();
                    break;
                case "ALUMNO":
                    using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("", connection);
                        command.CommandText = "SELECT a.IdEmpresa, a.IdAlumno, a.Codigo, a.IdPersona, p.pe_Naturaleza, p.pe_nombreCompleto, p.pe_apellido, p.pe_nombre, p.IdTipoDocumento, p.pe_cedulaRuc, a.Direccion, a.Celular, a.Correo, p.pe_sexo, p.pe_fechaNacimiento, "
                            + " p.CodCatalogoSangre, p.CodCatalogoCONADIS, p.PorcentajeDiscapacidad, p.NumeroCarnetConadis, a.Estado, a.IdCatalogoESTMAT, a.IdCurso, a.IdCatalogoESTALU, p.pe_telfono_Contacto, "
                            + " aca_Catalogo_1.NomCatalogo AS NomCatalogoESTMAT, c.NomCatalogo AS NomCatalogoESTALU, a.FechaIngreso, a.LugarNacimiento, a.IdPais, a.Cod_Region, a.IdProvincia, a.IdCiudad, a.IdParroquia, a.Sector, p.IdReligion, "
                            + " p.AsisteCentroCristiano, p.IdGrupoEtnico, a.Dificultad_Lectura, a.Dificultad_Escritura, a.Dificultad_Matematicas "
                            + " FROM     dbo.aca_Alumno AS a WITH (nolock)INNER JOIN "
                            + " dbo.tb_persona AS p WITH(nolock) ON a.IdPersona = p.IdPersona LEFT OUTER JOIN "
                            + " dbo.aca_Catalogo AS c WITH(nolock) ON a.IdCatalogoESTALU = c.IdCatalogo LEFT OUTER JOIN "
                            + " dbo.aca_Catalogo AS aca_Catalogo_1 WITH(nolock) ON a.IdCatalogoESTMAT = aca_Catalogo_1.IdCatalogo "                            
                            + " WHERE a.IdEmpresa = " + IdEmpresa.ToString() + " AND a.Estado = 1 AND a.IdAlumno = " + IdEntidad.ToString();
                        var ResultValue = command.ExecuteScalar();

                        if (ResultValue == null)
                            return null;

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            info = new tb_persona_Info
                            {
                                IdPersona = Convert.ToDecimal(reader["IdPersona"]),
                                IdEntidad = Convert.ToDecimal(reader["IdAlumno"]),
                                CodAlumno = string.IsNullOrEmpty(reader["Codigo"].ToString()) ? null : reader["Codigo"].ToString(),
                                pe_nombreCompleto = Convert.ToString(reader["pe_nombreCompleto"]),
                                pe_cedulaRuc = Convert.ToString(reader["pe_cedulaRuc"])
                            };
                        }
                    }
                    /*
                    EntitiesAcademico context_aca = new EntitiesAcademico();
                    info = (from q in context_aca.vwaca_Alumno
                            where q.Estado == true
                            && q.IdEmpresa == IdEmpresa
                            && q.IdAlumno == IdEntidad
                            select new tb_persona_Info
                            {
                                IdPersona = q.IdPersona,
                                pe_nombreCompleto = q.pe_nombreCompleto,
                                pe_cedulaRuc = q.pe_cedulaRuc,
                                IdEntidad = q.IdAlumno,
                                CodAlumno = q.Codigo
                            }).FirstOrDefault();
                    context_aca.Dispose();
                    */
                    break;
                case "ALUMNO_MATRICULA":
                    using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("", connection);
                        var IdCatalogoRetirado = Convert.ToInt32(cl_enumeradores.eCatalogoAcademicoAlumno.RETIRADO);
                        command.CommandText = "SELECT a.IdEmpresa, a.IdAlumno, a.Codigo, a.IdPersona, p.pe_Naturaleza, p.pe_nombreCompleto, p.pe_apellido, p.pe_nombre, p.IdTipoDocumento, p.pe_cedulaRuc, a.Direccion, a.Celular, a.Correo, p.pe_sexo, p.pe_fechaNacimiento, "
                            + " p.CodCatalogoSangre, p.CodCatalogoCONADIS, p.PorcentajeDiscapacidad, p.NumeroCarnetConadis, a.Estado, a.IdCatalogoESTMAT, a.IdCurso, a.IdCatalogoESTALU, p.pe_telfono_Contacto, "
                            + " aca_Catalogo_1.NomCatalogo AS NomCatalogoESTMAT, c.NomCatalogo AS NomCatalogoESTALU, a.FechaIngreso, a.LugarNacimiento, a.IdPais, a.Cod_Region, a.IdProvincia, a.IdCiudad, a.IdParroquia, a.Sector, p.IdReligion, "
                            + " p.AsisteCentroCristiano, p.IdGrupoEtnico, a.Dificultad_Lectura, a.Dificultad_Escritura, a.Dificultad_Matematicas "
                            + " FROM     dbo.aca_Alumno AS a WITH (nolock)INNER JOIN "
                            + " dbo.tb_persona AS p WITH(nolock) ON a.IdPersona = p.IdPersona LEFT OUTER JOIN "
                            + " dbo.aca_Catalogo AS c WITH(nolock) ON a.IdCatalogoESTALU = c.IdCatalogo LEFT OUTER JOIN "
                            + " dbo.aca_Catalogo AS aca_Catalogo_1 WITH(nolock) ON a.IdCatalogoESTMAT = aca_Catalogo_1.IdCatalogo "
                            + " WHERE a.IdEmpresa = " + IdEmpresa.ToString() + " AND a.Estado = 1 and a.IdAlumno = " + IdEntidad.ToString();
                        var ResultValue = command.ExecuteScalar();

                        if (ResultValue == null)
                            return null;

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            info = new tb_persona_Info
                            {
                                IdPersona = Convert.ToDecimal(reader["IdPersona"]),
                                IdEntidad = Convert.ToDecimal(reader["IdAlumno"]),
                                CodAlumno = string.IsNullOrEmpty(reader["Codigo"].ToString()) ? null : reader["Codigo"].ToString(),
                                pe_nombreCompleto = Convert.ToString(reader["pe_nombreCompleto"]),
                                pe_cedulaRuc = Convert.ToString(reader["pe_cedulaRuc"])
                            };
                        }
                    }
                    /*
                    EntitiesAcademico context_mat = new EntitiesAcademico();
                    var IdCatalogoRetirado = Convert.ToInt32(cl_enumeradores.eCatalogoAcademicoAlumno.RETIRADO);
                    info = (from q in context_mat.vwaca_Alumno
                            where q.Estado == true
                            && q.IdEmpresa == IdEmpresa
                            && q.IdAlumno == IdEntidad
                            && q.IdCatalogoESTALU != IdCatalogoRetirado
                            select new tb_persona_Info
                            {
                                IdPersona = q.IdPersona,
                                pe_nombreCompleto = q.pe_nombreCompleto,
                                pe_cedulaRuc = q.pe_cedulaRuc,
                                IdEntidad = q.IdAlumno,
                                CodAlumno = q.Codigo
                            }).FirstOrDefault();
                    context_mat.Dispose();
                    */
                    break;
                case "ALUMNO_MATRICULADOS":
                    EntitiesAcademico context_matriculados = new EntitiesAcademico();
                    var info_anio = odata_anio.getInfo_AnioEnCurso(IdEmpresa, 0);
                    var IdAnio = (info_anio == null ? 0 : info_anio.IdAnio);
                    var IdCatRetirado = Convert.ToInt32(cl_enumeradores.eCatalogoAcademicoAlumno.RETIRADO);
                    info = (from q in context_matriculados.vwaca_Matricula
                            where q.IdEmpresa == IdEmpresa
                            && q.IdAlumno == IdEntidad
                            && q.IdAnio != IdAnio
                            select new tb_persona_Info
                            {
                                IdPersona =  Convert.ToDecimal(q.IdPersona),
                                pe_nombreCompleto = q.pe_nombreCompleto,
                                pe_cedulaRuc = q.pe_cedulaRuc,
                                IdEntidad = q.IdAlumno,
                                CodAlumno = q.Codigo
                            }).FirstOrDefault();
                    context_matriculados.Dispose();
                    break;
                case "TUTOR":
                    EntitiesAcademico context_tutor = new EntitiesAcademico();
                    info = (from q in context_tutor.vwaca_Profesor
                            where q.Estado == true
                            && q.IdEmpresa == IdEmpresa
                            && q.IdProfesor == IdEntidad
                            select new tb_persona_Info
                            {
                                IdPersona = q.IdPersona,
                                pe_nombreCompleto = q.pe_nombreCompleto,
                                pe_cedulaRuc = q.pe_cedulaRuc,
                                IdEntidad = q.IdProfesor
                            }).FirstOrDefault();
                    context_tutor.Dispose();
                    break;
                case "INSPECTOR":
                    EntitiesAcademico context_insp = new EntitiesAcademico();
                    info = (from q in context_insp.vwaca_Profesor
                            where q.Estado == true
                            && q.IdEmpresa == IdEmpresa
                            && q.IdProfesor == IdEntidad
                            && q.EsInspector == true
                            select new tb_persona_Info
                            {
                                IdPersona = q.IdPersona,
                                pe_nombreCompleto = q.pe_nombreCompleto,
                                pe_cedulaRuc = q.pe_cedulaRuc,
                                IdEntidad = q.IdProfesor
                            }).FirstOrDefault();
                    context_insp.Dispose();
                    break;
                case "PROFESOR":
                    EntitiesAcademico context_prof = new EntitiesAcademico();
                    info = (from q in context_prof.vwaca_Profesor
                            where q.Estado == true
                            && q.IdEmpresa == IdEmpresa
                            && q.IdProfesor == IdEntidad
                            && q.EsProfesor == true
                            select new tb_persona_Info
                            {
                                IdPersona = q.IdPersona,
                                pe_nombreCompleto = q.pe_nombreCompleto,
                                pe_cedulaRuc = q.pe_cedulaRuc,
                                IdEntidad = q.IdProfesor
                            }).FirstOrDefault();
                    context_prof.Dispose();
                    break;
                case "EMPLEA":
                    EntitiesRRHH context_e = new EntitiesRRHH();
                    info = (from q in context_e.vwro_empleados_consulta
                            where q.em_estado == "A"
                            && q.IdEmpresa == IdEmpresa
                            && q.IdEmpleado == IdEntidad
                            select new tb_persona_Info
                            {
                                IdPersona = q.IdPersona,
                                pe_nombreCompleto = q.Empleado,
                                pe_cedulaRuc = q.pe_cedulaRuc,
                                IdEntidad = q.IdEmpleado
                            }).FirstOrDefault();
                    context_e.Dispose();
                    break;
                case "PROVEE":
                    EntitiesCuentasPorPagar context_p = new EntitiesCuentasPorPagar();
                    info = (from q in context_p.vwcp_proveedor_consulta
                            where q.pr_estado == "A"
                            && q.IdEmpresa == IdEmpresa
                            && q.IdProveedor == IdEntidad
                            select new tb_persona_Info
                            {
                                IdPersona = q.IdPersona,
                                pe_nombreCompleto = q.pe_nombreCompleto,
                                pe_cedulaRuc = q.pe_cedulaRuc,
                                IdEntidad = q.IdProveedor
                            }).FirstOrDefault();
                    context_p.Dispose();
                    break;
                case "CLIENTE":
                    EntitiesFacturacion context_f = new EntitiesFacturacion();
                    info = (from q in context_f.vwfa_cliente_consulta
                            where q.Estado == "A"
                            && q.IdEmpresa == IdEmpresa
                            && q.IdCliente == IdEntidad
                            select new tb_persona_Info
                            {
                                IdPersona = q.IdPersona,
                                pe_nombreCompleto = q.pe_nombreCompleto,
                                pe_cedulaRuc = q.pe_cedulaRuc,
                                IdEntidad = q.IdCliente
                            }).FirstOrDefault();
                    context_f.Dispose();
                    break;
            }

            context_g.Dispose();

            return info;
        }

    }
}
