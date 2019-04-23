$(function () {
    loadTable(`/Medias/Table`);

    $('#verModal').on('hidden.bs.modal', function (e) {
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
    });
});

function sentFiles(inputId) {
    var input = document.getElementById(inputId);
    var files = input.files;
    var formData = new FormData();

    if (files.length < 1)
        return;

    for (var i = 0; i < files.length; i++) {
        formData.append("files", files[i]);
    }

    $("#progressUpload").parent().toggleClass('d-none');

    $.ajax({
        url: '/Medias/UploadFiles',
        data: formData,
        processData: false,
        contentType: false,
        type: 'POST',
        xhr: function () {
            var xhr = $.ajaxSettings.xhr();

            xhr.upload.addEventListener("progress", function (evt) {
                if (evt.lengthComputable) {
                    var percentComplete = evt.loaded / evt.total * 100;
                    $("#progressUpload").css({ "width": percentComplete + "%" });
                }
            }, false);

            return xhr;
        }
    }).done(
        result => {
            console.log(result);
            loadAlert("Operación realizada con exito", "Exito", "alert-success");
            loadTable(`/Medias/Table`);
        }
    ).fail(
        result => {
            console.log(result);
            loadAlert("No se pudo realizar la operación", "Error", "alert-danger");
        }
    ).always(() => {
        setTimeout(() => {
            $("#progressUpload").parent().toggleClass('d-none');
            $("#progressUpload").css({ "width": "0%" });
        }, 500);
    });
}

function loadTable(url) {
    $("#divTabla").load(url, function () {
        $("a.page-link").on("click", function (event) {
            event.preventDefault();

            var $pageLink = $(this);

            if ($pageLink.hasClass("disabled"))
                return;

            loadTable($pageLink.attr("href"));
        });
    });
}

function loadView(id, contentType) {
    if (contentType.includes("image")) {
        var img = document.getElementById("img");
        img.src = `/Medias/GetMedia/${id}`;
        $(img).show();
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
    }
}

function loadDelete(id) {
    $("#bodyDelete").load(`/Medias/Delete/${id}`);
}

function EliminarMedia() {
    let id = $("#deleteMedia").val();

    $.ajax({
        type: 'POST',
        url: '/medias/delete',
        data: { id },
        beforeSend: function (xhr) {
            xhr.setRequestHeader("MY-XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
        }
    }).done(function (response) {
        console.log(response);
        const pageItemActive = document.getElementById("pageItemActive");

        if (pageItemActive !== null) {
            var idCurrentItem = document.getElementById("pageItemActive").dataset.val;

            loadTable(`/Medias/Table?pag=${idCurrentItem}`);
        }
        else {
            loadTable(`/Medias/Table`);
        }

        loadAlert("El archivo ha sido eliminado con exito", "Exito", "alert-success");
    }).fail(function () {
        console.log("No se pudo eliminar el archivo");
        loadAlert("No se pudo eliminar el archivo", "Error", "alert-danger");
    });

    $("#eliminarModal").modal('hide');
}

function loadAlert(mensaje, tipoMensaje, nameClass) {
    $("#cardPrincipalBody").prepend(`<div class="alert ${nameClass} alert-dismissible fade show" role="alert">
        <strong>${tipoMensaje}!</strong > ${mensaje}<button class="close" type = "button" data-dismiss="alert" aria-label="Close" >
        <span aria-hidden="true">×</span></button></div>`);

    setTimeout(function () {
        $(".alert").alert('close');
    }, 3000);
}

