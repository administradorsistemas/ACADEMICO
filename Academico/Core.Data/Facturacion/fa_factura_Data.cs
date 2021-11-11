using Core.Data.Base;
using Core.Data.Contabilidad;
using Core.Data.General;
using Core.Info.Contabilidad;
using Core.Info.Facturacion;
using Core.Info.General;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Facturacion
{
   public class fa_factura_Data
    {
        fa_notaCreDeb_Data odatNc = new fa_notaCreDeb_Data();
        public List<fa_factura_consulta_Info> get_list(int IdEmpresa, int IdSucursal, DateTime Fecha_ini, DateTime Fecha_fin)
        {
            try
            {
                List<fa_factura_consulta_Info> Lista=new List<fa_factura_consulta_Info>();
                Fecha_ini = Fecha_ini.Date;
                Fecha_fin = Fecha_fin.Date;
                int IdSucursalIni = IdSucursal;
                int IdSucursalFin = IdSucursal == 0 ? 999 : IdSucursal;

                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();

                    #region Query
                    string query = "SELECT dbo.fa_factura.IdEmpresa, dbo.fa_factura.IdSucursal, dbo.fa_factura.IdBodega, dbo.fa_factura.IdCbteVta, dbo.fa_factura.vt_NumFactura, dbo.fa_factura.vt_fecha, dbo.tb_persona.pe_nombreCompleto AS Nombres, "
                    + " dbo.fa_Vendedor.Ve_Vendedor, det.vt_Subtotal0, det.vt_SubtotalIVA, det.vt_iva, det.vt_total, dbo.fa_factura.Estado, dbo.fa_factura.esta_impresa, dbo.fa_factura_x_in_Ing_Egr_Inven.IdEmpresa_in_eg_x_inv, "
                    + " dbo.fa_factura_x_in_Ing_Egr_Inven.IdSucursal_in_eg_x_inv, dbo.fa_factura_x_in_Ing_Egr_Inven.IdMovi_inven_tipo_in_eg_x_inv, dbo.fa_factura_x_in_Ing_Egr_Inven.IdNumMovi_in_eg_x_inv, dbo.fa_factura.Fecha_Autorizacion, "
                    + " dbo.fa_factura.vt_autorizacion, dbo.fa_factura.IdAlumno, tb_persona_1.pe_nombreCompleto AS NombresAlumno, dbo.fa_factura.IdUsuario "
                    + " FROM dbo.tb_persona AS tb_persona_1 WITH (nolock)INNER JOIN "
                    + " dbo.aca_Alumno WITH(nolock) ON tb_persona_1.IdPersona = dbo.aca_Alumno.IdPersona RIGHT OUTER JOIN "
                    + " dbo.fa_factura WITH(nolock) INNER JOIN "
                    + " dbo.fa_Vendedor WITH(nolock) ON dbo.fa_factura.IdEmpresa = dbo.fa_Vendedor.IdEmpresa AND dbo.fa_factura.IdVendedor = dbo.fa_Vendedor.IdVendedor LEFT OUTER JOIN "
                    + " ( "
                    + " SELECT IdEmpresa, IdSucursal, IdBodega, IdCbteVta, SUM(vt_Subtotal0) AS vt_Subtotal0, SUM(vt_SubtotalIVA) AS vt_SubtotalIVA, SUM(vt_iva) AS vt_iva, SUM(vt_total) AS vt_total "
                    + " FROM( "
                        + " SELECT IdEmpresa, IdSucursal, IdBodega, IdCbteVta, CASE WHEN vt_por_iva = 0 THEN vt_Subtotal ELSE 0 END AS vt_Subtotal0, CASE WHEN vt_por_iva > 0 THEN vt_Subtotal ELSE 0 END AS vt_SubtotalIVA, vt_iva, "
                        + " vt_total "
                    + " FROM dbo.fa_factura_det WITH(nolock) "
                    + " ) AS A "
                    + " GROUP BY IdEmpresa, IdSucursal, IdBodega, IdCbteVta) AS det ON dbo.fa_factura.IdCbteVta = det.IdCbteVta AND dbo.fa_factura.IdBodega = det.IdBodega AND dbo.fa_factura.IdSucursal = det.IdSucursal AND "
                    + " dbo.fa_factura.IdEmpresa = det.IdEmpresa LEFT OUTER JOIN "
                    + " dbo.fa_cliente_contactos ON dbo.fa_factura.IdEmpresa = dbo.fa_cliente_contactos.IdEmpresa AND dbo.fa_factura.IdCliente = dbo.fa_cliente_contactos.IdCliente LEFT OUTER JOIN "
                    + " dbo.fa_factura_x_in_Ing_Egr_Inven ON dbo.fa_factura.IdEmpresa = dbo.fa_factura_x_in_Ing_Egr_Inven.IdEmpresa_fa AND dbo.fa_factura.IdSucursal = dbo.fa_factura_x_in_Ing_Egr_Inven.IdSucursal_fa AND "
                    + " dbo.fa_factura.IdBodega = dbo.fa_factura_x_in_Ing_Egr_Inven.IdBodega_fa AND dbo.fa_factura.IdCbteVta = dbo.fa_factura_x_in_Ing_Egr_Inven.IdCbteVta_fa INNER JOIN "
                    + " dbo.fa_cliente ON dbo.fa_factura.IdEmpresa = dbo.fa_cliente.IdEmpresa AND dbo.fa_factura.IdCliente = dbo.fa_cliente.IdCliente INNER JOIN "
                    + " dbo.tb_persona ON dbo.fa_cliente.IdPersona = dbo.tb_persona.IdPersona ON dbo.aca_Alumno.IdEmpresa = dbo.fa_factura.IdEmpresa AND dbo.aca_Alumno.IdAlumno = dbo.fa_factura.IdAlumno "
                    + " WHERE dbo.fa_factura.IdEmpresa=" + IdEmpresa.ToString() + " AND dbo.fa_factura.IdSucursal BETWEEN " + IdSucursalIni + " AND " + IdSucursalFin.ToString()
                    + " AND dbo.fa_factura.vt_fecha BETWEEN DATEFROMPARTS(" + Fecha_ini.Year.ToString() + "," + Fecha_ini.Month.ToString() + "," + Fecha_ini.Day.ToString() + ") and DATEFROMPARTS(" + Fecha_fin.Year.ToString() + "," + Fecha_fin.Month.ToString() + "," + Fecha_fin.Day.ToString() + ")"
                    + " AND dbo.fa_factura.IdAlumno > 0 "
                    + " order by dbo.fa_factura.IdCbteVta desc";
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 0;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new fa_factura_consulta_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdSucursal = Convert.ToInt32(reader["IdSucursal"]),
                            IdBodega = Convert.ToInt32(reader["IdBodega"]),
                            IdCbteVta = Convert.ToDecimal(reader["IdCbteVta"]),

                            vt_NumFactura = string.IsNullOrEmpty(reader["vt_NumFactura"].ToString()) ? null : reader["vt_NumFactura"].ToString(),
                            vt_fecha = Convert.ToDateTime(reader["vt_fecha"]),
                            NomContacto = string.IsNullOrEmpty(reader["Nombres"].ToString()) ? null : reader["Nombres"].ToString(),
                            Ve_Vendedor = string.IsNullOrEmpty(reader["Ve_Vendedor"].ToString()) ? null : reader["Ve_Vendedor"].ToString(),
                            vt_Subtotal0 = string.IsNullOrEmpty(reader["vt_Subtotal0"].ToString()) ? (double?)null : Convert.ToDouble(reader["vt_Subtotal0"]),
                            vt_SubtotalIVA = string.IsNullOrEmpty(reader["vt_SubtotalIVA"].ToString()) ? (double?)null : Convert.ToDouble(reader["vt_SubtotalIVA"]),
                            vt_iva = string.IsNullOrEmpty(reader["vt_iva"].ToString()) ? (double?)null : Convert.ToDouble(reader["vt_iva"]),
                            vt_total = string.IsNullOrEmpty(reader["vt_total"].ToString()) ? (double?)null : Convert.ToDouble(reader["vt_total"]),
                            Estado = string.IsNullOrEmpty(reader["Estado"].ToString()) ? null : reader["Estado"].ToString(),
                            esta_impresa = string.IsNullOrEmpty(reader["esta_impresa"].ToString()) ? false : Convert.ToBoolean(reader["esta_impresa"]),

                            IdEmpresa_in_eg_x_inv = string.IsNullOrEmpty(reader["IdEmpresa_in_eg_x_inv"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdEmpresa_in_eg_x_inv"]),
                            IdSucursal_in_eg_x_inv = string.IsNullOrEmpty(reader["IdSucursal_in_eg_x_inv"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdSucursal_in_eg_x_inv"]),
                            IdMovi_inven_tipo_in_eg_x_inv = string.IsNullOrEmpty(reader["IdMovi_inven_tipo_in_eg_x_inv"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdMovi_inven_tipo_in_eg_x_inv"]),
                            IdNumMovi_in_eg_x_inv = string.IsNullOrEmpty(reader["IdNumMovi_in_eg_x_inv"].ToString()) ? (int?)null : Convert.ToInt32(reader["IdNumMovi_in_eg_x_inv"]),

                            vt_autorizacion = string.IsNullOrEmpty(reader["vt_autorizacion"].ToString()) ? null : reader["vt_autorizacion"].ToString(),
                            Fecha_Autorizacion = string.IsNullOrEmpty(reader["Fecha_Autorizacion"].ToString()) ? (DateTime?)null : Convert.ToDateTime(reader["Fecha_Autorizacion"]),
                            NombresAlumno = string.IsNullOrEmpty(reader["NombresAlumno"].ToString()) ? null : reader["NombresAlumno"].ToString(),
                            EstadoBool = string.IsNullOrEmpty(reader["Estado"].ToString()) ? false : (reader["Estado"].ToString()=="A" ? true : false),

                            IdUsuario = string.IsNullOrEmpty(reader["IdUsuario"].ToString()) ? null : reader["IdUsuario"].ToString(),
                        });
                    }
                    reader.Close();
                }

                /*
                using (EntitiesFacturacion Context = new EntitiesFacturacion())
                {
                    Lista = (from q in Context.vwfa_factura
                             where q.IdEmpresa == IdEmpresa
                             && q.IdSucursal >= IdSucursalIni
                             && q.IdSucursal <= IdSucursalFin
                             && Fecha_ini <= q.vt_fecha && q.vt_fecha <= Fecha_fin
                             && q.IdAlumno >0
                             orderby q.IdCbteVta descending
                             select new fa_factura_consulta_Info
                             {
                                 IdEmpresa = q.IdEmpresa,
                                 IdSucursal = q.IdSucursal,
                                 IdBodega = q.IdBodega,
                                 IdCbteVta = q.IdCbteVta,

                                 vt_NumFactura = q.vt_NumFactura,
                                 vt_fecha = q.vt_fecha,
                                 NomContacto = q.Nombres,
                                 Ve_Vendedor = q.Ve_Vendedor,
                                 vt_Subtotal0 = q.vt_Subtotal0,
                                 vt_SubtotalIVA = q.vt_SubtotalIVA,
                                 vt_iva = q.vt_iva,
                                 vt_total = q.vt_total,
                                 Estado = q.Estado,
                                 esta_impresa = q.esta_impresa,

                                 IdEmpresa_in_eg_x_inv = q.IdEmpresa_in_eg_x_inv,
                                 IdSucursal_in_eg_x_inv = q.IdSucursal_in_eg_x_inv,
                                 IdMovi_inven_tipo_in_eg_x_inv = q.IdMovi_inven_tipo_in_eg_x_inv,
                                 IdNumMovi_in_eg_x_inv = q.IdNumMovi_in_eg_x_inv,

                                 vt_autorizacion = q.vt_autorizacion,
                                 Fecha_Autorizacion = q.Fecha_Autorizacion,
                                 NombresAlumno = q.NombresAlumno,
                                 EstadoBool = q.Estado == "A" ? true : false,

                                 IdUsuario=q.IdUsuario
                             }).ToList();
                }
                */
                return Lista;
            }
            catch (Exception ex)
            {
                tb_LogError_Data LogData = new tb_LogError_Data();
                LogData.GuardarDB(new tb_LogError_Info { Descripcion = ex.Message, InnerException = ex.InnerException == null ? null : ex.InnerException.Message, Clase = "fa_factura_Data", Metodo = "get_list", IdUsuario = "consulta" });
                return new List<fa_factura_consulta_Info>();
            }
        }
        public fa_factura_Info get_info(int IdEmpresa, int IdSucursal, int IdBodega, decimal IdCbteVta)
        {
            try
            {
                fa_factura_Info info = new fa_factura_Info();
                using (EntitiesFacturacion Context = new EntitiesFacturacion())
                {
                    fa_factura Entity = Context.fa_factura.FirstOrDefault(q => q.IdEmpresa == IdEmpresa && q.IdSucursal == IdSucursal && q.IdBodega == IdBodega && q.IdCbteVta == IdCbteVta);
                    if (Entity == null) return null;
                    info = new fa_factura_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdSucursal = Entity.IdSucursal,
                        IdBodega = Entity.IdBodega,
                        IdCbteVta = Entity.IdCbteVta,
                        CodCbteVta = Entity.CodCbteVta,
                        vt_tipoDoc = Entity.vt_tipoDoc,
                        vt_serie1 = Entity.vt_serie1,
                        vt_serie2 = Entity.vt_serie2,
                        vt_NumFactura = Entity.vt_NumFactura,
                        Fecha_Autorizacion = Entity.fecha_primera_cuota,
                        vt_autorizacion = Entity.vt_autorizacion,
                        vt_fecha = Entity.vt_fecha,
                        vt_fech_venc = Entity.vt_fech_venc,
                        IdCliente = Entity.IdCliente,
                        IdVendedor = Entity.IdVendedor,
                        vt_plazo = Entity.vt_plazo,
                        vt_Observacion = Entity.vt_Observacion,
                        vt_tipo_venta = Entity.vt_tipo_venta,
                        IdCaja = Entity.IdCaja,
                        IdPuntoVta = Entity.IdPuntoVta,
                        fecha_primera_cuota = Entity.fecha_primera_cuota,
                        Fecha_Transaccion = Entity.fecha_primera_cuota,
                        Estado = Entity.Estado,
                        esta_impresa = Entity.esta_impresa,
                        valor_abono = Entity.valor_abono,
                        IdNivel = Entity.IdNivel,
                        IdCatalogo_FormaPago = Entity.IdCatalogo_FormaPago,
                        IdAlumno = Entity.IdAlumno,
                        IdEmpresa_rol=Entity.IdEmpresa_rol,
                        IdEmpleado = Entity.IdEmpleado,
                        AplicacionMasiva = Entity.AplicacionMasiva
                    };

                    info.info_resumen = Context.fa_factura_resumen.Where(q => q.IdEmpresa == IdEmpresa && q.IdSucursal == IdSucursal && q.IdBodega == IdBodega && q.IdCbteVta == IdCbteVta).Select(q => new fa_factura_resumen_Info
                    {
                        IdEmpresa = q.IdEmpresa,
                        IdSucursal = q.IdSucursal,
                        IdBodega = q.IdBodega,
                        IdCbteVta = q.IdCbteVta,
                        SubtotalConDscto = q.SubtotalConDscto,
                        SubtotalIVAConDscto = q.SubtotalIVAConDscto,
                        SubtotalIVASinDscto = q.SubtotalIVASinDscto,
                        SubtotalSinDscto = q.SubtotalSinDscto,
                        SubtotalSinIVAConDscto = q.SubtotalSinIVAConDscto,
                        SubtotalSinIVASinDscto = q.SubtotalSinIVASinDscto,
                        Total = q.Total,
                        ValorEfectivo = q.ValorEfectivo,
                        Descuento = q.Descuento,
                        ValorIVA = q.ValorIVA,
                        Cambio = q.Cambio
                    }).FirstOrDefault();

                    info.info_resumen = info.info_resumen ?? new fa_factura_resumen_Info();
                }
                return info;
            }
            catch (Exception ex)
            {
                tb_LogError_Data LogData = new tb_LogError_Data();
                LogData.GuardarDB(new tb_LogError_Info { Descripcion = ex.Message, InnerException = ex.InnerException == null ? null : ex.InnerException.Message, Clase = "fa_factura_Data", Metodo = "get_info" });
                return new fa_factura_Info();
            }
        }

        public fa_factura_Info GetCtaCbleDebe(int IdEmpresa, int IdSucursal, int IdBodega, decimal IdCbteVta)
        {
            try
            {
                fa_factura_Info info = new fa_factura_Info();
                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = "select a.IdEmpresa, a.IdSucursal, a.IdBodega, a.IdCbteVta, a.vt_tipoDoc,case when d.EnCurso = 1 or (x.FechaDesde < d.FechaDesde) then e.IdCtaCbleDebe else d.IdCtaCbleCierre end as IdCtaCbleDebe, a.vt_serie1+'-'+a.vt_serie2+'-'+a.vt_NumFactura vt_NumFactura"
                                        + " from fa_factura  as a with(nolock) join"
                                        + " fa_factura_resumen as b with(nolock) on a.IdEmpresa = b.IdEmpresa and a.IdSucursal = b.IdSucursal and a.IdBodega = b.IdBodega and a.IdCbteVta = b.IdCbteVta join"
                                        + " aca_Matricula_Rubro as c with(nolock) on b.IdEmpresa = c.IdEmpresa and b.IdSucursal = c.IdSucursal and b.IdBodega = c.IdBodega and b.IdCbteVta = c.IdCbteVta join"
                                        + " aca_AnioLectivo as d with(nolock) on c.IdEmpresa = d.IdEmpresa and c.IdAnio = d.IdAnio join"
                                        + " aca_AnioLectivo_Curso_Plantilla_Parametrizacion as e with(nolock) on c.IdEmpresa = e.IdEmpresa and c.IdAnio = e.IdAnio and c.IdSede = e.IdSede and c.IdNivel = e.IdNivel and c.IdJornada = e.IdJornada and c.IdCurso = e.IdCurso and c.IdPlantilla = e.IdPlantilla and c.IdRubro = e.IdRubro left join"
                                        + " aca_AnioLectivo as x with(nolock) on x.IdEmpresa = a.IdEmpresa and x.EnCurso = 1"
                                        + " where a.IdEmpresa = " + IdEmpresa.ToString() + " and a.IdSucursal = " + IdSucursal.ToString() + " and a.IdBodega = " + IdBodega.ToString() + " and a.IdCbteVta = " + IdCbteVta.ToString()
                                        + " UNION ALL"
                                        + " select a.IdEmpresa, a.IdSucursal, a.IdBodega, a.IdCbteVta, a.vt_tipoDoc, case when f.EnCurso = 1 or (x.FechaDesde < f.FechaDesde) then e.IdCtaCbleDebe else f.IdCtaCbleCierre end as IdCtaCbleDebe, a.vt_serie1+'-'+a.vt_serie2+'-'+a.vt_NumFactura vt_NumFactura "
                                        + " from fa_factura as a with(nolock) left join"
                                        + " aca_Matricula_Rubro as b with(nolock) on a.IdEmpresa = b.IdEmpresa and a.IdSucursal = b.IdSucursal and a.IdBodega = b.IdBodega and a.IdCbteVta = b.IdCbteVta JOIN"
                                        + " fa_factura_resumen as c with(nolock) on a.IdEmpresa = c.IdEmpresa and a.IdSucursal = c.IdSucursal and a.IdBodega = c.IdBodega and a.IdCbteVta = c.IdCbteVta left join"
                                        + " aca_Matricula as d with(nolock) on a.IdEmpresa = d.IdEmpresa and a.IdAlumno = d.IdAlumno and c.IdEmpresa = d.IdEmpresa and c.IdAnio = d.IdAnio left join"
                                        + " aca_AnioLectivo_Curso_Plantilla_Parametrizacion as e with(nolock) on d.IdEmpresa = e.IdEmpresa and d.IdAnio = e.IdAnio and d.IdSede = e.IdSede and d.IdNivel = e.IdNivel and d.IdJornada = e.IdJornada and d.IdCurso = e.IdCurso and d.IdPlantilla = e.IdPlantilla and c.IdRubro = e.IdRubro left join"
                                        + " aca_AnioLectivo as f with(nolock) on e.IdEmpresa = f.IdEmpresa and e.IdAnio = f.IdAnio left join "
                                        + " aca_AnioLectivo as x with (nolock) on x.IdEmpresa = a.IdEmpresa and x.EnCurso = 1"
                                        + " where b.IdMatricula is null and a.Estado = 'A' and c.IdAnio is not null"
                                        + " and a.IdEmpresa = " + IdEmpresa.ToString() + " and a.IdSucursal = " + IdSucursal.ToString() + " and a.IdBodega = " + IdBodega.ToString() + " and a.IdCbteVta = " + IdCbteVta.ToString();

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        info = new fa_factura_Info
                        {
                            IdCtaCble = reader["IdCtaCbleDebe"].ToString(),
                            vt_NumFactura = reader["vt_NumFactura"].ToString()
                        };                        
                    }
                    reader.Close();
                }
                return info;
            }
            catch (Exception ex)
            {
                tb_LogError_Data LogData = new tb_LogError_Data();
                LogData.GuardarDB(new tb_LogError_Info { Descripcion = ex.Message, InnerException = ex.InnerException == null ? null : ex.InnerException.Message, Clase = "fa_factura_Data", Metodo = "GetCtaCbleDebe" });
                return new fa_factura_Info();
            }
        }

        public fa_factura_Info get_info(int IdEmpresa, int IdSucursal, int IdBodega, decimal IdAlumno, string Serie1, string Serie2, string NumFact)
        {
            try
            {
                fa_factura_Info info = new fa_factura_Info();
                using (EntitiesFacturacion Context = new EntitiesFacturacion())
                {
                    fa_factura Entity = Context.fa_factura.FirstOrDefault(q => q.IdEmpresa == IdEmpresa && q.IdSucursal == IdSucursal && q.IdBodega == IdBodega
                    && q.vt_serie1 == Serie1 && q.vt_serie2 == Serie2 && q.vt_NumFactura == NumFact && q.Estado == "A");
                    if (Entity == null) return null;
                    info = new fa_factura_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdSucursal = Entity.IdSucursal,
                        IdBodega = Entity.IdBodega,
                        IdCbteVta = Entity.IdCbteVta,
                        CodCbteVta = Entity.CodCbteVta,
                        vt_tipoDoc = Entity.vt_tipoDoc,
                        vt_serie1 = Entity.vt_serie1,
                        vt_serie2 = Entity.vt_serie2,
                        vt_NumFactura = Entity.vt_NumFactura,
                        Fecha_Autorizacion = Entity.fecha_primera_cuota,
                        vt_autorizacion = Entity.vt_autorizacion,
                        vt_fecha = Entity.vt_fecha,
                        vt_fech_venc = Entity.vt_fech_venc,
                        IdCliente = Entity.IdCliente,
                        IdVendedor = Entity.IdVendedor,
                        vt_plazo = Entity.vt_plazo,
                        vt_Observacion = Entity.vt_Observacion,
                        vt_tipo_venta = Entity.vt_tipo_venta,
                        IdCaja = Entity.IdCaja,
                        IdPuntoVta = Entity.IdPuntoVta,
                        fecha_primera_cuota = Entity.fecha_primera_cuota,
                        Fecha_Transaccion = Entity.fecha_primera_cuota,
                        Estado = Entity.Estado,
                        esta_impresa = Entity.esta_impresa,
                        valor_abono = Entity.valor_abono,
                        IdNivel = Entity.IdNivel,
                        IdCatalogo_FormaPago = Entity.IdCatalogo_FormaPago,
                        IdAlumno = Entity.IdAlumno,
                        IdEmpresa_rol = Entity.IdEmpresa_rol,
                        IdEmpleado = Entity.IdEmpleado,
                        AplicacionMasiva = Entity.AplicacionMasiva
                    };

                    info.info_resumen = info.info_resumen ?? new fa_factura_resumen_Info();
                }
                return info;
            }
            catch (Exception ex)
            {
                tb_LogError_Data LogData = new tb_LogError_Data();
                LogData.GuardarDB(new tb_LogError_Info { Descripcion = ex.Message, InnerException = ex.InnerException == null ? null : ex.InnerException.Message, Clase = "fa_factura_Data", Metodo = "get_info" });
                return new fa_factura_Info();
            }
        }
        private decimal get_id(int IdEmpresa, int IdSucursal, int IdBodega)
        {
            try
            {
                decimal ID = 1;
                using (EntitiesFacturacion Context = new EntitiesFacturacion())
                {
                    var lst = from q in Context.fa_factura
                              where q.IdEmpresa == IdEmpresa
                              && q.IdSucursal == IdSucursal
                              && q.IdBodega == IdBodega
                              select q;
                    if (lst.Count() > 0)
                        ID = lst.Max(q => q.IdCbteVta) + 1;
                }
                return ID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool guardarDB(fa_factura_Info info)
        {
            EntitiesFacturacion db_f = new EntitiesFacturacion();
            EntitiesGeneral db_g = new EntitiesGeneral();
            EntitiesAcademico db_a = new EntitiesAcademico();
            try
            {
                #region Variables
                int secuencia = 1;
                ct_cbtecble_Data data_ct = new ct_cbtecble_Data();
                #endregion

                #region Cabecera
                var factura = new fa_factura
                {
                    IdEmpresa = info.IdEmpresa,
                    IdSucursal = info.IdSucursal,
                    IdBodega = info.IdBodega,
                    IdCbteVta = info.IdCbteVta = get_id(info.IdEmpresa, info.IdSucursal, info.IdBodega),
                    CodCbteVta = info.CodCbteVta,
                    vt_tipoDoc = info.vt_tipoDoc,
                    vt_serie1 = info.vt_serie1,
                    vt_serie2 = info.vt_serie2,
                    vt_NumFactura = info.vt_NumFactura,
                    Fecha_Autorizacion = info.Fecha_Autorizacion,
                    vt_autorizacion = info.vt_autorizacion,
                    vt_fecha = info.vt_fecha.Date,
                    vt_fech_venc = info.vt_fech_venc.Date,
                    IdCliente = info.IdCliente,
                    IdVendedor = info.IdVendedor,
                    vt_plazo = info.vt_plazo,
                    vt_Observacion = string.IsNullOrEmpty(info.vt_Observacion) ? "" : info.vt_Observacion,
                    IdCatalogo_FormaPago = info.IdCatalogo_FormaPago,
                    vt_tipo_venta = info.vt_tipo_venta,
                    IdCaja = info.IdCaja,
                    IdPuntoVta = info.IdPuntoVta,
                    fecha_primera_cuota = info.fecha_primera_cuota,
                    Fecha_Transaccion = DateTime.Now,
                    Estado = info.Estado = "A",
                    esta_impresa = info.esta_impresa,
                    valor_abono = info.valor_abono,
                    IdUsuario = info.IdUsuario,
                    IdNivel = info.IdNivel,
                    IdAlumno = info.IdAlumno,
                    IdEmpresa_rol= info.IdEmpresa_rol,
                    IdEmpleado=info.IdEmpleado,
                    AplicacionMasiva = info.AplicacionMasiva
                };
                #endregion

                #region Detalle
                foreach (var item in info.lst_det)
                {
                    db_f.fa_factura_det.Add(new fa_factura_det
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdSucursal = info.IdSucursal,
                        IdBodega = info.IdBodega,
                        IdCbteVta = info.IdCbteVta,
                        Secuencia = item.Secuencia = secuencia++,

                        IdProducto = item.IdProducto,
                        vt_cantidad = item.vt_cantidad,
                        vt_Precio = item.vt_Precio,
                        vt_PorDescUnitario = item.vt_PorDescUnitario,
                        vt_DescUnitario = item.vt_DescUnitario,
                        vt_PrecioFinal = item.vt_PrecioFinal,
                        vt_Subtotal = item.vt_Subtotal,
                        vt_por_iva = item.vt_por_iva,
                        IdCod_Impuesto_Iva = item.IdCod_Impuesto_Iva,
                        vt_iva = item.vt_iva,
                        vt_total = item.vt_total,
                        vt_detallexItems = item.vt_detallexItems,

                        IdEmpresa_pf = item.IdEmpresa_pf,
                        IdSucursal_pf = item.IdSucursal_pf,
                        IdProforma = item.IdProforma,
                        Secuencia_pf = item.Secuencia_pf,

                        IdCentroCosto = item.IdCentroCosto,
                        IdPunto_Cargo = item.IdPunto_Cargo,
                        IdPunto_cargo_grupo = item.IdPunto_cargo_grupo
                    });

                    #region MatriculaRubro
                    aca_Matricula_Rubro Entity_MatricularRubro = db_a.aca_Matricula_Rubro.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdMatricula == item.IdMatricula
                    && q.IdPeriodo == item.aca_IdPeriodo && q.IdRubro == item.aca_IdRubro);
                    if (Entity_MatricularRubro != null)
                    {
                        Entity_MatricularRubro.IdSucursal = info.IdSucursal;
                        Entity_MatricularRubro.IdBodega = info.IdBodega;
                        Entity_MatricularRubro.IdCbteVta = info.IdCbteVta;
                        Entity_MatricularRubro.FechaFacturacion = info.vt_fecha.Date;
                    }

                    db_a.SaveChanges();
                    #endregion
                }
                #endregion

                #region Resumen
                db_f.fa_factura_resumen.Add(new fa_factura_resumen
                {
                    IdEmpresa = info.IdEmpresa,
                    IdSucursal = info.IdSucursal,
                    IdBodega = info.IdBodega,
                    IdCbteVta = info.IdCbteVta,

                    SubtotalConDscto = info.info_resumen.SubtotalConDscto,
                    SubtotalIVAConDscto = info.info_resumen.SubtotalIVAConDscto,
                    SubtotalIVASinDscto = info.info_resumen.SubtotalIVASinDscto,
                    SubtotalSinDscto = info.info_resumen.SubtotalSinDscto,
                    SubtotalSinIVAConDscto = info.info_resumen.SubtotalSinIVAConDscto,
                    SubtotalSinIVASinDscto = info.info_resumen.SubtotalSinIVASinDscto,

                    Total = info.info_resumen.Total,
                    Descuento = info.info_resumen.Descuento,
                    ValorEfectivo = info.info_resumen.ValorEfectivo,
                    ValorIVA = info.info_resumen.ValorIVA,
                    Cambio = info.info_resumen.Cambio,

                    ValorProntoPago = info.info_resumen.ValorProntoPago,
                    IdRubro = info.lst_det.Where(q => q.aca_IdRubro != null).ToList().Count == 0 ? null : info.lst_det.Max(q => q.aca_IdRubro),
                    IdPeriodo = info.lst_det.Where(q => q.aca_IdPeriodo != null).ToList().Count == 0 ? null : info.lst_det.Max(q => q.aca_IdPeriodo),
                    IdPlantilla = info.lst_det.Where(q => q.aca_IdPlantilla != null).ToList().Count == 0 ? null : info.lst_det.Max(q => q.aca_IdPlantilla),
                    IdAnio = info.lst_det.Where(q => q.aca_IdPlantilla != null).ToList().Count == 0 ? null : info.lst_det.Max(q => q.aca_IdAnio)
                });
                #endregion

                var cliente = db_f.fa_cliente.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdCliente == info.IdCliente).FirstOrDefault();
                var persona = db_g.tb_persona.Where(q => q.IdPersona == cliente.IdPersona).FirstOrDefault();

                #region Talonario
                //var tal = odata_tal.GetUltimoNoUsadoFacElec(info.IdEmpresa, info.vt_tipoDoc, info.vt_serie1, info.vt_serie2);
                //if (tal != null)
                //    factura.vt_NumFactura = info.vt_NumFactura = tal.NumDocumento;

                fa_PuntoVta_Data data_puntovta = new fa_PuntoVta_Data();
                tb_sis_Documento_Tipo_Talonario_Data data_talonario = new tb_sis_Documento_Tipo_Talonario_Data();
                fa_PuntoVta_Info info_puntovta = new fa_PuntoVta_Info();
                tb_sis_Documento_Tipo_Talonario_Info ultimo_talonario = new tb_sis_Documento_Tipo_Talonario_Info();
                tb_sis_Documento_Tipo_Talonario_Info info_talonario = new tb_sis_Documento_Tipo_Talonario_Info();
                info_puntovta = data_puntovta.get_info(info.IdEmpresa, info.IdSucursal, info.IdPuntoVta??0);

                if (info_puntovta != null)
                {
                    if (info_puntovta.EsElectronico == true)
                    {
                        ultimo_talonario = data_talonario.GetUltimoNoUsado(info.IdEmpresa, info_puntovta.codDocumentoTipo, info_puntovta.Su_CodigoEstablecimiento, info_puntovta.cod_PuntoVta, info_puntovta.EsElectronico,true);

                        if (ultimo_talonario != null)
                        {
                            factura.vt_serie1 = info.vt_serie1 = ultimo_talonario.Establecimiento;
                            factura.vt_serie2 = info.vt_serie2 = ultimo_talonario.PuntoEmision;
                            factura.vt_NumFactura = info.vt_NumFactura = ultimo_talonario.NumDocumento;
                        }
                    }
                    else
                    {
                        info_talonario.IdEmpresa = info.IdEmpresa;
                        info_talonario.CodDocumentoTipo = info.vt_tipoDoc;
                        info_talonario.Establecimiento = info.vt_serie1;
                        info_talonario.PuntoEmision = info.vt_serie2;
                        info_talonario.NumDocumento = info.vt_NumFactura;
                        info_talonario.IdSucursal = info.IdSucursal;
                        info_talonario.Usado = true;

                        data_talonario.modificar_estado_usadoDB(info_talonario);
                    }
                }
                #endregion

                db_f.fa_factura.Add(factura);
                db_f.SaveChanges();

                #region Contabilidad
                var parametro = db_f.fa_parametro.Where(q => q.IdEmpresa == info.IdEmpresa).FirstOrDefault();
                
                    ct_cbtecble_Info diario = armar_diario(info, Convert.ToInt32(parametro.IdTipoCbteCble_Factura));
                    if (diario != null)
                        if (data_ct.guardarDB(diario))
                        {
                            db_f.fa_factura_x_ct_cbtecble.Add(new fa_factura_x_ct_cbtecble
                            {
                                vt_IdEmpresa = info.IdEmpresa,
                                vt_IdSucursal = info.IdSucursal,
                                vt_IdBodega = info.IdBodega,
                                vt_IdCbteVta = info.IdCbteVta,

                                ct_IdEmpresa = diario.IdEmpresa,
                                ct_IdTipoCbte = diario.IdTipoCbte,
                                ct_IdCbteCble = diario.IdCbteCble,
                            });
                            db_f.SaveChanges();
                        }
                #endregion

                #region Descuento por rol
                var Nc = ArmarNotaCreditoDescuentoPorRol(info);
                if (Nc != null)
                {
                    if(odatNc.guardarDB(Nc))
                    {

                    }
                }
                #endregion
                
                db_f.Dispose();
                db_a.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                tb_LogError_Data LogData = new tb_LogError_Data();
                LogData.GuardarDB(new tb_LogError_Info { Descripcion = ex.Message, InnerException = ex.InnerException == null ? null : ex.InnerException.Message, Clase = "fa_factura_Data", Metodo = "guardarDB", IdUsuario = info.IdUsuario });
                return false;
            }
        }

        public ct_cbtecble_Info armar_diario(fa_factura_Info info, int IdTipoCbte)
        {
            try
            {
                using (EntitiesFacturacion db = new EntitiesFacturacion())
                {
                    var lstCuentas = db.vwfa_factura_ParaContabilizarAcademico.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdSucursal == info.IdSucursal && q.IdBodega == info.IdBodega && q.IdCbteVta == info.IdCbteVta).FirstOrDefault();
                    if (lstCuentas == null)
                        return null;
                    ct_cbtecble_Info diario = new ct_cbtecble_Info
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdTipoCbte = IdTipoCbte,
                        IdCbteCble = 0,
                        cb_Fecha = info.vt_fecha.Date,
                        IdSucursal = info.IdSucursal,

                        IdUsuario = info.IdUsuario,
                        IdUsuarioUltModi = info.IdUsuarioUltModi,
                        cb_Observacion = "FACT# " + info.vt_serie1 + "-" + info.vt_serie2 + "-" + info.vt_NumFactura + " " + "CLIENTE: " + lstCuentas.NombreCliente + " ALUMNO: " + lstCuentas.NombreAlumno + " " + info.vt_Observacion,
                        CodCbteCble = "FACT# " + info.vt_NumFactura,
                        cb_Valor = 0,
                        lst_ct_cbtecble_det = new List<ct_cbtecble_det_Info>()
                    };
                    int secuencia = 1;

                    #region CXC
                    diario.lst_ct_cbtecble_det.Add(new ct_cbtecble_det_Info
                    {
                        IdCtaCble = lstCuentas.IdCtaCbleDebe,
                        dc_Valor =  Math.Round(Convert.ToDouble(lstCuentas.SubtotalConDscto),2,MidpointRounding.AwayFromZero),
                        secuencia = secuencia++
                    });
                    #endregion

                    #region Venta
                    diario.lst_ct_cbtecble_det.Add(new ct_cbtecble_det_Info
                    {
                        IdCtaCble = lstCuentas.IdCtaCbleHaber,
                        dc_Valor = Math.Round(Convert.ToDouble(lstCuentas.SubtotalConDscto) * -1, 2, MidpointRounding.AwayFromZero),
                        secuencia = secuencia++
                    });
                    #endregion

                    if (info.lst_det.Count == 0)
                        return null;

                    diario.lst_ct_cbtecble_det.RemoveAll(q => q.dc_Valor == 0);

                    if (diario.lst_ct_cbtecble_det.Count == 0)
                        return null;

                    if (diario.lst_ct_cbtecble_det.Where(q => q.dc_Valor == 0).Count() > 0)
                        return null;

                    double descuadre = Math.Round(diario.lst_ct_cbtecble_det.Sum(q => q.dc_Valor), 2, MidpointRounding.AwayFromZero);
                    if (descuadre < -0.02 || 0.02 <= descuadre)
                        return null;

                    if (descuadre <= 0.02 || -0.02 <= descuadre && descuadre != 0)
                    {
                        if (descuadre > 0)
                            diario.lst_ct_cbtecble_det.Where(q => q.dc_Valor < 0).FirstOrDefault().dc_Valor -= descuadre;
                        else
                            diario.lst_ct_cbtecble_det.Where(q => q.dc_Valor > 0).FirstOrDefault().dc_Valor += (descuadre * -1);
                    }

                    descuadre = Math.Round(diario.lst_ct_cbtecble_det.Sum(q => q.dc_Valor), 2, MidpointRounding.AwayFromZero);
                    if (descuadre != 0)
                        return null;

                    return diario;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool modificarDB(fa_factura_Info info)
        {
            EntitiesFacturacion db_f = new EntitiesFacturacion();
            EntitiesGeneral db_g = new EntitiesGeneral();
            try
            {
                #region Variables
                int secuencia = 1;
                ct_cbtecble_Data data_ct = new ct_cbtecble_Data();
                #endregion

                #region Factura

                #region Cabecera
                fa_factura Entity = db_f.fa_factura.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdSucursal == info.IdSucursal && q.IdBodega == info.IdBodega && q.IdCbteVta == info.IdCbteVta);
                if (Entity == null) return false;
                
                Entity.vt_fecha = info.vt_fecha.Date;
                Entity.vt_fech_venc = info.vt_fech_venc.Date;
                Entity.IdCliente = info.IdCliente;
                Entity.IdVendedor = info.IdVendedor;
                Entity.vt_plazo = info.vt_plazo;
                Entity.vt_Observacion = string.IsNullOrEmpty(info.vt_Observacion) ? "" : info.vt_Observacion;
                Entity.IdCatalogo_FormaPago = info.IdCatalogo_FormaPago;
                Entity.vt_tipo_venta = info.vt_tipo_venta;
                Entity.fecha_primera_cuota = info.fecha_primera_cuota;
                Entity.valor_abono = info.valor_abono;
                Entity.IdNivel = info.IdNivel;
                Entity.IdUsuarioUltModi = info.IdUsuarioUltModi;
                Entity.Fecha_UltMod = DateTime.Now;
                Entity.IdEmpresa_rol = info.IdEmpresa_rol;
                Entity.IdEmpleado = info.IdEmpleado;
                #endregion
                var cliente = db_f.fa_cliente.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdCliente == info.IdCliente).FirstOrDefault();
                var persona = db_g.tb_persona.Where(q => q.IdPersona == cliente.IdPersona).FirstOrDefault();

                #region Resumen
                var resu = db_f.fa_factura_resumen.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdSucursal == info.IdSucursal && q.IdBodega == info.IdBodega && q.IdCbteVta == info.IdCbteVta).FirstOrDefault();
                if(resu != null)
                    db_f.fa_factura_resumen.Remove(resu);

                db_f.fa_factura_resumen.Add(new fa_factura_resumen
                {
                    IdEmpresa = info.IdEmpresa,
                    IdSucursal = info.IdSucursal,
                    IdBodega = info.IdBodega,
                    IdCbteVta = info.IdCbteVta,

                    SubtotalConDscto = info.info_resumen.SubtotalConDscto,
                    SubtotalIVAConDscto = info.info_resumen.SubtotalIVAConDscto,
                    SubtotalIVASinDscto = info.info_resumen.SubtotalIVASinDscto,
                    SubtotalSinDscto = info.info_resumen.SubtotalSinDscto,
                    SubtotalSinIVAConDscto = info.info_resumen.SubtotalSinIVAConDscto,
                    SubtotalSinIVASinDscto = info.info_resumen.SubtotalSinIVASinDscto,

                    Total = info.info_resumen.Total,
                    Descuento = info.info_resumen.Descuento,
                    ValorEfectivo = info.info_resumen.ValorEfectivo,
                    ValorIVA = info.info_resumen.ValorIVA,
                    Cambio = info.info_resumen.Cambio,

                    ValorProntoPago = info.info_resumen.ValorProntoPago,
                    IdRubro = info.lst_det.Where(q => q.aca_IdRubro != null).ToList().Count == 0 ? null : info.lst_det.Max(q => q.aca_IdRubro),
                    IdPeriodo = info.lst_det.Where(q => q.aca_IdPeriodo != null).ToList().Count == 0 ? null : info.lst_det.Max(q => q.aca_IdPeriodo),
                    IdPlantilla = info.lst_det.Where(q => q.aca_IdPlantilla != null).ToList().Count == 0 ? null : info.lst_det.Max(q => q.aca_IdPlantilla),
                    IdAnio = info.lst_det.Where(q => q.aca_IdPlantilla != null).ToList().Count == 0 ? null : info.lst_det.Max(q => q.aca_IdAnio)
                });
                #endregion

                #region Detalle
                var lst_det = db_f.fa_factura_det.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdSucursal == info.IdSucursal && q.IdBodega == info.IdBodega && q.IdCbteVta == info.IdCbteVta).ToList();
                db_f.fa_factura_det.RemoveRange(lst_det);

                foreach (var item in info.lst_det)
                {
                    db_f.fa_factura_det.Add(new fa_factura_det
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdSucursal = info.IdSucursal,
                        IdBodega = info.IdBodega,
                        IdCbteVta = info.IdCbteVta,
                        Secuencia = item.Secuencia = secuencia++,

                        IdProducto = item.IdProducto,
                        vt_cantidad = item.vt_cantidad,
                        vt_Precio = item.vt_Precio,
                        vt_PorDescUnitario = item.vt_PorDescUnitario,
                        vt_DescUnitario = item.vt_DescUnitario,
                        vt_PrecioFinal = item.vt_PrecioFinal,
                        vt_Subtotal = item.vt_Subtotal,
                        vt_por_iva = item.vt_por_iva,
                        IdCod_Impuesto_Iva = item.IdCod_Impuesto_Iva,
                        vt_iva = item.vt_iva,
                        vt_total = item.vt_total,

                        IdEmpresa_pf = item.IdEmpresa_pf,
                        IdSucursal_pf = item.IdSucursal_pf,
                        IdProforma = item.IdProforma,
                        Secuencia_pf = item.Secuencia_pf,

                        IdCentroCosto = item.IdCentroCosto,
                        IdPunto_Cargo = item.IdPunto_Cargo,
                        IdPunto_cargo_grupo = item.IdPunto_cargo_grupo,
                    });
                }
                #endregion
                #endregion
                db_f.SaveChanges();

                #region Contabilidad
                var parametro = db_f.fa_parametro.Where(q => q.IdEmpresa == info.IdEmpresa).FirstOrDefault();
                //if (!string.IsNullOrEmpty(cliente.IdCtaCble_cxc_Credito))
                {
                    var conta = db_f.fa_factura_x_ct_cbtecble.Where(q => q.vt_IdEmpresa == info.IdEmpresa && q.vt_IdSucursal == info.IdSucursal && q.vt_IdBodega == info.IdBodega && q.vt_IdCbteVta == info.IdCbteVta).FirstOrDefault();
                    if (conta == null)
                    {
                        ct_cbtecble_Info diario = armar_diario(info, Convert.ToInt32(parametro.IdTipoCbteCble_Factura));
                        if (diario != null)
                        {
                            if (data_ct.guardarDB(diario))
                            {
                                db_f.fa_factura_x_ct_cbtecble.Add(new fa_factura_x_ct_cbtecble
                                {
                                    vt_IdEmpresa = info.IdEmpresa,
                                    vt_IdSucursal = info.IdSucursal,
                                    vt_IdBodega = info.IdBodega,
                                    vt_IdCbteVta = info.IdCbteVta,

                                    ct_IdEmpresa = diario.IdEmpresa,
                                    ct_IdTipoCbte = diario.IdTipoCbte,
                                    ct_IdCbteCble = diario.IdCbteCble,
                                });
                                db_f.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        ct_cbtecble_Info diario = armar_diario(info, Convert.ToInt32(parametro.IdTipoCbteCble_Factura));
                        if (diario != null)
                        {
                            diario.IdCbteCble = conta.ct_IdCbteCble;
                            data_ct.modificarDB(diario);
                        }
                    }
                }
                #endregion

                db_f.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                tb_LogError_Data LogData = new tb_LogError_Data();
                LogData.GuardarDB(new tb_LogError_Info { Descripcion = ex.Message, InnerException = ex.InnerException == null ? null : ex.InnerException.Message, Clase = "fa_factura_Data", Metodo = "modificarDB", IdUsuario = info.IdUsuario });
                return false;
            }
        }

        public bool modificarEstadoImpresion(int IdEmpresa, int IdSucursal, int IdBodega, decimal IdCbteVta, bool estado_impresion)
        {
            try
            {
                using (EntitiesFacturacion Context = new EntitiesFacturacion())
                {
                    var Entity = Context.fa_factura.Where(q => q.IdEmpresa == IdEmpresa && q.IdSucursal == IdSucursal && q.IdBodega == IdBodega && q.IdCbteVta == IdCbteVta).FirstOrDefault();
                    if (Entity != null)
                    {
                        Entity.esta_impresa = estado_impresion;
                        Context.SaveChanges();
                    }
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool anularDB(fa_factura_Info info)
        {
            try
            {
                #region Variables
                ct_cbtecble_Data odata_ct = new ct_cbtecble_Data();
                #endregion

                using (EntitiesFacturacion Context = new EntitiesFacturacion())
                {
                    fa_factura Entity = Context.fa_factura.FirstOrDefault(q => q.IdEmpresa == info.IdEmpresa && q.IdSucursal == info.IdSucursal && q.IdBodega == info.IdBodega && q.IdCbteVta == info.IdCbteVta);
                    if (Entity == null) return false;
                    {
                        Entity.MotivoAnulacion = info.MotivoAnulacion;
                        Entity.IdUsuarioUltAnu = info.IdUsuarioUltAnu;
                        Entity.Estado = "I";
                    }
                    
                    var conta = Context.fa_factura_x_ct_cbtecble.Where(q => q.vt_IdEmpresa == info.IdEmpresa && q.vt_IdSucursal == info.IdSucursal && q.vt_IdBodega == info.IdBodega && q.vt_IdCbteVta == info.IdCbteVta).FirstOrDefault();
                    if (conta != null)
                        if (!odata_ct.anularDB(new ct_cbtecble_Info { IdEmpresa = conta.ct_IdEmpresa, IdTipoCbte = conta.ct_IdTipoCbte, IdCbteCble = conta.ct_IdCbteCble, IdUsuarioAnu = info.IdUsuarioUltAnu, cb_MotivoAnu = info.MotivoAnulacion }))
                        {
                            Entity.MotivoAnulacion = null;
                            Entity.IdUsuarioUltAnu = null;
                            Entity.Estado = "A";
                        }

                    Context.SaveChanges();
                }

                using (EntitiesAcademico db = new EntitiesAcademico())
                {
                    var RubroFacturado = db.aca_Matricula_Rubro.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdSucursal == info.IdSucursal && q.IdBodega == info.IdBodega && q.IdCbteVta == info.IdCbteVta).FirstOrDefault();
                    if (RubroFacturado != null)
                    {
                        RubroFacturado.IdSucursal = null;
                        RubroFacturado.IdBodega = null;
                        RubroFacturado.IdCbteVta = null;
                        RubroFacturado.FechaFacturacion = null;
                        RubroFacturado.EnMatricula = false;
                        db.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool MostrarCuotasRpt(int IdEmpresa, int IdSucursal, int IdBodega, decimal IdCbteVta)
        {
            try
            {
                using (EntitiesFacturacion Context = new EntitiesFacturacion())
                {
                    var fa = (from f in Context.fa_factura
                              where f.IdEmpresa == IdEmpresa
                              && f.IdSucursal == IdSucursal
                              && f.IdBodega == IdBodega
                              && f.IdCbteVta == IdCbteVta
                             join t in Context.fa_TerminoPago
                             on new { IdTerminoPago = f.vt_tipo_venta } equals new { t.IdTerminoPago }
                             select new
                             {
                                 Num_Coutas = t.Num_Coutas
                             }).FirstOrDefault();
                    if (fa.Num_Coutas > 0)
                        return true;
                }
                return false;
               
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool Contabilizar(int IdEmpresa, int IdSucursal, int IdBodega, decimal IdCbteVta, string NombreContacto)
        {
            EntitiesFacturacion db = new EntitiesFacturacion();
            ct_cbtecble_Data data_ct = new ct_cbtecble_Data();
            try
            {
                var factura = get_info(IdEmpresa, IdSucursal, IdBodega, IdCbteVta);
                if (factura != null)
                {
                    fa_factura_det_Data odata_det = new fa_factura_det_Data();
                    factura.lst_det = odata_det.get_list(factura.IdEmpresa, IdSucursal, IdBodega, IdCbteVta);
                }
                var parametro = db.fa_parametro.Where(q => q.IdEmpresa == factura.IdEmpresa).FirstOrDefault();
                
                    var conta = db.fa_factura_x_ct_cbtecble.Where(q => q.vt_IdEmpresa == factura.IdEmpresa && q.vt_IdSucursal == factura.IdSucursal && q.vt_IdBodega == factura.IdBodega && q.vt_IdCbteVta == factura.IdCbteVta).FirstOrDefault();
                    if (conta == null)
                    {
                        ct_cbtecble_Info diario = armar_diario(factura, Convert.ToInt32(parametro.IdTipoCbteCble_Factura));
                        if (diario != null)
                        {
                            if (data_ct.guardarDB(diario))
                            {
                                db.fa_factura_x_ct_cbtecble.Add(new fa_factura_x_ct_cbtecble
                                {
                                    vt_IdEmpresa = factura.IdEmpresa,
                                    vt_IdSucursal = factura.IdSucursal,
                                    vt_IdBodega = factura.IdBodega,
                                    vt_IdCbteVta = factura.IdCbteVta,

                                    ct_IdEmpresa = diario.IdEmpresa,
                                    ct_IdTipoCbte = diario.IdTipoCbte,
                                    ct_IdCbteCble = diario.IdCbteCble,
                                });
                                db.SaveChanges();
                                return true;
                            }
                        }
                    }
                    else
                    {
                        ct_cbtecble_Info diario = armar_diario(factura, Convert.ToInt32(parametro.IdTipoCbteCble_Factura));
                        if (diario != null)
                        {
                            diario.IdCbteCble = conta.ct_IdCbteCble;
                            data_ct.modificarDB(diario);
                            return true;
                        }
                    }
                

                db.Dispose();
                return false;
            }
            catch (Exception)
            {
                db.Dispose();
                throw;
            }
        }

        public bool modificarEstadoAutorizacion(int IdEmpresa, int IdSucursal, int IdBodega, decimal IdCbteVta)
        {
            try
            {
                using (EntitiesFacturacion Context = new EntitiesFacturacion())
                {
                    var Entity = Context.fa_factura.Where(q => q.IdEmpresa == IdEmpresa && q.IdSucursal == IdSucursal && q.IdBodega == IdBodega && q.IdCbteVta == IdCbteVta).FirstOrDefault();
                    if (Entity != null)
                    {
                        Entity.aprobada_enviar_sri = true;
                        Context.SaveChanges();
                    }
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ValidarDocumentoAnulacion(int IdEmpresa, int IdSucursal, int IdBodega, decimal IdCbteVta, string vt_tipoDoc, ref string mensaje)
        {
            try
            {
                using (EntitiesFacturacion db = new EntitiesFacturacion())
                {
                    var obj = db.vwfa_factura_sin_automatico.Where(q => q.IdEmpresa == IdEmpresa && q.IdSucursal == IdSucursal && q.IdBodega_Cbte == IdBodega && q.IdCbte_vta_nota == IdCbteVta && q.dc_TipoDocumento == vt_tipoDoc && q.estado == "A").Count();
                    if (obj > 0)
                    {
                        mensaje = "El documento no puede ser anulado porque se encuentra parcial o totalmente cobrado";
                        return false;
                    }
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public fa_notaCreDeb_Info ArmarNotaCreditoDescuentoPorRol(fa_factura_Info info)
        {
            try
            {
                EntitiesCuentasPorCobrar dbCxc = new EntitiesCuentasPorCobrar();
                EntitiesFacturacion dbFac = new EntitiesFacturacion();
                EntitiesAcademico dbACA = new EntitiesAcademico();

                var TerminoPago = dbFac.fa_TerminoPago.Where(q => q.IdTerminoPago == info.vt_tipo_venta).FirstOrDefault();
                if (TerminoPago == null)
                    return null;

                if (!(TerminoPago.AplicaDescuentoNomina ?? false))
                    return null;

                var MecanismoPago = dbACA.aca_MecanismoDePago.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdTerminoPago == info.vt_tipo_venta).FirstOrDefault();
                if (MecanismoPago == null || MecanismoPago.IdTerminoPago == null)
                    return null;

                var TipoNota = dbFac.fa_TipoNota.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdTipoNota == MecanismoPago.IdTipoNotaDescuentoPorRol).FirstOrDefault();
                if (TipoNota == null)
                    return null;

                if (TipoNota.IdProducto == null)
                    return null;
                
                var PuntoVta = dbFac.vwfa_PuntoVta.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdSucursal == info.IdSucursal && q.IdPuntoVta == info.IdPuntoVta).FirstOrDefault();
                if (PuntoVta == null)
                    return null;

                PuntoVta = dbFac.vwfa_PuntoVta.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdSucursal == info.IdSucursal && q.cod_PuntoVta == PuntoVta.cod_PuntoVta && q.codDocumentoTipo == "NTCR").FirstOrDefault();
                if (PuntoVta == null)
                    return null;
                
                fa_notaCreDeb_Info Retorno = new fa_notaCreDeb_Info
                {
                    IdEmpresa = info.IdEmpresa,
                    IdSucursal = info.IdSucursal,
                    IdBodega = info.IdBodega,
                    IdPuntoVta = PuntoVta.IdPuntoVta,
                    CodNota = "DsctoPorRol",
                    CreDeb = "C",
                    CodDocumentoTipo = "NTCR",
                    Serie1 = null,
                    Serie2 = null,
                    NumAutorizacion = null,
                    NumNota_Impresa = null,
                    Fecha_Autorizacion = null,
                    IdCliente = info.IdCliente,
                    IdAlumno = info.IdAlumno,
                    no_fecha = DateTime.Now.Date,
                    no_fecha_venc = DateTime.Now.Date,
                    IdTipoNota = TipoNota.IdTipoNota,
                    sc_observacion = "NC por dscto. por rol " + info.vt_tipo_venta + " "+ info.vt_NumFactura + " " + info.vt_Observacion,
                    NaturalezaNota = "INT",
                    IdCtaCble_TipoNota = TipoNota.IdCtaCble,
                    IdCobro_tipo = "NTCR",
                    IdUsuario = info.IdUsuario,
                    IdUsuarioUltMod = info.IdUsuarioUltModi,
                    info_resumen = new fa_notaCreDeb_resumen_Info(),
                    lst_det = new List<fa_notaCreDeb_det_Info>(),
                    lst_cruce = new List<fa_notaCreDeb_x_fa_factura_NotaDeb_Info>()
                };

                Retorno.lst_det.Add(new fa_notaCreDeb_det_Info
                {
                    IdProducto = TipoNota.IdProducto ?? 0,
                    sc_cantidad = 1,
                    sc_cantidad_factura = null,
                    sc_Precio = Convert.ToDouble(info.info_resumen.ValorProntoPago),
                    sc_precioFinal = Convert.ToDouble(info.info_resumen.ValorProntoPago),
                    sc_descUni = 0,
                    sc_PordescUni = 0,
                    vt_por_iva = 0,
                    IdCod_Impuesto_Iva = "IVA0",
                    sc_iva = 0,
                    sc_subtotal = Convert.ToDouble(info.info_resumen.ValorProntoPago),
                    sc_total = Convert.ToDouble(info.info_resumen.ValorProntoPago)
                });

                Retorno.lst_cruce.Add(new fa_notaCreDeb_x_fa_factura_NotaDeb_Info
                {
                    IdEmpresa_fac_nd_doc_mod = info.IdEmpresa,
                    IdSucursal_fac_nd_doc_mod = info.IdSucursal,
                    IdBodega_fac_nd_doc_mod = info.IdBodega,
                    IdCbteVta_fac_nd_doc_mod = info.IdCbteVta,
                    vt_tipoDoc = info.vt_tipoDoc,
                    Valor_Aplicado = Convert.ToDouble(info.info_resumen.ValorProntoPago),
                    ValorProntoPago = Convert.ToDouble(info.info_resumen.Total - info.info_resumen.ValorProntoPago),
                    fecha_cruce = DateTime.Now.Date
                });

                Retorno.info_resumen = new fa_notaCreDeb_resumen_Info
                {
                    SubtotalIVASinDscto = 0,
                    SubtotalSinIVASinDscto = Convert.ToDecimal(info.info_resumen.ValorProntoPago),
                    SubtotalSinDscto = Convert.ToDecimal(info.info_resumen.ValorProntoPago),
                    Descuento = 0,
                    SubtotalIVAConDscto = 0,
                    SubtotalSinIVAConDscto = Convert.ToDecimal(info.info_resumen.ValorProntoPago),
                    SubtotalConDscto = Convert.ToDecimal(info.info_resumen.ValorProntoPago),
                    IdCod_Impuesto_IVA = "IVA0",
                    ValorIVA = 0,
                    Total = Convert.ToDecimal(info.info_resumen.ValorProntoPago),
                    PorIva = 0
                };

                return Retorno;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
