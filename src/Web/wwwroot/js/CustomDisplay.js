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