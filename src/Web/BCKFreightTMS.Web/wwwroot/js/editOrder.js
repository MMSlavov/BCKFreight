$(document).ready(GetData($("#OrderToCompanyId").val()));

[...document.querySelectorAll("#OrderToCompanyId option")].forEach((v) => {
    v.setAttribute("data-tokens", v.textContent.toLowerCase())
})

$(function () {$('.selectpicker').selectpicker();});

document.getElementById("OrderToCompanyId").addEventListener("change", onChange)
function onChange(ev) {
    GetData(ev.target.value);
}

function GetData(id) {
    $.getJSON("/Orders/GetContacts", { companyId: id }, function (d) {
        let row = "";
        $("#contactFrom").empty();
        $.each(d, function (i, v) {
            row += "<option value=" + v.value + ">" + v.text + "</option>";
        });
        $("#contactFrom").html(row);
        //let item = new Option("Select", null, true, true);
        //$(item).html("Select");
        //$("#contactFrom").append(item);
    })
    $.getJSON("/Orders/GetDrivers", { companyId: id }, function (d) {
        let row = "";
        $("#driver").empty();
        $.each(d, function (i, v) {
            row += "<option value=" + v.value + ">" + v.text + "</option>";
        });
        $("#driver").html(row);
        //let item = new Option("Select", null, true, true);
        //$(item).html("Select");
        //item.setAttribute("disabled", "disabled");
        //$("#driver").append(item);
    })
    $.getJSON("/Orders/GetVehicles", { companyId: id }, function (d) {
        let row = "";
        $("#vehicle").empty();
        $.each(d, function (i, v) {
            row += "<option value=" + v.value + ">" + v.text + "</option>";
        });
        $("#vehicle").html(row);
        //let item = new Option("Select", null, true, true);
        //$(item).html("Select");
        //item.setAttribute("disabled", "disabled");
        //$("#vehicle").append(item);
    })
}

document.getElementById("tabs")
    .addEventListener("click", (ev) => {
        if (ev.target && ev.target.classList.contains("tablinks")) {
            open(ev.target.id);
        }
        else if (ev.target && ev.target.id.includes("add")) {
            addAction(ev, ev.target.id.substring(3).toLowerCase());
        }
    });

$("#actions").on("click", ".delete", function (e) {
    e.preventDefault();
    open(this.parentNode.id.slice(0, -1));
    $("#tabs a[id=" + this.parentNode.id + "]").remove();
    $(this).parent('div').remove();
    [...document.getElementsByClassName("tabcontent")].forEach((t, i) => {
        t.innerHTML = t.innerHTML.replace(/_\d_/g, "_" + i + "_").replace(/\[\d\]/g, "[" + i + "]");
    });
    document.querySelectorAll(".tabcontent[id^=loading]").forEach((t, i) => {
        if (i != 0) {
            t.id = `loading${i + 1}`;
        }
    });
    document.querySelectorAll(".tabcontent[id^=unloading]").forEach((t, i) => {
        if (i != 0) {
            t.id = `unloading${i + 1}`;
        }
    });
    document.querySelectorAll(".tablinks[id^=loading]").forEach((t, i) => {
        if (i != 0) {
            t.id = `loading${i + 1}`;
            t.textContent = `#${i + 1}`;
        }
    });
    document.querySelectorAll(".tablinks[id^=unloading]").forEach((t, i) => {
        if (i != 0) {
            t.id = `unloading${i + 1}`;
            t.textContent = `#${i + 1}`;
        }
    });
});

function addAction(evt, type) {
    let index = document.getElementsByClassName("tabcontent").length;
    let action = document.querySelector(".tabcontent[id=" + type + "]").innerHTML.replace(/_\d_/g, "_" + index + "_").replace(/\[\d\]/g, "[" + index + "]");

    $("#actions").append("<div id='" + type + "" + index + "' class='tabcontent rounded-bottom bg-white'><a href='#' class='delete float-right'><i class='fas fa-minus-circle text-danger'></i></a>" + action + '</div>');
    let btn = document.querySelector(".tablinks[id=" + type + "]").cloneNode();
    btn.id += index;
    btnIndex = document.querySelectorAll(".tablinks[id^=" + type + "]").length + 1;
    btn.textContent = "#" + btnIndex;
    btn.classList.remove("active-tab");
    evt.target.parentNode.insertBefore(btn, evt.target);
}

function open(id) {
    let tabcontent = document.getElementsByClassName("tabcontent");
    for (const t of tabcontent) {
        t.style.display = "none";
    }
    [...document.getElementsByClassName("tablinks")].forEach((t) => {
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