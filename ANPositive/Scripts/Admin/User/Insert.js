$(function () {
    $("#form").find("*").filter(":input:visible:first").focus();

    $("button.insert").unbind("click").click(function () {
        var formError = [];

        $("#form .validate").each(function (index, obj) {
            var key = $(obj).attr("id"),
                value = $(obj).val(),
                element = $(obj).data("element").toLowerCase(),
                $error = application.form.verify(obj, key, value, element);

            if (formError.length === 0 && $error.error) {
                formError = $error;
            } else if ($error.error) {
                formError.message.push($error.message);
            }
        });

        if (formError.error) {
            $(formError.focus).focus();
            $("#formWarning").html(formError.message.join(""));
            setTimeout(function () {
                $("#formWarning")
                    .fadeIn("slow")
                    .removeClass("hide")
                    .addClass("show")
                    .animate({ backgroundColor: "#faebcc" }, 1500)
                    .animate({ backgroundColor: "#f2dede" }, 1500);
            }, 500);
            $("#goToUp").click();
            $("ul.nav.nav-tabs > li > a[href=\"#" + $(formError.focus).closest("div.tab-pane").attr("id") + "\"]").tab("show");
            return false;
        } else {
            formError = [];
            $("fieldset.form-group").removeClass("has-error").addClass("has-success");

            var data = { };

            $(".formelement").each(function (index, obj) {
                var key = $(obj).attr("id");
                data[key] = application.form.formatValues(obj);
            });

            application.form.sendAjax(data, ajaxUrl, true, redirectUrl);
        }
    });
});