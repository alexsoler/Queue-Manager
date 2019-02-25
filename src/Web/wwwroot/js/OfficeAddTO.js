function Guardar() {

    office.Tasks = $("input[name='Tareas']").map(function () {
        if (this.checked) {
            return { Id: this.value };
        }
    }).get();

    office.Operators = $("input[name='Operadores']").map(function () {
        if (this.checked) {
            return { Id: this.value };
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
    window.location = "/Offices/Index";
}