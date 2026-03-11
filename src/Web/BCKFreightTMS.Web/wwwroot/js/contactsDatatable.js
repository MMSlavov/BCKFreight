/**
 * Contacts DataTable with JWT API
 * Uses JWT authentication exclusively
 */

// Initialize DataTable with JWT API
$(document).ready(function () {
    $("#contactsDatatable").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/api/contacts/datatable",
            "type": "POST",
            "datatype": "json",
            "beforeSend": function(xhr) {
                const authHeader = window.jwtManager.getAuthHeader();
                if (authHeader) {
                    xhr.setRequestHeader('Authorization', authHeader);
                } else {
                    console.error('No JWT token found. Please login.');
                }
            },
            "error": function(xhr, error, code) {
                if (xhr.status === 401) {
                    console.error('Unauthorized - JWT token expired or invalid');
                    window.apiClient.handleUnauthorized();
                } else {
                    console.error('DataTable error:', error);
                }
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
                    return "<a href = '#' onclick = showInPopup('/Contacts/Edit" + row.type + "Modal/" + row.id + "','Edit') title='Edit'><i class='fas fa-edit'></i></a><a href = '#' onclick = showInPopup('/Contacts/Details/" + row.id + "','Details') title = 'Details' ><span><i class='p-1 fas fa-info-circle'></i></span></a ><a href='Contacts/Delete?id=" + row.id + "' title='Delete'><i class='p-1 fas fa-trash-alt'></i></a>";
                }, 
                className: "text-center justify-content-around"
            },
        ]
    });
});
