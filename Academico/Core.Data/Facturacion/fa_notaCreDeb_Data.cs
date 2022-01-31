using Core.Data.Academico;
using Core.Data.Base;
using Core.Data.Caja;
using Core.Data.Contabilidad;
using Core.Data.CuentasPorCobrar;
using Core.Data.General;
using Core.Info.Caja;
using Core.Info.Contabilidad;
using Core.Info.CuentasPorCobrar;
using Core.Info.Facturacion;
using Core.Info.General;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Core.Data.Facturacion
{
    public class fa_notaCreDeb_Data
    {
        #region Variables
        cxc_cobro_tipo_Data odat_TipoCobro = new cxc_cobro_tipo_Data();
        caj_Caja_Movimiento_Data odata_MovimientoCaja = new caj_Caja_Movimiento_Data();
        ct_cbtecble_Data odata_ct = new ct_cbtecble_Data();
        fa_notaCreDeb_det_Data odata_Det = new fa_notaCreDeb_det_Data();
        fa_notaCreDeb_x_fa_factura_NotaDeb_Data odata_DetNDFac = new fa_notaCreDeb_x_fa_factura_NotaDeb_Data();
        aca_Alumno_Data DataAlumno = new aca_Alumno_Data();
        #endregion

        caj_Caja_Movimiento_Data odataMovCaja = new caj_Caja_Movimiento_Data();
        public List<fa_notaCreDeb_consulta_Info> get_list(int IdEmpresa, int IdSucursal, DateTime Fecha_ini, DateTime Fecha_fin, string CreDeb)
        {
            try
            {
                List<fa_notaCreDeb_consulta_Info> Lista;
                Fecha_ini = Fecha_ini.Date;
                Fecha_fin = Fecha_fin.Date;
                using (EntitiesFacturacion Context = new EntitiesFacturacion())
                {
                    Lista = (from q in Context.vwfa_notaCreDeb
                             where q.IdEmpresa == IdEmpresa
                             && q.IdSucursal == IdSucursal
                             && Fecha_ini <= q.no_fecha
                             && q.no_fecha <= Fecha_fin
                             && q.CreDeb == CreDeb
                             select new fa_notaCreDeb_consulta_Info
                             {
                                 IdEmpresa = q.IdEmpresa,
                                 IdSucursal = q.IdSucursal,
                                 IdBodega = q.IdBodega,
                                 IdNota = q.IdNota,
                                 CreDeb = q.CreDeb,
                                 NumNota_Impresa = q.NumNota_Impresa,
                                 no_fecha = q.no_fecha,
                                 Nombres = q.Nombres,
                                 sc_subtotal = q.sc_subtotal,
                                 sc_iva = q.sc_iva,
                                 sc_total = q.sc_total,
                                 Estado = q.Estado,

                                 EstadoBool = q.Estado == "A" ? true : false
                             }).ToList();
                }

                return Lista;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<fa_notaCreDeb_consulta_Info> get_list_academico(int IdEmpresa, int IdSucursal, DateTime Fecha_ini, DateTime Fecha_fin, string CreDeb)
        {
            try
            {
                List<fa_notaCreDeb_consulta_Info> Lista;
                Fecha_ini = Fecha_ini.Date;
                Fecha_fin = Fecha_fin.Date;
                using (EntitiesFacturacion Context = new EntitiesFacturacion())
                {
                    Lista = (from q in Context.vwfa_notaCreDeb
                             where q.IdEmpresa == IdEmpresa
                             && q.IdSucursal == IdSucursal
                             && Fecha_ini <= q.no_fecha
                             && q.no_fecha <= Fecha_fin
                             && q.CreDeb == CreDeb
                             && q.IdAlumno!= null
                             select new fa_notaCreDeb_consulta_Info
                             {
                                 IdEmpresa = q.IdEmpresa,
                                 IdSucursal = q.IdSucursal,
                                 IdBodega = q.IdBodega,
                                 IdNota = q.IdNota,
                                 CreDeb = q.CreDeb,
                                 NumNota_Impresa = q.NumNota_Impresa,
                                 no_fecha = q.no_fecha,
                                 Nombres = q.Nombres,
                                 NombresAlumno=q.pe_nombreCompleto,
                                 Usuario=q.IdUsuario,
                                 sc_subtotal = q.sc_subtotal,
                                 sc_iva = q.sc_iva,
                                 sc_total = q.sc_total,
                                 Estado = q.Estado,
                                 NaturalezaNota =q.NaturalezaNota,
                                 No_Descripcion = q.No_Descripcion,
                                 NumAutorizacion = q.NumAutorizacion,
                                 Fecha_Autorizacion = q.Fecha_Autorizacion,
                                 IdUsuario = q.IdUsuario,
                                 EstadoBool = q.Estado == "A" ? true : false
                             }).ToList();
                }

                return Lista.OrderByDescending(q=> q.IdNota).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public fa_notaCreDeb_Info get_info(int IdEmpresa, int IdSucursal, int IdBodega, decimal IdNota)
        {
            try
            {
                fa_notaCreDeb_Info info;

                using (EntitiesFacturacion Context = new EntitiesFacturacion())
                {
                    Context.SetCommandTimeOut(3000); // By Acueva 2022-01-31
                    var Entity = Context.fa_notaCreDeb.Where(q => q.IdEmpresa == IdEmpresa && q.IdSucursal == IdSucursal && q.IdBodega == IdBodega && q.IdNota == IdNota).FirstOrDefault();
                    if (Entity == null) return null;
                    info = new fa_notaCreDeb_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdSucursal = Entity.IdSucursal,
                        IdBodega = Entity.IdBodega,
                        IdNota = Entity.IdNota,
                        IdPuntoVta = Entity.IdPuntoVta,
                        dev_IdEmpresa = Entity.dev_IdEmpresa,
                        dev_IdDev_Inven = Entity.dev_IdDev_Inven,
                        CodNota = Entity.CodNota,
                        CreDeb = Entity.CreDeb.Trim(),
                        CodDocumentoTipo = Entity.CodDocumentoTipo,
                        Serie1 = Entity.Serie1,
                        Serie2 = Entity.Serie2,
                        NumNota_Impresa = Entity.NumNota_Impresa,
                        NumAutorizacion = Entity.NumAutorizacion,
                        Fecha_Autorizacion = Entity.Fecha_Autorizacion,
                        IdCliente = Entity.IdCliente,
                        IdAlumno = Entity.IdAlumno,
                        no_fecha = Entity.no_fecha,
                        no_fecha_venc = Entity.no_fecha_venc,
                        IdTipoNota = Entity.IdTipoNota,
                        sc_observacion = Entity.sc_observacion,
                        Estado = Entity.Estado,
                        NaturalezaNota = Entity.NaturalezaNota,
                        IdCtaCble_TipoNota = Entity.IdCtaCble_TipoNota,
                        IdCobro_tipo = Entity.IdCobro_tipo
                    };

                    info.info_resumen = new fa_notaCreDeb_resumen_Info();
                    var EntityResumen = Context.fa_notaCreDeb_resumen.Where(q => q.IdEmpresa == IdEmpresa && q.IdSucursal == IdSucursal && q.IdBodega == IdBodega && q.IdNota == IdNota).FirstOrDefault();
                    if (EntityResumen==null)
                    {
                        info.info_resumen = new fa_notaCreDeb_resumen_Info();
                    }
                    else
                    {
                        info.info_resumen = new fa_notaCreDeb_resumen_Info
                        {
                            IdEmpresa = EntityResumen.IdEmpresa,
                            IdSucursal = EntityResumen.IdSucursal,
                            IdBodega = EntityResumen.IdBodega,
                            IdNota = EntityResumen.IdNota,
                            SubtotalIVASinDscto = EntityResumen.SubtotalIVASinDscto,
                            SubtotalSinIVASinDscto = EntityResumen.SubtotalSinIVASinDscto,
                            SubtotalSinDscto = EntityResumen.SubtotalSinDscto,
                            Descuento = EntityResumen.Descuento,
                            SubtotalIVAConDscto = EntityResumen.SubtotalIVAConDscto,
                            SubtotalSinIVAConDscto = EntityResumen.SubtotalSinIVAConDscto,
                            SubtotalConDscto = EntityResumen.SubtotalConDscto,
                            IdCod_Impuesto_IVA = EntityResumen.IdCod_Impuesto_IVA,
                            ValorIVA = EntityResumen.ValorIVA,
                            Total = EntityResumen.Total,
                            PorIva = EntityResumen.PorIva
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
        public bool DocumentoExiste(int IdEmpresa, string CodDocumentoTipo, string Serie1, string Serie2, string NumNota_Impresa)
        {
            try
            {
                using (EntitiesFacturacion Context = new EntitiesFacturacion())
                {
                    var lst = from q in Context.fa_notaCreDeb
                              where q.IdEmpresa == IdEmpresa
                              && q.CodDocumentoTipo == CodDocumentoTipo
                              && q.Serie1 == Serie1
                              && q.Serie2 == Serie2
                              && q.NumNota_Impresa == NumNota_Impresa
                              select q;

                    if (lst.Count() > 0)
                        return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private decimal get_id(int IdEmpresa, int IdSucursal, int IdBodega)
        {
            try
            {
                decimal ID = 1;

                using (EntitiesFacturacion Context = new EntitiesFacturacion())
                {
                    var lst = from q in Context.fa_notaCreDeb
                              where q.IdEmpresa == IdEmpresa
                              && q.IdSucursal == IdSucursal
                              && q.IdBodega == IdBodega
                              select q;

                    if (lst.Count() > 0)
                        ID = lst.Max(q => q.IdNota) + 1;
                }

                return ID;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool guardarDB(fa_notaCreDeb_Info info)
        {            
            try
            {
                #region Variables
                int Secuencia = 1;
                
                cxc_cobro_Data odata_cobr = new cxc_cobro_Data();
                
                #endregion

                using (EntitiesFacturacion db_f = new EntitiesFacturacion())
                {
                    var TipoNota = db_f.fa_TipoNota.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdTipoNota == info.IdTipoNota).FirstOrDefault();
                    if (TipoNota != null)
                    {
                        info.IdCtaCble_TipoNota = info.CreDeb == "D" ? TipoNota.IdCtaCbleCXC : TipoNota.IdCtaCble;
                    }

                    #region Nota de debito credito

                    #region Cabecera
                    fa_notaCreDeb Entity = new fa_notaCreDeb
                    //db_f.fa_notaCreDeb.Add(new fa_notaCreDeb
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdSucursal = info.IdSucursal,
                        IdBodega = info.IdBodega,
                        IdNota = info.IdNota = get_id(info.IdEmpresa, info.IdSucursal, info.IdBodega),
                        IdPuntoVta = info.IdPuntoVta,
                        CodNota = info.CodNota,
                        CreDeb = info.CreDeb.Trim(),
                        CodDocumentoTipo = info.CodDocumentoTipo,
                        Serie1 = info.Serie1,
                        Serie2 = info.Serie2,
                        NumNota_Impresa = info.NumNota_Impresa,
                        NumAutorizacion = info.NumAutorizacion,
                        Fecha_Autorizacion = info.Fecha_Autorizacion,
                        IdCliente = info.IdCliente,
                        IdAlumno = info.IdAlumno,
                        no_fecha = info.no_fecha.Date,
                        no_fecha_venc = info.no_fecha_venc.Date,
                        IdTipoNota = info.IdTipoNota,
                        sc_observacion = info.sc_observacion,
                        Estado = info.Estado = "A",
                        NaturalezaNota = info.NaturalezaNota,
                        IdCtaCble_TipoNota = info.IdCtaCble_TipoNota,
                        IdCobro_tipo = string.IsNullOrEmpty(info.IdCobro_tipo) ? (info.CreDeb == "C" ? "NTCR" : "NTDB"): info.IdCobro_tipo,
                        IdUsuario = info.IdUsuario,
                        FechaTransaccion = DateTime.Now
                    };
                    #endregion

                    #region Detalle
                    foreach (var item in info.lst_det)
                    {
                        db_f.fa_notaCreDeb_det.Add(new fa_notaCreDeb_det
                        {
                            IdEmpresa = info.IdEmpresa,
                            IdSucursal = info.IdSucursal,
                            IdBodega = info.IdBodega,
                            IdNota = info.IdNota,
                            Secuencia = Secuencia++,
                            IdProducto = item.IdProducto,
                            sc_cantidad = item.sc_cantidad,
                            sc_cantidad_factura=item.sc_cantidad_factura,
                            sc_Precio = item.sc_Precio,
                            sc_descUni = item.sc_descUni,
                            sc_PordescUni = item.sc_PordescUni,
                            sc_precioFinal = item.sc_precioFinal,
                            vt_por_iva = item.vt_por_iva,
                            sc_iva = item.sc_iva,
                            IdCod_Impuesto_Iva = item.IdCod_Impuesto_Iva,
                            sc_subtotal = item.sc_subtotal,
                            sc_total = item.sc_total,
                            IdCentroCosto = item.IdCentroCosto,
                            IdPunto_Cargo = item.IdPunto_Cargo,
                            IdPunto_cargo_grupo = item.IdPunto_cargo_grupo
                        });
                    }
                    #endregion

                    #region Resumen
                    fa_notaCreDeb_resumen Entity_Resumen = new fa_notaCreDeb_resumen
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdSucursal = info.IdSucursal,
                        IdBodega = info.IdBodega,
                        IdNota = info.IdNota,
                        SubtotalIVASinDscto = info.info_resumen.SubtotalIVASinDscto,
                        SubtotalSinIVASinDscto = info.info_resumen.SubtotalSinIVASinDscto,
                        SubtotalSinDscto = info.info_resumen.SubtotalSinDscto,
                        Descuento = info.info_resumen.Descuento,
                        SubtotalIVAConDscto = info.info_resumen.SubtotalIVAConDscto,
                        SubtotalSinIVAConDscto = info.info_resumen.SubtotalSinIVAConDscto,
                        SubtotalConDscto = info.info_resumen.SubtotalConDscto,
                        IdCod_Impuesto_IVA = info.info_resumen.IdCod_Impuesto_IVA,
                        ValorIVA = info.info_resumen.ValorIVA,
                        Total = info.info_resumen.Total,
                        PorIva = info.info_resumen.PorIva,
                        IdAnio = info.info_resumen.IdAnio,
                        IdMatricula= info.info_resumen.IdMatricula
                    };
                    db_f.fa_notaCreDeb_resumen.Add(Entity_Resumen);
                    #endregion

                    #region Cruce
                    Secuencia = 1;
                    foreach (var item in info.lst_cruce)
                    {
                        db_f.fa_notaCreDeb_x_fa_factura_NotaDeb.Add(new fa_notaCreDeb_x_fa_factura_NotaDeb
                        {
                            IdEmpresa_nt = info.IdEmpresa,
                            IdSucursal_nt = info.IdSucursal,
                            IdBodega_nt = info.IdBodega,
                            IdNota_nt = info.IdNota,
                            secuencia = Secuencia++,
                            IdEmpresa_fac_nd_doc_mod = item.IdEmpresa_fac_nd_doc_mod,
                            IdSucursal_fac_nd_doc_mod = item.IdSucursal_fac_nd_doc_mod,
                            IdBodega_fac_nd_doc_mod = item.IdBodega_fac_nd_doc_mod,
                            IdCbteVta_fac_nd_doc_mod = item.IdCbteVta_fac_nd_doc_mod,
                            vt_tipoDoc = item.vt_tipoDoc,
                            Valor_Aplicado = item.Valor_Aplicado,
                            fecha_cruce = DateTime.Now,
                            ValorProntoPago = item.ValorProntoPago
                        });
                    }
                    #endregion

                    #region Talonario
                    fa_PuntoVta_Data data_puntovta = new fa_PuntoVta_Data();
                    tb_sis_Documento_Tipo_Talonario_Data data_talonario = new tb_sis_Documento_Tipo_Talonario_Data();
                    fa_PuntoVta_Info info_puntovta = new fa_PuntoVta_Info();
                    tb_sis_Documento_Tipo_Talonario_Info ultimo_talonario = new tb_sis_Documento_Tipo_Talonario_Info();
                    tb_sis_Documento_Tipo_Talonario_Info info_talonario = new tb_sis_Documento_Tipo_Talonario_Info();
                    info_puntovta = data_puntovta.get_info(info.IdEmpresa, info.IdSucursal, info.IdPuntoVta);

                    if (info_puntovta != null && info.NaturalezaNota == "SRI")
                    {
                        if (info_puntovta.EsElectronico == true)
                        {
                            ultimo_talonario = data_talonario.GetUltimoNoUsado(info.IdEmpresa, info_puntovta.codDocumentoTipo, info_puntovta.Su_CodigoEstablecimiento, info_puntovta.cod_PuntoVta, info_puntovta.EsElectronico,true);

                            if (ultimo_talonario != null)
                            {
                                Entity.Serie1 = info.Serie1 = ultimo_talonario.Establecimiento;
                                Entity.Serie2 = info.Serie2 = ultimo_talonario.PuntoEmision;
                                Entity.NumNota_Impresa = info.NumNota_Impresa = ultimo_talonario.NumDocumento;
                            }
                        }
                        else
                        {
                            info_talonario.IdEmpresa = info.IdEmpresa;
                            info_talonario.CodDocumentoTipo = info.CodDocumentoTipo;
                            info_talonario.Establecimiento = info.Serie1;
                            info_talonario.PuntoEmision = info.Serie2;
                            info_talonario.NumDocumento = info.NumNota_Impresa;
                            info_talonario.IdSucursal = info.IdSucursal;
                            info_talonario.Usado = true;

                            data_talonario.modificar_estado_usadoDB(info_talonario);
                        }
                    }
                    else{
                        Entity.Serie1 = null;
                        Entity.Serie2 = null;
                        Entity.NumNota_Impresa = null;
                    }
                    #endregion

                    db_f.fa_notaCreDeb.Add(Entity);
                    db_f.SaveChanges();

                    #endregion

                    #region Contabilidad
                    info.IdCobro_tipo = info.CreDeb == "D" ? "NTDB" : (info.IdCobro_tipo ?? "NTCR");
                    var TipoCobro = odat_TipoCobro.get_info( info.IdCobro_tipo);
                    if (TipoCobro != null)
                    {
                        ct_cbtecble_Info diario = armar_diario(info);

                        if (diario != null)
                        {
                            if (TipoCobro.tc_Tomar_Cta_Cble_De == "CAJA")
                            {
                                caj_Caja_Movimiento_Info MovimientoCaja = armar_movimiendoCaja(info, diario);
                                if (MovimientoCaja != null)
                                {
                                    if (odata_MovimientoCaja.guardarDB(MovimientoCaja))
                                    {
                                        db_f.fa_notaCreDeb_x_ct_cbtecble.Add(new fa_notaCreDeb_x_ct_cbtecble
                                        {
                                            no_IdEmpresa = info.IdEmpresa,
                                            no_IdSucursal = info.IdSucursal,
                                            no_IdBodega = info.IdBodega,
                                            no_IdNota = info.IdNota,

                                            ct_IdEmpresa = diario.IdEmpresa,
                                            ct_IdTipoCbte = diario.IdTipoCbte,
                                            ct_IdCbteCble = MovimientoCaja.IdCbteCble,

                                            observacion = info.CodDocumentoTipo + (info.NaturalezaNota == "SRI" ? ("-" + info.Serie1 + "-" + info.Serie2 + "-" + info.NumNota_Impresa) : info.IdNota.ToString("000000000"))
                                        });
                                        db_f.SaveChanges();
                                    }
                                }
                            }
                            else
                            {
                                if (odata_ct.guardarDB(diario))
                                {
                                    db_f.fa_notaCreDeb_x_ct_cbtecble.Add(new fa_notaCreDeb_x_ct_cbtecble
                                    {
                                        no_IdEmpresa = info.IdEmpresa,
                                        no_IdSucursal = info.IdSucursal,
                                        no_IdBodega = info.IdBodega,
                                        no_IdNota = info.IdNota,

                                        ct_IdEmpresa = diario.IdEmpresa,
                                        ct_IdTipoCbte = diario.IdTipoCbte,
                                        ct_IdCbteCble = diario.IdCbteCble,

                                        observacion = info.CodDocumentoTipo + (info.NaturalezaNota == "SRI" ? ("-" + info.Serie1 + "-" + info.Serie2 + "-" + info.NumNota_Impresa) : info.IdNota.ToString("000000000"))
                                    });
                                    db_f.SaveChanges();
                                }
                            }
                        }
                    }
                    #endregion

                    #region Cobranza
                    if (info.CreDeb.Trim() == "C" && info.lst_cruce.Count != 0 && info.lst_cruce.Where(q=> q.Valor_Aplicado > 0).Count() > 0)
                    {
                        cxc_cobro_Info cobro = armar_cobro(info);
                        if (cobro != null)
                        {
                            if (odata_cobr.guardarDB(cobro))
                            {
                                db_f.fa_notaCreDeb_x_cxc_cobro.Add(new fa_notaCreDeb_x_cxc_cobro
                                {
                                    IdEmpresa_nt = info.IdEmpresa,
                                    IdSucursal_nt = info.IdSucursal,
                                    IdBodega_nt = info.IdBodega,
                                    IdNota_nt = info.IdNota,
                                    IdEmpresa_cbr = cobro.IdEmpresa,
                                    IdSucursal_cbr = cobro.IdSucursal,
                                    IdCobro_cbr = cobro.IdCobro,
                                    Valor_cobro = Math.Round(info.lst_cruce.Sum(q => q.Valor_Aplicado), 2, MidpointRounding.AwayFromZero)
                                });
                                db_f.SaveChanges();
                            }
                        }
                    }

                    #endregion
                }

                return true;
            }
            catch (Exception ex)
            {
                tb_LogError_Data LogData = new tb_LogError_Data();
                LogData.GuardarDB(new tb_LogError_Info { Descripcion = ex.Message, InnerException = ex.InnerException == null ? null : ex.InnerException.Message, Clase = "fa_notaCreDeb_Data", Metodo = "guardarDB", IdUsuario = info.IdUsuario });
                return false;
            }
        }
        public bool modificarDB(fa_notaCreDeb_Info info)
        {
            try
            {
                #region Variables
                int Secuencia = 1;
                ct_cbtecble_Data odata_ct = new ct_cbtecble_Data();
                cxc_cobro_Data odata_cobr = new cxc_cobro_Data();
                cxc_cobro_tipo_Data odat_TipoCobro = new cxc_cobro_tipo_Data();
                caj_Caja_Movimiento_Data odata_MovimientoCaja = new caj_Caja_Movimiento_Data();
                #endregion

                using (EntitiesFacturacion db_f = new EntitiesFacturacion())
                {
                    var TipoNota = db_f.fa_TipoNota.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdTipoNota == info.IdTipoNota).FirstOrDefault();
                    if (TipoNota != null)
                    {
                        info.IdCtaCble_TipoNota = info.CreDeb == "D" ? TipoNota.IdCtaCbleCXC : TipoNota.IdCtaCble;
                    }

                    #region Nota de debito credito

                    #region Cabecera
                    var entity = db_f.fa_notaCreDeb.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdSucursal == info.IdSucursal && q.IdBodega == info.IdBodega && q.IdNota == info.IdNota).FirstOrDefault();
                    if (entity == null) return false;

                    //entity.IdPuntoVta = info.IdPuntoVta;
                    entity.CodNota = info.CodNota;
                    entity.CreDeb = info.CreDeb.Trim();
                    //entity.CodDocumentoTipo = info.CodDocumentoTipo;
                    //entity.Serie1 = info.Serie1;
                    //entity.Serie2 = info.Serie2;
                    //entity.NumNota_Impresa = info.NumNota_Impresa;
                    //entity.NumAutorizacion = info.NumAutorizacion;
                    //entity.Fecha_Autorizacion = info.Fecha_Autorizacion;
                    entity.IdCliente = info.IdCliente;
                    entity.IdAlumno = info.IdAlumno;
                    entity.no_fecha = info.no_fecha.Date;
                    entity.no_fecha_venc = info.no_fecha_venc.Date;
                    entity.IdTipoNota = info.IdTipoNota;
                    entity.sc_observacion = info.sc_observacion;
                    //entity.NaturalezaNota = info.NaturalezaNota;
                    entity.IdCtaCble_TipoNota = info.IdCtaCble_TipoNota;
                    entity.IdUsuarioUltMod = info.IdUsuarioUltMod;
                    entity.Fecha_UltMod = DateTime.Now;

                    #endregion

                    #region Detalle
                    var lst = db_f.fa_notaCreDeb_det.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdSucursal == info.IdSucursal && q.IdBodega == info.IdBodega && q.IdNota == info.IdNota).ToList();
                    db_f.fa_notaCreDeb_det.RemoveRange(lst);

                    foreach (var item in info.lst_det)
                    {
                        db_f.fa_notaCreDeb_det.Add(new fa_notaCreDeb_det
                        {
                            IdEmpresa = info.IdEmpresa,
                            IdSucursal = info.IdSucursal,
                            IdBodega = info.IdBodega,
                            IdNota = info.IdNota,
                            Secuencia = Secuencia++,
                            IdProducto = item.IdProducto,
                            sc_cantidad = item.sc_cantidad,
                            sc_cantidad_factura = item.sc_cantidad_factura,
                            sc_Precio = item.sc_Precio,
                            sc_descUni = item.sc_descUni,
                            sc_PordescUni = item.sc_PordescUni,
                            sc_precioFinal = item.sc_precioFinal,
                            vt_por_iva = item.vt_por_iva,
                            sc_iva = item.sc_iva,
                            IdCod_Impuesto_Iva = item.IdCod_Impuesto_Iva,
                            sc_subtotal = item.sc_subtotal,
                            sc_total = item.sc_total,
                            IdCentroCosto = item.IdCentroCosto,
                            IdPunto_Cargo = item.IdPunto_Cargo,
                            IdPunto_cargo_grupo = item.IdPunto_cargo_grupo
                        });
                    }
                    #endregion

                    #region Resumen
                    fa_notaCreDeb_resumen Entity_Resumen = new fa_notaCreDeb_resumen
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdSucursal = info.IdSucursal,
                        IdBodega = info.IdBodega,
                        IdNota = info.IdNota,
                        SubtotalIVASinDscto = info.info_resumen.SubtotalIVASinDscto,
                        SubtotalSinIVASinDscto = info.info_resumen.SubtotalSinIVASinDscto,
                        SubtotalSinDscto = info.info_resumen.SubtotalSinDscto,
                        Descuento = info.info_resumen.Descuento,
                        SubtotalIVAConDscto = info.info_resumen.SubtotalIVAConDscto,
                        SubtotalSinIVAConDscto = info.info_resumen.SubtotalSinIVAConDscto,
                        SubtotalConDscto = info.info_resumen.SubtotalConDscto,
                        IdCod_Impuesto_IVA = info.info_resumen.IdCod_Impuesto_IVA,
                        ValorIVA = info.info_resumen.ValorIVA,
                        Total = info.info_resumen.Total,
                        PorIva = info.info_resumen.PorIva
                    };

                    var notaDebCred = db_f.fa_notaCreDeb_resumen.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdSucursal == info.IdSucursal && q.IdBodega == info.IdBodega && q.IdNota == info.IdNota).FirstOrDefault();
                    if (notaDebCred != null)
                        db_f.fa_notaCreDeb_resumen.Remove(notaDebCred);

                    db_f.fa_notaCreDeb_resumen.Add(Entity_Resumen);
                    #endregion

                    #region Cruce
                    var lst_cruce = db_f.fa_notaCreDeb_x_fa_factura_NotaDeb.Where(q => q.IdEmpresa_nt == info.IdEmpresa && q.IdSucursal_nt == info.IdSucursal && q.IdBodega_nt == info.IdBodega && q.IdNota_nt == info.IdNota).ToList();
                    db_f.fa_notaCreDeb_x_fa_factura_NotaDeb.RemoveRange(lst_cruce);
                    Secuencia = 1;
                    foreach (var item in info.lst_cruce)
                    {
                        db_f.fa_notaCreDeb_x_fa_factura_NotaDeb.Add(new fa_notaCreDeb_x_fa_factura_NotaDeb
                        {
                            IdEmpresa_nt = info.IdEmpresa,
                            IdSucursal_nt = info.IdSucursal,
                            IdBodega_nt = info.IdBodega,
                            IdNota_nt = info.IdNota,
                            secuencia = Secuencia++,
                            IdEmpresa_fac_nd_doc_mod = item.IdEmpresa_fac_nd_doc_mod,
                            IdSucursal_fac_nd_doc_mod = item.IdSucursal_fac_nd_doc_mod,
                            IdBodega_fac_nd_doc_mod = item.IdBodega_fac_nd_doc_mod,
                            IdCbteVta_fac_nd_doc_mod = item.IdCbteVta_fac_nd_doc_mod,
                            vt_tipoDoc = item.vt_tipoDoc,
                            Valor_Aplicado = item.Valor_Aplicado,
                            fecha_cruce = DateTime.Now,
                            ValorProntoPago = item.ValorProntoPago
                        });
                    }
                    #endregion

                    db_f.SaveChanges();

                    #endregion

                    #region Contabilidad
                    info.IdCobro_tipo = entity.IdCobro_tipo;
                    var TipoCobro = odat_TipoCobro.get_info(entity.IdCobro_tipo);
                    if (TipoCobro != null)
                    {
                        var rel_conta = db_f.fa_notaCreDeb_x_ct_cbtecble.Where(q => q.no_IdEmpresa == info.IdEmpresa && q.no_IdSucursal == info.IdSucursal && q.no_IdBodega == info.IdBodega && q.no_IdNota == info.IdNota).FirstOrDefault();
                        ct_cbtecble_Info diario = armar_diario(info);
                        if (diario != null)
                        {
                            if (rel_conta == null)
                            {
                                if (TipoCobro.tc_Tomar_Cta_Cble_De == "CAJA")
                                {
                                    caj_Caja_Movimiento_Info MovimientoCaja = armar_movimiendoCaja(info, diario);
                                    if (MovimientoCaja != null)
                                    {
                                        if (odata_MovimientoCaja.guardarDB(MovimientoCaja))
                                        {
                                            db_f.fa_notaCreDeb_x_ct_cbtecble.Add(new fa_notaCreDeb_x_ct_cbtecble
                                            {
                                                no_IdEmpresa = info.IdEmpresa,
                                                no_IdSucursal = info.IdSucursal,
                                                no_IdBodega = info.IdBodega,
                                                no_IdNota = info.IdNota,

                                                ct_IdEmpresa = diario.IdEmpresa,
                                                ct_IdTipoCbte = diario.IdTipoCbte,
                                                ct_IdCbteCble = MovimientoCaja.IdCbteCble,

                                                observacion = info.CodDocumentoTipo + (info.NaturalezaNota == "SRI" ? ("-" + info.Serie1 + "-" + info.Serie2 + "-" + info.NumNota_Impresa) : info.IdNota.ToString("000000000"))
                                            });
                                            db_f.SaveChanges();
                                        }
                                    }
                                }
                                else
                                {
                                    if (odata_ct.guardarDB(diario))
                                    {
                                        db_f.fa_notaCreDeb_x_ct_cbtecble.Add(new fa_notaCreDeb_x_ct_cbtecble
                                        {
                                            no_IdEmpresa = info.IdEmpresa,
                                            no_IdSucursal = info.IdSucursal,
                                            no_IdBodega = info.IdBodega,
                                            no_IdNota = info.IdNota,

                                            ct_IdEmpresa = diario.IdEmpresa,
                                            ct_IdTipoCbte = diario.IdTipoCbte,
                                            ct_IdCbteCble = diario.IdCbteCble,

                                            observacion = info.CodDocumentoTipo + (info.NaturalezaNota == "SRI" ? ("-" + info.Serie1 + "-" + info.Serie2 + "-" + info.NumNota_Impresa) : info.IdNota.ToString("000000000"))
                                        });
                                        db_f.SaveChanges();
                                    }
                                }
                            }
                            else
                            {
                                diario.IdCbteCble = rel_conta.ct_IdCbteCble;
                                if (TipoCobro.tc_Tomar_Cta_Cble_De == "CAJA")
                                {
                                    caj_Caja_Movimiento_Info MovimientoCaja = armar_movimiendoCaja(info, diario);
                                    if (MovimientoCaja != null)
                                    {
                                        MovimientoCaja.IdCbteCble = rel_conta.ct_IdCbteCble;
                                        odata_MovimientoCaja.modificarDB(MovimientoCaja);
                                    }
                                }
                                else
                                {   
                                    odata_ct.modificarDB(diario);
                                }
                            }
                        }
                    }
                        
                    #endregion

                    #region Cobranza
                    if (info.CreDeb.Trim() == "C" && info.lst_cruce.Count != 0)
                    {
                        cxc_cobro_Info cobro = armar_cobro(info);                                                
                        if (cobro != null)
                        {
                            var rel_cobr = db_f.fa_notaCreDeb_x_cxc_cobro.Where(q => q.IdEmpresa_nt == info.IdEmpresa && q.IdSucursal_nt == info.IdSucursal && q.IdBodega_nt == info.IdBodega && q.IdNota_nt == info.IdNota).FirstOrDefault();
                            if (rel_cobr == null)
                            {
                                if (odata_cobr.guardarDB(cobro))
                                {
                                    db_f.fa_notaCreDeb_x_cxc_cobro.Add(new fa_notaCreDeb_x_cxc_cobro
                                    {
                                        IdEmpresa_nt = info.IdEmpresa,
                                        IdSucursal_nt = info.IdSucursal,
                                        IdBodega_nt = info.IdBodega,
                                        IdNota_nt = info.IdNota,
                                        IdEmpresa_cbr = cobro.IdEmpresa,
                                        IdSucursal_cbr = cobro.IdSucursal,
                                        IdCobro_cbr = cobro.IdCobro,
                                        Valor_cobro = Math.Round(info.lst_cruce.Sum(q => q.Valor_Aplicado), 2, MidpointRounding.AwayFromZero)
                                    });
                                    db_f.SaveChanges();
                                }
                            }else
                            {
                                cobro.IdCobro = rel_cobr.IdCobro_cbr;
                                odata_cobr.modificarDB(cobro);
                            }
                        }
                    }

                    #endregion
                }

                return true;
            }
            catch (Exception ex)
            {
                tb_LogError_Data LogData = new tb_LogError_Data();
                LogData.GuardarDB(new tb_LogError_Info { Descripcion = ex.Message, InnerException = ex.InnerException == null ? null : ex.InnerException.Message, Clase = "fa_notaCreDeb_Data", Metodo = "modificarDB", IdUsuario = info.IdUsuario });
                return false;
            }
        }
        public bool anularDB(fa_notaCreDeb_Info info)
        {
            try
            {
                #region Variables
                ct_cbtecble_Data odata_ct = new ct_cbtecble_Data();
                cxc_cobro_Data odata_cobr = new cxc_cobro_Data();
                #endregion
                
                using (EntitiesFacturacion db_f = new EntitiesFacturacion())
                {
                    #region Nota de debito credito

                    #region Cabecera
                    var entity = db_f.fa_notaCreDeb.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdSucursal == info.IdSucursal && q.IdBodega == info.IdBodega && q.IdNota == info.IdNota).FirstOrDefault();
                    if (entity == null) return false;

                    entity.Estado = "I";
                    entity.IdUsuarioUltAnu = info.IdUsuarioUltAnu;
                    entity.Fecha_UltAnu = DateTime.Now;
                    entity.MotiAnula = info.MotiAnula;
                    #endregion

                    var lst_cruce = db_f.fa_notaCreDeb_x_fa_factura_NotaDeb.Where(q => q.IdEmpresa_nt == info.IdEmpresa && q.IdSucursal_nt == info.IdSucursal && q.IdBodega_nt == info.IdBodega && q.IdNota_nt == info.IdNota).ToList();
                    db_f.fa_notaCreDeb_x_fa_factura_NotaDeb.RemoveRange(lst_cruce);
                    #endregion

                    #region Contabilidad
                    var rel_conta = db_f.fa_notaCreDeb_x_ct_cbtecble.Where(q => q.no_IdEmpresa == info.IdEmpresa && q.no_IdSucursal == info.IdSucursal && q.no_IdBodega == info.IdBodega && q.no_IdNota == info.IdNota).FirstOrDefault();
                    if (rel_conta != null)
                    {
                        var MovCaj = odataMovCaja.get_info(rel_conta.ct_IdEmpresa, rel_conta.ct_IdTipoCbte, rel_conta.ct_IdCbteCble);
                        if (MovCaj != null)
                        {
                            if(!odataMovCaja.anularDB(new caj_Caja_Movimiento_Info
                            {
                                IdEmpresa = rel_conta.ct_IdEmpresa,
                                IdTipocbte = rel_conta.ct_IdTipoCbte,
                                IdCbteCble = rel_conta.ct_IdCbteCble
                            }))
                            {
                                entity.Estado = "A";
                                entity.IdUsuarioUltAnu = null;
                                entity.Fecha_UltAnu = null;
                                entity.MotiAnula = null;
                            }
                        }else
                        if (!odata_ct.anularDB(new ct_cbtecble_Info { IdEmpresa = rel_conta.ct_IdEmpresa, IdTipoCbte = rel_conta.ct_IdTipoCbte, IdCbteCble = rel_conta.ct_IdCbteCble, IdUsuarioAnu = info.IdUsuarioUltAnu }))
                        {
                            entity.Estado = "A";
                            entity.IdUsuarioUltAnu = null;
                            entity.Fecha_UltAnu = null;
                            entity.MotiAnula = null;
                        }
                    }

                    #endregion

                    #region Cobranza

                    var rel_cobr = db_f.fa_notaCreDeb_x_cxc_cobro.Where(q => q.IdEmpresa_nt == info.IdEmpresa && q.IdSucursal_nt == info.IdSucursal && q.IdBodega_nt == info.IdBodega && q.IdNota_nt == info.IdNota).FirstOrDefault();
                    if (rel_cobr != null)
                    {
                        if (lst_cruce.Count > 0)
                        {
                            if (!odata_cobr.anularDB(new cxc_cobro_Info { IdEmpresa = rel_cobr.IdEmpresa_cbr, IdSucursal = rel_cobr.IdSucursal_cbr, IdCobro = rel_cobr.IdCobro_cbr, IdUsuarioUltAnu = info.IdUsuarioUltAnu }))
                            {
                                entity.Estado = "A";
                                entity.IdUsuarioUltAnu = null;
                                entity.Fecha_UltAnu = null;
                                entity.MotiAnula = null;
                            }
                        }
                    }
                    #endregion

                    db_f.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private cxc_cobro_Info armar_cobro(fa_notaCreDeb_Info info)
        {
            try
            {
                cxc_cobro_Info cobro = new cxc_cobro_Info
                {
                    IdEmpresa = info.IdEmpresa,
                    IdSucursal = info.IdSucursal,
                    IdCobro = 0,
                    IdCobro_tipo = info.CreDeb.Trim() == "C" ? "NTCR" : "NTDB",
                    cr_fecha = info.no_fecha,
                    cr_fechaCobro = info.no_fecha,
                    cr_fechaDocu = info.no_fecha,
                    cr_NumDocumento = info.CodDocumentoTipo +  (info.NaturalezaNota == "SRI" ? ("-" + info.Serie1 + "-" + info.Serie2 + "-" + info.NumNota_Impresa) : info.IdNota.ToString("000000000")),
                    cr_observacion = info.CodDocumentoTipo + (info.NaturalezaNota == "SRI" ? ("-" + info.Serie1 + "-" + info.Serie2 + "-" + info.NumNota_Impresa) : info.IdNota.ToString("000000000")),
                    cr_TotalCobro = Math.Round(info.lst_cruce.Sum(q=>q.Valor_Aplicado),2,MidpointRounding.AwayFromZero),
                    IdCaja = 1,
                    IdCliente = info.IdCliente,
                    IdAlumno = info.IdAlumno,
                    IdUsuario = info.IdUsuario,
                    IdTipoNotaCredito = info.IdTipoNota,
                    IdUsuarioUltMod = info.IdUsuarioUltMod,
                    lst_det = new List<cxc_cobro_det_Info>()
                };

                int Secuencia = 1;
                foreach (var item in info.lst_cruce.Where(q=> q.Valor_Aplicado > 0).ToList())
                {
                    cobro.lst_det.Add(new cxc_cobro_det_Info
                    {
                        IdEmpresa = cobro.IdEmpresa,
                        IdSucursal = cobro.IdSucursal,
                        IdCobro = cobro.IdCobro,
                        secuencial = Secuencia++,
                        IdCobro_tipo_det = cobro.IdCobro_tipo,
                        IdBodega_Cbte = item.IdBodega_fac_nd_doc_mod,
                        IdCbte_vta_nota = item.IdCbteVta_fac_nd_doc_mod,
                        dc_TipoDocumento = item.vt_tipoDoc,
                        dc_ValorPago = item.Valor_Aplicado,  
                        dc_ValorProntoPago = item.ValorProntoPago                      
                    });
                }

                return cobro;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private ct_cbtecble_Info armar_diario(fa_notaCreDeb_Info info)
        {
            try
            {
                EntitiesCaja dbCaj = new EntitiesCaja();
                EntitiesCuentasPorCobrar dbCxc = new EntitiesCuentasPorCobrar();
                EntitiesFacturacion db = new EntitiesFacturacion();
                
                    var lst = GetCtaCbleNCND(info.IdEmpresa,info.IdSucursal,info.IdBodega,info.IdNota);
                    var NCND = lst.Count > 0 ? lst[0] : null;
                    if (NCND == null)
                        return null;
                    
                    var paramFac = db.fa_parametro.Where(q => q.IdEmpresa == info.IdEmpresa).FirstOrDefault();
                    if (paramFac == null)
                        return null;

                    var paramCaj = dbCaj.caj_parametro.Where(q => q.IdEmpresa == info.IdEmpresa).FirstOrDefault();
                    if (paramCaj == null)
                        return null;

                    var TipoCobro = dbCxc.cxc_cobro_tipo.Where(q => q.IdCobro_tipo == info.IdCobro_tipo).FirstOrDefault();
                    if (TipoCobro == null)
                        return null;

                    var ptoVta = db.vwfa_PuntoVta.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdPuntoVta == info.IdPuntoVta).FirstOrDefault();
                    if (ptoVta == null)
                        return null;

                    var Caja = dbCaj.caj_Caja.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdCaja == ptoVta.IdCaja).FirstOrDefault();
                    if (Caja == null)
                        return null;

                    var TipoCobroCuenta = dbCxc.cxc_cobro_tipo_Param_conta_x_sucursal.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdSucursal == info.IdSucursal && q.IdCobro_tipo == TipoCobro.IdCobro_tipo).FirstOrDefault();
                    
                    
                    #region Cabecera
                    ct_cbtecble_Info diario = new ct_cbtecble_Info
                    {
                        IdEmpresa = info.IdEmpresa,
                        IdTipoCbte = TipoCobro.tc_Tomar_Cta_Cble_De == "CAJA" ? paramCaj.IdTipoCbteCble_MoviCaja_Ing : (info.CreDeb == "C" ? paramFac.IdTipoCbteCble_NC : paramFac.IdTipoCbteCble_ND),
                        IdCbteCble = 0,
                        cb_Fecha = info.no_fecha.Date,
                        IdSucursal = info.IdSucursal,
                        IdPeriodo = Convert.ToInt32(info.no_fecha.ToString("yyyyMM")),
                        IdUsuario = info.IdUsuario,
                        IdUsuarioUltModi = info.IdUsuarioUltMod,
                        cb_Observacion = NCND.Observacion,
                        CodCbteCble = info.CodDocumentoTipo + (info.NaturalezaNota == "SRI" ? info.NumNota_Impresa : info.IdNota.ToString()),
                        cb_Valor = 0,
                        lst_ct_cbtecble_det = new List<ct_cbtecble_det_Info>()
                    };
                    #endregion

                    int secuencia = 1;

                #region Debe
                diario.lst_ct_cbtecble_det.Add(new ct_cbtecble_det_Info
                {
                    IdEmpresa = diario.IdEmpresa,
                    IdTipoCbte = diario.IdTipoCbte,
                    IdCbteCble = diario.IdCbteCble,
                    secuencia = secuencia++,
                    IdCtaCble = TipoCobro.tc_Tomar_Cta_Cble_De == "CAJA" ? Caja.IdCtaCble : (TipoCobro.tc_Tomar_Cta_Cble_De == "TIP_COBRO" ? (TipoCobroCuenta != null ? TipoCobroCuenta.IdCtaCble : NCND.IdCtaCbleDebe): NCND.IdCtaCbleDebe),
                    dc_Valor = Math.Round(lst.Sum(q => q.Valor),2,MidpointRounding.AwayFromZero),
                });
                #endregion

                #region Haber
                foreach (var item in lst)
                {
                    diario.lst_ct_cbtecble_det.Add(new ct_cbtecble_det_Info
                    {
                        IdEmpresa = diario.IdEmpresa,
                        IdTipoCbte = diario.IdTipoCbte,
                        IdCbteCble = diario.IdCbteCble,
                        IdCtaCble = item.IdCtaCbleHaber,
                        secuencia = secuencia++,
                        dc_Valor = Math.Round(item.Valor, 2, MidpointRounding.AwayFromZero)*-1,
                        dc_Observacion = item.DocumentoCruce
                    });
                }
                #endregion

                if (diario.lst_ct_cbtecble_det.Count == 0)
                        return null;

                    if (Math.Round(diario.lst_ct_cbtecble_det.Sum(q => q.dc_Valor), 2, MidpointRounding.AwayFromZero) != 0)
                        return null;

                    if (diario.lst_ct_cbtecble_det.Where(q => q.dc_Valor == 0).Count() > 0)
                        return null;

                    return diario;
            
                
            }
            catch (Exception ex)
            {
                tb_LogError_Data LogData = new tb_LogError_Data();
                LogData.GuardarDB(new tb_LogError_Info { Descripcion = ex.Message, InnerException = ex.InnerException == null ? null : ex.InnerException.Message, Clase = "fa_notaCreDeb", Metodo = "armar_diario", IdUsuario = info.IdUsuario });
                return null;
            }
        }
        private caj_Caja_Movimiento_Info armar_movimiendoCaja(fa_notaCreDeb_Info infoNC, ct_cbtecble_Info infoCT)
        {
            try
            {
                EntitiesCaja dbCaj = new EntitiesCaja();
                EntitiesCuentasPorCobrar dbCxc = new EntitiesCuentasPorCobrar();
                EntitiesFacturacion dbFac = new EntitiesFacturacion();
                EntitiesAcademico dbAca = new EntitiesAcademico();

                var paramCaj = dbCaj.caj_parametro.Where(q => q.IdEmpresa == infoNC.IdEmpresa).FirstOrDefault();
                if (paramCaj == null)
                    return null;

                var paramCxc = dbCxc.cxc_Parametro.Where(q => q.IdEmpresa == infoNC.IdEmpresa).FirstOrDefault();
                if (paramCxc == null)
                    return null;

                var ptoVta = dbFac.vwfa_PuntoVta.Where(q => q.IdEmpresa == infoNC.IdEmpresa && q.IdPuntoVta == infoNC.IdPuntoVta).FirstOrDefault();
                if (ptoVta == null)
                    return null;

                //var Alumno = dbAca.vwaca_Alumno.Where(q => q.IdEmpresa == infoNC.IdEmpresa && q.IdAlumno == infoNC.IdAlumno).FirstOrDefault();
                var Alumno = DataAlumno.getInfo(infoNC.IdEmpresa, Convert.ToDecimal(infoNC.IdAlumno));
                if (Alumno == null)
                    return null;

                caj_Caja_Movimiento_Info Movimiento = new caj_Caja_Movimiento_Info
                {
                    IdEmpresa = infoNC.IdEmpresa,
                    IdTipocbte = paramCaj.IdTipoCbteCble_MoviCaja_Ing,
                    IdTipoMovi = paramCxc.pa_IdTipoMoviCaja_x_Cobros_x_cliente,
                    IdCaja = ptoVta.IdCaja,
                    IdTipo_Persona = "ALUMNO",
                    IdPersona = Alumno.IdPersona,
                    IdEntidad  = Alumno.IdAlumno,
                    cm_fecha = infoNC.no_fecha,
                    cm_observacion = infoNC.sc_observacion,
                    cm_Signo = "+",
                    cm_valor = Convert.ToDouble(infoNC.info_resumen.Total),
                    info_caj_Caja_Movimiento_det = new caj_Caja_Movimiento_det_Info
                    {
                        cr_Valor = Convert.ToDouble(infoNC.info_resumen.Total),
                        IdCobro_tipo = infoNC.IdCobro_tipo
                    },
                    IdUsuario = infoNC.IdUsuario,
                    lst_ct_cbtecble_det = infoCT.lst_ct_cbtecble_det
                };

                return Movimiento;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool modificarEstadoAutorizacion(int IdEmpresa, int IdSucursal, int IdBodega, decimal IdNota)
        {
            try
            {
                using (EntitiesFacturacion Context = new EntitiesFacturacion())
                {
                    var Entity = Context.fa_notaCreDeb.Where(q => q.IdEmpresa == IdEmpresa && q.IdSucursal == IdSucursal && q.IdBodega == IdBodega && q.IdNota == IdNota).FirstOrDefault();
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

        public List<fa_notaCreDeb_Info> get_list_bajo_demanda(ListEditItemsRequestedByFilterConditionEventArgs args, int IdEmpresa, decimal IdAlumno)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;
            List<fa_notaCreDeb_Info> Lista = new List<fa_notaCreDeb_Info>();
            Lista = GetListNCConSaldo(IdEmpresa,IdAlumno, skip, take, args.Filter);
            return Lista;
        }

        public List<fa_notaCreDeb_Info> GetListNCConSaldo(int IdEmpresa, decimal IdAlumno,int Skip, int take, string Filter)
        {
            try
            {
                List<fa_notaCreDeb_Info> Lista = new List<fa_notaCreDeb_Info>();

                using (EntitiesFacturacion db = new EntitiesFacturacion())
                {
                    db.SetCommandTimeOut(6000); // By Acueva 2022-01-31
                    var lst = db.vwfa_notaCreDeb_ParaConciliarNC.Where(q => q.IdEmpresa == IdEmpresa && q.IdAlumno == IdAlumno).ToList().Skip(Skip).Take(take).OrderBy(q => q.IdNota);

                    foreach (var item in lst)
                    {
                        Lista.Add(new fa_notaCreDeb_Info
                        {
                            IdEmpresa = item.IdEmpresa,
                            IdSucursal = item.IdSucursal,
                            IdBodega = item.IdBodega,
                            IdNota = item.IdNota,
                            no_fecha = item.no_fecha,
                            sc_observacion = item.sc_observacion,
                            sc_total = Convert.ToDouble(item.Total),
                            sc_saldo = Math.Round(Convert.ToDouble(item.Total - item.Valor_Aplicado),2,MidpointRounding.AwayFromZero),
                            IdString = item.IdSucursal.ToString().PadLeft(4,'0') + item.IdBodega.ToString().PadLeft(4, '0') + item.IdNota.ToString().PadLeft(10, '0'),
                            NumNota_Impresa = item.NumNota
                        });
                    }
                }

                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public fa_notaCreDeb_Info get_info_bajo_demanda(ListEditItemRequestedByValueEventArgs args, int IdEmpresa)
        {
            if (args.Value == null)
                return null;
            return get_info(IdEmpresa, args.Value.ToString());
        }
        public fa_notaCreDeb_Info get_info(int IdEmpresa,string IdString)
        {
            try
            {
                fa_notaCreDeb_Info info;

                using (EntitiesFacturacion Context = new EntitiesFacturacion())
                {
                    int IdSucursal = string.IsNullOrEmpty(IdString) ? 0 : Convert.ToInt32(IdString.Substring(0, 4));
                    int IdBodega = string.IsNullOrEmpty(IdString) ? 0 : Convert.ToInt32(IdString.Substring(4, 4));
                    int IdNota = string.IsNullOrEmpty(IdString) ? 0 : Convert.ToInt32(IdString.Substring(8, 10));
                    if (IdNota == 0)
                        return null;

                    var Entity = Context.vwfa_notaCreDeb.Where(q => q.IdEmpresa == IdEmpresa && q.IdSucursal == IdSucursal && q.IdBodega == IdBodega && q.IdNota == IdNota).FirstOrDefault();
                    if (Entity == null) return null;
                    info = new fa_notaCreDeb_Info
                    {
                        IdEmpresa = Entity.IdEmpresa,
                        IdSucursal = Entity.IdSucursal,
                        IdBodega = Entity.IdBodega,
                        IdNota = Entity.IdNota,
                        no_fecha = Entity.no_fecha,
                        sc_observacion = Entity.sc_observacion,
                        sc_total = Convert.ToDouble(Entity.sc_total),
                        NumNota_Impresa = Entity.NumNota_Impresa,
                        IdString = Entity.IdSucursal.ToString("0000") + Entity.IdBodega.ToString("0000") + Entity.IdNota.ToString("0000000000")
                    };

                    var NCSaldo = Context.vwfa_notaCreDeb_ParaConciliarNC.Where(q => q.IdEmpresa == IdEmpresa && q.IdSucursal == IdSucursal && q.IdBodega == IdBodega && q.IdNota == IdNota).FirstOrDefault();
                    if (NCSaldo != null)
                    {
                        info.sc_saldo = Math.Round(Convert.ToDouble(NCSaldo.Total - NCSaldo.Valor_Aplicado ?? 0), 2, MidpointRounding.AwayFromZero);
                    }
                }
                return info;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<fa_notaCreDeb_Info> get_list_credito_favor(int IdEmpresa, decimal IdAlumno)
        {
            try
            {
                List<fa_notaCreDeb_Info> Lista = new List<fa_notaCreDeb_Info>();
                using (EntitiesFacturacion Context = new EntitiesFacturacion())
                {
                    var lst = Context.vwfa_notaCreDeb_ParaConciliarNC.Where(q=> q.IdEmpresa == IdEmpresa && q.IdAlumno == IdAlumno).ToList();

                    foreach (var item in lst)
                    {
                        var info = new fa_notaCreDeb_Info
                        {
                            IdEmpresa = item.IdEmpresa,
                            IdSucursal = item.IdSucursal,
                            IdBodega = item.IdBodega,
                            IdNota = item.IdNota,
                            IdAlumno = item.IdAlumno,
                            sc_saldo = Convert.ToDouble(item.Saldo)
                        };
                        Lista.Add(info);
                    }
                }
                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<fa_notaCreDeb_Info> get_list_aplicacion_masiva(int IdEmpresa)
        {
            try
            {
                List<fa_notaCreDeb_Info> Lista = new List<fa_notaCreDeb_Info>();
                using (EntitiesFacturacion Context = new EntitiesFacturacion())
                {
                    Context.SetCommandTimeOut(3000);// by Acueva 2022-01-31
                    var lst = Context.vwfa_notaCreDeb_ParaConciliarNC.Where(q => q.IdEmpresa == IdEmpresa).ToList();

                    foreach (var item in lst)
                    {
                        var info = new fa_notaCreDeb_Info
                        {
                            IdEmpresa = item.IdEmpresa,
                            IdSucursal = item.IdSucursal,
                            IdBodega = item.IdBodega,
                            IdNota = item.IdNota,
                            IdAlumno = item.IdAlumno,
                            NomAlumno = item.pe_nombreCompleto,
                            sc_saldo = Convert.ToDouble(item.Saldo)
                        };
                        Lista.Add(info);
                    }
                    //Lista = Context.vwfa_notaCreDeb_ParaConciliarNC.Where(q => q.IdEmpresa == IdEmpresa && q.IdAlumno==4605).GroupBy(q => new { q.IdEmpresa, q.IdAlumno, q.pe_nombreCompleto }).Select(q => new fa_notaCreDeb_Info
                    //{
                    //    IdEmpresa = q.Key.IdEmpresa,
                    //    IdAlumno = q.Key.IdAlumno,
                    //    NomAlumno = q.Key.pe_nombreCompleto,
                    //    sc_saldo = (double?)q.Sum(h => h.Saldo)
                    //}).ToList();
                }
                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool Contabilizar(int IdEmpresa, int IdSucursal, int IdBodega, decimal IdNota)
        {
            try
            {
                EntitiesFacturacion db_f = new EntitiesFacturacion();
                var info = get_info(IdEmpresa, IdSucursal, IdBodega, IdNota);
                if (info == null || info.Estado == "I")
                    return false;

                info.lst_det = odata_Det.get_list(IdEmpresa, IdSucursal, IdBodega, IdNota);
                info.lst_cruce = odata_DetNDFac.get_list(IdEmpresa, IdSucursal, IdBodega, IdNota);

                var TipoCobro = odat_TipoCobro.get_info(info.IdCobro_tipo);
                if (TipoCobro != null)
                {
                    var rel_conta = db_f.fa_notaCreDeb_x_ct_cbtecble.Where(q => q.no_IdEmpresa == info.IdEmpresa && q.no_IdSucursal == info.IdSucursal && q.no_IdBodega == info.IdBodega && q.no_IdNota == info.IdNota).FirstOrDefault();
                    ct_cbtecble_Info diario = armar_diario(info);
                    if (diario != null)
                    {
                        if (rel_conta == null)
                        {
                            if (TipoCobro.tc_Tomar_Cta_Cble_De == "CAJA")
                            {
                                caj_Caja_Movimiento_Info MovimientoCaja = armar_movimiendoCaja(info, diario);
                                if (MovimientoCaja != null)
                                {
                                    if (odata_MovimientoCaja.guardarDB(MovimientoCaja))
                                    {
                                        db_f.fa_notaCreDeb_x_ct_cbtecble.Add(new fa_notaCreDeb_x_ct_cbtecble
                                        {
                                            no_IdEmpresa = info.IdEmpresa,
                                            no_IdSucursal = info.IdSucursal,
                                            no_IdBodega = info.IdBodega,
                                            no_IdNota = info.IdNota,

                                            ct_IdEmpresa = diario.IdEmpresa,
                                            ct_IdTipoCbte = diario.IdTipoCbte,
                                            ct_IdCbteCble = MovimientoCaja.IdCbteCble,

                                            observacion = info.CodDocumentoTipo + (info.NaturalezaNota == "SRI" ? ("-" + info.Serie1 + "-" + info.Serie2 + "-" + info.NumNota_Impresa) : info.IdNota.ToString("000000000"))
                                        });
                                        db_f.SaveChanges();
                                    }
                                }
                            }
                            else
                            {
                                if (odata_ct.guardarDB(diario))
                                {
                                    db_f.fa_notaCreDeb_x_ct_cbtecble.Add(new fa_notaCreDeb_x_ct_cbtecble
                                    {
                                        no_IdEmpresa = info.IdEmpresa,
                                        no_IdSucursal = info.IdSucursal,
                                        no_IdBodega = info.IdBodega,
                                        no_IdNota = info.IdNota,

                                        ct_IdEmpresa = diario.IdEmpresa,
                                        ct_IdTipoCbte = diario.IdTipoCbte,
                                        ct_IdCbteCble = diario.IdCbteCble,

                                        observacion = info.CodDocumentoTipo + (info.NaturalezaNota == "SRI" ? ("-" + info.Serie1 + "-" + info.Serie2 + "-" + info.NumNota_Impresa) : info.IdNota.ToString("000000000"))
                                    });
                                    db_f.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            diario.IdCbteCble = rel_conta.ct_IdCbteCble;
                            if (TipoCobro.tc_Tomar_Cta_Cble_De == "CAJA")
                            {
                                caj_Caja_Movimiento_Info MovimientoCaja = armar_movimiendoCaja(info, diario);
                                if (MovimientoCaja != null)
                                {
                                    MovimientoCaja.IdCbteCble = rel_conta.ct_IdCbteCble;
                                    odata_MovimientoCaja.modificarDB(MovimientoCaja);
                                }
                            }
                            else
                            {
                                odata_ct.modificarDB(diario);
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<fa_notaCreDeb_ParaContabilizar> GetCtaCbleNCND(int IdEmpresa, int IdSucursal, int IdBodega, decimal IdNota)
        {
            try
            {
                List<fa_notaCreDeb_ParaContabilizar> Lista = new List<fa_notaCreDeb_ParaContabilizar>();

                using (SqlConnection connection = new SqlConnection(CadenaDeConexion.GetConnectionString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = "DECLARE @IdEmpresa int = "+IdEmpresa.ToString()+", @IdSucursal int = "+IdSucursal.ToString()+", @IdBodega int = "+IdBodega.ToString()+", @IdNota numeric = "+IdNota.ToString()
                    + " SELECT A.IdEmpresa, A.IdSucursal, A.IdBodega, A.IdNota, 1 QueryNumber, 'NC CON CRUCE CON CONCILIACION' Tipo, c.IdCtaCbleCXC IdCtaCbleDebe, c.IdCtaCble IdCtaCbleHaber, '' DocumentoCruce, d.Total as Valor, "
                      + " case when A.NaturalezaNota = 'INT' then 'NTCR ID ' + cast(A.IdNota as varchar) ELSE ISNULL(A.CodDocumentoTipo, '') +' ' + ISNULL(A.Serie1, '') + '-' + ISNULL(A.Serie2, '') + '-' + ISNULL(A.NumNota_Impresa, '') END +"
                         + " ' CLIENTE: ' + l.pe_nombreCompleto + ' ALUMNO: ' + n.pe_nombreCompleto + ' OBS: ' + isnull(a.sc_observacion, '') as Observacion"
                      + " FROM fa_notaCreDeb AS A WITH(NOLOCK) join"
                      + " fa_notaCreDeb_x_fa_factura_NotaDeb as b WITH(NOLOCK) on a.IdEmpresa = b.IdEmpresa_nt and a.IdSucursal = b.IdSucursal_nt and a.IdBodega = b.IdBodega_nt and a.IdNota = b.IdNota_nt left join"
                      + " fa_TipoNota as c WITH(NOLOCK) on a.IdEmpresa = c.IdEmpresa and a.IdTipoNota = c.IdTipoNota  LEFT JOIN"
                      + " fa_notaCreDeb_resumen as d WITH(NOLOCK) on a.IdEmpresa = d.IdEmpresa and a.IdSucursal = d.IdSucursal and a.IdBodega = d.IdBodega and a.IdNota = d.IdNota LEFT JOIN"
                      + " fa_cliente as k WITH(NOLOCK) on a.IdEmpresa = k.IdEmpresa and a.IdCliente = k.IdCliente LEFT JOIN"
                      + " tb_persona as l WITH(NOLOCK) on l.IdPersona = k.IdPersona LEFT JOIN"
                      + " aca_Alumno as m WITH(NOLOCK) on a.IdEmpresa = m.IdEmpresa and a.IdAlumno = m.IdAlumno LEFT JOIN"
                      + " tb_persona as n WITH(NOLOCK) on m.IdPersona = n.IdPersona"
                      + " where a.CreDeb = 'C'"
                      + " and exists("
                      + " select x.IdEmpresa from cxc_ConciliacionNotaCredito as x WITH(NOLOCK)"
                      + " where a.IdEmpresa = x.IdEmpresa and a.IdSucursal = x.IdSucursal and a.IdBodega = x.IdBodega and a.IdNota = x.IdNota"
                      + " and x.Estado = 1"
                      + " ) and not exists("
                          + " select x.IdEmpresa_nt from fa_notaCreDeb_x_fa_factura_NotaDeb as x WITH(NOLOCK)"
                                                + " where a.IdEmpresa = x.IdEmpresa_nt and a.IdSucursal = x.IdSucursal_nt and a.IdBodega = x.IdBodega_nt and a.IdNota = X.IdNota_nt and x.Valor_Aplicado = 0"
                      + " )"
                      + " and a.IdEmpresa = @IdEmpresa and a.IdSucursal = @IdSucursal and a.IdBodega = @IdBodega and a.IdNota = @IdNota and b.Valor_Aplicado <> 0"
                      + " GROUP BY A.IdEmpresa, A.IdSucursal, A.IdBodega, A.IdNota,c.IdCtaCbleCXC, c.IdCtaCble,d.Total, l.pe_nombreCompleto, n.pe_nombreCompleto, a.sc_observacion, "
                      + " A.NaturalezaNota, A.CodDocumentoTipo, A.Serie1, A.Serie2, A.NumNota_Impresa"
                      + " UNION ALL"
                      + " SELECT A.IdEmpresa, A.IdSucursal, A.IdBodega, A.IdNota, 2, 'NC CON CRUCE SIN CONCILIACION', j.IdCtaCbleCXC,  "
                      + " CASE WHEN b.vt_tipoDoc = 'FACT'"
                      + " THEN CASE WHEN G.EnCurso = 1 or year(g.FechaDesde) >= year(xa.FechaDesde) THEN F.IdCtaCbleDebe ELSE ( CASE WHEN b.IdCbteVta_fac_nd_doc_mod = 64902 then '0104009002' else G.IdCtaCbleCierre end ) END"
                      + " WHEN b.vt_tipoDoc = 'NTDB'"
                      + " THEN H.IdCtaCble_TipoNota"
                      + " END,"
                      + " CASE WHEN b.vt_tipoDoc = 'FACT'"
                      + " THEN 'FACT ' + d.vt_serie1 + '-' + d.vt_serie2 + '-' + d.vt_NumFactura"
                      + " WHEN b.vt_tipoDoc = 'NTDB'"
                      + " THEN case when h.NaturalezaNota = 'INT' then 'NTDB ID ' + cast(h.IdNota as varchar) ELSE ISNULL(H.CodDocumentoTipo, '') +' ' + ISNULL(H.Serie1, '') + '-' + ISNULL(H.Serie2, '') + '-' + ISNULL(H.NumNota_Impresa, '') END"
                         + " END DocumentoCruce, "
                      + " b.Valor_Aplicado, "
                      + " case when A.NaturalezaNota = 'INT' then 'NTCR ID ' + cast(A.IdNota as varchar) ELSE ISNULL(A.CodDocumentoTipo, '') +' ' + ISNULL(A.Serie1, '') + '-' + ISNULL(A.Serie2, '') + '-' + ISNULL(A.NumNota_Impresa, '') END +"
                         + " ' CLIENTE: ' + l.pe_nombreCompleto + ' ALUMNO: ' + n.pe_nombreCompleto + ' OBS: ' + isnull(a.sc_observacion, '') as Observacion"
                      + " FROM fa_notaCreDeb AS A WITH(NOLOCK) join"
                      + " fa_notaCreDeb_x_fa_factura_NotaDeb as b WITH(NOLOCK) on a.IdEmpresa = b.IdEmpresa_nt and a.IdSucursal = b.IdSucursal_nt and a.IdBodega = b.IdBodega_nt and a.IdNota = b.IdNota_nt LEFT JOIN"
                      + " fa_factura as d WITH(NOLOCK) on b.IdEmpresa_fac_nd_doc_mod = d.IdEmpresa and b.IdSucursal_fac_nd_doc_mod = d.IdSucursal and b.IdBodega_fac_nd_doc_mod = d.IdBodega and b.IdCbteVta_fac_nd_doc_mod = d.IdCbteVta and b.vt_tipoDoc = d.vt_tipoDoc left join"
                      + " aca_Matricula_Rubro as e WITH(NOLOCK) on e.IdEmpresa = d.IdEmpresa and e.IdSucursal = d.IdSucursal and e.IdBodega = d.IdBodega and e.IdCbteVta = d.IdCbteVta left join"
                      + " aca_AnioLectivo_Curso_Plantilla_Parametrizacion as f WITH(NOLOCK)on f.IdEmpresa = e.IdEmpresa and f.IdAnio = e.IdAnio and f.IdSede = e.IdSede and f.IdNivel = e.IdNivel and f.IdJornada = e.IdJornada and f.IdCurso = e.IdCurso and f.IdPlantilla = e.IdPlantilla and f.IdRubro = e.IdRubro left join"
                      + " aca_AnioLectivo as g WITH(NOLOCK) on e.IdEmpresa = g.IdEmpresa and e.IdAnio = g.IdAnio left join"
                      + " fa_notaCreDeb as h WITH(NOLOCK) on b.IdEmpresa_fac_nd_doc_mod = h.IdEmpresa and b.IdSucursal_fac_nd_doc_mod = h.IdSucursal and b.IdBodega_fac_nd_doc_mod = h.IdBodega and b.IdCbteVta_fac_nd_doc_mod = h.IdNota and b.vt_tipoDoc = h.CodDocumentoTipo left join"
                      + " fa_TipoNota as j WITH(NOLOCK) on a.IdEmpresa = j.IdEmpresa and a.IdTipoNota = j.IdTipoNota LEFT JOIN"
                      + " fa_cliente as k WITH(NOLOCK) on a.IdEmpresa = k.IdEmpresa and a.IdCliente = k.IdCliente LEFT JOIN"
                      + " tb_persona as l WITH(NOLOCK) on l.IdPersona = k.IdPersona LEFT JOIN"
                      + " aca_Alumno as m WITH(NOLOCK) on a.IdEmpresa = m.IdEmpresa and a.IdAlumno = m.IdAlumno LEFT JOIN"
                      + " tb_persona as n WITH(NOLOCK) on m.IdPersona = n.IdPersona LEFT JOIN"
                      + " aca_AnioLectivo as xa with(nolock) on xa.IdEmpresa = a.IdEmpresa and xa.EnCurso = 1"
                      + " where a.CreDeb = 'C' and NOT exists("
                          + " select x.IdEmpresa from cxc_ConciliacionNotaCredito as x WITH(NOLOCK)"
                          + " where a.IdEmpresa = x.IdEmpresa and a.IdSucursal = x.IdSucursal and a.IdBodega = x.IdBodega and a.IdNota = x.IdNota"
                          + " and x.Estado = 1"
                      + " ) and a.IdEmpresa = @IdEmpresa and a.IdSucursal = @IdSucursal and a.IdBodega = @IdBodega and a.IdNota = @IdNota"
                      + " and not exists("
                          + " select x.IdEmpresa_nt from fa_notaCreDeb_x_fa_factura_NotaDeb as x WITH(NOLOCK)"
                                                + " where a.IdEmpresa = x.IdEmpresa_nt and a.IdSucursal = x.IdSucursal_nt and a.IdBodega = x.IdBodega_nt and a.IdNota = X.IdNota_nt and x.Valor_Aplicado = 0"
                      + " )"
                      + " UNION ALL"
                      + " SELECT A.IdEmpresa, A.IdSucursal, A.IdBodega, A.IdNota, 3, 'NC SIN CRUCE',c.IdCtaCbleCXC IdCtaCbleDebe, c.IdCtaCble IdCtaCbleHaber,"
                      + " '' DocumentoCruce, d.Total,  "
                      + " case when A.NaturalezaNota = 'INT' then 'NTCR ID ' + cast(A.IdNota as varchar) ELSE ISNULL(A.CodDocumentoTipo, '') +' ' + ISNULL(A.Serie1, '') + '-' + ISNULL(A.Serie2, '') + '-' + ISNULL(A.NumNota_Impresa, '') END +"
                         + " ' CLIENTE: ' + l.pe_nombreCompleto + ' ALUMNO: ' + n.pe_nombreCompleto + ' OBS: ' + isnull(a.sc_observacion, '') as Observacion"
                      + " FROM fa_notaCreDeb AS A WITH(NOLOCK) LEFT join"
                      + " fa_notaCreDeb_x_fa_factura_NotaDeb as b WITH(NOLOCK) on a.IdEmpresa = b.IdEmpresa_nt and a.IdSucursal = b.IdSucursal_nt and a.IdBodega = b.IdBodega_nt and a.IdNota = b.IdNota_nt left join"
                      + " fa_TipoNota as c WITH(NOLOCK) on a.IdEmpresa = c.IdEmpresa and a.IdTipoNota = c.IdTipoNota LEFT JOIN"
                      + " fa_notaCreDeb_resumen as d WITH(NOLOCK) on a.IdEmpresa = d.IdEmpresa and a.IdSucursal = d.IdSucursal and a.IdBodega = d.IdBodega and a.IdNota = d.IdNota left join"
                      + " fa_cliente as k WITH(NOLOCK) on a.IdEmpresa = k.IdEmpresa and a.IdCliente = k.IdCliente LEFT JOIN"
                      + " tb_persona as l WITH(NOLOCK) on l.IdPersona = k.IdPersona LEFT JOIN"
                      + " aca_Alumno as m WITH(NOLOCK) on a.IdEmpresa = m.IdEmpresa and a.IdAlumno = m.IdAlumno LEFT JOIN"
                      + " tb_persona as n WITH(NOLOCK) on m.IdPersona = n.IdPersona"
                      + " where a.CreDeb = 'C'"
                      + " and a.IdEmpresa = @IdEmpresa and a.IdSucursal = @IdSucursal and a.IdBodega = @IdBodega and a.IdNota = @IdNota and (b.IdNota_nt is null or exists("
                          + " select x.IdEmpresa_nt from fa_notaCreDeb_x_fa_factura_NotaDeb as x WITH(NOLOCK)"
                                                + " where a.IdEmpresa = x.IdEmpresa_nt and a.IdSucursal = x.IdSucursal_nt and a.IdBodega = x.IdBodega_nt and a.IdNota = X.IdNota_nt and x.Valor_Aplicado = 0"
                      + " ))"
                      + " GROUP BY A.IdEmpresa, A.IdSucursal, A.IdBodega, A.IdNota,c.IdCtaCbleCXC, c.IdCtaCble,d.Total, l.pe_nombreCompleto, n.pe_nombreCompleto, a.sc_observacion, "
                      + " A.NaturalezaNota, A.CodDocumentoTipo, A.Serie1, A.Serie2, A.NumNota_Impresa"
                      + " UNION ALL"
                      + " SELECT a.IdEmpresa, a.IdSucursal, a.IdBodega, a.IdNota, 4, 'ND', c.IdCtaCbleCXC, c.IdCtaCble, '',b.Total, "
                      + " case when A.NaturalezaNota = 'INT' then 'NTDB ID ' + cast(A.IdNota as varchar) ELSE ISNULL(A.CodDocumentoTipo, '') +' ' + ISNULL(A.Serie1, '') + '-' + ISNULL(A.Serie2, '') + '-' + ISNULL(A.NumNota_Impresa, '') END +"
                         + " ' CLIENTE: ' + l.pe_nombreCompleto + ' ALUMNO: ' + n.pe_nombreCompleto + ' OBS: ' + isnull(a.sc_observacion, '') as Observacion"
                      + " FROM fa_notaCreDeb as a WITH(NOLOCK) LEFT JOIN"
                      + " fa_notaCreDeb_resumen as b WITH(NOLOCK) on a.IdEmpresa = b.IdEmpresa and a.IdSucursal = b.IdSucursal and a.IdBodega = b.IdBodega and a.IdNota = b.IdNota LEFT JOIN"
                      + " fa_TipoNota as c WITH(NOLOCK) on a.IdEmpresa = c.IdEmpresa and a.IdTipoNota = c.IdTipoNota LEFT JOIN"
                      + " fa_cliente as k WITH(NOLOCK) on a.IdEmpresa = k.IdEmpresa and a.IdCliente = k.IdCliente LEFT join"
                      + " tb_persona as l WITH(NOLOCK) on l.IdPersona = k.IdPersona LEFT JOIN"
                      + " aca_Alumno as m WITH(NOLOCK) on a.IdEmpresa = m.IdEmpresa and a.IdAlumno = m.IdAlumno LEFT join"
                      + " tb_persona as n WITH(NOLOCK) on m.IdPersona = n.IdPersona"
                      + " where a.CreDeb = 'D' and a.IdEmpresa = @IdEmpresa and a.IdSucursal = @IdSucursal and a.IdBodega = @IdBodega and a.IdNota = @IdNota"
                      + " group by a.IdEmpresa, a.IdSucursal, a.IdBodega, a.IdNota, c.IdCtaCbleCXC, c.IdCtaCble, b.Total,l.pe_nombreCompleto, n.pe_nombreCompleto, a.sc_observacion, "
                      + " A.NaturalezaNota, A.CodDocumentoTipo, A.Serie1, A.Serie2, A.NumNota_Impresa";
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Lista.Add(new fa_notaCreDeb_ParaContabilizar
                        {
                            IdEmpresa = Convert.ToInt32(reader["IdEmpresa"]),
                            IdSucursal = Convert.ToInt32(reader["IdSucursal"]),
                            IdBodega = Convert.ToInt32(reader["IdBodega"]),
                            IdNota = Convert.ToDecimal(reader["IdNota"]),
                            QueryNumber = Convert.ToInt32(reader["QueryNumber"]),
                            Tipo = Convert.ToString(reader["Tipo"]),
                            IdCtaCbleDebe = Convert.ToString(reader["IdCtaCbleDebe"]),
                            IdCtaCbleHaber = Convert.ToString(reader["IdCtaCbleHaber"]),
                            DocumentoCruce = Convert.ToString(reader["DocumentoCruce"]),
                            Valor = Convert.ToDouble(reader["Valor"]),
                            Observacion = Convert.ToString(reader["Observacion"])
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

