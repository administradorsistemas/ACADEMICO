function ValidarEnter() {
    $(document).ready(function () {
        $('form').keypress(function (e) {
            if (e == 13) {
                return false;
            }
        });

        $('input').keypress(function (e) {
            if (e.which == 13) {
                return false;
            }
        });
    });
}
function IrAlInicio() {
    $(document).ready(function () {
        $('body, html').animate({
            scrollTop: '0px'
        }, 300);
    });
}


function SiguienteAspirante() {
    if ($("#AspiranteValido").val() == "1") {
        ValidarDatosRegistro_Aspirante();
    }
}
function SiguientePadre() {
    if ($("#PadreValido").val() == "1") {
        $("#TienePadre").val("1");
        ValidarDatosRegistro_Padre();
    }
    else {
        if ($("#CedulaRuc_Padre").val() == "") {
            $("#TienePadre").val("0");

            $("#MensajeError").html("");
            $("#DivError").hide();

            $("#DatosPadre").hide();
            $("#BtnPadre").attr("class", "w-10 h-10 rounded-full button text-gray-600 bg-gray-200 dark:bg-dark-1");
            $("#DatosMadre").show();
            $("#BtnMadre").attr("class", "w-10 h-10 rounded-full button text-white bg-theme-1");
        }
    }
}
function AnteriorPadre() {
    $("#DatosPadre").hide();
    $("#BtnPadre").attr("class", "w-10 h-10 rounded-full button text-gray-600 bg-gray-200 dark:bg-dark-1");
    $("#DatosAspirante").show();
    $("#BtnAspirante").attr("class", "w-10 h-10 rounded-full button text-white bg-theme-1");
}
function SiguienteMadre() {
    if ($("#MadreValido").val() == "1") {
        $("#TieneMadre").val("1");
        ValidarDatosRegistro_Madre();
    }
    else {
        if ($("#CedulaRuc_Madre").val() == "") {
            $("#TieneMadre").val("0");

            $("#MensajeError").html("");
            $("#DivError").hide();

            $("#DatosMadre").hide();
            $("#BtnMadre").attr("class", "w-10 h-10 rounded-full button text-gray-600 bg-gray-200 dark:bg-dark-1");
            $("#DatosRepresentante").show();
            $("#BtnRepresentante").attr("class", "w-10 h-10 rounded-full button text-white bg-theme-1");
        }
    }

    if ($("#TieneMadre").val() == "1") {
        $("#BtnMadreRep").show();
    }
    else {
        $("#BtnMadreRep").hide();
    }

    if ($("#TienePadre").val() == "1") {
        $("#BtnPadreRep").show();
    }
    else {
        $("#BtnPadreRep").hide();
    }
}
function AnteriorMadre() {
    $("#DatosMadre").hide();
    $("#BtnMadre").attr("class", "w-10 h-10 rounded-full button text-gray-600 bg-gray-200 dark:bg-dark-1");
    $("#DatosPadre").show();
    $("#BtnPadre").attr("class", "w-10 h-10 rounded-full button text-white bg-theme-1");
}
function SiguienteRepresentante() {
    if ($("#RepresentanteValido").val() == "1") {
        ValidarDatosRegistro_Representante();
    }
}
function AnteriorRepresentante() {
    $("#DatosRepresentante").hide();
    $("#BtnRepresentante").attr("class", "w-10 h-10 rounded-full button text-gray-600 bg-gray-200 dark:bg-dark-1");
    $("#DatosMadre").show();
    $("#BtnMadre").attr("class", "w-10 h-10 rounded-full button text-white bg-theme-1");
}
function SiguienteSocioEconomico() {
    ValidarDatosSocioEconomico();
}
function AnteriorSocioEconomico() {
    $("#DatosSocioEconomico").hide();
    $("#BtnSocioEconomico").attr("class", "w-10 h-10 rounded-full button text-gray-600 bg-gray-200 dark:bg-dark-1");
    $("#DatosRepresentante").show();
    $("#BtnRepresentante").attr("class", "w-10 h-10 rounded-full button text-white bg-theme-1");
}
function SiguienteArchivos() {
    ValidarArchivos();
}
function AnteriorArchivos() {
    $("#DatosArchivos").hide();
    $("#BtnArchivos").attr("class", "w-10 h-10 rounded-full button text-gray-600 bg-gray-200 dark:bg-dark-1");
    $("#DatosSocioEconomico").show();
    $("#BtnSocioEconomico").attr("class", "w-10 h-10 rounded-full button text-white bg-theme-1");
}
function SiguienteTerminos() {
    ValidarTerminos();
}
function AnteriorTerminos() {
    $("#DatosTerminos").hide();
    $("#DatosTerminos").attr("class", "w-10 h-10 rounded-full button text-gray-600 bg-gray-200 dark:bg-dark-1");
    $("#DatosArchivos").show();
    $("#BtnArchivos").attr("class", "w-10 h-10 rounded-full button text-white bg-theme-1");
}

function Validar_cedula_ruc_Aspirante() {
    var url_sistema = GetPathServer();
    console.log(url_sistema);
    var datos = {
        naturaleza: $("#Naturaleza_Aspirante").val(),
        tipo_documento: $("#IdTipoDocumento_Aspirante").val(),
        cedula_ruc: $("#CedulaRuc_Aspirante").val(),
    }

    $.ajax({
        type: 'POST',
        data: JSON.stringify(datos),
        url: url_sistema + '/Admision/Validar_cedula_ruc',
        async: false,
        bDeferRender: true,
        bProcessing: true,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data.isValid == true) {
                $("#error_documento_aspirante").hide();
                $("#Naturaleza_Aspirante").val(data.return_naturaleza);
                $("#AspiranteValido").val("1");
            }
            else {
                $("#error_documento_aspirante").show();
                $("#AspiranteValido").val("0");
            }
        },
        error: function (error) {
        }
    });
};


function ValidarDatosRegistro_Aspirante() {
    var mensaje = "";

    if ($("#IdAnio").val() == "0") {
        mensaje += "año lectivo, ";
    }
    if ($("#IdSede").val() == "0") {
        mensaje += "sede, ";
    }
    if ($("#IdJornada").val() == "0") {
        mensaje += "jornada, ";
    }
    if ($("#IdNivel").val() == "0") {
        mensaje += "nivel, ";
    }
    if ($("#IdCurso").val() == "0") {
        mensaje += "curso, ";
    }
    if ($("#Naturaleza_Aspirante").val() == "") {
        mensaje += "naturaleza, ";
    }
    if ($("#IdTipoDocumento_Aspirante").val() == "") {
        mensaje += "tipo de documento, ";
    }
    else {
        if ($("#IdTipoDocumento_Aspirante").val() == "RUC") {
            mensaje += "tipo de documento no válido, ";
            //$("#MensajeError").html("Los datos del aspirante no son válidos, tipo de documento incorrecto");
            //$("#DivError").show();
        }
    }
    if ($("#CedulaRuc_Aspirante").val() == "") {
        mensaje += "número de cédula, ";
        //$("#MensajeError").html("Los datos del aspirante no son válidos, debe de ingresar número de cédula");
        //$("#DivError").show();
    }
    if ($("#Nombres_Aspirante").val() == "" || $("#Apellidos_Aspirante").val() == "") {
        mensaje += "nombres y apellidos, ";
        //$("#MensajeError").html("Los datos del aspirante no son válidos, debe de ingresar nombres y apellidos");
        //$("#DivError").show();
    }
    if ($("#LugarNacimiento_Aspirante").val() == "") {
        mensaje += "lugar de nacimiento, ";
    }
    if ($("#FechaNacimiento_Aspirante").val() == "" || $("#FechaNacimiento_Aspirante").val() == null) {
        mensaje += "fecha de nacimiento, ";
    }
    if ($("#IdPais_Aspirante").val() == "") {
        mensaje += "país, ";
    }
    if ($("#Cod_Region_Aspirante").val() == "") {
        mensaje += "región, ";
    }
    if ($("#IdProvincia_Aspirante").val() == "") {
        mensaje += "provincia, ";
    }
    if ($("#IdCiudad_Aspirante").val() == "") {
        mensaje += "ciudad, ";
    }
    if ($("#IdParroquia_Aspirante").val() == "") {
        mensaje += "parroquia, ";
    }
    if ($("#Sexo_Aspirante").val() == "") {
        mensaje += "sexo, ";
    }
    if ($("#CodCatalogoSangre_Aspirante").val() == "") {
        mensaje += "tipo de sangre, ";
    }
    if ($("#IdGrupoEtnico_Aspirante").val() == "0") {
        mensaje += "grupo étnico, ";
    }
    if ($("#Telefono_Aspirante").val() == "") {
        mensaje += "número de teléfono, ";
    }
    if ($("#Celular_Aspirante").val() == "") {
        mensaje += "número de celular, ";
    }
    if ($("#Correo_Aspirante").val() == "") {
        mensaje += "correo electrónico, ";
    }
    if ($("#IdReligion_Aspirante").val() == "0") {
        mensaje += "religión, ";
    }
    if ($("#AsisteCentroCristiano_Aspirante").prop('checked') == null) {
        mensaje += "asiste o no a Centro Cristiano, ";
    }
    if ($("#Direccion_Aspirante").val() == "") {
        mensaje += "dirección, ";
    }
    if ($("#Sector_Aspirante").val() == "") {
        mensaje += "sector, ";
    }
    if ($("#Dificultad_Lectura").prop('checked') == null) {
        mensaje += "dificultad en lectura, ";
    }
    if ($("#Dificultad_Escritura").prop('checked') == null) {
        mensaje += "dificultad en escritura, ";
    }
    if ($("#Dificultad_Matematicas").prop('checked') == null) {
        mensaje += "dificultad en matemáticas, ";
    }
    if ($("#CodCatalogoCONADIS_Aspirante").val() != "") {
        if ($("#PorcentajeDiscapacidad_Aspirante").val() == "") {
            mensaje += "porcentaje de discapacidad, ";
        }
        if ($("#NumeroCarnetConadis_Aspirante").val() == "") {
            mensaje += "número de carnet de Conadis, ";
        }
    }

    IrAlInicio();
    if (mensaje == "") {
        $("#MensajeError").html("");
        $("#DivError").hide();

        $("#DatosAspirante").hide();
        $("#BtnAspirante").attr("class", "w-10 h-10 rounded-full button text-gray-600 bg-gray-200 dark:bg-dark-1");
        $("#DatosPadre").show();
        $("#BtnPadre").attr("class", "w-10 h-10 rounded-full button text-white bg-theme-1");
    }
    else {
        $("#MensajeError").html("Ingresar o seleccionar: " + mensaje);
        $("#DivError").show();
    }
}

