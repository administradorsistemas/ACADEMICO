﻿using Core.Bus.Caja;
using Core.Bus.Contabilidad;
using Core.Bus.CuentasPorCobrar;
using Core.Bus.Facturacion;
using Core.Info.CuentasPorCobrar;
using Core.Web.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Web.Areas.CuentasPorCobrar.Controllers
{
    public class ParametroCXCController : Controller
    {
        #region Variables
        cxc_Parametro_Bus bus_parametro = new cxc_Parametro_Bus();
        ct_cbtecble_tipo_Bus bus_tipo_comprobante = new ct_cbtecble_tipo_Bus();
        cxc_cobro_tipo_Bus bus_cobrotipo = new cxc_cobro_tipo_Bus();
        caj_Caja_Movimiento_Tipo_Bus bus_movimiento = new caj_Caja_Movimiento_Tipo_Bus();
        caj_Caja_Bus bus_caja = new caj_Caja_Bus();
        fa_TipoNota_Bus bus_tiponota = new fa_TipoNota_Bus();
        #endregion

        #region Index
        public ActionResult Index()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            cxc_Parametro_Info model = bus_parametro.get_info(IdEmpresa);
            if (model == null)
                model = new cxc_Parametro_Info { IdEmpresa = IdEmpresa };
            cargar_combos(IdEmpresa);
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(cxc_Parametro_Info model)
        {
            model.IdUsuario = SessionFixed.IdUsuario;
            model.IdUsuarioUltMod = SessionFixed.IdUsuario;
            if (!bus_parametro.guardarDB(model))
                ViewBag.mensaje = "No se pudieron actualizar los registros";
            cargar_combos(model.IdEmpresa);
            return View(model);
        }

        #endregion

        #region Metodos
        private void cargar_combos(int IdEmpresa)
        {

            var lst_tipo_comprobante = bus_tipo_comprobante.get_list(IdEmpresa, false);
            ViewBag.lst_tipo_comprobante = lst_tipo_comprobante;

            var lst_cobrotipo = bus_cobrotipo.get_list(false);
            ViewBag.lst_cobrotipo = lst_cobrotipo;

            var lst_movimiento = bus_movimiento.get_list(IdEmpresa, false);
            ViewBag.lst_movimiento = lst_movimiento;

            bool EsContador = Convert.ToBoolean(SessionFixed.EsContador);
            var lst_caja = bus_caja.GetList(IdEmpresa, 0, false, SessionFixed.IdUsuario, EsContador);
            ViewBag.lst_caja = lst_caja;

            var lst_tiponota = bus_tiponota.get_list(IdEmpresa, "D", false);
            ViewBag.lst_tiponota = lst_tiponota;
            
            var lst_tipo_nota_credito = bus_tiponota.get_list(IdEmpresa, "C", false);
            ViewBag.lst_tipo_nota_credito = lst_tipo_nota_credito;

            var lst_tipo_nota_pago_anticipado = bus_tiponota.get_list(IdEmpresa, "C", false);
            ViewBag.lst_tipo_nota_pago_anticipado = lst_tipo_nota_pago_anticipado;
        }

        #endregion
    }
}