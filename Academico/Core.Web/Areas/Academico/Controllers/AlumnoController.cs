using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web;
using Core.Info.Academico;
using Core.Web.Helps;
using Core.Bus.Academico;
using Core.Bus.General;
using Core.Info.Helps;
using Core.Info.General;
using Core.Bus.Facturacion;
using Core.Info.Facturacion;
using System.IO;
using ExcelDataReader;

namespace Core.Web.Areas.Academico.Controllers
{
    public class AlumnoController : Controller
    {
        #region Variables
        aca_Alumno_Bus bus_alumno = new aca_Alumno_Bus();
        aca_AlumnoDocumento_Bus bus_alumno_documento = new aca_AlumnoDocumento_Bus();
        aca_Familia_Bus bus_familia = new aca_Familia_Bus();
        tb_Catalogo_Bus bus_catalogo = new tb_Catalogo_Bus();
        aca_Catalogo_Bus bus_aca_catalogo = new aca_Catalogo_Bus();
        aca_alumno_List Lista_Alumno = new aca_alumno_List();
        aca_Familia_List Lista_Familia = new aca_Familia_List();
        aca_AlumnoDocumento_List ListaAlumnoDocumento = new aca_AlumnoDocumento_List();
        aca_SocioEconomico_Bus bus_socioeconomico = new aca_SocioEconomico_Bus();
        tb_profesion_Bus bus_profesion = new tb_profesion_Bus();
        aca_CatalogoFicha_Bus bus_catalogo_ficha = new aca_CatalogoFicha_Bus();
        fa_cliente_tipo_Bus bus_clientetipo = new fa_cliente_tipo_Bus();
        fa_TerminoPago_Bus bus_termino_pago = new fa_TerminoPago_Bus();
        fa_cliente_Bus bus_cliente = new fa_cliente_Bus();
        fa_cliente_contactos_Bus bus_cliente_cont = new fa_cliente_contactos_Bus();
        tb_ciudad_Bus bus_ciudad = new tb_ciudad_Bus();
        tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
        aca_Sede_Bus bus_sede = new aca_Sede_Bus();
        aca_Documento_Bus bus_documento = new aca_Documento_Bus();
        tb_Religion_Bus bus_religion = new tb_Religion_Bus();
        tb_GrupoEtnico_Bus bus_grupoetnico = new tb_GrupoEtnico_Bus();
        aca_parametro_Bus bus_parametro = new aca_parametro_Bus();
        aca_AnioLectivo_Bus bus_anio = new aca_AnioLectivo_Bus();
        aca_Matricula_Bus bus_matricula = new aca_Matricula_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        string mensaje = string.Empty;
        public static UploadedFile file { get; set; }
        public int IdAlumno_ { get; set; }
        public static byte[] imagen { get; set; }
        public static byte[] pr_imagen { get; set; }
        aca_Menu_x_seg_usuario_Bus bus_permisos = new aca_Menu_x_seg_usuario_Bus();
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