function ValidarDatosRegistro_Padre() {
    var mensaje = "";
    if ($("#Naturaleza_Padre").val() == "") {
        mensaje += "naturaleza, ";
    }
    if ($("#IdTipoDocumento_Padre").val() == "") {
        mensaje += "tipo de documento, ";
    }
    else {
        if ($("#IdTipoDocumento_Padre").val() == "RUC") {
            if ($("#RazonSocial_Padre").val() == "") {
                mensaje += "razón social, ";
            }
        }
        else {
            if ($("#Nombres_Padre").val() == "") {
                mensaje += "nombres, ";
            }
            if ($("#Apellidos_Padre").val() == "") {
                mensaje += "apellidos, ";
            }
        }
    }
    if ($("#CedulaRuc_Padre").val() == "") {
        mensaje += "número de cédula, ";
    }
    /*
    if ($("#FechaNacimiento_Padre").val() == "" || $("#FechaNacimiento_Padre").val() == null) {
        mensaje += "fecha de nacimiento, ";
    }
    if ($("#IdPais_Padre").val() == "") {
        mensaje += "país, ";
    }
    if ($("#Cod_Region_Padre").val() == "") {
        mensaje += "región, ";
    }
    if ($("#IdProvincia_Padre").val() == "") {
        mensaje += "provincia, ";
    }
    if ($("#IdCiudad_Padre").val() == "") {
        mensaje += "ciudad, ";
    }
    if ($("#IdParroquia_Padre").val() == "") {
        mensaje += "parroquia, ";
    }
    if ($("#Sexo_Padre").val() == "") {
        mensaje += "sexo, ";
    }
    if ($("#IdEstadoCivil_Padre").val() == "") {
        mensaje += "estado civil, ";
    }
    if ($("#IdGrupoEtnico_Padre").val() == "0") {
        mensaje += "grupo étnico, ";
    }
    if ($("#Telefono_Padre").val() == "") {
        mensaje += "número de teléfono, ";
    }
    if ($("#Celular_Padre").val() == "") {
        mensaje += "número de celular, ";
    }
    if ($("#Correo_Padre").val() == "") {
        mensaje += "correo electrónico, ";
    }
    if ($("#IdReligion_Padre").val() == "") {
        mensaje += "religión, ";
    }
    if ($("#AsisteCentroCristiano_Padre").prop('checked') == null) {
        mensaje += "asiste o no a Centro Cristiano, ";
    }
    if ($("#CasaPropia_Padre").prop('checked') == null) {
        mensaje += "tiene casa propia, ";
    }
    if ($("#VehiculoPropio_Padre").prop('checked') == null) {
        mensaje += "tiene casa propia, ";
    }
    else {
        if ($("#VehiculoPropio_Padre").prop('checked') == true) {
            if ($("#Marca_Padre").val() == "") {
                mensaje += "marca de vehículo, ";
            }
            if ($("#Modelo_Padre").val() == "") {
                mensaje += "modelo de vehículo, ";
            }
        }
    }  
    if ($("#Direccion_Padre").val() == "") {
        mensaje += "dirección, ";
    }
    if ($("#Sector_Padre").val() == "") {
        mensaje += "sector, ";
    }
    if ($("#EmpresaTrabajo_Padre").val() == "") {
        mensaje += "empresa donde trabaja, ";
    }
    if ($("#IdCatalogoFichaInst_Padre").val() == "0") {
        mensaje += "intrucción, ";
    }
    if ($("#IdProfesion_Padre").val() == "0") {
        mensaje += "profesión, ";
    }
    if ($("#CargoTrabajo_Padre").val() == "") {
        mensaje += "cargo, ";
    }
    if ($("#TelefonoTrabajo_Padre").val() == "") {
        mensaje += "teléfono trabajo, ";
    }
    if ($("#AniosServicio_Padre").val() == "") {
        mensaje += "años de servicio, ";
    }
    if ($("#IngresoMensual_Padre").val() == "") {
        mensaje += "ingreso mensual, ";
    }
    if ($("#DireccionTrabajo_Padre").val() == "") {
        mensaje += "dirección de trabajo, ";
    }
    if ($("#CodCatalogoCONADIS_Padre").val() != "") {
        if ($("#PorcentajeDiscapacidad_Padre").val() == "") {
            mensaje += "porcentaje de discapacidad, ";
        }
        if ($("#NumeroCarnetConadis_Padre").val() == "") {
            mensaje += "número de carnet de Conadis, ";
        }
    }

    if ($("#SeFactura_Padre").prop('checked') == true) {
        if ($("#Idtipo_cliente_Padre").val() == "0") {
            mensaje += "tipo de cliente, ";
        }
        if ($("#IdCiudad_Padre_Fact").val() == "") {
            mensaje += "ciudad para facturacion, ";
        }
        if ($("#IdParroquia_Padre_Fact").val() == "") {
            mensaje += "parroquia para facturacion, ";
        }
    }
    */
    IrAlInicio();
    if (mensaje == "") {
        $("#MensajeError").html("");
        $("#DivError").hide();

        $("#DatosPadre").hide();
        $("#BtnPadre").attr("class", "w-10 h-10 rounded-full button text-gray-600 bg-gray-200 dark:bg-dark-1");
        $("#DatosMadre").show();
        $("#BtnMadre").attr("class", "w-10 h-10 rounded-full button text-white bg-theme-1");
    }
    else {
        $("#MensajeError").html("Ingresar o seleccionar: " + mensaje);
        $("#DivError").show();
    }
}
function ValidarDatosRegistro_Madre() {
    var mensaje = "";
    if ($("#Naturaleza_Madre").val() == "") {
        mensaje += "naturaleza, ";
    }
    if ($("#IdTipoDocumento_Madre").val() == "") {
        mensaje += "tipo de documento, ";
    }
    else {
        if ($("#IdTipoDocumento_Madre").val() == "RUC") {
            if ($("#RazonSocial_Madre").val() == "") {
                mensaje += "razón social, ";
            }
        }
        else {
            if ($("#Nombres_Madre").val() == "") {
                mensaje += "nombres, ";
            }
            if ($("#Apellidos_Madre").val() == "") {
                mensaje += "apellidos, ";
            }
        }
    }
    if ($("#CedulaRuc_Madre").val() == "") {
        mensaje += "número de cédula, ";
    }
    /*
    if ($("#FechaNacimiento_Madre").val() == "" || $("#FechaNacimiento_Madre").val() == null) {
        mensaje += "fecha de nacimiento, ";
    }
    if ($("#IdPais_Madre").val() == "") {
        mensaje += "país, ";
    }
    if ($("#Cod_Region_Madre").val() == "") {
        mensaje += "región, ";
    }
    if ($("#IdProvincia_Madre").val() == "") {
        mensaje += "provincia, ";
    }
    if ($("#IdCiudad_Madre").val() == "") {
        mensaje += "ciudad, ";
    }
    if ($("#IdParroquia_Madre").val() == "") {
        mensaje += "parroquia, ";
    }
    if ($("#Sexo_Madre").val() == "") {
        mensaje += "sexo, ";
    }
    if ($("#IdEstadoCivil_Madre").val() == "") {
        mensaje += "estado civil, ";
    }
    if ($("#IdGrupoEtnico_Madre").val() == "0") {
        mensaje += "grupo étnico, ";
    }
    if ($("#Telefono_Madre").val() == "") {
        mensaje += "número de teléfono, ";
    }
    if ($("#Celular_Madre").val() == "") {
        mensaje += "número de celular, ";
    }
    if ($("#Correo_Madre").val() == "") {
        mensaje += "correo electrónico, ";
    }
    if ($("#IdReligion_Madre").val() == "") {
        mensaje += "religión, ";
    }
    if ($("#AsisteCentroCristiano_Madre").prop('checked') == null) {
        mensaje += "asiste o no a Centro Cristiano, ";
    }
    if ($("#CasaPropia_Madre").prop('checked') == null) {
        mensaje += "tiene casa propia, ";
    }
    if ($("#VehiculoPropio_Madre").prop('checked') == null) {
        mensaje += "tiene casa propia, ";
    }
    else {
        if ($("#VehiculoPropio_Madre").prop('checked') == true) {
            if ($("#Marca_Madre").val() == "") {
                mensaje += "marca de vehículo, ";
            }
            if ($("#Modelo_Madre").val() == "") {
                mensaje += "modelo de vehículo, ";
            }
        }
    }
    if ($("#Direccion_Madre").val() == "") {
        mensaje += "dirección, ";
    }
    if ($("#Sector_Madre").val() == "") {
        mensaje += "sector, ";
    }
    if ($("#EmpresaTrabajo_Madre").val() == "") {
        mensaje += "empresa donde trabaja, ";
    }
    if ($("#IdCatalogoFichaInst_Madre").val() == "0") {
        mensaje += "intrucción, ";
    }
    if ($("#IdProfesion_Madre").val() == "0") {
        mensaje += "profesión, ";
    }
    if ($("#CargoTrabajo_Madre").val() == "") {
        mensaje += "cargo, ";
    }
    if ($("#TelefonoTrabajo_Madre").val() == "") {
        mensaje += "teléfono trabajo, ";
    }
    if ($("#AniosServicio_Madre").val() == "") {
        mensaje += "años de servicio, ";
    }
    if ($("#IngresoMensual_Madre").val() == "") {
        mensaje += "ingreso mensual, ";
    }
    if ($("#DireccionTrabajo_Madre").val() == "") {
        mensaje += "dirección de trabajo, ";
    }
    if ($("#CodCatalogoCONADIS_Madre").val() != "") {
        if ($("#PorcentajeDiscapacidad_Madre").val() == "") {
            mensaje += "porcentaje de discapacidad, ";
        }
        if ($("#NumeroCarnetConadis_Madre").val() == "") {
            mensaje += "número de carnet de Conadis, ";
        }
    }

    if ($("#SeFactura_Madre").prop('checked') == true) {
        if ($("#Idtipo_cliente_Madre").val() == "0") {
            mensaje += "tipo de cliente, ";
        }
        if ($("#IdCiudad_Madre_Fact").val() == "") {
            mensaje += "ciudad para facturacion, ";
        }
        if ($("#IdParroquia_Madre_Fact").val() == "") {
            mensaje += "parroquia para facturacion, ";
        }
    }
    */
    IrAlInicio();
    if (mensaje == "") {
        $("#MensajeError").html("");
        $("#DivError").hide();

        $("#DatosMadre").hide();
        $("#BtnMadre").attr("class", "w-10 h-10 rounded-full button text-gray-600 bg-gray-200 dark:bg-dark-1");
        $("#DatosRepresentante").show();
        $("#BtnRepresentante").attr("class", "w-10 h-10 rounded-full button text-white bg-theme-1");
    }
    else {
        $("#MensajeError").html("Ingresar o seleccionar: " + mensaje);
        $("#DivError").show();
    }
}

