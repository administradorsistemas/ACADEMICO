using Core.Bus.Academico;
using Core.Bus.CuentasPorCobrar;
using Core.Bus.Facturacion;
using Core.Bus.General;
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
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Core.Web.Areas.Academico.Controllers
{
    public class MatriculaController : Controller
    {
        #region Variables
        aca_Matricula_Bus bus_matricula = new aca_Matricula_Bus();
        aca_Matricula_List Lista_Matricula = new aca_Matricula_List();
        aca_AnioLectivo_Bus bus_anio = new aca_AnioLectivo_Bus();
        tb_persona_Bus bus_persona = new tb_persona_Bus();
        aca_Familia_Bus bus_familia = new aca_Familia_Bus();
        aca_Matricula_PorCurso_List Lista_Matricula_PorCurso = new aca_Matricula_PorCurso_List();
        aca_MecanismoDePago_Bus bus_mecanismo = new aca_MecanismoDePago_Bus();
        aca_Plantilla_Rubro_Bus bus_plantilla_rubro = new aca_Plantilla_Rubro_Bus();
        //aca_Plantilla_Rubro_List Lista_DetallePlantilla = new aca_Plantilla_Rubro_List();
        aca_PermisoMatricula_Bus bus_permiso = new aca_PermisoMatricula_Bus();
        aca_Matricula_Rubro_List ListaMatriculaRubro = new aca_Matricula_Rubro_List();
        aca_Matricula_Rubro_Bus bus_matricula_rubro = new aca_Matricula_Rubro_Bus();
        aca_AnioLectivo_Jornada_Curso_Bus bus_jornada_curso = new aca_AnioLectivo_Jornada_Curso_Bus();
        aca_Plantilla_Bus bus_plantilla = new aca_Plantilla_Bus();
        aca_AlumnoDocumento_Bus bus_alumno_documento = new aca_AlumnoDocumento_Bus();
        aca_AnioLectivo_Curso_Documento_List Lista_DocumentosMatricula = new aca_AnioLectivo_Curso_Documento_List();
        aca_AnioLectivo_Jornada_Curso_Bus bus_aniolectivo_jornada_curso = new aca_AnioLectivo_Jornada_Curso_Bus();
        aca_AnioLectivo_Curso_Documento_Bus bus_curso_documento = new aca_AnioLectivo_Curso_Documento_Bus();
        aca_SocioEconomico_Bus bus_socioeconomico = new aca_SocioEconomico_Bus();
        aca_AnioLectivo_Paralelo_Profesor_Bus bus_materias_x_paralelo = new aca_AnioLectivo_Paralelo_Profesor_Bus();
        aca_AnioLectivo_Periodo_Bus bus_anio_periodo = new aca_AnioLectivo_Periodo_Bus();
        fa_PuntoVta_Bus bus_punto_venta = new fa_PuntoVta_Bus();
        fa_Vendedor_Bus bus_vendedor = new fa_Vendedor_Bus();
        fa_TerminoPago_Bus bus_termino_pago = new fa_TerminoPago_Bus();
        fa_formaPago_Bus bus_forma_pago = new fa_formaPago_Bus();
        fa_catalogo_Bus bus_catalogo = new fa_catalogo_Bus();
        fa_cliente_Bus bus_cliente = new fa_cliente_Bus();
        fa_factura_Bus bus_factura = new fa_factura_Bus();
        tb_sis_Documento_Tipo_Talonario_Bus bus_talonario = new tb_sis_Documento_Tipo_Talonario_Bus();
        tb_sis_Impuesto_Bus bus_impuesto = new tb_sis_Impuesto_Bus();
        aca_AnioLectivo_Rubro_Bus bus_anio_rubro = new aca_AnioLectivo_Rubro_Bus();
        aca_AnioLectivo_Rubro_Periodo_Bus bus_anio_rubro_periodo = new aca_AnioLectivo_Rubro_Periodo_Bus();
        cxc_cobro_Bus bus_cobro = new cxc_cobro_Bus();
        tb_mes_Bus bus_mes = new tb_mes_Bus();
        aca_MatriculaCondicional_Bus bus_matricula_condicional = new aca_MatriculaCondicional_Bus();
        fa_notaCreDeb_Bus bus_notaDebCre = new fa_notaCreDeb_Bus();
        aca_Menu_x_seg_usuario_Bus bus_permisos = new aca_Menu_x_seg_usuario_Bus();
        tb_empresa_Bus bus_empresa = new tb_empresa_Bus();
        aca_AnioLectivoParcial_Bus bus_parcial = new aca_AnioLectivoParcial_Bus();
        aca_MatriculaCambios_Bus bus_cambios = new aca_MatriculaCambios_Bus();
        aca_Alumno_Bus bus_alumno = new aca_Alumno_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        string mensaje = string.Empty;
        string mensajeInfo = string.Empty;
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
            var info_anio = bus_anio.GetInfo_AnioEnCurso(Convert.ToInt32(SessionFixed.IdEmpresa), 0);

            aca_Matricula_Info model = new aca_Matricula_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdAnio = info_anio==null ? 0: info_anio.IdAnio,
                IdSede = Convert.ToInt32(SessionFixed.IdSede),
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession)
            };

            List<aca_Matricula_Info> lista = bus_matricula.GetList(model.IdEmpresa, model.IdAnio, model.IdSede, true);
            Lista_Matricula.set_list(lista, Convert.ToDecimal(SessionFixed.IdTransaccionSession));

            #region Permisos
            aca_Menu_x_seg_usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(SessionFixed.IdSede), SessionFixed.IdUsuario, "Academico", "Matricula", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(aca_Matricula_Info model)
        {
            SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
            List<aca_Matricula_Info> lista = bus_matricula.GetList(model.IdEmpresa, model.IdAnio, model.IdSede, true);
            Lista_Matricula.set_list(lista, Convert.ToDecimal(model.IdTransaccionSession));
            
            #region Permisos
            aca_Menu_x_seg_usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(SessionFixed.IdSede), SessionFixed.IdUsuario, "Academico", "Matricula", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_Matricula(bool Nuevo = false, bool Modificar = false, bool Anular = false)
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;

            ViewBag.Nuevo = Nuevo;
            ViewBag.Modificar = Modificar;
            ViewBag.Anular = Anular;

            List<aca_Matricula_Info> model = Lista_Matricula.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_Matricula", model);
        }
        #endregion

        #region Combos bajo demanada
        public ActionResult Cmb_MatriculaAlumno()
        {
            decimal model = new decimal();
            return PartialView("_CmbAlumno", model);
        }
        public List<tb_persona_Info> get_list_bajo_demanda_alumno(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            return bus_persona.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), cl_enumeradores.eTipoPersona.ALUMNO.ToString());
        }
        public tb_persona_Info get_info_bajo_demanda_alumno(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_persona.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), cl_enumeradores.eTipoPersona.ALUMNO.ToString());
        }

        public ActionResult ComboBoxPartial_Anio()
        {
            return PartialView("_ComboBoxPartial_Anio", new aca_AnioLectivo_NivelAcademico_Jornada_Info());
        }
        public ActionResult ComboBoxPartial_Sede()
        {
            int IdAnio = (Request.Params["IdAnio"] != null) ? int.Parse(Request.Params["IdAnio"]) : -1;
            return PartialView("_ComboBoxPartial_Sede", new aca_AnioLectivo_NivelAcademico_Jornada_Info { IdAnio = IdAnio });
        }

        public ActionResult ComboBoxPartial_Curso()
        {
            int IdAnio = (Request.Params["IdAnio"] != null) ? int.Parse(Request.Params["IdAnio"]) : -1;
            decimal IdAlumno = (Request.Params["IdAlumno"] != null) ? decimal.Parse(Request.Params["IdAlumno"]) : -1;
            string Validar = (Request.Params["Validar"] != null) ? Convert.ToString(Request.Params["Validar"]) : "N";
            return PartialView("_ComboBoxPartial_Curso", new aca_AnioLectivo_Jornada_Curso_Info { IdAnio = IdAnio, IdAlumno = IdAlumno, Validar = Validar });
        }

        public ActionResult ComboBoxPartial_Plantilla()
        {
            int IdAnio = (Request.Params["IdAnio"] != null) ? int.Parse(Request.Params["IdAnio"]) : -1;
            string IdComboCurso = (Request.Params["IdCurso"] != null) ? (Request.Params["IdCurso"]).ToString() : null;
            int IdSede = -1;
            int IdNivel = -1;
            int IdJornada = -1;
            int IdCurso = -1;

            if (!string.IsNullOrEmpty(IdComboCurso))
            {
                var regex = new Regex(@".{4}");
                string result = regex.Replace(IdComboCurso, "$&" + Environment.NewLine);
                string[] array = result.Split('\n');
                if (array.Count() >= 5)
                {
                    IdAnio = Convert.ToInt32(array[1]);
                    IdSede = Convert.ToInt32(array[2]);
                    IdNivel = Convert.ToInt32(array[3]);
                    IdJornada = Convert.ToInt32(array[4]);
                    IdCurso = Convert.ToInt32(array[5]);
                }
            }
            return PartialView("_ComboBoxPartial_Plantilla", new aca_AnioLectivo_Curso_Plantilla_Info { IdAnio = IdAnio, IdSede = IdSede, IdNivel = IdNivel, IdJornada = IdJornada, IdCurso = IdCurso });
        }

        public ActionResult ComboBoxPartial_Paralelo()
        {
            string IdComboCurso = (Request.Params["IdCurso"] != null) ? (Request.Params["IdCurso"]).ToString() : null;
            int IdAnio = -1;
            int IdSede = -1;
            int IdNivel = -1;
            int IdJornada = -1;
            int IdCurso = -1;

            if (!string.IsNullOrEmpty(IdComboCurso))
            {
                var regex = new Regex(@".{4}");
                string result = regex.Replace(IdComboCurso, "$&" + Environment.NewLine);
                string[] array = result.Split('\n');
                if (array.Count() >= 5)
                {
                    IdAnio = Convert.ToInt32(array[1]);
                    IdSede = Convert.ToInt32(array[2]);
                    IdNivel = Convert.ToInt32(array[3]);
                    IdJornada = Convert.ToInt32(array[4]);
                    IdCurso = Convert.ToInt32(array[5]);
                }
            }
            
            return PartialView("_ComboBoxPartial_Paralelo", new aca_AnioLectivo_Curso_Paralelo_Info { IdAnio = IdAnio, IdSede = IdSede, IdNivel = IdNivel, IdJornada = IdJornada, IdCurso = IdCurso });
        }
        #endregion

        #region Combos bajo demanada Empleado
        public ActionResult Cmb_MatriculaEmpleado()
        {
            int IdEmpresa_rol = (Request.Params["IdEmpresa_rol"] != null) ? int.Parse(Request.Params["IdEmpresa_rol"]) : 0;
            return PartialView("_CmbEmpleado", new aca_Matricula_Info { IdEmpresa_rol = IdEmpresa_rol });
        }
        #endregion

        #region Metodos
        private void cargar_combos()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            int IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal);
            bool EsContador = Convert.ToBoolean(SessionFixed.EsContador);

            var lst_mecanismo = bus_mecanismo.GetList(IdEmpresa, false);
            ViewBag.lst_mecanismo = lst_mecanismo;

            var lst_ptoventa = bus_punto_venta.GetListUsuario(IdEmpresa, IdSucursal, false, SessionFixed.IdUsuario, EsContador, "FACT");
            ViewBag.lst_ptoventa = lst_ptoventa;

            var lst_vendedor = bus_vendedor.get_list(IdEmpresa, false);
            ViewBag.lst_vendedor = lst_vendedor;

            var lst_pago = bus_termino_pago.get_list(false);
            ViewBag.lst_pago = lst_pago;

            var lst_formapago = bus_catalogo.get_list((int)cl_enumeradores.eTipoCatalogoFact.FormaDePago, false);
            ViewBag.lst_formapago = lst_formapago;

            var lst_empresa = bus_empresa.get_list(false);
            ViewBag.lst_empresa = lst_empresa;
        }
        #endregion

        #region GridDetalle alumnos paralelo
        [ValidateInput(false)]
        public ActionResult GridViewPartial_AlumnosPorParalelo()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;

            List<aca_Matricula_Info> model = Lista_Matricula_PorCurso.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_AlumnosPorParalelo", model);
        }
        #endregion

        #region DetallePlantilla (nuevo)
        [ValidateInput(false)]
        public ActionResult GridViewPartial_DetallePlantilla()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;

            List<aca_Matricula_Rubro_Info> model = ListaMatriculaRubro.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            int x = 0;
            string selectedIDs = "";
            ViewData["selectedIDs"] = Request.Params["selectedIDs"];
            if (ViewData["selectedIDs"] == null)
            {   
                foreach (var item in model.Where(q => q.seleccionado == true).ToList())
                {
                    if (x == 0)
                        selectedIDs = item.IdString;
                    else
                        selectedIDs += "," + item.IdString;
                    x++;
                }
                ViewData["selectedIDs"] = selectedIDs;
            }else
            {
                foreach (var item in model.Where(q => q.seleccionado == true).ToList())
                {
                    if (x == 0)
                        selectedIDs = item.IdString;
                    else
                        selectedIDs += "," + item.IdString;
                    x++;
                }
                ViewData["selectedIDs"] = selectedIDs;
            }

            return PartialView("_GridViewPartial_DetallePlantilla", model);
        }
        #endregion

        #region Funciones del detalle (modificar)
        private void cargar_combos_detalle()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var lst_mecanismo = bus_mecanismo.GetList(IdEmpresa, false);
            ViewBag.lst_mecanismo = lst_mecanismo;
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_MatriculaRubro()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            List<aca_Matricula_Rubro_Info> model = ListaMatriculaRubro.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            cargar_combos_detalle();
            return PartialView("_GridViewPartial_MatriculaRubro", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditingUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] aca_Matricula_Rubro_Info info_det)
        {
            int IdEmpresa = Convert.ToInt32(Session["IdEmpresa"]);
            if (ModelState.IsValid)
                ListaMatriculaRubro.UpdateRow(info_det, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            var model = ListaMatriculaRubro.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            cargar_combos_detalle();
            return PartialView("_GridViewPartial_MatriculaRubro", model);
        }
        #endregion

        #region Grid documentos
        [ValidateInput(false)]
        public ActionResult GridViewPartial_AlumnoDocumentos()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;

            List<aca_AnioLectivo_Curso_Documento_Info> model = Lista_DocumentosMatricula.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            ViewData["documentoIDs"] = Request.Params["documentoIDs"];
            if (ViewData["documentoIDs"] == null)
            {
                int x = 0;
                string documentoIDs = "";
                foreach (var item in model.Where(q => q.seleccionado == true).ToList())
                {
                    if (x == 0)
                        documentoIDs = Convert.ToString(item.IdStringDoc);
                    else
                        documentoIDs += "," + item.IdStringDoc;
                    x++;
                }
                ViewData["documentoIDs"] = documentoIDs;
            }

            return PartialView("_GridViewPartial_AlumnoDocumentos", model);
        }
        #endregion

        #region Json
        public JsonResult SetMatricula_PorCurso(int IdEmpresa=0, int IdAnio = 0, string IdComboCurso = "", int IdParalelo = 0)
        {
            int IdSede = 0;
            int IdNivel = 0;
            int IdJornada = 0;
            int IdCurso = 0;

            //public JsonResult SetMatricula_PorCurso(int IdEmpresa=0, int IdAnio = 0, int IdSede = 0, int IdNivel = 0, int IdJornada = 0, int IdCurso = 0, int IdParalelo = 0, decimal IdTransaccionSession = 0)
            //IdEmpresa = Convert.ToInt32(model.IdComboCurso.Substring(0, 4));
            //IdAnio = Convert.ToInt32(model.IdComboCurso.Substring(4, 4));
            IdSede = Convert.ToInt32(IdComboCurso.Substring(8, 4));
            IdNivel = Convert.ToInt32(IdComboCurso.Substring(12, 4));
            IdJornada = Convert.ToInt32(IdComboCurso.Substring(16, 4));
            IdCurso = Convert.ToInt32(IdComboCurso.Substring(20, 4));

            var lista_PorCurso = bus_matricula.GetList_PorCurso(IdEmpresa, IdAnio, IdSede, IdNivel, IdJornada, IdCurso, IdParalelo);
            Lista_Matricula_PorCurso.set_list(lista_PorCurso, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return Json(lista_PorCurso, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LimpiarListaDetalle(int IdEmpresa = 0)
        {
            decimal Total = 0;
            decimal TotalProntoPago = 0;
            decimal ValorTotal = 0;
            decimal ValorTotalPP = 0;

            ValorTotal = Total;
            ValorTotalPP = TotalProntoPago;
            ListaMatriculaRubro.set_list(new List<aca_Matricula_Rubro_Info>(), Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return Json(new { Valor = ValorTotal, ProntoPago = ValorTotalPP }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SetMatriculaRubro(int IdEmpresa = 0, int IdAnio = 0, int IdPlantilla = 0, int IdMatricula=0)
        {
            decimal Total = 0;
            decimal TotalProntoPago = 0;
            decimal ValorDescuento = 0;
            decimal ValorRubro = 0;
            decimal ValorTotal = 0;
            decimal ValorTotalPP = 0;
            List<aca_Matricula_Rubro_Info> lst_MatriculaRubro = new List<aca_Matricula_Rubro_Info>();
            if (IdMatricula == 0)
            {
                var info_plantilla = bus_plantilla.GetInfo(IdEmpresa, IdAnio, IdPlantilla);
                lst_MatriculaRubro = bus_matricula_rubro.GetList_Matricula(IdEmpresa, IdAnio, IdPlantilla);
                if (lst_MatriculaRubro.Count()>0)
                {
                    var IdPrimerPeriodo = lst_MatriculaRubro.Min(q => q.IdPeriodo);

                    foreach (var item in lst_MatriculaRubro)
                    {
                        var info_anio_periodo = bus_anio_periodo.GetInfo(IdEmpresa, IdAnio, item.IdPeriodo);

                        if (item.IdPeriodo == IdPrimerPeriodo)
                        {
                            item.seleccionado = true;
                        }

                        if (item.AplicaProntoPago == true)
                        {
                            if (DateTime.Now.Date <= item.FechaProntoPago)
                            {
                                /*
                                if (info_plantilla.TipoDescuento == "%")
                                {
                                    ValorDescuento = (item.Total * (info_plantilla.Valor / 100));
                                    ValorRubro = item.Total - ValorDescuento;
                                    TotalProntoPago = TotalProntoPago + Math.Round(ValorRubro, 2, MidpointRounding.AwayFromZero);
                                }
                                else
                                {
                                    ValorRubro = (item.Total - info_plantilla.Valor);
                                    TotalProntoPago = TotalProntoPago + Math.Round(ValorRubro, 2, MidpointRounding.AwayFromZero);
                                }*/
                                ValorRubro = Math.Round(item.ValorProntoPago, 2, MidpointRounding.AwayFromZero);
                                TotalProntoPago = TotalProntoPago + Math.Round(ValorRubro, 2, MidpointRounding.AwayFromZero);
                            }
                            else
                            {
                                ValorRubro = (item.Total);
                                TotalProntoPago = TotalProntoPago + Math.Round(ValorRubro, 2, MidpointRounding.AwayFromZero);
                            }
                            if (item.seleccionado)
                            {
                                Total = Total + Math.Round((item.ValorProntoPago), 2, MidpointRounding.AwayFromZero);
                            }
                            
                        }
                        else
                        {
                            ValorRubro = (item.Total);
                            Total = Total + Math.Round((item.Total), 2, MidpointRounding.AwayFromZero);
                            TotalProntoPago = TotalProntoPago + Math.Round((item.Total), 2, MidpointRounding.AwayFromZero);
                        }

                        item.ValorProntoPago = ValorRubro;
                        item.FechaProntoPago = Convert.ToDateTime(info_anio_periodo.FechaProntoPago);
                    }
                    ValorTotal = lst_MatriculaRubro.Where(q => q.seleccionado == true).Sum(q => q.Total);
                    ValorTotalPP = lst_MatriculaRubro.Where(q => q.seleccionado == true).Sum(q => q.ValorProntoPago);
                    ListaMatriculaRubro.set_list(lst_MatriculaRubro, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
                }
                else
                {
                    ValorTotal = Total;
                    ValorTotalPP = TotalProntoPago;
                    ListaMatriculaRubro.set_list(new List<aca_Matricula_Rubro_Info>(), Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
                }
                
            }
            else
            {
                lst_MatriculaRubro = ListaMatriculaRubro.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
                var lst_nueva_plantilla = bus_matricula_rubro.GetList_Matricula(IdEmpresa, IdAnio, IdPlantilla);
                var lista_no_cobrado = lst_MatriculaRubro.Where(q=>q.EnMatricula==false).ToList();

                List<aca_Matricula_Rubro_Info> lista_nueva = new List<aca_Matricula_Rubro_Info>();

                foreach (var item in lst_MatriculaRubro)
                {
                    if (item.EnMatricula== true)
                    {
                        lista_nueva.Add(item);
                    }
                }

                
                foreach (var item1 in lista_no_cobrado)
                {
                    foreach (var item2 in lst_nueva_plantilla)
                    {
                        if (item1.IdPeriodo == item2.IdPeriodo)
                        {
                            item2.IdMecanismo = item1.IdMecanismo;
                            lista_nueva.Add(item2);
                        }
                    }
                }

                ListaMatriculaRubro.set_list(lista_nueva, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            }
            
            
            return Json(new { Valor = ValorTotal, ProntoPago = ValorTotalPP }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SumarValores(int IdAnio = 0, int IdPlantilla = 0, string Seleccionados = "")
        {
            var IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            decimal Total = 0;
            decimal TotalProntoPago = 0;
            decimal ValorRubro = 0;
            decimal ValorDescuento = 0;

            if (Seleccionados != null && Seleccionados != "")
            {
                string[] array = Seleccionados.Split(',');
                var info_plantilla = bus_plantilla.GetInfo(IdEmpresa, IdAnio, IdPlantilla);
                
                var lst_detalle = ListaMatriculaRubro.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
                foreach (var item in array)
                {
                    var info = lst_detalle.Where(q=>q.IdString== item).FirstOrDefault();
                    var info_anio_periodo = bus_anio_periodo.GetInfo(IdEmpresa, IdAnio, info.IdPeriodo);
                    if (info.AplicaProntoPago == true)
                    {
                        if (DateTime.Now.Date <= info_anio_periodo.FechaProntoPago)
                        {
                            /*
                            if (info_plantilla.TipoDescuento == "%")
                            {
                                ValorDescuento = (info.Total * (info_plantilla.Valor / 100));
                                ValorRubro = info.Total - ValorDescuento;
                                TotalProntoPago = TotalProntoPago + Math.Round(ValorRubro, 2, MidpointRounding.AwayFromZero);
                            }
                            else
                            {
                                ValorRubro = (info.Total - info_plantilla.Valor);
                                TotalProntoPago = TotalProntoPago + Math.Round(ValorRubro, 2, MidpointRounding.AwayFromZero);
                            }
                            */
                            ValorRubro = info.ValorProntoPago;
                            TotalProntoPago = TotalProntoPago + Math.Round(ValorRubro, 2, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            ValorRubro = info.Total;
                            TotalProntoPago = TotalProntoPago + Math.Round((ValorRubro), 2, MidpointRounding.AwayFromZero);
                        }

                        Total = Total + Math.Round((info.Total), 2, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        ValorRubro = info.Total;
                        Total = Total + Math.Round((info.Total), 2, MidpointRounding.AwayFromZero);
                        TotalProntoPago = TotalProntoPago + Math.Round(ValorRubro, 2, MidpointRounding.AwayFromZero);
                    }
                    
                }
            }
            return Json(new { ValorPlantilla= Total, ValorPlantillaProntoPago  = TotalProntoPago }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidarMatricula(int IdEmpresa = 0, int IdAnio = 0, int IdAlumno = 0, string Nuevo = "S")
        {
            var IdCatalogoPERNEG_Negar = Convert.ToInt32(cl_enumeradores.eCatalogoPermisoMatricula.NEGAR);
            var IdCatalogoPERNEG_Permitir = Convert.ToInt32(cl_enumeradores.eCatalogoPermisoMatricula.PERMITIR);
            var mensaje = "";
            var Validacion = 0;
            var pe_cedulaRuc = "";
            if (Nuevo == "S")
            {
                var existe_matricula = bus_matricula.GetInfo_ExisteMatricula(IdEmpresa, IdAnio, IdAlumno);
                if (existe_matricula != null)
                {
                    mensaje += "El estudiante ya se encuentra matriculado en el año lectivo seleccionado.</ br > ";
                    Validacion = 1;
                }
            }

            if (Validacion == 0)
            {
                var existe_matricula = bus_matricula.GetInfo_ExisteMatricula(IdEmpresa, IdAnio, IdAlumno);
                if (existe_matricula != null)
                {
                    mensaje += "El estudiante ya se encuentra matriculado en el año lectivo seleccionado.</ br > ";
                }

                var PermitirMatricula = bus_permiso.GetInfo_ByMatricula(IdEmpresa, IdAnio, IdAlumno, IdCatalogoPERNEG_Permitir);

                if (PermitirMatricula!=null && PermitirMatricula.IdPermiso!=0)
                {

                }
                else
                {
                    var NegarMatricula = bus_permiso.GetInfo_ByMatricula(IdEmpresa, IdAnio, IdAlumno, IdCatalogoPERNEG_Negar);
                    if (NegarMatricula != null)
                    {
                        var IdUsuario = (string.IsNullOrEmpty(NegarMatricula.IdUsuarioModificacion) ? NegarMatricula.IdUsuarioCreacion : NegarMatricula.IdUsuarioCreacion);
                        mensaje += "El estudiante tiene negación de matrícula. Observación: " + NegarMatricula.Observacion + " , usuario: " + IdUsuario + " </br>";
                    }

                    List<cxc_cobro_Info> lst_DeudaAlumno = bus_cobro.get_list_deuda(IdEmpresa, IdAlumno);

                    if (lst_DeudaAlumno.Sum(q => q.cr_saldo) > 0)
                    {
                        var Saldo = Math.Round(lst_DeudaAlumno.Sum(q => q.cr_saldo), 2, MidpointRounding.AwayFromZero).ToString("C2");
                        mensaje += "El estudiante tiene saldo pendiente: " + Saldo + ".</br>";
                    }

                    List<fa_notaCreDeb_Info> lst_CreditoAlumno = bus_notaDebCre.get_list_credito_favor(IdEmpresa, IdAlumno);

                    if (lst_CreditoAlumno.Sum(q => q.sc_saldo) > 0)
                    {
                        var Saldo = Math.Round(lst_CreditoAlumno.Sum(q => Convert.ToDouble(q.sc_saldo)), 2, MidpointRounding.AwayFromZero).ToString("C2");
                        mensajeInfo += "El estudiante tiene un saldo a favor: " + Saldo + ".</br>";
                    }

                    var ObsMatriculaCondicional = "";
                    List<aca_MatriculaCondicional_Info> lst_MatriculaCondicional = bus_matricula_condicional.GetList_Matricula(IdEmpresa, IdAnio, IdAlumno);

                    if (lst_MatriculaCondicional.Count() > 0)
                    {
                        var cant = lst_MatriculaCondicional.Count();
                        var cont = 0;
                        foreach (var item in lst_MatriculaCondicional)
                        {
                            ObsMatriculaCondicional += item.Observacion;
                            if (cont < (cant - 1))
                            {
                                ObsMatriculaCondicional = ObsMatriculaCondicional + " - ";
                            }
                            cont++;
                        }
                        mensajeInfo = "El estudiante tiene matrícula condicional. Observación: " + ObsMatriculaCondicional;
                    }
                }
            }

            return Json(new { msg = mensaje , msgInf = mensajeInfo }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult guardar(DateTime Fecha, int IdEmpresa = 0, int IdAnio = 0, decimal IdAlumno = 0, string IdComboCurso = "", int IdParalelo = 0, int IdPlantilla = 0,
            int IdMecanismo = 0, int IdMecanismoDet = 0,string Observacion = "",string Ids = "", string IDs_Doc="", int IdSucursal = 0, int IdPuntoVta=0, string IdCatalogo_FormaPago="", 
            string vt_serie1="", string vt_serie2 = "", string vt_NumFactura = "", int IdEmpresa_rol = 0 , decimal IdEmpleado=0, int IdCatalogoESTMAT = 0, bool EsPatrocinado = false, decimal IdTransaccionSession = 0)
        {
            decimal Matricula = 0;
            int Empresa = 0;
            int Plantilla = 0;
            int Periodo = 0;
            int Rubro = 0;
            var lst_DetallePlantilla = ListaMatriculaRubro.get_list(Convert.ToDecimal(IdTransaccionSession));
            var lst_DetalleDocumentos = Lista_DocumentosMatricula.get_list(Convert.ToDecimal(IdTransaccionSession));

            if (IdAnio != 0 && IdAlumno != 0 && IdComboCurso != "" && IdParalelo != 0 && IdPlantilla != 0 && IdPuntoVta != 0 && IdMecanismo != 0 && IdMecanismoDet != 0 && lst_DetallePlantilla.Count() > 0 && Ids != "")
            {
                //IdEmpresa = Convert.ToInt32(model.IdComboCurso.Substring(0, 4));
                //IdAnio = Convert.ToInt32(model.IdComboCurso.Substring(4, 4));
                int IdSede = Convert.ToInt32(IdComboCurso.Substring(8, 4));
                int IdNivel = Convert.ToInt32(IdComboCurso.Substring(12, 4));
                int IdJornada = Convert.ToInt32(IdComboCurso.Substring(16, 4));
                int IdCurso = Convert.ToInt32(IdComboCurso.Substring(20, 4));

                var info_rep_eco = bus_familia.GetInfo_Representante(IdEmpresa, IdAlumno, cl_enumeradores.eTipoRepresentante.ECON.ToString());
                var info_rep_legal = bus_familia.GetInfo_Representante(IdEmpresa, IdAlumno, cl_enumeradores.eTipoRepresentante.LEGAL.ToString());

                var info_mecanismo = bus_mecanismo.GetInfo(IdEmpresa, IdMecanismo);
                var info_termino_pago = bus_termino_pago.get_info(info_mecanismo.IdTerminoPago);
                if (info_termino_pago != null && info_termino_pago.AplicaDescuentoNomina == false)
                {
                    info_mecanismo = bus_mecanismo.GetInfo(IdEmpresa, IdMecanismoDet);
                    info_termino_pago = bus_termino_pago.get_info(info_mecanismo.IdTerminoPago);
                }
                var info_alumno = bus_alumno.GetInfo(IdEmpresa, IdAlumno);
                var pe_cedulaRuc = (info_alumno == null ? "" : info_alumno.pe_cedulaRuc);

                aca_Matricula_Info info_matricula = new aca_Matricula_Info
                {
                    IdEmpresa = IdEmpresa,
                    IdAlumno = IdAlumno,
                    IdAnio = IdAnio,
                    IdSede = IdSede,
                    IdNivel = IdNivel,
                    IdJornada = IdJornada,
                    IdCurso = IdCurso,
                    IdParalelo = IdParalelo,
                    IdPersonaF = (info_rep_eco == null ? 0 : info_rep_eco.IdPersona),
                    IdPersonaR = (info_rep_legal == null ? 0 : info_rep_legal.IdPersona),
                    IdPlantilla = IdPlantilla,
                    IdMecanismo = IdMecanismo,
                    Fecha = Fecha.Date,
                    Observacion = Observacion,
                    IdCatalogoESTMAT = IdCatalogoESTMAT,
                    IdEmpresa_rol = ((info_termino_pago!=null && info_termino_pago.AplicaDescuentoNomina == true)? IdEmpresa_rol :(int?)null ),
                    IdEmpleado = ((info_termino_pago != null && info_termino_pago.AplicaDescuentoNomina == true) ? IdEmpleado : (decimal?)null ),
                    EsPatrocinado = EsPatrocinado,
                    IdUsuarioCreacion = SessionFixed.IdUsuario,
                    lst_MatriculaRubro = new List<aca_Matricula_Rubro_Info>(),
                    pe_cedulaRuc= pe_cedulaRuc
                };

                string[] array = Ids.Split(',');
                if (lst_DetallePlantilla.Count > 0)
                {
                    foreach (var item_det in lst_DetallePlantilla)
                    {
                        aca_Matricula_Rubro_Info info_mat_rubro = new aca_Matricula_Rubro_Info
                        {
                            IdEmpresa = IdEmpresa,
                            IdPeriodo = item_det.IdPeriodo,
                            IdRubro = item_det.IdRubro,
                            IdMecanismo = IdMecanismoDet,
                            IdProducto = item_det.IdProducto,
                            Subtotal = item_det.Subtotal,
                            IdCod_Impuesto_Iva = item_det.IdCod_Impuesto_Iva,
                            Porcentaje = item_det.Porcentaje,
                            ValorIVA = item_det.ValorIVA,
                            Total = item_det.Total,
                            IdAnio = item_det.IdAnio,
                            IdPlantilla = item_det.IdPlantilla,
                            ValorProntoPago = item_det.ValorProntoPago,
                            FechaProntoPago = item_det.FechaProntoPago,
                            IdSede = IdSede,
                            IdNivel = IdNivel,
                            IdJornada = IdJornada,
                            IdCurso = IdCurso,
                            IdParalelo = IdParalelo
                        };

                        info_matricula.lst_MatriculaRubro.Add(info_mat_rubro);
                    }

                    if (Ids != "")
                    {
                        foreach (var item in array)
                        {

                            Empresa = Convert.ToInt32(item.Substring(0, 7));
                            Plantilla = Convert.ToInt32(item.Substring(8, 6));
                            Periodo = Convert.ToInt32(item.Substring(15, 6));
                            Rubro = Convert.ToInt32(item.Substring(22, 6));
                            var info_detalle = lst_DetallePlantilla.Where(q => q.IdPlantilla == Plantilla && q.IdRubro == Rubro && q.IdPeriodo == Periodo).FirstOrDefault();

                            foreach (var item1 in info_matricula.lst_MatriculaRubro)
                            {
                                if (item1.IdPeriodo == Periodo && item1.IdRubro == Rubro)
                                {
                                    item1.EnMatricula = true;
                                    item1.IdMecanismo = info_matricula.IdMecanismo;
                                }

                            }

                        }
                    }
                }

                string[] array_doc = IDs_Doc.Split(',');
                var lst_alumno_documentos = new List<aca_AlumnoDocumento_Info>();
                if (IDs_Doc != "")
                {
                    foreach (var item in array_doc)
                    {
                        var existe_documento = bus_alumno_documento.GetInfo(info_matricula.IdEmpresa, info_matricula.IdAlumno, Convert.ToInt32(item));
                        if (existe_documento == null)
                        {
                            var info_doc = new aca_AlumnoDocumento_Info
                            {
                                IdEmpresa = info_matricula.IdEmpresa,
                                IdAlumno = info_matricula.IdAlumno,
                                IdDocumento = Convert.ToInt32(item),
                                EnArchivo = true
                            };

                            lst_alumno_documentos.Add(info_doc);
                        }
                    }
                }

                info_matricula.lst_documentos = lst_alumno_documentos;

                #region Calificacion y conducta
                var lst_materias_x_curso = bus_materias_x_paralelo.GetList(IdEmpresa, IdSede, IdAnio, IdNivel, IdJornada, IdCurso, IdParalelo);
                var lst_materias_cualitativas = lst_materias_x_curso.Where(q => q.IdCatalogoTipoCalificacion == Convert.ToInt32(cl_enumeradores.eCatalogoTipoCalificacion.CUALI)).ToList();
                var lst_materias_cuantitativas = lst_materias_x_curso.Where(q => q.IdCatalogoTipoCalificacion == Convert.ToInt32(cl_enumeradores.eCatalogoTipoCalificacion.CUANTI)).ToList();

                var lst_parcial = bus_parcial.GetList_x_Tipo(IdEmpresa, IdSede, IdAnio, Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM1));
                lst_parcial.AddRange(bus_parcial.GetList_x_Tipo(IdEmpresa, IdSede, IdAnio, Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.QUIM2)));

                info_matricula.lst_MatriculaCalificacionCualitativa = new List<aca_MatriculaCalificacionCualitativa_Info>();
                info_matricula.lst_calificacion_parcial = new List<aca_MatriculaCalificacionParcial_Info>();
                info_matricula.lst_calificacion = new List<aca_MatriculaCalificacion_Info>();
                info_matricula.lst_conducta = new List<aca_MatriculaConducta_Info>();
                info_matricula.lst_asistencia = new List<aca_MatriculaAsistencia_Info>();
                info_matricula.lst_MatriculaCalificacionCualitativaPromedio = new List<aca_MatriculaCalificacionCualitativaPromedio_Info>();
                info_matricula.lst_MatriculaGrado = new List<aca_MatriculaGrado_Info>();

                #region Cuantitativas
                if (lst_materias_cuantitativas != null && lst_materias_cuantitativas.Count > 0)
                {
                    foreach (var item in lst_materias_cuantitativas)
                    {
                        if (lst_parcial != null && lst_parcial.Count > 0)
                        {
                            foreach (var item_p in lst_parcial)
                            {
                                var info_calificacion_parcial = new aca_MatriculaCalificacionParcial_Info
                                {
                                    IdProfesor = item.IdProfesor,
                                    IdMateria = item.IdMateria,
                                    IdCatalogoParcial = item_p.IdCatalogoParcial
                                };

                                info_matricula.lst_calificacion_parcial.Add(info_calificacion_parcial);

                            }
                        }

                        var info_calificacion = new aca_MatriculaCalificacion_Info
                        {
                            IdProfesor = item.IdProfesor,
                            IdMateria = item.IdMateria
                        };

                        info_matricula.lst_calificacion.Add(info_calificacion);
                    }
                }                
                #endregion

                #region Cualitativas
                if (lst_materias_cualitativas != null && lst_materias_cualitativas.Count > 0)
                {
                    foreach (var item in lst_materias_cualitativas)
                    {
                        if (lst_parcial != null && lst_parcial.Count > 0)
                        {
                            foreach (var item_p in lst_parcial)
                            {
                                var info_calificacion_cualitativa_parcial = new aca_MatriculaCalificacionCualitativa_Info
                                {
                                    IdProfesor = item.IdProfesor,
                                    IdMateria = item.IdMateria,
                                    IdCatalogoParcial = item_p.IdCatalogoParcial
                                };

                                info_matricula.lst_MatriculaCalificacionCualitativa.Add(info_calificacion_cualitativa_parcial);

                            }
                        }
                        var info_calificacion_cualitativa_promedio = new aca_MatriculaCalificacionCualitativaPromedio_Info
                        {
                            IdProfesor = item.IdProfesor,
                            IdMateria = item.IdMateria
                        };

                        info_matricula.lst_MatriculaCalificacionCualitativaPromedio.Add(info_calificacion_cualitativa_promedio);
                    }
                }
                #endregion

                #endregion

                var personaRep = bus_persona.get_info(info_matricula.IdPersonaF);
                var ExisteCliente = bus_cliente.get_info_x_num_cedula(info_matricula.IdEmpresa, (personaRep==null ? "": personaRep.pe_cedulaRuc));

                if (info_matricula.IdPersonaR != 0 && info_matricula.IdPersonaF != 0 && ExisteCliente.IdCliente!=0)
                {
                    if (info_termino_pago!=null && info_termino_pago.AplicaDescuentoNomina==true && (info_matricula.IdEmpresa_rol==null || info_matricula.IdEmpleado==0))
                    {
                        mensaje = "Ingrese el empleado para descuento en rol";
                    }
                    else
                    {
                        if (bus_matricula.GuardarDB(info_matricula))
                        {
                            var lst_rubros_x_cobrar = info_matricula.lst_MatriculaRubro.Where(q => q.EnMatricula == true).ToList();
                            var mecanismo = bus_mecanismo.GetInfo(info_matricula.IdEmpresa, info_matricula.IdMecanismo);
                            var termino_pago = bus_termino_pago.get_info(mecanismo.IdTerminoPago);
                            var punto_venta = bus_punto_venta.get_info(info_matricula.IdEmpresa, Convert.ToInt32(SessionFixed.IdSucursal), Convert.ToInt32(IdPuntoVta));
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
                                    IdSucursal = IdSucursal,
                                    IdBodega = punto_venta.IdBodega,
                                    vt_tipoDoc = "FACT",
                                    vt_serie1 = vt_serie1,
                                    vt_serie2 = vt_serie2,
                                    vt_NumFactura = vt_NumFactura,
                                    IdAlumno = info_matricula.IdAlumno,
                                    IdCliente = Cliente.IdCliente,
                                    IdVendedor = 1,
                                    IdNivel = 1,
                                    IdCatalogo_FormaPago = IdCatalogo_FormaPago,
                                    vt_fecha = DateTime.Now,
                                    vt_plazo = termino_pago.Dias_Vct,
                                    vt_fech_venc = DateTime.Now.AddDays(termino_pago.Dias_Vct),
                                    vt_tipo_venta = mecanismo.IdTerminoPago,
                                    vt_Observacion = ObsFactura,
                                    Estado = "A",
                                    IdCaja = punto_venta.IdCaja,
                                    IdEmpresa_rol = info_matricula.IdEmpresa_rol,
                                    IdEmpleado = info_matricula.IdEmpleado,
                                    IdUsuario = SessionFixed.IdUsuario,
                                    IdPuntoVta = IdPuntoVta,
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
                                    mensaje = "No se ha podido guardar la factura";
                                }
                                else
                                {
                                    #region Actualiza MatriculaRubro
                                    //var info_matricula_rubro = bus_matricula_rubro.GetInfo(info_matricula.IdEmpresa, info_matricula.IdMatricula, Convert.ToInt32(info_factura.info_resumen.IdPeriodo), Convert.ToInt32(info_factura.info_resumen.IdRubro));
                                    //info_matricula_rubro.IdSucursal = info_factura.IdSucursal;
                                    //info_matricula_rubro.IdBodega = info_factura.IdBodega;
                                    //info_matricula_rubro.IdCbteVta = info_factura.IdCbteVta;
                                    //info_matricula_rubro.FechaFacturacion = info_factura.vt_fecha;

                                    //if (!bus_matricula_rubro.ModificarDB(info_matricula_rubro))
                                    //{
                                    //    mensaje = "No se ha podido modificar el rubro por matricula";
                                    //}
                                    #endregion
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            mensaje = "No se ha podido guardar el registro";
                        }
                    }

                Empresa = info_matricula.IdEmpresa;
                Matricula = info_matricula.IdMatricula;
                }
                else
                {
                    if (ExisteCliente.IdCliente==0 || ExisteCliente==null)
                    {
                        mensaje = "Verique que exista el cliente";
                    }
                    else
                    {
                        mensaje = "Verique que el alumno tenga asignados representantes";
                    }
                }
            }
            else
            {
                mensaje = "Ingrese la información solicitada";
            }
                
            return Json(new { msg = mensaje, IdEmpresa = Empresa, IdMatricula = Matricula }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult actualizar(int IdEmpresa = 0, int IdMatricula = 0, string Observacion = "", decimal IdTransaccionSession = 0)
        {
            var info_matricula = bus_matricula.GetInfo(IdEmpresa, IdMatricula);

            info_matricula.Observacion = Observacion;
            info_matricula.IdUsuarioModificacion = SessionFixed.IdUsuario;

            if (!bus_matricula.ModificarDB(info_matricula))
            {
                mensaje = "El registro no ha podido ser modificado";
            }
                
            return Json(new { msg = mensaje, IdEmpresa = info_matricula.IdEmpresa, IdMatricula = info_matricula.IdMatricula }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditarMecanismo(string IdString = "")
        {
            string resultado = string.Empty;
            int IdEmpresa = Convert.ToInt32(IdString.Substring(0, 4));
            int IdMatricula = Convert.ToInt32(IdString.Substring(4, 6));
            int IdPeriodo = Convert.ToInt32(IdString.Substring(10, 4));
            int IdRubro = Convert.ToInt32(IdString.Substring(14, 6));

            var model = bus_matricula_rubro.GetInfo(IdEmpresa, IdMatricula, IdPeriodo, IdRubro);
            var lst_detalle = ListaMatriculaRubro.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            if (model != null)
            {
                if (lst_detalle.Count >0)
                {
                    foreach (var item in lst_detalle)
                    {
                        if (item.IdEmpresa == IdEmpresa && item.IdMatricula == IdMatricula && item.IdPeriodo == IdPeriodo && item.IdRubro == IdRubro)
                        { 

                        }
                    }
                }
            }

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult VerMatriculaAnterior(int IdEmpresa = 0, int IdAnio = 0, int IdAlumno = 0)
        {
            mensaje = "";
            decimal IdMatricula = 0;
            var info_anio = bus_anio.GetInfo(IdEmpresa, IdAnio);
            var Anioanterior = info_anio.FechaDesde.Year - 1;
            var info_anio_anterior = bus_anio.GetInfo_AnioAnterior(IdEmpresa, Anioanterior);
            if (info_anio_anterior!= null)
            {
                var info_matricula_anterior = bus_matricula.GetInfo_ExisteMatricula(IdEmpresa, info_anio_anterior.IdAnio, IdAlumno);

                if (info_matricula_anterior == null)
                {
                    mensaje = "El alumno no tiene matricula anterior";
                }
                else
                {
                    IdMatricula = info_matricula_anterior.IdMatricula;
                }
            }
            else
            {
                mensaje = "El alumno no tiene matricula anterior";
            }

            return Json(new { msg=mensaje, Matricula = IdMatricula }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SetAlumnoDocumentos(int IdEmpresa = 0, int IdAnio = 0, string IdCurso = "", decimal IdAlumno = 0)
        {
            var IdSede = Convert.ToInt32(IdCurso.Substring(8, 4));
            var IdNivel = Convert.ToInt32(IdCurso.Substring(12, 4));
            var IdJornada = Convert.ToInt32(IdCurso.Substring(16, 4));
            var IdCursoMat = Convert.ToInt32(IdCurso.Substring(20, 4));
            List<aca_AnioLectivo_Curso_Documento_Info> lst_Documentos = new List<aca_AnioLectivo_Curso_Documento_Info>();

            var lst_doc_curso = bus_curso_documento.GetList_Matricula(IdEmpresa, IdSede, IdAnio, IdNivel, IdJornada, IdCursoMat);
            var lst_doc_alumno = bus_alumno_documento.GetList(IdEmpresa, IdAlumno, true);

            if (lst_doc_curso!= null && lst_doc_curso.Count>0)
            {
                foreach (var item in lst_doc_curso)
                {
                    item.seleccionado = false;
                    item.IdStringDoc = Convert.ToString(item.IdDocumento);

                    foreach (var item1 in lst_doc_alumno)
                    {
                        if (item.IdDocumento == item1.IdDocumento)
                        {
                            item.seleccionado = true;
                            break;
                        }
                    }
                    lst_Documentos.Add(item);

                }
            }

            Lista_DocumentosMatricula.set_list(lst_Documentos, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return Json(lst_Documentos, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUltimoDocumento(int IdSucursal = 0, int IdPuntoVta = 0)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            tb_sis_Documento_Tipo_Talonario_Info resultado;
            var punto_venta = bus_punto_venta.get_info(IdEmpresa, IdSucursal, IdPuntoVta);
            if (punto_venta != null)
            {
                resultado = bus_talonario.GetUltimoNoUsado(IdEmpresa, cl_enumeradores.eTipoDocumento.FACT.ToString(), punto_venta.Su_CodigoEstablecimiento, punto_venta.cod_PuntoVta, punto_venta.EsElectronico, false);
            }
            else
                resultado = new tb_sis_Documento_Tipo_Talonario_Info();
            if (resultado == null)
                resultado = new tb_sis_Documento_Tipo_Talonario_Info();
            return Json(new { data_puntovta = punto_venta, data_talonario = resultado }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDatosMecanismo(int IdEmpresa = 0, int IdMecanismo = 0, int IdMecanismoOtros = 0)
        {
            bool resultado = false;
            var Empresa = 0;
            var info_mecanismo = bus_mecanismo.GetInfo(IdEmpresa, IdMecanismo);
            var info_termino_pago = bus_termino_pago.get_info(info_mecanismo.IdTerminoPago);

            if (info_termino_pago != null && info_termino_pago.AplicaDescuentoNomina == true)
            {
                resultado = info_termino_pago.AplicaDescuentoNomina ?? false;
                Empresa = (info_mecanismo == null ? 0 : info_mecanismo.IdEmpresa_rol??0);
            }
            else
            {
                if (IdMecanismoOtros!=0)
                {
                    info_mecanismo = bus_mecanismo.GetInfo(IdEmpresa, IdMecanismoOtros);
                    info_termino_pago = bus_termino_pago.get_info(info_mecanismo.IdTerminoPago);
                    if (info_termino_pago != null && info_termino_pago.AplicaDescuentoNomina == true)
                    {
                        resultado = info_termino_pago.AplicaDescuentoNomina ?? false;
                        Empresa = (info_mecanismo == null ? 0 : info_mecanismo.IdEmpresa_rol??0);
                    }
                }
            }

            return Json(new { MostrarEmpleado = resultado, IdEmpresa_rol = Empresa}, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Acciones
        public ActionResult Nuevo(int IdEmpresa = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            aca_Menu_x_seg_usuario_Info inf = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(SessionFixed.IdSede), SessionFixed.IdUsuario, "Academico", "Matricula", "Index");
            if (!inf.Nuevo)
                return RedirectToAction("Index");
            #endregion

            var info = bus_anio.GetInfo_AnioEnCurso(Convert.ToInt32(SessionFixed.IdEmpresa), 0);
            aca_Matricula_Info model = new aca_Matricula_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSede = Convert.ToInt32(SessionFixed.IdSede),
                IdAnio = (info == null ? 0 : info.IdAnio),
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                Fecha = DateTime.Now.Date,
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                IdCatalogo_FormaPago = "CRE",
                Validar = "S",
                IdEmpresa_rol= 0,
                IdCatalogoESTMAT = Convert.ToInt32(cl_enumeradores.eCatalogoAcademicoMatricula.MATRICULADO)
            };
            model.lst_matricula_curso = new List<aca_Matricula_Info>();
            model.lst_alumno_documentos = new List<aca_AnioLectivo_Curso_Documento_Info>();
            
            Lista_Matricula_PorCurso.set_list(model.lst_matricula_curso, model.IdTransaccionSession);
            cargar_combos();
            return View(model);
        }

        public ActionResult Consultar(int IdEmpresa = 0, int IdMatricula = 0, bool Exito = false)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            aca_Menu_x_seg_usuario_Info inf = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(SessionFixed.IdSede), SessionFixed.IdUsuario, "Academico", "Matricula", "Index");
            ViewBag.Nuevo = inf.Nuevo;
            ViewBag.Modificar = inf.Modificar;
            ViewBag.Anular = inf.Anular;
            #endregion

            aca_Matricula_Info model = bus_matricula.GetInfo(IdEmpresa, IdMatricula);
            model.Validar = "N";

            var info_curso = bus_jornada_curso.GetInfoCursoMatricula(model.IdEmpresa, model.IdAnio, model.IdMatricula);
            model.IdComboCurso = (info_curso == null ? "" : info_curso.IdComboCurso);
            model.NomCurso = (info_curso == null ? "" : info_curso.NomCurso);

            var IdAnio = Convert.ToInt32(model.IdComboCurso.Substring(4, 4));
            var IdSede = Convert.ToInt32(model.IdComboCurso.Substring(8, 4));
            var IdNivel = Convert.ToInt32(model.IdComboCurso.Substring(12, 4));
            var IdJornada = Convert.ToInt32(model.IdComboCurso.Substring(16, 4));
            var IdCursoMat = Convert.ToInt32(model.IdComboCurso.Substring(20, 4));

            if (model == null)
                return RedirectToAction("Index");

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            model.IdEmpresa_rol = (model.IdEmpresa_rol == null ? IdEmpresa : model.IdEmpresa_rol);
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession);
            model.lst_matricula_curso = new List<aca_Matricula_Info>();
            model.lst_matricula_curso = bus_matricula.GetList_PorCurso(model.IdEmpresa, model.IdAnio, model.IdSede, model.IdNivel, model.IdJornada, model.IdCurso, model.IdParalelo);
            Lista_Matricula_PorCurso.set_list(model.lst_matricula_curso, model.IdTransaccionSession);

            model.lst_MatriculaRubro = new List<aca_Matricula_Rubro_Info>();
            model.lst_MatriculaRubro = bus_matricula_rubro.GetList(model.IdEmpresa, model.IdMatricula);
            ListaMatriculaRubro.set_list(model.lst_MatriculaRubro, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            model.lst_alumno_documentos = new List<aca_AnioLectivo_Curso_Documento_Info>();
            var lst_doc_curso = bus_curso_documento.GetList_Matricula(IdEmpresa, IdSede, IdAnio, IdNivel, IdJornada, IdCursoMat);
            var lst_doc_alumno = bus_alumno_documento.GetList(IdEmpresa, model.IdAlumno, true);

            if (lst_doc_curso != null && lst_doc_curso.Count > 0)
            {
                foreach (var item in lst_doc_curso)
                {
                    item.seleccionado = false;
                    item.IdStringDoc = Convert.ToString(item.IdDocumento);

                    foreach (var item1 in lst_doc_alumno)
                    {
                        if (item.IdDocumento == item1.IdDocumento)
                        {
                            item.seleccionado = true;
                            break;
                        }
                    }
                    model.lst_alumno_documentos.Add(item);
                }
            }

            var CambioPlantilla = bus_cambios.getInfo_UltimoCambioPlantilla(IdEmpresa, IdMatricula);
            var CambioCurso = bus_cambios.getInfo_UltimoCambioParalelo(IdEmpresa, IdMatricula);
            model.ObservacionCambioCurso = CambioCurso == null ? null : CambioCurso.Observacion;
            model.ObservacionCambioPlantilla = CambioPlantilla == null ? null : CambioPlantilla.Observacion;

            Lista_DocumentosMatricula.set_list(model.lst_alumno_documentos, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            cargar_combos();
            return View(model);
        }
        public ActionResult Modificar(int IdEmpresa = 0, int IdMatricula = 0, bool Exito = false)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            aca_Menu_x_seg_usuario_Info inf = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(SessionFixed.IdSede), SessionFixed.IdUsuario, "Academico", "Matricula", "Index");
            if (!inf.Modificar)
                return RedirectToAction("Index");
            #endregion

            aca_Matricula_Info model = bus_matricula.GetInfo(IdEmpresa, IdMatricula);
            model.Validar = "N";

            var info_curso = bus_jornada_curso.GetInfoCursoMatricula(model.IdEmpresa, model.IdAnio, model.IdMatricula);
            model.IdComboCurso = (info_curso==null ? "" : info_curso.IdComboCurso);
            model.NomCurso = (info_curso == null ? "" : info_curso.NomCurso);

            var IdAnio = Convert.ToInt32(model.IdComboCurso.Substring(4, 4));
            var IdSede = Convert.ToInt32(model.IdComboCurso.Substring(8, 4));
            var IdNivel = Convert.ToInt32(model.IdComboCurso.Substring(12, 4));
            var IdJornada = Convert.ToInt32(model.IdComboCurso.Substring(16, 4));
            var IdCursoMat = Convert.ToInt32(model.IdComboCurso.Substring(20, 4));

            if (model == null)
                return RedirectToAction("Index");

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            model.IdEmpresa_rol = (model.IdEmpresa_rol == null ? IdEmpresa : model.IdEmpresa_rol);
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession);
            model.lst_matricula_curso = new List<aca_Matricula_Info>();
            model.lst_matricula_curso = bus_matricula.GetList_PorCurso(model.IdEmpresa, model.IdAnio, model.IdSede, model.IdNivel, model.IdJornada, model.IdCurso, model.IdParalelo);
            Lista_Matricula_PorCurso.set_list(model.lst_matricula_curso, model.IdTransaccionSession);

            model.lst_MatriculaRubro = new List<aca_Matricula_Rubro_Info>();
            model.lst_MatriculaRubro = bus_matricula_rubro.GetList(model.IdEmpresa, model.IdMatricula);
            ListaMatriculaRubro.set_list(model.lst_MatriculaRubro, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            model.lst_alumno_documentos = new List<aca_AnioLectivo_Curso_Documento_Info>();
            var lst_doc_curso = bus_curso_documento.GetList_Matricula(IdEmpresa, IdSede, IdAnio, IdNivel, IdJornada, IdCursoMat);
            var lst_doc_alumno = bus_alumno_documento.GetList(IdEmpresa, model.IdAlumno, true);

            if (lst_doc_curso != null && lst_doc_curso.Count > 0)
            {
                foreach (var item in lst_doc_curso)
                {
                    item.seleccionado = false;
                    item.IdStringDoc = Convert.ToString(item.IdDocumento);

                    foreach (var item1 in lst_doc_alumno)
                    {
                        if (item.IdDocumento == item1.IdDocumento)
                        {
                            item.seleccionado = true;
                            break;
                        }     
                    }
                    model.lst_alumno_documentos.Add(item);
                }
            }

            var CambioPlantilla = bus_cambios.getInfo_UltimoCambioPlantilla(IdEmpresa, IdMatricula);
            var CambioCurso = bus_cambios.getInfo_UltimoCambioParalelo(IdEmpresa, IdMatricula);
            model.ObservacionCambioCurso = CambioCurso == null ? null : CambioCurso.Observacion;
            model.ObservacionCambioPlantilla = CambioPlantilla == null ? null : CambioPlantilla.Observacion;

            Lista_DocumentosMatricula.set_list(model.lst_alumno_documentos, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            cargar_combos();
            return View(model);
        }

        public ActionResult Eliminar(int IdEmpresa = 0, int IdMatricula = 0, bool Exito = false)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            #region Permisos
            aca_Menu_x_seg_usuario_Info inf = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(SessionFixed.IdSede), SessionFixed.IdUsuario, "Academico", "Matricula", "Index");
            if (!inf.Anular)
                return RedirectToAction("Index");
            #endregion

            aca_Matricula_Info model = bus_matricula.GetInfo(IdEmpresa, IdMatricula);
            model.Validar = "N";

            var info_curso = bus_jornada_curso.GetInfoCursoMatricula(model.IdEmpresa, model.IdAnio, model.IdMatricula);
            model.IdComboCurso = (info_curso == null ? "" : info_curso.IdComboCurso);
            model.NomCurso = (info_curso == null ? "" : info_curso.NomCurso);

            var IdAnio = Convert.ToInt32(model.IdComboCurso.Substring(4, 4));
            var IdSede = Convert.ToInt32(model.IdComboCurso.Substring(8, 4));
            var IdNivel = Convert.ToInt32(model.IdComboCurso.Substring(12, 4));
            var IdJornada = Convert.ToInt32(model.IdComboCurso.Substring(16, 4));
            var IdCursoMat = Convert.ToInt32(model.IdComboCurso.Substring(20, 4));

            if (model == null)
                return RedirectToAction("Index");

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession);
            model.lst_matricula_curso = new List<aca_Matricula_Info>();
            model.lst_matricula_curso = bus_matricula.GetList_PorCurso(model.IdEmpresa, model.IdAnio, model.IdSede, model.IdNivel, model.IdJornada, model.IdCurso, model.IdParalelo);
            Lista_Matricula_PorCurso.set_list(model.lst_matricula_curso, model.IdTransaccionSession);

            model.lst_MatriculaRubro = new List<aca_Matricula_Rubro_Info>();
            model.lst_MatriculaRubro = bus_matricula_rubro.GetList(model.IdEmpresa, model.IdMatricula);
            ListaMatriculaRubro.set_list(model.lst_MatriculaRubro, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            model.IdEmpresa_rol = (model.IdEmpresa_rol == null ? IdEmpresa : model.IdEmpresa_rol);
            model.lst_alumno_documentos = new List<aca_AnioLectivo_Curso_Documento_Info>();
            var lst_doc_curso = bus_curso_documento.GetList_Matricula(IdEmpresa, IdSede, IdAnio, IdNivel, IdJornada, IdCursoMat);
            var lst_doc_alumno = bus_alumno_documento.GetList(IdEmpresa, model.IdAlumno, true);

            if (lst_doc_curso != null && lst_doc_curso.Count > 0)
            {
                foreach (var item in lst_doc_curso)
                {
                    item.seleccionado = false;
                    item.IdStringDoc = Convert.ToString(item.IdDocumento);

                    foreach (var item1 in lst_doc_alumno)
                    {
                        if (item.IdDocumento == item1.IdDocumento)
                        {
                            item.seleccionado = true;
                            break;
                        }
                    }
                    model.lst_alumno_documentos.Add(item);
                }
            }

            var CambioPlantilla = bus_cambios.getInfo_UltimoCambioPlantilla(IdEmpresa, IdMatricula);
            var CambioCurso = bus_cambios.getInfo_UltimoCambioParalelo(IdEmpresa, IdMatricula);
            model.ObservacionCambioCurso = CambioCurso == null ? null : CambioCurso.Observacion;
            model.ObservacionCambioPlantilla = CambioPlantilla == null ? null : CambioPlantilla.Observacion;

            Lista_DocumentosMatricula.set_list(model.lst_alumno_documentos, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Eliminar(aca_Matricula_Info model)
        {
            var guardar = true;
            model.lst_MatriculaRubro = ListaMatriculaRubro.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            foreach (var item in model.lst_MatriculaRubro)
            {
                if (item.IdCbteVta != null)
                {
                    var info_factura = bus_factura.get_info(item.IdEmpresa, Convert.ToInt32(item.IdSucursal), Convert.ToInt32(item.IdBodega), Convert.ToInt32(item.IdCbteVta));
                    if (info_factura != null)
                    {
                        if (info_factura.Estado == "A")
                        {
                            guardar = false;
                            break;                          
                        }
                    }
                }
            }

            if (guardar==true)
            {
                if (!bus_matricula.EliminarDB(model))
                {
                    ViewBag.mensaje = "No se ha podido anular el registro";
                    SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                    cargar_combos();
                    return View(model);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ViewBag.mensaje = "No se puede eliminar la matrícula, existen facturas activas.";
                SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                cargar_combos();
                return View(model);
                //return RedirectToAction("Eliminar", new { IdEmpresa = model.IdEmpresa, IdMatricula = model.IdMatricula, Exito = false });
            }
        }

        #endregion
    }

    public class aca_Matricula_List
    {
        string Variable = "aca_Matricula_Info";
        public List<aca_Matricula_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<aca_Matricula_Info> list = new List<aca_Matricula_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<aca_Matricula_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<aca_Matricula_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }

    public class aca_Matricula_PorCurso_List
    {
        string Variable = "aca_Matricula_PorCurso_Info";
        public List<aca_Matricula_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<aca_Matricula_Info> list = new List<aca_Matricula_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<aca_Matricula_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<aca_Matricula_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }

    public class aca_Matricula_Rubro_List
    {
        string Variable = "aca_Matricula_Rubro_Info";
        public List<aca_Matricula_Rubro_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<aca_Matricula_Rubro_Info> list = new List<aca_Matricula_Rubro_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<aca_Matricula_Rubro_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<aca_Matricula_Rubro_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }

        public void UpdateRow(aca_Matricula_Rubro_Info info_det, decimal IdTransaccion)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            aca_Matricula_Rubro_Info edited_info = get_list(IdTransaccion).Where(q => q.IdString == info_det.IdString).FirstOrDefault();
            if (edited_info.FechaFacturacion==null)
            {
                edited_info.IdMecanismo = info_det.IdMecanismo;
            }
        }
    }
}