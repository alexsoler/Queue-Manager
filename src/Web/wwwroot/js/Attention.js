//Parametros para al metodo OnHoldTable en el controllador
var idOffice; 
var sortOrder;
var actualTicketId; 
var asc;
var estadosTicket = {
    CALLED: "called",
    INASSISTANCE: "inassistance",
    FINISHED: "finished",
    NONE: "none"
};
var estado = estadosTicket.NONE;

var connection = new signalR.HubConnectionBuilder().withUrl("/queueHub").build();

connection.on("ReceiveToken", function (ticketParameter) {
    //loadTable();
    createNewTicket(ticketParameter);
    asc = !asc;
    sortTable(sortOrder);
    //filterTable();

    $(`tr#${ticketParameter.id}`).fadeIn("slow");
    filterTable();
    asc = !asc;
    console.log(ticketParameter);
});

//Carga tabla cuando el usuario se conecta a la oficina
connection.on("ConnectToOffice", function (message) {
    console.log(message);
    sortOrder = 3; //Establece el ordenamiento por hora de creacion de forma predeterminada
    loadTable();

    document.getElementById("btnLlamarSiguiente").disabled = false; //Habilita el boton llamar siguiente
});

//Cuando un ticket es llamado deja de estar disponible para otras oficinas
connection.on("RemoveTicketCalled", function (id) {
    //loadTable();
    var $trDelet = $(`tr#${id}`);
    $trDelet.fadeOut("fast", (complete) => {
        $trDelet.remove();
    }); //Elimina el ticket
});

//Si la oficina llama a un ticket carga la vista parcial para atenderlo
connection.on("ToAttendTicket", function (id) {
    estado = estadosTicket.CALLED;
    actualTicketId = id;
});

//Inicio de conexion signalr
connection.start().catch(function (err) {
    return console.error(err.toString());
});

$(function () {

});

function loadTable() {
    $("#divTabla").load(`/Attention/OnHoldTable?idOffice=${idOffice}`, function () {
        asc = false;
    });
}

//Carga oficina al seleccionarla
function loadOffice(id, e) {

    //Cuando se cambia de oficina se elimina del grupo de signalr la anterior
    if (idOffice !== undefined) {
        connection.invoke("RemoveFromGroup", idOffice.toString()).catch(function (err) {
            return console.error(err.toString());
        });


    }

    //Agrega el usuario al grupo de oficina
    connection.invoke("AddToGroup", id.toString()).catch(function (err) {
        return console.error(err.toString());
    });

    $("#SelectOffice").text(e.text); //El boton para seleccionar la oficina adquiere el nombre de la oficina seleccionada

    idOffice = id;
}

//Se ejecuta cada vez que se llama a un cliente
function callClient(idTicket, e) {
    if (estado === estadosTicket.CALLED) {
        $.ajax({
            type: 'POST',
            url: '/Attention/NotAttention',
            data: { id: actualTicketId }
        }).done(() => {
            estado = estadosTicket.CALLED;
        });
    }
    else if (estado === estadosTicket.INASSISTANCE) {
        $.ajax({
            type: 'POST',
            url: '/Attention/FinalizeAttention',
            data: { id: actualTicketId }
        }).done(() => {
            var BUTTON = document.getElementById("IniciarAtencion");
            BUTTON.innerHTML = `<i class="fa fa-play"></i>
                               <br />
                               Iniciar Atención`;
            BUTTON.dataset.mode = "start";
            document.getElementById("btnLlamarSiguiente").disabled = false;

            estado = estadosTicket.FINISHED;

            stopwatch.stop();
            stopwatch.reset();
        }).fail((error) => {
            console.error(error);
        });
    }

    var TR = e.parentNode.parentNode.children;

    document.getElementById("NameTicket").innerText = TR[0].textContent;
    document.getElementById("ddServicio").innerText = TR[1].textContent;
    document.getElementById("ddPrioridad").innerText = TR[2].textContent.trim();

    document.getElementById("RepetirLlamado").disabled = false;
    document.getElementById("IniciarAtencion").disabled = false;
    document.getElementById("NoSePresento").disabled = false;

    connection.invoke("CallClient", idTicket, idOffice).catch(function (err) {
        return console.error(err.toString());
    });
}

