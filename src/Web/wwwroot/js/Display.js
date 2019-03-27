
var connection = new signalR.HubConnectionBuilder().withUrl("/queueHub").build();

//Inicio de conexion signalr
connection.start().then(function () {
    connection.invoke("AddToGroup", "display").catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("ConnectToOffice", function (message) {
    console.log(message);
});

connection.on("CallDisplayTicket", function (ticket) {
    let $Ticket = $("#displayTicket");
    let $Office = $("#displayOffice");

    let $TicketH1 = $("#ticketHistorico1");
    let $OfficeH1 = $("#officeHistorico1");

    let $TicketH2 = $("#ticketHistorico2");
    let $OfficeH2 = $("#officeHistorico2");

    let $TicketH3 = $("#ticketHistorico3");
    let $OfficeH3 = $("#officeHistorico3");

    $TicketH3.text($TicketH2.text());
    $OfficeH3.text($OfficeH2.text());

    $TicketH2.text($TicketH1.text());
    $OfficeH2.text($OfficeH1.text());

    $TicketH1.text($Ticket.text());
    $OfficeH1.text($Office.text());

    $Ticket.text(ticket.ticket);
    $Office.text(ticket.office);

    $Ticket.fadeTo(100, 0.1)
        .fadeTo(100, 1)
        .fadeTo(100, 0.1)
        .fadeTo(100, 1);

    speak(`Ticket ${ticket.ticket} ${ticket.office}`);
});

connection.on("CallBackDisplayTicket", function (ticket) {
    let Ticket = document.getElementById("displayTicket");

    if (Ticket.innerText.indexOf(ticket.ticket) > -1) {
        $(Ticket).fadeTo(100, 0.1)
            .fadeTo(100, 1)
            .fadeTo(100, 0.1)
            .fadeTo(100, 1);
    }

    speak(`Ticket ${ticket.ticket} ${ticket.office}`);
});


$(function () {

});


//Remueve la pantalla del ddisplay 
window.addEventListener("beforeunload", function (event) {
    connection.invoke("RemoveFromGroup", "display").catch(function (err) {
        return console.error(err.toString());
    });

    var confirmationMessage = "\o/";

    (e || this.window.event).returnValue = confirmationMessage;
    return confirmationMessage;
});

function speak(text) {
    var msg = new SpeechSynthesisUtterance();
    msg.text = text;
    msg.volume = 1;
    msg.rate = 0.8;
    msg.pitch = 0.4;
    msg.lang = "es-MX";

    window.speechSynthesis.speak(msg);
}