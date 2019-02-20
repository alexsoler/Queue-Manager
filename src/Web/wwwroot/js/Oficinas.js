var gestionTareas = {
    tareasAgregadas: [],
    tareasNuevas: [],
    tareasEliminadas: []
};

var gestionOperadores = {
    operadoresAgregados: [],
    operadoresNuevos: [],
    operadoresEliminados: []
};


$(function () {

    $("#editarModal").on('shown.bs.modal', function () {
        gestionTareas.tareasAgregadas = $("input[name = 'Tareas']").map(function () {
            if (this.checked) return this.value;
        }).get();
        gestionTareas.tareasNuevas = [];
        gestionTareas.tareasEliminadas = [];

        gestionOperadores.operadoresAgregados = $("input[name = 'Operadores']").map(function () {
            if (this.checked) return this.value;
        }).get();
        gestionOperadores.operadoresNuevos = [];
        gestionOperadores.operadoresEliminados = [];
    });

    $(document).on("click", "input[name = 'Tareas']", function () {
        if (this.checked) {
            if (!gestionTareas.tareasAgregadas.includes(this.value))
                gestionTareas.tareasNuevas.push(this.value);

            if (gestionTareas.tareasEliminadas.includes(this.value)) {
                let indice = gestionTareas.tareasEliminadas.indexOf(this.value);
                gestionTareas.tareasEliminadas.splice(indice, 1);
            }
        }
        else {
            if (gestionTareas.tareasAgregadas.includes(this.value))
                gestionTareas.tareasEliminadas.push(this.value);

            if (gestionTareas.tareasNuevas.includes(this.value)) {
                let indice = gestionTareas.tareasNuevas.indexOf(this.value);
                gestionTareas.tareasNuevas.splice(indice, 1);
            }
        }
    });

    $(document).on("click", "input[name = 'Operadores']", function () {
        if (this.checked) {
            if (!gestionOperadores.operadoresAgregados.includes(this.value))
                gestionOperadores.operadoresNuevos.push(this.value);

            if (gestionOperadores.operadoresEliminados.includes(this.value)) {
                let indice = gestionOperadores.operadoresEliminados.indexOf(this.value);
                gestionOperadores.operadoresEliminados.splice(indice, 1);
            }
        }
        else {
            if (gestionOperadores.operadoresAgregados.includes(this.value))
                gestionOperadores.operadoresEliminados.push(this.value);

            if (gestionOperadores.operadoresNuevos.includes(this.value)) {
                let indice = gestionOperadores.operadoresNuevos.indexOf(this.value);
                gestionOperadores.operadoresNuevos.splice(indice, 1);
            }
        }
    });
});

function loadEdit(id) {
    $.ajax({
        type: 'GET',
        url: '/Offices/Edit',
        data: { id },
        success: function (response) {
            $("#bodyEdit").html(response);

            var form = $('#FormEdit');
            form.removeData('validator');
            form.removeData('unobtrusiveValidation');
            $.validator.unobtrusive.parse(form);
        },
        failure: function (response) {
            console.log(response);
        }
    });
}

function EditarOficina(event) {
    event.preventDefault();
    let formIsValid = $("#FormEdit").valid();

    if (!formIsValid)
        return;

    let oficina = {
        Id: document.forms['FormEdit'][0].value,
        Name: document.forms['FormEdit'][1].value,
        Prefix: document.forms['FormEdit'][2].value,
        Description: document.forms['FormEdit'][3].value,
        CreationDate: document.forms['FormEdit'][4].value
    };

    var data = {
        officeViewModel: oficina,
        tareasNuevas: gestionTareas.tareasNuevas,
        tareasEliminadas: gestionTareas.tareasEliminadas,
        operadoresNuevos: gestionOperadores.operadoresNuevos,
        operadoresEliminados: gestionOperadores.operadoresEliminados
    };

    $.ajax({
        type: 'POST',
        url: '/Offices/Edit',
        data: data,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("MY-XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
        }
    }).done(response => {
        console.log(response);
        loadTable();
        loadAlert("La oficina ha sido editada con exito.", "Exito", "alert-success");

    }).fail(response => {
        console.log("No se pudo editar la oficina: " + response);
        loadAlert("No se pudo editar la oficina", "Error", "alert-danger");
    });

    $("#editarModal").modal('hide');
}

function loadTable() {
    var pag = $("#pageItemActive").data("val");
    $("#divTabla").load(`/Offices/TableOffices?pag=${pag}`);
}

function loadAlert(mensaje, tipoMensaje, nameClass) {
    $("#cardPrincipalBody").prepend(`<div class="alert ${nameClass} alert-dismissible fade show" role="alert">
        <strong>${tipoMensaje}!</strong > ${mensaje}<button class="close" type = "button" data-dismiss="alert" aria-label="Close" >
        <span aria-hidden="true">×</span></button></div>`);

    setTimeout(function () {
        $(".alert").alert('close');
    }, 5000);
}