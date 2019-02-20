function Guardar() {

    office.OfficeTasks = $("input[name='Tareas']").map(function () {
        if (this.checked) {
            return { OfficeId: office.Id, TaskId: this.value };
        }
    }).get();

    office.OfficeOperators = $("input[name='Operadores']").map(function () {
        if (this.checked) {
            return { OfficeId: office.Id, ApplicationUserId: this.value };
        }
    }).get();

    $.ajax({
        type: 'POST',
        url: '/Offices/TaskAndOperatorsSave',
        data: { officevm: office },
        beforeSend: function (xhr) {
            xhr.setRequestHeader("MY-XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
        }
    }).done(function (result) {
        console.log(result);
        window.location = "/Offices/Index";
    }).fail(function (result) {
        console.log(result);
    });
}

function Cancelar() {
    window.location = "/";
}