function ValidarDatosRegistro_Representante() {
    var mensaje = "";
    if ($("#Representante").val() == "O") {
        if ($("#Naturaleza_Representante").val() == "") {
            mensaje += "naturaleza, ";
        }
        if ($("#IdTipoDocumento_Representante").val() == "") {
            mensaje += "tipo de documento, ";
        }
        else {
            if ($("#IdTipoDocumento_Representante").val() == "RUC") {
                if ($("#RazonSocial_Representante").val() == "") {
                    mensaje += "razón social, ";
                }
            }
            else {
                if ($("#Nombres_Representante").val() == "") {
                    mensaje += "nombres, ";
                }
                if ($("#Apellidos_Representante").val() == "") {
                    mensaje += "apellidos, ";
                }
            }
        }
        if ($("#CedulaRuc_Representante").val() == "") {
            mensaje += "número de cédula, ";
        }
        if ($("#FechaNacimiento_Representante").val() == "" || $("#FechaNacimiento_Representante").val() == null) {
            mensaje += "fecha de nacimiento, ";
        }
        if ($("#IdPais_Representante").val() == "") {
            mensaje += "país, ";
        }
        if ($("#Cod_Region_Representante").val() == "") {
            mensaje += "región, ";
        }
        if ($("#IdProvincia_Representante").val() == "") {
            mensaje += "provincia, ";
        }
        if ($("#IdCiudad_Representante").val() == "") {
            mensaje += "ciudad, ";
        }
        if ($("#IdParroquia_Representante").val() == "") {
            mensaje += "parroquia, ";
        }
        if ($("#Sexo_Representante").val() == "") {
            mensaje += "sexo, ";
        }
        if ($("#IdEstadoCivil_Representante").val() == "") {
            mensaje += "estado civil, ";
        }
        if ($("#IdGrupoEtnico_Representante").val() == "0") {
            mensaje += "grupo étnico, ";
        }
        if ($("#Telefono_Representante").val() == "") {
            mensaje += "número de teléfono, ";
        }
        if ($("#Celular_Representante").val() == "") {
            mensaje += "número de celular, ";
        }
        if ($("#Correo_Representante").val() == "") {
            mensaje += "correo electrónico, ";
        }
        if ($("#IdReligion_Representante").val() == "") {
            mensaje += "religión, ";
        }
        if ($("#AsisteCentroCristiano_Representante").prop('checked') == null) {
            mensaje += "asiste o no a Centro Cristiano, ";
        }
        if ($("#CasaPropia_Representante").prop('checked') == null) {
            mensaje += "tiene casa propia, ";
        }
        if ($("#VehiculoPropio_Representante").prop('checked') == null) {
            mensaje += "tiene casa propia, ";
        }
        else {
            if ($("#VehiculoPropio_Representante").prop('checked') == true) {
                if ($("#Marca_Representante").val() == "") {
                    mensaje += "marca de vehículo, ";
                }
                if ($("#Modelo_Representante").val() == "") {
                    mensaje += "modelo de vehículo, ";
                }
            }
        }
        if ($("#Direccion_Representante").val() == "") {
            mensaje += "dirección, ";
        }
        if ($("#Sector_Representante").val() == "") {
            mensaje += "sector, ";
        }
        if ($("#EmpresaTrabajo_Representante").val() == "") {
            mensaje += "empresa donde trabaja, ";
        }
        if ($("#IdCatalogoFichaInst_Representante").val() == "0") {
            mensaje += "intrucción, ";
        }
        if ($("#IdProfesion_Representante").val() == "0") {
            mensaje += "profesión, ";
        }
        if ($("#CargoTrabajo_Representante").val() == "") {
            mensaje += "cargo, ";
        }
        if ($("#TelefonoTrabajo_Representante").val() == "") {
            mensaje += "teléfono trabajo, ";
        }
        if ($("#AniosServicio_Representante").val() == "") {
            mensaje += "años de servicio, ";
        }
        if ($("#IngresoMensual_Representante").val() == "") {
            mensaje += "ingreso mensual, ";
        }
        if ($("#DireccionTrabajo_Representante").val() == "") {
            mensaje += "dirección de trabajo, ";
        }
        if ($("#CodCatalogoCONADIS_Representante").val() != "") {
            if ($("#PorcentajeDiscapacidad_Representante").val() == "") {
                mensaje += "porcentaje de discapacidad, ";
            }
            if ($("#NumeroCarnetConadis_Representante").val() == "") {
                mensaje += "número de carnet de Conadis, ";
            }
        }

        if ($("#SeFactura_Representante").prop('checked') == true) {
            if ($("#Idtipo_cliente_Representante").val() == "0") {
                mensaje += "tipo de cliente, ";
            }
            if ($("#IdCiudad_Representante_Fact").val() == "") {
                mensaje += "ciudad para facturacion, ";
            }
            if ($("#IdParroquia_Representante_Fact").val() == "") {
                mensaje += "parroquia para facturacion, ";
            }
        }
    }

    if ($("#Representante").val() == "P") {
        if ($("#Naturaleza_Padre").val() == "") {
            mensaje += "naturaleza, ";
        }
        if ($("#IdTipoDocumento_Padre").val() == "") {
            mensaje += "tipo de documento, ";
        }
        else {
            if ($("#IdTipoDocumento_Padre").val() == "RUC") {
                if ($("#RazonSocial_Padre").val() == "") {
                    mensaje += "razón social, ";
                }
            }
            else {
                if ($("#Nombres_Padre").val() == "") {
                    mensaje += "nombres, ";
                }
                if ($("#Apellidos_Padre").val() == "") {
                    mensaje += "apellidos, ";
                }
            }
        }
        if ($("#CedulaRuc_Padre").val() == "") {
            mensaje += "número de cédula, ";
        }

        if ($("#FechaNacimiento_Padre").val() == "" || $("#FechaNacimiento_Padre").val() == null) {
            mensaje += "fecha de nacimiento, ";
        }
        if ($("#IdPais_Padre").val() == "") {
            mensaje += "país, ";
        }
        if ($("#Cod_Region_Padre").val() == "") {
            mensaje += "región, ";
        }
        if ($("#IdProvincia_Padre").val() == "") {
            mensaje += "provincia, ";
        }
        if ($("#IdCiudad_Padre").val() == "") {
            mensaje += "ciudad, ";
        }
        if ($("#IdParroquia_Padre").val() == "") {
            mensaje += "parroquia, ";
        }
        if ($("#Sexo_Padre").val() == "") {
            mensaje += "sexo, ";
        }
        if ($("#IdEstadoCivil_Padre").val() == "") {
            mensaje += "estado civil, ";
        }
        if ($("#IdGrupoEtnico_Padre").val() == "0") {
            mensaje += "grupo étnico, ";
        }
        if ($("#Telefono_Padre").val() == "") {
            mensaje += "número de teléfono, ";
        }
        if ($("#Celular_Padre").val() == "") {
            mensaje += "número de celular, ";
        }
        if ($("#Correo_Padre").val() == "") {
            mensaje += "correo electrónico, ";
        }
        if ($("#IdReligion_Padre").val() == "") {
            mensaje += "religión, ";
        }
        if ($("#AsisteCentroCristiano_Padre").prop('checked') == null) {
            mensaje += "asiste o no a Centro Cristiano, ";
        }
        if ($("#CasaPropia_Padre").prop('checked') == null) {
            mensaje += "tiene casa propia, ";
        }
        if ($("#VehiculoPropio_Padre").prop('checked') == null) {
            mensaje += "tiene casa propia, ";
        }
        else {
            if ($("#VehiculoPropio_Padre").prop('checked') == true) {
                if ($("#Marca_Padre").val() == "") {
                    mensaje += "marca de vehículo, ";
                }
                if ($("#Modelo_Padre").val() == "") {
                    mensaje += "modelo de vehículo, ";
                }
            }
        }
        if ($("#Direccion_Padre").val() == "") {
            mensaje += "dirección, ";
        }
        if ($("#Sector_Padre").val() == "") {
            mensaje += "sector, ";
        }
        if ($("#EmpresaTrabajo_Padre").val() == "") {
            mensaje += "empresa donde trabaja, ";
        }
        if ($("#IdCatalogoFichaInst_Padre").val() == "0") {
            mensaje += "intrucción, ";
        }
        if ($("#IdProfesion_Padre").val() == "0") {
            mensaje += "profesión, ";
        }
        if ($("#CargoTrabajo_Padre").val() == "") {
            mensaje += "cargo, ";
        }
        if ($("#TelefonoTrabajo_Padre").val() == "") {
            mensaje += "teléfono trabajo, ";
        }
        if ($("#AniosServicio_Padre").val() == "") {
            mensaje += "años de servicio, ";
        }
        if ($("#IngresoMensual_Padre").val() == "") {
            mensaje += "ingreso mensual, ";
        }
        if ($("#DireccionTrabajo_Padre").val() == "") {
            mensaje += "dirección de trabajo, ";
        }
        if ($("#CodCatalogoCONADIS_Padre").val() != "") {
            if ($("#PorcentajeDiscapacidad_Padre").val() == "") {
                mensaje += "porcentaje de discapacidad, ";
            }
            if ($("#NumeroCarnetConadis_Padre").val() == "") {
                mensaje += "número de carnet de Conadis, ";
            }
        }

        if ($("#SeFactura_Padre").prop('checked') == true) {
            if ($("#Idtipo_cliente_Padre").val() == "0") {
                mensaje += "tipo de cliente, ";
            }
            if ($("#IdCiudad_Padre_Fact").val() == "") {
                mensaje += "ciudad para facturacion, ";
            }
            if ($("#IdParroquia_Padre_Fact").val() == "") {
                mensaje += "parroquia para facturacion, ";
            }
        }
    }

    if ($("#Representante").val() == "M") {
        if ($("#Naturaleza_Madre").val() == "") {
            mensaje += "naturaleza, ";
        }
        if ($("#IdTipoDocumento_Madre").val() == "") {
            mensaje += "tipo de documento, ";
        }
        else {
            if ($("#IdTipoDocumento_Madre").val() == "RUC") {
                if ($("#RazonSocial_Madre").val() == "") {
                    mensaje += "razón social, ";
                }
            }
            else {
                if ($("#Nombres_Madre").val() == "") {
                    mensaje += "nombres, ";
                }
                if ($("#Apellidos_Madre").val() == "") {
                    mensaje += "apellidos, ";
                }
            }
        }
        if ($("#CedulaRuc_Madre").val() == "") {
            mensaje += "número de cédula, ";
        }

        if ($("#FechaNacimiento_Madre").val() == "" || $("#FechaNacimiento_Madre").val() == null) {
            mensaje += "fecha de nacimiento, ";
        }
        if ($("#IdPais_Madre").val() == "") {
            mensaje += "país, ";
        }
        if ($("#Cod_Region_Madre").val() == "") {
            mensaje += "región, ";
        }
        if ($("#IdProvincia_Madre").val() == "") {
            mensaje += "provincia, ";
        }
        if ($("#IdCiudad_Madre").val() == "") {
            mensaje += "ciudad, ";
        }
        if ($("#IdParroquia_Madre").val() == "") {
            mensaje += "parroquia, ";
        }
        if ($("#Sexo_Madre").val() == "") {
            mensaje += "sexo, ";
        }
        if ($("#IdEstadoCivil_Madre").val() == "") {
            mensaje += "estado civil, ";
        }
        if ($("#IdGrupoEtnico_Madre").val() == "0") {
            mensaje += "grupo étnico, ";
        }
        if ($("#Telefono_Madre").val() == "") {
            mensaje += "número de teléfono, ";
        }
        if ($("#Celular_Madre").val() == "") {
            mensaje += "número de celular, ";
        }
        if ($("#Correo_Madre").val() == "") {
            mensaje += "correo electrónico, ";
        }
        if ($("#IdReligion_Madre").val() == "") {
            mensaje += "religión, ";
        }
        if ($("#AsisteCentroCristiano_Madre").prop('checked') == null) {
            mensaje += "asiste o no a Centro Cristiano, ";
        }
        if ($("#CasaPropia_Madre").prop('checked') == null) {
            mensaje += "tiene casa propia, ";
        }
        if ($("#VehiculoPropio_Madre").prop('checked') == null) {
            mensaje += "tiene casa propia, ";
        }
        else {
            if ($("#VehiculoPropio_Madre").prop('checked') == true) {
                if ($("#Marca_Madre").val() == "") {
                    mensaje += "marca de vehículo, ";
                }
                if ($("#Modelo_Madre").val() == "") {
                    mensaje += "modelo de vehículo, ";
                }
            }
        }
        if ($("#Direccion_Madre").val() == "") {
            mensaje += "dirección, ";
        }
        if ($("#Sector_Madre").val() == "") {
            mensaje += "sector, ";
        }
        if ($("#EmpresaTrabajo_Madre").val() == "") {
            mensaje += "empresa donde trabaja, ";
        }
        if ($("#IdCatalogoFichaInst_Madre").val() == "0") {
            mensaje += "intrucción, ";
        }
        if ($("#IdProfesion_Madre").val() == "0") {
            mensaje += "profesión, ";
        }
        if ($("#CargoTrabajo_Madre").val() == "") {
            mensaje += "cargo, ";
        }
        if ($("#TelefonoTrabajo_Madre").val() == "") {
            mensaje += "teléfono trabajo, ";
        }
        if ($("#AniosServicio_Madre").val() == "") {
            mensaje += "años de servicio, ";
        }
        if ($("#IngresoMensual_Madre").val() == "") {
            mensaje += "ingreso mensual, ";
        }
        if ($("#DireccionTrabajo_Madre").val() == "") {
            mensaje += "dirección de trabajo, ";
        }
        if ($("#CodCatalogoCONADIS_Madre").val() != "") {
            if ($("#PorcentajeDiscapacidad_Madre").val() == "") {
                mensaje += "porcentaje de discapacidad, ";
            }
            if ($("#NumeroCarnetConadis_Madre").val() == "") {
                mensaje += "número de carnet de Conadis, ";
            }
        }

        if ($("#SeFactura_Madre").prop('checked') == true) {
            if ($("#Idtipo_cliente_Madre").val() == "0") {
                mensaje += "tipo de cliente, ";
            }
            if ($("#IdCiudad_Madre_Fact").val() == "") {
                mensaje += "ciudad para facturacion, ";
            }
            if ($("#IdParroquia_Madre_Fact").val() == "") {
                mensaje += "parroquia para facturacion, ";
            }
        }
    }

    if ($("#SeFactura_Padre").prop('checked') == false && $("#SeFactura_Madre").prop('checked') == false && $("#SeFactura_Representante").prop('checked') == false) {
        mensaje += "representante económico (persona que se factura), ";
    }


    IrAlInicio();
    if (mensaje == "") {
        $("#MensajeError").html("");
        $("#DivError").hide();

        $("#DatosRepresentante").hide();
        $("#BtnRepresentante").attr("class", "w-10 h-10 rounded-full button text-gray-600 bg-gray-200 dark:bg-dark-1");
        $("#DatosSocioEconomico").show();
        $("#BtnSocioEconomico").attr("class", "w-10 h-10 rounded-full button text-white bg-theme-1");
    }
    else {
        $("#MensajeError").html("Ingresar o seleccionar: " + mensaje);
        $("#DivError").show();
    }
}
function ValidarDatosSocioEconomico() {
    var mensaje = "";
    if ($("#IdCatalogoFichaVive_Aspirante").val() == "0") {
        mensaje += "aspirante vive con, ";
    }
    if ($("#IdCatalogoFichaViv_Aspirante").val() == "0") {
        mensaje += "tenencia de vivienda, ";
    }
    if ($("#IdCatalogoFichaTipoViv_Aspirante").val() == "0") {
        mensaje += "tipo de vivienda, ";
    }
    if ($("#IdCatalogoFichaAgua_Aspirante").val() == "0") {
        mensaje += "tipo de agua, ";
    }
    if ($("#TieneElectricidad_Aspirante").prop('checked') == null) {
        mensaje += "tiene energía eléctrica, ";
    }
    if ($("#TieneHermanos_Aspirante").prop('checked') == null) {
        mensaje += "tiene hermanos, ";
    }
    else {
        if ($("#TieneHermanos_Aspirante").prop('checked') == true) {
            if ($("#CantidadHermanos").val() == "") {
                mensaje += "cuántos hermanos, ";
            }
        }
    }
    if ($("#SueldoPadre").val() == "") {
        mensaje += "sueldo del padre, ";
    }
    if ($("#OtroIngresoPadre").val() == "") {
        mensaje += "otros ingresos del padre, ";
    }
    if ($("#SueldoMadre").val() == "") {
        mensaje += "sueldo de la madre, ";
    }
    if ($("#OtroIngresoMadre").val() == "") {
        mensaje += "otros ingresos de la madre, ";
    }
    if ($("#GastoAlimentacion").val() == "") {
        mensaje += "gastos de alimentación, ";
    }
    if ($("#GastoEducacion").val() == "") {
        mensaje += "gastos de educación, ";
    }
    if ($("#GastoServicioBasico").val() == "") {
        mensaje += "gastos de servicios básicos, ";
    }
    if ($("#GastoSalud").val() == "") {
        mensaje += "gastos de salud, ";
    }
    if ($("#GastoArriendo").val() == "") {
        mensaje += "gastos de arriendo, ";
    }
    if ($("#GastoPrestamo").val() == "") {
        mensaje += "gastos de préstamos, ";
    }
    if ($("#OtroGasto").val() == "") {
        mensaje += "otros gastos, ";
    }
    if ($("#IdCatalogoFichaMotivo_Aspirante").val() == "0") {
        mensaje += "qué le impulsa a ingresar al Liceo Cristiano de Guayaquil, ";
    }
    else {
        if ($("#IdCatalogoFichaMotivo_Aspirante").val() == 15) {
            if ($("#OtroMotivoIngreso_Aspirante").val() == "") {
                mensaje += "otra razón que le impulsa a ingresar al Liceo Cristiano de Guayaquil, ";
            }
        }
    }
    if ($("#IdCatalogoFichaInst_Aspirante").val() == 0) {
        mensaje += "cómo se informo de nuestra institución, ";
    }
    else {
        if ($("#IdCatalogoFichaInst_Aspirante").val() == 19) {
            if ($("#OtroInformacionInst_Aspirante").val() == "") {
                mensaje += "otra razón cómo se informo de nuestra institución, ";
            }
        }
    }
    if ($("#IdCatalogoFichaFinanc_Aspirante").val() == 0) {
        mensaje += "cómo financiara los estudios, ";
    }
    else {
        if ($("#IdCatalogoFichaFinanc_Aspirante").val() == 22) {
            if ($("#OtroFinanciamiento_Aspirante").val() == "") {
                mensaje += "otra razón ómo financiara los estudios, ";
            }
        }
    }
    IrAlInicio();
    if (mensaje == "") {
        $("#MensajeError").html("");
        $("#DivError").hide();

        $("#DatosSocioEconomico").hide();
        $("#BtnSocioEconomico").attr("class", "w-10 h-10 rounded-full button text-gray-600 bg-gray-200 dark:bg-dark-1");
        $("#DatosArchivos").show();
        $("#BtnArchivos").attr("class", "w-10 h-10 rounded-full button text-white bg-theme-1");
    }
    else {
        if ($("#Representante").val() == "P") {
            $("#MensajeError").html("Ingresar o seleccionar los datos del padre: " + mensaje);
        }
        else if ($("#Representante").val() == "M") {
            ("#MensajeError").html("Ingresar o seleccionar los datos de la madre: " + mensaje);
        }
        else {
            $("#MensajeError").html("Ingresar o seleccionar: " + mensaje);
        }
        $("#DivError").show();
    }
}
function Validar_cedula_ruc_Padre() {
    var url_sistema = GetPathServer();
    var datos = {
        naturaleza: $("#Naturaleza_Padre").val(),
        tipo_documento: $("#IdTipoDocumento_Padre").val(),
        cedula_ruc: $("#CedulaRuc_Padre").val(),
    }

    $.ajax({
        type: 'POST',
        data: JSON.stringify(datos),
        url: url_sistema + '/Admision/Validar_cedula_ruc',
        async: false,
        bDeferRender: true,
        bProcessing: true,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data.isValid == true) {
                $("#error_documento_padre").hide();
                $("#Naturaleza_Padre").val(data.return_naturaleza);

                $("#PadreValido").val("1");
            }
            else {
                $("#error_documento_padre").show();
                $("#PadreValido").val("0");
            }
        },
        error: function (error) {
        }
    });
};

