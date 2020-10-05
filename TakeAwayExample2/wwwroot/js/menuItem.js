let dataTable;

$(document).ready(() => loadList());

//The price is rendered with two decimal on the datatable because of line 18S
function loadList() {
    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "/api/menuitem",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "menuItemName", "width": "25%" },
            {
                "data": "menuItemPrice",
                "render": (data) => {
                    return "$" + (Math.round(data * 100) / 100).toFixed(2);
                },
                "width": "15%"
            },
            { "data": "category.categoryName", "width": "15%" },
            { "data": "foodType.foodTypeName", "width": "15%" },
            {
                "data": "menuItemID",
                "render": (data) => {
                    return `<div class="text-center">
                                <a href="/Admin/menuitem/upsert?id=${data}" class="btn btn-success text-white" style="cursor: pointer; width: 100px;">
                                    <i class="far fa-edit"></i> Edit
                                </a>
                                <a class="btn btn-danger text-white" style="cursor: pointer; width: 100px;" onclick=Delete('/api/menuitem/'+${data})>
                                    <i class="far fa-trash-alt"></i> Delete
                                </a>
                              </div>`
                }, "width": "30%"
            }
        ],
        "language": {
            "emptyTable": "no data found."
        },
        "width": "100%",
        "order": [[2, "asc"]]
    });
}
//Note the order is for default ordering by column, the column identity are 0 based array (first column is 0)

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