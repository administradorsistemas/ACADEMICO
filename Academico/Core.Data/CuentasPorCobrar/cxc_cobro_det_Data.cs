using Core.Data.Base;
using Core.Info.CuentasPorCobrar;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Core.Data.CuentasPorCobrar
{
    public class cxc_cobro_det_Data
    {
        public List<cxc_cobro_det_Info> get_list_cartera(int IdEmpresa, int IdSucursal, decimal IdCliente, bool FiltrarPorCliente)
        {
            try
            {
                List<cxc_cobro_det_Info> Lista = new List<cxc_cobro_det_Info>();

                using (EntitiesCuentasPorCobrar Context = new EntitiesCuentasPorCobrar())
                {
                    Context.SetCommandTimeOut(6000);
                    var lst = Context.vwcxc_cartera_x_cobrar.Where(q=> q.IdEmpresa == IdEmpresa
                             && q.IdSucursal == IdSucursal                        
                             && q.IdCliente == (FiltrarPorCliente == true ? IdCliente : q.IdCliente)
                             && q.IdAlumno == (FiltrarPorCliente == false ? IdCliente : q.IdAlumno)

                             && q.Saldo > 0).ToList();

                    foreach (var q in lst)
                    {
                        Lista.Add(new cxc_cobro_det_Info
                        {
                            IdEmpresa = q.IdEmpresa,
                            IdSucursal = q.IdSucursal,
                            IdBodega_Cbte = q.IdBodega,
                            dc_TipoDocumento = q.vt_tipoDoc,
                            vt_NumDocumento = q.vt_NunDocumento,
                            Observacion = q.Referencia,
                            IdCbte_vta_nota = q.IdComprobante,
                            vt_fecha = q.vt_fecha,
                            vt_total = q.vt_total,
                            Saldo = q.Saldo,
                            vt_Subtotal = q.vt_Subtotal,
                            vt_iva = q.vt_iva,
                            vt_fech_venc = q.vt_fech_venc,
                            NomCliente = q.NomCliente,
                            ValorProntoPago = q.ValorProntoPago,
                            FechaProntoPago = q.FechaProntoPago,

                            IdAnio = q.IdAnio,
                            IdPlantilla = q.IdPlantilla,
                            IdPuntoVta = q.IdPuntoVta,
                            IdCliente = q.IdCliente,
                            IdAlumno = q.IdAlumno,
                            SaldoProntoPago = q.SaldoProntoPago??0
                        });
                    }

                    Lista.ForEach(q => { q.secuencia = q.dc_TipoDocumento + "-" + q.IdBodega_Cbte.ToString() + "-" + q.IdCbte_vta_nota.ToString(); q.dc_ValorPago = Convert.ToDouble(q.Saldo); });
                }

                return Lista.OrderBy(q=> q.vt_fecha).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<cxc_cobro_det_Info> get_list_cartera_x_alumno(int IdEmpresa, int IdSucursal, decimal IdAlumno)
        {
            try
            {
                List<cxc_cobro_det_Info> Lista = new List<cxc_cobro_det_Info>();

                using (EntitiesCuentasPorCobrar Context = new EntitiesCuentasPorCobrar())
                {
                    Context.SetCommandTimeOut(6000);
                    var lst = Context.vwcxc_cartera_x_cobrar.Where(q => q.IdEmpresa == IdEmpresa
                             && q.IdSucursal == IdSucursal && q.IdAlumno == IdAlumno).ToList();

                    foreach (var q in lst)
                    {
                        Lista.Add(new cxc_cobro_det_Info
                        {
                            IdEmpresa = q.IdEmpresa,
                            IdSucursal = q.IdSucursal,
                            IdBodega_Cbte = q.IdBodega,
                            dc_TipoDocumento = q.vt_tipoDoc,
                            vt_NumDocumento = q.vt_NunDocumento,
                            Observacion = q.Referencia,
                            IdCbte_vta_nota = q.IdComprobante,
                            vt_fecha = q.vt_fecha,
                            vt_total = q.vt_total,
                            Saldo = q.Saldo,
                            vt_Subtotal = q.vt_Subtotal,
                            vt_iva = q.vt_iva,
                            vt_fech_venc = q.vt_fech_venc,
                            NomCliente = q.NomCliente,
                            ValorProntoPago = q.ValorProntoPago,
                            FechaProntoPago = q.FechaProntoPago,
                            TotalxCobrado = q.TotalxCobrado,
                            IdAnio = q.IdAnio,
                            IdPlantilla = q.IdPlantilla,
                            IdPuntoVta = q.IdPuntoVta,
                            IdCliente = q.IdCliente,
                            IdAlumno = q.IdAlumno
                        });
                    }

                    Lista.ForEach(q => { q.secuencia = q.dc_TipoDocumento + "-" + q.IdBodega_Cbte.ToString() + "-" + q.IdCbte_vta_nota.ToString(); q.dc_ValorPago = Convert.ToDouble(q.Saldo); });
                }

                return Lista;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<cxc_cobro_det_Info> get_list(int IdEmpresa, int IdSucursal, decimal IdCobro)
        {
            try
            {
                List<cxc_cobro_det_Info> Lista;

                using (EntitiesCuentasPorCobrar Context = new EntitiesCuentasPorCobrar())
                {
                    Lista = (from q in Context.vwcxc_cobro_det
                             where q.IdEmpresa == IdEmpresa
                             && q.IdSucursal == IdSucursal
                             && q.IdCobro == IdCobro
                             select new cxc_cobro_det_Info
                             {
                                 IdEmpresa = q.IdEmpresa,
                                 IdSucursal = q.IdSucursal,
                                 IdBodega_Cbte = q.IdBodega_Cbte,
                                 dc_TipoDocumento = q.dc_TipoDocumento,
                                 vt_NumDocumento = q.vt_NumFactura,
                                 Observacion = q.vt_Observacion,
                                 IdCbte_vta_nota = q.IdCbte_vta_nota,
                                 vt_fecha = q.vt_fecha,
                                 vt_total = q.vt_total,
                                 Saldo = q.saldo_sin_cobro,
                                 vt_Subtotal = q.vt_Subtotal,
                                 vt_iva = q.vt_iva,
                                 vt_fech_venc = q.vt_fech_venc,
                                 IdCobro_tipo_det = q.IdCobro_tipo,
                                 IdCobro = q.IdCobro,
                                 dc_ValorPago = q.dc_ValorPago,
                                 IdNotaCredito = q.IdNotaCredito,
                                 dc_ValorProntoPago = q.dc_ValorProntoPago,
                                 IdAnio = q.IdAnio,
                                 IdPlantilla = q.IdPlantilla,
                                 IdPuntoVta = q.IdPuntoVta,
                                 IdAlumno = q.IdAlumno
                             }).ToList();

                    Lista.ForEach(q => { q.secuencia = q.dc_TipoDocumento +"-"+ q.IdBodega_Cbte.ToString() +"-"+ q.IdCbte_vta_nota.ToString(); q.Saldo_final = Convert.ToDouble(q.Saldo - q.dc_ValorPago); });
                }

                return Lista;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<cxc_cobro_det_Info> get_list(int IdEmpresa, int IdSucursal, int IdBodega, decimal IdCbteVta, string dc_TipoDocumento)
        {
            try
            {
                List<cxc_cobro_det_Info> Lista;

                using (EntitiesCuentasPorCobrar Context = new EntitiesCuentasPorCobrar())
                {
                    Lista = (from q in Context.vwcxc_cobro_det_retencion
                             where q.IdEmpresa == IdEmpresa
                             && q.IdSucursal == IdSucursal
                             && q.IdBodega_Cbte == IdBodega
                             && q.IdCbte_vta_nota == IdCbteVta
                             && q.dc_TipoDocumento == dc_TipoDocumento
                             select new cxc_cobro_det_Info
                             {
                                 IdEmpresa = q.IdEmpresa,
                                 IdSucursal = q.IdSucursal,
                                 IdBodega_Cbte = q.IdBodega_Cbte,
                                 IdCbte_vta_nota = q.IdCbte_vta_nota,
                                 dc_TipoDocumento = q.dc_TipoDocumento,
                                 IdCobro = q.IdCobro,
                                 secuencial = q.secuencial,
                                 IdCobro_tipo_det = q.IdCobro_tipo,
                                 dc_ValorPago = q.dc_ValorPago,
                                 tc_descripcion = q.tc_descripcion,
                                 ESRetenIVA = q.ESRetenIVA,
                                 ESRetenFTE = q.ESRetenFTE,
                                 PorcentajeRet = q.PorcentajeRet,
                                 cr_fecha = q.cr_fecha,
                                 cr_NumDocumento = q.cr_NumDocumento
                             }).ToList();
                }

                return Lista;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<cxc_cobro_det_Info> get_list_AP(int IdEmpresa)
        {
            try
            {
                List<cxc_cobro_det_Info> Lista;

                using (EntitiesCuentasPorCobrar Context = new EntitiesCuentasPorCobrar())
                {
                    Context.SetCommandTimeOut(6000); // by Acueva 2022-01-31
                    Lista = (from q in Context.vwcxc_cartera_x_cobrar
                             where q.IdEmpresa == IdEmpresa
                             select new cxc_cobro_det_Info
                             {
                                 IdEmpresa = q.IdEmpresa,
                                 IdSucursal = q.IdSucursal,
                                 IdBodega_Cbte = q.IdBodega,
                                 dc_TipoDocumento = q.vt_tipoDoc,
                                 vt_NumDocumento = q.vt_NunDocumento,
                                 IdCbte_vta_nota = q.IdComprobante,
                                 vt_fecha = q.vt_fecha,
                                 vt_total = q.vt_total,
                                 Saldo = q.Saldo,
                                 vt_Subtotal = q.vt_Subtotal,
                                 vt_iva = q.vt_iva,
                                 vt_fech_venc = q.vt_fech_venc,
                                 dc_ValorProntoPago = q.ValorProntoPago,
                                 IdAnio = q.IdAnio,
                                 IdPlantilla = q.IdPlantilla,
                                 IdPuntoVta = q.IdPuntoVta,
                                 IdAlumno = q.IdAlumno
                             }).ToList();

                    Lista.ForEach(q => { q.secuencia = q.dc_TipoDocumento + "-" + q.IdBodega_Cbte.ToString() + "-" + q.IdCbte_vta_nota.ToString(); q.Saldo_final = Convert.ToDouble(q.Saldo - q.dc_ValorPago); });
                }

                return Lista;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<cxc_cobro_det_Info> get_list_AP(int IdEmpresa, decimal IdAlumno)
        {
            try
            {
                List<cxc_cobro_det_Info> Lista;

                using (EntitiesCuentasPorCobrar Context = new EntitiesCuentasPorCobrar())
                {
                    Lista = (from q in Context.vwcxc_cartera_x_cobrar
                             where q.IdEmpresa == IdEmpresa
                             && q.IdAlumno == IdAlumno
                             select new cxc_cobro_det_Info
                             {
                                 IdEmpresa = q.IdEmpresa,
                                 IdSucursal = q.IdSucursal,
                                 IdBodega_Cbte = q.IdBodega,
                                 dc_TipoDocumento = q.vt_tipoDoc,
                                 vt_NumDocumento = q.vt_NunDocumento,
                                 IdCbte_vta_nota = q.IdComprobante,
                                 vt_fecha = q.vt_fecha,
                                 vt_total = q.vt_total,
                                 Saldo = q.Saldo,
                                 vt_Subtotal = q.vt_Subtotal,
                                 vt_iva = q.vt_iva,
                                 vt_fech_venc = q.vt_fech_venc,
                                 dc_ValorProntoPago = q.ValorProntoPago,
                                 ValorProntoPago = q.ValorProntoPago,
                                 IdAnio = q.IdAnio,
                                 IdPlantilla = q.IdPlantilla,
                                 IdPuntoVta = q.IdPuntoVta,
                                 IdAlumno = q.IdAlumno
                             }).ToList();

                    Lista.ForEach(q => { q.secuencia = q.dc_TipoDocumento + "-" + q.IdBodega_Cbte.ToString() + "-" + q.IdCbte_vta_nota.ToString(); q.Saldo_final = Convert.ToDouble(q.Saldo - q.dc_ValorPago); });
                }

                return Lista;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<cxc_cobro_det_Info> GetListCobrosSinAplicarNC(int IdEmpresa, DateTime FechIni, DateTime FechaFin)
        {
            try
            {
                List<cxc_cobro_det_Info> Lista = new List<cxc_cobro_det_Info>();

                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = "select a.IdEmpresa, a.IdSucursal, a.IdCobro, a.secuencial, a.dc_TipoDocumento, a.dc_ValorPago, a.dc_ValorProntoPago, a.IdNotaCredito,"
                                        + " B.cr_fecha, isnull(c.IdAlumno, d.IdAlumno) IdAlumno,isnull(c.IdCliente, d.IdCliente) IdCliente, b.IdUsuario,"
                                        + " case when a.dc_TipoDocumento = 'FACT' then c.vt_serie1 + '-' + c.vt_serie2 + '-' + c.vt_NumFactura else "
                                        + " case when d.NaturalezaNota = 'SRI' then d.Serie1 + '-' + d.Serie2 + '-' + d.NumNota_Impresa else cast(d.IdNota as varchar(10)) end end as NumDocumento, f.pe_nombreCompleto,"
                                        + " a.IdBodega_Cbte, a.IdCbte_vta_nota"
                                        + " from cxc_cobro_det as a WITH(NOLOCK) join"
                                        + " cxc_cobro as b WITH(NOLOCK) on a.IdEmpresa = b.IdEmpresa and a.IdSucursal = b.IdSucursal and a.IdCobro = b.IdCobrO LEFT JOIN"
                                        + " fa_factura As c WITH (NOLOCK)on c.IdEmpresa = a.IdEmpresa and c.IdSucursal = a.IdSucursal and c.IdBodega = a.IdBodega_Cbte and c.IdCbteVta = a.IdCbte_vta_nota and c.vt_tipoDoc = a.dc_TipoDocumento left join"
                                        + " fa_notaCreDeb as d WITH(NOLOCK) on d.IdEmpresa = a.IdEmpresa and d.IdSucursal = a.IdSucursal and d.IdBodega = a.IdBodega_Cbte and d.IdNota = a.IdCbte_vta_nota and d.CodDocumentoTipo = a.dc_TipoDocumento left join"
                                        + " aca_Alumno as e WITH(NOLOCK) on e.IdEmpresa = b.IdEmpresa and e.IdAlumno = b.IdAlumno join"
                                        + " tb_persona as f WITH(NOLOCK) on e.IdPersona = f.IdPersona"
                                        + " where a.IdNotaCredito is null and a.dc_ValorProntoPago > 0 and a.estado = 'A' and b.cr_estado = 'A'"
                                        + " and a.IdEmpresa = "+IdEmpresa.ToString()+" and b.cr_fecha between DATEFROMPARTS("+FechIni.Year.ToString()+ ", " + FechIni.Month.ToString() + ", " + FechIni.Day.ToString() + ") and DATEFROMPARTS(" + FechaFin.Year.ToString() + "," + FechaFin.Month.ToString() + ", " + FechaFin.Day.ToString() + ")";
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new cxc_cobro_det_Info
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdSucursal = Convert.ToInt32(reader["IdSucursal"]),
                            IdCobro = Convert.ToDecimal(reader["IdCobro"]),
                            secuencial = Convert.ToInt32(reader["secuencial"]),
                            dc_TipoDocumento = Convert.ToString(reader["dc_TipoDocumento"]),
                            dc_ValorPago = Convert.ToDouble(reader["dc_ValorPago"]),
                            dc_ValorProntoPago = Convert.ToDouble(reader["dc_ValorProntoPago"]),
                            cr_fecha = Convert.ToDateTime(reader["cr_fecha"]),
                            IdAlumno = Convert.ToDecimal(reader["IdAlumno"]),
                            IdCliente = Convert.ToDecimal(reader["IdCliente"]),
                            IdUsuario = Convert.ToString(reader["IdUsuario"]),
                            Observacion = Convert.ToString(reader["NumDocumento"]),
                            pe_nombreCompleto = Convert.ToString(reader["pe_nombreCompleto"]),
                            IdBodega_Cbte = Convert.ToInt32(reader["IdBodega_Cbte"]),
                            IdCbte_vta_nota = Convert.ToInt32(reader["IdCbte_vta_nota"])
                        });
                    }
                    reader.Read();
                    Lista.ForEach(q => q.secuencia = q.IdSucursal.ToString().PadLeft(4,'0') + q.IdCobro.ToString().PadLeft(10,'0') + q.secuencial.ToString().PadLeft(4,'0'));
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