function Validar_cedula_ruc_Madre() {
    var url_sistema = GetPathServer();
    var datos = {
        naturaleza: $("#Naturaleza_Madre").val(),
        tipo_documento: $("#IdTipoDocumento_Madre").val(),
        cedula_ruc: $("#CedulaRuc_Madre").val(),
    }

    $.ajax({
        type: 'POST',
        data: JSON.stringify(datos),
        url: url_sistema + '/Admision/Validar_cedula_ruc',
        async: false,
        bDeferRender: true,
        bProcessing: true,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data.isValid == true) {
                $("#error_documento_madre").hide();
                $("#Naturaleza_Madre").val(data.return_naturaleza);
                $("#MadreValido").val("1");
            }
            else {
                $("#error_documento_madre").show();
                $("#PadreValido").val("0");
            }
        },
        error: function (error) {
        }
    });
};

function Validar_cedula_ruc_Representante() {
    var url_sistema = GetPathServer();
    var datos = {
        naturaleza: $("#Naturaleza_Representante").val(),
        tipo_documento: $("#IdTipoDocumento_Representante").val(),
        cedula_ruc: $("#CedulaRuc_Representante").val(),
    }

    $.ajax({
        type: 'POST',
        data: JSON.stringify(datos),
        url: url_sistema + '/Admision/Validar_cedula_ruc',
        async: false,
        bDeferRender: true,
        bProcessing: true,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data.isValid == true) {
                $("#error_documento_representante").hide();
                $("#Naturaleza_Representante").val(data.return_naturaleza);
                $("#RepresentanteValido").val("1");
            }
            else {
                $("#error_documento_representante").show();
                $("#RepresentanteValido").val("0");
            }
        },
        error: function (error) {
        }
    });
};

function CargarJornada() {
    var url_sistema = GetPathServer();
    var datos = {
        IdEmpresa: $("#IdEmpresa").val(),
        IdAnio: $("#IdAnio").val(),
        IdSede: $("#IdSede").val(),
    }

    $.ajax({
        type: 'POST',
        data: JSON.stringify(datos),
        url: url_sistema + '/Admision/CargarJornada',
        async: false,
        bDeferRender: true,
        bProcessing: true,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != "") {
                $("#IdJornada").empty();
                $.each(data, function (i, row) {
                    $("#IdJornada").append("<option value=" + row.IdJornada + ">" + row.NomJornada + "</option>");
                });
            }
            CargarNivel();
        },
        error: function (error) {
        }
    });
};

function CargarNivel() {
    var url_sistema = GetPathServer();
    var datos = {
        IdEmpresa: $("#IdEmpresa").val(),
        IdAnio: $("#IdAnio").val(),
        IdSede: $("#IdSede").val(),
        IdJornada: $("#IdJornada").val(),
    }

    $.ajax({
        type: 'POST',
        data: JSON.stringify(datos),
        url: url_sistema + '/Admision/CargarNivel',
        async: false,
        bDeferRender: true,
        bProcessing: true,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != "") {
                $("#IdNivel").empty();
                $.each(data, function (i, row) {
                    $("#IdNivel").append("<option value=" + row.IdNivel + ">" + row.NomNivel + "</option>");
                });
            }
            CargarCurso();
        },
        error: function (error) {
        }
    });
};

