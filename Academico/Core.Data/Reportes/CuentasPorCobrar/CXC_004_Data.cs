using Core.Data.Base;
using Core.Info.Reportes.CuentasPorCobrar;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Core.Data.Reportes.CuentasPorCobrar
{
    public class CXC_004_Data
    {
        public List<CXC_004_Info> Getlist_Reporte(int IdEmpresa, string IdUsuario, DateTime FechaCorte)
        {
            try
            {
                FechaCorte = FechaCorte.Date;

                List <CXC_004_Info> Lista = new List<CXC_004_Info>();

                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = "DECLARE @Fecha date = DATEFROMPARTS(" + FechaCorte.Year.ToString() + "," + FechaCorte.Month.ToString() + "," + FechaCorte.Day.ToString() + ") "
                        + " exec Academico.SPCXC_004 " + IdEmpresa.ToString() + ", '" + IdUsuario + "',@Fecha";
                    command.CommandTimeout = 5000;
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Lista.Add(new CXC_004_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdAlumno = Convert.ToDecimal(reader["IdAlumno"]),
                            IdUsuario = Convert.ToString(reader["IdUsuario"]),
                            NomAnio = Convert.ToString(reader["NomAnio"]),
                            CodigoAlumno = Convert.ToString(reader["CodigoAlumno"]),
                            NombreAlumno = Convert.ToString(reader["NombreAlumno"]),
                            SaldoDeudor = Convert.ToDecimal(reader["SaldoDeudor"]),
                            SaldoAcreedor = Convert.ToDecimal(reader["SaldoAcreedor"]),
                            SaldoFinal = Convert.ToDecimal(reader["SaldoFinal"]),
                            NombreJornada = Convert.ToString(reader["NombreJornada"])
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

        public List<CXC_004_Info> Getlist_Resumen(int IdEmpresa, string IdUsuario, DateTime FechaCorte)
        {
            try
            {
                List<CXC_004_Info> ListaResumen = new List<CXC_004_Info>();

                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = "DECLARE @Fecha date = DATEFROMPARTS(" + FechaCorte.Year.ToString() + "," + FechaCorte.Month.ToString() + "," + FechaCorte.Day.ToString() + ") "
                        + " exec Academico.SPCXC_004 " + IdEmpresa.ToString() + ", '" + IdUsuario + "',@Fecha";
                    command.CommandTimeout = 5000;
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ListaResumen.Add(new CXC_004_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdAlumno = Convert.ToDecimal(reader["IdAlumno"]),
                            IdUsuario = Convert.ToString(reader["IdUsuario"]),
                            NomAnio = Convert.ToString(reader["NomAnio"]),
                            CodigoAlumno = Convert.ToString(reader["CodigoAlumno"]),
                            NombreAlumno = Convert.ToString(reader["NombreAlumno"]),
                            SaldoDeudor = Convert.ToDecimal(reader["SaldoDeudor"]),
                            SaldoAcreedor = Convert.ToDecimal(reader["SaldoAcreedor"]),
                            SaldoFinal = Convert.ToDecimal(reader["SaldoFinal"]),
                            NombreJornada = Convert.ToString(reader["NombreJornada"])
                        });
                    }
                    reader.Close();
                }


                return ListaResumen;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<CXC_004_Info> Getlist_Saldo(int IdEmpresa, DateTime FechaCorte)
        {
            try
            {
                List<CXC_004_Info> ListaSaldo = new List<CXC_004_Info>();

                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = "DECLARE @IdEmpresa int = " + IdEmpresa.ToString() + " , @FechaFin date = DATEFROMPARTS(" + FechaCorte.Year + ", " + FechaCorte.Month + ", " + FechaCorte.Day + ") "
                   + " select a.IdCtaCbleDebe, a.pc_Cuenta, sum(a.Saldo) Debe, 0 Haber, Padre"
                      + " from("
                         + " select a.IdEmpresa, a.IdSucursal, a.IdBodega, a.IdCbteVta, a.vt_tipoDoc, a.vt_fecha, b.Total - isnull(f.dc_ValorPago, 0) Saldo, a.vt_Observacion, case when g.EnCurso = 1 or g.FechaDesde > x.FechaDesde then d.IdCtaCbleDebe else g.IdCtaCbleCierre end IdCtaCbleDebe, e.pc_Cuenta, h.pc_Cuenta Padre"
                          + " from fa_factura as a left join"
                          + " aca_AnioLectivo as x on x.IdEmpresa = a.IdEmpresa and x.EnCurso = 1"
                          + " join fa_factura_resumen as b on a.IdEmpresa = b.IdEmpresa and a.IdSucursal = b.IdSucursal and a.IdBodega = b.IdBodega and a.IdCbteVta = b.IdCbteVta join"
                          + " aca_Matricula_Rubro as c on a.IdEmpresa = c.IdEmpresa and a.IdSucursal = c.IdSucursal and a.IdBodega = c.IdBodega and a.IdCbteVta = c.IdCbteVta left join"
                          + " aca_AnioLectivo_Curso_Plantilla_Parametrizacion as d on c.IdEmpresa = d.IdEmpresa and c.IdAnio = d.IdAnio and c.IdPlantilla = d.IdPlantilla and c.IdSede = d.IdSede and c.IdNivel = d.IdNivel and c.IdJornada = d.IdJornada and c.IdCurso = d.IdCurso and c.IdRubro = d.IdRubro left join"
                          + " aca_AnioLectivo as g on g.IdEmpresa = c.IdEmpresa and g.IdAnio = c.IdAnio left join"
                          + " ct_plancta as e on d.IdEmpresa = e.IdEmpresa and case when g.EnCurso = 1 or g.FechaDesde > x.FechaDesde then d.IdCtaCbleDebe else g.IdCtaCbleCierre end = e.IdCtaCble left join"
                          + " ("
                              + " select b.IdEmpresa, b.IdSucursal, b.IdBodega_Cbte, b.IdCbte_vta_nota, b.dc_TipoDocumento, sum(b.dc_ValorPago) dc_ValorPago"
                              + " from cxc_cobro as a"
                              + " join cxc_cobro_det as b on a.IdEmpresa = b.IdEmpresa and a.IdSucursal = b.IdSucursal and a.IdCobro = b.IdCobro"
                              + " where a.IdEmpresa = @IdEmpresa and a.cr_estado = 'A' AND B.estado = 'A' and a.cr_fecha <= @FechaFin"
                              + " group by  b.IdEmpresa, b.IdSucursal, b.IdBodega_Cbte, b.IdCbte_vta_nota, b.dc_TipoDocumento"
                          + " ) AS f on a.IdEmpresa = f.IdEmpresa and a.IdSucursal = f.IdSucursal and a.IdBodega = f.IdBodega_Cbte and a.IdCbteVta = f.IdCbte_vta_nota and a.vt_tipoDoc = f.dc_TipoDocumento LEFT JOIN"
                          + " ct_plancta as h on h.IdEmpresa = e.IdEmpresa and h.IdCtaCble = left(e.IdCtaCble, 4)"
                          + " where a.IdEmpresa = @IdEmpresa and a.Estado = 'A' and a.vt_fecha <= @FechaFin and a.IdAlumno is not null"
                          + " and dbo.BankersRounding(b.Total - isnull(f.dc_ValorPago, 0), 2) > 0"
                          + " UNION ALL"
                          + " SELECT a.IdEmpresa, a.IdSucursal, a.IdBodega, a.IdNota, a.CodDocumentoTipo, a.no_fecha, b.Total - isnull(f.dc_ValorPago, 0) as Saldo, a.sc_observacion, a.IdCtaCble_TipoNota, d.pc_Cuenta, h.pc_Cuenta Padre"
                          + " FROM fa_notaCreDeb as a join"
                          + " fa_notaCreDeb_resumen as b on a.IdEmpresa = b.IdEmpresa and a.IdSucursal = b.IdSucursal and a.IdBodega = b.IdBodega and a.IdNota = b.IdNota join"
                          + " fa_TipoNota as c on a.IdEmpresa = c.IdEmpresa and a.IdTipoNota = c.IdTipoNota left join"
                          + " ct_plancta as d on a.IdEmpresa = d.IdEmpresa and a.IdCtaCble_TipoNota = d.IdCtaCble left join"
                          + " ("
                              + " select b.IdEmpresa, b.IdSucursal, b.IdBodega_Cbte, b.IdCbte_vta_nota, b.dc_TipoDocumento, sum(b.dc_ValorPago) dc_ValorPago"
                              + " from cxc_cobro as a"
                              + " join cxc_cobro_det as b on a.IdEmpresa = b.IdEmpresa and a.IdSucursal = b.IdSucursal and a.IdCobro = b.IdCobro"
                              + " where a.IdEmpresa = @IdEmpresa and a.cr_estado = 'A' AND B.estado = 'A' and a.cr_fecha <= @FechaFin"
                              + " group by  b.IdEmpresa, b.IdSucursal, b.IdBodega_Cbte, b.IdCbte_vta_nota, b.dc_TipoDocumento"
                          + " ) AS f on a.IdEmpresa = f.IdEmpresa and a.IdSucursal = f.IdSucursal and a.IdBodega = f.IdBodega_Cbte and a.IdNota = f.IdCbte_vta_nota and a.CodDocumentoTipo = f.dc_TipoDocumento  left join"
                          + " ct_plancta as h on d.IdEmpresa = h.IdEmpresa and left(d.IdCtaCble, 4) = h.IdCtaCble"
                          + " where a.IdEmpresa = @IdEmpresa and a.Estado = 'A' and a.CreDeb = 'D' and a.no_fecha <= @FechaFin and a.IdAlumno is not null"
                          + " and dbo.BankersRounding(b.Total - isnull(f.dc_ValorPago, 0), 4) > 0"
                      + " )"
                      + " a group by a.IdCtaCbleDebe, a.pc_Cuenta,Padre"
                      + " UNION ALL"
                      + " SELECT a.IdCtaCble_TipoNota, d.pc_Cuenta, 0 as Debe, dbo.BankersRounding(sum(b.Total) - sum(ISNULL(C.Valor_Aplicado, 0)), 2) Haber, h.pc_Cuenta as Padre"
                          + " FROM fa_notaCreDeb AS A inner join"
                          + " fa_notaCreDeb_resumen as b on a.IdEmpresa = b.IdEmpresa and a.IdSucursal = b.IdSucursal and a.IdBodega = b.IdBodega and a.IdNota = b.IdNota left join"
                          + " ("
                              + " select a1.IdEmpresa_nt, a1.IdSucursal_nt, a1.IdBodega_nt, a1.IdNota_nt, sum(a1.Valor_Aplicado) Valor_Aplicado"
                              + " from fa_notaCreDeb_x_fa_factura_NotaDeb as a1 inner join"
                              + " fa_notaCreDeb as a2 on a1.IdEmpresa_nt = a2.IdEmpresa and a1.IdSucursal_nt = a2.IdSucursal and a1.IdBodega_nt = a2.IdBodega and a1.IdNota_nt = a2.IdNota"
                              + " where a2.IdEmpresa = @IdEmpresa and  a2.CreDeb = 'C' and a2.no_fecha <= @FechaFin and cast(a1.fecha_cruce as date) <= @FechaFin"
                              + " group by a1.IdEmpresa_nt, a1.IdSucursal_nt, a1.IdBodega_nt, a1.IdNota_nt"
                          + " ) as c on a.IdEmpresa = c.IdEmpresa_nt AND A.IdSucursal = C.IdSucursal_nt AND A.IdBodega = C.IdBodega_nt AND A.IdNota = C.IdNota_nt left join"
                          + " ct_plancta as d on a.IdEmpresa = d.IdEmpresa and a.IdCtaCble_TipoNota = d.IdCtaCble left join"
                          + " ct_plancta as h on d.IdEmpresa = h.IdEmpresa and left(d.IdCtaCble, 4) = h.IdCtaCble"
                          + " WHERE A.IdEmpresa = @IdEmpresa AND A.no_fecha <= @FechaFin AND A.Estado = 'A' and a.CreDeb = 'C'"
                          + " group by a.IdCtaCble_TipoNota, d.pc_Cuenta ,h.pc_Cuenta"
                          + " having dbo.BankersRounding(sum(b.Total) - sum(ISNULL(C.Valor_Aplicado, 0)), 2) > 0";
                    command.CommandTimeout = 5000;
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ListaSaldo.Add(new CXC_004_Info
                        {
                            IdCtaCbleDebe = Convert.ToString(reader["IdCtaCbleDebe"]),
                            pc_Cuenta = Convert.ToString(reader["pc_Cuenta"]),                            
                            Debe = Convert.ToDecimal(reader["Debe"]),
                            Haber = Convert.ToDecimal(reader["Haber"]),
                            Padre = Convert.ToString(reader["Padre"])
                        });
                    }
                    reader.Close();
                }


                return ListaSaldo;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
