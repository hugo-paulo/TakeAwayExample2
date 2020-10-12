let dataTable;

$(document).ready(function () {
    loadList();
});

function loadList() {
    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "/api/user",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "fullName", "width": "25%" },
            { "data": "email", "width": "25%" },
            { "data": "phoneNumber", "width": "25%" },
            {
                "data": { id: "id", lockoutEnd: "lockoutEnd" }, //Because we have more  than one value, we use anonymous object
                "render": function (data) {

                    let today = Date.now();
                    let lockout = new Date(data.lockoutEnd).getTime();

                    //True == user is locked out
                    if (lockout > today) {
                        return `<div class="text-center">                                    
                                    <i class="fa fa-toggle-on fa-2x" style="cursor: pointer;" onclick=LockUnlock('${data.id}')></i>
                                    <p>Unlock</p>
                                </div>`;
                    }
                    else {
                        return `<div class="text-center">
                                <i class="fa fa-toggle-off fa-2x" style="cursor: pointer;" onclick=LockUnlock('${data.id}')></i>    
                                <p>Lock</p>                                    
                                </div>`;
                    }
                }, "width": "30%"
            }
        ],
        "language": {
            "emptyTable": "no data found."
        },
        "width": "100%"
    });
}

function LockUnlock(id) {

    $.ajax({
        type: "POST",
        url: '/api/User',
        data: JSON.stringify(id),
        contentType: "application/json", //Note this is not a path its type like name says
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