$(function () {
    $("#areaFilter").change(function () {
        $.getJSON("/Orders/GetCarriersByArea", { area: $("#areaFilter").val() }, function (d) {
            var row = "";
            $("#CompanyToId").empty();
            $.each(d, function (i, v) {
                row += "<option value=" + v.value + ">" + v.text + "</option>";
            });
            $("#CompanyToId").html(row);
            RefreshDropdowns();
        })
    })
    $("#CompanyToId").change(function () {
        RefreshDropdowns();
    });
    $('#form-modal').on('hidden.bs.modal', function () {
        RefreshDropdowns();
    });
    function RefreshDropdowns() {
        $.getJSON("/Orders/GetContacts", { companyId: $("#CompanyToId").val() }, function (d) {
            var row = "";
            $("#contactTo").empty();
            $.each(d, function (i, v) {
                row += "<option value=" + v.value + ">" + v.text + "</option>";
            });
            $("#contactTo").html(row);
            var item = new Option("Select", null, true, true);
            $(item).html("Select");
            item.setAttribute("disabled", "disabled");
            $("#contactTo").append(item);
        })
        $.getJSON("/Orders/GetDrivers", { companyId: $("#CompanyToId").val() }, function (d) {
            var row = "";
            $("#driver").empty();
            $.each(d, function (i, v) {
                row += "<option value=" + v.value + ">" + v.text + "</option>";
            });
            $("#driver").html(row);
            var item = new Option("Select", null, true, true);
            $(item).html("Select");
            item.setAttribute("disabled", "disabled");
            $("#driver").append(item);
        })
        $.getJSON("/Orders/GetVehicles", { companyId: $("#CompanyToId").val() }, function (d) {
            var row = "";
            $("#vehicle").empty();
            $.each(d, function (i, v) {
                row += "<option value=" + v.value + ">" + v.text + "</option>";
            });
            $("#vehicle").html(row);
            var item = new Option("Select", null, true, true);
            $(item).html("Select");
            item.setAttribute("disabled", "disabled");
            $("#vehicle").append(item);
        })
    }
})
$(function () {
    $('#submit').on('click', function (evt) {
        evt.preventDefault();
        $.get('/Contacts/GetCompany', $('#search').serialize()).done(function (data) {
            $.each($('#company_input input'), function () {
                let index = $("#company_input input").index(this);
                let input = this.getAttribute('name');
                console.log(input);
                $('#company_input input').eq(index).val(data[Object.keys(data)
                    .find(k => k.toLowerCase() === input.toLowerCase())]);
            });
        });
    });
});