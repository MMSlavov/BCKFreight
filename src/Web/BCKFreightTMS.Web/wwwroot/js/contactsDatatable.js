var form = $('#__AjaxAntiForgeryForm');
var token = $('input[name="__RequestVerificationToken"]', form).val();
$.ajaxSetup({
    headers: {
        'RequestVerificationToken': token
            }
        });
$(document).ready(function () {
    $("#contactsDatatable").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/Contacts/GetContacts",
            "type": "POST",
            "datatype": "json",
            "headers": {
                'RequestVerificationToken': token
            }
        },
        "columnDefs": [{
            "targets": [0],
            "visible": false,
            "searchable": false
        }],
        "columns": [
            { "data": "id", "name": "Id", "autoWidth": true },
            { "data": "name", "name": "Name", "autoWidth": true },
            { "data": "type", "name": "Type", "autoWidth": true },
            { "data": "contacts", "name": "Contacts", "autoWidth": true },
            { "data": "address", "name": "Address", "autoWidth": true },
            {
                "render": function (data, type, row) {
                    //<a href='Contacts/Edit?id=" + row.id + "' title='Edit'><i class='pr-3 fas fa-edit'></i></a>
                    return "<a href = '#' onclick = showInPopup('Contacts/Details/"+ row.id +"','Details') title = 'Details' ><span><i class='p-1 fas fa-info-circle'></i></span></a ><a href='Contacts/Delete?id=" + row.id + "' title='Delete'><i class='pl-3 fas fa-trash-alt'></i></a>";
                }
            },
        ]
    });
});  
