/**
 * Create Order with JWT API
 * Uses JWT authentication exclusively
 */

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
            const areaValue = $("#" + id + " #areaFilter").val();
            
            // Use JWT API
            window.apiClient.getCarriersByArea(areaValue)
                .then(d => {
                    let row = "";
                    $("#" + id + " select[id*=CompanyId]").empty();
                    $.each(d, function (i, v) {
                        row += "<option value=" + v.value + ">" + v.text + "</option>";
                    });
                    $("#" + id + " select[id*=CompanyId]").html(row);
                    RefreshDropdowns();
                    $(tc).find('.selectpicker').selectpicker('refresh');
                })
                .catch(err => {
                    console.error('Error loading carriers:', err);
                    if (typeof notyf !== 'undefined') {
                        notyf.error('Error loading carriers');
                    }
                });
        });

        $("#" + id + " select[id*=CompanyId]").change(function () {
            RefreshDropdowns();
        });

        $('#form-modal').on('submit', function () {
            RefreshDropdowns();
        });
    });

    function RefreshDropdowns() {
        const selectedCompanyId = $("#" + id + " select[id*=CompanyId]").val();

        // Load contacts via JWT API
        window.apiClient.getOrderContacts(selectedCompanyId)
            .then(d => {
                let row = "";
                addContactBtn.style.display = "";
                $("#" + id + " #contactTo").empty();
                $.each(d, function (i, v) {
                    row += "<option value=" + v.value + ">" + v.text + "</option>";
                });
                $("#" + id + " #contactTo").html(row);
                let item = new Option("Select", '', true, true);
                $(item).html("Select");
                $("#" + id + " #contactTo").append(item);
            })
            .catch(err => console.error('Error loading contacts:', err));

        // Load drivers via JWT API
        window.apiClient.getOrderDrivers(selectedCompanyId)
            .then(d => {
                let row = "";
                addDriverBtn.style.display = "";
                $("#" + id + " #driver").empty();
                $.each(d, function (i, v) {
                    row += "<option value=" + v.value + ">" + v.text + "</option>";
                });
                $("#" + id + " #driver").html(row);
                let item = new Option("Select", '', true, true);
                $(item).html("Select");
                $("#" + id + " #driver").append(item);
            })
            .catch(err => console.error('Error loading drivers:', err));

        // Load vehicles via JWT API
        window.apiClient.getOrderVehicles(selectedCompanyId)
            .then(d => {
                let row = "";
                addVehicleBtn.style.display = "";
                $("#" + id + " #vehicle").empty();
                $.each(d, function (i, v) {
                    row += "<option value=" + v.value + ">" + v.text + "</option>";
                });
                $("#" + id + " #vehicle").html(row);
                let item = new Option("Select", '', true, true);
                $(item).html("Select");
                $("#" + id + " #vehicle").append(item);
            })
            .catch(err => console.error('Error loading vehicles:', err));

        // Load trailers via JWT API
        window.apiClient.getOrderTrailers(selectedCompanyId)
            .then(d => {
                let row = "";
                addTrailerBtn.style.display = "";
                $("#" + id + " #trailer").empty();
                $.each(d, function (i, v) {
                    row += "<option value=" + v.value + ">" + v.text + "</option>";
                });
                $("#" + id + " #trailer").html(row);
                let item = new Option("Select", '', true, true);
                $(item).html("Select");
                $("#" + id + " #trailer").append(item);
            })
            .catch(err => console.error('Error loading trailers:', err));
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