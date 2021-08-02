using Core.Data.Academico;
using Core.Data.Facturacion;
using Core.Data.General;
using Core.Info.Academico;
using Core.Info.Facturacion;
using Core.Info.General;
using Core.Info.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Bus.Academico
{
    public class aca_AnioLectivo_Periodo_Bus
    {
        aca_AnioLectivo_Periodo_Data odata = new aca_AnioLectivo_Periodo_Data();
        fa_PuntoVta_Data odata_punto_venta = new fa_PuntoVta_Data();
        fa_TerminoPago_Data odata_termino_pago = new fa_TerminoPago_Data();
        aca_MecanismoDePago_Data odata_mecanismo = new aca_MecanismoDePago_Data();
        tb_sis_Impuesto_Data odata_impuesto = new tb_sis_Impuesto_Data();
        fa_factura_Data odata_factura = new fa_factura_Data();
        tb_sis_Documento_Tipo_Talonario_Data odata_talonario = new tb_sis_Documento_Tipo_Talonario_Data();

        public List<aca_AnioLectivo_Periodo_Info> GetList(int IdEmpresa)
        {
            try
            {
                return odata.getList(IdEmpresa);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<aca_AnioLectivo_Periodo_Info> getList_AnioCurso(int IdEmpresa)
        {
            try
            {
                return odata.getList_AnioCurso(IdEmpresa);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<aca_AnioLectivo_Periodo_Info> GetList(int IdEmpresa, int IdAnio, bool MostrarAnulados)
        {
            try
            {
                return odata.getList(IdEmpresa, IdAnio, MostrarAnulados);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public aca_AnioLectivo_Periodo_Info GetInfo(int IdEmpresa, int IdAnio, int Periodo)
        {
            try
            {
                return odata.getInfo(IdEmpresa, IdAnio, Periodo);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ModificarDB(List<aca_AnioLectivo_Periodo_Info> info)
        {
            try
            {
                return odata.modificarDB(info);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool AnularDB(aca_AnioLectivo_Periodo_Info info)
        {
            try
            {
                return odata.anularDB(info);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool ModificarPeriodoDB(aca_AnioLectivo_Periodo_Info info)
        {
            try
            {
                return odata.modificar_FacturacionMasiva(info);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool Modificar_FacturacionMasiva(aca_AnioLectivo_Periodo_Info info)
        {
            try
            {
                //if(odata.modificar_FacturacionMasiva(info))
                //{
                    var punto_venta = odata_punto_venta.get_info(info.IdEmpresa, Convert.ToInt32(info.IdSucursal), Convert.ToInt32(info.IdPuntoVta));

                    #region Factura
                    foreach (var item in info.lst_det_fact_masiva)
                    {
                        tb_sis_Documento_Tipo_Talonario_Info talonario = new tb_sis_Documento_Tipo_Talonario_Info(); ;
                        if (punto_venta != null)
                        {
                            talonario = odata_talonario.GetUltimoNoUsado(info.IdEmpresa, cl_enumeradores.eTipoDocumento.FACT.ToString(), punto_venta.Su_CodigoEstablecimiento, punto_venta.cod_PuntoVta, punto_venta.EsElectronico, false);
                        }

                        var mecanismo = odata_mecanismo.getInfo_by_IdTermino(info.IdEmpresa, item.IdTerminoPago);
                        var termino_pago = odata_termino_pago.get_info(item.IdTerminoPago);

                        var info_factura = new fa_factura_Info
                        {
                            IdEmpresa = info.IdEmpresa,
                            IdSucursal = Convert.ToInt32(info.IdSucursal),
                            IdBodega = punto_venta.IdBodega,
                            vt_tipoDoc = "FACT",
                            vt_serie1 = punto_venta.Su_CodigoEstablecimiento,
                            vt_serie2 = punto_venta.cod_PuntoVta,
                            vt_NumFactura = talonario.NumDocumento,
                            IdAlumno = item.IdAlumno,
                            IdCliente = item.IdCliente,
                            IdVendedor = 1,
                            IdNivel = 1,
                            IdCatalogo_FormaPago = "CRE",
                            //vt_fecha = item.FechaDesde,
                            vt_fecha = DateTime.Now.Date,
                            vt_plazo = termino_pago.Dias_Vct,
                            vt_fech_venc = DateTime.Now.AddDays(termino_pago.Dias_Vct),
                            vt_tipo_venta = item.IdTerminoPago,
                            vt_Observacion = item.vt_Observacion,
                            Estado = "A",
                            IdCaja = punto_venta.IdCaja,
                            IdEmpresa_rol = item.IdEmpresa_rol,
                            IdEmpleado = item.IdEmpleado,
                            IdUsuario = info.IdUsuarioModificacion,
                            IdPuntoVta = info.IdPuntoVta,
                            aprobada_enviar_sri = false,
                            AplicacionMasiva = true
                        };

                        info_factura.lst_det = new List<fa_factura_det_Info>();
                        var info_impuesto = odata_impuesto.get_info(item.IdCod_Impuesto_Iva);
                        var fact_det = new fa_factura_det_Info
                        {
                            Secuencia = 1,
                            IdProducto = item.IdProducto,
                            vt_cantidad = 1,
                            vt_Precio = Convert.ToDouble(item.Total),
                            vt_PorDescUnitario = 0,
                            vt_DescUnitario = 0,
                            vt_PrecioFinal = Convert.ToDouble(item.Total),
                            vt_Subtotal = Convert.ToDouble(item.Subtotal),
                            vt_por_iva = info_impuesto.porcentaje,
                            IdCod_Impuesto_Iva = item.IdCod_Impuesto_Iva,
                            vt_iva = Convert.ToDouble(item.ValorIVA),
                            vt_total = Convert.ToDouble(item.Total),
                            //vt_detallexItems = null,
                            IdMatricula = item.IdMatricula,
                            aca_IdPeriodo = item.IdPeriodo,
                            aca_IdAnio = item.IdAnio,
                            aca_IdPlantilla = item.IdPlantilla,
                            aca_IdRubro = item.IdRubro,
                            AplicaProntoPago = (item.Total==item.ValorProntoPago ? false : true),
                            FechaProntoPago = item.FechaProntoPago,
                            ValorProntoPago = Convert.ToDouble(item.ValorProntoPago)
                        };

                        info_factura.lst_det.Add(fact_det);

                        #region Resumen
                        info_factura.info_resumen = new fa_factura_resumen_Info
                        {
                            SubtotalIVASinDscto = (decimal)Math.Round(info_factura.lst_det.Where(q => q.vt_por_iva != 0).Sum(q => q.vt_cantidad * q.vt_Precio), 2, MidpointRounding.AwayFromZero),
                            SubtotalSinIVASinDscto = (decimal)Math.Round(info_factura.lst_det.Where(q => q.vt_por_iva == 0).Sum(q => q.vt_cantidad * q.vt_Precio), 2, MidpointRounding.AwayFromZero),

                            Descuento = (decimal)Math.Round(info_factura.lst_det.Sum(q => q.vt_DescUnitario * q.vt_cantidad), 2, MidpointRounding.AwayFromZero),

                            SubtotalIVAConDscto = (decimal)Math.Round(info_factura.lst_det.Where(q => q.vt_por_iva != 0).Sum(q => q.vt_Subtotal), 2, MidpointRounding.AwayFromZero),
                            SubtotalSinIVAConDscto = (decimal)Math.Round(info_factura.lst_det.Where(q => q.vt_por_iva == 0).Sum(q => q.vt_Subtotal), 2, MidpointRounding.AwayFromZero),

                            ValorIVA = (decimal)Math.Round(info_factura.lst_det.Sum(q => q.vt_iva), 2, MidpointRounding.AwayFromZero)
                        };
                        info_factura.info_resumen.SubtotalSinDscto = info_factura.info_resumen.SubtotalIVASinDscto + info_factura.info_resumen.SubtotalSinIVASinDscto;
                        info_factura.info_resumen.SubtotalConDscto = info_factura.info_resumen.SubtotalIVAConDscto + info_factura.info_resumen.SubtotalSinIVAConDscto;
                        info_factura.info_resumen.Total = info_factura.info_resumen.SubtotalConDscto + info_factura.info_resumen.ValorIVA;
                        info_factura.info_resumen.ValorProntoPago = (decimal)Math.Round(info_factura.lst_det.Sum(q => q.ValorProntoPago ?? 0), 2, MidpointRounding.AwayFromZero);
                        info_factura.info_resumen.FechaProntoPago = ((info_factura.lst_det.Where(q => q.FechaProntoPago != null).ToList().Count > 0) ? info_factura.lst_det.Min(q => q.FechaProntoPago) : null);
                        info_factura.info_resumen.IdPeriodo = item.IdPeriodo;
                        info_factura.info_resumen.IdAnio = item.IdAnio;
                        info_factura.info_resumen.IdPlantilla = item.IdPlantilla;
                        info_factura.info_resumen.IdRubro = item.IdRubro;
                        #endregion

                        odata_factura.guardarDB(info_factura);
                    }
                    #endregion
                //}

                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
