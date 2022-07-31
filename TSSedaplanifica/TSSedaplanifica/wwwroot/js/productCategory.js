$(document).ready(function () {
    $("#categoryTypeId").change(function () {
        console.log("Entro por Typo de categorya");
        $("#categoryId").empty();
        $.ajax({
            type: 'GET',
            url: '/Products/GeneListById',
            dataType: 'json',
            data: { categoryTypeId: $("#categoryTypeId").val(), ProductId: $("#productId").val() },
            success: function (data) {
                $.each(data, function (i, data) {

                    $("#categoryId").append('<option value="'
                        + data.id + '">'
                        + data.name + '</option>');
                });
            },
            error: function (ex) {
                swal({ title: "Error", text: "Se presento lo siguiente, \n" + ex, icon: "error", button: false, timer: 1500 });
            }
        });
        return false;
    })

    $("#categoryId").change(function () {
        console.log("Entro por categorya");
        $("#productId").empty();
        $.ajax({
            type: 'GET',
            url: '/Solicits/ProductListById',
            dataType: 'json',
            data: { categoryId: $("#categoryId").val() },
            success: function (data) {
                $.each(data, function (i, data) {

                    $("#productId").append('<option value="'
                        + data.id + '">'
                        + data.name + '</option>');
                });
            },
            error: function (ex) {
                swal({ title: "Error", text: "Se presento lo siguiente, \n" + ex, icon: "error", button: false, timer: 1500 });
            }
        });
        return false;
    })
});