function CargarCurso() {
    var url_sistema = GetPathServer();
    var datos = {
        IdEmpresa: $("#IdEmpresa").val(),
        IdAnio: $("#IdAnio").val(),
        IdSede: $("#IdSede").val(),
        IdJornada: $("#IdJornada").val(),
        IdNivel: $("#IdNivel").val(),
    }

    $.ajax({
        type: 'POST',
        data: JSON.stringify(datos),
        url: url_sistema + '/Admision/CargarCurso',
        async: false,
        bDeferRender: true,
        bProcessing: true,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != "") {
                $("#IdCurso").empty();
                $.each(data, function (i, row) {
                    $("#IdCurso").append("<option value=" + row.IdCurso + ">" + row.NomCurso + "</option>");
                });
            }
        },
        error: function (error) {
        }
    });
};

function CargarRegion_Aspirante() {
    var url_sistema = GetPathServer();
    $("#Cod_Region_Aspirante").empty();
    var datos = {
        IdPais: $("#IdPais_Aspirante").val(),
    }
    $.ajax({
        type: 'POST',
        data: JSON.stringify(datos),
        url: url_sistema + '/Admision/CargarRegion',
        async: false,
        bDeferRender: true,
        bProcessing: true,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != "") {
                $.each(data, function (i, row) {
                    $("#Cod_Region_Aspirante").append("<option value=" + row.Cod_Region + ">" + row.Nom_region + "</option>");
                });
            }
        },
        error: function (error) {
        }
    });
};

function CargarProvincia_Aspirante() {
    var url_sistema = GetPathServer();
    $("#IdProvincia_Aspirante").empty();
    var datos = {
        IdPais: $("#IdPais_Aspirante").val(),
    }
    $.ajax({
        type: 'POST',
        data: JSON.stringify(datos),
        url: url_sistema + '/Admision/CargarProvincia',
        async: false,
        bDeferRender: true,
        bProcessing: true,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != "") {
                $.each(data, function (i, row) {
                    $("#IdProvincia_Aspirante").append("<option value=" + row.IdProvincia + ">" + row.Descripcion_Prov + "</option>");
                });
            }
            CargaCiudad_Aspirante();
        },
        error: function (error) {
        }
    });
};

function CargaCiudad_Aspirante() {
    var url_sistema = GetPathServer();
    $("#IdCiudad_Aspirante").empty();
    var datos = {
        IdProvincia: $("#IdProvincia_Aspirante").val(),
    }
    $.ajax({
        type: 'POST',
        data: JSON.stringify(datos),
        url: url_sistema + '/Admision/CargarCiudad',
        async: false,
        bDeferRender: true,
        bProcessing: true,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != "") {
                $.each(data, function (i, row) {
                    $("#IdCiudad_Aspirante").append("<option value=" + row.IdCiudad + ">" + row.Descripcion_Ciudad + "</option>");
                });
            }
            CargarParroquia_Aspirante()
        },
        error: function (error) {
        }
    });
};

function CargarParroquia_Aspirante() {
    var url_sistema = GetPathServer();
    $("#IdParroquia_Aspirante").empty();
    var datos = {
        IdCiudad: $("#IdCiudad_Aspirante").val(),
    }
    $.ajax({
        type: 'POST',
        data: JSON.stringify(datos),
        url: url_sistema + '/Admision/CargarParroquia',
        async: false,
        bDeferRender: true,
        bProcessing: true,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != "") {
                $.each(data, function (i, row) {
                    $("#IdParroquia_Aspirante").append("<option value=" + row.IdParroquia + ">" + row.nom_parroquia + "</option>");
                });
            }
        },
        error: function (error) {
        }
    });
};

function CargarRegion_Padre() {
    var url_sistema = GetPathServer();
    $("#Cod_Region_Padre").empty();
    var datos = {
        IdPais: $("#IdPais_Padre").val(),
    }
    $.ajax({
        type: 'POST',
        data: JSON.stringify(datos),
        url: url_sistema + '/Admision/CargarRegion',
        async: false,
        bDeferRender: true,
        bProcessing: true,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != "") {
                $.each(data, function (i, row) {
                    $("#Cod_Region_Padre").append("<option value=" + row.Cod_Region + ">" + row.Nom_region + "</option>");
                });
            }
        },
        error: function (error) {
        }
    });
};

function CargarProvincia_Padre() {
    var url_sistema = GetPathServer();
    $("#IdProvincia_Padre").empty();
    var datos = {
        IdPais: $("#IdPais_Padre").val()
    }
    $.ajax({
        type: 'POST',
        data: JSON.stringify(datos),
        url: url_sistema + '/Admision/CargarProvincia',
        async: false,
        bDeferRender: true,
        bProcessing: true,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != "") {
                $.each(data, function (i, row) {
                    $("#IdProvincia_Padre").append("<option value=" + row.IdProvincia + ">" + row.Descripcion_Prov + "</option>");
                });
            }
            CargaCiudad_Padre();
        },
        error: function (error) {
        }
    });
};

function CargaCiudad_Padre() {
    var url_sistema = GetPathServer();
    $("#IdCiudad_Padre").empty();
    var datos = {
        IdProvincia: $("#IdProvincia_Padre").val(),
    }
    $.ajax({
        type: 'POST',
        data: JSON.stringify(datos),
        url: url_sistema + '/Admision/CargarCiudad',
        async: false,
        bDeferRender: true,
        bProcessing: true,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != "") {
                $.each(data, function (i, row) {
                    $("#IdCiudad_Padre").append("<option value=" + row.IdCiudad + ">" + row.Descripcion_Ciudad + "</option>");
                });
            }
            CargarParroquia_Padre()
        },
        error: function (error) {
        }
    });
};

function CargarParroquia_Padre() {
    var url_sistema = GetPathServer();
    $("#IdParroquia_Padre").empty();
    var datos = {
        IdCiudad: $("#IdCiudad_Padre").val(),
    }
    $.ajax({
        type: 'POST',
        data: JSON.stringify(datos),
        url: url_sistema + '/Admision/CargarParroquia',
        async: false,
        bDeferRender: true,
        bProcessing: true,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != "") {
                $.each(data, function (i, row) {
                    $("#IdParroquia_Padre").append("<option value=" + row.IdParroquia + ">" + row.nom_parroquia + "</option>");
                });
            }
        },
        error: function (error) {
        }
    });
};

function CargarRegion_Madre() {
    var url_sistema = GetPathServer();
    $("#Cod_Region_Madre").empty();
    var datos = {
        IdPais: $("#IdPais_Madre").val(),
    }
    $.ajax({
        type: 'POST',
        data: JSON.stringify(datos),
        url: url_sistema + '/Admision/CargarRegion',
        async: false,
        bDeferRender: true,
        bProcessing: true,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != "") {
                $.each(data, function (i, row) {
                    $("#Cod_Region_Madre").append("<option value=" + row.Cod_Region + ">" + row.Nom_region + "</option>");
                });
            }
        },
        error: function (error) {
        }
    });
};

function CargarProvincia_Madre() {
    var url_sistema = GetPathServer();
    $("#IdProvincia_Madre").empty();
    var datos = {
        IdPais: $("#IdPais_Madre").val()
    }
    $.ajax({
        type: 'POST',
        data: JSON.stringify(datos),
        url: url_sistema + '/Admision/CargarProvincia',
        async: false,
        bDeferRender: true,
        bProcessing: true,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != "") {
                $.each(data, function (i, row) {
                    $("#IdProvincia_Madre").append("<option value=" + row.IdProvincia + ">" + row.Descripcion_Prov + "</option>");
                });
            }
            CargaCiudad_Madre();
        },
        error: function (error) {
        }
    });
};

function CargaCiudad_Madre() {
    var url_sistema = GetPathServer();
    $("#IdCiudad_Madre").empty();
    var datos = {
        IdProvincia: $("#IdProvincia_Madre").val(),
    }
    $.ajax({
        type: 'POST',
        data: JSON.stringify(datos),
        url: url_sistema + '/Admision/CargarCiudad',
        async: false,
        bDeferRender: true,
        bProcessing: true,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != "") {
                $.each(data, function (i, row) {
                    $("#IdCiudad_Madre").append("<option value=" + row.IdCiudad + ">" + row.Descripcion_Ciudad + "</option>");
                });
            }
            CargarParroquia_Madre()
        },
        error: function (error) {
        }
    });
};

function CargarParroquia_Madre() {
    var url_sistema = GetPathServer();
    $("#IdParroquia_Madre").empty();
    var datos = {
        IdCiudad: $("#IdCiudad_Madre").val(),
    }
    $.ajax({
        type: 'POST',
        data: JSON.stringify(datos),
        url: url_sistema + '/Admision/CargarParroquia',
        async: false,
        bDeferRender: true,
        bProcessing: true,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != "") {
                $.each(data, function (i, row) {
                    $("#IdParroquia_Madre").append("<option value=" + row.IdParroquia + ">" + row.nom_parroquia + "</option>");
                });
            }
        },
        error: function (error) {
        }
    });
};

function CargarRegion_Representante() {
    var url_sistema = GetPathServer();
    $("#Cod_Region_Representante").empty();
    var datos = {
        IdPais: $("#IdPais_Representante").val(),
    }
    $.ajax({
        type: 'POST',
        data: JSON.stringify(datos),
        url: url_sistema + '/Admision/CargarRegion',
        async: false,
        bDeferRender: true,
        bProcessing: true,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != "") {
                $.each(data, function (i, row) {
                    $("#Cod_Region_Representante").append("<option value=" + row.Cod_Region + ">" + row.Nom_region + "</option>");
                });
            }
        },
        error: function (error) {
        }
    });
};

function CargarProvincia_Representante() {
    var url_sistema = GetPathServer();
    $("#IdProvincia_Representante").empty();
    var datos = {
        IdPais: $("#IdPais_Representante").val()
    }
    $.ajax({
        type: 'POST',
        data: JSON.stringify(datos),
        url: url_sistema + '/Admision/CargarProvincia',
        async: false,
        bDeferRender: true,
        bProcessing: true,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != "") {
                $.each(data, function (i, row) {
                    $("#IdProvincia_Representante").append("<option value=" + row.IdProvincia + ">" + row.Descripcion_Prov + "</option>");
                });
            }
            CargaCiudad_Representante();
        },
        error: function (error) {
        }
    });
};

function CargaCiudad_Representante() {
    var url_sistema = GetPathServer();
    $("#IdCiudad_Representante").empty();
    var datos = {
        IdProvincia: $("#IdProvincia_Representante").val(),
    }
    $.ajax({
        type: 'POST',
        data: JSON.stringify(datos),
        url: url_sistema + '/Admision/CargarCiudad',
        async: false,
        bDeferRender: true,
        bProcessing: true,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != "") {
                $.each(data, function (i, row) {
                    $("#IdCiudad_Representante").append("<option value=" + row.IdCiudad + ">" + row.Descripcion_Ciudad + "</option>");
                });
            }
            CargarParroquia_Representante()
        },
        error: function (error) {
        }
    });
};

function CargarParroquia_Representante() {
    var url_sistema = GetPathServer();
    $("#IdParroquia_Representante").empty();
    var datos = {
        IdCiudad: $("#IdCiudad_Representante").val(),
    }
    $.ajax({
        type: 'POST',
        data: JSON.stringify(datos),
        url: url_sistema + '/Admision/CargarParroquia',
        async: false,
        bDeferRender: true,
        bProcessing: true,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != "") {
                $.each(data, function (i, row) {
                    $("#IdParroquia_Representante").append("<option value=" + row.IdParroquia + ">" + row.nom_parroquia + "</option>");
                });
            }
        },
        error: function (error) {
        }
    });
};

