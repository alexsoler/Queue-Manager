var connection = new signalR.HubConnectionBuilder().withUrl("/queueHub").build();

connection.start().catch(function (err) {
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

function changeColorSelect(e, area) {
    const areaColor = document.getElementById(area);
    areaColor.classList.remove(areaColor.classList.item(1));
    areaColor.classList.add(e.value);
}

function EditTouchCustom(event) {
    event.preventDefault();

    $.ajax({
        type: 'POST',
        url: '/CustomTouch/SaveTouchCustom',
        data: $("#FormEditTouch").serialize()
    }).done(function () {
        loadAlert("Se guardaron los cambios con exito", "Exito", "alert-success");
        connection.invoke("ReloadPage", "touch").catch(function (err) {
            return console.error(err.toString());
        });
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