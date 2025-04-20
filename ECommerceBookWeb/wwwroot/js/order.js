var dataTable;
$(document).ready(function () {
    var status = window.location.search.split('=')[1];
    loadDataTable(status);
});

function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            url: '/admin/order/getall?status='+status,
            dataSrc: 'data'
        },
        "columns": [
            { data: 'id', "width": "5%" },
            { data: 'name', "width": "10%" },
            { data: 'phoneNumber', "width": "15%" },
            { data: 'applicationUser.email', "width": "15%" },
            { data: 'orderStatus', "width": "10%" },
            { data: 'orderTotal', "width": "8%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-100 btn-group" role="group" >
                                 <a href="/admin/order/details?orderHeaderId=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i></a>
                            </div>`;
                }, "width": "10%"
            }
        ]
    }); }
