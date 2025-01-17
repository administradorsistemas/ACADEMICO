﻿using Core.Bus.Academico;
using Core.Bus.General;
using Core.Info.Academico;
using Core.Info.General;
using Core.Info.Helps;
using Core.Web.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Web.Areas.General.Controllers
{
    public class TipoDocumentoTalonarioController : Controller
    {
        #region Variables
        tb_sis_Documento_Tipo_Talonario_Bus bus_talonario = new tb_sis_Documento_Tipo_Talonario_Bus();
        tb_sis_Documento_Tipo_Bus bus_tipodoc = new tb_sis_Documento_Tipo_Bus();
        tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
        aca_Sede_Bus bus_sede = new aca_Sede_Bus();
        aca_Menu_x_seg_usuario_Bus bus_permisos = new aca_Menu_x_seg_usuario_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        #endregion

        #region Index / Metodos
        public ActionResult Index()
        {
            cl_filtros_Info model = new cl_filtros_Info
            {
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                CodDocumentoTipo = ""
            };
            cargar_combos_consulta();
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(cl_filtros_Info model)
        {
            ViewBag.IdSucursal = model.IdSucursal;
            ViewBag.CodDocumentoTipo = model.CodDocumentoTipo;
            cargar_combos_consulta();
            return View(model);
        }
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

            tb_sis_Documento_Tipo_Bus bus_tipo = new tb_sis_Documento_Tipo_Bus();
            var lst_doc = bus_tipo.get_list(false);
            lst_doc.Add(new tb_sis_Documento_Tipo_Info
            {
                codDocumentoTipo = "",
                descripcion = "Todos"
            });
            ViewBag.lst_doc = lst_doc;
        }
        [ValidateInput(false)]
        public ActionResult GridViewPartial_tipodocumentotal(int IdSucursal = 0, string CodDocumentoTipo = "")
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            ViewBag.IdEmpresa = IdEmpresa;
            ViewBag.IdSucursal = IdSucursal;
            ViewBag.CodDocumentoTipo = CodDocumentoTipo;

            var model = bus_talonario.get_list(IdEmpresa, IdSucursal, CodDocumentoTipo, true);
            return PartialView("_GridViewPartial_tipodocumentotal", model);
        }
        private void cargar_combos(int IdEmpresa)
        {
            var lst_talonario = bus_tipodoc.get_list(false);
            ViewBag.lst_talonario = lst_talonario;

            var lst_sucursal = bus_sucursal.get_list(IdEmpresa, false);
            ViewBag.lst_sucursal = lst_sucursal;
        }

        #endregion

        #region Acciones
        public ActionResult Nuevo(int IdEmpresa = 0)
        {
            tb_sis_Documento_Tipo_Talonario_Info model = new tb_sis_Documento_Tipo_Talonario_Info
            {
                IdEmpresa = IdEmpresa,
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                FechaCaducidad = DateTime.Now
            };

            #region Permisos
            aca_Menu_x_seg_usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(SessionFixed.IdSede), SessionFixed.IdUsuario, "General", "TipoDocumentoTalonario", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            cargar_combos(IdEmpresa);
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(tb_sis_Documento_Tipo_Talonario_Info model)
        {
            decimal documento_inicial = Convert.ToDecimal(model.NumDocumento);
            decimal documento_final = Convert.ToDecimal(model.Documentofinal);
            //for (decimal i = documento_inicial; i < documento_final; i++)
            //{
            //    tb_sis_Documento_Tipo_Talonario_Info info = new tb_sis_Documento_Tipo_Talonario_Info
            //    {
            //        IdEmpresa = model.IdEmpresa,
            //        CodDocumentoTipo = model.CodDocumentoTipo,
            //        Establecimiento = model.Establecimiento,
            //        PuntoEmision = model.PuntoEmision,
            //        NumDocumento = i.ToString("000000000"),
            //       es_Documento_Electronico = model.es_Documento_Electronico,
            //       FechaCaducidad = model.FechaCaducidad,
            //       IdSucursal = model.IdSucursal,
            //       NumAutorizacion = model.NumAutorizacion,
            //       Usado = model.Usado,                    
            //    };
            //    if (!bus_talonario.guardarDB(info))
            //    {
            //        cargar_combos(model.IdEmpresa);
            //        return View(model);
            //    }
            //}           
            int length = model.NumDocumento.Length;
            string relleno = string.Empty;
            for (int i = 0; i < length; i++)
            {
                relleno += "0";
            }
            decimal secuencia = documento_inicial;
            for (decimal i = documento_inicial; i < documento_final + 1; i++)
            {

                tb_sis_Documento_Tipo_Talonario_Info info = new tb_sis_Documento_Tipo_Talonario_Info
                {
                    IdEmpresa = model.IdEmpresa,
                    CodDocumentoTipo = model.CodDocumentoTipo,
                    NumDocumento = secuencia.ToString(relleno),
                    Establecimiento = model.Establecimiento,
                    PuntoEmision = model.PuntoEmision,
                    EstadoBool = model.EstadoBool,
                    Estado = model.EstadoBool == true ? "A" : "I",
                    Usado = model.Usado,
                    es_Documento_Electronico = model.es_Documento_Electronico,
                    FechaCaducidad = model.FechaCaducidad,
                    IdSucursal = model.IdSucursal,
                    NumAutorizacion = model.NumAutorizacion,
                };
                if (!bus_talonario.guardarDB(info))
                {
                    cargar_combos(model.IdEmpresa);
                    return View(model);
                }
                secuencia++;
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, CodDocumentoTipo=model.CodDocumentoTipo, Establecimiento=model.Establecimiento, PuntoEmision=model.PuntoEmision, NumDocumento=model.NumDocumento,  Exito = true });
        }

        public ActionResult Consultar(int IdEmpresa = 0, string CodDocumentoTipo = "", string Establecimiento = "", string PuntoEmision = "", string NumDocumento = "", bool Exito=false)
        {
            tb_sis_Documento_Tipo_Talonario_Info model = bus_talonario.get_info(IdEmpresa, CodDocumentoTipo, Establecimiento, PuntoEmision, NumDocumento);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            aca_Menu_x_seg_usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(SessionFixed.IdSede), SessionFixed.IdUsuario, "General", "TipoDocumentoTalonario", "Index");
            if (model.Estado == "I")
            {
                info.Modificar = false;
                info.Anular = false;
            }
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            cargar_combos(IdEmpresa);
            return View(model);
        }

        public ActionResult Modificar(int IdEmpresa = 0, string CodDocumentoTipo = "", string Establecimiento = "", string PuntoEmision = "", string NumDocumento = "")
        {
            tb_sis_Documento_Tipo_Talonario_Info model = bus_talonario.get_info(IdEmpresa, CodDocumentoTipo, Establecimiento, PuntoEmision, NumDocumento);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            aca_Menu_x_seg_usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(SessionFixed.IdSede), SessionFixed.IdUsuario, "General", "TipoDocumentoTalonario", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            cargar_combos(IdEmpresa);
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(tb_sis_Documento_Tipo_Talonario_Info model)
        {
            if (!bus_talonario.modificarDB(model))
            {
                cargar_combos(model.IdEmpresa);
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, CodDocumentoTipo = model.CodDocumentoTipo, Establecimiento = model.Establecimiento, PuntoEmision = model.PuntoEmision, NumDocumento = model.NumDocumento, Exito = true });

        }

        public ActionResult Anular(int IdEmpresa = 0, string CodDocumentoTipo = "", string Establecimiento = "", string PuntoEmision = "", string NumDocumento = "")
        {
            tb_sis_Documento_Tipo_Talonario_Info model = bus_talonario.get_info(IdEmpresa, CodDocumentoTipo, Establecimiento, PuntoEmision, NumDocumento);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            aca_Menu_x_seg_usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(SessionFixed.IdSede), SessionFixed.IdUsuario, "General", "TipoDocumentoTalonario", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            cargar_combos(IdEmpresa);
            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(tb_sis_Documento_Tipo_Talonario_Info model)
        {
            if (!bus_talonario.anularDB(model))
            {
                cargar_combos(model.IdEmpresa);
                return View(model);
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region Json
        public JsonResult get_NumeroDocumentoInicial(int IdEmpresa = 0, string CodDocumentoTipo = "", string Establecimiento = "", string PuntoEmision = "")
        {
            var NumeroDocumento = bus_talonario.get_NumeroDocumentoInicial(IdEmpresa, CodDocumentoTipo, Establecimiento, PuntoEmision);

            return Json(NumeroDocumento, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetInfoEstablecimiento(int IdEmpresa = 0, int IdSucursal = 0)
        {
            var resultado = bus_sucursal.get_info(IdEmpresa, IdSucursal);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}