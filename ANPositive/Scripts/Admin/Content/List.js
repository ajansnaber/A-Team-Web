$(function() {
    var table = $("#table")
        .removeClass("display")
        .addClass("table table-striped table-bordered")
        .dataTable({
            dom: "frtip",
            rowReorder: {
                update: false
            },
            processing: true,
            iDisplayLength: 25,
            serverSide: true,
            autoWidth: false,
            paginate: true,
            aLengthMenu: [[5, 10, 25, 50, -1], [5, 10, 25, 50, "Hepsi"]],
            ajax: sAjaxSource,
            columns: [
                {
                    name: "displayOrder",
                    data: "displayOrder",
                    searchable: false,
                    sortable: true,
                    title: "#",
                    width: "5%",
                    class: "text-center",
                    orderable: true,
                    mRender: function(data, type, row) {
                        return "<i class=\"fa fa-bars\" aria-hidden=\"true\"></i>";
                    }
                },
                {
                    data: "title",
                    name: "title",
                    title: "Sayfa Başlığı",
                    searchable: true,
                    sortable: false,
                    mRender: function(data, type, row) {
                        return row.isDeleted
                            ? "<del class=\"text-danger\">" + row.title + "</del>"
                            : row.title;
                    }
                },
                {
                    data: "menuPosition",
                    name: "menuPosition",
                    title: "Menü",
                    width: "12%",
                    class: "text-center",
                    searchable: true,
                    sortable: false,
                    mRender: function(data, type, row) {
                        var html = null;
                        if (row.menuPosition === 0) {
                            html = "<span class=\"label-danger label\">Üst Menü</span>";
                        } else {
                            html = "<span class=\"label-success label\">Alt Menü</span>";
                        }

                        return html;
                    }
                },
                {
                    data: "language",
                    name: "language",
                    title: "Dil",
                    width: "12%",
                    class: "text-center",
                    searchable: true,
                    sortable: false,
                    mRender: function(data, type, row) {
                        var html = null;
                        if (row.language === 1) {
                            html = "<span class=\"label-primary label\">Türkçe</span>";
                        } else {
                            html = "<span class=\"label-warning label\">İngilizce</span>";
                        }

                        return html;
                    }
                },
                {
                    data: "published",
                    name: "published",
                    class: "text-center",
                    title: "Durum",
                    sortable: false,
                    searchable: false,
                    width: "12%",
                    mRender: function(data, type, row) {
                        return row.published
                            ? "<i class=\"fa fa-check true-icon\"></i>"
                            : "<i class=\"fa fa-close false-icon\"></i>";
                    }
                },
                {
                    data: null,
                    name: null,
                    class: "text-center",
                    title: "Düzenle",
                    searchable: false,
                    sortable: false,
                    width: "12%",
                    mRender: function(data, type, row) {
                        return "<a href=\"" +
                            editLink +
                            "/" +
                            row.id +
                            "\" class=\"btn btn-default btn-xs\" type=\"button\"><i class=\"fa fa-pencil\"></i>&nbsp;&nbsp;Düzenle</a>";
                    }
                }
            ],
            oLanguage: {
                sProcessing: "",
                sLengthMenu: "Sayfada _MENU_ Kayıt Göster",
                sZeroRecords: "<div style=\"text-align:center; color:#2494f2;\">Kayıt Bulunamadı!</div>",
                sInfo: "  _TOTAL_ Kayıttan _START_ - _END_ Arası Kayıtlar",
                sInfoEmpty: "Kayıt Yok",
                sInfoFiltered: "( _MAX_ Kayıt İçerisinden Bulunan)",
                sInfoPostFix: "",
                sSearch: "Bul: ",
                sUrl: "",
                oPaginate: {
                    sFirst: "İlk",
                    sPrevious: "Önceki",
                    sNext: "Sonraki",
                    sLast: "Son"
                }
            },
            initComplete: function(settings, json) {
                $("#form").find("*").filter(":input:visible:first").focus();

                $(this).on("row-reorder.dt",
                    function(dragEvent, data, nodes) {
                        var tableData = table.fnGetData(),
                            displayOrders = [];
                        $.each(data,
                            function(index, item) {
                                while (item.newPosition > displayOrders.length) {
                                    displayOrders.push({
                                        id: tableData[displayOrders.length].id,
                                        displayOrder: displayOrders.length
                                    });
                                }

                                displayOrders.push({
                                    id: tableData[item.oldPosition].id,
                                    displayOrder: item.newPosition
                                });
                            });

                        while (tableData.length > displayOrders.length) {
                            displayOrders.push({
                                id: tableData[displayOrders.length].id,
                                displayOrder: displayOrders.length
                            });
                        }

                        if (displayOrders.length >= 1) {
                            application.form.sendAjax(displayOrders, updateLink, false, null);
                        }
                    });
            }
        });
});