﻿using Core.Bus.Academico;
using Core.Bus.Banco;
using Core.Bus.Caja;
using Core.Bus.Contabilidad;
using Core.Bus.CuentasPorCobrar;
using Core.Bus.Facturacion;
using Core.Bus.General;
using Core.Bus.SeguridadAcceso;
using Core.Data.Academico;
using Core.Info.Academico;
using Core.Info.CuentasPorCobrar;
using Core.Info.Facturacion;
using Core.Info.General;
using Core.Info.Helps;
using Core.Web.Helps;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Web.Areas.CuentasPorCobrar.Controllers
{
    public class CobranzaController : Controller
    {
        #region Variables
        cxc_ConciliacionNotaCredito_Bus bus_conciliacion = new cxc_ConciliacionNotaCredito_Bus();
        tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
        cxc_cobro_Bus bus_cobro = new cxc_cobro_Bus();
        caj_Caja_Bus bus_caja = new caj_Caja_Bus();
        caj_parametro_Bus bus_param_caja = new caj_parametro_Bus();
        cxc_cobro_tipo_Bus bus_cobro_tipo = new cxc_cobro_tipo_Bus();
        cxc_Parametro_Bus bus_param_cxc = new cxc_Parametro_Bus();
        tb_persona_Bus bus_persona = new tb_persona_Bus();
        tb_banco_Bus bus_banco = new tb_banco_Bus();
        ba_Banco_Cuenta_Bus bus_banco_cuenta = new ba_Banco_Cuenta_Bus();
        cxc_cobro_det_Bus bus_det = new cxc_cobro_det_Bus();
        cxc_cobro_det_List list_det = new cxc_cobro_det_List();
        ct_periodo_Bus bus_periodo = new ct_periodo_Bus();
        cxc_cobro_det_x_cruzar_List List_x_Cruzar = new cxc_cobro_det_x_cruzar_List();
        List<cxc_cobro_det_Info> ListaDetalleXCruzar = new List<cxc_cobro_det_Info>();
        seg_usuario_Bus bus_usuario = new seg_usuario_Bus();
        fa_cliente_Bus bus_cliente = new fa_cliente_Bus();
        aca_Familia_Bus bus_familia = new aca_Familia_Bus();
        aca_AnioLectivo_Rubro_Bus bus_aca_anio_rubro = new aca_AnioLectivo_Rubro_Bus();
        aca_AnioLectivo_Periodo_Bus bus_anio_periodo = new aca_AnioLectivo_Periodo_Bus();
        tb_TarjetaCredito_Bus bus_tarjeta = new tb_TarjetaCredito_Bus();
        fa_TipoNota_Bus bus_tipo_nota = new fa_TipoNota_Bus();
        aca_AnioLectivo_Bus bus_anioLectivo = new aca_AnioLectivo_Bus();
        aca_Matricula_Bus bus_matricula = new aca_Matricula_Bus();
        fa_notaCreDeb_Bus bus_notaDebCre = new fa_notaCreDeb_Bus();
        tb_ColaCorreoCodigo_Bus busCorreoCodigo = new tb_ColaCorreoCodigo_Bus();
        tb_ColaCorreo_Bus busCorreo = new tb_ColaCorreo_Bus();
        cxc_LiquidacionTarjeta_Bus busLiquidacionTarjeta = new cxc_LiquidacionTarjeta_Bus();
        aca_PlantillaTipo_Bus bus_tipo_plantilla = new aca_PlantillaTipo_Bus();
        aca_Plantilla_Bus bus_plantilla = new aca_Plantilla_Bus();
        aca_AnioLectivo_Bus bus_anio = new aca_AnioLectivo_Bus();
        fa_cliente_contactos_Bus bus_cliente_contacto = new fa_cliente_contactos_Bus();
        cxc_Parametro_Bus bus_parametros = new cxc_Parametro_Bus();
        string mensaje = string.Empty;
        string mensajeInfo = string.Empty;
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        aca_Menu_x_seg_usuario_Bus bus_permisos = new aca_Menu_x_seg_usuario_Bus();
        cxc_cobro_List Lista_Cobro = new cxc_cobro_List();
        #endregion

        #region Metodos ComboBox bajo demanda
        public ActionResult CmbCliente_Cobranza()
        {
            decimal model = new decimal();
            return PartialView("_CmbCliente_Cobranza", model);
        }
        public List<tb_persona_Info> get_list_bajo_demanda(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            return bus_persona.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), cl_enumeradores.eTipoPersona.CLIENTE.ToString());
        }
        public tb_persona_Info get_info_bajo_demanda(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_persona.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), cl_enumeradores.eTipoPersona.CLIENTE.ToString());
        }
        #endregion

        #region Combo bajo demanda Alumno
        public ActionResult Cmb_Alumno_Cobranza()
        {
            decimal model = new decimal();
            return PartialView("_CmbAlumno_Cobranza", model);
        }
        public List<tb_persona_Info> get_list_bajo_demanda_alumno(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            return bus_persona.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), cl_enumeradores.eTipoPersona.ALUMNO.ToString());
        }
        public tb_persona_Info get_info_bajo_demanda_alumno(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_persona.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), cl_enumeradores.eTipoPersona.ALUMNO.ToString());
        }
        #endregion

        #region Index
        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            cl_filtros_Info model = new cl_filtros_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                fecha_ini= DateTime.Now.Date.AddMonths(-1),
                fecha_fin=DateTime.Now
            };
            cargar_combos_consulta();

            #region Permisos
            aca_Menu_x_seg_usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(SessionFixed.IdSede), SessionFixed.IdUsuario, "CuentasPorCobrar", "Cobranza", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            List<cxc_cobro_Info> lista = bus_cobro.get_list_matricula(model.IdEmpresa, model.IdSucursal, model.fecha_ini, model.fecha_fin);
            Lista_Cobro.set_list(lista, Convert.ToDecimal(SessionFixed.IdTransaccionSession));

            return View(model);
        }
        [HttpPost]
        public ActionResult Index(cl_filtros_Info model)
        {
            cargar_combos_consulta();
            SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();

            #region Permisos
            aca_Menu_x_seg_usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(SessionFixed.IdSede), SessionFixed.IdUsuario, "CuentasPorCobrar", "Cobranza", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            List<cxc_cobro_Info> lista = bus_cobro.get_list_matricula(model.IdEmpresa, model.IdSucursal, model.fecha_ini, model.fecha_fin);
            Lista_Cobro.set_list(lista, Convert.ToDecimal(SessionFixed.IdTransaccionSession));

            return View(model);
        }

        #endregion

        #region Metodos
        private void cargar_combos_consulta()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var lst_sucursal = bus_sucursal.GetList(IdEmpresa, Convert.ToString(SessionFixed.IdUsuario), false);
            lst_sucursal.Add(new tb_sucursal_Info
            {
                IdEmpresa = IdEmpresa,
                IdSucursal = 0,
                Su_Descripcion = "Todos"
            });
            ViewBag.lst_sucursal = lst_sucursal;
        }
        private void cargar_combos(int IdEmpresa, int IdSucursal)
        {
            var lst_sucursal = bus_sucursal.GetList(IdEmpresa, Convert.ToString(SessionFixed.IdUsuario), false);
            ViewBag.lst_sucursal = lst_sucursal;

            bool EsContador = Convert.ToBoolean(SessionFixed.EsContador);
            var lst_caja = bus_caja.GetList(IdEmpresa, IdSucursal, false, SessionFixed.IdUsuario, EsContador);
            ViewBag.lst_caja = lst_caja;

            var lst_cobro_tipo = bus_cobro_tipo.get_list(false);
            lst_cobro_tipo = lst_cobro_tipo.Where(q => q.IdMotivo_tipo_cobro != "RET" && !q.IdCobro_tipo.StartsWith("CRU") && !q.IdCobro_tipo.StartsWith("NT") && !q.IdCobro_tipo.StartsWith("NC") && !q.IdCobro_tipo.StartsWith("TRAN_CLI") && !q.IdCobro_tipo.StartsWith("CHQF")).ToList();
            ViewBag.lst_cobro_tipo = lst_cobro_tipo;

            var lst_banco = bus_banco.get_list(false);
            ViewBag.lst_banco = lst_banco;

            var lst_banco_cuenta = bus_banco_cuenta.get_list(IdEmpresa, IdSucursal, false);
            ViewBag.lst_banco_cuenta = lst_banco_cuenta;

            var lst_tarjeta = bus_tarjeta.GetList(IdEmpresa, false);
            ViewBag.lst_tarjeta = lst_tarjeta;

            var lst_tipo_nota = bus_tipo_nota.get_list(IdEmpresa,"C", false);
            ViewBag.lst_tipo_nota = lst_tipo_nota;
        }

        private bool validar(cxc_cobro_Info i_validar, ref string msg)
        {
            var familia = bus_familia.GetInfo_Representante(i_validar.IdEmpresa, i_validar.IdAlumno ?? 0, "ECON");
            if (familia == null)
            {
                msg = "El alumno no tiene familiar destinado ser un cliente";
                return false;
            }

            var Cliente = bus_cliente.get_info_x_num_cedula(i_validar.IdEmpresa, familia.pe_cedulaRuc);
            if (Cliente == null || Cliente.IdCliente==0)
            {
                msg = "El familiar destinado a ser un cliente no existe en el módulo de clientes";
                return false;
            }
            i_validar.IdCliente = Cliente.IdCliente;
            i_validar.IdEntidad = i_validar.IdCliente;
            if (i_validar.cr_TotalCobro == 0)
            {
                msg = "No ha ingresado el total a cobrar";
                return false;
            }
            if (Math.Round(i_validar.cr_saldo, 2, MidpointRounding.AwayFromZero) < 0)
            {
                msg = "El valor aplicado a los documentos es mayor al total a cobrar";
                return false;
            }

            i_validar.lst_det = list_det.get_list(i_validar.IdTransaccionSession);
            if (i_validar.lst_det.Count == 0 && i_validar.cr_saldo == 0)
            {
                msg = "No ha seleccionado documentos para realizar la cobranza";
                return false;
            }

            if (i_validar.lst_det.Where(q => q.dc_ValorPago == 0).Count() > 0)
            {
                msg = "Existen documentos con valor aplicado 0";
                return false;
            }
            if (i_validar.IdCobro > 0 && i_validar.lst_det.Where(q => Math.Round(q.dc_ValorPago, 2, MidpointRounding.AwayFromZero) > Math.Round((double)q.Saldo, 2, MidpointRounding.AwayFromZero)).Count() > 0)
            {
                msg = "Existen documentos cuyo valor aplicado es mayor al saldo de la factura";
                return false;
            }
            string observacion = "Canc./ ";
            foreach (var item in i_validar.lst_det)
            {
                observacion += item.vt_NumDocumento + "/";
            }
            if (i_validar.lst_det.Count == 0)
            {
                observacion = "Cobro sin documentos";
            }
            i_validar.cr_observacion = observacion;
            i_validar.cr_fechaCobro = i_validar.cr_fecha;
            i_validar.cr_fechaDocu = i_validar.cr_fecha;

            if (!string.IsNullOrEmpty(i_validar.IdCobro_tipo))
                i_validar.lst_det.ForEach(q => q.IdCobro_tipo_det = i_validar.IdCobro_tipo);


            if (i_validar.lst_det.Count == 0 && i_validar.cr_saldo > 0)
            {
                if (i_validar.IdTipoNotaCredito == null)
                {
                    msg = "Debe ingresar el tipo de nota de crédito a aplicar para el saldo";
                    return false;
                }

                if (bus_det.get_list_cartera(i_validar.IdEmpresa,i_validar.IdSucursal,i_validar.IdAlumno ?? 0,false).Count > 0)
                {
                    msg = "No puede realizar un cobro sin documentos cuando el estudiante tiene documentos por cobrar";
                    return false;
                }
            }

            if (i_validar.lst_det.Count > 0 && i_validar.cr_saldo > 0)
            {
                msg = "Tiene un excedente de "+ i_validar.cr_saldo.ToString("c2")+", por favor cree un nuevo cobro para el excedente";
                return false;
            }

            switch (i_validar.IdCobro_tipo)
            {
                case "DEPO":
                    if (i_validar.IdBanco == null)
                    {
                        msg = "El campo cuenta bancaria es obligatorio para depositos";
                        return false;
                    }
                    i_validar.cr_Banco = null;
                    i_validar.IdTarjeta = null;
                    i_validar.cr_Tarjeta = null;                    
                    break;
                case "TARJ":
                    if (i_validar.IdTarjeta == null || string.IsNullOrEmpty(i_validar.cr_Tarjeta))
                    {
                        msg = "El campo tarjeta de crédito es obligatorio";
                        return false;
                    }
                    i_validar.cr_Banco = null;
                    i_validar.IdBanco = null;
                    break;
                case "CHQF":
                    if (string.IsNullOrEmpty(i_validar.cr_Banco))
                    {
                        msg = "El campo banco es obligatorio para cheques";
                        return false;
                    }
                    if (string.IsNullOrEmpty(i_validar.cr_cuenta))
                    {
                        msg = "El campo cuenta es obligatorio para cheques";
                        return false;
                    }
                    if (string.IsNullOrEmpty(i_validar.cr_NumDocumento))
                    {
                        msg = "El campo # cheque es obligatorio para cheques";
                        return false;
                    }
                    i_validar.IdBanco = null;
                    //i_validar.cr_Banco = null;
                    i_validar.IdTarjeta = null;
                    i_validar.cr_Tarjeta = null;
                    break;

                case "CHQV":
                    if (string.IsNullOrEmpty(i_validar.cr_Banco))
                    {
                        msg = "El campo banco es obligatorio para cheques";
                        return false;
                    }
                    if (string.IsNullOrEmpty(i_validar.cr_cuenta))
                    {
                        msg = "El campo cuenta es obligatorio para cheques";
                        return false;
                    }
                    if (string.IsNullOrEmpty(i_validar.cr_NumDocumento))
                    {
                        msg = "El campo # cheque es obligatorio para cheques";
                        return false;
                    }
                    i_validar.IdBanco = null;
                    //i_validar.cr_Banco = null;
                    i_validar.IdTarjeta = null;
                    i_validar.cr_Tarjeta = null;
                    break;
                default:
                    i_validar.IdBanco = null;
                    i_validar.cr_Banco = null;
                    i_validar.IdTarjeta = null;
                    i_validar.cr_Tarjeta = null;
                    break;
            }

            foreach (var item in i_validar.lst_det)
            {
                if (i_validar.cr_fecha < item.vt_fecha)
                {
                    msg = "Existen comprobantes de venta con fecha mayor a la fecha del cobro aplicado";
                    return false;
                }
            }

            if (i_validar.lst_det.Count > 0)
            {
                if (i_validar.cr_saldo != 0)
                {
                    msg = "No puede existir un excedente cuando hay documentos aplicados, debe realizar un cobro sin documentos para el pago anticipado";
                    return false;
                }
            }
            if (i_validar.lst_det.Count > 0)
            {
                //Obtener la mayor fecha de los documentos seleccionados
                DateTime FechaMayor = i_validar.lst_det.Max(q => q.FechaProntoPago ?? DateTime.Now.Date.AddYears(-10));
                //De la lista de TODO lo pendiente de pagar obtengo la menor fecha
                var lst = List_x_Cruzar.get_list(i_validar.IdTransaccionSession);

                foreach (var item in i_validar.lst_det)
                {
                    var obj = lst.Where(q => q.secuencia == item.secuencia).FirstOrDefault();
                    if (obj != null)
                    {
                        lst.Remove(obj);
                    }
                }
                if (lst.Count > 0)
                {
                    DateTime FechaMenor = lst.Min(q => q.FechaProntoPago ?? DateTime.Now.Date.AddYears(-10));
                    if (FechaMayor > FechaMenor)
                    {
                        msg = "No puede realizar el cobro ya que existen facturas no seleccionadas con fecha menor";
                        return false;
                    }
                }
            }

            if (!bus_periodo.ValidarFechaTransaccion(i_validar.IdEmpresa, i_validar.cr_fecha, cl_enumeradores.eModulo.CXC, i_validar.IdSucursal, ref msg))
            {
                return false;
            }

            if (i_validar.IdCobro>0)
            {
                var LstDet = bus_det.get_list(i_validar.IdEmpresa, i_validar.IdSucursal, i_validar.IdCobro);
                if ((LstDet.Count == 0 && i_validar.lst_det.Count != 0) || (LstDet.Count != 0 && i_validar.lst_det.Count == 0))
                {
                    msg = "Si el cobro tenia relacionados documentos, no se puede modificar para convertirlo en un cobro anticipado, debe anularlo";
                    return false;
                }

                if (!bus_conciliacion.ValidarEnConciliacionNC(i_validar.IdEmpresa, i_validar.IdSucursal, 0, i_validar.IdCobro, "COBRO"))
                {
                    ViewBag.mensaje = "El cobro se ha creado a partir de una conciliación de NC y no puede ser modificado";
                    return false;
                }

                if (!busLiquidacionTarjeta.ValidarExisteLiquidacionPorTarjeta(i_validar.IdEmpresa, i_validar.IdSucursal, i_validar.IdCobro))
                {
                    ViewBag.mensaje = "El cobro se encuentra en una liquidación de tarjeta de crédito y no puede ser modificado";
                    return false;
                }
                
            }
            
            return true;
        }
        #endregion

        #region Acciones
        public ActionResult Nuevo(int IdEmpresa = 0)
        {
            int IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal);
            var param_caja = bus_param_caja.get_info(IdEmpresa);
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            #region Permisos
            aca_Menu_x_seg_usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(SessionFixed.IdSede), SessionFixed.IdUsuario, "CuentasPorCobrar", "Cobranza", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            var paramCxc = bus_param_cxc.get_info(IdEmpresa);
            if (paramCxc == null)
                return RedirectToAction("Index");

            cxc_cobro_Info model = new cxc_cobro_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdEmpresa = IdEmpresa,
                IdSucursal = IdSucursal,
                cr_fecha = DateTime.Now.Date,
                IdCobro_tipo = "EFEC",
                lst_det = new List<cxc_cobro_det_Info>(),
                IdTipoNotaCredito = paramCxc.IdTipoNotaPagoAnticipado,
                fecha_ini = DateTime.Now.Date.AddMonths(-1),
                fecha_fin = DateTime.Now
            };
            list_det.set_list(new List<cxc_cobro_det_Info>(), model.IdTransaccionSession);
            List_x_Cruzar.set_list(new List<cxc_cobro_det_Info>(), model.IdTransaccionSession);

            cargar_combos(IdEmpresa, model.IdSucursal);
            
            return View(model);
        }
        [HttpPost]
        public ActionResult Nuevo(cxc_cobro_Info model)
        {
            model.lst_det = list_det.get_list(model.IdTransaccionSession);
            if (!validar(model, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                cargar_combos(model.IdEmpresa, model.IdSucursal);
                return View(model);
            }
            model.IdUsuario = SessionFixed.IdUsuario;

            if (!bus_cobro.guardarDB(model))
            {
                ViewBag.mensaje = mensaje;
                cargar_combos(model.IdEmpresa, model.IdSucursal);
                return View(model);
            }

            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdSucursal = model.IdSucursal, IdCobro = model.IdCobro, Exito = true });
        }
        public ActionResult Consultar(int IdEmpresa = 0, int IdSucursal = 0, decimal IdCobro = 0, bool Exito = false)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            ViewBag.MostrarBoton = true;
            cxc_cobro_Info model = bus_cobro.get_info(IdEmpresa, IdSucursal, IdCobro);
            if (model == null)
                return RedirectToAction("Index");
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession);
            model.lst_det = bus_det.get_list(IdEmpresa, IdSucursal, IdCobro);

            #region Permisos
            aca_Menu_x_seg_usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(SessionFixed.IdSede), SessionFixed.IdUsuario, "CuentasPorCobrar", "Cobranza", "Index");
            if (model.cr_estado=="A" && model.IdCobro_tipo!= "NTCR")
            {

            }
            else
            {
                info.Modificar = false;
                info.Anular = false;
            }

            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            if (ViewBag.MostrarBoton == true && model.lst_det.Where(q => q.dc_ValorProntoPago > 0).Count() > 0)
            {
                ViewBag.mensaje = "El cobro no se puede modificar ya que aplica pronto pago, debe anular y realizar uno nuevo";
                ViewBag.MostrarBoton = false;
            }

            list_det.set_list(model.lst_det, model.IdTransaccionSession);
            model.IdEntidad = model.IdCliente;
            cargar_combos(IdEmpresa, model.IdSucursal);

            var Anio = bus_anioLectivo.GetInfo_AnioEnCurso(IdEmpresa, 0);
            if (Anio != null)
            {
                var Matricula = bus_matricula.GetInfo_ExisteMatricula(model.IdEmpresa, Anio.IdAnio, model.IdAlumno ?? 0);
                if (Matricula != null)
                {
                    var info_plantilla = bus_plantilla.GetInfo(IdEmpresa, Matricula.IdAnio, Matricula.IdPlantilla);
                    var info_tipo_plantilla = bus_tipo_plantilla.getInfo(IdEmpresa, Convert.ToInt32(info_plantilla == null ? 0 : info_plantilla.IdTipoPlantilla));

                    model.DatosAlumno = Matricula.NomNivel + " " + Matricula.NomJornada + " " + Matricula.NomCurso + " " + Matricula.NomParalelo + " / " + (info_tipo_plantilla == null ? "" : info_tipo_plantilla.NomPlantillaTipo) + " - " + (info_plantilla == null ? "" : info_plantilla.NomPlantilla);
                }
                else
                    model.DatosAlumno = "NO MATRICULADO";
            }


            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            #region Validacion Periodo
            if (ViewBag.MostrarBoton == true && !bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.cr_fecha, cl_enumeradores.eModulo.CXC, model.IdSucursal, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }
            #endregion

            #region Validacion Movimiento de caja
            if (ViewBag.MostrarBoton == true && !bus_cobro.ValidarMostrarBotonModificar(IdEmpresa, model.IdSucursal, model.IdCobro))
            {
                ViewBag.mensaje = "El cobro no se puede modificar porque el movimiento de caja asociado ya fue depositado o fue registrado al cerrar una caja chica";
                ViewBag.MostrarBoton = false;
            }
            #endregion

            return View(model);
        }
        public ActionResult Modificar(int IdEmpresa = 0, int IdSucursal = 0, decimal IdCobro = 0, bool Exito = false)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            ViewBag.MostrarBoton = true;
            cxc_cobro_Info model = bus_cobro.get_info(IdEmpresa, IdSucursal, IdCobro);
            if (model == null)
                return RedirectToAction("Index");
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession);
            model.lst_det = bus_det.get_list(IdEmpresa, IdSucursal, IdCobro);
            
            #region Permisos
            aca_Menu_x_seg_usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(SessionFixed.IdSede), SessionFixed.IdUsuario, "CuentasPorCobrar", "Cobranza", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            if (ViewBag.MostrarBoton == true && model.lst_det.Where(q => q.dc_ValorProntoPago > 0).Count() > 0)
            {
                ViewBag.mensaje = "El cobro no se puede modificar ya que aplica pronto pago, debe anular y realizar uno nuevo";
                ViewBag.MostrarBoton = false;
            }
            
            list_det.set_list(model.lst_det, model.IdTransaccionSession);
            model.IdEntidad = model.IdCliente;
            cargar_combos(IdEmpresa, model.IdSucursal);

            var Anio = bus_anioLectivo.GetInfo_AnioEnCurso(IdEmpresa, 0);
            if (Anio != null)
            {
                var Matricula = bus_matricula.GetInfo_ExisteMatricula(model.IdEmpresa, Anio.IdAnio, model.IdAlumno ?? 0);
                if (Matricula != null)
                {
                    var info_plantilla = bus_plantilla.GetInfo(IdEmpresa, Matricula.IdAnio, Matricula.IdPlantilla);
                    var info_tipo_plantilla = bus_tipo_plantilla.getInfo(IdEmpresa, Convert.ToInt32(info_plantilla == null ? 0 : info_plantilla.IdTipoPlantilla));

                    model.DatosAlumno = Matricula.NomNivel + " " + Matricula.NomJornada + " " + Matricula.NomCurso + " " + Matricula.NomParalelo + " / " + (info_tipo_plantilla == null ? "" : info_tipo_plantilla.NomPlantillaTipo) + " - " + (info_plantilla == null ? "" : info_plantilla.NomPlantilla);
                }
                else
                    model.DatosAlumno = "NO MATRICULADO";
            }
            

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;
            
            #region Validacion Periodo
            if (ViewBag.MostrarBoton == true && !bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.cr_fecha, cl_enumeradores.eModulo.CXC, model.IdSucursal, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }
            #endregion

            #region Validacion Movimiento de caja
            if (ViewBag.MostrarBoton == true && !bus_cobro.ValidarMostrarBotonModificar(IdEmpresa, model.IdSucursal,model.IdCobro))
            {
                ViewBag.mensaje = "El cobro no se puede modificar porque el movimiento de caja asociado ya fue depositado o fue registrado al cerrar una caja chica";
                ViewBag.MostrarBoton = false;
            }
            #endregion
            
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(cxc_cobro_Info model)
        {
            model.lst_det = list_det.get_list(model.IdTransaccionSession);
            if (!validar(model, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                cargar_combos(model.IdEmpresa, model.IdSucursal);
                ViewBag.MostrarBoton = true;
                return View(model);
            }
            model.IdUsuarioUltMod = SessionFixed.IdUsuario;
            if (!bus_cobro.modificarDB(model))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = true;
                cargar_combos(model.IdEmpresa, model.IdSucursal);
                return View(model);
            }

            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdSucursal = model.IdSucursal, IdCobro = model.IdCobro, Exito = true });
        }

        public ActionResult Anular(int IdEmpresa = 0, int IdSucursal = 0, decimal IdCobro = 0)
        {
            ViewBag.MostrarBoton = true;

            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            cxc_cobro_Info model = bus_cobro.get_info(IdEmpresa, IdSucursal, IdCobro);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            aca_Menu_x_seg_usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(SessionFixed.IdSede), SessionFixed.IdUsuario, "CuentasPorCobrar", "Cobranza", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession);
            model.lst_det = bus_det.get_list(IdEmpresa, IdSucursal, IdCobro);
            list_det.set_list(model.lst_det, model.IdTransaccionSession);
            model.IdEntidad = model.IdCliente;
            cargar_combos(IdEmpresa, model.IdSucursal);

            var Anio = bus_anioLectivo.GetInfo_AnioEnCurso(IdEmpresa, 0);
            if (Anio != null)
            {
                var Matricula = bus_matricula.GetInfo_ExisteMatricula(model.IdEmpresa, Anio.IdAnio, model.IdAlumno ?? 0);
                if (Matricula != null)
                {
                    var info_plantilla = bus_plantilla.GetInfo(IdEmpresa, Matricula.IdAnio, Matricula.IdPlantilla);
                    var info_tipo_plantilla = bus_tipo_plantilla.getInfo(IdEmpresa, Convert.ToInt32(info_plantilla == null ? 0 : info_plantilla.IdTipoPlantilla));

                    model.DatosAlumno = Matricula.NomNivel + " " + Matricula.NomJornada + " " + Matricula.NomCurso + " " + Matricula.NomParalelo + " / " + (info_tipo_plantilla == null ? "" : info_tipo_plantilla.NomPlantillaTipo) + " - " + (info_plantilla == null ? "" : info_plantilla.NomPlantilla);
                }
                else
                    model.DatosAlumno = "NO MATRICULADO";
            }

            #region Validacion Periodo
            ViewBag.MostrarBoton = true;
            if (ViewBag.MostrarBoton == true && !bus_periodo.ValidarFechaTransaccion(IdEmpresa, model.cr_fecha, cl_enumeradores.eModulo.CXC, model.IdSucursal, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                ViewBag.MostrarBoton = false;
            }
            #endregion

            #region Validacion Movimiento de caja
            if (ViewBag.MostrarBoton == true && !bus_cobro.ValidarMostrarBotonModificar(IdEmpresa, model.IdSucursal, model.IdCobro))
            {
                ViewBag.mensaje = "El cobro no se puede modificar porque el movimiento de caja asociado ya fue depositado o fue registrado al cerrar una caja chica";
                ViewBag.MostrarBoton = false;
            }
            #endregion


            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(cxc_cobro_Info model)
        {
            if (!bus_conciliacion.ValidarEnConciliacionNC(model.IdEmpresa, model.IdSucursal, 0, model.IdCobro, "COBRO"))
            {
                ViewBag.mensaje = "El cobro se ha creado a partir de una conciliación de NC y no puede ser anulado";
                cargar_combos(model.IdEmpresa, model.IdSucursal);
                return View(model);
            }

            if (!busLiquidacionTarjeta.ValidarExisteLiquidacionPorTarjeta(model.IdEmpresa, model.IdSucursal, model.IdCobro))
            {
                ViewBag.mensaje = "El cobro se encuentra en una liquidación de tarjeta de crédito y no puede ser anulado";
                cargar_combos(model.IdEmpresa, model.IdSucursal);
                return View(model);
            }
            model.IdUsuarioUltAnu = SessionFixed.IdUsuario;
            if (!bus_cobro.anularDB(model))
            {
                ViewBag.mensaje = mensaje;
                cargar_combos(model.IdEmpresa, model.IdSucursal);
                return View(model);
            }

            return RedirectToAction("Index");
        }

        #endregion

        #region Grids
        [ValidateInput(false)]
        public ActionResult GridViewPartial_cobranza(DateTime? Fecha_ini, DateTime? Fecha_fin, int IdSucursal = 0, bool Nuevo = false, bool Modificar = false, bool Anular = false)
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;

            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            ViewBag.Fecha_ini = Fecha_ini == null ? DateTime.Now.Date.AddMonths(-1) : Convert.ToDateTime(Fecha_ini);
            ViewBag.Fecha_fin = Fecha_fin == null ? DateTime.Now.Date : Convert.ToDateTime(Fecha_fin);
            ViewBag.IdSucursal = IdSucursal;

            ViewBag.Nuevo = Nuevo;
            ViewBag.Modificar = Modificar;
            ViewBag.Anular = Anular;

            //var model = bus_cobro.get_list_matricula(IdEmpresa, IdSucursal, ViewBag.Fecha_ini, ViewBag.Fecha_fin);
            List<cxc_cobro_Info> model = Lista_Cobro.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_cobranza", model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_cobranza_x_alumno()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            List<cxc_cobro_Info> model = Lista_Cobro.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return PartialView("_GridViewPartial_cobranza_x_alumno", model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_cobranza_det()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = list_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_cobranza_det", model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_cobranza_facturas_x_cruzar()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = List_x_Cruzar.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return PartialView("_GridViewPartial_cobranza_facturas_x_cruzar", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditingUpdateFactura([ModelBinder(typeof(DevExpressEditorsBinder))] cxc_cobro_det_Info info_det)
        {
            if (ModelState.IsValid)
                list_det.UpdateRow(info_det, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            var model = list_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_cobranza_det", model);
        }

        public ActionResult EditingDeleteFactura(string secuencia)
        {
            list_det.DeleteRow(secuencia, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            var model = list_det.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_cobranza_det", model);
        }
        #endregion

        #region Json
        public JsonResult GetListFacturas_PorIngresar(decimal IdTransaccionSession = 0, int IdEmpresa = 0, int IdSucursal = 0, decimal IdAlumno = 0)
        {
            var lst = bus_det.get_list_cartera(IdEmpresa, IdSucursal, IdAlumno, false);

            List_x_Cruzar.set_list(lst, IdTransaccionSession);

            return Json(lst.Count, JsonRequestBehavior.AllowGet);
        }
        [HttpPost, ValidateInput(false)]
        public JsonResult EditingAddNewFactura(string IDs = "", double TotalACobrar = 0, decimal IdTransaccionSession = 0)
        {
            double saldo = TotalACobrar;
            double ValorProntoPago = 0;
            if (IDs != "")
            {
                int IdEmpresaSesion = Convert.ToInt32(SessionFixed.IdEmpresa);
                var lst_x_ingresar = List_x_Cruzar.get_list(IdTransaccionSession);
                string[] array = IDs.Split(',');

                foreach (var item in array)
                {
                    var info_det = lst_x_ingresar.Where(q => q.secuencia == item).FirstOrDefault();
                    if (info_det != null)
                        list_det.AddRow(info_det, IdTransaccionSession);
                }
            }
            var lst = list_det.get_list(IdTransaccionSession);
            var lstFinal = new List<cxc_cobro_det_Info>();
            foreach (var item in lst)
            {
                ValorProntoPago = Math.Round((item.vt_total - item.ValorProntoPago ?? 0),2,MidpointRounding.AwayFromZero);
                if (saldo > 0)
                {
                    item.dc_ValorProntoPago = Math.Round(saldo,2,MidpointRounding.AwayFromZero) >= Math.Round((Convert.ToDouble(item.Saldo) - ValorProntoPago),2,MidpointRounding.AwayFromZero) ? Math.Round(ValorProntoPago,2,MidpointRounding.AwayFromZero) : 0;
                    item.dc_ValorPago = Math.Round(saldo,2,MidpointRounding.AwayFromZero) >= Math.Round((Convert.ToDouble(item.Saldo) - ValorProntoPago),2,MidpointRounding.AwayFromZero) ? Math.Round(Convert.ToDouble(item.Saldo) - ValorProntoPago,2,MidpointRounding.AwayFromZero) : Math.Round(saldo,2,MidpointRounding.AwayFromZero);
                    item.Saldo_final  = Math.Round(Convert.ToDouble(item.Saldo - ValorProntoPago) - item.dc_ValorPago,2,MidpointRounding.AwayFromZero);
                    item.ValorProntoPago = ValorProntoPago;
                    saldo =  Math.Round(saldo - item.dc_ValorPago,2,MidpointRounding.AwayFromZero);
                    lstFinal.Add(item);
                }
                else
                    break;
            }
            list_det.set_list(lstFinal, IdTransaccionSession);

            var resultado = saldo;
            return Json(Math.Round(resultado, 2, MidpointRounding.AwayFromZero), JsonRequestBehavior.AllowGet);
        }

        public JsonResult CalcularSaldo(double TotalACobrar = 0, decimal IdTransaccionSession = 0)
        {
            double saldo = TotalACobrar;

            var lst = list_det.get_list(IdTransaccionSession);
            foreach (var item in lst)
            {
                saldo -= item.dc_ValorPago;
            }
            list_det.set_list(lst, IdTransaccionSession);
            var resultado = saldo;
            return Json(Math.Round(resultado, 2, MidpointRounding.AwayFromZero), JsonRequestBehavior.AllowGet);
        }

        public void VaciarLista(decimal IdTransaccionSession = 0)
        {
            list_det.set_list(new List<cxc_cobro_det_Info>(), IdTransaccionSession);
        }

        public JsonResult GetIdCajaPorSucursal(int IdEmpresa, int IdSucursal)
        {
            var resultado = bus_caja.GetIdCajaPorSucursal(IdEmpresa, IdSucursal);

            return Json(resultado, JsonRequestBehavior.AllowGet);

        }

        public JsonResult SetCliente(int IdEmpresa = 0, decimal IdAlumno = 0)
        {
            decimal IdCliente = 0;
            var info_familia = bus_familia.GetInfo_Representante(IdEmpresa, IdAlumno, cl_enumeradores.eTipoRepresentante.ECON.ToString());
            var info_cliente = bus_cliente.get_info_x_num_cedula(IdEmpresa, (info_familia == null ? "" : info_familia.pe_cedulaRuc));
            IdCliente = (info_cliente == null ? 0 : info_cliente.IdCliente);

            return Json(IdCliente, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SetAlumno(int IdEmpresa = 0, decimal IdAlumno = 0)
        {
            string DatosAlumno = string.Empty;
            double Saldo = Math.Round(bus_cobro.GetSaldoAlumno(IdEmpresa, IdAlumno, true),2,MidpointRounding.AwayFromZero);
            var AnioLectivo = bus_anioLectivo.GetInfo_AnioEnCurso(IdEmpresa, 0);
            if (AnioLectivo != null)
            {
                var Matricula = bus_matricula.GetInfo_ExisteMatricula(IdEmpresa, AnioLectivo.IdAnio, IdAlumno);
                if (Matricula != null)
                {
                    var info_plantilla = bus_plantilla.GetInfo(IdEmpresa, Matricula.IdAnio, Matricula.IdPlantilla);
                    var info_tipo_plantilla = bus_tipo_plantilla.getInfo(IdEmpresa, Convert.ToInt32(info_plantilla==null ? 0 : info_plantilla.IdTipoPlantilla));
                    DatosAlumno = Matricula.NomNivel + " " + Matricula.NomJornada + " " + Matricula.NomCurso + " " + Matricula.NomParalelo +" / "+ (info_tipo_plantilla==null ? "" : info_tipo_plantilla.NomPlantillaTipo) +" - "+ (info_plantilla == null ? "" : info_plantilla.NomPlantilla);

                }
                else
                {
                    DatosAlumno = "NO MATRICULADO";
                }                 
            }

            List<fa_notaCreDeb_Info> lst_CreditoAlumno = bus_notaDebCre.get_list_credito_favor(IdEmpresa, IdAlumno);

            if (lst_CreditoAlumno.Sum(q => q.sc_saldo) > 0)
            {
                var SaldoCredito = Math.Round(lst_CreditoAlumno.Sum(q => Convert.ToDouble(q.sc_saldo)), 2, MidpointRounding.AwayFromZero).ToString("C2");
                mensajeInfo += "El estudiante tiene un saldo a favor: " + SaldoCredito + ".</br>";
            }

            return Json(new { Saldo = Saldo, DatosAlumno = DatosAlumno, mensajeInfo = mensajeInfo },JsonRequestBehavior.AllowGet);
        }

        public JsonResult EnviarCorreo(int IdEmpresa, int IdSede, decimal IdAlumno, string Correos,string CodigoCorreo)
        {
            string Mensaje = string.Empty;
            var Codigo = busCorreoCodigo.GetInfo(IdEmpresa, CodigoCorreo);
            if (Codigo != null)
            {
                busCorreo.GuardarDB(new tb_ColaCorreo_Info
                {
                    IdEmpresa = IdEmpresa,
                    Codigo = CodigoCorreo,
                    Destinatarios = Correos,
                    Asunto = Codigo.Asunto,
                    Cuerpo = Codigo.Cuerpo,
                    Parametros = IdEmpresa.ToString() + ";" + IdSede.ToString() + ";" + IdAlumno.ToString(),
                    IdUsuarioCreacion = SessionFixed.IdUsuario
                });
            }

            return Json(Mensaje, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EnviarCorreoCobro(int IdEmpresa, int IdSucursal, decimal IdCobro, decimal IdAlumno, string Correos, string CodigoCorreo)
        {
            string Mensaje = string.Empty;
            var Codigo = busCorreoCodigo.GetInfo(IdEmpresa, CodigoCorreo);
            if (Codigo != null)
            {
                busCorreo.GuardarDB(new tb_ColaCorreo_Info
                {
                    IdEmpresa = IdEmpresa,
                    Codigo = CodigoCorreo,
                    Destinatarios = Correos,
                    Asunto = Codigo.Asunto,
                    Cuerpo = Codigo.Cuerpo,
                    Parametros = IdEmpresa.ToString() + ";" + IdSucursal.ToString() + ";"+ IdCobro.ToString() + ";" + IdAlumno.ToString(),
                    IdUsuarioCreacion = SessionFixed.IdUsuario
                });
            }

            return Json(Mensaje, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DatosCorreo(int IdEmpresa, decimal IdAlumno)
        {
            string Mensaje = string.Empty;
            var info_anio = bus_anio.GetInfo_AnioEnCurso(IdEmpresa, 0);
            var info_matricula = bus_matricula.GetInfo_UltimaMatricula(IdEmpresa, IdAlumno);
            var info_persona_factura = bus_persona.get_info(IdEmpresa, cl_enumeradores.eTipoPersona.PERSONA.ToString(), (info_matricula == null ? 0 : info_matricula.IdPersonaF));
            var info_cliente = bus_cliente.get_info_x_num_cedula(IdEmpresa, (info_persona_factura == null ? "" : info_persona_factura.pe_cedulaRuc));
            var info_cliente_contacto = bus_cliente_contacto.get_info(IdEmpresa, info_cliente.IdCliente, 1);
            Mensaje = (info_cliente_contacto == null ? "" : info_cliente_contacto.Correo);

            return Json(Mensaje, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SetConsultaAlumno(DateTime fecha_ini, DateTime fecha_fin, int IdEmpresa = 0, int IdSucursal = 0, decimal IdAlumno = 0, decimal IdTransaccionSession = 0)
        {
            List<cxc_cobro_Info> lista = bus_cobro.get_list_matricula_alumno(IdEmpresa, IdSucursal, IdAlumno, fecha_ini, fecha_fin);
            Lista_Cobro.set_list(lista, Convert.ToDecimal(SessionFixed.IdTransaccionSession));

            return Json(lista, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }

    public class cxc_cobro_det_List
    {
        string Variable = "cxc_cobro_det_Info";
        public List<cxc_cobro_det_Info> get_list(decimal IdTransaccionSession)
        {

            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<cxc_cobro_det_Info> list = new List<cxc_cobro_det_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<cxc_cobro_det_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<cxc_cobro_det_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }

        public void AddRow(cxc_cobro_det_Info info_det, decimal IdTransaccionSession)
        {
            List<cxc_cobro_det_Info> list = get_list(IdTransaccionSession);
            if (list.Where(q => q.secuencia == info_det.secuencia).FirstOrDefault() == null)
            {
                info_det.Saldo_final = Convert.ToDouble(info_det.Saldo) - info_det.dc_ValorPago;
                list.Add(info_det);
            }
        }

        public void UpdateRow(cxc_cobro_det_Info info_det, decimal IdTransaccionSession)
        {
            cxc_cobro_det_Info edited_info = get_list(IdTransaccionSession).Where(m => m.secuencia == info_det.secuencia).First();
            edited_info.Saldo_final = Convert.ToDouble(edited_info.Saldo) - info_det.dc_ValorPago;
            edited_info.dc_ValorPago = info_det.dc_ValorPago;
        }

        public void DeleteRow(string secuencia, decimal IdTransaccionSession)
        {
            List<cxc_cobro_det_Info> list = get_list(IdTransaccionSession);
            list.Remove(list.Where(m => m.secuencia == secuencia).FirstOrDefault());
        }

        public cxc_cobro_det_Info GetRow(string secuencia, decimal IdTransaccionSession)
        {
            List<cxc_cobro_det_Info> list = get_list(IdTransaccionSession);
            return list.Where(m => m.secuencia == secuencia).FirstOrDefault();
        }
    }

    public class cxc_cobro_det_x_cruzar_List
    {
        string Variable = "cxc_cobro_det_x_cruzar_Info";
        public List<cxc_cobro_det_Info> get_list(decimal IdTransaccionSession)
        {

            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<cxc_cobro_det_Info> list = new List<cxc_cobro_det_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<cxc_cobro_det_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<cxc_cobro_det_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }

    public class cxc_cobro_List
    {
        string Variable = "cxc_cobro_Info";
        public List<cxc_cobro_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<cxc_cobro_Info> list = new List<cxc_cobro_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<cxc_cobro_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<cxc_cobro_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}