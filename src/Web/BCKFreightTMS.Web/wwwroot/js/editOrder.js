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
        var row = "";
        $("#contactFrom").empty();
        $.each(d, function (i, v) {
            row += "<option value=" + v.value + ">" + v.text + "</option>";
        });
        $("#contactFrom").html(row);
        //var item = new Option("Select", null, true, true);
        //$(item).html("Select");
        //$("#contactFrom").append(item);
    })
    $.getJSON("/Orders/GetDrivers", { companyId: id }, function (d) {
        var row = "";
        $("#driver").empty();
        $.each(d, function (i, v) {
            row += "<option value=" + v.value + ">" + v.text + "</option>";
        });
        $("#driver").html(row);
        //var item = new Option("Select", null, true, true);
        //$(item).html("Select");
        //item.setAttribute("disabled", "disabled");
        //$("#driver").append(item);
    })
    $.getJSON("/Orders/GetVehicles", { companyId: id }, function (d) {
        var row = "";
        $("#vehicle").empty();
        $.each(d, function (i, v) {
            row += "<option value=" + v.value + ">" + v.text + "</option>";
        });
        $("#vehicle").html(row);
        //var item = new Option("Select", null, true, true);
        //$(item).html("Select");
        //item.setAttribute("disabled", "disabled");
        //$("#vehicle").append(item);
    })
}

document.getElementById("addAction").addEventListener("click", function () {
    var index = document.getElementsByClassName("card").length;
    var actionEl = document.getElementById("action").cloneNode(true);
    $(actionEl).find(":input").val("");
    var action = actionEl.innerHTML.replace(/_0_/g, "_" + index + "_").replace(/\[0\]/g, "[" + index + "]");
    $("#actionsWrapper").append('<div class="card p-2"><div class="row"><div class="col-11 p-2">' + action + '</div><div class="align-self-center"><a href="#" class="delete"><i class="fas fa-trash-alt"></i></a></div></div></div>');
    console.log("add");
})
$("#actionsWrapper").on("click", ".delete", function (e) {
    e.preventDefault();
    $(this).parent('div').parent('div').parent('div').remove();
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