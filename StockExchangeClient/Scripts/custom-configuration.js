function refresh() {

    $.get('/Home/GetTicker', function (result) {
        $('#ticker').html(result);
    });
}

setInterval(refresh, 10000);

$(document).ready(function () {
    $("#AddStockModalDialog").on("show.bs.modal", function (e) {

        $("#AddStockModal").find("input[type=text]").val("").removeClass("input-validation-error");
        $(".field-validation-error").html("").addClass("field-validation-valid").removeClass("field-validation-error");
        $("#errorMessage").text("")
    });

    $("#AddStockModalDialog").on("hide.bs.modal", function (e) {

        $("#AddStockModal").find("input[type=text], select").val("").removeClass("input-validation-error");
        $(".field-validation-error").html("").addClass("field-validation-valid").removeClass("field-validation-error");
        $("#errorMessage").text("")
    });

    $("#confirmDelete").on("show.bs.modal", function (e) {
        var stockId = $(e.relatedTarget).data("id");
        $('#confirmDelete .modal-body').append('<input type="hidden" name="stockToDeleteId" value="' + stockId + '">');
    });
});

function HandleModelResponse(data) {
    if (data.success) {
        location.reload();
    } else {
        console.log(data.message)
        $(".errorMessage").text(data.message)
    }
}

