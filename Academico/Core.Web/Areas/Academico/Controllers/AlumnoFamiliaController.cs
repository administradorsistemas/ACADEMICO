﻿using Core.Bus.Academico;
using Core.Bus.General;
using Core.Info.Academico;
using Core.Info.General;
using Core.Info.Helps;
using Core.Web.Helps;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Web.Areas.Academico.Controllers
{
    public class AlumnoFamiliaController : Controller
    {
        #region Variables
        aca_Familia_Bus bus_familia = new aca_Familia_Bus();
        aca_Familia_List Lista_Familia = new aca_Familia_List();
        tb_persona_Bus bus_persona = new tb_persona_Bus();
        tb_Catalogo_Bus bus_catalogo = new tb_Catalogo_Bus();
        aca_Catalogo_Bus bus_aca_catalogo = new aca_Catalogo_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        string mensaje = string.Empty;
        #endregion

        #region Metodos
        private void cargar_combos()
        {
            var lst_sexo = bus_catalogo.get_list(Convert.ToInt32(cl_enumeradores.eTipoCatalogoGeneral.SEXO), false);
            var lst_estado_civil = bus_catalogo.get_list(Convert.ToInt32(cl_enumeradores.eTipoCatalogoGeneral.ESTCIVIL), false);
            var lst_tipo_doc = bus_catalogo.get_list(Convert.ToInt32(cl_enumeradores.eTipoCatalogoGeneral.TIPODOC), false);
            var lst_tipo_naturaleza = bus_catalogo.get_list(Convert.ToInt32(cl_enumeradores.eTipoCatalogoGeneral.TIPONATPER), false);
            var lst_tipo_sangre = bus_catalogo.get_list(Convert.ToInt32(cl_enumeradores.eTipoCatalogoGeneral.TIPOSANGRE), false);
            var lst_tipo_discapacidad = bus_catalogo.get_list(Convert.ToInt32(cl_enumeradores.eTipoCatalogoGeneral.TIPODISCAP), false);
            var lst_parentezco = bus_aca_catalogo.GetList_x_Tipo(Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.PAREN),false);
            lst_tipo_discapacidad.Add(new tb_Catalogo_Info { CodCatalogo = "", ca_descripcion = "" });

            ViewBag.lst_sexo = lst_sexo;
            ViewBag.lst_estado_civil = lst_estado_civil;
            ViewBag.lst_tipo_doc = lst_tipo_doc;
            ViewBag.lst_tipo_naturaleza = lst_tipo_naturaleza;
            ViewBag.lst_tipo_sangre = lst_tipo_sangre;
            ViewBag.lst_tipo_discapacidad = lst_tipo_discapacidad;
            ViewBag.lst_parentezco = lst_parentezco;
        }

        private bool validar(aca_Familia_Info info, ref string msg)
        {
            string return_naturaleza = "";

            if (cl_funciones.ValidaIdentificacion(info.IdTipoDocumento, info.pe_Naturaleza, info.pe_cedulaRuc, ref return_naturaleza))
            {
                info.pe_Naturaleza = return_naturaleza;
            }
            else
            {
                msg = "Número de identificación del alumno inválida";
                return false;
            }

            return true;
        }
        #endregion

        #region Index
        public ActionResult Index(int IdEmpresa = 0, int IdAlumno = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion
            ViewBag.IdEmpresa = IdEmpresa;
            ViewBag.IdAlumno = IdAlumno;
            aca_Familia_Info model = new aca_Familia_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession)
            };

            List<aca_Familia_Info> lista = bus_familia.GetList(IdEmpresa, IdAlumno);
            Lista_Familia.set_list(lista, Convert.ToDecimal(SessionFixed.IdTransaccionSession));

            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_AlumnoFamilia(int IdEmpresa=0, decimal IdAlumno=0)
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            ViewBag.IdEmpresa = IdEmpresa;
            ViewBag.IdAlumno = IdAlumno;
            List<aca_Familia_Info> model = Lista_Familia.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_AlumnoFamilia", model);
        }
        #endregion

        #region Metodos ComboBox bajo demanda alumno
        public ActionResult Cmb_FamiliaAlumno()
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
        #endregion

        #region Json
        public JsonResult Validar_cedula_ruc(string naturaleza = "", string tipo_documento = "", string cedula_ruc = "")
        {
            var return_naturaleza = "";
            var isValid = cl_funciones.ValidaIdentificacion(tipo_documento, naturaleza, cedula_ruc, ref return_naturaleza);

            return Json(new { isValid = isValid, return_naturaleza = return_naturaleza }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult get_info_x_num_cedula(int IdEmpresa = 0, decimal IdAlumno=0, string pe_cedulaRuc = "")
        {
            var resultado = bus_familia.get_info_x_num_cedula(IdEmpresa, IdAlumno, pe_cedulaRuc);
            resultado.anio = Convert.ToDateTime(resultado.pe_fechaNacimiento).Year.ToString();
            var mes = Convert.ToDateTime(resultado.pe_fechaNacimiento).Month;
            mes = mes - 1;
            resultado.mes = mes.ToString();
            resultado.dia = Convert.ToDateTime(resultado.pe_fechaNacimiento).Day.ToString();

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Acciones
        public ActionResult Nuevo(int IdEmpresa = 0, int IdAlumno = 0)
        {
            aca_Familia_Info model = new aca_Familia_Info
            {
                IdEmpresa = IdEmpresa,
                IdAlumno = IdAlumno,
                pe_Naturaleza = "NATU",
                CodCatalogoCONADIS = ""
            };
            ViewBag.IdEmpresa = IdEmpresa;
            ViewBag.IdAlumno = IdAlumno;
            cargar_combos();
            return View(model);
        }
        [HttpPost]
        public ActionResult Nuevo(aca_Familia_Info model)
        {
            model.IdUsuarioCreacion = SessionFixed.IdUsuario;

            var info_persona_familia = new tb_persona_Info
            {
                IdPersona = model.IdPersona,
                pe_Naturaleza = model.pe_Naturaleza,
                IdTipoDocumento = model.IdTipoDocumento,
                pe_cedulaRuc = model.pe_cedulaRuc,
                pe_nombre = model.pe_nombre,
                pe_apellido = model.pe_apellido,
                pe_nombreCompleto = model.pe_apellido + " "+model.pe_nombre,
                pe_razonSocial = model.pe_apellido + " " + model.pe_nombre,
                IdEstadoCivil = model.IdEstadoCivil,
                pe_sexo = model.pe_sexo,
                CodCatalogoSangre = model.CodCatalogoSangre,
                CodCatalogoCONADIS = model.CodCatalogoCONADIS,
                NumeroCarnetConadis = model.NumeroCarnetConadis,
                PorcentajeDiscapacidad = model.PorcentajeDiscapacidad,
                pe_fechaNacimiento = model.pe_fechaNacimiento,
                pe_telfono_Contacto = model.pe_telfono_Contacto,
                pe_correo = model.Correo,
                pe_celular = model.Celular,
                pe_direccion = model.Direccion
            };

            model.info_persona= info_persona_familia;

            if (!validar(model, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                cargar_combos();
                return View(model);
            }

            if (!bus_familia.guardarDB(model))
            {
                ViewBag.IdAlumno = model.IdAlumno;
                ViewBag.IdEmpresa = model.IdEmpresa;
                return View(model);
            }

            //return RedirectToAction("Index", new { IdEmpresa = model.IdEmpresa, IdAlumno = model.IdAlumno });
            return RedirectToAction("Modificar", new { IdEmpresa = model.IdEmpresa, IdAlumno = model.IdAlumno, Secuencia = model.Secuencia, Exito = true });
        }

        public ActionResult Modificar(int IdEmpresa = 0, int IdAlumno = 0, int Secuencia = 0, bool Exito = false)
        {
            aca_Familia_Info model = bus_familia.GetInfo(IdEmpresa, IdAlumno, Secuencia);
            if (model == null)
                return RedirectToAction("Index", new { IdEmpresa = IdEmpresa, IdAlumno = IdAlumno });

            model.CodCatalogoCONADIS = (model.CodCatalogoCONADIS == null ? "" : model.CodCatalogoCONADIS);
            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            ViewBag.IdEmpresa = IdEmpresa;
            ViewBag.IdAlumno = IdAlumno;
            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(aca_Familia_Info model)
        {
            model.IdUsuarioModificacion = SessionFixed.IdUsuario;

            var info_persona_familia = new tb_persona_Info
            {
                IdPersona = model.IdPersona,
                pe_Naturaleza = model.pe_Naturaleza,
                IdTipoDocumento = model.IdTipoDocumento,
                pe_cedulaRuc = model.pe_cedulaRuc,
                pe_nombre = model.pe_nombre,
                pe_apellido = model.pe_apellido,
                pe_nombreCompleto = model.pe_apellido + " " + model.pe_nombre,
                pe_razonSocial = model.pe_apellido + " " + model.pe_nombre,
                IdEstadoCivil = model.IdEstadoCivil,
                pe_sexo = model.pe_sexo,
                CodCatalogoSangre = model.CodCatalogoSangre,
                CodCatalogoCONADIS = model.CodCatalogoCONADIS,
                NumeroCarnetConadis = model.NumeroCarnetConadis,
                PorcentajeDiscapacidad = model.PorcentajeDiscapacidad,
                pe_fechaNacimiento = model.pe_fechaNacimiento,
                pe_telfono_Contacto = model.pe_telfono_Contacto,
                pe_correo = model.Correo,
                pe_celular = model.Celular,
                pe_direccion = model.Direccion
            };

            model.info_persona = info_persona_familia;

            if (!bus_familia.modificarDB(model))
            {
                ViewBag.IdAlumno = model.IdAlumno;
                ViewBag.IdEmpresa = model.IdEmpresa;
                return View(model);
            }

            //return RedirectToAction("Index", new { IdEmpresa = model.IdEmpresa, IdAlumno = model.IdAlumno });
            return RedirectToAction("Modificar", new { IdEmpresa = model.IdEmpresa, IdAlumno = model.IdAlumno, Secuencia = model.Secuencia, Exito = true });
        }

        public ActionResult Anular(int IdEmpresa = 0, int IdAlumno = 0, int Secuencia = 0)
        {
            aca_Familia_Info model = bus_familia.GetInfo(IdEmpresa, IdAlumno, Secuencia);
            model.CodCatalogoCONADIS = (model.CodCatalogoCONADIS == null ? "" : model.CodCatalogoCONADIS);

            if (model == null)
                return RedirectToAction("Index", new { IdEmpresa = IdEmpresa, IdAlumno = IdAlumno });
            ViewBag.IdEmpresa = IdEmpresa;
            ViewBag.IdAlumno = IdAlumno;
            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(aca_Familia_Info model)
        {
            model.IdUsuarioAnulacion = SessionFixed.IdUsuario;
            if (!bus_familia.anularDB(model))
            {
                ViewBag.IdAlumno = model.IdAlumno;
                ViewBag.IdEmpresa = model.IdEmpresa;
                cargar_combos();
                return View(model);
            }

            return RedirectToAction("Index", new { IdEmpresa = model.IdEmpresa, IdAlumno = model.IdAlumno });
        }

        #endregion
    }

    public class aca_Familia_List
    {
        string Variable = "aca_Familia_Info";
        public List<aca_Familia_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<aca_Familia_Info> list = new List<aca_Familia_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<aca_Familia_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<aca_Familia_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}