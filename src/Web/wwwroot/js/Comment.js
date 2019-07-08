$(function () {
    loadTable(`/Comments/Table`);
});

function loadTable(url) {
    $("#divTabla").load(url, function () {
        $("a.page-link").on("click", function (event) {
            event.preventDefault();

            var $pageLink = $(this);

            if ($pageLink.hasClass("disabled"))
                return;

            var isview = document.getElementById("checkCommentsView").checked;

            loadTable($pageLink.attr("href").concat(`&isView=${isview}`));
        });
    });
}

function changeCheckboxView(e) {
    loadTable(`/Comments/Table?isView=${e.checked}`);
}

function changeCommentToView(id, e) {
    $.ajax({
        type: 'POST',
        url: '/Comments/ChangeCommentToView',
        data: { id }
    }).done(function () {
        $(e).parent().parent().fadeOut("fast");
    }).fail(function (message) {
        console.error(message);
    });
}