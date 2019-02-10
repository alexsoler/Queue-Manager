function EditarUsuario() {
    let formIsValid  = $("#FormEdit").valid();

    if (formIsValid) {
        var data = {
            Name: $("#Name").val(),
            UserName: $("#UserName").val(),
            Email: $("#Email").val(),
            PhoneNumber: $("#PhoneNumber").val(),
            NewPassword: $("#NewPassword").val(),
            Roles: $("input[name='Roles']").map(function (index) {
                return { Rol: this.value, Asignado: this.checked };
            }).get()
        };


        let jsonData = JSON.stringify(data);

        $.ajax({
            type: 'POST',
            url: '/Usuarios/Edit',
            data: jsonData,
            contentType: "application/json",
            dataType: "json",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("MY-XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            success: function (response) {
                console.log(response);

                if (document.URL.includes("EditarUsuario")) {
                    $("#cardPrincipalBody > div.mb-2 > form").submit();
                }
                else {
                    loadTable();
                }
                
                loadAlert("El usuario ha sido actualizado con exito.", "Exito", "alert-success");
            },
            failure: function () {
                console.log("No se pudo editar el usuario");
                loadAlert("No se pudo editar el usuario", "Error", "alert-danger");
            }
        });

        $("#editarModal").modal('hide');
    }
}

function EliminarUsuario() {
    let username = $("#deleteUser").val();

    $.ajax({
        type: 'POST',
        url: '/Usuarios/delete',
        data: { username },
        dataType: "json",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("MY-XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: function (response) {
            console.log(response);

            if (document.URL.includes("EliminarUsuario")) {
                $("#cardPrincipalBody > div.mb-2 > form").submit();
            }
            else {
                loadTable();
            }

            loadAlert("El usuario ha sido eliminado con exito", "Exito", "alert-success");
        },
        failure: function () {
            console.log("No se pudo eliminar el usuario");
            loadAlert("No se pudo eliminar el usuario", "Error", "alert-danger");
        }
    });

    $("#eliminarModal").modal('hide');
}

function loadTable() {
    $("#divTabla").load('/Identity/Account?handler=ViewPartialTable');
}

function loadDetails(username) {
    $.ajax({
        type: 'GET',
        url: '/Usuarios/Details',
        data: { username },
        success: function (response) {
            $("#bodyDetails").html(response);
        },
        failure: function (response) {
            console.log(response);
        }
    });
}

function loadEdit(username) {
    $.ajax({
        type: 'GET',
        url: '/Usuarios/Edit',
        data: { username },
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

function loadDelete(username) {
    $.ajax({
        type: 'GET',
        url: '/Usuarios/Delete',
        data: { username },
        success: function (response) {
            $("#bodyDelete").html(response);
        },
        failure: function (response) {
            console.log(response);
        }
    });
}

function loadAlert(mensaje, tipoMensaje, nameClass) {
    $("#cardPrincipalBody").prepend('<div class="alert ' + nameClass + ' alert-dismissible fade show" role="alert">' +
        '<strong> ' + tipoMensaje + '!</strong > ' +mensaje+
        '<button class="close" type = "button" data-dismiss="alert" aria-label="Close" >' +
        '<span aria-hidden="true">×</span>' +
        '</button>' +
        '</div>');

    setTimeout(function () {
        $(".alert").alert('close');
    }, 5000);
}

function searchUser(event) {

    event.preventDefault();

    let currentSearch = document.getElementById('inputSearch').value;
    let typeResult = document.getElementById('inputTypeSearch').value;

    var data = { currentSearch, typeResult };

    $.ajax({
        type: 'GET',
        url: '/Usuarios/Search',
        data: data,
        success: function (result) {
            $("#collapseResult").html(result).collapse();
            
        },
        failure: function (response) {
            console.log(response);
        }
    });
}