            aca_Alumno_Info model = new aca_Alumno_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession)
            };

            List<aca_Alumno_Info> lista = bus_alumno.GetList(model.IdEmpresa, true);
            Lista_Alumno.set_list(lista, Convert.ToDecimal(SessionFixed.IdTransaccionSession));

            #region Permisos
            aca_Menu_x_seg_usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(SessionFixed.IdSede), SessionFixed.IdUsuario, "Academico", "Alumno", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_alumno(bool Nuevo = false, bool Modificar = false, bool Anular = false)
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            List<aca_Alumno_Info> model = Lista_Alumno.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            ViewBag.Nuevo = Nuevo;
            ViewBag.Modificar = Modificar;
            ViewBag.Anular = Anular;

            return PartialView("_GridViewPartial_alumno", model);
        }
        #endregion

        #region Combos bajo demanda

        public ActionResult CmbDocumentoAlumno()
        {
            aca_Documento_Info model = new aca_Documento_Info();
            return PartialView("_CmbDocumentoAlumno", model);
        }

        public List<aca_Documento_Info> get_list_bajo_demandaDocumento(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            int IdAnio = Convert.ToInt32(SessionFixed.IdAnio);
            return bus_documento.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa));
        }
        public aca_Documento_Info get_info_bajo_demandaDocumento(ListEditItemRequestedByValueEventArgs args)
        {
            int IdAnio = Convert.ToInt32(SessionFixed.IdAnio);
            return bus_documento.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa));
        }
        #endregion

        #region Metodos
        private void cargar_combos()
        {
            var IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var lst_sexo = bus_catalogo.get_list(Convert.ToInt32(cl_enumeradores.eTipoCatalogoGeneral.SEXO), false);
            var lst_estado_civil = bus_catalogo.get_list(Convert.ToInt32(cl_enumeradores.eTipoCatalogoGeneral.ESTCIVIL), false);
            var lst_tipo_doc = bus_catalogo.get_list(Convert.ToInt32(cl_enumeradores.eTipoCatalogoGeneral.TIPODOC), false);
            var lst_tipo_naturaleza = bus_catalogo.get_list(Convert.ToInt32(cl_enumeradores.eTipoCatalogoGeneral.TIPONATPER), false);
            var lst_tipo_sangre = bus_catalogo.get_list(Convert.ToInt32(cl_enumeradores.eTipoCatalogoGeneral.TIPOSANGRE), false);
            var lst_tipo_discapacidad = bus_catalogo.get_list(Convert.ToInt32(cl_enumeradores.eTipoCatalogoGeneral.TIPODISCAP), false);
            var lst_instruccion = bus_catalogo_ficha.GetList_x_Tipo(Convert.ToInt32(cl_enumeradores.eTipoCatalogoSocioEconomico.INSTRUCCION), false);
            lst_instruccion.Add(new aca_CatalogoFicha_Info { IdCatalogoFicha = 0, NomCatalogoFicha = "" });
            lst_tipo_discapacidad.Add(new tb_Catalogo_Info { CodCatalogo = "", ca_descripcion = ""});
            var lst_profesion = bus_profesion.GetList(false);
            var lst_religion = bus_religion.GetList(false);
            var lst_grupoetnico = bus_grupoetnico.GetList(false);

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

            var lst_termino_pago = bus_termino_pago.get_list(false);
            ViewBag.lst_termino_pago = lst_termino_pago;

            var lst_clientetipo = bus_clientetipo.get_list(IdEmpresa, false);
            ViewBag.lst_clientetipo = lst_clientetipo;

            var lst_ciudad = bus_ciudad.get_list("", false);
            ViewBag.lst_ciudad = lst_ciudad;

            var lst_sucursal = bus_sucursal.get_list(IdEmpresa, false);
            ViewBag.lst_sucursal = lst_sucursal;
        }

        private bool validar(aca_Alumno_Info info, ref string msg)
        {
            string return_naturaleza = "";
            string return_naturaleza_padre = "";
            string return_naturaleza_madre = "";

            if (cl_funciones.ValidaIdentificacion(info.info_persona_alumno.IdTipoDocumento, info.info_persona_alumno.pe_Naturaleza, info.info_persona_alumno.pe_cedulaRuc, ref return_naturaleza))
            {
                info.info_persona_alumno.pe_Naturaleza = return_naturaleza;
            }
            else
            {
                msg = "Número de identificación del alumno inválida";
                return false;
            }

            if (info.info_persona_padre.IdTipoDocumento != "" && info.info_persona_padre.pe_Naturaleza != "" && info.info_persona_padre.pe_cedulaRuc != null)
            {
                if (cl_funciones.ValidaIdentificacion(info.info_persona_padre.IdTipoDocumento, info.info_persona_padre.pe_Naturaleza, info.info_persona_padre.pe_cedulaRuc, ref return_naturaleza_padre))
                {
                    info.info_persona_padre.pe_Naturaleza = return_naturaleza_padre;
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
                info.info_valido_padre = false;
            }

            if (info.info_persona_madre.IdTipoDocumento != "" && info.info_persona_madre.pe_Naturaleza != "" && info.info_persona_madre.pe_cedulaRuc != null)
            {
                if (cl_funciones.ValidaIdentificacion(info.info_persona_madre.IdTipoDocumento, info.info_persona_madre.pe_Naturaleza, info.info_persona_madre.pe_cedulaRuc, ref return_naturaleza_madre))
                {
                    info.info_persona_madre.pe_Naturaleza = return_naturaleza_madre;
                    info.info_valido_madre = true;
                }
                else
                {
                    msg = "Número de identificación de la madre inválida";
                    info.info_valido_madre = false;
                    return false;
                }
            }
            else
            {
                info.info_valido_madre = false;
            }

            if (info.info_persona_padre.pe_cedulaRuc!=null && info.info_persona_madre.pe_cedulaRuc!=null && (info.info_persona_padre.pe_cedulaRuc == info.info_persona_madre.pe_cedulaRuc))
            {
                msg = "No se puede registrar a la misma persona como padre y madre";
                return false;
            }

            if (info.IdAlumno==0)
            {
                if (!string.IsNullOrEmpty(info.info_persona_padre.pe_cedulaRuc) && (info.info_persona_padre.pe_cedulaRuc == info.info_persona_alumno.pe_cedulaRuc))
                {
                    msg = "El número de identificacion del alumno no puede ser igual al número de identificacion del padre";
                    return false;
                }

                if (!string.IsNullOrEmpty(info.info_persona_madre.pe_cedulaRuc) && (info.info_persona_madre.pe_cedulaRuc == info.info_persona_alumno.pe_cedulaRuc))
                {
                    msg = "El número de identificacion del alumno no puede ser igual al número de identificacion de la madre";
                    return false;
                }
            }
            else
            {
                var existe_fam_alumno = bus_familia.existe_familia(info.IdEmpresa, info.IdAlumno, info.info_persona_alumno.pe_cedulaRuc);

                if (existe_fam_alumno != null)
                {
                    msg = "El número de identificacion del alumno ya existe como familiar - " + existe_fam_alumno.NomCatalogo;
                    return false;
                }
            }

            return true;
        }

        private tb_persona_Info armar_info_padre(aca_Alumno_Info model)
        {
            var info_persona = new tb_persona_Info
                {
                    IdPersona = model.IdPersona_padre,
                    pe_Naturaleza = model.pe_Naturaleza_padre,
                    IdTipoDocumento = model.IdTipoDocumento_padre,
                    pe_cedulaRuc = (model.pe_cedulaRuc_padre=="" ? null : model.pe_cedulaRuc_padre),
                    pe_nombre = model.pe_nombre_padre,
                    pe_apellido = model.pe_apellido_padre,
                    pe_nombreCompleto = model.pe_nombreCompleto_padre,
                    pe_razonSocial = model.pe_razonSocial_padre,
                    pe_sexo = model.pe_sexo_padre,
                    CodCatalogoCONADIS = model.CodCatalogoCONADIS_padre,
                    NumeroCarnetConadis = model.NumeroCarnetConadis_padre,
                    PorcentajeDiscapacidad = model.PorcentajeDiscapacidad_padre,
                    pe_fechaNacimiento = model.pe_fechaNacimiento_padre,
                    pe_telfono_Contacto = model.pe_telfono_Contacto_padre,
                    pe_correo = model.Correo_padre,
                    pe_celular = model.Celular_padre,
                    pe_direccion = model.Direccion_padre,
                    IdEstadoCivil = model.IdEstadoCivil_padre,
                    IdProfesion = model.IdProfesion_padre,
                    IdReligion = model.IdReligion_padre,
                    AsisteCentroCristiano = model.AsisteCentroCristiano_padre
            };        

            return info_persona;
        }

        private tb_persona_Info armar_info_madre(aca_Alumno_Info model)
        {
            var info_persona = new tb_persona_Info
                {
                    IdPersona = model.IdPersona_madre,
                    pe_Naturaleza = model.pe_Naturaleza_madre,
                    IdTipoDocumento = model.IdTipoDocumento_madre,
                    pe_cedulaRuc = (model.pe_cedulaRuc_madre== "" ? null : model.pe_cedulaRuc_madre),
                    pe_nombre = model.pe_nombre_madre,
                    pe_apellido = model.pe_apellido_madre,
                    pe_nombreCompleto = model.pe_nombreCompleto_madre,
                    pe_razonSocial = model.pe_razonSocial_madre,
                    pe_sexo = model.pe_sexo_madre,
                    CodCatalogoCONADIS = model.CodCatalogoCONADIS_madre,
                    NumeroCarnetConadis = model.NumeroCarnetConadis_madre,
                    PorcentajeDiscapacidad = model.PorcentajeDiscapacidad_madre,
                    pe_fechaNacimiento = model.pe_fechaNacimiento_madre,
                    pe_telfono_Contacto = model.pe_telfono_Contacto_madre,
                    pe_correo = model.Correo_madre,
                    pe_celular = model.Celular_madre,
                    pe_direccion = model.Direccion_madre,
                    IdEstadoCivil = model.IdEstadoCivil_madre,
                    IdProfesion = model.IdProfesion_madre,
                    IdReligion = model.IdReligion_madre,
                    AsisteCentroCristiano = model.AsisteCentroCristiano_madre
            };

            return info_persona;
        }
        #endregion
        #region CombosPadre
        public ActionResult ComboBoxPartial_Pais_padre()
        {
            return PartialView("_ComboBoxPartial_Pais_padre", new aca_Alumno_Info());
        }
        public ActionResult ComboBoxPartial_Region_padre()
        {
            string IdPais = (Request.Params["IdPais_padre"] != null) ? Convert.ToString(Request.Params["IdPais_padre"]) : "";
            return PartialView("_ComboBoxPartial_Region_padre", new aca_Alumno_Info { IdPais_padre = IdPais });
        }
        public ActionResult ComboBoxPartial_Provincia_padre()
        {
            string IdPais = (Request.Params["IdPais_padre"] != null) ? Convert.ToString(Request.Params["IdPais_padre"]) : "";
            string Cod_Region = (Request.Params["Cod_Region_padre"] != null) ? Convert.ToString(Request.Params["Cod_Region_padre"]) : "";
            return PartialView("_ComboBoxPartial_Provincia_padre", new aca_Alumno_Info { IdPais_padre = IdPais, Cod_Region_padre = Cod_Region });
        }
        public ActionResult ComboBoxPartial_Ciudad_padre()
        {
            string IdPais = (Request.Params["IdPais_padre"] != null) ? Convert.ToString(Request.Params["IdPais_padre"]) : "";
            string Cod_Region = (Request.Params["Cod_Region_padre"] != null) ? Convert.ToString(Request.Params["Cod_Region_padre"]) : "";
            string IdProvincia = (Request.Params["IdProvincia_padre"] != null) ? Convert.ToString(Request.Params["IdProvincia_padre"]) : "";
            return PartialView("_ComboBoxPartial_Ciudad_padre", new aca_Alumno_Info { IdPais_padre = IdPais, Cod_Region_padre = Cod_Region, IdProvincia_padre = IdProvincia });
        }
        public ActionResult ComboBoxPartial_Parroquia_padre()
        {
            string IdPais = (Request.Params["IdPais_padre"] != null) ? Convert.ToString(Request.Params["IdPais_padre"]) : "";
            string Cod_Region = (Request.Params["Cod_Region_padre"] != null) ? Convert.ToString(Request.Params["Cod_Region_padre"]) : "";
            string IdProvincia = (Request.Params["IdProvincia_padre"] != null) ? Convert.ToString(Request.Params["IdProvincia_padre"]) : "";
            string IdCiudad = (Request.Params["IdCiudad_padre"] != null) ? Convert.ToString(Request.Params["IdCiudad_padre"]) : "";
            return PartialView("_ComboBoxPartial_Parroquia_padre", new aca_Alumno_Info { IdPais_padre = IdPais, Cod_Region_padre = Cod_Region, IdProvincia_padre = IdProvincia, IdCiudad_padre = IdCiudad });
        }
        #endregion
        #region CombosMadre
        public ActionResult ComboBoxPartial_Pais_madre()
        {
            return PartialView("_ComboBoxPartial_Pais_madre", new aca_Alumno_Info());
        }
        public ActionResult ComboBoxPartial_Region_madre()
        {
            string IdPais = (Request.Params["IdPais_madre"] != null) ? Convert.ToString(Request.Params["IdPais_madre"]) : "";
            return PartialView("_ComboBoxPartial_Region_madre", new aca_Alumno_Info { IdPais_madre = IdPais });
        }
        public ActionResult ComboBoxPartial_Provincia_madre()
        {
            string IdPais = (Request.Params["IdPais_madre"] != null) ? Convert.ToString(Request.Params["IdPais_madre"]) : "";
            string Cod_Region = (Request.Params["Cod_Region_madre"] != null) ? Convert.ToString(Request.Params["Cod_Region_madre"]) : "";
            return PartialView("_ComboBoxPartial_Provincia_madre", new aca_Alumno_Info { IdPais_madre = IdPais, Cod_Region_madre = Cod_Region });
        }
        public ActionResult ComboBoxPartial_Ciudad_madre()
        {
            string IdPais = (Request.Params["IdPais_madre"] != null) ? Convert.ToString(Request.Params["IdPais_madre"]) : "";
            string Cod_Region = (Request.Params["Cod_Region_madre"] != null) ? Convert.ToString(Request.Params["Cod_Region_madre"]) : "";
            string IdProvincia = (Request.Params["IdProvincia_madre"] != null) ? Convert.ToString(Request.Params["IdProvincia_madre"]) : "";
            return PartialView("_ComboBoxPartial_Ciudad_madre", new aca_Alumno_Info { IdPais_madre = IdPais, Cod_Region_madre = Cod_Region, IdProvincia_madre = IdProvincia });
        }
        public ActionResult ComboBoxPartial_Parroquia_madre()
        {
            string IdPais = (Request.Params["IdPais_madre"] != null) ? Convert.ToString(Request.Params["IdPais_madre"]) : "";
            string Cod_Region = (Request.Params["Cod_Region_madre"] != null) ? Convert.ToString(Request.Params["Cod_Region_madre"]) : "";
            string IdProvincia = (Request.Params["IdProvincia_madre"] != null) ? Convert.ToString(Request.Params["IdProvincia_madre"]) : "";
            string IdCiudad = (Request.Params["IdCiudad_madre"] != null) ? Convert.ToString(Request.Params["IdCiudad_madre"]) : "";
            return PartialView("_ComboBoxPartial_Parroquia_madre", new aca_Alumno_Info { IdPais_madre = IdPais, Cod_Region_madre = Cod_Region, IdProvincia_madre = IdProvincia, IdCiudad_madre = IdCiudad });
        }
        #endregion
        #region Combos
        public ActionResult ComboBoxPartial_Pais()
        {
            return PartialView("_ComboBoxPartial_Pais", new aca_Alumno_Info());
        }
        public ActionResult ComboBoxPartial_Region()
        {
            string IdPais = (Request.Params["IdPais"] != null) ? Convert.ToString(Request.Params["IdPais"]) : "";
            return PartialView("_ComboBoxPartial_Region", new aca_Alumno_Info { IdPais = IdPais });
        }
        public ActionResult ComboBoxPartial_Provincia()
        {
            string IdPais = (Request.Params["IdPais"] != null) ? Convert.ToString(Request.Params["IdPais"]) : "";
            string Cod_Region= (Request.Params["Cod_Region"] != null) ? Convert.ToString(Request.Params["Cod_Region"]) : "";
            return PartialView("_ComboBoxPartial_Provincia", new aca_Alumno_Info { IdPais = IdPais, Cod_Region= Cod_Region });
        }
        public ActionResult ComboBoxPartial_Ciudad()
        {
            string IdPais = (Request.Params["IdPais"] != null) ? Convert.ToString(Request.Params["IdPais"]) : "";
            string Cod_Region = (Request.Params["Cod_Region"] != null) ? Convert.ToString(Request.Params["Cod_Region"]) : "";
            string IdProvincia = (Request.Params["IdProvincia"] != null) ? Convert.ToString(Request.Params["IdProvincia"]) : "";
            return PartialView("_ComboBoxPartial_Ciudad", new aca_Alumno_Info { IdPais = IdPais, Cod_Region = Cod_Region, IdProvincia= IdProvincia });
        }
        public ActionResult ComboBoxPartial_Parroquia()
        {
            string IdPais = (Request.Params["IdPais"] != null) ? Convert.ToString(Request.Params["IdPais"]) : "";
            string Cod_Region = (Request.Params["Cod_Region"] != null) ? Convert.ToString(Request.Params["Cod_Region"]) : "";
            string IdProvincia = (Request.Params["IdProvincia"] != null) ? Convert.ToString(Request.Params["IdProvincia"]) : "";
            string IdCiudad = (Request.Params["IdCiudad"] != null) ? Convert.ToString(Request.Params["IdCiudad"]) : "";
            return PartialView("_ComboBoxPartial_Parroquia", new aca_Alumno_Info { IdPais = IdPais, Cod_Region = Cod_Region, IdProvincia = IdProvincia, IdCiudad = IdCiudad });
        }

        public ActionResult cmb_parroquia_padre()
        {
            string IdCiudadPadre = (Request.Params["fx_IdCiudad_padre_fact"] != null) ? Request.Params["fx_IdCiudad_padre_fact"].ToString() : "";
            return PartialView("_cmb_parroquia_padre", new aca_Alumno_Info { IdCiudad = IdCiudadPadre });
        }
        public ActionResult cmb_parroquia_madre()
        {
            string IdCiudadMadre = (Request.Params["fx_IdCiudad_madre_fact"] != null) ? Request.Params["fx_IdCiudad_madre_fact"].ToString() : "";
            return PartialView("_cmb_parroquia_madre", new aca_Alumno_Info { IdCiudad = IdCiudadMadre });
        }
        #endregion

        #region Documentos
        [ValidateInput(false)]
        public ActionResult GridViewPartial_AlumnoDocumento()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;

            List<aca_AlumnoDocumento_Info> model = ListaAlumnoDocumento.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_AlumnoDocumento", model);
        }

        #region funciones del detalle
        [HttpPost, ValidateInput(false)]
        public ActionResult EditingAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] aca_AlumnoDocumento_Info info_det)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var Lista = ListaAlumnoDocumento.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            if (Lista.Where(q => q.IdDocumento == info_det.IdDocumento).ToList().Count == 0)
            {
                if (info_det.IdDocumento != 0)
                {
                    var info_documento = bus_documento.GetInfo(IdEmpresa, info_det.IdDocumento);
                    if (info_documento != null)
                    {
                        info_det.NomDocumento = info_documento.NomDocumento;
                    }
                }
                
                ListaAlumnoDocumento.AddRow(info_det, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            }

            var model = ListaAlumnoDocumento.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_AlumnoDocumento", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditingUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] aca_AlumnoDocumento_Info info_det)
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var Lista = ListaAlumnoDocumento.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            if (info_det != null && info_det.IdDocumento!=0)
            {
                ListaAlumnoDocumento.UpdateRow(info_det, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
                //if (Lista.Where(q => q.IdDocumento == info_det.IdDocumento).ToList().Count == 0)
                //{
                //    if (info_det.IdDocumento != 0)
                //    {
                //        var info_documento = bus_documento.GetInfo(IdEmpresa, info_det.IdDocumento);
                //        if (info_documento != null)
                //        {
                //            info_det.NomDocumento = info_documento.NomDocumento;
                //        }
                //    }

                //    ListaAlumnoDocumento.UpdateRow(info_det, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
                //}
            }
       
            var model = ListaAlumnoDocumento.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_AlumnoDocumento", model);
        }

        public ActionResult EditingDelete(int Secuencia)
        {
            ListaAlumnoDocumento.DeleteRow(Secuencia, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            var model = ListaAlumnoDocumento.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return PartialView("_GridViewPartial_AlumnoDocumento", model);
        }
        #endregion
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
            aca_Alumno_Info model = new aca_Alumno_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                pe_Naturaleza = "NATU",
                pe_Naturaleza_padre = "NATU",
                pe_Naturaleza_madre = "NATU",
                IdCatalogoFichaInst_madre = 25,
                IdCatalogoFichaInst_padre = 25,
                Idtipo_cliente_padre = 1,
                Idtipo_cliente_madre = 1,
                IdTipoCredito_padre = "CON",
                IdTipoCredito_madre = "CON",
                IdPais = "1",
                Cod_Region = "00001",
                IdProvincia = "09",
                IdCiudad = "09",
                IdParroquia = "09",
                IdCiudad_padre_fact = "09",
                IdParroquia_padre_fact = "09",
                IdCiudad_madre_fact = "09",
                IdParroquia_madre_fact = "09",
                CodCatalogoCONADIS = "",
                CodCatalogoSangre = "O+",
                CodCatalogoSangre_padre = "O+",
                CodCatalogoSangre_madre = "O+",
                IdPersona_padre = 0,
                FechaIngreso = DateTime.Now,
                alu_foto = new byte[0],
                IdPais_padre = "1",
                Cod_Region_padre = "00001",
                IdProvincia_padre = "09",
                IdCiudad_padre = "09",
                IdParroquia_padre = "09",
                IdPais_madre = "1",
                Cod_Region_madre = "00001",
                IdProvincia_madre = "09",
                IdCiudad_madre = "09",
                IdParroquia_madre = "09",
                Dificultad_Escritura = false,
                Dificultad_Lectura = false,
                Dificultad_Matematicas = false,
                lst_alumno_documentos = new List<aca_AlumnoDocumento_Info>()
            };

            cargar_combos();

            #region Permisos
            aca_Menu_x_seg_usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(SessionFixed.IdSede), SessionFixed.IdUsuario, "Academico", "Alumno", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            return View(model);
        }
        [HttpPost]
        public ActionResult Nuevo(aca_Alumno_Info model)
        {
            model.IdUsuarioCreacion = SessionFixed.IdUsuario;
            model.IdUsuario = SessionFixed.IdUsuario;
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            model.IdSucursal = bus_sede.GetInfo(model.IdEmpresa, model.IdSede).IdSucursal;
            var infoAnioActual = bus_anio.GetInfo_AnioEnCurso(Convert.ToInt32(SessionFixed.IdEmpresa), 0);
            model.IdAnioRegistro = infoAnioActual.IdAnio;

            var info_persona_alumno = new tb_persona_Info
            {
                IdPersona = model.IdPersona,
                pe_Naturaleza = model.pe_Naturaleza,
                IdTipoDocumento = model.IdTipoDocumento,
                pe_cedulaRuc = model.pe_cedulaRuc,
                pe_nombre = model.pe_nombre,
                pe_apellido = model.pe_apellido,
                pe_nombreCompleto = model.pe_nombreCompleto,
                pe_razonSocial = model.pe_nombreCompleto,
                pe_sexo = model.pe_sexo,
                CodCatalogoSangre = model.CodCatalogoSangre,
                CodCatalogoCONADIS = model.CodCatalogoCONADIS,
                NumeroCarnetConadis =model.NumeroCarnetConadis,
                PorcentajeDiscapacidad = model.PorcentajeDiscapacidad,
                pe_fechaNacimiento = model.pe_fechaNacimiento,
                pe_telfono_Contacto = model.pe_telfono_Contacto,
                pe_correo = model.Correo,
                pe_celular = model.Celular,
                pe_direccion = model.Direccion,
                IdReligion = model.IdReligion,
                AsisteCentroCristiano = model.AsisteCentroCristiano,
                IdGrupoEtnico = model.IdGrupoEtnico
            };

            model.lst_alumno_documentos = ListaAlumnoDocumento.get_list(model.IdTransaccionSession);
            model.IdCatalogoESTALU = Convert.ToInt32(cl_enumeradores.eCatalogoAcademicoAlumno.PROMOVIDO);
            model.IdCatalogoESTMAT = Convert.ToInt32(cl_enumeradores.eCatalogoAcademicoMatricula.REGISTRADO);
            model.info_persona_alumno = info_persona_alumno;
            model.info_persona_padre = armar_info_padre(model);
            model.info_persona_madre = armar_info_madre(model);

            if (model.alu_foto == null)
                model.alu_foto = new byte[0];

            if (!validar(model, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                cargar_combos();
                return View(model);
            }

            if (!bus_alumno.GuardarDB(model))
            {
                ViewBag.mensaje = "No se ha podido guardar el registro";
                cargar_combos();
                return View(model);
            }

            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdAlumno = model.IdAlumno, Exito = true });
        }

        public ActionResult Consultar(int IdEmpresa = 0, int IdAlumno = 0, bool Exito = false)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            aca_Alumno_Info model = bus_alumno.GetInfo(IdEmpresa, IdAlumno);

            model.IdPais = (model.IdPais == "" || model.IdPais == null ? "1" : model.IdPais);
            model.Cod_Region = (model.IdPais == "" || model.Cod_Region == null ? "00001" : model.Cod_Region);
            model.IdProvincia = (model.IdProvincia == "" || model.IdProvincia == null ? "09" : model.IdProvincia);
            model.IdCiudad = (model.IdCiudad == "" || model.IdCiudad == null ? "09" : model.IdCiudad);
            model.IdParroquia = (model.IdParroquia == "" || model.IdParroquia == null ? "09" : model.IdParroquia);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            aca_Menu_x_seg_usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(SessionFixed.IdSede), SessionFixed.IdUsuario, "Academico", "Alumno", "Index");
            if (model.Estado==false)
            {
                info.Modificar = false;
                info.Anular = false;
            }
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            model.IdTransaccionSession = Convert.ToInt32(SessionFixed.IdTransaccionSessionActual);
            model.lst_alumno_documentos = bus_alumno_documento.GetList(IdEmpresa, IdAlumno, false);
            ListaAlumnoDocumento.set_list(model.lst_alumno_documentos, model.IdTransaccionSession);

            aca_Familia_Info info_fam_padre = bus_familia.GetListTipo(IdEmpresa, IdAlumno, Convert.ToInt32(cl_enumeradores.eTipoParentezco.PAPA));
            aca_Familia_Info info_fam_madre = bus_familia.GetListTipo(IdEmpresa, IdAlumno, Convert.ToInt32(cl_enumeradores.eTipoParentezco.MAMA));
            model.CodCatalogoCONADIS = (model.CodCatalogoCONADIS == null ? "" : model.CodCatalogoCONADIS);

            if (model.alu_foto == null)
                model.alu_foto = new byte[0];

            try
            {
                var info_parametro = bus_parametro.get_info(model.IdEmpresa);
                var Ruta = (info_parametro == null ? "" : info_parametro.RutaImagen_Alumno);
                model.alu_foto = System.IO.File.ReadAllBytes(Server.MapPath(Ruta) +model.IdAlumno.ToString() + ".jpg");
                if (model.alu_foto == null)
                    model.alu_foto = new byte[0];
            }
            catch (Exception)
            {

                model.alu_foto = new byte[0];
            }


            if (info_fam_padre == null)
            {
                info_fam_padre = new aca_Familia_Info
                {
                    IdCiudad_fact = "09",
                    IdParroquia_fact = "09",
                    IdPais = "1",
                    Cod_Region = "00001",
                    IdProvincia = "09",
                    IdCiudad = "09",
                    IdParroquia = "09"
                };
            }
            else
            {
                var existe_cliente_padre = bus_cliente.get_info_x_num_cedula(model.IdEmpresa, info_fam_padre.pe_cedulaRuc);
                var cliente = bus_cliente.get_info(model.IdEmpresa, existe_cliente_padre.IdCliente);
                info_fam_padre.IdTipoCredito = ((cliente == null || cliente.IdTipoCredito == "") ? "CON" : cliente.IdTipoCredito);
                info_fam_padre.Idtipo_cliente = ((cliente == null || cliente.Idtipo_cliente == 0) ? 1 : cliente.Idtipo_cliente);
                var IdClientePadre = ((cliente == null || cliente.IdCliente == 0) ? 0 : cliente.IdCliente);
                var info_contacto_padre = bus_cliente_cont.get_info(model.IdEmpresa, IdClientePadre, 1);
                info_fam_padre.IdCiudad_fact = (info_contacto_padre == null ? "09" : info_contacto_padre.IdCiudad);
                info_fam_padre.IdParroquia_fact = (info_contacto_padre == null ? "09" : info_contacto_padre.IdParroquia);
            }

            if (info_fam_madre == null)
            {
                info_fam_madre = new aca_Familia_Info
                {
                    IdCiudad_fact = "09",
                    IdParroquia_fact = "09",
                    IdPais = "1",
                    Cod_Region = "00001",
                    IdProvincia = "09",
                    IdCiudad = "09",
                    IdParroquia = "09"
                };
            }
            else
            {
                var existe_cliente_madre = bus_cliente.get_info_x_num_cedula(model.IdEmpresa, info_fam_madre.pe_cedulaRuc);
                var cliente = bus_cliente.get_info(model.IdEmpresa, existe_cliente_madre.IdCliente);
                info_fam_madre.IdTipoCredito = ((cliente == null || cliente.IdTipoCredito == "") ? "CON" : cliente.IdTipoCredito);
                info_fam_madre.Idtipo_cliente = ((cliente == null || cliente.Idtipo_cliente == 0) ? 1 : cliente.Idtipo_cliente);
                var IdClienteMadre = ((cliente == null || cliente.IdCliente == 0) ? 0 : cliente.IdCliente);
                var info_contacto_madre = bus_cliente_cont.get_info(model.IdEmpresa, IdClienteMadre, 1);
                info_fam_madre.IdCiudad_fact = (info_contacto_madre == null ? "09" : info_contacto_madre.IdCiudad);
                info_fam_madre.IdParroquia_fact = (info_contacto_madre == null ? "09" : info_contacto_madre.IdParroquia);
            }

            model.IdPersona_padre = info_fam_padre.IdPersona;
            model.SeFactura_padre = info_fam_padre.SeFactura;
            model.EsRepresentante_padre = info_fam_padre.EsRepresentante;
            model.IdTipoDocumento_padre = info_fam_padre.IdTipoDocumento;
            model.pe_Naturaleza_padre = info_fam_padre.pe_Naturaleza;
            model.pe_cedulaRuc_padre = info_fam_padre.pe_cedulaRuc;
            model.pe_nombre_padre = info_fam_padre.pe_nombre;
            model.pe_apellido_padre = info_fam_padre.pe_apellido;
            model.pe_nombreCompleto_padre = info_fam_padre.pe_nombreCompleto;
            model.pe_razonSocial_padre = info_fam_padre.pe_razonSocial;
            model.pe_fechaNacimiento_padre = info_fam_padre.pe_fechaNacimiento;
            model.pe_sexo_padre = info_fam_padre.pe_sexo;
            model.IdEstadoCivil_padre = info_fam_padre.IdEstadoCivil;
            model.pe_telfono_Contacto_padre = info_fam_padre.Telefono;
            model.Celular_padre = info_fam_padre.Celular;
            model.Correo_padre = info_fam_padre.Correo;
            model.Direccion_padre = info_fam_padre.Direccion;
            model.PorcentajeDiscapacidad_padre = info_fam_padre.PorcentajeDiscapacidad;
            model.NumeroCarnetConadis_padre = info_fam_padre.NumeroCarnetConadis;
            model.CodCatalogoCONADIS_padre = (info_fam_padre == null || info_fam_padre.CodCatalogoCONADIS == null ? "" : info_fam_padre.CodCatalogoCONADIS);
            model.IdProfesion_padre = info_fam_padre.IdProfesion;
            model.IdCatalogoFichaInst_padre = info_fam_padre.IdCatalogoFichaInst;
            model.EmpresaTrabajo_padre = info_fam_padre.EmpresaTrabajo;
            model.DireccionTrabajo_padre = info_fam_padre.DireccionTrabajo;
            model.TelefonoTrabajo_padre = info_fam_padre.TelefonoTrabajo;
            model.CargoTrabajo_padre = info_fam_padre.CargoTrabajo;
            model.AniosServicio_padre = info_fam_padre.AniosServicio;
            model.IngresoMensual_padre = info_fam_padre.IngresoMensual;
            model.VehiculoPropio_padre = info_fam_padre.VehiculoPropio;
            model.Marca_padre = info_fam_padre.Marca;
            model.Modelo_padre = info_fam_padre.Modelo;
            model.AnioVehiculo_padre = info_fam_padre.AnioVehiculo;
            model.CasaPropia_padre = info_fam_padre.CasaPropia;
            model.IdReligion_padre = info_fam_padre.IdReligion;
            model.AsisteCentroCristiano_padre = info_fam_padre.AsisteCentroCristiano;
            model.EstaFallecido_padre = info_fam_padre.EstaFallecido;
            model.IdTipoCredito_padre = (info_fam_padre.IdTipoCredito == null ? "CON" : info_fam_padre.IdTipoCredito);
            model.Idtipo_cliente_padre = (info_fam_padre.Idtipo_cliente == 0 ? 1 : info_fam_padre.Idtipo_cliente);
            model.IdCiudad_padre_fact = info_fam_padre.IdCiudad_fact;
            model.IdParroquia_padre_fact = info_fam_padre.IdParroquia_fact;
            model.IdPais_padre = info_fam_padre.IdPais;
            model.Cod_Region_padre = info_fam_padre.Cod_Region;
            model.IdProvincia_padre = info_fam_padre.IdProvincia;
            model.IdCiudad_padre = info_fam_padre.IdCiudad;
            model.IdParroquia_padre = info_fam_padre.IdParroquia;
            model.Sector_padre = info_fam_padre.Sector;
            model.IdGrupoEtnico_padre = info_fam_padre.IdGrupoEtnico;

            model.IdPersona_madre = info_fam_madre.IdPersona;
            model.SeFactura_madre = info_fam_madre.SeFactura;
            model.EsRepresentante_madre = info_fam_madre.EsRepresentante;
            model.IdTipoDocumento_madre = info_fam_madre.IdTipoDocumento;
            model.pe_Naturaleza_madre = info_fam_madre.pe_Naturaleza;
            model.pe_cedulaRuc_madre = info_fam_madre.pe_cedulaRuc;
            model.pe_nombre_madre = info_fam_madre.pe_nombre;
            model.pe_apellido_madre = info_fam_madre.pe_apellido;
            model.pe_nombreCompleto_madre = info_fam_madre.pe_nombreCompleto;
            model.pe_razonSocial_madre = info_fam_madre.pe_razonSocial;
            model.pe_fechaNacimiento_madre = info_fam_madre.pe_fechaNacimiento;
            model.pe_sexo_madre = info_fam_madre.pe_sexo;
            model.IdEstadoCivil_madre = info_fam_madre.IdEstadoCivil;
            model.pe_telfono_Contacto_madre = info_fam_madre.Telefono;
            model.Celular_madre = info_fam_madre.Celular;
            model.Correo_madre = info_fam_madre.Correo;
            model.Direccion_madre = info_fam_madre.Direccion;
            model.PorcentajeDiscapacidad_madre = info_fam_madre.PorcentajeDiscapacidad;
            model.NumeroCarnetConadis_madre = info_fam_madre.NumeroCarnetConadis;
            model.CodCatalogoCONADIS_madre = (info_fam_madre == null || info_fam_madre.CodCatalogoCONADIS == null ? "" : info_fam_madre.CodCatalogoCONADIS);
            model.IdProfesion_madre = info_fam_madre.IdProfesion;
            model.IdCatalogoFichaInst_madre = info_fam_madre.IdCatalogoFichaInst;
            model.EmpresaTrabajo_madre = info_fam_madre.EmpresaTrabajo;
            model.DireccionTrabajo_madre = info_fam_madre.DireccionTrabajo;
            model.TelefonoTrabajo_madre = info_fam_madre.TelefonoTrabajo;
            model.CargoTrabajo_madre = info_fam_madre.CargoTrabajo;
            model.AniosServicio_madre = info_fam_madre.AniosServicio;
            model.IngresoMensual_madre = info_fam_madre.IngresoMensual;
            model.VehiculoPropio_madre = info_fam_madre.VehiculoPropio;
            model.Marca_madre = info_fam_madre.Marca;
            model.Modelo_madre = info_fam_madre.Modelo;
            model.AnioVehiculo_madre = info_fam_madre.AnioVehiculo;
            model.CasaPropia_madre = info_fam_madre.CasaPropia;
            model.IdReligion_madre = info_fam_madre.IdReligion;
            model.AsisteCentroCristiano_madre = info_fam_madre.AsisteCentroCristiano;
            model.EstaFallecido_madre = info_fam_madre.EstaFallecido;
            model.IdTipoCredito_madre = (info_fam_madre.IdTipoCredito == null ? "CON" : info_fam_madre.IdTipoCredito);
            model.Idtipo_cliente_madre = (info_fam_madre.Idtipo_cliente == 0 ? 1 : info_fam_madre.Idtipo_cliente);
            model.IdCiudad_madre_fact = info_fam_madre.IdCiudad_fact;
            model.IdParroquia_madre_fact = info_fam_madre.IdParroquia_fact;
            model.IdPais_madre = info_fam_madre.IdPais;
            model.Cod_Region_madre = info_fam_madre.Cod_Region;
            model.IdProvincia_madre = info_fam_madre.IdProvincia;
            model.IdCiudad_madre = info_fam_madre.IdCiudad;
            model.IdParroquia_madre = info_fam_madre.IdParroquia;
            model.Sector_madre = info_fam_madre.Sector;
            model.IdGrupoEtnico_madre = info_fam_madre.IdGrupoEtnico;

            if (model == null)
                return RedirectToAction("Index");

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            cargar_combos();
            return View(model);
        }


        public ActionResult Modificar(int IdEmpresa = 0, int IdAlumno = 0, bool Exito = false)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            aca_Alumno_Info model = bus_alumno.GetInfo(IdEmpresa, IdAlumno);

            model.IdPais = (model.IdPais == "" || model.IdPais == null ? "1" : model.IdPais);
            model.Cod_Region = (model.IdPais == "" || model.Cod_Region == null ? "00001" : model.Cod_Region); 
            model.IdProvincia = (model.IdProvincia == "" || model.IdProvincia == null ? "09" : model.IdProvincia);
            model.IdCiudad = (model.IdCiudad == "" || model.IdCiudad == null ? "09" : model.IdCiudad);
            model.IdParroquia = (model.IdParroquia == "" || model.IdParroquia == null ? "09" : model.IdParroquia);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            aca_Menu_x_seg_usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(SessionFixed.IdSede), SessionFixed.IdUsuario, "Academico", "Alumno", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            model.IdTransaccionSession = Convert.ToInt32(SessionFixed.IdTransaccionSessionActual);
            model.lst_alumno_documentos = bus_alumno_documento.GetList(IdEmpresa, IdAlumno, false);
            ListaAlumnoDocumento.set_list(model.lst_alumno_documentos, model.IdTransaccionSession);

            aca_Familia_Info info_fam_padre = bus_familia.GetListTipo(IdEmpresa, IdAlumno, Convert.ToInt32(cl_enumeradores.eTipoParentezco.PAPA));
            aca_Familia_Info info_fam_madre = bus_familia.GetListTipo(IdEmpresa, IdAlumno, Convert.ToInt32(cl_enumeradores.eTipoParentezco.MAMA));
            model.CodCatalogoCONADIS = (model.CodCatalogoCONADIS == null ? "" : model.CodCatalogoCONADIS);

            if (model.alu_foto == null)
                model.alu_foto = new byte[0];

            try
            {

                var info_parametro = bus_parametro.get_info(model.IdEmpresa);
                var Ruta = (info_parametro == null ? "" : info_parametro.RutaImagen_Alumno);
                model.alu_foto = System.IO.File.ReadAllBytes(Server.MapPath(Ruta) + model.IdAlumno.ToString() + ".jpg");
                if (model.alu_foto == null)
                    model.alu_foto = new byte[0];
            }
            catch (Exception)
            {

                model.alu_foto = new byte[0];
            }


            if (info_fam_padre == null)
            {
                info_fam_padre = new aca_Familia_Info
                {
                    IdCiudad_fact = "09",
                    IdParroquia_fact = "09",
                    IdPais = "1",
                    Cod_Region = "00001",
                    IdProvincia = "09",
                    IdCiudad = "09",
                    IdParroquia = "09"
                };
            }
            else
            {
                var existe_cliente_padre = bus_cliente.get_info_x_num_cedula(model.IdEmpresa, info_fam_padre.pe_cedulaRuc);
                var cliente = bus_cliente.get_info(model.IdEmpresa, existe_cliente_padre.IdCliente);
                info_fam_padre.IdTipoCredito = ((cliente == null || cliente.IdTipoCredito == "") ? "CON" : cliente.IdTipoCredito);
                info_fam_padre.Idtipo_cliente = ((cliente == null || cliente.Idtipo_cliente == 0 )? 1 : cliente.Idtipo_cliente);
                var IdClientePadre = ((cliente == null || cliente.IdCliente == 0) ? 0 : cliente.IdCliente);
                var info_contacto_padre = bus_cliente_cont.get_info(model.IdEmpresa, IdClientePadre, 1);
                info_fam_padre.IdCiudad_fact = (info_contacto_padre == null ? "09" : info_contacto_padre.IdCiudad);
                info_fam_padre.IdParroquia_fact = (info_contacto_padre == null ? "09" : info_contacto_padre.IdParroquia);
            }

            if (info_fam_madre == null)
            {
                info_fam_madre = new aca_Familia_Info
                {
                    IdCiudad_fact = "09",
                    IdParroquia_fact = "09",
                    IdPais = "1",
                    Cod_Region = "00001",
                    IdProvincia = "09",
                    IdCiudad = "09",
                    IdParroquia = "09"
                };
            }
            else
            {
                var existe_cliente_madre = bus_cliente.get_info_x_num_cedula(model.IdEmpresa, info_fam_madre.pe_cedulaRuc);
                var cliente = bus_cliente.get_info(model.IdEmpresa, existe_cliente_madre.IdCliente);
                info_fam_madre.IdTipoCredito = ((cliente == null || cliente.IdTipoCredito == "") ? "CON" : cliente.IdTipoCredito);
                info_fam_madre.Idtipo_cliente = ((cliente == null || cliente.Idtipo_cliente == 0) ? 1 : cliente.Idtipo_cliente);
                var IdClienteMadre = ((cliente == null || cliente.IdCliente == 0) ? 0 : cliente.IdCliente);
                var info_contacto_madre = bus_cliente_cont.get_info(model.IdEmpresa, IdClienteMadre, 1);
                info_fam_madre.IdCiudad_fact = (info_contacto_madre == null ? "09" : info_contacto_madre.IdCiudad);
                info_fam_madre.IdParroquia_fact = (info_contacto_madre == null ? "09" : info_contacto_madre.IdParroquia);
            }

            model.IdPersona_padre = info_fam_padre.IdPersona;
            model.SeFactura_padre = info_fam_padre.SeFactura;
            model.EsRepresentante_padre = info_fam_padre.EsRepresentante;
            model.IdTipoDocumento_padre = info_fam_padre.IdTipoDocumento;
            model.pe_Naturaleza_padre = info_fam_padre.pe_Naturaleza;
            model.pe_cedulaRuc_padre = info_fam_padre.pe_cedulaRuc;
            model.pe_nombre_padre = info_fam_padre.pe_nombre;
            model.pe_apellido_padre = info_fam_padre.pe_apellido;
            model.pe_nombreCompleto_padre = info_fam_padre.pe_nombreCompleto;
            model.pe_razonSocial_padre = info_fam_padre.pe_razonSocial;
            model.pe_fechaNacimiento_padre = info_fam_padre.pe_fechaNacimiento;
            model.pe_sexo_padre = info_fam_padre.pe_sexo;
            model.IdEstadoCivil_padre = info_fam_padre.IdEstadoCivil;
            model.pe_telfono_Contacto_padre = info_fam_padre.Telefono;
            model.Celular_padre = info_fam_padre.Celular;
            model.Correo_padre = info_fam_padre.Correo;
            model.Direccion_padre = info_fam_padre.Direccion;
            model.PorcentajeDiscapacidad_padre = info_fam_padre.PorcentajeDiscapacidad;
            model.NumeroCarnetConadis_padre = info_fam_padre.NumeroCarnetConadis;
            model.CodCatalogoCONADIS_padre = (info_fam_padre == null || info_fam_padre.CodCatalogoCONADIS == null ? "" : info_fam_padre.CodCatalogoCONADIS);
            model.IdProfesion_padre = info_fam_padre.IdProfesion;
            model.IdCatalogoFichaInst_padre = info_fam_padre.IdCatalogoFichaInst;
            model.EmpresaTrabajo_padre = info_fam_padre.EmpresaTrabajo;
            model.DireccionTrabajo_padre = info_fam_padre.DireccionTrabajo;
            model.TelefonoTrabajo_padre = info_fam_padre.TelefonoTrabajo;
            model.CargoTrabajo_padre = info_fam_padre.CargoTrabajo;
            model.AniosServicio_padre = info_fam_padre.AniosServicio;
            model.IngresoMensual_padre = info_fam_padre.IngresoMensual;
            model.VehiculoPropio_padre = info_fam_padre.VehiculoPropio;
            model.Marca_padre = info_fam_padre.Marca;
            model.Modelo_padre = info_fam_padre.Modelo;
            model.AnioVehiculo_padre = info_fam_padre.AnioVehiculo;
            model.CasaPropia_padre = info_fam_padre.CasaPropia;
            model.IdReligion_padre = info_fam_padre.IdReligion;
            model.AsisteCentroCristiano_padre = info_fam_padre.AsisteCentroCristiano;
            model.EstaFallecido_padre = info_fam_padre.EstaFallecido;
            model.IdTipoCredito_padre = (info_fam_padre.IdTipoCredito == null ? "CON" : info_fam_padre.IdTipoCredito);
            model.Idtipo_cliente_padre = (info_fam_padre.Idtipo_cliente == 0 ? 1 : info_fam_padre.Idtipo_cliente);
            model.IdCiudad_padre_fact = info_fam_padre.IdCiudad_fact;
            model.IdParroquia_padre_fact = info_fam_padre.IdParroquia_fact;
            model.IdPais_padre = info_fam_padre.IdPais;
            model.Cod_Region_padre = info_fam_padre.Cod_Region;
            model.IdProvincia_padre = info_fam_padre.IdProvincia;
            model.IdCiudad_padre = info_fam_padre.IdCiudad;
            model.IdParroquia_padre = info_fam_padre.IdParroquia;
            model.Sector_padre = info_fam_padre.Sector;

            model.IdPersona_madre = info_fam_madre.IdPersona;
            model.SeFactura_madre = info_fam_madre.SeFactura;
            model.EsRepresentante_madre = info_fam_madre.EsRepresentante;
            model.IdTipoDocumento_madre = info_fam_madre.IdTipoDocumento;
            model.pe_Naturaleza_madre = info_fam_madre.pe_Naturaleza;
            model.pe_cedulaRuc_madre = info_fam_madre.pe_cedulaRuc;
            model.pe_nombre_madre = info_fam_madre.pe_nombre;
            model.pe_apellido_madre = info_fam_madre.pe_apellido;
            model.pe_nombreCompleto_madre = info_fam_madre.pe_nombreCompleto;
            model.pe_razonSocial_madre = info_fam_madre.pe_razonSocial;
            model.pe_fechaNacimiento_madre = info_fam_madre.pe_fechaNacimiento;
            model.pe_sexo_madre = info_fam_madre.pe_sexo;
            model.IdEstadoCivil_madre = info_fam_madre.IdEstadoCivil;
            model.pe_telfono_Contacto_madre = info_fam_madre.Telefono;
            model.Celular_madre = info_fam_madre.Celular;
            model.Correo_madre = info_fam_madre.Correo;
            model.Direccion_madre = info_fam_madre.Direccion;
            model.PorcentajeDiscapacidad_madre = info_fam_madre.PorcentajeDiscapacidad;
            model.NumeroCarnetConadis_madre = info_fam_madre.NumeroCarnetConadis;
            model.CodCatalogoCONADIS_madre = (info_fam_madre == null || info_fam_madre.CodCatalogoCONADIS == null ? "" : info_fam_madre.CodCatalogoCONADIS);
            model.IdProfesion_madre = info_fam_madre.IdProfesion;
            model.IdCatalogoFichaInst_madre = info_fam_madre.IdCatalogoFichaInst;
            model.EmpresaTrabajo_madre = info_fam_madre.EmpresaTrabajo;
            model.DireccionTrabajo_madre = info_fam_madre.DireccionTrabajo;
            model.TelefonoTrabajo_madre = info_fam_madre.TelefonoTrabajo;
            model.CargoTrabajo_madre = info_fam_madre.CargoTrabajo;
            model.AniosServicio_madre = info_fam_madre.AniosServicio;
            model.IngresoMensual_madre = info_fam_madre.IngresoMensual;
            model.VehiculoPropio_madre = info_fam_madre.VehiculoPropio;
            model.Marca_madre = info_fam_madre.Marca;
            model.Modelo_madre = info_fam_madre.Modelo;
            model.AnioVehiculo_madre = info_fam_madre.AnioVehiculo;
            model.CasaPropia_madre = info_fam_madre.CasaPropia;
            model.IdReligion_madre = info_fam_madre.IdReligion;
            model.AsisteCentroCristiano_madre = info_fam_madre.AsisteCentroCristiano;
            model.EstaFallecido_madre = info_fam_madre.EstaFallecido;
            model.IdTipoCredito_madre = (info_fam_madre.IdTipoCredito == null ? "CON" : info_fam_madre.IdTipoCredito);
            model.Idtipo_cliente_madre = (info_fam_madre.Idtipo_cliente == 0 ? 1 : info_fam_madre.Idtipo_cliente);
            model.IdCiudad_madre_fact = info_fam_madre.IdCiudad_fact;
            model.IdParroquia_madre_fact = info_fam_madre.IdParroquia_fact;
            model.IdPais_madre = info_fam_madre.IdPais;
            model.Cod_Region_madre = info_fam_madre.Cod_Region;
            model.IdProvincia_madre = info_fam_madre.IdProvincia;
            model.IdCiudad_madre = info_fam_madre.IdCiudad;
            model.IdParroquia_madre = info_fam_madre.IdParroquia;
            model.Sector_madre = info_fam_madre.Sector;

            if (model == null)
                return RedirectToAction("Index");

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(aca_Alumno_Info model)
        {
            model.IdUsuarioModificacion = SessionFixed.IdUsuario;
            model.IdUsuario = SessionFixed.IdUsuario;
            model.IdSede = Convert.ToInt32(SessionFixed.IdSede);
            model.IdSucursal = bus_sede.GetInfo(model.IdEmpresa, model.IdSede).IdSucursal;

            var info_persona_alumno = new tb_persona_Info
            {
                IdPersona = model.IdPersona,
                pe_Naturaleza = model.pe_Naturaleza,
                IdTipoDocumento = model.IdTipoDocumento,
                pe_cedulaRuc = model.pe_cedulaRuc,
                pe_nombre = model.pe_nombre,
                pe_apellido = model.pe_apellido,
                pe_nombreCompleto = model.pe_nombreCompleto,
                pe_razonSocial = model.pe_nombreCompleto,
                pe_sexo = model.pe_sexo,
                CodCatalogoSangre = model.CodCatalogoSangre,
                CodCatalogoCONADIS = model.CodCatalogoCONADIS,
                NumeroCarnetConadis = model.NumeroCarnetConadis,
                PorcentajeDiscapacidad = model.PorcentajeDiscapacidad,
                pe_fechaNacimiento = model.pe_fechaNacimiento,
                pe_telfono_Contacto = model.pe_telfono_Contacto,
                pe_correo = model.Correo,
                pe_celular = model.Celular,
                pe_direccion = model.Direccion,
                IdReligion = model.IdReligion,
                AsisteCentroCristiano = model.AsisteCentroCristiano,
                IdGrupoEtnico = model.IdGrupoEtnico
            };

            model.info_persona_alumno = info_persona_alumno;
            model.info_persona_padre = armar_info_padre(model);
            model.info_persona_madre = armar_info_madre(model);
            model.lst_alumno_documentos = ListaAlumnoDocumento.get_list(model.IdTransaccionSession);

            if (model.alu_foto == null)
                model.alu_foto = new byte[0];

            if (!validar(model, ref mensaje))
            {
                ViewBag.mensaje = mensaje;
                cargar_combos();
                return View(model);
            }

            if (!bus_alumno.ModificarDB(model))
            {
                ViewBag.mensaje = "No se ha podido modificar el registro";
                cargar_combos();
                return View(model);
            }

            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdAlumno = model.IdAlumno, Exito = true });
        }

        public ActionResult Anular(int IdEmpresa = 0, int IdAlumno = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            aca_Alumno_Info model = bus_alumno.GetInfo(IdEmpresa, IdAlumno);
            if(model == null)
                return RedirectToAction("Index");

            #region Permisos
            aca_Menu_x_seg_usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), Convert.ToInt32(SessionFixed.IdSede), SessionFixed.IdUsuario, "Academico", "Alumno", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion
            model.IdTransaccionSession = Convert.ToInt32(SessionFixed.IdTransaccionSessionActual);
            model.lst_alumno_documentos = bus_alumno_documento.GetList(IdEmpresa, IdAlumno, false);
            ListaAlumnoDocumento.set_list(model.lst_alumno_documentos, model.IdTransaccionSession);

            aca_Familia_Info info_fam_padre = bus_familia.GetListTipo(IdEmpresa, IdAlumno, Convert.ToInt32(cl_enumeradores.eTipoParentezco.PAPA));
            aca_Familia_Info info_fam_madre = bus_familia.GetListTipo(IdEmpresa, IdAlumno, Convert.ToInt32(cl_enumeradores.eTipoParentezco.MAMA));

            model.CodCatalogoCONADIS = (model.CodCatalogoCONADIS == null ? "" : model.CodCatalogoCONADIS);

            if (model.alu_foto == null)
                model.alu_foto = new byte[0];

            if (model.alu_foto == null)
                model.alu_foto = new byte[0];

            try
            {

                //model.alu_foto = System.IO.File.ReadAllBytes(Server.MapPath(UploadDirectory) + model.IdEmpresa.ToString("000") + model.IdAlumno.ToString("000000") + ".jpg");
                var info_parametro = bus_parametro.get_info(model.IdEmpresa);
                var Ruta = (info_parametro == null ? "" : info_parametro.RutaImagen_Alumno);
                model.alu_foto = System.IO.File.ReadAllBytes(Server.MapPath(Ruta) + model.IdAlumno.ToString() + ".jpg");
            }
            catch (Exception)
            {

                model.alu_foto = new byte[0];
            }

            if (info_fam_padre == null)
            {
                info_fam_padre = new aca_Familia_Info();
            }
            else
            {
                var existe_cliente_padre = bus_cliente.get_info_x_num_cedula(model.IdEmpresa, info_fam_padre.pe_cedulaRuc);
                var cliente = bus_cliente.get_info(model.IdEmpresa, existe_cliente_padre.IdCliente);
                info_fam_padre.IdTipoCredito = ((cliente == null || cliente.IdTipoCredito == "") ? "CON" : cliente.IdTipoCredito);
                info_fam_padre.Idtipo_cliente = ((cliente == null || cliente.Idtipo_cliente == 0) ? 1 : cliente.Idtipo_cliente);
                var IdClientePadre = ((cliente == null || cliente.IdCliente == 0) ? 0 : cliente.IdCliente);
                var info_contacto_padre = bus_cliente_cont.get_info(model.IdEmpresa, IdClientePadre, 1);
                info_fam_padre.IdCiudad_fact = (info_contacto_padre == null ? "09" : info_contacto_padre.IdCiudad);
                info_fam_padre.IdParroquia_fact = (info_contacto_padre == null ? "09" : info_contacto_padre.IdParroquia);
            }

            if (info_fam_madre == null)
            {
                info_fam_madre = new aca_Familia_Info();
            }
            else
            {
                var existe_cliente_madre = bus_cliente.get_info_x_num_cedula(model.IdEmpresa, info_fam_madre.pe_cedulaRuc);
                var cliente = bus_cliente.get_info(model.IdEmpresa, existe_cliente_madre.IdCliente);
                info_fam_madre.IdTipoCredito = ((cliente == null || cliente.IdTipoCredito == "") ? "CON" : cliente.IdTipoCredito);
                info_fam_madre.Idtipo_cliente = ((cliente == null || cliente.Idtipo_cliente == 0) ? 1 : cliente.Idtipo_cliente);
                var IdClienteMadre = ((cliente == null || cliente.IdCliente == 0) ? 0 : cliente.IdCliente);
                var info_contacto_madre = bus_cliente_cont.get_info(model.IdEmpresa, IdClienteMadre, 1);
                info_fam_madre.IdCiudad_fact = (info_contacto_madre == null ? "09" : info_contacto_madre.IdCiudad);
                info_fam_madre.IdParroquia_fact = (info_contacto_madre == null ? "09" : info_contacto_madre.IdParroquia);
            }

            model.IdPersona_padre = info_fam_padre.IdPersona;
            model.SeFactura_padre = info_fam_padre.SeFactura;
            model.IdTipoDocumento_padre = info_fam_padre.IdTipoDocumento;
            model.pe_Naturaleza_padre = info_fam_padre.pe_Naturaleza;
            model.pe_cedulaRuc_padre = info_fam_padre.pe_cedulaRuc;
            model.pe_nombre_padre = info_fam_padre.pe_nombre;
            model.pe_apellido_padre = info_fam_padre.pe_apellido;
            model.pe_fechaNacimiento_padre = info_fam_padre.pe_fechaNacimiento;
            model.pe_sexo_padre = info_fam_padre.pe_sexo;
            model.IdEstadoCivil_padre = info_fam_padre.IdEstadoCivil;
            model.pe_telfono_Contacto_padre = info_fam_padre.Telefono;
            model.Celular_padre = info_fam_padre.Celular;
            model.Correo_padre = info_fam_padre.Correo;
            model.Direccion_padre = info_fam_padre.Direccion;
            model.PorcentajeDiscapacidad_padre = info_fam_padre.PorcentajeDiscapacidad;
            model.NumeroCarnetConadis_padre = info_fam_padre.NumeroCarnetConadis;
            model.CodCatalogoCONADIS_padre = (info_fam_padre.CodCatalogoCONADIS == null ? "" : info_fam_padre.CodCatalogoCONADIS);
            model.IdProfesion_padre = info_fam_padre.IdProfesion;
            model.IdReligion_padre = info_fam_padre.IdReligion;
            model.AsisteCentroCristiano_padre = info_fam_padre.AsisteCentroCristiano;
            model.EstaFallecido_padre = info_fam_padre.EstaFallecido;
            model.IdTipoCredito_padre = (info_fam_padre.IdTipoCredito == null ? "CON" : info_fam_padre.IdTipoCredito);
            model.Idtipo_cliente_padre = (info_fam_padre.Idtipo_cliente == 0 ? 1 : info_fam_padre.Idtipo_cliente);
            model.IdCiudad_padre_fact = info_fam_padre.IdCiudad_fact;
            model.IdParroquia_padre_fact = info_fam_padre.IdParroquia_fact;
            model.IdPais_padre = info_fam_padre.IdPais;
            model.Cod_Region_padre = info_fam_padre.Cod_Region;
            model.IdProvincia_padre = info_fam_padre.IdProvincia;
            model.IdCiudad_padre = info_fam_padre.IdCiudad;
            model.IdParroquia_padre = info_fam_padre.IdParroquia;
            model.Sector_padre = info_fam_padre.Sector;

            model.IdPersona_madre = info_fam_madre.IdPersona;
            model.SeFactura_madre = info_fam_madre.SeFactura;
            model.IdTipoDocumento_madre = info_fam_madre.IdTipoDocumento;
            model.pe_Naturaleza_madre = info_fam_madre.pe_Naturaleza;
            model.pe_cedulaRuc_madre = info_fam_madre.pe_cedulaRuc;
            model.pe_nombre_madre = info_fam_madre.pe_nombre;
            model.pe_apellido_madre = info_fam_madre.pe_apellido;
            model.pe_fechaNacimiento_madre = info_fam_madre.pe_fechaNacimiento;
            model.pe_sexo_madre = info_fam_madre.pe_sexo;
            model.IdEstadoCivil_madre = info_fam_madre.IdEstadoCivil;
            model.pe_telfono_Contacto_madre = info_fam_madre.Telefono;
            model.Celular_madre = info_fam_madre.Celular;
            model.Correo_madre = info_fam_madre.Correo;
            model.Direccion_madre = info_fam_madre.Direccion;
            model.PorcentajeDiscapacidad_madre = info_fam_madre.PorcentajeDiscapacidad;
            model.NumeroCarnetConadis_madre = info_fam_madre.NumeroCarnetConadis;
            model.CodCatalogoCONADIS_madre = (info_fam_madre.CodCatalogoCONADIS == null ? "" : info_fam_madre.CodCatalogoCONADIS);
            model.IdProfesion_madre = info_fam_madre.IdProfesion;
            model.IdReligion_madre = info_fam_madre.IdReligion;
            model.AsisteCentroCristiano_madre = info_fam_madre.AsisteCentroCristiano;
            model.EstaFallecido_madre = info_fam_madre.EstaFallecido;
            model.IdTipoCredito_madre = (info_fam_madre.IdTipoCredito == null ? "CON" : info_fam_madre.IdTipoCredito);
            model.Idtipo_cliente_madre = (info_fam_madre.Idtipo_cliente == 0 ? 1 : info_fam_madre.Idtipo_cliente);
            model.IdCiudad_madre_fact = info_fam_madre.IdCiudad_fact;
            model.IdParroquia_madre_fact = info_fam_madre.IdParroquia_fact;
            model.IdPais_madre = info_fam_madre.IdPais;
            model.Cod_Region_madre = info_fam_madre.Cod_Region;
            model.IdProvincia_madre = info_fam_madre.IdProvincia;
            model.IdCiudad_madre = info_fam_madre.IdCiudad;
            model.IdParroquia_madre = info_fam_madre.IdParroquia;
            model.Sector_madre = info_fam_madre.Sector;

            if (model == null)
                return RedirectToAction("Index");

            cargar_combos();
            return View(model);
        }
        [HttpPost]
        public ActionResult Anular(aca_Alumno_Info model)
        {
            model.IdUsuarioAnulacion = SessionFixed.IdUsuario;
            model.IdUsuario = SessionFixed.IdUsuario;

            var info_anio = bus_anio.GetInfo_AnioEnCurso(model.IdEmpresa, 0);
            var info_matricula = bus_matricula.GetInfo_ExisteMatricula(model.IdEmpresa, (info_anio==null ?0: info_anio.IdAnio), model.IdAlumno);

            if (model.alu_foto == null)
                model.alu_foto = new byte[0];

            if (info_matricula==null || (info_matricula.IdMatricula != 0 && info_matricula.EsRetirado == true))
            {
                if (!bus_alumno.AnularDB(model))
                {
                    ViewBag.mensaje = "No se ha podido anular el registro";
                    cargar_combos();
                    return View(model);
                }
            }
            else
            {
                if(info_matricula.IdMatricula!=0 && info_matricula.EsRetirado==false)
                {
                    ViewBag.mensaje = "Existe una matricula activa para el año lectivo en curso, realice el retiro del estudiante y luego proceda con la anulación";
                    cargar_combos();
                    return View(model);
                }
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Json
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
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult get_info_x_num_cedula_familia(int IdEmpresa = 0, decimal IdAlumno = 0, string pe_cedulaRuc = "")
        {
            var resultado = bus_familia.get_info_x_num_cedula(IdEmpresa, IdAlumno, pe_cedulaRuc);
            resultado.anio = Convert.ToDateTime(resultado.pe_fechaNacimiento).Year.ToString();
            var mes = Convert.ToDateTime(resultado.pe_fechaNacimiento).Month;
            mes = mes - 1;
            resultado.mes = mes.ToString();
            resultado.dia = Convert.ToDateTime(resultado.pe_fechaNacimiento).Day.ToString();

            resultado.IdPais = ((resultado.IdPais == null || resultado.IdPais=="") ? "1" : resultado.IdPais);
            resultado.Cod_Region = ((resultado.Cod_Region == null || resultado.Cod_Region == "") ? "00001" : resultado.Cod_Region);
            resultado.IdProvincia = ((resultado.IdProvincia == null || resultado.IdProvincia == "") ? "09" : resultado.IdProvincia);
            resultado.IdCiudad = ((resultado.IdCiudad == null || resultado.IdCiudad == "") ? "09" : resultado.IdCiudad);
            resultado.IdParroquia = ((resultado.IdParroquia == null || resultado.IdParroquia == "") ? "09" : resultado.IdParroquia);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public JsonResult VerFichaSocioEconomica(int IdEmpresa = 0, int IdAlumno = 0)
        {
            mensaje = "";
            int IdSocioEconomico = 0;
            var info_socioeconomico = bus_socioeconomico.GetInfo_by_Alumno(IdEmpresa, IdAlumno);
            IdSocioEconomico = (info_socioeconomico == null ? 0 : info_socioeconomico.IdSocioEconomico);

            return Json(new { SocioEconomico = IdSocioEconomico }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SetAlumnoDocumento(decimal IdAlumno = 0)
        {
            SessionFixed.IdAlumno = IdAlumno.ToString();

            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult ActualizarVariablesSession(int IdEmpresa = 0, decimal IdTransaccionSession = 0)
        {
            string retorno = string.Empty;
            SessionFixed.IdEmpresa = IdEmpresa.ToString();
            SessionFixed.IdTransaccionSession = IdTransaccionSession.ToString();
            SessionFixed.IdTransaccionSessionActual = IdTransaccionSession.ToString();
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Funciones imagen alumno
        public JsonResult nombre_imagen(decimal IdAlumno = 0)
        {
            try
            {
                if (IdAlumno == 0)
                    IdAlumno = bus_alumno.GetId(Convert.ToInt32(SessionFixed.IdEmpresa));
                SessionFixed.NombreImagenAlumno = IdAlumno.ToString();
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult get_imagen_general()
        {
            byte[] model = empresa_imagen.pr_imagen;
            if (model == null)
                model = new byte[0];
            return PartialView("_Empresa_imagen", model);
        }
        public class empresa_imagen
        {
            public static byte[] pr_imagen { get; set; }
            public static DevExpress.Web.UploadControlValidationSettings UploadValidationSettings = new DevExpress.Web.UploadControlValidationSettings()
            {
                AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".png" },
                MaxFileSize = 4000000
            };
            public static void FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
            {

                if (e.UploadedFile.IsValid)
                {
                    pr_imagen = e.UploadedFile.FileBytes;
                }
            }
        }
        public JsonResult actualizar_div()
        {
            return Json(SessionFixed.NombreImagenAlumno, JsonRequestBehavior.AllowGet);
        }
        //public string UploadDirectory = "~/Content/imagenes/alumnos/";
        public string UploadDirectory = "";
        public ActionResult DragAndDropImageUpload([ModelBinder(typeof(DragAndDropSupportDemoBinder))]IEnumerable<UploadedFile> ucDragAndDrop)
        {

            try
            {
                //Extract Image File Name.
                string fileName = System.IO.Path.GetFileName(ucDragAndDrop.FirstOrDefault().FileName);
                //Set the Image File Path.
                int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
                var info_parametros = bus_parametro.get_info(IdEmpresa);
                UploadDirectory = (info_parametros==null ? "" : info_parametros.RutaImagen_Alumno);
                UploadDirectory = UploadDirectory + SessionFixed.NombreImagenAlumno + ".jpg";
                imagen = ucDragAndDrop.FirstOrDefault().FileBytes;
                //Save the Image File in Folder.
                ucDragAndDrop.FirstOrDefault().SaveAs(Server.MapPath(UploadDirectory));
                SessionFixed.NombreImagenAlumno = UploadDirectory;

                file = ucDragAndDrop.FirstOrDefault();
                return Json(ucDragAndDrop.FirstOrDefault().FileBytes, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return View();
            }

        }

        #endregion

        #region Importacion
        public ActionResult UploadControlUpload()
        {
            UploadControlExtension.GetUploadedFiles("UploadControlFile", UploadControlSettings.UploadValidationSettings, UploadControlSettings.FileUploadComplete);
            return null;
        }
        public ActionResult Importar(int IdEmpresa = 0)
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            aca_Alumno_Info model = new aca_Alumno_Info
            {
                IdEmpresa = IdEmpresa,
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual)
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult Importar(aca_Alumno_Info model)
        {
            try
            {
                var IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
                var Lista_Estudiantes = Lista_Alumno.get_list(model.IdTransaccionSession);
                var Lista_Familia_Estudiantes= Lista_Familia.get_list(model.IdTransaccionSession);
                foreach (var item in Lista_Estudiantes)
                {
                    if (!bus_alumno.GuardarDB(item))
                    {
                        ViewBag.mensaje = "Error al importar el archivo";
                        return View(model);
                    }
                }

                foreach (var item in Lista_Familia_Estudiantes)
                {
                    var info_alumno = bus_alumno.get_info_x_num_cedula(IdEmpresa, item.pe_cedulaRuc_Alumno);
                    if (info_alumno != null)
                    {
                        item.IdAlumno = info_alumno.IdAlumno;
                        if (!bus_familia.guardarDB(item))
                        {
                            ViewBag.mensaje = "Error al importar el archivo";
                            return View(model);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                //SisLogError.set_list((ex.InnerException) == null ? ex.Message.ToString() : ex.InnerException.ToString());

                ViewBag.error = ex.Message.ToString();
                return View(model);
            }

            return RedirectToAction("Index");
        }

        public ActionResult GridViewPartial_AlumnoImportacion()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_Alumno.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_AlumnoImportacion", model);
        }
        public ActionResult GridViewPartial_AlumnoFamiliaImportacion()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_Familia.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_AlumnoFamiliaImportacion", model);
        }
        #endregion
    }

    #region Clases para imagen alumno
    public class DragAndDropSupportDemoBinder : DevExpressEditorsBinder
    {
        public DragAndDropSupportDemoBinder()
        {
            UploadControlBinderSettings.ValidationSettings.Assign(UploadControlDemosHelper.UploadValidationSettings);
            UploadControlBinderSettings.FileUploadCompleteHandler = UploadControlDemosHelper.FileUploadComplete;
        }
    }
    public class UploadControlDemosHelper
    {
        public static byte[] em_foto { get; set; }
        public static DevExpress.Web.UploadControlValidationSettings UploadValidationSettings = new DevExpress.Web.UploadControlValidationSettings()
        {
            AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".png" },
            MaxFileSize = 4000000
        };
        public static void FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
        {

            if (e.UploadedFile.IsValid)
            {
                em_foto = e.UploadedFile.FileBytes;
                //var filename = Path.GetFileName(e.UploadedFile.FileName);
                //e.UploadedFile.SaveAs("~/Content/imagenes/"+e.UploadedFile.FileName, true);
            }
        }
    }
    #endregion

    public class aca_alumno_List
    {
        string Variable = "aca_Alumno_Info";
        public List<aca_Alumno_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<aca_Alumno_Info> list = new List<aca_Alumno_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<aca_Alumno_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<aca_Alumno_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }

    public class aca_AlumnoDocumento_List
    {
        aca_AlumnoDocumento_Bus bus_alumno_doc = new aca_AlumnoDocumento_Bus();

        string Variable = "aca_AlumnoDocumento_Info";
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

        public void AddRow(aca_AlumnoDocumento_Info info_det, decimal IdTransaccionSession)
        {
            int IdEmpresa = string.IsNullOrEmpty(SessionFixed.IdEmpresa) ? 0 : Convert.ToInt32(SessionFixed.IdEmpresa);
            List<aca_AlumnoDocumento_Info> list = get_list(IdTransaccionSession);
            info_det.Secuencia = list.Count == 0 ? 1 : list.Max(q => q.Secuencia) + 1;
            info_det.IdAlumno = Convert.ToInt32(SessionFixed.IdAlumno);
            info_det.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);

            bus_alumno_doc.GuardarDB(info_det);
            list.Add(info_det);
        }

        public void UpdateRow(aca_AlumnoDocumento_Info info_det, decimal IdTransaccionSession)
        {
            int IdEmpresa = string.IsNullOrEmpty(SessionFixed.IdEmpresa) ? 0 : Convert.ToInt32(SessionFixed.IdEmpresa);

            aca_AlumnoDocumento_Info edited_info = get_list(IdTransaccionSession).Where(m => m.Secuencia == info_det.Secuencia).FirstOrDefault();
            //edited_info.IdDocumento = info_det.IdDocumento;
            //edited_info.NomDocumento = info_det.NomDocumento;
            edited_info.EnArchivo = info_det.EnArchivo;
            edited_info.IdAlumno = Convert.ToInt32(SessionFixed.IdAlumno);
            edited_info.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            bus_alumno_doc.ModificarDB(edited_info);
        }

        public void DeleteRow(int Secuencia, decimal IdTransaccionSession)
        {
            List<aca_AlumnoDocumento_Info> list = get_list(IdTransaccionSession);
            var info_det = list.Where(q=>q.Secuencia == Secuencia).FirstOrDefault();
            info_det.IdAlumno = Convert.ToInt32(SessionFixed.IdAlumno);
            info_det.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);

            list.Remove(list.Where(q => q.Secuencia == Secuencia).FirstOrDefault());
            bus_alumno_doc.EliminarDB(info_det);
        }
    }

    public class UploadControlSettings
    {
        public static DevExpress.Web.UploadControlValidationSettings UploadValidationSettings = new DevExpress.Web.UploadControlValidationSettings()
        {
            AllowedFileExtensions = new string[] { ".xlsx" },
            MaxFileSize = 40000000
        };
        public static void FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
        {
            #region Variables
            aca_alumno_List ListaEstudiantes = new aca_alumno_List();
            aca_Familia_List ListaFamilia = new aca_Familia_List();
            List<aca_Alumno_Info> Lista_Estudiantes = new List<aca_Alumno_Info>();
            List<aca_Familia_Info> Lista_FamiliaEstudiantes = new List<aca_Familia_Info>();
            tb_persona_Bus bus_persona = new tb_persona_Bus();
            aca_Catalogo_Bus bus_aca_catalogo = new aca_Catalogo_Bus();
            
            int cont = 0;
            decimal IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            #endregion

            Stream stream = new MemoryStream(e.UploadedFile.FileBytes);
            if (stream.Length > 0)
            {
                IExcelDataReader reader = null;
                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);

                #region Alumno   
                var lst_persona = bus_persona.get_list(false);
                var IdAlumno = 1;
                var no_validas = "";
                var repetidos = "";

                while (reader.Read())
                {
                    if (!reader.IsDBNull(0) && cont > 0)
                    {
                        var return_naturaleza = "";
                        var cedula_ruc_alumno = (Convert.ToString(reader.GetValue(3))).Trim();

                        tb_persona_Info info_persona_alumno = new tb_persona_Info();
                        tb_persona_Info info_persona_alu = new tb_persona_Info();

                        if (cedula_ruc_alumno== "0943334409" || cedula_ruc_alumno == "0929694446" || cedula_ruc_alumno == "0911583078" || cedula_ruc_alumno == "0931748693" || cedula_ruc_alumno == "0923513766")
                        {
                            var a = bus_persona.get_info_x_num_cedula(cedula_ruc_alumno);
                        }

                        info_persona_alu = lst_persona.Where(q=>q.pe_cedulaRuc == cedula_ruc_alumno).FirstOrDefault();
                        info_persona_alumno = info_persona_alu;

                        if (cl_funciones.ValidaIdentificacion(Convert.ToString(reader.GetValue(2)), Convert.ToString(reader.GetValue(1)), cedula_ruc_alumno, ref return_naturaleza))
                        {
                            if (info_persona_alumno == null || info_persona_alu.IdPersona==0)
                            {
                                tb_persona_Info info_alumno = new tb_persona_Info
                                {
                                    pe_Naturaleza = Convert.ToString(reader.GetValue(1)),
                                    pe_nombreCompleto = Convert.ToString(reader.GetValue(4)).Trim() + ' ' + Convert.ToString(reader.GetValue(5)).Trim(),
                                    pe_razonSocial = (Convert.ToString(reader.GetValue(1)) == "NATU" ? "" : Convert.ToString(reader.GetValue(4)).Trim() + ' ' + Convert.ToString(reader.GetValue(5)).Trim()),
                                    pe_apellido = Convert.ToString(reader.GetValue(4)).Trim(),
                                    pe_nombre = Convert.ToString(reader.GetValue(5)).Trim(),
                                    pe_fechaNacimiento = Convert.ToDateTime(reader.GetValue(6)),
                                    pe_sexo = Convert.ToString(reader.GetValue(7)),
                                    IdTipoDocumento = Convert.ToString(reader.GetValue(2)),
                                    pe_cedulaRuc = cedula_ruc_alumno,
                                    pe_direccion = Convert.ToString(reader.GetValue(8)).Trim(),
                                    pe_telfono_Contacto = Convert.ToString(reader.GetValue(10)).Trim(),
                                    pe_correo = Convert.ToString(reader.GetValue(9)).Trim(),
                                };
                                info_persona_alumno = info_alumno;
                            }
                            else
                            {
                                info_persona_alumno = bus_persona.get_info(info_persona_alu.IdPersona);
                                var Naturaleza = Convert.ToString(reader.GetValue(1));
                                info_persona_alumno.pe_Naturaleza = Naturaleza;
                                info_persona_alumno.pe_nombreCompleto = Convert.ToString(reader.GetValue(4)).Trim() + ' ' + Convert.ToString(reader.GetValue(5)).Trim();
                                info_persona_alumno.pe_razonSocial = (Convert.ToString(reader.GetValue(1)) == "NATU" ? "" : Convert.ToString(reader.GetValue(4)) + ' ' + Convert.ToString(reader.GetValue(5)));
                                info_persona_alumno.pe_apellido = Convert.ToString(reader.GetValue(4)).Trim();
                                info_persona_alumno.pe_nombre = Convert.ToString(reader.GetValue(5)).Trim();
                                info_persona_alumno.IdTipoDocumento = Convert.ToString(reader.GetValue(2)).Trim();
                                info_persona_alumno.pe_cedulaRuc = cedula_ruc_alumno;
                                info_persona_alumno.pe_direccion = Convert.ToString(reader.GetValue(8)).Trim();
                                info_persona_alumno.pe_telfono_Contacto = Convert.ToString(reader.GetValue(10)).Trim();
                                info_persona_alumno.pe_correo = Convert.ToString(reader.GetValue(9)).Trim();
                                info_persona_alumno.pe_sexo = Convert.ToString(reader.GetValue(7)).Trim();
                            }

                            info_persona_alumno.pe_Naturaleza = return_naturaleza;
                            info_persona_alumno.pe_nombreCompleto = (info_persona_alumno.pe_razonSocial != "" ? info_persona_alumno.pe_razonSocial : (info_persona_alumno.pe_apellido + ' ' + info_persona_alumno.pe_nombre));
                            
                            aca_Alumno_Info info = new aca_Alumno_Info
                            {
                                IdEmpresa = IdEmpresa,
                                IdAlumno = IdAlumno,
                                Codigo = Convert.ToString(reader.GetValue(0)).Trim(),
                                IdPersona = info_persona_alumno.IdPersona,
                                Direccion = info_persona_alumno.pe_direccion,
                                Celular = info_persona_alumno.pe_telfono_Contacto,
                                Correo = info_persona_alumno.pe_correo,
                                Estado = true,
                                IdCatalogoESTMAT = 1,
                                IdCurso = null,
                                IdCatalogoESTALU = 8,
                                FechaIngreso = Convert.ToDateTime(reader.GetValue(11)),
                                LugarNacimiento = "",
                                IdPais = null,
                                Cod_Region = null,
                                IdProvincia = null,
                                IdCiudad = null,
                                IdParroquia = null,
                                Sector = "",

                                IdUsuarioCreacion = SessionFixed.IdUsuario,
                                FechaCreacion = DateTime.Now
                            };

                            info.info_persona_alumno = info_persona_alumno;
                            IdAlumno++;

                            if (Lista_Estudiantes.Where(q => q.info_persona_alumno.pe_cedulaRuc == info_persona_alumno.pe_cedulaRuc).Count() == 0)
                            {
                                Lista_Estudiantes.Add(info);
                            }
                            else
                            {
                                repetidos = repetidos + cedula_ruc_alumno + " ";
                            }
                            
                        }
                        else
                        {
                            no_validas = no_validas + cedula_ruc_alumno + " ";
                        }
                    }
                    else
                        cont++;
                }
                no_validas = " " + no_validas;
                repetidos = " " + repetidos;
                ListaEstudiantes.set_list(Lista_Estudiantes, IdTransaccionSession);
                #endregion

                //Para avanzar a la siguiente hoja de excel
                cont = 0;
                reader.NextResult();
                var x = reader.ResultsCount;
                while (reader.Read())
                {
                    var Secuencia = 1;
                    no_validas = "";
                    repetidos = "";
                    if (!reader.IsDBNull(0) && cont > 0)
                    {
                        var return_naturaleza_familia = "";
                        var cedula_ruc_familia = Convert.ToString(reader.GetValue(2)).Trim();
                        var cedula_alumno = Convert.ToString(reader.GetValue(0)).Trim();
                        tb_persona_Info info_persona_familia = new tb_persona_Info();
                        tb_persona_Info info_persona_fam = new tb_persona_Info();
                        info_persona_fam = lst_persona.Where(q => q.pe_cedulaRuc == cedula_ruc_familia).FirstOrDefault();
                        info_persona_familia = info_persona_fam;
                        aca_Alumno_Info InfoAlumno = Lista_Estudiantes.Where(q => q.info_persona_alumno.pe_cedulaRuc == cedula_alumno).FirstOrDefault();

                        if (cl_funciones.ValidaIdentificacion(Convert.ToString(reader.GetValue(4)), Convert.ToString(reader.GetValue(3)), cedula_ruc_familia, ref return_naturaleza_familia))
                        {
                            if (info_persona_familia == null)
                            {
                                tb_persona_Info persona_fam = new tb_persona_Info
                                {
                                    pe_Naturaleza = Convert.ToString(reader.GetValue(3)),
                                    pe_nombreCompleto = Convert.ToString(reader.GetValue(5)).Trim() + ' ' + Convert.ToString(reader.GetValue(6)).Trim(),
                                    pe_razonSocial = (Convert.ToString(reader.GetValue(3)) == "NATU" ? "" : Convert.ToString(reader.GetValue(5)) + ' ' + Convert.ToString(reader.GetValue(6))),
                                    pe_apellido = Convert.ToString(reader.GetValue(5)).Trim(),
                                    pe_nombre = Convert.ToString(reader.GetValue(6)).Trim(),
                                    pe_sexo = Convert.ToString(reader.GetValue(7)).Trim(),
                                    IdTipoDocumento = Convert.ToString(reader.GetValue(4)),
                                    pe_cedulaRuc = cedula_ruc_familia,
                                    pe_direccion = Convert.ToString(reader.GetValue(8)).Trim(),
                                    pe_telfono_Contacto = Convert.ToString(reader.GetValue(10)).Trim(),
                                    pe_celular = Convert.ToString(reader.GetValue(11)).Trim(),
                                    pe_correo = Convert.ToString(reader.GetValue(9)).Trim(),
                                    IdReligion = Convert.ToInt32(reader.GetValue(26)),
                                    IdProfesion = Convert.ToInt32(reader.GetValue(18))
                                };
                                info_persona_familia = persona_fam;
                            }
                            else
                            {
                                info_persona_familia = bus_persona.get_info(info_persona_fam.IdPersona);
                                //var Naturaleza = Convert.ToString(reader.GetValue(3));
                                //info_persona_familia.pe_Naturaleza = Naturaleza;
                                //info_persona_familia.pe_nombreCompleto = Convert.ToString(reader.GetValue(5)).Trim() + ' ' + Convert.ToString(reader.GetValue(6)).Trim();
                                //info_persona_familia.pe_razonSocial = (Convert.ToString(reader.GetValue(3)) == "NATU" ? "" : Convert.ToString(reader.GetValue(5)).Trim() + ' ' + Convert.ToString(reader.GetValue(6)).Trim());
                                //info_persona_familia.pe_apellido = Convert.ToString(reader.GetValue(5)).Trim();
                                //info_persona_familia.pe_nombre = Convert.ToString(reader.GetValue(6)).Trim();
                                //info_persona_familia.IdTipoDocumento = Convert.ToString(reader.GetValue(4));
                                //info_persona_familia.pe_cedulaRuc = cedula_ruc_familia;
                                //info_persona_familia.pe_direccion = Convert.ToString(reader.GetValue(8)).Trim();
                                //info_persona_familia.pe_telfono_Contacto = Convert.ToString(reader.GetValue(10)).Trim();
                                //info_persona_familia.pe_correo = Convert.ToString(reader.GetValue(9)).Trim();
                                //info_persona_familia.pe_celular = Convert.ToString(reader.GetValue(11)).Trim();
                                //info_persona_familia.IdReligion = Convert.ToInt32(reader.GetValue(26));
                                //info_persona_familia.IdProfesion = Convert.ToInt32(reader.GetValue(18));
                            }

                            //info_persona_familia.pe_Naturaleza = return_naturaleza_familia;
                            //info_persona_familia.pe_nombreCompleto = (info_persona_familia.pe_razonSocial != "" ? info_persona_familia.pe_razonSocial : (info_persona_familia.pe_apellido + ' ' + info_persona_familia.pe_nombre));

                            if (InfoAlumno != null && InfoAlumno.IdAlumno > 0)
                            {

                                aca_Familia_Info info_fam = new aca_Familia_Info
                                {
                                    IdEmpresa = IdEmpresa,
                                    IdAlumno = InfoAlumno.IdAlumno,
                                    pe_cedulaRuc_Alumno = cedula_alumno,
                                    Secuencia = Secuencia,
                                    IdCatalogoPAREN = Convert.ToInt32(reader.GetValue(1)),
                                    IdPersona = info_persona_familia.IdPersona,
                                    Direccion = Convert.ToString(reader.GetValue(8)).Trim(),
                                    Celular = Convert.ToString(reader.GetValue(11)),
                                    Correo = Convert.ToString(reader.GetValue(9)).Trim(),
                                    SeFactura = (Convert.ToString(reader.GetValue(13)) == "SI" ? true : false),
                                    EsRepresentante = (Convert.ToString(reader.GetValue(12)) == "SI" ? true : false),
                                    EmpresaTrabajo = Convert.ToString(reader.GetValue(14)).Trim(),
                                    DireccionTrabajo = Convert.ToString(reader.GetValue(15)).Trim(),
                                    TelefonoTrabajo = Convert.ToString(reader.GetValue(16)).Trim(),
                                    CargoTrabajo = Convert.ToString(reader.GetValue(17)).Trim(),
                                    AniosServicio = Convert.ToInt32(reader.GetValue(19)),
                                    IngresoMensual = Convert.ToDouble(reader.GetValue(20)),
                                    VehiculoPropio = (Convert.ToString(reader.GetValue(21)) == "SI" ? true : false),
                                    AnioVehiculo = Convert.ToInt32(reader.GetValue(24)),
                                    Marca = Convert.ToString(reader.GetValue(22)).Trim(),
                                    Modelo = Convert.ToString(reader.GetValue(23)).Trim(),
                                    CasaPropia = (Convert.ToString(reader.GetValue(25)) == "SI" ? true : false),
                                    Estado = true,
                                    AsisteCentroCristiano = false,
                                    IdUsuarioCreacion = SessionFixed.IdUsuario,
                                    FechaCreacion = DateTime.Now
                                };

                                info_fam.info_persona = info_persona_familia;
                                Lista_FamiliaEstudiantes.Add(info_fam);
                                Secuencia++;
                            }

                        }
                        else
                        {
                            no_validas = no_validas + cedula_ruc_familia + " ";
                        }
                    }
                    cont++;
                }
                no_validas = " " + no_validas;
                ListaFamilia.set_list(Lista_FamiliaEstudiantes, IdTransaccionSession);
            }
        }
    }
}