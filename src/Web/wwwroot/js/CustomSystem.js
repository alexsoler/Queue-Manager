function EditSystemCustom(event) {
    event.preventDefault();

    $.ajax({
        type: 'POST',
        url: '/CustomSystem/SaveSystemCustom',
        data: $('#FormEditSystem').serialize()
    }).done(function () {
        loadAlert("Se guardaron los cambios con exito", "Exito", "alert-success");
    }).fail(function () {
        loadAlert("No se pudo guardar los cambios", "Error", "alert-danger");
    });
}

function loadAlert(mensaje, tipoMensaje, nameClass) {
    $("body").prepend(`<div class="alert ${nameClass} alert-dismissible fade show" role="alert">
        <strong>${tipoMensaje}!</strong > ${mensaje}<button class="close" type = "button" data-dismiss="alert" aria-label="Close" >
        <span aria-hidden="true">×</span></button></div>`);

    setTimeout(function () {
        $(".alert").alert('close');
    }, 2000);
}