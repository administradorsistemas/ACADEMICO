﻿using Core.Bus.General;
using Core.Info.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Web.Areas.General.Controllers
{
    public class TipoDocumentoController : Controller
    {
        #region Index
        tb_sis_Documento_Tipo_Bus bus_tipodocumento = new tb_sis_Documento_Tipo_Bus();
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_tipodocumento()
        {
            List<tb_sis_Documento_Tipo_Info> model = bus_tipodocumento.get_list(true);
            return PartialView("_GridViewPartial_tipodocumento", model);
        }

        #endregion
        #region Acciones
        public ActionResult Nuevo()
        {
            tb_sis_Documento_Tipo_Info model = new tb_sis_Documento_Tipo_Info();
            return View();
        }

        [HttpPost]
        public ActionResult Nuevo(tb_sis_Documento_Tipo_Info model)
        {

            if (bus_tipodocumento.validar_existe_CodDocumento(model.codDocumentoTipo))
            {
                ViewBag.mensaje = "El código ya se encuentra registrado";
                return View(model);
            }

            if (!bus_tipodocumento.guardarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Modificar(string codDocumentoTipo = "")
        {
            tb_sis_Documento_Tipo_Info model = bus_tipodocumento.get_info(codDocumentoTipo);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(tb_sis_Documento_Tipo_Info model)
        {
            if (!bus_tipodocumento.modificarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Anular(string codDocumentoTipo = "")
        {
            tb_sis_Documento_Tipo_Info model = bus_tipodocumento.get_info(codDocumentoTipo);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(tb_sis_Documento_Tipo_Info model)
        {
            if (!bus_tipodocumento.anularDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Index");
        }

        #endregion
    }
}