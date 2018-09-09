$(function() {
    var table = $("#table")
        .removeClass("display")
        .addClass("table table-striped table-bordered")
        .dataTable({
            dom: "frtip",
            processing: true,
            iDisplayLength: 25,
            serverSide: true,
            autoWidth: false,
            paginate: true,
            aLengthMenu: [[5, 10, 25, 50, -1], [5, 10, 25, 50, "Hepsi"]],
            ajax: sAjaxSource,
            columns: [
                {
                    name: "id",
                    data: "id",
                    visible: false,
                    searchable: false,
                    sortable: false,
                },
                {
                    data: "title",
                    name: "title",
                    title: "Görev Tanımı",
                    searchable: true,
                    sortable: false
                },
                {
                    data: "email",
                    name: "email",
                    title: "Eposta",
                    searchable: true,
                    sortable: false
                },
                {
                    data: "firstName",
                    name: "firstName",
                    title: "Adı",
                    searchable: true,
                    sortable: false
                },
                {
                    data: "lastName",
                    name: "lastName",
                    title: "Soyadı",
                    searchable: true,
                    sortable: false
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