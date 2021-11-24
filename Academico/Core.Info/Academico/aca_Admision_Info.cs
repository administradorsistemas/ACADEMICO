using Core.Info.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Core.Info.Academico
{
    public class aca_Admision_Info
    {
        public decimal IdTransaccionSession { get; set; }
        public int IdEmpresa { get; set; }
        public decimal IdAdmision { get; set; }
        [Required(ErrorMessage = "El campo sede es obligatorio")]
        public int IdSede { get; set; }
        [Required(ErrorMessage = "El campo año lectivo es obligatorio")]
        public int IdAnio { get; set; }
        [Required(ErrorMessage = "El campo jornada es obligatorio")]
        public int IdJornada { get; set; }
        [Required(ErrorMessage = "El campo nivel es obligatorio")]
        public int IdNivel { get; set; }
        [Required(ErrorMessage = "El campo curso es obligatorio")]
        public int IdCurso { get; set; }
        [Required(ErrorMessage = "El campo naturaleza es obligatorio")]
        public string Naturaleza_Aspirante { get; set; }
        [Required(ErrorMessage = "El campo tipo de documento es obligatorio")]
        public string IdTipoDocumento_Aspirante { get; set; }
        [Required(ErrorMessage = "El campo cédula es obligatorio")]
        public string CedulaRuc_Aspirante { get; set; }
        [Required(ErrorMessage = "El campo nombres es obligatorio")]
        //[StringLength(0, MinimumLength = 0, ErrorMessage = "el campo nombres debe tener máximo 100 caracteres")]
        public string Nombres_Aspirante { get; set; }
        [Required(ErrorMessage = "El campo apellidos es obligatorio")]
        //[StringLength(0, MinimumLength = 0, ErrorMessage = "el campo apellidos debe tener máximo 100 caracteres")]
        public string Apellidos_Aspirante { get; set; }
        //[Required(ErrorMessage = "El campo nombre completo del aspirante es obligatorio")]
        //[StringLength(1, MinimumLength = 0, ErrorMessage = "el campo nombre completo debe tener máximo 200 caracteres")]
        public string NombreCompleto_Aspirante { get; set; }
        [Required(ErrorMessage = "El campo direccion es obligatorio")]
        public string Direccion_Aspirante { get; set; }
        [Required(ErrorMessage = "El campo teléfono es obligatorio")]
        public string Telefono_Aspirante { get; set; }
        [Required(ErrorMessage = "El campo celular es obligatorio")]
        public string Celular_Aspirante { get; set; }
        [Required(ErrorMessage = "El campo correo es obligatorio")]
        public string Correo_Aspirante { get; set; }
        [Required(ErrorMessage = "El campo sexo es obligatorio")]
        public string Sexo_Aspirante { get; set; }
        [Required(ErrorMessage = "El campo fecha de nacimiento es obligatorio")]
        public Nullable<System.DateTime> FechaNacimiento_Aspirante { get; set; }
        [Required(ErrorMessage = "El campo tipo de sangre es obligatorio")]
        public string CodCatalogoSangre_Aspirante { get; set; }
        public string CodCatalogoCONADIS_Aspirante { get; set; }
        public Nullable<double> PorcentajeDiscapacidad_Aspirante { get; set; }
        public string NumeroCarnetConadis_Aspirante { get; set; }
        [Required(ErrorMessage = "El campo grupo étnico es obligatorio")]
        public Nullable<int> IdGrupoEtnico_Aspirante { get; set; }
        [Required(ErrorMessage = "El campo religión es obligatorio")]
        public Nullable<int> IdReligion_Aspirante { get; set; }
        [Required(ErrorMessage = "El campo asiste a Centro Cristiano es obligatorio")]
        public bool AsisteCentroCristiano_Aspirante { get; set; }
        [Required(ErrorMessage = "El campo lugar de nacimiento es obligatorio")]
        public string LugarNacimiento_Aspirante { get; set; }
        [Required(ErrorMessage = "El campo país es obligatorio")]
        public string IdPais_Aspirante { get; set; }
        [Required(ErrorMessage = "El campo región es obligatorio")]
        public string Cod_Region_Aspirante { get; set; }
        [Required(ErrorMessage = "El campo provincia es obligatorio")]
        public string IdProvincia_Aspirante { get; set; }
        [Required(ErrorMessage = "El campo ciudad es obligatorio")]
        public string IdCiudad_Aspirante { get; set; }
        [Required(ErrorMessage = "El campo parroquia es obligatorio")]
        public string IdParroquia_Aspirante { get; set; }
        [Required(ErrorMessage = "El campo sector es obligatorio")]
        public string Sector_Aspirante { get; set; }
        public System.DateTime FechaIngreso_Aspirante { get; set; }
        [Required(ErrorMessage = "El campo dificultad para leer es obligatorio")]
        public bool Dificultad_Lectura { get; set; }
        [Required(ErrorMessage = "El campo dificultad para escribir es obligatorio")]
        public bool Dificultad_Escritura { get; set; }
        [Required(ErrorMessage = "El campo dificultad para matematicas es obligatorio")]
        public bool Dificultad_Matematicas { get; set; }
        [Required(ErrorMessage = "El campo acepta terminos y condiciones es obligatorio")]
        public bool AceptaTerminos { get; set; }
        [Required(ErrorMessage = "El campo tipo de vivienda es obligatorio")]
        public int IdCatalogoFichaTipoViv_Aspirante { get; set; }
        [Required(ErrorMessage = "El campo vivienda es obligatorio")]
        public int IdCatalogoFichaViv_Aspirante { get; set; }
        [Required(ErrorMessage = "El campo agua es obligatorio")]
        public int IdCatalogoFichaAgua_Aspirante { get; set; }
        [Required(ErrorMessage = "El campo electricidad es obligatorio")]
        public bool TieneElectricidad_Aspirante { get; set; }
        [Required(ErrorMessage = "El campo tiene hermanos es obligatorio")]
        public bool TieneHermanos_Aspirante { get; set; }
        [Required(ErrorMessage = "El campo cantidad de hermanos es obligatorio")]
        public Nullable<int> CantidadHermanos { get; set; }
        [Required(ErrorMessage = "El campo motivo de ingreso es obligatorio")]
        public int IdCatalogoFichaMotivo_Aspirante { get; set; }
        [Required(ErrorMessage = "El campo como se informó es obligatorio")]
        public int IdCatalogoFichaInst_Aspirante { get; set; }
        [Required(ErrorMessage = "El campo finaciamiento de estudios es obligatorio")]
        public int IdCatalogoFichaFinanc_Aspirante { get; set; }
        [Required(ErrorMessage = "El campo vive con es obligatorio")]
        public int IdCatalogoFichaVive_Aspirante { get; set; }
        [Required(ErrorMessage = "El campo otro motivo es obligatorio")]
        public string OtroMotivoIngreso_Aspirante { get; set; }
        [Required(ErrorMessage = "El campo otra información es obligatorio")]
        public string OtroInformacionInst_Aspirante { get; set; }
        [Required(ErrorMessage = "El campo otro financiamiento es obligatorio")]
        public string OtroFinanciamiento_Aspirante { get; set; }
        [Required(ErrorMessage = "El campo naturaleza es obligatorio")]
        public string Naturaleza_Padre { get; set; }
        [Required(ErrorMessage = "El campo tipo de documento es obligatorio")]
        public string IdTipoDocumento_Padre { get; set; }
        //[Required(ErrorMessage = "El campo cédula o Ruc es obligatorio")]
        public string CedulaRuc_Padre { get; set; }
        //[Required(ErrorMessage = "El campo nombres es obligatorio")]
        //[StringLength(1, MinimumLength = 0, ErrorMessage = "el campo nombres debe tener máximo 100 caracteres")]
        public string Nombres_Padre { get; set; }
        //[Required(ErrorMessage = "El campo apellidos es obligatorio")]
        //[StringLength(1, MinimumLength = 0, ErrorMessage = "el campo apellidos debe tener máximo 100 caracteres")]
        public string Apellidos_Padre { get; set; }
        public string NombreCompleto_Padre { get; set; }
        public string RazonSocial_Padre { get; set; }
        //[Required(ErrorMessage = "El campo direccion es obligatorio")]
        public string Direccion_Padre { get; set; }
        //[Required(ErrorMessage = "El campo teléfono es obligatorio")]
        public string Telefono_Padre { get; set; }
        //[Required(ErrorMessage = "El campo celular es obligatorio")]
        public string Celular_Padre { get; set; }
        //[Required(ErrorMessage = "El campo correo es obligatorio")]
        public string Correo_Padre { get; set; }
        //[Required(ErrorMessage = "El campo sexo es obligatorio")]
        public string Sexo_Padre { get; set; }
        //[Required(ErrorMessage = "El campo fecha de nacimiento es obligatorio")]
        public Nullable<System.DateTime> FechaNacimiento_Padre { get; set; }
        public string CodCatalogoCONADIS_Padre { get; set; }
        public Nullable<double> PorcentajeDiscapacidad_Padre { get; set; }
        public string NumeroCarnetConadis_Padre { get; set; }
        //[Required(ErrorMessage = "El campo grupo étnico es obligatorio")]
        public Nullable<int> IdGrupoEtnico_Padre { get; set; }
        //[Required(ErrorMessage = "El campo religión es obligatorio")]
        public Nullable<int> IdReligion_Padre { get; set; }
        //[Required(ErrorMessage = "El campo estado civil es obligatorio")]
        public string IdEstadoCivil_Padre { get; set; }
        //[Required(ErrorMessage = "El campo asiste a Centro Cristiano es obligatorio")]
        public bool AsisteCentroCristiano_Padre { get; set; }
        //[Required(ErrorMessage = "El campo país es obligatorio")]
        public string IdPais_Padre { get; set; }
        //[Required(ErrorMessage = "El campo región es obligatorio")]
        public string Cod_Region_Padre { get; set; }
        //[Required(ErrorMessage = "El campo provincia es obligatorio")]
        public string IdProvincia_Padre { get; set; }
        //[Required(ErrorMessage = "El campo ciudad es obligatorio")]
        public string IdCiudad_Padre { get; set; }
        //[Required(ErrorMessage = "El campo parroquia es obligatorio")]
        public string IdParroquia_Padre { get; set; }
        //[Required(ErrorMessage = "El campo sector es obligatorio")]
        public string Sector_Padre { get; set; }
        //[Required(ErrorMessage = "El campo parentezco es obligatorio")]
        public int IdCatalogoPAREN_Padre { get; set; }
        //[Required(ErrorMessage = "El campo instrucción es obligatorio")]
        public Nullable<int> IdCatalogoFichaInst_Padre { get; set; }
        //[Required(ErrorMessage = "El campo empresa donde trabaja es obligatorio")]
        public string EmpresaTrabajo_Padre { get; set; }
        //[Required(ErrorMessage = "El campo dirección de trabajo es obligatorio")]
        public string DireccionTrabajo_Padre { get; set; }
        //[Required(ErrorMessage = "El campo teléfono de trabajo es obligatorio")]
        public string TelefonoTrabajo_Padre { get; set; }
        //[Required(ErrorMessage = "El campo cargo es obligatorio")]
        public string CargoTrabajo_Padre { get; set; }
        //[Required(ErrorMessage = "El campo años de servicio es obligatorio")]
        public Nullable<int> AniosServicio_Padre { get; set; }
        //[Required(ErrorMessage = "El campo ingreso mensual es obligatorio")]
        public Nullable<double> IngresoMensual_Padre { get; set; }
        //[Required(ErrorMessage = "El campo vehículo propio es obligatorio")]
        public bool VehiculoPropio_Padre { get; set; }
        //[Required(ErrorMessage = "El campo marca de vehículo es obligatorio")]
        public string Marca_Padre { get; set; }
        //[Required(ErrorMessage = "El campo modelo de vehículo es obligatorio")]
        public string Modelo_Padre { get; set; }
        //[Required(ErrorMessage = "El campo año de vehículo es obligatorio")]
        public Nullable<int> AnioVehiculo_Padre { get; set; }
        //[Required(ErrorMessage = "El campo casa propia es obligatorio")]
        public bool CasaPropia_Padre { get; set; }
        public bool EstaFallecido_Padre { get; set; }
        //[Required(ErrorMessage = "El campo profesión es obligatorio")]
        public Nullable<int> IdProfesion_Padre { get; set; }
        //[Required(ErrorMessage = "El campo se factura es obligatorio")]
        public bool SeFactura_Padre { get; set; }
        //[Required(ErrorMessage = "El campo tipo de cliente es obligatorio")]
        public Nullable<int> Idtipo_cliente_Padre { get; set; }
        public string IdTipoCredito_Padre { get; set; }
        //[Required(ErrorMessage = "El campo ciudad es obligatorio")]
        public string IdCiudad_Padre_Fact { get; set; }
        //[Required(ErrorMessage = "El campo parroquia es obligatorio")]
        public string IdParroquia_Padre_Fact { get; set; }
        [Required(ErrorMessage = "El campo naturaleza es obligatorio")]
        public string Naturaleza_Madre { get; set; }
        [Required(ErrorMessage = "El campo tipo documento es obligatorio")]
        public string IdTipoDocumento_Madre { get; set; }
        //[Required(ErrorMessage = "El campo cédula o Ruc es obligatorio")]
        public string CedulaRuc_Madre { get; set; }
        //[Required(ErrorMessage = "El campo nombres es obligatorio")]
        //[StringLength(1, MinimumLength = 0, ErrorMessage = "el campo nombres debe tener máximo 100 caracteres")]
        public string Nombres_Madre { get; set; }
        //[Required(ErrorMessage = "El campo apellidos es obligatorio")]
        //[StringLength(1, MinimumLength = 0, ErrorMessage = "el campo apellidos debe tener máximo 100 caracteres")]
        public string Apellidos_Madre { get; set; }
        public string NombreCompleto_Madre { get; set; }
        public string RazonSocial_Madre { get; set; }
        //[Required(ErrorMessage = "El campo direccion es obligatorio")]
        public string Direccion_Madre { get; set; }
        //[Required(ErrorMessage = "El campo teléfono es obligatorio")]
        public string Telefono_Madre { get; set; }
        //[Required(ErrorMessage = "El campo celular es obligatorio")]
        public string Celular_Madre { get; set; }
        //[Required(ErrorMessage = "El campo correo es obligatorio")]
        public string Correo_Madre { get; set; }
        //[Required(ErrorMessage = "El campo sexo es obligatorio")]
        public string Sexo_Madre { get; set; }
        //[Required(ErrorMessage = "El campo fecha de nacimiento es obligatorio")]
        public Nullable<System.DateTime> FechaNacimiento_Madre { get; set; }
        public string CodCatalogoCONADIS_Madre { get; set; }
        public Nullable<double> PorcentajeDiscapacidad_Madre { get; set; }
        public string NumeroCarnetConadis_Madre { get; set; }
        //[Required(ErrorMessage = "El campo grupo étnico es obligatorio")]
        public Nullable<int> IdGrupoEtnico_Madre { get; set; }
        //[Required(ErrorMessage = "El campo religión es obligatorio")]
        public Nullable<int> IdReligion_Madre { get; set; }
        //[Required(ErrorMessage = "El campo estado civil es obligatorio")]
        public string IdEstadoCivil_Madre { get; set; }
        public bool AsisteCentroCristiano_Madre { get; set; }
        //[Required(ErrorMessage = "El campo país es obligatorio")]
        public string IdPais_Madre { get; set; }
        //[Required(ErrorMessage = "El campo región es obligatorio")]
        public string Cod_Region_Madre { get; set; }
        //[Required(ErrorMessage = "El campo provincia es obligatorio")]
        public string IdProvincia_Madre { get; set; }
        //[Required(ErrorMessage = "El campo ciudad es obligatorio")]
        public string IdCiudad_Madre { get; set; }
        //[Required(ErrorMessage = "El campo parroquia es obligatorio")]
        public string IdParroquia_Madre { get; set; }
        //[Required(ErrorMessage = "El campo sector es obligatorio")]
        public string Sector_Madre { get; set; }
        //[Required(ErrorMessage = "El campo parentezco es obligatorio")]
        public int IdCatalogoPAREN_Madre { get; set; }
        //[Required(ErrorMessage = "El campo instrucción es obligatorio")]
        public Nullable<int> IdCatalogoFichaInst_Madre { get; set; }
        //[Required(ErrorMessage = "El campo empresa donde trabaja es obligatorio")]
        public string EmpresaTrabajo_Madre { get; set; }
        //[Required(ErrorMessage = "El campo dirección del trabajo es obligatorio")]
        public string DireccionTrabajo_Madre { get; set; }
        //[Required(ErrorMessage = "El campo teléfono del trabajo es obligatorio")]
        public string TelefonoTrabajo_Madre { get; set; }
        //[Required(ErrorMessage = "El campo cargo es obligatorio")]
        public string CargoTrabajo_Madre { get; set; }
        //[Required(ErrorMessage = "El campo años de servicio es obligatorio")]
        public Nullable<int> AniosServicio_Madre { get; set; }
        public Nullable<double> IngresoMensual_Madre { get; set; }
        public bool VehiculoPropio_Madre { get; set; }
        public string Marca_Madre { get; set; }
        public string Modelo_Madre { get; set; }
        public Nullable<int> AnioVehiculo_Madre { get; set; }
        public bool CasaPropia_Madre { get; set; }
        public bool EstaFallecido_Madre { get; set; }
        public Nullable<int> IdProfesion_Madre { get; set; }
        //[Required(ErrorMessage = "El campo se factura es obligatorio")]
        public bool SeFactura_Madre { get; set; }
        //[Required(ErrorMessage = "El campo tipo de cliente es obligatorio")]
        public Nullable<int> Idtipo_cliente_Madre { get; set; }
        public string IdTipoCredito_Madre { get; set; }
        //[Required(ErrorMessage = "El campo ciudad es obligatorio")]
        public string IdCiudad_Madre_Fact { get; set; }
        //[Required(ErrorMessage = "El campo parroquia es obligatorio")]
        public string IdParroquia_Madre_Fact { get; set; }
        [Required(ErrorMessage = "El campo naturaleza es obligatorio")]
        public string Naturaleza_Representante { get; set; }
        [Required(ErrorMessage = "El campo tipo de documento es obligatorio")]
        public string IdTipoDocumento_Representante { get; set; }
        //[Required(ErrorMessage = "El campo cédula o Ruc es obligatorio")]
        public string CedulaRuc_Representante { get; set; }
        //[Required(ErrorMessage = "El campo nombres es obligatorio")]
        //[StringLength(1, MinimumLength = 0, ErrorMessage = "el campo nombres debe tener máximo 100 caracteres")]
        public string Nombres_Representante { get; set; }
        //[Required(ErrorMessage = "El campo apellidos es obligatorio")]
        //[StringLength(1, MinimumLength = 0, ErrorMessage = "el campo apellidos debe tener máximo 100 caracteres")]
        public string Apellidos_Representante { get; set; }
        public string NombreCompleto_Representante { get; set; }
        public string RazonSocial_Representante { get; set; }
        //[Required(ErrorMessage = "El campo direccion es obligatorio")]
        public string Direccion_Representante { get; set; }
        public string Telefono_Representante { get; set; }
        //[Required(ErrorMessage = "El campo celular es obligatorio")]
        public string Celular_Representante { get; set; }
        //[Required(ErrorMessage = "El campo correo es obligatorio")]
        public string Correo_Representante { get; set; }
        //[Required(ErrorMessage = "El campo sexo es obligatorio")]
        public string Sexo_Representante { get; set; }
        public Nullable<System.DateTime> FechaNacimiento_Representante { get; set; }
        public string CodCatalogoCONADIS_Representante { get; set; }
        public Nullable<double> PorcentajeDiscapacidad_Representante { get; set; }
        public string NumeroCarnetConadis_Representante { get; set; }
        public Nullable<int> IdGrupoEtnico_Representante { get; set; }
        //[Required(ErrorMessage = "El campo religión es obligatorio")]
        public Nullable<int> IdReligion_Representante { get; set; }
        public string IdEstadoCivil_Representante { get; set; }
        public bool AsisteCentroCristiano_Representante { get; set; }
        public string IdPais_Representante { get; set; }
        public string Cod_Region_Representante { get; set; }
        public string IdProvincia_Representante { get; set; }
        public string IdCiudad_Representante { get; set; }
        public string IdParroquia_Representante { get; set; }
        public string Sector_Representante { get; set; }
        public int IdCatalogoPAREN_Representante { get; set; }
        public Nullable<int> IdCatalogoFichaInst_Representante { get; set; }
        public string EmpresaTrabajo_Representante { get; set; }
        public string DireccionTrabajo_Representante { get; set; }
        public string TelefonoTrabajo_Representante { get; set; }
        public string CargoTrabajo_Representante { get; set; }
        public Nullable<int> AniosServicio_Representante { get; set; }
        public Nullable<double> IngresoMensual_Representante { get; set; }
        public bool VehiculoPropio_Representante { get; set; }
        public string Marca_Representante { get; set; }
        public string Modelo_Representante { get; set; }
        public Nullable<int> AnioVehiculo_Representante { get; set; }
        public bool CasaPropia_Representante { get; set; }
        public bool EstaFallecido_Representante { get; set; }
        public Nullable<int> IdProfesion_Representante { get; set; }
        //[Required(ErrorMessage = "El campo se factura es obligatorio")]
        public bool SeFactura_Representante { get; set; }
        //[Required(ErrorMessage = "El campo tipo de cliente es obligatorio")]
        public Nullable<int> Idtipo_cliente_Representante { get; set; }
        public string IdTipoCredito_Representante { get; set; }
        //[Required(ErrorMessage = "El campo ciudad es obligatorio")]
        public string IdCiudad_Representante_Fact { get; set; }
        //[Required(ErrorMessage = "El campo parroquia es obligatorio")]
        public string IdParroquia_Representante_Fact { get; set; }
        public double SueldoPadre { get; set; }
        public double SueldoMadre { get; set; }
        public double OtroIngresoPadre { get; set; }
        public double OtroIngresoMadre { get; set; }
        public double GastoAlimentacion { get; set; }
        public double GastoEducacion { get; set; }
        public double GastoServicioBasico { get; set; }
        public double GastoSalud { get; set; }
        public double GastoArriendo { get; set; }
        public double GastoPrestamo { get; set; }
        public double OtroGasto { get; set; }
        public int IdCatalogoESTADM { get; set; }
        public bool Estado { get; set; }
        public string IdUsuarioCreacion { get; set; }
        public Nullable<System.DateTime> FechaCreacion { get; set; }
        public string IdUsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string IdUsuarioRevision { get; set; }
        public Nullable<System.DateTime> FechaRevision { get; set; }
        public Nullable<System.DateTime> FechaPreMatriculacion { get; set; }
        public string IdUsuarioAnulacion { get; set; }
        public Nullable<System.DateTime> FechaAnulacion { get; set; }
        public string MotivoAnulacion { get; set; }


        #region Campos que no exiten en la tabla
        public int TienePadre { get; set; }
        public int TieneMadre { get; set; }
        public double TotalIngresos { get; set; }
        public int AspiranteValido { get; set; }
        public int PadreValido { get; set; }
        public int MadreValido { get; set; }
        public int RepresentanteValido { get; set; }
        public double TotalEgresos { get; set; }
        public double Saldo { get; set; }
        public bool info_valido_aspirante { get; set; }
        public bool info_valido_padre { get; set; }
        public bool info_valido_madre { get; set; }
        public bool info_valido_representante { get; set; }
        public DateTime FechaActual { get; set; }
        public string CodigoEstadoAdmision { get; set; }
        public string EstadoAdmision { get; set; }
        public string FechaString { get; set; }
        public int CantidadArchivos { get; set; }
        public string Descripcion { get; set; }
        public string NomSede { get; set; }
        public string NomNivel { get; set; }
        public string NomJornada { get; set; }
        public string NomCurso { get; set; }
        public string NomParalelo { get; set; }
        public int OrdenCurso { get; set; }
        public int OrdenNivel { get; set; }
        public int OrdenJornada { get; set; }
        public int OrdenParalelo { get; set; }
        public string Representante { get; set; }
        [Required(ErrorMessage = "El campo foto del aspirante es obligatorio")]
        public HttpPostedFileBase FotoAspirante { get; set; }
        public HttpPostedFileBase CedulaAspirante { get; set; }
        public HttpPostedFileBase CedulaRepresentante { get; set; }
        public HttpPostedFileBase RecordAcademicoAspirante { get; set; }
        public HttpPostedFileBase PagoAlDiaAspirante { get; set; }
        public HttpPostedFileBase CertificadoLaboral { get; set; }
        public int EsRepresentante_padre { get; set; }
        public int EsRepresentante_madre { get; set; }
        public int EsRepresentante_otro { get; set; }
        #endregion

        #region PreMatricula
        public decimal IdPreMatricula { get; set; }
        public decimal IdAlumno { get; set; }
        public decimal IdPersona_Aspirante { get; set; }
        public decimal IdPersona_Padre { get; set; }
        public decimal IdPersona_Madre { get; set; }
        public decimal IdPersona_Representante { get; set; }
        public tb_persona_Info info_persona_alumno { get; set; }
        public tb_persona_Info info_persona_padre { get; set; }
        public tb_persona_Info info_persona_madre { get; set; }
        public tb_persona_Info info_persona_representante { get; set; }
        public Nullable<int> IdCatalogoESTPREMAT { get; set; }
        public int IdSucursal { get; set; }
        public int IdParalelo { get; set; }
        public int IdPlantilla { get; set; }
        public int IdMecanismo { get; set; }
        public string Observacion { get; set; }
        public DateTime Fecha { get; set; }
        public bool EsPatrocinado { get; set; }
        public decimal IdMecanismoDet { get; set; }
        public Nullable<int> IdEmpresa_rol { get; set; }
        public Nullable<decimal> IdEmpleado { get; set; }
        public string IdCatalogo_FormaPago { get; set; }
        public bool AplicaDescuentoNomina { get; set; }
        public int IdPuntoVta { get; set; }
        public string vt_serie1 { get; set; }
        public string vt_serie2 { get; set; }
        public string vt_NumFactura { get; set; }
        public string IdComboCurso { get; set; }
        public double ValorPlantilla { get; set; }
        public double ValorPlantillaProntoPago { get; set; }
        public List<aca_AnioLectivo_Curso_Documento_Info> lst_documentos { get; set; }
        public List<aca_Matricula_Info> lst_matricula_curso { get; set; }
        public List<aca_PreMatricula_Rubro_Info> lst_PreMatriculaDet { get; set; }
        public int IdCatalogoPAREN_OtroFamiliar { get; set; }

        public string IdUsuarioSesion { get; set; }
        public int CantEstudiantes { get; set; }
        #endregion
    }
}
