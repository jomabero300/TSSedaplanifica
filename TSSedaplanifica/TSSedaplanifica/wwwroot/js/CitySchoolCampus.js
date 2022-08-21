$(document).ready(function () {
    $("#cityId").change(function () {
        $("#schoolId").empty();
        $.ajax({
            type: 'GET',
            url: '/Solicits/GeneListSchools',
            dataType: 'json',
            data: { Id: $("#cityId").val(), IsEsta :true},
            success: function (data) {
                $.each(data, function (i, data) {

                    $("#schoolId").append('<option value="'
                        + data.id + '">'
                        + data.name + '</option>');
                });

            },
            error: function (ex) {
                //swal({ title: "Error", text: "Se presento lo siguiente, \n" + ex, icon: "error", button: false, timer: 1500 });
                alert('Se presento un error.');
            }
        });
        return false;
    })

    $("#schoolId").change(function () {
        $("#campusId").empty();
        $.ajax({
            type: 'GET',
            url: '/Solicits/GeneListSchools',
            dataType: 'json',
            data: { Id: $("#schoolId").val()},
            success: function (data) {
                $.each(data, function (i, data) {

                    $("#campusId").append('<option value="'
                        + data.id + '">'
                        + data.name + '</option>');
                });

            },
            error: function (ex) {
                //swal({ title: "Error", text: "Se presento lo siguiente, \n" + ex, icon: "error", button: false, timer: 1500 });
                alert('Se presento un error.');
            }
        });
        return false;
    })
})