function PadreRepresentante() {
    var Nombre = $("#Nombres_Padre").val();
    var Apellido = $("#Apellidos_Padre").val();
    $("#RepresentanteParensco").html("Padre: " + Nombre + " " + Apellido);
    $("#DivDatosRepresentante").hide();
    $("#Representante").val("P");
    $("#RepresentanteValido").val("1");
}

function MadreRepresentante() {
    var Nombre = $("#Nombres_Madre").val();
    var Apellido = $("#Apellidos_Madre").val();
    $("#RepresentanteParensco").html("Madre: " + Nombre + " " + Apellido);
    $("#DivDatosRepresentante").hide();
    $("#Representante").val("M");
    $("#RepresentanteValido").val("1");
}

function OtroRepresentante() {
    $("#RepresentanteParensco").html("Otra Persona");
    $("#DivDatosRepresentante").show();
    $("#Representante").val("O");
}

function mostrar_otro_motivo() {
    var MotivoIngreso = $("#IdCatalogoFichaMotivo_Aspirante").val();
    if (MotivoIngreso == 15) {
        $("#otro_motivo_ingreso").show();
    }
    else {
        $("#otro_motivo_ingreso").hide();
    }
};

function mostrar_otro_informacion_inst() {
    var MotivoIngreso = $("#IdCatalogoFichaInst_Aspirante").val();
    if (MotivoIngreso == 19) {
        $("#otro_informacion_inst").show();
    }
    else {
        $("#otro_informacion_inst").hide();
    }
};

function mostrar_otro_financiamiento() {
    var Informacion = $("#IdCatalogoFichaFinanc_Aspirante").val();
    if (Informacion == 22) {
        $("#otro_forma_financ").show();
    }
    else {
        $("#otro_forma_financ").hide();
    }
};

function mostrar_CantidadHermanos() {
    if ($("#TieneHermanos_Aspirante").prop('checked') == true) {
        $("#cant_hermanos").show();
    }
    else {
        $("#cant_hermanos").hide();
    }
}

function mostrar_VehiculoPadre() {
    if ($("#VehiculoPropio_Padre").prop('checked') == true) {
        $("#MarcaVehiculoPadre").show();
        $("#ModeloVehiculoPadre").show();
        $("#AnioVehiculoPadre").show();
    }
    else {
        $("#MarcaVehiculoPadre").hide();
        $("#ModeloVehiculoPadre").hide();
        $("#AnioVehiculoPadre").hide();
    }
}

function mostrar_VehiculoMadre() {
    if ($("#VehiculoPropio_Madre").prop('checked') == true) {
        $("#MarcaVehiculoMadre").show();
        $("#ModeloVehiculoMadre").show();
        $("#AnioVehiculoMadre").show();
    }
    else {
        $("#MarcaVehiculoMadre").hide();
        $("#ModeloVehiculoMadre").hide();
        $("#AnioVehiculoMadre").hide();
    }
}

function mostrar_VehiculoRepresentante() {
    if ($("#VehiculoPropio_Representante").prop('checked') == true) {
        $("#MarcaVehiculoRepresentante").show();
        $("#ModeloVehiculoRepresentante").show();
        $("#AnioVehiculoRepresentante").show();
    }
    else {
        $("#MarcaVehiculoRepresentante").hide();
        $("#ModeloVehiculoRepresentante").hide();
        $("#AnioVehiculoRepresentante").hide();
    }
}

function MostrarFacturacion_Padre() {
    if ($("#SeFactura_Padre").prop('checked') == true) {
        $("#SeFactura_Madre").prop('checked', false);
        $("#SeFactura_Representante").prop('checked', false);
        MostrarFacturacion_Madre();
        MostrarFacturacion_Representante();

        $("#DatosFacturacionPadre").show();
    }
    else {
        $("#DatosFacturacionPadre").hide();
    }
}

function MostrarFacturacion_Madre() {
    if ($("#SeFactura_Madre").prop('checked') == true) {
        $("#SeFactura_Padre").prop('checked', false);
        $("#SeFactura_Representante").prop('checked', false);
        MostrarFacturacion_Padre();
        MostrarFacturacion_Representante();

        $("#DatosFacturacionMadre").show();
    }
    else {
        $("#DatosFacturacionMadre").hide();
    }
}

function MostrarFacturacion_Representante() {
    if ($("#SeFactura_Representante").prop('checked') == true) {
        $("#SeFactura_Madre").prop('checked', false);
        $("#SeFactura_Padre").prop('checked', false);
        MostrarFacturacion_Madre();
        MostrarFacturacion_Padre();

        $("#DatosFacturacionRepresentante").show();
    }
    else {
        $("#DatosFacturacionRepresentante").hide();
    }
}

function sumar_ingresos() {
    var TotalIng = parseFloat($("#SueldoPadre").val()) + parseFloat($("#SueldoMadre").val()) + parseFloat($("#OtroIngresoPadre").val()) + parseFloat($("#OtroIngresoMadre").val());
    $("#TotalIngresos").val(TotalIng);
    saldo();
};

function sumar_egresos() {
    var TotalGasto = parseFloat($("#GastoAlimentacion").val()) + parseFloat($("#GastoEducacion").val()) + parseFloat($("#GastoServicioBasico").val()) + parseFloat($("#GastoSalud").val()) + parseFloat($("#GastoArriendo").val()) + parseFloat($("#GastoPrestamo").val()) + parseFloat($("#OtroGasto").val());
    $("#TotalEgresos").val(TotalGasto);
    saldo();
};

function saldo() {
    var Total = parseFloat($("#TotalIngresos").val()) - parseFloat($("#TotalEgresos").val());
    $("#Saldo").val(Total);
};

function get_info_x_num_cedula_aspirante() {
    var cedula = $("#CedulaRuc_Aspirante").val();
    var tipo_doc = $("#IdTipoDocumento_Aspirante").val();

    if (cedula == null || cedula == "") {
        vaciar_campos_aspirante();
        return;
    }

    if (tipo_doc == "RUC") {
        if (cedula.length != 13) {
            //alert("El documento de tipo RUC, debe tener una longitud de 13 caracteres");
            $("#MensajeError").html("El documento de tipo RUC, debe tener una longitud de 13 caracteres");
            $("#DivError").show();
            $("#CedulaRuc_Aspirante").val("");
            vaciar_campos_aspirante();
            return;
        }
    } else
        if (tipo_doc == "CED") {
            if (cedula.length != 10) {
                //alert("El documento de tipo cédula, debe tener una longitud de 10 caracteres");
                $("#MensajeError").html("El documento de tipo cédula, debe tener una longitud de 10 caracteres");
                $("#DivError").show();
                $("#CedulaRuc_Aspirante").val("");
                vaciar_campos_aspirante();
                return;
            }
        }
    var url_sistema = GetPathServer();
    var datos = {
        IdEmpresa: $("#IdEmpresa").val(),
        IdAnio: IdAnio.GetValue(),
        pe_cedulaRuc: cedula
    }
    $.ajax({
        type: 'POST',
        data: JSON.stringify(datos),
        url: url_sistema + '/Admision/get_info_x_num_cedula',
        async: false,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data == "") {
                return;
            }
            if (data.IdAlumno != 0) {
                $("#IdAlumno").val(data.IdAlumno);
            }
            if (data.IdAdmision != 0) {
                $("#MensajeError").html("El aspirante ya fue registrado");
                $("#DivError").show();
                $("#AspiranteValido").val("0");

                IrAlInicio();
                //alert("El aspirante ya fue registrado");
                //window.location.href = '/Admision/Index'
            }
            else {
                if (data.IdPersona != 0) {
                    $("#Naturaleza_Aspirante").val(data.pe_Naturaleza);
                    $("#IdTipoDocumento_Aspirante").val(data.IdTipoDocumento);
                    $("#Nombres_Aspirante").val(data.pe_nombre);
                    $("#Apellidos_Aspirante").val(data.pe_apellido);
                    $("#Sexo_Aspirante").val(data.pe_sexo);
                    //pe_fechaNacimiento.SetDate(new Date(data.anio, data.mes, data.dia));
                    $("#Direccion_Aspirante").val(data.Direccion);
                    $("#Telefono_Aspirante").val(data.pe_telfono_Contacto);
                    $("#Celular_Aspirante").val(data.Celular);
                    $("#Correo_Aspirante").val(data.Correo);
                    var FechaNacimiento = new Date(data.anio, data.mes, data.dia);
                    var Fecha = FechaNacimiento.getDate() + '/' + (FechaNacimiento.getMonth() + 1) + '/' + FechaNacimiento.getFullYear();
                    $("#FechaNacimiento_Aspirante").val(Fecha);
                    $("#CodCatalogoSangre_Aspirante").val(data.CodCatalogoSangre);
                    $("#CodCatalogoCONADIS_Aspirante").val(data.CodCatalogoCONADIS);
                    $("#PorcentajeDiscapacidad_Aspirante").val(data.PorcentajeDiscapacidad);
                    $("#NumeroCarnetConadis_Aspirante").val(data.NumeroCarnetConadis);
                    $("#IdGrupoEtnico_Aspirante").val(data.IdGrupoEtnico);
                    $("#IdReligion_Aspirante").val(data.IdReligion);
                    $("#AsisteCentroCristiano_Aspirante").prop("checked", data.AsisteCentroCristiano);

                    $("#LugarNacimiento_Aspirante").val(data.LugarNacimiento);
                    $("#IdPais_Aspirante").val(data.IdPais);
                    CargarRegion_Aspirante();
                    $("#Cod_Region_Aspirante").val(data.Cod_Region);
                    CargarProvincia_Aspirante();
                    $("#IdProvincia_Aspirante").val(data.IdProvincia);
                    CargaCiudad_Aspirante();
                    $("#IdCiudad_Aspirante").val(data.IdCiudad);
                    CargarParroquia_Aspirante();
                    $("#IdParroquia_Aspirante").val(data.IdParroquia);
                    $("#Sector_Aspirante").val(data.Sector);
                    $("#CodCatalogoCONADIS_Aspirante").val(data.CodCatalogoCONADIS);
                    $("#PorcentajeDiscapacidad_Aspirante").val(data.PorcentajeDiscapacidad);
                    $("#NumeroCarnetConadis_Aspirante").val(data.NumeroCarnetConadis);

                    $("#MensajeError").html("");
                    $("#DivError").hide();
                    $("#AspiranteValido").val("1");
                }
                else {
                    $("#MensajeError").html("");
                    $("#DivError").hide();
                    $("#AspiranteValido").val("1");
                }
            }
        },
        error: function (error) {
            alert(error);
        }
    });
}

