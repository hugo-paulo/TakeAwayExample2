﻿var dataTable;

$(document).ready(function () {
    loadList();
});

function loadList() {
    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "/api/category",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "categoryName", "width": "40%" },
            { "data": "displayOrder", "width": "30%" },
            {
                "data": "categoryID",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/Admin/category/upsert?id=${data}" class="btn btn-success text-white" style="cursor: pointer; width: 100px;">
                                    <i class="far fa-edit"></i> Edit
                                </a>
                                <a class="btn btn-danger text-white" style="cursor: pointer; width: 100px;" onclick=Delete('/api/category/'+${data})>
                                    <i class="far fa-trash-alt"></i> Delete
                                </a>
                            </div>`;
                }, "width": "30%"
            }
        ],
        "language": {
            "emptyTable": "no data found."
        },
        "width": "100%"
    });
}

function Delete(url) {

    //This is a sweet alert (sweet alert api)
    swal({
        title: "Delete",
        text: "Are you sure you want to delete the entry",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}