//Se ejecuta cuando se da click al boton "Llamar siguiente"
function CallNext() {
    var rowTicket = $("#tableTickets > tbody > tr td");

    if (rowTicket.length < 1)
        return;

    $(rowTicket[4].firstElementChild).trigger("click");
}

//Limpia todos los tickets en espera
function LimpiarTickets() {
    var TRs = $("tbody tr");
    var idArray = TRs.map(function () { return $(this).attr("id"); }).get();

    $.ajax({
        type: 'POST',
        url: '/Attention/ClearAll',
        data: { idTickets: idArray }
    }).done(() => TRs.remove()).fail((error) => console.error(error));

    $("#limpiarModal").modal('hide');
}

//Quitar usuario del grupo oficina
window.addEventListener("beforeunload", function (event) {
    if (estado === estadosTicket.CALLED) $.post('/Attention/NotAttention', { id: actualTicketId }, () => { });
    else if (estado === estadosTicket.INASSISTANCE) $.post('/Attention/FinalizeAttention', { id: actualTicketId }, () => { });

    connection.invoke("RemoveFromGroup", idOffice.toString()).catch(function (err) {
        return console.error(err.toString());
    });

    var confirmationMessage = "\o/";

    (e || this.window.event).returnValue = confirmationMessage;
    return confirmationMessage;
});


function createNewTicket(ticket) {
    var priorityClass = ticket["namePriority"] === "Normal" ? "badge badge-success" : "badge badge-warning";

    var newToken = `<tr id="${ticket["id"]}" style="display: none;">
                <td>${ticket["displayTokenName"]}</td>
                <td>${ticket["nameTask"]}</td>
                <td>
                    <span class="${priorityClass}">
                        ${ticket["namePriority"]}
                    </span>
                </td>
                <td>${ticket["creationDate"]}</td>
                <td>
                    <a class="btn btn-sm btn-info mr-1" data-toggle="modal" data-target="#verModal" onclick="callClient('${ticket["id"]}', this)">Llamar</a>
                </td>
            </tr>`;

    $("table tbody").append(newToken);
}

//Se ejecuta el dar click en Iniciar/Finalizar Atención
function startOrFinalizeAttention(BUTTON) {
    var mode = BUTTON.dataset.mode;

    if (mode === "start") {
        $.ajax({
            type: 'POST',
            url: '/Attention/StartAttention',
            data: { id: actualTicketId }
        }).done((e) => {
            BUTTON.innerHTML = `<i class="fa fa-stop"></i>
                                <br />
                                Finalizar Atención`;
            BUTTON.dataset.mode = "finalize";

            document.getElementById("RepetirLlamado").disabled = true;
            document.getElementById("NoSePresento").disabled = true;
            document.getElementById("btnLlamarSiguiente").disabled = true;

            estado = estadosTicket.INASSISTANCE;

            stopwatch.start();

            console.log(e);
        }).fail((error) => {
            console.error(error);
        });
    }
    else {
        $.ajax({
            type: 'POST',
            url: '/Attention/FinalizeAttention',
            data: { id: actualTicketId }
        }).done(() => {
            BUTTON.innerHTML = `<i class="fa fa-play"></i>
                               <br />
                               Iniciar Atención`;
            BUTTON.dataset.mode = "start";
            BUTTON.disabled = true;
            document.getElementById("NameTicket").innerHTML = "&nbsp;";
            document.getElementById("ddServicio").innerText = "";
            document.getElementById("ddPrioridad").innerText = "";
            document.getElementById("btnLlamarSiguiente").disabled = false;

            estado = estadosTicket.FINISHED;

            stopwatch.stop();
            stopwatch.reset();
        }).fail((error) => {
                console.error(error);
        });
        
    }
}