function get_info_x_num_cedula_padre() {
    var cedula = $("#CedulaRuc_Padre").val();
    var tipo_doc = $("#IdTipoDocumento_Padre").val();

    if (cedula == null || cedula == "") {
        vaciar_campos_padre();
        return;
    }

    if (tipo_doc == "RUC") {
        if (cedula.length != 13) {
            alert("El documento de tipo RUC, debe tener una longitud de 13 caracteres");
            $("#CedulaRuc_Padre").val("");
            vaciar_campos_padre();
            return;
        }
    } else
        if (tipo_doc == "CED") {
            if (cedula.length != 10) {
                alert("El documento de tipo cédula, debe tener una longitud de 10 caracteres");
                $("#CedulaRuc_Padre").val("");
                vaciar_campos_padre();
                return;
            }
        }

    var url_sistema = GetPathServer();
    var datos = {
        pe_cedulaRuc: cedula
    }
    $.ajax({
        type: 'POST',
        data: JSON.stringify(datos),
        url: url_sistema + '/Admision/get_info_x_num_cedula_persona',
        async: false,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data.IdPersona == 0) {
                return;
            }
            else {
                $("#Naturaleza_Padre").val(data.pe_Naturaleza);
                $("#IdTipoDocumento_Padre").val(data.IdTipoDocumento);
                $("#Nombres_Padre").val(data.pe_nombre);
                $("#Apellidos_Padre").val(data.pe_apellido);
                $("#RazonSocial_Padre").val(data.pe_razonSocial);
                $("#Sexo_Padre").val(data.pe_sexo);
                //pe_fechaNacimiento.SetDate(new Date(data.anio, data.mes, data.dia));
                $("#Direccion_Padre").val(data.pe_direccion);
                $("#Telefono_Padre").val(data.pe_telfono_Contacto);
                $("#Celular_Padre").val(data.pe_celular);
                $("#Correo_Padre").val(data.pe_correo);
                $("#IdEstadoCivil_Padre").val(data.IdEstadoCivil);
                var FechaNacimiento = new Date(data.anio, data.mes, data.dia);
                var Fecha = FechaNacimiento.getDate() + '/' + (FechaNacimiento.getMonth() + 1) + '/' + FechaNacimiento.getFullYear();
                $("#FechaNacimiento_Padre").val(Fecha);
                $("#CodCatalogoCONADIS_Padre").val(data.CodCatalogoCONADIS);
                $("#PorcentajeDiscapacidad_Padre").val(data.PorcentajeDiscapacidad);
                $("#NumeroCarnetConadis_Padre").val(data.NumeroCarnetConadis);
                $("#IdGrupoEtnico_Padre").val(data.IdGrupoEtnico);
                $("#IdReligion_Padre").val(data.IdReligion);
                $("#IdProfesion_Padre").val(data.IdProfesion);
                $("#AsisteCentroCristiano_Padre").prop("checked", data.AsisteCentroCristiano);

                $("#IdPais_Padre").val("");
                CargarRegion_Padre();
                $("#Cod_Region_Padre").val("");
                CargarProvincia_Padre();
                $("#IdProvincia_Padre").val("");
                CargaCiudad_Padre();
                $("#IdCiudad_Padre").val("");
                CargarParroquia_Padre();
                $("#IdParroquia_Padre").val("");
            }
        },
        error: function (error) {
            alert(error);
        }
    });
}

function get_info_x_num_cedula_madre() {
    var cedula = $("#CedulaRuc_Madre").val();
    var tipo_doc = $("#IdTipoDocumento_Madre").val();

    if (cedula == null || cedula == "") {
        vaciar_campos_madre();
        return;
    }

    if (tipo_doc == "RUC") {
        if (cedula.length != 13) {
            alert("El documento de tipo RUC, debe tener una longitud de 13 caracteres");
            $("#CedulaRuc_Madre").val("");
            vaciar_campos_madre();
            return;
        }
    } else
        if (tipo_doc == "CED") {
            if (cedula.length != 10) {
                alert("El documento de tipo cédula, debe tener una longitud de 10 caracteres");
                $("#CedulaRuc_Madre").val("");
                vaciar_campos_madre();
                return;
            }
        }

    var url_sistema = GetPathServer();
    var datos = {
        pe_cedulaRuc: cedula
    }
    $.ajax({
        type: 'POST',
        data: JSON.stringify(datos),
        url: url_sistema + '/Admision/get_info_x_num_cedula_persona',
        async: false,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data.IdPersona == 0) {
                return;
            }
            else {
                $("#Naturaleza_Madre").val(data.pe_Naturaleza);
                $("#IdTipoDocumento_Madre").val(data.IdTipoDocumento);
                $("#Nombres_Madre").val(data.pe_nombre);
                $("#Apellidos_Madre").val(data.pe_apellido);
                $("#RazonSocial_Madre").val(data.pe_razonSocial);
                $("#Sexo_Madre").val(data.pe_sexo);
                //pe_fechaNacimiento.SetDate(new Date(data.anio, data.mes, data.dia));
                $("#Direccion_Madre").val(data.pe_direccion);
                $("#Telefono_Madre").val(data.pe_telfono_Contacto);
                $("#Celular_Madre").val(data.pe_celular);
                $("#Correo_Madre").val(data.pe_correo);
                $("#IdEstadoCivil_Madre").val(data.IdEstadoCivil);
                //$("#FechaNacimiento_Madre").val(new Date(data.anio, data.mes, data.dia));
                var FechaNacimiento = new Date(data.anio, data.mes, data.dia);
                var Fecha = FechaNacimiento.getDate() + '/' + (FechaNacimiento.getMonth() + 1) + '/' + FechaNacimiento.getFullYear();
                $("#FechaNacimiento_Madre").val(Fecha);
                $("#CodCatalogoCONADIS_Madre").val(data.CodCatalogoCONADIS);
                $("#PorcentajeDiscapacidad_Madre").val(data.PorcentajeDiscapacidad);
                $("#NumeroCarnetConadis_Madre").val(data.NumeroCarnetConadis);
                $("#IdGrupoEtnico_Madre").val(data.IdGrupoEtnico);
                $("#IdReligion_Madre").val(data.IdReligion);
                $("#IdProfesion_Madre").val(data.IdProfesion);
                $("#AsisteCentroCristiano_Madre").prop("checked", data.AsisteCentroCristiano);

                $("#IdPais_Madre").val("");
                CargarRegion_Madre();
                $("#Cod_Region_Madre").val("");
                CargarProvincia_Madre();
                $("#IdProvincia_Madre").val("");
                CargaCiudad_Madre();
                $("#IdCiudad_Madre").val("");
                CargarParroquia_Madre();
                $("#IdParroquia_Madre").val("");
            }
        },
        error: function (error) {
            alert(error);
        }
    });
}

function get_info_x_num_cedula_representante() {
    var cedula = $("#CedulaRuc_Representante").val();
    var tipo_doc = $("#IdTipoDocumento_Representante").val();

    if (cedula == null || cedula == "") {
        vaciar_campos_representante();
        return;
    }

    if (tipo_doc == "RUC") {
        if (cedula.length != 13) {
            alert("El documento de tipo RUC, debe tener una longitud de 13 caracteres");
            $("#CedulaRuc_Representante").val("");
            vaciar_campos_representante();
            return;
        }
    } else
        if (tipo_doc == "CED") {
            if (cedula.length != 10) {
                alert("El documento de tipo cédula, debe tener una longitud de 10 caracteres");
                $("#CedulaRuc_Representante").val("");
                vaciar_campos_representante();
                return;
            }
        }

    var url_sistema = GetPathServer();
    var datos = {
        pe_cedulaRuc: cedula
    }
    $.ajax({
        type: 'POST',
        data: JSON.stringify(datos),
        url: url_sistema + '/Admision/get_info_x_num_cedula_persona',
        async: false,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data.IdPersona == 0) {
                return;
            }
            else {
                $("#Naturaleza_Representante").val(data.pe_Naturaleza);
                $("#IdTipoDocumento_Representante").val(data.IdTipoDocumento);
                $("#Nombres_Representante").val(data.pe_nombre);
                $("#Apellidos_Representante").val(data.pe_apellido);
                $("#RazonSocial_Representante").val(data.pe_razonSocial);
                $("#Sexo_Representante").val(data.pe_sexo);
                //pe_fechaNacimiento.SetDate(new Date(data.anio, data.mes, data.dia));
                $("#Direccion_Representante").val(data.pe_direccion);
                $("#Telefono_Representante").val(data.pe_telfono_Contacto);
                $("#Celular_Representante").val(data.pe_celular);
                $("#Correo_Representante").val(data.pe_correo);
                $("#IdEstadoCivil_Representante").val(data.IdEstadoCivil);
                //$("#FechaNacimiento_Representante").val(new Date(data.anio, data.mes, data.dia));
                var FechaNacimiento = new Date(data.anio, data.mes, data.dia);
                var Fecha = FechaNacimiento.getDate() + '/' + (FechaNacimiento.getMonth() + 1) + '/' + FechaNacimiento.getFullYear();
                $("#FechaNacimiento_Representante").val(Fecha);
                $("#CodCatalogoCONADIS_Representante").val(data.CodCatalogoCONADIS);
                $("#PorcentajeDiscapacidad_Representante").val(data.PorcentajeDiscapacidad);
                $("#NumeroCarnetConadis_Representante").val(data.NumeroCarnetConadis);
                $("#IdGrupoEtnico_Representante").val(data.IdGrupoEtnico);
                $("#IdReligion_Representante").val(data.IdReligion);
                $("#IdProfesion_Representante").val(data.IdProfesion);
                $("#AsisteCentroCristiano_Representante").prop("checked", data.AsisteCentroCristiano);

                $("#IdPais_Representante").val("");
                CargarRegion_Representante();
                $("#Cod_Region_Representante").val("");
                CargarProvincia_Representante();
                $("#IdProvincia_Representante").val("");
                CargaCiudad_Representante();
                $("#IdCiudad_Representante").val("");
                CargarParroquia_Representante();
                $("#IdParroquia_Representante").val("");
            }
        },
        error: function (error) {
            alert(error);
        }
    });
}

function vaciar_campos_aspirante() {
    $("#Naturaleza_Aspirante").val("NATU");
    $("#IdTipoDocumento_Aspirante").val("CED");
    $("#Nombres_Aspirante").val("");
    $("#Apellidos_Aspirante").val("");
    $("#Sexo_Aspirante").val("");
    $("#Direccion_Aspirante").val("");
    $("#Telefono_Aspirante").val("");
    $("#Celular_Aspirante").val("");
    $("#Correo_Aspirante").val("");
    $("#FechaNacimiento_Aspirante").val(new Date().getDate() + "/" + (new Date().getMonth() + 1) + "/" + new Date().getFullYear());
    $("#CodCatalogoSangre_Aspirante").val("");
    $("#CodCatalogoCONADIS_Aspirante").val("");
    $("#PorcentajeDiscapacidad_Aspirante").val("");
    $("#NumeroCarnetConadis_Aspirante").val("");
    $("#IdGrupoEtnico_Aspirante").val("0");
    $("#IdReligion_Aspirante").val("0");
    $("#AsisteCentroCristiano_Aspirante").prop("checked", false);

    $("#LugarNacimiento_Aspirante").val("");
    $("#IdPais_Aspirante").val("");
    $("#Cod_Region_Aspirante").val("");
    $("#IdProvincia_Aspirante").val("");
    $("#IdCiudad_Aspirante").val("");
    $("#IdParroquia_Aspirante").val("");
    $("#Sector_Aspirante").val("");
    $("#IdCiudad_Aspirante").val("");
}

