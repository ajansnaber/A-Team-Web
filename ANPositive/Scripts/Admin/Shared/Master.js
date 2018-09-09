/*
 * 
 *      ANPositive Master Javascript Function v3.5.1
 *      30.08.2018  - 15.00
 *      
 *
*/

"use strict";

moment.locale("tr");

var images = [],
    table,
    application = {
        name: appName,
        form: {
            sendAjax: function(data, ajaxUrl, redirect, redirectUrl) {
                $(".ibox-content").toggleClass("sk-loading");

                $.ajax({
                    type: "POST",
                    url: ajaxUrl,
                    data: JSON.stringify(data),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function(msg) {
                        var result = msg.result;

                        if (result.success) {
                            toastr["success"](result.message);
                            if (redirect) {
                                setTimeout(function() {
                                        window.location.href = redirectUrl;
                                    },
                                    5000);
                            } else {
                                $(".ibox-content").toggleClass("sk-loading");
                            }
                        } else {
                            toastr["error"](result.message);
                        }
                    },
                    error: function() {
                        console.log("Bir sorun oluştu. Lütfen daha sonra tekrar deneyin.");
                    }
                });
            },
            verify: function(obj, key, value, dataType) {
                var formerror = false,
                    $focuselement,
                    errormessage = "";

                switch (dataType) {
                case "phone":
                    if (!application.form.verifyFunc.text(parseInt($(obj).data("minlength")), value)) {
                        if (!formerror) {
                            $focuselement = $(obj);
                        };
                        $("div." + key).addClass("has-error");
                        errormessage += "<p><i class=\"fa fa-exclamation\"></i> " + $(obj).data("errormsg") + "</p>";
                        formerror = true;
                    } else {
                        $("div." + key).removeClass("has-error").addClass("has-success");
                    }

                    break;
                case "image":
                    if (!application.form.verifyFunc.image()) {
                        if (!formerror) {
                            $focuselement = $(obj);
                        };
                        $("div." + key).addClass("has-error");
                        errormessage += "<p><i class=\"fa fa-exclamation\"></i> " + $(obj).data("errormsg") + "</p>";
                        formerror = true;
                    } else {
                        $("div." + key).removeClass("has-error").addClass("has-success");
                    }

                    break;
                case "url":
                    if (!application.form.verifyFunc.url(value)) {
                        if (!formerror) {
                            $focuselement = $(obj);
                        };
                        $("div." + key).addClass("has-error");
                        errormessage += "<p><i class=\"fa fa-exclamation\"></i> " + $(obj).data("errormsg") + "</p>";
                        formerror = true;
                    } else {
                        $("div." + key).removeClass("has-error").addClass("has-success");
                    }

                    break;
                case "array":
                    if (!application.form.verifyFunc.array(value)) {
                        if (!formerror) {
                            $focuselement = $(obj);
                        };
                        $("div." + key).addClass("has-error");
                        errormessage += "<p><i class=\"fa fa-exclamation\"></i> " + $(obj).data("errormsg") + "</p>";
                        formerror = true;
                    } else {
                        $("div." + key).removeClass("has-error").addClass("has-success");
                    }

                    break;
                case "email":
                    if (!application.form.verifyFunc.email(value)) {
                        if (!formerror) {
                            $focuselement = $(obj);
                        };
                        $("div." + key).addClass("has-error");
                        errormessage += "<p><i class=\"fa fa-exclamation\"></i> " + $(obj).data("errormsg") + "</p>";
                        formerror = true;
                    } else {
                        $("div." + key).removeClass("has-error").addClass("has-success");
                    }

                    break;
                case "date":
                    if (!application.form.verifyFunc.date(value)) {
                        if (!formerror) {
                            $focuselement = $(obj);
                        };
                        $("div." + key).addClass("has-error");
                        errormessage += "<p><i class=\"fa fa-exclamation\"></i> " + $(obj).data("errormsg") + "</p>";
                        formerror = true;
                    } else {
                        $("div." + key).removeClass("has-error").addClass("has-success");
                    }

                    break;
                case "password":
                    var pass1 = $(obj).val(),
                        pass2 = $("#" + $(obj).data("target")).val();

                    if (!application.form.verifyFunc.password(pass1)) {
                        if (!formerror) {
                            $focuselement = $(obj);
                        };
                        $("div." + key).addClass("has-error");
                        errormessage += "<p><i class=\"fa fa-exclamation\"></i> " + $(obj).data("errormsg") + "</p>";
                        formerror = true;
                    } else if (!application.form.verifyFunc.passwordIsEqual(pass1, pass2)) {
                        if (!formerror) {
                            $focuselement = $(obj);
                        };
                        $("div." + key).addClass("has-error");
                        errormessage += "<p><i class=\"fa fa-exclamation\"></i> " +
                            $("#" + $(obj).data("target")).data("errormsg") +
                            "</p>";
                        formerror = true;
                    } else {
                        $("div." + key).removeClass("has-error").addClass("has-success");
                    }

                    break;
                case "username":
                    if (!application.form.verifyFunc.username(value)) {
                        if (!formerror) {
                            $focuselement = $(obj);
                        };
                        $("div." + key).addClass("has-error");
                        errormessage += "<p><i class=\"fa fa-exclamation\"></i> " + $(obj).data("errormsg") + "</p>";
                        formerror = true;
                    } else {
                        $("div." + key)
                            .removeClass("has-error")
                            .addClass("has-success");
                    }

                    break;
                case "money":
                    if (!application.form.verifyFunc.money(value)) {
                        if (!formerror) {
                            $focuselement = $(obj);
                        };
                        $("div." + key).addClass("has-error");
                        errormessage += "<p><i class=\"fa fa-exclamation\"></i> " + $(obj).data("errormsg") + "</p>";
                        formerror = true;
                    } else {
                        $("div." + key).removeClass("has-error").addClass("has-success");
                    }

                    break;
                default:
                    value = $(obj).data("element") === "ckeditor" ? CKEDITOR.instances["body"].getData() : value;
                    if (!application.form.verifyFunc.text(parseInt($(obj).data("minlength")), value)) {
                        if (!formerror) {
                            $focuselement = $(obj);
                        };
                        $("div." + key).addClass("has-error");
                        errormessage += "<p><i class=\"fa fa-exclamation\"></i> " + $(obj).data("errormsg") + "</p>";
                        formerror = true;
                    } else {
                        $("div." + key)
                            .removeClass("has-error")
                            .addClass("has-success");
                    }

                    break;
                }

                return {
                    error: formerror,
                    focus: $focuselement,
                    message: [errormessage]
                };
            },
            formatValues: function(obj) {
                var value = $(obj).data("element") === "multiple"
                    ? $(obj).val().toString()
                    : $(obj).data("element") === "ckeditor"
                    ? CKEDITOR.instances["body"].getData()
                    : $(obj).data("element") === "date"
                    ? $(obj).data("datepicker").getFormattedDate("yyyy-mm-dd")
                    : $(obj).data("element") === "phone"
                    ? $(obj).cleanVal()
                    : ($(obj).data("element") === "password" || $(obj).data("element") === "passwordAgain")
                    ? application.tools.cryptoPassword($(obj).val())
                    : $(obj).data("element") === "money"
                    ? $(obj).autoNumeric("get")
                    : $(obj).data("element") === "digit"
                    ? $(obj).autoNumeric("get")
                    : $(obj).val();

                return value;
            },
            verifyFunc: {
                email: function(item) {
                    var pattern = new RegExp(
                        /^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i
                    );
                    return pattern.test(item);
                },
                text: function(min, item) {
                    return item === undefined || item === null || item.length < min || item === "" ? false : true;
                },
                image: function() {
                    return images.length === 0 ? false : true;
                },
                url: function(item) {
                    var pattern =
                        new RegExp(/((http|https)\:\/\/)?[a-zA-Z0-9\.\/\?\:@\-_=#]+\.([a-zA-Z0-9\.\/\?\:@\-_=#])*/);
                    return pattern.test(item);
                },
                date: function(item) {
                    var d = moment(item, "D.M.YYYY");
                    if (d === null || !d.isValid() || d === "undefined") return false;

                    return item.indexOf(d.format("D.M.YYYY")) >= 0 ||
                        item.indexOf(d.format("DD.MM.YYYY")) >= 0 ||
                        item.indexOf(d.format("D.M.YY")) >= 0 ||
                        item.indexOf(d.format("DD.MM.YY")) >= 0;
                },
                password: function(password) {
                    var pattern =
                        new RegExp(
                            /^(?:(?=.*[a-z])(?:(?=.*[A-Z])(?=.*[\d\W])|(?=.*\W)(?=.*\d))|(?=.*\W)(?=.*[A-Z])(?=.*\d)).{8,}$/i);
                    return pattern.test(password);
                },
                passwordIsEqual: function(item1, item2) {
                    return item1 === item2 ? true : false;
                },
                username: function(item) {
                    var pattern = new RegExp(/^[-\w\.\$@\*\!]{1,30}$/i);
                    return pattern.test(item);
                },
                money: function(item) {
                    var pattern = new RegExp(/^\d+(?:\.\d{0,2})$/);
                    return pattern.test(item);
                },
                array: function(item) {
                    if (Array.isArray(item) && item.length > 0) {
                        return true;
                    } else {
                        return false;
                    }
                }
            }
        },
        listImages: {
            singleFileList: function(uploadFolder, uploadTitle) {
                if (images.length === 1) {
                    $(".uploadFile").toggleClass("hide");
                    $(".fileIsUploaded ").toggleClass("hide");
                    $(".showUploadedFile").html("<a href=\"/uploads/" +
                        uploadFolder +
                        "/" +
                        images[0] +
                        "\" title=\"" +
                        uploadTitle +
                        "\" data-fancybox=\"gallery\"><i class=\"fa fa-picture-o\"></i> Görseli Görüntüle</a>");
                }
            },
            galleryList: function(uploadFolder, uploadTitle) {

            },
            resetImages: function() {
                images = [];
                $(".uploadFile").toggleClass("hide");
                $(".fileIsUploaded ").toggleClass("hide");
                $(".showUploadedFile").html(null);
            }
        },
        plugins: {
            load: function() {
                $("body").append(
                    "<div id=\"goToUp\" class=\"btn btn-info\"><span class=\"fa fa-arrow-circle-up fa-2x\"></span></div>");

                $(window).scroll(function() {
                    if ($(this).scrollTop() !== 0) {
                        $("#goToUp").fadeIn();
                    } else {
                        $("#goToUp").fadeOut();
                    }
                });

                $("#goToUp").click(function() {
                    $("html, body").animate({ scrollTop: 0 }, "slow");
                    return false;
                });

                if ($(".maxlength").length >= 1) {
                    $(".maxlength").maxlength({
                        placement: "top",
                        alwaysShow: false,
                        threshold: 5,
                        warningClass: "label label-success",
                        limitReachedClass: "label label-danger",
                        separator: ". karakteri yazıyorsunuz. Bu alan en fazla",
                        preText: "",
                        postText: " karakter olabilir.",
                        validate: true
                    });
                }

                if ($("input.money").length >= 1) {
                    $("input.money").autoNumeric("init",
                        {
                            digitGroupSeparator: ".",
                            decimalCharacter: ","
                        });
                }

                if ($("input.digit").length >= 1) {
                    $("input.digit").autoNumeric("init",
                        {
                            mDec: 0
                        });
                }

                if ($("input.onlynumeric").length >= 1) {
                    $("input.onlynumeric").numeric();
                }

                if ($(".phonemask").length >= 1) {
                    $(".phonemask").mask("(000) 000-0000");
                }

                if ($("select.select2-multiple").length >= 1) {
                    $("select.select2-multiple").select2({
                        minimumResultsForSearch: 1,
                        tags: true,
                        language: "tr-TR"
                    });
                }

                if ($("select.select2-single").length >= 1) {
                    $("select.select2-single").select2({
                        language: "tr-TR"
                    });
                }

                if ($("input.autocomplete").length >= 1) {
                    $("input.autocomplete").on("focus",
                        function() {
                            var maxwidth = parseInt($(this).width()) + 26;
                            $(this).devbridgeAutocomplete({
                                serviceUrl: "/Admin/Api/Auto-Complete?tableName=" +
                                    $(this).data("tablename") +
                                    "&columnName=" +
                                    $(this).data("columnname"),
                                minChars: 2,
                                width: maxwidth,
                                formatResult: function(suggestion, currentValue) {
                                    return suggestion.value;
                                }
                            });
                        });
                }

                if ($("#form input[type=text]").length > 0) {
                    $("#form input[type=text]").prop("autocomplete", "off");
                }

                if ($(".datepicker").length >= 1) {
                    $(".datepicker")
                        .attr("placeholder", "__.__.____")
                        .datepicker({
                            format: "dd.mm.yyyy",
                            weekStart: 1,
                            language: "tr",
                            orientation: "bottom left",
                            todayHighlight: true,
                            autoclose: true
                        });
                };

                if ($("#form").find("[data-element='il']").length) {
                    $("#iladi").addClass("formelement");

                    $.when(
                        $.getJSON("/il-ilce.json", function(v1) {})
                    ).done(function(veri) {
                        var illerilceler = veri.illerilceler,
                            il = $("#il"),
                            ilce = $("#ilce");

                        il.empty();

                        $.each(illerilceler,
                            function(index, item) {
                                if (!il.find("option").filter("[value='" + item.plaka + "']").length) {
                                    il.append($("<option></option>").attr("value", item.plaka).text(item.il));
                                }
                            });

                        il.change(function() {
                            var plaka = $(this).val(),
                                ilceler = illerilceler.filter(function(item) {
                                    return item.plaka === plaka;
                                }).sort(function(o1, o2) {
                                    return o1.ilce > o2.ilce ? 1 : o1.ilce < o2.ilce ? -1 : 0;
                                });

                            $("#iladi").val(il.find("option").filter("[value='" + plaka + "']").text());

                            ilce.empty();

                            $.each(ilceler,
                                function(index, item) {
                                    ilce.append($("<option></option>").attr("value", item.ilce).text(item.ilce));
                                });
                        });

                        setTimeout(function() {
                                il.val(35).change();
                            },
                            100);
                    });
                }

                if ($("#fileupload").length >= 1) {
                    $("#fileupload")
                        .fileupload({
                            dataType: "json",
                            disableImageResize: /Android(?!.*Chrome)|Opera/
                                .test(window.navigator && navigator.userAgent),
                            imageMaxWidth: uploadParameters.imageMaxWidth,
                            imageMaxHeight: uploadParameters.imageMaxHeight,
                            imageCrop: uploadParameters.imageCrop
                        })
                        .bind("fileuploaddestroyed",
                            function(e, data) {
                                var fileName = data.context.find("a[download]").attr("download"),
                                    index = images.indexOf(fileName);

                                if (index !== -1) images.splice(index, 1);
                            })
                        .bind("fileuploaddone",
                            function(e, data) {
                                $.each(data.result.files,
                                    function(e, file) {
                                        var index = images.indexOf(file.name);
                                        if (index === -1) images.push(file.name);

                                        if (uploadParameters.singleFile) {
                                            application.listImages.singleFileList(uploadParameters.uploadFolder,
                                                uploadParameters.uploadTitle);
                                        } else {
                                            application.listImages.galleryList(uploadParameters.uploadFolder,
                                                uploadParameters.uploadTitle);
                                        }
                                    });
                            });
                }

                String.prototype.formatUnicorn = String.prototype.formatUnicorn ||
                    function () {
                        var str = this.toString();
                        if (arguments.length) {
                            var t = typeof arguments[0];
                            var key;
                            var args = ("string" === t || "number" === t) ?
                                Array.prototype.slice.call(arguments)
                                : arguments[0];

                            for (key in args) {
                                str = str.replace(new RegExp("\\{" + key + "\\}", "gi"), args[key]);
                            }
                        }

                        return str;
                    };
            }
        }
    };

$(document).ready(function() {
    application.plugins.load();
});