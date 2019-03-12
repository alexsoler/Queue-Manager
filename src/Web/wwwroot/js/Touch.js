"use strict";
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
    console.log(ticketParameter);
});

connection.start().catch(function (err) {
    return console.error(err.toString());
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

