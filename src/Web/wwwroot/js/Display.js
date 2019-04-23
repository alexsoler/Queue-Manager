let listaDeReproduccion, contadorMedia;


var connection = new signalR.HubConnectionBuilder().withUrl("/queueHub").build();

//Inicio de conexion signalr
connection.start().then(function () {
    connection.invoke("AddToGroup", "display").catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
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

connection.on("ConnectToOffice", function (message) {
    console.log(message);
});

connection.on("Reload", function () {
    setTimeout(function () {
        window.location.reload();
    }, 500);
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

    speak(`Ticket <silence msec="100" /> ${ticket.ticket}! ${ticket.office}`);
});

connection.on("CallBackDisplayTicket", function (ticket) {
    let Ticket = document.getElementById("displayTicket");

    if (Ticket.innerText.indexOf(ticket.ticket) > -1) {
        $(Ticket).fadeTo(100, 0.1)
            .fadeTo(100, 1)
            .fadeTo(100, 0.1)
            .fadeTo(100, 1);
    }

    speak(`Ticket <silence msec="100" /> ${ticket.ticket}! ${ticket.office}`);
});


$(function () {
    getMediaList();
    document.getElementById("video").addEventListener("ended", nextMediaPlay, false);
    document.getElementById("audio").addEventListener("ended", nextMediaPlay, false);

    loadMessages();
});

function getMediaList() {
    $.get("/Display/GetPlayList", function (response) {
        listaDeReproduccion = response;

        if (listaDeReproduccion.length > 0) {
            contadorMedia = 0;
            playMedia(listaDeReproduccion[contadorMedia].idMedia, listaDeReproduccion[contadorMedia].contentType);
        }
    });
}

function playMedia(id, contentType) {
    if (contentType.includes("image")) {
        var img = document.getElementById("img");
        img.src = `/Medias/GetMedia/${id}`;
        $(img).fadeIn("slow", () => {
            setTimeout(() => $(img).fadeOut("slow", () => nextMediaPlay()), duracionImagenes);
        });
    }
    else if (contentType.includes("video")) {
        var video = document.getElementsByTagName('video')[0];

        if (video.canPlayType(contentType) === "")
            return;

        var source = document.getElementById("vid");
        source.src = `/Medias/GetMedia/${id}`;
        source.type = contentType;
        $("#embedVideo").show();

        video.load();
        video.volume = volumenMultimedia;
    }
    else if (contentType.includes("audio")) {
        var audio = document.getElementsByTagName('audio')[0];

        if (audio.canPlayType(contentType) === "")
            return;

        var sourceAud = document.getElementById("aud");
        sourceAud.src = `/Medias/GetMedia/${id}`;
        sourceAud.type = contentType;

        $("#embedAudio").show();
        audio.load();
        audio.volume = volumenMultimedia;
    }
}

function nextMediaPlay() {
    var img = document.getElementById("img");
    var vid = document.getElementById("vid");
    var aud = document.getElementById("aud");

    if (img.hasAttribute('src')) {
        img.removeAttribute('src');
    }
    else if (vid.hasAttribute('src')) {
        vid.parentElement.pause();
        vid.removeAttribute('src');
        vid.removeAttribute('type');
        vid.parentElement.load();
    }
    else if (aud.hasAttribute('src')) {
        aud.parentElement.pause();
        aud.removeAttribute('src');
        aud.removeAttribute('type');
        aud.parentElement.load();
    }

    $("#img, #embedVideo, #embedAudio").hide();

    if (listaDeReproduccion.length === ++contadorMedia) {
        getMediaList();
    }
    else {
        playMedia(listaDeReproduccion[contadorMedia].idMedia, listaDeReproduccion[contadorMedia].contentType);
    }
}

//Mostrar mensajes
function loadMessages() {

    $("#messagesCard").load("/Display/MessagesPV", function () {
        const $carousel = $("#carouselExampleSlidesOnly");
        $carousel.on("slide.bs.carousel", function () {
            if ($("#messagesCarousel").children().last().hasClass("active")) {
                loadMessages();
            }
        });

        $carousel.carousel({ interval: duracionMensajes });
    });
}

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
    msg.volume = volumenVoz;
    msg.rate = 0.8;
    msg.pitch = 1;
    msg.lang = "es-MX";

    window.speechSynthesis.speak(msg);
}