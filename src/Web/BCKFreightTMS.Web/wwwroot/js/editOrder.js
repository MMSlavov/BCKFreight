﻿$(document).ready(function () {
    $('.summernote').summernote({
        tabsize: 2,
        height: 100,
        toolbar: [
            ['font', ['bold', 'underline', 'clear']],
            ['color', ['color']],
            ['para', ['ul', 'ol', 'paragraph']],
            ['table', ['table']],
            ['insert', ['link', 'picture', 'video']]
        ]
    });
});

[...document.querySelectorAll(".tabcontent[id*=course]")].forEach(tc => {
    let companyId = tc.querySelector("select[id*=CompanyId]");
    let addContactBtn = tc.querySelector("#addContact");
    let addDriverBtn = tc.querySelector("#addDriver");
    let addVehicleBtn = tc.querySelector("#addVehicle");
    let addCarrierBtn = tc.querySelector("#addCarrier");

    addContactBtn.addEventListener("click", function (ev) {
        showInPopup(`/Contacts/AddPersonModal/${companyId.value}?role=Contact`, 'Add contact', null, () => { GetData(companyId.value) });
    });
    addDriverBtn.addEventListener("click", function (ev) {
        showInPopup(`/Contacts/AddPersonModal/${companyId.value}?role=Driver`, 'Add driver', null, () => { GetData(companyId.value) });
    });
    addVehicleBtn.addEventListener("click", function (ev) {
        showInPopup(`/Vehicles/AddVehicleModal`, 'Add vehicle', null, () => { GetData(companyId.value) });
    });
    addCarrierBtn.addEventListener("click", function (ev) {
        showInPopup(`/Contacts/AddCompanyModal/`, 'Add carrier', null, () => { GetData(companyId.value) });
    });

    $(function () { $('.selectpicker').selectpicker(); });

    tc.querySelector("select[id*=CompanyId]").addEventListener("change", onChange)
    function onChange(ev) {
        GetData(ev.target.value);
    }

    function GetData(id) {
        $.getJSON("/Orders/GetContacts", { companyId: id }, function (d) {
            let row = "";
            $(tc).find("#contactTo").empty();
            $.each(d, function (i, v) {
                row += "<option value=" + v.value + ">" + v.text + "</option>";
            });
            $(tc).find("#contactTo").html(row);
            let item = new Option("Select", null, true, true);
            $(item).html("Select");
            item.setAttribute("disabled", "disabled");
            $(tc).find("#contactTo").append(item);
        })
        $.getJSON("/Orders/GetDrivers", { companyId: id }, function (d) {
            let row = "";
            $(tc).find("#driver").empty();
            $.each(d, function (i, v) {
                row += "<option value=" + v.value + ">" + v.text + "</option>";
            });
            $(tc).find("#driver").html(row);
            let item = new Option("Select", null, true, true);
            $(tc).find(item).html("Select");
            item.setAttribute("disabled", "disabled");
            $(tc).find("#driver").append(item);
        })
        $.getJSON("/Orders/GetVehicles", { companyId: id }, function (d) {
            let row = "";
            $(tc).find("#vehicle").empty();
            $.each(d, function (i, v) {
                row += "<option value=" + v.value + ">" + v.text + "</option>";
            });
            $(tc).find("#vehicle").html(row);
            let item = new Option("Select", null, true, true);
            $(item).html("Select");
            item.setAttribute("disabled", "disabled");
            $(tc).find("#vehicle").append(item);
        })
    }

    tc.querySelector("#aTabs")
        .addEventListener("click", (ev) => {
            if (ev.target && ev.target.classList.contains("tablinks")) {
                open(ev.target.id);
            }
            else if (ev.target && ev.target.id.includes("add")) {
                addAction(ev, ev.target.id.substring(3).toLowerCase());
            }
        });

    $(tc).find("#actions").on("click", ".delete", function (e) {
        e.preventDefault();
        open(this.parentNode.id.slice(0, -1));
        $(tc).find("#aTabs a[id=" + this.parentNode.id + "]").remove();
        $(this).parent('div').remove();
        [...tc.querySelectorAll(".tabcontent")].forEach((t, i) => {
            t.innerHTML = t.innerHTML.replace(/_\d_/g, "_" + i + "_").replace(/\[\d\]/g, "[" + i + "]");
        });
        tc.querySelectorAll(".tabcontent[id^=loading]").forEach((t, i) => {
            if (i != 0) {
                t.id = `loading${i + 1}`;
            }
        });
        tc.querySelectorAll(".tabcontent[id^=unloading]").forEach((t, i) => {
            if (i != 0) {
                t.id = `unloading${i + 1}`;
            }
        });
        tc.querySelectorAll(".tablinks[id^=loading]").forEach((t, i) => {
            if (i != 0) {
                t.id = `loading${i + 1}`;
                t.textContent = `#${i + 1}`;
            }
        });
        tc.querySelectorAll(".tablinks[id^=unloading]").forEach((t, i) => {
            if (i != 0) {
                t.id = `unloading${i + 1}`;
                t.textContent = `#${i + 1}`;
            }
        });
    });

    function addAction(evt, type) {
        let index = tc.querySelectorAll(".tabcontent").length;
        let action = tc.querySelector(".tabcontent[id=" + type + "]").cloneNode(true);
        [...action.querySelectorAll("input:not([type=hidden]), textarea")].forEach((v) => {
            v.value = '';
            v.defaultValue = '';
        })
        action.querySelector("input[id*=__Id]").value = '-1';
        let actionHtml = action.innerHTML.replace(/_\d_/g, "_" + index + "_").replace(/\[\d\]/g, "[" + index + "]");
        $("#actions").append("<div id='" + type + "" + index + "' class='tabcontent rounded-bottom bg-white'><a href='#' class='delete float-right'><i class='fas fa-minus-circle text-danger'></i></a>" + actionHtml + '</div>');
        let btn = tc.querySelector(".tablinks[id=" + type + "]").cloneNode();
        btn.id += index;
        btnIndex = tc.querySelectorAll(".tablinks[id^=" + type + "]").length + 1;
        btn.textContent = "#" + btnIndex;
        btn.classList.remove("active-tab");
        evt.target.parentNode.insertBefore(btn, evt.target);
    }

    function open(id) {
        let tabcontent = [...tc.querySelectorAll(".tabcontent")];
        for (const t of tabcontent) {
            t.style.display = "none";
        }
        [...tc.querySelectorAll(".tablinks")].forEach((t) => {
            t.className = t.className.replace(" active-tab", "");
        });
        tc.querySelector(".tabcontent[id=" + id + "]").style.display = "block";
        tc.querySelector(".tablinks[id=" + id + "]").className += " active-tab";
    }
});
document.getElementById("cTabs")
    .addEventListener("click", (ev) => {
        console.log("click");
        if (ev.target && ev.target.classList.contains("tablinks")) {
            openCourse(ev.target.id);
        }
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


//document.getElementById("addAction").addEventListener("click", function () {
//    let index = document.getElementsByClassName("card").length;
//    let actionEl = document.getElementById("action").cloneNode(true);
//    $(actionEl).find(":input").val("");
//    let action = actionEl.innerHTML.replace(/_0_/g, "_" + index + "_").replace(/\[0\]/g, "[" + index + "]");
//    $("#actionsWrapper").append('<div class="card p-2"><div class="row"><div class="col-11 p-2">' + action + '</div><div class="align-self-center"><a href="#" class="delete"><i class="fas fa-trash-alt"></i></a></div></div></div>');
//    console.log("add");
//})
//$("#actionsWrapper").on("click", ".delete", function (e) {
//    e.preventDefault();
//    $(this).parent('div').parent('div').parent('div').remove();
//})
//$(function () {
//    $('#submit').on('click', function (evt) {
//        evt.preventDefault();
//        $.get('/Contacts/GetCompany', $('#search').serialize()).done(function (data) {
//            $.each($('#company_input input'), function () {
//                let index = $("#company_input input").index(this);
//                let input = this.getAttribute('name');
//                console.log(input);
//                $('#company_input input').eq(index).val(data[Object.keys(data)
//                    .find(k => k.toLowerCase() === input.toLowerCase())]);
//            });
//        });
//    });
//});