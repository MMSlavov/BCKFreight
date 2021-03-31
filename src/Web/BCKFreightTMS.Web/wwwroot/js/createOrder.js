//let currentCId = document.querySelector("a.tablinks.active-tab").id;
//let currentCourse = document.querySelector(".tabcontent[id=" + currentCId + "]");
//let companyId = currentCourse.querySelector("select[id*=CompanyId]");

[...document.querySelectorAll(".tabcontent[id*=course]")].forEach(tc => {
    let id = tc.id;
    let companyId = tc.querySelector("select[id*=CompanyId]");
    let addContactBtn = tc.querySelector(".addContact");
    let addDriverBtn = tc.querySelector(".addDriver");
    let addVehicleBtn = tc.querySelector(".addVehicle");
    let addTrailerBtn = tc.querySelector(".addTrailer");
    let radios = tc.querySelector(".radios");
    let trailerSelectEl = $("#" + id + " #trailerSelect");

    radios.addEventListener("change", function (ev) {
        if (ev.target.id == "radSolo") {
            trailerSelectEl.find("select").val("");
            trailerSelectEl.hide();
        }
        else {
            trailerSelectEl.show();
        }
    });
    addVehicleBtn.addEventListener("click", function (ev) {
        let typeEl = $(tc).find("input:radio:checked");
        let action = typeEl.data().action;
        let title = typeEl.data().modal_title;

        showInPopup('/Vehicles/' + action + '/' + companyId.value, title, null, RefreshDropdowns);
    });
    addTrailerBtn.addEventListener("click", function (ev) {
        showInPopup('/Vehicles/AddTrailerModal/' + companyId.value, ev.target.parentNode.dataset.modal_title, null, RefreshDropdowns);
    });
    addContactBtn.addEventListener("click", function (ev) {
        showInPopup(`/Contacts/AddPersonModal/${companyId.value}?role=Contact`, 'Add contact', null, RefreshDropdowns);
    });
    addDriverBtn.addEventListener("click", function (ev) {
        showInPopup(`/Contacts/AddPersonModal/${companyId.value}?role=Driver`, 'Add driver', null, RefreshDropdowns);
    });

    $(function () { $(tc).find('.selectpicker').selectpicker(); });

    $(function () {
        $("#" + id + " #areaFilter").change(function () {
            $.getJSON("/Orders/GetCarriersByArea", { area: $("#" + id + " #areaFilter").val() }, function (d) {
                let row = "";
                $("#" + id + " select[id*=CompanyId]").empty();
                $.each(d, function (i, v) {
                    row += "<option value=" + v.value + ">" + v.text + "</option>";
                });
                $("#" + id + " select[id*=CompanyId]").html(row);
                RefreshDropdowns();
                $(tc).find('.selectpicker').selectpicker('refresh');
            })
        })
        $("#" + id + " select[id*=CompanyId]").change(function () {
            RefreshDropdowns();
        });
        $('#form-modal').on('submit', function () {
            RefreshDropdowns();
        });
    })

    function RefreshDropdowns() {
        $.getJSON("/Orders/GetContacts", { companyId: $("#" + id + " select[id*=CompanyId]").val() }, function (d) {
            let row = "";
            addContactBtn.style.display = "";
            $("#" + id + " #contactTo").empty();
            $.each(d, function (i, v) {
                row += "<option value=" + v.value + ">" + v.text + "</option>";
            });
            $("#" + id + " #contactTo").html(row);
            let item = new Option("Select", '', true, true);
            $(item).html("Select");
            //item.setAttribute("disabled", "disabled");
            $("#" + id + " #contactTo").append(item);
        })
        $.getJSON("/Orders/GetDrivers", { companyId: $("#" + id + " select[id*=CompanyId]").val() }, function (d) {
            let row = "";
            addDriverBtn.style.display = "";
            $("#" + id + " #driver").empty();
            $.each(d, function (i, v) {
                row += "<option value=" + v.value + ">" + v.text + "</option>";
            });
            $("#" + id + " #driver").html(row);
            let item = new Option("Select", '', true, true);
            $(item).html("Select");
            //item.setAttribute("disabled", "disabled");
            $("#" + id + " #driver").append(item);
        })
        $.getJSON("/Orders/GetVehicles", { companyId: $("#" + id + " select[id*=CompanyId]").val() }, function (d) {
            let row = "";
            addVehicleBtn.style.display = "";
            $("#" + id + " #vehicle").empty();
            $.each(d, function (i, v) {
                row += "<option value=" + v.value + ">" + v.text + "</option>";
            });
            $("#" + id + " #vehicle").html(row);
            let item = new Option("Select", '', true, true);
            $(item).html("Select");
            //item.setAttribute("disabled", "disabled");
            $("#" + id + " #vehicle").append(item);
        })
        $.getJSON("/Orders/GetTrailers", { companyId: $("#" + id + " select[id*=CompanyId]").val() }, function (d) {
            let row = "";
            addTrailerBtn.style.display = "";
            $("#" + id + " #trailer").empty();
            $.each(d, function (i, v) {
                row += "<option value=" + v.value + ">" + v.text + "</option>";
            });
            $("#" + id + " #trailer").html(row);
            let item = new Option("Select", '', true, true);
            $(item).html("Select");
            //item.setAttribute("disabled", "disabled");
            $("#" + id + " #trailer").append(item);
        })
    }
});

document.getElementById("tabs")
    .addEventListener("click", (ev) => {
        openCourse(ev.target.id);
    });

function openCourse(id) {
    let tabcontent = document.querySelectorAll(".tabcontent[id*=course]");
    for (const t of tabcontent) {
        t.style.display = "none";
    }
    [...document.querySelectorAll(".tablinks[id*=course]")].forEach((t) => {
        t.className = t.className.replace(" active-tab", "");
    });
    document.querySelector(".tabcontent[id=" + id + "]").style.display = "block";
    document.querySelector(".tablinks[id=" + id + "]").className += " active-tab";
}