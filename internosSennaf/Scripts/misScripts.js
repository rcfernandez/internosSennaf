// LIMPIAR INPUT
function limpiarFiltro() {
    $('#filtrar').val("")
}


// SCRIPT PARA QUE AL INICIO VAYA DIRECTO A LA ACCION DE MOSTRAR LISTADO DE SEDE CENTRAL
window.onload = function () {
    $.ajax({
        type: "GET",
        url: '@Url.Content("~/Home/MostrarListadoInternos / 2")',
        beforeSend: function myfunction() {
            $("#Cargando").show();
        },
        success: function (resultado) {
            $("#activo").focus();
            $("#resultado").html(resultado);
            $("#Cargando").hide();
            //alert("funca");
        },
        error: function () {
            alert("Error al cargar el listado de internos");
            $("#Cargando").hide();
        }
        });
};





