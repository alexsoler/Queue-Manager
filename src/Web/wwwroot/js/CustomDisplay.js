function AddMediaToDisplay(id, e) {
    const $LisDisplay = $("#listDisplay li");
    let order = 100;
    if ($LisDisplay.length > 0) {
        order = $LisDisplay.last().data("order") + 100;
    }

    $.ajax({
        type: 'POST',
        url: '/CustomDisplay/AddMediaToDisplay',
        data: { id, order }
    }).done((result) => {
        loadListDisplayMedia();
        $(e.parentElement).fadeOut("fast", (element) => $(element).remove());
    }).fail((result) => {
        console.error(result);
    });
}

function RemoveMediaToDisplay(id, idMedia, e) {
    $.ajax({
        type: 'POST',
        url: '/CustomDisplay/RemoveMediaToDisplay',
        data: { id, idMedia }
    }).done(() => {
        loadListMedia();
        $(e.parentElement).fadeOut("fast", (element) => $(element).remove());
    }).fail((result) => {
        console.error(result);
    });
}

function loadListDisplayMedia() {
    $("#divLoadDisplay").load("/CustomDisplay/DisplayMediaPV", function () {
        ps1 = new PerfectScrollbar("#listDisplay");
        AddSortable();
    });
}

function loadListMedia() {
    $("#divLoadMedia").load("/CustomDisplay/MediaPV", function () {
        ps2 = new PerfectScrollbar("#listMedia");
    });
}

//Hace ordenable la lista de archivos multimedia a mostrar en la pantalla de visualizacion
function AddSortable() {
    $("#listDisplay").sortable({
        update: function (event, ui) {
            const id = ui.item.data("id");
            let prevOrder = ui.item.prev().data("order");
            let nextOrder = ui.item.next().data("order");
            let order = 0;

            if (prevOrder === undefined) {
                prevOrder = 0;
            }

            if (nextOrder === undefined) {
                order = Number(prevOrder) + 100;
            }
            else {
                order = (Number(prevOrder) + Number(nextOrder)) / 2;
            }

            $.ajax({
                type: 'POST',
                url: '/CustomDisplay/UpdateOrder',
                data: { id, order }
            }).done(() => {
                console.log('Nuevo orden aplicado');
                ui.item.data("order", order);
            }).fail((error) => {
                console.error('No se pudo aplicar un nuevo orden: ' + error);
            });
        }
    });
}

AddSortable();

function NuevoMensaje() {
    const $message = $("#txtAreaMessage");

    if ($message.val().length < 1) {
        return;
    }

    $.ajax({
        type: 'POST',
        url: '/CustomDisplay/AddNewMessage',
        data: { message: $message.val() }
    }).done(function (displayMessage) {
        loadAlert("Mensaje agregado con exito!", "Exito", "alert-success");
        $("#listMensajes").append(
            `<li class="list-group-item list-group-item-action" data-id="${displayMessage.id}">
                 <p contenteditable="true">${displayMessage.message}</p>
                 <button type="button" class="btn btn-success btn-sm" onclick="EditarMensaje(${displayMessage.id}, this)">Guardar</button>
                 <button type="button" class="btn btn-danger btn-sm" onclick="EliminarMensaje(${displayMessage.id}, this)">Eliminar</button>
             </li>`);
        $message.val("");
    }).fail(function () {
        loadAlert("No se pudo agregar el mensaje", "Error", "alert-danger");
    });
}

function EditarMensaje(id, e) {
    const mensaje = $(e).siblings("p").text();

    $.ajax({
        type: 'POST',
        url: '/CustomDisplay/EditMessage',
        data: { id: id, message: mensaje }
    }).done(function () {
        loadAlert("Se edito el mensaje con exito", "Exito", "alert-success");
    }).fail(function () {
        loadAlert("No se pudo editar el mensaje", "Error", "alert-danger");
    });
}

function EliminarMensaje(id, e) {
    const li = $(e).parent();

    $.ajax({
        type: 'POST',
        url: '/CustomDisplay/DeletMessage',
        data: { id }
    }).done(function () {
        loadAlert("Se elimino el mensaje con exito", "Exito", "alert-success");
        li.fadeOut("fast", function () {
            li.remove();
        });
    }).fail(function () {
        loadAlert("No se pudo eliminar el mensaje", "Error", "alert-danger");
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