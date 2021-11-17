using Core.Bus.CuentasPorCobrar;
using Core.Bus.Facturacion;
using Core.Bus.General;
using Core.Data.Academico;
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
    public class aca_Matricula_Bus
    {
        tb_ColaCorreo_Data odata_correo = new tb_ColaCorreo_Data();
        aca_Catalogo_Data odata_catalogo = new aca_Catalogo_Data();
        aca_Matricula_Data odata = new aca_Matricula_Data();
        aca_Alumno_Data odata_alumno = new aca_Alumno_Data();
        aca_Familia_Data odata_familia = new aca_Familia_Data();
        tb_persona_Data odata_persona = new tb_persona_Data();
        public List<aca_Matricula_Info> GetList(int IdEmpresa, int IdAnio, int IdSede, bool MostrarAnulados)
        {
            try
            {
                return odata.getList(IdEmpresa, IdAnio, IdSede, MostrarAnulados);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<aca_Matricula_Info> GetListAlumno(int IdEmpresa, int IdSede, decimal IdAlumno, bool MostrarAnulados)
        {
            try
            {
                return odata.getListAlumno(IdEmpresa, IdSede, IdAlumno, MostrarAnulados);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<aca_Matricula_Info> GetList_PorCurso(int IdEmpresa, int IdAnio, int IdSede, int IdNivel, int IdJornada, int IdCurso, int IdParalelo)
        {
            try
            {
                return odata.getList_PorCurso(IdEmpresa, IdAnio, IdSede, IdNivel, IdJornada, IdCurso, IdParalelo);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public aca_Matricula_Info GetInfo(int IdEmpresa, decimal IdMatricula)
        {
            try
            {
                return odata.getInfo(IdEmpresa, IdMatricula);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public aca_Matricula_Info GetInfo_X_PreMatricula(int IdEmpresa, decimal IdPreMatricula)
        {
            try
            {
                return odata.getInfo_X_PreMatricula(IdEmpresa, IdPreMatricula);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public aca_Matricula_Info GetInfo_UltimaMatricula(int IdEmpresa, decimal IdAlumno)
        {
            try
            {
                return odata.getInfo_UltimaMatricula(IdEmpresa, IdAlumno);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public aca_Matricula_Info GetInfo_ExisteMatricula(int IdEmpresa, int IdAnio, decimal IdAlumno)
        {
            try
            {
                return odata.getInfo_ExisteMatricula(IdEmpresa, IdAnio, IdAlumno);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool GuardarPreMatriculaDB(aca_Matricula_Info info_matricula)
        {
            aca_MecanismoDePago_Bus bus_mecanismo = new aca_MecanismoDePago_Bus();
            aca_Plantilla_Rubro_Bus bus_plantilla_rubro = new aca_Plantilla_Rubro_Bus();
            fa_PuntoVta_Bus bus_punto_venta = new fa_PuntoVta_Bus();
            fa_Vendedor_Bus bus_vendedor = new fa_Vendedor_Bus();
            fa_TerminoPago_Bus bus_termino_pago = new fa_TerminoPago_Bus();
            fa_formaPago_Bus bus_forma_pago = new fa_formaPago_Bus();
            fa_catalogo_Bus bus_catalogo = new fa_catalogo_Bus();
            fa_cliente_Bus bus_cliente = new fa_cliente_Bus();
            fa_factura_Bus bus_factura = new fa_factura_Bus();
            aca_AnioLectivo_Periodo_Bus bus_anio_periodo = new aca_AnioLectivo_Periodo_Bus();
            aca_Familia_Bus bus_familia = new aca_Familia_Bus();
            aca_AnioLectivo_Rubro_Bus bus_anio_rubro = new aca_AnioLectivo_Rubro_Bus();
            aca_AnioLectivo_Rubro_Periodo_Bus bus_anio_rubro_periodo = new aca_AnioLectivo_Rubro_Periodo_Bus();
            cxc_cobro_Bus bus_cobro = new cxc_cobro_Bus();
            tb_mes_Bus bus_mes = new tb_mes_Bus();
            tb_sis_Impuesto_Bus bus_impuesto = new tb_sis_Impuesto_Bus();
            aca_Admision_Bus bus_admision = new aca_Admision_Bus();
            aca_PreMatricula_Bus bus_prematricula = new aca_PreMatricula_Bus();
            try
            {
                if (odata.guardarDB(info_matricula))
                {
                    var lst_rubros_x_cobrar = info_matricula.lst_MatriculaRubro.Where(q => q.EnMatricula == true).ToList();
                    var mecanismo = bus_mecanismo.GetInfo(info_matricula.IdEmpresa, info_matricula.IdMecanismo);
                    var termino_pago = bus_termino_pago.get_info(mecanismo.IdTerminoPago);
                    var punto_venta = bus_punto_venta.get_info(info_matricula.IdEmpresa, info_matricula.IdSucursal, info_matricula.IdPuntoVta);
                    var RepEconomico = bus_familia.GetInfo_Representante(info_matricula.IdEmpresa, info_matricula.IdAlumno, cl_enumeradores.eTipoRepresentante.ECON.ToString());
                    var Cliente = bus_cliente.get_info_x_num_cedula(info_matricula.IdEmpresa, RepEconomico.pe_cedulaRuc);

                    #region Factura
                    foreach (var item in lst_rubros_x_cobrar)
                    {
                        var AnioLectivo_Periodo = bus_anio_periodo.GetInfo(info_matricula.IdEmpresa, info_matricula.IdAnio, item.IdPeriodo);
                        var AnioLectivo_Rubro = bus_anio_rubro.GetInfo(info_matricula.IdEmpresa, info_matricula.IdAnio, item.IdRubro);
                        var AnioLectivo_Rubro_Periodo = bus_anio_rubro_periodo.GetInfo(info_matricula.IdEmpresa, info_matricula.IdAnio, item.IdRubro, item.IdPeriodo);
                        var mes = bus_mes.get_list().Where(q => q.idMes == AnioLectivo_Periodo.IdMes).FirstOrDefault();
                        var ObsFactura = AnioLectivo_Rubro.NomRubro + " " + (AnioLectivo_Rubro.NumeroCuotas > 1 ? (AnioLectivo_Rubro_Periodo.Secuencia + "/" + AnioLectivo_Rubro.NumeroCuotas) : "") + " " + mes.smes + " " + AnioLectivo_Periodo.FechaHasta.Year;
                        var NumPension = (AnioLectivo_Rubro.NumeroCuotas > 1 ? (AnioLectivo_Rubro_Periodo.Secuencia + "/" + AnioLectivo_Rubro.NumeroCuotas) : "");

                        var info_factura = new fa_factura_Info
                        {
                            IdEmpresa = info_matricula.IdEmpresa,
                            IdSucursal = info_matricula.IdSucursal,
                            IdBodega = punto_venta.IdBodega,
                            vt_tipoDoc = "FACT",
                            //vt_serie1 = vt_serie1,
                            //vt_serie2 = vt_serie2,
                            //vt_NumFactura = vt_NumFactura,
                            IdAlumno = info_matricula.IdAlumno,
                            IdCliente = Cliente.IdCliente,
                            IdVendedor = 1,
                            IdNivel = 1,
                            IdCatalogo_FormaPago = info_matricula.IdCatalogo_FormaPago,
                            vt_fecha = DateTime.Now,
                            vt_plazo = termino_pago.Dias_Vct,
                            vt_fech_venc = DateTime.Now.AddDays(termino_pago.Dias_Vct),
                            vt_tipo_venta = mecanismo.IdTerminoPago,
                            vt_Observacion = ObsFactura,
                            Estado = "A",
                            IdCaja = punto_venta.IdCaja,
                            IdEmpresa_rol = info_matricula.IdEmpresa_rol,
                            IdEmpleado = info_matricula.IdEmpleado,
                            IdUsuario = info_matricula.IdUsuarioCreacion,
                            IdPuntoVta = info_matricula.IdPuntoVta,
                            aprobada_enviar_sri = false
                        };

                        info_factura.lst_det = new List<fa_factura_det_Info>();
                        var info_impuesto = bus_impuesto.get_info(item.IdCod_Impuesto_Iva);
                        var fact_det = new fa_factura_det_Info
                        {
                            Secuencia = 1,
                            IdProducto = item.IdProducto,
                            vt_cantidad = 1,
                            vt_Precio = Convert.ToDouble(item.Total),
                            vt_PorDescUnitario = 0,
                            vt_DescUnitario = 0,
                            vt_PrecioFinal = Convert.ToDouble(item.Total),
                            vt_Subtotal = Convert.ToDouble(item.Total),
                            vt_por_iva = info_impuesto.porcentaje,
                            IdCod_Impuesto_Iva = item.IdCod_Impuesto_Iva,
                            vt_iva = Convert.ToDouble(item.ValorIVA),
                            vt_total = Convert.ToDouble(item.Total),
                            vt_detallexItems = NumPension,
                            IdMatricula = info_matricula.IdMatricula,
                            aca_IdPeriodo = item.IdPeriodo,
                            aca_IdAnio = item.IdAnio,
                            aca_IdPlantilla = item.IdPlantilla,
                            aca_IdRubro = item.IdRubro,
                            AplicaProntoPago = item.AplicaProntoPago,
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
                        info_factura.info_resumen.IdAnio = info_matricula.IdAnio;
                        info_factura.info_resumen.IdPlantilla = info_matricula.IdPlantilla;
                        info_factura.info_resumen.IdRubro = item.IdRubro;
                        #endregion

                        if (!bus_factura.guardarDB(info_factura))
                        {
                            return false;
                            //mensaje = "No se ha podido guardar la factura";
                        }
                    }
                    #endregion

                    #region Familia
                    var lst_familia = bus_familia.GetList(info_matricula.IdEmpresa, Convert.ToInt32(info_matricula.IdAlumno));
                    if (lst_familia.Count > 0)
                    {
                        foreach (var item in lst_familia)
                        {
                            var info_familia_factura = bus_familia.GetInfo(item.IdEmpresa, Convert.ToInt32(item.IdAlumno), item.Secuencia);
                            if (info_familia_factura.IdPersona == info_matricula.IdPersonaF)
                            {
                                info_familia_factura.SeFactura = true; 
                            }
                            else
                            {
                                info_familia_factura.SeFactura = false;
                            }

                            if (info_familia_factura.IdPersona == info_matricula.IdPersonaR)
                            {
                                info_familia_factura.EsRepresentante = true;
                            }
                            else
                            {
                                info_familia_factura.EsRepresentante = false;
                            }
                            bus_familia.modificarCheck(info_familia_factura);
                        }

                    }
                    #endregion

                    var info_admision = bus_admision.GetInfo(info_matricula.IdEmpresa, info_matricula.IdAdmision);
                    if (info_admision!=null)
                    {
                        info_admision.IdUsuarioModificacion = info_matricula.IdUsuarioCreacion;
                        info_admision.IdCatalogoESTADM = Convert.ToInt32(cl_enumeradores.eTipoCatalogoAdmision.MATRICULADO);
                        bus_admision.ModificarEstado(info_admision);

                        var info_catalogo = odata_catalogo.getInfo(Convert.ToInt32(info_admision.IdCatalogoESTADM));
                        var info_alumno = odata_alumno.getInfo(info_matricula.IdEmpresa, info_matricula.IdAlumno);
                        var info_familia_papa = odata_familia.getListTipo(info_matricula.IdEmpresa, info_matricula.IdAlumno, Convert.ToInt32(cl_enumeradores.eTipoParentezco.PAPA));
                        var info_familia_mama = odata_familia.getListTipo(info_matricula.IdEmpresa, info_matricula.IdAlumno, Convert.ToInt32(cl_enumeradores.eTipoParentezco.MAMA));
                        var info_familia_rep_eco = odata_familia.getInfo_Representante(info_matricula.IdEmpresa, info_matricula.IdAlumno, cl_enumeradores.eTipoRepresentante.ECON.ToString());
                        var info_familia_rep_legal = odata_familia.getInfo_Representante(info_matricula.IdEmpresa, info_matricula.IdAlumno, cl_enumeradores.eTipoRepresentante.LEGAL.ToString());
                        var Destinatarios = (info_familia_papa == null ? "" : info_familia_papa.Correo) + ";" + (info_familia_mama == null ? "" : info_familia_mama.Correo) + ";" + (info_familia_rep_eco==null?"": info_familia_rep_eco.Correo)+";"+(info_familia_rep_legal==null ? "" : info_familia_rep_legal.Correo);
                        var info_correo = new tb_ColaCorreo_Info
                        {
                            IdEmpresa = info_matricula.IdEmpresa,
                            Destinatarios = Destinatarios,
                            Asunto = "ASPIRANTE MATRICULADO",
                            Parametros = "",
                            Codigo = "",
                            IdUsuarioCreacion = info_matricula.IdUsuarioCreacion,
                            Cuerpo = (info_catalogo == null ? "" : info_catalogo.NomCatalogo),
                            FechaCreacion = DateTime.Now
                        };
                        odata_correo.GuardarDB(info_correo);
                    }           

                    var info_prematricula = bus_prematricula.GetInfo_PorIdAdmision(info_matricula.IdEmpresa, info_matricula.IdAdmision);
                    if (info_prematricula != null)
                    {
                        info_prematricula.IdCatalogoESTPREMAT = Convert.ToInt32(cl_enumeradores.eCatalogoAcademicoMatricula.MATRICULADO);
                        bus_prematricula.ModificarEstado(info_prematricula);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public bool GuardarDB(aca_Matricula_Info info_matricula)
        {
            try
            {
                return odata.guardarDB(info_matricula);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public bool ModificarDB(aca_Matricula_Info info)
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

        public bool ModificarPlantillaDB(aca_Matricula_Info info)
        {
            try
            {
                return odata.modificarPlantillaDB(info);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool ModificarEstadoMatricula(aca_Matricula_Info info)
        {
            try
            {
                return odata.modificarEstadoMatriculaDB(info);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool ModificarCursoParaleloDB(aca_Matricula_Info info)
        {
            try
            {
                return odata.modificarCursoParaleloDB(info);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool EliminarDB(aca_Matricula_Info info)
        {
            try
            {
                return odata.eliminarDB(info);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<aca_Matricula_Info> GetList_Calificaciones(int IdEmpresa, int IdAnio, int IdSede, int IdNivel, int IdJornada, int IdCurso, int IdParalelo, decimal IdAlumno)
        {
            try
            {
                return odata.getList_Calificaciones(IdEmpresa, IdAnio, IdSede, IdNivel, IdJornada, IdCurso, IdParalelo, IdAlumno);
            }
            catch (Exception)
            {
                throw;
            }
        }


        //DASHBOARD
        public List<aca_Matricula_Info> Dashboard_EstudiantesGeneral(int IdEmpresa, int IdAnio, int IdSede)
        {
            try
            {
                return odata.Dashboard_EstudiantesGeneral(IdEmpresa, IdAnio, IdSede);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<aca_Matricula_Info> CantEstudiantesActual(int IdEmpresa, int IdAnio, int IdSede)
        {
            try
            {
                return odata.CantEstudiantesActual(IdEmpresa, IdAnio, IdSede);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<aca_Matricula_Info> CantEstudiantesJornada(int IdEmpresa, int IdAnio, int IdSede)
        {
            try
            {
                return odata.CantEstudiantesJornada(IdEmpresa, IdAnio, IdSede);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<aca_Matricula_Info> CantEstudiantesNivel(int IdEmpresa, int IdAnio, int IdSede)
        {
            try
            {
                return odata.CantEstudiantesNivel(IdEmpresa, IdAnio, IdSede);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
