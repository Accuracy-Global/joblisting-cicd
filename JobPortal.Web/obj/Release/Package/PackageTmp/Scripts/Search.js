$(function () {
    $.getJSON("/help/countrylist", {}, function (list) {
        $("#CountryId").empty();
        $("#CountryId").append("<option value = ''>SELECT</option>");

        if (list.length != 0) {
            $(list).each(function () {
                $("#CountryId").append("<option value =" + this.Id + ">" + this.Name + "</option>");
            });
        }
    });

    $.getJSON("/help/categorylist", {}, function (list) {
        $("#CategoryId").empty();
        $("#CategoryId").append("<option value = ''>SELECT</option>");

        if (list.length != 0) {
            $(list).each(function () {
                $("#CategoryId").append("<option value =" + this.Id + ">" + this.Name + "</option>");
            });
        }
    });

    $.getJSON("/help/qualifications", {}, function (list) {
        $("#QualificationId").empty();
        $("#QualificationId").append("<option value = ''>SELECT</option>");

        if (list.length != 0) {
            $(list).each(function () {
                $("#QualificationId").append("<option value =" + this.Id + ">" + this.Name + "</option>");
            });
        }
    });

    $("#AgeMin").change(function () {
        $.post("/jsonhelper/getagelist", { age: $('option:selected', $(this)).val() },
        function (result) {
            $("#AgeMax").empty();
            $("#AgeMax").append("<option value =\"\">SELECT</option>");
            if (result.length != 0) {
                $(result).each(function () {
                    $("#AgeMax").append("<option value =" + this.Value + ">" + this.Text + "</option>");
                });
            }
        });
    });

    $("#CountryId").change(function () {
        $("#StateId").empty();
        $.getJSON("/help/statelist", { countryId: $(this).val() }, function (result) {
            $("#StateId").append("<option value =" + '' + ">" + 'SELECT' + "</option>");
            if (result.length != 0) {
                $(result).each(function () {
                    $("#StateId").append("<option value =" + this.Id + ">" + this.Name + "</option>");
                });
            }
        });
    });

    $("#CategoryId").change(function () {
        if($(this).val()!=null){
            $("#SpecializationId").empty();
            $("#SpecializationId").append("<option value =\"\">SELECT</option>");
            $.getJSON("help/specializationlist", { categoryId: $(this).val() }, function (result) {
                if (result.length != 0) {
                    $(result).each(function () {
                        $("#SpecializationId").append("<option value =" + this.Id + ">" + this.Name + "</option>");
                    });
                    $("#SpecializationId").val($("#SpecializationId").data("value"));
                }
            });
        }
    });
});