function vaciar_campos_padre() {
    $("#Naturaleza_Padre").val("NATU");
    $("#IdTipoDocumento_Padre").val("CED");
    $("#CedulaRuc_Padre").val("");
    $("#EstaFallecido_Padre").prop('checked', false);
    $("#Nombres_Padre").val("");
    $("#Apellidos_Padre").val("");
    $("#RazonSocial_Padre").val("");
    $("#FechaNacimiento_Padre").val("");
    $("#IdPais_Padre").val("1");
    $("#Cod_Region_Padre").val("00001");
    $("#IdProvincia_Padre").val("");
    $("#IdCiudad_Padre").val("");
    $("#IdParroquia_Padre").val("");
    $("#Sexo_Padre").val("");
    $("#IdEstadoCivil_Padre").val("");
    $("#IdGrupoEtnico_Padre").val("");
    $("#Telefono_Padre").val("");
    $("#Celular_Padre").val("");
    $("#Correo_Padre").val("");
    $("#AsisteCentroCristiano_Padre").prop('checked', false);
    $("#CasaPropia_Padre").prop('checked', false);
    $("#VehiculoPropio_Padre").prop('checked', false);
    $("#Marca_Padre").val("");
    $("#Modelo_Padre").val("");
    $("#IdReligion_Padre").val("");
    $("#Direccion_Padre").val("");
    $("#Sector_Padre").val("");
    $("#EmpresaTrabajo_Padre").val("");
    $("#IdCatalogoFichaInst_Padre").val("");
    $("#IdProfesion_Padre").val("");
    $("#CargoTrabajo_Padre").val("");
    $("#TelefonoTrabajo_Padre").val("");
    $("#AniosServicio_Padre").val("");
    $("#IngresoMensual_Padre").val("");
    $("#DireccionTrabajo_Padre").val("");
    $("#CodCatalogoCONADIS_Padre").val("");
    $("#PorcentajeDiscapacidad_Padre").val("");
    $("#NumeroCarnetConadis_Padre").val("");
}

function vaciar_campos_madre() {
    $("#Naturaleza_Madre").val("NATU");
    $("#IdTipoDocumento_Madre").val("CED");
    $("#CedulaRuc_Madre").val("");
    $("#EstaFallecido_Madre").prop('checked', false);
    $("#Nombres_Madre").val("");
    $("#Apellidos_Madre").val("");
    $("#RazonSocial_Madre").val("");
    $("#FechaNacimiento_Madre").val("");
    $("#IdPais_Madre").val("1");
    $("#Cod_Region_Madre").val("00001");
    $("#IdProvincia_Madre").val("");
    $("#IdCiudad_Madre").val("");
    $("#IdParroquia_Madre").val("");
    $("#Sexo_Madre").val("");
    $("#IdEstadoCivil_Madre").val("");
    $("#IdGrupoEtnico_Madre").val("");
    $("#Telefono_Madre").val("");
    $("#Celular_Madre").val("");
    $("#Correo_Madre").val("");
    $("#AsisteCentroCristiano_Madre").prop('checked', false);
    $("#CasaPropia_Madre").prop('checked', false);
    $("#VehiculoPropio_Madre").prop('checked', false);
    $("#Marca_Madre").val("");
    $("#Modelo_Madre").val("");
    $("#IdReligion_Madre").val("");
    $("#Direccion_Madre").val("");
    $("#Sector_Madre").val("");
    $("#EmpresaTrabajo_Madre").val("");
    $("#IdCatalogoFichaInst_Madre").val("");
    $("#IdProfesion_Madre").val("");
    $("#CargoTrabajo_Madre").val("");
    $("#TelefonoTrabajo_Madre").val("");
    $("#AniosServicio_Madre").val("");
    $("#IngresoMensual_Madre").val("");
    $("#DireccionTrabajo_Madre").val("");
    $("#CodCatalogoCONADIS_Madre").val("");
    $("#PorcentajeDiscapacidad_Madre").val("");
    $("#NumeroCarnetConadis_Madre").val("");
}

function vaciar_campos_representante() {
    $("#Naturaleza_Representante").val("NATU");
    $("#IdTipoDocumento_Representante").val("CED");
    $("#CedulaRuc_Representante").val("");
    $("#EstaFallecido_Representante").prop('checked', false);
    $("#Nombres_Representante").val("");
    $("#Apellidos_Representante").val("");
    $("#RazonSocial_Representante").val("");
    $("#FechaNacimiento_Representante").val("");
    $("#IdPais_Representante").val("1");
    $("#Cod_Region_Representante").val("00001");
    $("#IdProvincia_Representante").val("");
    $("#IdCiudad_Representante").val("");
    $("#IdParroquia_Representante").val("");
    $("#Sexo_Representante").val("");
    $("#IdEstadoCivil_Representante").val("");
    $("#IdGrupoEtnico_Representante").val("");
    $("#Telefono_Representante").val("");
    $("#Celular_Representante").val("");
    $("#Correo_Representante").val("");
    $("#AsisteCentroCristiano_Representante").prop('checked', false);
    $("#CasaPropia_Representante").prop('checked', false);
    $("#VehiculoPropio_Representante").prop('checked', false);
    $("#Marca_Representante").val("");
    $("#Modelo_Representante").val("");
    $("#IdReligion_Representante").val("");
    $("#Direccion_Representante").val("");
    $("#Sector_Representante").val("");
    $("#EmpresaTrabajo_Representante").val("");
    $("#IdCatalogoFichaInst_Representante").val("");
    $("#IdProfesion_Representante").val("");
    $("#CargoTrabajo_Representante").val("");
    $("#TelefonoTrabajo_Representante").val("");
    $("#AniosServicio_Representante").val("");
    $("#IngresoMensual_Representante").val("");
    $("#DireccionTrabajo_Representante").val("");
    $("#CodCatalogoCONADIS_Representante").val("");
    $("#PorcentajeDiscapacidad_Representante").val("");
    $("#NumeroCarnetConadis_Representante").val("");
}

function ValidarTerminos() {
    var mensaje = "";
    $('#Guardar').attr("disabled", true);
    if ($("#AceptaTerminos").prop('checked') == null || $("#AceptaTerminos").prop('checked') == false) {
        mensaje += "Aceptar terminos y condiciones, ";
    }

    IrAlInicio();
    if (mensaje == "") {
        $("#MensajeError").html("");
        $("#DivError").hide();

        $("#FormAdmision").submit();
    }
    else {
        $("#MensajeError").html(mensaje);
        $("#DivError").show();

        $('#Guardar').attr("disabled", false);
    }
}

function ValidarArchivos() {
    var mensaje = "";

    if ($("#IdAlumno").val() == null || $("#IdAlumno").val() == 0) {
        if ($('#FotoAspirante').val() == "") {
            mensaje += "seleccione archivo para foto del aspirante, ";
        }
        else {
            if ($("#FotoAspirante")[0].files[0].size > 0 && $("#FotoAspirante")[0].files[0].size > 4000000) {
                mensaje += "el archivo foto del aspirante ha superado el peso máximo, ";
            }
        }

        if ($('#CedulaAspirante').val() == "") {
            mensaje += "seleccione archivo para cédula del aspirante, ";
        }
        else {
            if ($("#CedulaAspirante")[0].files[0].size > 0 && $("#CedulaAspirante")[0].files[0].size > 4000000) {
                mensaje += "el archivo cédula del aspirante ha superado el peso máximo, ";
            }
        }

        if ($('#CedulaRepresentante').val() == "") {
            mensaje += "seleccione archivo para cédula del representante, ";
        }
        else {
            if ($("#CedulaRepresentante")[0].files[0].size > 0 && $("#CedulaRepresentante")[0].files[0].size > 4000000) {
                mensaje += "el archivo cédula del representante ha superado el peso máximo, ";
            }
        }

        if ($('#RecordAcademicoAspirante').val() == "") {
            mensaje += "seleccione archivo para record académico del aspirante, ";
        }
        else {
            if ($("#RecordAcademicoAspirante")[0].files[0].size > 0 && $("#RecordAcademicoAspirante")[0].files[0].size > 4000000) {
                mensaje += "el archivo record académico del aspirante ha superado el peso máximo, ";
            }
        }

        if ($('#PagoAlDiaAspirante').val() == "") {
            mensaje += "seleccione archivo para pago al día del aspirante, ";
        }
        else {
            if ($("#PagoAlDiaAspirante")[0].files[0].size > 0 && $("#PagoAlDiaAspirante")[0].files[0].size > 4000000) {
                mensaje += "el archivo pago al día del aspirante ha superado el peso máximo, ";
            }
        }

        if ($('#CertificadoLaboral').val() == "") {
            mensaje += "seleccione archivo para certificado laboral, ";
        }
        else {
            if ($("#CertificadoLaboral")[0].files[0].size > 0 && $("#CertificadoLaboral")[0].files[0].size > 4000000) {
                mensaje += "el archivo certificado laboral ha superado el peso máximo, ";
            }
        }

        IrAlInicio();
        if (mensaje == "") {
            $("#MensajeError").html("");
            $("#DivError").hide();

            $("#DatosArchivos").hide();
            $("#BtnArchivos").attr("class", "w-10 h-10 rounded-full button text-gray-600 bg-gray-200 dark:bg-dark-1");
            $("#DatosTerminos").show();
            $("#BtnTerminos").attr("class", "w-10 h-10 rounded-full button text-white bg-theme-1");
        }
        else {
            $("#MensajeError").html(mensaje);
            $("#DivError").show();
        }
    }
    else {
        IrAlInicio();
        $("#MensajeError").html("");
        $("#DivError").hide();

        $("#DatosArchivos").hide();
        $("#BtnArchivos").attr("class", "w-10 h-10 rounded-full button text-gray-600 bg-gray-200 dark:bg-dark-1");
        $("#DatosTerminos").show();
        $("#BtnTerminos").attr("class", "w-10 h-10 rounded-full button text-white bg-theme-1");
    }

};

function CargarParroquia_Facturacion_Padre() {
    var url_sistema = GetPathServer();
    $("#IdParroquia_Padre_Fact").empty();
    var datos = {
        IdCiudad: $("#IdCiudad_Padre_Fact").val(),
    }
    $.ajax({
        type: 'POST',
        data: JSON.stringify(datos),
        url: url_sistema + '/Admision/CargarParroquia',
        async: false,
        bDeferRender: true,
        bProcessing: true,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != "") {
                $.each(data, function (i, row) {
                    $("#IdParroquia_Padre_Fact").append("<option value=" + row.IdParroquia + ">" + row.nom_parroquia + "</option>");
                });
            }
        },
        error: function (error) {
        }
    });
};

function CargarParroquia_Facturacion_Madre() {
    var url_sistema = GetPathServer();
    $("#IdParroquia_Madre_Fact").empty();
    var datos = {
        IdCiudad: $("#IdCiudad_Madre_Fact").val(),
    }
    $.ajax({
        type: 'POST',
        data: JSON.stringify(datos),
        url: url_sistema + '/Admision/CargarParroquia',
        async: false,
        bDeferRender: true,
        bProcessing: true,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != "") {
                $.each(data, function (i, row) {
                    $("#IdParroquia_Madre_Fact").append("<option value=" + row.IdParroquia + ">" + row.nom_parroquia + "</option>");
                });
            }
        },
        error: function (error) {
        }
    });
};

function CargarParroquia_Facturacion_Representante() {
    var url_sistema = GetPathServer();
    $("#IdParroquia_Representante_Fact").empty();
    var datos = {
        IdCiudad: $("#IdCiudad_Representante_Fact").val(),
    }
    $.ajax({
        type: 'POST',
        data: JSON.stringify(datos),
        url: url_sistema + '/Admision/CargarParroquia',
        async: false,
        bDeferRender: true,
        bProcessing: true,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data != "") {
                $.each(data, function (i, row) {
                    $("#IdParroquia_Representante_Fact").append("<option value=" + row.IdParroquia + ">" + row.nom_parroquia + "</option>");
                });
            }
        },
        error: function (error) {
        }
    });
};