//Se ejecuta si la persona no se presenta
function NotAttention(BUTTON) {
    $.ajax({
        type: 'POST',
        url: '/Attention/NotAttention',
        data: { id: actualTicketId }
    }).done(() => {
        document.getElementById("NameTicket").innerHTML = "&nbsp;";
        document.getElementById("ddServicio").innerText = "";
        document.getElementById("ddPrioridad").innerText = "";

        BUTTON.disabled = true;
        document.getElementById("RepetirLlamado").disabled = true;
        document.getElementById("IniciarAtencion").disabled = true;

        estado = estadosTicket.NONE;
    }).fail((error) => {
        console.error(error);
    });
}

function sortTable(n, event, e) {
    sortOrder = n;
    var table, rows, switching, i, x, y, shouldSwitch;
    table = document.getElementById("tableTickets");
    switching = true;
    /* Make a loop that will continue until
    no switching has been done: */
    while (switching) {
        // Start by saying: no switching is done:
        switching = false;
        rows = table.rows;
        /* Loop through all table rows (except the
        first, which contains table headers): */
        for (i = 1; i < (rows.length - 1); i++) {
            // Start by saying there should be no switching:
            shouldSwitch = false;
            /* Get the two elements you want to compare,
            one from current row and one from the next: */
            x = rows[i].getElementsByTagName("TD")[n];
            y = rows[i + 1].getElementsByTagName("TD")[n];
            // Check if the two rows should switch place:

            if (asc) {
                if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase()) {
                    // If so, mark as a switch and break the loop:
                    shouldSwitch = true;
                    break;
                }
            }
            else {
                if (x.innerHTML.toLowerCase() < y.innerHTML.toLowerCase()) {
                    // If so, mark as a switch and break the loop:
                    shouldSwitch = true;
                    break;
                }
            }
        }
        if (shouldSwitch) {
            /* If a switch has been marked, make the switch
            and mark that a switch has been done: */
            rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
            switching = true;
        }
    }

    if (event !== undefined) {
        var icon = $("#sortIcon");
        icon.removeClass();

        if (asc && n < 3) {
            icon.addClass("fa fa-sort-alpha-asc");
            asc = false;
        }
        else if (!asc && n < 3) {
            icon.addClass("fa fa-sort-alpha-desc");
            asc = true;
        }
        else if (asc && n === 3) {
            icon.addClass("fa fa-sort-numeric-asc");
            asc = false;
        }
        else if(!asc && n === 3) {
            icon.addClass("fa fa-sort-numeric-desc");
            asc = true;
        }

        $(e).append(icon);
    }
}

function filterTable() {
    // Declare variables 
    var input1, input2 , filter1, filter2, table, tr, td1, td2, i, txtValue1, txtValue2;
    input1 = document.getElementById("selectTask");
    input2 = document.getElementById("selectPriority");
    filter1 = input1.value.toUpperCase();
    filter2 = input2.value.toUpperCase();
    table = document.getElementById("tableTickets");
    tr = table.getElementsByTagName("tr");

    // Loop through all table rows, and hide those who don't match the search query
    for (i = 0; i < tr.length; i++) {
        td1 = tr[i].getElementsByTagName("td")[1];
        td2 = tr[i].getElementsByTagName("td")[2];
        if (td1 && td2) {
            txtValue1 = td1.textContent || td1.innerText;
            txtValue2 = td2.textContent || td2.innerText;
            if (txtValue1.toUpperCase().indexOf(filter1) > -1 &&
                txtValue2.toUpperCase().indexOf(filter2) > -1) {
                tr[i].style.display = "";
            }
            else if (txtValue1.toUpperCase().indexOf(filter1) > -1 &&
                (filter2 === "ESPECIAL" && txtValue2.toUpperCase().indexOf("NORMAL") < 0)) {
                tr[i].style.display = "";
            }
            else {
                tr[i].style.display = "none";
            }
        }
    }
}