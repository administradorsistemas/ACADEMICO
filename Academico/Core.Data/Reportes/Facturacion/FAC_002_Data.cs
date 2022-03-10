using Core.Data.Base;
using Core.Info.Reportes.Facturacion;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Reportes.Facturacion
{
    public class FAC_002_Data
    {
        public List<FAC_002_Info> get_list(int IdEmpresa, int IdSucursal, int IdBodega, decimal IdCbteVta, int IdAnio, int IdSede, int IdJornada, int IdNivel, int IdCurso, int IdParalelo, DateTime FechaIni, DateTime FechaFin)
        {
            try
            {
                List<FAC_002_Info> Lista = new List<FAC_002_Info>();
                int IdNivelIni = IdNivel;
                int IdNivelFin = IdNivel == 0 ? 9999999 : IdNivel;

                int IdCursoIni = IdCurso;
                int IdCursoFin = IdCurso == 0 ? 9999999 : IdCurso;

                int IdParaleloIni = IdParalelo;
                int IdParaleloFin = IdParalelo == 0 ? 9999999 : IdParalelo;
                /*
                using (EntitiesReportes Context = new EntitiesReportes())
                {
                    Lista = (from q in Context.VWFAC_002
                             where q.IdEmpresa == IdEmpresa
                             && q.IdSucursal == IdSucursal
                             && q.IdBodega == IdBodega
                             && q.IdCbteVta == IdCbteVta
                             select new FAC_002_Info
                             {
                                 cli_cedulaRuc = q.cli_cedulaRuc,
                                 cli_correo = q.cli_correo,
                                 cli_direccion = q.cli_direccion,
                                 cli_Nombre = q.cli_Nombre,
                                 cli_Telefonos = q.cli_Telefonos,
                                 DescuentoTotal = q.DescuentoTotal,
                                 Fecha_Autorizacion = q.Fecha_Autorizacion,
                                 FormaDePago = q.FormaDePago,
                                 IdBodega = q.IdBodega,
                                 IdCatalogo_FormaPago = q.IdCatalogo_FormaPago,
                                 IdCbteVta = q.IdCbteVta,
                                 IdEmpresa = q.IdEmpresa,
                                 IdProducto = q.IdProducto,
                                 IdSucursal = q.IdSucursal,
                                 pr_descripcion = q.pr_descripcion,
                                 Secuencia = q.Secuencia,
                                 SubtotalConDscto = q.SubtotalConDscto,
                                 SubtotalIVA = q.SubtotalIVA,
                                 SubtotalSinDscto = q.SubtotalSinDscto,
                                 SubtotalSinIVA = q.SubtotalSinIVA,
                                 Su_Descripcion = q.Su_Descripcion,
                                 Su_Direccion = q.Su_Direccion,
                                 Su_Telefonos = q.Su_Telefonos,
                                 vt_autorizacion = q.vt_autorizacion,
                                 Cambio = q.Cambio,
                                 vt_cantidad = q.vt_cantidad,
                                 vt_fecha = q.vt_fecha,
                                 vt_iva = q.vt_iva,
                                 vt_NumFactura = q.vt_NumFactura,
                                 vt_por_iva = q.vt_por_iva,
                                 vt_Precio = q.vt_Precio,
                                 Total = q.Total,
                                 ValorEfectivo = q.ValorEfectivo,
                                 vt_Observacion = q.vt_Observacion,

                                 Descuento = q.Descuento,
                                 SubtotalIVAConDscto = q.SubtotalIVAConDscto,
                                 SubtotalIVASinDscto = q.SubtotalIVASinDscto,
                                 SubtotalSinIVAConDscto = q.SubtotalSinIVAConDscto,
                                 SubtotalSinIVASinDscto = q.SubtotalSinIVASinDscto,
                                 T_SubtotalConDscto = q.T_SubtotalConDscto,
                                 T_SubtotalSinDscto = q.T_SubtotalSinDscto,
                                 ValorIVA = q.ValorIVA,
                                 vt_total = q.vt_total,

                                 NomSede = q.NomSede,
                                 NomJornada = q.NomJornada,
                                 NomNivel = q.NomNivel,
                                 NomCurso = q.NomCurso,
                                 NomPlantilla = q.NomPlantilla,
                                 NomParalelo = q.NomParalelo,
                                 NomAlumno = q.NomAlumno,
                                 
                                 IdAlumno = q.IdAlumno,
                                 IdCliente = q.IdCliente,
                                 Codigo = q.Codigo
                             }).ToList();
                }*/

                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    string query = "";
                    #region Query
                    query = "DECLARE @IdEmpresa int = " + IdEmpresa + ","
                    + " @IdAnio int = " + IdAnio + ","
                    + " @IdSede int = " + IdSede + ","
                    + " @IdJornada int = " + IdJornada + ","
                    + " @IdNivelIni int = " + IdNivelIni + ","
                    + " @IdNivelFin int = " + IdNivelFin + ","
                    + " @IdCursoIni int = " + IdCursoIni + ","
                    + " @IdCursoFin int = " + IdCursoFin + ","
                    + " @IdParaleloIni int = " + IdParaleloIni + ","
                    + " @IdParaleloFin int = " + IdParaleloFin + ","
                    + " @IdSucursal int = " + IdSucursal + ","
                    + " @IdBodega int = " + IdBodega + ","
                    + " @IdCbteVta decimal = " + IdCbteVta
                    + " SELECT d.IdEmpresa, d.IdSucursal, d.IdBodega, d.IdCbteVta, d.Secuencia, d.IdProducto, pro.pr_descripcion, d.vt_cantidad, d.vt_Precio, d.vt_cantidad * d.vt_Precio AS SubtotalSinDscto, d.vt_cantidad * d.vt_DescUnitario AS DescuentoTotal, "
                    + "d.vt_Subtotal AS SubtotalConDscto, d.vt_iva, d.vt_total, d.vt_por_iva, CASE WHEN d.vt_por_iva > 0 THEN vt_cantidad *vt_Precio ELSE 0 END AS SubtotalIVA, "
                    + "CASE WHEN d.vt_por_iva = 0 THEN vt_cantidad *vt_Precio ELSE 0 END AS SubtotalSinIVA, c.vt_fecha, c.vt_serie1 + '-' + c.vt_serie2 + '-' + c.vt_NumFactura AS vt_NumFactura, per.pe_nombreCompleto AS cli_Nombre, "
                    + "per.pe_cedulaRuc AS cli_cedulaRuc, con.Direccion AS cli_direccion, con.Telefono AS cli_Telefonos, con.Correo AS cli_correo, su.Su_Descripcion, su.Su_Telefonos, su.Su_Direccion, cat.Nombre AS FormaDePago, c.IdCatalogo_FormaPago, "
                    + "c.vt_autorizacion, c.Fecha_Autorizacion, c.vt_Observacion, dbo.fa_factura_resumen.SubtotalIVASinDscto, dbo.fa_factura_resumen.SubtotalSinIVASinDscto, dbo.fa_factura_resumen.SubtotalSinDscto AS T_SubtotalSinDscto, "
                    + "dbo.fa_factura_resumen.Descuento, dbo.fa_factura_resumen.SubtotalIVAConDscto, dbo.fa_factura_resumen.SubtotalSinIVAConDscto, dbo.fa_factura_resumen.SubtotalConDscto AS T_SubtotalConDscto, "
                    + "dbo.fa_factura_resumen.ValorIVA, dbo.fa_factura_resumen.Total, dbo.fa_factura_resumen.ValorEfectivo, dbo.fa_factura_resumen.Cambio, e.NomSede, e.NomNivel, e.NomJornada, e.NomCurso, e.NomParalelo, e.NomPlantilla, "
                    + "p.pe_nombreCompleto AS NomAlumno, c.IdAlumno, c.IdCliente, f.Codigo, "
                    + "e.IdAnio, e.IdSede, e.IdJornada, e.IdNivel, e.IdCurso, e.IdParalelo,e.OrdenJornada, e.OrdenNivel, e.OrdenCurso, e.OrdenParalelo "
                    + "FROM     dbo.fa_cliente_contactos AS con INNER JOIN "
                    + "dbo.fa_factura AS c ON con.IdEmpresa = c.IdEmpresa AND con.IdCliente = c.IdCliente INNER JOIN "
                    + "dbo.fa_factura_det AS d ON c.IdEmpresa = d.IdEmpresa AND c.IdSucursal = d.IdSucursal AND c.IdBodega = d.IdBodega AND c.IdCbteVta = d.IdCbteVta INNER JOIN "
                    + "dbo.in_Producto AS pro ON d.IdEmpresa = pro.IdEmpresa AND d.IdProducto = pro.IdProducto INNER JOIN "
                    + "dbo.fa_cliente AS cli ON con.IdEmpresa = cli.IdEmpresa AND con.IdCliente = cli.IdCliente INNER JOIN "
                    + "dbo.tb_persona AS per ON cli.IdPersona = per.IdPersona INNER JOIN "
                    + "dbo.tb_sucursal AS su ON c.IdEmpresa = su.IdEmpresa AND c.IdSucursal = su.IdSucursal LEFT OUTER JOIN "
                    + "dbo.fa_factura_resumen ON c.IdEmpresa = dbo.fa_factura_resumen.IdEmpresa AND c.IdSucursal = dbo.fa_factura_resumen.IdSucursal AND c.IdBodega = dbo.fa_factura_resumen.IdBodega AND "
                    + "c.IdCbteVta = dbo.fa_factura_resumen.IdCbteVta LEFT OUTER JOIN "
                    + "dbo.fa_catalogo AS cat ON c.IdCatalogo_FormaPago = cat.IdCatalogo LEFT OUTER JOIN "
                    + "(SELECT A1.IdEmpresa, A1.IdAnio, A1.IdAlumno, SedeNivel.NomSede, SedeNivel.NomNivel, NivelJornada.NomJornada, JornadaCurso.NomCurso, CursoParalelo.NomParalelo, CursoParalelo.CodigoParalelo, A3.NomPlantilla, "
                    + "A1.IdSede, A1.IdJornada, A1.IdNivel, A1.IdCurso, A1.IdParalelo, NivelJornada.OrdenJornada, SedeNivel.OrdenNivel, JornadaCurso.OrdenCurso, CursoParalelo.OrdenParalelo "
                    + "FROM      dbo.aca_AnioLectivo_NivelAcademico_Jornada AS NivelJornada INNER JOIN "
                    + "dbo.aca_AnioLectivo_Sede_NivelAcademico AS SedeNivel ON NivelJornada.IdEmpresa = SedeNivel.IdEmpresa AND NivelJornada.IdAnio = SedeNivel.IdAnio AND NivelJornada.IdSede = SedeNivel.IdSede AND "
                    + "NivelJornada.IdNivel = SedeNivel.IdNivel INNER JOIN "
                    + "dbo.aca_AnioLectivo_Jornada_Curso AS JornadaCurso ON NivelJornada.IdEmpresa = JornadaCurso.IdEmpresa AND NivelJornada.IdAnio = JornadaCurso.IdAnio AND NivelJornada.IdSede = JornadaCurso.IdSede AND "
                    + "NivelJornada.IdNivel = JornadaCurso.IdNivel AND NivelJornada.IdJornada = JornadaCurso.IdJornada INNER JOIN "
                    + "dbo.aca_AnioLectivo_Curso_Paralelo AS CursoParalelo ON JornadaCurso.IdEmpresa = CursoParalelo.IdEmpresa AND JornadaCurso.IdAnio = CursoParalelo.IdAnio AND JornadaCurso.IdSede = CursoParalelo.IdSede AND "
                    + "JornadaCurso.IdNivel = CursoParalelo.IdNivel AND JornadaCurso.IdJornada = CursoParalelo.IdJornada AND JornadaCurso.IdCurso = CursoParalelo.IdCurso RIGHT OUTER JOIN "
                    + "dbo.aca_Matricula AS A1 INNER JOIN "
                    + "dbo.aca_AnioLectivo AS A2 ON A1.IdEmpresa = A2.IdEmpresa AND A1.IdAnio = A2.IdAnio ON CursoParalelo.IdParalelo = A1.IdParalelo AND CursoParalelo.IdEmpresa = A1.IdEmpresa AND CursoParalelo.IdAnio = A1.IdAnio AND "
                    + "CursoParalelo.IdSede = A1.IdSede AND CursoParalelo.IdNivel = A1.IdNivel AND CursoParalelo.IdJornada = A1.IdJornada AND CursoParalelo.IdCurso = A1.IdCurso LEFT OUTER JOIN "
                    + "dbo.aca_Plantilla AS A3 ON A1.IdEmpresa = A3.IdEmpresa AND A1.IdAnio = A3.IdAnio AND A1.IdPlantilla = A3.IdPlantilla "
                    + "WHERE(A2.EnCurso = 1 or A2.EnCurso = 0)) AS e ON c.IdEmpresa = e.IdEmpresa AND c.IdAlumno = e.IdAlumno INNER JOIN " // by Acueva añadi 2022-03-10 or A2.EnCurso = 0 
                    + "dbo.aca_Alumno AS f ON f.IdEmpresa = c.IdEmpresa AND f.IdAlumno = c.IdAlumno INNER JOIN "
                    + "dbo.tb_persona AS p ON f.IdPersona = p.IdPersona "
                    + "WHERE(c.IdAlumno IS NOT NULL) "
                    + "and c.IdEmpresa = @IdEmpresa ";
                    if (IdJornada!=0)
                    {
                        query += "and e.IdSede = @IdSede "
                        + " and e.IdAnio = @IdAnio "
                        + " and e.IdJornada = @IdJornada "
                        + " and e.IdNivel between @IdNivelIni and @IdNivelFin "
                        + " and e.IdCurso  between @IdCursoIni and @IdCursoFin "
                        + " and e.IdParalelo  between @IdParaleloIni and @IdParaleloFin "
                        + " and c.vt_fecha between DATEFROMPARTS(" + FechaIni.Year.ToString() + ", " + FechaIni.Month.ToString() + ", " + FechaIni.Day.ToString() + ") and DATEFROMPARTS(" + FechaFin.Year.ToString() + ", " + FechaFin.Month.ToString() + ", " + FechaFin.Day.ToString() + ") ";
                    }
                    else
                    {
                        query += " and c.IdSucursal = @IdSucursal "
                               + " and c.IdBodega = @IdBodega "
                               + " and c.IdCbteVta = @IdCbteVta ";
                    }
                    
                    #endregion

                    SqlCommand command = new SqlCommand(query, connection);
                    command.CommandTimeout = 5000;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new FAC_002_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdAlumno = Convert.ToDecimal(reader["IdAlumno"]),
                            IdCbteVta = Convert.ToDecimal(reader["IdCbteVta"]),
                            vt_fecha = Convert.ToDateTime(reader["vt_fecha"]),
                            vt_Observacion = string.IsNullOrEmpty(reader["vt_Observacion"].ToString()) ? null : reader["vt_Observacion"].ToString(),
                            cli_cedulaRuc = string.IsNullOrEmpty(reader["cli_cedulaRuc"].ToString()) ? null : reader["cli_cedulaRuc"].ToString(),
                            cli_correo = string.IsNullOrEmpty(reader["cli_correo"].ToString()) ? null : reader["cli_correo"].ToString(),
                            cli_direccion = string.IsNullOrEmpty(reader["cli_direccion"].ToString()) ? null : reader["cli_direccion"].ToString(),
                            cli_Nombre = string.IsNullOrEmpty(reader["cli_Nombre"].ToString()) ? null : reader["cli_Nombre"].ToString(),
                            cli_Telefonos = string.IsNullOrEmpty(reader["cli_Telefonos"].ToString()) ? null : reader["cli_Telefonos"].ToString(),
                            DescuentoTotal = Convert.ToDouble(reader["DescuentoTotal"]),
                            Fecha_Autorizacion = string.IsNullOrEmpty(reader["Fecha_Autorizacion"].ToString()) ? (DateTime?)null : Convert.ToDateTime(reader["Fecha_Autorizacion"]),
                            FormaDePago = string.IsNullOrEmpty(reader["FormaDePago"].ToString()) ? null : reader["FormaDePago"].ToString(),
                            IdBodega = Convert.ToInt32(reader["IdBodega"]),
                            IdCatalogo_FormaPago = string.IsNullOrEmpty(reader["IdCatalogo_FormaPago"].ToString()) ? null : reader["IdCatalogo_FormaPago"].ToString(),
                            IdProducto = Convert.ToDecimal(reader["IdProducto"]),
                            IdSucursal = Convert.ToInt32(reader["IdSucursal"]),
                            pr_descripcion = string.IsNullOrEmpty(reader["pr_descripcion"].ToString()) ? null : reader["pr_descripcion"].ToString(),
                            Secuencia = Convert.ToInt32(reader["Secuencia"]),
                            SubtotalConDscto = Convert.ToDouble(reader["SubtotalConDscto"]),
                            SubtotalIVA = Convert.ToDouble(reader["SubtotalIVA"]),
                            SubtotalSinDscto = Convert.ToDouble(reader["SubtotalSinDscto"]),
                            SubtotalSinIVA = Convert.ToDouble(reader["SubtotalSinIVA"]),
                            Su_Descripcion = string.IsNullOrEmpty(reader["Su_Descripcion"].ToString()) ? null : reader["Su_Descripcion"].ToString(),
                            Su_Direccion = string.IsNullOrEmpty(reader["Su_Direccion"].ToString()) ? null : reader["Su_Direccion"].ToString(),
                            Su_Telefonos = string.IsNullOrEmpty(reader["Su_Telefonos"].ToString()) ? null : reader["Su_Telefonos"].ToString(),
                            vt_autorizacion = string.IsNullOrEmpty(reader["vt_autorizacion"].ToString()) ? null : reader["vt_autorizacion"].ToString(),
                            Cambio = string.IsNullOrEmpty(reader["Cambio"].ToString()) ? (decimal?) null : Convert.ToDecimal(reader["Cambio"]),
                            vt_cantidad = Convert.ToDouble(reader["vt_cantidad"]),
                            vt_iva = Convert.ToDouble(reader["vt_iva"]),
                            vt_NumFactura = string.IsNullOrEmpty(reader["vt_NumFactura"].ToString()) ? null : reader["vt_NumFactura"].ToString(),
                            vt_por_iva = Convert.ToDouble(reader["vt_por_iva"]),
                            vt_Precio = Convert.ToDouble(reader["vt_Precio"]),
                            Total = string.IsNullOrEmpty(reader["Total"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["Total"]),
                            ValorEfectivo = string.IsNullOrEmpty(reader["ValorEfectivo"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["ValorEfectivo"]),
                            NomSede = string.IsNullOrEmpty(reader["NomSede"].ToString()) ? null : reader["NomSede"].ToString(),
                            NomCurso = string.IsNullOrEmpty(reader["NomCurso"].ToString()) ? null : reader["NomCurso"].ToString(),
                            NomJornada = string.IsNullOrEmpty(reader["NomJornada"].ToString()) ? null : reader["NomJornada"].ToString(),
                            NomNivel = string.IsNullOrEmpty(reader["NomNivel"].ToString()) ? null : reader["NomNivel"].ToString(),
                            NomParalelo = string.IsNullOrEmpty(reader["NomParalelo"].ToString()) ? null : reader["NomParalelo"].ToString(),
                            Codigo = string.IsNullOrEmpty(reader["Codigo"].ToString()) ? null : reader["Codigo"].ToString(),
                            NomPlantilla = string.IsNullOrEmpty(reader["NomPlantilla"].ToString()) ? null : reader["NomPlantilla"].ToString(),
                            NomAlumno = string.IsNullOrEmpty(reader["NomAlumno"].ToString()) ? null : reader["NomAlumno"].ToString(),
                            IdAnio = Convert.ToInt32(reader["IdAnio"]),
                            IdSede = Convert.ToInt32(reader["IdSede"]),
                            IdJornada = Convert.ToInt32(reader["IdJornada"]),
                            IdCurso = Convert.ToInt32(reader["IdCurso"]),
                            IdParalelo = Convert.ToInt32(reader["IdParalelo"]),
                            IdNivel = Convert.ToInt32(reader["IdNivel"]),
                            OrdenNivel = string.IsNullOrEmpty(reader["OrdenNivel"].ToString()) ? (int?)null : Convert.ToInt32(reader["OrdenNivel"]),
                            OrdenJornada = string.IsNullOrEmpty(reader["OrdenJornada"].ToString()) ? (int?)null : Convert.ToInt32(reader["OrdenJornada"]),
                            OrdenCurso = string.IsNullOrEmpty(reader["OrdenCurso"].ToString()) ? (int?)null : Convert.ToInt32(reader["OrdenCurso"]),
                            OrdenParalelo = string.IsNullOrEmpty(reader["OrdenParalelo"].ToString()) ? (int?)null : Convert.ToInt32(reader["OrdenParalelo"]),
                            IdCliente = Convert.ToDecimal(reader["IdCliente"]),
                            Descuento = string.IsNullOrEmpty(reader["Descuento"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["Descuento"]),
                            SubtotalIVAConDscto = string.IsNullOrEmpty(reader["SubtotalIVAConDscto"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["SubtotalIVAConDscto"]),
                            SubtotalIVASinDscto = string.IsNullOrEmpty(reader["SubtotalIVASinDscto"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["SubtotalIVASinDscto"]),
                            SubtotalSinIVAConDscto = string.IsNullOrEmpty(reader["SubtotalSinIVAConDscto"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["SubtotalSinIVAConDscto"]),
                            SubtotalSinIVASinDscto = string.IsNullOrEmpty(reader["SubtotalSinIVASinDscto"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["SubtotalSinIVASinDscto"]),
                            T_SubtotalConDscto = string.IsNullOrEmpty(reader["T_SubtotalConDscto"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["T_SubtotalConDscto"]),
                            T_SubtotalSinDscto = string.IsNullOrEmpty(reader["T_SubtotalSinDscto"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["T_SubtotalSinDscto"]),
                            ValorIVA = string.IsNullOrEmpty(reader["ValorIVA"].ToString()) ? (decimal?)null : Convert.ToDecimal(reader["ValorIVA"]),
                            vt_total = Convert.ToDouble(reader["vt_total"]),
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
