using Core.Info.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Info.Academico
{
    public class aca_Alumno_Info
    {
        public decimal IdTransaccionSession { get; set; }
        public int IdEmpresa { get; set; }
        public decimal IdAlumno { get; set; }
        [StringLength(50, MinimumLength = 0, ErrorMessage = "el campo código debe tener máximo 50")]
        public string Codigo { get; set; }
        public decimal IdPersona { get; set; }
        public string Direccion { get; set; }
        public string Celular { get; set; }
        public string Correo { get; set; }
        public Nullable<bool> Estado { get; set; }
        public int IdCatalogoESTMAT { get; set; }
        public Nullable<int> IdCurso { get; set; }
        public int IdCatalogoESTALU { get; set; }
        public string MotivoNoMatricula { get; set; }
        public System.DateTime FechaIngreso { get; set; }
        [StringLength(300, MinimumLength = 1, ErrorMessage = "el campo descripción debe tener mínimo 1 caracter y máximo 300")]
        public string LugarNacimiento { get; set; }
        public string IdPais { get; set; }
        public string Cod_Region { get; set; }
        public string IdProvincia { get; set; }
        public string IdCiudad { get; set; }
        public string IdParroquia { get; set; }
        [StringLength(500, MinimumLength = 0, ErrorMessage = "el campo numero de carnet de conadis debe tener máximo 500 caracteres")]
        public string Sector { get; set; }
        public Nullable<bool> Dificultad_Lectura { get; set; }
        public Nullable<bool> Dificultad_Escritura { get; set; }
        public Nullable<bool> Dificultad_Matematicas { get; set; }
        public string IdUsuarioCreacion { get; set; }
        public Nullable<System.DateTime> FechaCreacion { get; set; }
        public string IdUsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public string IdUsuarioAnulacion { get; set; }
        public Nullable<System.DateTime> FechaAnulacion { get; set; }
        [Required(ErrorMessage = "El campo motivo de anulación es obligatorio")]
        public string MotivoAnulacion { get; set; }

        #region Campos que no existen en la tabla
        public byte[] alu_foto { get; set; }
        public string NomCatalogoESTALU { get; set; }
        public string NomCatalogoESTMAT { get; set; }

        #region Alumno
        [Required(ErrorMessage = "El campo tipo de documento es obligatorio")]
        public string IdTipoDocumento { get; set; }
        [Required(ErrorMessage = "El campo naturaleza es obligatorio")]
        public string pe_Naturaleza { get; set; }
        [Required(ErrorMessage = "El campo nombre completo es obligatorio")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "el campo nombre completo debe tener mínimo 2 caracter y máximo 200 caracteres")]
        public string pe_nombreCompleto { get; set; }
        [StringLength(100, MinimumLength = 0, ErrorMessage = "el campo apellidos debe tener máximo 100 caracteres")]
        public string pe_apellido { get; set; }
        [StringLength(100, MinimumLength = 0, ErrorMessage = "el campo nombres debe tener máximo 100 caracteres")]
        public string pe_nombre { get; set; }
        [Required(ErrorMessage = "El campo numero de documento es obligatorio")]
        public string pe_cedulaRuc { get; set; }
        public string pe_telfono_Contacto { get; set; }
        public string pe_sexo { get; set; }
        public Nullable<System.DateTime> pe_fechaNacimiento { get; set; }
        public string CodCatalogoSangre { get; set; }
        public string CodCatalogoCONADIS { get; set; }
        public Nullable<double> PorcentajeDiscapacidad { get; set; }
        [StringLength(200, MinimumLength = 0, ErrorMessage = "el campo numero de carnet de conadis debe tener máximo 200 caracteres")]
        public string NumeroCarnetConadis { get; set; }
        public List<aca_AlumnoDocumento_Info> lst_alumno_documentos { get; set; }
        public string IdUsuario { get; set; }
        public Nullable<int> IdReligion { get; set; }
        public Nullable<bool> AsisteCentroCristiano { get; set; }
        public Nullable<int> IdGrupoEtnico { get; set; }
        public List<aca_Familia_Info> lst_familia { get; set; }
        #endregion

        #region Datos padre
        public int IdCatalogo_padre { get; set; }
        public decimal IdPersona_padre { get; set; }
        public string IdTipoDocumento_padre { get; set; }
        public string pe_Naturaleza_padre { get; set; }
        [Required(ErrorMessage = "El campo nombre completo es obligatorio")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "el campo nombre completo debe tener mínimo 2 caracter y máximo 200 caracteres")]
        public string pe_nombreCompleto_padre { get; set; }
        [StringLength(100, MinimumLength = 0, ErrorMessage = "el campo apellidos debe tener máximo 100 caracteres")]
        public string pe_apellido_padre { get; set; }
        [StringLength(100, MinimumLength = 0, ErrorMessage = "el campo nombres debe tener máximo 100 caracteres")]
        public string pe_nombre_padre { get; set; }
        public string pe_cedulaRuc_padre { get; set; }
        [StringLength(150, MinimumLength = 0, ErrorMessage = "el campo razón social debe tener máximo 150 caracteres")]
        public string pe_razonSocial_padre { get; set; }
        public string Direccion_padre { get; set; }
        public string pe_telfono_Contacto_padre { get; set; }
        public string Celular_padre { get; set; }
        public string Correo_padre { get; set; }
        public string pe_sexo_padre { get; set; }
        public string IdEstadoCivil_padre { get; set; }
        public Nullable<System.DateTime> pe_fechaNacimiento_padre { get; set; }
        public string CodCatalogoSangre_padre { get; set; }
        public string CodCatalogoCONADIS_padre { get; set; }
        public Nullable<double> PorcentajeDiscapacidad_padre { get; set; }
        [StringLength(200, MinimumLength = 0, ErrorMessage = "el campo numero de carnet de conadis debe tener máximo 200 caracteres")]
        public string NumeroCarnetConadis_padre { get; set; }
        public bool SeFactura_padre { get; set; }
        public bool EsRepresentante_padre { get; set; }
        public Nullable<int> IdCatalogoFichaInst_padre { get; set; }
        [StringLength(200, MinimumLength = 0, ErrorMessage = "el campo empresa debe tener máximo 200")]
        public string EmpresaTrabajo_padre { get; set; }
        [StringLength(500, MinimumLength = 0, ErrorMessage = "el campo dirección debe tener máximo 500")]
        public string DireccionTrabajo_padre { get; set; }
        [StringLength(200, MinimumLength = 0, ErrorMessage = "el campo telefono debe tener máximo 200")]
        public string TelefonoTrabajo_padre { get; set; }
        [StringLength(200, MinimumLength = 0, ErrorMessage = "el campo cargo debe tener máximo 200")]
        public string CargoTrabajo_padre { get; set; }
        public Nullable<int> AniosServicio_padre { get; set; }
        public Nullable<double> IngresoMensual_padre { get; set; }
        public bool VehiculoPropio_padre { get; set; }
        [StringLength(50, MinimumLength = 0, ErrorMessage = "el campo marca debe tener máximo 50")]
        public string Marca_padre { get; set; }
        [StringLength(50, MinimumLength = 0, ErrorMessage = "el campo modelo debe tener máximo 50")]
        public string Modelo_padre { get; set; }
        public bool CasaPropia_padre { get; set; }
        public Nullable<int> IdProfesion_padre { get; set; }
        public bool info_valido_padre { get; set; }
        [Required(ErrorMessage = "El campo tipo de cliente es obligatorio")]
        public int Idtipo_cliente_padre { get; set; }
        public string IdTipoCredito_padre { get; set; }
        [Required(ErrorMessage = "El campo ciudad es obligatorio")]
        public string IdCiudad_padre_fact { get; set; }
        [Required(ErrorMessage = "El campo parroquia es obligatorio")]
        public string IdParroquia_padre_fact { get; set; }
        [Required(ErrorMessage = "El campo sucursal es obligatorio")]
        public Nullable<int> AnioVehiculo_padre { get; set; }
        public Nullable<int> IdReligion_padre { get; set; }
        public Nullable<bool> AsisteCentroCristiano_padre { get; set; }
        public bool EstaFallecido_padre { get; set; }
        public string IdPais_padre { get; set; }
        public string Cod_Region_padre { get; set; }
        public string IdProvincia_padre { get; set; }
        public string IdCiudad_padre { get; set; }
        public string IdParroquia_padre { get; set; }
        [StringLength(500, MinimumLength = 0, ErrorMessage = "el campo numero de carnet de conadis debe tener máximo 500 caracteres")]
        public string Sector_padre { get; set; }
        public Nullable<int> IdGrupoEtnico_padre { get; set; }
        #endregion

        #region Datos madre
        public int IdCatalogo_madre { get; set; }
        public decimal IdPersona_madre { get; set; }
        public string IdTipoDocumento_madre { get; set; }
        public string pe_Naturaleza_madre { get; set; }
        [Required(ErrorMessage = "El campo nombre completo es obligatorio")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "el campo nombre completo debe tener mínimo 2 caracter y máximo 200 caracteres")]
        public string pe_nombreCompleto_madre { get; set; }
        [StringLength(150, MinimumLength = 0, ErrorMessage = "el campo razón social debe tener máximo 150 caracteres")]
        public string pe_razonSocial_madre { get; set; }
        [StringLength(100, MinimumLength = 0, ErrorMessage = "el campo apellidos debe tener máximo 100 caracteres")]
        public string pe_apellido_madre { get; set; }
        [StringLength(100, MinimumLength = 0, ErrorMessage = "el campo nombres debe tener máximo 100 caracteres")]
        public string pe_nombre_madre { get; set; }
        public string pe_cedulaRuc_madre { get; set; }
        public string Direccion_madre { get; set; }
        public string pe_telfono_Contacto_madre { get; set; }
        public string Celular_madre { get; set; }
        public string Correo_madre { get; set; }
        public string pe_sexo_madre { get; set; }
        public string IdEstadoCivil_madre { get; set; }
        public Nullable<System.DateTime> pe_fechaNacimiento_madre { get; set; }
        public string CodCatalogoSangre_madre { get; set; }
        public string CodCatalogoCONADIS_madre { get; set; }
        public Nullable<double> PorcentajeDiscapacidad_madre { get; set; }
        [StringLength(200, MinimumLength = 0, ErrorMessage = "el campo numero de carnet de conadis debe tener máximo 200 caracteres")]
        public string NumeroCarnetConadis_madre { get; set; }
        public bool SeFactura_madre { get; set; }
        public bool EsRepresentante_madre { get; set; }
        public Nullable<int> IdCatalogoFichaInst_madre { get; set; }
        [StringLength(200, MinimumLength = 0, ErrorMessage = "el campo empresa debe tener máximo 200")]
        public string EmpresaTrabajo_madre { get; set; }
        [StringLength(500, MinimumLength = 0, ErrorMessage = "el campo dirección debe tener máximo 500")]
        public string DireccionTrabajo_madre { get; set; }
        [StringLength(200, MinimumLength = 0, ErrorMessage = "el campo telefono debe tener máximo 200")]
        public string TelefonoTrabajo_madre { get; set; }
        [StringLength(200, MinimumLength = 0, ErrorMessage = "el campo cargo debe tener máximo 200")]
        public string CargoTrabajo_madre { get; set; }
        public Nullable<int> AniosServicio_madre { get; set; }
        public Nullable<double> IngresoMensual_madre { get; set; }
        public bool VehiculoPropio_madre { get; set; }
        [StringLength(50, MinimumLength = 0, ErrorMessage = "el campo marca debe tener máximo 50")]
        public string Marca_madre { get; set; }
        [StringLength(50, MinimumLength = 0, ErrorMessage = "el campo modelo debe tener máximo 50")]
        public string Modelo_madre { get; set; }
        public bool CasaPropia_madre { get; set; }
        public Nullable<int> IdProfesion_madre { get; set; }
        public bool info_valido_madre { get; set; }
        [Required(ErrorMessage = "El campo tipo de cliente es obligatorio")]
        public int Idtipo_cliente_madre { get; set; }
        public string IdTipoCredito_madre { get; set; }
        [Required(ErrorMessage = "El campo ciudad es obligatorio")]
        public string IdCiudad_madre_fact { get; set; }
        [Required(ErrorMessage = "El campo parroquia es obligatorio")]
        public string IdParroquia_madre_fact { get; set; }
        [Required(ErrorMessage = "El campo sucursal es obligatorio")]
        public Nullable<int> AnioVehiculo_madre { get; set; }
        public Nullable<int> IdReligion_madre { get; set; }
        public Nullable<bool> AsisteCentroCristiano_madre { get; set; }
        public bool EstaFallecido_madre { get; set; }
        public string IdPais_madre { get; set; }
        public string Cod_Region_madre { get; set; }
        public string IdProvincia_madre { get; set; }
        public string IdCiudad_madre { get; set; }
        public string IdParroquia_madre { get; set; }
        [StringLength(500, MinimumLength = 0, ErrorMessage = "el campo numero de carnet de conadis debe tener máximo 500 caracteres")]
        public string Sector_madre { get; set; }
        public Nullable<int> IdGrupoEtnico_madre { get; set; }
        #endregion

        #region Datos representante
        public int IdCatalogo_representante { get; set; }
        public decimal IdPersona_representante { get; set; }
        public string IdTipoDocumento_representante { get; set; }
        public string pe_Naturaleza_representante { get; set; }
        [Required(ErrorMessage = "El campo nombre completo es obligatorio")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "el campo nombre completo debe tener mínimo 2 caracter y máximo 200 caracteres")]
        public string pe_nombreCompleto_representante { get; set; }
        [StringLength(150, MinimumLength = 0, ErrorMessage = "el campo razón social debe tener máximo 150 caracteres")]
        public string pe_razonSocial_representante { get; set; }
        [StringLength(100, MinimumLength = 0, ErrorMessage = "el campo apellidos debe tener máximo 100 caracteres")]
        public string pe_apellido_representante { get; set; }
        [StringLength(100, MinimumLength = 0, ErrorMessage = "el campo nombres debe tener máximo 100 caracteres")]
        public string pe_nombre_representante { get; set; }
        public string pe_cedulaRuc_representante { get; set; }
        public string Direccion_representante { get; set; }
        public string pe_telfono_Contacto_representante { get; set; }
        public string Celular_representante { get; set; }
        public string Correo_representante { get; set; }
        public string pe_sexo_representante { get; set; }
        public string IdEstadoCivil_representante { get; set; }
        public Nullable<System.DateTime> pe_fechaNacimiento_representante { get; set; }
        public string CodCatalogoSangre_representante { get; set; }
        public string CodCatalogoCONADIS_representante { get; set; }
        public Nullable<double> PorcentajeDiscapacidad_representante { get; set; }
        [StringLength(200, MinimumLength = 0, ErrorMessage = "el campo numero de carnet de conadis debe tener máximo 200 caracteres")]
        public string NumeroCarnetConadis_representante { get; set; }
        public bool SeFactura_representante { get; set; }
        public bool EsRepresentante_representante { get; set; }
        public Nullable<int> IdCatalogoFichaInst_representante { get; set; }
        [StringLength(200, MinimumLength = 0, ErrorMessage = "el campo empresa debe tener máximo 200")]
        public string EmpresaTrabajo_representante { get; set; }
        [StringLength(500, MinimumLength = 0, ErrorMessage = "el campo dirección debe tener máximo 500")]
        public string DireccionTrabajo_representante { get; set; }
        [StringLength(200, MinimumLength = 0, ErrorMessage = "el campo telefono debe tener máximo 200")]
        public string TelefonoTrabajo_representante { get; set; }
        [StringLength(200, MinimumLength = 0, ErrorMessage = "el campo cargo debe tener máximo 200")]
        public string CargoTrabajo_representante { get; set; }
        public Nullable<int> AniosServicio_representante { get; set; }
        public Nullable<double> IngresoMensual_representante { get; set; }
        public bool VehiculoPropio_representante { get; set; }
        [StringLength(50, MinimumLength = 0, ErrorMessage = "el campo marca debe tener máximo 50")]
        public string Marca_representante { get; set; }
        [StringLength(50, MinimumLength = 0, ErrorMessage = "el campo modelo debe tener máximo 50")]
        public string Modelo_representante { get; set; }
        public bool CasaPropia_representante { get; set; }
        public Nullable<int> IdProfesion_representante { get; set; }
        public bool info_valido_representante { get; set; }
        [Required(ErrorMessage = "El campo tipo de cliente es obligatorio")]
        public int Idtipo_cliente_representante { get; set; }
        public string IdTipoCredito_representante { get; set; }
        [Required(ErrorMessage = "El campo ciudad es obligatorio")]
        public string IdCiudad_representante_fact { get; set; }
        [Required(ErrorMessage = "El campo parroquia es obligatorio")]
        public string IdParroquia_representante_fact { get; set; }
        [Required(ErrorMessage = "El campo sucursal es obligatorio")]
        public Nullable<int> AnioVehiculo_representante { get; set; }
        public Nullable<int> IdReligion_representante { get; set; }
        public Nullable<bool> AsisteCentroCristiano_representante { get; set; }
        public bool EstaFallecido_representante { get; set; }
        public string IdPais_representante { get; set; }
        public string Cod_Region_representante { get; set; }
        public string IdProvincia_representante { get; set; }
        public string IdCiudad_representante { get; set; }
        public string IdParroquia_representante { get; set; }
        [StringLength(500, MinimumLength = 0, ErrorMessage = "el campo numero de carnet de conadis debe tener máximo 500 caracteres")]
        public string Sector_representante { get; set; }
        public Nullable<int> IdGrupoEtnico_representante { get; set; }
        #endregion

        public tb_persona_Info info_persona_alumno { get; set; }
        public tb_persona_Info info_persona_padre { get; set; }
        public tb_persona_Info info_persona_madre { get; set; }
        public tb_persona_Info info_persona_representante { get; set; }
        public string anio { get; set; }
        public string mes { get; set; }
        public string dia { get; set; }
        public int IdSede { get; set; }
        public int IdAnioRegistro { get; set; }
        public string AnioCodigo { get; set; }
        public int IdSucursal { get; set; }

        #region Datos Alumno Periodo Actual
        public string NomRepLegal { get; set; }
        public string CelularRepresentante { get; set; }
        public string CorreoRepLegal { get; set; }
        public string NomRepEconomico { get; set; }
        public string CelularEmiteFactura { get; set; }
        public string correoRepEconomico { get; set; }
        public int IdAnio { get; set; }
        public int IdJornada { get; set; }
        public int IdNivel { get; set; }
        public int IdParalelo { get; set; }
        public string NomSede { get; set; }
        public string NomNivel { get; set; }
        public string NomJornada { get; set; }
        public string NomCurso { get; set; }
        public string NomParalelo { get; set; }
        public int OrdenParalelo { get; set; }
        public int OrdenCurso { get; set; }
        public int OrdenNivel { get; set; }
        public int OrdenJornada { get; set; }
        public decimal IdMatricula { get; set; }
        public double Saldo { get; set; }
        public double SaldoProntoPago { get; set; }
        public string NomPlantillaTipo { get; set; }
        public string TelefonoEmiteFactura { get; set; }
        public string TelefonoRepresentante { get; set; }
        #endregion

        #region Admision
        public decimal IdAdmision { get; set; }
        public decimal IdPreMatricula { get; set; }
        #endregion

        #endregion
    }
}
