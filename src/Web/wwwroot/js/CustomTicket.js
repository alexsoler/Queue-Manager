$(function () {
    RefreshViewPreviewTicket();
});

function EditTicketCustom(event) {
    event.preventDefault();

    $.ajax({
        type: 'POST',
        url: '/CustomTicket/SaveTicketCustom',
        data: $("#FormEditTicket").serialize()
    }).done(function () {
        loadAlert("Se guardaron los cambios con exito", "Exito", "alert-success");
        RefreshViewPreviewTicket();
    }).fail(function () {
        loadAlert("No se pudo guardar los cambios", "Error", "alert-danger");
    });
}

function RefreshViewPreviewTicket() {
    const custom = $('#FormEditTicket').serialize();
    document.getElementById("frameTicket").src = `/Touch/Ticket?displayTokenName=C012&NamePriority=Prioridad&NameTask=Tarea&${custom}`;
}

function loadAlert(mensaje, tipoMensaje, nameClass) {
    $("body").prepend(`<div class="alert ${nameClass} alert-dismissible fade show" role="alert">
        <strong>${tipoMensaje}!</strong > ${mensaje}<button class="close" type = "button" data-dismiss="alert" aria-label="Close" >
        <span aria-hidden="true">×</span></button></div>`);

    setTimeout(function () {
        $(".alert").alert('close');
    }, 2000);
}