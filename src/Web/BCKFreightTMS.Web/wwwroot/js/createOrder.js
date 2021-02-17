let companyId = document.getElementById("CompanyToId");
let addContactBtn = document.getElementById("addContact");
let addDriverBtn = document.getElementById("addDriver");
let addVehicleBtn = document.getElementById("addVehicle");
let addTrailerBtn = document.getElementById("addTrailer");
let radios = document.getElementById("radios");
let trailerSelectEl = $("#trailerSelect");

radios.addEventListener("change", function (ev) {
    if (ev.target.id == "radSolo") {
        trailerSelectEl.find("select").val("");
        trailerSelectEl.hide();
    }
    else {
        $("#trailerSelect").show();
    }
});
addVehicleBtn.addEventListener("click", function (ev) {
    let typeEl = $("input:radio:checked");
    let action = typeEl.data().action;
    let title = typeEl.data().modal_title;

    showInPopup('/Vehicles/' + action + '/' + companyId.value, title);
});
addTrailerBtn.addEventListener("click", function (ev) {
    showInPopup('/Vehicles/AddTrailerModal/' + companyId.value, ev.target.parentNode.dataset.modal_title);
});
addContactBtn.addEventListener("click", function (ev) {
    showInPopup(`/Contacts/AddPersonModal/${companyId.value}?role=Contact`, 'Add contact');
});
addDriverBtn.addEventListener("click", function (ev) {
    showInPopup(`/Contacts/AddPersonModal/${companyId.value}?role=Driver`, 'Add driver');
});

$(function () { $('.selectpicker').selectpicker(); });

$(function () {
    $("#areaFilter").change(function () {
        $.getJSON("/Orders/GetCarriersByArea", { area: $("#areaFilter").val() }, function (d) {
            let row = "";
            $("#CompanyToId").empty();
            $.each(d, function (i, v) {
                row += "<option value=" + v.value + ">" + v.text + "</option>";
            });
            $("#CompanyToId").html(row);
            RefreshDropdowns();
            $('.selectpicker').selectpicker('refresh');
        })
    })
    $("#CompanyToId").change(function () {
        RefreshDropdowns();
    });
    $('#form-modal').on('hidden.bs.modal', function (ev) {
        RefreshDropdowns();
    });
    function RefreshDropdowns() {
        $.getJSON("/Orders/GetContacts", { companyId: $("#CompanyToId").val() }, function (d) {
            let row = "";
            addContactBtn.style.display = "";
            $("#contactTo").empty();
            $.each(d, function (i, v) {
                row += "<option value=" + v.value + ">" + v.text + "</option>";
            });
            $("#contactTo").html(row);
            let item = new Option("Select", '', true, true);
            $(item).html("Select");
            //item.setAttribute("disabled", "disabled");
            $("#contactTo").append(item);
        })
        $.getJSON("/Orders/GetDrivers", { companyId: $("#CompanyToId").val() }, function (d) {
            let row = "";
            addDriverBtn.style.display = "";
            $("#driver").empty();
            $.each(d, function (i, v) {
                row += "<option value=" + v.value + ">" + v.text + "</option>";
            });
            $("#driver").html(row);
            let item = new Option("Select", '', true, true);
            $(item).html("Select");
            //item.setAttribute("disabled", "disabled");
            $("#driver").append(item);
        })
        $.getJSON("/Orders/GetVehicles", { companyId: $("#CompanyToId").val() }, function (d) {
            let row = "";
            addVehicleBtn.style.display = "";
            $("#vehicle").empty();
            $.each(d, function (i, v) {
                row += "<option value=" + v.value + ">" + v.text + "</option>";
            });
            $("#vehicle").html(row);
            let item = new Option("Select", '', true, true);
            $(item).html("Select");
            //item.setAttribute("disabled", "disabled");
            $("#vehicle").append(item);
        })
        $.getJSON("/Orders/GetTrailers", { companyId: $("#CompanyToId").val() }, function (d) {
            let row = "";
            addTrailerBtn.style.display = "";
            $("#trailer").empty();
            $.each(d, function (i, v) {
                row += "<option value=" + v.value + ">" + v.text + "</option>";
            });
            $("#trailer").html(row);
            let item = new Option("Select", '', true, true);
            $(item).html("Select");
            //item.setAttribute("disabled", "disabled");
            $("#trailer").append(item);
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