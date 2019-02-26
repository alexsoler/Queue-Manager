function CrearTarea(event) {
    event.preventDefault();
    let formIsValid = $("#FormCreateTask").valid();

    if (formIsValid) {
        var data = $("#FormCreateTask").serialize();

        $.ajax({
            type: 'POST',
            url: '/Tasks/Create',
            data: data
        }).done(function (result) {
            console.log(result);
            AgregarCboxTask(result);
        }).fail(function (result) {
            console.log(result);
        });

        $("#modalAddTask").modal("hide");
        document.getElementById("FormCreateTask").reset();
    }
}

function AgregarCboxTask(task) {
    const checkBlock = `<div class="form-check" style="width: 200px">
        <input type="checkbox" class="form-check-input" name="Tareas" value="${task.id}" />
        <label class="form-check-label" for="Tareas">${task.name}</label></div>`;

    $("#checkTasks").append(checkBlock);
}