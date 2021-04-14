import { ShowModal } from './addInvoiceRow.js';

let invoiceEndEl = document.querySelector("#invoiceEnd");
let finish = document.querySelector("#finish");
let vatEl = invoiceEndEl.querySelector("#vat");
let vatReasonEl = document.querySelector('.vatReason');

vatReasonEl.addEventListener("change", (ev) => {
    calculateTotal();
})

document.getElementById("yes").addEventListener("change", (ev) => {
    let parent = ev.target.parentNode.parentNode.parentNode;
    parent.nextSibling.nextSibling.style.display = "block";
    //parent.style.display = "none";
});

[...document.querySelectorAll("#docCheck a")].forEach((e) => e.addEventListener("click", (ev) => CheckDoc(ev.target.id)));
[...document.querySelectorAll("#approve a")].forEach((e) => e.addEventListener("click", (ev) => ApproveDoc(ev.target.id)));

function ApproveDoc(id) {
    let courseRow = document.getElementById(`row_${id}`);
    courseRow.classList.remove("table-danger");
    courseRow.classList.add("table-success");
    HideCurrentDocumentation();
}

function SetRows() {
    [...document.querySelectorAll("#rows tr")].forEach((r) => r.addEventListener("click", (ev) => {
        if (ev.target.classList.contains("delete")) {
            let id = ev.target.id;
            RemoveOrderTo(id);
            HideCurrentDocumentation();
            calculateTotal();
        }
        else {
            openDocumentation(ev.currentTarget.id.split("_").pop());
        }
    }))
}
SetRows();

document.querySelector("#addRow a").addEventListener("click", (ev) => ShowModal(ev.target.dataset.companyid, ev.target.dataset.title, CheckDoc, SetRows))

document.getElementById("confirmBtn").addEventListener("click", () => HideCurrentDocumentation());

document.getElementById("no").addEventListener("change", (ev) => {
    let parent = ev.target.parentNode.parentNode.parentNode.parentNode;
    ShowInvoiceEnd();
    parent.style.display = "none";
    ev.target.parentNode.parentNode.parentNode.nextSibling.nextSibling.style.display = "none";
})

function ShowInvoiceEnd() {
    calculateTotal();
    invoiceEndEl.style.display = "";
}

function calculateTotal() {
    let subTotal = [...document.getElementsByClassName("price")].reduce((sum, e) => { return sum + parseFloat(e.textContent.trim()) }, 0);
    invoiceEndEl.querySelector("#subTotal").textContent = subTotal.toFixed(2);

    let vat = 0;
    if (vatReasonEl.value == vatReasonEl.dataset.vatitemid) {
        vat = subTotal * 0.2;
    }
    vatEl.textContent = vat.toFixed(2);
    let total = subTotal + vat;
    invoiceEndEl.querySelector("#total").textContent = total.toFixed(2);
}

document.getElementById("showFinish").addEventListener("click", (ev) => {
    console.log(ev.target.parentNode);
    ShowFinish();
    ev.target.parentNode.style.display = "none";
})
function ShowFinish() {
    if (document.querySelectorAll(".table-danger").length > 0) {
        let finishBtn = finish.querySelector("#finishBtn");
        finishBtn.classList.remove("btn-success");
        finishBtn.classList.add("btn-warning");
        finishBtn.value = "Непълна документация";
    }
    finish.style.display = "";
}

function CheckDoc(id) {
    let reqDoc = $('#doc_' + id + ' #reqDoc label').map(function () { return this.textContent });
    let courseRow = document.getElementById(`row_${id}`);
    if ($('#doc_' + id + ' :checkbox:checked').length != reqDoc.length) {
        $('#modal').modal('show');
        courseRow.classList.add("table-danger");
        $('#doc_' + id + ' .solve').show();
        return;
    }
    let isValid = true
    $('#doc_' + id + ' :checkbox:checked').map(function () { return this.nextSibling }).each(function () {
        if (jQuery.inArray(this.textContent, reqDoc) == -1) {
            $('#modal').modal('show');
            courseRow.classList.add("table-danger");
            isValid = false;
            return;
        }
    })
    if (isValid) {
        courseRow.classList.remove("table-danger");
        courseRow.classList.add("table-success");
        $('#doc_' + id + ' .solve').hide();
        HideCurrentDocumentation();
    }
    else {
        $('#doc_' + id + ' .solve').show();
    }
}

function HideCurrentDocumentation() {
    [...document.querySelectorAll("#rows tr")].forEach((t) => {
        t.classList.remove("active-row");
    });
    let addRow = document.querySelector("#invoiceAddRow");
    if (document.querySelectorAll(".table-danger").length > 0) {
        addRow.querySelector("#no").setAttribute("disabled", "true");
    }
    else {
        addRow.querySelector("#no").removeAttribute("disabled");
    }
    document.querySelector("#docs").style.display = "none";
    addRow.style.display = "block";
    $('#modal').modal('hide');
}

document.getElementById("addBankBtn").addEventListener("click", (ev) => showInPopup("/Contacts/AddBankDetailsModal/" + ev.target.dataset.companyid, ev.target.dataset.title, SetBankDetailsModal));
function SetBankDetailsModal() {
    document.getElementById("BDForm").addEventListener("submit", (ev) => {
        ev.preventDefault();
        return jQueryAjaxPost(ev.target, RefreshBankDetails)
    })
}

function RefreshBankDetails(form) {
    $.getJSON("/Contacts/GetBankDetails", { companyId: form.dataset.companyid }, function (d) {
        let row = "";
        $("#InvoiceIn_BankDetailsId").empty();
        $.each(d, function (i, v) {
            row += "<option value=" + v.value + ">" + v.text + "</option>";
        });
        $("#InvoiceIn_BankDetailsId").html(row);
        //let item = new Option("Select", '', true, true);
        //$(item).html("Select");
        //item.setAttribute("disabled", "disabled");
        //$("#InvoiceIn_BankDetailsId").append(item);
    })
}


function openDocumentation(id) {
    document.getElementById("invoiceAddRow").style.display = "none";
    let docs = document.querySelector("#docs");
    docs.style.display = "";
    let docsEls = docs.querySelectorAll("div[id*=doc_]");
    for (const t of docsEls) {
        t.style.display = "none";
    }
    [...document.querySelectorAll("#rows tr")].forEach((t) => {
        t.classList.remove("active-row");
    });
    document.querySelector("#doc_" + id).style.display = "";
    document.querySelector("#row_" + id).className += " active-row";
}

//[...document.querySelectorAll(".solve")].forEach((e) => {
//    e.addEventListener("click", (ev) => {
//        ev.target.style.display = "none";
//        ev.target.nextSibling.nextSibling.style.display = "";
//    })
//});

export { CheckDoc, openDocumentation };
