$(function () {
    $("#CompanyFromId").change(function () {
        RefreshContact();
    })
    $('#form-modal').on('hidden.bs.modal', function () {
        RefreshContact();
    });
    function RefreshContact() {
        $.getJSON("/Orders/GetContacts", { companyId: $("#CompanyFromId").val() }, function (d) {
            var row = "";
            $("#contactFrom").empty();
            $.each(d, function (i, v) {
                row += "<option value=" + v.value + ">" + v.text + "</option>";
            });
            $("#contactFrom").html(row);
            var item = new Option("Select", null, true, true);
            $(item).html("Select");
            $("#contactFrom").append(item);
        })
    }
})

document.getElementById("tabs")
    .addEventListener("click", (ev) => {
        if (ev.target && ev.target.classList.contains("tablinks")) {
            open(ev, ev.target.id);
        }
        else if (ev.target && ev.target.id.includes("add")) {
            addAction(ev, ev.target.id.substring(3).toLowerCase());
        }
    });

function addAction(evt, type) {
    let index = document.getElementsByClassName("tabcontent").length;
    let action = document.querySelector(".tabcontent[id=" + type + "]").innerHTML.replace(/_\d_/g, "_" + index + "_").replace(/\[\d\]/g, "[" + index + "]");

    $("#actions").append("<div id='" + type + "" + index + "' class='tabcontent rounded-bottom'>" + action + '</div>');
    let btn = document.querySelector(".tablinks[id=" + type + "]").cloneNode();
    btn.id += index;
    btnIndex = document.querySelectorAll(".tablinks[id^=" + type + "]").length + 1;
    btn.textContent = "#" + btnIndex;
    btn.classList.remove("active-tab");
    evt.target.parentNode.insertBefore(btn, evt.target);
}

function open(evt, id) {
    let tabcontent = document.getElementsByClassName("tabcontent");
    for (const t of tabcontent) {
        t.style.display = "none";
    }
    [...document.getElementsByClassName("tablinks")].forEach((t) => {
        t.className = t.className.replace(" active-tab", "");
    });
    document.querySelector(".tabcontent[id=" + id + "]").style.display = "block";
    evt.target.className += " active-tab";
}

        //document.getElementById("addAction").addEventListener("click", function () {
        //    var index = document.getElementsByClassName("card").length;
        //    var action = document.getElementById("action").innerHTML.replace(/_0_/g, "_" + index + "_").replace(/\[0\]/g, "[" + index + "]");

        //    $("#actionsWrapper").append('<div class="card p-2"><div class="row"><div class="col-11 p-2">' + action + '</div><div class="align-self-center"><a href="#" class="delete"><i class="fas fa-trash-alt"></i></a></div></div></div>');
        //    console.log("add");
        //})
        //$("#actionsWrapper").on("click", ".delete", function (e) {
        //    e.preventDefault();
        //    $(this).parent('div').parent('div').parent('div').remove();
        //})