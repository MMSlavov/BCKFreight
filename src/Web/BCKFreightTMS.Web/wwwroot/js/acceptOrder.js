$(document).ready(function () {
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

let companyId = document.getElementById("CompanyFromId");
let addContactBtn = document.getElementById("addContact");
addContactBtn.addEventListener("click", function (ev) {
    showInPopup(`/Contacts/AddPersonModal/${companyId.value}?role=Contact`, 'Add contact');
});
 
$(function () {
    $("#CompanyFromId").change(function () {
        refreshContact();
        addContactBtn.style.display = "";
    });
    $('#form-modal').on('hidden.bs.modal', function () {
        refreshContact();
        $('.selectpicker').selectpicker('refresh');
    });
    function refreshContact() {
        $.getJSON("/Orders/GetContacts", { companyId: $("#CompanyFromId").val() }, function (d) {
            let row = "";
            $("#contactFrom").empty();
            $.each(d, function (i, v) {
                row += "<option value=" + v.value + ">" + v.text + "</option>";
            });
            $("#contactFrom").html(row);
            let item = new Option("Select", null, true, true);
            $(item).html("Select");
            item.setAttribute("disabled", "disabled");
            $("#contactFrom").append(item);
        })
    }
})
function SetActions() {
    let aTabs = document.querySelectorAll("#aTabs");
    aTabs[aTabs.length - 1].addEventListener("click", (ev) => {
            if (ev.target && ev.target.classList.contains("tablinks")) {
                openAction(ev.target.id, ev.target.parentNode.parentNode.parentNode.id);
            }
            else if (ev.target && ev.target.id.includes("add")) {
                ev.preventDefault();
                addAction(ev, ev.target.id.substring(3).toLowerCase());
            }
        });
}
SetActions()

document.getElementById("cTabs")
        .addEventListener("click", (ev) => {
            if (ev.target && ev.target.classList.contains("tablinks")) {
                openCourse(ev.target.id);
            }
            else if (ev.target && ev.target.id.includes("add")) {
                ev.preventDefault();
                addCourse(ev, ev.target.id.substring(3).toLowerCase());
                //SetDeleteCourses();
                SetActions();
                SetDeleteActions();
            }
        });

function SetDeleteActions() {
    $(".actions").on("click", ".delete", function (e) {
        e.preventDefault();
        let course = e.target.parentNode.parentNode.parentNode.parentNode;
        openAction(this.parentNode.id.slice(0, -1), course.id);
        $("#" + course.id + " #aTabs a[id=" + this.parentNode.id + "]").remove();
        $(this).parent('div').remove();
        let tabContIndex = 0;
        [...course.getElementsByClassName("tabcontent")].forEach((t) => {
            t.innerHTML = t.innerHTML.replace(/OrderActions_\d_/g, "OrderActions_" + tabContIndex + "_")
                                    .replace(/OrderActions\[\d\]/g, "OrderActions[" + tabContIndex + "]");
            tabContIndex++;
        });
        course.querySelectorAll(".tabcontent[id^=loading]").forEach((t, i) => {
            if (i != 0) {
                t.id = `loading${i + 1}`;
            }
        });
        course.querySelectorAll(".tabcontent[id^=unloading]").forEach((t, i) => {
            if (i != 0) {
                t.id = `unloading${i + 1}`;
            }
        });
        course.querySelectorAll(".tablinks[id^=loading]").forEach((t, i) => {
            if (i != 0) {
                t.id = `loading${i + 1}`;
                t.textContent = `#${i + 1}`;
            }
        });
        course.querySelectorAll(".tablinks[id^=unloading]").forEach((t, i) => {
            if (i != 0) {
                t.id = `unloading${i + 1}`;
                t.textContent = `#${i + 1}`;
            }
        });
    });
}
SetDeleteActions();

//function SetDeleteCourses() {
//    $("#courses").on("click", ".delete", function (e) {
//        e.preventDefault();
//        let course = e.target.parentNode.parentNode;
//        console.log(course.id);
//        openCourse(course.id.slice(0, -1));
//        $("#cTabs a[id=" + course.id + "]").remove();
//        $(this).parent('div').remove();
//        //[...document.querySelectorAll(".tabcontent[id*=course]")].forEach((t, i) => {
//        //    t.innerHTML = t.innerHTML.replace(/OrderTos_\d_/g, `OrderTos_${i}_`)
//        //                            .replace(/OrderTos\[\d\]/g, `OrderTos[${i}]`);
//        //    if (i != 0) {
//        //        t.id = `course${i}`;
//        //    }
//        //});
//        document.querySelectorAll(".tablinks[id*=course]").forEach((t, i) => {
//            if (i != 0) {
//                t.id = `course${i}`;
//                t.textContent = `#${i + 1}`;
//            }
//        });
//        SetActions();
//        SetDeleteActions();
//    });
//}
//SetDeleteCourses();

function addAction(evt, type) {
    let course = document.querySelector(".tabcontent[id=" + evt.target.parentNode.parentNode.parentNode.id + "]");
    let index = course.getElementsByClassName("tabcontent").length;
    let action = course.querySelector(".tabcontent[id=" + type + "]").cloneNode(true);
    [...action.querySelectorAll("input:not([type=hidden]), textarea")].forEach((v) => {
        v.value = '';
        v.defaultValue = '';
    })
    //action.querySelector("input[id*=__Id]").value = '-1';
    let actionHtml = action.innerHTML.replace(/OrderActions_\d_/g, "OrderActions_" + index + "_")
                                    .replace(/OrderActions\[\d\]/g, "OrderActions[" + index + "]");

    $("#" + course.id + " .actions").append("<div id='" + type + "" + index + "' class='tabcontent rounded-bottom bg-white'><a href='javascript:' class='delete float-right'><i class='fas fa-minus-circle text-danger'></i></a>" + actionHtml + '</div>');
    let btn = course.querySelector(".tablinks[id=" + type + "]").cloneNode();
    btn.id += index;
    btnIndex = course.querySelectorAll(".tablinks[id^=" + type + "]").length + 1;
    btn.textContent = "#" + btnIndex;
    btn.classList.remove("active-tab");
    evt.target.parentNode.insertBefore(btn, evt.target);
}

function addCourse(evt) {
    let index = document.querySelectorAll(".tabcontent[id^=course]").length;
    let course = document.querySelector(".tabcontent[id=course]").cloneNode(true);
    [...course.querySelectorAll("input:not([type=checkbox],[type=hidden]), textarea")].forEach((v) => {
        v.value = '';
        v.defaultValue = '';
    });
    [...course.querySelectorAll(".cargoSpec")].forEach((v) => {
        v.value = 0;
        v.defaultValue = 0;
    })
    let courseHtml = course.innerHTML.replace(/OrderTos_\d_/g, "OrderTos_" + index + "_").replace(/OrderTos\[\d\]/g, "OrderTos[" + index + "]");

    $("#courses").append("<div id='course" + index + "' class='tabcontent rounded-bottom'>" + courseHtml + '</div>');/*<a href='#' class='delete float-right'><i class='fas fa-minus-circle text-danger'></i></a>*/
    let btn = document.querySelector(".tablinks[id=course]").cloneNode();
    btn.id += index;
    btnIndex = document.querySelectorAll(".tablinks[id^=course]").length + 1;
    btn.textContent = "#" + btnIndex;
    btn.classList.remove("active-tab");
    evt.target.parentNode.insertBefore(btn, evt.target);
}

function openAction(id, courseId) {
    let course = document.querySelector(".tabcontent[id=" + courseId + "]");
    let tabcontent = course.querySelectorAll(".tabcontent[id*=loading]");
    for (const t of tabcontent) {
        t.style.display = "none";
    }
    [...course.querySelectorAll(".tablinks[id*=loading]")].forEach((t) => {
        t.className = t.className.replace(" active-tab", "");
    });
    course.querySelector(".tabcontent[id=" + id + "]").style.display = "block";
    course.querySelector(".tablinks[id=" + id + "]").className += " active-tab";
}

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