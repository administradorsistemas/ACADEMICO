using Core.Bus.Academico;
using Core.Bus.CuentasPorCobrar;
using Core.Bus.Facturacion;
using Core.Bus.General;
using Core.Info.Academico;
using Core.Info.CuentasPorCobrar;
using Core.Info.Facturacion;
using Core.Info.General;
using Core.Info.Helps;
using Core.Web.Helps;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Core.Web.Areas.Academico.Controllers
{
    public class ProcesarAdmisionController : Controller
    {
        #region Variables
        aca_SocioEconomico_Bus bus_socio_economico = new aca_SocioEconomico_Bus();
        aca_Matricula_Bus bus_matricula = new aca_Matricula_Bus();
        aca_Matricula_List Lista_Matricula = new aca_Matricula_List();
        aca_CatalogoFicha_Bus bus_catalogo_socioeconomico = new aca_CatalogoFicha_Bus();
        aca_Menu_x_seg_usuario_Bus bus_permisos = new aca_Menu_x_seg_usuario_Bus();
        tb_Religion_Bus bus_religion = new tb_Religion_Bus();
        tb_GrupoEtnico_Bus bus_grupoetnico = new tb_GrupoEtnico_Bus();
        tb_Catalogo_Bus bus_catalogo = new tb_Catalogo_Bus();
        aca_Catalogo_Bus bus_aca_catalogo = new aca_Catalogo_Bus();
        aca_Admision_Bus bus_admision = new aca_Admision_Bus();
        aca_SocioEconomico_Bus bus_socioeconomico = new aca_SocioEconomico_Bus();
        tb_profesion_Bus bus_profesion = new tb_profesion_Bus();
        aca_CatalogoFicha_Bus bus_catalogo_ficha = new aca_CatalogoFicha_Bus();
        tb_persona_Bus bus_persona = new tb_persona_Bus();
        aca_AnioLectivo_Bus bus_anio = new aca_AnioLectivo_Bus();
        aca_Sede_Bus bus_sede = new aca_Sede_Bus();
        aca_Jornada_Bus bus_jornada = new aca_Jornada_Bus();
        aca_NivelAcademico_Bus bus_nivel = new aca_NivelAcademico_Bus();
        aca_Curso_Bus bus_curso = new aca_Curso_Bus();
        tb_pais_Bus bus_pais = new tb_pais_Bus();
        tb_region_Bus bus_region = new tb_region_Bus();
        tb_provincia_Bus bus_provincia = new tb_provincia_Bus();
        tb_ciudad_Bus bus_ciudad = new tb_ciudad_Bus();
        tb_parroquia_Bus bus_parroquia = new tb_parroquia_Bus();
        aca_Alumno_Bus bus_alumno = new aca_Alumno_Bus();
        aca_Familia_Bus bus_familia = new aca_Familia_Bus();
        aca_ProcesarAdmision_List Lista_ProcesarAdmision = new aca_ProcesarAdmision_List();
        fa_cliente_tipo_Bus bus_clientetipo = new fa_cliente_tipo_Bus();
        fa_TerminoPago_Bus bus_termino_pago = new fa_TerminoPago_Bus();
        aca_PreMatricula_Bus bus_prematricula = new aca_PreMatricula_Bus();
        aca_MecanismoDePago_Bus bus_mecanismo = new aca_MecanismoDePago_Bus();
        aca_AnioLectivo_Curso_Documento_List Lista_DocumentosMatricula = new aca_AnioLectivo_Curso_Documento_List();
        aca_PreMatricula_Rubro_List ListaPreMatriculaRubro = new aca_PreMatricula_Rubro_List();
        aca_AnioLectivo_Curso_Documento_Bus bus_curso_documento = new aca_AnioLectivo_Curso_Documento_Bus();
        aca_PreMatricula_Rubro_Bus bus_prematricula_rubro = new aca_PreMatricula_Rubro_Bus();
        aca_AnioLectivo_Periodo_Bus bus_anio_periodo = new aca_AnioLectivo_Periodo_Bus();
        fa_PuntoVta_Bus bus_punto_venta = new fa_PuntoVta_Bus();
        fa_Vendedor_Bus bus_vendedor = new fa_Vendedor_Bus();
        fa_formaPago_Bus bus_forma_pago = new fa_formaPago_Bus();
        fa_cliente_Bus bus_cliente = new fa_cliente_Bus();
        aca_AlumnoDocumento_Bus bus_alumno_documento = new aca_AlumnoDocumento_Bus();
        aca_Plantilla_Bus bus_plantilla = new aca_Plantilla_Bus();
        aca_Plantilla_Rubro_Bus bus_plantilla_rubro = new aca_Plantilla_Rubro_Bus();
        aca_AlumnoDocumentoAdmision_List Lista_DocAdmision = new aca_AlumnoDocumentoAdmision_List();
        aca_parametro_Bus bus_parametro = new aca_parametro_Bus();
        tb_ColaCorreo_Bus bus_cola_correo = new tb_ColaCorreo_Bus();
        aca_PermisoMatricula_Bus bus_permiso = new aca_PermisoMatricula_Bus();
        fa_notaCreDeb_Bus bus_notaDebCre = new fa_notaCreDeb_Bus();
        cxc_cobro_Bus bus_cobro = new cxc_cobro_Bus();
        aca_MatriculaCondicional_Bus bus_matricula_condicional = new aca_MatriculaCondicional_Bus();
        aca_Matricula_PorParalelo_List List_Matricula_PorParalelo = new aca_Matricula_PorParalelo_List();
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

            var IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var info_anio = bus_anio.GetInfo_AnioAdmision(IdEmpresa);
      
            var model = new aca_Admision_Info
            {
                IdEmpresa = IdEmpresa,
                IdAnio = (info_anio == null ? 0 : info_anio.IdAnio),
                IdSede = Convert.ToInt32(SessionFixed.IdSede),
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession)
            };

            List<aca_Admision_Info> lst_admisiones = bus_admision.GetList_Academico(model.IdEmpresa, model.IdSede, model.IdAnio);
            lst_admisiones.ForEach(q => q.IdUsuarioSesion = SessionFixed.IdUsuario);
            Lista_ProcesarAdmision.set_list(lst_admisiones, Convert.ToDecimal(SessionFixed.IdTransaccionSession));
            cargar_combos(model);

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(aca_Admision_Info model)
        {
            List<aca_Admision_Info> lst_admisiones = bus_admision.GetList_Academico(model.IdEmpresa, model.IdSede, model.IdAnio);
            lst_admisiones.ForEach(q=> q.IdUsuarioSesion = SessionFixed.IdUsuario);
            Lista_ProcesarAdmision.set_list(lst_admisiones, Convert.ToDecimal(SessionFixed.IdTransaccionSession));
            cargar_combos(model);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_ProcesarAdmision(bool Nuevo = false, bool Modificar = false, bool Anular = false)
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            List<aca_Admision_Info> model = Lista_ProcesarAdmision.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return PartialView("_GridViewPartial_ProcesarAdmision", model);
        }
        #endregion

        #region IndexPrematricula
        public ActionResult IndexPrematricula()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            var IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var info_anio = bus_anio.GetInfo_AnioAdmision(IdEmpresa);

            var model = new aca_Admision_Info
            {
                IdEmpresa = IdEmpresa,
                IdAnio = (info_anio == null ? 0 : info_anio.IdAnio),
                IdSede = Convert.ToInt32(SessionFixed.IdSede),
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession)
            };

            List<aca_Admision_Info> lst_admisiones = bus_admision.GetList_PreMatriculaAcademico(model.IdEmpresa, model.IdSede, model.IdAnio);
            Lista_ProcesarAdmision.set_list(lst_admisiones, Convert.ToDecimal(SessionFixed.IdTransaccionSession));
            cargar_combos(model);

            return View(model);
        }

        [HttpPost]
        public ActionResult IndexPrematricula(aca_Admision_Info model)
        {
            List<aca_Admision_Info> lst_admisiones = bus_admision.GetList_PreMatriculaAcademico(model.IdEmpresa, model.IdSede, model.IdAnio);
            Lista_ProcesarAdmision.set_list(lst_admisiones, Convert.ToDecimal(SessionFixed.IdTransaccionSession));
            cargar_combos(model);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_Prematricula()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            List<aca_Admision_Info> model = Lista_ProcesarAdmision.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return PartialView("_GridViewPartial_Prematricula", model);
        }
        #endregion

        #region Combos
        public ActionResult ComboBoxPartial_Pais()
        {
            return PartialView("_ComboBoxPartial_Pais", new aca_Admision_Info());
        }
        public ActionResult ComboBoxPartial_Region()
        {
            string IdPais = (Request.Params["IdPais"] != null) ? Convert.ToString(Request.Params["IdPais"]) : "";
            return PartialView("_ComboBoxPartial_Region", new aca_Admision_Info { IdPais_Aspirante = IdPais });
        }
        public ActionResult ComboBoxPartial_Provincia()
        {
            string IdPais = (Request.Params["IdPais"] != null) ? Convert.ToString(Request.Params["IdPais"]) : "";
            string Cod_Region = (Request.Params["Cod_Region"] != null) ? Convert.ToString(Request.Params["Cod_Region"]) : "";
            return PartialView("_ComboBoxPartial_Provincia", new aca_Admision_Info { IdPais_Aspirante = IdPais, Cod_Region_Aspirante = Cod_Region });
        }
        public ActionResult ComboBoxPartial_Ciudad()
        {
            string IdPais = (Request.Params["IdPais"] != null) ? Convert.ToString(Request.Params["IdPais"]) : "";
            string Cod_Region = (Request.Params["Cod_Region"] != null) ? Convert.ToString(Request.Params["Cod_Region"]) : "";
            string IdProvincia = (Request.Params["IdProvincia"] != null) ? Convert.ToString(Request.Params["IdProvincia"]) : "";
            return PartialView("_ComboBoxPartial_Ciudad", new aca_Admision_Info { IdPais_Aspirante = IdPais, Cod_Region_Aspirante = Cod_Region, IdProvincia_Aspirante = IdProvincia });
        }
        public ActionResult ComboBoxPartial_Parroquia()
        {
            string IdPais = (Request.Params["IdPais"] != null) ? Convert.ToString(Request.Params["IdPais"]) : "";
            string Cod_Region = (Request.Params["Cod_Region"] != null) ? Convert.ToString(Request.Params["Cod_Region"]) : "";
            string IdProvincia = (Request.Params["IdProvincia"] != null) ? Convert.ToString(Request.Params["IdProvincia"]) : "";
            string IdCiudad = (Request.Params["IdCiudad"] != null) ? Convert.ToString(Request.Params["IdCiudad"]) : "";
            return PartialView("_ComboBoxPartial_Parroquia", new aca_Admision_Info { IdPais_Aspirante = IdPais, Cod_Region_Aspirante = Cod_Region, IdProvincia_Aspirante = IdProvincia, IdCiudad_Aspirante = IdCiudad });
        }

        public ActionResult cmb_parroquia_padre()
        {
            string IdCiudadPadre = (Request.Params["fx_IdCiudad_padre_fact"] != null) ? Request.Params["fx_IdCiudad_padre_fact"].ToString() : "";
            return PartialView("_cmb_parroquia_padre", new aca_Admision_Info { IdCiudad_Padre_Fact = IdCiudadPadre });
        }
        public ActionResult cmb_parroquia_madre()
        {
            string IdCiudadMadre = (Request.Params["fx_IdCiudad_madre_fact"] != null) ? Request.Params["fx_IdCiudad_madre_fact"].ToString() : "";
            return PartialView("_cmb_parroquia_madre", new aca_Admision_Info { IdCiudad_Madre_Fact = IdCiudadMadre });
        }
        public ActionResult cmb_parroquia_representante()
        {
            string IdCiudadMadre = (Request.Params["fx_IdCiudad_representante_fact"] != null) ? Request.Params["fx_IdCiudad_representante_fact"].ToString() : "";
            return PartialView("_cmb_parroquia_representante", new aca_Admision_Info { IdCiudad_Representante_Fact = IdCiudadMadre });
        }
        #endregion
        #region CombosPadre
        public ActionResult ComboBoxPartial_Pais_padre()
        {
            return PartialView("_ComboBoxPartial_Pais_padre", new aca_Admision_Info());
        }
        public ActionResult ComboBoxPartial_Region_padre()
        {
            string IdPais = (Request.Params["IdPais_padre"] != null) ? Convert.ToString(Request.Params["IdPais_padre"]) : "";
            return PartialView("_ComboBoxPartial_Region_padre", new aca_Admision_Info { IdPais_Padre = IdPais });
        }
        public ActionResult ComboBoxPartial_Provincia_padre()
        {
            string IdPais = (Request.Params["IdPais_padre"] != null) ? Convert.ToString(Request.Params["IdPais_padre"]) : "";
            string Cod_Region = (Request.Params["Cod_Region_padre"] != null) ? Convert.ToString(Request.Params["Cod_Region_padre"]) : "";
            return PartialView("_ComboBoxPartial_Provincia_padre", new aca_Admision_Info { IdPais_Padre = IdPais, Cod_Region_Padre = Cod_Region });
        }
        public ActionResult ComboBoxPartial_Ciudad_padre()
        {
            string IdPais = (Request.Params["IdPais_padre"] != null) ? Convert.ToString(Request.Params["IdPais_padre"]) : "";
            string Cod_Region = (Request.Params["Cod_Region_padre"] != null) ? Convert.ToString(Request.Params["Cod_Region_padre"]) : "";
            string IdProvincia = (Request.Params["IdProvincia_padre"] != null) ? Convert.ToString(Request.Params["IdProvincia_padre"]) : "";
            return PartialView("_ComboBoxPartial_Ciudad_padre", new aca_Admision_Info { IdPais_Padre = IdPais, Cod_Region_Padre = Cod_Region, IdProvincia_Aspirante = IdProvincia });
        }
        public ActionResult ComboBoxPartial_Parroquia_padre()
        {
            string IdPais = (Request.Params["IdPais_padre"] != null) ? Convert.ToString(Request.Params["IdPais_padre"]) : "";
            string Cod_Region = (Request.Params["Cod_Region_padre"] != null) ? Convert.ToString(Request.Params["Cod_Region_padre"]) : "";
            string IdProvincia = (Request.Params["IdProvincia_padre"] != null) ? Convert.ToString(Request.Params["IdProvincia_padre"]) : "";
            string IdCiudad = (Request.Params["IdCiudad_padre"] != null) ? Convert.ToString(Request.Params["IdCiudad_padre"]) : "";
            return PartialView("_ComboBoxPartial_Parroquia_padre", new aca_Admision_Info { IdPais_Padre = IdPais, Cod_Region_Padre = Cod_Region, IdProvincia_Padre = IdProvincia, IdCiudad_Padre = IdCiudad });
        }
        #endregion
        #region CombosMadre
        public ActionResult ComboBoxPartial_Pais_madre()
        {
            return PartialView("_ComboBoxPartial_Pais_madre", new aca_Admision_Info());
        }
        public ActionResult ComboBoxPartial_Region_madre()
        {
            string IdPais = (Request.Params["IdPais_madre"] != null) ? Convert.ToString(Request.Params["IdPais_madre"]) : "";
            return PartialView("_ComboBoxPartial_Region_madre", new aca_Admision_Info { IdPais_Madre = IdPais });
        }
        public ActionResult ComboBoxPartial_Provincia_madre()
        {
            string IdPais = (Request.Params["IdPais_madre"] != null) ? Convert.ToString(Request.Params["IdPais_madre"]) : "";
            string Cod_Region = (Request.Params["Cod_Region_madre"] != null) ? Convert.ToString(Request.Params["Cod_Region_madre"]) : "";
            return PartialView("_ComboBoxPartial_Provincia_madre", new aca_Admision_Info { IdPais_Madre = IdPais, Cod_Region_Madre = Cod_Region });
        }
        public ActionResult ComboBoxPartial_Ciudad_madre()
        {
            string IdPais = (Request.Params["IdPais_madre"] != null) ? Convert.ToString(Request.Params["IdPais_madre"]) : "";
            string Cod_Region = (Request.Params["Cod_Region_madre"] != null) ? Convert.ToString(Request.Params["Cod_Region_madre"]) : "";
            string IdProvincia = (Request.Params["IdProvincia_madre"] != null) ? Convert.ToString(Request.Params["IdProvincia_madre"]) : "";
            return PartialView("_ComboBoxPartial_Ciudad_madre", new aca_Admision_Info { IdPais_Madre = IdPais, Cod_Region_Madre = Cod_Region, IdProvincia_Madre = IdProvincia });
        }
        public ActionResult ComboBoxPartial_Parroquia_madre()
        {
            string IdPais = (Request.Params["IdPais_madre"] != null) ? Convert.ToString(Request.Params["IdPais_madre"]) : "";
            string Cod_Region = (Request.Params["Cod_Region_madre"] != null) ? Convert.ToString(Request.Params["Cod_Region_madre"]) : "";
            string IdProvincia = (Request.Params["IdProvincia_madre"] != null) ? Convert.ToString(Request.Params["IdProvincia_madre"]) : "";
            string IdCiudad = (Request.Params["IdCiudad_madre"] != null) ? Convert.ToString(Request.Params["IdCiudad_madre"]) : "";
            return PartialView("_ComboBoxPartial_Parroquia_madre", new aca_Admision_Info { IdPais_Madre = IdPais, Cod_Region_Madre = Cod_Region, IdProvincia_Madre = IdProvincia, IdCiudad_Madre = IdCiudad });
        }
        #endregion
        #region CombosMadre
        public ActionResult ComboBoxPartial_Pais_representante()
        {
            return PartialView("_ComboBoxPartial_Pais_representante", new aca_Admision_Info());
        }
        public ActionResult ComboBoxPartial_Region_representante()
        {
            string IdPais = (Request.Params["IdPais_representante"] != null) ? Convert.ToString(Request.Params["IdPais_representante"]) : "";
            return PartialView("_ComboBoxPartial_Region_representante", new aca_Admision_Info { IdPais_Representante = IdPais });
        }
        public ActionResult ComboBoxPartial_Provincia_representante()
        {
            string IdPais = (Request.Params["IdPais_representante"] != null) ? Convert.ToString(Request.Params["IdPais_representante"]) : "";
            string Cod_Region = (Request.Params["Cod_Region_representante"] != null) ? Convert.ToString(Request.Params["Cod_Region_representante"]) : "";
            return PartialView("_ComboBoxPartial_Provincia_representante", new aca_Admision_Info { IdPais_Representante = IdPais, Cod_Region_Representante = Cod_Region });
        }
        public ActionResult ComboBoxPartial_Ciudad_representante()
        {
            string IdPais = (Request.Params["IdPais_representante"] != null) ? Convert.ToString(Request.Params["IdPais_representante"]) : "";
            string Cod_Region = (Request.Params["Cod_Region_representante"] != null) ? Convert.ToString(Request.Params["Cod_Region_representante"]) : "";
            string IdProvincia = (Request.Params["IdProvincia_representante"] != null) ? Convert.ToString(Request.Params["IdProvincia_representante"]) : "";
            return PartialView("_ComboBoxPartial_Ciudad_representante", new aca_Admision_Info { IdPais_Representante = IdPais, Cod_Region_Representante = Cod_Region, IdProvincia_Representante = IdProvincia });
        }
        public ActionResult ComboBoxPartial_Parroquia_representante()
        {
            string IdPais = (Request.Params["IdPais_representante"] != null) ? Convert.ToString(Request.Params["IdPais_representante"]) : "";
            string Cod_Region = (Request.Params["Cod_Region_representante"] != null) ? Convert.ToString(Request.Params["Cod_Region_representante"]) : "";
            string IdProvincia = (Request.Params["IdProvincia_representante"] != null) ? Convert.ToString(Request.Params["IdProvincia_representante"]) : "";
            string IdCiudad = (Request.Params["IdCiudad_representante"] != null) ? Convert.ToString(Request.Params["IdCiudad_representante"]) : "";
            return PartialView("_ComboBoxPartial_Parroquia_representante", new aca_Admision_Info { IdPais_Representante = IdPais, Cod_Region_Representante = Cod_Region, IdProvincia_Representante = IdProvincia, IdCiudad_Representante = IdCiudad });
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
            return bus_persona.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), cl_enumeradores.eTipoPersona.ALUMNO_MATRICULA.ToString());
        }
        public tb_persona_Info get_info_bajo_demanda_alumno(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_persona.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), cl_enumeradores.eTipoPersona.ALUMNO_MATRICULA.ToString());
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
        public ActionResult Cmb_PreMatriculaEmpleado()
        {
            int IdEmpresa_rol = (Request.Params["IdEmpresa_rol"] != null) ? int.Parse(Request.Params["IdEmpresa_rol"]) : 0;
            return PartialView("_CmbEmpleado", new aca_Matricula_Info { IdEmpresa_rol = IdEmpresa_rol });
        }
        #endregion
        #region Grid documentos
        [ValidateInput(false)]
        public ActionResult GridViewPartial_Documentos()
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

            return PartialView("_GridViewPartial_Documentos", model);
        }
        #endregion
        #region Grid documentos admision
        [ValidateInput(false)]
        public ActionResult GridViewPartial_DocumentosAdmision()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            List<aca_AlumnoDocumento_Info> model = Lista_DocAdmision.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return PartialView("_GridViewPartial_DocumentosAdmision", model);
        }
        #endregion

        #region GridDetalle alumnos paralelo
        [ValidateInput(false)]
        public ActionResult GridViewPartial_AlumnosPorParalelo()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;

            List<aca_Matricula_Info> model = List_Matricula_PorParalelo.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_AlumnosPorParalelo", model);
        }
        #endregion
        #region DetallePlantilla (nuevo)
        [ValidateInput(false)]
        public ActionResult GridViewPartial_DetallePlantilla()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;

            List<aca_PreMatricula_Rubro_Info> model = ListaPreMatriculaRubro.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
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
            }
            else
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
        [ValidateInput(false)]
        public ActionResult GridViewPartial_PreMatriculaRubro()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            List<aca_PreMatricula_Rubro_Info> model = ListaPreMatriculaRubro.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_PreMatriculaRubro", model);
        }
        #region Acciones
        public ActionResult Consultar(int IdEmpresa = 0, decimal IdAdmision = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            aca_Admision_Info model = bus_admision.GetInfo(IdEmpresa, IdAdmision);
            model.CodCatalogoCONADIS_Aspirante = model.CodCatalogoCONADIS_Aspirante == null ? "" : model.CodCatalogoCONADIS_Aspirante;
            model.CodCatalogoCONADIS_Madre = model.CodCatalogoCONADIS_Madre == null ? "" : model.CodCatalogoCONADIS_Madre;
            model.CodCatalogoCONADIS_Padre = model.CodCatalogoCONADIS_Padre == null ? "" : model.CodCatalogoCONADIS_Padre;
            model.CodCatalogoCONADIS_Representante = model.CodCatalogoCONADIS_Representante == null ? "" : model.CodCatalogoCONADIS_Representante;

            if (model == null)
                return RedirectToAction("Index");

            //if (Exito)
            //    ViewBag.MensajeSuccess = MensajeSuccess;

            cargar_combos(model);
            return View(model);
        }

        public ActionResult Modificar(int IdEmpresa = 0, decimal IdAdmision = 0, bool Exito=false)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            aca_Admision_Info model = bus_admision.GetInfo(IdEmpresa, IdAdmision);
            if (model == null)
                return RedirectToAction("Index");

            #region Validar Deuda y Matricula
            var IdCatalogoPERNEG_Negar = Convert.ToInt32(cl_enumeradores.eCatalogoPermisoMatricula.NEGAR);
            var IdCatalogoPERNEG_Permitir = Convert.ToInt32(cl_enumeradores.eCatalogoPermisoMatricula.PERMITIR);
            var msgInfo = "";
            var mensaje = "";
            var info_alumno = bus_alumno.get_info_x_num_cedula(IdEmpresa, model.CedulaRuc_Aspirante);
            var IdAlumno = info_alumno == null ? 0 : info_alumno.IdAlumno;
            List<fa_notaCreDeb_Info> lst_CreditoAlumno = bus_notaDebCre.get_list_credito_favor(model.IdEmpresa, IdAlumno);


            if (lst_CreditoAlumno.Sum(q => q.sc_saldo) > 0)
            {
                var Saldo = Math.Round(lst_CreditoAlumno.Sum(q => Convert.ToDouble(q.sc_saldo)), 2, MidpointRounding.AwayFromZero).ToString("C2");
                msgInfo += "El estudiante tiene un saldo a favor: " + Saldo + " - ";
            }

            var ObsMatriculaCondicional = "";
            List<aca_MatriculaCondicional_Info> lst_MatriculaCondicional = bus_matricula_condicional.GetList_Matricula(model.IdEmpresa, model.IdAnio, IdAlumno);

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
                msgInfo += "El estudiante tiene matrícula condicional. Observación: " + ObsMatriculaCondicional;
            }
            ViewBag.mensajeInfo = msgInfo == "" ? null : msgInfo;

            if (IdAlumno != 0)
            {
                var info_matricula = bus_matricula.GetInfo_ExisteMatricula(model.IdEmpresa, model.IdAnio, model.IdAlumno);
                if (info_matricula!=null)
                {
                    mensaje += "El estudiante ya se encuentra matriculado - ";
                }
                var PermitirMatricula = bus_permiso.GetInfo_ByMatricula(model.IdEmpresa, model.IdAnio, IdAlumno, IdCatalogoPERNEG_Permitir);

                if (PermitirMatricula != null && PermitirMatricula.IdPermiso != 0)
                {

                }
                else
                {
                    var NegarMatricula = bus_permiso.GetInfo_ByMatricula(model.IdEmpresa, model.IdAnio, IdAlumno, IdCatalogoPERNEG_Negar);
                    if (NegarMatricula != null)
                    {
                        var IdUsuario = (string.IsNullOrEmpty(NegarMatricula.IdUsuarioModificacion) ? NegarMatricula.IdUsuarioCreacion : NegarMatricula.IdUsuarioCreacion);
                        mensaje += "El estudiante tiene negación de matrícula. Observación: " + NegarMatricula.Observacion + " , usuario: " + IdUsuario + " - ";
                    }

                    List<cxc_cobro_Info> lst_DeudaAlumno = bus_cobro.get_list_deuda(model.IdEmpresa, IdAlumno);

                    if (lst_DeudaAlumno.Sum(q => q.cr_saldo) > 0)
                    {
                        var Saldo = Math.Round(lst_DeudaAlumno.Sum(q => q.cr_saldo), 2, MidpointRounding.AwayFromZero).ToString("C2");
                        mensaje += "El estudiante tiene saldo pendiente: " + Saldo + " - ";
                    }
                }
                ViewBag.mensaje = mensaje == "" ? null : mensaje;
            }
            else
            {
                ViewBag.mensaje = mensaje == "" ? null : mensaje;
            }
            #endregion

            if (model.CedulaRuc_Representante == model.CedulaRuc_Padre)
            {
                model.EsRepresentante_padre = 1;
                model.Representante = "P";
            }
            else if (model.CedulaRuc_Representante == model.CedulaRuc_Madre)
            {
                model.EsRepresentante_madre = 1;
                model.Representante = "M";
            }
            else
            {
                model.EsRepresentante_otro = 1;
                model.Representante = "O";
            }

            model.TienePadre = string.IsNullOrEmpty(model.CedulaRuc_Padre) ? 0 : 1;
            model.TieneMadre = string.IsNullOrEmpty(model.CedulaRuc_Madre) ? 0 : 1;

            var info_ExistePersonaAspirante = bus_persona.get_info_x_num_cedula(model.CedulaRuc_Aspirante);
            var info_ExistePersonaPadre = bus_persona.get_info_x_num_cedula(model.CedulaRuc_Padre);
            var info_ExistePersonaMadre = bus_persona.get_info_x_num_cedula(model.CedulaRuc_Madre);
            var info_ExistePersonaRepresentante = bus_persona.get_info_x_num_cedula(model.CedulaRuc_Representante);
            var info_ExisteAlumno = bus_alumno.get_info_x_num_cedula(model.IdEmpresa, model.CedulaRuc_Aspirante);

            model.IdAlumno = (info_ExisteAlumno == null ? 0 : info_ExisteAlumno.IdAlumno);
            model.IdPersona_Aspirante = (info_ExistePersonaAspirante == null ? 0 : info_ExistePersonaAspirante.IdPersona);
            model.IdPersona_Madre = (info_ExistePersonaMadre == null ? 0 : info_ExistePersonaMadre.IdPersona);
            model.IdPersona_Padre = (info_ExistePersonaPadre == null ? 0 : info_ExistePersonaPadre.IdPersona);
            model.IdPersona_Representante = (info_ExistePersonaRepresentante == null ? 0 : info_ExistePersonaRepresentante.IdPersona);

            model.CodCatalogoCONADIS_Aspirante = (model.CodCatalogoCONADIS_Aspirante == null ? "" : model.CodCatalogoCONADIS_Aspirante);
            model.CodCatalogoCONADIS_Padre = (model.CodCatalogoCONADIS_Padre == null ? "" : model.CodCatalogoCONADIS_Padre);
            model.CodCatalogoCONADIS_Madre = (model.CodCatalogoCONADIS_Madre == null ? "" : model.CodCatalogoCONADIS_Madre);
            model.CodCatalogoCONADIS_Representante = (model.CodCatalogoCONADIS_Representante == null ? "" : model.CodCatalogoCONADIS_Representante);

            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession);
            model.Fecha = DateTime.Now.Date;
            model.IdSucursal = bus_sede.GetInfo(model.IdEmpresa, model.IdSede).IdSucursal;
            model.IdCatalogo_FormaPago = "CRE";
            model.IdEmpresa_rol = 0;
            model.info_valido_aspirante = true;
            model.info_valido_madre = true;
            model.info_valido_padre = true;
            model.IdComboCurso = model.IdEmpresa.ToString("0000") + model.IdAnio.ToString("0000") + model.IdSede.ToString("0000") + model.IdNivel.ToString("0000") + model.IdJornada.ToString("0000") + model.IdCurso.ToString("0000");

            model.IdMecanismo = 1;
            model.IdMecanismoDet = 1;
            //IdEmpresa = Convert.ToInt32(model.IdComboCurso.Substring(0, 4));
            //IdAnio = Convert.ToInt32(model.IdComboCurso.Substring(4, 4));
            //int IdSede = Convert.ToInt32(IdComboCurso.Substring(8, 4));
            //int IdNivel = Convert.ToInt32(IdComboCurso.Substring(12, 4));
            //int IdJornada = Convert.ToInt32(IdComboCurso.Substring(16, 4));
            //int IdCurso = Convert.ToInt32(IdComboCurso.Substring(20, 4));

            model.IdCatalogoESTPREMAT = Convert.ToInt32(cl_enumeradores.eCatalogoAcademicoMatricula.REGISTRADO);
            model.lst_documentos = new List<aca_AnioLectivo_Curso_Documento_Info>();
            var lst_doc_curso = bus_curso_documento.GetList_Matricula(model.IdEmpresa, model.IdSede, model.IdAnio, model.IdNivel, model.IdJornada, model.IdCurso);
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
                    model.lst_documentos.Add(item);
                }
            }

            Lista_DocumentosMatricula.set_list(model.lst_documentos, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            #region Documentos
            var lst_documentos = DocumentosAdmision(model.IdEmpresa, model.IdAdmision, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            Lista_DocAdmision.set_list(lst_documentos, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            #endregion


            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            cargar_combos(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(aca_Admision_Info model)
        {
            model.IdUsuarioModificacion = SessionFixed.IdUsuario;
            var info_prematricula = new aca_PreMatricula_Info();
            var lst_rubro = ListaPreMatriculaRubro.get_list(Convert.ToDecimal(model.IdTransaccionSession));
            info_prematricula = armar_info_prematricula(model);

            //var lst_DetallePlantilla = ListaPreMatriculaRubro.get_list(Convert.ToDecimal(model.IdTransaccionSession));
            var lst_DetalleDocumentos = Lista_DocumentosMatricula.get_list(Convert.ToDecimal(model.IdTransaccionSession)).Where(q => q.seleccionado == true).ToList();
            
            var lst_alumno_documentos = new List<aca_AlumnoDocumento_Info>();
            foreach (var item in lst_DetalleDocumentos)
            {
                var existe_documento = bus_alumno_documento.GetInfo(info_prematricula.IdEmpresa, info_prematricula.IdAlumno, Convert.ToInt32(item.IdStringDoc));
                if (existe_documento == null)
                {
                    var info_doc = new aca_AlumnoDocumento_Info
                    {
                        IdEmpresa = info_prematricula.IdEmpresa,
                        IdAlumno = info_prematricula.IdAlumno,
                        IdDocumento = Convert.ToInt32(item.IdDocumento),
                        EnArchivo = true
                    };

                    lst_alumno_documentos.Add(info_doc);
                }
                else
                {
                    existe_documento.EnArchivo = true;
                    lst_alumno_documentos.Add(existe_documento);
                }

            }

            //info_prematricula.lst_PreMatriculaRubro = lst_DetallePlantilla;
            info_prematricula.lst_Documentos = lst_alumno_documentos;
            if (!validar_PreMatricula(info_prematricula, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                cargar_combos(model);
                return View(model);
            }

            if (!bus_prematricula.GuardarDB(info_prematricula))
            {
                ViewBag.mensaje = "No se ha podido guardar el registro";
                SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
                cargar_combos(model);
                return View(model);
            }

            return RedirectToAction("Index");
        }

        public ActionResult ConsultarPrematricula(int IdEmpresa = 0, decimal IdPreMatricula = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            aca_PreMatricula_Info model = bus_prematricula.GetInfo(IdEmpresa, IdPreMatricula);
            if (model == null)
                return RedirectToAction("Index");

            model.IdComboCurso = model.IdEmpresa.ToString("0000") + model.IdAnio.ToString("0000") + model.IdSede.ToString("0000") + model.IdNivel.ToString("0000") + model.IdJornada.ToString("0000") + model.IdCurso.ToString("0000");
            model.IdEmpresa_rol = (model.IdEmpresa_rol == null ? IdEmpresa : model.IdEmpresa_rol);
            model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession);
            model.lst_matricula_curso = new List<aca_Matricula_Info>();
            model.lst_matricula_curso = bus_matricula.GetList_PorCurso(model.IdEmpresa, model.IdAnio, model.IdSede, model.IdNivel, model.IdJornada, model.IdCurso, model.IdParalelo);
            List_Matricula_PorParalelo.set_list(model.lst_matricula_curso, model.IdTransaccionSession);

            model.lst_PreMatriculaRubro = new List<aca_PreMatricula_Rubro_Info>();
            model.lst_PreMatriculaRubro = bus_prematricula_rubro.GetList(model.IdEmpresa, model.IdPreMatricula);
            ListaPreMatriculaRubro.set_list(model.lst_PreMatriculaRubro, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            model.lst_alumnoDocumentos = new List<aca_AnioLectivo_Curso_Documento_Info>();
            var lst_doc_curso = bus_curso_documento.GetList_Matricula(model.IdEmpresa, model.IdSede, model.IdAnio, model.IdNivel, model.IdJornada, model.IdCurso);
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
                    model.lst_alumnoDocumentos.Add(item);
                }
            }

            Lista_DocumentosMatricula.set_list(model.lst_alumnoDocumentos, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            #region Documentos
            var lst_documentos = DocumentosAdmision(model.IdEmpresa, model.IdAdmision, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            Lista_DocAdmision.set_list(lst_documentos, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            #endregion

            return View(model);
        }
        #endregion

        #region Metodos
        private void cargar_combos(aca_Admision_Info model)
        {
            var lst_vivienda = bus_catalogo_socioeconomico.GetList_x_Tipo(Convert.ToInt32(cl_enumeradores.eTipoCatalogoSocioEconomico.VIVIENDA), false);
            ViewBag.lst_vivienda = lst_vivienda;
            var lst_tipo_vivienda = bus_catalogo_socioeconomico.GetList_x_Tipo(Convert.ToInt32(cl_enumeradores.eTipoCatalogoSocioEconomico.TIPOVIVIENDA), false);
            ViewBag.lst_tipo_vivienda = lst_tipo_vivienda;
            var lst_agua = bus_catalogo_socioeconomico.GetList_x_Tipo(Convert.ToInt32(cl_enumeradores.eTipoCatalogoSocioEconomico.AGUA), false);
            ViewBag.lst_agua = lst_agua;
            var lst_ing_institucion = bus_catalogo_socioeconomico.GetList_x_Tipo(Convert.ToInt32(cl_enumeradores.eTipoCatalogoSocioEconomico.MOTIVOING), false);
            ViewBag.lst_ing_institucion = lst_ing_institucion;
            var lst_institucion = bus_catalogo_socioeconomico.GetList_x_Tipo(Convert.ToInt32(cl_enumeradores.eTipoCatalogoSocioEconomico.INSTITUCION), false);
            ViewBag.lst_institucion = lst_institucion;
            var lst_financiamiento = bus_catalogo_socioeconomico.GetList_x_Tipo(Convert.ToInt32(cl_enumeradores.eTipoCatalogoSocioEconomico.ESTUDIOS), false);
            ViewBag.lst_financiamiento = lst_financiamiento;
            var lst_vivecon = bus_catalogo_socioeconomico.GetList_x_Tipo(Convert.ToInt32(cl_enumeradores.eTipoCatalogoSocioEconomico.VIVECON), false);
            ViewBag.lst_vivecon = lst_vivecon;

            var lst_anio = bus_anio.GetList_Matricula(model.IdEmpresa, false);

            var lst_sede = bus_sede.GetList(model.IdEmpresa, false);
            var lst_jornada = new List<aca_Jornada_Info>();
            var lst_nivel = new List<aca_NivelAcademico_Info>();
            var lst_curso = new List<aca_Curso_Info>();

            var lst_sexo = bus_catalogo.get_list(Convert.ToInt32(cl_enumeradores.eTipoCatalogoGeneral.SEXO), false);
            lst_sexo.Add(new tb_Catalogo_Info { CodCatalogo = "", ca_descripcion = "--- Seleccione ---" });

            var lst_estado_civil = bus_catalogo.get_list(Convert.ToInt32(cl_enumeradores.eTipoCatalogoGeneral.ESTCIVIL), false);
            lst_estado_civil.Add(new tb_Catalogo_Info { CodCatalogo = "", ca_descripcion = "--- Seleccione ---" });

            var lst_tipo_doc = bus_catalogo.get_list(Convert.ToInt32(cl_enumeradores.eTipoCatalogoGeneral.TIPODOC), false);
            var lst_tipo_naturaleza = bus_catalogo.get_list(Convert.ToInt32(cl_enumeradores.eTipoCatalogoGeneral.TIPONATPER), false);

            var lst_tipo_sangre = bus_catalogo.get_list(Convert.ToInt32(cl_enumeradores.eTipoCatalogoGeneral.TIPOSANGRE), false);
            lst_tipo_sangre.Add(new tb_Catalogo_Info { CodCatalogo = "", ca_descripcion = "--- Seleccione ---" });

            var lst_tipo_discapacidad = bus_catalogo.get_list(Convert.ToInt32(cl_enumeradores.eTipoCatalogoGeneral.TIPODISCAP), false);
            lst_tipo_discapacidad.Add(new tb_Catalogo_Info { CodCatalogo = "", ca_descripcion = "--- Seleccione ---" });

            var lst_instruccion = bus_catalogo_ficha.GetList_x_Tipo(Convert.ToInt32(cl_enumeradores.eTipoCatalogoSocioEconomico.INSTRUCCION), false);
            lst_instruccion.Add(new aca_CatalogoFicha_Info { IdCatalogoFicha = 0, NomCatalogoFicha = "--- Seleccione ---" });

            var lst_profesion = bus_profesion.GetList(false);
            lst_profesion.Add(new tb_profesion_Info { IdProfesion = 0, Descripcion = "--- Seleccione ---" });

            var lst_religion = bus_religion.GetList(false);
            lst_religion.Add(new tb_Religion_Info { IdReligion = 0, NomReligion = "--- Seleccione ---" });

            var lst_grupoetnico = bus_grupoetnico.GetList(false);
            lst_grupoetnico.Add(new tb_GrupoEtnico_Info { IdGrupoEtnico = 0, NomGrupoEtnico = "--- Seleccione ---" });

            var lst_parentesco = bus_aca_catalogo.GetList_x_Tipo(Convert.ToInt32(cl_enumeradores.eTipoCatalogoAcademico.PAREN), false);
            var lst_pais = bus_pais.get_list(false);
            lst_pais.Add(new tb_pais_Info { IdPais = "", Nombre = "--- Seleccione ---" });
            var lst_region = new List<tb_region_Info>();
            var lst_provincia = new List<tb_provincia_Info>();
            var lst_ciudad = new List<tb_ciudad_Info>();
            var lst_parroquia = new List<tb_parroquia_Info>();

            var lst_termino_pago = bus_termino_pago.get_list(false);
            var lst_clientetipo = bus_clientetipo.get_list(model.IdEmpresa, false);
            var lst_ciudad_factura = bus_ciudad.get_list("", false);
            var lst_parroquia_factura = new List<tb_parroquia_Info>();

            ViewBag.lst_anio = lst_anio;
            ViewBag.lst_sede = lst_sede;
            ViewBag.lst_jornada = lst_jornada;
            ViewBag.lst_nivel = lst_nivel;
            ViewBag.lst_curso = lst_curso;
            ViewBag.lst_sexo = lst_sexo;
            ViewBag.lst_estado_civil = lst_estado_civil;
            ViewBag.lst_tipo_doc = lst_tipo_doc;
            ViewBag.lst_tipo_naturaleza = lst_tipo_naturaleza;
            ViewBag.lst_tipo_sangre = lst_tipo_sangre;
            ViewBag.lst_tipo_discapacidad = lst_tipo_discapacidad;
            ViewBag.lst_profesion = lst_profesion;
            ViewBag.lst_instruccion = lst_instruccion;
            ViewBag.lst_religion = lst_religion;
            ViewBag.lst_grupoetnico = lst_grupoetnico;
            ViewBag.lst_pais = lst_pais;
            ViewBag.lst_region = lst_region;
            ViewBag.lst_provincia = lst_provincia;
            ViewBag.lst_ciudad = lst_ciudad;
            ViewBag.lst_parroquia = lst_parroquia;
            ViewBag.lst_parentesco = lst_parentesco;
            ViewBag.lst_termino_pago = lst_termino_pago;
            ViewBag.lst_clientetipo = lst_clientetipo;
            ViewBag.lst_ciudad_factura = lst_ciudad_factura;
            ViewBag.lst_parroquia_factura = lst_parroquia_factura;

            bool EsContador = Convert.ToBoolean(SessionFixed.EsContador);
            var lst_mecanismo = bus_mecanismo.GetList(model.IdEmpresa, false);
            ViewBag.lst_mecanismo = lst_mecanismo;

            var lst_ptoventa = bus_punto_venta.GetListUsuario(model.IdEmpresa, model.IdSucursal, false, SessionFixed.IdUsuario, EsContador, "FACT");
            ViewBag.lst_ptoventa = lst_ptoventa;

            var lst_vendedor = bus_vendedor.get_list(model.IdEmpresa, false);
            ViewBag.lst_vendedor = lst_vendedor;

            var lst_pago = bus_termino_pago.get_list(false);
            ViewBag.lst_pago = lst_pago;

            var lst_formapago = bus_catalogo.get_list((int)cl_enumeradores.eTipoCatalogoFact.FormaDePago, false);
            ViewBag.lst_formapago = lst_formapago;
        }
        private bool validar(aca_Admision_Info info, ref string msg)
        {
            string return_naturaleza = "";
            string return_naturaleza_padre = "";
            string return_naturaleza_madre = "";
            string return_naturaleza_representante = "";
            if (info.FotoAspirante != null)
            {
                if (info.FotoAspirante.ContentLength > 0 && info.FotoAspirante.ContentLength >= 4000000)
                {
                    msg = "Peso de archivo (foto del aspirante) no permitido";
                    return false;
                }
            }
            else
            {
                msg = "Cargue el archivo (foto del aspirante)";
                return false;
            }

            if (info.CedulaAspirante != null)
            {
                if (info.CedulaAspirante.ContentLength > 0 && info.CedulaAspirante.ContentLength >= 4000000)
                {
                    msg = "Peso de archivo (cédula del aspirante) no permitido";
                    return false;
                }
            }
            else
            {
                msg = "Cargue el archivo (cédula del aspirante)";
                return false;
            }
            if (info.CedulaRepresentante != null)
            {
                if (info.CedulaRepresentante.ContentLength > 0 && info.CedulaRepresentante.ContentLength >= 4000000)
                {
                    msg = "Peso de archivo (cédula del representante) no permitido";
                    return false;
                }
            }
            else
            {
                msg = "Cargue el archivo (cédula del representante)";
                return false;
            }
            if (info.RecordAcademicoAspirante != null)
            {
                if (info.RecordAcademicoAspirante.ContentLength > 0 && info.RecordAcademicoAspirante.ContentLength >= 4000000)
                {
                    msg = "Peso de archivo (record académico del aspirante) no permitido";
                    return false;
                }
            }
            else
            {
                msg = "Cargue el archivo (record académico del aspirante)";
                return false;
            }
            if (info.PagoAlDiaAspirante != null)
            {
                if (info.PagoAlDiaAspirante.ContentLength > 0 && info.PagoAlDiaAspirante.ContentLength >= 4000000)
                {
                    msg = "Peso de archivo (pago al día) no permitido";
                    return false;
                }
            }
            else
            {
                msg = "Cargue el archivo (pago al día)";
                return false;
            }
            if (info.CertificadoLaboral != null)
            {
                if (info.CertificadoLaboral.ContentLength > 0 && info.CertificadoLaboral.ContentLength >= 4000000)
                {
                    msg = "Peso de archivo (certificado laboral) no permitido";
                    return false;
                }
            }
            else
            {
                msg = "Cargue el archivo (certificado laboral)";
                return false;
            }

            info.NombreCompleto_Aspirante = info.Apellidos_Aspirante + ' ' + info.Nombres_Aspirante;
            info.FechaNacimiento_Aspirante = (info.FechaNacimiento_Aspirante == null ? (DateTime?)null : info.FechaNacimiento_Aspirante);
            info.FechaNacimiento_Padre = (info.FechaNacimiento_Padre == null ? (DateTime?)null : info.FechaNacimiento_Padre);
            info.FechaNacimiento_Madre = (info.FechaNacimiento_Madre == null ? (DateTime?)null : info.FechaNacimiento_Madre);

            if (string.IsNullOrEmpty(info.RazonSocial_Padre))
            {
                info.NombreCompleto_Padre = info.Apellidos_Padre + ' ' + info.Nombres_Padre;
            }
            else
            {
                info.NombreCompleto_Padre = info.RazonSocial_Padre;
            }

            if (string.IsNullOrEmpty(info.RazonSocial_Madre))
            {
                info.NombreCompleto_Madre = info.Apellidos_Madre + ' ' + info.Nombres_Madre;
            }
            else
            {
                info.NombreCompleto_Madre = info.RazonSocial_Madre;
            }

            if (!string.IsNullOrEmpty(info.IdTipoDocumento_Aspirante) && !string.IsNullOrEmpty(info.Naturaleza_Aspirante) && !string.IsNullOrEmpty(info.CedulaRuc_Aspirante))
            {
                if (cl_funciones.ValidaIdentificacion(info.IdTipoDocumento_Aspirante, info.Naturaleza_Aspirante, info.CedulaRuc_Aspirante, ref return_naturaleza))
                {
                    info.Naturaleza_Aspirante = return_naturaleza;
                    info.info_valido_aspirante = true;
                }
                else
                {
                    msg = "Número de identificación del aspirante inválida";
                    info.info_valido_aspirante = false;
                    return false;
                }
            }
            else
            {
                msg = "Complete la información del aspirante";
                info.info_valido_aspirante = false;
                return false;
            }

            if (!string.IsNullOrEmpty(info.IdTipoDocumento_Padre) && !string.IsNullOrEmpty(info.Naturaleza_Padre) && !string.IsNullOrEmpty(info.CedulaRuc_Padre))
            {
                if (cl_funciones.ValidaIdentificacion(info.IdTipoDocumento_Padre, info.Naturaleza_Padre, info.CedulaRuc_Padre, ref return_naturaleza_padre))
                {
                    info.Naturaleza_Padre = return_naturaleza_padre;
                    info.info_valido_padre = true;
                }
                else
                {
                    msg = "Número de identificación del padre inválida";
                    info.info_valido_padre = false;
                    return false;
                }
            }
            else
            {
                msg = "Complete la información del padre";
                info.info_valido_padre = false;
                return false;
            }

            if (!string.IsNullOrEmpty(info.IdTipoDocumento_Madre) && !string.IsNullOrEmpty(info.Naturaleza_Madre) && !string.IsNullOrEmpty(info.CedulaRuc_Madre))
            {
                if (cl_funciones.ValidaIdentificacion(info.IdTipoDocumento_Madre, info.Naturaleza_Madre, info.CedulaRuc_Madre, ref return_naturaleza_madre))
                {
                    info.Naturaleza_Madre = return_naturaleza_madre;
                    info.info_valido_madre = true;
                }
                else
                {
                    msg = "Número de identificación del madre inválida";
                    info.info_valido_madre = false;
                    return false;
                }
            }
            else
            {
                msg = "Complete la información de la madre";
                info.info_valido_madre = false;
                return false;
            }

            if (!string.IsNullOrEmpty(info.IdTipoDocumento_Representante) && !string.IsNullOrEmpty(info.Naturaleza_Representante) && !string.IsNullOrEmpty(info.CedulaRuc_Representante))
            {
                if (cl_funciones.ValidaIdentificacion(info.IdTipoDocumento_Representante, info.Naturaleza_Representante, info.CedulaRuc_Representante, ref return_naturaleza_representante))
                {
                    info.Naturaleza_Representante = return_naturaleza_representante;
                    info.info_valido_representante = true;
                }
                else
                {
                    msg = "Número de identificación del representante inválida";
                    info.info_valido_representante = false;
                    return false;
                }
            }
            else
            {
                msg = "Complete la información del representante";
                info.info_valido_representante = false;
                return false;
            }

            if (info.info_valido_aspirante == true && info.info_valido_padre == true && info.info_valido_madre == true && info.info_valido_representante == true)
            {
                if (info.CedulaRuc_Padre != null && info.CedulaRuc_Madre != null && (info.CedulaRuc_Padre == info.CedulaRuc_Madre))
                {
                    msg = "No se puede registrar a la misma persona como padre y madre";
                    return false;
                }
            }
            else
            {
                msg = "Complete la información solicitada";
                return false;
            }

            return true;
        }
        private bool validar_PreMatricula(aca_PreMatricula_Info info, ref string msg)
        {
            string return_naturaleza = "";
            string return_naturaleza_padre = "";
            string return_naturaleza_madre = "";
            string return_naturaleza_representante = "";

            var info_prematricula = bus_prematricula.GetInfo_PorIdAlumno(info.IdEmpresa, info.IdSede, info.IdAnio, info.IdAlumno);
            var IdPrematricula = (info_prematricula==null ? 0 : info_prematricula.IdPreMatricula);
            if (IdPrematricula!=0)
            {
                msg = "El aspirante ya tiene una prematricula para el año lectivo seleccionado";
                return false;
            }

            if (cl_funciones.ValidaIdentificacion(info.info_alumno.info_persona_alumno.IdTipoDocumento, info.info_alumno.info_persona_alumno.pe_Naturaleza, info.info_alumno.info_persona_alumno.pe_cedulaRuc, ref return_naturaleza))
            {
                info.info_alumno.info_persona_alumno.pe_Naturaleza = return_naturaleza;

                if (info.pe_nombreCompleto=="")
                {
                    msg = "Nombre Completo no válido";
                    return false;
                }
            }
            else
            {
                msg = "Número de identificación del alumno inválida";
                return false;
            }

            if (info.info_alumno.info_persona_padre.IdTipoDocumento != "" && info.info_alumno.info_persona_padre.pe_Naturaleza != "" && info.info_alumno.info_persona_padre.pe_cedulaRuc != null)
            {
                if (cl_funciones.ValidaIdentificacion(info.info_alumno.info_persona_padre.IdTipoDocumento, info.info_alumno.info_persona_padre.pe_Naturaleza, info.info_alumno.info_persona_padre.pe_cedulaRuc, ref return_naturaleza_padre))
                {
                    info.info_alumno.info_persona_padre.pe_Naturaleza = return_naturaleza_padre;
                    info.info_alumno.info_valido_padre = true;

                    if (info.info_alumno.info_persona_padre.pe_nombreCompleto == "")
                    {
                        if (info.info_alumno.info_persona_padre.IdTipoDocumento == "RUC")
                        {
                            msg = "Razón social del padre no válida";
                            return false;
                        }
                        else
                        {
                            msg = "Nombre completo del padre no válido";
                            return false;
                        }
                    }
                }
                else
                {
                    msg = "Número de identificación del padre inválida";
                    info.info_alumno.info_valido_padre = false;
                    return false;
                }
            }
            else
            {
                info.info_valido_padre = false;
            }

            if (info.info_alumno.info_persona_madre.IdTipoDocumento != "" && info.info_alumno.info_persona_madre.pe_Naturaleza != "" && info.info_alumno.info_persona_madre.pe_cedulaRuc != null)
            {
                if (cl_funciones.ValidaIdentificacion(info.info_alumno.info_persona_madre.IdTipoDocumento, info.info_alumno.info_persona_madre.pe_Naturaleza, info.info_alumno.info_persona_madre.pe_cedulaRuc, ref return_naturaleza_madre))
                {
                    info.info_alumno.info_persona_madre.pe_Naturaleza = return_naturaleza_madre;
                    info.info_alumno.info_valido_madre = true;

                    if (info.info_alumno.info_persona_madre.pe_nombreCompleto == "")
                    {
                        if (info.info_alumno.info_persona_madre.IdTipoDocumento == "RUC")
                        {
                            msg = "Razón social de la madre no válida";
                            return false;
                        }
                        else
                        {
                            msg = "Nombre completo de la madre no válido";
                            return false;
                        }
                    }
                }
                else
                {
                    msg = "Número de identificación de la madre inválida";
                    info.info_alumno.info_valido_madre = false;
                    return false;
                }
            }
            else
            {
                info.info_alumno.info_valido_madre = false;
            }

            if (info.info_alumno.info_persona_representante.IdTipoDocumento != "" && info.info_alumno.info_persona_representante.pe_Naturaleza != "" && info.info_alumno.info_persona_representante.pe_cedulaRuc != null)
            {
                if (cl_funciones.ValidaIdentificacion(info.info_alumno.info_persona_representante.IdTipoDocumento, info.info_alumno.info_persona_representante.pe_Naturaleza, info.info_alumno.info_persona_representante.pe_cedulaRuc, ref return_naturaleza_representante))
                {
                    info.info_alumno.info_persona_representante.pe_Naturaleza = return_naturaleza_representante;
                    info.info_alumno.info_valido_representante = true;

                    if (info.info_alumno.info_persona_representante.pe_nombreCompleto == "")
                    {
                        if (info.info_alumno.info_persona_representante.IdTipoDocumento == "RUC")
                        {
                            msg = "Razón social del representante no válida";
                            return false;
                        }
                        else
                        {
                            msg = "Nombre completo del representante no válido";
                            return false;
                        }
                    }
                }
                else
                {
                    msg = "Número de identificación del representante inválida";
                    info.info_alumno.info_valido_representante = false;
                    return false;
                }
            }
            else
            {
                info.info_alumno.info_valido_representante = false;
            }

            if (info.info_alumno.pe_cedulaRuc_madre == info.info_alumno.pe_cedulaRuc_padre)
            {
                msg = "No se puede registrar a la misma persona como padre y madre";
                return false;
            }

            if (info.info_alumno.SeFactura_madre ==false && info.info_alumno.SeFactura_padre== false && info.info_alumno.SeFactura_representante==false)
            {
                msg = "Debe seleccionar a una persona para facturar";
                return false;
            }
            else
            {
                if (info.info_alumno.SeFactura_madre == true && info.info_alumno.SeFactura_padre == true && info.info_alumno.SeFactura_representante == true)
                {
                    msg = "Debe seleccionar a una sola persona para facturar";
                    return false;
                }
            }

            //var info_rep_economico = bus_familia.GetInfo_Representante(info.IdEmpresa, info.IdAlumno,cl_enumeradores.eTipoRepresentante.ECON.ToString());
            //var info_rep_legal = bus_familia.GetInfo_Representante(info.IdEmpresa, info.IdAlumno, cl_enumeradores.eTipoRepresentante.LEGAL.ToString());
            //var info_cat_economico = bus_aca_catalogo.GetInfo((info_rep_economico==null ? 0 : info_rep_economico.IdCatalogoPAREN));
            //var info_cat_legal = bus_aca_catalogo.GetInfo((info_rep_legal == null ? 0 : info_rep_legal.IdCatalogoPAREN));

            //if (info.info_alumno.SeFactura_madre==true)
            //{
            //    if (info_rep_economico!= null && info_rep_economico.IdCatalogoPAREN!= info.info_alumno.IdCatalogo_madre && info_rep_economico.pe_cedulaRuc== info.info_alumno.pe_cedulaRuc_madre)
            //    {
            //        msg = "La persona registrada como representante económico ya está asignada con otro tipo de familiar - " + (info_cat_economico == null ? "" : info_cat_economico.NomCatalogo);
            //        return false;
            //    }
            //}
            //if (info.info_alumno.SeFactura_padre == true)
            //{
            //    if (info_rep_economico != null && info_rep_economico.IdCatalogoPAREN != info.info_alumno.IdCatalogo_padre && info_rep_economico.pe_cedulaRuc == info.info_alumno.pe_cedulaRuc_padre)
            //    {
            //        msg = "La persona registrada como representante económico ya está asignada con otro tipo de familiar - " + (info_cat_economico == null ? "" : info_cat_economico.NomCatalogo);
            //        return false;
            //    }
            //}
            //if (info.info_alumno.SeFactura_representante == true)
            //{
            //    if (info_rep_economico != null && info_rep_economico.IdCatalogoPAREN != info.info_alumno.IdCatalogo_representante && info_rep_economico.pe_cedulaRuc == info.info_alumno.pe_cedulaRuc_representante)
            //    {
            //        msg = "La persona registrada como representante económico ya está asignada con otro tipo de familiar - " + (info_cat_economico == null ? "" : info_cat_economico.NomCatalogo);
            //        return false;
            //    }
            //}

            //if (info.info_alumno.EsRepresentante_madre == true)
            //{
            //    if (info_rep_legal != null && info_rep_legal.IdCatalogoPAREN != info.info_alumno.IdCatalogo_madre && info_rep_legal.pe_cedulaRuc == info.info_alumno.pe_cedulaRuc_madre)
            //    {
            //        msg = "La persona registrada como representante legal ya está asignada con otro tipo de familiar - " + (info_cat_legal == null ? "" : info_cat_legal.NomCatalogo);
            //        return false;
            //    }
            //}
            //if (info.info_alumno.EsRepresentante_padre == true)
            //{
            //    if (info_rep_legal != null && info_rep_legal.IdCatalogoPAREN != info.info_alumno.IdCatalogo_padre && info_rep_legal.pe_cedulaRuc == info.info_alumno.pe_cedulaRuc_padre)
            //    {
            //        msg = "La persona registrada como representante legal ya está asignada con otro tipo de familiar - " + (info_cat_legal == null ? "" : info_cat_legal.NomCatalogo);
            //        return false;
            //    }
            //}
            //if (info.info_alumno.EsRepresentante_representante == true)
            //{
            //    if (info_rep_legal != null && info_rep_legal.IdCatalogoPAREN != info.info_alumno.IdCatalogo_representante && info_rep_legal.pe_cedulaRuc == info.info_alumno.pe_cedulaRuc_representante)
            //    {
            //        msg = "La persona registrada como representante legal ya está asignada con otro tipo de familiar - " + (info_cat_legal == null ? "" : info_cat_legal.NomCatalogo);
            //        return false;
            //    }
            //}

            if (info.IdCurso==0)
            {
                msg = "Seleccione curso";
                return false;
            }

            if (info.IdParalelo == 0)
            {
                msg = "Seleccione paralelo";
                return false;
            }

            if (info.lst_PreMatriculaRubro.Count == 0)
            {
                msg = "Seleccione plantilla, el estudiante no tiene rubros";
                return false;
            }

            var IdCatalogoPERNEG_Negar = Convert.ToInt32(cl_enumeradores.eCatalogoPermisoMatricula.NEGAR);
            var IdCatalogoPERNEG_Permitir = Convert.ToInt32(cl_enumeradores.eCatalogoPermisoMatricula.PERMITIR);
            if (info.IdAlumno!=0 )
            {
                var PermitirMatricula = bus_permiso.GetInfo_ByMatricula(info.IdEmpresa, info.IdAnio, info.IdAlumno, IdCatalogoPERNEG_Permitir);

                if (PermitirMatricula != null && PermitirMatricula.IdPermiso != 0)
                {

                }
                else
                {
                    var NegarMatricula = bus_permiso.GetInfo_ByMatricula(info.IdEmpresa, info.IdAnio, info.IdAlumno, IdCatalogoPERNEG_Negar);
                    if (NegarMatricula != null)
                    {
                        var IdUsuario = (string.IsNullOrEmpty(NegarMatricula.IdUsuarioModificacion) ? NegarMatricula.IdUsuarioCreacion : NegarMatricula.IdUsuarioCreacion);
                        msg += "El estudiante tiene negación de matrícula. Observación: " + NegarMatricula.Observacion + " , usuario: " + IdUsuario + " ";
                        return false;
                    }

                    List<cxc_cobro_Info> lst_DeudaAlumno = bus_cobro.get_list_deuda(info.IdEmpresa, info.IdAlumno);

                    if (lst_DeudaAlumno.Sum(q => q.cr_saldo) > 0)
                    {
                        var Saldo = Math.Round(lst_DeudaAlumno.Sum(q => q.cr_saldo), 2, MidpointRounding.AwayFromZero).ToString("C2");
                        msg += "El estudiante tiene saldo pendiente: " + Saldo + " ";
                        return false;
                    }
                }
            }


            return true;
        }
        private aca_PreMatricula_Info armar_info_prematricula(aca_Admision_Info model)
        {
            var info_ExistePersonaAspirante = bus_persona.get_info_x_num_cedula(model.CedulaRuc_Aspirante);
            var info_ExistePersonaPadre = bus_persona.get_info_x_num_cedula(model.CedulaRuc_Padre);
            var info_ExistePersonaMadre = bus_persona.get_info_x_num_cedula(model.CedulaRuc_Madre);
            var info_ExistePersonaRepresentante = bus_persona.get_info_x_num_cedula(model.CedulaRuc_Representante);
            var info_ExisteAlumno = bus_alumno.get_info_x_num_cedula(model.IdEmpresa, model.CedulaRuc_Aspirante);

            int IdEmpresa = Convert.ToInt32(model.IdComboCurso.Substring(0, 4));
            int IdAnio = Convert.ToInt32(model.IdComboCurso.Substring(4, 4));
            int IdSede = Convert.ToInt32(model.IdComboCurso.Substring(8, 4));
            int IdNivel = Convert.ToInt32(model.IdComboCurso.Substring(12, 4));
            int IdJornada = Convert.ToInt32(model.IdComboCurso.Substring(16, 4));
            int IdCurso = Convert.ToInt32(model.IdComboCurso.Substring(20, 4));

            if (model.Representante == "P")
            {
                model.Naturaleza_Representante = model.Naturaleza_Padre;
                model.IdTipoDocumento_Representante = model.IdTipoDocumento_Padre;
                model.CedulaRuc_Representante = model.CedulaRuc_Padre;
                model.Nombres_Representante = model.Nombres_Padre;
                model.Apellidos_Representante = model.Apellidos_Padre;
                model.NombreCompleto_Representante = (model.IdTipoDocumento_Padre == "RUC" ? model.RazonSocial_Padre : ((string.IsNullOrEmpty(model.Apellidos_Padre) && string.IsNullOrEmpty(model.Nombres_Padre) ? "" : model.Apellidos_Padre + ' ' + model.Nombres_Padre)));
                model.RazonSocial_Representante = model.RazonSocial_Padre;
                model.Direccion_Representante = model.Direccion_Padre;
                model.Telefono_Representante = model.Telefono_Padre;
                model.Celular_Representante = model.Celular_Padre;
                model.Correo_Representante = model.Correo_Padre;
                model.Sexo_Representante = (model.Sexo_Padre == "" ? null : model.Sexo_Padre);
                model.FechaNacimiento_Representante = (model.FechaNacimiento_Padre == null ? (DateTime?)null : model.FechaNacimiento_Padre);
                model.CodCatalogoCONADIS_Representante = (model.CodCatalogoCONADIS_Padre == "" ? null : model.CodCatalogoCONADIS_Padre);
                model.PorcentajeDiscapacidad_Representante = model.PorcentajeDiscapacidad_Padre;
                model.NumeroCarnetConadis_Representante = model.NumeroCarnetConadis_Padre;
                model.IdGrupoEtnico_Representante = (model.IdGrupoEtnico_Padre == 0 ? (int?)null : model.IdGrupoEtnico_Padre);
                model.IdReligion_Representante = (model.IdReligion_Padre == 0 ? (int?)null : model.IdReligion_Padre);
                model.IdEstadoCivil_Representante = (model.IdEstadoCivil_Padre == "" ? null : model.IdEstadoCivil_Padre);
                model.AsisteCentroCristiano_Representante = model.AsisteCentroCristiano_Padre;
                model.IdPais_Representante = (model.IdPais_Padre == "" ? null : model.IdPais_Padre);
                model.Cod_Region_Representante = (model.Cod_Region_Padre == "" ? null : model.Cod_Region_Padre);
                model.IdProvincia_Representante = (model.IdProvincia_Padre == "" ? null : model.IdProvincia_Padre);
                model.IdCiudad_Representante = (model.IdCiudad_Padre == "" ? null : model.IdCiudad_Padre);
                model.IdParroquia_Representante = (model.IdParroquia_Padre == "" ? null : model.IdParroquia_Padre);
                model.Sector_Representante = model.Sector_Padre;
                model.IdCatalogoPAREN_Representante = model.IdCatalogoPAREN_Padre;
                model.IdCatalogoFichaInst_Representante = (model.IdCatalogoFichaInst_Padre == 0 ? (int?)null : model.IdCatalogoFichaInst_Padre);
                model.EmpresaTrabajo_Representante = model.EmpresaTrabajo_Padre;
                model.IdProfesion_Representante = (model.IdProfesion_Padre == 0 ? (int?)null : model.IdCatalogoFichaInst_Padre);
                model.DireccionTrabajo_Representante = model.DireccionTrabajo_Padre;
                model.TelefonoTrabajo_Representante = model.TelefonoTrabajo_Padre;
                model.CargoTrabajo_Representante = model.CargoTrabajo_Padre;
                model.AniosServicio_Representante = model.AniosServicio_Padre;
                model.IngresoMensual_Representante = model.IngresoMensual_Padre;
                model.VehiculoPropio_Representante = model.VehiculoPropio_Padre;
                model.Marca_Representante = model.Marca_Padre;
                model.Modelo_Representante = model.Modelo_Padre;
                model.AnioVehiculo_Representante = model.AnioVehiculo_Padre;
                model.CasaPropia_Representante = model.CasaPropia_Padre;
                model.EstaFallecido_Representante = model.EstaFallecido_Padre;
                model.SeFactura_Representante = model.SeFactura_Padre;
            }
            else if (model.Representante == "M")
            {
                model.Naturaleza_Representante = model.Naturaleza_Madre;
                model.IdTipoDocumento_Representante = model.IdTipoDocumento_Madre;
                model.CedulaRuc_Representante = model.CedulaRuc_Madre;
                model.Nombres_Representante = model.Nombres_Madre;
                model.Apellidos_Representante = model.Apellidos_Madre;
                model.NombreCompleto_Representante = (model.IdTipoDocumento_Madre == "RUC" ? model.RazonSocial_Madre : ((string.IsNullOrEmpty(model.Apellidos_Madre) && string.IsNullOrEmpty(model.Nombres_Madre) ? "" : model.Apellidos_Madre + ' ' + model.Nombres_Madre)));
                model.RazonSocial_Representante = model.RazonSocial_Madre;
                model.Direccion_Representante = model.Direccion_Madre;
                model.Telefono_Representante = model.Telefono_Madre;
                model.Celular_Representante = model.Celular_Madre;
                model.Correo_Representante = model.Correo_Madre;
                model.Sexo_Representante = (model.Sexo_Madre == "" ? null : model.Sexo_Madre);
                model.FechaNacimiento_Representante = (model.FechaNacimiento_Madre == null ? (DateTime?)null : model.FechaNacimiento_Madre);
                model.CodCatalogoCONADIS_Representante = (model.CodCatalogoCONADIS_Madre == "" ? null : model.CodCatalogoCONADIS_Madre);
                model.PorcentajeDiscapacidad_Representante = model.PorcentajeDiscapacidad_Madre;
                model.NumeroCarnetConadis_Representante = model.NumeroCarnetConadis_Madre;
                model.IdGrupoEtnico_Representante = (model.IdGrupoEtnico_Madre == 0 ? (int?)null : model.IdGrupoEtnico_Madre);
                model.IdReligion_Representante = (model.IdReligion_Madre == 0 ? (int?)null : model.IdReligion_Madre);
                model.IdEstadoCivil_Representante = (model.IdEstadoCivil_Madre == "" ? null : model.IdEstadoCivil_Madre);
                model.AsisteCentroCristiano_Representante = model.AsisteCentroCristiano_Madre;
                model.IdPais_Representante = (model.IdPais_Madre == "" ? null : model.IdPais_Madre);
                model.Cod_Region_Representante = (model.Cod_Region_Madre == "" ? null : model.Cod_Region_Madre);
                model.IdProvincia_Representante = (model.IdProvincia_Madre == "" ? null : model.IdProvincia_Madre);
                model.IdCiudad_Representante = (model.IdCiudad_Madre == "" ? null : model.IdCiudad_Madre);
                model.IdParroquia_Representante = (model.IdParroquia_Madre == "" ? null : model.IdParroquia_Madre);
                model.Sector_Representante = model.Sector_Madre;
                model.IdCatalogoPAREN_Representante = model.IdCatalogoPAREN_Madre;
                model.IdCatalogoFichaInst_Representante = (model.IdCatalogoFichaInst_Madre == 0 ? (int?)null : model.IdCatalogoFichaInst_Madre);
                model.EmpresaTrabajo_Representante = model.EmpresaTrabajo_Madre;
                model.IdProfesion_Representante = (model.IdProfesion_Madre == 0 ? (int?)null : model.IdCatalogoFichaInst_Madre);
                model.DireccionTrabajo_Representante = model.DireccionTrabajo_Madre;
                model.TelefonoTrabajo_Representante = model.TelefonoTrabajo_Madre;
                model.CargoTrabajo_Representante = model.CargoTrabajo_Madre;
                model.AniosServicio_Representante = model.AniosServicio_Madre;
                model.IngresoMensual_Representante = model.IngresoMensual_Madre;
                model.VehiculoPropio_Representante = model.VehiculoPropio_Madre;
                model.Marca_Representante = model.Marca_Madre;
                model.Modelo_Representante = model.Modelo_Madre;
                model.AnioVehiculo_Representante = model.AnioVehiculo_Madre;
                model.CasaPropia_Representante = model.CasaPropia_Madre;
                model.EstaFallecido_Representante = model.EstaFallecido_Madre;
                model.SeFactura_Representante = model.SeFactura_Madre;
            }
            else
            {
                model.Naturaleza_Representante = model.Naturaleza_Representante;
                model.IdTipoDocumento_Representante = model.IdTipoDocumento_Representante;
                model.CedulaRuc_Representante = model.CedulaRuc_Representante;
                model.Nombres_Representante = model.Nombres_Representante;
                model.Apellidos_Representante = model.Apellidos_Representante;
                model.NombreCompleto_Representante = (model.IdTipoDocumento_Representante == "RUC" ? model.RazonSocial_Representante : ((string.IsNullOrEmpty(model.Apellidos_Representante) && string.IsNullOrEmpty(model.Nombres_Representante) ? "" :  model.Apellidos_Representante + ' ' + model.Nombres_Representante)));
                model.RazonSocial_Representante = model.RazonSocial_Representante;
                model.Direccion_Representante = model.Direccion_Representante;
                model.Telefono_Representante = model.Telefono_Representante;
                model.Celular_Representante = model.Celular_Representante;
                model.Correo_Representante = model.Correo_Representante;
                model.Sexo_Representante = (model.Sexo_Representante == "" ? null : model.Sexo_Representante);
                model.FechaNacimiento_Representante = (model.FechaNacimiento_Representante == null ? (DateTime?)null : model.FechaNacimiento_Representante);
                model.CodCatalogoCONADIS_Representante = (model.CodCatalogoCONADIS_Representante == "" ? null : model.CodCatalogoCONADIS_Representante);
                model.PorcentajeDiscapacidad_Representante = model.PorcentajeDiscapacidad_Representante;
                model.NumeroCarnetConadis_Representante = model.NumeroCarnetConadis_Representante;
                model.IdGrupoEtnico_Representante = (model.IdGrupoEtnico_Representante == 0 ? (int?)null : model.IdGrupoEtnico_Representante);
                model.IdReligion_Representante = (model.IdReligion_Representante == 0 ? (int?)null : model.IdReligion_Representante);
                model.IdEstadoCivil_Representante = (model.IdEstadoCivil_Representante == "" ? null : model.IdEstadoCivil_Representante);
                model.AsisteCentroCristiano_Representante = model.AsisteCentroCristiano_Representante;
                model.IdPais_Representante = (model.IdPais_Representante == "" ? null : model.IdPais_Representante);
                model.Cod_Region_Representante = (model.Cod_Region_Representante == "" ? null : model.Cod_Region_Representante);
                model.IdProvincia_Representante = (model.IdProvincia_Representante == "" ? null : model.IdProvincia_Representante);
                model.IdCiudad_Representante = (model.IdCiudad_Representante == "" ? null : model.IdCiudad_Representante);
                model.IdParroquia_Representante = (model.IdParroquia_Representante == "" ? null : model.IdParroquia_Representante);
                model.Sector_Representante = model.Sector_Representante;
                model.IdCatalogoPAREN_Representante = model.IdCatalogoPAREN_Representante;
                model.IdCatalogoFichaInst_Representante = (model.IdCatalogoFichaInst_Representante == 0 ? (int?)null : model.IdCatalogoFichaInst_Representante);
                model.EmpresaTrabajo_Representante = model.EmpresaTrabajo_Representante;
                model.IdProfesion_Representante = (model.IdProfesion_Representante == 0 ? (int?)null : model.IdCatalogoFichaInst_Representante);
                model.DireccionTrabajo_Representante = model.DireccionTrabajo_Representante;
                model.TelefonoTrabajo_Representante = model.TelefonoTrabajo_Representante;
                model.CargoTrabajo_Representante = model.CargoTrabajo_Representante;
                model.AniosServicio_Representante = model.AniosServicio_Representante;
                model.IngresoMensual_Representante = model.IngresoMensual_Representante;
                model.VehiculoPropio_Representante = model.VehiculoPropio_Representante;
                model.Marca_Representante = model.Marca_Representante;
                model.Modelo_Representante = model.Modelo_Representante;
                model.AnioVehiculo_Representante = model.AnioVehiculo_Representante;
                model.CasaPropia_Representante = model.CasaPropia_Representante;
                model.EstaFallecido_Representante = model.EstaFallecido_Representante;
                model.SeFactura_Representante = model.SeFactura_Representante;
            }

            var info_Alumno = new aca_Alumno_Info
            {
                IdEmpresa = model.IdEmpresa,
                IdAlumno = (info_ExisteAlumno == null ? 0 : info_ExisteAlumno.IdAlumno),
                IdPersona = (info_ExisteAlumno == null ? 0 : info_ExisteAlumno.IdPersona),
                Codigo = (info_ExisteAlumno == null ? "" : info_ExisteAlumno.Codigo),
                Estado = true,
                Correo = model.Correo_Aspirante,
                Direccion = model.Direccion_Aspirante,
                Celular = model.Celular_Aspirante,
                FechaIngreso = DateTime.Now,
                IdCatalogoESTALU = Convert.ToInt32(cl_enumeradores.eCatalogoAcademicoAlumno.PROMOVIDO),
                IdCatalogoESTMAT = Convert.ToInt32(cl_enumeradores.eCatalogoAcademicoMatricula.REGISTRADO),
                IdPais = model.IdPais_Aspirante,
                Cod_Region = model.Cod_Region_Aspirante,
                IdProvincia = model.IdProvincia_Aspirante,
                IdCiudad = model.IdCiudad_Aspirante,
                IdParroquia = model.IdParroquia_Aspirante,
                Sector = model.Sector_Aspirante,
                LugarNacimiento = model.LugarNacimiento_Aspirante,
                Dificultad_Lectura = model.Dificultad_Lectura,
                Dificultad_Escritura = model.Dificultad_Escritura,
                Dificultad_Matematicas = model.Dificultad_Matematicas,
                pe_apellido = model.Apellidos_Aspirante,
                pe_nombre = model.Nombres_Aspirante,
                pe_nombreCompleto = model.NombreCompleto_Aspirante,
                pe_Naturaleza = model.Naturaleza_Aspirante,
                NumeroCarnetConadis = model.NumeroCarnetConadis_Aspirante,
                CodCatalogoCONADIS = (model.CodCatalogoCONADIS_Aspirante == "" ? null : model.CodCatalogoCONADIS_Aspirante),
                AsisteCentroCristiano = model.AsisteCentroCristiano_Aspirante,
                pe_cedulaRuc = model.CedulaRuc_Aspirante,
                pe_sexo = model.Sexo_Aspirante,
                IdGrupoEtnico = model.IdGrupoEtnico_Aspirante,
                IdUsuario = SessionFixed.IdUsuario,
                FechaCreacion = DateTime.Now,
                IdAnio = IdAnio,
                IdSede = IdSede,
                IdJornada = IdJornada,
                IdNivel = IdNivel,
                IdCurso = IdCurso,
                IdParalelo = model.IdParalelo,
                IdSucursal = model.IdSucursal,
                IdReligion = model.IdReligion_Aspirante,
                IdTipoDocumento = model.IdTipoDocumento_Aspirante,
                IdAnioRegistro = model.IdAnio,

                AniosServicio_madre = model.AniosServicio_Madre,
                AnioVehiculo_madre = model.AnioVehiculo_Madre,
                AsisteCentroCristiano_madre = model.AsisteCentroCristiano_Madre,
                CargoTrabajo_madre = model.CargoTrabajo_Madre,
                CasaPropia_madre = model.CasaPropia_Madre,
                Celular_madre = model.Celular_Madre,
                CodCatalogoSangre_madre = null,
                IdCatalogoFichaInst_madre = model.IdCatalogoFichaInst_Madre,
                CodCatalogoSangre = model.CodCatalogoSangre_Aspirante,
                Cod_Region_madre = model.Cod_Region_Madre,
                Correo_madre = model.Correo_Madre,
                DireccionTrabajo_madre = model.DireccionTrabajo_Madre,
                Direccion_madre = model.Direccion_Madre,
                EmpresaTrabajo_madre = model.EmpresaTrabajo_Madre,
                EsRepresentante_madre = false,
                EstaFallecido_madre = model.EstaFallecido_Madre,
                IdCiudad_madre_fact = model.IdCiudad_Madre_Fact,
                IdEstadoCivil_madre = model.IdEstadoCivil_Madre,
                IdPais_madre = model.IdPais_Madre,
                CodCatalogoCONADIS_madre = (model.CodCatalogoCONADIS_Madre == "" ? null : model.CodCatalogoCONADIS_Madre),
                IdCiudad_madre = model.IdCiudad_Madre,
                IdParroquia_madre = model.IdParroquia_Madre,
                IdParroquia_madre_fact = model.IdParroquia_Madre_Fact,
                IdPersona_madre = model.IdPersona_Madre,
                IdTipoCredito_madre = model.IdTipoCredito_Padre,
                IdReligion_madre = model.IdReligion_Madre,
                IdProvincia_madre = model.IdProvincia_Madre,
                IdTipoDocumento_madre = model.IdTipoDocumento_Madre,
                Idtipo_cliente_madre = model.Idtipo_cliente_Madre ?? 0,
                IngresoMensual_madre = model.IngresoMensual_Madre,
                IdProfesion_madre = model.IdProfesion_Madre,
                Marca_madre = model.Marca_Madre,
                pe_apellido_madre = model.Apellidos_Madre,
                NumeroCarnetConadis_madre=model.NumeroCarnetConadis_Madre,
                pe_cedulaRuc_madre = model.CedulaRuc_Madre,
                pe_fechaNacimiento_madre = model.FechaNacimiento_Madre,
                pe_Naturaleza_madre=model.Naturaleza_Madre,
                pe_nombreCompleto_madre = model.NombreCompleto_Madre,
                pe_nombre_madre = model.Nombres_Madre,
                Modelo_madre = model.Modelo_Madre,
                pe_razonSocial_madre = model.RazonSocial_Madre,
                pe_sexo_madre=model.Sexo_Madre,
                pe_telfono_Contacto_madre = model.Telefono_Madre,
                PorcentajeDiscapacidad_madre = model.PorcentajeDiscapacidad_Madre,
                Sector_madre = model.Sector_Madre,
                SeFactura_madre = model.SeFactura_Madre,
                IdGrupoEtnico_madre = model.IdGrupoEtnico_Madre,
                
                TelefonoTrabajo_madre = model.TelefonoTrabajo_Madre,
                VehiculoPropio_madre = model.VehiculoPropio_Madre,
                IdCatalogo_madre = model.IdCatalogoPAREN_Madre,

                AniosServicio_padre = model.AniosServicio_Padre,
                pe_cedulaRuc_padre = model.CedulaRuc_Aspirante,
                AnioVehiculo_padre = model.AnioVehiculo_Padre,
                AsisteCentroCristiano_padre = model.AsisteCentroCristiano_Padre, 
                CargoTrabajo_padre = model.CargoTrabajo_Padre,               
                CasaPropia_padre = model.CasaPropia_Padre,
                Celular_padre = model.Celular_Padre,
                CodCatalogoSangre_padre = null,
                Cod_Region_padre = model.Cod_Region_Padre,
                DireccionTrabajo_padre = model.DireccionTrabajo_Padre,
                CodCatalogoCONADIS_padre = (model.CodCatalogoCONADIS_Padre == "" ? null : model.CodCatalogoCONADIS_Padre),
                Correo_padre = model.Correo_Padre,
                IdCiudad_padre_fact = model.IdCiudad_Madre_Fact,
                Direccion_padre = model.Direccion_Padre,                
                EmpresaTrabajo_padre = model.EmpresaTrabajo_Padre,                 
                EsRepresentante_padre = false,               
                EstaFallecido_padre = model.EstaFallecido_Padre,
                IdCatalogoFichaInst_padre = model.IdCatalogoFichaInst_Padre,
                IdCiudad_padre = model.IdCiudad_Padre,
                IdEstadoCivil_padre = model.IdEstadoCivil_Padre,
                IdPais_padre = model.IdPais_Padre,
                IdParroquia_padre = model.IdParroquia_Padre,
                IdPersona_padre = model.IdPersona_Padre,
                IdParroquia_padre_fact = model.IdParroquia_Padre_Fact,
                IdProfesion_padre = model.IdProfesion_Padre,
                IdReligion_padre = model.IdReligion_Padre,
                IdProvincia_padre = model.IdProvincia_Padre,
                IdTipoCredito_padre = model.IdTipoCredito_Padre,
                IdTipoDocumento_padre = model.IdTipoDocumento_Padre,
                Idtipo_cliente_padre = model.Idtipo_cliente_Padre ?? 0,
                IngresoMensual_padre = model.IngresoMensual_Padre,
                Marca_padre = model.Marca_Padre,
                NumeroCarnetConadis_padre=model.NumeroCarnetConadis_Padre,
                pe_apellido_padre=model.Apellidos_Padre,
                pe_fechaNacimiento_padre=model.FechaNacimiento_Padre,
                pe_Naturaleza_padre=model.Naturaleza_Padre,
                pe_nombreCompleto_padre=model.NombreCompleto_Padre,
                pe_nombre_padre=model.Nombres_Padre,
                pe_razonSocial_padre = model.RazonSocial_Padre,
                pe_sexo_padre=model.Sexo_Padre,
                pe_telfono_Contacto_padre=model.Telefono_Padre,
                PorcentajeDiscapacidad_padre=model.PorcentajeDiscapacidad_Padre,
                Sector_padre=model.Sector_Padre,
                SeFactura_padre=model.SeFactura_Padre,
                TelefonoTrabajo_padre=model.TelefonoTrabajo_Padre,
                VehiculoPropio_padre = model.VehiculoPropio_Padre,
                IdCatalogo_padre = model.IdCatalogoPAREN_Padre,
                Modelo_padre=model.Modelo_Padre,
                IdGrupoEtnico_padre = model.IdGrupoEtnico_Padre,

                AniosServicio_representante = model.AniosServicio_Representante,
                pe_cedulaRuc_representante = model.CedulaRuc_Representante,
                AnioVehiculo_representante = model.AnioVehiculo_Representante,
                AsisteCentroCristiano_representante = model.AsisteCentroCristiano_Representante,
                CargoTrabajo_representante = model.CargoTrabajo_Representante,
                CasaPropia_representante = model.CasaPropia_Representante,
                Celular_representante = model.Celular_Representante,
                CodCatalogoSangre_representante = null,
                Cod_Region_representante = model.Cod_Region_Representante,
                DireccionTrabajo_representante = model.DireccionTrabajo_Representante,
                CodCatalogoCONADIS_representante = (model.CodCatalogoCONADIS_Representante == "" ? null : model.CodCatalogoCONADIS_Representante),
                Correo_representante = model.Correo_Representante,
                IdCiudad_representante_fact = model.IdCiudad_Madre_Fact,
                Direccion_representante = model.Direccion_Representante,
                EmpresaTrabajo_representante = model.EmpresaTrabajo_Representante,
                EsRepresentante_representante = false,
                EstaFallecido_representante = model.EstaFallecido_Representante,
                IdCatalogoFichaInst_representante = model.IdCatalogoFichaInst_Representante,
                IdCiudad_representante = model.IdCiudad_Representante,
                IdEstadoCivil_representante = model.IdEstadoCivil_Representante,
                IdPais_representante = model.IdPais_Representante,
                IdParroquia_representante = model.IdParroquia_Representante,
                IdPersona_representante = model.IdPersona_Representante,
                IdParroquia_representante_fact = model.IdParroquia_Representante_Fact,
                IdProfesion_representante = model.IdProfesion_Representante,
                IdReligion_representante = model.IdReligion_Representante,
                IdProvincia_representante = model.IdProvincia_Representante,
                IdTipoCredito_representante = model.IdTipoCredito_Representante,
                IdTipoDocumento_representante = model.IdTipoDocumento_Representante,
                Idtipo_cliente_representante = model.Idtipo_cliente_Representante ?? 0,
                IngresoMensual_representante = model.IngresoMensual_Representante,
                Marca_representante = model.Marca_Representante,
                NumeroCarnetConadis_representante = model.NumeroCarnetConadis_Representante,
                pe_apellido_representante = model.Apellidos_Representante,
                pe_fechaNacimiento_representante = model.FechaNacimiento_Representante,
                pe_Naturaleza_representante = model.Naturaleza_Representante,
                pe_nombreCompleto_representante = model.NombreCompleto_Representante,
                pe_nombre_representante = model.Nombres_Representante,
                pe_razonSocial_representante = model.RazonSocial_Representante,
                pe_sexo_representante = model.Sexo_Representante,
                pe_telfono_Contacto_representante = model.Telefono_Representante,
                PorcentajeDiscapacidad_representante = model.PorcentajeDiscapacidad_Representante,
                Sector_representante = model.Sector_Representante,
                SeFactura_representante = model.SeFactura_Representante,
                TelefonoTrabajo_representante = model.TelefonoTrabajo_Representante,
                VehiculoPropio_representante = model.VehiculoPropio_Representante,
                IdCatalogo_representante = model.IdCatalogoPAREN_Representante,
                Modelo_representante = model.Modelo_Representante,
                IdGrupoEtnico_representante = model.IdGrupoEtnico_Representante,
                TelefonoRepresentante = model.Telefono_Representante,
            };

            if (model.CedulaRuc_Representante == model.CedulaRuc_Madre)
            {
                info_Alumno.EsRepresentante_madre = true;
            }
            else if (model.CedulaRuc_Representante == model.CedulaRuc_Padre)
            {
                info_Alumno.EsRepresentante_padre = true;
            }
            else
            {
                info_Alumno.EsRepresentante_representante = true;
            }

            info_Alumno.info_persona_alumno = new tb_persona_Info();
            info_Alumno.info_persona_padre = new tb_persona_Info();
            info_Alumno.info_persona_madre = new tb_persona_Info();

            var info_persona_aspirante = new tb_persona_Info
            {
                IdPersona = (info_ExistePersonaAspirante == null ? 0 : info_ExistePersonaAspirante.IdPersona),
                pe_Naturaleza = model.Naturaleza_Aspirante,
                IdTipoDocumento = model.IdTipoDocumento_Aspirante,
                pe_cedulaRuc = (model.CedulaRuc_Aspirante == "" ? null : model.CedulaRuc_Aspirante),
                pe_nombre = model.Nombres_Aspirante,
                pe_apellido = model.Apellidos_Aspirante,
                pe_nombreCompleto = model.NombreCompleto_Aspirante,
                pe_razonSocial = "",
                pe_sexo = model.Sexo_Aspirante,
                CodCatalogoCONADIS = model.CodCatalogoCONADIS_Aspirante,
                NumeroCarnetConadis = model.NumeroCarnetConadis_Aspirante,
                PorcentajeDiscapacidad = model.PorcentajeDiscapacidad_Aspirante,
                pe_fechaNacimiento = model.FechaNacimiento_Aspirante,
                pe_telfono_Contacto = model.Telefono_Aspirante,
                pe_correo = model.Correo_Aspirante,
                pe_celular = model.Celular_Aspirante,
                pe_direccion = model.Direccion_Aspirante,
                IdEstadoCivil = null,
                IdProfesion = null,
                IdReligion = model.IdReligion_Aspirante,
                IdGrupoEtnico = model.IdGrupoEtnico_Aspirante,
                AsisteCentroCristiano = model.AsisteCentroCristiano_Aspirante,
            };
            var info_persona_padre = new tb_persona_Info
            {
                IdPersona = (info_ExistePersonaPadre == null ? 0 : info_ExistePersonaPadre.IdPersona),
                pe_Naturaleza = model.Naturaleza_Padre,
                IdTipoDocumento = model.IdTipoDocumento_Padre,
                pe_cedulaRuc = (model.CedulaRuc_Padre == "" ? null : model.CedulaRuc_Padre),
                pe_nombre = model.Nombres_Padre,
                pe_apellido = model.Apellidos_Padre,
                pe_nombreCompleto = model.NombreCompleto_Padre,
                pe_razonSocial = model.RazonSocial_Padre,
                pe_sexo = model.Sexo_Padre,
                CodCatalogoCONADIS = model.CodCatalogoCONADIS_Padre,
                NumeroCarnetConadis = model.NumeroCarnetConadis_Padre,
                PorcentajeDiscapacidad = model.PorcentajeDiscapacidad_Padre,
                pe_fechaNacimiento = model.FechaNacimiento_Padre,
                pe_telfono_Contacto = model.Telefono_Padre,
                pe_correo = model.Correo_Padre,
                pe_celular = model.Celular_Padre,
                pe_direccion = model.Direccion_Padre,
                IdEstadoCivil = model.IdEstadoCivil_Padre,
                IdProfesion = model.IdProfesion_Padre,
                IdReligion = model.IdReligion_Padre,
                IdGrupoEtnico = model.IdGrupoEtnico_Padre,
                AsisteCentroCristiano = model.AsisteCentroCristiano_Padre,
            };
            var info_persona_madre = new tb_persona_Info
            {
                IdPersona = (info_ExistePersonaMadre == null ? 0 : info_ExistePersonaMadre.IdPersona),
                pe_Naturaleza = model.Naturaleza_Madre,
                IdTipoDocumento = model.IdTipoDocumento_Madre,
                pe_cedulaRuc = (model.CedulaRuc_Madre == "" ? null : model.CedulaRuc_Madre),
                pe_nombre = model.Nombres_Madre,
                pe_apellido = model.Apellidos_Madre,
                pe_nombreCompleto = model.NombreCompleto_Madre,
                pe_razonSocial = model.RazonSocial_Madre,
                pe_sexo = model.Sexo_Madre,
                CodCatalogoCONADIS = model.CodCatalogoCONADIS_Madre,
                NumeroCarnetConadis = model.NumeroCarnetConadis_Madre,
                PorcentajeDiscapacidad = model.PorcentajeDiscapacidad_Madre,
                pe_fechaNacimiento = model.FechaNacimiento_Madre,
                pe_telfono_Contacto = model.Telefono_Madre,
                pe_correo = model.Correo_Madre,
                pe_celular = model.Celular_Madre,
                pe_direccion = model.Direccion_Madre,
                IdEstadoCivil = model.IdEstadoCivil_Madre,
                IdProfesion = model.IdProfesion_Madre,
                IdReligion = model.IdReligion_Madre,
                IdGrupoEtnico = model.IdGrupoEtnico_Madre,
                AsisteCentroCristiano = model.AsisteCentroCristiano_Madre,
            };

            var info_persona_representante = new tb_persona_Info
            {
                IdPersona = (info_ExistePersonaRepresentante == null ? 0 : info_ExistePersonaRepresentante.IdPersona),
                pe_Naturaleza = model.Naturaleza_Representante,
                IdTipoDocumento = model.IdTipoDocumento_Representante,
                pe_cedulaRuc = (model.CedulaRuc_Representante == "" ? null : model.CedulaRuc_Representante),
                pe_nombre = model.Nombres_Representante,
                pe_apellido = model.Apellidos_Representante,
                pe_nombreCompleto = model.NombreCompleto_Representante,
                pe_razonSocial = model.RazonSocial_Representante,
                pe_sexo = model.Sexo_Representante,
                CodCatalogoCONADIS = model.CodCatalogoCONADIS_Representante,
                NumeroCarnetConadis = model.NumeroCarnetConadis_Representante,
                PorcentajeDiscapacidad = model.PorcentajeDiscapacidad_Representante,
                pe_fechaNacimiento = model.FechaNacimiento_Representante,
                pe_telfono_Contacto = model.Telefono_Representante,
                pe_correo = model.Correo_Representante,
                pe_celular = model.Celular_Representante,
                pe_direccion = model.Direccion_Representante,
                IdEstadoCivil = model.IdEstadoCivil_Representante,
                IdProfesion = model.IdProfesion_Representante,
                IdReligion = model.IdReligion_Representante,
                IdGrupoEtnico = model.IdGrupoEtnico_Representante,
                AsisteCentroCristiano = model.AsisteCentroCristiano_Representante,
            };

            info_Alumno.info_persona_alumno = info_persona_aspirante;
            info_Alumno.info_persona_padre = info_persona_padre;
            info_Alumno.info_persona_madre = info_persona_madre;
            info_Alumno.info_persona_representante = info_persona_representante;

            var info_SocioEconomico = new aca_SocioEconomico_Info
            {
                IdEmpresa = model.IdEmpresa,
                IdAlumno = info_Alumno == null ? 0 : info_Alumno.IdAlumno,
                IdCatalogoFichaVi = model.IdCatalogoFichaViv_Aspirante,
                IdCatalogoFichaTVi = model.IdCatalogoFichaTipoViv_Aspirante,
                IdCatalogoFichaAg = model.IdCatalogoFichaAgua_Aspirante,
                TieneElectricidad = model.TieneElectricidad_Aspirante,
                TieneHermanos = model.TieneHermanos_Aspirante,
                CantidadHermanos = model.CantidadHermanos,
                SueldoPadre = model.SueldoPadre,
                SueldoMadre = model.SueldoMadre,
                OtroIngresoMadre = model.OtroIngresoMadre,
                OtroIngresoPadre = model.OtroIngresoPadre,
                GastoAlimentacion = model.GastoAlimentacion,
                GastoEducacion = model.GastoEducacion,
                GastoServicioBasico = model.GastoServicioBasico,
                GastoSalud = model.GastoSalud,
                GastoArriendo = model.GastoArriendo,
                GastoPrestamo = model.GastoPrestamo,
                OtroGasto = model.OtroGasto,
                IdCatalogoFichaMot = model.IdCatalogoFichaMotivo_Aspirante,
                IdCatalogoFichaIns = model.IdCatalogoFichaInst_Aspirante,
                IdCatalogoFichaFin = model.IdCatalogoFichaFinanc_Aspirante,
                IdCatalogoFichaVive = model.IdCatalogoFichaVive_Aspirante,
                OtroFinanciamiento = model.OtroFinanciamiento_Aspirante,
                OtroInformacionInst = model.OtroInformacionInst_Aspirante,
                OtroMotivoIngreso = model.OtroMotivoIngreso_Aspirante,
                IdUsuarioCreacion = SessionFixed.IdUsuario,
                IdUsuarioModificacion = SessionFixed.IdUsuario,
        };

        var info_mecanismo = bus_mecanismo.GetInfo(model.IdEmpresa, model.IdMecanismo);
        var info_termino_pago = bus_termino_pago.get_info(info_mecanismo.IdTerminoPago);
        if (info_termino_pago != null && info_termino_pago.AplicaDescuentoNomina == false)
        {
            info_mecanismo = bus_mecanismo.GetInfo(model.IdEmpresa, model.IdMecanismoDet);
            info_termino_pago = bus_termino_pago.get_info(info_mecanismo.IdTerminoPago);
        }

        var info_PreMatricula = new aca_PreMatricula_Info
        {
            IdEmpresa = model.IdEmpresa,
            IdAdmision = model.IdAdmision,
            IdAlumno = info_Alumno==null ? 0 : info_Alumno.IdAlumno,
            IdAnio = IdAnio,
            IdSede = IdSede,
            IdNivel = IdNivel,
            IdJornada = IdJornada,
            IdCurso = IdCurso,
            IdParalelo = model.IdParalelo,
            IdPlantilla = model.IdPlantilla,
            IdMecanismo = model.IdMecanismo,
            Fecha = model.Fecha.Date,
            Observacion = model.Observacion,
            IdCatalogoESTPREMAT = model.IdCatalogoESTPREMAT,
            IdEmpresa_rol = ((info_termino_pago != null && info_termino_pago.AplicaDescuentoNomina == true) ? model.IdEmpresa_rol : (int?)null),
            IdEmpleado = ((info_termino_pago != null && info_termino_pago.AplicaDescuentoNomina == true) ? model.IdEmpleado : (decimal?)null),
            EsPatrocinado = model.EsPatrocinado,
            IdUsuarioCreacion = SessionFixed.IdUsuario,
            info_alumno = new aca_Alumno_Info(),
            lst_PreMatriculaRubro = new List<aca_PreMatricula_Rubro_Info>(),
            ExisteAlumno = (info_ExisteAlumno.IdAlumno>0 ? true : false),
            IdSucursal = model.IdSucursal,
            IdPuntoVta = model.IdPuntoVta,
            Valor = Convert.ToDecimal(model.ValorPlantilla),
            ValorProntoPago = Convert.ToDecimal(model.ValorPlantillaProntoPago),
        };

        info_PreMatricula.info_alumno = info_Alumno;
        info_PreMatricula.info_socioeconomico = info_SocioEconomico;

        if (model.CedulaRuc_Representante == model.CedulaRuc_Padre || model.CedulaRuc_Representante == model.CedulaRuc_Madre)
        {
            info_PreMatricula.OtraPersonaFamiliar = false;              
        }
        else
        {
            info_PreMatricula.OtraPersonaFamiliar = true;
            info_PreMatricula.IdCatalogoPAREN_OtroFamiliar = model.IdCatalogoPAREN_Representante;
        }
        info_PreMatricula.lst_PreMatriculaRubro = ListaPreMatriculaRubro.get_list(model.IdTransaccionSession);

        info_PreMatricula.lst_PreMatriculaRubro.Where(q=>q.seleccionado==true).ToList().ForEach(q=> q.IdMecanismo = model.IdMecanismo);
        info_PreMatricula.lst_PreMatriculaRubro.Where(q => q.seleccionado== false).ToList().ForEach(q => q.IdMecanismo = model.IdMecanismoDet);
        return info_PreMatricula;
    }
        #endregion

        #region JSON
        public JsonResult CantidadAdmisiones(string IdUsuario = "")
        {
            var IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var info_anio = bus_anio.GetInfo_AnioEnCurso(IdEmpresa,0);
            var IdAnio = info_anio == null ? 0 : info_anio.IdAnio;

            var lst_admisiones = bus_admision.GetList_Admisiones(IdEmpresa,IdSede);
            var cantidad = lst_admisiones.Where(q=>q.IdCatalogoESTADM==Convert.ToInt32(cl_enumeradores.eTipoCatalogoAdmision.REGISTRADO)).Count();
            return Json(cantidad, JsonRequestBehavior.AllowGet);
    }
        public JsonResult Validar_cedula_ruc(string naturaleza = "", string tipo_documento = "", string cedula_ruc = "")
        {
            var return_naturaleza = "";
            var isValid = cl_funciones.ValidaIdentificacion(tipo_documento, naturaleza, cedula_ruc, ref return_naturaleza);

            return Json(new { isValid = isValid, return_naturaleza = return_naturaleza }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult get_info_x_num_cedula(int IdEmpresa = 0, string pe_cedulaRuc = "")
        {
            var resultado = bus_alumno.get_info_x_num_cedula(IdEmpresa, pe_cedulaRuc);
            resultado.anio = Convert.ToDateTime(resultado.pe_fechaNacimiento).Year.ToString();
            var mes = Convert.ToDateTime(resultado.pe_fechaNacimiento).Month;
            mes = mes - 1;
            resultado.mes = mes.ToString();
            resultado.dia = Convert.ToDateTime(resultado.pe_fechaNacimiento).Day.ToString();

            var info_admision = bus_admision.GetInfo_CedulaAspirante(IdEmpresa, pe_cedulaRuc);
            resultado.IdAdmision = (info_admision == null ? 0 : info_admision.IdAdmision);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public JsonResult get_info_x_num_cedula_Prematricula(int IdEmpresa = 0, int IdAnio = 0, string pe_cedulaRuc = "")
        {
            var IdSede = Convert.ToInt32(SessionFixed.IdSede);
            var resultado = bus_alumno.get_info_x_num_cedula(IdEmpresa, pe_cedulaRuc);

            resultado.anio = Convert.ToDateTime(resultado.pe_fechaNacimiento).Year.ToString();
            var mes = Convert.ToDateTime(resultado.pe_fechaNacimiento).Month;
            mes = mes - 1;
            resultado.mes = mes.ToString();
            resultado.dia = Convert.ToDateTime(resultado.pe_fechaNacimiento).Day.ToString();

            var info_prematricula = bus_prematricula.GetInfo_PorIdAlumno(IdEmpresa, IdSede, IdAnio, resultado.IdAlumno);
            resultado.IdAdmision = (info_prematricula == null ? 0 : info_prematricula.IdAdmision);
            resultado.IdPreMatricula = (info_prematricula == null ? 0 : info_prematricula.IdPreMatricula);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public JsonResult get_info_x_num_cedula_persona(string pe_cedulaRuc = "")
        {
            var resultado = new tb_persona_Info();
            resultado = bus_persona.get_info_x_num_cedula(pe_cedulaRuc);
            resultado = (resultado == null ? new tb_persona_Info() : resultado);

            resultado.anio = Convert.ToDateTime(resultado.pe_fechaNacimiento).Year.ToString();
            var mes = Convert.ToDateTime(resultado.pe_fechaNacimiento).Month;
            mes = mes - 1;
            resultado.mes = mes.ToString();
            resultado.dia = Convert.ToDateTime(resultado.pe_fechaNacimiento).Day.ToString();

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ActualizarDocumentosSeleccionados(string IdComboCurso = "", string Seleccionados = "")
        {
            var IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var lst_detalle = Lista_DocumentosMatricula.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            lst_detalle.ForEach(q=>q.seleccionado=false);
            if (Seleccionados != null && Seleccionados != "")
            {
                string[] array = Seleccionados.Split(',');
                foreach (var item in array)
                {
                    var info = lst_detalle.Where(q => q.IdStringDoc == item).FirstOrDefault();
                    if (info != null)
                    {
                        lst_detalle.Where(q => q.IdStringDoc == item).FirstOrDefault().seleccionado = true;
                    }
                }
                Lista_DocumentosMatricula.set_list(lst_detalle, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            }
            return Json(lst_detalle, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SumarValores(int IdAnio = 0, int IdPlantilla = 0, string Seleccionados = "", decimal IdTransaccionSession=0)
        {
            var IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            decimal Total = 0;
            decimal TotalProntoPago = 0;
            decimal ValorRubro = 0;
            decimal ValorDescuento = 0;
            var lst_detalle = ListaPreMatriculaRubro.get_list(IdTransaccionSession);

            if (Seleccionados != null && Seleccionados != "")
            {
                string[] array = Seleccionados.Split(',');
                var info_plantilla = bus_plantilla.GetInfo(IdEmpresa, IdAnio, IdPlantilla);

                foreach (var item in array)
                {
                    var info = lst_detalle.Where(q => q.IdString == item).FirstOrDefault();
                    if (info!=null)
                    {
                        lst_detalle.Where(q => q.IdString == item).FirstOrDefault().seleccionado = true;
                    }

                    var info_anio_periodo = bus_anio_periodo.GetInfo(IdEmpresa, IdAnio, info.IdPeriodo);
                    if (info.AplicaProntoPago == true)
                    {
                        if (DateTime.Now.Date <= info_anio_periodo.FechaProntoPago)
                        {
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
                ListaPreMatriculaRubro.set_list(lst_detalle , IdTransaccionSession);
            }
            return Json(new { ValorPlantilla = Total, ValorPlantillaProntoPago = TotalProntoPago }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult EnlistarDocumentos(int IdEmpresa = 0, decimal IdAdmision=0, decimal IdTransaccionSession=0)
        {
            var info_parametros = bus_parametro.get_info(IdEmpresa);
            List<aca_AlumnoDocumento_Info> lst_documentos = new List<aca_AlumnoDocumento_Info>();
            string ftpURLPrefix = "ftp://";
            List<string> Lista = new List<string>();
            string url = ftpURLPrefix + info_parametros.FtpUrl + IdAdmision.ToString();
            //string url = ftpURLPrefix + info_parametros.FtpUrl + "1";
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);

            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.EnableSsl = true;
            request.Credentials = new NetworkCredential(info_parametros.FtpUser, info_parametros.FtpPassword);

            ServicePointManager.ServerCertificateValidationCallback =
                 (s, certificate, chain, sslPolicyErrors) => true;

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);

            string Directorio = reader.ReadToEnd();
            reader.Close();
            response.Close();
            string[] stringSeparators = new string[] { "\r\n" };
            string[] lines = Directorio.Split(stringSeparators, StringSplitOptions.None);
            Lista = lines.Where(q => !string.IsNullOrEmpty(q) && q != "Processed").ToList();
            var secuencia = 1;
            foreach (var item in Lista)
            {
                var aluDocumento = new aca_AlumnoDocumento_Info
                {
                    Secuencia = secuencia++,
                    IdAdmision = IdAdmision,
                    NomDocumento = item,
                    urlDoc = url+"/"+item
                };
                lst_documentos.Add(aluDocumento);
            }
            Lista_DocAdmision.set_list(lst_documentos, IdTransaccionSession);

            return Json(lst_documentos, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SetDocumentos(int IdEmpresa = 0, int IdAnio = 0, string IdCurso = "", decimal IdAlumno = 0)
        {
            var IdSede = Convert.ToInt32(IdCurso.Substring(8, 4));
            var IdNivel = Convert.ToInt32(IdCurso.Substring(12, 4));
            var IdJornada = Convert.ToInt32(IdCurso.Substring(16, 4));
            var IdCursoMat = Convert.ToInt32(IdCurso.Substring(20, 4));
            List<aca_AnioLectivo_Curso_Documento_Info> lst_Documentos = new List<aca_AnioLectivo_Curso_Documento_Info>();

            var lst_doc_curso = bus_curso_documento.GetList_Matricula(IdEmpresa, IdSede, IdAnio, IdNivel, IdJornada, IdCursoMat);
            var lst_doc_alumno = bus_alumno_documento.GetList(IdEmpresa, IdAlumno, true);

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
                    lst_Documentos.Add(item);
                }
            }

            Lista_DocumentosMatricula.set_list(lst_Documentos, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return Json(lst_Documentos, JsonRequestBehavior.AllowGet);
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
                Empresa = (info_mecanismo == null ? 0 : info_mecanismo.IdEmpresa_rol ?? 0);
            }
            else
            {
                if (IdMecanismoOtros != 0)
                {
                    info_mecanismo = bus_mecanismo.GetInfo(IdEmpresa, IdMecanismoOtros);
                    info_termino_pago = bus_termino_pago.get_info(info_mecanismo.IdTerminoPago);
                    if (info_termino_pago != null && info_termino_pago.AplicaDescuentoNomina == true)
                    {
                        resultado = info_termino_pago.AplicaDescuentoNomina ?? false;
                        Empresa = (info_mecanismo == null ? 0 : info_mecanismo.IdEmpresa_rol ?? 0);
                    }
                }
            }

            return Json(new { MostrarEmpleado = resultado, IdEmpresa_rol = Empresa }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LimpiarListaDetalle(int IdEmpresa = 0, decimal IdTransaccionSession=0)
        {
            decimal Total = 0;
            decimal TotalProntoPago = 0;
            decimal ValorTotal = 0;
            decimal ValorTotalPP = 0;

            ValorTotal = Total;
            ValorTotalPP = TotalProntoPago;
            ListaPreMatriculaRubro.set_list(new List<aca_PreMatricula_Rubro_Info>(), IdTransaccionSession);

            return Json(new { Valor = ValorTotal, ProntoPago = ValorTotalPP }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SetMatriculaRubro(int IdEmpresa = 0, int IdAnio = 0, int IdPlantilla = 0, int IdMatricula = 0, decimal IdTransaccionSession=0)
        {
            decimal Total = 0;
            decimal TotalProntoPago = 0;
            decimal ValorDescuento = 0;
            decimal ValorRubro = 0;
            decimal ValorTotal = 0;
            decimal ValorTotalPP = 0;
            List<aca_PreMatricula_Rubro_Info> lst_MatriculaRubro = new List<aca_PreMatricula_Rubro_Info>();
            if (IdMatricula == 0)
            {
                var info_plantilla = bus_plantilla.GetInfo(IdEmpresa, IdAnio, IdPlantilla);
                lst_MatriculaRubro = bus_prematricula_rubro.GetList_PreMatricula(IdEmpresa, IdAnio, IdPlantilla);
                if (lst_MatriculaRubro.Count() > 0)
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
                    ListaPreMatriculaRubro.set_list(lst_MatriculaRubro, IdTransaccionSession);
                }
                else
                {
                    ValorTotal = Total;
                    ValorTotalPP = TotalProntoPago;
                    ListaPreMatriculaRubro.set_list(new List<aca_PreMatricula_Rubro_Info>(), IdTransaccionSession);
                }

            }
            else
            {
                lst_MatriculaRubro = ListaPreMatriculaRubro.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
                var lst_nueva_plantilla = bus_prematricula_rubro.GetList_PreMatricula(IdEmpresa, IdAnio, IdPlantilla);
                var lista_no_cobrado = lst_MatriculaRubro.Where(q => q.EnMatricula == false).ToList();

                List<aca_PreMatricula_Rubro_Info> lista_nueva = new List<aca_PreMatricula_Rubro_Info>();

                foreach (var item in lst_MatriculaRubro)
                {
                    if (item.EnMatricula == true)
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

                ListaPreMatriculaRubro.set_list(lista_nueva, IdTransaccionSession);
            }


            return Json(new { Valor = ValorTotal, ProntoPago = ValorTotalPP }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ProcesarAdmision(int IdEmpresa = 0, decimal IdAdmision = 0)
        {
            var info_admision = bus_admision.GetInfo(IdEmpresa, IdAdmision);
            var Destinatarios = (info_admision == null ? "" : (info_admision.SeFactura_Padre==true ? info_admision.Correo_Padre: (info_admision.SeFactura_Madre ? info_admision.Correo_Madre : info_admision.Correo_Representante)) + ";" + info_admision.Correo_Padre + ";" + info_admision.Correo_Madre + ";" + info_admision.Correo_Representante);
            var mensaje = "";
            if (info_admision.IdUsuarioRevision == null)
            {
                info_admision.IdUsuarioRevision = SessionFixed.IdUsuario;
                info_admision.IdUsuarioModificacion = SessionFixed.IdUsuario;
                info_admision.IdCatalogoESTADM = Convert.ToInt32(cl_enumeradores.eTipoCatalogoAdmision.ENPROCESO);

                
                if (bus_admision.ModificarEstadoEnProceso(info_admision))
                {
                    mensaje = "El estado de la admisión cambió a proceso de revisión";

                    var info_catalogo = bus_aca_catalogo.GetInfo(Convert.ToInt32(info_admision.IdCatalogoESTADM));
                    var info_correo = new tb_ColaCorreo_Info
                    {
                        IdEmpresa = info_admision.IdEmpresa,
                        Destinatarios = Destinatarios,
                        Asunto = "ADMISION EN PROCESO DE REVISION",
                        Parametros = "",
                        Codigo = "",
                        IdUsuarioCreacion = SessionFixed.IdUsuario,
                        Cuerpo = (info_catalogo == null ? "" : info_catalogo.NomCatalogo),
                        FechaCreacion = DateTime.Now
                    };
                    bus_cola_correo.GuardarDB(info_correo);
                }
                else
                {
                    mensaje = "El estado de la admisión no se ha actualizado";
                }
            }
            else
            {
                mensaje = "La admisión ya esta siendo revisando por: " + info_admision.IdUsuarioRevision + " desde: " + info_admision.FechaRevision.ToString();
            }
            
            
            return Json(mensaje, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SetMatricula_PorCurso(int IdEmpresa = 0, int IdAnio = 0, string IdComboCurso = "", int IdParalelo = 0)
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
            List_Matricula_PorParalelo.set_list(lista_PorCurso, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return Json(lista_PorCurso, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public List<aca_AlumnoDocumento_Info> DocumentosAdmision(int IdEmpresa = 0, decimal IdAdmision = 0, decimal IdTransaccionSession = 0)
        {
            var info_parametros = bus_parametro.get_info(IdEmpresa);
            List<aca_AlumnoDocumento_Info> lst_documentos = new List<aca_AlumnoDocumento_Info>();

            try
            {
                string ftpURLPrefix = "ftp://";
                List<string> Lista = new List<string>();
                string url = ftpURLPrefix + info_parametros.FtpUrl + IdAdmision.ToString();
                //string url = ftpURLPrefix + info_parametros.FtpUrl + "1";
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);

                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.EnableSsl = true;
                request.Credentials = new NetworkCredential(info_parametros.FtpUser, info_parametros.FtpPassword);

                ServicePointManager.ServerCertificateValidationCallback =
                     (s, certificate, chain, sslPolicyErrors) => true;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);

                string Directorio = reader.ReadToEnd();
                reader.Close();
                response.Close();
                string[] stringSeparators = new string[] { "\r\n" };
                string[] lines = Directorio.Split(stringSeparators, StringSplitOptions.None);
                Lista = lines.Where(q => !string.IsNullOrEmpty(q) && q != "Processed").ToList();
                var secuencia = 1;
                foreach (var item in Lista)
                {
                    var aluDocumento = new aca_AlumnoDocumento_Info
                    {
                        Secuencia = secuencia++,
                        IdAdmision = IdAdmision,
                        NomDocumento = item,
                        urlDoc = url + "/" + item
                    };
                    lst_documentos.Add(aluDocumento);
                }
            }
            catch (Exception ex)
            {
                lst_documentos = new List<aca_AlumnoDocumento_Info>();
            }           

            return lst_documentos;
        }

        public void DescargarArchivo(int IdAdmision, string filename)
        {
            //string fileName = filename;
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var info_parametros = bus_parametro.get_info(IdEmpresa);
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://"+info_parametros.FtpUrl+"/"+IdAdmision+"/"+filename);

            request.Method = WebRequestMethods.Ftp.DownloadFile;

            request.Credentials = new NetworkCredential(info_parametros.FtpUser, info_parametros.FtpPassword);

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            using (MemoryStream stream = new MemoryStream())
            {
                response.GetResponseStream().CopyTo(stream);
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite(stream.ToArray());
                Response.End();
            }
        }
    }
}

public class aca_ProcesarAdmision_List
{
    string Variable = "aca_ProcesarAdmision_Info";
    public List<aca_Admision_Info> get_list(decimal IdTransaccionSession)
    {
        if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
        {
            List<aca_Admision_Info> list = new List<aca_Admision_Info>();

            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
        return (List<aca_Admision_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
    }

    public void set_list(List<aca_Admision_Info> list, decimal IdTransaccionSession)
    {
        HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
    }
}

public class aca_Matricula_PorParalelo_List
{
    string Variable = "aca_Matricula_PorParalelo_Info";
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
public class aca_AlumnoDocumentoAdmision_List
{
    string Variable = "aca_AlumnoDocumentoAdmision_Info";
    public List<aca_AlumnoDocumento_Info> get_list(decimal IdTransaccionSession)
    {
        if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
        {
            List<aca_AlumnoDocumento_Info> list = new List<aca_AlumnoDocumento_Info>();

            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
        return (List<aca_AlumnoDocumento_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
    }

    public void set_list(List<aca_AlumnoDocumento_Info> list, decimal IdTransaccionSession)
    {
        HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
    }
}
public class aca_PreMatricula_Rubro_List
{
    string Variable = "aca_PreMatricula_Rubro_Info";
    public List<aca_PreMatricula_Rubro_Info> get_list(decimal IdTransaccionSession)
    {
        if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
        {
            List<aca_PreMatricula_Rubro_Info> list = new List<aca_PreMatricula_Rubro_Info>();

            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
        return (List<aca_PreMatricula_Rubro_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
    }

    public void set_list(List<aca_PreMatricula_Rubro_Info> list, decimal IdTransaccionSession)
    {
        HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
    }

    public void UpdateRow(aca_PreMatricula_Rubro_Info info_det, decimal IdTransaccion)
    {
        int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
        aca_PreMatricula_Rubro_Info edited_info = get_list(IdTransaccion).Where(q => q.IdString == info_det.IdString).FirstOrDefault();
        if (edited_info.FechaFacturacion == null)
        {
            edited_info.IdMecanismo = info_det.IdMecanismo;
        }
    }
}