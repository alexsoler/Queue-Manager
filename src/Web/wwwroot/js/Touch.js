﻿"use strict";

var tarea, prioridad;
var ticketParameter = TicketParameter;

function TicketParameter (Id, DisplayTokenName, NumberTicket, NameTask, NamePriority, CreationDate) {
    this.Id = Id;
    this.DisplayTokenName = DisplayTokenName;
    this.NumberTicket = NumberTicket;
    this.NameTask = NameTask;
    this.NamePriority = NamePriority;
    this.CreationDate = CreationDate;
}


var connection = new signalR.HubConnectionBuilder().withUrl("/queueHub").build();

connection.on("ReceiveToken", function (ticketParameter) {
    printJS({
        printable: `/touch/ticket?DisplayTokenName=${ticketParameter.displayTokenName}&NameTask=${ticketParameter.nameTask}&NamePriority=${ticketParameter.namePriority}`,
        type: 'pdf',
        showModal: true,
        modalMessage: mensajeNotificacion
    });

    closeNav();
    console.log(ticketParameter);
});

connection.on("Reload", function () {
    setTimeout(function () {
        window.location.reload();
    }, 500);
});

connection.start().then(function () {
    connection.invoke("AddToGroup", "touch").catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    console.error(err.toString());

    return alert("No se pudo conectar, recargue la pagina.");
});

async function start() {
    try {
        await connection.start();
        console.log('connected');
    } catch (err) {
        console.log(err);
        setTimeout(() => start(), 5000);
    }
}

connection.onclose(async () => {
    await start();
});

function openNav(idTarea) {
    document.getElementById("myNav").style.height = "100%";
    tarea = idTarea;
}

function closeNav() {
    document.getElementById("myNav").style.height = "0%";
}

function crearToken(idPrioridad) {
    prioridad = idPrioridad;
    console.log(tarea, prioridad);

    connection.invoke("CreateToken", { IdTarea: tarea, IdPrioridad: prioridad })
        .catch(function (err) {
            return console.error(err.toString());
        });

    event.preventDefault();
}

function SendComment(event) {
    event.preventDefault();

    $.ajax({
        type: 'POST',
        url: '/Comments/SaveComment',
        data: $("#FormComment").serialize()
    }).done(function () {
        loadAlert("Se envio su comentario", "Exito", "alert-success");

    }).fail(function () {
        loadAlert("No se pudo enviar el comentario", "Error", "alert-danger");
    });

    $("#commentsModal").modal('hide');
}

$('#commentsModal').on('hidden.bs.modal', function (e) {
    // do something...
    document.getElementById("FormComment").reset();
});

$('#signoutModal').on('hidden.bs.modal', function (e) {
    // do something...
    document.getElementById("FormLogOut").reset();
});

//Remueve la pantalla touch del grupo signalR 
window.addEventListener("beforeunload", function (event) {
    connection.invoke("RemoveFromGroup", "touch").catch(function (err) {
        return console.error(err.toString());
    });

    var confirmationMessage = "\o/";

    (e || this.window.event).returnValue = confirmationMessage;
    return confirmationMessage;
});

function loadAlert(mensaje, tipoMensaje, nameClass) {
    $("body").prepend(`<div class="alert ${nameClass} alert-dismissible fade show" role="alert">
        <strong>${tipoMensaje}!</strong > ${mensaje}<button class="close" type = "button" data-dismiss="alert" aria-label="Close" >
        <span aria-hidden="true">×</span></button></div>`);

    setTimeout(function () {
        $(".alert").alert('close');
    }, 2000);
}