let dataTable;

$(document).ready(() => loadList());

function loadList() {
    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "/api/foodtype",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "foodTypeName", "width": "40%" },
            {
                "data": "foodTypeID",
                "render": (data) => {
                    return `<div class="text-center">
                                <a href="/Admin/foodtype/upsert?id=${data}" class="btn btn-success text-white" style="cursor: pointer; width: 100px;">
                                    <i class="far fa-edit"></i> Edit
                                </a>
                                <a class="btn btn-danger text-white" style="cursor: pointer; width: 100px;" onclick=Delete('/api/foodtype/'+${data})>
                                    <i class="far fa-trash-alt"></i> Delete
                                </a>
                              </div>`
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